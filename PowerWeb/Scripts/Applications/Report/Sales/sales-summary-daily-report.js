$(document).ready(function() {
    loadDataSource();
    initElementEvents();
    initElementState();  
    function loadDataSource() {
        ajaxPost({
            url: app.setting.baseUrl + 'API/Lookup/Outlet.ashx',
            success: function(result) {
                loadDropdownDataSource('Outlet', result, '-- Select One --');
            }
        });
    }    
    
    function initElementEvents() {        
        var btnExport = $('#btnExport');
        btnExport.off();
        btnExport.on('click', function(evt) {
            evt.preventDefault();
            
            var url = app.setting.baseUrl + 'API/Report/Sales/SalesSummaryDaily/ExportData.ashx';
            url += '?Outlet=' + $('#Outlet').val();
            url += '&SalesDate=' + $('#SalesDate').val();
            window.location = url;
        });
    }
    
    function initElementState() {
        $('.datepicker').datepicker({
            dateFormat: 'yy-mm-dd'
        });
        $('.datepicker').datepicker('setDate', (new Date()));
    }
});