<%@ Page Title="<%$Resources:dictionary,Resource Setup %>" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="ResourceScaffold.aspx.cs" Inherits="PowerWeb.Scaffolds.ResourceScaffold" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <ajax:ScriptManager ID="ScriptManager1" runat="server">
        </ajax:ScriptManager>
        <table style="width: 600px" id="FieldsTable">
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
                    <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                        <asp:Literal ID="Literal16" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                    </asp:LinkButton><div class="divider">
                    </div>
                    <asp:LinkButton ID="LinkButton4" class="classname" runat="server" OnClick="btnClearFilter_Click">
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
        <input id="btnAddNew" runat="server" class="classname" onclick="location.href='ResourceScaffold.aspx?id=0'" type="button"
            width="130px" value="<%$Resources:dictionary, Add New%>" /><div class="divider">
            </div>
        <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Select All%>" Width="130px" ID="BtnSelectAll"
            OnClick="BtnSelectAll_Click" /><div class="divider">
            </div>
        <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Clear Selection%>" ID="BtnClearSelection"
            OnClick="BtnClearSelection_Click" Width="130px" /><div class="divider">
            </div>
        <asp:Button class="classname" runat="server" Text="<%$Resources:dictionary, Delete Selected%>" ID="BtnDeleteSelection"
            OnClick="BtnDeleteSelection_Click" Width="130px" /><div class="divider">
            </div>
        &nbsp;
        <div style="height: 10px;">
        </div>
        <div style="height: 7px;">
        </div>
        <asp:GridView ID="GridView1" SkinID="scaffold" runat="server" AllowPaging="True"
            AllowSorting="True" AutoGenerateColumns="False" OnDataBound="GridView1_DataBound"
            OnSorting="GridView1_Sorting" OnPageIndexChanging="GridView1_PageIndexChanging"
            DataKeyNames="ResourceID" PageSize="20">
            <Columns>
                <asp:HyperLinkField Text="<%$Resources:dictionary, Edit%>" DataNavigateUrlFields="ResourceID" DataNavigateUrlFormatString="ResourceScaffold.aspx?id={0}" />
                <asp:TemplateField HeaderText="<%$Resources:dictionary, %>Delete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ResourceID" HeaderText="<%$Resources:dictionary, Resource ID%>" SortExpression="ResourceID">
                </asp:BoundField>
                <asp:BoundField DataField="ResourceName" HeaderText="<%$Resources:dictionary, Resource Name%>" SortExpression="ResourceName">
                </asp:BoundField>
                <asp:BoundField DataField="ResourceGroupName" HeaderText="<%$Resources:dictionary, Group Name%>" SortExpression="ResourceGroupName">
                </asp:BoundField>
                <asp:BoundField DataField="Capacity" HeaderText="<%$Resources:dictionary, Capacity%>" SortExpression="Capacity">
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, No Resource%>" />
            </EmptyDataTemplate>
            <PagerTemplate>
                <div style="border-top: 1px solid #666666">
                    <br />
                    <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, << First%>"
                        CommandArgument="First" CommandName="Page" />
                    <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary, < Previous%>"
                        CommandArgument="Prev" CommandName="Page" />
                    <asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Page%>" />
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
                    <asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, Resource ID%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtResourceID" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Resource Name%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtResourceName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Resource Group%>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlResourceGroup" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, Capacity%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtCapacity" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Room Charge%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtRoomCharge" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Minimum Spending%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtMinSpending" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Minimum Spending Charge%>" />
                </td>
                <td>
                    <asp:TextBox ID="txtMinSpendingCharge" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:Button ID="btnSave" CssClass="classname" runat="server" Text="<%$Resources:dictionary, Save%>" OnClick="btnSave_Click">
                    </asp:Button>&nbsp;
                    <asp:Button ID="btnSaveAndNew" runat="server" CssClass="classname" OnClick="btnSaveAndNew_Click"
                        Text="<%$ Resources:dictionary, Save and New%>" />
                    &nbsp;
                    <input id="btnReturn" runat="server" type="button" class="classname" onclick="location.href='ResourceScaffold.aspx'"
                        value="<%$Resources:dictionary, Return%>" />
                    <asp:Button ID="btnDelete" CssClass="classname" runat="server" CausesValidation="False"
                        Text="<%$Resources:dictionary, Delete%>" OnClick="btnDelete_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
