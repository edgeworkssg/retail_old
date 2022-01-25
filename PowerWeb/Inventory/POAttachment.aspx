<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="POAttachment.aspx.cs" Inherits="PowerWeb.Inventory.POAttachment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblNoAttachment" runat="server" Text="No attachment for this Purchase Order." Visible="false"></asp:Label>
        <asp:GridView ID="gvAttachment" runat="server" Width="100%" SkinID="scaffold" 
            AutoGenerateColumns="False" onrowdatabound="gvAttachment_RowDataBound">
            <Columns>
                <asp:BoundField DataField="FileName" HeaderText="File Name" />
                <asp:BoundField DataField="FileSize" HeaderText="File Size" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="hlDownload" runat="server" NavigateUrl="" Text="Download"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
