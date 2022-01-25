Number.prototype.formatMoney = function(c, d, t) {
    var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

$(document).ready(function() {
    LoadDataSource();

    $("#dialog-form").dialog({
        autoOpen: false,
        height: 520,
        width: 850,
        modal: true,
        buttons: {
            "Save": function(evt) {
                evt.preventDefault();
                var Quantity = 0;
                var AnyQuantity = 0;
                var valid = true;
                var PromoPrice = 0;
                var DiscPercent = 0;
                var DiscAmount = 0;

                /*validation form detail*/
                if ($("#txtQty").val() != "") {
                    Quantity = parseInt($("#txtQty").val());
                }

                if ($("#txtAnyQty").val() != "") {
                    AnyQuantity = parseInt($("#txtAnyQty").val());
                }

                if ($("#txtPromoPrice").val() != "") {
                    PromoPrice = parseFloat($("#txtPromoPrice").val());
                }

                if ($("#txtDiscPercent").val() != "") {
                    DiscPercent = parseFloat($("#txtDiscPercent").val());
                }

                if ($("#txtDiscAmount").val() != "") {
                    DiscAmount = parseFloat($("#txtDiscAmount").val());
                }

                if ($("#rbPromoPrice").is(":checked") && (PromoPrice <= 0 || PromoPrice == "NaN")) {
                    $("#lblWarningItemForm").html("Promo Price must be greater than 0.");
                    $("#lblWarningItemForm").show();
                    valid = false;
                } else if ($("#rbDiscPercent").is(":checked") && (DiscPercent <= 0 || DiscPercent == "NaN" || DiscPercent > 100)) {
                    $("#lblWarningItemForm").html("Disc (%) must be more than 0% and below 100%.");
                    $("#lblWarningItemForm").show();
                    valid = false;
                } else if ($("#rbDiscAmount").is(":checked") && (DiscAmount <= 0 || DiscAmount == "NaN")) {
                    $("#lblWarningItemForm").html("Disc Amount must be greater than 0.");
                    $("#lblWarningItemForm").show();
                    valid = false;
                }

                if (valid) {
                    if ($("#rbCategory").is(":checked")) {
                        if ($("#ListCategory").val() == "") {
                            $("#lblWarningItemForm").html("Please select one Category.");
                            $("#lblWarningItemForm").show();
                        } else if (AnyQuantity <= 0 || AnyQuantity == "NaN") {
                            $("#lblWarningItemForm").html("Any Quantity must be greater than 0.");
                            $("#lblWarningItemForm").show();
                        }
                        else {
                            $("[id*=tmpDetailId]").val($("#hdDetailID").val());
                            $("[id*=tmpCategoryName]").val($("#ListCategory").val());
                            $("[id*=tmpItemNo]").val("*");
                            $("[id*=tmpItemGroup]").val("");
                            $("[id*=tmpPromoCampaignDetID]").val($("#hdPromoCampaignDetID").val());

                            $("[id*=tmpQty]").val($("#txtQty").val());
                            $("[id*=tmpAnyQty]").val($("#txtAny").val());

                            $("[id*=tmpPromoPrice]").val($("#txtPromoPrice").val());
                            $("[id*=tmpDiscPercent]").val($("#txtDiscPercent").val());
                            $("[id*=tmpDiscAmount]").val($("#txtDiscAmount").val());
                            $("[id*=tmpRetailPrice]").val($("#txtRetailPrice").val());
                            $("[id*=tmpUnitPrice]").val($("#hdUnitPrice").val());

                            $(this).dialog("close");
                            $("[id*=btnAddDetails]").click();
                        }
                    } else if ($("#rbItemGroup").is(":checked")) {
                        if ($("#ListItemGroup").val() == "") {
                            $("#lblWarningItemForm").html("Please select one Item Group.");
                            $("#lblWarningItemForm").show();
                        } else if (AnyQuantity <= 0 || AnyQuantity == "NaN") {
                            $("#lblWarningItemForm").html("Any Quantity must be greater than 0.");
                            $("#lblWarningItemForm").show();
                        }
                        else {
                            $("[id*=tmpDetailId]").val($("#hdDetailID").val());
                            $("[id*=tmpItemGroup]").val($("#ListItemGroup").val());
                            $("[id*=tmpCategoryName]").val("");
                            /*$("[id*=tmpItemNo]").val($("#ListItem").val());*/
                            $("[id*=tmpItemNo]").val($("#hdSelectedItemNo").val());
                            $("[id*=tmpPromoCampaignDetID]").val($("#hdPromoCampaignDetID").val());

                            $("[id*=tmpQty]").val($("#txtQty").val());
                            $("[id*=tmpAnyQty]").val($("#txtAny").val());

                            $("[id*=tmpPromoPrice]").val($("#txtPromoPrice").val());
                            $("[id*=tmpDiscPercent]").val($("#txtDiscPercent").val());
                            $("[id*=tmpDiscAmount]").val($("#txtDiscAmount").val());
                            $("[id*=tmpRetailPrice]").val($("#txtRetailPrice").val());
                            $("[id*=tmpUnitPrice]").val($("#hdUnitPrice").val());

                            $(this).dialog("close");
                            $("[id*=btnAddDetails]").click();
                        }
                    }
                    else {
                        if ($("#hdSelectedItemNo").val() == "") {
                            $("#lblWarningItemForm").html("Please select one Product.");
                            $("#lblWarningItemForm").show();
                        } else if (Quantity <= 0 || Quantity == "NaN") {
                            $("#lblWarningItemForm").html("Quantity must be greater than 0.");
                            $("#lblWarningItemForm").show();
                        }
                        else {
                            $("[id*=tmpDetailId]").val($("#hdDetailID").val());
                            $("[id*=tmpCategoryName]").val("");
                            $("[id*=tmpItemNo]").val($("#hdSelectedItemNo").val());
                            $("[id*=tmpItemGroup]").val("");
                            $("[id*=tmpPromoCampaignDetID]").val($("#hdPromoCampaignDetID").val());

                            $("[id*=tmpQty]").val($("#txtQty").val());
                            $("[id*=tmpAnyQty]").val($("#txtAny").val());
                            $("[id*=tmpUnitPrice]").val($("#hdUnitPrice").val());

                            $("[id*=tmpPromoPrice]").val($("#txtPromoPrice").val());
                            $("[id*=tmpDiscPercent]").val($("#txtDiscPercent").val());
                            $("[id*=tmpDiscAmount]").val($("#txtDiscAmount").val());
                            $("[id*=tmpRetailPrice]").val($("#txtRetailPrice").val());

                            $(this).dialog("close");
                            $("[id*=btnAddDetails]").click();
                        }
                    }
                }
            },
            Cancel: function(evt) {
                evt.preventDefault();
                $(this).dialog("close");
            }
        },
        close: function(evt) {
            evt.preventDefault();
            ResetDialogFrom();
        }
    });

    $("#dialog-CopyPromoForm").dialog({
        autoOpen: false,
        height: 400,
        width: 600,
        modal: true,
        buttons: {
            "Save": function(evt) {
                if ($("#ListOutletFrom").val() == "" || $("#ListOutletTo").val() == "") {
                    $("#lblWarningCopyPromo").html("Please select the Outlet source and destination.");
                    $("#lblWarningCopyPromo").show();
                } else if ($("#ListOutletFrom").val() == $("#ListOutletTo").val()) {
                    $("#lblWarningCopyPromo").html("Can not copy promo on same place.");
                    $("#lblWarningCopyPromo").show();
                } else {
                    $("[id*=tmpOutletFrom]").val($("#ListOutletFrom").val());
                    $("[id*=tmpOutletTo]").val($("#ListOutletTo").val());
                    $(this).dialog("close");
                    $("[id*=btnCopyPromotion]").click();
                }
            },
            Cancel: function(evt) {
                evt.preventDefault();
                $(this).dialog("close");
            }
        },
        close: function(evt) {
            evt.preventDefault();
            ResetDialogCopyPromo();
        }
    });

    $("#dialog-searchform").dialog({
        autoOpen: false,
        height: 400,
        width: 600,
        modal: true,
        buttons: {
            Cancel: function(evt) {
                evt.preventDefault();
                $(this).dialog("close");
            }
        },
        close: function(evt) {
            evt.preventDefault();

        }
    });

    $("#btnAddItem").click(function(evt) {
        evt.preventDefault();
        $("#dialog-form").dialog("open");
        $("#hdDetailID").val("0");
        $("[id*=tmpIsEdit]").val("0");
    });

    $("#btnCopyPromo").click(function(evt) {
        evt.preventDefault();
        $("#dialog-CopyPromoForm").dialog("open");
    });

    $("[action=edit_detail]").click(function(e) {
        $("[id*=tmpIsEdit]").val("1");
        var data = $(this).attr("data");
        var it = data.split("|");
        console.log(it);

        $("#hdDetailID").val(it[10]);
        $("#hdPromoCampaignDetID").val(it[0]);

        var rbItem = $("[name=rbItem]");
        for (var i = 0; i < rbItem.length; i++) {
            $(rbItem[i]).prop("checked", false);
        }

        /*$("#ListItem").val("").trigger("liszt:updated");*/
        $("#ListCategory").val("").trigger("liszt:updated");
        $("#ListItemGroup").val("").trigger("liszt:updated");

        $("#rbPromoPrice").prop("checked", true);


        if (it[1] === "" && it[2] !== "*" && it[3] !== "") {
            $("#rbProduct").prop("checked", true);
            /*$("#ListItem").val(it[2]).trigger("liszt:updated");*/

            $("#hdSelectedItemNo").val(it[2]);

            ajaxPost({
                url: app.setting.baseUrl + "API/Promo/GetItemByItemNo.ashx?ItemNo=" + it[2],
                success: function(result) {
                    $("#hdSelectedItemName").val(result.ItemName);
                    $("#txtSelectedItem").val(result.ItemNo + " - " + result.ItemName);
                }
            });

            ajaxPost({
                url: app.setting.baseUrl + "API/Promo/GetRetailPriceItem.ashx",
                data:
            {
                ItemNo: $("#hdSelectedItemNo").val(),
                PromoCampaignHdrID: it[9]
            },
                type: "GET",
                success: function(result) {
                    if (result.Status == true) {
                        $("#hdUnitPrice").val(result.Data);
                        if (result.Message != "") {
                            $("#lblWarningItemForm").html(result.Message);
                            $("#lblWarningItemForm").show();
                        }
                        else {
                            $("#lblWarningItemForm").html("");
                            $("#lblWarningItemForm").hide();
                        }
                        CountPrice();
                    } else {
                        $("#lblWarningItemForm").html("Item doesn't found.");
                        $("#lblWarningItemForm").show();
                    }
                }
            });
        } else if (it[1] === "" && it[2] === "*" && it[3] !== "") {
            $("#rbCategory").prop("checked", true);
            $("#ListCategory").val(it[3]).trigger("liszt:updated");

            ajaxPost({
                url: app.setting.baseUrl + "API/Promo/GetRetailPriceItem.ashx",
                data:
            {
                ItemNo: $("#ListCategory").val(),
                PromoCampaignHdrID: it[9]
            },
                type: "GET",
                success: function(result) {
                    if (result.Status == true) {
                        $("#hdUnitPrice").val(result.Data);
                        if (result.Message != "") {
                            $("#lblWarningItemForm").html(result.Message);
                            $("#lblWarningItemForm").show();
                        }
                        else {
                            $("#lblWarningItemForm").html("");
                            $("#lblWarningItemForm").hide();
                        }
                        CountPrice();
                    } else {
                        $("#lblWarningItemForm").html("Item doesn't found.");
                        $("#lblWarningItemForm").show();
                    }
                }
            });
        } else if (it[1] !== "" && it[2] === "" && it[3] === "") {
            $("#rbItemGroup").prop("checked", true);
            $("#ListItemGroup").val(it[1]).trigger("liszt:updated");

            ajaxPost({
                url: app.setting.baseUrl + "API/Promo/GetRetailPriceItem.ashx",
                data:
            {
                ItemNo: $("#ListItemGroup").val(),
                PromoCampaignHdrID: it[9]
            },
                type: "GET",
                success: function(result) {
                    if (result.Status == true) {
                        $("#hdUnitPrice").val(result.Data);
                        if (result.Message != "") {
                            $("#lblWarningItemForm").html(result.Message);
                            $("#lblWarningItemForm").show();
                        }
                        else {
                            $("#lblWarningItemForm").html("");
                            $("#lblWarningItemForm").hide();
                        }
                        CountPrice();
                    } else {
                        $("#lblWarningItemForm").html("Item doesn't found.");
                        $("#lblWarningItemForm").show();
                    }
                }
            });
        }

        $("#txtQty").val(it[4]);

        if ((it[5] | 0) > 0)
            $("#txtAny").val(it[5]);

        if ((it[6] | 0) == 0 && (it[7] | 0) == 0 && (it[8] | 0) >= 0) {
            $("#rbPromoPrice").prop("checked", true);
            $("#txtPromoPrice").val(it[8]);
        } else if ((it[6] | 0) > 0 && (it[7] | 0) == 0 && (it[8] | 0) == 0) {
            $("#rbDiscPercent").prop("checked", true);
            $("#txtDiscPercent").val(it[6]);
        } else if ((it[6] | 0) == 0 && (it[7] | 0) > 0 && (it[8] | 0) == 0) {
            $("#rbDiscAmount").prop("checked", true);
            $("#txtDiscAmount").val(it[7]);
        }



        $("#dialog-form").dialog("open");
    });

    $("#btnScan").click(function() {
        GetItemByBarcode();
    });

    $("#ListItem").change(function() {
        var promoCampaignHdrID = 0;
        if ($("[id*=hdPromoCampaignHdrID]").val() != "") {
            promoCampaignHdrID = parseInt($("[id*=hdPromoCampaignHdrID]").val());
        }

        ajaxPost({
            url: app.setting.baseUrl + "API/Promo/GetRetailPriceItem.ashx",
            data:
            {
                ItemNo: $("#ListItem").val(),
                PromoCampaignHdrID: promoCampaignHdrID
            },
            type: "GET",
            success: function(result) {
                if (result.Status == true) {
                    $("#rbProduct").prop("checked", true);
                    $("#hdUnitPrice").val(result.Data);
                    if (result.Message != "") {
                        $("#lblWarningItemForm").html(result.Message);
                        $("#lblWarningItemForm").show();
                    }
                    else {
                        $("#lblWarningItemForm").html("");
                        $("#lblWarningItemForm").hide();
                    }
                    CountPrice();
                } else {
                    $("#lblWarningItemForm").html("Item doesn't found.");
                    $("#lblWarningItemForm").show();
                }
            }
        });
    });

    $("#ListCategory").change(function() {
        var promoCampaignHdrID = 0;
        if ($("[id*=hdPromoCampaignHdrID]").val() != "") {
            promoCampaignHdrID = parseInt($("[id*=hdPromoCampaignHdrID]").val());
        }
        ajaxPost({
            url: app.setting.baseUrl + "API/Promo/GetMaxPriceItemCategory.ashx",
            data:
            {
                CategoryName: $("#ListCategory").val(),
                PromoCampaignHdrID: promoCampaignHdrID
            },
            type: "GET",
            success: function(result) {
                if (result.Status == true) {
                    $("#rbCategory").prop("checked", true);
                    $("#hdUnitPrice").val(result.Data);
                    if (result.Message != "") {
                        $("#lblWarningItemForm").html(result.Message);
                        $("#lblWarningItemForm").show();
                    }
                    else {
                        $("#lblWarningItemForm").html("");
                        $("#lblWarningItemForm").hide();
                    }
                    CountPrice();
                } else {
                    $("#lblWarningItemForm").html("Category doesn't found.");
                    $("#lblWarningItemForm").show();
                }
            }
        });
    });

    $("#ListItemGroup").change(function() {
        var promoCampaignHdrID = 0;
        if ($("[id*=hdPromoCampaignHdrID]").val() != "") {
            promoCampaignHdrID = parseInt($("[id*=hdPromoCampaignHdrID]").val());
        }
        ajaxPost({
            url: app.setting.baseUrl + "API/Promo/GetTotalPriceItemGroup.ashx",
            data:
            {
                ItemGroupId: $("#ListItemGroup").val(),
                PromoCampaignHdrID: promoCampaignHdrID
            },
            type: "GET",
            success: function(result) {
                if (result.Status == true) {
                    $("#rbItemGroup").prop("checked", true);
                    $("#hdUnitPrice").val(result.Data);
                    if (result.Message != "") {
                        $("#lblWarningItemForm").html(result.Message);
                        $("#lblWarningItemForm").show();
                    }
                    else {
                        $("#lblWarningItemForm").html("");
                        $("#lblWarningItemForm").hide();
                    }
                    CountPrice();
                } else {
                    $("#lblWarningItemForm").html("Item Group doesn't found.");
                    $("#lblWarningItemForm").show();
                }
            }
        });
    });

    $("#txtQty").change(function() {
        CountPrice();
    });

    $("#txtAny").change(function() {
        CountPrice();
    });

    $("#txtPromoPrice").change(function() {
        $("#rbPromoPrice").prop("checked", true);
        CountPrice();
    });

    $("#txtDiscPercent").change(function() {
        $("#rbDiscPercent").prop("checked", true);
        CountPrice();
    });

    $("#txtDiscAmount").change(function() {
        $("#rbDiscAmount").prop("checked", true);
        CountPrice();
    });

    $("input:radio[name='rbPrice']").change(function() {
        CountPrice();
    });

    $("#btnSearchItem").click(function(e) {
        e.preventDefault();
        $("#tbodyResult tr").remove();
        $("#dialog-searchform").dialog("open");
        SearchItem();
    });

    $("#btnDialogSearch").click(function() {
        $("#tbodyResult tr").remove();
        $("#SavedFilter").val($("#txtDialogSearch").val());
        SearchItem();
    });

    $("#btnLoadMore").click(function() {
        SearchItem();
    });
});

function SearchItem(){
   var skip = $("#tbodyResult tr").length;
   var filter = $("#SavedFilter").val();

   ajaxPost({
       url: app.setting.baseUrl + "API/Lookup/ItemWithFilter.ashx?filter=" + filter +"&&skip=" + skip + '&&take=20',
       success: function(result) {
       for (var i = 0; i < result.records.length; i++) 
       {
           $("#tbodyResult").append("<tr><td>" + result.records[i].ItemNo + "</td><td>" + result.records[i].ItemName + "</td><td><input type='button' class='btn btn-success' value='ADD' onclick='javascript: SelectItem(\"" + result.records[i].ItemNo + "\")'></input></td></tr>");
       }

       if (result.totalRecords > 0) {
           $("#tbodyNotFound").hide();
           if (result.totalRecords > $("#tbodyResult tr").length) {
               $("#divShowLoadMore").show();
           }
           else {
               $("#divShowLoadMore").hide();
           }
       }
       else {
           $("#tbodyNotFound").show();
           $("#divShowLoadMore").hide();
       }
       }
   });
}

function SelectItem(itemno) {
    $("#dialog-searchform").dialog("close");
    $("#hdSelectedItemNo").val(itemno);
   
    ajaxPost({
        url: app.setting.baseUrl + "API/Promo/GetItemByItemNo.ashx?ItemNo=" + itemno,
        success: function(result) {
            $("#hdSelectedItemName").val(result.ItemName);
            $("#txtSelectedItem").val(itemno + " - " + result.ItemName);
        }
    });

    var promoCampaignHdrID = 0;
    if ($("[id*=hdPromoCampaignHdrID]").val() != "") {
        promoCampaignHdrID = parseInt($("[id*=hdPromoCampaignHdrID]").val());
    }

    ajaxPost({
        url: app.setting.baseUrl + "API/Promo/GetRetailPriceItem.ashx",
        data:
            {
                ItemNo: itemno,
                PromoCampaignHdrID: promoCampaignHdrID
            },
        type: "GET",
        success: function(result) {
            if (result.Status == true) {
                $("#rbProduct").prop("checked", true);
                $("#hdUnitPrice").val(result.Data);
                if (result.Message != "") {
                    $("#lblWarningItemForm").html(result.Message);
                    $("#lblWarningItemForm").show();
                }
                else {
                    $("#lblWarningItemForm").html("");
                    $("#lblWarningItemForm").hide();
                }
                CountPrice();
            } else {
                $("#lblWarningItemForm").html("Item doesn't found.");
                $("#lblWarningItemForm").show();
            }
        }
    });
}

function EditDetail() {
    $("#dialog-form").dialog("open");
}


function LoadDataSource() {
    ajaxPost({
        url: app.setting.baseUrl + "API/Lookup/CategoryName.ashx",
        success: function(result) {
            loadDropdownDataSource("ListCategory", result, false);
        }
    });

    /*ajaxPost({
        url: app.setting.baseUrl + 'API/Lookup/ItemAll.ashx',
        success: function(result) {
            loadDropdownDataSource('ListItem', result, false);
        }
    });*/

    ajaxPost({
        url: app.setting.baseUrl + "API/Lookup/ItemGroup.ashx",
        success: function(result) {
            loadDropdownDataSource("ListItemGroup", result, false);
        }
    });

    ajaxPost({
        url: app.setting.baseUrl + "API/Lookup/Outlet.ashx",
        success: function(result) {
            loadDropdownDataSource("ListOutletFrom", result, false);
        }
    });

    ajaxPost({
        url: app.setting.baseUrl + "API/Lookup/Outlet.ashx",
        success: function(result) {
            loadDropdownDataSource("ListOutletTo", result, false);
        }
    });
    
    setTimeout(function() {
        $(".chosen-select").chosen();
    }, 2000);
}

function GetItemByBarcode() {
    console.log($("#txtBarcode").val());
    var promoCampaignHdrID = 0;
    if ($("[id*=hdPromoCampaignHdrID]").val() != "") {
        promoCampaignHdrID = parseInt($("[id*=hdPromoCampaignHdrID]").val());
    }
    ajaxPost({
        url: app.setting.baseUrl + "API/Promo/GetItemByBarcode.ashx",
        data:
        {
            Barcode: $("#txtBarcodeItem").val(),
            PromoCampaignHdrID: promoCampaignHdrID
        },
        type: "GET",
        success: function(result) {
            if (result.Status == true) {
                $("#rbProduct").prop("checked", true);
                $("#hdUnitPrice").val(result.Data.RetailPrice);
                /*$('#ListItem').val(result.Data.ItemNo);
                $('#ListItem').trigger("liszt:updated");*/

                $("#hdSelectedItemNo").val(result.Data.ItemNo);
                $("#hdSelectedItemName").val(result.Data.ItemName);
                $("#txtSelectedItem").val(result.Data.ItemNo + " - " + result.Data.ItemName);
                
                if (result.Message != "") {
                    $("#lblWarningItemForm").html(result.Message);
                    $("#lblWarningItemForm").show();
                }
                else {
                    $("#lblWarningItemForm").html("");
                    $("#lblWarningItemForm").hide();
                }
                CountPrice();
            } else {
                $("#lblWarningItemForm").html("Item doesn't found.");
                $("#lblWarningItemForm").show();
            }
        }
    });
}

function ResetDialogFrom() {
    $("#hdPromoCampaignDetID").val("");
    $("#txtBarcodeItem").val("");
    $("#ListCategory").val("");
    /*$("#ListItem").val("");*/
    $("#hdSelectedItemNo").val("");
    $("#hdSelectedItemName").val("");
    $("#txtSelectedItem").val("");
    $("#ListItemGroup").val("");
   
    $("#rbBarcode").prop("checked", true);
    $("#hdPromoCampaignDetID").val("0");
    $("#lblWarningItemForm").html("");
    $("#lblWarningItemForm").hide();

    $("#txtQty").val("");
    $("#txtAny").val("");
    $("#txtAny").addClass("input-disabled");

    $("#rbPromoPrice").prop("checked", true);
    $("#txtRetailPrice").val("");
    $("#txtPromoPrice").val("");
    $("#txtDiscPercent").val("");
    $("#txtDiscAmount").val("");
}

function ResetDialogCopyPromo() {
    $("#rbBarcode").prop("checked", true);
    $("#hdPromoCampaignDetID").val("0");
    $("#lblWarningCopyPromo").html("");
}

function CountPrice() {
    console.log("Count Price");
    $("#txtQty").prop("disabled", false);
    $("#txtQty").removeClass("input-disabled");
    $("#txtAny").prop("disabled", false);
    $("#txtAny").removeClass("input-disabled");
   
    if ($("#rbCategory").is(":checked") || $("#rbItemGroup").is(":checked")) {
        $("#txtQty").val("");
        $("#txtQty").prop("disabled", true);
        $("#txtQty").addClass("input-disabled");
    } else if ($("#rbProduct").is(":checked")) {
        $("#txtAny").val("");
        $("#txtAny").prop("disabled", true);
        $("#txtAny").addClass("input-disabled");
    }

    var retailPrice = 0;
    var promoPrice = 0;
    var qty = 0;
    var anyqty = 0;
    var unitPrice = 0;

    if ($("#txtQty").val() != "") {
        qty = parseFloat($("#txtQty").val());
    }
    
    if ($("#hdUnitPrice").val() != "") {
        unitPrice = parseFloat($("#hdUnitPrice").val());
    }

    if (qty != 0) {
        retailPrice = qty * unitPrice;
    } else {
        retailPrice = unitPrice;
    }

    $("#txtRetailPrice").val(retailPrice.formatMoney(2, ".", ","));
    
    if ($("#rbDiscPercent").is(":checked")) {
        var discPercent = 0;
        if ($("#txtDiscPercent").val() != "") {
            discPercent = parseFloat($("#txtDiscPercent").val().replace(",", ""));
            $("#txtDiscPercent").val(discPercent.formatMoney(2, ".", ","));
        }
        $("#txtDiscAmount").val("");
        $("#txtPromoPrice").val("");
    } else if ($("#rbDiscAmount").is(":checked")) {
        var discAmount = 0;
        if ($("#txtDiscAmount").val() != "") {
            discAmount = parseFloat($("#txtDiscAmount").val().replace(",", ""));
            $("#txtDiscAmount").val(discAmount.formatMoney(2, ".", ","));
        }
        $("#txtDiscPercent").val("");
        $("#txtPromoPrice").val("");
    }else{
        if ($("#txtPromoPrice").val() != "") {
            promoPrice = parseFloat($("#txtPromoPrice").val().replace(",", ""));
            $("#txtPromoPrice").val(promoPrice.formatMoney(2, ".", ","));
        }
        $("#txtDiscAmount").val("");
        $("#txtDiscPercent").val("");
    }
}

function jsDecimals(e) {
    var evt = (e) ? e : window.event;
    var key = (evt.keyCode) ? evt.keyCode : evt.which;
    if (key != null) {
        key = parseInt(key, 10);
        if ((key < 48 || key > 57) && (key < 96 || key > 105)) {
            if (!jsIsUserFriendlyChar(key, "Decimals")) { return false; }
        }
        else {
            if (evt.shiftKey) { return false; }
        }
    }
    return true;
}

// Function to check for user friendly keys //------------------------------------------
function jsIsUserFriendlyChar(val, step) {
    // Backspace, Tab, Enter, Insert, and Delete 
    if (val == 8 || val == 9 || val == 13 || val == 45 || val == 46) {
        return true;
    }
    // Ctrl, Alt, CapsLock, Home, End, and Arrows 

    if ((val > 16 && val < 21) || (val > 34 && val < 41)) { return true; }

    if (step == "Decimals") {
        if (val == 190 || val == 110) {
            //Check dot key code should be allowed 
            return true;
        }
    }
    //The rest
    return false;
}

function isMoneyKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        if (charCode == 44 || charCode == 46)
            return true
        else
            return false;
    }
    return true;
}

