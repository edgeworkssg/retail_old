<%@ Page Title="Shelf Tag Printing" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="ShelfTagPrinting.aspx.cs" Inherits="PowerWeb.Reports.ShelfTagPrinting" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <asp:Label ID="lblResult" runat="server"></asp:Label>
    <table width="500px">
        <tr>
            <td style="width: 105px;">
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:dictionary, Outlet%>" />
            </td>
            <td>
                <asp:DropDownList ID="ddlOutlet" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="500px">
        <tr>
            <td style="width: 105px;">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary, Template%>" />
            </td>
            <td>
                <asp:DropDownList ID="ddlTemplate" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td style="width: 105px;">
                <asp:Literal ID="Literal9" runat="server" Text="Start From" />
            </td>
            <td>
                <asp:TextBox ID="txtStartFrom" runat="server" Width="50px" Text="1"></asp:TextBox>
                <td colspan="2">
                </td>
        </tr>
    </table>
    <br />
    <table width="700px">
        <tr>
            <td colspan="2">
                Item Input
            </td>
        </tr>
        <tr>
            <td style="width: 105px;">
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Search%>" />
            </td>
            <td>
                <asp:TextBox ID="txtProduct" runat="server" Width="300px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton></td></tr><tr style="height: 15px;">
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:dictionary, Item Dropdown%>" />
            </td>
            <td>
                <ajax:UpdatePanel ID="UpdatePanel2" runat="server">
                    <Triggers>
                        <ajax:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlItemDropdown" runat="server">
                        </asp:DropDownList>
                    </ContentTemplate>
                </ajax:UpdatePanel>
            </td>
        </tr>
        <tr style="height: 15px;">
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary, Qty%>" />
            </td>
            <td>
                <asp:TextBox ID="txtQty" runat="server" Width="150px" Style="text-align: right;">1</asp:TextBox></td></tr></table><asp:LinkButton ID="btnAddToList" class="classname" runat="server" OnClick="btnAddToList_Click">
        <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:dictionary, Add%>" />
    </asp:LinkButton><br />
    <br />
    <br />
    <ajax:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <ajax:AsyncPostBackTrigger ControlID="btnAddToList" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:HiddenField ID="hfSelectedItemNo" runat="server" />
            <asp:GridView ID="gvCart" runat="server" Width="800px" AllowPaging="True" SkinID="scaffold"
                PageSize="5" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" EmptyDataText="Empty Cart"
                OnRowDeleting="gvCart_RowDeleting">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True">
                        <HeaderStyle Width="80px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:CommandField>
                    <asp:BoundField DataField="ItemNo" HeaderText="ItemNo" SortExpression="ItemNo" />
                    <asp:BoundField DataField="ItemName" HeaderText="ItemName" SortExpression="ItemName" />
                    <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetCarts"
                TypeName="PowerWeb.Reports.ShelfTagPrinting" OldValuesParameterFormatString="original_{0}">
            </asp:ObjectDataSource>
        </ContentTemplate>
    </ajax:UpdatePanel>
    <br />
    <asp:LinkButton ID="btnCreateTag" class="classname" runat="server" OnClick="btnCreateTag_Click"
        Style="margin-right: 5px;">
        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:dictionary, Preview%>" />
    </asp:LinkButton><asp:LinkButton ID="btnExportPDF" class="classname" runat="server"
        OnClick="btnExportPDF_Click">
        <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:dictionary, PDF%>" /></asp:LinkButton><br />
    <br />
    <CR:CrystalReportViewer ID="crViewer" runat="server" AutoDataBind="true" />
</asp:Content>
