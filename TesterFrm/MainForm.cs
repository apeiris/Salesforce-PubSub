using Microsoft.Extensions.Hosting;
using NetUtils;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Text.Json;
using System.Diagnostics;
using Serilog;
using System.Windows.Forms;
namespace TesterFrm {
	public partial class MainForm : Form {
		#region fields
		private readonly IMemoryCache _cache;
		private const string CacheKey = "SalesforceAccessToken";
		private readonly IHost _host;
		private readonly ISalesforceService _salesforceService;
		private readonly PubSubService _pubSubService;
		private readonly SalesforceConfig _config;
		private readonly Serilog.ILogger _logger;
		static string _token = "";
		static string _instanceUrl = "";
		static string _tenantId = "";
		//static bool _doing = false;
		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
		private readonly object _dgvLock = new object();
		static bool _sfsObjectsLoaded = false;
		// Dictionary to store checkbox states, scoped per DataGridView
		private readonly Dictionary<DataGridView, Dictionary<int, bool>> _rowHeaderCheckStatesMap
			= new Dictionary<DataGridView, Dictionary<int, bool>>();

		private DataTable _sourceTable; // Data source for dgvSource
		private DataTable _destinationTable; // Data source for dgvDestination
		private List<DataRow> _rowsToMove = new List<DataRow>(); // Temp storage for rows to move


		private readonly object _lock = new object();
		#endregion fields
		#region buttons
		private async void btnAuthenticate_Click(object sender, EventArgs e) {
			await _semaphore.WaitAsync();
			try {
				btnAuthenticate.Enabled = false;
				this.Invoke((Action)(() => txtResult.Clear()));
				var token = await _salesforceService.AuthenticateAsync();
				this.Invoke((Action)(() => txtResult.Text = $"Token copied to clipboard: {token}..."));
				Clipboard.SetText(token);

			} catch (Exception ex) {
				this.Invoke((Action)(() => txtResult.Text = $"Authentication failed: {ex.Message}"));
			}
			finally {
				btnAuthenticate.Enabled = true;
				_semaphore.Release();
			}
		}
		private void btnGetTokenAsync_Click(object sender, EventArgs e) {
			_ = GetAccessToken();
		}
		private async void btnSubscribe_Click(object sender, EventArgs e) {
			//lbxResult.Items.Add("Starting subscription...");
			try {

				await _pubSubService.StartSubscriptionsAsync(msg => {
					txtResult.Invoke(new Action(() => txtResult.Text = msg));
				});
				Clipboard.SetText(txtResult.Text);
				toolStripStatusLabel1.Text = "Token copied to Clipboard.";
			} catch (Exception ex) {
				MessageBox.Show($"Error: {ex.Message}");
			}
		}
		private async void btnGetSchema_Click(object sender, EventArgs e) {
			await _semaphore.WaitAsync();
			try {
				if (lbxObjects.SelectedItems.Count == 0) {
					MessageBox.Show("Please select a topic from the list.");
					return;
				}

				// Perform the async operation outside the lock
				DataTable dt = await _salesforceService.GetObjectSchemaAsDataTableAsync(ObjectFromTopic((string)lbxObjects.SelectedItem!));

				// Now synchronize access to the UI with lock
				lock (_dgvLock) {
					this.Invoke((Action)(() => {
						dgvObject.DataSource = dt;
						dgvObject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

						toolStripStatusLabel1.Text = $"Schema for {dt.TableName} having {dt.Rows.Count} rows loaded successfully.";
					}));
				}
			} catch (Exception ex) {
				this.Invoke((Action)(() => txtResult.Text = $"Error: {ex.Message}"));
			}
			finally {
				_semaphore.Release();
			}
		}
		private void CopySchema(DataGridView source, DataGridView destination) {
			// Copy column structure to destination DataGridView
			foreach (DataGridViewColumn col in source.Columns) {
				destination.Columns.Add((DataGridViewColumn)col.Clone());
			}

			// Ensure the destination DataTable matches the schema
			if (destination.DataSource == null) {
				// If no DataSource exists, create a new DataTable and assign it
				destination.DataSource = new DataTable();
			}

			DataTable destTable = (DataTable)destination.DataSource;
			DataTable srcTable = (DataTable)source.DataSource;

			if (srcTable == null) {
				throw new InvalidOperationException("Source DataGridView has no DataTable assigned.");
			}

			// Clear and rebuild destination columns
			destTable.Columns.Clear();
			foreach (DataColumn col in srcTable.Columns) {
				destTable.Columns.Add(col.ColumnName, col.DataType);
			}
		}
		private async void btnGetSfObjects_Click(object sender, EventArgs e) {

			await LoadSfObjectsAsync();
		}
		#region move right and left
		private void btnMoveRight_Click(object sender, EventArgs e) {
			if (dgvSource.SelectedRows.Count == 0) return;
			_sourceTable = (DataTable)dgvSource.DataSource!;
			if (dgvDestination.DataSource == null) {
				_destinationTable = _sourceTable!.Clone(); // Clone structure only
				dgvDestination.DataSource = _destinationTable;
			} else {
				_destinationTable = (DataTable)dgvDestination.DataSource;
			}
			// Sort the source table by name
			List<DataRow> rowsToRemove = new List<DataRow>();// Create a list to store rows to remove (to avoid modifying collection during iteration)
			foreach (DataGridViewRow row in dgvSource.SelectedRows) {// Move selected rows
				DataRow sourceRow = ((DataRowView)row.DataBoundItem!).Row;
				_destinationTable!.ImportRow(sourceRow); // Add to destination
				rowsToRemove.Add(sourceRow);// Mark for removal from source
			}
			foreach (DataRow row in rowsToRemove) { // Remove rows from source after iteration
				_sourceTable!.Rows.Remove(row);
			}
			dgvSource.DataSource = null;// Refresh both DataGridViews
			dgvSource.DataSource = _sourceTable;
			dgvSource.Columns[0].HeaderText = "Salesforce Objects";
			dgvSource.Refresh();
			dgvDestination.DataSource = null;
			dgvDestination.DataSource = _destinationTable;
			dgvDestination.Refresh();
			dgvDestination.Columns[0].HeaderText = "CDC Candidates";
			dgvSource.AutoResizeColumns();// Optional: Adjust column sizes
			dgvDestination.AutoResizeColumns();
			lblSourceList.Text = $"{dgvSource.Rows.Count} Salesforce objects";
		}

		private void btnMoveLeft_Click(object sender, EventArgs e) {
			if (dgvDestination.SelectedRows.Count == 0) return;
			_sourceTable = (DataTable)dgvSource.DataSource;
			_destinationTable = (DataTable)dgvDestination.DataSource;
			if (_sourceTable == null) {
				_sourceTable = _destinationTable.Clone();
				dgvSource.DataSource = _sourceTable;
			}
			List<DataRow> rowsToRemove = new List<DataRow>();
			foreach (DataGridViewRow row in dgvDestination.SelectedRows) {
				DataRow destRow = ((DataRowView)row.DataBoundItem).Row;
				_sourceTable.ImportRow(destRow);
				rowsToRemove.Add(destRow);
			}
			foreach (DataRow row in rowsToRemove) {
				_destinationTable.Rows.Remove(row);
			}
			dgvSource.DataSource = null;
			dgvSource.DataSource = _sourceTable;
			dgvSource.Refresh();
			dgvSource.Columns[0].HeaderText = "Salesforce Objects";

			dgvDestination.DataSource = null;
			dgvDestination.DataSource = _destinationTable;
			dgvDestination.Refresh();
			dgvDestination.Columns[0].HeaderText = "CDC Candidates";
			dgvSource.AutoResizeColumns();
			dgvDestination.AutoResizeColumns();
		}
		#endregion move right and left
		private void btnClearDestination_Click(object sender, EventArgs e) {
			dgvDestination.SelectAll();
			foreach (DataGridViewRow row in dgvDestination.Rows) {
				btnMoveLeft.PerformClick();
			}
			lblDestinationList.Text = $"{dgvDestination.Rows.Count} candidate rows";
		}

		private void btnCommit_Click(object sender, EventArgs e) {
			List<string> selectedFields = _config.Topics.GetFieldsToFilterByName((string)lbxObjects.SelectedItem);
			Debug.WriteLine($"Selected fields:{string.Join(", ", selectedFields)}");
			MessageBox.Show($"Selected fields:{string.Join(", ", selectedFields)}");
		}
		#endregion buttons
		#region dgv
		private void UpdateRowHeaderCheckboxes(DataGridView dgvObject, string columnName, List<string> fieldList) {
			// Ensure the map exists for this DataGridView
			if (!_rowHeaderCheckStatesMap.ContainsKey(dgvObject)) {
				_rowHeaderCheckStatesMap[dgvObject] = new Dictionary<int, bool>();
			}
			// Loop through all rows in the DataGridView
			foreach (DataGridViewRow row in dgvObject.Rows) {
				if (row.Cells["name"].Value != null) {
					string cellValue = row.Cells[columnName].Value.ToString();
					// Check if the cell value exists in the List<string>
					bool shouldBeChecked = fieldList.Contains(cellValue);
					// Update the checkbox state in the map
					_rowHeaderCheckStatesMap[dgvObject][row.Index] = shouldBeChecked;
					// Force the row header to redraw with the new checkbox state
					row.HeaderCell.Value = shouldBeChecked; //.ToString();
					dgvObject.InvalidateRow(row.Index);
				}
			}
		}

		private void SetupDataGridViewHeaders(string tn) {


		
			dgvObject.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
			dgvObject.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
			dgvObject.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.DarkBlue;
			dgvObject.TopLeftHeaderCell.Value = "Subscribe";

			dgvSource.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
			dgvSource.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
			dgvSource.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			dgvSource.ColumnHeadersHeight = 40;

			dgvSource.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
			dgvSource.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.DarkBlue;

			dgvSource.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

			dgvSource.RowTemplate.Height = 30; // Set the height of the row template
			dgvSource.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dgvSource.AutoGenerateColumns = true;
			dgvSource.DataSource = null;
			dgvSource.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			dgvDestination.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
			dgvDestination.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
			dgvDestination.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			dgvDestination.ColumnHeadersHeight = 40;

			dgvDestination.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
			dgvDestination.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.DarkBlue;

			dgvDestination.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


			dgvDestination.RowTemplate.Height = 30; // Set the height of the row template
			dgvDestination.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dgvDestination.AutoGenerateColumns = true;
			dgvDestination.DataSource = null;
			dgvDestination.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


		}

		private void dgvRowCountChanged(object sender, EventArgs e) {

			switch (sender) {
				case DataGridView s when s == dgvSource:
				lblSourceList.Text = $"{dgvSource.Rows.Count} Salesforce objects";
				break;

				case DataGridView s when s == dgvDestination:
				lblDestinationList.Text = $"{dgvDestination.Rows.Count} candidate Object";
				break;
			}
		}

		private void AttachHandlers(DataGridView dgv) {
			//dgv.RowPostPaint += DataGridView_RowPostPaint;
			//dgv.MouseClick += DataGridView_MouseClick;
		}
		private async Task LoadSfObjectsAsync() {
			_logger.Debug("LoadSfObjectsAsync()");
			this.Invoke((Action)(() => Cursor.Current = Cursors.WaitCursor));
			await _semaphore.WaitAsync();
			try {
				_sourceTable = await _salesforceService.GetAllObjects();
				_sourceTable.DefaultView.Sort = "name ASC"; // Sort the source table by name

				lock (_dgvLock) {
					this.Invoke((Action)(() => {
						dgvSource.DataSource = _sourceTable;
						dgvSource.Columns[0].HeaderText = "Salesforce Objects";
						toolStripStatusLabel1.Text = $"Schema for {_sourceTable.TableName} having {_sourceTable.Rows.Count} rows loaded successfully.";
						CopySchema(dgvSource, dgvDestination);
					}));
				}
			} catch (Exception ex) {
				Debug.WriteLine($"Error: {ex.Message}");
			}
			finally {
				_semaphore.Release();
				this.Invoke((Action)(() => Cursor.Current = Cursors.Default));
			}
		}
		#endregion dgv
		#region form
		public MainForm(IMemoryCache cache, ISalesforceService salesforceService, PubSubService pubSubService, IOptions<SalesforceConfig> config) {
			InitializeComponent();
			_cache = cache;
			_salesforceService = salesforceService;
			_pubSubService = pubSubService;
			_config = config.Value;
			_logger = Log.ForContext<MainForm>();

			_logger.Information("MainForm initialized.");
		}
		private void Form1_Load(object sender, EventArgs e) {
			string savedTab = string.IsNullOrEmpty(Properties.Settings.Default.SelectedTab) ? "tbpSfObjects" : Properties.Settings.Default.SelectedTab;
			if (!string.IsNullOrEmpty(savedTab) && tabControl1.TabPages.ContainsKey(savedTab)) {
				//tabControl1.SelectedTab = tabControl1.TabPages[savedTab];

				//tabControl1.TabPages[savedTab].Select();

				TabPage tbp = tabControl1.TabPages[savedTab];
				tabControl1_Selected(sender, new TabControlEventArgs(tbp, tabControl1.SelectedIndex, TabControlAction.Selected));
			}
			lblPanel1.Parent = splitContainer1.Panel1;
			lblPanel2.Parent = splitContainer1.Panel2;
			SetupDataGridViewHeaders("");
		}
		protected override void OnFormClosed(FormClosedEventArgs e) {
			base.OnFormClosed(e);
			if (_host != null) _host.Dispose();
		}
		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.SelectedTab = tabControl1.SelectedTab.Name;
			Properties.Settings.Default.Save();
		}
		#endregion	 form	
		#region groupbox
		private void grpFilterOptions_EnabledChanged(object sender, EventArgs e) {
		}
		#endregion groupbox
		#region helpers
		public string ObjectFromTopic(string topicName) {
			var match = Regex.Match(topicName, @"/data/(\w+)ChangeEvent");
			return match.Success ? match.Groups[1].Value : throw new ArgumentException($"Invalid topic name format: {topicName}");
		}
		private async Task GetAccessToken() {
			if (!_cache.TryGetValue(CacheKey, out string token)) {
				(token, _instanceUrl, _tenantId) = await _salesforceService.GetAccessTokenAsync();
				_cache.Set(CacheKey, token, TimeSpan.FromMinutes(30));
			}
			this.Invoke((Action)(() => txtResult.Text = $"Token: {token}, Instance URL: {_instanceUrl}, Tenant ID: {_tenantId}"));
		}
		public async Task<DataTable> RemoveRowsNotInColumnList(DataTable dataTable, List<string> allColumns) {
			if (dataTable == null || !dataTable.Columns.Contains("Name") || allColumns == null)
				return null;

			var rowsToDelete = dataTable.AsEnumerable()
				.Where(row => !allColumns.Contains(row.Field<string>("Name")))
				.ToList();

			foreach (var row in rowsToDelete) {
				dataTable.Rows.Remove(row);
			}
			return dataTable;
		}
		private void CopySourceScheama(DataGridView source, DataGridView destination) {


			destination.Columns.Clear();
			destination.Rows.Clear();

			// Copy header style
			destination.ColumnHeadersDefaultCellStyle = source.ColumnHeadersDefaultCellStyle.Clone();
			destination.ColumnHeadersHeight = source.ColumnHeadersHeight;
			destination.ColumnHeadersHeightSizeMode = source.ColumnHeadersHeightSizeMode;
			destination.EnableHeadersVisualStyles = source.EnableHeadersVisualStyles;
			destination.RowHeadersWidth = source.RowHeadersWidth;
			// Copy columns
			foreach (DataGridViewColumn col in source.Columns) {
				DataGridViewColumn ncol = (DataGridViewColumn)col.Clone();

				// Explicitly set properties
				ncol.Width = col.Width;                    // Set the exact width
				ncol.MinimumWidth = col.MinimumWidth;      // Set minimum width
				ncol.FillWeight = col.FillWeight;          // Set fill weight for proportional sizing
				ncol.Resizable = col.Resizable;            // Copy resizable property

				// Force Displayed width to match source (optional, for visible columns)
				//	ncol.Displayed = col.Displayed;

				// Disable auto-sizing to preserve the exact width
				ncol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

				// Add the column to destination
				destination.Columns.Add(ncol);


			}
			destination.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;


		}




		#endregion helpers
		#region lbx
		private void LoadTopics(ListBox listBox) {
			listBox.Items.Clear();
			listBox.Items.AddRange(_config.Topics.Select(topic => topic.Name).ToArray());
		}

		private async void lbxObjects_SelectedIndexChanged(object sender, EventArgs e) {
			if (lbxObjects.SelectedItem == null) return;
			this.UseWaitCursor = true;
			await _semaphore.WaitAsync();
			try {
				string selectedTopic = lbxObjects.SelectedItem.ToString();
				this.Invoke((Action)(() => {

					lblPanel1.Text = toolStripStatusLabel1.Text;
					List<string> fields = _config.Topics.GetFieldsToFilterByName(selectedTopic);
					lbxFields.Items.Clear();
					lbxFields.Items.AddRange(fields.ToArray());

				}));
				// Perform async operations outside the lock
				DataTable dt = await _salesforceService.GetObjectSchemaAsDataTableAsync(ObjectFromTopic(selectedTopic));

				toolStripStatusLabel1.Text = $" {ObjectFromTopic(selectedTopic)} has {dt.Rows.Count} fields.";
				if (rbtFilterSubscribed.Checked) {
					dt = await RemoveRowsNotInColumnList(dt, _config.Topics.GetFieldsToFilterByName(selectedTopic));
				}
				// Synchronize UI updates with lock
				lock (_dgvLock) {
					this.Invoke((Action)(() => {
						dgvObject.DataSource = dt;
						dgvObject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
						UpdateRowHeaderCheckboxes(dgvObject, "name", _config.Topics.GetFieldsToFilterByName(selectedTopic));
					}));
				}
			} catch (Exception ex) {
				this.Invoke((Action)(() => toolStripStatusLabel1.Text = $"Error: {ex.Message}"));
			}
			finally {

				_semaphore.Release();
				this.UseWaitCursor = false;
			}

		}

		private void filterChanged(object sender, EventArgs e) {
			lbxObjects_SelectedIndexChanged(null, null);
		}

		#endregion lbx
		#region tabs

		private async Task tabControl1_Selected(object sender, TabControlEventArgs e) {
			_logger.Information($"tabpage={e.TabPage.Name}");
			Log.Information("hahaha");
			Debug.WriteLine("this is debuggggg");
			Debug.WriteLine($" tabpage={e.TabPage.Name}");
			switch (e.TabPage.Name.ToLower()) {
				case "tbpsfobjects":
				if (!_sfsObjectsLoaded) {
					await LoadSfObjectsAsync();
					_sfsObjectsLoaded = true;
					CopySourceScheama(dgvSource, dgvDestination);
				}
				break;
				case "tbppubsub":
				LoadTopics(lbxObjects); // Load topics into the listbox
				break;
				default: break;
			}
			tabControl1.SelectedTab = e.TabPage;
		}
		private async void TabControl1_Selected(object sender, TabControlEventArgs e) {
			await tabControl1_Selected(sender, e); // Call the async Task method
		}
		#endregion tabs







	
	}
}





