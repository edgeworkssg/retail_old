<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="SpecialActivityReport" Title="<%$Resources:dictionary,Special Activity Report %>" Codebehind="SpecialActivityReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="600px" id="FieldsTable">
        <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
        <tr>
            <td >
                <asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal></td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseStartDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Use Start Date %>" /></td>
            <td >
                <asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal></td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseEndDate" runat="server" Checked="True" Text="<%$ Resources:dictionary, Use End Date %>" /></td>
        </tr>
        <tr><td colspan=2 style="height: 15px" >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td colspan=2 align="right" class="ExportButton">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>
    </table>    
    <asp:GridView ID="gvReport"
            Width="100%" 
            runat="server" 
            AllowPaging="True" 
            AllowSorting="True"
            PageSize=20
            OnDataBound="gvReport_DataBound" 
            OnSorting="gvReport_Sorting"
	        OnPageIndexChanging="gvReport_PageIndexChanging" 
            AutoGenerateColumns="False"             
            SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:BoundField />
            <asp:BoundField DataField="ActivityTime" SortExpression="ActivityTime" HeaderText="<%$ Resources:dictionary,Date %>" />
            <asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="<%$ Resources:dictionary,ItemNo %>" />
            <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="<%$ Resources:dictionary,Quantity %>" />
            <asp:BoundField DataField="DiscountGiven" HeaderText="<%$ Resources:dictionary,Discount %>" SortExpression="DiscountGiven" />
            <asp:BoundField DataField="UnitPriceGiven" HeaderText="<%$ Resources:dictionary,Unit Price %>" SortExpression="UnitPriceGiven" />
            <asp:BoundField DataField="CurrentRetailPrice" HeaderText="<%$ Resources:dictionary,Current R.Price %>" SortExpression="CurrentRetailPrice" />
            <asp:BoundField DataField="Cashier" SortExpression="Cashier" HeaderText="<%$ Resources:dictionary,Cashier %>" />
            <asp:BoundField DataField="Outlet" SortExpression="Outlet" HeaderText="<%$ Resources:dictionary,Outlet %>"   />
            <asp:BoundField DataField="RefNo" SortExpression="RefNo" HeaderText="<%$ Resources:dictionary,RefNo %>" />
            <asp:BoundField DataField="LineRemark" HeaderText="<%$ Resources:dictionary,Line Remark %>" SortExpression="LineRemark" />
            <asp:BoundField DataField="HeaderRemark" HeaderText="<%$ Resources:dictionary,Hdr Remark %>" SortExpression="HeaderRemark" />
            <asp:BoundField DataField="IsPromo" HeaderText="Promo?" SortExpression="IsPromo" />
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

