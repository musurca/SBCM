using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SBCM {
    public class MapImage {
        private Bitmap _image;

        // Ensure that the map image is loaded from the serialized
        // base-64 string
        private string _imageDataString;
        public string ImageDataString {
            get {
                if (_image != null
                    && (
                           _imageDataString == null
                        || _imageDataString == ""
                    )
                ) {
                    _imageDataString = DataFuncs.BitmapToBase64(_image);
                }
                return _imageDataString;
            }

            set {
                _imageDataString = value;
                _image = DataFuncs.Base64ToBitmap(
                    _imageDataString
                );
            }
        }

        public float UTM_Anchor_X { get; set; }
        public float UTM_Anchor_Y { get; set; }

        public float UTM_X_Step { get; set; }
        public float UTM_Y_Step { get; set; }

        public MapImage() {
            _image = null;
            _imageDataString = "";
            SetUTMAnchor(0.0f, 0.0f);
            SetUTMStep(0.0f, 0.0f);
        }

        public MapImage(string imageFilePath) {
            _image = new Bitmap(imageFilePath);
            _imageDataString = DataFuncs.BitmapToBase64(_image);
            _image.Dispose();
            // Reconstruct image from compressed version
            _image = DataFuncs.Base64ToBitmap(
                _imageDataString
            );
            SetUTMAnchor(0.0f, 0.0f);
            SetUTMStep(0.0f, 0.0f);
        }

        public void Dispose() {
            _image?.Dispose();
            _imageDataString = "";
        }

        public bool WithinUTMExtents(int utm_x, int utm_y) {
            int utm_x0 = (int)UTM_Anchor_X;
            int utm_y0 = (int)UTM_Anchor_Y;
            int utm_x1 = ((int)(UTM_Anchor_X + UTM_X_Step * _image.Width)) % 10000;
            int utm_y1 = ((int)(UTM_Anchor_Y - UTM_Y_Step * _image.Height)) % 10000;
            if (utm_x1 < 0) utm_x1 += 10000;
            if (utm_y1 < 0) utm_y1 += 10000;

            var x_extents = new List<(float, float)>();
            if(utm_x1 < utm_x0) {
                x_extents.Add((utm_x0, 10000));
                x_extents.Add((0, utm_x1));
            } else {
                x_extents.Add((utm_x0, utm_x1));
            }

            var y_extents = new List<(float, float)>();
            if (utm_y1 > utm_y0) {
                x_extents.Add((utm_y1, 10000));
                x_extents.Add((0, utm_y0));
            } else {
                x_extents.Add((utm_y1, utm_y0));
            }

            bool in_range = false;
            foreach (var range in x_extents) {
                if (utm_x >= range.Item1 && utm_x <= range.Item2) {
                    in_range = true;
                    break;
                }
            }
            if (!in_range) return false;

            in_range = false;
            foreach (var range in y_extents) {
                if (utm_x >= range.Item1 && utm_x <= range.Item2) {
                    in_range = true;
                    break;
                }
            }
            if (!in_range) return false;

            return true;
        }

        public int UTM_Distance(int utm_x0, int utm_y0, int utm_x1, int utm_y1) {
            UTMToImage(utm_x0, utm_y0, out float x0, out float y0);
            UTMToImage(utm_x1, utm_y1, out float x1, out float y1);

            float x_dist = UTM_X_Step * (x1 - x0);
            float y_dist = UTM_Y_Step * (y1 - y0);

            return (int)Math.Round(
                10 * Math.Sqrt(x_dist * x_dist + y_dist * y_dist)
            );
        }

        public void ImageToUTM(Point imagePos, out int x, out int y) {
            x = ((int)(UTM_Anchor_X + UTM_X_Step * imagePos.X)) % 10000;
            y = ((int)(UTM_Anchor_Y - UTM_Y_Step * imagePos.Y)) % 10000;
            if (x < 0) x += 10000;
            if (y < 0) y += 10000;
        }

        public void UTMToImage(int utm_x, int utm_y, out float x, out float y) {
            if (UTM_X_Step != 0.0f && UTM_Y_Step != 0.0f) {
                float x_dist;
                if(utm_x < UTM_Anchor_X) {
                    x_dist = 10000 - UTM_Anchor_X + utm_x;
                } else {
                    x_dist = utm_x - UTM_Anchor_X;
                }

                float y_dist;
                if (utm_y > UTM_Anchor_Y) {
                    y_dist = 10000 - utm_y + UTM_Anchor_Y;
                } else {
                    y_dist = UTM_Anchor_Y - utm_y;
                }

                x = x_dist / UTM_X_Step;
                y = y_dist / UTM_Y_Step;
            } else {
                x = y = 0.0f;
            }
        }

        public Bitmap GetBitmap() {
            return _image;
        }

        /* DEPRECATED -- TO BE REMOVED
        public static Bitmap DownloadBitmapFromURL(string url) {
            Bitmap bitmap = null;

            try {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                bitmap = new Bitmap(responseStream);
            } catch {
                // TODO: some kind of error response to user
            }

            return bitmap;
        }
        */

        public void SetUTMAnchor(float x, float y) {
            UTM_Anchor_X = x;
            UTM_Anchor_Y = y;
        }

        public void SetUTMStep(float x_step, float y_step) {
            UTM_X_Step = x_step;
            UTM_Y_Step = y_step;
        }
    }
}
