using System.Text.Json;
using System.Data;
namespace NetUtils;

public interface ISalesforceService
	{
	Task<string> AuthenticateAsync();
	Task<(string token, string instanceUrl, string tenantId)> GetAccessTokenAsync();
	Task<JsonElement> GetObjectSchemaAsync(string objectName, CancellationToken cancellationToken = default);
	Task<string> GetObjectSchemaSummaryAsync(string objectName);

	Task<DataSet> GetObjectSchemaAsDataTableAsync(string objectName);
	Task<DataTable> GetAllObjects();

}