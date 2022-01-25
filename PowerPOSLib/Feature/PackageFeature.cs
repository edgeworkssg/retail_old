using DataLogic = PowerPOS;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;
using PowerPOS.Container;

namespace PowerPOS.Feature
{
    public partial class Package
    {
        #region #) Variables & Properties
        public static string ErrMsg_NotAvailable = "(error)Package feature is not available";
        private static PowerPOSLib.PowerPOSSync.Synchronization _ws;

        private static bool _isAvailable = true;
        private static bool _isRealTime = true;

        public static bool isAvailable
        {
            get
            { return _isAvailable; }
            set
            { _isAvailable = value; }
        }
        public static bool isRealTime
        {
            get
            { return _isRealTime; }
            set
            {
                SyncClientController.Load_WS_URL();
                ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;

                //if (ws.Url.ToString().ToLower().Contains("localhost"))
                //    isRealTime = false;

                _isRealTime = value;
            }
        }

        private static PowerPOSLib.PowerPOSSync.Synchronization ws
        {
            get
            {
                if (_ws == null)
                {
                    SyncClientController.Load_WS_URL();
                    _ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    _ws.Timeout = 100000;
                    _ws.Url = SyncClientController.WS_URL;
                }
                return _ws;
            }
            set
            { _ws = value; }
        }
        #endregion

        /// <summary>
        /// Run through all OrderHdr that has not-allocated points/course, and update it all.
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        /// <remarks>
        ///     *) Fully Migrated
        ///     *) Waiting for review process of Update Package
        /// </remarks>
        public static bool AllocatePendingPackage(out string Status)
        {
            try
            {
                #region *) Validation: Is feature available?
                if (!isAvailable) throw new Exception(ErrMsg_NotAvailable);
                #endregion

                #region *) Initialize: Fill default output parameter
                Status = "";
                #endregion

                bool isAllAllocated = true;

                OrderHdrCollection myOrderHdrs;
                #region *) Quarry: Load Outstanding OrderHdr
                string sqlOrderHdrCollector =
                    "SELECT * " +
                    "FROM OrderHdr " +
                    "WHERE (IsPointAllocated = 0 OR IsPointAllocated IS NULL) " +
                        "AND MembershipNo IS NOT NULL AND MembershipNo <> ''";

                myOrderHdrs = new OrderHdrCollection();
                myOrderHdrs.Load(DataService.GetReader(new QueryCommand(sqlOrderHdrCollector)));
                #endregion

                foreach (OrderHdr oneOrderHdr in myOrderHdrs)
                {
                    decimal availablePoint = 0;     /// Total Points before transaction
                    decimal diffPoint = 0;          /// Total Points changed

                    string status;
                    bool isPointAllocated = true;

                    DataTable dt;

                    #region *) Quarry: Fetch the list of uncommited package to DataTable
                    string sqlPackageLoader =
                        "SELECT Item.ItemNo AS RefNo " +
                            ", CASE WHEN (ISNULL(Item." + Item.UserColumns.PointGetAmount + ",0) = 0) THEN RetailPrice ELSE ISNULL(Item." + Item.UserColumns.PointGetAmount + ",0) END AS Amount " +
                            ", Item." + Item.UserColumns.PointGetMode + " AS PointType " +
                        "FROM OrderDet " +
                            "INNER JOIN Item ON OrderDet.ItemNo = Item.ItemNo " +
                        "WHERE OrderHdrID = @OrderHdr " +
                        "UNION ALL " +
                        "SELECT ReceiptDet." + ReceiptDet.UserColumns.PointItemNo + " AS RefNo " +
                            ", 0 - ReceiptDet.Amount AS Amount " +
                            ", '" + Item.PointMode.Dollar + "' AS PointType " +
                        "FROM ReceiptHdr " +
                            "INNER JOIN ReceiptDet ON ReceiptHdr.ReceiptHdrID = ReceiptDet.ReceiptHdrID " +
                        "WHERE ReceiptDet.PaymentType = @PointLabel " +
                            "AND OrderHdrID = @OrderHdr " +
                        "UNION ALL " +
                        "SELECT OrderDet.ItemNo AS RefNo " +
                            ", -1 AS Amount " +
                            ", 'T' AS PointType " +
                        "FROM OrderDet " +
                            "INNER JOIN ReceiptHdr ON OrderDet.OrderHdrID = ReceiptHdr.OrderHdrID " +
                            "INNER JOIN ReceiptDet ON ReceiptHdr.ReceiptHdrID = ReceiptDet.ReceiptHdrID " +
                        "WHERE ReceiptDet.PaymentType = @PackageLabel " +
                            "AND OrderDet.OrderHdrID = @OrderHdr";

                    QueryCommand Cmd = new QueryCommand(sqlPackageLoader);
                    Cmd.AddParameter("@OrderHdr", oneOrderHdr.OrderHdrID, DbType.String);
                    Cmd.AddParameter("@PointLabel", POSController.PAY_POINTS, DbType.String);
                    Cmd.AddParameter("@PackageLabel", POSController.PAY_PACKAGE, DbType.String);

                    dt = new DataTable("PointPackage");
                    dt.Load(SubSonic.DataService.GetReader(Cmd));
                    #endregion

                    string SalesPerson;
                    #region *) Quarry: Get the Sales Person
                    SalesCommissionRecordCollection SalesPersons = oneOrderHdr.SalesCommissionRecordRecords();
                    if (SalesPersons.Count < 1)
                    { SalesPerson = UserInfo.username; }
                    else
                    { SalesPerson = SalesPersons[0].SalesPersonID; }
                    #endregion

                    if (!UpdatePackage(dt, oneOrderHdr.OrderHdrID, oneOrderHdr.OrderDate, 0, oneOrderHdr.MembershipNo, SalesPerson, UserInfo.username, out availablePoint, out diffPoint, out status))
                        throw new Exception(status);

                    #region #) Core: Set IsPointAllocated as TRUE on OrderHdr
                    if (isPointAllocated)
                    {
                        oneOrderHdr.MarkOld(); /// Without this line, MyOrderHdr will be considered as NEW, and will do Insert instead of Update.
                        oneOrderHdr.InitialPoint = availablePoint;
                        oneOrderHdr.PointAmount = diffPoint;
                        oneOrderHdr.IsPointAllocated = true;
                        oneOrderHdr.Save();
                    }
                    #endregion
                }


                Status = "";
                return isAllAllocated;
            }
            catch (Exception X)
            {
                Status = X.Message;
                return false;
            }
        }

        /// <summary>
        /// Run through all OrderHdr that has not-allocated points/course, and update it all.
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        /// <remarks>
        ///     *) Fully Migrated
        ///     *) Waiting for review process of Update Package
        /// </remarks>
        public static bool AllocatePendingPackageServer(out string Status)
        {
            try
            {
                #region *) Validation: Is feature available?
                if (!isAvailable) throw new Exception(ErrMsg_NotAvailable);
                #endregion

                #region *) Initialize: Fill default output parameter
                Status = "";
                #endregion

                bool isAllAllocated = true;

                OrderHdrCollection myOrderHdrs;
                #region *) Quarry: Load Outstanding OrderHdr
                string sqlOrderHdrCollector =
                    "SELECT * " +
                    "FROM OrderHdr " +
                    "WHERE (IsPointAllocated = 0 OR IsPointAllocated IS NULL) " +
                        "AND MembershipNo IS NOT NULL AND MembershipNo <> '' AND MembershipNo <> 'WALK-IN'";

                myOrderHdrs = new OrderHdrCollection();
                myOrderHdrs.Load(DataService.GetReader(new QueryCommand(sqlOrderHdrCollector)));
                #endregion

                foreach (OrderHdr oneOrderHdr in myOrderHdrs)
                {
                    decimal availablePoint = 0;     /// Total Points before transaction
                    decimal diffPoint = 0;          /// Total Points changed

                    string status;
                    bool isPointAllocated = true;

                    DataTable dt;

                    #region *) Quarry: Fetch the list of uncommited package to DataTable
                    string sqlPackageLoader =
                        "SELECT Item.ItemNo AS RefNo " +
                            ", CASE WHEN (ISNULL(Item." + Item.UserColumns.PointGetAmount + ",0) = 0) THEN RetailPrice ELSE ISNULL(Item." + Item.UserColumns.PointGetAmount + ",0) END AS Amount " +
                            ", Item." + Item.UserColumns.PointGetMode + " AS PointType " +
                        "FROM OrderDet " +
                            "INNER JOIN Item ON OrderDet.ItemNo = Item.ItemNo " +
                        "WHERE OrderHdrID = @OrderHdr " +
                        "UNION ALL " +
                        "SELECT ReceiptDet." + ReceiptDet.UserColumns.PointItemNo + " AS RefNo " +
                            ", 0 - ReceiptDet.Amount AS Amount " +
                            ", '" + Item.PointMode.Dollar + "' AS PointType " +
                        "FROM ReceiptHdr " +
                            "INNER JOIN ReceiptDet ON ReceiptHdr.ReceiptHdrID = ReceiptDet.ReceiptHdrID " +
                        "WHERE ReceiptDet.PaymentType = @PointLabel " +
                            "AND OrderHdrID = @OrderHdr " +
                        "UNION ALL " +
                        "SELECT OrderDet.ItemNo AS RefNo " +
                            ", -1 AS Amount " +
                            ", 'T' AS PointType " +
                        "FROM OrderDet " +
                            "INNER JOIN ReceiptHdr ON OrderDet.OrderHdrID = ReceiptHdr.OrderHdrID " +
                            "INNER JOIN ReceiptDet ON ReceiptHdr.ReceiptHdrID = ReceiptDet.ReceiptHdrID " +
                        "WHERE ReceiptDet.PaymentType = @PackageLabel " +
                            "AND OrderDet.OrderHdrID = @OrderHdr";

                    QueryCommand Cmd = new QueryCommand(sqlPackageLoader);
                    Cmd.AddParameter("@OrderHdr", oneOrderHdr.OrderHdrID, DbType.String);
                    Cmd.AddParameter("@PointLabel", POSController.PAY_POINTS, DbType.String);
                    Cmd.AddParameter("@PackageLabel", POSController.PAY_PACKAGE, DbType.String);

                    dt = new DataTable("PointPackage");
                    dt.Load(SubSonic.DataService.GetReader(Cmd));
                    #endregion

                    string SalesPerson;
                    #region *) Quarry: Get the Sales Person
                    SalesCommissionRecordCollection SalesPersons = oneOrderHdr.SalesCommissionRecordRecords();
                    if (SalesPersons.Count < 1)
                    { SalesPerson = UserInfo.username; }
                    else
                    { SalesPerson = SalesPersons[0].SalesPersonID; }
                    #endregion

                    if (!UpdatePackage(dt, oneOrderHdr.OrderHdrID, oneOrderHdr.OrderDate, 0, oneOrderHdr.MembershipNo, SalesPerson, UserInfo.username, out availablePoint, out diffPoint, out status))
                        throw new Exception(status);

                    #region #) Core: Set IsPointAllocated as TRUE on OrderHdr
                    if (isPointAllocated)
                    {
                        oneOrderHdr.MarkOld(); /// Without this line, MyOrderHdr will be considered as NEW, and will do Insert instead of Update.
                        oneOrderHdr.InitialPoint = availablePoint;
                        oneOrderHdr.PointAmount = diffPoint;
                        oneOrderHdr.IsPointAllocated = true;
                        oneOrderHdr.Save();
                    }
                    #endregion
                }


                Status = "";
                return isAllAllocated;
            }
            catch (Exception X)
            {
                Status = X.Message;
                return false;
            }
        }


        /// <summary>
        /// Update package
        /// </summary>
        /// <param name="PointData">Contains 3 columns (ItemNo,Points,PointType)</param>
        /// <param name="OrderHdrID"></param>
        /// <param name="TransactionDate"></param>
        /// <param name="ValidPeriods"></param>
        /// <param name="MembershipNo"></param>
        /// <param name="SalesPersonID"></param>
        /// <param name="UserName"></param>
        /// <param name="InitialPoint"></param>
        /// <param name="DiffPoint"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        /// <remarks>
        ///     *) Fully Migrated
        ///     *) Please review the PointData Type. Don't use DataTable. Easy to make mistake in the future
        /// </remarks>
        public static bool UpdatePackage(DataTable PointData, string OrderHdrID, DateTime TransactionDate
            , int ValidPeriods, string MembershipNo, string SalesPersonID, string UserName
            , out decimal InitialPoint, out decimal DiffPoint, out string Status)
        {
            try
            {
                #region *) Validation: Is feature available?
                if (!isAvailable) throw new Exception(ErrMsg_NotAvailable);
                #endregion

                #region *) Initialize: Fill default output parameter
                Status = "";
                InitialPoint = 0;
                DiffPoint = 0;
                #endregion

                #region *) Validation: Is PointData given in the correct format?
                /// If there is no data, just consider it as success
                if (PointData == null) return true;
                if (PointData.Rows.Count < 1) return true;

                if (PointData.Columns.Count != 3)
                    throw new Exception("(error)Input data format is wrong. Need 3 Columns (string,decimal,string)");

                if (PointData.Columns[0].DataType != Type.GetType("System.String"))
                    throw new Exception("(error)Input data format is wrong. Need 3 Columns (string,decimal,string)");
                if (PointData.Columns[1].DataType != Type.GetType("System.Decimal"))
                    throw new Exception("(error)Input data format is wrong. Need 3 Columns (string,decimal,string)");
                if (PointData.Columns[2].DataType != Type.GetType("System.String"))
                    throw new Exception("(error)Input data format is wrong. Need 3 Columns (string,decimal,string)");
                #endregion

                #region *) Core: Update the package
                if (isRealTime)
                {
                    try
                    {
                        return ws.UpdateAll(PointData, OrderHdrID, TransactionDate, ValidPeriods
                            , MembershipNo, SalesPersonID, UserName, out InitialPoint, out DiffPoint, out  Status);
                    }
                    catch (Exception X)
                    {
                        Logger.writeLog(X);
                        throw new Exception("(error)Cannot connect to server");
                    }
                }
                else
                {
                    if (!PackageController.UpdateAll(PointData, OrderHdrID, TransactionDate, ValidPeriods, MembershipNo, SalesPersonID, UserName, out InitialPoint, out DiffPoint, out Status))
                        throw new Exception(Status);
                }
                #endregion

                return true;
            }
            catch (Exception X)
            {
                Status = X.Message;
                InitialPoint = 0;
                DiffPoint = 0;

                return false;
            }
        }

        public static bool UpdatePackageServer(DataTable PointData, string OrderHdrID, DateTime TransactionDate
            , int ValidPeriods, string MembershipNo, string SalesPersonID, string UserName
            , out decimal InitialPoint, out decimal DiffPoint, out string Status)
        {
            try
            {
                #region *) Validation: Is feature available?
                if (!isAvailable) throw new Exception(ErrMsg_NotAvailable);
                #endregion

                #region *) Initialize: Fill default output parameter
                Status = "";
                InitialPoint = 0;
                DiffPoint = 0;
                #endregion

                #region *) Validation: Is PointData given in the correct format?
                /// If there is no data, just consider it as success
                if (PointData == null) return true;
                if (PointData.Rows.Count < 1) return true;

                if (PointData.Columns.Count != 3)
                    throw new Exception("(error)Input data format is wrong. Need 3 Columns (string,decimal,string)");

                if (PointData.Columns[0].DataType != Type.GetType("System.String"))
                    throw new Exception("(error)Input data format is wrong. Need 3 Columns (string,decimal,string)");
                if (PointData.Columns[1].DataType != Type.GetType("System.Decimal"))
                    throw new Exception("(error)Input data format is wrong. Need 3 Columns (string,decimal,string)");
                if (PointData.Columns[2].DataType != Type.GetType("System.String"))
                    throw new Exception("(error)Input data format is wrong. Need 3 Columns (string,decimal,string)");
                #endregion

                #region *) Core: Update the package

                if (!PackageController.UpdateAll(PointData, OrderHdrID, TransactionDate, ValidPeriods, MembershipNo, SalesPersonID, UserName, out InitialPoint, out DiffPoint, out Status))
                    throw new Exception(Status);

                #endregion

                return true;
            }
            catch (Exception X)
            {
                Status = X.Message;
                InitialPoint = 0;
                DiffPoint = 0;

                return false;
            }
        }

        private static decimal getTotalExcludedAmountForPointsPercentageCalculation(ReceiptDetCollection myReceiptDet)
        {

            //status = "";
            //
            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.ExcludePaymentTypeForPointsCalculation), false))
                return 0;

            string paymentTypeList = AppSetting.GetSetting(AppSetting.SettingsName.Points.ExcludedPaymentTypes);
            if (paymentTypeList == "")
                return 0;

            //finish validation then get the amount
            foreach (ReceiptDet rd in myReceiptDet)
            {
                if (rd.PaymentType.ToLower() == paymentTypeList.ToLower())
                {
                    return rd.Amount;
                }
            }



            return 0;
        }

        /// <summary>
        /// Extract all changes in packages from Order and Receipt
        /// </summary>
        /// <param name="myOrderDet"></param>
        /// <param name="myReceiptDet"></param>
        /// <param name="Output"></param>
        /// <param name="DiffPoint"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        /// <remarks>
        ///     *) Fully Migrated
        /// </remarks>
        public static bool BreakDownPackageChangesList
            (OrderDetCollection myOrderDet, ReceiptDetCollection myReceiptDet,
                decimal PointPercentage
            , out DataTable Output, out decimal DiffPoint, out string Status)
        {
            #region *) Initialize: Fill default output parameter
            Output = new DataTable("PackageList");
            Output.Columns.Add("Key", Type.GetType("System.String"));
            Output.Columns.Add("Value", Type.GetType("System.Decimal"));
            Status = "";
            DiffPoint = 0;
            #endregion

            try
            {
                string PercentagePointsName = AppSetting.GetSetting(AppSetting.SettingsName.Points.PercentagePointName);

                if (PercentagePointsName == null ||
                    AppSetting.GetSetting(AppSetting.SettingsName.Points.IsUsingPercentage).ToLower() != "true")
                    PointPercentage = 0;

                decimal PaidByPoints = 0; /// For the Points by Percentage only
                decimal PaidByInstallment = 0; /// For the Points by Percentage only
                decimal AmountToExcludeFromRecDet = getTotalExcludedAmountForPointsPercentageCalculation(myReceiptDet);  /// For the Points by Percentage only

                ///Load Payment for r 

                #region *) Load all the Paid By Points
                foreach (ReceiptDet oneRecDet in myReceiptDet)
                {
                    if (oneRecDet.PaymentType == POSController.PAY_POINTS)
                        PaidByPoints += oneRecDet.Amount;

                    if (oneRecDet.PaymentType == POSController.PAY_INSTALLMENT)
                        PaidByInstallment += oneRecDet.Amount;
                }
                #endregion

                string orderHdrID = "";
                bool isInstPayment = false;
                string instRefNo = "";
                decimal PointsGet = 0;
                decimal PointsRefund = 0;
                #region *) Load all Received Points & Course package [From OrderDet]
                foreach (OrderDet oneOrderDet in myOrderDet)
                {
                    orderHdrID = oneOrderDet.OrderHdrID;

                    #region *) Validation: Ignore Voided OrderDet
                    if (oneOrderDet.IsVoided) continue;
                    #endregion

                    Item oneItem = oneOrderDet.Item;
                    if (oneOrderDet.ItemNo == Installment.CreditPaymentName)
                    {
                        instRefNo = oneOrderDet.InstRefNo;
                        isInstPayment = true;
                    }

                    #region *) Validation: Calculate Non Package Item (To be point by percentage)
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.WontGetRewardPointsIfBuyPackageItem), true))
                    {
                        // Will NOT get reward points from point/package items
                        if (oneItem.PointGetMode != Item.PointMode.Dollar &&
                            oneItem.PointGetMode != Item.PointMode.Times)
                        {
                            if (oneOrderDet.Quantity >= 0)
                            {
                                PointsGet += oneOrderDet.Amount;
                                if (PointsGet > PaidByPoints)
                                {
                                    PointsGet -= PaidByPoints;
                                    PaidByPoints = 0;
                                }
                                else
                                {
                                    PaidByPoints -= PointsGet;
                                    PointsGet = 0;
                                }
                            }
                            else
                            {
                                PointsRefund += oneOrderDet.Amount;
                            }
                            continue;
                        }
                    }
                    else
                    {
                        // Will get reward points from point/package items
                        //if refund
                        if (oneOrderDet.Quantity >= 0)
                        {
                            PointsGet += oneOrderDet.Amount;
                            if (PointsGet > PaidByPoints)
                            {
                                PointsGet -= PaidByPoints;
                                PaidByPoints = 0;
                            }
                            else
                            {
                                PaidByPoints -= PointsGet;
                                PointsGet = 0;
                            }
                        }
                        else
                        {
                            PointsRefund += oneOrderDet.Amount;
                        }

                        if (oneItem.PointGetMode != Item.PointMode.Dollar &&
                            oneItem.PointGetMode != Item.PointMode.Times)
                        {
                            continue;
                        }
                    }
                    #endregion

                    #region *) Core: Add Point Details (1 line for 1 Quantity, 5 Lines for 5 Quantities)
                    for (int Counter = 0; Counter < Math.Abs(oneOrderDet.Quantity.GetValueOrDefault(0)); Counter++)
                    {
                        if (oneItem.IsOpenPricePackage)
                        {
                            /*
                             * Check purchase or usage
                             */
                            if (string.IsNullOrEmpty(oneOrderDet.PointItemNo))
                            {
                                if (oneOrderDet.Quantity >= 0)
                                {
                                    object[] myobject = new object[2];
                                    myobject[0] = oneItem.ItemNo + "|OPP|" + (Convert.ToDecimal(oneOrderDet.UnitPrice * oneOrderDet.Quantity)).ToString("N2").Replace(",", "").PadLeft(12, '0') + DateTime.Now.ToString("yyyyMMddHHmmss") +
                                                    oneOrderDet.Remark;
                                    myobject[1] = oneOrderDet.PointGetAmount;
                                    Output.Rows.Add(myobject);
                                }
                                else
                                {
                                    //Query qry = new Query("PointAllocationLog");
                                    //qry.WHERE("OrderHdrID = " + tmpOD.OrderHdrID);
                                    //qry.WHERE("MembershipNo = " + tmpOD.OrderHdr.MembershipNo);
                                    //qry.WHERE("userfld1 like " + tmpOD.ItemNo + "|OPP|%");

                                    // Get userfld1 value from PointAllocationLog of the original transaction first
                                    OrderDet tmpOD = new OrderDet(oneOrderDet.RefundOrderDetID);
                                    PointAllocationLogCollection palog = new PointAllocationLogCollection();
                                    palog.Where("OrderHdrID", tmpOD.OrderHdrID);
                                    palog.Where("MembershipNo", tmpOD.OrderHdr.MembershipNo);
                                    palog.Where("userfld1", Comparison.Like, tmpOD.ItemNo + "|OPP|%");
                                    palog.Load();
                                    if (palog.Count > 0)
                                    {
                                        object[] myobject = new object[2];
                                        myobject[0] = palog[0].Userfld1;
                                        myobject[1] = -oneOrderDet.PointGetAmount;
                                        Output.Rows.Add(myobject);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (oneOrderDet.Quantity > 0)
                            {
                                Output.Rows.Add(new object[] 
                                { 
                                    oneItem.ItemNo, 
                                    (oneItem.PointGetAmount == 0 ? (oneOrderDet.Amount / oneOrderDet.Quantity.GetValueOrDefault(0)) : oneItem.PointGetAmount) 
                                });
                            }
                            else if (oneOrderDet.Quantity < 0)
                            {
                                Output.Rows.Add(new object[] 
                                { 
                                    oneItem.ItemNo, 
                                    (oneItem.PointGetAmount == 0 ? (oneOrderDet.Amount / oneOrderDet.Quantity.GetValueOrDefault(0)) : -oneItem.PointGetAmount) 
                                });
                            }
                        }
                    }
                    #endregion

                    if (oneItem.PointGetMode == Item.PointMode.Dollar)
                    {
                        #region *) Core: Summarize total points changed
                        DiffPoint += oneOrderDet.Quantity.GetValueOrDefault(0) * (oneItem.PointGetAmount == 0 ? oneItem.RetailPrice : oneItem.PointGetAmount);
                        #endregion
                    }
                }

                PointsGet += PointsRefund;
                #endregion

                decimal pendingReward = 0;
                decimal calcPointsGet = 0;
                #region *) Paid by installment should not be added to point reward
                if (PaidByInstallment > 0)
                {
                    if (PointsGet > PaidByInstallment)
                    {
                        pendingReward = PaidByInstallment;
                        PointsGet = PointsGet - PaidByInstallment;
                    }
                    else
                    {
                        pendingReward = PointsGet;
                        PointsGet = 0;
                    }
                }
                #endregion

                if (AmountToExcludeFromRecDet > 0)
                {
                    if (PointsGet > AmountToExcludeFromRecDet)
                    {
                        //pendingReward = AmountToExcludeFromRecDet;
                        PointsGet = PointsGet - AmountToExcludeFromRecDet;
                    }
                    else
                    {
                        //pendingReward = PointsGet;
                        PointsGet = 0;
                    }
                }


                decimal maxPointsGet = decimal.MaxValue;
                if (isInstPayment)
                {
                    OrderHdr tmpOH = new OrderHdr(instRefNo);
                    if (tmpOH != null && !string.IsNullOrEmpty(tmpOH.OrderHdrID))
                        maxPointsGet = tmpOH.PendingReward.GetValueOrDefault(0);
                }

                //PointsGet = PointsGet * PointPercentage / 100;
                //if (PointsGet >= 0)
                //{
                    string pointsRounding = AppSetting.GetSetting(AppSetting.SettingsName.Points.Rounding);
                    if (string.IsNullOrEmpty(pointsRounding) || pointsRounding.ToUpper() == "NONE")
                    {
                        calcPointsGet = PointsGet * PointPercentage / 100;
                        if (calcPointsGet > maxPointsGet) calcPointsGet = maxPointsGet;
                        Output.Rows.Add(new object[] { PercentagePointsName, calcPointsGet });
                        pendingReward = pendingReward * PointPercentage / 100;
                    }
                    else if (pointsRounding.ToUpper() == "ROUND UP")
                    {
                        calcPointsGet = Math.Ceiling(PointsGet * PointPercentage / 100);
                        if (calcPointsGet > maxPointsGet) calcPointsGet = maxPointsGet;
                        Output.Rows.Add(new object[] { PercentagePointsName, calcPointsGet });
                        pendingReward = Math.Ceiling(pendingReward * PointPercentage / 100);
                    }
                    else if (pointsRounding.ToUpper() == "ROUND DOWN")
                    {
                        calcPointsGet = Math.Floor(PointsGet * PointPercentage / 100);
                        if (calcPointsGet > maxPointsGet) calcPointsGet = maxPointsGet;
                        Output.Rows.Add(new object[] { PercentagePointsName, calcPointsGet });
                        pendingReward = Math.Floor(pendingReward * PointPercentage / 100);
                    }
                    else
                    {
                        calcPointsGet = PointsGet * PointPercentage / 100;
                        if (calcPointsGet > maxPointsGet) calcPointsGet = maxPointsGet;
                        Output.Rows.Add(new object[] { PercentagePointsName, calcPointsGet });
                        pendingReward = pendingReward * PointPercentage / 100;
                    }
                //}

                if (PaidByInstallment > 0)
                {
                    OrderHdr tmpOH = new OrderHdr(orderHdrID);
                    if (tmpOH != null && !string.IsNullOrEmpty(tmpOH.OrderHdrID))
                    {
                        tmpOH.PendingReward = pendingReward;
                        tmpOH.TotalReward = pendingReward;
                        string sqlString = "Update OrderHdr set Userfloat3 = " + pendingReward.ToString("N3") + ", userfloat4 = " + pendingReward.ToString("N3") +
                            " where orderhdrid = '" + tmpOH.OrderHdrID + "'";
                        //tmpOH.Save(tmpOH.CreatedBy);
                        DataService.ExecuteQuery(new QueryCommand(sqlString));
                    }
                }

                if (isInstPayment)
                {
                    OrderHdr tmpOH = new OrderHdr(instRefNo);
                    if (tmpOH != null && !string.IsNullOrEmpty(tmpOH.OrderHdrID))
                    {
                        tmpOH.PendingReward -= calcPointsGet;
                        //tmpOH.Save(tmpOH.ModifiedBy);
                        string sqlString = "Update OrderHdr set Userfloat3 = " + pendingReward.ToString("N3") +
                            " where orderhdrid = '" + tmpOH.OrderHdrID + "'";
                        //tmpOH.Save(tmpOH.CreatedBy);
                        DataService.ExecuteQuery(new QueryCommand(sqlString));
                    }
                }

                #region *) Load all Used Points Package [From ReceiptDet]
                foreach (ReceiptDet oneRecDet in myReceiptDet)
                {
                    if (oneRecDet.PaymentType == POSController.PAY_POINTS)
                    {
                        #region *) Core: Add Point Details
                        Output.Rows.Add(new object[] { oneRecDet.PointItemNo, 0 - oneRecDet.Amount });
                        #endregion
                        #region *) Core: Summarize total points changed
                        DiffPoint -= oneRecDet.Amount;
                        #endregion
                    }
                }
                #endregion

                #region *) Validation: Is feature available?
                if (!isAvailable)
                {
                    if (Output.Rows.Count > 0)
                    { throw new Exception(ErrMsg_NotAvailable); }
                    else
                    { return true; }
                }
                #endregion

                return true;
            }
            catch (Exception X)
            {
                Status = X.Message;
                DiffPoint = 0;
                return false;
            }
        }

        /// <summary>
        /// Try to update points. The data is not commited to database.
        /// </summary>
        /// <param name="AddedPoints"></param>
        /// <param name="DeductedPoints"></param>
        /// <param name="Result"></param>
        /// <returns>
        ///     *) Fully Migrated
        ///     *) False if Error occured and need to terminate process - Error is handled inside
        /// </returns>
        public static bool BreakAmountIntoReceipts(string MembershipNo, DateTime CurrentDate, SortedList<string, decimal> AddedPoints, decimal DeductedPoints, ref POSController pos, out string Status)
        {
            try
            {
                #region *) Initialize: Fill default output parameter
                Status = "";
                #endregion

                #region *) Validation: Is feature available?
                if (!isAvailable) return true;
                #endregion

                #region *) Validation: Is Membership Applied?
                if (!pos.MembershipApplied())
                    throw new Exception("(warning)Please assign member to use points");
                #endregion

                DataTable CurrentAmounts;

                #region *) Load CurrentAmount from Database/WebService
                if (isRealTime)
                {
                    if (!ws.GetCurrentPointsAmount(MembershipNo, CurrentDate, out CurrentAmounts))
                        throw new Exception("(warning)Cannot get point from server\nPlease try again");
                }
                else
                {
                    if (!PackageController.GetCurrentAmount_Points(MembershipNo, CurrentDate, out CurrentAmounts, out Status))
                        return false;
                }
                #endregion

                decimal AvailablePoint = 0;
                decimal AddedPoint = 0;
                decimal RunningPoint = DeductedPoints;
                int Counter = 0;

                if (DeductedPoints < 0)  // Pay refund transaction with points
                {
                    string CurrentPointRefNo = "";  /// Reference to ItemNo
                    if (CurrentAmounts.Rows.Count > 0)
                        CurrentPointRefNo = CurrentAmounts.Rows[0]["RefNo"].ToString();
                    else
                        CurrentPointRefNo = AppSetting.GetSetting(AppSetting.SettingsName.Points.PercentagePointName);

                    if (!pos.AddReceiptLinePayment_Points(DeductedPoints, POSController.PAY_POINTS, CurrentPointRefNo, out Status))
                        throw new Exception("(warning)Cannot add payment.\n" + Status);
                    RunningPoint = 0;
                }
                else
                {
                    #region *) Try to DeductPoint and Separate ReceiptDet - Check From DataBase
                    while (RunningPoint > 0 && Counter < CurrentAmounts.Rows.Count)
                    {
                        decimal CurrentPoint = decimal.Parse(CurrentAmounts.Rows[Counter]["Points"].ToString());
                        string CurrentPointRefNo = CurrentAmounts.Rows[Counter]["RefNo"].ToString(); /// Reference to ItemNo

                        if (RunningPoint > CurrentPoint)
                        {
                            if (!pos.AddReceiptLinePayment_Points(CurrentPoint, POSController.PAY_POINTS, CurrentPointRefNo, out Status))
                                throw new Exception("(warning)Cannot add payment.\n" + Status);
                            AvailablePoint += CurrentPoint;
                            RunningPoint = RunningPoint - CurrentPoint;
                        }
                        else
                        {
                            if (!pos.AddReceiptLinePayment_Points(RunningPoint, POSController.PAY_POINTS, CurrentPointRefNo, out Status))
                                throw new Exception("(warning)Cannot add payment.\n" + Status);
                            AvailablePoint += RunningPoint;
                            RunningPoint = 0;
                        }
                        Counter++;
                    }
                    #endregion
                }

                /// the following code will deduct from newly added points
                /// Please review before use
                #region *) Try to DeductPoint and Separate ReceiptDet - Check From AddedPoints [Disabled]
                //Counter = 0;
                //while (RunningPoint > 0 && Counter < AddedPoints.Count)
                //{
                //    decimal CurrentPoint = AddedPoints.Values[Counter];
                //    string CurrentPointRefNo = AddedPoints.Keys[Counter]; /// Reference to ItemNo

                //    if (RunningPoint > CurrentPoint)
                //    {
                //        if (!pos.AddReceiptLinePayment_Points(CurrentPoint, POSController.PAY_POINTS, CurrentPointRefNo))
                //            throw new Exception("(warning)Cannot add payment.\nPlease contact your administrator");
                //        AddedPoint += CurrentPoint;
                //        RunningPoint = RunningPoint - CurrentPoint;
                //    }
                //    else
                //    {
                //        if (!pos.AddReceiptLinePayment_Points(RunningPoint, POSController.PAY_POINTS, CurrentPointRefNo))
                //            throw new Exception("(warning)Cannot add payment.\nPlease contact your administrator");
                //        AddedPoint += RunningPoint;
                //        RunningPoint = 0;
                //    }
                //    Counter++;
                //}
                #endregion

                if (RunningPoint > 0)
                    throw new Exception("(warning)Point is not sufficient\nPoints stored in database = " + AvailablePoint.ToString("N2")
                        /*+ "\nPoints added = " + AddedPoint.ToString("N2") */+ "\nPoints used = " + DeductedPoints.ToString("N2"));

                return true;
            }
            catch (Exception X)
            {
                Status = X.Message;

                return false;
            }
        }

        /// <summary>
        /// Try to update points. The data is not commited to database.
        /// </summary>
        /// <param name="AddedPoints"></param>
        /// <param name="DeductedPoints"></param>
        /// <param name="Result"></param>
        /// <returns>
        ///     *) Fully Migrated
        ///     *) False if Error occured and need to terminate process - Error is handled inside
        /// </returns>
        public static bool PayReceiptsByPoints(string MembershipNo, DateTime CurrentDate, string ItemNo, SortedList<string, decimal> AddedPoints, decimal DeductedPoints, ref POSController pos, out string Status)
        {
            try
            {
                #region *) Initialize: Fill default output parameter
                Status = "";
                #endregion

                #region *) Validation: Is feature available?
                if (!isAvailable) return true;
                #endregion

                #region *) Validation: Is Membership Applied?
                if (!pos.MembershipApplied())
                    throw new Exception("(warning)Please assign member to use points");
                #endregion

                DataTable CurrentAmounts;

                #region *) Load CurrentAmount from Database/WebService
                if (isRealTime)
                {
                    if (!ws.GetCurrentPointsAmount(MembershipNo, CurrentDate, out CurrentAmounts))
                        throw new Exception("(warning)Cannot get point from server\nPlease try again");
                }
                else
                {
                    if (!PackageController.GetCurrentAmount_Points(MembershipNo, CurrentDate, out CurrentAmounts, out Status))
                        return false;
                }
                #endregion

                decimal AvailablePoint = 0;
                decimal AddedPoint = 0;
                decimal RunningPoint = DeductedPoints;
                decimal Amount = 0;
                int Counter = 0;
                #region *) Try to DeductPoint and Separate ReceiptDet - Check From DataBase
                while (RunningPoint > 0 && Counter < CurrentAmounts.Rows.Count)
                {
                    decimal CurrentPoint = decimal.Parse(CurrentAmounts.Rows[Counter]["Points"].ToString());
                    string CurrentPointRefNo = CurrentAmounts.Rows[Counter]["RefNo"].ToString(); /// Reference to ItemNo

                    if (CurrentPointRefNo == ItemNo)
                    {
                        string st;
                        Amount = pos.CalculateTotalPaid_ByPointsByName(CurrentPointRefNo, out st);
                        AvailablePoint += CurrentPoint;
                        if (RunningPoint > (CurrentPoint - Amount))
                        {
                            /*    if (!pos.AddReceiptLinePayment_Points(CurrentPoint, POSController.PAY_POINTS, CurrentPointRefNo, out Status))
                                    throw new Exception("(warning)Cannot add payment.\n" + Status);
                                AvailablePoint += CurrentPoint;
                                RunningPoint = RunningPoint - CurrentPoint;*/
                        }
                        else
                        {
                            if (!pos.AddReceiptLinePayment_Points(RunningPoint, POSController.PAY_POINTS, CurrentPointRefNo, out Status))
                                throw new Exception("(warning)Cannot add payment.\n" + Status);
                            //AvailablePoint += RunningPoint;
                            RunningPoint = 0;
                        }
                    }
                    Counter++;
                }
                #endregion

                /// the following code will deduct from newly added points
                /// Please review before use
                #region *) Try to DeductPoint and Separate ReceiptDet - Check From AddedPoints [Disabled]
                //Counter = 0;
                //while (RunningPoint > 0 && Counter < AddedPoints.Count)
                //{
                //    decimal CurrentPoint = AddedPoints.Values[Counter];
                //    string CurrentPointRefNo = AddedPoints.Keys[Counter]; /// Reference to ItemNo

                //    if (RunningPoint > CurrentPoint)
                //    {
                //        if (!pos.AddReceiptLinePayment_Points(CurrentPoint, POSController.PAY_POINTS, CurrentPointRefNo))
                //            throw new Exception("(warning)Cannot add payment.\nPlease contact your administrator");
                //        AddedPoint += CurrentPoint;
                //        RunningPoint = RunningPoint - CurrentPoint;
                //    }
                //    else
                //    {
                //        if (!pos.AddReceiptLinePayment_Points(RunningPoint, POSController.PAY_POINTS, CurrentPointRefNo))
                //            throw new Exception("(warning)Cannot add payment.\nPlease contact your administrator");
                //        AddedPoint += RunningPoint;
                //        RunningPoint = 0;
                //    }
                //    Counter++;
                //}
                #endregion

                if (RunningPoint > 0)
                    throw new Exception("(warning)Point is not sufficient " /*+ Points stored in database = " + (AvailablePoint - Amount).ToString("N2")*/
                        /*+ "\nPoints added = " + AddedPoint.ToString("N2") */ + "\nPoints used = " + Amount.ToString("N2"));

                return true;
            }
            catch (Exception X)
            {
                Status = X.Message;

                return false;
            }
        }

        public static string[] getPackageListLocal(string MembershipNo, out string Status)
        {
            string[] Output;
            try
            {
                #region *) Validation: Is feature available?
                if (!isAvailable) throw new Exception(ErrMsg_NotAvailable);
                #endregion

                #region *) Initialize: Fill default output parameter
                Status = "";
                #endregion

                #region *) Core: get Package List
                bool includeZeroRemaining = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowPackageEvenIfRemainingIsZero), true);
                Output = MembershipController.getRemainingPackageList(MembershipNo, includeZeroRemaining);
                #endregion

                return Output;
            }
            catch (Exception X)
            {
                Status = X.Message;
                return null;
            }
        }

        public static string[] getPackageList(string MembershipNo, out string Status)
        {
            string[] Output;
            try
            {
                #region *) Validation: Is feature available?
                if (!isAvailable) throw new Exception(ErrMsg_NotAvailable);
                #endregion

                #region *) Initialize: Fill default output parameter
                Status = "";
                #endregion

                #region *) Core: get Package List
                if (isRealTime)
                {
                    try
                    {
                        //Output = ws.GetHistory_Buttons(MembershipNo);
                        Output = ws.getPackageList(MembershipNo);

                    }
                    catch (Exception X)
                    {
                        Logger.writeLog(X);
                        throw new Exception("(error)Cannot connect to server");
                    }
                }
                else
                {
                    Output = MembershipController.getRemainingPackageList(MembershipNo);
                }
                #endregion

                return Output;
            }
            catch (Exception X)
            {
                Status = X.Message;
                return null;
            }
        }

        public static bool RevertPackage(string OrderHdrID, DateTime TransactionDate, string MembershipNo, string SalesPersonID, string UserName, out string Status)
        {
            Status = "";
            try
            {
                #region *) Validation: Is feature available?
                if (!isAvailable) return true; /// Not important to be checked
                #endregion

                #region *) Validation: is Package (from transaction) allocated?
                OrderHdr myOrderHdr = new OrderHdr(OrderHdrID);
                //if (!myOrderHdr.IsPointAllocated)
                //    throw new Exception("(warning)Point has not been updated.\nUsually, this is happened because of some internet connection problem\nPlease do manual update before void this receipt.");
                #endregion

                #region *) Core: Revert Point
                if (isRealTime)
                {
                    try
                    {
                        return ws.RevertPoints(OrderHdrID, TransactionDate, MembershipNo, SalesPersonID, UserName, out  Status);
                    }
                    catch (Exception X)
                    {
                        Logger.writeLog(X);
                        throw new Exception("(error)Cannot connect to server");
                    }
                }
                else
                {
                    return PackageController.RevertPoints(OrderHdrID, TransactionDate, MembershipNo, SalesPersonID, UserName, out Status);
                }
                #endregion
            }
            catch (Exception X)
            {
                Status = X.Message;

                return false;
            }

        }

        public static bool RevertPackageServer(string OrderHdrID, DateTime TransactionDate, string MembershipNo, string SalesPersonID, string UserName, out string Status)
        {
            Status = "";
            try
            {
                #region *) Validation: Is feature available?
                if (!isAvailable) return true; /// Not important to be checked
                #endregion

                #region *) Validation: is Package (from transaction) allocated?
                OrderHdr myOrderHdr = new OrderHdr(OrderHdrID);
                if (!isPointAllocatedForOrderHdrID(OrderHdrID))
                    return true;
                //if (!myOrderHdr.IsPointAllocated)
                //throw new Exception("(warning)Point has not been updated.\nUsually, this is happened because of some internet connection problem\nPlease do manual update before void this receipt.");
                #endregion

                #region *) Core: Revert Point

                return PackageController.RevertPoints(OrderHdrID, TransactionDate, MembershipNo, SalesPersonID, UserName, out Status);

                #endregion
            }
            catch (Exception X)
            {
                Status = X.Message;

                return false;
            }

        }

        public static bool isPointAllocatedForOrderHdrID(string orderHdrID)
        {
            bool res = false;
            PointAllocationLogCollection posCol = new PointAllocationLogCollection();
            posCol.Where(PointAllocationLog.Columns.OrderHdrID, orderHdrID);
            posCol.Load();

            if (posCol.Count > 0)
                res = true;

            return res;

        }

        public static bool GetCurrentBreakdownPrice(string membershipNo, DateTime CurrentDate, string PackageRefNo, out decimal BreakdownPrice, out string Status)
        {
            #region *) Validation: Is feature available?
            if (!isAvailable) throw new Exception(ErrMsg_NotAvailable);
            #endregion

            #region *) Initialize: Fill default output parameter
            BreakdownPrice = 0;
            Status = "";
            #endregion

            try
            {
                /*if (isRealTime)
                {
                    try
                    {
                        return ws.GetPackageBreakdown(membershipNo, CurrentDate, PackageRefNo, out BreakdownPrice, out Status);
                    }
                    catch (Exception X)
                    {
                        Logger.writeLog(X);
                        throw new Exception("(Error)Cannot connect to server. Please check your connection.");
                    }
                }
                else
                {*/
                bool ActionResult;
                ActionResult = PackageController.GetCurrentBreakdown(membershipNo, CurrentDate, PackageRefNo, out BreakdownPrice, out Status);
                return ActionResult;
                //}

            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                Status = X.Message;
                BreakdownPrice = 0;
                return false;
            }
        }

        public static bool GetCurrentAmount_andBreakdownPrice(string membershipNo, DateTime CurrentDate, string PackageRefNo, out decimal CurrentAmount, out decimal BreakdownPrice, out string Status)
        {
            #region *) Validation: Is feature available?
            if (!isAvailable) throw new Exception(ErrMsg_NotAvailable);
            #endregion

            #region *) Initialize: Fill default output parameter
            CurrentAmount = 0;
            BreakdownPrice = 0;
            Status = "";
            #endregion

            try
            {
                if (isRealTime)
                {
                    try
                    {
                        return ws.GetPackageAmount_plusBreakdown(membershipNo, CurrentDate, PackageRefNo, out CurrentAmount, out BreakdownPrice, out Status);
                    }
                    catch (Exception X)
                    {
                        Logger.writeLog(X);
                        throw new Exception("(error)Cannot connect to server");
                    }
                }
                else
                {
                    bool ActionResult;
                    ActionResult = PackageController.GetCurrentBreakdown(membershipNo, CurrentDate, PackageRefNo, out BreakdownPrice, out Status);
                    ActionResult = ActionResult && PackageController.GetCurrentAmount(membershipNo, CurrentDate, PackageRefNo, out CurrentAmount, out Status);
                    return ActionResult;
                }

            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                Status = X.Message;
                CurrentAmount = 0;
                BreakdownPrice = 0;
                return false;
            }
        }

        public static bool GetCurrentAmount(string membershipNo, DateTime CurrentDate, string PackageRefNo, out decimal Output, out string Status)
        {
            #region *) Validation: Is feature available?
            if (!isAvailable) throw new Exception(ErrMsg_NotAvailable);
            #endregion

            #region *) Initialize: Fill default output parameter
            Output = 0;
            Status = "";
            #endregion

            try
            {
                if (isRealTime)
                {
                    try
                    {
                        return ws.GetPackageAmount(membershipNo, CurrentDate, PackageRefNo, out Output, out Status);
                    }
                    catch (Exception X)
                    {
                        Logger.writeLog(X);
                        throw new Exception("(error)Cannot connect to server");
                    }
                }
                else
                {
                    return PackageController.GetCurrentAmount(membershipNo, CurrentDate, PackageRefNo, out Output, out Status);
                }

            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                Status = X.Message;
                Output = 0;
                return false;
            }
        }
        public static bool GetCurrentAmount(string membershipNo, DateTime CurrentDate, out DataTable Output, out string Status)
        {
            try
            {
                #region *) Validation: Is feature available?
                if (!isAvailable) throw new Exception(ErrMsg_NotAvailable); /// Process is success but return null
                #endregion

                #region *) Initialize: Fill default output parameter
                Output = null;
                Status = "";
                #endregion

                if (isRealTime)
                {
                    try
                    {
                        return ws.GetPackageAmounts(membershipNo, CurrentDate, out Output, out Status);
                    }
                    catch (Exception X)
                    {
                        Logger.writeLog(X);
                        throw new Exception("(error)Cannot connect to server");
                    }
                }
                else
                {
                    return PackageController.GetCurrentAmount(membershipNo, CurrentDate, out Output, out Status);
                }
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                Status = X.Message;
                Output = null;
                return false;
            }
        }

        public static bool GetCurrentAmountLocal(string membershipNo, DateTime CurrentDate, out DataTable Output, out string Status)
        {
            try
            {
                #region *) Validation: Is feature available?
                if (!isAvailable) throw new Exception(ErrMsg_NotAvailable); /// Process is success but return null
                #endregion

                #region *) Initialize: Fill default output parameter
                Output = null;
                Status = "";
                #endregion

                return PackageController.GetCurrentAmount(membershipNo, CurrentDate, out Output, out Status);

            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                Status = X.Message;
                Output = null;
                return false;
            }
        }

        public static bool AllocateNewlyEarnedPointsToCurrentAmounts(DataTable CurrentAmounts, string MembershipNo, out DataTable Output, out string Status)
        {
            try
            {
                #region *) Initialize: Fill default output parameter
                Status = "";
                Output = CurrentAmounts.Copy();
                #endregion

                #region *) Validation if membership no = WALK-IN
                if (MembershipNo == "WALK-IN")
                    return true;
                #endregion

                // CurrentAmounts Schema:
                // RefNo : String -- This is the ItemName
		        // Points : Decimal
                // PointType : String
                // userfld1 : String

                #region *) New rules : We get NewPoints from PointTempLog
                DataTable NewPoints = new DataTable();
                PointTempLogCollection ptlColl = new PointTempLogCollection();
                ptlColl.Where(PointTempLog.Columns.MembershipNo, MembershipNo);
                NewPoints = ptlColl.Load().ToDataTable();
                #endregion

                foreach (DataColumn col in Output.Columns)
                {
                    col.ReadOnly = false;
                }

                decimal DiffPoint = 0;

                foreach (DataRow Rw in NewPoints.Rows)
                {
                    decimal OneDiffPoint = 0;
                    #region *) Initialize: Parse OneDiffPoint (Total Points changed for 1 transaction)
                    if (!decimal.TryParse(Rw["PointAllocated"].ToString(), out OneDiffPoint))
                        throw new Exception("(warning)Cannot parse Point Value for Package " + Rw["RefNo"].ToString());
                    #endregion

                    #region *) Core: If PointType == Dollar, append to DiffPoint
                    if (Rw["PointType"].ToString() == Item.PointMode.Dollar)
                        DiffPoint += OneDiffPoint;
                    #endregion

                    if (OneDiffPoint > 0)
                    {
                        #region *) Core: Upsert To CurrentAmounts datatable

                        bool isFound = false;
                        foreach (DataRow cur in Output.Rows)
                        {
                            if (cur["userfld1"].ToString() == Rw["RefNo"].ToString())
                            {
                                cur["Points"] = cur["Points"].ToString().GetDecimalValue() + OneDiffPoint;
                                isFound = true;
                                break;
                            }
                        }

                        if (!isFound)
                        {
                            string itemNo = Rw["RefNo"].ToString();
                            string refNo = Rw["RefNo"].ToString();
                            string pointType = Rw["PointType"].ToString();
                            if (pointType == "T" && refNo.Contains("|OPP|"))
                            {
                                itemNo = refNo.Substring(0, refNo.IndexOf("|OPP|"));
                            }

                            Item theItem = new Item(Item.Columns.ItemNo, itemNo);
                            Output.Rows.Add(theItem.ItemName, OneDiffPoint, pointType, refNo);
                        }

                        #endregion
                    }
                    else if (OneDiffPoint < 0)
                    {
                        decimal absDiffPoint = Math.Abs(OneDiffPoint);
                        bool isFound = false;

                        foreach (DataRow cur in Output.Rows)
                        {
                            string refNo = Rw["PointType"].ToString().IndexOf("OPP") > 0 ? Rw["PointType"].ToString().Remove(0, 1) : Rw["RefNo"].ToString();
                            if (cur["userfld1"].ToString() == refNo)
                            {
                                if (cur["Points"].ToString().GetDecimalValue() < absDiffPoint)
                                    throw new Exception("(warning)Insufficient points to be deducted");

                                cur["Points"] = cur["Points"].ToString().GetDecimalValue() - absDiffPoint;
                                isFound = true;
                                break;
                            }
                        }

                        if (!isFound)
                            throw new Exception("(warning)Insufficient points to be deducted");
                    }
                }

                return true;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                Status = X.Message;
                Output = CurrentAmounts;
                return false;
            }
        }

        #region *) OLD GetInitialPointsAndPointsEarned
//        public static bool GetInitialPointsAndPointsEarned(string membershipNo, string orderHdrID, out decimal pointInitial, out decimal pointEarned)
//        {
//            pointInitial = 0;
//            pointEarned = 0;

//            try
//            {
//                //                PointAllocationLog palEarned = new PointAllocationLog("OrderHdrID", orderHdrID);
//                //                if (!string.IsNullOrEmpty(palEarned.OrderHdrID) && palEarned.OrderHdrID == orderHdrID)
//                //                {
//                //                    // OrderHdrID found in PointAllocationLog
//                //                    pointEarned = palEarned.PointAllocated;

//                //                    string sql = @"
//                //                                    SELECT ISNULL(SUM(PointAllocated), 0)
//                //                                    FROM PointAllocationLog 
//                //                                    WHERE MembershipNo = '{0}' AND AllocationDate < '{1}' AND userfld1 = '{2}'
//                //                                 ";
//                //                    sql = string.Format(sql, membershipNo,
//                //                                             palEarned.AllocationDate.ToString("yyyy-MM-dd HH:mm:ss.fff"),
//                //                                             palEarned.PointItemNo);
//                //                    object tmpObj = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));
//                //                    if (tmpObj != null && tmpObj is decimal)
//                //                        pointInitial = (decimal)tmpObj;

//                //                }

//                OrderHdr oh = new OrderHdr(orderHdrID);
//                if (!string.IsNullOrEmpty(oh.OrderHdrID) && oh.OrderHdrID == orderHdrID)
//                {
//                    // OrderHdrID found
//                    string sql;
//                    object tmpObj;

//                    sql = @"
//                            SELECT ISNULL(SUM(PointAllocated), 0)
//                            FROM PointAllocationLog pal
//                                INNER JOIN Item itm ON itm.ItemNo = pal.userfld1
//                            WHERE itm.userfld10 = 'D' AND pal.MembershipNo = '{0}' AND pal.OrderHdrID = '{1}'
//                           ";
//                    sql = string.Format(sql, membershipNo, orderHdrID);
//                    tmpObj = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));
//                    if (tmpObj != null && tmpObj is decimal)
//                        pointEarned = (decimal)tmpObj;

//                    sql = @"
//                            SELECT ISNULL(SUM(PointAllocated), 0)
//                            FROM PointAllocationLog pal
//                                INNER JOIN Item itm ON itm.ItemNo = pal.userfld1
//                            WHERE itm.userfld10 = 'D' AND pal.MembershipNo = '{0}' AND pal.AllocationDate < '{1}' 
//                           ";
//                    sql = string.Format(sql, membershipNo, oh.OrderDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
//                    tmpObj = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));
//                    if (tmpObj != null && tmpObj is decimal)
//                        pointInitial = (decimal)tmpObj;
//                }
//                else
//                {
//                    // OrderHdrID NOT found in PointAllocationLog

//                    // TO DO : what if OrderHdrID not found?
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                Logger.writeLog(ex);
//                return false;
//            }
//        }
        #endregion

        public static bool GetInitialPointsAndPointsEarned(string membershipNo, POSController pos, out decimal pointInitial, out decimal pointEarned)
        {
            pointInitial = 0;
            pointEarned = 0;

            try
            {
                string sql;
                object tmpObj;

                // If OrderHdrID exists in PointAllocationLog that means the points for this transaction has already been downloaded.
                sql = @"
                            SELECT ISNULL(SUM(PointAllocated), 0)
                            FROM (
                                SELECT ISNULL(SUM(PointAllocated), 0) AS PointAllocated
                                FROM PointAllocationLog pal
                                    INNER JOIN Item itm ON itm.ItemNo = pal.userfld1
                                WHERE itm.userfld10 = 'D' AND pal.MembershipNo = '{0}' AND pal.OrderHdrID = '{1}'
                                UNION
                                SELECT ISNULL(SUM(PointAllocated), 0)
                                FROM PointTempLog
                                WHERE PointType = 'D' AND MembershipNo = '{0}' AND OrderHdrID = '{1}'
                            ) a
                       ";
                sql = string.Format(sql, membershipNo, pos.myOrderHdr.OrderHdrID);
                tmpObj = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));
                if (tmpObj != null && tmpObj is decimal)
                    pointEarned = (decimal)tmpObj;
                

                sql = @"
                            SELECT ISNULL(SUM(PointAllocated), 0)
                            FROM (
                                SELECT ISNULL(SUM(PointAllocated), 0) AS PointAllocated
                                FROM PointAllocationLog pal
                                    INNER JOIN Item itm ON itm.ItemNo = pal.userfld1
                                WHERE itm.userfld10 = 'D' AND pal.MembershipNo = '{0}' AND pal.AllocationDate < '{1}' 
                                UNION
                                SELECT ISNULL(SUM(PointAllocated), 0)
                                FROM PointTempLog
                                WHERE PointType = 'D' AND MembershipNo = '{0}' AND OrderHdrID < '{2}'
                            ) a
                           ";
                sql = string.Format(sql, membershipNo, pos.myOrderHdr.OrderDate.ToString("yyyy-MM-dd HH:mm:ss.fff"), pos.myOrderHdr.OrderHdrID);
                tmpObj = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));
                if (tmpObj != null && tmpObj is decimal)
                    pointInitial = (decimal)tmpObj;

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static decimal GetCurrentBalance(string membershipNo, DateTime CurrentDate, string ItemNo, out string Status)
        {
            try
            {
                #region *) Validation: Is feature available?
                if (!isAvailable) throw new Exception(ErrMsg_NotAvailable); /// Process is success but return null
                #endregion

                #region *) Initialize: Fill default output parameter
                //Output = null;
                Status = "";
                #endregion

                return PackageController.GetCurrentAmountPerItem(membershipNo, CurrentDate, ItemNo, out Status);

            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                Status = X.Message;
                //Output = null;
                return 0;
            }
        }

        public static bool GetCurrentAmounts_Points(string MembershipNo, DateTime CurrentDate, out DataTable CurrentAmounts, out string Status)
        {
            Status = "";
            CurrentAmounts = new DataTable();
            if (isRealTime)
            {
                if (!ws.GetCurrentPointsAmount(MembershipNo, CurrentDate, out CurrentAmounts))
                {
                    Status = "(warning)Cannot get point from server\nPlease try again";
                    throw new Exception("(warning)Cannot get point from server\nPlease try again");
                    return false;
                }
                return true;
            }
            else
            {
                if (!PackageController.GetCurrentAmount_Points(MembershipNo, CurrentDate, out CurrentAmounts, out Status))
                    return false;
                return true;
            }
        }

        public static bool UpdItemPointBreak(Decimal TimeGet, Decimal BreakDownPrice, string ItemNo, out decimal Output, out string Status)
        {
            Status = "";
            try
            {

                #region *) Validation: Is feature available?
                if (!isAvailable) throw new Exception(ErrMsg_NotAvailable);
                #endregion

                #region *) Initialize: Fill default output parameter
                Output = 0;
                Status = "";
                #endregion

                #region *) Core: Revert Point
                if (isRealTime)
                {
                    try
                    {
                        // return true;
                        return ws.UpdMembershipPackage(TimeGet, BreakDownPrice, ItemNo, out Output, out Status);
                    }
                    catch (Exception X)
                    {
                        Logger.writeLog(X);
                        throw new Exception("(error)Cannot connect to server");
                    }
                }
                else
                {
                    return PackageController.updateMemberShipPackage(TimeGet, BreakDownPrice, ItemNo, out Output, out Status);
                }
                #endregion
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                Status = X.Message;
                Output = 0;
                return false;
            }

        }


        #region *) WaitingConfirmation: Ready to be deleted
        public static bool Validate_BeforeMakePayment(DataLogic.POSController pos, out string Status)
        {
            try
            {
                #region *) Validation: Is feature available?
                if (!isAvailable)
                {
                    Status = "Feature is not available";
                    return true;
                }
                #endregion

                #region *) Initialize: Fill default output parameter
                Status = "";
                #endregion

                /// Error message is checked
                #region *) Validation: Cannot buy PointPackage Item if no Membership provided
                if (pos.containPackages && !pos.MembershipApplied())
                    throw new Exception("(warning)Please assign membership to continue");
                #endregion

                return true;
            }
            catch (Exception X)
            {
                Status = X.Message;

                return false;
            }
        }

        public static bool Validate_BeforeMakePayment_Obsolete(DataLogic.POSController pos)
        {
            try
            {
                if (!_isAvailable) return true;

                /// Error message is checked
                #region *) Validation: Cannot buy PointPackage Item if no Membership provided
                if (pos.containPackages && !pos.MembershipApplied())
                {
                    MessageBox.Show(
                        "Please assign membership to continue", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                #endregion

                return true;
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
        }

        public static bool hasGetPackageItem(DataLogic.POSController pos, bool isPartialPayment)
        {
            try
            {
                /// do not have Package Item if this feature is not available
                if (!isAvailable) return false;

                #region *) Validation: Has Package Item?
                if (isPartialPayment)
                {
                    bool HasPackageItem = false;

                    foreach (OrderDet oneDet in pos.myOrderDet)
                        if (!oneDet.IsVoided & oneDet.Item.PointGetMode == DataLogic.Item.PointMode.Times)
                            HasPackageItem = true;

                    if (!HasPackageItem)
                        return false;
                }
                else
                {
                    foreach (OrderDet oneDet in pos.myOrderDet)
                        if (!oneDet.IsVoided & oneDet.Item.PointGetMode != DataLogic.Item.PointMode.Times)
                            return false;
                }
                #endregion

                return true;
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
        }
        #endregion

        #region Ulu-Ulu codes
        public static bool isAbleToUsePackage(DataLogic.POSController pos, bool isPartialPayment)
        {
            try
            {
                /// Cannot use Package if this feature is not available
                if (!isAvailable) return false;

                #region *) Validation: Has Package Item?
                /// If GetMode = Times, is Able to Redeem by times by 1 Time
                if (!hasGetPackageItem(pos, isPartialPayment))
                {
                    if (isPartialPayment)
                        throw new Exception("(warning)No package item is found.\nPlease check the order or choose another payment mode.");
                    else
                        throw new Exception("(warning)Some item is not package item.\nPlease check the order.");
                }
                #endregion

                #region *) Validation: Is Membership Applied?
                if (!pos.MembershipApplied())
                    throw new Exception("(warning)Please assign membership to continue");
                #endregion

                //#region *) Validation: is remaining amount sufficient?
                //foreach (OrderDet oneDet in pos.myOrderDet)
                //    if (oneDet.Item.PointRedeemMode == DataLogic.Item.PointMode.Times)
                //    {
                //        decimal Available = GetCurrentAmount(pos.CurrentMember.MembershipNo, DateTime.Now, oneDet.Item.PointRedeemRefNo, isRealTime);
                //        decimal Needed = oneDet.Quantity * (oneDet.Item.PointRedeemAmount == 0 ? oneDet.Item.RetailPrice : oneDet.Item.PointRedeemAmount);

                //        if (Available < Needed)
                //            throw new Exception("(warning)Package [" + oneDet.Item.ItemName + "] only have " + Available.ToString("N2") + " point(s).\n Please top up before using this package.");
                //    }
                //#endregion

                return true;
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
        }

        public static bool hasUsePackageItem(DataLogic.POSController pos, bool isPartialPayment)
        {
            try
            {
                /// do not have Package Item if this feature is not available
                if (!isAvailable) return false;

                #region *) Validation: Has Package Item?
                if (isPartialPayment)
                {
                    bool HasPackageItem = false;

                    foreach (OrderDet oneDet in pos.myOrderDet)
                        if (!oneDet.IsVoided & oneDet.Item.PointRedeemMode == DataLogic.Item.PointMode.Times)
                            HasPackageItem = true;

                    if (!HasPackageItem)
                        return false;
                }
                else
                {
                    foreach (OrderDet oneDet in pos.myOrderDet)
                        if (!oneDet.IsVoided & oneDet.Item.PointRedeemMode != DataLogic.Item.PointMode.Times)
                            return false;
                }
                #endregion

                return true;
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
        }

        //public static decimal GetCurrentAmount(string membershipNo, DateTime CurrentDate, string PackageRefNo, bool GetFromServer)
        //{
        //    try
        //    {
        //        #region -= ONLINE MODE =-
        //        if (GetFromServer && isRealTime)
        //        {
        //            SyncClientController.Load_WS_URL();
        //            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
        //            ws.Timeout = 100000;
        //            ws.Url = SyncClientController.WS_URL;

        //            decimal Result = 0;
        //            Result = ws.GetPackageAmount(membershipNo, CurrentDate, PackageRefNo);

        //            return Result;
        //        }
        //        #endregion
        //        #region -= OFFLINE/SERVER MODE =-
        //        else
        //        {
        //            string QueryStr =
        //                      "SELECT ISNULL(SUM(Points),0) " +
        //                      "FROM MembershipPoints " +
        //                      "WHERE MembershipNo = @MembershipNo " +
        //                          "AND StartValidPeriod <= @CurrentDate AND EndValidPeriod >= @CurrentDate " +
        //                          "AND MembershipPoints.userfld1 = @PackageRefNo";

        //            QueryCommand cmd = new QueryCommand(QueryStr);
        //            cmd.AddParameter("@MembershipNo", membershipNo, System.Data.DbType.String);
        //            cmd.AddParameter("@CurrentDate", CurrentDate, System.Data.DbType.DateTime);
        //            cmd.AddParameter("@PackageRefNo", PackageRefNo, System.Data.DbType.String);

        //            string sPoints = DataService.ExecuteScalar(cmd).ToString();

        //            if (string.IsNullOrEmpty(sPoints))
        //                throw new Exception("Loading package failed;\nInput M'ship No = " + membershipNo + ";\nPackage Ref No=" + PackageRefNo + ";\nDate = " + CurrentDate.ToString());

        //            decimal points = 0;

        //            if (!decimal.TryParse(sPoints, out points))
        //                throw new Exception("Point convertion from string to decimal is failed, Point is " + sPoints);

        //            return points;
        //        }
        //        #endregion
        //    }
        //    catch (Exception X)
        //    {
        //        if (GetFromServer | !isRealTime)
        //        {
        //            if (X.Message.StartsWith("(warning)"))
        //            {
        //                MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            }
        //            else if (X.Message.StartsWith("(error)"))
        //            {
        //                MessageBox.Show(X.Message.Replace("(error)", ""), "Error",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //            else
        //            {
        //                Logger.writeLog(X);
        //                MessageBox.Show(
        //                    "Some error occurred. Please contact your administrator.", "Error"
        //                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //        else
        //        {
        //            Logger.writeLog(X);
        //        }

        //        return 0;
        //    }
        //}
        public static bool isAbleToGetPackage()
        {
            return true;
        }






        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////// DOLLAR

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////// Times


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////// General

        public static bool GetHistory_Last50(string MembershipNo, string PackageName
            , out DataTable Output, out DateTime StartValidPeriod, out DateTime EndValidPeriod
            , out decimal RemainingPoint, out string Status)
        {
            Status = "";

            StartValidPeriod = new DateTime(2000, 1, 1);
            EndValidPeriod = new DateTime(2100, 1, 1);
            RemainingPoint = 0;

            try
            {
                if (isRealTime)
                {
                    try
                    {
                        Output = ws.GetHistory_Point(MembershipNo, PackageName, out StartValidPeriod, out EndValidPeriod, out RemainingPoint);
                    }
                    catch (Exception X)
                    {
                        Logger.writeLog(X);
                        throw new Exception("(error)Cannot connect to server");
                    }
                }
                else
                {
                    Output = MembershipController.GetHistory_Point_WebSite(MembershipNo, PackageName, out StartValidPeriod, out EndValidPeriod, out RemainingPoint);
                }

                return true;
            }
            catch (Exception X)
            {
                Output = null;
                Status = X.Message;
                return false;
            }
        }

        public static bool GetHistory_Last50Local(string MembershipNo, string PackageName
            , out DataTable Output, out DateTime StartValidPeriod, out DateTime EndValidPeriod
            , out decimal RemainingPoint, out string Status)
        {
            Status = "";

            StartValidPeriod = new DateTime(2000, 1, 1);
            EndValidPeriod = new DateTime(2100, 1, 1);
            RemainingPoint = 0;

            try
            {

                Output = MembershipController.GetHistory_Point_WebSite(MembershipNo, PackageName, out StartValidPeriod, out EndValidPeriod, out RemainingPoint);
                return true;
            }
            catch (Exception X)
            {
                Output = null;
                Status = X.Message;
                return false;
            }
        }

        public decimal GetCurrentPoint(string membershipNo, DateTime CurrentDate, out string status)
        {
            return MembershipController.GetCurrentPoint(membershipNo, CurrentDate, out status);
        }
        public static bool getAvailablePoints(string membershipNo, DateTime CurrentDate
            , out decimal Output, out string Status)
        {
            Output = 0;
            Status = "Not Available";
            if (!isAvailable) return false;

            Status = "";

            try
            {
                if (isRealTime)
                {
                    try
                    {
                        Output = ws.GetCurrentPoint(membershipNo, CurrentDate, out Status);
                    }
                    catch (Exception X)
                    {
                        Logger.writeLog(X);
                        throw new Exception("(error)Cannot connect to server");
                    }
                }
                else
                {
                    Output = MembershipController.GetCurrentPoint(membershipNo, CurrentDate, out Status);

                    if (!string.IsNullOrEmpty(Status))
                        throw new Exception(Status);
                }

                return true;
            }
            catch (Exception X)
            {
                Status = X.Message;

                return false;
            }
        }
        #endregion

        #region Ulu-Ulu codes - Points
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// Item Types:                                                                         ///
        ///                                                                                     ///
        ///                 || GetPoint  | RedeemPoint || PayByPoint | PayByTimes | PayByDollar ///
        /// =================================================================================== ///
        /// PointPackage    || Dollar    | [Nothing]   || False      | False      | True        ///
        /// CoursePackage   || Times     | [Nothing]   || False      | False      | True        ///
        /// Service         || [Nothing] | Dollar      || True       | False      | True        ///
        /// Product         || [Nothing] | [Nothing]   || False      | False      | True        ///
        ///////////////////////////////////////////////////////////////////////////////////////////

        public enum ItemTypes { PointPackage, CoursePackage, Service, CourseUsage, Unknown };

        public static ItemTypes ItemType(DataLogic.Item ItemInQuestion)
        {
            try
            {
                if (ItemInQuestion == null) return ItemTypes.Unknown;
                if (ItemInQuestion.IsNew || !ItemInQuestion.IsLoaded) return ItemTypes.Unknown;
                if (ItemInQuestion.Deleted.GetValueOrDefault(false)) return ItemTypes.Unknown;

                if (ItemInQuestion.PointGetMode == DataLogic.Item.PointMode.Dollar && (ItemInQuestion.PointRedeemMode != DataLogic.Item.PointMode.Dollar && ItemInQuestion.PointRedeemMode != DataLogic.Item.PointMode.Times))
                    return ItemTypes.PointPackage;
                if (ItemInQuestion.PointGetMode == DataLogic.Item.PointMode.Times && ItemInQuestion.PointRedeemMode == DataLogic.Item.PointMode.Dollar)
                    return ItemTypes.CoursePackage;
                if ((ItemInQuestion.PointGetMode != DataLogic.Item.PointMode.Dollar && ItemInQuestion.PointGetMode != DataLogic.Item.PointMode.Times) && ItemInQuestion.PointRedeemMode == DataLogic.Item.PointMode.Dollar)
                    return ItemTypes.Service;
                if ((ItemInQuestion.PointGetMode != DataLogic.Item.PointMode.Dollar && ItemInQuestion.PointGetMode != DataLogic.Item.PointMode.Times) && ItemInQuestion.PointRedeemMode == DataLogic.Item.PointMode.Times)
                    return ItemTypes.CourseUsage;
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return ItemTypes.Unknown;
        }

        public static bool Validate_canPayByPoint_Wholly(DataLogic.POSController pos)
        {
            try
            {
                if (!_isAvailable) return false;

                if (!pos.MembershipApplied())
                    throw new Exception("(warning)Please provide membership id to use point");

                if (pos.hasItemThatCannotBeRedeemedByPoints)
                    throw new Exception("(warning)Some item cannot be paid by points");

                return true;
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
        }

        public bool hasPointUnredeemableOrder(DataLogic.OrderDetCollection myOrderDet)
        {
            try
            {
                if (myOrderDet.Count < 1) return true;

                foreach (OrderDet oneDet in myOrderDet)
                {
                    if (!oneDet.IsVoided && !oneDet.IsPromoFreeOfCharge)
                    {
                        if (oneDet.Item.PointRedeemMode != Item.PointMode.Dollar)
                            return true;
                    }
                }

                return false;
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}