using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Newtonsoft.Json;
using PowerPOSLib.Container;
using System.Web.SessionState;

namespace PowerWeb.API.Product.ItemImporter
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UpdateCache : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            JsonResult result = new JsonResult();
            var columnPosition = Convert.ToInt32(context.Request.Params["columnPosition"] ?? "0");
            var rowId = Convert.ToInt32(context.Request.Params["rowId"] ?? "0");
            var value = context.Request.Params["value"] ?? "";
            var dataId = context.Request.Params["DataID"] ?? "";
            bool isFound = false;
            try
            {
                DataTable uploadedData = (DataTable)context.Session[dataId];
                int iterator = 0;
                foreach (DataRow rowItem in uploadedData.Rows)
                {
                    if (rowId == iterator)
                    {
                        switch (columnPosition)
                        {
                            case 7:
                                rowItem["GSTRule"] = value;
                                break;
                            case 6:
                                rowItem["Barcode"] = value;
                                break;
                            case 5:
                                rowItem["ItemName"] = value;
                                break;
                            case 4:
                                rowItem["ItemNo"] = value;
                                break;
                            case 3:
                                rowItem["Category"] = value;
                                break;
                            case 2:
                                rowItem["Department"] = value;
                                break;
                        }
                        result.Message = value;
                        isFound = true;
                    }

                    iterator++;
                }

                result.Status = isFound;

                var data = uploadedData;
            }
            catch (Exception)
            {
                result.Status = false;
                result.Message = "Cannot update data.";
            }

            context.Response.ContentType = "text/html";
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
