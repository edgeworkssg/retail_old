using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.SessionState;
using Newtonsoft.Json;
using PowerPOSLib.Container;
using PowerPOS;

namespace PowerWeb.API.Product.ItemImporter
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Save : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string dataID = context.Request.Params["DataID"];
            DataTable dt = (DataTable)context.Session[dataID];
            string errorMessage = "";
            int iterator = 2;
            JsonResult result = new JsonResult()
            {
                Status = false,
                Message = "",
                Details = "",
                Data = null
            };

            foreach (DataRow item in dt.Rows)
            {
                string department = item["Department"].ToString();
                string category = item["Category"].ToString();
                string itemNo = item["ItemNo"].ToString();
                string itemName = item["ItemName"].ToString();
                string barcode = item["Barcode"].ToString();
                string serviceItemRaw = item["ServiceItem"].ToString();
                string inventoryItemRaw = item["InventoryItem"].ToString();
                string costPriceRaw = item["CostPrice"].ToString();
                string retailPriceRaw = item["RetailPrice"].ToString();
                decimal retailPrice = 0;
                decimal costPrice = 0;
                string nonDiscountableRaw = item["NonDiscountable"].ToString();
                string giveCommissionRaw = item["GiveCommission"].ToString();
                bool giveCommission = false;
                bool nonDiscountable = false;
                bool inventoryItem = false;
                bool serviceItem = false;
                decimal openingBalance = 0;
                string openingBalanceRaw = item["OpeningBalance"].ToString();

                if (string.IsNullOrEmpty(department) == true)
                {
                    errorMessage += "<br />B" + iterator + "\t: Department cannot be null.";
                }

                if (string.IsNullOrEmpty(category) == true)
                {
                    errorMessage += "<br />C" + iterator + "\t: Category cannot be null.";
                }

                if (string.IsNullOrEmpty(itemNo) == true)
                {
                    errorMessage += "<br />D" + iterator + "\t: Item No. cannot be null.";
                }

                if (string.IsNullOrEmpty(itemName) == true)
                {
                    errorMessage += "<br />E" + iterator + "\t: Item Name cannot be null.";
                }

                if (string.IsNullOrEmpty(barcode) == true)
                {
                    errorMessage += "<br />F" + iterator + "\t: Barcode cannot be null.";
                }

                if (string.IsNullOrEmpty(retailPriceRaw) == true)
                {
                    errorMessage += "<br />G" + iterator + "\t: Retail price cannot be null.";
                }
                else
                {
                    try
                    {
                        retailPrice = Decimal.Parse(retailPriceRaw);
                    }
                    catch (Exception ex)
                    {
                        errorMessage += "<br />G" + iterator + "\t: Retail price not in correct format.";
                    }
                }

                if (string.IsNullOrEmpty(costPriceRaw) == true)
                {
                    errorMessage += "<br />H" + iterator + "\t: Cost price cannot be null.";
                }
                else
                {
                    try
                    {
                        costPrice = Decimal.Parse(costPriceRaw);
                    }
                    catch (Exception ex)
                    {
                        errorMessage += "<br />H" + iterator + "\t: Cost price not in correct format.";
                    }
                }

                if (string.IsNullOrEmpty(serviceItemRaw) == true)
                {
                    errorMessage += "<br />I" + iterator + "\t: Service Item cannot be null."; 
                }
                else if (string.IsNullOrEmpty(inventoryItemRaw) == true)
                {
                    errorMessage += "<br />J" + iterator + "\t: Inventory Item cannot be null.";
                }
                else if (string.IsNullOrEmpty(serviceItemRaw) == false && string.IsNullOrEmpty(inventoryItemRaw) == false && serviceItemRaw.ToLower() == "yes" && inventoryItemRaw.ToLower() == "yes")
                {
                    errorMessage += "<br />I-J : " + iterator + "\t: Please select 'Yes' only once out of 2 columns (Inventory Item or Service Item) ";
                }
                else if (string.IsNullOrEmpty(serviceItemRaw) == false && string.IsNullOrEmpty(inventoryItemRaw) == false)
                {
                    if (serviceItemRaw.ToLower() == "yes")
                    {
                        serviceItem = true;
                    }

                    if (inventoryItemRaw.ToLower() == "yes")
                    {
                        inventoryItem = true;
                    }
                }

                if (string.IsNullOrEmpty(nonDiscountableRaw) == true)
                {
                    errorMessage += "<br />K" + iterator + "\t: Non Discountable cannot be null.";
                }
                else if(nonDiscountableRaw.ToLower() == "yes")
                {
                    nonDiscountable = true;                    
                }

                if (string.IsNullOrEmpty(giveCommissionRaw) == true)
                {
                    errorMessage += "<br />K" + iterator + "\t: Give commission cannot be null.";
                }
                else if (giveCommissionRaw.ToLower() == "yes")
                {
                    giveCommission = true;
                }

                iterator++;
            }

            if (errorMessage.Length > 0)
            {
                result.Status = false;
                result.Message = "Following rows are not filled correctly : <br /><br />" + errorMessage;
            }
            else
            {
                result = SaveData(dt, context.Session["username"].ToString());
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(result));
        }

        private JsonResult SaveData(DataTable dt, string username)
        {
            JsonResult result = new JsonResult();
            result.Status = false;
            result.Message = "";
            result.Details = "";
            result.Data = null;

            try
            {
                System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                using (System.Transactions.TransactionScope transScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        string department = item["Department"].ToString();
                        string category = item["Category"].ToString();
                        string itemNo = item["ItemNo"].ToString();
                        string itemName = item["ItemName"].ToString();
                        string barcode = item["Barcode"].ToString();
                        string serviceItemRaw = item["ServiceItem"].ToString();
                        string inventoryItemRaw = item["InventoryItem"].ToString();
                        string costPriceRaw = item["CostPrice"].ToString();
                        string retailPriceRaw = item["RetailPrice"].ToString();
                        decimal retailPrice = 0;
                        decimal costPrice = 0;
                        string nonDiscountableRaw = item["NonDiscountable"].ToString();
                        string giveCommissionRaw = item["GiveCommission"].ToString();
                        bool giveCommission = false;
                        bool nonDiscountable = false;
                        bool inventoryItem = false;
                        bool serviceItem = false;
                        decimal openingBalance = 0;
                        string openingBalanceRaw = item["OpeningBalance"].ToString();

                        try
                        {
                            retailPrice = Decimal.Parse(retailPriceRaw);
                        }
                        catch (Exception ex)
                        {
                        }

                        try
                        {
                            costPrice = Decimal.Parse(costPriceRaw);
                        }
                        catch (Exception ex)
                        {
                        }

                        if (string.IsNullOrEmpty(serviceItemRaw) == false && string.IsNullOrEmpty(inventoryItemRaw) == false)
                        {
                            if (serviceItemRaw.ToLower() == "yes")
                            {
                                serviceItem = true;
                            }

                            if (inventoryItemRaw.ToLower() == "yes")
                            {
                                inventoryItem = true;
                            }
                        }

                        if (nonDiscountableRaw.ToLower() == "yes")
                        {
                            nonDiscountable = true;
                        }

                        if (giveCommissionRaw.ToLower() == "yes")
                        {
                            giveCommission = true;
                        }

                        PowerPOS.Item itemC = new PowerPOS.Item();

                        itemC.IsNew = true;
                        itemC.ItemNo = itemNo;
                        itemC.ItemName = itemName;
                        itemC.ItemDesc = itemName;
                        itemC.CategoryName = category;
                        itemC.FactoryPrice = costPrice;
                        itemC.RetailPrice = retailPrice;
                        itemC.MinimumPrice = retailPrice;
                        itemC.IsNonDiscountable = nonDiscountable;
                        itemC.IsInInventory = inventoryItem;
                        itemC.IsServiceItem = serviceItem;

                        itemC.Save(username);
                    }

                    transScope.Complete();   
                }

                result.Status = true;
                result.Message = "Item imported successfully.";
            }
            catch (Exception)
            {
                result.Status = false;
                result.Message = "Cannot import item from template. Please contact administrator.";
            }

            return result;
        }

        private void DeleteSavedItem()
        {
              
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
