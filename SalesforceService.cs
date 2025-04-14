using System.Data;
using System.Security.Cryptography.Xml;
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
		public SalesforceService(HttpClient httpClient, IOptions<SalesforceConfig> settings, ILogger<SalesforceService> logger) {
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_logger.LogDebug("SalesforceService initialized with settings: {Settings}", _settings);
		}
		public async Task<string> AuthenticateAsync() {
			var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.LoginUrl}/services/oauth2/token");
			var parameters = new Dictionary<string, string>
			{
				{ "grant_type", "password" },
				{ "client_id", _settings.ClientId },
				{ "client_secret", _settings.ClientSecret },
				{ "username", _settings.Username },
				{ "password", _settings.Password }
			};
			request.Content = new FormUrlEncodedContent(parameters);
			var response = await _httpClient.SendAsync(request);
			if (!response.IsSuccessStatusCode) {
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new Exception($"Authentication failed with status {response.StatusCode}: {errorContent}");
			}
			var content = await response.Content.ReadAsStringAsync();
			var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content);
			return tokenResponse.access_token;
		}
		public async Task<(string token, string instanceUrl, string tenantId)> GetAccessTokenAsync() {
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
			if (!response.IsSuccessStatusCode)
				throw new HttpRequestException($"OAuth failed: {response.StatusCode}, {responseContent}");
			var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
			var tenantId = data!["id"].Split('/')[^2];
			return (data["access_token"], data["instance_url"], tenantId);
		}
		public async Task<JsonElement> GetObjectSchemaAsync(string objectName, CancellationToken cancellationToken = default) {
			if (string.IsNullOrWhiteSpace(objectName)) {    // Validate input
				throw new ArgumentException("Object name cannot be empty or null", nameof(objectName));
			}
			var (token, instanceUrl, _) = await GetAccessTokenAsync();// Get token and instance URL
			if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(instanceUrl)) {
				_logger.LogError("Invalid token or instance URL for {ObjectName}", objectName);
				throw new Exception("Authentication failed: missing token or instance URL");
			}
			string apiVersion = string.IsNullOrEmpty(_settings.ApiVersion) ? "63.0" : _settings.ApiVersion; // Construct URL
			string url = $"{instanceUrl}/services/data/v{apiVersion}/sobjects/{Uri.EscapeDataString(objectName)}/describe";
			_logger.LogDebug("Fetching schema for {ObjectName} from {Url}", objectName, url);
			using var request = new HttpRequestMessage(HttpMethod.Get, url);// Set up request
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
				if (!schema.TryGetProperty("fields", out _) || !schema.TryGetProperty("childRelationships", out _)) {// Validate schema
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
		public async Task<DataTable> GetAllObjects() {
			var (token, instanceUrl, _) = await GetAccessTokenAsync();
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
		public async Task<DataSet> GetObjectSchemaAsDataTableAsync(string objectName) {
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
							new XElement("relationshipName", f.relationshipName ?? ""),
							new XElement("referenceTo", string.Join(",", f.referenceTo))
						)
					),
				childRelationNames.Select(cr => new XElement($"relations",
								new XElement("Name", cr.Name ?? ""),
								new XElement("ChildSObject", cr.Child ?? ""),
								new XElement("Field", cr.field ?? ""),
								new XElement("Cascade", cr.Cascade)
							)) );
				using (XmlReader xr = schemaRoot.CreateReader()) ds.ReadXml(xr);
			} catch (Exception ex) {
				_logger.LogError(ex.Message);
				_logger.LogError(ex.StackTrace);
				return null;
			}
			return ds;
		}
		// ===================================================================================
		public class TokenResponse {
			public string access_token { get; set; }
			public string instance_url { get; set; }
			public string id { get; set; }
		}
	}
}