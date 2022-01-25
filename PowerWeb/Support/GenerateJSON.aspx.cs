using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using PowerPOS;
using PowerWeb;
using SpreadsheetLight;
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace PowerWeb.Support
{
    public partial class GenereateJSON : PageBase
    {
        private int requiredcolumns = 15;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void ExportData(string fileName, DataTable dt)
        {
            fileName = fileName.Replace(" ", "_");
            SLDocument sl = new SLDocument();
            var style = sl.CreateStyle();
            style.FormatCode = "###";
            int iStartRowIndex = 1;
            int iStartColumnIndex = 1;
            sl.SetColumnStyle(5, style);
            sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, dt, true);
            int iEndRowIndex = iStartRowIndex + dt.Rows.Count + 1 - 1;
            int iEndColumnIndex = iStartColumnIndex + dt.Columns.Count - 1;
            SLTable table = sl.CreateTable(iStartRowIndex, iStartColumnIndex, iEndRowIndex, iEndColumnIndex);
            table.SetTableStyle(SLTableStyleTypeValues.Medium2);
            table.HasTotalRow = false;
            sl.InsertTable(table);

            sl.AutoFitColumn(2, iEndColumnIndex);

            sl.FreezePanes(1, 0);

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            //Response.BufferOutput = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "_Result.xlsx");
            sl.SaveAs(Response.OutputStream);
            Response.End();
        }

        protected void btnExportBlank_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataTable dt = GetInitialDataTable();
            ExportData("GenerateJSON", dt);
        }

        protected DataTable GetInitialDataTable()
        {
            DataTable dtInput = new DataTable();
            dtInput.Columns.Add("STOCK", typeof(string)); // 0
            dtInput.Columns.Add("BARCODE", typeof(string));//1
            dtInput.Columns.Add("REF.", typeof(string));//2
            dtInput.Columns.Add("DESCRIPTION", typeof(string));//3
            dtInput.Columns.Add("GLASS", typeof(string));//4
            dtInput.Columns.Add("BASE", typeof(string));//5
            dtInput.Columns.Add("LAMPS", typeof(string));//6           
            dtInput.Columns.Add("BALLAST", typeof(string));//7
            dtInput.Columns.Add("LIST PRICE", typeof(string));//8
            dtInput.Columns.Add("NETTPRICE", typeof(string));//9
            dtInput.Columns.Add("UOM", typeof(string));//10
            dtInput.Columns.Add("STD. COST", typeof(string));//11
            dtInput.Columns.Add("REGION", typeof(string));//12
            dtInput.Columns.Add("CODE", typeof(string));//13
            dtInput.Columns.Add("LAST IN", typeof(string));//14
           
            return dtInput;
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            if (fuItemImporter.HasFile)
            {
                DataTable dtInput = GetInitialDataTable();
                
                #region *) Read Excel

                try
                {
                    using (SLDocument sl = new SLDocument(fuItemImporter.PostedFile.InputStream, "Sheet1"))
                    {
                        SLWorksheetStatistics stats = sl.GetWorksheetStatistics();
                        int iStartColumnIndex = 1;
                        int iStartRowIndex =  stats.StartRowIndex + 1;
                        int iEndRowIndex =  stats.EndRowIndex;
                        int iEndRowDataIndex = iEndRowIndex;
                        //for (int i = iEndRowIndex; i >= iStartRowIndex; i--)
                        //{
                        //    if (sl.HasCellValue(i, 2))
                        //    {
                        //        iEndRowDataIndex = i;
                        //        break;
                        //    }
                        //}

                        //check the header
                        string[] columns = new string[] { "STOCK", "BARCODE", "REF.", "DESCRIPTION", "GLASS", "BASE", "LAMPS", "BALLAST", "LIST PRICE", "NETTPRICE", "UOM", "STD. COST", "REGION", "CODE", "LAST IN" };
                        if (stats.EndColumnIndex < requiredcolumns)
                        {
                            string missingcolumns = "Missing Column : ";
                            for(int i = 0; i < columns.Count(); i++)
                            {
                                //if (!sl.GetCellValueAsString(1, i + 1).Equals(columns[i]))
                                //{
                                //    missingcolumns += columns[i] + ", ";
                                //}

                                if (!findHeader(sl,columns[i], stats.EndColumnIndex))
                                {
                                    missingcolumns += columns[i] + ", ";
                                }
                            }

                            throw (new Exception(missingcolumns.Substring(0, missingcolumns.Length - 2)));
                        }

                        for (int row = iStartRowIndex; row <= iEndRowDataIndex; ++row)
                        {
                            dtInput.Rows.Add(
                                sl.GetCellValueAsString(row, iStartColumnIndex),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 1),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 2),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 3),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 4),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 5),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 6),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 7),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 8),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 9),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 10),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 11),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 12),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 13),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 14));
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error " + ex.Message;
                    Logger.writeLog(ex);
                    return;
                }
                #endregion

                #region *) Import Data

                if (dtInput.Rows.Count > 0)
                {
                    string status = "";
                    DataTable resultDt = new DataTable();
                    string result = "";
                    bool isSuccess = ExportToJSON(dtInput,out result, out status);
                    if (isSuccess)
                    {
                        lblStatus.Text = "Data Generated";

                        //Download the Text file.
                        Response.Clear();
                        Response.Buffer = true;
                        Response.BufferOutput = true;
                        Response.AddHeader("content-disposition", "attachment;filename=data.txt");
                        Response.Charset = "";
                        Response.ContentType = "application/text";
                        Response.Output.Write(result);
                        Response.Flush();
                        Response.End();

                        //ExportData(Path.GetFileNameWithoutExtension(fuItemImporter.FileName), resultDt);
                    }
                    else
                    {
                        lblStatus.Text = "ERROR : " + status;
                        //if (resultDt.Rows.Count > 0)
                        //    ExportData(Path.GetFileNameWithoutExtension(fuItemImporter.FileName), resultDt);
                    }
                }
                else
                {
                    lblStatus.Text = "No Data Imported";
                }

                #endregion
            }
        }

        protected bool findHeader(SLDocument sl, string HeaderName, int ColumnNumber)
        {
            bool objReturn = false;
            for (int i = 0; i < ColumnNumber; i++)
            {
                if (sl.GetCellValueAsString(1, i + 1).Equals(HeaderName))
                {
                    objReturn = true;
                    break;
                }
            }

            return objReturn;
        }

        protected bool ExportToJSON(DataTable input,out string result, out string status)
        {
            status = "";
            result = "";
            try
            {
                //string result = new JavaScriptSerializer().Serialize(input);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;

                foreach (DataRow dr in input.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in input.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                //result = serializer.Serialize(rows);
                result = Newtonsoft.Json.JsonConvert.SerializeObject(rows);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error export to JSON: " + ex.Message);
                status = ex.Message;
                return false;
            }

        }
    }
}
