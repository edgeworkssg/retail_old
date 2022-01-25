using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using PowerPOS;
using System.Data;
using SubSonic;
using System.Xml;
using System.IO;
using PowerWeb.Synchronization.Classes;
using PowerPOS.Container;
using Newtonsoft.Json;

namespace PowerWeb.Synchronization
{
    /// <summary>
    /// Summary description for Integration
    /// </summary>
    [WebService(Namespace = "http://www.edgeworks.com.sg/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Integration : System.Web.Services.WebService
    {
        public class AuthHeader : SoapHeader
        {
            public string Username;
            public string Password;
        }

        public AuthHeader Authentication;


        [WebMethod(CacheDuration = 0)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)] 
        public String GetItemList(bool syncAll)
        {
             return SynchronizationController.GetItemSerial(syncAll);
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public String GetMemberList(bool syncAll)
        {
            return SynchronizationController.GetDataTableSerial("Membership", syncAll);
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String GetCategoryList(bool syncAll)
        {
            DataTable dt = SynchronizationController.GetDataTable("Category", syncAll).Tables[0];
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public bool StockOutItem(string itemno, int quantity, string username, int StockOutReasonID,
            int InventoryLocationID, bool IsAdjustment, bool deductRemainingQty, out string newRefNo, out string status)
        {   
            try
            {
                string PointOfSaleString = AppSetting.GetSetting("SyncPointOfSale");
                int PointOfSaleID = 0;
                int inventoryLoc = 0; 
                if (int.TryParse(PointOfSaleString, out PointOfSaleID))
                {
                    PointOfSale pSale = new PointOfSale(PointOfSaleID);
                    if (pSale != null && pSale.PointOfSaleName != null && pSale.PointOfSaleName != "")
                    {
                        inventoryLoc = pSale.Outlet.InventoryLocation.InventoryLocationID;
                    }
                }
                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);

                ctrl.AddItemIntoInventory(itemno, quantity, out status);
                if (ctrl.StockOut(username, StockOutReasonID, inventoryLoc, IsAdjustment, deductRemainingQty, out status))
                {
                    newRefNo = ctrl.GetInvHdrRefNo();
                    return true;
                }
                else
                {
                    newRefNo = "";
                    return false;
                }

            }
            catch (Exception ex)
            {
                newRefNo = "";
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public List<InventoryItem> GetStockQuantity(Int32 InventoryLocation)
        {
            return SynchronizationController.GetStockQuantity(InventoryLocation);
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string GetStockQuantityByItem(Int32 InventoryLocation, string ItemNo)
        {
            return SynchronizationController.GetStockQuantityByItem(InventoryLocation, ItemNo);
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string GetItemStockQuantity(Int32 InventoryLocation, string ItemNo)
        {
            //return SynchronizationController.GetStockQuantityByItem(InventoryLocation, ItemNo);
            string sql = @"
                            SELECT IT.ItemNo, ISNULL(ISM.BalanceQty, 0) AS RunningQty 
                            FROM Item IT
                                LEFT JOIN ItemSummary ISM ON ISM.ItemNo = IT.ItemNo AND ISM.InventoryLocationID = @InventoryLocationID
                            WHERE IT.ItemNo = @ItemNo
                          ";
            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
            cmd.Parameters.Add("@InventoryLocationID", InventoryLocation, DbType.Int32);
            cmd.Parameters.Add("@ItemNo", ItemNo, DbType.String);
            DataSet ds = DataService.GetDataSet(cmd);

            decimal qty;
            if (ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0 || !decimal.TryParse(ds.Tables[0].Rows[0]["RunningQty"].ToString(), out qty))
            {
                return JsonConvert.SerializeObject(new { result = false, message = "Item not found", Quantity = 0 }); 
            }

            return JsonConvert.SerializeObject(new { result = true, message = "", Quantity = qty });
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public bool SendNewMembershipSignup(string membershipno, int membershipgroupid, string nametoappear, string firstname,
            string lastname, string address, string email,
            string country, string nric, string mobile, string gender, out string status)
        {
            try
            {
                #region validation
                if (membershipno == "")
                {
                    status = "Error. Membership Number cannot be empty";
                    return false;
                }
                if (nametoappear == "")
                {
                    status = "Error. Membership Name cannot be empty";
                    return false;
                }

                MembershipGroup mg = new MembershipGroup(membershipgroupid);
                if (mg == null || mg.IsLoaded == false)
                {
                    status = "Error. Membership Group ID is not Exist";
                    return false;
                }

                MembershipCollection mCol = new MembershipCollection();
                mCol.Where(Membership.Columns.MembershipNo, membershipno);
                mCol.Load();
                if (mCol.Count > 0)
                {
                    status = "Error. Membership Number already Exist";
                    return false;
                }
                #endregion
                status = "";
                Membership m = new Membership();
                m.MembershipNo = membershipno;
                m.MembershipGroupId = membershipgroupid;
                m.NameToAppear = nametoappear;
                m.FirstName = firstname;
                m.LastName = lastname;
                m.StreetName = address;
                m.Email = email;
                m.Country = country;
                m.Nric = nric;
                m.Mobile = mobile;
                m.Gender = gender;
                m.SubscriptionDate = DateTime.Today;
                m.ExpiryDate = DateTime.Today.AddYears(30);
                m.Deleted = false;
                m.Save();
                return true;
            }
            catch (Exception ex) { status = "Failed Add Member." + ex.Message; Logger.writeLog(ex.Message); return false; }


        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public bool AddProduct(string ProductID, string ProductName, string CategoryName, string DepartmentName, 
            string Barcode, bool IsInventory, decimal RetailPrice, decimal FactoryPrice,
            string itemDescription, string Remark, string GSTRule, string Attributes1, string Attributes2, string Attributes3,
            string Attributes4, string Attributes5, string Attributes6, string Attributes7, string Attributes8, bool isMatrixItem, 
            bool NonDiscountable, bool isPointRedeemable, 
             out string status)
        {
            try
            {
                QueryCommandCollection col = new QueryCommandCollection();
                status = "";
                #region Validation
                if (ProductID == "")
                {
                    status = "Error. Product ID cannot be Empty";
                    return false;
                }

                if (ProductName == "")
                {
                    status = "Error. Product Name cannot be Empty";
                    return false;
                }

                if (CategoryName == "")
                {
                    status = "Error. Category Name cannot be Empty";
                    return false;
                }
                
                Item i = new Item(ProductID);
                if (i != null && i.ItemNo != "" && i.IsLoaded)
                {
                    status = "Error. Product ID already Exist";
                    return false;
                }

                ItemDepartment id = new ItemDepartment(DepartmentName);
                if (id == null || id.ItemDepartmentID == "")
                {
                    ItemDepartment dept = new ItemDepartment();
                    dept.ItemDepartmentID = DepartmentName;
                    dept.DepartmentName = DepartmentName;
                    dept.Deleted= false;
                    col.Add(dept.GetInsertCommand("Integration"));
                }

                Category c = new Category(CategoryName);
                if (c == null || c.CategoryName == "")
                {
                    //Create new Category
                    Category cat = new Category();
                    cat.CategoryName = CategoryName;
                    cat.CategoryId = CategoryName;
                    cat.IsDiscountable = true;
                    cat.IsForSale = true;
                    cat.ItemDepartmentId = DepartmentName;
                    cat.Deleted = false;
                    col.Add(cat.GetInsertCommand("Integration"));
                }
                #endregion

                Item m = new Item(ProductID);

                if (m.IsLoaded && m.ItemNo != null && m.ItemNo != "")
                {
                    //Update
                    m.ItemName = ProductName;
                    m.CategoryName = CategoryName;
                    m.Barcode = Barcode;
                    m.RetailPrice = RetailPrice;
                    m.FactoryPrice = FactoryPrice;
                    if (IsInventory)
                    {
                        m.IsInInventory = true;
                        m.IsServiceItem = false;
                    }
                    else
                    {
                        m.IsInInventory = false;
                        m.IsServiceItem = true;
                    }
                    m.ItemDesc = itemDescription;
                    m.Remark = Remark;
                    if (GSTRule == "Inclusive")
                        m.GSTRule = 2;
                    else if (GSTRule == "Exclusive")
                        m.GSTRule = 1;
                    else
                        m.GSTRule = 0;
                    m.Attributes1 = Attributes1;
                    m.Attributes2 = Attributes2;
                    m.Attributes3 = Attributes3;
                    m.Attributes4 = Attributes4;
                    m.Attributes5 = Attributes5;
                    m.Attributes6 = Attributes6;
                    m.Attributes7 = Attributes7;
                    m.Attributes8 = Attributes8;
                    m.Userflag5 = isMatrixItem;
                    m.IsNonDiscountable = NonDiscountable;
                    if (isPointRedeemable)
                        m.Userfld9 = "D";
                    else
                        m.Userfld9 = "N";

                    m.Deleted = false;
                }
                else
                {

                    m = new Item();
                    m.ItemNo = ProductID;
                    m.ItemName = ProductName;
                    m.CategoryName = CategoryName;
                    m.Barcode = Barcode;
                    m.RetailPrice = RetailPrice;
                    m.FactoryPrice = FactoryPrice;
                    if (IsInventory)
                    {
                        m.IsInInventory = true;
                        m.IsServiceItem = false;
                    }
                    else
                    {
                        m.IsInInventory = false;
                        m.IsServiceItem = true;
                    }
                    m.ItemDesc = itemDescription;
                    m.Remark = Remark;
                    if (GSTRule == "Inclusive")
                        m.GSTRule = 2;
                    else if (GSTRule == "Exclusive")
                        m.GSTRule = 1;
                    else
                        m.GSTRule = 0;
                    m.Attributes1 = Attributes1;
                    m.Attributes2 = Attributes2;
                    m.Attributes3 = Attributes3;
                    m.Attributes4 = Attributes4;
                    m.Attributes5 = Attributes5;
                    m.Attributes6 = Attributes6;
                    m.Attributes7 = Attributes7;
                    m.Attributes8 = Attributes8;
                    m.Userflag5 = isMatrixItem;
                    m.IsNonDiscountable = NonDiscountable;
                    if (isPointRedeemable)
                        m.Userfld9 = "D";
                    else
                        m.Userfld9 = "N";
                      
                    m.Deleted = false;
                    col.Add(m.GetInsertCommand("Integration"));
                }

                DataService.ExecuteTransaction(col);
                return true;
            }
            catch (Exception ex) { status = "Failed Add Product." + ex.Message; Logger.writeLog(ex.Message); return false; }


        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public bool DeleteProduct(string ProductID,
             out string status)
        {
            try
            {
                
                status = "";
                #region Validation
                if (ProductID == "")
                {
                    status = "Error. Product ID cannot be Empty";
                    return false;
                }
                #endregion
                

                Item i = new Item(ProductID);
                if (i != null && i.ItemNo != "" && i.IsLoaded)
                {
                    i.Deleted = true;
                    i.Save();
                }
                else
                {
                    status = "Error. Product Not Found";
                    return false;
                }
                return true;
                
            }
            catch (Exception ex) { status = "Failed Delete Product." + ex.Message; Logger.writeLog(ex.Message); return false; }


        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public bool DeleteMember(string MembershipNo,
             out string status)
        {
            try
            {

                status = "";
                #region Validation
                if (MembershipNo == "")
                {
                    status = "Error. Member No cannot be Empty";
                    return false;
                }
                #endregion


                Membership i = new Membership(MembershipNo);
                if (i != null && i.MembershipNo != "" && i.IsLoaded)
                {
                    i.Deleted = true;
                    i.Save();
                }
                return true;

            }
            catch (Exception ex) { status = "Failed Delete Member." + ex.Message; Logger.writeLog(ex.Message); return false; }


        }


        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string SendSales(string orderdata, string membershipno, string cashier, string paymentdata)
        {
            try
            {
                if (orderdata == null)
                { Logger.writeLog ( "data null"); }
                Logger.writeLog("try to sync :" + orderdata + "," + paymentdata);
                string status = "";
                #region Set Default Value
                if (membershipno == "")
                {
                    membershipno = "WALK-IN";
                }
                if (cashier == "")
                {   
                    cashier = AppSetting.GetSetting("SyncCashier");
                }
                #endregion

                #region Parse OrderData
                string[] orders;
                string[] payments;
                try
                {
                    orders = SynchronizationController.ParseOrder(orderdata);
                    payments = SynchronizationController.ParseOrder(paymentdata);
                }
                catch (Exception ex) 
                {   
                    status = "Error Parsing Order Data / Payment Data";
                    Logger.writeLog("Error Parsing Order Data" + ex.Message); 
                    return status;
                }
                #endregion
                //Load orderdata
                PointOfSaleInfo.PointOfSaleID = int.Parse(AppSetting.GetSetting("SyncPointOfSale"));
                PointOfSaleInfo.IntegrateWithInventory = false;
                PointOfSale p = new PointOfSale(PointOfSaleInfo.PointOfSaleID);
                PointOfSaleInfo.InventoryLocationID = (int)p.Outlet.InventoryLocationID;
                POSController pos = new POSController();
                Logger.writeLog("Assign Membership");
                if (!pos.AssignMembership(membershipno, out status))
                {
                    Logger.writeLog("Insert Member Failed." + status);
                    return status;
                }
                #region insert item to pos
                Logger.writeLog("Insert Item to POS");
                if (orders.Length > 0)
                {   
                    try 
                    {
                        foreach (string s in orders)
                        {
                            if (s != "")
                            {
                                string[] temporder = s.Split(',');
                                if (temporder.Length == 8)
                                {
                                    if (POSController.IsOrderExist(temporder[0].Trim()))
                                    {
                                        status = "Order Already Exist";
                                        Logger.writeLog("Order Already Exist");
                                        return status;
                                    }

                                    pos.myOrderHdr.OrderDate = DateTime.Parse(temporder[1].Trim());
                                    pos.myOrderHdr.Userfld5 = temporder[0].Trim();

                                    string productCode = temporder[2].Trim();

                                    Item vi = getItem(productCode);
                                    if (vi != null && vi.ItemNo != "")
                                    {
                                        pos.AddSynchronizedItemToOrder(vi, int.Parse(temporder[3].Trim()), decimal.Parse(temporder[4].Trim()), decimal.Parse(temporder[7].Trim()), decimal.Parse(temporder[6].Trim()), decimal.Parse(temporder[5].Trim()), true, "", out status);
                                    }
                                    else
                                    {
                                        status = "Cannot Find Item Specified.";
                                        Logger.writeLog(status);
                                        return status;
                                    }

                                }
                                else
                                {
                                    status = "Error Parsing Order Details Data.";
                                    Logger.writeLog(status);
                                    return status;
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        status = "Error Parsing Order Details Data.";
                        Logger.writeLog(status);
                        return status;
                    }
                }
                else
                {
                    status = "No Order Data Found. Please check your data";
                    Logger.writeLog("No Order Data Found. Please check your data");
                    return status;
                }

                if (payments.Length > 0)
                {
                    try 
                    {
                        foreach (string s in payments)
                        {
                            if (s != "")
                            {
                                string[] temppayment = s.Split(',');
                                if (temppayment.Length == 2)
                                {
                                    decimal change = 0;
                                    if (!pos.AddReceiptLinePayment(decimal.Parse(temppayment[1].Trim()), temppayment[0].Trim(), "",0,"",0, out change, out status))
                                    {
                                        status = "Add Payment Failed. Please check your data";
                                        Logger.writeLog(status);
                                        return status;
                                    }
                                }
                                else
                                {
                                    if (temppayment.Length == 3)
                                    {
                                        decimal change = 0;
                                        if (!pos.AddReceiptLinePaymentWithRemark(decimal.Parse(temppayment[1].Trim()), temppayment[0].Trim(), temppayment[2].Trim(), out change, out status))
                                        {
                                            status = "Add Payment Failed. Please check your data";
                                            Logger.writeLog(status);
                                            return status;
                                        }
                                    }
                                    else
                                    {
                                        status = "Error Parsing Payment Details Data.";
                                        Logger.writeLog(status);
                                        return status;
                                    }
                                }
                            }

                        }
                        
                    }
                    catch (Exception ex)
                    {
                        status = "Error Parsing Payment Details Data.";
                        Logger.writeLog(status);
                        return status;
                    }
                }

                else
                {
                    status = "No Order Data Found. Please check your data";
                    Logger.writeLog("No Order Data Found. Please check your data");
                    return status;
                }
                #endregion

                pos.myOrderHdr.NettAmount= pos.CalculateTotalAmount(out status);
                pos.myOrderHdr.GrossAmount = pos.CalculateTotalAmount(out status);
                //pos.myOrderHdr.CashierID = "Admin";
                
                bool isPointAllocated = false;
                if (pos.ConfirmOrderFromSync(true, cashier , out isPointAllocated, out status))
                {
                    
                    if (!pos.ExecuteStockOutFromSync(out status))
                    {
                        status = "Success. Stock Out Failed";
                    }
                    Logger.writeLog("After StockOut : " + status);
                    if (status == "")
                    {
                        status = "Success";
                    }
                    return status;
                }
                else
                {
                    Logger.writeLog("Confirm Order Failed." + status);
                    status = "Confirm Order Failed." + status;
                    return status;
                }

            }
            catch (Exception ex) { string status = "Failed Insert Order Data." + ex.Message; Logger.writeLog(ex.Message); return status; }


        }

        private Item getItem(string productCode)
        {
            Item res = null;
            try
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Integration.UseCustomFieldForItemNo), false))
                {
                    res = new Item(AppSetting.GetSetting(AppSetting.SettingsName.Integration.CustomFieldName),productCode);
                }
                else
                {
                    res = new Item(productCode);
                }
                return res;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error get Item." + ex.Message);
                return null;
            }
        }


        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string VoidSales(string OrderNo, string remark)
        {
            try
            {
                string status;
                if (OrderNo== null || OrderNo == "")
                { 
                    status = "Order No cannot be Empty"; 
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }
                
                #region Validate Order 
                OrderHdrCollection ohCol = new OrderHdrCollection();
                ohCol.Where(OrderHdr.Columns.Userfld5, OrderNo);
                ohCol.Load();
                if (ohCol.Count <= 0)
                {
                    status = "Order No cannot be found. ";
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }
                if (ohCol[0].IsVoided)
                {
                    status = "Order No already voided. ";
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }
                #endregion
                #region Void Receipt 
                POSController pos = new POSController(ohCol[0].OrderHdrID);
                if (pos != null && pos.myOrderHdr.OrderHdrID != null && pos.myOrderHdr.OrderHdrID != "")
                {
                    if (!POSController.VoidReceiptServer(pos.myOrderHdr.OrderRefNo, UserInfo.username, UserInfo.username, remark, out status))
                    {
                        return JsonConvert.SerializeObject(new { result = false, status = status });
                    }
                }
                #endregion
                return JsonConvert.SerializeObject(new { result = true, status = "" }); 
            }
            catch (Exception ex) 
            { 
                string status = "Failed Void Receipt " + OrderNo + "." + ex.Message; 
                Logger.writeLog(ex.Message);
                return JsonConvert.SerializeObject(new { result = false, status = status });
            }


        }

        #region *) In2Kids Integration

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string IsEmailRegistered(string emailAddress)
        {
            Membership mbr = new Membership(Membership.Columns.Email, emailAddress.Trim());
            bool found;

            if (string.IsNullOrEmpty(mbr.Email))
            {
                // Not found
                mbr = null;
                found = false;
            }
            else
            {
                // Found
                found = true;
            }

            return JsonConvert.SerializeObject(new { result = found, data = mbr });
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string IsNRICRegistered(string nric)
        {
            Membership mbr = new Membership(Membership.Columns.Nric, nric.Trim());
            bool found;

            if (string.IsNullOrEmpty(mbr.Nric))
            {
                // Not found
                mbr = null;
                found = false;
            }
            else
            {
                // Found
                found = true;
            }

            return JsonConvert.SerializeObject(new { result = found, data = mbr });
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string GetMemberTransactionHistory(string membershipNo)
        {
            try
            {
                Membership activeMember = new Membership(membershipNo);
                DataTable dt;
                bool found;

                if (string.IsNullOrEmpty(activeMember.MembershipNo))
                {
                    // Not found
                    found = false;
                    dt = null;
                }
                else
                {
                    found = true;
                    int rowtotal = 0;
                    if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Membership.MembershipSummaryRowNumber), out rowtotal))
                    {
                        rowtotal = 5000;
                    }

                    dt = activeMember.GetPastTransaction(rowtotal, true);
                    dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
                }

                return JsonConvert.SerializeObject(new { result = found, data = dt });
            }
            catch (Exception ex) 
            {
                string status = "Failed to get member transaction history." + ex.Message; 
                Logger.writeLog(ex.Message);
                return status; 
            }
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string GetMemberFULLTransactionHistory(string membershipNo)
        {
            try
            {
                Membership activeMember = new Membership(membershipNo);
                DataTable dt;
                bool found;

                if (string.IsNullOrEmpty(activeMember.MembershipNo))
                {
                    // Not found
                    found = false;
                    dt = null;
                }
                else
                {
                    found = true;
                    int rowtotal = 0;
                    if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Membership.MembershipSummaryRowNumber), out rowtotal))
                    {
                        rowtotal = 5000;
                    }

                    dt = activeMember.GetPastTransaction(rowtotal);
                    dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
                }

                return JsonConvert.SerializeObject(new { result = found, data = dt });
            }
            catch (Exception ex)
            {
                string status = "Failed to get member transaction history." + ex.Message;
                Logger.writeLog(ex.Message);
                return status;
            }
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string GetMemberTransactionHistoryByDate(string membershipNo, DateTime startDate, DateTime endDate)
        {
            try
            {
                Membership activeMember = new Membership(membershipNo);
                DataTable dt;
                bool found;

                if (string.IsNullOrEmpty(activeMember.MembershipNo))
                {
                    // Not found
                    found = false;
                    dt = null;
                }
                else
                {
                    found = true;
                    dt = activeMember.GetPastTransaction(startDate, endDate, true);
                    dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
                }

                return JsonConvert.SerializeObject(new { result = found, data = dt });
            }
            catch (Exception ex)
            {
                string status = "Failed to get member transaction history." + ex.Message;
                Logger.writeLog(ex.Message);
                return status;
            }
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string GetMemberFULLTransactionHistoryByDate(string membershipNo, DateTime startDate, DateTime endDate)
        {
            try
            {
                Membership activeMember = new Membership(membershipNo);
                DataTable dt;
                bool found;

                if (string.IsNullOrEmpty(activeMember.MembershipNo))
                {
                    // Not found
                    found = false;
                    dt = null;
                }
                else
                {
                    found = true;
                    dt = activeMember.GetPastTransaction(startDate, endDate);
                    dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
                }

                return JsonConvert.SerializeObject(new { result = found, data = dt });
            }
            catch (Exception ex)
            {
                string status = "Failed to get member transaction history." + ex.Message;
                Logger.writeLog(ex.Message);
                return status;
            }
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string AddPoints(string membershipNo, string orderHdrID, DateTime transactionDate, int validPeriod,
                                decimal points, string userName)
        {
            string status = "";

            try
            {
                #region *) Validation
                if (string.IsNullOrEmpty(membershipNo))
                {
                    status = "membershipNo is empty.";
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }

                Membership mbr = new Membership(membershipNo);
                if (string.IsNullOrEmpty(mbr.MembershipNo))
                {
                    status = "Member not found.";
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }

                string itemNo = AppSetting.GetSetting(AppSetting.SettingsName.Points.IntegrationPointsItemNo);
                if (string.IsNullOrEmpty(itemNo))
                {
                    status = "Default ItemNo for Points has not been configured yet.";
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }

                Item item = new Item(itemNo);
                if (string.IsNullOrEmpty(item.ItemNo))
                {
                    status = "Invalid ItemNo for Points.";
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }

                if (points == 0)
                {
                    status = "Points cannot be zero.";
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }
                #endregion

                DataTable pointData = new DataTable("PointPackage");
                pointData.Columns.Add("RefNo", Type.GetType("System.String"));   // ItemNo for Point
                pointData.Columns.Add("Amount", Type.GetType("System.Decimal"));
                pointData.Columns.Add("PointType", Type.GetType("System.String"));   // "D"
                DataRow row = pointData.NewRow();
                row["RefNo"] = item.ItemNo;
                row["Amount"] = points;
                row["PointType"] = "D";  // Always D (Dollar)
                pointData.Rows.Add(row);

                bool result = PackageController.UpdateAll(pointData, orderHdrID, transactionDate, validPeriod, membershipNo, userName, userName, out status);
                return JsonConvert.SerializeObject(new { result = result, status = status });
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return JsonConvert.SerializeObject(new { result = false, status = status });
            }
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string DeductPoints(string membershipNo, string orderHdrID, DateTime transactionDate, decimal points, string userName)
        {
            int validPeriod = 0;
            points = -points;
            return AddPoints(membershipNo, orderHdrID, transactionDate, validPeriod, points, userName);
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string GetCurrentPoints(string membershipNo, DateTime currentDate)
        {
            string status = "";

            try
            {
                #region *) Validation
                if (string.IsNullOrEmpty(membershipNo))
                {
                    status = "membershipNo is empty.";
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }

                Membership mbr = new Membership(membershipNo);
                if (string.IsNullOrEmpty(mbr.MembershipNo))
                {
                    status = "Member not found.";
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }

                string itemNo = AppSetting.GetSetting(AppSetting.SettingsName.Points.IntegrationPointsItemNo);
                if (string.IsNullOrEmpty(itemNo))
                {
                    status = "Default ItemNo for Points has not been configured yet.";
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }

                Item item = new Item(itemNo);
                if (string.IsNullOrEmpty(item.ItemNo))
                {
                    status = "Invalid ItemNo for Points.";
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }
                #endregion

                decimal points = 0;
                bool result = PackageController.GetCurrentAmount(membershipNo, currentDate, itemNo, out points, out status);
                return JsonConvert.SerializeObject(new { result = result, status = status, points = points });
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return JsonConvert.SerializeObject(new { result = false, status = status });
            }
        }

        #endregion

        #region WooCommerce

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string GetItemStockByListOfBarcode(Int32 InventoryLocation, string jsonBarcode)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            List<String> barcodes = new JavaScriptSerializer().Deserialize<List<String>>(jsonBarcode);

            List<SKUItem> items = new List<SKUItem>();

            foreach (var barcode in barcodes)
            {
                string sql = @"
                    SELECT IT.Barcode, ITS.BalanceQty
                    FROM ItemSummary ITS
                    JOIN Item IT ON IT.ItemNo = ITS.ItemNo
                    WHERE ISNULL(ITS.Deleted, 0) = 0 
                        AND IT.Barcode = @Barcode
                        AND ITS.InventoryLocationID = @InventoryLocationID
                ";
                QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
                cmd.Parameters.Add("@Barcode", barcode);
                cmd.Parameters.Add("@InventoryLocationID", InventoryLocation);

                IDataReader rdr = DataService.GetReader(cmd);
                while (rdr.Read())
                {
                    decimal qty = 0;
                    decimal.TryParse(rdr["BalanceQty"].ToString(), out qty);

                    items.Add(new SKUItem { Barcode = rdr["Barcode"].ToString(), Qty = qty < 0 ? 0 : qty });
                }
            }

            return JsonConvert.SerializeObject(new { data = items });
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod]
        public string SendSalesByBarcode(string orderdata, string membershipno, string cashier, string paymentdata)
        {
            try
            {
                if (orderdata == null)
                { Logger.writeLog("data null"); }
                Logger.writeLog("try to sync :" + orderdata + "," + paymentdata);
                string status = "";
                #region Set Default Value
                if (membershipno == "")
                {
                    membershipno = "WALK-IN";
                }
                if (cashier == "")
                {
                    cashier = AppSetting.GetSetting("SyncCashier");
                }
                #endregion

                #region Parse OrderData
                string[] orders;
                string[] payments;
                try
                {
                    orders = SynchronizationController.ParseOrder(orderdata);
                    payments = SynchronizationController.ParseOrder(paymentdata);
                }
                catch (Exception ex)
                {
                    status = "Error Parsing Order Data / Payment Data";
                    Logger.writeLog("Error Parsing Order Data" + ex.Message);
                    return status;
                }
                #endregion
                //Load orderdata
                PointOfSaleInfo.PointOfSaleID = int.Parse(AppSetting.GetSetting("SyncPointOfSale"));
                PointOfSaleInfo.IntegrateWithInventory = false;
                PointOfSale p = new PointOfSale(PointOfSaleInfo.PointOfSaleID);
                PointOfSaleInfo.InventoryLocationID = (int)p.Outlet.InventoryLocationID;
                POSController pos = new POSController();
                Logger.writeLog("Assign Membership");
                if (!pos.AssignMembership(membershipno, out status))
                {
                    Logger.writeLog("Insert Member Failed." + status);
                    return status;
                }
                #region insert item to pos
                Logger.writeLog("Insert Item to POS");
                if (orders.Length > 0)
                {
                    try
                    {
                        foreach (string s in orders)
                        {
                            if (s != "")
                            {
                                string[] temporder = s.Split(',');
                                if (temporder.Length == 8)
                                {
                                    if (POSController.IsOrderExist(temporder[0].Trim()))
                                    {
                                        status = "Order Already Exist";
                                        Logger.writeLog("Order Already Exist");
                                        return status;
                                    }

                                    pos.myOrderHdr.OrderDate = DateTime.Parse(temporder[1].Trim());
                                    pos.myOrderHdr.Userfld5 = temporder[0].Trim();

                                    string barcode = temporder[2].Trim();
                                    string itemNo = "";

                                    var it = new ItemController().FetchByBarcode(barcode);
                                    if (string.IsNullOrEmpty(it.ItemNo))
                                    {
                                        itemNo = AppSetting.GetSetting(AppSetting.SettingsName.Sync.OtherItemNo);
                                        if (string.IsNullOrEmpty(itemNo))
                                        {
                                            status = "OtherItemNo is not setup";
                                            Logger.writeLog("OtherItemNo is not setup");
                                            return status;
                                        }
                                    }
                                    else
                                    {
                                        itemNo = it.ItemNo;
                                    }


                                    Item vi = new Item(itemNo);
                                    if (vi != null && vi.ItemNo != "")
                                    {
                                        pos.AddSynchronizedItemToOrder(vi, int.Parse(temporder[3].Trim()), decimal.Parse(temporder[4].Trim()), decimal.Parse(temporder[7].Trim()), decimal.Parse(temporder[6].Trim()), decimal.Parse(temporder[5].Trim()), true, "", out status);
                                    }
                                    else
                                    {
                                        status = "Cannot Find Item Specified.";
                                        Logger.writeLog(status);
                                        return status;
                                    }

                                }
                                else
                                {
                                    status = "Error Parsing Order Details Data.";
                                    Logger.writeLog(status);
                                    return status;
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        status = "Error Parsing Order Details Data.";
                        Logger.writeLog(status);
                        return status;
                    }
                }
                else
                {
                    status = "No Order Data Found. Please check your data";
                    Logger.writeLog("No Order Data Found. Please check your data");
                    return status;
                }

                if (payments.Length > 0)
                {
                    try
                    {
                        foreach (string s in payments)
                        {
                            if (s != "")
                            {
                                string[] temppayment = s.Split(',');
                                if (temppayment.Length == 2)
                                {
                                    decimal change = 0;
                                    if (!pos.AddReceiptLinePayment(decimal.Parse(temppayment[1].Trim()), temppayment[0].Trim(), "", 0, "", 0, out change, out status))
                                    {
                                        status = "Add Payment Failed. Please check your data";
                                        Logger.writeLog(status);
                                        return status;
                                    }
                                }
                                else
                                {
                                    if (temppayment.Length == 3)
                                    {
                                        decimal change = 0;
                                        if (!pos.AddReceiptLinePaymentWithRemark(decimal.Parse(temppayment[1].Trim()), temppayment[0].Trim(), temppayment[2].Trim(), out change, out status))
                                        {
                                            status = "Add Payment Failed. Please check your data";
                                            Logger.writeLog(status);
                                            return status;
                                        }
                                    }
                                    else
                                    {
                                        status = "Error Parsing Payment Details Data.";
                                        Logger.writeLog(status);
                                        return status;
                                    }
                                }
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        status = "Error Parsing Payment Details Data.";
                        Logger.writeLog(status);
                        return status;
                    }
                }

                else
                {
                    status = "No Order Data Found. Please check your data";
                    Logger.writeLog("No Order Data Found. Please check your data");
                    return status;
                }
                #endregion

                pos.myOrderHdr.NettAmount = pos.CalculateTotalAmount(out status);
                pos.myOrderHdr.GrossAmount = pos.CalculateTotalAmount(out status);
                //pos.myOrderHdr.CashierID = "Admin";

                bool isPointAllocated = false;
                if (pos.ConfirmOrderFromSync(true, cashier, out isPointAllocated, out status))
                {

                    if (!pos.ExecuteStockOutFromSync(out status))
                    {
                        status = "Success. Stock Out Failed";
                    }
                    Logger.writeLog("After StockOut : " + status);
                    if (status == "")
                    {
                        status = "Success";
                    }
                    return status;
                }
                else
                {
                    Logger.writeLog("Confirm Order Failed." + status);
                    status = "Confirm Order Failed." + status;
                    return status;
                }

            }
            catch (Exception ex) { string status = "Failed Insert Order Data." + ex.Message; Logger.writeLog(ex.Message); return status; }


        }


        #endregion
    }
}
