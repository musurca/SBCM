using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SBCM {
    public class CallsignParser {
        static readonly string MARKER_COMPANY   = "{company}";
        static readonly string MARKER_PLATOON   = "{platoon}";
        static readonly string MARKER_SECTION   = "{section}";
        static readonly string MARKER_TEAM      = "{team}";

        readonly string _company_regex_str;
        readonly string _platoon_regex_str;
        readonly string _section_regex_str;
        readonly string _team_regex_str;

        readonly Regex _regex_platoon;
        readonly List<string> _regex_platoon_symbol_order;

        readonly Regex _regex_section;
        readonly List<string> _regex_section_symbol_order;

        readonly Regex _regex_team;
        readonly List<string> _regex_team_symbol_order;

        readonly string _symbol_company_co;
        readonly string _symbol_company_xo;
        readonly string _symbol_platoon_co;
        readonly string _symbol_platoon_xo;

        readonly string _pattern_platoon;
        readonly string _pattern_section;
        readonly string _pattern_team;

        readonly string[] _symbols_company;
        readonly string[] _symbols_platoon;
        readonly string[] _symbols_section;
        readonly string[] _symbols_team;

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
            _pattern_platoon = pattern_platoon;
            _pattern_section = pattern_section;
            _pattern_team = pattern_team;

            _symbols_company = symbols_company;
            _symbols_platoon = symbols_platoon;
            _symbols_section = symbols_section;
            _symbols_team = symbols_team;

            _company_regex_str = BuildRegexFromSymbolList(symbols_company);
            _platoon_regex_str = BuildRegexFromSymbolList(symbols_platoon, symbol_company_co, symbol_company_xo);
            _section_regex_str = BuildRegexFromSymbolList(symbols_section, symbol_platoon_co, symbol_platoon_xo);
            _team_regex_str = BuildRegexFromSymbolList(symbols_team);

            _regex_platoon = new Regex(SwapSymbols(pattern_platoon, out _regex_platoon_symbol_order));
            _regex_section = new Regex(SwapSymbols(pattern_section, out _regex_section_symbol_order));
            _regex_team = new Regex(SwapSymbols(pattern_team, out _regex_team_symbol_order));

            _symbol_company_co = symbol_company_co;
            _symbol_company_xo = symbol_company_xo;
            _symbol_platoon_co = symbol_platoon_co;
            _symbol_platoon_xo = symbol_platoon_xo;
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
                callsign = _pattern_team;
                callsign = callsign.Replace(MARKER_TEAM, team);
                teamValid = _symbols_team.Contains(team);
            }

            if (section != "") {
                if (callsign == "") {
                    callsign = _pattern_section;
                }

                callsign = callsign.Replace(MARKER_SECTION, section);
                sectionValid = _symbols_section.Contains(section);
            } else {
                if (callsign == "") {
                    callsign = _pattern_platoon;
                }
            }

            callsign = callsign.Replace(MARKER_PLATOON, platoon)
                .Replace(MARKER_COMPANY, company);

            return _symbols_platoon.Contains(platoon)
                && _symbols_company.Contains(company)
                && sectionValid && teamValid;
        }

        private bool ExtractCompanyPlatoon(
            string callsign,
            Regex regex,
            List<string> symbol_order,
            out string company,
            out string platoon,
            out string section,
            out string team
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

                if (company != "" && platoon != "") {
                    return true;
                }
            }
            return false;
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

            // Is it a team?
            if (
                ExtractCompanyPlatoon(
                    callsign,
                    _regex_team,
                    _regex_team_symbol_order,
                    out company, out platoon,
                    out section, out team
                )
            ) {
                return;
            }

            // Is it a section or platoon CO/XO?
            if (
                ExtractCompanyPlatoon(
                    callsign,
                    _regex_section,
                    _regex_section_symbol_order,
                    out company, out platoon,
                    out section, out team
                )
            ) {
                if (section == _symbol_platoon_co) {
                    command = CommandState.CO;
                    section = "";
                } else if (section == _symbol_platoon_xo) {
                    command = CommandState.XO;
                    section = "";
                }

                return;
            }

            // Is it a platoon or company CO/XO?
            if (
                ExtractCompanyPlatoon(
                    callsign,
                    _regex_platoon,
                    _regex_platoon_symbol_order,
                    out company, out platoon,
                    out section, out team
                )
            ) {
                if (platoon == _symbol_company_co) {
                    command = CommandState.CO;
                    platoon = "";
                } else if (platoon == _symbol_company_xo) {
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
                .Replace(MARKER_COMPANY, _company_regex_str)
                .Replace(MARKER_PLATOON, _platoon_regex_str)
                .Replace(MARKER_SECTION, _section_regex_str)
                .Replace(MARKER_TEAM, _team_regex_str)
            ;
        }
    }
}