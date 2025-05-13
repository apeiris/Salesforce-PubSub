using System.Data;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using JsonSerializer = System.Text.Json.JsonSerializer;
namespace NetUtils {
	public class SalesforceService : ISalesforceService {
		private readonly HttpClient _httpClient;
		private readonly SalesforceConfig _settings;
		private readonly ILogger<SalesforceService> _logger;
		private static readonly object _tokenLock = new object();
		private static string _cachedToken;
		private static string _cachedInstanceUrl;
		private static DateTimeOffset? _tokenExpiresAt;
		public event EventHandler<AuthenticationEventArgs> AuthenticationAttempt;
		public SalesforceService(HttpClient httpClient, IOptions<SalesforceConfig> settings, ILogger<SalesforceService> logger) {
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_logger.LogDebug("SalesforceService initialized with settings: {Settings}", _settings);
		}
		public async Task<JsonElement> GetObjectSchemaAsync(string objectName, CancellationToken cancellationToken = default) {
			if (string.IsNullOrWhiteSpace(objectName)) {
				throw new ArgumentException("Object name cannot be empty or null", nameof(objectName));
			}




			var (token, instanceUrl, expiresAt) = await GetAccessTokenAsync();





			// Construct URL
			string apiVersion = string.IsNullOrEmpty(_settings.ApiVersion) ? "63.0" : _settings.ApiVersion;
			string url = $"{instanceUrl}/services/data/v{apiVersion}/sobjects/{Uri.EscapeDataString(objectName)}/describe/";
			_logger.LogDebug("Fetching schema for {ObjectName} from {Url}", objectName, url);

			// Set up request
			using var request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Headers.Add("Authorization", $"Bearer {token}");
			request.Headers.Add("Accept", "application/json");

			try {
				using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
				if (!response.IsSuccessStatusCode) {
					string errorContent = await response.Content.ReadAsStringAsync();
					_logger.LogError("API request failed for {ObjectName}: Status={StatusCode}, Content={Content}",
						objectName, response.StatusCode, errorContent);
					throw new Exception($"Failed to retrieve schema for {objectName}: {response.ReasonPhrase}");
				}

				await using var stream = await response.Content.ReadAsStreamAsync();
				JsonElement schema;
				try {
					schema = await JsonSerializer.DeserializeAsync<JsonElement>(stream, cancellationToken: cancellationToken);
				} catch (JsonException jsonEx) {
					_logger.LogError("Failed to deserialize schema for {ObjectName}: {Message}", objectName, jsonEx.Message);
					throw new Exception($"Invalid JSON response for {objectName}", jsonEx);
				}

				// Validate schema
				if (!schema.TryGetProperty("fields", out _) || !schema.TryGetProperty("childRelationships", out _)) {
					_logger.LogError("Schema for {ObjectName} missing required properties", objectName);
					throw new Exception($"Invalid schema for {objectName}: missing fields or childRelationships");
				}

				_logger.LogDebug("Successfully retrieved schema for {ObjectName}", objectName);
				return schema;
			} catch (HttpRequestException httpEx) {
				_logger.LogError("HTTP error retrieving schema for {ObjectName}: {Message}", objectName, httpEx.Message);
				throw new Exception($"Failed to retrieve schema for {objectName}: {httpEx.Message}", httpEx);
			} catch (TaskCanceledException timeoutEx) {
				_logger.LogError("Request timed out for {ObjectName}: {Message}", objectName, timeoutEx.Message);
				throw new Exception($"Schema request for {objectName} timed out", timeoutEx);
			} catch (Exception ex) {
				_logger.LogError("Unexpected error retrieving schema for {ObjectName}: {Message}", objectName, ex.Message);
				throw;
			}
		}
		private AccessTokenCache _tokenCache;
		public async Task<(string token, string instanceUrl, string tenantId)> GetAccessTokenAsync() {
			if (_tokenCache != null && _tokenCache.Expiry > DateTime.UtcNow.AddMinutes(5)) {// Check if cache exists and is not expired, with 5-minute buffer to account for call processing time
				return (_tokenCache.Token, _tokenCache.InstanceUrl, _tokenCache.TenantId);
			}
			var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.LoginUrl}/services/oauth2/token") {
				Content = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{ "grant_type", "password" },
					{ "client_id", _settings.ClientId },
					{ "client_secret", _settings.ClientSecret },
					{ "username", _settings.Username },
					{ "password", _settings.Password }
				})
			};

			var response = await _httpClient.SendAsync(request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode) {
				RaiseAuthenticationAttempt(LogLevel.Error, $"Authentication failed: {responseContent}");
				return (null, null, null);
			}

			var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
			var tenantId = data!["id"].Split('/')[^2];
			_tokenCache = new AccessTokenCache {// Cache the token with 1 hour expiry, minus 5-minute buffer
				Token = data["access_token"],
				InstanceUrl = data["instance_url"],
				TenantId = tenantId,
				Expiry = DateTime.UtcNow.AddMinutes(115) // 2 hour expiry with 5-minute buffer
			};
			RaiseAuthenticationAttempt(LogLevel.Information, $"Authenticated {responseContent}");
			return (_tokenCache.Token, _tokenCache.InstanceUrl, _tokenCache.TenantId);
		}
		private void RaiseAuthenticationAttempt(LogLevel ll, string message, string instanceUrl = null,
		[CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0) {
			string detailedMessage = $"{message} [Method: {callerMemberName}, Line: {callerLineNumber}]";
			AuthenticationAttempt?.Invoke(this, new AuthenticationEventArgs(ll, detailedMessage, instanceUrl));
		}
		public async Task<DataTable> GetAllObjects() {
			var (token, instanceUrl, tenantId) = await GetAccessTokenAsync();
			string url = $"{instanceUrl}/services/data/v{_settings.ApiVersion}/sobjects";
			using var request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Headers.Add("Authorization", $"Bearer {token}");
			request.Headers.Add("Accept", "application/json");

			try {
				HttpResponseMessage response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();
				string jsonResponse = await response.Content.ReadAsStringAsync();
				using var jdoc = JsonDocument.Parse(jsonResponse);

				var objects = jdoc.RootElement
					.GetProperty("sobjects")
					.EnumerateArray()
					.Where(obj => {
						string name = obj.GetProperty("name").GetString();
						return name != "Name";
					})
					.Select(obj => new { Name = obj.GetProperty("name").GetString() });
				var root = new XElement("Objects",
					objects.Select(f => new XElement("Object",
						new XElement("Name", f.Name)
					))
				);
				var dataSet = new DataSet();
				using (var reader = root.CreateReader()) {
					dataSet.ReadXml(reader);
				}

				if (dataSet.Tables.Count > 0) {
					dataSet.Tables[0].TableName = "sobjects";
					return dataSet.Tables[0];
				} else {
					throw new Exception("No table created from XML.");
				}
			} catch (HttpRequestException ex) {
				throw new Exception($"Failed to retrieve schema from {url}: {ex.Message}", ex);
			} catch (JsonException ex) {
				throw new Exception($"Failed to parse JSON response:\n{ex.Message}", ex);
			} catch (Exception ex) {
				throw new Exception($"Unexpected error: {ex.Message}", ex);
			}
		}
		public async Task<string> GetObjectSchemaSummaryAsync(string objectName) {
			JsonElement schema = await GetObjectSchemaAsync(objectName);
			var sb = new System.Text.StringBuilder();
			sb.AppendLine($"Object: {schema.GetProperty("name").GetString()}");
			sb.AppendLine($"Label: {schema.GetProperty("label").GetString()}");
			sb.AppendLine("Fields:");
			// Iterate through fields
			foreach (JsonElement field in schema.GetProperty("fields").EnumerateArray()) {
				string fieldName = field.GetProperty("name").GetString();
				string fieldType = field.GetProperty("type").GetString();
				string fieldLabel = field.GetProperty("label").GetString();
				string fieldLength = field.GetProperty("length").GetUInt32().ToString();
				bool isCustom = field.GetProperty("custom").GetBoolean();
				sb.AppendLine($"- {fieldName} ({fieldType}): {fieldLabel}:{fieldLength}: {(isCustom ? "[Custom]" : "")}");
			}
			return sb.ToString();
		}
		public async Task<DataSet> GetObjectSchemaAsDataSetAsync(string objectName) {
			var schemax = await GetObjectSchemaAsync(objectName);
			JsonDocument schema = JsonDocument.Parse(schemax.GetRawText());
			DataSet ds = new DataSet();
			try {
				var fields = schema.RootElement
			   .GetProperty("fields")
			   .EnumerateArray()
			   .Select(field => new {
				   Name = field.GetProperty("name").GetString(),
				   Type = field.GetProperty("type").GetString(),
				   Label = field.GetProperty("label").GetString(),
				   Length = field.GetProperty("length").GetUInt32(),
				   Nullable = field.GetProperty("nillable").GetBoolean(),
				   relationshipName = field.GetProperty("relationshipName").GetString(),
				   referenceTo = field.TryGetProperty("referenceTo", out var refProp) && refProp.ValueKind == JsonValueKind.Array ? refProp.EnumerateArray().Select(e => e.GetString()).ToList() : new List<string>()
			   });
				var childRelationNames = schema.RootElement
							.GetProperty("childRelationships")
							.EnumerateArray()
							.Where(rel => rel.TryGetProperty("relationshipName", out var prop) && prop.ValueKind == JsonValueKind.String)
							.Select(rel => new {
								Name = rel.GetProperty("relationshipName").GetString(),
								Child = rel.GetProperty("childSObject").GetString(),
								field = rel.GetProperty("field").GetString(),
								Cascade = rel.GetProperty("cascadeDelete").GetBoolean()
							});
				XElement schemaRoot = new XElement(objectName,
						fields.Select(f => new XElement(objectName,
							new XElement("Name", f.Name ?? ""),
							new XElement("Type", f.Type ?? ""),
							new XElement("Label", f.Label ?? ""),
							new XElement("Length", f.Length),
							new XElement("Nullable", f.Nullable),
							new XElement("relationshipName", f.relationshipName ?? ""),
							new XElement("referenceTo", string.Join(",", f.referenceTo))
						)
					),
				childRelationNames.Select(cr => new XElement($"relations",
								new XElement("Name", cr.Name ?? ""),
								new XElement("ChildSObject", cr.Child ?? ""),
								new XElement("Field", cr.field ?? ""),
								new XElement("Cascade", cr.Cascade)
							)));
				using (XmlReader xr = schemaRoot.CreateReader()) ds.ReadXml(xr);
			} catch (Exception ex) {
				_logger.LogError(ex.Message);
				_logger.LogError(ex.StackTrace);
				return null;
			}
			return ds;
		}
		//====================================================================================
		public async Task<DataTable> GetSalesforceRecord(string objectName, string recordId) {
			try {
				// Get access token and instance URL
				var (token, instanceUrl, tenantId) = await GetAccessTokenAsync();

				// Construct the REST API endpoint for the record
				string endpoint = $"{instanceUrl}/services/data/v{_settings.ApiVersion}/sobjects/{objectName}/{recordId}";

				// Set up the HTTP request
				_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
				var response = await _httpClient.GetAsync(endpoint);

				if (!response.IsSuccessStatusCode) {
					throw new Exception($"Failed to retrieve data: {response.StatusCode}");
				}

				// Parse JSON response
				string jsonResponse = await response.Content.ReadAsStringAsync();
				using var jsonDoc = JsonDocument.Parse(jsonResponse);

				// Project JSON properties to a dictionary (avoiding foreach)
				var fields = jsonDoc.RootElement.EnumerateObject()
					.Where(prop => prop.Name != "attributes") // Exclude metadata
					.Select(prop => new KeyValuePair<string, string>(prop.Name, prop.Value.ToString()))
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

				// Convert to XML
				var xmlElement = new XElement("Record",
					fields.Select(kvp => new XElement(kvp.Key, kvp.Value))
				);
				string xmlString = xmlElement.ToString();

				// Load XML into DataTable
				DataTable dataTable = new DataTable(objectName);
				using (var xmlReader = new System.IO.StringReader(xmlString)) {
					var xdoc = XDocument.Parse(xmlString);
					var columns = xdoc.Descendants("Record").Elements()
						.Select(e => e.Name.LocalName)
						.Distinct();

					// Create columns in DataTable
					columns.ToList().ForEach(col => dataTable.Columns.Add(col));

					// Add row to DataTable
					var row = dataTable.NewRow();
					fields.ToList().ForEach(kvp => row[kvp.Key] = kvp.Value);
					dataTable.Rows.Add(row);
				}

				return dataTable;
			} catch (Exception ex) {
				throw new Exception($"Error retrieving Salesforce data: {ex.Message}", ex);
			}
		}
		//====================================================================================
		public async Task<DataTable> GetCDCSubscriptions() {
			var (token, instanceUrl, tenantId) = await GetAccessTokenAsync();
			string url = $"{instanceUrl}/services/data/v{_settings.ApiVersion}/sobjects/EventBusSubscriber";
			using var request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Headers.Add("Authorization", $"Bearer {token}");
			request.Headers.Add("Accept", "application/json");
			//	var response = await _httpClient.SendAsync(request);
			try {
				HttpResponseMessage response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();
				string jsonResponse = await response.Content.ReadAsStringAsync();
				using var jdoc = JsonDocument.Parse(jsonResponse);

				var objects = jdoc.RootElement
					.GetProperty("sobjects")
					.EnumerateArray()
					.Where(obj => {
						string name = obj.GetProperty("name").GetString();
						return name != "Name";
					})
					.Select(obj => new { Name = obj.GetProperty("name").GetString() });
				
			} catch (HttpRequestException ex) {

				throw new Exception($"Failed to retrieve subscriptions from {url}: {ex.Message}", ex);
			}
			return null;

		}
		//====================================================================================
		#region helpers
		public class AuthenticationEventArgs : EventArgs {
			public LogLevel LogLevel { get; }
			public string Message { get; }
			public string InstanceUrl { get; }

			public AuthenticationEventArgs(LogLevel ll, string message, string instanceUrl = null) {
				//_logger.Logle
				LogLevel = ll;
				Message = message;
				InstanceUrl = instanceUrl;
			}
		}
		public class SchemaFetchedEventArgs : EventArgs {
			public string ObjectName { get; }
			public DataTable Schema { get; }
			public bool Success { get; }
			public string ErrorMessage { get; }

			public SchemaFetchedEventArgs(string objectName, DataTable schema, bool success, string errorMessage = null) {
				ObjectName = objectName;
				Schema = schema;
				Success = success;
				ErrorMessage = errorMessage;
			}
		}
		#endregion	helpers
		// ===================================================================================
		public class TokenResponse {
			public string access_token { get; set; }
			public string instance_url { get; set; }
			public string id { get; set; }
		}
		public class AccessTokenCache {
			public string Token { get; set; }
			public string InstanceUrl { get; set; }
			public string TenantId { get; set; }
			public DateTime Expiry { get; set; }
		}
	}
}