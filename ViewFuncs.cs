using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBCM {
    public class ViewFuncs {
        public static Dictionary<string, object> PopulateTreeViewWithOOB(TreeView view, Force force) {
            Dictionary<string, object> oobToUnit = new Dictionary<string, object>();

            TreeNode AddOOBNode(TreeNodeCollection nodes, string nodeName, object unit) {
                if (!oobToUnit.ContainsKey(nodeName)) {
                    oobToUnit.Add(nodeName, unit);
                    return nodes.Add(nodeName);
                }
                
                return null;
            }

            Battalion b = force.Hierarchy;
            view.BeginUpdate();
            view.Nodes.Clear();
            oobToUnit.Clear();
            foreach (Company c in b.Companies.Values) {
                TreeNode companyNode = AddOOBNode(view.Nodes, c.ID, c);
                if (c.CO != null) {
                    AddOOBNode(companyNode.Nodes, $"{c.CO.Callsign} ({c.CO.Type})", c.CO);
                }
                if (c.XO != null) {
                    AddOOBNode(companyNode.Nodes, $"{c.XO.Callsign} ({c.XO.Type})", c.XO);
                }

                foreach (Platoon p in c.Platoons.Values) {
                    if (force.GenerateCallsign(out string platoonCallsign, c.ID, p.ID)) {
                        TreeNode platoonNode = AddOOBNode(companyNode.Nodes, platoonCallsign, p);
                        if (platoonNode == null) { continue; }

                        List<Unit> allMembers = new List<Unit>();
                        foreach (Unit u in p.Members) {
                            allMembers.Add(u);
                        }

                        foreach (Unit u in p.Members) {
                            if (u.Team == "") {
                                string sectionID = $"{u.Callsign} ({u.Type})";
                                if (p.CO == u) {
                                    sectionID += " (CO)";
                                } else if (p.XO == u) {
                                    sectionID += " (XO)";
                                }
                                TreeNode sectionNode = AddOOBNode(platoonNode.Nodes, sectionID, u);
                                if(sectionNode == null) { continue; }

                                if (!u.IsOperational()) {
                                    sectionNode.ForeColor = Color.LightGray;
                                } else if (
                                    u.Unit_Class != Unit.CLASS_PERSONNEL
                                    && u.GetDamageState().IsDamaged()
                                ) {
                                    sectionNode.ForeColor = Color.DarkGoldenrod;
                                }
                                foreach (Unit sub_u in p.Members) {
                                    if (sub_u.Team != "" && sub_u.Section == u.Section) {
                                        string teamID = $"{sub_u.Callsign} ({sub_u.Type})";
                                        TreeNode node = AddOOBNode(sectionNode.Nodes, teamID, sub_u);
                                        if (node == null) {
                                            continue;
                                        }

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
                            TreeNode node = AddOOBNode(platoonNode.Nodes, unitID, u);
                            if(node == null) { continue; }

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
            foreach (Unit u in b.Unattached) {
                AddOOBNode(view.Nodes, u.Callsign, u);
            }
            view.EndUpdate();

            return oobToUnit;
        }
    }
}
