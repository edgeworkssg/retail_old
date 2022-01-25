$(document).ready(function() {
    loadDataSource();
    initElementEvents();
       
    function loadDataSource() {
        ajaxPost({
            url: app.setting.baseUrl + 'API/Lookup/InventoryLocation.ashx',
            success: function(result) {
                loadDropdownDataSource('InventoryLocation', result, '-- Select One --');
            }
        });
    }    
    
    function initElementEvents() {
        var btnExport = $('#btnExport');
        btnExport.off();
        btnExport.on('click', function(evt) {
            evt.preventDefault();
            
            var url = app.setting.baseUrl + 'API/Report/Stock/DailyStock/ExportData.ashx';
            url += '?InventoryLocation=' + $('#InventoryLocation').val();
            window.location = url;
        });
    }
});