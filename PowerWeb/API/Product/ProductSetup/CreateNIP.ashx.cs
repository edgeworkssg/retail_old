using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PowerPOSLib.Container;
using PowerPOS;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace PowerWeb.API.Product.ProductSetup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CreateNIP : IHttpHandler, IReadOnlySessionState
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
                 string factoryPrice = context.Request.Params["FactoryPrice"];
                 string retailPrice = context.Request.Params["RetailPrice"];
                 string UOM = context.Request.Params["UOM"];
                 string SizeConvType = context.Request.Params["SizeConvType"];
                 string SizeConv = context.Request.Params["SizeConv"];
                 string DeductedItemNo = context.Request.Params["DeductedItemNo"];

                 System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                 to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;

                 using (System.Transactions.TransactionScope transScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                 {
                     PowerPOS.Item item;
                     ItemController itemLogic = new ItemController();

                     if (itemLogic.CheckIfBarcodeExists(barcode, itemNo))
                     {
                         throw new Exception("Barcode is duplicated!");
                     }
                     else if(itemLogic.CheckIfUOMExist(UOM, DeductedItemNo))
                     {
                        throw new Exception("UOM is duplicated!");
                     }
                     else
                     {
                         PowerPOS.Item MainItem = new PowerPOS.Item(DeductedItemNo);

                         item = new PowerPOS.Item();

                         if (!item.IsNew)
                             throw new Exception(string.Format("Product with Item No {0} already exist", itemNo));

                         item.ItemNo = itemNo;
                         item.IsNew = true;
                         item.Barcode = barcode;
                         item.ItemName = itemName;
                         item.ItemDesc = itemName;
                         item.UniqueID = Guid.NewGuid();
                         item.CategoryName = MainItem.CategoryName;
                         item.RetailPrice = decimal.Parse(retailPrice.Replace(",", ""));
                         item.FactoryPrice = decimal.Parse(factoryPrice.Replace(",", ""));
                         item.IsNonDiscountable = false;
                         item.IsCommission = false;
                         item.Userflag1 = false;
                         item.UOM = UOM;

                         item.IsInInventory = false;
                         item.IsServiceItem = false;
                         item.NonInventoryProduct = true;
                         item.PointGetAmount = 0;
                         item.PointGetMode = PowerPOS.Item.PointMode.None;
                         item.PointRedeemAmount = 0;
                         item.PointRedeemMode = PowerPOS.Item.PointMode.None;
                         item.Userfloat3 = null; /// Course Breakdown Price
                         item.DeductedItem = DeductedItemNo;
                         decimal tempDec = 0; decimal.TryParse(SizeConv, out tempDec);
                         item.DeductConvRate = tempDec;
                         item.DeductConvType = SizeConvType.ToUpper() == "DOWN";

                         item.GSTRule = MainItem.GSTRule;
                         item.Attributes1 = "";
                         item.Attributes2 = "";
                         item.Attributes3 = "";
                         item.Attributes4 = "";
                         item.Attributes5 = "";
                         item.Remark = "";

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
                 result.Message = "Cannot save product. Error :" + ex.Message;
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
