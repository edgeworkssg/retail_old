<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewOutletPricing.aspx.cs"
    Inherits="PowerWeb.Product.ViewOutletPricing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Outlet Pricing</title>
    <link href="App_Themes/Default/StyleSheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 36px;
        }
        .style2
        {
            height: 23px;
        }
    </style>
    <script src="../Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $('#chkAll_ProductMaster').change(function() {
                var checkboxes = $(".chkProductMaster").find(':checkbox:enabled');
                if ($(this).prop('checked')) {
                    checkboxes.prop('checked', true);
                } else {
                    checkboxes.prop('checked', false);
                }
            });
            $('#chkAll_OutletGroup').change(function() {
                var checkboxes = $(".chkOutletGroup").find(':checkbox:enabled');
                if ($(this).prop('checked')) {
                    checkboxes.prop('checked', true);
                } else {
                    checkboxes.prop('checked', false);
                }
            });
            $('#chkAll_Outlet').change(function() {
                var checkboxes = $(".chkOutlet").find(':checkbox:enabled');
                if ($(this).prop('checked')) {
                    checkboxes.prop('checked', true);
                } else {
                    checkboxes.prop('checked', false);
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="900px">
            <tr>
                <td colspan="3" class="wl_pageheaderSub">
                    Outlet Pricing
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblStatus" runat="server" ForeColor="#FF9900" />
                </td>
            </tr>
            <tr>
                <td style="width: 50%">
                    Item No
                </td>
                <td style="width: 5px">
                    :
                </td>
                <td style="width: 50%">
                    <asp:Label ID="lblItemNo" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Item Name
                </td>
                <td style="width: 5px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblItemName" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Product Master Price
                </td>
                <td class="style2">
                    :
                </td>
                <td class="style2">
                    <asp:Label ID="lblProductMasterPrice" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="width: 5px">
                </td>
                <td>
                </td>
            </tr>
      <%--      <tr>
                <td colspan="3" class="wl_pageheaderSub">
                    
                    Product Master
                </td>
            </tr>--%>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gvProductMaster" SkinID="scaffold" runat="server" AllowPaging="false"
                        AllowSorting="False" AutoGenerateColumns="False" Width="100%" ShowHeader="true">
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Width="25px" />
                                <HeaderTemplate>
                                    <input type="checkbox" id="chkAll_ProductMaster" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelected" runat="server" Checked='<%# Bind("Active") %>' CssClass="chkProductMaster" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Master">
                                <HeaderStyle width="300px" />
                                <ItemStyle Width="300px" />
                                <ItemTemplate>
                                    Retail Price &nbsp; <asp:Button ID="btnApplyPrice" runat="server" class="classname" OnClick="btnApplyPrice_Click" Text="<%$ Resources:dictionary, Apply This Price to All Outlet %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Retail Price">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRetailPrice" runat="server" Width="100px" Text='<%#Bind("RetailPrice", "{0:N2}")%>'></asp:TextBox>                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P1">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP1" runat="server" Width="100px" Text='<%#Bind("P1", "{0:N2}")%>'></asp:TextBox>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P2">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP2" runat="server" Width="100px" Text='<%#Bind("P2", "{0:N2}")%>'></asp:TextBox>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P3">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP3" runat="server" Width="100px" Text='<%#Bind("P3", "{0:N2}")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P4">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP4" runat="server" Width="100px" Text='<%#Bind("P4", "{0:N2}")%>'></asp:TextBox>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P5">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP5" runat="server" Width="100px" Text='<%#Bind("P5", "{0:N2}")%>'></asp:TextBox>                                    
                                </ItemTemplate>
                            </asp:TemplateField>                            
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" class="wl_pageheaderSub">
                    <input type="checkbox" id="chkAll_OutletGroup" /> &nbsp;
                    Outlet Group Level
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gvOutletGroupLevel" SkinID="scaffold" runat="server" AllowPaging="false"
                        AllowSorting="False" AutoGenerateColumns="False" Width="100%" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Width="25px" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelected" runat="server" Checked='<%# Bind("Active") %>' CssClass="chkOutletGroup" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Outlet Group Name">
                                <ItemStyle Width="300px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOutletGroupName" runat="server" Text='<%# Bind("OutletGroupName") %>'></asp:Label>
                                    <asp:Label ID="lblOutletGroupID" runat="server" Visible="false" Text='<%# Bind("OutletGroupID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Retail Price">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRetailPrice" runat="server" Width="100px" Text='<%#Bind("RetailPrice", "{0:N2}")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P1">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP1" runat="server" Width="100px" Text='<%#Bind("P1", "{0:N2}")%>'></asp:TextBox>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P2">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP2" runat="server" Width="100px" Text='<%#Bind("P2", "{0:N2}")%>'></asp:TextBox>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P3">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP3" runat="server" Width="100px" Text='<%#Bind("P3", "{0:N2}")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P4">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP4" runat="server" Width="100px" Text='<%#Bind("P4", "{0:N2}")%>'></asp:TextBox>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P5">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP5" runat="server" Width="100px" Text='<%#Bind("P5", "{0:N2}")%>'></asp:TextBox>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
           <tr>
                <td colspan="3" class="wl_pageheaderSub">
                     <input type="checkbox" id="chkAll_Outlet" /> &nbsp;
                    Outlet Level
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gvOutletLevel" SkinID="scaffold" runat="server" 
                        AutoGenerateColumns="False" Width="100%" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Width="25px" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelected" runat="server" Checked='<%# Bind("Active") %>' CssClass="chkOutlet" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Outlet Name">
                                <ItemStyle Width="300px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOutletName" runat="server" Text='<%# Bind("OutletName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Retail Price">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRetailPrice" runat="server" Width="100px" Text='<%#Bind("RetailPrice", "{0:N2}")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P1">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP1" runat="server" Width="100px" Text='<%#Bind("P1", "{0:N2}")%>'></asp:TextBox>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P2">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP2" runat="server" Width="100px" Text='<%#Bind("P2", "{0:N2}")%>'></asp:TextBox>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P3">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP3" runat="server" Width="100px" Text='<%#Bind("P3", "{0:N2}")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P4">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP4" runat="server" Width="100px" Text='<%#Bind("P4", "{0:N2}")%>'></asp:TextBox>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P5">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtP5" runat="server" Width="100px" Text='<%#Bind("P5", "{0:N2}")%>'></asp:TextBox>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="2" align="right">
                    <asp:Button ID="btnSavePrice" runat="server" Text="Save" CssClass="classname" OnClick="btnSavePrice_Click" />
                    &nbsp;
                    <input type="button" class="classname" value="Close" onclick="javascript:window.close();"
                        width="75px" />
                </td>
            </tr>
            <%--<tr>
                <td colspan="3" class="style1" align="right">
                    <asp:Button ID="btnSetToProductMaster" runat="server" class="classname" Text="Set All To Product Master Price"
                        Width="250px" onclick="btnSetToProductMaster_Click" />
                        &nbsp;
                    <asp:Button ID="btnSetToIndividualOutlet" runat="server" class="classname" Text="Set All To Individual Outlet Price"
                        Width="250px" onclick="btnSetToIndividualOutlet_Click" />
                    
                </td>
            </tr>--%>
            <tr>
                <td colspan="3" class="style1">
                    <asp:GridView ID="gvSupplier" SkinID="scaffold" runat="server" AllowPaging="false"
                        AllowSorting="False" AutoGenerateColumns="False" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="SupplierName" HeaderText="Supplier" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
