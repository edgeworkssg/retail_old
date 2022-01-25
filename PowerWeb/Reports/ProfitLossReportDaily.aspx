<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="DailyProfitLossReport" Title="<%$Resources:dictionary,Daily Profit/Loss Report %>"
    CodeBehind="ProfitLossReportDaily.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
    var newwindow;
    function poptastic(url)
    {
	    newwindow=window.open(url,'name','height=700,width=650,resizeable=1,scrollbars=1');
	    if (window.focus) {newwindow.focus()}
    }
    </script>

    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtDate">
    </cc1:CalendarExtender>
    &nbsp;
    <table width="600px" id="FilterTable">
        <tr style="height: 20px;">
            <td colspan="4" class="wl_pageheaderSub" style="height: 20px;">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;<asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
            <td style="width: 8px">
                &nbsp;<br />
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
            </td>
            <td>
                <asp:DropDownList ID="ddlOutlet" runat="server" Width="179px">
                </asp:DropDownList>
            </td>
            <td style="width: 8px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Include Pre Order %>"></asp:Literal>
            </td>
            <td>
                <asp:CheckBox ID="cbIncludePreOrder" Checked="false" runat="server" />
            </td>
            <td style="width: 8px">
            </td>
            <td>
            </td>
        </tr>
        <%--  <tr>
        <td colspan=2 >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td colspan=2 align="right" class="ExportButton">
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
                    <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>
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
    <asp:GridView ID="gvReport" Width="100%" runat="server" AllowPaging="True" AllowSorting="True"
        OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting" OnPageIndexChanging="gvReport_PageIndexChanging"
        AutoGenerateColumns="False" SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound"
        ShowFooter="True">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <a id="HyperLink1" href="javascript:poptastic('../Order/OrderDetail.aspx?id=<%# Eval("OrderHdrID")%>');">
                        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,View %>"></asp:Literal></a></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="GrossSales" HeaderText="<%$Resources:dictionary,Gross Sales %>"
                SortExpression="GrossSales" />
            <asp:BoundField DataField="DiscountSales" HeaderText="<%$Resources:dictionary,Discount %>"
                SortExpression="DiscountSales" />
            <asp:BoundField DataField="DiscountPercentage" HeaderText="<%$Resources:dictionary,Disc (%) %>"
                SortExpression="DiscountPercentage" />
            <asp:BoundField DataField="NettSalesBeforeGST" HeaderText="<%$Resources:dictionary,Nett Sales (Inclusive GST) %>"
                SortExpression="NettSalesBeforeGST" />
            <asp:BoundField DataField="GSTAmount" HeaderText="<%$Resources:dictionary,GST Amount %>"
                SortExpression="GSTAmount" />
            <asp:BoundField DataField="NettSalesAfterGST" HeaderText="<%$Resources:dictionary,Nett Sales (Without GST) %>"
                SortExpression="NettSalesAfterGST" />
            <asp:BoundField DataField="CostofGoodsSold" HeaderText="<%$Resources:dictionary,Cost of Goods Sold %>"
                SortExpression="CostofGoodsSold" />
            <asp:BoundField DataField="ProfitLoss" HeaderText="<%$Resources:dictionary,Profit/Loss %>"
                SortExpression="ProfitLoss" />
            <asp:BoundField DataField="ProfitLossPercentage" HeaderText="<%$Resources:dictionary,Profit/Lost (%) %>"
                SortExpression="ProfitLossPercentage" />
            <asp:BoundField DataField="OutletName" HeaderText="<%$Resources:dictionary,Outlet %>"
                SortExpression="OutletName" />
            <asp:BoundField DataField="InVoiceNo" HeaderText="<%$Resources:dictionary,Ref No %>"
                SortExpression="InVoiceNo" />
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
    <asp:Label ID="lblIncomplete" runat="server" Text="<%$Resources:dictionary,**The report P&L shows as INCOMPLETE because there are inventory item being sold but there are insufficient quantity to deduct the sold item. Kindly check Undeducted Sales Report under Inventory section and adjust the inventory accordingly.%>"
        Style="font-weight: 700; color: #FF0000" Visible="False"></asp:Label></asp:Content>
