namespace LAN_Spy.View {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.开始ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动所有模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止所有模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.扫描ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动扫描模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.扫描主机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.侦测主机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.毒化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动毒化模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始毒化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.监视ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动监视模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始监视ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SplitLine = new System.Windows.Forms.Label();
            this.HostListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.主机列表复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加到目标组1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加到目标组2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PoisonerTab = new System.Windows.Forms.TabPage();
            this.Target2ListContainer = new System.Windows.Forms.GroupBox();
            this.Target2List = new System.Windows.Forms.ListBox();
            this.Target1ListContainer = new System.Windows.Forms.GroupBox();
            this.Target1List = new System.Windows.Forms.ListBox();
            this.ScannerTab = new System.Windows.Forms.TabPage();
            this.HostList = new System.Windows.Forms.DataGridView();
            this.HostIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostMAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainTabContainer = new System.Windows.Forms.TabControl();
            this.WatcherTab = new System.Windows.Forms.TabPage();
            this.ConnectionList = new System.Windows.Forms.DataGridView();
            this.TargetListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.毒化目标复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.从目标组移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectionListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.连接列表复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.断开此连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SrcAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DstAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainMenu.SuspendLayout();
            this.HostListMenuStrip.SuspendLayout();
            this.PoisonerTab.SuspendLayout();
            this.Target2ListContainer.SuspendLayout();
            this.Target1ListContainer.SuspendLayout();
            this.ScannerTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HostList)).BeginInit();
            this.MainTabContainer.SuspendLayout();
            this.WatcherTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionList)).BeginInit();
            this.TargetListMenuStrip.SuspendLayout();
            this.ConnectionListMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始ToolStripMenuItem,
            this.扫描ToolStripMenuItem,
            this.毒化ToolStripMenuItem,
            this.监视ToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.MainMenu.Size = new System.Drawing.Size(876, 34);
            this.MainMenu.TabIndex = 0;
            // 
            // 开始ToolStripMenuItem
            // 
            this.开始ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动所有模块ToolStripMenuItem,
            this.停止所有模块ToolStripMenuItem,
            this.关于ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.开始ToolStripMenuItem.Name = "开始ToolStripMenuItem";
            this.开始ToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.开始ToolStripMenuItem.Text = "开始";
            // 
            // 启动所有模块ToolStripMenuItem
            // 
            this.启动所有模块ToolStripMenuItem.Name = "启动所有模块ToolStripMenuItem";
            this.启动所有模块ToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.启动所有模块ToolStripMenuItem.Size = new System.Drawing.Size(200, 30);
            this.启动所有模块ToolStripMenuItem.Text = "启动所有模块";
            this.启动所有模块ToolStripMenuItem.Click += new System.EventHandler(this.启动所有模块ToolStripMenuItem_Click);
            // 
            // 停止所有模块ToolStripMenuItem
            // 
            this.停止所有模块ToolStripMenuItem.Name = "停止所有模块ToolStripMenuItem";
            this.停止所有模块ToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.停止所有模块ToolStripMenuItem.Size = new System.Drawing.Size(200, 30);
            this.停止所有模块ToolStripMenuItem.Text = "停止所有模块";
            this.停止所有模块ToolStripMenuItem.Click += new System.EventHandler(this.停止所有模块ToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(200, 30);
            this.关于ToolStripMenuItem.Text = "关于...";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(200, 30);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 扫描ToolStripMenuItem
            // 
            this.扫描ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动扫描模块ToolStripMenuItem,
            this.扫描主机ToolStripMenuItem,
            this.侦测主机ToolStripMenuItem});
            this.扫描ToolStripMenuItem.Name = "扫描ToolStripMenuItem";
            this.扫描ToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.扫描ToolStripMenuItem.Text = "扫描";
            // 
            // 启动扫描模块ToolStripMenuItem
            // 
            this.启动扫描模块ToolStripMenuItem.Name = "启动扫描模块ToolStripMenuItem";
            this.启动扫描模块ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.启动扫描模块ToolStripMenuItem.Text = "启动模块";
            this.启动扫描模块ToolStripMenuItem.Click += new System.EventHandler(this.启动扫描模块ToolStripMenuItem_Click);
            // 
            // 扫描主机ToolStripMenuItem
            // 
            this.扫描主机ToolStripMenuItem.Enabled = false;
            this.扫描主机ToolStripMenuItem.Name = "扫描主机ToolStripMenuItem";
            this.扫描主机ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.扫描主机ToolStripMenuItem.Text = "扫描主机";
            this.扫描主机ToolStripMenuItem.Click += new System.EventHandler(this.扫描主机ToolStripMenuItem_Click);
            // 
            // 侦测主机ToolStripMenuItem
            // 
            this.侦测主机ToolStripMenuItem.Enabled = false;
            this.侦测主机ToolStripMenuItem.Name = "侦测主机ToolStripMenuItem";
            this.侦测主机ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.侦测主机ToolStripMenuItem.Text = "侦测主机";
            this.侦测主机ToolStripMenuItem.Click += new System.EventHandler(this.侦测主机ToolStripMenuItem_Click);
            // 
            // 毒化ToolStripMenuItem
            // 
            this.毒化ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动毒化模块ToolStripMenuItem,
            this.开始毒化ToolStripMenuItem});
            this.毒化ToolStripMenuItem.Name = "毒化ToolStripMenuItem";
            this.毒化ToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.毒化ToolStripMenuItem.Text = "毒化";
            // 
            // 启动毒化模块ToolStripMenuItem
            // 
            this.启动毒化模块ToolStripMenuItem.Name = "启动毒化模块ToolStripMenuItem";
            this.启动毒化模块ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.启动毒化模块ToolStripMenuItem.Text = "启动模块";
            this.启动毒化模块ToolStripMenuItem.Click += new System.EventHandler(this.启动毒化模块ToolStripMenuItem_Click);
            // 
            // 开始毒化ToolStripMenuItem
            // 
            this.开始毒化ToolStripMenuItem.Enabled = false;
            this.开始毒化ToolStripMenuItem.Name = "开始毒化ToolStripMenuItem";
            this.开始毒化ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.开始毒化ToolStripMenuItem.Text = "开始毒化";
            this.开始毒化ToolStripMenuItem.EnabledChanged += new System.EventHandler(this.开始毒化ToolStripMenuItem_EnabledChanged);
            // 
            // 监视ToolStripMenuItem
            // 
            this.监视ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动监视模块ToolStripMenuItem,
            this.开始监视ToolStripMenuItem});
            this.监视ToolStripMenuItem.Name = "监视ToolStripMenuItem";
            this.监视ToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.监视ToolStripMenuItem.Text = "监视";
            // 
            // 启动监视模块ToolStripMenuItem
            // 
            this.启动监视模块ToolStripMenuItem.Name = "启动监视模块ToolStripMenuItem";
            this.启动监视模块ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.启动监视模块ToolStripMenuItem.Text = "启动模块";
            this.启动监视模块ToolStripMenuItem.Click += new System.EventHandler(this.启动监视模块ToolStripMenuItem_Click);
            // 
            // 开始监视ToolStripMenuItem
            // 
            this.开始监视ToolStripMenuItem.Enabled = false;
            this.开始监视ToolStripMenuItem.Name = "开始监视ToolStripMenuItem";
            this.开始监视ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.开始监视ToolStripMenuItem.Text = "开始监视";
            this.开始监视ToolStripMenuItem.EnabledChanged += new System.EventHandler(this.开始监视ToolStripMenuItem_EnabledChanged);
            // 
            // SplitLine
            // 
            this.SplitLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SplitLine.Location = new System.Drawing.Point(0, 40);
            this.SplitLine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SplitLine.Name = "SplitLine";
            this.SplitLine.Size = new System.Drawing.Size(876, 3);
            this.SplitLine.TabIndex = 1;
            // 
            // HostListMenuStrip
            // 
            this.HostListMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.HostListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.主机列表复制ToolStripMenuItem,
            this.添加到目标组1ToolStripMenuItem,
            this.添加到目标组2ToolStripMenuItem});
            this.HostListMenuStrip.Name = "HostListMenuStrip";
            this.HostListMenuStrip.Size = new System.Drawing.Size(200, 88);
            this.HostListMenuStrip.Opened += new System.EventHandler(this.HostListMenuStrip_Opened);
            // 
            // 主机列表复制ToolStripMenuItem
            // 
            this.主机列表复制ToolStripMenuItem.Name = "主机列表复制ToolStripMenuItem";
            this.主机列表复制ToolStripMenuItem.Size = new System.Drawing.Size(199, 28);
            this.主机列表复制ToolStripMenuItem.Text = "复制选中项";
            this.主机列表复制ToolStripMenuItem.Click += new System.EventHandler(this.DataGridView复制ToolStripMenuItem_Click);
            // 
            // 添加到目标组1ToolStripMenuItem
            // 
            this.添加到目标组1ToolStripMenuItem.Name = "添加到目标组1ToolStripMenuItem";
            this.添加到目标组1ToolStripMenuItem.Size = new System.Drawing.Size(199, 28);
            this.添加到目标组1ToolStripMenuItem.Text = "添加到目标组1";
            this.添加到目标组1ToolStripMenuItem.Click += new System.EventHandler(this.添加到目标组1ToolStripMenuItem_Click);
            // 
            // 添加到目标组2ToolStripMenuItem
            // 
            this.添加到目标组2ToolStripMenuItem.Name = "添加到目标组2ToolStripMenuItem";
            this.添加到目标组2ToolStripMenuItem.Size = new System.Drawing.Size(199, 28);
            this.添加到目标组2ToolStripMenuItem.Text = "添加到目标组2";
            // 
            // PoisonerTab
            // 
            this.PoisonerTab.Controls.Add(this.Target2ListContainer);
            this.PoisonerTab.Controls.Add(this.Target1ListContainer);
            this.PoisonerTab.Location = new System.Drawing.Point(4, 28);
            this.PoisonerTab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PoisonerTab.Name = "PoisonerTab";
            this.PoisonerTab.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PoisonerTab.Size = new System.Drawing.Size(868, 474);
            this.PoisonerTab.TabIndex = 1;
            this.PoisonerTab.Text = "毒化目标";
            this.PoisonerTab.UseVisualStyleBackColor = true;
            // 
            // Target2ListContainer
            // 
            this.Target2ListContainer.Controls.Add(this.Target2List);
            this.Target2ListContainer.Location = new System.Drawing.Point(435, 9);
            this.Target2ListContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Target2ListContainer.Name = "Target2ListContainer";
            this.Target2ListContainer.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Target2ListContainer.Size = new System.Drawing.Size(420, 448);
            this.Target2ListContainer.TabIndex = 3;
            this.Target2ListContainer.TabStop = false;
            this.Target2ListContainer.Text = "目标组2";
            // 
            // Target2List
            // 
            this.Target2List.FormattingEnabled = true;
            this.Target2List.ItemHeight = 18;
            this.Target2List.Location = new System.Drawing.Point(9, 30);
            this.Target2List.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Target2List.Name = "Target2List";
            this.Target2List.Size = new System.Drawing.Size(400, 400);
            this.Target2List.TabIndex = 1;
            // 
            // Target1ListContainer
            // 
            this.Target1ListContainer.Controls.Add(this.Target1List);
            this.Target1ListContainer.Location = new System.Drawing.Point(9, 9);
            this.Target1ListContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Target1ListContainer.Name = "Target1ListContainer";
            this.Target1ListContainer.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Target1ListContainer.Size = new System.Drawing.Size(420, 448);
            this.Target1ListContainer.TabIndex = 2;
            this.Target1ListContainer.TabStop = false;
            this.Target1ListContainer.Text = "目标组1";
            // 
            // Target1List
            // 
            this.Target1List.FormattingEnabled = true;
            this.Target1List.ItemHeight = 18;
            this.Target1List.Location = new System.Drawing.Point(9, 30);
            this.Target1List.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Target1List.Name = "Target1List";
            this.Target1List.Size = new System.Drawing.Size(400, 400);
            this.Target1List.TabIndex = 0;
            // 
            // ScannerTab
            // 
            this.ScannerTab.Controls.Add(this.HostList);
            this.ScannerTab.Location = new System.Drawing.Point(4, 28);
            this.ScannerTab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ScannerTab.Name = "ScannerTab";
            this.ScannerTab.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ScannerTab.Size = new System.Drawing.Size(868, 474);
            this.ScannerTab.TabIndex = 0;
            this.ScannerTab.Text = "主机列表";
            this.ScannerTab.UseVisualStyleBackColor = true;
            // 
            // HostList
            // 
            this.HostList.AllowUserToAddRows = false;
            this.HostList.AllowUserToDeleteRows = false;
            this.HostList.AllowUserToResizeRows = false;
            this.HostList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.HostList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.HostList.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.HostList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.HostList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.HostList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HostIP,
            this.HostMAC});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.HostList.DefaultCellStyle = dataGridViewCellStyle2;
            this.HostList.Location = new System.Drawing.Point(9, 9);
            this.HostList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.HostList.Name = "HostList";
            this.HostList.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.HostList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.HostList.RowHeadersVisible = false;
            this.HostList.RowTemplate.Height = 23;
            this.HostList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.HostList.Size = new System.Drawing.Size(846, 448);
            this.HostList.TabIndex = 2;
            // 
            // HostIP
            // 
            this.HostIP.HeaderText = "主机IP";
            this.HostIP.Name = "HostIP";
            this.HostIP.ReadOnly = true;
            // 
            // HostMAC
            // 
            this.HostMAC.HeaderText = "主机MAC";
            this.HostMAC.Name = "HostMAC";
            this.HostMAC.ReadOnly = true;
            // 
            // MainTabContainer
            // 
            this.MainTabContainer.Controls.Add(this.ScannerTab);
            this.MainTabContainer.Controls.Add(this.PoisonerTab);
            this.MainTabContainer.Controls.Add(this.WatcherTab);
            this.MainTabContainer.Location = new System.Drawing.Point(0, 40);
            this.MainTabContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MainTabContainer.Name = "MainTabContainer";
            this.MainTabContainer.SelectedIndex = 0;
            this.MainTabContainer.Size = new System.Drawing.Size(876, 506);
            this.MainTabContainer.TabIndex = 3;
            // 
            // WatcherTab
            // 
            this.WatcherTab.Controls.Add(this.ConnectionList);
            this.WatcherTab.Location = new System.Drawing.Point(4, 28);
            this.WatcherTab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.WatcherTab.Name = "WatcherTab";
            this.WatcherTab.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.WatcherTab.Size = new System.Drawing.Size(868, 474);
            this.WatcherTab.TabIndex = 2;
            this.WatcherTab.Text = "连接列表";
            this.WatcherTab.UseVisualStyleBackColor = true;
            // 
            // ConnectionList
            // 
            this.ConnectionList.AllowUserToAddRows = false;
            this.ConnectionList.AllowUserToDeleteRows = false;
            this.ConnectionList.AllowUserToResizeRows = false;
            this.ConnectionList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ConnectionList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.ConnectionList.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ConnectionList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.ConnectionList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConnectionList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SrcAddress,
            this.DstAddress});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConnectionList.DefaultCellStyle = dataGridViewCellStyle5;
            this.ConnectionList.Location = new System.Drawing.Point(9, 9);
            this.ConnectionList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ConnectionList.Name = "ConnectionList";
            this.ConnectionList.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ConnectionList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.ConnectionList.RowHeadersVisible = false;
            this.ConnectionList.RowTemplate.Height = 23;
            this.ConnectionList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ConnectionList.Size = new System.Drawing.Size(846, 448);
            this.ConnectionList.TabIndex = 3;
            // 
            // TargetListMenuStrip
            // 
            this.TargetListMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.TargetListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.毒化目标复制ToolStripMenuItem,
            this.从目标组移除ToolStripMenuItem});
            this.TargetListMenuStrip.Name = "TargetListMenuStrip";
            this.TargetListMenuStrip.Size = new System.Drawing.Size(189, 60);
            // 
            // 毒化目标复制ToolStripMenuItem
            // 
            this.毒化目标复制ToolStripMenuItem.Name = "毒化目标复制ToolStripMenuItem";
            this.毒化目标复制ToolStripMenuItem.Size = new System.Drawing.Size(188, 28);
            this.毒化目标复制ToolStripMenuItem.Text = "复制选中项";
            // 
            // 从目标组移除ToolStripMenuItem
            // 
            this.从目标组移除ToolStripMenuItem.Name = "从目标组移除ToolStripMenuItem";
            this.从目标组移除ToolStripMenuItem.Size = new System.Drawing.Size(188, 28);
            this.从目标组移除ToolStripMenuItem.Text = "从目标组移除";
            // 
            // ConnectionListMenuStrip
            // 
            this.ConnectionListMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ConnectionListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接列表复制ToolStripMenuItem,
            this.断开此连接ToolStripMenuItem});
            this.ConnectionListMenuStrip.Name = "ConnectionListMenuStrip";
            this.ConnectionListMenuStrip.Size = new System.Drawing.Size(171, 60);
            // 
            // 连接列表复制ToolStripMenuItem
            // 
            this.连接列表复制ToolStripMenuItem.Name = "连接列表复制ToolStripMenuItem";
            this.连接列表复制ToolStripMenuItem.Size = new System.Drawing.Size(170, 28);
            this.连接列表复制ToolStripMenuItem.Text = "复制选中项";
            this.连接列表复制ToolStripMenuItem.Click += new System.EventHandler(this.DataGridView复制ToolStripMenuItem_Click);
            // 
            // 断开此连接ToolStripMenuItem
            // 
            this.断开此连接ToolStripMenuItem.Name = "断开此连接ToolStripMenuItem";
            this.断开此连接ToolStripMenuItem.Size = new System.Drawing.Size(170, 28);
            this.断开此连接ToolStripMenuItem.Text = "断开此连接";
            // 
            // SrcAddress
            // 
            this.SrcAddress.HeaderText = "源地址";
            this.SrcAddress.Name = "SrcAddress";
            this.SrcAddress.ReadOnly = true;
            // 
            // DstAddress
            // 
            this.DstAddress.HeaderText = "目标地址";
            this.DstAddress.Name = "DstAddress";
            this.DstAddress.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 542);
            this.Controls.Add(this.MainTabContainer);
            this.Controls.Add(this.SplitLine);
            this.Controls.Add(this.MainMenu);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "GUI for LAN Spy";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.HostListMenuStrip.ResumeLayout(false);
            this.PoisonerTab.ResumeLayout(false);
            this.Target2ListContainer.ResumeLayout(false);
            this.Target1ListContainer.ResumeLayout(false);
            this.ScannerTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HostList)).EndInit();
            this.MainTabContainer.ResumeLayout(false);
            this.WatcherTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionList)).EndInit();
            this.TargetListMenuStrip.ResumeLayout(false);
            this.ConnectionListMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem 开始ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动所有模块ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止所有模块ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 扫描ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 毒化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 监视ToolStripMenuItem;
        private System.Windows.Forms.Label SplitLine;
        private System.Windows.Forms.ToolStripMenuItem 扫描主机ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 侦测主机ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动扫描模块ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动毒化模块ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动监视模块ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip HostListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 主机列表复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加到目标组1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加到目标组2ToolStripMenuItem;
        private System.Windows.Forms.TabPage PoisonerTab;
        private System.Windows.Forms.TabPage ScannerTab;
        private System.Windows.Forms.DataGridView HostList;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostMAC;
        private System.Windows.Forms.TabControl MainTabContainer;
        private System.Windows.Forms.TabPage WatcherTab;
        private System.Windows.Forms.ListBox Target2List;
        private System.Windows.Forms.ListBox Target1List;
        private System.Windows.Forms.GroupBox Target2ListContainer;
        private System.Windows.Forms.GroupBox Target1ListContainer;
        private System.Windows.Forms.DataGridView ConnectionList;
        private System.Windows.Forms.ContextMenuStrip TargetListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 毒化目标复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 从目标组移除ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ConnectionListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 连接列表复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 断开此连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始毒化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始监视ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrcAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn DstAddress;
    }
}