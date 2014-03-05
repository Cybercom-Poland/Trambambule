<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="GrantedAchievements.aspx.cs" Inherits="Trambambule.GrantedAchievements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%=Page.ResolveUrl("~/Assets/jquery-1.10.2.min.js")%>" type="text/javascript"></script>
    <script src="<%=Page.ResolveUrl("~/Assets/jquery.cookie.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var accordionTemp = 0;
        var achievementsCookieName = 'TrambambuleAchievementsAccordion';
        jQuery(document).ready(
        function () {
            if (jQuery.cookie(achievementsCookieName) != null) {
                accordionTemp = jQuery.cookie(achievementsCookieName);
            }
        });

        function accordionShow(me) {
            var a = jQuery(me);
            var dataId = a.data('id');
            jQuery('.accordionElement').each(
            function () {
                if (jQuery(this).css('display') != 'none') {
                    jQuery(this).slideUp('slow');
                }
            });
            if (accordionTemp != dataId) {
                a.siblings('.accordionElement').slideDown('slow', function () {
                    window.scrollTo(0, a.position().top);
                });
                jQuery.cookie(achievementsCookieName, dataId, { expires: 1, path: '/' });
                accordionTemp = dataId;                
            }
            else {
                jQuery.cookie(achievementsCookieName, '0', { expires: 1, path: '/' });
                accordionTemp = 0;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        Zdobyte osiągnięcia</h1>
    <asp:Repeater ID="repAchievements" runat="server" OnItemDataBound="repAchievements_ItemBound">
        <HeaderTemplate>
            <table class="Table" style="border-collapse: collapse;">
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <a class="center" href="javascript:void(null);" onclick="accordionShow(this);" data-id="<%#Eval("Achievement.Id")%>">
                        <h3>
                            <%#Eval("Achievement.Title")%></h3>
                    </a>
                    <div class="accordionElement" <%#GetHiddenAttribute((int)Eval("Achievement.Id")) %>>
                        <div class="left">
                            <div>
                                <b>Poziomy: </b>
                            </div>
                            <br />
                            <div class="lvl">
                                <img src='<%#Eval("Achievement.Level1Icon")%>' alt="Poziom <%#Trambambule.Common.GetAchievementsLevelName(1)%> - <%#Eval("Achievement.Level1Label")%>" title="Poziom <%#Trambambule.Common.GetAchievementsLevelName(1)%> - <%#Eval("Achievement.Level1Label")%>" height="45px" width="30px" />
                                <span>Poziom <%#Trambambule.Common.GetAchievementsLevelName(1)%>: 
                                    <%#Eval("Achievement.Level1Label")%>
                                </span>
                            </div>
                            <div class="lvl">
                                <img src='<%#Eval("Achievement.Level2Icon")%>' alt="Poziom <%#Trambambule.Common.GetAchievementsLevelName(2)%>: <%#Eval("Achievement.Level2Label")%>" title="Poziom <%#Trambambule.Common.GetAchievementsLevelName(2)%>: <%#Eval("Achievement.Level2Label")%>" height="45px" width="30px" />
                                <span>Poziom <%#Trambambule.Common.GetAchievementsLevelName(2)%>: 
                                    <%#Eval("Achievement.Level2Label")%>
                                </span>
                            </div>
                            <div class="lvl">
                                <img src='<%#Eval("Achievement.Level3Icon")%>' alt="Poziom<%#Trambambule.Common.GetAchievementsLevelName(3)%>: <%#Eval("Achievement.Level3Label")%>" title="Poziom <%#Trambambule.Common.GetAchievementsLevelName(3)%>: <%#Eval("Achievement.Level3Label")%>" height="45px" width="30px" />
                                   <span>Poziom <%#Trambambule.Common.GetAchievementsLevelName(3)%>: 
                                    <%#Eval("Achievement.Level3Label")%>
                                </span>
                            </div>
                        </div>
                        <div class="right">
                            <asp:PlaceHolder ID="phPlayers" runat="server">
                                <asp:Repeater ID="repPlayers" runat="server">
                                    <ItemTemplate>
                                        <div class="lvl withPadding<%#GetCustomClass((int)Eval("Player.Id"))%>">
                                            <img src="<%#GetIcon((int)Eval("LevelOfAchievement"))%>" alt="Poziom <%#Trambambule.Common.GetAchievementsLevelName((int)Eval("LevelOfAchievement")) %>: <%#GetLabel((int)Eval("LevelOfAchievement"))%>"
                                                height="45px" width="30px" title="Poziom <%#Trambambule.Common.GetAchievementsLevelName((int)Eval("LevelOfAchievement")) %>: <%#GetLabel((int)Eval("LevelOfAchievement"))%>" />
                                            <a href="<%=Request.RawUrl.IndexOf("?") > 0 ? Request.RawUrl.Substring(0,Request.RawUrl.IndexOf("?")) : Request.RawUrl%>?userId=<%#Eval("Player.Id")%>">
                                                <%#Eval("Player.FirstName") + " " + Eval("Player.LastName")%></a> (<%#GetTextWithAnchors(Eval("ToNextLevelLabel").ToString())%>)
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phNoResults" runat="server">
                                <div class="withPadding">
                                    <span>To osiągnięcie nie zostało jeszcze zdobyte przez jakiegokolwiek gracza.</span></div>
                            </asp:PlaceHolder>
                        </div>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody></table></FooterTemplate>
    </asp:Repeater>
</asp:Content>
