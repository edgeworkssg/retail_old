using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class Supplier
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string PaymentTerm = @"Userfld1";
            /// <summary>Userfld2</summary>
            public static string Currency = @"Userfld2";
            /// <summary>Userfld3</summary>
            public static string SupplierCode = @"Userfld3";
            /// <summary>Userfld4</summary>
            public static string LinkedUser = @"Userfld4";
            /// <summary>Userint1</summary>
            public static string GSTRule = @"Userint1";
            /// <summary>Userint2</summary>
            public static string WarehouseID = @"Userint2";
            /// <summary>Userfloat1</summary>
            public static string MinimumOrder = @"Userfloat1";
            /// <summary>Userfloat2</summary>
            public static string DeliveryCharge = @"Userfloat2";
            /// <summary>Userflag1</summary>
            public static string IsWarehouse = @"Userflag1";
        }

        #region Custom Properties

        public string GST
        {
            get
            {
                string _gst = "Non GST";
                if (Userint1 == 1)
                    _gst = "Exclusive GST";
                else if (Userint1 == 2)
                    _gst = "Inclusive GST";
                return _gst;
            }
        }

        public string PaymentTerm
        {
            get { return GetColumnValue<string>(UserColumns.PaymentTerm); }
            set { SetColumnValue(UserColumns.PaymentTerm, value); }
        }
        public string Currency
        {
            get { return GetColumnValue<string>(UserColumns.Currency); }
            set { SetColumnValue(UserColumns.Currency, value); }
        }
        public string SupplierCode
        {
            get { return GetColumnValue<string>(UserColumns.SupplierCode); }
            set { SetColumnValue(UserColumns.SupplierCode, value); }
        }
        public string LinkedUser
        {
            get { return GetColumnValue<string>(UserColumns.LinkedUser); }
            set { SetColumnValue(UserColumns.LinkedUser, value); }
        }
        public string GSTRule
        {
            get { return GetColumnValue<string>(UserColumns.GSTRule); }
            set { SetColumnValue(UserColumns.GSTRule, value); }
        }
        public string MinimumOrder
        {
            get { return GetColumnValue<string>(UserColumns.MinimumOrder); }
            set { SetColumnValue(UserColumns.MinimumOrder, value); }
        }
        public string DeliveryCharge
        {
            get { return GetColumnValue<string>(UserColumns.DeliveryCharge); }
            set { SetColumnValue(UserColumns.DeliveryCharge, value); }
        }

        public int? WarehouseID
        {
            get { return GetColumnValue<int?>(UserColumns.WarehouseID); }
            set { SetColumnValue(UserColumns.WarehouseID, value); }
        }

        public bool? IsWarehouse
        {
            get { return GetColumnValue<bool?>(UserColumns.IsWarehouse); }
            set { SetColumnValue(UserColumns.IsWarehouse, value); }
        }

        #endregion
    }
}
