<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CampaignReportDetail.aspx.cs"
    Inherits="PowerWeb.Reports.CampaignReportDetail" title="<%$Resources:dictionary, Campaign Report Detail%>" enableEventValidation="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Campaign Report Detail%>" /></title>
</head>
<body>
    <form id="form1" runat="server">
    <h4><asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, PROMO DETAIL%>" /></h4>
    <table width="600px" id="FilterTable">
        <tr>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Promo Code%>"></asp:Literal>
            </td>
            <td style="width: 200px">
                <asp:HiddenField ID="txtPromoID" runat="server" />
                <asp:Literal ID="txtPromoCode" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary, Promo Name%>"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="txtPromoName" runat="server">
                </asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal>
            </td>
            <td style="width: 200px">
                <asp:Literal ID="txtStartDate" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="txtEndDate" runat="server">
                </asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
            </td>
            <td style="width: 200px">
                <asp:Literal ID="txtOutlet" runat="server"></asp:Literal>
                <asp:HiddenField ID="txtSearch" runat="server" />
            </td>
        </tr>
    </table>
    <div>
        <asp:GridView ID="gvReport" Width="100%" ShowFooter="true" runat="server" AllowPaging="True"
            AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
            OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False"
            SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" PageSize="20">
            <Columns>
                <asp:BoundField DataField="ReceiptNo" SortExpression="ReceiptNo" HeaderText="<%$Resources:dictionary, Receipt No%>" />
                <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="<%$Resources:dictionary, Item Name%>" />
                <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="<%$Resources:dictionary, Category Name%>" />
                <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="<%$Resources:dictionary, Qty Order%>"
                    DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="<%$Resources:dictionary, Amount Order%>"
                    DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="PromoCode" SortExpression="PromoCode" HeaderText="<%$Resources:dictionary, Promo Code%>" />
                <asp:BoundField DataField="PromoCampaignName" SortExpression="PromoCampaignName"
                    HeaderText="<%$Resources:dictionary, Campaign Name%>" />
                <asp:BoundField DataField="QtyUsed" SortExpression="QtyUsed" HeaderText="<%$Resources:dictionary, Promo Used in Order%>"
                    DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="OrderDate" SortExpression="OrderDate" HeaderText="<%$Resources:dictionary, Order Date%>"
                    DataFormatString="{0:dd MMM yyyy hh:mm tt}" />
                <asp:BoundField DataField="OutletName" SortExpression="OutletName" HeaderText="<%$Resources:dictionary,Outlet %>" />
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
                <asp:Literal ID="ltrxx"  runat="server" Text="<%$Resources:dictionary, No Data%>" /></EmptyDataTemplate>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
