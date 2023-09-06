using Newtonsoft.Json;
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
    public partial class NewFromReport : Form {
        public Campaign NewCampaign;

        public NewFromReport() {
            InitializeComponent();
            NewCampaign = null;
        }

        private void NewFromReport_Load(object sender, EventArgs e) {

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

        private void VerifyCanProceed() {
            btnNext.Enabled = ((campaignName.Text.Length > 0)
                && (reportFilePath.Text.Length > 0)
                && (mapImageURL.Text.Length > 0));
        }

        private void campaignName_TextChanged(object sender, EventArgs e) {
            VerifyCanProceed();
        }

        private void mapImageURL_TextChanged(object sender, EventArgs e) {
            VerifyCanProceed();
        }

        private void EnableInput() {
            campaignName.Enabled = true;
            campaignDate.Enabled = true;
            mapImageURL.Enabled = true;
            reportFilePath.Enabled = true;
            btnCancel.Enabled = true;
            VerifyCanProceed();
        }

        private void DisableInput() {
            campaignName.Enabled = false;
            campaignDate.Enabled = false;
            mapImageURL.Enabled = false;
            reportFilePath.Enabled = false;
            btnCancel.Enabled = false;
            btnNext.Enabled = false;
        }

        private void btnNext_Click(object sender, EventArgs e) {
            DisableInput();

            MapImage mapImage = new MapImage(mapImageURL.Text);
            if(mapImage.GetBitmap() == null) {
                // TODO: error dialog
                mapImageURL.Text = "";
                EnableInput();
                return;
            } else {
                // TODO: Calibrate the image
            }

            Dictionary<string, Force> forces = new Dictionary<string, Force>();
            ReportParser.MergeReport(
                reportFilePath.Text,
                forces
            );

            NewCampaign = new Campaign(
                campaignName.Text, 
                campaignDate.Value, 
                mapImage, 
                forces
            );

            SetCallsignTemplates setCallsignTemplates = new SetCallsignTemplates();
            setCallsignTemplates.SetCampaign(NewCampaign);
            if (setCallsignTemplates.ShowDialog() == DialogResult.OK) {
                DialogResult = DialogResult.OK;
            } else {
                EnableInput();
            }
        }
    }
}
