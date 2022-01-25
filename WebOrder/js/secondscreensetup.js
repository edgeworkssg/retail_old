/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />

$(function () {
    $("#divSecondScreenModify").hide();
    $("#divSecondScreenCopy").hide();
    $("#selPOSFilter").select2();

    DAL.fnGetPointOfSales(function (result) {
        if (result.status == "") {
            var pointofsales = result.records;
            ko.applyBindings(new SecondScreenSetupViewModel(pointofsales), document.getElementById("dynamicContent"));
        }
        else {
            BootstrapDialog.alert(result.status);
        }
    });

    function TheFile(data) {
        var self = this;

        self.FileName = ko.observable(data.FileName);
        self.FileSize = data.FileSize;
        self.FileSizeInKB = data.FileSizeInKB;
        self.FileSizeInMB = data.FileSizeInMB;
        self.FilePath = data.FilePath;
        self.PointOfSaleID = ko.observable(data.PointOfSaleID);
        self.PointOfSaleName = data.PointOfSaleName;

        self.isSelected = ko.observable(false);
    };

    // Knockout.js ViewModel
    function SecondScreenSetupViewModel(pointofsales) {
        var self = this;

        self.savedFilter = null;

        self.pointOfSaleList = ko.observableArray(pointofsales);
        self.pointOfSaleID = ko.observable(sessionStorage.PointOfSaleID);

        self.fileList = ko.observableArray();
        self.currentFile = ko.observable();
        self.showLoadMore = ko.observable(false);
        self.sortKey = ko.observable({ columnName: "FileName", asc: true });
        self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.maxFileSize = 0;

        self.copyFromPointOfSaleID = ko.observable(sessionStorage.PointOfSaleID);
        self.copyToPointOfSaleID = ko.observable(sessionStorage.PointOfSaleID);
        self.allowOverwrite = ko.observable(false);

        self.loadData = function () {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                filter = {
                    pointofsaleid: self.pointOfSaleID()
                };
                self.savedFilter = filter;
            };

            DAL.fnGetSecondScreenFiles(JSON.stringify(filter), self.fileList().length, settings.numOfRecords, self.sortKey().columnName, self.sortKey().asc, function (result) {
                if (result.status == "") {
                    for (var i = 0; i < result.records.length; i++) {
                        self.fileList.push(new TheFile(result.records[i]));
                    };

                    if (result.totalRecords > self.fileList().length) {
                        self.showLoadMore(true);
                    }
                    else {
                        self.showLoadMore(false);
                    };

                    self.maxFileSize = result.maxFileSize;
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.searchFiles = function () {
            self.fileList.removeAll();
            self.showLoadMore(false);
            self.savedFilter = null;
            self.loadData();
        };

        self.openAddNewPage = function () {
            // Initialize new data
            var newFile = new TheFile({ FileName: "" });
            self.currentFile(newFile);

            $("#divSecondScreenList").hide();
            $("#divSecondScreenCopy").hide();
            $("#divSecondScreenModify").fadeIn();
            $("#selPOSModify").select2();
            self.bindFileUpload();
        };

        self.openUpdatePage = function (file) {
            self.currentFile(file);
            $("#divSecondScreenList").hide();
            $("#divSecondScreenCopy").hide();
            $("#divSecondScreenModify").fadeIn();
            $("#selPOSModify").select2();
            $("#btnSave").unbind("click");
            $("#btnSave").click(self.renameFile);
        };

        self.openCopyPage = function () {
            $("#divSecondScreenList").hide();
            $("#divSecondScreenModify").hide();
            $("#divSecondScreenCopy").fadeIn();
            $("#selPOSCopyFrom").select2();
            $("#selPOSCopyTo").select2();
        };

        self.backToList = function () {
            $("#divSecondScreenModify").hide();
            $("#divSecondScreenCopy").hide();
            $("#divSecondScreenList").fadeIn();
        };

        self.renameFile = function () {
            BootstrapDialog.confirm("Are you sure you want to rename this file?", function (ok) {
                if (ok) {
                    DAL.fnRenameSecondScreenFile(JSON.parse(ko.toJSON(self.currentFile)), function (result) {
                        if (result.status == "") {
                            BootstrapDialog.alert("The file has been renamed successfully.");
                            self.backToList();
                            self.searchFiles();
                        }
                        else {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                }
            });
        };

        self.deleteSelectedFiles = function () {
            BootstrapDialog.confirm("Are you sure you want to delete the selected files?", function (ok) {
                if (ok) {
                    var itemsToDelete = ko.utils.arrayFilter(self.fileList(), function (item) {
                        return item.isSelected() == true;
                    });

                    if (itemsToDelete.length > 0) {
                        DAL.fnDeleteSecondScreenFiles(itemsToDelete, function (result) {
                            if (result.status == "") {
                                BootstrapDialog.alert("The files have been deleted successfully.");
                                self.searchFiles();
                            }
                            else {
                                BootstrapDialog.alert(result.status);
                            };
                        });
                    };
                };
            });
        };

        self.copyAllFiles = function () {
            if (self.copyFromPointOfSaleID() == self.copyToPointOfSaleID()) {
                BootstrapDialog.alert("'From' and 'To' outlet cannot be the same.");
                return;
            };
            
            DAL.fnCopySecondScreenFiles(self.copyFromPointOfSaleID(), self.copyToPointOfSaleID(), self.allowOverwrite(), function (result) {
                if (result.status == "") {
                    BootstrapDialog.alert("The files have been copied successfully.");
                    self.copyFromPointOfSaleID(sessionStorage.PointOfSaleID);
                    self.copyToPointOfSaleID(sessionStorage.PointOfSaleID);
                    self.backToList();
                    self.searchFiles();
                }
                else {
                    BootstrapDialog.alert(result.status);
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
            self.searchFiles();
        };

        self.bindFileUpload = function () {
            var url = connection.serverAddress + "/synchronization/Handler/SecondScreenFileUpload.ashx";

            $('#fileupload').fileupload({
                url: url,
                dataType: 'json',
                autoUpload: false,
                add: function (e, data) {
                    $("#fileName").text(data.files ? data.files[0].name : "");
                    $("#btnSave").unbind("click");
                    data.context = $("#btnSave").click(function () {
                        if (data.files[0].size > self.maxFileSize) {
                            BootstrapDialog.alert("File size is too big.");
                            return;
                        }
                        //data.context = $('<p/>').text('Uploading...').replaceAll($(this));
                        data.formData = { pointofsaleid: self.pointOfSaleID() };
                        data.submit();
                    });
                },
                start: function (e) {
                    $('#progress .progress-bar').css('width', '0%');
                },
                done: function (e, data) {
                    if (data.result.status == "") {
                        BootstrapDialog.alert("The file has been uploaded successfully.");
                        self.backToList();
                        self.searchFiles();
                    }
                    else {
                        BootstrapDialog.alert(data.result.status);
                    }
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
        };

        self.isSelectAllFiles = ko.computed(function () {
            return ko.utils.arrayFilter(self.fileList(), function (item) {
                return item.isSelected() == true;
            }).length == self.fileList().length;
        });

        self.toggleSelectAllFiles = function () {
            var toggle = !self.isSelectAllFiles();
            for (var i = 0; i < self.fileList().length; i++) {
                self.fileList()[i].isSelected(toggle);
            }
            return true;
        };

        self.deleteCheckedBtnState = ko.computed(function () {
            return ko.utils.arrayFilter(self.fileList(), function (item) {
                return item.isSelected() == true;
            }).length < 1;
        });

        self.loadData();

    };

    $("form").submit(function (e) {
        e.preventDefault();
    });

});

