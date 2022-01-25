﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public partial class PurchaseOrderHdr
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld3</summary>
            public static string DeliveryDate = @"Userfld3";
            /// <summary>Userfld4</summary>
            public static string DeliveryAddress = @"Userfld4";
            /// <summary>Userfld5</summary>
            public static string PaymentTerm = @"Userfld5";
            /// <summary>Userfld6</summary>
            public static string ReceivingTime = @"Userfld6";
            /// <summary>Userfld7</summary>
            public static string Status = @"Userfld7";
            /// <summary>Userfld10</summary>
            public static string GSTType = @"Userfld10";
            /// <summary>Userfloat1</summary>
            public static string MinPurchase = @"Userfloat1";
            /// <summary>Userfloat2</summary>
            public static string DelCharge = @"Userfloat2";
            /// <summary>Userflag1</summary>
            public static string IsAutoGenerated = @"Userflag1";
            /// <summary>Userfld2</summary>
            public static string GoodsOrderRefNo = @"Userfld2";
        }
        #region Custom Properties

        public string DeliveryDate
        {
            get { return GetColumnValue<string>(UserColumns.DeliveryDate); }
            set { SetColumnValue(UserColumns.DeliveryDate, value); }
        }
        public string DeliveryAddress
        {
            get { return GetColumnValue<string>(UserColumns.DeliveryAddress); }
            set { SetColumnValue(UserColumns.DeliveryAddress, value); }
        }

        public string PaymentTerm
        {
            get { return GetColumnValue<string>(UserColumns.PaymentTerm); }
            set { SetColumnValue(UserColumns.PaymentTerm, value); }
        }

        public string ReceivingTime
        {
            get { return GetColumnValue<string>(UserColumns.ReceivingTime); }
            set { SetColumnValue(UserColumns.ReceivingTime, value); }
        }

        public string Status
        {
            get { return GetColumnValue<string>(UserColumns.Status); }
            set { SetColumnValue(UserColumns.Status, value); }
        }

        public string GSTType
        {
            get { return GetColumnValue<string>(UserColumns.GSTType); }
            set { SetColumnValue(UserColumns.GSTType, value); }
        }

        public decimal MinPurchase
        {
            get { return GetColumnValue<decimal?>(UserColumns.MinPurchase).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.MinPurchase, value); }
        }

        public decimal DelCharge
        {
            get { return GetColumnValue<decimal?>(UserColumns.DelCharge).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.DelCharge, value); }
        }

        public string GoodsOrderRefNo
        {
            get { return GetColumnValue<string>(UserColumns.GoodsOrderRefNo); }
            set { SetColumnValue(UserColumns.GoodsOrderRefNo, value); }
        }
        
        #endregion
    }
}
