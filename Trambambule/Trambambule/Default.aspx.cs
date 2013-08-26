using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Trambambule
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                gvPlayerResults.DataBind();

                ddlPlayers.DataSource = DataAccess.GetPlayers()
                    .Select(p => new { Id = p.Id, Name = DataAccess.GetPlayerName(p) })
                    .OrderBy(p => p.Name);
                ddlPlayers.DataBind();
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

            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                List<TeamMatch> playerMatches = context.TeamMatches.Where(p =>
                    p.TeamMatchPlayers.Any(x => x.PlayerId == player.Id)).ToList();
                List<TeamMatch> rivalMatches = context.TeamMatches.Where(p =>
                    !p.TeamMatchPlayers.Any(x => x.PlayerId == player.Id) &&
                    context.TeamMatches.Any(x => x.MatchId == p.MatchId && x.TeamMatchPlayers.Any(z => z.PlayerId == player.Id)))
                    .ToList();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Rozegranych: " + playerMatches.Count());
                sb.AppendLine("Wygranych: " +  playerMatches.Count(p => p.Result == (int)Common.EResult.Win));
                sb.AppendLine("Przegranych: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Loose));
                sb.AppendLine("Zremisowanych: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Draw));
                sb.AppendLine("Wygranych [atak]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Win
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence));
                sb.AppendLine("Przegranych [atak]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Loose
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence));
                sb.AppendLine("Zremisowanych [atak]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Draw
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence));
                sb.AppendLine("Wygranych [obrona]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Win
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence));
                sb.AppendLine("Przegranych [obrona]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Loose
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence));
                sb.AppendLine("Zremisowanych [obrona]: " + playerMatches.Count(p => p.Result == (int)Common.EResult.Draw
                    && p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence));
                sb.AppendLine("Goli strzelonych: " + playerMatches.Sum(p => p.Goals));
                sb.AppendLine("Goli straconych: " + rivalMatches.Sum(p => p.Goals));
                sb = sb.Replace(Environment.NewLine, "<br/>");
                pnlUserStatsDetails.Controls.Add(new LiteralControl(sb.ToString()));
            }
        }
    }
}