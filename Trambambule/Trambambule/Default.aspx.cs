using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI.HtmlControls;
using Trambambule.Achievements.Helpers;

namespace Trambambule
{
    public partial class Default : System.Web.UI.Page
    {
        List<PlayersAchievements> playersAchievements = null;
        protected void Page_Load(object sender, EventArgs e)
        {
        //    Page.LoadComplete += new EventHandler(Page_LoadComplete);

            if (Request.UrlReferrer != null && string.Compare(Request.UrlReferrer.LocalPath.ToLower(), Request.Url.LocalPath, true) != 0)
            {
                Session["defaultSortExpression"] = null;
                Session["defaultSortDirection"] = null;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            object oSortExpression = Session["defaultSortExpression"];
            object oSortDirection = Session["defaultSortDirection"];

            if (oSortExpression != null)
            {
                gvPlayerResults.Sort(oSortExpression.ToString(),(SortDirection)Enum.Parse(typeof(SortDirection), oSortDirection.ToString()));
            }
           // gvPlayerResults.DataBind();
        }

        protected void gvPlayerResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            string argument = Request["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(argument) && argument.ToLower().Contains("sort") && Session["defaultSortExpression"] == null && Session["defaultSortDirection"] == null)
            {
                Session["defaultSortDirection"] = SortDirection.Descending.ToString();
                Session["defaultSortExpression"] = e.SortExpression;
            }
            else
            {
                Session["defaultSortDirection"] = e.SortDirection;
                Session["defaultSortExpression"] = e.SortExpression;
            }
        }

        protected void gvPlayerResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Player p = DataAccess.GetPlayer(Server.HtmlDecode(e.Row.Cells[2].Text));
                if(p != null)
                    e.Row.Cells[2].Text = Server.HtmlDecode(PlayerHelper.GetPlayerNameLink(p));

                if (playersAchievements == null)
                    playersAchievements = DataAccess.GetPlayersWithAchievements();

                PlayersAchievements pa = playersAchievements.FirstOrDefault(w => w.Player.Id == p.Id);
                if (pa != null)
                {
                    List<AchievementWithCurrentLevel> achievements = pa.Achievements;
                    if (achievements.Any())
                    {
                        StringBuilder sb = new StringBuilder("<div class=\"achievementsIcons\">");
                        for (int i = 0; i < pa.Achievements.Count; i++)
                        {
                            if (i > 0 && i % 4 == 0)
                                sb.Append("</div><div class=\"achievementsIcons\">");
                            AchievementWithCurrentLevel awcl = pa.Achievements[i];
                            string src = "";
                            string label = "Poziom {0}: {1}";
                            switch (awcl.Level)
                            {
                                case 1:
                                    src = awcl.Achievement.Level1Icon;
                                    label = string.Format(label,Common.GetAchievementsLevelName(1), awcl.Achievement.Level1Label);
                                    break;
                                case 2:
                                    src = awcl.Achievement.Level2Icon;
                                    label = string.Format(label, Common.GetAchievementsLevelName(2), awcl.Achievement.Level2Label);
                                    break;
                                case 3:
                                    src = awcl.Achievement.Level3Icon;
                                    label = string.Format(label, Common.GetAchievementsLevelName(3), awcl.Achievement.Level3Label);
                                    break;
                            }
                            sb.AppendFormat("<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" height=\"19px\" width=\"19px\" />", src, label);
                        }

                        if (!sb.ToString().EndsWith("</div>"))
                            sb.Append("</div>");

                        e.Row.Cells[9].Text = sb.ToString();
                    }
                    else
                    {
                        e.Row.Cells[9].Text = "Brak";
                    }
                }
                else
                {
                    e.Row.Cells[9].Text = "Brak";
                }
                
                if(Session["UserBasicStatsPlayer"] != null)
                {
                    Player player = (Player)Session["UserBasicStatsPlayer"];
                    if (e.Row.Cells[0].Text == player.Id.ToString())
                        e.Row.Font.Bold = true;
                }

                int balance = -1;

                if (int.TryParse(e.Row.Cells[6].Text, out balance))
                {
                    if (balance > 0)
                        e.Row.Cells[6].Text = "+" + balance.ToString();
                }


                int pointsChange = -1;

                if (int.TryParse(e.Row.Cells[8].Text, out pointsChange))
                {
                    if (pointsChange > 0)
                        e.Row.Cells[8].Text = "+" + pointsChange.ToString();
                }
            }
        }

        protected void gvPlayerResults_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    if (tc.HasControls())
                    {
                        LinkButton lnk = (LinkButton)tc.Controls[0];
                        if (lnk != null)
                        {
                            lnk.CssClass = "sortingHeader";
                            lnk.ToolTip = "Posortuj po polu: " + lnk.Text;
                            HtmlGenericControl span = new HtmlGenericControl("span");
                            span.InnerText = lnk.Text;
                            lnk.Controls.Add(span);

                            HtmlImage img = new HtmlImage();

                            object oSortDirection = Session["defaultSortDirection"];
                            object oSortExpression = Session["defaultSortExpression"];

                            if (oSortExpression == null)
                            {
                                if (lnk.Text == "#")                                
                                    img.Src = Page.ResolveUrl("~/Assets/arrow_asc.png");                                
                                else
                                    img.Src = Page.ResolveUrl("~/Assets/arrow_Sorting.png");                                
                            }
                            else
                            {
                                string sortExpression = oSortExpression.ToString();
                                string sortDirection = oSortDirection.ToString();
                                if (string.Compare(sortExpression, lnk.Text) == 0)
                                {
                                    if (sortDirection == SortDirection.Ascending.ToString())
                                        img.Src = Page.ResolveUrl("~/Assets/arrow_asc.png");
                                    else
                                        img.Src = Page.ResolveUrl("~/Assets/arrow_desc.png");
                                }
                                else
                                    img.Src = Page.ResolveUrl("~/Assets/arrow_Sorting.png");                               
                            }

                            img.Alt = "Posortuj po polu: " + lnk.Text;
                            img.Width = 14;
                            img.Height = 14;
                            lnk.Controls.Add(img);

                        }
                    }
                }
            }
        }
    }
}