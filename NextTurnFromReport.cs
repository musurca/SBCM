using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBCM {
    public partial class NextTurnFromReport : Form {
        private Campaign _campaign;

        public NextTurnFromReport(Campaign campaign) {
            InitializeComponent();

            _campaign = campaign;
        }
        private void VerifyCanProceed() {
            btnOK.Enabled = reportFilePath.Text.Length > 0;
        }
        private void btnSelectReport_Click(object sender, EventArgs e) {
            using (
                OpenFileDialog dialog = new OpenFileDialog()
            ) {
                dialog.Filter = "HTM files (*.htm)|*.htm";
                dialog.FilterIndex = 0;
                dialog.RestoreDirectory = true;

                // Show the dialog and check if the user clicked OK
                if (dialog.ShowDialog() == DialogResult.OK) {
                    // Retrieve the selected folder path
                    reportFilePath.Text = dialog.FileName;

                    VerifyCanProceed();
                }
            }
        }

        private void EnableInput() {
            VerifyCanProceed();
            btnCancel.Enabled = true;
            btnSelectReport.Enabled = true;
            timeAdvMinutes.Enabled = true;
        }

        private void DisableInput() {
            btnOK.Enabled = false;
            btnCancel.Enabled = false;
            btnSelectReport.Enabled = false;
            timeAdvMinutes.Enabled = false;
        }

        private void btnOK_Click(object sender, EventArgs e) {
            DisableInput();

            Dictionary<string, Force> forces = _campaign.GetLastTurn().CloneForces();
            try {
                ReportParser.MergeReport(
                    reportFilePath.Text,
                    forces
                );
            } catch {
                MessageBox.Show(
                    "Couldn't load the report file. Make sure that you've selected a valid Steel Beasts after-action battle report (.HTM file).",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                EnableInput();
                return;
            }

            _campaign.NextTurn(forces, (int)timeAdvMinutes.Value);

            DialogResult = DialogResult.OK;
        }
    }
}
