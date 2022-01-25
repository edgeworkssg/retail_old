<%@ Page Title="Print User Barcode" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="PrintUserBarcode.aspx.cs" Inherits="PowerWeb.Scaffolds.PrintUserBarcode" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <CR:CrystalReportViewer ID="crReport" name="CrystalReportViewer" runat="server" DisplayGroupTree="False"
        Width="350px" Height="50px" HasCrystalLogo="False" HasExportButton="True" OnNavigate="CR_Navigate"
        EnableDrillDown="False" HasViewList="False"/>
</asp:Content>
