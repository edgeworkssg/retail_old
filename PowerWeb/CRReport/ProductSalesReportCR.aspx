<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="ProductSalesReportCR.aspx.cs" Inherits="PowerWeb.CRReport.ProductSalesReportCR" Title="Product Sales Report" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager id="ScriptManager1" runat="server" />
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Animated="False" Format="dd MMM yyyy" PopupButtonID="ImageButton1" TargetControlID="txtStartDate" />
    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Animated="False" Format="dd MMM yyyy" PopupButtonID="ImageButton2" TargetControlID="txtEndDate" />
  <div style="height:20px;width:1122px" class="wl_pageheaderSub"> <asp:Literal ID = "Literal8" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal> </div>
    
    <table width="1122px">
       <%-- <tr><td colspan="4" class="wl_pageheaderSub" style="width: 100%"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
       --%>
        <tr>
            <td  style="width: 153px; height: 3px">
                <asp:RadioButton ID="rdbRange" runat="server" Checked="True" GroupName="DateSearch"
                    Text="<%$ Resources:dictionary, Start Date %>" /></td>
            <td style="width: 408px; height: 3px">
                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /></td>
            <td  style="width: 153px; height: 3px">
                <asp:Literal ID = "Literal2" runat="server" Text="<%$Resources:dictionary,End Date %>"></asp:Literal></td>
            <td style="width: 408px; height: 3px">
                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/App_Themes/Default/image/Calendar_scheduleHS.png" /></td>
        </tr>      
        <tr>
            <td  style="width: 102px">
                <asp:RadioButton ID="rdbMonth" runat="server" GroupName="DateSearch" Text="<%$ Resources:dictionary, Month %>"
                    Width="68px" /></td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" Width="122px">
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
                </asp:DropDownList>
                <asp:Label ID="lblYear" runat="server"></asp:Label></td>
            <td >
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td  style="width: 102px">
                <asp:Literal ID = "Literal7" runat="server" Text="<%$Resources:dictionary,Department %>" /></td>
            <td>
                <subsonic:DropDown ID="ddDept" runat="server" PromptValue="" TableName="ItemDepartment"
                    TextField="DepartmentName" ValueField="ItemDepartmentID" Width="172px" 
                    OnInit="ddDept_Init" ShowPrompt="True" PromptText="ALL">
                </subsonic:DropDown></td>
            <td >
            <asp:Literal ID = "Literal4" runat="server" Text="<%$Resources:dictionary,Category %>"></asp:Literal></td><td>
                <asp:DropDownList ID="ddCategory" runat="server" Width="179px">
                </asp:DropDownList></td></tr>
        <tr><td  style="width: 102px; height: 31px;"><asp:Literal ID = "Literal5" runat="server" Text="<%$Resources:dictionary,Point Of Sale %>"></asp:Literal></td><td style="height: 31px">
            <subsonic:DropDown ID="ddPOS" runat="server" ShowPrompt="True" TableName="PointOfSale"
                TextField="PointOfSaleName" ValueField="PointOfSaleID" Width="170px" PromptText="ALL" OnInit="ddPOS_Init">
            </subsonic:DropDown></td>
        <td  style="height: 31px"><asp:Literal ID = "Literal6" runat="server" Text="<%$Resources:dictionary,Outlet %>"></asp:Literal></td><td style="height: 31px">
            <asp:DropDownList ID="ddlOutlet" runat="server" Width="180px">
            </asp:DropDownList></td></tr>
        <tr>
            <td  style="width: 102px"> <asp:Literal ID = "Literal3" runat="server" Text="Search"></asp:Literal></td>
            <td><asp:TextBox ID="txtItemName" runat="server" Width="172px"></asp:TextBox></td>
            <td >
            </td>
            <td>
            </td>
        </tr>
        <tr><td colspan=2 >
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:dictionary, Clear %>" OnClick="btnClear_Click" /></td>
            <td colspan=2 align=right class="ExportButton">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" Text="<%$ Resources:dictionary, Export %>"></asp:LinkButton></td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "Literal1" runat="server" Text="Stock Balance Report"></asp:Literal></td></tr>
    </table>
    <div style="position:relative; z-index:0;">
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="True" DisplayGroupTree="False" Width="350px" Height="50px" />
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"></asp:ObjectDataSource>
    </div>
</asp:Content>
