using System.Data;
using System.Text.Json;
namespace NetUtils;
public interface ISalesforceService {
	Task<(string token, string instanceUrl, string tenantId)> GetAccessTokenAsync();
	Task<JsonElement> GetObjectSchemaAsync(string objectName, CancellationToken cancellationToken = default);
	Task<string> GetObjectSchemaSummaryAsync(string objectName);
	Task<DataSet> GetObjectSchemaAsDataSetAsync(string objectName, bool useTooling = false);
	Task<DataTable> GetAllObjects();
	Task<DataTable> GetSalesforceRecord(string objectName, string recordId);
	
	Task<JsonElement> GetPlatformEventChannel(CancellationToken cancellationToken = default);
	Task<DataTable> GetCDCEnabledEntitiesAsync(CancellationToken cancellationToken = default);
	//Task<JsonElement> ExecuteSoqlQueryRawAsync(string soqlQuery, CancellationToken cancellationToken = default,bool useTooling =false);
	Task<DataTable> ExecSoqlToTable(string soql, bool useTooling);
	Task<JsonElement> ExecuteSoqlQueryRawAsync(string soqlQuery, CancellationToken cancellationToken = default, bool useTooling = true, HttpMethod? method = null);
	Task<DataTable> UpsertSobject(string objectName, string recordId, string jsonFields);
	Task DeleteSobject(string objectName, string recordId);
}