using System;
using SubSonic;

namespace PowerPOS
{
    public partial class OrderHdrCollection
    {
        /// <summary>
        /// Fetch all CounterCloseLog that has CounterCloseLog.StartTime bigger that/equal with inputed StartTime and CounterCloseLog.EndTime lesser that inputed EndTime.
        /// </summary>
        /// <param name="PointOfSaleID">Point Of Sale that you want to check</param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        public void LoadByTime(int PointOfSaleID, DateTime StartTime, DateTime EndTime)
        {
            try
            {
                Query qr = new Query("OrderHdr");
                qr.OrderBy = OrderBy.Desc(OrderHdr.Columns.OrderDate);
                qr.QueryType = QueryType.Select;
                qr.AddWhere(OrderHdr.Columns.OrderDate, Comparison.GreaterOrEquals, StartTime);
                qr.AddWhere(OrderHdr.Columns.OrderDate, Comparison.LessThan, EndTime);
                qr.AddWhere(OrderHdr.Columns.PointOfSaleID, Comparison.Equals, PointOfSaleID);

                LoadAndCloseReader(qr.ExecuteReader());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        /// <summary>
        /// Get the minimum Start Time in the collection
        /// </summary>
        /// <returns>Will return [DateTime.Max] if there is no data inside</returns>
        public DateTime Min_OrderDate()
        {
            DateTime Output = DateTime.MaxValue;

            for (int Counter = 0; Counter < this.Count; Counter++)
            {
                if (Output.CompareTo(this[Counter].OrderDate) > 0)
                    Output = this[Counter].OrderDate;
            }

            return Output;
        }
        /// <summary>
        /// Get the maximum End Time in the collection
        /// </summary>
        /// <returns>Will return [DateTime.Min] if there is no data inside</returns>
        public DateTime Max_OrderDate()
        {
            DateTime Output = DateTime.MinValue;

            for (int Counter = 0; Counter < this.Count; Counter++)
            {
                if (Output.CompareTo(this[Counter].OrderDate) < 0)
                    Output = this[Counter].OrderDate;
            }

            return Output;
        }
    }
}
