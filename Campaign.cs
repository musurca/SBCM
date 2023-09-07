using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace SBCM {
    public class Turn { 
        public int Index { get; set; }
        public Dictionary<string, Force> Forces;

        public Turn() {
        }

        public Turn(int index, Dictionary<string, Force> forces) {
            Index = index;
            Forces = forces;
        }
    }

    public class MapImage {
        private Bitmap _image;
        private string _imageURL;
        public string ImageURL {
            get {
                return _imageURL;
            }

            set {
                _imageURL = value;
                if (_imageURL != "") {
                    _image = DownloadBitmapFromURL(_imageURL);
                }
            }
        }

        public float UTM_Anchor_X { get; set; }
        public float UTM_Anchor_Y { get; set; }

        public float UTM_X_Step { get; set; }
        public float UTM_Y_Step { get; set; }

        public MapImage(string imageURL) {
            _image = null;
            ImageURL = imageURL;
            SetUTMAnchor(0.0f, 0.0f);
            SetUTMStep(0.0f, 0.0f);
        }

        public void ImageToUTM(Point imagePos, out int x, out int y) {
            x = (int)(UTM_Anchor_X + UTM_X_Step * imagePos.X);
            y = (int)(UTM_Anchor_Y + UTM_Y_Step * imagePos.Y);
        }

        public void UTMToImage(int utm_x, int utm_y, out int x, out int y) {
            if (UTM_X_Step != 0.0f && UTM_Y_Step != 0.0f) {
                x = (int)((utm_x - UTM_Anchor_X) / UTM_X_Step);
                y = (int)((utm_y - UTM_Anchor_Y) / UTM_Y_Step);
            } else {
                x = y = 0;
            }
        }

        public Bitmap GetBitmap() {
            return _image;
        }

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

        public void SetUTMAnchor(float x, float y) {
            UTM_Anchor_X = x;
            UTM_Anchor_Y = y;
        }

        public void SetUTMStep(float x_step, float y_step) {
            UTM_X_Step = x_step;
            UTM_Y_Step = y_step;
        }
    }

    public class Campaign {
        public string Name { get; set; }
        public MapImage MapImage { get; set; }
        public DateTime StartDate { get; set; }
        public bool ReadOnly { get; set; }

        private Dictionary<string, CallsignParser> _callsignTemplates = new Dictionary<string, CallsignParser>();
        public Dictionary<string, CallsignParser> CallsignTemplates {
            get {
                return _callsignTemplates;
            }

            set {
                _callsignTemplates = value;
                ApplyCallsignTemplates();
            }
        }

        private List<Turn> _turns = new List<Turn>();
        public List<Turn> Turns {
            get {
                return _turns;
            }

            set {
                _turns = value;
                ApplyCallsignTemplates();
            }
        }

        public Campaign() {

        }

        public Campaign(
            string name, 
            DateTime startDate, 
            MapImage mapImage, 
            Dictionary<string, Force> forces
        ) {
            Name = name;
            StartDate = startDate;
            MapImage = mapImage;
            Turns = new List<Turn>();
            ReadOnly = false;

            NextTurn(forces);
        }

        private void ApplyCallsignTemplates() {
            if (_callsignTemplates.Values.Count > 0) {
                foreach (Turn turn in Turns) {
                    foreach(string key in turn.Forces.Keys) {
                        if(turn.Forces.TryGetValue(key, out Force force)) {
                            if (_callsignTemplates.ContainsKey(key)) {
                                force.SetCallsignTemplate(
                                    _callsignTemplates[key]
                                );
                            }
                            force.EstimatePositions();
                        }
                    }
                }
            }
        }

        public void SetCallsignTemplate(string forceName, CallsignParser template) {
            if (_callsignTemplates.ContainsKey(forceName)) {
                _callsignTemplates[forceName] = template;
            } else {
                _callsignTemplates.Add(forceName, template);
            }
            foreach (Turn turn in Turns) {
                if(turn.Forces.ContainsKey(forceName)) {
                    Force f = turn.Forces[forceName];
                    f.SetCallsignTemplate(template);
                }
            }
        }

        public void NextTurn(Dictionary<string, Force> forces) {
            int index = Turns.Count;
            Turns.Add(new Turn(index, forces));
        }

        public Turn GetLastTurn() {
            if (Turns.Count > 0) {
                return Turns[Turns.Count - 1];
            }
            return null;
        }

        public void Serialize(string filepath) {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(filepath, GZip.Compress(json));
        }

        public static Campaign Deserialize(string filepath) {
            string gzjson = File.ReadAllText(filepath);
            Campaign cam = JsonConvert.DeserializeObject<Campaign>(
                GZip.Decompress(gzjson)
            );
            cam.ApplyCallsignTemplates();
            return cam;
        }
    }
}
