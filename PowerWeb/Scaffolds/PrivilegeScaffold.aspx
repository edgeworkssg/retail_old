<%@ Page Theme="Default" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    Inherits="PrivilegeScaffold" Title="<%$Resources:dictionary,Privilege Setup %>"
    CodeBehind="PrivilegeScaffold.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    <asp:LinkButton ID="LinkButton4" class="classname" runat="server" OnClick="btnClearFilter_Click">
                        <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:dictionary, Clear%>" />
                    </asp:LinkButton>
                    <input id="btnAddNew" runat="server" class="classname" onclick="location.href='PrivilegeScaffold.aspx?id=0'"
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
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="UserPrivilegeID"
            PageSize="50" SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="UserPrivilegeID"
                    DataNavigateUrlFormatString="PrivilegeScaffold.aspx?id={0}" />
                <asp:BoundField DataField="PrivilegeName" HeaderText="<%$Resources:dictionary, Privilege Name%>">
                </asp:BoundField>
                <asp:BoundField DataField="FormName" HeaderText="<%$Resources:dictionary, Form Name%>">
                </asp:BoundField>
                <asp:BoundField DataField="PrivilegeCategory" HeaderText="<%$Resources:dictionary, Privilege Category%>">
                </asp:BoundField>
                <asp:TemplateField HeaderText="<%$Resources:dictionary, Super Admin%>">
                    <ItemTemplate>
                        <asp:Label ID="lblSuperAdmin" Text='<%# Eval("IsSuperAdmin").ToString().ToUpper() == "TRUE" ? "YES" : "NO" %>'
                            runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:dictionary, Retail%>">
                    <ItemTemplate>
                        <asp:Label ID="Label1" Text='<%# Eval("IsRetail").ToString().ToUpper() == "TRUE" ? "YES" : "NO" %>'
                            runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:dictionary, Wholesale%>">
                    <ItemTemplate>
                        <asp:Label ID="Label2" Text='<%# Eval("IsWholesale").ToString().ToUpper() == "TRUE" ? "YES" : "NO" %>'
                            runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:dictionary, Beauty%>">
                    <ItemTemplate>
                        <asp:Label ID="Label3" Text='<%# Eval("IsBeauty").ToString().ToUpper() == "TRUE" ? "YES" : "NO" %>'
                            runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, No Privileges%>" />
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
                    <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, User Privilege ID%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtID" runat="server" Enabled="false"></asp:TextBox>
                    <asp:HiddenField ID="hdfID" runat="server" Value="0" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Privilege Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPrivilegesName" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary, Form Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtFormName" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Privilege Category%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPrivilegeCategory" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary, POS Type%>" />
                </td>
                <td>
                    <asp:CheckBox ID="cbIsRetail" runat="server" Checked="False" Text="<%$Resources:dictionary, Retail%>" />&nbsp;
                    <asp:CheckBox ID="cbIsWholesale" runat="server" Checked="False" Text="<%$Resources:dictionary, Wholesale%>" />&nbsp;
                    <asp:CheckBox ID="cbIsBeauty" runat="server" Checked="False" Text="<%$Resources:dictionary, Beauty%>" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary, Super Admin%>" />
                </td>
                <td>
                    <asp:CheckBox ID="cbIsSuperAdmin" runat="server" Checked="False" />
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
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='PrivilegeScaffold.aspx'"
                        type="button" value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary, Delete%>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
