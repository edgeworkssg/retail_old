<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" Theme="Default" AutoEventWireup="true" Inherits="MembershipRemarkCategoryScaffold" Title="<%$Resources:dictionary,Membership Remark Category Setup %>" Codebehind="MembershipRemarkCategoryScaffold.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <subsonic:Scaffold ID="Scaffold1" runat="server" TableName="MembershipRemarkCategory"  GridViewSkinID="scaffold" EditTableItemCaptionCellCssClass="scaffoldEditLabel" ButtonCssClass="scaffoldButton" TextBoxCssClass="scaffoldTextBox">
    </subsonic:Scaffold>
</asp:Content>

