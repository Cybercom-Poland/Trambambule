using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trambambule.Achievements.Helpers;

namespace Trambambule.Achievements
{
    public class PlayedMatchesCount : AchievementBase
    {
        public override bool CalculateLevel1(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string comment)
        {
            return CalculatePlayedMatchesCount(firstTime, player, achievement, context, 1, 100, 100, out comment);
        }

        public override bool CalculateLevel2(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string comment)
        {
            return CalculatePlayedMatchesCount(firstTime, player, achievement, context, 2, 200, 100, out comment);
        }

        public override bool CalculateLevel3(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string comment)
        {
            return CalculatePlayedMatchesCount(firstTime, player, achievement, context, 3, 300, 100, out comment);
        }

        private bool CalculatePlayedMatchesCount(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, int level, int top, int nextStep, out string comment)
        {
            comment = "";
            int i = 0;
            List<Match> playerMatches = context.Matches.Where(p => p.TeamMatches.Any(x =>
                                                  x.TeamMatchPlayers.Any(z => z.PlayerId == player.Id))).ToList();

            if (playerMatches.Any())
            {
                i = playerMatches.Count;

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
            else
            {
                comment = "Brakuje " + top + " " + GramaticHelper(top) + " do kolejnego poziomu";
                return false;
            }
        }

        private string GramaticHelper(int count)
        {
            if (count == 1)
                return "rozegranego meczu";
            else
                return "rozegranych meczy";
        }
    }
}