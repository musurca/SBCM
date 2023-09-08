namespace SBCM {
    partial class NewFromReport {
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
            this.label1 = new System.Windows.Forms.Label();
            this.campaignName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.reportFilePath = new System.Windows.Forms.TextBox();
            this.btnSelectReport = new System.Windows.Forms.Button();
            this.campaignDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mapImageURL = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.campaignTime = new System.Windows.Forms.DateTimePicker();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Campaign name:";
            // 
            // campaignName
            // 
            this.campaignName.Location = new System.Drawing.Point(118, 13);
            this.campaignName.MaxLength = 64;
            this.campaignName.Name = "campaignName";
            this.campaignName.Size = new System.Drawing.Size(332, 20);
            this.campaignName.TabIndex = 0;
            this.campaignName.TextChanged += new System.EventHandler(this.campaignName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Battle report (.HTM):";
            // 
            // reportFilePath
            // 
            this.reportFilePath.Enabled = false;
            this.reportFilePath.Location = new System.Drawing.Point(118, 102);
            this.reportFilePath.Name = "reportFilePath";
            this.reportFilePath.Size = new System.Drawing.Size(251, 20);
            this.reportFilePath.TabIndex = 3;
            this.reportFilePath.TabStop = false;
            // 
            // btnSelectReport
            // 
            this.btnSelectReport.Location = new System.Drawing.Point(375, 100);
            this.btnSelectReport.Name = "btnSelectReport";
            this.btnSelectReport.Size = new System.Drawing.Size(75, 23);
            this.btnSelectReport.TabIndex = 2;
            this.btnSelectReport.Text = "Browse...";
            this.btnSelectReport.UseVisualStyleBackColor = true;
            this.btnSelectReport.Click += new System.EventHandler(this.btnSelectReport_Click);
            // 
            // campaignDate
            // 
            this.campaignDate.Location = new System.Drawing.Point(118, 39);
            this.campaignDate.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.campaignDate.MinDate = new System.DateTime(1914, 1, 1, 0, 0, 0, 0);
            this.campaignDate.Name = "campaignDate";
            this.campaignDate.Size = new System.Drawing.Size(228, 20);
            this.campaignDate.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Start date & time:";
            this.label3.UseMnemonic = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelectImage);
            this.groupBox1.Controls.Add(this.campaignTime);
            this.groupBox1.Controls.Add(this.mapImageURL);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.campaignName);
            this.groupBox1.Controls.Add(this.btnSelectReport);
            this.groupBox1.Controls.Add(this.campaignDate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.reportFilePath);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(462, 132);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // mapImageURL
            // 
            this.mapImageURL.Enabled = false;
            this.mapImageURL.Location = new System.Drawing.Point(118, 76);
            this.mapImageURL.Name = "mapImageURL";
            this.mapImageURL.Size = new System.Drawing.Size(251, 20);
            this.mapImageURL.TabIndex = 8;
            this.mapImageURL.TabStop = false;
            this.mapImageURL.TextChanged += new System.EventHandler(this.mapImageURL_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Map image file:";
            // 
            // btnNext
            // 
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(400, 143);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "Next >>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 143);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // campaignTime
            // 
            this.campaignTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.campaignTime.Location = new System.Drawing.Point(352, 39);
            this.campaignTime.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.campaignTime.MinDate = new System.DateTime(1914, 1, 1, 0, 0, 0, 0);
            this.campaignTime.Name = "campaignTime";
            this.campaignTime.Size = new System.Drawing.Size(98, 20);
            this.campaignTime.TabIndex = 9;
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Location = new System.Drawing.Point(375, 74);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(75, 23);
            this.btnSelectImage.TabIndex = 10;
            this.btnSelectImage.Text = "Browse...";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            this.btnSelectImage.Click += new System.EventHandler(this.btnSelectImage_Click);
            // 
            // NewFromReport
            // 
            this.AcceptButton = this.btnNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(487, 173);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewFromReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Campaign Wizard";
            this.Load += new System.EventHandler(this.NewFromReport_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox campaignName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox reportFilePath;
        private System.Windows.Forms.Button btnSelectReport;
        private System.Windows.Forms.DateTimePicker campaignDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox mapImageURL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker campaignTime;
        private System.Windows.Forms.Button btnSelectImage;
    }
}