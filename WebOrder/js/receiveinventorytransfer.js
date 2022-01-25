/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />
/// <reference path="bootstrap-dialog.min.js" />
/// <reference path="bootstrap-toggle.min.js" />

$(function () {
    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;
    var dateDisplayFormat = settings.dateDisplayFormat;
    
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
        ko.applyBindings(new ReceiveTransferViewModel(locations), document.getElementById("dynamicContent"));
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
        
        self.VarianceQuantity = ko.computed(function () {
            return self.Quantity() != null && self.FullFilledQuantity() != null ? self.FullFilledQuantity() - self.Quantity() : 0;
        }); 
                  
        self.isSelected = ko.observable(false);
        self.isVisible = ko.observable(true);
    };

    function ItemDepartment(data) {
        var self = this;
        self.ItemDepartmentID = data.ItemDepartmentID;
        self.DepartmentName = data.DepartmentName;
        self.DepartmentOrder = data.DepartmentOrder;
        self.isSelected = ko.observable(false);
    };

    // Knockout.js ViewModel
    function ReceiveTransferViewModel(locations) {
        var self = this;

        self.statusDropdown = ko.observableArray(["ALL", "Submitted", "Received", "Rejected"]);
        self.status = ko.observable("Submitted");
        if (sessionStorage.isUseTransferApproval.toUpperCase() == "TRUE") {
            self.statusDropdown(["ALL", "Approved", "Received", "Rejected"]);
            self.status("Approved");
        }

        self.savedFilter = null;
        self.savedItemFilter = null;
        //self.allowChangeClinic = JSON.parse(sessionStorage.privileges).indexOf("Allow Change Inventory Location") > -1;
        self.searchOrderNumber = ko.observable("");
        self.stockTransferHdrs = ko.observableArray();
        self.stockTransferDets = ko.observableArray();
        self.currentStockTransfer = ko.observable();
        
        self.isReceived = ko.observable(false);        
        self.showLoadMore = ko.observable(false);
        self.isPreview = ko.observable(false);		
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
        
        self.sortKeyHdr = ko.observable({ columnName: "StockTransferHdrRefNo", asc: false });
        self.sortKeyDet = ko.observable({ columnName: "CategoryName", asc: false });
        
		self.SelectedStockTransferDetRefNo = ko.observable("");
        self.RequiredDate = ko.observable("");

        self.showDeptFilter = false;
        self.itemDepartments = ko.observableArray();

        self.loadData = function () {
            var filter;
            if (self.savedFilter) {
                filter = self.savedFilter;
            }
            else {
                filter = {
                    StockTransferHdrRefNo: self.searchOrderNumber(),
                    FromInventoryLocationID: 0,
                    ToInventoryLocationID: self.inventoryLocation().InventoryLocationID,
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
        
        self.stockTransferGoThroughWarehouse = ko.observable(false);
        DAL.fnGetSetting("StockTransferWillGoThroughWarehouse", "yes", function (res) {
            if (res == "True") {
                self.stockTransferGoThroughWarehouse(true);
            };
        });

        
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

                    var usetransferapproval = sessionStorage.isUseTransferApproval.toUpperCase() == "TRUE";

                    $("#divStockTransferList").hide();
                    $("#divStockTransferModify").fadeIn();
                    self.isReceived((!usetransferapproval && transferHdr.Status() == "Submitted") || (usetransferapproval && transferHdr.Status() == "Approved") ? false : true);
                    self.IsCanPrint(self.stockTransferGoThroughWarehouse() && (transferHdr.Status() == 'Approved' || transferHdr.Status() == 'Received' || transferHdr.Status() == 'Rejected' || transferHdr.Status() == 'Posted') ? true : false);
                    self.currentStockTransfer(transferHdr);
                    self.stockTransferDets.removeAll();
                    self.loadStockTransferDetails(transferHdr.StockTransferHdrRefNo);
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
					self.showDeptFilter = false;
                    self.fillStockTransferDetails(result);
                    $("#divItemDeptFilter").trigger('detach.ScrollToFixed');
                    $("#divItemDeptFilter").scrollToFixed({ marginTop: 50 });
					$("#theadStockTransfer").trigger('detach.ScrollToFixed');
                    $("#theadStockTransfer").scrollToFixed({ marginTop:  $("#divItemDeptFilter").height() + 50 });
                    ResizeThead();
                   
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
                            DepartmentOrder: result.records[i].DepartmentOrder
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
                self.itemDepartments.push(new ItemDepartment({ ItemDepartmentID: "--ALL--", DepartmentName: "ALL", DepartmentOrder: 9999 }));
                self.deptFilterClick(self.itemDepartments()[0]);
            };
        };

        self.backToList = function () {
            self.showDeptFilter = false;
            $("#divStockTransferModify").hide();
            $("#divStockTransferList").fadeIn();
            $("#divItemDeptFilter").trigger('detach.ScrollToFixed');
            
            self.searchStockTransfer();
        };

        self.setTallyAllStockTransfer = function () {

            BootstrapDialog.confirm("Are you sure you want to tally the selected items?", function (ok) {
                if (ok) {
                    var itemsToUpdate = ko.utils.arrayFilter(self.stockTransferDets(), function (item) {
                        return item.isSelected() == true;
                    });
                    var listToUpdate = new Array();
                    for (var i = 0; i < itemsToUpdate.length; i++) {
                        listToUpdate.push(itemsToUpdate[i].StockTransferDetRefNo);
                    };
					
					DAL.fnSetTallyAllStockTransfer(JSON.stringify(listToUpdate), function (result) {
						if (result.status == "") {
							self.stockTransferDets.removeAll();
							self.loadStockTransferDetails(self.currentStockTransfer().StockTransferHdrRefNo);
						}
						else {
							BootstrapDialog.alert(result.status);
						};
					});
                };
            });
        };

        self.rejectSelectedTransferDet = function () {
            BootstrapDialog.confirm("Are you sure you want to reject the selected items?", function (ok) {
                if (ok) {
                    var itemsToUpdate = ko.utils.arrayFilter(self.stockTransferDets(), function (item) {
                        return item.isSelected() == true;
                    });
                    var listToUpdate = new Array();
                    for (var i = 0; i < itemsToUpdate.length; i++) {
                        listToUpdate.push(itemsToUpdate[i].StockTransferDetRefNo);
                    };
					
					DAL.fnRejectTransferDetail(JSON.stringify(listToUpdate), function (result) {
						if (result.status == "") {
							self.stockTransferDets.removeAll();
							self.loadStockTransferDetails(self.currentStockTransfer().StockTransferHdrRefNo);
						}
						else {
							BootstrapDialog.alert(result.status);
						};
					});
                };
            });
        };

        self.receiveAllTalyStockTransfer = function () {
            
            BootstrapDialog.confirm("Are you sure you want to all tally this Stock Transfer?", function (ok) {
                if (ok) {
					DAL.fnReceiveAllTallyStockTransfer(self.currentStockTransfer().StockTransferHdrRefNo, function (result) {
						if (result.status == "") {
							self.stockTransferDets.removeAll();
							self.loadStockTransferDetails(self.currentStockTransfer().StockTransferHdrRefNo);                            
						}
						else {
							BootstrapDialog.alert(result.status);
						};
					});
                };
            });
        };

        self.receiveStockTransfer = function () {
            
            var itemsToUpdate = ko.utils.arrayFilter(self.stockTransferDets(), function (item) {
                return !isNaN(parseFloat(item.FullFilledQuantity())) && isFinite(item.FullFilledQuantity());;
            });
            
            if(itemsToUpdate.length==0)
            {
                BootstrapDialog.alert("Please set the fullfilled qty");
                return;                
            }
            
            BootstrapDialog.confirm("Are you sure you want to receive this Stock Transfer?", function (ok) {
                if (ok) {
					DAL.fnReceiveStockTransfer(self.currentStockTransfer().StockTransferHdrRefNo, function (result) {
						if (result.status == "") {
							BootstrapDialog.alert("Stock Transfer Received Successfully");
                            self.backToList();
						}
						else {
							BootstrapDialog.alert(result.status);
						};
					});
                };
            });
        };

        self.stockTransferDetsApproved = ko.computed(function () {
            return ko.utils.arrayFilter(self.stockTransferDets(), function (item) {
                return item.Status() != "Rejected";
            });
        });

        self.stockTransferDetsRejected = ko.computed(function () {
            return ko.utils.arrayFilter(self.stockTransferDets(), function (item) {
                return item.Status() == "Rejected";
            });
        });

        self.print = function (PurchaseOrderHeaderRefNo) {
            var urlReport = settings.crReportLocation + "?r=StockTransfer.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&ShowBarcode=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
            //window.print();
        };

        self.printFromDetails = function () {
            self.print(self.currentStockTransfer().StockTransferHdrRefNo);
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
			if(self.isReceived())
				return;
			self.SelectedStockTransferDetRefNo(poDet.StockTransferDetRefNo);
			$("#txtUpdateQty").val(poDet.FullFilledQuantity());		
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
					if(self.stockTransferDets()[i].StockTransferDetRefNo == self.SelectedStockTransferDetRefNo()){
						currIndex = i;
						break;		
					};
				};
				
				if(currIndex!=-1){
				    self.stockTransferDets()[currIndex].FullFilledQuantity(qty);
				    DAL.fnChangeFullFilledQty(self.stockTransferDets()[currIndex].StockTransferDetRefNo, qty, self.stockTransferDets()[i].Remark(), function (result) {
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
					if(self.stockTransferDets()[i].StockTransferDetRefNo == self.SelectedStockTransferDetRefNo()){
						self.stockTransferDets()[i].FullFilledQuantity(qty);
						DAL.fnChangeFullFilledQty(self.stockTransferDets()[i].StockTransferDetRefNo, qty, self.stockTransferDets()[i].Remark(), function (result) {
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
		
        self.setRemark = function (poDet) {
            if (!self.isReceived()) {
                BootstrapDialog.prompt("Remarks:", (poDet.Remark() ? poDet.Remark() : ""), function (remark) {
                    if (remark != null) {
                        poDet.Remark(remark.trim());
                        if (poDet.FullFilledQuantity() == null || poDet.FullFilledQuantity() == "")
                            poDet.FullFilledQuantity(0);
                        DAL.fnChangeFullFilledQty(poDet.StockTransferDetRefNo, poDet.FullFilledQuantity(), remark, function (result) {
                            if (result.status != "") {
                                BootstrapDialog.alert(result.status);
                            };
                        });
                    };
                });
            };
        };        
        
		self.cancelQty = function() {
			$("#updateQtyModal").modal("hide");			
			return;
		};					
		
        self.changeInventoryLocation = function (location) {
            self.inventoryLocation(location);
            self.inventoryLocationFilterText(location.InventoryLocationName);
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

        self.deleteCheckedBtnState = ko.computed(function () {
            return ko.utils.arrayFilter(self.stockTransferDets(), function (item) {
                return item.isSelected() == true;
            }).length < 1;
        });

        self.isTransferHdrNotFound = ko.computed(function () {
            return self.stockTransferHdrs().length < 1;
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

        self.inventoryLocationFilterTextKeyUp = function (obj, e) {
            if (self.inventoryLocationFiltered().length > 0)
                $(e.target.parentNode).addClass('open');
            else
                $(e.target.parentNode).removeClass('open');
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
            var urlReport = settings.crReportLocation + "?r=StockTransferPrintout.rpt&ut=" + sessionStorage.userToken + "&showfilter=false&HideTopBannerMenu=true&DocNo=" + PurchaseOrderHeaderRefNo;
            window.open(urlReport, "_blank", "width=700,height=500,location=0");
            //window.print();
        };


        self.loadData();

    };

    $("form").submit(function (e) {
        e.preventDefault();
    });
});

