<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UserStats.aspx.cs" Inherits="Trambambule.UserStats" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Statystyki gracza</h1>

    <div style="clear:both;">
        <div class="FloatingBox">
            <h1>Gra z partnerami</h1>
            <asp:Panel ID="pnlPartnerStats" runat="server" CssClass="FloatingBoxContent">
            </asp:Panel>
            <h1>Ranking</h1>
            <asp:Panel ID="pnlRanking" runat="server" CssClass="FloatingBoxContent">
            </asp:Panel>
            <h1>Bramki</h1>
            <asp:Panel ID="pnlGoals" runat="server" CssClass="FloatingBoxContent">
            </asp:Panel>
        </div>
        <div class="FloatingBox">
            <h1>Rywale</h1>
            <asp:Panel ID="pnlRivals" runat="server" CssClass="FloatingBoxContent">
            </asp:Panel>
            <h1>Wykresy (ost. 20 gier)</h1>
            <asp:Panel ID="pnlGraphs" runat="server" CssClass="FloatingBoxContent">
                <asp:LineChart ID="lcRating" runat="server" 
                    ChartWidth="343" ChartHeight="200" ChartType="Basic" 
                    ChartTitleColor="#0E426C" CategoryAxisLineColor="#D08AD9" 
                    ValueAxisLineColor="#D08AD9" BaseLineColor="#A156AB"
                    CssClass="lineChart" BorderStyle="None">            
                </asp:LineChart>
                <p></p>
                <asp:LineChart ID="lcRanking" runat="server" 
                    ChartWidth="343" ChartHeight="200" ChartType="Basic" 
                    ChartTitleColor="#0E426C" CategoryAxisLineColor="#D08AD9" 
                    ValueAxisLineColor="#D08AD9" BaseLineColor="#A156AB"
                    CssClass="lineChart" BorderStyle="None">
                </asp:LineChart>
            </asp:Panel>
        </div>
    </div>
    <div style="clear:both; margin: 5px;">
        <h1>Zestawienie statystyk z partnerami</h1>
        <asp:GridView ID="gvPartners" runat="server" CssClass="Table" OnRowDataBound="gvStats_RowDataBound"></asp:GridView>
    </div>
    <div style="clear:both; margin: 5px;">
        <h1>Zestawienie statystyk z rywalami</h1>
        <asp:GridView ID="gvRivals" runat="server" CssClass="Table" OnRowDataBound="gvStats_RowDataBound"></asp:GridView>
    </div>

</asp:Content>
