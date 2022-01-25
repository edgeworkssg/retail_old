/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />

$(function() {
    var dateDisplayFormat = settings.dateDisplayFormat;
    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;

    $("#divApprovalDetails").hide();
    $("#dateFrom").datepicker({ autoclose: true, format: "d MM yyyy", todayHighlight: true });
    $("#dateTo").datepicker({ autoclose: true, format: "d MM yyyy", todayHighlight: true });

    DAL.fnGetInventoryLocationList(function(locations) {
        locations.unshift({ InventoryLocationID: "0", InventoryLocationName: "ALL" });
        ko.applyBindings(new OrderApprovalViewModel(locations), document.getElementById("dynamicContent"));
    });

    function PurchaseOrderHeader(data) {
        var self = this;

        self.PurchaseOrderHeaderRefNo = data.PurchaseOrderHeaderRefNo;
        self.PurchaseOrderDate = new Date(parseInt(data.PurchaseOrderDate.replace("/Date(", "").replace(")/", "")));
        self.Status = ko.observable(data.Status);
        self.Remark = data.Remark;
        self.InventoryLocationID = data.InventoryLocationID;
        self.InventoryLocationName = data.InventoryLocation.InventoryLocationName;
        self.ApprovalDate = new Date(parseInt(data.ApprovalDate.replace("/Date(", "").replace(")/", "")));
        self.DateNeededBy = new Date(parseInt(data.DateNeededBy.replace("/Date(", "").replace(")/", "")));
        self.poType = data.poType;
        self.PurchaseOrderDateFormatted = ko.computed(function() {
            return new Date(self.PurchaseOrderDate).toString(dateDisplayFormatWithTime);
        });

        self.DateNeededByFormatted = ko.computed(function() {
            return new Date(self.DateNeededBy).toString(dateDisplayFormatWithTime);
        });

        self.ApprovalDateFormatted = ko.computed(function() {
            return new Date(self.ApprovalDate).toString(dateDisplayFormatWithTime);

            //self.AutoStockIn = ko.Observable("No");		

        });
    };

    function PurchaseOrderDetail(data) {
        var self = this;

        self.PurchaseOrderHeaderRefNo = data.PurchaseOrderHeaderRefNo;
        self.PurchaseOrderDetailRefNo = data.PurchaseOrderDetailRefNo;
        self.ItemNo = data.ItemNo;
        self.ItemName = data.ItemName;
        self.QtyInWH = data.QtyInWH;
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
    };

    // Knockout.js ViewModel
    function OrderApprovalViewModel(locations) {
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

        self.statusDropdown = ["ALL", "Submitted", "Approved", "Rejected", "Posted"];
        self.savedFilter = null;

        self.selectedDate = ko.observable(self.dateDropdown[0]);
        self.status = ko.observable("Submitted");
        self.purchaseOrderHeaders = ko.observableArray();
        self.purchaseOrderDetails = ko.observableArray();
        self.currentPurchaseOrder = ko.observable();
        self.BackPurchaseOrder = ko.observable();
        self.isReadOnly = ko.observable(false);
        self.showLoadMore = ko.observable(false);
        self.sortKeyPOHdr = ko.observable({ columnName: "PurchaseOrderDate", asc: false });
        self.sortKeyPODet = ko.observable({ columnName: "ItemNo", asc: true });
        self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.inventoryLocationList = ko.observableArray(locations);
        self.inventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function(item) {
            return item.InventoryLocationID == (self.allowChangeClinic ? 0 : sessionStorage.locationID);
        }));
        self.inventoryLocationFilterText = ko.observable(self.inventoryLocation().InventoryLocationName);
        //self.proceedToPrint = ko.observable(false);
        //self.autoStockIn = ko.Observable("No");	
        self.radioSelectedOptionValue = ko.observable("No")

        self.potypeDropdown = ["Replenish", "Back Order", "Special Order", "Pre Order"];
        self.typepo = ko.observable("Replenish");

        self.IsCanPrint = ko.computed(function() {
            return self.status() == 'Submitted' || self.status() == 'Approved' || self.status() == 'Received' || self.status() == 'Rejected' || self.status() == 'Posted';
        });

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

        self.loadData = function() {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                filter = {
                    frompage: "OrderApproval",
                    startdate: self.selectedDate().startDate,
                    enddate: self.selectedDate().endDate,
                    inventorylocationid: self.inventoryLocation().InventoryLocationID,
                    status: self.status(),
                    potype: self.typepo()
                };
                self.savedFilter = filter;
            };

            DAL.fnGetPurchaseOrderHeaderList(JSON.stringify(filter), self.purchaseOrderHeaders().length, settings.numOfRecords, self.sortKeyPOHdr().columnName, self.sortKeyPOHdr().asc, function(data) {
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

                    self.currentPurchaseOrder(orderHdr);
                    self.purchaseOrderDetails.removeAll();
                    self.loadPurchaseOrderDetails(orderHdr.PurchaseOrderHeaderRefNo);
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
                            QtyInWH: result.records[i].WarehouseBalance,
                            Quantity: result.records[i].PurchaseOrderDetail.Quantity,
                            QtyApproved: result.records[i].PurchaseOrderDetail.QtyApproved,
                            Remark: result.records[i].PurchaseOrderDetail.Remark,
                            Status: result.records[i].PurchaseOrderDetail.Status,
                            UOM: result.records[i].PurchaseOrderDetail.Item.UOM,
                            BaseLevel: (result.records[i].PurchaseOrderDetail.Item.BaseLevel == "0") ? "1" : result.records[i].PurchaseOrderDetail.Item.BaseLevel
                        };
                        self.purchaseOrderDetails.push(new PurchaseOrderDetail(poDet));
                    };

                    self.sortKeyPODet({ columnName: "ItemNo", asc: false });
                    self.sortPODet('ItemNo');

                    $("#divBackToTop").backToTop({
                        scrollTop: $("#backToFirstProduct").position().top - 70,
                        offset: $("#backToFirstProduct").position().top - 70
                    });

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


        self.submitApproval = function() {

            var poApproval = function() {
                DAL.fnPurchaseOrderApproval(JSON.parse(ko.toJSON(self.purchaseOrderDetails())), self.currentPurchaseOrder().PurchaseOrderHeaderRefNo, sessionStorage.username, self.radioSelectedOptionValue(), function(result) {
                    if (result.status == "") {
                        alert("Done approve " + self.currentPurchaseOrder().PurchaseOrderHeaderRefNo);
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

            BootstrapDialog.confirm("Are you sure you want to submit this Approval?", function(ok) {
                if (ok) {
                    for (var i = 0; i < self.purchaseOrderDetails().length; i++) {
                        var poDet = self.purchaseOrderDetails()[i];
                        if (poDet.QtyApproved() > poDet.QtyInWH) {
                            self.askBackOrder = true;
                            //BootstrapDialog.alert(poDet.ItemName + ": Approved Quantity is greater than Warehouse Balance");
                        };

                        if (Math.ceil(poDet.QtyApproved() / poDet.BaseLevel) != (poDet.QtyApproved() / poDet.BaseLevel)) {
                            BootstrapDialog.alert(poDet.ItemName + ": Approved Quantity is not a multiply of base level value");
                            return;
                        };

                        if (poDet.QtyApproved() != poDet.Quantity && (!poDet.Remark() || poDet.Remark().trim() == "")) {
                            BootstrapDialog.alert(poDet.ItemName + ": Remarks is mandatory if Approved Quantity is not equal to Ordered Quantity");
                            return;
                        };

                        if (poDet.QtyApproved() > 0) {
                            poDet.Status("Approved");
                        }
                        else {
                            poDet.Status("Rejected");
                        };
                    };


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
                if (qty > poDet.QtyInWH) {
                    BootstrapDialog.alert(poDet.ItemName + ": Approved Quantity is greater than Warehouse Balance");
                    return;
                };

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
                        qty = parseInt(qty);
                        var roundedQty = Math.ceil(qty / poDet.BaseLevel) * poDet.BaseLevel;

                        if (Math.ceil(qty / poDet.BaseLevel) != (qty / poDet.BaseLevel)) {
                            BootstrapDialog.confirm("Base level is " + poDet.BaseLevel + ". Quantity will be changed to " + roundedQty, function(ok) {
                                if (ok) {
                                    qty = roundedQty;
                                    updateQty(qty);
                                }
                                else {
                                    return;
                                };
                            });
                        }
                        else {
                            updateQty(qty);
                        };
                    };
                });
            };
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
            var urlReport = settings.crReportLocation + "?r=OrderPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
            //window.print();
        };

        self.printFromDetails = function() {
            self.print(self.currentPurchaseOrder().PurchaseOrderHeaderRefNo);
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


        self.loadData();

    };

    $("form").submit(function(e) {
        e.preventDefault();
    });
});

