namespace ProcessAutomation.Main
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerCheckPayInProcess = new System.Windows.Forms.Timer(this.components);
            this.timerCheckChildProcess = new System.Windows.Forms.Timer(this.components);
            this.timerReadMessage = new System.Windows.Forms.Timer(this.components);
            this.tabPayIn = new System.Windows.Forms.TabPage();
            this.lblPayInProgress = new System.Windows.Forms.Label();
            this.btnStopPayIn = new System.Windows.Forms.Button();
            this.btnStartPayIn = new System.Windows.Forms.Button();
            this.webLayout = new System.Windows.Forms.WebBrowser();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.SettingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabReaMessage = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtTotal = new System.Windows.Forms.Label();
            this.dateExecuteTo = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dateExecuteFrom = new System.Windows.Forms.DateTimePicker();
            this.web_listBox_filter = new System.Windows.Forms.ListBox();
            this.cbStopAutoLoadMess = new System.Windows.Forms.CheckBox();
            this.txtAccount_filter = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.isError_filter = new System.Windows.Forms.ComboBox();
            this.isProcessed_filter = new System.Windows.Forms.ComboBox();
            this.isSatisfied_filter = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Web = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Money = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecievedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MessageContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsSatisfied = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsProcessed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DateExcute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Error = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnShowHistory = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblReadMessageProgress = new System.Windows.Forms.Label();
            this.lblErrorReadMessage = new System.Windows.Forms.Label();
            this.btnStopReadMessage = new System.Windows.Forms.Button();
            this.btnStartReadMessage = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.connectPortBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SerialPortCombobox = new System.Windows.Forms.ComboBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.SettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerCheckKeepSection = new System.Windows.Forms.Timer(this.components);
            this.timerCheckNewAccount = new System.Windows.Forms.Timer(this.components);
            this.tabPayIn.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.tabReaMessage.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPayIn
            // 
            this.tabPayIn.Controls.Add(this.lblPayInProgress);
            this.tabPayIn.Controls.Add(this.btnStopPayIn);
            this.tabPayIn.Controls.Add(this.btnStartPayIn);
            this.tabPayIn.Controls.Add(this.webLayout);
            this.tabPayIn.Controls.Add(this.menuStrip2);
            this.tabPayIn.Location = new System.Drawing.Point(4, 33);
            this.tabPayIn.Name = "tabPayIn";
            this.tabPayIn.Padding = new System.Windows.Forms.Padding(3);
            this.tabPayIn.Size = new System.Drawing.Size(1446, 761);
            this.tabPayIn.TabIndex = 1;
            this.tabPayIn.Text = "Nạp Tiền";
            this.tabPayIn.UseVisualStyleBackColor = true;
            // 
            // lblPayInProgress
            // 
            this.lblPayInProgress.AutoSize = true;
            this.lblPayInProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayInProgress.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblPayInProgress.Location = new System.Drawing.Point(219, 54);
            this.lblPayInProgress.Name = "lblPayInProgress";
            this.lblPayInProgress.Size = new System.Drawing.Size(170, 25);
            this.lblPayInProgress.TabIndex = 19;
            this.lblPayInProgress.Text = "Đang nạp tiền ...";
            // 
            // btnStopPayIn
            // 
            this.btnStopPayIn.Location = new System.Drawing.Point(22, 42);
            this.btnStopPayIn.Name = "btnStopPayIn";
            this.btnStopPayIn.Size = new System.Drawing.Size(175, 45);
            this.btnStopPayIn.TabIndex = 11;
            this.btnStopPayIn.Text = "Dừng nạp tiền";
            this.btnStopPayIn.UseVisualStyleBackColor = true;
            this.btnStopPayIn.Click += new System.EventHandler(this.btnStopPayIn_Click);
            // 
            // btnStartPayIn
            // 
            this.btnStartPayIn.Location = new System.Drawing.Point(22, 42);
            this.btnStartPayIn.Name = "btnStartPayIn";
            this.btnStartPayIn.Size = new System.Drawing.Size(175, 45);
            this.btnStartPayIn.TabIndex = 10;
            this.btnStartPayIn.Text = "Bắt đầu nạp tiền";
            this.btnStartPayIn.UseVisualStyleBackColor = true;
            this.btnStartPayIn.Click += new System.EventHandler(this.btnStartPayIn_Click);
            // 
            // webLayout
            // 
            this.webLayout.Location = new System.Drawing.Point(3, 126);
            this.webLayout.MinimumSize = new System.Drawing.Size(20, 20);
            this.webLayout.Name = "webLayout";
            this.webLayout.Size = new System.Drawing.Size(1440, 639);
            this.webLayout.TabIndex = 9;
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingToolStripMenuItem1});
            this.menuStrip2.Location = new System.Drawing.Point(3, 3);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(1440, 33);
            this.menuStrip2.TabIndex = 20;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // SettingToolStripMenuItem1
            // 
            this.SettingToolStripMenuItem1.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.SettingToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Black;
            this.SettingToolStripMenuItem1.Name = "SettingToolStripMenuItem1";
            this.SettingToolStripMenuItem1.Size = new System.Drawing.Size(85, 29);
            this.SettingToolStripMenuItem1.Text = "Cài Đặt";
            this.SettingToolStripMenuItem1.Click += new System.EventHandler(this.SettingToolStripMenuItem_Click);
            // 
            // tabReaMessage
            // 
            this.tabReaMessage.Controls.Add(this.groupBox3);
            this.tabReaMessage.Controls.Add(this.groupBox2);
            this.tabReaMessage.Controls.Add(this.groupBox1);
            this.tabReaMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabReaMessage.Location = new System.Drawing.Point(4, 33);
            this.tabReaMessage.Name = "tabReaMessage";
            this.tabReaMessage.Padding = new System.Windows.Forms.Padding(3);
            this.tabReaMessage.Size = new System.Drawing.Size(1446, 761);
            this.tabReaMessage.TabIndex = 0;
            this.tabReaMessage.Text = "Đọc Tin Nhắn";
            this.tabReaMessage.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtTotal);
            this.groupBox3.Controls.Add(this.dateExecuteTo);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.dateExecuteFrom);
            this.groupBox3.Controls.Add(this.web_listBox_filter);
            this.groupBox3.Controls.Add(this.cbStopAutoLoadMess);
            this.groupBox3.Controls.Add(this.txtAccount_filter);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.isError_filter);
            this.groupBox3.Controls.Add(this.isProcessed_filter);
            this.groupBox3.Controls.Add(this.isSatisfied_filter);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.Controls.Add(this.btnShowHistory);
            this.groupBox3.Location = new System.Drawing.Point(3, 121);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1439, 755);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tìm Kiếm";
            // 
            // txtTotal
            // 
            this.txtTotal.AutoSize = true;
            this.txtTotal.ForeColor = System.Drawing.Color.Red;
            this.txtTotal.Location = new System.Drawing.Point(890, 144);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(70, 24);
            this.txtTotal.TabIndex = 32;
            this.txtTotal.Text = "label10";
            // 
            // dateExecuteTo
            // 
            this.dateExecuteTo.CustomFormat = "dd/MM/yyyy";
            this.dateExecuteTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateExecuteTo.Location = new System.Drawing.Point(580, 139);
            this.dateExecuteTo.Name = "dateExecuteTo";
            this.dateExecuteTo.Size = new System.Drawing.Size(150, 29);
            this.dateExecuteTo.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(760, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 25);
            this.label6.TabIndex = 17;
            this.label6.Text = "Tổng tiền :";
            // 
            // dateExecuteFrom
            // 
            this.dateExecuteFrom.CustomFormat = "dd/MM/yyyy";
            this.dateExecuteFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateExecuteFrom.Location = new System.Drawing.Point(354, 139);
            this.dateExecuteFrom.Name = "dateExecuteFrom";
            this.dateExecuteFrom.Size = new System.Drawing.Size(189, 29);
            this.dateExecuteFrom.TabIndex = 22;
            // 
            // web_listBox_filter
            // 
            this.web_listBox_filter.FormattingEnabled = true;
            this.web_listBox_filter.HorizontalExtent = 4;
            this.web_listBox_filter.ItemHeight = 24;
            this.web_listBox_filter.Location = new System.Drawing.Point(10, 50);
            this.web_listBox_filter.Name = "web_listBox_filter";
            this.web_listBox_filter.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.web_listBox_filter.Size = new System.Drawing.Size(199, 100);
            this.web_listBox_filter.TabIndex = 31;
            // 
            // cbStopAutoLoadMess
            // 
            this.cbStopAutoLoadMess.AutoSize = true;
            this.cbStopAutoLoadMess.Location = new System.Drawing.Point(909, 28);
            this.cbStopAutoLoadMess.Name = "cbStopAutoLoadMess";
            this.cbStopAutoLoadMess.Size = new System.Drawing.Size(167, 28);
            this.cbStopAutoLoadMess.TabIndex = 28;
            this.cbStopAutoLoadMess.Text = "Tự động load tin";
            this.cbStopAutoLoadMess.UseVisualStyleBackColor = true;
            this.cbStopAutoLoadMess.CheckedChanged += new System.EventHandler(this.cbStopAutoLoadMess_CheckedChanged);
            // 
            // txtAccount_filter
            // 
            this.txtAccount_filter.Location = new System.Drawing.Point(354, 26);
            this.txtAccount_filter.Name = "txtAccount_filter";
            this.txtAccount_filter.Size = new System.Drawing.Size(189, 29);
            this.txtAccount_filter.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(230, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 24);
            this.label4.TabIndex = 22;
            this.label4.Text = "Ngày xử lý";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(230, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 24);
            this.label3.TabIndex = 22;
            this.label3.Text = "Tài khoản";
            // 
            // isError_filter
            // 
            this.isError_filter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.isError_filter.FormattingEnabled = true;
            this.isError_filter.Location = new System.Drawing.Point(689, 26);
            this.isError_filter.Name = "isError_filter";
            this.isError_filter.Size = new System.Drawing.Size(185, 32);
            this.isError_filter.TabIndex = 23;
            // 
            // isProcessed_filter
            // 
            this.isProcessed_filter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.isProcessed_filter.FormattingEnabled = true;
            this.isProcessed_filter.Location = new System.Drawing.Point(689, 82);
            this.isProcessed_filter.Name = "isProcessed_filter";
            this.isProcessed_filter.Size = new System.Drawing.Size(185, 32);
            this.isProcessed_filter.TabIndex = 23;
            // 
            // isSatisfied_filter
            // 
            this.isSatisfied_filter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.isSatisfied_filter.FormattingEnabled = true;
            this.isSatisfied_filter.Location = new System.Drawing.Point(354, 81);
            this.isSatisfied_filter.Name = "isSatisfied_filter";
            this.isSatisfied_filter.Size = new System.Drawing.Size(189, 32);
            this.isSatisfied_filter.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(576, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 24);
            this.label9.TabIndex = 22;
            this.label9.Text = "Lỗi";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(576, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 24);
            this.label8.TabIndex = 22;
            this.label8.Text = "Đã xử lý";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(238, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 24);
            this.label7.TabIndex = 22;
            this.label7.Text = "Hợp lệ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 24);
            this.label2.TabIndex = 22;
            this.label2.Text = "Web";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Web,
            this.Account,
            this.Money,
            this.RecievedDate,
            this.MessageContent,
            this.IsSatisfied,
            this.IsProcessed,
            this.DateExcute,
            this.Error,
            this.Id});
            this.dataGridView1.Location = new System.Drawing.Point(4, 174);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView1.RowTemplate.Height = 35;
            this.dataGridView1.Size = new System.Drawing.Size(1429, 460);
            this.dataGridView1.TabIndex = 21;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            // 
            // Web
            // 
            this.Web.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Web.DataPropertyName = "Web";
            this.Web.FillWeight = 50.09074F;
            this.Web.Frozen = true;
            this.Web.HeaderText = "Web";
            this.Web.Name = "Web";
            this.Web.ReadOnly = true;
            this.Web.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Web.Width = 50;
            // 
            // Account
            // 
            this.Account.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Account.DataPropertyName = "Account";
            this.Account.FillWeight = 289.0847F;
            this.Account.Frozen = true;
            this.Account.HeaderText = "Tài Khoản";
            this.Account.Name = "Account";
            this.Account.ReadOnly = true;
            this.Account.Width = 60;
            // 
            // Money
            // 
            this.Money.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Money.DataPropertyName = "Money";
            this.Money.FillWeight = 56.80839F;
            this.Money.Frozen = true;
            this.Money.HeaderText = "Số tiền";
            this.Money.Name = "Money";
            this.Money.ReadOnly = true;
            this.Money.Width = 70;
            // 
            // RecievedDate
            // 
            this.RecievedDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RecievedDate.DataPropertyName = "RecievedDate";
            this.RecievedDate.FillWeight = 108.8008F;
            this.RecievedDate.Frozen = true;
            this.RecievedDate.HeaderText = "Ngày Nhận";
            this.RecievedDate.Name = "RecievedDate";
            this.RecievedDate.ReadOnly = true;
            this.RecievedDate.Width = 120;
            // 
            // MessageContent
            // 
            this.MessageContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.MessageContent.DataPropertyName = "MessageContent";
            this.MessageContent.FillWeight = 108.8008F;
            this.MessageContent.Frozen = true;
            this.MessageContent.HeaderText = "Nội Dung";
            this.MessageContent.Name = "MessageContent";
            this.MessageContent.ReadOnly = true;
            this.MessageContent.Width = 600;
            // 
            // IsSatisfied
            // 
            this.IsSatisfied.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IsSatisfied.DataPropertyName = "IsSatisfied";
            this.IsSatisfied.FillWeight = 36.34379F;
            this.IsSatisfied.Frozen = true;
            this.IsSatisfied.HeaderText = "Hợp Lệ";
            this.IsSatisfied.Name = "IsSatisfied";
            this.IsSatisfied.ReadOnly = true;
            this.IsSatisfied.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsSatisfied.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsSatisfied.Width = 70;
            // 
            // IsProcessed
            // 
            this.IsProcessed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IsProcessed.DataPropertyName = "IsProcessed";
            this.IsProcessed.FillWeight = 32.46909F;
            this.IsProcessed.Frozen = true;
            this.IsProcessed.HeaderText = "Đã Xử Lý";
            this.IsProcessed.Name = "IsProcessed";
            this.IsProcessed.ReadOnly = true;
            this.IsProcessed.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsProcessed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsProcessed.Width = 70;
            // 
            // DateExcute
            // 
            this.DateExcute.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DateExcute.DataPropertyName = "DateExcute";
            this.DateExcute.FillWeight = 108.8008F;
            this.DateExcute.Frozen = true;
            this.DateExcute.HeaderText = "Ngày Xử Lý";
            this.DateExcute.Name = "DateExcute";
            this.DateExcute.ReadOnly = true;
            this.DateExcute.Width = 160;
            // 
            // Error
            // 
            this.Error.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Error.DataPropertyName = "Error";
            this.Error.FillWeight = 108.8008F;
            this.Error.Frozen = true;
            this.Error.HeaderText = "Lỗi";
            this.Error.Name = "Error";
            this.Error.ReadOnly = true;
            this.Error.Width = 250;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.Visible = false;
            // 
            // btnShowHistory
            // 
            this.btnShowHistory.Location = new System.Drawing.Point(909, 82);
            this.btnShowHistory.Name = "btnShowHistory";
            this.btnShowHistory.Size = new System.Drawing.Size(118, 33);
            this.btnShowHistory.TabIndex = 20;
            this.btnShowHistory.Text = "Tìm";
            this.btnShowHistory.UseVisualStyleBackColor = true;
            this.btnShowHistory.Click += new System.EventHandler(this.btnShowHistory_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblReadMessageProgress);
            this.groupBox2.Controls.Add(this.lblErrorReadMessage);
            this.groupBox2.Controls.Add(this.btnStopReadMessage);
            this.groupBox2.Controls.Add(this.btnStartReadMessage);
            this.groupBox2.Location = new System.Drawing.Point(600, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(835, 109);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Đọc Tin Nhắn";
            // 
            // lblReadMessageProgress
            // 
            this.lblReadMessageProgress.AutoSize = true;
            this.lblReadMessageProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReadMessageProgress.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblReadMessageProgress.Location = new System.Drawing.Point(231, 43);
            this.lblReadMessageProgress.Name = "lblReadMessageProgress";
            this.lblReadMessageProgress.Size = new System.Drawing.Size(211, 25);
            this.lblReadMessageProgress.TabIndex = 18;
            this.lblReadMessageProgress.Text = "Đang đọc tin nhắn ...";
            // 
            // lblErrorReadMessage
            // 
            this.lblErrorReadMessage.AutoSize = true;
            this.lblErrorReadMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorReadMessage.ForeColor = System.Drawing.Color.Red;
            this.lblErrorReadMessage.Location = new System.Drawing.Point(6, 75);
            this.lblErrorReadMessage.Name = "lblErrorReadMessage";
            this.lblErrorReadMessage.Size = new System.Drawing.Size(70, 25);
            this.lblErrorReadMessage.TabIndex = 17;
            this.lblErrorReadMessage.Text = "label2";
            // 
            // btnStopReadMessage
            // 
            this.btnStopReadMessage.Location = new System.Drawing.Point(6, 37);
            this.btnStopReadMessage.Name = "btnStopReadMessage";
            this.btnStopReadMessage.Size = new System.Drawing.Size(207, 38);
            this.btnStopReadMessage.TabIndex = 15;
            this.btnStopReadMessage.Text = "Dừng đọc tin nhắn";
            this.btnStopReadMessage.UseVisualStyleBackColor = true;
            this.btnStopReadMessage.Click += new System.EventHandler(this.btnStopReadMessage_Click);
            // 
            // btnStartReadMessage
            // 
            this.btnStartReadMessage.Location = new System.Drawing.Point(6, 37);
            this.btnStartReadMessage.Name = "btnStartReadMessage";
            this.btnStartReadMessage.Size = new System.Drawing.Size(207, 38);
            this.btnStartReadMessage.TabIndex = 14;
            this.btnStartReadMessage.Text = "Bắt đầu đọc tin nhắn";
            this.btnStartReadMessage.UseVisualStyleBackColor = true;
            this.btnStartReadMessage.Click += new System.EventHandler(this.btnStartReadMessage_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.connectPortBtn);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.SerialPortCombobox);
            this.groupBox1.Location = new System.Drawing.Point(7, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(587, 109);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kết Nối Thiết Bị";
            // 
            // connectPortBtn
            // 
            this.connectPortBtn.Location = new System.Drawing.Point(435, 39);
            this.connectPortBtn.Name = "connectPortBtn";
            this.connectPortBtn.Size = new System.Drawing.Size(124, 33);
            this.connectPortBtn.TabIndex = 8;
            this.connectPortBtn.Text = "Kết Nối";
            this.connectPortBtn.UseVisualStyleBackColor = true;
            this.connectPortBtn.Click += new System.EventHandler(this.connectPortBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "Cổng Kết Nối";
            // 
            // SerialPortCombobox
            // 
            this.SerialPortCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SerialPortCombobox.FormattingEnabled = true;
            this.SerialPortCombobox.Location = new System.Drawing.Point(154, 39);
            this.SerialPortCombobox.Name = "SerialPortCombobox";
            this.SerialPortCombobox.Size = new System.Drawing.Size(261, 32);
            this.SerialPortCombobox.TabIndex = 6;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabReaMessage);
            this.tabControl.Controls.Add(this.tabPayIn);
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(1, 5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1454, 798);
            this.tabControl.TabIndex = 10;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1440, 33);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // SettingToolStripMenuItem
            // 
            this.SettingToolStripMenuItem.Name = "SettingToolStripMenuItem";
            this.SettingToolStripMenuItem.Size = new System.Drawing.Size(12, 29);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1452, 802);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Bot";
            this.Load += new System.EventHandler(this.Main_Load);
            this.tabPayIn.ResumeLayout(false);
            this.tabPayIn.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.tabReaMessage.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerCheckPayInProcess;
        private System.Windows.Forms.Timer timerCheckChildProcess;
        private System.Windows.Forms.Timer timerReadMessage;
        private System.Windows.Forms.TabPage tabPayIn;
        private System.Windows.Forms.Button btnStopPayIn;
        private System.Windows.Forms.Button btnStartPayIn;
        private System.Windows.Forms.WebBrowser webLayout;
        private System.Windows.Forms.TabPage tabReaMessage;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button connectPortBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SerialPortCombobox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStopReadMessage;
        private System.Windows.Forms.Button btnStartReadMessage;
        private System.Windows.Forms.Label lblErrorReadMessage;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnShowHistory;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAccount_filter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox isSatisfied_filter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblReadMessageProgress;
        private System.Windows.Forms.Label lblPayInProgress;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem SettingToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem SettingToolStripMenuItem1;
        private System.Windows.Forms.ComboBox isError_filter;
        private System.Windows.Forms.ComboBox isProcessed_filter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbStopAutoLoadMess;
        private System.Windows.Forms.DataGridViewTextBoxColumn Web;
        private System.Windows.Forms.DataGridViewTextBoxColumn Account;
        private System.Windows.Forms.DataGridViewTextBoxColumn Money;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecievedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn MessageContent;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsSatisfied;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsProcessed;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateExcute;
        private System.Windows.Forms.DataGridViewTextBoxColumn Error;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.ListBox web_listBox_filter;
        private System.Windows.Forms.Timer timerCheckKeepSection;
        private System.Windows.Forms.DateTimePicker dateExecuteTo;
        private System.Windows.Forms.DateTimePicker dateExecuteFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label txtTotal;
        private System.Windows.Forms.Timer timerCheckNewAccount;
    }
}