<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="PlayersList.aspx.cs" Inherits="Trambambule.PlayersList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%=Page.ResolveUrl("Assets/jquery-1.10.2.min.js")%>" type="text/javascript"></script>
    <script src="<%=Page.ResolveUrl("Assets/jquery.cookie.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        function hideShowPanel(me) {
            var addPanel = jQuery('#addPanel');
            var a = jQuery(me);
            var cookieName = 'TrambambuleAddNewPlayer';
            if (addPanel.css('display') == 'none') {
                jQuery.cookie(cookieName, '1', { expires: 1, path: '/' });
                addPanel.slideDown('slow');
                a.text('Ukryj panel dodawania nowego gracza');
            }
            else {
                jQuery.cookie(cookieName, '0', { expires: 1, path: '/' });
                addPanel.slideUp('slow');
                a.text('Pokaż panel dodawania nowego gracza');
            }
        }

        function clearValidations() {
            jQuery('.validator').css('display', 'none');
        }
         

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        Lista graczy</h1>
    <a class="addNewPlayerPlayer" href="javascript:void(null);" onclick="hideShowPanel(this);">
        <%=ShowPanelText %></a>
    <div id="addPanel" <%=Hidden %>>
        <div id="fields">
            <div>
                <span>Imię: </span>
                <asp:TextBox ID="txtNewPlayerFirstName" runat="server" MaxLength="100" ValidationGroup="addNewPlayerGroup"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqTxtNewPlayerFirstName" runat="server" Text="*"
                    ControlToValidate="txtNewPlayerFirstName" CssClass="validator" Display="Dynamic"
                    ErrorMessage="Pole Imię jest wymagane" ValidationGroup="addNewPlayerGroup"></asp:RequiredFieldValidator>
            </div>
            <div>
                <span>Nazwisko:</span><asp:TextBox ID="txtNewPlayerLastName" runat="server" MaxLength="100"
                    ValidationGroup="addNewPlayerGroup"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqTxtNewPlayerLastName" runat="server" Text="*"
                    ControlToValidate="txtNewPlayerLastName" CssClass="validator" Display="Dynamic"
                    ErrorMessage="Pole Nazwisko jest wymagane" ValidationGroup="addNewPlayerGroup"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cusValidator" runat="server" Display="Dynamic" EnableClientScript="false"
                    OnServerValidate="cusValidator_ServerValidate" CssClass="validator" Text="*"
                    ValidationGroup="addNewPlayerGroup"></asp:CustomValidator>
            </div>
            <div>
                <span>Miasto:</span><asp:DropDownList ID="ddlLocation" runat="server">
                    <asp:ListItem Text="Warszawa" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Łódź" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="btns">
                <asp:Button ID="btnAddNewPlayer" runat="server" OnClick="btnAddNewPlayer_Click" Text="Dodaj gracza"
                    ValidationGroup="addNewPlayerGroup" CssClass="addNewPlayerBtn" OnClientClick="clearValidations();" />
                <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Wyczyść pola" />
            </div>
        </div>
        <div class="InfoPanel">
            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="validator" ValidationGroup="addNewPlayerGroup" />
            <asp:Literal ID="litStatus" runat="server"></asp:Literal>
        </div>
    </div>
    <asp:Repeater ID="repPlayers" runat="server">
        <HeaderTemplate>
            <table class="Table" style="border-collapse: collapse;">
                <tbody>
                    <tr>
                        <th>
                            #
                        </th>
                        <th>
                            Imie i nazwisko
                        </th>
                        <th>
                            Miasto
                        </th>
                    </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%#(Container.ItemIndex +1).ToString()%>
                </td>
                <td>
                    <a class="person<%#GetCustomClass((int)Eval("Id"))%>" href="<%=Request.RawUrl.IndexOf("?") > 0 ? Request.RawUrl.Substring(0,Request.RawUrl.IndexOf("?")) : Request.RawUrl%>?userId=<%#Eval("Id")%><%=Request.QueryString["p"] != null? "&p=1" : "" %>">
                        <%#Eval("FirstName") + " " + Eval("LastName")%></a>
                </td>
                <td>
                    <%#GetLocation((int)Eval("Location")) %>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
