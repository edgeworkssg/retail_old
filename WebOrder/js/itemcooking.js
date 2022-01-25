/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />
/// <reference path="bootstrap-toggle.min.js" />

$(function() {
    $('#tableDetail').freezeTable({
        'freezeColumn': false,
        'freezeHead': true,
        'namespace': 'tableDetail'
    });

    $('#tableSearch').freezeTable({
        'freezeColumn': false,
        'freezeHead': true,
        'namespace': 'tableSearch'
    });

    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;
    var dateDisplayFormat = settings.dateDisplayFormat;

    $("#divPurchaseOrderModify").hide();
    $("#divPurchaseOrderDetail").hide();

    DAL.fnGetInventoryLocationList(function(locations) {
        //locations.unshift({ InventoryLocationID: "0", InventoryLocationName: "ALL" });
        ko.applyBindings(new ItemCookingViewModel(locations), document.getElementById("dynamicContent"));
    });

    function ItemCookDetail(data) {
        var self = this;

        self.ItemCookDetailID = data.ItemCookDetailID;
        self.ItemCookHistoryID = data.ItemCookHistoryID;
        self.ItemNo = ko.observable(data.ItemNo);
        self.ItemName = ko.observable(data.ItemName);
        self.UOM = ko.observable(data.UOM);
        self.Qty = ko.observable(data.Qty);
        self.OriginalQty = ko.observable(data.OriginalQty);
        self.UnitPrice = ko.observable(data.UnitPrice);
        self.TotalCost = ko.observable(data.TotalCost);
        self.isSelected = ko.observable(false);
        self.IsLoadFromRecipe = data.IsLoadFromRecipe;

        self.QtyWithFormat = ko.computed(function() {
            return parseFloat(self.Qty()).toFixed(2);
        });

        self.TotalCostWithFormat = ko.computed(function() {
            return parseFloat(self.TotalCost()).toFixed(2);
        });
    }

    function ItemCookDetailView(data) {
        var self = this;

        self.ProductName = data.ProductName;
        self.ProductNo = data.ProductNo;
        self.ProductUsedNo = ko.observable(data.ProductUsedNo);
        self.ProductUsedName = ko.observable(data.ProductUsedName);
        self.ProductUsedQty = ko.observable(data.ProductUsedQty);
        self.ProductUsedUOM = ko.observable(data.ProductUsedUOM);
        self.ProductUsedUnitPrice = ko.observable(data.ProductUsedUnitPrice);
        self.ProductUsedCOG = ko.computed(function() {
            var retailprice = 0;
            var qty = 0;

            if (self.ProductUsedQty() != null)
                qty = parseFloat(self.ProductUsedQty());

            if (self.ProductUsedUnitPrice() != null)
                retailprice = parseFloat(self.ProductUsedUnitPrice());

            return qty * retailprice;
        });
    }

    function ItemCookHistory(data) {
        var self = this;

        self.DocumentNo = data.DocumentNo;
        self.Status = data.Status;
        self.COG = data.COG;
        self.ItemCookHistoryID = data.ItemCookHistoryID;
        self.CookDate = new Date(parseInt(data.CookDate.replace("/Date(", "").replace(")/", "")));
        self.CookDateFormatted = ko.computed(function() {
            return new Date(self.CookDate).toString(dateDisplayFormatWithTime);
        });
        self.InventoryLocationID = data.InventoryLocationID;
        self.InventoryLocationName = data.InventoryLocationName;
        self.DepartmentName = data.DepartmentName;
        self.CategoryName = data.CategoryName;
        self.ItemName = data.ItemName;
        self.ItemNo = ko.observable(data.ItemNo);
        self.Quantity = ko.observable(data.Quantity);
    };

    function ItemDepartment(data) {
        var self = this;
        self.ItemDepartmentID = data.ItemDepartmentID;
        self.DepartmentName = data.DepartmentName;
        self.DepartmentOrder = data.DepartmentOrder;
        self.isSelected = ko.observable(false);
    };

    function ItemToAdd(data) {
        var self = this;
        self.data = data;
    };

    // Knockout.js ViewModel
    function ItemCookingViewModel(locations) {
        var self = this;

        self.inventoryLocationList = ko.observableArray(locations);
        self.inventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function(item) {
            return item.InventoryLocationID == sessionStorage.locationID;
        }));

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
        self.selectedDate = ko.observable(self.dateDropdown[0]);

        self.displayDate = ko.computed(function() {
            return self.selectedDate().startDate.toString(dateDisplayFormat) + " - " + self.selectedDate().endDate.toString(dateDisplayFormat);
        });

        self.statusDropdown = ["ALL", "Pending", "Completed", "Cancelled"];

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


        self.itemCookHistories = ko.observableArray();
        self.showLoadMore = ko.observable(false);
        self.showLoadMoreItems = ko.observable(false);
        self.searchItemResults = ko.observableArray();
        self.savedFilter = null;
        self.savedItemFilter = null;
        self.selectedItemNo = ko.observable("");
        self.selectedItem = ko.observable();
        self.quantity = ko.observable("");
        self.searchOrderNumber = ko.observable("");
        self.status = ko.observable("ALL");
        self.sortKeyPOHdr = ko.observable({ columnName: "CookDate", asc: false });
        self.sortKeyPODet = ko.observable({ columnName: "ItemName", asc: true });

        self.currentItemCook = ko.observable();
        self.itemCookDetails = ko.observableArray();
        self.isReadOnly = ko.observable(false);

        self.CurrentProductDetail = ko.observable();
        self.IsAddItemToHeader = ko.observable(true);
        self.TitleSearchDialog = ko.observable("");
        self.HeaderSearchItemNo = ko.observable("");
        self.HeaderSearchItemName = ko.observable("");

        self.isItemNotFound = ko.computed(function() {
            return self.searchItemResults().length < 1;
        });

        self.inventoryLocationText = ko.observable(self.inventoryLocation().InventoryLocationName);
        self.inventoryLocationTextKeyUp = function(obj, e) {
            if (self.inventoryLocationList().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');
        };

        self.changeInventoryLocation = function(location) {
            self.inventoryLocation(location);
            self.inventoryLocationText(location.InventoryLocationName);
        };

        self.openSearchProductDialogFromHeader = function() {
            self.IsAddItemToHeader(true);
            self.searchItemResults.removeAll();
            self.showLoadMoreItems(false);
            self.TitleSearchDialog("Search Product");
            self.HeaderSearchItemNo("Product No");
            self.HeaderSearchItemName("Product Name");
            $("#searchProductModal").modal({ backdrop: 'static' });
        }

        self.openSearchProductDialog = function() {
            self.IsAddItemToHeader(false);
            self.searchItemResults.removeAll();
            self.showLoadMoreItems(false);
            self.TitleSearchDialog("Search Product Used");
            self.HeaderSearchItemNo("Product Used No");
            self.HeaderSearchItemName("Product Used Name");
            $("#searchProductModal").modal({ backdrop: 'static' });
        };

        self.searchItem = function() {
            self.searchItemResults.removeAll();
            self.showLoadMoreItems(false);
            self.savedItemFilter = null;
            self.loadSearchItem();

            $("#txtSearchItem").val("");
        };

        self.ChangeItem = function() {
            if (self.selectedItemNo() == '') {
                BootstrapDialog.alert("Please select item");
                return;
            }

            if (self.IsAddItemToHeader()) {

                var cookHist = {
                    DocumentNo: self.currentItemCook().DocumentNo,
                    Status: self.currentItemCook().Status,
                    COG: self.currentItemCook().COG,
                    ItemCookHistoryID: self.currentItemCook().ItemCookHistoryID,
                    CookDate: self.currentItemCook().CookDate.toString(dateValueFormat),
                    InventoryLocationID: self.currentItemCook().InventoryLocationID,
                    InventoryLocationName: self.currentItemCook().InventoryLocationName,
                    DepartmentName: self.currentItemCook().DepartmentName,
                    CategoryName: self.currentItemCook().CategoryName,
                    ItemName: "",
                    ItemNo: self.selectedItemNo(),
                    Quantity: self.currentItemCook().Quantity
                };

                self.currentItemCook().ItemNo(self.selectedItemNo());

                DAL.fnSaveItemCookHistory(JSON.stringify(cookHist), function(result) {
                    if (result.status == "") {
                        var hdr = new ItemCookHistory(result.ItemCookHistory)

                        self.currentItemCook(hdr);
                        self.isReadOnly(self.currentItemCook().Status == "Pending" ? false : true);

                        self.itemCookDetails.removeAll();

                        for (var i = 0; i < result.detail.length; i++) {
                            self.itemCookDetails.push(new ItemCookDetail(result.detail[i]));
                        };

                    } else {
                        BootstrapDialog.alert(result.status);
                    }
                });
            } else {
                self.CurrentProductDetail().ProductUsedNo(self.selectedItemNo());
                self.CurrentProductDetail().ProductUsedName(self.selectedItem().ItemName);
                self.CurrentProductDetail().ProductUsedUOM(self.selectedItem().Uom);
                self.CurrentProductDetail().ProductUsedUnitPrice(self.selectedItem().FactoryPrice);
            }

        };

        self.loadSearchItem = function() {
            var filter;
            if (self.savedItemFilter) {
                filter = self.savedItemFilter;
            }
            else {
                filter = $("#txtSearchItem").val();
                self.savedItemFilter = filter;
            };

            if (self.IsAddItemToHeader()) {
                DAL.fnSearchRecipeMainItem(filter, false, self.searchItemResults().length, settings.numOfRecords, function(result) {
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
            } else {
                DAL.fnSearchRecipeMainItem(filter, false, self.searchItemResults().length, settings.numOfRecords, function(result) {
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
            }
        };

        self.isPOHdrNotFound = ko.computed(function() {
            return self.itemCookHistories().length < 1;
        });

        self.loadData = function() {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            } else {
                filter = {
                    documentNo: self.searchOrderNumber(),
                    inventoryLocationID: self.inventoryLocation().InventoryLocationID,
                    status: self.status(),
                    startdate: self.selectedDate().startDate.toString("yyyy-MM-dd"),
                    enddate: self.selectedDate().endDate.toString("yyyy-MM-dd")
                };
                self.savedFilter = filter;
            };

            DAL.fnFetchCookItem(JSON.stringify(filter), self.itemCookHistories().length, settings.numOfRecords, self.sortKeyPOHdr().columnName, self.sortKeyPOHdr().asc, function(data) {
                for (var i = 0; i < data.records.length; i++) {
                    self.itemCookHistories.push(new ItemCookHistory(data.records[i]));
                };

                if (data.totalRecords > self.itemCookHistories().length) {
                    self.showLoadMore(true);
                }
                else {
                    self.showLoadMore(false);
                };
            });

            $('#tableSearch').freezeTable('update');
        };

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
            self.loadData();
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

            self.itemCookDetails.sort(function(a, b) {
                var sortDirection = (self.sortKeyPODet().asc) ? 1 : -1;
                var valueA = ko.utils.unwrapObservable(a[self.sortKeyPODet().columnName]);
                var valueB = ko.utils.unwrapObservable(b[self.sortKeyPODet().columnName]);
                return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
            });
        };

        self.selectItem = function(item) {
            self.selectedItemNo(item.data.ItemNo);
            self.selectedItem(item.data);

            self.ChangeItem();
            $("#searchProductModal").modal("hide");
        };

        self.cookItem = function() {
            BootstrapDialog.confirm("Are you sure you want to produce the item?", function(ok) {
                if (ok) {
                    DAL.fnCookItemWithDetail(self.currentItemCook().ItemCookHistoryID, sessionStorage.PointOfSaleID, function(result) {
                        if (result.isSuccess) {

                            $("#divPurchaseOrderDetail").hide();
                            $("#divPurchaseOrderModify").hide();
                            $("#divPurchaseOrderList").fadeIn();

                            self.selectedItemNo("");
                            self.itemCookHistories.removeAll();
                            self.loadData();
                            BootstrapDialog.alert("Production success");
                        }
                        else {
                            BootstrapDialog.alert("Production failed : " + result.status);
                        }
                    });
                }
            });
        };

        self.cancelItemCook = function() {
            BootstrapDialog.confirm("Are you sure you want to cancel this Production?", function(ok) {
                if (ok) {
                    DAL.fnCancelItemCookHistory(self.currentItemCook().ItemCookHistoryID, function(result) {
                        if (result.status == "") {
                            $("#divPurchaseOrderDetail").hide();
                            $("#divPurchaseOrderModify").hide();
                            $("#divPurchaseOrderList").fadeIn();

                            self.selectedItemNo("");
                            self.itemCookHistories.removeAll();
                            self.loadData();
                            BootstrapDialog.alert("Production cancelled");
                        }
                        else {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                }
            });
        };

        self.openAddNewPage = function() {
            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function(item) {
                return item.InventoryLocationName.toUpperCase() == self.inventoryLocationText().toUpperCase() &&
                           item.InventoryLocationName.toUpperCase() != "ALL";
            });
            if (match) {
                self.inventoryLocation(match);
                self.inventoryLocationText(self.inventoryLocation().InventoryLocationName);
            }
            else {
                BootstrapDialog.alert("Please select a valid outlet.");
                return;
            };

            self.IsAddItemToHeader(true);

            // Initialize new PO Header
            var cookHist = {
                DocumentNo: "",
                Status: "Pending",
                COG: 0,
                ItemCookHistoryID: 0,
                CookDate: new Date().toString(dateValueFormat),
                InventoryLocationID: self.inventoryLocation().InventoryLocationID,
                InventoryLocationName: self.inventoryLocation().InventoryLocationName,
                DepartmentName: "",
                CategoryName: "",
                ItemName: "",
                ItemNo: "",
                Quantity: 0
            };

            self.itemCookDetails.removeAll();

            $("#divPurchaseOrderList").hide();
            $("#divPurchaseOrderDetail").hide();
            $("#divPurchaseOrderModify").fadeIn();

            $('#tableDetail').freezeTable('update');

            var hdr = new ItemCookHistory(cookHist);

            self.currentItemCook(hdr);
            self.isReadOnly(self.currentItemCook().Status == "Pending" ? false : true);
        };

        self.changeItemQty = function(detail) {
            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Used:", detail.Qty(), function(qty) {
                    if (qty != null && !isNaN(qty)) {
                        qty = parseFloat(qty);

                        DAL.fnChangeQtyItemCookDetail(detail.ItemCookDetailID, qty, function(result) {
                            if (result.status == "") {
                                detail.Qty(qty);
                                detail.TotalCost(parseFloat(detail.Qty()) * parseFloat(detail.UnitPrice()))
                            } else {
                                BootstrapDialog.alert(result.status);
                            }
                        });
                    };
                });
            }
        };

        self.openUpdatePage = function(order) {
            self.IsAddItemToHeader(true);

            DAL.fnGetItemCookHistory(order.DocumentNo, function(result) {
                if (result.status == "") {
                    $("#divPurchaseOrderList").hide();
                    $("#divPurchaseOrderDetail").hide();
                    $("#divPurchaseOrderModify").fadeIn();

                    var hdr = new ItemCookHistory(result.ItemCookHistory)

                    self.currentItemCook(hdr);
                    self.isReadOnly(self.currentItemCook().Status == "Pending" ? false : true);

                    self.itemCookDetails.removeAll();

                    for (var i = 0; i < result.detail.length; i++) {
                        self.itemCookDetails.push(new ItemCookDetail(result.detail[i]));
                    };

                    $('#tableDetail').freezeTable('update');
                }
                else {
                    BootstrapDialog.alert(result.status);
                }
            });
        };

        self.deleteSelectedPODet = function() {
            BootstrapDialog.confirm("Are you sure you want to delete the selected items?", function(ok) {
                if (ok) {
                    var itemsToDelete = ko.utils.arrayFilter(self.itemCookDetails(), function(item) {
                        return item.isSelected() == true;
                    });

                    for (var i = 0; i < itemsToDelete.length; i++) {
                        DAL.fnDeleteItemCookDetail(itemsToDelete[i], function(result, deletedItem) {
                            if (result.status == "") {

                                self.itemCookDetails.remove(deletedItem);

                                $('#tableDetail').freezeTable('update');

                                //                                $("#divBackToTop").backToTop({
                                //                                    scrollTop: $("#backToFirstProduct").position().top - 70,
                                //                                    offset: $("#backToFirstProduct").position().top - 70
                                //                                });
                            }
                            else {
                                BootstrapDialog.alert(result.status);
                            };
                        });

                    };
                };
            });
        };

        self.addItemToOrder = function() {
            var newDetail = {
                ProductName: self.currentItemCook().ItemName,
                ProductNo: self.currentItemCook().ItemNo,
                ProductUsedNo: "",
                ProductUsedName: "",
                ProductUsedQty: 0,
                ProductUsedUOM: "",
                ProductUsedUnitPrice: 0,
                ProductUsedCOG: 0
            };

            self.IsAddItemToHeader(false);

            self.CurrentProductDetail(new ItemCookDetailView(newDetail));


            $("#divPurchaseOrderList").hide();
            $("#divPurchaseOrderModify").hide();
            $("#divPurchaseOrderDetail").fadeIn();
        };

        self.backProductUsed = function() {
            $("#divPurchaseOrderList").hide();
            $("#divPurchaseOrderDetail").hide();
            $("#divPurchaseOrderModify").fadeIn();
            self.IsAddItemToHeader(true);
        }

        self.addProductUsed = function() {
            var qty = parseFloat(self.CurrentProductDetail().ProductUsedQty());

            if (self.CurrentProductDetail().ProductUsedNo() == "") {
                BootstrapDialog.alert("Please Select Product Used");
                return;
            }

            if (self.CurrentProductDetail().ProductUsedQty() && isNaN(self.CurrentProductDetail().ProductUsedQty())) {
                BootstrapDialog.alert("Invalid Quantity");
                return;
            }

            if (qty <= 0) {
                BootstrapDialog.alert("Quantity must greater than zero");
                return;
            }

            var match = ko.utils.arrayFirst(self.itemCookDetails(), function(item) {
                return item.ItemNo().trim().toUpperCase() == self.CurrentProductDetail().ProductUsedNo().toUpperCase();
            });
            if (match) {
                BootstrapDialog.alert("Can not insert duplicate Product Used");
                return;
            };

            var mainQty = 0;
            if (self.currentItemCook().Quantity() && !isNaN(self.currentItemCook().Quantity()))
                mainQty = parseFloat(self.currentItemCook().Quantity());

            var detail = {
                ItemCookDetailID: 0,
                ItemCookHistoryID: self.currentItemCook().ItemCookHistoryID,
                ItemNo: self.CurrentProductDetail().ProductUsedNo(),
                ItemName: self.CurrentProductDetail().ProductUsedName(),
                UOM: self.CurrentProductDetail().ProductUsedUOM(),
                Qty: self.CurrentProductDetail().ProductUsedQty(),
                OriginalQty: 1,
                UnitPrice: self.CurrentProductDetail().ProductUsedUnitPrice(),
                TotalCost: 0
            };

            DAL.fnAddItemCookDetail(detail, function(result) {
                if (result.status == "") {
                    self.itemCookDetails.push(new ItemCookDetail(result.detail));

                    $("#divPurchaseOrderList").hide();
                    $("#divPurchaseOrderDetail").hide();
                    $("#divPurchaseOrderModify").fadeIn();

                    $('#tableDetail').freezeTable('update');
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });

        };

        self.changeQtyHeader = function() {
            var qty = 0;
            if (self.currentItemCook().Quantity() && !isNaN(self.currentItemCook().Quantity()))
                qty = parseFloat(self.currentItemCook().Quantity());

            DAL.fnChangeQtyItemCookHistory(self.currentItemCook().ItemCookHistoryID, qty, function(result) {
                if (result.status == "") {
                    for (var i = 0; i < self.itemCookDetails().length; i++) {
                        if (self.itemCookDetails()[i].IsLoadFromRecipe == true) {
                            self.itemCookDetails()[i].Qty(qty * parseFloat(self.itemCookDetails()[i].OriginalQty()));
                            self.itemCookDetails()[i].TotalCost(qty * parseFloat(self.itemCookDetails()[i].OriginalQty()) * parseFloat(self.itemCookDetails()[i].UnitPrice()));
                        }
                    }
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.deleteCheckedBtnState = ko.computed(function() {
            var countSelected = ko.utils.arrayFilter(self.itemCookDetails(), function(item) {
                return item.isSelected() == true;
            }).length;

            return countSelected < 1;
        });

        self.isSelectAllDet = ko.computed(function() {
            var countSelected = ko.utils.arrayFilter(self.itemCookDetails(), function(item) {
                return item.isSelected() == true;
            }).length;
            var countVisible = self.itemCookDetails().length;
            return countSelected == countVisible;
        });

        self.toggleSelectAllDet = function() {
            var toggle = !self.isSelectAllDet();
            for (var i = 0; i < self.itemCookDetails().length; i++) {
                self.itemCookDetails()[i].isSelected(toggle);
            }
            return true;
        };


        self.totalCost = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.itemCookDetails().length; i++) {
                if (self.itemCookDetails()[i].TotalCost() != null && parseFloat(self.itemCookDetails()[i].TotalCost()) > 0) {
                    total += parseFloat(self.itemCookDetails()[i].TotalCost());
                }
            }

            return total.toFixed(2);
        });

        self.totalHeaderCost = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.itemCookDetails().length; i++) {
                if (self.itemCookDetails()[i].OriginalQty() != null && parseFloat(self.itemCookDetails()[i].OriginalQty()) > 0) {
                    total += parseFloat(self.itemCookDetails()[i].Qty()) * parseFloat(self.itemCookDetails()[i].UnitPrice());
                }
            }

            var finalTotal = 0;

            if (self.currentItemCook() != null && self.currentItemCook().Quantity() != 0) {
                finalTotal = total / self.currentItemCook().Quantity();
            }

            return finalTotal.toFixed(2);
        });

        self.backToList = function() {
            $("#divPurchaseOrderModify").hide();
            $("#divPurchaseOrderDetail").hide();
            $("#divPurchaseOrderList").fadeIn();
            self.searchCookItem();
        };

        self.searchCookItem = function() {
            self.itemCookHistories.removeAll();
            self.showLoadMore(false);
            self.selectedItemNo("");
            self.savedFilter = null;
            self.loadData();
        };

        self.loadData();
    };

    $("form").submit(function(e) {
        e.preventDefault();
    });
});

