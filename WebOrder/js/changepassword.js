/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />

$(function () {
    ko.applyBindings(new ChangePasswordViewModel(), document.getElementById("dynamicContent"));

    // Knockout.js ViewModel
    function ChangePasswordViewModel() {
        var self = this;

        self.oldPassword = ko.observable("");
        self.newPassword = ko.observable("");
        self.confirmPassword = ko.observable("");

        self.save = function () {
            DAL.fnChangePassword(sessionStorage.username, self.oldPassword(), self.newPassword(), function (result) {
                if (result.status == "") {
                    BootstrapDialog.alert("Password has been changed successfully.");
                    self.oldPassword("");
                    self.newPassword("");
                    self.confirmPassword("");
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };
    };

    $("form").submit(function (e) {
        e.preventDefault();
    });
});