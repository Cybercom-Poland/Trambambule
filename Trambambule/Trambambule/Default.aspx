<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Trambambule.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <h1>Ranking graczy</h1>

    <div class="ContentPanel">
        <asp:GridView ID="gvPlayerResults" runat="server" DataSourceID="sqldsResults" CssClass="Table">        
        </asp:GridView>
        <asp:SqlDataSource ID="sqldsResults" runat="server" ConnectionString="<%$ ConnectionStrings:TrambambuleConnectionString %>"
            SelectCommand="GetUserResults" SelectCommandType="StoredProcedure">        
        </asp:SqlDataSource>
    </div>
    
</asp:Content>
