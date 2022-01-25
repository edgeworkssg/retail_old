<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" Theme="Default" AutoEventWireup="true" Inherits="PromoCodeScaffold" Title="<%$Resources:dictionary,Promo Code Setup %>" Codebehind="PromoCodeScaffold.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <subsonic:Scaffold ID="Scaffold1" runat="server" TableName="PromoCode"  
    GridViewSkinID="scaffold" EditTableItemCaptionCellCssClass="scaffoldEditLabel" 
    ButtonCssClass="scaffoldButton" TextBoxCssClass="scaffoldTextBox">
    </subsonic:Scaffold>
</asp:Content>

