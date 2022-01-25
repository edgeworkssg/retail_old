<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="MissingDailySales.aspx.cs" Inherits="PowerWeb.Reports.MissingDailySales"
    Title="<%$Resources:dictionary, Missing Daily Sales%>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <table width="600px" id="FilterTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="fieldname" style="width: 105px">
                <asp:Literal ID="lblOutlet" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlOutlet" runat="server" Width="200px" DataValueField="OutletName"
                    DataTextField="OutletName" OnInit="ddlOutlet_Init" AutoPostBack="true" 
                    onselectedindexchanged="ddlOutlet_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="fieldname">
                <asp:Literal ID="lblPOS" runat="server" Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddPOS" runat="server" Width="200px" DataValueField="PointOfSaleID"
                    DataTextField="PointOfSaleName">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldname">
                <asp:Label ID="lblMonth" runat="server" Text="Month" />
            </td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" Width="120px">
                    <asp:ListItem Value="1" Text="<%$Resources:dictionary,January %>"></asp:ListItem>
                    <asp:ListItem Value="2" Text="<%$Resources:dictionary,February %>"></asp:ListItem>
                    <asp:ListItem Value="3" Text="<%$Resources:dictionary,March %>"></asp:ListItem>
                    <asp:ListItem Value="4" Text="<%$Resources:dictionary,April %>"></asp:ListItem>
                    <asp:ListItem Value="5" Text="<%$Resources:dictionary,May %>"></asp:ListItem>
                    <asp:ListItem Value="6" Text="<%$Resources:dictionary,June %>"></asp:ListItem>
                    <asp:ListItem Value="7" Text="<%$Resources:dictionary,July %>"></asp:ListItem>
                    <asp:ListItem Value="8" Text="<%$Resources:dictionary,August %>"></asp:ListItem>
                    <asp:ListItem Value="9" Text="<%$Resources:dictionary,September %>"></asp:ListItem>
                    <asp:ListItem Value="10" Text="<%$Resources:dictionary,October %>"></asp:ListItem>
                    <asp:ListItem Value="11" Text="<%$Resources:dictionary,November %>"></asp:ListItem>
                    <asp:ListItem Value="12" Text="<%$Resources:dictionary,December %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlYear" runat="server" Width="80px">
                </asp:DropDownList>
            </td>
            <td class="fieldname" colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" CssClass="classname" />
                <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" CssClass="classname" />
            </td>
            <td colspan="2" align="right">
                <asp:LinkButton ID="lnkExport" runat="server" CssClass="classBlue" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gvReport" Width="800px" runat="server" ShowFooter="True" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" PageSize="20">
        <Columns>
            <asp:BoundField HeaderText="<%$Resources:dictionary, Sales Date%>" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false"
                DataField="SalesDate" SortExpression="SalesDate" />
            <asp:BoundField DataField="OutletName" HeaderText="<%$Resources:dictionary, Outlet%>" SortExpression="OutletName" />
            <asp:BoundField DataField="PointOfSaleName" HeaderText="<%$Resources:dictionary, Point Of Sale%>" SortExpression="PointOfSaleName" />
            <asp:BoundField DataField="RetailerLevel" HeaderText="<%$Resources:dictionary, Retailer Level%>" SortExpression="RetailerLevel" />
            <asp:BoundField DataField="ShopNo" HeaderText="<%$Resources:dictionary, Shop No%>" SortExpression="ShopNo" />                        
            <asp:BoundField DataField="TenantMachineID" HeaderText="<%$Resources:dictionary, Tenant Code%>" SortExpression="TenantMachineID" />            
        </Columns>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" CssClass="scaffoldEditItem" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                    ID="lblPageCount" runat="server"></asp:Label>
                <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> "
                    CommandArgument="Next" CommandName="Page" />
                <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                    CommandArgument="Last" CommandName="Page" />
            </div>
        </PagerTemplate>
    </asp:GridView>
</asp:Content>
