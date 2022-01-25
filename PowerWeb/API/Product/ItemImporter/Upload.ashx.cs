using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.SessionState;
using System.IO;
using Excel;
using Newtonsoft.Json;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;

namespace PowerWeb.API.Product.ItemImporter
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Upload : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            DataSet result = null;
            DataTable dt = null;

            if (context.Request.Files.Count > 0)
            {
                HttpFileCollection files = context.Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    string newFileName = Guid.NewGuid().ToString();
                    string filePath = context.Server.MapPath("~/Uploads/" + newFileName);
                    file.SaveAs(filePath);

                    //FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

                    //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

                    //result = excelReader.AsDataSet();

                    //excelReader.IsFirstRowAsColumnNames = true;

                    dt = new DataTable();
                    dt.Clear();

                    dt.Columns.Add("Status");
                    dt.Columns.Add("Active");
                    dt.Columns.Add("Department");
                    dt.Columns.Add("Category");
                    dt.Columns.Add("ItemNo");
                    dt.Columns.Add("ItemName");
                    dt.Columns.Add("Barcode");
                    dt.Columns.Add("RetailPrice");
                    dt.Columns.Add("CostPrice");
                    dt.Columns.Add("ServiceItem");
                    dt.Columns.Add("InventoryItem");
                    dt.Columns.Add("NonDiscountable");
                    dt.Columns.Add("GiveCommission");
                    dt.Columns.Add("OpeningBalance");
                    dt.Columns.Add("Attributes1");
                    dt.Columns.Add("Attributes2");
                    dt.Columns.Add("Attributes3");
                    dt.Columns.Add("Attributes4");
                    dt.Columns.Add("Attributes5");
                    dt.Columns.Add("GSTRule");

                    var ds = new DataSet();

                    try
                    {
                        using (var conn = new OleDbConnection(GetExcelConnectionString32(filePath)))
                        {
                            conn.Open();

                            foreach (var sheet in GetSheets(conn))
                            {
                                FillTableWithData(sheet, conn, ds);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        using (var conn = new OleDbConnection(GetExcelConnectionString64(filePath)))
                        {
                            conn.Open();

                            foreach (var sheet in GetSheets(conn))
                            {
                                FillTableWithData(sheet, conn, ds);
                            }
                        }
                    }

                    int iterator = 0;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        object[] cols = row.ItemArray;

                        if (iterator >= 0 && 
                            string.IsNullOrEmpty(cols[1].ToString()) == false)
                        {
                            if (iterator >= 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr["Status"] = null;
                                dr["Active"] = cols[0] != null ? cols[0].ToString() : "";
                                dr["Department"] = cols[1] != null ? cols[1].ToString() : "";
                                dr["Category"] = cols[2] != null ? cols[2].ToString() : "";
                                dr["ItemNo"] = cols[3] != null ? cols[3].ToString() : "";
                                dr["ItemName"] = cols[4] != null ? cols[4].ToString() : "";
                                dr["Barcode"] = cols[5] != null ? cols[5].ToString() : "";
                                dr["RetailPrice"] = cols[6] != null ? cols[6].ToString() : "";
                                dr["CostPrice"] = cols[7] != null ? cols[7].ToString() : "";
                                dr["ServiceItem"] = cols[8] != null ? cols[8].ToString() : "";
                                dr["InventoryItem"] = cols[9] != null ? cols[9].ToString() : "";
                                dr["NonDiscountable"] = cols[10] != null ? cols[10].ToString() : "";
                                dr["GiveCommission"] = cols[11] != null ? cols[11].ToString() : "";
                                dr["OpeningBalance"] = cols[12] != null ? cols[12].ToString() : "";
                                dr["Attributes1"] = cols[13] != null ? cols[13].ToString() : "";
                                dr["Attributes2"] = cols[14] != null ? cols[14].ToString() : "";
                                dr["Attributes3"] = cols[15] != null ? cols[15].ToString() : "";
                                dr["Attributes4"] = cols[16] != null ? cols[16].ToString() : "";
                                dr["Attributes5"] = cols[17] != null ? cols[17].ToString() : "";
                                dr["GSTRule"] = "Non GST";

                                dt.Rows.Add(dr);
                            }
                        }
                        else if (iterator > 0)
                        {
                            break;
                        }
                        iterator++;
                    }

                    //while (excelReader.Read())
                    //{
                    //    if (iterator > 0 && string.IsNullOrEmpty(excelReader.GetString(1)) == false
                    //                     && string.IsNullOrEmpty(excelReader.GetString(2)) == false
                    //                     && string.IsNullOrEmpty(excelReader.GetString(3)) == false
                    //                     && string.IsNullOrEmpty(excelReader.GetString(4)) == false)
                    //    if (iterator > 0)
                    //    {
                    //        DataRow dr = dt.NewRow();
                    //        dr["Status"] = null;
                    //        dr["Active"] = excelReader.GetString(0);
                    //        dr["Department"] = excelReader.GetString(1);
                    //        dr["Category"] = excelReader.GetString(2);
                    //        dr["ItemNo"] = excelReader.GetString(3);
                    //        dr["ItemName"] = excelReader.GetString(4);
                    //        dr["Barcode"] = excelReader.GetString(5);
                    //        dr["RetailPrice"] = excelReader.GetString(6);
                    //        dr["CostPrice"] = excelReader.GetString(7);
                    //        dr["ServiceItem"] = excelReader.GetString(8);
                    //        dr["InventoryItem"] = excelReader.GetString(9);
                    //        dr["NonDiscountable"] = excelReader.GetString(10);
                    //        dr["GiveCommission"] = excelReader.GetString(11);
                    //        dr["OpeningBalance"] = excelReader.GetString(12);
                    //        dr["Attributes1"] = excelReader.GetString(13);
                    //        dr["Attributes2"] = excelReader.GetString(14);
                    //        dr["Attributes3"] = excelReader.GetString(15);
                    //        dr["Attributes4"] = excelReader.GetString(16);
                    //        dr["Attributes5"] = excelReader.GetString(17);

                    //        dt.Rows.Add(dr);
                    //    }
                    //    else if (iterator > 0)
                    //    {
                    //        break;
                    //    }
                    //    iterator++;
                    //}
                }
            }

            string dataId = Guid.NewGuid().ToString();
            context.Session[dataId] = dt;

            if (dt.Rows.Count == 0)
            {
                dataId = "-";
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(dataId);
        }

        private static void FillTableWithData(string tableName, OleDbConnection conn, DataSet ds)
        {
            var query = string.Format("SELECT * FROM [{0}]", tableName);
            using (var da = new OleDbDataAdapter(query, conn))
            {
                var dt = new DataTable(tableName);
                da.Fill(dt);
                ds.Tables.Add(dt);
            }
        }
        private static IEnumerable<string> GetSheets(OleDbConnection conn)
        {
            var sheets = new List<string>();
            var dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dtSheet != null)
            {
                foreach (DataRow drSheet in dtSheet.Rows)
                {
                    string sheet = drSheet["TABLE_NAME"].ToString();
                    if (sheet.EndsWith("$") || sheet.StartsWith("'") && sheet.EndsWith("$'"))
                    {
                        sheets.Add(sheet);
                    }
                }
            }

            return sheets;
        }
        private static string GetExcelConnectionString64(string fullPathToExcelFile)
        {
            var props = new Dictionary<string, string>();
            props["Provider"] = "Microsoft.ACE.OLEDB.12.0";
            props["Data Source"] = fullPathToExcelFile;
            props["Extended Properties"] = "Excel 8.0";

            var sb = new StringBuilder();
            foreach (var prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
        }
        private static string GetExcelConnectionString32(string fullPathToExcelFile)
        {
            var props = new Dictionary<string, string>();
            props["Provider"] = "Microsoft.JET.OLEDB.4.0";
            props["Data Source"] = fullPathToExcelFile;
            props["Extended Properties"] = "Excel 8.0";

            var sb = new StringBuilder();
            foreach (var prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
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
