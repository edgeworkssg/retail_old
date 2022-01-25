<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="PrintDeliveryOrder.aspx.cs" Inherits="PowerPOS.PrintDeliveryOrder"
    Theme="default" Title="<%$Resources:dictionary, Print Delivery Order%>" ClientTarget="uplevel" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldSearchDateFrom" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="imbSearchDateFrom" TargetControlID="txtSearchDateFrom">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldSearchDateTo" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="imbSearchDateTo" TargetControlID="txtSearchDateTo">
    </cc1:CalendarExtender>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <asp:Panel ID="pnlGrid" runat="server">
                <asp:Label ID="lblResult" runat="server"></asp:Label>
                <table id="FilterTable" width="700px">
                    <tr>
                        <td>
                            <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Delivery Date%>" />
                        </td>
                        <td>
                            <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary, Start Date%>" />
                            <asp:TextBox ID="txtSearchDateFrom" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="imbSearchDateFrom" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png"
                                Style="height: 16px; width: 16px" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, End Date%>" />
                            <asp:TextBox ID="txtSearchDateTo" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="imbSearchDateTo" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png"
                                Style="height: 16px; width: 16px" />
                        </td>
                    </tr>
                </table>
                <div style="height: 5px;">
                </div>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Button ID="btnSearch" CssClass="classname" runat="server" Text="<%$Resources:dictionary, Search%>"
                                OnClick="btnSearch_Click"></asp:Button>&nbsp;&nbsp;
                            <asp:Button ID="btnPrintAll" CssClass="classname" runat="server" Text="<%$Resources:dictionary, Print All%>"
                                OnClick="btnPrintAll_Click"></asp:Button>&nbsp;&nbsp;
                            <asp:Button ID="btnPrintSelected" CssClass="classname" runat="server" Text="<%$Resources:dictionary, Print Selected%>"
                                OnClick="btnPrintSelected_Click"></asp:Button>&nbsp;&nbsp;
                        </td>
                        <td align="right">
                            <asp:Button ID="btnExport" CssClass="classBlue" runat="server" Text="<%$Resources:dictionary, Export%>"
                                OnClick="btnExport_Click"></asp:Button>&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
                <div style="height: 20px;">
                </div>
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
                    OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="OrderNumber"
                    PageSize="50" OnRowDataBound="GridView1_RowDataBound" SkinID="scaffold">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkTicked" runat="server" />
                                <asp:HiddenField ID="hidOrderNumber" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PurchaseOrderRefNo" HeaderText="<%$Resources:dictionary, Delivery Order No%>"
                            SortExpression="PurchaseOrderRefNo"></asp:BoundField>
                        <asp:BoundField DataField="CustomInvoiceNo" HeaderText="<%$Resources:dictionary, Invoice No%>"
                            SortExpression="CustomInvoiceNo"></asp:BoundField>
                        <asp:BoundField DataField="OutletName" HeaderText="<%$Resources:dictionary, Outlet%>"
                            SortExpression="OutletName"></asp:BoundField>
                        <asp:BoundField DataField="OrderDate" HeaderText="<%$Resources:dictionary, Invoice Date%>"
                            SortExpression="OrderDate" DataFormatString="{0:dd MMM yyyy HH:mm:ss}"></asp:BoundField>
                        <asp:BoundField DataField="RecipientName" HeaderText="<%$Resources:dictionary, Recipient Name%>"
                            SortExpression="RecipientName"></asp:BoundField>
                        <asp:BoundField DataField="MobileNo" HeaderText="<%$Resources:dictionary, Mobile No%>"
                            SortExpression="MobileNo"></asp:BoundField>
                        <asp:BoundField DataField="HomeNo" HeaderText="<%$Resources:dictionary, Home No%>"
                            SortExpression="HomeNo"></asp:BoundField>
                        <asp:BoundField DataField="DeliveryAddressFull" HeaderText="<%$Resources:dictionary, Delivery Address%>"
                            SortExpression="DeliveryAddressFull"></asp:BoundField>
                        <asp:BoundField DataField="DeliveryDateTime" HeaderText="<%$Resources:dictionary, Delivery Date & Time%>"
                            SortExpression="DeliveryDateTime"></asp:BoundField>
                        <asp:BoundField DataField="BalancePayment" HeaderText="<%$Resources:dictionary, Balance Payment%>"
                            SortExpression="BalancePayment" DataFormatString="{0:$0.00}"></asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderText="<%$Resources:dictionary, Remark%>"
                            SortExpression="Remark"></asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Literal ID="ltr01" runat="server" Text="<%$Resources:dictionary,No Delivery Order %>" />
                    </EmptyDataTemplate>
                    <PagerTemplate>
                        <div style="border-top: 1px solid #666666">
                            <br />
                            <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                                CommandArgument="First" CommandName="Page" />
                            <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                                CommandArgument="Prev" CommandName="Page" />
                            <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Page%>" />
                            <asp:DropDownList ID="ddlPages" runat="server" CssClass="scaffoldEditItem" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                            </asp:DropDownList>
                            of
                            <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                            <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Next >%>"
                                CommandArgument="Next" CommandName="Page" />
                            <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Last >>%>"
                                CommandArgument="Last" CommandName="Page" />
                    </PagerTemplate>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="pnlEdit" runat="server">
                <table id="FieldsTable" cellpadding="5" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Literal ID="Literal16" runat="server" Text="<%$Resources:dictionary, Invoice No%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlInvoiceNo" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal15" runat="server" Text="<%$Resources:dictionary, Outlet%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlOutlet" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary, Invoice Date%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlInvoiceDate" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary, Delivery Order No%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlPurchaseOrderRefNo" runat="server" Enabled="false"></asp:TextBox>
                            <asp:Label ID="lblID" runat="server" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:dictionary, Recipient Name%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlRecipientName" runat="server" MaxLength="200" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary, Mobile No%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlMobileNo" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary, Home No%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlHomeNo" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary, Postal Code%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlPostalCode" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary, Delivery Address%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlDeliveryAddress" runat="server" TextMode="MultiLine" Height="100px"
                                Width="500px" MaxLength="2147483647" Enabled="false">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary, Unit No%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlUnitNo" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary, Delivery Date%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlDeliveryDate" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary, Delivery Time%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlDeliveryTime" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Balance Payment%>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlBalancePayment" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Remark %>" />
                        </td>
                        <td>
                            <asp:TextBox ID="ctrlRemark" runat="server" TextMode="MultiLine" Height="100px" Width="500px"
                                MaxLength="2147483647" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="GridView2" runat="server" AllowPaging="false" AllowSorting="False"
                    Width="100%" AutoGenerateColumns="False" DataKeyNames="DetailsID" PageSize="50"
                    SkinID="scaffold">
                    <Columns>
                        <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary, Item No%>">
                        </asp:BoundField>
                        <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary, Item Name%>">
                        </asp:BoundField>
                        <asp:BoundField DataField="Quantity" HeaderText="<%$Resources:dictionary, Quantity%>">
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, No Delivery Order%>" />
                    </EmptyDataTemplate>
                </asp:GridView>
                <br />
                <input id="btnReturn" runat="server" type="button" onclick="history.back()" class="classname"
                    value="<%$Resources:dictionary, Return%>" /></asp:Panel>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:Button ID="btnDone" CssClass="classname" runat="server" Text="<%$Resources:dictionary, Done%>"
                OnClick="btnDone_Click"></asp:Button><br />
            <br />
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" DisplayGroupTree="False"
                Width="100%" Height="50px" HasCrystalLogo="False" HasExportButton="False" HasViewList="False"
                PrintMode="Pdf" />
        </asp:View>
    </asp:MultiView>
</asp:Content>
