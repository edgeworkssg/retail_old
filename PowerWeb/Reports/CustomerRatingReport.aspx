<%@ Page Title="<%$Resources:dictionary,Customer Rating Report %>" Language="C#"
    MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="CustomerRatingReport.aspx.cs"
    Inherits="PowerWeb.Reports.CustomerRatingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables_themeroller.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/POSWeb.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/FontAwesome/css/font-awesome.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Applications/Rating/RatingReport.css") %>" />
    
    <div class="page">
        <input type="hidden" name="BaseUrl" id="BaseUrl" value="<%= ResolveUrl("~/") %>" />
        <input type="hidden" name="paramGroupBy" id="paramGroupBy" value="" />
        <input type="hidden" name="paramFilter" id="paramFilter" value="" />
        <input type="hidden" name="paramStartDate" id="paramStartDate" value="" />
        <input type="hidden" name="paramEndDate" id="paramEndDate" value="" />
        
        <table class="panel align-left">
            <tr>
                <td style="width: 80px">
                    <label class="font-13" for="ddlOutlet"><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:dictionary,Outlet %>" /></label>
                </td>
                <td>
                    <select name="ddlOutlet" id="ddlOutlet">
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    <label class="font-13" for="ddlStaff"><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:dictionary,Staff %>" /></label>
                </td>
                <td>
                    <input name="txtStaff" id="txtStaff" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
        </table>
        <div class="panel align-left" id="panelButtonForm">
            <button class="button button-small" id="btnStaff">
                By Staff</button>
            <button class="button button-small button-gray" id="btnOutlet">
                By Outlet</button>
        </div>
        <div class="panel align-left" id="divFilter">
            <a href="#" id="filterToday" class="font-13 filter-active">Today</a> | <a href="#" id="filterWeek" class="font-13 filter-nonactive">This Week</a>
            | <a href="#" id="filterMonth" class="font-13 filter-nonactive">This Month</a> | <a href="#" id="filterYear" class="font-13 filter-nonactive">This Year</a>
        </div>
        <div class="panel align-left" style="margin-top: 10px;" id="divDate">
            <button class="button button-small" id="btnPrev">
                <i class="fa fa-chevron-left"></i>
            </button>
            &nbsp;<span id="lblDate"></span>&nbsp;
            <button class="button button-small" id="btnNext">
                <i class="fa fa-chevron-right"></i>
            </button>
        </div>
        <br />
        <div id="rating"></div>
    </div>
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.dataTables.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Parsley/js/parsley.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/lib.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/setting.js") %>"></script>
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/MomentJs/moment.js") %>"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            app.setting.baseUrl = $('#BaseUrl').val();
        });
    </script>
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Rating/RatingReport.js") %>"></script>
</asp:Content>
