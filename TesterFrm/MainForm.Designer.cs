namespace TesterFrm
	{
	partial class MainForm
		{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
			{
			if (disposing && (components != null))
				{
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
			dgvSfObjects = new DataGridView();
			btnSubscribeToCDC = new Button();
			btnMoveRight = new Button();
			button3 = new Button();
			dgvDestination = new DataGridView();
			button2 = new Button();
			btnClearDestination = new Button();
			tbpPubSub = new TabPage();
			tableLayoutPanel1 = new TableLayoutPanel();
			tableLayoutPanel2 = new TableLayoutPanel();
			button1 = new Button();
			btnSubscribe = new Button();
			grpFilterOptions = new GroupBox();
			rbtFilterNone = new RadioButton();
			rbtFilterSubscribed = new RadioButton();
			btnCommit = new Button();
			dgvObject = new DataGridView();
			splitContainer1 = new SplitContainer();
			lbxObjects = new ListBox();
			lbxFields = new ListBox();
			lblPanel1 = new Label();
			lblPanel2 = new Label();
			tbpOAuth2 = new TabPage();
			tbpDescribeObject = new TabPage();
			tableLayoutPanel5 = new TableLayoutPanel();
			label1 = new Label();
			txtObjectName = new TextBox();
			label2 = new Label();
			statusStrip1.SuspendLayout();
			tabControl1.SuspendLayout();
			tbpSfObjects.SuspendLayout();
			tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvSfObjects).BeginInit();
			((System.ComponentModel.ISupportInitialize)dgvDestination).BeginInit();
			tbpPubSub.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			tableLayoutPanel2.SuspendLayout();
			grpFilterOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvObject).BeginInit();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
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
			statusStrip1.Size = new Size(1152, 22);
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
			tabControl1.Size = new Size(1152, 706);
			tabControl1.TabIndex = 4;
			tabControl1.Selected += TabControl1_Selected;
			// 
			// tbpSfObjects
			// 
			tbpSfObjects.Controls.Add(tableLayoutPanel3);
			tbpSfObjects.Location = new Point(4, 24);
			tbpSfObjects.Name = "tbpSfObjects";
			tbpSfObjects.Size = new Size(1144, 678);
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
			tableLayoutPanel3.Controls.Add(dgvSfObjects, 0, 1);
			tableLayoutPanel3.Controls.Add(btnSubscribeToCDC, 0, 6);
			tableLayoutPanel3.Controls.Add(btnMoveRight, 1, 2);
			tableLayoutPanel3.Controls.Add(dgvDestination, 2, 1);
			tableLayoutPanel3.Controls.Add(button2, 2, 6);
			tableLayoutPanel3.Controls.Add(btnClearDestination, 3, 6);
			tableLayoutPanel3.Controls.Add(button3, 1, 3);
			tableLayoutPanel3.Controls.Add(label2, 0, 0);
			tableLayoutPanel3.Location = new Point(55, 3);
			tableLayoutPanel3.Name = "tableLayoutPanel3";
			tableLayoutPanel3.RowCount = 7;
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 71.7791443F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 28.22086F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 137F));
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 136F));
			tableLayoutPanel3.Size = new Size(1023, 592);
			tableLayoutPanel3.TabIndex = 0;
			// 
			// dgvSfObjects
			// 
			dgvSfObjects.AllowUserToAddRows = false;
			dgvSfObjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
			dgvSfObjects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvSfObjects.Dock = DockStyle.Fill;
			dgvSfObjects.EditMode = DataGridViewEditMode.EditProgrammatically;
			dgvSfObjects.Location = new Point(3, 44);
			dgvSfObjects.Name = "dgvSfObjects";
			dgvSfObjects.ReadOnly = true;
			tableLayoutPanel3.SetRowSpan(dgvSfObjects, 5);
			dgvSfObjects.Size = new Size(470, 408);
			dgvSfObjects.TabIndex = 0;
			// 
			// btnSubscribeToCDC
			// 
			btnSubscribeToCDC.Location = new Point(3, 458);
			btnSubscribeToCDC.Name = "btnSubscribeToCDC";
			btnSubscribeToCDC.Size = new Size(108, 23);
			btnSubscribeToCDC.TabIndex = 0;
			btnSubscribeToCDC.Text = "Subscribe To CDC";
			btnSubscribeToCDC.UseVisualStyleBackColor = true;
			btnSubscribeToCDC.Click += btnGetSfObjects_Click;
			// 
			// btnMoveRight
			// 
			btnMoveRight.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
			btnMoveRight.Location = new Point(479, 176);
			btnMoveRight.Name = "btnMoveRight";
			btnMoveRight.Size = new Size(75, 40);
			btnMoveRight.TabIndex = 0;
			btnMoveRight.Text = ">";
			btnMoveRight.UseVisualStyleBackColor = true;
			btnMoveRight.Click += btnMoveRight_Click;
			// 
			// button3
			// 
			button3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
			button3.Location = new Point(479, 227);
			button3.Name = "button3";
			button3.Size = new Size(75, 40);
			button3.TabIndex = 1;
			button3.Text = "<";
			button3.UseVisualStyleBackColor = true;
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
			tableLayoutPanel3.SetRowSpan(dgvDestination, 5);
			dgvDestination.Size = new Size(449, 408);
			dgvDestination.TabIndex = 2;
			// 
			// button2
			// 
			button2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
			button2.Location = new Point(571, 458);
			button2.Name = "button2";
			button2.Size = new Size(173, 32);
			button2.TabIndex = 4;
			button2.Text = "Commit to Database";
			button2.UseVisualStyleBackColor = true;
			// 
			// btnClearDestination
			// 
			btnClearDestination.Location = new Point(750, 458);
			btnClearDestination.Name = "btnClearDestination";
			btnClearDestination.Size = new Size(108, 32);
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
			tbpPubSub.Size = new Size(1144, 678);
			tbpPubSub.TabIndex = 1;
			tbpPubSub.Text = "Pub/Sub";
			tbpPubSub.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.ColumnCount = 2;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 41.0369072F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58.9630928F));
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
			tableLayoutPanel1.Controls.Add(dgvObject, 1, 0);
			tableLayoutPanel1.Controls.Add(splitContainer1, 0, 0);
			tableLayoutPanel1.Controls.Add(lblPanel1, 1, 1);
			tableLayoutPanel1.Controls.Add(lblPanel2, 1, 2);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(3, 3);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 3;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 64.85671F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 35.1432877F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
			tableLayoutPanel1.Size = new Size(1138, 672);
			tableLayoutPanel1.TabIndex = 0;
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
			tableLayoutPanel2.Location = new Point(3, 433);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 4;
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 51.6129036F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 48.3870964F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tableLayoutPanel2.Size = new Size(283, 200);
			tableLayoutPanel2.TabIndex = 3;
			// 
			// button1
			// 
			button1.Location = new Point(132, 120);
			button1.Name = "button1";
			button1.Size = new Size(145, 44);
			button1.TabIndex = 2;
			button1.Text = "Clear";
			button1.UseVisualStyleBackColor = true;
			// 
			// btnSubscribe
			// 
			btnSubscribe.Location = new Point(3, 120);
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
			grpFilterOptions.Location = new Point(3, 65);
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
			btnCommit.Size = new Size(123, 26);
			btnCommit.TabIndex = 7;
			btnCommit.Text = "Commit";
			btnCommit.UseVisualStyleBackColor = true;
			btnCommit.Click += btnCommit_Click;
			// 
			// dgvObject
			// 
			dgvObject.AllowUserToAddRows = false;
			dgvObject.AllowUserToDeleteRows = false;
			dgvObject.AllowUserToResizeColumns = false;
			dgvObject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
			dgvObject.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvObject.Dock = DockStyle.Fill;
			dgvObject.Location = new Point(470, 3);
			dgvObject.Name = "dgvObject";
			tableLayoutPanel1.SetRowSpan(dgvObject, 2);
			dgvObject.Size = new Size(665, 657);
			dgvObject.TabIndex = 4;
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.Location = new Point(3, 3);
			splitContainer1.Name = "splitContainer1";
			splitContainer1.Orientation = Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(lbxObjects);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(lbxFields);
			splitContainer1.Size = new Size(461, 424);
			splitContainer1.SplitterDistance = 185;
			splitContainer1.TabIndex = 6;
			// 
			// lbxObjects
			// 
			lbxObjects.Dock = DockStyle.Fill;
			lbxObjects.FormattingEnabled = true;
			lbxObjects.Location = new Point(0, 0);
			lbxObjects.Name = "lbxObjects";
			lbxObjects.Size = new Size(461, 185);
			lbxObjects.TabIndex = 5;
			lbxObjects.SelectedIndexChanged += lbxObjects_SelectedIndexChanged;
			// 
			// lbxFields
			// 
			lbxFields.Dock = DockStyle.Fill;
			lbxFields.FormattingEnabled = true;
			lbxFields.Location = new Point(0, 0);
			lbxFields.Name = "lbxFields";
			lbxFields.Size = new Size(461, 235);
			lbxFields.TabIndex = 6;
			// 
			// lblPanel1
			// 
			lblPanel1.AutoSize = true;
			lblPanel1.Dock = DockStyle.Top;
			lblPanel1.Location = new Point(3, 663);
			lblPanel1.Name = "lblPanel1";
			lblPanel1.Size = new Size(461, 9);
			lblPanel1.TabIndex = 7;
			lblPanel1.Text = "lblPanel1";
			lblPanel1.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// lblPanel2
			// 
			lblPanel2.AutoSize = true;
			lblPanel2.Dock = DockStyle.Top;
			lblPanel2.Location = new Point(470, 663);
			lblPanel2.Name = "lblPanel2";
			lblPanel2.Size = new Size(665, 9);
			lblPanel2.TabIndex = 8;
			lblPanel2.Text = "Subscribed Fields:";
			lblPanel2.TextAlign = ContentAlignment.TopCenter;
			// 
			// tbpOAuth2
			// 
			tbpOAuth2.Controls.Add(btnGetTokenAsync);
			tbpOAuth2.Controls.Add(btnAuthenticate);
			tbpOAuth2.Controls.Add(txtResult);
			tbpOAuth2.Location = new Point(4, 24);
			tbpOAuth2.Name = "tbpOAuth2";
			tbpOAuth2.Padding = new Padding(3);
			tbpOAuth2.Size = new Size(1144, 678);
			tbpOAuth2.TabIndex = 0;
			tbpOAuth2.Text = "OAuth2";
			tbpOAuth2.UseVisualStyleBackColor = true;
			// 
			// tbpDescribeObject
			// 
			tbpDescribeObject.Controls.Add(tableLayoutPanel5);
			tbpDescribeObject.Location = new Point(4, 24);
			tbpDescribeObject.Name = "tbpDescribeObject";
			tbpDescribeObject.Size = new Size(1144, 678);
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
			tableLayoutPanel5.Size = new Size(1144, 678);
			tableLayoutPanel5.TabIndex = 0;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Dock = DockStyle.Fill;
			label1.Location = new Point(3, 0);
			label1.Name = "label1";
			label1.Size = new Size(110, 32);
			label1.TabIndex = 0;
			label1.Text = "Object Name";
			label1.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// txtObjectName
			// 
			txtObjectName.AcceptsReturn = true;
			txtObjectName.Dock = DockStyle.Fill;
			txtObjectName.Location = new Point(119, 3);
			txtObjectName.Name = "txtObjectName";
			txtObjectName.Size = new Size(1022, 23);
			txtObjectName.TabIndex = 1;
			txtObjectName.Text = "Account";
			// 
			// label2
			// 
			label2.AutoSize = true;
			tableLayoutPanel3.SetColumnSpan(label2, 5);
			label2.Dock = DockStyle.Fill;
			label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			label2.ForeColor = SystemColors.HotTrack;
			label2.Location = new Point(3, 0);
			label2.Name = "label2";
			label2.Size = new Size(1017, 41);
			label2.TabIndex = 5;
			label2.Text = "Choose the objects that require Change Data Capture (CDC) subscription.";
			label2.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1152, 728);
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
			((System.ComponentModel.ISupportInitialize)dgvSfObjects).EndInit();
			((System.ComponentModel.ISupportInitialize)dgvDestination).EndInit();
			tbpPubSub.ResumeLayout(false);
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			tableLayoutPanel2.ResumeLayout(false);
			grpFilterOptions.ResumeLayout(false);
			grpFilterOptions.PerformLayout();
			((System.ComponentModel.ISupportInitialize)dgvObject).EndInit();
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
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
		private Button button2;
		private DataGridView dgvSfObjects;
		private Button btnSubscribeToCDC;
		private Button button3;
		private DataGridView dgvDestination;
		private Button btnClearDestination;
		private Button btnMoveRight;
		private Label label2;
	}
	}