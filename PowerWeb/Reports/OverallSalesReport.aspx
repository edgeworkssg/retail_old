<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="OverallSalesReport" Title="Overall Sales Report"
    CodeBehind="OverallSalesReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <div style="height: 20px;" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
    </div>
    <table width="600px" id="FilterTable">
        <%--        <tr style="height:20px;"><td colspan="4" class="wl_pageheaderSub" ><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>--%>
        <tr>
            <td style="width: 102px; height: 3px">
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
            <td style="height: 3px">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>
        <tr>
            <td style="width: 102px">
                <asp:RadioButton ID="rdbMonth" runat="server" GroupName="DateSearch" Text="<%$ Resources:dictionary, Month %>"
                    Width="68px" />
            </td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" Width="122px">
                    <asp:ListItem Value="1">January</asp:ListItem>
                    <asp:ListItem Value="2">February</asp:ListItem>
                    <asp:ListItem Value="3">March</asp:ListItem>
                    <asp:ListItem Value="4">April</asp:ListItem>
                    <asp:ListItem Value="5">May</asp:ListItem>
                    <asp:ListItem Value="6">June</asp:ListItem>
                    <asp:ListItem Value="7">July</asp:ListItem>
                    <asp:ListItem Value="8">August</asp:ListItem>
                    <asp:ListItem Value="9">September</asp:ListItem>
                    <asp:ListItem Value="10">October</asp:ListItem>
                    <asp:ListItem Value="11">November</asp:ListItem>
                    <asp:ListItem Value="12">December</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblYear" runat="server"></asp:Label>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 102px">
                <asp:Literal ID="Literal2" runat="server" Text="Search"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtItemName" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Category %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddCategory" runat="server" Width="179px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 102px; height: 31px;">
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal>
            </td>
            <td style="height: 31px">
                <subsonic:DropDown ID="ddPOS" runat="server" ShowPrompt="True" TableName="PointOfSale"
                    TextField="PointOfSaleName" ValueField="PointOfSaleID" Width="170px" PromptText="ALL"
                    OnInit="ddPOS_Init">
                </subsonic:DropDown>
            </td>
            <td style="height: 31px">
                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
            </td>
            <td style="height: 31px">
                <asp:DropDownList ID="ddlOutlet" runat="server" Width="180px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 102px; height: 31px;">
                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Department %>"></asp:Literal>
            </td>
            <td>
                <subsonic:DropDown ID="ddDept" runat="server" PromptValue="" TableName="ItemDepartment"
                    TextField="DepartmentName" ValueField="ItemDepartmentID" Width="172px" OnInit="ddDept_Init"
                    ShowPrompt="True" PromptText="ALL">
                </subsonic:DropDown>
            </td>
            <td style="height: 31px">
                Sales Person
            </td>
            <td style="height: 31px">
                <asp:TextBox ID="txtSalesPerson" runat="server" Width="172px" />
            </td>
        </tr>
        <tr>
            <td style="width: 102px">
                Membership No
            </td>
            <td style="height: 31px">
                <asp:TextBox ID="txtMemberNo" runat="server" Width="172px" />
            </td>
            <td style="height: 31px">
                Member Name
            </td>
            <td style="height: 31px">
                <asp:TextBox ID="txtMemberName" runat="server" Width="172px" />
            </td>            
        </tr>        
        <%-- <tr><td colspan=2 >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td colspan=2 align=right class="ExportButton">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>--%>
    </table>
    <table id="search_ExportTable" style="vertical-align: middle; width: 600px; height: 40px;"
        border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="height: 30px; width: 50%; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton><div class="divider">
                </div>
                <asp:LinkButton ID="LinkButton4" class="classname" runat="server">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>
            </td>
            <td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                vertical-align: middle; right: 0px;">
                <asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="height: 25px;">
                <asp:Literal ID="litMessage" runat="server" Text="" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" ShowFooter="True" Width="100%" runat="server" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" DataKeyNames="ItemNo" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" PageSize="20">
        <Columns>
            <asp:BoundField DataField="PointOfSale" HeaderText="Point Of Sale" SortExpression="PointOfSale" />
            <asp:BoundField DataField="Outlet" HeaderText="Outlet" SortExpression="Outlet" />
            <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No." />
            <asp:BoundField DataField="OrderDate" HeaderText="Order Date" SortExpression="OrderDate"
                DataFormatString="{0:dd-MM-yyyy}" />
            <asp:BoundField DataField="SalesPerson" HeaderText="Sales Person" SortExpression="SalesPerson" />
            <asp:BoundField DataField="MembershipNo" HeaderText="Membership No" SortExpression="MembershipNo" />
            <asp:BoundField DataField="MemberName" HeaderText="Member Name" SortExpression="MemberName" />
            <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
            <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
            <asp:BoundField DataField="ItemNo" HeaderText="Item No" SortExpression="ItemNo" />
            <asp:BoundField DataField="ItemName" HeaderText="Item Name" SortExpression="ItemName" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
            <asp:BoundField DataField="SalesPrice" HeaderText="Sales Price" SortExpression="SalesPrice"
                DataFormatString="{0:N2}" />
            <asp:BoundField DataField="FactoryPrice" HeaderText="Factory Price" SortExpression="FactoryPrice"
                DataFormatString="{0:N2}" />
            <asp:BoundField DataField="Profit" HeaderText="Profit" SortExpression="Profit" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
            <asp:BoundField DataField="QtyAfter" HeaderText="Qty After" />
        </Columns>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal><asp:DropDownList
                    ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                    ID="lblPageCount" runat="server"></asp:Label><asp:Button ID="btnNext" runat="server"
                        CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next"
                        CommandName="Page" />
                <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                    CommandArgument="Last" CommandName="Page" />
            </div>
        </PagerTemplate>
    </asp:GridView>
</asp:Content>
