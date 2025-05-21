using System.Buffers.Text;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
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

		public class FieldMeta {
			public string Name { get; set; }
			public string Type { get; set; }
			public string Label { get; set; }
			public uint Length { get; set; }
			public bool Nullable { get; set; }
			public string DefaultValue { get; set; }
			public string RelationshipName { get; set; }
			public List<string> ReferenceTo { get; set; }
		}


	
		public async Task<JsonElement> GetPlatformEventChannel(CancellationToken cancellationToken = default) {
			var (token, instanceUrl, expiresAt) = await GetAccessTokenAsync();

			string objectName = "PlatformEventChannel";
			string url = $"{instanceUrl}/services/data/v{_settings.ApiVersion}/tooling/sobjects/{objectName}";
			_logger.LogDebug("Posting to {ObjectName} at {Url}", objectName, url);

			using var request = new HttpRequestMessage(HttpMethod.Post, url);
			request.Headers.Add("Authorization", $"Bearer {token}");
			request.Headers.Add("Accept", "application/json");

			// If needed, set Content-Type and a body (example: empty JSON object)
			request.Content = new StringContent("{}", Encoding.UTF8, "application/json");

			try {
				using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
				if (!response.IsSuccessStatusCode) {
					string errorContent = await response.Content.ReadAsStringAsync();
					_logger.LogError("API POST failed for {ObjectName}: Status={StatusCode}, Content={Content}",
						objectName, response.StatusCode, errorContent);
					throw new Exception($"Failed to POST to {objectName}: {response.ReasonPhrase}");
				}

				await using var stream = await response.Content.ReadAsStreamAsync();
				JsonElement result = await JsonSerializer.DeserializeAsync<JsonElement>(stream, cancellationToken: cancellationToken);

				_logger.LogDebug("Successfully POSTed to {ObjectName}", objectName);
				return result;
			} catch (Exception ex) {
				_logger.LogError("Error during POST for {ObjectName}: {Message}", objectName, ex.Message);
				throw;
			}
		}


		public async Task<JsonElement> GetObjectSchemaAsync(string objectName, CancellationToken cancellationToken = default) {
			if (string.IsNullOrWhiteSpace(objectName)) {
				throw new ArgumentException("Object name cannot be empty or null", nameof(objectName));
			}
			var (token, instanceUrl, expiresAt) = await GetAccessTokenAsync();
			string apiVersion = string.IsNullOrEmpty(_settings.ApiVersion) ? "63.0" : _settings.ApiVersion;
			string url = $"{instanceUrl}/services/data/v{apiVersion}/sobjects/{Uri.EscapeDataString(objectName)}/describe/";
			_logger.LogDebug("Fetching schema for {ObjectName} from {Url}", objectName, url);
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
				using (var reader = root.CreateReader()) dataSet.ReadXml(reader);
				if (dataSet.Tables.Count > 0) {
					dataSet.Tables[0].TableName = "sobjects";
					return dataSet.Tables[0];
				} else 	throw new Exception("No table created from XML.");
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
			foreach (JsonElement field in schema.GetProperty("fields").EnumerateArray()) {	// Iterate through fields
				string fieldName = field.GetProperty("name").GetString();
				string fieldType = field.GetProperty("type").GetString();
				string fieldLabel = field.GetProperty("label").GetString();
				string fieldLength = field.GetProperty("length").GetUInt32().ToString();
				bool isCustom = field.GetProperty("custom").GetBoolean();
				sb.AppendLine($"- {fieldName} ({fieldType}): {fieldLabel}:{fieldLength}: {(isCustom ? "[Custom]" : "")}");
			}
			return sb.ToString();
		}
		public static List<FieldMeta> GetNonNullableFields(JsonElement schema) {
			try {
				return schema
					.GetProperty("fields")
					.EnumerateArray()
					//.Where(field => !field.GetProperty("nillable").GetBoolean()) // Only non-nullable fields
					.Select(field => {
						// Safely handle defaultValue
						string defaultValue = null;
						if (field.TryGetProperty("defaultValue", out var defaultProp)) {
							switch (defaultProp.ValueKind) {
								case JsonValueKind.String:
								defaultValue = $"'{defaultProp.GetString()}'";
								break;
								case JsonValueKind.Number:
								defaultValue = defaultProp.GetRawText();
								break;
								case JsonValueKind.True:
								case JsonValueKind.False:
								//defaultValue = defaultProp.GetBoolean().ToString().ToLower();
								defaultValue = defaultProp.GetBoolean() ? "1" : "0";// so this match the T-Sql
								break;
								case JsonValueKind.Null:
								defaultValue = null; // or "No default value" if preferred
								break;
								default:
								defaultValue = defaultProp.GetRawText(); // Fallback for unexpected types
								break;
							}
						}

						return new FieldMeta {
							Name = field.GetProperty("name").GetString(),
							Type = field.GetProperty("type").GetString(),
							Label = field.GetProperty("label").GetString(),
							Length = field.GetProperty("length").GetUInt32(),
							Nullable = field.GetProperty("nillable").GetBoolean(),
							DefaultValue = defaultValue,
							RelationshipName = field.TryGetProperty("relationshipName", out var relProp) && relProp.ValueKind != JsonValueKind.Null
								? relProp.GetString()
								: null,
							ReferenceTo = field.TryGetProperty("referenceTo", out var refProp) && refProp.ValueKind == JsonValueKind.Array
								? refProp.EnumerateArray().Select(e => e.GetString()).ToList()
								: new List<string>()
						};
					})
					.ToList();
			} catch (Exception ex) {
				throw new InvalidOperationException($"Error processing Salesforce describe schema: {ex.Message}", ex);
			}
		}
		public async Task<DataSet> GetObjectSchemaAsDataSetAsync(string objectName,bool useTooling=false) {
			var schemax = await GetObjectSchemaAsync(objectName);
			JsonDocument schema = JsonDocument.Parse(schemax.GetRawText());
			DataSet ds = new DataSet();
			try {
				var fields = GetNonNullableFields(schema.RootElement);
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
							new XElement("Default",f.DefaultValue),
							new XElement("relationshipName", f.RelationshipName ?? ""),
							new XElement("referenceTo", string.Join(",", f.ReferenceTo))
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
				var (token, instanceUrl, tenantId) = await GetAccessTokenAsync();// Get access token and instance URL
				string endpoint = $"{instanceUrl}/services/data/v{_settings.ApiVersion}/sobjects/{objectName}/{recordId}";// Construct the REST API endpoint for the record
				_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);   // Set up the HTTP request
				var response = await _httpClient.GetAsync(endpoint);
				if (!response.IsSuccessStatusCode) throw new Exception($"Failed to retrieve data: {response.StatusCode}");
				string jsonResponse = await response.Content.ReadAsStringAsync();
				using var jsonDoc = JsonDocument.Parse(jsonResponse);
				var fields = jsonDoc.RootElement.EnumerateObject()// Project JSON properties to a dictionary (avoiding foreach)
					.Where(prop => prop.Name != "attributes") // Exclude metadata
					.Select(prop => new KeyValuePair<string, string>(prop.Name, prop.Value.ToString()))
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
				var xmlElement = new XElement("Record", fields.Select(kvp => new XElement(kvp.Key, kvp.Value)));
				string xmlString = xmlElement.ToString();
				DataTable dataTable = new DataTable(objectName);// Load XML into DataTable
				using (var xmlReader = new System.IO.StringReader(xmlString)) {
					var xdoc = XDocument.Parse(xmlString);
					var columns = xdoc.Descendants("Record").Elements()
						.Select(e => e.Name.LocalName)
						.Distinct();
					columns.ToList().ForEach(col => dataTable.Columns.Add(col));// Create columns in DataTable
					var row = dataTable.NewRow();// Add row to DataTable
					fields.ToList().ForEach(kvp => row[kvp.Key] = kvp.Value);
					dataTable.Rows.Add(row);
				}
				return dataTable;
			} catch (Exception ex) {
				throw new Exception($"Error retrieving Salesforce data: {ex.Message}", ex);
			}
		}
		//====================================================================================
		
		//=========19-05-2025-1===============================================================
		public async Task CheckDailyChangeEventsAsync() {
			var (token, instanceUrl, tenantId) = await GetAccessTokenAsync();

			var client = new RestClient($"{instanceUrl}/services/data/{_settings.ApiVersion}");
			var request = new RestRequest("limits", Method.Get);
			request.AddHeader("Authorization", $"Bearer {token}");

			var response = await client.ExecuteAsync(request);
			if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content)) {
				throw new Exception($"Failed to retrieve limits: {response.StatusCode} - {response.Content}");
			}

			using var jsonDoc = JsonDocument.Parse(response.Content);
			Console.WriteLine("\nDaily Change Events Limit:");
			if (jsonDoc.RootElement.TryGetProperty("DailyChangeEvents", out var dailyEvents)) {
				int max = dailyEvents.GetProperty("Max").GetInt32();
				int remaining = dailyEvents.GetProperty("Remaining").GetInt32();
				int used = max - remaining;
				Console.WriteLine($"Max: {max} events per 24 hours");
				Console.WriteLine($"Used: {used} events");
				Console.WriteLine($"Remaining: {remaining} events");
				Console.WriteLine($"Percentage Used: {(double)used / max * 100:F2}%");
				if (remaining < max * 0.2) // Warn if less than 20% remaining
				{
					Console.WriteLine("Warning: Low remaining events. Consider disabling CDC for low-priority objects or contacting Salesforce to increase limits.");
				}
			} else {
				Console.WriteLine("DailyChangeEvents limit not found. Your org may not support CDC, or the API version does not expose this limit.");
				Console.WriteLine("Contact Salesforce Support to verify CDC limits for your org.");
			}
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