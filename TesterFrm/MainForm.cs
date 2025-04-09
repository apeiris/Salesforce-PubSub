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

		private async void btnGetSfObjects_Click(object sender, EventArgs e) {

			await LoadSfObjectsAsync();
		}
		private void btnMoveRight_Click(object sender, EventArgs e) {

			if (dgvDestination.Columns.Count == 0) {
				CopySourceScheama(dgvSfObjects, dgvDestination);
			}

			var rowsToRemove = new List<DataRow>();
			foreach (DataGridViewRow row in dgvSfObjects.SelectedRows) {
				if (!row.IsNewRow) {
					// Get the underlying DataRow from the DataTable
					DataRowView drv = row.DataBoundItem as DataRowView;
					if (drv != null) {
						// Add to right grid
						dgvDestination.Rows.Add(drv["Name"]);
						// Mark for removal from source
						rowsToRemove.Add(drv.Row);
					}
				}
			}

			// Remove the rows from the source DataTable
			DataTable sT = (DataTable)dgvSfObjects.DataSource!;
			foreach (DataRow row in rowsToRemove) {
				sT.Rows.Remove(row);
			}

			// Refresh the left grid
			dgvSfObjects.Refresh();
		}
		private void btnClearDestination_Click(object sender, EventArgs e) {
			dgvDestination.Columns.Clear();
			dgvDestination.Rows.Clear();
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
		//private void DataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
		//	if (sender is DataGridView dgv) {
		//		Graphics g = e.Graphics;
		//		Rectangle rowBounds = dgv.GetRowDisplayRectangle(e.RowIndex, true);
		//		Point checkBoxLocation = new Point(
		//			rowBounds.Left + (dgv.RowHeadersWidth - 14) / 2,
		//			rowBounds.Top + (rowBounds.Height - 14) / 2
		//		);

		//		// Get or initialize the check states for this DataGridView
		//		if (!_rowHeaderCheckStatesMap.TryGetValue(dgv, out var rowHeaderCheckStates)) {
		//			rowHeaderCheckStates = new Dictionary<int, bool>();
		//			_rowHeaderCheckStatesMap[dgv] = rowHeaderCheckStates;
		//		}

		//		bool isChecked = rowHeaderCheckStates.ContainsKey(e.RowIndex) && rowHeaderCheckStates[e.RowIndex];
		//		CheckBoxState state = isChecked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
		//		CheckBoxRenderer.DrawCheckBox(g, checkBoxLocation, state);
		//	}
		//}
		private void SetupDataGridViewHeaders(string tn) {
			dgvObject.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
			dgvObject.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
			dgvObject.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.DarkBlue;
			dgvObject.TopLeftHeaderCell.Value = "Subscribe";

			dgvSfObjects.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
			dgvSfObjects.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
			dgvSfObjects.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.DarkBlue;

			dgvSfObjects.TopLeftHeaderCell.Value = "CDC";
			//dgvSfObjects.TopLeftHeaderCell.OwningColumn.Width = 40;
			dgvSfObjects.TopLeftHeaderCell.ToolTipText = "Susbcribe CDC events on this object.";

		}

		private void DataGridView_MouseClick(object sender, MouseEventArgs e) {
			if (sender is DataGridView dgv) {
				var hit = dgv.HitTest(e.X, e.Y);
				if (hit.Type == DataGridViewHitTestType.RowHeader) {
					int rowIndex = hit.RowIndex;

					Rectangle rowRect = dgv.GetRowDisplayRectangle(rowIndex, true);
					Rectangle checkboxRect = new Rectangle(
						rowRect.Left + (dgv.RowHeadersWidth - 14) / 2,
						rowRect.Top + (rowRect.Height - 14) / 2,
						14, 14
					);

					// Get or initialize the check states for this DataGridView
					if (!_rowHeaderCheckStatesMap.TryGetValue(dgv, out var rowHeaderCheckStates)) {
						rowHeaderCheckStates = new Dictionary<int, bool>();
						_rowHeaderCheckStatesMap[dgv] = rowHeaderCheckStates;
					}

					if (checkboxRect.Contains(e.Location)) {
						bool current = rowHeaderCheckStates.ContainsKey(rowIndex) && rowHeaderCheckStates[rowIndex];
						rowHeaderCheckStates[rowIndex] = !current;
						dgv.InvalidateRow(rowIndex);
					}
					if (dgv.Name.ToLower() == "dgvobject") {// huh lets remove or add the name column to lbxFields
						if (dgv.Columns["name"] != null) {
							string cellValue = dgv.Rows[rowIndex].Cells["name"].Value.ToString();
							if (lbxFields.Items.Contains(cellValue)) {
								lbxFields.Items.Remove(cellValue);
							} else {
								lbxFields.Items.Add(cellValue);
							}
						}
					} else if (dgv.Name.ToLower() == "dgvsfobjects") {
						if (dgv.Columns["name"] != null) {
							string cellValue = dgv.Rows[rowIndex].Cells["name"].Value.ToString();
							if (lbxFields.Items.Contains(cellValue)) {
								lbxFields.Items.Remove(cellValue);
							} else {
								lbxFields.Items.Add(cellValue);

							}
						}
					}
				}
			}
		}

		private void AttachHandlers(DataGridView dgv) {
			//dgv.RowPostPaint += DataGridView_RowPostPaint;
			dgv.MouseClick += DataGridView_MouseClick;
		}
		private async Task LoadSfObjectsAsync() {
			this.Invoke((Action)(() => Cursor.Current = Cursors.WaitCursor));
			await _semaphore.WaitAsync();
			try {
				DataTable dt = await _salesforceService.GetAllObjects();
				lock (_dgvLock) {
					this.Invoke((Action)(() => {
						dgvSfObjects.AutoGenerateColumns = true;
						dgvSfObjects.DataSource = null;
						dgvSfObjects.DataSource = dt;
						dgvSfObjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
						toolStripStatusLabel1.Text = $"Schema for {dt.TableName} having {dt.Rows.Count} rows loaded successfully.";
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
			AttachHandlers(dgvObject);
			AttachHandlers(dgvSfObjects);
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
			//dgvObject.RowPostPaint += dgvObject_RowPostPaint;
			//dgvObject.MouseClick += dgvObject_MouseClick;
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
					CopySourceScheama(dgvSfObjects, dgvDestination);
				}
				break;
				case "tbppubsub":
				LoadTopics(lbxObjects);	// Load topics into the listbox
				break;
				default:break;
			}
			tabControl1.SelectedTab = e.TabPage;
		}
		private async void TabControl1_Selected(object sender, TabControlEventArgs e) {
			await tabControl1_Selected(sender, e); // Call the async Task method
		}
		#endregion tabs






	}
}





