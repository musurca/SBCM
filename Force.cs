using System.Collections.Generic;
using System.Linq;

namespace SBCM {
    public class Team {
        public string ID { get; }
        public Unit Member { get; set; }

        public int UTM_X { get; set; }
        public int UTM_Y { get; set; }

        public Team(string id) {
            ID = id;

            UTM_X = UTM_Y = -1;
        }

        public void UpdatePosition() {
            UTM_X = Member.UTM_X;
            UTM_Y = Member.UTM_Y;
        }
    }

    public class Section {
        string ID { get; }
        List<Unit> Members { get; set; }
        Dictionary<string, Team> Teams { get; set; }

        public int UTM_X { get; set; }
        public int UTM_Y { get; set; }

        public Section(string id) {
            ID = id;
            Members = new List<Unit>();
            Teams = new Dictionary<string, Team>();

            UTM_X = UTM_Y = -1;
        }

        public void UpdatePosition() {
            Battalion.AveragePosition(
                Members,
                out int u_x,
                out int u_y
            );
            UTM_X = u_x;
            UTM_Y = u_y;

            foreach (Team t in Teams.Values) {
                t.UpdatePosition();
            }
        }

        public void AddUnit(Unit u) {
            if (u.Section == ID) {
                Members.Add(u);

                if (u.Team != "") {
                    Team t;
                    if (!Teams.ContainsKey(u.Team)) {
                        t = new Team(u.Team);
                        Teams.Add(u.Team, t);
                        t.Member = u;
                    } else {
                        //Assert(!Teams.ContainsKey(u.Team), "should never replace team!")
                    }
                }
            }
        }

        public void DetachUnit(Unit u) {
            if (u.Section == ID) {
                Members.Remove(u);

                if (u.Team != "") {
                    if (Teams.ContainsKey(u.Team)) {
                        Teams.Remove(u.Team);
                    }
                }
            }
        }
    }

    public class Platoon {
        public string ID { get; }
        public Unit CO { get; set; }
        public Unit XO { get; set; }
        public List<Unit> Members { get; set; }
        public Dictionary<string, Section> Sections { get; set; }

        public int UTM_X { get; set; }
        public int UTM_Y { get; set; }

        public Platoon(string id) {
            ID = id;
            Members = new List<Unit>();
            Sections = new Dictionary<string, Section>();

            CO = null;
            XO = null;

            UTM_X = UTM_Y = -1;
        }

        public void UpdatePosition() {
            Battalion.AveragePosition(
                Members,
                out int u_x,
                out int u_y
            );
            UTM_X = u_x;
            UTM_Y = u_y;

            foreach (Section s in Sections.Values) {
                s.UpdatePosition();
            }
        }

        public void SetPosition(int utm_x, int utm_y) {
            foreach (Unit u in Members) {
                u.UTM_X = utm_x;
                u.UTM_Y = utm_y;
            }

            UpdatePosition();
        }

        public void AddUnit(Unit u) {
            if (u.Platoon == ID) {
                Members.Add(u);

                if (u.Section == "") {
                    // TODO handle replacing CO
                    if (u.Command == CommandState.CO) {
                        // TODO: assert if CO already defined
                        CO = u;
                    } else if (u.Command == CommandState.XO) {
                        // TODO assert if XO already defined
                        XO = u;
                    }
                } else {
                    Section s;
                    if (Sections.ContainsKey(u.Section)) {
                        s = Sections[u.Section];
                    } else {
                        s = new Section(u.Section);
                        Sections.Add(u.Section, s);
                    }
                    s.AddUnit(u);
                }
            }
        }

        public void DetachUnit(Unit u) {
            if (u.Platoon == ID) {
                Members.Remove(u);

                if (u.Section == "") {
                    // TODO handle replacing CO
                    if (CO == u) {
                        // TODO: assert if CO already defined
                        CO = null;
                        u.Command = CommandState.NONE;
                    } else if (XO == u) {
                        // TODO: assert if XO already defined
                        XO = null;
                        u.Command = CommandState.NONE;
                    }
                } else {
                    if (Sections.ContainsKey(u.Section)) {
                        Section s = Sections[u.Section];
                        s.DetachUnit(u);
                    }
                }
            }
        }
    }

    public class Company {
        public string ID { get; }

        public Unit CO { get; set; }
        public Unit XO { get; set; }

        public List<Unit> Members { get; set; }
        public Dictionary<string, Platoon> Platoons { get; set; }

        public Company(string id) {
            ID = id;
            Members = new List<Unit>();
            Platoons = new Dictionary<string, Platoon>();

            CO = null;
            XO = null;
        }

        public void UpdatePosition() {
            foreach (Platoon p in Platoons.Values) {
                p.UpdatePosition();
            }
        }

        public void AddUnit(Unit u) {
            if (u.Company == ID) {
                Members.Add(u);

                if (u.Platoon == "") {
                    // TODO handle replacing CO
                    if (u.Command == CommandState.CO) {
                        // TODO: assert if CO already defined
                        CO = u;
                    } else if (u.Command == CommandState.XO) {
                        // TODO: assert if XO already defined
                        XO = u;
                    }
                } else {
                    Platoon p;
                    if (Platoons.ContainsKey(u.Platoon)) {
                        p = Platoons[u.Platoon];
                    } else {
                        p = new Platoon(u.Platoon);
                        Platoons.Add(u.Platoon, p);
                    }
                    p.AddUnit(u);
                }
            }
        }

        public void DetachUnit(Unit u) {
            if (u.Company == ID) {
                Members.Remove(u);

                if (u.Platoon == "") {
                    // TODO handle replacing CO
                    if (CO == u) {
                        // TODO: assert if CO already defined
                        CO = null;
                        u.Command = CommandState.NONE;
                    } else if (XO == u) {
                        // TODO: assert if XO already defined
                        XO = null;
                        u.Command = CommandState.NONE;
                    }
                } else {
                    if (Platoons.ContainsKey(u.Platoon)) {
                        Platoon p = Platoons[u.Platoon];
                        p.DetachUnit(u);
                    }
                }
            }
        }
    }

    public class Battalion {
        public Dictionary<string, Company> Companies { get; set; }
        public List<Unit> Unattached { get; set; }

        public Battalion() {
            Companies = new Dictionary<string, Company>();
            Unattached = new List<Unit>();
        }

        public void UpdatePosition() {
            foreach (Company c in Companies.Values) {
                c.UpdatePosition();
            }
        }

        public static void AveragePosition(
            List<Unit> unitList, 
            out int utm_x, 
            out int utm_y
        ) {
            int num_samples = 0;
            int sum_utm_x = 0;
            int sum_utm_y = 0;
            foreach (Unit u in unitList) {
                if (u.UTM_X != -1 && u.UTM_Y != -1) {
                    sum_utm_x += u.UTM_X;
                    sum_utm_y += u.UTM_Y;
                    num_samples++;
                }
            }

            if (num_samples > 0) {
                utm_x = sum_utm_x / num_samples;
                utm_y = sum_utm_y / num_samples;
            } else {
                utm_x = utm_y = -1;
            }
        }

        public void Clear() {
            Companies.Clear();
            Unattached.Clear();
        }

        public void AddUnit(Unit u) {
            if (u.Company != "") {
                if (Unattached.Contains(u)) {
                    Unattached.Remove(u);
                }

                Company c;
                if (Companies.ContainsKey(u.Company)) {
                    c = Companies[u.Company];
                } else {
                    c = new Company(u.Company);
                    Companies[u.Company] = c;
                }
                c.AddUnit(u);
            } else {
                DetachUnit(u);
            }
        }

        public void DetachUnit(Unit u) {
            if (u.Company != "") {
                if (Companies.ContainsKey(u.Company)) {
                    Company c = Companies[u.Company];
                    c.DetachUnit(u);
                }

            }

            u.ClearHierarchy();
            if (!Unattached.Contains(u)) {
                Unattached.Add(u);
            }
        }
    }

    public class Force {
        public string Name { get; }
        public Battalion Hierarchy { get; set; }

        Dictionary<string, Unit> _units;

        CallsignParser _callsignTemplate;

        public Force(string name) {
            Name = name;

            _units = new Dictionary<string, Unit>();
            _callsignTemplate = null;

            Hierarchy = new Battalion();
        }

        public bool GenerateCallsign(
            out string callsign,
            string company,
            string platoon,
            string section = "",
            string team = ""
        ) {
            if (_callsignTemplate != null) {
                return _callsignTemplate.BuildCallsign(
                    out callsign,
                    company,
                    platoon,
                    section,
                    team
                );
            }

            callsign = "";
            return false;
        }

        public void EstimatePositions() {
            Hierarchy.UpdatePosition();
        }

        public void Detach(Unit u) {
            Hierarchy.DetachUnit(u);
        }

        public void Reattach(Unit u) {
            ParseUnitHierarchy(u);
            AddUnitToBattalion(u);
        }

        private void AddUnitToBattalion(Unit u) {
            Hierarchy.AddUnit(u);
        }

        private void RebuildBattalion() {
            Hierarchy.Clear();

            foreach (Unit u in _units.Values) {
                AddUnitToBattalion(u);
            }
        }

        public void SetCallsignTemplate(CallsignParser parser) {
            _callsignTemplate = parser;

            if (_units.Count > 0) {
                foreach (Unit u in _units.Values) {
                    ParseUnitHierarchy(u);
                }

                RebuildBattalion();
            }
        }

        public List<Unit> GetUnits() {
            return _units.Values.ToList();
        }

        public Unit GetUnit(string callsign) {
            if (_units.ContainsKey(callsign)) {
                return _units[callsign];
            }

            return null;
        }

        private void ParseUnitHierarchy(Unit u) {
            if (_callsignTemplate != null) {
                _callsignTemplate.GetBattalionPosition(
                    u.Callsign,
                    out string company,
                    out string platoon,
                    out string section,
                    out string team,
                    out CommandState command
                );

                u.Company = company;
                u.Platoon = platoon;
                u.Section = section;
                u.Team = team;
                u.Command = command;
            }
        }

        public void AddUnit(Unit u) {
            ParseUnitHierarchy(u);
            _units.Add(u.Callsign, u);
            AddUnitToBattalion(u);
        }

        public Dictionary<string, int> GetAmmoState() {
            Dictionary<string, int> ammoState = new Dictionary<string, int>();

            List<Unit> units = GetUnits();
            foreach (Unit u in units) {
                GunAmmo[] ammos = u.GetAllAmmo();
                foreach (GunAmmo ammo in ammos) {
                    if (ammo.Type != "") {
                        if (ammoState.ContainsKey(ammo.Type)) {
                            ammoState[ammo.Type] += ammo.Amount;
                        } else {
                            ammoState.Add(ammo.Type, ammo.Amount);
                        }
                    }
                }
            }

            return ammoState;
        }

        public Dictionary<string, int> GetAmmoCapacity() {
            Dictionary<string, int> ammoState = new Dictionary<string, int>();

            List<Unit> units = GetUnits();
            foreach (Unit u in units) {
                GunAmmo[] ammos = u.GetAllAmmo();
                foreach (GunAmmo ammo in ammos) {
                    if (ammo.Type != "") {
                        if (ammoState.ContainsKey(ammo.Type)) {
                            ammoState[ammo.Type] += ammo.Maximum;
                        } else {
                            ammoState.Add(ammo.Type, ammo.Maximum);
                        }
                    }
                }
            }

            return ammoState;
        }
    }
}