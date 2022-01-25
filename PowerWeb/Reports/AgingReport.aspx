<%@ Page Title="<%$Resources:dictionary,Aging Report %>" Language="C#" MasterPageFile="~/PowerPOSMst.master"
    AutoEventWireup="true" CodeBehind="AgingReport.aspx.cs" Inherits="PowerWeb.Reports.AgingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="800px" id="FilterTable">
        <tr>
            <td colspan="4" class="wl_pageheaderSub">
                <asp:Literal ID="SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="fieldname" style="width: 105px">
                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Membership%>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtMembership" runat="server" Width="141px"></asp:TextBox>
            </td>
            <td class="fieldname">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Invoice No %>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtInvoiceNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>"
                    OnClick="btnSearch_Click" CssClass="classname" />
                <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>"
                    OnClick="btnClear_Click" CssClass="classname" />
            </td>
            <td colspan="2" align="right">
                <asp:LinkButton ID="lnkExport" runat="server" CssClass="classBlue" OnClick="lnkExport_Click"
                    Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvReport" Width="800px" runat="server" ShowFooter="True" AllowPaging="True"
        AllowSorting="True" OnDataBound="gvReport_DataBound" OnSorting="gvReport_Sorting"
        OnPageIndexChanging="gvReport_PageIndexChanging" AutoGenerateColumns="False" SkinID="scaffold"
        OnRowDataBound="gvReport_RowDataBound" PageSize="20">
        <Columns>
            <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:dictionary,Customer Name %>" SortExpression="Customer Name" />                            
            <asp:BoundField DataField="InvoiceNo" HeaderText="<%$Resources:dictionary,Invoice No %>" SortExpression="InvoiceNo" />            
            <asp:BoundField DataField="TotalCurrent" HeaderText="<%$ Resources:dictionary,Total Current %>" SortExpression="TotalCurrent" />
            <asp:BoundField DataField="aDays" HeaderText="<%$Resources:dictionary,1 - 30 Days %>" SortExpression="aDays" />                
            <asp:BoundField DataField="bDays" HeaderText="<%$Resources:dictionary,31 - 60 Days%>" SortExpression="bDays" />         
            <asp:BoundField DataField="cDays" HeaderText="<%$Resources:dictionary,61 Days and Above %>" SortExpression="cDays" />
            <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DueDate" />
        </Columns>
        <PagerTemplate>
            <div style="border-top: 1px solid #666666">
                <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>"
                    CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>"
                    CommandArgument="Prev" CommandName="Page" />
                <asp:Literal ID="pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" CssClass="scaffoldEditItem" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
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
</asp:Content>
