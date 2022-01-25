/// <reference path="bootstrap-dialog.min.js" />

$(function () {
    // Change this to the location of your server-side upload handler:
    var url = connection.serverAddress + "/synchronization/ItemImporter.ashx";

    $('#fileupload').fileupload({
        url: url,
        dataType: 'json',
        start: function (e) {
            $('#progress .progress-bar').css('width', '0%');
        },
        done: function (e, data) {
            if (data.result.status == "")
                BootstrapDialog.alert("The file has been imported successfully.");
            else
                BootstrapDialog.alert(data.result.status);
        },
        fail: function (e, data) {
            BootstrapDialog.alert("Error occured: " + data.errorThrown);
        },
        progressall: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#progress .progress-bar').css(
                'width',
                progress + '%'
            );
        }
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');

});