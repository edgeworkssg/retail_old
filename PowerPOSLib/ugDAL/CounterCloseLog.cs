using SubSonic;
using System;

namespace PowerPOS
{
    public partial class CounterCloseLog
    {
        /// <summary>
        /// Fetch CounterCloseLog that contains CheckedTime
        /// </summary>
        /// <param name="PointOfSaleID">Point Of Sale that you want to check</param>
        /// <param name="CheckedTime">Any time that you want to check</param>
        public void LoadByClosingTime(int PointOfSaleID, DateTime CheckedTime)
        {
            SetSQLProps();
            InitSetDefaults();
            try
            {
                Query qr = new Query("CounterCloseLog");
                qr.Top = "1";
                qr.OrderBy = OrderBy.Desc(CounterCloseLog.Columns.EndTime);
                qr.QueryType = QueryType.Select;
                qr.AddWhere(CounterCloseLog.Columns.StartTime, Comparison.LessOrEquals, CheckedTime);
                qr.AddWhere(CounterCloseLog.Columns.EndTime, Comparison.GreaterThan, CheckedTime);
                qr.AddWhere(CounterCloseLog.Columns.PointOfSaleID, Comparison.Equals, PointOfSaleID);

                LoadAndCloseReader(qr.ExecuteReader());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        /// <summary>
        /// Fetch last CounterCloseLog
        /// </summary>
        /// <param name="PointOfSaleID">Point Of Sale that you want to check</param>
        public void LoadLastClosing(int PointOfSaleID)
        {
            SetSQLProps();
            InitSetDefaults();
            try
            {
                Query qr = new Query("CounterCloseLog");
                qr.Top = "1";
                qr.OrderBy = OrderBy.Desc(CounterCloseLog.Columns.EndTime);
                qr.QueryType = QueryType.Select;
                qr.AddWhere(CounterCloseLog.Columns.PointOfSaleID, Comparison.Equals, PointOfSaleID);

                LoadAndCloseReader(qr.ExecuteReader());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
    }
}
