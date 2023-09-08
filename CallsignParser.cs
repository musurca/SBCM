using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms.VisualStyles;

namespace SBCM {
    public class CallsignParser {
        static readonly string MARKER_COMPANY   = "{company}";
        static readonly string MARKER_PLATOON   = "{platoon}";
        static readonly string MARKER_SECTION   = "{section}";
        static readonly string MARKER_TEAM      = "{team}";

        public string CompanyRegexStr;
        public string PlatoonRegexStr;
        public string SectionRegexStr;
        public string TeamRegexStr;

        public Regex RegexPlatoon;
        public List<string> RegexPlatoonSymbolOrder;

        public Regex RegexSection;
        public List<string> RegexSectionSymbolOrder;

        public Regex RegexTeam;
        public List<string> RegexTeamSymbolOrder;

        public string SymbolCompanyCO;
        public string SymbolCompanyXO;
        public string SymbolPlatoonCO;
        public string SymbolPlatoonXO;

        public string PatternPlatoon;
        public string PatternSection;
        public string PatternTeam;

        public string[] SymbolsCompanyList;
        public string[] SymbolsPlatoonList;
        public string[] SymbolsSectionList;
        public string[] SymbolsTeamList;

        private string _longestPossibleCallsign = "";
        public string LongestPossibleCallsign {
            get {
                if(_longestPossibleCallsign.Length == 0) {
                    CalculateLongestPossibleCallsign();
                }
                return _longestPossibleCallsign;
            }
        }

        public CallsignParser() {

        }

        public CallsignParser(
            string pattern_platoon,
            string pattern_section,
            string pattern_team,
            string[] symbols_company,
            string[] symbols_platoon,
            string[] symbols_section,
            string[] symbols_team,
            string symbol_company_co,
            string symbol_company_xo,
            string symbol_platoon_co,
            string symbol_platoon_xo
        ) {
            PatternPlatoon = pattern_platoon;
            PatternSection = pattern_section;
            PatternTeam = pattern_team;

            SymbolsCompanyList = symbols_company;
            SymbolsPlatoonList = symbols_platoon;
            SymbolsSectionList = symbols_section;
            SymbolsTeamList = symbols_team;

            CompanyRegexStr = BuildRegexFromSymbolList(symbols_company);
            PlatoonRegexStr = BuildRegexFromSymbolList(symbols_platoon, symbol_company_co, symbol_company_xo);
            SectionRegexStr = BuildRegexFromSymbolList(symbols_section, symbol_platoon_co, symbol_platoon_xo);
            TeamRegexStr = BuildRegexFromSymbolList(symbols_team);

            RegexPlatoon = new Regex(SwapSymbols(pattern_platoon, out RegexPlatoonSymbolOrder));
            RegexSection = new Regex(SwapSymbols(pattern_section, out RegexSectionSymbolOrder));
            RegexTeam = new Regex(SwapSymbols(pattern_team, out RegexTeamSymbolOrder));

            SymbolCompanyCO = symbol_company_co;
            SymbolCompanyXO = symbol_company_xo;
            SymbolPlatoonCO = symbol_platoon_co;
            SymbolPlatoonXO = symbol_platoon_xo;
        }

        private void CalculateLongestPossibleCallsign() {
            string LongestStringInList(string[] strs) {
                string ret = "";
                foreach (string str in strs) {
                    if (str.Length > ret.Length) {
                        ret = str;
                    }
                }
                return ret;
            }

            string longest_pattern = PatternPlatoon.Length > PatternSection.Length
                ? (PatternPlatoon.Length > PatternTeam.Length ? PatternPlatoon : PatternTeam)
                : (PatternSection.Length > PatternTeam.Length ? PatternSection : PatternTeam);

            string longest_company = LongestStringInList(SymbolsCompanyList);
            string longest_platoon = LongestStringInList(SymbolsPlatoonList);
            string longest_section = LongestStringInList(SymbolsSectionList);
            string longest_team = LongestStringInList(SymbolsTeamList);

            _longestPossibleCallsign = longest_pattern.Replace(MARKER_COMPANY, longest_company)
                .Replace(MARKER_PLATOON, longest_platoon)
                .Replace(MARKER_SECTION, longest_section)
                .Replace(MARKER_TEAM, longest_team);
        }

        public bool BuildCallsign(
            out string callsign,
            string company,
            string platoon,
            string section = "",
            string team = ""
        ) {
            callsign = "";
            bool teamValid = true;
            bool sectionValid = true;
            if (team != "") {
                callsign = PatternTeam;
                callsign = callsign.Replace(MARKER_TEAM, team);
                teamValid = SymbolsTeamList.Contains(team);
            }

            if (section != "") {
                if (callsign == "") {
                    callsign = PatternSection;
                }

                callsign = callsign.Replace(MARKER_SECTION, section);
                sectionValid = SymbolsSectionList.Contains(section);
            } else {
                if (callsign == "") {
                    callsign = PatternPlatoon;
                }
            }

            callsign = callsign.Replace(MARKER_PLATOON, platoon)
                .Replace(MARKER_COMPANY, company);

            return SymbolsPlatoonList.Contains(platoon)
                && SymbolsCompanyList.Contains(company)
                && sectionValid && teamValid;
        }

        private void ExtractCompanyPlatoon(
            string callsign,
            Regex regex,
            List<string> symbol_order,
            ref string company,
            ref string platoon,
            ref string section,
            ref string team
        ) {
            company = "";
            platoon = "";
            section = "";
            team = "";

            Match match = regex.Match(callsign);
            if (match.Success) {
                for (int i = 1; i < match.Groups.Count; i++) {
                    Group g = match.Groups[i];
                    if (i - 1 >= symbol_order.Count) {
                        break;
                    }
                    string symbol = symbol_order[i - 1];

                    if (symbol == MARKER_COMPANY) {
                        company = g.Value;
                    } else if (symbol == MARKER_PLATOON) {
                        platoon = g.Value;
                    } else if (symbol == MARKER_SECTION) {
                        section = g.Value;
                    } else if (symbol == MARKER_TEAM) {
                        team = g.Value;
                    }
                }
            }
        }

        public void GetBattalionPosition(
            string callsign,
            out string company,
            out string platoon,
            out string section,
            out string team,
            out CommandState command
        ) {
            command = CommandState.NONE;

            team = "";
            section = "";
            platoon = "";
            company = "";

            // Is it a team?
            ExtractCompanyPlatoon(
                callsign,
                RegexTeam,
                RegexTeamSymbolOrder,
                ref company, ref platoon,
                ref section, ref team
            );
            if(team != "") {
                return;
            }

            // Is it a section or platoon CO/XO?
            ExtractCompanyPlatoon(
                callsign,
                RegexSection,
                RegexSectionSymbolOrder,
                ref company, ref platoon,
                ref section, ref team
            );
            if(section != "") { 
                if (section == SymbolPlatoonCO) {
                    command = CommandState.CO;
                    section = "";
                } else if (section == SymbolPlatoonXO) {
                    command = CommandState.XO;
                    section = "";
                }

                return;
            }

            // Is it a platoon or company CO/XO?
            ExtractCompanyPlatoon(
                callsign,
                RegexPlatoon,
                RegexPlatoonSymbolOrder,
                ref company, ref platoon,
                ref section, ref team
            );
            if(platoon != "") { 
                if (platoon == SymbolCompanyCO) {
                    command = CommandState.CO;
                    platoon = "";
                } else if (platoon == SymbolCompanyXO) {
                    command = CommandState.XO;
                    platoon = "";
                }

                return;
            }
        }

        public static string BuildRegexFromSymbolList(string[] symbols, string addl_symbol_co = "", string addl_symbol_xo = "") {
            // Constructs a group of matching symbols in valid regex
            string regex = "";
            for (int i = 0; i < symbols.Length; i++) {
                string symbol = symbols[i];
                regex += symbol;
                if (i != symbols.Length - 1) {
                    regex += "|";
                }
            }
            if (addl_symbol_co != "") {
                regex += "|" + addl_symbol_co + "|" + addl_symbol_xo;
            }
            return "(" + regex + ")";
        }

        public string SwapSymbols(string input, out List<string> order) {
            // Clean input by adding escape to reserved regex characters
            string output = input;
            Regex regex = new Regex(@"(\/|\.|\+|\*|\?|\^|\(|\)|\[|\]|\||\\)");
            Match regex_match = regex.Match(input);
            if (regex_match.Success) {
                List<string> matchedStrs = new List<string>();
                for (int i = 1; i < regex_match.Groups.Count; i++) {
                    Group g = regex_match.Groups[i];
                    if (!matchedStrs.Contains(g.Value)) {
                        matchedStrs.Add(g.Value);
                    }
                }
                foreach (string matchStr in matchedStrs) {
                    output = output.Replace(matchStr, @"\" + matchStr);
                }
            }

            // Determine order of symbol matches
            int index_company = output.IndexOf(MARKER_COMPANY);
            int index_platoon = output.IndexOf(MARKER_PLATOON);
            int index_section = output.IndexOf(MARKER_SECTION);
            int index_team = output.IndexOf(MARKER_TEAM);

            SortedDictionary<int, string> symbolOrder = new SortedDictionary<int, string>();
            if (index_company != -1) {
                symbolOrder.Add(index_company, MARKER_COMPANY);
            }
            if (index_platoon != -1) {
                symbolOrder.Add(index_platoon, MARKER_PLATOON);
            }
            if (index_section != -1) {
                symbolOrder.Add(index_section, MARKER_SECTION);
            }
            if (index_team != -1) {
                symbolOrder.Add(index_team, MARKER_TEAM);
            }
            order = symbolOrder.Values.ToList();

            // Build the final regex
            return output
                .Replace(MARKER_COMPANY, CompanyRegexStr)
                .Replace(MARKER_PLATOON, PlatoonRegexStr)
                .Replace(MARKER_SECTION, SectionRegexStr)
                .Replace(MARKER_TEAM, TeamRegexStr)
            ;
        }
    }
}