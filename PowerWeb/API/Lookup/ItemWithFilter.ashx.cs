using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using SubSonic;
using Newtonsoft.Json;
using PowerPOS;

namespace PowerWeb.API.Lookup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ItemWithFilter : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string filter = string.IsNullOrEmpty(context.Request.Params["filter"]) ? "%" : context.Request.Params["filter"];
            int skip = context.Request.Params["skip"].GetIntValue();
            int take = context.Request.Params["take"].GetIntValue();

            ItemCollection col = new ItemCollection();
            string query = "Select * from item where (ISNULL(deleted,0) = 0 OR EXISTS(SELECT * FROM OutletGroupItemMap WHERE ItemNo = Item.ItemNo AND ISNULL(Deleted, 0) = 0) ) and ISNULL(ItemNo,'') + ISNULL(ItemName,'') like '%" + filter + "%' Order by ItemName ASC";

            DataSet ds = DataService.GetDataSet(new QueryCommand(query));

            col.Load(ds.Tables[0]);

            // Get total records before being paged
            var totalRecords = col.Count();

            List<ItemResult> objReturn = new List<ItemResult>();
            if (take > 0)
            {
                for (int index = skip; index < skip + take && index < col.Count; index++)
                {
                    objReturn.Add(new ItemResult() { ItemNo = col[index].ItemNo, ItemName = col[index].ItemName });
                }
            }

            var result = new
            {
                records = objReturn,
                totalRecords = totalRecords
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

    public class ItemResult {
        public string ItemNo { get;set; }
        public string ItemName { get; set; }
    }
}
