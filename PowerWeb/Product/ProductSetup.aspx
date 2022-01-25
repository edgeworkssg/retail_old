<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="ProductSetup.aspx.cs" Inherits="PowerWeb.Product.ProductSetup" Title="<%$Resources:dictionary,Product Setup %>" %>

<asp:Content ID="panelProductSetup" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ScriptManager ID="ScriptManager1" runat="server">
    </ajax:ScriptManager>
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/DataTable/css/jquery.dataTables_themeroller.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/Vendors/FontAwesome/css/font-awesome.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/App_Themes/Applications/POSWeb.css") %>" />
    <div class="page">
        <div class="panel" id="panelFilterProduct">
            <input type="hidden" name="BaseUrl" id="BaseUrl" value="<%= ResolveUrl("~/") %>" />
            <div class="form-group">
                <label class="element-label" for="filterProductId">
                    Product ID</label>
                <div class="element-wrapper">
                    <input type="text" name="filterProductId" id="filterProductId" placeholder="Type Product ID here ..."
                        class="span4" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="filterProductName">
                    Product Name</label>
                <div class="element-wrapper">
                    <input type="text" name="filterProductName" id="filterProductName" placeholder="Type Product Name here ..."
                        class="span4" />
                </div>
            </div>
            <div class="form-group bottom">
                <label class="element-label" for="">
                    &nbsp;</label>
                <div class="element-wrapper">
                    <button class="button" id="btnFilter">
                        <i class="fa fa-search"></i>Search</button>
                    <button class="button" id="btnClearFilter">
                        <i class="fa fa-eraser"></i>Clear</button>
                </div>
            </div>
        </div>
        <div class="panel align-right crud-button-panel top" id="panelButtonGrid">
            <button class="button" id="btnAdd">
                <i class="fa fa-plus"></i>&nbsp;Add</button>
            <button class="button" id="btnExport">
                <i class="fa fa-file-excel-o"></i>&nbsp;Export</button>
        </div>
        <div class="panel align-right crud-button-panel top" id="panelButtonForm">
            <button class="button" id="btnSave">
                <i class="fa fa-save"></i>&nbsp;Save</button>
            <button class="button" id="btnCancel">
                <i class="fa fa-refresh"></i>&nbsp;Cancel</button>
        </div>
        <div class="panel boundary-top" id="panelGridProduct">
            <div class="grid-outer-wrapper">
                <table class="grid display" id="gridProduct">
                    <thead>
                        <th>
                            Action
                        </th>
                        <th>
                            Item No
                        </th>
                        <th>
                            Product Name
                        </th>
                        <th>
                            Category
                        </th>
                        <th>
                            Department
                        </th>
                        <th>
                            Retail Price
                        </th>
                        <th>
                            Factory Price
                        </th>
                        <th>
                            Inventory Item
                        </th>
                        <th>
                            Non Discountable
                        </th>
                        <th>
                            Barcode
                        </th>
                    </thead>
                </table>
            </div>
        </div>
        <div class="panel" id="panelButtonMode">
             <div class="form-group">
                <label class="element-label">
                </label>
                <div class="element-wrapper">
                    <button class="button" id="btnNormal">
                        Normal</button>
                    <button class="button" id="btnMatrix">
                        Matrix</button>
                </div>
            </div>
        </div>
        <div class="panel" id="panelUpdateProduct">
            <div class="form-group">
                <label class="element-label" for="ItemNo" id="lblItemNoNormal">
                    Item No</label>
                <div class="element-wrapper">
                    <input type="text" name="ItemNo" id="ItemNo" placeholder="Item No" class="span4"
                        data-parsley-required="false" maxlength="50" />
                    <input type="hidden" id="UserFlag1" value="false" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="Barcode">
                    Barcode</label>
                <div class="element-wrapper">
                    <input type="text" name="Barcode" id="Barcode" placeholder="Barcode" class="span4"
                        maxlength="100" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="ItemName" id="lblItemNameNormal">
                    Item Name</label>
                <div class="element-wrapper">
                    <input type="text" name="ItemName" id="ItemName" placeholder="Item Name" class="span4"
                        data-parsley-required="true" maxlength="600" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="CategoryName">
                    Category Name</label>
                <div class="element-wrapper">
                    <select name="CategoryName" id="CategoryName" placeholder="Category Name" class="span4"
                        data-parsley-required="true" maxlength="600">
                    </select>
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="FactoryPrice">
                    Factory Price</label>
                <div class="element-wrapper">
                    <input type="text" name="FactoryPrice" id="FactoryPrice" placeholder="Factory Price"
                        class="span-1" data-parsley-required="true" data-parsley-type="number" data-parsley-maxlength="8"
                        maxlength="8" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="RetailPrice">
                    Retail Price</label>
                <div class="element-wrapper">
                    <input type="text" name="RetailPrice" id="RetailPrice" placeholder="Retail Price"
                        class="span-1" data-parsley-required="true" data-parsley-type="number" data-parsely-maxlength="8"
                        maxlength="8" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="NonDiscountable">
                    Non Discountable</label>
                <div class="element-wrapper">
                    <input type="checkbox" value="True" name="IsNonDiscountable" id="NonDiscountable"
                        placeholder="Non Discountable" class="span4" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="GST">
                    GST</label>
                <div class="element-wrapper">
                    <select name="GST" id="GST" placeholder="GST" class="span4" data-parsley-required="true" />
                    </select>
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="IsCommission">
                    Give Commission</label>
                <div class="element-wrapper align-left">
                    <input type="checkbox" value="True" name="IsCommission" id="IsCommission" placeholder="Give Commission"
                        class="span4" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="PointRedeemable">
                    Point Redeemable</label>
                <div class="element-wrapper align-left">
                    <input type="checkbox" value="True" name="PointRedeemable" id="PointRedeemable" placeholder="Point Redeemable"
                        class="span4" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="ProductType">
                    Product Type</label>
                <div class="element-wrapper">
                    <select name="ProductType" id="ProductType" placeholder="ProductType" class="span4" />
                    </select>
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="PointsGet">
                    Points Get</label>
                <div class="element-wrapper">
                    <input type="text" name="PointsGet" id="PointsGet" placeholder="Points Get" class="span4"
                        data-parsley-type="number" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="TimesGet">
                    Times Get</label>
                <div class="element-wrapper">
                    <input type="text" name="TimesGet" id="TimesGet" placeholder="Times Get" class="span4"
                        data-parsley-type="number" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="BreakdownPrice">
                    Breakdown Price</label>
                <div class="element-wrapper">
                    <input type="text" name="BreakdownPrice" id="BreakdownPrice" placeholder="Breakdown Price"
                        class="span4" data-parsley-type="number" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="ItemDescription">
                    Item Description</label>
                <div class="element-wrapper">
                    <textarea name="ItemDescription" id="ItemDescription" placeholder="Item Description"
                        class="span4" maxlength="600" data-parsley-maxlength="600">
                    </textarea>
                </div>
            </div>
          </div>
        <div class="panel" id="panelAttributeNormal">
            <div class="form-group">
                <label class="element-label" for="Attributes1">
                    Attributes 1</label>
                <div class="element-wrapper">
                    <input type="text" name="Attributes1" id="Attributes1" placeholder="Attributes 1"
                        class="span4" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="">
                    Attributes 2</label>
                <div class="element-wrapper">
                    <input type="text" name="Attributes2" id="Attributes2" placeholder="Attributes 2"
                        class="span4" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="Attributes3">
                    Attributes 3</label>
                <div class="element-wrapper">
                    <input type="text" name="Attributes3" id="Attribute3" placeholder="Attributes 3"
                        class="span4" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="Attribute4">
                    Attributes 4</label>
                <div class="element-wrapper">
                    <input type="text" name="Attributes4" id="Attributes4" placeholder="Attributes 4"
                        class="span4" />
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="Attribute 5">
                    Attributes 5</label>
                <div class="element-wrapper">
                    <input type="text" name="Attributes5" id="Attributes5" placeholder="Attributes 5"
                        class="span4" />
                </div>
            </div>
        </div>
        <div class="panel" id="panelAttributeMatrix">
            <div class="form-group">
                <label class="element-label" for="Attributes3">
                    Attributes 3</label>
                <div class="element-wrapper">
                    <div id="containerAtt3" class="container">
                    </div>
                   <div style="margin:5px 0 5px  150px;">
                        <input id="txtAddAtt3"  type="text" /> <button class="button" id="btnAddAtt3"> Add to List</button>
                    </div>
                </div>
                
            </div>
            <div class="form-group">
                <label class="element-label" for="Attributes4">
                    Attributes 4</label>
                <div class="element-wrapper">
                    <div id="containerAtt4" class="container">
                    </div>
                   <div style="margin:5px 0 5px  150px;">
                        <input id="txtAddAtt4"  type="text" /> <button class="button" id="btnAddAtt4"> Add to List</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel" id="panelUpdateProductFooter">
            <div class="form-group">
                <label class="element-label" for="Remark">
                    Remark</label>
                <div class="element-wrapper">
                    <textarea type="text" name="Remark" id="Remark" placeholder="Remark"
                        class="span4">
                    </textarea>
                </div>
            </div>
            <div class="form-group">
                <label class="element-label" for="">
                    Supplier</label>
                <div class="element-wrapper">
                    Supplier ID | Supplier Name
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function() {
            app.setting.baseUrl = $('#BaseUrl').val();
        });
    </script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/DataTable/js/jquery.dataTables.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Vendors/Parsley/js/parsley.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/lib.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Libs/setting.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/Applications/Product/product-setup.js") %>"></script>

</asp:Content>
