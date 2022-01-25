using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOS
{
    public partial class RatingController
    {
        public static DataTable GetRatingSystemList()
        {
            string sql = "Select *, CASE ISNULL(RatingImageUrl,'') WHEN '' THEN '' ELSE '~/Rating/' + RatingImageUrl END as ImageUrl from RatingMaster order by Rating desc";
            DataSet ds = DataService.GetDataSet(new QueryCommand(sql));

            return ds.Tables[0];
        }

        public static DataTable GetRatingFeedbackList(string RatingType)
        {
            string sql = "Select *, CASE ISNULL(SelectionImageUrl,'') WHEN '' THEN '' ELSE '~/Rating/' + SelectionImageUrl END as ImageUrl from RatingFeedback where ISNULL(deleted,0) = 0 and RatingType = '" + RatingType + "' order by SelectionText";
            DataSet ds = DataService.GetDataSet(new QueryCommand(sql));

            return ds.Tables[0];
        }
    }
}
