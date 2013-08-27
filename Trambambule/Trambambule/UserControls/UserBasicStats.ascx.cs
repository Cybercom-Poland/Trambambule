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
                ddlPlayers.DataSource = DataAccess.GetPlayers()
                    .Select(p => new { Id = p.Id, Name = PlayerHelper.GetPlayerName(p) })
                    .OrderBy(p => p.Name);
                ddlPlayers.DataBind();
                if (Session["UserBasicStatsPlayer"] != null)
                    try { ddlPlayers.SelectedValue = ((Player)Session["UserBasicStatsPlayer"]).Id.ToString(); }
                    catch { }
                BindPlayerStats();
            }
        }

        protected void ddlPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPlayerStats();
        }

        private void BindPlayerStats()
        {
            Player player = DataAccess.GetPlayer(ddlPlayers.SelectedItem.Text);
            if (player == null) return;

            Session["UserBasicStatsPlayer"] = player;
            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                List<TeamMatch> playerMatches = context.TeamMatches.Where(p =>
                    p.TeamMatchPlayers.Any(x => x.PlayerId == player.Id)).ToList();
                StringBuilder sb = new StringBuilder();

                List<TeamMatch> lastGames = playerMatches.OrderByDescending(p => p.Timestamp).Take(5).ToList();
                if(lastGames.Any())
                {
                    TeamMatchPlayer playerData = lastGames.First().TeamMatchPlayers.First(p => p.PlayerId == player.Id);
                    sb.AppendLine("Obecny RD: " + playerData.RD);
                    sb.AppendLine("Obecny ranking: " + playerData.Rating);
                    string form = string.Empty;
                    foreach(TeamMatch tm in lastGames)
                        form += GetMatchResultLabel(tm);
                    sb.AppendLine("Forma: " + form);
                    sb.Append("<hr/>");
                }

                sb.AppendLine("Rozegranych: " + playerMatches.Count());
                sb.AppendLine("Wygranych: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Win));
                sb.AppendLine("Przegranych: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Loose));
                sb.AppendLine("Zremisowanych: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Draw));
                sb.AppendLine("Goli strzelonych: " + playerMatches.Sum(p => p.GoalsScored));
                sb.AppendLine("Goli straconych: " + playerMatches.Sum(p => p.GoalsLost));

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
                default :
                    return string.Empty;
            }
        }
    }
}