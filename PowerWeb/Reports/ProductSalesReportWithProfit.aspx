<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="ProductSalesReportWithProfit" Title="Product Sales Report (With Profit) "
    CodeBehind="ProductSalesReportWithProfit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="600px">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td  style="width: 102px; height: 3px">
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
            <td  style="height: 3px">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>
        <tr>
            <td  style="width: 102px">
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
            <td >
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td  style="width: 102px">
                <asp:Literal ID="Literal2" runat="server" Text="Search"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtItemName" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td >
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Category %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddCategory" runat="server" Width="179px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td  style="width: 102px; height: 31px;">
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal>
            </td>
            <td style="height: 31px">
                <subsonic:DropDown ID="ddPOS" runat="server" ShowPrompt="True" TableName="PointOfSale"
                    TextField="PointOfSaleName" ValueField="PointOfSaleID" Width="170px" PromptText="ALL"
                    OnInit="ddPOS_Init">
                </subsonic:DropDown>
            </td>
            <td  style="height: 31px">
                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
            </td>
            <td style="height: 31px">
                <asp:DropDownList ID="ddlOutlet" runat="server" Width="180px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td  style="width: 102px">
                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Department %>"></asp:Literal>
            </td>
            <td>
                <subsonic:DropDown ID="ddDept" runat="server" PromptValue="" TableName="ItemDepartment"
                    TextField="DepartmentName" ValueField="ItemDepartmentID" Width="172px" OnInit="ddDept_Init"
                    ShowPrompt="True" PromptText="ALL">
                </subsonic:DropDown>
            </td>
            <td >
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />
            </td>
            <td colspan="2" align="right" class="ExportButton">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" ShowFooter="True" Width="100%" runat="server" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" DataKeyNames="ItemNo" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" PageSize="20">
        <Columns>
            <asp:BoundField DataField="DepartmentName" HeaderText="Department" SortExpression="DepartmentName" />
            <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="<%$Resources:dictionary,Category %>" />
            <asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="<%$Resources:dictionary,Item No %>" />
            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="<%$Resources:dictionary,Item %>" />
            <asp:BoundField DataField="Attributes1" SortExpression="Attributes1" HeaderText="Attributes1" />
            <asp:BoundField DataField="Attributes2" SortExpression="Attributes2" HeaderText="Attributes2" />
            <asp:BoundField DataField="Attributes3" SortExpression="Attributes3" HeaderText="Attributes3" />
            <asp:BoundField DataField="Attributes4" SortExpression="Attributes4" HeaderText="Attributes4" />
            <asp:BoundField DataField="Attributes5" SortExpression="Attributes5" HeaderText="Attributes5" />
            <asp:BoundField DataField="TotalQuantity" SortExpression="TotalQuantity" HeaderText="<%$Resources:dictionary,Total Qty %>" />
            <asp:BoundField DataField="TotalAmount" SortExpression="TotalAmount" HeaderText="Nett Sales" />
            <asp:BoundField Visible="true" DataField="GSTAmount" SortExpression="GSTAmount" HeaderText="<%$Resources:dictionary,GST Amount %>" />
            <asp:BoundField Visible="False" DataField="Discount" SortExpression="Discount" 
                HeaderText="<%$Resources:dictionary,Discount %>" />
            <asp:BoundField Visible="true" DataField="TotalAmountWithoutGST" SortExpression="TotalAmountWithoutGST"
                HeaderText="<%$Resources:dictionary,Nett Sales (Without GST) %>" />
            <asp:BoundField Visible="true" DataField="TotalCostOfGoodsSold" HeaderText="<%$Resources:dictionary,Cost Of Goods %>"
                SortExpression="TotalCostOfGoodsSold" />
            <asp:BoundField Visible="true" DataField="ProfitLoss" HeaderText="<%$Resources:dictionary,P/L %>"
                SortExpression="ProfitLoss" />
            <asp:BoundField Visible="true" DataField="ProfitLossPercentage" HeaderText="<%$Resources:dictionary,P/L(%) %>"
                SortExpression="ProfitLossPercentage" />
            <asp:BoundField DataField="PointOfSaleName" SortExpression="PointOfSaleName" HeaderText="<%$Resources:dictionary,Point Of Sale %>" />
            <asp:BoundField DataField="OutletName" SortExpression="OutletName" HeaderText="<%$Resources:dictionary,Outlet %>" />
        </Columns>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True"
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
