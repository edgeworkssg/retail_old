<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberTransactionsDetailReport.aspx.cs"
    Inherits="MemberTransactionsDetailReport" Title="<%$ Resources:dictionary, Installment History %>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <div style="height: 20px; width: 650px;" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$ Resources:dictionary, Installment History %>"></asp:Literal>
    </div>
    <table width="650px" id="FilterTable">
        <tr class="wl_lightRaw">
            <td>
                Installment History for
                <asp:Label ID="lblMembership" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr class="wl_darkRaw">
            <td>
                <asp:LinkButton ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <br />
    <div>
        <asp:GridView ID="gvTransactions" runat="server" Width="900px" AutoGenerateColumns="False"
            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
            CellPadding="3" GridLines="Vertical">
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <Columns>
                <asp:BoundField DataField="OrderDate" HeaderText="Order Date" SortExpression="OrderDate"
                    DataFormatString="{0:dd MMM yyyy hh:mm tt}" ItemStyle-Width="180px" />
                <asp:BoundField DataField="MembershipNo" HeaderText="Membership No" SortExpression="MembershipNo"
                    ItemStyle-Width="100px" />
                <asp:BoundField DataField="NameToAppear" HeaderText="Name" SortExpression="NameToAppear" />
                <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No" SortExpression="ReceiptNo" />
                <asp:BoundField DataField="PaymentFor" HeaderText="Payment For" SortExpression="PaymentFor" />
                <asp:BoundField DataField="Credit" HeaderText="Credit" SortExpression="Credit" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="Debit" HeaderText="Debit" SortExpression="Debit" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="OutletName" HeaderText="Outlet" SortExpression="OutletName" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <EmptyDataTemplate>
                <asp:Literal ID="literal5" runat="server" Text="No Installment data has been created yet"></asp:Literal></EmptyDataTemplate>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
