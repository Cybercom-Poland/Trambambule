using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trambambule.Achievements.Helpers;

namespace Trambambule.Achievements
{
    public class GoalsScored : AchievementBase
    {
        public override bool CalculateLevel1(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string comment)
        {
            return CalculateGoalsScored(firstTime, player, achievement, context, 1, 1500, 500, out comment);
        }

        public override bool CalculateLevel2(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string comment)
        {
            return CalculateGoalsScored(firstTime, player, achievement, context, 2, 2000, 500, out comment);
        }

        public override bool CalculateLevel3(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string comment)
        {
            return CalculateGoalsScored(firstTime, player, achievement, context, 3, 2500, 500, out comment);
        }

        private bool CalculateGoalsScored(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, int level, int top, int nextStep, out string comment)
        {
            comment = "";
            int i = 0;
            List<Match> playerMatches = context.Matches.Where(p => p.TeamMatches.Any(x =>
                                                  x.TeamMatchPlayers.Any(z => z.PlayerId == player.Id))).ToList();

            if (playerMatches.Any())
            {
                foreach (Match pm in playerMatches)
                {
                    foreach (TeamMatch tm in pm.TeamMatches)
                    {
                        var tmp = tm.TeamMatchPlayers.FirstOrDefault(w => w.PlayerId == player.Id);
                        if (tmp != null)
                        {
                            i += tmp.TeamMatch.GoalsScored;
                        }
                    }
                }
            }


            if (i == top)
            {
                if (level < 3)
                    comment = "Brakuje " + nextStep + " " + GramaticHelper(nextStep) + " z rzędu do kolejnego poziomu";
                else
                    comment = "Zdobyto najwyższy poziom w tym osiągnięciu";
                
                return true;
            }
            else if (i > top)
            {
                if (level < 3)
                {
                    int t = top + nextStep - i;
                    comment = "Brakuje " + t + " " + GramaticHelper(t) + " do kolejnego poziomu";
                }
                else
                    comment = "Zdobyto najwyższy poziom w tym osiągnięciu";
            }
            else
            {
                int t = top - i;
                comment = "Brakuje " + t + " " + GramaticHelper(t) + " do kolejnego poziomu"; 
                return false;
            }

            if (i >= top)
                return true;
            else
                return false;
        }

        private string GramaticHelper(int count)
        {
            if (count == 1)
                return "strzelonego bramek";
            else
                return "strzelonych bramek";
        }

    }
}