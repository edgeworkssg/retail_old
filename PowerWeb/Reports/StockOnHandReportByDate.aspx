<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="StockOnHandReportByDate" Title="<%$Resources:dictionary, Stock Balance Report by Date%>"
    CodeBehind="StockOnHandReportByDate.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <table width="900px" id="FilterTable">
        <tr style="height: 20px;">
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="width: 102px; height: 3px">
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td style="height: 3px">
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
            <td colspan="2">               
            </td>
        </tr>
        <tr>
            <td >
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Item Department %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlItemDepartment" runat="server" OnInit="ddlItemDepartment_Init">
                </asp:DropDownList>
            </td>
            <td >
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Category %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlCategory" runat="server" OnInit="ddlCategory_Init">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Supplier  %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlSupplier" runat="server" OnInit="ddlSupplier_Init">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="lblInventoryLocation" runat="server" Text="<%$Resources:dictionary,Inventory Location %>"></asp:Literal>
            </td>
            <td>
                <%--<subsonic:DropDown ID="ddlInventoryLocation" runat="server" PromptText="--Please Select--"
                    ShowPrompt="True" TableName="InventoryLocation" TextField="InventoryLocationName"
                    ValueField="InventoryLocationID" Width="177px" PromptValue="0">
                </subsonic:DropDown>--%>
                <asp:DropDownList ID="ddlInventoryLocation" runat="server" OnInit="ddlInventoryLocation_Init">
                </asp:DropDownList>
                &nbsp;
                <asp:CheckBox ID="cbUseInventoryLocationGroup" runat="server" Text="<%$Resources:dictionary, Use Inventory Location Group%>"
                    OnCheckedChanged="cbUseInventoryLocationGroup_OnCheckedChanged" AutoPostBack="true" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtSearch" runat="server" Width="172px"></asp:TextBox>
                &nbsp;
                <%--<asp:CheckBox ID="cbShowCostPrice" runat="server" Text="<%$Resources:dictionary, Display Cost Price%>" Visible="False" />--%>
            </td>
            <td>
                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Cost Of Goods %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlInventoryLocationCost" runat="server">
                </asp:DropDownList>
                <asp:CheckBox ID="useGroupPrice" Visible="false" Checked="true" Text="<%$Resources:dictionary, Use Inventory Location Group Cost %>" runat="server" />&nbsp;
                <asp:CheckBox ID="useGlobalPrice" Visible="false"  Checked="true" Text="<%$Resources:dictionary, Use Global Cost %>" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton><div class="divider">
                </div>
                <asp:LinkButton ID="LinkButton4" class="classname" runat="server" OnClick="btnClear_Click">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>
            </td>
            <td  colspan="2" align="right">
                <asp:LinkButton ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton><div class="divider">
                    </div>
                <asp:LinkButton ID="btnExportStockTakeForm" runat="server" class="classBlue" Text="<%$ Resources:dictionary, Export Stock Take Form%>"
                    OnClick="LinkButton1_Click"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label>
            </td>
        </tr>
    </table>
    <br /><br />
    <asp:GridView ID="gvReport" Width="98%" runat="server" AllowPaging="True" AllowSorting="True"
        OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting" OnPageIndexChanging="gvReport_PageIndexChanging"
        AutoGenerateColumns="False" PageSize="20" SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound"
        ShowFooter="True">
        <Columns>
            <asp:BoundField DataField="DepartmentName" SortExpression="DepartmentName" HeaderText="<%$Resources:dictionary, Department%>" />
            <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="<%$Resources:dictionary,Category %>" />
            <asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="<%$Resources:dictionary,ItemNo %>" />
            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="<%$Resources:dictionary,Item %>" />
            <asp:BoundField DataField="Barcode" SortExpression="UOM" HeaderText="<%$Resources:dictionary,Barcode %>" />
            <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="<%$Resources:dictionary,Quantity %>" />
            <asp:BoundField DataField="RetailPrice" SortExpression="RetailPrice" HeaderText="<%$Resources:dictionary, Retail Price%>" />
            <asp:BoundField DataField="TotalRetail" SortExpression="TotalRetail" HeaderText="<%$Resources:dictionary, Total Retail%>" />
            <asp:BoundField DataField="COG" SortExpression="COG" HeaderText="<%$Resources:dictionary,Avg Unit Cost %>" />
            <asp:BoundField DataField="TotalCost" SortExpression="TotalCost" HeaderText="<%$Resources:dictionary,Total Cost %>" />  
            <asp:BoundField DataField="COGGroup" SortExpression="COGGroup" HeaderText="<%$Resources:dictionary,Avg Unit Cost %>" />
            <asp:BoundField DataField="TotalCostGroup" SortExpression="TotalCostGroup" HeaderText="<%$Resources:dictionary,Total Cost %>" /> 
            <asp:BoundField DataField="GlobalCostGroup" SortExpression="COGGroup" HeaderText="<%$Resources:dictionary,Avg Unit Cost %>" />
            <asp:BoundField DataField="TotalGlobalCostGroup" SortExpression="TotalCostGroup" HeaderText="<%$Resources:dictionary,Total Cost%>" />             
            <asp:BoundField DataField="Attributes1" SortExpression="Attributes1" HeaderText="<%$Resources:dictionary, Attributes1%>" />
            <asp:BoundField DataField="Attributes2" SortExpression="Attributes2" HeaderText="<%$Resources:dictionary, Attributes2%>" />
            <asp:BoundField DataField="Attributes3" SortExpression="Attributes3" HeaderText="<%$Resources:dictionary, Attributes3%>" />
            <asp:BoundField DataField="Attributes4" SortExpression="Attributes4" HeaderText="<%$Resources:dictionary, Attributes4%>" />
            <asp:BoundField DataField="Attributes5" SortExpression="Attributes5" HeaderText="<%$Resources:dictionary, Attributes5%>" />
            <asp:BoundField DataField="Attributes6" SortExpression="Attributes6" HeaderText="<%$Resources:dictionary, Attributes6%>" />
            <asp:BoundField DataField="Attributes7" SortExpression="Attributes7" HeaderText="<%$Resources:dictionary, Attributes7%>" />
            <asp:BoundField DataField="Attributes8" SortExpression="Attributes8" HeaderText="<%$Resources:dictionary, Attributes8%>" />
        </Columns>
        <PagerTemplate>
            <div style="text-align: center; border: 1px solid #666666">
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
        <EmptyDataTemplate>
            <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Search does not produce any result%>" /></EmptyDataTemplate>
    </asp:GridView>
</asp:Content>
