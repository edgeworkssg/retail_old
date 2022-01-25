<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberTransactionsReport.aspx.cs"
    Inherits="MemberTransactionsReport" Title="<%$ Resources:dictionary, Statement Account %>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    
    <form id="form1" runat="server">
     <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <br />
    <div style="height: 20px; width: 650px;" class="wl_pageheaderSub">
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$ Resources:dictionary, Statement Account %>"></asp:Literal>
    </div>
    <div>
        <table width="650px" id="FilterTable">
            <tr class="wl_lightRaw">
                <td colspan="4">
                    Statement Account of :
                    <asp:Label ID="lblMembership" runat="server" Text="Label"></asp:Label>
                    <asp:HiddenField ID="hMembershipNo" runat="server" />
                </td>
            </tr>
            <tr   class="wl_darkRaw">
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                </td>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal>
                 </td>
                 <td>
                    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                </td>
            </tr>
            <tr  class="wl_lightRaw">
                <td  colspan="2">
                    <asp:CheckBox ID="cbShowPaidInvoices" runat="server" Text="<%$ Resources:dictionary, Show Paid Invoices %>"
                        AutoPostBack="true" OnCheckedChanged="cbShowPaidInvoices_OnCheckedChanged" />
                </td>
                <td align="right"  colspan="2">
                    <asp:Button ID="btnSearch" CssClass="classBlue" runat="server" Text="<%$ Resources:dictionary, Search %>"
                        OnClick="btnSearch_Click" />
                    <asp:LinkButton ID="lnkPrint" class="classBlue" runat="server" OnClick="lnkPrint_Click"
                        Text="<%$ Resources:dictionary, Print %>"></asp:LinkButton>
                    <asp:LinkButton ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click"
                        Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div>
        <asp:GridView ID="gvTransactions" runat="server" Width="800px" AutoGenerateColumns="False"
            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
            CellPadding="3" GridLines="Vertical" OnSelectedIndexChanged="gvTransactions_SelectedIndexChanged">
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="View" runat="server" CausesValidation="false" CommandName="Select"
                            Text="<%$Resources:dictionary, View%>"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="OrderDate" HeaderText="Order Date" SortExpression="OrderDate"
                    DataFormatString="{0:dd MMM yyyy}">
                    <ItemStyle Width="100px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                <asp:BoundField DataField="PaymentTerm" HeaderText="Payment Term" SortExpression="PaymentTerm" />
                <asp:BoundField DataField="DaysOutStanding" HeaderText="Days OutStanding" SortExpression="DaysOutStanding" />
                <asp:BoundField DataField="Credit" HeaderText="Invoice Amount" SortExpression="Credit"
                    DataFormatString="{0:N2}" />
                <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance"
                    DataFormatString="{0:N2}" />
                <asp:BoundField DataField="OutletName" HeaderText="Outlet" SortExpression="OutletName" />
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderHdrID" runat="server" Text='<%# Eval("orderHdrID")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <EmptyDataTemplate>
                <asp:Literal ID="literal5" runat="server" Text="No Installment data has been created yet"></asp:Literal></EmptyDataTemplate>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
