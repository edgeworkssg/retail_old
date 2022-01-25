using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOSLib.Controller.Commission
{
    public partial class CommissionController
    {
        public DataTable FetchCommissionList()
        {
            string sql = @"
                SELECT
                    CO.CommissionID
                    , CO.SalesGroupID
                    , UG.GroupName as SalesGroupName
                    , CO.CommissionType
                    , CO.CategoryName
                    , CO.ItemNo
                    , IT.ItemName
                    , CO.CommissionBasedOn
                    , CO.QuantityFrom
                    , CO.QuantityTo
                    , CO.AmountCommission
                    , CO.AmountFrom
                    , CO.AmountTo
                    , CO.PercentageCommission
                FROM Commission CO
                LEFT JOIN Item IT ON IT.ItemNo = CO.ItemNo
                LEFT JOIN UserGroup UG ON UG.GroupId = CO.SalesGroupID
            ";

            DataSet ds = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS"));
            return ds.Tables[0];
        }

        public bool Delete(int id)
        {
            string sql = @"DELETE FROM Commission WHERE CommissionID = @CommissionID";
            QueryCommand qc = new QueryCommand(sql);
            qc.AddParameter("@CommissionID", id);

            return DataService.ExecuteQuery(qc) > 0;
        }
    }
}
