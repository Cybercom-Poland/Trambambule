<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserBasicStats.ascx.cs" Inherits="Trambambule.UserControls.UserBasicStats" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

    <asp:Panel ID="pnlUserStats" runat="server" CssClass="SideBarPanel">
        <asp:TextBox ID="tbxPlayers" runat="server" MaxLength="100" AutoPostBack="true" OnTextChanged="tbxPlayers_TextChanged"
            CssClass="TopSelector">                
        </asp:TextBox>
        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="tbxPlayers"
            CompletionInterval="2" MinimumPrefixLength="2" ServiceMethod="GetPlayerNames" UseContextKey="true" 
            ServicePath="~/SendResult.aspx" CompletionListCssClass="AutoComplete" CompletionListItemCssClass="AutoCompleteItem"
            CompletionListHighlightedItemCssClass="AutoCompleteItemHighlighted">
        </asp:AutoCompleteExtender>
        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="tbxPlayers" 
            WatermarkText="Wybierz gracza">
        </asp:TextBoxWatermarkExtender>

        <div class="content">
            <asp:Label ID="lblStatsDetails" runat="server"></asp:Label>
        </div>
    </asp:Panel>