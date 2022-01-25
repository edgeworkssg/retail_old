/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />

$(function() {


    $('#tableDiscrepancies').freezeTable({
        'freezeColumn': false,
        'freezeHead': true,
        'namespace': 'tableApproved'
    });

    $('#tableTallies').freezeTable({
        'freezeColumn': false,
        'freezeHead': true,
        'namespace': 'tableRejected'
    });

    DAL.fnGetInventoryLocationList(function(locations) {
        //locations.unshift({ InventoryLocationID: "0", InventoryLocationName: "ALL" });
        ko.applyBindings(new GoodsReceiveViewModel(locations), document.getElementById("dynamicContent"));
    });

    function PurchaseOrderDetail(data) {
        var self = this;

        self.PurchaseOrderHeaderRefNo = data.PurchaseOrderHeaderRefNo;
        self.PurchaseOrderDetailRefNo = data.PurchaseOrderDetailRefNo;
        self.CategoryName = data.CategoryName;
        self.ItemNo = data.ItemNo;
        self.ItemName = data.ItemName;
        self.QtyApproved = data.QtyApproved;
        //self.QtyOutstanding = data.QtyOutstanding;
        self.QtyReceived = ko.observable(data.QtyReceived);
        self.Status = data.Status;
        self.UOM = data.UOM;
        self.BaseLevel = data.BaseLevel;
        self.ItemIsDeleted = data.ItemIsDeleted;
        self.FactoryPrice = data.FactoryPrice;

        self.QtyDiff = ko.computed(function() {
            return parseFloat(self.QtyReceived()) - self.QtyApproved;
        });
    };

    // Knockout.js ViewModel
    function GoodsReceiveViewModel(locations) {
        var self = this;

        self.searchOrderNumber = ko.observable("");
        self.purchaseOrderHeaderRefNo = ko.observable("");
        self.POType = ko.observable("");
        self.purchaseOrderDetails = ko.observableArray();
        self.isReadOnly = ko.observable(true);
        self.isEverReceived = ko.observable(false);
        //self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.inventoryLocationList = ko.observableArray(locations);
        self.inventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function(item) {
            //    return item.InventoryLocationID == (self.allowChangeClinic ? 0 : sessionStorage.locationID);
            return item.InventoryLocationID == sessionStorage.locationID;
        }));
        self.inventoryLocationFilterText = ko.observable(self.inventoryLocation().InventoryLocationName);
        self.destInventoryLocation = ko.observable();
        self.sortKeyPODet = ko.observable({ columnName: "ItemNo", asc: true });

        self.selectedItemNo = ko.observable("");

        self.loadData = function() {
            
            self.isReadOnly(true);
            DAL.fnGetItemsToReceive(self.searchOrderNumber(), self.inventoryLocation().InventoryLocationID, function(result) {
                if (result.status == "") {
                    var arrDeleted = new Array();

                    self.purchaseOrderDetails.removeAll();
                    
                    self.purchaseOrderHeaderRefNo(self.searchOrderNumber());
                    self.POType(result.POType);
                    self.inventoryLocation(result.InventoryLocation);
                    self.destInventoryLocation(result.DestInventoryLocation);

                    if (result.POType.toUpperCase() == "ORDER" || result.POType.toUpperCase() == "REPLENISH") {
                        self.inventoryLocationFilterText(result.InventoryLocation.InventoryLocationName);
                    }
                    else {
                        self.inventoryLocationFilterText(result.DestInventoryLocation.InventoryLocationName);
                    };

                    for (var i = 0; i < result.records.length; i++) {
                        var poDet = {
                            PurchaseOrderHeaderRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderHeaderRefNo,
                            PurchaseOrderDetailRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderDetailRefNo,
                            CategoryName: result.records[i].PurchaseOrderDetail.Item.CategoryName,
                            ItemNo: result.records[i].PurchaseOrderDetail.ItemNo,
                            ItemName: result.records[i].PurchaseOrderDetail.Item.ItemName,
                            QtyApproved: result.records[i].PurchaseOrderDetail.QtyApproved,
                            //QtyOutstanding: result.records[i].QtyOutstanding,
                            QtyReceived: result.records[i].QtyReceived,
                            Status: result.records[i].PurchaseOrderDetail.Status,
                            UOM: result.records[i].PurchaseOrderDetail.Item.UOM,
                            BaseLevel: (result.records[i].PurchaseOrderDetail.Item.BaseLevel == "0") ? "1" : result.records[i].PurchaseOrderDetail.Item.BaseLevel,
                            ItemIsDeleted: result.records[i].PurchaseOrderDetail.Item.Deleted,
                            FactoryPrice: result.records[i].PurchaseOrderDetail.FactoryPrice
                        };
                        self.purchaseOrderDetails.push(new PurchaseOrderDetail(poDet));
                        if (poDet.ItemIsDeleted) arrDeleted.push(poDet.ItemNo);
                    };
                    //debugger;
                    if (self.purchaseOrderDetails().length > 0 && result.hasBeenReceived == false) self.isReadOnly(false);

                    if (self.isEverReceived == true) self.isEverReceived(true);

                    if ((self.POType().toUpperCase() == "ORDER" || self.POType().toUpperCase() == "REPLENISH") && arrDeleted.length > 0) {
                        BootstrapDialog.alert("You just imported inactivated Drug. Please check the list");
                    };

                    self.sortKeyPODet({ columnName: "ItemNo", asc: false });
                    self.sortPODet('ItemNo');

                    $('#tableDiscrepancies').freezeTable("update");
                    $('#tableTallies').freezeTable("update");

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

        self.searchPurchaseOrder = function() {
            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function(item) {
                return item.InventoryLocationName.toUpperCase() == self.inventoryLocationFilterText().toUpperCase();
            });
            if (match) {
                self.inventoryLocation(match);
                self.inventoryLocationFilterText(self.inventoryLocation().InventoryLocationName);
            }
            else {
                BootstrapDialog.alert("Please select a valid Outlet.");
                return;
            };

            if (!self.searchOrderNumber()) {
                BootstrapDialog.alert("Please enter a Document Number.");
                return;
            };

            self.purchaseOrderDetails.removeAll();
            self.loadData();
        };

        self.setQtyReceived = function(poDet) {
            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Received Quantity:", poDet.QtyReceived(), function(qty) {
                    if (qty != null && !isNaN(qty)) {
                        if (qty == "") qty = 0;
                        qty = parseFloat(qty);
                        poDet.QtyReceived(qty);
                    };
                });
            };
        };

        self.doStockIn = function() {
            var tmpPODet = ko.utils.arrayFilter(self.purchaseOrderDetails(), function(item) {
                return item.QtyReceived() > 0;
            });

            if (tmpPODet.length < 1) {
                BootstrapDialog.alert("Please input at least one quantity to receive.");
                return;
            };

            BootstrapDialog.confirm("Are you sure you want to submit this Goods Receive?", function(ok) {
                if (ok) {
                    var itemsToReceive = new Array();
                    for (var i = 0; i < tmpPODet.length; i++) {
                        var item = { ItemNo: tmpPODet[i].ItemNo, Quantity: tmpPODet[i].QtyReceived, FactoryPrice: tmpPODet[i].FactoryPrice };
                        itemsToReceive.push(item);
                    }

                    if (self.POType().toUpperCase() == "ORDER" || self.POType().toUpperCase() == "REPLENISH") {
                        DAL.fnStockIn(self.purchaseOrderHeaderRefNo(), JSON.parse(ko.toJSON(itemsToReceive)), sessionStorage.username, -1, self.inventoryLocation().InventoryLocationID, false, true, "", function(result) {
                            if (result.status == "") {
                                BootstrapDialog.alert("Goods have been received successfully.");
                                self.isReadOnly(true);
                            }
                            else {
                                BootstrapDialog.alert(result.status);
                            };
                        });
                    }
                    //                    else if (self.POType().toUpperCase() == "TRANSFER") {
                    //                        DAL.fnStockTransfer(self.purchaseOrderHeaderRefNo(), JSON.parse(ko.toJSON(itemsToReceive)), sessionStorage.username, self.inventoryLocation().InventoryLocationID, self.destInventoryLocation().InventoryLocationID, function(result) {
                    //                            if (result.status == "") {
                    //                                BootstrapDialog.alert("Goods have been transferred successfully.");
                    //                                self.isReadOnly(true);
                    //                            }
                    //                            else {
                    //                                BootstrapDialog.alert(result.status);
                    //                            };
                    //                        });
                    //                    };
                };
            });
        };

        self.changeInventoryLocation = function(location) {
            self.inventoryLocation(location);
            self.inventoryLocationFilterText(location.InventoryLocationName);
        };

        self.allTally = function() {
            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                self.purchaseOrderDetails()[i].QtyReceived(self.purchaseOrderDetails()[i].QtyApproved);
            };
        };

        self.isPODetNotFound = ko.computed(function() {
            return self.purchaseOrderDetails().length < 1;
        });

        self.purchaseOrderDetailsTally = ko.computed(function() {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function(item) {
                return item.QtyDiff() == 0;
            });
        });

        self.purchaseOrderDetailsDiscrepancy = ko.computed(function() {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function(item) {
                return item.QtyDiff() != 0;
            });
        });

        self.sortPODet = function(sortKey) {
            var currSortKey = self.sortKeyPODet();
            if (currSortKey.columnName == sortKey) {
                currSortKey.asc = !currSortKey.asc;
            }
            else {
                currSortKey.columnName = sortKey;
                currSortKey.asc = true;
            };
            self.sortKeyPODet(currSortKey);

            self.purchaseOrderDetails.sort(function(a, b) {
                var sortDirection = (self.sortKeyPODet().asc) ? 1 : -1;
                var valueA = ko.utils.unwrapObservable(a[self.sortKeyPODet().columnName]);
                var valueB = ko.utils.unwrapObservable(b[self.sortKeyPODet().columnName]);
                return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
            });
        };

        self.print = function() {
            var PurchaseOrderHeaderRefNo = self.purchaseOrderHeaderRefNo();
            var urlReport = settings.crReportLocation + "?r=GRPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
            //window.print();
        };

        self.inventoryLocationFiltered = ko.computed(function() {
            if (self.inventoryLocationFilterText() && self.inventoryLocationFilterText().toUpperCase() != "ALL") {
                return ko.utils.arrayFilter(self.inventoryLocationList(), function(item) {
                    return item.InventoryLocationName.toUpperCase().indexOf(self.inventoryLocationFilterText().toUpperCase()) == 0;
                });
            }
            else {
                return self.inventoryLocationList();
            };
        });

        self.inventoryLocationFilterTextKeyUp = function(obj, e) {
            if (self.inventoryLocationFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');
        };

        self.scanProduct = function (data, event) {
            if (event.keyCode != 13) return true;  // will continue to write the letters to textbox
            if (!self.selectedItemNo()) return false;
            if (!self.purchaseOrderHeaderRefNo() || self.purchaseOrderDetails().length <= 0 || self.isReadOnly()) return false;

            var itemNo = self.selectedItemNo().trim().toUpperCase();
            self.selectedItemNo(itemNo);

            DAL.fnGetItemNo(itemNo, function (theResult) {
                if (theResult.status == "") {
                    itemNo = theResult.ItemNo;

                    for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                        if (self.purchaseOrderDetails()[i].ItemNo == itemNo) {
                            var qty = self.purchaseOrderDetails()[i].QtyReceived();

                            if (qty == null || qty == "" || isNaN(self.purchaseOrderDetails()[i].QtyReceived())) qty = 0;
                            qty = parseFloat(qty);
                            self.purchaseOrderDetails()[i].QtyReceived(qty + 1);
                            self.selectedItemNo("");
                            return;
                        }
                    }

                    // ItemNo not found
                    BootstrapDialog.alert("Item Code " + itemNo + " is not found in the list.");
                    self.selectedItemNo("");
                }
                else {
                    BootstrapDialog.alert(theResult.status);
                    self.selectedItemNo("");
                };
            });
        };
    };

    $("form").submit(function(e) {
        e.preventDefault();
    });
});

