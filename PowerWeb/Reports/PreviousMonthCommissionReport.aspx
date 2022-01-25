<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="PreviousMonthCommissionReport" Title="<%$Resources:dictionary,Past Months Commission Report %>" Codebehind="PreviousMonthCommissionReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <table width="600px" id="FieldsTable">
        <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
        <tr>
            <td >
                <asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Select Date %>"></asp:Literal></td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" Width="81px">
                    <asp:ListItem Value="1">January</asp:ListItem>
                    <asp:ListItem Value="2">February</asp:ListItem>
                    <asp:ListItem Value="3">March</asp:ListItem>
                    <asp:ListItem Value="4">April</asp:ListItem>
                    <asp:ListItem Value="5">May</asp:ListItem>
                    <asp:ListItem Value="6">June</asp:ListItem>
                    <asp:ListItem Value="7">July</asp:ListItem>
                    <asp:ListItem Value="8">August</asp:ListItem>
                    <asp:ListItem Value="9">September</asp:ListItem>
                    <asp:ListItem Value="10">October</asp:ListItem>
                    <asp:ListItem Value="11">November</asp:ListItem>
                    <asp:ListItem Value="12">December</asp:ListItem>
                </asp:DropDownList><asp:DropDownList ID="ddlYear" runat="server" Width="66px">
                </asp:DropDownList></td>
            <td >
            </td>
            <td>
            </td>
        </tr>
        <tr><td >
            <asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary,Sales Person %>"></asp:Literal></td><td>
            <asp:TextBox ID="txtSalesPersonName" runat="server" Width="143px"></asp:TextBox></td>
        <td >
            <asp:Literal ID = "Literal3" runat="server" Text="<%$Resources:dictionary,Group Name %>"></asp:Literal></td><td>
            <asp:DropDownList ID="ddlGroupName" runat="server" Width="148px">
            </asp:DropDownList></td></tr>
        <tr><td colspan=2 >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
         <td colspan=2 align=right class="ExportButton">
             <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton>
             </td>
        </tr>
    </table>    
    <asp:GridView ID="gvReport"
            Width="600px" 
            PageSize=20
            runat="server" 
            AllowPaging="True" 
            AllowSorting="True"
            OnDataBound="gvReport_DataBound" 
            OnSorting="gvReport_Sorting"
	        OnPageIndexChanging="gvReport_PageIndexChanging" 
            AutoGenerateColumns="False"             
            SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:BoundField DataField="SalesPersonName" HeaderText="<%$Resources:dictionary,Sales Person %>" SortExpression="SalesPersonName" />
            <asp:BoundField DataField="GroupName" HeaderText="<%$Resources:dictionary,Group %>" SortExpression="GroupName" />
            <asp:BoundField DataField="TotalSales" HeaderText="<%$Resources:dictionary,Total Sales %>" SortExpression="TotalSales" />
            <asp:BoundField DataField="commissionrate" HeaderText="<%$Resources:dictionary,Rate %>" SortExpression="commissionrate" />
            <asp:BoundField DataField="Commission" HeaderText="<%$Resources:dictionary,Commission Amount %>" SortExpression="Commission" />
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
    <table width="600">
    <tr>
    <td  style="width: 130px; height: 18px"><asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,Total Sales Amount %>"></asp:Literal></td><td style="width: 85px; height: 18px">
        <asp:Label ID="lblTotalSales" runat="server" Width="66px"></asp:Label></td>
    <td  style="width: 130px; height: 18px"><asp:Literal ID = "Literal5" runat="server" Text="<%$Resources:dictionary,Total Commission Amount %>"></asp:Literal></td><td style="width: 12px; height: 18px">
        <asp:Label ID="lblTotalComm" runat="server" Width="56px"></asp:Label></td>
    </tr></table>
</asp:Content>

