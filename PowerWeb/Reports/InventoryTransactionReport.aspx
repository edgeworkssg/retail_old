<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="InventoryTransactionReport" Title="<%$Resources:dictionary,Inventory Activity Report %>"
    CodeBehind="InventoryTransactionReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="600px" id="FieldsTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseStartDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Use Start Date %>" />
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseEndDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Use End Date %>" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Item Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtItemName" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,User Name %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtUserName" runat="server" Width="173px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Location %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddLocation" runat="server" CausesValidation="True" Width="182px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Movement Type %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlMovementType" runat="server" CausesValidation="True" Width="182px">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="Stock In" Text="<%$Resources:dictionary, Stock In%>"></asp:ListItem>
                    <asp:ListItem Value="Stock Out" Text="<%$Resources:dictionary, Stock Out%>"></asp:ListItem>
                    <asp:ListItem Value="Adjustment In" Text="<%$Resources:dictionary, Adjustment In%>"></asp:ListItem>
                    <asp:ListItem Value="Adjustment Out" Text="<%$Resources:dictionary, Adjustment Out%>"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Remark%>" />
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" Width="172px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Line Remark%>" />
            </td>
            <td>
                <asp:TextBox ID="txtLineRemark" runat="server" Width="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 15px">
                &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" />
            </td>
            <td colspan="2" align="right" valign="middle" class="ExportButton">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" Width="800px" runat="server" ShowFooter="true" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        PageSize="20" OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
        SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:BoundField />
            <asp:BoundField DataField="InventoryDate" SortExpression="InventoryDate" HeaderText="<%$Resources:dictionary,Date %>" />
            <asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="<%$Resources:dictionary,Item %>" />
            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="<%$Resources:dictionary,Item %>" />
            <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="<%$Resources:dictionary,Category %>" />
            <asp:BoundField Visible="false" DataField="ProductLine" SortExpression="ProductLine"
                HeaderText="<%$Resources:dictionary,Product Line %>" />
            <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="<%$Resources:dictionary,User %>" />
            <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="<%$Resources:dictionary,Qty %>" />
            <asp:BoundField DataField="MovementType" HeaderText="<%$Resources:dictionary,Type %>"
                SortExpression="MovementType" />
            <asp:BoundField DataField="InventoryLocationName" HeaderText="<%$Resources:dictionary,Location %>"
                SortExpression="InventoryLocationName" />
            <asp:BoundField Visible="false" DataField="RetailPrice" SortExpression="RetailPrice"
                HeaderText="<%$Resources:dictionary,Retail Price (SGD) %>" />
            <asp:BoundField Visible="false" DataField="FactoryPriceUSD" SortExpression="FactoryPriceUSD"
                HeaderText="<%$Resources:dictionary,Factory Price (USD) %>" />
            <asp:BoundField Visible="false" DataField="FactoryPrice" SortExpression="FactoryPrice"
                HeaderText="<%$Resources:dictionary,Factory Price (SGD) %>" />
            <asp:BoundField DataField="CostOfGoods" SortExpression="CostOfGoods" HeaderText="<%$Resources:dictionary,Cost of Goods (SGD) %>" />
            <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="<%$Resources:dictionary, Remark%>" />
            <asp:BoundField DataField="ItemRemark" SortExpression="ItemRemark" HeaderText="<%$Resources:dictionary, Line Remark%>" />
        </Columns>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
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
