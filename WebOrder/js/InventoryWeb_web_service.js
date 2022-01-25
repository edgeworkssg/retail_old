            
InventoryWebWS = {
    fnFail: function (err, customMsg) {
        var errMsg = customMsg + "\n";
        if (err.status == 404) {
            errMsg = errMsg + "Can't connect to the web service. Please check your internet connection or the Service URL in Settings page.";
        }
        else {
            errMsg = errMsg + "Error " + err.status + " : " + err.statusText;
        };
        alert(errMsg);
    },

    fnGetDataTable: function (tableName, syncAll, callback) {
        $.post(localStorage.serviceURL + "/GetDataTable", { tableName: tableName, syncAll: syncAll }, function (loader) {
            var res = new Array();

            if ($.xml2json(loader).diffgram.NewDataSet != null) {
                var data = $.xml2json(loader).diffgram.NewDataSet.Table;

                if (data.length > 0) {
                    $.each(data, function () { res.push(this); });
                }
                else {
                    res.push(data);
                }

                if (callback) callback(res);
            }
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Loading Data From Server.") });
    },

    fnLogin: function (username, password, callback) {
        $.post(localStorage.serviceURL + "/LoginWebOrderSupplierPortal", { username: username, password: password }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Login") });
    },

    fnCheckUserToken: function(userToken, callback) {
        $.post(localStorage.serviceURL + "/CheckUserToken", { userToken: userToken }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Validating User Token") });
    },

    fnSavePurchaseOrderHeader: function (data, username, callback) {
        $.post(localStorage.serviceURL + "/SavePurchaseOrderHeaderXY", { data: data, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Save Order") });
    },
	
	fnSavePurchaseOrderHeaderCreateItems: function (data, username, createItems, callback) {
	    $.post(localStorage.serviceURL + "/SavePurchaseOrderHeaderCreateItems", { data: data, username: username, createItems: createItems }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Save Order") });
    },

    fnGetPurchaseOrderHeaderList: function (filter, skip, take, sortBy, isAscending, callback) {
        $.post(localStorage.serviceURL + "/GetPurchaseOrderHeaderList", { filter: filter, skip: skip, take: take, sortBy: sortBy, isAscending: isAscending }, function (loader) {
            var res = $.xml2json(loader).text;
	    if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Order List") });
    },

    fnGetPurchaseOrderHeaderListWithOutletName: function(filter, skip, take, sortBy, isAscending, callback) {
        $.post(localStorage.serviceURL + "/GetPurchaseOrderHeaderListWithOutletName", { filter: filter, skip: skip, take: take, sortBy: sortBy, isAscending: isAscending }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function(err) { InventoryWebWS.fnFail(err, "Error Get Order List") });
    },

    fnGetPurchaseOrderHeader: function (PurchaseOrderHeaderRefNo, callback) {
        $.post(localStorage.serviceURL + "/GetPurchaseOrderHeader", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Order Header") });
    },

    fnGetPurchaseOrderDetailList: function (PurchaseOrderHeaderRefNo, callback) {
        $.post(localStorage.serviceURL + "/GetPurchaseOrderDetailList", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Order Detail") });
    },

    fnGetPurchaseOrderDetailListSupplierPortal: function (PurchaseOrderHeaderRefNo, username, numOfDays, isShowSalesQty, callback) {
        $.post(localStorage.serviceURL + "/GetPurchaseOrderDetailListSupplierPortal", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, username: username, numOfDays: numOfDays, isShowSalesQty: isShowSalesQty }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Order Detail") });
    },

    fnSavePurchaseOrderDetail: function (data, username, callback) {
        $.post(localStorage.serviceURL + "/SavePurchaseOrderDetailXY", { data: data, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Save Order Detail") });
    },

    fnSavePurchaseOrderDetailXYSupplierPortal: function (data, username, numOfDays, isShowSalesQty, callback) {
        $.post(localStorage.serviceURL + "/SavePurchaseOrderDetailXYSupplierPortal", { data: data, username: username, numOfDays: numOfDays, isShowSalesQty: isShowSalesQty }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Save Order Detail") });
    },

    fnDeletePurchaseOrderDetail: function (data, callback) {
        $.post(localStorage.serviceURL + "/DeletePurchaseOrderDetail", { data: data }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Delete Order Detail") });
    },
    
    fnUpdateSalesPersonPurchaseOrder: function (PurchaseOrderHeaderRefNo, username, callback) {
        $.post(localStorage.serviceURL + "/UpdateSalesPersonPurchaseOrder", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Update Sales Person") });
    },

    fnDeletePurchaseOrder: function (PurchaseOrderHeaderRefNo, callback) {
        $.post(localStorage.serviceURL + "/DeletePurchaseOrder", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Delete Order") });
    },

    fnChangePODetailQty: function (data, callback) {
        $.post(localStorage.serviceURL + "/ChangePODetailQtyXY", { data: data }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Update Quantity") });
    },

    fnChangePODetailFactoryPrice: function (PurchaseOrderDetailRefNo, price, username, callback) {
        $.post(localStorage.serviceURL + "/ChangePODetailFactoryPrice", { PurchaseOrderDetailRefNo: PurchaseOrderDetailRefNo, price: price, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Change PO Detail Factory Price") });
    },

    fnChangePODetailFactoryPriceAll: function(PurchaseOrderHeaderRefNo, levelPrice, username, callback) {
        $.post(localStorage.serviceURL + "/ChangePODetailFactoryPriceAll", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, levelPrice: levelPrice, username: username }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function(err) { InventoryWebWS.fnFail(err, "Error Change PO Detail Factory Price All") });
    },

    fnChangePOQtyApproved: function (data, callback) {
        $.post(localStorage.serviceURL + "/ChangePOQtyApproved", { data: data }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Update Approved Quantity") });
    },
    
    fnChangeApprovedPOApprovedQty: function (data, callback) {
    $.post(localStorage.serviceURL + "/ChangeApprovedPOApprovedQtyWithOrderFrom", { data: data }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Update Approved Quantity") });
    },

    fnChangeCreditInvoiceNo: function (PurchaseOrderHeaderRefNo, CreditInvoiceNo, username, callback) {
        $.post(localStorage.serviceURL + "/ChangeCreditInvoiceNo", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, CreditInvoiceNo: CreditInvoiceNo, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Change Credit Invoice No") });
    },

    fnChangeCreditInvoiceNoST: function(StockTransferHdrRefNo, CreditInvoiceNo, username, callback) {
        $.post(localStorage.serviceURL + "/ChangeCreditInvoiceNoST", { StockTransferHdrRefNo: StockTransferHdrRefNo, CreditInvoiceNo: CreditInvoiceNo, username: username }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function(err) { InventoryWebWS.fnFail(err, "Error Change Credit Invoice No") });
    },

    fnChangeInvoiceNoST: function(StockTransferHdrRefNo, InvoiceNo, username, callback) {
        $.post(localStorage.serviceURL + "/ChangeInvoiceNoST", { StockTransferHdrRefNo: StockTransferHdrRefNo, InvoiceNo: InvoiceNo, username: username }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function(err) { InventoryWebWS.fnFail(err, "Error Change Invoice No") });
    },

	fnCreateBackOrder: function (data, PurchaseOrderHeaderRefNo, username, callback) {
		$.post(localStorage.serviceURL + "/CreateBackOrder", { data: data, PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, username: username }, function (loader) {
			
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { alert("WS fail");
		InventoryWebWS.fnFail(err, "Error Create Back Order") });
    },
	
	fnPurchaseOrderApproval: function (data, PurchaseOrderHeaderRefNo, username, autostockin, priceLevel, callback) {
	$.post(localStorage.serviceURL + "/PurchaseOrderApprovalGRWithPriceLevelOrderFrom", { data: data, PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, username: username, autoStockIn: autostockin, priceLevel: priceLevel }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Approve Order") });
    },
	
	fnPurchaseOrderApprovalAutoApprove: function (PurchaseOrderHeaderRefNo, username, autostockin, callback) {
	    $.post(localStorage.serviceURL + "/PurchaseOrderApprovalAutoApproveWithOrderFrom", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, username: username, autoStockIn: autostockin }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Auto Approve Order") });
    },

    fnGetItemsToReceive: function (PurchaseOrderHeaderRefNo, InventoryLocationID, callback) {
        $.post(localStorage.serviceURL + "/GetItemsToReceive", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, InventoryLocationID: InventoryLocationID }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Items to Receive") });
    },

    fnStockIn: function (PurchaseOrderHeaderRefNo, data, username, StockInReasonID, InventoryLocationID, IsAdjustment, CalculateCOGS, Remark, callback) {
        $.post(localStorage.serviceURL + "/StockIn", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, detail: data, username: username, StockInReasonID: StockInReasonID, InventoryLocationID: InventoryLocationID, IsAdjustment: IsAdjustment, CalculateCOGS: CalculateCOGS, Remark: Remark }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error in processing Stock In") });
    },

    fnStockOut: function (PurchaseOrderHeaderRefNo, data, username, StockOutReasonID, InventoryLocationID, IsAdjustment, deductRemainingQty, Remark, callback) {
        $.post(localStorage.serviceURL + "/StockOut", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, detail: data, username: username, StockOutReasonID: StockOutReasonID, InventoryLocationID: InventoryLocationID, IsAdjustment: IsAdjustment, deductRemainingQty: deductRemainingQty, Remark: Remark }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error in processing Stock Out") });
    },

    fnStockReturn: function (PurchaseOrderHeaderRefNo, data, username, StockOutReasonID, InventoryLocationID, IsAdjustment, calculateCOGS, Remark, callback) {
        $.post(localStorage.serviceURL + "/StockReturn", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, detail: data, username: username, StockOutReasonID: StockOutReasonID, InventoryLocationID: InventoryLocationID, IsAdjustment: IsAdjustment, calculateCOGS: calculateCOGS, Remark: Remark }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error in processing Stock Return") });
    },

    fnStockTransfer: function (PurchaseOrderHeaderRefNo, data, username, FromInventoryLocationID, ToInventoryLocationID, callback) {
        $.post(localStorage.serviceURL + "/StockTransfer", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, detail: data, username: username, FromInventoryLocationID: FromInventoryLocationID, ToInventoryLocationID: ToInventoryLocationID }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error in processing Stock Transfer") });
    },

    fnSearchItem: function (name, IsInventoryItemOnly, ShowSystemItem, ShowDeletedItem, skip, take, callback) {
        $.post(localStorage.serviceURL + "/SearchItem", { name: name, IsInventoryItemOnly: IsInventoryItemOnly, ShowSystemItem: ShowSystemItem, ShowDeletedItem: ShowDeletedItem, skip: skip, take: take }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Search Item") });
    },

    fnSearchItemUserPortal: function (name, username, isSupplier, isRestrictedSupplier, supplierID, isShowSalesQty, numOfDays, InventoryLocationID, IsInventoryItemOnly, ShowSystemItem, ShowDeletedItem, skip, take, callback) {
        $.post(localStorage.serviceURL + "/SearchItemUserPortal", { name: name, username: username, isSupplier: isSupplier, isRestrictedSupplier: isRestrictedSupplier, supplierID: supplierID, isShowSalesQty: isShowSalesQty, numOfDays: numOfDays, InventoryLocationID: InventoryLocationID, IsInventoryItemOnly: IsInventoryItemOnly, ShowSystemItem: ShowSystemItem, ShowDeletedItem: ShowDeletedItem, skip: skip, take: take }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Search Item") });
    },
    
    fnSearchUser: function (name, ShowDeletedUser, skip, take, callback) {
        $.post(localStorage.serviceURL + "/SearchUser", { name: name, ShowDeletedUser: ShowDeletedUser, skip: skip, take: take }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Search User") });
    },

    fnGetItem: function (ItemNo, callback) {
        $.post(localStorage.serviceURL + "/GetItem", { ItemNo: ItemNo }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Item") });
    },

    fnGetItemNo: function (barcode, callback) {
        $.post(localStorage.serviceURL + "/GetItemNo", { barcode: barcode }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Item No") });
    },

    fnSaveStockTakeDoc: function (data, username, callback) {
        $.post(localStorage.serviceURL + "/SaveStockTakeDoc", { data: data, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Save Stock Take Document") });
    },

    fnGetStockTakeDocList: function (filter, skip, take, sortBy, isAscending, callback) {
        $.post(localStorage.serviceURL + "/GetStockTakeDocList", { filter: filter, skip: skip, take: take, sortBy: sortBy, isAscending: isAscending }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Stock Take Document List") });
    },

    fnGetStockTakeDoc: function (StockTakeDocRefNo, callback) {
        $.post(localStorage.serviceURL + "/GetStockTakeDoc", { StockTakeDocRefNo: StockTakeDocRefNo }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Stock Take Document") });
    },

    fnSaveStockTake: function (detail, StockTakeDocRefNo, StockTakeDate, TakenBy, VerifiedBy, InventoryLocationID, username, callback) {
        $.post(localStorage.serviceURL + "/SaveStockTake", { detail: detail, StockTakeDocRefNo: StockTakeDocRefNo, StockTakeDate: StockTakeDate, TakenBy: TakenBy, VerifiedBy: VerifiedBy, InventoryLocationID: InventoryLocationID, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Save Stock Take Entries") });
    },

    fnGetInventoryLocations: function (callback) {
        $.post(localStorage.serviceURL + "/GetInventoryLocations", {}, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Inventory Location") });
    },

    fnGetAllLocationWithOutstandingStockTake: function (callback) {
        $.post(localStorage.serviceURL + "/GetAllLocationWithOutstandingStockTake", {}, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Clinic") });
    },

    fnGetPointOfSales: function (callback) {
        $.post(localStorage.serviceURL + "/GetPointOfSales", {}, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Point Of Sale") });
    },

    fnGetStockTakeListForApproval: function (filter, callback) {
        $.post(localStorage.serviceURL + "/GetStockTakeListForApproval", { filter: filter }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Stock Take List For Stock Take Approval") });
    },

    fnGetStockTakeList: function (StockTakeDocRefNo, callback) {
        $.post(localStorage.serviceURL + "/GetStockTakeList", { StockTakeDocRefNo: StockTakeDocRefNo }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Stock Take List") });
    },

    //fnStockTakeUpdateMarked: function (StockTakeID, bit, callback) {
    //    $.post(localStorage.serviceURL + "/StockTakeUpdateMarked", { StockTakeID: StockTakeID, bit: bit }, function (loader) {
    //        var res = $.xml2json(loader).text;
    //        if (callback) callback(JSON.parse(res));
    //    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Update Marked column") });
    //},

    fnDeleteStockTake: function (data, callback) {
        $.post(localStorage.serviceURL + "/DeleteStockTake", { data: data }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Delete Stock Take") });
    },

    fnStockTakeApproval: function (StockTakeDocRefNo, InventoryLocationID, docStatus, username, callback) {
        $.post(localStorage.serviceURL + "/StockTakeApproval", { StockTakeDocRefNo: StockTakeDocRefNo, InventoryLocationID: InventoryLocationID, docStatus: docStatus, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Approve Stock Take") });
    },

    fnGetNotifications: function (username, callback) {
        $.post(localStorage.serviceURL + "/GetNotifications", { username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Notifications") });
    },

    fnUpdateInventoryLocationIsFrozen: function (InventoryLocationID, IsFrozen, username, callback) {
        $.post(localStorage.serviceURL + "/UpdateInventoryLocationIsFrozen", { InventoryLocationID: InventoryLocationID, IsFrozen: IsFrozen, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Update Clinic's Freeze/Unfreeze Status") });
    },

    fnUpdateInventoryLocationIsFrozen_All: function (IsFrozen, username, callback) {
        $.post(localStorage.serviceURL + "/UpdateInventoryLocationIsFrozen_All", { IsFrozen: IsFrozen, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Update All Clinic's Freeze/Unfreeze Status") });
    },

    fnIsInventoryLocationFrozen: function (InventoryLocationID, callback) {
        $.post(localStorage.serviceURL + "/IsInventoryLocationFrozen", { InventoryLocationID: InventoryLocationID }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Checking Clinic's Freeze/Unfreeze Status") });
    },

    fnRevertPOHeaderStatus: function (PurchaseOrderHeaderRefNo, poStatus, callback) {
        $.post(localStorage.serviceURL + "/RevertPOHeaderStatus", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, poStatus: poStatus }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Revert Document Status") });
    },

    fnGetPurchaseHeaderSetting: function(callback) {
        $.post(localStorage.serviceURL + "/GetPurchaseHeaderSetting", {}, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function(err) { InventoryWebWS.fnFail(err, "Error Get Purchase Header Setting") });
    },

    fnChangePODetailExpiryDate: function (data, callback) {
        $.post(localStorage.serviceURL + "/ChangePODetailExpiryDate", { data: data }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Update Expiry Date") });
    },

    fnChangePassword: function (username, oldpassword, newpassword, callback) {
        $.post(localStorage.serviceURL + "/ChangePassword", { username: username, oldpassword: oldpassword, newpassword: newpassword }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Change Password") });
    },

    fnGetEmailNotificationList: function (filter, skip, take, sortBy, isAscending, callback) {
        $.post(localStorage.serviceURL + "/GetEmailNotificationList", { filter: filter, skip: skip, take: take, sortBy: sortBy, isAscending: isAscending }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Email Notification List") });
    },

    fnGetEmailNotification: function (EmailAddress, callback) {
        $.post(localStorage.serviceURL + "/GetEmailNotification", { EmailAddress: EmailAddress }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Email Notification") });
    },

    fnSaveEmailNotification: function (data, username, callback) {
        $.post(localStorage.serviceURL + "/SaveEmailNotification", { data: data, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Save Email Notification") });
    },

    fnDeleteEmailNotification: function (EmailAddress, callback) {
        $.post(localStorage.serviceURL + "/DeleteEmailNotification", { EmailAddress: EmailAddress }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Delete Email Notification") });
    },

    fnGetParamsForReport: function (queryName, callback) {
        $.post(localStorage.serviceURL + "/GetParamsForReport", { queryName: queryName }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Parameters For Report") });
    },

    fnGetReportDataByQueryName: function (queryName, callback) {
        $.post(localStorage.serviceURL + "/GetReportDataByQueryName", { queryName: queryName }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Report Data By Query Name") });
    },
	
	fnGetSetting: function (AppSettingKey, boolValue, callback) {
		$.post(localStorage.serviceURL + "/GetSetting", { AppSettingKey: AppSettingKey, boolValue: boolValue }, function (loader) {
			var res = $.xml2json(loader).text;
			//alert(res);
            if (callback) callback(res);
			
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Setting") });
    },
	
	fnGetItemBaseLevelList: function (filter, skip, take, sortBy, isAscending, callback) {
        $.post(localStorage.serviceURL + "/GetItemBaseLevelList", { filter: filter, skip: skip, take: take, sortBy: sortBy, isAscending: isAscending }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Item Base Level List") });
    },
	
	fnAddItemBaseLevel: function (data, username, callback) {
        $.post(localStorage.serviceURL + "/AddItemBaseLevel", { data: data, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Add Item Optimal Stock") });
    },

    fnAddItemBaseLevelByCategory: function(data, username, callback) {
        $.post(localStorage.serviceURL + "/AddItemBaseLevelByCategory", { data: data, username: username }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function(err) { InventoryWebWS.fnFail(err, "Error Add Item Optimal Stock") });
    },
    
	fnGetSecondScreenFiles: function (filter, skip, take, sortBy, isAscending, callback) {
	    $.post(localStorage.serviceURL + "/GetSecondScreenFiles", { filter: filter, skip: skip, take: take, sortBy: sortBy, isAscending: isAscending }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Second Screen Files") });
	},

	fnRenameSecondScreenFile: function (data, callback) {
	    $.post(localStorage.serviceURL + "/RenameSecondScreenFile", { data: data }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Rename Second Screen File") });
	},


	fnGetSecondScreenFiles: function (filter, skip, take, sortBy, isAscending, callback) {
	    $.post(localStorage.serviceURL + "/GetSecondScreenFiles", { filter: filter, skip: skip, take: take, sortBy: sortBy, isAscending: isAscending }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Second Screen Files") });
	},

	fnRenameSecondScreenFile: function (data, callback) {
	    $.post(localStorage.serviceURL + "/RenameSecondScreenFile", { data: data }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Rename Second Screen File") });
	},

	fnDeleteSecondScreenFiles: function (data, callback) {
	    $.post(localStorage.serviceURL + "/DeleteSecondScreenFiles", { data: data }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Delete Second Screen Files") });
	},

	fnCopySecondScreenFiles: function (fromPointOfSaleID, toPointOfSaleID, overwrite, callback) {
	    $.post(localStorage.serviceURL + "/CopySecondScreenFiles", { fromPointOfSaleID: fromPointOfSaleID, toPointOfSaleID: toPointOfSaleID, overwrite: overwrite }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Copy Second Screen Files") });
	},
	
	fnDeleteItemOptimalStock: function (data, callback) {
	    $.post(localStorage.serviceURL + "/DeleteItemOptimalStock", { data: data }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Delete Item Optimal Stock") });
	},
	
	fnCopyItemOptimalStock: function (fromInventoryLocationID, toInventoryLocationID, overwrite, callback) {
	    $.post(localStorage.serviceURL + "/CopyItemOptimalStock", { fromInventoryLocationID: fromInventoryLocationID, toInventoryLocationID: toInventoryLocationID, overwrite: overwrite }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Item Optimal Stock") });
	},
	
	fnGetItemBaseLevel: function (baselevelid, callback) {
        $.post(localStorage.serviceURL + "/GetItemBaseLevel", { BaseLevelID: baselevelid }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Item Base Level") });
    },
	
	fnGetCategoryList: function (filter, callback) {
		$.post(localStorage.serviceURL + "/GetCategoryList", { filter: filter}, function (loader) {
            var res = $.xml2json(loader).text;
			if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Category List") });
    },
	
	fnGetItemList: function (filter, callback) {
		$.post(localStorage.serviceURL + "/GetItemList", { filter: filter}, function (loader) {
            var res = $.xml2json(loader).text;
			if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { 
		InventoryWebWS.fnFail(err, "Error GetItem List") });
    },

    fnGetItemListForRecipe: function (filter, IsInventoryItemOnly, IsRecipeHeader, callback) {
	    $.post(localStorage.serviceURL + "/GetItemListForRecipe", { filter: filter, IsInventoryItemOnly: IsInventoryItemOnly, IsRecipeHeader: IsRecipeHeader }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Item List") });
	},
	
	fnLoadItemsFromOptimalStock: function (PurchaseOrderHeaderRefNo, username, callback) {
	    $.post(localStorage.serviceURL + "/LoadItemsFromOptimalStockSupplierPortal", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
			if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Load Item From Optimal Stock") });
    },
	
	fnLoadItemsFromSales: function (PurchaseOrderHeaderRefNo, startDate, endDate, username, callback) {
	    $.post(localStorage.serviceURL + "/LoadItemsFromSalesSupplierPortal", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, startDate: startDate, endDate: endDate, username: username }, function (loader) {
            var res = $.xml2json(loader).text;
			if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Load Item From Sales") });
    },
	
	fnClearPurchaseOrderReference: function (PurchaseOrderHeaderRefNo, callback) {
		$.post(localStorage.serviceURL + "/ClearPurchaseOrderReference", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo}, function (loader) {
            var res = $.xml2json(loader).text;
			if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Clear Purchase Order Reference") });
    },
	
	fnClearPurchaseOrderDetailReference: function (PurchaseOrderDetailRefNo, callback) {
		$.post(localStorage.serviceURL + "/ClearPurchaseOrderDetailReference", { PurchaseOrderDetailRefNo: PurchaseOrderDetailRefNo}, function (loader) {
            var res = $.xml2json(loader).text;
			if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Clear Purchase Order Detail Reference") });
	},

	fnGetSuppliersUserPortal: function(callback) {
	    $.post(localStorage.serviceURL + "/GetSuppliersUserPortal", { username: sessionStorage.username }, function(loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Suppliers") });
    },

    fnGetSuppliersUserPortalWithoutWarehouse: function(callback) {
	    $.post(localStorage.serviceURL + "/GetSuppliersUserPortalWithoutWarehouse", { username: sessionStorage.username }, function(loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Suppliers") });
    },
    
    fnGetWarehouseList: function(callback) {
	    $.post(localStorage.serviceURL + "/GetWarehouseList", {}, function(loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Warehouses") });
	},

	fnGetSuppliers: function(callback) {
	    $.post(localStorage.serviceURL + "/GetSuppliers", {}, function(loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function(err) { InventoryWebWS.fnFail(err, "Error Get Suppliers") });
	},

	fnGetPriceLevel: function (callback) {
	    $.post(localStorage.serviceURL + "/GetPriceLevel", {}, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Price Level") });
	},
    
    fnFetchStockTransferList: function (filter, take, skip, orderBy, orderDir, callback) {
        $.post(localStorage.serviceURL + "/FetchStockTransferList", { filter: filter, take: take, skip: skip, orderBy: orderBy, orderDir: orderDir }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Fetch Stock Transfer List") });
    },
    
    fnFetchStockTransferDet: function (stockTransferHdrRefNo, callback) {
        $.post(localStorage.serviceURL + "/FetchStockTransferDet", { stockTransferHdrRefNo: stockTransferHdrRefNo }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Fetch Stock Transfer Det") });
    },    
    
    fnSaveStockTransferHdr: function (data, userName, callback) {
        $.post(localStorage.serviceURL + "/SaveStockTransferHdr", { data: data, userName: userName }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Save Stock Transfer Hdr") });
    },        
    
    fnFetchStockTransferHdr: function (stockTransferHdrRefNo, callback) {
        $.post(localStorage.serviceURL + "/FetchStockTransferHdr", { stockTransferHdrRefNo: stockTransferHdrRefNo }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Fetch Stock Transfer Hdr") });
    },

    fnRevertStockTransferHdr: function(stockTransferHdrRefNo, userName, callback) {
        $.post(localStorage.serviceURL + "/RevertStockTransferHdr", { stockTransferHdrRefNo: stockTransferHdrRefNo, userName: userName }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function(err) { InventoryWebWS.fnFail(err, "Error Fetch Stock Transfer Hdr") });
    },
    
    fnAddStockTransferItem: function (stockTransferHdrRefNo, itemNo, qty, userName, callback) {
        $.post(localStorage.serviceURL + "/AddStockTransferItem", { stockTransferHdrRefNo: stockTransferHdrRefNo, itemNo: itemNo, qty: qty, userName: userName }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Add Stock Transfer Item") });
    },             
    
    fnAddAllTransferItems: function (stockTransferHdrRefNo, userName, callback) {
        $.post(localStorage.serviceURL + "/AddAllTransferItems", { stockTransferHdrRefNo: stockTransferHdrRefNo, userName: userName }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Add All Transfer Items") });
    },
    
    fnChangeStockTransferQty: function (stockTransferDetRefNo, qty, userName, callback) {
        $.post(localStorage.serviceURL + "/ChangeStockTransferQty", { stockTransferDetRefNo: stockTransferDetRefNo, qty: qty, userName: userName }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Change Stock Transfer Qty") });
    },          

    fnDeleteStockTransferItem: function (stockTransferDetRefNo, userName, callback) {
        $.post(localStorage.serviceURL + "/DeleteStockTransferItem", { stockTransferDetRefNo: stockTransferDetRefNo, userName: userName }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Delete Stock Transfer Item") });
    },
    
    fnChangeFullFilledQty: function (stockTransferDetRefNo, fullFilledQty, remark, userName, callback) {
        $.post(localStorage.serviceURL + "/ChangeFullFilledQty", { stockTransferDetRefNo: stockTransferDetRefNo, fullFilledQty: fullFilledQty, remark: remark, userName: userName }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Change Fullfilled Qty") });
    },    
    
    fnRejectTransferDetail: function (stockTransferDetRefNo, userName, callback) {
        $.post(localStorage.serviceURL + "/RejectTransferDetail", { stockTransferDetRefNo: stockTransferDetRefNo, userName: userName }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Reject Transfer Detail") });
    },        
    
    fnSetTallyAllStockTransfer: function (stockTransferDetRefNo, userName, callback) {
        $.post(localStorage.serviceURL + "/SetTallyAllStockTransfer", { stockTransferDetRefNo: stockTransferDetRefNo, userName: userName }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Set Tally All Stock Transfer") });
    },        
    
    fnReceiveStockTransfer: function (stockTransferHdrRefNo, userName, callback) {
        $.post(localStorage.serviceURL + "/ReceiveStockTransfer", { stockTransferHdrRefNo: stockTransferHdrRefNo, userName: userName }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Receive Stock Transfer") });
    },
    
    fnReceiveAllTallyStockTransfer: function (stockTransferHdrRefNo, userName, callback) {
        $.post(localStorage.serviceURL + "/ReceiveAllTallyStockTransfer", { stockTransferHdrRefNo: stockTransferHdrRefNo, userName: userName }, function (loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Receive Stock Transfer") });
    },

    fnStockTransferApproval: function (data, StockTransferHdrRefNo, username, priceLevel,autoStockIn, callback) {
    $.post(localStorage.serviceURL + "/StockTransferApproval", { data: data, StockTransferHdrRefNo: StockTransferHdrRefNo, username: username, priceLevel: priceLevel, autoStockIn: autoStockIn }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Approve Stock Transfer") });
    },

    fnChangeSTDetailFactoryPrice: function(StockTransferDetRefNo, price, username, callback) {
        $.post(localStorage.serviceURL + "/ChangeSTDetailFactoryPrice", { StockTransferDetRefNo: StockTransferDetRefNo, price: price, username: username }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Change Stock Transfer Detail Factory Price") });
    },

    fnChangeSTDetailFactoryPriceAll: function(StockTransferHdrRefNo, levelPrice, username, callback) {
        $.post(localStorage.serviceURL + "/ChangeSTDetailFactoryPriceAll", { StockTransferHdrRefNo: StockTransferHdrRefNo, levelPrice: levelPrice, username: username }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function(err) { InventoryWebWS.fnFail(err, "Error Change Stock Transfer Detail Factory Price") });
    },


    fnChangeRemarkHeader: function(PurchaseOrderHeaderRefNo, Remarks, username, callback) {
        $.post(localStorage.serviceURL + "/ChangePORemarkHeader", { PurchaseOrderHeaderRefNo: PurchaseOrderHeaderRefNo, Remarks: Remarks, username: username }, function(loader) {
            var res = $.xml2json(loader).text;
            if (callback) callback(JSON.parse(res));
        }, "xml").fail(function(err) { InventoryWebWS.fnFail(err, "Error Change Remark Header") });
    },

    fnGetRecipeHeaderList: function (filter, callback) {
	    $.post(localStorage.serviceURL + "/GetRecipeHeaderList", { filter: filter }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Recipe Header List") });
	},

	fnSearchRecipeHeader: function (filter, skip, take, sortBy, isAscending, isHaveWrongConversion, callback) {
	    $.post(localStorage.serviceURL + "/SearchRecipeHeader", { filter: filter, skip: skip, take: take, sortBy: sortBy, isAscending: isAscending, isHaveWrongConversion: isHaveWrongConversion }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Search Recipe") });
	},

	fnSaveRecipeWithConvRate: function (data, username, callback) {
	    $.post(localStorage.serviceURL + "/SaveRecipeWithConvRate", { data: data, username: username }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Save Recipe") });
	},

	fnGetRecipeHeader: function (RecipeHeaderID, callback) {
	    $.post(localStorage.serviceURL + "/GetRecipeHeader", { recipeHeaderID: RecipeHeaderID }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Recipe Header") });
	},

	fnGetRecipeDetail: function (RecipeHeaderID, callback) {
	    $.post(localStorage.serviceURL + "/GetRecipeDetail", { recipeHeaderID: RecipeHeaderID }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Recipe Detail") });
	},

	fnDeleteRecipeHeader: function (RecipeHeaderID, username, callback) {
	    $.post(localStorage.serviceURL + "/DeleteRecipeHeader", { recipeHeaderID: RecipeHeaderID, username: username }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Delete Recipe Header") });
	},

	fnGetUOMListByItemNo: function (itemNo, callback) {
	    $.post(localStorage.serviceURL + "/GetUOMListByItemNo", { itemNo: itemNo }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get UOM List") });
	},

	fnSearchRecipeMainItem: function (name, showProductOnly, skip, take, callback) {
	    $.post(localStorage.serviceURL + "/SearchRecipeMainItem", { name: name, showProductOnly: showProductOnly, skip: skip, take: take }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Search Recipe Main Item") });
	},

	fnGetCostRateByItemNo: function (itemNo, callback) {
	    $.post(localStorage.serviceURL + "/GetCostRateByItemNo", { itemNo: itemNo }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Recipe Detail") });
	},

	fnGetConversionRate: function (itemNo, fromUOM, toUOM, callback) {
	    $.post(localStorage.serviceURL + "/GetConversionRate", { itemNo: itemNo, fromUOM: fromUOM, toUOM: toUOM }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Conversion Rate") });
	},

	fnGetUOMList: function (callback) {
	    $.post(localStorage.serviceURL + "/GetUOMList", {}, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get UOM List") });
	},

	fnGetUOMConversionList: function (filter, skip, take, sortBy, isAscending, callback) {
	    $.post(localStorage.serviceURL + "/GetUOMConversionList", { filter: filter, skip: skip, take: take, sortBy: sortBy, isAscending: isAscending }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get UOM Conversion List") });
	},

	fnSaveUOMConversion: function (data, username, callback) {
	    $.post(localStorage.serviceURL + "/SaveUOMConversion", { data: data, username: username }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Save UOM Conversion") });
	},

	fnDeleteUOMConversion: function (data, username, callback) {
	    $.post(localStorage.serviceURL + "/DeleteUOMConversion", { data: data, username: username }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Delete UOM Conversion") });
	},

	fnSearchCookItem: function (name, skip, take, callback) {
	    $.post(localStorage.serviceURL + "/SearchCookItem", { name: name, skip: skip, take: take }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Search Cook Item") });
	},

	fnFetchCookItem: function (filter, skip, take, sortBy, isAscending, callback) {
	    $.post(localStorage.serviceURL + "/FetchCookItem", { filter: filter, skip: skip, take: take, sortBy: sortBy, isAscending: isAscending, }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Fetch Cook Item") });
	},

	fnCookItem: function (itemNo, quantity, pointOfSaleID, inventoryLocationID, remarks, userName, callback) {
	    $.post(localStorage.serviceURL + "/CookItem", { itemNo: itemNo, quantity: quantity, pointOfSaleID: pointOfSaleID, inventoryLocationID: inventoryLocationID, remarks: remarks, userName: userName }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Cook Item") });
	},
	
    fnSaveItemCookHistory: function (data, username, callback) {
	    $.post(localStorage.serviceURL + "/SaveItemCookHistory", { data: data, username: username }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Create Item Cook History") });
	},
	
	fnGetItemCookHistory: function (DocumentNo, callback) {
	    $.post(localStorage.serviceURL + "/GetItemCookHistory", { DocumentNo: DocumentNo }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Get Item Cook History") });
	},
	
	fnDeleteItemCookDetail: function (data, username, callback) {
	    $.post(localStorage.serviceURL + "/DeleteItemCookDetail", { data: data, username: username }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Delete Item Cook Detail") });
	},
	
		
	fnAddItemCookDetail: function (data, username, callback) {
	    $.post(localStorage.serviceURL + "/AddItemCookDetail", { data: data, username: username }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Add Item Cook Detail") });
	},
	
	fnChangeQtyItemCookHistory: function (itemCookHistoryID, qty, username, callback) {
	    $.post(localStorage.serviceURL + "/ChangeQtyItemCookHistory", { itemCookHistoryID: itemCookHistoryID, qty: qty, username: username }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Change Qty Item Cook Header") });
	},
	
	fnChangeQtyItemCookDetail: function (itemCookDetailID, qty, username, callback) {
	    $.post(localStorage.serviceURL + "/ChangeQtyItemCookDetail", { itemCookDetailID: itemCookDetailID, qty: qty, username: username }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Change Qty Item Cook Detail") });
	},
	
	fnCookItemWithDetail: function (itemCookHistoryID, pointOfSaleID, username, callback) {
	    $.post(localStorage.serviceURL + "/CookItemWithDetail", { itemCookHistoryID: itemCookHistoryID, pointOfSaleID: pointOfSaleID, username: username }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Cook Item") });
	},
	
	fnCancelItemCookHistory: function (itemCookHistoryID, username, callback) {
	    $.post(localStorage.serviceURL + "/CancelItemCookHistory", { ItemCookHistoryID: itemCookHistoryID, username: username }, function (loader) {
	        var res = $.xml2json(loader).text;
	        if (callback) callback(JSON.parse(res));
	    }, "xml").fail(function (err) { InventoryWebWS.fnFail(err, "Error Cancel Cook Item") });
	}
};

