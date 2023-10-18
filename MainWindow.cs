using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SBCM {
    public partial class MainWindow : Form {
        float _scale;
        Bitmap _mapImage;
        PointF _mapAnchor;
        PointF _mapCenter;
        bool _mousePanning;
        bool _mouseMeasuring;
        float _xScroll, _yScroll;

        Point _measureUTM_0;
        Point _measureUTM_1;

        Point _startPoint;

        string _currentCampaignFileName;

        Dictionary<string, Force> _forces = new Dictionary<string, Force>();

        Dictionary<string, object> _oobToUnit;
        Campaign _campaign;

        Point _selectedUTMCoord;
        object _selectedUnit;
        ShotEvent _selectedEvent;

        Matrix _mapTransform;

        Dictionary<Rectangle, object> _selectionRects = new Dictionary<Rectangle, object>();

        bool _unsavedWork;

        // Determine counter size from longest possible callsign
        float _counterWidth;
        float _counterHeight;

        // Brushes
        Font _blockFont = new Font(SystemFonts.DefaultFont.FontFamily, 12.0f);
        Font _measureFont = new Font(SystemFonts.DefaultFont.FontFamily, 10.0f);
        Pen _whitePen = new Pen(Color.White);
        Pen _redPen = new Pen(Color.Red, 2.0f);
        Pen _explosionOutline = new Pen(Color.MediumVioletRed);
        Pen _measurePen = new Pen(Color.Gray, 2.0f);
        SolidBrush _blackBrush = new SolidBrush(Color.Black);
        SolidBrush _whiteBrush = new SolidBrush(Color.White);
        SolidBrush _selectedBrush = new SolidBrush(Color.DarkSeaGreen);
        SolidBrush _deadBrush = new SolidBrush(Color.Gray);
        SolidBrush _deadSelectBrush = new SolidBrush(Color.DarkGray);
        SolidBrush _damagedBrush = new SolidBrush(Color.DarkGoldenrod);
        SolidBrush _damageSelectBrush = new SolidBrush(Color.Goldenrod);
        SolidBrush _blockBrush = new SolidBrush(Color.DarkGreen);
        SolidBrush _fontBrush = new SolidBrush(Color.White);
        SolidBrush _explosionBrush = new SolidBrush(Color.Red);

        public MainWindow() {
            InitializeComponent();
            KeyPreview = true;

            mapPanel.MouseWheel += mapPanel_Scroll;

            typeof(Panel).InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                mapPanel,
                new object[] { true }
            );

            _mapImage = null;
            _scale = 1.0f;
            _mousePanning = false;
            _mouseMeasuring = false;
            _xScroll = _yScroll = 0.0f;

            _mapAnchor = new PointF();
            _mapCenter = new PointF();
            _selectedUTMCoord = new Point();
            _measureUTM_0 = new Point();
            _measureUTM_1 = new Point();

            campaignName.Text = "";
            campaignDate.Text = "";
            turnCounter.Items.Clear();
            turnCounter.Enabled = false;
            labelUTMX.Text = "";
            labelUTMY.Text = "";

            _currentCampaignFileName = "";

            Rectangle rect = mapPanel.ClientRectangle;
            _mapCenter.X = rect.Left + ((rect.Right - rect.Left) * 0.5f);
            _mapCenter.Y = rect.Top + ((rect.Bottom - rect.Top) * 0.5f);

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

            mapPanelContextMenu.Enabled = false;

            _unsavedWork = false;

            mapShowSelector.SelectedItem = mapShowSelector.Items[0];
        }

        private void BuildCounterSize() {
            Force currentForce = _forces[(string)forceSelector.SelectedItem];
            string longestPossibleCallsign = _campaign.CallsignTemplates[currentForce.Name]
            .LongestPossibleCallsign;
            SizeF csSize;
            using (Graphics g = Graphics.FromImage(new Bitmap(1, 1))) {
                csSize = g.MeasureString(longestPossibleCallsign, _blockFont);
            }
            _counterWidth = csSize.Width + (2.0f / _scale);
            _counterHeight = (float)Math.Max(
                csSize.Height, csSize.Width * 0.5625
            );
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

        private object PickUnitFromOOB() {
            string selected = oobTree.SelectedNode.Text;
            if(_oobToUnit.ContainsKey(selected)) {
                return _oobToUnit[selected];
            }
            return null;
        }

        private void PopulateTurnCounter() {
            List<Turn> allTurns = _campaign.Turns;
            turnCounter.BeginUpdate();
            turnCounter.Items.Clear();
            foreach(Turn t in allTurns) {
                if(t.Index == 0) {
                    turnCounter.Items.Add("Setup Phase");
                } else {
                    turnCounter.Items.Add($"Turn {t.Index}");
                }
            }
            turnCounter.EndUpdate();
            turnCounter.Enabled = turnCounter.Items.Count > 1;
        }

        private void PopulateOOBTree() {
            if (_forces != null && forceSelector.SelectedItem != null) {
                Force currentForce = _forces[(string)forceSelector.SelectedItem];
                _oobToUnit = ViewFuncs.PopulateTreeViewWithOOB(oobTree, currentForce);
            }
        }

        private void RebuildSelectionRects() {
            if (_campaign == null) { return; }
            
            MapImage map = _campaign.MapImage;

            Force currentForce = _forces[(string)forceSelector.SelectedItem];

            float halfCounterWidth = _counterWidth / 2.0f;
            float halfCounterHeight = _counterHeight / 2.0f;

            _selectionRects.Clear();
            string mapViewSelect = mapShowSelector.SelectedItem.ToString();
            if (mapViewSelect == "Platoons") {
                foreach (Company c in currentForce.Hierarchy.Companies.Values) {
                    foreach (Platoon p in c.Platoons.Values) {
                        if (p.UTM_X != -1 && p.UTM_Y != -1) {
                            map.UTMToImage(
                                p.UTM_X, p.UTM_Y,
                                out float x_pos, out float y_pos
                            );

                            Rectangle key = new Rectangle(
                                (int)(x_pos - halfCounterWidth),
                                (int)(y_pos - halfCounterHeight),
                                (int)(_counterWidth),
                                (int)(_counterHeight)
                            );

                            if(_selectionRects.ContainsKey(key)) {
                                _selectionRects[key] = p;
                            } else {
                                _selectionRects.Add(key, p);
                            }
                        }
                    }
                }
            } else if (mapViewSelect == "All Units") {
                foreach (Unit u in currentForce.Units.Values) {
                    if (u.UTM_X != -1 && u.UTM_Y != -1) {
                        map.UTMToImage(
                            u.UTM_X, u.UTM_Y,
                            out float x_pos, out float y_pos
                        );

                        Rectangle key = new Rectangle(
                                (int)(x_pos - halfCounterWidth),
                                (int)(y_pos - halfCounterHeight),
                                (int)(_counterWidth),
                                (int)(_counterHeight)
                            );

                        if (_selectionRects.ContainsKey(key)) {
                            _selectionRects[key] = u;
                        } else {
                            _selectionRects.Add(key, u);
                        }
                    }
                }
            }
        }

        private void DrawMapToGraphics(MapImage map, Graphics g, float scale=1.0f) {
            g.DrawImage(map.GetBitmap(), 0.0f, 0.0f);

            // Draw units/platoons on the map
            string mapViewSelect = mapShowSelector.SelectedItem.ToString();
            if (map.UTM_X_Step != 0.0f && map.UTM_Y_Step != 0.0f && mapViewSelect != "Nothing") {
                Force currentForce = _forces[(string)forceSelector.SelectedItem];

                float halfCounterWidth = _counterWidth / 2.0f;
                float halfCounterHeight = _counterHeight / 2.0f;
                float oneOverScale = 1.0f / scale;
                float twoOverScale = 2.0f * oneOverScale;
                float fiveOverScale = 5.0f * oneOverScale;

                Pen outlinePen = new Pen(Color.White, oneOverScale);

                if (mapViewSelect == "Platoons") {
                    foreach (Company c in currentForce.Hierarchy.Companies.Values) {
                        foreach (Platoon p in c.Platoons.Values) {
                            SolidBrush brush = _selectedUnit == p ? _selectedBrush : _blockBrush;
                            if (p.UTM_X != -1 && p.UTM_Y != -1) {
                                map.UTMToImage(
                                    p.UTM_X, p.UTM_Y,
                                    out float x_pos, out float y_pos
                                );

                                g.FillRectangle(
                                    brush,
                                    x_pos - halfCounterWidth,
                                    y_pos - halfCounterHeight,
                                    _counterWidth, _counterHeight
                                );

                                g.DrawRectangle(
                                    outlinePen,
                                    x_pos - halfCounterWidth - oneOverScale,
                                    y_pos - halfCounterHeight - oneOverScale,
                                    _counterWidth + twoOverScale, _counterHeight + twoOverScale
                                ); ;

                                currentForce.GenerateCallsign(
                                    out string callsign,
                                    c.ID, p.ID
                                );

                                SizeF callSize = g.MeasureString(
                                    callsign,
                                    _blockFont
                                );

                                g.DrawString(
                                    callsign,
                                    _blockFont, _fontBrush,
                                    x_pos - callSize.Width / 2.0f,
                                    y_pos - callSize.Height / 2.0f
                                );
                            }
                        }
                    }
                } else {
                    foreach (Unit u in currentForce.Units.Values) {
                        SolidBrush brush = _selectedUnit == u
                            ? (
                                u.Damage.Destroyed
                                ? _deadSelectBrush
                                : (
                                    u.Damage.IsDamaged()
                                    ? _damageSelectBrush
                                    : _selectedBrush
                                  )
                               )
                            : (
                                u.Damage.Destroyed
                                ? _deadBrush
                                : (
                                   u.Damage.IsDamaged()
                                   ? _damagedBrush
                                   : _blockBrush
                                  )
                              );

                        if (u.UTM_X != -1 && u.UTM_Y != -1) {
                            map.UTMToImage(
                                u.UTM_X, u.UTM_Y,
                                out float x_pos, out float y_pos
                            );

                            g.FillRectangle(
                                brush,
                                x_pos - halfCounterWidth,
                                y_pos - halfCounterHeight,
                                _counterWidth, _counterHeight
                            );

                            g.DrawRectangle(
                                outlinePen,
                                x_pos - halfCounterWidth - oneOverScale,
                                y_pos - halfCounterHeight - oneOverScale,
                                _counterWidth + twoOverScale, _counterHeight + twoOverScale
                            );

                            SizeF callSize = g.MeasureString(
                                u.Callsign,
                                _blockFont
                            );

                            g.DrawString(
                                u.Callsign,
                                _blockFont, _fontBrush,
                                x_pos - callSize.Width / 2.0f,
                                y_pos - callSize.Height / 2.0f
                           );
                        }
                    }
                }

                if (_selectedEvent != null) {
                    Pen explosionPen = new Pen(Color.Red, twoOverScale);

                    map.UTMToImage(
                        _selectedEvent.From_UTM_X, _selectedEvent.From_UTM_Y,
                        out float from_x_pos, out float from_y_pos
                    );

                    map.UTMToImage(
                        _selectedEvent.To_UTM_X, _selectedEvent.To_UTM_Y,
                        out float to_x_pos, out float to_y_pos
                    );

                    float tenOverScale = 2.0f * fiveOverScale;

                    g.DrawLine(explosionPen, from_x_pos, from_y_pos, to_x_pos, to_y_pos);
                    g.DrawEllipse(_explosionOutline, to_x_pos - fiveOverScale, to_y_pos - fiveOverScale, tenOverScale, tenOverScale);
                    g.FillEllipse(_explosionBrush, to_x_pos - fiveOverScale, to_y_pos - fiveOverScale, tenOverScale, tenOverScale);

                    explosionPen.Dispose();
                }

                outlinePen.Dispose();
            }
        }

        private void DisposeOfBrushes() {
            _blockFont.Dispose();
            _measureFont.Dispose();

            // Dispose of brushes
            _blackBrush.Dispose();
            _whiteBrush.Dispose();
            _selectedBrush.Dispose();
            _blockBrush.Dispose();
            _fontBrush.Dispose();
            _deadBrush.Dispose();
            _deadSelectBrush.Dispose();
            _damagedBrush.Dispose();
            _damageSelectBrush.Dispose();
            _whitePen.Dispose();
            _redPen.Dispose();
            _measurePen.Dispose();
            _explosionOutline.Dispose();
            _explosionBrush.Dispose();
        }

        private void panel1_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.FillRectangle(_blackBrush, mapPanel.ClientRectangle);

            if (_mapImage != null) {
                // Center map, scale by zoom, then move to anchor
                g.TranslateTransform(_mapCenter.X, _mapCenter.Y);
                g.ScaleTransform(_scale, _scale);
                g.TranslateTransform(_mapAnchor.X, _mapAnchor.Y);

                // Store inverse of transform
                _mapTransform?.Dispose();
                _mapTransform = g.Transform;
                _mapTransform.Invert();

                // Draw the map
                // Border
                g.DrawRectangle(
                    _whitePen,
                    -5.0f,
                    -5.0f,
                    _mapImage.Width + 10.0f,
                    _mapImage.Height + 10.0f
                );

                DrawMapToGraphics(_campaign.MapImage, g, _scale);
                
                if(_mouseMeasuring) {
                    _campaign.MapImage.UTMToImage(
                        _measureUTM_0.X, 
                        _measureUTM_0.Y, 
                        out float x0, 
                        out float y0
                    );

                    _campaign.MapImage.UTMToImage(
                        _measureUTM_1.X,
                        _measureUTM_1.Y,
                        out float x1,
                        out float y1
                    );

                    int dist_m = _campaign.MapImage.UTM_Distance(
                        _measureUTM_0.X, _measureUTM_0.Y, 
                        _measureUTM_1.X, _measureUTM_1.Y
                    );

                    string distStr = $"{dist_m} m";

                    SizeF distStrSize = g.MeasureString(
                        distStr,
                        _measureFont
                    );

                    g.DrawLine(_measurePen, x0, y0, x1, y1);
                    g.DrawString(
                        distStr,
                        _measureFont,
                        _whiteBrush,
                        x1 - distStrSize.Width * 0.5f,
                        y1 - distStrSize.Height - (2.0f / _scale)
                    );
                }
            }
        }

        private Point _originPt;
        private Point[] _tempTransformPts;
        private Point PanelToImage(Point panelPoint) {
            if (_mapTransform != null) {
                if(_tempTransformPts == null) {
                    _originPt = new Point(0, 0);
                    _tempTransformPts = new Point[1];
                }
                _tempTransformPts[0] = panelPoint;
                _mapTransform.TransformPoints(_tempTransformPts);
                return _tempTransformPts[0];
            }
            _originPt.X = 0;
            _originPt.Y = 0;
            return _originPt;
        }

        private void mapPanel_MouseMove(object sender, MouseEventArgs e) {
            if(_mapImage == null) {
                labelUTMX.Text = "";
                labelUTMY.Text = ""; 
                return; 
            }

            MapImage map = _campaign.MapImage;

            map.ImageToUTM(
                PanelToImage(e.Location),
                out int utm_x, out int utm_y
            );

            if (_mousePanning) {
                // Pan the map

                Point curLoc = e.Location;
                _mapAnchor.X += ((e.Location.X - _startPoint.X) / _scale);
                _mapAnchor.Y += ((e.Location.Y - _startPoint.Y) / _scale);
                _mapAnchor.X = Math.Max(-_mapImage.Width, Math.Min(0, _mapAnchor.X));
                _mapAnchor.Y = Math.Max(-_mapImage.Height, Math.Min(0, _mapAnchor.Y));
                _startPoint = curLoc;
            }
            
            if(_mouseMeasuring) {
                _measureUTM_1.X = utm_x;
                _measureUTM_1.Y = utm_y;
            }

            if(_mousePanning || _mouseMeasuring) {
                mapPanel.Invalidate();
            }

            // Update UTM coords under mouse
            labelUTMX.Text = utm_x.ToString();
            labelUTMY.Text = utm_y.ToString();
        }

        private void mapPanel_MouseDown(object sender, MouseEventArgs e) {
            if(_mapImage == null) { return;  }

            if(e.Button == MouseButtons.Left) {
                // See if the mouse clicked a unit's selection rect
                Object obj = null;
                Point imagePt = PanelToImage(e.Location);
                foreach(Rectangle key in _selectionRects.Keys) {
                    if(key.Contains(imagePt)) {
                        obj = _selectionRects[key];
                    }
                }
                if ((obj == null && _selectedUnit == null) || (obj == _selectedUnit)) {
                    // If nothing selected/deselected, start measuring
                    _mouseMeasuring = true;

                    MapImage map = _campaign.MapImage;

                    map.ImageToUTM(
                        PanelToImage(e.Location),
                        out int utm_x, out int utm_y
                    );

                    _measureUTM_0.X = utm_x;
                    _measureUTM_0.Y = utm_y;
                    _measureUTM_1.X = utm_x;
                    _measureUTM_1.Y = utm_y;
                } else {
                    // Otherwise select/deselect the unit
                    SelectUnit(obj, false);
                }
            }
            
            if (e.Button == MouseButtons.Middle) {
                // Start map pan
                _startPoint = e.Location;
                _mousePanning = true;
            }
            
            if(e.Button == MouseButtons.Right) {
                // Select a UTM coordinate for moving a unit
                MapImage map = _campaign.MapImage;

                map.ImageToUTM(
                    PanelToImage(e.Location),
                    out int utm_x, out int utm_y
                );

                _selectedUTMCoord.X = utm_x;
                _selectedUTMCoord.Y = utm_y;
            }
        }

        private void mapPanel_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if(_mouseMeasuring) {
                    // Stop measurement
                    _mouseMeasuring = false;
                    mapPanel.Invalidate();
                }
            }
            
            if (e.Button == MouseButtons.Middle) {
                // Stop map pan
                _mousePanning = false;
            }
        }

        private void mapPanel_Scroll(object sender, MouseEventArgs e) {
            // Map zoom
            _scale = Math.Min(10.0f, Math.Max(0.5f, _scale * (1.0f + e.Delta / 5000.0f)));

            _blockFont.Dispose();
            _blockFont = new Font(SystemFonts.DefaultFont.FontFamily, 12.0f / _scale);
            _measureFont.Dispose();
            _measureFont = new Font(SystemFonts.DefaultFont.FontFamily, 10.0f / _scale);
            _measurePen.Dispose();
            _measurePen = new Pen(Color.Gray, 2.0f / _scale);

            BuildCounterSize();

            RebuildSelectionRects();
            mapPanel.Invalidate();
        }

        private void PopulateEvents() {
            Force currentForce = null;
            if (forceSelector.SelectedItem != null) {
                if (_forces.ContainsKey(forceSelector.SelectedItem.ToString())) {
                    currentForce = _forces[forceSelector.SelectedItem.ToString()];
                }
            }
            eventGrid.Rows.Clear();
            if (currentForce != null) {
                foreach (ShotEvent evt in currentForce.GetEvents()) {
                    eventGrid.Rows.Add(evt.Time, evt.EventText);
                }
            }
            eventGrid.ClearSelection();
        }

        private void PopulatePlayers() {
            Force currentForce = null;
            if (forceSelector.SelectedItem != null) {
                if (_forces.ContainsKey(forceSelector.SelectedItem.ToString())) {
                    currentForce = _forces[forceSelector.SelectedItem.ToString()];
                }
            }

            playerGrid.Rows.Clear();
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
        }

        private void RefreshForce() {
            PopulateEvents();
            PopulatePlayers();

            // Populate force OOB
            PopulateOOBTree();

            RebuildSelectionRects();
            mapPanel.Invalidate();
        }

        private void forceSelector_SelectedIndexChanged(object sender, EventArgs e) {
            RefreshForce();
        }

        private void LoadTurn(Turn curTurn) {
            DateTime turnTime = curTurn.CurrentTime;

            string month = CultureInfo.CurrentCulture.
                DateTimeFormat.GetMonthName(turnTime.Month);

            campaignDate.Text = $"{month} {turnTime.Day}, {turnTime.Year} {turnTime.ToString("HH:mm")}";

            _forces = curTurn.Forces;

            string currentForceName = "";
            if(forceSelector.SelectedItem != null) {
                currentForceName = forceSelector.SelectedItem.ToString();
            }

            forceSelector.Items.Clear();
            foreach (string forceName in _forces.Keys) {
                forceSelector.Items.Add(forceName);
            }
            if (currentForceName == "") {
                forceSelector.SelectedItem = _forces.Keys.ToList()[0];
            } else {
                forceSelector.SelectedItem = currentForceName;
            }

            SelectUnit(null, false);
            RefreshForce();
        }

        private void LoadCampaign(Campaign campaign) {
            _campaign?.Dispose();

            _unsavedWork = false;
            
            _campaign = campaign;
            campaignName.Text = _campaign.Name;
            _mapImage = _campaign.MapImage.GetBitmap();
            _mapAnchor.X = -_mapImage.Width / 2;
            _mapAnchor.Y = -_mapImage.Height / 2;

            editCallsignTemplateToolStripMenuItem.Enabled = true;
            recalibrateMapImageToolStripMenuItem.Enabled = true;
            nextTurnFromReportToolStripMenuItem.Enabled = true;
            campaignStateToolStripMenuItem.Enabled = true;
            unitsToCSVToolStripMenuItem.Enabled = true;
            saveAsToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            imageFromMapToolStripMenuItem.Enabled = true;

            btnMapExport.Enabled = true;

            PopulateTurnCounter();
            Turn curTurn = _campaign.GetLastTurn();
            turnCounter.SelectedItem = turnCounter.Items[curTurn.Index];

            BuildCounterSize();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            if(_unsavedWork) {
                DialogResult dr = GiveOptionToSave();
                if (dr == DialogResult.Yes) {
                    SaveOrSaveAs();
                } else if (dr == DialogResult.Cancel) {
                    return;
                }
            }

            using (NewFromReport newFromReport = new NewFromReport()) {
                if (newFromReport.ShowDialog() == DialogResult.OK) {
                    LoadCampaign(newFromReport.NewCampaign);
                    _unsavedWork = true;
                }
            }
        }

        private void nextTurnFromReportToolStripMenuItem_Click(object sender, EventArgs e) {
            using (NextTurnFromReport nextTurnDialog = new NextTurnFromReport(_campaign)) {
                if (nextTurnDialog.ShowDialog() == DialogResult.OK) {
                    PopulateTurnCounter();
                    turnCounter.SelectedItem = turnCounter.Items[turnCounter.Items.Count - 1];
                }
            }
        }

        private string getPlural(int num) {
            return num == 1 ? "" : "s";
        }

        private void SelectUnit(object obj, bool centerOnMap) {
            Force currentForce = _forces[(string)forceSelector.SelectedItem];

            ammoGrid.Rows.Clear();

            _selectedUnit = obj;
            _selectedEvent = null;

            if (obj != null) {
                mapPanelContextMenu.Enabled = true;

                utmXBox.Enabled = !_campaign.ReadOnly && !(obj is Company);
                utmYBox.Enabled = !_campaign.ReadOnly && !(obj is Company);

                int map_x = -1, map_y = -1;

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
                                float pct = 0.0f;
                                if (ammo.Maximum > 0) {
                                    pct = ammo.Amount / (float)ammo.Maximum;
                                }
                                int pct_rnd = (int)Math.Round(pct*100, 0);
                                int rowIndex = ammoGrid.Rows.Add(ammo.Type, ammo.Amount.ToString(), $"{pct_rnd}%");

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

                    map_x = u.UTM_X;
                    map_y = u.UTM_Y;

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
                        map_x = p.UTM_X;
                        map_y = p.UTM_Y;
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
                                if (dam.Immobilized) { 
                                    vehImmobilized++; 
                                } else if (dam.IsDamaged()) { 
                                    vehDamaged++; 
                                }
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
                        float pct = 0.0f;
                        if (ammo.Maximum > 0) {
                            pct = ammo.Amount / (float)ammo.Maximum;
                        }
                        int pct_rnd = (int)Math.Round(pct * 100, 0);
                        int rowIndex = ammoGrid.Rows.Add(ammo.Type, ammo.Amount.ToString(), $"{pct_rnd}%");

                        if (pct < 0.05f) {
                            ammoGrid.Rows[rowIndex].Cells[1].Style.ForeColor = Color.Red;
                        } else if (pct <= 0.5f) {
                            ammoGrid.Rows[rowIndex].Cells[1].Style.ForeColor = Color.DarkGoldenrod;
                        }
                    }
                }

                // Move camera to unit position
                if (centerOnMap) {
                    CenterMapOnUTM(map_x, map_y);
                }
            } else {
                mapPanelContextMenu.Enabled = false;

                statusStrength.Visible = false;
                statusDestroyed.Visible = false;
                statusGroup.Text = "";
                HideVehicleDamage();

                callsignLabel.Text = "";
                typeLabel.Text = "";
                companyLabel.Text = "";
                platoonLabel.Text = "";
                sectionLabel.Text = "";
                teamLabel.Text = "";

                utmXBox.Text = utmYBox.Text = "";
                utmXBox.Enabled = utmYBox.Enabled = false;

                labelCompanyHeader.Visible = false;
                labelPlatoonHeader.Visible = false;
                labelSectionHeader.Visible = false;
                labelTeamHeader.Visible = false;
            }

            ammoGrid.ClearSelection();
            eventGrid.ClearSelection();

            mapPanel.Invalidate();
        }

        private void CenterMapOnUTM(int utm_x, int utm_y) {
            if (utm_x != -1 && utm_y != -1) {
                if (_campaign.MapImage.WithinUTMExtents(utm_x, utm_y)) {
                    _campaign.MapImage.UTMToImage(
                        utm_x, utm_y,
                        out float image_x, out float image_y
                    );

                    _mapAnchor.X = -image_x;
                    _mapAnchor.Y = -image_y;
                }
            }
        }

        private void oobTree_AfterSelect(object sender, TreeViewEventArgs e) {
            if(_forces == null) { return;  }

            Force currentForce = _forces[(string)forceSelector.SelectedItem];

            SelectUnit(
                PickUnitFromOOB(),
                true
             );
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

            _unsavedWork = true;
        }

        private void SaveOrSaveAs() {
            if (_currentCampaignFileName == "") {
                SaveAs();
            } else {
                Save();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveOrSaveAs();
        }

        private DialogResult GiveOptionToSave() {
            return MessageBox.Show(
                "You have unsaved changes. Do you want to save this campaign before continuing?",
                "Unsaved changes",
                MessageBoxButtons.YesNoCancel
            );
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e) {
            if(_unsavedWork) {
                DialogResult dr = GiveOptionToSave();
                if(dr == DialogResult.Yes) {
                    SaveOrSaveAs();
                } else if(dr == DialogResult.Cancel) {
                    return;
                }
            }

            using (OpenFileDialog dialog = new OpenFileDialog()) {
                dialog.Filter = "Campaigns (*.cam)|*.cam";
                dialog.FilterIndex = 0;
                dialog.RestoreDirectory = true;

                // Show the dialog and check if the user clicked OK
                if (dialog.ShowDialog() == DialogResult.OK && dialog.FileName != "") {
                    try {
                        LoadCampaign(
                            Campaign.Deserialize(dialog.FileName)
                        );
                        _currentCampaignFileName = dialog.FileName;
                    } catch {
                        MessageBox.Show(
                            "Can't load that campaign file. It may be corrupted.", 
                            "Error",
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error
                        );
                    }
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
            try {
                _campaign.Serialize(_currentCampaignFileName);

                _unsavedWork = false;
            } catch {
                MessageBox.Show(
                    "Can't save the campaign file. Your hard drive may be out of space, or you might not have access to that folder.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e) {
            Rectangle rect = mapPanel.ClientRectangle;
            _mapCenter.X = rect.Left + ((rect.Right - rect.Left) * 0.5f);
            _mapCenter.Y = rect.Top + ((rect.Bottom - rect.Top) * 0.5f);
            mapPanel.Invalidate();
        }

        private void recalibrateMapImageToolStripMenuItem_Click(object sender, EventArgs e) {
            using(CalibrateMapImage cmi = new CalibrateMapImage(_campaign.MapImage)) {
                if(cmi.ShowDialog() == DialogResult.OK) {
                    _unsavedWork = true;
                }
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) {
            if (_unsavedWork) {
                DialogResult dr = GiveOptionToSave();
                if (dr == DialogResult.Yes) {
                    SaveOrSaveAs();
                } else if (dr == DialogResult.Cancel) {
                    e.Cancel = true;
                    return;
                }
            }

            _campaign?.Dispose();
            _mapTransform?.Dispose();
            DisposeOfBrushes();
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
                        foreach (Unit u in p.Members) {
                            if (u.UTM_X == -1) {
                                u.UTM_X = utm_x;
                            }
                        }
                        p.UpdatePosition();
                    }
                    RebuildSelectionRects();
                    mapPanel.Invalidate();

                    _unsavedWork = true;
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
                        foreach (Unit u in p.Members) {
                            if (u.UTM_Y == -1) {
                                u.UTM_Y = utm_y;
                            }
                        }
                        p.UpdatePosition();
                    }
                    RebuildSelectionRects();
                    mapPanel.Invalidate();

                    _unsavedWork = true;
                }
            } catch {
                // Ignore
            }
        }

        private void moveUnitMenuItem_Click(object sender, EventArgs e) {
            if (_selectedUnit != null) {
                if (_selectedUnit is Unit) {
                    Unit u = (Unit)_selectedUnit;
                    
                    u.UTM_X = _selectedUTMCoord.X;
                    u.UTM_Y = _selectedUTMCoord.Y;

                } else if (_selectedUnit is Platoon) {
                    Platoon p = (Platoon)_selectedUnit;

                    int sample_x = 0, sample_y = 0, samples=0;
                    foreach (Unit u in p.Members) {
                        if (u.UTM_X != -1 && u.UTM_Y != -1) {
                            sample_x += u.UTM_X;
                            sample_y += u.UTM_Y;
                            samples++;
                        }
                    }
                    int addl_members = p.Members.Count - samples;

                    if (addl_members > 0) {
                        // Find new weighted position relative to where other members are
                        int weight_x = (_selectedUTMCoord.X * p.Members.Count - sample_x) / addl_members;
                        int weight_y = (_selectedUTMCoord.Y * p.Members.Count - sample_y) / addl_members;
                        foreach (Unit u in p.Members) {
                            if (u.UTM_X == -1 || u.UTM_Y == -1) {
                                u.UTM_X = weight_x;
                                u.UTM_Y = weight_y;
                            }
                        }
                    } else {
                        // Shift each member from weighted platoon center
                        int diff_x = _selectedUTMCoord.X - (sample_x / samples);
                        int diff_y = _selectedUTMCoord.Y - (sample_y / samples);

                        foreach (Unit u in p.Members) {
                            u.UTM_X += diff_x;
                            u.UTM_Y += diff_y;
                        }
                    }
                    p.UpdatePosition();
                }

                utmXBox.Text = _selectedUTMCoord.X.ToString();
                utmYBox.Text = _selectedUTMCoord.Y.ToString();

                RebuildSelectionRects();
                mapPanel.Invalidate();

                _unsavedWork = true;
            }
        }

        private void mapShow_SelectedIndexChanged(object sender, EventArgs e) {
            RebuildSelectionRects();
            mapPanel.Invalidate();
        }

        private void eventGrid_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            // Event clicked
            if(eventGrid.Rows.Count == 0) { return; }
            DataGridViewRow row = eventGrid.Rows[e.RowIndex];
            string time = row.Cells[0].Value.ToString();
            string evt_txt = row.Cells[1].Value.ToString();

            Force currentForce = _forces[(string)forceSelector.SelectedItem];
            foreach(ShotEvent evt in currentForce.GetEvents()) {
                if(time == evt.Time && evt_txt == evt.EventText) {
                    Unit u = currentForce.GetUnit(evt.Unit_Callsign);
                    SelectUnit(u, false);
                    _selectedEvent = evt;

                    int mid_x = (evt.From_UTM_X + evt.To_UTM_X) / 2;
                    int mid_y = (evt.From_UTM_Y + evt.To_UTM_Y) / 2;

                    CenterMapOnUTM(mid_x, mid_y);
                    mapPanel.Invalidate();
                    break;
                }
            }
        }

        private void eventGrid_CellClick(object sender, DataGridViewCellEventArgs e) {
            eventGrid_CellContentClick(sender, e);
        }

        private void campaignStateToolStripMenuItem_Click(object sender, EventArgs e) {
            Force currentForce = _forces[(string)forceSelector.SelectedItem];
            using(SaveFileDialog saveFileDialog = new SaveFileDialog()) {
                saveFileDialog.Filter = "Campaigns (*.cam)|*.cam";
                saveFileDialog.Title = $"Save campaign state for {currentForce.Name}";
                saveFileDialog.ShowDialog();

                // If the file name is not an empty string open it for saving.
                if (saveFileDialog.FileName != "") {
                    _campaign.CloneForSide(currentForce.Name).Serialize(saveFileDialog.FileName);
                }
            }
        }

        private void btnMapExport_Click(object sender, EventArgs e) {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog()) {
                saveFileDialog.Filter = "PNG file (*.png)|*.png";
                saveFileDialog.Title = $"Current map image";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "") {
                    using (Bitmap mapExport = new Bitmap(_mapImage)) {
                        using (Graphics g = Graphics.FromImage(mapExport)) {
                            DrawMapToGraphics(_campaign.MapImage, g);
                        }

                        try {
                            mapExport.Save(
                                saveFileDialog.FileName,
                                System.Drawing.Imaging.ImageFormat.Png
                            );
                        } catch {
                            MessageBox.Show(
                                "Can't save the image file. Your hard drive may be out of space, or you might not have access to that folder.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                }
            }
        }

        private void imageFromMapToolStripMenuItem_Click(object sender, EventArgs e) {
            btnMapExport_Click(sender, e);
        }

        private void unitsToCSVToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_forces != null && forceSelector.SelectedItem != null) {
                Force currentForce = _forces[(string)forceSelector.SelectedItem];

                using (SaveFileDialog saveFileDialog = new SaveFileDialog()) {
                    saveFileDialog.Filter = "Comma-separated value file (*.csv)|*.csv";
                    saveFileDialog.Title = $"CSV file";
                    saveFileDialog.ShowDialog();

                    if (saveFileDialog.FileName != "") {
                        try {
                            using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName)) {
                                writer.WriteLine(Unit.SerializeToCSVColumnHeadings());
                                foreach (Unit unit in currentForce.GetUnitList()) {
                                    writer.WriteLine(unit.SerializeToCSVRow());
                                }
                            }
                        } catch {
                            MessageBox.Show(
                               "Can't save the CSV file. Your hard drive may be out of space, or you might not have access to that folder.",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error
                           );
                        }
                    }
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            using(AboutSBCM aboutDialog = new AboutSBCM()) {
                aboutDialog.ShowDialog();
            }
        }

        private void turnCounter_SelectedIndexChanged(object sender, EventArgs e) {
            int index = turnCounter.Items.IndexOf(turnCounter.SelectedItem);
            LoadTurn(_campaign.GetTurn(index));
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Left) _xScroll -= 1.0f;
            if (e.KeyCode == Keys.Right) _xScroll += 1.0f;
            if (e.KeyCode == Keys.Up) _yScroll -= 1.0f;
            if (e.KeyCode == Keys.Down) _yScroll += 1.0f;

            _xScroll = Math.Max(-1.0f, Math.Min(1.0f, _xScroll));
            _yScroll = Math.Max(-1.0f, Math.Min(1.0f, _yScroll));

            if (((int)_xScroll) == 0 && ((int)_yScroll) == 0) {
                ScrollTimer.Enabled = false;
            } else {
                ScrollTimer.Enabled = true;
            }
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Left) _xScroll += 1.0f;
            if (e.KeyCode == Keys.Right) _xScroll -= 1.0f;
            if (e.KeyCode == Keys.Up) _yScroll += 1.0f;
            if (e.KeyCode == Keys.Down) _yScroll -= 1.0f;

            _xScroll = Math.Max(-1.0f, Math.Min(1.0f, _xScroll));
            _yScroll = Math.Max(-1.0f, Math.Min(1.0f, _yScroll));

            if (((int)_xScroll) == 0 && ((int)_yScroll) == 0) {
                ScrollTimer.Enabled = false;
            } else {
                ScrollTimer.Enabled = true;
            }
        }

        private void ScrollTimer_Tick(object sender, EventArgs e) {
            if (_mapImage != null) {
                _mapAnchor.X -= (15.0f * _xScroll / _scale);
                _mapAnchor.Y -= (15.0f * _yScroll / _scale);
                _mapAnchor.X = Math.Max(-_mapImage.Width, Math.Min(0, _mapAnchor.X));
                _mapAnchor.Y = Math.Max(-_mapImage.Height, Math.Min(0, _mapAnchor.Y));
                mapPanel.Invalidate();
            }
        }
    }
}
