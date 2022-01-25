<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="DeliveryReport.aspx.cs" Inherits="PowerWeb.Reports.DeliveryReport"
    Title="Delivery Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="1000px" id="FilterTable">
        <tr style="height: 20px;">
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="width: 105px">
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$ Resources:dictionary, Start Date %>" />
            </td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                &nbsp;
            </td>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
            </td>
        </tr>
    </table>
    <table id="search_ExportTable" style="vertical-align: middle; width: 1000px; height: 40px;"
        border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="height: 30px; width: 50%; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                <asp:LinkButton ID="btnSearch" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton><div class="divider">
                </div>
                <asp:LinkButton ID="btnClear" class="classname" runat="server" OnClick="btnClear_Click">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton>
            </td>
            <td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                vertical-align: middle; right: 0px;">
                <asp:LinkButton ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click">
                    <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="height: 25px;">
                <asp:Literal ID="litMessage" runat="server" Text="" />
            </td>
        </tr>
    </table>
    <div id="divGrid" runat="server" style="width: 1000px; overflow-x: scroll;" >
        <asp:GridView ID="gvReport" runat="server" ShowFooter="True" AllowPaging="True" AllowSorting="False"
            OnSorting="gvReport_Sorting" AutoGenerateColumns="false"
            OnPageIndexChanging="gvReport_PageIndexChanging" SkinID="scaffold" 
            PageSize="20" ondatabound="gvReport_DataBound" OnRowDataBound="gvReport_RowDataBound">
            <Columns>
                <asp:BoundField DataField="DONumber" SortExpression="DONumber" HeaderText="DO Num"  />
                <asp:BoundField DataField="DODate" SortExpression="DODate" HeaderText="DO Date" />
                <asp:BoundField DataField="CustomerOrderNo" SortExpression="CustomerOrderNo"  HeaderText="Customer Order No" />
                <asp:BoundField DataField="InvoiceNo" SortExpression="InvoiceNo"  HeaderText="Invoice Num" />
                <asp:BoundField DataField="InvoiceDate" SortExpression="InvoiceDate" HeaderText="Invoice Date" />
                <asp:BoundField DataField="SalesmanName" SortExpression="SalesmanName" HeaderText="Salesman Name" />
                <asp:BoundField DataField="PaymentTerm" SortExpression="PaymentTerm" HeaderText="Payment Term" />
                <asp:BoundField DataField="SalesOffice" SortExpression="SalesOffice" HeaderText="Sales Office" />
                <asp:BoundField DataField="CustomerCode" SortExpression="CustomerCode" HeaderText="Customer Code" />
                <asp:BoundField DataField="SalesOrder" SortExpression="SalesOrder" HeaderText="Sales Order" />                
                <asp:BoundField DataField="CustomerTelNo" SortExpression="CustomerTelNo" HeaderText="Customer Tel No" />
                <asp:BoundField DataField="CustomerPostalCode" SortExpression="CustomerPostalCode" HeaderText="Customer Postal Code" />
                <asp:BoundField DataField="CustomerName1" SortExpression="CustomerName1" HeaderText="Customer Name 1" />
                <asp:BoundField DataField="INVToAddrs1" SortExpression="INVToAddrs1" HeaderText="INVToAddrs1" />
                <asp:BoundField DataField="INVToAddrs2" SortExpression="INVToAddrs2" HeaderText="INVToAddrs2" />
                <asp:BoundField DataField="INVToAddrs3" SortExpression="INVToAddrs3" HeaderText="INVToAddrs3" />
                <asp:BoundField DataField="INVToAddrs4" SortExpression="INVToAddrs4" HeaderText="INVToAddrs4" />
                <asp:BoundField DataField="INVToAddrs5" SortExpression="INVToAddrs5" HeaderText="INVToAddrs5" />
                <asp:BoundField DataField="INVToAddrs6" SortExpression="INVToAddrs6" HeaderText="INVToAddrs6" />
                <asp:BoundField DataField="DeliveryToName1" SortExpression="DeliveryToName1" HeaderText="DeliveryToName1" />
                <asp:BoundField DataField="DELToAddrs1" SortExpression="DELToAddrs1" HeaderText="DELToAddrs1" />
                <asp:BoundField DataField="DELToAddrs2" SortExpression="DELToAddrs2" HeaderText="DELToAddrs2" />
                <asp:BoundField DataField="DELToAddrs3" SortExpression="DELToAddrs3" HeaderText="DELToAddrs3" />
                <asp:BoundField DataField="DELToAddrs4" SortExpression="DELToAddrs4" HeaderText="DELToAddrs4" />
                <asp:BoundField DataField="DELToAddrs5" SortExpression="DELToAddrs5" HeaderText="DELToAddrs5" />
                <asp:BoundField DataField="DELToAddrs6" SortExpression="DELToAddrs6" HeaderText="DELToAddrs6" />
                <asp:BoundField DataField="No" SortExpression="No" HeaderText="No" />
                <asp:BoundField DataField="ItemCode" SortExpression="ItemCode" HeaderText="ItemCode" />
                <asp:BoundField DataField="WarehouseCode" SortExpression="WarehouseCode" HeaderText="WarehouseCode" />
                <asp:BoundField DataField="Qty" SortExpression="Qty" HeaderText="Qty" />
                <asp:BoundField DataField="UOM" SortExpression="UOM" HeaderText="UOM" />
                <asp:BoundField DataField="CURR" SortExpression="CURR" HeaderText="CURR" />
                <asp:BoundField DataField="UnitPrice" SortExpression="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="DISCPercent" SortExpression="DISCPercent" HeaderText="DISC Percent" />
                <asp:BoundField DataField="AmountAfterLineDisc" SortExpression="AmountAfterLineDisc" HeaderText="Amount After Line Disc" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="OutstandingBalance" SortExpression="OutstandingBalance" HeaderText="Outstanding Balance" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="SubTotalAmount" SortExpression="SubTotalAmount" HeaderText="Sub Total Amount" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="OrderDiscPercent" SortExpression="OrderDiscPercent" HeaderText="Order Disc Percent" />
                <asp:BoundField DataField="OrderDiscAmount" SortExpression="OrderDiscAmount" HeaderText="Order Disc Amount" />
                <asp:BoundField DataField="AmountAfterDisc" SortExpression="AmountAfterDisc" HeaderText="Amount After Disc" />
                <asp:BoundField DataField="GSTAmount" SortExpression="GSTAmount" HeaderText="GSTAmount" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="TotalINVAmount" SortExpression="TotalINVAmount" HeaderText="TotalINVAmount" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="DepositPaid" SortExpression="DepositPaid" HeaderText="DepositPaid" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="TotalShipmentNum" SortExpression="TotalShipmentNum" HeaderText="Total Shipment Num" />
                <asp:BoundField DataField="ItemDescription1" SortExpression="ItemDescription1" HeaderText="Item Description 1" />
                <asp:BoundField DataField="ItemDescription2" SortExpression="ItemDescription2" HeaderText="Item Description 2" />
                <asp:BoundField DataField="ItemDescription3" SortExpression="ItemDescription3" HeaderText="Item Description 3" />
                <asp:BoundField DataField="ItemLineText1" SortExpression="ItemLineText1" HeaderText="Item Line Text 1" />
                <asp:BoundField DataField="ItemLineText2" SortExpression="ItemLineText2" HeaderText="Item Line Text 2" />
                <asp:BoundField DataField="ItemLineText3" SortExpression="ItemLineText3" HeaderText="Item Line Text 3" />
                <asp:BoundField DataField="FooterText1" SortExpression="FooterText1" HeaderText="Footer Text 1" />
                <asp:BoundField DataField="FooterText2" SortExpression="FooterText2" HeaderText="Footer Text 2" />                
                <asp:BoundField DataField="FooterText3" SortExpression="FooterText3" HeaderText="Footer Text 3" />
                <asp:BoundField DataField="FooterText4" SortExpression="FooterText4" HeaderText="Footer Text 4" />
                <asp:BoundField DataField="FooterText5" SortExpression="FooterText5" HeaderText="Footer Text 5" />
                <asp:BoundField DataField="FooterText6" SortExpression="FooterText6" HeaderText="Footer Text 6" />                
                <asp:BoundField DataField="FooterText7" SortExpression="FooterText7" HeaderText="Footer Text 7" />
                <asp:BoundField DataField="FooterText8" SortExpression="FooterText8" HeaderText="Footer Text 8" />                
                <asp:BoundField DataField="FooterText9" SortExpression="FooterText9" HeaderText="Footer Text 9" />
                <asp:BoundField DataField="FooterText10" SortExpression="FooterText10" HeaderText="Footer Text 10" />                
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
                (No Data Found)</EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>
