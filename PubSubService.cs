using System.Text.Json;
using Microsoft.Extensions.Options;
using Grpc.Core;
using Grpc.Net.Client;
using Avro;
using static NetUtils.PubSub;
using System.Text;
using Avro.IO;
using Avro.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace NetUtils {
	public class PubSubService : IDisposable {
		private readonly ISalesforceService _oauthService;
		private readonly SalesforceConfig _config;
		private readonly GrpcChannel _channel;
		private readonly PubSubClient _client;
		private readonly List<(AsyncDuplexStreamingCall<FetchRequest, FetchResponse> Call, CancellationTokenSource Cts)> _subscriptions;
		private readonly Dictionary<string, RecordSchema> _schemaCache;

		public PubSubService(ISalesforceService oauthService, IOptions<SalesforceConfig> configOptions) {
			_oauthService = oauthService ?? throw new ArgumentNullException(nameof(oauthService));
			_config = configOptions?.Value ?? throw new ArgumentNullException(nameof(configOptions));
			_channel = GrpcChannel.ForAddress(_config.GrpcUrl, new GrpcChannelOptions {
				Credentials = ChannelCredentials.SecureSsl
			});
			_client = new PubSubClient(_channel);
			_subscriptions = new List<(AsyncDuplexStreamingCall<FetchRequest, FetchResponse>, CancellationTokenSource)>();
			_schemaCache = new Dictionary<string, RecordSchema>();
		}

		public async Task<List<string>> StartSubscriptionsAsync(Action<string> onProgressUpdate) {
			onProgressUpdate?.Invoke("Starting subscriptions...");
			var (token, instanceUrl, tenantId) = await _oauthService.GetAccessTokenAsync();
			List<string> t = new List<string>();
			foreach (var topic in _config.Topics) {
				t.Add("Subscribing to topic:" + topic.Name);
				await SubscribeToTopicAsync(topic.Name, token, instanceUrl, tenantId, topic.FieldsToFilter, onProgressUpdate);
			}
			t.Add("Subscription started successfully..");
			return t;
		}

		private async Task SubscribeToTopicAsync(string topic, string token, string instanceUrl, string tenantId, List<string> fieldsToFilter, Action<string> onProgressUpdate) {
			onProgressUpdate?.Invoke($"Subscribing to {topic}...");
			var fetchRequest = new FetchRequest {
				TopicName = topic,
				NumRequested = 10,
				ReplayPreset = ReplayPreset.Latest
			};

			var callOptions = new CallOptions(credentials: CustomGrpcCredentials.Create(token, instanceUrl, tenantId));
			try {

				var streamingCall = _client.Subscribe(callOptions);
				await streamingCall.RequestStream.WriteAsync(fetchRequest);


				var cts = new CancellationTokenSource();
				_subscriptions.Add((streamingCall, cts));
				_ = Task.Run(async () => {
					try {
						while (await streamingCall.ResponseStream.MoveNext(cts.Token)) {
							var response = streamingCall.ResponseStream.Current;
							string responseJson = response.ToString();
							// Parse the JSON-like response
							var jsonDoc = JsonDocument.Parse(responseJson);
							if (jsonDoc.RootElement.TryGetProperty("events", out var eventsElement)) {
								var eventsArray = eventsElement.EnumerateArray();
								foreach (var evt in eventsArray) {
									var eventObj = evt.GetProperty("event");
									string schemaId = eventObj.GetProperty("schemaId").GetString();
									string payloadBase64 = eventObj.GetProperty("payload").GetString();
									byte[] payload = Convert.FromBase64String(payloadBase64);
									var schema = await GetSchemaAsync(schemaId, token, instanceUrl, tenantId);
									List<string> decodedEvent = DecodeChangeEvent(payload, schema, fieldsToFilter);
									foreach (var field in decodedEvent) {
										onProgressUpdate?.Invoke(field);
									}
								}
								await streamingCall.RequestStream.WriteAsync(fetchRequest);
							}
						}
					} catch (Exception ex) {
						Debug.WriteLine($"Error listening to {topic}: {ex.Message}\r\n");
					}
				});
			} catch (RpcException ex) {
				Debug.WriteLine($"gRPC Error for {topic}: {ex.StatusCode}, Detail: {ex.Status.Detail}\r\n");
			} catch (Exception ex) {
				Debug.WriteLine($"Unexpected error for {topic}: {ex.Message}\r\n");
			}
		}

		public async Task<RecordSchema> GetSchemaAsync(string schemaId, string token, string instanceUrl, string tenantId) {
			if (_schemaCache.TryGetValue(schemaId, out RecordSchema cachedSchema)) {
				return cachedSchema;
			}

			var schemaRequest = new SchemaRequest { SchemaId = schemaId };
			var callOptions = new CallOptions(credentials: CustomGrpcCredentials.Create(token, instanceUrl, tenantId));
			try {
				var schemaResponse = await _client.GetSchemaAsync(schemaRequest, callOptions);
				var schema = Schema.Parse(schemaResponse.SchemaJson) as RecordSchema;
				_schemaCache[schemaId] = schema;

				return schema;
			} catch (RpcException ex) {
				throw;
			}
		}

		private List<string> DecodeChangeEvent(byte[] payload, RecordSchema schema, List<string> fieldsToFilter) {
			try {
				using var stream = new MemoryStream(payload);
				var reader = new BinaryDecoder(stream);
				var datumReader = new GenericDatumReader<GenericRecord>(schema, schema);
				var record = datumReader.Read(null, reader);

				// Use fieldsToFilter from config
				var filterSet = new HashSet<string>(fieldsToFilter);

				// Get ChangeEventHeader
				if (!record.TryGetValue("ChangeEventHeader", out object headerObj) || headerObj is not GenericRecord header) {
					//	return "{Error: No ChangeEventHeader found}";
					return new List<string> { "Error: No Header on ChangeEvent " };
				}
				// Extract key fields from ChangeEventHeader
				string changeType = header.TryGetValue("changeType", out object ct) ? ct.ToString() : "Unknown";
				string entityName = header.TryGetValue("entityName", out object en) ? en.ToString() : "Unknown";
				var recordIds = header.TryGetValue("recordIds", out object rIds) && rIds is IList<object> idsList
					? idsList.Select(id => id.ToString()).ToList()
					: new List<string>();
				var changedFields = header.TryGetValue("changedFields", out object cf) && cf is IList<object> cfList
					? cfList.Select(f => f.ToString()).ToList()
					: new List<string>();
				// Build output based on changeType
				var fields = new List<string>
		{
			$"entityName: {entityName}",
			$"recordIds: [{string.Join(", ", recordIds)}]",
			$"changeType: {changeType}"
		};

				if (changedFields.Any()) {
					fields.Add($"changedFields: [{string.Join(", ", changedFields)}]");
				}
				// Handle DELETE specifically
				if (changeType == "DELETE") {
					if (recordIds.Any()) {
						//return $"{{Deleted Record - entityName: {entityName}, recordId: {recordIds[0]}, changeType: {changeType}}}";
						return fields;
					}
					//return $"{{Deleted Record - entityName: {entityName}, recordId: Not Available, changeType: {changeType}}}";
					return fields;
				}

				// For non-DELETE events, include filtered fields
				foreach (var field in schema.Fields) {
					if (filterSet.Contains(field.Name) && record.TryGetValue(field.Name, out object value) && value != null) {
						if (changedFields.Contains(field.Name)) {
							fields.Add($"{field.Name}: {value} (changed)");
						} else {
							fields.Add($"{field.Name}: {value}");
						}
					}
				}
				//return fields.Any() ? $"{{{string.Join(", ", fields)}}}" : "{No filtered fields changed}";
				return fields;
			} catch (Exception ex) {
				//return $"Error decoding event: {ex.Message}";

				return new List<string> { ex.Message };
			}
		}
		#region required but has no application specific functionality
		public void Dispose() {
			foreach (var (call, cts) in _subscriptions) {
				cts.Cancel();
				call.Dispose();
			}
			_subscriptions.Clear();
			_channel.Dispose();
		}
		#endregion
	}
}
