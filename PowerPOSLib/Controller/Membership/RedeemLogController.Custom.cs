using System.Data;
using System;
using SubSonic;
namespace PowerPOS
{
    public partial class RedeemLogController
    {
        public DataTable FetchReedemHistory(DateTime startDate, DateTime endDate)
        {
            try
            {
                ViewRedeemLogCollection v = new ViewRedeemLogCollection();
                v.Where(ViewRedeemLog.Columns.RedeemDate, Comparison.GreaterOrEquals, startDate);
                v.Where(ViewRedeemLog.Columns.RedeemDate, Comparison.LessOrEquals, endDate);
                return v.Load().ToDataTable();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }

        }

    }
}
