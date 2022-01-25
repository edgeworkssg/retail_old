<%@ Page Language="C#" EnableEventValidation="true"  MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="SalesChartByOutlet" Title="Sales Chart By Outlet" Codebehind="SalesChartByOutlet.aspx.cs" %>

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
    <table width="100%" border=0>
    <tr>
        <td>
            <iframe id="iframe1" runat=server width="400" height="200" />
        </td>
    </tr>
    <tr>
        <td><iframe id="iframe2" runat=server width="600" height="400" /></td>
    </tr>
    </table>
    
    
</asp:Content>

