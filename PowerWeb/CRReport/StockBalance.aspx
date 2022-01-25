<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="StockBalance.aspx.cs" Inherits="PowerWeb.CRReport.StockBalance" Title="<%$Resources:dictionary, Stock Balance by Attributes%>" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div style="height:20px;width:100%;" class="wl_pageheaderSub"> <asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal> </div>
   
    <table style="width: 100%">
      <%--  <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "SEARCHLbl" runat="server" Text="<%$Resources:dictionary,SEARCH %>"></asp:Literal></td></tr>
       --%> <tr>
            <td  style="width: 102px; height: 3px">
                <asp:Literal ID="Literal3" Text="<%$Resources:dictionary,Group By :  %>" runat="server"></asp:Literal></td>
            <td style="height: 3px">
                <subsonic:DropDown ID="cmbReportSelector" runat="server" Width="179px" TableName="AttributesLabel" 
                    DataTextField="Label" DataValueField="AttributesNo" /></td>
            <td  style="width: 102px; height: 3px">
                <asp:Literal ID="Literal2" Text="<%$Resources:dictionary, Location : %>" runat="server"></asp:Literal></td>
            <td style="height: 3px">
                <subsonic:DropDown ID="cmbInventoryLocation" runat="server" Width="179px" TableName="InventoryLocation" 
                    DataTextField="InventoryLocationName" DataValueField="InventoryLocationID" 
                    ShowPrompt="true" PromptText="-- ALL --" PromptValue="0" /></td></tr>
        <tr>
            <td colspan=2 >
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:dictionary, Search %>" OnClick="btnSearch_Click" />
            <td colspan=2 align=right class="ExportButton">&nbsp;</td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr><td colspan=4 class="wl_pageheaderSub"><asp:Literal ID = "Literal1" runat="server" Text="<%$Resources:dictionary, Stock Balance Report %>"></asp:Literal></td></tr>
    </table>
    <div style="position:relative; z-index:0;">
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="True" DisplayGroupTree="False" Width="350px" Height="50px" />
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"></asp:ObjectDataSource>
    </div>
</asp:Content>
