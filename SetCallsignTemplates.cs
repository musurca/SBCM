using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SBCM {
    public partial class SetCallsignTemplates : Form {
        static readonly string DEFAULT_PATTERN_PLATOON  = "{company}-{platoon}";
        static readonly string DEFAULT_PATTERN_SECTION  = "{company}-{platoon}{section}";
        static readonly string DEFAULT_PATTERN_TEAM     = "{company}-{platoon}{section}/{team}";

        static readonly string[] DEFAULT_COMPANY_IDS = new string[] {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P"
        };
        static readonly string[] DEFAULT_PLATOON_IDS = new string[] {
            "1", "2", "3", "4", "5", "6", "7", "8", "9"
        };
        static readonly string[] DEFAULT_SECTION_IDS = new string[] {
            "1", "2", "3", "4", "5", "6", "7", "8", "9"
        };
        static readonly string[] DEFAULT_TEAM_IDS = new string[] {
            "A", "B", "C", "D"
        };

        static readonly string DEFAULT_COMPANY_CO = "66";
        static readonly string DEFAULT_COMPANY_XO = "65";
        static readonly string DEFAULT_PLATOON_CO = "Plt CO";
        static readonly string DEFAULT_PLATOON_XO = "Plt XO";

        Campaign _campaign;
        string _currentForceName;
        Dictionary<string, CallsignParser> _templates;
        Turn _lastTurn;

        List<TextBox> _companyIDs;
        List<TextBox> _platoonIDs;
        List<TextBox> _sectionIDs;
        List<TextBox> _teamIDs;

        public SetCallsignTemplates(Campaign campaign) {
            InitializeComponent();
            _currentForceName = "";

            _companyIDs = new List<TextBox> {
                companyIDBox01, companyIDBox02, companyIDBox03, companyIDBox04,
                companyIDBox05, companyIDBox06, companyIDBox07, companyIDBox08,
                companyIDBox09, companyIDBox10, companyIDBox11, companyIDBox12,
                companyIDBox13, companyIDBox14, companyIDBox15, companyIDBox16
            };
            _platoonIDs = new List<TextBox> {
                platoonIDBox01, platoonIDBox02, platoonIDBox03, platoonIDBox04,
                platoonIDBox05, platoonIDBox06, platoonIDBox07, platoonIDBox08,
                platoonIDBox09, platoonIDBox10, platoonIDBox11, platoonIDBox12,
                platoonIDBox13, platoonIDBox14, platoonIDBox15
            };
            _sectionIDs = new List<TextBox> {
                sectionIDBox01, sectionIDBox02, sectionIDBox03, sectionIDBox04,
                sectionIDBox05, sectionIDBox06, sectionIDBox07, sectionIDBox08,
                sectionIDBox09, sectionIDBox10, sectionIDBox11, sectionIDBox12,
                sectionIDBox13, sectionIDBox14, sectionIDBox15, sectionIDBox16
            };
            _teamIDs = new List<TextBox> {
                teamIDBox01, teamIDBox02, teamIDBox03, teamIDBox04
            };

            SetCampaign(campaign);
        }

        public void SetCampaign(Campaign campaign) {
            _campaign = campaign;
            _templates = new Dictionary<string, CallsignParser>();
            Dictionary<string, CallsignParser> existingTemplates = _campaign.CallsignTemplates;
            foreach (string key in existingTemplates.Keys) {
                _templates.Add(key, existingTemplates[key]);
            }
            _lastTurn = _campaign.GetLastTurn();

            CallsignParser defaultTemplate = MakeDefaultTemplate();

            sideSelector.Items.Clear();
            foreach(Force f in _lastTurn.Forces.Values) {
                if(!_templates.ContainsKey(f.Name)) {
                    _templates.Add(f.Name, defaultTemplate);
                }
                sideSelector.Items.Add(f.Name);
            }
            _currentForceName = sideSelector.Items[0].ToString();
            PopulateTextBoxes();

            sideSelector.SelectedItem = _currentForceName;
        }

        private CallsignParser MakeDefaultTemplate() {
            return new CallsignParser(
                DEFAULT_PATTERN_PLATOON, DEFAULT_PATTERN_SECTION, DEFAULT_PATTERN_TEAM,
                DEFAULT_COMPANY_IDS, DEFAULT_PLATOON_IDS, 
                DEFAULT_SECTION_IDS, DEFAULT_TEAM_IDS, 
                DEFAULT_COMPANY_CO, DEFAULT_COMPANY_XO, 
                DEFAULT_PLATOON_CO, DEFAULT_PLATOON_XO
            );
        }

        private void PopulateTextBoxes() {
            void PopulateTextBoxesFromArray(List<TextBox> boxlist, string[] symArray) {
                // Fill from the symbols we have
                for (int i = 0; i < symArray.Length; i++) {
                    boxlist[i].Text = symArray[i];
                }
                // Empty others
                for (int i = symArray.Length; i < boxlist.Count; i++) {
                    boxlist[i].Text = "";
                }
            }

            CallsignParser template = _templates[_currentForceName];

            patternPlatoonBox.Text = template.PatternPlatoon;
            patternSectionBox.Text = template.PatternSection;
            patternTeamBox.Text = template.PatternTeam;

            PopulateTextBoxesFromArray(_companyIDs, template.SymbolsCompanyList);
            PopulateTextBoxesFromArray(_platoonIDs, template.SymbolsPlatoonList);
            PopulateTextBoxesFromArray(_sectionIDs, template.SymbolsSectionList);
            PopulateTextBoxesFromArray(_teamIDs, template.SymbolsTeamList);

            companyIDBoxCO.Text = template.SymbolCompanyCO;
            companyIDBoxXO.Text = template.SymbolCompanyXO;
            platoonIDBoxCO.Text = template.SymbolPlatoonCO;
            platoonIDBoxXO.Text = template.SymbolPlatoonXO;
        }


        private CallsignParser MakeTemplate() {
            string patternPlatoon = patternPlatoonBox.Text.Trim();
            string patternSection = patternSectionBox.Text.Trim();
            string patternTeam = patternTeamBox.Text.Trim();

            List<string> BuildSymbolList(List<TextBox> boxList) {
                List<string> symList = new List<string>();
                foreach (TextBox box in boxList) {
                    string txt = box.Text.Trim();
                    if (txt.Length > 0) {
                        symList.Add(txt);
                    }
                }
                return symList;
            }

            List<string> companyIDs = BuildSymbolList(_companyIDs);
            List<string> platoonIDs = BuildSymbolList(_platoonIDs);
            List<string> sectionIDs = BuildSymbolList(_sectionIDs);
            List<string> teamIDs = BuildSymbolList(_teamIDs);

            string companyCO = companyIDBoxCO.Text.Trim();
            string companyXO = companyIDBoxXO.Text.Trim();
            string platoonCO = platoonIDBoxCO.Text.Trim();
            string platoonXO = platoonIDBoxXO.Text.Trim();

            return new CallsignParser(
                patternPlatoon, patternSection, patternTeam, 
                companyIDs.ToArray(), platoonIDs.ToArray(), 
                sectionIDs.ToArray(), teamIDs.ToArray(), 
                companyCO, companyXO, platoonCO, platoonXO
            );
        }

        private CallsignParser SaveCurrentTemplate() {
            // Save our current template
            CallsignParser template = MakeTemplate();
            _templates[_currentForceName] = template;
            return template;
        }

        private void VerifyTemplate() {
            CallsignParser template = SaveCurrentTemplate();
            Force f = _lastTurn.Forces[_currentForceName];
            f.SetCallsignTemplate(template);
            ViewFuncs.PopulateTreeViewWithOOB(oobView, f);
        }

        private void sideSelector_SelectedIndexChanged(object sender, EventArgs e) {
            SaveCurrentTemplate();

            _currentForceName = (string)sideSelector.SelectedItem;
            PopulateTextBoxes();

            // Load the next template
            Force f = _lastTurn.Forces[_currentForceName];
            f.SetCallsignTemplate(_templates[_currentForceName]);
            ViewFuncs.PopulateTreeViewWithOOB(oobView, f);
        }

        private void btnVerify_Click(object sender, EventArgs e) {
            VerifyTemplate();
        }

        private void btnFinish_Click(object sender, EventArgs e) {
            SaveCurrentTemplate();
            foreach(string forceName in _templates.Keys) {
                _campaign.SetCallsignTemplate(forceName, _templates[forceName]);
            }

            DialogResult = DialogResult.OK;
        }

        private void onBox_Leave(object sender, EventArgs e) {
            VerifyTemplate();
        }
    }
}
