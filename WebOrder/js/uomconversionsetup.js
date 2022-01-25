/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />

$(function() {
    $("#divUOMConversionModify").hide();
    $("#selCategoryFilter").select2();
    $("#selItemFilter").select2();
    $("#selUOMFilter").select2();

    DAL.fnGetCategoryList(JSON.stringify({}), function(result) {
        if (result.status == "") {
            var categories = result.records;
            categories.unshift({ CategoryName: "ALL" });
            DAL.fnGetItemListForRecipe(JSON.stringify({ itemName: "ALL" }), false, false, function (result) {
                if (result.status == "") {
                    var items = result.records;
                    items.unshift({ ItemNo: "ALL", ItemName: "ALL" });
                    DAL.fnGetUOMList(function(result) {
                        if (result.status == "") {
                            var uoms = result.records;
                            uoms.unshift("ALL");
                            ko.applyBindings(new UOMConversionSetupViewModel(categories, items, uoms), document.getElementById("dynamicContent"));
                        }
                        else {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        }
        else {
            BootstrapDialog.alert(result.status);
        };
    });

    function UOMConversion(data) {
        var self = this;

        self.ConversionDetID = data.ConversionDetID;
        self.ItemNo = ko.observable(data.ItemNo);
        self.ItemName = data.ItemName;
        self.CategoryName = data.CategoryName;
        self.FromUOM = ko.observable(data.FromUOM);
        self.ToUOM = ko.observable(data.ToUOM);
        self.ConversionRate = ko.observable(data.ConversionRate);
        self.Remark = ko.observable(data.Remark);

        self.isSelected = ko.observable(false);
    };

    // Knockout.js ViewModel
    function UOMConversionSetupViewModel(categories, items, uoms) {
        var self = this;

        self.savedFilter = null;

        self.categoryList = ko.observableArray(categories);
        self.categoryName = ko.observable();
        self.itemList = ko.observableArray(items);
        self.itemNo = ko.observable();
        self.uomList = ko.observableArray(uoms);
        self.uom = ko.observable();

        self.uomConversionList = ko.observableArray();
        self.currentUOMConversion = ko.observable();
        self.showLoadMore = ko.observable(false);
        self.sortKey = ko.observable({ columnName: "ItemNo", asc: true });
        self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.allowToAddData = JSON.parse(sessionStorage.privileges).indexOf("ADD UOM CONVERSION") > -1;
        
        var uomAddEdit = uoms.slice(0);
        uomAddEdit.shift();

        self.loadData = function() {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                filter = {
                    categoryname: self.categoryName(),
                    itemno: self.itemNo(),
                    uom: self.uom()
                };
                self.savedFilter = filter;
            };

            DAL.fnGetUOMConversionList(JSON.stringify(filter), self.uomConversionList().length, settings.numOfRecords, self.sortKey().columnName, self.sortKey().asc, function(result) {
                if (result.status == "") {
                    for (var i = 0; i < result.records.length; i++) {
                        self.uomConversionList.push(new UOMConversion(result.records[i]));
                    };

                    if (result.totalRecords > self.uomConversionList().length) {
                        self.showLoadMore(true);
                    }
                    else {
                        self.showLoadMore(false);
                    };
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.search = function() {
            self.uomConversionList.removeAll();
            self.showLoadMore(false);
            self.savedFilter = null;
            self.loadData();
        };

        self.openAddNewPage = function() {
            // Initialize new data
            var newObj = new UOMConversion({ ItemNo: self.itemNo() });
            self.currentUOMConversion(newObj);

            $("#divUOMConversionList").hide();
            $("#divUOMConversionModify").fadeIn();
            $("#selItemModify").select2();
        };

        self.openUpdatePage = function(obj) {
            self.currentUOMConversion(new UOMConversion(ko.toJS(obj)));
            $("#divUOMConversionList").hide();
            $("#divUOMConversionModify").fadeIn();
            $("#selItemModify").select2();
        };

        self.backToList = function() {
            $("#divUOMConversionModify").hide();
            $("#divUOMConversionList").fadeIn();
        };

        self.save = function() {
            if (self.currentUOMConversion().ItemNo() == "ALL") {
                BootstrapDialog.alert("Please select Item first.");
            }
            else {
                DAL.fnSaveUOMConversion(ko.toJS(self.currentUOMConversion()), function(result) {
                    if (result.status == "") {
                        BootstrapDialog.alert("UOM Conversion successfully saved.");
                        self.backToList();
                        self.search();
                    }
                    else {
                        BootstrapDialog.alert(result.status);
                    };
                });
            };
        };

        self.deleteSelectedFiles = function() {
            BootstrapDialog.confirm("Are you sure you want to delete the selected items?", function(ok) {
                if (ok) {
                    var itemsToDelete = ko.utils.arrayFilter(self.uomConversionList(), function(item) {
                        return item.isSelected() == true;
                    });

                    if (itemsToDelete.length > 0) {
                        DAL.fnDeleteUOMConversion(itemsToDelete, function(result) {
                            if (result.status == "") {
                                BootstrapDialog.alert("The items have been deleted successfully.");
                                self.search();
                            }
                            else {
                                BootstrapDialog.alert(result.status);
                            };
                        });
                    };
                };
            });
        };

        self.sortGrid = function(sortKey) {
            var currSortKey = self.sortKey();
            if (currSortKey.columnName == sortKey) {
                currSortKey.asc = !currSortKey.asc;
            }
            else {
                currSortKey.columnName = sortKey;
                currSortKey.asc = true;
            };
            self.sortKey(currSortKey);
            self.search();
        };

        self.isSelectAll = ko.computed(function() {
            return ko.utils.arrayFilter(self.uomConversionList(), function(item) {
                return item.isSelected() == true;
            }).length == self.uomConversionList().length;
        });

        self.toggleSelectAll = function() {
            var toggle = !self.isSelectAll();
            for (var i = 0; i < self.uomConversionList().length; i++) {
                self.uomConversionList()[i].isSelected(toggle);
            }
            return true;
        };

        self.deleteCheckedBtnState = ko.computed(function() {
            return ko.utils.arrayFilter(self.uomConversionList(), function(item) {
                return item.isSelected() == true;
            }).length < 1;
        });

        self.categoryChange = function() {
            if (self.categoryName() == "ALL") {
                self.itemList(items);
            }
            else {
                self.itemList(ko.utils.arrayFilter(items, function(item) {
                    return item.CategoryName == self.categoryName() || item.ItemNo == "ALL";
                }));
            };
            $("#selItemFilter").val("ALL").trigger("change");
        };

        self.fromUOMFiltered = ko.computed(function() {
            if (self.currentUOMConversion() && self.currentUOMConversion().FromUOM()) {
                return ko.utils.arrayFilter(uomAddEdit, function(item) {
                    return item.toUpperCase().indexOf(self.currentUOMConversion().FromUOM().toUpperCase()) == 0;
                });
            }
            else {
                return uomAddEdit;
            };
        });

        self.fromUOMKeyUp = function(obj, e) {
            if (self.fromUOMFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');
        };

        self.changeFromUOM = function(uom) {
            self.currentUOMConversion().FromUOM(uom);
        };

        self.toUOMFiltered = ko.computed(function() {
            if (self.currentUOMConversion() && self.currentUOMConversion().ToUOM()) {
                return ko.utils.arrayFilter(uomAddEdit, function(item) {
                    return item.toUpperCase().indexOf(self.currentUOMConversion().ToUOM().toUpperCase()) == 0;
                });
            }
            else {
                return uomAddEdit;
            };
        });

        self.toUOMKeyUp = function(obj, e) {
            if (self.toUOMFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');
        };

        self.changeToUOM = function(uom) {
            self.currentUOMConversion().ToUOM(uom);
        };

        self.loadData();

    };

    $("form").submit(function(e) {
        e.preventDefault();
    });

});

