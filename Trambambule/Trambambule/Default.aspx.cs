﻿using System;
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
                    .Select(p => new { Id = p.Id, Name = PlayerHelper.GetPlayerName(p) })
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
                sb.AppendLine("Goli strzelonych: " + playerMatches.Sum(p => p.GoalsScored));
                sb.AppendLine("Goli straconych: " + playerMatches.Sum(p => p.GoalsLost));
                sb.AppendLine("Goli strzelonych [atak]: " + playerMatches.Where(p =>
                    p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence).Sum(p => p.GoalsScored));
                sb.AppendLine("Goli straconych [atak]: " + playerMatches.Where(p =>
                    p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence).Sum(p => p.GoalsLost));
                sb.AppendLine("Goli strzelonych [obrona]: " + playerMatches.Where(p => 
                    p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence).Sum(p => p.GoalsScored));
                sb.AppendLine("Goli straconych [obrona]: " + playerMatches.Where(p =>
                    p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence).Sum(p => p.GoalsLost));
                sb = sb.Replace(Environment.NewLine, "<br/>");
                pnlUserStatsDetails.Controls.Add(new LiteralControl(sb.ToString()));
            }
        }
    }
}