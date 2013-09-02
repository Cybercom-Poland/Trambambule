<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Trambambule.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <h1>Ranking graczy</h1>
    
    <div class="ContentPanel">
        <asp:GridView ID="gvPlayerResults" runat="server" DataSourceID="sqldsResults" CssClass="Table"
            OnRowDataBound="gvPlayerResults_RowDataBound">        
        </asp:GridView>
        <center>
            <asp:RadioButtonList ID="rblLocation" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                <asp:ListItem Text="Ogólny" Value="-1" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Warszawa" Value="0"></asp:ListItem>
                <asp:ListItem Text="Łódź" Value="1"></asp:ListItem>
            </asp:RadioButtonList>
        </center>
        <asp:SqlDataSource ID="sqldsResults" runat="server" ConnectionString="<%$ ConnectionStrings:TrambambuleConnectionString %>"
            SelectCommand="GetUserResults" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="rblLocation" PropertyName="SelectedValue" DbType="Int16" Name="Location" />
            </SelectParameters>        
        </asp:SqlDataSource>
    </div>
    
</asp:Content>
