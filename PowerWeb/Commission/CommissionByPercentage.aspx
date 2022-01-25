<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="CommissionByPercentage.aspx.cs" Inherits="PowerWeb.Commission.CommissionByPercentage" Title="<%$Resources:dictionary,Commission By Percentage Setup %>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables_themeroller.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/FontAwesome/css/font-awesome.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/POSWeb.css") %>" />
    <input type="hidden" name="BaseUrl" id="BaseUrl" value="<%= ResolveUrl("~/") %>" />
    
    <div class="page">
        <div class="panel align-right crud-button-panel top" id="panelButtonGrid">
            <button class="button" id="btnAdd">
                <i class="fa fa-plus"></i>&nbsp;<asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Add%>" /></button>
            <%--<button class="button" id="btnExport">
                <i class="fa fa-file-excel-o"></i>&nbsp;Export</button>--%>
        </div>
        <div class="panel align-right crud-button-panel top" id="panelButtonForm">
            <button class="button" id="btnSave">
                <i class="fa fa-save"></i>&nbsp;<asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Save%>" /></button>
            <button class="button" id="btnCancel">
                <i class="fa fa-refresh"></i>&nbsp;<asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Cancel%>" /></button>
        </div>
        <div class="panel boundary-top" id="panelGridCommission">
            <div class="grid-outer-wrapper">
                <table class="grid display" id="gridCommission">
                    <thead>
                        <th>
                            <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, Actions%>" /> &nbsp;
                        </th>
                        <th>
                            <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Sales Group%>" /> &nbsp;
                        </th>
                        
                        <th>
                            <asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Transaction Type%>" /> &nbsp;
                        </th>
                        
                        <th>
                            <asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, Lower Limit%>" /> &nbsp;
                        </th>
                        
                        <th>
                            <asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Upper Limit%>" /> &nbsp;
                        </th>
                        
                        <th>
                            <asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:dictionary, (%) Commission%>" /> &nbsp;
                        </th>
                    </thead>
                </table>
            </div>
        </div>
        <div class="panel" id="panelUpdateCommission">
            <input type="hidden" name="UniqueID" id="UniqueID" value="" />
        
            <div class="form-group">
                <label class="element-label" for="SalesGroupID">
                    <asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary, Sales Group%>" /></label>
                <div class="element-wrapper">
                    <select name="SalesGroupID" id="SalesGroupID" class="span2" data-parsley-required="true">
                    </select>
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="CommissionCategory">
                    <asp:Literal ID="Literal10"  runat="server" Text="<%$Resources:dictionary, Commmission Type%>" /></label>
                <div class="element-wrapper">
                    <select name="CommissionType" id="CommissionType" class="span2" data-parsley-required="true">
                    </select>
                </div>
            </div>  
            <div class="form-group">
                <label class="element-label" for="LowerLimit"><asp:Literal ID="Literal11"  runat="server" Text="<%$Resources:dictionary, Lower Limit%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="LowerLimit" id="LowerLimit" value="" class="span1" data-parsley-type="number" maxlength="21" />
                </div>
            </div>  
            <div class="form-group">
                <label class="element-label" for="UpperLimit"><asp:Literal ID="Literal12"  runat="server" Text="<%$Resources:dictionary, Upper Limit%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="UpperLimit" id="UpperLimit" value="" class="span1" data-parsley-type="number" maxlength="21" />
                </div>
            </div> 
            <div class="form-group">
                <label class="element-label" for="PercentCommission"><asp:Literal ID="Literal13"  runat="server" Text="<%$Resources:dictionary, Commission (%)%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="PercentCommission" id="PercentCommission" value="" class="span1" data-parsley-required="true" data-parsley-type="number" maxlength="8" />
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
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/lib.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/setting.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Commission/commission-by-percentage.js") %>"></script>
</asp:Content>
