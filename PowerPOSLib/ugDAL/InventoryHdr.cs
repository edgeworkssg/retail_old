using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class InventoryHdr
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string VendorInvoiceNo = @"Userfld1";
            /// <summary>Userfld5</summary>
            public static string CustomRefNo = @"Userfld5";
            /// <summary>Userfld6</summary>
            public static string CustomField1 = @"Userfld6";
            /// <summary>Userfld7</summary>
            public static string CustomField2 = @"Userfld7";
            /// <summary>Userfld8</summary>
            public static string CustomField3 = @"Userfld8";
            /// <summary>Userfld9</summary>
            public static string CustomField4 = @"Userfld9";
            /// <summary>Userfld10</summary>
            public static string CustomField5 = @"Userfld10";
            /// <summary>Userfloat1</summary>
            public static string AdditionalCost1 = @"Userfloat1";
            /// <summary>Userfloat2</summary>
            public static string AdditionalCost2 = @"Userfloat2";
            /// <summary>Userfloat3</summary>
            public static string AdditionalCost3 = @"Userfloat3";
            /// <summary>Userfloat4</summary>
            public static string AdditionalCost4 = @"Userfloat4";
            /// <summary>Userfloat5</summary>
            public static string AdditionalCost5 = @"Userfloat5";

            /// <summary>Userfld5</summary>
            public static string StockDocumentNo = @"Userfld3";


            /// <summary>UserInt1</summary>
            public static string GSTRule = @"Userint1";
        }

        #region Custom Properties
        /// <summary>
        /// Vendor Invoice No
        /// </summary>
        public string VendorInvoiceNo
        {
            get { return GetColumnValue<string>(UserColumns.VendorInvoiceNo); }
            set { SetColumnValue(UserColumns.VendorInvoiceNo, value); }
        }

        
        /// <summary>
        /// Custom Ref No
        /// </summary>
        public string CustomRefNo
        {
            get { return GetColumnValue<string>(UserColumns.CustomRefNo); }
            set { SetColumnValue(UserColumns.CustomRefNo, value); }
        }

        /// <summary>
        /// CustomField1
        /// </summary>
        public string CustomField1
        {
            get { return GetColumnValue<string>(UserColumns.CustomField1); }
            set { SetColumnValue(UserColumns.CustomField1, value); }
        }
        /// <summary>
        /// CustomField2
        /// </summary>
        public string CustomField2
        {
            get { return GetColumnValue<string>(UserColumns.CustomField2); }
            set { SetColumnValue(UserColumns.CustomField2, value); }
        }
        /// <summary>
        /// CustomField3
        /// </summary>
        public string CustomField3
        {
            get { return GetColumnValue<string>(UserColumns.CustomField3); }
            set { SetColumnValue(UserColumns.CustomField3, value); }
        }
        /// <summary>
        /// CustomField4
        /// </summary>
        public string CustomField4
        {
            get { return GetColumnValue<string>(UserColumns.CustomField4); }
            set { SetColumnValue(UserColumns.CustomField4, value); }
        }
        /// <summary>
        /// CustomField5
        /// </summary>
        public string CustomField5
        {
            get { return GetColumnValue<string>(UserColumns.CustomField5); }
            set { SetColumnValue(UserColumns.CustomField5, value); }
        }
        /// <summary>
        /// CustomFloat1
        /// </summary>
        public decimal AdditionalCost1
        {
            get { return GetColumnValue<decimal>(UserColumns.AdditionalCost1); }
            set { SetColumnValue(UserColumns.AdditionalCost1, value); }
        }
        /// <summary>
        /// CustomFloat2
        /// </summary>
        public decimal AdditionalCost2
        {
            get { return GetColumnValue<decimal>(UserColumns.AdditionalCost2); }
            set { SetColumnValue(UserColumns.AdditionalCost2, value); }
        }
        /// <summary>
        /// CustomFloat3
        /// </summary>
        public decimal AdditionalCost3
        {
            get { return GetColumnValue<decimal>(UserColumns.AdditionalCost3); }
            set { SetColumnValue(UserColumns.AdditionalCost3, value); }
        }
        /// <summary>
        /// CustomFloat4
        /// </summary>
        public decimal AdditionalCost4
        {
            get { return GetColumnValue<decimal>(UserColumns.AdditionalCost4); }
            set { SetColumnValue(UserColumns.AdditionalCost4, value); }
        }
        /// <summary>
        /// CustomFloat5
        /// </summary>
        public decimal AdditionalCost5
        {
            get { return GetColumnValue<decimal>(UserColumns.AdditionalCost5); }
            set { SetColumnValue(UserColumns.AdditionalCost5, value); }
        }
       
        /// <summary>
        /// Stock Document No
        /// </summary>
        public string StockDocumentNo
        {
            get { return GetColumnValue<string>(UserColumns.StockDocumentNo); }
            set { SetColumnValue(UserColumns.StockDocumentNo, value); }
        }

        /// <summary>
        /// GSTRule
        /// </summary>
        public int GSTRule
        {
            get { return GetColumnValue<int>(UserColumns.GSTRule); }
            set { SetColumnValue(UserColumns.GSTRule, value); }
        }
        #endregion
    }
}
