/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />

$(function() {
    var dateDisplayFormat = settings.dateDisplayFormat;
    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;

    $("#divItemOptimalStockModify").hide();
    $("#divItemBaseLevelCopy").hide();
    $("#divSetByCategory").hide();

    DAL.fnGetInventoryLocationList(function(locations) {
        locations.unshift({ InventoryLocationID: "0", InventoryLocationName: "ALL" });
        ko.applyBindings(new ItemOptimalStockViewModel(locations), document.getElementById("dynamicContent"));
    });

    function ItemBaseLevel(data) {
        var self = this;

        self.BaseLevelID = data.BaseLevelID;
        self.ItemNo = ko.observable(data.ItemNo);
        self.ItemName = ko.observable(data.ItemName);
        self.BaseLevelQuantity = ko.observable(data.BaseLevelQuantity);
        self.InventoryLocationID = ko.observable(data.InventoryLocationID);
        self.InventoryLocationName = ko.observable(data.InventoryLocationName);
        self.Uom = ko.observable(data.Uom);
        self.Category = ko.observable(data.Category);
        self.isSelected = ko.observable(false);
    };

    function Category(data) {
        var self = this;

        self.CategoryName = data.CategoryName;
    };

    function Item(data) {
        var self = this;

        self.ItemNo = data.ItemNo;
        self.ItemName = data.ItemName;

    }

    function ItemToAdd(data) {
        var self = this;
        self.data = data;
    };

    // Knockout.js ViewModel
    function ItemOptimalStockViewModel(locations) {
        var self = this;

        self.savedFilter = null;
        self.savedItemFilter = null;

        self.searchOrderNumber = ko.observable("");
        self.itemBaseLevels = ko.observableArray();
        self.currentPurchaseOrder = ko.observable();
        self.currentItemBaseLevel = ko.observable();
        var newItem = new ItemBaseLevel({ BaseLevelID: "" });
        self.currentItemBaseLevel(newItem);
        self.searchItemResults = ko.observableArray();
        self.selectedItemNo = ko.observable("");
        self.selectedItemName = ko.observable("");
        self.expiryDate = ko.observable("");
        self.qtyToAdd = ko.observable("");
        self.isReadOnly = ko.observable(false);
        self.showLoadMore = ko.observable(false);

        self.showLoadMoreItems = ko.observable(false);
        self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.inventoryLocationList = ko.observableArray(locations);
        self.inventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function(item) {
            return item.InventoryLocationID == (self.allowChangeClinic ? 0 : sessionStorage.locationID);
        }));
        self.inventoryLocationFilterText = ko.observable(self.inventoryLocation().InventoryLocationName);
        self.inventoryLocationFilterTextDetail = ko.observable("");
        self.sortKeyPOHdr = ko.observable({ columnName: "ItemNo", asc: false });

        self.categoryList = ko.observableArray();
        self.selectedCategoryName = ko.observable("ALL");

        self.itemList = ko.observableArray();
        self.selectedSearchItemNo = ko.observable("");

        self.copyFromInventoryLocationID = ko.observable(sessionStorage.locationID);
        self.copyToInventoryLocationID = ko.observable(sessionStorage.locationID);
        self.allowOverwrite = ko.observable(false);
        self.IsNew = ko.observable(true);
        //self.CategoryCount = ko.observable();

        self.BaseCatInvLocationID = ko.observable(sessionStorage.locationID);
        self.BaseCatName = ko.observable("ALL");
        self.BaseCatQuantity = ko.observable(0);

        self.loadDataCategoryAndItem = function() {
            var filter = { categoryName: "ALL", itemName: "ALL" };

            DAL.fnGetCategoryList(JSON.stringify(filter), function(data) {

                self.categoryList.removeAll();

                var categoryAll = { CategoryName: 'ALL' };
                self.categoryList.push(new Category(categoryAll));

                for (var i = 0; i < data.records.length; i++) {
                    self.categoryList.push(new Category(data.records[i]));
                };

            });

//            DAL.fnGetItemList(JSON.stringify(filter), function(data) {

//                self.itemList.removeAll();

//                var itemAll = { ItemNo: 'ALL', ItemName: 'ALL' };
//                self.itemList.push(new Item(itemAll));

//                for (var i = 0; i < data.records.length; i++) {
//                    self.itemList.push(new Item(data.records[i]));
//                };

//            });

        }

        self.loadData = function() {

            var filter;

            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                filter = {
                    inventoryLocation: self.inventoryLocationFilterText(),
                    categoryName: self.selectedCategoryName(),
                    itemNo: self.selectedSearchItemNo()
                };
                self.savedFilter = filter;
            };


            DAL.fnGetItemBaseLevelList(JSON.stringify(filter), self.itemBaseLevels().length, settings.numOfRecords, self.sortKeyPOHdr().columnName, self.sortKeyPOHdr().asc, function(data) {

                for (var i = 0; i < data.records.length; i++) {
                    self.itemBaseLevels.push(new ItemBaseLevel(data.records[i]));
                };
            });
        };

        self.searchItemOptimalStock = function() {
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

            self.itemBaseLevels.removeAll();
            self.showLoadMore(false);
            self.savedFilter = null;
            self.loadData();
            //self.searchOrderNumber("");
        };

        self.openAddNewPage = function() {
            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function(item) {
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

            self.IsNew(true);

            // Initialize new PO Header
            var newItem = new ItemBaseLevel({ BaseLevelID: "" });
            self.currentItemBaseLevel(newItem);
            $("#divItemOptimalStockList").hide();
            $("#divItemBaseLevelCopy").hide();
            $("#divItemOptimalStockModify").fadeIn();

        };

        self.openUpdatePage = function(order) {
            self.IsNew(false);
            DAL.fnGetItemBaseLevel(order.BaseLevelID, function(result) {
                if (result.status == "") {
                    //alert(result.Item.InventoryLocationId);	
                    // Create a PurchaseOrderHeader object to work with.

                    var item = new ItemBaseLevel(result.Item);

                    $("#divItemOptimalStockList").hide();
                    $("#divItemOptimalStockModify").fadeIn();

                    self.currentItemBaseLevel(item);
                    self.inventoryLocation().InventoryLocationID = result.Item.InventoryLocationId;
                    //self.inventoryLocationFilterTextDetail(item.InventoryLocationName());
                    /*var selectedReason = ko.utils.arrayFirst(self.reasonList(), function (item) {
                    return item.ReasonID == orderHdr.ReasonID();
                    });
                    if (selectedReason) self.reason(selectedReason);

                    self.purchaseOrderDetails.removeAll();
                    self.loadPurchaseOrderDetails(orderHdr.PurchaseOrderHeaderRefNo);*/
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.openCopyPage = function() {
            $("#divItemOptimalStockModify").hide();
            $("#divItemOptimalStockList").hide();
            $("#divItemBaseLevelCopy").fadeIn();
            $("#divSetByCategory").hide();
            $("#divNewProducts").trigger('detach.ScrollToFixed');
            $("#selPOSCopyFrom").select2();
            $("#selPOSCopyTo").select2();
        };

        self.backToList = function() {
            $("#divItemOptimalStockModify").hide();
            $("#divItemOptimalStockList").fadeIn();
            $("#divItemBaseLevelCopy").hide();
            $("#divSetByCategory").hide();
            $("#divNewProducts").trigger('detach.ScrollToFixed');
        };

        self.openSetByCategory = function() {
            $("#divItemOptimalStockModify").hide();
            $("#divItemOptimalStockList").hide();
            $("#divSetByCategory").fadeIn();
            $("#divItemBaseLevelCopy").hide();
            $("#divNewProducts").trigger('detach.ScrollToFixed');
            $("#selOutletCategory").select2();

            self.BaseCatInvLocationID(sessionStorage.locationID);
            self.BaseCatName("ALL");
            self.BaseCatQuantity(0);
        };

        self.openSearchProductDialog = function() {
            $("#searchProductModal").modal({ backdrop: 'static' });
        };

        self.searchItem = function() {
            self.searchItemResults.removeAll();
            self.showLoadMoreItems(false);
            self.savedItemFilter = null;
            self.loadSearchItem();
            $("#txtSearchItem").val("");
        };

        self.isSelectAllFiles = ko.computed(function() {
            return ko.utils.arrayFilter(self.itemBaseLevels(), function(item) {
                return item.isSelected() == true;
            }).length == self.itemBaseLevels().length;
        });

        self.toggleSelectAllFiles = function() {
            var toggle = !self.isSelectAllFiles();
            for (var i = 0; i < self.itemBaseLevels().length; i++) {
                self.itemBaseLevels()[i].isSelected(toggle);
            }
            return true;
        };

        self.selectAllFiles = function() {
            for (var i = 0; i < self.itemBaseLevels().length; i++) {
                self.itemBaseLevels()[i].isSelected(true);
            }
            return true;
        };

        self.clearSelection = function() {
            for (var i = 0; i < self.itemBaseLevels().length; i++) {
                self.itemBaseLevels()[i].isSelected(false);
            }
            return true;
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
            //alert(filter);
            DAL.fnSearchItem(filter, true, false, true, self.searchItemResults().length, settings.numOfRecords, function(result) {
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

        self.selectItem = function(item) {
            var newItem = new ItemBaseLevel({ BaseLevelID: "", ItemNo: item.data.ItemNo, ItemName: item.data.ItemName, Uom: item.data.Uom });

            self.currentItemBaseLevel(newItem);
            /*self.currentItemBaseLevel.ItemNo(item.data.ItemNo);
            self.currentItemBaseLevel.ItemName(item.data.ItemName);*/
            $("#searchProductModal").modal("hide");
        };

        self.isItemNotFound = ko.computed(function() {
            return self.searchItemResults().length < 1;
        });

        self.isPOHdrNotFound = ko.computed(function() {
            return self.itemBaseLevels().length < 1;
        });


        self.changeInventoryLocation = function(location) {
            self.inventoryLocation(location);
            self.inventoryLocationFilterText(location.InventoryLocationName);
        };

        /* 


        self.print = function (PurchaseOrderHeaderRefNo) {
        var urlReport = settings.crReportLocation + "?r=ReturnPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&DocNo=" + PurchaseOrderHeaderRefNo;
        window.open(urlReport, "_blank", "width=700,height=500,location=0");
        //window.print();
        };

        */

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
            self.itemBaseLevels.removeAll();
            self.loadData();
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
            /*if (self.inventoryLocationFiltered().length > 0)
            $(e.target.parentNode).addClass('open');
            else
            $(e.target.parentNode).removeClass('open');*/
        };

        self.addItemBaseLevel = function() {

            var qty = parseInt(self.currentItemBaseLevel().BaseLevelQuantity()) || 0;

            if (qty == 0) {
                BootstrapDialog.alert("Please Input Quantity");
            }
            else {
                var item = {
                    BaseLevelID: self.currentItemBaseLevel().BaseLevelID,
                    ItemNo: self.currentItemBaseLevel().ItemNo(),
                    BaseLevelQuantity: self.currentItemBaseLevel().BaseLevelQuantity(),
                    InventoryLocationID: self.inventoryLocation().InventoryLocationID
                };
                //alert("test");
                DAL.fnAddItemBaseLevel(item, function(result) {
                    if (!result.error) {
                        self.backToList();
                        self.itemBaseLevels.removeAll();
                        self.loadData();
                    }
                    else {
                        BootstrapDialog.alert(result.status);
                    }
                });
            }
        };

        self.addItemBaseLevelByCategory = function() {
            var qty = parseInt(self.BaseCatQuantity()) || 0;

            if (qty == 0) {
                BootstrapDialog.alert("Please Input Quantity");
            }
            else if (self.BaseCatName() == "ALL") {
                BootstrapDialog.alert("Please Input Category");
            }
            else {
                var item = {
                    CategoryName: self.BaseCatName(),
                    BaseLevelQuantity: self.BaseCatQuantity(),
                    InventoryLocationID: self.BaseCatInvLocationID()
                };
                //alert("test");
                DAL.fnAddItemBaseLevelByCategory(item, function(result) {
                    if (result.status == '') {
                        BootstrapDialog.alert("The optimal stock have been saved successfully.");
                        self.backToList();
                        self.itemBaseLevels.removeAll();
                        self.loadData();
                    }
                    else {
                        BootstrapDialog.alert(result.status);
                    }
                });
            }
        };

        self.copyItemOptimalOutlet = function() {
            if (self.copyFromInventoryLocationID() == self.copyToInventoryLocationID()) {
                BootstrapDialog.alert("'From' and 'To' outlet cannot be the same.");
                return;
            };

            DAL.fnCopyItemOptimalStock(self.copyFromInventoryLocationID(), self.copyToInventoryLocationID(), self.allowOverwrite(), function(result) {
                if (result.status == "") {
                    BootstrapDialog.alert("The optimal stock have been copied successfully.");
                    self.copyFromInventoryLocationID(sessionStorage.InventoryLocationID);
                    self.copyToInventoryLocationID(sessionStorage.InventoryLocationID);
                    self.backToList();
                    self.searchItemOptimalStock();
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.deleteCheckedBtnState = ko.computed(function() {
            return ko.utils.arrayFilter(self.itemBaseLevels(), function(item) {
                return item.isSelected() == true;
            }).length < 1;
        });

        self.deleteSelectedItem = function() {
            BootstrapDialog.confirm("Are you sure you want to delete the selected files?", function(ok) {
                if (ok) {
                    var itemsToDelete = ko.utils.arrayFilter(self.itemBaseLevels(), function(item) {
                        return item.isSelected() == true;
                    });

                    if (itemsToDelete.length > 0) {
                        DAL.fnDeleteItemOptimalStock(itemsToDelete, function(result) {
                            if (result.status == "") {
                                BootstrapDialog.alert("The item have been deleted successfully.");
                                self.searchItemOptimalStock();
                            }
                            else {
                                BootstrapDialog.alert(result.status);
                            };
                        });
                    };
                };
            });
        };

        /*self.inventoryLocationFilterTextDetailKeyUp = function (obj, e) {
        if (self.inventoryLocationFilteredDetail().length > 0)
        $(e.target.parentNode).addClass('open');
        else
        $(e.target.parentNode).removeClass('open');

            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function (item) {
        return item.InventoryLocationName.toUpperCase() == self.inventoryLocationFilterTextDetail().toUpperCase() &&
        item.InventoryLocationName.toUpperCase() != "ALL";
        });

            if (match) self.editInventoryLocationInDetails(match);
        };*/

        self.loadDataCategoryAndItem();
        self.loadData();

    };

    $("form").submit(function(e) {
        e.preventDefault();
    });
});