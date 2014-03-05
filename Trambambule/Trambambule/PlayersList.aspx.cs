using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Trambambule.Achievements.Helpers;
using Trambambule.Achievements;

namespace Trambambule
{
    public partial class PlayersList : System.Web.UI.Page
    {
        protected string Hidden = "";
        protected string ShowPanelText = "";
        private int selectedUserId = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["TrambambuleAddNewPlayer"] == null || Request.Cookies["TrambambuleAddNewPlayer"].Value == "0")
            {
                Hidden = "style=\"display:none;\"";
                ShowPanelText = "Pokaż panel dodawania nowego gracza";
            }
            else
            {
                ShowPanelText = "Ukryj panel dodawania nowego gracza";
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session["UserBasicStatsPlayer"] == null)
            {
                if(Request.QueryString["userId"] != null && Request.QueryString["userId"] != "")
                    int.TryParse(Request.QueryString["userId"], out selectedUserId);
            } 
            else
            {
                selectedUserId = ((Player)Session["UserBasicStatsPlayer"]).Id;
            }

            repPlayers.DataSource = DataAccess.GetPlayers();
            repPlayers.DataBind();
        }

        protected string GetCustomClass(int id)
        {
            if (selectedUserId == id)
                return " bold";
            else
                return "";
        }

        protected string GetLocation(int cityIndex)
        {
            if (cityIndex == 0)
                return "Warszawa";
            else if (cityIndex == 1)
                return "Łódź";
            else
                return "";
        }

        protected void btnAddNewPlayer_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string firstName = txtNewPlayerFirstName.Text.Trim();
                string lastName = txtNewPlayerLastName.Text.Trim();
                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && firstName.Length > 0 && firstName.Length <= 100 && lastName.Length > 0 && lastName.Length <= 100)
                {
                    int location = int.Parse(ddlLocation.SelectedValue);
                    string nick = (firstName.Length < 2 ? "Jakiś" : firstName.Substring(0, 2)) + (lastName.Length < 2 ? "Pseudonim" : lastName.Substring(0, 2));
                    using (TrambambuleDBContextDataContext context = new TrambambuleDBContextDataContext())
                    {
                        try
                        {
                            Player p = new Player();
                            p.FirstName = firstName;
                            p.LastName = lastName;
                            p.Location = location;
                            p.Nickname = nick;
                            p.Timestamp = DateTime.Now;

                            context.Players.InsertOnSubmit(p);
                            context.SubmitChanges();

                            Cache.Remove(DataAccess.PlayersListCacheName);
                            Cache.Remove(DataAccess.PlayersNamesAndSurnamesWithoutPolishCharsCacheName);
                            Cache.Remove(DataAccess.PlayersAchievementsCacheName);
                            Cache.Remove(DataAccess.AllAchievementsCacheName);
                            Cache.Remove(DataAccess.OverallStatsCacheName);

                            new AchievementsService(new List<Player>() { p }).RecalculateAchievements(true);

                            litStatus.Text = "<div class=\"validator\">Użytkownik został dodany</div>";
                            txtNewPlayerFirstName.Text = "";
                            txtNewPlayerLastName.Text = "";
                            ddlLocation.SelectedIndex = 0;
                        }
                        catch { litStatus.Text = "<div class=\"validator\">Wystąpił błąd podczas dodawania nowego użytkownika</div>"; }
                    }
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtNewPlayerFirstName.Text = "";
            txtNewPlayerLastName.Text = "";
            litStatus.Text = "";
            ddlLocation.SelectedIndex = 0;
        }

        protected void cusValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string firstName = txtNewPlayerFirstName.Text.Trim();
            string lastName = txtNewPlayerLastName.Text.Trim();
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && firstName.Length > 0 && firstName.Length <= 100 && lastName.Length > 0 && lastName.Length <= 100)
            {
                int location = int.Parse(ddlLocation.SelectedValue);
                string data = Common.CreateTextCompareString(firstName) + " " + Common.CreateTextCompareString(lastName);               
                if (DataAccess.GetPlayersNamesAndSurnamesWithoutPolishChars().FirstOrDefault(w => w.NameAndLastName == data && w.LocationId == location) != null)
                {
                    args.IsValid = false;
                    cusValidator.ErrorMessage = string.Format("W bazie danych jest już użytkownik {0} {1} w mieście {2}", firstName, lastName, ddlLocation.SelectedItem.Text);
                }
                else
                {
                    args.IsValid = true;
                }
            }
            else
            {
                args.IsValid = true;
            }
        }
    }
}