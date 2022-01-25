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
using System.Collections.Specialized;
using SubSonic;

namespace PowerWeb.API.Product.ItemImporter
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SaveData : IHttpHandler, IRequiresSessionState
    {
        string defaultGSTRule = "";

        public void ProcessRequest(HttpContext context)
        {
            string dataID = context.Request.Params["DataID"];
            defaultGSTRule = context.Request.Params["DefaultGSTRule"];

            switch (defaultGSTRule)
            {
                case  "3":
                    defaultGSTRule = "Non GST";
                    break;

                case "2":
                    defaultGSTRule = "Inclusive GST";
                    break;
                    
                case  "1":
                    defaultGSTRule = "Exclusive GST";
                    break;
            }

            DataTable dt = (DataTable)context.Session[dataID];
            string errorMessage = "";
            int iterator = 2;
            bool isError = false;

            JsonResult result = new JsonResult()
            {
                Status = false,
                Message = "",
                Details = "",
                Data = null
            };

            ArrayList processedData = new ArrayList();
            ArrayList itemNoList = new ArrayList();
            ArrayList barCodeList = new ArrayList();
            NameValueCollection categoryDeptPair = new NameValueCollection(); 
            ItemController itemLogic = new ItemController();

            foreach (DataRow item in dt.Rows)
            {
                errorMessage = "";
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
                string currentTime = DateTime.Now.ToString("yyyy-MM-dd");
                string gstRule = (item["GSTRule"] ?? "Non GST").ToString();

                string strSql = "select count(*) from ItemDepartment a where a.DepartmentName = '" + department + "'";
                QueryCommand cmd = new QueryCommand(strSql, "PowerPOS");
                int recordCount = (int)DataService.ExecuteScalar(cmd);

                if (recordCount < 1)
                {
                    string strInsertNewData = "insert into ItemDepartment (ItemDepartmentID, DepartmentName, Remark, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, Deleted) "
                                            + "values('" + department + "', '" + department + "', '-', '" + currentTime  +"', '" + currentTime + "', 'Item Importer', 'Item Importer', 0)";
                    QueryCommand cmdInsertNewData = new QueryCommand(strInsertNewData, "PowerPOS");
                    DataService.ExecuteQuery(cmdInsertNewData);
                }

                strSql = "select count(*) from Category a where a.CategoryName = '" + category + "'";
                cmd = new QueryCommand(strSql, "PowerPOS");
                recordCount = (int)DataService.ExecuteScalar(cmd);

                if (recordCount < 1)
                {
                    string strInsertNewData = "insert into Category (CategoryName, Remarks, Category_ID, IsDiscountable, IsForSale, IsGST, AccountCategory, ItemDepartmentID, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Deleted) "
                                            + "values ('" + category + "', '', '', 1, 1, 1, '', '" + department + "', 'Item Importer', '" + currentTime + "', 'Item Importer', '" + currentTime + "', 0) ";
                    QueryCommand cmdInsertNewData = new QueryCommand(strInsertNewData, "PowerPOS");
                    DataService.ExecuteQuery(cmdInsertNewData);
                }

                if (categoryDeptPair.AllKeys.Contains(category) == false)
                {
                    categoryDeptPair.Add(category, department);     
                }
                else if (categoryDeptPair[category] != department)
                {
                    errorMessage += "There are category with the same value, but the Department are different.";
                }

                if (itemNoList.Contains(itemNo) == false)
                {
                    itemNoList.Add(itemNo);
                }
                else
                {
                    errorMessage += "There are record in excel file that has same Item number.<br />";
                }

                if (barCodeList.Contains(barcode) == false)
                {
                    barCodeList.Add(barcode);
                }
                else
                {
                    errorMessage += "There are record in excel file that has same barcode.<br />";
                }

                if (itemLogic.CheckIfBarcodeExists(barcode, itemNo))
                {
                    errorMessage += "There are record in database with the same barcode.<br />";
                }

                //if (itemLogic.CheckIfItemNoExists(itemNo))
                //{
                //    errorMessage += "There are record in database with the same Item No.<br />";
                //}

                if (string.IsNullOrEmpty(department) == true)
                {
                    errorMessage += "Department cannot be null.<br />";
                }

                if (string.IsNullOrEmpty(category) == true)
                {
                    errorMessage += "Category cannot be null.<br />";
                }

                if (string.IsNullOrEmpty(itemNo) == true)
                {
                    errorMessage += "Item No. cannot be null.<br />";
                }

                if (string.IsNullOrEmpty(itemName) == true)
                {
                    errorMessage += "Item Name cannot be null.<br />";
                }

                if (string.IsNullOrEmpty(barcode) == true)
                {
                    errorMessage += "Barcode cannot be null.<br />";
                }

                if (string.IsNullOrEmpty(retailPriceRaw) == true)
                {
                    errorMessage += "Retail price cannot be null.<br />";
                }
                else
                {
                    try
                    {
                        retailPrice = Decimal.Parse(retailPriceRaw);
                    }
                    catch (Exception ex)
                    {
                        errorMessage += "Retail price not in correct format.<br />";
                    }
                }

                if (string.IsNullOrEmpty(costPriceRaw) == true)
                {
                    errorMessage += "Cost price cannot be null.<br />";
                }
                else
                {
                    try
                    {
                        costPrice = Decimal.Parse(costPriceRaw);
                    }
                    catch (Exception ex)
                    {
                        errorMessage += "Cost price not in correct format.<br />";
                    }
                }

                if (string.IsNullOrEmpty(serviceItemRaw) == true)
                {
                    errorMessage += "Service Item cannot be null.<br />";
                }
                else if (string.IsNullOrEmpty(inventoryItemRaw) == true)
                {
                    errorMessage += "Inventory Item cannot be null.<br />";
                }
                else if (string.IsNullOrEmpty(serviceItemRaw) == false && string.IsNullOrEmpty(inventoryItemRaw) == false && serviceItemRaw.ToLower() == "yes" && inventoryItemRaw.ToLower() == "yes")
                {
                    errorMessage += "Please select 'Yes' only once out of 2 columns (Inventory Item or Service Item) <br />";
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
                    errorMessage += "Non Discountable cannot be null.<br />";
                }
                else if (nonDiscountableRaw.ToLower() == "yes")
                {
                    nonDiscountable = true;
                }

                if (string.IsNullOrEmpty(giveCommissionRaw) == true)
                {
                    errorMessage += "Give commission cannot be null.<br />";
                }
                else if (giveCommissionRaw.ToLower() == "yes")
                {
                    giveCommission = true;
                }
                item["Status"] = errorMessage;



                processedData.Add(new
                {
                    Status = new { 
                        errorMessage = errorMessage,
                        elementID = Guid.NewGuid().ToString()
                    },
                    Active = item["Active"],
                    Department = item["Department"],
                    Category = item["Category"],
                    ItemNo = item["ItemNo"],
                    ItemName = item["ItemName"],
                    Barcode = item["Barcode"],
                    RetailPrice = item["RetailPrice"],
                    CostPrice = item["CostPrice"],
                    ServiceItem = item["ServiceItem"],
                    InventoryItem = item["InventoryItem"],
                    NonDiscountable = item["NonDiscountable"],
                    GiveCommission = item["GiveCommission"],
                    OpeningBalance = item["OpeningBalance"],
                    Attributes1 = item["Attributes1"],
                    Attributes2 = item["Attributes2"],
                    Attributes3 = item["Attributes3"],
                    Attributes4 = item["Attributes4"],
                    Attributes5 = item["Attributes5"],
                    GSTRule = item["GSTRule"],
                });

                if (errorMessage.Length > 0)
                {
                    isError = true;
                }

                iterator++;
            }

            if (isError == true)
            {
                result.Status = false;
                //result.Message = "Following rows are not filled correctly : <br /><br />" + errorMessage;
                result.Message = "Cannot saving product from excel. Please check error messages provided within the grid.";
                context.Session["SavingItemState"] = null;
            }
            else
            {
                result = SaveUploadedData(ref dt, context.Session["username"].ToString());

                if (result.Status == true)
                {
                    context.Session["SavingItemState"] = true;
                }
                else
                {
                    context.Session["SavingItemState"] = false;
                }
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(processedData));
        }

        private JsonResult SaveUploadedData(ref DataTable dt, string username)
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
                    QueryCommandCollection cmds = new QueryCommandCollection();
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
                        string attributes1 = item["Attributes1"].ToString();
                        string attributes2 = item["Attributes2"].ToString();
                        string attributes3 = item["Attributes3"].ToString();
                        string attributes4 = item["Attributes4"].ToString();
                        string attributes5 = item["Attributes5"].ToString();
                        string gstRule = (item["GSTRule"] ?? "Non GST").ToString();

                        if (string.IsNullOrEmpty(defaultGSTRule) == false)
                        {
                            gstRule = defaultGSTRule;
                        }

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
                        else
                        {
                            nonDiscountable = false;
                        }

                        if (giveCommissionRaw.ToLower() == "yes")
                        {
                            giveCommission = true;
                        }
                        else
                        {
                            giveCommission = false;
                        }

                        string strSql = "select count(*) from Item a where a.ItemNo = '" + itemNo + "'";
                        int recordCount = (int)DataService.ExecuteScalar(new QueryCommand(strSql, "PowerPOS"));

                        bool isNewRecord = false;
                        PowerPOS.Item itemC;
                        if (recordCount > 0)
                        {
                            itemC = new PowerPOS.Item(itemNo);
                            itemC.IsNew = false;
                            isNewRecord = false;
                        }
                        else
                        {
                            itemC = new PowerPOS.Item();
                            itemC.ItemNo = itemNo;
                            itemC.IsNew = true;
                            isNewRecord = true;
                        }

                        itemC.ItemName = itemName;
                        itemC.ItemDesc = itemName;
                        itemC.Barcode = barcode;
                        itemC.CategoryName = category;
                        itemC.FactoryPrice = costPrice;
                        itemC.RetailPrice = retailPrice;
                        itemC.MinimumPrice = retailPrice;
                        itemC.IsNonDiscountable = nonDiscountable;
                        itemC.IsInInventory = inventoryItem;
                        itemC.IsServiceItem = serviceItem;
                        itemC.IsCommission = giveCommission;
                        itemC.Attributes1 = attributes1;
                        itemC.Attributes2 = attributes2;
                        itemC.Attributes3 = attributes3;
                        itemC.Attributes4 = attributes4;
                        itemC.Attributes5 = attributes5;
                        itemC.Deleted = false;

                        switch (gstRule)
                        {
                            case "Non GST" :
                                itemC.GSTRule = 3;
                                break;

                            case "Inclusive GST":
                                itemC.GSTRule = 2;
                                break;

                            case "Exclusive GST":
                                itemC.GSTRule = 1;
                                break;
                        }

                        QueryCommand cmd;
                        if (isNewRecord == false)
                        {
                            cmd = itemC.GetUpdateCommand(username);
                        }
                        else
                        {
                            cmd = itemC.GetInsertCommand(username);
                        }

                        cmds.Add(cmd);
                    }

                    DataService.ExecuteTransaction(cmds);

                    transScope.Complete();
                }

                result.Status = true;
                result.Message = "Product saved successfully.";
            }
            catch (Exception)
            {
                result.Status = false;
                result.Message = "Cannot save product from excel. Please contact administrator.";
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
