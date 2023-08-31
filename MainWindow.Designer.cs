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
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oobTree = new System.Windows.Forms.TreeView();
            this.forceSelector = new System.Windows.Forms.ComboBox();
            this.mapPanel = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelCampaignName = new System.Windows.Forms.Label();
            this.mapShowSelector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unitsToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playersToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextTurnFromReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnCounter = new System.Windows.Forms.Label();
            this.campaignStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.eventList = new System.Windows.Forms.DataGridView();
            this.TurnColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mainMenu.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventList)).BeginInit();
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
            this.mainMenu.Size = new System.Drawing.Size(784, 24);
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
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "New...";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            // 
            // oobTree
            // 
            this.oobTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.oobTree.Location = new System.Drawing.Point(536, 62);
            this.oobTree.Name = "oobTree";
            this.oobTree.Size = new System.Drawing.Size(236, 207);
            this.oobTree.TabIndex = 1;
            // 
            // forceSelector
            // 
            this.forceSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.forceSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.forceSelector.FormattingEnabled = true;
            this.forceSelector.Location = new System.Drawing.Point(536, 35);
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
            this.mapPanel.Location = new System.Drawing.Point(12, 62);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(518, 420);
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
            this.groupBox2.Location = new System.Drawing.Point(12, 29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(518, 29);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // labelCampaignName
            // 
            this.labelCampaignName.AutoSize = true;
            this.labelCampaignName.Location = new System.Drawing.Point(6, 11);
            this.labelCampaignName.Name = "labelCampaignName";
            this.labelCampaignName.Size = new System.Drawing.Size(99, 13);
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
            this.mapShowSelector.Location = new System.Drawing.Point(454, 488);
            this.mapShowSelector.Name = "mapShowSelector";
            this.mapShowSelector.Size = new System.Drawing.Size(76, 21);
            this.mapShowSelector.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(418, 492);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Show";
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nextTurnFromReportToolStripMenuItem});
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.updateToolStripMenuItem.Text = "Update";
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
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // unitsToCSVToolStripMenuItem
            // 
            this.unitsToCSVToolStripMenuItem.Name = "unitsToCSVToolStripMenuItem";
            this.unitsToCSVToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.unitsToCSVToolStripMenuItem.Text = "Units to CSV...";
            // 
            // eventsToCSVToolStripMenuItem
            // 
            this.eventsToCSVToolStripMenuItem.Name = "eventsToCSVToolStripMenuItem";
            this.eventsToCSVToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.eventsToCSVToolStripMenuItem.Text = "Events to CSV...";
            // 
            // playersToCSVToolStripMenuItem
            // 
            this.playersToCSVToolStripMenuItem.Name = "playersToCSVToolStripMenuItem";
            this.playersToCSVToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.playersToCSVToolStripMenuItem.Text = "Players to CSV...";
            // 
            // nextTurnFromReportToolStripMenuItem
            // 
            this.nextTurnFromReportToolStripMenuItem.Name = "nextTurnFromReportToolStripMenuItem";
            this.nextTurnFromReportToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.nextTurnFromReportToolStripMenuItem.Text = "Next turn...";
            this.nextTurnFromReportToolStripMenuItem.Click += new System.EventHandler(this.nextTurnFromReportToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            // 
            // turnCounter
            // 
            this.turnCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.turnCounter.AutoSize = true;
            this.turnCounter.Location = new System.Drawing.Point(468, 11);
            this.turnCounter.Name = "turnCounter";
            this.turnCounter.Size = new System.Drawing.Size(38, 13);
            this.turnCounter.TabIndex = 1;
            this.turnCounter.Text = "Turn 1";
            this.turnCounter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // campaignStateToolStripMenuItem
            // 
            this.campaignStateToolStripMenuItem.Name = "campaignStateToolStripMenuItem";
            this.campaignStateToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.campaignStateToolStripMenuItem.Text = "Campaign state...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.eventList);
            this.groupBox1.Location = new System.Drawing.Point(536, 275);
            this.groupBox1.MinimumSize = new System.Drawing.Size(236, 236);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 236);
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
            this.eventList.Size = new System.Drawing.Size(222, 211);
            this.eventList.TabIndex = 0;
            this.eventList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick_1);
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
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 521);
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
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eventList)).EndInit();
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
    }
}

