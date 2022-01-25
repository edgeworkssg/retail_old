using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PowerPOSLib.Container;
using SubSonic;
using System.Data;
using Newtonsoft.Json;

namespace PowerWeb.API.Promo
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetAvgPriceItemGroup : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var ItemGroupId = context.Request.Params["ItemGroupId"];
            var PromoCampaignHdrID = Int32.Parse(context.Request.Params["PromoCampaignHdrID"]);

            JsonResult result = new JsonResult()
            {
                Status = false,
                Message = "",
                Data = null
            };

            try
            {
                decimal retailPrice = 0;
                string query = "SELECT ISNULL(SUM(m.UnitQty * i.RetailPrice),0) as RetailPrice " +
                                "from ItemGroupMap m " +
                                "inner join Item i on m.ItemNo = i.ItemNo " +
                                "where m.ItemGroupID = " + ItemGroupId;
                QueryCommand cmd = new QueryCommand(query);
                DataTable dq = new DataTable();
                dq.Load(DataService.GetReader(cmd));

                if (dq != null && dq.Rows.Count > 0)
                {
                    retailPrice = Decimal.Parse(dq.Rows[0][0].ToString());
                }

                /*Check existing promo */
                if (PromoCampaignHdrID == 0)
                {
                    /*if new*/
                    string query2 = "select ISNULL(MAX(PromoCampaignHdrID),0) as promocampaignhdrid " +
                                    "from PromoCampaignHdr";
                    QueryCommand cmd2 = new QueryCommand(query2);
                    DataTable dt = new DataTable();
                    dt.Load(DataService.GetReader(cmd2));


                    if (dt.Rows[0][0] != null && dt.Rows[0][0].ToString() != "{}")
                        PromoCampaignHdrID = Int32.Parse(dt.Rows[0][0].ToString()) + 1;
                }

                string query3 = "select PromoCampaignName " +
                        "from PromoCampaignHdr h inner join PromoCampaignDet d on h.PromoCampaignHdrID = d.PromoCampaignHdrID " +
                        "where ISNULL(h.Deleted,0) = 0 and ISNULL(d.Deleted,0) = 0 and CONVERT(date, h.DateTo) > GETDATE() and d.ItemGroupID = '" + ItemGroupId + "' " +
                        "and h.PromoCampaignHdrID <> " + PromoCampaignHdrID;

                DataTable d = new DataTable();
                d.Load(DataService.GetReader(new QueryCommand(query3)));

                if (d.Rows.Count > 0)
                {
                    string existingpromo = "";
                    for (int i = 0; i < d.Rows.Count; i++)
                    {
                        existingpromo += d.Rows[i][0].ToString() + ",";
                    }
                    existingpromo = existingpromo.Substring(0, existingpromo.Length - 1);
                    result.Message = "This Item Group are being used on other promos(" + existingpromo + "). Please check your item again and don't forget to set the priority right";
                }
                else
                {
                    result.Message = "";
                }

                result.Status = true;
                result.Data = retailPrice;

            }
            catch (Exception)
            {
                result.Status = false;
                result.Message = "Cannot find item or item is deleted.";
            }

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
