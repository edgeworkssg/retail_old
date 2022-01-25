$(document).ready(function() {
    var tooltipTextCollection = {};
    var idIterator = 0;

    var ItemImporter = {
        DataID: '',
        CheckImportingStatus: false,
        ActionButton: ''
    };
    
    initLayout();
    initElementEvents();
    initElementState();
    loadDataSource();
    
    function initElementEvents() {
        var btnLoad = $('#btnLoad');
        var btnSave = $('#btnSave');
        var fileItem = $('#FileItem');
        
        btnLoad.off();
        btnSave.off();
        fileItem.off();
        
        fileItem.on('change', function(evt) {
            var _this = $(this);
            FileItem_change();
        });
        
        btnLoad.on('click', function(evt) {
            evt.preventDefault();
           
            btnLoad_click(); 
        });
        
        btnSave.on('click', function(evt) {
            evt.preventDefault();
            saveData();
            ItemImporter.ActionButton = 'save';
        });
    }
    
    function loadDataSource() {
        ajaxPost({
            url: app.setting.baseUrl + 'API/Lookup/GST.ashx',
            success: function(result) {
                loadDropdownDataSource('GSTRule', result, '-- Select One --');
                
                setTimeout(function() {
                    //$("#GSTRule > option[value='']").remove();
                
                    ajaxPost({
                        url: app.setting.baseUrl + 'API/Product/ItemImporter/DefaultGSTRule.ashx',
                        success: function(result2) {
                            $('#GSTRule').val(result2);   
                        }
                    });   
                }, 1000);
            }
        });        
    }
    
    function initLayout() {
        $('#panelGridItem').hide();
    }
    
    function initElementState() {
        $('#panelRules').hide();
    }
    
    function btnLoad_click() {
        $('#FileItem').click();    
    }
    
    function FileItem_change(obj) {
        var fileItem = $('#FileItem');
        var files = fileItem[0].files[0];
        
        var formData = new FormData();
        formData.append('FileItem', files);
        formData.append('Sender', 'Zhein');
        
        $.ajax({
            url: app.setting.baseUrl + 'API/Product/ItemImporter/Upload.ashx',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function() {
                showUploadProgress('Uploading file');
            },
            success: function(result) {
                ItemImporter.DataID = result;
                $('#DataID').val(result);
                
                if(result == '-') {
                    showNotification('Excel file is not in correct format. You must use Microsoft Excel 2003/XP format (*.xls) in order to use Item Importer.');
                }
                else {
                    initGrid(result);
                }
            },
            cache: false,
            xhr: function() {
                myXhr = $.ajaxSettings.xhr();
                if(myXhr.upload){
                    myXhr.upload.addEventListener('progress',function(prog) {
                        var uploadProgress = ((prog.loaded / prog.total) * 100);
                        $('.progress-bar-inner').css({
                            'width': uploadProgress + '%'
                        });
                        
                        if(uploadProgress >= 100) {
                            setTimeout(function() {
                                $('.progress-notification-wrapper > .file-name-wrapper').text('Processing file.');   
                            }, 100);
                        }
                    }, false);
                }
                return myXhr;
            },
            complete: function() {
                $('.progress-notification-wrapper').fadeOut(function() {
                    $('.progress-notification-wrapper').remove();
                });
            }
        });
    }   
    
    function showUploadProgress(fileName, progress) {
        var strHtml = '<div class="progress-notification-wrapper">';
        strHtml += '<div class="file-name-wrapper">' + (fileName || 'Uploading') + '</div>';
        strHtml += '<div class="progress-bar-wrapper"><div class="progress-bar-outer"><div class="progress-bar-inner"></div></div></div>';
        strHtml += '<div class="icon-wrapper"><i class="fa fa-spinner fa-spin"></i><div>';
        strHtml += '</div>';
        
        $('.progress-notification-wrapper').remove();
        $('body').append(strHtml);
        $('.progress-notification-wrapper').fadeIn();
    }
    
    function reloadGrid(gridName) {
        $('#' + gridName).dataTable().fnDestroy();
        initGrid();  
    }
    
    function initGrid(dataID) {
        $('#panelRules').fadeIn();
    
        var gridName = '#gridItem';
        $('#panelGridItem').fadeIn();
        $(gridName).dataTable().fnDestroy();
        $(gridName).DataTable({
            aoColumns: [
                {
                    sWidth: '25px',
                    sClass: 'align-center',
                    mData: 'Status',
                    mRender: function (a, b, c) {
                        if(a == null) {
                            return '<img class="icon-check" src="' + (app.setting.baseUrl + 'App_Themes/Applications/Images/check.png') + '" />';
                        }
                        else {
                            return '<img data-action="error" class="icon-error" src="' + (app.setting.baseUrl + 'App_Themes/Applications/Images/error.png') + '" />';
                        }
                    }
                },
                {
                    sWidth: '40px',
                    sClass: 'align-center',
                    mData: null,
                    mRender: function () {
                        return '<i data-action="view" class="fa fa-search table-button"> View</i>';
                    }
                },
                { mData: 'Department', width: '100px' },
                { mData: 'Category', width: '100px' },
                { mData: 'ItemNo', width: '100px' },
                { mData: 'ItemName', width: '200px' },
                { mData: 'Barcode', width: '120px' }, 
                //{ mData: 'GSTRule', width: '120px' } 
            ],
            "aLengthMenu": [
                [5, 10, 20, 50, 100, 150, -1],
                [5, 10, 20, 50, 100, 150, "All"]
            ],
            "fnServerParams": function (aoData) {
                aoData.push({
                    name: "DataID",
                    value: dataID
                });
            },
            "iDisplayLength": (5),
            "bServerSide": (false),
            "sServerMethod": "POST",
            "sAjaxSource": app.setting.baseUrl + 'API/Product/ItemImporter/GetData.ashx',
            "sAjaxDataProp": (""),
            "aaSorting": [[0, "asc"], [1, "asc"]],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                var _this = $(nRow);
                var btnView = _this.children('td').children('i[data-action="view"]');
                var btnError = _this.children('td').children('img[data-action="error"]');
                
                btnView.off();
                btnView.on('click', function () {
                    showDetailsItem(aData);                                                               
                });
                
                btnError.off();
                btnError.on('click', function () {
                });
                
                initTooltips();
            },
            "bAutoWidth": false,
            "bFilter": true,
            "bProcessing": true,
            "bRedraw": true,
            "fnDrawCallback": function(oSettings) {
                initTooltips();
            }
        });  
        
        makeEditable();
    }
    
    function makeEditable() {
        $('#gridItem').dataTable().makeEditable({
            sUpdateURL: app.setting.baseUrl + "API/Product/ItemImporter/UpdateCache.ashx?DataID=" + $('#DataID').val(),
            fnOnUpdated: function(status, data)
            {       
            },
            fnShowError: function (message, action, data) {
                var result = JSON.parse(message);
                console.log(result);
            },
            aoColumns: [
                {},
                {},
                {},
                {},
                {},
                {},
                {},
                {
                    type: 'select',
                    onblur: 'submit',
                    data: "{'Non GST':'Non GST','Inclusive GST':'Inclusive GST','Exclusive GST':'Exclusive GST'}"
                }
            ]
        });
    }
    
    function initGridSave(dataID) {
        var gridName = '#gridItem';
        $('#panelGridItem').fadeIn();
        $(gridName).dataTable().fnDestroy();
        $(gridName).DataTable({
            aoColumns: [
                {
                    sWidth: '25px',
                    sClass: 'align-center',
                    mData: 'Status',
                    mRender: function (a, b, c) {
                        if(a.errorMessage == null || a.errorMessage == "") {
                            return '<img class="icon-check" src="' + (app.setting.baseUrl + 'App_Themes/Applications/Images/check.png') + '" />';
                        }
                        else {
                            return '<img data-id="' + a.elementID + '" data-action="error" class="icon-error tooltips" title="' + a.errorMessage + '" src="' + (app.setting.baseUrl + 'App_Themes/Applications/Images/error.png') + '" />';
                        }
                    }
                },
                {
                    sWidth: '40px',
                    sClass: 'align-center',
                    mData: null,
                    mRender: function () {
                        return '<i data-action="view" class="fa fa-search table-button"> View</i>';
                    }
                },
                { mData: 'Department', width: '100px' },
                { mData: 'Category', width: '100px' },
                { mData: 'ItemNo', width: '100px' },
                { mData: 'ItemName', width: '200px' },
                { mData: 'Barcode', width: '120px' }, 
                //{ mData: 'GSTRule', width: '120px' } 
            ],
            "aLengthMenu": [
                [5, 10, 20, 50, 100, 150, -1],
                [5, 10, 20, 50, 100, 150, "All"]
            ],
            "fnServerParams": function (aoData) {
                aoData.push({
                    name: "DataID",
                    value: dataID
                });
                aoData.push({
                    name: "DefaultGSTRule",
                    value: $('#GSTRule').val()
                });
            },
            "iDisplayLength": (5),
            "bServerSide": (false),
            "sServerMethod": "POST",
            "sAjaxSource": app.setting.baseUrl + 'API/Product/ItemImporter/SaveData.ashx',
            "sAjaxDataProp": (""),
            "aaSorting": [[0, "asc"], [1, "asc"]],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                var _this = $(nRow);
                var btnView = _this.children('td').children('i[data-action="view"]');
                var btnError = _this.children('td').children('img[data-action="error"]');
                
                btnView.off();
                btnView.on('click', function () {
                    showDetailsItem(aData);                                                               
                });
                
                btnError.off();
                btnError.on('click', function () {
                });
                
                initTooltips();
            },
            "bAutoWidth": false,
            "bFilter": true,
            "bProcessing": true,
            "bRedraw": true,
            "fnDrawCallback": function(oSettings) {
                initTooltips();
                
                if(ItemImporter.CheckImportingStatus == false && ItemImporter.ActionButton == 'save') {
                    ItemImporter.CheckImportingStatus = true;
                    ItemImporter.ActionButton = '';
                     
                    ajaxPost({
                        url: app.setting.baseUrl + 'API/Product/ItemImporter/SaveStatus.ashx',
                        type: 'POST',
                        success: function(result) {
                            showNotification(result.Message);
                            ItemImporter.CheckImportingStatus = false;
                        },
                        error: function() {
                            ItemImporter.CheckImportingStatus = false;  
                        },
                        complete: function() {
                            ItemImporter.CheckImportingStatus = false;
                        }
                    });
                }
            },
            "footerCallback": function() {
            }
        });  
        
        makeEditable();
    }
    
    function showDetailsItem(data) {
        var strItem = '<div class="popup-overlay">';
        strItem += '<div class="popup-outer">';
        strItem += '<div class="popup-inner">';
        
        strItem += '<div class="header">';
        strItem += '<div class="title">Item Details</div>';
        strItem += '</div>';
        
        strItem += '<div class="body">';    
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Active</label><div class="details-data">: ' + (data.Active || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Department</label><div class="details-data">: ' + (data.Department || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Category</label><div class="details-data">: ' + (data.Category || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Item No.</label><div class="details-data">: ' + (data.ItemNo || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Item Name</label><div class="details-data">: ' + (data.ItemName || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Barcode</label><div class="details-data">: ' + (data.Barcode || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Retail Price</label><div class="details-data">: ' + (data.RetailPrice || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Cost Price</label><div class="details-data">: ' + (data.CostPrice || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Service Item</label><div class="details-data">: ' + (data.ServiceItem || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Inventory Item</label><div class="details-data">: ' + (data.InventoryItem || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Non Discountable</label><div class="details-data">: ' + (data.NonDiscountable || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Give Commission</label><div class="details-data">: ' + (data.GiveCommission || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Opening Balance</label><div class="details-data">: ' + (data.OpeningBalance || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Attributes 1</label><div class="details-data">: ' + (data.Attributes1 || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Attributes 2</label><div class="details-data">: ' + (data.Attributes2 || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Attributes 3</label><div class="details-data">: ' + (data.Attributes3 || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Attributes 4</label><div class="details-data">: ' + (data.Attributes4 || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="data-wrapper">';
        strItem += '<label class="label-data">Attributes 5</label><div class="details-data">: ' + (data.Attributes5 || '') + '</div>';
        strItem += '</div>';
        
        strItem += '<div class="clear-both" />';
        
        strItem += '</div>';
        
        strItem += '</div>';
        strItem += '</div>';
        strItem += '</div>';
        
        $('.popup-overlay').remove();
        $('body').append(strItem);
        $('.popup-overlay').fadeIn();   
        
        $('.popup-overlay').off();     
        $('.popup-overlay').on('click', function(evt) {
            var _this = $(this);
            _this.fadeOut(function() {
                _this.remove();
            });
        });     
    }                            
    
    function saveData() {
        var url = app.setting.baseUrl + 'API/Product/ItemImporter/SaveData.ashx';
        var params = { DataID: ItemImporter.DataID };
        initGridSave(params.DataID);
    }
    
    function initTooltips() {
        var tooltipsElement = $('.tooltips');
        var _rootThis = this;
        
        $.each(tooltipsElement, function(key, val) {
            var _this = $(this);
            var tooltipText = _this.attr("title");
            var elementId = _this.attr("data-id");
            
            if(tooltipText != null && tooltipText != undefined && tooltipText != "") {
                tooltipTextCollection[elementId] = tooltipText;
            }
            
            if(tooltipTextCollection[elementId] != null && tooltipTextCollection[elementId] != undefined && tooltipTextCollection[elementId] != "") {
                Tipped.create('[data-id="' + elementId + '"]', tooltipText, { position: 'right', size: 'large' });
            }
        });
    }
});
