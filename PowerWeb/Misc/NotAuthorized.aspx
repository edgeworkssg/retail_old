<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" Inherits="NotAuthorized" Title="<%$Resources:dictionary,Insufficient Privilege %>" Codebehind="NotAuthorized.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<h1 align=center ><asp:Literal ID = "Lbl" runat="server" Text="<%$Resources:dictionary, You do not have sufficient privilege to access this page.%>"></asp:Literal></h1>
</asp:Content>

