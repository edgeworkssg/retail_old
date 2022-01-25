using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using PowerPOS;
using System.Web.UI;
using System.Web.SessionState;
using PowerPOSLib.Container;
using Newtonsoft.Json;

namespace PowerWeb.Product.Action
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Update : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            JsonResult result = new JsonResult()
            {
                Status = false,
                Message = "",
                Details = "",
                Data = null
            };
            try
            {
                string itemNo = context.Request.Params["ItemNo"];
                string itemName = context.Request.Params["ItemName"];
                string barcode = context.Request.Params["Barcode"];
                string categoryName = context.Request.Params["CategoryName"];
                string factoryPrice = context.Request.Params["FactoryPrice"];
                string retailPrice = context.Request.Params["RetailPrice"];
                string isNonDiscountable = context.Request.Params["IsNonDiscountable"] ?? "false";
                string GST = context.Request.Params["GST"];
                string isCommission = context.Request.Params["IsCommission"] ?? "false";
                string pointRedeemable = context.Request.Params["PointRedeemable"] ?? "false";
                string pointsGet = context.Request.Params["PointsGet"];
                string timesGet = context.Request.Params["TimesGet"];
                string breakdownPrice = context.Request.Params["BreakdownPrice"];
                string itemDescription = context.Request.Params["ItemDescription"];
                string attributes1 = context.Request.Params["Attributes1"];
                string attributes2 = context.Request.Params["Attributes2"];
                string attributes3 = context.Request.Params["Attributes3"];
                string attributes4 = context.Request.Params["Attributes4"];
                string attributes5 = context.Request.Params["Attributes5"];
                string remark = context.Request.Params["Remark"];
                string productType = context.Request.Params["ProductType"];

                System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;

                using (System.Transactions.TransactionScope transScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                {
                    Item item;
                    ItemController itemLogic = new ItemController();

                    if (itemLogic.CheckIfBarcodeExists(barcode, itemNo))
                    {
                        result.Status = true;
                        result.Message = "Barcode is duplicated!";
                    }
                    else
                    {
                        item = new Item(itemNo);
                        item.IsNew = false;
                        item.Barcode = barcode;
                        item.ItemName = itemName;
                        item.ItemDesc = itemDescription;
                        item.CategoryName = categoryName;
                        item.RetailPrice = decimal.Parse(retailPrice.Replace(",", ""));
                        item.FactoryPrice = decimal.Parse(factoryPrice.Replace(",", ""));
                        item.IsNonDiscountable = (isNonDiscountable == "true") ? true : false;
                        item.IsCommission = (isCommission == "true") ? true : false;
                        item.Userflag1 = false;

                        if (productType == "Service")
                        {
                            item.IsInInventory = false;
                            item.IsServiceItem = true;
                            item.PointGetAmount = 0;
                            item.PointGetMode = Item.PointMode.None;
                            item.PointRedeemAmount = 0;
                            item.PointRedeemMode = ((pointRedeemable.ToLower() == "true") ? Item.PointMode.Dollar : Item.PointMode.None);
                            item.Userfloat3 = null; /// Course Breakdown Price
                        }
                        else if (productType == "PointPackage")
                        {
                            item.IsInInventory = false;
                            item.IsServiceItem = false;
                            decimal tempDec = 0; decimal.TryParse(pointsGet, out tempDec);
                            item.PointGetAmount = tempDec;
                            item.PointGetMode = Item.PointMode.Dollar;
                            item.PointRedeemAmount = 0;
                            item.PointRedeemMode = Item.PointMode.None;
                            item.Userfloat3 = null; /// Course Breakdown Price
                        }
                        else if (productType == "CoursePackage")
                        {
                            item.IsInInventory = false;
                            item.IsServiceItem = false;
                            decimal tempDec = 0; decimal.TryParse(timesGet, out tempDec);
                            item.PointGetAmount = tempDec;
                            item.PointGetMode = Item.PointMode.Times;
                            item.PointRedeemAmount = 0;
                            item.PointRedeemMode = Item.PointMode.None;
                            decimal.TryParse(breakdownPrice, out tempDec);
                            item.Userfloat3 = tempDec; /// Course Breakdown Price
                        }
                        else if (productType == "OpenPriceProduct")
                        {
                            item.IsInInventory = true;
                            item.IsServiceItem = true;
                            item.PointGetAmount = 0;
                            item.PointGetMode = Item.PointMode.None;
                            item.PointRedeemAmount = 0;
                            item.PointRedeemMode = ((pointRedeemable.ToLower() == "true") ? Item.PointMode.Dollar : Item.PointMode.None);
                            item.Userfloat3 = null; /// Course Breakdown Price
                        }
                        else /// Categorized as Product
                        {
                            item.IsInInventory = true;
                            item.IsServiceItem = false;
                            item.PointGetAmount = 0;
                            item.PointGetMode = Item.PointMode.None;
                            item.PointRedeemAmount = 0;
                            item.PointRedeemMode = (pointRedeemable.ToLower() == "true" ? Item.PointMode.Dollar : Item.PointMode.None);
                            item.Userfloat3 = null; /// Course Breakdown Price
                        }

                        item.GSTRule = (int?)Int32.Parse(GST);
                        item.Attributes1 = attributes1;
                        item.Attributes2 = attributes2;
                        item.Attributes3 = attributes3;
                        item.Attributes4 = attributes4;
                        item.Attributes5 = attributes5;

                        item.Remark = remark;
                        item.Deleted = false;
                        item.Save(context.Session["username"].ToString());
                        transScope.Complete();

                        result.Status = true;
                        result.Message = "Product has been saved.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Cannot save product.";
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(result));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
