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
			tbpOAuth2 = new TabPage();
			tbpPubSub = new TabPage();
			tableLayoutPanel1 = new TableLayoutPanel();
			tableLayoutPanel2 = new TableLayoutPanel();
			button1 = new Button();
			btnSubscribe = new Button();
			grpFilterOptions = new GroupBox();
			rbtFilterNone = new RadioButton();
			rbtFilterSubscribed = new RadioButton();
			dgvObject = new DataGridView();
			splitContainer1 = new SplitContainer();
			lbxObjects = new ListBox();
			lbxFields = new ListBox();
			lblPanel1 = new Label();
			lblPanel2 = new Label();
			statusStrip1.SuspendLayout();
			tabControl1.SuspendLayout();
			tbpOAuth2.SuspendLayout();
			tbpPubSub.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			tableLayoutPanel2.SuspendLayout();
			grpFilterOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvObject).BeginInit();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
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
			tabControl1.Controls.Add(tbpOAuth2);
			tabControl1.Controls.Add(tbpPubSub);
			tabControl1.Dock = DockStyle.Fill;
			tabControl1.Location = new Point(0, 0);
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabControl1.Size = new Size(1152, 706);
			tabControl1.TabIndex = 4;
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
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34.71002F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65.2899857F));
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
			tableLayoutPanel1.Controls.Add(dgvObject, 1, 0);
			tableLayoutPanel1.Controls.Add(splitContainer1, 0, 0);
			tableLayoutPanel1.Controls.Add(lblPanel1, 1, 1);
			tableLayoutPanel1.Controls.Add(lblPanel2, 1, 2);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(3, 3);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 3;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 68.1749649F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 31.825037F));
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
			tableLayoutPanel2.Location = new Point(3, 455);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 4;
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 52.63158F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 47.36842F));
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
			button1.Click += button1_Click;
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
			grpFilterOptions.EnabledChanged += grpFilterOptions_EnabledChanged;
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
			// dgvObject
			// 
			dgvObject.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvObject.Dock = DockStyle.Fill;
			dgvObject.Location = new Point(398, 3);
			dgvObject.Name = "dgvObject";
			dgvObject.Size = new Size(737, 446);
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
			splitContainer1.Size = new Size(389, 446);
			splitContainer1.SplitterDistance = 195;
			splitContainer1.TabIndex = 6;
			// 
			// lbxObjects
			// 
			lbxObjects.Dock = DockStyle.Fill;
			lbxObjects.FormattingEnabled = true;
			lbxObjects.Location = new Point(0, 0);
			lbxObjects.Name = "lbxObjects";
			lbxObjects.Size = new Size(389, 195);
			lbxObjects.TabIndex = 5;
			lbxObjects.SelectedIndexChanged += lbxObjects_SelectedIndexChanged;
			// 
			// lbxFields
			// 
			lbxFields.Dock = DockStyle.Fill;
			lbxFields.FormattingEnabled = true;
			lbxFields.Location = new Point(0, 0);
			lbxFields.Name = "lbxFields";
			lbxFields.Size = new Size(389, 247);
			lbxFields.TabIndex = 6;
			// 
			// lblPanel1
			// 
			lblPanel1.AutoSize = true;
			lblPanel1.Dock = DockStyle.Top;
			lblPanel1.Location = new Point(398, 452);
			lblPanel1.Name = "lblPanel1";
			lblPanel1.Size = new Size(737, 15);
			lblPanel1.TabIndex = 7;
			lblPanel1.Text = "lblPanel1";
			lblPanel1.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// lblPanel2
			// 
			lblPanel2.AutoSize = true;
			lblPanel2.Dock = DockStyle.Top;
			lblPanel2.Location = new Point(398, 663);
			lblPanel2.Name = "lblPanel2";
			lblPanel2.Size = new Size(737, 9);
			lblPanel2.TabIndex = 8;
			lblPanel2.Text = "Subscribed Fields:";
			lblPanel2.TextAlign = ContentAlignment.TopCenter;
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
			tbpOAuth2.ResumeLayout(false);
			tbpOAuth2.PerformLayout();
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
	}
	}