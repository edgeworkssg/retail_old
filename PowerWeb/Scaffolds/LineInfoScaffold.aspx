<%@ Page Language="C#" Title="<%$Resources:dictionary, Line Info Setup%>" Inherits="LineInfoScaffold" MasterPageFile="~/PowerPOSMSt.master"
    Theme="default" CodeBehind="LineInfoScaffold.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<h2>Line Info Setup</h2>--%>
    <asp:Panel ID="pnlGrid" runat="server">
        <input id="btnAddNew" runat="server" class="classname" onclick="location.href='LineInfoScaffold.aspx?id=0'" type="button"
            value="<%$Resources:dictionary, Add New%>" />
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="LineInfoID" PageSize="50"
            SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="LineInfoID" DataNavigateUrlFormatString="LineInfoScaffold.aspx?id={0}"
                    Text="<%$Resources:dictionary, Edit%>" />
                <asp:BoundField DataField="LineInfoID" HeaderText="<%$Resources:dictionary, ID%>" SortExpression="LineInfoID" />
                <asp:BoundField DataField="LineInfoName" HeaderText="<%$Resources:dictionary, LineInfo Name%>" SortExpression="LineInfoName" />
            </Columns>
            <EmptyDataTemplate>
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Page %>" />
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, of %>" />
                    <asp:Label ID="lblPageCount0" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Next >%>" CommandArgument="Next"
                        CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Last >> %>"
                        CommandArgument="Last" CommandName="Page" />
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
            <tr>
                <td style="height: 25px; width: 186px">
                    <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, LineInfo ID%>" />
                </td>
                <td style="height: 25px">
                    <asp:Label ID="lblID" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 186px">
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, LineInfo Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlLineInfoName" runat="server" MaxLength="50" Width="225px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 186px">
                    <asp:Label ID="ctrlModifiedOn" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="ctrlModifiedBy" runat="server" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedOn" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="ctrlCreatedBy" runat="server" Visible="false"></asp:Label>
                    <asp:DropDownList ID="ctrlDepartmentID" runat="server" Visible="False">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        Text="<%$Resources:dictionary, Save%>" />
                    &nbsp;
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='LineInfoScaffold.aspx'" type="button"
                        value="<%$Resources:dictionary,Return %>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary,Delete %>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
