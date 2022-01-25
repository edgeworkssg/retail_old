<%@ Page Title="<%$Resources:dictionary,Alternate Item Barcode Setup%>" Language="C#"
    MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="AlternateBarcodeScaffold.aspx.cs"
    Inherits="PowerWeb.Scaffolds.AlternateBarcodeScaffold" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <ajax:ScriptManager ID="ScriptManager1" runat="server">
        </ajax:ScriptManager>
        <table style="width: 600px" id="FieldsTable">
            <tr>
                <td colspan="2" class="wl_pageheaderSub">
                    <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 147px">
                    <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
                </td>
                <td style="width: 63px">
                    <asp:TextBox ID="txtSearch" runat="server" Width="297px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table id="search_ExportTable" style="vertical-align: middle; width: 600px; height: 40px;"
            border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td style="height: 30px; width: 50%; background-color: #FFFFFF; left: 0; vertical-align: middle;">
                    <input id="addNew" runat="server" class="classname" onclick="location.href='AlternateBarcodeScaffold.aspx?id=0'"
                        type="button" value="<%$Resources:dictionary, Add New%>" /><div class="divider">
                        </div>
                    <asp:LinkButton ID="btnSearch" class="classname" runat="server" OnClick="btnSearch_Click">
                        <asp:Literal ID="Literal16" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                    </asp:LinkButton><div class="divider">
                    </div>
                    <asp:LinkButton ID="btnClear" class="classname" runat="server" OnClick="btnClearFilter_Click">
                        <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:dictionary, Clear%>" />
                    </asp:LinkButton>
                </td>
                <td align="right" style="height: 30px; width: 50%; background-color: #FFFFFF; padding-right: 0px;
                    vertical-align: middle; right: 0px;">
                    <asp:LinkButton ID="LinkButton3" class="classBlue" runat="server" OnClick="lnkExport_Click">
                        <asp:Literal ID="Literal20" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 25px;">
                    <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:GridView ID="GridView1" SkinID="scaffold" runat="server" AllowPaging="True"
            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ItemNo" PageSize="20"
            Width="60%" OnDataBound="GridView1_DataBound" OnPageIndexChanging="GridView1_PageIndexChanging">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="BarcodeID"
                    DataNavigateUrlFormatString="AlternateBarcodeScaffold.aspx?id={0}" />
                <asp:BoundField DataField="BarcodeID" HeaderText="<%$Resources:dictionary, ID%>">
                </asp:BoundField>
                <asp:BoundField DataField="Barcode" HeaderText="<%$Resources:dictionary, Barcode%>">
                </asp:BoundField>
                <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:dictionary, Item%>">
                </asp:BoundField>
                <asp:BoundField DataField="ItemNo" HeaderText="<%$Resources:dictionary, Item No%>">
                </asp:BoundField>
                <asp:BoundField DataField="OriginalBarcode" HeaderText="<%$Resources:dictionary, Original Barcode%>">
                </asp:BoundField>
                <asp:BoundField DataField="Deleted" HeaderText="<%$Resources:dictionary, Deleted%>">
                </asp:BoundField>
                <asp:BoundField DataField="ModifiedBy" HeaderText="<%$Resources:dictionary, Edited By%>">
                </asp:BoundField>
                <asp:BoundField DataField="LastEditDate" HeaderText="<%$Resources:dictionary, Last Edit Date%>">
                </asp:BoundField>
                <asp:BoundField DataField="CreationDate" HeaderText="<%$Resources:dictionary, Creation Date%>">
                </asp:BoundField>
            </Columns>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                    <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                        ID="lblPageCount" runat="server"></asp:Label>
                    <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> "
                        CommandArgument="Next" CommandName="Page" />
                    <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                        CommandArgument="Last" CommandName="Page" />
                </div>
            </PagerTemplate>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, No Data Found%>" />
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server">
        <asp:Label ID="lblResult" runat="server"></asp:Label>
        <table cellpadding="5" cellspacing="0" width="1000" id="FieldsTable">
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary, Barcode ID%>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblBarcodeID" runat="server" Width="172px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary, Barcode %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtBarcode" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Search %>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtSearchItem" runat="server"></asp:TextBox><asp:Button ID="btnSearchItem"
                        runat="server" Text="Search" class="classname" CausesValidation="False" OnClick="btnSearchItem_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,Item No  %>"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddsItem" runat="server" Width="344px" DataValueField="ItemNo"
                        DataTextField="ItemName" />
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary, Created On%>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblCreatedOn" runat="server" Width="150px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Created By%>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblCreatedBy" runat="server" Width="150px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:dictionary, Modified On%>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblModifiedOn" runat="server" Width="150px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:dictionary,Modified By %>"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lblModifiedBy" runat="server" Width="150px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 122px">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary, Deleted%>"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBox ID="cbDeleted" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" CssClass="classname" OnClick="btnSave_Click"
                        CausesValidation="true" Text="<%$ Resources:dictionary, Save %>" />
                    &nbsp;
                    <input id="btnReturn" runat="server" class="classname" onclick="location.href='AlternateBarcodeScaffold.aspx'"
                        type="button" value="<%$Resources:dictionary,Return %>" />
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="classname"
                        OnClick="btnDelete_Click" Text="<%$ Resources:dictionary, Delete %>" />
                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
