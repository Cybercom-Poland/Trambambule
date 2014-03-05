<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="Trambambule.UserControls.Header" %>
<img src="<%=Page.ResolveUrl("~/Assets/cybercom_logo.png") %>" class="headerLogo" alt="Cybercom Poland" />
<%--<asp:Image runat="server" ImageUrl="~/Assets/cybercom_logo.png" AlternateText="Cybercom Poland" CssClass="headerLogo" />--%>
<div class="menu">
    <ul>
        <%--        <li class="link"><a href="/SendResult.aspx">Zgłoś wynik meczu</a> </li>
        <li class="link"><a href="/Default.aspx">Ranking</a> </li>
        <li class="link"><a href="/LastGames.aspx">Ostatnie mecze</a> </li>
        <li class="link"><a href="/UserStats.aspx">Statystyki</a> </li>
        <li class="link last"><a href="/GrantedAchievements.aspx">Osiągnięcia</a> </li>
        --%>
        <li class="link">
            <asp:HyperLink ID="hplSendResult" runat="server" NavigateUrl="~/SendResult.aspx" Text="Zgłoś wynik meczu"></asp:HyperLink>
        </li>
        <li class="link">
            <asp:HyperLink ID="hplDefault" runat="server" NavigateUrl="~/Default.aspx" Text="Ranking"></asp:HyperLink>
        </li>
        <li class="link">
            <asp:HyperLink ID="hplLastGames" runat="server" NavigateUrl="~/LastGames.aspx" Text="Ostatnie mecze"></asp:HyperLink>
        </li>
        <li class="link">
            <asp:HyperLink ID="hplUserStats" runat="server" NavigateUrl="~/UserStats.aspx" Text="Statystyki"></asp:HyperLink>
        </li>
         <li class="link">
            <asp:HyperLink ID="hplGrantedAchievements" runat="server" NavigateUrl="~/GrantedAchievements.aspx"
                Text="Osiągnięcia"></asp:HyperLink>
        </li>
        <li class="link">
            <asp:HyperLink ID="hplPlayers" runat="server" NavigateUrl="~/PlayersList.aspx" Text="Gracze"></asp:HyperLink>
        </li>
        <li class="lin last">
            <asp:HyperLink ID="hplHelp" runat="server" NavigateUrl="~/Help.aspx" Text="Pomoc"></asp:HyperLink>
        </li>
       
       <%-- <li class="icon"><a href="/PlayersList.aspx" class="add" title="Lista graczy i dodawanie nowego gracza">
        </a></li>
        <li class="icon"><a href="/Help.aspx" class="help" title="Pomoc"></a></li>--%>
    </ul>
    <span class="AppName">Trambambule</span>
</div>
