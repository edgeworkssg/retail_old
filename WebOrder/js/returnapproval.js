/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />
/// <reference path="setting.js" />

$(function() {
    var dateDisplayFormat = settings.dateDisplayFormat;
    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;

    $('#tableApproved').freezeTable({
        'freezeColumn': false,
        'freezeHead': true,
        'namespace': 'tableApproved'
    });

    $('#tableRejected').freezeTable({
        'freezeColumn': false,
        'freezeHead': true,
        'namespace': 'tableRejected'
    });

    $('#tableSearch').freezeTable({
        'freezeColumn': false,
        'freezeHead': true,
        'namespace': 'tableSearch'
    });
    
    $("#divApprovalDetails").hide();
    $("#dateFrom").datepicker({ autoclose: true, format: "d MM yyyy", todayHighlight: true });
    $("#dateTo").datepicker({ autoclose: true, format: "d MM yyyy", todayHighlight: true });

    DAL.fnGetInventoryLocationList(function(locations) {
        //locations.unshift({ InventoryLocationID: "0", InventoryLocationName: "ALL" });
        DAL.fnGetWarehouseList(function(warehouses) {
            ko.applyBindings(new OrderApprovalViewModel(locations, warehouses), document.getElementById("dynamicContent"));
        });
    });

    function PurchaseOrderHeader(data) {
        var self = this;

        self.PurchaseOrderHeaderRefNo = data.PurchaseOrderHeaderRefNo;
        self.PurchaseOrderDate = new Date(parseInt(data.PurchaseOrderDate.replace("/Date(", "").replace(")/", "")));
        self.Status = ko.observable(data.Status);
        self.Remark = data.Remark;
        self.InventoryLocationID = data.InventoryLocationID;
        self.InventoryLocationName = data.InventoryLocation.InventoryLocationName;
        self.ReasonID = data.ReasonID;
        self.ReasonName = data.InventoryStockOutReason.ReasonName;
        self.ApprovalDate = new Date(parseInt(data.ApprovalDate.replace("/Date(", "").replace(")/", "")));
        self.ShipVia = ko.observable(data.ShipVia);

        self.PurchaseOrderDateFormatted = ko.computed(function() {
            return new Date(self.PurchaseOrderDate).toString(dateDisplayFormatWithTime);
        });

        self.ApprovalDateFormatted = ko.computed(function() {
            return new Date(self.ApprovalDate).toString(dateDisplayFormatWithTime);
        });

        self.PriceLevel = data.PriceLevel;

        self.IsCanPrint = ko.computed(function() {
            return self.Status() == 'Submitted' || self.Status() == 'Approved' || self.Status() == 'Rejected';
        });

        self.WarehouseID = ko.observable(data.WarehouseID);
        self.SupplierID = ko.observable(data.SupplierID);
        self.OrderFromName = data.OrderFromName;
    };

    function PurchaseOrderDetail(data) {
        var self = this;

        self.PurchaseOrderHeaderRefNo = data.PurchaseOrderHeaderRefNo;
        self.PurchaseOrderDetailRefNo = data.PurchaseOrderDetailRefNo;
        self.ItemNo = data.ItemNo;
        self.ItemName = data.ItemName;
        if (data.ExpiryDate) {
            if (data.ExpiryDate.indexOf("/Date") > -1)
                self.ExpiryDate = new Date(parseInt(data.ExpiryDate.replace("/Date(", "").replace(")/", "")));
            else
                self.ExpiryDate = new Date(data.ExpiryDate);
        }
        self.Quantity = data.Quantity;
        self.Remark = ko.observable(data.Remark);
        self.Status = ko.observable(data.Status);
        self.UOM = data.UOM;
        self.BaseLevel = data.BaseLevel;

        self.ExpiryDateFormatted = ko.computed(function() {
            if (self.ExpiryDate)
                return new Date(self.ExpiryDate).toString(dateDisplayFormat);
            else
                return "";
        });

        if (!data.Status) {
            self.QtyApproved = ko.observable(data.Quantity);
        }
        else {
            self.QtyApproved = ko.observable(data.QtyApproved);
        }

        self.isSelected = ko.observable(false);

        self.FactoryPrice = ko.observable(data.FactoryPrice);
        self.FactoryPriceFormatted = ko.computed(function() {
            return self.FactoryPrice() == null || self.FactoryPrice().length === 0 ? "0.00" : parseFloat(self.FactoryPrice()).toFixed(2);
        });
        self.TotalCost = ko.computed(function() {
            return (self.FactoryPrice() == null || self.QtyApproved() == null) ? (0).toFixed(2) : parseFloat(self.FactoryPrice() * self.QtyApproved()).toFixed(2);
        });

        self.P1Price = data.P1Price;
        self.P2Price = data.P2Price;
        self.P3Price = data.P3Price;
        self.P4Price = data.P4Price;
        self.P5Price = data.P5Price;
    };

    // Knockout.js ViewModel
    function OrderApprovalViewModel(locations, warehouses) {
        var self = this;

        self.dateDropdown = [
            { name: "Today", startDate: fnToday().start, endDate: fnToday().end },
            { name: "Yesterday", startDate: fnYesterday().start, endDate: fnYesterday().end },
            { name: "This week", startDate: fnThisWeek().start, endDate: fnThisWeek().end },
            { name: "Last week", startDate: fnLastWeek().start, endDate: fnLastWeek().end },
            { name: "This month", startDate: fnThisMonth().start, endDate: fnThisMonth().end },
            { name: "Last month", startDate: fnLastMonth().start, endDate: fnLastMonth().end },
            { name: "This year", startDate: fnThisYear().start, endDate: fnThisYear().end },
            { name: "Last year", startDate: fnLastYear().start, endDate: fnLastYear().end },
            { name: "---", startDate: null, endDate: null },
            { name: "Custom...", startDate: null, endDate: null }
        ];

        self.statusDropdown = ["ALL", "Submitted", "Approved", "Rejected"];
        self.savedFilter = null;

        self.selectedDate = ko.observable(self.dateDropdown[0]);
        self.status = ko.observable("Submitted");
        self.purchaseOrderHeaders = ko.observableArray();
        self.purchaseOrderDetails = ko.observableArray();
        self.currentPurchaseOrder = ko.observable();
        self.isReadOnly = ko.observable(false);
        self.IsCanPrint = ko.observable(false);
        self.showLoadMore = ko.observable(false);
        self.sortKeyPOHdr = ko.observable({ columnName: "PurchaseOrderDate", asc: false });
        self.sortKeyPODet = ko.observable({ columnName: "ItemNo", asc: true });
        //self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.inventoryLocationList = ko.observableArray(locations);
        self.inventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function(item) {
            //return item.InventoryLocationID == (self.allowChangeClinic ? 0 : sessionStorage.locationID);
            return item.InventoryLocationID == sessionStorage.locationID;
        }));
        self.inventoryLocationFilterText = ko.observable(self.inventoryLocation().InventoryLocationName);
        self.currentStatus = ko.observable('');

        self.warehouseName = ko.observable("");
        self.warehouseList = ko.observableArray(warehouses);

        self.displayDate = ko.computed(function() {
            return self.selectedDate().startDate.toString(dateDisplayFormat) + " - " + self.selectedDate().endDate.toString(dateDisplayFormat);
        });

        self.selectDate = function(date) {
            switch (date.name) {
                case "---":
                    break;
                case "Custom...":
                    // show date picker modal
                    $("#dateFrom").val(self.selectedDate().startDate.toString(dateDisplayFormat));
                    $("#dateTo").val(self.selectedDate().endDate.toString(dateDisplayFormat));
                    $("#dateFrom").datepicker('update');
                    $("#dateTo").datepicker('update');
                    $("#customdateModal").modal();
                    break;
                default:
                    self.selectedDate(date);
                    break;
            }
        };

        self.setCustomDate = function() {
            self.selectedDate({
                name: "Custom...",
                startDate: new Date($("#dateFrom").val() + " 00:00:00"),
                endDate: new Date($("#dateTo").val() + " 23:59:59")
            });
            $("#customdateModal").modal("hide");
        };

        self.showPriceLevel = ko.observable(false);
        self.priceLevel = ko.observable();
        self.selectedPriceLevel = ko.observable("");
        self.selectedPriceLevelKey = ko.observable("");
        DAL.fnGetSetting("GoodsOrdering_ShowPriceLevelForWebOrder", "yes", function(res) {
            if (res == "True") {
                DAL.fnGetPriceLevel(function(resLevel) {
                    if (resLevel.status == "" && resLevel.records.length > 0) {
                        self.priceLevel(resLevel.records);
                        self.showPriceLevel(true);
                    }
                });
            };
        });

        self.setPriceLevel = function(o) {
            if (!self.isReadOnly()) {
                self.selectedPriceLevel(o.Value);
                self.selectedPriceLevelKey(o.Key);

                updateFactoryPrice();
            };
        };

        function updateFactoryPrice() {
            if (!self.isReadOnly()) {
                DAL.fnChangePODetailFactoryPriceAll(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, self.selectedPriceLevelKey(), function(result) {
                    if (result.status == "") {
                        for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                            var poDet = self.purchaseOrderDetails()[i];
                            var price = poDet.FactoryPrice();

                            var oldprice = poDet.FactoryPrice();

                            if (self.selectedPriceLevelKey() == "P1" && poDet.P1Price != null)
                                price = poDet.P1Price;
                            else if (self.selectedPriceLevelKey() == "P2" && poDet.P2Price != null)
                                price = poDet.P2Price;
                            else if (self.selectedPriceLevelKey() == "P3" && poDet.P3Price != null)
                                price = poDet.P3Price;
                            else if (self.selectedPriceLevelKey() == "P4" && poDet.P4Price != null)
                                price = poDet.P4Price;
                            else if (self.selectedPriceLevelKey() == "P5" && poDet.P5Price != null)
                                price = poDet.P5Price;

                            poDet.FactoryPrice(price);
                        }
                    }
                    else {
                        BootstrapDialog.alert(result.status);
                    };
                });             
            }
        }

        self.showFactoryPrice = ko.observable(false);
        DAL.fnGetSetting("GoodsOrdering_ShowFactoryPriceInReturnApproval", "yes", function(res) {
            if (res == "True") {
                self.showFactoryPrice(true);
            };
        });

        self.loadData = function() {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                filter = {
                    frompage: "ReturnApproval",
                    startdate: self.selectedDate().startDate,
                    enddate: self.selectedDate().endDate,
                    inventorylocationid: self.inventoryLocation().InventoryLocationID,
                    status: self.status(),
                    potype: "Return"
                };
                self.savedFilter = filter;
            };

            DAL.fnGetPurchaseOrderHeaderListWithOutletName(JSON.stringify(filter), self.purchaseOrderHeaders().length, settings.numOfRecords, self.sortKeyPOHdr().columnName, self.sortKeyPOHdr().asc, function(data) {
                for (var i = 0; i < data.records.length; i++) {
                    self.purchaseOrderHeaders.push(new PurchaseOrderHeader(data.records[i]));
                };

                if (data.totalRecords > self.purchaseOrderHeaders().length) {
                    self.showLoadMore(true);
                }
                else {
                    self.showLoadMore(false);
                };

                $('#tableSearch').freezeTable('update');
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

            self.purchaseOrderHeaders.removeAll();
            self.showLoadMore(false);
            self.savedFilter = null;
            self.loadData();
        };

        self.openUpdatePage = function(order) {
            DAL.fnGetPurchaseOrderHeader(order.PurchaseOrderHeaderRefNo, function(result) {
                if (result.status == "") {
                    // Create a PurchaseOrderHeader object to work with.
                    var orderHdr = new PurchaseOrderHeader(result.PurchaseOrderHeader);
                    $("#divApprovalList").hide();
                    $("#divApprovalDetails").fadeIn();
                    self.isReadOnly(orderHdr.Status() == "Submitted" ? false : true);
                    self.IsCanPrint(orderHdr.Status() == "Submitted" || orderHdr.Status() == 'Approved' || orderHdr.Status() == 'Rejected' ? true : false);
                    self.currentStatus(orderHdr.Status());
                    self.currentPurchaseOrder(orderHdr);
                    self.purchaseOrderDetails.removeAll();
                    self.loadPurchaseOrderDetails(orderHdr.PurchaseOrderHeaderRefNo);

                    var war = ko.utils.arrayFirst(self.warehouseList(), function(item) {
                        return item.SupplierID == orderHdr.SupplierID();
                    });
                    if (war) {
                        self.warehouseName(war.SupplierName);
                    }
                    else {
                        self.warehouseName("");
                    };

                    if (self.showPriceLevel() && self.priceLevel().length > 0) {
                        var level = ko.utils.arrayFirst(self.priceLevel(), function(item) {
                            return item.Key == orderHdr.PriceLevel;
                        });
                        if (level) {
                            self.selectedPriceLevel(level.Value);
                            self.selectedPriceLevelKey(level.Key);
                        }
                        else {
                            self.selectedPriceLevel("");
                            self.selectedPriceLevelKey("");
                        }
                    };
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });

        };

        self.loadPurchaseOrderDetails = function(PurchaseOrderHeaderRefNo) {
            DAL.fnGetPurchaseOrderDetailList(PurchaseOrderHeaderRefNo, function(result) {
                if (result.status == "") {
                    for (var i = 0; i < result.records.length; i++) {
                        var poDet = {
                            PurchaseOrderHeaderRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderHeaderRefNo,
                            PurchaseOrderDetailRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderDetailRefNo,
                            ItemNo: result.records[i].PurchaseOrderDetail.ItemNo,
                            ItemName: result.records[i].PurchaseOrderDetail.Item.ItemName,
                            ExpiryDate: result.records[i].PurchaseOrderDetail.ExpiryDate,
                            Quantity: result.records[i].PurchaseOrderDetail.Quantity,
                            QtyApproved: result.records[i].PurchaseOrderDetail.QtyApproved,
                            Remark: result.records[i].PurchaseOrderDetail.Remark,
                            Status: result.records[i].PurchaseOrderDetail.Status,
                            UOM: result.records[i].PurchaseOrderDetail.Item.UOM,
                            BaseLevel: (result.records[i].PurchaseOrderDetail.Item.BaseLevel == "0") ? "1" : result.records[i].PurchaseOrderDetail.Item.BaseLevel,
                            FactoryPrice: result.records[i].PurchaseOrderDetail.FactoryPrice,
                            P1Price: result.records[i].PurchaseOrderDetail.Item.P1Price,
                            P2Price: result.records[i].PurchaseOrderDetail.Item.P2Price,
                            P3Price: result.records[i].PurchaseOrderDetail.Item.P3Price,
                            P4Price: result.records[i].PurchaseOrderDetail.Item.P4Price,
                            P5Price: result.records[i].PurchaseOrderDetail.Item.P5Price
                        };
                        self.purchaseOrderDetails.push(new PurchaseOrderDetail(poDet));
                    };

                    self.sortKeyPODet({ columnName: "ItemNo", asc: false });
                    self.sortPODet('ItemNo');

                    $('#tableApproved').freezeTable('update');
                    $('#tableRejected').freezeTable('update');

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

        self.backToList = function() {
            $("#divApprovalDetails").hide();
            $("#divApprovalList").fadeIn();
        };

        self.isSelectAllPODet = ko.computed(function() {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function(item) {
                return item.isSelected() == true;
            }).length == self.purchaseOrderDetails().length;
        });

        self.toggleSelectAllPODet = function() {
            var toggle = !self.isSelectAllPODet();
            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                self.purchaseOrderDetails()[i].isSelected(toggle);
            }
            return true;
        };

        self.rejectCheckedBtnState = ko.computed(function() {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function(item) {
                return item.isSelected() == true;
            }).length < 1;
        });

        self.submitApproval = function() {
            if (self.showPriceLevel() && !self.selectedPriceLevelKey()) {
                BootstrapDialog.alert("Please select Price Level first.");
                return;
            }

            BootstrapDialog.confirm("Are you sure you want to submit this Approval?", function(ok) {
                if (ok) {
                    for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                        var poDet = self.purchaseOrderDetails()[i];

                        //if (Math.ceil(poDet.QtyApproved() / poDet.BaseLevel) != (poDet.QtyApproved() / poDet.BaseLevel)) {
                        //    BootstrapDialog.alert(poDet.ItemName + ": Approved Quantity is not a multiply of base level value");
                        //    return;
                        //};

                        if (poDet.QtyApproved() != poDet.Quantity && (!poDet.Remark() || poDet.Remark().trim() == "")) {
                            BootstrapDialog.alert(poDet.ItemName + " (Exp. " + poDet.ExpiryDateFormatted() + ") : Remarks is mandatory if Approved Quantity is not equal to Returned Quantity");
                            return;
                        };

                        if (poDet.QtyApproved() > 0) {
                            poDet.Status("Approved");
                        }
                        else {
                            poDet.Status("Rejected");
                        };
                    };

                    DAL.fnPurchaseOrderApproval(JSON.parse(ko.toJSON(self.purchaseOrderDetails())), self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, sessionStorage.username, "No", self.selectedPriceLevelKey(), function(result) {
                        if (result.status == "") {
                            var prevStatus = self.currentPurchaseOrder().Status();

                            self.doStockOut(function(status) {
                                if (status == "") {
                                    self.currentPurchaseOrder(new PurchaseOrderHeader(result.PurchaseOrderHeader));
                                    self.isReadOnly(true);
                                    // Look for the same item in array and update (replace) it with current item that we're working with
                                    var match = ko.utils.arrayFirst(self.purchaseOrderHeaders(), function(item) {
                                        return item.PurchaseOrderHeaderRefNo == self.currentPurchaseOrder().PurchaseOrderHeaderRefNo;
                                    });
                                    self.purchaseOrderHeaders.replace(match, self.currentPurchaseOrder());
                                    // Refresh the Purchase Order Details
                                    self.purchaseOrderDetails.removeAll();
                                    self.loadPurchaseOrderDetails(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo);

                                    self.purchaseOrderHeaders.sort(function(a, b) {
                                        var sortDirection = (self.sortKeyPOHdr().asc) ? 1 : -1;
                                        var valueA = ko.utils.unwrapObservable(a[self.sortKeyPOHdr().columnName]);
                                        var valueB = ko.utils.unwrapObservable(b[self.sortKeyPOHdr().columnName]);
                                        return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
                                    });

                                    self.backToList();
                                }
                                else {
                                    BootstrapDialog.alert(status);

                                    DAL.fnRevertPOHeaderStatus(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, prevStatus, function(result) {
                                        if (result.status != "") {
                                            BootstrapDialog.alert(result.status);
                                        };
                                    });
                                };
                            });
                        }
                        else {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                };
            });
        };

        self.returnToPending = function() {
            var poReturnToPending = function() {
                DAL.fnRevertPOHeaderStatus(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, 'Pending', function(result) {
                    if (result.status == "") {
                        self.backToList();
                        self.searchPurchaseOrder();
                    }
                    else {
                        BootstrapDialog.alert(result.status);
                    };
                });
            };

            BootstrapDialog.confirm("Are you sure you want to return this Stock Return to the pending status?", function(ok) {
                if (ok) {
                    var c = 0;
                    for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                        var poDet = self.purchaseOrderDetails()[i];
                        if (poDet.Status == "Rejected")
                            c++;
                    }

                    if (c == 0)
                        poReturnToPending();
                    else
                        BootstrapDialog.alert("There is a rejected item. This process is cancelled.");
                }
            });
        };

        self.doStockOut = function(callback) {
            var tmpPODet = ko.utils.arrayFilter(self.purchaseOrderDetails(), function(item) {
                return item.QtyApproved() > 0;
            });

            if (tmpPODet.length > 0) {
                var itemsToReturn = new Array();
                for (var i = 0; i < tmpPODet.length; i++) {
                    var item = { ItemNo: tmpPODet[i].ItemNo, Quantity: tmpPODet[i].QtyApproved };
                    itemsToReturn.push(item);
                };
                DAL.fnStockReturn(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, JSON.parse(ko.toJSON(itemsToReturn)), sessionStorage.username, self.currentPurchaseOrder().ReasonID, self.currentPurchaseOrder().InventoryLocationID, false, true, "", function(result) {
                    if (result.status == "") {
                        BootstrapDialog.alert("Stock have been returned successfully.");
                        if (callback) callback("");
                    }
                    else {
                        if (callback) callback(result.status);
                    };
                });
            }
            else {
                if (callback) callback("");
            };
        };

        self.changeInventoryLocation = function(location) {
            self.inventoryLocation(location);
            self.inventoryLocationFilterText(location.InventoryLocationName);
        };

        self.changeCreditInvoiceNo = function(o) {
            DAL.fnChangeCreditInvoiceNo(o.PurchaseOrderHeaderRefNo, o.ShipVia(), function(result) {
                if (result.status != "") {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.rejectChecked = function() {
            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].isSelected()) {
                    self.purchaseOrderDetails()[i].QtyApproved("0");
                    self.purchaseOrderDetails()[i].Status("Rejected");
                    self.purchaseOrderDetails()[i].isSelected(false);
                    DAL.fnChangePOQtyApproved(JSON.parse(ko.toJSON(self.purchaseOrderDetails()[i])), function(result) {
                        if (result.status != "") {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                };
            };
        };

        self.setQtyApproved = function(poDet) {
            var updateQty = function(qty) {
                if (qty > 0) {
                    poDet.Status("Approved");
                }
                else {
                    poDet.Status("Rejected");
                };

                poDet.QtyApproved(qty);
                DAL.fnChangePOQtyApproved(JSON.parse(ko.toJSON(poDet)), function(result) {
                    if (result.status != "") {
                        BootstrapDialog.alert(result.status);
                    };
                });
            };

            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Approved Quantity:", poDet.QtyApproved(), function(qty) {
                    if (qty != null && !isNaN(qty)) {
                        qty = parseFloat(qty);
                        var roundedQty = Math.ceil(qty / poDet.BaseLevel) * poDet.BaseLevel;

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
                        updateQty(qty);
                        //};
                    };
                });
            };
        };

        self.setFactoryPrice = function(poDet) {
            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Factory Price:", poDet.FactoryPrice(), function(price) {
                    if (price != null && !isNaN(price)) {
                        price = parseFloat(price);
                        DAL.fnChangePODetailFactoryPrice(poDet.PurchaseOrderDetailRefNo, price, function(result) {
                            if (result.status == "") {
                                poDet.FactoryPrice(price);
                            }
                            else {
                                BootstrapDialog.alert(result.status);
                            };
                        });
                    };
                });
            }
        };

        self.setRemark = function(poDet) {
            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Remarks:", (poDet.Remark() ? poDet.Remark() : ""), function(remark) {
                    if (remark != null) {
                        poDet.Remark(remark.trim());
                        DAL.fnChangePOQtyApproved(JSON.parse(ko.toJSON(poDet)), function(result) {
                            if (result.status != "") {
                                BootstrapDialog.alert(result.status);
                            };
                        });
                    };
                });
            };
        };

        self.isPOHdrNotFound = ko.computed(function() {
            return self.purchaseOrderHeaders().length < 1;
        });

        self.purchaseOrderDetailsApprovedOrPending = ko.computed(function() {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function(item) {
                return item.Status() != "Rejected";
            });
        });

        self.purchaseOrderDetailsRejected = ko.computed(function() {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function(item) {
                return item.Status() == "Rejected";
            });
        });

        self.sortPOHdr = function(sortKey) {
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

        self.print = function(PurchaseOrderHeaderRefNo) {
            var urlReport = settings.crReportLocation + "?r=ReturnPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&ShowBarcode=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
            //window.print();
        };

        self.printFromDetails = function() {
            self.print(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo);
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


        self.loadData();

    };

    $("form").submit(function(e) {
        e.preventDefault();
    });
});

