<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="SendResult.aspx.cs" Inherits="Trambambule.SendResult" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upanSendResults" runat="server">
        <ContentTemplate>
                        
            <h1>Zgłoś wynik meczu</h1>

            <asp:Panel ID="pnlSendResult" runat="server" CssClass="ContentPanel" DefaultButton="btnSubmit">
                <table class="SendResults">
                    <tr>
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
                    </tr>
                    <tr>
                        <td colspan="2">
                            <h3>
                                Atak
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="tbxPlayer1Off" runat="server" Columns="40" OnTextChanged="tbxPlayer_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="tbxPlayer1Off" ValidationGroup="vgsr"
                                Display="None" ErrorMessage="Pole wymagane"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfv1">
                            </asp:ValidatorCalloutExtender>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="tbxPlayer1Off"
                                CompletionInterval="3" MinimumPrefixLength="3" ServiceMethod="GetPlayerNames" UseContextKey="true" ServicePath="">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxPlayer2Off" runat="server" Columns="40" OnTextChanged="tbxPlayer_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbxPlayer2Off" ValidationGroup="vgsr"
                                Display="None" ErrorMessage="Pole wymagane"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator1">
                            </asp:ValidatorCalloutExtender>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="tbxPlayer2Off"
                                CompletionInterval="2" MinimumPrefixLength="2" ServiceMethod="GetPlayerNames" UseContextKey="true" ServicePath="">
                            </asp:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <h3>
                                Obrona
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="tbxPlayer1Deff" runat="server" Columns="40" OnTextChanged="tbxPlayer_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbxPlayer1Deff" ValidationGroup="vgsr"
                                Display="None" ErrorMessage="Pole wymagane"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator2">
                            </asp:ValidatorCalloutExtender>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" TargetControlID="tbxPlayer1Deff"
                                CompletionInterval="2" MinimumPrefixLength="2" ServiceMethod="GetPlayerNames" UseContextKey="true" ServicePath="">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxPlayer2Deff" runat="server" Columns="40" OnTextChanged="tbxPlayer_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbxPlayer2Deff" ValidationGroup="vgsr"
                                Display="None" ErrorMessage="Pole wymagane"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFieldValidator3">
                            </asp:ValidatorCalloutExtender>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" TargetControlID="tbxPlayer2Deff"
                                CompletionInterval="2" MinimumPrefixLength="2" ServiceMethod="GetPlayerNames" UseContextKey="true" ServicePath="">
                            </asp:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <h3>
                                Wynik
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="tbxScoreA" runat="server" MaxLength="2" Columns="2"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbxScoreA"
                                FilterMode="ValidChars" FilterType="Numbers">
                            </asp:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbxScoreA" ValidationGroup="vgsr"
                                Display="None" ErrorMessage="Pole wymagane"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" TargetControlID="RequiredFieldValidator4">
                            </asp:ValidatorCalloutExtender>
                            :
                            <asp:TextBox ID="tbxScoreB" runat="server" MaxLength="2" Columns="2"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbxScoreB"
                                FilterMode="ValidChars" FilterType="Numbers">
                            </asp:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbxScoreB" ValidationGroup="vgsr"
                                Display="None" ErrorMessage="Pole wymagane"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" TargetControlID="RequiredFieldValidator5">
                            </asp:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnSubmit" runat="server" Text="Zgłoś wynik" OnClick="btnSubmit_Click" CausesValidation="true"
                                style="margin-top: 5px;" ValidationGroup="vgsr" />
                            &nbsp;<asp:Button ID="btnClear" runat="server" Text="Wyczyść" OnClick="btnClear_Click" 
                                CausesValidation="false" />

                            <asp:Panel ID="pnlInfo" runat="server" CssClass="InfoPanel"></asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
