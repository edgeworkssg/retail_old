using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using System.ComponentModel;

namespace PowerPOS
{
    public partial class ReportController
    {
        public static DataTable FetchMillionIntegrationReport(
            DateTime startDate,
            DateTime endDate,
            string SortColumn,
            string SortDir)
        {
            if (SortColumn == "") { SortColumn = "OH.OrderRefNo"; }
            if (SortDir == "") { SortDir = "ASC"; }

            string sqlString;

            #region string for SQL
            sqlString =
            @"DECLARE @startdate datetime; 
            DECLARE @enddate datetime; 
            SET @startdate = '" + startDate.ToString("dd MMM yyyy HH:mm:ss") + @"' 
            SET @enddate = '" + endDate.ToString("dd MMM yyyy HH:mm:ss") + @"' 

            DECLARE @TempResult AS TABLE
            (
                 accno NVARCHAR(200)
                ,doc_type NVARCHAR(200)
                ,doc_no NVARCHAR(200)
                ,seq INT
                ,doc_date DATETIME
                ,ref_no NVARCHAR(200)
                ,ref_no2 NVARCHAR(200)
                ,desp NVARCHAR(200)
                ,desp2 NVARCHAR(200)
                ,amount MONEY
                ,debit MONEY
                ,credit MONEY
                ,fx_amount MONEY
                ,fx_debit MONEY
                ,fx_credit MONEY
                ,fx_rate MONEY
                ,curr_code NVARCHAR(200)
                ,taxcode NVARCHAR(200)
                ,taxable MONEY
                ,fx_taxable MONEY
                ,link_seq NVARCHAR(200)
                ,billtype NVARCHAR(200)
                ,remark1 NVARCHAR(200)
                ,remark2 NVARCHAR(200)
                ,cheque_no NVARCHAR(200)
                ,projcode NVARCHAR(200)
                ,deptcode NVARCHAR(200)
                ,accmgr_id NVARCHAR(200)
                ,batchno NVARCHAR(200)										
            )

            INSERT INTO @TempResult
            SELECT   '5000/0000' accno
                    ,'GL' doc_type
                    ,'' doc_no
                    ,1 seq
                    ,OH.OrderDate doc_date
                    ,OH.OrderRefNo ref_no 
                    ,'' ref_no2
                    ,'cash sales' desp
                    ,'' desp2
                    ,-1 * CASE WHEN OH.GSTAmount IS NOT NULL THEN OH.NettAmount-OH.GSTAmount ELSE OH.NettAmount END amount
                    ,0 debit
                    ,CASE WHEN OH.GSTAmount IS NOT NULL THEN OH.NettAmount-OH.GSTAmount ELSE OH.NettAmount END credit
                    ,-1 * CASE WHEN OH.GSTAmount IS NOT NULL THEN OH.NettAmount-OH.GSTAmount ELSE OH.NettAmount END fx_amount
                    ,0 fx_debit
                    ,CASE WHEN OH.GSTAmount IS NOT NULL THEN OH.NettAmount-OH.GSTAmount ELSE OH.NettAmount END fx_credit 
                    ,1 fx_rate
                    ,'SGD' curr_code
                    ,'' taxcode
                    ,0 taxable
                    ,0 fx_taxable
                    ,'0' link_seq
                    ,'P' billtype
                    ,'' remark1	
                    ,'' remark2	
                    ,'' cheque_no	
                    ,'' projcode	
                    ,'' deptcode	
                    ,'' accmgr_id
                    ,'' batchno
            FROM	OrderHdr OH 
            WHERE	OH.IsVoided <> 1 AND OH.OrderDate BETWEEN @startdate AND @enddate
            ORDER BY OH.OrderRefNo  ASC

            INSERT INTO @TempResult
            SELECT   '4050/0001' accno
                    ,'GL' doc_type
                    ,'' doc_no
                    ,2 seq
                    ,OH.OrderDate doc_date
                    ,OH.OrderRefNo ref_no 
                    ,'' ref_no2
                    ,'gst' desp
                    ,'' desp2
                    ,-1 * ISNULL(OH.GSTAmount,0) amount
                    ,0 debit
                    ,ISNULL(OH.GSTAmount,0) credit
                    ,-1 * ISNULL(OH.GSTAmount,0) fx_amount
                    ,0 fx_debit
                    ,ISNULL(OH.GSTAmount,0) fx_credit 
                    ,1 fx_rate
                    ,'SGD' curr_code
                    ,'SRI' taxcode
                    ,-1 * CASE WHEN OH.GSTAmount IS NOT NULL THEN OH.NettAmount-OH.GSTAmount ELSE OH.NettAmount END taxable
                    ,0 fx_taxable
                    ,'1' link_seq
                    ,'P' billtype
                    ,'' remark1	
                    ,'' remark2	
                    ,'' cheque_no	
                    ,'' projcode	
                    ,'' deptcode	
                    ,'' accmgr_id
                    ,'' batchno
            FROM	OrderHdr OH 
            WHERE	OH.IsVoided <> 1 AND OH.OrderDate BETWEEN @startdate AND @enddate
		            AND ISNULL(OH.GSTAmount,0) > 0
            ORDER BY OH.OrderRefNo  ASC

            INSERT INTO @TempResult
            SELECT   '3020/0001' accno
                    ,'GL' doc_type
                    ,'' doc_no
                    ,3 seq
                    ,OH.OrderDate doc_date
                    ,OH.OrderRefNo ref_no 
                    ,'' ref_no2
                    ,'total' desp
                    ,'' desp2
                    ,OH.NettAmount amount
                    ,OH.NettAmount debit
                    ,0 credit
                    ,OH.NettAmount fx_amount
                    ,OH.NettAmount fx_debit
                    ,0 fx_credit 
                    ,1 fx_rate
                    ,'SGD' curr_code
                    ,'' taxcode
                    ,0 taxable
                    ,0 fx_taxable
                    ,'0' link_seq
                    ,'P' billtype
                    ,'' remark1	
                    ,'' remark2	
                    ,'' cheque_no	
                    ,'' projcode	
                    ,'' deptcode	
                    ,'' accmgr_id
                    ,'' batchno
            FROM	OrderHdr OH 
            WHERE	OH.IsVoided <> 1 AND OH.OrderDate BETWEEN @startdate AND @enddate
            ORDER BY OH.OrderRefNo  ASC

            SELECT 
                 accno
                ,doc_type
                ,doc_no
                , ROW_NUMBER() OVER(PARTITION BY ref_no ORDER BY seq) AS seq 
                ,doc_date 
                ,ref_no
                ,ref_no2
                ,desp
                ,desp2 
                ,amount 
                ,debit 
                ,credit 
                ,fx_amount 
                ,fx_debit 
                ,fx_credit 
                ,fx_rate 
                ,curr_code 
                ,taxcode 
                ,taxable 
                ,fx_taxable 
                ,link_seq 
                ,billtype
                ,remark1
                ,remark2
                ,cheque_no 
                ,projcode
                ,deptcode
                ,accmgr_id
                ,batchno
            FROM @TempResult
            ORDER BY ref_no, seq";
            #endregion

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sqlString)));
            return dt;
        }

        public static DataTable FetchInventoryActivityHeader
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string UserName, string InventoryLocationID, string movementType, string remark,
            string SortColumn, string SortDir, bool showGoodsReceive)
        {
            return FetchInventoryActivityHeaderWithRefNo
            (useStartDate, useEndDate, StartDate, EndDate,
             UserName, InventoryLocationID, movementType, remark, "",
             SortColumn, SortDir, showGoodsReceive);
        }

        public static DataTable FetchInventoryActivityHeaderWithRefNo
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string UserName, string InventoryLocationID, string movementType, string remark, string refNo,
            string SortColumn, string SortDir, bool showGoodsReceive)
        {
            string sqlString = 
                "SELECT InventoryHdr.InventoryHdrRefNo, InventoryHdr.InventoryDate, InventoryHdr.UserName, InventoryHdr.MovementType, "+
                    "InventoryHdr.StockOutReasonID, InventoryHdr.ExchangeRate, InventoryHdr.PurchaseOrderNo, InventoryHdr.InvoiceNo, "+
                    "InventoryHdr.DeliveryOrderNo, InventoryHdr.Supplier, InventoryHdr.FreightCharge, InventoryHdr.DeliveryCharge, InventoryHdr.Tax, "+
                    "InventoryHdr.Discount, InventoryHdr.Remark, InventoryHdr.InventoryLocationID, InventoryHdr.DepartmentID, InventoryHdr.TmpSavedData, "+
                    "InventoryHdr.CreatedOn, InventoryHdr.ModifiedOn, InventoryHdr.CreatedBy, InventoryHdr.ModifiedBy, InventoryHdr.UniqueID, "+
                    "InventoryHdr.Deleted, InventoryHdr.userfld1, InventoryHdr.userfld2, InventoryHdr.userfld3, InventoryHdr.userfld4, InventoryHdr.userfld5, "+
                    "InventoryHdr.userfld6, InventoryHdr.userfld7, InventoryHdr.userfld8, InventoryHdr.userfld9, InventoryHdr.userfld10, InventoryHdr.userflag1, "+
                    "InventoryHdr.userflag2, InventoryHdr.userflag3, InventoryHdr.userflag4, InventoryHdr.userflag5, InventoryHdr.userfloat1, "+
                    "InventoryHdr.userfloat2, InventoryHdr.userfloat3, InventoryHdr.userfloat4, InventoryHdr.userfloat5, InventoryHdr.userint1, "+
                    "InventoryHdr.userint2, InventoryHdr.userint3, InventoryHdr.userint4, InventoryHdr.userint5, InventoryLocation.InventoryLocationName, "+
                    "InventoryStockOutReason.ReasonName, ISNULL(LT.FromInventoryHdrRefNo,'') as TransferOutRef, " +
                    "ISNULL(TransOut.ToInventoryHdrRefNo,'') as TransferInRef, " +
                    "ISNULL(LocOut.InventoryLocationName, ISNULL(LocIn.InventoryLocationName,'')) as TransferToFrom " +
                "FROM InventoryHdr INNER JOIN "+
                    "InventoryLocation ON InventoryHdr.InventoryLocationID = InventoryLocation.InventoryLocationID LEFT OUTER JOIN "+
                    "InventoryStockOutReason ON InventoryHdr.StockOutReasonID = InventoryStockOutReason.ReasonID "+
                    "LEFT JOIN LocationTransfer LT ON InventoryHdr.InventoryHdrRefNo = LT.ToInventoryHdrRefNo " +
                    "LEFT JOIN InventoryLocation LocOut ON LT.FromInventoryLocationID = LocOut.InventoryLocationID " +
                    "LEFT JOIN LocationTransfer TransOut ON InventoryHdr.InventoryHdrRefNo = TransOut.FromInventoryHdrRefNo " +
                    "LEFT JOIN InventoryLocation LocIn ON TransOut.ToInventoryLocationID = LocIn.InventoryLocationID " +
                "WHERE ((InventoryHdr.StockOutReasonID IS NULL) OR " +
                    "(InventoryHdr.StockOutReasonID <> 0) OR "+
                    "(InventoryHdr.MovementType <> 'Stock Out')) ";
        
            if (useStartDate & useEndDate)
            {
                sqlString += "AND (InventoryHdr.InventoryDate BETWEEN '" + StartDate.ToString("dd MMM yyyy HH:mm:ss") + "' AND '" + EndDate.ToString("dd MMM yyyy HH:mm:ss") + "') ";
            }
            else if (useStartDate)
            {
                sqlString += "AND InventoryHdr.InventoryDate >= '" + StartDate.ToString("dd MMM yyyy HH:mm:ss") + "' ";
            }
            else if (useEndDate)
            {
                sqlString += "AND InventoryHdr.InventoryDate <= '" + EndDate.ToString("dd MMM yyyy HH:mm:ss") + "' ";
            }

            if (UserName != "")
            {
                sqlString += "AND InventoryHdr.UserName LIKE '%" + UserName + "%' ";
            }

            if (movementType != "" && movementType != "ALL")
            {
                sqlString += "AND InventoryHdr.MovementType = '" + movementType + "' ";
            }

            if (InventoryLocationID != "" && InventoryLocationID != "0")
            {
                sqlString += "AND InventoryHdr.InventoryLocationID = " + InventoryLocationID + " ";
            }
            if (remark != "")
            {
                sqlString += "AND remark like '%" + remark + "%'";
            }

            if (!string.IsNullOrEmpty(refNo))
            {
                sqlString += "AND ISNULL(InventoryHdr.InventoryHdrRefNo, '') + ISNULL(InventoryHdr.Userfld5, '')  like '%" + refNo + "%'";
            }
            //sqlString += "AND InventoryHdr.MovementType <> '" + InventoryController.InventoryMovementType_TransferIn + "' ";

            sqlString += "AND InventoryHdr.Remark NOT LIKE 'Stock Take Adj.%' ";

            if (!showGoodsReceive)
            {
                sqlString += "AND InventoryHdr.MovementType <> '" + InventoryController.InventoryMovementType_StockIn + "' ";
            }

            if (SortColumn != null && SortColumn != "")
            {
                if (SortDir.Trim() == "ASC")
                {
                    sqlString += "ORDER BY " + SortColumn + " ASC ";
                }
                else if (SortDir.Trim() == "DESC")
                {
                    sqlString += "ORDER BY " + SortColumn + " DESC ";
                }
                else
                {
                    sqlString += "ORDER BY InventoryHdr.InventoryDate DESC ";
                }
            }
            else
            {
                sqlString += "ORDER BY InventoryHdr.InventoryDate DESC ";
            }

            DataTable Dt = new DataTable();
            //Dt.Load(DataService.GetReader(new QueryCommand(sqlString)));
            

            return DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];

        }

        public static DataTable FetchStockCardReport
            (DateTime StartDate, DateTime EndDate, int LocationID,
            string Search)
        {
            string status;
            decimal BalBefore, BalAfter;
            decimal StockIn, StockOut, TransferIn, TransferOut, AdjustmentIn, AdjustmentOut;

            InventoryLocation loc = new InventoryLocation(LocationID);
            string location = loc.InventoryLocationName;
            DataTable dt = new DataTable();

            dt.Columns.Add("DepartmentName");
            dt.Columns.Add("CategoryName");
            dt.Columns.Add("ItemNo");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("BalanceBefore", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("StockIn", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("StockOut", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("TransferIn", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("TransferOut", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("AdjustmentIn", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("AdjustmentOut", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("BalanceAfter", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("InventoryLocationName");

            //
            //Fetch balance quantity for this item.
            ViewItemCollection it = new ViewItemCollection();
            it.Where(ViewItem.Columns.IsInInventory, true);
            it.Where(ViewItem.Columns.Deleted, false);
            if (Search != "")
            {
                Search = "%" + Search + "%";
                it.Where(ViewItem.Columns.Search, Comparison.Like, Search);
            }
            /*
            if (CategoryName != "")
            {
                CategoryName = "%" + CategoryName + "%";
                it.Where(Item.Columns.CategoryName, Comparison.Like, CategoryName);
            }*/
            it.Load();
            DataRow dr;
            for (int i = 0; i < it.Count; i++)
            {

                dr = dt.NewRow();
                dr["DepartmentName"] = it[i].DepartmentName;
                dr["CategoryName"] = it[i].CategoryName;
                dr["ItemNo"] = it[i].ItemNo;
                dr["ItemName"] = it[i].ItemName;

                BalBefore =
                    InventoryController.
                    GetStockBalanceQtyByItemByDate(it[i].ItemNo, LocationID, StartDate, out status);
                dr["BalanceBefore"] = BalBefore;


                StockIn = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                    (StartDate, EndDate, LocationID,
                    it[i].ItemNo, InventoryController.InventoryMovementType_StockIn);
                dr["StockIn"] = StockIn;

                StockOut = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                    (StartDate, EndDate, LocationID,
                    it[i].ItemNo, InventoryController.InventoryMovementType_StockOut);
                dr["StockOut"] = StockOut;

                TransferIn = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                    (StartDate, EndDate, LocationID,
                    it[i].ItemNo, InventoryController.InventoryMovementType_TransferIn);
                dr["TransferIn"] = TransferIn;

                TransferOut = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                    (StartDate, EndDate, LocationID,
                    it[i].ItemNo, InventoryController.InventoryMovementType_TransferOut);
                dr["TransferOut"] = TransferOut;

                AdjustmentIn = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                            (StartDate, EndDate, LocationID,
                            it[i].ItemNo, InventoryController.InventoryMovementType_AdjustmentIn);
                dr["AdjustmentIn"] = AdjustmentIn;

                AdjustmentOut = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                                (StartDate, EndDate, LocationID,
                                it[i].ItemNo, InventoryController.InventoryMovementType_AdjustmentOut);
                dr["AdjustmentOut"] = AdjustmentOut;

                BalAfter =
                    InventoryController.
                    GetStockBalanceQtyByItemByDate(it[i].ItemNo, LocationID, EndDate, out status);
                dr["BalanceAfter"] = BalAfter;
                dr["InventoryLocationName"] = location;
                dt.Rows.Add(dr);
            }

            return dt;
        }
        public static DataTable FetchStockCardReport
            (DateTime StartDate, DateTime EndDate, int LocationID,
            string ItemName, string CategoryName)
        {
            string status;
            decimal BalBefore, BalAfter;
            decimal StockIn, StockOut, TransferIn, TransferOut, AdjustmentIn, AdjustmentOut, ReturnOut, CostPrice; ;

            InventoryLocation loc = new InventoryLocation(LocationID);
            string location = loc.InventoryLocationName;
            DataTable dt = new DataTable();

            dt.Columns.Add("DepartmentName");
            dt.Columns.Add("CategoryName");
            dt.Columns.Add("ItemNo");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Attributes1");
            dt.Columns.Add("BalanceBefore", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("StockIn", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("StockOut", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("TransferIn", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("TransferOut", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("AdjustmentIn", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("AdjustmentOut", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("ReturnOut", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("BalanceAfter", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("InventoryLocationName");
            dt.Columns.Add("UOM");
            ItemName = "%" + ItemName + "%";
            CategoryName = "%" + CategoryName + "%";
            //
            //Fetch balance quantity for this item.
            ViewItemCollection it = new ViewItemCollection();
            /*it.Where(Item.Columns.IsInInventory, true);
            it.Where(Item.Columns.Deleted, false);
            it.Where(Item.Columns.ItemName, Comparison.Like, ItemName);
            it.Where(Item.Columns.CategoryName, Comparison.Like, CategoryName);
            it.Load();*/
            string sqlString = "Select v.* From ViewItem V " +
            "LEFT JOIN (SELECT ItemNo, altBarcode = STUFF((SELECT N', ' + Barcode " +
                "FROM dbo.AlternateBarcode AS p2 " +
                   "WHERE p2.ItemNo = p.ItemNo " +
                   "ORDER BY Barcode " +
                   " FOR XML PATH(N'')), 1, 2, N'') " +
                " FROM dbo.AlternateBarcode AS p " +
                "GROUP BY ItemNo ) ALT ON V.ItemNo = ALT.ItemNo " +
            "WHERE V.IsInInventory = 1 and V.Deleted = 0 " +
            "AND V.ItemNo + V.ItemName + ISNULL(ALT.altBarcode,'') + isnull(V.Barcode,'') like '" + ItemName + "' " +
            "AND V.CategoryName like '" + CategoryName + "'";
            DataTable dtItem = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
            it.Load(dtItem);
            DataRow dr;
            for (int i = 0; i < it.Count; i++)
            {

                dr = dt.NewRow();
                dr["DepartmentName"] = it[i].DepartmentName;
                dr["CategoryName"] = it[i].CategoryName;
                dr["ItemNo"] = it[i].ItemNo;
                dr["ItemName"] = it[i].ItemName.Replace("\"", "");
                dr["Attributes1"] = it[i].Attributes1;

                BalBefore =
                    InventoryController.
                    GetStockBalanceQtyByItemByDate(it[i].ItemNo, LocationID, StartDate, out status);
                dr["BalanceBefore"] = BalBefore;


                StockIn = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                    (StartDate, EndDate, LocationID,
                    it[i].ItemNo, InventoryController.InventoryMovementType_StockIn);
                dr["StockIn"] = StockIn;

                StockOut = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                    (StartDate, EndDate, LocationID,
                    it[i].ItemNo, InventoryController.InventoryMovementType_StockOut);
                dr["StockOut"] = StockOut;

                TransferIn = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                    (StartDate, EndDate, LocationID,
                    it[i].ItemNo, InventoryController.InventoryMovementType_TransferIn);
                dr["TransferIn"] = TransferIn;

                TransferOut = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                    (StartDate, EndDate, LocationID,
                    it[i].ItemNo, InventoryController.InventoryMovementType_TransferOut);
                dr["TransferOut"] = TransferOut;

                AdjustmentIn = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                            (StartDate, EndDate, LocationID,
                            it[i].ItemNo, InventoryController.InventoryMovementType_AdjustmentIn);
                dr["AdjustmentIn"] = AdjustmentIn;

                AdjustmentOut = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                                (StartDate, EndDate, LocationID,
                                it[i].ItemNo, InventoryController.InventoryMovementType_AdjustmentOut);
                dr["AdjustmentOut"] = AdjustmentOut;

                ReturnOut = InventoryController.GetTotalQtyByItemNoAndMovementTypeAndLocationID
                               (StartDate, EndDate, LocationID,
                               it[i].ItemNo, InventoryController.InventoryMovementType_ReturnOut);
                dr["ReturnOut"] = ReturnOut;

                CostPrice = InventoryController.GetLatestCostPriceByItemNoAndLocationID(StartDate, EndDate, LocationID, it[i].ItemNo);
                dr["CostPrice"] = CostPrice;


                BalAfter =
                    InventoryController.
                    GetStockBalanceQtyByItemByDate(it[i].ItemNo, LocationID, EndDate, out status);
                dr["BalanceAfter"] = BalAfter;
                dr["InventoryLocationName"] = location;
                Item tmpItem = new Item(it[i].ItemNo);
                dr["UOM"] = tmpItem.UOM;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static DataTable FetchStockCardReportWithStockOutDetails
            (DateTime StartDate, DateTime EndDate, int InventoryLocationID,
            bool displayCostPrice, string searchQuery, string SortColumn, string SortDir)
        {
            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

            StartDate = StartDate.Date;
            EndDate = EndDate.Date.AddDays(1);

            if (CostingMethod == InventoryController.CostingTypes.FIFO)
                return FetchStockCardReportWithStockOutDetails_FIFO
                    (StartDate, EndDate, InventoryLocationID, displayCostPrice, searchQuery, SortColumn, SortDir);
            else if (CostingMethod == InventoryController.CostingTypes.FixedAvg)
                return FetchStockCardReportWithStockOutDetails_FixedAvg
                    (StartDate, EndDate, InventoryLocationID, displayCostPrice, searchQuery, SortColumn, SortDir);
            else
                return FetchStockCardReportWithStockOutDetails_FIFO
                    (StartDate, EndDate, InventoryLocationID, displayCostPrice, searchQuery, SortColumn, SortDir);
        }
        public static DataTable FetchStockCardReportWithStockOutDetails_FIFO
            (DateTime StartDate, DateTime EndDate, int InventoryLocationID, 
            bool displayCostPrice, string searchQuery, string SortColumn, string SortDir)
        {
            //string DeptID = "";
            if (searchQuery == "") { searchQuery = "%"; }
            //if (InventoryLocationName == "") { InventoryLocationName = "ALL"; }
            //if (DeptID == "0") { DeptID = ""; }
            //Query qr = ViewItem.CreateQuery();
            //qr.QueryType = QueryType.Select;
            //qr.AddWhere(ViewItem.Columns.Deleted, false);
            //qr.AddWhere(ViewItem.Columns.IsInInventory, true);
            string tmp = "%" + searchQuery + "%";
            string searchSQL = " AND c.ItemNo+ItemName+c.Barcode+ISNULL(attributes1,'')+ISNULL(attributes2,'')+ISNULL(attributes3,'')+ " +
                         "ISNULL(attributes4,'')+ISNULL(attributes5,'')+ISNULL(attributes6,'')+ISNULL(attributes7,'')+ISNULL(attributes8,'')+ " +
                         "ISNULL(ItemDesc,'')+ISNULL(departmentname,'')+ISNULL(D.CategoryName,'') like '%' + @search + '%' ";
            if (searchQuery == "") searchSQL = "";


            if (SortColumn == "") SortColumn = "ItemNo";
            if (SortDir == "") SortDir = "ASC";
            string InventoryLocationName = new InventoryLocation(InventoryLocationID).InventoryLocationName;

            //Get inventoryLocationID from InventoryLocationName

            string SQL = "declare @search as nvarchar(200); " +
                        "declare @inventorylocationName as nvarchar(200); " +
                        "set @search = '" + searchQuery + "' " +
                        "set @inventorylocationName = '" + InventoryLocationName + "' " +
                        "select f.departmentname as Department ,c.categoryname as Category, c.itemno as ItemNo, c.itemname as [Item Name],c.factoryprice as [Cost Price],isnull(a.movementtype,'Stock In') + (CASE WHEN Movementtype = 'Stock Out' AND Not a.StockOutReasonID is null THEN '(' + y.ReasonName + ')' ELSE '' END) as movementtype, SUM(isnull(quantity,0)) as Quantity  " +
                        "from inventoryhdr a  " +
                        "inner join inventorydet b " +
                        "on a.inventoryhdrrefno = b.inventoryhdrrefno " +
                        "right outer join Item c  " +
                        "on c.ItemNo = b.itemno  " +
                        "inner join Category d on " +
                        "d.categoryname = c.CategoryName " +
                        "inner join InventoryLocation e  " +
                        "on a.InventoryLocationID = e.inventorylocationid " +
                        "inner join itemdepartment f on d.itemdepartmentid = f.itemdepartmentid " +
                        "left outer join InventoryStockOutReason y on y.ReasonID = a.StockOutReasonID " + 
                        "where  inventorydate >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + " and inventorydate <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and " +
                        "(c.deleted=0 or c.deleted is null) and InventoryLocationName like '%' + @inventorylocationName + '%' " + searchSQL +
                        "group by f.departmentname,c.itemno,c.categoryname, c.itemname,c.factoryprice,isnull(a.movementtype,'Stock In') + (CASE WHEN Movementtype = 'Stock Out' AND Not a.StockOutReasonID is null THEN '(' + y.ReasonName + ')' ELSE '' END)  " +
                        "order by " + SortColumn + " " + SortDir;

            DataTable stockBal = Pivot(DataService.GetReader(new QueryCommand(SQL, "PowerPOS")), "ItemNo", "MovementType", "Quantity");
            
            if (!stockBal.Columns.Contains("Stock In")) stockBal.Columns.Add("Stock In", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Stock Out(Sales)")) stockBal.Columns.Add("Stock Out(Sales)", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Transfer In")) stockBal.Columns.Add("Transfer In", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Transfer Out")) stockBal.Columns.Add("Transfer Out", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Adjustment In")) stockBal.Columns.Add("Adjustment In", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Adjustment Out")) stockBal.Columns.Add("Adjustment Out", System.Type.GetType("System.Int32"));
            
            
            //Rearrange the ordinals
            stockBal.Columns["Stock In"].SetOrdinal(4);
            if (stockBal.Columns.Contains("Stock Out(Sales)")) stockBal.Columns["Stock Out(Sales)"].SetOrdinal(5);
            stockBal.Columns["Transfer In"].SetOrdinal(stockBal.Columns.Count - 1);
            stockBal.Columns["Transfer Out"].SetOrdinal(stockBal.Columns.Count - 1);
            stockBal.Columns["Adjustment In"].SetOrdinal(stockBal.Columns.Count - 1);
            stockBal.Columns["Adjustment Out"].SetOrdinal(stockBal.Columns.Count - 1);

            DataTable openingBal = FetchStockReportByDate(searchQuery, InventoryLocationID, displayCostPrice, StartDate, "", SortColumn, SortDir);
            DataTable closingBal = FetchStockReportByDate(searchQuery, InventoryLocationID, displayCostPrice, EndDate, "", SortColumn, SortDir);
            stockBal.Columns.Add("Opening Balance", System.Type.GetType("System.Int32"));
            stockBal.Columns.Add("Closing Balance", System.Type.GetType("System.Int32"));
            stockBal.Columns["Opening Balance"].SetOrdinal(4);
            stockBal.Columns["Closing Balance"].SetOrdinal(stockBal.Columns.Count - 1);
            DataTable undeductedSales = ReportController.FetchUnDeductedSales(InventoryLocationID);
            
            //stockBal.Columns.Add("Cost Price", System.Type.GetType("System.Decimal"));
            stockBal.Columns["Cost Price"].SetOrdinal(4);
            for (int i = 0; i < stockBal.Rows.Count; i++)
            {
                //manually select from the result table....
                int openingQty, closingQty, undeductedQty;
                DataRow[] dr;
                dr = openingBal.Select("itemno = '" + stockBal.Rows[i]["ItemNo"] + "'");
                if (dr != null && dr.Length > 0)
                {
                    openingQty = (int)dr[0]["OnHand"];
                }
                else
                {
                    openingQty = 0;
                }
                dr = closingBal.Select("itemno = '" + stockBal.Rows[i]["ItemNo"] + "'");
                if (dr != null && dr.Length > 0)
                {
                    closingQty = (int)dr[0]["OnHand"];
                }
                else
                {
                    closingQty = 0;
                }
                dr = undeductedSales.Select("itemno = '" + stockBal.Rows[i]["ItemNo"] + "'");
                if (dr != null && dr.Length > 0)
                {
                    undeductedQty = (int)dr[0]["Quantity"];
                }
                else
                {
                    undeductedQty = 0;
                }
                decimal cogs = 0.0M;
                /*
                if (displayCostPrice && stockBal.Rows[i]["Closing Balance"] is int)
                {
                    
                    int onHandQty = (int)stockBal.Rows[i]["Closing Balance"];
                    cogs =
                        InventoryController.FetchAverageCostOfGoodsLeftByItemNo
                        (onHandQty, stockBal.Rows[i]["ItemNo"].ToString(), InventoryLocationID);
                      

                }
                 */ 
                //stockBal.Rows[i]["Cost Of Goods"] = cogs;
                
                stockBal.Rows[i]["Opening Balance"] = openingQty;
                stockBal.Rows[i]["Closing Balance"] = closingQty;
                int cursales = 0;
                int.TryParse(stockBal.Rows[i]["Stock Out(Sales)"].ToString(), out cursales);
                stockBal.Rows[i]["Stock Out(Sales)"] = cursales + undeductedQty;
            }

            /*
            stockBal.Columns.Add("OnHand", System.Type.GetType("System.Int32"));
            stockBal.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
            stockBal.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));

            for (int j = 0; j < stockBal.Rows.Count; j++)
            {
                int stockin = 0, stockout = 0, transferin = 0, transferout = 0, adjustmentin = 0, adjustmentout = 0;
                int.TryParse(stockBal.Rows[j]["Stock In"].ToString(), out stockin);
                //int.TryParse(stockBal.Rows[j]["Stock Out"].ToString(), out stockout);
                int.TryParse(stockBal.Rows[j]["Transfer In"].ToString(), out transferin);
                int.TryParse(stockBal.Rows[j]["Transfer Out"].ToString(), out transferout);
                int.TryParse(stockBal.Rows[j]["Adjustment In"].ToString(), out adjustmentin);
                int.TryParse(stockBal.Rows[j]["Adjustment Out"].ToString(), out adjustmentout);
                int negativeSales = InventoryController.
                    GetTotalUndeductedSalesByItem(stockBal.Rows[j]["ItemNo"].ToString(),
                    InventoryLocationID);


                int onHandQty =
                    stockin - stockout + transferin -
                    transferout + adjustmentin - adjustmentout - negativeSales;
                stockBal.Rows[j]["OnHand"] = onHandQty;
                decimal cogs = 0.0M;
                if (displayCostPrice)
                {
                    cogs =
                        InventoryController.FetchAverageCostOfGoodsLeftByItemNo
                        (onHandQty, stockBal.Rows[j]["ItemNo"].ToString(), InventoryLocationID);

                }
                stockBal.Rows[j]["CostOfGoods"] = cogs;
                stockBal.Rows[j]["TotalCost"] = cogs * onHandQty;
            }

            */
            return stockBal;
        }
        public static DataTable FetchStockCardReportWithStockOutDetails_FixedAvg
            (DateTime StartDate, DateTime EndDate, int InventoryLocationID,
            bool displayCostPrice, string searchQuery, string SortColumn, string SortDir)
        {
            //string DeptID = "";
            if (searchQuery == "") { searchQuery = "%"; }
            //if (InventoryLocationName == "") { InventoryLocationName = "ALL"; }
            //if (DeptID == "0") { DeptID = ""; }
            //Query qr = ViewItem.CreateQuery();
            //qr.QueryType = QueryType.Select;
            //qr.AddWhere(ViewItem.Columns.Deleted, false);
            //qr.AddWhere(ViewItem.Columns.IsInInventory, true);
            string tmp = "%" + searchQuery + "%";
            string searchSQL = " AND c.ItemNo+ItemName+c.Barcode+ISNULL(attributes1,'')+ISNULL(attributes2,'')+ISNULL(attributes3,'')+ " +
                         "ISNULL(attributes4,'')+ISNULL(attributes5,'')+ISNULL(attributes6,'')+ISNULL(attributes7,'')+ISNULL(attributes8,'')+ " +
                         "ISNULL(ItemDesc,'')+ISNULL(departmentname,'') like '%' + @search + '%' ";
            if (searchQuery == "") searchSQL = "";


            if (SortColumn == "") SortColumn = "ItemNo";
            if (SortDir == "") SortDir = "ASC";
            string InventoryLocationName = new InventoryLocation(InventoryLocationID).InventoryLocationName;

            //Get inventoryLocationID from InventoryLocationName

            string SQL = "declare @search as nvarchar(200); " +
                        "declare @inventorylocationName as nvarchar(200); " +
                        "set @search = '" + searchQuery + "' " +
                        "set @inventorylocationName = '" + InventoryLocationName + "' " +
                        "select f.departmentname as Department " +
                        "   , c.categoryname as Category " +
                        "   , c.itemno as ItemNo " +
                        "   , c.itemname as [Item Name] " +
                        "   , c.Attributes1 " +
                        "   , case when @inventorylocationName = '' then c.AvgCostPrice else s.CostPrice end as [Cost Price] " +
                        "   , isnull(a.movementtype,'Stock In') + (CASE WHEN Movementtype = 'Stock Out' AND Not a.StockOutReasonID is null THEN '(' + y.ReasonName + ')' ELSE '' END) as movementtype " +
                        "   , SUM(isnull(quantity,0)) as Quantity  " +
                        "from inventoryhdr a  " +
                        "inner join inventorydet b " +
                        "on a.inventoryhdrrefno = b.inventoryhdrrefno " +
                        "right outer join Item c  " +
                        "on c.ItemNo = b.itemno  " +
                        "inner join Category d on " +
                        "d.categoryname = c.CategoryName " +
                        "inner join InventoryLocation e  " +
                        "on a.InventoryLocationID = e.inventorylocationid " +
                        "inner join itemdepartment f on d.itemdepartmentid = f.itemdepartmentid " +
                        "left outer join InventoryStockOutReason y on y.ReasonID = a.StockOutReasonID " +
                        "left outer join ItemSummary s on s.ItemNo = b.ItemNo and s.InventoryLocationID = a.InventoryLocationID " +
                        "where  inventorydate >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + " and inventorydate <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and " +
                        "(c.deleted=0 or c.deleted is null) and InventoryLocationName like '%' + @inventorylocationName + '%' " + searchSQL +
                        "group by f.departmentname,c.itemno,c.categoryname, c.itemname, c.Attributes1,c.AvgCostPrice,s.CostPrice,isnull(a.movementtype,'Stock In') + (CASE WHEN Movementtype = 'Stock Out' AND Not a.StockOutReasonID is null THEN '(' + y.ReasonName + ')' ELSE '' END)  " +
                        "order by " + SortColumn + " " + SortDir;

            DataTable stockBal = Pivot(DataService.GetReader(new QueryCommand(SQL, "PowerPOS")), "ItemNo", "MovementType", "Quantity");

            if (!stockBal.Columns.Contains("Stock In")) stockBal.Columns.Add("Stock In", System.Type.GetType("System.Decimal"));
            if (!stockBal.Columns.Contains("Stock Out(Sales)")) stockBal.Columns.Add("Stock Out(Sales)", System.Type.GetType("System.Decimal"));
            if (!stockBal.Columns.Contains("Transfer In")) stockBal.Columns.Add("Transfer In", System.Type.GetType("System.Decimal"));
            if (!stockBal.Columns.Contains("Transfer Out")) stockBal.Columns.Add("Transfer Out", System.Type.GetType("System.Decimal"));
            if (!stockBal.Columns.Contains("Adjustment In")) stockBal.Columns.Add("Adjustment In", System.Type.GetType("System.Decimal"));
            if (!stockBal.Columns.Contains("Adjustment Out")) stockBal.Columns.Add("Adjustment Out", System.Type.GetType("System.Decimal"));


            //Rearrange the ordinals
            stockBal.Columns["Stock In"].SetOrdinal(5);
            if (stockBal.Columns.Contains("Stock Out(Sales)")) stockBal.Columns["Stock Out(Sales)"].SetOrdinal(6);
            stockBal.Columns["Transfer In"].SetOrdinal(stockBal.Columns.Count - 1);
            stockBal.Columns["Transfer Out"].SetOrdinal(stockBal.Columns.Count - 1);
            stockBal.Columns["Adjustment In"].SetOrdinal(stockBal.Columns.Count - 1);
            stockBal.Columns["Adjustment Out"].SetOrdinal(stockBal.Columns.Count - 1);

            DataTable openingBal = FetchStockReportByDate(searchQuery, InventoryLocationID, displayCostPrice, StartDate, "", SortColumn, SortDir);
            DataTable closingBal = FetchStockReportByDate(searchQuery, InventoryLocationID, displayCostPrice, EndDate, "", SortColumn, SortDir);
            stockBal.Columns.Add("Opening Balance", System.Type.GetType("System.Decimal"));
            stockBal.Columns.Add("Closing Balance", System.Type.GetType("System.Decimal"));
            stockBal.Columns["Opening Balance"].SetOrdinal(5);
            stockBal.Columns["Closing Balance"].SetOrdinal(stockBal.Columns.Count - 1);
            DataTable undeductedSales = ReportController.FetchUnDeductedSales(InventoryLocationID);

            //stockBal.Columns.Add("Cost Price", System.Type.GetType("System.Decimal"));
            stockBal.Columns["Cost Price"].SetOrdinal(5);
            for (int i = 0; i < stockBal.Rows.Count; i++)
            {
                //manually select from the result table....
                decimal openingQty, closingQty, undeductedQty;
                DataRow[] dr;
                dr = openingBal.Select("itemno = '" + stockBal.Rows[i]["ItemNo"] + "'");
                if (dr != null && dr.Length > 0)
                {
                    openingQty = dr[0]["OnHand"].ToString().GetDecimalValue();
                }
                else
                {
                    openingQty = 0;
                }
                dr = closingBal.Select("itemno = '" + stockBal.Rows[i]["ItemNo"] + "'");
                if (dr != null && dr.Length > 0)
                {
                    closingQty = dr[0]["OnHand"].ToString().GetDecimalValue();
                }
                else
                {
                    closingQty = 0;
                }
                dr = undeductedSales.Select("itemno = '" + stockBal.Rows[i]["ItemNo"] + "'");
                if (dr != null && dr.Length > 0)
                {
                    undeductedQty = dr[0]["Quantity"].ToString().GetDecimalValue();
                }
                else
                {
                    undeductedQty = 0;
                }
                //decimal cogs = 0.0M;
                //if (displayCostPrice)
                //{

                //    decimal onHandQty = closingQty;
                //    cogs =
                //        InventoryController.FetchAverageCostOfGoodsLeftByItemNo
                //        (onHandQty, stockBal.Rows[i]["ItemNo"].ToString(), InventoryLocationID);
                      

                //}
                //stockBal.Rows[i]["Cost Price"] = cogs;

                stockBal.Rows[i]["Opening Balance"] = openingQty;
                stockBal.Rows[i]["Closing Balance"] = closingQty;
                decimal cursales = 0;
                decimal.TryParse(stockBal.Rows[i]["Stock Out(Sales)"].ToString(), out cursales);
                stockBal.Rows[i]["Stock Out(Sales)"] = cursales + undeductedQty;
            }

            /*
            stockBal.Columns.Add("OnHand", System.Type.GetType("System.Int32"));
            stockBal.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
            stockBal.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));

            for (int j = 0; j < stockBal.Rows.Count; j++)
            {
                int stockin = 0, stockout = 0, transferin = 0, transferout = 0, adjustmentin = 0, adjustmentout = 0;
                int.TryParse(stockBal.Rows[j]["Stock In"].ToString(), out stockin);
                //int.TryParse(stockBal.Rows[j]["Stock Out"].ToString(), out stockout);
                int.TryParse(stockBal.Rows[j]["Transfer In"].ToString(), out transferin);
                int.TryParse(stockBal.Rows[j]["Transfer Out"].ToString(), out transferout);
                int.TryParse(stockBal.Rows[j]["Adjustment In"].ToString(), out adjustmentin);
                int.TryParse(stockBal.Rows[j]["Adjustment Out"].ToString(), out adjustmentout);
                int negativeSales = InventoryController.
                    GetTotalUndeductedSalesByItem(stockBal.Rows[j]["ItemNo"].ToString(),
                    InventoryLocationID);


                int onHandQty =
                    stockin - stockout + transferin -
                    transferout + adjustmentin - adjustmentout - negativeSales;
                stockBal.Rows[j]["OnHand"] = onHandQty;
                decimal cogs = 0.0M;
                if (displayCostPrice)
                {
                    cogs =
                        InventoryController.FetchAverageCostOfGoodsLeftByItemNo
                        (onHandQty, stockBal.Rows[j]["ItemNo"].ToString(), InventoryLocationID);

                }
                stockBal.Rows[j]["CostOfGoods"] = cogs;
                stockBal.Rows[j]["TotalCost"] = cogs * onHandQty;
            }

            */

            foreach (DataColumn col in stockBal.Columns)
            {
                if (col.ColumnName.ToLower() == "attributes1")
                {
                    AttributesLabel al = new AttributesLabel(1);
                    if (al != null && !String.IsNullOrEmpty(al.Label))
                    {
                        col.ColumnName = al.Label;
                    }
                }
            }

            return stockBal;
        }

        public static DataTable FetchInventoryActivityReport
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemName, string UserName, string InventoryLocationID, string movementType,
            string remark, string lineremark, string SortColumn, string SortDir)
        {
            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

            StartDate = StartDate.Date;
            EndDate = EndDate.Date.AddDays(1);

            if (CostingMethod == InventoryController.CostingTypes.FIFO)
                return FetchInventoryActivityReport_FIFO(useStartDate, useEndDate, StartDate, EndDate,
                    ItemName, UserName, InventoryLocationID, movementType, remark, lineremark, SortColumn, SortDir);
            else if (CostingMethod == InventoryController.CostingTypes.FixedAvg)
                return FetchInventoryActivityReport_FixedAvg(useStartDate, useEndDate, StartDate, EndDate,
                    ItemName, UserName, InventoryLocationID, movementType, remark, lineremark, SortColumn, SortDir);
            else
                return FetchInventoryActivityReport_FIFO(useStartDate, useEndDate, StartDate, EndDate,
                    ItemName, UserName, InventoryLocationID, movementType, remark, lineremark, SortColumn, SortDir);
        }
        public static DataTable FetchInventoryActivityReport_FIFO
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemName, string UserName, string InventoryLocationID, string movementType,
            string remark, string lineremark, string SortColumn, string SortDir)
        {

            string SQL = "Select * from ViewInventoryActivity Where 1=1 ";
            Query qr = ViewInventoryActivity.CreateQuery();

            if (useStartDate)
            {
                SQL += " AND " + ViewInventoryActivity.Columns.InventoryDate + " >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (useEndDate)
            {
                SQL += " AND " + ViewInventoryActivity.Columns.InventoryDate + " <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            if (ItemName != "")
            {
                SQL += " AND " + ViewInventoryActivity.Columns.ItemName + " LIKE '%" + ItemName + "%'";
            }

            if (UserName != "")
            {
                SQL += " AND " + ViewInventoryActivity.Columns.UserName + " LIKE '%" + UserName + "%'";
            }

            if (movementType != "" && movementType != "ALL")
            {
                SQL += " AND " + ViewInventoryActivity.Columns.MovementType + " = '" + movementType + "'";
            }

            if (InventoryLocationID != "" && InventoryLocationID != "0")
            {
                SQL += " AND " + ViewInventoryActivity.Columns.InventoryLocationID + " = " + InventoryLocationID;
            }

            if (remark != "")
            {

                SQL += " AND " + ViewInventoryActivity.Columns.Remark + " like '%" + remark + "%'";
            }


            if (lineremark != "")
            {

                SQL += " AND " + ViewInventoryActivity.Columns.ItemRemark + " like '%" + lineremark + "%'";
            }
            SQL += " AND NOT " + ViewInventoryActivity.Columns.MovementType + " = '" + InventoryController.InventoryMovementType_TransferOut + "'";
            SQL += " AND NOT " + ViewInventoryActivity.Columns.MovementType + " = '" + InventoryController.InventoryMovementType_TransferIn + "'";
            SQL += " AND NOT ISNULL(ItemRemark,'') LIKE 'Stock Take Adj.%'";

            SubSonic.TableSchema.TableColumn checkIfColumnExist = ViewInventoryActivity.Schema.GetColumn(SortColumn);

            //Sorting
            if (checkIfColumnExist != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    SQL += " Order by " + SortColumn + " ASC";
                }
                else if (SortDir.Trim() == "DESC")
                {
                    SQL += " Order by " + SortColumn + " DESC";
                }
                else
                {
                    SQL += " Order by InventoryDate DESC";

                }
            }
            else
            {
                SQL += " Order by InventoryDate DESC";
            }
            //QueryCommand cmd = qr.BuildSelectCommand();
            //cmd.AddParameter("", "");


            //DataTable dt = qr.WHERE("ItemRemark", Comparison.NotLike, "Stock Take Adj.%").
            //                OR( "ItemRemark",Comparison.Is, null).
            //                ExecuteDataSet().Tables[0];
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");

            return DataService.GetDataSet(cmd).Tables[0];

            //return DataService.GetDataSet(cmd).Tables[0];
        }
        public static DataTable FetchInventoryActivityReport_FixedAvg
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemName, string UserName, string InventoryLocationID, string movementType,
            string remark, string lineremark, string SortColumn, string SortDir)
        {
            string SQL =
                "SELECT * FROM (" +
                    "SELECT ID.InventoryDetRefNo, ID.ItemNo, ID.Quantity, ID.RemainingQty, ID.Remark AS ItemRemark " +
                        ", IH.InvoiceNo, IH.Supplier, IH.Remark, IH.InventoryDate, ISNULL(UM.DisplayName,IH.UserName) UserName " +
                        ", IH.InventoryHdrRefNo, IT.ItemName, IC.CategoryName, PL.InventoryLocationName " +
                        ", IH.MovementType, CASE WHEN IH.MovementType LIKE '% In' THEN ID.CostOfGoods ELSE ISNULL(CST.COG,0) END AS CostOfGoods "+
                        ", ID.StockInRefNo, PL.InventoryLocationID, ID.IsDiscrepancy, SOR.ReasonName, ID.FactoryPrice, ID.GST " +
                        ", IH.StockOutReasonID, IT.RetailPrice, IT.FactoryPrice AS FactoryPriceUSD, IT.ProductLine " +
                        ", IH.DepartmentID, IT.Attributes1, IT.Attributes2, IT.Attributes3, IT.Attributes4, IT.Attributes5 " +
                        ", IT.Attributes6, IT.Attributes7, IT.Attributes8, IT.Remark AS Expr1, IT.ProductionDate " +
                        ", IT.hasWarranty, IT.IsDelivery, IT.GSTRule, IT.IsVitaMix, IT.IsWaterFilter, IT.IsYoung " +
                        ", IT.IsJuicePlus, IT.IsCourse, IT.CourseTypeID, IT.IsServiceItem " +
                        ", IT.ItemNo + ' ' + IT.ItemName + ' ' + IT.CategoryName + ' ' + ISNULL(II.DepartmentName, '') + ' ' " +
                            "+ ISNULL(IT.ItemDesc, '') + ' ' + ISNULL(IT.Attributes1, '') + ' ' + ISNULL(IT.Attributes2, '') + ' ' " +
                            "+ ISNULL(IT.Attributes3, '') + ' ' + ISNULL(IT.Attributes4, '') + ' ' + ISNULL(IT.Attributes5, '') + ' ' " +
                            "+ ISNULL(IT.Attributes6, '') + ' ' + ISNULL(IT.Attributes7, '') + ' ' + ISNULL(IT.Attributes8, '') Search " +
                        ", II.DepartmentName , II.ItemDepartmentId, IT.Barcode, IT.ItemDesc, IT.MinimumPrice, IT.IsInInventory, IT.Brand " +
                        ", IT.IsNonDiscountable, IT.Deleted, ID.BalanceBefore, ID.BalanceAfter, ID.ExpiryDate " +
                    "FROM InventoryDet ID " +
                        "INNER JOIN InventoryHdr IH ON ID.InventoryHdrRefNo = IH.InventoryHdrRefNo " +
                        "INNER JOIN InventoryLocation PL ON IH.InventoryLocationID = PL.InventoryLocationID " +
                        "INNER JOIN Item IT ON ID.ItemNo = IT.ItemNo " +
                        "INNER JOIN Category IC ON IT.CategoryName = IC.CategoryName " +
                        "INNER JOIN ItemDepartment II ON IC.ItemDepartmentId = II.ItemDepartmentID " +
                        "LEFT OUTER JOIN InventoryStockOutReason SOR ON IH.StockOutReasonID = SOR.ReasonID " +
                        "LEFT OUTER JOIN UserMst UM ON IH.UserName = UM.UserName " +
                        "LEFT OUTER JOIN " +
                        "( "+
                            "SELECT InventoryLocationID, ItemNo, SUM(Quantity) AS Quantity" +
                                ", CASE WHEN SUM(Quantity) = 0 THEN 0 ELSE SUM(Quantity * CostOfGoods) / SUM(Quantity) END AS COG " +
                            "FROM InventoryHdr IH " +
                                "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
                            "WHERE MovementType LIKE '% In' " +
                            "GROUP BY InventoryLocationID, ItemNo " +
                        ") CST ON IT.ItemNo = CST.ItemNo AND PL.InventoryLocationID = CST.InventoryLocationID " +
                    "WHERE 1 = 1 ";

            if (useStartDate)
                SQL += "AND IH.InventoryDate >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            if (useEndDate)
                SQL += "AND IH.InventoryDate <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

            if (ItemName != "")
                SQL += "AND IT.ItemName LIKE '%" + ItemName + "%' ";

            if (UserName != "")
                SQL += "AND ISNULL(UM.DisplayName,IH.UserName) LIKE '%" + UserName + "%' ";

            if (movementType != "" && movementType != "ALL")
                SQL += "AND IH.MovementType = '" + movementType + "' ";

            if (InventoryLocationID != "" && InventoryLocationID != "0")
                SQL += "AND IH.InventoryLocationID = " + InventoryLocationID + " ";

            if (remark != "")
                SQL += "AND IH.Remark LIKE '%" + remark + "%' ";

            if (lineremark != "")
                SQL += "AND ID.Remark LIKE '%" + lineremark + "%' ";

            SQL += "AND NOT IH.MovementType = '" + InventoryController.InventoryMovementType_TransferOut + "' ";
            SQL += "AND NOT IH.MovementType = '" + InventoryController.InventoryMovementType_TransferIn + "' ";
            SQL += "AND NOT ISNULL(ID.Remark,'') LIKE 'Stock Take Adj.%' ";

            SQL += ") SP "; /// Closing Mark, So sorting will use the display name, not the original name.

            //Sorting
            if (SortDir.Trim() == "ASC")
                SQL += " Order by " + SortColumn + " ASC";
            else if (SortDir.Trim() == "DESC")
                SQL += " Order by " + SortColumn + " DESC";
            else
                SQL += " Order by InventoryDate DESC";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");

            return DataService.GetDataSet(cmd).Tables[0];
        }

        public static DataTable FetchInventoryActivityReportWithTransfer
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemName, string UserName, string InventoryLocationID, string movementType,
            string remark, string lineremark, 
            string SortColumn, string SortDir)
        {

            return FetchInventoryActivityReportWithTransferAndRefNo
            (useStartDate, useEndDate, StartDate, EndDate,
            ItemName, UserName, InventoryLocationID, movementType,
            remark, lineremark, "",
            SortColumn, SortDir);
        }

        public static DataTable FetchInventoryActivityReportWithTransferAndRefNo
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemName, string UserName, string InventoryLocationID, string movementType,
            string remark, string lineremark, string refNo,
            string SortColumn, string SortDir)
        {

            string SQL = "Select * from ViewInventoryActivity Where 1=1 ";
            Query qr = ViewInventoryActivity.CreateQuery();

            if (useStartDate)
            {
                SQL += " AND " + ViewInventoryActivity.Columns.InventoryDate + " >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (useEndDate)
            {
                SQL += " AND " + ViewInventoryActivity.Columns.InventoryDate + " <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            if (ItemName != "")
            {
                SQL += " AND (" + ViewInventoryActivity.Columns.ItemName + " LIKE '%" + ItemName + "%' OR " +
                       ViewInventoryActivity.Columns.ItemNo + " LIKE '%" + ItemName + "%' OR " +
                       ViewInventoryActivity.Columns.Barcode + " LIKE '%" + ItemName + "%')";
            }

            if (UserName != "")
            {
                SQL += " AND " + ViewInventoryActivity.Columns.UserName + " LIKE '%" + UserName + "%'";
            }

            if (movementType != "" && movementType != "ALL")
            {
                SQL += " AND " + ViewInventoryActivity.Columns.MovementType + " = '" + movementType + "'";
            }

            if (InventoryLocationID != "" && InventoryLocationID != "0")
            {
                SQL += " AND " + ViewInventoryActivity.Columns.InventoryLocationID + " = " + InventoryLocationID;
            }

            if (remark != "")
            {

                SQL += " AND " + ViewInventoryActivity.Columns.Remark + " like '%" + remark + "%'";
            }


            if (lineremark != "")
            {

                SQL += " AND " + ViewInventoryActivity.Columns.ItemRemark + " like '%" + lineremark + "%'";
            }

            if (refNo != "")
            {
                SQL += " AND " + ViewInventoryActivity.Columns.InventoryHdrRefNo + " like '%" + refNo + "%'";
            }
            //SQL += " AND NOT " + ViewInventoryActivity.Columns.MovementType + " = '" + InventoryController.InventoryMovementType_TransferOut + "'";
            //SQL += " AND NOT " + ViewInventoryActivity.Columns.MovementType + " = '" + InventoryController.InventoryMovementType_TransferIn + "'";
            SQL += " AND NOT ISNULL(ItemRemark,'') LIKE 'Stock Take Adj.%'";

            SubSonic.TableSchema.TableColumn checkIfColumnExist = ViewInventoryActivity.Schema.GetColumn(SortColumn);

            //Sorting
            if (checkIfColumnExist != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    SQL += " Order by " + SortColumn + " ASC";
                }
                else if (SortDir.Trim() == "DESC")
                {
                    SQL += " Order by " + SortColumn + " DESC";
                }
                else
                {
                    SQL += " Order by InventoryDate DESC";

                }
            }
            else
            {
                SQL += " Order by InventoryDate DESC";
            }
            //QueryCommand cmd = qr.BuildSelectCommand();
            //cmd.AddParameter("", "");


            //DataTable dt = qr.WHERE("ItemRemark", Comparison.NotLike, "Stock Take Adj.%").
            //                OR( "ItemRemark",Comparison.Is, null).
            //                ExecuteDataSet().Tables[0];
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");

            return DataService.GetDataSet(cmd).Tables[0];

            //return DataService.GetDataSet(cmd).Tables[0];
        }

        public static DataTable FetchInventoryStockOutReport
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemName, string UserName, string InventoryLocationID, string Remark, string StockOutReasonID,
            string SortColumn, string SortDir)
        {
            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

            StartDate = StartDate.Date;
            EndDate = EndDate.Date.AddDays(1);

            /*if (CostingMethod == InventoryController.CostingTypes.FIFO)
                return FetchInventoryStockOutReport_FIFO(useStartDate, useEndDate, StartDate, EndDate,
                    ItemName, UserName, InventoryLocationID, Remark, StockOutReasonID, SortColumn, SortDir);
            else if (CostingMethod == InventoryController.CostingTypes.FixedAvg)*/
                return FetchInventoryStockOutReport_FixedAvg(useStartDate, useEndDate, StartDate, EndDate,
                    ItemName, UserName, InventoryLocationID, Remark, StockOutReasonID, SortColumn, SortDir);
            /*else
                return FetchInventoryStockOutReport_FIFO(useStartDate, useEndDate, StartDate, EndDate,
                    ItemName, UserName, InventoryLocationID, Remark, StockOutReasonID, SortColumn, SortDir);*/
        }
        public static DataTable FetchInventoryStockOutReport_FIFO
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemName, string UserName, string InventoryLocationID, string Remark, string StockOutReasonID,
            string SortColumn, string SortDir)
        {
            string filter = "";

            if (useStartDate)
            {
                filter += " and inventorydate >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss ") + "'";
            }
            if (useEndDate)
            {
                filter += " and inventorydate <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (ItemName != "")
            {
                filter += " and ItemName like '%" + ItemName + "%'";
            }
            if (InventoryLocationID != "" && InventoryLocationID != "0")
            {
                filter += " and a.InventoryLocationID = " + InventoryLocationID + " ";
            }
            if (UserName != "")
            {
                filter += " and UserName like '%" + UserName + "%'";
            }
            if (Remark != "")
            {
                filter += " and a.Remark + b.remark like '%" + Remark + "%' ";
            }
            if (StockOutReasonID != "" && StockOutReasonID != "0")
            {
                filter += " and ReasonID = " + StockOutReasonID.ToString() + "";
            }
            string SQL = "" +
            "select b.remark as lineRemark,* " +
            "from InventoryHdr a inner join InventoryDet b " +
            "on a.InventoryHdrRefNo = b.InventoryHdrRefNo " +
            "inner join InventoryStockOutReason c " +
            "on a.StockOutReasonID = c.ReasonID inner join item x on x.itemno = b.itemno " +
            "inner join inventorylocation y on y.inventorylocationID = a.InventoryLocationID " +
            "where a.MovementType = 'Stock Out'  " +
            "and not StockOutReasonID = 0 and not StockOutReasonID=1 " +
            "and not StockOutReasonID = 2 " + filter;

            return DataService.GetDataSet(new QueryCommand(SQL, "PowerPOS")).Tables[0];
        }
        public static DataTable FetchInventoryStockOutReport_FixedAvg
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string ItemName, string UserName, string InventoryLocationID, string Remark, string StockOutReasonID,
            string SortColumn, string SortDir)
        {
            string sqlString =
                "SELECT InventoryDate, IT.ItemNo, ItemName, IT.CategoryName, IT.Attributes1, ProductLine, UserName, ID.Quantity, ReasonName, IT.Userfld1 as UOM " +
                    ", InventoryLocationName, RetailPrice, 0 AS FactoryPriceUSD, 0 AS FactoryPrice, ISNULL(ID.FactoryPrice,0) AS CostOfGoods, ISNULL(ID.FactoryPrice,0) * ID.Quantity AS TotalValue, IH.Remark AS Remark, ID.remark AS LineRemark " +
                "FROM InventoryHdr IH " +
                    "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
                    "INNER JOIN InventoryStockOutReason SOR ON IH.StockOutReasonID = SOR.ReasonID " +
                    "INNER JOIN Item IT ON ID.ItemNo = IT.ItemNo " +
                    "INNER JOIN InventoryLocation PL ON IH.InventoryLocationID = PL.InventoryLocationID " +
                "WHERE IH.MovementType = 'Stock Out' ";
                    //"AND StockOutReasonID NOT IN (0,1,2) ";

            if (useStartDate)
                sqlString += "AND InventoryDate >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss ") + "' ";
            if (useEndDate)
                sqlString += "AND inventorydate <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

            if (ItemName != "")
                sqlString += "AND ItemName like '%" + ItemName + "%' ";
            if (InventoryLocationID != "" && InventoryLocationID != "0")
                sqlString += "AND IH.InventoryLocationID = " + InventoryLocationID + " ";

            if (UserName != "")
                sqlString += "AND UserName like '%" + UserName + "%' ";
            if (Remark != "")
                sqlString += "AND ISNULL(IH.Remark,'') + ISNULL(ID.remark,'') LIKE '%" + Remark + "%' ";

            if (StockOutReasonID != "" && StockOutReasonID != "0")
                sqlString += "AND ReasonID = " + StockOutReasonID.ToString() + "";

            return DataService.GetDataSet(new QueryCommand(sqlString, "PowerPOS")).Tables[0];
        }
        public static DataTable FetchInventoryStockOutReportPivotByReason
            (DateTime StartDate, DateTime EndDate,
            string locationName, string reasonName, string search)
        {
            DataTable dt = Pivot(SPs.FetchStockOutReportGroupByProductAndStockOutReason
                (StartDate, EndDate, locationName, reasonName, search).GetReader(),
                "ItemNo", "ReasonName", "Quantity");

            //Add Total
            dt.Columns.Add("Total", System.Type.GetType("System.Int32"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int total = 0;
                for (int j = 4; j < dt.Columns.Count - 1; j++)
                {
                    if (dt.Rows[i][j] != null && dt.Rows[i][j] is Int32)
                    {
                        total += (int)dt.Rows[i][j];
                    }
                }
                dt.Rows[i][dt.Columns.Count - 1] = total;
            }
            return dt;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static DataTable FetchInventoryTransferReport
          (bool useStartDate, bool useEndDate,
            DateTime StartDate, DateTime EndDate,
            string SearchQuery, string FromLocationID, string ToLocationID,
            string TransferBy, string DeptID, string CategoryName,
            string SortColumn, string SortDir)
        {

            #region oldViewInventoryTransferDetailCollection
            /*
            ViewInventoryTransferDetailCollection myView = new ViewInventoryTransferDetailCollection();
            if (useStartDate & useEndDate)
            {
                myView.BetweenAnd(ViewInventoryTransferDetail.Columns.InventoryDate, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myView.Where(ViewInventoryTransferDetail.Columns.InventoryDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myView.Where(ViewInventoryTransferDetail.Columns.InventoryDate, SubSonic.Comparison.LessOrEquals, EndDate);
            }


            if (TransferBy != "")
            {
                myView.Where(ViewInventoryTransferDetail.Columns.TransferFromBy, SubSonic.Comparison.Like, TransferBy);
            }

            if (SearchQuery != "")
            {
                myView.Where(ViewInventoryTransferDetail.Columns.Search, SubSonic.Comparison.Like, SearchQuery);
            }

            if (FromLocationID != "")
            {
                myView.Where(ViewInventoryTransferDetail.Columns.FromInventoryLocationID, int.Parse(FromLocationID));
            }

            if (ToLocationID != "")
            {
                myView.Where(ViewInventoryTransferDetail.Columns.ToInventoryLocationID, int.Parse(ToLocationID));
            }

            SubSonic.TableSchema.TableColumn checkIfColumnExist = ViewInventoryTransferDetail.Schema.GetColumn(SortColumn);

            if (checkIfColumnExist != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myView.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myView.OrderByDesc(SortColumn);
                }
            }

            return myView.Load().ToDataTable();
            */
            #endregion

            try
            {
                DataTable dt = new DataTable();

                if (SortColumn == "") { SortColumn = "InventoryDate"; }
                if (SortDir == "") { SortDir = "DESC"; }
                if (DeptID == "0") { DeptID = ""; }
                //if (CategoryName == "") { CategoryName = "%"; }
                //if (SearchQuery == "") { SearchQuery = "%"; }

                string SQL =
                     "declare @startdate as DATETIME; " +
                    "declare @enddate  as DATETIME; " +
                    "set @startdate = '" + StartDate.ToString("yyyy-MM-dd") + "'; " +
                    "set @enddate = '" + EndDate.ToString("yyyy-MM-dd") + "'; " +
                    "SELECT FromLoc.InventoryDate, " +
                    "fromName.InventoryLocationName AS FromInventoryLocationName, " +
                    "ToName.InventoryLocationName AS ToInventoryLocationName, " +
                    "i.DepartmentName AS Department, i.CategoryName AS Category, i.ItemNo, ItemName = Replace(i.ItemName, '\"', ''), " +
                    "FromLoc.Quantity AS TransferQty, CASE WHEN ISNULL(FromLoc.CostOfGoods,0) = 0 then FromLoc.FactoryPrice else FromLoc.CostOfGoods END as CostOfGoods, " +
                    "FromLoc.Quantity*(CASE WHEN ISNULL(FromLoc.CostOfGoods,0) = 0 then FromLoc.FactoryPrice else FromLoc.CostOfGoods END) AS LineTotal, a.TransferFromBy " +
                    "FROM dbo.ViewInventoryActivityByItemNo AS FromLoc INNER JOIN " +
                          "dbo.LocationTransfer AS a ON FromLoc.InventoryHdrRefNo = a.FromInventoryHdrRefNo INNER JOIN " +
                          "dbo.ViewItem AS i ON FromLoc.ItemNo = i.ItemNo INNER JOIN " +
                          "dbo.InventoryLocation AS fromName ON a.FromInventoryLocationID = fromName.InventoryLocationID INNER JOIN " +
                          "dbo.InventoryLocation AS ToName ON a.ToInventoryLocationID = ToName.InventoryLocationID ";

                bool whereExist = false;

                //start and end Date
                if (useStartDate && useEndDate)
                {
                    SQL += "WHERE InventoryDate >= @startdate AND InventoryDate <= @enddate";
                    whereExist = true;
                }
                else if (useStartDate)
                {
                    SQL += "WHERE InventoryDate <= @startdate";
                    whereExist = true;
                }
                else if (useEndDate)
                {
                    SQL += "WHERE InventoryDate <= @enddate";
                    whereExist = true;
                }

                //transferBy
                if (whereExist && TransferBy != "")
                {
                    SQL += " AND a.TransferFromBy LIKE '%" + TransferBy + "%'";
                }
                else if (TransferBy != "")
                {
                    SQL += " WHERE a.TransferFromBy LIKE '%" + TransferBy + "%'";
                    whereExist = true;
                }

                //searchQuery
                if (whereExist && SearchQuery != "")
                {
                    SQL += " AND i.DepartmentName+i.CategoryName+i.ItemName+i.Barcode+i.ItemNo LIKE '%" + SearchQuery + "%'";
                }
                else if (SearchQuery != "")
                {
                    SQL += " WHERE i.DepartmentName+i.CategoryName+i.ItemName+i.Barcode+i.ItemNo LIKE '%" + SearchQuery + "%'";
                    whereExist = true;
                    //myView.Where(ViewInventoryTransferDetail.Columns.Search, SubSonic.Comparison.Like, SearchQuery);
                }

                //locationID
                if (whereExist && FromLocationID != "")
                {
                    SQL += " AND fromName.InventoryLocationID = " + int.Parse(FromLocationID);
                }
                else if (FromLocationID != "")
                {
                    SQL += " WHERE fromName.InventoryLocationID = " + int.Parse(FromLocationID);
                    whereExist = true;
                    
                    //myView.Where(ViewInventoryTransferDetail.Columns.FromInventoryLocationID, int.Parse(FromLocationID));
                }
                if (whereExist && ToLocationID != "")
                {
                    SQL += " AND ToName.InventoryLocationID = " + int.Parse(ToLocationID);
                }
                else if (ToLocationID != "")
                {
                    SQL += " WHERE ToName.InventoryLocationID = " + int.Parse(ToLocationID);
                    whereExist = true;
                    //myView.Where(ViewInventoryTransferDetail.Columns.ToInventoryLocationID, int.Parse(ToLocationID));
                }

                //Department
                if (whereExist && DeptID != "")
                {
                    SQL += " AND i.DepartmentName LIKE '%" + DeptID + "%'";
                }
                else if (DeptID != "")
                {
                    SQL += " WHERE i.DepartmentName LIKE '%" + DeptID + "%'";
                    whereExist = true;
                    //myView.Where(ViewInventoryTransferDetail.Columns.Search, SubSonic.Comparison.Like, SearchQuery);
                }
                //Category
                if (whereExist && CategoryName != "")
                {
                    SQL += " AND i.CategoryName LIKE '%" + CategoryName + "%'";
                }
                else if (CategoryName != "")
                {
                    SQL += " WHERE i.CategoryName LIKE '%" + CategoryName + "%'";
                    whereExist = true;
                    //myView.Where(ViewInventoryTransferDetail.Columns.Search, SubSonic.Comparison.Like, SearchQuery);
                }

                SQL += " ORDER BY " + SortColumn + " " + SortDir;
               
                dt.Load(SubSonic.DataService.GetReader(new QueryCommand(SQL)));

                return dt;
            }

            catch (Exception e)
            {
                Logger.writeLog(e);
                return null;
            }

        }


        public static DataTable FetchInventorySummaryReport(string DeptID)
        {
            DataTable dt = new DataTable("InventorySummaryReport");
            dt.Columns.Add("Department");
            dt.Columns.Add("Category");
            dt.Columns.Add("Item No");
            dt.Columns.Add("Item Name");

            InventoryLocationCollection invLoc = new InventoryLocationCollection();
            invLoc.Load();

            for (int i = 0; i < invLoc.Count; i++)
            {
                dt.Columns.Add(new DataColumn(invLoc[i].InventoryLocationName, Type.GetType("System.Int32")));
            }

            dt.Columns.Add(new DataColumn("ALL", Type.GetType("System.Int32")));
            DataTable[] InvBalQty;
            InvBalQty = new DataTable[dt.Columns.Count - 4];

            for (int i = 4; i < dt.Columns.Count; i++)
            {
                InvBalQty[i - 4] = SPs.FetchStockReport("%", "%",
                   dt.Columns[i].ColumnName, DeptID, "ItemNo", "ASC").GetDataSet().Tables[0];
                InvBalQty[i - 4].TableName = dt.Columns[i].ColumnName;
            }

            ItemCollection myItem = new ItemCollection();
            myItem.Where(Item.Columns.IsInInventory, true);
            myItem.OrderByAsc("ItemName");
            myItem.Load();

            DataRow[] dr;
            DataRow dtRow;

            for (int j = 0; j < myItem.Count; j++)
            {
                dtRow = dt.NewRow();
                dtRow["Department"] = myItem[j].Category.ItemDepartment.DepartmentName;
                dtRow["Item No"] = myItem[j].ItemNo;
                dtRow["Item Name"] = myItem[j].ItemName;
                dtRow["Category"] = myItem[j].CategoryName;
                //dtRow["SkinType"] = myItem[j].SkinType;

                for (int i = 0; i < InvBalQty.Length; i++)
                {
                    dr = InvBalQty[i].Select("ItemNo = '" + myItem[j].ItemNo + "'");
                    if (dr != null && dr.Length > 0)
                    {
                        dtRow[InvBalQty[i].TableName] = dr[0]["OnHand"].ToString();
                    }
                    else
                    {
                        dtRow[InvBalQty[i].TableName] = "0";
                    }
                }
                dt.Rows.Add(dtRow);
            }

            return dt;
        }

        public static DataTable FetchStockTakeReport
       (bool useFromEndDate, bool useToEndDate,
           DateTime FromEndDate, DateTime ToEndDate,
           string SearchText, int InventoryLocationID,
            string TakenBy, string VerifiedBy,
           string SortColumn, string SortDir)
        {

            ViewAdjustedStockTakeCollection myViewAdjustedStockTake = new ViewAdjustedStockTakeCollection();

            if (useFromEndDate & useToEndDate)
            {
                myViewAdjustedStockTake.BetweenAnd(ViewAdjustedStockTake.Columns.StockTakeDate, FromEndDate, ToEndDate);
            }
            else if (useFromEndDate)
            {
                myViewAdjustedStockTake.Where(ViewAdjustedStockTake.Columns.StockTakeDate, SubSonic.Comparison.GreaterOrEquals, FromEndDate);
            }
            else if (useToEndDate)
            {
                myViewAdjustedStockTake.Where(ViewAdjustedStockTake.Columns.StockTakeDate, SubSonic.Comparison.LessOrEquals, ToEndDate);
            }

            if (SearchText != "")
            {
                myViewAdjustedStockTake.Where(ViewAdjustedStockTake.Columns.Search, SubSonic.Comparison.Like, "%" + SearchText + "%");
            }

            if (TakenBy != "")
            {
                myViewAdjustedStockTake.Where(ViewAdjustedStockTake.Columns.TakenBy, SubSonic.Comparison.Like, "%" + TakenBy + "%");
            }

            if (VerifiedBy != "")
            {
                myViewAdjustedStockTake.Where(ViewAdjustedStockTake.Columns.VerifiedBy, SubSonic.Comparison.Like, "%" + VerifiedBy + "%");
            }

            if (InventoryLocationID != 0)
            {
                myViewAdjustedStockTake.Where(ViewAdjustedStockTake.Columns.InventoryLocationID,
                    SubSonic.Comparison.Equals, InventoryLocationID);
            }

            SubSonic.TableSchema.TableColumn t = ViewAdjustedStockTake.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewAdjustedStockTake.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewAdjustedStockTake.OrderByDesc(SortColumn);
                }
            }

            DataTable dt = myViewAdjustedStockTake.Load().ToDataTable();

            foreach (DataRow row in dt.Rows)
            {
                row["ItemName"] = row["ItemName"].ToString().Replace("\"", "");
                row["itemname"] = row["ItemName"].ToString().Replace("\"", "");   
            }

            return dt;
        }
        /*
        public static DataTable FetchStockReport(
           string searchQuery,
           int InventoryLocationID,
           string DeptID,
           string SortColumn,
           string SortDir
        )
        {
            if (searchQuery == "") { searchQuery = "%"; }
            //if (InventoryLocationName == "") { InventoryLocationName = "ALL"; }
            if (DeptID == "0") { DeptID = ""; }

            Query qr = ViewItem.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.AddWhere(ViewItem.Columns.Deleted, false);
            qr.AddWhere(ViewItem.Columns.IsInInventory, true);
            string tmp = "%" + searchQuery + "%";

            DataTable remainingItem = (qr.WHERE(ViewItem.Columns.Search, Comparison.Like, tmp).ExecuteDataSet()).Tables[0];
            remainingItem.Columns.Add("OnHand", System.Type.GetType("System.Int32"));
            remainingItem.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
            remainingItem.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));

            string status;
            //loop through and add into the item....
            for (int j = 0; j < remainingItem.Rows.Count; j++)
            {
                int onHandQty = InventoryController.GetStockBalanceQtyByItemByDate(remainingItem.Rows[j]["ItemNo"].ToString(),                    
                InventoryLocationID, UtilityController.GetCurrentTime(), out status);
                int negativeSales = InventoryController.GetTotalUndeductedSalesByItem(remainingItem.Rows[j]["ItemNo"].ToString(), InventoryLocationID);
                remainingItem.Rows[j]["OnHand"] = onHandQty - negativeSales;
                decimal cogs = InventoryController.FetchAverageCostOfGoodsLeftByItemNo(onHandQty, remainingItem.Rows[j]["ItemNo"].ToString(), InventoryLocationID);
                remainingItem.Rows[j]["CostOfGoods"] = cogs;
                remainingItem.Rows[j]["TotalCost"] = cogs * onHandQty;
            }

            return remainingItem;
        }
        */
        /*
        public static DataTable FetchStockReportAgainstRemainingQty(
           string searchQuery,
           int InventoryLocationID,
           string DeptID,
           string SortColumn,
           string SortDir
        )
        {
            if (searchQuery == "") { searchQuery = "%"; }
            //if (InventoryLocationName == "") { InventoryLocationName = "ALL"; }
            if (DeptID == "0") { DeptID = ""; }

            Query qr = ViewItem.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.AddWhere(ViewItem.Columns.Deleted, false);
            qr.AddWhere(ViewItem.Columns.IsInInventory, true);
            string tmp = "%" + searchQuery + "%";

            DataTable remainingItem = (qr.WHERE(ViewItem.Columns.Search, Comparison.Like, tmp).ExecuteDataSet()).Tables[0];
            remainingItem.Columns.Add("OnHand", System.Type.GetType("System.Int32"));
            remainingItem.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
            remainingItem.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));
            remainingItem.Columns.Add("RemainingQty", System.Type.GetType("System.Int32"));
            string status;
            //loop through and add into the item....
            for (int j = 0; j < remainingItem.Rows.Count; j++)
            {
                int onHandQty = InventoryController.GetStockBalanceQtyByItemByDate(remainingItem.Rows[j]["ItemNo"].ToString(),
                InventoryLocationID, UtilityController.GetCurrentTime(), out status);
                remainingItem.Rows[j]["OnHand"] = onHandQty;
                decimal cogs = InventoryController.FetchAverageCostOfGoodsLeftByItemNo(onHandQty, remainingItem.Rows[j]["ItemNo"].ToString(), InventoryLocationID);
                remainingItem.Rows[j]["CostOfGoods"] = cogs;
                remainingItem.Rows[j]["TotalCost"] = cogs * onHandQty;
            }

            return remainingItem;
        }
        */
        /*
        public static DataTable FetchStockReport(
           string itemname,
           string CategoryName,
           string InventoryLocationName,
           string DeptID,
           string SortColumn,
           string SortDir
        )
        {
            if (CategoryName == "") { CategoryName = "%"; }
            if (itemname == "") { itemname = "%"; }
            if (InventoryLocationName == "") { InventoryLocationName = "ALL"; }
            if (DeptID == "0") { DeptID = ""; }
            //if (SortColumn == "") { SortColumn = "ItemName"; }
            //if (SortDir == "") { SortDir = "ASC"; }
            DataSet ds = SPs.FetchStockReport(itemname, CategoryName,
                        InventoryLocationName, DeptID, SortColumn, SortDir).GetDataSet();


            return ds.Tables[0];
        }
        */
        public static DataTable FetchDeletedItemStockReport(
           string searchQuery,
           int InventoryLocationID,
            bool displayCostPrice,
           string DeptID,
           string SortColumn,
           string SortDir
        )
        {

            if (searchQuery == "") { searchQuery = "%"; }
            //if (InventoryLocationName == "") { InventoryLocationName = "ALL"; }
            if (DeptID == "0") { DeptID = ""; }
            //Query qr = ViewItem.CreateQuery();
            //qr.QueryType = QueryType.Select;
            //qr.AddWhere(ViewItem.Columns.Deleted, false);
            //qr.AddWhere(ViewItem.Columns.IsInInventory, true);
            string tmp = "%" + searchQuery + "%";
            string searchSQL = " AND c.ItemNo+c.Barcode+ItemName+ISNULL(attributes1,'')+ISNULL(attributes2,'')+ISNULL(attributes3,'')+ " +
                         "ISNULL(attributes4,'')+ISNULL(attributes5,'')+ISNULL(attributes6,'')+ISNULL(attributes7,'')+ISNULL(attributes8,'')+ " +
                         "ISNULL(ItemDesc,'')+ISNULL(departmentname,'') like '%' + @search + '%' ";
            if (searchQuery == "") searchSQL = "";


            if (SortColumn == "") SortColumn = "ItemNo";
            if (SortDir == "") SortDir = "ASC";
            string InventoryLocationName = new InventoryLocation(InventoryLocationID).InventoryLocationName;

            //Get inventoryLocationID from InventoryLocationName

            string SQL = "declare @search as nvarchar(200); " +
                        "declare @inventorylocationName as nvarchar(200); " +
                        "set @search = '" + searchQuery + "' " +
                        "set @inventorylocationName = '" + InventoryLocationName + "' " +
                        "select f.departmentname,c.itemno,c.categoryname, c.itemname,isnull(movementtype,'Stock In') as MovementType, SUM(isnull(quantity,0)) as Quantity  " +
                        "from inventoryhdr a  " +
                        "inner join inventorydet b " +
                        "on a.inventoryhdrrefno = b.inventoryhdrrefno " +
                        "right outer join Item c  " +
                        "on c.ItemNo = b.itemno  " +
                        "inner join Category d on " +
                        "d.categoryname = c.CategoryName " +
                        "inner join InventoryLocation e  " +
                        "on a.InventoryLocationID = e.inventorylocationid " +
                        "inner join itemdepartment f on d.itemdepartmentid = f.itemdepartmentid " +
                        "where  " +
                        "c.deleted=1 and InventoryLocationName like '%' + @inventorylocationName + '%' " + searchSQL +
                        "group by f.departmentname,c.itemno,c.categoryname, c.itemname,MovementType " +
                        "order by " + SortColumn + " " + SortDir;

            DataTable stockBal = Pivot(DataService.GetReader(new QueryCommand(SQL, "PowerPOS")), "ItemNo", "MovementType", "Quantity");
            if (!stockBal.Columns.Contains("Stock In")) stockBal.Columns.Add("Stock In", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Stock Out")) stockBal.Columns.Add("Stock Out", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Transfer In")) stockBal.Columns.Add("Transfer In", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Transfer Out")) stockBal.Columns.Add("Transfer Out", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Adjustment In")) stockBal.Columns.Add("Adjustment In", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Adjustment Out")) stockBal.Columns.Add("Adjustment Out", System.Type.GetType("System.Int32"));

            stockBal.Columns.Add("OnHand", System.Type.GetType("System.Int32"));
            stockBal.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
            stockBal.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));

            DataTable undeductedSales = ReportController.FetchUnDeductedSales(InventoryLocationID);

            for (int j = 0; j < stockBal.Rows.Count; j++)
            {
                int stockin = 0, stockout = 0, transferin = 0, transferout = 0, adjustmentin = 0, adjustmentout = 0;
                int.TryParse(stockBal.Rows[j]["Stock In"].ToString(), out stockin);
                int.TryParse(stockBal.Rows[j]["Stock Out"].ToString(), out stockout);
                int.TryParse(stockBal.Rows[j]["Transfer In"].ToString(), out transferin);
                int.TryParse(stockBal.Rows[j]["Transfer Out"].ToString(), out transferout);
                int.TryParse(stockBal.Rows[j]["Adjustment In"].ToString(), out adjustmentin);
                int.TryParse(stockBal.Rows[j]["Adjustment Out"].ToString(), out adjustmentout);
                //int negativeSales = InventoryController.
                //    GetTotalUndeductedSalesByItem(stockBal.Rows[j]["ItemNo"].ToString(),
                //    InventoryLocationID);
                int negativeSales = 0;
                DataRow[] dr = undeductedSales.Select("itemno = '" + stockBal.Rows[j]["ItemNo"] + "'");
                if (dr != null && dr.Length > 0)
                {
                    negativeSales = (int)dr[0]["Quantity"];
                }
                else
                {
                    negativeSales = 0;
                }
                

                int onHandQty =
                    stockin - stockout + transferin -
                    transferout + adjustmentin - adjustmentout - negativeSales;
                stockBal.Rows[j]["OnHand"] = onHandQty;
                decimal cogs = 0.0M;
                if (displayCostPrice)
                {
                    cogs =
                        InventoryController.FetchAverageCostOfGoodsLeftByItemNo
                        (onHandQty, stockBal.Rows[j]["ItemNo"].ToString(), InventoryLocationID);

                }
                stockBal.Rows[j]["CostOfGoods"] = cogs;
                stockBal.Rows[j]["TotalCost"] = cogs * onHandQty;
            }


            return stockBal;
        }

        public static DataTable FetchStockReportByDate(
           string searchQuery,
           int InventoryLocationID,
            bool displayCostPrice,
            DateTime StockBalanceDate,
           string DeptID,
           string SortColumn,
           string SortDir
        )
        {

            if (searchQuery == "") { searchQuery = "%"; }
            //if (InventoryLocationName == "") { InventoryLocationName = "ALL"; }
            if (DeptID == "0") { DeptID = ""; }
            //Query qr = ViewItem.CreateQuery();
            //qr.QueryType = QueryType.Select;
            //qr.AddWhere(ViewItem.Columns.Deleted, false);
            //qr.AddWhere(ViewItem.Columns.IsInInventory, true);
            string tmp = "%" + searchQuery + "%";
            string searchSQL = " AND c.ItemNo+ItemName+c.Barcode+ISNULL(attributes1,'')+ISNULL(attributes2,'')+ISNULL(attributes3,'')+ " +
                         "ISNULL(attributes4,'')+ISNULL(attributes5,'')+ISNULL(attributes6,'')+ISNULL(attributes7,'')+ISNULL(attributes8,'')+ " +
                         "ISNULL(ItemDesc,'')+ISNULL(departmentname,'') like '%' + @search + '%' ";
            if (searchQuery == "") searchSQL = "";
            
            
            if (SortColumn == "") SortColumn = "ItemNo";
            if (SortDir == "") SortDir = "ASC";
            string InventoryLocationName = new InventoryLocation(InventoryLocationID).InventoryLocationName;
            
            //Get inventoryLocationID from InventoryLocationName

            string SQL = "declare @search as nvarchar(200); " +
                        "declare @inventorylocationName as nvarchar(200); " +
                        "set @search = '" + searchQuery + "' " +
                        "set @inventorylocationName = '" + InventoryLocationName + "' " +
                        "select f.departmentname,c.itemno,c.categoryname, c.itemname,isnull(movementtype,'Stock In') as MovementType, SUM(isnull(quantity,0)) as Quantity  " +
                        "from inventoryhdr a  " +
                        "inner join inventorydet b " +
                        "on a.inventoryhdrrefno = b.inventoryhdrrefno " +
                        "right outer join Item c  " +
                        "on c.ItemNo = b.itemno  " +
                        "inner join Category d on " +
                        "d.categoryname = c.CategoryName " +
                        "inner join InventoryLocation e  " +
                        "on a.InventoryLocationID = e.inventorylocationid " +
                        "inner join itemdepartment f on d.itemdepartmentid = f.itemdepartmentid " + 
                        "where  " + "inventorydate <= '" + StockBalanceDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and " + 
                        "(c.deleted=0 or c.deleted is null) and InventoryLocationName like '%' + @inventorylocationName + '%' " + searchSQL +
                        "group by f.departmentname,c.itemno,c.categoryname, c.itemname,MovementType " +
                        "order by " + SortColumn + " " + SortDir;

            DataTable stockBal = Pivot(DataService.GetReader(new QueryCommand(SQL, "PowerPOS")), "ItemNo", "MovementType", "Quantity");
            if (!stockBal.Columns.Contains("Stock In")) stockBal.Columns.Add("Stock In", System.Type.GetType("System.Decimal"));
            if (!stockBal.Columns.Contains("Stock Out")) stockBal.Columns.Add("Stock Out", System.Type.GetType("System.Decimal"));
            if (!stockBal.Columns.Contains("Transfer In")) stockBal.Columns.Add("Transfer In", System.Type.GetType("System.Decimal"));
            if (!stockBal.Columns.Contains("Transfer Out")) stockBal.Columns.Add("Transfer Out", System.Type.GetType("System.Decimal"));
            if (!stockBal.Columns.Contains("Adjustment In")) stockBal.Columns.Add("Adjustment In", System.Type.GetType("System.Decimal"));
            if (!stockBal.Columns.Contains("Adjustment Out")) stockBal.Columns.Add("Adjustment Out", System.Type.GetType("System.Decimal"));

            stockBal.Columns.Add("OnHand", System.Type.GetType("System.Decimal"));
            stockBal.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
            stockBal.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));

            for (int j = 0; j < stockBal.Rows.Count; j++)
            {
                decimal stockin = 0, stockout = 0, transferin = 0, transferout = 0, adjustmentin = 0, adjustmentout = 0;
                decimal.TryParse(stockBal.Rows[j]["Stock In"].ToString(), out stockin);
                decimal.TryParse(stockBal.Rows[j]["Stock Out"].ToString(), out stockout);
                decimal.TryParse(stockBal.Rows[j]["Transfer In"].ToString(), out transferin);
                decimal.TryParse(stockBal.Rows[j]["Transfer Out"].ToString(), out transferout);
                decimal.TryParse(stockBal.Rows[j]["Adjustment In"].ToString(), out adjustmentin);
                decimal.TryParse(stockBal.Rows[j]["Adjustment Out"].ToString(), out adjustmentout);
                decimal negativeSales = InventoryController.
                    GetTotalUndeductedSalesByItem(stockBal.Rows[j]["ItemNo"].ToString(),
                    InventoryLocationID);


                decimal onHandQty =
                    stockin - stockout + transferin -
                    transferout + adjustmentin - adjustmentout - negativeSales;
                stockBal.Rows[j]["OnHand"] = onHandQty;
                decimal cogs = 0.0M;
                if (displayCostPrice)
                {
                    cogs =
                        InventoryController.FetchAverageCostOfGoodsLeftByItemNo
                        (onHandQty, stockBal.Rows[j]["ItemNo"].ToString(), InventoryLocationID);
                    
                }
                stockBal.Rows[j]["CostOfGoods"] = cogs;
                stockBal.Rows[j]["TotalCost"] = cogs * onHandQty;
            }


            return stockBal;
        }

        public static DataTable FetchStockReport
            (string searchQuery, int InventoryLocationID, bool displayCostPrice, string DeptID, string SortColumn, string SortDir)
        {
            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

           /* if (CostingMethod == InventoryController.CostingTypes.FIFO)
                return FetchStockReport_FIFO(searchQuery, InventoryLocationID, displayCostPrice, DeptID, SortColumn, SortDir);
            else if (CostingMethod == InventoryController.CostingTypes.FixedAvg)
                return FetchStockReportItem_FixedAvg(searchQuery, InventoryLocationID, displayCostPrice, DeptID, SortColumn, SortDir);
            else
                return FetchStockReport_FIFO(searchQuery, InventoryLocationID, displayCostPrice, DeptID, SortColumn, SortDir);*/
            return FetchStockReportItemSummary_FIFO(searchQuery, InventoryLocationID, displayCostPrice, DeptID, SortColumn, SortDir);
            
        }
        #region *) Fetch Stock Report OLD
        public static DataTable FetchStockReport_FIFO
            (string searchQuery, int InventoryLocationID, bool displayCostPrice, string DeptID, string SortColumn, string SortDir)
        {

            if (searchQuery == "") { searchQuery = "%"; }
            //if (InventoryLocationName == "") { InventoryLocationName = "ALL"; }
            if (DeptID == "0") { DeptID = ""; }
            //Query qr = ViewItem.CreateQuery();
            //qr.QueryType = QueryType.Select;
            //qr.AddWhere(ViewItem.Columns.Deleted, false);
            //qr.AddWhere(ViewItem.Columns.IsInInventory, true);
            string tmp = "%" + searchQuery + "%";
            string searchSQL = " AND c.ItemNo+ItemName+c.Barcode+ISNULL(attributes1,'')+ISNULL(attributes2,'')+ISNULL(attributes3,'')+ " +
                         "ISNULL(attributes4,'')+ISNULL(attributes5,'')+ISNULL(attributes6,'')+ISNULL(attributes7,'')+ISNULL(attributes8,'')+ " +
                         "ISNULL(ItemDesc,'')+ISNULL(departmentname,'') like '%' + @search + '%' ";
            if (searchQuery == "") searchSQL = "";


            if (SortColumn == "") SortColumn = "ItemNo";
            if (SortDir == "") SortDir = "ASC";
            string InventoryLocationName = new InventoryLocation(InventoryLocationID).InventoryLocationName;

            //Get inventoryLocationID from InventoryLocationName


            string SQL = "DECLARE @search AS NVARCHAR(200); " +
                        "DECLARE @inventorylocationName AS NVARCHAR(200); " +
                        "SET @search = '" + searchQuery + "' " +
                        "SET @inventorylocationName = '" + InventoryLocationName + "' " +
                        "SELECT c.FactoryPrice, c.RetailPrice, f.departmentname,c.itemno,c.categoryname, itemname = replace(c.itemname, '\"', ''),isnull(movementtype,'Stock In') as MovementType, " + 
                        "SUM(isnull(quantity,0)) as Quantity, c.Userfld1 as UOM   " +
                            ", Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8 " +
                        "FROM (inventoryhdr a " +
                            "INNER JOIN inventorydet b ON a.inventoryhdrrefno = b.inventoryhdrrefno " +
                            "INNER JOIN InventoryLocation e ON a.InventoryLocationID = e.inventorylocationid " +
                            ")RIGHT OUTER JOIN Item c ON c.ItemNo = b.itemno " +
                            "INNER JOIN Category d ON d.categoryname = c.CategoryName " +
                            "INNER JOIN itemdepartment f on d.itemdepartmentid = f.itemdepartmentid " +
                        "WHERE " +
                        "(c.deleted=0 or c.deleted is null) AND f.ItemDepartmentID <> 'SYSTEM' AND c.IsInInventory = 1 "+
                            "and ISNULL(InventoryLocationName,'') like '%' + @inventorylocationName + '%' " + searchSQL +
                        "group by f.departmentname,c.itemno,c.categoryname, c.itemname,isnull(movementtype,'Stock In') " +
                            ", Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8, c.RetailPrice " +
                        "order by " + SortColumn + " " + SortDir;

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            cmd.CommandTimeout = 9999999;
            DataTable stockBal = Pivot(DataService.GetReader(cmd), "ItemNo", "MovementType", "Quantity");
            if (!stockBal.Columns.Contains("Stock In")) stockBal.Columns.Add("Stock In", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Stock Out")) stockBal.Columns.Add("Stock Out", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Transfer In")) stockBal.Columns.Add("Transfer In", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Transfer Out")) stockBal.Columns.Add("Transfer Out", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Adjustment In")) stockBal.Columns.Add("Adjustment In", System.Type.GetType("System.Int32"));
            if (!stockBal.Columns.Contains("Adjustment Out")) stockBal.Columns.Add("Adjustment Out", System.Type.GetType("System.Int32"));

            stockBal.Columns.Add("OnHand", System.Type.GetType("System.Int32"));
            stockBal.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
            stockBal.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));

            DataTable undeductedSales = ReportController.FetchUnDeductedSales(InventoryLocationID);

            for (int j = 0; j < stockBal.Rows.Count; j++)
            {
                int stockin = 0, stockout = 0, transferin = 0, transferout = 0, adjustmentin = 0, adjustmentout = 0;
                int.TryParse(stockBal.Rows[j]["Stock In"].ToString(), out stockin);
                int.TryParse(stockBal.Rows[j]["Stock Out"].ToString(), out stockout);
                int.TryParse(stockBal.Rows[j]["Transfer In"].ToString(), out transferin);
                int.TryParse(stockBal.Rows[j]["Transfer Out"].ToString(), out transferout);
                int.TryParse(stockBal.Rows[j]["Adjustment In"].ToString(), out adjustmentin);
                int.TryParse(stockBal.Rows[j]["Adjustment Out"].ToString(), out adjustmentout);
                //int negativeSales = InventoryController.
                //    GetTotalUndeductedSalesByItem(stockBal.Rows[j]["ItemNo"].ToString(),
                //    InventoryLocationID);
                int negativeSales = 0;
                DataRow[] dr = undeductedSales.Select("itemno = '" + stockBal.Rows[j]["ItemNo"] + "'");
                if (dr != null && dr.Length > 0)
                {
                    negativeSales = (int)dr[0]["Quantity"];
                }
                else
                {
                    negativeSales = 0;
                }
                
                int onHandQty =
                    stockin - stockout + transferin -
                    transferout + adjustmentin - adjustmentout - negativeSales;
                stockBal.Rows[j]["OnHand"] = onHandQty;
                decimal cogs = 0.0M;
                if (displayCostPrice)
                {
                    cogs =
                        InventoryController.FetchAverageCostOfGoodsLeftByItemNo
                        (onHandQty, stockBal.Rows[j]["ItemNo"].ToString(), InventoryLocationID);

                }
                stockBal.Rows[j]["CostOfGoods"] = cogs;
                stockBal.Rows[j]["TotalCost"] = cogs * onHandQty;
            }


            return stockBal;
        }

        public static DataTable FetchStockReport_FixedAvg
            (string searchQuery, int InventoryLocationID, bool displayCostPrice, string DeptID, string SortColumn, string SortDir)
        {
            if (searchQuery == "") { searchQuery = "%"; }
            if (SortColumn == "") SortColumn = "ItemNo";
            if (SortDir == "") SortDir = "ASC";

            string InventoryLocationName = new InventoryLocation(InventoryLocationID).InventoryLocationName;

//            string SQL = 
//                "DECLARE @search AS NVARCHAR(200); " +
//                "DECLARE @inventorylocationName AS NVARCHAR(200); " +
//                "DECLARE @inventorylocationID AS INT; " +
//                "SET @search = '" + searchQuery + "' " +
//                "SET @inventorylocationName = '" + InventoryLocationName + "' " +
//                "SET @inventorylocationID = " + InventoryLocationID + " " +
//                "SELECT IT.FactoryPrice, IT.RetailPrice, IP.DepartmentName,IT.ItemNo,IC.CategoryName, ItemName = Replace(IT.ItemName, '\"', '') " +
//                    ", [Stock In], [Stock Out], [Transfer In], [Transfer Out], [Adjustment In], [Adjustment Out] " +
//                    ", Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8, " +
//                    @" ( CASE WHEN (ISNULL((ISNULL([Stock In],0)- ISNULL([Stock Out],0) + ISNULL([Transfer In],0) -ISNULL([Transfer Out],0)+ ISNULL([Adjustment In],0)-[Adjustment Out]),0) - ISNULL(OD.Qty,0)) < 0 THEN 0 ELSE (ISNULL((ISNULL([Stock In],0)- ISNULL([Stock Out],0) + ISNULL([Transfer In],0) -ISNULL([Transfer Out],0)+ ISNULL([Adjustment In],0)-[Adjustment Out]),0) - ISNULL(OD.Qty,0)) * ISNULL(CG.COG,0) END ) / (
//                        case 
//                            when ( [Stock In] + [Adjustment In] + [Transfer In] - [Stock Out] - [Adjustment Out] - [Transfer Out] ) > 0 then ( [Stock In] + [Adjustment In] + [Transfer In] - [Stock Out] - [Adjustment Out] - [Transfer Out] )
//                            else 1
//                        end
//                    )  as TotalCostPrice  " + 
//                "FROM " +
//                    "( " +
//                        "SELECT ItemNo " +
//                            ", SUM(CASE WHEN movementtype LIKE 'Stock In' THEN quantity ELSE 0 END) as [Stock In] " +
//                            ", SUM(CASE WHEN movementtype LIKE 'Stock Out' THEN quantity ELSE 0 END) as [Stock Out] " +
//                            ", SUM(CASE WHEN movementtype LIKE 'Transfer In' THEN quantity ELSE 0 END) as [Transfer In] " +
//                            ", SUM(CASE WHEN movementtype LIKE 'Transfer Out' THEN quantity ELSE 0 END) as [Transfer Out] " +
//                            ", SUM(CASE WHEN movementtype LIKE 'Adjustment In' THEN quantity ELSE 0 END) as [Adjustment In] " +
//                            ", SUM(CASE WHEN movementtype LIKE 'Adjustment Out' THEN quantity ELSE 0 END) as [Adjustment Out] " +
//                        "FROM InventoryHdr IH " +
//                            "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
//                            "INNER JOIN InventoryLocation LI ON IH.InventoryLocationID = LI.InventoryLocationID "+
//                        "WHERE ISNULL(InventoryLocationName,'') like '%' + @inventorylocationName + '%' " +
//                        @"
//                            and CAST(IH.InventoryLocationID AS VARCHAR(4)) LIKE CASE WHEN @inventorylocationID <> 0 THEN CAST(@inventorylocationID AS VARCHAR(4)) ELSE '%' END 
//                        " +
//                        "GROUP BY ItemNo " +
//                    ") JQ " +
//                    "RIGHT OUTER JOIN Item IT ON JQ.ItemNo = IT.ItemNo " +
//                    "INNER JOIN Category IC ON IC.CategoryName = IT.CategoryName " +
//                    "INNER JOIN itemdepartment IP on IP.itemdepartmentid = IC.ItemDepartmentId " +
//                    @"LEFT JOIN (
//		                    select itemno, sum(quantity) as qty from orderdet 
//		                      where ISNULL(inventoryhdrrefno,'') = '' 
//		   and orderhdrid in (select orderhdrid from orderhdr where isvoided = 0) group by itemno
//	                    ) as OD
//                    on JQ.ItemNo = OD.ItemNo  
//                " + 
//                @"
//                    LEFT JOIN (select id.itemno, avg(costofgoods) as cog from inventoryhdr ih, inventorydet id, item i where ih.inventoryhdrrefno = id.inventoryhdrrefno 
//                        and ih.movementtype like '% In' and id.itemno = i.itemno group by id.itemno, i.factoryprice)  
//                        CG on JQ.Itemno = CG.ItemNo 
//                " +
//                " WHERE (IT.Deleted=0 or IT.Deleted is null) " +
//@"
//" +
//                    "AND IP.ItemDepartmentID <> 'SYSTEM' AND IsInInventory = 1 " +
//                    "AND (IP.DepartmentName LIKE '%' + @search + '%' OR IT.ItemNo+IT.Barcode+ItemName+ISNULL(attributes1,'')+ISNULL(attributes2,'')+ISNULL(attributes3,'') " +
//                        "+ ISNULL(attributes4,'') + ISNULL(attributes5,'') + ISNULL(attributes6,'')+ISNULL(attributes7,'')+ISNULL(attributes8,'') " +
//                        "+ ISNULL(ItemDesc,'') + IC.CategoryName LIKE '%' + @search + '%') " +
//                "ORDER BY " + SortColumn + " " + SortDir;

            string SQL = @"
                DECLARE @search AS NVARCHAR(200); 
                DECLARE @Location AS INT; 
                SET @search = '" + searchQuery + @"'; 
                SET @Location = " + InventoryLocationID + @"; 

                SELECT IT.FactoryPrice
                     , ItemName = Replace(IT.ItemName, '""', '') 
                     , IP.DepartmentName
                     , IT.ItemNo
	                 , IC.CategoryName
	                 , IT.ItemName
	                 , IT.RetailPrice     
	                 , Quantity = ISNULL((ISNULL(StockIn,0)- ISNULL(StockOut,0) + ISNULL(TransferIn,0) -ISNULL(TransferOut,0)+ ISNULL(AdjustmentIn,0)-AdjustmentOut),0) - ISNULL(OD.Qty,0)    
	                 , [Stock In] = ISNULL(StockIn,0) 
	                 , [Stock Out] = ISNULL(StockOut,0)     
	                 , [Transfer In] = ISNULL(TransferIn,0) 
	                 , [Transfer Out] = ISNULL(TransferOut,0)      
	                 , [Adjustment In] = ISNULL(AdjustmentIn,0) 
	                 , [Adjustment Out] = ISNULL(AdjustmentOut,0) 
                     , Attributes1
	                 , Attributes2
	                 , Attributes3
	                 , Attributes4
	                 , Attributes5
	                 , Attributes6
	                 , Attributes7
	                 , Attributes8
                     , IT.Userfld1 as UOM 
	                 , UndeductedSales = ISNULL(OD.Qty,0)     
	                 , COG = CASE 
				                WHEN ISNULL(JQ.ItemNo,'') = '' THEN IT.FactoryPrice 
				                WHEN ISNULL(CG.COG,0) = 0 THEN IT.FactoryPrice 
				                ELSE ISNULL(CG.COG,0) 
			                 END                
	                 , TotalCostPrice =	CASE 
						                WHEN (ISNULL((ISNULL(StockIn,0)- ISNULL(StockOut,0) + ISNULL(TransferIn,0) -ISNULL(TransferOut,0)+ ISNULL(AdjustmentIn,0)-AdjustmentOut),0) - ISNULL(OD.Qty,0)) < 0 THEN 0 
						                ELSE (ISNULL((ISNULL(StockIn,0)- ISNULL(StockOut,0) + ISNULL(TransferIn,0) -ISNULL(TransferOut,0)+ ISNULL(AdjustmentIn,0)-AdjustmentOut),0) - ISNULL(OD.Qty,0)) * ISNULL(CG.COG,0) 
					                END   
                  FROM (     
			                SELECT ItemNo
			                     , SUM ( 
							                CASE 
								                WHEN movementtype LIKE 'Stock In' THEN quantity 
								                ELSE 0 
							                END
					                   ) as StockIn             
				                 , SUM (	CASE 
								                WHEN movementtype LIKE 'Stock Out' THEN quantity 
								                ELSE 0 
							                END
					                   ) as StockOut     
				                , SUM(
							                CASE 
								                WHEN movementtype LIKE 'Transfer In' THEN quantity 
								                ELSE 0 
							                END 
					                   ) as TransferIn     
			                    , SUM (	
							                CASE 
								                WHEN movementtype LIKE 'Transfer Out' THEN quantity 
								                ELSE 0 
							                END
					                   ) as TransferOut     
			                    , SUM (
							                CASE 
								                WHEN movementtype LIKE 'Adjustment In' THEN quantity 
								                ELSE 0 
							                END
					                  ) as AdjustmentIn     
			                    , SUM (
						                CASE 
							                WHEN movementtype LIKE 'Adjustment Out' THEN quantity 
							                ELSE 0 
						                END
					                  ) as AdjustmentOut            
			                    , SUM (
						                CASE 
							                WHEN MovementType LIKE '% In' then RemainingQty 
							                ELSE 0 
						                END 
					                   ) as RemainingQTY    
		                     FROM InventoryHdr IH             
			                INNER JOIN InventoryDet ID 
			                   ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo         
		                    INNER JOIN InventoryLocation LI 
			                   ON IH.InventoryLocationID = LI.InventoryLocationID         
		                    WHERE CAST(IH.InventoryLocationID AS VARCHAR(4)) LIKE (
																	                CASE 
																		                WHEN @Location <> 0 THEN CAST(@Location AS VARCHAR(4)) 
																		                ELSE '%' 
																	                END         
																                ) 
		                    GROUP BY ItemNo 
                      ) JQ 
                RIGHT OUTER JOIN Item IT 
                   ON JQ.ItemNo = IT.ItemNo    
                INNER JOIN Category IC ON 
                   IC.CategoryName = IT.CategoryName     
                INNER JOIN itemdepartment IP 
                   on IP.itemdepartmentid = IC.ItemDepartmentId     
                 LEFT JOIN (
		                select itemno
		                     , sum(quantity) as qty 
		                  from orderdet 
		                 where ISNULL(inventoryhdrrefno,'') = ''     
		                   and orderhdrid in (
								                select orderhdrid 
								                  from orderhdr 
								                 where isvoided = 0
							                  ) 
		                 group by itemno
	                  ) OD      
                   on JQ.ItemNo = OD.ItemNo      
                  LEFT JOIN ( 
				                select id.itemno
				                     , avg(costofgoods) as cog 
				                  from inventoryhdr ih
				                     , inventorydet id
					                 , item i 
				                 where ih.inventoryhdrrefno = id.inventoryhdrrefno     
				                   and ih.movementtype like '% In' 
				                   and id.itemno = i.itemno 
				                 group by id.itemno, i.factoryprice
		                    )  CG 
	                on JQ.Itemno = CG.ItemNo 
                 WHERE (IT.Deleted=0 or IT.Deleted is null) 
                   AND IP.ItemDepartmentID <> 'SYSTEM' 
                   AND IsInInventory = 1 
                   AND IT.ItemNo + ItemName + IC.CategoryName + ISNULL(IT.Barcode,'') + ISNULL(ItemDesc,'') + ISNULL(attributes1,'') + ISNULL(attributes2,'') + ISNULL(attributes3,'') + ISNULL(attributes4,'') + ISNULL(attributes5,'') + ISNULL(attributes6,'') + ISNULL(attributes7,'') + ISNULL(attributes8,'') LIKE N'%' + @search + '%' 
                 ORDER BY ItemNo ASC  
                
            ";

            DataTable stockBal = new DataTable();
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            cmd.CommandTimeout = 9999999;
            stockBal.Load(DataService.GetReader(cmd));

            stockBal.Columns.Add("OnHand", System.Type.GetType("System.Int32"));
            stockBal.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
            stockBal.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));

            DataTable undeductedSales = ReportController.FetchUnDeductedSales(InventoryLocationID);

            for (int j = 0; j < stockBal.Rows.Count; j++)
            {
                if (stockBal.Rows[j]["ItemNo"].ToString().Contains("'"))
                    throw new Exception("Item No contains [']. Please remove this special character");

                int stockin = 0, stockout = 0, transferin = 0, transferout = 0, adjustmentin = 0, adjustmentout = 0;
                int.TryParse(stockBal.Rows[j]["Stock In"].ToString(), out stockin);
                int.TryParse(stockBal.Rows[j]["Stock Out"].ToString(), out stockout);
                int.TryParse(stockBal.Rows[j]["Transfer In"].ToString(), out transferin);
                int.TryParse(stockBal.Rows[j]["Transfer Out"].ToString(), out transferout);
                int.TryParse(stockBal.Rows[j]["Adjustment In"].ToString(), out adjustmentin);
                int.TryParse(stockBal.Rows[j]["Adjustment Out"].ToString(), out adjustmentout);
                //int negativeSales = InventoryController.
                //    GetTotalUndeductedSalesByItem(stockBal.Rows[j]["ItemNo"].ToString(),
                //    InventoryLocationID);
                int negativeSales = 0;
                DataRow[] dr = undeductedSales.Select("itemno = '" + stockBal.Rows[j]["ItemNo"] + "'");

                if (dr != null && dr.Length > 0)
                {
                    negativeSales = (int)dr[0]["Quantity"];
                }
                else
                {
                    negativeSales = 0;
                }

                int onHandQty =
                    stockin - stockout + transferin -
                    transferout + adjustmentin - adjustmentout - negativeSales;
                stockBal.Rows[j]["OnHand"] = onHandQty;
                decimal cogs = 0.0M;
                if (displayCostPrice)
                {
                    cogs =
                        InventoryController.FetchAverageCostOfGoodsLeftByItemNo
                        (onHandQty, stockBal.Rows[j]["ItemNo"].ToString(), InventoryLocationID);

                }
                stockBal.Rows[j]["CostOfGoods"] = cogs;
                stockBal.Rows[j]["TotalCost"] = cogs * onHandQty;
            }

            return stockBal;
        }
        #endregion

        #region *) Fetch Stock Report NEW
        public static DataTable FetchStockReportItemSummary_FIFO
            (string searchQuery, int InventoryLocationID, bool displayCostPrice, string DeptID, string SortColumn, string SortDir)
        {

            if (searchQuery == "") { searchQuery = "%"; }
            if (DeptID == "0") { DeptID = ""; }
            string tmp = "%" + searchQuery + "%";
            string searchSQL = " AND c.ItemNo+ItemName+c.Barcode+ISNULL(attributes1,'')+ISNULL(attributes2,'')+ISNULL(attributes3,'')+ " +
                         "ISNULL(attributes4,'')+ISNULL(attributes5,'')+ISNULL(attributes6,'')+ISNULL(attributes7,'')+ISNULL(attributes8,'')+ " +
                         "ISNULL(ItemDesc,'')+ISNULL(departmentname,'') like '%' + @search + '%' ";
            if (searchQuery == "") searchSQL = "";
            
            if (SortColumn == "") SortColumn = "ItemNo";
            if (SortDir == "") SortDir = "ASC";
            string InventoryLocationName = new InventoryLocation(InventoryLocationID).InventoryLocationName;

            //Get inventoryLocationID from InventoryLocationName
            DataTable dt = new DataTable();
            try
            {
                string SQL = "DECLARE @search AS NVARCHAR(200); " +
                            "DECLARE @inventorylocationName int; " +
                            "SET @search = '" + searchQuery + "' " +
                            "SET @inventorylocationName = " + InventoryLocationID.ToString() + " " +
                            "SELECT c.FactoryPrice, c.RetailPrice, f.departmentname,c.itemno,c.categoryname " + 
                            ", itemname = replace(c.itemname, '\"', '') " +
                            ", SUM(ISNULL(ISM.BalanceQty,0)) AS OnHand " +
                            ", SUM(ISNULL(ISM.CostPrice,0)) AS CostOfGoods " +
		                    ", SUM(ISNULL(ISM.BalanceQty,0) * ISNULL(ISM.CostPrice,0))  AS TotalCost " + 
                            ", c.Userfld1 as UOM   " +
                            ", Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8 " +
                            "FROM Item c " +
                                "LEFT JOIN ItemSummary iSM ON c.itemno = ISM.ItemNo " +
                                "INNER JOIN Category d ON d.categoryname = c.CategoryName " +
                                "INNER JOIN itemdepartment f on d.itemdepartmentid = f.itemdepartmentid " +
                            "WHERE " +
                            "ISNULL(c.deleted,0) = 0 AND f.ItemDepartmentID <> 'SYSTEM' AND c.IsInInventory = 1 " +
                                "AND CAST(ISNULL(ISM.inventoryLocationID,0) AS VARCHAR(MAX)) " +
                                "like CASE WHEN @inventorylocationName =0 THEN '%' ELSE CAST(@InventoryLocationName as VARCHAR(MAX)) END " + 
                                searchSQL +
                            " GROUP BY c.FactoryPrice, c.RetailPrice, f.departmentname,c.itemno,c.categoryname , " +
                            " replace(c.itemname, '\"', '') , " +
                            "c.Userfld1 " +
                            ", Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8 " +
                            "order by " + SortColumn + " " + SortDir;

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                cmd.CommandTimeout = 9999999;
                DataTable stockBal = DataService.GetDataSet(cmd).Tables[0];

                return stockBal;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        /**Single Item*/
        public static DataTable FetchStockReportItemSummarySingleItem_FIFO(string ItemNo, int InventoryLocationID, bool displayCostPrice)
        {
            string InventoryLocationName = new InventoryLocation(InventoryLocationID).InventoryLocationName;

            //Get inventoryLocationID from InventoryLocationName
            DataTable dt = new DataTable();
            try
            {
                string SQL = "DECLARE @search AS NVARCHAR(200); " +
                            "DECLARE @inventorylocationName int; " +
                            "SET @inventorylocationName = " + InventoryLocationID.ToString() + " " +
                            "SELECT c.FactoryPrice, c.RetailPrice, f.departmentname,c.itemno,c.categoryname " +
                            ", itemname = replace(c.itemname, '\"', '') " +
                            ", SUM(ISNULL(ISM.BalanceQty,0)) AS OnHand " +
                            ", SUM(ISNULL(ISM.CostPrice,0)) AS CostOfGoods " +
                            ", SUM(ISNULL(ISM.BalanceQty,0) * ISNULL(ISM.CostPrice,0))  AS TotalCost " +
                            ", c.Userfld1 as UOM   " +
                            ", Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8 " +
                            "FROM Item c " +
                                "LEFT JOIN ItemSummary iSM ON c.itemno = ISM.ItemNo AND CAST(ISNULL(ISM.inventoryLocationID,0) AS VARCHAR(MAX)) " +            
                                "LIKE CASE WHEN @inventorylocationName =0 THEN '%' ELSE CAST(@inventorylocationName as VARCHAR(MAX)) END " +
                                "INNER JOIN Category d ON d.categoryname = c.CategoryName " +
                                "INNER JOIN itemdepartment f on d.itemdepartmentid = f.itemdepartmentid " +
                            "WHERE " +
                            "ISNULL(c.deleted,0) = 0 AND f.ItemDepartmentID <> 'SYSTEM' AND c.IsInInventory = 1 AND c.ItemNo = '" + ItemNo + "' " +
                            " GROUP BY c.FactoryPrice, c.RetailPrice, f.departmentname,c.itemno,c.categoryname , " +
                            " replace(c.itemname, '\"', '') , " +
                            "c.Userfld1 " +
                            ", Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8 ";
                            

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                cmd.CommandTimeout = 9999999;
                DataTable stockBal = DataService.GetDataSet(cmd).Tables[0];

                return stockBal;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        #endregion
        public static decimal FetchCostOfGoodsAvg(string itemNo, int InventoryLocationID)
        {
            decimal cost = 0;

            string InventoryLocationName = new InventoryLocation(InventoryLocationID).InventoryLocationName;

            string SQL =
                "DECLARE @search AS NVARCHAR(200); " +
                "DECLARE @inventorylocationName AS NVARCHAR(200); " +
                "SET @search = '' " +
                "SET @inventorylocationName = '" + InventoryLocationName + "' " +
                "SELECT IT.RetailPrice, IP.DepartmentName,IT.ItemNo,IC.CategoryName, ItemName = Replace(IT.ItemName, '\"', '') " +
                    ", [Stock In], [Stock Out], [Transfer In], [Transfer Out], [Adjustment In], [Adjustment Out] " +
                    ", Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8, ISNULL(userflag1,'false') as [Userflag1]  " +
                "FROM " +
                    "( " +
                        "SELECT ItemNo " +
                            ", SUM(CASE WHEN movementtype LIKE 'Stock In' THEN quantity ELSE 0 END) as [Stock In] " +
                            ", SUM(CASE WHEN movementtype LIKE 'Stock Out' THEN quantity ELSE 0 END) as [Stock Out] " +
                            ", SUM(CASE WHEN movementtype LIKE 'Transfer In' THEN quantity ELSE 0 END) as [Transfer In] " +
                            ", SUM(CASE WHEN movementtype LIKE 'Transfer Out' THEN quantity ELSE 0 END) as [Transfer Out] " +
                            ", SUM(CASE WHEN movementtype LIKE 'Adjustment In' THEN quantity ELSE 0 END) as [Adjustment In] " +
                            ", SUM(CASE WHEN movementtype LIKE 'Adjustment Out' THEN quantity ELSE 0 END) as [Adjustment Out] " +
                        "FROM InventoryHdr IH " +
                            "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
                            "INNER JOIN InventoryLocation LI ON IH.InventoryLocationID = LI.InventoryLocationID " +
                        "WHERE ISNULL(InventoryLocationName,'') like '%' + @inventorylocationName + '%' " +
                        "GROUP BY ItemNo " +
                    ") JQ " +
                    "RIGHT OUTER JOIN Item IT ON JQ.ItemNo = IT.ItemNo " +
                    "INNER JOIN Category IC ON IC.CategoryName = IT.CategoryName " +
                    "INNER JOIN itemdepartment IP on IP.itemdepartmentid = IC.ItemDepartmentId " +
                "WHERE (IT.Deleted=0 or IT.Deleted is null) " +
                    "AND IP.ItemDepartmentID <> 'SYSTEM' AND IsInInventory = 1 " +
                    "AND IT.ItemNo = '" + itemNo + "'";

            DataTable stockBal = new DataTable();
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            cmd.CommandTimeout = 9999999;
            stockBal.Load(DataService.GetReader(cmd));

            stockBal.Columns.Add("OnHand", System.Type.GetType("System.Int32"));
            stockBal.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
            stockBal.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));

            DataTable undeductedSales = ReportController.FetchUnDeductedSales(InventoryLocationID);

            for (int j = 0; j < stockBal.Rows.Count; j++)
            {
                if (stockBal.Rows[j]["ItemNo"].ToString().Contains("'"))
                    throw new Exception("Item No contains [']. Please remove this special character");

                int stockin = 0, stockout = 0, transferin = 0, transferout = 0, adjustmentin = 0, adjustmentout = 0;
                int.TryParse(stockBal.Rows[j]["Stock In"].ToString(), out stockin);
                int.TryParse(stockBal.Rows[j]["Stock Out"].ToString(), out stockout);
                int.TryParse(stockBal.Rows[j]["Transfer In"].ToString(), out transferin);
                int.TryParse(stockBal.Rows[j]["Transfer Out"].ToString(), out transferout);
                int.TryParse(stockBal.Rows[j]["Adjustment In"].ToString(), out adjustmentin);
                int.TryParse(stockBal.Rows[j]["Adjustment Out"].ToString(), out adjustmentout);
                //int negativeSales = InventoryController.
                //    GetTotalUndeductedSalesByItem(stockBal.Rows[j]["ItemNo"].ToString(),
                //    InventoryLocationID);
                int negativeSales = 0;
                DataRow[] dr = undeductedSales.Select("itemno = '" + stockBal.Rows[j]["ItemNo"] + "'");

                if (dr != null && dr.Length > 0)
                {
                    negativeSales = (int)dr[0]["Quantity"];
                }
                else
                {
                    negativeSales = 0;
                }

                int onHandQty =
                    stockin - stockout + transferin -
                    transferout + adjustmentin - adjustmentout - negativeSales;
                stockBal.Rows[j]["OnHand"] = onHandQty;
                cost = 0.0M;
                cost =
                    InventoryController.FetchAverageCostOfGoodsLeftByItemNo
                    (onHandQty, stockBal.Rows[j]["ItemNo"].ToString(), InventoryLocationID);
                stockBal.Rows[j]["CostOfGoods"] = cost;
                stockBal.Rows[j]["TotalCost"] = cost * onHandQty;
            }

            return cost;
        }

        public static DataTable FetchStockReportWithMinimumQty(
            string searchQuery,
            int InventoryLocationID,
            string SortColumn,
            string SortDir
            )
        {

            if (searchQuery == "") { searchQuery = "%"; }
            string tmp = "%" + searchQuery + "%";
            string searchSQL = "  ISNULL(c.ItemNo,'')+ISNULL(c.ItemName,'')+c.Barcode+ISNULL(attributes1,'')+ISNULL(attributes2,'')+ISNULL(attributes3,'')+ " +
                         "ISNULL(attributes4,'')+ISNULL(attributes5,'')+ISNULL(attributes6,'')+ISNULL(attributes7,'')+ISNULL(attributes8,'')+ " +
                         "ISNULL(ItemDesc,'')+ISNULL(d.ItemDepartmentId,'') like '%' + @search + '%' ";
            if (searchQuery == "") searchSQL = "";


            if (SortColumn == "") SortColumn = "ItemNo";
            if (SortDir == "") SortDir = "ASC";
            string InventoryLocationName = new InventoryLocation(InventoryLocationID).InventoryLocationName;

            //Get inventoryLocationID from InventoryLocationName

            string SQL = "DECLARE @search AS NVARCHAR(200); "+
                "DECLARE @inventorylocationName AS NVARCHAR(200); "+
                "SET @search = '"+searchQuery +"' "+
                "SET @inventorylocationName = '" + InventoryLocationName + "' " +
                "SELECT m.* "+
	                ", m.Quantity AS [On Hand] "+
	                ", ISNULL(MinQty,0) AS MinQty "+
                "FROM ( "+
                    "SELECT ItemNo, CategoryName, DepartmentName, ItemName, Attributes1, Quantity " +
		            "FROM ( "+
                        "SELECT c.ItemNo, c.CategoryName, DepartmentName, c.ItemName, c.Attributes1 " +
                            ", CASE WHEN @inventorylocationName = '' THEN ISNULL(c.BalanceQuantity,0) ELSE isnull(a.BalanceQty,0) END AS Quantity " +
                            ", CASE WHEN @inventorylocationName = '' THEN ISNULL(c.AvgCostPrice, c.FactoryPrice) ELSE ISNULL(a.CostPrice,0) END as CostOfGoods " +
                            ", CASE WHEN @inventorylocationName = '' THEN ISNULL(c.AvgCostPrice, c.FactoryPrice) * ISNULL(c.BalanceQuantity,0) ELSE isnull(a.BalanceQty,0) * ISNULL(a.CostPrice,0) END as TotalCost   " +
                        "FROM Item c   " +
                            "LEFT OUTER JOIN ItemSummary a ON c.ItemNo = a.ItemNo  " +
                            "LEFT OUTER JOIN Category d ON d.categoryname = c.CategoryName  " +
                            "LEFT OUTER JOIN ItemDepartment ON d.ItemDepartmentID = ItemDepartment.ItemDepartmentID " +
                            "LEFT OUTER JOIN InventoryLocation e  on a.InventoryLocationID = e.InventoryLocationID  " +
                        "WHERE (c.Deleted IS NULL OR c.Deleted = 0 ) AND ISNULL(e.InventoryLocationName,'') LIKE '%' + @inventorylocationName + '%' AND " +
                            searchSQL + " " +
		                //"GROUP BY c.ItemNo, c.CategoryName, DepartmentName, c.ItemName, MovementType  "+
	                ") dt "+
	            ") m  "+
                "LEFT OUTER JOIN ( " +
	                "SELECT x.ItemNo, MIN(TriggerQuantity) AS MinQty "+
	                "FROM ItemQuantityTrigger x  "+
		                "INNER JOIN InventoryLocation ON x.InventoryLocationID = InventoryLocation.InventoryLocationID "+
                        "INNER JOIN Item c ON x.ItemNo = c.ItemNo " +
                        "INNER JOIN Category d ON c.CategoryName = d.CategoryName "+
                    "WHERE x.Deleted IS NULL OR x.Deleted = 0  " +
                        "AND InventoryLocationName LIKE '%' + @inventorylocationName + '%' AND" +
                    searchSQL + " " +
                    "GROUP BY x.ItemNo  " +
                ") o ON m.ItemNo = o.ItemNo  " +
                "WHERE m.Quantity < ISNULL(MinQty,0) " +
                "ORDER BY [" + SortColumn + "] " + SortDir;

            DataTable stockBal=new DataTable();
            stockBal.Load(DataService.GetReader(new QueryCommand(SQL, "PowerPOS")));

            //stockBal.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
            //stockBal.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));

            /*for (int j = 0; j < stockBal.Rows.Count; j++)
            {


                int onHandQty = int.Parse(stockBal.Rows[j]["On Hand"].ToString());
                decimal cogs = InventoryController.FetchAverageCostOfGoodsLeftByItemNo(onHandQty, stockBal.Rows[j]["ItemNo"].ToString(), InventoryLocationID);
                stockBal.Rows[j]["CostOfGoods"] = cogs;
                stockBal.Rows[j]["TotalCost"] = cogs * onHandQty;
            }*/


            return stockBal;
        }
        /*Optimize Version with SP*/
        public static DataTable FetchStockReportBreakdownByLocationWithSP(string searchQuery)
        {
            try
            {
                string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (CostingMethod == null) CostingMethod = "";
                CostingMethod = CostingMethod.ToLower();

                DataTable finalResult = SPs.FetchStockReportBreakdownByLocation(searchQuery, CostingMethod).GetDataSet().Tables[0];

                finalResult.Columns.Add("Total", System.Type.GetType("System.Int32"));
                int lineTotal;
                for (int j = 0; j < finalResult.Rows.Count; j++)
                {
                    lineTotal = 0;
                    //for each item, find the stock balances and update the appropriate column
                    for (int i = 4; i < finalResult.Columns.Count - 1; i++)
                    {
                        lineTotal += Int32.Parse(finalResult.Rows[j][i].ToString());
                    }
                    finalResult.Rows[j]["Total"] = lineTotal;
                }

                return finalResult;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchStockReportBreakdownByLocationItemSummaryWithSP(string searchQuery)
        {
            try
            {
                string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (CostingMethod == null) CostingMethod = "";
                CostingMethod = CostingMethod.ToLower();

                DataTable finalResult = SPs.FetchStockReportBreakdownByLocationItemSummary(searchQuery, CostingMethod).GetDataSet().Tables[0];

                finalResult.Columns.Add("Total",typeof(decimal));
                decimal lineTotal;
                for (int j = 0; j < finalResult.Rows.Count; j++)
                {
                    lineTotal = 0;
                    //for each item, find the stock balances and update the appropriate column
                    for (int i = 6; i < finalResult.Columns.Count - 1; i++)
                    {
                        lineTotal += (finalResult.Rows[j][i]+"").GetDecimalValue();
                    }
                    finalResult.Rows[j]["Total"] = lineTotal;
                }

                foreach (DataColumn col in finalResult.Columns)
                {
                    if (col.ColumnName.ToLower() == "attributes1")
                    {
                        AttributesLabel al = new AttributesLabel(1);
                        if (al != null && !String.IsNullOrEmpty(al.Label))
                        {
                            col.ColumnName = al.Label;
                        }
                    }
                }

                return finalResult;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchStockReportBreakdownByLocationItemSummaryWithSPWithRemoveZeroQty(bool removeZeroQty,string searchQuery)
        {
            try
            {
                string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (CostingMethod == null) CostingMethod = "";
                CostingMethod = CostingMethod.ToLower();

                DataTable finalResult = SPs.FetchStockReportBreakdownByLocationItemSummary(searchQuery, CostingMethod).GetDataSet().Tables[0];

                finalResult.Columns.Add("Total", typeof(decimal));
                decimal lineTotal;
                for (int j = 0; j < finalResult.Rows.Count; j++)
                {
                    lineTotal = 0;
                    //for each item, find the stock balances and update the appropriate column
                    for (int i = 5; i < finalResult.Columns.Count - 1; i++)
                    {
                        lineTotal += (finalResult.Rows[j][i] + "").GetDecimalValue();
                    }
                    finalResult.Rows[j]["Total"] = lineTotal;
                }

                if (removeZeroQty)
                {
                    DataSet ds = new DataSet();
                    DataTable finalResultwithoutzero = finalResult.Select("Total > 0").CopyToDataTable();
                    ds.Tables.Add(finalResultwithoutzero);
                    return ds.Tables[0];
                    
                }

                return finalResult;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }


        public static DataTable FetchStockReportBreakdownByLocation(
                   string searchQuery,
                    bool displayCostPrice,
                   string DeptID,
                   string SortColumn,
                   string SortDir
                )
        {
            try
            {
                string searchSQL = " AND a.ItemNo+a.ItemName+a.Barcode+ISNULL(attributes1,'')+ISNULL(attributes2,'')+ISNULL(attributes3,'')+ " +
                         "ISNULL(attributes4,'')+ISNULL(attributes5,'')+ISNULL(attributes6,'')+ISNULL(attributes7,'')+ISNULL(attributes8,'')+ " +
                         "ISNULL(ItemDesc,'')+ISNULL(c.departmentname,'') like '%" + searchQuery + "%' ";
            
                InventoryLocationCollection invLoc = new InventoryLocationCollection();
                invLoc.Where(InventoryLocation.Columns.Deleted, false);
                invLoc.Load();

                Logger.writeLog("Load Inventory Location done");

                //Load Product List
                DataTable[] stockOnHand = new DataTable[invLoc.Count];
                for (int i = 0; i < invLoc.Count; i++)
                {
                    stockOnHand[i] = new DataTable();
                    stockOnHand[i].TableName = invLoc[i].InventoryLocationName.Trim(System.IO.Path.GetInvalidFileNameChars()).Replace(" ", "");
                    stockOnHand[i] = ReportController.FetchStockReport(searchQuery, invLoc[i].InventoryLocationID, false, DeptID, SortColumn, SortDir);
                }

                Logger.writeLog("Load Product List done");

                //left outer join of all the stock balances
                string sql = "select c.ItemDepartmentID as Department,b.CategoryName as Category,a.ItemNo as ItemNo,Replace(a.ItemName, '\"', '') as ItemName from item a inner join category b  on a.categoryname = b.categoryname inner join itemdepartment c on c.itemdepartmentid = b.itemdepartmentid where a.deleted=0 and isininventory=1 " + searchSQL + " order by  c.ItemDepartmentID,b.CategoryName,a.ItemNo,a.ItemName";
                DataTable finalResult = new DataTable();
                finalResult.TableName = "Result";
                finalResult = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS")).Tables[0];
                for (int i = 0; i < invLoc.Count; i++)
                {
                    DataColumn d = new DataColumn(invLoc[i].InventoryLocationName.Trim(System.IO.Path.GetInvalidFileNameChars()).Replace(" ", ""), System.Type.GetType("System.Int32"));
                    d.DefaultValue = 0;
                    finalResult.Columns.Add(d);
                }

                finalResult.Columns.Add("Total", System.Type.GetType("System.Int32"));
                int lineTotal;
                for (int j = 0; j < finalResult.Rows.Count; j++)
                {
                    lineTotal = 0;
                    //for each item, find the stock balances and update the appropriate column
                    for (int i = 0; i < invLoc.Count; i++)
                    {
                        DataRow[] dr = stockOnHand[i].Select("ItemNo = '" + 
                            finalResult.Rows[j]["ItemNo"].ToString() + "'");
                        
                        if (dr.Length > 0 && dr[0]["OnHand"] != null)
                        {
                            finalResult.Rows[j][invLoc[i].InventoryLocationName.Trim
                                (System.IO.Path.GetInvalidFileNameChars()).Replace(" ", "")]
                                = dr[0]["OnHand"];
                            lineTotal += (int)dr[0]["OnHand"];
                        }                        
                    }
                    finalResult.Rows[j]["Total"] = lineTotal;
                }

                Logger.writeLog("Left outer join stock balances done");
                return finalResult;
 
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        //public static DataTable FetchStockReportBreakdownByLocationOld(
        //   string searchQuery,
        //    bool displayCostPrice,
        //   string DeptID,
        //   string SortColumn,
        //   string SortDir
        //)
        //{

        //    if (searchQuery == "") { searchQuery = "%"; }

        //    if (DeptID == "0") { DeptID = ""; }
        //    string tmp = "%" + searchQuery + "%";
        //    string searchSQL = " c.ItemNo+ItemName+c.Barcode+ISNULL(attributes1,'')+ISNULL(attributes2,'')+ISNULL(attributes3,'')+ " +
        //                 "ISNULL(attributes4,'')+ISNULL(attributes5,'')+ISNULL(attributes6,'')+ISNULL(attributes7,'')+ISNULL(attributes8,'')+ " +
        //                 "ISNULL(ItemDesc,'')+ISNULL(departmentname,'') like '%' + @search + '%' ";
        //    if (searchQuery == "") searchSQL = "";


        //    if (SortColumn == "") SortColumn = "ItemNo";
        //    if (SortDir == "") SortDir = "ASC";

        //    string SQL = "declare @search as nvarchar(200); " +
        //                "set @search = '" + searchQuery + "' " +
        //                "select a.inventorylocationid,f.departmentname,c.categoryname, c.itemno,c.itemname,isnull(movementtype,'Stock In') as MovementType, SUM(isnull(quantity,0)) as Quantity  " +
        //                "from inventoryhdr a  " +
        //                "inner join inventorydet b " +
        //                "on a.inventoryhdrrefno = b.inventoryhdrrefno " +
        //                "right outer join Item c  " +
        //                "on c.ItemNo = b.itemno  " +
        //                "inner join Category d on " +
        //                "d.categoryname = c.CategoryName " +
        //                "inner join InventoryLocation e  " +
        //                "on a.InventoryLocationID = e.inventorylocationid " +
        //                "inner join itemdepartment f on d.itemdepartmentid = f.itemdepartmentid " +
        //                "where  " + searchSQL +
        //                "group by a.inventorylocationid,f.departmentname,c.categoryname,c.itemno, c.itemname,MovementType " +
        //                "order by a.inventorylocationid, movementtype," + SortColumn + " " + SortDir;

        //    DataTable stockBal = Pivot(DataService.GetReader(new QueryCommand(SQL, "PowerPOS")), "ItemNo", "MovementType", "Quantity");
        //    if (!stockBal.Columns.Contains("Stock In")) stockBal.Columns.Add("Stock In", System.Type.GetType("System.Int32"));
        //    if (!stockBal.Columns.Contains("Stock Out")) stockBal.Columns.Add("Stock Out", System.Type.GetType("System.Int32"));
        //    if (!stockBal.Columns.Contains("Transfer In")) stockBal.Columns.Add("Transfer In", System.Type.GetType("System.Int32"));
        //    if (!stockBal.Columns.Contains("Transfer Out")) stockBal.Columns.Add("Transfer Out", System.Type.GetType("System.Int32"));
        //    if (!stockBal.Columns.Contains("Adjustment In")) stockBal.Columns.Add("Adjustment In", System.Type.GetType("System.Int32"));
        //    if (!stockBal.Columns.Contains("Adjustment Out")) stockBal.Columns.Add("Adjustment Out", System.Type.GetType("System.Int32"));

        //    stockBal.Columns.Add("OnHand", System.Type.GetType("System.Int32"));
            
        //    for (int j = 0; j < stockBal.Rows.Count; j++)
        //    {
        //        int inventoryLocationID = -1, stockin = 0, stockout = 0, transferin = 0, transferout = 0, adjustmentin = 0, adjustmentout = 0;
        //        int.TryParse(stockBal.Rows[j]["Stock In"].ToString(), out stockin);
        //        int.TryParse(stockBal.Rows[j]["Stock Out"].ToString(), out stockout);
        //        int.TryParse(stockBal.Rows[j]["Transfer In"].ToString(), out transferin);
        //        int.TryParse(stockBal.Rows[j]["Transfer Out"].ToString(), out transferout);
        //        int.TryParse(stockBal.Rows[j]["Adjustment In"].ToString(), out adjustmentin);
        //        int.TryParse(stockBal.Rows[j]["Adjustment Out"].ToString(), out adjustmentout);
        //        int.TryParse(stockBal.Rows[j]["InventoryLocationID"].ToString(), out inventoryLocationID);

        //        int negativeSales = InventoryController.
        //           GetTotalUndeductedSalesByItem(stockBal.Rows[j]["ItemNo"].ToString(),
        //            inventoryLocationID);
        //        //int negativeSales = 0;
        //        //DataRow[] dr = undeductedSales.Select("itemno = '" + stockBal.Rows[j]["ItemNo"] + "'");
        //        //if (dr != null && dr.Length > 0)
        //        //{
        //            //negativeSales = (int)dr[0]["Quantity"];
        //        //}
        //        //else
        //        //{
        //            //negativeSales = 0;
        //        //}
                
        //        int onHandQty =
        //            stockin - stockout + transferin -
        //            transferout + adjustmentin - adjustmentout - negativeSales;
        //        stockBal.Rows[j]["OnHand"] = onHandQty;


        //    }

        //    DataTable result = Pivot(stockBal, "ItemNo", "InventoryLocationID", "OnHand");
        //    if (result != null)
        //    {
        //        result.Columns.Remove("Stock In");
        //        result.Columns.Remove("Stock Out");
        //        result.Columns.Remove("Transfer In");
        //        result.Columns.Remove("Transfer Out");
        //        result.Columns.Remove("Adjustment In");
        //        result.Columns.Remove("Adjustment Out");
        //        result.TableName = "Result";
        //        if (result.Columns.Count > 3)
        //        {
        //            result.Columns[0].ColumnName = "Department";
        //            result.Columns[1].ColumnName = "Category";
        //            result.Columns[2].ColumnName = "Item No";
        //            result.Columns[3].ColumnName = "Item Name";

        //            for (int p = 4; p < result.Columns.Count; p++)
        //            {
        //                int locID;
        //                if (int.TryParse(result.Columns[p].ColumnName, out locID))
        //                {
        //                    //get inventory location name from loc ID
        //                    InventoryLocation v = new InventoryLocation(locID);
        //                    if (v.IsLoaded && !v.IsNew)
        //                    {
        //                        if (!result.Columns.Contains(v.InventoryLocationName))
        //                        {
        //                            result.Columns[p].ColumnName = v.InventoryLocationName;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    return result;
        //}
        
        //public static DataTable FetchStockOrderReport(
        //    int selectedYear,
        //    int selectedMonth,
        //    int NumOfMonthStock,
        //    string DeptID
        //)
        //{

        //    if (searchQuery == "") { searchQuery = "%"; }
            
        //    if (DeptID == "0") { DeptID = ""; }
        //    string tmp = "%" + searchQuery + "%";
        //    string searchSQL = " c.ItemNo+ItemName+ISNULL(attributes1,'')+ISNULL(attributes2,'')+ISNULL(attributes3,'')+ " +
        //                 "ISNULL(attributes4,'')+ISNULL(attributes5,'')+ISNULL(attributes6,'')+ISNULL(attributes7,'')+ISNULL(attributes8,'')+ " +
        //                 "ISNULL(ItemDesc,'')+ISNULL(departmentname,'') like '%' + @search + '%' ";
        //    if (searchQuery == "") searchSQL = "";


        //    if (SortColumn == "") SortColumn = "ItemNo";
        //    if (SortDir == "") SortDir = "ASC";
            
        //    string SQL = "declare @search as nvarchar(200); " +                        
        //                "set @search = '" + searchQuery + "' " +
        //                "select a.inventorylocationid,f.departmentname,c.categoryname, c.itemno,c.itemname,isnull(movementtype,'Stock In') as MovementType, SUM(isnull(quantity,0)) as Quantity  " +
        //                "from inventoryhdr a  " +
        //                "inner join inventorydet b " +
        //                "on a.inventoryhdrrefno = b.inventoryhdrrefno " +
        //                "right outer join Item c  " +
        //                "on c.ItemNo = b.itemno  " +
        //                "inner join Category d on " +
        //                "d.categoryname = c.CategoryName " +
        //                "inner join InventoryLocation e  " +
        //                "on a.InventoryLocationID = e.inventorylocationid " +
        //                "inner join itemdepartment f on d.itemdepartmentid = f.itemdepartmentid " +
        //                "where  " + searchSQL +
        //                "group by a.inventorylocationid,f.departmentname,c.categoryname,c.itemno, c.itemname,MovementType " +
        //                "order by a.inventorylocationid, movementtype," + SortColumn + " " + SortDir;

        //    DataTable stockBal = Pivot(DataService.GetReader(new QueryCommand(SQL, "PowerPOS")), "ItemNo", "MovementType", "Quantity");
        //    if (!stockBal.Columns.Contains("Stock In")) stockBal.Columns.Add("Stock In", System.Type.GetType("System.Int32"));
        //    if (!stockBal.Columns.Contains("Stock Out")) stockBal.Columns.Add("Stock Out", System.Type.GetType("System.Int32"));
        //    if (!stockBal.Columns.Contains("Transfer In")) stockBal.Columns.Add("Transfer In", System.Type.GetType("System.Int32"));
        //    if (!stockBal.Columns.Contains("Transfer Out")) stockBal.Columns.Add("Transfer Out", System.Type.GetType("System.Int32"));
        //    if (!stockBal.Columns.Contains("Adjustment In")) stockBal.Columns.Add("Adjustment In", System.Type.GetType("System.Int32"));
        //    if (!stockBal.Columns.Contains("Adjustment Out")) stockBal.Columns.Add("Adjustment Out", System.Type.GetType("System.Int32"));

        //    stockBal.Columns.Add("OnHand", System.Type.GetType("System.Int32"));
            
        //    for (int j = 0; j < stockBal.Rows.Count; j++)
        //    {
        //        int inventoryLocationID = -1, stockin = 0, stockout = 0, transferin = 0, transferout = 0, adjustmentin = 0, adjustmentout = 0;
        //        int.TryParse(stockBal.Rows[j]["Stock In"].ToString(), out stockin);
        //        int.TryParse(stockBal.Rows[j]["Stock Out"].ToString(), out stockout);
        //        int.TryParse(stockBal.Rows[j]["Transfer In"].ToString(), out transferin);
        //        int.TryParse(stockBal.Rows[j]["Transfer Out"].ToString(), out transferout);
        //        int.TryParse(stockBal.Rows[j]["Adjustment In"].ToString(), out adjustmentin);
        //        int.TryParse(stockBal.Rows[j]["Adjustment Out"].ToString(), out adjustmentout);
        //        int.TryParse(stockBal.Rows[j]["InventoryLocationID"].ToString(), out inventoryLocationID);
                
        //        int negativeSales = InventoryController.
        //            GetTotalUndeductedSalesByItem(stockBal.Rows[j]["ItemNo"].ToString(),
        //            inventoryLocationID);


        //        int onHandQty =
        //            stockin - stockout + transferin -
        //            transferout + adjustmentin - adjustmentout - negativeSales;
        //        stockBal.Rows[j]["OnHand"] = onHandQty;
                
                
        //    }

        //    DataTable result = Pivot(stockBal, "ItemNo", "InventoryLocationID", "OnHand");
        //    if (result != null)
        //    {
        //        result.Columns.Remove("Stock In");
        //        result.Columns.Remove("Stock Out");
        //        result.Columns.Remove("Transfer In");
        //        result.Columns.Remove("Transfer Out");
        //        result.Columns.Remove("Adjustment In");
        //        result.Columns.Remove("Adjustment Out");
        //        result.TableName = "Result";
        //        if (result.Columns.Count > 3)
        //        {
        //            result.Columns[0].ColumnName = "Department";
        //            result.Columns[1].ColumnName = "Category";
        //            result.Columns[2].ColumnName = "Item No";
        //            result.Columns[3].ColumnName = "Item Name";

        //            for (int p = 4; p < result.Columns.Count; p++)
        //            {
        //                int locID;
        //                if (int.TryParse(result.Columns[p].ColumnName, out locID))
        //                {
        //                    //get inventory location name from loc ID
        //                    InventoryLocation v = new InventoryLocation(locID);
        //                    if (v.IsLoaded && !v.IsNew)
        //                    {
        //                        if (!result.Columns.Contains(v.InventoryLocationName))
        //                        {
        //                            result.Columns[p].ColumnName = v.InventoryLocationName;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    return result;
        //}
     public static DataTable Pivot(DataTable dataValues, string keyColumn, string pivotNameColumn, string pivotValueColumn)
        {
            try
            {
                DataTable tmp = new DataTable();
                DataRow r;
                string LastKey = "//dummy//";
                int i, pValIndex, pNameIndex;
                string s;
                bool FirstRow = true;

                pValIndex = dataValues.Columns[pivotValueColumn].Ordinal;
                pNameIndex = dataValues.Columns[pivotNameColumn].Ordinal;

                for (i = 0; i <= dataValues.Columns.Count - 1; i++)
                {
                    if (i != pValIndex && i != pNameIndex)
                        tmp.Columns.Add(dataValues.Columns[i].ColumnName, dataValues.Columns[i].DataType);
                }

                r = tmp.NewRow();

                foreach (DataRow row1 in dataValues.Rows)
                {
                    if (row1[keyColumn].ToString() != LastKey)
                    {
                        if (!FirstRow)
                            tmp.Rows.Add(r);

                        r = tmp.NewRow();
                        FirstRow = false;

                        //loop thru fields of row1 and populate tmp table
                        for (i = 0; i <= row1.ItemArray.Length - 3; i++)
                            r[i] = row1[tmp.Columns[i].ToString()];

                        LastKey = row1[keyColumn].ToString();
                    }

                    s = row1[pNameIndex].ToString();

                    if (!tmp.Columns.Contains(s))
                        tmp.Columns.Add(s, dataValues.Columns[pNameIndex].DataType);
                    r[s] = row1[pValIndex];
                }

                //add that final row to the datatable:
                tmp.Rows.Add(r);

                return tmp;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
    }

     public static DataTable FetchPurchaseOrderHeader
      (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
      string UserName, string InventoryLocationID, string movementType, string remark,
      string SortColumn, string SortDir, bool showGoodsReceive)
     {
         string sqlString =
             "SELECT PurchaseOrderHdr.PurchaseOrderHdrRefNo, PurchaseOrderHdr.PurchaseOrderDate, PurchaseOrderHdr.UserName, " +
                 "PurchaseOrderHdr.ExchangeRate, " +
                 "PurchaseOrderHdr.Supplier, PurchaseOrderHdr.FreightCharge, PurchaseOrderHdr.DeliveryCharge,  " +
                 "PurchaseOrderHdr.Discount, PurchaseOrderHdr.Remark, PurchaseOrderHdr.InventoryLocationID, " +
                 "PurchaseOrderHdr.CreatedOn, PurchaseOrderHdr.ModifiedOn, PurchaseOrderHdr.CreatedBy, PurchaseOrderHdr.ModifiedBy, " +
                 "PurchaseOrderHdr.UniqueID, " +
                 "InventoryLocation.InventoryLocationName, " +
                 "CustomRefNo = CASE WHEN ISNULL(PurchaseOrderHdr.CustomRefNo, '') <> '' THEN PurchaseOrderHdr.CustomRefNo ELSE PurchaseOrderHdr.PurchaseOrderHdrRefNo END " +

             "FROM PurchaseOrderHdr,  " +
                 "InventoryLocation   " +

             "WHERE PurchaseOrderHdr.InventoryLocationID = InventoryLocation.InventoryLocationID AND ISNULL(PurchaseOrderHdr.Deleted,0) = 0 ";

         if (useStartDate & useEndDate)
         {
             sqlString += "AND (PurchaseOrderHdr.PurchaseOrderDate BETWEEN '" + StartDate.ToString("dd MMM yyyy HH:mm:ss") + "' AND '" + EndDate.ToString("dd MMM yyyy HH:mm:ss") + "') ";
         }
         else if (useStartDate)
         {
             sqlString += "AND PurchaseOrderHdr.PurchaseOrderDate >= '" + StartDate.ToString("dd MMM yyyy HH:mm:ss") + "' ";
         }
         else if (useEndDate)
         {
             sqlString += "AND PurchaseOrderHdr.PurchaseOrderDate <= '" + EndDate.ToString("dd MMM yyyy HH:mm:ss") + "' ";
         }

         if (UserName != "")
         {
             sqlString += "AND PurchaseOrderHdr.UserName LIKE '%" + UserName + "%' ";
         }

         if (InventoryLocationID != "" && InventoryLocationID != "0")
         {
             sqlString += "AND PurchaseOrderHdr.InventoryLocationID = " + InventoryLocationID + " ";
         }
         if (remark != "")
         {
             sqlString += "AND remark like '%" + remark + "%'";
         }

         sqlString += "AND PurchaseOrderHdr.Remark NOT LIKE 'Stock Take Adj.%' ";

         if (SortColumn != null && SortColumn != "")
         {
             if (SortDir.Trim() == "ASC")
             {
                 sqlString += "ORDER BY " + SortColumn + " ASC ";
             }
             else if (SortDir.Trim() == "DESC")
             {
                 sqlString += "ORDER BY " + SortColumn + " DESC ";
             }
             else
             {
                 sqlString += "ORDER BY PurchaseOrderHdr.PurchaseOrderDate DESC ";
             }
         }
         else
         {
             sqlString += "ORDER BY PurchaseOrderHdr.PurchaseOrderDate DESC ";
         }

         DataTable Dt = new DataTable();
         Dt.Load(DataService.GetReader(new QueryCommand(sqlString)));

         return Dt;

     }

     public static DataTable FetchPurchaseOrderHeader
      (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
      string UserName, string InventoryLocationID, string movementType, string remark,
      string SortColumn, string SortDir, bool showGoodsReceive, string supplierName)
     {
         string sqlString =
             "SELECT PurchaseOrderHdr.PurchaseOrderHdrRefNo, PurchaseOrderHdr.PurchaseOrderDate, PurchaseOrderHdr.UserName, " +
                 "PurchaseOrderHdr.ExchangeRate, " +
                 "PurchaseOrderHdr.Supplier, PurchaseOrderHdr.FreightCharge, PurchaseOrderHdr.DeliveryCharge,  " +
                 "PurchaseOrderHdr.Discount, PurchaseOrderHdr.Remark, PurchaseOrderHdr.InventoryLocationID, " +
                 "PurchaseOrderHdr.CreatedOn, PurchaseOrderHdr.ModifiedOn, PurchaseOrderHdr.CreatedBy, PurchaseOrderHdr.ModifiedBy, " +
                 "PurchaseOrderHdr.UniqueID, " +
                 "CASE WHEN ISNULL(PurchaseOrderHdr.Userfld7,'') = '' THEN 'Submitted' ELSE ISNULL(PurchaseOrderHdr.Userfld7,'') END Status, " +
                 "InventoryLocation.InventoryLocationName, Supplier.SupplierName, " +
                 "CustomRefNo = CASE WHEN ISNULL(PurchaseOrderHdr.CustomRefNo, '') <> '' THEN PurchaseOrderHdr.CustomRefNo ELSE PurchaseOrderHdr.PurchaseOrderHdrRefNo END " +

             "FROM PurchaseOrderHdr,  " +
                 "InventoryLocation,   " +
                 "Supplier " +
             "WHERE PurchaseOrderHdr.InventoryLocationID = InventoryLocation.InventoryLocationID AND ISNULL(PurchaseOrderHdr.Deleted,0) = 0 AND Supplier.SupplierID = PurchaseOrderHdr.Supplier ";

         if (useStartDate & useEndDate)
         {
             sqlString += "AND (PurchaseOrderHdr.PurchaseOrderDate BETWEEN '" + StartDate.ToString("dd MMM yyyy HH:mm:ss") + "' AND '" + EndDate.ToString("dd MMM yyyy HH:mm:ss") + "') ";
         }
         else if (useStartDate)
         {
             sqlString += "AND PurchaseOrderHdr.PurchaseOrderDate >= '" + StartDate.ToString("dd MMM yyyy HH:mm:ss") + "' ";
         }
         else if (useEndDate)
         {
             sqlString += "AND PurchaseOrderHdr.PurchaseOrderDate <= '" + EndDate.ToString("dd MMM yyyy HH:mm:ss") + "' ";
         }

         if (UserName != "")
         {
             sqlString += "AND PurchaseOrderHdr.UserName LIKE '%" + UserName + "%' ";
         }

         if (InventoryLocationID != "" && InventoryLocationID != "0")
         {
             sqlString += "AND PurchaseOrderHdr.InventoryLocationID = " + InventoryLocationID + " ";
         }

         if (supplierName != "")
         {
             sqlString += "AND Supplier.SupplierName like '%" + supplierName + "%' ";
         }

         if (remark != "")
         {
             sqlString += "AND remark like '%" + remark + "%'";
         }

         sqlString += "AND PurchaseOrderHdr.Remark NOT LIKE 'Stock Take Adj.%' ";

         if (SortColumn != null && SortColumn != "")
         {
             if (SortDir.Trim() == "ASC")
             {
                 sqlString += "ORDER BY " + SortColumn + " ASC ";
             }
             else if (SortDir.Trim() == "DESC")
             {
                 sqlString += "ORDER BY " + SortColumn + " DESC ";
             }
             else
             {
                 sqlString += "ORDER BY PurchaseOrderHdr.PurchaseOrderDate DESC ";
             }
         }
         else
         {
             sqlString += "ORDER BY PurchaseOrderHdr.PurchaseOrderDate DESC ";
         }

         DataTable Dt = new DataTable();
         Dt.Load(DataService.GetReader(new QueryCommand(sqlString)));

         return Dt;

     }   

     public static DataTable GetSMFReport(string search, bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate)
     {
         if (string.IsNullOrEmpty(search))
             search = "%"; 

         DataTable dt = new DataTable();
         dt.Columns.Add("No");
         dt.Columns.Add("Date");
         dt.Columns.Add("ReceiptNo");
         dt.Columns.Add("ClientName");
         dt.Columns.Add("SMFNO");
         dt.Columns.Add("NRIC");

         string sqlcategory = "select distinct c.CategoryName from category c inner join item i on c.CategoryName = i.CategoryName " +
                                "where isnull(c.deleted,0) = 0 and ISNULL(i.deleted, 0) = 0 and (isnull(i.userflag3,0) = 1 or isnull(i.userflag4,0) = 1)";

         DataTable dCategory = DataService.GetDataSet(new QueryCommand(sqlcategory)).Tables[0];

         if (dCategory.Rows.Count > 0)
         {
             for (int i = 0; i < dCategory.Rows.Count; i++)
             {
                 dt.Columns.Add(dCategory.Rows[i][0].ToString() + "_ItemNo");
                 dt.Columns.Add(dCategory.Rows[i][0].ToString() + "_SMFAmount", Type.GetType("System.Decimal"));
             }
         }

         dt.Columns.Add("TotalSMF", Type.GetType("System.Decimal"));
         dt.Columns.Add("PatientPayable", Type.GetType("System.Decimal"));
         dt.Columns.Add("PaymentType");
         dt.Columns.Add("PAMedifund", Type.GetType("System.Decimal"));
         dt.Columns.Add("PWF", Type.GetType("System.Decimal"));

         //string sqlreport = "select distinct  oh.OrderDate, ISNULL(oh.userfld5, OrderRefNo) as ReceiptNo, m.NameToAppear as MembershipName " +
         //       ",ISNULL(od.userfld4,'') as SMFNo, ISNULL(m.NRIC,'') as NRIC " +
         //       ",sm.TotalSMF, sm.PAMedifund, sm.PWF, sm.PatientPayable, oh.OrderHdrID, rd.PaymentType " +
         //   "from ReceiptHdr rh  " +
         //   "inner join receiptdet rd on rh.receipthdrid = rd.ReceiptHdrID " +
         //   "inner join OrderHdr oh on oh.orderhdrid = rh.orderhdrid " +
         //   "inner join Membership m on oh.membershipno = m.MembershipNo " +
         //   "inner join orderdet od on oh.orderhdrid = od.OrderHdrID " +
         //   "inner join item i on od.itemno = i.itemno " +
         //   "left outer join  " +
         //   "( select dt.receipthdrid, sum(case when dt.PaymentType = 'SMF' then dt.Amount else 0 end) as TotalSMF, sum(case when dt.PaymentType = 'PAMedifund' then dt.Amount else 0 end) as PAMedifund, " +
         //   " sum(case when dt.PaymentType = 'PWF' then dt.Amount else 0 end) as PWF, sum(case when dt.PaymentType <> 'PWF' and dt.PaymentType <> 'SMF' and dt.PaymentType <> 'PAMedifund' then dt.Amount else 0 end) as PatientPayable " +
         //   " from receiptdet dt group by dt.receipthdrid )sm on rd.receipthdrid = sm.receipthdrid " +
         //   " where rd.PaymentType in ('SMF', 'PAMedifund', 'PWF') " +
         //   " and isnull(rh.isvoided,0) = 0 and isnull(oh.isvoided,0) = 0 and isnull(od.isvoided,0) = 0 and ISNULL(i.deleted, 0) = 0 and (isnull(i.userflag3,0) = 1 or isnull(i.userflag4,0) = 1) " +
         //   "and ISNULL(oh.userfld5, OrderRefNo) + ISNULL(m.NameToAppear,'') + ISNULL(od.userfld4,'') + ISNULL(m.NRIC,'') + i.ItemNo LIKE N'%" + search + "%' ";

         string sqlreport = @"
            SELECT OrderDate, ReceiptNo, MembershipName, MAX(SMFNo) as SMFNo, NRIC, 
                TotalSMF, PAMedifund, PWF, PatientPayable, OrderHdrID, PaymentType 
            FROM (
                SELECT DISTINCT oh.OrderDate, ISNULL(oh.userfld5, OrderRefNo) as ReceiptNo, 
                    m.NameToAppear as MembershipName, ISNULL(od.userfld4,'') as SMFNo, ISNULL(m.NRIC,'') as NRIC,
                    sm.TotalSMF, sm.PAMedifund, sm.PWF, sm.PatientPayable, oh.OrderHdrID, 
                    STUFF((
                            SELECT ', ' + PaymentType
                            FROM ReceiptDet
                            WHERE ReceiptHdrID = rh.ReceiptHdrID AND PaymentType NOT IN ('SMF', 'PAMedifund', 'PWF')
                            FOR XML PATH('')
                        ), 1, 2, '') as PaymentType
                FROM ReceiptHdr rh
                    INNER JOIN ReceiptDet rd ON rh.ReceiptHdrID = rd.ReceiptHdrID 
                    INNER JOIN OrderHdr oh ON oh.OrderHdrID = rh.OrderHdrID 
                    INNER JOIN Membership m ON oh.MembershipNo = m.MembershipNo 
                    INNER JOIN OrderDet od ON oh.OrderHdrID = od.OrderHdrID 
                    INNER JOIN Item i ON od.ItemNo = i.ItemNo 
                    LEFT OUTER JOIN (
                            SELECT dt.ReceiptHdrID, 
                                SUM(CASE WHEN dt.PaymentType = 'SMF' THEN dt.Amount ELSE 0 END) as TotalSMF, 
                                SUM(CASE WHEN dt.PaymentType = 'PAMedifund' THEN dt.Amount ELSE 0 END) as PAMedifund, 
                                SUM(CASE WHEN dt.PaymentType = 'PWF' THEN dt.Amount ELSE 0 END) as PWF, 
                                SUM(CASE WHEN dt.PaymentType NOT IN ('PWF', 'SMF', 'PAMedifund') THEN dt.Amount ELSE 0 END) as PatientPayable  
                            FROM ReceiptDet dt 
                            GROUP BY dt.ReceiptHdrID
                        ) sm ON rd.ReceiptHdrID = sm.ReceiptHdrID 
                WHERE rd.PaymentType IN ('SMF', 'PAMedifund', 'PWF') 
                    AND ISNULL(rh.IsVoided, 0) = 0 AND ISNULL(oh.IsVoided, 0) = 0 AND ISNULL(od.IsVoided, 0) = 0 
                    /*AND ISNULL(i.Deleted, 0) = 0 AND (ISNULL(i.userflag3, 0) = 1 OR ISNULL(i.userflag4, 0) = 1)*/ 
                    AND ISNULL(oh.userfld5, OrderRefNo) + ISNULL(m.NameToAppear, '') + ISNULL(od.userfld4, '') 
                        + ISNULL(m.NRIC, '') + i.ItemNo LIKE N'%" + search + "%' ";

         if (useStartDate & useEndDate)
         {
             sqlreport += "AND oh.OrderDate BETWEEN '" + StartDate.ToString("yyyy-MM-dd") + "' AND '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
         }
         else if (useStartDate)
         {
             sqlreport += "AND oh.OrderDate >= '" + StartDate.ToString("yyyy-MM-dd") + "' ";
         }
         else if (useEndDate)
         {
             sqlreport += "AND oh.OrderDate <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
         }

         sqlreport += @"
            ) a
            GROUP BY OrderDate, ReceiptNo, MembershipName, NRIC, 
                TotalSMF, PAMedifund, PWF, PatientPayable, OrderHdrID, PaymentType 
                      ";

         DataTable dReport = DataService.GetDataSet(new QueryCommand(sqlreport)).Tables[0];

         if (dReport.Rows.Count > 0)
         { 
             int index = 1;
            for(int i = 0; i < dReport.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["No"] = index;
                dr["Date"] = DateTime.Parse(dReport.Rows[i]["OrderDate"].ToString()).ToString("dd-MM-yyyy");
                dr["ReceiptNo"] = dReport.Rows[i]["ReceiptNo"];
                dr["ClientName"] = dReport.Rows[i]["MembershipName"];
                dr["SMFNO"] = dReport.Rows[i]["SMFNo"];
                dr["NRIC"] = dReport.Rows[i]["NRIC"];
                dr["TotalSMF"] = Math.Round(Convert.ToDecimal(dReport.Rows[i]["TotalSMF"] ?? 0), 2, MidpointRounding.AwayFromZero); //.ToString("N2");
                dr["PatientPayable"] = Math.Round(Convert.ToDecimal(dReport.Rows[i]["PatientPayable"] ?? 0), 2, MidpointRounding.AwayFromZero); //.ToString("N2");
                dr["PaymentType"] = dReport.Rows[i]["PaymentType"] ?? "";
                dr["PAMedifund"] = Math.Round(Convert.ToDecimal(dReport.Rows[i]["PAMedifund"] ?? 0), 2, MidpointRounding.AwayFromZero); //.ToString("N2");
                dr["PWF"] = Math.Round(Convert.ToDecimal(dReport.Rows[i]["PWF"] ?? 0), 2, MidpointRounding.AwayFromZero); //.ToString("N2");

                string orderhdrid = dReport.Rows[i]["OrderHdrID"].ToString();
                string sqlitem = "select od.ItemNo, i.ItemName, i.CategoryName, " +
                                 "CASE WHEN EXISTS(SELECT * FROM ReceiptDet rd WHERE rd.ReceiptHdrID = od.OrderHdrID AND rd.PaymentType in ('SMF', 'PAMedifund', 'PWF')) THEN ISNULL(od.userfloat2,0) ELSE 0 END as [SMFAmount] " +
                                 "from orderdet od inner join item i on od.ItemNo = i.ItemNo " +
                                 "where orderhdrid = '" + orderhdrid + "' and ISNULL(i.deleted, 0) = 0 and (isnull(i.userflag3,0) = 1 or isnull(i.userflag4,0) = 1) ";

                DataTable dItem = DataService.GetDataSet(new QueryCommand(sqlitem)).Tables[0];
                
                if(dItem.Rows.Count >0)
                {
                    for(int j = 0;j < dItem.Rows.Count;j++)
                    {
                        string category = dItem.Rows[j]["CategoryName"].ToString();

                        dr[category + "_ItemNo"] = dItem.Rows[j]["ItemNo"];
                        dr[category + "_SMFAmount"] = Math.Round(Convert.ToDecimal(dItem.Rows[j]["SMFAmount"] ?? 0), 2);

                        string sql = @"
                                        SELECT oh.userfld5
                                        FROM OrderDet od
                                            INNER JOIN OrderHdr oh ON oh.OrderHdrID = od.OrderHdrID
                                            INNER JOIN OrderHdr ref ON ref.userfld5 = od.userfld5
                                        WHERE od.ItemNo = '" + dItem.Rows[j]["ItemNo"].ToString() + "' AND od.userfld5 = '" + dr["ReceiptNo"].ToString() + @"'
                                     ";
                        QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
                        object result = DataService.ExecuteScalar(cmd);
                        if (result != null && !string.IsNullOrEmpty(result.ToString()))
                        {
                            dr[category + "_ItemNo"] += " (Refunded in " + result.ToString() + ")";
                        }
                        else
                        {
                            sql = @"
                                    SELECT ref.userfld5
                                    FROM OrderDet od
                                        INNER JOIN OrderHdr oh ON oh.OrderHdrID = od.OrderHdrID
                                        INNER JOIN OrderHdr ref ON ref.userfld5 = od.userfld5
                                    WHERE od.ItemNo = '" + dItem.Rows[j]["ItemNo"].ToString() + "' AND oh.userfld5 = '" + dr["ReceiptNo"].ToString() + @"'
                                  ";
                            cmd = new QueryCommand(sql, "PowerPOS");
                            result = DataService.ExecuteScalar(cmd);
                            if (result != null && !string.IsNullOrEmpty(result.ToString()))
                            {
                                dr[category + "_ItemNo"] += " (Refund for " + result.ToString() + ")";
                            }
                        }

                    }
                }

                dt.Rows.Add(dr);
                index++;
            }
         }

        return dt;
     }

     public static DataTable GetGoodsReceiveList(string refNo, out string status)
     {
         status = "";
         DataTable dt = new DataTable();
         try
         {
             string invrefno = "";
             if(!string.IsNullOrEmpty(refNo))
                 invrefno = "AND InventoryHdrRefNo = '" + refNo + "' ";

             string SQLString =
                     "SELECT Item.*, Category.*, ItemDepartment.*, ISNULL(Quantity, 0) Quantity, Item.Barcode as BarcodeText FROM Item " +
                    "INNER JOIN Category ON Item.CategoryName = Category.CategoryName " +
                    "INNER JOIN ItemDepartment ON Category.ItemDepartmentID = ItemDepartment.ItemDepartmentID " +
                    "INNER JOIN " +
                    "( " +
                        "SELECT ItemNo, Quantity AS Quantity FROM InventoryDet WHERE InventoryHdrRefNo IN " +
                        "(SELECT TOP 1 InventoryHdrRefNo FROM InventoryHdr WHERE MovementType = 'Stock In' {0} ORDER BY InventoryDate DESC) " +
                    ") Qty ON Item.ItemNo = Qty.ItemNo " +
                "WHERE Category.CategoryName <> 'SYSTEM' AND Category.Deleted = 0 AND Item.Deleted = 0 " +
                "ORDER BY ItemName ";


             SQLString = string.Format(SQLString, invrefno);

             dt.Load(SubSonic.DataService.GetReader(new QueryCommand(SQLString)));
         }
         catch (Exception ex)
         {
             Logger.writeLog("Error Get Good Receive:" + ex.Message);
             status = ex.Message;
         }
         return dt;
     }
  
    }
}