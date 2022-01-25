using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using System.Collections;
using PowerPOS.Container;
using SubSonic;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace PowerPOS
{
    [System.ComponentModel.DataObject]
    [Serializable]
    public partial class MallIntegrationProviderController
    {

        #region Properties
        public OrderHdr myOrderHdr;             //Order Header        
        public OrderDetCollection myOrderDet;   //Order Detail
        public ReceiptHdr recHdr;
        public ReceiptDetCollection recDet;
        bool isNewMember;
        private double GST;
        #endregion

        #region constant 
        //Constant for payment types        
        public const string PAY_CASH = "CASH";

        public const string PAY_FOREIGN_CURRENCY = "FOREIGN CURRENCY";

        public const string PAY_NETS = "NETS";

        public const string PAY_CHINA_NETS = "CHINA NETS";

        public const string PAY_AMEX = "AMEX";

        public const string PAY_VISA = "VISA";

        public const string PAY_MASTER = "MASTER";

        public const string PAY_VOUCHER = "VOUCHER";

        public const string PAY_CHEQUE = "CHEQUE";

        public const string PAY_INSTALLMENT = "INSTALLMENT";

        public const string PAY_POINTS = "POINTS";

        public const string PAY_PACKAGE = "PACKAGE";

        public const string PAY_PAMEDIFUND = "PAMedifund";

        public const string PAY_SMF = "SMF";

        public const string PAY_PWF = "PWF";

        public const string PAY_NETS_FLASHPAY = "NetsFlashPay";

        public const string PAY_NETS_CASHCARD = "CashCard";

        public const string PAY_NETS_ATMCARD = "NetsCard";
        #endregion

        #region Main Function

        /*public bool (string mallCode, string tenantCode, DateTime orderDate, int transactionCount,
            decimal totalSalesAfterTax, decimal totalSalesBeforeTax, decimal totalTax, string username, string remarks, out string status)
        {
            bool result = false;
            status = "";
            try
            {
                string orderHdrID = "";
                result = InsertMallIntegration(tenantCode, orderDate, transactionCount, totalSalesAfterTax, totalTax, username, remarks, out status, out orderHdrID);
                if (result)
                {
                    int posid = 0;
                    MallIntegrationProviderController.CheckTenantID(tenantCode, out posid);
                    /*ManualSalesUpdate msu = new ManualSalesUpdate();
                    msu.PointOfSaleID = posid;
                    msu.MallCode = mallCode;
                    msu.TenantCode = tenantCode;
                    msu.DateX = orderDate.ToString("yyyy-MM-dd");
                    msu.Hour = orderDate.ToString("hh:mm:ss tt");
                    msu.TransactionCount = transactionCount;
                    msu.TotalSalesAfterTax = totalSalesAfterTax;
                    msu.TotalSalesBeforeTax = totalSalesBeforeTax;
                    msu.TotalTax = totalTax;
                    msu.Remarks = remarks;
                    msu.Userfld1 = orderHdrID;
                    msu.Save(username);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Error : " + ex.Message;
                result = false;
            }
            return result;
        }*/

        public bool InsertMallIntegration(string tenantCode, DateTime orderDate, int TransactionCount, 
            decimal TotalSalesAfterTax, decimal TotalTax, string username, string remarks, int pointofsaleid, out string status, out string 
            orderHdrID)
        {
            bool result = false;
            orderHdrID = "";
            //InsertScript = new QueryCommand("");
            try
            {
                status = "";
                
                myOrderHdr.OrderDate = orderDate;
                myOrderHdr.PointOfSaleID = pointofsaleid;
                myOrderHdr.Remark = remarks;
                myOrderHdr.Userint1 = TransactionCount;
                myOrderHdr.OrderDate = orderDate;
                #region Insert Item

                Item it = new Item("HOURLYSALES");
                if (it != null && it.IsLoaded && it.ItemNo != "")
                {

                    if (!AddServiceItemToOrder(it, 1, TotalSalesAfterTax, TotalTax, orderDate, "", out status))
                    {
                        status = status = "Error. Error Inserting Item Data.";
                        return false;
                        //return JsonConvert.SerializeObject(new { result = false, status = "Error. Error Inserting Data." });
                    }
                }
                else
                {
                    status = "Error. Cannot find Hourly Sales Item.";
                    return false;

                    //return JsonConvert.SerializeObject(new { result = false,  });
                }
                #endregion

                #region Add ReceiptLine
                decimal totalAmount = CalculateTotalAmount(out status);
                decimal change;
                if (!AddReceiptLinePayment(totalAmount, "CASH", "", 0, "", 0, out change, out status))
                {
                    status = "Error Adding Payment." + status;
                    return false;
                }
                #endregion

                bool isPointAllocated;
                if (!ConfirmOrder(false, out isPointAllocated, username, out status))
                {
                    status = "Error." + status;
                    return false;
                    //return JsonConvert.SerializeObject(new { result = false,  });
                }
                else
                    orderHdrID = myOrderHdr.OrderHdrID;
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                status = "Error Insert Mall Integration." + ex.Message;

                return false;
            }
        }
        #endregion

        #region Validator
        public static bool CheckLogin(string mallCode, string tenantCode, string APIKey, out string status)
        {
            try
            {
                int posid = 0;
                status = "";
                #region Check Tenant ID
                if (!MallIntegrationProviderController.CheckTenantID(tenantCode, out posid))
                {
                    status = "Invalid Mall Code / Tenant Code";
                    return false;
                    //return JsonConvert.SerializeObject(new { result = false,  });
                }

                #endregion
                #region Check Mall code
                PointOfSale p = new PointOfSale(posid);
                if (p.Outlet.Userfld1 != mallCode)
                {
                    status = "Invalid Mall Code / Tenant Code";
                    return false;
                }
                #endregion

                #region Check Mall code
                string sqlString = "Select APIKey from PointOfSale where pointofsaleid = " + p.PointOfSaleID.ToString();
                object tmpKey = DataService.ExecuteScalar(new QueryCommand(sqlString));
                if (tmpKey.ToString() != UserController.EncryptData(APIKey))
                {
                    status = "Invalid API Key";
                    return false;
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                status = "Error Encountered. " + ex.Message;
                return false;
            }
        }

        public static bool validateFile(string username, string data, out string status)
        {
            status = "";
            bool result =false;
            try
            {

                List<MallIntegrationData> tmp;
                try
                {
                    tmp = new JavaScriptSerializer().Deserialize<List<MallIntegrationData>>(data);
                }
                catch (Exception ex)
                {
                    Logger.writeLog("Error Parsing Data Sent, " + data + "." + ex.Message);
                    result = false; 
                    status = "Error Parsing the data.";
                    return result;
                }
                for (int i = 0; i < tmp.Count; i++)
                {
                    int posid = 0;
                    #region Check Tenant ID
                    if (!MallIntegrationProviderController.CheckTenantID(tmp[i].TenantCode, out posid))
                    {
                        status = "Invalid Tenant Code : " + tmp[i].TenantCode;
                        return false;
                        //return JsonConvert.SerializeObject(new { result = false,  });
                    }

                    if (tmp[i].TenantCode != username && username != "test")
                    {
                        status = "Invalid Tenant Code : " + tmp[i].TenantCode;
                        return false;
                    }
                    
                    #endregion
                    #region Check Mall code
                    PointOfSale p = new PointOfSale(posid);
                    if (p.Outlet.Userfld1 != tmp[i].MallCode)
                    {
                        status = "Invalid Mall Code : " + tmp[i].MallCode;
                        return false;
                    }
                    #endregion

                    #region Check Date
                    DateTime orderDate;
                    int hr = 0;
                    if (!int.TryParse(tmp[i].Hour, out hr))
                    {
                        status = "Invalid Hour Value : " + tmp[i].Hour;
                        return false;
                    }
                    if (!MallIntegrationProviderController.CheckDate(tmp[i].Date, hr, out status, out orderDate))
                    {
                        return false;
                    }
                    #endregion

                    #region *) Check if the date is after business commencement date
                    string sqlString = "Select BusinessStartDate from PointOfSale where pointofsaleid = " + p.PointOfSaleID.ToString();
                    object tmpObjStartDate = DataService.ExecuteScalar(new QueryCommand(sqlString));
                    DateTime businessStartDate;
                    if (!DateTime.TryParse(tmpObjStartDate.ToString(), out businessStartDate))
                    {
                        // business start date not set yet
                        status = "Business commencement date for this customer not set yet. Please check your data";
                        return false;
                    }
                    else
                    {
                        if (orderDate < businessStartDate)
                        {
                            status = string.Format("Order Date({0}) is before business commencement date : {1}", orderDate.ToString("yyyy-MM-dd HH:mm:ss"), businessStartDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            return false;
                        }
                    }

                    sqlString = "Select BusinessEndDate from PointOfSale where pointofsaleid = " + p.PointOfSaleID.ToString();
                    object tmpObjEndDate = DataService.ExecuteScalar(new QueryCommand(sqlString));
                    DateTime businessEndDate;
                    if (!DateTime.TryParse(tmpObjEndDate.ToString(), out businessEndDate))
                    {
                        // business start date not set yet
                        status = "Business End date for this customer not set yet. Please check your data";
                        return false;
                    }
                    else
                    {
                        if (orderDate > businessEndDate)
                        {
                            status = string.Format("Order Date({0}) is after business end date : {1}", orderDate.ToString("yyyy-MM-dd HH:mm:ss"), businessEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            return false;
                        }
                    }

                    if (orderDate > DateTime.Today.AddDays(1))
                    {
                        status = string.Format("Order Date({0}) is after today  : {1}", orderDate.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"));
                        return false;
                    }
                    #endregion

                    #region check amount
                    decimal _NettAmount;
                    decimal _GrossAmount;
                    decimal _GSTAmount;

                    if (!decimal.TryParse(tmp[i].TotalSalesAfterTax, out _NettAmount))
                    {
                        status = "Total Sales After Tax Value is wrong : " + tmp[i].TotalSalesAfterTax;
                        return false;
                    }

                    if (!decimal.TryParse(tmp[i].TotalSalesBeforeTax, out _GrossAmount))
                    {
                        status = "Total Sales Before Tax Value is wrong : " + tmp[i].TotalSalesBeforeTax;
                        return false;
                    }

                    if (!decimal.TryParse(tmp[i].TotalTax, out _GSTAmount))
                    {
                        status = "Total Tax Value is wrong : " + tmp[i].TotalTax;
                        return false;
                    }
                    #endregion
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return false;
            }
        }

        #endregion
        #region "Constructor"
        public MallIntegrationProviderController()
        {
            ResetObject();
        }

        public void ResetObject()
        {
            try
            {
                //Initialize order hdr
                myOrderHdr = new OrderHdr();
                isNewMember = false;
                LoadGST();
                myOrderHdr.OrderHdrID = CreateNewOrderNo(PointOfSaleInfo.PointOfSaleID);
                myOrderHdr.OrderRefNo = "OR" + myOrderHdr.OrderHdrID;
                myOrderHdr.CashierID = UserInfo.username;
                myOrderHdr.Discount = 0; //get default values from database
                myOrderHdr.Gst = GST;
                myOrderHdr.Userfld5 = myOrderHdr.OrderRefNo;
                myOrderHdr.OrderDate = DateTime.Now;
                myOrderHdr.PointOfSaleID = PointOfSaleInfo.PointOfSaleID; //get default values from config file

                //Initialize order detail
                myOrderDet = new OrderDetCollection();

                //Create Receipt Hdr
                recHdr = new ReceiptHdr();
                recHdr.ReceiptDate = DateTime.Now;
                recHdr.ReceiptHdrID = myOrderHdr.OrderHdrID;
                recHdr.OrderHdrID = myOrderHdr.OrderHdrID;
                recHdr.ReceiptDate = DateTime.Now;
                recHdr.ReceiptRefNo = "RCP" + recHdr.ReceiptHdrID;
                recHdr.CashierID = UserInfo.username;
                recHdr.PointOfSaleID = myOrderHdr.PointOfSaleID;

                //Create Receipt Det
                recDet = new ReceiptDetCollection();
                    
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        //Load GST from database
        private bool LoadGST()
        {
            //Load GST from GST Table
            Query qry = new Query("GST");
            Where whr = new Where();
            whr.ColumnName = PowerPOS.Gst.Columns.CommenceDate;
            whr.Comparison = Comparison.LessOrEquals;
            whr.ParameterName = "@CommenceDate";
            whr.ParameterValue = DateTime.Now.ToString("yyyy-MM-dd");
            whr.TableName = "GST";
            //pull out from GST table
            object obj = qry.GetMax(PowerPOS.Gst.Columns.GSTRate, whr);
            GST = (double.Parse(obj.ToString()));
            return true;
        }

        #endregion

        #region Add Item & Payment 
        //Create new order ref no for POS
        private string CreateNewOrderNo(int PointOfSaleID)
        {
            int runningNo = 0;

            //use stored procedure to pull out the biggest number for today's order
            //format of order: YYMMDDSSSSNNNN
            //This stored procedure pull out the last order number
            IDataReader ds = PowerPOS.SPs.GetNewOrderHdrNoByPointOfSaleID(PointOfSaleID).GetReader();
            while (ds.Read())
            {
                if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
                {
                    runningNo = 0;
                }
            }
            ds.Close();
            runningNo += 1;

            //YYMMDDSSSSNNNN                
            //YY - year
            //MM - month
            //DD - day
            //SSSS - PointOfSale ID
            //NNNN - Running No
            return DateTime.Now.ToString("yyMMdd") + PointOfSaleID.ToString().PadLeft(4, '0') + runningNo.ToString().PadLeft(4, '0');
        }

        public bool AddServiceItemToOrder
            (Item myItem, int quantity, decimal amount, decimal GSTAmount, DateTime OrderDetDate, 
             string remark, out string status)
        {
            try
            {
                if (!myItem.IsServiceItem.HasValue || !myItem.IsServiceItem.Value)
                {
                    status = "This item is not a service item";
                    return false;
                }

                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.Remark = remark;
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.IsPreOrder = false;
                tmpdet.UnitPrice = amount - GSTAmount; 
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.OriginalRetailPrice = amount;
                tmpdet.GrossSales = amount;
                tmpdet.Quantity = quantity;
                tmpdet.GSTAmount = GSTAmount;
                tmpdet.Amount = amount;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                
                tmpdet.Discount = 0;
                tmpdet.OrderDetDate = OrderDetDate;
                //CalculateLineAmount(ref tmpdet);
                myOrderDet.Add(tmpdet);
                
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddReceiptLinePayment
            (decimal paymentAmt, string paymentType, string paymentrefno,
            decimal foreignCurrencyRate, string foreignCurrencyCode, decimal foreignCurrencyAmount,
             out decimal change, out string status)
        {
            ////////////////////////////////////////////////////////
            /// DEFINITION:                                      ///
            /// ------------------------------------------------ ///
            /// OrderAmount -> Total amount that need to be paid ///
            /// TotalPaid   -> Total amount that has been paid   ///
            /// paymentAmt  -> amount of current payment         ///
            ////////////////////////////////////////////////////////

            try
            {
                #region *) Initialize: Fill default output parameters
                change = 0;
                status = "";
                #endregion

                status = "";
                

                decimal OrderAmount;
                #region *) Initialize: Get total amount that SHOULD be paid
                /// Calculate Total Amount
                OrderAmount = CalculateTotalAmount(out status);
                #endregion

                decimal TotalPaid;
                #region *) Initialize: Get total amount that HAS BEEN paid AndAlso Update to ReceiptHdr.Amount
                TotalPaid = CalculateTotalPaid(out status);
                if (status != "")
                    throw new Exception("(error)Error while calculating total amount: " + status);
                #endregion

                #region *) Validation: Payment must be bigger than 0 [Exit if False]
                //Validate txtAmount
                /*if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseNegativeSalesAmount), false))
                {
                    if (paymentAmt <= 0)
                        throw new Exception("(warning)Payment must be bigger than 0");
                }*/
                #endregion

                OrderAmount = Decimal.Parse(OrderAmount.ToString("N2"));
                #region *) Validation: Total paid cannot be more than remaining balance (Except Cash will add Change)
                if (Math.Abs(Math.Round(TotalPaid + paymentAmt, 2)) > Math.Abs(OrderAmount))
                {
                    //Accept if voucher payment is more than what is wanted.
                    if (paymentType != POSController.PAY_VOUCHER &&
                        paymentType != POSController.PAY_CASH
                        && paymentType != POSController.PAY_FOREIGN_CURRENCY
                        && !paymentType.StartsWith(POSController.PAY_CASH + "-"))
                    {
                        throw new Exception("(warning)The amount you entered exceeds the amount needed.");
                    }
                    else if (paymentType == POSController.PAY_CASH
                        || paymentType.StartsWith(POSController.PAY_CASH + "-"))
                    {
                        change = TotalPaid + paymentAmt - OrderAmount;

                        paymentAmt = OrderAmount - TotalPaid;
                        //Validate txtAmount
                        if (paymentAmt <= 0)
                        {
                            throw new Exception("(warning)You are not allowed to make anymore payment.");
                        }
                    }
                }
                #endregion

                decimal ExtraCharge;
                #region *) Initialize: Get Extra Charge amount
                //ExtraCharge = CheckExtraChargeAmount(paymentType, paymentAmt);
                #endregion

                #region *) Core: Create / Update Extra Charge information in OrderDetCollection
                //AddExtraCharge(ExtraCharge);
                #endregion

                #region *) Core: Create new ReceiptDet record
                ReceiptDet recDetTmp = new ReceiptDet();
                recDetTmp.Amount = paymentAmt;
                recDetTmp.PaymentRefNo = paymentrefno;
                recDetTmp.ReceiptDetID = recHdr.ReceiptHdrID + "." + recDet.Count;
                recDetTmp.PaymentType = paymentType;
                recDetTmp.ExtraChargeAmount = 0;
                recDetTmp.ForeignCurrencyCode = foreignCurrencyCode;
                recDetTmp.ForeignCurrencyRate = foreignCurrencyRate;
                recDetTmp.ForeignAmountReceived = foreignCurrencyAmount;
                if (paymentType == POSController.PAY_CASH
                        || paymentType.StartsWith(POSController.PAY_CASH + "-"))
                {
                    recDetTmp.Change = change;
                    recDetTmp.ForeignAmountReturned = foreignCurrencyRate == 0 ? 0 : (change / foreignCurrencyRate);
                }
                recDet.Add(recDetTmp);
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                change = 0;
                status = ex.Message;
                return false;
            }
        }

        //Get the maximum OrderDet Line ID
        //use this to add new OrderDet
        private string GetDetMaxID(out string status)
        {
            status = "";
            try
            {
                if (myOrderDet != null)
                {
                    //if the list is empty, start with zero
                    if (myOrderDet.Count == 0) return "1";

                    //otherwise loop through the existing orderdet and find the max value
                    int max = 0;
                    int lineVal;
                    for (int i = 0; i < myOrderDet.Count; i++)
                    {
                        //get the number behind the dot
                        lineVal = int.Parse(myOrderDet[i].OrderDetID.Split('.')[1]);
                        //if it is more than the max, assign it
                        if (max < lineVal)
                        {
                            max = lineVal;
                        }
                    }
                    //return the max plus 1
                    return (max + 1).ToString();
                }
                //if somehow failed, return this
                status = "Order Line Detail has not been created.";
                return "";
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return "";
            }
        }

        #endregion

        #region Static Validation
        public static bool CheckTenantID( string tenantID, out int posid)
        {
            bool result = false;
            posid = 0;
            try
            {
                string sqlString = "Select PointOfSaleID from PointOfSale where TenantMachineID = '" + tenantID + "'";
                object obj = DataService.ExecuteScalar(new QueryCommand(sqlString));
                if (int.TryParse(obj.ToString(), out posid))
                {
                    result = true;
                }

                
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Getting Point Of Sale," + ex.Message);
                result = false;
            }
            return result;
                
        }

        public static bool CheckDate(string Date, int hour, out string status, out DateTime res)
        {
            bool result = false;
            res = DateTime.Today;
            try
            {
                if (Date.Length != 8)
                {
                    status = "Error. Wrong Date Format.";
                    //res = DateTime.Now();
                    return false;
                }

                if (hour < 0 && hour > 23)
                {
                    status = "Error. Wrong Hour Definition";
                    return false;
                }
                res = new DateTime(int.Parse(Date.Substring(0, 4)), int.Parse(Date.Substring(4, 2)), int.Parse(Date.Substring(6, 2)), hour, 0, 0);
                status = "";
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Converting Date," + ex.Message);
                status = "";
                result = false;
            }
            return result;

        }
        #endregion

        #region Confirm Order
        public bool ConfirmOrder(bool doRounding, out bool isPointAllocated, string username, out string status)
        {
            isPointAllocated = false;

            try
            {
                decimal diffPoint = 0;  /// Jumlah point yang akan ditambah / di-deduct ke server
                decimal availablePoint = 0;

                DataTable PackageList = new DataTable();
                status = "";
                

                //Create the command collection to contain the transactions SQL
                QueryCommandCollection cmd = new QueryCommandCollection();
                QueryCommand mycmd;

                #region *) Core: Save OrderHdr Info
                //create a new order refno
                string newOrderHdrID = CreateNewOrderNo(myOrderHdr.PointOfSaleID);
                myOrderHdr.OrderHdrID = newOrderHdrID;
                myOrderHdr.CashierID = username;
                myOrderHdr.OrderRefNo = "OR" + newOrderHdrID;

                //myOrderHdr.OrderDate = DateTime.Now;
                myOrderHdr.UniqueID = Guid.NewGuid();
                myOrderHdr.NettAmount = CalculateTotalAmount(out status); //use total paid > How much customer paid. This amount can be more because of vouchers
                myOrderHdr.GrossAmount = CalculateTotalGrossAmount(out status);
                myOrderHdr.GSTAmount = CalculateTotalGST(out status);
                myOrderHdr.MembershipNo = "WALK-IN";
                myOrderHdr.Userfld5 = myOrderHdr.OrderRefNo;
                
                mycmd = myOrderHdr.GetInsertCommand(UserInfo.username);
                DataService.ExecuteQuery(mycmd);
                #endregion

                //decimal GrossAmount = 0;

                #region *) Core: Save OrderDetCollections
                //Get the query for every order line
                myOrderDet.Sort(OrderDet.Columns.OrderDetDate, false);

                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!myOrderDet[i].IsVoided) //only save non voided transactions
                    {
                        myOrderDet[i].OrderHdrID = newOrderHdrID;
                        myOrderDet[i].UniqueID = Guid.NewGuid();
                        myOrderDet[i].OrderDetID = newOrderHdrID + "." + i.ToString();
                        myOrderDet[i].OrderDetDate = myOrderHdr.OrderDate;
                        myOrderDet[i].CostOfGoodSold = 0; //initialize to allow sync
                        //myOrderDet[i].InventoryHdrRefNo = "";

                        myOrderDet[i].OriginalRetailPrice = myOrderDet[i].Item.RetailPrice;
                        //Calculate Gross Amount and Discount
                        //TODO: Use retail price for open item?                        
                        //myOrderDet[i].OriginalRetailPrice * myOrderDet[i].Quantity;
                        //GrossAmount += myOrderDet[i].GrossSales.Value;

                        if (myOrderDet[i].IsPromo)
                        {
                            myOrderDet[i].Amount = myOrderDet[i].PromoAmount;
                            myOrderDet[i].Discount = (decimal)myOrderDet[i].PromoDiscount;
                        }
                        mycmd = myOrderDet[i].GetInsertCommand(UserInfo.username);
                        DataService.ExecuteQuery(mycmd);

                        
                    }
                }
                #endregion

                #region *) Core: Save ReceiptHdr [contains the payment modes and amounts]
                recHdr.CashierID = username;
                recHdr.ReceiptHdrID = newOrderHdrID;
                recHdr.OrderHdrID = newOrderHdrID;
                recHdr.ReceiptRefNo = "RCP" + newOrderHdrID;
                recHdr.ReceiptDate = myOrderHdr.OrderDate;
                recHdr.PointOfSaleID = myOrderHdr.PointOfSaleID;
                recHdr.Amount = myOrderHdr.NettAmount;
                recHdr.AmountBeforeRounding = myOrderHdr.NettAmount;
                recHdr.UniqueID = Guid.NewGuid();
                mycmd = recHdr.GetInsertCommand(UserInfo.username);
                DataService.ExecuteQuery(mycmd);
                #endregion

                #region *) Core: Save ReceiptDet
                for (int i = 0; i < recDet.Count; i++)
                {
                    recDet[i].ReceiptHdrID = newOrderHdrID;
                    recDet[i].ReceiptDetID = newOrderHdrID + "." + i;
                    recDet[i].UniqueID = Guid.NewGuid();
                    mycmd = recDet[i].GetInsertCommand(UserInfo.username);
                    DataService.ExecuteQuery(mycmd);

                    //if voucher - change voucher status to be redeemed

                    if (recDet[i].PaymentType == POSController.PAY_VOUCHER)
                    {
                        Query qr = Voucher.CreateQuery();
                        qr.AddWhere(Voucher.Columns.VoucherNo, recDet[i].PaymentRefNo);
                        qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, 2);
                        qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, DateTime.Now);
                        //cmd.Add();
                        DataService.ExecuteQuery(qr.BuildUpdateCommand());

                    }
                }
                #endregion

                #region *) Core: Save SalesCommissionRecord
                //if (PointOfSaleInfo.promptSalesPerson)
                {
                    UserMst usm = new UserMst(username);
                    
                    if (SalesPersonInfo.SalesPersonID != "0")
                    {
                        //assign sales person to the sales person
                        SalesCommissionRecord sr = new SalesCommissionRecord();
                        sr.OrderHdrID = newOrderHdrID;
                        sr.SalesGroupID = usm.SalesPersonGroupID;
                        sr.SalesPersonID = "admin";
                        sr.UniqueID = Guid.NewGuid();
                        //cmd.Add();
                        DataService.ExecuteQuery(sr.GetInsertCommand("admin"));
                    }
                    else
                    {
                        //assign commission to th cashier if no sales person selected
                        SalesCommissionRecord sr = new SalesCommissionRecord();
                        sr.OrderHdrID = newOrderHdrID;
                        sr.SalesGroupID = usm.SalesPersonGroupID;
                        sr.SalesPersonID = "admin";
                        sr.UniqueID = Guid.NewGuid();
                        //cmd.Add(sr.GetInsertCommand("admin"));
                        DataService.ExecuteQuery(sr.GetInsertCommand("admin"));
                    }
                }
                #endregion

                status = "";

                #region *) Core: Commit all local transaction
                //SubSonic.DataService.ExecuteTransaction(cmd);
                #endregion

                #region *) PostTransaction: Apply Points from Sales
                #region *) PostTransaction: Set IsPointAllocated as TRUE on OrderHdr
                if (isPointAllocated)
                {
                    myOrderHdr.MarkOld(); /// Without this line, MyOrderHdr will be considered as NEW, and will do Re-Create instead of Update.
                    myOrderHdr.InitialPoint = availablePoint;
                    myOrderHdr.PointAmount = diffPoint;
                    myOrderHdr.IsPointAllocated = true;
                    myOrderHdr.Save();
                }
                #endregion

                

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message.Replace("(warning)", "").Replace("(error)", "");

                //log into logger
                Logger.writeLog(ex);

                return false;
            }
        }
        #endregion
        
        #endregion

        public decimal CalculateTotalGST(out string status)
        {
            try
            {
                status = "";
                decimal totalGST = 0;
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!myOrderDet[i].IsVoided
                        && !myOrderDet[i].IsPromoFreeOfCharge)
                    {
                        totalGST += myOrderDet[i].GSTAmount.GetValueOrDefault(0);
                    }
                }
                return totalGST;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return -1;
            }
        }

        public decimal CalculateTotalAmount(out string status)
        {
            try
            {
                status = "";
                decimal TotalAmount = 0;
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!myOrderDet[i].IsVoided
                        && !myOrderDet[i].IsPromoFreeOfCharge)
                    {
                        if (myOrderDet[i].IsPromo)
                            TotalAmount += myOrderDet[i].PromoAmount;
                        else
                            TotalAmount += myOrderDet[i].Amount;
                    }
                }
                //TotalAmount = TotalAmount * (decimal)(1 + GST / 100);

                //Calculate overall discount
                TotalAmount = TotalAmount * (decimal)(1 - myOrderHdr.Discount / 100);

                //return CommonUILib.RemoveRoundUp(TotalAmount);
                return TotalAmount;

            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return -1;
            }
        }

        public decimal CalculateTotalGrossAmount(out string status)
        {
            try
            {
                status = "";
                decimal TotalAmount = 0;
                for (int i = 0; i < myOrderDet.Count; i++)
                {

                    TotalAmount += myOrderDet[i].GrossSales == null ? 0 : (decimal)myOrderDet[i].GrossSales;
                    
                }
                //TotalAmount = TotalAmount * (decimal)(1 + GST / 100);

                //Calculate overall discount
                //TotalAmount = TotalAmount * (decimal)(1 - myOrderHdr.Discount / 100);

                //return CommonUILib.RemoveRoundUp(TotalAmount);
                return TotalAmount;

            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return -1;
            }
        }



        public decimal CalculateTotalPaid(out string status)
        {
            status = "";
            try
            {
                decimal TotalPaid = 0;

                for (int i = 0; i < recDet.Count; i++)
                {
                    if (recDet[i].PaymentType != POSController.PAY_PACKAGE)
                        TotalPaid += recDet[i].Amount;
                }
                recHdr.Amount = TotalPaid;
                return TotalPaid;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return -1;
            }
        }


    }

    class MallIntegrationData
    {
        public string MallCode;
        public string TenantCode;
        public string Date;
        public string Hour;
        public string TransactionCount;
        public string TotalSalesAfterTax;
        public string TotalSalesBeforeTax;
        public string TotalTax;
    }
}
