<%@ Page Theme="Default" Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    Inherits="ItemCostPriceScaffold" Title="<%$Resources:dictionary,Item Cost Price Setup %>"
    CodeBehind="ItemCostPriceScaffold.aspx.cs" %>

<%--GridViewSkinID="scaffold"  EditTableCssC EditTableItemCss  EditTableItemCaptionCellCssClass="scaffoldEditLabel" TextBoxCssClass="scaffoldTextBox"--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <subsonic:Scaffold ID="Scaffold1" runat="server" TableName="ItemCostPrice" GridViewSkinID="scaffold"
        ButtonCssClass="classname" EditTableItemCaptionCellCssClass="wl_bodytxt" HiddenGridColumns="UserFld1,UserFld2,UserFld3,UserFld4,UserFld5,UserFld6,UserFld7,UserFld8,UserFld9,UserFld10,Userfloat1,UserFloat2,UserFloat3,UserFloat4,UserFloat5,Userflag1,UserFlag2,UserFlag3,UserFlag4,UserFlag5,Userint1,UserInt2,UserInt3,UserInt4,UserInt5"
        HiddenEditorColumns="UserFld1,UserFld2,UserFld3,UserFld4,UserFld5,UserFld6,UserFld7,UserFld8,UserFld9,UserFld10,Userfloat1,UserFloat2,UserFloat3,UserFloat4,UserFloat5,Userflag1,UserFlag2,UserFlag3,UserFlag4,UserFlag5,Userint1,UserInt2,UserInt3,UserInt4,UserInt5">
    </subsonic:Scaffold>
</asp:Content>
