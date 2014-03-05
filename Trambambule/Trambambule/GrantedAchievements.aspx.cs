using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Trambambule.Achievements.Helpers;
using System.Text.RegularExpressions;

namespace Trambambule
{
    public partial class GrantedAchievements : System.Web.UI.Page
    {
        private int selectedUserId = -1;
        private int selectedAchievementInAccordion = -1;
        private Achievement currentAchievement = null;        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["TrambambuleAchievementsAccordion"] != null && Request.Cookies["TrambambuleAchievementsAccordion"].Value != "")
            {
                int.TryParse(Request.Cookies["TrambambuleAchievementsAccordion"].Value, out selectedAchievementInAccordion);
            }            
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session["UserBasicStatsPlayer"] == null)
            {
                if (Request.QueryString["userId"] != null && Request.QueryString["userId"] != "")
                {
                    int.TryParse(Request.QueryString["userId"], out selectedUserId);
                }
            }
            else
            {
                selectedUserId = ((Player)Session["UserBasicStatsPlayer"]).Id;
            }

            List<AchievementsWithPlayers> awpList = new List<AchievementsWithPlayers>();
            if (Cache[DataAccess.AllAchievementsCacheName] == null)
            {
                using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
                {
                    var achievements = context.Achievements.ToList();
                    foreach (Achievement achievement in achievements)
                    {
                        var players = (from ap in context.AchievementPlayer
                                       join p in context.Players
                                       on ap.PlayerId equals p.Id
                                       where ap.AchievementId == achievement.Id
                                       && ap.LevelOfAchievement > 0
                                       select new PlayerWithLevelOfAchievement(p, ap.LevelOfAchievement, ap.ToNextLevelLabel)).ToList();

                        AchievementsWithPlayers awp = new AchievementsWithPlayers(achievement, players);
                        awpList.Add(awp);
                    }
                }

                Cache.Insert(DataAccess.AllAchievementsCacheName, awpList, null, DateTime.Now.AddSeconds(DataAccess.CacheTimeInSeconds), TimeSpan.Zero);
            }
            else
            {
                awpList = (List<AchievementsWithPlayers>)HttpContext.Current.Cache[DataAccess.AllAchievementsCacheName];
            }

            if (awpList.Count > 0)
            {
                repAchievements.DataSource = awpList;
                repAchievements.DataBind();
            }
        }

        protected string GetHiddenAttribute(int id)
        {
            if (selectedAchievementInAccordion > 0 && id == selectedAchievementInAccordion)            
                return "";            
            else
                return "style=\"display:none;\"";
        }
     
        protected string GetTextWithAnchors(string text)
        {
            var matches = Regex.Matches(text, @"{P_\d*}");
            foreach (System.Text.RegularExpressions.Match m in matches)
            {
                int id = int.Parse(m.Value.Replace("{P_","").Replace("}", ""));
                text = text.Replace(m.Value, PlayerHelper.GetPlayerNameLink(DataAccess.GetPlayer(id)));
            }
            return text;
        }

        protected string GetCustomClass(int id)
        {
            if (selectedUserId == id)
                return " bold";
            else
                return "";
        }

        protected string GetIcon(int currentLevel)
        {
            switch (currentLevel)
            {
                case 1:
                    return currentAchievement.Level1Icon;
                case 2:
                    return currentAchievement.Level2Icon;
                case 3:
                    return currentAchievement.Level3Icon;
            }

            return "";
        }

        protected string GetLabel(int currentLevel)
        {
            switch (currentLevel)
            {
                case 1:
                    return currentAchievement.Level1Label;
                case 2:
                    return currentAchievement.Level2Label;
                case 3:
                    return currentAchievement.Level3Label;
            }

            return "";
        }

        protected void repAchievements_ItemBound(object sender, RepeaterItemEventArgs args)
        {
            var item = args.Item;
            if (item != null)
            {
                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                {
                    AchievementsWithPlayers awp = (AchievementsWithPlayers)item.DataItem;
                    if (awp.Players != null && awp.Players.Count > 0)
                    {
                        currentAchievement = awp.Achievement;
                        PlaceHolder phNoResult = (PlaceHolder)item.FindControl("phNoResults");
                        if (phNoResult != null)
                        {
                            phNoResult.Visible = false;
                        }

                        Repeater repPlayers = (Repeater)item.FindControl("repPlayers");
                        if (repPlayers != null)
                        {
                            repPlayers.DataSource = awp.Players;
                            repPlayers.DataBind();
                        }
                    }
                    else
                    {
                        PlaceHolder phPlayers = (PlaceHolder)item.FindControl("phPlayers");
                        if (phPlayers != null)
                        {
                            phPlayers.Visible = false;
                        }
                    }
                }
            }
        }
    }
}