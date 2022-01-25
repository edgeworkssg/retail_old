<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="ViewRedemptionLog" Title="<%$Resources:dictionary,Redemption History%>" Codebehind="ViewRedemptionLog.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"> 
    <script>
    var newwindow;
    function poptastic(url)
    {
	    newwindow=window.open(url,'name','height=700,width=650,resizeable=1,scrollbars=1');
	    if (window.focus) {newwindow.focus()}
    }
    </script>
    <ajax:ScriptManager id="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <cc1:CalendarExtender ID="cldStartDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="cldEndDate" runat="server" Animated="False" Format="dd MMM yyyy"
        PopupButtonID="ImageButton2" TargetControlID="txtEndDate">
    </cc1:CalendarExtender>
     <div style="height:20px;width:600px;" class="wl_pageheaderSub"> <asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal> </div>
    <table width="600px" id="FieldsTable">
       <%-- <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
        --%><tr>
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
        <tr><td ><asp:Literal ID = "Literal3" runat="server" Text="<%$Resources:dictionary,Membership No %>"></asp:Literal></td><td><asp:TextBox ID="txtMembershipNo" runat="server" Width="172px"></asp:TextBox></td>
        <td ><asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,Membership Group %>"></asp:Literal></td><td><subsonic:DropDown ID="ddGroupName" runat="server" OnInit="ddPOS_Init" PromptText="ALL"
                ShowPrompt="True" TableName="MembershipGroup" TextField="GroupName" ValueField="MembershipGroupID"
                Width="170px">
        </subsonic:DropDown></td></tr>
        <tr><td ><asp:Literal ID = "Literal5" runat="server" Text="<%$Resources:dictionary,Name To Appear %>"></asp:Literal></td><td>
            <asp:TextBox ID="txtNameToAppear" runat="server" Width="172px"></asp:TextBox></td>
        <td >
                <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:dictionary,NRIC %>"></asp:Literal></td><td>
                <asp:TextBox ID="txtNRIC" runat="server" Width="172px"></asp:TextBox></td></tr>
        <tr>
            <td >
                <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:dictionary,Last Name %>"></asp:Literal></td>
            <td>
                <asp:TextBox ID="txtLastName" runat="server" Width="172px"></asp:TextBox></td>
            <td >
                <asp:Literal ID = "Literal6" runat="server" Text="<%$Resources:dictionary,First Name %>"></asp:Literal>&nbsp;</td>
            <td>
                &nbsp;<asp:TextBox ID="txtFirstName" runat="server" Width="172px"></asp:TextBox></td>
        </tr>
        <tr>
            <td ><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:dictionary,Point of Sale %>"></asp:Literal>
            </td>
            <td>
            <asp:DropDownList ID="ddlOutlet" runat="server" Width="179px">
            </asp:DropDownList></td>
            <td ><asp:Literal ID="Literal9" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal>
            </td>
            <td><subsonic:DropDown ID="ddPOS" runat="server" OnInit="ddPOS_Init" PromptText="ALL"
                ShowPrompt="True" TableName="PointOfSale" TextField="PointOfSaleName" ValueField="PointOfSaleID"
                Width="170px">
            </subsonic:DropDown></td>
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
            PageSize=20
            AllowPaging="True" 
            AllowSorting="True"
            OnDataBound="gvReport_DataBound" 
            OnSorting="gvReport_Sorting"
	        OnPageIndexChanging="gvReport_PageIndexChanging" 
            AutoGenerateColumns="False"             
            SkinID="scaffold" OnRowDataBound="gvReport_RowDataBound">
        <Columns>
            <asp:BoundField DataField="RedemptionDate" HeaderText="Date" SortExpression="RedemptionDate" />
            <asp:BoundField Visible=False DataField="MembershipNo" HeaderText="MembershipNo" SortExpression="MembershipNo" />
            <asp:TemplateField HeaderText="MembershipNo">
                <ItemTemplate>
                    <a ID="HyperLink1" 
                     href="javascript:poptastic('../Membership/MembershipDetail.aspx?id=<%# Eval("MembershipNo")%>');">
                     <%# Eval("MembershipNo")%>
                     </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="GroupName" HeaderText="Group" SortExpression="GroupName" />
            <asp:BoundField Visible=False  DataField="OrderHdrID" HeaderText="Order No" SortExpression="OrderHdrID" />
            <asp:BoundField DataField="NametoAppear" HeaderText="Name" SortExpression="NameToAppear" />
            <asp:BoundField Visible=False DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
            <asp:BoundField Visible=False DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
            <asp:BoundField DataField="NRIC" HeaderText="NRIC" SortExpression="NRIC" />
            <asp:BoundField DataField="Points" HeaderText="Redeemed Points" SortExpression="Points" />
            <asp:BoundField DataField="NettAmount" HeaderText="Amount" SortExpression="NettAmount" />
            <asp:TemplateField HeaderText="Order No">
                <ItemTemplate>
                    <a ID="HyperLink1" 
                     href="javascript:poptastic('../Order/OrderDetail.aspx?id=<%# Eval("OrderHdrID")%>');">
                     <%# Eval("OrderHdrID")%>
                     </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PointOfSaleName" SortExpression="PointOfSaleName" HeaderText="Point Of Sale" />
            <asp:BoundField DataField="OutletName" SortExpression="OutletName" HeaderText="Outlet" />
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

