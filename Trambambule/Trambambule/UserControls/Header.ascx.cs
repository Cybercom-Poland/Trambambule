using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Trambambule.UserControls
{
    public partial class Header : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rawUrl = Request.RawUrl.ToLower();
            if (rawUrl.Contains("?"))
                rawUrl = rawUrl.Substring(0, rawUrl.IndexOf("?"));

            
            if (rawUrl.Contains("/sendresult.aspx"))
            {
                hplSendResult.CssClass += "bold";
            }            
            else if (rawUrl.Contains("/lastgames.aspx"))
            {
                hplLastGames.CssClass += "bold";
            }
            else if (rawUrl.Contains("/userstats.aspx"))
            {
                hplUserStats.CssClass += "bold";
            }
            else if (rawUrl.Contains("/grantedachievements.aspx"))
            {
                hplGrantedAchievements.CssClass += "bold";
            }
            else if (rawUrl.Contains("/playerslist.aspx"))
            {
                hplPlayers.CssClass += "bold";
            }
            else if (rawUrl.Contains("/help.aspx"))
            {
                hplHelp.CssClass += "bold";
            }
            else
            {
                hplDefault.CssClass += "bold";
            }
        }
    }
}