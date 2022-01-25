$(document).ready(function() {
   initElementsEvent();
   initElementsState();
   loadDataSource();
   
   function initElementsEvent() {
        var btnNew = $('#btnNew');
        var btnSave = $('#btnSave');
        var btnDate = $('.btn-date');
        
        btnNew.off();
        btnSave.off();
        
        btnNew.on('click', function(evt) {
            evt.preventDefault();
    
            btnNew_click();        
        });
        
        btnSave.on('click', function(evt) {
            evt.preventDefault();
            btnSave_click();
        });
        
        $.each(btnDate || [], function(btnDateIndex, btnDateElement) {
            var _this = $(btnDateElement);
            var parentElementName = _this.attr('element');    
            var parentElement = $('#' + parentElementName);
            
            _this.off();
            _this.on('click', function(evt) {
                parentElement.focus();
            });
        });
        
        $('.title-case').focusout(function(evt) {
            var obj = $(this);
            obj.val(toTitleCase(obj.val()));
        });
        $('.email-case').focusout(function(evt) {
            var obj = $(this);
            obj.val(obj.val().toLowerCase());
        });
   }
   function btnNew_click() {
        clearInput('panelForm');
        clearErrorValidation();   
        
        var now = new Date();
        var expiryDate = new Date();
        expiryDate.setFullYear(now.getFullYear() + 100, now.getMonth() + 1, now.getDate());
        
        $('.datepicker').datepicker('setDate', now);  
        $('#ExpiryDate').datepicker('setDate', expiryDate);           
   }
   function btnSave_click() {
        var formValidation = $('#aspnetForm').parsley(); 
        var result = formValidation.validate();  
        var url = app.setting.baseUrl + 'API/Membership/NewMembership/Save.ashx';
        var data = $('#panelForm').serializeObject();
        data['Child1DateOfBirth'] = $('#Child1DateOfBirthYear').val() + '-' + formatInto2Digit($('#Child1DateOfBirthMonth').val()) + '-' + formatInto2Digit($('#Child1DateOfBirthDate').val());
        data['Child2DateOfBirth'] = $('#Child2DateOfBirthYear').val() + '-' + formatInto2Digit($('#Child2DateOfBirthMonth').val()) + '-' + formatInto2Digit($('#Child2DateOfBirthDate').val());
        
        $.each($('#date-wrapper-1 > .parsley-errors-list'), function(index, element) {
            $(element).remove();
        })
        $.each($('#date-wrapper-2 > .parsley-errors-list'), function(index, element) {
            $(element).remove();
        })
        
        if(result) {
            ajaxPost({
                url: url,
                data: data,
                success: function(ajaxResult) {
                    if(ajaxResult.Status == true) {
                        clearInput('panelForm');
                        clearErrorValidation(); 
                        loadDataSource();
                    }
                    
                    showNotification(ajaxResult.Message);
                }
            });   
        }         
   }
   
   function initElementsState() {
        $('.datepicker').datepicker({
            dateFormat: 'yy-mm-dd'
        });
        
        var now = new Date();
        var expiryDate = new Date();
        expiryDate.setFullYear(now.getFullYear() + 1, now.getMonth(), now.getDate());
        $('.datepicker').datepicker('setDate', (new Date()));  
        $('#ExpiryDate').datepicker('setDate', expiryDate);  
   }

    function loadDataSource() {
        var currentDateTime = new Date();
        var currentYear = currentDateTime.getFullYear();
        var currentMonth = currentDateTime.getMonth() + 1;
        var currentDate = currentDateTime.getDate();
        
        var yearList = new Array();
        var monthList = [
                            'January', 'February', 'March', 'April', 
                            'May', 'June', 'July', 'August', 
                            'September', 'October', 'November', 'December' 
                        ];
        for(var i=currentYear; i>= currentYear - 15; i--) {
            yearList.push(i);
        }
        
        var strHtmlYear = "";
        $.each(yearList, function(index, value) {
            strHtmlYear += '<option value="' + value + '">' + value + '</option>';
        });
        $('.datepicker-year').html(strHtmlYear);
        
        var strHtmlMonth = '';
        var i = 1;
        $.each(monthList, function(index, value) {
            strHtmlMonth += '<option value="' + i + '">' + value + '</option>';
            i++;
        });
        $('.datepicker-month').html(strHtmlMonth);
        
        var strHtmlDate = '';
        for(var x=1; x<=31; x++) {
            strHtmlDate += '<option value="' + x + '">' + x + '</option>';
        }
        $('.datepicker-date').html(strHtmlDate);
     
        $('.datepicker-year').val(currentYear);
        $('.datepicker-month').val(currentMonth);
        $('.datepicker-date').val(currentDate);
    }
});