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
    public partial class EnterUTMCoord : Form {

        public int UTM_X;
        public int UTM_Y;

        public EnterUTMCoord() {
            InitializeComponent();
        }

        private void VerifyInput() {
            btnOK.Enabled = (utmXBox.Text.Length > 0 && utmYBox.Text.Length > 0);
        }

        private void utmXBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }

            VerifyInput();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            int valid_utms = 0;

            try {
                UTM_X = int.Parse(utmXBox.Text);
                valid_utms++;
            } catch {
                utmXBox.Text = "";
            }
            try {
                UTM_Y = int.Parse(utmYBox.Text);
                valid_utms++;
            } catch {
                utmYBox.Text = "";
            }

            if (valid_utms == 2) {
                DialogResult = DialogResult.OK;
            } else {
                VerifyInput();
            }
        }
    }
}
