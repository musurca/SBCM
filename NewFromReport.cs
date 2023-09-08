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
            campaignTime.ShowUpDown = true;
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

            Dictionary<string, Force> forces = new Dictionary<string, Force>();
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

            MapImage mapImage = new MapImage(mapImageURL.Text);
            if (mapImage.GetBitmap() == null) {
                MessageBox.Show(
                    "Couldn't load the image. Check that the URL is correct, and that your internet connection is working.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                mapImageURL.Text = "";
                EnableInput();
                return;
            }

            using (CalibrateMapImage calibrateImage = new CalibrateMapImage(mapImage)) {
                if (calibrateImage.ShowDialog() != DialogResult.OK) {
                    EnableInput();
                    return;
                }
            }

            // Combine date and time
            DateTime campaignDateTime = campaignDate.Value.Date +
                    campaignTime.Value.TimeOfDay;

            if (forces.Count > 0) {
                NewCampaign = new Campaign(
                    campaignName.Text,
                    campaignDateTime,
                    mapImage,
                    forces
                );

                using (
                    SetCallsignTemplates setCallsignTemplates = new SetCallsignTemplates(NewCampaign)
                ) {
                    if (setCallsignTemplates.ShowDialog() == DialogResult.OK) {
                        DialogResult = DialogResult.OK;
                    }
                }

                
            } else {
                MessageBox.Show(
                    "Couldn't find any sides in the battle report!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                EnableInput();
            }
        }

        private void btnSelectImage_Click(object sender, EventArgs e) {
            using (
                OpenFileDialog dialog = new OpenFileDialog()
            ) {
                dialog.Filter = "Image files (*.png,*.jpg,*.bmp,*.tif)|*.png;*.jpg;*.bmp;*.tif";
                dialog.FilterIndex = 0;
                dialog.RestoreDirectory = true;

                // Show the dialog and check if the user clicked OK
                if (dialog.ShowDialog() == DialogResult.OK) {
                    // Retrieve the selected folder path
                    mapImageURL.Text = dialog.FileName;

                    VerifyCanProceed();
                }
            }
        }
    }
}
