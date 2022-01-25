<%@ Page Language="C#" EnableEventValidation="true" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" Inherits="InventorySummaryReport" Title="<%$Resources:dictionary,Inventory Summary Report %>"
    CodeBehind="InventorySummaryReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="800px">
        <tr>
            <td>
                <asp:Literal ID="ltr1" runat="server" Text="<%$Resources:dictionary, Department%>" />
            </td>
            <td>
                <asp:DropDownList ID="ddlDept" runat="server" Width="175px" DataValueField="ItemDepartmentID"
                    DataTextField="DepartmentName" OnInit="ddlDept_Init">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="ltr2" runat="server" Text="<%$Resources:dictionary, Category%>" />
            </td>
            <td>
                <asp:DropDownList ID="ddlCategory" runat="server" Width="175px" DataValueField="CategoryName"
                    DataTextField="CategoryName" OnInit="ddlCategory_Init">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltr3" runat="server" Text="<%$Resources:dictionary, Supplier%>" />
            </td>
            <td>
                <asp:DropDownList ID="ddlSupplier" runat="server" Width="175px" DataValueField="SupplierID"
                    DataTextField="SupplierName" OnInit="ddlSupplier_Init">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="ltr00" runat="server" Text="<%$Resources:dictionary, Search%>" />
            </td>
            <td>
                <asp:TextBox ID="txtSearch" runat="server" Width="175px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <asp:LinkButton ID="LinkButton1" class="classname" runat="server" OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:dictionary, Search%>" />
                </asp:LinkButton>
            </td>
            <td align="right" colspan="2">
                <asp:LinkButton ID="lnkExport" runat="server" class="classBlue" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" Width="800px" runat="server" AllowPaging="True" AllowSorting="false"
        OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting" ShowFooter="true"
        OnPageIndexChanging="gvReport_PageIndexChanging" SkinID="scaffold" PageSize="20"
        OnRowDataBound="gvReport_RowDataBound">
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal><asp:DropDownList
                    ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Literal ID="ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label
                    ID="lblPageCount" runat="server"></asp:Label><asp:Button ID="btnNext" runat="server"
                        CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next"
                        CommandName="Page" />
                <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> "
                    CommandArgument="Last" CommandName="Page" />
            </div>
        </PagerTemplate>
    </asp:GridView>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headContent">
</asp:Content>
