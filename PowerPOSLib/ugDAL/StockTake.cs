using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class StockTake
    {
        public partial struct UserColumns
        {
            /// <summary>userfld1</summary>
            public static string BatchNo = @"userfld1";

            /// <summary>userfld2</summary>
            public static string UniqueID = @"userfld2";
            
            /// <summary>userfld3</summary>
            public static string StockTakeDocumentNo = @"userfld3";

            /// <summary>Userfld10</summary>
            public static string SerialNo = @"Userfld10";

            /// <summary>userflag1</summary>
            public static string Deleted = @"userflag1";
        }


        #region *) Custom Properties
        /// <summary>Userfld10 - SerialNo</summary>
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
        /// <summary>userfld1 - BatchNo</summary>
        public string BatchNo
        {
            get { return GetColumnValue<string>(UserColumns.BatchNo); }
            set { SetColumnValue(UserColumns.BatchNo, value); }
        }

        /// <summary>userfld2 - UniqueID</summary>
        public string UniqueID
        {
            get { return GetColumnValue<string>(UserColumns.UniqueID); }
            set { SetColumnValue(UserColumns.UniqueID, value); }
        }

        /// <summary>userfld3 - StockTakeDocumentNo</summary>
        public string StockTakeDocumentNo
        {
            get { return GetColumnValue<string>(UserColumns.StockTakeDocumentNo); }
            set { SetColumnValue(UserColumns.StockTakeDocumentNo, value); }
        }

        /// <summary>userflag1 - Deleted</summary>
        public bool Deleted
        {
            get { return GetColumnValue<bool>(UserColumns.Deleted); }
            set { SetColumnValue(UserColumns.Deleted, value); }
        }
        #endregion

        #region *) Custom Methods
        public static bool DeleteLogically(int StockTakeID)
        {
            StockTake st = new StockTake(StockTakeID);
            if (st != null && st.StockTakeID == StockTakeID)
            {
                st.Deleted = true;
                st.Save();
                return true;
            }
            else
            {
                Logger.writeLog(string.Format("Failed to delete StockTake data. StockTakeID {0} doesn't exist.", StockTakeID.ToString()));
                return false;
            }
        }
        #endregion
    }
}
