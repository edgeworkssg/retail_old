namespace PowerPOS
{
    public partial class ReceiptDet
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1 - Item No of point package used to do this payment</summary>
            public static string PointItemNo = @"Userfld1";
        }

        #region Custom Properties
        public string PointItemNo
        {
            get { return GetColumnValue<string>(UserColumns.PointItemNo); }
            set { SetColumnValue(UserColumns.PointItemNo, value); }
        }
        #endregion
    }
}
