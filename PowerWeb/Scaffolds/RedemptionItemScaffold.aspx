<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" Theme="Default" AutoEventWireup="true" Inherits="RedemptionItemScaffold" Title="<%$Resources:dictionary,Redemption Item Setup %>" Codebehind="RedemptionItemScaffold.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <subsonic:Scaffold ID="Scaffold1" runat="server" TableName="RedemptionItem"  
    GridViewSkinID="scaffold" EditTableItemCaptionCellCssClass="scaffoldEditLabel" 
    ButtonCssClass="scaffoldButton" TextBoxCssClass="scaffoldTextBox">
    </subsonic:Scaffold>
</asp:Content>

