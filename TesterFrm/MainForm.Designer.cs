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
			dgvSource = new DataGridView();
			tableLayoutPanel10 = new TableLayoutPanel();
			button26 = new Button();
			button25 = new Button();
			button24 = new Button();
			button23 = new Button();
			button22 = new Button();
			button21 = new Button();
			button20 = new Button();
			button19 = new Button();
			button18 = new Button();
			button17 = new Button();
			button16 = new Button();
			button15 = new Button();
			button14 = new Button();
			button13 = new Button();
			button12 = new Button();
			button11 = new Button();
			button10 = new Button();
			button9 = new Button();
			button8 = new Button();
			button7 = new Button();
			button6 = new Button();
			button5 = new Button();
			button4 = new Button();
			button3 = new Button();
			button2 = new Button();
			bsA = new Button();
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
			lblSourceList = new Label();
			tbpPubSub = new TabPage();
			tableLayoutPanel1 = new TableLayoutPanel();
			splitContainer1 = new SplitContainer();
			splitContainer2 = new SplitContainer();
			lbxObjects = new ListBox();
			lblPanel1 = new Label();
			rtxFieldsJsonArray = new RichTextBox();
			dgvObject = new DataGridView();
			lblSelectedTable = new Label();
			tableLayoutPanel2 = new TableLayoutPanel();
			btnSubscribe = new Button();
			button1 = new Button();
			btnRegisterFields = new Button();
			btnDeleteCDCRegistration = new Button();
			tableLayoutPanel4 = new TableLayoutPanel();
			dgvRelations = new DataGridView();
			lblRelations = new Label();
			grpFilterOptions = new GroupBox();
			rbtFilterNone = new RadioButton();
			rbtFilterSubscribed = new RadioButton();
			tbpOAuth2 = new TabPage();
			tbpDescribeObject = new TabPage();
			tableLayoutPanel5 = new TableLayoutPanel();
			label1 = new Label();
			txtObjectName = new TextBox();
			tbpEventLog = new TabPage();
			splitContainer3 = new SplitContainer();
			tableLayoutPanel6 = new TableLayoutPanel();
			tableLayoutPanel7 = new TableLayoutPanel();
			btnClearLog = new Button();
			lbxLog = new ListBox();
			rtfLog = new RichTextBox();
			tbpCDCEvents = new TabPage();
			tableLayoutPanel8 = new TableLayoutPanel();
			splitContainer4 = new SplitContainer();
			lbxCDCTopics = new ListBox();
			splitContainer5 = new SplitContainer();
			lbxCDCEvents = new ListBox();
			dgvFilteredFields = new DataGridView();
			tableLayoutPanel9 = new TableLayoutPanel();
			btnCDCStartSubscription = new Button();
			lblCDCStatus = new Label();
			statusStrip1.SuspendLayout();
			tabControl1.SuspendLayout();
			tbpSfObjects.SuspendLayout();
			tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvSource).BeginInit();
			tableLayoutPanel10.SuspendLayout();
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
			tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvRelations).BeginInit();
			grpFilterOptions.SuspendLayout();
			tbpOAuth2.SuspendLayout();
			tbpDescribeObject.SuspendLayout();
			tableLayoutPanel5.SuspendLayout();
			tbpEventLog.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
			splitContainer3.Panel1.SuspendLayout();
			splitContainer3.Panel2.SuspendLayout();
			splitContainer3.SuspendLayout();
			tableLayoutPanel6.SuspendLayout();
			tableLayoutPanel7.SuspendLayout();
			tbpCDCEvents.SuspendLayout();
			tableLayoutPanel8.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer4).BeginInit();
			splitContainer4.Panel1.SuspendLayout();
			splitContainer4.Panel2.SuspendLayout();
			splitContainer4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer5).BeginInit();
			splitContainer5.Panel1.SuspendLayout();
			splitContainer5.Panel2.SuspendLayout();
			splitContainer5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvFilteredFields).BeginInit();
			tableLayoutPanel9.SuspendLayout();
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
			tabControl1.Controls.Add(tbpEventLog);
			tabControl1.Controls.Add(tbpCDCEvents);
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
			tableLayoutPanel3.Controls.Add(dgvSource, 0, 1);
			tableLayoutPanel3.Controls.Add(tableLayoutPanel10, 0, 5);
			tableLayoutPanel3.Controls.Add(btnMoveRight, 1, 2);
			tableLayoutPanel3.Controls.Add(dgvDestination, 2, 1);
			tableLayoutPanel3.Controls.Add(btnMoveLeft, 1, 3);
			tableLayoutPanel3.Controls.Add(label2, 0, 0);
			tableLayoutPanel3.Controls.Add(lblDestinationList, 2, 5);
			tableLayoutPanel3.Controls.Add(grpPrimaryKey, 2, 6);
			tableLayoutPanel3.Controls.Add(lblSourceList, 0, 6);
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
			// tableLayoutPanel10
			// 
			tableLayoutPanel10.ColumnCount = 26;
			tableLayoutPanel3.SetColumnSpan(tableLayoutPanel10, 2);
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel10.Controls.Add(button26, 25, 0);
			tableLayoutPanel10.Controls.Add(button25, 24, 0);
			tableLayoutPanel10.Controls.Add(button24, 23, 0);
			tableLayoutPanel10.Controls.Add(button23, 22, 0);
			tableLayoutPanel10.Controls.Add(button22, 21, 0);
			tableLayoutPanel10.Controls.Add(button21, 20, 0);
			tableLayoutPanel10.Controls.Add(button20, 19, 0);
			tableLayoutPanel10.Controls.Add(button19, 18, 0);
			tableLayoutPanel10.Controls.Add(button18, 17, 0);
			tableLayoutPanel10.Controls.Add(button17, 16, 0);
			tableLayoutPanel10.Controls.Add(button16, 15, 0);
			tableLayoutPanel10.Controls.Add(button15, 14, 0);
			tableLayoutPanel10.Controls.Add(button14, 13, 0);
			tableLayoutPanel10.Controls.Add(button13, 12, 0);
			tableLayoutPanel10.Controls.Add(button12, 11, 0);
			tableLayoutPanel10.Controls.Add(button11, 10, 0);
			tableLayoutPanel10.Controls.Add(button10, 9, 0);
			tableLayoutPanel10.Controls.Add(button9, 8, 0);
			tableLayoutPanel10.Controls.Add(button8, 7, 0);
			tableLayoutPanel10.Controls.Add(button7, 6, 0);
			tableLayoutPanel10.Controls.Add(button6, 5, 0);
			tableLayoutPanel10.Controls.Add(button5, 4, 0);
			tableLayoutPanel10.Controls.Add(button4, 3, 0);
			tableLayoutPanel10.Controls.Add(button3, 2, 0);
			tableLayoutPanel10.Controls.Add(button2, 1, 0);
			tableLayoutPanel10.Controls.Add(bsA, 0, 0);
			tableLayoutPanel10.Location = new Point(3, 455);
			tableLayoutPanel10.Name = "tableLayoutPanel10";
			tableLayoutPanel10.RowCount = 1;
			tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel10.Size = new Size(562, 30);
			tableLayoutPanel10.TabIndex = 11;
			// 
			// button26
			// 
			button26.Location = new Point(503, 3);
			button26.Name = "button26";
			button26.Size = new Size(14, 23);
			button26.TabIndex = 25;
			button26.Text = "Z";
			button26.UseVisualStyleBackColor = true;
			button26.Click += bs_Click;
			// 
			// button25
			// 
			button25.Location = new Point(483, 3);
			button25.Name = "button25";
			button25.Size = new Size(14, 23);
			button25.TabIndex = 24;
			button25.Text = "Y";
			button25.UseVisualStyleBackColor = true;
			button25.Click += bs_Click;
			// 
			// button24
			// 
			button24.Location = new Point(463, 3);
			button24.Name = "button24";
			button24.Size = new Size(14, 23);
			button24.TabIndex = 23;
			button24.Text = "X";
			button24.UseVisualStyleBackColor = true;
			button24.Click += bs_Click;
			// 
			// button23
			// 
			button23.Location = new Point(443, 3);
			button23.Name = "button23";
			button23.Size = new Size(14, 23);
			button23.TabIndex = 22;
			button23.Text = "W";
			button23.UseVisualStyleBackColor = true;
			button23.Click += bs_Click;
			// 
			// button22
			// 
			button22.Location = new Point(423, 3);
			button22.Name = "button22";
			button22.Size = new Size(14, 23);
			button22.TabIndex = 21;
			button22.Text = "V";
			button22.UseVisualStyleBackColor = true;
			button22.Click += bs_Click;
			// 
			// button21
			// 
			button21.Location = new Point(403, 3);
			button21.Name = "button21";
			button21.Size = new Size(14, 23);
			button21.TabIndex = 20;
			button21.Text = "U";
			button21.UseVisualStyleBackColor = true;
			button21.Click += bs_Click;
			// 
			// button20
			// 
			button20.Location = new Point(383, 3);
			button20.Name = "button20";
			button20.Size = new Size(14, 23);
			button20.TabIndex = 19;
			button20.Text = "T";
			button20.UseVisualStyleBackColor = true;
			button20.Click += bs_Click;
			// 
			// button19
			// 
			button19.Location = new Point(363, 3);
			button19.Name = "button19";
			button19.Size = new Size(14, 23);
			button19.TabIndex = 18;
			button19.Text = "S";
			button19.UseVisualStyleBackColor = true;
			button19.Click += bs_Click;
			// 
			// button18
			// 
			button18.Location = new Point(343, 3);
			button18.Name = "button18";
			button18.Size = new Size(14, 23);
			button18.TabIndex = 17;
			button18.Text = "R";
			button18.UseVisualStyleBackColor = true;
			button18.Click += bs_Click;
			// 
			// button17
			// 
			button17.Location = new Point(323, 3);
			button17.Name = "button17";
			button17.Size = new Size(14, 23);
			button17.TabIndex = 16;
			button17.Text = "Q";
			button17.UseVisualStyleBackColor = true;
			button17.Click += bs_Click;
			// 
			// button16
			// 
			button16.Location = new Point(303, 3);
			button16.Name = "button16";
			button16.Size = new Size(14, 23);
			button16.TabIndex = 15;
			button16.Text = "P";
			button16.UseVisualStyleBackColor = true;
			button16.Click += bs_Click;
			// 
			// button15
			// 
			button15.Location = new Point(283, 3);
			button15.Name = "button15";
			button15.Size = new Size(14, 23);
			button15.TabIndex = 14;
			button15.Text = "O";
			button15.UseVisualStyleBackColor = true;
			button15.Click += bs_Click;
			// 
			// button14
			// 
			button14.Location = new Point(263, 3);
			button14.Name = "button14";
			button14.Size = new Size(14, 23);
			button14.TabIndex = 13;
			button14.Text = "N";
			button14.UseVisualStyleBackColor = true;
			button14.Click += bs_Click;
			// 
			// button13
			// 
			button13.Location = new Point(243, 3);
			button13.Name = "button13";
			button13.Size = new Size(14, 23);
			button13.TabIndex = 12;
			button13.Text = "M";
			button13.UseVisualStyleBackColor = true;
			button13.Click += bs_Click;
			// 
			// button12
			// 
			button12.Location = new Point(223, 3);
			button12.Name = "button12";
			button12.Size = new Size(14, 23);
			button12.TabIndex = 11;
			button12.Text = "L";
			button12.UseVisualStyleBackColor = true;
			button12.Click += bs_Click;
			// 
			// button11
			// 
			button11.Location = new Point(203, 3);
			button11.Name = "button11";
			button11.Size = new Size(14, 23);
			button11.TabIndex = 10;
			button11.Text = "K";
			button11.UseVisualStyleBackColor = true;
			button11.Click += bs_Click;
			// 
			// button10
			// 
			button10.Location = new Point(183, 3);
			button10.Name = "button10";
			button10.Size = new Size(14, 23);
			button10.TabIndex = 9;
			button10.Text = "J";
			button10.UseVisualStyleBackColor = true;
			button10.Click += bs_Click;
			// 
			// button9
			// 
			button9.Location = new Point(163, 3);
			button9.Name = "button9";
			button9.Size = new Size(14, 23);
			button9.TabIndex = 8;
			button9.Text = "I";
			button9.UseVisualStyleBackColor = true;
			button9.Click += bs_Click;
			// 
			// button8
			// 
			button8.Location = new Point(143, 3);
			button8.Name = "button8";
			button8.Size = new Size(14, 23);
			button8.TabIndex = 7;
			button8.Text = "H";
			button8.UseVisualStyleBackColor = true;
			button8.Click += bs_Click;
			// 
			// button7
			// 
			button7.Location = new Point(123, 3);
			button7.Name = "button7";
			button7.Size = new Size(14, 23);
			button7.TabIndex = 6;
			button7.Text = "G";
			button7.UseVisualStyleBackColor = true;
			button7.Click += bs_Click;
			// 
			// button6
			// 
			button6.Location = new Point(103, 3);
			button6.Name = "button6";
			button6.Size = new Size(14, 23);
			button6.TabIndex = 5;
			button6.Text = "F";
			button6.UseVisualStyleBackColor = true;
			button6.Click += bs_Click;
			// 
			// button5
			// 
			button5.Location = new Point(83, 3);
			button5.Name = "button5";
			button5.Size = new Size(14, 23);
			button5.TabIndex = 4;
			button5.Text = "E";
			button5.UseVisualStyleBackColor = true;
			button5.Click += bs_Click;
			// 
			// button4
			// 
			button4.Location = new Point(63, 3);
			button4.Name = "button4";
			button4.Size = new Size(14, 23);
			button4.TabIndex = 3;
			button4.Text = "D";
			button4.UseVisualStyleBackColor = true;
			button4.Click += bs_Click;
			// 
			// button3
			// 
			button3.Location = new Point(43, 3);
			button3.Name = "button3";
			button3.Size = new Size(14, 23);
			button3.TabIndex = 2;
			button3.Text = "C";
			button3.UseVisualStyleBackColor = true;
			button3.Click += bs_Click;
			// 
			// button2
			// 
			button2.Location = new Point(23, 3);
			button2.Name = "button2";
			button2.Size = new Size(14, 23);
			button2.TabIndex = 1;
			button2.Text = "B";
			button2.UseVisualStyleBackColor = true;
			button2.Click += bs_Click;
			// 
			// bsA
			// 
			bsA.Location = new Point(3, 3);
			bsA.Name = "bsA";
			bsA.Size = new Size(14, 23);
			bsA.TabIndex = 0;
			bsA.Text = "A";
			bsA.UseVisualStyleBackColor = true;
			bsA.Click += bs_Click;
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
			btnCommitToDB.Location = new Point(0, 35);
			btnCommitToDB.Name = "btnCommitToDB";
			btnCommitToDB.Size = new Size(173, 32);
			btnCommitToDB.TabIndex = 4;
			btnCommitToDB.Text = "Commit to Database";
			btnCommitToDB.UseVisualStyleBackColor = true;
			btnCommitToDB.Click += btnCommitToDB_Click;
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
			// lblSourceList
			// 
			lblSourceList.AutoSize = true;
			lblSourceList.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			lblSourceList.ForeColor = Color.Brown;
			lblSourceList.Location = new Point(3, 488);
			lblSourceList.Name = "lblSourceList";
			lblSourceList.Size = new Size(96, 21);
			lblSourceList.TabIndex = 6;
			lblSourceList.Text = "Placeholder";
			lblSourceList.TextAlign = ContentAlignment.MiddleCenter;
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
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.17601F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71.82399F));
			tableLayoutPanel1.Controls.Add(splitContainer1, 0, 1);
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 2);
			tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 1, 2);
			tableLayoutPanel1.Controls.Add(grpFilterOptions, 0, 0);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(3, 3);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 4;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 55.35445F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 44.64555F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
			tableLayoutPanel1.Size = new Size(1409, 672);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// splitContainer1
			// 
			tableLayoutPanel1.SetColumnSpan(splitContainer1, 2);
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.Location = new Point(3, 50);
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
			splitContainer1.Size = new Size(1403, 335);
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
			splitContainer2.Panel2.Controls.Add(rtxFieldsJsonArray);
			splitContainer2.Size = new Size(392, 335);
			splitContainer2.SplitterDistance = 167;
			splitContainer2.TabIndex = 7;
			// 
			// lbxObjects
			// 
			lbxObjects.Dock = DockStyle.Fill;
			lbxObjects.FormattingEnabled = true;
			lbxObjects.Location = new Point(0, 21);
			lbxObjects.Name = "lbxObjects";
			lbxObjects.Size = new Size(392, 146);
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
			lblPanel1.Size = new Size(149, 21);
			lblPanel1.TabIndex = 7;
			lblPanel1.Text = "CDC Subscriptions";
			lblPanel1.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// rtxFieldsJsonArray
			// 
			rtxFieldsJsonArray.Location = new Point(0, 3);
			rtxFieldsJsonArray.Name = "rtxFieldsJsonArray";
			rtxFieldsJsonArray.Size = new Size(389, 158);
			rtxFieldsJsonArray.TabIndex = 0;
			rtxFieldsJsonArray.Text = "";
			// 
			// dgvObject
			// 
			dgvObject.AllowUserToAddRows = false;
			dgvObject.AllowUserToDeleteRows = false;
			dgvObject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
			dgvObject.ColumnHeadersHeight = 40;
			dgvObject.Dock = DockStyle.Fill;
			dgvObject.Location = new Point(0, 21);
			dgvObject.Name = "dgvObject";
			dgvObject.Size = new Size(1007, 314);
			dgvObject.TabIndex = 4;
			dgvObject.CellContentClick += dgvObject_CellContentClick_1;
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
			tableLayoutPanel2.Controls.Add(btnSubscribe, 0, 4);
			tableLayoutPanel2.Controls.Add(button1, 1, 4);
			tableLayoutPanel2.Controls.Add(btnRegisterFields, 1, 0);
			tableLayoutPanel2.Controls.Add(btnDeleteCDCRegistration, 0, 0);
			tableLayoutPanel2.Location = new Point(3, 391);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 5;
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 51.6129036F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 48.3870964F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tableLayoutPanel2.Size = new Size(391, 269);
			tableLayoutPanel2.TabIndex = 3;
			// 
			// btnSubscribe
			// 
			btnSubscribe.Location = new Point(3, 221);
			btnSubscribe.Name = "btnSubscribe";
			btnSubscribe.Size = new Size(104, 44);
			btnSubscribe.TabIndex = 0;
			btnSubscribe.Text = "Subscribe";
			btnSubscribe.UseVisualStyleBackColor = true;
			btnSubscribe.Click += btnSubscribe_Click;
			// 
			// button1
			// 
			button1.Location = new Point(181, 221);
			button1.Name = "button1";
			button1.Size = new Size(145, 44);
			button1.TabIndex = 2;
			button1.Text = "Clear";
			button1.UseVisualStyleBackColor = true;
			// 
			// btnRegisterFields
			// 
			btnRegisterFields.BackColor = Color.Green;
			btnRegisterFields.ForeColor = Color.Yellow;
			btnRegisterFields.Location = new Point(181, 3);
			btnRegisterFields.Name = "btnRegisterFields";
			btnRegisterFields.Size = new Size(155, 44);
			btnRegisterFields.TabIndex = 7;
			btnRegisterFields.Text = "Subscribe";
			btnRegisterFields.UseVisualStyleBackColor = false;
			btnRegisterFields.Click += btnRegisterFields_Click;
			// 
			// btnDeleteCDCRegistration
			// 
			btnDeleteCDCRegistration.BackColor = Color.Green;
			btnDeleteCDCRegistration.ForeColor = Color.Yellow;
			btnDeleteCDCRegistration.Location = new Point(3, 3);
			btnDeleteCDCRegistration.Name = "btnDeleteCDCRegistration";
			btnDeleteCDCRegistration.Size = new Size(155, 44);
			btnDeleteCDCRegistration.TabIndex = 8;
			btnDeleteCDCRegistration.Text = "Delete Subscription";
			btnDeleteCDCRegistration.UseVisualStyleBackColor = false;
			btnDeleteCDCRegistration.Click += btnDeleteCDCSubscription_Click;
			// 
			// tableLayoutPanel4
			// 
			tableLayoutPanel4.ColumnCount = 1;
			tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tableLayoutPanel4.Controls.Add(dgvRelations, 0, 1);
			tableLayoutPanel4.Controls.Add(lblRelations, 0, 0);
			tableLayoutPanel4.Dock = DockStyle.Fill;
			tableLayoutPanel4.Location = new Point(400, 391);
			tableLayoutPanel4.Name = "tableLayoutPanel4";
			tableLayoutPanel4.RowCount = 2;
			tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 11.8942728F));
			tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 88.10573F));
			tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tableLayoutPanel4.Size = new Size(1006, 269);
			tableLayoutPanel4.TabIndex = 7;
			// 
			// dgvRelations
			// 
			dgvRelations.AllowUserToAddRows = false;
			dgvRelations.AllowUserToDeleteRows = false;
			dgvRelations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
			dgvRelations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvRelations.Dock = DockStyle.Fill;
			dgvRelations.Location = new Point(3, 34);
			dgvRelations.Name = "dgvRelations";
			dgvRelations.Size = new Size(1000, 232);
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
			// grpFilterOptions
			// 
			grpFilterOptions.Controls.Add(rbtFilterNone);
			grpFilterOptions.Controls.Add(rbtFilterSubscribed);
			grpFilterOptions.Location = new Point(3, 3);
			grpFilterOptions.Name = "grpFilterOptions";
			grpFilterOptions.Size = new Size(233, 41);
			grpFilterOptions.TabIndex = 6;
			grpFilterOptions.TabStop = false;
			grpFilterOptions.Text = "Filter";
			// 
			// rbtFilterNone
			// 
			rbtFilterNone.AutoSize = true;
			rbtFilterNone.Checked = true;
			rbtFilterNone.Location = new Point(6, 16);
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
			rbtFilterSubscribed.Location = new Point(66, 16);
			rbtFilterSubscribed.Name = "rbtFilterSubscribed";
			rbtFilterSubscribed.Size = new Size(110, 19);
			rbtFilterSubscribed.TabIndex = 5;
			rbtFilterSubscribed.Text = "Only subscribed";
			rbtFilterSubscribed.UseVisualStyleBackColor = true;
			rbtFilterSubscribed.CheckedChanged += filterChanged;
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
			// tbpEventLog
			// 
			tbpEventLog.Controls.Add(splitContainer3);
			tbpEventLog.Location = new Point(4, 24);
			tbpEventLog.Name = "tbpEventLog";
			tbpEventLog.Size = new Size(1415, 678);
			tbpEventLog.TabIndex = 4;
			tbpEventLog.Text = "Event Log";
			tbpEventLog.UseVisualStyleBackColor = true;
			// 
			// splitContainer3
			// 
			splitContainer3.Dock = DockStyle.Fill;
			splitContainer3.Location = new Point(0, 0);
			splitContainer3.Name = "splitContainer3";
			// 
			// splitContainer3.Panel1
			// 
			splitContainer3.Panel1.Controls.Add(tableLayoutPanel6);
			// 
			// splitContainer3.Panel2
			// 
			splitContainer3.Panel2.Controls.Add(rtfLog);
			splitContainer3.Size = new Size(1415, 678);
			splitContainer3.SplitterDistance = 762;
			splitContainer3.TabIndex = 0;
			// 
			// tableLayoutPanel6
			// 
			tableLayoutPanel6.ColumnCount = 1;
			tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tableLayoutPanel6.Controls.Add(tableLayoutPanel7, 0, 1);
			tableLayoutPanel6.Controls.Add(lbxLog, 0, 0);
			tableLayoutPanel6.Dock = DockStyle.Fill;
			tableLayoutPanel6.Location = new Point(0, 0);
			tableLayoutPanel6.Name = "tableLayoutPanel6";
			tableLayoutPanel6.RowCount = 2;
			tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 90.26549F));
			tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 9.734513F));
			tableLayoutPanel6.Size = new Size(762, 678);
			tableLayoutPanel6.TabIndex = 0;
			// 
			// tableLayoutPanel7
			// 
			tableLayoutPanel7.ColumnCount = 2;
			tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tableLayoutPanel7.Controls.Add(btnClearLog, 0, 0);
			tableLayoutPanel7.Location = new Point(3, 615);
			tableLayoutPanel7.Name = "tableLayoutPanel7";
			tableLayoutPanel7.RowCount = 2;
			tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
			tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
			tableLayoutPanel7.Size = new Size(200, 60);
			tableLayoutPanel7.TabIndex = 0;
			// 
			// btnClearLog
			// 
			btnClearLog.Location = new Point(3, 3);
			btnClearLog.Name = "btnClearLog";
			btnClearLog.Size = new Size(93, 23);
			btnClearLog.TabIndex = 0;
			btnClearLog.Text = "Clear log";
			btnClearLog.UseVisualStyleBackColor = true;
			btnClearLog.Click += btnClearLog_Click;
			// 
			// lbxLog
			// 
			lbxLog.Dock = DockStyle.Fill;
			lbxLog.DrawMode = DrawMode.OwnerDrawFixed;
			lbxLog.FormattingEnabled = true;
			lbxLog.HorizontalScrollbar = true;
			lbxLog.Location = new Point(3, 3);
			lbxLog.Name = "lbxLog";
			lbxLog.ScrollAlwaysVisible = true;
			lbxLog.Size = new Size(756, 606);
			lbxLog.TabIndex = 1;
			lbxLog.Click += lbxLog_Click;
			lbxLog.DrawItem += lbxLog_DrawItem;
			// 
			// rtfLog
			// 
			rtfLog.Dock = DockStyle.Fill;
			rtfLog.Location = new Point(0, 0);
			rtfLog.Name = "rtfLog";
			rtfLog.Size = new Size(649, 678);
			rtfLog.TabIndex = 0;
			rtfLog.Text = "";
			// 
			// tbpCDCEvents
			// 
			tbpCDCEvents.Controls.Add(tableLayoutPanel8);
			tbpCDCEvents.Location = new Point(4, 24);
			tbpCDCEvents.Name = "tbpCDCEvents";
			tbpCDCEvents.Size = new Size(1415, 678);
			tbpCDCEvents.TabIndex = 5;
			tbpCDCEvents.Text = "CDC Events";
			tbpCDCEvents.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel8
			// 
			tableLayoutPanel8.ColumnCount = 1;
			tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 73.5689F));
			tableLayoutPanel8.Controls.Add(splitContainer4, 0, 0);
			tableLayoutPanel8.Controls.Add(tableLayoutPanel9, 0, 1);
			tableLayoutPanel8.Controls.Add(lblCDCStatus, 0, 2);
			tableLayoutPanel8.Dock = DockStyle.Fill;
			tableLayoutPanel8.Location = new Point(0, 0);
			tableLayoutPanel8.Name = "tableLayoutPanel8";
			tableLayoutPanel8.RowCount = 3;
			tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 78.46608F));
			tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 21.5339241F));
			tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
			tableLayoutPanel8.Size = new Size(1415, 678);
			tableLayoutPanel8.TabIndex = 0;
			// 
			// splitContainer4
			// 
			splitContainer4.Dock = DockStyle.Fill;
			splitContainer4.Location = new Point(3, 3);
			splitContainer4.Name = "splitContainer4";
			// 
			// splitContainer4.Panel1
			// 
			splitContainer4.Panel1.Controls.Add(lbxCDCTopics);
			// 
			// splitContainer4.Panel2
			// 
			splitContainer4.Panel2.Controls.Add(splitContainer5);
			splitContainer4.Size = new Size(1409, 478);
			splitContainer4.SplitterDistance = 525;
			splitContainer4.TabIndex = 0;
			// 
			// lbxCDCTopics
			// 
			lbxCDCTopics.Dock = DockStyle.Fill;
			lbxCDCTopics.FormattingEnabled = true;
			lbxCDCTopics.HorizontalScrollbar = true;
			lbxCDCTopics.Location = new Point(0, 0);
			lbxCDCTopics.Name = "lbxCDCTopics";
			lbxCDCTopics.ScrollAlwaysVisible = true;
			lbxCDCTopics.Size = new Size(525, 478);
			lbxCDCTopics.TabIndex = 1;
			// 
			// splitContainer5
			// 
			splitContainer5.Dock = DockStyle.Fill;
			splitContainer5.Location = new Point(0, 0);
			splitContainer5.Name = "splitContainer5";
			// 
			// splitContainer5.Panel1
			// 
			splitContainer5.Panel1.Controls.Add(lbxCDCEvents);
			// 
			// splitContainer5.Panel2
			// 
			splitContainer5.Panel2.Controls.Add(dgvFilteredFields);
			splitContainer5.Size = new Size(880, 478);
			splitContainer5.SplitterDistance = 401;
			splitContainer5.TabIndex = 0;
			// 
			// lbxCDCEvents
			// 
			lbxCDCEvents.Dock = DockStyle.Fill;
			lbxCDCEvents.FormattingEnabled = true;
			lbxCDCEvents.HorizontalScrollbar = true;
			lbxCDCEvents.Location = new Point(0, 0);
			lbxCDCEvents.Name = "lbxCDCEvents";
			lbxCDCEvents.ScrollAlwaysVisible = true;
			lbxCDCEvents.Size = new Size(401, 478);
			lbxCDCEvents.TabIndex = 0;
			// 
			// dgvFilteredFields
			// 
			dgvFilteredFields.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvFilteredFields.Dock = DockStyle.Fill;
			dgvFilteredFields.Location = new Point(0, 0);
			dgvFilteredFields.Name = "dgvFilteredFields";
			dgvFilteredFields.Size = new Size(475, 478);
			dgvFilteredFields.TabIndex = 0;
			// 
			// tableLayoutPanel9
			// 
			tableLayoutPanel9.ColumnCount = 3;
			tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 235F));
			tableLayoutPanel9.Controls.Add(btnCDCStartSubscription, 0, 0);
			tableLayoutPanel9.Location = new Point(3, 487);
			tableLayoutPanel9.Name = "tableLayoutPanel9";
			tableLayoutPanel9.RowCount = 1;
			tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
			tableLayoutPanel9.Size = new Size(695, 41);
			tableLayoutPanel9.TabIndex = 1;
			// 
			// btnCDCStartSubscription
			// 
			btnCDCStartSubscription.Location = new Point(3, 3);
			btnCDCStartSubscription.Name = "btnCDCStartSubscription";
			btnCDCStartSubscription.Size = new Size(185, 35);
			btnCDCStartSubscription.TabIndex = 0;
			btnCDCStartSubscription.Text = "Start Subscription";
			btnCDCStartSubscription.UseVisualStyleBackColor = true;
			btnCDCStartSubscription.Click += btnCDCStartSubscription_Click;
			// 
			// lblCDCStatus
			// 
			lblCDCStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			lblCDCStatus.AutoSize = true;
			lblCDCStatus.Location = new Point(3, 617);
			lblCDCStatus.Name = "lblCDCStatus";
			lblCDCStatus.Size = new Size(66, 61);
			lblCDCStatus.TabIndex = 2;
			lblCDCStatus.Text = "CDC Status";
			lblCDCStatus.TextAlign = ContentAlignment.MiddleLeft;
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
			tableLayoutPanel10.ResumeLayout(false);
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
			tableLayoutPanel4.ResumeLayout(false);
			tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)dgvRelations).EndInit();
			grpFilterOptions.ResumeLayout(false);
			grpFilterOptions.PerformLayout();
			tbpOAuth2.ResumeLayout(false);
			tbpOAuth2.PerformLayout();
			tbpDescribeObject.ResumeLayout(false);
			tableLayoutPanel5.ResumeLayout(false);
			tableLayoutPanel5.PerformLayout();
			tbpEventLog.ResumeLayout(false);
			splitContainer3.Panel1.ResumeLayout(false);
			splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
			splitContainer3.ResumeLayout(false);
			tableLayoutPanel6.ResumeLayout(false);
			tableLayoutPanel7.ResumeLayout(false);
			tbpCDCEvents.ResumeLayout(false);
			tableLayoutPanel8.ResumeLayout(false);
			tableLayoutPanel8.PerformLayout();
			splitContainer4.Panel1.ResumeLayout(false);
			splitContainer4.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer4).EndInit();
			splitContainer4.ResumeLayout(false);
			splitContainer5.Panel1.ResumeLayout(false);
			splitContainer5.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer5).EndInit();
			splitContainer5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvFilteredFields).EndInit();
			tableLayoutPanel9.ResumeLayout(false);
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
		private TabPage tbpEventLog;
		private SplitContainer splitContainer3;
		private TableLayoutPanel tableLayoutPanel6;
		private TableLayoutPanel tableLayoutPanel7;
		private Button btnClearLog;
		private ListBox lbxLog;
		private RichTextBox rtfLog;
		private TabPage tbpCDCEvents;
		private TableLayoutPanel tableLayoutPanel8;
		private SplitContainer splitContainer4;
		private ListBox lbxCDCEvents;
		private ListBox lbxCDCTopics;
		private TableLayoutPanel tableLayoutPanel9;
		private Button btnCDCStartSubscription;
		private Label lblCDCStatus;
		private SplitContainer splitContainer5;
		private DataGridView dgvFilteredFields;
		private Button btnRegisterFields;
		private Button button2;
		private Button btnDeleteCDCRegistration;
		private RichTextBox rtxFieldsJsonArray;
		private TableLayoutPanel tableLayoutPanel10;
		private Button bsA;
		private Button button25;
		private Button button24;
		private Button button23;
		private Button button22;
		private Button button21;
		private Button button20;
		private Button button19;
		private Button button18;
		private Button button17;
		private Button button16;
		private Button button15;
		private Button button14;
		private Button button13;
		private Button button12;
		private Button button11;
		private Button button10;
		private Button button9;
		private Button button8;
		private Button button7;
		private Button button6;
		private Button button5;
		private Button button4;
		private Button button3;
		private Button button26;
	}
}