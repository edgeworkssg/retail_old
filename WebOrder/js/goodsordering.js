/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />

$(function () {
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
    

    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;
    var dateDisplayFormat = settings.dateDisplayFormat;

    $("#divPurchaseOrderModify").hide();

    DAL.fnGetInventoryLocationList(function (locations) {
        DAL.fnGetSuppliersUserPortal(function (suppliers) {
            //locations.unshift({ InventoryLocationID: "0", InventoryLocationName: "ALL" });
            //suppliers.unshift({ SupplierID: null, SupplierName: "ALL" });
            DAL.fnGetSuppliersUserPortalWithoutWarehouse(function (suppliersnowh) {
                DAL.fnGetWarehouseList(function (warehouses) {
                    ko.applyBindings(new GoodsOrderingViewModel(locations, suppliers, suppliersnowh, warehouses), document.getElementById("dynamicContent"));
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
        self.InventoryLocationID = ko.observable(data.InventoryLocationID);
        self.InventoryLocationName = ko.observable(data.InventoryLocation.InventoryLocationName);
        self.POType = data.POType;
        self.RequestedBy = data.RequestedBy;

        if (data.DateNeededBy)
            self.DateNeededBy = ko.observable(new Date(parseInt(data.DateNeededBy.replace("/Date(", "").replace(")/", ""))));
        else
            self.DateNeededBy = ko.observable();

        self.SpecialValidFrom = ko.observable(new Date(parseInt(data.SpecialValidFrom.replace("/Date(", "").replace(")/", ""))))
        self.SpecialValidTo = ko.observable(new Date(parseInt(data.SpecialValidTo.replace("/Date(", "").replace(")/", ""))))
        self.WarehouseID = ko.observable(data.WarehouseID);
        self.SupplierID = ko.observable(data.SupplierID);
        self.PurchaseOrderDateFormatted = ko.computed(function () {
            return new Date(self.PurchaseOrderDate).toString(dateDisplayFormatWithTime);
        });
        
        self.DateNeededByFormatted = ko.computed(function () {
            if (self.DateNeededBy())
                return new Date(self.DateNeededBy()).toString(dateDisplayFormat);
            else
                return "";
        });

        self.SpecialValidFromFormatted = ko.computed(function () {
            return new Date(self.SpecialValidFrom()).toString(dateDisplayFormat);
        });

        self.SpecialValidToFormatted = ko.computed(function () {
            return new Date(self.SpecialValidTo()).toString(dateDisplayFormat);
        });

        self.isSpecial = ko.computed(function () {
            if (data.POType.indexOf("Special") > -1)
                return true
            else
                return false;
        });
        
        self.SalesPersonID = ko.observable("");
        self.SalesPersonName = ko.observable("");
        
        if(data.SalesPerson != null)
        {
            self.SalesPersonID = ko.observable(data.SalesPerson.UserName);
            self.SalesPersonName = ko.observable(data.SalesPerson.DisplayName);
        }

        self.DateFrom = ko.observable(data.DateFrom);
        self.DateTo = ko.observable(data.DateTo);
        
        if (data.DateFrom != null)
		{
			self.DateFrom = ko.observable(new Date(parseInt(data.DateFrom.replace("/Date(", "").replace(")/", ""))));
			self.DateTo = ko.observable(new Date(parseInt(data.DateTo.replace("/Date(", "").replace(")/", ""))));
        }
        
        self.OrderFrom = ko.computed(function () {
            if (data.WarehouseID == null || data.WarehouseID == 0)
                return "supplier"
            else
                return "warehouse";
        });
        
        self.OrderFromName = data.OrderFromName;
        self.ShipVia = data.ShipVia;
    };

    function PurchaseOrderDetail(data) {
        var self = this;

        self.PurchaseOrderHeaderRefNo = data.PurchaseOrderHeaderRefNo;
        self.PurchaseOrderDetailRefNo = data.PurchaseOrderDetailRefNo;
        self.CategoryName = data.CategoryName;
        self.ItemNo = data.ItemNo;
        self.ItemName = data.ItemName;
        self.ItemDepartmentID = data.ItemDepartmentID;
        self.Quantity = ko.observable(data.Quantity);
        self.OriginalQuantity = ko.observable(data.OriginalQuantity);
        self.QtyInStock = data.QtyInStock;
        self.QtyApproved = data.QtyApproved;
        self.RejectQty = ko.observable(data.RejectQty);
        self.OptimalStock = data.OptimalStock;
        self.UOM = data.UOM;
        self.BaseLevel = data.BaseLevel;
        self.Remark = data.Remark;
        self.Status = data.Status;
        self.SalesQty = data.SalesQty;
        self.isSelected = ko.observable(false);
        self.isVisible = ko.observable(true);
        self.Amount = ko.observable(data.Amount);
        self.GSTAmount = ko.observable(data.GSTAmount);
        self.DiscountDetail = data.DiscountDetail;
        
        self.GSTAmountString = ko.computed(function() {
            return self.GSTAmount() == null || self.GSTAmount().length === 0 ? "" : parseFloat(self.GSTAmount()).toFixed(2);
        });

        self.SalesPeriod1 = data.SalesPeriod1;
        self.SalesPeriod2 = data.SalesPeriod2;
        self.SalesPeriod3 = data.SalesPeriod3;

        self.FactoryPrice = ko.observable(data.FactoryPrice);
        self.FactoryPriceFormatted = ko.computed(function () {
            return self.FactoryPrice() == null || self.FactoryPrice().length === 0 ? "0.00" : parseFloat(self.FactoryPrice()).toFixed(2);
        });
        self.TotalCost = ko.computed(function () {
            return (self.FactoryPrice() == null || self.Quantity() == null) ? (0).toFixed(2) : parseFloat(self.FactoryPrice() * self.Quantity()).toFixed(2);
        });
    };

    function ItemToAdd(data) {
        var self = this;
        self.data = data;
    };
    
    function UserToAdd(data) {
        var self = this;
        self.data = data;
    };

    function ItemDepartment(data) {
        var self = this;
        self.ItemDepartmentID = data.ItemDepartmentID;
        self.DepartmentName = data.DepartmentName;
        self.DepartmentOrder = data.DepartmentOrder;
        self.isSelected = ko.observable(false);
    };

    // Knockout.js ViewModel
    function GoodsOrderingViewModel(locations, suppliers, suppliersnowh, warehouses) {
        var self = this;

        self.statusDropdown = ["ALL", "Pending", "Submitted", "Approved", "Received", "Rejected", "Cancelled", "Posted"];
        self.savedFilter = null;
        self.savedItemFilter = null;
        self.savedUserFilter = null;
    
        self.searchOrderNumber = ko.observable("");
        self.purchaseOrderHeaders = ko.observableArray();
        self.purchaseOrderDetails = ko.observableArray();
        self.currentPurchaseOrder = ko.observable();
        
        self.searchItemResults = ko.observableArray();
        self.selectedItemNo = ko.observable("");
        self.showLoadMoreItems = ko.observable(false);
        
        self.searchUserResults = ko.observableArray();
        self.selectedUserName = ko.observable("");
        self.showLoadMoreUsers = ko.observable(false);  
        
        self.qtyToAdd = ko.observable("");
        self.isReadOnly = ko.observable(false);
        self.isSpecial = ko.observable(false);
        self.showLoadMore = ko.observable(false);
        self.status = ko.observable("Pending");
        self.currentStatus = ko.observable("");
        self.lblSalesPersonName = ko.observable("Beauty Advisor");
        
        //self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.inventoryLocationList = ko.observableArray(locations);
        self.inventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function (item) {
            //return item.InventoryLocationID == (self.allowChangeClinic ? 0 : sessionStorage.locationID);
            return item.InventoryLocationID == sessionStorage.locationID;
        }));
        self.inventoryLocationFilterText = ko.observable(self.inventoryLocation().InventoryLocationName);
        self.inventoryLocationFilterTextDetail = ko.observable("");
        self.sortKeyPOHdr = ko.observable({ columnName: "PurchaseOrderDate", asc: false });
        self.sortKeyPODet = ko.observable({ columnName: "CategoryName", asc: true });
        self.potypeDropdown = ["Replenish", "Back Order", "Special Order"];
        self.typepo = ko.observable("Replenish");
        self.IsLockSalesPersonGR = ko.observable(false);
        
        self.dateNeededBy = ko.observable("");
        self.specialValidFrom = ko.observable("");
        self.specialValidTo = ko.observable("");

        self.startDate = ko.observable(new Date());
        self.endDate = ko.observable(new Date());
        self.saveMessage = ko.observable("");

        self.showDeptFilter = false;
        self.itemDepartments = ko.observableArray();

        self.supplierName = ko.observable("");
        if (sessionStorage.isRestrictedSupplierList.toUpperCase() == "TRUE")
            self.supplierList = ko.observableArray(ko.utils.arrayFilter(suppliersnowh, function (item) {
                return item.SupplierName != "ALL";
            })); // if isRestrictedSupplierList don't show "ALL" option
        else
            self.supplierList = ko.observableArray(suppliersnowh);

        self.supplierListSearch = ko.observableArray(suppliers);
        if (suppliers.length > 0) {
            self.supplierListSearchFilterText = ko.observable(suppliers[0].SupplierName);
            self.supplierListSearchFilter = ko.observable(suppliers[0]);
        }
        else {
            self.supplierListSearchFilterText = ko.observable("");
            self.supplierListSearchFilter = ko.observable();
        }

        self.warehouseName = ko.observable("");
        self.warehouseList = ko.observableArray(warehouses);
        
        if (sessionStorage.isSupplier == "true") {
            self.showOrderFromSupplier = ko.observable(true);
            self.showOrderFromWarehouse = ko.observable(false);
        }
        else {
            self.showOrderFromWarehouse = ko.observable(true);
            self.showOrderFromSupplier = ko.observable(false);
            DAL.fnGetSetting("GoodsOrdering_AllowOutletToOrderFromSupplier", "yes", function (res) {
                if (res == "True") {
                    self.showOrderFromSupplier = ko.observable(true);
                };
            });            
        }

        self.startDateFormatted = ko.computed(function () {
            return self.startDate().toString(dateDisplayFormat);
        });

        self.endDateFormatted = ko.computed(function () {
            return self.endDate().toString(dateDisplayFormat);
        });

        self.numOfDays = ko.observable(1); // default 1 days
        self.showSalesQty = ko.observable(false);
        self.SalesPeriod1Text = ko.observable("Sales Period 1");
        self.SalesPeriod2Text = ko.observable("Sales Period 2");
        self.SalesPeriod3Text = ko.observable("Sales Period 3");

        DAL.fnGetSetting("GoodsOrdering_RangeSalesShownGR", "no", function (range) {
            if (isNaN(range) || range <= 0)
                self.numOfDays(1);
            else
                self.numOfDays(range);

            self.UpdateSalesQtyHeaderText();

            DAL.fnGetSetting("GoodsOrdering_ShowSalesGR", "yes", function (res) {
                if (res == "True") {
                    self.showSalesQty(true);
                };
            });
        });

        self.UpdateSalesQtyHeaderText = function () {
            var range = self.numOfDays();
            var startDate = Date.today().addDays(-range);
            var endDate = startDate.clone().addDays(range).addMilliseconds(-1);
            self.SalesPeriod1Text(startDate.toString("d MMM") + " - " + endDate.toString("d MMM"));

            startDate = startDate.clone().addDays(-range);
            endDate = startDate.clone().addDays(range).addMilliseconds(-1);
            self.SalesPeriod2Text(startDate.toString("d MMM") + " - " + endDate.toString("d MMM"));

            startDate = startDate.clone().addDays(-range);
            endDate = startDate.clone().addDays(range).addMilliseconds(-1);
            self.SalesPeriod3Text(startDate.toString("d MMM") + " - " + endDate.toString("d MMM"));
        };

        self.changeSalesQtyDays = function () {
            if (isNaN(self.numOfDays()) || self.numOfDays() <= 0) self.numOfDays(1);
            if (self.showSalesQty()) {
                self.UpdateSalesQtyHeaderText();
                self.purchaseOrderDetails.removeAll();
                self.loadPurchaseOrderDetails(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo);
            }
        };

        self.changeShowSalesQty = function() {
            self.changeSalesQtyDays();
            return true;
        };

        self.disableOrderFrom = ko.computed(function () {
            return (self.isReadOnly() || (self.purchaseOrderDetails().length > 0 && self.showOrderFromSupplier()));
        });

        self.showFactoryPrice = ko.observable(false);
        DAL.fnGetSetting("GoodsOrdering_ShowFactoryPriceInGoodsOrdering", "yes", function (res) {
            if (res == "True") {
                self.showFactoryPrice(true);
            };
        });
        
        self.hideQtyInOutlet = ko.observable(false);
        DAL.fnGetSetting("GoodsOrdering_HideQtyInOutlet", "yes", function (res) {
            if (res == "True") {
                self.hideQtyInOutlet(true);
            };
        });

        self.loadData = function () {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                if (self.supplierListSearchFilter().SupplierName.toUpperCase() != self.supplierListSearchFilterText().toUpperCase()) return;
                var supplierID = self.supplierListSearchFilterText() == "ALL" || self.supplierListSearchFilterText() == "" ? "0" : self.supplierListSearchFilter().SupplierID;
                
                filter = {
                    purchaseorderheaderrefno: self.searchOrderNumber(),
                    inventorylocationid: self.inventoryLocation().InventoryLocationID,
                    status: self.status(),
                    potype: self.typepo(),
                    username: sessionStorage.username,
                    supplierid: supplierID
                };
                self.savedFilter = filter;
            };

            DAL.fnGetPurchaseOrderHeaderListWithOutletName(JSON.stringify(filter), self.purchaseOrderHeaders().length, settings.numOfRecords, self.sortKeyPOHdr().columnName, self.sortKeyPOHdr().asc, function (data) {
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

            var supplierID = null;
            var warehouseID = 0;
            var warehouseName = '';
            
            if (sessionStorage.isRestrictedSupplierList.toUpperCase() == "TRUE") {
                // if Is Restricted, set supplier to the first one in the list
                if (self.supplierList().length > 0) {
                    supplierID = self.supplierList()[0].SupplierID;
                }
                else {
                    BootstrapDialog.alert("No Supplier is assigned to this user. Please assign Supplier first.");
                    return;
                }
            }else{
                var supplier = null;
                
                if(self.warehouseList().length == 1) 
                {
                    if(self.warehouseList()[0].SupplierName == 'ALL')
                    {
                        BootstrapDialog.alert("Please create at least one Warehouse");
                        return;
                    }
                    else{
                        supplierID = self.warehouseList()[0].SupplierID;
                        warehouseID = self.warehouseList()[0].WarehouseID ? self.warehouseList()[0].WarehouseID : 0;
                        warehouseName = self.warehouseList()[0].SupplierName;
                    }
                }
            }

            // Initialize new PO Header
            var poHdr = {
                PurchaseOrderDate: new Date().toString(dateValueFormat),
                DateNeededBy: new Date().toString(dateValueFormat),
                SpecialValidFrom: new Date().toString(dateValueFormat),
                SpecialValidTo: new Date().toString(dateValueFormat),
                Status: "Pending",
                Remark: "",
                InventoryLocationID: self.inventoryLocation().InventoryLocationID,
                InventoryLocationName: self.inventoryLocation().InventoryLocationName,
                POType: self.typepo(),
                RequestedBy: sessionStorage.username,
                SalesPersonID: sessionStorage.username,
                SalesPersonName: sessionStorage.displayName,
                SupplierID: supplierID,
                WarehouseID: warehouseID
            };
            self.saveMessage("");
            DAL.fnSavePurchaseOrderHeaderCreateItems(poHdr, function (result) {
                if (result.status == "") {
                    var newPOHeader = new PurchaseOrderHeader(result.PurchaseOrderHeader);
                    self.currentPurchaseOrder(newPOHeader);
                    self.purchaseOrderHeaders.unshift(newPOHeader);
                    self.showDeptFilter = result.showDeptFilter;
                    
                    var war = ko.utils.arrayFirst(self.warehouseList(), function(item) {
                        return item.SupplierID == newPOHeader.SupplierID;
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

                    $("#divPurchaseOrderList").hide();
                    $("#divPurchaseOrderModify").fadeIn();
                    self.isReadOnly(orderHdr.Status() == "Pending" ? false : true);
                    self.currentStatus(orderHdr.Status());
                    self.saveMessage("");
                    self.currentPurchaseOrder(orderHdr);
                    self.inventoryLocationFilterTextDetail(orderHdr.InventoryLocationName());
                    self.purchaseOrderDetails.removeAll();
                    self.loadPurchaseOrderDetails(orderHdr.PurchaseOrderHeaderRefNo);
                    self.bindDatePicker();

                    var sup = ko.utils.arrayFirst(self.supplierList(), function (item) {
                        return item.SupplierID == orderHdr.SupplierID();
                    });
                    if (sup) {
                        self.supplierName(sup.SupplierName);
                    }
                    else {
                        self.supplierName("ALL");
                    };

                    var war = ko.utils.arrayFirst(self.warehouseList(), function (item) {
                        return item.SupplierID == orderHdr.SupplierID();
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
            });
        };
        
        self.fillPurchaseOrderDetails = function (result) {
            self.itemDepartments.removeAll();
            for (var i = 0; i < result.records.length; i++) {
                var poDet = {
                    PurchaseOrderHeaderRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderHeaderRefNo,
                    PurchaseOrderDetailRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderDetailRefNo,
                    CategoryName: result.records[i].PurchaseOrderDetail.Item.CategoryName,
                    ItemNo: result.records[i].PurchaseOrderDetail.ItemNo,
                    ItemName: result.records[i].PurchaseOrderDetail.Item.ItemName,
                    ItemDepartmentID: (result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment) ? result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment.ItemDepartmentID : "",
                    Quantity: result.records[i].PurchaseOrderDetail.Quantity,
                    FactoryPrice: result.records[i].PurchaseOrderDetail.FactoryPrice,
                    OriginalQuantity: (result.records[i].PurchaseOrderDetail.OriginalQuantity == null) || (result.records[i].PurchaseOrderDetail.OriginalQuantity == 0 && result.records[i].PurchaseOrderDetail.Quantity > 0) ? result.records[i].PurchaseOrderDetail.Quantity : result.records[i].PurchaseOrderDetail.OriginalQuantity ,
                    RejectQty: result.records[i].PurchaseOrderDetail.RejectQty,
                    QtyInStock: result.records[i].StockBalance,
                    QtyApproved: result.records[i].PurchaseOrderDetail.QtyApproved,
                    OptimalStock: result.records[i].OptimalStock,
                    UOM: result.records[i].PurchaseOrderDetail.Item.UOM,
                    BaseLevel: (result.records[i].PurchaseOrderDetail.Item.BaseLevel == 0) ? 1 : result.records[i].PurchaseOrderDetail.Item.BaseLevel,
                    Remark: result.records[i].PurchaseOrderDetail.Remark,
                    Status: result.records[i].PurchaseOrderDetail.Status,
                    SalesQty: result.records[i].PurchaseOrderDetail.SalesQty,
                    Amount: result.records[i].PurchaseOrderDetail.Amount,
                    GSTAmount: result.records[i].PurchaseOrderDetail.GSTAmount,
                    DiscountDetail: result.records[i].PurchaseOrderDetail.DiscountDetail,
                    SalesPeriod1: result.records[i].SalesPeriod1,
                    SalesPeriod2: result.records[i].SalesPeriod2,
                    SalesPeriod3: result.records[i].SalesPeriod3
                };
                self.purchaseOrderDetails.push(new PurchaseOrderDetail(poDet));
                
                if(result.salesPerson != null)
                {
                    self.currentPurchaseOrder().SalesPersonID(result.salesPerson.UserName);
                    self.currentPurchaseOrder().SalesPersonName(result.salesPerson.DisplayName);
                 }
                
                if (self.showDeptFilter) {
                    // Store the ItemDepartments
                    if (result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment) {
                        var itemDept = new ItemDepartment({
                            ItemDepartmentID: result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment.ItemDepartmentID,
                            DepartmentName: result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment.DepartmentName,
                            DepartmentOrder: result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment.DepartmentOrder,
                        });
                        var match = ko.utils.arrayFilter(self.itemDepartments(), function (item) {
                            return item.ItemDepartmentID == itemDept.ItemDepartmentID;
                        });
                        if (match.length == 0) {
                            self.itemDepartments.push(itemDept);
                        };
                    };
                };
            };

            self.sortKeyPODet({ columnName: "ItemNo", asc: false });
            self.sortPODet('ItemNo');

            if (self.itemDepartments().length > 0) {
                // Sort and then activate the first filter
                self.sortItemDept();
                self.itemDepartments.push(new ItemDepartment({ ItemDepartmentID: "--ALL--", DepartmentName: "ALL", DepartmentOrder: 9999 }));
                self.deptFilterClick(self.itemDepartments()[0]);
            };
            
            $('#tableApproved').freezeTable('update');
            $('#tableRejected').freezeTable('update');
        };

        self.loadPurchaseOrderDetails = function (PurchaseOrderHeaderRefNo) {
            DAL.fnGetPurchaseOrderDetailListSupplierPortal(PurchaseOrderHeaderRefNo, self.numOfDays(), self.showSalesQty(), function (result) {
                if (result.status == "") {
                    self.fillPurchaseOrderDetails(result);
                    //for (var i = 0; i < result.records.length; i++) {
                    //    var poDet = {
                    //        PurchaseOrderHeaderRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderHeaderRefNo,
                    //        PurchaseOrderDetailRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderDetailRefNo,
                    //        CategoryName: result.records[i].PurchaseOrderDetail.Item.CategoryName,
                    //        ItemNo: result.records[i].PurchaseOrderDetail.ItemNo,
                    //        ItemName: result.records[i].PurchaseOrderDetail.Item.ItemName,
                    //        ItemDepartmentID: (result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment) ? result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment.ItemDepartmentID : "",
                    //        Quantity: result.records[i].PurchaseOrderDetail.Quantity,
                    //        QtyInStock: result.records[i].StockBalance,
                    //        QtyApproved: result.records[i].PurchaseOrderDetail.QtyApproved,
                    //        OptimalStock: result.records[i].OptimalStock,
                    //        UOM: result.records[i].PurchaseOrderDetail.Item.UOM,
                    //        BaseLevel: (result.records[i].PurchaseOrderDetail.Item.BaseLevel == 0) ? 1 : result.records[i].PurchaseOrderDetail.Item.BaseLevel,
                    //        Remark: result.records[i].PurchaseOrderDetail.Remark,
                    //        Status: result.records[i].PurchaseOrderDetail.Status,
                    //        SalesQty: result.records[i].PurchaseOrderDetail.SalesQty
                    //    };
                    //    self.purchaseOrderDetails.push(new PurchaseOrderDetail(poDet));
                        
                    //    if (self.showDeptFilter) {
                    //        // Store the ItemDepartments
                    //        if (result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment) {
                    //            var itemDept = new ItemDepartment({
                    //                ItemDepartmentID: result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment.ItemDepartmentID,
                    //                DepartmentName: result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment.DepartmentName,
                    //                DepartmentOrder: result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment.DepartmentOrder,
                    //            });
                    //            var match = ko.utils.arrayFilter(self.itemDepartments(), function (item) {
                    //                return item.ItemDepartmentID == itemDept.ItemDepartmentID;
                    //            });
                    //            if (match.length == 0) {
                    //                self.itemDepartments.push(itemDept);
                    //            };
                    //        };
                    //    };
                    //};

                    //if (self.itemDepartments().length > 0) {
                    //    // Sort and then activate the first filter
                    //    self.sortItemDept();
                    //    self.itemDepartments.push(new ItemDepartment({ ItemDepartmentID: "--ALL--", DepartmentName: "ALL", DepartmentOrder: 9999 }));
                    //    self.deptFilterClick(self.itemDepartments()[0]);
                    //};

                    //self.sortKeyPODet({ columnName: "CategoryName", asc: false });
                    //self.sortPODet('CategoryName');

                    // $("#divNewProducts").trigger('detach.ScrollToFixed');
                    // $("#divNewProducts").scrollToFixed({ marginTop: 50 });
                   
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
        }

        self.SavePOHdr = function (callback) {
//			alert(ko.toJSON(self.currentPurchaseOrder));
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

                    // Have to rebind, or else the datepicker won't show up after SavePOHdr
                    self.bindDatePicker();

                    if (result.backOrderNo != "" && self.currentPurchaseOrder().POType.toUpperCase() == "BACK ORDER") {
                        BootstrapDialog.alert("There is already an existing Back Order for this outlet. Back Order " + self.currentPurchaseOrder().PurchaseOrderHeaderRefNo + " has been cancelled, and all the ordered items have been automatically added to the existing Back Order : " + result.backOrderNo);
                    }

                    if (callback) callback(true);
                }
                else {
                    BootstrapDialog.alert(result.status);
                    if (callback) callback(false);
                };
            });
        };

        self.backToList = function () {
            self.showDeptFilter = false;
            $("#divPurchaseOrderModify").hide();
            $("#divPurchaseOrderList").fadeIn();
            // $("#divNewProducts").trigger('detach.ScrollToFixed');
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

            var supplierID = self.currentPurchaseOrder().SupplierID() ? self.currentPurchaseOrder().SupplierID() : 0;
            DAL.fnSearchItemUserPortal(filter, sessionStorage.username, sessionStorage.isSupplier, sessionStorage.isRestrictedSupplierList, supplierID, self.showSalesQty(), self.numOfDays(), self.currentPurchaseOrder().InventoryLocationID(), true, false, false, self.searchItemResults().length, settings.numOfRecords, function (result) {
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
                BootstrapDialog.alert("Please enter Item Code.");
                return;
            };

            //if (self.qtyToAdd() < 1) {
            //    BootstrapDialog.alert("Quantity must be greater than zero.");
            //    return;
            //};
            if (self.qtyToAdd() && isNaN(self.qtyToAdd())) {
                BootstrapDialog.alert("Invalid Quantity.");
                return;
            };
            if (self.qtyToAdd() == "") {
                self.qtyToAdd("1");
            };

            DAL.fnGetItemNo(itemNo, function (theResult) {
                if (theResult.status == "") {
                    itemNo = theResult.ItemNo;

                    var poDet = {
                        PurchaseOrderHeaderRefNo: self.currentPurchaseOrder().PurchaseOrderHeaderRefNo,
                        PurchaseOrderDetailRefNo: "",
                        ItemNo: itemNo,
                        Quantity: self.qtyToAdd(),
                        OriginalQuantity: self.qtyToAdd(),
                        QtyInStock: 0,
                        RejectQty: 0
                    };

                    DAL.fnSavePurchaseOrderDetailXYSupplierPortal(poDet, self.numOfDays(), self.showSalesQty(), function (result) {
                        if (result.status == "") {
                            // Look for the same item in array and remove then reinsert it on top
                            var match = ko.utils.arrayFirst(self.purchaseOrderDetails(), function (item) {
                                return item.ItemNo.trim().toUpperCase() == itemNo;
                            });
                            if (match) {
                                self.purchaseOrderDetails.remove(match);
                            };

                            poDet.PurchaseOrderDetailRefNo = result.PurchaseOrderDetail.PurchaseOrderDetailRefNo;
                            poDet.Quantity = result.PurchaseOrderDetail.Quantity;
                            poDet.OriginalQuantity = result.PurchaseOrderDetail.OriginalQuantity;
                            poDet.RejectQty = result.PurchaseOrderDetail.RejectQty;
                            poDet.CategoryName = result.PurchaseOrderDetail.Item.CategoryName;
                            poDet.ItemName = result.PurchaseOrderDetail.Item.ItemName;
                            poDet.ItemDepartmentID = (result.PurchaseOrderDetail.Item.Category.ItemDepartment) ? result.PurchaseOrderDetail.Item.Category.ItemDepartment.ItemDepartmentID : "";
                            poDet.UOM = result.PurchaseOrderDetail.Item.UOM;
                            //alert(result.PurchaseOrderDetail.Item.ItemName);
                            poDet.BaseLevel = (result.PurchaseOrderDetail.Item.BaseLevel == 0) ? 1 : result.PurchaseOrderDetail.Item.BaseLevel;
                            poDet.QtyInStock = result.StockBalance;
                            poDet.OptimalStock = result.OptimalStock;
                            poDet.SalesPeriod1 = result.SalesPeriod1;
                            poDet.SalesPeriod2 = result.SalesPeriod2;
                            poDet.SalesPeriod3 = result.SalesPeriod3;
                            poDet.FactoryPrice = result.PurchaseOrderDetail.Item.FactoryPrice;
                            self.purchaseOrderDetails.unshift(new PurchaseOrderDetail(poDet));

                            self.selectedItemNo("");
                            self.qtyToAdd("");

                            self.sortKeyPODet({ columnName: "", asc: false });
                            $("#txtSelectedItemNo").focus();
                        }
                            //else if (result.status.indexOf("\"err\":\"CONFIRM_QTY\"") > -1) {
                            //    var baseLevel = JSON.parse(result.status).BaseLevel;
                            //    var roundedQty = JSON.parse(result.status).RoundedQty;
                            //    BootstrapDialog.confirm("Base level is " + baseLevel + ". Quantity will be changed to " + roundedQty, function (ok) {
                            //        if (ok) {
                            //            self.qtyToAdd(roundedQty);
                            //            self.addItemToOrder();
                            //        }
                            //        else {
                            //            return;
                            //        };
                            //    });
                            //}
                        else if (result.status.err == "CONFIRM_QTY") {
                            var baseLevel = result.status.BaseLevel;
                            var roundedQty = result.status.RoundedQty;
                            BootstrapDialog.confirm("Base level is " + baseLevel + ". Quantity will be changed to " + roundedQty, function (ok) {
                                if (ok) {
                                    self.qtyToAdd(roundedQty);
                                    self.addItemToOrder();
                                }
                                else {
                                    return;
                                };
                            });
                        }
                        else {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                }
                else {
                    BootstrapDialog.alert(theResult.status);
                };
            });
        };
        
        self.openSearchUserDialog = function () {
            $("#searchUserModal").modal({ backdrop: 'static' });
        };
        
        self.searchUser = function () {
            self.searchUserResults.removeAll();
            self.showLoadMoreUsers(false);
            self.savedUserFilter = null;
            self.loadSearchUser();
            $("#txtSearchUser").val("");
        };

        self.loadSearchUser = function () {
            var filter;
            if (self.savedItemFilter) {
                filter = self.savedUserFilter;
            }
            else {
                filter = $("#txtSearchUser").val();
                self.savedUserFilter = filter;
            };

            DAL.fnSearchUser(filter, false, self.searchUserResults().length, settings.numOfRecords, function (result) {
                if (result != null) {
                    for (var i = 0; i < result.records.length; i++) {
                        self.searchUserResults.push(new UserToAdd(result.records[i]));
                    };

                    if (result.totalRecords > self.searchUserResults().length) {
                        self.showLoadMoreUsers(true);
                    }
                    else {
                        self.showLoadMoreUsers(false);
                    };
                };
            });
        };
        
        self.DeletePersonChange = function(newValue){
           
                DAL.fnUpdateSalesPersonPurchaseOrder(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, "", function (result) {
                    if(result.status == "")
                    {
                        self.currentPurchaseOrder().SalesPersonID("");
                        self.currentPurchaseOrder().SalesPersonName("");
                    } 
                    else {
                        BootstrapDialog.alert(result.status);
                    };
                });
            
        }
        
        self.selectUser = function(user){
            
            $("#searchUserModal").modal("hide");
            
            DAL.fnUpdateSalesPersonPurchaseOrder(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, user.data.UserName, function (result) {
                if(result.status == "")
                {
                    self.currentPurchaseOrder().SalesPersonID(result.salesPerson.UserName);
                    self.currentPurchaseOrder().SalesPersonName(result.salesPerson.DisplayName);
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
                                DAL.fnClearPurchaseOrderDetailReference(deletedItem.PurchaseOrderDetailRefNo, function (res) {
                                });
                                self.purchaseOrderDetails.remove(deletedItem);

                                // $("#divNewProducts").trigger('detach.ScrollToFixed');
                                // $("#divNewProducts").scrollToFixed({ marginTop: 50 });
                                
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
                };
            });
        };

        self.cancelPurchaseOrder = function () {
            BootstrapDialog.confirm("Are you sure you want to cancel this Order?", function (ok) {
                if (ok) {
                    self.currentPurchaseOrder().Status("Cancelled");
                    self.SavePOHdr(function (success) {
                        if (success) {
                            self.isReadOnly(true);

                            DAL.fnClearPurchaseOrderReference(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, function (res) {
                            });
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
                    //        alert("Order has been cancelled.");
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
                BootstrapDialog.alert("Please insert at least 1 item to order.");
                return;
            };
            
            if (!self.showOrderFromSupplier() && !self.currentPurchaseOrder().SupplierID()) {
                BootstrapDialog.alert("Please select Order From first.");
                return;
            };
            

            BootstrapDialog.confirm("Are you sure you want to submit this Order?", function (ok) {
                if (ok) {
                    self.currentPurchaseOrder().Status("Submitted");
                    self.SavePOHdr(function (success) {
                        if (success) {
                            self.isReadOnly(true);
                            if (self.currentPurchaseOrder().POType.toUpperCase() != "BACK ORDER") { // Do Not Auto Approve Back Order
                                DAL.fnGetSetting("AutoApproveOrder", "yes", function (res) {
                                    if (res == "True") {
                                        DAL.fnPurchaseOrderApprovalAutoApprove(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, sessionStorage.username, "Y", function (result) {
                                            if (result.status == "") {
                                                if (result.backOrderNo != "") {
                                                    BootstrapDialog.alert("Order " + self.currentPurchaseOrder().PurchaseOrderHeaderRefNo + " has been approved. However some of the items are pending in the back order " + result.backOrderNo);
                                                }
                                                else {
                                                    BootstrapDialog.alert("Order " + self.currentPurchaseOrder().PurchaseOrderHeaderRefNo + " has been approved.");
                                                }
                                                //alert(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo + " is Approved");
                                                self.currentPurchaseOrder(new PurchaseOrderHeader(result.PurchaseOrderHeader));
                                                self.isReadOnly(true);
                                                // Look for the same item in array and update (replace) it with current item that we're working with
                                                var match = ko.utils.arrayFirst(self.purchaseOrderHeaders(), function (item) {
                                                    return item.PurchaseOrderHeaderRefNo == self.currentPurchaseOrder().PurchaseOrderHeaderRefNo;
                                                });
                                                self.purchaseOrderHeaders.replace(match, self.currentPurchaseOrder());
                                                // Refresh the Purchase Order Details
                                                self.purchaseOrderDetails.removeAll();
                                                self.loadPurchaseOrderDetails(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo);

                                                self.backToList();
                                            }
                                            else {
                                                self.currentPurchaseOrder().Status("Pending");
                                                self.SavePOHdr(function (success){});
                                                BootstrapDialog.alert(result.status);
                                                
                                            };
                                        });
                                    }
                                });
                            };

                            //self.backToList();
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
                //if (qty < 1) {
                //    BootstrapDialog.alert("Quantity must be greater than zero.");
                //    return;
                //};
                
                poDet.OriginalQuantity(qty + poDet.RejectQty());
                poDet.Quantity(qty);
                    
                if(poDet.SalesQty != null && poDet.SalesQty != "")
                {
                    poDet.RejectQty(poDet.SalesQty - poDet.Quantity()); 
                }
                
                DAL.fnChangePODetailQty(JSON.parse(ko.toJSON(poDet)), function (result) {
                    if (result.status != "") {
                        BootstrapDialog.alert(result.status);
                    };
                });
            };

            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Ordered Quantity:", poDet.Quantity(), function (qty) {
                    if (qty != null && !isNaN(qty)) {
                        var roundedQty = Math.ceil(qty / poDet.BaseLevel) * poDet.BaseLevel;

                        qty = parseFloat(qty);
                        //qty = parseInt(qty);
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
        
        self.changeItemQtyReject = function (poDet) {
            var updateRejectQty = function (qty) {
                //console.log(poDet.RejectQty());
                //console.log(poDet.OriginalQuantity());
                if(poDet.SalesQty != null && poDet.SalesQty != "")
                {
                    poDet.OriginalQuantity(poDet.SalesQty);
                    poDet.RejectQty(qty);
                    poDet.Quantity(poDet.OriginalQuantity() - qty); 
                }
                else
                {
                    poDet.RejectQty(qty);
                    poDet.Quantity(poDet.OriginalQuantity() - qty);
                }
                
                DAL.fnChangePODetailQty(JSON.parse(ko.toJSON(poDet)), function (result) {
                    if (result.status != "") {
                        BootstrapDialog.alert(result.status);
                    };
                });
            };

            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Reject Quantity:", poDet.RejectQty(), function (qty) {
                    if (qty != null && !isNaN(qty)) {
                        var roundedQty = Math.ceil(qty / poDet.BaseLevel) * poDet.BaseLevel;
                        qty = parseFloat(qty);
                        
                        if(qty > poDet.OriginalQuantity())
                        {
                            BootstrapDialog.alert("Reject Qty should not more than Ordered Quantity!");
                        }
                        else
                        {
                            updateRejectQty(qty);
                        }
                    };
                });
            };
        };

        self.changeInventoryLocation = function (location) {
            self.inventoryLocation(location);
            self.inventoryLocationFilterText(location.InventoryLocationName);
        };
        
        
        self.changeSupplierListSearch = function (location) {
            self.supplierListSearchFilter(location);
            self.supplierListSearchFilterText(location.SupplierName);
        };

        self.editInventoryLocationInDetails = function (location) {
            self.inventoryLocationFilterTextDetail(location.InventoryLocationName);
            self.currentPurchaseOrder().InventoryLocationID(location.InventoryLocationID);
            self.currentPurchaseOrder().InventoryLocationName(location.InventoryLocationName);
            self.SavePOHdr();
        };

        self.editSupplier = function (supplier) {
            self.supplierName(supplier.SupplierName);
            self.warehouseName("");
            self.currentPurchaseOrder().SupplierID(supplier.SupplierID);
            self.currentPurchaseOrder().WarehouseID(supplier.WarehouseID ? supplier.WarehouseID : 0);
            self.SavePOHdr();
        };

        self.editWarehouse = function (supplier) {
            self.warehouseName(supplier.SupplierName);
            self.supplierName("");
            self.currentPurchaseOrder().SupplierID(supplier.SupplierID);
            self.currentPurchaseOrder().WarehouseID(supplier.WarehouseID ? supplier.WarehouseID : 0);
            self.SavePOHdr();
        };

        self.changeRemarks = function () {
            self.SavePOHdr();
        };

        self.changeDateNeededBy = function () {
            if ($("#dateNeededBy").val())
                self.currentPurchaseOrder().DateNeededBy(new Date($("#dateNeededBy").val()));
            else
                self.currentPurchaseOrder().DateNeededBy(null);

            self.SavePOHdr();
        };

        self.changeSpecialValidFrom = function () {
            self.currentPurchaseOrder().SpecialValidFrom(new Date($("#specialValidFrom").val()));
            self.SavePOHdr();
        };

        self.changeSpecialValidTo = function () {
            self.currentPurchaseOrder().SpecialValidTo(new Date($("#specialValidTo").val()));
            self.SavePOHdr();
        };

        self.changeStartDate = function () {
            self.startDate(new Date($("#startDate").val()));
            //self.SavePOHdr();
        };

        self.changeEndDate = function () {
            self.endDate(new Date($("#endDate").val()));
            //self.SavePOHdr();
        };

        self.setFactoryPrice = function (poDet) {
            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Factory Price:", poDet.FactoryPrice(), function (price) {
                    if (price != null && !isNaN(price)) {
                        price = parseFloat(price);
                        DAL.fnChangePODetailFactoryPrice(poDet.PurchaseOrderDetailRefNo, price, function (result) {
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

        self.setTotalCost = function (poDet) {
            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Total Cost:", poDet.TotalCost(), function (totalCost) {
                    if (totalCost != null && !isNaN(totalCost)) {
                        totalCost = parseFloat(totalCost);
                        var price = totalCost / poDet.Quantity();
                        price = parseFloat(price.toFixed(4));
                        DAL.fnChangePODetailFactoryPrice(poDet.PurchaseOrderDetailRefNo, price, function (result) {
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

        self.isSelectAllPODet = ko.computed(function () {
            var countSelected = ko.utils.arrayFilter(self.purchaseOrderDetails(), function (item) {
                return item.isSelected() == true;
            }).length;
            var countVisible = ko.utils.arrayFilter(self.purchaseOrderDetails(), function (item) {
                return item.isVisible() == true;
            }).length;
            return countSelected == countVisible;
        });

        self.toggleSelectAllPODet = function () {
            var toggle = !self.isSelectAllPODet();
            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].isVisible())
                    self.purchaseOrderDetails()[i].isSelected(toggle);
            }
            return true;
        };

        self.deleteCheckedBtnState = ko.computed(function () {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function (item) {
                return item.isSelected() == true;
            }).length < 1;
        });

        self.isRecordRejected = ko.computed(function () {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function (item) {
                return item.QtyApproved < item.Quantity;
            }).length < 1;
        });

        self.isPOHdrNotFound = ko.computed(function () {
            return self.purchaseOrderHeaders().length < 1;
        });

        self.isItemNotFound = ko.computed(function () {
            return self.searchItemResults().length < 1;
        });
        
        self.isUserNotFound = ko.computed(function () {
            return self.searchUserResults().length < 1;
        });

        self.isApprovedOrRejected = ko.computed(function () {
            if (self.currentPurchaseOrder()) {
                return self.currentPurchaseOrder().Status() == 'Approved' || self.currentPurchaseOrder().Status() == 'Rejected' || self.currentPurchaseOrder().Status() == 'Posted'  || self.currentPurchaseOrder().Status() == 'Received';
            }
            else {
                return false;
            };
        });
        
        self.isLoadFromSales = ko.computed(function () {
            return ko.utils.arrayFilter(self.purchaseOrderDetails(), function (item) {
                return item.SalesQty > 0;
            }).length > 0;
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
        
        self.totalAmountApprovedOrPending = ko.computed(function() {
            var total = 0;
            
            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if(self.purchaseOrderDetails()[i].Status != "Rejected")
                {
                    if(self.purchaseOrderDetails()[i].Amount() != null && parseFloat(self.purchaseOrderDetails()[i].Amount()) > 0)
                    {
                        total += parseFloat(self.purchaseOrderDetails()[i].Amount());
                    }
                }
            }
            
            return total.toFixed(2);
        });
        
        self.totalGSTAmountApprovedOrPending = ko.computed(function() {
            var total = 0;
            
            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if(self.purchaseOrderDetails()[i].Status != "Rejected")
                {
                    if(self.purchaseOrderDetails()[i].GSTAmount() != null && parseFloat(self.purchaseOrderDetails()[i].GSTAmount()) > 0)
                    {
                        total += parseFloat(self.purchaseOrderDetails()[i].GSTAmount());
                    }
                }
            }
                        
            return total.toFixed(2);
        });
        
         self.totalAmountRejected = ko.computed(function() {
            var total = 0;
            
            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if(self.purchaseOrderDetails()[i].Status == "Rejected")
                {
                    if(self.purchaseOrderDetails()[i].Amount() != null && parseFloat(self.purchaseOrderDetails()[i].Amount()) > 0)
                    {
                        total += parseFloat(self.purchaseOrderDetails()[i].Amount());
                    }
                }
            }
            
            return total.toFixed(2);
        });
        
        self.totalGSTAmountRejected = ko.computed(function() {
            var total = 0;
            
            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if(self.purchaseOrderDetails()[i].Status == "Rejected")
                {
                    if(self.purchaseOrderDetails()[i].GSTAmount() != null && parseFloat(self.purchaseOrderDetails()[i].GSTAmount()) > 0)
                    {
                        total += parseFloat(self.purchaseOrderDetails()[i].GSTAmount());
                    }
                }
            }
                        
            return total.toFixed(2);
        });

        self.totalCostApprovedOrPending = ko.computed(function () {
            var total = 0;

            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].Status != "Rejected") {
                    if (self.purchaseOrderDetails()[i].TotalCost() != null && parseFloat(self.purchaseOrderDetails()[i].TotalCost()) > 0) {
                        total += parseFloat(self.purchaseOrderDetails()[i].TotalCost());
                    }
                }
            }

            return total.toFixed(2);
        });

        self.totalCostRejected = ko.computed(function () {
            var total = 0;

            for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                if (self.purchaseOrderDetails()[i].Status == "Rejected") {
                    if (self.purchaseOrderDetails()[i].TotalCost() != null && parseFloat(self.purchaseOrderDetails()[i].TotalCost()) > 0) {
                        total += parseFloat(self.purchaseOrderDetails()[i].TotalCost());
                    }
                }
            }

            return total.toFixed(2);
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

        self.print = function (PurchaseOrderHeaderRefNo, Status) {
            var urlReport = "";
            // if(Status == 'Received')
            //     urlReport = settings.crReportLocation + "?r=GRPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&ShowBarcode=true&DocNo=" + PurchaseOrderHeaderRefNo;
            // else
                urlReport = settings.crReportLocation + "?r=OrderPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&ShowBarcode=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
            //window.print();
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
        
               
        self.supplierListSearchFiltered = ko.computed(function () {
            if (self.supplierListSearchFilterText() && self.supplierListSearchFilterText().toUpperCase() != "ALL") {
                return ko.utils.arrayFilter(self.supplierListSearch(), function (item) {
                    return item.SupplierName.toUpperCase().indexOf(self.supplierListSearchFilterText().toUpperCase()) == 0;
                });
            }
            else {
                return self.supplierListSearch();                
            };
        });
        
        self.supplierListFiltered = ko.computed(function () {
            if (self.supplierName() && self.supplierName().toUpperCase() != "ALL") {
                return ko.utils.arrayFilter(self.supplierList(), function (item) {
                    return item.SupplierName.toUpperCase().indexOf(self.supplierName().toUpperCase()) == 0;
                });
            }
            else {
                return self.supplierList();
            };
        });

        self.warehouseListFiltered = ko.computed(function () {
            if (self.warehouseName() && self.warehouseName().toUpperCase() != "ALL") {
                return ko.utils.arrayFilter(self.warehouseList(), function (item) {
                    return item.SupplierName.toUpperCase().indexOf(self.warehouseName().toUpperCase()) == 0;
                });
            }
            else {
                return self.warehouseList();
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

        self.supplierListSearchFilterTextKeyUp = function (obj, e) {
            if (self.supplierListSearchFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');
        };
        
        self.supplierListFilterTextKeyUp = function (obj, e) {
            if (self.supplierListFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');

            var match = ko.utils.arrayFirst(self.supplierListFiltered(), function (item) {
                return item.SupplierName.toUpperCase() == self.supplierName().toUpperCase() &&
                       item.SupplierName.toUpperCase() != "ALL";
            });

            if (match) self.editSupplier(match);
        };

        self.warehouseListFilterTextKeyUp = function (obj, e) {
            if (self.warehouseListFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');

            var match = ko.utils.arrayFirst(self.warehouseListFiltered(), function (item) {
                return item.SupplierName.toUpperCase() == self.warehouseName().toUpperCase() &&
                       item.SupplierName.toUpperCase() != "ALL";
            });

            if (match) self.editSupplier(match);
        };

        self.bindDatePicker = function () {
            $("#dateNeededBy").datepicker({ autoclose: true, format: "d M yyyy", todayHighlight: true });
            $("#dateNeededBy").datepicker().on('hide', function (e) { self.changeDateNeededBy() });
            $("#dateNeededBy").datepicker('update');

            $("#specialValidFrom").datepicker({ autoclose: true, format: "d M yyyy", todayHighlight: true });
            $("#specialValidFrom").datepicker().on('hide', function (e) { self.changeSpecialValidFrom() });
            $("#specialValidFrom").datepicker('update');

            $("#specialValidTo").datepicker({ autoclose: true, format: "d M yyyy", todayHighlight: true });
            $("#specialValidTo").datepicker().on('hide', function (e) { self.changeSpecialValidTo() });
            $("#specialValidTo").datepicker('update');

            $("#startDate").datepicker({ autoclose: true, format: "d M yyyy", todayHighlight: true });
            $("#startDate").datepicker().on('hide', function (e) { self.changeStartDate() });
            $("#startDate").datepicker('update');

            $("#endDate").datepicker({ autoclose: true, format: "d M yyyy", todayHighlight: true });
            $("#endDate").datepicker().on('hide', function (e) { self.changeEndDate() });
            $("#endDate").datepicker('update');
        };

        self.showItemsFromOptimalStock = function () {
            self.purchaseOrderDetails.removeAll();
            DAL.fnLoadItemsFromOptimalStock(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, function (result) {
                if (result.status == "") {
                    self.fillPurchaseOrderDetails(result);

                    //for (var i = 0; i < result.records.length; i++) {
                    //    var poDet = {
                    //        PurchaseOrderHeaderRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderHeaderRefNo,
                    //        PurchaseOrderDetailRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderDetailRefNo,
                    //        CategoryName: result.records[i].PurchaseOrderDetail.Item.CategoryName,
                    //        ItemNo: result.records[i].PurchaseOrderDetail.ItemNo,
                    //        ItemName: result.records[i].PurchaseOrderDetail.Item.ItemName,
                    //        ItemDepartmentID: (result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment) ? result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment.ItemDepartmentID : "",
                    //        Quantity: result.records[i].PurchaseOrderDetail.Quantity,
                    //        QtyInStock: result.records[i].StockBalance,
                    //        QtyApproved: result.records[i].PurchaseOrderDetail.QtyApproved,
                    //        OptimalStock: result.records[i].OptimalStock,
                    //        UOM: result.records[i].PurchaseOrderDetail.Item.UOM,
                    //        BaseLevel: (result.records[i].PurchaseOrderDetail.Item.BaseLevel == 0) ? 1 : result.records[i].PurchaseOrderDetail.Item.BaseLevel,
                    //        Remark: result.records[i].PurchaseOrderDetail.Remark,
                    //        Status: result.records[i].PurchaseOrderDetail.Status
                    //    };
                    //    self.purchaseOrderDetails.push(new PurchaseOrderDetail(poDet));
                    //};

                    //self.sortKeyPODet({ columnName: "CategoryName", asc: false });
                    //self.sortPODet('CategoryName');

//                    $("#divNewProducts").trigger('detach.ScrollToFixed');
//                    $("#divNewProducts").scrollToFixed({ marginTop: 50 });

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

        self.showItemsFromSales = function () {
            //self.startDate(self.endDate());
            self.startDate(new Date().addDays(-5));
            $("#selectDateModal").modal({ backdrop: 'static' });
        };

        self.SubmitSearchItemsFromSales = function () {
            self.purchaseOrderDetails.removeAll();
            DAL.fnLoadItemsFromSales(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, self.startDate().toString(dateValueFormat), self.endDate().toString(dateValueFormat), function (result) {
                if (result.status == "") {
                    self.fillPurchaseOrderDetails(result);
                    self.currentPurchaseOrder().DateFrom(self.startDate().toString(dateValueFormat));
                    self.currentPurchaseOrder().DateTo(self.endDate().toString(dateValueFormat));                    

                    // $("#divNewProducts").trigger('detach.ScrollToFixed');
                    // $("#divNewProducts").scrollToFixed({ marginTop: 50 });
                    
                    $("#divBackToTop").backToTop({
                        scrollTop: $("#backToFirstProduct").position().top - 70,
                        offset: $("#backToFirstProduct").position().top - 70
                    });
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
            $("#selectDateModal").modal("hide");

        };

        self.showItemsFromInventory = function () {
            self.purchaseOrderDetails.removeAll();
            DAL.fnLoadItemsFromInventory(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, function (result) {
                if (result.status == "") {
                    self.showDeptFilter = result.showDeptFilter;
                    self.fillPurchaseOrderDetails(result);

                    //for (var i = 0; i < result.records.length; i++) {
                    //    var poDet = {
                    //        PurchaseOrderHeaderRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderHeaderRefNo,
                    //        PurchaseOrderDetailRefNo: result.records[i].PurchaseOrderDetail.PurchaseOrderDetailRefNo,
                    //        CategoryName: result.records[i].PurchaseOrderDetail.Item.CategoryName,
                    //        ItemNo: result.records[i].PurchaseOrderDetail.ItemNo,
                    //        ItemName: result.records[i].PurchaseOrderDetail.Item.ItemName,
                    //        ItemDepartmentID: (result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment) ? result.records[i].PurchaseOrderDetail.Item.Category.ItemDepartment.ItemDepartmentID : "",
                    //        Quantity: result.records[i].PurchaseOrderDetail.Quantity,
                    //        QtyInStock: result.records[i].StockBalance,
                    //        QtyApproved: result.records[i].PurchaseOrderDetail.QtyApproved,
                    //        OptimalStock: result.records[i].OptimalStock,
                    //        UOM: result.records[i].PurchaseOrderDetail.Item.UOM,
                    //        BaseLevel: (result.records[i].PurchaseOrderDetail.Item.BaseLevel == 0) ? 1 : result.records[i].PurchaseOrderDetail.Item.BaseLevel,
                    //        Remark: result.records[i].PurchaseOrderDetail.Remark,
                    //        Status: result.records[i].PurchaseOrderDetail.Status
                    //    };
                    //    self.purchaseOrderDetails.push(new PurchaseOrderDetail(poDet));
                    //};

                    //self.sortKeyPODet({ columnName: "CategoryName", asc: false });
                    //self.sortPODet('CategoryName');

                    // $("#divNewProducts").trigger('detach.ScrollToFixed');
                    // $("#divNewProducts").scrollToFixed({ marginTop: 50 });
                    
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

        self.showItemsFromOptimalStockBtnState = ko.computed(function () {
            return false;
        });

        self.showItemsFromSalesBtnState = ko.computed(function () {
            return false;
        });

        self.showItemsFromInventoryBtnState = ko.computed(function () {
            return false;
        });

        self.deptFilterClick = function (obj, e) {
            for (var i = 0; i < self.itemDepartments().length; i++) {
                if (self.itemDepartments()[i] == obj) {
                    self.itemDepartments()[i].isSelected(true);

                    // Set visibility of Purchase Order Detail
                    for (var j = 0; j < self.purchaseOrderDetails().length; j++) {
                        if (self.purchaseOrderDetails()[j].ItemDepartmentID == obj.ItemDepartmentID || obj.ItemDepartmentID == "--ALL--") {
                            self.purchaseOrderDetails()[j].isVisible(true);
                        }
                        else {
                            self.purchaseOrderDetails()[j].isVisible(false);
                        };
                    };
                }
                else {
                    self.itemDepartments()[i].isSelected(false);
                };
            };
        };

        self.sortItemDept = function () {
            self.itemDepartments.sort(function (a, b) {
                var valueA = ko.utils.unwrapObservable(a["DepartmentOrder"]);
                var valueB = ko.utils.unwrapObservable(b["DepartmentOrder"]);
                return valueA == valueB ? 0 : (valueA < valueB ? -1 : 1);
            });
        };
        
        self.GetPurchaseHeaderSetting = function() {
            DAL.fnGetPurchaseHeaderSetting(function(result) {
                if (result.status == "") {
                    self.lblSalesPersonName(result.TextBeautyAdvisors);
                    
                    if(result.IsLockSalesPersonGR.toUpperCase() == "TRUE" || result.IsLockSalesPersonGR.toUpperCase() == "YES")
                        self.IsLockSalesPersonGR(true);
                    else
                        self.IsLockSalesPersonGR(false);
                }else{
                    BootstrapDialog.alert(result.status)
                }
                
            });
        }
        
        self.IsLockSalesPerson = ko.computed(function () {
            return self.IsLockSalesPersonGR() || self.isReadOnly();
        });

        self.GetPurchaseHeaderSetting();
        
        self.loadData();

    };

    $("form").submit(function (e) {
        e.preventDefault();
    });
});

