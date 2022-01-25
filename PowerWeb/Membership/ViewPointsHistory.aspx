<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPointsHistory.aspx.cs" Inherits="ViewPointsHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><asp:Literal ID="OrderDetailTitle" runat="server" Text="<%$Resources:dictionary,View Points History %>"></asp:Literal></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="5" cellspacing="0" width="100%">
            <tr>
                <td class="wl_pageheaderSub" style="text-align: center" colspan="2">
                    <asp:Literal ID="Literal30" runat="server" Text="<%$Resources:dictionary,View Points History %>"></asp:Literal>
                </td>
            </tr>
        </table>
        <p style="text-align: right">
            <asp:LinkButton ID="LinkButton1" runat="server" class="classBlue" OnClick="showAll_Click"
                Text="Show All"></asp:LinkButton>
            <asp:LinkButton ID="lnkExport" runat="server" class="classBlue" OnClick="lnkExport_Click"
                Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
        </p>
        <asp:GridView ID="gvReport" Width="100%" runat="server" AutoGenerateColumns="False" SkinID="scaffold">
            <Columns>
                <asp:BoundField DataField="AllocationDate" HeaderText="<%$Resources:dictionary, Date%>" DataFormatString="{0:dd MMM yyyy - HH:mm:ss}" />
                <asp:BoundField DataField="RefNo" HeaderText="<%$Resources:dictionary, Order Ref No%>" />
                <asp:BoundField DataField="Amount" HeaderText="<%$Resources:dictionary, Points Change%>" />
                <asp:BoundField DataField="Balance" HeaderText="<%$Resources:dictionary, Balance%>" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
