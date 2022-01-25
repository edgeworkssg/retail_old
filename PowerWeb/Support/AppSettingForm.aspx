<%@ Page Language="C#" MasterPageFile="~/PowerPOSMst.master" AutoEventWireup="true"
    CodeBehind="AppSettingForm.aspx.cs" Inherits="PowerWeb.Support.AppSettingForm"
    Title="Configuration" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Head" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .accordionHeader
        {
            font-family: Arial,Helvetica,sans-serif;
            color: #FFF;
            font-size: 16px;
            width: 100%;
            padding: 3px 3px 3px 5px;
            height: 23px;
            background: #51A2E4;
            border: solid 1px #87B3D1;
            cursor: pointer;
            background: #4d9ee0; /* Old browsers */
            background: -moz-linear-gradient(top,  #4d9ee0 0%, #69b8f3 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#4d9ee0), color-stop(100%,#69b8f3)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top,  #4d9ee0 0%,#69b8f3 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top,  #4d9ee0 0%,#69b8f3 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top,  #4d9ee0 0%,#69b8f3 100%); /* IE10+ */
            background: linear-gradient(to bottom,  #4d9ee0 0%,#69b8f3 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#4d9ee0', endColorstr='#69b8f3',GradientType=0 ); /* IE6-9 */
        }
        .accordionHeaderSelected
        {
            font-family: Arial,Helvetica,sans-serif;
            color: #FFF;
            font-size: 16px;
            width: 100%;
            padding: 3px 3px 3px 5px;
            height: 23px;
            background: #4A9BDD;
            border: solid 1px #357FB7;
            cursor: pointer;
        }
        .accordionContent
        {
            width: 100%;
            background: #E1E1E1;
            border: solid 1px #D1D1D1;
            padding: 5px;
            width: 788px !important;
            overflow: hidden !important;
        }
        .ctl00_ContentPlaceHolder1_MyAccordion
        {
            overflow: hidden !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="classname" OnClick="btnSave_Click" />
    <ajax:ScriptManager ID="ScriptManager1" runat="server" />
    <cc1:Accordion ID="MyAccordion" runat="Server" SelectedIndex="0" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="Limit" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40"
        RequireOpenedPane="false" SuppressHeaderPostbacks="true" Width="800px">
        <Panes>
            <cc1:AccordionPane ID="pane1" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    Product Listing</Header>
                <Content>
                    <asp:CheckBox ID="DisplayItemOpenPrice" runat="server" Text="Display Item Type : Open Price"
                        Checked="true" /><br />
                    <asp:CheckBox ID="DisplayItemService" runat="server" Text="Display Item Type : Services"
                        Checked="true" /><br />
                    <asp:CheckBox ID="DisplayItemPointPackage" runat="server" Text="Display Item Type : Point & Package"
                        Checked="true" /><br />
                    <asp:CheckBox ID="DisplayItemCourse" runat="server" Text="Display Item Type : Course"
                        Checked="true" /><br />
                    <asp:CheckBox ID="DisplayGiveCommission" runat="server" Text="Display Give Commission"
                        Checked="true" /><br />
                    <asp:CheckBox ID="DisplayIsNonDiscountable" runat="server" Text="Display Is Non Discountable"
                        Checked="true" /><br />
                    <asp:CheckBox ID="DisplayPointRedeemable" runat="server" Text="Display Point Redeemable"
                        Checked="true" /><br />
                    <asp:CheckBox ID="DisplaySupplier" runat="server" Text="Display Supplier" Checked="true" /><br />
                    <asp:CheckBox ID="DisplayUOM" runat="server" Text="Display UOM" Checked="false" /><br />
                    <asp:Literal ID="Literal2" runat="server" Text="Cost Price Text : " />
                    <asp:TextBox ID="CostPriceText" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal3" runat="server" Text="Retail Price Text : " />
                    <asp:TextBox ID="RetailPriceText" runat="server" Text="" /><br />
                    <asp:CheckBox ID="DisplayPrice1" runat="server" Text="Display Price 1" Checked="true" /><br />
                    <asp:Literal ID="Literal18" runat="server" Text="Discount P1 Name : " />
                    <asp:TextBox ID="txtDiscountP1Name" runat="server" Text="" /><br />
                    <asp:CheckBox ID="DisplayPrice2" runat="server" Text="Display Price 2" Checked="true" /><br />
                    <asp:Literal ID="Literal19" runat="server" Text="Discount P2 Name : " />
                    <asp:TextBox ID="txtDiscountP2Name" runat="server" Text="" /><br />
                    <asp:CheckBox ID="DisplayPrice3" runat="server" Text="Display Price 3" Checked="true" /><br />
                    <asp:Literal ID="Literal20" runat="server" Text="Discount P3 Name : " />
                    <asp:TextBox ID="txtDiscountP3Name" runat="server" Text="" /><br />
                    <asp:CheckBox ID="DisplayPrice4" runat="server" Text="Display Price 4" Checked="true" /><br />
                    <asp:Literal ID="Literal21" runat="server" Text="Discount P4 Name : " />
                    <asp:TextBox ID="txtDiscountP4Name" runat="server" Text="" /><br />
                    <asp:CheckBox ID="DisplayPrice5" runat="server" Text="Display Price 5" Checked="true" /><br />
                    <asp:Literal ID="Literal22" runat="server" Text="Discount P5 Name : " />
                    <asp:TextBox ID="txtDiscountP5Name" runat="server" Text="" /><br />
                    <asp:CheckBox ID="Item_AllowPreOrder" runat="server" Text="Allow Pre Order" /><br />
                    <asp:CheckBox ID="Item_ShowVendorDeliveryOption" runat="server" Text="Show Vendor Delivery Option" /><br />
                    <asp:Literal ID="Literal23" runat="server" Text="Price Num Digit : " />
                    <asp:TextBox ID="txtNumDigit" runat="server" Text="" /><br />
                    <asp:Literal ID="ltrlItemExpiry" runat="server" Text="Item Expiry Period Column : " />
                    <asp:TextBox ID="txtItemExpiryPeriodColumn" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal24" runat="server" Text="Product Setup Page Size : " />
                    <asp:TextBox ID="txtProductSetupPageSize" runat="server" Text="" /><br />
                    <asp:CheckBox ID="UseSelectableItemNameFilter" runat="server" Text="Use Selectable Item Name Filter"
                        Checked="true" /><br />
                    <asp:CheckBox ID="UseSelectableAttributesFilter" runat="server" Text="Use Selectable Attributes Filter"
                        Checked="true" /><br />
                    <asp:CheckBox ID="HideDeletedItem" runat="server" Text="Hide Deleted Item on Product"
                        Checked="false" /><br />
                     <asp:Literal ID="Literal32" runat="server" Text="Default GST Setting : " />
                     <asp:DropDownList ID="ddDefaultGSTSetting" runat="server" Height="20px" Width="130px">
                        <asp:ListItem>Exclusive GST</asp:ListItem>
                        <asp:ListItem>Inclusive GST</asp:ListItem>
                        <asp:ListItem>Non GST</asp:ListItem>
                    </asp:DropDownList><br />
                     <asp:Literal ID="Literal33" runat="server" Text="Attributes1 : " />
                    <asp:TextBox ID="txtAttributes1" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal34" runat="server" Text="Attributes2 : " />
                    <asp:TextBox ID="txtAttributes2" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal35" runat="server" Text="Attributes3 : " />
                    <asp:TextBox ID="txtAttributes3" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal36" runat="server" Text="Attributes4 : " />
                    <asp:TextBox ID="txtAttributes4" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal37" runat="server" Text="Attributes5 : " />
                    <asp:TextBox ID="txtAttributes5" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal38" runat="server" Text="Attributes6 : " />
                    <asp:TextBox ID="txtAttributes6" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal39" runat="server" Text="Attributes7 : " />
                    <asp:TextBox ID="txtAttributes7" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal40" runat="server" Text="Attributes8 : " />
                    <asp:TextBox ID="txtAttributes8" runat="server" Text="" /><br />
                    <asp:CheckBox ID="ShowWarningWhenSellingPriceLessThanCostPrice" runat="server" Text="Show Warning When Selling Price Less Than Cost Price" /><br /><br />
                    <asp:CheckBox ID="chAutoGenerateBarcode" runat="server" Text="Auto Generate Barcode" /><br />
                    <div style="margin-left:17px;">
                        <asp:RadioButton ID="rbAutoGenerateBarcodeA" runat="server" GroupName="AutoGenerateBarcode" />&nbsp;<asp:Literal ID="Literal72" runat="server" Text="From the Setting" />
                        <div style="margin-left:25px;">
                            <asp:Literal ID="Literal61" runat="server" Text="Last Barcode Generated : " />
                            <asp:TextBox ID="LastBarcodeGenerated" runat="server" Text="" /><br />
                            <asp:Literal ID="Literal62" runat="server" Text="Barcode Prefix : " />
                            <asp:TextBox ID="BarcodePrefix" runat="server" Text="" /><br />
                        </div>
                        <asp:RadioButton ID="rbAutoGenerateBarcodeB" runat="server" GroupName="AutoGenerateBarcode" />&nbsp;<asp:Literal ID="Literal74" runat="server" Text="From the Category Prefix" />
                        <div style="margin-left:25px;">
                            <asp:Literal ID="Literal73" runat="server" Text="Running Number No of Digit : " />
                            <asp:TextBox ID="txtCategoryRunningNumberNoofDigit" runat="server" Text="" />
                        </div>
                    </div>
                    <asp:CheckBox ID="Item_DisplayAutoCaptureWeight" runat="server" Text="Display Auto Capture Weight" /><br />
                    <asp:CheckBox ID="Item_DisplayNonInventoryProduct" runat="server" Text="Display Non Inventory Product" Checked="false" /><br />
                    <asp:CheckBox ID="Item_DisplayMinimumSellingPrice" runat="server" Text="Display Minimum Selling Price" Checked="false" /><br />
                    <asp:CheckBox ID="Item_DisplayApplyFuturePrice" runat="server" Text="Display Apply Future Price" Checked="false" /><br />
                    <asp:CheckBox ID="IsEditCategory_ProductOutletSetup" runat="server" Text="Can Edit Category at Product Setup When Choose Outlet" /><br />
                    <asp:CheckBox ID="IsEditCostPrice_ProductOutletSetup" runat="server" Text="Can Edit Cost Price at Product Setup When Choose Outlet" /><br />
                    <asp:CheckBox ID="AllowOverrideItemNameOutlet" runat="server" Text="Allow Override Item Name for Outlet" /><br />
                    <asp:CheckBox ID="UseCustomerPricing" runat="server" Text="Use Customer Pricing" /><br />
                    <%--<asp:CheckBox ID="AddProductOneOutletOnly" runat="server" Text="To Add Prodiuct only in One Outlet" /><br />--%>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane20" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    General Setting</Header>
                <Content>
                    <asp:Literal ID="Literal71" runat="server" Text="POS Type: " />
                    <asp:DropDownList ID="ddlPOSType" runat="server" Height="20px" Width="130px">
                        <asp:ListItem>Retail</asp:ListItem>
                        <asp:ListItem>Wholesale</asp:ListItem>
                        <asp:ListItem>Beauty</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:Literal ID="ltrCompanyID" runat="server" Text="Company ID : " />
                    <asp:TextBox ID="CompanyID" runat="server" Text="" /><br />
                    <asp:Literal ID="ltrCustomerMasterURL" runat="server" Text="Mobord Customer API URL : " />
                    <asp:TextBox ID="CustomerMasterURL" runat="server" Text="" /><br />
                    <asp:Literal ID="ltrAPICallerID" runat="server" Text="API Caller ID : " />
                    <asp:TextBox ID="APICallerID" runat="server" Text="" /><br />
                    <asp:Literal ID="ltrAPIPrivateKey" runat="server" Text="API Private Key : " />
                    <asp:TextBox ID="APIPrivateKey" runat="server" Text="" /><br />                    
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane2" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Supplier Listing</Header>
                <Content>
                    <asp:CheckBox ID="DisplayCurrencyOnSupplier" runat="server" Text="Display Currency"
                        Checked="true" /><br />
                    <asp:CheckBox ID="DisplayGSTOnSupplier" runat="server" Text="Display GST" Checked="true" /><br />
                    <asp:CheckBox ID="DisplayMinimumOrderOnSupplier" runat="server" Text="Display Minimum Order"
                        Checked="true" /><br />
                    <asp:CheckBox ID="DisplayDeliveryChargeOnSupplier" runat="server" Text="Display Delivery Charge"
                        Checked="true" /><br />
                    <asp:CheckBox ID="DisplayPaymentTermOnSupplier" runat="server" Text="Display Payment Term"
                        Checked="true" /><br />
                    <asp:Literal ID="Literal7" runat="server" Text="Default Currency : " />
                    <asp:TextBox ID="DefaultCurrency" runat="server" Text="SGD" /><br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="pane2" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    Item Supplier Listing</Header>
                <Content>
                    <asp:CheckBox Visible="true" ID="DisplayCurrencyOnItemSupplierMap" runat="server"
                        Text="Display Currency" Checked="true" /><br />
                    <asp:CheckBox Visible="false" ID="DisplayGSTOnItemSupplierMap" runat="server" Text="Display GST"
                        Checked="true" />
                    <asp:Literal ID="Literal1" runat="server" Text="Max Packing Size : " />
                    <asp:TextBox ID="MaxPackingSizeOnItemSupplierMap" runat="server" Text="0" /><br />
                    <asp:Literal Visible="false" ID="Literal6" runat="server" Text="Available Currency : " />
                    <asp:TextBox Visible="false" ID="AvailableCurrency" runat="server" Text="SGD,USD,EUR,IDR,JPY,AUD" />
                    <asp:Literal Visible="false" ID="Literal8" runat="server" Text=" (split by ',')" /><br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane1" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                Visible="false">
                <Header>
                    Purchase Order</Header>
                <Content>
                    <asp:Literal ID="Literal4" runat="server" Text="PO Role : " />
                    <asp:TextBox ID="PORole" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal5" runat="server" Text="PO Company : " />
                    <asp:TextBox ID="POCompany" runat="server" Text="" /><br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane3" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                Visible="true">
                <Header>
                    Inventory</Header>
                <Content>
                    <asp:CheckBox ID="AllowStockTransferGoThroughEvenStockIsZero" runat="server" Text="Allow Stock Transfer Go Through Even Stock Is Zero" />
                    <br />
                    <asp:CheckBox ID="AllowDeductInvQtyNotSufficient" runat="server" Text="Allow Deduct Inventory Although Stock Balance Quantity Not Sufficient (Goods Ordering and Stock Return)" />
                    <br />
                    <asp:CheckBox ID="CalculateAvgCostatInventoryLocationLevel" runat="server" Text="Calculate Avg Cost at Inventory Location Level" />
                    <br />
                    <asp:CheckBox ID="CalculateAvgCostatInventoryLocationGroupLevel" runat="server" Text="Calculate Avg Cost at Inventory Location Group Level" />
                    <br />                    
                    <asp:CheckBox ID="ShowBatchNoStockTake" runat="server" Text="Show Batch No on Adjust Stock Take" />
                    <br />
                    <asp:CheckBox ID="ShowParValueStockTake" runat="server" Text="Show Par Value on Adjust Stock Take" />
                    <br />
                    <asp:Button ID="btnGenerateItemSummary" Visible="false" runat="server" Text="Generate Item Summary" OnClick="btnGenerateItemSummary_Click" />
                    <br />
                    <asp:CheckBox ID="StockReturnNoAffectCOGS" runat="server" Text="Stock Return Not Affect COGS" />
                    <br />
                    <asp:CheckBox ID="IsAutoStockIn" runat="server" Text="Auto Stock In at Order Approval" />
                    <br />
                    <asp:CheckBox ID="EditableAutoStockIn" runat="server" Text="Allow Edit Auto Stock In at Order Approval" />
                    <br /> <asp:Literal ID="LiteralBeauty" runat="server" Text="Replace 'Beauty Advisors' text with :" />
                    <asp:TextBox ID="TextBeautyAdvisors" runat="server" Text="" /> <br />                   
                    <asp:CheckBox ID="IsLockSalesPersonGR" runat="server" Text="Lock Beauty Advisor/Sales Person at Goods Receive" />
                    <br />
                    <asp:Literal ID="ltrCostCalculationMode" runat="server" Text="Cost Calculation Mode : "></asp:Literal>
                    <asp:DropDownList ID="CostCalculationMode" runat="server">
                        <asp:ListItem Value="AVERAGE" Text="AVERAGE"></asp:ListItem>
                        <asp:ListItem Value="FIFO" Text="FIFO"></asp:ListItem>                        
                    </asp:DropDownList>
                    <br />
                    <asp:CheckBox ID="EnableProductSerialNo" runat="server" Text="Enable Product Serial No" />                    
                    <br />
                    <asp:CheckBox ID="Inventory_AllowToUpdateRetailPriceInGoodsReceive" runat="server" Text="Allow To Update Retail Price In Goods Receive" />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="pane3" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    Goods Ordering</Header>
                <Content>
                    <asp:CheckBox ID="AllowCreateOrderForOtherOutlet" runat="server" Text="Allow Create Order For Other Outlet" />
                    <br />
                    <asp:CheckBox ID="AutoApproveOrder" runat="server" Text="Auto Approve Order" />
                    <br />
                    <asp:Literal ID="lbl1" runat="server" Text="Default Sales Date Range : " />
                    <asp:TextBox ID="DefaultSalesDateRange" runat="server" Text="" />
                    <br />
                    <asp:CheckBox ID="AutoCreateSupplierPOUponOutletOrderApproval" runat="server" Text="Auto Create Supplier PO Upon Outlet Order Approval" />
                    <br />
                    <asp:CheckBox ID="AutoApproveSupplierPO" runat="server" Text="Auto Approve Supplier PO" />
                    <br />
                    <asp:CheckBox ID="ShowPriceLevelForWebOrder" runat="server" Text="Show Price Level for Web Order" />
                    <br />
                    <asp:CheckBox ID="ShowFactoryPriceInGoodsOrdering" runat="server" Text="Show Factory Price In Goods Ordering" />
                    <br />
                    <asp:CheckBox ID="ShowFactoryPriceInOrderApproval" runat="server" Text="Show Factory Price In Order Approval" />
                    <br />
                    <asp:CheckBox ID="ShowFactoryPriceInReturnApproval" runat="server" Text="Show Factory Price In Return Approval" />
                    <br />
                    <asp:CheckBox ID="AllowCreateInvoiceForStockTransferAndGoodsOrdering" runat="server" Text="Allow Create Invoice for Stock Transfer and Goods Ordering" />
                    <br />
                    <asp:CheckBox ID="StockReturnWillReturnStockToWarehouse" runat="server" Text="Stock Return Will Return Stock To Warehouse Also" />
                    <br />
                    <asp:CheckBox ID="GoodsOrdering_ShowSalesGR" runat="server" Text="Show Sales Quantity on Goods Receiving Web Order" />
                    <br />
                    <asp:Literal ID="Literal76" runat="server" Text="Range of Day on Show Sales :" />
                    <asp:TextBox ID="RangeSalesShownGR" runat="server" Text="" />
                    <br />
                    <asp:CheckBox ID="ShowPrintDOButtonInGoodsOrdering" runat="server" Text="Show Print DO Button In Goods Ordering" />
                    <br />
                    <asp:CheckBox ID="AllowOutletToOrderFromSupplier" runat="server" Text="Allow Outlet To Order From Supplier" />
                    <br />  
                    <asp:CheckBox ID="HideQtyInOutlet" runat="server" Text="Hide Qty in Outlet" />
                    <br /> 
                    <asp:Literal ID="Literal78" runat="server" Text="Status when All Received : " />
                    <asp:DropDownList ID="ddlStatusAllReceived" runat="server" Height="20px" Width="130px">
                        <asp:ListItem>Received</asp:ListItem>
                        <asp:ListItem>Posted</asp:ListItem>
                    </asp:DropDownList>
                    <br /> 
                    <asp:CheckBox ID="AutoGenerateInvoiceNo" runat="server" Text="Auto Generate Invoice No and Credit Note on Approval" />
                    <br />
                    <asp:Literal ID="Literal79" runat="server" Text="Prefix Invoice for Invoice and Credit Note No :" />
                     <asp:TextBox ID="AutoGenerateInvoiceNoPrefix" runat="server" Text="" />
                    <br />  
                    <asp:Literal ID="Literal80" runat="server" Text="Length :" />
                     <asp:TextBox ID="AutoGenerateInvoiceLength" runat="server" Text="" />
                    <br />        
                    <asp:Literal ID="Literal81" runat="server" Text="Default GST Setting on Web Order (Goods Ordering, Stock Return, Stock Transfer): " />
                     <asp:DropDownList ID="GoodsOrdering_InvoiceGSTRule" runat="server" Height="20px" Width="130px">
                        <asp:ListItem>Exclusive GST</asp:ListItem>
                        <asp:ListItem>Inclusive GST</asp:ListItem>
                        <asp:ListItem>Non GST</asp:ListItem>
                    </asp:DropDownList><br />       
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane21" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    Stock Transfer</Header>
                <Content>
                    <asp:CheckBox ID="StockTransferWillGoThroughWarehouse" runat="server" Text="Stock Transfer Will Go Through Warehouse" />
                    <br />                
                    <asp:CheckBox ID="UseTransferApproval" runat="server" Text="Use Transfer Approval" />
                    <br />
                    <asp:CheckBox ID="ShowFactoryPriceInTransferApproval" runat="server" Text="Show Factory Price In Transfer Approval" />
                    <br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="pane4" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    Invoice</Header>
                <Content>
                    <asp:Literal ID="Literal9" runat="server" Text="Replace 'Line Info' text with :" />
                    <asp:TextBox ID="LineInfo_ReplaceTextWith" runat="server" Text="" />
                    <br />
                    <asp:CheckBox ID="Membership_ShowRemarkInTransactionReport" runat="server" Text="Membership Transaction Report: Show Remark" />
                    <br />
                    <asp:CheckBox ID="Membership_ShowLineInfoInTransactionReport" runat="server" Text="Membership Transaction Report: Show Line Info" />
                    <br />
                    <asp:CheckBox ID="Membership_ShowBalancePaymentInTransactionReport" runat="server"
                        Text="Membership Transaction Report: Show Balance Payment" />
                    <br />
                    <asp:CheckBox ID="Membership_ShowQtyInTransactionReport" runat="server" Text="Membership Transaction Report: Show Quantity" />
                    <br />
                    <asp:CheckBox ID="Membership_ShowQtyOnHandInTransactionReport" runat="server" Text="Membership Transaction Report: Show Quantity On Hand" />
                    <br />
                    <asp:Literal ID="Literal17" runat="server" Text="Sales Cost Of Goods Using : " />
                    <asp:DropDownList ID="ddSalesCostPrice" runat="server" Height="20px" Width="130px">
                        <asp:ListItem>Item Summary Avg Cost Price</asp:ListItem>
                        <asp:ListItem>Item Avg Cost Price </asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:CheckBox ID="EnableCreditInvoice" runat="server" Text="Enable Credit Invoice" />
                    <br />
                    <asp:CheckBox ID="EnableStockTransferAtInvoice" runat="server" Text="Enable Stock Transfer At Invoice" />
                    <br />
                    <asp:CheckBox ID="UseSeparatedRefNoPerMembershipGroup" runat="server" Text="Use Separated Ref No Per Membership Group" />
                    <br />
                    <asp:Literal ID="Literal25" runat="server" Text="Separated Ref No Positioning : " />
                    <asp:DropDownList ID="ddlFirstPos" runat="server" Width="100px">
                        <asp:ListItem Value="Invoice_POSCodeRefNoPosition" Text="POS Code" />
                        <asp:ListItem Value="Invoice_MemberGroupCodeRefNoPosition" Text="Member Group Code" />
                        <asp:ListItem Value="Invoice_RunningNoRefNoPosition" Text="Running No" />
                    </asp:DropDownList>
                    <asp:TextBox ID="txtFirstSeparator" runat="server" Width="15px" />
                    <asp:DropDownList ID="ddlSecondPos" runat="server" Width="100px">
                        <asp:ListItem Value="Invoice_POSCodeRefNoPosition" Text="POS Code" />
                        <asp:ListItem Value="Invoice_MemberGroupCodeRefNoPosition" Text="Member Group Code" />
                        <asp:ListItem Value="Invoice_RunningNoRefNoPosition" Text="Running No" />
                    </asp:DropDownList>
                    <asp:TextBox ID="txtSecondSeparator" runat="server" Width="15px" />
                    <asp:DropDownList ID="ddlThirdPos" runat="server" Width="100px">
                        <asp:ListItem Value="Invoice_POSCodeRefNoPosition" Text="POS Code" />
                        <asp:ListItem Value="Invoice_MemberGroupCodeRefNoPosition" Text="Member Group Code" />
                        <asp:ListItem Value="Invoice_RunningNoRefNoPosition" Text="Running No" />
                    </asp:DropDownList>
                    <br />
                    <asp:Literal ID="Literal26" runat="server" Text="RefNo Running No Length : " />
                    <asp:TextBox ID="SeparatedRefNoLength" runat="server" Text="" />
                    <br />
                    <asp:Literal ID="Literal27" runat="server" Text="RefNo Running No Start No : " />
                    <asp:TextBox ID="SeparatedRefNoStartNo" runat="server" Text="" />
                    <br />
                    <asp:Literal ID="Literal28" runat="server" Text="RefNo Running No Increment : " />
                    <asp:TextBox ID="SeparatedRefNoStartIncrement" runat="server" Text="" />
                    <br />
                    <asp:CheckBox ID="LinkPOSToMember" runat="server" Text="Link POS To Member" />
                    <br />
                    <asp:CheckBox ID="Membership_AllowShowSalesOutlet" runat="server" Text="Allow Show Membership Sales Summary In Outlet only" />
                    <br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" HeaderCssClass="accordionHeader" ContentCssClass="accordionContent"
                ID="AccordionPane312" HeaderSelectedCssClass="accordionHeaderSelected">
                <Header>
                    Update Modified On Table
                </Header>
                <Content>
                    <table>
                        <tr>
                            <td>
                                Table Name
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtTableName" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Primary Key Column
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPKColumn" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
                            </td>
                        </tr>
                    </table>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="pane5" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    Funding</Header>
                <Content>
                    <asp:CheckBox ID="Funding_EnableFunding" runat="server" Text="Enable Funding" />
                    <br />
                    <asp:CheckBox ID="Funding_EnablePAMed" runat="server" Text="PA Medifund" />
                    <asp:TextBox ID="Funding_PAMedPercentage" runat="server" Text="" Width="30px" />
                    %
                    <br />
                    <asp:CheckBox ID="Funding_EnableSMF" runat="server" Text="SMF" />
                    <asp:TextBox ID="Funding_SMFPercentage" runat="server" Text="" Width="30px" />%
                    <br />
                    <asp:CheckBox ID="Funding_EnablePWF" runat="server" Text="PWF" />
                    <br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="pane6" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    Email Sender</Header>
                <Content>
                    <asp:Literal ID="Literal10" runat="server" Text="SMTP Server" />
                    <asp:TextBox ID="EmailSender_SMTP" runat="server" Text="" />
                    <br />
                    <asp:Literal ID="Literal11" runat="server" Text="Port" />
                    <asp:TextBox ID="EmailSender_Port" runat="server" Text="" />
                    <br />
                    <asp:Literal ID="Literal12" runat="server" Text="Sender Mail" />
                    <asp:TextBox ID="EmailSender_SenderEmail" runat="server" Text="" />
                    <br />
                    <asp:Literal ID="Literal13" runat="server" Text="Default Mail To" />
                    <asp:TextBox ID="EmailSender_DefaultMailTo" runat="server" Text="" />
                    <br />
                    <asp:Literal ID="Literal14" runat="server" Text="Username" />
                    <asp:TextBox ID="EmailSender_Username" runat="server" Text="" />
                    <br />
                    <asp:Literal ID="Literal15" runat="server" Text="Password" />
                    <asp:TextBox ID="EmailSender_Password" runat="server" Text="" TextMode="Password" />
                    (only fill this field if needed to change the password)
                    <br />
                    <asp:CheckBox ID="EmailSender_BccToOwner" runat="server" Text="BCC to Owner's Email :" />
                    <asp:TextBox ID="EmailSender_OwnerEmailAddress" runat="server" Text="" />
                    <br />
                    Cc
                    <asp:TextBox ID="EmailSender_Cc" runat="server" Text="" />
                    <br />
                    <asp:Literal ID="Literal16" runat="server" Text="Use as Receipt No in Email Receipt" />
                    <asp:DropDownList ID="EmailSender_ReceiptNoInEmailReceipt" runat="server">
                        <asp:ListItem Text="OrderHdrID" Value="OrderHdrID"></asp:ListItem>
                        <asp:ListItem Text="Custom Invoice No" Value="Custom Invoice No"></asp:ListItem>
                        <asp:ListItem Text="Line Info" Value="Line Info"></asp:ListItem>
                    </asp:DropDownList>
                    <br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane4" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Purchase Order</Header>
                <Content>
                    <asp:CheckBox ID="PurchaseOrder_UseCustomNo" runat="server" Text="Use Server Generated Custom Purchase Order No" />
                    <br />
                    Custom Prefix
                    <asp:TextBox ID="PurchaseOrder_CustomPrefix" runat="server" Text="" />
                    <br />
                    Custom Suffix
                    <asp:TextBox ID="PurchaseOrder_CustomSuffix" runat="server" Text="" />
                    <br />
                    Length
                    <asp:TextBox ID="PurchaseOrder_NumberLength" runat="server" Text="" />
                    <br />
                    Current No
                    <asp:TextBox ID="PurchaseOrder_CurrentNo" runat="server" Text="" />
                    <br />
                    Reset every
                    <asp:DropDownList ID="PurchaseOrder_ResetNumberEvery" runat="server">
                    </asp:DropDownList>
                    <br />
                    Date format
                    <asp:TextBox ID="PurchaseOrder_CustomNoDateFormat" runat="server" Text="" />
                    <br />
                    <asp:Literal ID="Literal29" runat="server" Text="PO Mail CC : " />
                    <asp:TextBox ID="POMailCC" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal30" runat="server" Text="PO Mail Subject : " />
                    <asp:TextBox ID="POMailSubject" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal31" runat="server" Text="PO Mail Content : " />
                    <asp:TextBox ID="POMailContent" runat="server" Text="" TextMode="MultiLine" Width="200px"
                        Height="100px" /><br />
                    <asp:CheckBox ID="IsSellingPriceEditable" runat="server" Text="Is Selling Price Editable" /><br />
                    <asp:CheckBox ID="AutoUpdateCostPriceOnSupplierPOApproval" runat="server" Text="Auto Update Cost Price On Supplier PO Approval" />
                    <br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane5" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Goods Receive Custom Ref No</Header>
                <Content>
                    <asp:CheckBox ID="GoodsReceive_UseCustomNo" runat="server" Text="Use Server Generated Custom Goods Receive No" />
                    <br />
                    Custom Prefix
                    <asp:TextBox ID="GoodsReceive_CustomPrefix" runat="server" Text="" />
                    <br />
                    Custom Suffix
                    <asp:TextBox ID="GoodsReceive_CustomSuffix" runat="server" Text="" />
                    <br />
                    Length
                    <asp:TextBox ID="GoodsReceive_NumberLength" runat="server" Text="" />
                    <br />
                    Current No
                    <asp:TextBox ID="GoodsReceive_CurrentNo" runat="server" Text="" />
                    <br />
                    Reset every
                    <asp:DropDownList ID="GoodsReceive_ResetNumberEvery" runat="server">
                    </asp:DropDownList>
                    <br />
                    Date format
                    <asp:TextBox ID="GoodsReceive_CustomNoDateFormat" runat="server" Text="" />
                    <br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" HeaderCssClass="accordionHeader" ContentCssClass="accordionContent"
                ID="AccordionPane11" HeaderSelectedCssClass="accordionHeaderSelected">
                <Header>
                    Language Setting
                </Header>
                <Content>
                    <asp:DropDownList ID="ddlLangSetting" OnInit="ddlLangSetting_Init" runat="server"
                        DataTextField="Name" DataValueField="ID">
                    </asp:DropDownList>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane6" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    NETS Integration</Header>
                <Content>
                    <asp:CheckBox ID="chkEnableNETSIntegration" runat="server" Text="Enable NETS Integration" /><br />
                    <asp:CheckBox ID="chkNETSCashCard" runat="server" Text="Cash Card" /><br />
                    <asp:CheckBox ID="chkNETSFlashPay" runat="server" Text="Flash Pay" /><br />
                    <asp:CheckBox ID="chkNETSATMCard" runat="server" Text="ATM Card" /><br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane7" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Points and Package</Header>
                <Content>
                    <asp:CheckBox ID="chkUsingPointsPercentage" runat="server" Text="Using Points Percentage" /><br />
                    Points Item No
                    <asp:TextBox ID="txtPointsItemNo" runat="server" Text="" />
                    <br />
                    <asp:CheckBox ID="chkUsingExpiryPeriodForPoints" runat="server" Text="Using Expiry Period For Points" /><br />
                    Expiry Period (Months)
                    <asp:TextBox ID="txtExpiryPeriod" runat="server" Text="" />
                    <br />
                    <asp:CheckBox ID="chkWontGetRewardPointsIfBuyPackageItem" runat="server" Text="Will not get reward points if buy point/package items" /><br />
                    <asp:CheckBox ID="chkExcludePaymentTypeForPointsCalculation" runat="server" Text="Exclude these payment type for points calculation" />
                    <asp:TextBox ID="txtExcludedPaymentType" runat="server" Text="" /> <br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane8" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    User</Header>
                <Content>
                    <asp:CheckBox ID="SeparateUserPerOutletPrivileges" runat="server" Text="Separate User For Report" /><br />
                    <asp:CheckBox ID="ShowPointOfSale" runat="server" Text="Show Point Of Sale" /><br />
                    <asp:CheckBox ID="ShowOutlet" runat="server" Text="Show Outlet" /><br />
                    <asp:CheckBox ID="User_UseSupplierPortal" runat="server" Text="Use Supplier Portal" /><br />                    
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane9" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    App Setting Sync</Header>
                <Content>
                    App Setting Sync List (separated by &#39;,&#39; )
                    <br />
                    <asp:TextBox ID="POSAppSettingSyncList" TextMode="MultiLine" runat="server" Text=""
                        Height="200px" Width="750px" /><br />
                    <asp:Literal ID="Literal75" runat="server" Text="Other Item No : " />
                    <asp:TextBox ID="OtherItemNo" runat="server" Text="" /><br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane10" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Mall Integration</Header>
                <Content>
                    <asp:CheckBox ID="UseMallManagement" runat="server" Text="Use Mall Management" /><br />
                    POS ID Prefix
                    <asp:TextBox ID="POSIDPrefix" runat="server" Text="" /><br />
                    Point Of Sale Text
                    <asp:TextBox ID="PointOfSaleText" runat="server" Text="" /><br />
                    Outlet Text
                    <asp:TextBox ID="OutletText" runat="server" Text="" /><br />
                    Interface Dev Team
                    <asp:TextBox ID="InterfaceDevTeam" runat="server" Text="" /><br />
                    Tenant ID Start From
                    <asp:TextBox ID="TenantIDStartFrom" runat="server" Text="" /><br />
                    Tenant ID Increment
                    <asp:TextBox ID="TenantIDIncrement" runat="server" Text="" /><br />
                    Daily Sales Discrepancy Percentage
                    <asp:TextBox ID="DiscrepancyPercentage" runat="server" Text="" /><br />
                    Cut Off Date
                    <asp:TextBox ID="CutOffDate" runat="server" Text="" /><br />
                    Mall Management Team Email
                    <asp:TextBox ID="MallManagementTeamEmail" runat="server" Text="" /><br />
                    Edgeworks Team Email
                    <asp:TextBox ID="EdgeworksTeamEmail" runat="server" Text="" /><br />
                    Retailer Level Attribute 1
                    <asp:TextBox ID="RetailerLevelAttribute1" runat="server" Text="" /><br />
                    Retailer Level Attribute 2
                    <asp:TextBox ID="RetailerLevelAttribute2" runat="server" Text="" /><br />
                    Interface File Specification Name
                    <asp:TextBox ID="InterfaceFileName" runat="server" Text="" /><br />
                    Interface File Specification
                    <asp:FileUpload ID="fuFileSpec" runat="server" />
                    <asp:Button OnClick="btnUploadFileSpec_Click" ID="btnUploadFileSpec" runat="server"
                        Text="Upload" />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane12" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Refund</Header>
                <Content>
                    <asp:CheckBox ID="Refund_RefundReceiptFromOtherOutlet" runat="server" Text="Allow refund receipt from other outlet" /><br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane13" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Discount</Header>
                <Content>
                    <asp:CheckBox ID="DiscountReportShowSearchDiscountReason" runat="server" Text="Transaction Discount Report - Show Search by Discount Reason" /><br />
                    Discount Reason (separated by &#39;;&#39; )
                    <br />
                    <asp:TextBox ID="Invoice_SelectableDiscountReason" TextMode="MultiLine" runat="server"
                        Text="" Height="50px" Width="750px" />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane14" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Appointment</Header>
                <Content>
                    <asp:CheckBox ID="UseResourceOnAppointment" runat="server" Text="Use Resource on Appointment" /><br />
                    Minimum Interval on Appointment View Web (15 - 60) <asp:TextBox ID="MinimumIntervalWeb" runat="server" Text="" /><br />
                    Auto Check Out Guests after <asp:TextBox ID="AutoCheckOutIntervalGuestBook" Width="50px" runat="server" Text="" /> hrs<br />
                    <asp:CheckBox ID="ShowPrefixMembershipOutlet" runat="server" Text="Show Prefix Membership for Outlet" /> 
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane15" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Mobile Stock</Header>
                <Content>
                    <asp:CheckBox ID="Mobile_DisplayCost" runat="server" Text="Display Cost" /><br />
                    <asp:CheckBox ID="Mobile_DisplayBatchNo" runat="server" Text="Display Batch No" /><br />
                    <asp:CheckBox ID="Mobile_DisplayShelf" runat="server" Text="Display Shelf" /><br />
                    <asp:CheckBox ID="Mobile_EnableStockIn" runat="server" Text="Enable Stock In" /><br />
                    <asp:CheckBox ID="Mobile_EnableStockOut" runat="server" Text="Enable Stock Out" /><br />
                    <asp:CheckBox ID="Mobile_EnableStockTake" runat="server" Text="Enable Stock Take" /><br />
                    <asp:CheckBox ID="Mobile_EnableStockTransfer" runat="server" Text="Enable Stock Transfer" /><br />
                    <asp:CheckBox ID="Mobile_EnablePO" runat="server" Text="Enable PO" /><br />
                    <asp:CheckBox ID="Mobile_EnableStockInFromPO" runat="server" Text="Enable Stock In From PO" /><br /><br />
                    <asp:CheckBox ID="Mobile_EnableRecordData" runat="server" Text="Enable Record Data" /><br />
                    <asp:Literal ID="Literal51" runat="server" Text="1. " /><asp:TextBox ID="Mobile_RecordData1" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal52" runat="server" Text="2. " /><asp:TextBox ID="Mobile_RecordData2" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal53" runat="server" Text="3. " /><asp:TextBox ID="Mobile_RecordData3" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal54" runat="server" Text="4. " /><asp:TextBox ID="Mobile_RecordData4" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal55" runat="server" Text="5. " /><asp:TextBox ID="Mobile_RecordData5" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal56" runat="server" Text="6. " /><asp:TextBox ID="Mobile_RecordData6" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal57" runat="server" Text="7. " /><asp:TextBox ID="Mobile_RecordData7" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal58" runat="server" Text="8. " /><asp:TextBox ID="Mobile_RecordData8" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal59" runat="server" Text="9. " /><asp:TextBox ID="Mobile_RecordData9" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal60" runat="server" Text="10. " /><asp:TextBox ID="Mobile_RecordData10" runat="server" Text="" /><br />
                    <asp:CheckBox ID="Mobile_SaveStockInInTemporaryFiles" runat="server" Text="Save Stock In in Saved Files" /><br />
                    <asp:Literal ID="Literal77" runat="server" Text="Product Data Applicable To : " />
                     <asp:DropDownList ID="ddlProductApplicableTo" runat="server" Height="20px" Width="130px">
                        <asp:ListItem>Product Master</asp:ListItem>
                        <asp:ListItem>Outlet</asp:ListItem>
                    </asp:DropDownList>
                </Content>
            </cc1:AccordionPane>
             <cc1:AccordionPane ID="AccordionPane16" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Low Qty Warning</Header>
                <Content>
                    <asp:Literal ID="Literal41" runat="server" Text="Userfld1 : " />
                    <asp:TextBox ID="LowQtyUserfld1" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal42" runat="server" Text="Userfld2 : " />
                    <asp:TextBox ID="LowQtyUserfld2" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal43" runat="server" Text="Userfld3 : " />
                    <asp:TextBox ID="LowQtyUserfld3" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal44" runat="server" Text="Userfld4 : " />
                    <asp:TextBox ID="LowQtyUserfld4" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal45" runat="server" Text="Userfld5 : " />
                    <asp:TextBox ID="LowQtyUserfld5" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal46" runat="server" Text="Userfld6 : " />
                    <asp:TextBox ID="LowQtyUserfld6" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal47" runat="server" Text="Userfld7 : " />
                    <asp:TextBox ID="LowQtyUserfld7" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal48" runat="server" Text="Userfld8 : " />
                    <asp:TextBox ID="LowQtyUserfld8" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal49" runat="server" Text="Userfld9 : " />
                    <asp:TextBox ID="LowQtyUserfld9" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal50" runat="server" Text="Userfld10 : " />
                    <asp:TextBox ID="LowQtyUserfld10" runat="server" Text="" />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane17" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Reports</Header>
                <Content>
                    <asp:Literal ID="Literal63" runat="server" Text="Aggregated Sales Report Max History : " />
                    <asp:TextBox ID="Reports_AggregatedSalesReportMaxHistory" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal70" runat="server" Text="ZXV3 Upload Directory : " />
                    <asp:TextBox ID="Reports_ZXV3UploadDirectory" runat="server" Text="" /><br />
                    <asp:CheckBox ID="Reports_ShowPointInstallmentBreakdownInDailySales" runat="server" Text="Show Point and Installment Breakdown in Daily Sales Report" /><br />
                    <asp:CheckBox ID="Report_UseDataWarehouse" runat="server" Text="Use Data Warehouse" /><br />                    
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane18" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    FTP Settings</Header>
                <Content>
                    <asp:Literal ID="Literal64" runat="server" Text="Protocol : " />
                    <asp:DropDownList ID="FTP_Protocol" runat="server">
                        <asp:ListItem Value="FTP" Text="FTP"></asp:ListItem>
                        <asp:ListItem Value="SFTP" Text="SFTP"></asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Literal ID="Literal65" runat="server" Text="Host : " />
                    <asp:TextBox ID="FTP_Host" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal66" runat="server" Text="Port : " />
                    <asp:TextBox ID="FTP_Port" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal67" runat="server" Text="Username : " />
                    <asp:TextBox ID="FTP_Username" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal68" runat="server" Text="Password : " />
                    <asp:TextBox ID="FTP_Password" runat="server" Text="" /><br />
                    <asp:Literal ID="Literal69" runat="server" Text="PassiveMode : " />
                    <asp:CheckBox ID="FTP_PassiveMode" runat="server" Text="" /><br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane19" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Audit Log</Header>
                <Content>
                    <asp:CheckBox ID="AuditLog_ProductSetup" runat="server" Text="Enable in Product Setup" /><br />
                    <asp:CheckBox ID="AuditLog_SetupProductPromotion" runat="server" Text="Enable in Setup Product Promotion" /><br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPaneCommission" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Commission</Header>
                <Content>
                    <asp:CheckBox ID="Commission_GiveCommissionUponPayment" runat="server" Text="Give Commission Upon Payment" /><br /><br />
                    <asp:Label ID="Label1" runat="server" Text="Commission Based on the (Total / Bracket) : "></asp:Label>
                    <asp:RadioButtonList ID="Commission_BasedOn" runat="server">
                        <asp:ListItem Text="Total : It will take to percentage of the bracket immediately" Value="Total" />
                        <asp:ListItem Text="Bracket : Count bracket by bracket hierarchically" Value="Bracket" />
                    </asp:RadioButtonList>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane22" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Recipe</Header>
                <Content>
                    <asp:CheckBox ID="Recipe_EnableRecipeManagement" runat="server" Text="Enable Recipe Management"
                        Checked="true" /><br />
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane23" runat="server" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
                <Header>
                    Membership</Header>
                <Content>
                    <asp:Literal ID="Literal82" runat="server" Text="Default Expiry Date (in years) from Subscription Date : " />
                    <asp:TextBox ID="Membership_DefaultExpiryDate" runat="server" Text="" /><br />                    
                </Content>
            </cc1:AccordionPane>
        </Panes>
    </cc1:Accordion>
</asp:Content>
