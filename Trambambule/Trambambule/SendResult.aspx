<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="SendResult.aspx.cs" Inherits="Trambambule.SendResult" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%=Page.ResolveUrl("~/Assets/jquery-1.10.2.min.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        function showLoader(id) {
            if (Page_ClientValidate("vgsr")) {
                var table = jQuery('.LeftPanel');
                var loader = jQuery('#loader');
                var img = loader.children('img');

                var width = table.width();
                var height = table.height();
                var left = table.position().left;
                var top = table.position().top;
                var topImg = (height / 2 - 27) + img.position().top;
                loader.css('top', top + 'px').css('left', left + 'px').css('width', width + 'px').css('height', height + 'px').css('display', 'block');
                img.css('top', topImg + 'px');
            }
        }

        function checkFocus() {
            var focused = false;
            jQuery('.actxt').each(
                function () {
                    if (!focused) {
                        if (jQuery(this).val() == '') {
                            jQuery(this).focus();
                            focused = true;
                        }
                    }
                });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        Zgłoś wynik meczu</h1>
    <asp:Panel ID="pnlSendResult" runat="server" CssClass="ContentPanel" DefaultButton="btnSubmit">
        <div id="loader" style="display:none;">
            <img src="<%=Page.ResolveUrl("~/Assets/ajax-loader.gif")%>" alt="loading" />
        </div>
        <table class="SendResults">

            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <h2>
                        Drużyna 1
                    </h2>
                </td>
                <td>
                    <h2>
                        Drużyna 2
                    </h2>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <h3>
                        Atak
                    </h3>
                </td>
            </tr>
            <tr>
                <td class="ratingtd">
                    <asp:Literal ID="litTbxPlayer1OffStats" runat="server"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="tbxPlayer1Off" runat="server" Columns="35" OnTextChanged="tbxPlayer_TextChanged"
                        AutoPostBack="true" CssClass="actxt"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="tbxPlayer1Off"
                        ValidationGroup="vgsr" Display="None" ErrorMessage="Pole wymagane"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfv1">
                    </asp:ValidatorCalloutExtender>
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="tbxPlayer1Off"
                        CompletionInterval="2" MinimumPrefixLength="2" ServiceMethod="GetPlayerNames"
                        UseContextKey="true" ServicePath="" CompletionListCssClass="AutoComplete" CompletionListItemCssClass="AutoCompleteItem"
                        CompletionListHighlightedItemCssClass="AutoCompleteItemHighlighted">
                    </asp:AutoCompleteExtender>
                </td>
                <td>
                    <asp:TextBox ID="tbxPlayer2Off" runat="server" Columns="35" OnTextChanged="tbxPlayer_TextChanged"
                        AutoPostBack="true" CssClass="actxt"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbxPlayer2Off"
                        ValidationGroup="vgsr" Display="None" ErrorMessage="Pole wymagane"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator1">
                    </asp:ValidatorCalloutExtender>
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="tbxPlayer2Off"
                        CompletionInterval="2" MinimumPrefixLength="2" ServiceMethod="GetPlayerNames"
                        UseContextKey="true" ServicePath="" CompletionListCssClass="AutoComplete" CompletionListItemCssClass="AutoCompleteItem"
                        CompletionListHighlightedItemCssClass="AutoCompleteItemHighlighted">
                    </asp:AutoCompleteExtender>
                </td>
                <td class="ratingtd">
                    <asp:Literal ID="litTbxPlayer2OffStats" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="2">
                    <h3>
                        <asp:ImageButton ImageUrl="~/Assets/exchange.png" OnClick="ibtn1_Click" runat="server"
                            ID="ibtn1" Style="float: left; height: 20px;" />
                        Obrona
                        <asp:ImageButton ImageUrl="~/Assets/exchange.png" OnClick="ibtn2_Click" runat="server"
                            ID="ibtn2" Style="float: right; height: 20px;" />
                    </h3>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="ratingtd">
                    <asp:Literal ID="litTbxPlayer1DeffStats" runat="server"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="tbxPlayer1Deff" runat="server" Columns="35" OnTextChanged="tbxPlayer_TextChanged"
                        AutoPostBack="true" CssClass="actxt"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbxPlayer1Deff"
                        ValidationGroup="vgsr" Display="None" ErrorMessage="Pole wymagane"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator2">
                    </asp:ValidatorCalloutExtender>
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" TargetControlID="tbxPlayer1Deff"
                        CompletionInterval="2" MinimumPrefixLength="2" ServiceMethod="GetPlayerNames"
                        UseContextKey="true" ServicePath="" CompletionListCssClass="AutoComplete" CompletionListItemCssClass="AutoCompleteItem"
                        CompletionListHighlightedItemCssClass="AutoCompleteItemHighlighted">
                    </asp:AutoCompleteExtender>
                </td>
                <td>
                    <asp:TextBox ID="tbxPlayer2Deff" runat="server" Columns="35" OnTextChanged="tbxPlayer_TextChanged"
                        AutoPostBack="true" CssClass="actxt"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbxPlayer2Deff"
                        ValidationGroup="vgsr" Display="None" ErrorMessage="Pole wymagane"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFieldValidator3">
                    </asp:ValidatorCalloutExtender>
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" TargetControlID="tbxPlayer2Deff"
                        CompletionInterval="2" MinimumPrefixLength="2" ServiceMethod="GetPlayerNames"
                        UseContextKey="true" ServicePath="" CompletionListCssClass="AutoComplete" CompletionListItemCssClass="AutoCompleteItem"
                        CompletionListHighlightedItemCssClass="AutoCompleteItemHighlighted">
                    </asp:AutoCompleteExtender>
                </td>
                <td class="ratingtd">
                    <asp:Literal ID="litTbxPlayer2DeffStats" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <h3>
                        Wynik
                    </h3>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:TextBox ID="tbxScoreA" runat="server" MaxLength="2" Columns="2" CssClass="actxt"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbxScoreA"
                        FilterMode="ValidChars" FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbxScoreA"
                        ValidationGroup="vgsr" Display="None" ErrorMessage="Pole wymagane"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" TargetControlID="RequiredFieldValidator4">
                    </asp:ValidatorCalloutExtender>
                    :
                    <asp:TextBox ID="tbxScoreB" runat="server" MaxLength="2" Columns="2" CssClass="actxt"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbxScoreB"
                        FilterMode="ValidChars" FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbxScoreB"
                        ValidationGroup="vgsr" Display="None" ErrorMessage="Pole wymagane"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" TargetControlID="RequiredFieldValidator5">
                    </asp:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>    
                <td colspan="4">
                    <asp:Button ID="btnSubmit" runat="server" Text="Zgłoś wynik" OnClientClick="showLoader();" OnClick="btnSubmit_Click"
                        CausesValidation="true" Style="margin-top: 5px;" ValidationGroup="vgsr" onFocus="checkFocus();" />
                    &nbsp;<asp:Button ID="btnClear" runat="server" Text="Wyczyść" OnClick="btnClear_Click"
                        CausesValidation="false" />
                    <asp:Panel ID="pnlInfo" runat="server" CssClass="InfoPanel">
                    </asp:Panel>
                    <asp:Literal ID="litAchievements" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
