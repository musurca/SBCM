using System;
using System.Collections.Generic;

namespace SBCM {
    public enum CommandState {
        NONE = 0,
        XO = 1,
        CO = 2
    }

    public class Unit {
        public static readonly int SLOT_MAINGUN_1 = 0;
        public static readonly int SLOT_MAINGUN_2 = 1;
        public static readonly int SLOT_MAINGUN_3 = 2;
        public static readonly int SLOT_MAINGUN_4 = 3;
        public static readonly int SLOT_MAINGUN_5 = 4;
        public static readonly int SLOT_MISSILE = 5;
        public static readonly int SLOT_RPG = 6;
        public static readonly int SLOT_COAX = 7;
        public static readonly int SLOT_HMG = 8;
        public static readonly int SLOT_MG = 9;
        public static readonly int SLOT_GRENADE_1 = 10;
        public static readonly int SLOT_OWS_1 = 11;
        public static readonly int SLOT_OWS_2 = 12;
        public static readonly int SLOT_OWS_SMOKE_1 = 13;
        public static readonly int SLOT_OWS_SMOKE_2 = 14;
        public static readonly int SLOT_RIFLE = 15;
        public static readonly int SLOT_SMOKE = 16;
        public static readonly int SLOT_GRENADE_2 = 17;
        public static readonly int MAX_AMMO_SLOTS = 18;

        public static readonly string CLASS_TANK = "Tank";
        public static readonly string CLASS_PC = "PC";
        public static readonly string CLASS_TRUCK = "Truck";
        public static readonly string CLASS_HELO = "Helicopter";
        public static readonly string CLASS_PERSONNEL = "personnel";

        public static readonly string[] AMMO_SLOT_NAMES = {
            "MAINGUN_1",
            "MAINGUN_2",
            "MAINGUN_3",
            "MAINGUN_4",
            "MAINGUN_5",
            "MISSILE",
            "RPG",
            "COAX",
            "HMG",
            "MG",
            "GRENADE_1",
            "OWS_1",
            "OWS_2",
            "OWS_SMOKE_1",
            "OWS_SMOKE_2",
            "RIFLE",
            "SMOKE",
            "GRENADE_2"
        };

        public string Force { get; }
        public string Callsign { get; }
        public string Type { get; }
        public string Unit_Class { get; }

        public int Strength_Maximum { get; set; }
        public int Strength_Current { get; set; }

        public string Company { get; set; }
        public string Platoon { get; set; }
        public string Section { get; set; }
        public string Team { get; set; }

        public CommandState Command { get; set; }

        public int Kills_Tanks { get; set; }
        public int Kills_PCs { get; set; }
        public int Kills_Helos { get; set; }
        public int Kills_Trucks { get; set; }
        public int Kills_Personnel { get; set; }
        public int Kills_Boats { get; set; }

        public int UTM_X { get; set; }
        public int UTM_Y { get; set; }

        public GunAmmo[] AllAmmo { get; set; }

        public DamageState Damage { get; set; }

        public Unit(string force, string callsign, string type, string unit_class) {
            Force = force;
            Callsign = callsign;
            Type = type;
            Unit_Class = unit_class;

            Strength_Current = 0;
            Strength_Maximum = 0;

            Company = "";
            Platoon = "";
            Section = "";
            Team = "";
            Command = CommandState.NONE;

            Kills_Tanks = 0;
            Kills_PCs = 0;
            Kills_Helos = 0;
            Kills_Trucks = 0;
            Kills_Personnel = 0;
            Kills_Boats = 0;

            UTM_X = UTM_Y = -1; // UNUSED FOR THE MOMENT

            AllAmmo = new GunAmmo[MAX_AMMO_SLOTS];
            Damage = new DamageState();
        }

        public GunAmmo[] GetAllAmmo() {
            return AllAmmo;
        }

        public GunAmmo GetAmmo(int index) {
            return AllAmmo[index];
        }

        public DamageState GetDamageState() {
            return Damage;
        }

        public void ClearHierarchy() {
            Company = "";
            Platoon = "";
            Section = "";
            Team = "";
            Command = CommandState.NONE;
        }

        public bool IsOperational() {
            return (Strength_Current > 0) && !Damage.Destroyed;
        }

        public int GetVehicleKills() {
            return Kills_Tanks + Kills_Trucks + Kills_PCs + Kills_Helos + Kills_Boats;
        }

        public static string SerializeToCSVColumnHeadings() {
            string csv_headings = "";

            List<string> attr = new List<string> {
                "Force", "Callsign", "Type", "Unit Class", "Strength (Current)", "Strength (Maximum)",
                "Company", "Platoon", "Section", "Team", "Rank",
                "UTM X", "UTM Y", "Kills (Tanks)", "Kills (PCs)", "Kills (Helicopters)", "Kills (Trucks)", "Kills (Personnel)",
                "Destroyed", "Immobilized", "Commander KIA", "Gunner KIA", "Loader KIA", "Driver KIA", "Damaged FCS", "Damaged Radio",
                "Damaged Turret"
            };

            for (int i = 0; i < MAX_AMMO_SLOTS; i++) {
                string slotName = AMMO_SLOT_NAMES[i];

                attr.Add($"AMMO_{slotName}_NAME");
                attr.Add($"AMMO_{slotName}_AMOUNT");
                attr.Add($"AMMO_{slotName}_MAX");
            }

            for (int i = 0; i < attr.Count; i++) {
                string obj = attr[i];
                csv_headings += $"\"{obj}\";";
            }

            return csv_headings;
        }

        public string SerializeToCSVRow() {
            string csv_export = "";
            List<object> attr = new List<object> {
                Force, Callsign, Type, Unit_Class, Strength_Current, Strength_Maximum,
                Company, Platoon, Section, Team, Command,
                UTM_X, UTM_Y, Kills_Tanks, Kills_PCs, Kills_Helos, Kills_Trucks, Kills_Personnel,
                Damage.Destroyed, Damage.Immobilized, Damage.CasualtyCommander,
                Damage.CasualtyGunner, Damage.CasualtyLoader, Damage.CasualtyDriver,
                Damage.DamagedFCS, Damage.DamagedRadio, Damage.DamagedTurret
            };
            foreach (GunAmmo ammo in AllAmmo) {
                attr.Add(ammo.Type);
                if (ammo.Type != "") {
                    attr.Add(ammo.Amount);
                    attr.Add(ammo.Maximum);
                } else {
                    attr.Add("");
                    attr.Add("");
                }
            }

            for (int i = 0; i < attr.Count; i++) {
                object obj = attr[i];
                if (obj is string) {
                    csv_export += $"\"{obj}\";";
                } else if (obj is bool) {
                    csv_export += $"{(((bool)obj) ? 1 : 0)};";
                } else if (obj is CommandState) {
                    string[] commandStateNames = { "None", "XO", "CO" };
                    csv_export += $"\"{commandStateNames[(int)obj]}\";";
                } else {
                    csv_export += $"{obj};";
                }
            }

            return csv_export;
        }
    }
}

