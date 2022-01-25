using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using PowerPOS;
using System.Data;
using SubSonic;
using System.Collections.Generic;
using PowerWeb.Synchronization.Classes;
using System.Linq;
using PowerWeb;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing;

/// <summary>
/// Summary description for Synchronization
/// </summary>
[WebService(Namespace = "http://www.edgeworks.com.sg/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Synchronization : System.Web.Services.WebService
{

    public Synchronization()
    {
        //InitializeComponent(); 
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

    }

    #region [INCOMING]

    [WebMethod]
    public bool UpdateMemberDataWeb(DataTable data, out string status)
    {
        return MembershipController.UpdateMemberDataWeb(data, out status);
    }

    [WebMethod]
    public bool UpdateMemberTagNoWeb(string MembershipNo, string TagNo, out string status)
    {
        return MembershipController.UpdateMemberTagNoWeb(MembershipNo, TagNo, out status);
    }

    [WebMethod(CacheDuration = 180)]
    public bool FetchOrdersCCMW(
        string[][] dsHeaders, string[][] dsDetails,
        string[][] dsReceiptHdr, string[][] dsReceiptDet)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchOrdersDataCCMW(dsHeaders, dsDetails, dsReceiptHdr, dsReceiptDet);

    }


    [WebMethod(CacheDuration = 180)]
    public bool FetchOrders(DataSet dsHeaders, DataSet dsDetails, DataSet dsReceiptHdr, DataSet dsReceiptDet, DataSet dsComm)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchOrdersData(dsHeaders, dsDetails, dsReceiptHdr, dsReceiptDet, dsComm);
    }

    [WebMethod(CacheDuration = 180)]
    public bool FetchNewMembershipSignUps(DataSet dsMember)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        Logger.writeLog("IP Address - Try to sync new signups:" + remote_caller);
        return SynchronizationController.FetchNewMembershipSignUpsData(dsMember);
    }
    [WebMethod(CacheDuration = 180)]
    public bool FetchInventorys(string[][] dsHeaders, string[][] dsDetails, string[][] remainingQty)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        Logger.writeLog("IP Address - Try to sync Inventorys:" + remote_caller);
        return SynchronizationController.FetchInventorysData(dsHeaders, dsDetails, remainingQty);
    }
    [WebMethod(CacheDuration = 180)]
    public bool FetchLogTable(DataSet dsLogTable, string logTableName)
    {
        return SynchronizationController.FetchLogTable(dsLogTable, logTableName);
    }
    [WebMethod(CacheDuration = 180)]
    public bool FetchLogTableWithUpdate(DataSet dsLogTable, string logTableName)
    {
        return SynchronizationController.FetchLogTableWithUpdateOption(dsLogTable, logTableName, true);
    }
    [WebMethod(CacheDuration = 180)]
    public bool FetchDeliveryOrder(DataSet dsData)
    {
        return SynchronizationController.FetchDeliveryOrderAndDetails(dsData);
    }
    /// <summary>
    /// Web method to save member remarks on web database. (created by John Harries)
    /// </summary>
    /// <param name="dsMemberTable">Modified membership remarks dataset.</param>
    /// <returns>True for success process and false for unsuccess process.</returns>
    [WebMethod(CacheDuration = 180)]
    public bool FetchMemberRemarks(DataSet dsMemberTable)
    {
        return SynchronizationController.FetchModifiedMembershipRemarks(dsMemberTable);
    }

    /// <summary>
    /// Web method to update OrderHdr.Remark on server database.
    /// </summary>
    [WebMethod]
    public bool UpdateOrderHdrRemark(string OrderHdrID, string Remark)
    {
        return SynchronizationController.UpdateOrderHdrRemark(OrderHdrID, Remark);
    }

    #endregion

    #region [FreezePOS]
    [WebMethod]
    public bool FreezePOSByPointOfSaleID(int PointOfSaleID)
    {
        return SynchronizationController.FreezePOSByPointOfSaleID(PointOfSaleID);
    }

    #endregion

    #region [OUTGOING]
    [WebMethod(CacheDuration = 0)]
    public DataSet GetDataTable(string tableName, bool syncAll)
    {
        return SynchronizationController.GetDataTable(tableName, syncAll);
    }

    [WebMethod(CacheDuration = 0)]
    public byte[] GetDataTableCompressed(string tableName, bool syncAll)
    {
        return SynchronizationController.GetDataTableCompressed(tableName, syncAll);
    }

    [WebMethod(CacheDuration = 0)]
    public string[][] GetOrderHdrList(DateTime startDate, DateTime endDate, int PointOfSaleID)
    {
        return SynchronizationController.GetOrderHdrList(startDate, endDate, PointOfSaleID);
    }

    [WebMethod(CacheDuration = 0)]
    public string[][] GetOrderHdrListWithoutPOSID(DateTime startDate, DateTime endDate)
    {
        return SynchronizationController.GetOrderHdrListWithoutPOSID(startDate, endDate);
    }

    [WebMethod(CacheDuration = 0)]
    public string[][] GetInventoryHdrList(DateTime startDate, DateTime endDate)
    {
        return SynchronizationController.GetOrderHdrListWithoutPOSID(startDate, endDate);
    }

    [WebMethod(CacheDuration = 0)]
    public DataSet FetchSales(DateTime startDate, DateTime endDate)
    {
        return SynchronizationController.FetchSales(startDate, endDate);
    }

    [WebMethod(CacheDuration = 0)]
    public byte[] GetDataTableCustomCompressed(string sqlQuery)
    {
        return SynchronizationController.GetDataTableCustomCompressed(sqlQuery);
    }

    [WebMethod(CacheDuration = 0)]
    public DataSet GetDeliveryOrder(DateTime startDate, DateTime endDate, int PointOfSaleID)
    {
        return SynchronizationController.GetDeliveryOrder(startDate, endDate, PointOfSaleID);
    }

    [WebMethod(CacheDuration = 0)]
    public byte[] GetPastTransaction(string membershipNo, int rowTotal)
    {
        Membership m = new Membership(membershipNo);
        if (m != null && m.MembershipNo != "")
        {
            DataTable dt = m.GetPastTransaction(rowTotal);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return SyncClientController.CompressDataSetToByteArray(ds);
        }

        return null;
    }

    [WebMethod(CacheDuration = 0)]
    public byte[] GetPastTransactionWithOutlet(string membershipNo, int rowTotal, string OutletName)
    {
        Membership m = new Membership(membershipNo);
        if (m != null && m.MembershipNo != "")
        {
            DataTable dt = m.GetPastTransaction(rowTotal, OutletName);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return SyncClientController.CompressDataSetToByteArray(ds);
        }

        return null;
    }
    #endregion

    #region [POINT & PACKAGE]
    [System.Web.Services.WebMethod]
    public string[] getPackageList(string MembershipNo)
    {
        return MembershipController.getRemainingPackageList(MembershipNo);
    }

    [System.Web.Services.WebMethod]
    public DataTable GetHistory_Point(string MembershipNo, string PackageName
        , out DateTime StartValidPeriod, out DateTime EndValidPeriod, out decimal RemainingPoint)
    {
        return MembershipController.GetHistory_Point_WebSite(MembershipNo, PackageName, out StartValidPeriod, out EndValidPeriod, out RemainingPoint);
    }

    [System.Web.Services.WebMethod]
    public bool AllocatePendingPackage()
    {
        string status = "";
        return PowerPOS.Feature.Package.AllocatePendingPackageServer(out status);
    }

    [System.Web.Services.WebMethod]
    public bool AllocatePendingPackageNEW()
    {
        string status = "";
        return SynchronizationController.AllocatePendingPackage(out status);
    }

    #endregion

    #region [Membership Point System]
    [System.Web.Services.WebMethod]
    public bool AddPoint(string membershipno, string OrderHdrID, DateTime StartValidPeriod, int ValidPeriod,
        decimal points, string UserName, out string status)
    {
        return MembershipController.AddPoints_Final(membershipno, OrderHdrID, StartValidPeriod, ValidPeriod, points, UserName, out status);
    }

    [System.Web.Services.WebMethod]
    public bool DeductPoints(string membershipno, string OrderHdrID, DateTime DeductionDate, decimal points, string packageRefNo,
        string UserName, out string status)
    {
        return MembershipController.DeductPoints_Final(membershipno, OrderHdrID, DeductionDate, points, packageRefNo, UserName, out status);
    }

    [System.Web.Services.WebMethod]
    public decimal GetCurrentPoint(string membershipNo, DateTime CurrentDate, out string status)
    {
        return MembershipController.GetCurrentPoint(membershipNo, CurrentDate, out status);
    }

    [System.Web.Services.WebMethod]
    public decimal GetAllocatedPoint(string OrderHdrID)
    {
        return MembershipController.GetAllocatedPoint(OrderHdrID);
    }

    [System.Web.Services.WebMethod]
    public bool GetCurrentPointsAmount(string membershipNo, DateTime CurrentDate, out DataTable Output)
    {
        string Status = "";
        if (!PackageController.GetCurrentAmount_Points(membershipNo, CurrentDate, out Output, out Status))
        { Logger.writeLog(Status); return false; }
        else
        { return true; }
    }
    #endregion

    #region [Membership Package System]
    [System.Web.Services.WebMethod]
    public bool GetPackageBreakdown(string membershipNo, DateTime CurrentDate, string PackageRefNo, out decimal Breakdown, out string Status)
    {
        return PackageController.GetCurrentBreakdown(membershipNo, CurrentDate, PackageRefNo, out Breakdown, out Status);
    }
    [System.Web.Services.WebMethod]
    public bool GetPackageAmount_plusBreakdown(string membershipNo, DateTime CurrentDate, string PackageRefNo, out decimal Amount, out decimal Breakdown, out string Status)
    {
        bool ActionResult;
        ActionResult = PackageController.GetCurrentBreakdown(membershipNo, CurrentDate, PackageRefNo, out Breakdown, out Status);
        ActionResult &= PackageController.GetCurrentAmount(membershipNo, CurrentDate, PackageRefNo, out Amount, out Status);
        return ActionResult;
    }
    [System.Web.Services.WebMethod]
    public bool GetPackageAmount(string membershipNo, DateTime CurrentDate, string PackageRefNo, out decimal Output, out string Status)
    {
        return PackageController.GetCurrentAmount(membershipNo, CurrentDate, PackageRefNo, out Output, out Status);
    }
    [System.Web.Services.WebMethod]
    public bool GetPackageAmounts(string membershipNo, DateTime CurrentDate, out DataTable Output, out string Status)
    {
        return PackageController.GetCurrentAmount(membershipNo, CurrentDate, out Output, out Status);
    }
    [System.Web.Services.WebMethod]
    public bool UpdateAll(DataTable PointData, string OrderHdrID, DateTime TransactionDate
       , int ValidPeriods, string MembershipNo, string SalesPersonID, string UserName
        , out decimal InitialPoint, out decimal DiffPoint, out string Status)
    {
        return PackageController.UpdateAll(PointData, OrderHdrID, TransactionDate
            , ValidPeriods, MembershipNo, SalesPersonID, UserName, out InitialPoint, out DiffPoint, out  Status);
    }

    [System.Web.Services.WebMethod]
    public bool RevertPoints(string OrderHdrID, DateTime TransactionDate, string MembershipNo, string SalesPersonID, string UserName, out string Status)
    {
        return PackageController.RevertPoints(OrderHdrID, TransactionDate, MembershipNo, SalesPersonID, UserName, out  Status);
    }
    [System.Web.Services.WebMethod]
    public bool UpdMembershipPackage(Decimal GetPoint, Decimal BreakDownPrice, string ItemNo, out decimal Breakdown, out string Status)
    {
        return PackageController.updateMemberShipPackage(GetPoint, BreakDownPrice, ItemNo, out Breakdown, out Status);
    }
    #endregion

    #region [Membership Installment]
    [WebMethod]
    public DataSet GetInstallmentListByCustomer(string MembershipNo, bool ShowOutstandingOnly, out string status)
    {
        return InstallmentController.GetInstallmentListByCustomer(MembershipNo, ShowOutstandingOnly, out status);
    }

    [WebMethod]
    public DataSet GetInstallmentDetailByOrderHdrID(string MembershipNo, string OrderHdrID, bool ShowAllTransactions, out string status)
    {
        return InstallmentController.GetInstallmentDetailByOrderHdrID(MembershipNo, OrderHdrID, ShowAllTransactions, out status);
    }
    #endregion

    #region [Inventory]
	[WebMethod]
    public bool CheckSerialNoIsExist(List<ItemTagModel> input, out string message)
    {
        return ItemTagController.CheckSerialNoIsExistHelper(input, out message);
    }

    [WebMethod]
    public bool CheckSerialNoIsNotExist(List<ItemTagModel> input, out string message)
    {
        return ItemTagController.CheckSerialNoIsNotExistHelper(input, out message);
    }
	
    [WebMethod]
    public List<InventoryItem> GetStockQuantity(Int32 InventoryLocation)
    {
        return SynchronizationController.GetStockQuantity(InventoryLocation);
    }

    [WebMethod]
    public string GetStockQuantityByItem(Int32 InventoryLocation, string ItemNo)
    {
        return SynchronizationController.GetStockQuantityByItem(InventoryLocation, ItemNo);
    }

    [WebMethod]
    public bool TransferOutAutoReceiveCompressed
            (byte[] data,
             string username, int FromInventoryLocationID,
            int ToInventoryLocationID, out string newRefNo, out string status)
    {
        try
        {

            PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (strCostingMethod.ToLower() == "fifo")
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            else if (strCostingMethod.ToLower() == "fixed avg")
                CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
            else
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

            InventoryController ctrl = new InventoryController(CostingMethod);

            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray
                (data);
            if (myDataSet == null || myDataSet.Tables.Count < 2)
            {
                newRefNo = "";
                status = "Unable to decompress parameter from server";
                Logger.writeLog(status);
                return false;
            }
            if (ctrl.LoadFromDataTable(myDataSet.Tables[0], myDataSet.Tables[1]))
            {
                if (ctrl.TransferOutAutoReceive(username, FromInventoryLocationID, ToInventoryLocationID, out status))
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
            else
            {
                status = "Unable to load data table";
                newRefNo = "";
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            newRefNo = "";
            return false;
        }
    }

    [WebMethod]
    public bool TransferOutAutoReceive
            (DataTable hdr, DataTable det,
             string username, int FromInventoryLocationID,
            int ToInventoryLocationID, out string newRefNo, out string status)
    {
        try
        {
            PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (strCostingMethod.ToLower() == "fifo")
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            else if (strCostingMethod.ToLower() == "fixed avg")
                CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
            else
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

            InventoryController ctrl = new InventoryController(CostingMethod);

            if (ctrl.LoadFromDataTable(hdr, det))
            {
                if (ctrl.TransferOutAutoReceive(username, FromInventoryLocationID, ToInventoryLocationID, out status))
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
            else
            {
                status = "Unable to load data table";
                newRefNo = "";
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            newRefNo = "";
            return false;
        }
    }

    [WebMethod]
    public bool StockOutCompressed(byte[] data, string username, int StockOutReasonID,
        int InventoryLocationID, bool IsAdjustment, bool deductRemainingQty, out string newRefNo, out string status)
    {
        try
        {
            PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (strCostingMethod.ToLower() == "fifo")
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            else if (strCostingMethod.ToLower() == "fixed avg")
                CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
            else
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

            InventoryController ctrl = new InventoryController(CostingMethod);

            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
            if (myDataSet == null || myDataSet.Tables.Count < 2)
            {
                newRefNo = "";
                status = "Unable to decompress parameter from server";
                Logger.writeLog(status);
                return false;
            }
            if (ctrl.LoadFromDataTable(myDataSet.Tables[0], myDataSet.Tables[1]))
            {
                if (ctrl.StockOut(username, StockOutReasonID, InventoryLocationID, IsAdjustment, deductRemainingQty, out status))
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
            else
            {
                status = "Unable to load data table";
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

    [WebMethod]
    public bool StockOut(DataTable hdr, DataTable det, string username, int StockOutReasonID,
        int InventoryLocationID, bool IsAdjustment, bool deductRemainingQty, out string newRefNo, out string status)
    {
        try
        {
            PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (strCostingMethod.ToLower() == "fifo")
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            else if (strCostingMethod.ToLower() == "fixed avg")
                CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
            else
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

            InventoryController ctrl = new InventoryController(CostingMethod);
            if (ctrl.LoadFromDataTable(hdr, det))
            {
                if (ctrl.StockOut(username, StockOutReasonID, InventoryLocationID, IsAdjustment, deductRemainingQty, out status))
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
            else
            {
                status = "Unable to load data table";
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

    [WebMethod]
    public bool StockOutItem(string itemno, int quantity, string username, int StockOutReasonID,
        int InventoryLocationID, bool IsAdjustment, bool deductRemainingQty, out string newRefNo, out string status)
    {
        try
        {
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
            if (ctrl.StockOut(username, StockOutReasonID, InventoryLocationID, IsAdjustment, deductRemainingQty, out status))
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

    [WebMethod]
    public bool StockInCompressed(//DataTable hdr, DataTable det,
            byte[] data,
            string username, int InventoryLocationID,
            bool IsAdjustment, bool CalculateCOGS, out string newRefNo, out string customRefNo,
            out string status)
    {
        try
        {
            PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (strCostingMethod.ToLower() == "fifo")
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            else if (strCostingMethod.ToLower() == "fixed avg")
                CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
            else
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

            InventoryController ctrl = new InventoryController(CostingMethod);
            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
            if (myDataSet == null || myDataSet.Tables.Count < 2)
            {
                newRefNo = "";
                customRefNo = "";
                status = "Unable to decompress parameter from server";
                Logger.writeLog(status);
                return false;
            }
            if (ctrl.LoadFromDataTable(myDataSet.Tables[0], myDataSet.Tables[1]))
            {
                if (ctrl.StockIn(username, InventoryLocationID, IsAdjustment,
                    CalculateCOGS, out status))
                {
                    newRefNo = ctrl.GetInvHdrRefNo();
                    customRefNo = ctrl.GetCustomRefNo();

                    /* Moved inside ctrl.StockIn()
                    // check if there are any stock take after the stock in 
                    string itemlist = "";
                    if (ctrl.gotStockTakeAfter(out itemlist))
                    {
                        ctrl.StockOutAdjustment(username, 1, InventoryLocationID, true, true, itemlist, out status);
                    }
                    */

                    return true;
                }
                else
                {
                    newRefNo = "";
                    customRefNo = "";
                    return false;
                }
            }
            else
            {
                newRefNo = "";
                customRefNo = "";
                status = "Unable to load data table";
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            newRefNo = "";
            customRefNo = "";
            return false;
        }
    }

    [WebMethod]
    public bool StockIn(DataTable hdr, DataTable det,
            string username, int InventoryLocationID,
            bool IsAdjustment, bool CalculateCOGS, out string newRefNo,
            out string status) //For backward compatibility
    {
        try
        {
            PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (strCostingMethod.ToLower() == "fifo")
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            else if (strCostingMethod.ToLower() == "fixed avg")
                CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
            else
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

            InventoryController ctrl = new InventoryController(CostingMethod);

            if (ctrl.LoadFromDataTable(hdr, det))
            {
                if (ctrl.StockIn(username, InventoryLocationID, IsAdjustment,
                    CalculateCOGS, out status))
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
            else
            {
                newRefNo = "";
                status = "Unable to load data table";
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            newRefNo = "";
            return false;
        }
    }

    [WebMethod]
    public bool StockReturnCompressed(//DataTable hdr, DataTable det,
            byte[] data,
            string username, int StockOutReasonID, int InventoryLocationID,
            bool IsAdjustment, bool CalculateCOGS, out string newRefNo, 
            out string status)
    {
        try
        {
            PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (strCostingMethod.ToLower() == "fifo")
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            else if (strCostingMethod.ToLower() == "fixed avg")
                CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
            else
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

            InventoryController ctrl = new InventoryController(CostingMethod);
            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
            if (myDataSet == null || myDataSet.Tables.Count < 2)
            {
                newRefNo = "";
                
                status = "Unable to decompress parameter from server";
                Logger.writeLog(status);
                return false;
            }
            if (ctrl.LoadFromDataTable(myDataSet.Tables[0], myDataSet.Tables[1]))
            {
                if (ctrl.StockReturn(username, StockOutReasonID, InventoryLocationID, IsAdjustment,
                    CalculateCOGS, out status))
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
            else
            {
                newRefNo = "";
                status = "Unable to load data table";
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            newRefNo = "";
            return false;
        }
    }

    [WebMethod]
    public bool StockReturn(DataTable hdr, DataTable det,
            string username, int StockOutReasonID, int InventoryLocationID,
            bool IsAdjustment, bool CalculateCOGS, out string newRefNo,
            out string status) //For backward compatibility
    {
        try
        {
            PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (strCostingMethod.ToLower() == "fifo")
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
            else if (strCostingMethod.ToLower() == "fixed avg")
                CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
            else
                CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

            InventoryController ctrl = new InventoryController(CostingMethod);

            if (ctrl.LoadFromDataTable(hdr, det))
            {
                if (ctrl.StockReturn(username, StockOutReasonID, InventoryLocationID, IsAdjustment,
                    CalculateCOGS, out status))
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
            else
            {
                newRefNo = "";
                status = "Unable to load data table";
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            newRefNo = "";
            return false;
        }
    }

    [WebMethod]
    public bool CreateStockTakeEntries(DataTable hdr, DataTable det, string username,
            string takenBy, string verifiedBy, out string status)
    {
        try
        {
            InventoryController ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            if (ctrl.LoadFromDataTable(hdr, det))
            {
                return ctrl.CreateStockTakeEntries(username, takenBy, verifiedBy, out status);
            }
            else
            {
                status = "Unable to load data table";
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            return false;
        }
    }

    [WebMethod]
    public bool CreateStockTakeEntriesCompressed
        (byte[] data, string username,
            string takenBy, string verifiedBy, out string status)
    {
        try
        {
            InventoryController ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
            if (myDataSet == null || myDataSet.Tables.Count < 2)
            {
                status = "Unable to decompress parameter from server";
                Logger.writeLog(status);
                return false;
            }
            if (ctrl.LoadFromDataTable(myDataSet.Tables[0], myDataSet.Tables[1]))
            {
                return ctrl.CreateStockTakeEntries(username, takenBy, verifiedBy, out status);
            }
            else
            {
                status = "Unable to load data table";
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            return false;
        }
    }

    [WebMethod]
    public bool AdjustStockTakeDiscrepancy(string username, object[] markedList, int locationID)
    {
        try
        {
            Query qr;

            //for (int i = 0; i < markedList.Count; i++)
            {
                qr = StockTake.CreateQuery();
                qr.QueryType = QueryType.Update;
                qr.AddWhere(StockTake.Columns.StockTakeID, Comparison.In, markedList);
                qr.AddUpdateSetting(StockTake.Columns.Marked, true);
                qr.Execute();
            }
            StockTakeController ct = new StockTakeController();
            return ct.CorrectStockTakeDiscrepancy
                        (username, locationID);
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            return false;
        }
    }

    [WebMethod]
    public bool AdjustStockTakeDiscrepancyNew(string username, string markedList, int locationID, out string status)
    {
        status = "";
        try
        {
            Query qr;

            Logger.writeLog("Start Adjust Stock");

            List<string> MarkedList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(markedList);
            int repeat = 1000;

            for (int i = 0; i < MarkedList.Count; i = i + repeat)
            {
                var items = MarkedList.Skip(i).Take(repeat); 

                qr = StockTake.CreateQuery();
                qr.QueryType = QueryType.Update;
                qr.AddWhere(StockTake.Columns.StockTakeID, Comparison.In, items.ToArray());
                qr.AddUpdateSetting(StockTake.Columns.Marked, true);
                qr.Execute();
            }
            StockTakeController ct = new StockTakeController();
            return ct.CorrectStockTakeDiscrepancyWithStatusMessage(username, locationID, out status);
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            return false;
        }
    }

    [WebMethod]
    public bool CorrectStockTakeDiscrepancy(string username, int locationID, out string status)
    {
        StockTakeController ct = new StockTakeController();
        return ct.CorrectStockTakeDiscrepancyWithStatusMessage(username, locationID, out status);
    }

    [WebMethod]
    public bool GetTotalOfAdjustedItem(string markedList, out int countResult)
    {
        countResult = 0;

        try 
        {
            Query qr;
            
            List<string> MarkedList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(markedList);
            int repeat = 1000;

            for (int i = 0; i < MarkedList.Count; i = i + repeat)
            {
                var items = MarkedList.Skip(i).Take(repeat);

                StockTakeCollection st = new StockTakeCollection();

                qr = StockTake.CreateQuery();
                qr.QueryType = QueryType.Select;
                qr.AddWhere(StockTake.Columns.StockTakeID, Comparison.In, items.ToArray());
                qr.AddUpdateSetting(StockTake.Columns.Marked, true);
                qr.AddUpdateSetting(StockTake.Columns.IsAdjusted, true);

                st.LoadAndCloseReader(qr.ExecuteReader());

                countResult += st.Count();
            }

            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("Error when check adjusted Item :" + ex);
            return false;
        }
    }



    [WebMethod]
    public bool IsThereUnAdjustedStockTake(int locationID)
    {
        try
        {
            return StockTakeController.IsThereUnAdjustedStockTake(locationID);
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            return false;
        }
    }

    [WebMethod]
    public string GetUnAdjustedStockTakeDate(int locationID)
    {
        try
        {
            return StockTakeController.GetUnAdjustedStockTakeDate(locationID);
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            return "";
        }
    }

    [WebMethod]
    public void DeleteStockTake(int StockTakeID)
    {
        //StockTake.Delete(StockTakeID);
        StockTake.DeleteLogically(StockTakeID);
    }

    [WebMethod]
    public void AssignStockOutToConfirmedOrderUsingTransactionScope()
    {
        InventoryController.AssignStockOutToConfirmedOrderUsingTransaction(false);
    }

    [WebMethod]
    public byte[] FetchStockBalanceReport(string searchQuery, int inventoryLocationID)
    {
        DataTable dt = ItemSummaryController.FetchStockBalance(searchQuery, inventoryLocationID);
        return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    [WebMethod]
    public byte[] FetchStockBalanceReportWithZeroQty(string searchQuery, int inventoryLocationID, bool isRemoveZeroQty)
    {
        DataTable dt = ItemSummaryController.FetchStockBalanceWithZeroQty(searchQuery, inventoryLocationID, isRemoveZeroQty);
        return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    [WebMethod]
    public byte[] FetchStockBalanceReportWithZeroQtyByCategory(string searchQuery, int inventoryLocationID, bool isRemoveZeroQty, string categoryName)
    {
        DataTable dt = ItemSummaryController.FetchStockBalanceWithZeroQty(searchQuery, inventoryLocationID, isRemoveZeroQty, categoryName);
        return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    [WebMethod]
    public byte[] FetchStockSummaryReport(string searchQuery)
    {
        DataTable dt = ReportController.FetchStockReportBreakdownByLocationItemSummaryWithSP(searchQuery);
        return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    [WebMethod]
    public byte[] FetchStockSummaryReportWithRemoveZeroQty(bool removeZeroQty, string searchQuery)
    {
        DataTable dt = ReportController.FetchStockReportBreakdownByLocationItemSummaryWithSPWithRemoveZeroQty(removeZeroQty, searchQuery);
        return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    [WebMethod]
    public byte[] FetchStockCardReport(DateTime startDate, DateTime endDate, int LocationId, string itemName, string categoryName)
    {
        DataTable dt = ReportController.FetchStockCardReport(startDate, endDate, LocationId, itemName, categoryName);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        return SyncClientController.CompressDataSetToByteArray(ds);
    }

    [WebMethod]
    public byte[] FetchInventoryActivityReportWithTransfer(bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemName, string UserName, string InventoryLocationID, string movementType,
            string remark, string lineremark)
    {
        DataTable dt = ReportController.FetchInventoryActivityReportWithTransfer(useStartDate, useEndDate, StartDate, EndDate,
            ItemName, UserName, InventoryLocationID, movementType,
            remark, lineremark, "", "");
        return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    [WebMethod]
    public byte[] FetchInventoryActivityReportWithTransferAndRefNo(bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemName, string UserName, string InventoryLocationID, string movementType, string refNo,
            string remark, string lineremark)
    {
        DataTable dt = ReportController.FetchInventoryActivityReportWithTransferAndRefNo(useStartDate, useEndDate, StartDate, EndDate,
            ItemName, UserName, InventoryLocationID, movementType, 
            remark, lineremark, refNo, "", "");
        return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    [WebMethod]
    public byte[] FetchInventoryActivityHeader
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string UserName, string InventoryLocationID, string movementType, string remark,
            string SortColumn, string SortDir, bool showGoodsReceive)
    {
        DataTable dt = ReportController.FetchInventoryActivityHeader(useStartDate, useEndDate, StartDate, EndDate,
            UserName, InventoryLocationID, movementType, remark,
            SortColumn, SortDir, showGoodsReceive);
        return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    [WebMethod]
    public byte[] FetchInventoryActivityHeaderWithRefNo
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string UserName, string InventoryLocationID, string movementType, string remark, string refNo, 
            string SortColumn, string SortDir, bool showGoodsReceive)
    {
        DataTable dt = ReportController.FetchInventoryActivityHeaderWithRefNo(useStartDate, useEndDate, StartDate, EndDate,
            UserName, InventoryLocationID, movementType, remark, refNo,
            SortColumn, SortDir, showGoodsReceive);
        return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    [WebMethod]
    public decimal FetchCostPriceByItemForStockIn
            (string ItemNo, string supplier)
    {
        decimal res = 0;
        Item it = new Item(ItemNo);
        if (it != null && it.ItemNo != "")
        {
            res = it.FactoryPrice;
        }

        if (supplier != "")
        {

            ItemSupplierMapCollection ism = new ItemSupplierMapCollection();
            ism.Where(ItemSupplierMap.Columns.ItemNo, ItemNo);
            ism.Where(ItemSupplierMap.Columns.SupplierID, supplier);
            ism.Load();
            if (ism.Count > 0)
            {
                res = ism[0].CostPrice;
            }

        }
        return res;
    }

    [WebMethod]
    public decimal GetStockBalanceQtyByItemSummaryByDate
            (string ItemNo, int locationID, DateTime RequiredDate, out string status)
    {
        return InventoryController.GetStockBalanceQtyByItemSummaryByDate(ItemNo, locationID, RequiredDate, out status);
    }

    [WebMethod]
    public byte[] FetchInventoryControllerByRefNo
            (string InventoryHdrRefNo)
    {
        InventoryController inv = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
        inv.LoadConfirmedInventoryController(InventoryHdrRefNo);
        DataSet myDataSet = new DataSet();
        myDataSet.Tables.Add(inv.InvHdrToDataTable());
        myDataSet.Tables.Add(inv.InvDetToDataTable());
        byte[] data = SyncClientController.CompressDataSetToByteArray(myDataSet);
        return data;
    }

    [WebMethod]
    public string GetTransferDestination(string InventoryHdrRefNo)
    {
        return InventoryController.GetTransferDestination(InventoryHdrRefNo);
    }

    [WebMethod]
    public string GetSourceDestination(string InventoryHdrRefNo)
    {
        return InventoryController.GetSourceDestination(InventoryHdrRefNo);
    }

    [WebMethod]
    public byte[] FetchItemTraceReport(DateTime StartDate, DateTime EndDate,
            string ItemName)
    {
        DataTable dt = InventoryController.FetchItemTrace(true, true, StartDate, EndDate,
            ItemName);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    [WebMethod]
    public string getSupplierByID(int suppplierID)
    {
        try
        {
            Supplier res = new Supplier(suppplierID);
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        catch (Exception ex) { Logger.writeLog(ex.Message); return Newtonsoft.Json.JsonConvert.SerializeObject(new Supplier()); ; }
    }


    [WebMethod]
    public string getSupplierByName(string suppName)
    {
        try
        {
            Supplier res = new Supplier(Supplier.Columns.SupplierName, suppName); ;
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        catch (Exception ex) { Logger.writeLog(ex.Message); return Newtonsoft.Json.JsonConvert.SerializeObject(new Supplier()); ; }
    }

    [WebMethod]
    public byte[] LoadConfirmedPurchaseOrderControllerForStockIn(string refNo, out string status)
    {
        status = "";
        try
        {
            PurchaseOdrController poc = new PurchaseOdrController();
            if (poc.LoadConfirmedPurchaseOrderControllerForStockIn(refNo, out status))
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(poc.InvHdrToDataTable());
                ds.Tables.Add(poc.InvDetToDataTable());
                return SyncClientController.CompressDataSetToByteArray(ds);
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        //return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    [WebMethod]
    public byte[] GetGoodsReceiveList(string refNo, out string status)
    {
        status = "";
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = ReportController.GetGoodsReceiveList(refNo, out status);
            ds.Tables.Add(dt);
            return SyncClientController.CompressDataSetToByteArray(ds);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [WebMethod]
    public byte[] InventoryFetchItemWithFilterWithSupplierID(string searchText, int supplierID, string numOfDays, string outletName, int invLocation, out string message)
    {
        DataTable dt = InventoryController.InventoryFetchItemWithFilterWithSupplierID(searchText, supplierID, numOfDays, outletName, invLocation, out message);
        return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    [WebMethod]
    public byte[] InventoryFetchItemWithFilter(string searchText, string supplierName, string numOfDays, string outletName, int invLocation, out string message)
    {
        DataTable dt = InventoryController.InventoryFetchItemWithFilter(searchText, supplierName, numOfDays, outletName, invLocation, out message);
        return SyncClientController.CompressDataSetToByteArray(dt.DataSet);
    }

    #endregion

    #region [SQL Change Tracking]

    [WebMethod]
    public long GetChangeTrackingCurrentVersion()
    {
        return SynchronizationController.GetChangeTrackingCurrentVersion();
    }

    [WebMethod]
    public int GetChangeTrackingVersionSyncStatus(long appSettingChangeTrackingVersion)
    {
        return SynchronizationController.GetChangeTrackingVersionSyncStatus(appSettingChangeTrackingVersion);
    }

    [WebMethod(CacheDuration = 0)]
    public byte[] GetChangeTrackingChangesCompressed(string tableName, long lastSyncVersion, string primaryKeyName)
    {
        return SynchronizationController.GetChangeTrackingChangesCompressed(tableName, lastSyncVersion, primaryKeyName);
    }

    [WebMethod]
    public bool EnableChangeTracking()
    {
        return SQLChangeTracking.EnableChangeTracking();
    }

    [WebMethod]
    public bool EnableChangeTrackingTable(string tableName)
    {
        return SQLChangeTracking.EnableChangeTrackingTable(tableName);
    }

    [WebMethod]
    public long? GetChangeTrackingVersion()
    {
        return SQLChangeTracking.GetChangeTrackingVersion();
    }

    [WebMethod]
    public long? GetChangeTrackingTableMinValidVersion(string tableName)
    {
        return SQLChangeTracking.GetMinValidVersion(tableName);
    }

    [WebMethod]
    public byte[] GetChangeTrackingFullTable(string tableName, string primaryKeyName)
    {
        return SyncClientController.CompressDataSetToByteArray(SQLChangeTracking.GetFullTable(tableName, primaryKeyName));
    }

    [WebMethod]
    public byte[] GetInventoryHdrFullTable(DateTime StartDate)
    {
        return SyncClientController.CompressDataSetToByteArray(SQLChangeTracking.GetInventoryHdrFullTable(StartDate));
    }

    [WebMethod]
    public byte[] GetInventoryDetFullTable(DateTime StartDate)
    {
        return SyncClientController.CompressDataSetToByteArray(SQLChangeTracking.GetInventoryDetFullTable(StartDate));
    }

    [WebMethod]
    public byte[] GetChangeTrackingChangesTable(string tableName, long lastSyncVersion, string primaryKeyname)
    {
        return SyncClientController.CompressDataSetToByteArray(SQLChangeTracking.GetChanges(tableName, lastSyncVersion, primaryKeyname));
    }

    #endregion

    #region [App Setting]

    [WebMethod]
    public string GetAppSettingValue(string appSettingKey)
    {
        return AppSetting.GetSetting(appSettingKey);
    }

    [WebMethod]
    public void SetAppSettingValue(string appSettingKey, string appSettingValue)
    {
        AppSetting.SetSetting(appSettingKey, appSettingValue);
    }

    [WebMethod]
    public void SetAppSettingValueBatch(DataTable Settings)
    {
        if (Settings != null)
        {
            foreach (DataRow dr in Settings.Rows)
            {
                AppSetting.SetSetting(dr["AppSettingKey"].ToString(), dr["AppSettingValue"].ToString());
            }
        }
    }

    [WebMethod]
    public void DeleteAppSettingKey(string appSettingKey)
    {
        AppSetting.DeleteSetting(appSettingKey);
    }

    #endregion

    #region [Misc]

    [WebMethod(CacheDuration = 0)]
    public bool ExecuteUpdateUsingDataTable(DataTable dt)
    {
        return SyncClientController.ExecuteUpdateUsingDataTable(dt);
    }

    [WebMethod(CacheDuration = 0)]
    public bool UploadOrderTables(byte[] data, string username, out string status)
    {
        try
        {
            // Unpack Data
            DataSet orderDataSet = SyncClientController.DecompressDataSetFromByteArray(data);

            // Spread into DataTables
            DataTable dtOrderHdr = orderDataSet.Tables["OrderHdr"].Copy().RemoveExisting("OrderHdr", "OrderHdrID");
            DataTable dtOrderDet = orderDataSet.Tables["OrderDet"].Copy().RemoveExisting("OrderDet", "OrderDetID");
            DataTable dtReceiptHdr = orderDataSet.Tables["ReceiptHdr"].Copy().RemoveExisting("ReceiptHdr", "ReceiptHdrID");
            DataTable dtReceiptDet = orderDataSet.Tables["ReceiptDet"].Copy().RemoveExisting("ReceiptDet", "ReceiptDetID");
            DataTable dtSalesCommissionRecord = orderDataSet.Tables["SalesCommissionRecord"].Copy().RemoveExisting("SalesCommissionRecord", "CommissionRecordID");
            DataSet dsOrderHdr = new DataSet();
            dsOrderHdr.Tables.Add(dtOrderHdr);
            DataSet dsOrderDet = new DataSet();
            dsOrderDet.Tables.Add(dtOrderDet);
            DataSet dsReceiptHdr = new DataSet();
            dsReceiptHdr.Tables.Add(dtReceiptHdr);
            DataSet dsReceiptDet = new DataSet();
            dsReceiptDet.Tables.Add(dtReceiptDet);
            DataSet dsSalesCommissionRecord = new DataSet();
            dsSalesCommissionRecord.Tables.Add(dtSalesCommissionRecord);

            // Transform into QueryCommand Foreach DataTable
            QueryCommandCollection cmdCol = new QueryCommandCollection();
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsOrderHdr, false));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsOrderDet, false));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsReceiptHdr, false));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsReceiptDet, false));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK
                (dsSalesCommissionRecord));

            // Execute Transaction
            DataService.ExecuteTransaction(cmdCol);

            status = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            return false;
        }
    }

    [WebMethod(CacheDuration = 0)]
    public bool UploadLogsTables(byte[] data, string username, out string status)
    {
        try
        {
            // Unpack Data
            DataSet logsDataSet = SyncClientController.DecompressDataSetFromByteArray(data);

            // Spread into DataTables
            DataTable dtWarningMsg = logsDataSet.Tables["WarningMsg"].Copy().RemoveExisting("WarningMsg", "UniqueID"); ;
            DataTable dtMembershipUpgradeLog = logsDataSet.Tables["MembershipUpgradeLog"].Copy().RemoveExisting("MembershipUpgradeLog", "OrderHdrID");
            DataTable dtCashRecording = logsDataSet.Tables["CashRecording"].Copy().RemoveExisting("CashRecording", "CashRecID");
            DataTable dtCounterCloseLog = logsDataSet.Tables["CounterCloseLog"].Copy().RemoveExisting("CounterCloseLog", "CounterCloseLogID");
            DataTable dtCounterCloseDet = logsDataSet.Tables["CounterCloseDet"].Copy().RemoveExisting("CounterCloseDet", "CounterCloseDetID");
            DataTable dtSpecialActivityLog = logsDataSet.Tables["SpecialActivityLog"].Copy().RemoveExisting("SpecialActivityLog", "SpecialActivityLogID");
            DataTable dtPreOrderRecord = logsDataSet.Tables["PreOrderRecord"].Copy().RemoveExisting("PreOrderRecord", "PreOrderLogID");
            DataTable dtMembership = logsDataSet.Tables["Membership"].Copy().RemoveExisting("Membership", "MembershipNo");
            DataTable dtMembershipRenewal = logsDataSet.Tables["MembershipRenewal"].Copy().RemoveExisting("MembershipRenewal", "renewalID");
            DataTable dtPackageRedemptionLog = logsDataSet.Tables["PackageRedemptionLog"].Copy().RemoveExisting("PackageRedemptionLog", "PackageRedeemID");


            DataSet dsWarningMsg = new DataSet();
            dsWarningMsg.Tables.Add(dtWarningMsg);
            DataSet dsMembershipUpgradeLog = new DataSet();
            dsMembershipUpgradeLog.Tables.Add(dtMembershipUpgradeLog);
            DataSet dsCashRecording = new DataSet();
            dsCashRecording.Tables.Add(dtCashRecording);
            DataSet dsCounterCloseLog = new DataSet();
            dsCounterCloseLog.Tables.Add(dtCounterCloseLog);
            DataSet dsCounterCloseDet = new DataSet();
            dsCounterCloseDet.Tables.Add(dtCounterCloseDet);
            DataSet dsSpecialActivityLog = new DataSet();
            dsSpecialActivityLog.Tables.Add(dtSpecialActivityLog);
            DataSet dsPreOrderRecord = new DataSet();
            dsPreOrderRecord.Tables.Add(dtPreOrderRecord);
            DataSet dsMembership = new DataSet();
            dsMembership.Tables.Add(dtMembership);
            DataSet dsMembershipRenewal = new DataSet();
            dsMembershipRenewal.Tables.Add(dtMembershipRenewal);
            DataSet dsPackageRedemptionLog = new DataSet();
            dsPackageRedemptionLog.Tables.Add(dtPackageRedemptionLog);

            // Transform into QueryCommand Foreach DataTable
            QueryCommandCollection cmdCol = new QueryCommandCollection();
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsWarningMsg, false));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsMembershipUpgradeLog, false));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK
                (dsCashRecording));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK
                (dsCounterCloseLog));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK(dsCounterCloseDet));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK
                (dsSpecialActivityLog));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK
                (dsPreOrderRecord));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsMembership, false));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK
                (dsMembershipRenewal));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK
                (dsPackageRedemptionLog));

            // Execute Transaction
            DataService.ExecuteTransaction(cmdCol);

            status = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            return false;
        }
    }

    [WebMethod(CacheDuration = 0)]
    public bool UploadTables(DataTable table, string primaryKey, string updateKey, bool isPKAutoGenerate, out string status)
    {
        bool isSuccess = false;
        status = "";
        try
        {
            isSuccess = SyncTables.UploadTable(table, primaryKey, updateKey, isPKAutoGenerate, out status);
        }
        catch (Exception ex)
        {
            isSuccess = false;
            status = "ERROR : " + ex.Message;
            Logger.writeLog(ex);
        }
        return isSuccess;
    }

    [WebMethod]
    public bool DeleteInventoryDetFromVoidedOrder()
    {
        try
        {
            return SynchronizationController.DeleteInventoryDetFromVoidedOrder();
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            return false;
        }
    }

    [WebMethod(CacheDuration = 0)]
    public bool UploadInventoryTables(byte[] data, string username, out string status)
    {
        try
        {
            // Unpack Data
            DataSet inventoryDataSet = SyncClientController.DecompressDataSetFromByteArray(data);

            // Spread into DataTables
            DataTable dtInventoryHdr = inventoryDataSet.Tables["InventoryHdr"].Copy().RemoveExisting("InventoryHdr", "InventoryHdrRefNo");
            DataTable dtInventoryDet = inventoryDataSet.Tables["InventoryDet"].Copy().RemoveExisting("InventoryDet", "InventoryDetRefNo");
            DataTable dtStockTake = inventoryDataSet.Tables["StockTake"].Copy().RemoveExisting("StockTake", "StockTakeID");
            DataTable dtLocationTransfer = inventoryDataSet.Tables["LocationTransfer"].Copy().RemoveExisting("LocationTransfer", "LocationTransferID");


            DataSet dsInventoryHdr = new DataSet();
            dsInventoryHdr.Tables.Add(dtInventoryHdr);
            DataSet dsInventoryDet = new DataSet();
            dsInventoryDet.Tables.Add(dtInventoryDet);
            DataSet dsStockTake = new DataSet();
            dsStockTake.Tables.Add(dtStockTake);
            DataSet dsLocationTransfer = new DataSet();
            dsLocationTransfer.Tables.Add(dtLocationTransfer);

            // Transform into QueryCommand Foreach DataTable
            QueryCommandCollection cmdCol = new QueryCommandCollection();
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsInventoryHdr, false));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsInventoryDet, false));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsStockTake, true));
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsLocationTransfer, true));

            // Execute Transaction
            DataService.ExecuteTransaction(cmdCol);

            status = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            return false;
        }
    }

    /// <summary>
    /// Upload table to server, default method of getting data is by using SQLChangeTracking GetFullTableOperationIfExists
    /// Dataset.Table[0] will be used
    /// </summary>
    /// <param name="data"></param>
    /// <param name="username"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    [WebMethod(CacheDuration = 0)]
    public bool UploadTable(byte[] data, bool isPKAutoGenerated, string username, out string status)
    {
        try
        {
            // Unpack data
            DataSet ds = SyncClientController.DecompressDataSetFromByteArray(data);

            // Transform dataset
            QueryCommandCollection cmdCol = new QueryCommandCollection();
            cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(ds, isPKAutoGenerated));

            // Execute
            DataService.ExecuteTransaction(cmdCol);

            status = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            status = ex.Message;
            Logger.writeLog(ex);
            return false;
        }
    }

    private bool SavePurchaseOrder(
            byte[] data,
            string username, int InventoryLocationID,
            bool CalculateCOGS, out string newRefNo, out string newCustomRefNo,
            out string status)
    {
        try
        {
            PurchaseOdrController ctrl = new PurchaseOdrController();
            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
            if (myDataSet == null || myDataSet.Tables.Count < 2)
            {
                newRefNo = "";
                newCustomRefNo = "";
                status = "Unable to decompress parameter from server";
                Logger.writeLog(status);
                return false;
            }
            if (ctrl.LoadFromDataTable(myDataSet.Tables[0], myDataSet.Tables[1]))
            {
                if (ctrl.CreateOrder(username, InventoryLocationID, out status))
                {
                    newRefNo = ctrl.GetPurchaseOrderHdrRefNo();
                    newCustomRefNo = ctrl.InvHdr.CustomRefNo;

                    #region *) Handle Attachment data
                    // If there's attachment
                    if (myDataSet.Tables.Count == 3)
                    {
                        SaveAttachment(myDataSet.Tables[2], "PurchaseOrder", newRefNo, username, out status);

                        //    string tempDir = "";
                        //    FileAttachmentCollection faColl = new FileAttachmentCollection();
                        //    faColl.Load(myDataSet.Tables[2]);

                        //    foreach (FileAttachment fa in faColl)
                        //    {
                        //        string dir = Server.MapPath("~/Attachment/PurchaseOrder/" + newRefNo);
                        //        if (!Directory.Exists(dir))
                        //        {
                        //            Directory.CreateDirectory(dir);
                        //        }
                        //        FileInfo fInfo = new FileInfo(Path.Combine(fa.FileLocation, fa.FileName));
                        //        fInfo.MoveTo(Path.Combine(dir, fa.FileName));
                        //        tempDir = fa.FileLocation;

                        //        fa.FileLocation = "~/Attachment/PurchaseOrder/" + newRefNo;
                        //        fa.RefID = newRefNo;
                        //        fa.IsNew = true;
                        //        fa.Save("SYNC");
                        //    }

                        //    //Clean up, delete the temp dir
                        //    if (!string.IsNullOrEmpty(tempDir)) Directory.Delete(tempDir);
                    }
                    #endregion

                    return true;
                }
                else
                {
                    newRefNo = "";
                    newCustomRefNo = "";
                    return false;
                }
            }
            else
            {
                newRefNo = "";
                newCustomRefNo = "";
                status = "Unable to load data table";
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            newRefNo = "";
            newCustomRefNo = "";
            return false;
        }
    }

    private bool SaveEditPurchaseOrder(
            byte[] data,
            string username, int InventoryLocationID,
            bool CalculateCOGS, bool IsEdit, out string newRefNo, out string newCustomRefNo,
            out string status)
    {
        try
        {
            PurchaseOdrController ctrl = new PurchaseOdrController();
            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
            if (myDataSet == null || myDataSet.Tables.Count < 2)
            {
                newRefNo = "";
                newCustomRefNo = "";
                status = "Unable to decompress parameter from server";
                Logger.writeLog(status);
                return false;
            }
            if (ctrl.LoadFromDataTable(myDataSet.Tables[0], myDataSet.Tables[1]))
            {
                if (ctrl.CreateEditOrder(username, InventoryLocationID, IsEdit, out status))
                {
                    newRefNo = ctrl.GetPurchaseOrderHdrRefNo();
                    newCustomRefNo = ctrl.InvHdr.CustomRefNo;

                    #region *) Handle Attachment data
                    // If there's attachment
                    if (myDataSet.Tables.Count == 3)
                    {
                        SaveAttachment(myDataSet.Tables[2], "PurchaseOrder", newRefNo, username, out status);
                    }
                    #endregion

                    return true;
                }
                else
                {
                    newRefNo = "";
                    newCustomRefNo = "";
                    return false;
                }
            }
            else
            {
                newRefNo = "";
                newCustomRefNo = "";
                status = "Unable to load data table";
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            newRefNo = "";
            newCustomRefNo = "";
            return false;
        }
    }

    [WebMethod]
    public bool PurchaseOrderCompressed(//DataTable hdr, DataTable det,
            byte[] data,
            string username, int InventoryLocationID,
            bool CalculateCOGS, out string newRefNo,
            out string status)
    {
        string newCustomRefNo;
        return SavePurchaseOrder(data, username, InventoryLocationID, CalculateCOGS, out newRefNo, out newCustomRefNo, out status);
    }

    [WebMethod]
    public bool PurchaseOrderCompressedWithCustomRefNo(//DataTable hdr, DataTable det,
            byte[] data,
            string username, int InventoryLocationID,
            bool CalculateCOGS, out string newRefNo, out string newCustomRefNo,
            out string status)
    {
        return SavePurchaseOrder(data, username, InventoryLocationID, CalculateCOGS, out newRefNo, out newCustomRefNo, out status);
    }

    [WebMethod]
    public bool PurchaseOrderCompressedWithCustomRefNoEdit(//DataTable hdr, DataTable det,
            byte[] data,
            string username, int InventoryLocationID,
            bool CalculateCOGS, bool IsEdit, out string newRefNo, out string newCustomRefNo,
            out string status)
    {
        return SaveEditPurchaseOrder(data, username, InventoryLocationID, CalculateCOGS, IsEdit, out newRefNo, out newCustomRefNo, out status);
    }

    [WebMethod]
    public bool SaveAttachment(DataTable data, string moduleName, string refNo, string username, out string status)
    {
        status = "";
        try
        {
            string tempDir = "";
            FileAttachmentCollection faColl = new FileAttachmentCollection();
            faColl.Load(data);

            foreach (FileAttachment fa in faColl)
            {
                string dir = Server.MapPath("~/Attachment/" + moduleName + "/" + refNo);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                FileInfo fInfo = new FileInfo(Path.Combine(fa.FileLocation, fa.FileName));
                fInfo.MoveTo(Path.Combine(dir, fa.FileName));
                tempDir = fa.FileLocation;

                fa.FileLocation = "~/Attachment/" + moduleName + "/" + refNo;
                fa.RefID = refNo;
                fa.IsNew = true;
                fa.Save(username);
            }

            //Clean up, delete the temp dir
            if (!string.IsNullOrEmpty(tempDir)) Directory.Delete(tempDir);

            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            return false;
        }
    }

    [WebMethod]
    public bool UploadAttachment(byte[] data, string tempFolder, string fileName, bool isLocalhost, string moduleName, out string serverPath, out string status)
    {
        status = "";
        serverPath = "";
        try
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                string dir;
                if (isLocalhost)
                    dir = Server.MapPath("~/Attachment/" + moduleName + "/" + tempFolder);
                else
                    dir = Server.MapPath("~/Attachment/Temp/" + tempFolder);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                serverPath = Path.Combine(dir, fileName);
                using (FileStream fs = new FileStream(serverPath, FileMode.Create))
                {
                    ms.WriteTo(fs);
                    ms.Close();
                    fs.Close();
                    fs.Dispose();
                }

                if (isLocalhost)
                    serverPath = "~/Attachment/" + moduleName + "/" + tempFolder + "/" + fileName;
            }
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            return false;
        }
    }

    [WebMethod]
    public bool DeleteAttachment(DataTable data, string username, out string status)
    {
        status = "";
        try
        {
            foreach (DataRow dr in data.Rows)
            {
                Guid attachmentID = new Guid(dr["AttachmentID"].ToString());
                FileAttachment fa = new FileAttachment(attachmentID);
                if (fa != null && fa.AttachmentID == attachmentID)
                {
                    fa.Deleted = true;
                    fa.Save(username);

                    string filePath = Server.MapPath(Path.Combine(fa.FileLocation, fa.FileName));
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            return false;
        }
    }

    [WebMethod]
    public DataTable GetCustomPONoSetting()
    {
        DataTable dt = new DataTable("AppSetting");
        dt.Columns.Add("key", Type.GetType("System.String"));
        dt.Columns.Add("value", Type.GetType("System.String"));
        dt.Rows.Add(AppSetting.SettingsName.PurchaseOrder.UseCustomNo, AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo));
        dt.Rows.Add(AppSetting.SettingsName.PurchaseOrder.CustomPrefix, AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomPrefix));
        dt.Rows.Add(AppSetting.SettingsName.PurchaseOrder.CustomSuffix, AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomSuffix));
        dt.Rows.Add(AppSetting.SettingsName.PurchaseOrder.NumberLength, AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.NumberLength));
        dt.Rows.Add(AppSetting.SettingsName.PurchaseOrder.CurrentNo, AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CurrentNo));
        dt.Rows.Add(AppSetting.SettingsName.PurchaseOrder.ResetNumberEvery, AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ResetNumberEvery));
        dt.Rows.Add(AppSetting.SettingsName.PurchaseOrder.CustomNoDateFormat, AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomNoDateFormat));
        dt.Rows.Add(AppSetting.SettingsName.PurchaseOrder.LastReset, AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.LastReset));
        return dt;
    }

    [WebMethod]
    public DataTable GetCustomGRNoSetting()
    {
        DataTable dt = new DataTable("AppSetting");
        dt.Columns.Add("key", Type.GetType("System.String"));
        dt.Columns.Add("value", Type.GetType("System.String"));
        dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.UseCustomNo, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.UseCustomNo));
        dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.CustomPrefix, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomPrefix));
        dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.CustomSuffix, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomSuffix));
        dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.NumberLength, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.NumberLength));
        dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.CurrentNo, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CurrentNo));
        dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.ResetNumberEvery, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ResetNumberEvery));
        dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.CustomNoDateFormat, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomNoDateFormat));
        dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.LastReset, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.LastReset));
        return dt;
    }


    [WebMethod]
    public bool SyncCategoryFromMagento(out string status)
    {
        try
        {
            status = "";
            if (SyncClientController.SyncCategory())
            {

                //LoadCategory();
                return true;
            }
            else
            {
                status = "Failed to Sync";
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            return false;
        }
    }

    #endregion

    #region [Real Time Sync]

    [WebMethod(CacheDuration = 0)]
    public bool SubmitSales(byte[] data, string username, bool createPurchaseOrder, out string status, out string orderHdrID)
    {
        orderHdrID = "";
        try
        {
            // Unpack Data
            DataSet orderDataSet = SyncClientController.DecompressDataSetFromByteArray(data);

            // Spread into DataTables
            DataTable dtOrderHdr = orderDataSet.Tables["OrderHdr"].Copy();
            DataTable dtOrderDet = orderDataSet.Tables["OrderDet"].Copy();
            DataTable dtReceiptHdr = orderDataSet.Tables["ReceiptHdr"].Copy();
            DataTable dtReceiptDet = orderDataSet.Tables["ReceiptDet"].Copy();
            DataTable dtMember = orderDataSet.Tables["Member"].Copy();
            DataSet dsOrderHdr = new DataSet();
            dsOrderHdr.Tables.Add(dtOrderHdr);
            DataSet dsOrderDet = new DataSet();
            dsOrderDet.Tables.Add(dtOrderDet);
            DataSet dsReceiptHdr = new DataSet();
            dsReceiptHdr.Tables.Add(dtReceiptHdr);
            DataSet dsReceiptDet = new DataSet();
            dsReceiptDet.Tables.Add(dtReceiptDet);


            POSController pos = new POSController();
            pos.myOrderHdr.Load(dsOrderHdr.Tables[0]);
            pos.myOrderDet.Load(dsOrderDet.Tables[0]);
            pos.recHdr.Load(dsOrderDet.Tables[0]);
            pos.recDet.Load(dsReceiptDet.Tables[0]);
            pos.CurrentMember = new Membership();
            if (dtMember.Rows.Count > 0)
                pos.CurrentMember.Load(dtMember);
            else
                pos.CurrentMember = new Membership("WALK-IN");
            bool IsPointAllocationSuccess = false;
            bool isSuccessful = pos.ConfirmOrderFromRealTimeSync
                    (false, out IsPointAllocationSuccess, username, out status);

            if (isSuccessful)
            {
                orderHdrID = pos.myOrderHdr.OrderHdrID;
                if (!pos.ExecuteStockOut(out status))
                {
                    Logger.writeLog("Failed to do stock out " + pos.GetUnsavedRefNo());
                }
                if (!InstallmentController.UpdateInstallmentByOrderHdr(orderHdrID, out status))
                {
                    Logger.writeLog("Failed to update installment data " + pos.GetUnsavedRefNo());
                }
            }
            else
            {
                pos.DeleteAllReceiptLinePayment();
                return false;
            }

            // The PO has not implement to the retail yet
            //if (createPurchaseOrder)
            //{
            //    if (!PurchaseOrderController.createPurchaseOrderFromSales(pos, true, true, out status))
            //    {
            //        return false;
            //    }
            //}

            status = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            return false;
        }
    }

    [WebMethod]
    public bool AddAccessLog(int pointOfSaleID, AccessSource src, string userName, string secondUserName, string accessTpe, string remarks)
    {
        bool result = true;

        try
        {
            AccessLogController.AddLogHelper(pointOfSaleID, src, userName, secondUserName, accessTpe, remarks);
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return result;
    }

    [WebMethod]
    public int[] FetchUpdatedGroupUserPrivileges(DateTime modifiedOn)
    {
        GroupUserPrivilegeCollection gupColl = new GroupUserPrivilegeCollection();
        gupColl.Where(GroupUserPrivilege.Columns.ModifiedOn, Comparison.GreaterThan, modifiedOn);
        gupColl.Load();
        return gupColl.Select(o => o.GroupID).Distinct().ToArray();
    }

    [WebMethod]
    public byte[] FetchGroupPrivilegesRealTime(int groupID)
    {
        return SynchronizationController.FetchGroupPrivilegesRealTimeCompressed(groupID);
    }

    [WebMethod]
    public int GetItemRecordCountAfterTimestamp(DateTime lastModifiedOn, string OutletName)
    {
        return SynchronizationController.GetItemTableRealTimeCount(lastModifiedOn, OutletName);
    }

    [WebMethod]
    public byte[] GetItemTableCompressedRealTime(DateTime lastModifiedOn, string OutletName, int count)
    {
        return SynchronizationController.GetItemTableRealTime(lastModifiedOn, OutletName, count);
    }

    [WebMethod]
    public byte[] FetchDataSetByTimestamp(string tableName, string pkColumn, DateTime modifiedDate, int noOfRecords, bool useInventoryLoc, int invLocID, bool usePOSID, int posID)
    {
        return SynchronizationController.FetchDataSetRealTimeCompressed(tableName, pkColumn, modifiedDate, noOfRecords, useInventoryLoc, invLocID, usePOSID, posID);
    }

    [WebMethod]
    public byte[] FetchDataSetByTimeStampPerMember(string tableName, string pkColumn, DateTime modifiedDate, int noOfRecords, string membershipNo)
    {
        return SynchronizationController.FetchDataSetRealTimeCompressedPerMember(tableName, pkColumn, modifiedDate, noOfRecords, membershipNo);
    }

    [WebMethod]
    public byte[] FetchDataSetByTimeStampPerModuleName(string tableName, string pkColumn, DateTime modifiedDate, int noOfRecords, string moduleName)
    {
        return SynchronizationController.FetchDataSetRealTimeCompressedPerModuleName(tableName, pkColumn, modifiedDate, noOfRecords, moduleName);
    }

    [WebMethod]
    public int FetchRecordNoByTimestamp(string tableName, DateTime modifiedOn)
    {
        return SynchronizationController.FetchRecordNoByTimestamp(tableName, modifiedOn);
    }

    [WebMethod]
    public int FetchRecordNoByTimestampByInvLocID(string tableName, DateTime modifiedOn, int invLocID)
    {
        return SynchronizationController.FetchRecordNoByTimestampByInvLocID(tableName, modifiedOn, invLocID);
    }

    [WebMethod]
    public int FetchRecordNoByTimestampByPOSID(string tableName, DateTime modifiedOn, int posID)
    {
        return SynchronizationController.FetchRecordNoByTimestampByPOSID(tableName, modifiedOn, posID);
    }

    [WebMethod]
    public int FetchRecordNoByTimestampByOutletName(string tableName, DateTime modifiedOn, string outletName)
    {
        return SynchronizationController.FetchRecordNoByTimestampByOutletName(tableName, modifiedOn, outletName);
    }

    [WebMethod]
    public int FetchRecordNoByTimestampByMember(string tableName, DateTime modifiedOn, string membershipNo)
    {
        return SynchronizationController.FetchRecordNoByTimestampByMember(tableName, modifiedOn, membershipNo);
    }

    [WebMethod]
    public int FetchRecordNoByTimestampByModuleName(string tableName, DateTime modifiedOn, string moduleName)
    {
        return SynchronizationController.FetchRecordNoByTimestampByModuleName(tableName, modifiedOn, moduleName);
    }

    [WebMethod]
    public int FetchPromoRecordNoByTimestampByOutletName(DateTime modifiedOn, string outletName)
    {
        return SynchronizationController.FetchPromoRecordNoByTimestampByOutletName(modifiedOn, outletName);
    }

    [WebMethod]
    public int GetPromoRecordCountAfterTimestamp(DateTime lastModifiedOn)
    {
        return SynchronizationController.GetCountPromoRealTimeDataAllNEW(lastModifiedOn, int.MaxValue);
    }

    [WebMethod]
    public int FetchRecordNoByTimestampAppointment(DateTime modifiedOn, int pointOfSaleID)
    {
        return SynchronizationController.FetchRecordNoAppointment(modifiedOn, pointOfSaleID);
    }

    [WebMethod]
    public bool getSalesLastTimeStamp(int PointofSaleID, out DateTime res)
    {
        res = DateTime.Now;

        //string sqlString = "Select ISNULL(max(modifiedon),'2011-01-01') as tgl from orderhdr where pointofsaleid = " + PointofSaleID.ToString();

        string sqlString = ""; object obj;
        if (AppSetting.CastBool(AppSetting.GetSetting("UseServerQuickRef"), false))
        {
            sqlString = "Select ISNULL(lastmodifiedon,'2001-01-01') as tgl from serverquickref where pointofsaleid = " + PointofSaleID.ToString();

            obj = DataService.ExecuteScalar(new QueryCommand(sqlString));
            if (obj != null)
            {
                DateTime Result;
                if (obj != null && obj.ToString() != "")
                {
                    res = (DateTime)obj;
                    if (res.Year != 2001)
                        return true;
                }
                else
                    return false;
            }
        }

        sqlString = "Select ISNULL(max(modifiedon),'2011-01-01') as tgl from orderhdr where pointofsaleid = " + PointofSaleID.ToString();
        obj = DataService.ExecuteScalar(new QueryCommand(sqlString));
        if (obj != null)
        {
            DateTime Result;
            if (obj != null && obj.ToString() != "")
            {
                res = (DateTime)obj;
                return true;
            }
            else
                return false;
        }
        return false;
    }

    [WebMethod]
    public DateTime GetAccessLogLastTimeStamp(int pointOfSaleID)
    {
        DateTime dt = DateTime.Now.AddYears(-5);

        try
        {
            string sql = @"SELECT	ISNULL(MAX(ModifiedOn),DATEADD(YEAR,-5,GETDATE())) LastDate
                            FROM	AccessLog 
                            WHERE	PointOfSaleID = {0} 
		                            AND AccessSource = 'POS'";
            sql = string.Format(sql, pointOfSaleID);
            DataTable data = new DataTable();
            data.Load(DataService.GetReader(new QueryCommand(sql)));
            if (data.Rows.Count > 0)
                dt = (DateTime)data.Rows[0]["LastDate"];
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return dt;
    }

    [WebMethod]
    public bool getCashRecordingLastTimeStamp(int PointofSaleID, out DateTime res)
    {
        res = DateTime.Now;

        string sqlString = @"SELECT ISNULL(MAX(ModifiedOn),'1990-01-01') AS Date 
                             FROM   CashRecording 
                             WHERE  PointOfSaleID = " + PointofSaleID;

        object obj = DataService.ExecuteScalar(new QueryCommand(sqlString));
        if (obj != null)
        {
            if (obj != null && obj.ToString() != "")
                res = (DateTime)obj;
            else
                return false;
            return true;
        }
        return true;
    }

    [WebMethod]
    public bool getQuotationLastTimeStamp(int PointofSaleID, out DateTime res)
    {
        res = DateTime.Now;

        string sqlString = "Select ISNULL(max(modifiedon),'2011-01-01') as tgl from QuotationHdr where pointofsaleid = " + PointofSaleID.ToString();


        object obj = DataService.ExecuteScalar(new QueryCommand(sqlString));
        if (obj != null)
        {
            DateTime Result;
            //Logger.writeLog(obj.ToString());
            if (obj != null && obj.ToString() != "")
                res = (DateTime)obj;
            else
                return false;

            return true;
        }
        return true;
    }

    [WebMethod]
    public bool getCounterCloselogLastTimeStamp(int PointofSaleID, out DateTime res)
    {
        res = DateTime.Now;

        string sqlString = "Select ISNULL(max(modifiedon),'2011-01-01') as tgl from countercloselog where pointofsaleid = " + PointofSaleID.ToString();


        object obj = DataService.ExecuteScalar(new QueryCommand(sqlString));
        if (obj != null)
        {
            DateTime Result;
            //Logger.writeLog(obj.ToString());
            if (obj != null && obj.ToString() != "")
                res = (DateTime)obj;
            else
                return false;

            return true;
        }
        return true;
    }

    [WebMethod]
    public DateTime GetAttendanceLastTimeStamp(int pointOfSaleID)
    {
        DateTime dt = DateTime.Now.AddYears(-5);

        try
        {
            string sql = @"SELECT	ISNULL(MAX(ModifiedOn),DATEADD(YEAR,-5,GETDATE())) LastDate
                            FROM	MembershipAttendance
                            WHERE	PointOfSaleID = {0}";
            sql = string.Format(sql, pointOfSaleID);
            DataTable data = new DataTable();
            data.Load(DataService.GetReader(new QueryCommand(sql)));
            if (data.Rows.Count > 0)
                dt = (DateTime)data.Rows[0]["LastDate"];
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return dt;
    }

    [WebMethod]
    public bool getLastTimeStampByPOSID(string tablename, int PointofSaleID, out DateTime res)
    {
        res = DateTime.Now;

        string sqlString = "Select ISNULL(max(modifiedon),'2011-01-01') as tgl from " + tablename + " where pointofsaleid = " + PointofSaleID.ToString();


        object obj = DataService.ExecuteScalar(new QueryCommand(sqlString));
        if (obj != null)
        {
            DateTime Result;
            //Logger.writeLog(obj.ToString());
            if (obj != null && obj.ToString() != "")
                res = (DateTime)obj;
            else
                return false;

            return true;
        }
        return true;
    }

    [WebMethod]
    public int GetInventoryCountRealTime(DateTime lastModifiedOn)
    {
        //Logger.writeLog("Get Real Time Count");
        return SynchronizationController.GetInventoryTableRealTimeCount(lastModifiedOn);
    }

    [WebMethod]
    public byte[] GetInventoryRealTimeData(DateTime lastModifiedOn, int recordsPerTime)
    {
        return SyncClientController.CompressDataSetToByteArray(SynchronizationController.GetInventoryRealTimeData(lastModifiedOn, recordsPerTime));
    }

    [WebMethod]
    public byte[] GetPromoRealTimeData(DateTime lastModifiedOn, int recordsPerTime, int PointOfSaleID)
    {
        return SyncClientController.CompressDataSetToByteArray(SynchronizationController.GetPromoRealTimeData(lastModifiedOn, recordsPerTime, PointOfSaleID));
    }

    [WebMethod]
    public byte[] GetPromoRealTimeDataAll(DateTime lastModifiedOn, int recordsPerTime)
    {
        return SyncClientController.CompressDataSetToByteArray(SynchronizationController.GetPromoRealTimeDataAllNEW(lastModifiedOn, recordsPerTime));
    }

    [WebMethod]
    public byte[] GetPurchaseOrderRealTimeData(DateTime lastModifiedOn, int recordsPerTime, int inventoryLocationID)
    {
        return SyncClientController.CompressDataSetToByteArray(SynchronizationController.GetPurchaseOrderRealTimeData(lastModifiedOn, recordsPerTime, inventoryLocationID));
    }

    [WebMethod]
    public byte[] GetItemGroupRealTimeData(DateTime lastModifiedOn, int recordsPerTime)
    {
        return SyncClientController.CompressDataSetToByteArray(SynchronizationController.GetItemGroupRealTimeData(lastModifiedOn, recordsPerTime));
    }

    [WebMethod]
    public byte[] GetStockTakeRealTimeData(DateTime lastModifiedOn, int recordsPerTime)
    {
        return SyncClientController.CompressDataSetToByteArray(SynchronizationController.GetStockTakeRealTimeData(lastModifiedOn, recordsPerTime));
    }

    [WebMethod]
    public byte[] GetPointsRealTimeDataAll(DateTime lastModifiedOn, int recordsPerTime, string membershipNo)
    {
        return SyncClientController.CompressDataSetToByteArray(SynchronizationController.GetPointsRealTimeDataAll(lastModifiedOn, recordsPerTime, membershipNo));
    }

    [WebMethod]
    public bool FetchOrdersCCMWRealTime(
        string[][] dsHeaders, string[][] dsDetails,
        string[][] dsReceiptHdr, string[][] dsReceiptDet, string[][] dsSalesComm, string[][] dsMember, string[][] dsVoidLog, string[][] dsUOMConv)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchOrdersDataCCMWRealTime(dsHeaders, dsDetails, dsReceiptHdr, dsReceiptDet, dsSalesComm, dsMember, dsVoidLog, dsUOMConv);
    }

    [WebMethod]
    public bool FetchAccessLogRealTime(string[][] accessLog)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller); 
        return SynchronizationController.FetchAccessLogRealTime(accessLog);
    }

    [WebMethod]
    public bool FetchAAttendanceRealTime(string[][] attendance)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller); 
        return SynchronizationController.FetchAttendanceRealTime(attendance);
    }

    [WebMethod]
    public bool FetchCounterCloseLogRealTime(string[][] dsCounterCloseLogs)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchCounterCloseRealTime(dsCounterCloseLogs);
    }

    [WebMethod]
    public bool FetchCounterCloseLogWithDetailRealTime(string[][] dsCounterCloseLogs, string[][] dsCounterCloseDets)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchCounterCloseWithDetailRealTime(dsCounterCloseLogs, dsCounterCloseDets);
    }

    [WebMethod]
    public bool FetchLoginActivityRealTime(string[][] dsLoginActivity)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchLoginActivityRealTime(dsLoginActivity);
    }

    [WebMethod]
    public bool FetchQuotationRealTime(string[][] dsQuoteHdr, string[][] dsQuoteDet)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchQuotationRealTime(dsQuoteHdr, dsQuoteDet);
    }

    [WebMethod]
    public bool FetchCashRecordingRealTime(string[][] dsCashRecording)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchCashRecordingRealTime(dsCashRecording);
    }

    [WebMethod]
    public bool FetchPerformanceLogRealTime(string[][] dsPerformanceLog)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchPerformanceLogRealTime(dsPerformanceLog);
    }

    [WebMethod]
    public bool FetchPerformanceLogSummaryRealTime(string[][] dsPerformanceLogSummary)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchPerformanceLogSummaryRealTime(dsPerformanceLogSummary);
    }

    [WebMethod]
    public bool SaveAppointment(DataSet Objs)
    {
        return SynchronizationController.SaveAppointment(Objs);
    }

    [WebMethod]
    public byte[] FetchDataSetByTimestampAppointment(DateTime modifiedDate, int noOfRecords, bool usePOSID, int posID)
    {
        return SynchronizationController.FetchDataSetRealTimeCompressedAppointment(modifiedDate, noOfRecords, usePOSID, posID);
    }

    [WebMethod]
    public byte[] FetchDataSetByTimestampSpecialDiscount(DateTime modifiedDate, int noOfRecords, string outletName)
    {
        return SynchronizationController.FetchDataSetRealTimeCompressedSpecialDiscounts(modifiedDate, noOfRecords, outletName);
    }

    #endregion

    #region [Saved Inventory File]
    [WebMethod]
    public string SaveInventoryFile(
        byte[] InventoryDS, string movementType, string remark, bool autosave)
    {
        //string result
        //String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        DataSet hdrDS = SyncClientController.DecompressDataSetFromByteArray(InventoryDS);
        //DataSet detDS = SyncClientController.DecompressDataSetFromByteArray(InventoryDetDS);

        return SynchronizationController.SaveInventoryFile(hdrDS, movementType, remark, autosave);
        //return 

    }

    [WebMethod]
    public byte[] GetInventorySavedFile(string ar)
    {
        //string result
        //String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.GetInventorySavedFile(ar);
        //return 

    }

    [WebMethod]
    public byte[] LoadInventoryFromFile(string saveName)
    {
        return SynchronizationController.LoadInventoryFromFile(saveName);
    }

    [WebMethod]
    public bool removeSavedFile(string saveName)
    {
        return SynchronizationController.removeSavedFile(saveName);
    }
    #endregion

    [WebMethod]
    public bool GetAppointmentLastTimeStamp(int PointofSaleID, out DateTime res, bool IsUpdateServer)
    {
        res = DateTime.Now;

        string sqlString = "Select ISNULL(max(modifiedon),'2011-01-01') as tgl from Appointment where PointOfSaleID = " + PointofSaleID.ToString();

        if (IsUpdateServer)
            sqlString += " AND ISNULL(IsServerUpdate,0) = 1";
        else
            sqlString += " AND ISNULL(IsServerUpdate,0) = 0";

        object obj = DataService.ExecuteScalar(new QueryCommand(sqlString));
        if (obj != null)
        {
            DateTime Result;
            if (obj != null && obj.ToString() != "")
                res = (DateTime)obj;
            else
                return false;

            return true;
        }
        return true;
    }

    /*[WebMethod]
    public bool FetchAppointmentsCCMWRealTime(
        string[][] dsHeaders, string[][] dsDetails)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchAppointmentDataRealTime(dsHeaders, dsDetails,null);
    }*/

    [WebMethod]
    public bool FetchAppointmentsCCMWRealTime(
        string[][] dsHeaders, string[][] dsDetails, string[][] dsMembers)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchAppointmentDataRealTime(dsHeaders, dsDetails, dsMembers);
    }

    [WebMethod]
    public bool UpdateAppointmentsIsUpdated(
        string ListId)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.UpdateAppointmentIsUpdated(ListId);
    }

    #region *) Real time sync Delivery Order
    [WebMethod]
    public bool GetDeliveryOrderLastTimeStamp(int PointofSaleID, bool IsUpdateServer, out DateTime res)
    {
        res = DateTime.Now;

        string sqlString = "SELECT ISNULL(max(do.ModifiedOn),'2011-01-01') as tgl FROM DeliveryOrder do INNER JOIN OrderHdr oh ON oh.OrderHdrID = do.SalesOrderRefNo WHERE oh.PointOfSaleID = " + PointofSaleID.ToString();

        if (IsUpdateServer)
            sqlString += " AND ISNULL(do.IsServerUpdate,0) = 1";
        else
            sqlString += " AND ISNULL(do.IsServerUpdate,0) = 0";

        object obj = DataService.ExecuteScalar(new QueryCommand(sqlString, "PowerPOS"));
        if (obj != null)
        {
            if (obj != null && obj.ToString() != "")
                res = (DateTime)obj;
            else
                return false;

            return true;
        }
        return true;
    }

    [WebMethod]
    public bool FetchDeliveryOrderCCMWRealTime(
        string[][] dsHeaders, string[][] dsDetails, string[][] dsMembers, string[][] dsDeposit)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.FetchDeliveryOrderDataRealTime(dsHeaders, dsDetails, dsMembers, dsDeposit);
    }

    [WebMethod]
    public int FetchRecordNoByTimestampDeliveryOrder(DateTime modifiedOn, int PointOfSaleID)
    {
        return SynchronizationController.FetchRecordNoDeliveryOrder(modifiedOn, PointOfSaleID);
    }

    [WebMethod]
    public byte[] FetchDataSetByTimestampDeliveryOrder(DateTime modifiedDate, int noOfRecords, int posID)
    {
        return SynchronizationController.FetchDataSetRealTimeCompressedDeliveryOrder(modifiedDate, noOfRecords, posID);
    }

    [WebMethod]
    public bool UpdateDeliveryOrderIsUpdated(string ListId)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SynchronizationController.UpdateDeliveryOrderIsUpdated(ListId);
    }

    [WebMethod]
    public bool DeleteSavedFileAll(out string status)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SavedFileController.DeleteSavedFileAll(out status);
    }

    [WebMethod]
    public bool DeleteSavedFileByName(string saveName, out string status)
    {
        String remote_caller = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
        //Logger.writeLog("IP Address - Try to sync Orders:" + remote_caller);
        return SavedFileController.DeleteSavedFileByName(saveName, out status);
    }
    #endregion

    #region Refund
    [WebMethod]
    public byte[] getOrderForRefund(string OrderRefNo, int currentPosID, bool AllowRefundForSameOutlet, out string status)
    {
        status = "";
        bool AllowRefundForOtherOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Refund.RefundReceiptFromOtherOutlet), false);
        try
        {
            PointOfSale p = new PointOfSale(currentPosID);
            if (p == null || p.PointOfSaleID == null)
                return null;

            OrderHdrCollection ohCol = new OrderHdrCollection();
            ohCol.Where(OrderHdr.Columns.Userfld5, OrderRefNo);
            ohCol.Load();

            if (ohCol.Count == 0)
            {
                ohCol.Where(OrderHdr.Columns.OrderRefNo, OrderRefNo);
                ohCol.Load();
            }

            if (ohCol.Count > 0)
            {

                if (!AllowRefundForOtherOutlet && !AllowRefundForSameOutlet)
                {
                    if (ohCol[0].PointOfSaleID != currentPosID)
                    {
                        status = "Order is valid, but can only be refunded in " + ohCol[0].PointOfSale.PointOfSaleName;
                        return null;
                    }
                }
                if (!AllowRefundForOtherOutlet && AllowRefundForSameOutlet)
                {
                    if (ohCol[0].PointOfSale.OutletName != p.OutletName)
                    {
                        status = "Receipt " + ohCol[0].OrderRefNo + " is valid, but refund can be done at " + ohCol[0].PointOfSale.OutletName + " outlet";
                        return null;
                    }
                }
                if (ohCol[0].IsVoided)
                {
                    status = "Receipt " + ohCol[0].OrderRefNo + " is voided.";
                    return null;
                }

            }
            else
            {
                status = "Order " + ohCol[0].OrderRefNo + " not found.";
                return null;
            }
            POSController pos = new POSController(ohCol[0].OrderHdrID);
            if (pos != null && pos.myOrderHdr != null && pos.myOrderHdr.OrderHdrID != "")
            {
                #region validation is Refunded
                ArrayList tmpRemove = new ArrayList();
                foreach (OrderDet od in pos.myOrderDet)
                {
                    decimal qtyRefunded = 0;
                    if (POSController.isRefunded(od.OrderDetID, out qtyRefunded) || (od.Quantity < 0 && od.ItemNo != "LINE_DISCOUNT"))
                    {
                        if (qtyRefunded < od.Quantity)
                        {
                            od.Quantity = (od.Quantity - qtyRefunded) * -1;
                            od.ReturnedReceiptNo = "OR" + od.OrderHdrID;
                            od.RefundOrderDetID = od.OrderDetID;
                            od.InventoryHdrRefNo = "";
                            OrderDet tempOD = od;
                            pos.CalculateLineAmount(ref tempOD);
                            status = "Warning. Some items has already refunded.";
                        }
                        else
                        {
                            tmpRemove.Add(od.OrderDetID);
                        }
                    }
                    else
                    {
                        if (!POSController.isRefunded(od.OrderDetID, out qtyRefunded))
                        {
                            od.Quantity = od.Quantity * -1;
                            od.ReturnedReceiptNo = "OR" + od.OrderHdrID;
                            od.RefundOrderDetID = od.OrderDetID;
                            od.InventoryHdrRefNo = "";
                            OrderDet tempOD = od;
                            pos.CalculateLineAmount(ref tempOD);
                        }
                    }
                }
                if (tmpRemove.Count == pos.myOrderDet.Count)
                {
                    status = "Order " + pos.myOrderHdr.OrderRefNo + " has been refunded previously.";
                    return null;
                }
                else
                {
                    if (tmpRemove.Count > 0)
                    {
                        for (int i = 0; i < tmpRemove.Count; i++)
                        {
                            OrderDet od = (OrderDet)pos.myOrderDet.Find(tmpRemove[i].ToString());
                            if (od != null && od.OrderDetID != "")
                                pos.myOrderDet.Remove(od);
                        }
                        status = "Warning. Some items has already refunded.";
                    }
                }
                #endregion

                //res = Newtonsoft.Json.JsonConvert.SerializeObject(pos);
                DataTable dtOrderHdr = ohCol.ToDataTable();
                DataTable dtOrderDet = pos.myOrderDet.ToDataTable();
                DataSet ds = new DataSet();
                ds.Tables.Add(dtOrderHdr);
                ds.Tables.Add(dtOrderDet);
                byte[] data = SyncClientController.CompressDataSetToByteArray(ds);
                return data;
            }
            return null;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex.Message);
            return null;
        }
    }
    #endregion

    #region *) Rating sync

    [WebMethod]
    public bool getRatingLastTimeStamp(int PointofSaleID, out DateTime res)
    {
        res = DateTime.Now;

        string sqlString = "Select ISNULL(max(modifiedon),'2011-01-01') as tgl from rating where POSID = " + PointofSaleID.ToString();


        object obj = DataService.ExecuteScalar(new QueryCommand(sqlString));
        if (obj != null)
        {
            DateTime Result;
            if (obj != null && obj.ToString() != "")
                res = (DateTime)obj;
            else
                return false;

            return true;
        }
        return true;
    }

    [WebMethod]
    public bool SyncRating(String data)
    {
        QueryCommandCollection cmd = new QueryCommandCollection();

        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
        List<RatingClz> ratings = new JavaScriptSerializer().Deserialize<List<RatingClz>>(data);

        try
        {
            foreach (var rating in ratings)
            {
                int count = 0;

                QueryCommand qc = new QueryCommand("SELECT count(*) AS c FROM Rating WHERE UniqueId='" + rating.UniqueId + "'");
                object obj = DataService.ExecuteScalar(qc);
                if (obj != null)
                {
                    try
                    {
                        count = Int32.Parse(obj.ToString());
                    }
                    catch (Exception)
                    {
                        count = 0;
                    }
                }

                if (count <= 0)
                    cmd.Add(new QueryCommand("INSERT INTO rating (POSID, Rating, Staff, Timestamp, CreatedOn, CreatedBy, UniqueID, OrderHdrID, ModifiedOn, ModifiedBy) VALUES (" + rating.POSID + ", " + rating.Rating + ", '" + rating.Staff + "', CONVERT(DATETIME, '" + rating.Timestamp + "', 105), CONVERT(DATETIME, '" + rating.CreatedOn + "', 105), '" + rating.CreatedBy + "', '" + rating.UniqueId + "', '" + rating.OrderHdrID + "', CONVERT(DATETIME, '" + rating.ModifiedOn + "', 105), '" + rating.ModifiedBy + "')"));
            }

            SubSonic.DataService.ExecuteTransaction(cmd);
        }
        catch (Exception e)
        {
            Logger.writeLog(e);
            return false;
        }

        return true;
    }

    public class RatingClz
    {
        public int RatingID { get; set; }
        public int POSID { get; set; }
        public int Rating { get; set; }
        public string Staff { get; set; }
        public string Timestamp { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public string UniqueId { get; set; }
        public string OrderHdrID { get; set; }
    }

    #endregion

    #region *) Real Time Sync for Voucher
    [WebMethod]
    public byte[] FetchDataSetByTimestampVoucherHeader(DateTime modifiedDate, int noOfRecords, string outletName)
    {
        return SynchronizationController.FetchDataSetRealTimeCompressedVoucherHeader(modifiedDate, noOfRecords, outletName);
    }

    [WebMethod]
    public byte[] FetchDataSetByTimestampVouchers(DateTime modifiedDate, int noOfRecords, string outletName)
    {
        return SynchronizationController.FetchDataSetRealTimeCompressedVouchers(modifiedDate, noOfRecords, outletName);
    }
    #endregion

    #region get Order 
    [WebMethod]
    public byte[] getOrder(string OrderRefNo, out string status)
    {
        status = "";
        try
        {
            OrderHdrCollection ohCol = new OrderHdrCollection();
            ohCol.Where(OrderHdr.Columns.OrderHdrID, OrderRefNo);
            ohCol.Load();

            if (ohCol.Count <= 0)
            {
                status = "Order " + ohCol[0].OrderRefNo + " not found.";
                return null;
            }
            POSController pos = new POSController(ohCol[0].OrderHdrID);
            
            if (pos != null && pos.myOrderHdr != null && pos.myOrderHdr.OrderHdrID != "")
            {
                //res = Newtonsoft.Json.JsonConvert.SerializeObject(pos);
                DataTable dtOrderHdr = ohCol.ToDataTable();
                DataTable dtOrderDet = pos.myOrderDet.ToDataTable();

                ReceiptHdrCollection rhCol = new ReceiptHdrCollection();
                rhCol.Where(OrderHdr.Columns.OrderHdrID, pos.myOrderHdr.OrderHdrID);
                rhCol.Load();

                DataTable dtReceiptHdr = rhCol.ToDataTable();
                DataTable dtReceiptDet = pos.recDet.ToDataTable();
                
                DataSet ds = new DataSet();
                ds.Tables.Add(dtOrderHdr);
                ds.Tables.Add(dtOrderDet);
                ds.Tables.Add(dtReceiptHdr);
                ds.Tables.Add(dtReceiptDet);
                
                byte[] data = SyncClientController.CompressDataSetToByteArray(ds);
                return data;
            }
            return null;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex.Message);
            return null;
        }
    }
    #endregion

    #region PreOrder

    [WebMethod]
    public string FetchPreOrderReportFromWeb(DateTime StartDate, DateTime EndDate, string ItemName, string MembershipNo,
            string CustName, string IsOutstanding, string Notified, bool OnlyReadyToDeliver, int InventoryLocationID, string SortBy, string SortDir, string Status)
    {
        return ReportController.FetchPreOrderReportFromWeb(StartDate, EndDate, ItemName, MembershipNo, CustName, IsOutstanding, Notified, OnlyReadyToDeliver,
            InventoryLocationID, SortBy, SortDir, Status);
    }

    [WebMethod]
    public bool CreateDeliveryPreOrderSingleOrderDetForWeb(string orderdetid, int qty, int personnelID,int pointOfSaleID, byte[] signature, out string status)
    {
        return SynchronizationController.CreateDeliveryPreOrderSingleOrderDetForWeb(orderdetid, qty, personnelID, pointOfSaleID, signature, out  status);
    }

    [WebMethod]
    public bool SetDeliveryAsDeliveredStatus(string orderdetid, string username, out string status)
    {
        return PreOrderController.SetDeliveryAsDeliveredStatus(orderdetid, username, out  status);
    }

    [WebMethod]
    public bool CancelPreOrder(string orderHdrID, string orderDetID, string username, out string status)
    {
        return PreOrderController.CancelPreOrder(orderHdrID, orderDetID, username, out status);
    }

    [WebMethod]
    public string DeliveryGetDeliveryTrack(string orderdetid) 
    {
        return DeliveryController.DeliveryGetDeliveryTrack(orderdetid);
    }

    [WebMethod]
    public bool DeliverySetDelivered(string doHdrID, string username, out string status)
    {
        return DeliveryController.DeliverySetDelivered(doHdrID, username, out status);
    }

    [WebMethod]
    public bool SendNotifiyMailDelivery(string orderHdrID, string orderDetID, out string status)
    {
        return SynchronizationController.SendNotifiyMailDelivery(orderHdrID, orderDetID, out status);
    }

    [WebMethod]
    public string FetchDeliveryOrderToPrintByOrderDetID(string orderDetID)
    {
        return DeliveryController.FetchDeliveryOrderToPrintByOrderDetID(orderDetID);
    }

    [WebMethod]
    public bool AutoAssignDepositWhenpayInstallment(string orderHdrID, decimal amount, out string status)
    {
        return SynchronizationController.AutoAssignDepositWhenpayInstallment(orderHdrID, amount, out status);
    }

    [WebMethod]
    public byte[] GetDataOrderDetForRefund(string orderhdrid, string orderdetid, int pointOfSaleID, bool AllowRefundForSameOutlet, out string status)
    {
        return POSController.GetDataOrderDetForRefund(orderhdrid, orderdetid, pointOfSaleID, AllowRefundForSameOutlet, out status);
    }

    [WebMethod]
    public string GetDataOrderDetForRefundStr(string orderhdrid, string orderdetid, int pointOfSaleID, bool AllowRefundForSameOutlet, out string status)
    {
        return POSController.GetDataOrderDetForRefundStr(orderhdrid, orderdetid, pointOfSaleID, AllowRefundForSameOutlet, out status);
    }

    [WebMethod]
    public bool ResetDepositAmount(string orderHdrID, out string status)
    {
        return PreOrderController.ResetDepositAmount(orderHdrID, out status);
    }

    [WebMethod]
    public bool AssignAutoDeposit(string orderHdrID, out string status)
    {
        return PreOrderController.AssignAutoDeposit(orderHdrID, out status);
    }
    #endregion
}
