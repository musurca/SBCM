using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
                rect.Left + (int)((rect.Right-rect.Left)*0.5), 
                rect.Top + (int)((rect.Bottom-rect.Top)*0.5)
            );
            _mapAnchor = new Point(0, 0);

            // Load Report
            _players = new Dictionary<string, Player>();
            _forces = new Dictionary<string, Force>();
            ReportParser.MergeReport(
                "B26 Smash & Crush v1.0.0.sce_11_12-20-22_23_07_07.htm",
                _players,
                _forces,
                true
            );

            foreach(string forceName in _forces.Keys ) {
                forceSelector.Items.Add( forceName );
            }
            forceSelector.SelectedItem = _forces.Keys.ToList()[0];
        }

        private void PopulateOOBTree() {
            Force currentForce = _forces[(string)forceSelector.SelectedItem];
            Battalion b = currentForce.Hierarchy;
            oobTree.BeginUpdate();
            oobTree.Nodes.Clear();
            foreach (Company c in b.Companies.Values) {
                TreeNode companyNode = oobTree.Nodes.Add(c.ID);
                if (c.CO != null) {
                    companyNode.Nodes.Add($"{c.CO.Callsign} ({c.CO.Type})");
                }
                if (c.XO != null) {
                    companyNode.Nodes.Add($"{c.XO.Callsign} ({c.XO.Type})");
                }

                foreach (Platoon p in c.Platoons.Values) {
                    if (currentForce.GenerateCallsign(out string platoonCallsign, c.ID, p.ID)) {
                        TreeNode platoonNode = companyNode.Nodes.Add(platoonCallsign);
                        
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
                                TreeNode sectionNode = platoonNode.Nodes.Add(unitID);
                                foreach (Unit sub_u in p.Members) {
                                    if (sub_u.Team != "" && sub_u.Section == u.Section) {
                                        unitID = $"{sub_u.Callsign} ({sub_u.Type})";
                                        sectionNode.Nodes.Add(unitID);
                                        allMembers.Remove(sub_u);
                                    }
                                }
                                allMembers.Remove(u);
                            }
                        }

                        // Team units without a parent section
                        foreach(Unit u in allMembers) {
                            string unitID = $"{u.Callsign} ({u.Type})";
                            if (p.CO == u) {
                                unitID += " (CO)";
                            } else if (p.XO == u) {
                                unitID += " (XO)";
                            }
                            TreeNode sectionNode = platoonNode.Nodes.Add(unitID);
                        }
                    }
                }
            }
            oobTree.EndUpdate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            //g.TranslateTransform(-_mapCenter.X, -_mapCenter.Y);
            g.ScaleTransform(_scale, _scale);
            g.TranslateTransform((int)(_mapAnchor.X*_scale), (int)(_mapAnchor.Y*_scale));
            g.DrawImage(_testMap, new Point((int)(-_mapCenter.X*_scale), (int)(-_mapCenter.Y*_scale)));
            
            
            //g.DrawEllipse(Pens.Firebrick, rect);
            //using (Pen pen = new Pen(Color.DarkBlue, 4f)) g.DrawLine(pen, 22, 22, 88, 88);
        }

        private void mapPanel_MouseMove(object sender, MouseEventArgs e) {
            if(_mouseDown) {
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
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void nextTurnFromReportToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e) {

        }
    }
}
