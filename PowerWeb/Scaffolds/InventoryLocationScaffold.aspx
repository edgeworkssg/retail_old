<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" Theme="Default" AutoEventWireup="true" Inherits="InventoryLocationScaffold" Title="<%$Resources:dictionary,Inventory Location Setup %>" Codebehind="InventoryLocationScaffold.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <asp:Panel ID="pnlGrid" runat="server">
        <input id="btnAddNew" runat="server" class="classname" onclick="location.href='InventoryLocationScaffold.aspx?id=0'"
            type="button" value="<%$Resources:dictionary, Add New%>" />
        <div style="height: 7px;">
        </div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="InventoryLocationID" PageSize="50"
            SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="InventoryLocationID" DataNavigateUrlFormatString="InventoryLocationScaffold.aspx?id={0}" />
                <asp:BoundField DataField="InventoryLocationID" HeaderText="<%$Resources:dictionary, Inventory Location ID%>" />
                <asp:BoundField DataField="InventoryLocationName" HeaderText="<%$Resources:dictionary, Location Name%>" />
                <asp:CheckBoxField DataField="Deleted" HeaderText="<%$Resources:dictionary,Deleted %>" />
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, No Data%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Page%>" />
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary, of%>" />
                    <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Next >%>"
                        CommandArgument="Next" CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Last >>%>"
                        CommandArgument="Last" CommandName="Page" />
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
            <tr>
                <td>
                    <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Inventory Location ID%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtID" runat="server" Enabled="false"></asp:TextBox>
                    <asp:HiddenField ID="hdfID" runat="server" Value="0" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal16" runat="server" Text="Inventory Group" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlLocationGroup" runat="server" 
                        DataValueField="InventoryLocationGroupID" 
                        DataTextField="InventoryLocationGroupName" Width="130px" 
                        oninit="ddlLocationGroup_Init" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Location Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtLocationName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary, Displayed Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtDisplayedName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Address 1%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtAddress1" runat="server"></asp:TextBox>
                </td>
            </tr> 
            <tr>
                <td>
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary, Address 2%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtAddress2" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary, Address 3%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtAddress3" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:dictionary, City%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary, Country%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary, Postal Code%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPostalCode" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr runat="server" id="trPriceLevel">
                <td>
                    <asp:Literal ID="Literal15" runat="server" Text="<%$Resources:dictionary, Default Price Level%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlPriceLevel" runat="server" 
                        DataValueField="Key" 
                        DataTextField="Value" Width="130px" 
                        oninit="ddlPriceLevel_Init" />
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
                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary, Created On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Modified By %>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary, Modified On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary, Deleted%>" />
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
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='InventoryLocationScaffold.aspx'"
                        type="button" value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary, Delete%>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
