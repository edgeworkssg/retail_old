<%@ Page Language="C#" Theme="Default" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    Inherits="NewProductMaster" Title="<%$Resources:dictionary,Product Setup %>"
    CodeBehind="NewProductMaster.aspx.cs" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="../CustomControl/MultiCheckCombo.ascx" TagName="MultiCheckCombo"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <ajax:scriptmanager id="ScriptManager1" runat="server">
        </ajax:scriptmanager>
    <script type="text/javascript" language="javascript">
        function previewFile() {
            var preview = document.querySelector('#<%=Image2.ClientID %>');
            var file = document.querySelector('#<%=fuItemPicture.ClientID %>').files[0];
            var reader = new FileReader();

            reader.onloadend = function() {
                preview.src = reader.result;
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "";
            }
        }

        function removeFile() {
            var preview = document.querySelector('#<%=Image2.ClientID %>');
            preview.src = "";
            return false;
        }
        var newwindow;
        function poptastic(url) {
            newwindow = window.open(url, 'name', 'height=600,width=1000,resizeable=1,scrollbars=1');
            if (window.focus) { newwindow.focus() }
        }        
    </script>

    <asp:Panel ID="pnlApplicableTo" runat="server">
        <table style="width: 600px" id="Table1">
            <tr style="background: #0000FF">
                <td class="scaffoldEditItemCaption1" style="width: 147px">
                    <asp:Literal ID="ltApplicable" runat="server" Text="<%$Resources:dictionary, Applicable To%>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:DropDownList ID="ddlApplicableTo" runat="server" Width="147px" OnSelectedIndexChanged="ddlApplicableTo_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Value="Product Master" Text="<%$Resources:dictionary, Product Master%>"></asp:ListItem>
                        <asp:ListItem Value="Outlet Group" Text="<%$Resources:dictionary, Outlet Group%>"></asp:ListItem>
                        <asp:ListItem Value="Outlet" Text="<%$Resources:dictionary, Outlet%>"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlOutletList" runat="server" Width="147px" OnSelectedIndexChanged="ddlOutlet_SelectedIndexChanged"
                        AutoPostBack="true" Enabled="false">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlGrid" runat="server">
        <asp:Panel ID="pnlSearch" runat="server" DefaultButton="LinkButton1">
            <table style="width: 600px" id="FieldsTable">
                <tr>
                    <td style="width: 147px">
                        <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
                    </td>
                    <td style="width: 63px" colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlSearchTerm" runat="server" Width="100px">
                                        <asp:ListItem Value="Contains" Text="<%$Resources:dictionary, Contains%>" Selected="True" />
                                        <asp:ListItem Value="StartsWith" Text="<%$Resources:dictionary, Starts With%>" />
                                        <asp:ListItem Value="EndsWith" Text="<%$Resources:dictionary, Ends With%>" />
                                        <asp:ListItem Value="ExactMatch" Text="<%$Resources:dictionary, Exact Match%>" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtItemNo" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="ltr06" runat="server" Text="<%$Resources:dictionary, Category Filter%>" />
                    </td>
                    <td align="left" style="height: 20px" colspan="2">
                        <uc1:MultiCheckCombo ID="MultiCheckCombo1" runat="server" />
                    </td>
                </tr>
                <tr id="trFilterItemName" runat="server" style="background-color: #dddbdc">
                    <td>
                        <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary, Item Name%>" />
                    </td>
                    <td colspan="2">
                        <asp:ListBox ID="ListBoxItemName" runat="server" Width="302px" Height="193px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                </tr>
                <tr id="trLblAttrib1" runat="server">
                    <td>
                        <asp:Label ID="lbl_Attributes1" runat="server" Text="<%$Resources:dictionary, Attributes1%>" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Attributes2" runat="server" Text="<%$Resources:dictionary, Attributes2%>" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Attributes3" runat="server" Text="<%$Resources:dictionary, Attributes3%>" />
                    </td>
                </tr>
                <tr id="trListAttrib1" runat="server">
                    <td>
                        <asp:ListBox ID="ListBox1" runat="server" Width="302px" Height="150px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                    <td>
                        <asp:ListBox ID="ListBox2" runat="server" Width="302px" Height="150px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                    <td>
                        <asp:ListBox ID="ListBox3" runat="server" Width="302px" Height="150px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                </tr>
            </table>
            <table id="tblAttrib2" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="lbl_Attributes4" runat="server" Text="<%$Resources:dictionary, Attributes4%>" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Attributes5" runat="server" Text="<%$Resources:dictionary, Attributes5%>" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Attributes6" runat="server" Text="<%$Resources:dictionary, Attributes6%>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ListBox ID="ListBox4" runat="server" Width="302px" Height="150px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                    <td>
                        <asp:ListBox ID="ListBox5" runat="server" Width="302px" Height="150px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                    <td>
                        <asp:ListBox ID="ListBox6" runat="server" Width="302px" Height="150px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                </tr>
            </table>
            <table id="tblAttrib3" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="lbl_Attributes7" runat="server" Text="<%$Resources:dictionary, Attributes7%>" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Attributes8" runat="server" Text="<%$Resources:dictionary, Attributes8%>" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Attributes9" runat="server" Text="<%$Resources:dictionary, Attributes9%>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ListBox ID="ListBox7" runat="server" Width="302px" Height="150px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                    <td>
                        <asp:ListBox ID="ListBox8" runat="server" Width="302px" Height="150px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                    <td>
                        <asp:ListBox ID="ListBox9" runat="server" Width="302px" Height="150px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                </tr>
            </table>
            <table id="search_ExportTable" style="vertical-align: middle; width: 600px; height: 40px;"
                border="0" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="height: 30px; width: 50%; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                        <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                            <asp:Literal ID="Literal16" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                        </asp:LinkButton><div class="divider">
                        </div>
                        <asp:LinkButton ID="LinkButton4" class="classname" runat="server" OnClick="btnClearFilter_Click">
                            <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:dictionary, Clear%>" />
                        </asp:LinkButton><div class="divider">
                        </div>
                        <asp:LinkButton ID="btnMerge" class="classname" runat="server" OnClick="btnMerge_Click">
                            <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:dictionary, Merge Similar Item%>" />
                        </asp:LinkButton>
                    </td>
                    <td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                        vertical-align: middle; right: 0px;">
                        <asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                            <asp:Literal ID="Literal20" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 25px;">
                        <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Add New%>"
            Width="130px" ID="BtnAddNew" OnClick="BtnAddNew_Click" /><div class="divider">
            </div>
        <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Select All%>"
            Width="130px" ID="BtnSelectAll" OnClick="BtnSelectAll_Click" /><div class="divider">
            </div>
        <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Clear Selection%>"
            ID="BtnClearSelection" OnClick="BtnClearSelection_Click" Width="130px" /><div class="divider">
            </div>
        <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Delete Selected%>"
            ID="BtnDeleteSelection" OnClick="BtnDeleteSelection_Click" Width="130px" /><div class="divider">
            </div>
        &nbsp;
        <div style="height: 10px;">
        </div>
        <asp:GridView ID="GridView1" SkinID="scaffold" runat="server" AllowPaging="false"
            AllowSorting="False" AutoGenerateColumns="False" DataKeyNames="ItemNo" Width="100%"
            OnRowDataBound="GridView1_RowDataBound">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="ItemNo,Userflag1"
                    DataNavigateUrlFormatString="NewProductMaster.aspx?id={0}&&matrixmode={1}" />
                <asp:TemplateField HeaderText="Delete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary,Item No %>">
                </asp:BoundField>
                <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Product Name %>">
                </asp:BoundField>
                <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:dictionary,Category %>">
                </asp:BoundField>
                <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:dictionary,Department %>">
                </asp:BoundField>
                <asp:BoundField DataFormatString="{0:N}" DataField="RetailPrice" HeaderText="<%$Resources:dictionary,Retail Price %>">
                </asp:BoundField>
                <asp:BoundField DataFormatString="{0:N}" DataField="FactoryPrice" HeaderText="<%$Resources:dictionary, Factory Price%>">
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <a id="hlEdit" href="javascript:poptastic('ViewOutletPricing.aspx?ItemNo=<%# Eval("ItemNo")%>');">
                            <asp:Literal ID="ltr01" runat="server" Text="<%$Resources:dictionary, View Outlet Pricing%>" /></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Attributes1" HeaderText="<%$Resources:dictionary, Attributes1%>" />
                <asp:BoundField DataField="Attributes2" HeaderText="<%$Resources:dictionary, Attributes2%>" />
                <asp:BoundField DataField="Attributes3" HeaderText="<%$Resources:dictionary, Attributes3%>" />
                <asp:BoundField DataField="Attributes4" HeaderText="<%$Resources:dictionary, Attributes4%>" />
                <asp:BoundField DataField="Attributes5" HeaderText="<%$Resources:dictionary, Attributes5%>" />
                <asp:BoundField DataField="Attributes6" HeaderText="<%$Resources:dictionary, Attributes6%>" />
                <asp:BoundField DataField="Attributes7" HeaderText="<%$Resources:dictionary, Attributes7%>" />
                <asp:BoundField DataField="Attributes8" HeaderText="<%$Resources:dictionary, Attributes8%>" />
                <asp:BoundField DataField="IsInInventory" HeaderText="<%$Resources:dictionary,Inventory Item %>">
                </asp:BoundField>
                <asp:BoundField DataField="IsNonDiscountable" HeaderText="<%$Resources:dictionary, Non Discountable%>">
                </asp:BoundField>
                <asp:BoundField DataField="PointType" HeaderText="<%$Resources:dictionary, Point Type%>">
                </asp:BoundField>
                <asp:BoundField DataFormatString="{0:N}" DataField="PointAmount" HeaderText="<%$Resources:dictionary, Point Amount%>">
                </asp:BoundField>
                <asp:BoundField DataField="Barcode" HeaderText="<%$Resources:dictionary,Barcode %>">
                </asp:BoundField>
                <asp:BoundField DataField="IsServiceItem" HeaderText="<%$Resources:dictionary,Service Item %>">
                </asp:BoundField>
                <asp:BoundField DataField="GSTRule" HeaderText="<%$Resources:dictionary,GST Rule %>">
                </asp:BoundField>
                <asp:BoundField DataField="Remark" HeaderText="<%$Resources:dictionary,Remark %>">
                </asp:BoundField>
                <asp:BoundField DataField="Supplier" HeaderText="<%$Resources:dictionary,Supplier %>">
                </asp:BoundField>
                <asp:BoundField DataField="UOM" HeaderText="<%$Resources:dictionary, UOM%>"></asp:BoundField>
                <asp:BoundField DataField="CreatedOn" HeaderText="<%$Resources:dictionary,Created On %>"
                    DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}"></asp:BoundField>
                <asp:BoundField DataField="CreatedBy" HeaderText="<%$Resources:dictionary,Created By %>">
                </asp:BoundField>
                <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary,Deleted %>">
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="literal5" runat="server" Text="<%$Resources:dictionary,No Product Created Yet %>"></asp:Literal>
            </EmptyDataTemplate>
        </asp:GridView>
        <div style="width: 100%; padding: 5px; color: Black; background-color: #999999; border-top: 1px solid #666666;"
            align="center">
            <asp:Button ID="btnFirst" runat="server" CssClass="classname" Text="<%$Resources:dictionary,<< First %>"
                OnClick="btnFirst_Click" />
            <asp:Button ID="btnPrev" runat="server" CssClass="classname" Text="<%$Resources:dictionary,< Previous%>"
                OnClick="btnPrev_Click" />
            <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
            <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged" />
            <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal>
            <asp:Label ID="lblPageCount" runat="server" />
            <asp:Button ID="btnNext" runat="server" CssClass="classname" Text="<%$Resources:dictionary,Next > %>"
                OnClick="btnNext_Click" />
            <asp:Button ID="btnLast" runat="server" CssClass="classname" Text="<%$Resources:dictionary,Last >> %>"
                OnClick="btnLast_Click" />
        </div>
    </asp:Panel>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete"
        SelectMethod="FetchAllNonDeletedItems_PlusPointInfo" TypeName="PowerPOS.ItemController">
        <DeleteParameters>
            <asp:Parameter Name="ItemNo" Type="Object" />
        </DeleteParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnlEdit" runat="server" onkeydown="return DisableEnterKey()">
        <cc1:CalendarExtender ID="cldDOB" runat="server" Animated="False" Format="dd MMM yyyy"
            PopupButtonID="ImageButton8" TargetControlID="txtFuturePriceDate"></cc1:CalendarExtender>
        <asp:HiddenField ID="ShowWarningWhenSellingPriceLessThanCostPrice" runat="server" />
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table cellpadding="5" cellspacing="0" width="1000" id="FieldsTable1">
            <tr id="divButton" runat="server" style="background-color: #F1A9A0">
                <td align="left" colspan="4">
                    <asp:Button ID="btnNormal" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnNormal_Click" Text="<%$ Resources:dictionary, Normal %>" />
                    &nbsp;
                    <asp:Button ID="btnMatrix" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnMatrix_Click" Text="<%$ Resources:dictionary, Matrix %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 180px">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Item No %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtItemNoEditor" runat="server" MaxLength="50"></asp:TextBox>
                    <asp:HiddenField ID="UserFlag1" runat="server"></asp:HiddenField>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtItemNoEditor"
                        ErrorMessage="<%$ Resources:dictionary, *Required! %>"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Barcode %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <div id="divBarcodeNormal" runat="server">
                        <asp:TextBox ID="txtBarcode" runat="server" MaxLength="90" onkeydown="KeyDownEventHandler(event, 'txtBarcode');"></asp:TextBox>
                        <asp:Button ID="btnGenerateBarcode" Text="<%$Resources:dictionary,Generate %>" runat="server"
                            CausesValidation="False" CssClass="classname" OnClick="btnGenerateBarcode_Click">
                        </asp:Button>
                    </div>
                    <div id="divBarcodeMatrix" runat="server">
                        <div id="divBarcodeNull" runat="server">
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:dictionary, Barcode will be generated automatically%>"></asp:Label>
                        </div>
                        <div id="divPrefixBarcode" runat="server">
                            <asp:Label ID="lblPrefix" runat="server" Text="<%$Resources:dictionary,Prefix %>"></asp:Label>
                            <asp:TextBox ID="txtPrefix" runat="server" Width="150px"></asp:TextBox>
                            <asp:Button ID="btnPrefixBarcode" Text="<%$Resources:dictionary,Generate Barcode %>"
                                runat="server" CausesValidation="False" CssClass="classname" OnClick="btnPrefixBarcode_Click">
                            </asp:Button>
                            <br />
                            <div id="ContainerGVBarcode" runat="server" class="container" style="width: 550px"
                                onscroll="saveScrollPos();">
                                <asp:HiddenField ID="ContainerGVBarcodeScrollPos" runat="server" />
                                <asp:HiddenField ID="ContainerGVBarcodeIsBottom" runat="server" />
                                <asp:GridView ID="gvBarcode" SkinID="scaffold" runat="server" AllowPaging="false"
                                    ShowHeaderWhenEmpty="true" AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="ItemNo"
                                    Width="100%" OnRowCancelingEdit="gvBarcode_RowCancelingEdit" OnRowEditing="gvBarcode_RowEditing"
                                    OnRowUpdating="gvBarcode_RowUpdating">
                                    <Columns>
                                        <asp:CommandField ShowEditButton="true" CausesValidation="false" />
                                        <asp:TemplateField HeaderText="<%$Resources:dictionary,Item No %>" HeaderStyle-Width="100px"
                                            ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemNoGV" runat="server" Text='<%# Bind("ItemNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$Resources:dictionary,Product Name %>" HeaderStyle-Width="250px"
                                            ItemStyle-Width="250px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemNameGV" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$Resources:dictionary,Barcode %>" HeaderStyle-Width="200px"
                                            ItemStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBarcodeGV" runat="server" Text='<%# Bind("Barcode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtBarcodeGV" runat="server" Text='<%# Bind("Barcode") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Attributes2" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="Attributes3" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="Attributes4" Visible="false"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <div id="divBarcodeWarningMessage" runat="server" visible="false">
                                <asp:Label ID="lblBarcodeWarningMessage" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 101px; height: 19px;">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Item Name %>"></asp:Literal>
                </td>
                <td colspan="3" style="height: 19px;">
                    <asp:TextBox ID="txtItemName" runat="server" MaxLength="290" Width="420px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtItemName"
                        ErrorMessage="<%$Resources:dictionary, Please input Item Name%>" SetFocusOnError="True"></asp:RequiredFieldValidator><br />
                </td>
            </tr>
            <tr>
                <td style="width: 101px">
                    <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary,Category Name %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlCategoryName" runat="server" Width="420px" Height="21px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlCategoryName"
                        ErrorMessage="<%$Resources:dictionary, Please select Category Name%>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 101px">
                    <asp:Literal ID="ltRetailPrice" runat="server" Text="<%$Resources:dictionary,Retail Price %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="txtRetailPrice" runat="server" Height="24px" Width="50px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtRetailPrice"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="500000000" MinimumValue="-100000000" Type="Currency"></asp:RangeValidator>
                </td>
                <td style="width: 101px">
                    <asp:Literal ID="ltCostPrice" runat="server" Text="<%$Resources:dictionary,Factory Price %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFactoryPrice" runat="server" Width="50px" Height="24px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtFactoryPrice"
                        ValidationExpression="((\d+)((\.\d{1,4})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="999999999" MinimumValue="0"></asp:RangeValidator>
                </td>
            </tr>
            <tr id = "rowMinimumSellingPrice" runat="server">
                <td style="width: 101px">
                    <asp:Literal ID="Literal29" runat="server" Text="<%$Resources:dictionary,Minimum Selling Price %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="txtMinimumPrice" runat="server" Height="24px" Width="50px"></asp:TextBox>
                </td>
                <td style="width: 101px">
                </td>
                <td>
                </td>
            </tr>
            <tr id="discountrow1" runat="server" style="background-color: #dddbdc">
                <td class="scaffoldEditItemCaption" style="width: 50px">
                    <asp:Literal ID="lblP1" runat="server" Text="<%$Resources:dictionary, Promotion Price%>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="txtP1" runat="server" Width="70px" Height="24px"></asp:TextBox>*
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr id="discountrow2" runat="server" style="background-color: #dddbdc">
                <td class="scaffoldEditItemCaption" style="width: 50px">
                    <asp:Literal ID="lblP2" runat="server" Text="<%$Resources:dictionary, Staff 20% SPP%>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="txtP2" runat="server" Width="70px" Height="24px"></asp:TextBox>*
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr id="discountrow3" runat="server" style="background-color: #dddbdc">
                <td class="scaffoldEditItemCaption" style="width: 50px">
                    <asp:Literal ID="lblP3" runat="server" Text="<%$Resources:dictionary, Staff 40% RCP%>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="txtP3" runat="server" Width="70px" Height="24px"></asp:TextBox>*
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr id="discountrow4" runat="server" style="background-color: #dddbdc">
                <td class="scaffoldEditItemCaption" style="width: 50px">
                    <asp:Literal ID="lblP4" runat="server" Text="<%$Resources:dictionary, Staff 40% RCP%>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="txtP4" runat="server" Width="70px" Height="24px"></asp:TextBox>*
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr id="discountrow5" runat="server" style="background-color: #dddbdc">
                <td class="scaffoldEditItemCaption" style="width: 50px">
                    <asp:Literal ID="lblP5" runat="server" Text="<%$Resources:dictionary, Staff 40% RCP%>"></asp:Literal>
                </td>
                <td class="scaffoldEditItem">
                    <asp:TextBox ID="txtP5" runat="server" Width="70px" Height="24px"></asp:TextBox>*
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 101px">
                    <asp:Literal ID="ltUOM" runat="server" Text="<%$Resources:dictionary, UOM%>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtUOM" runat="server" Width="50px" Height="24px"></asp:TextBox>
                </td>
                <td style="width: 101px">
                    <asp:Literal ID="Literal23" runat="server" Text="<%$Resources:dictionary,GST %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:DropDownList ID="ddGST" runat="server" Height="20px" Width="130px">
                        <asp:ListItem Value="0" Text="<%$Resources:dictionary, --Please Select--%>"></asp:ListItem>
                        <asp:ListItem Value="Exclusive GST" Text="<%$Resources:dictionary, Exclusive GST%>"></asp:ListItem>
                        <asp:ListItem Value="Inclusive GST" Text="<%$Resources:dictionary, Inclusive GST%>"></asp:ListItem>
                        <asp:ListItem Value="Non GST" Text="<%$Resources:dictionary, Non GST%>"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddGST"
                        ErrorMessage="<%$Resources:dictionary, Select GST mode%>" InitialValue="--Please Select--"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="rowNonDiscountable" runat="server">
                <td>
                    <asp:Literal ID="ltNonDiscountable" runat="server" Text="<%$Resources:dictionary,Is Non Discountable %>"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBox ID="cbIsNonDiscountable" runat="server" Checked="False" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr id="rowPointRedeemable" runat="server">
                <td style="width: 101px">
                    <asp:Literal ID="ltPointRedeemAble" runat="server" Text="<%$Resources:dictionary, Point Redeemable%>"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBox ID="cbPointRedeemable" runat="server" Checked="False" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr id="rowGiveCommission" runat="server">
                <td style="width: 101px">
                    <asp:Literal ID="ltGiveCommision" runat="server" Text="<%$Resources:dictionary,Give Commission %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:CheckBox ID="cbGiveCommission" runat="server" Checked="False" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr id="rowAutoCaptureWeight" runat="server">
                <td style="width: 101px">
                    <asp:Literal ID="ltAutoCaptureWeight" runat="server" Text="<%$Resources:dictionary,Auto Capture Weight %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:CheckBox ID="cbAutoCaptureWeight" runat="server" Checked="False" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 101px">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary, Item Type%>"></asp:Literal>
                </td>
                <td colspan="3">
                    <table cellpadding="5" cellspacing="0" width="100%">
                        <tr style="background-color: #EBEBEB;">
                            <td style="width: 156px; text-align: left">
                                <asp:RadioButton ID="rbProduct" runat="server" Text="<%$Resources:dictionary, Product%>"
                                    GroupName="ItemType" />
                            </td>
                            <td style="width: 101px">
                                &nbsp;
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr id="rowOpenPrice" runat="server" style="background-color: #DDDBDC;">
                            <td style="width: 156px; text-align: left">
                                <asp:RadioButton ID="rbOpenPriceProduct" runat="server" GroupName="ItemType" Text="<%$Resources:dictionary,Open Price Product %>" />
                            </td>
                            <td style="width: 101px">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="rowService" runat="server" style="background-color: #ebebeb;">
                            <td style="width: 156px; text-align: left">
                                <asp:RadioButton ID="rbService" runat="server" Text="<%$Resources:dictionary, Service%>"
                                    GroupName="ItemType" />
                            </td>
                            <td style="width: 101px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr id="rowPointPackage" runat="server" style="background-color: #DDDBDC;">
                            <td style="width: 156px; text-align: left; height: 37px;">
                                <asp:RadioButton ID="rbPoint" runat="server" Text="<%$Resources:dictionary, Point Package%>"
                                    GroupName="ItemType" />
                            </td>
                            <td style="width: 101px; font-weight: bold; height: 37px;">
                                <asp:Literal ID="Literal24" runat="server" Text="Points Get"></asp:Literal>
                                :
                            </td>
                            <td style="height: 37px">
                                <asp:TextBox ID="txtPointGet" runat="server"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtPointGet"
                                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="5000000000"
                                    MinimumValue="0" Type="Currency"></asp:RangeValidator>
                                <asp:Label ID="Label1" runat="server" ForeColor="#009933" Text="<%$Resources:dictionary, * put 0 to follow Retail Price%>"></asp:Label>
                            </td>
                        </tr>
                        <tr id="rowCourse1" runat="server" style="background-color: #ebebeb;">
                            <td rowspan="2" style="width: 156px; text-align: left" valign="top">
                                <asp:RadioButton ID="rbCourse" runat="server" Text="<%$Resources:dictionary, Course Package%>"
                                    GroupName="ItemType" />
                            </td>
                            <td style="width: 101px; font-weight: bold">
                                <asp:Literal ID="Literal25" runat="server" Text="<%$Resources:dictionary, Times Get%>"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTimesGet" runat="server"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator7" runat="server" ControlToValidate="txtTimesGet"
                                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="50000"
                                    MinimumValue="0" Type="Integer"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr id="rowCourse2" runat="server" style="background-color: #ebebeb;">
                            <td style="width: 101px; font-weight: bold" valign="top">
                                <asp:Literal ID="Literal19" runat="server" Text="<%$Resources:dictionary, Breakdown Price%>"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBreakdownPrice" runat="server"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator8" runat="server" ControlToValidate="txtBreakdownPrice"
                                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="5000000000"
                                    MinimumValue="0" Type="Currency"></asp:RangeValidator>
                                <asp:Label ID="Label3" runat="server" ForeColor="#009933" Text="<%$Resources:dictionary, * Individual price of the course%>"></asp:Label><br />
                                <asp:CheckBox runat="server" ID="cbOpenPackace" Text="<%$Resources:dictionary, Is Open Package%>" />
                            </td>
                        </tr>
                        <tr id="rowNonInventoryProduct" runat="server" style="background-color: #DDDBDC;">
                            <td style="width: 156px; text-align: left; height: 37px;">
                                <asp:RadioButton ID="rbNonInventoryProduct" runat="server" Text="<%$Resources:dictionary, Non Inventory Product%>"
                                    GroupName="ItemType" />
                            </td>
                            <td style="width: 101px; font-weight: bold; height: 37px;" colspan="2">
                                <table>
                                    <tr runat="server" style="background-color: #DDDBDC;">
                                        <td style="width: 122px">
                                            <asp:Literal ID="Literal30" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearchItem" runat="server"></asp:TextBox><asp:Button ID="btnSearchItem"
                                                runat="server" Text="Search" class="classname" CausesValidation="False" OnClick="btnSearchItem_Click" />
                                        </td>
                                    </tr>
                                    <tr runat="server" style="background-color: #DDDBDC;">
                                        <td style="width: 100px">
                                            <asp:Literal ID="Literal31" runat="server" Text="<%$Resources:dictionary,Item:  %>"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlItem" runat="server" Width="344px" DataValueField="ItemNo"
                                                DataTextField="ItemName" />
                                            <asp:Button ID="btnSetDeductItem" runat="server" Text="Set Deduct Item" class="classname"
                                                CausesValidation="False" OnClick="btnSetDeductItem_Click" />
                                        </td>
                                    </tr>
                                    <tr runat="server" style="background-color: #DDDBDC;">
                                        <td style="width: 100px">
                                            <asp:Literal ID="Literal32" runat="server" Text="<%$Resources:dictionary,Deducted Item %>"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="DeductedItemNo" runat="server" />
                                            <asp:Label ID="DeductedItemLabel" runat="server" />
                                        </td>
                                    </tr>
                                    <tr runat="server" style="background-color: #DDDBDC;">
                                        <td style="width: 100px">
                                            <asp:Literal ID="Literal33" runat="server" Text="<%$Resources:dictionary,Conversion Qty %>"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="DeductConvRate" runat="server" />
                                            <asp:Label ID="DeductedUOM" runat="server" />
                                            <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="DeductConvRate"
                                                ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="5000000000"
                                                MinimumValue="0" Type="Currency"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                            <asp:Literal ID="Literal34" runat="server" Text="<%$Resources:dictionary,Conversion Type%>"></asp:Literal>
                                        </td>
                                        <td>
                                           <asp:DropDownList ID="ddlDeductConvType" runat="server">
                                                <asp:ListItem Text="Up" Value="false" Selected="True" />
                                                <asp:ListItem Text="Down" Value="true" />
                                           </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="trPreOrder">
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary, Pre-Order%>"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBox ID="cbPreOrder" runat="server" Checked="False" Text="<%$Resources:dictionary, Allow Pre-Order%>" />
                </td>
                <td colspan="2">
                    <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary, Cap Qty:%>"></asp:Literal>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtPreOrderCapQty" runat="server" Width="50px" Height="24px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtPreOrderCapQty"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="5000000000" MinimumValue="0" Type="Double" Display="Dynamic"></asp:RangeValidator>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Literal ID="Literal21" runat="server" Text="<%$Resources:dictionary, Sold Qty:%>"></asp:Literal>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtPreOrderSoldQty" runat="server" Width="50px" Height="24px" Enabled="false"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Literal ID="Literal22" runat="server" Text="<%$Resources:dictionary, Bal Qty:%>"></asp:Literal>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtPreOrderBalQty" runat="server" Width="50px" Height="24px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr runat="server" id="trVendorDelivery">
                <td>
                    <asp:Literal ID="Literal27" runat="server" Text="<%$Resources:dictionary, Vendor Delivery%>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:CheckBox ID="cbVendorDelivery" runat="server" Checked="False" Text="<%$Resources:dictionary, Vendor Delivery%>" />
                </td>
            </tr>
            <tr runat="server" id="trFunding">
                <td>
                    <asp:Literal ID="Literal28" runat="server" Text="<%$Resources:dictionary, Funding%>"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBox ID="cbPAMed" runat="server" Checked="False" Text="<%$Resources:dictionary, PA Medifund%>" />
                    <br />
                    <asp:CheckBox ID="cbSMF" runat="server" Checked="False" Text="<%$Resources:dictionary, SMF%>" />
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtPAMed" runat="server" Width="75px" Enabled="false"></asp:TextBox>
                    <br />
                    <asp:TextBox ID="txtSMF" runat="server" Width="75px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 101px; height: 50px;">
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Item Description %>"></asp:Literal>
                </td>
                <td style="height: 50px;" colspan="3">
                    <asp:TextBox ID="txtItemDesc" runat="server" MaxLength="250" Height="52px" TextMode="MultiLine"
                        Width="344px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 101px; height: 50px;">
                    <asp:Literal ID="Literal26" runat="server" Text="<%$Resources:dictionary,Is Consignment %>"></asp:Literal>
                </td>
                <td style="height: 50px;" colspan="3">
                    <asp:CheckBox ID="chkIsConsigment" runat="server" Text="<%$Resources:dictionary,Is Consignment %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 101px; height: 50px;">
                    <asp:CheckBox ID="IsUsingFixedCOG" runat="server" Text="<%$Resources:dictionary,Is Using Fixed Value for Cost Of Goods %>" />
                </td>
                <td style="height: 50px;">
                    <asp:RadioButton ID="IsFixedCOGPercentage" GroupName="FixedCOGType" runat="server"
                        Checked="true" Text="<%$Resources:dictionary,Margin Percentage %>" />&nbsp;<br /><br />
                    <asp:RadioButton ID="IsFixedCOGValue" GroupName="FixedCOGType" runat="server" Text="<%$Resources:dictionary,Value %>" />
                </td>
                <td style="height: 50px;">
                    <asp:TextBox ID="txtFixedCOGPercentage" runat="server"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator9" runat="server" ControlToValidate="txtFixedCOGPercentage"
                        ErrorMessage="<%$ Resources:dictionary, Enter a Percentage number between 1 - 100 %>" MaximumValue="100"
                        MinimumValue="1" Type="Double"></asp:RangeValidator><br />
                    <asp:TextBox ID="txtFixedCOGValue" runat="server"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator10" runat="server" ControlToValidate="txtFixedCOGValue"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="50000"
                        MinimumValue="1" Type="Currency"></asp:RangeValidator><br />
                </td>
                <td></td>
            </tr>
            <tr id="attributesrowmatrix" runat="server">
                <td style="width: 101px">
                    <asp:Label ID="lblAttributes3Matrix" runat="server" Text="<%$Resources:dictionary, Attributes 3%>"></asp:Label>
                </td>
                <td style="width: 180px">
                    <div id="ContainerAtt3" class="container" runat="server" onscroll="savescrollatt3();">
                        <asp:HiddenField ID="ContainerAtt3ScrollPos" runat="server" />
                        <asp:HiddenField ID="ContainerAtt3IsBottom" runat="server" />
                        <asp:CheckBoxList ID="MatrixAttributes3" name="MatrixAttributes3" runat="server"
                            Width="250px">
                        </asp:CheckBoxList>
                    </div>
                    <asp:CustomValidator ID="RequiredFieldValidator5" runat="server" ClientValidationFunction="ValidateMatrixAttributes3"
                        ErrorMessage="<%$ Resources:dictionary, *Required! %>"></asp:CustomValidator>
                    <br />
                    <asp:TextBox ID="txtAddAtt3" runat="server"></asp:TextBox>
                    <asp:Button CssClass="classname" ID="btnAddAtt3" runat="server" OnClick="btnAddAtt3_Click"
                        Text="<%$ Resources:dictionary, Add To List %>" CausesValidation="false"></asp:Button>
                </td>
                <td style="width: 101px">
                    <asp:Label ID="lblAttributes4Matrix" runat="server" Text="<%$Resources:dictionary, Attributes 4%>"></asp:Label>
                </td>
                <td style="width: 180px">
                    <div id="ContainerAtt4" class="container" runat="server" onscroll="savescrollatt4();">
                        <asp:HiddenField ID="ContainerAtt4ScrollPos" runat="server" />
                        <asp:HiddenField ID="ContainerAtt4IsBottom" runat="server" />
                        <asp:CheckBoxList ID="MatrixAttributes4" name="MatrixAttributes4" runat="server"
                            Width="250px">
                        </asp:CheckBoxList>
                    </div>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateMatrixAttributes4"
                        ErrorMessage="<%$ Resources:dictionary, *Required! %>"></asp:CustomValidator>
                    <br />
                    <asp:TextBox ID="txtAddAtt4" runat="server"></asp:TextBox>
                    <asp:Button CssClass="classname" ID="btnAddAtt4" runat="server" OnClick="btnAddAtt4_Click"
                        Text="<%$ Resources:dictionary, Add To List %>" CausesValidation="false"></asp:Button>
                </td>
            </tr>
            <tr id="attributesrow1" runat="server">
                <td style="width: 101px">
                    <asp:Label ID="lblAttributes1" runat="server" Text=""></asp:Label>
                </td>
                <td style="width: 180px" colspan="3">
                    <asp:TextBox ID="txtAttributes1" runat="server" MaxLength="250" Height="52px" TextMode="MultiLine"
                        Width="344px"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow2" runat="server">
                <td style="width: 101px">
                    <asp:Label ID="lblAttributes2" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAttributes2" runat="server" MaxLength="250" Height="52px" TextMode="MultiLine"
                        Width="344px"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow3" runat="server">
                <td style="width: 101px">
                    <asp:Label ID="lblAttributes3" runat="server" Text=""></asp:Label>
                </td>
                <td style="width: 180px" colspan="3">
                    <asp:TextBox ID="txtAttributes3" runat="server" MaxLength="250" Height="52px" TextMode="MultiLine"
                        Width="344px"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow4" runat="server">
                <td style="width: 101px">
                    <asp:Label ID="lblAttributes4" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAttributes4" runat="server" MaxLength="250" Height="52px" TextMode="MultiLine"
                        Width="344px"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow5" runat="server">
                <td style="width: 101px;">
                    <asp:Label ID="lblAttributes5" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAttributes5" runat="server" MaxLength="250" Height="52px" TextMode="MultiLine"
                        Width="344px"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow6" runat="server">
                <td style="width: 101px;">
                    <asp:Label ID="lblAttributes6" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAttributes6" runat="server" MaxLength="250" Height="52px" TextMode="MultiLine"
                        Width="344px"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow7" runat="server">
                <td style="width: 101px;">
                    <asp:Label ID="lblAttributes7" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAttributes7" runat="server" MaxLength="250" Height="52px" TextMode="MultiLine"
                        Width="344px"></asp:TextBox>
                </td>
            </tr>
            <tr id="attributesrow8" runat="server">
                <td style="width: 101px;">
                    <asp:Label ID="lblAttributes8" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAttributes8" runat="server" MaxLength="250" Height="52px" TextMode="MultiLine"
                        Width="344px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 101px; height: 50px">
                    <asp:Literal ID="Literal17" runat="server" Text="<%$Resources:dictionary,Remark %>"></asp:Literal>
                </td>
                <td colspan="3" style="height: 50px">
                    <asp:TextBox ID="txtRemark" runat="server" Height="133px" MaxLength="250" TextMode="MultiLine"
                        Width="340px"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td style="width: 101px; height: 50px;">
                    <asp:Literal ID="Literal38" runat="server" Text="<%$Resources:dictionary,Excluded Sales Item %>"></asp:Literal>
                </td>
                <td style="height: 50px;" colspan="3">
                    <asp:CheckBox ID="cbIsExcludeProfitLossReport" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td style="width: 101px;">
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Item Picture %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:FileUpload ID="fuItemPicture" runat="server" Height="21px" Width="218px" onchange="previewFile()" />
                    <br />
                    <asp:Image ID="Image2" runat="server" Width="155px" />
                    <br />
                    <asp:Button ID="btnRemoveImage" runat="server" Text="Remove Image" OnClientClick="javascript:return removeFile();"
                        Visible="False" />
                </td>
            </tr>
            <tr style="background-color: #ebebeb;" id="rowSupplier" runat="server">
                <td style="width: 101px; height: 50px">
                    <asp:Literal ID="Literal15" runat="server" Text="<%$Resources:dictionary,Supplier %>"></asp:Literal>
                </td>
                <td colspan="3" style="height: 50px">
                    <asp:DropDownList ID="ddlSupplier" runat="server" Width="420px" Height="21px">
                    </asp:DropDownList>
                </td>
            </tr>
            <asp:panel ID="pnlApplyFuturePrice" runat="server">
            <tr style="background-color: #ebebeb;" id="TrcApplyFuturePrice" runat="server">
                <td style="width: 101px;">
                    <asp:Literal ID="Literal35" runat="server" Text="Future Price"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:CheckBox ID="chkApplyFuturPrice" runat="server" Text="Apply Future Price" OnClick="applyFuturPriceChange(this.checked);" />
                </td>
            </tr>            
            <tr id="trFuturePriceDate" style="background-color: #ebebeb;">
                <td></td>
                <td style="width: 101px;">
                    <asp:Literal ID="Literal36" runat="server" Text="Future Price Commenced On"></asp:Literal>
                </td>
                <td colspan="2" >
                    <asp:TextBox ID="txtFuturePriceDate" runat="server" Width="118px"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png"
                        Style="height: 16px; width: 16px" />
                </td>
            </tr>    
            <tr id="trFutureRetailPrice" style="background-color: #ebebeb;">
                <td></td>
                <td style="width: 101px;">
                    <asp:Literal ID="Literal37" runat="server" Text="Retail Price"></asp:Literal>
                </td>
                <td colspan="2" >
                    <asp:TextBox ID="txtFutureRetailPrice" runat="server" Height="24px" Width="50px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator15" runat="server" ControlToValidate="txtFutureRetailPrice"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="500000000" MinimumValue="-100000000" Type="Currency"></asp:RangeValidator>
                </td>            
            </tr>             
            <tr id="trFuturePrice1" style="background-color: #ebebeb;">
                <td></td>
                <td style="width: 101px;">
                    <asp:Literal ID="lblFutureP1" runat="server" Text="P1"></asp:Literal>
                </td>
                <td colspan="2" >
                    <asp:TextBox ID="txtFutureP1" runat="server" Height="24px" Width="50px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator16" runat="server" ControlToValidate="txtFutureP1"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="500000000" MinimumValue="-100000000" Type="Currency"></asp:RangeValidator>
                </td>            
            </tr>                               
            <tr id="trFuturePrice2" style="background-color: #ebebeb;">
                <td></td>
                <td style="width: 101px;" >
                    <asp:Literal ID="lblFutureP2" runat="server" Text="P2"></asp:Literal>
                </td>
                <td colspan="2" >
                    <asp:TextBox ID="txtFutureP2" runat="server" Height="24px" Width="50px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator11" runat="server" ControlToValidate="txtFutureP2"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="500000000" MinimumValue="-100000000" Type="Currency"></asp:RangeValidator>
                </td>            
            </tr>
            <tr id="trFuturePrice3" style="background-color: #ebebeb;">
                <td></td>
                <td style="width: 101px; ">
                    <asp:Literal ID="lblFutureP3" runat="server" Text="P3"></asp:Literal>
                </td>
                <td colspan="2" >
                    <asp:TextBox ID="txtFutureP3" runat="server" Height="24px" Width="50px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator12" runat="server" ControlToValidate="txtFutureP3"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="500000000" MinimumValue="-100000000" Type="Currency"></asp:RangeValidator>
                </td>            
            </tr>                  
            <tr id="trFuturePrice4" style="background-color: #ebebeb;">
                <td></td>
                <td style="width: 101px; ">
                    <asp:Literal ID="lblFutureP4" runat="server" Text="P4"></asp:Literal>
                </td>
                <td colspan="2" >
                    <asp:TextBox ID="txtFutureP4" runat="server" Height="24px" Width="50px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator13" runat="server" ControlToValidate="txtFutureP4"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="500000000" MinimumValue="-100000000" Type="Currency"></asp:RangeValidator>
                </td>            
            </tr>                  
            <tr id="trFuturePrice5" style="background-color: #ebebeb;">
                <td></td>
                <td style="width: 101px; ">
                    <asp:Literal ID="lblFutureP5" runat="server" Text="P5"></asp:Literal>
                </td>
                <td colspan="2" >
                    <asp:TextBox ID="txtFutureP5" runat="server" Height="24px" Width="50px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator14" runat="server" ControlToValidate="txtFutureP5"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="500000000" MinimumValue="-100000000" Type="Currency"></asp:RangeValidator>
                </td>            
            </tr>
            </asp:panel>
            <tr style="background-color: #dddbdc">
                <td style="width: 101px; height: 50px">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary, Created By%>"></asp:Literal>
                </td>
                <td style="width: 101px; height: 50px" colspan="3">
                    <asp:Literal ID="LtrlCreatedBy" runat="server" Text="-"></asp:Literal>
                </td>
            </tr>
            <tr style="background-color: #dddbdc">
                <td style="width: 101px; height: 50px">
                    <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:dictionary, Created On%>"></asp:Literal>
                </td>
                <td style="width: 101px; height: 50px" colspan="3">
                    <asp:Literal ID="LtrlCreatedOn" runat="server" Text="-"></asp:Literal>
                </td>
            </tr>
            <tr style="background-color: #dddbdc">
                <td style="width: 101px; height: 50px">
                    <asp:Literal ID="ltIsDeleted" runat="server" Text="<%$Resources:dictionary, Is Deleted%>"></asp:Literal>
                </td>
                <td colspan="3" style="height: 50px">
                    <asp:CheckBox ID="chkDeleted" runat="server" Text="<%$Resources:dictionary, Deleted%>" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="4">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        Text="<%$ Resources:dictionary, Save %>" />
                    <asp:Button ID="btnSaveNew" runat="server" CssClass="classname" Text="<%$Resources:dictionary, Save and New%>"
                        OnClick="btnSaveNew_Click" />
                    <asp:Button ID="btnReturn" runat="server" CssClass="classname" Text="<%$Resources:dictionary, Return%>"
                        OnClick="btnReturn_Click" CausesValidation="false" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$ Resources:dictionary, Delete %>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/restoreScrollPosition-1.0.jquery.min.js") %>"></script>--%>

    <script type="text/javascript">
        $(document).ready(function() {
            /*setScrollPos();*/
            $('#ctl00_ContentPlaceHolder1_btnSave, #ctl00_ContentPlaceHolder1_btnSaveNew').bind('click', function() {
                var showWarning = ($('#ctl00_ContentPlaceHolder1_ShowWarningWhenSellingPriceLessThanCostPrice').val() | 0) > 0;
                var sellingPrice = Number($("#ctl00_ContentPlaceHolder1_txtRetailPrice").val());
                var costPrice = Number($("#ctl00_ContentPlaceHolder1_txtFactoryPrice").val());
                return showWarning ? (sellingPrice < costPrice ? confirm('Selling Price is less than cost price. Do you want to save it?') : true) : true;
            });
        });

        function KeyDownEventHandler(e, box) {

            var charCode = (e.which) ? e.which : event.keyCode

            if (charCode == 13) {
                e.preventDefault();
                $('#ctl00_ContentPlaceHolder1_txtItemName').focus();
            }
        }

        function DisableEnterKey() {
            if (event.keyCode == 13) {
                return false;
            }
            else {
                return true;
            }
        }

        function saveScrollPos() {
            // do your thing
            if (typeof isPostBack == "undefined") {
                isPostBack = false;
            }

            if (!isPostBack) {
                var div = document.getElementById('<%=ContainerGVBarcode.ClientID%>');
                document.getElementById('<%=ContainerGVBarcodeIsBottom.ClientID%>').value = "false";
                if (div.scrollTop) {
                    y = div.scrollTop;
                }
                else {
                    y = document.body.scrollTop;
                }

                document.getElementById("<%=ContainerGVBarcodeScrollPos.ClientID%>").value = y;
                console.log(document.getElementById("<%=ContainerGVBarcodeScrollPos.ClientID%>").value);
            } else {
                isPostBack = false;
            }
        }

        function savescrollatt3() {
            // do your thing
            if (typeof isPostBack == "undefined") {
                isPostBack = false;
            }

            if (!isPostBack) {
                var div = document.getElementById('<%=ContainerAtt3.ClientID%>');
                document.getElementById('<%=ContainerAtt3IsBottom.ClientID%>').value = "false";
                if (div.scrollTop) {
                    y = div.scrollTop;
                }
                else {
                    y = document.body.scrollTop;
                }

                console.log(document.getElementById("<%=ContainerAtt3ScrollPos.ClientID%>").value);
                document.getElementById("<%=ContainerAtt3ScrollPos.ClientID%>").value = y;
            } else {
                isPostBack = false;
            }
        }

        function savescrollatt4() {
            // do your thing
            if (typeof isPostBack == "undefined") {
                isPostBack = false;
            }

            if (!isPostBack) {
                var div = document.getElementById('<%=ContainerAtt4.ClientID%>');
                document.getElementById('<%=ContainerAtt4IsBottom.ClientID%>').value = "false";
                if (div.scrollTop) {
                    y = div.scrollTop;
                }
                else {
                    y = document.body.scrollTop;
                }

                console.log(document.getElementById("<%=ContainerAtt4ScrollPos.ClientID%>").value);
                document.getElementById("<%=ContainerAtt4ScrollPos.ClientID%>").value = y;
            } else {
                isPostBack = false;
            }
        }

        function setScrollPos() {
            setTimeout(function() {
                if (document.getElementById("<%=ContainerGVBarcodeScrollPos.ClientID%>") != null) {
                    console.log(document.getElementById("<%=ContainerGVBarcodeScrollPos.ClientID%>").value);
                    var div = document.getElementById('<%=ContainerGVBarcode.ClientID%>');
                    div.scrollTop = document.getElementById("<%=ContainerGVBarcodeScrollPos.ClientID%>").value;
                }

                console.log(document.getElementById("<%=ContainerAtt3ScrollPos.ClientID%>").value);
                var div3 = document.getElementById('<%=ContainerAtt3.ClientID%>');
                div3.scrollTop = parseInt(document.getElementById("<%=ContainerAtt3ScrollPos.ClientID%>").value) + 20;

                console.log(document.getElementById("<%=ContainerAtt4ScrollPos.ClientID%>").value);
                var div4 = document.getElementById('<%=ContainerAtt4.ClientID%>');
                div4.scrollTop = parseInt(document.getElementById("<%=ContainerAtt4ScrollPos.ClientID%>").value) + 20;
            }, 1200);
        }
    </script>

    <script type="text/javascript">
        function ValidateMatrixAttributes3(sender, args) {
            var checkBoxList = document.getElementById("<%=MatrixAttributes3.ClientID %>");
            var checkboxes = checkBoxList.getElementsByTagName("input");
            var isValid = false;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    isValid = true;
                    break;
                }
            }
            args.IsValid = isValid;
        }

        function ValidateMatrixAttributes4(sender, args) {
            var checkBoxList = document.getElementById("<%=MatrixAttributes4.ClientID %>");
            var checkboxes = checkBoxList.getElementsByTagName("input");
            var isValid = false;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    isValid = true;
                    break;
                }
            }
            args.IsValid = isValid;
        }

        function applyFuturPriceChange(checked) {
            var isChecked = document.getElementById('ctl00_ContentPlaceHolder1_chkApplyFuturPrice').checked;
            console.log('Future Price : ' + isChecked);

            var display = 'table-row';
            if (!checked)
                display = 'none';
            document.getElementById('trFuturePriceDate').style.display = display;
            document.getElementById('trFutureRetailPrice').style.display = display;
            document.getElementById('trFuturePrice1').style.display = display;
            document.getElementById('trFuturePrice2').style.display = display;
            document.getElementById('trFuturePrice3').style.display = display;
            document.getElementById('trFuturePrice4').style.display = display;
            document.getElementById('trFuturePrice5').style.display = display;
        }
        document.addEventListener("DOMContentLoaded", function(event) {
            //do work
            var isChecked = document.getElementById('ctl00_ContentPlaceHolder1_chkApplyFuturPrice').checked;
            console.log('Future Price : ' + isChecked);
            applyFuturPriceChange(isChecked);
        });
    </script>

</asp:Content>
