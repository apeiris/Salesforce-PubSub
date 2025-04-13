namespace TesterFrm {
	partial class MainForm {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent() {
			btnAuthenticate = new Button();
			txtResult = new TextBox();
			btnGetTokenAsync = new Button();
			statusStrip1 = new StatusStrip();
			toolStripStatusLabel1 = new ToolStripStatusLabel();
			tabControl1 = new TabControl();
			tbpSfObjects = new TabPage();
			tableLayoutPanel3 = new TableLayoutPanel();
			lblSourceList = new Label();
			dgvSource = new DataGridView();
			btnMoveRight = new Button();
			dgvDestination = new DataGridView();
			btnMoveLeft = new Button();
			label2 = new Label();
			lblDestinationList = new Label();
			grpPrimaryKey = new GroupBox();
			label3 = new Label();
			textBox1 = new TextBox();
			chkAddIdentityField = new CheckBox();
			btnCommitToDB = new Button();
			btnClearDestination = new Button();
			tbpPubSub = new TabPage();
			tableLayoutPanel1 = new TableLayoutPanel();
			splitContainer1 = new SplitContainer();
			splitContainer2 = new SplitContainer();
			lbxObjects = new ListBox();
			lblPanel1 = new Label();
			lbxFields = new ListBox();
			dgvObject = new DataGridView();
			lblSelectedTable = new Label();
			tableLayoutPanel2 = new TableLayoutPanel();
			button1 = new Button();
			btnSubscribe = new Button();
			grpFilterOptions = new GroupBox();
			rbtFilterNone = new RadioButton();
			rbtFilterSubscribed = new RadioButton();
			btnCommit = new Button();
			tableLayoutPanel4 = new TableLayoutPanel();
			dgvRelations = new DataGridView();
			lblRelations = new Label();
			tbpOAuth2 = new TabPage();
			tbpDescribeObject = new TabPage();
			tableLayoutPanel5 = new TableLayoutPanel();
			label1 = new Label();
			txtObjectName = new TextBox();
			statusStrip1.SuspendLayout();
			tabControl1.SuspendLayout();
			tbpSfObjects.SuspendLayout();
			tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvSource).BeginInit();
			((System.ComponentModel.ISupportInitialize)dgvDestination).BeginInit();
			grpPrimaryKey.SuspendLayout();
			tbpPubSub.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
			splitContainer2.Panel1.SuspendLayout();
			splitContainer2.Panel2.SuspendLayout();
			splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvObject).BeginInit();
			tableLayoutPanel2.SuspendLayout();
			grpFilterOptions.SuspendLayout();
			tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvRelations).BeginInit();
			tbpOAuth2.SuspendLayout();
			tbpDescribeObject.SuspendLayout();
			tableLayoutPanel5.SuspendLayout();
			SuspendLayout();
			// 
			// btnAuthenticate
			// 
			btnAuthenticate.Location = new Point(389, 226);
			btnAuthenticate.Margin = new Padding(4, 3, 4, 3);
			btnAuthenticate.Name = "btnAuthenticate";
			btnAuthenticate.Size = new Size(100, 35);
			btnAuthenticate.TabIndex = 0;
			btnAuthenticate.Text = "Authenticate";
			btnAuthenticate.UseVisualStyleBackColor = true;
			btnAuthenticate.Click += btnAuthenticate_Click;
			// 
			// txtResult
			// 
			txtResult.Location = new Point(21, 20);
			txtResult.Margin = new Padding(4, 3, 4, 3);
			txtResult.Multiline = true;
			txtResult.Name = "txtResult";
			txtResult.Size = new Size(674, 115);
			txtResult.TabIndex = 1;
			// 
			// btnGetTokenAsync
			// 
			btnGetTokenAsync.Location = new Point(21, 226);
			btnGetTokenAsync.Margin = new Padding(4, 3, 4, 3);
			btnGetTokenAsync.Name = "btnGetTokenAsync";
			btnGetTokenAsync.Size = new Size(311, 35);
			btnGetTokenAsync.TabIndex = 2;
			btnGetTokenAsync.Text = "Task<token,iUrl,tenantId> = GetTokenAsync()";
			btnGetTokenAsync.UseVisualStyleBackColor = true;
			btnGetTokenAsync.Click += btnGetTokenAsync_Click;
			// 
			// statusStrip1
			// 
			statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
			statusStrip1.Location = new Point(0, 706);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Size = new Size(1423, 22);
			statusStrip1.TabIndex = 3;
			statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			toolStripStatusLabel1.Size = new Size(118, 17);
			toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// tabControl1
			// 
			tabControl1.Controls.Add(tbpSfObjects);
			tabControl1.Controls.Add(tbpPubSub);
			tabControl1.Controls.Add(tbpOAuth2);
			tabControl1.Controls.Add(tbpDescribeObject);
			tabControl1.Dock = DockStyle.Fill;
			tabControl1.Location = new Point(0, 0);
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabControl1.Size = new Size(1423, 706);
			tabControl1.TabIndex = 4;
			tabControl1.Selected += TabControl1_Selected;
			// 
			// tbpSfObjects
			// 
			tbpSfObjects.Controls.Add(tableLayoutPanel3);
			tbpSfObjects.Location = new Point(4, 24);
			tbpSfObjects.Name = "tbpSfObjects";
			tbpSfObjects.Size = new Size(1415, 678);
			tbpSfObjects.TabIndex = 2;
			tbpSfObjects.Text = "Objects";
			tbpSfObjects.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			tableLayoutPanel3.ColumnCount = 4;
			tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92F));
			tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 179F));
			tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 276F));
			tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tableLayoutPanel3.Controls.Add(lblSourceList, 0, 5);
			tableLayoutPanel3.Controls.Add(dgvSource, 0, 1);
			tableLayoutPanel3.Controls.Add(btnMoveRight, 1, 2);
			tableLayoutPanel3.Controls.Add(dgvDestination, 2, 1);
			tableLayoutPanel3.Controls.Add(btnMoveLeft, 1, 3);
			tableLayoutPanel3.Controls.Add(label2, 0, 0);
			tableLayoutPanel3.Controls.Add(lblDestinationList, 2, 5);
			tableLayoutPanel3.Controls.Add(grpPrimaryKey, 2, 6);
			tableLayoutPanel3.Location = new Point(55, 3);
			tableLayoutPanel3.Name = "tableLayoutPanel3";
			tableLayoutPanel3.RowCount = 8;
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 71.7791443F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 28.22086F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 149F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 73F));
			tableLayoutPanel3.Size = new Size(1023, 592);
			tableLayoutPanel3.TabIndex = 0;
			// 
			// lblSourceList
			// 
			lblSourceList.AutoSize = true;
			lblSourceList.Dock = DockStyle.Fill;
			lblSourceList.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			lblSourceList.ForeColor = Color.Brown;
			lblSourceList.Location = new Point(3, 452);
			lblSourceList.Name = "lblSourceList";
			lblSourceList.Size = new Size(470, 36);
			lblSourceList.TabIndex = 6;
			lblSourceList.Text = "Placeholder";
			lblSourceList.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// dgvSource
			// 
			dgvSource.AllowUserToAddRows = false;
			dgvSource.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
			dgvSource.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvSource.Dock = DockStyle.Fill;
			dgvSource.EditMode = DataGridViewEditMode.EditProgrammatically;
			dgvSource.Location = new Point(3, 44);
			dgvSource.Name = "dgvSource";
			dgvSource.ReadOnly = true;
			tableLayoutPanel3.SetRowSpan(dgvSource, 4);
			dgvSource.Size = new Size(470, 405);
			dgvSource.TabIndex = 0;
			dgvSource.RowsAdded += dgvRowCountChanged;
			dgvSource.RowsRemoved += dgvRowCountChanged;
			// 
			// btnMoveRight
			// 
			btnMoveRight.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
			btnMoveRight.Location = new Point(479, 199);
			btnMoveRight.Name = "btnMoveRight";
			btnMoveRight.Size = new Size(75, 40);
			btnMoveRight.TabIndex = 0;
			btnMoveRight.Text = ">";
			btnMoveRight.UseVisualStyleBackColor = true;
			btnMoveRight.Click += btnMoveRight_Click;
			// 
			// dgvDestination
			// 
			dgvDestination.AllowUserToAddRows = false;
			dgvDestination.AllowUserToDeleteRows = false;
			dgvDestination.AllowUserToOrderColumns = true;
			dgvDestination.AllowUserToResizeColumns = false;
			dgvDestination.AllowUserToResizeRows = false;
			dgvDestination.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
			dgvDestination.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			tableLayoutPanel3.SetColumnSpan(dgvDestination, 2);
			dgvDestination.Location = new Point(571, 44);
			dgvDestination.Name = "dgvDestination";
			tableLayoutPanel3.SetRowSpan(dgvDestination, 4);
			dgvDestination.Size = new Size(449, 352);
			dgvDestination.TabIndex = 2;
			dgvDestination.RowsAdded += dgvRowCountChanged;
			dgvDestination.RowsRemoved += dgvRowCountChanged;
			// 
			// btnMoveLeft
			// 
			btnMoveLeft.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
			btnMoveLeft.Location = new Point(479, 260);
			btnMoveLeft.Name = "btnMoveLeft";
			btnMoveLeft.Size = new Size(75, 40);
			btnMoveLeft.TabIndex = 1;
			btnMoveLeft.Text = "<";
			btnMoveLeft.UseVisualStyleBackColor = true;
			btnMoveLeft.Click += btnMoveLeft_Click;
			// 
			// label2
			// 
			label2.AutoSize = true;
			tableLayoutPanel3.SetColumnSpan(label2, 5);
			label2.Dock = DockStyle.Fill;
			label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			label2.ForeColor = Color.Blue;
			label2.Location = new Point(3, 0);
			label2.Name = "label2";
			label2.Size = new Size(1017, 41);
			label2.TabIndex = 5;
			label2.Text = "Choose the objects that require Change Data Capture (CDC) subscription.";
			label2.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// lblDestinationList
			// 
			lblDestinationList.AutoSize = true;
			tableLayoutPanel3.SetColumnSpan(lblDestinationList, 2);
			lblDestinationList.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			lblDestinationList.ForeColor = Color.Brown;
			lblDestinationList.Location = new Point(571, 452);
			lblDestinationList.Name = "lblDestinationList";
			lblDestinationList.Size = new Size(96, 21);
			lblDestinationList.TabIndex = 8;
			lblDestinationList.Text = "Placeholder";
			lblDestinationList.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// grpPrimaryKey
			// 
			grpPrimaryKey.AccessibleRole = AccessibleRole.None;
			tableLayoutPanel3.SetColumnSpan(grpPrimaryKey, 2);
			grpPrimaryKey.Controls.Add(label3);
			grpPrimaryKey.Controls.Add(textBox1);
			grpPrimaryKey.Controls.Add(chkAddIdentityField);
			grpPrimaryKey.Controls.Add(btnCommitToDB);
			grpPrimaryKey.Controls.Add(btnClearDestination);
			grpPrimaryKey.Location = new Point(571, 491);
			grpPrimaryKey.Name = "grpPrimaryKey";
			tableLayoutPanel3.SetRowSpan(grpPrimaryKey, 2);
			grpPrimaryKey.Size = new Size(449, 90);
			grpPrimaryKey.TabIndex = 10;
			grpPrimaryKey.TabStop = false;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(142, 12);
			label3.Name = "label3";
			label3.Size = new Size(60, 15);
			label3.TabIndex = 11;
			label3.Text = "Col.Name";
			// 
			// textBox1
			// 
			textBox1.AccessibleRole = AccessibleRole.None;
			textBox1.Location = new Point(208, 8);
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(100, 23);
			textBox1.TabIndex = 10;
			textBox1.Text = "Id";
			// 
			// chkAddIdentityField
			// 
			chkAddIdentityField.AccessibleRole = AccessibleRole.None;
			chkAddIdentityField.AutoSize = true;
			chkAddIdentityField.Checked = true;
			chkAddIdentityField.CheckState = CheckState.Checked;
			chkAddIdentityField.Location = new Point(6, 10);
			chkAddIdentityField.Name = "chkAddIdentityField";
			chkAddIdentityField.Size = new Size(114, 19);
			chkAddIdentityField.TabIndex = 9;
			chkAddIdentityField.Text = "Add Primary Key";
			chkAddIdentityField.UseVisualStyleBackColor = true;
			// 
			// btnCommitToDB
			// 
			btnCommitToDB.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
			btnCommitToDB.Location = new Point(6, 35);
			btnCommitToDB.Name = "btnCommitToDB";
			btnCommitToDB.Size = new Size(173, 32);
			btnCommitToDB.TabIndex = 4;
			btnCommitToDB.Text = "Commit to Database";
			btnCommitToDB.UseVisualStyleBackColor = true;
			btnCommitToDB.Click += btnCommitObjectsAsDbArtefacts;
			// 
			// btnClearDestination
			// 
			btnClearDestination.Location = new Point(208, 35);
			btnClearDestination.Name = "btnClearDestination";
			btnClearDestination.Size = new Size(86, 32);
			btnClearDestination.TabIndex = 3;
			btnClearDestination.Text = "Clear";
			btnClearDestination.UseVisualStyleBackColor = true;
			btnClearDestination.Click += btnClearDestination_Click;
			// 
			// tbpPubSub
			// 
			tbpPubSub.Controls.Add(tableLayoutPanel1);
			tbpPubSub.Location = new Point(4, 24);
			tbpPubSub.Name = "tbpPubSub";
			tbpPubSub.Padding = new Padding(3);
			tbpPubSub.Size = new Size(1415, 678);
			tbpPubSub.TabIndex = 1;
			tbpPubSub.Text = "Pub/Sub";
			tbpPubSub.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.ColumnCount = 2;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21.2569313F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78.7430649F));
			tableLayoutPanel1.Controls.Add(splitContainer1, 0, 0);
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
			tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 1, 1);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(3, 3);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 3;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 55.35445F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 44.64555F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tableLayoutPanel1.Size = new Size(1409, 672);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// splitContainer1
			// 
			tableLayoutPanel1.SetColumnSpan(splitContainer1, 2);
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.Location = new Point(3, 3);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(dgvObject);
			splitContainer1.Panel2.Controls.Add(lblSelectedTable);
			splitContainer1.Size = new Size(1403, 361);
			splitContainer1.SplitterDistance = 392;
			splitContainer1.TabIndex = 6;
			// 
			// splitContainer2
			// 
			splitContainer2.Dock = DockStyle.Fill;
			splitContainer2.Location = new Point(0, 0);
			splitContainer2.Name = "splitContainer2";
			splitContainer2.Orientation = Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			splitContainer2.Panel1.Controls.Add(lbxObjects);
			splitContainer2.Panel1.Controls.Add(lblPanel1);
			// 
			// splitContainer2.Panel2
			// 
			splitContainer2.Panel2.Controls.Add(lbxFields);
			splitContainer2.Size = new Size(392, 361);
			splitContainer2.SplitterDistance = 180;
			splitContainer2.TabIndex = 7;
			// 
			// lbxObjects
			// 
			lbxObjects.Dock = DockStyle.Fill;
			lbxObjects.FormattingEnabled = true;
			lbxObjects.Location = new Point(0, 21);
			lbxObjects.Name = "lbxObjects";
			lbxObjects.Size = new Size(392, 159);
			lbxObjects.TabIndex = 5;
			lbxObjects.SelectedIndexChanged += lbxObjects_SelectedIndexChanged;
			// 
			// lblPanel1
			// 
			lblPanel1.AutoSize = true;
			lblPanel1.Dock = DockStyle.Top;
			lblPanel1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			lblPanel1.Location = new Point(0, 0);
			lblPanel1.Name = "lblPanel1";
			lblPanel1.Size = new Size(82, 21);
			lblPanel1.TabIndex = 7;
			lblPanel1.Text = "lblPanel1";
			lblPanel1.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// lbxFields
			// 
			lbxFields.Dock = DockStyle.Fill;
			lbxFields.FormattingEnabled = true;
			lbxFields.Location = new Point(0, 0);
			lbxFields.Name = "lbxFields";
			lbxFields.Size = new Size(392, 177);
			lbxFields.TabIndex = 6;
			// 
			// dgvObject
			// 
			dgvObject.AllowUserToAddRows = false;
			dgvObject.AllowUserToDeleteRows = false;
			dgvObject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
			dgvObject.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvObject.Dock = DockStyle.Fill;
			dgvObject.Location = new Point(0, 21);
			dgvObject.Name = "dgvObject";
			dgvObject.Size = new Size(1007, 340);
			dgvObject.TabIndex = 4;
			// 
			// lblSelectedTable
			// 
			lblSelectedTable.AutoSize = true;
			lblSelectedTable.Dock = DockStyle.Top;
			lblSelectedTable.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			lblSelectedTable.Location = new Point(0, 0);
			lblSelectedTable.Name = "lblSelectedTable";
			lblSelectedTable.Size = new Size(136, 21);
			lblSelectedTable.TabIndex = 8;
			lblSelectedTable.Text = "lblSelectedTable";
			lblSelectedTable.TextAlign = ContentAlignment.TopCenter;
			// 
			// tableLayoutPanel2
			// 
			tableLayoutPanel2.ColumnCount = 2;
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45.58304F));
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54.41696F));
			tableLayoutPanel2.Controls.Add(button1, 1, 3);
			tableLayoutPanel2.Controls.Add(btnSubscribe, 0, 3);
			tableLayoutPanel2.Controls.Add(grpFilterOptions, 0, 2);
			tableLayoutPanel2.Controls.Add(btnCommit, 0, 0);
			tableLayoutPanel2.Location = new Point(3, 370);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 4;
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 51.6129036F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 48.3870964F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tableLayoutPanel2.Size = new Size(293, 231);
			tableLayoutPanel2.TabIndex = 3;
			// 
			// button1
			// 
			button1.Location = new Point(136, 151);
			button1.Name = "button1";
			button1.Size = new Size(145, 44);
			button1.TabIndex = 2;
			button1.Text = "Clear";
			button1.UseVisualStyleBackColor = true;
			// 
			// btnSubscribe
			// 
			btnSubscribe.Location = new Point(3, 151);
			btnSubscribe.Name = "btnSubscribe";
			btnSubscribe.Size = new Size(104, 44);
			btnSubscribe.TabIndex = 0;
			btnSubscribe.Text = "Subscribe";
			btnSubscribe.UseVisualStyleBackColor = true;
			btnSubscribe.Click += btnSubscribe_Click;
			// 
			// grpFilterOptions
			// 
			tableLayoutPanel2.SetColumnSpan(grpFilterOptions, 2);
			grpFilterOptions.Controls.Add(rbtFilterNone);
			grpFilterOptions.Controls.Add(rbtFilterSubscribed);
			grpFilterOptions.Location = new Point(3, 96);
			grpFilterOptions.Name = "grpFilterOptions";
			grpFilterOptions.Size = new Size(233, 49);
			grpFilterOptions.TabIndex = 6;
			grpFilterOptions.TabStop = false;
			grpFilterOptions.Text = "Filter";
			// 
			// rbtFilterNone
			// 
			rbtFilterNone.AutoSize = true;
			rbtFilterNone.Checked = true;
			rbtFilterNone.Location = new Point(6, 22);
			rbtFilterNone.Name = "rbtFilterNone";
			rbtFilterNone.Size = new Size(54, 19);
			rbtFilterNone.TabIndex = 4;
			rbtFilterNone.TabStop = true;
			rbtFilterNone.Text = "None";
			rbtFilterNone.UseVisualStyleBackColor = true;
			rbtFilterNone.CheckedChanged += filterChanged;
			// 
			// rbtFilterSubscribed
			// 
			rbtFilterSubscribed.AutoSize = true;
			rbtFilterSubscribed.Location = new Point(66, 22);
			rbtFilterSubscribed.Name = "rbtFilterSubscribed";
			rbtFilterSubscribed.Size = new Size(110, 19);
			rbtFilterSubscribed.TabIndex = 5;
			rbtFilterSubscribed.Text = "Only subscribed";
			rbtFilterSubscribed.UseVisualStyleBackColor = true;
			rbtFilterSubscribed.CheckedChanged += filterChanged;
			// 
			// btnCommit
			// 
			btnCommit.Location = new Point(3, 3);
			btnCommit.Name = "btnCommit";
			btnCommit.Size = new Size(123, 42);
			btnCommit.TabIndex = 7;
			btnCommit.Text = "Commit";
			btnCommit.UseVisualStyleBackColor = true;
			btnCommit.Click += btnCommit_Click;
			// 
			// tableLayoutPanel4
			// 
			tableLayoutPanel4.ColumnCount = 1;
			tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tableLayoutPanel4.Controls.Add(dgvRelations, 0, 1);
			tableLayoutPanel4.Controls.Add(lblRelations, 0, 0);
			tableLayoutPanel4.Dock = DockStyle.Fill;
			tableLayoutPanel4.Location = new Point(302, 370);
			tableLayoutPanel4.Name = "tableLayoutPanel4";
			tableLayoutPanel4.RowCount = 2;
			tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 11.8942728F));
			tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 88.10573F));
			tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tableLayoutPanel4.Size = new Size(1104, 290);
			tableLayoutPanel4.TabIndex = 7;
			// 
			// dgvRelations
			// 
			dgvRelations.AllowUserToAddRows = false;
			dgvRelations.AllowUserToDeleteRows = false;
			dgvRelations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
			dgvRelations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvRelations.Dock = DockStyle.Fill;
			dgvRelations.Location = new Point(3, 37);
			dgvRelations.Name = "dgvRelations";
			dgvRelations.Size = new Size(1098, 250);
			dgvRelations.TabIndex = 10;
			// 
			// lblRelations
			// 
			lblRelations.AutoSize = true;
			lblRelations.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			lblRelations.Location = new Point(3, 0);
			lblRelations.Name = "lblRelations";
			lblRelations.Size = new Size(101, 21);
			lblRelations.TabIndex = 9;
			lblRelations.Text = "lblRelations";
			lblRelations.TextAlign = ContentAlignment.TopCenter;
			// 
			// tbpOAuth2
			// 
			tbpOAuth2.Controls.Add(btnGetTokenAsync);
			tbpOAuth2.Controls.Add(btnAuthenticate);
			tbpOAuth2.Controls.Add(txtResult);
			tbpOAuth2.Location = new Point(4, 24);
			tbpOAuth2.Name = "tbpOAuth2";
			tbpOAuth2.Padding = new Padding(3);
			tbpOAuth2.Size = new Size(1415, 678);
			tbpOAuth2.TabIndex = 0;
			tbpOAuth2.Text = "OAuth2";
			tbpOAuth2.UseVisualStyleBackColor = true;
			// 
			// tbpDescribeObject
			// 
			tbpDescribeObject.Controls.Add(tableLayoutPanel5);
			tbpDescribeObject.Location = new Point(4, 24);
			tbpDescribeObject.Name = "tbpDescribeObject";
			tbpDescribeObject.Size = new Size(1415, 678);
			tbpDescribeObject.TabIndex = 3;
			tbpDescribeObject.Text = "Describe Object";
			tbpDescribeObject.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel5
			// 
			tableLayoutPanel5.ColumnCount = 2;
			tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78.14685F));
			tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 1028F));
			tableLayoutPanel5.Controls.Add(label1, 0, 0);
			tableLayoutPanel5.Controls.Add(txtObjectName, 1, 0);
			tableLayoutPanel5.Dock = DockStyle.Fill;
			tableLayoutPanel5.Location = new Point(0, 0);
			tableLayoutPanel5.Name = "tableLayoutPanel5";
			tableLayoutPanel5.RowCount = 3;
			tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 53.3333321F));
			tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 46.6666679F));
			tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 617F));
			tableLayoutPanel5.Size = new Size(1415, 678);
			tableLayoutPanel5.TabIndex = 0;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Dock = DockStyle.Fill;
			label1.Location = new Point(3, 0);
			label1.Name = "label1";
			label1.Size = new Size(381, 32);
			label1.TabIndex = 0;
			label1.Text = "Object Name";
			label1.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// txtObjectName
			// 
			txtObjectName.AcceptsReturn = true;
			txtObjectName.Dock = DockStyle.Fill;
			txtObjectName.Location = new Point(390, 3);
			txtObjectName.Name = "txtObjectName";
			txtObjectName.Size = new Size(1022, 23);
			txtObjectName.TabIndex = 1;
			txtObjectName.Text = "Account";
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1423, 728);
			Controls.Add(tabControl1);
			Controls.Add(statusStrip1);
			Margin = new Padding(4, 3, 4, 3);
			Name = "MainForm";
			Text = "Salesforce OAuth2 Authentication";
			FormClosing += Form1_FormClosing;
			Load += Form1_Load;
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			tabControl1.ResumeLayout(false);
			tbpSfObjects.ResumeLayout(false);
			tableLayoutPanel3.ResumeLayout(false);
			tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)dgvSource).EndInit();
			((System.ComponentModel.ISupportInitialize)dgvDestination).EndInit();
			grpPrimaryKey.ResumeLayout(false);
			grpPrimaryKey.PerformLayout();
			tbpPubSub.ResumeLayout(false);
			tableLayoutPanel1.ResumeLayout(false);
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			splitContainer2.Panel1.ResumeLayout(false);
			splitContainer2.Panel1.PerformLayout();
			splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
			splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvObject).EndInit();
			tableLayoutPanel2.ResumeLayout(false);
			grpFilterOptions.ResumeLayout(false);
			grpFilterOptions.PerformLayout();
			tableLayoutPanel4.ResumeLayout(false);
			tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)dgvRelations).EndInit();
			tbpOAuth2.ResumeLayout(false);
			tbpOAuth2.PerformLayout();
			tbpDescribeObject.ResumeLayout(false);
			tableLayoutPanel5.ResumeLayout(false);
			tableLayoutPanel5.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}


		private System.Windows.Forms.Button btnAuthenticate;
		private System.Windows.Forms.TextBox txtResult;
		private Button btnGetTokenAsync;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel toolStripStatusLabel1;
		private TabControl tabControl1;
		private TabPage tbpOAuth2;
		private TabPage tbpPubSub;
		private TableLayoutPanel tableLayoutPanel1;
		private Button btnSubscribe;
		private Button button1;
		private TableLayoutPanel tableLayoutPanel2;
		private DataGridView dgvObject;
		private ListBox lbxObjects;
		private SplitContainer splitContainer1;
		private ListBox lbxFields;
		private Label lblPanel1;
		private Label lblPanel2;
		private RadioButton rbtFilterSubscribed;
		private RadioButton rbtFilterNone;
		private GroupBox grpFilterOptions;
		private TabPage tbpSfObjects;
		private Button btnCommit;
		private TabPage tbpDescribeObject;
		private TableLayoutPanel tableLayoutPanel5;
		private Label label1;
		private TextBox txtObjectName;
		private TableLayoutPanel tableLayoutPanel3;
		private Button btnCommitToDB;
		private DataGridView dgvSource;
		private Button btnMoveLeft;
		private DataGridView dgvDestination;
		private Button btnClearDestination;
		private Button btnMoveRight;
		private Label label2;
		private Label lblSourceList;
		private Label lblDestinationList;
		private GroupBox grpPrimaryKey;
		private CheckBox chkAddIdentityField;
		private Label label3;
		private TextBox textBox1;
		private SplitContainer splitContainer2;
		private Label lblSelectedTable;
		private TableLayoutPanel tableLayoutPanel4;
		//	private DataGridView dgvRelations;
		private Label lblRelations;
		private DataGridView dgvRelations;
	}
}