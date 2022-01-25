<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditLineSalesPerson.aspx.cs" Inherits="PowerWeb.SalesPerson.EditLineSalesPerson" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:DropDownList ID="ddSalesPerson" runat="server" Height="32px" Width="166px">
        </asp:DropDownList>        
        <br />
        <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />
        <br />
    <asp:Label ID="lblMessage"  runat="server" Font-Bold="True" ForeColor="#009900"></asp:Label>
    </div>
    </form>
</body>
</html>
