<%@ Page Language="C#" Title="<%$Resources:dictionary, Membership Group Setup%>"
    Inherits="MembershipGroupScaffold" MasterPageFile="~/PowerPOSMSt.master" Theme="default"
    CodeBehind="MembershipGroupScaffold.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <input id="btnAddnew" runat="server" class="classname" onclick="location.href='MembershipGroupScaffold.aspx?id=0'"
            type="button" value="<%$Resources:dictionary, Add New%>" />
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="MembershipGroupID"
            PageSize="50" SkinID="scaffold" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="MembershipGroupID"
                    DataNavigateUrlFormatString="MembershipGroupScaffold.aspx?id={0}" />
                <asp:BoundField DataField="MembershipGroupID" HeaderText="<%$Resources:dictionary, GroupID%>"
                    SortExpression="MembershipGroupID"></asp:BoundField>
                <asp:BoundField DataField="GroupName" HeaderText="<%$Resources:dictionary, Group Name%>"
                    SortExpression="GroupName"></asp:BoundField>
                <asp:BoundField DataField="Discount" HeaderText="<%$Resources:dictionary, Discount%>"
                    SortExpression="Discount"></asp:BoundField>
                <asp:BoundField DataField="Userfloat1" HeaderText="<%$Resources:dictionary, Points Percentage%>"
                    SortExpression="Userfloat1"></asp:BoundField>
                <asp:BoundField DataField="Userfloat2" HeaderText="<%$Resources:dictionary, Spending Limit%>"
                    SortExpression="Userfloat2" />
                <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary, Deleted%>"
                    SortExpression="Deleted"></asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, No Membership Group%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Page%>" />
                    <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary, of%>" />
                    <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, Next >%>"
                        CommandArgument="Next" CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >>%>"
                        CommandArgument="Last" CommandName="Page" />
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server" CssClass="LabelMessage"></asp:Label>
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
            <tr>
                <td>
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary, GroupID%>" />
                </td>
                <td>
                    <asp:Label ID="lblID" runat="server" Text="[ID]"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary, Group Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtGroupName" runat="server" Width="237px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary, Discount%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlDiscount" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary, Points Percentage%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlPointsPercentage" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Spending Limit %>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlSpendingLimit" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary,Price Tier%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlPriceTier" runat="server" MaxLength="250"></asp:DropDownList>
                </td>
            </tr>
            
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Created By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary, Created On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary, Modified By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary, Modified On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Deleted%>" />
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
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='MembershipGroupScaffold.aspx'"
                        type="button" value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary, Delete%>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
