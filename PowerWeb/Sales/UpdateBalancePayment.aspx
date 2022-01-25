<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="UpdateBalancePayment.aspx.cs" Inherits="PowerWeb.Sales.UpdateBalancePayment"
    Title="<%$Resources:dictionary, Update Balance Payment%>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables_themeroller.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/FontAwesome/css/font-awesome.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/Tipped/css/tipped/tipped.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/JQueryUI/jquery-ui.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/POSWeb.css") %>" />
    <input type="hidden" name="BaseUrl" id="BaseUrl" value="<%= ResolveUrl("~/") %>" />
    <div class="page">
        <div class="panel align-right crud-button-panel top" id="panelButtonForm">
            <button class="button" id="buttonSave">
                <i class="fa fa-save"></i>&nbsp;<asp:Literal ID="ltr00"  runat="server" Text="<%$Resources:dictionary, Save%>" /></button>
            <button class="button" id="buttonCancel">
                <i class="fa fa-refresh"></i>&nbsp;<asp:Literal ID="Literal1"  runat="server" Text="<%$Resources:dictionary, Cancel%>" /></button>
        </div>
        <div class="panel" id="panelFilter">
            <div class="form-group">
                <label class="element-label" for="filterDeliveryNo">
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$Resources:dictionary, Delivery No.%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="filterDeliveryNo" id="filterDeliveryNo" placeholder="Delivery No."
                        class="span2" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="filterInvoiceNo">
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$Resources:dictionary, Invoice No.%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="filterInvoiceNo" id="filterInvoiceNo" placeholder="Invoice No."
                        class="span2" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="filterOutstanding">
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:dictionary, Outstanding%>" /></label>
                <div class="element-wrapper">
                    <select type="text" name="filterOutstanding" id="filterOutstanding" placeholder="Outstanding"
                        class="span2">
                        <option value=""><asp:Literal ID="Literal5"  runat="server" Text="<%$Resources:dictionary, Select All%>" /></option>
                        <option value="0"><asp:Literal ID="Literal6"  runat="server" Text="<%$Resources:dictionary, Fully Paid%>" /></option>
                        <option value="1"><asp:Literal ID="Literal7"  runat="server" Text="<%$Resources:dictionary, Outstanding%>" /></option>
                    </select>
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="">
                    <asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:dictionary, Delivery Date%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="filterStartDate" id="filterStartDate" placeholder="" class="span1 datepicker" />
                    <img class="date-picker" data-action="datepicker" data-element="filterStartDate"
                        src="<%= ResolveUrl("~/App_Themes/Default/image/Calendar_scheduleHS.png") %>" />
                    &nbsp;&nbsp;&nbsp;to&nbsp;&nbsp;&nbsp;
                    <input type="text" name="filterEndDate" id="filterEndDate" placeholder="" class="span1 datepicker" />
                    <img class="date-picker" data-action="datepicker" data-element="filterEndDate" src="<%= ResolveUrl("~/App_Themes/Default/image/Calendar_scheduleHS.png") %>" />
                </div>
            </div>
            <div class="form-group bottom">
                <label class="element-label" for="">
                    &nbsp;</label>
                <div class="element-wrapper">
                    <button class="button" id="buttonSearch">
                        <i class="fa fa-search"></i><asp:Literal ID="Literal9"  runat="server" Text="<%$Resources:dictionary, Search%>" /></button>
                    <button class="button" id="buttonClearSearch">
                        <i class="fa fa-eraser"></i><asp:Literal ID="Literal10"  runat="server" Text="<%$Resources:dictionary,Clear %>" /></button>
                </div>
            </div>
        </div>
        <div class="panel boundary-top" id="panelGridBalancePayment">
            <div class="grid-outer-wrapper">
                <table class="grid display" id="gridBalancePayment">
                    <thead>
                        <th>
                            <asp:Literal ID="Literal11"  runat="server" Text="<%$Resources:dictionary, Action%>" />
                        </th>
                        <th>
                            <asp:Literal ID="Literal12"  runat="server" Text="<%$Resources:dictionary, Delivery Order No.%>" />
                        </th>
                        <th>
                            <asp:Literal ID="Literal13"  runat="server" Text="<%$Resources:dictionary, Outlet%>" />
                        </th>
                        <th>
                            <asp:Literal ID="Literal14"  runat="server" Text="<%$Resources:dictionary, Invoice No%>" />
                        </th>
                        <th>
                            <asp:Literal ID="Literal15"  runat="server" Text="<%$Resources:dictionary, Invoice Date%>" />
                        </th>
                        <th>
                            <asp:Literal ID="Literal16"  runat="server" Text="<%$Resources:dictionary, Recipient Name%>" />
                        </th>
                        <th>
                            <asp:Literal ID="Literal17"  runat="server" Text="<%$Resources:dictionary, Mobile No.%>" />
                        </th>
                        <th>
                            <asp:Literal ID="Literal18"  runat="server" Text="<%$Resources:dictionary, Home No.%>" />
                        </th>
                        <th>
                            <asp:Literal ID="Literal19"  runat="server" Text="<%$Resources:dictionary, Delivery Address%>" />
                        </th>
                        <th>
                            <asp:Literal ID="Literal20"  runat="server" Text="<%$Resources:dictionary,Delivery Date & Time %>" />
                        </th>
                        <th>
                            <asp:Literal ID="Literal21"  runat="server" Text="<%$Resources:dictionary, Total Payment%>" />
                        </th>
                        <th>
                            <asp:Literal ID="Literal22"  runat="server" Text="<%$Resources:dictionary, Balance Payment%>" />
                        </th>
                        <th>
                            <asp:Literal ID="Literal23"  runat="server" Text="<%$Resources:dictionary, Payment Remarks%>" />
                        </th>
                    </thead>
                </table>
            </div>
        </div>
        <div class="panel" id="panelForm">
            <div class="form-group">
                <label class="element-label" for="OrderNo">
                    <asp:Literal ID="Literal24"  runat="server" Text="<%$Resources:dictionary, Delivery No.%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="CustomDeliveryNo" id="CustomDeliveryNo" placeholder="" class="span4"
                        disabled="true" data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="OutletName">
                    <asp:Literal ID="Literal25"  runat="server" Text="<%$Resources:dictionary,Outlet %>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="OutletName" id="OutletName" placeholder="" class="span4"
                        disabled="true" data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="OrderDate">
                    <asp:Literal ID="Literal26"  runat="server" Text="<%$Resources:dictionary, Invoice Date%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="InvoiceDate" id="InvoiceDate" placeholder="" class="span4"
                        disabled="true" data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="RecipientName">
                    <asp:Literal ID="Literal27"  runat="server" Text="<%$Resources:dictionary, Recipient Name%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="RecipientName" id="RecipientName" placeholder="" class="span4"
                        disabled="true" data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="MobileNo">
                    <asp:Literal ID="Literal28"  runat="server" Text="<%$Resources:dictionary, Mobile No.%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="MobileNo" id="MobileNo" placeholder="" class="span4" disabled="true"
                        data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="HomeNo">
                    <asp:Literal ID="Literal29"  runat="server" Text="<%$Resources:dictionary, Home No.%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="HomeNo" id="HomeNo" placeholder="" class="span4" disabled="true"
                        data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="Office">
                    <asp:Literal ID="Literal30"  runat="server" Text="<%$Resources:dictionary, Office%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="Office" id="Office" placeholder="" class="span4" disabled="true"
                        data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="PostalCode">
                    <asp:Literal ID="Literal31"  runat="server" Text="<%$Resources:dictionary, Postal Code%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="PostalCode" id="PostalCode" placeholder="" class="span4"
                        disabled="true" data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="DeliveryAddress">
                    <asp:Literal ID="Literal32"  runat="server" Text="<%$Resources:dictionary, Address%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="DeliveryAddress" id="DeliveryAddress" placeholder="" class="span4"
                        disabled="true" data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="UnitNo">
                    <asp:Literal ID="Literal33"  runat="server" Text="<%$Resources:dictionary, Unit No.%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="UnitNo" id="UnitNo" placeholder="" class="span4" disabled="true"
                        data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="DeliveryDate">
                    <asp:Literal ID="Literal34"  runat="server" Text="<%$Resources:dictionary, Delivery Date %>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="DeliveryDate" id="DeliveryDate" placeholder="" class="span4"
                        disabled="true" data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="DeliveryTime">
                    <asp:Literal ID="Literal35"  runat="server" Text="<%$Resources:dictionary, Delivery Time%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="DeliveryTime" id="DeliveryTime" placeholder="" class="span4"
                        disabled="true" data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="Remarks">
                    <asp:Literal ID="Literal36"  runat="server" Text="<%$Resources:dictionary, Remarks%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="Remarks" id="Remarks" placeholder="" class="span4" disabled="true"
                        data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <br />
            <br />
            <h2>
                <asp:Literal ID="Literal37"  runat="server" Text="<%$Resources:dictionary, Payment Details%>" /></h2>
            <hr style="width: 555px; margin-left: 0px; height: 2px; border: 0 none; background-color: Black;" />
            <div class="form-group">
                <label class="element-label" for="InstallmentShowed">
                    <asp:Literal ID="Literal38"  runat="server" Text="<%$Resources:dictionary, Installment%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="InstallmentShowed" id="InstallmentShowed" placeholder=""
                        class="span1 align-right" disabled="true" data-parsley-required="false" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="PaymentMode">
                    <asp:Literal ID="Literal39"  runat="server" Text="<%$Resources:dictionary, Payment Mode%>" /></label>
                <div class="element-wrapper">
                    <select type="text" name="PaymentMode" id="PaymentMode" class="span2" maxlength="50"
                        data-parsley-required="true">
                    </select>
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="PaymentAmount">
                    <asp:Literal ID="Literal40"  runat="server" Text="<%$Resources:dictionary, Payment Amount%>" /></label>
                <div class="element-wrapper">
                    <input type="text" name="PaymentAmount" id="PaymentAmount" placeholder="" class="span1 align-right"
                        data-parsley-required="true" data-parsley-type="number" maxlength="50" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="Remark">
                    <asp:Literal ID="Literal41"  runat="server" Text="<%$Resources:dictionary, Remark%>" /></label>
                <div class="element-wrapper">
                    <textarea name="Remark" id="Remark" placeholder="" class="span4" data-parsley-required="false"
                        maxlength="200"></textarea>
                </div>
            </div>
            <input type="hidden" name="PointOfSaleID" id="PointOfSaleID" />
            <input type="hidden" name="BalancePayment" id="BalancePayment" />
            <input type="hidden" name="InvoiceNo" id="InvoiceNo" />
            <input type="hidden" name="MembershipNo" id="MembershipNo" />
        </div>
    </div>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.js") %>"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            app.setting.baseUrl = $('#BaseUrl').val();
        });
    </script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.dataTables.min.js") %>"></script>

    <%--<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.jeditable.js") %>"></script>--%>
    <%--<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery-ui.js") %>"></script>--%>
    <%--<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.validate.js") %>"></script>--%>
    <%--<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.dataTables.editable.js") %>"></script>--%>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/JQueryUI/jquery-ui.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Parsley/js/parsley.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/AccountingJs/accounting.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/MomentJs/moment.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Tipped/js/tipped/tipped.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Tipped/js/imagesloaded/imagesloaded.pkgd.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/lib.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/setting.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Sales/update-balance-payment.js") %>"></script>

</asp:Content>
