using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SBCM {
    public partial class MainWindow : Form {
        float _scale;
        Bitmap _mapImage;
        Point _mapAnchor;
        Point _mapCenter;
        bool _mouseDown;

        Point _startPoint;

        string _currentCampaignFileName;

        Dictionary<string, Force> _forces = new Dictionary<string, Force>();

        Dictionary<string, object> _oobToUnit;
        Campaign _campaign;

        object _selectedUnit;

        Matrix _mapTransform;

        // TEST URL = https://i.imgur.com/aqIr9Ol.png

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

            _mapImage = null;//new Bitmap("map.png");
            _scale = 1.0f;
            _mouseDown = false;

            campaignName.Text = "";
            turnCounter.Text = "";
            labelUTMX.Text = "";
            labelUTMY.Text = "";

            _currentCampaignFileName = "";

            Rectangle rect = mapPanel.ClientRectangle;
            _mapCenter = new Point(
                rect.Left + (int)((rect.Right - rect.Left) * 0.5),
                rect.Top + (int)((rect.Bottom - rect.Top) * 0.5)
            );
            //_mapAnchor = new Point(-_mapCenter.X, -_mapCenter.Y);

            // Load Report
            //_forces = new Dictionary<string, Force>();
            //ReportParser.MergeReport(
            //    "B26 Smash & Crush v1.0.0.sce_11_12-20-22_23_07_07.htm",
            //    _forces
            //);

            _oobToUnit = new Dictionary<string, object>();
            ammoGrid.ClearSelection();

            statusDestroyed.Visible = false;
            HideVehicleDamage();
            callsignLabel.Text = "";
            typeLabel.Text = "";
            companyLabel.Text = "";
            platoonLabel.Text = "";
            sectionLabel.Text = "";
            teamLabel.Text = "";

            labelCompanyHeader.Visible = false;
            labelPlatoonHeader.Visible = false;
            labelSectionHeader.Visible = false;
            labelTeamHeader.Visible = false;

            mapShowSelector.SelectedItem = mapShowSelector.Items[0];
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

        private object SelectUnitFromOOB() {
            string selected = oobTree.SelectedNode.Text;
            if(_oobToUnit.ContainsKey(selected)) {
                return _oobToUnit[selected];
            }
            return null;
        }

        private void PopulateOOBTree() {
            if (_forces != null && forceSelector.SelectedItem != null) {
                Force currentForce = _forces[(string)forceSelector.SelectedItem];
                _oobToUnit = ViewFuncs.PopulateTreeViewWithOOB(oobTree, currentForce);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush blackBrush = new SolidBrush(Color.Black)) {
                g.FillRectangle(blackBrush, mapPanel.ClientRectangle);
            }

            if (_mapImage != null) {
                Force currentForce = _forces[(string)forceSelector.SelectedItem];

                // Center map, scale by zoom, then move to anchor
                g.TranslateTransform(_mapCenter.X, _mapCenter.Y);
                g.ScaleTransform(_scale, _scale);
                g.TranslateTransform(_mapAnchor.X, _mapAnchor.Y);

                // Store inverse of transform
                _mapTransform?.Dispose();
                _mapTransform = g.Transform;
                _mapTransform.Invert();

                // Draw the map
                using (Pen whitePen = new Pen(Color.White)) {
                    // Border
                    g.DrawRectangle(whitePen, -5, -5, _mapImage.Width + 10, _mapImage.Height + 10);
                }
                g.DrawImage(_mapImage, 0, 0);

                // Draw units/platoons on the map
                string mapViewSelect = mapShowSelector.SelectedItem.ToString();
                MapImage map = _campaign.MapImage;
                if (map.UTM_X_Step != 0.0f && map.UTM_Y_Step != 0.0f && mapViewSelect != "None") {
                    SolidBrush blockBrush = new SolidBrush(Color.DarkBlue);
                    SolidBrush fontBrush = new SolidBrush(Color.White);
                    if (mapViewSelect == "Platoons") {
                        foreach (Company c in currentForce.Hierarchy.Companies.Values) {
                            foreach (Platoon p in c.Platoons.Values) {
                                if (p.UTM_X != -1 && p.UTM_Y != -1) {
                                    map.UTMToImage(p.UTM_X, p.UTM_Y, out int x_pos, out int y_pos);

                                    g.FillRectangle(
                                        blockBrush,
                                        x_pos - 10,
                                        y_pos - 10,
                                        20, 20
                                    );

                                    currentForce.GenerateCallsign(out string callsign, c.ID, p.ID);
                                    g.DrawString(
                                        callsign, 
                                        SystemFonts.DefaultFont, fontBrush, 
                                        x_pos - 10, y_pos - 10
                                    );
                                }
                            }
                        }
                    } else {
                        foreach (Unit u in currentForce.Units.Values) {
                            if (u.UTM_X != -1 && u.UTM_Y != -1) {
                                map.UTMToImage(u.UTM_X, u.UTM_Y, out int x_pos, out int y_pos);

                                g.FillRectangle(
                                    blockBrush,
                                    x_pos - 10,
                                    y_pos - 10,
                                    20, 20
                                );

                                g.DrawString(
                                    u.Callsign, 
                                    SystemFonts.DefaultFont, fontBrush, 
                                    x_pos - 10, y_pos - 10
                               );
                            }
                        }
                    }
                    blockBrush.Dispose();
                    fontBrush.Dispose();
                }
            }
        }

        private Point PanelToImage(Point panelPoint) {
            if (_mapTransform != null) {
                Point[] transPt = new Point[] { panelPoint };
                _mapTransform.TransformPoints(transPt);
                return transPt[0];
            }
            return new Point(0, 0);
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
            if(_mapImage != null) {
                MapImage map = _campaign.MapImage;

                map.ImageToUTM(
                    PanelToImage(e.Location), 
                    out int utm_x, out int utm_y
                );
                labelUTMX.Text = utm_x.ToString();
                labelUTMY.Text = utm_y.ToString();
            } else {
                labelUTMX.Text = "";
                labelUTMY.Text = "";
            }

        }

        private void mapPanel_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right && _mapImage != null) {
                _startPoint = e.Location;
                _mouseDown = true;
            }
        }

        private void mapPanel_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                _mouseDown = false;
            }
        }

        private void mapPanel_Scroll(object sender, MouseEventArgs e) {
            _scale = Math.Min(2.0f, Math.Max(0.5f, _scale + e.Delta / 10000.0f));
            mapPanel.Invalidate();
        }

        private void forceSelector_SelectedIndexChanged(object sender, EventArgs e) {
            Force currentForce = null;
            if (forceSelector.SelectedItem != null) {
                if (_forces.ContainsKey(forceSelector.SelectedItem.ToString())) {
                    currentForce = _forces[forceSelector.SelectedItem.ToString()];
                }
            }

            eventGrid.Rows.Clear();
            playerGrid.Rows.Clear();
            
            // Populate force OOB
            PopulateOOBTree();

            // Populate units
            if (currentForce != null) {
                foreach (ShotEvent evt in currentForce.GetEvents()) {
                    eventGrid.Rows.Add(evt.Time, evt.EventText);
                }
            }
            eventGrid.ClearSelection();

            // Populate players
            if (currentForce != null) {
                foreach (Player player in currentForce?.GetPlayers()) {
                    playerGrid.Rows.Add(
                        player.Name,
                        player.GetHitPercentage(),
                        player.GetPersonalKD(),
                        player.GetUnitKD(),
                        player.PersonalKills,
                        player.PersonalLosses,
                        player.PersonalFratricides,
                        player.UnitKills_Vehicles,
                        player.GetUnitLossesVehicle(),
                        player.UnitLosses_Personnel
                    );
                }
            }
            playerGrid.ClearSelection();

            mapPanel.Invalidate();
        }

        private void LoadCampaign(Campaign campaign) {
            _campaign = campaign;

            campaignName.Text = _campaign.Name;
            _mapImage = _campaign.MapImage.GetBitmap();
            _mapAnchor = new Point(-_mapImage.Width / 2, -_mapImage.Height / 2);

            Turn curTurn = _campaign.GetLastTurn();
            turnCounter.Text = curTurn.Index == 0
                ? "Setup Phase"
                : $"Turn {curTurn.Index}";

            _forces = curTurn.Forces;

            forceSelector.Items.Clear();
            foreach (string forceName in _forces.Keys) {
                forceSelector.Items.Add(forceName);
            }
            forceSelector.SelectedItem = _forces.Keys.ToList()[0];

            editCallsignTemplateToolStripMenuItem.Enabled = true;
            recalibrateMapImageToolStripMenuItem.Enabled = true;
            nextTurnFromReportToolStripMenuItem.Enabled = true;
            campaignStateToolStripMenuItem.Enabled = true;
            unitsToCSVToolStripMenuItem.Enabled = true;
            saveAsToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            using (NewFromReport newFromReport = new NewFromReport()) {
                if (newFromReport.ShowDialog() == DialogResult.OK) {
                    LoadCampaign(newFromReport.NewCampaign);
                }
            }
        }

        private void nextTurnFromReportToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private string getPlural(int num) {
            return num == 1 ? "" : "s";
        }

        private void oobTree_AfterSelect(object sender, TreeViewEventArgs e) {
            if(_forces == null) { return;  }

            Force currentForce = _forces[(string)forceSelector.SelectedItem];

            object obj = SelectUnitFromOOB();
            ammoGrid.Rows.Clear();
   
            if(obj != null) {
                _selectedUnit = obj;
                utmXBox.Enabled = !_campaign.ReadOnly && !(obj is Company);
                utmYBox.Enabled = !_campaign.ReadOnly && !(obj is Company);

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

                    labelCompanyHeader.Visible = u.Company != "";
                    labelPlatoonHeader.Visible = u.Platoon != "";
                    labelSectionHeader.Visible = u.Section != "";
                    labelTeamHeader.Visible = u.Team != "";

                    labelKillCount.Text = $"{u.GetVehicleKills()} / {u.Kills_Personnel}";

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
                    labelCompanyHeader.Visible = true;
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

                        labelPlatoonHeader.Visible = false;
                        labelSectionHeader.Visible = false;
                        labelTeamHeader.Visible = false;

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

                        labelPlatoonHeader.Visible = true;
                        labelSectionHeader.Visible = false;
                        labelTeamHeader.Visible = false;

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

                    int killsVeh = 0;
                    int killsPers = 0;

                    foreach (Unit u in members) {
                        killsVeh += u.GetVehicleKills();
                        killsPers += u.Kills_Personnel;

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

                    labelKillCount.Text = $"{killsVeh} / {killsPers}";

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

                labelCompanyHeader.Visible = false;
                labelPlatoonHeader.Visible = false;
                labelSectionHeader.Visible = false;
                labelTeamHeader.Visible = false;
            }

            ammoGrid.ClearSelection();
            eventGrid.ClearSelection();
        }

        private void ammoGrid_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            ammoGrid.ClearSelection();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void mapShowSelector_SelectedIndexChanged(object sender, EventArgs e) {
            mapPanel.Invalidate();
        }

        private void editCallsignTemplateToolStripMenuItem_Click(object sender, EventArgs e) {
            SetCallsignTemplates setCallsignTemplates = new SetCallsignTemplates(_campaign);
            setCallsignTemplates.ShowDialog();
            setCallsignTemplates.Dispose();
            PopulateOOBTree();
            mapPanel.Invalidate();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            if(_currentCampaignFileName == "") {
                SaveAs();
            } else {
                Save();
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e) {
            using (OpenFileDialog dialog = new OpenFileDialog()) {
                dialog.Filter = "Campaigns (*.cam)|*.cam";
                dialog.FilterIndex = 0;
                dialog.RestoreDirectory = true;

                // Show the dialog and check if the user clicked OK
                if (dialog.ShowDialog() == DialogResult.OK && dialog.FileName != "") {
                    LoadCampaign(
                        Campaign.Deserialize(dialog.FileName)
                    );
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveAs();
        }

        private void SaveAs() {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog()) { 
                saveFileDialog.Filter = "Campaigns (*.cam)|*.cam";
                saveFileDialog.Title = "Save campaign file";
                saveFileDialog.ShowDialog();

                // If the file name is not an empty string open it for saving.
                if (saveFileDialog.FileName != "") {
                    _currentCampaignFileName = saveFileDialog.FileName;
                    Save();
                }
            }
        }

        private void Save() {
            _campaign.Serialize(_currentCampaignFileName);
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e) {
            Rectangle rect = mapPanel.ClientRectangle;
            _mapCenter = new Point(
                rect.Left + (int)((rect.Right - rect.Left) * 0.5),
                rect.Top + (int)((rect.Bottom - rect.Top) * 0.5)
            );
            mapPanel.Invalidate();
        }

        private void recalibrateMapImageToolStripMenuItem_Click(object sender, EventArgs e) {
            using(CalibrateMapImage cmi = new CalibrateMapImage(_campaign.MapImage)) {
                cmi.ShowDialog();
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) {
            _mapTransform?.Dispose();
        }

        private void utmXBox_Leave(object sender, EventArgs e) {
            try {
                int utm_x = int.Parse(utmXBox.Text.Trim());
                if(_selectedUnit != null) {
                    if(_selectedUnit is Unit) {
                        Unit u = (Unit)_selectedUnit;
                        u.UTM_X = utm_x;
                    } else if(_selectedUnit is Platoon) {
                        Platoon p = (Platoon)_selectedUnit;
                        p.UTM_X = utm_x;
                    }
                    mapPanel.Invalidate();
                }
            } catch {
                // Ignore
            }
        }

        private void utmYBox_Leave(object sender, EventArgs e) {
            try {
                int utm_y = int.Parse(utmYBox.Text.Trim());
                if (_selectedUnit != null) {
                    if (_selectedUnit is Unit) {
                        Unit u = (Unit)_selectedUnit;
                        u.UTM_Y = utm_y;
                    } else if (_selectedUnit is Platoon) {
                        Platoon p = (Platoon)_selectedUnit;
                        p.UTM_Y = utm_y;
                    }
                    mapPanel.Invalidate();
                }
            } catch {
                // Ignore
            }
        }
    }
}
