namespace SBCM {
    partial class NextTurnFromReport {
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
            this.btnSelectReport = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.reportFilePath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timeAdvMinutes = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeAdvMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectReport
            // 
            this.btnSelectReport.Location = new System.Drawing.Point(375, 17);
            this.btnSelectReport.Name = "btnSelectReport";
            this.btnSelectReport.Size = new System.Drawing.Size(75, 23);
            this.btnSelectReport.TabIndex = 1;
            this.btnSelectReport.Text = "Browse...";
            this.btnSelectReport.UseVisualStyleBackColor = true;
            this.btnSelectReport.Click += new System.EventHandler(this.btnSelectReport_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Battle report (.HTM):";
            // 
            // reportFilePath
            // 
            this.reportFilePath.Enabled = false;
            this.reportFilePath.Location = new System.Drawing.Point(118, 19);
            this.reportFilePath.Name = "reportFilePath";
            this.reportFilePath.Size = new System.Drawing.Size(251, 20);
            this.reportFilePath.TabIndex = 61;
            this.reportFilePath.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.timeAdvMinutes);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.reportFilePath);
            this.groupBox1.Controls.Add(this.btnSelectReport);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 94);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Advance time by:";
            // 
            // timeAdvMinutes
            // 
            this.timeAdvMinutes.Location = new System.Drawing.Point(118, 56);
            this.timeAdvMinutes.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.timeAdvMinutes.Name = "timeAdvMinutes";
            this.timeAdvMinutes.Size = new System.Drawing.Size(51, 20);
            this.timeAdvMinutes.TabIndex = 2;
            this.timeAdvMinutes.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(175, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 62;
            this.label3.Text = "minutes";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(12, 112);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(401, 112);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // NextTurnFromReport
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(489, 150);
            this.ControlBox = false;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NextTurnFromReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Next turn from report";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeAdvMinutes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSelectReport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox reportFilePath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown timeAdvMinutes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}