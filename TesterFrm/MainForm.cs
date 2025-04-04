using Microsoft.Extensions.Hosting;
using NetUtils;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using Microsoft.Extensions.Options;
namespace TesterFrm {
	public partial class MainForm : Form {
		private readonly IMemoryCache _cache;
		private const string CacheKey = "SalesforceAccessToken";
		private readonly IHost _host;
		private readonly ISalesforceService _salesforceService;
		private readonly PubSubService _pubSubService;
		private readonly SalesforceConfig _config;
		static string _token = "";
		static string _instanceUrl = "";
		static string _tenantId = "";

		public MainForm(IMemoryCache cache, ISalesforceService salesforceService, PubSubService pubSubService, IOptions<SalesforceConfig> config) {
			InitializeComponent();
			_cache = cache;
			_salesforceService = salesforceService;
			_pubSubService = pubSubService;
			_config = config.Value;
		}
		private async void btnAuthenticate_Click(object sender, EventArgs e) {
			txtResult.Clear();
			try {
				var token = await _salesforceService.AuthenticateAsync();
				txtResult.Text = $"Successfully authenticated. Access Token: {token}...";
			} catch (Exception ex) {
				txtResult.Text = $"Authentication failed: {ex.Message}";
			}
		}
		protected override void OnFormClosed(FormClosedEventArgs e) {
			base.OnFormClosed(e);
			_host.Dispose(); // Clean up the host when the form closes
		}
		private async Task GetAccessToken() {
			(_token, _instanceUrl, _tenantId) = await _salesforceService.GetAccessTokenAsync();
			txtResult.Text = $"Token: {_token}, Instance URL: {_instanceUrl}, Tenant ID: {_tenantId}";
		}
		private void btnGetTokenAsync_Click(object sender, EventArgs e) {
			_ = GetAccessToken();
		}
		private void Form1_Load(object sender, EventArgs e) {
			string savedTab = Properties.Settings.Default.SelectedTab;
			if (!string.IsNullOrEmpty(savedTab) && tabControl1.TabPages.ContainsKey(savedTab)) {
				tabControl1.SelectedTab = tabControl1.TabPages[savedTab];
			}
			LoadTopics(lbxObjects);// Load topics into the ListBox
		}
		private async void btnSubscribe_Click(object sender, EventArgs e) {
			//lbxResult.Items.Add("Starting subscription...");
			try {

				await _pubSubService.StartSubscriptionsAsync(msg => {
					//	lbxResult.Invoke(new Action(() => lbxResult.Items.Add(msg)));
				});
			} catch (Exception ex) {
				MessageBox.Show($"Error: {ex.Message}");
			}
		}
		private void button1_Click(object sender, EventArgs e) {

		}
		private void SetupDataGridViewHeaders(string tn) {
			dgvObject.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
			dgvObject.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
			dgvObject.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.DarkBlue;
			dgvObject.Columns[0].HeaderText = tn;

		}
		private async void btnGetSchema_Click(object sender, EventArgs e) {
			try {
				DataTable dt = await _salesforceService.GetObjectSchemaAsDataTableAsync("Account");
				dgvObject.DataSource = dt;
				dgvObject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
				SetupDataGridViewHeaders(dt.TableName);
				toolStripStatusLabel1.Text = $"Schema for {dt.TableName} having {dt.Rows.Count} rows loaded successfully.";
			} catch (Exception ex) {
				txtResult.Text = $"Error: {ex.Message}";
			}
		}
		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.SelectedTab = tabControl1.SelectedTab.Name;
			Properties.Settings.Default.Save();
		}

		private void LoadTopics(ListBox listBox) {
			listBox.Items.Clear();
			foreach (var topic in _config.Topics) {
				listBox.Items.Add(topic.Name);
			}
		}

	}


}