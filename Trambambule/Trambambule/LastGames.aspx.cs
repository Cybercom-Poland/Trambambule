using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Trambambule
{
    public partial class LastGames : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.LoadComplete += new EventHandler(Page_LoadComplete);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session["UserBasicStatsPlayer"] != null)
                BindLastGames();
        }

        private void BindLastGames()
        {
            Player player = (Player)Session["UserBasicStatsPlayer"];
            tblResults.Rows.Clear();

            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                List<Match> playerMatches = context.Matches.Where(p => p.TeamMatches.Any(x =>
                    x.TeamMatchPlayers.Any(z => z.PlayerId == player.Id)))
                    .OrderByDescending(p => p.Timestamp).Take(50).ToList();
                if (!playerMatches.Any()) return;

                foreach (Match m in playerMatches) AddRow(m, player);
            }
        }

        private void AddRow(Match m, Player p)
        {
            TableRow row = new TableRow();
            row.Cells.Add(new TableCell()
            {
                HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center,
                Text = m.Timestamp.ToString("dd-MM-yyyy HH:mm:ss")
            });
            //row.Cells.Add(new TableCell()
            //{
            //    HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center,
            //    Text = "ATAK<br/>OBRONA"
            //});
            row.Cells.Add(new TableCell()
            {
                HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center,
                Text = GetTeamString(m.TeamMatches[0], p)
            });
            row.Cells.Add(new TableCell()
            {
                HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center,
                Text = m.TeamMatches[0].GoalsScored + " : " + m.TeamMatches[0].GoalsLost
            });
            row.Cells.Add(new TableCell()
            {
                HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center,
                Text = GetTeamString(m.TeamMatches[1], p)
            });
            Common.EResult result = (Common.EResult)Enum.Parse(typeof(Common.EResult),
                m.TeamMatches.First(x => x.TeamMatchPlayers.Any(z => z.PlayerId == p.Id)).Result.ToString());
            switch (result)
            {
                case Common.EResult.Loose:
                    //row.CssClass = "loose";
                    row.Cells[2].ForeColor = Color.Red;
                    break;
                case Common.EResult.Win:
                    //row.CssClass = "win";
                    row.Cells[2].ForeColor = Color.Green;
                    break;
                case Common.EResult.Draw:
                    //row.CssClass = "draw";
                    row.Cells[2].ForeColor = Color.Blue;
                    break;
            }
            tblResults.Rows.Add(row);
        }

        private string GetTeamString(TeamMatch tm, Player p)
        {
            return string.Format("{0} ({2})<br/>{1} ({3})",
                GetPlayerNameString(tm.TeamMatchPlayers.First(x => x.Position == (byte)Common.EPosition.Offence).Player, p),
                GetPlayerNameString(tm.TeamMatchPlayers.First(x => x.Position == (byte)Common.EPosition.Defence).Player, p),
                GetRatingChangeString(tm.TeamMatchPlayers.First(x => x.Position == (byte)Common.EPosition.Offence).RatingChange),
                GetRatingChangeString(tm.TeamMatchPlayers.First(x => x.Position == (byte)Common.EPosition.Defence).RatingChange));
        }

        private string GetPlayerNameString(Player tmp, Player p)
        {
            return tmp.Id == p.Id
                ? ("<b>" + PlayerHelper.GetPlayerNameLink(tmp) + "</b>")
                : PlayerHelper.GetPlayerNameLink(tmp);
        }

        private string GetRatingChangeString(double? ratingChange)
        {
            if (!ratingChange.HasValue || (int)ratingChange.Value == 0)
            {
                return string.Format("<span style='color: blue;'>0</span>");
            }
            if (ratingChange.Value < 0)
            {
                return string.Format("<span style='color: red;'>{0}</span>", ratingChange.Value.ToString("n0"));
            }
            else
            {
                return string.Format("<span style='color: green;'>+{0}</span>", ratingChange.Value.ToString("n0"));
            }
        }
    }
}