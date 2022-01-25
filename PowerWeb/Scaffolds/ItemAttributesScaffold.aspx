<%@ Page Theme="Default" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="ItemAttributesScaffold.aspx.cs" Inherits="PowerWeb.Scaffolds.ItemAttributesScaffold"
    Title="<%$Resources:dictionary,Item Attributes Setup %>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
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
                    <input id="btnAdd" runat="server" class="classname" onclick="location.href='ItemAttributesscaffold.aspx?id=0'"
                        type="button" value="<%$Resources:dictionary, Add New%>" />
                </td>
            </tr>
            <tr>
                <td style="height: 25px;">
                    <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
            </tr>
        </table>
        <div style="height: 7px;">
        </div>
        <asp:GridView ID="GridView1" SkinID="scaffold" runat="server" AllowPaging="True"
            AllowSorting="True" AutoGenerateColumns="False" OnDataBound="GridView1_DataBound"
            OnSorting="GridView1_Sorting" OnPageIndexChanging="GridView1_PageIndexChanging"
            DataKeyNames="ID" PageSize="20">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="ID"
                    DataNavigateUrlFormatString="ItemAttributesscaffold.aspx?id={0}" />
                <asp:BoundField DataField="Type" HeaderText="<%$Resources:dictionary, Type %>" SortExpression="Type">
                </asp:BoundField>
                <asp:BoundField DataField="Value" HeaderText="<%$Resources:dictionary, Value%>" SortExpression="Value">
                </asp:BoundField>
                <asp:BoundField DataField="CreatedOn" HeaderText="<%$Resources:dictionary, Created On%>"
                    SortExpression="CreatedOn"></asp:BoundField>
                <asp:BoundField DataField="ModifiedOn" HeaderText="<%$Resources:dictionary, Modified On%>"
                    SortExpression="ModifiedOn"></asp:BoundField>
                <asp:BoundField DataField="CreatedBy" HeaderText="<%$Resources:dictionary, Created By %>"
                    SortExpression="CreatedBy"></asp:BoundField>
                <asp:BoundField DataField="ModifiedBy" HeaderText="<%$Resources:dictionary, Modified By%>"
                    SortExpression="ModifiedBy"></asp:BoundField>
                <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary, Deleted%>"
                    SortExpression="Deleted"></asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, No Item Attributes%>" />
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
                    <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Item Attributes%>" />
                </td>
                <td>
                    <asp:Label ID="lblID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Type%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlType" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary, Value%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlValue" runat="server" MaxLength="50"></asp:TextBox>
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
                    <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary, Modified On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary, Created By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary, Modified By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary, Deleted %>" />
                </td>
                <td>
                    <asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:Button ID="btnSave" CssClass="classname" runat="server" Text="<%$Resources:dictionary, Save%>"
                        OnClick="btnSave_Click"></asp:Button>&nbsp;
                    <input id="btnReturn" runat="server" type="button" class="classname" onclick="location.href='ItemAttributesScaffold.aspx'"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" CssClass="classname" runat="server" CausesValidation="False"
                        Text="<%$Resources:dictionary,Delete %>" OnClick="btnDelete_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
