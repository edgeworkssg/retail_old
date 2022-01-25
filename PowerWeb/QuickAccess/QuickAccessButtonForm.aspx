<%@ Page Language="C#" AutoEventWireup="true" Inherits="QuickAccessButtonForm" Codebehind="QuickAccessButtonForm.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><asp:Literal ID = "OrderDetailTitle" runat="server" Text="Button Setup"></asp:Literal></title>
    <style type="text/css">
        .style1
        {
            font-size: 8pt;
            font-family: "Segoe UI", Verdana, Tahoma;
            width: 369px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">    
    <div>                        
        <table cellpadding="5" cellspacing="0"width="600">
            <tr>
                <td  style="width: 145px">
                    &nbsp;</td>
                <td class="style1">
                    &nbsp;</td>
                <td  rowspan="4" style="width: 287px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td  style="width: 145px">
                    <asp:Literal ID = "Literal2" runat="server" Text="Item Name"></asp:Literal></td>
                <td class="style1">
            <subsonic:DropDown ID="ddItemName" runat="server" ShowPrompt="True" TableName="Item"
                TextField="ItemName" ValueField="ItemNo" Width="429px" PromptText="ALL" 
                        Height="21px">
            </subsonic:DropDown></td>
            </tr>
            <tr>
                <td  style="width: 145px">
                    <asp:Literal ID = "Literal6" runat="server" Text="ForeColor"></asp:Literal></td>
                <td class="style1">
                    <asp:DropDownList ID="ddForeColor" runat="server" Height="26px" Width="428px">
                        <asp:ListItem>Black</asp:ListItem>
                        <asp:ListItem>White</asp:ListItem>
                        <asp:ListItem>Blue</asp:ListItem>
                        <asp:ListItem>Red</asp:ListItem>
                        <asp:ListItem>Green</asp:ListItem>
                        <asp:ListItem>Grey</asp:ListItem>
                        <asp:ListItem>Orange</asp:ListItem>
                        <asp:ListItem>Brown</asp:ListItem>
                        <asp:ListItem>Gold</asp:ListItem>
                        <asp:ListItem>LightBlue</asp:ListItem>
                        <asp:ListItem>LightOrange</asp:ListItem>
                        <asp:ListItem>Purple</asp:ListItem>
                        <asp:ListItem>Yellow</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td  style="width: 145px">
                    <asp:Literal ID = "Literal7" runat="server" Text="Back Color"></asp:Literal></td>
                <td class="style1">
                    <asp:DropDownList ID="ddBackColor" runat="server" Height="35px" Width="427px">
                        <asp:ListItem>White</asp:ListItem>
                        <asp:ListItem>Black</asp:ListItem>
                        <asp:ListItem>Blue</asp:ListItem>
                        <asp:ListItem>Red</asp:ListItem>
                        <asp:ListItem>Green</asp:ListItem>
                        <asp:ListItem>Grey</asp:ListItem>
                        <asp:ListItem>Orange</asp:ListItem>
                        <asp:ListItem>Brown</asp:ListItem>
                        <asp:ListItem>Gold</asp:ListItem>
                        <asp:ListItem>LightBlue</asp:ListItem>
                        <asp:ListItem>LightOrange</asp:ListItem>
                        <asp:ListItem>Purple</asp:ListItem>
                        <asp:ListItem>Yellow</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="3">
                    <asp:Button ID="btnSave" runat="server" CssClass="scaffoldButton" OnClick="btnSave_Click"
                        Text="<%$ Resources:dictionary, Save %>" />&nbsp;&nbsp; 
                    <asp:Button ID="btnDelete" runat="server" CssClass="scaffoldButton" 
                        Text="Delete" onclick="btnDelete_Click" /> </td>
            </tr>
        </table>		
        <asp:Label ID="lblResult" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>


