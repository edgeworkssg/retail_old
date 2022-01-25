<%@ Page Language="C#" AutoEventWireup="true" Inherits="Order_OrderDetail" Codebehind="OrderDetail.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><asp:Literal ID = "OrderDetailTitle" runat="server" Text="<%$Resources:dictionary,Order Detail %>"></asp:Literal></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <table width="600px" id="FieldsTable">
        <tr>
            <td ><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Order No %>"></asp:Literal></td><td style="width: 141px">
                <asp:Label ID="lblOrderNo" runat="server" Text="-" Width="113px"></asp:Label></td>
            <td ><asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Order Date %>"></asp:Literal></td><td>
                <asp:Label ID="lblOrderDate" runat="server" Text="-" Width="112px"></asp:Label></td>
        </tr>
        <tr>
            <td ><asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary,Cashier %>"></asp:Literal></td><td style="width: 141px">
                <asp:Label ID="lblCashier" runat="server" Text="-" Width="113px"></asp:Label></td>
            <td ><asp:Literal ID = "Literal3" runat="server" Text="<%$Resources:dictionary,Sales Person %>"></asp:Literal></td><td>
                <asp:Label ID="lblSalesPerson" runat="server" Text="-" Width="113px"></asp:Label></td>
        </tr>
        <tr>
            <td  style="height: 18px"><asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal></td><td style="width: 141px; height: 18px">
                <asp:Label ID="lblOutlet" runat="server" Text="-" Width="113px"></asp:Label></td>
            <td ><asp:Literal ID = "Literal5" runat="server" Text="<%$Resources:dictionary,Point of Sale %>"></asp:Literal>s</td><td style="height: 18px">
                <asp:Label ID="lblPOS" runat="server" Text="-" Width="113px"></asp:Label></td>
        </tr>
        <tr>
            <td  style="height: 18px">
                <asp:Literal ID = "Literal6" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal></td>
            <td style="width: 141px; height: 18px">
                <asp:Label ID="lblMembershipNo" runat="server" Text="-" Width="113px"></asp:Label></td>
            <td >
                <asp:Literal ID = "Literal7" runat="server" Text="<%$Resources:dictionary,Membership Name %>"></asp:Literal></td>
            <td style="height: 18px">
                <asp:Label ID="lblMembershipName" runat="server" Text="-" Width="113px"></asp:Label></td>
        </tr>
        <tr>
            <td  style="height: 18px">
                <asp:Literal ID = "Literal8" runat="server" Text="<%$Resources:dictionary,Membership Group %>"></asp:Literal></td>
            <td style="width: 141px; height: 18px">
                <asp:Label ID="lblMembershipGroup" runat="server" Text="-" Width="113px"></asp:Label></td>
            <td  style="width: 82px; height: 18px">
            </td>
            <td style="height: 18px">
            </td>
        </tr>
        <tr>
            <td colspan=4>
            <asp:GridView ID="gvOrder"
            Width="600px" 
            runat="server"                         
            AutoGenerateColumns="False"             
            SkinID="scaffold" OnRowDataBound="gvOrder_RowDataBound">
        <Columns>            
            <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary,Item No %>" />
            <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Item Name %>" />
            <asp:BoundField HeaderText="<%$Resources:dictionary,Qty %>" DataField="Quantity" />
            <asp:BoundField DataField="Price" HeaderText="<%$Resources:dictionary,Unit Price %>" />
            <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:dictionary,Category %>" />
            <asp:BoundField DataField="Disc(%)" HeaderText="<%$Resources:dictionary,Disc %>" />
            <asp:BoundField DataField="Amount" HeaderText="<%$Resources:dictionary,Amount %>" />
            <asp:BoundField DataField="IsPromo" HeaderText="<%$Resources:dictionary,Promo? %>" />
            <asp:BoundField DataField="IsSpecial" HeaderText="<%$Resources:dictionary,Special? %>" />
            <asp:BoundField DataField="IsFreeOfCharge" HeaderText="<%$Resources:dictionary,FOC? %>" />
            <asp:BoundField DataField="IsVoided" HeaderText="<%$Resources:dictionary,Voided? %>" />
            <asp:BoundField DataField="SalesPerson" HeaderText="Sales Person" />
            <asp:BoundField DataField="SalesPerson2" HeaderText="Sales Person2" />
        </Columns>        
    </asp:GridView>
            </td>            
        </tr>
        <tr>
            <td colspan=4>
            <table width=100%><tr><td>
            <br />
        <asp:Label ID="Label4" runat="server" Css Text="Gross Sales&nbsp;" Width="134px"></asp:Label>
                <asp:Label ID="lblGrossSales" runat="server" Width="84px"></asp:Label><br />
        <asp:Label ID="Label2" runat="server" Css Text="Discount&nbsp;" Width="134px"></asp:Label>
                <asp:Label ID="lblTotalDiscount" runat="server" Width="84px"></asp:Label><br />
                <asp:Label ID="Label6" runat="server" Css Text="GST Amount&nbsp;" Width="134px"></asp:Label>
                <asp:Label ID="lblGSTTotal" runat="server" Width="84px"></asp:Label><br />
                <asp:Label ID="Label1" runat="server" Css Text="NETT Total&nbsp;" Width="134px"></asp:Label>
                <asp:Label ID="lblTotal" runat="server" Width="84px"></asp:Label><br />
                <br />    
            <asp:GridView ID="gvReceipt"
            Width="418px" 
            runat="server"             
            AutoGenerateColumns="False"             
            SkinID="scaffold" OnRowDataBound="gvReceipt_RowDataBound" >
        <Columns>            
            <asp:BoundField DataField="PaymentType" HeaderText="<%$Resources:dictionary,Payment Type %>" />
            <asp:BoundField DataField="Amount" HeaderText="<%$Resources:dictionary,Amount %>" />
            <asp:BoundField DataField="Userfld1" HeaderText="<%$Resources:dictionary,Cheque No %>" />
            <asp:BoundField DataField="Userfld2" HeaderText="<%$Resources:dictionary,Bank %>" />
            </Columns>        
        </asp:GridView>
        
                <asp:GridView ID="gvCostOfGoods"
            Width="418px" 
            runat="server"             
            AutoGenerateColumns="False"             
            SkinID="scaffold" OnRowDataBound="gvCostOfGoods_RowDataBound"  >
                    <Columns>
                        <asp:BoundField DataField="ItemNo" HeaderText="<%$ Resources:dictionary,Item No %>" />
                        <asp:BoundField DataField="ItemName" HeaderText="<%$ Resources:dictionary,Item Name %>" />
                        <asp:BoundField DataField="CostOfGoods" HeaderText="<%$ Resources:dictionary,Cost Of Goods %>" />
                        <asp:BoundField DataField="Quantity" HeaderText="<%$ Resources:dictionary,Quantity %>" />
                        <asp:BoundField HeaderText="<%$ Resources:dictionary,Total %>" />
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                <br />
                </td></tr>
                </table>
            </td>            
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
