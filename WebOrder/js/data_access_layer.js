/// <reference path="InventoryWeb_web_service.js" />

//DAL - DATA ACCESS LAYER CLASS
//Handle all that relates to accessing WebSQL
DAL = {
    DB: null,
    //Drop All Tables in DB
    fnDropAllTables: function(callback) {
        try {
            if (DAL.DB == null) {
                if (callback) {
                    callback(false);
                    return false;
                }
                else {
                    return null;
                }
            }
            DAL.DB.transaction(function(tx) {
                //tx.executeSql('DROP TABLE Supplier');
                tx.executeSql('DROP TABLE InventoryLocation');
                //tx.executeSql('DROP TABLE Currencies');
                //tx.executeSql('DROP TABLE Item');
                //tx.executeSql('DROP TABLE Category');
                //tx.executeSql('DROP TABLE ItemDepartment');
                tx.executeSql('DROP TABLE InventoryStockOutReason');

                tx.executeSql('DROP TABLE PowerLog ');
            });

            DAL.fnCreateTablesIfNotExist(callback);
        }
        catch (e) {
            System.alert(e);
            //Log e to DB
            if (callback) {
                callback(false);
                return false;
            }
            else {
                return null;
            }
        }
    },

    //Create DB tables in the DB if it does not exist.
    fnCreateTablesIfNotExist: function(callback) {
        try {
            if (DAL.DB == null) {
                if (callback) {
                    callback(false);
                    return false;
                }
                else {
                    return null;
                }
            }
            DAL.DB.transaction(function(tx) {
                tx.executeSql('CREATE TABLE IF NOT EXISTS PowerLog (logid INTEGER PRIMARY KEY AUTOINCREMENT, msg, actiondate)');
                //tx.executeSql('CREATE TABLE IF NOT EXISTS Supplier (SupplierID, SupplierName, CustomerAddress, ShipToAddress, BillToAddress, ContactNo1, ContactNo2, ContactNo3, ContactPerson1, ContactPerson2, ContactPerson3, AccountNo, userfld1, userfld2, userfld3, userfld4, userfld5, userfld6, userfld7, userfld8, userfld9, userfld10, userflag1, userflag2, userflag3, userflag4, userflag5, userfloat1, userfloat2, userfloat3, userfloat4, userfloat5, userint1, userint2, userint3, userint4, userint5)');
                tx.executeSql('CREATE TABLE IF NOT EXISTS InventoryLocation (InventoryLocationID, InventoryLocationName, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, Deleted, userfld1, userfld2, userfld3, userfld4, userfld5, userfld6, userfld7, userfld8, userfld9, userfld10, userflag1, userflag2, userflag3, userflag4, userflag5, userfloat1, userfloat2, userfloat3, userfloat4, userfloat5, userint1, userint2, userint3, userint4, userint5)');
                //tx.executeSql('CREATE TABLE IF NOT EXISTS Currencies (CurrencyId, CurrencyCode, Description, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, Deleted)');
                //tx.executeSql('CREATE TABLE IF NOT EXISTS Item (ItemNo, ItemName, Barcode, CategoryName, RetailPrice, FactoryPrice, MinimumPrice, ItemDesc, IsServiceItem, IsInInventory, IsNonDiscountable, IsCourse, CourseTypeID, Brand, ProductLine, Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8, Remark, ProductionDate, IsGST, hasWarranty, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, UniqueID, Deleted, userfld1, userfld2, userfld3, userfld4, userfld5, userfld6, userfld7, userfld8, userfld9, userfld10, userflag1, userflag2, userflag3, userflag4, userflag5, userfloat1, userfloat2, userfloat3, userfloat4, userfloat5, userint1, userint2, userint3, userint4, userint5, IsDelivery, GSTRule, IsVitaMix, IsWaterFilter, IsYoung, IsJuicePlus, IsCommission, UOM, BaseLevel)');
                //tx.executeSql('CREATE TABLE IF NOT EXISTS Category (CategoryName, Remarks, Category_ID, IsDiscountable, IsForSale, IsGST, AccountCategory, ItemDepartmentId, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Deleted, userfld1, userfld2, userfld3, userfld4, userfld5, userfld6, userfld7, userfld8, userfld9, userfld10, userflag1, userflag2, userflag3, userflag4, userflag5, userfloat1, userfloat2, userfloat3, userfloat4, userfloat5, userint1, userint2, userint3, userint4, userint5)');
                //tx.executeSql('CREATE TABLE IF NOT EXISTS ItemDepartment (ItemDepartmentID, DepartmentName, Remark, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, Deleted)');
                tx.executeSql('CREATE TABLE IF NOT EXISTS InventoryStockOutReason (ReasonID, ReasonName, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Deleted, userfld1, userfld2, userfld3, userfld4, userfld5, userfld6, userfld7, userfld8, userfld9, userfld10, userflag1, userflag2, userflag3, userflag4, userflag5, userfloat1, userfloat2, userfloat3, userfloat4, userfloat5, userint1, userint2, userint3, userint4, userint5)');

                tx.executeSql('CREATE TABLE IF NOT EXISTS Settings (PropName,propValue)');
            });
            if (callback) {
                callback(true);
                return true;
            }
            else {
                return true;
            }
        }
        catch (e) {
            System.alert(e);
            //Log e to DB
            if (callback) {
                callback(false);
                return false;
            }
            else {
                return null;
            }
        }
    },

    //Open WebSQL and assign DB variable with the SQL DB Object
    fnOpenDB: function(callback) {
        if (DAL.DB == null) {
            try {
                if (window.openDatabase) {
                    DAL.DB = openDatabase('InventoryWeb', '1.0', 'Inventory Web DB', 5 * 1024 * 1024);
                    if (callback) callback();
                }
                else {
                    //User is using browser that doesnt support WebSQL
                    System.alert('Database is not supported. Please change your browser to browser that support WebSQL.');
                }
            }
            catch (e) {
                //error catched
                //log the error to DB?
                System.alert('Error in creating/opening Database.' + e);
            }
        }
    },

    fnLoadAllSetting: function(callback) {
        DAL.fnOpenDB();
        DAL.DB.transaction(function(tx) {
            tx.executeSql("SELECT * FROM Settings", [], function(tx, results) {
                if (results.rows.length > 0) {
                    if (callback) callback(results);
                }
                else {
                    if (callback) callback(null);
                }
            });
        });
    },

    fnLoadSetting: function(name, callback) {
        DAL.fnOpenDB();
        DAL.DB.transaction(function(tx) {
            tx.executeSql("SELECT propValue FROM Settings WHERE propName=?", [name], function(tx, results) {
                if (results.rows.length > 0) {
                    if (callback) callback(results.rows.item(0).propValue);
                }
                else {
                    if (callback) callback(null);
                }
            });
        });
    },

    fnSaveSetting: function(name, value, callback) {
        DAL.fnOpenDB();
        DAL.DB.transaction(function(tx) {
            tx.executeSql('CREATE TABLE IF NOT EXISTS Settings (PropName,propValue)');
            tx.executeSql("SELECT propValue FROM Settings WHERE propName=?", [name], function(tx, results) {
                if (results.rows.length > 0) {
                    tx.executeSql('UPDATE Settings SET propValue=? WHERE propName=?', [value, name], function(tx, result) {
                        if (callback) callback(true);
                    });
                }
                else {
                    tx.executeSql('INSERT INTO Settings (propValue,propName) VALUES (?,?)', [value, name], function(tx, result) {
                        if (callback) callback(true);
                    });
                }
            });
        });
    },

    ////Get List of all Supplier 
    //fnGetSuppliersList: function (callback) {
    //    DAL.fnOpenDB();
    //    DAL.DB.transaction(function (tx) {
    //        tx.executeSql('SELECT * FROM Supplier', [], function (tx, results) {
    //            var res = new Array();
    //            if (results.rows.length > 0) {
    //                for (var i = 0; i < results.rows.length; i++) {
    //                    res.push(results.rows.item(i));
    //                };
    //            };
    //            if (callback) callback(res);
    //        }, function (tx, err) {
    //            alert('error');
    //        });
    //    });
    //},

    //fnSaveSupplier: function (d) {
    //    try {
    //        DAL.fnOpenDB();
    //        DAL.DB.transaction(function (tx) {
    //            tx.executeSql('DELETE FROM Supplier WHERE SupplierID = ?', [d.SupplierID]);
    //            var sql = 'INSERT INTO Supplier (SupplierID, SupplierName, CustomerAddress, ShipToAddress, BillToAddress, ContactNo1, ContactNo2, ContactNo3, ContactPerson1, ContactPerson2, ContactPerson3, AccountNo, userfld1, userfld2, userfld3, userfld4, userfld5, userfld6, userfld7, userfld8, userfld9, userfld10, userflag1, userflag2, userflag3, userflag4, userflag5, userfloat1, userfloat2, userfloat3, userfloat4, userfloat5, userint1, userint2, userint3, userint4, userint5) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)';
    //            tx.executeSql(sql, [d.SupplierID, d.SupplierName, d.CustomerAddress, d.ShipToAddress, d.BillToAddress, d.ContactNo1, d.ContactNo2, d.ContactNo3, d.ContactPerson1, d.ContactPerson2, d.ContactPerson3, d.AccountNo, d.userfld1, d.userfld2, d.userfld3, d.userfld4, d.userfld5, d.userfld6, d.userfld7, d.userfld8, d.userfld9, d.userfld10, d.userflag1, d.userflag2, d.userflag3, d.userflag4, d.userflag5, d.userfloat1, d.userfloat2, d.userfloat3, d.userfloat4, d.userfloat5, d.userint1, d.userint2, d.userint3, d.userint4, d.userint5], function (mx) {
    //                return true;
    //            });
    //        });
    //    } catch (ex) {
    //        Logger.fnWriteLog(ex.message);
    //    }
    //},

    //Get List of all InventoryLocation 
    fnGetInventoryLocationList: function(callback) {
//                dal.fnopendb();
//                dal.db.transaction(function (tx) {
//                    tx.executesql('select * from inventorylocation order by inventorylocationname', [], function (tx, results) {
//                        var res = new array();
//                        if (results.rows.length > 0) {
//                            for (var i = 0; i < results.rows.length; i++) {
//                                res.push(results.rows.item(i));
//                            };
//                        };
//                        if (callback) callback(res);
//                    }, function (tx, err) {
//                        alert('error');
//                    });
//                });
        var res = new Array();
        if(sessionStorage.InventoryLocationCollection != null)
            res = JSON.parse(sessionStorage.InventoryLocationCollection);
        if (callback) callback(res);
    },

    //Get InventoryLocation by InventoryLocationID
    fnGetInventoryLocation: function(InventoryLocationID, callback) {
        DAL.fnOpenDB();
        DAL.DB.transaction(function(tx) {
            tx.executeSql('SELECT * FROM InventoryLocation WHERE InventoryLocationID = ?', [InventoryLocationID.toString()], function(tx, results) {
                if (results.rows.length > 0) {
                    if (callback) callback(results.rows.item(0));
                }
                else {
                    if (callback) callback(null);
                };
            }, function(tx, err) {
                alert('error');
            });
        });
    },

    fnSaveInventoryLocation: function(d) {
        try {
            DAL.fnOpenDB();
            DAL.DB.transaction(function(tx) {
                tx.executeSql('DELETE FROM InventoryLocation WHERE InventoryLocationID = ?', [d.InventoryLocationID]);
                var sql = 'INSERT INTO InventoryLocation (InventoryLocationID, InventoryLocationName, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, Deleted, userfld1, userfld2, userfld3, userfld4, userfld5, userfld6, userfld7, userfld8, userfld9, userfld10, userflag1, userflag2, userflag3, userflag4, userflag5, userfloat1, userfloat2, userfloat3, userfloat4, userfloat5, userint1, userint2, userint3, userint4, userint5) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)';
                tx.executeSql(sql, [d.InventoryLocationID, d.InventoryLocationName, d.CreatedOn, d.CreatedBy, d.ModifiedOn, d.ModifiedBy, d.Deleted, d.userfld1, d.userfld2, d.userfld3, d.userfld4, d.userfld5, d.userfld6, d.userfld7, d.userfld8, d.userfld9, d.userfld10, d.userflag1, d.userflag2, d.userflag3, d.userflag4, d.userflag5, d.userfloat1, d.userfloat2, d.userfloat3, d.userfloat4, d.userfloat5, d.userint1, d.userint2, d.userint3, d.userint4, d.userint5], function(mx) {
                    return true;
                });
            });
        } catch (ex) {
            Logger.fnWriteLog(ex.message);
        }
    },

    ////Get List of all Currencies 
    //fnGetCurrenciesList: function (callback) {
    //    DAL.fnOpenDB();
    //    DAL.DB.transaction(function (tx) {
    //        tx.executeSql('SELECT * FROM Currencies', [], function (tx, results) {
    //            var res = new Array();
    //            if (results.rows.length > 0) {
    //                for (var i = 0; i < results.rows.length; i++) {
    //                    res.push(results.rows.item(i));
    //                };
    //            };
    //            if (callback) callback(res);
    //        }, function (tx, err) {
    //            alert('error');
    //        });
    //    });
    //},

    //fnSaveCurrencies: function (d) {
    //    try {
    //        DAL.fnOpenDB();
    //        DAL.DB.transaction(function (tx) {
    //            tx.executeSql('DELETE FROM Currencies WHERE CurrencyId = ?', [d.CurrencyId]);
    //            var sql = 'INSERT INTO Currencies (CurrencyId, CurrencyCode, Description, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, Deleted) VALUES (?,?,?,?,?,?,?,?)';
    //            tx.executeSql(sql, [d.CurrencyId, d.CurrencyCode, d.Description, d.CreatedOn, d.CreatedBy, d.ModifiedOn, d.ModifiedBy, d.Deleted], function (mx) {
    //                return true;
    //            });
    //        });
    //    } catch (ex) {
    //        Logger.fnWriteLog(ex.message);
    //    }
    //},

    //fnSaveItem: function (d) {
    //    try {
    //        DAL.fnOpenDB();
    //        DAL.DB.transaction(function (tx) {
    //            tx.executeSql('DELETE FROM Item WHERE ItemNo = ?', [d.ItemNo]);
    //            var sql = 'INSERT INTO Item (ItemNo, ItemName, Barcode, CategoryName, RetailPrice, FactoryPrice, MinimumPrice, ItemDesc, IsServiceItem, IsInInventory, IsNonDiscountable, IsCourse, CourseTypeID, Brand, ProductLine, Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8, Remark, ProductionDate, IsGST, hasWarranty, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, UniqueID, Deleted, userfld1, userfld2, userfld3, userfld4, userfld5, userfld6, userfld7, userfld8, userfld9, userfld10, userflag1, userflag2, userflag3, userflag4, userflag5, userfloat1, userfloat2, userfloat3, userfloat4, userfloat5, userint1, userint2, userint3, userint4, userint5, IsDelivery, GSTRule, IsVitaMix, IsWaterFilter, IsYoung, IsJuicePlus, IsCommission, UOM, BaseLevel) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)';
    //            tx.executeSql(sql, [d.ItemNo, d.ItemName, d.Barcode, d.CategoryName, d.RetailPrice, d.FactoryPrice, d.MinimumPrice, d.ItemDesc, d.IsServiceItem, d.IsInInventory, d.IsNonDiscountable, d.IsCourse, d.CourseTypeID, d.Brand, d.ProductLine, d.Attributes1, d.Attributes2, d.Attributes3, d.Attributes4, d.Attributes5, d.Attributes6, d.Attributes7, d.Attributes8, d.Remark, d.ProductionDate, d.IsGST, d.hasWarranty, d.CreatedOn, d.CreatedBy, d.ModifiedOn, d.ModifiedBy, d.UniqueID, d.Deleted, d.userfld1, d.userfld2, d.userfld3, d.userfld4, d.userfld5, d.userfld6, d.userfld7, d.userfld8, d.userfld9, d.userfld10, d.userflag1, d.userflag2, d.userflag3, d.userflag4, d.userflag5, d.userfloat1, d.userfloat2, d.userfloat3, d.userfloat4, d.userfloat5, d.userint1, d.userint2, d.userint3, d.userint4, d.userint5, d.IsDelivery, d.GSTRule, d.IsVitaMix, d.IsWaterFilter, d.IsYoung, d.IsJuicePlus, d.IsCommission, d.userfld1, d.userint1], function (mx) {
    //                return true;
    //            });
    //        });
    //    } catch (ex) {
    //        Logger.fnWriteLog(ex.message);
    //    }
    //},

    ////Get Item by Item No
    //fnGetItemByItemNo: function (itemNo, callback) {
    //    DAL.fnOpenDB();
    //    DAL.DB.transaction(function (tx) {
    //        tx.executeSql('SELECT * FROM Item WHERE ItemNo = ?', [itemNo], function (tx, results) {
    //            if (results.rows.length > 0) {
    //                if (callback) callback(results.rows.item(0));
    //            }
    //            else {
    //                if (callback) callback(null);
    //            };
    //        }, function (tx, err) {
    //            alert('error');
    //        });
    //    });
    //},

    //fnSaveCategory: function (d) {
    //    try {
    //        DAL.fnOpenDB();
    //        DAL.DB.transaction(function (tx) {
    //            tx.executeSql('DELETE FROM Category WHERE CategoryName = ?', [d.CategoryName]);
    //            var sql = 'INSERT INTO Category (CategoryName, Remarks, Category_ID, IsDiscountable, IsForSale, IsGST, AccountCategory, ItemDepartmentId, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Deleted, userfld1, userfld2, userfld3, userfld4, userfld5, userfld6, userfld7, userfld8, userfld9, userfld10, userflag1, userflag2, userflag3, userflag4, userflag5, userfloat1, userfloat2, userfloat3, userfloat4, userfloat5, userint1, userint2, userint3, userint4, userint5) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)';
    //            tx.executeSql(sql, [d.CategoryName, d.Remarks, d.Category_ID, d.IsDiscountable, d.IsForSale, d.IsGST, d.AccountCategory, d.ItemDepartmentId, d.CreatedBy, d.CreatedOn, d.ModifiedBy, d.ModifiedOn, d.Deleted, d.userfld1, d.userfld2, d.userfld3, d.userfld4, d.userfld5, d.userfld6, d.userfld7, d.userfld8, d.userfld9, d.userfld10, d.userflag1, d.userflag2, d.userflag3, d.userflag4, d.userflag5, d.userfloat1, d.userfloat2, d.userfloat3, d.userfloat4, d.userfloat5, d.userint1, d.userint2, d.userint3, d.userint4, d.userint5], function (mx) {
    //                return true;
    //            });
    //        });
    //    } catch (ex) {
    //        Logger.fnWriteLog(ex.message);
    //    }
    //},

    //fnSaveItemDepartment: function (d) {
    //    try {
    //        DAL.fnOpenDB();
    //        DAL.DB.transaction(function (tx) {
    //            tx.executeSql('DELETE FROM ItemDepartment WHERE ItemDepartmentID = ?', [d.ItemDepartmentID]);
    //            var sql = 'INSERT INTO ItemDepartment (ItemDepartmentID, DepartmentName, Remark, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, Deleted) VALUES (?,?,?,?,?,?,?,?)';
    //            tx.executeSql(sql, [d.ItemDepartmentID, d.DepartmentName, d.Remark, d.CreatedOn, d.ModifiedOn, d.CreatedBy, d.ModifiedBy, d.Deleted], function (mx) {
    //                return true;
    //            });
    //        });
    //    } catch (ex) {
    //        Logger.fnWriteLog(ex.message);
    //    }
    //},

    fnSaveInventoryStockOutReason: function(d) {
        try {
            DAL.fnOpenDB();
            DAL.DB.transaction(function(tx) {
                tx.executeSql('DELETE FROM InventoryStockOutReason WHERE ReasonID = ?', [d.ReasonID]);
                var sql = 'INSERT INTO InventoryStockOutReason (ReasonID, ReasonName, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Deleted, userfld1, userfld2, userfld3, userfld4, userfld5, userfld6, userfld7, userfld8, userfld9, userfld10, userflag1, userflag2, userflag3, userflag4, userflag5, userfloat1, userfloat2, userfloat3, userfloat4, userfloat5, userint1, userint2, userint3, userint4, userint5) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)';
                tx.executeSql(sql, [d.ReasonID, d.ReasonName, d.CreatedBy, d.CreatedOn, d.ModifiedBy, d.ModifiedOn, d.Deleted, d.userfld1, d.userfld2, d.userfld3, d.userfld4, d.userfld5, d.userfld6, d.userfld7, d.userfld8, d.userfld9, d.userfld10, d.userflag1, d.userflag2, d.userflag3, d.userflag4, d.userflag5, d.userfloat1, d.userfloat2, d.userfloat3, d.userfloat4, d.userfloat5, d.userint1, d.userint2, d.userint3, d.userint4, d.userint5], function(mx) {
                    return true;
                });
            });
        } catch (ex) {
            Logger.fnWriteLog(ex.message);
        }
    },

    //Get List of all InventoryStockOutReason 
    fnGetInventoryStockOutReasonList: function(type, callback) {
        DAL.fnOpenDB();
        DAL.DB.transaction(function (tx) {
            tx.executeSql('SELECT * FROM InventoryStockOutReason WHERE userfld1 = ? AND Deleted = "false"', [type], function (tx, results) {
                var res = new Array();
                if (results.rows.length > 0) {
                    for (var i = 0; i < results.rows.length; i++) {
                        res.push(results.rows.item(i));
                    };
                };
                if (callback) callback(res);
            }, function(tx, err) {
                alert('error');
            });
        });
    },


    // ============================================================
    // BELOW THIS POINT IS USING WEB SERVICE TO DO CRUD OPERATIONS,
    // DATABASE IS LOCATED ON SERVER
    // ============================================================


    //Save PurchaseOrderHeader
    fnSavePurchaseOrderHeader: function(d, callback) {
        InventoryWebWS.fnSavePurchaseOrderHeader(JSON.stringify(d), sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    //Save PurchaseOrderHeader With Item Calculated
    fnSavePurchaseOrderHeaderCreateItems: function(d, callback) {
        InventoryWebWS.fnGetSetting("ShowAllItemInGoodsOrdering", "yes", function(res) {
            //alert(res);
            InventoryWebWS.fnSavePurchaseOrderHeaderCreateItems(JSON.stringify(d), sessionStorage.username, res, function(result) {
                if (callback) callback(result);
            });
        });
    },

    //Get List of all PurchaseOrderHeader 
    fnGetPurchaseOrderHeaderList: function(filter, skip, take, sortBy, isAscending, callback) {
        InventoryWebWS.fnGetPurchaseOrderHeaderList(filter, skip, take, sortBy, isAscending, function(data) {
            if (callback) callback(data);
        });
    },

    //Get List of all PurchaseOrderHeader 
    fnGetPurchaseOrderHeaderListWithOutletName: function(filter, skip, take, sortBy, isAscending, callback) {
        InventoryWebWS.fnGetPurchaseOrderHeaderListWithOutletName(filter, skip, take, sortBy, isAscending, function(data) {
            if (callback) callback(data);
        });
    },

    //Get PurchaseOrderHeader by PurchaseOrderHeaderRefNo
    fnGetPurchaseOrderHeader: function(PurchaseOrderHeaderRefNo, callback) {
        InventoryWebWS.fnGetPurchaseOrderHeader(PurchaseOrderHeaderRefNo, function(result) {
            if (callback) callback(result);
        });
    },

    //Save PurchaseOrderDetail
    fnSavePurchaseOrderDetail: function(d, callback) {
        InventoryWebWS.fnSavePurchaseOrderDetail(JSON.stringify(d), sessionStorage.username, function(data) {
            if (callback) callback(data);
        });
    },

    //Save PurchaseOrderDetail
    fnSavePurchaseOrderDetailXYSupplierPortal: function (d, numOfDays, isShowSalesQty, callback) {
        InventoryWebWS.fnSavePurchaseOrderDetailXYSupplierPortal(JSON.stringify(d), sessionStorage.username, numOfDays, isShowSalesQty, function (data) {
            if (callback) callback(data);
        });
    },

    //Get List of PurchaseOrderDetail in a PurchaseOrderHeader
    fnGetPurchaseOrderDetailList: function(PurchaseOrderHeaderRefNo, callback) {
        InventoryWebWS.fnGetPurchaseOrderDetailList(PurchaseOrderHeaderRefNo, function (data) {
            if (callback) callback(data);
        });
    },

    //Get List of PurchaseOrderDetail in a PurchaseOrderHeader
    fnGetPurchaseOrderDetailListSupplierPortal: function (PurchaseOrderHeaderRefNo, numOfDays, isShowSalesQty, callback) {
        InventoryWebWS.fnGetPurchaseOrderDetailListSupplierPortal(PurchaseOrderHeaderRefNo, sessionStorage.username, numOfDays, isShowSalesQty, function (data) {
            if (callback) callback(data);
        });
    },

    //Delete PurchaseOrder Header & Detail
    fnDeletePurchaseOrder: function(PurchaseOrderHeaderRefNo, callback) {
        InventoryWebWS.fnDeletePurchaseOrder(PurchaseOrderHeaderRefNo, function(result) {
            if (callback) callback(result);
        });
    },

    //Delete PurchaseOrder Detail
    fnDeletePurchaseOrderDetail: function(d, callback) {
        InventoryWebWS.fnDeletePurchaseOrderDetail(JSON.stringify(d), function(result) {
            if (callback) callback(result, d);
        });
    },

    //Delete PurchaseOrder Detail
    fnUpdateSalesPersonPurchaseOrder: function(PurchaseOrderHeaderRefNo, username, callback) {
        InventoryWebWS.fnUpdateSalesPersonPurchaseOrder(PurchaseOrderHeaderRefNo, username, function(result) {
            if (callback) callback(result);
        });
    },

    //Change Ordered Quantity in PurchaseOrderDetail
    fnChangePODetailQty: function(d, callback) {
        InventoryWebWS.fnChangePODetailQty(JSON.stringify(d), function(result) {
            if (callback) callback(result);
        });
    },

    //Change Factory Price in PurchaseOrderDetail
    fnChangePODetailFactoryPrice: function (PurchaseOrderDetailRefNo, price, callback) {
        InventoryWebWS.fnChangePODetailFactoryPrice(PurchaseOrderDetailRefNo, price, sessionStorage.username, function (result) {
            if (callback) callback(result);
        });
    },

    //Change Factory Price in PurchaseOrderDetail
    fnChangePODetailFactoryPriceAll: function(PurchaseOrderHeaderRefNo, levelPrice, callback) {
        InventoryWebWS.fnChangePODetailFactoryPriceAll(PurchaseOrderHeaderRefNo, levelPrice, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    //Change Approved Quantity in PurchaseOrderDetail
    fnChangePOQtyApproved: function(d, callback) {
        InventoryWebWS.fnChangePOQtyApproved(JSON.stringify(d), function(result) {
            if (callback) callback(result);
        });
    },

    //Change Approved Quantity in approved PurchaseOrderDetail
    fnChangeApprovedPOApprovedQty: function(d, callback) {
        InventoryWebWS.fnChangeApprovedPOApprovedQty(JSON.stringify(d), function(result) {
            if (callback) callback(result);
        });
    },

    //Change Credit Invoice No in PurchaseOrderHeader
    fnChangeCreditInvoiceNo: function (PurchaseOrderHeaderRefNo, CreditInvoiceNo, callback) {
        InventoryWebWS.fnChangeCreditInvoiceNo(PurchaseOrderHeaderRefNo, CreditInvoiceNo, sessionStorage.username, function (result) {
            if (callback) callback(result);
        });
    },

    //Change Credit Invoice No in Stock Transfer
    fnChangeCreditInvoiceNoST: function(StockTransferHdrRefNo, CreditInvoiceNo, callback) {
        InventoryWebWS.fnChangeCreditInvoiceNoST(StockTransferHdrRefNo, CreditInvoiceNo, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    //Change Invoice No in Stock Transfer
    fnChangeInvoiceNoST: function(StockTransferHdrRefNo, InvoiceNo, callback) {
        InventoryWebWS.fnChangeInvoiceNoST(StockTransferHdrRefNo, InvoiceNo, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    //Create Back Order Status
    fnCreateBackOrder: function(d, PurchaseOrderHeaderRefNo, username, callback) {

        InventoryWebWS.fnCreateBackOrder(JSON.stringify(d), PurchaseOrderHeaderRefNo, username, function(res) {
            if (callback) callback(res);
        });
    },

    //Update Purchase Order Approval Status
    fnPurchaseOrderApproval: function (d, PurchaseOrderHeaderRefNo, username, autoStockIn, priceLevel, callback) {
	    InventoryWebWS.fnPurchaseOrderApproval(JSON.stringify(d), PurchaseOrderHeaderRefNo, username, autoStockIn, priceLevel, function (result) {
            if (callback) callback(result);
        });
    },

    fnPurchaseOrderApprovalAutoApprove: function(PurchaseOrderHeaderRefNo, username, autoStockIn, callback) {
        InventoryWebWS.fnPurchaseOrderApprovalAutoApprove(PurchaseOrderHeaderRefNo, username, autoStockIn, function(result) {
            if (callback) callback(result);
        });
    },

    //Get List of PurchaseOrderDetail to receive in Goods Receive
    fnGetItemsToReceive: function(PurchaseOrderHeaderRefNo, InventoryLocationID, callback) {
        InventoryWebWS.fnGetItemsToReceive(PurchaseOrderHeaderRefNo, InventoryLocationID, function(data) {
            if (callback) callback(data);
        });
    },

    // Do Stock In operation
    fnStockIn: function(PurchaseOrderHeaderRefNo, data, username, StockInReasonID, InventoryLocationID, IsAdjustment, CalculateCOGS, Remark, callback) {
        InventoryWebWS.fnStockIn(PurchaseOrderHeaderRefNo, JSON.stringify(data), username, StockInReasonID, InventoryLocationID, IsAdjustment, CalculateCOGS, Remark, function(result) {
            if (callback) callback(result);
        });
    },

    // Do Stock Out operation
    fnStockOut: function(PurchaseOrderHeaderRefNo, data, username, StockOutReasonID, InventoryLocationID, IsAdjustment, deductRemainingQty, Remark, callback) {
        InventoryWebWS.fnStockOut(PurchaseOrderHeaderRefNo, JSON.stringify(data), username, StockOutReasonID, InventoryLocationID, IsAdjustment, deductRemainingQty, Remark, function(result) {
            if (callback) callback(result);
        });
    },

    // Do Stock Return operation
    fnStockReturn: function (PurchaseOrderHeaderRefNo, data, username, StockOutReasonID, InventoryLocationID, IsAdjustment, calculateCOGS, Remark, callback) {
        InventoryWebWS.fnStockReturn(PurchaseOrderHeaderRefNo, JSON.stringify(data), username, StockOutReasonID, InventoryLocationID, IsAdjustment, calculateCOGS, Remark, function (result) {
            if (callback) callback(result);
        });
    },

    // Do Stock Transfer operation
    fnStockTransfer: function(PurchaseOrderHeaderRefNo, data, username, FromInventoryLocationID, ToInventoryLocationID, callback) {
        InventoryWebWS.fnStockTransfer(PurchaseOrderHeaderRefNo, JSON.stringify(data), username, FromInventoryLocationID, ToInventoryLocationID, function(result) {
            if (callback) callback(result);
        });
    },

    // Search Item
    fnSearchItem: function (name, IsInventoryItemOnly, ShowSystemItem, ShowDeletedItem, skip, take, callback) {
        InventoryWebWS.fnSearchItem(name.replace(/\*/g, '%'), IsInventoryItemOnly, ShowSystemItem, ShowDeletedItem, skip, take, function (data) {
            if (callback) callback(data);
        });
    },

    // Search Item
    fnSearchItemUserPortal: function (name, username, isSupplier, isRestrictedSupplier, supplierID, isShowSalesQty, numOfDays, InventoryLocationID, IsInventoryItemOnly, ShowSystemItem, ShowDeletedItem, skip, take, callback) {
        InventoryWebWS.fnSearchItemUserPortal(name.replace(/\*/g, '%'), username, isSupplier, isRestrictedSupplier, supplierID, isShowSalesQty, numOfDays, InventoryLocationID, IsInventoryItemOnly, ShowSystemItem, ShowDeletedItem, skip, take, function (data) {
            if (callback) callback(data);
        });
    },

    // Search User
    fnSearchUser: function(name, ShowDeletedUser, skip, take, callback) {
        InventoryWebWS.fnSearchUser(name.replace(/\*/g, '%'), ShowDeletedUser, skip, take, function(data) {
            if (callback) callback(data);
        });
    },

    // Get Item
    fnGetItem: function(ItemNo, callback) {
        InventoryWebWS.fnGetItem(ItemNo, function(result) {
            if (callback) callback(result);
        });
    },

    // Get Item No
    fnGetItemNo: function (barcode, callback) {
        InventoryWebWS.fnGetItemNo(barcode, function (result) {
            if (callback) callback(result);
        });
    },

    //Save StockTakeDoc
    fnSaveStockTakeDoc: function(d, callback) {
        InventoryWebWS.fnSaveStockTakeDoc(JSON.stringify(d), sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    //Get List of all StockTakeDoc 
    fnGetStockTakeDocList: function(filter, skip, take, sortBy, isAscending, callback) {
        InventoryWebWS.fnGetStockTakeDocList(filter, skip, take, sortBy, isAscending, function(data) {
            if (callback) callback(data);
        });
    },

    //Get StockTakeDoc by StockTakeDocRefNo
    fnGetStockTakeDoc: function(StockTakeDocRefNo, callback) {
        InventoryWebWS.fnGetStockTakeDoc(StockTakeDocRefNo, function(result) {
            if (callback) callback(result);
        });
    },

    //Save StockTake Entries
    fnSaveStockTake: function(d, StockTakeDocRefNo, StockTakeDate, TakenBy, VerifiedBy, InventoryLocationID, callback) {
        InventoryWebWS.fnSaveStockTake(JSON.stringify(d), StockTakeDocRefNo, StockTakeDate, TakenBy, VerifiedBy, InventoryLocationID, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    //Get All Inventory Locations from Web Service
    fnGetInventoryLocationsFromWS: function(callback) {
        InventoryWebWS.fnGetInventoryLocations(function(data) {
            if (callback) callback(data);
        });
    },

    //Get All Inventory Location with outstanding Stock Take 
    fnGetAllLocationWithOutstandingStockTake: function(callback) {
        InventoryWebWS.fnGetAllLocationWithOutstandingStockTake(function(data) {
            if (callback) callback(data);
        });
    },

    //Get All PointOfSale from Web Service
    fnGetPointOfSales: function(callback) {
        InventoryWebWS.fnGetPointOfSales(function(data) {
            if (callback) callback(data);
        });
    },

    //Get List of all StockTake for Stock Take Approval
    fnGetStockTakeListForApproval: function(filter, callback) {
        InventoryWebWS.fnGetStockTakeListForApproval(filter, function(data) {
            if (callback) callback(data);
        });
    },

    //Get List of all StockTake for Stock Take Doc detail
    fnGetStockTakeList: function(StockTakeDocRefNo, callback) {
        InventoryWebWS.fnGetStockTakeList(StockTakeDocRefNo, function(data) {
            if (callback) callback(data);
        });
    },

    ////Update Stock Take Marked column 
    //fnStockTakeUpdateMarked: function (StockTakeID, bit, callback) {
    //    InventoryWebWS.fnStockTakeUpdateMarked(StockTakeID, bit, function (data) {
    //        if (callback) callback(data);
    //    });
    //},

    //Delete Stock Take
    fnDeleteStockTake: function(data, callback) {
        InventoryWebWS.fnDeleteStockTake(data, function(result) {
            if (callback) callback(result);
        });
    },

    //Approve/Reject Stock Take
    fnStockTakeApproval: function(StockTakeDocRefNo, InventoryLocationID, docStatus, callback) {
        InventoryWebWS.fnStockTakeApproval(StockTakeDocRefNo, InventoryLocationID, docStatus, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    //Get Notifications
    fnGetNotifications: function(callback) {
        InventoryWebWS.fnGetNotifications(sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    //Update Inventory Location's IsFrozen column 
    fnUpdateInventoryLocationIsFrozen: function(InventoryLocationID, IsFrozen, callback) {
        InventoryWebWS.fnUpdateInventoryLocationIsFrozen(InventoryLocationID, IsFrozen, sessionStorage.username, function(data) {
            if (callback) callback(data);
        });
    },

    //Update All Inventory Location's IsFrozen column 
    fnUpdateInventoryLocationIsFrozen_All: function(IsFrozen, callback) {
        InventoryWebWS.fnUpdateInventoryLocationIsFrozen_All(IsFrozen, sessionStorage.username, function(data) {
            if (callback) callback(data);
        });
    },

    //Check the status of Inventory Location (Frozen or not)
    fnIsInventoryLocationFrozen: function(InventoryLocationID, callback) {
        InventoryWebWS.fnIsInventoryLocationFrozen(InventoryLocationID, function(data) {
            if (callback) callback(data);
        });
    },

    //Revert Document Status in case some process failed.
    fnRevertPOHeaderStatus: function(PurchaseOrderHeaderRefNo, poStatus, callback) {
        InventoryWebWS.fnRevertPOHeaderStatus(PurchaseOrderHeaderRefNo, poStatus, function(data) {
            if (callback) callback(data);
        });
    },


    //Get Setting Purchase Header.
    fnGetPurchaseHeaderSetting: function(callback) {
        InventoryWebWS.fnGetPurchaseHeaderSetting(function(data) {
            if (callback) callback(data);
        });
    },

    //Change Expiry Date in PurchaseOrderDetail
    fnChangePODetailExpiryDate: function(d, callback) {
        InventoryWebWS.fnChangePODetailExpiryDate(JSON.stringify(d), function(result) {
            if (callback) callback(result);
        });
    },

    //Change Password
    fnChangePassword: function(username, oldpassword, newpassword, callback) {
        InventoryWebWS.fnChangePassword(username, oldpassword, newpassword, function(result) {
            if (callback) callback(result);
        });
    },

    //Get Email Notification List
    fnGetEmailNotificationList: function(filter, skip, take, sortBy, isAscending, callback) {
        InventoryWebWS.fnGetEmailNotificationList(filter, skip, take, sortBy, isAscending, function(result) {
            if (callback) callback(result);
        });
    },

    //Get Email Notification by Email Address
    fnGetEmailNotification: function(EmailAddress, callback) {
        InventoryWebWS.fnGetEmailNotification(EmailAddress, function(result) {
            if (callback) callback(result);
        });
    },

    //Save Email Notification
    fnSaveEmailNotification: function(data, username, callback) {
        InventoryWebWS.fnSaveEmailNotification(JSON.stringify(data), username, function(result) {
            if (callback) callback(result);
        });
    },

    //Delete Email Notification
    fnDeleteEmailNotification: function(EmailAddress, callback) {
        InventoryWebWS.fnDeleteEmailNotification(EmailAddress, function(result) {
            if (callback) callback(result);
        });
    },

    //Get Setting 
    fnGetSetting: function(AppSettingKey, boolValue, callback) {
        InventoryWebWS.fnGetSetting(AppSettingKey, boolValue, function(result) {
            if (callback) callback(result);
        });
    },

    //Get List of all PurchaseOrderHeader 
    fnGetItemBaseLevelList: function(filter, skip, take, sortBy, isAscending, callback) {

        InventoryWebWS.fnGetItemBaseLevelList(filter, skip, take, sortBy, isAscending, function(data) {
            if (callback) callback(data);
        });
    },

    //Add iTem Base Level
    fnAddItemBaseLevel: function(data, callback) {
        alert(JSON.stringify(data));
        InventoryWebWS.fnAddItemBaseLevel(JSON.stringify(data), sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    //Add iTem Base Level By Category
    fnAddItemBaseLevelByCategory: function(data, callback) {
        alert(JSON.stringify(data));
        InventoryWebWS.fnAddItemBaseLevelByCategory(JSON.stringify(data), sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    //Get available parameters for filtering
    fnGetParamsForReport: function(queryName, callback) {
        InventoryWebWS.fnGetParamsForReport(queryName, function(result) {
            if (callback) callback(result);
        });
    },

    //Get Report Data By QueryName
    fnGetReportDataByQueryName: function(queryName, callback) {
        InventoryWebWS.fnGetReportDataByQueryName(queryName, function(result) {
            if (callback) callback(result);
        });
    },

    // Get second screen files 
    fnGetSecondScreenFiles: function(filter, skip, take, sortBy, isAscending, callback) {
        InventoryWebWS.fnGetSecondScreenFiles(filter, skip, take, sortBy, isAscending, function(result) {
            if (callback) callback(result);
        });
    },

    // Rename second screen file
    fnRenameSecondScreenFile: function(d, callback) {
        InventoryWebWS.fnRenameSecondScreenFile(JSON.stringify(d), function(result) {
            if (callback) callback(result);
        });
    },

    fnDeleteSecondScreenFiles: function(d, callback) {
        InventoryWebWS.fnDeleteSecondScreenFiles(JSON.stringify(d), function(result) {
            if (callback) callback(result);
        });
    },

    fnCopySecondScreenFiles: function(fromPointOfSaleID, toPointOfSaleID, overwrite, callback) {
        InventoryWebWS.fnCopySecondScreenFiles(fromPointOfSaleID, toPointOfSaleID, overwrite, function(result) {
            if (callback) callback(result);
        });
    },

    fnDeleteItemOptimalStock: function(d, callback) {
        InventoryWebWS.fnDeleteItemOptimalStock(JSON.stringify(d), function(result) {
            alert(result);
            if (callback) callback(result);
        });
    },

    fnCopyItemOptimalStock: function(fromInventoryLocationID, toInventoryLocationID, overwrite, callback) {
        InventoryWebWS.fnCopyItemOptimalStock(fromInventoryLocationID, toInventoryLocationID, overwrite, function(result) {
            if (callback) callback(result);
        });
    },

    //Get Item Base Level
    fnGetItemBaseLevel: function(baselevelid, callback) {
        InventoryWebWS.fnGetItemBaseLevel(baselevelid, function(data) {
            if (callback) callback(data);
        });
    },

    //Get List of all InventoryLocation 
    fnGetCategoryList: function(filter, callback) {
        InventoryWebWS.fnGetCategoryList(filter, function(result) {
            if (callback) callback(result);
        });
    },

    // Get Item List
    fnGetItemList: function(filter, callback) {
        InventoryWebWS.fnGetItemList(filter, function(result) {
            if (callback) callback(result);
        });
    },

    // Get Item List for Recipe
    fnGetItemListForRecipe: function (filter, IsInInventory, IsRecipeHeader, callback) {
        InventoryWebWS.fnGetItemListForRecipe(filter, IsInInventory, IsRecipeHeader, function (result) {
            if (callback) callback(result);
        });
    },

    fnLoadItemsFromOptimalStock: function(PurchaseOrderHeaderRefNo, callback) {
        InventoryWebWS.fnLoadItemsFromOptimalStock(PurchaseOrderHeaderRefNo, sessionStorage.username, function (data) {
            if (callback) callback(data);
        });
    },
    fnLoadItemsFromSales: function(PurchaseOrderHeaderRefNo, startDate, endDate, callback) {
        InventoryWebWS.fnLoadItemsFromSales(PurchaseOrderHeaderRefNo, startDate, endDate, sessionStorage.username, function (data) {
            if (callback) callback(data);
        });
    },

    fnClearPurchaseOrderReference: function(PurchaseOrderHeaderRefNo, callback) {
        InventoryWebWS.fnClearPurchaseOrderReference(PurchaseOrderHeaderRefNo, function(data) {
            if (callback) callback(data);
        });
    },

    fnClearPurchaseOrderDetailReference: function(PurchaseOrderDetailRefNo, callback) {
        InventoryWebWS.fnClearPurchaseOrderDetailReference(PurchaseOrderDetailRefNo, function(data) {
            if (callback) callback(data);
        });
	},

    // Get Suppliers
	fnGetSuppliers: function (callback) {
	    InventoryWebWS.fnGetSuppliers(function (result) {
	        if (callback) callback(result.records);
	    });
	},

	// Get Suppliers
	fnGetSuppliersUserPortal: function(callback) {
	    InventoryWebWS.fnGetSuppliersUserPortal(function(result) {
	        if (callback) callback(result.records);
	    });
    },
    
    // Get Suppliers
	fnGetSuppliersUserPortalWithoutWarehouse: function(callback) {
	    InventoryWebWS.fnGetSuppliersUserPortalWithoutWarehouse(function(result) {
	        if (callback) callback(result.records);
	    });
    },
    
    // Get Warehouses
	fnGetWarehouseList: function(callback) {
	    InventoryWebWS.fnGetWarehouseList(function(result) {
	        if (callback) callback(result.records);
	    });
	},

    // Get Price Level
	fnGetPriceLevel: function (callback) {
	    InventoryWebWS.fnGetPriceLevel(function (result) {
	        if (callback) callback(result);
	    });
	},

    fnFetchStockTransferList: function(filter, take, skip, orderBy, orderDir, callback) {
        InventoryWebWS.fnFetchStockTransferList(filter, take, skip, orderBy, orderDir, function(result) {
            if (callback) callback(result);
        });
    },

    fnFetchStockTransferDet: function(stockTransferHdrRefNo, callback) {
        InventoryWebWS.fnFetchStockTransferDet(stockTransferHdrRefNo, function(result) {
            if (callback) callback(result);
        });
    },

    fnSaveStockTransferHdr: function(data, callback) {
        InventoryWebWS.fnSaveStockTransferHdr(data, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnFetchStockTransferHdr: function(stockTransferHdrRefNo, callback) {
        InventoryWebWS.fnFetchStockTransferHdr(stockTransferHdrRefNo, function(result) {
            if (callback) callback(result);
        });
    },

    fnRevertStockTransferHdr: function(stockTransferHdrRefNo, callback) {
        InventoryWebWS.fnRevertStockTransferHdr(stockTransferHdrRefNo, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnAddStockTransferItem: function(stockTransferHdrRefNo, itemNo, qty, callback) {
        InventoryWebWS.fnAddStockTransferItem(stockTransferHdrRefNo, itemNo, qty, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnAddAllTransferItems: function(stockTransferHdrRefNo, callback) {
        InventoryWebWS.fnAddAllTransferItems(stockTransferHdrRefNo, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnDeleteStockTransferItem: function(stockTransferDetRefNo, callback) {
        InventoryWebWS.fnDeleteStockTransferItem(stockTransferDetRefNo, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnChangeStockTransferQty: function (stockTransferDetRefNo, qty, callback) {
        InventoryWebWS.fnChangeStockTransferQty(stockTransferDetRefNo, qty, sessionStorage.username, function (result) {
            if (callback) callback(result);
        });
    },

    fnChangeFullFilledQty: function(stockTransferDetRefNo, fullFilledQty, remark, callback) {
        InventoryWebWS.fnChangeFullFilledQty(stockTransferDetRefNo, fullFilledQty, remark, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnRejectTransferDetail: function(stockTransferDetRefNo, callback) {
        InventoryWebWS.fnRejectTransferDetail(stockTransferDetRefNo, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnSetTallyAllStockTransfer: function(stockTransferDetRefNo, callback) {
        InventoryWebWS.fnSetTallyAllStockTransfer(stockTransferDetRefNo, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnReceiveStockTransfer: function(stockTransferHdrRefNo, callback) {
        InventoryWebWS.fnReceiveStockTransfer(stockTransferHdrRefNo, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnReceiveAllTallyStockTransfer: function(stockTransferHdrRefNo, callback) {
        InventoryWebWS.fnReceiveAllTallyStockTransfer(stockTransferHdrRefNo, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnStockTransferApproval: function(data, StockTransferHdrRefNo, username, priceLevel, autoStockIn, callback) {
        InventoryWebWS.fnStockTransferApproval(data, StockTransferHdrRefNo, sessionStorage.username, priceLevel, autoStockIn, function (result) {
            if (callback) callback(result);
        });
    },

    fnChangeSTDetailFactoryPrice: function(StockTransferDetRefNo, price, callback) {
        InventoryWebWS.fnChangeSTDetailFactoryPrice(StockTransferDetRefNo, price, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnChangeSTDetailFactoryPriceAll: function(StockTransferHdrRefNo, levelPrice, callback) {
        InventoryWebWS.fnChangeSTDetailFactoryPriceAll(StockTransferHdrRefNo, levelPrice, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnChangeRemarkHeader: function(PurchaseOrderHeaderRefNo, Remarks, callback) {
        InventoryWebWS.fnChangeRemarkHeader(PurchaseOrderHeaderRefNo, Remarks, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    // fnGetRecipeHeaderList
	fnGetRecipeHeaderList: function (filter, callback) {
	    InventoryWebWS.fnGetRecipeHeaderList(filter, function (result) {
	        if (callback) callback(result);
	    });
	},

    //Search Recipe
	fnSearchRecipeHeader: function (filter, take, skip, sortBy, isAscending, isHaveWrongConversion, callback) {
	    InventoryWebWS.fnSearchRecipeHeader(filter, take, skip, sortBy, isAscending, isHaveWrongConversion, function (result) {
	        if (callback) callback(result);
	    });
	},

    //Save Item Base level
	fnSaveRecipeWithConvRate: function (data, callback) {
	    //alert(JSON.stringify(data));
	    InventoryWebWS.fnSaveRecipeWithConvRate(data, sessionStorage.username, function (result) {
	        if (callback) callback(result);
	    });
	},

    // Get Recipe Header
	fnGetRecipeHeader: function (RecipeHeaderID, callback) {
	    InventoryWebWS.fnGetRecipeHeader(RecipeHeaderID, function (result) {
	        if (callback) callback(result);
	    });
	},

    // Get Recipe Detail
	fnGetRecipeDetail: function (RecipeHeaderID, callback) {
	    InventoryWebWS.fnGetRecipeDetail(RecipeHeaderID, function (result) {
	        if (callback) callback(result);
	    });
	},

    //delete Recipe Header
	fnDeleteRecipeHeader: function (RecipeHeaderID, callback) {
	    InventoryWebWS.fnDeleteRecipeHeader(RecipeHeaderID, sessionStorage.username, function (result) {
	        if (callback) callback(result);
	    });
	},

    //Get UOM List By ItemNo
	fnGetUOMListByItemNo: function (itemNo, callback) {
	    InventoryWebWS.fnGetUOMListByItemNo(itemNo, function (result) {
	        if (callback) callback(result);
	    });
	},

    // Search Item
	fnSearchRecipeMainItem: function (name, showProductOnly, skip, take, callback) {
	    InventoryWebWS.fnSearchRecipeMainItem(name.replace(/\*/g, '%'), showProductOnly, skip, take, function (data) {
	        if (callback) callback(data);
	    });
	},

    // Get Cost Rate
	fnGetCostRateByItemNo: function (itemNo, callback) {
	    InventoryWebWS.fnGetCostRateByItemNo(itemNo, function (result) {
	        if (callback) callback(result);
	    });
	},

    // Get Conversion Rate
	fnGetConversionRate: function (itemNo, fromUOM, toUOM, callback) {
	    InventoryWebWS.fnGetConversionRate(itemNo, fromUOM, toUOM, function (result) {
	        if (callback) callback(result);
	    });
	},

    // Get UOM List
	fnGetUOMList: function (callback) {
	    InventoryWebWS.fnGetUOMList(function (result) {
	        if (callback) callback(result);
	    });
	},

    // Get UOM Conversion List
	fnGetUOMConversionList: function (filter, skip, take, sortBy, isAscending, callback) {
	    InventoryWebWS.fnGetUOMConversionList(filter, skip, take, sortBy, isAscending, function (result) {
	        if (callback) callback(result);
	    });
	},

    // Save UOM Conversion
	fnSaveUOMConversion: function (data, callback) {
	    InventoryWebWS.fnSaveUOMConversion(JSON.stringify(data), sessionStorage.username, function (result) {
	        if (callback) callback(result);
	    });
	},

    // Delete UOM Conversion
	fnDeleteUOMConversion: function (data, callback) {
	    InventoryWebWS.fnDeleteUOMConversion(JSON.stringify(data), sessionStorage.username, function (result) {
	        if (callback) callback(result);
	    });
	},

    // Search Cook Item
	fnSearchCookItem: function (name, skip, take, callback) {
	    InventoryWebWS.fnSearchCookItem(name.replace(/\*/g, '%'), skip, take, function (data) {
	        if (callback) callback(data);
	    });
	},

    // Search Cook Item
	fnFetchCookItem: function (filter, skip, take, sortBy, isAscending, callback) {
	    InventoryWebWS.fnFetchCookItem(filter, skip, take, sortBy, isAscending, function (data) {
	        if (callback) callback(data);
	    });
	},

	fnCookItem: function (itemNo, quantity, pointOfSaleID, inventoryLocationID, remarks, callback) {
	    InventoryWebWS.fnCookItem(itemNo, quantity, pointOfSaleID, inventoryLocationID, remarks, sessionStorage.username, function (data) {
	        if (callback) callback(data);
	    });
	},

	fnSaveItemCookHistory: function(data, callback) {
	    InventoryWebWS.fnSaveItemCookHistory(data, sessionStorage.username, function(data) {
	        if (callback) callback(data);
	    });
	},


	fnGetItemCookHistory: function(DocumentNo, callback) {
	    InventoryWebWS.fnGetItemCookHistory(DocumentNo, function(data) {
	        if (callback) callback(data);
	    });
	},

	fnDeleteItemCookDetail: function(d, callback) {
        InventoryWebWS.fnDeleteItemCookDetail(JSON.stringify(d), sessionStorage.username, function(result) {
            if (callback) callback(result, d);
        });
    },

    fnAddItemCookDetail: function(d, callback) {
        InventoryWebWS.fnAddItemCookDetail(JSON.stringify(d), sessionStorage.username, function(result) {
            if (callback) callback(result, d);
        });
    },
    
    fnChangeQtyItemCookHistory: function(itemCookHistoryID, qty, callback) {
        InventoryWebWS.fnChangeQtyItemCookHistory(itemCookHistoryID, qty, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnChangeQtyItemCookDetail: function(itemCookDetailID, qty, callback) {
        InventoryWebWS.fnChangeQtyItemCookDetail(itemCookDetailID, qty, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnCookItemWithDetail: function(itemCookHistoryID, pointOfSaleID, callback) {
        InventoryWebWS.fnCookItemWithDetail(itemCookHistoryID, pointOfSaleID, sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    },

    fnCancelItemCookHistory: function(itemCookHistoryID, callback) {
        InventoryWebWS.fnCancelItemCookHistory(itemCookHistoryID,  sessionStorage.username, function(result) {
            if (callback) callback(result);
        });
    }	
}