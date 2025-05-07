using Microsoft.Extensions.Hosting;
using NetUtils;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using System.Diagnostics;
using mySalesforce;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;
using System.Security.Cryptography.Xml;
using enmRetrievedFrom = TesterFrm.MainForm.enmRetrieveFrom;
using Newtonsoft.Json;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ToolTip = System.Windows.Forms.ToolTip;
using Button = System.Windows.Forms.Button;
//using DocumentFormat.OpenXml.Wordprocessing;
namespace TesterFrm {
	public partial class MainForm : Form {
		#region enums
		public enum enmRetrieveFrom {
			SalesForce,
			SqlServer,
			None
		}

		#endregion enums
		#region fields
		private readonly IMemoryCache _cache;
		private const string CacheKey = "SalesforceAccessToken";
		private readonly IHost _host;
		private readonly ISalesforceService _salesforceService;
		private readonly PubSubService _pubSubService;
		private readonly SalesforceConfig _config;
		private readonly ILogger<MainForm> _l;

		static string _token = "";
		static string _instanceUrl = "";
		static string _tenantId = "";
		//static bool _doing = false;
		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
		private readonly object _dgvLock = new object();
		static bool _sfsObjectsLoaded = false;
		private List<string> _sfoTables = new List<string>(); // List of Salesforce objects from SQL Server
															  // Dictionary to store checkbox states, scoped per DataGridView
		private readonly Dictionary<DataGridView, Dictionary<int, bool>> _rowHeaderCheckStatesMap
			= new Dictionary<DataGridView, Dictionary<int, bool>>();

		private DataTable _sourceTable; // Data source for dgvSource
		private DataTable _destinationTable; // Data source for dgvDestination
		private DataTable _dtRegistered; // Data source for registered tables
		private static int _lbxLogMw = 0;

		private List<DataRow> _rowsToMove = new List<DataRow>(); // Temp storage for rows to move
		private readonly SqlServerLib _sqlServerLib;

		private readonly object _lock = new object();

		private static enmRetrieveFrom _retrieveFrom = enmRetrieveFrom.SalesForce;
		private static enmRetrievedFrom _retrievedFrom = _retrieveFrom;

		#endregion fields
		#region events
		#region pubsubservice events
		private void PubSubService_ProgressUpdated(object sender, ProgressUpdateEventArgs e) {
			if (lbxCDCTopics.InvokeRequired) {
				lbxCDCTopics.Invoke(new Action(() => lbxCDCTopics.Items.Add(e.Message)));
			} else lbxCDCTopics.Items.Add(e.Message);
		}
		private void _pubSubService_ChangeEventReceived(object? sender, CDCEventArgs e) {
			if (dgvFilteredFields.InvokeRequired) {// Ensure UI updates happen on the UI thread

				dgvFilteredFields.Invoke(new Action(() => dgvFilteredFields.DataSource = e.FilteredFields));
				switch (e.ChangeType) {
					case "UPDATE":
					_sqlServerLib.CDCUpdateOrInsert(e.FilteredFields);
					break;//------------------------------------------------------
					case "CREATE":
					
					break;//------------------------------------------------------
					case "DELETE":
					_sqlServerLib.ExecuteNoneQuery($"DELETE FROM {e.EntityName} where Id='{e.RecordIds[0]}'}");
					break;
				}
				//	lbxCDCEvents.Invoke(new Action(() => lbxCDCEvents.Items.Add(e.FilteredFields)));
			} else dgvFilteredFields.DataSource = e.FilteredFields;
		}
		#endregion pubsubservice events
		#region _sqlserver events
		private void SqlEventObjectExist(object? sender, SqlObjectQuery e) {
			//	throw new NotImplementedException();
			Log($"CDC {e.ObjectType} {e.ObjectName} exist={e.Exist}  id= {e.Id} ", LogLevel.Information);

			btnDeleteCDCRegistration.Visible = e.Exist;
			toolStripStatusLabel1.ForeColor = Color.Yellow;
			if (e.Exist) {
				toolStripStatusLabel1.Text += $" object in the sql server with the Id {e.Id}";
				toolStripStatusLabel1.BackColor = Color.Green;
				_retrievedFrom = enmRetrieveFrom.SqlServer;
				_retrieveFrom = _retrievedFrom;
				btnRegisterFields.Text = "Update Fields";
			} else {
				_retrievedFrom = enmRetrieveFrom.SalesForce;// does not exist retrive from sf
				_retrieveFrom = _retrievedFrom;
				toolStripStatusLabel1.BackColor = Color.Brown;
				btnRegisterFields.Text = "Register Fields";
			}
			setControlColor(btnRegisterFields, e.Exist);
		}

		private void _sqlServerLib_SqlEvent(object? sender, SqlEventArg e) {
			//throw new NotImplementedException();
			Log(e.Message, LogLevel.Debug);
			if (!e.HasErrors) {
				switch (e.ReturningFrom) {
					case "RegisterExludedCDCFields":
					lbxObjects_SelectedIndexChanged(sender, e);// redo it to refresh UI
					break;
					case "DeleteCDCObject":
					//LoadTopics(lbxCDCTopics, false);
					lbxObjects_SelectedIndexChanged(sender, e);// redo it to refresh UI	
					lbxObjects.Items.Remove(lbxObjects.SelectedItem);

					lblPanel1.Text = $"{lbxObjects.Items.Count} Subscribed CDC Object";
					break;


				}
			}
		}


		#endregion _sqlserver events
		#endregion events
		#region buttons
		private async void btnAuthenticate_Click(object sender, EventArgs e) {
			await _semaphore.WaitAsync();
			try {
				btnAuthenticate.Enabled = false;
				this.Invoke((Action)(() => txtResult.Clear()));
				var token = await _salesforceService.GetSFTokenAsync();
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

				await _pubSubService.StartSubscriptionsAsync();

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
				//DataTable dt = await _salesforceService.GetObjectSchemaAsDataTableAsync(ObjectFromTopic((string)lbxObjects.SelectedItem!));
				DataSet ds = await _salesforceService.GetObjectSchemaAsDataSetAsync(ObjectFromTopic((string)lbxObjects.SelectedItem!));
				// Now synchronize access to the UI with lock
				DataTable dt = ds.Tables[ObjectFromTopic((string)lbxObjects.SelectedItem!)];
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
		#region move right and left
		private void btnMoveRight_Click(object sender, EventArgs e) {
			_l.LogDebug($"Selected rowcount={dgvSource.SelectedRows.Count}");
			if (dgvSource.SelectedRows.Count == 0) return;
			_sourceTable = (DataTable)dgvSource.DataSource!;
			if (dgvDestination.DataSource == null) {
				_destinationTable = _sourceTable!.Clone(); // Clone structure only
				dgvDestination.DataSource = _destinationTable;
			} else {
				_destinationTable = (DataTable)dgvDestination.DataSource;
			}
			List<DataRow> rowsToRemove = new List<DataRow>();// Create a list to store rows to remove (to avoid modifying collection during iteration)
			foreach (DataGridViewRow row in dgvSource.SelectedRows) {// Move selected rows
				DataRow sourceRow = ((DataRowView)row.DataBoundItem!).Row;
				_destinationTable!.ImportRow(sourceRow); // Add to destination
				rowsToRemove.Add(sourceRow);// Mark for removal from source
			}
			foreach (DataRow row in rowsToRemove) { // Remove rows from source after iteration
				_sourceTable!.Rows.Remove(row);
			}
			_l.LogDebug($"source.columns[0].header={dgvSource.Columns[0].HeaderText} destination={dgvDestination.Columns[0].HeaderText}");
			dgvSource.DataSource = null;// Refresh both DataGridViews
			dgvSource.DataSource = _sourceTable;
			dgvSource.Columns[0].HeaderText = "Salesforce Objects";
			dgvSource.Refresh();
			_l.LogDebug($"source.columns[0].header={dgvSource.Columns[0].HeaderText} destination={dgvDestination.Columns[0].HeaderText}");

			dgvDestination.DataSource = null;
			dgvDestination.DataSource = _destinationTable;
			dgvDestination.Refresh();
			dgvSource.AutoResizeColumns();// Optional: Adjust column sizes
			dgvDestination.AutoResizeColumns();
			dgvDestination.Columns[0].HeaderText = "CDC Candidates";
			lblSourceList.Text = $"{dgvSource.Rows.Count} Salesforce objects";
			Log($"Source count = {_sourceTable.Rows.Count} Destination={_destinationTable.Rows.Count}", LogLevel.Debug);
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
			_destinationTable.Clear();
			dgvDestination.Refresh();
			//foreach (DataGridViewRow row in dgvDestination.Rows) {
			//	btnMoveLeft.PerformClick();
			//}
			lblDestinationList.Text = $"{dgvDestination.Rows.Count} candidate rows";
		}
		private void btnClearLog_Click(object sender, EventArgs e) {
			lbxLog.Items.Clear();
		}
		private async void btnCommitToDB_Click(object sender, EventArgs e) {
			List<string> selectedFields = _config.Topics.GetFieldsToFilterByName((string)lbxObjects.SelectedItem);
			string script = "";
			Debug.WriteLine($"Selected fields:{string.Join(", ", selectedFields)}");
			lbxLog.Items.Add(new LogItem("btnCommit_Click executed", LogLevel.Debug));
			foreach (DataRow dr in _destinationTable.Rows) {
				string tblName = dr["name"].ToString();
				Log($"Processing {dr["name"]}", LogLevel.Debug);
				DataSet ds = await _salesforceService.GetObjectSchemaAsDataSetAsync(tblName);
				if (ds != null) {
					script = _sqlServerLib.GenerateCreateTableScript(ds.Tables[0], _config.SqlSchemaName, tblName);
					_sqlServerLib.ExecuteNoneQuery(script);
					rtfLog.Text = script;
				} else {
					Log($"Schema for the table {tblName} could not be retrived..", LogLevel.Error);
					//Debugger.Break();
				}
			}
		}
		private void btnRegisterFields_Click(object sender, EventArgs e) {
			var b = (Button)sender;
			if (b.Text.Contains("Update")) {
				updateFields();
				return;
			}
			DataTable? Fields = dgvObject.DataSource as DataTable;
			Fields.TableName = ObjectFromTopic(lbxObjects.Text);
			Fields.Columns["Exclude"].DefaultValue = false;
			Fields.AsEnumerable()
				  .Where(row => row.IsNull("Exclude") || string.IsNullOrEmpty(row["Exclude"].ToString()))
				  .ToList()
				  .ForEach(row => row["Exclude"] = false);
			string sxml = Fields.GetXml("Name,Exclude");
			var (rowsInserted, tableName) = _sqlServerLib.RegisterExludedCDCFields(sxml);
			Log($"{rowsInserted} rows inserted into {tableName}", LogLevel.Debug);
		}
		private void updateFields() {
			DataTable Fields = dgvObject.DataSource as DataTable;
			_sqlServerLib.UpdateServerTable(Fields, "SELECT [Id],[IsExcluded]  FROM [dbo].[CDCObjectFields] ");
		}
		private void btnDeleteCDCSubscription_Click(object sender, EventArgs e) {
			_sqlServerLib.DeleteCDCObject(ObjectFromTopic(lbxObjects.Text));
		}
		#endregion buttons
		#region dgv
		/*
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
		*/
		private void SetupDataGridViewHeaders(string tn) {



			dgvObject.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
			dgvObject.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
			dgvObject.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.DarkBlue;
			dgvObject.TopLeftHeaderCell.Value = "Subscribe";
			dgvObject.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			dgvObject.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dgvObject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

			dgvSource.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
			dgvSource.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
			dgvSource.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			dgvSource.ColumnHeadersHeight = 50;

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


			dgvRelations.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
			dgvRelations.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.DarkBlue;
			dgvRelations.TopLeftHeaderCell.Value = "Subscribe";
			dgvRelations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			dgvRelations.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dgvRelations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


		}
		private void SetContainedControlsEnabled(Control control, bool enabled) {
			foreach (Control child in control.Controls) {
				child.Enabled = enabled;
				//SetContainedControlsEnabled(child, enabled);
			}
		}
		private void dgvRowCountChanged(object sender, EventArgs e) {

			switch (sender) {
				case DataGridView s when s == dgvSource:
				lblSourceList.Text = $"{dgvSource.Rows.Count} Salesforce objects";
				btnMoveRight.Enabled = dgvSource.Rows.Count > 0;
				btnCommitToDB.Enabled = dgvDestination.Rows.Count > 0;
				SetContainedControlsEnabled(grpPrimaryKey, btnCommitToDB.Enabled);
				break;

				case DataGridView s when s == dgvDestination:
				lblDestinationList.Text = $"{dgvDestination.Rows.Count} candidate Object";
				btnCommitToDB.Enabled = dgvDestination.Rows.Count > 0;
				SetContainedControlsEnabled(grpPrimaryKey, btnCommitToDB.Enabled);
				break;

			}
		}
		private void dgvObject_CellContentClick_1(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex < 0) return;
			if (dgvObject.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn) {
				dgvObject.CommitEdit(DataGridViewDataErrorContexts.Commit);
				DataTable dtObject = (DataTable)dgvObject.DataSource;
				dtObject.TableName = lbxObjects.Text;
				bool currentValue = Convert.ToBoolean(dgvObject.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
				if (dtObject.Columns.Contains("IsExcluded")) { // if it is a sql server table
					this.Invoke((Action)(() =>/* get only the field Names for the lbxFields*/  {
						List<string?> fields = dtObject.AsEnumerable()
							.Where(r => !(r["IsExcluded"] is bool b && b)) // filter out rows where Exclude == true 
							.Select(r => r["FieldName"]?.ToString())
							.Where(v => !string.IsNullOrEmpty(v))
							.ToList();
						rtxFieldsJsonArray.Text = JsonConvert.SerializeObject(fields);
						lblSelectedTable.Text = $"{dtObject.TableName} - filtered: {fields.Count}";
					}));
				} else {
					this.Invoke((Action)(() =>/* get only the field Names for the lbxFields*/  {
						List<string?> fields = dtObject.AsEnumerable()
							.Where(r => !(r["Exclude"] is bool b && b)) // filter out rows where Exclude == true 
							.Select(r => r["Name"]?.ToString())
							.Where(v => !string.IsNullOrEmpty(v))
							.ToList();
						rtxFieldsJsonArray.Text = JsonConvert.SerializeObject(fields);
						lblSelectedTable.Text = $"{dtObject.TableName} - filtered: {fields.Count}";
					}));
				}
			}
		}
		private async Task LoadSfObjectsAsync() {
			Log("LoadSFObjectsAsync", LogLevel.Debug);
			this.Invoke((Action)(() => Cursor.Current = Cursors.WaitCursor));
			await _semaphore.WaitAsync();
			try {
				_sourceTable = await _salesforceService.GetAllObjects();
				_dtRegistered = _sqlServerLib.GetAll_sfoTables();
				Log($"the registed from Sql server contains {_dtRegistered.Rows.Count} ", LogLevel.Debug);
				_sourceTable = _dtRegistered.ExcludeRegistered(_sourceTable, "name");
				_l.LogDebug($"after exluding registered={_dtRegistered.Rows.Count}");
				_sourceTable.DefaultView.Sort = "name ASC"; // Sort the source table by name
				lock (_dgvLock) {
					this.Invoke((Action)(() => {
						dgvSource.DataSource = _sourceTable;
						dgvSource.Columns[0].HeaderText = "Salesforce Objects";
						toolStripStatusLabel1.Text = $"Schema for {_sourceTable.TableName} having {_sourceTable.Rows.Count} rows loaded successfully.";
						if (_dtRegistered.Rows.Count > 0) {
							Log($"Registered rowcount {_dtRegistered.Rows.Count}", LogLevel.Debug);
							_destinationTable = _dtRegistered;
							dgvDestination.DataSource = _destinationTable;
						}
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
		public MainForm(IMemoryCache cache, ISalesforceService salesforceService, PubSubService pubSubService, IOptions<SalesforceConfig> config, SqlServerLib sqlServerLib, ILogger<MainForm> logger) {
			InitializeComponent();
			ToolTip tt = new ToolTip();
			tt.OwnerDraw = true;
			tt.InitialDelay = 1;
			tt.IsBalloon = false;
			tt.Draw += (s, e) => {
				e.Graphics.FillRectangle(Brushes.LightCyan, e.Bounds);
				e.Graphics.DrawRectangle(Pens.SteelBlue, e.Bounds);
				e.Graphics.DrawString(e.ToolTipText, SystemFonts.DefaultFont, Brushes.Black, e.Bounds);
			};
			string ttText = "Schemas of selected Salesforce objects must be persisted in SQL Server.\n" +
				"These objects are selected from the Objects tab.\n" +
				"Only objects marked as CDC Candidates are eligible for Pub/Sub operations.\n" +
				"To view all registered schemas, select 'Filter None' from the filter radio buttons.\n" +
				"In the Pub/Sub tab, choose the objects that require subscription.\n" +
				"Selected objects are automatically converted into gRPC topic format.\n" +
				"Select the excluded fields (IsExcluded) that should still be transferred\n" +
				"to SQL Server when a gRPC change event is received from Salesforce Service Bus.";


			tt.SetToolTip(lbxObjects, ttText);
			tt.SetToolTip(dgvObject, ttText);
			tt.SetToolTip(lblSelectedTable, ttText);


			_cache = cache;
			_salesforceService = salesforceService;
			_pubSubService = pubSubService;
			_config = config.Value;
			_sqlServerLib = sqlServerLib;
			_pubSubService.ProgressUpdated += PubSubService_ProgressUpdated;
			_pubSubService.CDCEvent += _pubSubService_ChangeEventReceived;
			if (_salesforceService is SalesforceService cs) {
				cs.AuthenticationAttempt += SalesforceService_AuthenticationAttempt;
			}
			_sqlServerLib.SqlEvent += (s, e) => {
				Log(e.Message, e.LogLevel);
			};
			_sqlServerLib = sqlServerLib ?? throw new ArgumentNullException(nameof(sqlServerLib));
			_l = logger ?? throw new ArgumentNullException(nameof(logger));
			_l.LogDebug("MainForm initialized.");
			_l.LogInformation("(logInformation)MainForm initialized.");
			_sqlServerLib.SqlObjectExist += SqlEventObjectExist;
			_sqlServerLib.SqlEvent += _sqlServerLib_SqlEvent;

		}


		private void Form1_Load(object sender, EventArgs e) {
			string savedTab = string.IsNullOrEmpty(Properties.Settings.Default.SelectedTab) ? "tbpSfObjects" : Properties.Settings.Default.SelectedTab;
			if (!string.IsNullOrEmpty(savedTab) && tabControl1.TabPages.ContainsKey(savedTab)) {
				TabPage tbp = tabControl1.TabPages[savedTab]!;
				tabControl1_Selected(sender, new TabControlEventArgs(tbp, tabControl1.SelectedIndex, TabControlAction.Selected));
			}
			lblPanel1.Parent = splitContainer1.Panel1;
			lblDestinationList.Text = "";
			SetupDataGridViewHeaders("");
		}
		private void SalesforceService_AuthenticationAttempt(object sender, SalesforceService.AuthenticationEventArgs e) {
			Invoke((Action)(() => {
				Log($"Authenticating: {e.Message}", e.LogLevel);
				btnAuthenticate.Enabled = false;
				toolStripStatusLabel1.Text = "Authenticating...";
			}));
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

		#region helpers
		private void LoadTopics(ListBox listBox, bool filtered) {
			listBox.Items.Clear();
			DataTable dataTable = filtered ? _sqlServerLib.Select("select * from dbo.ftcdcObjects()") : _sqlServerLib.GetAll_sfoTables();
			listBox.Items.AddRange(_sqlServerLib.GetChangeEventUrls(dataTable).ToArray());
		}
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
			if (destination.Rows.Count > 0) destination.Rows.Clear();
			destination.ColumnHeadersDefaultCellStyle = source.ColumnHeadersDefaultCellStyle.Clone();
			destination.ColumnHeadersHeight = source.ColumnHeadersHeight;
			destination.ColumnHeadersHeightSizeMode = source.ColumnHeadersHeightSizeMode;
			destination.EnableHeadersVisualStyles = source.EnableHeadersVisualStyles;
			destination.RowHeadersWidth = source.RowHeadersWidth;
			foreach (DataGridViewColumn col in source.Columns) { // copy columns
				DataGridViewColumn ncol = (DataGridViewColumn)col.Clone();
				ncol.Width = col.Width;                    // Set the exact width
				ncol.MinimumWidth = col.MinimumWidth;      // Set minimum width
				ncol.FillWeight = col.FillWeight;          // Set fill weight for proportional sizing
				ncol.Resizable = col.Resizable;            // Copy resizable property
				ncol.AutoSizeMode = col.AutoSizeMode;             // DataGridViewAutoSizeColumnMode.None;// Disable auto-sizing to preserve the exact width
				destination.Columns.Add(ncol);// Add the column to destination
			}
			destination.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			if (dgvDestination.Columns.Count > 0)
				dgvDestination.Columns[0].HeaderText = "CDC Candidates";

		}
		private async Task CommitObjectsAsDbArtefactsAsync(object sender, EventArgs e) {
			try {
				if (lbxObjects.SelectedItem == null) {// Validate selection
					toolStripStatusLabel1.Text = "Please select an object first.";
					return;
				}
				List<string> selectedFields = _config.Topics.GetFieldsToFilterByName((string)lbxObjects.SelectedItem);// Get selected fields
				_l.LogDebug($"Selected fields: {string.Join(", ", selectedFields)}");
				_destinationTable.TableName = "sfSObjects";// Configure destination table
				_l.LogDebug($"Processing {_destinationTable.Rows.Count} rows in destination table");
				foreach (DataRow row in _destinationTable.Rows) {// Process each row
					string tableName = row["name"]?.ToString();
					if (string.IsNullOrEmpty(tableName)) {
						_l.LogWarning("Encountered empty table name, skipping...");
						continue;
					}
					try {
						DataSet schema = await _salesforceService.GetObjectSchemaAsDataSetAsync(tableName);// Fetch schema and process
						lbxObjects.Items.Add($"/data/{tableName}ChangeEvent");
					} catch (Exception ex) {
						_l.LogError($"Failed to process table {tableName}: {ex.Message}");
						continue;
					}
				}
				toolStripStatusLabel1.Text = $"Processed {_destinationTable.Rows.Count} tables. Select tables and fields in pub/sub tab.";
			} catch (Exception ex) {
				_l.LogError($"Unexpected error during commit: {ex.Message}");
				toolStripStatusLabel1.Text = "Error processing tables. Check logs for details.";
				MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void setControlColor(Object sender, bool c) {

			switch (sender.GetType().Name.ToLower()) {
				case "button":
				var o = (Button)sender;
				o.BackColor = c ? Color.Green : Color.Brown;

				break;
				default:
				Log($"sender gettype().name={sender.GetType().Name}", LogLevel.Information);
				break;
			}

		}
		private (string jsonString, int count) tableColumnToJsonArray(DataTable dt, string sColumn, string fColumn) {

			List<string?> fields = dt.AsEnumerable()
								.Where(r => !(r[fColumn] is bool b && b))
									.Select(r => r[sColumn]?.ToString())
									.Where(v => !string.IsNullOrEmpty(v))
									.ToList();
			return (JsonConvert.SerializeObject(fields), fields.Count);
		}
		#endregion helpers
		#region litBoxes



		private async void lbxObjects_SelectedIndexChanged(object sender, EventArgs e) {
			if (lbxObjects.SelectedItem == null) return;
			this.UseWaitCursor = true;
			await _semaphore.WaitAsync();
			try {
				string selectedTopic = lbxObjects.SelectedItem.ToString();
				string selectedObject = ObjectFromTopic(selectedTopic);
				dgvObject.DataSource = null;
				dgvRelations.DataSource = null;
				_sqlServerLib.AssertCDCObjectExist(selectedObject);// this will fire event to set series of changes 
				switch (_retrieveFrom) {// set by AssertCDCObjectExists
					case enmRetrieveFrom.SalesForce:
					DataSet ds = await _salesforceService.GetObjectSchemaAsDataSetAsync(selectedObject);// async operations outside the lock
					DataTable dtObject = ds.Tables[selectedObject];
					toolStripStatusLabel1.Text = $" {ObjectFromTopic(selectedTopic)} has {dtObject.Rows.Count} fields.";
					if (rbtFilterSubscribed.Checked) {
						dtObject = await RemoveRowsNotInColumnList(dtObject, _config.Topics.GetFieldsToFilterByName(selectedTopic));
					}
					lock (_dgvLock) {// Synchronize UI updates with lock
						this.Invoke((Action)(() => {
							dtObject.Columns["Name"]!.SetOrdinal(0);
							dtObject.Columns["type"]!.SetOrdinal(1);
							dtObject.Columns["length"]!.SetOrdinal(2);
							DataColumn dc = dtObject.Columns.Add("Exclude", typeof(bool));
							dc.DefaultValue = false;
							dc.SetOrdinal(3);
							dgvObject.DataSource = dtObject;
							dgvObject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
							dgvObject.Columns["Exclude"]!.Width = 80;
							lblSelectedTable.Text = ObjectFromTopic(selectedTopic);
							dgvRelations.DataSource = ds.Tables["relations"];
							this.Invoke((Action)(() =>/* get only the field Names for the lbxFields*/  {
								lblSelectedTable.Text = ObjectFromTopic(selectedTopic);
								var r = tableColumnToJsonArray(dtObject, "Name", "Exclude");
								lblSelectedTable.Text += " - filtered:" + r.count.ToString();
								rtxFieldsJsonArray.Text = r.jsonString;
								//rtxFieldsJsonArray.Text = tableColumnToJsonArray(dtObject,"Name","Exclude");
								//	var r = tableColumnToJsonArray(ds.Tables["relations"], "Name", "Exclude");
								//	rtxFieldsJsonArray.Text = r.jsonString;
								//		lblSelectedTable.Text +=" - filtered:"+ r.count.ToString();
							}));
						}));
					}
					break;
					case enmRetrievedFrom.SqlServer:
					this.Invoke((Action)(() => {
						//string sqlSelect = $"select * from cdcObjectFields c join CDCObject p on c.CdcObject_Id=p.Id where p.ObjectName='{selectedObject}'";

						string sqlSelect = $"select * from ftcdcObjectFields('{selectedObject}',1)";
						dtObject = _sqlServerLib.Select(sqlSelect);
						dtObject.Columns["FieldName"]!.SetOrdinal(0);
						dgvObject.DataSource = dtObject;
						lblSelectedTable.Text = ObjectFromTopic(selectedTopic);
						toolStripStatusLabel1.Text = $"Object {selectedObject} already exists in the SQL Server.";
						btnDeleteCDCRegistration.Visible = true;
						btnRegisterFields.Text = "Update Fields";
						var r = tableColumnToJsonArray(dtObject, "FieldName", "IsExcluded");
						rtxFieldsJsonArray.Text = r.jsonString;
						lblSelectedTable.Text += " - filtered:" + r.count.ToString();
					}));
					break;
				}

			} catch (Exception ex) {
				this.Invoke((Action)(() => toolStripStatusLabel1.Text = $"Error: {ex.Message}"));
			}

			finally {
				_semaphore.Release();
				this.UseWaitCursor = false;
			}

		}

		#region lbxLog
		private void Log(string msg, LogLevel l, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string fp = "") {
			msg = $"{msg}:{callerMemberName}:{callerLineNumber}:{fp.Split('\\').Last()}";
			lbxLog.Items.Add(new LogItem(msg, l));
		}
		private void lbxLog_DrawItem(object sender, DrawItemEventArgs e) {
			if (e.Index < 0) return;
			Color bc;
			LogItem li = (LogItem)lbxLog.Items[e.Index];
			string text = lbxLog.Items[e.Index].ToString();
			switch (li.Level) {
				case LogLevel.Debug:
				bc = Color.Blue;
				break;
				case LogLevel.Information:
				bc = Color.Green;
				break;
				case LogLevel.Warning:
				bc = Color.Yellow;
				break;
				case LogLevel.Error:
				bc = Color.Red;
				break;
				default:
				bc = Color.Gray;
				break;
			}
			int iW = (int)e.Graphics.MeasureString(text, e.Font).Width;
			_lbxLogMw = iW > _lbxLogMw ? iW : _lbxLogMw;
			lbxLog.HorizontalExtent = _lbxLogMw;
			e.Graphics.FillRectangle(new SolidBrush(bc), e.Bounds);
			TextRenderer.DrawText(e.Graphics, text, e.Font, e.Bounds, e.ForeColor, TextFormatFlags.Left);
			e.DrawFocusRectangle();
		}
		private void lbxLog_Click(object sender, EventArgs e) {

			Clipboard.SetText(lbxLog.SelectedItem.ToString());
			statusStrip1.Text = $"Copied to clipboard: {Clipboard.GetText()}"; // optional logging

		}
		#endregion lbxLog
		#endregion listBoxes
		#region radio buttons
		private void filterChanged(object sender, EventArgs e) {
			dgvObject.DataSource = null;
			lblSelectedTable.Text = ""; // clethe dgvObject panel label
			rtxFieldsJsonArray.Text = "";
			var x = (RadioButton)sender;
			if (!x.Checked) return;
			bool showOnlySubscribed = (x == rbtFilterSubscribed);

			LoadTopics(lbxObjects, showOnlySubscribed);
			lblPanel1.Text = $"{lbxObjects.Items.Count} {(showOnlySubscribed == true ? "Subscribed" : "Registered")} CDC objects";

		}
		#endregion radio buttons
		#region tabs
		private async Task tabControl1_Selected(object sender, TabControlEventArgs e) {
			_l.LogDebug($"(logger) tabpage={e.TabPage.Name}");
			switch (e.TabPage.Name.ToLower()) {
				case "tbpsfobjects":
				if (!_sfsObjectsLoaded) {
					await LoadSfObjectsAsync();
					_sfsObjectsLoaded = true;
					CopySourceScheama(dgvSource, dgvDestination);
				}
				break;
				case "tbppubsub":

				LoadTopics(lbxObjects, rbtFilterSubscribed.Checked); // Load sfo Tables from sql server  topics into the listbox
				lblPanel1.Text = $"{lbxObjects.Items.Count} registered CDC objects";
				break;
				default: break;
			}
			tabControl1.SelectedTab = e.TabPage;
		}
		private async void TabControl1_Selected(object sender, TabControlEventArgs e) {
			await tabControl1_Selected(sender, e); // Call the async Task method
		}
		#endregion tabs

		#region tooltip

		#endregion tooltip

		private void btnCDCStartSubscription_Click(object sender, EventArgs e) {

		}
	}
	#region utility classes
	public class LogItem {
		public string Message { get; set; }
		public LogLevel Level { get; set; }
		public LogItem(string message, LogLevel level) {
			Message = message;
			Level = level;
		}
		public override string ToString() {
			return Message;
		}
	}



	#endregion utility classes

}





