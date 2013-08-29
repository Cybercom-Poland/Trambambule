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
        }

        protected void tbxPlayers_TextChanged(object sender, EventArgs e)
        {
            Player player = DataAccess.GetPlayer(tbxPlayers.Text);
            if (player == null) tbxPlayers.Text = string.Empty;
            else Session["UserBasicStatsPlayer"] = player;
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

                List<TeamMatch> lastGames = playerMatches.OrderByDescending(p => p.Timestamp).Take(10).ToList();
                if(lastGames.Any())
                {
                    TeamMatchPlayer playerData = lastGames.First().TeamMatchPlayers.First(p => p.PlayerId == player.Id);
                    sb.AppendLine("Pozycja w rankingu: " + context.GetPlayerRankPosition(player.Id));
                    sb.AppendLine("Punktów rankingowych: " + (playerData.Rating.HasValue ? playerData.Rating.Value.ToString("n0") : string.Empty));
                    string form = string.Empty;
                    foreach(TeamMatch tm in lastGames)
                        form += GetMatchResultLabel(tm);
                    sb.AppendLine("Forma: " + form);
                    sb.Append("<hr/>");
                }

                sb.AppendLine("Rozegranych: " + playerMatches.Select(p => new { Mid = p.MatchId }).Distinct().Count());
                sb.AppendLine("Wygranych: " + playerMatches.Select(p => new { Mid = p.MatchId, Result = p.Result }).Distinct().Count(p => p.Result == (int)Common.EResult.Win));
                sb.AppendLine("Przegranych: " + playerMatches.Select(p => new { Mid = p.MatchId, Result = p.Result }).Distinct().Count(p => p.Result == (int)Common.EResult.Loose));
                sb.AppendLine("Zremisowanych: " + playerMatches.Select(p => new { Mid = p.MatchId, Result = p.Result }).Distinct().Count(p => p.Result == (int)Common.EResult.Draw));
                sb.AppendLine("Goli strzelonych: " + playerMatches.Select(p => new { Mid = p.MatchId, GoalsScored = p.GoalsScored }).Distinct().Sum(p => p.GoalsScored));
                sb.AppendLine("Goli straconych: " + playerMatches.Select(p => new { Mid = p.MatchId, GoalsLost = p.GoalsLost }).Distinct().Sum(p => p.GoalsLost));

                sb.Append("<hr/>");

                sb.AppendLine("Wygranych [atak]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Win
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence));
                sb.AppendLine("Przegranych [atak]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Loose
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence));
                sb.AppendLine("Zremisowanych [atak]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Draw
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence));
                sb.AppendLine("Goli strzelonych [atak]: " + playerMatches.Where(p =>
                    p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence).Sum(p => p.GoalsScored));
                sb.AppendLine("Goli straconych [atak]: " + playerMatches.Where(p =>
                    p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence).Sum(p => p.GoalsLost));
                try
                {
                    sb.AppendLine("Śr. goli strzelonych [atak]: " + playerMatches.Where(p =>
                        p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence).Average(p => p.GoalsScored).ToString("n2"));
                    sb.AppendLine("Śr. goli straconych [atak]: " + playerMatches.Where(p =>
                        p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence).Average(p => p.GoalsLost).ToString("n2"));
                }
                catch { }
                
                sb.Append("<hr/>");

                sb.AppendLine("Wygranych [obrona]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Win
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence));
                sb.AppendLine("Przegranych [obrona]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Loose
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence));
                sb.AppendLine("Zremisowanych [obrona]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Draw
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence));
                sb.AppendLine("Goli strzelonych [obrona]: " + playerMatches.Where(p =>
                    p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence).Sum(p => p.GoalsScored));
                sb.AppendLine("Goli straconych [obrona]: " + playerMatches.Where(p =>
                    p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence).Sum(p => p.GoalsLost));
                try
                {
                    sb.AppendLine("Śr. goli strzelonych [obrona]: " + playerMatches.Where(p =>
                        p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence).Average(p => p.GoalsScored).ToString("n2"));
                    sb.AppendLine("Śr. goli straconych [obrona]: " + playerMatches.Where(p =>
                        p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence).Average(p => p.GoalsLost).ToString("n2"));
                }
                catch { }
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