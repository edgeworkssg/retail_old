$(document).ready(function() {
    initElementState();
    initElementEvent();
    loadDataSource();
    
    function initElementState() {
        $('#panelButtonForm').hide();
        $('#panelForm').hide();
        $('.datepicker').datepicker({
            dateFormat: 'yy-mm-dd'
        }).datepicker('setDate', new Date());
        
        initGrid();
    }
    
    function loadDataSource() {
        ajaxPost({
            url: app.setting.baseUrl + 'API/Lookup/Outlet.ashx',
            success: function(result) {
                loadDropdownDataSource('filterOutlet', result, false);
            }
        });
        ajaxPost({
            url: app.setting.baseUrl + 'API/Lookup/PaymentTypes.ashx',
            success: function(result) {
                loadDropdownDataSource('PaymentMode', result, false);
            }
        });
    }
    
    function initElementEvent() {
        $.each($('[data-action="datepicker"]') || [], function(index, element) {
            var _this = $(this);
            var parentElement = _this.attr('data-element');
            _this.off();
            _this.on('click', function(evt) {
                evt.preventDefault();
                $('#' + parentElement).focus();
            });
        });
        
        var buttonSearch = $('#buttonSearch');
        var buttonClearSearch = $('#buttonClearSearch');
        var buttonSave = $('#buttonSave');
        var buttonCancel = $('#buttonCancel');
        
        buttonSearch.off();
        buttonSearch.on('click', function(evt) {
            evt.preventDefault();
            reloadGrid();
        });
        
        buttonClearSearch.off();
        buttonClearSearch.on('click', function(evt) {
            evt.preventDefault();
            
            $('input, select').val('');
            $('.datepicker').datepicker('setDate', new Date());
            reloadGrid();
        });
        
        buttonSave.off();
        buttonSave.on('click', function(evt) {
            evt.preventDefault();
            
            var url = app.setting.baseUrl + "API/Sales/UpdateBalancePayment/Pay.ashx";
            var params = {
                InvoiceNo: $('#InvoiceNo').val(),
                PointOfSaleID: $('#PointOfSaleID').val(),
                PaymentMode: $('#PaymentMode').val(),
                PaymentAmount: $('#PaymentAmount').val(),
                BalancePayment: $('#BalancePayment').val(),
                Remark: $('#Remark').val(),
                MembershipNo: $('#MembershipNo').val()
            };
            
            var validation = $('form').parsley();
            if(validation.validate()) {
                ajaxPost({
                    url: url,
                    data: params,
                    success: function(result) {
                        if(result.Status == true) {
                            showFrontEndForm();
                            reloadGrid();
                        }
                        
                        showNotification(result.Message);
                        clearErrorValidation();
                    }
                });
            }
        });
        
        buttonCancel.off();
        buttonCancel.on('click', function(evt) {
            evt.preventDefault();
            showFrontEndForm();
            clearErrorValidation();
        });
    }
    
    function showFormDetail() {
        $('#panelGridBalancePayment').hide();
        $('#panelFilter').hide();
        $('#panelButtonForm').fadeIn();
        $('#panelForm').fadeIn();
    }
    
    function showFrontEndForm() {
        $('#panelGridBalancePayment').fadeIn();
        $('#panelFilter').fadeIn();
        $('#panelButtonForm').hide();
        $('#panelForm').hide();
    }
    
    function initGrid() {
        var url = app.setting.baseUrl + 'API/Sales/UpdateBalancePayment/List.ashx';
            
        var gridName = '#gridBalancePayment';
        $('#panelGridBalancePayment').fadeIn();
        $(gridName).dataTable().fnDestroy();
        $(gridName).DataTable({
            aoColumns: [
                {
                    sWidth: '40px',
                    sClass: 'align-center',
                    mData: null,
                    mRender: function () {
                        return '<i data-action="payment" class="fa fa-check-square table-button"> Pay</i>';
                    }
                },
                { mData: 'CustomDeliveryNo', width: '100px' },
                { mData: 'OutletName', width: '100px' },
                { mData: 'CustomInvoiceNo', width: '100px' },
                { mData: 'InvoiceDate', width: '100px', mRender: function(data) {
                    return moment(new Date(data)).format('DD-MMM-YYYY');
                  } 
                },
                { mData: 'RecipientName', width: '100px' },
                { mData: 'MobileNo', width: '100px' },
                { mData: 'HomeNo', width: '100px' },
                { mData: 'DeliveryAddress', width: '100px' },
                { mData: 'DeliveryDateDetails', width: '100px' },
                { mData: 'Credit', sClass: 'align-right', width: '100px', mRender: function(data){
                    return accounting.formatMoney(data, { symbol: '$', format: '%s %v' });
                  }
                },
                { mData: 'BalancePayment', sClass: 'align-right', width: '100px', mRender: function(data){
                    return accounting.formatMoney(data, { symbol: '$', format: '%s %v' });
                  }
                },
                { mData: 'PaymentRemarks', width: '100px' },
            ],
            "aLengthMenu": [
                [5, 10, 20, 50, 100, 150, -1],
                [5, 10, 20, 50, 100, 150, "All"]
            ],
            "fnServerParams": function (aoData) {
                aoData.push({
                    name: "StartDate",
                    value: $('#filterStartDate').val()
                });
                aoData.push({
                    name: "EndDate",
                    value: $('#filterEndDate').val()
                });
                aoData.push({
                    name: "InvoiceNo",
                    value: $('#filterInvoiceNo').val()
                });
                aoData.push({
                    name: "DeliveryNo",
                    value: $('#filterDeliveryNo').val()
                });
                aoData.push({
                    name: "Outstanding",
                    value: $('#filterOutstanding').val()
                });
            },
            "iDisplayLength": (5),
            "bServerSide": (false),
            "sServerMethod": "POST",
            "sAjaxSource": url,
            "sAjaxDataProp": (""),
            "aaSorting": [[0, "asc"], [1, "asc"]],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                var _this = $(nRow);
                var btnPayment = _this.children('td').children('i[data-action="payment"]');
                
                btnPayment.off();
                btnPayment.on('click', function () {
                    if(aData["BalancePayment"] > 0) {
                        showFormDetail();
                        clearInput('panelForm');
                        
                        $('#PaymentMode, #PaymentAmount, #Remark').val('');
                        
                        aData["ReceiptNo"] = aData["OrderNo"];
                        aData["InstallmentShowed"] = accounting.formatMoney(aData["BalancePayment"], { symbol: '$', format: '%s %v' });
                        populateData(aData);
                        
                        $('#DeliveryDate').val(moment(new Date(aData.DeliveryDate)).format("DD MMM YYYY"));
                        console.log(aData);
                    }
                    else {
                        showNotification("There is nothing to paid with."); 
                    }
                });
            },
            "bAutoWidth": false,
            "bFilter": true,
            "bProcessing": true,
            "bRedraw": true,
            "fnDrawCallback": function(oSettings) {
            }
        });  
    }
    
    function clearFilter() {
        reloadGrid('gridUpdatePaymentBalance');
    }
    
    function reloadGrid(gridName) {
        $('#' + gridName).dataTable().fnDestroy();
        initGrid();  
    }
});