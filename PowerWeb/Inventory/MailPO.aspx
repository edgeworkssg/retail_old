<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MailPO.aspx.cs" Inherits="PowerWeb.Inventory.MailPO" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Mail PO%>" /></title>
    <link href="~/App_Themes/Default/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="~/Dashboard/Script/dashboard.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 31px;
        }
        .style2
        {
            width: 150px;
            height: 23px;
        }
        .style3
        {
            width: 5px;
            height: 23px;
        }
        .style4
        {
            height: 23px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding: 20px">
        <table style="width: 600px">
            <tr>
                <td colspan="3" align="center" style="padding: 5px; background-color: #C0C0C0;" 
                    class="style1">
                    <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, PO Ref No :%>" />
                    <asp:Label ID="lblPORefNo" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblStatus" runat="server" />                
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Mail To Supplier%>" />
                </td>
                <td style="width: 50px">
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtMail" runat="server" Width="350px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, CC%>" />
                </td>
                <td style="width: 50px">
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtCC" runat="server" Width="350px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Subject%>" />
                </td>
                <td style="width: 50px">
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtSubject" runat="server" Width="350px"></asp:TextBox>
                </td>
            </tr>            
            <tr>
                <td colspan="3">
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Message%>" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Width="600px" Height="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="right">
                    <asp:Button ID="btnSend" runat="server" class="classname" Text="<%$Resources:dictionary, Send%>" 
                        Width="75px" onclick="btnSend_Click" />
                    <%--<asp:Button ID="btnCancel" runat="server" class="classname" Text="Cancel" />--%>
                    <input type="button" class="classname" value="Cancel" onclick="javascript:window.close();" Width="75px" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divReport" runat="server">
        <CR:CrystalReportViewer ID="crReport" runat="server" DisplayGroupTree="False" Width="100%"
            Height="50px" HasCrystalLogo="False" HasExportButton="False" HasViewList="False"
            PrintMode="Pdf" />
    </div>    
    </form>
</body>
</html>
