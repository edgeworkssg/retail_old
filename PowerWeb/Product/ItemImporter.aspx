<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="ItemImporter.aspx.cs" Inherits="PowerWeb.Product.ItemImporter" Title="Item Importer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables_themeroller.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/FontAwesome/css/font-awesome.min.css") %>" />
    <%--<link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/Tooltipster/css/tooltipster.css") %>" />--%>
    <%--<link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/Tooltipster/css/themes/tooltipster-light.css") %>" />--%>
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/Tipped/css/tipped/tipped.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/POSWeb.css") %>" />
    <input type="hidden" name="BaseUrl" id="BaseUrl" value="<%= ResolveUrl("~/") %>" />
    <input type="hidden" name="DataID" id="DataID" value="" />
    <input type="file" name="FileItem" id="FileItem" class="hide" />
    
    <div class="page">
        <div class="panel align-right crud-button-panel top" id="panelButtonForm">
            <button class="button" id="btnLoad">
                <i class="fa fa-cloud-upload"></i>&nbsp;Load</button>
            <button class="button" id="btnSave">
                <i class="fa fa-save"></i>&nbsp;Save</button>
        </div>
        
         <div class="panel" id="panelRules">
            <div class="form-group">
                <label class="element-label" for="GSTRule">GST Rule</label>
                <div class="element-wrapper">
                    <select name="GSTRule" id="GSTRule" placeholder="GST Rule" class="span2">
                    </select>
                </div>
            </div>
        </div>
        
        <div class="panel boundary-top" id="panelGridItem">
            <div class="grid-outer-wrapper">
                <table class="grid display" id="gridItem">
                    <thead>
                        <th>
                            Status
                        </th>
                        
                        <th>
                            Actions 
                        </th>
                        
                        <th>
                            Department  
                        </th>
                        
                        <th>
                            Category
                        </th>
                        
                        <th>
                            Item No.
                        </th>
                        
                        <th>
                            Item Name
                        </th>
                        
                        <th>
                            Barcode
                        </th>
                    </thead>
                </table>
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
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.jeditable.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery-ui.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.validate.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.dataTables.editable.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Parsley/js/parsley.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/AccountingJs/accounting.min.js") %>"></script>
    <%--<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Tooltipster/js/jquery.tooltipster.min.js") %>"></script>--%>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Tipped/js/tipped/tipped.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Tipped/js/imagesloaded/imagesloaded.pkgd.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/lib.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/setting.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Product/item-importer.js") %>"></script>   
</asp:Content>
