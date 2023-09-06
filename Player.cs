using System;

namespace SBCM {
    public class Player {
        public string Name { get; }

        public int ShotsHit { get; set; }
        public int ShotsTaken { get; set; }

        public int PersonalKills { get; set; }
        public int PersonalLosses { get; set; }
        public int PersonalFratricides { get; set; }

        public int UnitKills_Vehicles { get; set; }
        public int UnitLosses_Tanks { get; set; }
        public int UnitLosses_PCs { get; set; }
        public int UnitLosses_Personnel { get; set; }
        public int UnitLosses_Helicopters { get; set; }
        public int UnitLosses_Trucks { get; set; }

        public Player(string name) {
            Name = name;

            ShotsTaken = ShotsHit = 0;

            PersonalKills = PersonalLosses = PersonalFratricides = 0;

            UnitKills_Vehicles = 0;
            UnitLosses_Tanks = 0;
            UnitLosses_PCs = 0;
            UnitLosses_Personnel = 0;
            UnitLosses_Helicopters = 0;
            UnitLosses_Trucks = 0;
        }

        public double GetHitPercentage() {
            double pct = 0.0;

            if (ShotsTaken > 0) {
                pct = 100.0 * ShotsHit / (double)ShotsTaken;
            }

            return Math.Round(pct, 2);
        }

        public double GetPersonalKD() {
            int losses = PersonalLosses + PersonalFratricides;

            double kd = 0.0;

            if (losses > 0) {
                kd = PersonalKills / (double)losses;
            } else if (PersonalKills > 0) {
                kd = PersonalKills;
            }

            return Math.Round(kd, 2);
        }

        public double GetUnitKD() {
            int losses = UnitLosses_Tanks
                + UnitLosses_PCs
                + UnitLosses_Helicopters
                + UnitLosses_Trucks;

            double kd = 0.0;

            if (losses > 0) {
                kd = UnitKills_Vehicles / (double)losses;
            } else if (UnitKills_Vehicles > 0) {
                kd = UnitKills_Vehicles;
            }

            return Math.Round(kd, 2);
        }

        public int GetUnitLossesVehicle() {
            return UnitLosses_Tanks
                + UnitLosses_PCs
                + UnitLosses_Helicopters
                + UnitLosses_Trucks;

        }
    }
}