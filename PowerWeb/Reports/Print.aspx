<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="Print.aspx.cs" Inherits="PowerWeb.Reports.Print" Title="Print Form" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
    function checkfields() {
        var r = document.getElementById("<%= receipttxt.ClientID %>").value;
        var n = document.getElementById("<%= noctxt.ClientID %>").value;

        if (r == "" || n=="") {
            alert("Please Enter Both Field Values");
            return false;
        }
        return true;
    }

   

</script>

    <table style="width: 100%">
        <tr>
            <td>
            
                <asp:Label ID="Label1" runat="server" Text="Receipt Number:"></asp:Label>
            
            </td>
            <td>
            
                <asp:TextBox ID="receipttxt" runat="server" Width="160px"></asp:TextBox>
            
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Number of Copies:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="noctxt" runat="server" Width="159px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="printbtn" runat="server" onclick="printbtn_Click" OnClientClick="javascript:checkfields();"  
                    Text="Print" Width="78px"/>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <p>
        &nbsp;<asp:Label ID="exlbl" runat="server"></asp:Label></p>
        <p>
            &nbsp;</asp:Content>
