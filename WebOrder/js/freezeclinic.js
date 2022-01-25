/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />
/// <reference path="date.js" />
/// <reference path="setting.js" />

$(function () {
    ko.applyBindings(new InventoryLocationViewModel(), document.getElementById("dynamicContent"));

    function InventoryLocation(data) {
        var self = this;

        self.InventoryLocationID = data.InventoryLocationID;
        self.InventoryLocationName = data.InventoryLocationName;
        self.DisplayedName = data.DisplayedName ? data.DisplayedName : "";  // null value is not good for sorting, so convert to empty string
        self.IsFrozen = ko.observable(data.IsFrozen);

        //self.IsFrozen.subscribe(function (newValue) {
        //    console.log(newValue);
        //    DAL.fnUpdateInventoryLocationIsFrozen(self.InventoryLocationID, newValue, function (result) {
        //        if (result.status != "") {
        //            self.IsFrozen(!newValue);
        //            BootstrapDialog.alert(result.status);
        //        };
        //    });
        //});
    };

    // Knockout.js ViewModel
    function InventoryLocationViewModel() {
        var self = this;

        self.inventoryLocations = ko.observableArray();
        self.sortKey = ko.observable({ columnName: "InventoryLocationName", asc: true });

        self.loadData = function () {
            DAL.fnGetInventoryLocationsFromWS(function (result) {
                if (result.status == "") {
                    for (var i = 0; i < result.records.length; i++) {
                        self.inventoryLocations.push(new InventoryLocation(result.records[i]));

                        self.sortKey().asc = !self.sortKey().asc;
                        self.sortGrid(self.sortKey().columnName);
                    };
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.updateIsFrozen = function (inventoryLocation) {
            var newValue = !inventoryLocation.IsFrozen();
            DAL.fnUpdateInventoryLocationIsFrozen(inventoryLocation.InventoryLocationID, newValue, function (result) {
                if (result.status != "") {
                    inventoryLocation.IsFrozen(!newValue);
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.updateIsFrozenAll = function (isFrozen) {
            DAL.fnUpdateInventoryLocationIsFrozen_All(isFrozen, function (result) {
                if (result.status == "") {
                    var msg = "";
                    for (var i = 0; i < result.records.length; i++) {
                        var eachstatus = JSON.parse(result.records[i]).status;
                        if (eachstatus) {
                            msg += eachstatus + "<br />";
                        };
                    };
                    if (msg) BootstrapDialog.alert(msg);
                    self.inventoryLocations.removeAll();
                    self.loadData();
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.isAllFrozen = function () {
            return ko.utils.arrayFilter(self.inventoryLocations(), function (item) {
                return item.IsFrozen() == true;
            }).length == self.inventoryLocations().length;
        };

        self.isAllNotFrozen = function () {
            return ko.utils.arrayFilter(self.inventoryLocations(), function (item) {
                return item.IsFrozen() == false;
            }).length == self.inventoryLocations().length;
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

            self.inventoryLocations.sort(function (a, b) {
                var sortDirection = (self.sortKey().asc) ? 1 : -1;
                var valueA = ko.utils.unwrapObservable(a[self.sortKey().columnName]);
                var valueB = ko.utils.unwrapObservable(b[self.sortKey().columnName]);
                return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
            });
        };

        self.loadData();
    };

    $("form").submit(function (e) {
        e.preventDefault();
    });
});