using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PowerPOS;
using SubSonic;
using System.Linq;
using PowerPOS.Container;

namespace PowerWeb.Support
{
    public partial class PriceImporter : System.Web.UI.Page
    {
        private const string ExportedFileName = "PriceImport";
        private const string ImportedFileNameSuffix = "PriceImport";
        private const string ImportedFileRptFileName = "PriceImportView.rpt";
        private const string vs_ImportedFileName = "PriceImportFile";
        private const string vs_ErrorFileName = "PriceImportErrorFile";
        private const string vs_ImportedRptFileName = "PriceImporttRptFile";
        private const string vs_ErrorRptFileName = "PriceImportErrorRptFile";
        private const string vs_BoundFileName = "PriceImportBindedFile";
        private const string vs_BoundRptFileName = "PriceImportBindedRptFile";
        private const string providerName = "PowerPOS";

        List<string> ColumnNames = new List<string>();
        List<string> ColumnTypes = new List<string>();

        AttributesLabelCollection LblList;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ColumnNames = new List<string>();
            ColumnTypes = new List<string>();

            LblList = new AttributesLabelCollection();
            LblList.Load();
            for (int Counter = 0; Counter < LblList.Count; Counter++)
            {
                ColumnNames.Add(LblList[Counter].Label);
                ColumnTypes.Add("System.String");
            }
        }

        #region #EXPORT
        protected void lnkExportPDF_Click(object sender, EventArgs e)
        {
            ReportDocument TheReport = BindExcelFile();
            TheReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, ExportedFileName + "_" + DateTime.Now.ToString("yyyyMMddTHHmmss"));
        }
        protected void lnkExportExcel_Click(object sender, EventArgs e)
        {
            ReportDocument TheReport = BindExcelFile();
            TheReport.ExportToHttpResponse(ExportFormatType.Excel, Response, true, ExportedFileName + "_" + DateTime.Now.ToString("yyyyMMddTHHmmss"));
        }
        protected void lnkExportRaw_Click(object sender, EventArgs e)
        {
            ReportDocument TheReport = BindExcelFile();
            TheReport.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true, ExportedFileName + "_" + DateTime.Now.ToString("yyyyMMddTHHmmss"));
        }
        #endregion

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (!FileUploader.HasFile)
                return;

            try
            {
                lblMsg.Text = "";

                #region *) Upload Excel File to Server
                string DirectoryStr = Server.MapPath("~/UploadedFile/");
                string FileName = ImportedFileNameSuffix + "-" + Guid.NewGuid().ToString() + ".xls";
                
                if (!Directory.Exists(DirectoryStr))
                    Directory.CreateDirectory(DirectoryStr);

                FileUploader.SaveAs(DirectoryStr + FileName);
                #endregion

                ViewState[vs_ImportedFileName] = FileName;
                ViewState[vs_ImportedRptFileName] = ImportedFileRptFileName;
                ViewState[vs_BoundFileName] = FileName;
                ViewState[vs_BoundRptFileName] = ImportedFileRptFileName;

                CrystalReportViewer1.ReportSource = BindExcelFile();
                CrystalReportViewer1.RefreshReport();

                lblMsg.Text = "Upload Successful";
            }
            catch (Exception X)
            {
                lblMsg.Text = X.Message;
                Logger.writeLog(X);
            }
        }

        private ReportDocument BindExcelFile()
        {
            string ReportName = Server.MapPath("~\\bin\\Reports\\" + ViewState[vs_BoundRptFileName]);
            string FileName = Server.MapPath("~\\UploadedFile\\" + ViewState[vs_BoundFileName].ToString());

            if (!File.Exists(ReportName))
                throw new Exception("Report is not found");

            if (ViewState[vs_ImportedFileName] == null || ViewState[vs_ImportedFileName].ToString() == "")
                return null;

            if (!ViewState[vs_ImportedFileName].ToString().EndsWith(".xls"))
                throw new Exception("Uploaded file is in the wrong format");

            ReportDocument TheReport = new ReportDocument();
            #region *) Get Report Template
            TheReport.Load(ReportName);
            #endregion

            string Status = string.Empty;

            // Columns
            ColumnNames = ExcelDataLogic.OpenExcelFileGetColumnHeaderList(FileName, out Status);
            ColumnTypes = new List<string>();
            foreach (string eachColumn in ColumnNames)
            {
                if (eachColumn == "Item No")
                {
                    ColumnTypes.Add("System.String");
                }
                else
                {
                    ColumnTypes.Add("System.Decimal");
                }
            }

            DataTable ExcelData;
            #region *) Get Excel Data
            ExcelData = ExcelDataLogic.OpenExcelFile(FileName, ColumnNames.ToArray(), ColumnTypes.ToArray(), out Status);

            // Remove Empty Rows
            int dataCount = ExcelData.Rows.Count - 1;
            for (int i = dataCount; i >= 0; i--)
            {
                if (ExcelData.Rows[i][0].ToString().Trim() == string.Empty)
                {
                    ExcelData.Rows.RemoveAt(i);
                }
            }
            ExcelData.AcceptChanges();

            #endregion

            // Unpivot ExcelData
            DataTable unPivotedExcelData = new DataTable();
            unPivotedExcelData.Columns.Add("ItemNo");
            unPivotedExcelData.Columns.Add("RetailPrice", typeof(decimal));
            unPivotedExcelData.Columns.Add("SchemeID");
            unPivotedExcelData.Columns.Add("SchemePrice", typeof(decimal));
            foreach (DataRow eachExcelRow in ExcelData.Rows)
            {
                for (int i = 2; i < ExcelData.Columns.Count; i++)
                {
                    DataRow newUnpivotRow = unPivotedExcelData.NewRow();
                    newUnpivotRow["ItemNo"] = eachExcelRow["Item No"];
                    newUnpivotRow["RetailPrice"] = eachExcelRow["Retail Price"];
                    newUnpivotRow["SchemeID"] = ExcelData.Columns[i].Caption;
                    newUnpivotRow["SchemePrice"] = eachExcelRow[i];
                    unPivotedExcelData.Rows.Add(newUnpivotRow);
                    unPivotedExcelData.AcceptChanges();
                }
            }

            if (Status != "") Logger.writeLog(Status);

            //for (int Counter = 0; Counter < LblList.Count; Counter++)
            //    ExcelData.Columns[LblList[Counter].Label].ColumnName = "Attributes" + LblList[Counter].AttributesNo.ToString();

            TheReport.SetDataSource(unPivotedExcelData);
            TheReport.Refresh();

            return TheReport;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string ReportName = Server.MapPath("~\\bin\\Reports\\" + ViewState[vs_ImportedRptFileName]);
            string FileName = Server.MapPath("~\\UploadedFile\\" + ViewState[vs_ImportedFileName].ToString());
            string UserName = Session["UserName"].ToString();

            lblMsg.Text = "";

            if (!File.Exists(ReportName))
                throw new Exception("Report is not found");

            if (ViewState[vs_ImportedFileName] == null || ViewState[vs_ImportedFileName].ToString() == "")
                return;

            if (!ViewState[vs_ImportedFileName].ToString().EndsWith(".xls"))
                throw new Exception("Uploaded file is in the wrong format");

            try
            {
                string Status = "";

                ReportDocument TheReport = new ReportDocument();
                #region *) Get Report Template
                TheReport.Load(ReportName);
                #endregion

                // Columns
                ColumnNames = ExcelDataLogic.OpenExcelFileGetColumnHeaderList(FileName, out Status);
                ColumnTypes = new List<string>();
                foreach (string eachColumn in ColumnNames)
                {
                    if (eachColumn == "Item No")
                    {
                        ColumnTypes.Add("System.String");
                    }
                    else
                    {
                        ColumnTypes.Add("System.Decimal");
                    }
                }

                DataTable ExcelData;
                #region *) Get Excel Data
                ExcelData = ExcelDataLogic.OpenExcelFile(FileName, ColumnNames.ToArray(), ColumnTypes.ToArray(), out Status);

                // Remove Empty Rows
                int dataCount = ExcelData.Rows.Count - 1;
                for (int i = dataCount; i >= 0; i--)
                {
                    if (ExcelData.Rows[i][0].ToString().Trim() == string.Empty)
                    {
                        ExcelData.Rows.RemoveAt(i);
                    }
                }
                ExcelData.AcceptChanges();

                #endregion

                DataTable Err = new DataTable();
                Err = ExcelData.Clone();
                if (!Err.Columns.Contains("Err"))
                    Err.Columns.Add("Err", Type.GetType("System.String"));
                Err.Rows.Clear();

                #region *) Try to Save Data, Put all the error into Err DataTable
                QueryCommandCollection Cmds = new QueryCommandCollection();

                // If Table PriceScheme doesn't exist, then Create Table
                string sqlCheckPriceSchemeTable = 
                    "IF (NOT EXISTS (SELECT * " +
                         "FROM INFORMATION_SCHEMA.TABLES " +
                         "WHERE TABLE_NAME = 'PriceScheme')) " +
                    "BEGIN " +
                        "CREATE TABLE [dbo].[PriceScheme]( " +
	                        "[SchemeID] [varchar](50) NOT NULL, " +
	                        "[ItemNo] [varchar](50) NULL, " +
	                        "[Price] [money] NULL " +
                        ") " +
                    "END";
                QueryCommand checkPriceSchemeTable = new QueryCommand(sqlCheckPriceSchemeTable, providerName);
                DataService.ExecuteQuery(checkPriceSchemeTable);

                ItemCollection IT = new ItemCollection();
                IT.LoadAndCloseReader(SubSonic.DataService.GetReader(new QueryCommand("SELECT * FROM Item")));

                foreach (DataRow Rw in ExcelData.Rows)
                {
                    QueryCommandCollection LocalCmds = new QueryCommandCollection();
                    string ErrorText;
                    ErrorText = "";

                    Dictionary<string, object> Cels = new Dictionary<string, object>();
                    for (int Counter = 0; Counter < ExcelData.Columns.Count; Counter++)
                        Cels.Add(ExcelData.Columns[Counter].ColumnName, Rw[Counter]);

                    string CurrItemValue = Cels["Item No"] == null ? "" : Cels["Item No"].ToString();
                    #region *) Validation: Check Null [ItemNo field]
                    if (string.IsNullOrEmpty(CurrItemValue))
                    {
                        ErrorText += "Item No cannot be null\n";
                    }
                    #endregion

                    // Check Item Existence
                    Item ItemRw = IT.FirstOrDefault(Fnc => Fnc.ItemNo.Trim() == CurrItemValue.Trim());
                    if (ItemRw == null)
                    {
                        ErrorText += "ItemNo is not found\n";
                    }
                    else
                    {
                        // Update RetailPrice Item
                        decimal tempDecimal = 0;
                        decimal.TryParse(Cels["Retail Price"].ToString(), out tempDecimal);
                        ItemRw.RetailPrice = tempDecimal;
                        LocalCmds.Add(ItemRw.GetUpdateCommand(UserInfo.username));

                        // Update Scheme(s) Price (Loop Thru 2 until Count)
                        for (int i = 2; i < Cels.Count; i++)
                        {
                            string sqlUpsertPriceScheme = 
                                "IF EXISTS ( SELECT SchemeID FROM PriceScheme " +
                                    "WHERE SchemeID = @SchemeID AND ItemNo = @ItemNo) " +
                                "BEGIN " +
                                    "IF (@SchemePrice <> -1 ) " +
                                    "BEGIN " +
	                                    "UPDATE PriceScheme SET Price = @SchemePrice " +
		                                    "WHERE SchemeID = @SchemeID " +
                                    "END " +
                                    "ELSE " +
                                    "BEGIN " +
	                                    "DELETE PriceScheme WHERE SchemeID = @SchemeID " +
                                    "END " +
                                "END " +
                                "ELSE " +
                                "BEGIN " +
                                    "IF (@SchemePrice <> -1 ) " +
                                    "BEGIN " +
	                                    "INSERT INTO PriceScheme (SchemeID, ItemNo, Price) " +
		                                    "VALUES (@SchemeID, @ItemNo, @SchemePrice) " +
                                    "END " +
                                "END";
                            QueryCommand upsertPriceScheme = new QueryCommand(sqlUpsertPriceScheme, providerName);
                            upsertPriceScheme.Parameters.Add("@ItemNo", CurrItemValue);
                            upsertPriceScheme.Parameters.Add("@SchemeID", ColumnNames[i]);
                            upsertPriceScheme.Parameters.Add("@SchemePrice", Cels[ColumnNames[i]].ToString());
                            LocalCmds.Add(upsertPriceScheme);
                        }
                    }

                    if (ErrorText != "")
                    {
                        Err.Rows.Add(Rw.ItemArray);
                        Err.Rows[Err.Rows.Count - 1].SetField("Err", ErrorText);
                    }
                    else
                    {
                        Cmds.AddRange(LocalCmds);
                    }
                }
                #endregion

                if (Err.Rows.Count <= 0)
                {
                    var tempCmd = (from cmd in Cmds
                                   where cmd != null
                                   select cmd).ToList();
                    QueryCommandCollection qCmdColl = new QueryCommandCollection();
                    qCmdColl.AddRange(tempCmd);

                    SubSonic.DataService.ExecuteTransaction(qCmdColl);
                }

                TheReport.SetDataSource(Err);
                TheReport.Refresh();

                CrystalReportViewer1.ReportSource = TheReport;
                CrystalReportViewer1.RefreshReport();

                if (Err.Rows.Count > 0)
                {
                    lblMsg.Text = "Error - Items are not saved - Please see the downloaded file for more info";
                    TheReport.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "SalesImporter_UploadError_" + DateTime.Now.ToString("dd MMM yyyy HHmmss"));
                }

                lblMsg.Text = "Update Successful - " + ExcelData.Rows.Count.ToString() + " Rows Updated";
            }
            catch (Exception X)
            {
                lblMsg.Text = "Error - Items are not saved - " + X.Message;
                Logger.writeLog(X);
            }
        }
    }
}
