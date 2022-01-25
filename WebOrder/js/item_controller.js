ItemController = {
    fnSearchItem: function (name, IsInventoryItemOnly, ShowSystemItem, callback) {
        DAL.fnOpenDB();
        DAL.DB.transaction(function (tx) {
            var sql = "SELECT Category.IsForSale, Category.IsDiscountable, Category.CategoryName, Category.Category_ID, Category.IsGST, Category.AccountCategory, Item.ItemNo, Item.ItemName, Item.Barcode, Item.RetailPrice, Item.FactoryPrice, Item.MinimumPrice, Item.ItemDesc, Item.IsInInventory, Item.IsNonDiscountable, Item.Brand, Item.ProductLine, Item.Remark, Item.Deleted, Item.Attributes1, Item.Attributes2, Item.Attributes3, Item.Attributes4, Item.Attributes5, Item.Attributes6, Item.Attributes8, Item.Attributes7, Category.ItemDepartmentId, ItemDepartment.DepartmentName, Item.IsServiceItem, Item.IsCourse, Item.CourseTypeID, Item.ProductionDate, Item.IsGST AS Expr1, Item.hasWarranty, Item.IsDelivery, Item.GSTRule, Item.IsVitaMix, Item.IsWaterFilter, Item.IsYoung, Item.IsJuicePlus, Item.IsCommission FROM Category INNER JOIN Item ON Category.CategoryName = Item.CategoryName INNER JOIN ItemDepartment ON Category.ItemDepartmentId = ItemDepartment.ItemDepartmentID WHERE Item.ItemNo || Item.ItemName || Item.CategoryName || COALESCE(ItemDepartment.DepartmentName, '') || COALESCE(Item.ItemDesc, '') || COALESCE(Item.Attributes1, '') || COALESCE(Item.Attributes2, '') || COALESCE(Item.Attributes3, '') || COALESCE(Item.Attributes4, '') || COALESCE(Item.Attributes5, '') || COALESCE(Item.Attributes6, '') || COALESCE(Item.Attributes8, '') || COALESCE(Item.Attributes7, '') LIKE ? AND Item.Deleted = 'false'";
            var sqlCount = "SELECT COUNT(*) AS NumberOfRecords FROM Category INNER JOIN Item ON Category.CategoryName = Item.CategoryName INNER JOIN ItemDepartment ON Category.ItemDepartmentId = ItemDepartment.ItemDepartmentID WHERE Item.ItemNo || Item.ItemName || Item.CategoryName || COALESCE(ItemDepartment.DepartmentName, '') || COALESCE(Item.ItemDesc, '') || COALESCE(Item.Attributes1, '') || COALESCE(Item.Attributes2, '') || COALESCE(Item.Attributes3, '') || COALESCE(Item.Attributes4, '') || COALESCE(Item.Attributes5, '') || COALESCE(Item.Attributes6, '') || COALESCE(Item.Attributes8, '') || COALESCE(Item.Attributes7, '') LIKE ? AND Item.Deleted = 'false'";

            if (IsInventoryItemOnly) {
                sql += " AND Item.IsInInventory = 'true' ";
                sqlCount += " AND Item.IsInInventory = 'true' ";
            };
            if (!ShowSystemItem) {
                sql += " AND Item.CategoryName <> 'SYSTEM' ";
                sqlCount += " AND Item.CategoryName <> 'SYSTEM' ";
            };

            sql += " LIMIT 50"; // Limit the search result to display

            tx.executeSql(sqlCount, ['%'+name+'%'], function (tx, countResult) {
                var res = new Array();
                var numberOfRecords = 0;
                if (countResult.rows.length > 0) {
                    numberOfRecords = countResult.rows.item(0).NumberOfRecords;
                    tx.executeSql(sql, ['%' + name + '%'], function (tx, results) {
                        if (results.rows.length > 0) {
                            for (var i = 0; i < results.rows.length; i++) {
                                res.push(results.rows.item(i));
                            };
                        };

                        var resultData = {
                            records: res,
                            totalRecords: numberOfRecords
                        };

                        if (callback) callback(resultData);
                    }, function (tx, err) {
                        alert('error: ' + err.code + "\n" + err.message);
                    });
                };
            }, function (tx, err) {
                alert('error: ' + err.code + "\n" + err.message);
            });

            

        });
    }
}