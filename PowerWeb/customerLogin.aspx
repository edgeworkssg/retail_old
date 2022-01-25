<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="customerLogin.aspx.cs" Inherits="PowerWeb.customerLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Login</title>
    <style type="text/css">
        .style1
        {
            text-align: left;
        }
        .style2
        {
            text-align: left;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table style="width: 251px; margin-right: 0px">
        <tr>
            <td width="50%" class="style2">Card No</td><td class="style1">
            <asp:TextBox ID="txtCardNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">NRIC</td><td class="style1">
            <asp:TextBox ID="txtNRIC" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align=right><b></b></td><td class="style1">
            <asp:Button ID="btnLogin" runat="server" onclick="btnLogin_Click" 
                Text="Login" />
            </td>
        </tr>
    </table>
    </div>
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
    </form>
</body>
</html>
