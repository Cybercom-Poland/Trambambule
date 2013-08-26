<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Trambambule.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HyperLink ID="hlSendResult" runat="server" NavigateUrl="~/SendResult.aspx" Text="Zgłoś wynik meczu"></asp:HyperLink>

    <asp:GridView ID="gvPlayerResults" runat="server" DataSourceID="sqldsResults" Visible="false">        
    </asp:GridView>
    <asp:SqlDataSource ID="sqldsResults" runat="server" ConnectionString="<%$ ConnectionStrings:TrambambuleConnectionString %>"
        SelectCommand="GetUserResults" SelectCommandType="StoredProcedure">        
    </asp:SqlDataSource>

    <asp:Panel ID="pnlUserStats" runat="server" CssClass="SendResults">
        <asp:DropDownList ID="ddlPlayers" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPlayers_SelectedIndexChanged"
            DataTextField="Name" DataValueField="Id">
        </asp:DropDownList>
        <asp:Panel ID="pnlUserStatsDetails" runat="server"></asp:Panel>
    </asp:Panel>

</asp:Content>
