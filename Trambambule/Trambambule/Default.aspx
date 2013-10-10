<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Trambambule.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <h1>    
        Ranking graczy
        <table cellpadding="0" cellspacing="0" border="0" style="float: right; margin-top: -3px; font-weight: normal;">
            <tr>
                <td style="padding-right: 15px;">
                    <asp:CheckBox ID="cbxStableRank" runat="server" Text="Tylko stabilny ranking" Checked="true" AutoPostBack="true" />
                </td>
                <td>
                    <asp:RadioButtonList ID="rblLocation" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Wspólny" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Warszawa" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Łódź" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </h1>
    
    <div class="ContentPanel">
        <asp:GridView ID="gvPlayerResults" runat="server" DataSourceID="sqldsResults" CssClass="Table"
            OnRowDataBound="gvPlayerResults_RowDataBound">        
        </asp:GridView>
        <center>
        </center>
        <asp:SqlDataSource ID="sqldsResults" runat="server" ConnectionString="<%$ ConnectionStrings:TrambambuleConnectionString %>"
            SelectCommand="GetUserResults" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="rblLocation" PropertyName="SelectedValue" DbType="Int16" Name="Location" />
                <asp:ControlParameter ControlID="cbxStableRank" PropertyName="Checked" DbType="Boolean" Name="Stable" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    
</asp:Content>
