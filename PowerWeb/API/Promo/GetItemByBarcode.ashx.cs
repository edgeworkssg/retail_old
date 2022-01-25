using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PowerPOSLib.Container;
using Newtonsoft.Json;
using SubSonic;
using PowerPOS;
using System.Data;

namespace PowerWeb.API.Promo
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetItemByBarcode : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var barcode = context.Request.Params["Barcode"];
            var PromoCampaignHdrID = Int32.Parse(context.Request.Params["PromoCampaignHdrID"]);

            JsonResult result = new JsonResult()
            {
                Status = false,
                Message = "",
                Data = null,
            };

            try
            {
                ItemController itemLogic = new ItemController();
                Query qr = Item.Query();
                qr.WHERE(Item.Columns.Barcode, barcode);
                qr.WHERE(Item.Columns.Deleted, Comparison.Equals, false);

                string query = @"Select * from item where barcode = '{0}' and (ISNULL(deleted,0) = 0 OR EXISTS(SELECT * FROM OutletGroupItemMap WHERE ItemNo = Item.ItemNo AND ISNULL(Deleted, 0) = 0) ) 
                                UNION 
                                Select it.* from alternatebarcode ab
                                inner join item it on  it.ItemNo = ab.ItemNo
                                where ISNULL(ab.Deleted,0) = 0 and ab.barcode = '{0}' 
                                and (ISNULL(it.deleted,0) = 0 OR EXISTS(SELECT * FROM OutletGroupItemMap WHERE ItemNo = it.ItemNo AND ISNULL(Deleted, 0) = 0) ) ";
                query = string.Format(query, barcode);
                DataSet ds = DataService.GetDataSet(new QueryCommand(query));
                
                //ItemCollection col = itemLogic.FetchByQuery(qr);

                ItemCollection col = new ItemCollection();
                col.Load(ds.Tables[0]);

                if (col != null && col.Count > 0)
                {
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
                            "where ISNULL(h.Deleted,0) = 0 and ISNULL(d.Deleted,0) = 0 and CONVERT(date, h.DateTo) > GETDATE() and d.ItemNo = '" + col[0].ItemNo + "' " +
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
                        result.Message = "This item are being used on other promos(" + existingpromo + "). Please check your item again and don't forget to set the priority right";
                    }
                    else
                    {
                        result.Message = "";
                    }

                    result.Status = true;
                    result.Data = col[0];

                }
                else 
                {
                    result.Status = false;
                }
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
