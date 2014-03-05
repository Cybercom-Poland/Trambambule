using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trambambule.Achievements.Helpers;
using System.Text;

namespace Trambambule.Achievements
{
    public class WinsWithOnePartner : AchievementBase
    {
        public override bool CalculateLevel1(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string comment)
        {
            return CalculateGamesWithOnePartner(firstTime, player, achievement, context, 1, 10, 10, out comment);
        }

        public override bool CalculateLevel2(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string comment)
        {
            return CalculateGamesWithOnePartner(firstTime, player, achievement, context, 2, 20, 10, out comment);
        }

        public override bool CalculateLevel3(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, out string comment)
        {
            return CalculateGamesWithOnePartner(firstTime, player, achievement, context, 3, 30, 10, out comment);
        }

        private bool CalculateGamesWithOnePartner(bool firstTime, Player player, Achievement achievement, TrambambuleDBContextDataContext context, int level, int top, int nextStep, out string comment)
        {
            comment = "";
            List<TeamMatch> playerMatches = context.TeamMatches.Where(p => p.Result == 1 &&
                              p.TeamMatchPlayers.Any(x => x.PlayerId == player.Id)).ToList();

            if (playerMatches.Any())
            {
                TeamMatchPlayer playerData = playerMatches.First().TeamMatchPlayers.First(p => p.PlayerId == player.Id);
                var partnerGames = playerMatches
                    .GroupBy(p => new { Partner = p.TeamMatchPlayers.First(x => x.PlayerId != playerData.PlayerId).Player })
                    .Select(x => new
                    {
                        PartnerId = x.Key.Partner.Id,
                        Games = x.Count(),
                    })
                    .OrderByDescending(p => p.Games).ToList();


                int topGames = -1;
                List<int> topPartners = new List<int>();
                foreach (var m in partnerGames)
                {
                    if (topGames == -1)
                    {
                        topGames = m.Games;
                    }

                    if (topGames > m.Games)
                    {
                        break;
                    }
                    else
                    {
                        topPartners.Add(m.PartnerId);
                    }
                }
                int t = top - topGames;
                if (topPartners.Count > 1)
                {
                    StringBuilder partnersList = new StringBuilder();
                    for (int i = 0; i < topPartners.Count; i++)
                    {
                        if (i != 0)
                            partnersList.Append(", {P_" + topPartners[i] + "}");
                        else
                            partnersList.Append("{P_" + topPartners[i] + "}");
                    }
                    comment = "Brakuje " + t + " " + GramaticHelper(t) + " z jednym z partnerów: " + partnersList + " do kolejnego poziomu";
                }
                else
                {
                    comment = "Brakuje " + t + " " + GramaticHelper(t) + " z: " + "{P_" + topPartners[0] + "}" + " do kolejnego poziomu";
                }


                if (t <= 0)
                {
                    if (level < 3)
                    {
                        if (topPartners.Count > 1)
                        {
                            StringBuilder partnersList = new StringBuilder();
                            for (int i = 0; i < topPartners.Count; i++)
                            {
                                if (i != 0)
                                    partnersList.Append(", {P_" + topPartners[i] + "}");
                                else
                                    partnersList.Append("{P_" + topPartners[i] + "}");
                            }
                            comment = "Brakuje " + (nextStep + t) + " " + GramaticHelper(nextStep) + " z jednym z partnerów: " + partnersList + " do kolejnego poziomu";
                        }
                        else
                        {
                            comment = "Brakuje " + (nextStep + t) + " " + GramaticHelper(nextStep) + " z: " + "{P_" + topPartners[0] + "}" + " do kolejnego poziomu";
                        }
                    }
                    else
                    {
                        comment = "Zdobyto najwyższy poziom w tym osiągnięciu z partnerem {P_" + topPartners[0] + "}";
                    }
                }

                if (topGames >= top)
                    return true;
            }
            else
            {
                comment = "Brakuje " + top + " " + GramaticHelper(top) + " z jakimkolwiek partnerem";
            }
            return false;
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