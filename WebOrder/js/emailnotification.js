/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />

$(function () {
    $("#divEmailNotifModify").hide();

    ko.applyBindings(new EmailNotifViewModel(), document.getElementById("dynamicContent"));

    function EmailNotification(data) {
        var self = this;

        self.EmailAddress = data.EmailAddress;
        self.Name = ko.observable(data.Name);
        self.ModuleX = ko.observable(data.ModuleX);
        self.Deleted = ko.observable(data.Deleted);

        self.Active = ko.computed({
            read: function () {
                return !self.Deleted();
            },
            write: function (value) {
                self.Deleted(!value);
            }
        });

        self.IsNew = ko.observable(data.IsNew);
    };

    // Knockout.js ViewModel
    function EmailNotifViewModel() {
        var self = this;

        self.savedFilter = null;

        self.textEmail = ko.observable("");
        self.textName = ko.observable("");
        self.textModule = ko.observable("");
        self.checkActive = ko.observable(true);
        self.emailNotifications = ko.observableArray();
        self.currentEmailNotif = ko.observable();
        self.showLoadMore = ko.observable(false);
        self.sortKey = ko.observable({ columnName: "EmailAddress", asc: true });

        self.loadData = function () {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                filter = {
                    emailaddress: self.textEmail(),
                    name: self.textName(),
                    modulename: self.textModule(),
                    active: self.checkActive()
                };
                self.savedFilter = filter;
            };

            DAL.fnGetEmailNotificationList(JSON.stringify(filter), self.emailNotifications().length, settings.numOfRecords, self.sortKey().columnName, self.sortKey().asc, function (data) {
                for (var i = 0; i < data.records.length; i++) {
                    self.emailNotifications.push(new EmailNotification(data.records[i]));
                };

                if (data.totalRecords > self.emailNotifications().length) {
                    self.showLoadMore(true);
                }
                else {
                    self.showLoadMore(false);
                };
            });
        };

        self.searchEmailNotif = function () {
            self.emailNotifications.removeAll();
            self.showLoadMore(false);
            self.savedFilter = null;
            self.loadData();
            self.textEmail("");
            self.textName("");
            self.textModule("");
        };

        self.openAddNewPage = function () {
            // Initialize new Email Notification
            var notif = {
                Deleted: false,
                IsNew: true
            };

            var newEmailNotif = new EmailNotification(notif);
            self.currentEmailNotif(newEmailNotif);

            $("#divEmailNotifList").hide();
            $("#divEmailNotifModify").fadeIn();
        };

        self.openUpdatePage = function (notif) {
            DAL.fnGetEmailNotification(notif.EmailAddress, function (result) {
                if (result.status == "") {
                    // Create a EmailNotification object to work with.
                    var emailNotif = new EmailNotification(result.EmailNotification);

                    $("#divEmailNotifList").hide();
                    $("#divEmailNotifModify").fadeIn();

                    self.currentEmailNotif(emailNotif);
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.backToList = function () {
            $("#divEmailNotifModify").hide();
            $("#divEmailNotifList").fadeIn();
        };

        self.saveEmailNotif = function () {
            BootstrapDialog.confirm("Are you sure you want to save this record?", function (ok) {
                if (ok) {
                    DAL.fnSaveEmailNotification(JSON.parse(ko.toJSON(self.currentEmailNotif)), sessionStorage.username, function (result) {
                        if (result.status == "") {
                            self.currentEmailNotif(new EmailNotification(result.EmailNotification));

                            // Look for the same item in array and update (replace) it with current item that we're working with
                            var match = ko.utils.arrayFirst(self.emailNotifications(), function (item) {
                                return item.EmailAddress == self.currentEmailNotif().EmailAddress;
                            });
                            if (match)
                                self.emailNotifications.replace(match, self.currentEmailNotif());
                            else
                                self.emailNotifications.push(self.currentEmailNotif());

                            self.emailNotifications.sort(function (a, b) {
                                var sortDirection = (self.sortKey().asc) ? 1 : -1;
                                var valueA = ko.utils.unwrapObservable(a[self.sortKey().columnName]).toLowerCase();
                                var valueB = ko.utils.unwrapObservable(b[self.sortKey().columnName]).toLowerCase();
                                return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
                            });

                            self.backToList();
                        }
                        else {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                }
            });
        };

        self.deleteEmailNotif = function () {
            BootstrapDialog.confirm("Are you sure you want to delete this record?", function (ok) {
                if (ok) {
                    DAL.fnDeleteEmailNotification(self.currentEmailNotif().EmailAddress, function (result) {
                        if (result.status == "") {
                            // Look for the same item in array and remove it
                            var match = ko.utils.arrayFirst(self.emailNotifications(), function (item) {
                                return item.EmailAddress == self.currentEmailNotif().EmailAddress;
                            });
                            self.emailNotifications.remove(match);
                            BootstrapDialog.alert("Record has been deleted.");
                            self.backToList();
                        }
                        else {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                };
            });
        };

        self.sortGrid = function (sortKey) {
            var currSortKey = self.sortKey();
            if (currSortKey.columnName == sortKey) {
                currSortKey.asc = !currSortKey.asc;
            }
            else {
                currSortKey.columnName = sortKey;
                currSortKey.asc = true;
            };
            self.sortKey(currSortKey);
            self.searchEmailNotif();
        };


        self.loadData();

    };

    $("form").submit(function (e) {
        e.preventDefault();
    });

});

