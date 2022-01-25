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
        $('#panelFilterProduct').hide();
        $('#panelUpdateProduct').hide();
        $('#panelAttributeNormal').hide();
        $('#panelAttributeMatrix').hide();
        $('#panelUpdateProductFooter').hide();
        $('#panelButtonMode').hide();
        $('#panelButtonForm').hide();
        
        $('#panelButtonGrid').fadeIn();
        $('#panelGridProduct').fadeIn();
        
        if(isGridReloaded == true) {
            reloadGrid('gridProduct');   
        }
    }
    
    function showForm() {
        $('#panelButtonGrid').hide();
        $('#panelGridProduct').hide();
        $('#panelFilterProduct').hide();
        $('#panelButtonMode').fadeIn();
        $('#panelUpdateProduct').fadeIn();
        $('#panelUpdateProductFooter').fadeIn();
        
        if(product.setting.saveState == 'Update')
        {
            $('#panelButtonMode').hide();
        }
        
        if(product.setting.formState == "Normal")
        {
            $('#panelAttributeNormal').fadeIn();
            $('#panelAttributeMatrix').hide();
        }
        else
        {
            $('#panelAttributeNormal').hide();
            $('#panelAttributeMatrix').fadeIn();
        }
        
        $('#panelButtonForm').fadeIn();
        
        clearInput('panelUpdateProduct');
        clearInput('panelAttributeNormal');
        clearInput('panelAttributeMatrix');
        clearInput('panelUpdateProductFooter');
        
        loadItemAttributes();
        
        var pointsGet = $('#PointsGet');
        var timesGet = $('#TimesGet');
        var breakdownPrice = $('#BreakdownPrice');
        
        pointsGet.attr('disabled', true);
        timesGet.attr('disabled', true);
        breakdownPrice.attr('disabled', true);
    }
    
    function loadDataSource() {
        ajaxPost({
            url: app.setting.baseUrl + 'API/Lookup/CategoryName.ashx',
            success: function(result) {
                loadDropdownDataSource('CategoryName', result, '-- Select One --');
            }
        });
        ajaxPost({
            url: app.setting.baseUrl + 'API/Lookup/GST.ashx',
            success: function(result) {
                loadDropdownDataSource('GST', result, '-- Select One --');
            }
        });
        ajaxPost({
            url: app.setting.baseUrl + 'API/Lookup/ProductType.ashx',
            success: function(result) {
                loadDropdownDataSource('ProductType', result, '-- Select One --');
            }
        });
    }
    
    function initElementState() {
        var pointsGet = $('#PointsGet');
        var timesGet = $('#TimesGet');
        var breakdownPrice = $('#BreakdownPrice');
        var itemNo = $('#ItemNo');
        
        pointsGet.attr('disabled', true);
        timesGet.attr('disabled', true);
        breakdownPrice.attr('disabled', true);
        
        itemNo.attr('disabled', true);
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
        var btnNormal = $('#btnNormal');
        var btnMatrix = $('#btnMatrix');
        var btnAddAtt3 = $('#btnAddAtt3');
        var btnAddAtt4 = $('#btnAddAtt4');
        
        btnAdd.off();
        btnExport.off();
        btnSave.off();
        btnCancel.off();
        btnNormal.off();
        btnMatrix.off();
        dropdownProductType.off();
        btnAddAtt3.off();
        btnAddAtt4.off();
        
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
            product.setting.formState = "Normal";
            showForm();
        });
        btnExport.on('click', function(evt) {
            evt.preventDefault();
            
            var url = app.setting.baseUrl + 'API/Product/ProductSetup/ExportToExcel.ashx?ItemNo=' + $('input[type="search"][aria-controls="gridProduct"]').val();
            window.location = url;
        });
        
        btnSave.on('click', function(evt) {
            evt.preventDefault();
            
            if(product.setting.saveState == 'Add') {
                if(product.setting.formState == "Normal"){
                    addData();
                }else{
                    addDataMatrix();
                }
            }
            else if(product.setting.saveState == 'Update') {
                if(product.setting.formState == "Normal"){
                    UpdateData();
                }else{
                    UpdateDataMatrix();
                }
            }
        });
        
        btnCancel.on('click', function(evt) {
            evt.preventDefault();
            showGrid();
        });
        
        btnNormal.on('click', function(evt){
            evt.preventDefault();
            product.setting.formState = "Normal";
            showForm();
        });
        
        btnMatrix.on('click', function(evt){
            evt.preventDefault();
            product.setting.formState = "Matrix";
            showForm();
        });
        
        btnAddAtt3.on('click', function(evt){
            evt.preventDefault();
            AddAttribute3();
        });
        
        btnAddAtt4.on('click', function(evt){
            evt.preventDefault();
            AddAttribute4();
        });
    }
    
    function reloadGrid(gridName) {
        $('#' + gridName).dataTable().fnDestroy();
        initGrid();  
    }
    
    function UpdateData() {
        var validation = $('form').parsley();
        if(validation.validate()) {
            var url = app.setting.baseUrl + 'API/Product/ProductSetup/Update.ashx';
            var params = $('#panelUpdateProduct').serializeObject();
            
            ajaxPost({
                url: url,
                data: params,
                success: function(result) {
                    if(result.Status == true) {
                        showGrid();
                        reloadGrid('gridProduct');  
                    }
                    showNotification(result.Message);     
                }
            });
        }
    }
    
    function addData() {
        var validation = $('form').parsley();
        if(validation.validate()) {
            var data = $('#panelUpdateProduct').serializeObject();
            data["ProductType"] = $('#ProductType').val();
            data["Attributes1"] = $('#Attributes1').val();
            data["Attributes2"] = $('#Attributes2').val();
            data["Attributes3"] = $('#Attributes3').val();
            data["Attributes4"] = $('#Attributes4').val();
            data["Attributes5"] = $('#Attributes5').val();
            data["Remark"] = $('#Remark').val();
            var url = app.setting.baseUrl + 'API/Product/ProductSetup/Save.ashx';

            ajaxPost({
                url: url,
                type: 'POST',
                data: data,
                success: function(result) {
                    if(result.Status == true) {
                        reloadGrid('gridProduct');  
                        $('#ItemNo').val(result.Data.ItemNo);
                        setTimeout(function() {
                            showGrid();
                        }, 1000);
                    }
                    showNotification(result.Message);
                }
            });            
        }   
    }
    
    function addDataMatrix() {
        var validation = $('form').parsley();
        if(validation.validate()) {
            var data = $('#panelUpdateProduct').serializeObject();
            data["ProductType"] = $('#ProductType').val();
            var att3 = "";
            
            $("[name=MatrixAttributes3]").each(function(){
                if($(this).is(":checked")){
                    att3 = att3 + $(this).val()+",";
                }
            });
            
            var att4 = "";
            
            $("[name=MatrixAttributes4]").each(function(){
                if($(this).is(":checked")){
                    att4 = att4 + $(this).val()+",";
                }
            });
            
            if(att3 != "" && att4 != ""){
                data["Attributes3"] = att3.substring(0, att3.length-1);
                data["Attributes4"] = att4.substring(0, att4.length-1);
                data["Remark"] = $('#Remark').val();
                var url = app.setting.baseUrl + 'API/Product/ProductSetup/SaveMatrix.ashx';

                ajaxPost({
                    url: url,
                    type: 'POST',
                    data: data,
                    success: function(result) {
                        if(result.Status == true) {
                            reloadGrid('gridProduct');  
                            $('#ItemNo').val(result.Data.ItemNo);
                            setTimeout(function() {
                                showGrid();
                            }, 1000);
                        }
                        showNotification(result.Message);
                    }
                });        
            }else{
                showNotification("Please select at least one Attribute 3 and Attributes 4.");
            }
                
        }   
    }
    
    function UpdateDataMatrix() {
        var validation = $('form').parsley();
        if(validation.validate()) {
            var data = $('#panelUpdateProduct').serializeObject();
            data["ProductType"] = $('#ProductType').val();
            var att3 = "";
            
            $("[name=MatrixAttributes3]").each(function(){
                if($(this).is(":checked")){
                    att3 = att3 + $(this).val()+",";
                }
            });
            
            var att4 = "";
            
            $("[name=MatrixAttributes4]").each(function(){
                if($(this).is(":checked")){
                    att4 = att4 + $(this).val()+",";
                }
            });
            
            if(att3 != "" && att4 != ""){
                data["Attributes3"] = att3.substring(0, att3.length-1);
                data["Attributes4"] = att4.substring(0, att4.length-1);
                data["Remark"] = $('#Remark').val();
                var url = app.setting.baseUrl + 'API/Product/ProductSetup/UpdateMatrix.ashx';

                ajaxPost({
                    url: url,
                    type: 'POST',
                    data: data,
                    success: function(result) {
                        if(result.Status == true) {
                            reloadGrid('gridProduct');  
                            $('#ItemNo').val(result.Data.ItemNo);
                            setTimeout(function() {
                                showGrid();
                            }, 1000);
                        }
                        showNotification(result.Message);
                    }
                });        
            }else{
                showNotification("Please select at least one Attributes 3 and Attributes 4.");
            }
                
        }   
    }
    
    function deleteData(data) {
        var confirmation = confirm('Do you want to delete this data?');
        var validation = $('#formUpdateProduct');
        var url = app.setting.baseUrl + 'API/Product/ProductSetup/Delete.ashx';
        
        if(confirmation == true) {
            ajaxPost({
                url: url,
                data: {
                    ItemNo: data.ItemNo,
                    Userflag1: data.Userflag1
                },
                type: 'GET',
                success: function(result) {
                    if(result.Status == true) {
                        reloadGrid('gridProduct');
                    }
                    
                    showNotification(result.Message);
                }
            });
        }    
    }
    
    function initGrid() {
        var gridName = '#gridProduct';
        var gridDataSourceUrl = app.setting.baseUrl + 'API/Product/List.ashx';
        $(gridName).DataTable({
            aoColumns: [
                {
                    sWidth: '120px',
                    sClass: 'text-center',
                    mData: null,
                    mRender: function () {
                        return '<i data-action="edit" class="fa fa-edit table-button"> Edit</i>' +
                               '&nbsp;&nbsp;&nbsp;' +
                               '<i data-action="delete" class="fa fa-trash-o table-button"> Delete</i>';
                    }
                },
                { mData: 'ItemNo', width: '65px' },
                { mData: 'ItemName', width: '250px' },
                { mData: 'DepartmentName', width: '75px' },
                { mData: 'CategoryName', width: '100px' },
                { mData: 'RetailPrice', width: '100px', mRender: function(a, b, c) { return '$ ' + (a || 0); } },
                { mData: 'FactoryPrice', width: '100px', mRender: function(a, b, c) { return '$ ' + (a || 0); } },
                { mData: 'IsInInventory', width: '100px' },
                { mData: 'IsNonDiscountable', width: '120px' },
                { mData: 'Barcode', width: '75px' },
            ],
            "aLengthMenu": [
                [5, 10, 20, 50, 100, 150, -1],
                [5, 10, 20, 50, 100, 150, "All"]
            ],
            "fnServerParams": function (aoData) {
                var productFilter = $('#panelFilterProduct').serializeObject();
                $.each(productFilter || [], function (key, val) {
                    aoData.push({
                        name: key,
                        value: val
                    });
                });
            },
            "iDisplayLength": (5),
            "bServerSide": (false),
            "sServerMethod": "POST",
            "sAjaxSource": app.setting.baseUrl + 'API/Product/ProductSetup/List.ashx',
            "sAjaxDataProp": (""),
            "aaSorting": [[0, "asc"], [1, "asc"]],
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
                    
                    var url = app.setting.baseUrl + 'API/Product/ProductSetup/LoadItem.ashx';
                    var params = { ItemNo: aData.ItemNo, Userflag1: aData.Userflag1 };
                    
                    ajaxPost({
                        url: url,
                        data: params,
                        type: 'GET',
                        success: function(result) {
                            if(result.Userflag1 == true)
                            {
                                product.setting.formState = "Matrix";
                            }
                            else
                            {
                                product.setting.formState = "Normal";
                            }
                            showForm();
                            setTimeout(function() {
                                  // wait till attributes loaded
                                   generateDataFormUpdateProduct(result);
                            }, 1000);
                           
                        }
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
    
    function generateDataFormUpdateProduct(data) {
        console.log(data);
        populateData(data);
        
        if(data.Userflag1 == true){
            var array = data.Attributes3.split(",");   
            
            $.each(array,function(i){
               $('[name=MatrixAttributes3]').each(function(){
                    if($(this).val() == array[i]){
                       $(this).prop('checked', true);
                    }
                });
            }); 
            
            var array2 = data.Attributes4.split(",");   
            
            $.each(array2,function(i){
                $('[name=MatrixAttributes4]').each(function(){
                    if($(this).val() == array2[i]){
                       $(this).prop('checked', true);
                    }
                });
            }); 
        }
        
        $('#GST').val((data.GSTRule || 0));
        
        $('#ItemDescription').val(data.ItemDesc || '');
        
        if(data.IsCommission == true) {
            $('#IsCommission').prop('checked', true);
        }
        
        if(data.IsNonDiscountable == true) {
            $('#NonDiscountable').prop('checked', true);
        }
        
        if(data.PointRedeemMode == 'D') {
            $('#PointRedeemable').prop('checked', true);
        }
        
        if(data.IsServiceItem == 'Yes' && data.IsInInventory == 'No') {
            $('#ProductType').val('Service');   
            $('#ProductType').change();
        }
        else if(data.IsServiceItem == 'Yes' && data.IsInInventory == 'Yes') {
            $('#ProductType').val('OpenPriceProduct'); 
            $('#ProductType').change();       
        }
        else if(data.PointGetMode == 'D') {
            $('#ProductType').val('PointPackage'); 
            $('#ProductType').change();
            $('#PointsGet').val((data.PointGetAmount || 0));
        }
        else if(data.PointGetMode == 'T') {
            $('#ProductType').val('CoursePackage');
            $('#ProductType').change();
            $('#TimesGet').val((data.PointGetAmount || 0));
            $('#BreakdownPrice').val((data.Userfloat3 || 0));
        }
        else {
            $('#ProductType').val('Product');  
            $('#ProductType').change();
        }
        
    }
    
    function AddAttribute3(){
        var item = $('#txtAddAtt3').val();
        
         $.ajax({
            type: "POST",
            url: app.setting.baseUrl + "API/Product/ItemAttributes/Add.ashx?Type=Attributes3&&Value=" + item,
            success: function(data) {
               if(data.Status == true){
                    if(item != ""){
                        $('#containerAtt3').append("<input type=\"checkbox\" name=\"MatrixAttributes3\" id=\"MatrixAttributes3\" value=\"" + item +"\"/> " + item + " <br />");
                        $('#txtAddAtt3').val("");
                    }
               }else{
                    showNotification("Error: " + data.Message);
               } 
            },
            error: function(jqXHR, textStatus, errorThrown) {
                showNotification("Error: " + errorThrown);
            }
        });
    }
    
    function AddAttribute4(){
        var item = $('#txtAddAtt4').val();
        
        $.ajax({
            type: "POST",
            url: app.setting.baseUrl + "API/Product/ItemAttributes/Add.ashx?Type=Attributes4&&Value=" + item,
            success: function(data) {
               if(data.Status == true){
                    if(item != ""){
                        $('#containerAtt4').append("<input type=\"checkbox\" name=\"MatrixAttributes4\" id=\"MatrixAttributes4\" value=\"" + item +"\"/> " + item + " <br />");
                        $('#txtAddAtt4').val("");
                    }
               }else{
                    showNotification("Error: " + data.Message);
               } 
            },
            error: function(jqXHR, textStatus, errorThrown) {
                showNotification("Error: " + errorThrown);
            }
        });
    } 
    
    function loadItemAttributes() {
        $.ajax({
            type: "GET",
            url: app.setting.baseUrl + "API/Lookup/ItemAttributes.ashx",
            success: function(data) {
                
                $('#containerAtt3').empty();
                $('#containerAtt4').empty();
                var i = 0;
                for(i=0;i<data.length;i++){
                    var Type = data[i].Type;
                    var Value = data[i].Value;
                    
                    if(Type == 'Attributes3')
                    {
                        $('#containerAtt3').append("<input type=\"checkbox\" name=\"MatrixAttributes3\" id=\"MatrixAttributes3\" value=\"" + Value +"\"/> " + Value + " <br />");
                    }else if(Type == 'Attributes4')
                    {
                         $('#containerAtt4').append("<input type=\"checkbox\" name=\"MatrixAttributes4\" id=\"MatrixAttributes4\" value=\"" + Value +"\"/> " + Value + " <br />");
                    } 
                }
            },
            error: function() {
                //showNotification("The Item Attributes could not be load correctly.");
            }
        });
    }
});

