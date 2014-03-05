using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Trambambule.Achievements.Helpers;
using System.Text.RegularExpressions;
//using Trambambule.Achievements.Helpers;

namespace Trambambule.UserControls
{
    public partial class UserBasicStats : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["UserBasicStatsPlayer"] != null)
                    try { tbxPlayers.Text = PlayerHelper.GetPlayerName((Player)Session["UserBasicStatsPlayer"]);  }
                    catch { }
            }
            Page.LoadComplete += new EventHandler(Page_LoadComplete);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            BindPlayerStats();
            BindPlayerRankChart();
            if (!string.IsNullOrEmpty(tbxPlayers.Text))
            {
                lbtClear.Enabled = true;
                lbtClear.CssClass = "close";
            }
            else
            {
                lbtClear.Enabled = false;
                lbtClear.CssClass = "close disabled";
            }
        }

        protected void tbxPlayers_TextChanged(object sender, EventArgs e)
        {
            Player player = DataAccess.GetPlayer(tbxPlayers.Text);
            if (player == null) tbxPlayers.Text = string.Empty;
            else Session["UserBasicStatsPlayer"] = player;
        }

        private void BindPlayerRankChart()
        {
            lcRanking.Series.Clear();
            lcRanking.Visible = false;
            Player player = ((Player)Session["UserBasicStatsPlayer"]);
            if (player == null) return;
            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                List<TeamMatchPlayer> hist = context.TeamMatchPlayers.Where(p => p.PlayerId == player.Id)
                    .OrderByDescending(p => p.Timestamp).Take(10).OrderBy(p => p.Timestamp).ToList();
                if (!hist.Any() || hist.Count < 2) return;
                lcRanking.CategoriesAxis = string.Join(",", hist.Select(p => p.Timestamp.ToString("dd-MM-yyyy HH:mm:ss")).ToArray());
                AjaxControlToolkit.LineChartSeries lcs = new AjaxControlToolkit.LineChartSeries();
                lcs.Data = hist.Where(p => p.RankPosition.HasValue).Select(p => (decimal)p.RankPosition.Value).ToArray();
                lcs.Name = "Historia pozycji rankingowej";
                lcRanking.Series.Add(lcs);
                lcRanking.Visible = true;
            }
        }

        private void BindPlayerStats()
        {
            Player player = ((Player)Session["UserBasicStatsPlayer"]);
            if (player == null) return;

            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                List<TeamMatch> playerMatches = context.TeamMatches.Where(p =>
                    p.TeamMatchPlayers.Any(x => x.PlayerId == player.Id)).ToList();
                StringBuilder sb = new StringBuilder();

                List<TeamMatch> lastGames = playerMatches.OrderByDescending(p => p.Timestamp).ToList();
                if(lastGames.Any())
                {
                    TeamMatchPlayer playerData = lastGames.First().TeamMatchPlayers.First(p => p.PlayerId == player.Id);
                    sb.AppendLine("Obecna pozycja w rankingu: " + context.GetPlayerRankPosition(player.Id));
                    sb.AppendLine("Punkty rankingowe: " + (playerData.Rating.HasValue
                        ? ((int)playerData.Rating.Value).ToString() : string.Empty));                    
                    string form = string.Empty;
                    foreach (TeamMatch tm in lastGames.Take(15).ToList())
                        form += GetMatchResultLabel(tm);
                    sb.AppendLine("Forma: " + form);
                    sb.Append("<hr/>");
                }

                //sb.AppendLine("Rozegranych: " + playerMatches.Select(p => new { Mid = p.MatchId }).Distinct().Count());
                sb.AppendLine("Bilans W/R/P: "
                    + playerMatches.Select(p => new { Mid = p.MatchId, Result = p.Result }).Distinct().Count(p => p.Result == (int)Common.EResult.Win) + " / "
                    + playerMatches.Select(p => new { Mid = p.MatchId, Result = p.Result }).Distinct().Count(p => p.Result == (int)Common.EResult.Draw) + " / "
                    + playerMatches.Select(p => new { Mid = p.MatchId, Result = p.Result }).Distinct().Count(p => p.Result == (int)Common.EResult.Loose));

                sb.AppendLine("Bilans W/R/P [atak]: "
                    + playerMatches.Count(p => p.Result == (int)Common.EResult.Win
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence) + " / "
                    + playerMatches.Count(p => p.Result == (int)Common.EResult.Draw
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence) + " / "
                    + playerMatches.Count(p => p.Result == (int)Common.EResult.Loose
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence));
                
                sb.AppendLine("Bilans W/R/P [obrona]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Win
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence) + " / "
                    + playerMatches.Count(p => p.Result == (int)Common.EResult.Draw
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence) + " / "
                    + playerMatches.Count(p => p.Result == (int)Common.EResult.Loose
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence));

                sb.AppendLine("Bilans bramek: "
                    + playerMatches.Select(p => new { Mid = p.MatchId, GoalsScored = p.GoalsScored }).Distinct().Sum(p => p.GoalsScored)
                    + " / "
                    + playerMatches.Select(p => new { Mid = p.MatchId, GoalsLost = p.GoalsLost }).Distinct().Sum(p => p.GoalsLost));

                var achievements = (from ap in context.AchievementPlayer
                                   join p in context.Players
                                   on ap.PlayerId equals p.Id
                                   join a in context.Achievements
                                   on ap.AchievementId equals a.Id
                                   where p.Id == player.Id
                                    select new PlayerWithAchievement(p, ap.LevelOfAchievement, ap.ToNextLevelLabel, a)).ToList();

                if (achievements.Any())
                {
                    sb.AppendLine("<hr/>Osiągnięcia:");
                    for (int i = 0; i < achievements.Count; i++)
                    {
                        var ach = achievements[i];
                        //if (ach.LevelOfAchievement>0)
                        //    sb.AppendFormat("{0}. Poziom {1} ({2}) w osiągnięciu \"{3}\".<br/>", (i + 1).ToString(), Common.GetAchievementsLevelName(ach.LevelOfAchievement), GetLabel(ach.LevelOfAchievement, ach.Achievement), ach.Achievement.Title);
                        //else
                        //    sb.AppendFormat("{0}. Nie zdobyto jeszcze żadnego poziomu w osiągnięciu \"{2}\". {3}.", (i+1).ToString(), 

                        if (ach.LevelOfAchievement > 0)
                            sb.AppendFormat("<b>{0}. {1}</b><br/>Poziom {2}: {3}.<br/>{4}.<br/>", (i + 1).ToString(), ach.Achievement.Title, Common.GetAchievementsLevelName(ach.LevelOfAchievement), GetLabel(ach.LevelOfAchievement, ach.Achievement), GetTextWithAnchors(ach.ToNextLevelLabel));
                        else
                            sb.AppendFormat("<b>{0}. {1}</b><br/>Nie osiągnięto jeszcze żadnego poziomu.<br/>{2}.<br/>",(i + 1).ToString(), ach.Achievement.Title, GetTextWithAnchors(ach.ToNextLevelLabel));
                    }
                    sb.Append("<hr/>");
                }

                sb = sb.Replace(Environment.NewLine, "<br/>");

                lblStatsDetails.Text = sb.ToString();
            }
        }

        protected string GetTextWithAnchors(string text)
        {
            var matches = Regex.Matches(text, @"{P_\d*}");
            foreach (System.Text.RegularExpressions.Match m in matches)
            {
                int id = int.Parse(m.Value.Replace("{P_", "").Replace("}", ""));
                text = text.Replace(m.Value, PlayerHelper.GetPlayerNameLink(DataAccess.GetPlayer(id)));
            }
            return text;
        }

        public static string GetLabel(int level, Achievement achievement)
        {
            return level == 1 ? achievement.Level1Label : level == 2 ? achievement.Level2Label : achievement.Level3Label;
        }

        protected void lbtClear_Click(object sender, EventArgs e)
        {
            Session["UserBasicStatsPlayer"] = null;
            string rawUrl = Request.RawUrl;
            Response.Redirect(rawUrl.Contains("?") ? rawUrl.Substring(0, rawUrl.IndexOf("?")) : rawUrl);
        }

        private string GetMatchResultLabel(TeamMatch tm)
        {
            Common.EResult result = (Common.EResult)Enum.Parse(typeof(Common.EResult), tm.Result.ToString());
            switch (result)
            {
                case Common.EResult.Draw:
                    return "<span style='color: blue;'>R </span>";
                case Common.EResult.Loose:
                    return "<span style='color: red;'>P </span>";
                case Common.EResult.Win:
                    return "<span style='color: green;'>Z </span>";
                default:
                    return string.Empty;
            }
        }
    }
}