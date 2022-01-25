/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />
/// <reference path="bootstrap-toggle.min.js" />

$(function () {
    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;
    var dateDisplayFormat = settings.dateDisplayFormat;
    
    $('#tableDet').freezeTable({
        'freezeColumn': false,
        'freezeHead': true,
        'namespace': 'tableDet'
    });

    $('#tableSearch').freezeTable({
        'freezeColumn': false,
        'freezeHead': true,
        'namespace': 'tableSearch'
    });


    $("#divStockTransferModify").hide();

    $(window).scroll(function(event){
          //Sets the current scroll position
          var st = $(this).scrollTop();
          var position = $("#divItemDeptFilter").position().top;
          var markbottom = $("#tableDetail").position().top + $('#tableDetail').outerHeight(true);

          //Determines up-or-down scrolling
          $("#divBottomButton").removeClass("fixedBottom");

          if (st > position){
            $("#divBottomButton").addClass("fixedBottom");
          } 

          if($(window).scrollTop() + $(window).height() == $(document).height()){
            $("#divBottomButton").removeClass("fixedBottom");
          }
          //Updates scroll position
    });

    //function resize thead when position fixed
    function ResizeThead(){
         $('#theadStockTransfer>th').each(function() {
            //debugger;
            var rc = $(this).attr('class');
    
            // remove any additional classes (eg table sort will add header class)
            var n = rc.split(' ');
            
            // get the width for the data
            var datawidth = $(this).width();
            // force header and data to stick to this width
		    $(this).width(datawidth);
            $('#tbodyStockTransfer>.'+n).each(function() {
                $(this).width(datawidth);
            });
         });
    };

    DAL.fnGetInventoryLocationList(function (locations) {
        //locations.unshift({ InventoryLocationID: "0", InventoryLocationName: "ALL" });
        DAL.fnGetWarehouseList(function(warehouses) {
            ko.applyBindings(new StockTransferViewModel(locations, warehouses), document.getElementById("dynamicContent"));
        });
    });

    function StockTransferHdr(data) {
        var self = this;

        self.StockTransferHdrRefNo = data.StockTransferHdrRefNo;
        self.TransferDate = ko.observable(new Date(parseInt(data.TransferDate.replace("/Date(", "").replace(")/", ""))));
        self.RequiredDate = ko.observable(new Date(parseInt(data.RequiredDate.replace("/Date(", "").replace(")/", ""))));        
        self.TransferFromLocationID = ko.observable(data.TransferFromLocationID);
        self.TransferFromLocation = ko.observable(data.TransferFromLocation);
        self.TransferToLocationID = ko.observable(data.TransferToLocationID);
        self.TransferToLocation = ko.observable(data.TransferToLocation);        
        self.RequestBy = ko.observable(data.RequestBy);
        self.Status = ko.observable(data.Status);
        self.Remark = ko.observable(data.Remark);                
        
        self.TransferDateFormatted = ko.computed(function () {
            return new Date(self.TransferDate()).toString(dateDisplayFormat);
        });        
        
        self.RequiredDateFormatted = ko.computed(function () {
            return new Date(self.RequiredDate()).toString(dateDisplayFormat);
        });
        
        self.WarehouseID = ko.observable(data.WarehouseID);
        self.SupplierID = ko.observable(data.SupplierID);

        self.PriceLevel = ko.observable(data.PriceLevel);

        self.ReturnToWarehouseID = ko.observable(data.ReturnToWarehouseID);
        self.ReturnToSupplierID = ko.observable(data.ReturnToSupplierID);
        self.CreditInvoiceNo = ko.observable(data.CreditInvoiceNo);
        self.InvoiceNo = ko.observable(data.InvoiceNo);
        self.AutoStockIn = ko.observable(data.AutoStockIn);
        
        self.radioSelectedOptionValue = ko.observable("No");
        if(self.AutoStockIn() == true)
            self.radioSelectedOptionValue("Yes");
            
        self.Yes = ko.computed(
            {
                read: function() {
                    return self.radioSelectedOptionValue() == "Yes";
                },
                write: function(value){
                    if (value)                    
                        self.radioSelectedOptionValue("Yes");
                }
            }
        ,self);
        self.No = ko.computed(
            {
                read: function() {
                    self.radioSelectedOptionValue() == "No";
                },
                write: function(value){
                    if (value)
                        self.radioSelectedOptionValue("No");                        
                }
            }
        ,self); 
    };

    function StockTransferDet(data) {
        var self = this;

        self.StockTransferHdrRefNo = data.StockTransferHdrRefNo;
        self.StockTransferDetRefNo = data.StockTransferDetRefNo;
        self.CategoryName = data.CategoryName;
        self.ItemNo = data.ItemNo;
        self.ItemName = data.ItemName;
        self.UOM = data.UOM;        
        self.ItemDepartmentID = data.ItemDepartmentID;
        self.DepartmentName = data.DepartmentName;        
        self.Quantity = ko.observable(data.Quantity);
        self.FullFilledQuantity = ko.observable(data.FullFilledQuantity);
        self.Status = ko.observable(data.Status);
        self.Remark = ko.observable(data.Remark);
                  
        self.isSelected = ko.observable(false);
        self.isVisible = ko.observable(true);

        self.FactoryPrice = ko.observable(data.FactoryPrice);
        self.FactoryPriceFormatted = ko.computed(function () {
            return self.FactoryPrice() == null || self.FactoryPrice().length === 0 ? "0.00" : parseFloat(self.FactoryPrice()).toFixed(2);
        });
        self.TotalCost = ko.computed(function () {
            return (self.FactoryPrice() == null || self.Quantity() == null) ? (0).toFixed(2) : parseFloat(self.FactoryPrice() * self.Quantity()).toFixed(2);
        });

        self.P1Price = data.P1Price;
        self.P2Price = data.P2Price;
        self.P3Price = data.P3Price;
        self.P4Price = data.P4Price;
        self.P5Price = data.P5Price;
    };

    function ItemToAdd(data) {
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
    function StockTransferViewModel(locations, warehouses) {
        var self = this;

        self.statusDropdown = ["ALL", "Submitted", "Approved", "Received", "Rejected"];
        self.savedFilter = null;
        self.savedItemFilter = null;
        //self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.searchOrderNumber = ko.observable("");
        self.stockTransferHdrs = ko.observableArray();
        self.stockTransferDets = ko.observableArray();
        self.currentStockTransfer = ko.observable();
        self.searchItemResults = ko.observableArray();
        self.selectedItemNo = ko.observable("");
        self.qtyToAdd = ko.observable("");
        self.isReadOnly = ko.observable(false);
        self.isSpecial = ko.observable(false);
        self.showLoadMore = ko.observable(false);
        self.isPreview = ko.observable(false);		
        self.status = ko.observable("Submitted");
        self.showLoadMoreItems = ko.observable(false);
        self.inventoryLocationList = ko.observableArray(locations); 
        self.inventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function (item) {
            //return item.InventoryLocationID == (self.allowChangeClinic ? 0 : sessionStorage.locationID);
            return item.InventoryLocationID == sessionStorage.locationID;
        }));
        self.inventoryLocationTo = ko.observable(ko.utils.arrayFirst(locations, function (item) {
            return item.InventoryLocationID == 0;
        }));        
        
        self.inventoryLocationFilterText = ko.observable(self.inventoryLocation().InventoryLocationName);
        self.inventoryLocationFilterTextDetail = ko.observable("");
        self.inventoryLocationFilterTextDetailTo = ko.observable("");        
        
        self.sortKeyHdr = ko.observable({ columnName: "StockTransferHdrRefNo", asc: false });
        self.sortKeyDet = ko.observable({ columnName: "CategoryName", asc: false });
        
		self.SelectedStockTransferHdrRefNo = ko.observable("");
        self.RequiredDate = ko.observable("");

        self.showDeptFilter = true;
        self.itemDepartments = ko.observableArray();

        self.showPriceLevel = ko.observable(false);
        self.priceLevel = ko.observable();
        self.selectedPriceLevel = ko.observable("");
        DAL.fnGetSetting("GoodsOrdering_ShowPriceLevelForWebOrder", "yes", function (res) {
            if (res == "True") {
                DAL.fnGetPriceLevel(function (resLevel) {
                    if (resLevel.status == "" && resLevel.records.length > 0) {
                        self.priceLevel(resLevel.records);
                        self.showPriceLevel(true);
                    }
                });
            };
        });
        
        self.stockTransferGoThroughWarehouse = ko.observable(false);
         DAL.fnGetSetting("StockTransferWillGoThroughWarehouse", "yes", function (res) {
            if (res == "True") {
                self.stockTransferGoThroughWarehouse(true);
            };
        });

        self.warehouseName = ko.observable("");
        self.warehouseList = ko.observableArray(warehouses);
        
        self.warehouseListFilterTextKeyUp = function(obj, e) {
            if (self.warehouseListFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');

            var match = ko.utils.arrayFirst(self.warehouseListFiltered(), function(item) {
                return item.SupplierName.toUpperCase() == self.warehouseName().toUpperCase() &&
                       item.SupplierName.toUpperCase() != "ALL";
            });

            if (match) self.editSupplier(match);
        };


        self.warehouseListFiltered = ko.computed(function() {
            if (self.warehouseName() && self.warehouseName().toUpperCase() != "ALL") {
                return ko.utils.arrayFilter(self.warehouseList(), function(item) {
                    return item.SupplierName.toUpperCase().indexOf(self.warehouseName().toUpperCase()) == 0;
                });
            }
            else {
                return self.warehouseList();
            };
        });

        self.editWarehouse = function(supplier) {
            self.warehouseName(supplier.SupplierName);
            self.currentStockTransfer().ReturnToSupplierID(supplier.SupplierID);
            self.currentStockTransfer().ReturnToWarehouseID(supplier.WarehouseID ? supplier.WarehouseID : 0);
            self.SaveStockTransfer();
        };
        
        self.showFactoryPrice = ko.observable(false);
        DAL.fnGetSetting("GoodsOrdering_ShowFactoryPriceInTransferApproval", "yes", function (res) {
            if (res == "True") {
                self.showFactoryPrice(true);
            };
        });

        self.loadData = function () {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                filter = {
                    StockTransferHdrRefNo: self.searchOrderNumber(),
                    FromInventoryLocationID: self.inventoryLocation().InventoryLocationID,
                    ToInventoryLocationID: 0,
                    Status: self.status()
                };
                self.savedFilter = filter;
            };

            DAL.fnFetchStockTransferList(JSON.stringify(filter), settings.numOfRecords, self.stockTransferHdrs().length, self.sortKeyHdr().columnName, self.sortKeyHdr().asc, function (data) {
                for (var i = 0; i < data.records.length; i++) {
                    self.stockTransferHdrs.push(new StockTransferHdr(data.records[i]));
                };

                if (data.totalRecords > self.stockTransferHdrs().length) {
                    self.showLoadMore(true);
                }
                else {
                    self.showLoadMore(false);
                };
                
                $('#tableSearch').freezeTable('update');
            });
        };

        self.searchStockTransfer = function () {
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

            self.stockTransferHdrs.removeAll();
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

            // Initialize new StockTake Header
            var hdr = {
                StockTransferHdrRefNo: "",
                TransferDate: new Date().toString(dateValueFormat), 
                RequiredDate: new Date().toString(dateValueFormat),
                TransferFromLocationID: self.inventoryLocation().InventoryLocationID,   
                TransferToLocationID: 0,             
                RequestBy: sessionStorage.username,                
                Status: "Pending",
                Remark: "",
            };
            DAL.fnSaveStockTransferHdr(JSON.stringify(hdr), function (result) {
                if (result.status == "") {
                    var transferHdr = new StockTransferHdr(result.data);
                    self.currentStockTransfer(transferHdr);
                    self.stockTransferHdrs.unshift(transferHdr);
                    self.showDeptFilter = true;
					self.isPreview(false);
                    self.openUpdatePage(transferHdr);
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };
        
        self.IsCanPrint = ko.observable(false);
        
        self.printInvoice = function(PurchaseOrderHeaderRefNo, Status) {
            var urlReport = "";
            urlReport = settings.crReportLocation + "?r=TransferOrderPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&ShowBarcode=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
        };

        self.printCreditNote = function(PurchaseOrderHeaderRefNo, Status) {
            var urlReport = "";
            urlReport = settings.crReportLocation + "?r=TransferReturnPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&ShowBarcode=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
        };

        self.printInvoiceFromDetails = function() {
            self.printInvoice(self.currentStockTransfer().StockTransferHdrRefNo, self.currentStockTransfer().Status());
        };

        self.printCreditNoteFromDetails = function() {
            self.printCreditNote(self.currentStockTransfer().StockTransferHdrRefNo, self.currentStockTransfer().Status());
        };
        
        self.openUpdatePage = function (hdr) {
            DAL.fnFetchStockTransferHdr(hdr.StockTransferHdrRefNo, function (result) {
                if (result.status == "") {
                    // Create a PurchaseOrderHeader object to work with.
                    var transferHdr = new StockTransferHdr(result.hdrData);

                    $("#divStockTransferList").hide();
                    $("#divStockTransferModify").fadeIn();
                    self.isReadOnly(transferHdr.Status() == "Submitted" ? false : true);
                    self.IsCanPrint(self.stockTransferGoThroughWarehouse() && (transferHdr.Status() == 'Approved' || transferHdr.Status() == 'Received' || transferHdr.Status() == 'Rejected' || transferHdr.Status() == 'Posted') ? true : false);
                                      
                    self.currentStockTransfer(transferHdr);                                                          
                    self.inventoryLocationFilterTextDetail(transferHdr.TransferFromLocation());
                    self.inventoryLocationFilterTextDetailTo(transferHdr.TransferToLocation());                    
                   
                    self.stockTransferDets.removeAll();
                    self.loadStockTransferDetails(transferHdr.StockTransferHdrRefNo);

                    if (self.showPriceLevel() && self.priceLevel().length > 0) {
                        var level = ko.utils.arrayFirst(self.priceLevel(), function (item) {
                            return item.Key == transferHdr.PriceLevel();
                        });
                        if (level)
                            self.selectedPriceLevel(level.Value);
                        else
                            self.selectedPriceLevel("");
                    }
                    
                    var war = ko.utils.arrayFirst(self.warehouseList(), function(item) {
                        return item.SupplierID == transferHdr.ReturnToSupplierID();
                    });
                    if (war) {
                        self.warehouseName(war.SupplierName);                        
                    }
                    else {
                        self.warehouseName("");
                    };
                    
                    self.bindDatePicker();
					self.isPreview(false);
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.loadStockTransferDetails = function (StockTransferHdrRefNo) {
            DAL.fnFetchStockTransferDet(StockTransferHdrRefNo, function (result) {
                if (result.status == "") {
					self.showDeptFilter = true;
                    self.fillStockTransferDetails(result);
                    $("#divItemDeptFilter").trigger('detach.ScrollToFixed');
                    $("#divItemDeptFilter").scrollToFixed({ marginTop: 50 });
					$("#theadStockTransfer").trigger('detach.ScrollToFixed');
                    $("#theadStockTransfer").scrollToFixed({ marginTop:  $("#divItemDeptFilter").height() + 50 });
                    ResizeThead();
                   
                   $('#tableDet').freezeTable('update');
                   
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

        self.fillStockTransferDetails = function (result) {
            self.itemDepartments.removeAll();
            for (var i = 0; i < result.records.length; i++) {
                self.stockTransferDets.push(new StockTransferDet(result.records[i]));
				
                if (self.showDeptFilter) {
                    // Store the ItemDepartments
                    if (result.records[i].ItemDepartmentID!='') {
                        var itemDept = new ItemDepartment({
                            ItemDepartmentID: result.records[i].ItemDepartmentID,
                            DepartmentName: result.records[i].DepartmentName,
                            DepartmentOrder: result.records[i].DepartmentOrder,
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
            if (self.itemDepartments().length > 0) {
                // Sort and then activate the first filter
                self.sortItemDept();
                self.itemDepartments.unshift(new ItemDepartment({ ItemDepartmentID: "--ALL--", DepartmentName: "ALL", DepartmentOrder: 9999 }));
                self.deptFilterClick(self.itemDepartments()[0]);
            };
        };

        self.SaveStockTransfer = function (callback) {
            DAL.fnSaveStockTransferHdr(JSON.stringify(JSON.parse(ko.toJSON(self.currentStockTransfer))), function (result) {
                if (result.status == "") {
                    self.currentStockTransfer(new StockTransferHdr(result.data));

                    // Look for the same item in array and update (replace) it with current item that we're working with
                    var match = ko.utils.arrayFirst(self.stockTransferHdrs(), function (item) {
                        return item.StockTransferHdrRefNo == self.currentStockTransfer().StockTransferHdrRefNo;
                    });
                    self.stockTransferHdrs.replace(match, self.currentStockTransfer());

                    self.stockTransferHdrs.sort(function (a, b) {
                        var sortDirection = (self.sortKeyHdr().asc) ? 1 : -1;
                        var valueA = ko.utils.unwrapObservable(a[self.sortKeyHdr().columnName]);
                        var valueB = ko.utils.unwrapObservable(b[self.sortKeyHdr().columnName]);
                        return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
                    });

                    // Have to rebind, or else the datepicker won't show up after SavePOHdr
                    self.bindDatePicker();

                    if (callback) callback(true);
                }
                else {
                    BootstrapDialog.alert(result.status);
                    if (callback) callback(false);
                };
            });
        };

        self.backToList = function () {
            self.showDeptFilter = true;
            $("#divStockTransferModify").hide();
            $("#divStockTransferList").fadeIn();
            $("#divItemDeptFilter").trigger('detach.ScrollToFixed');
            
            self.searchStockTransfer();
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

            DAL.fnSearchItem(filter, true, false, false, self.searchItemResults().length, settings.numOfRecords, function (result) {
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

        self.addItemAllItems = function () {

            DAL.fnAddAllTransferItems(self.currentStockTransfer().StockTransferHdrRefNo, function (result) {
                if (result.status == "") {
                    self.stockTransferDets.removeAll();
                    self.loadStockTransferDetails(self.currentStockTransfer().StockTransferHdrRefNo);
                    self.selectedItemNo("");
                    self.qtyToAdd("");

                    self.sortKeyDet({ columnName: "CategoryName", asc: false });
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };
		
        self.addItemToTransfer = function () {
            var itemNo = self.selectedItemNo().trim().toUpperCase();
            self.selectedItemNo(itemNo);

            if (!itemNo) {
                BootstrapDialog.alert("Please enter Item Code.");
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

            DAL.fnAddStockTransferItem(self.currentStockTransfer().StockTransferHdrRefNo, itemNo, self.qtyToAdd(), function (result) {
                if (result.status == "") {
                    var match = ko.utils.arrayFirst(self.stockTransferDets(), function (item) {
                        return item.ItemNo.trim().toUpperCase() == itemNo;
                    });
                    
                    if (match) {
                        self.stockTransferDets.remove(match);
					}
                    self.stockTransferDets.unshift(new StockTransferDet(result.transferDet));
                    self.selectedItemNo("");
                    self.qtyToAdd("");

                    self.sortKeyDet({ columnName: "CategoryName", asc: false });

                    $("#divItemDeptFilter").trigger('detach.ScrollToFixed');
                    $("#divItemDeptFilter").scrollToFixed({ marginTop: 50 });
                    $("#theadStockTransfer").trigger('detach.ScrollToFixed');
                    $("#theadStockTransfer").scrollToFixed({ marginTop: $("#divItemDeptFilter").height() + 50 });
                    ResizeThead();
                    
                    $('#tableDet').freezeTable('update');
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.deleteSelectedTransferDet = function () {
            BootstrapDialog.confirm("Are you sure you want to delete the selected items?", function (ok) {
                if (ok) {
                    var itemsToDelete = ko.utils.arrayFilter(self.stockTransferDets(), function (item) {
                        return item.isSelected() == true;
                    });
                    var listToDelete = new Array();
                    for (var i = 0; i < itemsToDelete.length; i++) {
                        listToDelete.push(itemsToDelete[i].StockTransferDetRefNo);
                    };
					DAL.fnDeleteStockTransferItem(JSON.stringify(listToDelete), function (result) {
						if (result.status == "") {
                            
                            for (var i = 0; i < itemsToDelete.length; i++) {
                                self.stockTransferDets.remove(itemsToDelete[i]);
                            };                            
                            
                            $('#tableDet').freezeTable('update');
                            
							$("#divItemDeptFilter").trigger('detach.ScrollToFixed');
							$("#divItemDeptFilter").scrollToFixed({ marginTop: 50 });
							$("#theadStockTransfer").trigger('detach.ScrollToFixed');
							$("#theadStockTransfer").scrollToFixed({ marginTop:  $("#divItemDeptFilter").height() + 50 });
							ResizeThead();

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
            });
        };

        self.cancelStockTransfer = function () {
            BootstrapDialog.confirm("Are you sure you want to cancel this Stock Transfer?", function (ok) {
                if (ok) {
                    self.currentStockTransfer().Status("Cancelled");
                    self.SaveStockTransfer(function (success) {
                        if (success) {
                            self.isReadOnly(true);
                            self.backToList();
                        }
                        else {
                            self.currentStockTransfer().Status("Pending");
                        };
                    });
                };
            });
        };

        self.submitPurchaseOrder = function () {
            if (self.stockTransferDets().length < 1) {
                BootstrapDialog.alert("Please insert at least 1 item to order.");
                return;
            };
            
            if(self.inventoryLocationFilterTextDetailTo() == ""){
                BootstrapDialog.alert("Please select transfer to outlet");
                return;                
            }
            
            if(self.inventoryLocationFilterTextDetailTo() == self.inventoryLocationFilterTextDetail()){
                BootstrapDialog.alert("Please select different transfer to outlet");
                return;                
            }

            if (self.showPriceLevel() && !self.currentStockTransfer().PriceLevel()) {
                BootstrapDialog.alert("Please select Price Level first.");
                return;
            }
            
            if (self.stockTransferGoThroughWarehouse() && !self.currentStockTransfer().ReturnToSupplierID()) {
                BootstrapDialog.alert("Please select Return To first.");
                return;
            }
            
            self.currentStockTransfer().AutoStockIn(self.currentStockTransfer().radioSelectedOptionValue() == "Yes");
           
            BootstrapDialog.confirm("Are you sure you want to submit this Transfer Approval?", function (ok) {
                if (ok) {
                    DAL.fnStockTransferApproval(ko.toJSON(self.stockTransferDets()), self.currentStockTransfer().StockTransferHdrRefNo, sessionStorage.username, self.currentStockTransfer().PriceLevel(),self.currentStockTransfer().AutoStockIn(), function (result) {
                        if (result.status == "") {
                            self.currentStockTransfer(new StockTransferHdr(result.StockTransferHdr));
                            self.isReadOnly(true);
                            // Look for the same item in array and update (replace) it with current item that we're working with
                            var match = ko.utils.arrayFirst(self.stockTransferHdrs(), function (item) {
                                return item.StockTransferHdrRefNo == self.currentStockTransfer().StockTransferHdrRefNo;
                            });
                            self.stockTransferHdrs.replace(match, self.currentStockTransfer());
                            // Refresh the Stock Transfer Details
                            self.stockTransferDets.removeAll();
                            self.loadStockTransferDetails(self.currentStockTransfer().StockTransferHdrRefNo);

                            self.stockTransferHdrs.sort(function (a, b) {
                                var sortDirection = (self.sortKeyHdr().asc) ? 1 : -1;
                                var valueA = ko.utils.unwrapObservable(a[self.sortKeyHdr().columnName]);
                                var valueB = ko.utils.unwrapObservable(b[self.sortKeyHdr().columnName]);
                                return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
                            });

                            self.backToList();
                        }
                        else {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                };
            });
        };
        
        self.returnToPending = function () {
            BootstrapDialog.confirm("Are you sure you want to revert this Stock Transfer?", function (ok) {
                if (ok) {
                    DAL.fnRevertStockTransferHdr(self.currentStockTransfer().StockTransferHdrRefNo, function (result) {
                        if (result.status == "") {
                            BootstrapDialog.alert("Status reverted");                            
                            self.backToList();                           
                        }
                        else {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                };
            });
        };
        
        self.currentStatus = ko.computed(function () {
            return self.currentStockTransfer() == null ? "" : self.currentStockTransfer().Status();
        });

        self.changeCreditInvoiceNo = function (o) {
            DAL.fnChangeCreditInvoiceNoST(o.StockTransferHdrRefNo, o.CreditInvoiceNo(), function (result) {
                if (result.status != "") {
                    BootstrapDialog.alert(result.status);
                };
            });
        };
                
        self.changeInvoiceNo = function (o) {
            DAL.fnChangeInvoiceNoST(o.StockTransferHdrRefNo, o.InvoiceNo(), function (result) {
                if (result.status != "") {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

		self.numericKeypad = function(key) {
			var startPos = document.getElementById("txtUpdateQty").selectionStart;
			var endPos = document.getElementById("txtUpdateQty").selectionEnd;			
			//console.log(startPos+" - "+endPos);
			
			var oldVal = $("#txtUpdateQty").val();
			if(key == "DEL"){
				if(startPos!=0 && startPos == endPos)
					startPos -= 1;			
				if(oldVal.length > 1) {
					oldVal = oldVal.slice(0,startPos)+""+oldVal.slice(endPos,(oldVal.length));
				}
				else {
					oldVal = "";
				}
			}
			else if(key == "."){
				if(oldVal.length == 0){
					oldVal = "0.";
					startPos += 1;					
				}
				else if(oldVal.indexOf(".")<0){
					oldVal = oldVal.slice(0,startPos)+""+key+""+oldVal.slice(endPos,(oldVal.length));
				}
				startPos += 1;
			}
			else if(key == "0"){
				oldVal = oldVal.slice(0,startPos)+""+key+""+oldVal.slice(endPos,(oldVal.length));
				startPos += 1;
			}
			else {
				oldVal = oldVal.slice(0,startPos)+""+key+""+oldVal.slice(endPos,(oldVal.length));
				startPos += 1;
            }
			$("#txtUpdateQty").val(oldVal);	
			//$("#txtUpdateQty").focus();	
			//document.getElementById("txtUpdateQty").selectionStart = startPos;
		    //document.getElementById("txtUpdateQty").selectionEnd = startPos;
			document.getElementById("txtUpdateQty").focus();
			document.getElementById("txtUpdateQty").setSelectionRange(oldVal.length, oldVal.length);
		};		
		
        self.changeItemQty = function (poDet) {
			if(self.isReadOnly())
				return;
			self.SelectedStockTransferHdrRefNo(poDet.StockTransferDetRefNo);
			$("#txtUpdateQty").val(poDet.Quantity());		
			$("#lblItemNameQty").html(poDet.ItemName + " ("+poDet.UOM+")");
			$("#updateQtyModal").modal({ backdrop: 'static' });
			//$("#updateQtyModal").on("shown.bs.modal", function () {
			//	$("#txtUpdateQty").select();				
			//	$("#txtUpdateQty").focus();				
			//});
			$("#txtUpdateQty").select();
			$("#txtUpdateQty").focus();
			//$("#txtUpdateQty").numeric();			
        };
		
		self.nextQty = function() {
			var qty = $("#txtUpdateQty").val();
			if($.isNumeric(qty)) {
				//self.ScrollPos($(document).scrollTop());
				var nextIndex = -1;
				var currIndex = -1;			
				for(var i = 0; i<self.stockTransferDets().length; i++){
					if(self.stockTransferDets()[i].StockTransferDetRefNo == self.SelectedStockTransferHdrRefNo()){
						currIndex = i;
						break;		
					};
				};
				
				if(currIndex!=-1){
					self.stockTransferDets()[currIndex].Quantity(qty);	
					DAL.fnChangeStockTransferQty(self.stockTransferDets()[currIndex].StockTransferDetRefNo, qty, function (result) {
						if (result.status != "") {
							bootstrapdialog.alert(result.status);
						}
						else {
							for(var k=(currIndex+1); k<self.stockTransferDets().length && nextIndex==-1; k++) {
								if(self.stockTransferDets()[k].isVisible()){
									nextIndex = k;
								};							
							};
							if(nextIndex != -1)
								self.changeItemQty(self.stockTransferDets()[nextIndex]);									
						};
					});
				};		
			};	
			return;
		};			
		
		self.saveQty = function() {
			var qty = $("#txtUpdateQty").val();
			if($.isNumeric(qty)) {
				//self.ScrollPos($(document).scrollTop());			
				for(var i = 0; i<self.stockTransferDets().length; i++){
					if(self.stockTransferDets()[i].StockTransferDetRefNo == self.SelectedStockTransferHdrRefNo()){
					    self.stockTransferDets()[i].Quantity(qty);
					    //if (qty > 0)
					    //    self.stockTransferDets()[i].Status("Submitted");
					    //else
					    //    self.stockTransferDets()[i].Status("Rejected");
						DAL.fnChangeStockTransferQty(self.stockTransferDets()[i].StockTransferDetRefNo,qty, function (result) {
							if (result.status != "") {
								BootstrapDialog.alert(result.status);
								return;
							};
						});					
					};
				};
			};
			$("#updateQtyModal").modal("hide");			
			return;
		};		
		
		self.cancelQty = function() {
			$("#updateQtyModal").modal("hide");			
			return;
		};					
		
        self.changeInventoryLocation = function (location) {
            self.inventoryLocation(location);
            self.inventoryLocationFilterText(location.InventoryLocationName);
        };

        self.editInventoryLocationInDetails = function (location) {
            self.inventoryLocationFilterTextDetail(location.InventoryLocationName);
            self.currentStockTransfer().TransferFromLocationID(location.InventoryLocationID);
            self.currentStockTransfer().TransferFromLocation(location.InventoryLocationName);
            self.SaveStockTransfer();
        };

        self.editInventoryLocationInDetailsTo = function (location) {
            self.inventoryLocationFilterTextDetailTo(location.InventoryLocationName);
            self.currentStockTransfer().TransferToLocationID(location.InventoryLocationID);
            self.currentStockTransfer().TransferToLocation(location.InventoryLocationName);
            self.SaveStockTransfer();
        };

        self.changePriceLevel = function (level) {
            if (!self.isReadOnly()) {
                self.selectedPriceLevel(level.Value);
                self.currentStockTransfer().PriceLevel(level.Key);

                updateFactoryPrice();
            }
        };

        function updateFactoryPrice() {
            if (!self.isReadOnly()) {
                DAL.fnChangeSTDetailFactoryPriceAll(self.currentStockTransfer().StockTransferHdrRefNo, self.currentStockTransfer().PriceLevel(), function(result) {
                    if (result.status == "") {
                        for (var i = 0; i < self.stockTransferDets().length; i++) {
                            var oDet = self.stockTransferDets()[i];
                            var plevel = self.currentStockTransfer().PriceLevel();
                            
                            if (plevel == "P1" && oDet.P1Price != null)
                                oDet.FactoryPrice(oDet.P1Price);
                            else if (plevel == "P2" && oDet.P2Price != null)
                                oDet.FactoryPrice(oDet.P2Price);
                            else if (plevel == "P3" && oDet.P3Price != null)
                                oDet.FactoryPrice(oDet.P3Price);
                            else if (plevel == "P4" && oDet.P4Price != null)
                                oDet.FactoryPrice(oDet.P4Price);
                            else if (plevel == "P5" && oDet.P5Price != null)
                                oDet.FactoryPrice(oDet.P5Price);
                        }
                    }
                    else {
                        BootstrapDialog.alert(result.status);
                    };
                });              
            }
        }

        self.setFactoryPrice = function (oDet) {
            if (!self.isReadOnly()) {
                BootstrapDialog.prompt("Factory Price:", poDet.FactoryPrice(), function(price) {
                    if (price != null && !isNaN(price)) {
                        price = parseFloat(price);
                        DAL.fnChangeSTDetailFactoryPrice(oDet.FactoryPrice(), price, function(result) {
                            if (result.status == "") {
                                oDet.FactoryPrice(price);
                            }
                            else {
                                BootstrapDialog.alert(result.status);
                            };
                        });
                    };
                });
            }
        };

        self.changeRemarks = function () {
            self.SaveStockTransfer();
        };

        self.changeDateNeededBy = function () {
            self.currentStockTransfer().RequiredDate(new Date($("#dateNeededBy").val()));
            self.SaveStockTransfer();
        };

        self.isSelectAllTransferDet = ko.computed(function () {
            var countSelected = ko.utils.arrayFilter(self.stockTransferDets(), function (item) {
                return item.isSelected() == true;
            }).length;
            var countVisible = ko.utils.arrayFilter(self.stockTransferDets(), function (item) {
                return item.isVisible() == true;
            }).length;
            return countSelected == countVisible;
        });

        self.toggleSelectAllTransferDet = function () {
            var toggle = !self.isSelectAllTransferDet();
            for (var i = 0; i < self.stockTransferDets().length; i++) {
                if (self.stockTransferDets()[i].isVisible())
                    self.stockTransferDets()[i].isSelected(toggle);
            }
            return true;
        };

        self.rejectCheckedBtnState = ko.computed(function () {
            return ko.utils.arrayFilter(self.stockTransferDets(), function (item) {
                return item.isSelected() == true;
            }).length < 1;
        });

        self.rejectChecked = function () {
            for (var i = 0; i < self.stockTransferDets().length; i++) {
                if (self.stockTransferDets()[i].isSelected()) {
                    self.stockTransferDets()[i].Quantity("0");
                    self.stockTransferDets()[i].Status("Rejected");
                    self.stockTransferDets()[i].isSelected(false);
                    DAL.fnChangeStockTransferQty(self.stockTransferDets()[i].StockTransferDetRefNo, 0, function (result) {
                        if (result.status != "") {
                            BootstrapDialog.alert(result.status);
                            return;
                        };
                    });
                };
            };
        };

        self.isTransferHdrNotFound = ko.computed(function () {
            return self.stockTransferHdrs().length < 1;
        });

        self.isItemNotFound = ko.computed(function () {
            return self.searchItemResults().length < 1;
        });

        self.stockTransferDetsApprovedOrPending = ko.computed(function () {
            return ko.utils.arrayFilter(self.stockTransferDets(), function (item) {
                return item.Status() != "Rejected";
            });
        });

        self.stockTransferDetsRejected = ko.computed(function () {
            return ko.utils.arrayFilter(self.stockTransferDets(), function (item) {
                return item.Status() == "Rejected";
            });
        });

        self.sortHdr = function (sortKey) {
            var currSortKey = self.sortKeyHdr();
            if (currSortKey.columnName == sortKey) {
                currSortKey.asc = !currSortKey.asc;
            }
            else {
                currSortKey.columnName = sortKey;
                currSortKey.asc = true;
            };
            self.sortKeyHdr(currSortKey);
            self.searchStockTransfer();
        };

        self.sortDet = function (sortKey) {
            var currSortKey = self.sortKeyDet();
            if (currSortKey.columnName == sortKey) {
                currSortKey.asc = !currSortKey.asc;
            }
            else {
                currSortKey.columnName = sortKey;
                currSortKey.asc = true;
            };
            self.sortKeyDet(currSortKey);

            self.stockTransferDets.sort(function (a, b) {
                var sortDirection = (self.sortKeyDet().asc) ? 1 : -1;
                var valueA = ko.utils.unwrapObservable(a[self.sortKeyDet().columnName]);
                var valueB = ko.utils.unwrapObservable(b[self.sortKeyDet().columnName]);
                return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
            });
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

        self.inventoryLocationFilteredDetailTo = ko.computed(function () {
            if (self.currentStockTransfer()) {
                var toLocation =  ko.utils.arrayFilter(self.inventoryLocationList(), function (item) {
                                        return item.InventoryLocationName.toUpperCase() != "ALL" &&
                                            item.InventoryLocationID != self.currentStockTransfer().TransferFromLocationID();
                                });
                if (self.inventoryLocationFilterTextDetailTo()) {
                    return ko.utils.arrayFilter(toLocation, function (item) {
                        return item.InventoryLocationName.toUpperCase().indexOf(self.inventoryLocationFilterTextDetailTo().toUpperCase()) == 0 &&
                            item.InventoryLocationName.toUpperCase() != "ALL";
                    });
                }                             
                return toLocation;
            }
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
        
        self.inventoryLocationFilterTextDetailKeyUpTo = function (obj, e) {
            if (self.inventoryLocationFilteredDetailTo().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');

            var match = ko.utils.arrayFirst(self.inventoryLocationList(), function (item) {
                return item.InventoryLocationName.toUpperCase() == self.inventoryLocationFilterTextDetailTo().toUpperCase() &&
                       item.InventoryLocationName.toUpperCase() != "ALL";
            });

            if (match) self.editInventoryLocationInDetailsTo(match);
        };        

        self.bindDatePicker = function () {
            $("#dateNeededBy").datepicker({ autoclose: true, format: "d M yyyy", todayHighlight: true });
            $("#dateNeededBy").datepicker().on('hide', function (e) { self.changeDateNeededBy() });
            $("#dateNeededBy").datepicker('update');
        };

		self.showPreviewItem = function () {
			self.isPreview(!self.isPreview());		
			for (var i = 0; i < self.itemDepartments().length; i++) {
				if(self.itemDepartments()[i].isSelected()) {
					for (var j = 0; j < self.stockTransferDets().length; j++) {
						if (self.stockTransferDets()[j].ItemDepartmentID == self.itemDepartments()[i].ItemDepartmentID 
								|| self.itemDepartments()[i].ItemDepartmentID == "--ALL--") {
							if(self.isPreview()) {
								self.stockTransferDets()[j].isVisible(self.stockTransferDets()[j].Quantity() > 0);								
							}
							else {
								self.stockTransferDets()[j].isVisible(true);															
							};
						}
						else {
							self.stockTransferDets()[j].isVisible(false);
						};
					};
				};
			};
			if(self.itemDepartments().length == 0){
				for (var j = 0; j < self.stockTransferDets().length; j++) {
					if(self.isPreview()) {
						self.stockTransferDets()[j].isVisible(self.stockTransferDets()[j].Quantity() > 0);								
					}
					else {
						self.stockTransferDets()[j].isVisible(true);															
					};
				};			
			};
		};		
		
        self.deptFilterClick = function (obj, e) {
            for (var i = 0; i < self.itemDepartments().length; i++) {
                if (self.itemDepartments()[i] == obj) {
                    self.itemDepartments()[i].isSelected(true);

                    // Set visibility of Purchase Order Detail
                    for (var j = 0; j < self.stockTransferDets().length; j++) {
                        if (self.stockTransferDets()[j].ItemDepartmentID == obj.ItemDepartmentID || obj.ItemDepartmentID == "--ALL--") {
							if(self.isPreview()) {
								self.stockTransferDets()[j].isVisible(self.stockTransferDets()[j].Quantity() > 0);								
							}
							else {
								self.stockTransferDets()[j].isVisible(true);															
							};
                        }
                        else {
                            self.stockTransferDets()[j].isVisible(false);
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
        
        self.print = function(PurchaseOrderHeaderRefNo) {
            var urlReport = settings.crReportLocation + "?r=StockTransferPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&ShowBarcode=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
            //window.print();
        };

        self.loadData();

    };

    $("form").submit(function (e) {
        e.preventDefault();
    });
});

