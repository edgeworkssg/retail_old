using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class InventoryDet
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string DORefNo = @"Userfld1";
            /// <summary>Userfld2</summary>
            public static string PurchaseOrderDetRefNo = @"Userfld2";
            /// <summary>Userfld3</summary>
            public static string OldItemNo = @"Userfld3";
            /// <summary>Userfld4</summary>
            public static string OrderDetID = @"Userfld4";
            /// <summary>Userfld6</summary>
            public static string RetailPrice = @"Userfld6";
            /// <summary>Userfld10</summary>
            public static string SerialNo = @"Userfld10";



            /// <summary>Userfloat1</summary>
            public static string InitialFactoryPrice = @"Userfloat1";
            /// <summary>Userfloat2</summary>
            public static string TotalCost = @"Userfloat2";
            /// <summary>Userfloat3</summary>
            public static string AdditionalCost = @"Userfloat3";
            /// <summary>Userfloat4</summary>
            public static string TotalInitialCost = @"Userfloat4";
            /// <summary>Userfloat5</summary>
            public static string AlternateCostPrice = @"Userfloat5";

            /// <summary>Userflag1</summary>
            public static string UseFixedCOGS = @"Userflag1";
        }

        #region Custom Properties

        public List<string> SerialNo
        {
            get
            {
                List<string> data = new List<string>();

                string colVal = GetColumnValue<string>(UserColumns.SerialNo);
                if (!string.IsNullOrEmpty(colVal))
                    data = colVal.ConvertFromJSON<List<string>>();

                return data;
            }
            set
            {
                if (value == null)
                    return;

                SetColumnValue(UserColumns.SerialNo, value.ConvertToJSON());
            }
        }

        public string SerialNoString
        {
            get { return GetColumnValue<string>(UserColumns.SerialNo); }
            set { SetColumnValue(UserColumns.SerialNo, value); }
        }

        public bool UseFixedCOGS
        {
            get { return GetColumnValue<bool?>(UserColumns.UseFixedCOGS).GetValueOrDefault(false); }
            set { SetColumnValue(UserColumns.UseFixedCOGS, value); }
        }

        /// <summary>
        /// Delivery OrderDetID that is referenced by this stock out
        /// </summary>
        public string DORefNo
        {
            get { return GetColumnValue<string>(UserColumns.DORefNo); }
            set { SetColumnValue(UserColumns.DORefNo, value); }
        }

        /// <summary>
        /// Delivery OrderDetID that is referenced by this stock out
        /// </summary>
        public string OldItemNo
        {
            get { return GetColumnValue<string>(UserColumns.OldItemNo); }
            set { SetColumnValue(UserColumns.OldItemNo, value); }
        }

        /// <summary>
        /// Purchase Order Det RefNo for item that received from Purchase Order
        /// </summary>
        public string PurchaseOrderDetRefNo
        {
            get { return GetColumnValue<string>(UserColumns.PurchaseOrderDetRefNo); }
            set { SetColumnValue(UserColumns.PurchaseOrderDetRefNo, value); }
        }

        /// <summary>
        /// OrderDetID (Userfld4)
        /// </summary>
        public string OrderDetID
        {
            get { return GetColumnValue<string>(UserColumns.OrderDetID); }
            set { SetColumnValue(UserColumns.OrderDetID, value); }
        }

        /// <summary>
        /// RetailPrice (Userfld6)
        /// </summary>
        public decimal? RetailPrice
        {
            get
            {
                string price = GetColumnValue<string>(UserColumns.RetailPrice);
                decimal result;
                if (decimal.TryParse(price, out result))
                    return result;
                else
                    return null;
            }
            set
            {
                SetColumnValue(UserColumns.RetailPrice, value.HasValue ? value.Value.ToString("N2") : null);
            }
        }

        /// <summary>
        /// Initial Factory Price
        /// </summary>
        public decimal? InitialFactoryPrice
        {
            get { return GetColumnValue<decimal?>(UserColumns.InitialFactoryPrice); }
            set { SetColumnValue(UserColumns.InitialFactoryPrice, value); }
        }

        /// <summary>
        /// Total Cost
        /// </summary>
        public decimal? TotalCost
        {
            get { return GetColumnValue<decimal?>(UserColumns.TotalCost); }
            set { SetColumnValue(UserColumns.TotalCost, value); }
        }

        /// <summary>
        /// Total Initial Cost
        /// </summary>
        public decimal TotalInitialCost
        {
            get { return GetColumnValue<decimal>(UserColumns.TotalInitialCost); }
            set { SetColumnValue(UserColumns.TotalInitialCost, value); }
        }

        /// <summary>
        /// Additional Cost
        /// </summary>
        public decimal? AdditionalCost
        {
            get { return GetColumnValue<decimal?>(UserColumns.AdditionalCost); }
            set { SetColumnValue(UserColumns.AdditionalCost, value); }
        }

        /// <summary>
        /// Alternate Cost Price
        /// </summary>
        public decimal? AlternateCostPrice
        {
            get { return GetColumnValue<decimal?>(UserColumns.AlternateCostPrice); }
            set { SetColumnValue(UserColumns.AlternateCostPrice, value); }
        }
        #endregion

    }
}
