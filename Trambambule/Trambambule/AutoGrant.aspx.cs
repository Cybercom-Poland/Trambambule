using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Trambambule.Achievements;
using Trambambule.Achievements.Helpers;

namespace Trambambule
{
    public partial class AutoGrant : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                bool achievementsExists = true;
                using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
                {
                    achievementsExists = context.AchievementPlayer.Any();
                }
                if (!achievementsExists)
                {
                    List<AchievementTextWithIds> achievementsData = new AchievementsService(DataAccess.GetPlayers().ToList()).RecalculateAchievements(true);

                    litStatus.Text = "Osiągnięcia zostały przyznane";

                    //List<AchievementsWithPlayers> awpList = new List<AchievementsWithPlayers>();
                    //using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
                    //{
                    //    var achievements = context.Achievements.ToList();
                    //    foreach (Achievement achievement in achievements)
                    //    {
                    //        var players = (from ap in context.AchievementPlayer
                    //                       join p in context.Players
                    //                       on ap.PlayerId equals p.Id
                    //                       where ap.AchievementId == achievement.Id
                    //                       select new PlayerWithLevelOfAchievement(p, ap.LevelOfAchievement, ap.ToNextLevelLabel)).ToList();

                    //        AchievementsWithPlayers awp = new AchievementsWithPlayers(achievement, players);
                    //        awpList.Add(awp);
                    //    }
                    //}
                    
                    Cache.Remove(DataAccess.PlayersListCacheName);
                    Cache.Remove(DataAccess.PlayersNamesAndSurnamesWithoutPolishCharsCacheName);
                    Cache.Remove(DataAccess.PlayersAchievementsCacheName);
                    Cache.Remove(DataAccess.AllAchievementsCacheName);
                    Cache.Remove(DataAccess.OverallStatsCacheName);
                }
            }
            catch (Exception ex)
            {
                litStatus.Text = ex.Message + " " + ex.StackTrace;
            }
        }
    }
}