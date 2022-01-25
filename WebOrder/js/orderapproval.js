/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />

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
        DAL.fnGetSuppliersUserPortal(function(suppliers) {
            DAL.fnGetSuppliersUserPortalWithoutWarehouse(function(suppliersnowh) {
                //locations.unshift({ InventoryLocationID: "0", InventoryLocationName: "ALL" });
                DAL.fnGetWarehouseList(function(warehouses) {
                    ko.applyBindings(new OrderApprovalViewModel(locations, suppliers, suppliersnowh, warehouses), document.getElementById("dynamicContent"));
                });
            });
        });
    });

    function PurchaseOrderHeader(data) {
        var self = this;

        self.PurchaseOrderHeaderRefNo = data.PurchaseOrderHeaderRefNo;
        self.PurchaseOrderDate = new Date(parseInt(data.PurchaseOrderDate.replace("/Date(", "").replace(")/", "")));
        self.Status = ko.observable(data.Status);
        self.Remark = ko.observable(data.Remark);
        self.InventoryLocationID = data.InventoryLocationID;
        self.InventoryLocationName = data.InventoryLocation.InventoryLocationName;
        self.ApprovalDate = new Date(parseInt(data.ApprovalDate.replace("/Date(", "").replace(")/", "")));

        if (data.DateNeededBy)
            self.DateNeededBy = new Date(parseInt(data.DateNeededBy.replace("/Date(", "").replace(")/", "")));
        else
            self.DateNeededBy = null;

        self.poType = data.poType;
        self.PurchaseOrderDateFormatted = ko.computed(function() {
            return new Date(self.PurchaseOrderDate).toString(dateDisplayFormatWithTime);
        });

        self.DateNeededByFormatted = ko.computed(function() {
            if (self.DateNeededBy)
                return new Date(self.DateNeededBy).toString(dateDisplayFormatWithTime);
            else
                return "";
        });

        self.ApprovalDateFormatted = ko.computed(function() {
            return new Date(self.ApprovalDate).toString(dateDisplayFormatWithTime);

            //self.AutoStockIn = ko.Observable("No");

        });

        self.SalesPersonID = ko.observable("");
        self.SalesPersonName = ko.observable("");

        if (data.SalesPerson != null) {
            self.SalesPersonID = ko.observable(data.SalesPerson.UserName);
            self.SalesPersonName = ko.observable(data.SalesPerson.DisplayName);
        }

        self.WarehouseID = data.WarehouseID;
        self.PriceLevel = data.PriceLevel;
        self.ShipVia = ko.observable(data.ShipVia);
        self.OrderFromName = data.OrderFromName;

        self.OrderFrom = ko.computed(function() {
            if (data.WarehouseID == null || data.WarehouseID == 0)
                return "supplier"
            else
                return "warehouse";
        });

        self.SupplierID = data.SupplierID;
    };

    function PurchaseOrderDetail(data) {
        var self = this;

        self.PurchaseOrderHeaderRefNo = data.PurchaseOrderHeaderRefNo;
        self.PurchaseOrderDetailRefNo = data.PurchaseOrderDetailRefNo;
        self.CategoryName = data.CategoryName;
        self.ItemNo = data.ItemNo;
        self.ItemName = data.ItemName;
        self.QtyInStock = data.QtyInStock;
        self.QtyInWH = data.QtyInWH;
        self.OptimalStock = data.OptimalStock;
        self.Quantity = data.Quantity;
        self.Remark = ko.observable(data.Remark);
        self.Status = ko.observable(data.Status);
        self.UOM = data.UOM;
        self.BaseLevel = data.BaseLevel;

        if (!data.Status) {
            self.QtyApproved = ko.observable(data.Quantity);
        }
        else {
            self.QtyApproved = ko.observable(data.QtyApproved);
        };

        self.isSelected = ko.observable(false);
        self.SalesQty = data.SalesQty;
        self.Amount = ko.observable(data.Amount);
        self.GSTAmount = ko.observable(data.GSTAmount);
        self.DiscountDetail = data.DiscountDetail;

        self.GSTAmountString = ko.computed(function() {
            return self.GSTAmount() == null || self.GSTAmount().length === 0 ? "" : parseFloat(self.GSTAmount()).toFixed(2);
        });

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
    function OrderApprovalViewModel(locations, suppliers, suppliersnowh, warehouses) {
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

        self.statusDropdown = ["ALL", "Submitted", "Approved", "Received", "Rejected", "Posted"];
        self.savedFilter = null;

        self.selectedDate = ko.observable(self.dateDropdown[0]);
        self.status = ko.observable("Submitted");
        self.purchaseOrderHeaders = ko.observableArray();
        self.purchaseOrderDetails = ko.observableArray();
        self.currentPurchaseOrder = ko.observable();
        self.BackPurchaseOrder = ko.observable();
        self.isReadOnly = ko.observable(false);
        self.lblSalesPersonName = ko.observable("Beauty Advisor");

        self.showLoadMore = ko.observable(false);
        self.showOnlyWHQtyMaster = ko.observable(false);
        self.sortKeyPOHdr = ko.observable({ columnName: "PurchaseOrderDate", asc: false });
        self.sortKeyPODet = ko.observable({ columnName: "ItemNo", asc: true });
        //self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.inventoryLocationList = ko.observableArray(locations);
        self.inventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function(item) {
            //return item.InventoryLocationID == (self.allowChangeClinic ? 0 : sessionStorage.locationID);
            return item.InventoryLocationID == sessionStorage.locationID;
        }));
        self.inventoryLocationFilterText = ko.observable(self.inventoryLocation().InventoryLocationName);
        //self.proceedToPrint = ko.observable(false);
        //self.autoStockIn = ko.Observable("No");
        self.radioSelectedOptionValue = ko.observable("No");
        self.radioSelectedEditable = ko.observable(false);
        self.IsAutoStockIn = ko.observable("No");

        self.potypeDropdown = ["Replenish", "Back Order", "Extra Order"];
        self.typepo = ko.observable("Replenish");
        self.searchOrderNumber = ko.observable("");

        if (sessionStorage.isRestrictedSupplierList.toUpperCase() == "TRUE")
            self.supplierList = ko.observableArray(ko.utils.arrayFilter(suppliersnowh, function(item) {
                return item.SupplierName != "ALL";
            })); // if isRestrictedSupplierList don't show "ALL" option
        else
            self.supplierList = ko.observableArray(suppliersnowh);

        self.warehouseList = ko.observableArray(warehouses);

        self.supplierListSearch = ko.observableArray(suppliers);
        if (suppliers.length > 0) {
            self.supplierListSearchFilterText = ko.observable(suppliers[0].SupplierName);
            self.supplierListSearchFilter = ko.observable(suppliers[0]);
        }
        else {
            self.supplierListSearchFilterText = ko.observable("");
            self.supplierListSearchFilter = ko.observable();
        }

        self.supplierListSearchFiltered = ko.computed(function() {
            if (self.supplierListSearchFilterText() && self.supplierListSearchFilterText().toUpperCase() != "ALL") {
                return ko.utils.arrayFilter(self.supplierListSearch(), function(item) {
                    return item.SupplierName.toUpperCase().indexOf(self.supplierListSearchFilterText().toUpperCase()) == 0;
                });
            }
            else {
                return self.supplierListSearch();
            };
        });

        self.supplierListSearchFilterTextKeyUp = function(obj, e) {
            if (self.supplierListSearchFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');
        };

        self.changeSupplierListSearch = function(location) {
            self.supplierListSearchFilter(location);
            self.supplierListSearchFilterText(location.SupplierName);
        };

        self.displayDate = ko.computed(function() {
            return self.selectedDate().startDate.toString(dateDisplayFormat) + " - " + self.selectedDate().endDate.toString(dateDisplayFormat);
        });

        self.IsCanPrint = ko.observable(false);

        if (sessionStorage.isSupplier == "true") {
            self.showOrderFromSupplier = ko.observable(true);
            self.showOrderFromWarehouse = ko.observable(false);
        }
        else {
            self.showOrderFromWarehouse = ko.observable(true);
            self.showOrderFromSupplier = ko.observable(false);
            DAL.fnGetSetting("GoodsOrdering_AllowOutletToOrderFromSupplier", "yes", function(res) {
                if (res == "True") {
                    self.showOrderFromSupplier = ko.observable(true);
                };
            });
        }

        self.supplierName = ko.observable("");
        self.warehouseName = ko.observable("");

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

        self.IsAllowOrderWhenQtyNotSuffice = ko.observable(false);
        DAL.fnGetSetting("GoodsOrdering_AllowDeductInvQtyNotSufficient", "yes", function(res) {
            if (res == "True") {
                self.IsAllowOrderWhenQtyNotSuffice(true);
            };
        });

        self.changeRemarks = function() {
            if (!self.isReadOnly()) {
                DAL.fnChangeRemarkHeader(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, self.currentPurchaseOrder().Remark(), function(result) {
                    if (result.status == "") {
                    }
                    else {
                        BootstrapDialog.alert(result.status);
                    };
                });
            }
        };

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
        DAL.fnGetSetting("GoodsOrdering_ShowFactoryPriceInOrderApproval", "yes", function(res) {
            if (res == "True") {
                self.showFactoryPrice(true);
            };
        });

        self.showPrintDO = ko.observable(false);
        DAL.fnGetSetting("GoodsOrdering_ShowPrintDOButtonInGoodsOrdering", "yes", function(res) {
            if (res == "True") {
                self.showPrintDO(true);
            };
        });

        self.loadData = function() {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                if (self.supplierListSearchFilter().SupplierName.toUpperCase() != self.supplierListSearchFilterText().toUpperCase()) return;
                var supplierID = self.supplierListSearchFilterText() == "ALL" || self.supplierListSearchFilterText() == "" ? "0" : self.supplierListSearchFilter().SupplierID;

                filter = {
                    frompage: "OrderApproval",
                    startdate: self.selectedDate().startDate,
                    enddate: self.selectedDate().endDate,
                    inventorylocationid: self.inventoryLocation().InventoryLocationID,
                    status: self.status(),
                    potype: self.typepo(),
                    showBackOrderWithReadyStock: self.showOnlyWHQtyMaster(),
                    supplierid: supplierID,
                    purchaseorderheaderrefno: self.searchOrderNumber()
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
                BootstrapDialog.alert("Please select a valid outlet.");
                return;
            };
            self.showOnlyWHQtyMaster(false);
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
                    self.radioSelectedOptionValue(orderHdr.WarehouseID == 0 ? "No" : "Yes");
                    self.IsCanPrint(orderHdr.Status() == "Submitted" || orderHdr.Status() == 'Approved' || orderHdr.Status() == 'Received' || orderHdr.Status() == 'Rejected' || orderHdr.Status() == 'Posted' ? true : false);

                    self.currentPurchaseOrder(orderHdr);
                    self.purchaseOrderDetails.removeAll();
                    self.loadPurchaseOrderDetails(orderHdr.PurchaseOrderHeaderRefNo);

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

                    if (result.IsAutoStockIn != "" && result.IsAutoStockIn != null) {
                        self.radioSelectedOptionValue(result.IsAutoStockIn);
                    }
                    else {
                        self.radioSelectedOptionValue(self.IsAutoStockIn());
                    }

                    var sup = ko.utils.arrayFirst(self.supplierList(), function(item) {
                        return item.SupplierID == orderHdr.SupplierID;
                    });
                    if (sup) {
                        self.supplierName(sup.SupplierName);
                    }
                    else {
                        self.supplierName("ALL");
                    };

                    var war = ko.utils.arrayFirst(self.warehouseList(), function(item) {
                        return item.SupplierID == orderHdr.SupplierID;
                    });
                    if (war) {
                        self.warehouseName(war.SupplierName);
                        self.supplierName("");
                    }
                    else {
                        self.warehouseName("");
                    };

                    $('#tableApproved').freezeTable('update');
                    $('#tableRejected').freezeTable('update');
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
                            CategoryName: result.records[i].PurchaseOrderDetail.Item.CategoryName,
                            ItemNo: result.records[i].PurchaseOrderDetail.ItemNo,
                            ItemName: result.records[i].PurchaseOrderDetail.Item.ItemName,
                            QtyInStock: result.records[i].StockBalance,
                            QtyInWH: result.records[i].WarehouseBalance,
                            OptimalStock: result.records[i].OptimalStock,
                            Quantity: result.records[i].PurchaseOrderDetail.Quantity,
                            FactoryPrice: result.records[i].PurchaseOrderDetail.FactoryPrice,
                            QtyApproved: result.records[i].PurchaseOrderDetail.QtyApproved,
                            Remark: result.records[i].PurchaseOrderDetail.Remark,
                            Status: result.records[i].PurchaseOrderDetail.Status,
                            UOM: result.records[i].PurchaseOrderDetail.Item.UOM,
                            BaseLevel: (result.records[i].PurchaseOrderDetail.Item.BaseLevel == "0") ? "1" : result.records[i].PurchaseOrderDetail.Item.BaseLevel,
                            Amount: result.records[i].PurchaseOrderDetail.Amount,
                            GSTAmount: result.records[i].PurchaseOrderDetail.GSTAmount,
                            DiscountDetail: result.records[i].PurchaseOrderDetail.DiscountDetail,
                            SalesQty: result.records[i].PurchaseOrderDetail.SalesQty,
                            P1Price: result.records[i].PurchaseOrderDetail.Item.P1Price,
                            P2Price: result.records[i].PurchaseOrderDetail.Item.P2Price,
                            P3Price: result.records[i].PurchaseOrderDetail.Item.P3Price,
                            P4Price: result.records[i].PurchaseOrderDetail.Item.P4Price,
                            P5Price: result.records[i].PurchaseOrderDetail.Item.P5Price
                        };
                        self.purchaseOrderDetails.push(new PurchaseOrderDetail(poDet));
                    };

                    //updateFactoryPrice();

                    self.sortKeyPODet({ columnName: "ItemNo", asc: false });
                    self.sortPODet('ItemNo');

                    $("#divBackToTop").backToTop({
                        scrollTop: $("#backToFirstProduct").position().top - 70,
                        offset: $("#backToFirstProduct").position().top - 70
                    });

                    $('#tableApproved').freezeTable('update');
                    $('#tableRejected').freezeTable('update');

                    //if (self.proceedToPrint()) {
                    //    self.proceedToPrint(false);
                    //    window.setTimeout(self.print, 500);
                    //};
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

        self.isLoadFromSales = ko.computed(function() {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function(item) {
                return item.SalesQty > 0;
            }).length > 0;
        });

        self.submitApproval = function() {

            var poApproval = function() {
                DAL.fnPurchaseOrderApproval(JSON.parse(ko.toJSON(self.purchaseOrderDetails())), self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, sessionStorage.username, self.radioSelectedOptionValue(), self.selectedPriceLevelKey(), function(result) {
                    if (result.status == "") {
                        //alert("Done approve " + self.currentPurchaseOrder().PurchaseOrderHeaderRefNo);
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
                        BootstrapDialog.alert(result.status);
                    };
                });
            };

            if (self.showPriceLevel() && !self.selectedPriceLevelKey()) {
                BootstrapDialog.alert("Please select Price Level first.");
                return;
            }

            BootstrapDialog.confirm("Are you sure you want to submit this Approval?", function(ok) {
                if (ok) {
                    self.isWHgotBalance = false;

                    if (self.currentPurchaseOrder().WarehouseID == 0) {
                        self.isWHgotBalance = true;
                    }
                    else {
                        if (self.IsAllowOrderWhenQtyNotSuffice() == false) {
                            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                                var poDet = self.purchaseOrderDetails()[i];
                                if (poDet.QtyInWH > 0) {
                                    self.isWHgotBalance = true;
                                }
                            }
                        }
                        else {
                            self.isWHgotBalance = true;
                        }
                    }

                    if (self.isWHgotBalance == false) {
                        BootstrapDialog.alert("There are no items with sufficient Warehouse Balance");
                        return;
                    }
                    self.askBackOrder = false;

                    for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                        var poDet = self.purchaseOrderDetails()[i];

                        if (poDet.QtyApproved() > poDet.QtyInWH) {
                            if (self.currentPurchaseOrder().poType != "Back Order") {
                                self.askBackOrder = true;
                            }
                        };

                        if (poDet.BaseLevel != 1 && (Math.ceil(poDet.QtyApproved() / poDet.BaseLevel) != (poDet.QtyApproved() / poDet.BaseLevel))) {
                            BootstrapDialog.alert(poDet.ItemName + ": Approved Quantity is not a multiply of base level value");
                            return;
                        };

                        /**
                        if (poDet.QtyApproved() != poDet.Quantity && (!poDet.Remark() || poDet.Remark().trim() == "")) {
                        BootstrapDialog.alert(poDet.ItemName + ": Remarks is mandatory if Approved Quantity is not equal to Ordered Quantity");
                        return;
                        };*/

                        if (poDet.QtyApproved() > 0) {
                            poDet.Status("Approved");
                        }
                        else {
                            poDet.Status("Rejected");
                        };
                    };

                    if (self.currentPurchaseOrder().WarehouseID == 0) {
                        self.askBackOrder = false;
                    } else if (self.IsAllowOrderWhenQtyNotSuffice() == true) {
                        self.askBackOrder = false;
                    }

                    if (self.askBackOrder == true) {
                        BootstrapDialog.confirm("Do you want to create back order?", function(ok) {
                            if (ok) {
                                poApproval();
                            }
                        });
                    }
                    else {
                        poApproval();
                    }

                }
            });
        };

        self.changeCreditInvoiceNo = function(o) {
            DAL.fnChangeCreditInvoiceNo(o.PurchaseOrderHeaderRefNo, o.ShipVia(), function(result) {
                if (result.status != "") {
                    BootstrapDialog.alert(result.status);
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

            BootstrapDialog.confirm("Are you sure you want to return this PO to the pending status?", function(ok) {
                if (ok) {
                    var c = 0;
                    for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                        var poDet = self.purchaseOrderDetails()[i];
                        if (poDet.Status() == "Rejected")
                            c++;
                    }

                    if (c == 0)
                        poReturnToPending();
                    else
                        BootstrapDialog.alert("There is a rejected item. This process is cancelled.");
                }
            });
        };

        self.changeInventoryLocation = function(location) {
            self.inventoryLocation(location);
            self.inventoryLocationFilterText(location.InventoryLocationName);
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
                //if (qty > poDet.QtyInWH) {
                //    BootstrapDialog.alert(poDet.ItemName + ": Approved Quantity is greater than Warehouse Balance");
                //    return;
                //};

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
                    } else {
                        poDet.QtyApproved(result.detail.QtyApproved);
                        if (result.detail.Amount != null) {
                            poDet.Amount(result.detail.Amount);
                            poDet.GSTAmount(result.detail.GSTAmount);
                        }
                    }
                });
            };

            var updateApprovedQty = function(qty) {
                poDet.QtyApproved(qty);
                DAL.fnChangeApprovedPOApprovedQty(JSON.parse(ko.toJSON(poDet)), function(result) {
                    if (result.status != "") {
                        BootstrapDialog.alert(result.status);
                    }
                    else {
                        poDet.QtyApproved(result.detail.QtyApproved);
                        if (result.detail.Amount != null) {
                            poDet.Amount(result.detail.Amount);
                            poDet.GSTAmount(result.detail.GSTAmount);
                        }
                    }
                });
            }

            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Approved Quantity:", poDet.QtyApproved(), function(qty) {
                    if (qty != null && !isNaN(qty)) {
                        qty = parseFloat(qty);
                        // var roundedQty = Math.ceil(qty / poDet.BaseLevel) * poDet.BaseLevel;

                        // if (Math.ceil(qty / poDet.BaseLevel) != (qty / poDet.BaseLevel)) {
                        //     BootstrapDialog.confirm("Base level is " + poDet.BaseLevel + ". Quantity will be changed to " + roundedQty, function(ok) {
                        //         if (ok) {
                        //             qty = roundedQty;
                        //             updateQty(qty);
                        //         }
                        //         else {
                        //             return;
                        //         };
                        //     });
                        // }
                        // else {
                        updateQty(qty);
                        // };
                    };
                });
            }
            // Note: Editing approved qty is disabled for now
            else if (self.currentPurchaseOrder().Status() == "Approved") {
                BootstrapDialog.prompt("Approved Quantity:", poDet.QtyApproved(), function(qty) {
                    if (qty != null && !isNaN(qty)) {
                        qty = parseFloat(qty);
                        //             // var roundedQty = Math.ceil(qty / poDet.BaseLevel) * poDet.BaseLevel;

                        //             // if (Math.ceil(qty / poDet.BaseLevel) != (qty / poDet.BaseLevel)) {
                        //             //     BootstrapDialog.confirm("Base level is " + poDet.BaseLevel + ". Quantity will be changed to " + roundedQty, function(ok) {
                        //             //         if (ok) {
                        //             //             qty = roundedQty;
                        //             //             updateApprovedQty(qty);
                        //             //         }
                        //             //         else {
                        //             return;
                        //         };
                        //     });
                        // }
                        // else {
                        updateApprovedQty(qty);
                        // };
                    };
                });
            }
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

        self.setTotalCost = function(poDet) {
            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Total Cost:", poDet.TotalCost(), function(totalCost) {
                    if (totalCost != null && !isNaN(totalCost)) {
                        totalCost = parseFloat(totalCost);
                        var price = totalCost / poDet.QtyApproved();
                        price = parseFloat(price.toFixed(4));
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

        self.changeShowOnlyItem = function() {

            self.showOnlyWHQtyMaster(true);

            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function(item) {
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
        };


        self.purchaseOrderDetailsApprovedOrPending = ko.computed(function() {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function(item) {

                //if (self.showOnlyWHQtyMaster())
                //{
                //   return item.Status() != "Rejected" && item.QtyInWH > 0;
                //}
                //else
                //{
                return item.Status() != "Rejected";
                //}
            });
        });



        self.purchaseOrderDetailsRejected = ko.computed(function() {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function(item) {

                return item.Status() == "Rejected";

            });
        });

        self.totalAmountApprovedOrPending = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].Status() != "Rejected") {
                    if (self.purchaseOrderDetails()[i].Amount() != null && parseFloat(self.purchaseOrderDetails()[i].Amount()) > 0) {
                        total += parseFloat(self.purchaseOrderDetails()[i].Amount());
                    }
                }
            }

            return total.toFixed(2);
        });

        self.totalGSTAmountApprovedOrPending = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].Status() != "Rejected") {
                    if (self.purchaseOrderDetails()[i].GSTAmount() != null && parseFloat(self.purchaseOrderDetails()[i].GSTAmount()) > 0) {
                        total += parseFloat(self.purchaseOrderDetails()[i].GSTAmount());
                    }
                }
            }

            return total.toFixed(2);
        });

        self.totalAmountRejected = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].Status() == "Rejected") {
                    if (self.purchaseOrderDetails()[i].Amount() != null && parseFloat(self.purchaseOrderDetails()[i].Amount()) > 0) {
                        total += parseFloat(self.purchaseOrderDetails()[i].Amount());
                    }
                }
            }

            return total.toFixed(2);
        });

        self.totalGSTAmountRejected = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].Status() == "Rejected") {
                    if (self.purchaseOrderDetails()[i].GSTAmount() != null && parseFloat(self.purchaseOrderDetails()[i].GSTAmount()) > 0) {
                        total += parseFloat(self.purchaseOrderDetails()[i].GSTAmount());
                    }
                }
            }

            return total.toFixed(2);
        });

        self.totalOrderedQtyApprovedOrPending = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].Status() != "Rejected") {
                    if (self.purchaseOrderDetails()[i].Quantity != null && parseFloat(self.purchaseOrderDetails()[i].Quantity) > 0) {
                        total += parseFloat(self.purchaseOrderDetails()[i].Quantity);
                    }
                }
            }

            return parseFloat(total.toFixed(4));
        });

        self.totalOrderedQtyRejected = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].Status() == "Rejected") {
                    if (self.purchaseOrderDetails()[i].Quantity != null && parseFloat(self.purchaseOrderDetails()[i].Quantity) > 0) {
                        total += parseFloat(self.purchaseOrderDetails()[i].Quantity);
                    }
                }
            }

            return parseFloat(total.toFixed(4));
        });

        self.totalApprovedQtyApprovedOrPending = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].Status() != "Rejected") {
                    if (self.purchaseOrderDetails()[i].QtyApproved() != null && parseFloat(self.purchaseOrderDetails()[i].QtyApproved()) > 0) {
                        total += parseFloat(self.purchaseOrderDetails()[i].QtyApproved());
                    }
                }
            }

            return parseFloat(total.toFixed(4));
        });

        self.totalApprovedQtyRejected = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].Status() == "Rejected") {
                    if (self.purchaseOrderDetails()[i].QtyApproved() != null && parseFloat(self.purchaseOrderDetails()[i].QtyApproved()) > 0) {
                        total += parseFloat(self.purchaseOrderDetails()[i].QtyApproved());
                    }
                }
            }

            return parseFloat(total.toFixed(4));
        });

        self.totalCostApprovedOrPending = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].Status() != "Rejected") {
                    if (self.purchaseOrderDetails()[i].TotalCost() != null && parseFloat(self.purchaseOrderDetails()[i].TotalCost()) > 0) {
                        total += parseFloat(self.purchaseOrderDetails()[i].TotalCost());
                    }
                }
            }

            return total.toFixed(2);
        });

        self.totalCostRejected = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].Status() == "Rejected") {
                    if (self.purchaseOrderDetails()[i].TotalCost() != null && parseFloat(self.purchaseOrderDetails()[i].TotalCost()) > 0) {
                        total += parseFloat(self.purchaseOrderDetails()[i].TotalCost());
                    }
                }
            }

            return total.toFixed(2);
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

        self.print = function(PurchaseOrderHeaderRefNo, Status) {
            var urlReport = "";
            // if (Status == 'Received')
            //     urlReport = settings.crReportLocation + "?r=GRPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&ShowBarcode=true&DocNo=" + PurchaseOrderHeaderRefNo;
            // else
            urlReport = settings.crReportLocation + "?r=OrderPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&ShowBarcode=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
            //window.print();
        };

        self.printDO = function(PurchaseOrderHeaderRefNo, Status) {
            var urlReport = "";
            urlReport = settings.crReportLocation + "?r=DeliveryOrderPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&ShowBarcode=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
            //window.print();
        };

        self.printFromDetails = function() {
            self.print(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, self.currentPurchaseOrder().Status());
        };

        self.printDOFromDetails = function() {
            self.printDO(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, self.currentPurchaseOrder().Status());
        };

        //self.openDetailsAndPrint = function (order) {
        //    self.proceedToPrint(true);
        //    self.openUpdatePage(order);
        //};

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

        self.GetPurchaseHeaderSetting = function() {
            DAL.fnGetPurchaseHeaderSetting(function(result) {
                if (result.status == "") {
                    self.lblSalesPersonName(result.TextBeautyAdvisors);

                    if (result.EditableAutoStockIn.toUpperCase() == 'FALSE') {
                        self.radioSelectedEditable(false);
                    }
                    else {
                        self.radioSelectedEditable(true);
                    }

                    if (result.IsAutoStockIn.toUpperCase() == 'FALSE') {
                        self.radioSelectedOptionValue("No");
                        self.IsAutoStockIn("No");
                    }
                    else {
                        self.radioSelectedOptionValue("Yes");
                        self.IsAutoStockIn("Yes");
                    }
                } else {
                    BootstrapDialog.alert(result.status)
                }
            });
        }

        self.GetPurchaseHeaderSetting();

        self.loadData();

    };

    $("form").submit(function(e) {
        e.preventDefault();
    });
});

