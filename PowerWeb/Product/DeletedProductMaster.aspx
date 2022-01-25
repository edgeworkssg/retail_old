<%@ Page Language="C#" Theme="Default" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    Inherits="DeletedProductMaster" Title="<%$Resources:dictionary, Deleted Product List%>" CodeBehind="DeletedProductMaster.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="pnlGrid" runat="server">
        <table style="width: 650px" id="FieldsTable">
            <%--    scaffoldEditItemCaption  scaffoldEditItemCaption  scaffoldEditItem scaffoldEditTable--%>
            <tr>
                <td style="width: 170px">
                    <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
                </td>
                <td class="" style="width: 330px">
                    <asp:TextBox ID="txtItemNo" runat="server" Width="200px"></asp:TextBox><div class="divider">
                    </div>
                    <asp:Button ID="btnSearch" runat="server" class="classname" OnClick="btnSearch_Click"
                        Text="<%$ Resources:dictionary, Search %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 170px">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Search By Item Name %>"></asp:Literal>
                </td>
                <td style="width: 330px">
                    <subsonic:DropDown ID="ddlItemName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDown1_SelectedIndexChanged"
                        OrderField="ItemName" ShowPrompt="True" TableName="Item" TextField="ItemName"
                        ValueField="ItemNo" Width="305px">
                    </subsonic:DropDown>
                </td>
            </tr>
        </table>
        <table style="width: 650px">
            <tr>
                <td colspan="2" he>
                    <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:LinkButton ID="lnkExport" class="classBlue" runat="server" OnClick="lnkExport_Click"
                        Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <div style="height: 7px;">
        </div>
        <asp:GridView ID="GridView1" SkinID="scaffold" runat="server" AllowPaging="True"
            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ItemNo" PageSize="20"
            Width="100%" OnRowDataBound="GridView1_RowDataBound" OnDataBound="GridView1_DataBound"
            OnPageIndexChanging="GridView1_PageIndexChanging">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="ItemNo" DataNavigateUrlFormatString="DeletedProductMaster.aspx?id={0}" />
                <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary,Item No %>">
                </asp:BoundField>
                <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary,Product Name %>">
                </asp:BoundField>
                <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:dictionary,Category %>">
                </asp:BoundField>
                <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:dictionary,Department %>">
                </asp:BoundField>
                <asp:BoundField DataField="RetailPrice" HeaderText="<%$Resources:dictionary,Retail Price %>">
                </asp:BoundField>
                <asp:BoundField DataField="IsInInventory" HeaderText="<%$Resources:dictionary,Inventory Item %>">
                </asp:BoundField>
                <asp:BoundField DataField="IsNonDiscountable" HeaderText="<%$Resources:dictionary,Non Discountable %>">
                </asp:BoundField>
                <asp:BoundField DataField="Attributes1" HeaderText=""></asp:BoundField>
                <asp:BoundField DataField="Attributes2" HeaderText=""></asp:BoundField>
                <asp:BoundField DataField="Attributes3" HeaderText=""></asp:BoundField>
                <asp:BoundField DataField="Attributes4" HeaderText=""></asp:BoundField>
                <asp:BoundField DataField="Attributes5" HeaderText=""></asp:BoundField>
                <asp:BoundField DataField="PointType" HeaderText="<%$Resources:dictionary, Point Type%>"></asp:BoundField>
                <asp:BoundField DataField="PointAmount" HeaderText="<%$Resources:dictionary, Point Amount%>"></asp:BoundField>
                <asp:BoundField DataField="Barcode" HeaderText="<%$Resources:dictionary,Barcode %>">
                </asp:BoundField>
                <asp:BoundField DataField="IsServiceItem" HeaderText="<%$Resources:dictionary,Service Item %>">
                </asp:BoundField>
                <asp:BoundField DataField="GSTRule" HeaderText="<%$Resources:dictionary,GST Rule %>">
                </asp:BoundField>
                <asp:BoundField DataField="Remark" HeaderText="<%$Resources:dictionary,Remark %>">
                </asp:BoundField>
                <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary,Deleted %>">
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="literal5" runat="server" Text="<%$Resources:dictionary,No Product Created Yet %>"></asp:Literal>
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <asp:Button ID="btnFirst" runat="server" CssClass="classBlue" Text="<%$Resources:dictionary,<< First %>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="classBlue" Text="<%$Resources:dictionary,< Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                    <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                        ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="classBlue" Text="<%$Resources:dictionary,Next > %> "
                        CommandArgument="Next" CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="classBlue" Text="<%$Resources:dictionary,Last >> %> "
                        CommandArgument="Last" CommandName="Page" />
                </div>
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete"
        SelectMethod="FetchAllNonDeletedItems_PlusPointInfo" TypeName="PowerPOS.ItemController">
        <DeleteParameters>
            <asp:Parameter Name="ItemNo" Type="Object" />
        </DeleteParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table cellpadding="5" cellspacing="0" width="800">
            <tr>
                <td style="width: 101px">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Item No %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="txtItemNoEditor" runat="server" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtItemNoEditor"
                        ErrorMessage="<%$ Resources:dictionary, *Required! %>"></asp:RequiredFieldValidator>
                </td>
                <td style="width: 101px">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Barcode %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtBarcode" runat="server" MaxLength="90"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 101px; height: 19px;">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Item Name %>"></asp:Literal>
                </td>
                <td colspan="3" style="height: 19px;">
                    <asp:TextBox ID="txtItemName" runat="server" MaxLength="290" Width="449px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtItemName"
                        ErrorMessage="*Please input Item Name" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 101px">
                    <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary,Category Name %>"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlCategoryName" runat="server" Width="449px" Height="21px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlCategoryName"
                        ErrorMessage="*Please select Category Name" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 101px">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Retail Price %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="txtRetailPrice" runat="server" Height="24px" Width="50px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtRetailPrice"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="500,000,000"
                        MinimumValue="-100,000,000" Type="Currency"></asp:RangeValidator>
                </td>
                <td style="width: 101px">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,Factory Price %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtFactoryPrice" runat="server" Width="50px" Height="24px"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtFactoryPrice"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="5,000,000,000"
                        MinimumValue="0" Type="Currency"></asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 101px">
                    <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:dictionary,Is Non Discountable %>"></asp:Literal>
                </td>
                <td style="width: 180px">
                    <asp:CheckBox ID="cbIsNonDiscountable" runat="server" Checked="False" />
                </td>
                <td style="width: 101px">
                    <asp:Literal ID="Literal23" runat="server" Text="<%$Resources:dictionary,GST %>"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddGST" runat="server" Height="20px" Width="130px">
                        <asp:ListItem>--Please Select--</asp:ListItem>
                        <asp:ListItem>Exclusive GST</asp:ListItem>
                        <asp:ListItem>Inclusive GST</asp:ListItem>
                        <asp:ListItem>Non GST</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddGST"
                        ErrorMessage="*Select GST mode" InitialValue="--Please Select--" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 101px">
                    <asp:Literal ID="Literal11" runat="server" Text="Item Type"></asp:Literal>
                </td>
                <td colspan="3">
                    <table cellpadding="5" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 156px; text-align: left">
                                <asp:RadioButton ID="rbProduct" runat="server" Text="Product" GroupName="ItemType" />
                            </td>
                            <td style="width: 101px">
                                &nbsp;
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 156px; text-align: left">
                                <asp:RadioButton ID="rbOpenPriceProduct" runat="server" GroupName="ItemType" Text="Open Price Product" />
                            </td>
                            <td style="width: 101px">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 156px; text-align: left">
                                <asp:RadioButton ID="rbService" runat="server" Text="Service" GroupName="ItemType" />
                            </td>
                            <td style="width: 101px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 156px; text-align: left; height: 37px;">
                                <asp:RadioButton ID="rbPoint" runat="server" Text="Point Package" GroupName="ItemType" />
                            </td>
                            <td style="width: 101px; font-weight: bold; height: 37px;">
                                <asp:Literal ID="Literal24" runat="server" Text="Points Get"></asp:Literal>
                                :
                            </td>
                            <td style="height: 37px">
                                <asp:TextBox ID="txtPointGet" runat="server"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtPointGet"
                                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="5,000,000,000"
                                    MinimumValue="0" Type="Currency"></asp:RangeValidator>
                                <asp:Label ID="Label1" runat="server" ForeColor="#009933" Text="* put 0 to follow Retail Price"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width: 156px; text-align: left">
                                <asp:RadioButton ID="rbCourse" runat="server" Text="Course Package" GroupName="ItemType" />
                            </td>
                            <td style="width: 101px; font-weight: bold">
                                <asp:Literal ID="Literal25" runat="server" Text="Times Get"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTimesGet" runat="server"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator7" runat="server" ControlToValidate="txtTimesGet"
                                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="50000"
                                    MinimumValue="0" Type="Integer"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 101px; font-weight: bold">
                                <asp:Literal ID="Literal19" runat="server" Text="Breakdown Price"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBreakdownPrice" runat="server"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator8" runat="server" ControlToValidate="txtBreakdownPrice"
                                    ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="5,000,000,000"
                                    MinimumValue="0" Type="Currency"></asp:RangeValidator>
                                <asp:Label ID="Label3" runat="server" ForeColor="#009933" Text="* Individual price of the course"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 101px;">
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Item Picture %>"></asp:Literal>
                </td>
                <td colspan="2">
                    <asp:FileUpload ID="FileUpload1" runat="server" Enabled="False" Height="21px" Width="218px" />
                </td>
                <td rowspan="2" style="width: 287px">
                    <asp:Image ID="ItemImage" runat="server" Height="100px" Width="100px" />
                </td>
            </tr>
            <tr>
                <td style="width: 101px; height: 50px;">
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Item Description %>"></asp:Literal>
                </td>
                <td style="height: 50px;" colspan="2">
                    <asp:TextBox ID="txtItemDesc" runat="server" MaxLength="250" Height="52px" TextMode="MultiLine"
                        Width="344px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 101px">
                    <asp:Label ID="lblAttributes1" runat="server" Text=""></asp:Label>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="txtAttributes1" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td style="width: 101px">
                    <asp:Label ID="lblAttributes2" runat="server" Text=""></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAttributes2" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 101px">
                    <asp:Label ID="lblAttributes3" runat="server" Text=""></asp:Label>
                </td>
                <td style="width: 180px">
                    <asp:TextBox ID="txtAttributes3" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td style="width: 101px">
                    <asp:Label ID="lblAttributes4" runat="server" Text=""></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAttributes4" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 101px;">
                    <asp:Label ID="lblAttributes5" runat="server" Text=""></asp:Label>
                </td>
                <td style="width: 180px;">
                    <asp:TextBox ID="txtAttributes5" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td style="width: 101px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 101px; height: 50px">
                    <asp:Literal ID="Literal17" runat="server" Text="<%$Resources:dictionary,Remark %>"></asp:Literal>
                </td>
                <td colspan="2" style="height: 50px">
                    <asp:TextBox ID="txtRemark" runat="server" Height="133px" MaxLength="250" TextMode="MultiLine"
                        Width="340px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="3">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        Text="<%$ Resources:dictionary, Save %>" />
                    &nbsp;
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='DeletedProductMaster.aspx'" type="button"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary, Un-Delete%>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
