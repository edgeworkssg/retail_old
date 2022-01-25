<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="CounterCloseLogReport" Title="<%$Resources:dictionary,Collection Report %>"
    CodeBehind="CounterCloseLogReport.aspx.cs" %>

<%@ Register Src="../CustomControl/OutletDropdownList.ascx" TagName="OutletDropdownList"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
        var newwindow;
        function poptastic(url) {
            newwindow = window.open(url, 'name', 'height=1000,width=850,resizeable=1,scrollbars=1');
            if (window.focus) { newwindow.focus() }
        }
    </script>

    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <asp:Panel ID="pnlSearch" runat="server">
        <table width="600px" id="FilterTable">
            <tr style="height: 20px;">
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
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Department %>"></asp:Literal>
                </td>
                <td>
                    <subsonic:DropDown ID="ddDept" runat="server" PromptValue="" TableName="Department"
                        TextField="DepartmentName" ValueField="DepartmentID" Width="172px" OnInit="ddDept_Init">
                    </subsonic:DropDown>
                </td>
                <td style="width: 127px">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <uc1:OutletDropdownList ID="OutletDropdownList" runat="server"></uc1:OutletDropdownList>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Supervisor %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtSupervisor" runat="server" Width="172px"></asp:TextBox>
                </td>
                <td style="width: 127px">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Cashier ID %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtCashier" runat="server" Width="173px"></asp:TextBox>
                </td>
            </tr>
            <%-- <tr><td colspan=2 >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td colspan=2 align="right" class="ExportButton">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr> --%>
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
                        <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>
                </td>
                <td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                    vertical-align: middle; right: 0px;">
                    <asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                        <asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 25px;">
                    <asp:Literal ID="litMessage" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                    <asp:Label ID="txtMissingDate" ForeColor="Red" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlGrid" runat="server">
        <asp:GridView ID="gvReport" Width="1000px" runat="server" PageSize="20" AllowPaging="True"
            AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
            OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
            SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" ShowFooter="True">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="CounterCloseID"
                    DataNavigateUrlFormatString="CounterCloseLogReport.aspx?id={0}" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <a id="hyperLink1" href="javascript:poptastic('CollectionReport.aspx?id=<%# Eval("CounterCloseID")%>');">
                            <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,View %>"></asp:Literal></a></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a id="hyperLink1" href="javascript:poptastic('TransactionReport.aspx?POS=<%# Eval("PointOfSaleID")%>&Outlet=<%# Eval("OutletName")%>&Start=<%# Eval("StartTime")%>&End=<%# Eval("EndTime")%>');">
                            <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,View Details%>"></asp:Literal></a></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="OutletName" SortExpression="OutletName" HeaderText="<%$ Resources:dictionary,Outlet %>" />
                <asp:BoundField DataField="PointOfSaleName" SortExpression="PointOfSaleName" HeaderText="<%$ Resources:dictionary,Point Of Sale %>" />
                <asp:BoundField DataField="StartTime" SortExpression="StartTime" HeaderText="<%$ Resources:dictionary,Opening Time %>" />
                <asp:BoundField DataField="EndTime" SortExpression="EndTime" HeaderText="<%$ Resources:dictionary,Closing Time %>" />
                <asp:BoundField DataField="TotalSystemRecorded" SortExpression="TotalSystemRecorded"
                    HeaderText="<%$ Resources:dictionary,Total %>">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="TotalActualCollected" SortExpression="TotalActualCollected"
                    HeaderText="<%$ Resources:dictionary,Collected %>">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="Variance" SortExpression="Variance" HeaderText="<%$ Resources:dictionary,+/- %>" />
                <asp:BoundField DataField="OpeningBalance" SortExpression="OpeningBalance" HeaderText="<%$ Resources:dictionary,Opening %>" />
                <asp:BoundField DataField="CashRecorded" HeaderText="<%$ Resources:dictionary,Cash %>"
                    SortExpression="CashRecorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="CashCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="CashCollected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="CashIn" SortExpression="CashIn" HeaderText="<%$ Resources:dictionary,Cash In %>" />
                <asp:BoundField DataField="CashOut" SortExpression="CashOut" HeaderText="<%$ Resources:dictionary,Cash Out %>" />
                <asp:BoundField DataField="FloatBalance" SortExpression="FloatBalance" HeaderText="<%$ Resources:dictionary,Closing %>"
                    Visible="false" />
                <asp:BoundField DataField="ClosingCashOut" HeaderText="<%$ Resources:dictionary,Closing Cash Out %>"
                    SortExpression="ClosingCashOut" />
                <asp:BoundField DataField="DepositBagNo" HeaderText="<%$ Resources:dictionary,Deposit Bag # %>"
                    SortExpression="DepositBagNo" />
                <asp:BoundField DataField="TotalForeignCurrency" HeaderText="<%$Resources:dictionary, Total Foreign Currency%>"
                    SortExpression="TotalForeignCurrency" DataFormatString="{0:N2}">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="RecordedAmount1" HeaderText="RecordedAmount1" SortExpression="RecordedAmount1">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="CollectedAmount1" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="CollectedAmount1">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="RecordedAmount2" HeaderText="RecordedAmount2" SortExpression="RecordedAmount2">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="CollectedAmount2" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="CollectedAmount2">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="RecordedAmount3" HeaderText="RecordedAmount3" SortExpression="RecordedAmount3">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="CollectedAmount3" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="CollectedAmount3">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="RecordedAmount4" HeaderText="RecordedAmount4" SortExpression="RecordedAmount4">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="CollectedAmount4" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="CollectedAmount4">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="RecordedAmount5" HeaderText="RecordedAmount5" SortExpression="RecordedAmount5">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="CollectedAmount5" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="CollectedAmount5">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="NetsRecorded" HeaderText="<%$ Resources:dictionary,NETS %>"
                    SortExpression="NetsRecorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="NetsCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="NetsCollected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="NetsCashCardRecorded" HeaderText="Nets Cash Card" SortExpression="NetsCashCard"
                    DataFormatString="{0:N2}">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="NetsCashCardCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="NetsCashCardCollected" DataFormatString="{0:N2}">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="NetsFlashPayRecorded" HeaderText="Nets Flash Pay" SortExpression="NetsFlashPayRecorded"
                    DataFormatString="{0:N2}">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="NetsFlashPayCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="NetsFlashPayCollected" DataFormatString="{0:N2}">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="NetsATMCardRecorded" HeaderText="Nets ATM Card" SortExpression="NetsATMCardRecorded"
                    DataFormatString="{0:N2}">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="NetsATMCardCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="NetsATMCardCollected" DataFormatString="{0:N2}">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="VisaRecorded" HeaderText="<%$ Resources:dictionary,Visa/Master %>"
                    SortExpression="VisaRecorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="VisaCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="VisaCollected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="ChinaNetsRecorded" HeaderText="<%$ Resources:dictionary,China Nets %>"
                    SortExpression="ChinaNetsRecorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="ChinaNetsCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="ChinaNetsCollected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="AmexRecorded" HeaderText="<%$ Resources:dictionary,AMEX %>"
                    SortExpression="AmexRecorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="AmexCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="AmexCollected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="Payment5Recorded" HeaderText="Payment5" SortExpression="Payment5Recorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="Payment5Collected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="Payment5Collected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="Payment6Recorded" HeaderText="Payment6" SortExpression="Payment6Recorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="Payment6Collected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="Payment6Collected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="Payment7Recorded" HeaderText="Payment7" SortExpression="Payment7Recorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="Payment7Collected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="Payment7Collected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="Payment8Recorded" HeaderText="Payment8" SortExpression="Payment8Recorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="Payment8Collected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="Payment8Collected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="Payment9Recorded" HeaderText="Payment9" SortExpression="Payment9Recorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="Payment9Collected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="Payment9Collected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="Payment10Recorded" HeaderText="Payment10" SortExpression="Payment10Recorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="Payment10Collected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="Payment10Collected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="VoucherRecorded" HeaderText="<%$ Resources:dictionary,Voucher %>"
                    SortExpression="VoucherRecorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="VoucherCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="VoucherCollected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="ChequeRecorded" HeaderText="<%$ Resources:dictionary,Cheque %>"
                    SortExpression="ChequeRecorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="ChequeCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="ChequeCollected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="ChequeRecorded" HeaderText="<%$ Resources:dictionary,Cheque %>"
                    SortExpression="ChequeRecorded" Visible="false">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="SMFRecorded" HeaderText="SMF" SortExpression="SMFRecorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="SMFCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="SMFCollected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="PAMedRecorded" HeaderText="PAMedifund" SortExpression="PAMedRecorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="PAMedCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="PAMedCollected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="PWFRecorded" HeaderText="PWF" SortExpression="PWFRecorded">
                    <ItemStyle BackColor="#80FFFF" />
                    <HeaderStyle BackColor="#004040" />
                </asp:BoundField>
                <asp:BoundField DataField="PWFCollected" HeaderText="<%$ Resources:dictionary,Collected %>"
                    SortExpression="PWFCollected">
                    <ItemStyle BackColor="#80FF80" />
                    <HeaderStyle BackColor="#004000" />
                </asp:BoundField>
                <asp:BoundField DataField="PointRecorded" HeaderText="<%$ Resources:dictionary,Point Recorded %>" />
                <asp:BoundField DataField="Cashier" SortExpression="Cashier" HeaderText="<%$ Resources:dictionary,Cashier %>" />
                <asp:BoundField DataField="Supervisor" SortExpression="Supervisor" HeaderText="<%$ Resources:dictionary,Supervisor %>" />
                <asp:BoundField AccessibleHeaderText="TotalGST" DataField="TotalGST" HeaderText="TotalGST" />
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
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, (No Data Found)%>" /></EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel Visible="false" ID="pnlChart" runat="server">
        <iframe id="iframe2" runat="server" width="600" height="400" />
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" onkeydown="return DisableEnterKey()">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table cellpadding="5" cellspacing="0" width="800" id="FieldsTable1">
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,ID %>"></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="lblCounterClose" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="lblOutlet" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="LblPOSID" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:dictionary,Opening Time %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblOpeningTime" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary,Closing Time %>"></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="lblClosingTime" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary,Cash In %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtCashIn" runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtCashIn"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal16" runat="server" Text="<%$Resources:dictionary,Cash out %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtCashOut" runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtCashOut"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal18" runat="server" Text="<%$Resources:dictionary,Deposit Bag # %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDepositBag" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr id="divCashHeader" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal29" runat="server" Text="<%$Resources:dictionary,Cash %>"></asp:Literal>
                </td>
            </tr>
            <tr id="divCashRecorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal25" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblCashRecorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divCashCollected" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal15" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtCash" runat="server" onchange="countVariance()" ></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtCash"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divCashSurplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal27" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblCashSurplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divNetsAtmCardHeader" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal26" runat="server" Text="Nets ATM Card"></asp:Literal>
                </td>
            </tr>
            <tr id="divNetsAtmCardRecorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal28" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblNetsAtmCardRecorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divNetsATMCard" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal17" runat="server" Text="<%$Resources:dictionary, Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtNetsAtmCard" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtNetsAtmCard"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divNetsAtmCardSurplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal135" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblNetsAtmCardSurplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divNetsCashCardHeader" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal31" runat="server" Text="Nets Cash Card"></asp:Literal>
                </td>
            </tr>
            <tr id="divNetsCashCardRecorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal32" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblNetsCashCardRecorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divNetsCashCard" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal19" runat="server" Text="<%$Resources:dictionary, Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtNetsCashCard" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" ControlToValidate="txtNetsCashCard"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divNetsCashCardSurplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal93" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblNetsCashCardSurplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divNetsFlashPayHeader" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal34" runat="server" Text="Nets Flash Pay"></asp:Literal>
                </td>
            </tr>
            <tr id="divNetsFlashPayRecorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal35" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblNetsFlashPayRecorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divNetsFlashPay" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal21" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtNetsFlashPay" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" ControlToValidate="txtNetsFlashPay"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divNetsFlashPaySurplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal95" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblNetsFlashPaySurplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment1Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblPayment1" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divPayment1Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal37" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment1Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment1" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal24" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPayment1" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" ControlToValidate="txtPayment1"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divPayment1Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal97" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment1Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment2Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblPayment2" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divPayment2Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal40" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment2Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment2" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal39" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPayment2" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" ControlToValidate="txtPayment2"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divPayment2Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal99" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment2Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment3Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblPayment3" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divPayment3Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal43" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment3Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment3" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal42" runat="server" Text="<%$Resources:dictionary,Cash %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPayment3" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator9" ControlToValidate="txtPayment3"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divPayment3Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal101" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment3Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment4Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblPayment4" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divPayment4Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal46" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment4Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment4" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal45" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPayment4" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator10" ControlToValidate="txtPayment4"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divPayment4Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal103" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment4Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment5Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblPayment5" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divPayment5Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal49" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment5Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment5" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal48" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPayment5" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator11" ControlToValidate="txtPayment5"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divPayment5Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal105" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment5Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment6Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblPayment6" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divPayment6Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal52" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment6Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment6" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal51" runat="server"  Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPayment6" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator12" ControlToValidate="txtPayment6"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divPayment6Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal107" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment6Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment7Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblPayment7" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divPayment7Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal55" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment7Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment7" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal54" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPayment7" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator13" ControlToValidate="txtPayment7"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divPayment7Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal109" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment7Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment8Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblPayment8" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divPayment8Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal58" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment8Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment8" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal57" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPayment8" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator14" ControlToValidate="txtPayment8"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divPayment8Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal111" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment8Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment9Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblPayment9" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divPayment9Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal61" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment9Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment9" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal60" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPayment9" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator15" ControlToValidate="txtPayment9"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divPayment9Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal113" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment9Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment10Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblPayment10" runat="server" ></asp:Literal>
                </td>
            </tr>
            <tr id="divPayment10Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal64" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment10Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPayment10" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal63" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPayment10" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator16" ControlToValidate="txtPayment10"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divPayment10Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal115" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPayment10Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divChequeHeader" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal66" runat="server" Text="<%$Resources:dictionary,Cheque %>"></asp:Literal>
                </td>
            </tr>
            <tr id="divChequeRecorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal67" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblChequeRecorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divCheque" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="lblCheque" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtCheque" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator17" ControlToValidate="txtCheque"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divChequeSurplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal117" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblChequeSurplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divVoucherHeader" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal59" runat="server" Text="<%$Resources:dictionary,Voucher %>"></asp:Literal>
                </td>
            </tr>
            <tr id="divVoucherRecorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal41" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblVoucherRecorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divVoucher" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal50" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtVoucher" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator26" ControlToValidate="txtCheque"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divVoucherSurplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal53" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblVoucherSurplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divForeignCurrency1Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblForeignCurrency1" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divForeignCurrency1Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal70" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblForeignCurrency1Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divForeignCurrency1" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal69" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtForeignCurrency1" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator18" ControlToValidate="txtForeignCurrency1"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divForeignCurrency1Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal119" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblForeignCurrency1Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divForeignCurrency2Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblForeignCurrency2" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divForeignCurrency2Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal73" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblForeignCurrency2Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divForeignCurrency2" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal72" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtForeignCurrency2" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator19" ControlToValidate="txtForeignCurrency2"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divForeignCurrency2Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal121" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblForeignCurrency2Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divForeignCurrency3Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblForeignCurrency3" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divForeignCurrency3Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal76" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblForeignCurrency3Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divForeignCurrency3" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal75" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtForeignCurrency3" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator20" ControlToValidate="txtForeignCurrency3"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divForeignCurrency3Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal123" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblForeignCurrency3Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divForeignCurrency4Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblForeignCurrency4" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divForeignCurrency4Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal79" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblForeignCurrency4Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divForeignCurrency4" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal78" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtForeignCurrency4" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator21" ControlToValidate="txtForeignCurrency4"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divForeignCurrency4Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal125" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblForeignCurrency4Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divForeignCurrency5Header" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="lblForeignCurrency5" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="divForeignCurrency5Recorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal82" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblForeignCurrency5Recorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divForeignCurrency5" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal81" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtForeignCurrency5" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator22" ControlToValidate="txtForeignCurrency5"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divForeignCurrency5Surplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal127" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblForeignCurrency5Surplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divSMFMedHeader" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal84" runat="server" Text="SMF"></asp:Literal>
                </td>
            </tr>
            <tr id="divSMFMedRecorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal85" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblSMFMedRecorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divSMFMed" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal20" runat="server"  Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtSMFMed" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator23" ControlToValidate="txtSMF"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divSMFMedSurplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal129" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblSMFMedSurplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPAMedHeader" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal87" runat="server" Text="PA Medifund"></asp:Literal>
                </td>
            </tr>
            <tr id="divPAMedRecorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal88" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPAMedRecorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPAMed" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal22" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPAMed" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator24" ControlToValidate="txtPAMed"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divPAMedSurplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal131" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPAMedSurplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPWFHeader" runat="server">
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal90" runat="server" Text="PWF"></asp:Literal>
                </td>
            </tr>
            <tr id="divPWFRecorded" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal91" runat="server" Text="<%$Resources:dictionary,Amount %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPWFRecorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr id="divPWF" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal23" runat="server" Text="<%$Resources:dictionary,Collected %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPWF" runat="server" onchange="countVariance()"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator25" ControlToValidate="txtPWF"
                        runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[-+]?\d+(\.\d+)?$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="divPWFSurplus" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal133" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblPWFSurplus" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal30" runat="server" Text="<%$Resources:dictionary, Total %>"></asp:Literal>
                </td>
            </tr>
             <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal36" runat="server" Text="<%$ Resources:dictionary,Opening Balance %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblOpeningBalance" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
             <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal33" runat="server" Text="<%$ Resources:dictionary,Total %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblTotalSystemRecorded" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
             <tr id="Tr2" runat="server">
                <td style="width: 150px">
                    <asp:Literal ID="Literal38" runat="server" Text="<%$ Resources:dictionary, Collected%>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblTotalActualCollected" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
             <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal44" runat="server" Text="<%$ Resources:dictionary,+/- %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblVariance" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            
            <tr>
                <td align="left" colspan="4">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        Text="<%$ Resources:dictionary, Save %>" />
                    <asp:Button ID="btnReturn" runat="server" CssClass="classname" Text="<%$Resources:dictionary, Return%>"
                        OnClick="btnReturn_Click" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/AccountingJs/accounting.min.js")%>"></script>

    <script type="text/javascript">
        function DisableEnterKey() {
            if (event.keyCode == 13) {
                return false;
            }
            else {
                return true;
            }
        }

        function countVariance() {

            var openingbalance = Number($("#ctl00_ContentPlaceHolder1_lblOpeningBalance").html().replace(/[,]/g, ""));
            var TotalSystemRecorded = Number($("#ctl00_ContentPlaceHolder1_lblTotalSystemRecorded").html().replace(/[,]/g, ""));

            var totalActualCollected = 0;
            var Cash = Number($("#ctl00_ContentPlaceHolder1_txtCash").val().replace(/[,]/g, ""));
            var CashRecorded = Number($("#ctl00_ContentPlaceHolder1_lblCashRecorded").html().replace(/[,]/g, ""));
            $("#ctl00_ContentPlaceHolder1_lblCashSurplus").html(accounting.formatMoney(Cash - CashRecorded, "", 2, ",", "."));
            totalActualCollected = totalActualCollected + Cash;

            if ($('#ctl00_ContentPlaceHolder1_divNetsATMCard').length) {
                var NetsATMCard = Number($("#ctl00_ContentPlaceHolder1_txtNetsAtmCard").val().replace(/[,]/g, ""));
                var NetsATMCardRecorded = Number($("#ctl00_ContentPlaceHolder1_lblNetsAtmCardRecorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblNetsAtmCardSurplus").html(accounting.formatMoney(NetsATMCard - NetsATMCardRecorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + NetsATMCard;
            }

            if ($('#ctl00_ContentPlaceHolder1_divNetsCashCard').length) {
                var NetsCashCard = Number($("#ctl00_ContentPlaceHolder1_txtNetsCashCard").val().replace(/[,]/g, ""));
                var NetsCashCardRecorded = Number($("#ctl00_ContentPlaceHolder1_lblNetsCashCardRecorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblNetsCashCardSurplus").html(accounting.formatMoney(NetsCashCard - NetsCashCardRecorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + NetsCashCard;
            }


            if ($('#ctl00_ContentPlaceHolder1_divNetsFlashPay').length) {
                var NetsFlashPay = Number($("#ctl00_ContentPlaceHolder1_txtNetsFlashPay").val().replace(/[,]/g, ""));
                var NetsFlashPayRecorded = Number($("#ctl00_ContentPlaceHolder1_lblNetsFlashPayRecorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblNetsFlashPaySurplus").html(accounting.formatMoney(NetsFlashPay - NetsFlashPayRecorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + NetsFlashPay;
            }

            if ($('#ctl00_ContentPlaceHolder1_divPayment1').length) {
                var Payment1 = Number($("#ctl00_ContentPlaceHolder1_txtPayment1").val().replace(/[,]/g, ""));
                var Payment1Recorded = Number($("#ctl00_ContentPlaceHolder1_lblPayment1Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblPayment1Surplus").html(accounting.formatMoney(Payment1 - Payment1Recorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + Payment1;
            }

            if ($('#ctl00_ContentPlaceHolder1_divPayment2').length) {
                var Payment2 = Number($("#ctl00_ContentPlaceHolder1_txtPayment2").val().replace(/[,]/g, ""));
                var Payment2Recorded = Number($("#ctl00_ContentPlaceHolder1_lblPayment2Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblPayment2Surplus").html(accounting.formatMoney(Payment2 - Payment2Recorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + Payment2;
            }

            if ($('#ctl00_ContentPlaceHolder1_divPayment3').length) {
                var Payment3 = Number($("#ctl00_ContentPlaceHolder1_txtPayment3").val().replace(/[,]/g, ""));
                var Payment3Recorded = Number($("#ctl00_ContentPlaceHolder1_lblPayment3Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblPayment3Surplus").html(accounting.formatMoney(Payment3 - Payment3Recorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + Payment3;
            }

            if ($('#ctl00_ContentPlaceHolder1_divPayment4').length) {
                var Payment4 = Number($("#ctl00_ContentPlaceHolder1_txtPayment4").val().replace(/[,]/g, ""));
                var Payment4Recorded = Number($("#ctl00_ContentPlaceHolder1_lblPayment4Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblPayment4Surplus").html(accounting.formatMoney(Payment4 - Payment4Recorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + Payment4;
            }


            if ($('#ctl00_ContentPlaceHolder1_divPayment5').length) {
                var Payment5 = Number($("#ctl00_ContentPlaceHolder1_txtPayment5").val().replace(/[,]/g, ""));
                var Payment5Recorded = Number($("#ctl00_ContentPlaceHolder1_lblPayment5Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblPayment5Surplus").html(accounting.formatMoney(Payment5 - Payment5Recorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + Payment5;
            }

            if ($('#ctl00_ContentPlaceHolder1_divPayment6').length) {
                var Payment6 = Number($("#ctl00_ContentPlaceHolder1_txtPayment6").val().replace(/[,]/g, ""));
                var Payment6Recorded = Number($("#ctl00_ContentPlaceHolder1_lblPayment6Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblPayment6Surplus").html(accounting.formatMoney(Payment6 - Payment6Recorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + Payment6;
            }

            if ($('#ctl00_ContentPlaceHolder1_divPayment7').length) {
                var Payment7 = Number($("#ctl00_ContentPlaceHolder1_txtPayment7").val().replace(/[,]/g, ""));
                var Payment7Recorded = Number($("#ctl00_ContentPlaceHolder1_lblPayment7Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblPayment7Surplus").html(accounting.formatMoney(Payment7 - Payment7Recorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + Payment7;
            }

            if ($('#ctl00_ContentPlaceHolder1_divPayment8').length) {
                var Payment8 = Number($("#ctl00_ContentPlaceHolder1_txtPayment8").val().replace(/[,]/g, ""));
                var Payment8Recorded = Number($("#ctl00_ContentPlaceHolder1_lblPayment8Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblPayment8Surplus").html(accounting.formatMoney(Payment8 - Payment8Recorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + Payment8;
            }

            if ($('#ctl00_ContentPlaceHolder1_divPayment9').length) {
                var Payment9 = Number($("#ctl00_ContentPlaceHolder1_txtPayment9").val().replace(/[,]/g, ""));
                var Payment9Recorded = Number($("#ctl00_ContentPlaceHolder1_lblPayment9Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblPayment9Surplus").html(accounting.formatMoney(Payment9 - Payment9Recorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + Payment9;
            }

            if ($('#ctl00_ContentPlaceHolder1_divPayment10').length) {
                var Payment10 = Number($("#ctl00_ContentPlaceHolder1_txtPayment10").val().replace(/[,]/g, ""));
                var Payment10Recorded = Number($("#ctl00_ContentPlaceHolder1_lblPayment10Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblPayment10Surplus").html(accounting.formatMoney(Payment10 - Payment10Recorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + Payment10;
            }

            if ($('#ctl00_ContentPlaceHolder1_divCheque').length) {
                var Cheque = Number($("#ctl00_ContentPlaceHolder1_txtCheque").val().replace(/[,]/g, ""));
                var ChequeRecorded = Number($("#ctl00_ContentPlaceHolder1_lblChequeRecorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblChequeSurplus").html(accounting.formatMoney(Cheque - ChequeRecorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + Cheque;
            }

            if ($('#ctl00_ContentPlaceHolder1_divVoucher').length) {
                var Voucher = Number($("#ctl00_ContentPlaceHolder1_txtVoucher").val().replace(/[,]/g, ""));
                var VoucherRecorded = Number($("#ctl00_ContentPlaceHolder1_lblVoucherRecorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblVoucherSurplus").html(accounting.formatMoney(Voucher - VoucherRecorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + Voucher;
            }


            if ($('#ctl00_ContentPlaceHolder1_divForeignCurrency1').length) {
                var ForeignCurrency1 = Number($("#ctl00_ContentPlaceHolder1_txtForeignCurrency1").val().replace(/[,]/g, ""));
                var ForeignCurrency1Recorded = Number($("#ctl00_ContentPlaceHolder1_lblForeignCurrency1Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblForeignCurrency1Surplus").html(accounting.formatMoney(ForeignCurrency1 - ForeignCurrency1Recorded, "", 2, ",", "."));
            }

            if ($('#ctl00_ContentPlaceHolder1_divForeignCurrency2').length) {
                var ForeignCurrency2 = Number($("#ctl00_ContentPlaceHolder1_txtForeignCurrency2").val().replace(/[,]/g, ""));
                var ForeignCurrency2Recorded = Number($("#ctl00_ContentPlaceHolder1_lblForeignCurrency2Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblForeignCurrency2Surplus").html(accounting.formatMoney(ForeignCurrency2 - ForeignCurrency2Recorded, "", 2, ",", "."));
            }

            if ($('#ctl00_ContentPlaceHolder1_divForeignCurrency3').length) {
                var ForeignCurrency3 = Number($("#ctl00_ContentPlaceHolder1_txtForeignCurrency3").val().replace(/[,]/g, ""));
                var ForeignCurrency3Recorded = Number($("#ctl00_ContentPlaceHolder1_lblForeignCurrency3Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblForeignCurrency3Surplus").html(accounting.formatMoney(ForeignCurrency3 - ForeignCurrency3Recorded, "", 2, ",", "."));
            }

            if ($('#ctl00_ContentPlaceHolder1_divForeignCurrency4').length) {
                var ForeignCurrency4 = Number($("#ctl00_ContentPlaceHolder1_txtForeignCurrency4").val().replace(/[,]/g, ""));
                var ForeignCurrency4Recorded = Number($("#ctl00_ContentPlaceHolder1_lblForeignCurrency4Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblForeignCurrency4Surplus").html(accounting.formatMoney(ForeignCurrency4 - ForeignCurrency4Recorded, "", 2, ",", "."));
            }


            if ($('#ctl00_ContentPlaceHolder1_divForeignCurrency5').length) {
                var ForeignCurrency5 = Number($("#ctl00_ContentPlaceHolder1_txtForeignCurrency5").val().replace(/[,]/g, ""));
                var ForeignCurrency5Recorded = Number($("#ctl00_ContentPlaceHolder1_lblForeignCurrency5Recorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblForeignCurrency5Surplus").html(accounting.formatMoney(ForeignCurrency5 - ForeignCurrency5Recorded, "", 2, ",", "."));
            }


            if ($('#ctl00_ContentPlaceHolder1_divSMFMed').length) {
                var SMFMed = Number($("#ctl00_ContentPlaceHolder1_txtSMFMed").val().replace(/[,]/g, ""));
                var SMFMedRecorded = Number($("#ctl00_ContentPlaceHolder1_lblSMFMedRecorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblSMFMedSurplus").html(accounting.formatMoney(SMFMed - SMFMedRecorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + SMFMed;
            }

            if ($('#ctl00_ContentPlaceHolder1_divPAMed').length) {
                var PAMed = Number($("#ctl00_ContentPlaceHolder1_txtPAMed").val().replace(/[,]/g, ""));
                var PAMedRecorded = Number($("#ctl00_ContentPlaceHolder1_lblPAMedRecorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblPAMedSurplus").html(accounting.formatMoney(PAMed - PAMedRecorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + PAMed;
            }


            if ($('#ctl00_ContentPlaceHolder1_divPWF').length) {
                var PWF = Number($("#ctl00_ContentPlaceHolder1_txtPWF").val().replace(/[,]/g, ""));
                var PWFRecorded = Number($("#ctl00_ContentPlaceHolder1_lblPWFRecorded").html().replace(/[,]/g, ""));
                $("#ctl00_ContentPlaceHolder1_lblPWFSurplus").html(accounting.formatMoney(PWF - PWFRecorded, "", 2, ",", "."));
                totalActualCollected = totalActualCollected + PWF;
            }

            var variance = (totalActualCollected - openingbalance) - TotalSystemRecorded;
            $("#ctl00_ContentPlaceHolder1_lblTotalActualCollected").html(accounting.formatMoney(totalActualCollected, "", 2, ",", "."));
            $("#ctl00_ContentPlaceHolder1_lblVariance").html(accounting.formatMoney(variance, "", 2, ",", "."));

        }
    </script>
</asp:Content>
