namespace SBCM {
    partial class MainWindow {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextTurnFromReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.campaignStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.unitsToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playersToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oobTree = new System.Windows.Forms.TreeView();
            this.forceSelector = new System.Windows.Forms.ComboBox();
            this.mapPanel = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.turnCounter = new System.Windows.Forms.Label();
            this.labelCampaignName = new System.Windows.Forms.Label();
            this.mapShowSelector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.eventList = new System.Windows.Forms.DataGridView();
            this.TurnColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.selectedUnitGroupBox = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.utmXBox = new System.Windows.Forms.TextBox();
            this.utmYBox = new System.Windows.Forms.TextBox();
            this.damagePanel = new System.Windows.Forms.Panel();
            this.statusStrength = new System.Windows.Forms.Label();
            this.statusDestroyed = new System.Windows.Forms.Label();
            this.damageLayout = new System.Windows.Forms.TableLayoutPanel();
            this.statusDriver = new System.Windows.Forms.Label();
            this.statusGunner = new System.Windows.Forms.Label();
            this.statusTurret = new System.Windows.Forms.Label();
            this.statusRadio = new System.Windows.Forms.Label();
            this.statusCommander = new System.Windows.Forms.Label();
            this.statusMobility = new System.Windows.Forms.Label();
            this.statusFCS = new System.Windows.Forms.Label();
            this.statusLoader = new System.Windows.Forms.Label();
            this.ammoGrid = new System.Windows.Forms.DataGridView();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.typeLabel = new System.Windows.Forms.Label();
            this.callsignLabel = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.teamLabel = new System.Windows.Forms.Label();
            this.sectionLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.platoonLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.companyLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.AmmoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CapacityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusGroup = new System.Windows.Forms.Label();
            this.mainMenu.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventList)).BeginInit();
            this.selectedUnitGroupBox.SuspendLayout();
            this.damagePanel.SuspendLayout();
            this.damageLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ammoGrid)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.updateToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(933, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.newToolStripMenuItem.Text = "New...";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nextTurnFromReportToolStripMenuItem});
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.updateToolStripMenuItem.Text = "Update";
            // 
            // nextTurnFromReportToolStripMenuItem
            // 
            this.nextTurnFromReportToolStripMenuItem.Name = "nextTurnFromReportToolStripMenuItem";
            this.nextTurnFromReportToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.nextTurnFromReportToolStripMenuItem.Text = "Next turn...";
            this.nextTurnFromReportToolStripMenuItem.Click += new System.EventHandler(this.nextTurnFromReportToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.campaignStateToolStripMenuItem,
            this.toolStripSeparator1,
            this.unitsToCSVToolStripMenuItem,
            this.eventsToCSVToolStripMenuItem,
            this.playersToCSVToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // campaignStateToolStripMenuItem
            // 
            this.campaignStateToolStripMenuItem.Name = "campaignStateToolStripMenuItem";
            this.campaignStateToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.campaignStateToolStripMenuItem.Text = "Campaign state...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // unitsToCSVToolStripMenuItem
            // 
            this.unitsToCSVToolStripMenuItem.Name = "unitsToCSVToolStripMenuItem";
            this.unitsToCSVToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.unitsToCSVToolStripMenuItem.Text = "Units to CSV...";
            // 
            // eventsToCSVToolStripMenuItem
            // 
            this.eventsToCSVToolStripMenuItem.Name = "eventsToCSVToolStripMenuItem";
            this.eventsToCSVToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.eventsToCSVToolStripMenuItem.Text = "Events to CSV...";
            // 
            // playersToCSVToolStripMenuItem
            // 
            this.playersToCSVToolStripMenuItem.Name = "playersToCSVToolStripMenuItem";
            this.playersToCSVToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.playersToCSVToolStripMenuItem.Text = "Players to CSV...";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            // 
            // oobTree
            // 
            this.oobTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.oobTree.Location = new System.Drawing.Point(685, 62);
            this.oobTree.Name = "oobTree";
            this.oobTree.Size = new System.Drawing.Size(236, 130);
            this.oobTree.TabIndex = 1;
            this.oobTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.oobTree_AfterSelect);
            // 
            // forceSelector
            // 
            this.forceSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.forceSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.forceSelector.FormattingEnabled = true;
            this.forceSelector.Location = new System.Drawing.Point(685, 33);
            this.forceSelector.Name = "forceSelector";
            this.forceSelector.Size = new System.Drawing.Size(236, 21);
            this.forceSelector.TabIndex = 2;
            this.forceSelector.SelectedIndexChanged += new System.EventHandler(this.forceSelector_SelectedIndexChanged);
            // 
            // mapPanel
            // 
            this.mapPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mapPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mapPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mapPanel.Location = new System.Drawing.Point(254, 62);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(425, 425);
            this.mapPanel.TabIndex = 3;
            this.mapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.mapPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapPanel_MouseDown);
            this.mapPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapPanel_MouseMove);
            this.mapPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mapPanel_MouseUp);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.turnCounter);
            this.groupBox2.Controls.Add(this.labelCampaignName);
            this.groupBox2.Location = new System.Drawing.Point(254, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(425, 29);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // turnCounter
            // 
            this.turnCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.turnCounter.Location = new System.Drawing.Point(338, 11);
            this.turnCounter.Name = "turnCounter";
            this.turnCounter.Size = new System.Drawing.Size(81, 13);
            this.turnCounter.TabIndex = 1;
            this.turnCounter.Text = "Turn 1";
            this.turnCounter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelCampaignName
            // 
            this.labelCampaignName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCampaignName.Location = new System.Drawing.Point(6, 11);
            this.labelCampaignName.Name = "labelCampaignName";
            this.labelCampaignName.Size = new System.Drawing.Size(289, 13);
            this.labelCampaignName.TabIndex = 0;
            this.labelCampaignName.Text = "Operation Devolver";
            // 
            // mapShowSelector
            // 
            this.mapShowSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mapShowSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mapShowSelector.FormattingEnabled = true;
            this.mapShowSelector.Items.AddRange(new object[] {
            "Platoons",
            "All Units",
            "Nothing"});
            this.mapShowSelector.Location = new System.Drawing.Point(603, 493);
            this.mapShowSelector.Name = "mapShowSelector";
            this.mapShowSelector.Size = new System.Drawing.Size(76, 21);
            this.mapShowSelector.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(567, 497);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Show";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.eventList);
            this.groupBox1.Location = new System.Drawing.Point(12, 231);
            this.groupBox1.MinimumSize = new System.Drawing.Size(236, 236);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 256);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Events";
            // 
            // eventList
            // 
            this.eventList.AllowUserToAddRows = false;
            this.eventList.AllowUserToDeleteRows = false;
            this.eventList.AllowUserToResizeRows = false;
            this.eventList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.eventList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TurnColumn,
            this.TimeColumn,
            this.EventColumn});
            this.eventList.Location = new System.Drawing.Point(8, 19);
            this.eventList.MultiSelect = false;
            this.eventList.Name = "eventList";
            this.eventList.ReadOnly = true;
            this.eventList.RowHeadersVisible = false;
            this.eventList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.eventList.ShowCellErrors = false;
            this.eventList.ShowEditingIcon = false;
            this.eventList.ShowRowErrors = false;
            this.eventList.Size = new System.Drawing.Size(222, 231);
            this.eventList.TabIndex = 0;
            // 
            // TurnColumn
            // 
            this.TurnColumn.Frozen = true;
            this.TurnColumn.HeaderText = "Turn";
            this.TurnColumn.MaxInputLength = 3;
            this.TurnColumn.Name = "TurnColumn";
            this.TurnColumn.ReadOnly = true;
            this.TurnColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // TimeColumn
            // 
            this.TimeColumn.Frozen = true;
            this.TimeColumn.HeaderText = "Time";
            this.TimeColumn.MaxInputLength = 5;
            this.TimeColumn.Name = "TimeColumn";
            this.TimeColumn.ReadOnly = true;
            this.TimeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // EventColumn
            // 
            this.EventColumn.Frozen = true;
            this.EventColumn.HeaderText = "Event";
            this.EventColumn.Name = "EventColumn";
            this.EventColumn.ReadOnly = true;
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Location = new System.Drawing.Point(12, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(236, 198);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Players";
            // 
            // selectedUnitGroupBox
            // 
            this.selectedUnitGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedUnitGroupBox.Controls.Add(this.label7);
            this.selectedUnitGroupBox.Controls.Add(this.label6);
            this.selectedUnitGroupBox.Controls.Add(this.utmXBox);
            this.selectedUnitGroupBox.Controls.Add(this.utmYBox);
            this.selectedUnitGroupBox.Controls.Add(this.damagePanel);
            this.selectedUnitGroupBox.Controls.Add(this.ammoGrid);
            this.selectedUnitGroupBox.Controls.Add(this.groupBox5);
            this.selectedUnitGroupBox.Controls.Add(this.groupBox4);
            this.selectedUnitGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedUnitGroupBox.Location = new System.Drawing.Point(685, 198);
            this.selectedUnitGroupBox.Name = "selectedUnitGroupBox";
            this.selectedUnitGroupBox.Size = new System.Drawing.Size(236, 316);
            this.selectedUnitGroupBox.TabIndex = 9;
            this.selectedUnitGroupBox.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(135, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Y:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "UTM X:";
            // 
            // utmXBox
            // 
            this.utmXBox.Enabled = false;
            this.utmXBox.Location = new System.Drawing.Point(53, 81);
            this.utmXBox.Name = "utmXBox";
            this.utmXBox.Size = new System.Drawing.Size(74, 20);
            this.utmXBox.TabIndex = 11;
            this.utmXBox.TabStop = false;
            this.utmXBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.utmXBox.WordWrap = false;
            // 
            // utmYBox
            // 
            this.utmYBox.Enabled = false;
            this.utmYBox.Location = new System.Drawing.Point(156, 81);
            this.utmYBox.Name = "utmYBox";
            this.utmYBox.Size = new System.Drawing.Size(74, 20);
            this.utmYBox.TabIndex = 10;
            this.utmYBox.TabStop = false;
            this.utmYBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // damagePanel
            // 
            this.damagePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.damagePanel.Controls.Add(this.statusGroup);
            this.damagePanel.Controls.Add(this.statusStrength);
            this.damagePanel.Controls.Add(this.statusDestroyed);
            this.damagePanel.Controls.Add(this.damageLayout);
            this.damagePanel.Location = new System.Drawing.Point(5, 103);
            this.damagePanel.Name = "damagePanel";
            this.damagePanel.Size = new System.Drawing.Size(226, 91);
            this.damagePanel.TabIndex = 9;
            // 
            // statusStrength
            // 
            this.statusStrength.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrength.Location = new System.Drawing.Point(2, 2);
            this.statusStrength.Name = "statusStrength";
            this.statusStrength.Size = new System.Drawing.Size(220, 84);
            this.statusStrength.TabIndex = 9;
            this.statusStrength.Text = "4 / 9";
            this.statusStrength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusDestroyed
            // 
            this.statusDestroyed.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusDestroyed.Location = new System.Drawing.Point(2, 2);
            this.statusDestroyed.Name = "statusDestroyed";
            this.statusDestroyed.Size = new System.Drawing.Size(220, 84);
            this.statusDestroyed.TabIndex = 8;
            this.statusDestroyed.Text = "DESTROYED";
            this.statusDestroyed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // damageLayout
            // 
            this.damageLayout.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.damageLayout.ColumnCount = 2;
            this.damageLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.damageLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.damageLayout.Controls.Add(this.statusDriver, 1, 3);
            this.damageLayout.Controls.Add(this.statusGunner, 1, 1);
            this.damageLayout.Controls.Add(this.statusTurret, 0, 2);
            this.damageLayout.Controls.Add(this.statusRadio, 0, 3);
            this.damageLayout.Controls.Add(this.statusCommander, 1, 0);
            this.damageLayout.Controls.Add(this.statusMobility, 0, 0);
            this.damageLayout.Controls.Add(this.statusFCS, 0, 1);
            this.damageLayout.Controls.Add(this.statusLoader, 1, 2);
            this.damageLayout.Location = new System.Drawing.Point(2, 2);
            this.damageLayout.Name = "damageLayout";
            this.damageLayout.RowCount = 4;
            this.damageLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.21951F));
            this.damageLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.78049F));
            this.damageLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.damageLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.damageLayout.Size = new System.Drawing.Size(220, 84);
            this.damageLayout.TabIndex = 8;
            // 
            // statusDriver
            // 
            this.statusDriver.Location = new System.Drawing.Point(113, 61);
            this.statusDriver.Name = "statusDriver";
            this.statusDriver.Size = new System.Drawing.Size(103, 22);
            this.statusDriver.TabIndex = 7;
            this.statusDriver.Text = "Driver";
            this.statusDriver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusGunner
            // 
            this.statusGunner.Location = new System.Drawing.Point(113, 21);
            this.statusGunner.Name = "statusGunner";
            this.statusGunner.Size = new System.Drawing.Size(103, 19);
            this.statusGunner.TabIndex = 5;
            this.statusGunner.Text = "Gunner";
            this.statusGunner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusTurret
            // 
            this.statusTurret.Location = new System.Drawing.Point(4, 41);
            this.statusTurret.Name = "statusTurret";
            this.statusTurret.Size = new System.Drawing.Size(102, 19);
            this.statusTurret.TabIndex = 2;
            this.statusTurret.Text = "Turret";
            this.statusTurret.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusRadio
            // 
            this.statusRadio.Location = new System.Drawing.Point(4, 61);
            this.statusRadio.Name = "statusRadio";
            this.statusRadio.Size = new System.Drawing.Size(102, 22);
            this.statusRadio.TabIndex = 3;
            this.statusRadio.Text = "Radio";
            this.statusRadio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusCommander
            // 
            this.statusCommander.Location = new System.Drawing.Point(113, 1);
            this.statusCommander.Name = "statusCommander";
            this.statusCommander.Size = new System.Drawing.Size(103, 19);
            this.statusCommander.TabIndex = 4;
            this.statusCommander.Text = "Commander";
            this.statusCommander.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusMobility
            // 
            this.statusMobility.Location = new System.Drawing.Point(4, 1);
            this.statusMobility.Name = "statusMobility";
            this.statusMobility.Size = new System.Drawing.Size(102, 19);
            this.statusMobility.TabIndex = 0;
            this.statusMobility.Text = "Mobile";
            this.statusMobility.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusFCS
            // 
            this.statusFCS.Location = new System.Drawing.Point(4, 21);
            this.statusFCS.Name = "statusFCS";
            this.statusFCS.Size = new System.Drawing.Size(102, 19);
            this.statusFCS.TabIndex = 1;
            this.statusFCS.Text = "FCS";
            this.statusFCS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusLoader
            // 
            this.statusLoader.Location = new System.Drawing.Point(113, 41);
            this.statusLoader.Name = "statusLoader";
            this.statusLoader.Size = new System.Drawing.Size(103, 19);
            this.statusLoader.TabIndex = 6;
            this.statusLoader.Text = "Loader";
            this.statusLoader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ammoGrid
            // 
            this.ammoGrid.AllowUserToAddRows = false;
            this.ammoGrid.AllowUserToDeleteRows = false;
            this.ammoGrid.AllowUserToResizeColumns = false;
            this.ammoGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ammoGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ammoGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ammoGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AmmoColumn,
            this.AmountColumn,
            this.CapacityColumn});
            this.ammoGrid.EnableHeadersVisualStyles = false;
            this.ammoGrid.Location = new System.Drawing.Point(5, 200);
            this.ammoGrid.MultiSelect = false;
            this.ammoGrid.Name = "ammoGrid";
            this.ammoGrid.ReadOnly = true;
            this.ammoGrid.RowHeadersVisible = false;
            this.ammoGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ammoGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ammoGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ammoGrid.ShowCellErrors = false;
            this.ammoGrid.ShowEditingIcon = false;
            this.ammoGrid.ShowRowErrors = false;
            this.ammoGrid.Size = new System.Drawing.Size(226, 110);
            this.ammoGrid.TabIndex = 0;
            this.ammoGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ammoGrid_CellContentClick);
            this.ammoGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ammoGrid_CellContentClick);
            this.ammoGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ammoGrid_CellContentClick);
            this.ammoGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ammoGrid_CellContentClick);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.typeLabel);
            this.groupBox5.Controls.Add(this.callsignLabel);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(4, 7);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(103, 72);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            // 
            // typeLabel
            // 
            this.typeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typeLabel.Location = new System.Drawing.Point(6, 39);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(91, 20);
            this.typeLabel.TabIndex = 1;
            this.typeLabel.Text = "label6";
            this.typeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // callsignLabel
            // 
            this.callsignLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.callsignLabel.Location = new System.Drawing.Point(6, 16);
            this.callsignLabel.Name = "callsignLabel";
            this.callsignLabel.Size = new System.Drawing.Size(91, 20);
            this.callsignLabel.TabIndex = 0;
            this.callsignLabel.Text = "label6";
            this.callsignLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.teamLabel);
            this.groupBox4.Controls.Add(this.sectionLabel);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.platoonLabel);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.companyLabel);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(113, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(117, 72);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            // 
            // teamLabel
            // 
            this.teamLabel.AutoSize = true;
            this.teamLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teamLabel.Location = new System.Drawing.Point(68, 52);
            this.teamLabel.Name = "teamLabel";
            this.teamLabel.Size = new System.Drawing.Size(44, 13);
            this.teamLabel.TabIndex = 6;
            this.teamLabel.Text = "A Team";
            // 
            // sectionLabel
            // 
            this.sectionLabel.AutoSize = true;
            this.sectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sectionLabel.Location = new System.Drawing.Point(68, 39);
            this.sectionLabel.Name = "sectionLabel";
            this.sectionLabel.Size = new System.Drawing.Size(36, 13);
            this.sectionLabel.TabIndex = 5;
            this.sectionLabel.Text = "A Sec";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Company";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // platoonLabel
            // 
            this.platoonLabel.AutoSize = true;
            this.platoonLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.platoonLabel.Location = new System.Drawing.Point(68, 26);
            this.platoonLabel.Name = "platoonLabel";
            this.platoonLabel.Size = new System.Drawing.Size(29, 13);
            this.platoonLabel.TabIndex = 4;
            this.platoonLabel.Text = "A Plt";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Location = new System.Drawing.Point(14, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Section";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // companyLabel
            // 
            this.companyLabel.AutoSize = true;
            this.companyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.companyLabel.Location = new System.Drawing.Point(68, 13);
            this.companyLabel.Name = "companyLabel";
            this.companyLabel.Size = new System.Drawing.Size(29, 13);
            this.companyLabel.TabIndex = 3;
            this.companyLabel.Text = "4 Co";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Platoon";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(26, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Team";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AmmoColumn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.AmmoColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.AmmoColumn.FillWeight = 90F;
            this.AmmoColumn.HeaderText = "Type";
            this.AmmoColumn.Name = "AmmoColumn";
            this.AmmoColumn.ReadOnly = true;
            this.AmmoColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AmmoColumn.Width = 109;
            // 
            // AmountColumn
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.AmountColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.AmountColumn.FillWeight = 10F;
            this.AmountColumn.HeaderText = "Amt";
            this.AmountColumn.Name = "AmountColumn";
            this.AmountColumn.ReadOnly = true;
            this.AmountColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AmountColumn.Width = 50;
            // 
            // CapacityColumn
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CapacityColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.CapacityColumn.FillWeight = 10F;
            this.CapacityColumn.HeaderText = "Max";
            this.CapacityColumn.Name = "CapacityColumn";
            this.CapacityColumn.ReadOnly = true;
            this.CapacityColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CapacityColumn.Width = 50;
            // 
            // statusGroup
            // 
            this.statusGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusGroup.Location = new System.Drawing.Point(2, 3);
            this.statusGroup.Name = "statusGroup";
            this.statusGroup.Size = new System.Drawing.Size(220, 84);
            this.statusGroup.TabIndex = 10;
            this.statusGroup.Text = "30 / 44 vehicles\r\n300 personnel\r\n\r\n10 vehicles immobilized";
            this.statusGroup.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 526);
            this.Controls.Add(this.selectedUnitGroupBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mapShowSelector);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mapPanel);
            this.Controls.Add(this.forceSelector);
            this.Controls.Add(this.oobTree);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(800, 560);
            this.Name = "MainWindow";
            this.Text = "Steel Beasts Campaign Manager";
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eventList)).EndInit();
            this.selectedUnitGroupBox.ResumeLayout(false);
            this.selectedUnitGroupBox.PerformLayout();
            this.damagePanel.ResumeLayout(false);
            this.damageLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ammoGrid)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.TreeView oobTree;
        private System.Windows.Forms.ComboBox forceSelector;
        private System.Windows.Forms.Panel mapPanel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelCampaignName;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ComboBox mapShowSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextTurnFromReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unitsToCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eventsToCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playersToCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label turnCounter;
        private System.Windows.Forms.ToolStripMenuItem campaignStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView eventList;
        private System.Windows.Forms.DataGridViewTextBoxColumn TurnColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventColumn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox selectedUnitGroupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label sectionLabel;
        private System.Windows.Forms.Label platoonLabel;
        private System.Windows.Forms.Label companyLabel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label teamLabel;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label callsignLabel;
        private System.Windows.Forms.DataGridView ammoGrid;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.Label statusDestroyed;
        private System.Windows.Forms.TableLayoutPanel damageLayout;
        private System.Windows.Forms.Label statusMobility;
        private System.Windows.Forms.Label statusFCS;
        private System.Windows.Forms.Label statusDriver;
        private System.Windows.Forms.Label statusLoader;
        private System.Windows.Forms.Label statusGunner;
        private System.Windows.Forms.Label statusCommander;
        private System.Windows.Forms.Label statusRadio;
        private System.Windows.Forms.Label statusTurret;
        private System.Windows.Forms.Panel damagePanel;
        private System.Windows.Forms.TextBox utmXBox;
        private System.Windows.Forms.TextBox utmYBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label statusStrength;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmmoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmountColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CapacityColumn;
        private System.Windows.Forms.Label statusGroup;
    }
}

