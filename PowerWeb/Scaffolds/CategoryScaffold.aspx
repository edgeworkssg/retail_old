<%@ Page Language="C#" Title="<%$Resources:dictionary, Category Setup%>" Inherits="CategoryScaffold"
    MasterPageFile="~/PowerPOSMSt.master" Theme="default" CodeBehind="CategoryScaffold.aspx.cs" %>

<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <asp:Panel ID="pnlGrid" runat="server">
        <input id="btnAddNew" runat="server" class="classname" onclick="location.href='CategoryScaffold.aspx?id=0'"
            type="button" value="<%$Resources:dictionary, Add New%>" />
        <div style="height: 7px;">
        </div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" OnSorting="GridView1_Sorting"
            OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="CategoryName"
            PageSize="50" SkinID="scaffold">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="CategoryName" DataNavigateUrlFormatString="CategoryScaffold.aspx?id={0}" />
                <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:dictionary, Category%>" SortExpression="CategoryName">
                </asp:BoundField>
                <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:dictionary, Department%>" SortExpression="DepartmentName">
                </asp:BoundField>
                <asp:BoundField DataField="Remarks" HeaderText="<%$Resources:dictionary, Remarks%>" SortExpression="Remarks">
                </asp:BoundField>
                <asp:BoundField DataField="IsForSale" HeaderText="<%$Resources:dictionary, Is For Sale%>" SortExpression="IsForSale">
                </asp:BoundField>
                <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary, Deleted%>" SortExpression="Deleted">
                </asp:BoundField>
                <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary, Deleted%>" SortExpression="Deleted">
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary,No Category %>" />
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
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Category %>" />
                </td>
                <td>
                    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hdfID" runat="server" Value="0" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Item Department%>" />
                </td>
                <td>
                    <subsonic:DropDown ID="ddlItemDept" runat="server" OrderField="DepartmentName" ShowPrompt="True"
                        TableName="ItemDepartment" TextField="DepartmentName" ValueField="ItemDepartmentID"
                        Width="305px">
                    </subsonic:DropDown>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, Remarks%>" />
                </td>
                <td>
                    <asp:TextBox ID="ctrlRemarks" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Is For Sale%>" />
                </td>
                <td>
                    <asp:CheckBox ID="cbIsForSale" runat="server" Checked="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Selling Restriction%>" />
                </td>
                <td>
                    <asp:CheckBox ID="cbIsSaleRestricted" Style="float: left; vertical-align: middle"
                        runat="server" Checked="true" />
                    <span style="float: left; vertical-align: middle"><asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Don't allow to sell from %>" /> &nbsp;&nbsp;
                    </span>
                    <cc2:TimeSelector ID="tsRestrictedStart" Style="float: left; vertical-align: middle"
                        MinuteIncrement="5" DisplaySeconds="false" runat="server">
                    </cc2:TimeSelector>
                    <span style="float: left; vertical-align: middle">&nbsp; &nbsp; <asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:dictionary, to %>" /> &nbsp;&nbsp;</span>
                    <cc2:TimeSelector ID="tsRestrictedEnd" Style="float: left; vertical-align: middle"
                        MinuteIncrement="5" DisplaySeconds="false" runat="server">
                    </cc2:TimeSelector>
                    &nbsp;
                </td>
            </tr>
            <tr runat="server" id="trFunding">
                <td>
                    <asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, Funding Cap %>" />
                </td>
                <td>
                    <div runat="server" id="divPAMed">
                        <span style="display: inline-block; width: 80px"><asp:Literal ID="Literal14"  runat="server" Text="<%$Resources:dictionary, PA Medifund %>" /></span>
                        <asp:TextBox ID="txtPAMedCap" runat="server" Width="75px" MaxLength="50"></asp:TextBox>
                    </div>
                    <div runat="server" id="divSMF">
                        <span style="display: inline-block; width: 80px"><asp:Literal ID="Literal15"  runat="server" Text="<%$Resources:dictionary, SMF%>" /></span>
                        <asp:TextBox ID="txtSMFCap" runat="server" Width="75px" MaxLength="50"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal16"  runat="server" Text="<%$Resources:dictionary, Age Restricted Item%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtAgeRestrictedItems" runat="server" Width="75px" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal17"  runat="server" Text="<%$Resources:dictionary, Override GST %>" />
                </td>
                <td>
                    <asp:CheckBox ID="chbOverrideGST" runat="server" Checked="False" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal18"  runat="server" Text="<%$Resources:dictionary, GST Percentage%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtGSTPercentage" runat="server" Width="75px" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal20"  runat="server" Text="<%$Resources:dictionary, Last Barcode Generated%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtLastBarcodeGenerated" runat="server" Width="75px" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal19"  runat="server" Text="<%$Resources:dictionary, Barcode Prefix%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtBarcodePrefix" runat="server" Width="75px" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary, Created By %>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal10"  runat="server" Text="<%$Resources:dictionary, Created On %>" />
                </td>
                <td>
                    <asp:Label ID="ctrlCreatedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal11"  runat="server" Text="<%$Resources:dictionary, Modified By %>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal12"  runat="server" Text="<%$Resources:dictionary, Modified On %>" />
                </td>
                <td>
                    <asp:Label ID="ctrlModifiedOn" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal13"  runat="server" Text="<%$Resources:dictionary, Deleted %>" />
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
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='CategoryScaffold.aspx'" type="button"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$Resources:dictionary,Delete %>" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <script type="text/javascript">
        $(document).ready(function() {

            setTimeout(function() {
                $("#<%= tsRestrictedStart.ClientID %>> table > tbody > tr").removeClass("wl_darkRaw").removeClass("wl_lightRaw");
                $("#<%= tsRestrictedEnd.ClientID %> > table > tbody > tr").removeClass("wl_darkRaw").removeClass("wl_lightRaw");
            }, 1000);
        });
    </script>

</asp:Content>
