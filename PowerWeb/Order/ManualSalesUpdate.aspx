<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="ManualSalesUpdate.aspx.cs" Inherits="PowerWeb.Order.ManualSalesUpdate"
    Title="Manual Sales Update" %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
            .hide
            {
            	display:none;
            }
            .gridTextBox
            {
            	margin-top:10px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <table width="1000px" id="FilterTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                Manual Sales Update
            </td>
        </tr>
        <tr>
            <td class="fieldname" style="width: 105px">
                <asp:Label ID="lblOutlet" runat="server" Text="Outlet" />
            </td>
            <td>
                <asp:DropDownList ID="ddlOutlet" runat="server" Width="200px" AutoPostBack="true"
                    oninit="ddlOutlet_Init" DataTextField="OutletName" 
                    DataValueField="OutletName" 
                    onselectedindexchanged="ddlOutlet_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="fieldname">
                <asp:Label ID="lblPOS" runat="server" Text="Point Of Sale" />
            </td>
            <td>
                <asp:DropDownList ID="ddlTenant" runat="server" Width="200px" DataTextField="PointOfSaleName" DataValueField="PointOfSaleID">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldname" style="width: 105px">
                Month
            </td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" Width="200px">
                    <asp:ListItem Text="January" Value="1" />
                    <asp:ListItem Text="February" Value="2" />
                    <asp:ListItem Text="March" Value="3" />
                    <asp:ListItem Text="April" Value="4" />
                    <asp:ListItem Text="May" Value="5" />
                    <asp:ListItem Text="June" Value="6" />
                    <asp:ListItem Text="July" Value="7" />
                    <asp:ListItem Text="August" Value="8" />
                    <asp:ListItem Text="September" Value="9" />
                    <asp:ListItem Text="October" Value="10" />
                    <asp:ListItem Text="November" Value="11" />
                    <asp:ListItem Text="December" Value="12" />                                                                                
                </asp:DropDownList>
            </td>
            <td class="fieldname">
                Year
            </td>
            <td>
                <asp:DropDownList ID="ddlYear" runat="server" Width="200px" 
                    oninit="ddlYear_Init">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" CssClass="classname" />
            </td>
            <td colspan="2" align="right">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" 
                    CssClass="classBlue" ValidationGroup="SubmitData" />
            </td>
        </tr>
    </table>
    <br />    
    <div>
        <asp:Label ID="lblStatus" runat="server" Font-Bold="True"
            ForeColor="Red" />
    </div>
    <asp:GridView ID="gvReport" Width="1000px" runat="server" ShowFooter="False" AllowPaging="False"
        AllowSorting="False" AutoGenerateColumns="False"
        SkinID="scaffold" onrowdatabound="gvReport_RowDataBound">
        <Columns>
            <asp:BoundField DataField="Days" HeaderText="Days" />
            <asp:BoundField DataField="MallCode" HeaderText="Mall Code" />            
            <asp:BoundField DataField="TenantCode" HeaderText="Tenant Code" />      
            <asp:BoundField DataField="Date" HeaderText="Date" ItemStyle-Width="100px" />
            <asp:TemplateField HeaderText="Hour" Visible="false">
                <ItemStyle Width="90px" />
                <ItemTemplate>
                    <cc2:TimeSelector ID="tsRestrictedStart" Style="float: left;vertical-align:middle" MinuteIncrement="1" DisplaySeconds="false"
                        runat="server" Hour="0" Minute="0">
                    </cc2:TimeSelector>                    
                </ItemTemplate>
            </asp:TemplateField>      
            <asp:TemplateField HeaderText="Transaction Count">
                <ItemTemplate>
                    <asp:TextBox CssClass="gridTextBox" ValidationGroup="SubmitData" ID="txtTransactionCount" runat="server" Text='<%# Bind("TransactionCount") %>' Width="135px" />
                    <br />
                    <asp:RangeValidator ID="rvTransactionCount" runat="server" Type="Integer" ValidationGroup="SubmitData" 
                    MinimumValue="0" MaximumValue="9999999" ControlToValidate="txtTransactionCount" 
                    ErrorMessage="Value must be a positive integer" Font-Size="XX-Small" />                    
                </ItemTemplate>
            </asp:TemplateField>      
            <asp:TemplateField HeaderText="Total Sales After Tax">
                <ItemTemplate>
                    <asp:TextBox CssClass="gridTextBox" ValidationGroup="SubmitData" ID="txtTotalSalesAfterTax" runat="server" Text='<%# Bind("TotalSalesAfterTax","{0:N2}") %>' Width="150px" />
                    <br />
                    <asp:RangeValidator ID="rvTotalSalesAfterTax" runat="server" Type="Currency" ValidationGroup="SubmitData" 
                    MinimumValue="0" MaximumValue="99999999" ControlToValidate="txtTotalSalesAfterTax" 
                    ErrorMessage="Value must be a positive decimal" Font-Size="XX-Small" />  
                </ItemTemplate>
            </asp:TemplateField>      
            <asp:TemplateField HeaderText="Total Sales Before Tax">
                <ItemTemplate>
                    <asp:TextBox CssClass="gridTextBox" ValidationGroup="SubmitData" ID="txtTotalSalesBeforeTax" runat="server" Text='<%# Bind("TotalSalesBeforeTax","{0:N2}") %>' Width="150px" />
                    <br />
                    <asp:RangeValidator ID="rvTotalSalesBeforeTax" runat="server" Type="Currency" ValidationGroup="SubmitData" 
                    MinimumValue="0" MaximumValue="99999999" ControlToValidate="txtTotalSalesBeforeTax" 
                    ErrorMessage="Value must be a positive decimal" Font-Size="XX-Small" />                                                            
                </ItemTemplate>
            </asp:TemplateField>      
            <asp:TemplateField HeaderText="Total Tax">
                <ItemTemplate>
                    <asp:TextBox CssClass="gridTextBox" ValidationGroup="SubmitData" ID="txtTotalTax" runat="server" Text='<%# Bind("TotalTax","{0:N2}") %>' Width="150px" />
                    <br />
                    <asp:RangeValidator ID="rvTotalTax" runat="server" Type="Currency" ValidationGroup="SubmitData" 
                    MinimumValue="0" MaximumValue="99999999" ControlToValidate="txtTotalTax" 
                    ErrorMessage="Value must be a positive decimal" Font-Size="XX-Small" />                                                                                
                </ItemTemplate>
            </asp:TemplateField>         
            <asp:TemplateField HeaderText="Remarks">
                <ItemTemplate>
                    <asp:TextBox ValidationGroup="SubmitData" ID="txtRemarks" runat="server" Text='<%# Bind("Remarks") %>' Width="100px" />
                </ItemTemplate>
            </asp:TemplateField>                                                                  
            <asp:BoundField DataField="POSID" HeaderText="POSID" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />                    
        </Columns>
    </asp:GridView>
</asp:Content>
