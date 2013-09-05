using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Trambambule.UserControls
{
    public partial class UserBasicStats : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["UserBasicStatsPlayer"] != null)
                    try { tbxPlayers.Text = PlayerHelper.GetPlayerName((Player)Session["UserBasicStatsPlayer"]); }
                    catch { }
            }
            Page.LoadComplete += new EventHandler(Page_LoadComplete);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            BindPlayerStats();
            BindPlayerRankChart();
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
                lcs.Name = "Zmiany pozycji w rankingu";
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
                    sb.AppendLine("Pozycja w rankingu: " + context.GetPlayerRankPosition(player.Id));
                    sb.AppendLine("Punktów rankingowych: " + (playerData.Rating.HasValue
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

                sb = sb.Replace(Environment.NewLine, "<br/>");
                lblStatsDetails.Text = sb.ToString();
            }
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

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetPlayerNames(string prefixText, int count)
        {
            List<Player> players = DataAccess.GetPlayers();
            if (players == null || players.Count == 0) return null;

            return players.Where(p => p.FirstName.ToLower().Contains(prefixText.ToLower()) ||
                    p.LastName.ToLower().Contains(prefixText.ToLower()))
                .OrderBy(p => p.LastName).ThenBy(p => p.FirstName).Take(count)
                .Select(p => PlayerHelper.GetPlayerName(p)).ToArray();
        }
    }
}