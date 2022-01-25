<%@ Page Title="<%$Resources:dictionary,Guest Book Compulsory Setup %>" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="GuestbookCompulsoryScaffolds.aspx.cs" Inherits="PowerWeb.Scaffolds.GuestbookCompulsoryScaffolds"
    Theme="Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <asp:GridView ID="GridView1" SkinID="scaffold" runat="server" AllowPaging="True"
            AllowSorting="True" AutoGenerateColumns="False" OnDataBound="GridView1_DataBound"
            OnSorting="GridView1_Sorting" OnPageIndexChanging="GridView1_PageIndexChanging"
            DataKeyNames="GuestBookCompulsoryID" PageSize="20">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="GuestBookCompulsoryID" DataNavigateUrlFormatString="GuestbookCompulsoryScaffolds.aspx?id={0}" />
                <asp:BoundField DataField="GuestBookCompulsoryID" HeaderText="<%$Resources:dictionary, ID%>" SortExpression="GuestBookCompulsoryID">
                </asp:BoundField>
                <asp:BoundField DataField="FieldName" HeaderText="<%$Resources:dictionary, Field Name%>" SortExpression="FieldName">
                </asp:BoundField>
                <asp:BoundField DataField="IsCompulsory" HeaderText="<%$Resources:dictionary, Compulsory%>" SortExpression="IsCompulsory">
                </asp:BoundField>
                <asp:BoundField DataField="IsVisible" HeaderText="<%$Resources:dictionary, Visible%>" SortExpression="IsVisible">
                </asp:BoundField>
                <asp:BoundField DataField="Label" HeaderText="<%$Resources:dictionary, Label%>" SortExpression="Label">
                </asp:BoundField>
                <asp:BoundField DataField="Remark" HeaderText="<%$Resources:dictionary, Remark%>" SortExpression="Remark"></asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltrx0"  runat="server" Text="<%$Resources:dictionary, No Guest Book Compulsory%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, Page%>" />
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ltr01"  runat="server" Text="<%$Resources:dictionary, of%>" />
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
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, ID %>" />
                </td>
                <td>
                    <asp:Label ID="lblID" runat="server" MaxLength="50"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Field Name%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlFieldName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, Compulsory%>" />
                </td>
                <td>
                    <asp:CheckBox ID="ctrlIsCompulsory" runat="server" Checked="False"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Visible%>" />
                </td>
                <td>
                    <asp:CheckBox ID="ctrlIsVisible" runat="server" Checked="False"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Label%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlLabel" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Remark%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlRemark" runat="server" TextMode="MultiLine" Height="100px" Width="500px"
                        MaxLength="2147483647"></asp:TextBox>
                </td>
            </tr>
            <tr style="display: none">
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
                    <input id="btnReturn" runat="server" type="button" class="classname" onclick="location.href='GuestbookCompulsoryscaffolds.aspx'"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" CssClass="classname" runat="server" CausesValidation="False"
                        Text="<%$Resources:dictionary, Delete %>" OnClick="btnDelete_Click" Visible="false"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
