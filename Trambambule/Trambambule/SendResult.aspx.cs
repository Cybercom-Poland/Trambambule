using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Trambambule.Achievements.Helpers;
using System.Text;
using Trambambule.Achievements;


namespace Trambambule
{
    public partial class SendResult : System.Web.UI.Page
    {
        private const string PreSubmittedStats = "Ranking: <b>{0}</b><br/>Punkty: <b>{1}</b>";
        private const string PostSubmittedStats = "Ranking: <b>{0}</b> ({1})<br/>Punkty: <b>{2}</b> ({3})";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                tbxPlayer1Off.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            litAchievements.Text = "";
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
            List<Player> playersInThisGame = new List<Player>() { p1Off, p1Def, p2Off, p2Def };
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

            double? player1OffPreRating, player1DefPreRating, player2OffPreRating, player2DefPreRating;
            double? player1OffPreRankPosition = null;
            double? player1DefPreRankPosition = null;
            double? player2OffPreRankPosition = null;
            double? player2DefPreRankPosition = null;
            int? player1OffPostRankPosition, player1DefPostRankPosition, player2OffPostRankPosition, player2DefPostRankPosition;

            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                var player1Off = context.TeamMatchPlayers.Where(p => p.PlayerId == tmp1Off.PlayerId)
                        .OrderByDescending(p => p.Timestamp).FirstOrDefault();

                var player1Def = context.TeamMatchPlayers.Where(p => p.PlayerId == tmp1Def.PlayerId)
                        .OrderByDescending(p => p.Timestamp).FirstOrDefault();

                var player2Off = context.TeamMatchPlayers.Where(p => p.PlayerId == tmp2Off.PlayerId)
                        .OrderByDescending(p => p.Timestamp).FirstOrDefault();

                var player2Def = context.TeamMatchPlayers.Where(p => p.PlayerId == tmp2Def.PlayerId)
                        .OrderByDescending(p => p.Timestamp).FirstOrDefault();

                if (player1Off != null)
                {
                    player1OffPreRating = player1Off.Rating;
                    player1OffPreRankPosition = context.GetPlayerRankPosition(player1Off.PlayerId);
                }
                else
                {
                    player1OffPreRating = PlayerHelper.INITIAL_RATING;
                    player1OffPreRankPosition = null;
                }

                if (player1Def != null)
                {
                    player1DefPreRating = player1Def.Rating;
                    player1DefPreRankPosition = context.GetPlayerRankPosition(player1Def.PlayerId);
                }
                else
                {
                    player1DefPreRating = PlayerHelper.INITIAL_RATING;
                    player1DefPreRankPosition = null;
                }

                if (player2Off != null)
                {
                    player2OffPreRating = player2Off.Rating;
                    player2OffPreRankPosition = context.GetPlayerRankPosition(player2Off.PlayerId);
                }
                else
                {
                    player2OffPreRating = PlayerHelper.INITIAL_RATING;
                    player2OffPreRankPosition = null;
                }

                if (player2Def != null)
                {
                    player2DefPreRating = player2Def.Rating;
                    player2DefPreRankPosition = context.GetPlayerRankPosition(player2Def.PlayerId);
                }
                else
                {
                    player2DefPreRating = PlayerHelper.INITIAL_RATING;
                    player2DefPreRankPosition = null;
                }

                PlayerHelper.FillPlayersRating(ref tmp1Off, ref tmp1Def, ref tmp2Off, ref tmp2Def,
                    player1Off,
                    player1Def,
                    player2Off,
                    player2Def,
                    g1, g2);

                context.Matches.InsertOnSubmit(match);
                context.TeamMatches.InsertOnSubmit(tm1);
                context.TeamMatches.InsertOnSubmit(tm2);
                context.TeamMatchPlayers.InsertOnSubmit(tmp1Def);
                context.TeamMatchPlayers.InsertOnSubmit(tmp2Def);
                context.TeamMatchPlayers.InsertOnSubmit(tmp1Off);
                context.TeamMatchPlayers.InsertOnSubmit(tmp2Off);
                context.SubmitChanges();

                player1OffPostRankPosition = context.GetPlayerRankPosition(tmp1Off.PlayerId);
                player1DefPostRankPosition = context.GetPlayerRankPosition(tmp1Def.PlayerId);
                player2OffPostRankPosition = context.GetPlayerRankPosition(tmp2Off.PlayerId);
                player2DefPostRankPosition = context.GetPlayerRankPosition(tmp2Def.PlayerId);
            }

            double player1OffRatingChange = Math.Round((double)(tmp1Off.Rating - player1OffPreRating),0);
            double player1DefRatingChange = Math.Round((double)(tmp1Def.Rating - player1DefPreRating),0);
            double player2OffRatingChange = Math.Round((double)(tmp2Off.Rating - player2OffPreRating),0);
            double player2DefRatingChange = Math.Round((double)(tmp2Def.Rating - player2DefPreRating),0);

            int player1OffRankPositionChange = player1OffPreRankPosition.HasValue ? (int)(player1OffPreRankPosition - player1OffPostRankPosition) : -1;
            int player1DefRankPositionChange = player1DefPreRankPosition.HasValue ? (int)(player1DefPreRankPosition - player1DefPostRankPosition) : -1;
            int player2OffRankPositionChange = player2OffPreRankPosition.HasValue ? (int)(player2OffPreRankPosition - player2OffPostRankPosition) : -1;
            int player2DefRankPositionChange = player2DefPreRankPosition.HasValue ? (int)(player2DefPreRankPosition - player2DefPostRankPosition) : -1;

            litTbxPlayer1OffStats.Text = string.Format(PostSubmittedStats, player1OffPostRankPosition, GetColouredRankPosition(player1OffRankPositionChange), Math.Round((double)tmp1Off.Rating, 0), GetColouredRating(player1OffRatingChange));
            litTbxPlayer1DeffStats.Text = string.Format(PostSubmittedStats, player1DefPostRankPosition, GetColouredRankPosition(player1DefRankPositionChange), Math.Round((double)tmp1Def.Rating, 0), GetColouredRating(player1DefRatingChange));
            litTbxPlayer2OffStats.Text = string.Format(PostSubmittedStats, player2OffPostRankPosition, GetColouredRankPosition(player2OffRankPositionChange), Math.Round((double)tmp2Off.Rating, 0), GetColouredRating(player2OffRatingChange));
            litTbxPlayer2DeffStats.Text = string.Format(PostSubmittedStats, player2DefPostRankPosition, GetColouredRankPosition(player2DefRankPositionChange), Math.Round((double)tmp2Def.Rating, 0), GetColouredRating(player2DefRatingChange));

            pnlInfo.Controls.Add(new LiteralControl("Mecz został zapisany"));
            tbxScoreA.Text = tbxScoreB.Text = "";
            tbxPlayer1Off.Focus();

            CalculateAchievements(playersInThisGame);
            Cache.Remove(DataAccess.OverallStatsCacheName);
        }

        private void CalculateAchievements(List<Player> playersInThisGame)
        {
            List<AchievementTextWithIds> achievementsData = new AchievementsService(playersInThisGame).RecalculateAchievements(false);
            if (achievementsData.Count > 0)
            {
                Cache.Remove(DataAccess.PlayersAchievementsCacheName);
                Cache.Remove(DataAccess.AllAchievementsCacheName);

                StringBuilder sb = new StringBuilder("<br/><br/><b>Zdobyte osiągnięcia:</b><br/><ul class=\"grantedAchievements\">");
                achievementsData.ForEach(w => sb.Append("<li>" + w.Text + "</li>"));
                sb.Append("</ul>");

                litAchievements.Text = sb.ToString();
            }
        }

        private string GetColouredRankPosition(int ranking)
        {
            if (ranking == -1)
                return "-";
            else if (ranking == 0)
                return ranking.ToString();
            else if (ranking > 0)
                return "<span style=\"color: green;\">+" + ranking.ToString() + "</span>";
            else
                return "<span style=\"color: red;\">" + ranking.ToString() + "</span>";
        }

        private string GetColouredRating(double ranking)
        {
            if (ranking == 0)
                return ranking.ToString();
            else if (ranking > 0)
                return "<span style=\"color: green;\">+" + ranking.ToString() + "</span>";
            else
                return "<span style=\"color: red;\">" + ranking.ToString() + "</span>";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            tbxPlayer1Off.Text = tbxPlayer2Off.Text = tbxPlayer1Deff.Text =
                tbxPlayer2Deff.Text = tbxScoreA.Text = tbxScoreB.Text = litAchievements.Text = 
                litTbxPlayer1DeffStats.Text = litTbxPlayer1OffStats.Text = litTbxPlayer2DeffStats.Text = litTbxPlayer2OffStats.Text = "";
        }

        protected void tbxPlayer_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            List<Player> players = DataAccess.GetPlayers();

            //ie11 fix - the rest is in javascript
            if (!players.Any(p => tb.Text.Equals(PlayerHelper.GetPlayerName(p))) ||
                (tb != tbxPlayer1Deff && tb.Text == tbxPlayer1Deff.Text) ||
                (tb != tbxPlayer2Deff && tb.Text == tbxPlayer2Deff.Text) ||
                (tb != tbxPlayer1Off && tb.Text == tbxPlayer1Off.Text) ||
                (tb != tbxPlayer2Off && tb.Text == tbxPlayer2Off.Text))
            {
                tb.Text = string.Empty;
                //tb.Focus();
            }
            //else if (tb == tbxPlayer1Off) tbxPlayer2Off.Focus();
            //else if (tb == tbxPlayer2Off) tbxPlayer1Deff.Focus();
            //else if (tb == tbxPlayer1Deff) tbxPlayer2Deff.Focus();
            //else if (tb == tbxPlayer2Deff) tbxScoreA.Focus();

            btnSubmit.Focus();

            if (string.IsNullOrEmpty(tbxPlayer1Off.Text))
                litTbxPlayer1OffStats.Text = "";

            if (string.IsNullOrEmpty(tbxPlayer1Deff.Text))
                litTbxPlayer1DeffStats.Text = "";

            if (string.IsNullOrEmpty(tbxPlayer2Off.Text))
                litTbxPlayer2OffStats.Text = "";

            if (string.IsNullOrEmpty(tbxPlayer2Deff.Text))
                litTbxPlayer2DeffStats.Text = "";

            if (!string.IsNullOrEmpty(tbxPlayer1Off.Text) || !string.IsNullOrEmpty(tbxPlayer1Deff.Text) ||
                !string.IsNullOrEmpty(tbxPlayer2Off.Text) || !string.IsNullOrEmpty(tbxPlayer2Deff.Text))
            {
                GetStatsBeforeSubmitting();
            }
        }

        private void GetStatsBeforeSubmitting()
        {
            Player p1Off = DataAccess.GetPlayer(tbxPlayer1Off.Text);
            Player p1Def = DataAccess.GetPlayer(tbxPlayer1Deff.Text);
            Player p2Off = DataAccess.GetPlayer(tbxPlayer2Off.Text);
            Player p2Def = DataAccess.GetPlayer(tbxPlayer2Deff.Text);

            if (p1Off != null || p1Def != null || p2Off != null || p2Def != null)
            {
                using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
                {
                    if (p1Off != null)
                    {
                        var player1Off = context.TeamMatchPlayers.Where(p => p.PlayerId == p1Off.Id)
                                .OrderByDescending(p => p.Timestamp).FirstOrDefault();

                        if (player1Off == null)
                            litTbxPlayer1OffStats.Text = string.Format(PreSubmittedStats, "-", PlayerHelper.INITIAL_RATING);
                        else
                            litTbxPlayer1OffStats.Text = string.Format(PreSubmittedStats, context.GetPlayerRankPosition(player1Off.PlayerId), Math.Round((double)player1Off.Rating, 0));
                    }

                    if (p1Def != null)
                    {
                        var player1Def = context.TeamMatchPlayers.Where(p => p.PlayerId == p1Def.Id)
                                .OrderByDescending(p => p.Timestamp).FirstOrDefault();

                        if (player1Def == null)
                            litTbxPlayer1DeffStats.Text = string.Format(PreSubmittedStats, "-", PlayerHelper.INITIAL_RATING);
                        else
                            litTbxPlayer1DeffStats.Text = string.Format(PreSubmittedStats, context.GetPlayerRankPosition(player1Def.PlayerId), Math.Round((double)player1Def.Rating, 0));
                    }

                    if (p2Off != null)
                    {
                        var player2Off = context.TeamMatchPlayers.Where(p => p.PlayerId == p2Off.Id)
                                .OrderByDescending(p => p.Timestamp).FirstOrDefault();

                        if (player2Off == null)
                            litTbxPlayer2OffStats.Text = string.Format(PreSubmittedStats, "-", PlayerHelper.INITIAL_RATING);
                        else
                            litTbxPlayer2OffStats.Text = string.Format(PreSubmittedStats, context.GetPlayerRankPosition(player2Off.PlayerId), Math.Round((double)player2Off.Rating, 0));
                    }

                    if (p2Def != null)
                    {
                        var player2Def = context.TeamMatchPlayers.Where(p => p.PlayerId == p2Def.Id)
                                .OrderByDescending(p => p.Timestamp).FirstOrDefault();

                        if (player2Def == null)
                            litTbxPlayer2DeffStats.Text = string.Format(PreSubmittedStats, "-", PlayerHelper.INITIAL_RATING);
                        else
                            litTbxPlayer2DeffStats.Text = string.Format(PreSubmittedStats, context.GetPlayerRankPosition(player2Def.PlayerId), Math.Round((double)player2Def.Rating, 0));
                    }
                }
            }
        }
   


        protected void ibtn1_Click(object sender, ImageClickEventArgs e)
        {
            string temp = tbxPlayer1Deff.Text;
            tbxPlayer1Deff.Text = tbxPlayer1Off.Text;
            tbxPlayer1Off.Text = temp;

            //pobrać ostatni rating i rank position
            GetStatsBeforeSubmitting();
        }

        protected void ibtn2_Click(object sender, ImageClickEventArgs e)
        {
            string temp = tbxPlayer2Deff.Text;
            tbxPlayer2Deff.Text = tbxPlayer2Off.Text;
            tbxPlayer2Off.Text = temp;

            //pobrać ostatni rating i rank position
            GetStatsBeforeSubmitting();
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetPlayerNames(string prefixText, int count)
        {
            List<Player> players = DataAccess.GetPlayers();
            if (players == null || players.Count == 0) return null;

            return players.Where(p => Common.CompareNames(p, prefixText))
                .OrderBy(p => p.LastName).ThenBy(p => p.FirstName).Take(count)
                .Select(p => PlayerHelper.GetPlayerName(p)).ToArray();
        }
    }
}