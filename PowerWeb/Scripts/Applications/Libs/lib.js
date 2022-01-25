function clearErrorValidation() {
    $('.parsley-required').html('');
}

function loadDropdownDataSource(elementName, data, selectOneText) {
    var element = $('#' + elementName);
    var htmlOptions = '';
    
    element.html('');
    htmlOptions += '<option value="">' + (selectOneText || '-- Select One --') + '</option>';
    
    $.each(data || [], function(key, val) {
          htmlOptions += '<option value="' + val.Value + '">' + val.Text + '</option>';    
    });   
    
    element.html(htmlOptions);     
}

function formatInto2Digit(inputValue) {
    return ('0' + inputValue).slice(-2);
}

function toTitleCase(str)
{
    return str.replace(/\w\S*/g, function(txt){return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();});
}

function showLoading() {
    if(options.beforeSend && typeof options.beforeSend == 'function') {
        options.beforeSend();    
    }   
    
    var htmlOverlay = '';
    htmlOverlay += '<div class="loading-overlay-outer">';
    htmlOverlay += '<div class="loading-overlay-inner">';
    htmlOverlay += '<i class="fa fa-spin fa-spinner"></i>';
    htmlOverlay += '</div>';
    htmlOverlay += '</div>';
    
    $('.loading-overlay-outer').remove();
    $('body').append(htmlOverlay);
    $('.loading-overlay-outer').fadeIn(); 
}

function hideLoading() {
    $('.loading-overlay-outer').fadeOut(function() {
        $('.loading-overlay-outer').remove();
    });
}
    
function ajaxPost(options) {
    $.ajax({
        url: options.url,
        type: (options.type || 'POST'),
        data: (options.data || {}),
        beforeSend: function() {
            if(options.beforeSend && typeof options.beforeSend == 'function') {
                options.beforeSend();    
            }   
            
            var htmlOverlay = '';
            htmlOverlay += '<div class="loading-overlay-outer">';
            htmlOverlay += '<div class="loading-overlay-inner">';
            htmlOverlay += '<i class="fa fa-spin fa-spinner"></i>';
            htmlOverlay += '</div>';
            htmlOverlay += '</div>';
            
            $('.loading-overlay-outer').remove();
            $('body').append(htmlOverlay);
            $('.loading-overlay-outer').fadeIn(); 
        },
        success: function(result) {
            if(options.success && typeof options.success == 'function') {
                options.success(result);
            }
        },
        error: function() {
            if(options.error && typeof options.error == 'function') {
                options.error();
            }
        },
        complete: function() {
            if(options.complete && typeof options.complete == 'function') {
                options.complete();
            }
            
            $('.loading-overlay-outer').fadeOut(function() {
                $('.loading-overlay-outer').remove();
            });
        }
    });
}

function clearInput(panelName) {
    var selector = '';
    
    if(panelName) {
        selector += '#' + panelName + ' ';
    }
    
    $(selector + 'input').val('');
    $(selector + 'select').val('');
    $(selector + 'textarea').val('');
    $(selector + 'input[type="checkbox"]').prop('checked', false);
}
    
function populateData(data, arg1) {
    var iterator = 1;
    var _this = this;
    _this.iterator = 1;
    var selectorContainer = "";

    if (arguments.length == 2) {
        if (typeof arg1 !== "function") {
            if (_this.isNullOrEmpty(arg1) == false) {
                selectorContainer += arg1 + " ";
            }
        }
        else {
            arg1(data);
        }
    }
    else if (arguments.length == 3) {
        if (_this.isNullOrEmpty(arg1) == false) {
            selectorContainer += ".main form " + arg1 + " ";
        }

        arg2(data);
    }

    $.each(data, function (key, value) {
        var $ctrl = $("[name=" + key + "]");
        var type = $ctrl.data("type");
        $ctrl.removeClass("error");

        if (type === undefined) {
            $(selectorContainer + "[name=\"" + key + "\"]").val(value);
        }
        else {
            if (type === "switch") {
                value = (value || false);
                $(selectorContainer + "#" + key + "Y").prop('checked', value).val(value);
                $(selectorContainer + "#" + key + "N").prop('checked', !value).val(value);
            }
            if (type === "datepicker" || type === "date") {
                value = (value) ? moment(value).format(SimDms.dateFormat) : undefined;
                $(selectorContainer + "[name=\"" + key + "\"]").val(value);
            }
            if (type === "decimal") {
                value = (value) ? _this.numberFormat(value, 2) : 0.00;
                $(selectorContainer + "[name=\"" + key + "\"]").data("kendoNumericTextBox").value(value);
            }
            if (type === "int") {
                value = (value) ? _this.numberFormat(value, 0) : 0;
                $(selectorContainer + "[name=\"" + key + "\"]").data("kendoNumericTextBox").value(value);
            }
            if (type === "select") {
                $(selectorContainer + "[name=\"" + key + "\"]").val(value);
            }
        }
    });

    if (arg1 !== undefined && typeof arg1 === "function") {
        arg1(data);
    }
}
    
$.fn.serializeObject = function () {
    var row = {};
    var oList = $(this).find('input[type="hidden"],input[type="text"],input[type="password"],input[type="checkbox"]:checked,input[type="radio"]:checked,select,textarea');

    oList.each(function () {
        console.log();
    
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

function showNotification(message) {
    var panelNotification = '';
    
    panelNotification += '<div class="notification-overlay-outer" data-notification="true">';
    panelNotification += '<div class="notification-overlay-inner">';
        panelNotification += '<div class="message">' + (message || '') + '</div>';
        panelNotification += '<div class="panel-button">';
        panelNotification += '<button class="button" id="btnNotificationOK">OK</button>';
        panelNotification += '</div>';
    panelNotification += '</div>';
    panelNotification += '</div>';
    
    $('.notification-overlay-outer').remove();
    $('body').append(panelNotification);
    $('.notification-overlay-outer').fadeIn();
    $('#btnNotificationOK').off();
    $('#btnNotificationOK').on('click', function(evt) {
        evt.preventDefault();
        
        $('.notification-overlay-outer').fadeOut(function() {
            $('.notification-overlay-outer').remove();
        });
    });
}  