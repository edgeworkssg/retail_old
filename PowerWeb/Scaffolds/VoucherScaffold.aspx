<%@ Page Theme="Default" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="VoucherScaffold" Title="<%$Resources:dictionary,Voucher Setup %>" Codebehind="VoucherScaffold.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
    <subsonic:Scaffold ID="Scaffold1" runat="server" TableName="Vouchers" 
    EditTableCssC EditTableItemCss  
    GridViewSkinID="scaffold" EditTableItemCaptionCellCssClass="scaffoldEditLabel" 
    ButtonCssClass="scaffoldButton" TextBoxCssClass="scaffoldTextBox"   >
    </subsonic:Scaffold>
</asp:Content>

