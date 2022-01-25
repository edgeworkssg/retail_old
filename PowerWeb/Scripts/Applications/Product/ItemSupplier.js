$(document).ready(function() {

    $("#dialog_AddNewNIP").dialog({
        autoOpen: false,
        height: 400,
        width: 600,
        modal: true,
        buttons: {
            "Save": function(evt) {
                evt.preventDefault();
                if ($("#txtItemNoNIP").val() == ''
                    || $("#txtItemNameNIP").val() == ''
                    || $("#txtBarcodeNIP").val() == ''
                    || $("#txtCostPriceNIP").val() == '' || !$.isNumeric($("#txtCostPriceNIP").val())
                    || $("#txtRetailPriceNIP").val() == '' || !$.isNumeric($("#txtRetailPriceNIP").val())
                    || $("#txtSizeConvNIP").val() == '' || !$.isNumeric($("#txtSizeConvNIP").val())
                    || $("#txtUOMNIP").val() == '') {
                            $("#lblWarningItemForm").html("Please fill form properly");
                            $("#lblWarningItemForm").show();
                }
                else {
                    ajaxPost({
                        url: app.setting.baseUrl + "API/Product/ProductSetup/CreateNIP.ashx",
                        data:
                        {
                            ItemNo: $("#txtItemNoNIP").val(),
                            ItemName: $("#txtItemNameNIP").val(),
                            Barcode: $("#txtBarcodeNIP").val(),
                            FactoryPrice: $("#txtCostPriceNIP").val(),
                            RetailPrice: $("#txtRetailPriceNIP").val(),
                            UOM: $("#txtUOMNIP").val(),
                            SizeConvType: $("#SizeConvType").val(),
                            SizeConv: $("#txtSizeConvNIP").val(),
                            DeductedItemNo: $("[id*=MainItemNo]").val()
                        },
                        type: "POST",
                        success: function(result) {
                            if (result.Status == true) {
                                $("#lblWarningItemForm").html("Product has been saved.");
                                $("#lblWarningItemForm").show();
                                $("#dialog_AddNewNIP").dialog("close");
                                $("[id*=btnAddNIP]").click();
                            } else {
                                $("#lblWarningItemForm").html(result.Message);
                                $("#lblWarningItemForm").show();
                            }
                        }
                    });
                }
            },
            Cancel: function(evt) {
                evt.preventDefault();
                $(this).dialog("close");
            }
        },
        close: function(evt) {
            evt.preventDefault();
            ResetDialogAddNewItem();
        }
    });

    $("#btnAddItem").click(function(evt) {
        evt.preventDefault();
        if ($("[id*=MainItemNo]").val() != '') {
            $("#dialog_AddNewNIP").dialog("open");
            $("#txtItemNameNIP").val($("[id*=MainItemName]").val());
            $("#txtBarcodeNIP").val($("[id*=MainItemBarcode]").val());
        }
    });

});

function CreateNIP() {
    var FactoryPrice = 0;
    var FactoryPrice = 0;
    var SizeConv = 0;
    
    if ($("#txtItemNoNIP").val() == '' 
        || $("#txtItemNameNIP").val() == ''
        || $("#txtBarcodeNIP").val() == ''
        || $("#txtCostPriceNIP").val() == '' || !$.isNumeric($("#txtCostPriceNIP").val())
        || $("#txtRetailPriceNIP").val() == '' || !$.isNumeric($("#txtRetailPriceNIP").val())
        || $("#txtSizeConvNIP").val() == '' || !$.isNumeric($("#txtSizeConvNIP").val())
        || $("#txtUOMNIP").val() == '') 
        {
            $("#lblWarningItemForm").html("Please fill form properly");
            $("#lblWarningItemForm").show();  
        }
        else
        {
            ajaxPost({
                url: app.setting.baseUrl + "API/Product/ProductSetup/CreateNIP.ashx",
                data:
                    {
                         ItemNo: $("#txtItemNoNIP").val(),
                         ItemName: $("#txtItemNameNIP").val(),
                         Barcode: $("#txtBarcodeNIP").val(),
                         FactoryPrice: $("#txtCostPriceNIP").val(),
                         RetailPrice: $("#txtRetailPriceNIP").val(),
                         UOM: $("#txtUOMNIP").val(),
                         SizeConvType: $("#SizeConvType").val(),
                         SizeConv: $("#txtSizeConvNIP").val(),
                         DeductedItemNo: $("[id*=MainItemNo]").val()
                    },
                type: "POST",
                success: function(result) {
                    if (result.Status == true) {
                        $("#lblWarningItemForm").html("Product has been saved.");
                        $("#lblWarningItemForm").show();
                    } else {
                        $("#lblWarningItemForm").html(result.Message);
                        $("#lblWarningItemForm").show();
                    }
                }
            });  
    }
}

function ResetDialogAddNewItem() {
    $("#txtItemNoNIP").val("");
    $("#txtItemNameNIP").val("");
    $("#txtBarcodeNIP").val("");
    $("#txtUOMNIP").val("");
    $("#txtSizeConvNIP").val("");
    $("#txtRetailPriceNIP").val("0");
    $("#txtCostPriceNIP").val("0");
}