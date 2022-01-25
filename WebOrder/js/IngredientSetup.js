/// <reference path="common_functions.js" />
/// <reference path="data_access_layer.js" />

$(function() {
    var dateDisplayFormat = settings.dateDisplayFormat;
    var dateDisplayFormatWithTime = settings.dateDisplayFormatWithTime;
    var dateValueFormat = settings.dateValueFormat;

    $("#divRecipeHeader").hide();
    $("#divRecipeDetail").hide();

    ko.applyBindings(new IngredientSetupModel(), document.getElementById("DynamicContent"));

    function Recipe(data) {
        var self = this;

        self.RecipeName = data.RecipeName;
        self.ItemNoHdr = data.ItemNo;
        self.Type = data.Type;
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

    function RecipeToSave(data) {
        var self = this;

        self.RecipeHeaderID = data.RecipeHeaderID;
        self.RecipeName = data.RecipeName;
        self.ItemNo = data.ItemNo;
        self.Type = data.Type;
        self.Details = data.Details;
    }

    function RecipeDetail(data) {
        var self = this;

        self.IsSelected = ko.observable(false);
        self.RecipeDetailID = ko.observable("");
        self.RecipeHeaderNo = ko.observable("");
        self.RecipeName = ko.observable("");
        self.ItemNo = ko.observable("");
        self.ItemName = ko.observable("");
        self.Qty = ko.observable(0);
        self.Uom = ko.observable("");
        self.IsPacking = ko.observable(false);
        self.MaterialUOM = ko.observable("");
        self.ConversionRate = ko.observable("");
        self.CostRate = ko.observable("");

        self.ConversionDesc = ko.computed(function() {
            if (self.MaterialUOM() != null && self.Uom() != null)
                return '(1 ' + self.MaterialUOM() + ' = ' + self.ConversionRate() + ' ' + self.Uom() + ')';
            else
                return '';
        });

        self.IsUOMSimilar = ko.computed(function() {
            return self.MaterialUOM() != null && self.Uom() != null && self.MaterialUOM() == self.Uom();
        });

        self.IsMissingConversionRecipeDetail = ko.computed(function() {
            return ((self.Uom() != self.MaterialUOM()) && (self.ConversionRate() == null || self.ConversionRate() == '')) ? 'danger' : '';
            //return self.ConversionRate() != null && self.ConversionRate() != null ? '' : 'danger';
        });

        self.ConversionRateStr = ko.computed(function() {
            return self.IsUOMSimilar() ? '1' : self.ConversionRate();
        });

        self.CostPrice = ko.computed(function() {
            var convrate = self.ConversionRate();
            if (convrate <= 0)
                convrate = 1;

            var qty = parseFloat(self.Qty());
            qty = Math.round(qty * 10000) / 10000;

            var costRate = parseFloat(self.CostRate());
            costRate = Math.round(costRate * 10000) / 10000;

            var finalPrice = (qty / convrate) * costRate;
            finalPrice = Math.round(finalPrice * 10000) / 10000;

            return finalPrice;
        });

        self.CostRateStr = ko.computed(function() {
            return accounting.formatMoney(self.CostPrice(), "$ ", 4, ",", ".");
        });

        self.CostRateWithFormat = ko.computed(function() {
            return accounting.formatMoney(self.CostRate(), "$ ", 4, ",", ".");
        });
    }

    function RecipeHeader(data) {
        var self = this;

        self.RecipeHeaderID = data.RecipeHeaderID;
        self.ItemNo = data.ItemNo;
        self.RecipeName = data.RecipeName;
        self.Type = data.Type;
        self.Deleted = data.Deleted;
    }

    function ViewRecipe(data) {
        var self = this;

        self.RecipeHeaderID = data.RecipeHeaderID;
        self.RecipeHeaderNo = data.RecipeHeaderNo;
        self.RecipeName = data.RecipeName;
        self.TotalMaterial = data.TotalMaterial;
        self.TotalMaterialCost = data.TotalMaterialCost;
        self.IsHaveWrongConv = data.IsHaveWrongConv;
        self.RetailPrice = data.RetailPrice;
        self.TotalMaterialCostStr = ko.computed(function() {
            return accounting.formatMoney(self.TotalMaterialCost, "$ ", 4, ",", ".")
        });
        self.RetailPriceStr = ko.computed(function() {
            return accounting.formatMoney(self.RetailPrice, "$ ", 2, ",", ".")
        });

        //   	self.RecipeDetailID = data.RecipeDetailID;
        //   	self.ItemNo = data.ItemNo;
        // self.ItemName = data.ItemName;
        // self.Qty = data.Qty;
        // self.Uom = data.Uom;
        // self.IsPacking = data.IsPacking;
        // self.IsPackingStr = "No";
        // if(data.IsPacking.toString().toLowerCase() == "true")
        // {
        // 	self.IsPackingStr = "Yes";
        // }
    }

    function IngredientSetupModel() {
        var self = this;

        /** for search page */
        self.sortKeyPOHdr = ko.observable({ columnName: "RecipeHeaderNo", asc: false });
        self.SavedFilter = null;
        self.isPOHdrNotFound = ko.observable(false);

        self.showProductOnly = ko.observable(false);

        self.FilterRecipeHeaderName = ko.observable("");
        self.RecipeList = ko.observableArray();

        self.FilterItemName = ko.observable("");
        self.ItemList = ko.observableArray();

        self.ViewRecipeList = ko.observableArray();
        self.showLoadMore = ko.observable(false);

        self.TitleSearchDialog = ko.observable("");
        self.HeaderSearchItemNo = ko.observable("");
        self.HeaderSearchItemName = ko.observable("");

        /*RecipeHeader*/
        self.CurrentRecipeHeader = ko.observable();
        self.IsEdit = ko.observable(true);

        self.RecipeDetailList = ko.observableArray();
        self.IsAddItemToHeader = ko.observable(false);
        self.UomList = ko.observableArray();


        /*RecipeDetail*/
        self.CurrentRecipeDetail = ko.observable();
        self.DetailIsEdit = ko.observable();
        self.DetailIsNew = ko.observable(false);

        self.SearchItemResults = ko.observableArray();
        self.ShowLoadMoreItems = ko.observable(false);
        self.SavedItemFilter = null;
        self.IsItemNotFound = ko.observable(false);


        self.LoadDataRecipeAndItem = function() {
            var filter = {
                recipeName: "ALL",
                itemName: "ALL"
            };

            DAL.fnGetRecipeHeaderList(JSON.stringify(filter), function(data) {

                var itemAll = { ItemNo: 'ALL', ItemName: 'ALL' };
                self.RecipeList.push(new Item(itemAll));

                for (var i = 0; i < data.records.length; i++) {
                    self.RecipeList.push(new Item(data.records[i]));
                };
            });

            DAL.fnGetItemListForRecipe(JSON.stringify(filter), true, false, function(data) {
                self.ItemList.removeAll();

                var itemAll = { ItemNo: 'ALL', ItemName: 'ALL' };
                self.ItemList.push(new Item(itemAll));

                for (var i = 0; i < data.records.length; i++) {
                    self.ItemList.push(new Item(data.records[i]));
                };
            });
        }

        self.loadData = function() {

            var filter;

            if (self.SavedFilter) {
                filter = self.SavedFilter;
            }
            else {
                filter = {
                    recipeHeaderName: self.FilterRecipeHeaderName(),
                    itemName: self.FilterItemName()
                };
                self.SavedFilter = filter;
            };

            //          DAL.fnSearchRecipe(JSON.stringify(filter),self.ViewRecipeList().length,settings.numOfRecords,self.sortKeyPOHdr().columnName, self.sortKeyPOHdr().asc, function (data) {

            //             	for (var i = 0; i < data.records.length; i++) {
            // 		self.ViewRecipeList.push(new ViewRecipe(data.records[i]));
            //              };

            //              if (data.totalRecords > self.ViewRecipeList().length) {
            //                  self.showLoadMore(true);
            //              }
            //              else {
            //                  self.showLoadMore(false);
            //              };

            // });

            DAL.fnSearchRecipeHeader(JSON.stringify(filter), self.ViewRecipeList().length, settings.numOfRecords, self.sortKeyPOHdr().columnName, self.sortKeyPOHdr().asc, false, function(data) {

                for (var i = 0; i < data.records.length; i++) {
                    self.ViewRecipeList.push(new ViewRecipe(data.records[i]));
                };

                if (data.totalRecords > self.ViewRecipeList().length) {
                    self.showLoadMore(true);
                }
                else {
                    self.showLoadMore(false);
                };

            });
        }

        self.showRecipeWithWrongConversion = function() {
            self.ViewRecipeList.removeAll();
            self.showLoadMore(false);


            var filter;

            if (self.SavedFilter) {
                filter = self.SavedFilter;
            }
            else {
                filter = {
                    recipeHeaderName: self.FilterRecipeHeaderName(),
                    itemName: self.FilterItemName()
                };
                self.SavedFilter = filter;
            };

            DAL.fnSearchRecipeHeader(JSON.stringify(filter), self.ViewRecipeList().length, settings.numOfRecords, self.sortKeyPOHdr().columnName, self.sortKeyPOHdr().asc, true, function(data) {

                for (var i = 0; i < data.records.length; i++) {
                    self.ViewRecipeList.push(new ViewRecipe(data.records[i]));
                };

                if (data.totalRecords > self.ViewRecipeList().length) {
                    self.showLoadMore(true);
                }
                else {
                    self.showLoadMore(false);
                };

            });
        }

        self.searchViewRecipe = function() {
            self.ViewRecipeList.removeAll();
            self.showLoadMore(false);
            self.SavedFilter = null;

            self.loadData();
        }

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
            self.ViewRecipeList.removeAll();
            self.loadData();
        };

        self.addNewRecipeHeader = function() {
            self.IsEdit(false);
            var recipeHeader =
	    		{
	    		    RecipeName: "",
	    		    ItemNo: "",
	    		    Deleted: false
	    		};
            self.CurrentRecipeHeader(new RecipeHeader(recipeHeader));
            self.RecipeDetailList.removeAll();

            $("#divInggredientHdrList").hide();
            $("#divRecipeDetail").hide();
            $("#divRecipeHeader").fadeIn();
        }

        self.backToList = function() {
            $("#divRecipeHeader").hide();
            $("#divRecipeDetail").hide();
            $("#divInggredientHdrList").fadeIn();
        };


        self.addRecipeHeader = function() {
            var isValid = false;

            if (self.CurrentRecipeHeader().ItemNo == "") {
                BootstrapDialog.alert("Please input Item No.");
            }
            else if (self.RecipeDetailList().length == 0) {
                BootstrapDialog.alert("Please at least add one Material");
            }
            else {
                isValid = true;
            }

            if (isValid) {
                var data = new RecipeToSave({
                    RecipeHeaderID: self.CurrentRecipeHeader().RecipeHeaderID,
                    RecipeName: self.CurrentRecipeHeader().RecipeName,
                    ItemNo: self.CurrentRecipeHeader().ItemNo,
                    Type: self.CurrentRecipeHeader().Type,
                    Details: self.RecipeDetailList()
                });

                DAL.fnSaveRecipeWithConvRate(ko.toJSON(data), function(result) {
                    if (!result.error) {
                        if (result.status)
                            BootstrapDialog.alert(result.status);
                        self.backToList();
                        self.ViewRecipeList.removeAll();
                        self.RecipeDetailList.removeAll();
                        self.SavedFilter = null;
                        self.loadData();
                    }
                    else {
                        BootstrapDialog.alert(result.status);

                    }
                });
            }
        }

        self.isPOHdrNotFound = ko.computed(function() {
            return self.ViewRecipeList().length < 1;
        });


        self.changeRecipeFilter = function(item) {
            self.FilterRecipeHeaderName(item.ItemName);
        };

        self.changeItemFilter = function(item) {
            self.FilterItemName(item.ItemName);
        };

        self.RecipeListFiltered = ko.computed(function() {
            if (self.FilterRecipeHeaderName() && self.FilterRecipeHeaderName().toUpperCase() != "ALL") {
                return ko.utils.arrayFilter(self.RecipeList(), function(item) {
                    return item.ItemName.toUpperCase().indexOf(self.FilterRecipeHeaderName().toUpperCase()) == 0;
                });
            }
            else {
                return self.RecipeList();
            };
        });


        self.ItemListFiltered = ko.computed(function() {
            if (self.FilterItemName() && self.FilterItemName().toUpperCase() != "ALL") {
                return ko.utils.arrayFilter(self.ItemList(), function(item) {
                    return item.ItemName.toUpperCase().indexOf(self.FilterItemName().toUpperCase()) == 0;
                });
            }
            else {
                return self.ItemList();
            };
        });

        /*****region function Recipe Header*/
        self.updateRecipeHeader = function(recipe) {

            var filter = recipe.RecipeHeaderID;

            DAL.fnGetRecipeHeader(recipe.RecipeHeaderID, function(result) {
                if (result.status == "") {
                    console.log("result.recipe:" + result.recipe.Type);

                    var recipe = new RecipeHeader(result.recipe);
                    self.CurrentRecipeHeader(recipe);
                }
                else {
                    BootstrapDialog.alert(result.status);
                };
            });

            self.RecipeDetailList.removeAll();


            //          DAL.fnSearchRecipe(JSON.stringify(filter),self.RecipeDetailList().length,-1,"ItemNo", true, function (data) {

            //             	for (var i = 0; i < data.records.length; i++) {
            // 		self.RecipeDetailList.push(new RecipeDetail(data.records[i]));
            //              };
            // });


            DAL.fnGetRecipeDetail(filter, function(data) {

                for (var i = 0; i < data.records.length; i++) {
                    //self.RecipeDetailList.push(new RecipeDetail(data.records[i]));
                    var detail = new RecipeDetail();
                    detail.RecipeDetailID(data.records[i].RecipeDetailID);
                    detail.RecipeHeaderNo(data.records[i].RecipeHeaderNo);
                    detail.RecipeName(data.records[i].RecipeName);
                    detail.ItemNo(data.records[i].ItemNo);
                    detail.ItemName(data.records[i].ItemName);
                    detail.Qty(data.records[i].Qty);
                    detail.Uom(data.records[i].Uom);
                    detail.IsPacking(data.records[i].IsPacking);
                    detail.MaterialUOM(data.records[i].MaterialUOM);
                    detail.ConversionRate(data.records[i].ConversionRate);
                    detail.CostRate(data.records[i].CostRate);

                    self.RecipeDetailList.push(detail);
                };


            });
            self.IsEdit(true);
            $("#divInggredientHdrList").hide();
            $("#divRecipeDetail").hide();
            $("#divRecipeHeader").fadeIn();


        }

        self.TotalCostRecipeHeader = ko.computed(function() {
            var total = 0;

            for (var i = 0; i < self.RecipeDetailList().length; i++) {
                if (self.RecipeDetailList()[i].CostRate() != null)
                    total += self.RecipeDetailList()[i].CostPrice();
            }

            //return Math.round(total).toFixed(4);
            return accounting.formatMoney(total, "$ ", 4, ",", "."); ;
        });

        self.IsMissingConversionRecipeHeader = ko.computed(function() {
            var ismissing = false;

            for (var i = 0; i < self.RecipeDetailList().length; i++) {
                if ((self.RecipeDetailList()[i].ConversionRate() == null || self.RecipeDetailList()[i].ConversionRate() == '') && (self.RecipeDetailList()[i].Uom() != self.RecipeDetailList()[i].MaterialUOM())) {
                    ismissing = true;
                }
            }

            return ismissing;
        });

        self.IsSelectAllFiles = ko.computed(function() {
            return ko.utils.arrayFilter(self.RecipeDetailList(), function(item) {
                return item.IsSelected() == true;
            }).length == self.RecipeDetailList().length;
        });

        self.toggleSelectAllFiles = function() {
            var toggle = !self.IsSelectAllFiles();
            for (var i = 0; i < self.RecipeDetailList().length; i++) {
                self.RecipeDetailList()[i].IsSelected(toggle);
            }
            return true;
        };

        self.selectAllRecipeDetail = function() {
            for (var i = 0; i < self.RecipeDetailList().length; i++) {
                self.RecipeDetailList()[i].IsSelected(true);
            }
            return true;
        }

        self.clearSelectionRecipeDetail = function() {
            for (var i = 0; i < self.RecipeDetailList().length; i++) {
                self.RecipeDetailList()[i].IsSelected(false);
            }
            return true;
        }

        self.deleteSelectionRecipeDetail = function() {
            BootstrapDialog.confirm("Are you sure you want to delete the selected ingredient for this product ?", function(ok) {
                if (ok) {
                    var itemsToDelete = ko.utils.arrayFilter(self.RecipeDetailList(), function(item) {
                        return item.IsSelected() == true;
                    });

                    for (var i = 0; i < itemsToDelete.length; i++) {

                        self.RecipeDetailList.remove(itemsToDelete[i]);

                    };
                }
            });
        }

        self.deleteRecipeHeader = function() {
            BootstrapDialog.confirm("Are you sure you want to delete this recipe ?", function(ok) {
                if (ok) {
                    DAL.fnDeleteRecipeHeader(self.CurrentRecipeHeader().RecipeHeaderID, function(result) {
                        if (result.status == "") {
                            BootstrapDialog.alert("The recipe have been deleted successfully.");

                            self.ViewRecipeList.removeAll();
                            self.loadData();
                            $("#divRecipeDetail").hide();
                            $("#divRecipeHeader").hide();
                            $("#divInggredientHdrList").fadeIn();
                        }
                        else {
                            BootstrapDialog.alert(result.status);
                        };
                    });
                }
            });
        }

        /**endregion*/

        /*****region function Recipe Header*/
        self.openAddRecipeDetail = function() {

            if (self.CurrentRecipeHeader().RecipeName == "" && self.CurrentRecipeHeader().ItemNo == "") {
                BootstrapDialog.alert("Please input Product Name first");

            } else {
                var recipeDetail =
            		{
            		    RecipeDetailID: "",
            		    RecipeName: self.CurrentRecipeHeader().RecipeName,
            		    RecipeHeaderNo: self.CurrentRecipeHeader().ItemNo

            		};

                var detail = new RecipeDetail();
                detail.RecipeDetailID("");
                detail.RecipeName(self.CurrentRecipeHeader().RecipeName);
                detail.RecipeHeaderNo(self.CurrentRecipeHeader().ItemNo);


                self.CurrentRecipeDetail(detail);
                //self.CurrentRecipeDetail(new RecipeDetail(recipeDetail));
                self.DetailIsNew(true);

                $("#divInggredientHdrList").hide();
                $("#divRecipeHeader").hide();
                $("#divRecipeDetail").fadeIn();
            }
        }

        self.openSearchProductDialogFromHeader = function() {
            self.IsAddItemToHeader(true);
            self.showProductOnly(false);
            self.SearchItemResults.removeAll();
            self.ShowLoadMoreItems(false);
            self.TitleSearchDialog("Search Product");
            self.HeaderSearchItemNo("Item No");
            self.HeaderSearchItemName("Product Name");
            $("#searchProductModal").modal({ backdrop: 'static' });
        }

        self.openSearchProductDialog = function() {
            self.showProductOnly(true);
            self.SearchItemResults.removeAll();
            self.ShowLoadMoreItems(false);
            self.TitleSearchDialog("Search Material");
            self.HeaderSearchItemNo("Material Item No");
            self.HeaderSearchItemName("Material Name");
            $("#searchProductModal").modal({ backdrop: 'static' });
        }

        self.backReceiptHeader = function() {
            $("#divInggredientHdrList").hide();
            $("#divRecipeDetail").hide();
            $("#divRecipeHeader").fadeIn();
        }

        self.updateRecipeDetail = function(detail) {
            if (self.CurrentRecipeHeader().RecipeName == "" && self.CurrentRecipeHeader().ItemNo == "") {
                BootstrapDialog.alert("Please input Product Name first");

            } else {
                DAL.fnGetUOMListByItemNo(detail.ItemNo, function(result) {
                    if (result.status == "") {
                        self.UomList(result.records);
                        var newDetail = new RecipeDetail();
                        newDetail.RecipeDetailID(detail.RecipeDetailID());
                        newDetail.RecipeHeaderNo(detail.RecipeHeaderNo());
                        newDetail.RecipeName(detail.RecipeName());
                        newDetail.ItemNo(detail.ItemNo());
                        newDetail.ItemName(detail.ItemName());
                        newDetail.Qty(detail.Qty());
                        newDetail.Uom(detail.Uom());
                        newDetail.IsPacking(detail.IsPacking());
                        newDetail.MaterialUOM(detail.MaterialUOM());
                        newDetail.ConversionRate(detail.ConversionRate());
                        newDetail.CostRate(detail.CostRate());

                        if (newDetail.ConversionRate() == null || newDetail.ConversionRate() == '') {
                            newDetail.ConversionRate(1);
                        }
                        self.CurrentRecipeDetail(newDetail);
                        self.DetailIsNew(false);

                        $("#divInggredientHdrList").hide();
                        $("#divRecipeHeader").hide();
                        setTimeout(function() {
                            $("#divRecipeDetail").fadeIn();
                            self.CurrentRecipeDetail().ConversionRate(detail.ConversionRate());
                            console.log("Set Conv Rate:" + detail.ConversionRate());
                        }, 100);
                    }
                });
            }
        }

        self.addRecipeDetail = function() {

            var qty = parseFloat(self.CurrentRecipeDetail().Qty()) || 0;
            var conversionRate = parseFloat(self.CurrentRecipeDetail().ConversionRate()) || 0;

            //console.log(">> ConversionRate(): " + self.CurrentRecipeDetail().ConversionRate());
            //console.log(">> conversionRate: " + conversionRate);

            if (qty == 0) {
                BootstrapDialog.alert("Please Input Quantity");
            }
            else if ((self.CurrentRecipeDetail().Uom() != null && self.CurrentRecipeDetail().Uom() != '') &&
                     (self.CurrentRecipeDetail().MaterialUOM() != null && self.CurrentRecipeDetail().MaterialUOM() != '') &&
                     (self.CurrentRecipeDetail().MaterialUOM() != self.CurrentRecipeDetail().Uom()) &&
                     (conversionRate == 0)) {
                BootstrapDialog.alert("Please Input Conversion Rate");
            }
            else {
                var isFound = false;
                var detail = self.CurrentRecipeDetail();
                for (var i = 0; i < self.RecipeDetailList().length; i++) {
                    var recipe = self.RecipeDetailList()[i];
                    if (recipe.ItemNo() == self.CurrentRecipeDetail().ItemNo()) {
                        isFound = true;
                        self.RecipeDetailList()[i].Qty(detail.Qty());
                        self.RecipeDetailList()[i].Uom(detail.Uom());
                        self.RecipeDetailList()[i].IsPacking(detail.IsPacking());
                        self.RecipeDetailList()[i].MaterialUOM(detail.MaterialUOM());
                        self.RecipeDetailList()[i].ConversionRate(detail.ConversionRate());
                        self.RecipeDetailList()[i].CostRate(detail.CostRate());
                        //self.RecipeDetailList.replace(self.RecipeDetailList()[i], detail);
                    }
                };

                if (!isFound) {
                    var detail = self.CurrentRecipeDetail();
                    self.RecipeDetailList.push(detail);
                }

                $("#divInggredientHdrList").hide();
                $("#divRecipeDetail").hide();
                $("#divRecipeHeader").fadeIn();
            }
        }

        self.searchItem = function() {
            self.SearchItemResults.removeAll();
            self.ShowLoadMoreItems(false);
            self.SavedItemFilter = null;
            self.loadSearchItem();
            $("#txtSearchItem").val("");
        };

        self.loadSearchItem = function() {
            var filter;
            if (self.SavedItemFilter) {
                filter = self.SavedItemFilter;
            }
            else {
                filter = $("#txtSearchItem").val();
                self.SavedItemFilter = filter;
            };
            //alert(filter);
            DAL.fnSearchRecipeMainItem(filter, self.showProductOnly(), self.SearchItemResults().length, settings.numOfRecords, function(result) {
                if (result != null) {
                    for (var i = 0; i < result.records.length; i++) {
                        self.SearchItemResults.push(new ItemToAdd(result.records[i]));
                    };

                    if (result.totalRecords > self.SearchItemResults().length) {
                        self.ShowLoadMoreItems(true);
                    }
                    else {
                        self.ShowLoadMoreItems(false);
                    };
                };
            });
        };

        self.selectItem = function(item) {
            if (self.IsAddItemToHeader()) {

                var recipeHeader = new RecipeHeader({
                    RecipeHeaderID: self.CurrentRecipeHeader().RecipeHeaderID,
                    ItemNo: item.data.ItemNo,
                    RecipeName: item.data.ItemName,
                    Type: item.data.Type,
                    Deleted: self.CurrentRecipeHeader().Deleted
                });

                self.CurrentRecipeHeader(recipeHeader);
                self.IsAddItemToHeader(false);
            }
            else {
                // var recipeDetail = new RecipeDetail({
                // 	RecipeDetailID: "",
                // 	RecipeHeaderNo: self.CurrentRecipeDetail().RecipeHeaderNo(),
                // 	RecipeName: self.CurrentRecipeDetail().RecipeName(),
                // 	ItemNo: item.data.ItemNo,
                // 	ItemName: item.data.ItemName,
                // 	Uom: item.data.Uom,
                // 	Qty:  self.CurrentRecipeDetail().Qty(),
                // 	IsPacking:  self.CurrentRecipeDetail().IsPacking()
                // });

                var recipeDetail = new RecipeDetail();
                recipeDetail.RecipeDetailID("");
                recipeDetail.RecipeHeaderNo(self.CurrentRecipeDetail().RecipeHeaderNo());
                recipeDetail.RecipeName(self.CurrentRecipeDetail().RecipeName());
                recipeDetail.ItemNo(item.data.ItemNo);
                recipeDetail.ItemName(item.data.ItemName);
                recipeDetail.MaterialUOM(item.data.Uom);
                recipeDetail.Qty(self.CurrentRecipeDetail().Qty());
                recipeDetail.IsPacking(self.CurrentRecipeDetail().IsPacking());

                DAL.fnGetCostRateByItemNo(item.data.ItemNo, function(result) {
                    if (result.status == "") {
                        recipeDetail.CostRate(result.record);
                    }
                });


                DAL.fnGetUOMListByItemNo(item.data.ItemNo, function(result) {
                    if (result.status == "") {
                        self.UomList(result.records);
                        recipeDetail.Uom(result.records[0]);
                    }
                });

                if ((recipeDetail.Uom() != null && recipeDetail.Uom() != '')
                    && (recipeDetail.MaterialUOM() != null && recipeDetail.MaterialUOM() != '')
                    && (recipeDetail.Uom() != recipeDetail.MaterialUOM())) {
                    DAL.fnGetConversionRate(recipeDetail.ItemNo(), recipeDetail.MaterialUOM(), recipeDetail.Uom(), function(result) {
                        if (result.status == "") {
                            if (result.record == 0) {
                                recipeDetail.ConversionRate(1)
                            }
                            else {
                                recipeDetail.ConversionRate(result.record);
                            }
                        } else {
                            recipeDetail.ConversionRate(1);
                        }
                    });

                } else {
                    recipeDetail.ConversionRate(1);
                }

                self.CurrentRecipeDetail(recipeDetail);
            }


            $("#searchProductModal").modal("hide");
        };

        self.IsItemNotFound = ko.computed(function() {
            return self.SearchItemResults().length < 1;
        });


        self.ConversionRateChange = function() {

            if ((self.CurrentRecipeDetail().Uom() != null && self.CurrentRecipeDetail().Uom() != '')
                && (self.CurrentRecipeDetail().MaterialUOM() != null && self.CurrentRecipeDetail().MaterialUOM() != '')
                && (self.CurrentRecipeDetail().Uom() != self.CurrentRecipeDetail().MaterialUOM())) {
                DAL.fnGetConversionRate(self.CurrentRecipeDetail().ItemNo(), self.CurrentRecipeDetail().MaterialUOM(), self.CurrentRecipeDetail().Uom(), function(result) {
                    if (result.status == "") {
                        if (result.record == 0) {
                            self.CurrentRecipeDetail().ConversionRate(1)
                        }
                        else {
                            self.CurrentRecipeDetail().ConversionRate(result.record);
                            console.log("Set Conv Rate result.record : " + result.record);
                        }

                    }
                    else {
                        self.CurrentRecipeDetail().ConversionRate(1);
                    }
                });

            } else {
                self.CurrentRecipeDetail().ConversionRate(1);
            }
        }

        /**endregion*/

        //self.LoadDataRecipeAndItem();
        self.loadData();

    };

    $("form").submit(function(e) {
        e.preventDefault();
    });
});