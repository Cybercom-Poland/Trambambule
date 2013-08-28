<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="Trambambule.UserControls.Header" %>

<asp:Image runat="server" ImageUrl="~/Assets/cybercom_logo.png" AlternateText="Cybercom Poland" CssClass="headerLogo" />
<div class="menu">
    <ul>
        <li class="link">            
            <asp:HyperLink ID="hlSendResult" runat="server" NavigateUrl="~/SendResult.aspx" Text="Zgłoś wynik meczu"></asp:HyperLink>
        </li>
        <li class="separator">
        </li>
        <li class="link">            
            <asp:HyperLink ID="hlResults" runat="server" NavigateUrl="~/Default.aspx" Text="Ranking graczy"></asp:HyperLink>
        </li>
    </ul>

    <span class="AppName">
        Trambambule
    </span>
</div>
