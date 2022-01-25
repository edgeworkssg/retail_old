/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />
/// <reference path="common_functions.js" />

$(function () {
    var dateDisplayFormat = settings.dateDisplayFormat;
    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;

    $("#divStockAdjustmentModify").hide();
    $("#expiryDate").datepicker({ autoclose: true, format: "d MM yyyy", todayHighlight: true, clearBtn: true });

    DAL.fnGetInventoryLocationList(function (locations) {
        locations.unshift({ InventoryLocationID: "0", InventoryLocationName: "ALL" });
        ko.applyBindings(new StockAdjustmentViewModel(locations), document.getElementById("dynamicContent"));
    });

    function PurchaseOrderHeader(data) {
        var self = this;

        self.PurchaseOrderHeaderRefNo = data.PurchaseOrderHeaderRefNo;
        self.PurchaseOrderDate = new Date(parseInt(data.PurchaseOrderDate.replace("/Date(", "").replace(")/", "")));
        self.Status = ko.observable(data.Status);
        self.Remark = ko.observable(data.Remark);
        self.InventoryLocationID = ko.observable(data.InventoryLocationID);
        self.InventoryLocationName = ko.observable(data.InventoryLocation.InventoryLocationName);
        self.POType = ko.observable(data.POType);
        self.ReasonID = ko.observable(data.ReasonID);
        self.RequestedBy = data.RequestedBy;

        self.PurchaseOrderDateFormatted = ko.computed(function () {
            return new Date(self.PurchaseOrderDate).toString(dateDisplayFormatWithTime);
        });
    };

    function PurchaseOrderDetail(data) {
        var self = this;

        self.PurchaseOrderHeaderRefNo = data.PurchaseOrderHeaderRefNo;
        self.PurchaseOrderDetailRefNo = data.PurchaseOrderDetailRefNo;
        self.ItemNo = data.ItemNo;
        self.ItemName = data.ItemName;
        self.ExpiryDate = ko.observable(data.ExpiryDate);
        self.Quantity = ko.observable(data.Quantity);
        self.QtyInStock = data.QtyInStock;
        self.QtyApproved = data.QtyApproved;
        self.UOM = data.UOM;
        self.BaseLevel = data.BaseLevel;
        self.Remark = data.Remark;
        self.Status = data.Status;

        self.ExpiryDateFormatted = ko.computed(function () {
            return (self.ExpiryDate()) ? new Date(self.ExpiryDate()).toString(dateDisplayFormat) : "";
        });

        self.isSelected = ko.observable(false);
    };

    function ItemToAdd(data) {
        var self = this;
        self.data = data;
    };

    // Knockout.js ViewModel
    function StockAdjustmentViewModel(locations) {
        var self = this;

        self.statusDropdown = ["ALL", "Pending", "Submitted", "Approved", "Rejected", "Cancelled"];
        self.adjTypeDropdown = ["Adjustment In", "Adjustment Out"];
        self.savedFilter = null;
        self.savedItemFilter = null;

        self.searchOrderNumber = ko.observable("");
        self.purchaseOrderHeaders = ko.observableArray();
        self.purchaseOrderDetails = ko.observableArray();
        self.currentPurchaseOrder = ko.observable();
        self.searchItemResults = ko.observableArray();
        self.selectedItemNo = ko.observable("");
        self.expiryDate = ko.observable("");
        self.qtyToAdd = ko.observable("");
        self.isReadOnly = ko.observable(false);
        self.showLoadMore = ko.observable(false);
        self.status = ko.observable("Pending");
        self.adjustmentType = ko.observable(self.adjTypeDropdown[0]);
        self.reasonList = ko.observableArray([{ ReasonID: "-1", ReasonName: "-- Select Reason --" }]);
        self.reason = ko.observable(self.reasonList()[0]);
        self.showLoadMoreItems = ko.observable(false);
        self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.inventoryLocationList = ko.observableArray(locations);
        self.inventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function (item) {
            return item.InventoryLocationID == (self.allowChangeClinic ? 0 : sessionStorage.locationID);
        }));
        self.inventoryLocationFilterText = ko.observable(self.inventoryLocation().InventoryLocationName);
        self.inventoryLocationFilterTextDetail = ko.observable("");
        self.sortKeyPOHdr = ko.observable({ columnName: "PurchaseOrderDate", asc: false });
        self.sortKeyPODet = ko.observable({ columnName: "ItemNo", asc: true });


        self.loadData = function () {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                filter = {
                    purchaseorderheaderrefno: self.searchOrderNumber(),
                    inventorylocationid: self.inventoryLocation().InventoryLocationID,
                    status: self.status(),
                    potype: self.adjustmentType()
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

            self.purchaseOrderHeaders.removeAll();
            self.showLoadMore(false);
            self.savedFilter = null;
            self.loadData();
            self.searchOrderNumber("");
        };

        self.openAddNewPage = function () {
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

            // Initialize new PO Header
            var poHdr = {
                PurchaseOrderDate: new Date().toString(dateValueFormat),
                Status: "Pending",
                Remark: "",
                InventoryLocationID: self.inventoryLocation().InventoryLocationID,
                InventoryLocationName: self.inventoryLocation().InventoryLocationName,
                ReasonID: -1,
                POType: self.adjustmentType(),
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

                    $("#divStockAdjustmentList").hide();
                    $("#divStockAdjustmentModify").fadeIn();
                    self.isReadOnly(orderHdr.Status() == "Pending" ? false : true);

                    self.currentPurchaseOrder(orderHdr);
                    self.inventoryLocationFilterTextDetail(orderHdr.InventoryLocationName());

                    self.reasonList([{ ReasonID: "-1", ReasonName: "-- Select Reason --" }]);
                    DAL.fnGetInventoryStockOutReasonList(orderHdr.POType(), function (reasons) {
                        self.reasonList(self.reasonList().concat(reasons));
                        var selectedReason = ko.utils.arrayFirst(self.reasonList(), function (item) {
                            return item.ReasonID == orderHdr.ReasonID();
                        });
                        if (selectedReason) self.reason(selectedReason);
                    });

                    self.purchaseOrderDetails.removeAll();
                    self.loadPurchaseOrderDetails(orderHdr.PurchaseOrderHeaderRefNo);
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
                            ExpiryDate: result.records[i].PurchaseOrderDetail.ExpiryDate,
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
            $("#divStockAdjustmentModify").hide();
            $("#divStockAdjustmentList").fadeIn();
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
                return item.ItemNo.trim().toUpperCase() == itemNo &&
                       item.ExpiryDate() == new Date(self.expiryDate()).toString(dateValueFormat);
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
                ExpiryDate: (self.expiryDate()) ? new Date(self.expiryDate()).toString(dateValueFormat) : null,
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
                    $("#expiryDate").datepicker('update', new Date());
                    self.expiryDate("");

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
                    self.currentPurchaseOrder().Status("Cancelled");
                    self.SavePOHdr(function (success) {
                        if (success) {
                            self.isReadOnly(true);
                            self.backToList();
                        }
                        else {
                            self.currentPurchaseOrder().Status("Pending");
                        };
                    });

                    //DAL.fnDeletePurchaseOrder(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, function (result) {
                    //    if (result.status == "") {
                    //        self.purchaseOrderDetails.removeAll();
                    //        // Look for the same item in array and remove it
                    //        var match = ko.utils.arrayFirst(self.purchaseOrderHeaders(), function (item) {
                    //            return item.PurchaseOrderHeaderRefNo == self.currentPurchaseOrder().PurchaseOrderHeaderRefNo;
                    //        });
                    //        self.purchaseOrderHeaders.remove(match);
                    //        alert("Request has been cancelled.");
                    //        self.backToList();
                    //    }
                    //    else {
                    //        alert(result.status);
                    //    };
                    //});
                };
            });
        };

        self.submitPurchaseOrder = function () {
            if (self.purchaseOrderDetails().length < 1) {
                BootstrapDialog.alert("Please insert at least 1 item to adjust.");
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
                //    BootstrapDialog.alert("Adjustment Quantity is greater than Quantity in Clinic.");
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
                BootstrapDialog.prompt("Adjustment Quantity:", poDet.Quantity(), function (qty) {
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

        self.changeInventoryLocation = function (location) {
            self.inventoryLocation(location);
            self.inventoryLocationFilterText(location.InventoryLocationName);
        };

        self.editInventoryLocationInDetails = function (location) {
            self.inventoryLocationFilterTextDetail(location.InventoryLocationName);
            self.currentPurchaseOrder().InventoryLocationID(location.InventoryLocationID);
            self.currentPurchaseOrder().InventoryLocationName(location.InventoryLocationName);
            self.SavePOHdr();
        };

        self.changeReason = function (reason) {
            self.reason(reason);
            self.currentPurchaseOrder().ReasonID(reason.ReasonID);
            self.SavePOHdr();
        };

        self.changePOType = function (potype) {
            self.currentPurchaseOrder().POType(potype);
            var reason = self.reasonList()[0];
            self.reason(reason);
            self.currentPurchaseOrder().ReasonID(reason.ReasonID);
            self.SavePOHdr();

            self.reasonList([{ ReasonID: "-1", ReasonName: "-- Select Reason --" }]);
            DAL.fnGetInventoryStockOutReasonList(potype, function (reasons) {
                self.reasonList(self.reasonList().concat(reasons));
            });
        };

        self.changeRemarks = function () {
            self.SavePOHdr();
        };

        self.changeExpiryDate = function (poDet) {
            var oldValue = poDet.ExpiryDate() ? poDet.ExpiryDate() : "";  //poDet.ExpiryDate is of type String
            BootstrapDialog.promptDatePicker("Expiry Date:", poDet.ExpiryDate(), function (expDate) {
                // expDate is of type Date
                var newValue = expDate ? new Date(expDate).toString(dateValueFormat) : "";

                console.log(oldValue);
                console.log(newValue);

                if (oldValue != newValue) {
                    console.log("SAVE");
                    poDet.ExpiryDate(expDate ? new Date(expDate).toString(dateValueFormat) : null);
                    DAL.fnChangePODetailExpiryDate(JSON.parse(ko.toJSON(poDet)), function (result) {
                        if (result.status != "") {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                }
            });
        }

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

        self.isPOHdrNotFound = ko.computed(function () {
            return self.purchaseOrderHeaders().length < 1;
        });

        self.isItemNotFound = ko.computed(function () {
            return self.searchItemResults().length < 1;
        });

        self.isApprovedOrRejected = ko.computed(function () {
            if (self.currentPurchaseOrder()) {
                return self.currentPurchaseOrder().Status() == 'Approved' || self.currentPurchaseOrder().Status() == 'Rejected';
            }
            else {
                return false;
            }
        });

        self.purchaseOrderDetailsApprovedOrPending = ko.computed(function () {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function (item) {
                return item.Status != "Rejected";
            });
        });

        self.purchaseOrderDetailsRejected = ko.computed(function () {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function (item) {
                return item.Status == "Rejected";
            });
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
        var urlReport = settings.crReportLocation + "?r=AdjustmentPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
            //BootstrapDialog.alert("Printout not available yet.");
        };

        self.printFromDetails = function () {
            self.print(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo);
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

        self.inventoryLocationFilteredDetail = ko.computed(function () {
            if (self.inventoryLocationFilterTextDetail()) {
                return ko.utils.arrayFilter(self.inventoryLocationList(), function (item) {
                    return item.InventoryLocationName.toUpperCase().indexOf(self.inventoryLocationFilterTextDetail().toUpperCase()) == 0 &&
                           item.InventoryLocationName.toUpperCase() != "ALL";
                });
            }
            else {
                return ko.utils.arrayFilter(self.inventoryLocationList(), function (item) {
                    return item.InventoryLocationName.toUpperCase() != "ALL";
                });
            };
        });

        self.inventoryLocationFilterTextKeyUp = function (obj, e) {
            if (self.inventoryLocationFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');
        };

        self.inventoryLocationFilterTextDetailKeyUp = function (obj, e) {
            if (self.inventoryLocationFilteredDetail().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');

            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function (item) {
                return item.InventoryLocationName.toUpperCase() == self.inventoryLocationFilterTextDetail().toUpperCase() &&
                       item.InventoryLocationName.toUpperCase() != "ALL";
            });

            if (match) self.editInventoryLocationInDetails(match);
        };


        DAL.fnGetInventoryStockOutReasonList(self.adjustmentType(), function (reasons) {
            self.reasonList(self.reasonList().concat(reasons));
        });

        self.loadData();

    };

    $("form").submit(function (e) {
        e.preventDefault();
    });
});

