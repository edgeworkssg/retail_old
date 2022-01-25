
function fnGetUrlVars() {
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
        vars[key] = value;
    });
    return vars;
}

function fnGetCurrentDateTime(){
    var n = new Date();
    var day=n.getDay();
    var month=n.getMonth();
    var year=n.getFullYear();
    var hour=n.getHours();
    var min=n.getMinutes();
    var sec=n.getSeconds();
    var date=year+"-"+month+"-"+day+" "+hour+":"+min+":"+sec;
    return date;
}

function fnToday() {
    var startDate = Date.today();
    var endDate = Date.today().addDays(1).addMilliseconds(-1);
    return { start: startDate, end: endDate }
}

function fnYesterday() {
    var startDate = Date.today().addDays(-1);
    var endDate = Date.today().addMilliseconds(-1);
    return { start: startDate, end: endDate }
}

function fnThisWeek() {
    // This week : from Monday to today
    var startDate = (Date.today().is().monday()) ? Date.today() : Date.today().last().monday();
    var endDate = Date.today().addDays(1).addMilliseconds(-1);
    return { start: startDate, end: endDate }
}

function fnLastWeek() {
    // Last week : from last Monday to last Sunday
    var startDate = (Date.today().last().week().is().monday()) ? Date.today().last().week() : Date.today().last().week().monday();
    var endDate = startDate.clone().addDays(7).addMilliseconds(-1);
    return { start: startDate, end: endDate }
}

function fnThisMonth() {
    // This month : from first day of the month to today
    var startDate = Date.today().moveToFirstDayOfMonth();
    var endDate = Date.today().addDays(1).addMilliseconds(-1);
    return { start: startDate, end: endDate }
}

function fnLastMonth() {
    // Last month : from first day to last day of previous month
    var startDate = Date.today().addMonths(-1).moveToFirstDayOfMonth();
    var endDate = startDate.clone().addMonths(1).addMilliseconds(-1);
    return { start: startDate, end: endDate }
}

function fnThisYear() {
    // This year : from first day of this year to today
    var startDate = new Date(new Date().getFullYear(), 0, 1);
    var endDate = Date.today().addDays(1).addMilliseconds(-1);
    return { start: startDate, end: endDate }
}

function fnLastYear() {
    // Last year : from first day to last day of previous year
    var startDate = new Date(new Date().getFullYear() - 1, 0, 1);
    var endDate = new Date(new Date().getFullYear(), 0, 1).addMilliseconds(-1);
    return { start: startDate, end: endDate }
}

function fnInventoryLocationMustBeFrozen(inventoryLocationID, callback) {
    DAL.fnIsInventoryLocationFrozen(inventoryLocationID, function (result) {
        if (result.status == "") {
            if (result.isFrozen) {
                if (callback) callback(true);
            }
            else {
                BootstrapDialog.alert("Warning: This Clinic is NOT frozen. You will not be able to submit this document.");
                if (callback) callback(false);
            };
        }
        else {
            BootstrapDialog.alert(result.status);
            if (callback) callback(false);
        };
    });
}

function fnInventoryLocationMustBeNotFrozen(inventoryLocationID, callback) {
    DAL.fnIsInventoryLocationFrozen(inventoryLocationID, function (result) {
        if (result.status == "") {
            if (!result.isFrozen) {
                if (callback) callback(true);
            }
            else {
                BootstrapDialog.alert("Warning: This Clinic IS frozen. You will not be able to submit this document.");
                if (callback) callback(false);
            };
        }
        else {
            BootstrapDialog.alert(result.status);
            if (callback) callback(false);
        };
    });
}

String.prototype.left = function (count) {
    return this.substring(0, count);
}

if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (str) {
        return this.slice(0, str.length) == str;
    };
}

if (typeof String.prototype.endsWith != 'function') {
    String.prototype.endsWith = function (str) {
        return this.slice(-str.length) == str;
    };
}

// Custom alerts
System = {
    alert: function (text, callback) {
        $("#alert").avgrund({
            height: 120,
            width: 200,
            holderClass: 'custom',
            showClose: true,
            showCloseText: 'X',
            onBlurContainer: 'body',
            setEvent: 'click',
            template: '<p>' + text + '</p>',
            onUnload: callback
        });
        $("#alert").click();
    }
}

jQuery.fn.centerScreen = function () {
    this.css("position", "fixed");
    this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) +
                                                $(window).scrollTop()) + "px");
    this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) +
                                                $(window).scrollLeft()) + "px");
    return this;
}

jQuery.fn.backToTop = function (option) {
    var self = this;
    var offset, duration, scrollTop;

    if (option != null && option.offset != null)
        offset = option.offset;
    else
        offset = 1;

    if (option != null && option.duration != null)
        duration = option.duration;
    else
        duration = 500;

    if (option != null && option.scrollTop != null)
        scrollTop = option.scrollTop;
    else
        scrollTop = 0;

    $(window).off("scroll.backToTop");
    self.off("click.backToTop");

    $(window).on("scroll.backToTop", function () {
        if ($(this).scrollTop() > offset) {
            self.fadeIn(duration);
        }
        else {
            self.fadeOut(duration);
        };
    });

    self.on("click.backToTop", function (event) {
        event.preventDefault();
        $('html, body').animate({ scrollTop: scrollTop }, duration);
        return false;
    });

    return self;
}

$(document).ready(function () {
    if ($("#alert").length == 0) {
        $("body").append("<div id='alert'></div>");
    }
    $("#divLoading").centerScreen();
});

var $loading = $("#divLoading");
$(document)
    .ajaxStart(function () {
        $loading.show();
    })
    .ajaxStop(function () {
        $loading.hide();
    });

// My own addition to show prompt message using BootstrapDialog
BootstrapDialog.prompt = function (message, defaultValue, callback) {
    var $txtbox = $("<input type=\"textbox\" class=\"form-control\" value=\"" + defaultValue + "\" />");
    new BootstrapDialog({
        title: 'Prompt',
        message: function (dialogRef) {
            var $message = $('<div />');
            $message.append(message + "<br />");
            $message.append($txtbox);
            return $message;
        },
        closable: false,
        data: {
            'callback': callback
        },
        buttons: [{
            label: 'Cancel',
            action: function (dialog) {
                typeof dialog.getData('callback') === 'function' && dialog.getData('callback')(null);
                dialog.close();
            }
        }, {
            label: 'OK',
            cssClass: 'btn-primary',
            action: function (dialog) {
                typeof dialog.getData('callback') === 'function' && dialog.getData('callback')($txtbox.val());
                dialog.close();
            }
        }]
    }).open();
};

// My own addition to show prompt message using BootstrapDialog
BootstrapDialog.prompt2 = function(message, rowcount, defaultValue, callback) {
    var $txtbox = $("<textarea rows=\"" + rowcount +"\" class=\"form-control\" value=\"" + defaultValue + "\" />");
    new BootstrapDialog({
        title: 'Prompt',
        message: function(dialogRef) {
            var $message = $('<div />');
            $message.append(message + "<br />");
            $message.append($txtbox);
            return $message;
        },
        closable: false,
        data: {
            'callback': callback
        },
        buttons: [{
            label: 'Cancel',
            action: function(dialog) {
                typeof dialog.getData('callback') === 'function' && dialog.getData('callback')(null);
                dialog.close();
            }
        }, {
            label: 'OK',
            cssClass: 'btn-primary',
            action: function(dialog) {
                typeof dialog.getData('callback') === 'function' && dialog.getData('callback')($txtbox.val());
                dialog.close();
            }
}]
        }).open();
    };

BootstrapDialog.promptDatePicker = function (message, defaultValue, callback) {
    var $datepicker = $("<div />");

    new BootstrapDialog({
        title: 'Prompt',
        message: function (dialogRef) {
            var $message = $('<div />');
            $message.append(message + "<br />");
            $message.append($datepicker);
            $datepicker.datepicker({ format: "d MM yyyy", todayHighlight: true, clearBtn: true });
            if (defaultValue) $datepicker.datepicker('update', new Date(defaultValue));
            return $message;
        },
        closable: false,
        data: {
            'callback': callback
        },
        buttons: [{
            label: 'Cancel',
            action: function (dialog) {
                typeof dialog.getData('callback') === 'function' && dialog.getData('callback')(defaultValue ? new Date(defaultValue) : null);
                dialog.close();
            }
        }, {
            label: 'OK',
            cssClass: 'btn-primary',
            action: function (dialog) {
                var date = ($datepicker.datepicker('getDate') == "Invalid Date") ? null : $datepicker.datepicker('getDate');
                typeof dialog.getData('callback') === 'function' && dialog.getData('callback')(date);
                dialog.close();
            }
        }]
    }).open();
};

function fnLoadDropdownDataSource(elementName, data, selectOneText) {
    var element = $('#' + elementName);
    var htmlOptions = '';
    
    element.html('');
    htmlOptions += '<option value="">' + (selectOneText || '-- Select One --') + '</option>';
    
    $.each(data || [], function(key, val) {
          htmlOptions += '<option value="' + val.Value + '">' + val.Text + '</option>';    
    });   

    element.html(htmlOptions); 
    element.select2();    
}

function fnLoadDropdownAjaxDataSource(options) {
    $.ajax({
        url: connection.serverAddress + '/' + options.url,
        data: (options.data || []),
        beforeSend: function() {
            $('#divLoading').fadeIn();
        },
        success: function(response) {   
            fnLoadDropdownDataSource(options.element, response, options.selectOneText);
            
            if(options.success && typeof options.success == 'function') {
                options.success(response);
            }
        },
        error: function(jqXhr, textStatus, errorThrown) {
            console.log('Error while doing ajax request', jqXhr, textStatus, errorThrown);
        },
        complete: function() {
            $('#divLoading').fadeOut();
        }
    });
}

function fnAjax(options) {
    $.ajax({
        url: connection.serverAddress + '/' + options.url,
        data: (options.data || []),
        type: 'POST',
        beforeSend: function() {
            $('#divLoading').fadeIn();
        },
        success: function(response) {   
            if(options.success && typeof options.success == 'function') {
                options.success(response);
            }
        },
        error: function(jqXhr, textStatus, errorThrown) {
            console.log('Error while doing ajax request', jqXhr, textStatus, errorThrown);
        },
        complete: function() {
            $('#divLoading').fadeOut();
        }
    });   
}

$.fn.serializeObject = function () {
    var row = {};
    var oList = $(this).find('input[type="hidden"],input[type="text"],input[type="password"],input[type="checkbox"]:checked,input[type="radio"]:checked,select,textarea');

    oList.each(function () {
        if (this.name == null || this.name == undefined || this.name == '') return;

        var _val = null;
        var name = this.name;
        var _obj = $(this);

        if ($(this).is('select')) {
            _val = $(this).find('option:selected').val();
        }
        else if ($(this).hasClass("number")) {
            _val = this.value.replace(",", "");
        }
        else if(_obj.is(':checkbox') == true) {
            _val = _obj.prop('checked');            
        }
        else {
            var _type = $(this).data("type") || "";
            switch (_type) {
                case "int":
                    _val = $(this).data("kendoNumericTextBox").value();
                    break;
                case "switch":
                    _val = eval(this.value);
                    break;
                case "decimal":
                    _val = $(this).data("kendoNumericTextBox").value();
                    break;
                case "datepicker":
                    if (this.value.length > 0) {
                        _val = moment(this.value, SimDms.dateFormat).format("YYYY-MM-DD");
                    }
                    else {
                        _val = undefined;
                    }
                    break;
                case "datetimepicker":
                    if (this.value.length > 0) {
                        _val = moment(this.value, SimDms.dateFormat).format("YYYY-MM-DD hh:mmA");
                    }
                    else {
                        _val = undefined;
                    }
                    break;
                default:
                    _val = this.value;
                    break;
            }
        }

        row[this.name] = _val;
    });
    return row;
}

function fnShowMessage(title, message) {
    var strHtml = '<div id="MessageBox" class="modal fade bootstrap-dialog type-primary size-normal in" tabindex="-1" id="ef883bf6-fba8-4b77-9d98-5454cab5ea7c" aria-hidden="false" style="display: block;"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"  style="background-color: #ED9C28;"><div class="bootstrap-dialog-header"><div class="bootstrap-dialog-title">' + title + '</div><div class="bootstrap-dialog-close-button" style="display: none;"><button class="close">×</button></div></div></div><div class="modal-body"><div class="bootstrap-dialog-body"><div class="bootstrap-dialog-message">' + message + '</div></div></div><div class="modal-footer"><div class="bootstrap-dialog-footer"><div class="bootstrap-dialog-footer-buttons"><button class="btn btn-default" data-dismiss="modal" id="55b1e405-d8f4-4fc1-9abf-4bee3b003413">OK</button></div></div></div></div></div></div>'

    $('#MessageBox').remove();
    $('body').append(strHtml);
    $('#MessageBox').modal('show');
}
