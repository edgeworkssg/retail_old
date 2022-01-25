<%@ Page Theme="Default" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    Inherits="ItemSupplierMapScaffold" Title="<%$Resources:dictionary,Item Supplier Setup %>"
    CodeBehind="ItemSupplierMapScaffold.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $("#<%= txtPackingSizeUOM1.ClientID %>").change(function() { CheckMultiplyOf10($("#<%= txtPackingSizeUOM1.ClientID %>").val()); });
            $("#<%= txtPackingSizeUOM2.ClientID %>").change(function() { CheckMultiplyOf10($("#<%= txtPackingSizeUOM2.ClientID %>").val()); });
            $("#<%= txtPackingSizeUOM3.ClientID %>").change(function() { CheckMultiplyOf10($("#<%= txtPackingSizeUOM3.ClientID %>").val()); });
            $("#<%= txtPackingSizeUOM4.ClientID %>").change(function() { CheckMultiplyOf10($("#<%= txtPackingSizeUOM4.ClientID %>").val()); });
            $("#<%= txtPackingSizeUOM5.ClientID %>").change(function() { CheckMultiplyOf10($("#<%= txtPackingSizeUOM5.ClientID %>").val()); });
            $("#<%= txtPackingSizeUOM6.ClientID %>").change(function() { CheckMultiplyOf10($("#<%= txtPackingSizeUOM6.ClientID %>").val()); });
            $("#<%= txtPackingSizeUOM7.ClientID %>").change(function() { CheckMultiplyOf10($("#<%= txtPackingSizeUOM7.ClientID %>").val()); });
            $("#<%= txtPackingSizeUOM8.ClientID %>").change(function() { CheckMultiplyOf10($("#<%= txtPackingSizeUOM8.ClientID %>").val()); });
            $("#<%= txtPackingSizeUOM9.ClientID %>").change(function() { CheckMultiplyOf10($("#<%= txtPackingSizeUOM9.ClientID %>").val()); });
            $("#<%= txtPackingSizeUOM10.ClientID %>").change(function() { CheckMultiplyOf10($("#<%= txtPackingSizeUOM10.ClientID %>").val()); });
        });
        function CheckMultiplyOf10(val) {
            //            if (val % 10 != 0) {
            //                alert('Not Multiply of 10');
            //            }
        }
    </script>

    <asp:Panel ID="pnlGrid" runat="server">
        <div style="height: 20px;" class="wl_pageheaderSub">
            <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
        </div>
        <table width="700px" id="FilterTable">
            <tr>
                <td style="width: 102px">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary, Supplier%>"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSupplier" runat="server" Width="179px" 
                        oninit="ddlSupplier_Init">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Item Name%>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtItemName" runat="server" Width="179px" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button Width="80px" class="classname" ID="btnSearchData" runat="server" Text="<%$Resources:dictionary, Search %>" 
                        onclick="btnSearchData_Click" />
                    <asp:Button Width="80px" class="classname" ID="btnClear" runat="server" 
                        Text="<%$Resources:dictionary, Clear%>" onclick="btnClear_Click" />                        
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <input id="btnAdd" runat="server" style="width:80px" class="classname" onclick="location.href='ItemSupplierMapScaffold.aspx?id=0'"
                        type="button" value="<%$Resources:dictionary, Add New%>" />                
                </td>
            </tr>            
        </table>
        <div style="height: 7px;">
        </div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" Width="700px"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="ItemSupplierMapID"
            PageSize="10" SkinID="scaffold" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="ItemSupplierMapID" DataNavigateUrlFormatString="ItemSupplierMapScaffold.aspx?id={0}" />
                <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary, Item No%>" SortExpression="ItemNo" />
                <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary, Item Name%>" SortExpression="ItemName" />
                <asp:BoundField DataField="Currency" HeaderText="<%$Resources:dictionary, Currency%>" SortExpression="Currency" />
                <asp:BoundField DataField="CostPrice" HeaderText="<%$Resources:dictionary, Cost Price%>" SortExpression="Cost Price" />
                <asp:BoundField DataField="GSTRule" HeaderText="<%$Resources:dictionary, GST Rule%>" SortExpression="GSTRule" />
                <asp:BoundField DataField="SupplierID" HeaderText="<%$Resources:dictionary, Supplier Id%>" SortExpression="SupplierID" Visible="false" />
                <asp:BoundField DataField="SupplierCode" HeaderText="<%$Resources:dictionary, Supplier Code %>" SortExpression="SupplierID" />
                <asp:BoundField DataField="SupplierName" HeaderText="<%$Resources:dictionary, Supplier Name%>" SortExpression="SupplierName" />
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, No Item Supplier Map%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="Literal25"  runat="server" Text="<%$Resources:dictionary, Page%>" />
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ltr111"  runat="server" Text="<%$Resources:dictionary, of%>" />
                    <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Next >%>" CommandArgument="Next"
                        CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Last >>%>"
                        CommandArgument="Last" CommandName="Page" />
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server" CssClass="LabelMessage"></asp:Label>
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="900px">
            <tr>
                <td>
                    <asp:Literal ID="Literal24"  runat="server" Text="<%$Resources:dictionary, Map ID%>" />
                </td>
                <td>
                    <asp:Label ID="lblID" runat="server" Text="[ID]"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal23"  runat="server" Text="<%$Resources:dictionary, Supplier ID%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ctrlSupplier" runat="server" Width="305px" AutoPostBack="True"
                        OnSelectedIndexChanged="ctrlSupplier_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal26"  runat="server" Text="<%$Resources:dictionary, Is Preferred Supplier%>" />
                </td>
                <td>
                    <asp:CheckBox ID="chkIsPreferredSupplier" runat="server" Checked="False" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary, Search%>"></asp:Literal>
                </td>
                <td style="width: 400px">
                    <asp:TextBox ID="txtItemNo" runat="server" Width="150px"></asp:TextBox><div class="divider">
                    </div>
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                        CssClass="classname" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Search By Item Name %>"></asp:Literal>
                </td>
                <td style="width: 400px">
                    <asp:DropDownList ID="ctrlItem" runat="server" Width="305px" AutoPostBack="True"
                        OnSelectedIndexChanged="ctrlItem_SelectedIndexChanged">
                    </asp:DropDownList>
            </tr>
            <tr>
                <td style="width: 200px">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, UOM%>"></asp:Literal>
                </td>
                <td style="width: 400px">
                    <asp:Literal ID="ltUOM" runat="server" Text=""></asp:Literal>
                </td>
            </tr>
            <tr id="rowCurrency" runat="server">
                <td style="width: 200px">
                    <asp:Literal ID="Literal21"  runat="server" Text="<%$Resources:dictionary, Currency%>" />
                </td>
                <td style="width: 400px">
                    <asp:Label ID="lblCurrency" runat="server" />
                    <asp:DropDownList Visible="false" ID="ddlCurrency" runat="server" Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal22"  runat="server" Text="<%$Resources:dictionary, Cost Price Per UOM%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlCostPrice" runat="server" MaxLength="250" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <!--<tr>
            <td >
                Trigger Level</td>
            <td >
                <asp:TextBox ID="ctrlTriggerLevel" runat="server" MaxLength="250"></asp:TextBox>
            </td>
        </tr>-->
            <tr id="rowGST" runat="server" visible="false">
                <td>
                    <asp:Literal ID="Literal20"  runat="server" Text="<%$Resources:dictionary, GST%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlGST" runat="server" Height="20px" Width="150px" EnableViewState="true">
                        <asp:ListItem Text="<%$Resources:dictionary, --Please Select--%>" Value="--Please Select--"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:dictionary, Exclusive GST%>" Value="Exclusive GST"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:dictionary, Inclusive GST%>" Value="Inclusive GST"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:dictionary, Non GST%>" Value="Non GST"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="rowPS1" runat="server">
                <td>
                    <asp:Literal ID="Literal19"  runat="server" Text="<%$Resources:dictionary, Packing Size 1%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPackingSize1" runat="server" Width="150px"></asp:TextBox>
                    &nbsp;=
                    <asp:TextBox ID="txtPackingSizeUOM1" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;<asp:Label ID="lblUOM1" runat="server" Text="" Visible="true" />&nbsp;&nbsp;
                    <asp:Label ID="Label11" runat="server" Text="<%$Resources:dictionary, Cost Price%>" Visible="true" />&nbsp;
                    <asp:TextBox ID="txtCostPrice1" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;
                    <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtCostPrice1"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtPackingSizeUOM1"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>&nbsp;
                </td>
            </tr>
            <tr id="rowPS2" runat="server">
                <td>
                    <asp:Literal ID="Literal18"  runat="server" Text="<%$Resources:dictionary, Packing Size 2%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPackingSize2" runat="server" Width="150px"></asp:TextBox>
                    &nbsp;=
                    <asp:TextBox ID="txtPackingSizeUOM2" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;<asp:Label ID="lblUOM2" runat="server" Text="" Visible="true" />&nbsp;&nbsp;
                    <asp:Label ID="Label2" runat="server" Text="<%$Resources:dictionary, Cost Price%>" Visible="true" />&nbsp;
                    <asp:TextBox ID="txtCostPrice2" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;
                    <asp:RangeValidator ID="RangeValidator12" runat="server" ControlToValidate="txtCostPrice2"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>
                    <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtPackingSizeUOM2"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>&nbsp;
                </td>
            </tr>
            <tr id="rowPS3" runat="server">
                <td>
                    <asp:Literal ID="Literal17"  runat="server" Text="<%$Resources:dictionary, Packing Size 3%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPackingSize3" runat="server" Width="150px"></asp:TextBox>
                    &nbsp;=
                    <asp:TextBox ID="txtPackingSizeUOM3" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;<asp:Label ID="lblUOM3" runat="server" Text="" Visible="true" />&nbsp;&nbsp;
                    <asp:Label ID="Label3" runat="server" Text="<%$Resources:dictionary, Cost Price%>" Visible="true" />&nbsp;
                    <asp:TextBox ID="txtCostPrice3" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;
                    <asp:RangeValidator ID="RangeValidator13" runat="server" ControlToValidate="txtCostPrice3"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>
                    <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtPackingSizeUOM3"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>&nbsp;
                </td>
            </tr>
            <tr id="rowPS4" runat="server">
                <td>
                    <asp:Literal ID="Literal16"  runat="server" Text="<%$Resources:dictionary, Packing Size 4%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPackingSize4" runat="server" Width="150px"></asp:TextBox>
                    &nbsp;=
                    <asp:TextBox ID="txtPackingSizeUOM4" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;<asp:Label ID="lblUOM4" runat="server" Text="" Visible="true" />&nbsp;&nbsp;
                    <asp:Label ID="Label4" runat="server" Text="<%$Resources:dictionary, Cost Price%>" Visible="true" />&nbsp;
                    <asp:TextBox ID="txtCostPrice4" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;
                    <asp:RangeValidator ID="RangeValidator14" runat="server" ControlToValidate="txtCostPrice4"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>
                    <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtPackingSizeUOM4"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>&nbsp;
                </td>
            </tr>
            <tr id="rowPS5" runat="server">
                <td>
                    <asp:Literal ID="Literal15"  runat="server" Text="<%$Resources:dictionary, Packing Size 5%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPackingSize5" runat="server" Width="150px"></asp:TextBox>
                    &nbsp;=
                    <asp:TextBox ID="txtPackingSizeUOM5" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;<asp:Label ID="lblUOM5" runat="server" Text="" Visible="true" />&nbsp;&nbsp;
                    <asp:Label ID="Label5" runat="server" Text="<%$Resources:dictionary, Cost Price%>" Visible="true" />&nbsp;
                    <asp:TextBox ID="txtCostPrice5" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;
                    <asp:RangeValidator ID="RangeValidator15" runat="server" ControlToValidate="txtCostPrice5"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>
                    <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtPackingSizeUOM5"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>&nbsp;
                </td>
            </tr>
            <tr id="rowPS6" runat="server">
                <td>
                    <asp:Literal ID="Literal14"  runat="server" Text="<%$Resources:dictionary, Packing Size 6%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPackingSize6" runat="server" Width="150px"></asp:TextBox>
                    &nbsp;=
                    <asp:TextBox ID="txtPackingSizeUOM6" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;<asp:Label ID="lblUOM6" runat="server" Text="" Visible="true" />&nbsp;&nbsp;
                    <asp:Label ID="Label6" runat="server" Text="<%$Resources:dictionary, Cost Price%>" Visible="true" />&nbsp;
                    <asp:TextBox ID="txtCostPrice6" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;
                    <asp:RangeValidator ID="RangeValidator16" runat="server" ControlToValidate="txtCostPrice6"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>
                    <asp:RangeValidator ID="RangeValidator7" runat="server" ControlToValidate="txtPackingSizeUOM6"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>&nbsp;
                </td>
            </tr>
            <tr id="rowPS7" runat="server">
                <td>
                    <asp:Literal ID="Literal13"  runat="server" Text="<%$Resources:dictionary, Packing Size 7%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPackingSize7" runat="server" Width="150px"></asp:TextBox>
                    &nbsp;=
                    <asp:TextBox ID="txtPackingSizeUOM7" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;<asp:Label ID="lblUOM7" runat="server" Text="" Visible="true" />&nbsp;&nbsp;
                    <asp:Label ID="Label7" runat="server" Text="<%$Resources:dictionary, Cost Price%>" Visible="true" />&nbsp;
                    <asp:TextBox ID="txtCostPrice7" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;
                    <asp:RangeValidator ID="RangeValidator17" runat="server" ControlToValidate="txtCostPrice7"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>
                    <asp:RangeValidator ID="RangeValidator8" runat="server" ControlToValidate="txtPackingSizeUOM7"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>&nbsp;
                </td>
            </tr>
            <tr id="rowPS8" runat="server">
                <td>
                    <asp:Literal ID="Literal12"  runat="server" Text="<%$Resources:dictionary, Packing Size 8%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPackingSize8" runat="server" Width="150px"></asp:TextBox>
                    &nbsp;=
                    <asp:TextBox ID="txtPackingSizeUOM8" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;<asp:Label ID="lblUOM8" runat="server" Text="" Visible="true" />&nbsp;&nbsp;
                    <asp:Label ID="Label8" runat="server" Text="<%$Resources:dictionary, Cost Price%>" Visible="true" />&nbsp;
                    <asp:TextBox ID="txtCostPrice8" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;
                    <asp:RangeValidator ID="RangeValidator18" runat="server" ControlToValidate="txtCostPrice8"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>
                    <asp:RangeValidator ID="RangeValidator9" runat="server" ControlToValidate="txtPackingSizeUOM8"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>&nbsp;
                </td>
            </tr>
            <tr id="rowPS9" runat="server">
                <td>
                    <asp:Literal ID="Literal11"  runat="server" Text="<%$Resources:dictionary, Packing Size 9%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPackingSize9" runat="server" Width="150px"></asp:TextBox>
                    &nbsp;=
                    <asp:TextBox ID="txtPackingSizeUOM9" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;<asp:Label ID="lblUOM9" runat="server" Text="" Visible="true" />&nbsp;&nbsp;
                    <asp:Label ID="Label9" runat="server" Text="<%$Resources:dictionary, Cost Price%>" Visible="true" />&nbsp;
                    <asp:TextBox ID="txtCostPrice9" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;
                    <asp:RangeValidator ID="RangeValidator19" runat="server" ControlToValidate="txtCostPrice9"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>
                    <asp:RangeValidator ID="RangeValidator10" runat="server" ControlToValidate="txtPackingSizeUOM9"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>&nbsp;
                </td>
            </tr>
            <tr id="rowPS10" runat="server">
                <td>
                    <asp:Literal ID="Literal10"  runat="server" Text="<%$Resources:dictionary, Packing Size 10%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPackingSize10" runat="server" Width="150px"></asp:TextBox>
                    &nbsp;=
                    <asp:TextBox ID="txtPackingSizeUOM10" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;<asp:Label ID="lblUOM10" runat="server" Text="" Visible="true" />&nbsp;&nbsp;
                    <asp:Label ID="Label10" runat="server" Text="<%$Resources:dictionary, Cost Price%>" Visible="true" />&nbsp;
                    <asp:TextBox ID="txtCostPrice10" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;
                    <asp:RangeValidator ID="RangeValidator20" runat="server" ControlToValidate="txtCostPrice10"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>
                    <asp:RangeValidator ID="RangeValidator11" runat="server" ControlToValidate="txtPackingSizeUOM10"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="9999999999"
                        MinimumValue="0" Type="Double"></asp:RangeValidator>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary, Created By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:dictionary, Created On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Modified By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, Modified On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Deleted%>" />
                </td>
                <td>
                    <asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        Text="<%$Resources:dictionary, Save %>" />
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='ItemSupplierMapScaffold.aspx'" type="button"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary, Delete%>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
