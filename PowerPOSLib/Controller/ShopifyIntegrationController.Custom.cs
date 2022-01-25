using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using PowerPOS.Container;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Data;

namespace PowerPOS
{
    public partial class ShopifyIntegrationController
    {
        public DeliveryOrderDetailCollection delOrderDetColl;
        public DeliveryOrder delOrderHdr;
        public DeliveryOrderCollection delOrderHdrColl;
        DeliveryController doc;
        public static bool AddShopifyIntegration(ShopifyEvent eventName,
            string reqHeader, string reqData, out string status)
        {
            bool isSuccess = false;
            status = "";
            try
            {
                if (string.IsNullOrEmpty(reqData))
                    throw new Exception("Request data cannot be empty");
                isSuccess = false;

                ShopifyIntegration data = new ShopifyIntegration();
                data.EventName = eventName.ToString();
                data.RequestDate = DateTime.Now;
                data.RequestHeader = reqHeader;
                data.RequestData = reqData;
                data.Status = "";
                data.Deleted = false;
                data.UniqueID = Guid.NewGuid();

                if (eventName == ShopifyEvent.OrderPayment)
                    isSuccess = OrderPayment(reqData, data.GetInsertCommand("SYSTEM"), out status);
                //else if (eventName == ShopifyEvent.OrderCancellation)
                //    isSuccess = OrderCancellation(reqData, data.GetInsertCommand("SYSTEM"), out status);
                else
                    data.Save("SYSTEM");
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "ERROR. " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        //private static bool OrderCancellation(string data, QueryCommand additonalCmd, out string status)
        //{
        //    bool isSuccess = false;
        //    status = "";

        //    try
        //    {
        //        var orderData = new JavaScriptSerializer().Deserialize<order>(data);

        //        string cashierID = AppSetting.GetSetting(AppSetting.SettingsName.Shopify.CashierID) + "";
        //        int posID = (AppSetting.GetSetting(AppSetting.SettingsName.Shopify.PointOfSaleID) + "").GetIntValue();
        //        //string tableNo = AppSetting.GetSetting(AppSetting.SettingsName.Shopify.TableNo) + "";
        //        string paymentType = AppSetting.GetSetting(AppSetting.SettingsName.Shopify.PaymentType) + "";
        //        string membershipGroup = AppSetting.GetSetting(AppSetting.SettingsName.Shopify.MembershipGroup) + "";
        //        string shippingCostItemNo = AppSetting.GetSetting(AppSetting.SettingsName.Shopify.ShippingCostItemNo) + "";

        //        var additionalCommands = new QueryCommandCollection();

        //        UserInfo.username = cashierID;

        //        POSController pos = new POSController();

        //        OrderHdr oh = new OrderHdr(OrderHdr.UserColumns.ShopifyTransactionID, orderData.id);

        //        if (oh.IsNew || oh.IsVoided == true)
        //            isSuccess = true;
        //        else
        //            isSuccess = POSController.VoidReceipt(oh.OrderRefNo, oh.CashierID, oh.CashierID, true, oh.Remark + " - VOIDED FROM SHOPIFY", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        isSuccess = false;
        //        status = ex.Message;
        //    }

        //    return isSuccess;
        //}

        private static bool OrderPayment(string data, QueryCommand additionalCmd, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                var orderData = new JavaScriptSerializer().Deserialize<order>(data);

                string cashierID = System.Configuration.ConfigurationManager.AppSettings[AppSetting.SettingsName.Shopify.CashierID];
                int posID = (System.Configuration.ConfigurationManager.AppSettings[AppSetting.SettingsName.Shopify.PointOfSaleID]).GetInt32Value();
                //string tableNo = AppSetting.GetSetting(AppSetting.SettingsName.Shopify.TableNo) + "";
                string paymentType = System.Configuration.ConfigurationManager.AppSettings[AppSetting.SettingsName.Shopify.PaymentType];
                string membershipGroup = System.Configuration.ConfigurationManager.AppSettings[AppSetting.SettingsName.Shopify.MembershipGroup] + "";
                string shippingCostItemNo = System.Configuration.ConfigurationManager.AppSettings[AppSetting.SettingsName.Shopify.ShippingCostItemNo] + "";
                string defaultItemNo = System.Configuration.ConfigurationManager.AppSettings[AppSetting.SettingsName.Shopify.DefaultItemNo] + "";
                PointOfSaleInfo.PointOfSaleID = posID;

                var additionalCommands = new QueryCommandCollection();

                UserInfo.username = cashierID;

                POSController pos = new POSController();


                OrderHdr oh = new OrderHdr(OrderHdr.UserColumns.ShopifyTransactionID, orderData.id);
                if (!oh.IsNew)
                    return true;
                SalesPersonInfo.SalesPersonID = "0";
                //pos.SetTableNo(tableNo);
                pos.myOrderHdr.ShopifyTransactionID = orderData.id + "";
                //pos.SetPax(1);
                //pos.myOrderHdr.Userflag1 = false;

                #region *) Validation

                if (orderData == null)
                    throw new Exception("Invalid order data");

                if (orderData.line_items == null || orderData.line_items.Count == 0)
                    throw new Exception("Order detail cannot be empty");

                #endregion

                #region *) Order Detail

                ViewItem myItem;
                foreach (var det in orderData.line_items)
                {

                    PowerPOS.Item it = new Item(Item.Columns.Attributes2, det.sku);
                    if (it.IsNew || it.Deleted.GetValueOrDefault(false))
                    {
                        it = new Item(defaultItemNo);
                        if (it.IsNew || it.Deleted.GetValueOrDefault(false))
                            throw new Exception(string.Format("Item {0} not found", det.product_id));
                        else
                            it.Remark = det.name;
                    }

                    //myItem = new ViewItem(ViewItem.Columns.ItemNo, det.product_id);

                    //decimal PreferedDiscount = pos.GetPreferredDiscount();
                    bool ApplyPromo = false;
                    //pos.AddItemToOrderWithPriceMode(it, (decimal)det.quantity, 0, ApplyPromo, "NORMAL", true, out status);
                    pos.AddItemToOrderWithPriceModeShopify(it, (decimal)det.quantity, 0, ApplyPromo, "NORMAL", true, (decimal)det.price, out status);
                }

                #endregion

                #region *) Membership

                if (orderData.customer != null)
                {
                    Membership mp = new Membership(Membership.Columns.MembershipNo, orderData.customer.id);
                    if (mp.IsNew)
                    {
                        mp.MembershipNo = orderData.customer.id + "";
                        mp.NameToAppear = orderData.customer.first_name + " " + orderData.customer.last_name;
                        mp.FirstName = orderData.customer.first_name;
                        mp.LastName = orderData.customer.last_name;
                        if (orderData.customer.phone != null)
                            mp.Mobile = orderData.customer.phone.Replace(" ", String.Empty);
                        else
                        {
                            if (orderData.billing_address.phone != null)
                            {
                                mp.Mobile = orderData.billing_address.phone.Replace(" ", String.Empty);
                            }
                            else
                            {
                                mp.Mobile = "";
                            }

                        }
                        mp.SubscriptionDate = DateTime.Now;
                        mp.DateOfBirth = DateTime.Now;
                        mp.ExpiryDate = DateTime.Now.AddYears(10);
                        mp.UniqueID = Guid.NewGuid();
                        if (orderData.customer.default_address != null)
                        {
                            if (orderData.customer.default_address.address1.Length > 50)
                            {
                                orderData.customer.default_address.address2 = orderData.customer.default_address.address1.Substring(50, orderData.customer.default_address.address1.Length - 50) + " " + orderData.customer.default_address.address2;
                                orderData.customer.default_address.address1 = orderData.customer.default_address.address1.Substring(0, 50);

                            }

                            mp.StreetName = orderData.customer.default_address.address1;
                            mp.StreetName2 = orderData.customer.default_address.address2;
                            mp.City = orderData.customer.default_address.city;
                            mp.ZipCode = orderData.customer.default_address.zip;
                            mp.Country = orderData.customer.default_address.country;
                        }
                        mp.Deleted = false;

                        MembershipGroup mg = new MembershipGroup(MembershipGroup.Columns.GroupName, membershipGroup);
                        if (mg.IsNew)
                        {
                            mg.GroupName = membershipGroup;
                            mg.Deleted = false;
                            additionalCommands.Add(mg.GetInsertCommand("SYSTEM"));
                        }

                        mp.MembershipGroupId = mg.MembershipGroupId;
                        additionalCommands.Add(mp.GetInsertCommand("SYSTEM"));
                        SubSonic.DataService.ExecuteTransaction(additionalCommands);
                    }

                    pos.AssignMembership(mp.MembershipNo, out status);
                }
                else
                {
                    pos.AssignMembership("WALK-IN", out status);
                }

                //pos.AssignMembership(mp.MembershipNo, out status);

                #endregion

                #region *) Delivery Order

                //bool isSelfCollection = false;
                //string collectionOutlet = "";
                //foreach (var item in orderData.note_attributes)
                //{
                //    if ((item.name + "").ToLower().Equals("delivery_method") && (item.value + "").ToLower().Equals("self collection"))
                //    {
                //        isSelfCollection = true;
                //    }

                //    if ((item.name + "").ToLower().Equals("collection_outlet"))
                //    {
                //        collectionOutlet = item.value + "";
                //    }
                //}

                //if (isSelfCollection || (orderData.shipping_lines != null && orderData.shipping_lines.Count > 0))
                //{
                //    var delivery_date = orderData.processed_at.GetValueOrDefault(DateTime.Now);
                //    var delivery_timeslot = "";
                //    var collection_date = orderData.processed_at.GetValueOrDefault(DateTime.Now);
                //    if (orderData.note_attributes != null)
                //    {
                //        foreach (var item in orderData.note_attributes)
                //        {
                //            if ((item.name + "").ToLower().Equals("delivery_date"))
                //                delivery_date = (item.value + "").GetDateValue("dd-MM-yyyy");
                //            else if ((item.name + "").ToLower().Equals("delivery_timeslot"))
                //                delivery_timeslot = item.value;
                //            else if ((item.name + "").ToLower().Equals("collection_date"))
                //                delivery_date = (item.value + "").GetDateValue("dd-MM-yyyy");
                //        }
                //    }


                //    var addr = orderData.shipping_address;
                //    if (addr == null)
                //        addr = orderData.billing_address;
                //    if (addr == null &&
                //        orderData.customer != null &&
                //        orderData.customer.default_address != null)
                //        addr = orderData.customer.default_address;

                //    if (string.IsNullOrEmpty(pos.GetMembershipNo()))
                //    {
                //        Membership mp = new Membership();
                //        mp.MembershipNo = MembershipController.GetNewMemberNo("M");
                //        mp.NameToAppear = addr.name;
                //        mp.FirstName = addr.first_name;
                //        mp.LastName = addr.last_name;
                //        mp.Mobile = addr.phone;
                //        mp.StreetName = addr.address1;
                //        mp.StreetName2 = addr.address2;
                //        mp.City = addr.city;
                //        mp.Country = addr.country;
                //        mp.ZipCode = addr.zip;
                //        mp.SubscriptionDate = DateTime.Now;
                //        mp.DateOfBirth = DateTime.Now;
                //        mp.ExpiryDate = DateTime.Now.AddYears(10);
                //        mp.Email = orderData.email;
                //        mp.UniqueID = Guid.NewGuid();
                //        mp.Deleted = false;
                //        MembershipGroup mg = new MembershipGroup(MembershipGroup.Columns.GroupName, membershipGroup);
                //        if (mg.IsNew)
                //        {
                //            mg.GroupName = membershipGroup;
                //            mg.Deleted = false;
                //            additionalCommands.Add(mg.GetInsertCommand("SYSTEM"));
                //        }
                //        mp.MembershipGroupId = mg.MembershipGroupId;
                //        additionalCommands.Add(mp.GetInsertCommand("SYSTEM"));

                //        pos.SetMembershipNo(mp.MembershipNo);
                //    }

                //    DeliveryOrderHdr hdr = new DeliveryOrderHdr();
                //    hdr.Name = addr.name;
                //    hdr.Address = addr.address1 + " " + addr.address2;
                //    hdr.ContactNo = addr.phone;
                //    hdr.Email = orderData.email;
                //    hdr.DeliveryDate = delivery_date;
                //    hdr.DeliveryOrderTime = delivery_timeslot;
                //    hdr.DeliveryInstruction = "";
                //    hdr.Status = DeliveryStatus.Submitted.ToString();
                //    hdr.Userflag1 = false;
                //    hdr.Userfld1 = addr.name;
                //    hdr.MembershipNo = pos.GetMembershipNo();
                //    hdr.PickFromOutlet = isSelfCollection;//orderData.shipping_lines[0].code.ToLower().StartsWith("self collection");
                //    hdr.ShopifyID = orderData.shipping_lines.Count == 0 ? "" : orderData.shipping_lines[0].id + "";
                //    if (hdr.PickFromOutlet)
                //    {
                //        Outlet ou = new Outlet(Outlet.UserColumns.MappedOutletID, collectionOutlet);
                //        if (ou.IsNew)
                //            hdr.PickFromOutlet = false;
                //        else
                //        {
                //            hdr.DeliverToOutletName = ou.OutletName;
                //            hdr.Address = ou.OutletName;
                //        }
                //    }

                //    DeliveryOrderDetCollection dets = new DeliveryOrderDetCollection();
                //    for (int i = 0; i < pos.myOrderDet.Count; i++)
                //    {
                //        var newDet = new DeliveryOrderDet();
                //        newDet.OrderDetID = pos.myOrderDet[i].OrderDetID;
                //        newDet.ItemNo = pos.myOrderDet[i].ItemNo;
                //        newDet.ItemName = pos.myOrderDet[i].Item.ItemName;
                //        newDet.OrderQuantity = pos.myOrderDet[i].Quantity;
                //        newDet.DeliveryQuantity = pos.myOrderDet[i].Quantity;
                //        newDet.UnitPrice = pos.myOrderDet[i].UnitPrice;
                //        newDet.Amount = pos.myOrderDet[i].Amount;
                //        if (newDet.DeliveryQuantity > 0)
                //            dets.Add(newDet);
                //    }

                //    pos.CaputureDelivery(hdr, dets);

                //    if (orderData.shipping_lines.Count > 0 && orderData.shipping_lines[0].price > 0)
                //    {
                //        string shippingItemNo = AppSetting.GetSetting(AppSetting.SettingsName.Shopify.ShippingCostItemNo) + "";
                //        Item shippingItem = new Item(Item.Columns.ItemNo, shippingItemNo);
                //        if (shippingItem.IsNew)
                //            throw new Exception("Shipping cost item not found");

                //        if (!pos.AddItemToOrder(shippingItem, 1, orderData.shipping_lines[0].price.GetValueOrDefault(0), out status))
                //            throw new Exception(string.Format("Cannot add Shipping cost item {0} : {1}", shippingItem.ItemNo, status));
                //    }
                //    pos.myOrderHdr.OrderType = "Delivery";
                //}
                //else
                //{
                //    pos.myOrderHdr.OrderType = "Cash & Carry";
                //}

                #endregion

                #region *) Discount

                //if (orderData.discount_codes != null)
                //{
                //    foreach (var disc in orderData.discount_codes)
                //    {
                //        OrderSubTotal ost = new OrderSubTotal();
                //        ost.Name = disc.code;
                //        ost.Ranking = 0;
                //        ost.DiscDollar = disc.amount.GetValueOrDefault(0);
                //        ost.SubTotalType = "discount ($$)";
                //        if (!pos.AddNewOrderSubtotalItem(ost, out status))
                //            throw new Exception(status);
                //    }
                //}

                //var subTotal = pos.CalculateSubTotalAmount(out status).ToString("N2");
                //decimal total = pos.CalculateGrandTotal();

                #endregion

                #region *) Receipt

                pos.calculateTotalGST();
                pos.CalculateTotalAmount(out status);
                pos.SetTotalReceiptAmount(orderData.total_price.GetValueOrDefault(0));
                decimal change;
                pos.AddReceiptLinePayment(orderData.total_price.GetValueOrDefault(0), paymentType, "", 0, "", 0, out change, out status);

                #endregion

                #region *) Confirm Order



                bool IsPointAllocationSuccess = false;
                if (!pos.ConfirmOrder(false, out IsPointAllocationSuccess, out status))
                    throw new Exception(status);

                //pos.myOrderHdr.Remark = orderData.order_number + "";
                //additionalCommands.Add(additionalCmd);
                //if (!pos.ConfirmOrderFB(true, additionalCommands, out status))
                //    throw new Exception(status);

                #endregion
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Integration.SyncCreateDeliveryOrder), false))
                {
                    //Create DO if setting enable
                    int count = 0;
                    int hdrNo = 0;
                    int detNo = 0;
                    string str = null;
                    var delOrderHdrColl = new DeliveryOrderCollection();
                    var delOrderDetColl = new DeliveryOrderDetailCollection();
                    if (orderData.customer != null)
                    {
                        Membership mp = new Membership(Membership.Columns.MembershipNo, orderData.customer.id);
                        //foreach (var det in orderData.line_items)
                        //{
                        //    //string newstr = dr["RecipientName"].ToString().Trim() + dr["MobileNo"].ToString().Trim() +
                        //    //             dr["HomeNo"].ToString().Trim() + dr["PostalCode"].ToString().Trim() +
                        //    //             dr["DeliveryAddress"].ToString().Trim() + dr["UnitNo"].ToString().Trim() +
                        //    //             dr["DeliveryDate"].ToString().Trim() + dr["DeliveryTime"].ToString().Trim() +
                        //    //             dr["Remarks"].ToString().Trim();
                        //    hdrNo++;
                        //    detNo = 0;

                        //    DeliveryOrder doHdr = new DeliveryOrder();
                        //    doHdr.CopyFrom(doc.myDeliveryOrderHdr);
                        //    doHdr.OrderNumber = hdrNo.ToString();
                        //    doHdr.PersonAssigned = -1;
                        //    doHdr.RecipientName = mp.NameToAppear;
                        //    doHdr.MobileNo = mp.Mobile;
                        //    doHdr.HomeNo = mp.Home;
                        //    doHdr.PostalCode = mp.ZipCode;
                        //    doHdr.DeliveryAddress = mp.StreetName;
                        //    //doHdr.UnitNo = ;
                        //    doHdr.Remark = "Created from shopify";
                        //    doHdr.IsVendorDelivery = false;
                        //    doHdr.DeliveryDate = null;
                        //    deliveryDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value.Date;
                        //    doHdr.TimeSlotFrom = null;
                        //    doHdr.TimeSlotTo = null;
                        //    delOrderHdrColl.Add(doHdr);

                        //    DeliveryOrderDetail doDet = new DeliveryOrderDetail();
                        //    doDet.Dohdrid = hdrNo.ToString();
                        //    PowerPOS.Item it = new Item(Item.UserColumns.MappedItemNo, det.sku);

                        //    doDet.ItemNo = it.ItemNo;
                        //    doDet.Quantity = det.quantity;
                        //    doDet.DetailsID = doDet.Dohdrid + "." + detNo.ToString();
                        //    doDet.OrderDetID = "";
                        //    delOrderDetColl.Add(doDet);



                        //}
                        var doc = new DeliveryController();
                        foreach (var det in pos.myOrderDet)
                        {
                            //string newstr = dr["RecipientName"].ToString().Trim() + dr["MobileNo"].ToString().Trim() +
                            //             dr["HomeNo"].ToString().Trim() + dr["PostalCode"].ToString().Trim() +
                            //             dr["DeliveryAddress"].ToString().Trim() + dr["UnitNo"].ToString().Trim() +
                            //             dr["DeliveryDate"].ToString().Trim() + dr["DeliveryTime"].ToString().Trim() +
                            //             dr["Remarks"].ToString().Trim();
                            hdrNo++;
                            detNo = 0;

                            DeliveryOrder doHdr = new DeliveryOrder();
                            doHdr.CopyFrom(doc.myDeliveryOrderHdr);
                            doHdr.OrderNumber = hdrNo.ToString();
                            doHdr.PersonAssigned = -1;
                            doHdr.RecipientName = mp.NameToAppear;
                            doHdr.MobileNo = mp.Mobile;
                            doHdr.HomeNo = mp.Home;
                            doHdr.PostalCode = mp.ZipCode;
                            doHdr.DeliveryAddress = mp.StreetName;
                            //doHdr.UnitNo = ;
                            doHdr.Remark = "Created from shopify";
                            doHdr.IsVendorDelivery = false;
                            doHdr.DeliveryDate = null;
                            var deliveryDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value.Date;
                            doHdr.TimeSlotFrom = null;
                            doHdr.TimeSlotTo = null;
                            delOrderHdrColl.Add(doHdr);

                            DeliveryOrderDetail doDet = new DeliveryOrderDetail();
                            doDet.Dohdrid = hdrNo.ToString();
                            //PowerPOS.Item it = new Item(Item.UserColumns.MappedItemNo, det.sku);

                            doDet.ItemNo = det.ItemNo;
                            doDet.Quantity = det.Quantity;
                            doDet.DetailsID = doDet.Dohdrid + "." + detNo.ToString();
                            doDet.OrderDetID = det.OrderDetID;
                            delOrderDetColl.Add(doDet);
                        }
                        DeliveryOrderController.SaveMultipleOrder(ref delOrderHdrColl, ref delOrderDetColl);

                        // Update OrderDet.DepositAmount
                        pos.myOrderDet.SaveAll();
                    }

                }


                isSuccess = true;

            }
            catch (Exception ex)
            {
                status = ex.Message;
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }
    }

    public class discount
    {
        public string code { set; get; }
        public decimal? amount { set; get; }
        public string type { set; get; }
    }

    public class propertiesobj
    {
        public string name { set; get; }
        public string value { set; get; }
    }

    public class line_item
    {
        public long? id { set; get; }
        public long? variant_id { set; get; }
        public string title { set; get; }
        public int? quantity { set; get; }
        public decimal? price { set; get; }
        public decimal? grams { set; get; }
        public string sku { set; get; }
        public string variant_title { set; get; }
        public string vendor { set; get; }
        public string fulfillment_service { set; get; }
        public long? product_id { set; get; }
        public string requires_shipping { set; get; }
        public bool? taxable { set; get; }
        public string gift_card { set; get; }
        public string name { set; get; }
        public string variant_inventory_management { set; get; }
        public List<propertiesobj> properties { set; get; }
        public string product_exists { set; get; }
        public long? fulfillable_quantity { set; get; }
        public decimal? total_discount { set; get; }
        public string fulfillment_status { set; get; }
        //public string tax_lines { set; get; } 
    }

    public class address
    {
        public string first_name { set; get; }
        public string address1 { set; get; }
        public string phone { set; get; }
        public string city { set; get; }
        public string zip { set; get; }
        public string province { set; get; }
        public string country { set; get; }
        public string last_name { set; get; }
        public string address2 { set; get; }
        public string company { set; get; }
        public string latitude { set; get; }
        public string longitude { set; get; }
        public string name { set; get; }
        public string country_code { set; get; }
        public string province_code { set; get; }
    }

    public class default_address
    {
        public long? id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public string province_code { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
    }

    public class customer
    {
        public long? id { set; get; }
        public string email { set; get; }
        public string accepts_marketing { set; get; }
        public DateTime? created_at { set; get; }
        public DateTime? updated_at { set; get; }
        public string first_name { set; get; }
        public string last_name { set; get; }
        public int? orders_count { set; get; }
        public string state { set; get; }
        public decimal? total_spent { set; get; }
        public long? last_order_id { set; get; }
        public string note { set; get; }
        public bool? verified_email { set; get; }
        public string multipass_identifier { set; get; }
        public bool? tax_exempt { set; get; }
        public string phone { set; get; }
        public string tags { set; get; }
        public string last_order_name { set; get; }
        public address default_address { set; get; }
    }

    public class shipping
    {
        public long? id { set; get; }
        public string title { set; get; }
        public decimal? price { set; get; }
        public string code { set; get; }
        public string source { set; get; }
        public string phone { set; get; }
        public string requested_fulfillment_service_id { set; get; }
        public string delivery_category { set; get; }
        public string carrier_identifier { set; get; }
        //public string tax_lines { set; get; }
    }

    public class attribute
    {
        public string name { set; get; }
        public string value { set; get; }
    }

    public class order
    {
        public long? id { set; get; }
        public string email { set; get; }
        public string closed_at { set; get; }
        public DateTime? created_at { set; get; }
        public DateTime? updated_at { set; get; }
        public int? number { set; get; }
        public string note { set; get; }
        public string token { set; get; }
        public string gateway { set; get; }
        public string test { set; get; }
        public decimal? total_price { set; get; }
        public decimal? subtotal_price { set; get; }
        public decimal? total_weight { set; get; }
        public decimal? total_tax { set; get; }
        public bool? taxes_included { set; get; }
        public string currency { set; get; }
        public string financial_status { set; get; }
        public bool? confirmed { set; get; }
        public decimal? total_discounts { set; get; }
        public decimal? total_line_items_price { set; get; }
        public string cart_token { set; get; }
        public string buyer_accepts_marketing { set; get; }
        public string name { set; get; }
        public string referring_site { set; get; }
        public string landing_site { set; get; }
        public string cancelled_at { set; get; }
        public string cancel_reason { set; get; }
        public decimal? total_price_usd { set; get; }
        public string checkout_token { set; get; }
        public string reference { set; get; }
        public long? user_id { set; get; }
        public string location_id { set; get; }
        public string source_identifier { set; get; }
        public string source_url { set; get; }
        public DateTime? processed_at { set; get; }
        public string device_id { set; get; }
        public string phone { set; get; }
        public string customer_locale { set; get; }
        public string browser_ip { set; get; }
        public string landing_site_ref { set; get; }
        public long? order_number { set; get; }
        public List<discount> discount_codes { set; get; }
        public List<attribute> note_attributes { set; get; }
        public List<string> payment_gateway_names { set; get; }
        public string processing_method { set; get; }
        public string checkout_id { set; get; }
        public string source_name { set; get; }
        public string fulfillment_status { set; get; }
        //public string tax_lines { set; get; }
        public string tags { set; get; }
        public string contact_email { set; get; }
        public string order_status_url { set; get; }
        public List<line_item> line_items { set; get; }
        public List<shipping> shipping_lines { set; get; }
        public address billing_address { set; get; }
        public address shipping_address { set; get; }
        //public string fulfillments { set; get; }
        //public string refunds { set; get; }
        public customer customer { set; get; }
    }



    public enum ShopifyEvent
    {
        CheckoutCreation,
        CheckoutDeletion,
        CheckoutUpdate,
        FulfillmentCreation,
        FulfillmentUpdate,
        OrderCancellation,
        OrderCreation,
        OrderDeletion,
        OrderFulfillment,
        OrderPayment,
        OrderUpdate,
        RefundCreate
    }
}
