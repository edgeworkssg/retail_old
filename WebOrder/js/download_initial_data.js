function fnInitializeEquipPDA(callback) {
    try {
        var num_of_records = 0;
        DAL.fnOpenDB(function () {
            DAL.fnCreateTablesIfNotExist();
        });
        Logger.fnWriteLog("Begin download database");

        //InventoryWebWS.fnGetDataTable("Supplier", true, function (data) {
        //    if (data != null) {
        //        for (var i = 0; i < data.length; i++) {
        //            DAL.fnSaveSupplier(data[i]);
        //            num_of_records += 1;
        //        }
        //    };

        InventoryWebWS.fnGetDataTable("InventoryLocation", true, function(data) {
            if (data != null) {
                for (var i = 0; i < data.length; i++) {
                    DAL.fnSaveInventoryLocation(data[i]);
                    num_of_records += 1;
                }
            };

            //InventoryWebWS.fnGetDataTable("Currencies", true, function (data) {
            //    if (data != null) {
            //        for (var i = 0; i < data.length; i++) {
            //            DAL.fnSaveCurrencies(data[i]);
            //            num_of_records += 1;
            //        }
            //    };

            //InventoryWebWS.fnGetDataTable("Item", true, function (data) {
            //    if (data != null) {
            //        for (var i = 0; i < data.length; i++) {
            //            DAL.fnSaveItem(data[i]);
            //            num_of_records += 1;
            //        }
            //    };

            //InventoryWebWS.fnGetDataTable("Category", true, function (data) {
            //    if (data != null) {
            //        for (var i = 0; i < data.length; i++) {
            //            DAL.fnSaveCategory(data[i]);
            //            num_of_records += 1;
            //        }
            //    };

            //InventoryWebWS.fnGetDataTable("ItemDepartment", true, function (data) {
            //    if (data != null) {
            //        for (var i = 0; i < data.length; i++) {
            //            DAL.fnSaveItemDepartment(data[i]);
            //            num_of_records += 1;
            //        }
            //    };

            InventoryWebWS.fnGetDataTable("InventoryStockOutReason", true, function(data) {
                //debugger;
                if (data != null) {
                    for (var i = 0; i < data.length; i++) {
                        DAL.fnSaveInventoryStockOutReason(data[i]);
                        num_of_records += 1;
                    }
                };

                Logger.fnWriteLog("Finish download database");
                if (callback) callback(num_of_records);
            });
            //});
            //});
            //});
            //});
        });
        //});
    }
    catch (ex) {
        Logger.writeLog(ex.toString());
    }
}

