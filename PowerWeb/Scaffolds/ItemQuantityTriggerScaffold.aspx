<%@ Page Theme="Default" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    Inherits="ItemQuantityTriggerScaffold" Title="<%$Resources:dictionary,Item Quantity Warning Setup %>"
    CodeBehind="ItemQuantityTriggerScaffold.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="pnlGrid" runat="server">
        <div style="height: 20px; width: 800px;" class="wl_pageheaderSub">
            <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
        </div>
        <table width="800px" id="FilterTable">
            <tr>
                <td>
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary, Inventory Location%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlInventoryLocation" runat="server" Width="150px" OnInit="ddlInventoryLocation_Init">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="fieldname" style="width: 300px">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary, Search%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtSearchItem" runat="server" Width="170px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnSearchItem" runat="server" CssClass="classname" Text="<%$ Resources:dictionary, Search %>"
            OnClick="btnSearchItem_Click" />
        <input id="btnAdd" runat="server" class="classname" onclick="location.href='ItemQuantityTriggerScaffold.aspx?id=0'"
            type="button" value="<%$Resources:dictionary, Add New%>" />
        <div style="height: 7px;">
        </div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="TriggerID" PageSize="50"
            SkinID="scaffold" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="TriggerId"
                    DataNavigateUrlFormatString="ItemQuantityTriggerScaffold.aspx?id={0}" />
                <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary, Item No%>"
                    SortExpression="ItemNo"></asp:BoundField>
                <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary, Item Name%>"
                    SortExpression="ItemName"></asp:BoundField>
                 <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:dictionary, Category Name%>"
                    SortExpression="CategoryName"></asp:BoundField>
                <asp:BoundField DataField="TriggerQuantity" HeaderText="<%$Resources:dictionary, Min Quantity%>"
                    SortExpression="TriggerQuantity"></asp:BoundField>
                <asp:BoundField DataField="TriggerLevel" HeaderText="<%$Resources:dictionary, Max Quantity%>"
                    SortExpression="TriggerLevel"></asp:BoundField>
                <asp:BoundField DataField="InventoryLocationId" Visible="false" HeaderText="<%$Resources:dictionary, Inventory Location Id%>"
                    SortExpression="InventoryLocationId"></asp:BoundField>
                <asp:BoundField DataField="InventoryLocationName" HeaderText="<%$Resources:dictionary, Inventory Location Name%>"
                    SortExpression="InventoryLocationName"></asp:BoundField>
                <asp:BoundField DataField="Userfld1" HeaderText="<%$Resources:dictionary, Userfld1%>"
                    SortExpression="Userfld1"></asp:BoundField>
                <asp:BoundField DataField="Userfld2" HeaderText="<%$Resources:dictionary, Userfld2%>"
                    SortExpression="Userfld2"></asp:BoundField>
                <asp:BoundField DataField="Userfld3" HeaderText="<%$Resources:dictionary, Userfld3%>"
                    SortExpression="Userfld3"></asp:BoundField>
                <asp:BoundField DataField="Userfld4" HeaderText="<%$Resources:dictionary, Userfld4%>"
                    SortExpression="Userfld4"></asp:BoundField>
                <asp:BoundField DataField="Userfld5" HeaderText="<%$Resources:dictionary, Userfld5%>"
                    SortExpression="Userfld5"></asp:BoundField>
                <asp:BoundField DataField="Userfld6" HeaderText="<%$Resources:dictionary, Userfld6%>"
                    SortExpression="Userfld6"></asp:BoundField>
                <asp:BoundField DataField="Userfld7" HeaderText="<%$Resources:dictionary, Userfld7%>"
                    SortExpression="Userfld7"></asp:BoundField>
                <asp:BoundField DataField="Userfld8" HeaderText="<%$Resources:dictionary, Userfld8%>"
                    SortExpression="Userfld8"></asp:BoundField>
                <asp:BoundField DataField="Userfld9" HeaderText="<%$Resources:dictionary, Userfld9%>"
                    SortExpression="Userfld9"></asp:BoundField>
                <asp:BoundField DataField="Userfld10" HeaderText="<%$Resources:dictionary, Userfld10%>"
                    SortExpression="Userfld10"></asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, No Item Quantity Trigger%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary, Page%>" />
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, of%>" />
                    <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Next >%>"
                        CommandArgument="Next" CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Last >>%>"
                        CommandArgument="Last" CommandName="Page" />
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server" CssClass="LabelMessage"></asp:Label>
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
            <tr>
                <td>
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,TriggerID %>" />
                </td>
                <td>
                    <asp:Label ID="lblID" runat="server" Text="[ID]"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary, Search%>"></asp:Literal>
                </td>
                <td style="width: 400px">
                    <asp:TextBox ID="txtItemNo" runat="server" Width="147px"></asp:TextBox><div class="divider">
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
                    <asp:DropDownList ID="ctrlItem" runat="server" Width="305px">
                    </asp:DropDownList>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary, Min Quantity%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlQuantity" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary, Max Quantity%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlTriggerLevel" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary, Inventory Location%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ctrlInventoryLocation" runat="server" Width="305px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trUserfld1" runat="server">
                <td>
                    <asp:Literal ID="lblUserfld1" runat="server" Text="<%$Resources:dictionary, Userfld1%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtUserfld1" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr id="trUserfld2" runat="server">
                <td>
                    <asp:Literal ID="lblUserfld2" runat="server" Text="<%$Resources:dictionary, Userfld2%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtUserfld2" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr id="trUserfld3" runat="server">
                <td>
                    <asp:Literal ID="lblUserfld3" runat="server" Text="<%$Resources:dictionary, Userfld3%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtUserfld3" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr id="trUserfld4" runat="server">
                <td>
                    <asp:Literal ID="lblUserfld4" runat="server" Text="<%$Resources:dictionary, Userfld4%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtUserfld4" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr id="trUserfld5" runat="server">
                <td>
                    <asp:Literal ID="lblUserfld5" runat="server" Text="<%$Resources:dictionary, Userfld5%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtUserfld5" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr id="trUserfld6" runat="server">
                <td>
                    <asp:Literal ID="lblUserfld6" runat="server" Text="<%$Resources:dictionary, Userfld6%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtUserfld6" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr id="trUserfld7" runat="server">
                <td>
                    <asp:Literal ID="lblUserfld7" runat="server" Text="<%$Resources:dictionary, Userfld7%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtUserfld7" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr id="trUserfld8" runat="server">
                <td>
                    <asp:Literal ID="lblUserfld8" runat="server" Text="<%$Resources:dictionary, Userfld8%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtUserfld8" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr id="trUserfld9" runat="server">
                <td>
                    <asp:Literal ID="lblUserfld9" runat="server" Text="<%$Resources:dictionary, Userfld9%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtUserfld9" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr id="trUserfld10" runat="server">
                <td>
                    <asp:Literal ID="lblUserfld10" runat="server" Text="<%$Resources:dictionary, Userfld10%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtUserfld10" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary, Created By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Created On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary, Modified By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Modified On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary,Deleted %>" />
                </td>
                <td>
                    <asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        Text="<%$Resources:dictionary, Save%>" />
                    &nbsp;
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='ItemQuantityTriggerScaffold.aspx'"
                        type="button" value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary, Delete%>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
