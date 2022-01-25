<%@ Page Title="Voucher Setup" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="VoucherHeaderScaffold.aspx.cs" Inherits="VoucherHeaderScaffold" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <p><asp:Label ID="lblResult" runat="server"></asp:Label></p>
    <asp:Panel ID="pnlGrid" runat="server">
        <input id="btnAddNew" runat="server" class="classname" onclick="location.href='VoucherHeaderScaffold.aspx?id=0'"
            type="button" value="<%$Resources:dictionary, Add New%>" />
        <div style="height: 7px;">
        </div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="VoucherHeaderID"
            PageSize="50" SkinID="scaffold" onrowcommand="GridView1_RowCommand">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="VoucherHeaderID" DataNavigateUrlFormatString="VoucherHeaderScaffold.aspx?id={0}" />
                <asp:BoundField DataField="VoucherHeaderID" HeaderText="<%$Resources:dictionary, ID%>" SortExpression="VoucherHeaderID">
                </asp:BoundField>
                <asp:BoundField DataField="VoucherHeaderName" HeaderText="<%$Resources:dictionary, Voucher Name%>" SortExpression="VoucherHeaderName">
                </asp:BoundField>
                <asp:BoundField DataField="Amount" HeaderText="<%$Resources:dictionary, Amount%>" SortExpression="Amount" DataFormatString="{0:N2}">
                </asp:BoundField>
                <asp:BoundField DataField="ValidFrom" HeaderText="<%$Resources:dictionary, Valid From%>" SortExpression="ValidFrom" DataFormatString="{0:dd MMM yyyy}">
                </asp:BoundField>
                <asp:BoundField DataField="ValidTo" HeaderText="<%$Resources:dictionary, Valid To%>" SortExpression="ValidTo" DataFormatString="{0:dd MMM yyyy}">
                </asp:BoundField>
                <asp:BoundField DataField="VoucherNoFrom" HeaderText="<%$Resources:dictionary, Voucher No From%>" SortExpression="VoucherNoFrom">
                </asp:BoundField>
                <asp:BoundField DataField="VoucherNoTo" HeaderText="<%$Resources:dictionary, Voucher No To%>" SortExpression="VoucherNoTo">
                </asp:BoundField>
                <asp:BoundField DataField="IssuedQuantity" HeaderText="<%$Resources:dictionary, Issued Quantity%>" SortExpression="IssuedQuantity">
                </asp:BoundField>
                <asp:BoundField DataField="SoldQuantity" HeaderText="<%$Resources:dictionary, Sold Quantity%>" SortExpression="SoldQuantity">
                </asp:BoundField>
                <asp:BoundField DataField="RedeemedQuantity" HeaderText="<%$Resources:dictionary, Redeemed Quantity%>" SortExpression="RedeemedQuantity">
                </asp:BoundField>
                <asp:BoundField DataField="CanceledQuantity" HeaderText="<%$Resources:dictionary, Canceled Quantity%>" SortExpression="CanceledQuantity">
                </asp:BoundField>
                <asp:BoundField DataField="RedeemedQuantityWithoutVoucherNo" HeaderText="<%$Resources:dictionary, Redeemed Quantity Without Voucher No%>" SortExpression="RedeemedQuantityWithoutVoucherNo">
                </asp:BoundField>
                <asp:BoundField DataField="Outlet" HeaderText="<%$Resources:dictionary, Outlet%>" SortExpression="Outlet">
                </asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkIssueAll" runat="server" Text="<%$Resources:dictionary, Issue All%>" CommandName="IssueAll" CommandArgument='<%# Eval("VoucherHeaderID") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkCancelAll" runat="server" Text="<%$Resources:dictionary, Cancel All%>" CommandName="CancelAll" CommandArgument='<%# Eval("VoucherHeaderID") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, No Voucher%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="Literal13"  runat="server" Text="<%$Resources:dictionary, Page%>" />
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
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
            <tr>
                <td>
                    <asp:Literal ID="Literal12"  runat="server" Text="<%$Resources:dictionary, ID %>" />
                </td>
                <td>
                    <asp:Label ID="lblVoucherHeaderID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal14"  runat="server" Text="<%$Resources:dictionary, Voucher Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtVoucherHeaderName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary, Amount%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtAmount"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="999999999999" MinimumValue="0" Type="Currency"></asp:RangeValidator>                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal11"  runat="server" Text="<%$Resources:dictionary, Valid From%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtValidFrom" runat="server" MaxLength="250"></asp:TextBox>
                    <asp:ImageButton ID="imgStartDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                    <cc1:CalendarExtender ID="ceStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
                        PopupButtonID="imgStartDate" TargetControlID="txtValidFrom">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal10"  runat="server" Text="<%$Resources:dictionary, Valid To%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtValidTo" runat="server" MaxLength="250"></asp:TextBox>
                    <asp:ImageButton ID="imgEndDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                    <cc1:CalendarExtender ID="ceEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
                        PopupButtonID="imgEndDate" TargetControlID="txtValidTo">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal17"  runat="server" Text="<%$Resources:dictionary, Voucher Prefix%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtVoucherPrefix" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal15"  runat="server" Text="<%$Resources:dictionary, Start Number%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtStartNumber" runat="server"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtStartNumber"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="999999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal16"  runat="server" Text="<%$Resources:dictionary, End Number%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtEndNumber" runat="server"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtEndNumber"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="999999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal18"  runat="server" Text="<%$Resources:dictionary, Voucher Suffix%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtVoucherSuffix" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal19"  runat="server" Text="<%$Resources:dictionary, Num Of Digit%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtNumOfDigit" runat="server"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtNumOfDigit"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="999999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>                    
                </td>
            </tr>
            <tr id="trOutlet" runat="server">
                <td>
                    <asp:Label ID="lblOutlet" runat="server" Text="<%$Resources:dictionary,Outlet %>" />
                </td>
                <td>
                    <asp:DropDownCheckBoxes ID="ddlMultiOutlet" runat="server" AddJQueryReference="True"
                        meta:resourcekey="checkBoxes1Resource1" UseButtons="False" UseSelectAllNode="True">
                        <Texts SelectBoxCaption="Select Outlet" />
                        <Style2 DropDownBoxBoxWidth="200" SelectBoxWidth="175" />
                    </asp:DropDownCheckBoxes>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Created By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary,Created On %>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Modified By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Modified On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Deleted%>" />
                </td>
                <td>
                    <asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        Text="<%$Resources:dictionary, Save%>" />
                    &nbsp;
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='VoucherHeaderScaffold.aspx'" type="button"
                        value="<%$Resources:dictionary, Return%>" causesvalidation="false" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary, Delete%>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
