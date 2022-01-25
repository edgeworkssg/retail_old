<%@ Page Theme="Default" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="FeedBackMsgScaffold" Title="<%$Resources:dictionary,Feedback Setup %>" Codebehind="FeedBackMsgScaffold.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <subsonic:Scaffold ID="Scaffold1" runat="server" TableName="FeedBackMsg" GridViewSkinID="scaffold"  EditTableItemCaptionCellCssClass="scaffoldEditLabel" ButtonCssClass="scaffoldButton" TextBoxCssClass="scaffoldTextBox"
    HiddenGridColumns="UserFld1,UserFld2,UserFld3,UserFld4,UserFld5,UserFld6,UserFld7,UserFld8,UserFld9,UserFld10,Userfloat1,UserFloat2,UserFloat3,UserFloat4,UserFloat5,Userflag1,UserFlag2,UserFlag3,UserFlag4,UserFlag5,Userint1,UserInt2,UserInt3,UserInt4,UserInt5"
      HiddenEditorColumns="UserFld1,UserFld2,UserFld3,UserFld4,UserFld5,UserFld6,UserFld7,UserFld8,UserFld9,UserFld10,Userfloat1,UserFloat2,UserFloat3,UserFloat4,UserFloat5,Userflag1,UserFlag2,UserFlag3,UserFlag4,UserFlag5,Userint1,UserInt2,UserInt3,UserInt4,UserInt5">
    </subsonic:Scaffold>
</asp:Content>

