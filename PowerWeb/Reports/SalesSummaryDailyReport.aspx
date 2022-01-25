<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="SalesSummaryDailyReport.aspx.cs" Inherits="PowerWeb.Reports.DailySalesSummaryReport" Title="<%$Resources:dictionary, Daily Sales Summary %>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables_themeroller.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/FontAwesome/css/font-awesome.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/JQUeryUI/jquery-ui.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/POSWeb.css") %>" />
    <input type="hidden" name="BaseUrl" id="BaseUrl" value="<%= ResolveUrl("~/") %>" />
    <div class="page">
        <div class="panel" id="panelInventory">
            <input type="hidden" name="UniqueID" id="UniqueID" value="" />
            <div class="form-group">
                <label class="element-label" for="SalesDate"><asp:Literal ID="Literal39"  runat="server" Text="<%$Resources:dictionary, Date%>" /></label>
                <div class="element-wrapper">
                    <input name="SalesDate" id="SalesDate" class="span3 datepicker" data-parsley-required="true" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="Outlet"><asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Outlet%>" /></label>
                <div class="element-wrapper">
                    <select name="Outlet" id="Outlet" class="span3" data-parsley-required="true">
                    </select>
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="">&nbsp;</label>
                <div class="element-wrapper">
                    <button class="button" id="btnExport">
                        <i class="fa fa-file-excel-o"></i>&nbsp;<asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Export%>" /></button> 
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function() {
            app.setting.baseUrl = $('#BaseUrl').val();
        });
    </script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.dataTables.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Parsley/js/parsley.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/AccountingJs/accounting.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/JQueryUI/jquery-ui.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/lib.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/setting.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Report/Sales/sales-summary-daily-report.js") %>"></script>

</asp:Content>
