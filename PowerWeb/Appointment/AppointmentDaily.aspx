<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="AppointmentDaily.aspx.cs" Inherits="PowerWeb.Appointment.AppointmentDaily"
    Title="<%$Resources:dictionary, Daily Appointment View%>" %>

<%@ Register Src="UCAppCalendarDaily.ascx" TagName="UCAppCalendarDaily"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link rel='stylesheet prefetch' href='http://netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css' />
    <style type="text/css">
        .fixedTable .table
        {
            background-color: white;
            width: auto;
        }
        .fixedTable .table tr th
        {
            padding: 0px !important;
        }
        .fixedTable .table tr td
        {
            min-height: 40px !important;
            height: 40px !important;
            max-height: 40px !important;
            min-width: 100px;
            width: 100px;
            padding: 0px;
            color: #fff;
            background: #D0D0D0;
            padding-top: 0px !important;
        }
        .fixedTable .table tr td:hover
        {
            cursor: pointer;
            background: #C4C4C4;
        }
        .fixedTable-header
        {
            width: 800px;
            height: 120px;
            margin-left: 100px;
            overflow: hidden;
            border-bottom: 1px solid #F5F5F5;
        }
        .fixedTable-sidebar
        {
            width: 100px;
            height: 310px;
            float: left;
            overflow: hidden;
            border-right: 1px solid #F5F5F5;
        }
        .fixedTable-body
        {
            overflow: auto;
            width: 800px;
            height: 310px;
            float: left;
        }
        .fixed-icon
        {
            position: relative;
            top: 0;
            left: 0;
            height: 100px;
            width: 100px;
            float: left;
            padding: 1px 1px 1px 1px;
            background: #808080;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/JQUeryUI/jquery-ui.css") %>" />
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <uc1:UCAppCalendarDaily ID="UCAppCalendarDaily1" runat="server" />

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/JQueryUI/jquery-ui.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.dataTables.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Parsley/js/parsley.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/AccountingJs/accounting.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Tooltipster/js/jquery.tooltipster.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/lib.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/setting.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Membership/new-membership.js") %>"></script>
    
</asp:Content>
