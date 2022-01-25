var product = {};
product.setting = {
    saveState: ''
};

$(document).ready(function() {
    initElementState();
    initElementEvents();
    initGrid();
    renderLayout();
    loadDataSource();
    
    function renderLayout() {
        showGrid();
    }
    
    function showGrid(isGridReloaded) {
        $('#panelFilterCommission').hide();
        $('#panelUpdateCommission').hide();
        $('#panelButtonForm').hide();
        
        $('#panelButtonGrid').fadeIn();
        $('#panelGridCommission').fadeIn();
        
        if(isGridReloaded == true) {
            reloadGrid('gridCommission');   
        }
    }
    
    function showForm() {
        $('#panelButtonGrid').hide();
        $('#panelGridCommission').hide();
        $('#panelFilterCommission').hide();
    
        $('#panelUpdateCommission').fadeIn();
        $('#panelButtonForm').fadeIn();
        
        clearInput('panelUpdateCommission');
    }
    
    function loadDataSource() {
        ajaxPost({
            url: app.setting.baseUrl + 'API/Lookup/CommissionType.ashx',
            success: function(result) {
                loadDropdownDataSource('CommissionType', result, false);
            }
        });
        ajaxPost({
            url: app.setting.baseUrl + 'API/Lookup/SalesGroup.ashx',
            success: function(result) {
                loadDropdownDataSource('SalesGroupID', result, false);
            }
        });
    }
    
    function initElementState() {
        var pointsGet = $('#PointsGet');
        var timesGet = $('#TimesGet');
        var breakdownPrice = $('#BreakdownPrice');
        
        pointsGet.attr('disabled', true);
        timesGet.attr('disabled', true);
        breakdownPrice.attr('disabled', true);
        
        pointsGet.val('');
        timesGet.val('');
        breakdownPrice.val('');
    }  
    
    function initElementEvents() {
        var btnAdd = $('#btnAdd');
        var btnExport = $('#btnExport');
        var btnSave = $('#btnSave');
        var btnCancel = $('#btnCancel');
        var dropdownProductType = $('#ProductType');
        
        btnAdd.off();
        btnExport.off();
        btnSave.off();
        btnCancel.off();
        dropdownProductType.off();
        
        dropdownProductType.on('change', function(evt) {
            initElementState();
            
            var _this = $(this);
            var _value = _this.val();
            
            if(_value == 'PointPackage') {
                $('#PointsGet').removeAttr('disabled');    
            }
            else if(_value == 'CoursePackage') {
                $('#TimesGet').removeAttr('disabled');
                $('#BreakdownPrice').removeAttr('disabled');
            }
        });
        
        btnAdd.on('click', function(evt) {
            evt.preventDefault();
            product.setting.saveState = 'Add';
            showForm();
            clearErrorValidation();
            $('#SalesGroupID').removeAttr('disabled');
            $('#CommissionType').removeAttr('disabled');
        });
        btnExport.on('click', function(evt) {
            evt.preventDefault();
        });
        
        btnSave.on('click', function(evt) {
            evt.preventDefault();
            
            if(product.setting.saveState == 'Add') {
                addData();    
            }
            else if(product.setting.saveState == 'Update') {
                UpdateData();   
            }
        });
        
        btnCancel.on('click', function(evt) {
            evt.preventDefault();
            showGrid();
        });
    }
    
    function reloadGrid(gridName) {
        $('#' + gridName).dataTable().fnDestroy();
        initGrid();  
    }
    
    function UpdateData() {
        var validation = $('#aspnetForm').parsley();
        if(validation.validate()) {
            var url = app.setting.baseUrl + 'API/Commission/CommissionByPercentage/Update.ashx';
            var data = $('#panelUpdateCommission').serializeObject();
            
            ajaxPost({
                url: url,
                type: 'POST',
                data: data,
                success: function(result) {
                    if(result.Status == true) {
                        reloadGrid('gridCommission');  
                        showGrid();
                    }
                    showNotification(result.Message);
                }
            }); 
        }
    }
    
    function addData() {
        var validation = $('#aspnetForm').parsley();
        if(validation.validate()) {
            var data = $('#panelUpdateCommission').serializeObject();
            var url = app.setting.baseUrl + 'API/Commission/CommissionByPercentage/Save.ashx';

            ajaxPost({
                url: url,
                type: 'POST',
                data: data,
                success: function(result) {
                    if(result.Status == true) {
                        reloadGrid('gridCommission');  
                        showGrid();
                    }
                    showNotification(result.Message);
                }
            });            
        }   
    }
    
    function deleteData(data) {
        var confirmation = confirm('Do you want to delete this data?');
        var validation = $('#formUpdateCommission');
        var url = app.setting.baseUrl + 'API/Commission/CommissionByPercentage/Delete.ashx';
        
        if(confirmation == true) {
            ajaxPost({
                url: url,
                data: {
                    UniqueID: data.UniqueID
                },
                type: 'GET',
                success: function(result) {
                    if(result.Status == true) {
                        reloadGrid('gridCommission');
                    }
                    
                    showNotification(result.Message);
                }
            });
        }    
    }
    
    function initGrid() {
        var gridName = '#gridCommission';
        $(gridName).DataTable({
            aoColumns: [
                {
                    sWidth: '80px',
                    sClass: 'align-center',
                    mData: null,
                    mRender: function () {
                        return '<i data-action="edit" class="fa fa-edit table-button"> Edit</i>' +
                               '&nbsp;&nbsp;&nbsp;' +
                               '<i data-action="delete" class="fa fa-trash-o table-button"> Delete</i>';
                    }, "bSortable": false 
                },
                { mData: 'GroupName', width: '120px' },
                { mData: 'CommissionType', width: '120px' },
                { mData: 'LowerLimit', width: '120px', mRender: function(a, b, c) { return accounting.formatMoney(a, '$', 2, '.', ',') }, sClass: 'align-right' },
                { mData: 'UpperLimit', width: '120px', mRender: function(a, b, c) { return accounting.formatMoney(a, '$', 2, '.', ',') }, sClass: 'align-right'  },
                { mData: 'PercentCommission', width: '120px', mRender: function(a, b, c) { return accounting.formatMoney(a, '', 2, '.', ',') }, sClass: 'align-right'  },
//                { mData: 'RetailPrice', width: '100px', mRender: function(a, b, c) { return '$ ' + (a || 0); } },
            ],
            "aLengthMenu": [
                [5, 10, 20, 50, 100, 150, -1],
                [5, 10, 20, 50, 100, 150, "All"]
            ],
            "fnServerParams": function (aoData) {
                var productFilter = $('#panelFilterCommission').serializeObject();
                $.each(productFilter || [], function (key, val) {
                    aoData.push({
                        name: key,
                        value: val
                    });
                });
            },
            "iDisplayLength": (8),
            "bServerSide": (false),
            "sServerMethod": "POST",
            "sAjaxSource": app.setting.baseUrl + 'API/Commission/CommissionByPercentage/List.ashx',
            "sAjaxDataProp": (""),
            "aaSorting": [[1, "asc"]],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                var _this = $(nRow);
                var btnEdit = _this.children('td').children('i[data-action="edit"]');
                var btnDelete = _this.children('td').children('i[data-action="delete"]');
                
                btnDelete.off();
                btnDelete.on('click', function(evt) {
                    evt.preventDefault();
                    deleteData(aData);
                });
                
                btnEdit.off();
                btnEdit.on('click', function () {
                    product.setting.saveState = 'Update';
                  
                    showForm();
                    clearInput('panelUpdateCommission');   
                    generateFormUpdateCommission(aData);
                    $('#SalesGroupID').attr('disabled', 'disabled');
                    $('#CommissionType').attr('disabled', 'disabled');
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
    
    function generateFormUpdateCommission(data) {
        populateData(data);   
    } 
});

