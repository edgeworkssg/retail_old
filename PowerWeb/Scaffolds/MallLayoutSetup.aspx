<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="MallLayoutSetup.aspx.cs" Inherits="PowerWeb.Scaffolds.MallLayoutSetup"
    Title="<%$Resources:dictionary, Mall Layout Setup%>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript">
        function SetShopLevel(val) {
            $("#ctl00_ContentPlaceHolder1_txtShopLevel").val(val);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <table width="800px" id="FilterTable">
            <tr>
                <td colspan="4" class="wl_pageheaderSub">
                    <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="fieldname">
                    <asp:Label ID="lblOutletFilter" runat="server" Text="<%$Resources:dictionary, Outlet%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlFilterOutlet" runat="server" DataTextField="OutletName"
                        DataValueField="OutletName" Width="200px" OnInit="ddlFilterOutlet_Init" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlFilterOutlet_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;
                </td>
                <td class="fieldname">
                    <asp:Label ID="lblShopLevel" runat="server" Text="<%$Resources:dictionary, Shop Level%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlFilterRetailerLevel" runat="server" Width="200px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="fieldname">
                    <asp:Label ID="lblOutletFilter2" runat="server" Text="<%$Resources:dictionary, Shop No%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtFilterShopNo" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td class="fieldname" colspan="2">
                    <asp:CheckBox ID="chkShowDeletedItems" runat="server" Text="<%$Resources:dictionary, Show Deleted Items%>" />
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <input id="btnAddNew" runat="server" class="classname" onclick="location.href='MallLayoutSetup.aspx?id=0'"
                        type="button" value="<%$Resources:dictionary, Add New%>" oninit="btnAddNew_Init" />&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 15px">
                    <asp:Button ID="btnSearch" runat="server" CssClass="classname" OnClick="btnSearch_Click"
                        Text="<%$ Resources:dictionary, Search %>" />
                    <asp:Button ID="btnClear" runat="server" CssClass="classname" OnClick="btnClear_Click"
                        Text="<%$ Resources:dictionary, Clear %>" />
                    &nbsp;
                </td>
                <td align="right" colspan="2">
                    <asp:LinkButton ID="lnkExport" runat="server" CssClass="classBlue" OnClick="lnkExport_Click"
                        Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView Width="800px" ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="RetailerLevelID"
            PageSize="20" SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit %>" DataNavigateUrlFields="RetailerLevelID" DataNavigateUrlFormatString="MallLayoutSetup.aspx?id={0}" />
                <asp:BoundField DataField="OutletName" HeaderText="<%$Resources:dictionary, Outlet%>" SortExpression="OutletName" />
                <asp:BoundField DataField="ShopLevel" HeaderText="<%$Resources:dictionary, Shop Level%>" SortExpression="ShopLevel" />
                <asp:BoundField DataField="ShopNo" HeaderText="<%$Resources:dictionary, Shop No%>" SortExpression="ShopNo" />
                <asp:BoundField DataField="ShopArea" HeaderText="<%$Resources:dictionary, Shop Area (sqft)%>" SortExpression="ShopArea" />
                <asp:BoundField Visible="false" DataField="Attribute1" HeaderText="<%$Resources:dictionary, Attribute1%>" SortExpression="Attribute1" />
                <asp:BoundField Visible="false" DataField="Attribute2" HeaderText="<%$Resources:dictionary, Attribute2%>" SortExpression="Attribute2" />
                <asp:CheckBoxField DataField="Deleted" HeaderText="<%$Resources:dictionary, Deleted%>" SortExpression="Deleted" />
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, No Retailer Level%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Page%>" />
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, of%>" />
                    <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Next >%>" CommandArgument="Next"
                        CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Last >>%>"
                        CommandArgument="Last" CommandName="Page" />
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="800px">
            <tr>
                <td>
                    <asp:Label ID="lblOutletGroupID" runat="server" Text="<%$Resources:dictionary, Retailer Level ID%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtRetailerLevelID" runat="server" Width="150px" Enabled="false"></asp:TextBox><asp:HiddenField
                        ID="hdfID" runat="server" Value="0" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblOutletName" runat="server" Text="<%$Resources:dictionary, Outlet%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlOutletName" runat="server" Width="150px" DataValueField="OutletName"
                        DataTextField="OutletName" OnInit="ddlOutletName_Init" AutoPostBack="true" OnSelectedIndexChanged="ddlOutletName_SelectedIndexChanged" />
                </td>
            </tr>
            <tr id="trShopLevel" runat="server">
                <td>
                    <asp:Label ID="lbl" runat="server" Text="<%$Resources:dictionary, Shop Level%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlShopLevel" runat="server" Width="150px" onchange="SetShopLevel(this.options[this.selectedIndex].value);" />
                    &nbsp;
                    <asp:TextBox ID="txtShopLevel" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trShopNo" runat="server">
                <td>
                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:dictionary,Shop No %>" />
                </td>
                <td>
                    <asp:TextBox ID="txtShopNo" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trShopArea" runat="server">
                <td>
                    <asp:Label ID="Label2" runat="server" Text="<%$Resources:dictionary, Shop Area (sqft)%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtShopArea" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trAttribute1" runat="server">
                <td>
                    <asp:Label ID="lblAttribute1" runat="server" Text="<%$Resources:dictionary, Attribute1%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtAttribute1" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trAttribute2" runat="server">
                <td>
                    <asp:Label ID="lblAttribute2" runat="server" Text="<%$Resources:dictionary, Attribute2%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtAttribute2" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Created On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, Created By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Modified On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Modified By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Deleted%>" />
                </td>
                <td>
                    <asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:Button ID="btnSave" CssClass="classname" runat="server" Text="<%$Resources:dictionary, Save%>" OnClick="btnSave_Click">
                    </asp:Button>&nbsp;
                    <input id="btnReturn" runat="server" type="button" onclick="location.href='MallLayoutSetup.aspx'" class="classname"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" CssClass="classname" runat="server" CausesValidation="False"
                        Text="<%$Resources:dictionary,Delete %>" OnClick="btnDelete_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
