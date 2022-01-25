using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using System.Data;
using SubSonic;

namespace PowerWeb.API.Lookup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class RatingReport : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string msg = "";
            string groupby = context.Request.Params["groupby"];
            string filter = context.Request.Params["filter"];
            string startdate = context.Request.Params["startdate"];
            string enddate = context.Request.Params["enddate"];
            string filteroutlet = context.Request.Params["filteroutlet"];
            string filterstaff = context.Request.Params["filterstaff"];

            if (string.IsNullOrEmpty(filteroutlet) || filteroutlet == "null")
                filteroutlet = "";

            DataTable detail = new DataTable();
            DataTable header = new DataTable();

            string query = "";
            string query2 = "";

            DateTime currentStart = DateTime.Parse(startdate);
            DateTime currentEnd = DateTime.Parse(enddate);

            DateTime lastStart = new DateTime();
            DateTime lastEnd = new DateTime();

            if (filter.ToUpper() == "YEAR")
            {
                lastStart = currentStart.AddYears(-1);
                lastEnd = currentEnd.AddYears(-1);
            }
            else if (filter.ToUpper() == "MONTH")
            {
                lastStart = currentStart.AddMonths(-1);
                lastEnd = currentEnd.AddMonths(-1);
            }
            else if (filter.ToUpper() == "WEEK")
            {
                lastStart = currentStart.AddDays(-7);
                lastEnd = currentEnd.AddDays(-7);
            }
            else
            {
                lastStart = currentStart.AddDays(-1);
                lastEnd = currentEnd.AddDays(-1);
            }

            try
            {
                if (groupby == "outlet")
                {
                    query2 = @"Select op.OutletName, op.PointOfSaleID, op.PointOfSaleName,
	                            ISNULL(cu.currentrating,0) as SumCurrentRating, ISNULL(cu.currentscore,0) as CurrentScore, 
	                            ISNULL(la.lastrating, 0) as SumLastRating, ISNULL(la.lastscore,0) as LastScore
                            from PointOfSale op
                            left join (
	                            select POSID, count(*) as currentrating, sum(ISNULL(rm.weight,0)) / ISNULL(count(*),1) as currentscore 
	                            from rating r inner join ratingmaster rm on r.rating = rm.rating
                                inner join usermst um on um.UserName = r.Staff
	                            where ISNULL(r.Deleted,0) = 0 
	                            and timestamp >= '{0}' and timestamp <= '{1}'
                                and (um.UserName like '%{5}%' or um.DisplayName like '%{5}%')
	                            group by r.POSID
                            ) as cu on op.PointOfSaleID = cu.POSID 
                            left join
                            (
                                select POSID, count(*) as Lastrating , sum(ISNULL(rm.weight,0)) / ISNULL(count(*),1) as lastscore 
	                            from rating r inner join ratingmaster rm on r.rating = rm.rating 
                                inner join usermst um on um.UserName = r.Staff
                                where ISNULL(r.Deleted,0) = 0 
                                and timestamp >= '{2}' and timestamp <= '{3}'
                                and (um.UserName like '%{5}%' or um.DisplayName like '%{5}%')
                                group by POSID 
                            ) as la on  op.PointOfSaleID = la.POSID 
                            where op.PointOfSaleID in (select distinct r.POSID from rating r
	                            where ISNULL(r.Deleted,0) = 0 
	                            and timestamp >= '{0}' and timestamp <= '{1}')
                                and (op.OutletName = '{4}' or '{4}' = '')
                            order by op.OutletName, op.PointOfSaleName";

                    query = @"Select ab.OutletName, ab.PointOfSaleID, ab.PointOfSaleName, ab.Rating, ab.RatingName, ab.RatingImageUrl,
	                        ISNULL(cu.currentrating,0) as CurrentRating, ISNULL(la.lastrating, 0) as LastRating
                        from 
                        (
	                        select op.OutletName, op.PointOfSaleID, op.PointOfSaleName, rm.Rating, rm.RatingName, rm.RatingImageUrl
	                        from PointOfSale op,  ratingMaster rm
	                        where isnull(op.Deleted,0) = 0 and isnull(rm.Deleted,0) = 0 
                        )ab 
                        left join (
	                        select POSID, rating, count(*) as currentrating 
	                        from  rating r
                            inner join usermst um on um.UserName = r.Staff
	                        where ISNULL(r.Deleted,0) = 0
	                        and timestamp >= '{0}' and timestamp <= '{1}'
                            and (um.UserName like '%{5}%' or um.DisplayName like '%{5}%')
	                        group by r.POSID, rating
                        ) as cu on ab.PointOfSaleID = cu.POSID and ab.Rating = cu.Rating
                        left join
                        (
                            select POSID, rating, count(*) as Lastrating from rating r
                            inner join usermst um on um.UserName = r.Staff
                            where ISNULL(r.Deleted,0) = 0
                            and timestamp >= '{2}' and timestamp <= '{3}'
                            and (um.UserName like '%{5}%' or um.DisplayName like '%{5}%')
                            group by POSID, rating 
                        ) as la on  ab.PointOfSaleID = la.POSID and ab.Rating = la.Rating
                        where ab.PointOfSaleID in (select distinct r.POSID from rating r
	                        where ISNULL(r.Deleted,0) = 0
	                        and timestamp >= '{0}' and timestamp <= '{1}')
                            and (ab.OutletName = '{4}' or '{4}' = '')
                        order by ab.OutletName asc, ab.PointOfSaleName asc, ab.Rating desc";
                }
                else
                {
                    query2 = @"Select um.DisplayName, um.UserName, 
                                    ISNULL(cu.CurrentRating,0) as SumCurrentRating, ISNULL(cu.currentscore,0) as CurrentScore, 
                                    ISNULL(la.LastRating,0) as SumLastRating, ISNULL(la.lastscore,0) as LastScore
                                from usermst um
                                left join (
	                                select staff, count(*) as CurrentRating, sum(ISNULL(rm.weight,0)) / ISNULL(count(*),1) as currentscore 
                                    from rating r inner join ratingmaster rm on r.rating = rm.rating
                                    inner join PointOfSale pos on pos.PointOfSaleID = r.POSID
	                                where ISNULL(r.Deleted,0) = 0
	                                and timestamp >= '{0}' and timestamp <= '{1}'
                                    and (pos.OutletName = '{4}' or '{4}' = '')
	                                group by staff 
                                ) as cu on um.UserName = cu.Staff 
                                left join
                                (
                                    select staff, count(*) as LastRating, sum(ISNULL(rm.weight,0)) / ISNULL(count(*),1) as lastscore 
	                                from rating r inner join ratingmaster rm on r.rating = rm.rating
                                    inner join PointOfSale pos on pos.PointOfSaleID = r.POSID
                                    where ISNULL(r.Deleted,0) = 0
                                    and timestamp >='{2}' and timestamp <= '{3}'
                                    and (pos.OutletName = '{4}' or '{4}' = '')
                                    group by staff 
                                ) as la on um.UserName = la.Staff
                                where um.UserName in (select distinct r.staff from rating r
	                                where ISNULL(r.Deleted,0) = 0
	                                and timestamp >='{0}' and timestamp <= '{1}')
                                    and (um.UserName like '%{5}%' or um.DisplayName like '%{5}%')
                                order by UserName";

                    query = @"
                        Select ab.DisplayName, ab.UserName, ab.Rating, ab.RatingName, ab.RatingImageUrl, 
	                        ISNULL(cu.currentrating,0) as CurrentRating, ISNULL(la.Lastrating, 0) as LastRating
                        from (
	                        select um.UserName, um.DisplayName, rm.Rating, rm.RatingName, rm.RatingImageUrl
	                        from usermst um, ratingMaster rm
	                        where isnull(um.Deleted,0) = 0 and isnull(rm.Deleted,0) = 0 
                        )as ab 
                        left join (
	                        select staff, rating, count(*) as currentrating from rating r, pointofsale pos
	                        where ISNULL(r.Deleted,0) = 0
	                        and timestamp >= '{0}' and timestamp <= '{1}'
                            and r.POSID = pos.PointOfSaleID
                            and (pos.OutletName = '{4}' or '{4}' = '')
	                        group by staff, rating 
                        ) as cu on ab.UserName = cu.Staff and cu.Rating = ab.rating
                        left join
                        (
                            select staff, rating, count(*) as Lastrating from rating r, pointofsale pos
                            where ISNULL(r.Deleted,0) = 0
                            and timestamp >= '{2}' and timestamp <= '{3}'
                            and r.POSID = pos.PointOfSaleID
                            and (pos.OutletName = '{4}' or '{4}' = '')
                            group by staff, rating 
                        ) as la on ab.UserName = la.Staff and la.Rating = ab.rating
                        where ab.UserName in (select distinct r.staff from rating r
	                        where ISNULL(r.Deleted,0) = 0
	                        and timestamp >= '{0}' and timestamp <= '{1}')
                            and (ab.UserName like '%{5}%' or ab.DisplayName like '%{5}%')
                        order by UserName asc, ab.Rating desc";
                }
                query = string.Format(query, startdate, enddate, lastStart.ToString("yyyy-MM-dd HH:mm:ss"), lastEnd.ToString("yyyy-MM-dd HH:mm:ss"), filteroutlet, filterstaff);
                query2 = string.Format(query2, startdate, enddate, lastStart.ToString("yyyy-MM-dd HH:mm:ss"), lastEnd.ToString("yyyy-MM-dd HH:mm:ss"), filteroutlet, filterstaff);

                DataSet ds = DataService.GetDataSet(new QueryCommand(query));
                detail = ds.Tables[0];

                DataSet ds2 = DataService.GetDataSet(new QueryCommand(query2));
                header = ds2.Tables[0];
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            var result = new
            {
                header = header,
                detail = detail,
                msg = msg
            };

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(result));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
