using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trambambule.Achievements.Helpers;

namespace Trambambule.Achievements
{
    public class WinsInRow : AchievementBase
    {
        public override bool CalculateLevel1(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string comment)
        {
            return CalculateWinsInRow(firstTime, player, achievement, context, 1, 5, 5, out comment);
        }

        public override bool CalculateLevel2(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string comment)
        {
            return CalculateWinsInRow(firstTime, player, achievement, context, 2, 10, 5, out comment);
        }

        public override bool CalculateLevel3(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string comment)
        {
            return CalculateWinsInRow(firstTime, player, achievement, context, 3, 15, 5, out comment);
        }

        private bool CalculateWinsInRow(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, int level, int top, int nextStep, out string comment)
        {
            comment = "";
            int i = 0;
            bool result = true;

            if (!firstTime)
            {
                List<Match> playerMatches = context.Matches.Where(p => p.TeamMatches.Any(x =>
                                            x.TeamMatchPlayers.Any(z => z.PlayerId == player.Id)))
                                            .OrderByDescending(p => p.Timestamp).Take(top).OrderByDescending(w => w.Timestamp).ToList();

                //if (playerMatches.Count == top)
                //{
                    List<TeamMatch> tmList = new List<TeamMatch>();
                    foreach (Match pm in playerMatches)
                    {
                        foreach (TeamMatch tm in pm.TeamMatches)
                        {
                            var tmp = tm.TeamMatchPlayers.FirstOrDefault(w => w.PlayerId == player.Id);
                            if (tmp != null)
                            {
                                if (tmp.TeamMatch.Result == 0)
                                {
                                    result = false;
                                    int t = (top - i);
                                    comment = "Brakuje " + t + " " + GramaticHelper(t) + " z rzędu do kolejnego poziomu";
                                    break;
                                }
                                else
                                {
                                    i++;
                                }
                            }
                        }
                        if (!result)
                            break;
                    }
                //}
                //else
                //{
                //    int t = top - playerMatches.Count;
                //    comment = "Brakuje " + t + " " + GramaticHelper(t) + " z rzędu do kolejnego poziomu";
                //    return false;
                //}


                if (comment == "")
                {
                    if (level < 3)
                    {
                        if (i > 0)
                            comment = "Brakuje " + (nextStep - i) + " " + GramaticHelper(nextStep - i) + " z rzędu do kolejnego poziomu";
                        else
                            comment = "Brakuje " + nextStep + " " + GramaticHelper(nextStep) + " z rzędu do kolejnego poziomu";
                    }
                    else
                        comment = "Zdobyto najwyższy poziom w tym osiągnięciu";
                }

                if (top == i && result)
                    return true;
                else
                    return false;
            }
            else
            {
                int j = 0;
                int winsAtTheBeginning = 0;
                bool lost = false;
                List<Match> playerMatches = context.Matches.Where(p => p.TeamMatches.Any(x =>
                                         x.TeamMatchPlayers.Any(z => z.PlayerId == player.Id)))
                                          .OrderByDescending(p => p.Timestamp).ToList();

                //if (playerMatches.Count >= top)
                //{
                    List<TeamMatch> tmList = new List<TeamMatch>();
                    foreach (Match pm in playerMatches)
                    {
                        for (int counter = 0; counter < pm.TeamMatches.Count; counter++)
                        {
                            TeamMatch tm = pm.TeamMatches[counter];
                            var tmp = tm.TeamMatchPlayers.FirstOrDefault(w => w.PlayerId == player.Id);
                            if (tmp != null)
                            {
                                if (tmp.TeamMatch.Result == 1)
                                {
                                    i++;
                                    if (!lost)
                                    {
                                        winsAtTheBeginning++;
                                    }
                                    if (i >= top)
                                    {
                                        j = i;
                                    }
                                }
                                else
                                {
                                    i = 0;
                                    if (!lost)
                                        lost = true;
                                }
                            }
                        }
                    }


                    if (comment == "")
                    {
                        if (level < 3)
                        {
                            if (winsAtTheBeginning > top || j > 0)
                            {
                                int t = top + nextStep - winsAtTheBeginning;
                                comment = "Brakuje " + t + " " + GramaticHelper(t) + " z rzędu do kolejnego poziomu";
                            }
                            else if (j == 0 )
                            {
                                if (winsAtTheBeginning > 0)
                                {
                                    int t = top - winsAtTheBeginning;
                                    comment = "Brakuje " + t + " " + GramaticHelper(t) + " z rzędu do kolejnego poziomu";
                                }
                                else if (winsAtTheBeginning == 0)
                                {
                                    comment = "Brakuje " + top + " " + GramaticHelper(top) + " z rzędu do kolejnego poziomu";
                                }
                            }
                        }
                        else
                            comment = "Zdobyto najwyższy poziom w tym osiągnięciu";
                    }
                    if (top <= j)
                        return true;
                    else
                        return false;
                //}
                //else
                //{
                //    int t = top - playerMatches.Count;
                //    comment = "Brakuje " + t + " " + GramaticHelper(t) + " z rzędu do kolejnego poziomu";
                //    return false;
                //}
            }
        }

        private string GramaticHelper(int count)
        {
            if (count == 1)
                return "zwycięstwa";
            else
                return "zwycięstw";
        }
    }
}