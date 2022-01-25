<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="OutletDailyCategorySales.aspx.cs" Inherits="PowerWeb.Reports.OutletDailyCategorySales"
    Title="Outlet Daily Category Sales" %>

<%@ Register Src="../CustomControl/OutletDropdownList.ascx" TagName="OutletDropdownList"
    TagPrefix="uc1" %>
    
<%@ Register Src="../CustomControl/MultiCheckCombo.ascx" TagName="MultiCheckCombo"
    TagPrefix="uc2" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .style4
        {
            height: 26px;
        }
        .style5
        {
            margin-right: 10px;
        }
        .fieldname
        {
            font-weight: bold;
            background-color: Gainsboro;
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <table width="800px" id="FilterTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="style4">
                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Month %>"></asp:Literal>
            </td>
            <td align="left" class="style4">
                <asp:DropDownList ID="ddlMonth" runat="server" Width="122px">
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
                <asp:DropDownList ID="ddlYear" runat="server">
                </asp:DropDownList>
            </td>
            <td colspan="2" align="left" class="style4">
                <asp:CheckBox ID="chkShowBeforeGST" runat="server" Text="<%$Resources:dictionary,Show Amount Before GST%>" />         
            </td>
        </tr>
        <tr>
            <uc1:OutletDropdownList ID="OutletDropdownList" runat="server"></uc1:OutletDropdownList>
        </tr>
        <tr>
            <td>
				
				<asp:Literal ID="Literal39"  runat="server" Text="<%$Resources:dictionary, Category%>" />
			</td>
            <td colspan="3">
                <uc2:MultiCheckCombo ID="mccCategory" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="Button1" runat="server" CssClass="classname" OnClick="btnSearch_Click"
                    Text="<%$ Resources:dictionary, Search %>" />
                <asp:Button ID="Button2" runat="server" CssClass="classname" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />            
            </td>
            <td align="right" colspan="2">
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="classname" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>            
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" Width="100%" runat="server" AllowPaging="True" AllowSorting="True"
        OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting" OnPageIndexChanging="gvReport_PageIndexChanging"
        SkinID="scaffold" PageSize="50" OnRowDataBound="gvReport_RowDataBound" 
    ShowFooter="True">
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
