using System.Text.Json;
using System.Data;
namespace NetUtils;

public interface ISalesforceService
	{
	Task<string> AuthenticateAsync();
	Task<(string token, string instanceUrl, string tenantId)> GetAccessTokenAsync();
	Task<JsonElement> GetObjectSchemaAsync(string objectName);
	Task<string> GetObjectSchemaSummaryAsync(string objectName);
	Task<DataTable> GetObjectSchemaAsDataTableAsync(string objectName);
	}