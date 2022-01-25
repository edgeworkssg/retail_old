using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public partial class PurchaseOrderDet
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string PackingSizeName = @"Userfld1";
            /// <summary>Userfld2</summary>
            public static string Currency = @"Userfld2";
            /// <summary>Userfld3</summary>
            public static string POReorderRef = @"Userfld3";

            /// <summary>Userfloat1</summary>
            public static string GSTAmount = @"Userfloat1";
            /// <summary>Userfloat2</summary>
            public static string RetailPrice = @"Userfloat2";
            /// <summary>Userfloat3</summary>
            public static string PackingSizeCost = @"Userfloat3";
            /// <summary>Userfloat4</summary>
            public static string PackingSizeUOM = @"Userfloat4";
            /// <summary>Userfloat5</summary>
            public static string PackingQuantity = @"Userfloat5";
            /// <summary>Userfloat6</summary>
            public static string GeneratedQty = @"Userfloat6";
            /// <summary>Userfloat7</summary>
            public static string QtyReceived = @"Userfloat7";

            /// <summary>Userint1</summary>
            public static string GSTRule = @"Userint1";

            /// <summary>Userflag1</summary>
            public static string IsDetailDeleted = @"Userflag1"; 
        }
        #region Custom Properties

        public string PackingSizeName
        {
            get { return GetColumnValue<string>(UserColumns.PackingSizeName); }
            set { SetColumnValue(UserColumns.PackingSizeName, value); }
        }
        public string Currency
        {
            get { return GetColumnValue<string>(UserColumns.Currency); }
            set { SetColumnValue(UserColumns.Currency, value); }
        }

        public decimal GSTAmount
        {
            get { return GetColumnValue<decimal?>(UserColumns.GSTAmount).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.GSTAmount, value); }
        }
        public decimal RetailPrice
        {
            get { return GetColumnValue<decimal?>(UserColumns.RetailPrice).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.RetailPrice, value); }
        }
        public decimal PackingSizeCost
        {
            get { return GetColumnValue<decimal?>(UserColumns.PackingSizeCost).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.PackingSizeCost, value); }
        }
        public decimal PackingSizeUOM
        {
            get { return GetColumnValue<decimal?>(UserColumns.PackingSizeUOM).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.PackingSizeUOM, value); }
        }
        public decimal PackingQuantity
        {
            get { return GetColumnValue<decimal?>(UserColumns.PackingQuantity).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.PackingQuantity, value); }
        }

        /// <summary>
        /// QtyReceived (Userfloat7)
        /// </summary>
        public decimal? QtyReceived
        {
            get { return GetColumnValue<decimal?>(UserColumns.QtyReceived); }
            set { SetColumnValue(UserColumns.QtyReceived, value); }
        }

        public int GSTRule
        {
            get { return GetColumnValue<int?>(UserColumns.GSTRule).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.GSTRule, value); }
        }

        /// <summary>
        /// (Userfld3) As a reference to the auto created PurchaseOrderDet 
        /// </summary>
        public string POReorderRef
        {
            get { return GetColumnValue<string>(UserColumns.POReorderRef); }
            set { SetColumnValue(UserColumns.POReorderRef, value); }
        }

        public bool IsDetailDeleted
        {
            get { return GetColumnValue<bool>(UserColumns.IsDetailDeleted); }
            set { SetColumnValue(UserColumns.IsDetailDeleted, value); }
        }
        #endregion
    }
}
