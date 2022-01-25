<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true" CodeBehind="SalesView.aspx.cs" Inherits="PowerWeb.Bill.BillList" Title="<%$Resources:dictionary, View Receipt%>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables_themeroller.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/FontAwesome/css/font-awesome.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/JQueryUI/jquery-ui.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/POSWeb.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/Sales/sales-view.css") %>" />
    <input type="hidden" name="BaseUrl" id="BaseUrl" value="<%= ResolveUrl("~/") %>" />
    
    <div class="background-overlay">
        <div class="overlay-wrapper">
            <div class="form-wrapper">
                <div class="header">
                    <div class="title"><asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Data%>" /></div>
                </div>
                <div class="body">
                    <span><asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Void Remark :%>" /> </span>
                    <textarea id="RemarkVoid"></textarea>
                </div>
                <div class="footer">
                    <button class="button" id="btnApplyVoid"><asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Void%>" /></button>
                    <button class="button" id="btnCancelVoid"><asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, Cancel%>" /></button>
                </div>
            </div>   
        </div>    
    </div>
    
    <div class="page">
        <div class="col-3 top-row">
            <div class="form-group">
                <label class=""><span style="font-size:15px;"><asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Date / Time Filter%>" /></span></label>
                <div class="element-wrapper">
                    &nbsp;
                </div>
            </div>
            <div class="form-group">
                <label class=""><span style="font-size:15px;"><asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Filter%>" /></span></label>
                <div class="element-wrapper">
                    &nbsp;
                </div>
            </div>
        </div>    
    </div>
    <div class="page">
        <%--<div class="col-3 top-row">
            <div class="form-group">
                <label class=""><span style="font-size:15px;">Date / Time Filter</span></label>
                <div class="element-wrapper">
                    &nbsp;
                </div>
            </div>
            <div class="form-group">
                <label class=""><span style="font-size:15px;">Filter</span></label>
                <div class="element-wrapper">
                    &nbsp;
                </div>
            </div>
        </div> --%>   
        
        <div class="clear-both spacer"></div> 
        
        <div class="col-3">
            <div class="form-group">
                <label class=""><asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, Start Date%>" /></label>
                <div class="element-wrapper">
                    <input type="text" class="datepicker" id="StartDate" style="width: 120px; text-align: center;" />
                    <img class="calendar-picker btn-date" date-object="StartDate" src="<%= ResolveUrl("~/App_Themes/Default/image/Calendar_scheduleHS.png") %>" style="border-width:0px;" />
                </div>
            </div>
            <div class="form-group">
                <label class=""><asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Ref No.%>" /></label>
                <div class="element-wrapper">
                    <input type="text" id="RefNo" />
                </div>
            </div>
            <div class="form-group">
                <label class=""><asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:dictionary, Status%>" /></label>
                <div class="element-wrapper">
                    <select id="Status">
                        <option value=""><asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary, ALL%>" /></option>
                        <option value="Voided"><asp:Literal ID="Literal10"  runat="server" Text="<%$Resources:dictionary, Voided%>" /></option>
                        <option value="Not Voided"><asp:Literal ID="Literal11"  runat="server" Text="<%$Resources:dictionary, Not Voided%>" /></option>
                    </select>
                </div>
            </div>   
            <%--<div class="form-group">
                <label class="">Name</label>
                <div class="element-wrapper">
                    <input type="text" id="Name" />
                </div>
            </div>--%>
        </div>    
        
        <div class="clear-both spacer"></div> 
        
        <div class="col-3">
            <div class="form-group">
                <label class=""><asp:Literal ID="Literal12"  runat="server" Text="<%$Resources:dictionary, End Date%>" /></label>
                <div class="element-wrapper">
                    <input class="datepicker" type="text" id="EndDate" style="width: 120px; text-align: center;" />
                    <img class="calendar-picker btn-date" date-object="EndDate" src="<%= ResolveUrl("~/App_Themes/Default/image/Calendar_scheduleHS.png") %>" style="border-width:0px;" />
                </div>
            </div>
            <div class="form-group">
                <label class=""><asp:Literal ID="Literal13"  runat="server" Text="<%$Resources:dictionary, Cashier%>" /></label>
                <div class="element-wrapper">
                    <input type="text" id="Cashier" />
                </div>
            </div>
            <div class="form-group">
                <label class=""><asp:Literal ID="Literal14"  runat="server" Text="<%$Resources:dictionary, Remark%>" /></label>
                <div class="element-wrapper">
                    <input type="text" id="Remark" />
                </div>
            </div>
        </div>  
        
        <div class="clear-both spacer"></div> 
        
        <div class="col-3">
            <div class="form-group">
                <label class="">&nbsp;</label>
            </div>
            <div class="form-group">
                <label class=""><asp:Literal ID="Literal15"  runat="server" Text="<%$Resources:dictionary, Name%>" /></label>
                <div class="element-wrapper">
                    <input type="text" id="Name" />
                </div>
            </div>
        </div>  
        
        <div class="clear-both spacer"></div>        
        
        <div class="col-3">
            <div class="form-group">
                <label class="">&nbsp;</label>
                <div class="element-wrapper">
                    <button class="button" id="btnSearch"><i class="fa fa-search"></i> <asp:Literal ID="Literal16"  runat="server" Text="<%$Resources:dictionary, Search%>" /></button>
                    <button class="button" id="btnExport"><i class="fa fa-excel-o"></i> <asp:Literal ID="Literal17"  runat="server" Text="<%$Resources:dictionary, Export%>" /></button>
                </div>
            </div>
        </div>  
        
        <div class="clear-both spacer"></div>
        
        <div class="align-right notes"><asp:Literal ID="Literal18"  runat="server" Text="<%$Resources:dictionary, NOTE : The changes will only be reflected in the Backend. Front End POS will not reflect the changes. (Only for hosted)%>" /></div>
        
        <div class="clear-both spacer"></div>
       
        <div class="panel boundary-top no-margin-left" id="panelGridCommission" style="margin-left: 0px;">
            <div class="grid-outer-wrapper">
                <table class="grid display" id="gridSales">
                    <thead>
                        <th>
                            <asp:Literal ID="Literal19"  runat="server" Text="<%$Resources:dictionary, Action%>" /> &nbsp;
                        </th>
                        
                        <th>
                            <asp:Literal ID="Literal20"  runat="server" Text="<%$Resources:dictionary, Order Ref No.%>" /> &nbsp;
                        </th>
                        
                        <th>
                            <asp:Literal ID="Literal21"  runat="server" Text="<%$Resources:dictionary, Amount%>" /> &nbsp;
                        </th>
                        
                        <th>
                            <asp:Literal ID="Literal22"  runat="server" Text="<%$Resources:dictionary, Date%>" /> &nbsp;
                        </th>
                        
                        <th>
                            <asp:Literal ID="Literal23"  runat="server" Text="<%$Resources:dictionary, Remark%>" /> &nbsp;
                        </th>
                        
                        <th>
                            <asp:Literal ID="Literal24"  runat="server" Text="<%$Resources:dictionary, Cashier%>" /> &nbsp;
                        </th>
                        
                        <th>
                            <asp:Literal ID="Literal25"  runat="server" Text="<%$Resources:dictionary, Payment Mode%>" /> &nbsp;
                        </th>
                        
                        <th>
                            <asp:Literal ID="Literal26"  runat="server" Text="<%$Resources:dictionary, Card No.%>" /> &nbsp;
                        </th>
                        
                        <th>
                            <asp:Literal ID="Literal27"  runat="server" Text="<%$Resources:dictionary, Name%>" /> &nbsp;
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
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/JQueryUI/jquery-ui.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.dataTables.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Parsley/js/parsley.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/AccountingJs/accounting.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/MomentJs/moment.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/lib.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/setting.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Sales/sales-view.js") %>"></script>
</asp:Content>


