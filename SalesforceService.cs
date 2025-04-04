using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Reflection.Metadata.Ecma335;
using System.Xml;
namespace NetUtils
	{
	public class SalesforceService : ISalesforceService
		{
		private readonly HttpClient _httpClient;
		private readonly SalesforceConfig _settings;


		public SalesforceService(HttpClient httpClient,IOptions<SalesforceConfig> settings )
			{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
			}

		public async Task<string> AuthenticateAsync()
			{
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
			if (!response.IsSuccessStatusCode)
				{
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new Exception($"Authentication failed with status {response.StatusCode}: {errorContent}");
				}
			var content = await response.Content.ReadAsStringAsync();
			var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content);
			return tokenResponse.access_token;
			}

		public async Task<(string token, string instanceUrl, string tenantId)> GetAccessTokenAsync()
			{
			var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.LoginUrl}/services/oauth2/token")
				{
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

		/// <summary>
		/// Gets the schema (metadata) of a Salesforce object using the REST API.
		/// </summary>
		/// <param name="objectName">The API name of the object (e.g., "Account").</param>
		/// <returns>A JsonElement containing the schema description.</returns>
		public async Task<JsonElement> GetObjectSchemaAsync(string objectName)
			{
			// Get token and instance URL
			var (token, instanceUrl, _) = await GetAccessTokenAsync();

			// Construct the describe endpoint URL
			string url = $"{instanceUrl}/services/data/v{_settings.ApiVersion}/sobjects/{objectName}/describe";

			// Set up the HTTP request
			using var request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Headers.Add("Authorization", $"Bearer {token}");
			request.Headers.Add("Accept", "application/json");

			try
				{
				// Send the request
				HttpResponseMessage response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();

				// Read and parse the JSON response
				string jsonResponse = await response.Content.ReadAsStringAsync();
				JsonElement schema = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
				return schema;
				}
			catch (HttpRequestException ex)
				{
				throw new Exception($"Failed to retrieve schema for {objectName}: {ex.Message}", ex);
				}
			}

		/// <summary>
		/// Gets a human-readable summary of the schema for a Salesforce object.
		/// </summary>
		/// <param name="objectName">The API name of the object (e.g., "Account").</param>
		/// <param name="apiVersion">The Salesforce API version (e.g., "v59.0").</param>
		/// <returns>A string summarizing the object's schema.</returns>
		public async Task<string> GetObjectSchemaSummaryAsync(string objectName)
			{
			JsonElement schema = await GetObjectSchemaAsync(objectName);
			var sb = new System.Text.StringBuilder();

			// Extract basic object info
			sb.AppendLine($"Object: {schema.GetProperty("name").GetString()}");

			sb.AppendLine($"Label: {schema.GetProperty("label").GetString()}");
			sb.AppendLine("Fields:");

			// Iterate through fields
			foreach (JsonElement field in schema.GetProperty("fields").EnumerateArray())
				{
				string fieldName = field.GetProperty("name").GetString();
				string fieldType = field.GetProperty("type").GetString();
				string fieldLabel = field.GetProperty("label").GetString();
				string fieldLength = field.GetProperty("length").GetUInt32().ToString();
				bool isCustom = field.GetProperty("custom").GetBoolean();

				sb.AppendLine($"- {fieldName} ({fieldType}): {fieldLabel}:{fieldLength}: {(isCustom ? "[Custom]" : "")}");
				}

			return sb.ToString();
			}

	


		public async Task<DataTable> GetObjectSchemaAsDataTableAsync(string objectName)
			{
			var schemax = await GetObjectSchemaAsync(objectName);
			JsonDocument schema = JsonDocument.Parse(schemax.ToString());

			var fields = schema.RootElement
		   .GetProperty("fields")
		   .EnumerateArray()
		   .Select(field => new
			   {
			   Name = field.GetProperty("name").GetString(),
			   Type = field.GetProperty("type").GetString(),
			   Label = field.GetProperty("label").GetString(),
			   Length = field.GetProperty("length").GetUInt32(),
			   Custom = field.GetProperty("custom").GetBoolean()
			   });

			XElement root = new XElement($"{objectName}",
				fields.Select(f => new XElement("Field",
					new XElement("Name", f.Name),
					new XElement("Type", f.Type),
					new XElement("Label", f.Label),
					new XElement("Length", f.Length),
					new XElement("Custom", f.Custom)
				))
			);
			DataSet ds = new DataSet();
			using (XmlReader xr = root.CreateReader()) ds.ReadXml(xr);
			ds.Tables[0].TableName = objectName;
			return ds.Tables[0];
			}

		// Helper methods (unchanged)
		private Type GetColumnType(JsonValueKind valueKind)
			{
			return valueKind switch
				{
					JsonValueKind.String => typeof(string),


					JsonValueKind.Number => typeof(int),
					JsonValueKind.True => typeof(bool),
					JsonValueKind.False => typeof(bool),
					JsonValueKind.Null => typeof(string),
					_ => null!
					};
			}

		private object ConvertJsonValue(JsonElement element, Type targetType)
			{
			if (element.ValueKind == JsonValueKind.Null)
				return DBNull.Value;

			if (targetType == typeof(string))
				return element.GetString();
			if (targetType == typeof(int))
				return element.TryGetInt32(out int value) ? value : 0;
			if (targetType == typeof(bool))
				return element.GetBoolean();

			return DBNull.Value;
			}
		}


	// ===================================================================================
	

	// Assuming these are defined elsewhere in your project

	public class TokenResponse
		{
		public string access_token { get; set; }
		public string instance_url { get; set; }
		public string id { get; set; }
		}
	}