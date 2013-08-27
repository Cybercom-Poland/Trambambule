<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserBasicStats.ascx.cs" Inherits="Trambambule.UserControls.UserBasicStats" %>

<asp:UpdatePanel ID="upanUserStats" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlUserStats" runat="server" CssClass="SideBarPanel">
            <asp:DropDownList ID="ddlPlayers" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPlayers_SelectedIndexChanged"
                CssClass="TopSelector" DataTextField="Name" DataValueField="Id">
            </asp:DropDownList>
            <div class="content">
                <asp:Label ID="lblStatsDetails" runat="server"></asp:Label>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>