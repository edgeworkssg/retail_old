<%@ Page Language="C#" Title="<%$Resources:dictionary,User Setup %>" Theme="Default"
    Inherits="UserMstscaffold" MasterPageFile="~/PowerPOSMst.master" CodeBehind="UserMstscaffold.aspx.cs" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        var newwindow;
        function poptastic(url) {
            newwindow = window.open(url, 'name', 'height=600,width=850,resizeable=1,scrollbars=1');
            if (window.focus) { newwindow.focus() }
        }                          
    </script>
    <asp:Panel ID="pnlGrid" runat="server" Width="100%">
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnNew" CssClass="classname" Text="<%$ Resources:dictionary, Add %>"
                        runat="server" OnClick="btnNew_Click"></asp:Button>
                </td>
                <td>
                    <asp:HyperLink ID="btnPrintAll" CssClass="classname" Text="Print All"
                        runat="server" NavigateUrl="javascript:poptastic('PrintUserBarcode.aspx');"></asp:HyperLink>
                </td>
                <td>
                    <asp:Button ID="btnGenerateAllBarcode" CssClass="classname" Text="Generate All Barcode"
                        runat="server" onclick="btnGenerateAllBarcode_Click"></asp:Button>
                </td>                                
                <td>
                    <asp:LinkButton ID="lnkExport" runat="server" CssClass="classBlue" OnClick="lnkExport_Click"
                        Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="UserName" PageSize="50"
            SkinID="scaffold" OnRowDataBound="GridView1_RowDataBound">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="hlEdit" runat="server" Text="<%$Resources:dictionary,Edit %>"
                            NavigateUrl='<%# string.Format("UserMstscaffold.aspx?id={0}", Server.UrlEncode(Eval("UserName", ""))) %>'></asp:HyperLink>
                        <asp:HyperLink ID="lnkPrint" runat="server" Text="Print" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="UserName" HeaderText="<%$Resources:dictionary,ID %>" SortExpression="UserName">
                </asp:BoundField>
                <asp:BoundField DataField="DisplayName" HeaderText="<%$Resources:dictionary,UserName %>"
                    SortExpression="DisplayName"></asp:BoundField>
                <asp:BoundField DataField="Password" Visible="false" HeaderText="<%$Resources:dictionary,Password %>"
                    SortExpression="Password"></asp:BoundField>
                <asp:BoundField DataField="Remark" HeaderText="<%$Resources:dictionary,Remark %>"
                    SortExpression="Remark"></asp:BoundField>
                <asp:BoundField DataField="GroupName" HeaderText="<%$Resources:dictionary,Group Name %>"
                    SortExpression="GroupName"></asp:BoundField>
                <asp:BoundField DataField="SalesPersonGroupName" HeaderText="<%$Resources:dictionary,Sales Person Group %>"
                    SortExpression="SalesPersonGroupName"></asp:BoundField>
                <asp:BoundField DataField="IsASalesPerson" HeaderText="<%$Resources:dictionary,Is Sales Person %>"
                    SortExpression="IsASalesPerson"></asp:BoundField>
                <asp:BoundField DataField="Outlet" HeaderText="<%$Resources:dictionary, Outlet%>" SortExpression="Outlet"></asp:BoundField>
                <asp:BoundField DataField="PointOfSale" HeaderText="<%$Resources:dictionary, Point of Sale%>" SortExpression="PointOfSale">
                </asp:BoundField>
                <asp:BoundField Visible="false" DataField="CreatedBy" HeaderText="<%$Resources:dictionary,Created By %>"
                    SortExpression="CreatedBy"></asp:BoundField>
                <asp:BoundField Visible="false" DataField="CreatedOn" HeaderText="<%$Resources:dictionary,Created On %>"
                    SortExpression="CreatedOn"></asp:BoundField>
                <asp:BoundField Visible="false" DataField="ModifiedBy" HeaderText="<%$Resources:dictionary,Modified By %>"
                    SortExpression="ModifiedBy"></asp:BoundField>
                <asp:BoundField Visible="false" DataField="ModifiedOn" HeaderText="<%$Resources:dictionary,Modified On %>"
                    SortExpression="ModifiedOn"></asp:BoundField>
                <asp:BoundField DataField="Barcode" HeaderText="Barcode" SortExpression="Barcode" Visible="false">
                </asp:BoundField>                    
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary, No UserMst  %>"></asp:Literal>
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                    <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                        ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> "
                        CommandArgument="Next" CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                        CommandArgument="Last" CommandName="Page" />
                </div>
            </PagerTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table id="FieldsTable" cellpadding="5" cellspacing="0" width="600px">
            <tr>
                <td colspan="2" align="left">
                    <asp:Button ID="btnAdd2" CssClass="classname" Text="<%$ Resources:dictionary, Add %>"
                        runat="server" OnClick="btnNew_Click"></asp:Button>
                    <asp:Button ID="btnSave" CssClass="classname" runat="server" Text="<%$ Resources:dictionary, Save %>"
                        OnClick="btnSave_Click"></asp:Button>&nbsp;
                    <input id="btnReturn" runat="server" type="button" onclick="location.href='UserMstscaffold.aspx'" class="classname"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" CssClass="classname" runat="server" CausesValidation="False"
                        Text="<%$ Resources:dictionary, Delete %>" OnClick="btnDelete_Click"></asp:Button>&nbsp;
                    <asp:Button ID="btnChangePwd" runat="server" CausesValidation="False" CssClass="classname"
                        Text="<%$ Resources:dictionary, Change Password %>" Width="129px" OnClick="btnChangePwd_Click"
                        Visible="False" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,User Name %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtID" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Display Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtDisplayName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltr1" runat="server" Text="<%$Resources:dictionary, Email%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="500"></asp:TextBox>
                </td>
            </tr>                    
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Password %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlPassword" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtCfmPwd"
                        ControlToValidate="ctrlPassword" ErrorMessage="<%$ Resources:dictionary, Password must match %>"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Re-Type Password %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtCfmPwd" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Barcode Token</td>
                <td>
                    <asp:Label ID="lblBarcodeToken" runat="server"></asp:Label>
                    <asp:Button ID="btnGenerateToken" runat="server" CausesValidation="False" 
                        CssClass="classname"  Text="Generate Token" Width="150px" 
                        onclick="btnGenerateToken_Click" />
                    <asp:Button ID="btnRemoveToken" runat="server" CausesValidation="False" 
                        CssClass="classname" onclick="btnRemoveToken_Click" Text="Remove" 
                        Width="150px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary,Remark %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlRemark" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Is Sales Person %>"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBox ID="cbIsSalesPerson" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:dictionary,Sales Person Group %>"></asp:Literal>
                </td>
                <td>
                    <subsonic:DropDown ID="ddGroupName" runat="server" OrderField="GroupName" PromptValue="0"
                        ShowPrompt="True" TableName="SalesGroup" TextField="GroupName" ValueField="SalesGroupID"
                        Width="175px">
                    </subsonic:DropDown>
                </td>
            </tr>
            <tr id="trOutlet" runat="server">
                <td>
                    <asp:Label ID="lblOutlet" runat="server" Text="Outlet" />
                </td>
                <td>
                    <asp:DropDownCheckBoxes ID="ddlMultiOutlet" runat="server" AddJQueryReference="True"
                        meta:resourcekey="checkBoxes1Resource1" UseButtons="False" UseSelectAllNode="True">
                        <Texts SelectBoxCaption="Select Outlet" />
                        <Style2 DropDownBoxBoxWidth="200" SelectBoxWidth="175" />
                    </asp:DropDownCheckBoxes>
                </td>
            </tr>
            <tr id="trPOS" runat="server">
                <td>
                    <asp:Label ID="lblPOS" runat="server" Text="<%$Resources:dictionary, Point of Sale%>" />
                </td>
                <td>
                    <asp:DropDownCheckBoxes ID="ddlMultiPOS" runat="server" AddJQueryReference="True"
                        meta:resourcekey="checkBoxes1Resource1" UseButtons="False" UseSelectAllNode="True">
                        <Texts SelectBoxCaption="Select POS" />
                        <Style2 DropDownBoxBoxWidth="200" SelectBoxWidth="175" />
                    </asp:DropDownCheckBoxes>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary,Group Name %>"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ctrlGroupName" runat="server" Width="175px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Dept Name %>"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ctrlDeptName" runat="server" Visible="False">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:dictionary,Basic Salary %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlBasicSalary" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:dictionary,Other Allowance %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="ctrlOtherAllowance" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <% if (isUseSupplierPortal)
               { %>
            <tr>
                <td>
                    <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:dictionary,Is Supplier %>"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBox ID="chkIsSupplier" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal15" runat="server" Text="<%$Resources:dictionary,Restricted Supplier List %>"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBox ID="chkIsRestrictedSupplierList" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="4" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal16" runat="server" Text="<%$Resources:dictionary,Supplier %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal17" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtSearchSupplier" runat="server"></asp:TextBox><asp:Button ID="btnSearchSupplier"
                        runat="server" Text="Search" class="classname" CausesValidation="False" OnClick="btnSearchSupplier_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal18" runat="server" Text="<%$Resources:dictionary,Supplier %>"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSupplier" runat="server" Width="344px" DataValueField="SupplierID"
                        DataTextField="SupplierName" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="btnAddSupplier" runat="server" Text="<%$ Resources:dictionary, Add Supplier to List %>"
                        CausesValidation="False" class="classname" OnClick="btnAddSupplier_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView ID="gvSupplier" runat="server" AutoGenerateColumns="False" SkinID="scaffold"
                        Width="500px" OnRowDataBound="gvSupplier_RowDataBound" OnRowDeleting="gvSupplier_RowDeleting">
                        <Columns>
                            <asp:CommandField ShowDeleteButton="true" CausesValidation="false" HeaderText="<%$Resources:dictionary,Delete%>" />
                            <asp:BoundField DataField="SupplierID" ReadOnly="true" HeaderText="<%$Resources:dictionary,Supplier ID %>" Visible="false" />
                            <asp:BoundField DataField="SupplierName" ReadOnly="true" HeaderText="<%$Resources:dictionary,Supplier Name %>" />
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeletedGVDetails" runat="server" Text='<%# Eval("Deleted")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <% } %>
            <tr>
                <td>
                    <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary,Created By %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Created On %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Modified By %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,Modified On %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
