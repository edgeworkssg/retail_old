<%@ Page Title="<%$Resources:dictionary, Item Group Promo%>" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="ItemGroupPromo.aspx.cs" Inherits="PowerWeb.Promo.ItemGroupPromo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" name="BaseUrl" id="BaseUrl" value="<%= ResolveUrl("~/") %>" /><asp:Panel
        ID="pnlIndex" runat="server">
        <table width="600px" id="FilterTable">
            <tr>
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server" Width="172px"></asp:TextBox>
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
                    </asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="height: 25px;">
                    <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
            </tr>
        </table>
        <input id="addNew" runat="server" class="classname" onclick="location.href='ItemGroupPromo.aspx?id=0'"
            type="button" width="130px" value="<%$Resources:dictionary, Add New%>" /><div class="divider">
            </div>
        <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Select All %>"
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
        <asp:GridView ID="GridView1" SkinID="scaffold" runat="server" AllowPaging="True"
            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ItemGroupId" PageSize="20"
            Width="400px" OnRowDataBound="GridView1_RowDataBound" OnDataBound="GridView1_DataBound"
            OnPageIndexChanging="GridView1_PageIndexChanging">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="ItemGroupId"
                    DataNavigateUrlFormatString="ItemGroupPromo.aspx?id={0}" />
                <asp:TemplateField HeaderText="<%$Resources:dictionary, Delete%>" ItemStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemGroupID" runat="server" Text='<%# Eval("ItemGroupId")%>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ItemGroupName" HeaderText="<%$Resources:dictionary, Item Group Name%>"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="literal5" runat="server" Text="No Item Group Created Yet"></asp:Literal></EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal><asp:DropDownList
                        ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                        ID="lblPageCount" runat="server"></asp:Label><asp:Button ID="btnNext" runat="server"
                            CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next"
                            CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                        CommandArgument="Last" CommandName="Page" />
                </div>
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <asp:HiddenField ID="ItemGroupId" runat="server" />
        <table cellpadding="5" cellspacing="0" width="1000" id="FieldsTable1">
            <tr>
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Item Group %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,Barcode %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtBarcode" runat="server" Width="172px"></asp:TextBox><%--<asp:RequiredFieldValidator
                        ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtBarcode" ErrorMessage="*Please input Barcode"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Group Name %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtGroupName" runat="server" Width="151px"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtGroupName"
                        ErrorMessage="*Please input Group Name" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Item Importer %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="fieldname" style="width: 300px">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Export Template for Items%>" />
                </td>
                <td>
                    <asp:Button ID="btnExportBlank" CausesValidation="false" runat="server" Text="<%$Resources:dictionary, Export%>"
                        OnClick="btnExportBlank_Click" />
                </td>
            </tr>
            <tr>
                <td class="fieldname">
                    <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Add Item to Group%>" />
                </td>
                <td>
                    <asp:FileUpload ID="fuItemImporter" runat="server" />
                    &nbsp;&nbsp;<asp:Button ID="btnImport" CausesValidation="false" runat="server" Text="<%$Resources:dictionary, Add Item%>"
                        OnClick="btnImport_Click" />
                </td>
            </tr>
            <tr>
                <td class="fieldname" colspan="2" style="text-align: center!important;">
                    <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal15" runat="server" Text="<%$Resources:dictionary,Item %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtSearchItem" runat="server"></asp:TextBox><asp:Button ID="btnSearchItem"
                        runat="server" Text="Search" class="classname" CausesValidation="False" OnClick="btnSearchItem_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Item:  %>"></asp:Literal>
                </td>
                <td>
                    <%-- <subsonic:DropDown ID="ddsItem" runat="server" ValueField="ItemNo" TextField="ItemName"
                        TableName="Item" DataValueField="ItemNo" DataTextField="ItemName" Width="344px">
                    </subsonic:DropDown>--%>
                    <asp:DropDownList ID="ddlItem" runat="server" Width="344px" DataValueField="ItemNo"
                        DataTextField="ItemName" />
                </td>
            </tr>
            <%-- <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Qty: %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtUnitQty" runat="server" Width="97px">1</asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtUnitQty"
                        ErrorMessage="<%$ Resources:dictionary, Enter a number %>" MaximumValue="65535"
                        MinimumValue="0" Type="Integer"></asp:RangeValidator>
                </td>
            </tr>--%>
            <tr>
                <td colspan="4">
                    <asp:Button ID="btnAddItem" runat="server" Text="<%$ Resources:dictionary, Include Item to Group %>"
                        CausesValidation="False" class="classname" OnClick="btnAddItem_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView ID="gvItem" runat="server" AutoGenerateColumns="False" SkinID="scaffold"
                        OnRowUpdating="gvItem_RowUpdating" OnRowEditing="gvItem_RowEditing" OnRowCancelingEdit="gvItem_RowCancelingEdit"
                        Width="500px" OnRowDataBound="gvItem_RowDataBound" OnRowDeleting="gvItem_RowDeleting">
                        <Columns>
                            <%--<asp:CommandField ShowEditButton="true" CausesValidation="false" HeaderText="<%$Resources:dictionary,Edit%>" />--%>
                            <asp:CommandField ShowDeleteButton="true" CausesValidation="false" HeaderText="<%$Resources:dictionary,Delete%>" />
                            <asp:BoundField DataField="ItemNo" ReadOnly="true" HeaderText="<%$Resources:dictionary,Item No %>"
                                SortExpression="ItemNo" />
                            <asp:BoundField DataField="ItemName" ReadOnly="true" HeaderText="<%$Resources:dictionary,Item Name %>" />
                            <%--<asp:TemplateField HeaderText="<%$Resources:dictionary,Quantity %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblQtyGV" runat="server" Text='<%# Bind("UnitQty") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtQtyGV" runat="server" Text='<%# Bind("UnitQty") %>' Width="50px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="Price" ReadOnly="true" HeaderText="<%$Resources:dictionary,Retail Price %>" />
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeletedGVDetails" runat="server" Text='<%# Eval("Deleted")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        CausesValidation="true" Text="<%$ Resources:dictionary, Save %>" />
                    &nbsp;
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='ItemGroupPromo.aspx'"
                        type="button" value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$ Resources:dictionary, Delete %>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
