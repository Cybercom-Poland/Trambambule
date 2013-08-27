using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace Trambambule
{
    public partial class SendResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                tbxPlayer1Off.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<Player> players = DataAccess.GetPlayers();
            int g1 = int.Parse(tbxScoreA.Text);
            int g2 = int.Parse(tbxScoreB.Text);
            Common.EResult t1Result = Common.GetResult(g1, g2);
            Common.EResult t2Result = Common.GetResult(g2, g1);

            Match match = new Match() { Id = Guid.NewGuid(), Timestamp = DateTime.Now };
            TeamMatch tm1 = new TeamMatch() { Id = Guid.NewGuid(), MatchId = match.Id, 
                Result = (byte)t1Result, GoalsScored = g1, GoalsLost = g2, Timestamp = DateTime.Now };
            TeamMatch tm2 = new TeamMatch() { Id = Guid.NewGuid(), MatchId = match.Id, 
                Result = (byte)t2Result, GoalsScored = g2, GoalsLost = g1, Timestamp = DateTime.Now };
            Player p1Off = DataAccess.GetPlayer(tbxPlayer1Off.Text);
            Player p1Def = DataAccess.GetPlayer(tbxPlayer1Deff.Text);
            Player p2Off = DataAccess.GetPlayer(tbxPlayer2Off.Text);
            Player p2Def = DataAccess.GetPlayer(tbxPlayer2Deff.Text);

            TeamMatchPlayer tmp1Off = new TeamMatchPlayer()
            {
                PlayerId = p1Off.Id,
                Position = (int)Common.EPosition.Offence,
                Timestamp = DateTime.Now,
                TeamMatchId = tm1.Id
            };
            TeamMatchPlayer tmp1Def = new TeamMatchPlayer()
            {
                PlayerId = p1Def.Id,
                Position = (int)Common.EPosition.Defence,
                Timestamp = DateTime.Now,
                TeamMatchId = tm1.Id
            };
            TeamMatchPlayer tmp2Off = new TeamMatchPlayer()
            {
                PlayerId = p2Off.Id,
                Position = (int)Common.EPosition.Offence,
                Timestamp = DateTime.Now,
                TeamMatchId = tm2.Id
            };
            TeamMatchPlayer tmp2Def = new TeamMatchPlayer()
            {
                PlayerId = p2Def.Id,
                Position = (int)Common.EPosition.Defence,
                Timestamp = DateTime.Now,
                TeamMatchId = tm2.Id
            };

            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                PlayerHelper.FillPlayersRating(ref tmp1Off, ref tmp1Def, ref tmp2Off, ref tmp2Def,
                    context.TeamMatchPlayers.Where(p => p.PlayerId == tmp1Off.PlayerId)
                        .OrderByDescending(p => p.Timestamp).FirstOrDefault(),
                    context.TeamMatchPlayers.Where(p => p.PlayerId == tmp1Def.PlayerId)
                        .OrderByDescending(p => p.Timestamp).FirstOrDefault(),
                    context.TeamMatchPlayers.Where(p => p.PlayerId == tmp2Off.PlayerId)
                        .OrderByDescending(p => p.Timestamp).FirstOrDefault(),
                    context.TeamMatchPlayers.Where(p => p.PlayerId == tmp2Def.PlayerId)
                        .OrderByDescending(p => p.Timestamp).FirstOrDefault(),
                    g1, g2);

                context.Matches.InsertOnSubmit(match);
                context.TeamMatches.InsertOnSubmit(tm1);
                context.TeamMatches.InsertOnSubmit(tm2);
                context.TeamMatchPlayers.InsertOnSubmit(tmp1Def);
                context.TeamMatchPlayers.InsertOnSubmit(tmp2Def);
                context.TeamMatchPlayers.InsertOnSubmit(tmp1Off);
                context.TeamMatchPlayers.InsertOnSubmit(tmp2Off);
                context.SubmitChanges();
            }
            pnlInfo.Controls.Add(new LiteralControl("Mecz został zapisany"));
            tbxScoreA.Text = tbxScoreB.Text = "";
            tbxPlayer1Off.Focus();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            tbxPlayer1Off.Text = tbxPlayer2Off.Text = tbxPlayer1Deff.Text =
                tbxPlayer2Deff.Text = tbxScoreA.Text = tbxScoreB.Text = "";
        }

        protected void tbxPlayer_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            List<Player> players = DataAccess.GetPlayers();
            if (!players.Any(p => tb.Text.Equals(PlayerHelper.GetPlayerName(p))))
            {
                tb.Text = string.Empty;
                tb.Focus();
            }
            else if (tb == tbxPlayer1Off) tbxPlayer2Off.Focus();
            else if (tb == tbxPlayer2Off) tbxPlayer1Deff.Focus();
            else if (tb == tbxPlayer1Deff) tbxPlayer2Deff.Focus();
            else if (tb == tbxPlayer2Deff) tbxScoreA.Focus();
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