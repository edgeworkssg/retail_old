<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="SpecialDiscountSetup.aspx.cs" Inherits="PowerWeb.Scaffolds.SpecialDiscountSetup"
    Title="<%$Resources:dictionary, Discount Setup%>" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<h2>Category</h2>--%>
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <asp:Panel ID="pnlGrid" runat="server">
        <input id="btnAddNew" runat="server" class="classname" onclick="location.href='SpecialDiscountSetup.aspx?id=0'"
            type="button" value="<%$Resources:dictionary, Add New%>" />
        <div style="height: 7px;">
        </div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="DiscountName"
            PageSize="50" SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="DiscountName" DataNavigateUrlFormatString="SpecialDiscountSetup.aspx?id={0}" />
                <asp:BoundField DataField="DiscountName" HeaderText="<%$Resources:dictionary, Discount Name%>" SortExpression="DiscountName">
                </asp:BoundField>
                <asp:BoundField DataField="DiscountPercentage" HeaderText="<%$Resources:dictionary, Discount Percentage%>" SortExpression="DiscountPercentage">
                </asp:BoundField>
                <asp:BoundField DataField="StartDate" HeaderText="<%$Resources:dictionary, Start Date%>" SortExpression="StartDate">
                </asp:BoundField>
                <asp:BoundField DataField="EndDate" HeaderText="<%$Resources:dictionary, End Date%>" SortExpression="EndDate">
                </asp:BoundField>
                <asp:CheckBoxField DataField="ShowLabel" HeaderText="<%$Resources:dictionary, Show Label%>" SortExpression="ShowLabel">
                </asp:CheckBoxField>
                <asp:BoundField DataField="PriorityLevel" HeaderText="<%$Resources:dictionary, Priority Level%>" SortExpression="PriorityLevel">
                </asp:BoundField>
                <asp:BoundField DataField="DiscountLabel" HeaderText="<%$Resources:dictionary, Discount Label%>" SortExpression="DiscountLabel">
                </asp:BoundField>
                <asp:BoundField DataField="AssignedOutlet" HeaderText="<%$Resources:dictionary, Outlet%>" SortExpression="AssignedOutlet">
                </asp:BoundField>
                <asp:CheckBoxField DataField="Enabled" HeaderText="<%$Resources:dictionary, Enabled%>" SortExpression="Enabled">
                </asp:CheckBoxField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, No Discount%>" />
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
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
            <tr>
                <td>
                    <asp:Literal ID="Literal12"  runat="server" Text="<%$Resources:dictionary,Discount Name %>" />
                </td>
                <td>
                    <asp:TextBox ID="txtDiscName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal11"  runat="server" Text="<%$Resources:dictionary, Start Date%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtStartDate" runat="server" MaxLength="250"></asp:TextBox>
                    <asp:ImageButton ID="imgStartDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                    <cc1:CalendarExtender ID="ceStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
                        PopupButtonID="imgStartDate" TargetControlID="txtStartDate">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal10"  runat="server" Text="<%$Resources:dictionary, End Date%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtEndDate" runat="server" MaxLength="250"></asp:TextBox>
                    <asp:ImageButton ID="imgEndDate" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" />
                    <cc1:CalendarExtender ID="ceEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
                        PopupButtonID="imgEndDate" TargetControlID="txtEndDate">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary, Discount Percentage%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtDiscPercentage" runat="server" MaxLength="250"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtDiscPercentage"
                        ValidationExpression="((\d+)((\.\d{1,2})?))$" ErrorMessage="<%$ Resources:dictionary, Enter a number %>"
                        MaximumValue="100" MinimumValue="0" Type="Currency"></asp:RangeValidator>                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:dictionary, Show Label%>" />
                </td>
                <td>
                    <asp:CheckBox ID="chkShowLabel" runat="server" Checked="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Priority Level%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPriorityLevel" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary,Discount Label %>" />
                </td>
                <td>
                    <asp:TextBox ID="txtDiscountLabel" runat="server" MaxLength="250"></asp:TextBox>
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
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Enabled%>" />
                </td>
                <td>
                    <asp:CheckBox ID="chkEnabled" runat="server" Checked="true" />
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
                    <asp:CheckBox ID="ctrlDeleted" runat="server" Checked="False" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        Text="<%$Resources:dictionary, Save%>" />
                    &nbsp;
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='SpecialDiscountSetup.aspx'" type="button"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary, Delete%>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headContent">
    <style type="text/css">
        .style2
        {
            height: 32px;
        }
    </style>
</asp:Content>
