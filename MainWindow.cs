using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SBCM {
    public partial class MainWindow : Form {
        float _scale;
        Bitmap _testMap;
        Point _mapAnchor;
        Point _mapCenter;
        bool _mouseDown;

        Point _startPoint;

        Dictionary<string, Player> _players;
        Dictionary<string, Force> _forces;

        Dictionary<string, object> _oobToUnit;

        public MainWindow() {
            InitializeComponent();

            mapPanel.MouseWheel += mapPanel_Scroll;

            typeof(Panel).InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                mapPanel,
                new object[] { true }
            );

            _testMap = new Bitmap("map.png");
            _scale = 1.0f;
            _mouseDown = false;

            Rectangle rect = mapPanel.ClientRectangle;
            _mapCenter = new Point(
                rect.Left + (int)((rect.Right - rect.Left) * 0.5),
                rect.Top + (int)((rect.Bottom - rect.Top) * 0.5)
            );
            _mapAnchor = new Point(-_mapCenter.X, -_mapCenter.Y);

            // Load Report
            _players = new Dictionary<string, Player>();
            _forces = new Dictionary<string, Force>();
            ReportParser.MergeReport(
                "B26 Smash & Crush v1.0.0.sce_11_12-20-22_23_07_07.htm",
                _players,
                _forces
            );

            _oobToUnit = new Dictionary<string, object>();
            ammoGrid.ClearSelection();

            statusDestroyed.Visible = false;
            HideVehicleDamage();

            //ammoGrid.RowsDefaultCellStyle.SelectionBackColor = Color.;
            //ammoGrid.RowsDefaultCellStyle.SelectionForeColor = Color.Empty;

            foreach (string forceName in _forces.Keys) {
                forceSelector.Items.Add(forceName);
            }
            forceSelector.SelectedItem = _forces.Keys.ToList()[0];
        }

        private void HideVehicleDamage() {
            damageLayout.Visible = false;
            statusMobility.Visible = false;
            statusFCS.Visible = false;
            statusTurret.Visible = false;
            statusRadio.Visible = false;
            statusCommander.Visible = false;
            statusGunner.Visible = false;
            statusLoader.Visible = false;
            statusDriver.Visible = false;
            damageLayout.Visible = false;
        }

        private void ShowVehicleDamage() {
            damageLayout.Visible = true;
            statusMobility.Visible = true;
            statusFCS.Visible = true;
            statusTurret.Visible = true;
            statusRadio.Visible = true;
            statusCommander.Visible = true;
            statusGunner.Visible = true;
            statusLoader.Visible = true;
            statusDriver.Visible = true;
        }

        private TreeNode AddOOBNode(TreeNodeCollection nodes, string nodeName, object unit) {
            _oobToUnit.Add(nodeName, unit);
            return nodes.Add(nodeName);
        }

        private object SelectUnitFromOOB() {
            string selected = oobTree.SelectedNode.Text;
            if(_oobToUnit.ContainsKey(selected)) {
                return _oobToUnit[selected];
            }
            return null;
        }

        private void PopulateOOBTree() {
            Force currentForce = _forces[(string)forceSelector.SelectedItem];
            Battalion b = currentForce.Hierarchy;
            oobTree.BeginUpdate();
            oobTree.Nodes.Clear();
            _oobToUnit.Clear();
            foreach (Company c in b.Companies.Values) {
                TreeNode companyNode = AddOOBNode(oobTree.Nodes, c.ID, c);
                if (c.CO != null) {
                    AddOOBNode(companyNode.Nodes, $"{c.CO.Callsign} ({c.CO.Type})", c.CO);
                }
                if (c.XO != null) {
                    AddOOBNode(companyNode.Nodes, $"{c.XO.Callsign} ({c.XO.Type})", c.XO);
                }

                foreach (Platoon p in c.Platoons.Values) {
                    if (currentForce.GenerateCallsign(out string platoonCallsign, c.ID, p.ID)) {
                        TreeNode platoonNode = AddOOBNode(companyNode.Nodes, platoonCallsign, p);

                        List<Unit> allMembers = new List<Unit>();
                        foreach (Unit u in p.Members) {
                            allMembers.Add(u);
                        }

                        foreach (Unit u in p.Members) {
                            if (u.Team == "") {
                                string unitID = $"{u.Callsign} ({u.Type})";
                                if (p.CO == u) {
                                    unitID += " (CO)";
                                } else if (p.XO == u) {
                                    unitID += " (XO)";
                                }
                                TreeNode sectionNode = AddOOBNode(platoonNode.Nodes, unitID, u);
                                if(!u.IsOperational()) {
                                    sectionNode.ForeColor = Color.LightGray;
                                } else if(
                                    u.Unit_Class != Unit.CLASS_PERSONNEL 
                                    && u.GetDamageState().IsDamaged()
                                ) {
                                    sectionNode.ForeColor = Color.DarkGoldenrod;
                                }
                                foreach (Unit sub_u in p.Members) {
                                    if (sub_u.Team != "" && sub_u.Section == u.Section) {
                                        unitID = $"{sub_u.Callsign} ({sub_u.Type})";
                                        TreeNode node = AddOOBNode(sectionNode.Nodes, unitID, sub_u);
                                        if (!u.IsOperational()) {
                                            node.ForeColor = Color.LightGray;
                                        } else if (
                                            u.Unit_Class != Unit.CLASS_PERSONNEL 
                                            && u.GetDamageState().IsDamaged()
                                        ) {
                                            node.ForeColor = Color.DarkGoldenrod;
                                        }
                                        allMembers.Remove(sub_u);
                                    }
                                }
                                allMembers.Remove(u);
                            }
                        }

                        // Team units without a parent section
                        foreach (Unit u in allMembers) {
                            string unitID = $"{u.Callsign} ({u.Type})";
                            if (p.CO == u) {
                                unitID += " (CO)";
                            } else if (p.XO == u) {
                                unitID += " (XO)";
                            }
                            TreeNode node  = AddOOBNode(platoonNode.Nodes, unitID, u);
                            if (!u.IsOperational()) {
                                node.ForeColor = Color.LightGray;
                            } else if (
                                u.Unit_Class != Unit.CLASS_PERSONNEL 
                                && u.GetDamageState().IsDamaged()
                            ) {
                                node.ForeColor = Color.DarkGoldenrod;
                            }
                        }
                    }
                }
            }
            oobTree.EndUpdate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e) {
            Force currentForce = _forces[(string)forceSelector.SelectedItem];

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush blackBrush = new SolidBrush(Color.Black)) {
                g.FillRectangle(blackBrush, mapPanel.ClientRectangle);
            }

            // Center map, scale by zoom, then move to anchor
            g.TranslateTransform(_mapCenter.X, _mapCenter.Y);
            g.ScaleTransform(_scale, _scale);
            g.TranslateTransform(_mapAnchor.X, _mapAnchor.Y);
            g.DrawImage(_testMap, 0, 0);

            SolidBrush blockBrush = new SolidBrush(Color.DarkBlue);
            SolidBrush fontBrush = new SolidBrush(Color.White);
            foreach (Company c in currentForce.Hierarchy.Companies.Values) {
                foreach (Platoon p in c.Platoons.Values) {
                    if (p.UTM_X != -1 && p.UTM_Y != -1) {
                        g.FillRectangle(
                            blockBrush,
                                
                                p.UTM_X - 10, p.UTM_Y / 10 - 10,
                                20, 20
                                
                        );
                        currentForce.GenerateCallsign(out string callsign, c.ID, p.ID);
                        g.DrawString(callsign, SystemFonts.DefaultFont, fontBrush, p.UTM_X-10, p.UTM_Y / 10-10);
                    }
                }
            }
            blockBrush.Dispose();
            fontBrush.Dispose();
        }

        private void mapPanel_MouseMove(object sender, MouseEventArgs e) {
            if (_mouseDown) {
                Point curLoc = e.Location;
                _mapAnchor = new Point(
                    _mapAnchor.X + (int)((e.Location.X - _startPoint.X) / _scale),
                    _mapAnchor.Y + (int)((e.Location.Y - _startPoint.Y) / _scale)
                );
                _startPoint = curLoc;

                mapPanel.Invalidate();
            }
        }

        private void mapPanel_MouseDown(object sender, MouseEventArgs e) {
            _startPoint = e.Location;
            _mouseDown = true;
        }

        private void mapPanel_MouseUp(object sender, MouseEventArgs e) {
            _mouseDown = false;
        }

        private void mapPanel_Scroll(object sender, MouseEventArgs e) {
            _scale = Math.Min(2.0f, Math.Max(0.5f, _scale + e.Delta / 10000.0f));
            mapPanel.Invalidate();
        }

        private void forceSelector_SelectedIndexChanged(object sender, EventArgs e) {
            PopulateOOBTree();
            mapPanel.Invalidate();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void nextTurnFromReportToolStripMenuItem_Click(object sender, EventArgs e) {

        }


        private string getPlural(int num) {
            return num == 1 ? "" : "s";
        }

        private void oobTree_AfterSelect(object sender, TreeViewEventArgs e) {
            Force currentForce = _forces[(string)forceSelector.SelectedItem];

            object obj = SelectUnitFromOOB();
            ammoGrid.Rows.Clear();
   
            if(obj != null) {
                if (obj is Unit) {
                    // Single unit selectec
                    Unit u = (Unit)obj;

                    statusGroup.Visible = false;

                    callsignLabel.Text = u.Callsign;
                    typeLabel.Text = u.Type;
                    companyLabel.Text = u.Company;
                    platoonLabel.Text = u.Platoon;
                    sectionLabel.Text = u.Section;
                    teamLabel.Text = u.Team;

                    utmXBox.Text = u.UTM_X.ToString();
                    utmYBox.Text = u.UTM_Y.ToString();

                    DamageState damage = u.GetDamageState();
                    bool destroyed = damage.Destroyed || u.Strength_Current == 0;

                    // Ammo
                    if (!destroyed) {
                        foreach (GunAmmo ammo in u.GetAllAmmo()) {
                            if (ammo.Type != "") {
                                int rowIndex = ammoGrid.Rows.Add(ammo.Type, ammo.Amount.ToString(), ammo.Maximum.ToString());

                                float pct = 1.0f;
                                if (ammo.Maximum > 0) {
                                    pct = ammo.Amount / (float)ammo.Maximum;
                                }
                                if (pct < 0.05f) {
                                    ammoGrid.Rows[rowIndex].Cells[1].Style.ForeColor = Color.Red;
                                } else if (pct <= 0.5f) {
                                    ammoGrid.Rows[rowIndex].Cells[1].Style.ForeColor = Color.DarkGoldenrod;
                                }
                            }
                        }
                    }

                    // Damage

                    if (destroyed) {
                        HideVehicleDamage();
                        statusDestroyed.Visible = true;
                        statusStrength.Visible = false;
                    } else {
                        statusDestroyed.Visible = false;

                        if (u.Unit_Class != Unit.CLASS_PERSONNEL) {
                            ShowVehicleDamage();
                            statusStrength.Visible = false;

                            if (damage.Immobilized) {
                                statusMobility.Text = "Immobilized";
                                statusMobility.ForeColor = Color.Red;
                            } else {
                                statusMobility.Text = "Mobile";
                                statusMobility.ForeColor = Color.Green;
                            }

                            statusFCS.ForeColor = damage.DamagedFCS ? Color.Red : Color.Green;
                            statusTurret.ForeColor = damage.DamagedTurret ? Color.Red : Color.Green;
                            statusRadio.ForeColor = damage.DamagedRadio ? Color.Red : Color.Green;

                            statusCommander.ForeColor = damage.CasualtyCommander ? Color.Red : Color.Green;
                            statusGunner.ForeColor = damage.CasualtyGunner ? Color.Red : Color.Green;
                            statusLoader.ForeColor = damage.CasualtyLoader ? Color.Red : Color.Green;
                            statusDriver.ForeColor = damage.CasualtyGunner ? Color.Red : Color.Green;
                        } else {
                            HideVehicleDamage();
                            statusStrength.Text = $"{u.Strength_Current} / {u.Strength_Maximum}\nsoldier{getPlural(u.Strength_Maximum)}";

                            statusStrength.Visible = true;
                        }
                    }
                } else {
                    statusDestroyed.Visible = false;
                    HideVehicleDamage();
                    statusStrength.Visible = false;
                    statusGroup.Visible = true;

                    List<Unit> members;
        
                    if (obj is Company) {
                        Company c = (Company)obj;
                        members = c.Members;

                        callsignLabel.Text = c.ID;
                        typeLabel.Text = "";
                        companyLabel.Text = c.ID;
                        platoonLabel.Text = "";
                        sectionLabel.Text = "";
                        teamLabel.Text = "";

                        utmXBox.Text = "";
                        utmYBox.Text = "";
                    } else { 
                        Platoon p = (Platoon)obj;
                        members = p.Members;

                        string comp = p.Members[0].Company;
                        currentForce.GenerateCallsign(out string callsign, comp, p.ID);
                        
                        callsignLabel.Text = callsign;
                        typeLabel.Text = "";
                        companyLabel.Text = comp;
                        platoonLabel.Text = p.ID;
                        sectionLabel.Text = "";
                        teamLabel.Text = "";

                        utmXBox.Text = p.UTM_X.ToString();
                        utmYBox.Text = p.UTM_Y.ToString();
                    }

                    // List group ammunition and composition
                    Dictionary<string, GunAmmo> ammoTypes = new Dictionary<string, GunAmmo>();
                    int vehStrength = 0;
                    int totalVehs = 0;
                    int persStrength = 0;
                    int totalPers = 0;
                    int vehImmobilized = 0;
                    int vehDamaged = 0;

                    foreach (Unit u in members) {
                        bool destroyed = !u.IsOperational();

                        DamageState dam = u.GetDamageState();
                        
                        if (u.Unit_Class != Unit.CLASS_PERSONNEL) {
                            totalVehs++;
                            vehStrength += u.Strength_Current;

                            if (!destroyed) {
                                if (dam.Immobilized) { vehImmobilized++; }
                                else if (dam.IsDamaged()) { vehDamaged++; }
                            }
                        } else {
                            totalPers += u.Strength_Maximum;
                            persStrength += u.Strength_Current;
                        }

                        if (!destroyed) {
                            foreach (GunAmmo ammo in u.GetAllAmmo()) {
                                GunAmmo newAmmo;
                                if (ammo.Type != "") {
                                    if (ammoTypes.ContainsKey(ammo.Type)) {
                                        newAmmo = ammoTypes[ammo.Type];
                                        newAmmo.Type = ammo.Type;
                                        newAmmo.Maximum += ammo.Maximum;
                                        newAmmo.Amount += ammo.Amount;
                                        ammoTypes[newAmmo.Type] = newAmmo;
                                    } else {
                                        newAmmo = new GunAmmo();
                                        newAmmo.Type = ammo.Type;
                                        newAmmo.Maximum = ammo.Maximum;
                                        newAmmo.Amount = ammo.Amount;
                                        ammoTypes.Add(newAmmo.Type, newAmmo);
                                    }
                                }
                            }
                        }
                    }

                    string vehiclesStrength = totalVehs > 0 ? $"{vehStrength} / {totalVehs} vehicle{getPlural(totalVehs)}\n" : "\n";
                    string personnelStrength = totalPers > 0 ? $"{persStrength} / {totalPers} personnel\n" : "\n";
                    string vehiclesImmobilized = vehImmobilized > 0 ? $"{vehImmobilized} vehicle{getPlural(vehImmobilized)} immobilized\n" : "\n";
                    string vehiclesDamaged = vehDamaged > 0 ? $"{vehDamaged} vehicle{getPlural(vehDamaged)} damaged\n" : "\n";

                    statusGroup.Text = $"{vehiclesStrength}{personnelStrength}\n{vehiclesImmobilized}{vehiclesDamaged}";

                    foreach (GunAmmo ammo in ammoTypes.Values) {
                        int rowIndex = ammoGrid.Rows.Add(ammo.Type, ammo.Amount.ToString(), ammo.Maximum.ToString());

                        float pct = 1.0f;
                        if (ammo.Maximum > 0) {
                            pct = ammo.Amount / (float)ammo.Maximum;
                        }
                        if (pct < 0.05f) {
                            ammoGrid.Rows[rowIndex].Cells[1].Style.ForeColor = Color.Red;
                        } else if (pct <= 0.5f) {
                            ammoGrid.Rows[rowIndex].Cells[1].Style.ForeColor = Color.DarkGoldenrod;
                        }
                    }
                }
            } else {
                statusStrength.Visible = false;
                statusDestroyed.Visible = false;
                HideVehicleDamage();

                callsignLabel.Text = "";
                typeLabel.Text = "";
                companyLabel.Text = "";
                platoonLabel.Text = "";
                sectionLabel.Text = "";
                teamLabel.Text = "";

                utmXBox.Text = utmYBox.Text = "";
            }

            ammoGrid.ClearSelection();
        }

        private void ammoGrid_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            ammoGrid.ClearSelection();
        }
    }
}
