using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBCM {
    public partial class CalibrateMapImage : Form {

        readonly MapImage _image;
        readonly Bitmap _bitmap;
        Point _mapAnchor;
        Point _mapCenter;
        float _scale;

        int _pointIndex;
        Point[] _panelSamples; 
        Point[] _utmSamples;

        Matrix _transform;

        public CalibrateMapImage(MapImage image) {
            InitializeComponent();

            typeof(Panel).InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                mapPanel,
                new object[] { true }
            );

            _image = image;
            _bitmap = _image.GetBitmap();
            _pointIndex = 0;
            _utmSamples = new Point[2];
            _panelSamples = new Point[2];

            _mapAnchor = new Point(
                -_bitmap.Width / 2, -_bitmap.Height / 2
            );

            SetPanelAttributes();
        }

        private void SetPanelAttributes() {
            Rectangle rect = mapPanel.ClientRectangle;
            int panelWidth = rect.Right - rect.Left;
            int panelHeight = rect.Bottom - rect.Top;

            _mapCenter = new Point(
                rect.Left + (int)(panelWidth * 0.5),
                rect.Top + (int)(panelHeight * 0.5)
            );

            if (panelHeight > panelWidth) {
                _scale = panelWidth / (float)_bitmap.Width;
            } else {
                _scale = panelHeight / (float)_bitmap.Height;
            }
        }

        private void mapPanel_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Fill with black
            using (SolidBrush blackBrush = new SolidBrush(Color.Black)) {
                g.FillRectangle(blackBrush, mapPanel.ClientRectangle);
            }

            // Center map, scale by zoom, then move to anchor
            g.TranslateTransform(_mapCenter.X, _mapCenter.Y);
            g.ScaleTransform(_scale, _scale);
            g.TranslateTransform(_mapAnchor.X, _mapAnchor.Y);
            g.DrawImage(_bitmap, 0, 0);

            // Store inverse transform of image
            _transform?.Dispose();
            _transform = g.Transform;
            _transform.Invert();
        }

        private void mapPanel_MouseClick(object sender, MouseEventArgs e) {
            if(_pointIndex > 1) { return; } // shouldn't happen but windows is weird

            // Determine position of click on image
            Point panelPos = PanelToImage(e.Location);
            if (
                   panelPos.X < 0
                || panelPos.X >= _bitmap.Width
                || panelPos.Y < 0
                || panelPos.Y >= _bitmap.Height
            ) {
                // Can't choose a point outside the image
                return;
            }
            _panelSamples[_pointIndex] = panelPos;

            // Sample UTM point
            int utm_x, utm_y;
            using (
                EnterUTMCoord enterCoordDiag = new EnterUTMCoord()
            ) {
                if (enterCoordDiag.ShowDialog() == DialogResult.OK) {
                    utm_x = enterCoordDiag.UTM_X;
                    utm_y = enterCoordDiag.UTM_Y;
                } else {
                    return;
                }
            }
            _utmSamples[_pointIndex] = new Point(utm_x, utm_y);

            if (++_pointIndex == 1) {
                // Move on to second point
                labelInstructions.Text = "Click a second point on the map to specify its UTM coordinates";
            } else {

                int lowPixelX, lowUTMX, highPixelX, highUTMX;
                if (_panelSamples[1].X < _panelSamples[0].X) {
                    lowPixelX = _panelSamples[1].X;
                    lowUTMX = _utmSamples[1].X;
                    highPixelX = _panelSamples[0].X;
                    highUTMX = _utmSamples[0].X;
                } else {
                    lowPixelX = _panelSamples[0].X;
                    lowUTMX = _utmSamples[0].X;
                    highPixelX = _panelSamples[1].X;
                    highUTMX = _utmSamples[1].X;
                }

                int lowPixelY, lowUTMY, highPixelY, highUTMY;
                if (_panelSamples[1].Y < _panelSamples[0].Y) {
                    lowPixelY = _panelSamples[1].Y;
                    lowUTMY = _utmSamples[1].Y;
                    highPixelY = _panelSamples[0].Y;
                    highUTMY = _utmSamples[0].Y;
                } else {
                    lowPixelY = _panelSamples[0].Y;
                    lowUTMY = _utmSamples[0].Y;
                    highPixelY = _panelSamples[1].Y;
                    highUTMY = _utmSamples[1].Y;
                }

                // UTM X increases as pixel X increases
                int utm_dist_x;
                if(highUTMX < lowUTMX) {
                    utm_dist_x = 10000 - lowUTMX + highUTMX;
                } else {
                    utm_dist_x = highUTMX - lowUTMX;
                }

                // UTM Y increases as pixel Y decreases
                int utm_dist_y;
                if (highUTMY > lowUTMY) {
                    utm_dist_y = 10000 - highUTMY + lowUTMY;
                } else {
                    utm_dist_y = lowUTMY - highUTMY;
                }

                // Calculate UTM anchor and step for image
                int pixel_dist_x = highPixelX - lowPixelX;
                int pixel_dist_y = highPixelY - lowPixelY;

                if (pixel_dist_x == 0 || pixel_dist_y == 0) {
                    _pointIndex--; // do over
                    return;
                }

                float x_step = utm_dist_x / (float)pixel_dist_x;
                float y_step = utm_dist_y / (float)pixel_dist_y;

                float utm_anchor_x = (lowUTMX - x_step * lowPixelX) % 10000.0f;
                if (utm_anchor_x < 0.0f) utm_anchor_x += 10000.0f;
                float utm_anchor_y = (lowUTMY + y_step * lowPixelY) % 10000.0f;
                if (utm_anchor_y < 0.0f) utm_anchor_y += 10000.0f;

                _image.SetUTMStep(x_step, y_step);
                _image.SetUTMAnchor(utm_anchor_x, utm_anchor_y);   

                DialogResult = DialogResult.OK;
            }
        }

        private Point PanelToImage(Point panelPoint) {
            if (_transform != null) {
                Point[] transPt = new Point[] { panelPoint };
                _transform.TransformPoints(transPt);
                return transPt[0];
            }
            return new Point(0, 0);
        }

        private void mapPanel_SizeChanged(object sender, EventArgs e) {
            SetPanelAttributes();
            mapPanel.Invalidate();
        }

        private void mapPanel_MouseMove(object sender, MouseEventArgs e) {
            Point np = PanelToImage(e.Location);
            if (np != null) {
                labelX.Text = np.X.ToString();
                labelY.Text = np.Y.ToString();
            }
        }

        private void CalibrateMapImage_FormClosing(object sender, FormClosingEventArgs e) {
            _transform?.Dispose();
        }
    }
}
