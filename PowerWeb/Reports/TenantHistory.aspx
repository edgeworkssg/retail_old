<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="TenantHistory.aspx.cs" Inherits="PowerWeb.Reports.TenantHistory"
    Title="<%$Resources:dictionary, Tenant History%>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="1000px" id="FilterTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="fieldname" style="width: 105px">
                <asp:Label ID="lblStartDate" runat="server" Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server" Width="141px"></asp:TextBox><asp:ImageButton
                    ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                &nbsp;
            </td>
            <td class="fieldname">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                &nbsp;
                <br />
            </td>
        </tr>
        <tr>
            <td class="fieldname">
                <asp:Literal ID="lblOutlet" runat="server" Text="<%$Resources:dictionary, Outlet %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlOutlet" runat="server" Width="150px" DataValueField="OutletName"
                    DataTextField="OutletName" OnInit="ddlOutlet_Init" AutoPostBack="true" OnSelectedIndexChanged="ddlOutlet_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="fieldname" style="width: 105px">
                <asp:Literal ID="lblPOS" runat="server" Text="<%$Resources:dictionary, Point of Sale %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddPOS" runat="server" Width="150px" DataValueField="PointOfSaleID"
                    DataTextField="PointOfSaleName">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldname">
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Access Source%>" />
            </td>
            <td>
                <asp:DropDownList ID="ddlAccessSource" runat="server" Width="150px" DataValueField="OutletName"
                    DataTextField="OutletName" OnInit="ddlOutlet_Init" AutoPostBack="true" OnSelectedIndexChanged="ddlOutlet_SelectedIndexChanged">
                    <asp:ListItem Value="ALL" Text="<%$Resources:dictionary, ALL%>"></asp:ListItem>
                    <asp:ListItem Value="WEB" Text="<%$Resources:dictionary, WEB%>"></asp:ListItem>
                    <asp:ListItem Value="WEBServices" Text="<%$Resources:dictionary, WEBServices%>"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="fieldname" style="width: 105px">
                <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Search%>" />
            </td>
            <td>
                <asp:TextBox ID="txtSearch" runat="server" Width="150px"></asp:TextBox>
            </td>
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
    <asp:GridView ID="gvReport" Width="1000px" runat="server" ShowFooter="True" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" PageSize="20">
        <Columns>
            <asp:BoundField ItemStyle-Width="100px" DataField="OutletName" HeaderText="<%$Resources:dictionary, Outlet%>"
                SortExpression="OutletName" />
            <asp:BoundField DataField="PointOfSaleName" HeaderText="<%$Resources:dictionary, Point Of Sale%>" SortExpression="PointOfSaleName" />
            <asp:BoundField DataField="TenantMachineID" HeaderText="<%$Resources:dictionary, Tenant Code%>" SortExpression="TenantMachineID" />
            <asp:BoundField DataField="RetailerLevel" HeaderText="<%$Resources:dictionary, Retailer Level%>" SortExpression="RetailerLevel" />
            <asp:BoundField ItemStyle-Width="100px" DataField="ShopNo" HeaderText="<%$Resources:dictionary, Shop No%>" SortExpression="ShopNo" />
            <asp:BoundField ItemStyle-Width="100px" HeaderText="<%$Resources:dictionary, Access Date%>" DataFormatString="{0:dd-MMM-yyyy}"
                HtmlEncode="false" DataField="AccessDate" SortExpression="AccessDate" />
            <asp:BoundField DataField="LoginName" HeaderText="<%$Resources:dictionary, Login Name%>" SortExpression="LoginName" />
            <asp:BoundField DataField="AccessSource" HeaderText="<%$Resources:dictionary, Access Source%>" SortExpression="AccessSource" />
            <asp:BoundField DataField="AccessType" HeaderText="<%$Resources:dictionary, Log Message%>" SortExpression="AccessType" />
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
