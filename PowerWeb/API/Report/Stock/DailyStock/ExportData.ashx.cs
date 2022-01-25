using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;
using SubSonic;
using System.IO;

namespace PowerWeb.API.Report.Stock.DailyStock
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ExportData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string inventoryLocation = context.Request.Params["InventoryLocation"];
            
            if (inventoryLocation == "")
            {
                inventoryLocation = "0";
            }

            string strSql = "DECLARE @search AS NVARCHAR(200);   ";
            strSql += "DECLARE @Location AS INT;   ";
            strSql += "SET @Location = " + inventoryLocation + "; ";
            strSql += "SET @Search = ''; ";
            strSql += "SELECT Category = (IP.DepartmentName + ' ' + IC.CategoryName)  ";
            strSql += ", IT.ItemNo  ";
            strSql += ", IT.ItemName  ";
            strSql += ", IT.ItemDesc  ";
            strSql += ", ISNULL((ISNULL(StockIn,0)- ISNULL(StockOut,0) + ISNULL(TransferIn,0) -ISNULL(TransferOut,0)+ ISNULL(AdjustmentIn,0)-AdjustmentOut),0) Quantity   ";
            strSql += "FROM ";
            strSql += "(   ";
            strSql += "SELECT ItemNo, SUM(CASE WHEN movementtype LIKE 'Stock In' THEN quantity ELSE 0 END) as StockIn   ";
            strSql += ", SUM(CASE WHEN movementtype LIKE 'Stock Out' THEN quantity ELSE 0 END) as StockOut   ";
            strSql += ", SUM(CASE WHEN movementtype LIKE 'Transfer In' THEN quantity ELSE 0 END) as TransferIn  ";
            strSql += ", SUM(CASE WHEN movementtype LIKE 'Transfer Out' THEN quantity ELSE 0 END) as TransferOut   ";
            strSql += ", SUM(CASE WHEN movementtype LIKE 'Adjustment In' THEN quantity ELSE 0 END) as AdjustmentIn  ";
            strSql += ", SUM(CASE WHEN movementtype LIKE 'Adjustment Out' THEN quantity ELSE 0 END) as AdjustmentOut  ";
            strSql += "FROM InventoryHdr IH   ";
            strSql += "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo   ";
            strSql += "INNER JOIN InventoryLocation LI ON IH.InventoryLocationID = LI.InventoryLocationID   ";
            strSql += "WHERE CAST(IH.InventoryLocationID AS VARCHAR(4)) LIKE CASE WHEN @Location <> 0 THEN CAST(@Location AS VARCHAR(4)) ELSE '%' END  ";
            strSql += "GROUP BY ItemNo  ";
            strSql += ") JQ  ";
            strSql += "RIGHT OUTER JOIN Item IT ON JQ.ItemNo = IT.ItemNo  ";
            strSql += "INNER JOIN Category IC ON IC.CategoryName = IT.CategoryName   ";
            strSql += "INNER JOIN itemdepartment IP on IP.itemdepartmentid = IC.ItemDepartmentId  ";
            strSql += "WHERE (IT.Deleted=0 or IT.Deleted is null)   ";
            strSql += "AND IP.ItemDepartmentID <> 'SYSTEM'   ";
            strSql += "AND IsInInventory = 1   ";
            strSql += "AND IT.ItemNo + ItemName + IC.CategoryName + ISNULL(ItemDesc,'')  ";
            strSql += " + ISNULL(attributes1,'') + ISNULL(attributes2,'') + ISNULL(attributes3,'') + ISNULL(attributes4,'')   ";
            strSql += "+ ISNULL(attributes5,'') + ISNULL(attributes6,'') + ISNULL(attributes7,'') + ISNULL(attributes8,'') ";
            strSql += "LIKE N'%' + @search + '%'   ";
            strSql += "ORDER BY ItemNo ASC  ";
            strSql += "select getdate() as CurrentDate; ";
            strSql += "if @Location = 0 or @Location is null  ";
            strSql += "begin ";
            strSql += "select Location = 'ALL'; ";
            strSql += "end ";
            strSql += "else ";
            strSql += "begin ";
            strSql += "select a.InventoryLocationName as Location ";
            strSql += "from InventoryLocation a ";
            strSql += "where a.InventoryLocationID = @Location; ";
            strSql += "end; ";


            ReportDocument crDoc = new ReportDocument();
            string reportPath = context.Request.PhysicalApplicationPath + "Reports\\ReportFiles\\StockBalance\\StockOnHandDailyReport.rpt";
            crDoc.Load(reportPath);
            DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));

            ds.Tables[0].TableName = "StockOnHandSummary_List";
            ds.Tables[1].TableName = "StockOnHandSummary_CurrentDate";
            ds.Tables[2].TableName = "StockOnHandSummary_Location";

            crDoc.SetDataSource(ds);
            MemoryStream rawData = (MemoryStream)crDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            context.Response.ClearContent();
            context.Response.ClearHeaders();
            context.Response.ContentType = "application/pdf";
            context.Response.AddHeader("Content-Disposition", "inline; filename=" + "Report.pdf");
            context.Response.AddHeader("Content-Length", rawData.Length.ToString());
            context.Response.BinaryWrite(rawData.ToArray());
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
