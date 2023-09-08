using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
            int utm_x1 = (int)(UTM_Anchor_X + UTM_X_Step * _image.Width);
            int utm_y1 = (int)(UTM_Anchor_Y + UTM_Y_Step * _image.Height);

            if(utm_x1 < utm_x0) {
                (utm_x0, utm_x1) = (utm_x1, utm_x0);
            }
            if(utm_y1 < utm_y0) {
                (utm_y0, utm_y1) = (utm_y1, utm_y0);
            }

            return utm_x >= utm_x0 && utm_y >= utm_y0
                && utm_x <= utm_x1 && utm_y <= utm_y1;
        }

        public void ImageToUTM(Point imagePos, out int x, out int y) {
            x = (int)(UTM_Anchor_X + UTM_X_Step * imagePos.X);
            y = (int)(UTM_Anchor_Y + UTM_Y_Step * imagePos.Y);
        }

        public void UTMToImage(int utm_x, int utm_y, out float x, out float y) {
            if (UTM_X_Step != 0.0f && UTM_Y_Step != 0.0f) {
                x = ((utm_x - UTM_Anchor_X) / UTM_X_Step);
                y = ((utm_y - UTM_Anchor_Y) / UTM_Y_Step);
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
