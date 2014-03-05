using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trambambule.Stats.Helpers;

namespace Trambambule.Stats
{
    public class OverallStats
    {
        public static List<BestStat> CalculateBestStats()
        {
            List<BestStat> bestStats = new List<BestStat>();

            //Wins as a defender
            int currentResult_WinsAsADefender = 0;
            int finalResult_LeastWinsAsADefender = 999999;
            int finalResult_MostWinsAsADefender = 0;
            Player finalPlayer_LeastWinsAsADefender = null;
            Player finalPlayer_MostWinsAsADefender = null;

            //Wins as an attacker
            int currentResult_WinsAsAnAttacker = 0;
            int finalResult_LeastWinsAsAnAttacker = 999999;
            int finalResult_MostWinsAsAnAttacker = 0;
            Player finalPlayer_LeastWinsAsAnAttacker = null;
            Player finalPlayer_MostWinsAsAnAttacker = null;

            //Overall wins
            int currentResult_Wins = 0;
            int finalResult_LeastWins = 999999;
            int finalResult_MostWins = 0;
            Player finalPlayer_LeastWins = null;
            Player finalPlayer_MostWins = null;

            //Goals Balance
            int currentResult_GoalsBalance = 0;
            int finalResult_WorstGoalsBalance = 999999;
            int finalResult_BestGoalsBalance = 0;
            Player finalPlayer_WorstGoalsBalance = null;
            Player finalPlayer_BestGoalsBalance = null;

            //Percent of wins
            int finalResult_LowestPercentOfWins = 999999;
            int finalResult_TopPercentOfWins = 0;
            Player finalPlayer_LowestPercentOfWins = null;
            Player finalPlayer_TopPercentOfWins = null;

            //Total games played
            int currentResult_GamesPlayed = 0;
            int finalResult_LeastGamesPlayed = 999999;
            int finalResult_MostGamesPlayed = 0;
            Player finalPlayer_LeastGamesPlayed = null;
            Player finalPlayer_MostGamesPlayed = null;

            var players = DataAccess.GetPlayers();
            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                try
                {
                    foreach (Player player in players)
                    {
                        currentResult_WinsAsADefender = 0;
                        currentResult_WinsAsAnAttacker = 0;
                        currentResult_Wins = 0;
                        currentResult_GoalsBalance = 0;
                        currentResult_GamesPlayed = 0;

                        List<Match> playerMatches = context.Matches.Where(p => p.TeamMatches.Any(x =>
                                                    x.TeamMatchPlayers.Any(z => z.PlayerId == player.Id))).ToList();

                        if (playerMatches != null && playerMatches.Count > 0)
                        {
                            foreach (Match pm in playerMatches)
                            {
                                foreach (TeamMatch tm in pm.TeamMatches)
                                {
                                    var tmp = tm.TeamMatchPlayers.FirstOrDefault(w => w.PlayerId == player.Id);
                                    if (tmp != null)
                                    {
                                        if (tmp.TeamMatch.Result == (int)Common.EResult.Win && tmp.Position == (int)Common.EPosition.Defence)
                                        {
                                            //Wins as a defender
                                            currentResult_WinsAsADefender++;
                                        }

                                        if (tmp.TeamMatch.Result == (int)Common.EResult.Win && tmp.Position == (int)Common.EPosition.Offence)
                                        {
                                            //Wins as an attacker
                                            currentResult_WinsAsAnAttacker++;
                                        }

                                        if (tmp.TeamMatch.Result == (int)Common.EResult.Win)
                                        {
                                            //Overall wins
                                            currentResult_Wins++;
                                        }

                                        //Goals Balance
                                        currentResult_GoalsBalance += tmp.TeamMatch.GoalsScored;
                                        currentResult_GoalsBalance -= tmp.TeamMatch.GoalsLost;
                                    }
                                }
                            }

                            if (currentResult_WinsAsADefender < finalResult_LeastWinsAsADefender)
                            {
                                //LeastWinsAsADefender
                                finalResult_LeastWinsAsADefender = currentResult_WinsAsADefender;
                                finalPlayer_LeastWinsAsADefender = player;
                            }
                            if (currentResult_WinsAsADefender > finalResult_MostWinsAsADefender)
                            {
                                //MostWinsAsADefender
                                finalResult_MostWinsAsADefender = currentResult_WinsAsADefender;
                                finalPlayer_MostWinsAsADefender = player;
                            }


                            if (currentResult_WinsAsAnAttacker < finalResult_LeastWinsAsAnAttacker)
                            {
                                //LeastWinsAsAnAttacker
                                finalResult_LeastWinsAsAnAttacker = currentResult_WinsAsAnAttacker;
                                finalPlayer_LeastWinsAsAnAttacker = player;
                            }
                            if (currentResult_WinsAsAnAttacker > finalResult_MostWinsAsAnAttacker)
                            {
                                //MostWinsAsAnAttacker
                                finalResult_MostWinsAsAnAttacker = currentResult_WinsAsAnAttacker;
                                finalPlayer_MostWinsAsAnAttacker = player;
                            }


                            if (currentResult_Wins < finalResult_LeastWins)
                            {
                                //LeasttWins
                                finalResult_LeastWins = currentResult_Wins;
                                finalPlayer_LeastWins = player;
                            }
                            if (currentResult_Wins > finalResult_MostWins)
                            {
                                //MostWins
                                finalResult_MostWins = currentResult_Wins;
                                finalPlayer_MostWins = player;
                            }


                            if (currentResult_GoalsBalance < finalResult_WorstGoalsBalance)
                            {
                                //WorstGoalsBalance
                                finalResult_WorstGoalsBalance = currentResult_GoalsBalance;
                                finalPlayer_WorstGoalsBalance = player;
                            }
                            if (currentResult_GoalsBalance > finalResult_BestGoalsBalance)
                            {
                                //BestGoalsBalance
                                finalResult_BestGoalsBalance = currentResult_GoalsBalance;
                                finalPlayer_BestGoalsBalance = player;
                            }


                            int current_PercentOfWins = (int)Math.Round((float)((float)currentResult_Wins / (float)playerMatches.Count) * 100, 0);
                            if (current_PercentOfWins < finalResult_LowestPercentOfWins)
                            {
                                //LowestPercentOfWins
                                finalResult_LowestPercentOfWins = current_PercentOfWins;
                                finalPlayer_LowestPercentOfWins = player;
                            }
                            if (current_PercentOfWins > finalResult_TopPercentOfWins)
                            {
                                //TopPercentOfWins
                                finalResult_TopPercentOfWins = current_PercentOfWins;
                                finalPlayer_TopPercentOfWins = player;
                            }

                            currentResult_GamesPlayed = playerMatches.Count;
                            if (currentResult_GamesPlayed < finalResult_LeastGamesPlayed)
                            {
                                //LeastGamesPlayed
                                finalResult_LeastGamesPlayed = currentResult_GamesPlayed;
                                finalPlayer_LeastGamesPlayed = player;
                            }
                            if (currentResult_GamesPlayed > finalResult_MostGamesPlayed)
                            {
                                //MostGamesPlayed
                                finalResult_MostGamesPlayed = currentResult_GamesPlayed;
                                finalPlayer_MostGamesPlayed = player;
                            }
                        }
                    }


                    bestStats.Add(new BestStat(finalPlayer_MostWins, finalResult_MostWins, "Najwięcej zwycięstw"));
                    bestStats.Add(new BestStat(finalPlayer_LeastWins, finalResult_LeastWins, "Najmniej zwycięstw"));

                    bestStats.Add(new BestStat(finalPlayer_MostWinsAsAnAttacker, finalResult_MostWinsAsAnAttacker, "Najwięcej zwycięstw grając w ataku"));
                    bestStats.Add(new BestStat(finalPlayer_LeastWinsAsAnAttacker, finalResult_LeastWinsAsAnAttacker, "Najmniej zwycięstw grając w ataku"));

                    bestStats.Add(new BestStat(finalPlayer_MostWinsAsADefender, finalResult_MostWinsAsADefender, "Najwięcej zwycięstw grając w obronie"));
                    bestStats.Add(new BestStat(finalPlayer_LeastWinsAsADefender, finalResult_LeastWinsAsADefender, "Najmniej zwycięstw grając w obronie"));

                    bestStats.Add(new BestStat(finalPlayer_MostGamesPlayed, finalResult_MostGamesPlayed, "Najwięcej rozegranych meczów"));
                    bestStats.Add(new BestStat(finalPlayer_LeastGamesPlayed, finalResult_LeastGamesPlayed, "Najmniej rozegranych meczów"));

                    bestStats.Add(new BestStat(finalPlayer_BestGoalsBalance, finalResult_BestGoalsBalance, "Najlepszy bilans bramek"));
                    bestStats.Add(new BestStat(finalPlayer_WorstGoalsBalance, finalResult_WorstGoalsBalance, "Najgorszy bilans bramek"));

                    bestStats.Add(new BestStat(finalPlayer_TopPercentOfWins, finalResult_TopPercentOfWins, "Największy procent wygranych"));
                    bestStats.Add(new BestStat(finalPlayer_LowestPercentOfWins, finalResult_LowestPercentOfWins, "Najmniejszy procent wygranych"));

                }
                catch { }
            }


            return bestStats;
        }

        public static int finalResult_LeastWinsAsAnAttacker { get; set; }

        public static Player finalPlayer_LeastWinsAsAnAttacker { get; set; }

        public static int finalResult_LeastWinsAsADefender { get; set; }

        public static Player finalPlayer_LeastWinsAsADefender { get; set; }

        public static int finalResult_LeastWins { get; set; }

        public static Player finalPlayer_LeastWins { get; set; }
    }
}
