namespace PowerPOS
{
    public partial class OrderDet
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string SalesPerson = @"Userfld1";
            /// <summary>Userfld2</summary>
            public static string PointItemNo = @"Userfld2";
            /// <summary>Userfld3</summary>
            public static string InstRefNo = @"Userfld3";
            /// <summary>Userfld4</summary>
            public static string LineInfo = @"Userfld4";
            /// <summary>Userfld5</summary>
            public static string ReturnedReceiptNo = @"Userfld5";
            /// <summary>Userfld6</summary>
            public static string SpecialDiscount = @"Userfld6";
            /// <summary>Userfld7</summary>
            public static string DiscountReason = @"Userfld7";
            /// <summary>Userfld8</summary>
            public static string DiscountDetail = @"Userfld8";
            /// <summary>Userfld9</summary>
            public static string SpecialBarcode = @"Userfld9";
            /// <summary>Userfld10</summary>
            public static string OrderPayment = @"Userfld10";
            /// <summary>Userfld11</summary>
            public static string DiscountAuthorizedBy = @"Userfld11";
            /// <summary>Userfld12</summary>
            public static string UOM = @"Userfld12";
            /// <summary>Userfld13</summary>
            public static string RefundOrderDetID = @"Userfld13";
            /// <summary>Userfld14</summary>
            public static string PackingSize = @"Userfld14";
            /// <summary>Userfld15</summary>
            public static string OldItemNo = @"Userfld15";
            /// <summary>Userfld16</summary>
            public static string PossiblePromoID = @"Userfld16";
            /// <summary>Userfld17</summary>
            public static string PossibleItemGroupID = @"Userfld17";
            /// <summary>Userfld18</summary>
            public static string POReorderRef = @"Userfld18";
            /// <summary>Userfld19</summary>
            public static string PriceMode = @"Userfld19";
            /// <summary>Userfld20</summary>
            public static string SalesPerson2 = @"Userfld20";


            /// <summary>Userfloat1</summary>
            public static string DepositAmount = @"Userfloat1";
            /// <summary>Userfloat2</summary>
            public static string FundingAmount = @"Userfloat2";
            /// <summary>Userfloat3</summary>
            public static string Discount2Percent = @"Userfloat3";
            /// <summary>Userfloat4</summary>
            public static string Discount2Dollar = @"Userfloat4";
            /// <summary>Userfloat5</summary>
            public static string OriginalPriceOfSPP = @"Userfloat5";
            /// <summary>Userfloat6</summary>
            public static string PointGetAmount = @"Userfloat6";
            /// <summary>Userfloat7</summary>
            public static string PackingQty = @"Userfloat7";
            /// <summary>Userfloat8</summary>
            public static string PackageBreakdownAmount = @"Userfloat8";
            /// <summary>Userfloat9</summary>
            public static string FinalPrice = @"Userfloat9";

            /// <summary>Userflag2</summary>
            public static string IsNotified = @"Userflag2";
            /// <summary>Userflag1</summary>
            public static string IsChangetoNonGST = @"Userflag1";
            /// <summary>Userflag3</summary>
            public static string IsPromoPossibilityChecked = @"Userflag3";
            /// <summary>Userflag5</summary>
            public static string IsPackageRedeemed = @"Userflag5";
            /// <summary>Userflag5</summary>
            public static string IsPriceManuallyChange = @"Userflag4";

            /// <summary>Userint1</summary>
            public static string PreOrderPointOfSalesID = @"Userint1";
        }
        
        #region Custom Properties

        public decimal PointGetAmount
        {
            get { return GetColumnValue<decimal?>(UserColumns.PointGetAmount).GetValueOrDefault(0); }
            set
            {
                if (value == 0)
                    SetColumnValue(UserColumns.PointGetAmount, null);
                else
                    SetColumnValue(UserColumns.PointGetAmount, value);
            }
        }

        /// <summary>
        /// Line Sales Person for each OrderDet
        /// </summary>
        public string SalesPerson
        {
            get { return GetColumnValue<string>(UserColumns.SalesPerson); }
            set { SetColumnValue(UserColumns.SalesPerson, value); }
        }

        /// <summary>
        /// Line Sales Person for each OrderDet
        /// </summary>
        public string SalesPerson2
        {
            get { return GetColumnValue<string>(UserColumns.SalesPerson2); }
            set { SetColumnValue(UserColumns.SalesPerson2, value); }
        }

        /// <summary>
        /// Line Sales Person for each OrderDet
        /// </summary>
        public string OldItemNo
        {
            get { return GetColumnValue<string>(UserColumns.OldItemNo); }
            set { SetColumnValue(UserColumns.OldItemNo, value); }
        }

        public string PointItemNo
        {
            get { return GetColumnValue<string>(UserColumns.PointItemNo); }
            set { SetColumnValue(UserColumns.PointItemNo, value); }
        }

        /// <summary>
        /// Reference Number, only for Installment Payment
        /// </summary>
        public string InstRefNo
        {
            get { return GetColumnValue<string>(UserColumns.InstRefNo); }
            set { SetColumnValue(UserColumns.InstRefNo, value); }
        }
        public string LineInfo
        {
            get { return GetColumnValue<string>(UserColumns.LineInfo); }
            set { SetColumnValue(UserColumns.LineInfo, value); }
        }
        public string ReturnedReceiptNo
        {
            get { return GetColumnValue<string>(UserColumns.ReturnedReceiptNo); }
            set { SetColumnValue(UserColumns.ReturnedReceiptNo, value); }
        }

		public string SpecialDiscount
        {
            get { return GetColumnValue<string>(UserColumns.SpecialDiscount); }
            set { SetColumnValue(UserColumns.SpecialDiscount, value); }
        }
        public string DiscountReason
        {
            get { return GetColumnValue<string>(UserColumns.DiscountReason); }
            set { SetColumnValue(UserColumns.DiscountReason, value); }
        }
        public string DiscountDetail
        {
            get { return GetColumnValue<string>(UserColumns.DiscountDetail); }
            set { SetColumnValue(UserColumns.DiscountDetail, value); }
        }
        public string SpecialBarcode
        {
            get { return GetColumnValue<string>(UserColumns.SpecialBarcode); }
            set { SetColumnValue(UserColumns.SpecialBarcode, value); }
        }
        public string OrderPayment
        {
            get { return GetColumnValue<string>(UserColumns.OrderPayment); }
            set { SetColumnValue(UserColumns.OrderPayment, value); }
        }
        public string DiscountAuthorizedBy
        {
            get { return GetColumnValue<string>(UserColumns.DiscountAuthorizedBy); }
            set { SetColumnValue(UserColumns.DiscountAuthorizedBy, value); }
        }
        public string UOM
        {
            get { return GetColumnValue<string>(UserColumns.UOM); }
            set { SetColumnValue(UserColumns.UOM, value); }
        }

        public string RefundOrderDetID
        {
            get { return GetColumnValue<string>(UserColumns.RefundOrderDetID); }
            set { SetColumnValue(UserColumns.RefundOrderDetID, value); }
        }
        public decimal DepositAmount
        {
            get { return GetColumnValue<decimal>(UserColumns.DepositAmount); }
            set { SetColumnValue(UserColumns.DepositAmount, value); }
        }

        public decimal FundingAmount
        {
            get { return GetColumnValue<decimal>(UserColumns.FundingAmount); }
            set { SetColumnValue(UserColumns.FundingAmount, value); }
        }

        public decimal Discount2Percent
        {
            get { return GetColumnValue<decimal?>(UserColumns.Discount2Percent).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.Discount2Percent, value); }
        }

        public decimal Discount2Dollar
        {
            get { return GetColumnValue<decimal?>(UserColumns.Discount2Dollar).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.Discount2Dollar, value); }
        }

        public decimal FinalPrice
        {
            get { return GetColumnValue<decimal?>(UserColumns.FinalPrice).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.FinalPrice, value); }
        }

        public decimal OriginalPriceOfSPP
        {
            get { return GetColumnValue<decimal?>(UserColumns.OriginalPriceOfSPP).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.OriginalPriceOfSPP, value); }
        }

        /// <summary>
        /// Whether email notification has been sent to customer to notify the availability of this ordered item.
        /// </summary>
        public bool IsNotified
        {
            get { return GetColumnValue<bool>(UserColumns.IsNotified); }
            set { SetColumnValue(UserColumns.IsNotified, value); }
        }

        public string PackingSize
        {
            get { return GetColumnValue<string>(UserColumns.PackingSize); }
            set { SetColumnValue(UserColumns.PackingSize, value); }
        }

        public decimal? PackingQty
        {
            get { return GetColumnValue<decimal?>(UserColumns.PackingQty); }
            set { SetColumnValue(UserColumns.PackingQty, value); }
        }


        public decimal PackageBreakdownAmount
        {
            get { return GetColumnValue<decimal?>(UserColumns.PackageBreakdownAmount).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.PackageBreakdownAmount, value); }
        }

        public bool IsChangetoNonGST
        {
            get { return GetColumnValue<bool>(UserColumns.IsChangetoNonGST); }
            set { SetColumnValue(UserColumns.IsChangetoNonGST, value); }
        }

        public bool IsPromoPossibilityChecked
        {
            get { return GetColumnValue<bool>(UserColumns.IsPromoPossibilityChecked); }
            set { SetColumnValue(UserColumns.IsPromoPossibilityChecked, value); }
        }

        public bool IsPackageRedeemed
        {
            get { return GetColumnValue<bool>(UserColumns.IsPackageRedeemed); }
            set { SetColumnValue(UserColumns.IsPackageRedeemed, value); }
        }

        public string PossiblePromoID
        {
            get { return GetColumnValue<string>(UserColumns.PossiblePromoID); }
            set { SetColumnValue(UserColumns.PossiblePromoID, value); }
        }
        public string PossibleItemGroupID
        {
            get { return GetColumnValue<string>(UserColumns.PossibleItemGroupID); }
            set { SetColumnValue(UserColumns.PossibleItemGroupID, value); }
        }

        /// <summary>
        /// (Userfld18) As a reference to the auto created PurchaseOrderDet 
        /// </summary>
        public string POReorderRef
        {
            get { return GetColumnValue<string>(UserColumns.POReorderRef); }
            set { SetColumnValue(UserColumns.POReorderRef, value); }
        }

        /// <summary>
        /// (Userfld19) As a reference to the Price Mode applied into the item 
        /// </summary>
        public string PriceMode
        {
            get { return GetColumnValue<string>(UserColumns.PriceMode); }
            set { SetColumnValue(UserColumns.PriceMode, value); }
        }

        /// <summary>
        /// (Userfld19) As a reference to the Price Mode applied into the item 
        /// </summary>
        public bool IsPriceManuallyChange
        {
            get { return GetColumnValue<bool>(UserColumns.IsPriceManuallyChange); }
            set { SetColumnValue(UserColumns.IsPriceManuallyChange, value); }
        }

        /// <summary>
        /// (Userint1) As a reference for inventory location for pre order item 
        /// </summary>
        public int PreOrderPointOfSalesID
        {
            get { return GetColumnValue<int>(UserColumns.PreOrderPointOfSalesID); }
            set { SetColumnValue(UserColumns.PreOrderPointOfSalesID, value); }
        }
        #endregion


    }
}