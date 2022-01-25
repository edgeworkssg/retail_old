<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberTransactionsReport.aspx.cs" Inherits="MemberTransactionsReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div></div>
    <div>
        <asp:GridView ID="gvTransactions" runat="server" Width="714px" 
            AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" 
            BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <Columns>
                <asp:BoundField DataField="OrderDate" HeaderText="Order Date" 
                    SortExpression="OrderDate" />
                <asp:BoundField DataField="MembershipNo" HeaderText="Membership No" 
                    SortExpression="MembershipNo" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No" 
                    SortExpression="ReceiptNo" />
                <asp:BoundField DataField="PaymentFor" HeaderText="Payment For" 
                    SortExpression="PaymentFor" />
                <asp:BoundField DataField="Credit" HeaderText="Credit" 
                    SortExpression="Credit" />
                <asp:BoundField DataField="Debit" HeaderText="Debit" SortExpression="Debit" />
                <asp:BoundField DataField="Outlet" HeaderText="Outlet" 
                    SortExpression="Outlet" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="#DCDCDC" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
