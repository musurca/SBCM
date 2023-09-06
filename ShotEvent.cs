using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBCM {
    public class ShotEvent {
        public string EventText { get; set; }
        public string Time { get; set; }

        public int From_UTM_X { get; set; }
        public int From_UTM_Y { get; set; }
        public int To_UTM_X { get; set; }
        public int To_UTM_Y { get; set; }

        public int Distance { get; set; }

        public string Unit_Callsign { get; set; }

        public ShotEvent() {

        }
    }
}
