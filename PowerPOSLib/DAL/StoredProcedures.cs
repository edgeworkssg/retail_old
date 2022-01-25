using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
namespace PowerPOS{
    public partial class SPs{
        
        /// <summary>
        /// Creates an object wrapper for the BACKUPDB Procedure
        /// </summary>
        public static StoredProcedure Backupdb(string DBName, string DBPATH)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("BACKUPDB", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@DBName", DBName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DBPATH", DBPATH, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the DeleteStockOutWithoutStockIn Procedure
        /// </summary>
        public static StoredProcedure DeleteStockOutWithoutStockIn()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("DeleteStockOutWithoutStockIn", DataService.GetInstance("PowerPOS"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchAllPossibleBuyXatPriceOfYPromo Procedure
        /// </summary>
        public static StoredProcedure FetchAllPossibleBuyXatPriceOfYPromo(string ItemNoList, bool? HasMember, int? PointOfSaleID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchAllPossibleBuyXatPriceOfYPromo", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemNoList", ItemNoList, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@HasMember", HasMember, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchAllPossibleItemGroupPromo Procedure
        /// </summary>
        public static StoredProcedure FetchAllPossibleItemGroupPromo(string ItemNoList, bool? HasMember, int? PointOfSaleID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchAllPossibleItemGroupPromo", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemNoList", ItemNoList, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@HasMember", HasMember, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchAllPossiblePromoAnyXofAllItems Procedure
        /// </summary>
        public static StoredProcedure FetchAllPossiblePromoAnyXofAllItems(string ItemNoList, bool? HasMember, int? PointOfSaleID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchAllPossiblePromoAnyXofAllItems", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemNoList", ItemNoList, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@HasMember", HasMember, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchAllPossiblePromoAnyXofAllItemsMemberGroup Procedure
        /// </summary>
        public static StoredProcedure FetchAllPossiblePromoAnyXofAllItemsMemberGroup(string ItemNoList, bool? HasMember, int? PointOfSaleID, int? MemberGroupID, string CategoryName, string ItemGroupIDList)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchAllPossiblePromoAnyXofAllItemsMemberGroup", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemNoList", ItemNoList, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@HasMember", HasMember, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@MemberGroupID", MemberGroupID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CategoryName", CategoryName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ItemGroupIDList", ItemGroupIDList, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchAllPossiblePromoForItem Procedure
        /// </summary>
        public static StoredProcedure FetchAllPossiblePromoForItem(string ItemNo, string CategoryName, int? Quantity, bool? HasMember, int? PointOfSaleID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchAllPossiblePromoForItem", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@CategoryName", CategoryName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Quantity", Quantity, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@HasMember", HasMember, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchAuditLogReport Procedure
        /// </summary>
        public static StoredProcedure FetchAuditLogReport(DateTime? StartDate, DateTime? EndDate, string Operation, string TableName, string Search)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchAuditLogReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@Operation", Operation, DbType.String, null, null);
        	
            sp.Command.AddParameter("@TableName", TableName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchAutoCompleteNames Procedure
        /// </summary>
        public static StoredProcedure FetchAutoCompleteNames(string Query)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchAutoCompleteNames", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@Query", Query, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchAutoCompleteWords Procedure
        /// </summary>
        public static StoredProcedure FetchAutoCompleteWords(string Query)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchAutoCompleteWords", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@Query", Query, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchCashOutReport Procedure
        /// </summary>
        public static StoredProcedure FetchCashOutReport(DateTime? startdate, DateTime? enddate, string PointOfSaleName, string OutletName, string DeptID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchCashOutReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleName", PointOfSaleName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchCategoryNameByPointOfSaleID Procedure
        /// </summary>
        public static StoredProcedure FetchCategoryNameByPointOfSaleID(int? PointOfSaleId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchCategoryNameByPointOfSaleID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleId", PointOfSaleId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchClosingReport Procedure
        /// </summary>
        public static StoredProcedure FetchClosingReport(bool? UseFromEndDate, bool? UseToEndDate, DateTime? FromEndDate, DateTime? ToEndDate, string CloseCounterReportRefNo, string Cashier, string SupervisorId, int? PointOfSaleID, string OutletName, int? DeptID, string SortBy, string SortDir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchClosingReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@UseFromEndDate", UseFromEndDate, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@UseToEndDate", UseToEndDate, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@FromEndDate", FromEndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@ToEndDate", ToEndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@CloseCounterReportRefNo", CloseCounterReportRefNo, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Cashier", Cashier, DbType.String, null, null);
        	
            sp.Command.AddParameter("@SupervisorId", SupervisorId, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@SortBy", SortBy, DbType.String, null, null);
        	
            sp.Command.AddParameter("@SortDir", SortDir, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchCollectionReport Procedure
        /// </summary>
        public static StoredProcedure FetchCollectionReport(DateTime? startdate, DateTime? enddate, string PointOfSaleName, string OutletName, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchCollectionReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleName", PointOfSaleName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchCollectionReportByDay Procedure
        /// </summary>
        public static StoredProcedure FetchCollectionReportByDay(DateTime? startdate, DateTime? enddate, string PointOfSaleName, string OutletName, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchCollectionReportByDay", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleName", PointOfSaleName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchCostOfGoods Procedure
        /// </summary>
        public static StoredProcedure FetchCostOfGoods(string ItemNo, int? LocationID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchCostOfGoods", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@LocationID", LocationID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchCostOfGoodsSold Procedure
        /// </summary>
        public static StoredProcedure FetchCostOfGoodsSold(DateTime? startdate, DateTime? enddate, string OutletName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchCostOfGoodsSold", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchCostOfGoodsToHandleNegativeQty Procedure
        /// </summary>
        public static StoredProcedure FetchCostOfGoodsToHandleNegativeQty(string ItemNo, string MovementType)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchCostOfGoodsToHandleNegativeQty", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@MovementType", MovementType, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchCustomerPurchaseBehaviorReport Procedure
        /// </summary>
        public static StoredProcedure FetchCustomerPurchaseBehaviorReport(DateTime? startdate, DateTime? enddate, string CategoryName, string ItemName, string PointOfSaleName, string OutletName, string membershipno, string nametoappear, string firstname, string lastname, int? MembershipGroupID, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchCustomerPurchaseBehaviorReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@CategoryName", CategoryName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ItemName", ItemName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleName", PointOfSaleName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@membershipno", membershipno, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@nametoappear", nametoappear, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@firstname", firstname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@lastname", lastname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@MembershipGroupID", MembershipGroupID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchDynamicSalesReport Procedure
        /// </summary>
        public static StoredProcedure FetchDynamicSalesReport(string StartDate, string EndDate, string ExtraFields, string TimeGrouping, string WhereLists)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchDynamicSalesReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.String, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ExtraFields", ExtraFields, DbType.String, null, null);
        	
            sp.Command.AddParameter("@TimeGrouping", TimeGrouping, DbType.String, null, null);
        	
            sp.Command.AddParameter("@WhereLists", WhereLists, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchGSTReport Procedure
        /// </summary>
        public static StoredProcedure FetchGSTReport(DateTime? startdate, DateTime? enddate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchGSTReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchItemByBarcode Procedure
        /// </summary>
        public static StoredProcedure FetchItemByBarcode(int? PointOfSaleId, string barcode)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchItemByBarcode", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleId", PointOfSaleId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@barcode", barcode, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchItemByName Procedure
        /// </summary>
        public static StoredProcedure FetchItemByName(int? PointOfSaleId, string itemName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchItemByName", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleId", PointOfSaleId, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@itemName", itemName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchItemNameListByPointOfSaleID Procedure
        /// </summary>
        public static StoredProcedure FetchItemNameListByPointOfSaleID(int? PointOfSaleId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchItemNameListByPointOfSaleID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleId", PointOfSaleId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchMembershipProductSales Procedure
        /// </summary>
        public static StoredProcedure FetchMembershipProductSales(string ItemList, string SelectList, string whereList)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchMembershipProductSales", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemList", ItemList, DbType.String, null, null);
        	
            sp.Command.AddParameter("@SelectList", SelectList, DbType.String, null, null);
        	
            sp.Command.AddParameter("@whereList", whereList, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchNegativeProductCategorySalesReport Procedure
        /// </summary>
        public static StoredProcedure FetchNegativeProductCategorySalesReport(string categoryname, DateTime? startdate, DateTime? enddate, string PointOfSaleName, string OutletName, string DeptID, bool? IsVoided, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchNegativeProductCategorySalesReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@categoryname", categoryname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleName", PointOfSaleName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@IsVoided", IsVoided, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchNegativeProductSalesReport Procedure
        /// </summary>
        public static StoredProcedure FetchNegativeProductSalesReport(DateTime? startdate, DateTime? enddate, string itemname, string PointOfSaleName, string OutletName, string CategoryName, string DeptID, bool? IsVoided, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchNegativeProductSalesReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@itemname", itemname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleName", PointOfSaleName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@CategoryName", CategoryName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@IsVoided", IsVoided, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchPreOrderReport Procedure
        /// </summary>
        public static StoredProcedure FetchPreOrderReport(DateTime? StartDate, DateTime? EndDate, string ItemName, string MembershipNo, string CustName, string IsOutstanding, string Notified, bool? OnlyReadyToDeliver, string SortBy, string SortDir, string Status, string OutletName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchPreOrderReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@ItemName", ItemName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@MembershipNo", MembershipNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@CustName", CustName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@IsOutstanding", IsOutstanding, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Notified", Notified, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@OnlyReadyToDeliver", OnlyReadyToDeliver, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@SortBy", SortBy, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@SortDir", SortDir, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Status", Status, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchProductCategorySalesReport Procedure
        /// </summary>
        public static StoredProcedure FetchProductCategorySalesReport(string categoryname, DateTime? startdate, DateTime? enddate, string PointOfSaleName, string OutletName, string DeptID, bool? IsVoided, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchProductCategorySalesReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@categoryname", categoryname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleName", PointOfSaleName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@IsVoided", IsVoided, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchProductCategorySalesReportWithCashOut Procedure
        /// </summary>
        public static StoredProcedure FetchProductCategorySalesReportWithCashOut(string categoryname, DateTime? startdate, DateTime? enddate, string PointOfSaleName, string OutletName, string DeptID, bool? IsVoided, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchProductCategorySalesReportWithCashOut", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@categoryname", categoryname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleName", PointOfSaleName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@IsVoided", IsVoided, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchProductSalesReport Procedure
        /// </summary>
        public static StoredProcedure FetchProductSalesReport(DateTime? startdate, DateTime? enddate, string itemname, string PointOfSaleName, string OutletName, string CategoryName, string DeptID, bool? IsVoided, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchProductSalesReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@itemname", itemname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleName", PointOfSaleName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@CategoryName", CategoryName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@IsVoided", IsVoided, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchProductSalesReportByPointOfSale Procedure
        /// </summary>
        public static StoredProcedure FetchProductSalesReportByPointOfSale(DateTime? startdate, DateTime? enddate, string pointofsaleid, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchProductSalesReportByPointOfSale", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@pointofsaleid", pointofsaleid, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchProductSalesReportByPointOfSaleByItems Procedure
        /// </summary>
        public static StoredProcedure FetchProductSalesReportByPointOfSaleByItems(DateTime? startdate, DateTime? enddate, string pointofsaleid, string ItemNoList, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchProductSalesReportByPointOfSaleByItems", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@pointofsaleid", pointofsaleid, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ItemNoList", ItemNoList, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchProductSalesReportBySalesPerson Procedure
        /// </summary>
        public static StoredProcedure FetchProductSalesReportBySalesPerson(DateTime? startdate, DateTime? enddate, string itemname, string PointOfSaleName, string OutletName, string DeptID, bool? IsVoided)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchProductSalesReportBySalesPerson", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@itemname", itemname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleName", PointOfSaleName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@IsVoided", IsVoided, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchProfitAndLossOnTransactionLevel Procedure
        /// </summary>
        public static StoredProcedure FetchProfitAndLossOnTransactionLevel(DateTime? startdate, DateTime? enddate, string OutletName, string DeptID, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchProfitAndLossOnTransactionLevel", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchProfitLossReport Procedure
        /// </summary>
        public static StoredProcedure FetchProfitLossReport(DateTime? startdate, DateTime? enddate, string OutletName, string DeptID, string sortdir, string sortby)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchProfitLossReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchQuickAccessCategories Procedure
        /// </summary>
        public static StoredProcedure FetchQuickAccessCategories(int? PointOfSaleID, Guid? QuickAccessGroupID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchQuickAccessCategories", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@QuickAccessGroupID", QuickAccessGroupID, DbType.Guid, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchRemainingQuantityOfAnItem Procedure
        /// </summary>
        public static StoredProcedure FetchRemainingQuantityOfAnItem(string ItemNo, int? LocationID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchRemainingQuantityOfAnItem", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@LocationID", LocationID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchSalesPersonPerformance Procedure
        /// </summary>
        public static StoredProcedure FetchSalesPersonPerformance(DateTime? startDate, DateTime? endDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchSalesPersonPerformance", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startDate", startDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@endDate", endDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchSalesPersonSalesByDate Procedure
        /// </summary>
        public static StoredProcedure FetchSalesPersonSalesByDate(DateTime? startdate, DateTime? enddate, string outletname, string pointofsalename, string salespersonname)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchSalesPersonSalesByDate", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startdate", startdate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@enddate", enddate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@outletname", outletname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@pointofsalename", pointofsalename, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@salespersonname", salespersonname, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchSpecialEventPrice Procedure
        /// </summary>
        public static StoredProcedure FetchSpecialEventPrice(DateTime? CurrentDate, string ItemNo, int? PointOfSaleID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchSpecialEventPrice", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@CurrentDate", CurrentDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchStockBalanceItemSummary Procedure
        /// </summary>
        public static StoredProcedure FetchStockBalanceItemSummary(string Search, int? InventoryLocationID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchStockBalanceItemSummary", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            sp.Command.AddParameter("@InventoryLocationID", InventoryLocationID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchStockBalanceItemSummaryByCategory Procedure
        /// </summary>
        public static StoredProcedure FetchStockBalanceItemSummaryByCategory(string Search, int? InventoryLocationID, string CategoryName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchStockBalanceItemSummaryByCategory", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            sp.Command.AddParameter("@InventoryLocationID", InventoryLocationID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@CategoryName", CategoryName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchStockOnHand Procedure
        /// </summary>
        public static StoredProcedure FetchStockOnHand(string itemname, string InventoryLocationName, string DeptID, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchStockOnHand", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@itemname", itemname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@InventoryLocationName", InventoryLocationName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchStockOutReportGroupByProductAndStockOutReason Procedure
        /// </summary>
        public static StoredProcedure FetchStockOutReportGroupByProductAndStockOutReason(DateTime? StartDate, DateTime? EndDate, string LocationName, string ReasonName, string search)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchStockOutReportGroupByProductAndStockOutReason", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@LocationName", LocationName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ReasonName", ReasonName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@search", search, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchStockOutThatIsNotSales Procedure
        /// </summary>
        public static StoredProcedure FetchStockOutThatIsNotSales()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchStockOutThatIsNotSales", DataService.GetInstance("PowerPOS"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchStockOutWithoutStockIn Procedure
        /// </summary>
        public static StoredProcedure FetchStockOutWithoutStockIn()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchStockOutWithoutStockIn", DataService.GetInstance("PowerPOS"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchStockOutWithoutStockInByInventoryLocationID Procedure
        /// </summary>
        public static StoredProcedure FetchStockOutWithoutStockInByInventoryLocationID(int? InventoryLocationID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchStockOutWithoutStockInByInventoryLocationID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@InventoryLocationID", InventoryLocationID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchStockReport Procedure
        /// </summary>
        public static StoredProcedure FetchStockReport(string itemname, string CategoryName, string InventoryLocationName, string DeptID, string sortby, string sortdir)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchStockReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@itemname", itemname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@CategoryName", CategoryName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@InventoryLocationName", InventoryLocationName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchStockReportBreakdownByLocation Procedure
        /// </summary>
        public static StoredProcedure FetchStockReportBreakdownByLocation(string Search, string CostingMethod)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchStockReportBreakdownByLocation", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CostingMethod", CostingMethod, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchStockReportBreakdownByLocationItemSummary Procedure
        /// </summary>
        public static StoredProcedure FetchStockReportBreakdownByLocationItemSummary(string Search, string CostingMethod)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchStockReportBreakdownByLocationItemSummary", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CostingMethod", CostingMethod, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchStockTakeQtyByItem Procedure
        /// </summary>
        public static StoredProcedure FetchStockTakeQtyByItem(DateTime? CurrentDate, string ItemNo, int? InventoryLocationID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchStockTakeQtyByItem", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@CurrentDate", CurrentDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@InventoryLocationID", InventoryLocationID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchTransactionReport Procedure
        /// </summary>
        public static StoredProcedure FetchTransactionReport(DateTime? StartDate, DateTime? EndDate, bool? UseStartDate, bool? UseEndDate, string RefNo, string CashierID, int? PointOfSaleID, string Outlet, string PaymentType, string Remarks, bool? ShowVoidedTransaction)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchTransactionReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@UseStartDate", UseStartDate, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@UseEndDate", UseEndDate, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@RefNo", RefNo, DbType.String, null, null);
        	
            sp.Command.AddParameter("@CashierID", CashierID, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Outlet", Outlet, DbType.String, null, null);
        	
            sp.Command.AddParameter("@PaymentType", PaymentType, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Remarks", Remarks, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ShowVoidedTransaction", ShowVoidedTransaction, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the FetchValidMembersPointsByDate Procedure
        /// </summary>
        public static StoredProcedure FetchValidMembersPointsByDate(DateTime? startValidPeriod, DateTime? endValidPeriod, string membershipno, string groupname, string nametoappear, string firstname, string lastname, string nric, string sortdir, string sortby)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("FetchValidMembersPointsByDate", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@startValidPeriod", startValidPeriod, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@endValidPeriod", endValidPeriod, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@membershipno", membershipno, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@groupname", groupname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@nametoappear", nametoappear, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@firstname", firstname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@lastname", lastname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@nric", nric, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortdir", sortdir, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@sortby", sortby, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetAddressFromPostalCode Procedure
        /// </summary>
        public static StoredProcedure GetAddressFromPostalCode(string PostalCode)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetAddressFromPostalCode", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PostalCode", PostalCode, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetDataTable Procedure
        /// </summary>
        public static StoredProcedure GetDataTable(string TableName, bool? SyncAll, string cutoffDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetDataTable", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@TableName", TableName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@SyncAll", SyncAll, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@cutoffDate", cutoffDate, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetItemMatrix Procedure
        /// </summary>
        public static StoredProcedure GetItemMatrix(string itemno)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetItemMatrix", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@itemno", itemno, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetLatestDataIndex Procedure
        /// </summary>
        public static StoredProcedure GetLatestDataIndex(string TableName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetLatestDataIndex", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@TableName", TableName, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetMaxItemNoMatrix Procedure
        /// </summary>
        public static StoredProcedure GetMaxItemNoMatrix(string attributes1)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetMaxItemNoMatrix", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@attributes1", attributes1, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetMaxItemRefNo Procedure
        /// </summary>
        public static StoredProcedure GetMaxItemRefNo(int? PointOfSaleID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetMaxItemRefNo", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetMaxResourceID Procedure
        /// </summary>
        public static StoredProcedure GetMaxResourceID(int? PointOfSaleID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetMaxResourceID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetNewCashRecRefNoByPointOfSaleID Procedure
        /// </summary>
        public static StoredProcedure GetNewCashRecRefNoByPointOfSaleID(int? PointOfSaleId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetNewCashRecRefNoByPointOfSaleID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleId", PointOfSaleId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetNewCounterCloseIDByPointOfSaleID Procedure
        /// </summary>
        public static StoredProcedure GetNewCounterCloseIDByPointOfSaleID(int? PointOfSaleId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetNewCounterCloseIDByPointOfSaleID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleId", PointOfSaleId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetNewDeliveryNoByPointOfSaleID Procedure
        /// </summary>
        public static StoredProcedure GetNewDeliveryNoByPointOfSaleID(int? PointOfSaleId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetNewDeliveryNoByPointOfSaleID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleId", PointOfSaleId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetNewDeliveryOrderHdrNoByPointOfSaleID Procedure
        /// </summary>
        public static StoredProcedure GetNewDeliveryOrderHdrNoByPointOfSaleID(int? PointOfSaleId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetNewDeliveryOrderHdrNoByPointOfSaleID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleId", PointOfSaleId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetNewGenericHdrNoByPointOfSaleID Procedure
        /// </summary>
        public static StoredProcedure GetNewGenericHdrNoByPointOfSaleID(string TableName, string KeyColumnName, string PointOfSaleId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetNewGenericHdrNoByPointOfSaleID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@TableName", TableName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@KeyColumnName", KeyColumnName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleId", PointOfSaleId, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetNewInventoryRefNoByInventoryLocationID Procedure
        /// </summary>
        public static StoredProcedure GetNewInventoryRefNoByInventoryLocationID(int? InventoryLocationId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetNewInventoryRefNoByInventoryLocationID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@InventoryLocationId", InventoryLocationId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetNewMembershipNoByPointOfSaleID Procedure
        /// </summary>
        public static StoredProcedure GetNewMembershipNoByPointOfSaleID(int? PointOfSaleId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetNewMembershipNoByPointOfSaleID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleId", PointOfSaleId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetNewMembershipNoByPointOfSalePrefix Procedure
        /// </summary>
        public static StoredProcedure GetNewMembershipNoByPointOfSalePrefix(string PrefixCode)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetNewMembershipNoByPointOfSalePrefix", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PrefixCode", PrefixCode, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetNewOrderHdrNoByPointOfSaleID Procedure
        /// </summary>
        public static StoredProcedure GetNewOrderHdrNoByPointOfSaleID(int? PointOfSaleId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetNewOrderHdrNoByPointOfSaleID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleId", PointOfSaleId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetNewOrderHdrNoByPointOfSaleIDForWeb Procedure
        /// </summary>
        public static StoredProcedure GetNewOrderHdrNoByPointOfSaleIDForWeb(int? PointOfSaleId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetNewOrderHdrNoByPointOfSaleIDForWeb", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleId", PointOfSaleId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetOrderHdrNotInOrderList Procedure
        /// </summary>
        public static StoredProcedure GetOrderHdrNotInOrderList(DateTime? StartDate, DateTime? EndDate, int? PointOfSaleID, string OrderList)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetOrderHdrNotInOrderList", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@OrderList", OrderList, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetOrderHdrNotInOrderListWithoutPOSID Procedure
        /// </summary>
        public static StoredProcedure GetOrderHdrNotInOrderListWithoutPOSID(DateTime? StartDate, DateTime? EndDate, string OrderList)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetOrderHdrNotInOrderListWithoutPOSID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@OrderList", OrderList, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetStockBalanceQtyByItem Procedure
        /// </summary>
        public static StoredProcedure GetStockBalanceQtyByItem(string ItemNo, string LocationID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetStockBalanceQtyByItem", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@LocationID", LocationID, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetTotalFloatAmount Procedure
        /// </summary>
        public static StoredProcedure GetTotalFloatAmount(int? PointOfSaleID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetTotalFloatAmount", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@PointOfSaleID", PointOfSaleID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetLatestCostPriceByItemNo Procedure
        /// </summary>
        public static StoredProcedure GetLatestCostPriceByItemNo(DateTime? StartDate, DateTime? EndDate, string ItemNo)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetLatestCostPriceByItemNo", DataService.GetInstance("PowerPOS"), "dbo");
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);

            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);

            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
            return sp;
        }

        /// <summary>
        /// Creates an object wrapper for the GetLatestCostPriceByItemNo Procedure
        /// </summary>
        public static StoredProcedure GetLatestCostPriceByItemNoAndLocationID(DateTime? StartDate, DateTime? EndDate, string ItemNo, int InventoryLocationID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetLatestCostPriceByItemNoAndLocationID", DataService.GetInstance("PowerPOS"), "dbo");
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);

            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);

            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);

            sp.Command.AddParameter("@InventoryLocationID", InventoryLocationID, DbType.Int32, 0, 10);

            return sp;
        }



        /// <summary>
        /// Creates an object wrapper for the GetTotalQtyByItemNoAndMovementType Procedure
        /// </summary>
        public static StoredProcedure GetTotalQtyByItemNoAndMovementType(DateTime? StartDate, DateTime? EndDate, string ItemNo, string MovementType)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetTotalQtyByItemNoAndMovementType", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@MovementType", MovementType, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetTotalQtyByItemNoAndMovementTypeAndLocationID Procedure
        /// </summary>
        public static StoredProcedure GetTotalQtyByItemNoAndMovementTypeAndLocationID(DateTime? StartDate, DateTime? EndDate, string ItemNo, string MovementType, int? InventoryLocationID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetTotalQtyByItemNoAndMovementTypeAndLocationID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@MovementType", MovementType, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@InventoryLocationID", InventoryLocationID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the GetTotalQtyByMovementTypeAndLocationID Procedure
        /// </summary>
        public static StoredProcedure GetTotalQtyByMovementTypeAndLocationID(DateTime? StartDate, DateTime? EndDate, string ItemNo, string MovementType, int? InventoryLocationID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("GetTotalQtyByMovementTypeAndLocationID", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@MovementType", MovementType, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@InventoryLocationID", InventoryLocationID, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_CategorySalesReport Procedure
        /// </summary>
        public static StoredProcedure ReportCategorySalesReport(string CategoryName, DateTime? StartDate, DateTime? EndDate, string OutletName, string DeptID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_CategorySalesReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@CategoryName", CategoryName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@DeptID", DeptID, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_DeliveryOrder Procedure
        /// </summary>
        public static StoredProcedure ReportDeliveryOrder(DateTime? InpStartDate, DateTime? InpEndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_DeliveryOrder", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@InpStartDate", InpStartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@InpEndDate", InpEndDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_InventorySummaryReport Procedure
        /// </summary>
        public static StoredProcedure ReportInventorySummaryReport(string ItemDepartmentID, string Category, int? SupplierID, string Search)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_InventorySummaryReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemDepartmentID", ItemDepartmentID, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Category", Category, DbType.String, null, null);
        	
            sp.Command.AddParameter("@SupplierID", SupplierID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_NewCommissionReport Procedure
        /// </summary>
        public static StoredProcedure ReportNewCommissionReport(DateTime? FilterStartDate, DateTime? FilterEndDate, string FilterUserName, int? Generate, string Month)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_NewCommissionReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@FilterStartDate", FilterStartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@FilterEndDate", FilterEndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@FilterUserName", FilterUserName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Generate", Generate, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Month", Month, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_NewCommissionReport_SalesDetail Procedure
        /// </summary>
        public static StoredProcedure ReportNewCommissionReportSalesDetail(DateTime? FilterStartDate, DateTime? FilterEndDate, string FilterUserName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_NewCommissionReport_SalesDetail", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@FilterStartDate", FilterStartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@FilterEndDate", FilterEndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@FilterUserName", FilterUserName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_NewCommissionReportBracket Procedure
        /// </summary>
        public static StoredProcedure ReportNewCommissionReportBracket(DateTime? FilterStartDate, DateTime? FilterEndDate, string FilterUserName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_NewCommissionReportBracket", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@FilterStartDate", FilterStartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@FilterEndDate", FilterEndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@FilterUserName", FilterUserName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_NewCommissionReportTotal Procedure
        /// </summary>
        public static StoredProcedure ReportNewCommissionReportTotal(DateTime? FilterStartDate, DateTime? FilterEndDate, string FilterUserName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_NewCommissionReportTotal", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@FilterStartDate", FilterStartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@FilterEndDate", FilterEndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@FilterUserName", FilterUserName, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_ProductSalesReport Procedure
        /// </summary>
        public static StoredProcedure ReportProductSalesReport(string CategoryName, DateTime? StartDate, DateTime? EndDate, string OutletName, string Search, int? SupplierID, string ItemDepartmentID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_ProductSalesReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@CategoryName", CategoryName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@OutletName", OutletName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            sp.Command.AddParameter("@SupplierID", SupplierID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@ItemDepartmentID", ItemDepartmentID, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_SalesCommissionSummary Procedure
        /// </summary>
        public static StoredProcedure ReportSalesCommissionSummary(DateTime? FilterStartDate, DateTime? FilterEndDate, string FilterUsername, int? ShowUnpaidOnly, int? Generate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_SalesCommissionSummary", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@FilterStartDate", FilterStartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@FilterEndDate", FilterEndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@FilterUsername", FilterUsername, DbType.String, null, null);
        	
            sp.Command.AddParameter("@ShowUnpaidOnly", ShowUnpaidOnly, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Generate", Generate, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_StockOnHandReport Procedure
        /// </summary>
        public static StoredProcedure ReportStockOnHandReport(string ItemDepartmentID, string Category, int? LocationID, string Supplier, string Search)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_StockOnHandReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemDepartmentID", ItemDepartmentID, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Category", Category, DbType.String, null, null);
        	
            sp.Command.AddParameter("@LocationID", LocationID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Supplier", Supplier, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_StockOnHandReportByDate Procedure
        /// </summary>
        public static StoredProcedure ReportStockOnHandReportByDate(string ItemDepartmentID, string Category, string Supplier, int? LocationID, DateTime? StartDate, string Search)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_StockOnHandReportByDate", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemDepartmentID", ItemDepartmentID, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Category", Category, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Supplier", Supplier, DbType.String, null, null);
        	
            sp.Command.AddParameter("@LocationID", LocationID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_StockOnHandReportGroup Procedure
        /// </summary>
        public static StoredProcedure ReportStockOnHandReportGroup(string ItemDepartmentID, string Category, int? LocationID, int? LocationGroupID, string Supplier, string Search)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_StockOnHandReportGroup", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemDepartmentID", ItemDepartmentID, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Category", Category, DbType.String, null, null);
        	
            sp.Command.AddParameter("@LocationID", LocationID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@LocationGroupID", LocationGroupID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Supplier", Supplier, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_StockOnHandReportInvLocGroup Procedure
        /// </summary>
        public static StoredProcedure ReportStockOnHandReportInvLocGroup(string ItemDepartmentID, string Category, int? LocationID, string Supplier, string Search, bool? IsUseInvLocGroup)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_StockOnHandReportInvLocGroup", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemDepartmentID", ItemDepartmentID, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Category", Category, DbType.String, null, null);
        	
            sp.Command.AddParameter("@LocationID", LocationID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Supplier", Supplier, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsUseInvLocGroup", IsUseInvLocGroup, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_StockOnHandReportItemGlobal Procedure
        /// </summary>
        public static StoredProcedure ReportStockOnHandReportItemGlobal(string ItemDepartmentID, string Category, int? LocationID, string Supplier, string Search)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_StockOnHandReportItemGlobal", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemDepartmentID", ItemDepartmentID, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Category", Category, DbType.String, null, null);
        	
            sp.Command.AddParameter("@LocationID", LocationID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Supplier", Supplier, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_StockOnHandReportItemSummary Procedure
        /// </summary>
        public static StoredProcedure ReportStockOnHandReportItemSummary(string ItemDepartmentID, string Category, int? LocationID, string Supplier, string Search)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_StockOnHandReportItemSummary", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemDepartmentID", ItemDepartmentID, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Category", Category, DbType.String, null, null);
        	
            sp.Command.AddParameter("@LocationID", LocationID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Supplier", Supplier, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_StockOnHandReportItemSummaryGroup Procedure
        /// </summary>
        public static StoredProcedure ReportStockOnHandReportItemSummaryGroup(string ItemDepartmentID, string Category, int? LocationID, string Supplier, string Search)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_StockOnHandReportItemSummaryGroup", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemDepartmentID", ItemDepartmentID, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Category", Category, DbType.String, null, null);
        	
            sp.Command.AddParameter("@LocationID", LocationID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Supplier", Supplier, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_StockOnHandReportLocGroupByDate Procedure
        /// </summary>
        public static StoredProcedure ReportStockOnHandReportLocGroupByDate(string ItemDepartmentID, string Category, int? LocationID, string Supplier, DateTime? StartDate, string Search, bool? IsUseInvLocGroup)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_StockOnHandReportLocGroupByDate", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ItemDepartmentID", ItemDepartmentID, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Category", Category, DbType.String, null, null);
        	
            sp.Command.AddParameter("@LocationID", LocationID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Supplier", Supplier, DbType.String, null, null);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            sp.Command.AddParameter("@IsUseInvLocGroup", IsUseInvLocGroup, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the REPORT_StockOnHandReportWithCostGroup Procedure
        /// </summary>
        public static StoredProcedure ReportStockOnHandReportWithCostGroup(string Category, int? LocationID, DateTime? StartDate, string Search)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("REPORT_StockOnHandReportWithCostGroup", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@Category", Category, DbType.String, null, null);
        	
            sp.Command.AddParameter("@LocationID", LocationID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@Search", Search, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the StylistCommissionReport Procedure
        /// </summary>
        public static StoredProcedure StylistCommissionReport(DateTime? StartDate, DateTime? EndDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("StylistCommissionReport", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the UPDATE_DW_HourlyPayment Procedure
        /// </summary>
        public static StoredProcedure UpdateDwHourlyPayment(DateTime? StartDate, DateTime? EndDate, string Outlet)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("UPDATE_DW_HourlyPayment", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@Outlet", Outlet, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the UPDATE_DW_HourlyProductSales Procedure
        /// </summary>
        public static StoredProcedure UpdateDwHourlyProductSales(DateTime? StartDate, DateTime? EndDate, string Outlet)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("UPDATE_DW_HourlyProductSales", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@Outlet", Outlet, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the UPDATE_DW_HourlySales Procedure
        /// </summary>
        public static StoredProcedure UpdateDwHourlySales(DateTime? StartDate, DateTime? EndDate, string Outlet)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("UPDATE_DW_HourlySales", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@StartDate", StartDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@EndDate", EndDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@Outlet", Outlet, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the UPDATE_FuturePrice Procedure
        /// </summary>
        public static StoredProcedure UpdateFuturePrice(DateTime? ExecDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("UPDATE_FuturePrice", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@ExecDate", ExecDate, DbType.DateTime, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the UPDATE_ItemBatchSummaryStockIn Procedure
        /// </summary>
        public static StoredProcedure UpdateItemBatchSummaryStockIn(string InventoryDetID, string ItemNo, int? InventoryLocationID, DateTime? InventoryDate, double? Qty, double? CostPrice, string BatchNo)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("UPDATE_ItemBatchSummaryStockIn", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@InventoryDetID", InventoryDetID, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@InventoryLocationID", InventoryLocationID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@InventoryDate", InventoryDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@Qty", Qty, DbType.Double, null, 53);
        	
            sp.Command.AddParameter("@CostPrice", CostPrice, DbType.Double, null, 53);
        	
            sp.Command.AddParameter("@BatchNo", BatchNo, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the UPDATE_ItemBatchSummaryStockOut Procedure
        /// </summary>
        public static StoredProcedure UpdateItemBatchSummaryStockOut(string InventoryDetID, string CostingMode, string MovementType, string ItemNo, int? InventoryLocationID, DateTime? InventoryDate, double? Qty)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("UPDATE_ItemBatchSummaryStockOut", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@InventoryDetID", InventoryDetID, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@CostingMode", CostingMode, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@MovementType", MovementType, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ItemNo", ItemNo, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@InventoryLocationID", InventoryLocationID, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@InventoryDate", InventoryDate, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@Qty", Qty, DbType.Double, null, 53);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the UPDATE_OrderDet_DataIndex Procedure
        /// </summary>
        public static StoredProcedure UpdateOrderDetDataIndex(int? OrderYear, int? OrderMonth, int? OrderDay)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("UPDATE_OrderDet_DataIndex", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@OrderYear", OrderYear, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@OrderMonth", OrderMonth, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@OrderDay", OrderDay, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the UPDATE_OrderHdr_DataIndex Procedure
        /// </summary>
        public static StoredProcedure UpdateOrderHdrDataIndex(int? OrderYear, int? OrderMonth, int? OrderDay)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("UPDATE_OrderHdr_DataIndex", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@OrderYear", OrderYear, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@OrderMonth", OrderMonth, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@OrderDay", OrderDay, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the UPDATE_ReceiptDet_DataIndex Procedure
        /// </summary>
        public static StoredProcedure UpdateReceiptDetDataIndex(int? OrderYear, int? OrderMonth, int? OrderDay)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("UPDATE_ReceiptDet_DataIndex", DataService.GetInstance("PowerPOS"), "dbo");
        	
            sp.Command.AddParameter("@OrderYear", OrderYear, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@OrderMonth", OrderMonth, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@OrderDay", OrderDay, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
    }
    
}
