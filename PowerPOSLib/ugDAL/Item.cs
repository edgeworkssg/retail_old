using System.Data;
using System;
using SubSonic;
namespace PowerPOS
{
    public partial class Item
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string UOM = @"Userfld1";
            /// <summary>Userfld2</summary>
            public static string MappedItemNo = @"Userfld2";
            /// <summary>Userint1</summary>
            public static string BaseLevel = @"Userint1";
            /// <summary>Userint2</summary>
            public static string CapQty = @"Userint2";
            /// <summary>Userint3</summary>
            public static string IsOpenPricePackage = @"Userint3";
            /// <summary>Userint4</summary>
            public static string IsConsignment = @"Userint4";
            /// <summary>Userint5</summary>
            public static string ExcludeProfitLoss = @"Userint5";
            
            
            /// <summary>Userflag1</summary>
            public static string IsMatrixItem = @"Userflag1";
            /// <summary>Userflag2</summary>
            public static string AllowPreOrder = @"Userflag2";
            /// <summary>Userflag3</summary>
            public static string IsPAMedifund = @"Userflag3";
            /// <summary>Userflag4</summary>
            public static string IsSMF = @"Userflag4";
            /// <summary>Userflag5</summary>
            public static string IsVendorDelivery = @"Userflag5";
            /// <summary>Userflag6</summary>
            public static string AutoCaptureWeight = @"Userflag6";         

            /// <summary>Userflag7</summary>
            public static string NonInventoryProduct = @"Userflag7";
            /// <summary>Userflag8</summary>
            public static string DeductConvType = @"Userflag8";
            /// <summary>Userflag9</summary>
            public static string IsUsingFixedCOG = @"Userflag9";
            /// <summary>Userflag10</summary>
            public static string IsUseSerialNo = @"Userflag10";

            /// <summary>Userfld9</summary>
            public static string IsPointRedeemable = @"Userfld9";
            /// <summary>Userfld8</summary>
            public static string DeductedItem = @"Userfld8";
            /// <summary>Userfloat5</summary>
            public static string DeductConvRate = @"Userfloat5";


            /// <summary>Userfloat6</summary>
            public static string P1Price = @"Userfloat6";
            /// <summary>Userfloat7</summary>
            public static string P2Price = @"Userfloat7";
            /// <summary>Userfloat8</summary>
            public static string P3Price = @"Userfloat8";
            /// <summary>Userfloat9</summary>
            public static string P4Price = @"Userfloat9";
            /// <summary>Userfloat10</summary>
            public static string P5Price = @"Userfloat10";


            /// <summary>Userfld7</summary>
            public static string FixedCOGType = @"Userfld7";
            /// <summary>Userfloat4</summary>
            public static string FixedCOGValue = @"Userfloat4";
        }

        #region Custom Properties

        /// <summary>
        /// IsUseSerialNo
        /// </summary>
        public bool IsUseSerialNo
        {
            get { return GetColumnValue<bool?>(UserColumns.IsUseSerialNo).GetValueOrDefault(false); }
            set { SetColumnValue(UserColumns.IsUseSerialNo, value); }
        }

        public bool IsConsignment
        {
            get
            {
                int? obj = GetColumnValue<int?>(UserColumns.IsConsignment);
                return obj.GetValueOrDefault(0) == 1;
            }
            set
            {
                SetColumnValue(UserColumns.IsConsignment, value ? 1 : 0);
            }
        }

        public bool IsOpenPricePackage
        {
            get
            {
                int? obj = GetColumnValue<int?>(UserColumns.IsOpenPricePackage);
                return obj.GetValueOrDefault(0) == 1;
            }
            set
            {
                SetColumnValue(UserColumns.IsOpenPricePackage, value ? 1 : 0);
            }
        }

        /// <summary>
        /// UOM
        /// </summary>
        public string UOM
        {
            get { return GetColumnValue<string>(UserColumns.UOM); }
            set { SetColumnValue(UserColumns.UOM, value); }
        }

        /// <summary>
        /// UOM
        /// </summary>
        public string MappedItemNo
        {
            get { return GetColumnValue<string>(UserColumns.MappedItemNo); }
            set { SetColumnValue(UserColumns.MappedItemNo, value); }
        }

        /// <summary>
        /// Base Level Value
        /// </summary>
        public int BaseLevel
        {
            get { return GetColumnValue<int>(UserColumns.BaseLevel); }
            set { SetColumnValue(UserColumns.BaseLevel, value); }
        }

        /// <summary>
        /// Cap Qty for Pre-Order
        /// </summary>
        public int CapQty
        {
            get { return GetColumnValue<int>(UserColumns.CapQty); }
            set { SetColumnValue(UserColumns.CapQty, value); }
        }

        /// <summary>
        /// IsMatrixItem
        /// </summary>
        public bool IsMatrixItem
        {
            get { return GetColumnValue<bool>(UserColumns.IsMatrixItem); }
            set { SetColumnValue(UserColumns.IsMatrixItem, value); }
        }

        /// <summary>
        /// Allow Pre-Order
        /// </summary>
        public bool AllowPreOrder
        {
            get { return GetColumnValue<bool>(UserColumns.AllowPreOrder); }
            set { SetColumnValue(UserColumns.AllowPreOrder, value); }
        }

        /// <summary>
        /// Accept PA Medifund funding method
        /// </summary>
        public bool IsPAMedifund
        {
            get { return GetColumnValue<bool>(UserColumns.IsPAMedifund); }
            set { SetColumnValue(UserColumns.IsPAMedifund, value); }
        }

        /// <summary>
        /// Accept SMF funding method
        /// </summary>
        public bool IsSMF
        {
            get { return GetColumnValue<bool>(UserColumns.IsSMF); }
            set { SetColumnValue(UserColumns.IsSMF, value); }
        }

        /// <summary>
        /// Vendor Delivery
        /// </summary>
        public bool IsVendorDelivery
        {
            get { return GetColumnValue<bool>(UserColumns.IsVendorDelivery); }
            set { SetColumnValue(UserColumns.IsVendorDelivery, value); }
        }

        /// <summary>
        /// Auto Capture Weight (Userflag6)
        /// </summary>
        public bool AutoCaptureWeight
        {
            get { return GetColumnValue<bool>(UserColumns.AutoCaptureWeight); }
            set { SetColumnValue(UserColumns.AutoCaptureWeight, value); }
        }

        /// <summary>
        /// Vendor Delivery
        /// </summary>
        public string IsPointRedeemable
        {
            get { return GetColumnValue<string>(UserColumns.IsPointRedeemable); }
            set { SetColumnValue(UserColumns.IsPointRedeemable, value); }
        }

        /// <summary>
        /// P1 Price
        /// </summary>
        public decimal? P1Price
        {
            get { return GetColumnValue<decimal?>(UserColumns.P1Price); }
            set { SetColumnValue(UserColumns.P1Price, value); }
        }
        /// <summary>
        /// P2 Price
        /// </summary>
        public decimal? P2Price
        {
            get { return GetColumnValue<decimal?>(UserColumns.P2Price); }
            set { SetColumnValue(UserColumns.P2Price, value); }
        }
        /// <summary>
        /// P3 Price
        /// </summary>
        public decimal? P3Price
        {
            get { return GetColumnValue<decimal?>(UserColumns.P3Price); }
            set { SetColumnValue(UserColumns.P3Price, value); }
        }
        /// <summary>
        /// P4 Price
        /// </summary>
        public decimal? P4Price
        {
            get { return GetColumnValue<decimal?>(UserColumns.P4Price); }
            set { SetColumnValue(UserColumns.P4Price, value); }
        }
        /// <summary>
        /// P5 Price
        /// </summary>
        public decimal? P5Price
        {
            get { return GetColumnValue<decimal?>(UserColumns.P5Price); }
            set { SetColumnValue(UserColumns.P5Price, value); }
        }

        /// <summary>
        /// Non Inventory Product (Userflag7)
        /// </summary>
        public bool NonInventoryProduct
        {
            get { return GetColumnValue<bool>(UserColumns.NonInventoryProduct); }
            set { SetColumnValue(UserColumns.NonInventoryProduct, value); }
        }

        /// <summary>
        /// Product Reference for Non Inventory Product(Userfld8)
        /// </summary>
        public string DeductedItem
        {
            get { return GetColumnValue<string>(UserColumns.DeductedItem); }
            set { SetColumnValue(UserColumns.DeductedItem, value); }
        }

        /// <summary>
        /// conv Rate for Deducted Item
        /// </summary>
        public decimal DeductConvRate
        {
            get { return GetColumnValue<decimal>(UserColumns.DeductConvRate); }
            set { SetColumnValue(UserColumns.DeductConvRate, value); }
        }

        /// <summary>
        /// conv Rate type for Deducted Item
        /// </summary>
        public bool DeductConvType
        {
            get { return GetColumnValue<bool>(UserColumns.DeductConvType); }
            set { SetColumnValue(UserColumns.DeductConvType, value); }
        }
        /// <summary>
        /// Fixed COG Type
        /// </summary>
        public string FixedCOGType
        {
            get { return GetColumnValue<string>(UserColumns.FixedCOGType); }
            set { SetColumnValue(UserColumns.FixedCOGType, value); }
        }

        /// <summary>
        /// Is Using Fixed COG (Userflag9)
        /// </summary>
        public bool IsUsingFixedCOG
        {
            get { return GetColumnValue<bool>(UserColumns.IsUsingFixedCOG); }
            set { SetColumnValue(UserColumns.IsUsingFixedCOG, value); }
        }

        /// <summary>
        /// Vendor Delivery
        /// </summary>
        public decimal FixedCOGValue
        {
            get { return GetColumnValue<decimal>(UserColumns.FixedCOGValue); }
            set { SetColumnValue(UserColumns.FixedCOGValue, value); }
        }


        /// <summary>
        /// Exclude Profit Loss
        /// </summary>
        public int ExcludeProfitLoss
        {
            get { return GetColumnValue<int>(UserColumns.ExcludeProfitLoss); }
            set { SetColumnValue(UserColumns.ExcludeProfitLoss, value); }
        }

        #endregion

        #region Custom Properties

        public DataTable OutletGroupPricing
        {
            get
            {
                DataTable dt = new DataTable();

                try
                {
                    string sql = @"SELECT	 TheItem.ItemNo
		                                ,TheItem.OutletGroupID
		                                ,TheItem.OutletGroupName
		                                ,ISNULL(OGI.RetailPrice, TheItem.RetailPrice) RetailPrice
                                        ,CASE WHEN OGI.P1 IS NULL THEN TheItem.Userfloat6 ELSE ISNULL(OGI.P1,0) END P1
                                        ,CASE WHEN OGI.P2 IS NULL THEN TheItem.Userfloat7 ELSE ISNULL(OGI.P2,0) END P2
                                        ,CASE WHEN OGI.P3 IS NULL THEN TheItem.Userfloat8 ELSE ISNULL(OGI.P3,0) END P3
                                        ,CASE WHEN OGI.P4 IS NULL THEN TheItem.Userfloat9 ELSE ISNULL(OGI.P4,0) END P4
                                        ,CASE WHEN OGI.P5 IS NULL THEN TheItem.Userfloat10 ELSE ISNULL(OGI.P5,0) END P5
                                        ,~CAST((CASE WHEN OGI.ItemNo IS NULL THEN TheItem.Deleted ELSE ISNULL(OGI.Deleted,0) END) AS bit) Active
                                FROM	(
		                                SELECT  I.ItemNo
				                                ,I.RetailPrice
				                                ,OG.OutletGroupID
				                                ,OG.OutletGroupName
                                                ,I.Userfloat6
                                                ,I.Userfloat7
                                                ,I.Userfloat8
                                                ,I.Userfloat9
                                                ,I.Userfloat10
                                                ,I.Deleted 
		                                FROM	Item I
				                                CROSS JOIN OutletGroup OG
		                                WHERE	I.ItemNo = '{0}' AND OG.Deleted = 0 
		                                ) TheItem LEFT JOIN OutletGroupItemMap OGI 
					                                ON OGI.ItemNo = TheItem.ItemNo 
					                                AND OGI.OutletGroupID = TheItem.OutletGroupID";
                    sql = string.Format(sql, this.ItemNo);
                    dt.Load(DataService.GetReader(new QueryCommand(sql)));
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }

                return dt;
            }
        }

        public DataTable OutletPricing
        {
            get
            {
                DataTable dt = new DataTable();

                try
                {
                    string sql = @"
                                    SELECT	 TheItem.ItemNo
		                                ,TheItem.OutletName
		                                ,ISNULL(OGI.RetailPrice, TheItem.RetailPrice) RetailPrice
                                        ,CASE WHEN OGI.P1 IS NULL THEN TheItem.Userfloat6 ELSE ISNULL(OGI.P1,0) END P1
                                        ,CASE WHEN OGI.P2 IS NULL THEN TheItem.Userfloat7 ELSE ISNULL(OGI.P2,0) END P2
                                        ,CASE WHEN OGI.P3 IS NULL THEN TheItem.Userfloat8 ELSE ISNULL(OGI.P3,0) END P3
                                        ,CASE WHEN OGI.P4 IS NULL THEN TheItem.Userfloat9 ELSE ISNULL(OGI.P4,0) END P4
                                        ,CASE WHEN OGI.P5 IS NULL THEN TheItem.Userfloat10 ELSE ISNULL(OGI.P5,0) END P5
                                        ,~CAST((CASE WHEN OGI.ItemNo IS NULL THEN TheItem.Deleted ELSE  ISNULL(OGI.Deleted,0) END) AS bit) Active
                                        FROM	(
		                                SELECT  I.ItemNo
				                                ,I.RetailPrice
				                                ,OG.OutletName
                                                ,I.Userfloat6
                                                ,I.Userfloat7
                                                ,I.Userfloat8
                                                ,I.Userfloat9
                                                ,I.Userfloat10
				                                ,I.Deleted 
		                                FROM	Item I
				                                CROSS JOIN Outlet OG
		                                WHERE	I.ItemNo = '{0}' AND OG.Deleted = 0
		                                ) TheItem 
		                                LEFT JOIN OutletGroupItemMap OGI 
					                                ON OGI.ItemNo = TheItem.ItemNo 
					                                AND OGI.OutletName = TheItem.OutletName
                                                    ";
                    sql = string.Format(sql, this.ItemNo);
                    dt.Load(DataService.GetReader(new QueryCommand(sql)));
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }

                return dt;
            }
        }

        #endregion
    }
}
