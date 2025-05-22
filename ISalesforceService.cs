using System.Text.Json;
using System.Data;
namespace NetUtils;
public interface ISalesforceService {
	Task<(string token, string instanceUrl, string tenantId)> GetAccessTokenAsync();
	Task<JsonElement> GetObjectSchemaAsync(string objectName, CancellationToken cancellationToken = default);
	Task<string> GetObjectSchemaSummaryAsync(string objectName);
	Task<DataSet> GetObjectSchemaAsDataSetAsync(string objectName,bool useTooling=false);
	Task<DataTable> GetAllObjects();
	Task<DataTable> GetSalesforceRecord(string objectName, string recordId);
	//Task<DataTable> GetCDCSubscriptions();
	Task<JsonElement> GetPlatformEventChannel(CancellationToken cancellationToken = default);
	Task<DataTable> GetCDCEnabledEntitiesAsync(CancellationToken cancellationToken = default);
}