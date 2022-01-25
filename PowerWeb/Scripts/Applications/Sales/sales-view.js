$(document).ready(function() {
    initElementState();
    initGrid();
    initElementEvents();
    
    function initElementEvents() {
        var btnSearch = $('#btnSearch');
        var btnExport = $('#btnExport');
        
        btnSearch.off();
        btnSearch.on('click', function(evt) {
            evt.preventDefault();
            reloadGrid('gridSales');
        });
        
        btnExport.off();
        btnExport.on('click', function(evt) {
            evt.preventDefault();
            
            var url = app.setting.baseUrl + 'API/Sales/SalesView/Export.ashx?' +
                      'StartDate=' + $('#StartDate').val() + '&' +
                      'EndDate=' + $('#EndDate').val() + '&' +
                      'RefNo=' + $('#RefNo').val() + '&' +
                      'Cashier=' + $('#Cashier').val() + '&' +
                      'Status=' + $('#Status').val() + '&' +
                      'Remark=' + $('#Remark').val() + '&' +
                      'Name=' + $('#Name').val() + '' +
                      '';
            window.location = url;
        });
    }
    
    function initElementState() {
        $('.datepicker').datepicker({
            dateFormat: 'yy-mm-dd'
        });
        $('.datepicker').datepicker('setDate', (new Date()));
        
        $.each($('.btn-date') || [], function(index, obj) {
            var _this = $(obj);
            var dateObject = $('#' + _this.attr('date-object'));
            
            _this.off();
            _this.on('click', function() {
                dateObject.focus();
            });
        });
    }
    
    function reloadGrid(gridName) {
        $('#' + gridName).dataTable().fnDestroy();
        initGrid();  
    }
    
    function initGrid() {
        var gridName = '#gridSales';
        $(gridName).DataTable({
            aoColumns: [
                {
                    sWidth: '70px', sClass: 'align-center', "bSortable": false,
                    mData: null, mRender: function (a, b, c) {
                        return '' +
                               '<button button-type="void" class="button" style="width: 60px; margin-bottom: 0px; font-size: 12px; margin-top: -7px; margin-bottom: -6px; padding: 5px;">Void</button>' +
                               '';                            
                    } 
                },
                {
                    sWidth: '120px', sClass: 'align-center', "bSortable": true,
                    mData: 'orderrefno', mRender: function (a, b, c) {
                        return a;                            
                    } 
                },
                {
                    sWidth: '50px', sClass: 'align-right', "bSortable": false,
                    mData: 'amount', mRender: function (a, b, c) {
                        return accounting.formatNumber(a, 2, "");                            
                    } 
                },
                {
                    sWidth: '100px', sClass: 'align-center', "bSortable": true,
                    mData: 'orderdate', mRender: function (a, b, c) {
                        return moment(new Date(a)).format("DD MMMM YYYY");                           
                    } 
                },
                {
                    sWidth: '250px', sClass: '', "bSortable": true,
                    mData: 'remark', mRender: function (a, b, c) {
                        return a;                            
                    } 
                },
                {
                    sWidth: '100px', sClass: '', "bSortable": true,
                    mData: 'cashierid', mRender: function (a, b, c) {
                        return a;                            
                    } 
                },
                {
                    sWidth: '90px', sClass: '', "bSortable": true,
                    mData: 'paymenttype', mRender: function (a, b, c) {
                        return a;                            
                    } 
                },
                {
                    sWidth: '120px', sClass: '', "bSortable": true,
                    mData: 'membershipno', mRender: function (a, b, c) {
                        return a;                            
                    } 
                },
                {
                    sWidth: '120px', sClass: '', "bSortable": true,
                    mData: 'nametoappear', mRender: function (a, b, c) {
                        return a;                            
                    } 
                },
            ],
            "aLengthMenu": [
                [5, 10, 20, 50, 100, 150, -1],
                [5, 10, 20, 50, 100, 150, "All"]
            ],
            "fnServerParams": function (aoData) {
                var paramsFilter = {
                    StartDate: $('#StartDate').val(),
                    EndDate: $('#EndDate').val(),
                    RefNo: $('#RefNo').val(),
                    Cashier: $('#Cashier').val(),
                    Status: $('#Status').val(),
                    Remark: $('#Remark').val(),
                    Name: $('#Name').val()
                };
                $.each(paramsFilter || [], function (key, val) {
                    aoData.push({
                        name: key,
                        value: val
                    });
                });
            },
            "iDisplayLength": (5),
            "bServerSide": (false),
            "sServerMethod": "POST",
            "sAjaxSource": app.setting.baseUrl + 'API/Sales/SalesView/List.ashx',
            "sAjaxDataProp": (""),
            //"aaSorting": [[1, "asc"], [2, "asc"], [4, "asc"]],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                var _this = $(nRow);
                var btnVoid = _this.children('td').children('button[button-type="void"]');
                
                if(aData.isvoided == true) {
                    _this.addClass('fill-red');
                    _this.children('td.sorting_1').css({ 
                        'background-color': 'red',
                        'color': 'white'
                    });
                    $.each(_this.children('td') || [], function(key, val) {
                        var obj = $(val).children('button');
                        if(obj.length > 0) {
                            obj.attr('disabled', true);
                        }                    
                    });
                }
                else {
                    _this.removeClass('fill-red');
                }
                
                btnVoid.off();
                btnVoid.on('click', function(evt) {
                    evt.preventDefault();
                    
                    $('.background-overlay').off();
                    $('.background-overlay').on('click', function(evt) {
                        //$(this).fadeOut();
                    });
                    $('.background-overlay').fadeIn();
                    $('#RemarkVoid').val('');
                    
                    var btnApplyVoid = $('#btnApplyVoid');
                    var btnCancelVoid = $('#btnCancelVoid');
                    
                    btnApplyVoid.off();
                    btnCancelVoid.off();
                    
                    btnApplyVoid.on('click', function(evt) {
                        evt.preventDefault();
                        
                        var remarkVoid = $('#RemarkVoid').val();
                        
                        if(remarkVoid == '' || remarkVoid == undefined || remarkVoid == null || remarkVoid.length == 0 || remarkVoid.trim() == '') {
                            alert('Remark cannot be null or white space!');
                        }
                        else {
                            var params = {
                                OrderHdrRefNo: aData.orderrefno,
                                Remark: $('#RemarkVoid').val()
                            };                   
                            var url = app.setting.baseUrl + 'API/Sales/SalesView/Void.ashx'
                            
                            ajaxPost({
                                url: url,
                                data: params,
                                beforeSend: function() {
                                    $('.background-overlay').hide();
                                },
                                success: function(result) {
                                    showNotification(result.Message);
                                    
                                    if(result.Status == true) {
                                        reloadGrid('gridSales');
                                    }
                                }
                            });
                        }
                    });
                    
                    btnCancelVoid.on('click', function(evt) {
                        evt.preventDefault();  
                        $('.background-overlay').fadeOut();                 
                    });
                });
            },
            "bAutoWidth": false,
            "bFilter": true,
            "bProcessing": true,
            "bRedraw": true,
            "fnDrawCallback": function(oSettings) {
                if($('#gridProduct').parent().length < 1) {
                }
            }
        });  
    }
});