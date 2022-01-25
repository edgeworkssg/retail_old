<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="CurrentMonthCommissionReport" Title="<%$Resources:dictionary,Current Month Commission Report %>" Codebehind="CurrentMonthCommissionReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <table width="600px" id="FieldsTable">
        <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
        <tr><td >
            <asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Sales Person %>"></asp:Literal></td><td>
            <asp:TextBox ID="txtSalesPersonName" runat="server" Width="172px"></asp:TextBox></td>
        <td >
            <asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary,Group Name %>"></asp:Literal></td><td>
            <asp:DropDownList ID="ddlGroupName" runat="server" Width="179px">
            </asp:DropDownList></td></tr>
        <tr><td colspan=2 >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td colspan=2 align="right" class="ExportButton">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
            </td>
        </tr>
    </table>    
    <asp:GridView ID="gvReport"
            Width="600px" 
            runat="server" 
            PageSize=20
            AllowPaging="True" 
            AllowSorting="True"
            OnDataBound="gvReport_DataBound" 
            OnSorting="gvReport_Sorting"
	        OnPageIndexChanging="gvReport_PageIndexChanging" 
            AutoGenerateColumns="False"             
            SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound"
             ShowFooter=true>
        <Columns>
            <asp:BoundField DataField="SalesPersonName" HeaderText="<%$Resources:dictionary,Sales Person %>" SortExpression="SalesPersonName" />
            <asp:BoundField DataField="GroupName" HeaderText="<%$Resources:dictionary,Group %>" SortExpression="GroupName" />
            <asp:BoundField DataField="SalesAmount" HeaderText="<%$Resources:dictionary,Total Sales %>" SortExpression="SalesAmount" />
            <asp:BoundField DataField="rate" HeaderText="<%$Resources:dictionary,Rate %>" SortExpression="rate" />
            <asp:BoundField DataField="CommissionAmount" HeaderText="<%$Resources:dictionary,Commission Amount %>" SortExpression="CommissionAmount" />
        </Columns>
         <PagerTemplate>
                        <div style="border-top:1px solid #666666">            
           <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>" CommandArgument="First" CommandName="Page"/>
            <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>" CommandArgument="Prev" CommandName="Page"/>
                <asp:Literal ID = "pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal>
                <asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList> <asp:Literal ID = "ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label ID="lblPageCount" runat="server"></asp:Label>
            <asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next" CommandName="Page"/>
            <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> " CommandArgument="Last" CommandName="Page"/>
            </div>
        </PagerTemplate>
    </asp:GridView>
</asp:Content>

