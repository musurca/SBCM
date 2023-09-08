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
        public DateTime CurrentTime; 

        public Turn() {
        }

        public Turn(int index, DateTime curTime, Dictionary<string, Force> forces) {
            Index = index;
            CurrentTime = curTime;
            Forces = forces;
        }

        public Dictionary <string, Force> CloneForces() {
            // Deep copy all forces in turn, but strip events
            Dictionary<string, Force> clonedForces = JsonConvert.DeserializeObject<Dictionary<string, Force>>(
                JsonConvert.SerializeObject(Forces)
            );

            foreach(Force f in clonedForces.Values) {
                f.ClearEvents();
            }

            return clonedForces;
        }
    }

    public class MapImage {
        private Bitmap _image;

        private string _imageDataString;
        public string ImageDataString {
            get {
                if (
                       _imageDataString == null
                    || _imageDataString == ""
                ) {
                    _imageDataString = GetImageDataString();
                }
                return _imageDataString; 
            }

            set {
                _imageDataString = value;
                _image = DataFuncs.JPEGBytesToImage(
                    DataFuncs.StringToBytes(_imageDataString)
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
            _imageDataString = GetImageDataString();
            // Reconstruct image from compressed version
            _image = DataFuncs.JPEGBytesToImage(
                DataFuncs.StringToBytes(_imageDataString)
            );
            SetUTMAnchor(0.0f, 0.0f);
            SetUTMStep(0.0f, 0.0f);
        }

        private string GetImageDataString() {
            return DataFuncs.BytesToString(
                DataFuncs.ImageToJPEGBytes(_image)
            );
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
        private string _fileVersion = "0100";
        public string FileVersion {
            get {
                return _fileVersion;
            }

            set {
                if(value != _fileVersion) {
                    // TODO for future version changes
                }
                _fileVersion = value;
            }
        }

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

        public void NextTurn(Dictionary<string, Force> forces, int minutesAdd = 0) {
            int index = Turns.Count;
            DateTime newTime = StartDate.AddMinutes(minutesAdd);

            Turns.Add(new Turn(index, newTime, forces));
            ApplyCallsignTemplates();
        }

        public Turn GetTurn(int index) {
            if (Turns.Count > 0) {
                foreach(Turn t in Turns) {
                    if (t.Index == index) {
                        return t;
                    }
                }
            }
            return null;
        }

        public Turn GetLastTurn() {
            if (Turns.Count > 0) {
                return Turns[Turns.Count - 1];
            }
            return null;
        }

        public Campaign CloneForSide(string sideName) {
            Campaign cloneCampaign = JsonConvert.DeserializeObject<Campaign>(
                JsonConvert.SerializeObject(this)
            );

            // Remove data for all other sides other than the one specified
            List<string> otherForces = new List<string>();

            foreach(string key in cloneCampaign._callsignTemplates.Keys) {
                if(key != sideName) {
                    otherForces.Add(key);
                }
            }
            foreach(string key in otherForces) {
                cloneCampaign._callsignTemplates.Remove(key);
            }
            foreach(Turn t in cloneCampaign.Turns) {
                otherForces.Clear();

                foreach (string key in t.Forces.Keys) {
                    if (key != sideName) {
                        otherForces.Add(key);
                    }
                }

                foreach(string key in otherForces) {
                    t.Forces.Remove(key);
                }
            }
            cloneCampaign.ApplyCallsignTemplates();

            return cloneCampaign;
        }

        public void Serialize(string filepath) {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(filepath, DataFuncs.Compress(json));
        }

        public static Campaign Deserialize(string filepath) {
            string gzjson = File.ReadAllText(filepath);
            Campaign cam = JsonConvert.DeserializeObject<Campaign>(
                DataFuncs.Decompress(gzjson)
            );
            cam.ApplyCallsignTemplates();
            return cam;
        }
    }
}
