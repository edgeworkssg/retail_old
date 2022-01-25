<%@ Page Language="C#" AutoEventWireup="true" Inherits="QuickAccessButtonSetup" Codebehind="QuickAccessButtonSetup.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><asp:Literal ID = "OrderDetailTitle" runat="server" Text="<%$Resources:dictionary,Order Detail %>"></asp:Literal></title>
    <script type="text/javascript" language="javascript">
    function RefreshPage()
        {
            window.document.getElementById("RebindFlagSpan").firstChild.Value = "1";
            window.document.forms(0).submit();
        }
        var newwindow;
        function poptastic(url)
        {
	        newwindow=window.open(url,'name','height=350,width=650,resizeable=1,scrollbars=1');
	        if (window.focus) {newwindow.focus()}
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">    
    <div>
    <span id="RebindFlagSpan">
        <asp:HiddenField ID="RebindFlagHiddenField" runat="server" Value="0" />
    </span>
        <table width="700px">
            <tr><td width="50%"><asp:Label ID="lblCatName" runat="server" Text="Label" 
                    Font-Names="Verdana" Font-Size="Medium"></asp:Label></td><td width="50%" align=right>
                <asp:Button ID="Button1" runat="server" Text="Refresh" /></td></tr>
        </table>        
        <asp:GridView ID="GridView1" runat="server" ShowHeader="False" 
            AutoGenerateColumns="False" onrowdatabound="GridView1_RowDataBound">
            <Columns>
                <asp:TemplateField ItemStyle-Height="80px" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server"
                        NavigateUrl="~/QuickAccess/QuickAccessButtonForm.aspx" >Empty</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Height="80px" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server"
                        NavigateUrl="~/QuickAccess/QuickAccessButtonForm.aspx" >Empty</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Height="80px" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server"
                         NavigateUrl="~/QuickAccess/QuickAccessButtonForm.aspx" >Empty</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Height="80px" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server"
                        NavigateUrl="~/QuickAccess/QuickAccessButtonForm.aspx" >Empty</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Height="80px" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server"
                        NavigateUrl="~/QuickAccess/QuickAccessButtonForm.aspx" >Empty</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Height="80px" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server"
                        NavigateUrl="~/QuickAccess/QuickAccessButtonForm.aspx" >Empty</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>    
    </div>    
    </form>    
</body>
</html>


