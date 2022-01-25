using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using PowerPOSLib.Container;
using System.Web.SessionState;
using PowerPOS;
using Newtonsoft.Json;
using SubSonic;

namespace PowerWeb.API.Product.ProductSetup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UpdateMatrix : IHttpHandler, IReadOnlySessionState
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
                string attributes3 = context.Request.Params["Attributes3"];
                string attributes4 = context.Request.Params["Attributes4"];
                string remark = context.Request.Params["Remark"];
                string productType = context.Request.Params["ProductType"];

                System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;

                using (System.Transactions.TransactionScope transScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                {
                    ItemController itemLogic = new ItemController();

                    var listAtt3 = attributes3.Split(',');
                    var listAtt4 = attributes4.Split(',');
                    var runningnumber = 1;

                    if (itemLogic.CheckIfBarcodeExistsMatrix(barcode, itemNo))
                    {
                        result.Status = false;
                        result.Message = "Barcode is duplicated!";
                    }
                    else
                    {   
                        //delete item that are not in product setup
                        Query qr = new Query("Item");
                        qr.QueryType = QueryType.Update;
                        qr.AddUpdateSetting("Deleted", true);
                        qr.AddWhere("Attributes1", Comparison.Equals, itemNo);
                        qr.AddWhere("Attributes3", Comparison.NotIn, attributes3);
                        qr.AddWhere("Attributes4", Comparison.NotIn, attributes4);
                        qr.Execute();

                        //update the data change existing and add new
                        foreach (var itema in listAtt3)
                        {
                            foreach (var itemb in listAtt4)
                            {
                                var existingitemno = "";
                                Item item;
                                Query qrs = new Query("Item");
                                qrs.QueryType = QueryType.Select;
                                qrs.SelectList = "ItemNo";
                                qrs.AddWhere("Attributes1", Comparison.Equals, itemNo);
                                qrs.AddWhere("Attributes3", Comparison.Equals, itema);
                                qrs.AddWhere("Attributes4", Comparison.Equals, itemb);
                                DataSet ds = qrs.ExecuteDataSet();
                                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    existingitemno = ds.Tables[0].Rows[0]["ItemNo"].ToString();
                                    item = new Item(existingitemno);
                                }
                                else 
                                {
                                    runningnumber = ItemController.getNewRunningNumberMatrix(itemNo);
                                    var itemno = itemNo + runningnumber.ToString("0#");

                                    item = new Item();
                                    item.ItemNo = itemno;
                                    item.IsNew = true;
                                    item.UniqueID = Guid.NewGuid();
                                }
                                
                                item.ItemDesc = itemDescription;
                                item.Barcode = barcode;
                                item.ItemName = string.Format("{0}-{1}-{2}", itemName, itema, itemb);
                                item.CategoryName = categoryName;
                                item.RetailPrice = decimal.Parse(retailPrice.Replace(",", ""));
                                item.FactoryPrice = decimal.Parse(factoryPrice.Replace(",", ""));
                                item.IsNonDiscountable = (isNonDiscountable == "true") ? true : false;
                                item.IsCommission = (isCommission == "true") ? true : false;
                                item.Userflag1 = true;

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
                                    item.PointRedeemMode = (pointRedeemable == "true" ? Item.PointMode.Dollar : Item.PointMode.None);
                                    item.Userfloat3 = null; /// Course Breakdown Price
                                }

                                item.GSTRule = (int?)Int32.Parse(GST);
                                item.Attributes1 = itemNo;
                                item.Attributes2 = itemName;
                                item.Attributes3 = itema;
                                item.Attributes4 = itemb;
                                item.Remark = remark;
                                item.Deleted = false;
                                item.Save(context.Session["username"].ToString());

                                runningnumber++;

                            }
                        }

                        transScope.Complete();

                        result.Status = true;
                        result.Message = "Product has been saved.";
                        result.Data = new
                        {
                            ItemNo = itemNo
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.InnerException.ToString();
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
