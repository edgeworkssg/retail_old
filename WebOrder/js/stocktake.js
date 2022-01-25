/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />
/// <reference path="date.js" />
/// <reference path="setting.js" />

$(function () {
    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;

    $(".dateButton").datepicker({ autoclose: true, format: "d MM yyyy", todayHighlight: true, orientation: "top auto" });

    DAL.fnGetInventoryLocationList(function (locations) {
        ko.applyBindings(new StockTakeViewModel(locations), document.getElementById("dynamicContent"));
    });

    function StockTake(data) {
        var self = this;

        //self.StockTakeDocRefNo = data.StockTakeDocRefNo;
        //self.StockTakeID = data.StockTakeID;
        //self.StockTakeDate = data.StockTakeDate;
        //self.TakenBy = data.TakenBy;
        //self.VerifiedBy = data.VerifiedBy;
        //self.InventoryLocationID = data.InventoryLocationID;
        self.ItemNo = data.ItemNo;
        self.ItemName = data.ItemName;
        self.StockTakeQty = ko.observable(data.StockTakeQty);
        self.FixedQty = data.FixedQty;
        self.LooseQty = data.LooseQty;
        self.UOM = data.UOM;
        self.BaseLevel = data.BaseLevel;
        self.Remark = ko.observable(data.Remark);

        self.isSelected = ko.observable(false);
    };

    function ItemToAdd(data) {
        var self = this;
        self.data = data;
    };

    // Knockout.js ViewModel
    function StockTakeViewModel(locations) {
        var self = this;

        self.savedItemFilter = null;

        self.stockTakes = ko.observableArray();
        self.StockTakeDocRefNo = ko.observable("");
        self.StockTakeDate = ko.observable(new Date(Date.today().toString("yyyy-MM-dd") + " 23:59:59"));
        self.StockTakeDateFormatted = ko.observable(self.StockTakeDate().toString(dateDisplayFormatWithTime));
        self.TakenBy = ko.observable("");
        self.VerifiedBy = ko.observable("");
        self.searchItemResults = ko.observableArray();
        self.selectedItemNo = ko.observable("");
        self.qtyToAdd = ko.observable("");
        self.showLoadMoreItems = ko.observable(false);
        self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.inventoryLocationList = ko.observableArray(locations);
        self.inventoryLocation = ko.observable(ko.utils.arrayFirst(locations, function (item) {
            return item.InventoryLocationID == (self.allowChangeClinic ? -1 : sessionStorage.locationID);
        }));
        self.inventoryLocationFilterText = ko.observable(self.inventoryLocation() ? self.inventoryLocation().InventoryLocationName : "");
        self.sortKey = ko.observable({ columnName: "ItemNo", asc: true });

        $(".dateButton").datepicker('update', new Date(self.StockTakeDate().toString("yyyy-MM-dd") + " 00:00:00"));

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

        self.addItemToList = function () {
            var itemNo = self.selectedItemNo().trim().toUpperCase();
            self.selectedItemNo(itemNo);

            if (!itemNo) {
                BootstrapDialog.alert("Please enter Item No.");
                return;
            };

            // Look for the same item in array and remove then reinsert it on top
            var match = ko.utils.arrayFirst(self.stockTakes(), function (item) {
                return item.ItemNo.trim().toUpperCase() == itemNo;
            });
            if (match) {
                self.stockTakes.remove(match);
                self.stockTakes.unshift(match);
                BootstrapDialog.alert("Duplicate drug found in the list.");
                return;
            };

            if (!self.qtyToAdd()) {
                BootstrapDialog.alert("Please enter Quantity.");
                return;
            };
            if (isNaN(self.qtyToAdd()) || self.qtyToAdd() < 0) {
                BootstrapDialog.alert("Invalid Quantity.");
                return;
            };

            var st = {
                ItemNo: itemNo,
                StockTakeQty: self.qtyToAdd()
            };

            DAL.fnGetItem(itemNo, function (result) {
                if (result.status == "") {
                    st.ItemName = result.Item.ItemName;
                    st.UOM = result.Item.UOM;
                    st.BaseLevel = (result.Item.BaseLevel == 0) ? 1 : result.Item.BaseLevel;
                    self.stockTakes.unshift(new StockTake(st));
                    self.selectedItemNo("");
                    self.qtyToAdd("");

                    self.sortKey({ columnName: "", asc: false });
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.deleteSelectedLine = function () {
            BootstrapDialog.confirm("Are you sure you want to delete the selected items?", function (ok) {
                if (ok) {
                    var itemsToDelete = ko.utils.arrayFilter(self.stockTakes(), function (item) {
                        return item.isSelected() == true;
                    });

                    for (var i = 0; i < itemsToDelete.length; i++) {
                        self.stockTakes.remove(itemsToDelete[i]);
                    };
                };
            });
        };

        self.clearForm = function () {
            self.StockTakeDocRefNo("");
            self.StockTakeDate(new Date(Date.today().toString("yyyy-MM-dd") + " 23:59:59"));

            self.inventoryLocation(ko.utils.arrayFirst(locations, function (item) {
                return item.InventoryLocationID == (self.allowChangeClinic ? -1 : sessionStorage.locationID);
            }));
            self.inventoryLocationFilterText(self.inventoryLocation() ? self.inventoryLocation().InventoryLocationName : "");
            
            self.TakenBy("");
            self.VerifiedBy("");
            self.selectedItemNo("");
            self.qtyToAdd("");
            self.searchItemResults.removeAll();
            self.stockTakes.removeAll();
        };

        self.cancelStockTake = function () {
            BootstrapDialog.confirm("Are you sure you want to cancel this Stock Take?", function (ok) {
                if (ok) {
                    self.clearForm();
                };
            });
        };

        self.submitStockTake = function () {
            if (!self.StockTakeDocRefNo().trim()) {
                BootstrapDialog.alert("Please enter Document Number.");
                return;
            };

            if (isNaN(new Date(self.StockTakeDateFormatted()).getTime())) {
                BootstrapDialog.alert("Please enter a valid date.");
                return;
            };

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

            if (!self.TakenBy().trim()) {
                BootstrapDialog.alert("Please specify stock take personnel.");
                return;
            };

            if (!self.VerifiedBy().trim()) {
                BootstrapDialog.alert("Please specify stock take verifier.");
                return;
            };

            if (self.stockTakes().length < 1) {
                BootstrapDialog.alert("Please insert at least 1 item.");
                return;
            };

            // Confirm Stock Take Date
            $("#confirmDateModal").modal({ backdrop: 'static' });
        };

        self.saveStockTake = function () {
            if (isNaN(new Date(self.StockTakeDateFormatted()).getTime())) {
                BootstrapDialog.alert("Please enter a valid date.");
                return;
            };

            $("#confirmDateModal").modal("hide");

            DAL.fnSaveStockTake(JSON.parse(ko.toJSON(self.stockTakes())), self.StockTakeDocRefNo(), self.StockTakeDate().toString("yyyy-MM-dd HH:mm:ss"), self.TakenBy(), self.VerifiedBy(), self.inventoryLocation().InventoryLocationID, function (result) {
                if (result.status == "") {
                    self.clearForm();
                    window.scrollTo(0, 0);
                    BootstrapDialog.alert("Stock Take has been saved successfully.");
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });
        };

        self.changeItemQty = function (st) {
            BootstrapDialog.prompt("Quantity:", st.StockTakeQty(), function (qty) {
                if (qty != null && !isNaN(qty)) {
                    qty = parseFloat(qty);
                    st.StockTakeQty(qty);
                };
            });
        };

        self.changeInventoryLocation = function (location) {
            self.inventoryLocation(location);
            self.inventoryLocationFilterText(location.InventoryLocationName);
            fnInventoryLocationMustBeFrozen(location.InventoryLocationID);
        };
        
        self.changeRemarks = function (st) {
            BootstrapDialog.prompt("Remarks:", (st.Remark() ? st.Remark() : ""), function (remark) {
                if (remark != null) {
                    st.Remark(remark.trim());
                };
            });
        };

        self.isSelectAllLines = ko.computed(function () {
            return ko.utils.arrayFilter(self.stockTakes(), function (item) {
                return item.isSelected() == true;
            }).length == self.stockTakes().length;
        });

        self.toggleSelectAllLines = function () {
            var toggle = !self.isSelectAllLines();
            for (var i = 0; i < self.stockTakes().length; i++) {
                self.stockTakes()[i].isSelected(toggle);
            }
            return true;
        };

        self.deleteCheckedBtnState = ko.computed(function () {
            return ko.utils.arrayFilter(self.stockTakes(), function (item) {
                return item.isSelected() == true;
            }).length < 1;
        });
        
        self.isItemNotFound = ko.computed(function () {
            return self.searchItemResults().length < 1;
        });

        self.sortGrid = function (sortKey) {
            var currSortKey = self.sortKey();
            if (currSortKey.columnName == sortKey) {
                currSortKey.asc = !currSortKey.asc;
            }
            else {
                currSortKey.columnName = sortKey;
                currSortKey.asc = true;
            };
            self.sortKey(currSortKey);

            self.stockTakes.sort(function (a, b) {
                var sortDirection = (self.sortKey().asc) ? 1 : -1;
                var valueA = ko.utils.unwrapObservable(a[self.sortKey().columnName]);
                var valueB = ko.utils.unwrapObservable(b[self.sortKey().columnName]);
                return valueA == valueB ? 0 : (valueA < valueB ? -1 * sortDirection : 1 * sortDirection);
            });
        };

        self.inventoryLocationFiltered = ko.computed(function () {
            if (self.inventoryLocationFilterText()) {
                return ko.utils.arrayFilter(self.inventoryLocationList(), function (item) {
                    return item.InventoryLocationName.toUpperCase().indexOf(self.inventoryLocationFilterText().toUpperCase()) == 0;
                });
            }
            else {
                return self.inventoryLocationList();
            };
        });

        self.inventoryLocationFilterTextKeyUp = function (obj, e) {
            if (self.inventoryLocationFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');
        };

        self.stockTakeDateChanged = function () {
            if (!isNaN(new Date(self.StockTakeDateFormatted()).getTime())) {  // Date validation
                self.StockTakeDate(new Date(self.StockTakeDateFormatted()));
            }
            else {
                $(".dateButton").datepicker('update', null);
            };
        };

        self.StockTakeDate.subscribe(function (newValue) {
            self.StockTakeDateFormatted(newValue.toString(dateDisplayFormatWithTime));
            $(".dateButton").datepicker('update', new Date(newValue.toString("yyyy-MM-dd") + " 00:00:00"));
        });

        $(".dateButton").datepicker().on("hide", function (e) {
            if (e.date) {
                var currTime = self.StockTakeDate().toString("HH:mm:ss");
                var newDateTime = e.date.toString("yyyy-MM-dd") + " " + currTime;
                self.StockTakeDate(new Date(newDateTime));
            };
        });

        if (self.inventoryLocation() && self.inventoryLocation().InventoryLocationID != 0) {
            fnInventoryLocationMustBeFrozen(self.inventoryLocation().InventoryLocationID);
        };

        // UPLOAD FILE
        var url = connection.serverAddress + "/synchronization/StockTakeImporter.ashx";
        $('#fileupload').fileupload({
            url: url,
            dataType: 'json',
            start: function (e) {
                $('#progress').show();
                $('#progress .progress-bar').css('width', '0%');
            },
            done: function (e, data) {
                $('#progress').hide();

                if (data.result.status == "") {
                    self.stockTakes.removeAll();
                    for (var i = 0; i < data.result.records.length; i++) {
                        var st = new StockTake(data.result.records[i]);
                        self.stockTakes.push(st);
                    };
                    self.sortKey({ columnName: "", asc: false });
                    BootstrapDialog.alert("The file has been imported successfully.");
                }
                else {
                    BootstrapDialog.alert(data.result.status);
                }
            },
            fail: function (e, data) {
                $('#progress').hide();
                BootstrapDialog.alert("Error occured: " + data.errorThrown);
            },
            progressall: function (e, data) {
                var progress = parseInt(data.loaded / data.total * 100, 10);
                $('#progress .progress-bar').css(
                    'width',
                    progress + '%'
                );
            }
        }).prop('disabled', !$.support.fileInput)
            .parent().addClass($.support.fileInput ? undefined : 'disabled');

    };

    $("#divNewProducts").scrollToFixed({ marginTop: 50 });
    $("#divBackToTop").backToTop({
        scrollTop: $("#backToFirstProduct").position().top - 70,
        offset: $("#backToFirstProduct").position().top - 70
    });

    $("form").submit(function (e) {
        e.preventDefault();
    });

});