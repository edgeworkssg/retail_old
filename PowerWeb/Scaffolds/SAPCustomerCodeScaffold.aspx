<%@ Page Title="<%$Resources:dictionary, SAP Customer Code Setup%>" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="SAPCustomerCodeScaffold.aspx.cs" Inherits="PowerWeb.Scaffolds.SAPCustomerCodeScaffold" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <input id="btnAddNew" runat="server" class="classname" onclick="location.href='SAPCustomerCodeScaffold.aspx?id=0'" type="button"
            value="<%$Resources:dictionary, Add New%>" />
        <div style="height: 7px;">
        </div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="Id" PageSize="50"
            SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="SAPCustomerCodeScaffold.aspx?id={0}" />
                <asp:BoundField DataField="SalesType" HeaderText="<%$Resources:dictionary, Order Type%>"></asp:BoundField>
                <asp:BoundField DataField="PaymentType" HeaderText="<%$Resources:dictionary, Payment Type%>"></asp:BoundField>
                <asp:BoundField DataField="PointOfSaleName" HeaderText="<%$Resources:dictionary,Sales Channel %>"></asp:BoundField>
                <asp:BoundField DataField="CustomerCode" HeaderText="<%$Resources:dictionary,Customer Code %>"></asp:BoundField>
                <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary,Deleted %>"></asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, No Data%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Page%>" />
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, of%>" />
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
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, ID%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtID" runat="server" Enabled="false"></asp:TextBox>
                    <asp:HiddenField ID="hdfID" runat="server" Value="0" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Order Type%>" />
                </td>
                <td>
                    <%--<asp:TextBox ID="txtSalesType" runat="server" MaxLength="50"></asp:TextBox>--%>
                    <asp:DropDownList ID="ddlSalesType" runat="server">
                        <asp:ListItem Value="Normal sales" Text="Normal sales"></asp:ListItem>
                        <asp:ListItem Value="Refund with Product Return" Text="Refund with Product Return"></asp:ListItem>
                        <asp:ListItem Value="Refund without Product Return" Text="Refund without Product Return"></asp:ListItem>
                        <asp:ListItem Value="Sample" Text="Sample"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, Payment Type%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtPaymentType" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary,Sales Channel %>" />
                </td>
                <td>
                    <subsonic:DropDown ID="ddlPOS" runat="server" TableName="PointOfSale" ValueField="PointOfSaleID" TextField="PointOfSaleName" Width="179px"></subsonic:DropDown>                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal10"  runat="server" Text="<%$Resources:dictionary, Customer Code%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtCustomerCode" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Created By%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, Created On%>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                   <asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary,Modified By %>" /> 
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                   <asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Modified On%>" /> 
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:dictionary, Deleted%>" />
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
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='SAPCustomerCodeScaffold.aspx'" type="button"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary, Delete%>" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
