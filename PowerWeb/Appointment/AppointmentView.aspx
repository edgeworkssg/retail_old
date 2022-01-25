<%@ Page Language="C#" Title ="Appointment View" AutoEventWireup="true" CodeBehind="AppointmentView.aspx.cs" Inherits="PowerWeb.Appointment.AppointmentView" MasterPageFile="~/PowerPOSMSt.master" Theme="default" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.HtmlControls" Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
	 <link href="../App_Themes/Default/AppointmentView.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
	Point of Sales
	<asp:DropDownList ID="cbPointOfSales" runat="server" AutoPostBack="True"></asp:DropDownList>
	<br />
	<br />
	<asp:Calendar ID="Calendar1" runat="server" onselectionchanged="Calendar1_SelectionChanged" ShowGridLines="True" 
		Width="226px" Height="113px"></asp:Calendar>
	<br />
	<br />
	<asp:HtmlGenericControl ID="AppointmentView2" runat="server"></asp:HtmlGenericControl>
	<br />
	
</asp:Content>