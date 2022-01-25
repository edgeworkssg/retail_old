<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="InventoryTransferReport" Title="<%$Resources:dictionary,Inventory Transfer Report %>" Codebehind="InventoryTransferReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
    <table width="600px" id="FilterTable">
        <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
        <tr>
            <td >
                <asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary,Start Date %>"></asp:Literal></td>
            <td>
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseStartDate" runat="server" Checked="True" Text="Use Start Date" /></td>
            <td >
                <asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal></td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /><br />
                &nbsp;<asp:CheckBox ID="cbUseEndDate" runat="server" Checked="True" Text="Use End Date" /></td>
        </tr>       
        <tr><td > <asp:Literal ID = "Literal3" runat="server" 
                Text="<%$ Resources:dictionary,Search %>"></asp:Literal></td><td>
                <asp:TextBox ID="txtSearch" runat="server" Width="172px"></asp:TextBox></td>
        <td >
                <asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,Transfer By %>"></asp:Literal>
            </td><td>
                <asp:TextBox ID="txtTransferBy" runat="server" Width="173px"></asp:TextBox></td></tr>
        <tr><td >
            <asp:Literal ID = "Literal6" runat="server" Text="<%$Resources:dictionary,From Location %>"></asp:Literal></td><td>
            <asp:DropDownList ID="ddFromLocation" runat="server" CausesValidation="True"
                 Width="182px">
            </asp:DropDownList></td>
        <td >
            <asp:Literal ID = "Literal7" runat="server" Text="<%$Resources:dictionary,To Location %>"></asp:Literal></td><td><asp:DropDownList ID="ddToLocation" runat="server" CausesValidation="True"
                 Width="182px">
        </asp:DropDownList></td></tr>
        <tr><td >
                <asp:Literal ID = "Literal8" runat="server" 
                Text="<%$Resources:dictionary,Department %>"></asp:Literal></td><td>
                <subsonic:DropDown ID="ddDept" runat="server" PromptValue="" TableName="ItemDepartment"
                    TextField="DepartmentName" ValueField="ItemDepartmentID" Width="172px" 
                    OnInit="ddDept_Init" ShowPrompt="True" PromptText="ALL">
                </subsonic:DropDown></td>
        <td >
            <asp:Literal ID = "Literal9" runat="server" 
                Text="<%$Resources:dictionary,Category %>"></asp:Literal></td><td>
                <asp:DropDownList ID="ddCategory" runat="server" Width="179px">
                </asp:DropDownList></td></tr>
       <%-- <tr><td colspan=2 style="height: 15px" >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
        <td colspan=2 align="right" valign="bottom" class="ExportButton">
            <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>--%>
    </table>  
               <table id="search_ExportTable" style="vertical-align:middle;width:600px;height:40px;" border="0" cellpadding="2" cellspacing="0">
            <tr>                
                <td style="height:30px;width:50%; background-color:#FFFFFF; left:0;vertical-align :middle ;" >
                    <asp:LinkButton  ID="LinkButton1"  class="classname"  runat="server"  OnClick="btnSearch_Click">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:dictionary, Search%>" /> </asp:LinkButton><div class="divider">   </div>
                    <asp:LinkButton  ID="LinkButton4" class="classname" runat="server" OnClick="btnClear_Click" ><asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:dictionary, Clear%>" /></asp:LinkButton></td><td align="right" style="height:30px;width:50%; background-color:#FFFFFF;padding-right:0px;vertical-align:middle;right:0px;">
                     <asp:LinkButton  ID="LinkButton3" class="classBlue"  runat="server" OnClick="lnkExport_Click" >  
                     <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:dictionary, Export%>" /></asp:LinkButton></td></tr><%--                     <tr>
                <td colspan="4" style="height:25px;"> 
                <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label></td></tr>--%> </table>
                <asp:GridView ID="gvReport"
            Width="100%" 
            runat="server" 
            AllowPaging="True" 
            PageSize=20
            AllowSorting="True"
            OnDataBound="gvReport_DataBound" 
            OnSorting="gvReport_Sorting"
	        OnPageIndexChanging="gvReport_PageIndexChanging" 
            AutoGenerateColumns="False"             
            SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound" 
        ShowFooter="True">
        <Columns>
            <asp:BoundField DataField="InventoryDate" SortExpression="InventoryDate" HeaderText="<%$Resources:dictionary,Date %>" />
            <asp:BoundField DataField="FromInventoryLocationName" SortExpression="FromInventoryLocationName" HeaderText="<%$Resources:dictionary,From %>" />
            <asp:BoundField DataField="ToInventoryLocationName" SortExpression="ToInventoryLocationName" HeaderText="<%$Resources:dictionary,To %>" />
            <asp:BoundField DataField="Department" SortExpression="Department" HeaderText="<%$Resources:dictionary,Department %>" />
            <asp:BoundField DataField="Category" SortExpression="Category" HeaderText="<%$Resources:dictionary,Category %>"/>
            <asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="<%$Resources:dictionary,Item No %>" />
            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="<%$Resources:dictionary,Item %>" />
            <asp:BoundField DataField="TransferQty" HeaderText="<%$Resources:dictionary,Transfer Qty %>" SortExpression="TransferQty" />            
            <asp:BoundField DataField="CostOfGoods" HeaderText="<%$Resources:dictionary,Cost of Goods %>" SortExpression="CostOfGoods" />            
            <asp:BoundField DataField="LineTotal" SortExpression="LineTotal" HeaderText="<%$Resources:dictionary,Total Cost %>"/>
            <asp:BoundField DataField="TransferFromBy" HeaderText="<%$Resources:dictionary,Transfer By %>" SortExpression="TransferFromBy" />            
        </Columns>
        <PagerTemplate>
                        <div style="border-top:1px solid #666666">            
           <asp:Button ID="btnFirst" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,<< First %>" CommandArgument="First" CommandName="Page"/>
            <asp:Button ID="btnPrev" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,< Previous%>" CommandArgument="Prev" CommandName="Page"/>
                <asp:Literal ID = "pagelbl" runat="server" Text="<%$Resources:dictionary,page %>"></asp:Literal><asp:DropDownList ID="ddlPages" runat="server" Css AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList> <asp:Literal ID = "ofLbl" runat="server" Text="<%$Resources:dictionary,of %>"></asp:Literal><asp:Label ID="lblPageCount" runat="server"></asp:Label><asp:Button ID="btnNext" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Next > %> " CommandArgument="Next" CommandName="Page"/>
            <asp:Button ID="btnLast" runat="server" CssClass="scaffoldButton" Text="<%$Resources:dictionary,Last >> %> " CommandArgument="Last" CommandName="Page"/>
            </div>
        </PagerTemplate>
        
    </asp:GridView>    
</asp:Content>

