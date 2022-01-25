<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="ItemImporter.aspx.cs" Inherits="PowerWeb.Support.ItemImporter" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager id="ScriptManager1" runat="server" />
    <div style="position:relative;z-index:2;">
        <table id="FilterTable" runat="server" width="1122px">
            <tr><td colspan="4" class="wl_pageheaderSub" style="width: 100%"><asp:Literal ID = "SEARCHLbl" runat="server" Text="IMPORT"></asp:Literal></td></tr>
            <tr>
                <td  colspan="2" style="width:561px; height:3px; text-align:left">
                    <asp:Literal ID="Literal1" runat="server" Text="Step 1: Please choose your file and press upload." /> <br />
                    <asp:FileUpload ID="FileUploader" runat="server" EnableViewState="true" /> <br />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /> </td>
                <td  colspan="2" style="width:561px; height:3px; text-align:left">
                    <asp:Literal runat="server" Text="Step 2: After confirm the list, please click the Save button." /> <br />
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblMsg" runat="server" CssClass="LabelMessage"></asp:Label>
                </td>
                <td align="right" class="ExportButton">
                    <asp:Literal runat="server" Text="Export to: " />
                    <asp:LinkButton ID="lnkExportRaw" runat="server" ToolTip="Data Only" OnClick="lnkExportRaw_Click">
                        <img id="ImgGrid" src="../App_Themes/Default/image/CSV.jpg" width="20" height="20" alt="Export to Excel (No Format)" /></asp:LinkButton>
                    <asp:LinkButton ID="lnkExportExcel" runat="server" ToolTip="Excel" OnClick="lnkExportExcel_Click">
                        <img id="ImgExcel" src="../App_Themes/Default/image/Excel.jpg" width="20" height="20" alt="Export to Excel" /></asp:LinkButton>
                    <asp:LinkButton ID="lnkExportPDF" runat="server" ToolTip="PDF" OnClick="lnkExportPDF_Click">
                        <img id="ImgPDF" src="../App_Themes/Default/image/PDF.jpg" width="20" height="20" alt="Export to PDF")" /></asp:LinkButton></td>
            </tr>
        </table>
    </div>
    <div style="position:relative; z-index:0;">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"  
            DisplayGroupTree="False" Width="100%" Height="50px" 
            HasCrystalLogo="False" HasExportButton="False"/>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"></asp:ObjectDataSource>
    </div>
</asp:Content>
