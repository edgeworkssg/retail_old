/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />

$(function () {
    var dateDisplayFormat = settings.dateDisplayFormat;
    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;

    $("#divStockTransferModify").hide();

    DAL.fnGetInventoryLocationList(function (locations) {
        locations.unshift({ InventoryLocationID: "0", InventoryLocationName: "ALL" });
        ko.applyBindings(new StockTransferViewModel(locations), document.getElementById("dynamicContent"));
    });

    function PurchaseOrderHeader(data) {
        var self = this;

        self.PurchaseOrderHeaderRefNo = data.PurchaseOrderHeaderRefNo;
        self.PurchaseOrderDate = new Date(parseInt(data.PurchaseOrderDate.replace("/Date(", "").replace(")/", "")));
        self.Status = ko.observable(data.Status);
        self.Remark = ko.observable(data.Remark);
        self.InventoryLocationID = ko.observable(data.InventoryLocationID);
        self.InventoryLocationName = ko.observable(data.InventoryLocation.InventoryLocationName);
        self.DestInventoryLocationID = ko.observable(data.DestInventoryLocationID);
        self.DestInventoryLocationName = ko.observable(data.DestInventoryLocation.InventoryLocationName);
        self.POType = data.POType;
        self.ReasonID = ko.observable(data.ReasonID);
        self.RequestedBy = data.RequestedBy;

        self.PurchaseOrderDateFormatted = ko.computed(function () {
            return new Date(self.PurchaseOrderDate).toString(dateDisplayFormatWithTime);
        });

        self.PriceLevel = ko.observable(data.PriceLevel);
    };

    function PurchaseOrderDetail(data) {
        var self = this;

        self.PurchaseOrderHeaderRefNo = data.PurchaseOrderHeaderRefNo;
        self.PurchaseOrderDetailRefNo = data.PurchaseOrderDetailRefNo;
        self.ItemNo = data.ItemNo;
        self.ItemName = data.ItemName;
        self.Quantity = ko.observable(data.Quantity);
        self.QtyInStock = data.QtyInStock;
        self.QtyApproved = data.QtyApproved;
        self.UOM = data.UOM;
        self.BaseLevel = data.BaseLevel;
        self.Remark = data.Remark;
        self.Status = data.Status;

        self.isSelected = ko.observable(false);
    };

    function ItemToAdd(data) {
        var self = this;
        self.data = data;
    };

    // Knockout.js ViewModel
    function StockTransferViewModel(locations) {
        var self = this;

        self.statusDropdown = ["ALL", "Pending", "Submitted", "Posted", "Cancelled"];
        self.savedFilter = null;
        self.savedItemFilter = null;

        self.searchOrderNumber = ko.observable("");
        self.purchaseOrderHeaders = ko.observableArray();
        self.purchaseOrderDetails = ko.observableArray();
        self.currentPurchaseOrder = ko.observable();
        self.searchItemResults = ko.observableArray();
        self.selectedItemNo = ko.observable("");
        self.qtyToAdd = ko.observable("");
        self.isReadOnly = ko.observable(false);
        self.showLoadMore = ko.observable(false);
        self.status = ko.observable("Pending");
        self.showLoadMoreItems = ko.observable(false);
        self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.inventoryLocationList = ko.observableArray(locations);
        self.fromInventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function (item) {
            return item.InventoryLocationID == (self.allowChangeClinic ? 0 : sessionStorage.locationID);
        }));
        self.toInventoryLocation = ko.observable(self.inventoryLocationList()[0]);
        self.fromInventoryLocationFilterText = ko.observable(self.fromInventoryLocation().InventoryLocationName);
        self.toInventoryLocationFilterText = ko.observable(self.toInventoryLocation().InventoryLocationName);
        self.sortKeyPOHdr = ko.observable({ columnName: "PurchaseOrderDate", asc: false });
        self.sortKeyPODet = ko.observable({ columnName: "ItemNo", asc: true });
        self.reasonList = ko.observableArray([{ ReasonID: "-1", ReasonName: "-- Select Reason --" }]);
        self.reason = ko.observable(self.reasonList()[0]);
        self.disableFromInventoryLocation = ko.observable(!self.allowChangeClinic && true);
        self.disableToInventoryLocation = ko.observable(false);

        self.showPriceLevel = ko.observable(false);
        self.priceLevel = ko.observable();
        self.selectedPriceLevel = ko.observable("");
        DAL.fnGetSetting("GoodsOrdering_ShowPriceLevelForWebOrder", "yes", function(res) {
            if (res == "True") {
                DAL.fnGetPriceLevel(function (resLevel) {
                    if (resLevel.status == "" && resLevel.records.length > 0) {
                        self.priceLevel(resLevel.records);
                        self.showPriceLevel(true);
                    }
                });
            };
        });

        self.loadData = function () {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                filter = {
                    purchaseorderheaderrefno: self.searchOrderNumber(),
                    inventorylocationid: self.fromInventoryLocation().InventoryLocationID,
                    destinventorylocationid: self.toInventoryLocation().InventoryLocationID,
                    status: self.status(),
                    potype: "Transfer"
                };
                self.savedFilter = filter;
            };
            
            DAL.fnGetPurchaseOrderHeaderList(JSON.stringify(filter), self.purchaseOrderHeaders().length, settings.numOfRecords, self.sortKeyPOHdr().columnName, self.sortKeyPOHdr().asc, function (data) {
                for (var i = 0; i < data.records.length; i++) {
                    self.purchaseOrderHeaders.push(new PurchaseOrderHeader(data.records[i]));
                };

                if (data.totalRecords > self.purchaseOrderHeaders().length) {
                    self.showLoadMore(true);
                }
                else {
                    self.showLoadMore(false);
                };
            });
        };

        self.searchPurchaseOrder = function () {
            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function (item) {
                return item.InventoryLocationName.toUpperCase() == self.fromInventoryLocationFilterText().toUpperCase();
            });
            if (match) {
                self.fromInventoryLocation(match);
                self.fromInventoryLocationFilterText(self.fromInventoryLocation().InventoryLocationName);
            }
            else {
                BootstrapDialog.alert("Please select a valid 'From Outlet'.");
                return;
            };

            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function (item) {
                return item.InventoryLocationName.toUpperCase() == self.toInventoryLocationFilterText().toUpperCase();
            });
            if (match) {
                self.toInventoryLocation(match);
                self.toInventoryLocationFilterText(self.toInventoryLocation().InventoryLocationName);
            }
            else {
                BootstrapDialog.alert("Please select a valid 'To Outlet'.");
                return;
            };

            if (self.fromInventoryLocationFilterText().toUpperCase() != "ALL" && self.fromInventoryLocationFilterText().toUpperCase() == self.toInventoryLocationFilterText().toUpperCase()) {
                BootstrapDialog.alert("'From Outlet' and 'To Outlet' cannot be the same.");
                return;
            };

            self.purchaseOrderHeaders.removeAll();
            self.showLoadMore(false);
            self.savedFilter = null;
            self.loadData();
            self.searchOrderNumber("");
        };

        self.openAddNewPage = function () {
            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function (item) {
                return item.InventoryLocationName.toUpperCase() == self.fromInventoryLocationFilterText().toUpperCase() &&
                       item.InventoryLocationName.toUpperCase() != "ALL";
            });
            if (match) {
                self.fromInventoryLocation(match);
                self.fromInventoryLocationFilterText(self.fromInventoryLocation().InventoryLocationName);
            }
            else {
                BootstrapDialog.alert("Please select a valid 'From Outlet'.");
                return;
            };

            if (!self.allowChangeClinic && self.fromInventoryLocation().InventoryLocationID != sessionStorage.locationID) {
                BootstrapDialog.alert("'From Outlet' must be a Outlet that is registered to your account.");
                return;
            };

            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function (item) {
                return item.InventoryLocationName.toUpperCase() == self.toInventoryLocationFilterText().toUpperCase();
            });
            if (match) {
                self.toInventoryLocation(match);
                self.toInventoryLocationFilterText(self.toInventoryLocation().InventoryLocationName);
            }
            else {
                BootstrapDialog.alert("Please select a valid 'To Outlet'.");
                return;
            };

            if (self.fromInventoryLocationFilterText().toUpperCase() == self.toInventoryLocationFilterText().toUpperCase()) {
                BootstrapDialog.alert("'From Outlet' and 'To Outlet' cannot be the same.");
                return;
            };

            // Initialize new PO Header
            var poHdr = {
                PurchaseOrderDate: new Date().toString(dateValueFormat),
                Status: "Pending",
                Remark: "",
                InventoryLocationID: self.fromInventoryLocation().InventoryLocationID,
                DestInventoryLocationID: self.toInventoryLocation().InventoryLocationID,
                ReasonID: -1,
                POType: "Transfer",
                RequestedBy: sessionStorage.username
            };

            DAL.fnSavePurchaseOrderHeader(poHdr, function (result) {
                if (result.status == "") {
                    var newPOHeader = new PurchaseOrderHeader(result.PurchaseOrderHeader);
                    self.currentPurchaseOrder(newPOHeader);
                    self.purchaseOrderHeaders.unshift(newPOHeader);
                    self.openUpdatePage(newPOHeader);
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });

        };

        self.openUpdatePage = function (order) {
            DAL.fnGetPurchaseOrderHeader(order.PurchaseOrderHeaderRefNo, function (result) {
                if (result.status == "") {
                    // Create a PurchaseOrderHeader object to work with.
                    var orderHdr = new PurchaseOrderHeader(result.PurchaseOrderHeader);

                    $("#divStockTransferList").hide();
                    $("#divStockTransferModify").fadeIn();

                    //if (self.allowChangeClinic) {
                    //    self.isReadOnly(orderHdr.Status() == "Pending" ? false : true);
                    //}
                    //else {
                    //    if (orderHdr.InventoryLocationID() == sessionStorage.locationID)
                    //        self.isReadOnly(orderHdr.Status() == "Pending" ? false : true);
                    //    else
                    //        self.isReadOnly(true);
                    //};

                    if (!self.allowChangeClinic && orderHdr.InventoryLocationID() != sessionStorage.locationID) {
                        self.isReadOnly(true);
                    }
                    else {
                        self.isReadOnly(orderHdr.Status() == "Pending" ? false : true);
                    };
                    
                    self.currentPurchaseOrder(orderHdr);
                    var selectedReason = ko.utils.arrayFirst(self.reasonList(), function (item) {
                        return item.ReasonID == orderHdr.ReasonID();
                    });
                    if (selectedReason) self.reason(selectedReason);

                    self.purchaseOrderDetails.removeAll();
                    self.loadPurchaseOrderDetails(orderHdr.PurchaseOrderHeaderRefNo);

                    if (self.showPriceLevel() && self.priceLevel().length > 0) {
                        var level = ko.utils.arrayFirst(self.priceLevel(), function (item) {
                            return item.Key == orderHdr.PriceLevel();
                        });
                        if (level)
                            self.selectedPriceLevel(level.Value);
                        else
                            self.selectedPriceLevel("");
                    }
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.loadPurchaseOrderDetails = function (PurchaseOrderHeaderRefNo) {
            DAL.fnGetPurchaseOrderDetailList(PurchaseOrderHeaderRefNo, function (result) {
                if (result.status == "") {
                    for (var i = 0; i < result.records.length; i++) {
                        var poDet = {
                            PurchaseOrderHeaderRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderHeaderRefNo,
                            PurchaseOrderDetailRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderDetailRefNo,
                            ItemNo: result.records[i].PurchaseOrderDetail.ItemNo,
                            ItemName: result.records[i].PurchaseOrderDetail.Item.ItemName,
                            Quantity: result.records[i].PurchaseOrderDetail.Quantity,
                            QtyInStock: result.records[i].StockBalance,
                            QtyApproved: result.records[i].PurchaseOrderDetail.QtyApproved,
                            UOM: result.records[i].PurchaseOrderDetail.Item.UOM,
                            BaseLevel: (result.records[i].PurchaseOrderDetail.Item.BaseLevel == 0) ? 1 : result.records[i].PurchaseOrderDetail.Item.BaseLevel,
                            Remark: result.records[i].PurchaseOrderDetail.Remark,
                            Status: result.records[i].PurchaseOrderDetail.Status
                        };
                        self.purchaseOrderDetails.push(new PurchaseOrderDetail(poDet));
                    };

                    self.sortKeyPODet({ columnName: "ItemNo", asc: false });
                    self.sortPODet('ItemNo');

                    $("#divNewProducts").scrollToFixed({ marginTop: 50 });
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

        self.SavePOHdr = function (callback) {
            DAL.fnSavePurchaseOrderHeader(JSON.parse(ko.toJSON(self.currentPurchaseOrder)), function (result) {
                if (result.status == "") {
                    self.currentPurchaseOrder(new PurchaseOrderHeader(result.PurchaseOrderHeader));

                    // Look for the same item in array and update (replace) it with current item that we're working with
                    var match = ko.utils.arrayFirst(self.purchaseOrderHeaders(), function (item) {
                        return item.PurchaseOrderHeaderRefNo == self.currentPurchaseOrder().PurchaseOrderHeaderRefNo;
                    });
                    self.purchaseOrderHeaders.replace(match, self.currentPurchaseOrder());

                    self.purchaseOrderHeaders.sort(function (a, b) {
                        var sortDirection = (self.sortKeyPOHdr().asc) ? 1 : -1;
                        var valueA = ko.utils.unwrapObservable(a[self.sortKeyPOHdr().columnName]);
                        var valueB = ko.utils.unwrapObservable(b[self.sortKeyPOHdr().columnName]);
                        return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
                    });
                    if (callback) callback(true);
                }
                else {
                    BootstrapDialog.alert(result.status);
                    if (callback) callback(false);
                };
            });
        };

        self.backToList = function () {
            $("#divStockTransferModify").hide();
            $("#divStockTransferList").fadeIn();
            $("#divNewProducts").trigger('detach.ScrollToFixed');
        };

        self.openSearchProductDialog = function () {
            $("#searchProductModal").modal({ backdrop: 'static' });
        };

        self.searchItem = function () {
            self.searchItemResults.removeAll();
            self.showLoadMoreItems(false);
            self.savedItemFilter = null;
            self.loadSearchItem();
            $("#txtSearchItem").val("");
        };

        self.loadSearchItem = function () {
            var filter;
            if (self.savedItemFilter) {
                filter = self.savedItemFilter;
            }
            else {
                filter = $("#txtSearchItem").val();
                self.savedItemFilter = filter;
            };

            DAL.fnSearchItem(filter, true, false, true, self.searchItemResults().length, settings.numOfRecords, function (result) {
                if (result != null) {
                    for (var i = 0; i < result.records.length; i++) {
                        self.searchItemResults.push(new ItemToAdd(result.records[i]));
                    };

                    if (result.totalRecords > self.searchItemResults().length) {
                        self.showLoadMoreItems(true);
                    }
                    else {
                        self.showLoadMoreItems(false);
                    };
                };
            });
        };

        self.selectItem = function (item) {
            self.selectedItemNo(item.data.ItemNo);
            $("#searchProductModal").modal("hide");
        };

        self.addItemToOrder = function () {
            var itemNo = self.selectedItemNo().trim().toUpperCase();
            self.selectedItemNo(itemNo);

            if (!itemNo) {
                BootstrapDialog.alert("Please enter Item No.");
                return;
            };

            // Look for the same item in array and remove then reinsert it on top
            var match = ko.utils.arrayFirst(self.purchaseOrderDetails(), function (item) {
                return item.ItemNo.trim().toUpperCase() == itemNo;
            });
            if (match) {
                self.purchaseOrderDetails.remove(match);
                self.purchaseOrderDetails.unshift(match);
                BootstrapDialog.alert("Duplicate drug found in the list.");
                return;
            };

            if (self.qtyToAdd() < 1) {
                BootstrapDialog.alert("Quantity must be greater than zero.");
                return;
            };
            if (isNaN(self.qtyToAdd())) {
                BootstrapDialog.alert("Invalid Quantity.");
                return;
            };

            var poDet = {
                PurchaseOrderHeaderRefNo: self.currentPurchaseOrder().PurchaseOrderHeaderRefNo,
                PurchaseOrderDetailRefNo: "",
                ItemNo: itemNo,
                Quantity: self.qtyToAdd(),
                QtyInStock: 0
            };

            DAL.fnSavePurchaseOrderDetail(poDet, function (result) {
                if (result.status == "") {
                    poDet.PurchaseOrderDetailRefNo = result.PurchaseOrderDetail.PurchaseOrderDetailRefNo;
                    poDet.Quantity = result.PurchaseOrderDetail.Quantity;
                    poDet.ItemName = result.PurchaseOrderDetail.Item.ItemName;
                    poDet.UOM = result.PurchaseOrderDetail.Item.UOM;
                    poDet.BaseLevel = (result.PurchaseOrderDetail.Item.BaseLevel == 0) ? 1 : result.PurchaseOrderDetail.Item.BaseLevel;
                    poDet.QtyInStock = result.StockBalance;
                    self.purchaseOrderDetails.unshift(new PurchaseOrderDetail(poDet));
                    self.selectedItemNo("");
                    self.qtyToAdd("");

                    self.sortKeyPODet({ columnName: "", asc: false });
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.deleteSelectedPODet = function () {
            BootstrapDialog.confirm("Are you sure you want to delete the selected items?", function (ok) {
                if (ok) {
                    var itemsToDelete = ko.utils.arrayFilter(self.purchaseOrderDetails(), function (item) {
                        return item.isSelected() == true;
                    });

                    for (var i = 0; i < itemsToDelete.length; i++) {
                        DAL.fnDeletePurchaseOrderDetail(itemsToDelete[i], function (result, deletedItem) {
                            if (result.status == "") {
                                self.purchaseOrderDetails.remove(deletedItem);
                            }
                            else {
                                BootstrapDialog.alert(result.status);
                            };
                        });
                    };
                };
            });
        };

        self.cancelPurchaseOrder = function () {
            BootstrapDialog.confirm("Are you sure you want to cancel this Request?", function (ok) {
                if (ok) {
                    var prevStatus = self.currentPurchaseOrder().Status();
                    self.currentPurchaseOrder().Status("Cancelled");
                    self.SavePOHdr(function (success) {
                        if (success) {
                            self.isReadOnly(true);
                            self.backToList();
                        }
                        else {
                            self.currentPurchaseOrder().Status(prevStatus);
                        };
                    });
                };
            });
        };

        self.submitPurchaseOrder = function () {
            if (self.purchaseOrderDetails().length < 1) {
                BootstrapDialog.alert("Please insert at least 1 item to transfer.");
                return;
            };

            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function (item) {
                return item.InventoryLocationName.toUpperCase() == self.currentPurchaseOrder().DestInventoryLocationName().toUpperCase() &&
                       item.InventoryLocationName.toUpperCase() != "ALL";
            });
            if (!match) {
                BootstrapDialog.alert("Please select a valid 'To Outlet'.");
                return;
            };

            if (self.currentPurchaseOrder().DestInventoryLocationName().toUpperCase() == self.currentPurchaseOrder().InventoryLocationName().toUpperCase()) {
                BootstrapDialog.alert("'From Outlet' and 'To Outlet' cannot be the same.");
                return;
            };

            if (self.currentPurchaseOrder().ReasonID() == -1) {
                BootstrapDialog.alert("Please select Reason first.");
                return;
            };

            if ((self.reason().ReasonName.toUpperCase() == "OTHERS" || self.reason().ReasonName.toUpperCase() == "OTHER") && self.currentPurchaseOrder().Remark().trim() == "") {
                BootstrapDialog.alert("Remark is compulsory if Reason is 'Others'");
                return;
            };

            if (self.showPriceLevel() && !self.currentPurchaseOrder().PriceLevel()) {
                BootstrapDialog.alert("Please select Price Level first.");
                return;
            }

            BootstrapDialog.confirm("Are you sure you want to submit this Request?", function (ok) {
                if (ok) {
                    self.currentPurchaseOrder().Status("Submitted");
                    self.SavePOHdr(function (success) {
                        if (success) {
                            self.isReadOnly(true);
                            self.backToList();
                        }
                        else {
                            self.currentPurchaseOrder().Status("Pending");
                        };
                    });
                };
            });
        };

        self.changeItemQty = function (poDet) {
            var updateQty = function (qty) {
                if (qty < 1) {
                    BootstrapDialog.alert("Quantity must be greater than zero.");
                    return;
                };
                //if (qty > poDet.QtyInStock) {
                //    BootstrapDialog.alert("Transfer Quantity is greater than Quantity in Outlet.");
                //    return;
                //};
                poDet.Quantity(qty);
                DAL.fnChangePODetailQty(JSON.parse(ko.toJSON(poDet)), function (result) {
                    if (result.status != "") {
                        BootstrapDialog.alert(result.status);
                    };
                });
            };

            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Transfer Quantity:", poDet.Quantity(), function (qty) {
                    if (qty != null && !isNaN(qty)) {
                        qty = parseFloat(qty);
                        updateQty(qty);

                        // === Skip the Base Level checking ===
                        //var roundedQty = Math.ceil(qty / poDet.BaseLevel) * poDet.BaseLevel;
                        //if (Math.ceil(qty / poDet.BaseLevel) != (qty / poDet.BaseLevel)) {
                        //    BootstrapDialog.confirm("Base level is " + poDet.BaseLevel + ". Quantity will be changed to " + roundedQty, function (ok) {
                        //        if (ok) {
                        //            qty = roundedQty;
                        //            updateQty(qty);
                        //        }
                        //        else {
                        //            return;
                        //        };
                        //    });
                        //}
                        //else {
                        //    updateQty(qty);
                        //};
                    };
                });
            };
        };

        self.changeFromInventoryLocation = function (location) {
            self.fromInventoryLocation(location);
            self.fromInventoryLocationFilterText(location.InventoryLocationName);
        };

        self.changeToInventoryLocation = function (location) {
            self.toInventoryLocation(location);
            self.toInventoryLocationFilterText(location.InventoryLocationName);
        };

        self.editDestInventoryLocation = function (location) {
            self.currentPurchaseOrder().DestInventoryLocationID(location.InventoryLocationID);
            self.currentPurchaseOrder().DestInventoryLocationName(location.InventoryLocationName);
            self.SavePOHdr();
        };

        self.changeReason = function (reason) {
            self.reason(reason);
            self.currentPurchaseOrder().ReasonID(reason.ReasonID);
            self.SavePOHdr();
        };

        self.changePriceLevel = function (level) {
            self.selectedPriceLevel(level.Value);
            self.currentPurchaseOrder().PriceLevel(level.Key);
            self.SavePOHdr();
        };

        self.changeRemarks = function () {
            self.SavePOHdr();
        };

        self.isSelectAllPODet = ko.computed(function () {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function (item) {
                return item.isSelected() == true;
            }).length == self.purchaseOrderDetails().length;
        });

        self.toggleSelectAllPODet = function () {
            var toggle = !self.isSelectAllPODet();
            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                self.purchaseOrderDetails()[i].isSelected(toggle);
            }
            return true;
        };

        self.deleteCheckedBtnState = ko.computed(function () {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function (item) {
                return item.isSelected() == true;
            }).length < 1;
        });

        self.isSubmittedOrPosted = ko.computed(function () {
            if (self.currentPurchaseOrder()) {
                return self.currentPurchaseOrder().Status() == "Submitted" || self.currentPurchaseOrder().Status() == "Posted";
            }
            else {
                return false;
            }
        });

        self.allowToCancel = function () {
            var orderHdr = self.currentPurchaseOrder();
            if (orderHdr) {
                if (!self.allowChangeClinic && orderHdr.InventoryLocationID() != sessionStorage.locationID) {
                    return false;
                }
                else {
                    return orderHdr.Status() == "Pending" || orderHdr.Status() == "Submitted";
                };
            }
            else {
                return false;
            };
        };

        //self.isPendingOrSubmitted = ko.computed(function () {
        //    if (self.currentPurchaseOrder()) {
        //        return self.currentPurchaseOrder().Status() == "Pending" || self.currentPurchaseOrder().Status() == "Submitted";
        //    }
        //    else {
        //        return false;
        //    }
        //});

        self.isPOHdrNotFound = ko.computed(function () {
            return self.purchaseOrderHeaders().length < 1;
        });

        self.isItemNotFound = ko.computed(function () {
            return self.searchItemResults().length < 1;
        });

        self.sortPOHdr = function (sortKey) {
            var currSortKey = self.sortKeyPOHdr();
            if (currSortKey.columnName == sortKey) {
                currSortKey.asc = !currSortKey.asc;
            }
            else {
                currSortKey.columnName = sortKey;
                currSortKey.asc = true;
            };
            self.sortKeyPOHdr(currSortKey);
            self.searchPurchaseOrder();
        };

        self.sortPODet = function (sortKey) {
            var currSortKey = self.sortKeyPODet();
            if (currSortKey.columnName == sortKey) {
                currSortKey.asc = !currSortKey.asc;
            }
            else {
                currSortKey.columnName = sortKey;
                currSortKey.asc = true;
            };
            self.sortKeyPODet(currSortKey);

            self.purchaseOrderDetails.sort(function (a, b) {
                var sortDirection = (self.sortKeyPODet().asc) ? 1 : -1;
                var valueA = ko.utils.unwrapObservable(a[self.sortKeyPODet().columnName]);
                var valueB = ko.utils.unwrapObservable(b[self.sortKeyPODet().columnName]);
                return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
            });
        };

        self.print = function (PurchaseOrderHeaderRefNo) {
        var urlReport = settings.crReportLocation + "?r=TransferPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
            //BootstrapDialog.alert("Printout not available yet.");
        };

        self.printFromDetails = function () {
            self.print(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo);
        };

        self.fromInventoryLocationFiltered = ko.computed(function () {
            if (self.fromInventoryLocationFilterText() && self.fromInventoryLocationFilterText().toUpperCase() != "ALL") {
                return ko.utils.arrayFilter(self.inventoryLocationList(), function (item) {
                    return item.InventoryLocationName.toUpperCase().indexOf(self.fromInventoryLocationFilterText().toUpperCase()) == 0;
                });
            }
            else {
                return self.inventoryLocationList();
            };
        });

        self.toInventoryLocationFiltered = ko.computed(function () {
            if (self.toInventoryLocationFilterText() && self.toInventoryLocationFilterText().toUpperCase() != "ALL") {
                return ko.utils.arrayFilter(self.inventoryLocationList(), function (item) {
                    return item.InventoryLocationName.toUpperCase().indexOf(self.toInventoryLocationFilterText().toUpperCase()) == 0;
                });
            }
            else {
                return self.inventoryLocationList();
            };
        });

        self.destInventoryLocationFiltered = ko.computed(function () {
            if (self.currentPurchaseOrder() && self.currentPurchaseOrder().DestInventoryLocationName()) {
                return ko.utils.arrayFilter(self.inventoryLocationList(), function (item) {
                    return item.InventoryLocationName.toUpperCase().indexOf(self.currentPurchaseOrder().DestInventoryLocationName().toUpperCase()) == 0 &&
                           item.InventoryLocationName.toUpperCase() != "ALL";
                });
            }
            else {
                return ko.utils.arrayFilter(self.inventoryLocationList(), function (item) {
                    return item.InventoryLocationName.toUpperCase() != "ALL";
                });
            };
        });

        self.fromInventoryLocationFilterTextKeyUp = function (obj, e) {
            if (self.fromInventoryLocationFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');
        };

        self.toInventoryLocationFilterTextKeyUp = function (obj, e) {
            if (self.toInventoryLocationFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');
        };

        self.destInventoryLocationKeyUp = function (obj, e) {
            if (self.destInventoryLocationFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');

            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function (item) {
                return item.InventoryLocationName.toUpperCase() == self.currentPurchaseOrder().DestInventoryLocationName().toUpperCase() &&
                       item.InventoryLocationName.toUpperCase() != "ALL";
            });

            if (match) self.editDestInventoryLocation(match);
        };

        self.changeClinicFilter = function (value) {
            if (value == "FROM") {
                self.fromInventoryLocation(self.inventoryLocationList()[0]);
                self.fromInventoryLocationFilterText(self.fromInventoryLocation().InventoryLocationName);
                self.disableFromInventoryLocation(false);

                self.toInventoryLocation(ko.utils.arrayFirst(locations, function (item) {
                    return item.InventoryLocationID == (self.allowChangeClinic ? 0 : sessionStorage.locationID);
                }));
                self.toInventoryLocationFilterText(self.toInventoryLocation().InventoryLocationName);
                self.disableToInventoryLocation(true);
            }
            else {
                self.toInventoryLocation(self.inventoryLocationList()[0]);
                self.toInventoryLocationFilterText(self.toInventoryLocation().InventoryLocationName);
                self.disableToInventoryLocation(false);

                self.fromInventoryLocation(ko.utils.arrayFirst(locations, function (item) {
                    return item.InventoryLocationID == (self.allowChangeClinic ? 0 : sessionStorage.locationID);
                }));
                self.fromInventoryLocationFilterText(self.fromInventoryLocation().InventoryLocationName);
                self.disableFromInventoryLocation(true);
            };
        };

        DAL.fnGetInventoryStockOutReasonList('Transfer', function (reasons) {
            self.reasonList(self.reasonList().concat(reasons));
        });

        self.loadData();

    };

    $("form").submit(function (e) {
        e.preventDefault();
    });
});

