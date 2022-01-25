using System.Data;
using System;
using SubSonic;
namespace PowerPOS
{
    public partial class RedemptionItemController
    {
        public DataTable FetchValidRedemptionItems(DateTime redemptionDate, decimal minPoints, decimal maxPoints)
        {

            //pull out from redemption item table view
            ViewRedemptionItemCollection v = new ViewRedemptionItemCollection();
            v.Where(ViewRedemptionItem.Columns.ValidStartDate, Comparison.LessOrEquals, redemptionDate);
            v.Where(ViewRedemptionItem.Columns.ValidEndDate, Comparison.GreaterOrEquals, redemptionDate);
            if (minPoints != 0) v.Where(ViewRedemptionItem.Columns.PointRequired, Comparison.GreaterOrEquals, minPoints);
            if (maxPoints != 0) v.Where(ViewRedemptionItem.Columns.PointRequired, Comparison.LessOrEquals, maxPoints);
            v.OrderByDesc(ViewRedemptionItem.Columns.PointRequired);
            return v.Load().ToDataTable();

        }

    }
}
