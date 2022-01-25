<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="OutletGroup.aspx.cs" Inherits="PowerWeb.Scaffolds.OutletGroup" Title="<%$Resources:dictionary, Outlet Group%>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <input id="btnAddNew" runat="server" class="classname" onclick="location.href='OutletGroup.aspx?id=0'" type="button"
            value="<%$Resources:dictionary, Add New%>" />
        <div style="height: 5px;">
        </div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="OutletGroupID"
            PageSize="50" SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="OutletGroupID" DataNavigateUrlFormatString="OutletGroup.aspx?id={0}" />
                <asp:BoundField DataField="OutletGroupID" HeaderText="<%$Resources:dictionary, Outlet Group ID %>" SortExpression="OutletGroupID">
                </asp:BoundField>
                <asp:BoundField DataField="OutletGroupName" HeaderText="<%$Resources:dictionary, Outlet Group Name%>" SortExpression="OutletGroupName">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="CreatedOn" HeaderText="<%$Resources:dictionary, Created On%>" SortExpression="CreatedOn">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="CreatedBy" HeaderText="<%$Resources:dictionary, Created By%>" SortExpression="CreatedBy">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="ModifiedOn" HeaderText="<%$Resources:dictionary, Modified On%>" SortExpression="ModifiedOn">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="ModifiedBy" HeaderText="<%$Resources:dictionary, Modified By%>" SortExpression="ModifiedBy">
                </asp:BoundField>
                <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary, Deleted%>" SortExpression="Deleted">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfld1" HeaderText="Userfld1" SortExpression="userfld1">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfld2" HeaderText="Userfld2" SortExpression="userfld2">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfld3" HeaderText="Userfld3" SortExpression="userfld3">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfld4" HeaderText="Userfld4" SortExpression="userfld4">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfld5" HeaderText="Userfld5" SortExpression="userfld5">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfld6" HeaderText="Userfld6" SortExpression="userfld6">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfld7" HeaderText="Userfld7" SortExpression="userfld7">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfld8" HeaderText="Userfld8" SortExpression="userfld8">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfld9" HeaderText="Userfld9" SortExpression="userfld9">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfld10" HeaderText="Userfld10" SortExpression="userfld10">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userflag1" HeaderText="Userflag1" SortExpression="userflag1">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userflag2" HeaderText="Userflag2" SortExpression="userflag2">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userflag3" HeaderText="Userflag3" SortExpression="userflag3">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userflag4" HeaderText="Userflag4" SortExpression="userflag4">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userflag5" HeaderText="Userflag5" SortExpression="userflag5">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfloat1" HeaderText="Userfloat1" SortExpression="userfloat1">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfloat2" HeaderText="Userfloat2" SortExpression="userfloat2">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfloat3" HeaderText="Userfloat3" SortExpression="userfloat3">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfloat4" HeaderText="Userfloat4" SortExpression="userfloat4">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userfloat5" HeaderText="Userfloat5" SortExpression="userfloat5">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userint1" HeaderText="Userint1" SortExpression="userint1">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userint2" HeaderText="Userint2" SortExpression="userint2">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userint3" HeaderText="Userint3" SortExpression="userint3">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userint4" HeaderText="Userint4" SortExpression="userint4">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="userint5" HeaderText="Userint5" SortExpression="userint5">
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, No Outlet%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Page%>" />
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, of%>" />
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
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
            <tr>
                <td>
                    <asp:Label ID="lblOutletGroupID" runat="server" Text="<%$Resources:dictionary, Outlet Group ID%>" />
                </td>
                <td>
                    &nbsp;<asp:TextBox ID="txtOutletGroupID" runat="server" Width="148px" Enabled="false"></asp:TextBox>
                    <asp:HiddenField ID="hdfID" runat="server" Value="0" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblOutletGroupName" runat="server" Text="<%$Resources:dictionary, Outlet Group Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlOutletGroupName" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Created On %>" />
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
                    <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary,Modified By %>" />
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
                    <input id="btnReturn" runat="server" type="button" onclick="location.href='OutletGroup.aspx'" class="classname"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" CssClass="classname" runat="server" CausesValidation="False"
                        Text="<%$Resources:dictionary, Delete%>" OnClick="btnDelete_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
