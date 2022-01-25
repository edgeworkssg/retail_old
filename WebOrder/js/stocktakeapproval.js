/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />
/// <reference path="date.js" />
/// <reference path="setting.js" />

$(function () {
    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;

    DAL.fnGetAllLocationWithOutstandingStockTake(function (result) {
        var locations = new Array();
        if (result.status == "") {
            locations = result.records;
        }
        else {
            BootstrapDialog.alert(result.status);
        };
        ko.applyBindings(new StockTakeViewModel(locations), document.getElementById("dynamicContent"));
    });

    function StockTake(data) {
        var self = this;
        
        self.StockTakeDocRefNo = data.StockTakeDocRefNo;
        self.StockTakeID = data.StockTakeID;
        self.StockTakeDate = new Date(parseInt(data.StockTakeDate.replace("/Date(", "").replace(")/", "")));
        self.TakenBy = data.TakenBy;
        self.VerifiedBy = data.VerifiedBy;
        self.InventoryLocationID = data.InventoryLocationID;
        self.ItemNo = data.ItemNo;
        self.ItemName = data.ItemName;
        self.StockTakeQty = data.StockTakeQty;
        self.SystemBalQty = data.SystemBalQty;
        self.Defi = data.Defi;
        self.FixedQty = data.FixedQty;
        self.LooseQty = data.LooseQty;
        self.UOM = data.Uom;
        self.BaseLevel = data.BaseLevel;
        self.Remark = data.Remark;
        self.Marked = ko.observable(data.Marked);

        self.StockTakeDateFormatted = ko.computed(function () {
            return self.StockTakeDate.toString(dateDisplayFormatWithTime);
        });

        //self.isSelected = ko.computed({
        //    read: function () {
        //        return self.Marked();
        //    },
        //    write: function (value) {
        //        self.Marked(value);
        //    }
        //});

        // UNCOMMENT THIS IF NEED TO UPDATE MARKED COLUMN IN REALTIME
        //self.Marked.subscribe(function (newValue) {
        //    DAL.fnStockTakeUpdateMarked(self.StockTakeID, newValue, function (result) {
        //        if (result.status != "") {
        //            BootstrapDialog.alert(result.status);
        //        };
        //        console.log(self.StockTakeID + " " + newValue.toString());
        //    });
        //});
    };

    // Knockout.js ViewModel
    function StockTakeViewModel(locations) {
        var self = this;

        self.savedFilter = null;

        self.stockTakes = ko.observableArray();
        self.searchDocNo = ko.observable("");
        //self.searchItemName = ko.observable("");
        //self.showLoadMore = ko.observable(false);
        self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.inventoryLocationList = ko.observableArray(locations);
        self.inventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function (item) {
            return item.InventoryLocationID == (self.allowChangeClinic ? -1 : sessionStorage.locationID);
        }));
        self.inventoryLocationFilterText = ko.observable(self.inventoryLocation() ? self.inventoryLocation().InventoryLocationName : "");
        self.sortKey = ko.observable({ columnName: "ItemNo", asc: true });

        self.loadData = function () {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                filter = {
                    stocktakedocrefno: self.searchDocNo(),
                    inventorylocationid: self.inventoryLocation() ? self.inventoryLocation().InventoryLocationID : -1
                    //itemname: self.searchItemName().replace(/\*/g, '%'),
                };
                self.savedFilter = filter;
            };

            DAL.fnGetStockTakeListForApproval(JSON.stringify(filter), function (result) {
                if (result.status == "") {
                    for (var i = 0; i < result.records.length; i++) {
                        self.stockTakes.push(new StockTake(result.records[i]));
                    };
                }
                else {
                    BootstrapDialog.alert(result.status);
                };

                //if (data.totalRecords > self.stockTakes().length) {
                //    self.showLoadMore(true);
                //}
                //else {
                //    self.showLoadMore(false);
                //};
            });
        };

        self.searchStockTake = function () {
            self.stockTakes.removeAll();
            self.savedFilter = null;

            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function (item) {
                return item.InventoryLocationName.toUpperCase() == self.inventoryLocationFilterText().toUpperCase();
            });
            if (match) {
                self.inventoryLocation(match);
                self.inventoryLocationFilterText(self.inventoryLocation().InventoryLocationName);
            }
            else {
                BootstrapDialog.alert("Please select a valid outlet.");
                return;
            };

            if (self.searchDocNo().trim() == "") {
                BootstrapDialog.alert("Please enter a Document Number.");
                return;
            }
            
            //self.showLoadMore(false);
            self.loadData();
            //self.searchItemName("");
        };

        self.submitStockTake = function () {
            BootstrapDialog.confirm("Are you sure you want to APPROVE this Stock Take Document?", function (ok) {
                if (ok) {
                    //var ids = new Array();
                    //for (var i = 0; i < self.stockTakes().length; i++) {
                    //    if (self.stockTakes()[i].isSelected())
                    //        ids.push(self.stockTakes()[i].StockTakeID);
                    //};

                    //DAL.fnApproveStockTake(JSON.stringify(ids), self.inventoryLocation().InventoryLocationID, function (result) {
                    DAL.fnStockTakeApproval(self.savedFilter.stocktakedocrefno, self.savedFilter.inventorylocationid, "Approved", function (result) {
                        if (result.status == "") {
                            BootstrapDialog.alert("Stock Take Document " + self.savedFilter.stocktakedocrefno + " has been approved successfully.");
                            //self.stockTakes.remove(function (item) { return ids.indexOf(item.StockTakeID) >= 0 });
                            self.stockTakes.removeAll();
                            self.savedFilter = null;
                            self.searchDocNo("");
                            self.inventoryLocationFilterText("");
                        }
                        else {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                };
            });
        };

        self.rejectStockTake = function () {
            BootstrapDialog.confirm("Are you sure you want to REJECT this Stock Take Document?", function (ok) {
                if (ok) {
                    DAL.fnStockTakeApproval(self.savedFilter.stocktakedocrefno, self.savedFilter.inventorylocationid, "Rejected", function (result) {
                        if (result.status == "") {
                            BootstrapDialog.alert("Stock Take Document " + self.savedFilter.stocktakedocrefno + " has been rejected successfully.");
                            self.stockTakes.removeAll();
                            self.savedFilter = null;
                            self.searchDocNo("");
                            self.inventoryLocationFilterText("");
                        }
                        else {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                };
            });
        };

        //self.deleteSelectedLine = function () {
        //    BootstrapDialog.confirm("Are you sure you want to delete the selected items?", function (ok) {
        //        if (ok) {
        //            var ids = new Array();
        //            for (var i = 0; i < self.stockTakes().length; i++) {
        //                if (self.stockTakes()[i].isSelected())
        //                    ids.push(self.stockTakes()[i].StockTakeID);
        //            };

        //            DAL.fnDeleteStockTake(JSON.stringify(ids), function (result) {
        //                if (result.status == "") {
        //                    self.stockTakes.remove(function (item) { return ids.indexOf(item.StockTakeID) >= 0 });
        //                }
        //                else {
        //                    BootstrapDialog.alert(result.status);
        //                };
        //            });
        //        };
        //    });
        //};

        self.changeInventoryLocation = function (location) {
            self.inventoryLocation(location);
            self.inventoryLocationFilterText(location.InventoryLocationName);
        };

        //self.isSelectAllLines = ko.computed(function () {
        //    return ko.utils.arrayFilter(self.stockTakes(), function (item) {
        //        return item.isSelected() == true;
        //    }).length == self.stockTakes().length;
        //});

        //self.toggleSelectAllLines = function () {
        //    var toggle = !self.isSelectAllLines();
        //    for (var i = 0; i < self.stockTakes().length; i++) {
        //        self.stockTakes()[i].isSelected(toggle);
        //    }
        //    return true;
        //};

        //self.deleteCheckedBtnState = ko.computed(function () {
        //    return ko.utils.arrayFilter(self.stockTakes(), function (item) {
        //        return item.isSelected() == true;
        //    }).length < 1;
        //});

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
            
            self.stockTakes.sort(function (a, b) {
                var sortDirection = (self.sortKey().asc) ? 1 : -1;
                var valueA = ko.utils.unwrapObservable(a[self.sortKey().columnName]);
                var valueB = ko.utils.unwrapObservable(b[self.sortKey().columnName]);
                return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
            });
        };

        self.inventoryLocationFiltered = ko.computed(function () {
            if (self.inventoryLocationFilterText()) {
                return ko.utils.arrayFilter(self.inventoryLocationList(), function (item) {
                    return item.InventoryLocationName.toUpperCase().indexOf(self.inventoryLocationFilterText().toUpperCase()) == 0;
                });
            }
            else {
                return self.inventoryLocationList();
            };
        });

        self.inventoryLocationFilterTextKeyUp = function (obj, e) {
            if (self.inventoryLocationFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');
        };

        //self.print = function () {
        //    var StockTakeDocRefNo = self.savedFilter.stocktakedocrefno;
        //    var urlReport = settings.crReportLocation + "?r=ReturnPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&DocNo=" + StockTakeDocRefNo;
        //    window.open(urlReport, "_blank", "width=700,height=500,location=0");
        //    //BootstrapDialog.alert("Printout not available yet.");
        //};

    };

    $("#divBackToTop").backToTop({
        scrollTop: $("#backToFirstProduct").position().top - 70,
        offset: $("#backToFirstProduct").position().top - 70
    });

    $("form").submit(function (e) {
        e.preventDefault();
    });

});