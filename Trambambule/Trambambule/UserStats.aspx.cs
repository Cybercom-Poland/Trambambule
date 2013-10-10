using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Trambambule
{
    public partial class UserStats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.LoadComplete += new EventHandler(Page_LoadComplete);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            BindGoalsStats();
            BindPartnersStats();
            BindRivalsStats();
            BindPlayerRatingStats();
            BindPlayerRatingChart();
            BindPlayerRankChart();
        }

        private void BindPlayerRatingChart()
        {
            lcRating.Series.Clear();
            lcRating.Visible = false;
            Player player = ((Player)Session["UserBasicStatsPlayer"]);
            if (player == null) return;
            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                List<TeamMatchPlayer> hist = context.TeamMatchPlayers.Where(p => p.PlayerId == player.Id)
                    .OrderByDescending(p => p.Timestamp).Take(20).OrderBy(p => p.Timestamp).ToList();
                if (!hist.Any() || hist.Count < 2) return;
                lcRating.CategoriesAxis = string.Join(",", hist.Select(p => p.Timestamp.ToString("dd-MM-yyyy HH:mm:ss")).ToArray());
                AjaxControlToolkit.LineChartSeries lcs = new AjaxControlToolkit.LineChartSeries();
                lcs.Data = hist.Select(p => (decimal)((int)
                    (p.Rating.Value - hist.Last().Rating.Value)
                    )).ToArray();
                lcs.Name = "Rożn. pkt. rank. z chwilą obecną";
                lcRating.Series.Add(lcs);
                lcRating.Visible = true;
            }
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
                    .OrderByDescending(p => p.Timestamp).Take(20).OrderBy(p => p.Timestamp).ToList();
                if (!hist.Any() || hist.Count < 2) return;
                lcRanking.CategoriesAxis = string.Join(",", hist.Select(p => p.Timestamp.ToString("dd-MM-yyyy HH:mm:ss")).ToArray());
                AjaxControlToolkit.LineChartSeries lcs = new AjaxControlToolkit.LineChartSeries();
                lcs.Data = hist.Where(p => p.RankPosition.HasValue).Select(p => (decimal)p.RankPosition.Value).ToArray();
                lcs.Name = "Historia pozycji rankingowej";
                lcRanking.Series.Add(lcs);
                lcRanking.Visible = true;
            }
        }

        private void BindPlayerRatingStats()
        {
            pnlRanking.Controls.Clear();
            Player player = ((Player)Session["UserBasicStatsPlayer"]);
            if (player == null) return;
            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                List<TeamMatchPlayer> playerMatches = context.TeamMatchPlayers.Where(p => p.PlayerId == player.Id)
                    .OrderByDescending(p => p.Timestamp).ToList();

                if (playerMatches.Any())
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Obecna pozycja w rankingu: "
                        + playerMatches.First().RankPosition);
                    sb.AppendLine("Najwyższa pozycja w rankingu: "
                        + playerMatches.Min(p => p.RankPosition));
                    sb.AppendLine("Najniższa pozycja w rankingu: "
                        + playerMatches.Max(p => p.RankPosition));
                    sb.AppendLine("Najwięcej punktów rankingowych: "
                        + playerMatches.Max(p => (int)p.Rating.Value));
                    sb.AppendLine("Najmniej punktów rankingowych: "
                        + playerMatches.Min(p => (int)p.Rating.Value));
                    sb.AppendLine("Notowany w rankingu od: "
                        + playerMatches.Min(p => p.Timestamp));
                    sb = sb.Replace(Environment.NewLine, "<br/>");

                    pnlRanking.Controls.Add(new LiteralControl(sb.ToString()));
                }
            }
        }

        private void BindGoalsStats()
        {
            pnlGoals.Controls.Clear();
            Player player = ((Player)Session["UserBasicStatsPlayer"]);
            if (player == null) return;

            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                List<TeamMatch> playerMatches = context.TeamMatches.Where(p =>
                    p.TeamMatchPlayers.Any(x => x.PlayerId == player.Id)).ToList();
                StringBuilder sb = new StringBuilder();

                //sb.AppendLine("Rozegranych: " + playerMatches.Select(p => new { Mid = p.MatchId }).Distinct().Count());
                
                sb.AppendLine("Bilans bramek: "
                    + playerMatches.Select(p => new { Mid = p.MatchId, GoalsScored = p.GoalsScored }).Distinct().Sum(p => p.GoalsScored)
                    + " / "
                    + playerMatches.Select(p => new { Mid = p.MatchId, GoalsLost = p.GoalsLost }).Distinct().Sum(p => p.GoalsLost));

                sb.Append("<hr/>");

                sb.AppendLine("Bilans bramek [atak]: "
                    + playerMatches.Where(p =>
                    p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence).Sum(p => p.GoalsScored)
                    + " / " + playerMatches.Where(p =>
                    p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence).Sum(p => p.GoalsLost));
                try
                {
                    sb.AppendLine("Średnia goli strzelonych [atak]: " + playerMatches.Where(p =>
                        p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence).Average(p => p.GoalsScored).ToString("n2"));
                    sb.AppendLine("Średnia goli straconych [atak]: " + playerMatches.Where(p =>
                        p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Offence).Average(p => p.GoalsLost).ToString("n2"));
                }
                catch { }


                sb.AppendLine("Bilans bramek [obrona]: "
                    + playerMatches.Where(p =>
                    p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence).Sum(p => p.GoalsScored)
                    + " / " + playerMatches.Where(p =>
                    p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence).Sum(p => p.GoalsLost));
                try
                {
                    sb.AppendLine("Średnia goli strzelonych [obrona]: " + playerMatches.Where(p =>
                        p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence).Average(p => p.GoalsScored).ToString("n2"));
                    sb.AppendLine("Średnia goli straconych [obrona]: " + playerMatches.Where(p =>
                        p.TeamMatchPlayers.First(x => x.PlayerId == player.Id).Position == (byte)Common.EPosition.Defence).Average(p => p.GoalsLost).ToString("n2"));
                }
                catch { }
                sb = sb.Replace(Environment.NewLine, "<br/>");

                pnlGoals.Controls.Add(new LiteralControl(sb.ToString()));
            }
        }

        private void BindPartnersStats()
        {
            pnlPartnerStats.Controls.Clear();
            Player player = ((Player)Session["UserBasicStatsPlayer"]);
            if (player == null) return;

            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                List<TeamMatch> playerMatches = context.TeamMatches.Where(p =>
                    p.TeamMatchPlayers.Any(x => x.PlayerId == player.Id)).ToList();
                StringBuilder sb = new StringBuilder();

                List<TeamMatch> lastGames = playerMatches.OrderByDescending(p => p.Timestamp).ToList();
                if (lastGames.Any())
                {
                    TeamMatchPlayer playerData = lastGames.First().TeamMatchPlayers.First(p => p.PlayerId == player.Id);
                    var partnerGames = playerMatches
                        .GroupBy(p => new { Partner = p.TeamMatchPlayers.First(x => x.PlayerId != playerData.PlayerId).Player })
                        .Select(x => new
                        {
                            //Partner = x.Key.Partner,
                            Name = PlayerHelper.GetPlayerNameLink(x.Key.Partner),
                            Games = x.Count(),
                            Won = x.Count(z => z.Result == (byte)Common.EResult.Win),
                            Drawn = x.Count(z => z.Result == (byte)Common.EResult.Draw),
                            Lost = x.Count(z => z.Result == (byte)Common.EResult.Loose),
                            Aggregate = x.Count(z => z.Result == (byte)Common.EResult.Win) - x.Count(z => z.Result == (byte)Common.EResult.Loose),
                            AvgAggregatePerc = 
                                100 * (x.Count(z => z.Result == (byte)Common.EResult.Win))
                                / (x.Count() == 0 ? 1 : x.Count()),
                            AvgGoalsScored = x.Average(z => z.GoalsScored),
                            AvgGoalsLost = x.Average(z => z.GoalsLost)
                        })
                        .OrderByDescending(p => p.AvgAggregatePerc);
                    gvPartners.DataSource = partnerGames;
                    gvPartners.DataBind();
                    sb.AppendLine(string.Format("Najczęstrzy partner: {0} ({1})",
                        partnerGames.OrderByDescending(p => p.Games).First().Name,
                        partnerGames.OrderByDescending(p => p.Games).First().Games));
                    sb.AppendLine(string.Format("Najlepszy bilans: {0} ({1})",
                        partnerGames.OrderByDescending(p => p.Aggregate).First().Name,
                        partnerGames.OrderByDescending(p => p.Aggregate).First().Aggregate < 0
                        ? (partnerGames.OrderByDescending(p => p.Aggregate).First().Aggregate).ToString()
                        : ("+" + partnerGames.OrderByDescending(p => p.Aggregate).First().Aggregate)));
                    sb.AppendLine(string.Format("Najgorszy bilans: {0} ({1})",
                        partnerGames.OrderBy(p => p.Aggregate).First().Name,
                        partnerGames.OrderBy(p => p.Aggregate).First().Aggregate < 0
                        ? (partnerGames.OrderBy(p => p.Aggregate).First().Aggregate).ToString()
                        : ("+" + partnerGames.OrderBy(p => p.Aggregate).First().Aggregate)));
                    sb.AppendLine(string.Format("Największy % wygranych: {0} ({1}%)",
                        partnerGames.OrderByDescending(p => p.AvgAggregatePerc).First().Name,
                        partnerGames.OrderByDescending(p => p.AvgAggregatePerc).First().AvgAggregatePerc));
                    sb.AppendLine(string.Format("Najmniejszy % wygranych: {0} ({1}%)",
                        partnerGames.OrderBy(p => p.AvgAggregatePerc).First().Name,
                        partnerGames.OrderBy(p => p.AvgAggregatePerc).First().AvgAggregatePerc));
                    sb.AppendLine(string.Format("Najwięcej goli zdobytych/mecz: {0} ({1})",
                        partnerGames.OrderByDescending(p => p.AvgGoalsScored).First().Name,
                        partnerGames.OrderByDescending(p => p.AvgGoalsScored).First().AvgGoalsScored.ToString("N2")));
                    sb.AppendLine(string.Format("Najmniej goli zdobytych/mecz: {0} ({1})",
                        partnerGames.OrderBy(p => p.AvgGoalsScored).First().Name,
                        partnerGames.OrderBy(p => p.AvgGoalsScored).First().AvgGoalsScored.ToString("N2")));
                    sb.AppendLine(string.Format("Najwięcej goli straconych/mecz: {0} ({1})",
                        partnerGames.OrderByDescending(p => p.AvgGoalsLost).First().Name,
                        partnerGames.OrderByDescending(p => p.AvgGoalsLost).First().AvgGoalsLost.ToString("N2")));
                    sb.AppendLine(string.Format("Najmniej goli straconych/mecz: {0} ({1})",
                        partnerGames.OrderBy(p => p.AvgGoalsLost).First().Name,
                        partnerGames.OrderBy(p => p.AvgGoalsLost).First().AvgGoalsLost.ToString("N2")));
                }

                sb = sb.Replace(Environment.NewLine, "<br/>");

                pnlPartnerStats.Controls.Add(new LiteralControl(sb.ToString()));
            }
        }

        private void BindRivalsStats()
        {
            pnlRivals.Controls.Clear();
            Player player = ((Player)Session["UserBasicStatsPlayer"]);
            if (player == null) return;

            using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
            {
                List<TeamMatchPlayer> rivalMatches = context.TeamMatchPlayers.Where(p =>
                    !p.TeamMatch.TeamMatchPlayers.Any(x => x.PlayerId == player.Id)
                    && p.TeamMatch.Match.TeamMatches.Any(x => x.TeamMatchPlayers.Any(y => y.PlayerId == player.Id)))
                    .OrderByDescending(p => p.Timestamp).ToList();
                StringBuilder sb = new StringBuilder();

                if (rivalMatches.Any())
                {
                    //TeamMatchPlayer playerData = lastGames.First().TeamMatchPlayers.First(p => p.PlayerId == player.Id);

                    var rivalGames = rivalMatches
                        .GroupBy(p => new { Partner = p.Player })
                        .Select(x => new
                        {
                            //Partner = x.Key.Partner,
                            Name = PlayerHelper.GetPlayerNameLink(x.Key.Partner),
                            Games = x.Count(),
                            Won = x.Count(z => z.TeamMatch.Result == (byte)Common.EResult.Win),
                            Drawn = x.Count(z => z.TeamMatch.Result == (byte)Common.EResult.Draw),
                            Lost = x.Count(z => z.TeamMatch.Result == (byte)Common.EResult.Loose),
                            Aggregate = x.Count(z => z.TeamMatch.Result == (byte)Common.EResult.Win) - x.Count(z => z.TeamMatch.Result == (byte)Common.EResult.Loose),
                            AvgAggregatePerc =
                                100 * (x.Count(z => z.TeamMatch.Result == (byte)Common.EResult.Win))
                                / (x.Count() == 0 ? 1 : x.Count()),
                            AvgGoalsScored = x.Average(z => z.TeamMatch.GoalsScored),
                            AvgGoalsLost = x.Average(z => z.TeamMatch.GoalsLost)
                        })
                        .OrderByDescending(p => p.AvgAggregatePerc);
                    gvRivals.DataSource = rivalGames;
                    gvRivals.DataBind();
                    sb.AppendLine(string.Format("Najczęstrzy rywal: {0} ({1})",
                        rivalGames.OrderByDescending(p => p.Games).First().Name,
                        rivalGames.OrderByDescending(p => p.Games).First().Games));
                    sb.AppendLine(string.Format("Najlepszy bilans: {0} ({1})",
                        rivalGames.OrderByDescending(p => p.Aggregate).First().Name,
                        rivalGames.OrderByDescending(p => p.Aggregate).First().Aggregate < 0
                        ? (rivalGames.OrderByDescending(p => p.Aggregate).First().Aggregate).ToString()
                        : ("+" + rivalGames.OrderByDescending(p => p.Aggregate).First().Aggregate)));
                    sb.AppendLine(string.Format("Najgorszy bilans: {0} ({1})",
                        rivalGames.OrderBy(p => p.Aggregate).First().Name,
                        rivalGames.OrderBy(p => p.Aggregate).First().Aggregate < 0
                        ? (rivalGames.OrderBy(p => p.Aggregate).First().Aggregate).ToString()
                        : ("+" + rivalGames.OrderBy(p => p.Aggregate).First().Aggregate)));
                    sb.AppendLine(string.Format("Największy % wygranych: {0} ({1}%)",
                        rivalGames.OrderByDescending(p => p.AvgAggregatePerc).First().Name,
                        rivalGames.OrderByDescending(p => p.AvgAggregatePerc).First().AvgAggregatePerc));
                    sb.AppendLine(string.Format("Najmniejszy % wygranych: {0} ({1}%)",
                        rivalGames.OrderBy(p => p.AvgAggregatePerc).First().Name,
                        rivalGames.OrderBy(p => p.AvgAggregatePerc).First().AvgAggregatePerc));
                    sb.AppendLine(string.Format("Najwięcej goli zdobytych/mecz: {0} ({1})",
                        rivalGames.OrderByDescending(p => p.AvgGoalsScored).First().Name,
                        rivalGames.OrderByDescending(p => p.AvgGoalsScored).First().AvgGoalsScored.ToString("N2")));
                    sb.AppendLine(string.Format("Najmniej goli zdobytych/mecz: {0} ({1})",
                        rivalGames.OrderBy(p => p.AvgGoalsScored).First().Name,
                        rivalGames.OrderBy(p => p.AvgGoalsScored).First().AvgGoalsScored.ToString("N2")));
                    sb.AppendLine(string.Format("Najwięcej goli straconych/mecz: {0} ({1})",
                        rivalGames.OrderByDescending(p => p.AvgGoalsLost).First().Name,
                        rivalGames.OrderByDescending(p => p.AvgGoalsLost).First().AvgGoalsLost.ToString("N2")));
                    sb.AppendLine(string.Format("Najmniej goli straconych/mecz: {0} ({1})",
                        rivalGames.OrderBy(p => p.AvgGoalsLost).First().Name,
                        rivalGames.OrderBy(p => p.AvgGoalsLost).First().AvgGoalsLost.ToString("N2")));
                }

                sb = sb.Replace(Environment.NewLine, "<br/>");

                pnlRivals.Controls.Add(new LiteralControl(sb.ToString()));
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

        protected void gvStats_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    e.Row.Cells[0].Text = HttpUtility.HtmlDecode(e.Row.Cells[0].Text);
                    e.Row.Cells[6].Text += "%";
                    e.Row.Cells[7].Text = decimal.Parse(e.Row.Cells[7].Text).ToString("n2");
                    e.Row.Cells[8].Text = decimal.Parse(e.Row.Cells[8].Text).ToString("n2");
                }
                catch { }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Gracz";
                e.Row.Cells[1].Text = "Gier";
                e.Row.Cells[2].Text = "W";
                e.Row.Cells[3].Text = "R";
                e.Row.Cells[4].Text = "P";
                e.Row.Cells[5].Text = "Bilans";
                e.Row.Cells[6].Text = "Śr. W";
                e.Row.Cells[7].Text = "Śr. br. zd";
                e.Row.Cells[8].Text = "Śr. br. st";

                e.Row.Cells[0].ToolTip = "Gracz";
                e.Row.Cells[1].ToolTip = "Gier";
                e.Row.Cells[2].ToolTip = "Ilość wygranych meczy";
                e.Row.Cells[3].ToolTip = "Ilość zremisowanych meczy";
                e.Row.Cells[4].ToolTip = "Ilość przegranych meczy";
                e.Row.Cells[5].ToolTip = "Bilans wygranych i przegranych";
                e.Row.Cells[6].ToolTip = "Średnia wygranych meczy w %";
                e.Row.Cells[7].ToolTip = "Średnia bramek zdobytych w meczu";
                e.Row.Cells[8].ToolTip = "Średnia bramek straconych w meczu";
            }
        }
    }
}