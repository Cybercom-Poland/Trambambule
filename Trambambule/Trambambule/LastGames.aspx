<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="LastGames.aspx.cs" Inherits="Trambambule.LastGames" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="upanSendResults" runat="server" UpdateMode="Always">
        <ContentTemplate>
                        
            <h1>Ostatnie mecze</h1>
            
            <div class="ContentPanel">
                <asp:Table ID="tblResults" runat="server" CssClass="Table" CellSpacing="0"></asp:Table>
            </div>            

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
