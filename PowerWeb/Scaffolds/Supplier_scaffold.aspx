<%@ Page Language="C#" Title="<%$Resources:dictionary,Supplier Scaffold %>" Inherits="Supplier_scaffold"
    MasterPageFile="~/PowerPOSMSt.master" Theme="default" CodeBehind="Supplier_scaffold.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Supplier %>"></asp:Literal></h2>
    <asp:Panel ID="pnlGrid" runat="server" Width="900px">
        <div style="height: 20px;" class="wl_pageheaderSub">
            <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
        </div>
        <table width="900px" id="FilterTable">
            <tr>
                <td style="width: 102px">
                    <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary, Search%>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server" Width="179px" />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="style1">
                    <asp:Button Width="80px" class="classname" ID="btnSearchData" runat="server" Text="<%$Resources:dictionary, Search%>"
                        OnClick="btnSearchData_Click" />
                    <asp:Button Width="80px" class="classname" ID="btnClear" runat="server" Text="<%$Resources:dictionary, Clear%>"
                        OnClick="btnClear_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <input id="btnAddNew" runat="server" class="classname" onclick="location.href='Supplier_scaffold.aspx?id=0'" type="button"
                        value="<%$Resources:dictionary, Add New%>" />
                </td>
            </tr>
        </table>
        <div style="height: 7px;">
        </div>
        <asp:GridView ID="GridView1" Width="100%" runat="server" AllowPaging="True" AllowSorting="True"
            SkinID="scaffold" AutoGenerateColumns="False" OnDataBound="GridView1_DataBound"
            OnSorting="GridView1_Sorting" OnPageIndexChanging="GridView1_PageIndexChanging"
            DataKeyNames="SupplierID" PageSize="10">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary,Edit %>" DataNavigateUrlFields="SupplierID"
                    DataNavigateUrlFormatString="Supplier_scaffold.aspx?id={0}" />
                <asp:BoundField DataField="SupplierName" HeaderText="<%$Resources:dictionary,Supplier Name %>"
                    SortExpression="SupplierName"></asp:BoundField>
                <asp:BoundField DataField="userfld3" SortExpression="userfld3" HeaderText="<%$Resources:dictionary, Supplier Code%>" />
                <asp:BoundField DataField="Currency" SortExpression="Currency" HeaderText="<%$Resources:dictionary, Currency%>" />
                <asp:BoundField DataField="GST" SortExpression="GST" HeaderText="<%$Resources:dictionary, GST%>" />
                <asp:BoundField DataField="PaymentTerm" SortExpression="PaymentTerm" HeaderText="<%$Resources:dictionary, Payment Term%>" />
                <asp:BoundField DataField="MinimumOrder" SortExpression="MinimumOrder" HeaderText="<%$Resources:dictionary, Minimum Order%>" />
                <asp:BoundField DataField="DeliveryCharge" SortExpression="DeliveryCharge" HeaderText="<%$Resources:dictionary, Delivery Charge%>" />
                <asp:BoundField DataField="CustomerAddress" HeaderText="<%$Resources:dictionary, Supplier Address1%>" SortExpression="CustomerAddress">
                </asp:BoundField>
                <asp:BoundField DataField="ShipToAddress" HeaderText="<%$Resources:dictionary, Supplier Address2%>" SortExpression="ShipToAddress">
                </asp:BoundField>
                <asp:BoundField DataField="BillToAddress" HeaderText="<%$Resources:dictionary, Supplier Address3%>" SortExpression="BillToAddress">
                </asp:BoundField>
                <asp:BoundField DataField="ContactNo1" HeaderText="<%$Resources:dictionary,Home %>"
                    SortExpression="ContactNo1"></asp:BoundField>
                <asp:BoundField DataField="ContactNo2" HeaderText="<%$Resources:dictionary,Email %>"
                    SortExpression="ContactNo2"></asp:BoundField>
                <asp:BoundField DataField="ContactNo3" HeaderText="<%$Resources:dictionary,Fax %>"
                    SortExpression="ContactNo3"></asp:BoundField>
                <asp:BoundField DataField="ContactPerson1" HeaderText="<%$Resources:dictionary,Contact Person1 %>"
                    SortExpression="ContactPerson1"></asp:BoundField>
                <asp:BoundField DataField="ContactPerson2" HeaderText="<%$Resources:dictionary,Contact Person2 %>"
                    SortExpression="ContactPerson2"></asp:BoundField>
                <asp:BoundField DataField="ContactPerson3" HeaderText="<%$Resources:dictionary,Contact Person3 %>"
                    SortExpression="ContactPerson3"></asp:BoundField>
                <asp:BoundField DataField="AccountNo" HeaderText="<%$Resources:dictionary,Account No %>"
                    SortExpression="AccountNo"></asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,No Supplier  %>"></asp:Literal>
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                        ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> "
                        CommandArgument="Next" CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                        CommandArgument="Last" CommandName="Page" />
                </div>
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Supplier %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblID" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Supplier Name %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlSupplierName" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Supplier Code%>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlSupplierCode" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr id="DisplayCurrencyOnSupplier" runat="server">
                <td>
                    <asp:Literal ID="Literal19"  runat="server" Text="<%$Resources:dictionary, Currency%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlCurrency" Width="200px" runat="server" Height="20px" OnInit="ddlCurrency_Init">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="DisplayGSTOnSupplier" runat="server">
                <td>
                    <asp:Literal ID="Literal18"  runat="server" Text="<%$Resources:dictionary, GST%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlGST" Width="200px" runat="server" Height="20px">
                        <asp:ListItem>--Please Select--</asp:ListItem>
                        <asp:ListItem Value="Exclusive GST" Text="<%$Resources:dictionary, Exclusive GST%>"></asp:ListItem>
                        <asp:ListItem Value="Inclusive GST" Text="<%$Resources:dictionary, Inclusive GST%>"></asp:ListItem>
                        <asp:ListItem Value="Non GST" Text="<%$Resources:dictionary, Non GST%>"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="DisplayPaymentTermOnSupplier" runat="server">
                <td valign="top">
                    <asp:Literal ID="Literal17"  runat="server" Text="<%$Resources:dictionary, Payment Term%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlPaymentTerm" Width="200px" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr id="DisplayMinimumOrderOnSupplier" runat="server">
                <td valign="top">
                    <asp:Literal ID="Literal16"  runat="server" Text="<%$Resources:dictionary, Minimum Order%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtMinOrder" Width="200px" runat="server"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtMinOrder"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="5,000,000,000"
                        MinimumValue="0" Type="Currency"></asp:RangeValidator>&nbsp;
                </td>
            </tr>
            <tr id="DisplayDeliveryChargeOnSupplier" runat="server">
                <td valign="top">
                    <asp:Literal ID="Literal15"  runat="server" Text="<%$Resources:dictionary, Delivery Charge%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtDeliveryCharge" Width="200px" runat="server"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtDeliveryCharge"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="5,000,000,000"
                        MinimumValue="0" Type="Currency"></asp:RangeValidator>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, Supplier Address1 %>" />
                </td>
                <td class="style2">
                    <asp:TextBox ID="ctrlCustomerAddress" Width="200px" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Supplier Address2%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlShipToAddress" Width="200px" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Supplier Address3%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlBillToAddress" Width="200px" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Home %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlContactNo1" Width="200px" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,Email %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlContactNo2" Width="200px" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Fax %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlContactNo3" Width="200px" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Contact Person1 %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlContactPerson1" Width="200px" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary,Contact Person2 %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlContactPerson2" Width="200px" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:dictionary,Contact Person3 %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlContactPerson3" Width="200px" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary,Account No %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlAccountNo" Width="200px" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal20" runat="server" Text="<%$Resources:dictionary,Is Warehouse %>"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBox ID="chkIsWarehouse" runat="server" Checked="false" 
                        AutoPostBack="True" oncheckedchanged="chkIsWarehouse_CheckedChanged" />
                </td>
            </tr>
            <tr runat="server" id="trWarehouseID" visible="false">
                <td>
                    <asp:Literal ID="Literal21" runat="server" Text="<%$Resources:dictionary,Warehouse ID %>"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlWarehouseID" runat="server" 
                        DataValueField="InventoryLocationID" 
                        DataTextField="InventoryLocationName" Width="200px" 
                        oninit="ddlWarehouseID_Init" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:Button ID="btnSave" CssClass="classname" runat="server" Text="<%$ Resources:dictionary, Save %>"
                        OnClick="btnSave_Click"></asp:Button>&nbsp;
                        
                     <input id="btnReturn" runat="server" class="classname" onclick="location.href='Supplier_scaffold.aspx'" type="button"
                        value="<%$Resources:dictionary, Return%>" />&nbsp;
                        
                    <asp:Button ID="btnDelete" CssClass="classname" runat="server" CausesValidation="False"
                        Text="<%$ Resources:dictionary, Delete %>" OnClick="btnDelete_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headContent">
    <style type="text/css">
        .style1
        {
            height: 32px;
        }
        .style2
        {
            height: 63px;
        }
    </style>
</asp:Content>
