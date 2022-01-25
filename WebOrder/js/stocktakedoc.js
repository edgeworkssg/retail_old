/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />

$(function () {
    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;

    $("#divStockTakeDocDetail").hide();

    DAL.fnGetInventoryLocationList(function (locations) {
        locations.unshift({ InventoryLocationID: "0", InventoryLocationName: "ALL" });
        ko.applyBindings(new StockTakeDocViewModel(locations), document.getElementById("dynamicContent"));
    });

    function StockTakeDoc(data) {
        var self = this;
        
        self.StockTakeDocRefNo = data.StockTakeDocRefNo;
        self.StockTakeDocDate = new Date(parseInt(data.StockTakeDocDate.replace("/Date(", "").replace(")/", "")));
        self.StockTakeType = data.StockTakeType;
        self.InventoryLocationID = data.InventoryLocationID;
        self.InventoryLocationName = data.InventoryLocation.InventoryLocationName;
        self.Status = data.Status;

        self.StockTakeDocDateFormatted = ko.computed(function () {
            return new Date(self.StockTakeDocDate).toString(dateDisplayFormatWithTime);
        });
    };

    function StockTake(data) {
        var self = this;

        self.ItemNo = data.ItemNo;
        self.ItemName = data.ItemName;
        self.StockTakeQty = data.StockTakeQty;
        self.FixedQty = data.FixedQty;
        self.LooseQty = data.LooseQty;
        self.UOM = data.UOM;
        self.BaseLevel = data.BaseLevel;
        self.Remark = data.Remark;
        self.TakenBy = data.TakenBy;
        self.VerifiedBy = data.VerifiedBy;
    };

    // Knockout.js ViewModel
    function StockTakeDocViewModel(locations) {
        var self = this;

        self.savedFilter = null;

        self.searchDocNumber = ko.observable("");
        self.stockTakeDocs = ko.observableArray();
        self.stockTakes = ko.observableArray();
        self.currentDoc = ko.observable();
        self.showLoadMore = ko.observable(false);
        self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.inventoryLocationList = ko.observableArray(locations);
        self.inventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function (item) {
            return item.InventoryLocationID == (self.allowChangeClinic ? 0 : sessionStorage.locationID);
        }));
        self.inventoryLocationFilterText = ko.observable(self.inventoryLocation().InventoryLocationName);
        self.sortKey = ko.observable({ columnName: "StockTakeDocDate", asc: false });
        self.sortKeyDetail = ko.observable({ columnName: "ItemNo", asc: true });

        self.loadData = function () {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                filter = {
                    stocktakedocrefno: self.searchDocNumber(),
                    inventorylocationid: self.inventoryLocation().InventoryLocationID
                };
                self.savedFilter = filter;
            };

            DAL.fnGetStockTakeDocList(JSON.stringify(filter), self.stockTakeDocs().length, settings.numOfRecords, self.sortKey().columnName, self.sortKey().asc, function (data) {
                for (var i = 0; i < data.records.length; i++) {
                    self.stockTakeDocs.push(new StockTakeDoc(data.records[i]));
                };

                if (data.totalRecords > self.stockTakeDocs().length) {
                    self.showLoadMore(true);
                }
                else {
                    self.showLoadMore(false);
                };
            });
        };

        self.searchDoc = function () {
            // Validate Clinic
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

            self.stockTakeDocs.removeAll();
            self.showLoadMore(false);
            self.savedFilter = null;
            self.loadData();
            self.searchDocNumber("");
        };

        self.newDoc = function (type) {
            // Validate Clinic
            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function (item) {
                return item.InventoryLocationName.toUpperCase() == self.inventoryLocationFilterText().toUpperCase() &&
                       item.InventoryLocationName.toUpperCase() != "ALL";
            });
            if (match) {
                self.inventoryLocation(match);
                self.inventoryLocationFilterText(self.inventoryLocation().InventoryLocationName);
            }
            else {
                BootstrapDialog.alert("Please select a valid outlet.");
                return;
            };

            // Initialize new Doc
            var doc = {
                StockTakeDocDate: new Date().toString(dateValueFormat),
                StockTakeType: type,
                InventoryLocationID: self.inventoryLocation().InventoryLocationID,
                Status: 'Pending'
            };

            DAL.fnSaveStockTakeDoc(doc, function (result) {
                if (result.status == "") {
                    var newDoc = new StockTakeDoc(result.StockTakeDoc);
                    self.stockTakeDocs.unshift(newDoc);
                    BootstrapDialog.alert("Created new document with Document Number: " + newDoc.StockTakeDocRefNo);
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.openDetailPage = function (stockTakeDoc) {
            DAL.fnGetStockTakeDoc(stockTakeDoc.StockTakeDocRefNo, function (result) {
                if (result.status == "") {
                    // Create a StockTakeDoc object to work with.
                    var doc = new StockTakeDoc(result.StockTakeDoc);

                    $("#divStockTakeDocList").hide();
                    $("#divStockTakeDocDetail").fadeIn();

                    self.currentDoc(doc);
                    self.stockTakes.removeAll();
                    self.loadStockTakes(doc.StockTakeDocRefNo);
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.loadStockTakes = function (StockTakeDocRefNo) {
            DAL.fnGetStockTakeList(StockTakeDocRefNo, function (result) {
                if (result.status == "") {
                    for (var i = 0; i < result.records.length; i++) {
                        var stockTake = {
                            ItemNo: result.records[i].ItemNo,
                            ItemName: result.records[i].ItemName,
                            StockTakeQty: result.records[i].StockTakeQty,
                            FixedQty: result.records[i].FixedQty,
                            LooseQty: result.records[i].LooseQty,
                            UOM: result.records[i].Uom,
                            BaseLevel: result.records[i].BaseLevel,
                            Remark: result.records[i].Remark,
                            TakenBy: result.records[i].TakenBy,
                            VerifiedBy: result.records[i].VerifiedBy
                        };
                        self.stockTakes.push(new StockTake(stockTake));
                    };

                    self.sortKeyDetail({ columnName: "ItemNo", asc: false });
                    self.sortGridDetail('ItemNo');

                    $("#divBackToTop").backToTop({
                        scrollTop: $("#backToFirstProduct").position().top - 70,
                        offset: $("#backToFirstProduct").position().top - 70
                    });
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.backToList = function () {
            $("#divStockTakeDocDetail").hide();
            $("#divStockTakeDocList").fadeIn();
        };

        self.changeInventoryLocation = function (location) {
            self.inventoryLocation(location);
            self.inventoryLocationFilterText(location.InventoryLocationName);
        };

        self.inventoryLocationFiltered = ko.computed(function () {
            if (self.inventoryLocationFilterText() && self.inventoryLocationFilterText().toUpperCase() != "ALL") {
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
            self.searchDoc();
        };

        self.sortGridDetail = function (sortKey) {
            var currSortKey = self.sortKeyDetail();
            if (currSortKey.columnName == sortKey) {
                currSortKey.asc = !currSortKey.asc;
            }
            else {
                currSortKey.columnName = sortKey;
                currSortKey.asc = true;
            };
            self.sortKeyDetail(currSortKey);

            self.stockTakes.sort(function (a, b) {
                var sortDirection = (self.sortKeyDetail().asc) ? 1 : -1;
                var valueA = ko.utils.unwrapObservable(a[self.sortKeyDetail().columnName]);
                var valueB = ko.utils.unwrapObservable(b[self.sortKeyDetail().columnName]);
                return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
            });
        };

        self.print = function (StockTakeDocRefNo) {
        var urlReport = settings.crReportLocation + "?r=StockTakeDocument.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&DocNo=" + StockTakeDocRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
        };

        self.printFromDetails = function () {
            var StockTakeDocRefNo = self.currentDoc().StockTakeDocRefNo;
            var urlReport = settings.crReportLocation + "?r=StockTakeDocumentWData.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&DocNo=" + StockTakeDocRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
            //BootstrapDialog.alert("Printout not available yet.");
        };

        self.loadData();
    };

    $("form").submit(function (e) {
        e.preventDefault();
    });
});