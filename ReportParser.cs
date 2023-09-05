using HtmlAgilityPack;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SBCM {
    internal class ReportParser {
        static string GetStringWithinEndParentheses(string str) {
            string newType = "";
            char[] typeChars = str.ToCharArray();
            int parenBreak = 1;
            for (int i = 1; i < str.Length; i++) {
                char c = typeChars[str.Length - i - 1];
                if (c == '(') {
                    parenBreak--;
                } else if (c == ')') {
                    parenBreak++;
                }

                if (parenBreak == 0) {
                    break;
                } else {
                    newType = c + newType;
                }
            }

            return newType;
        }

        static void ParseGunAmmo(IEnumerable<HtmlNode> row, ref int columnIndex, ref GunAmmo gun, bool initialSetting = false) {
            string gunType = row.ElementAt(columnIndex).InnerHtml;
            if (gunType == "&nbsp;") {
                gun.Type = "";
            } else {
                // Get ammo type from within parentheses
                gun.Type = GetStringWithinEndParentheses(gunType);
            }

            int start = int.Parse(row.ElementAt(columnIndex + 1).InnerHtml);
            int end = int.Parse(row.ElementAt(columnIndex + 2).InnerHtml);

            if (initialSetting) {
                gun.Maximum = start;
                gun.Amount = end;
            } else {
                //gun.Amount -= start - end;
                gun.Amount = end; // We'll trust the report as the final word on ammo level
            }

            columnIndex += 4;
        }

        static HtmlNode NextSiblingElement(HtmlNode n) {
            HtmlNode next = n.NextSibling;
            while (next.NodeType != HtmlNodeType.Element) {
                next = next.NextSibling;
            }

            return next;
        }

        public static bool MergeReport(
            string filename,
            Dictionary<string, Player> scenarioPlayers,
            Dictionary<string, Force> scenarioForces
        ) {
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(filename);

            bool campaignInitializing = scenarioForces.Count == 0;

            // Verify that report is in supported game version
            /* // ENABLE THIS LATER
            HtmlNode meta = htmlDoc.DocumentNode.SelectSingleNode("//meta");
            string gameVersion = meta.GetAttributeValue("content", "undefined");
            Assert(
                gameVersion == "4.379",
                $"Expected version 4.379, got {gameVersion}"
            );
            */

            CallsignParser callsignParser = new CallsignParser(
                "{company}-{platoon}",
                "{company}-{platoon}{section}",
                "{company}-{platoon}{section}/{team}",
                new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M" },
                new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" },
                new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" },
                new string[] { "A", "B", "C", "D" },
                "CO", "XO", "CO", "XO"
            );

            HtmlNodeCollection tables = htmlDoc.DocumentNode.SelectNodes("//table");

            const int OVERALL_PLAYERS_ROW = 5;

            HtmlNode overallTable = tables[0];
            HtmlNode forcesTable = tables[1];

            // Determine number of players
            HtmlNode playersNode = overallTable.SelectNodes("tr")[OVERALL_PLAYERS_ROW];
            var numPlayersNode = playersNode.Elements("td").ElementAt(1);
            int numPlayers = int.Parse(numPlayersNode.InnerHtml);

            // Determine forces in play
            HtmlNodeCollection forcesRows = forcesTable.SelectNodes("tr");
            for (int i = 1; i < forcesRows.Count; i++) {
                HtmlNode forceRowNode = forcesRows[i];
                var forceNameColumn = forceRowNode.Elements("td").ElementAt(0);
                string forceName = forceNameColumn.InnerHtml;

                Force newForce = new Force(forceName);
                newForce.SetCallsignTemplate(callsignParser);
                scenarioForces.Add(forceName, newForce);
            }

            // Parse force & event information
            bool eventsParsed = false;
            HtmlNodeCollection headers = htmlDoc.DocumentNode.SelectNodes("//h2");
            foreach (HtmlNode header in headers) {
                if (header.InnerHtml.Equals("Players")) {
                    // Parse player stats
                    HtmlNode head = header;

                    Regex statRegex = new Regex(@"(\d+) out of (\d+)");
                    Regex lossesRegex = new Regex(@"Total losses - (tanks|PCs|personnel|helicopters|trucks):");

                    for (int i = 0; i < numPlayers; i++) {
                        HtmlNode playerForceNode = NextSiblingElement(head);
                        string playerForceString = playerForceNode.InnerHtml;
                        string playerForce = GetStringWithinEndParentheses(playerForceString);
                        string playerName = playerForceString.Replace(" (" + playerForce + ")", "");

                        Player player;
                        if (scenarioPlayers.ContainsKey(playerName)) {
                            player = scenarioPlayers[playerName];
                        } else {
                            player = new Player(playerName);
                            scenarioPlayers.Add(playerName, player);
                        }

                        HtmlNode playerStatsTable = NextSiblingElement(playerForceNode);
                        var rows = playerStatsTable.Descendants("tr").ToList();
                        foreach (var row in rows) {
                            var columns = row.Descendants("td");
                            string rowHeader = columns.ElementAt(0).InnerHtml;
                            string secondColumn = columns.ElementAt(1).InnerHtml;

                            if (rowHeader == "Hit percentage:") {
                                Match m = statRegex.Match(columns.ElementAt(2).InnerHtml);
                                if (m.Success) {
                                    player.ShotsHit += int.Parse(m.Groups[1].Value);
                                    player.ShotsTaken += int.Parse(m.Groups[2].Value);
                                }
                            } else if (rowHeader == "User kills (Vehicle):") {
                                player.PersonalKills += int.Parse(secondColumn);
                            } else if (rowHeader == "User losses (Vehicle):") {
                                player.PersonalLosses += int.Parse(secondColumn);
                            } else if (rowHeader == "User fratricide (Vehicle):") {
                                player.PersonalFratricides += int.Parse(secondColumn);
                            } else if (rowHeader == "Total kills (Vehicle):") {
                                player.UnitKills_Vehicles += int.Parse(secondColumn);
                            } else {
                                // Unit losses
                                Match m = lossesRegex.Match(secondColumn);
                                if (m.Success) {
                                    string matchItem = m.Groups[1].Value;

                                    int a;//, b;
                                    m = statRegex.Match(columns.ElementAt(2).InnerHtml);
                                    if (m.Success) {
                                        a = int.Parse(m.Groups[1].Value);
                                        //b = int.Parse(m.Groups[2].Value);

                                        switch (matchItem) {
                                            case "tanks":
                                                player.UnitLosses_Tanks += a;
                                                break;

                                            case "PCs":
                                                player.UnitLosses_PCs += a;
                                                break;

                                            case "personnel":
                                                player.UnitLosses_PCs += a;
                                                break;

                                            case "trucks":
                                                player.UnitLosses_Trucks += a;
                                                break;

                                            case "helicopters":
                                                player.UnitLosses_Helicopters += a;
                                                break;

                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                        }

                        head = playerStatsTable;
                    }
                } else if (header.InnerHtml.Equals("Unit Information")) {
                    // Parse force logistics state
                    HtmlNode head = header;

                    for (int i = 0; i < scenarioForces.Count; i++) {
                        // Force name
                        HtmlNode forceNameNode = NextSiblingElement(head);
                        string forceName = forceNameNode.InnerHtml;
                        Force currentForce = scenarioForces[forceName];

                        // Force table
                        HtmlNode forceTable = NextSiblingElement(forceNameNode);
                        HtmlNodeCollection rows = forceTable.SelectNodes("tr");
                        int startIndex;
                        int rowDividers = 0;
                        for (startIndex = 0; startIndex < rows.Count; startIndex++) {
                            HtmlNode row = rows[startIndex];

                            if (row.Elements("td").Count() == 1) {
                                rowDividers++;
                            } else if (rowDividers >= 3) {
                                var unitColumns = row.Descendants("td");

                                string unitCallsign = unitColumns.ElementAt(0).InnerHtml;
                                string unitClass = unitColumns.ElementAt(1).InnerHtml;
                                string unitType = unitColumns.ElementAt(2).InnerHtml;

                                Unit unit = currentForce.GetUnit(unitCallsign);
                                if (unit != null) {
                                    // TODO handle this case better
                                    Debug.Assert(
                                        unit.Unit_Class == unitClass && unit.Type == unitType,
                                        $"Unit {unitCallsign}/{unitType}/{unitClass} does not match {unit.Callsign}/{unit.Type}/{unit.Unit_Class}"
                                    );
                                } else {
                                    unit = new Unit(forceName, unitCallsign, unitType, unitClass);
                                    currentForce.AddUnit(unit);
                                }

                                //Console.WriteLine($"{unit.Callsign}: {unit.Company} company, {unit.Platoon} platoon, {unit.Section} section, {unit.Team} team - CO: {unit.CO} XO: {unit.XO}");

                                if (campaignInitializing) {
                                    unit.Strength_Maximum = int.Parse(unitColumns.ElementAt(3).InnerHtml);
                                }
                                unit.Strength_Current = int.Parse(unitColumns.ElementAt(4).InnerHtml);

                                unit.Kills_Tanks += int.Parse(unitColumns.ElementAt(5).InnerHtml);
                                unit.Kills_PCs += int.Parse(unitColumns.ElementAt(6).InnerHtml);
                                unit.Kills_Helos += int.Parse(unitColumns.ElementAt(7).InnerHtml);
                                unit.Kills_Trucks += int.Parse(unitColumns.ElementAt(8).InnerHtml);
                                unit.Kills_Personnel += int.Parse(unitColumns.ElementAt(9).InnerHtml);
                                unit.Kills_Boats += int.Parse(unitColumns.ElementAt(10).InnerHtml);

                                int gunAmmoIndex = 11;
                                GunAmmo[] gunAmmo = unit.GetAllAmmo();
                                for (int mg = 0; mg < 18; mg++) {
                                    ParseGunAmmo(
                                        unitColumns,
                                        ref gunAmmoIndex,
                                        ref gunAmmo[mg],
                                        campaignInitializing
                                    );
                                }
                            }
                        }

                        head = forceTable;
                    }
                } else if (header.InnerHtml.Equals("Events") && !eventsParsed) {
                    // Parse granular damage events
                    HtmlNode eventTable = NextSiblingElement(header);

                    var rows = eventTable.Descendants("tr").Skip(2).ToList();

                    foreach (HtmlNode row in rows) {
                        var eventColumns = row.Descendants("td");

                        string eventTime = eventColumns.ElementAt(0).InnerHtml;

                        string shooterString = eventColumns.ElementAt(2).InnerHtml;
                        string shooterForce = GetStringWithinEndParentheses(shooterString);
                        string shooterCallsign = shooterString.Replace(" (" + shooterForce + ")", "");

                        int shooterX = int.Parse(eventColumns.ElementAt(3).InnerHtml);
                        int shooterY = int.Parse(eventColumns.ElementAt(4).InnerHtml);

                        string targetString = eventColumns.ElementAt(7).InnerHtml;
                        string targetForce = GetStringWithinEndParentheses(targetString);
                        string targetCallsign = targetString.Replace(" (" + targetForce + ")", "");

                        int targetX = int.Parse(eventColumns.ElementAt(8).InnerHtml);
                        int targetY = int.Parse(eventColumns.ElementAt(9).InnerHtml);

                        Force sForce = scenarioForces[shooterForce];
                        Force tForce = scenarioForces[targetForce];

                        Unit shooter = sForce.GetUnit(shooterCallsign);
                        Unit target = tForce.GetUnit(targetCallsign);

                        // Update units with last-known positions
                        shooter.UTM_X = shooterX;
                        shooter.UTM_Y = shooterY;

                        target.UTM_X = targetX;
                        target.UTM_Y = targetY;

                        // Set last-known damage state of the unit
                        DamageState state = target.GetDamageState();
                        state.Destroyed = state.Destroyed || (eventColumns.ElementAt(12).InnerHtml == "X") || (target.Strength_Current == 0);
                        state.Immobilized = state.Immobilized || eventColumns.ElementAt(13).InnerHtml == "X";
                        state.CasualtyCommander = state.CasualtyCommander || eventColumns.ElementAt(14).InnerHtml == "X";
                        state.CasualtyGunner = state.CasualtyGunner || eventColumns.ElementAt(15).InnerHtml == "X";
                        state.CasualtyLoader = state.CasualtyLoader || eventColumns.ElementAt(16).InnerHtml == "X";
                        state.CasualtyDriver = state.CasualtyDriver || eventColumns.ElementAt(17).InnerHtml == "X";
                        state.DamagedFCS = state.DamagedFCS || eventColumns.ElementAt(18).InnerHtml == "X";
                        state.DamagedRadio = state.DamagedRadio || eventColumns.ElementAt(19).InnerHtml == "X";
                        state.DamagedTurret = state.DamagedTurret || eventColumns.ElementAt(20).InnerHtml == "X";
                    }

                    eventsParsed = true;
                }
            }

            foreach(Force f in scenarioForces.Values) {
                f.EstimatePositions();
            }

            using (StreamWriter writer = new StreamWriter("units.csv")) {
                writer.WriteLine(Unit.SerializeToCSVColumnHeadings());
                foreach (Force f in scenarioForces.Values) {
                    foreach (Unit unit in f.GetUnits()) {
                        writer.WriteLine(unit.SerializeToCSVRow());
                    }
                }
            }

            return true;
        }
    }
}