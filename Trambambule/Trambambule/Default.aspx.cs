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
            Page.LoadComplete += new EventHandler(Page_LoadComplete);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            gvPlayerResults.DataBind();
        }
        
        protected void gvPlayerResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Player p = DataAccess.GetPlayer(Server.HtmlDecode(e.Row.Cells[2].Text));
                if(p != null)
                    e.Row.Cells[2].Text = Server.HtmlDecode(PlayerHelper.GetPlayerNameLink(p));
                if(Session["UserBasicStatsPlayer"] != null)
                {
                    Player player = (Player)Session["UserBasicStatsPlayer"];
                    if (e.Row.Cells[0].Text == player.Id.ToString())
                        e.Row.Font.Bold = true;
                }
            }
        }
    }
}