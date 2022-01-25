using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.IO;
using PowerPOS;
using ClosedXML.Excel;
using PowerPOS.Container;

namespace PowerWeb.API.Sales.SalesView
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Export : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var currentTime = DateTime.Now;
            string year = currentTime.Year.ToString();
            string month = "0" + currentTime.Month.ToString();
            string day = "0" + currentTime.Day.ToString();
            string hour = "0" + currentTime.Hour.ToString();
            string minute = "0" + currentTime.Minute.ToString();
            string excelFileName = "ReceiptView_" + year + month.Substring(month.Length - 2) + day.Substring(day.Length - 2) + hour.Substring(hour.Length - 2) + minute.Substring(minute.Length - 2) + ".xlsx";
            string outputFile = context.Server.MapPath("~/Exports/" + excelFileName);

            string startDate = context.Request.Params["StartDate"] ?? "";
            string endDate = context.Request.Params["EndDate"] ?? "";
            string refNo = context.Request.Params["RefNo"] ?? "";
            string cashier = context.Request.Params["Cashier"] ?? "";
            string status = context.Request.Params["Status"] ?? "";
            string remark = context.Request.Params["Remark"] ?? "";
            string name = context.Request.Params["Name"] ?? "";

            string isVoidedValue = "";

            if (status == "Voided")
            {
                isVoidedValue = "1";
            }
            else if (status == "Not Voided")
            {
                isVoidedValue = "0";
            }

            refNo = refNo.Replace("RCP", "").Replace("OR", "");

            DataTable dataRaw = ReportController.FetchTransactionReportForViewSalesWeb(startDate, endDate, refNo, cashier, "", PointOfSaleInfo.OutletName, remark, isVoidedValue, name);

            int recNo = 5;
            string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Receipt Report");

            ws.Cell("A2").Value = "Receipt Report";
            ws.Cell("A2").Style.Font.FontColor = XLColor.Black;
            ws.Cell("A2").Style.Font.Bold = true;
            ws.Cell("A2").Style.Font.FontSize = 25;

            ws.Cell("A3").Value = "Printed Date";
            ws.Cell("B3").Value = ": " + DateTime.Now.ToLongDateString() + "   " + DateTime.Now.ToShortTimeString();

            string rngTableRaw = "A" + recNo + ":" + letters[dataRaw.Columns.Count - 1] + recNo + "";
            var rngTable = ws.Range(rngTableRaw);

            rngTable.Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            rngTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngTable.Style.Font.Bold = true;
            rngTable.Style.Font.FontColor = XLColor.White;
            rngTable.Style.Fill.BackgroundColor = XLColor.DarkBlue;

            int counter1 = 1;
            int counter2 = 0;
            int isVoidedColumn = 0;
            int amountColumn = 0;
            int orderDateColumn = 0;
            foreach (DataColumn dataColumn in dataRaw.Columns)
            {
                ws.Columns(counter1.ToString()).Width = 25;

                if (dataColumn.ColumnName == "remark")
                {
                    ws.Columns(counter1.ToString()).Width = 50;
                }
                if (dataColumn.ColumnName == "isvoided")
                {
                    isVoidedColumn =  counter2;
                    ws.Cell(letters[counter2] + recNo).Style.Alignment.SetWrapText();
                }
                if (dataColumn.ColumnName == "amount")
                {
                    amountColumn = counter2;
                }
                if (dataColumn.ColumnName == "orderdate")
                {
                    orderDateColumn = counter2;
                }

                ws.Cell(letters[counter2] + recNo).Value = dataColumn.ColumnName;
                ws.Cell(letters[counter2] + recNo).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell(letters[counter2] + recNo).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell(letters[counter2] + recNo).Style.Border.TopBorderColor = XLColor.Black;
                ws.Cell(letters[counter2] + recNo).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell(letters[counter2] + recNo).Style.Border.RightBorderColor = XLColor.Black;
                ws.Cell(letters[counter2] + recNo).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cell(letters[counter2] + recNo).Style.Border.BottomBorderColor = XLColor.Black;
                ws.Cell(letters[counter2] + recNo).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell(letters[counter2] + recNo).Style.Border.LeftBorderColor = XLColor.Black;

                counter1++;
                counter2++;
            }

            recNo++;
            foreach (DataRow dataRow in dataRaw.Rows)
            {
                counter1 = 1;
                counter2 = 0;
                foreach (var data in dataRow.ItemArray)
                {
                    if(counter2 == isVoidedColumn) {
                        ws.Cell(letters[counter2] + recNo).Value = (Convert.ToBoolean(data) == true) ? "Void" : "-";
                        if (Convert.ToBoolean(data) == true)
                        {
                            string rangeVoidedCell = "A" + recNo + ":" + letters[dataRaw.Columns.Count - 1] + recNo + "";
                            var rngTableVoid = ws.Range(rangeVoidedCell);

                            rngTableVoid.Style
                                       .Font.SetBold()
                                       .Fill.SetBackgroundColor(XLColor.Red);

                            rngTableVoid.Style.Font.Bold = true;
                            rngTableVoid.Style.Font.FontColor = XLColor.White;
                            rngTableVoid.Style.Fill.BackgroundColor = XLColor.Red;
                        }
                    }
                    else if (counter2 != amountColumn && counter2 != orderDateColumn)
                    {
                        ws.Cell(letters[counter2] + recNo).Value = data == null ? "" : "'" + data.ToString();
                    }
                    else
                    {
                        ws.Cell(letters[counter2] + recNo).Value = data == null ? "" : data;
                    }

                    ws.Cell(letters[counter2] + recNo).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                    ws.Cell(letters[counter2] + recNo).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(letters[counter2] + recNo).Style.Border.TopBorderColor = XLColor.Black;
                    ws.Cell(letters[counter2] + recNo).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(letters[counter2] + recNo).Style.Border.RightBorderColor = XLColor.Black;
                    ws.Cell(letters[counter2] + recNo).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(letters[counter2] + recNo).Style.Border.BottomBorderColor = XLColor.Black;
                    ws.Cell(letters[counter2] + recNo).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Cell(letters[counter2] + recNo).Style.Border.LeftBorderColor = XLColor.Black;

                    if (counter2 == amountColumn)
                    {
                        ws.Cell(letters[counter2] + recNo).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        ws.Cell(letters[counter2] + recNo).Style.NumberFormat.Format = "#,##0.#0";
                    }

                    if (counter2 == orderDateColumn)
                    {
                        ws.Cell(letters[counter2] + recNo).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(letters[counter2] + recNo).Style.DateFormat.Format = "dd MMM yyyy";
                    }
                    counter1++;
                    counter2++;
                }

                recNo++;
            }

            wb.SaveAs(outputFile);
            context.Response.ContentType = "application/vnd.ms-excel";
            context.Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(outputFile));
            context.Response.WriteFile(outputFile);
            context.Response.End();
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
