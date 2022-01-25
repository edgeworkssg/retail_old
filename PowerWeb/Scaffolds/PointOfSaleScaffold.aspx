<%@ Page Language="C#" Title="<%$Resources:dictionary, Point Of Sale Setup%>" Inherits="PointOfSaleScaffold"
    MasterPageFile="~/PowerPOSMSt.master" Theme="default" CodeBehind="PointOfSaleScaffold.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<h2>PointOfSale</h2>--%>
    <asp:Panel ID="pnlGrid" runat="server">
        <input id="btnAddNew" runat="server" class="classname" onclick="location.href='PointOfSaleScaffold.aspx?id=0'"
            type="button" value="<%$Resources:dictionary, Add New%>" />
        <div style="height: 5px;">
        </div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="PointOfSaleID"
            PageSize="50" SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="PointOfSaleID" DataNavigateUrlFormatString="PointOfSaleScaffold.aspx?id={0}" />
                <asp:BoundField DataField="PointOfSaleName" HeaderText="<%$Resources:dictionary, Point Of Sale Name%>" SortExpression="PointOfSaleName">
                </asp:BoundField>
                <asp:BoundField DataField="PointOfSaleDescription" HeaderText="<%$Resources:dictionary, Point Of Sale Description%>"
                    SortExpression="PointOfSaleDescription"></asp:BoundField>
                <asp:BoundField DataField="OutletName" HeaderText="<%$Resources:dictionary, Outlet Name%>" SortExpression="OutletName">
                </asp:BoundField>
                <asp:BoundField DataField="PhoneNo" HeaderText="<%$Resources:dictionary, Phone No%>" SortExpression="PhoneNo">
                </asp:BoundField>
                <asp:BoundField DataField="QuickAccessGroupID" HeaderText="<%$Resources:dictionary, Quick Access Group%>" SortExpression="QuickAccessGroupID"
                    Visible="false"></asp:BoundField>
                <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary, Deleted%>" SortExpression="Deleted">
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, No Point Of Sale%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary, Page%>" />
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
                    <asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:dictionary, Point Of Sale ID%>" />
                </td>
                <td>
                    <asp:Label ID="lblID" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Point Of Sale Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlPointOfSaleName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, Point Of Sale Description%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlPointOfSaleDescription" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Outlet Name%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ctrlOutletName" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="rowLinkToMember" runat="server">
                <td>
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Link To Member %>" />
                </td>
                <td>
                    <asp:DropDownList ID="ctrlMember" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, Quick Access Group%>" />
                </td>
                <td>
                    <subsonic:DropDown TableName="QuickAccessGroup" OrderField="QuickAccessGroupName"
                        runat="server" ID="ctrlQuickAccessGroup" ValueField="QuickAccessGroupID" TextField="QuickAccessGroupName"
                        Width="172px">
                    </subsonic:DropDown>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Phone No%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlPhoneNo" runat="server" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Membership Code%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtMembershipCode" runat="server" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Deleted%>" />
                </td>
                <td>
                    <asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;<asp:Label ID="Label1" runat="server" Text="<%$Resources:dictionary, Price Scheme ID%>" Visible="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPriceSchemeID" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
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
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='PointOfSaleScaffold.aspx'" type="button"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary, Delete%>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
