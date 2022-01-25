using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using PowerPOSLib.Container;
using PowerPOS;
using Newtonsoft.Json;
using SubSonic.Utilities;
using SubSonic;

namespace PowerWeb.API.Product.Item
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UpdateFactoryPrice : IHttpHandler
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
                QueryCommandCollection cmdColl = new QueryCommandCollection();
                string itemNo = context.Request.Params["ItemNo"] ?? "";
                string factoryPrice = context.Request.Params["FactoryPrice"] ?? "0.00";
                string userName = context.Request.Params["Username"] ?? "";
                string supplier = context.Request.Params["Supplier"] ?? "";
                string strSql = @"update Item 
                                     set FactoryPrice = " + factoryPrice + @"
                                       , ModifiedOn = getdate()
                                       , ModifiedBy = '" + userName + @"'
                                   where ItemNo = '" + itemNo + @"'
                                ";
                cmdColl.Add(new QueryCommand(strSql, "PowerPOS"));

                strSql = @"update Item set FactoryPrice = userfloat5 * " + factoryPrice + @", ModifiedOn = getdate()
                                , ModifiedBy = '" + userName + @"' 
                            where Itemno in (select ItemNo from Item where userfld8 = '" + itemNo + @"') 
                          ";
                cmdColl.Add(new QueryCommand(strSql, "PowerPOS"));

                if (!string.IsNullOrEmpty(supplier))
                {
                    strSql = @"
                                update ItemSupplierMap
                                set CostPrice = " + factoryPrice + @"
                                  , ModifiedOn = getdate()
                                  , ModifiedBy = '" + userName + @"'
                                where ItemNo = '" + itemNo + @"'
                                  and SupplierID = " + supplier + @"
                              ";
                    cmdColl.Add(new QueryCommand(strSql, "PowerPOS"));

                    strSql = @"update itemsuppliermap set itemsuppliermap.costprice = item.userfloat5 * " + factoryPrice + @", itemsuppliermap.modifiedon = getdate()
                                , ModifiedBy = '" + userName + @"' 
                               from item 
                                where itemsuppliermap.itemno = item.itemno and itemsuppliermap.itemno in (select itemno from item where userfld8 = '" + itemNo + @"') 
                                and SupplierID = " + supplier + @"";
                    cmdColl.Add(new QueryCommand(strSql, "PowerPOS"));
                    Logger.writeLog(strSql);
                }

                DataService.ExecuteTransaction(cmdColl);

                result.Status = true;
                result.Message = "Item cost price updated!";
            }
            catch (Exception)
            {
                result.Status = false;
                result.Message = "Cannot update item cost price!";
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
