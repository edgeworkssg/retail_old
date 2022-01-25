using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PowerPOS;
using SubSonic;
using System.Linq;

namespace PowerWeb.Support
{
    public partial class ItemImporter : System.Web.UI.Page
    {
        private const string ExportedFileName = "ItemImport";
        private const string ImportedFileNameSuffix = "ItemImport";
        private const string ImportedFileRptFileName = "ItemImportView.rpt";
        private const string vs_ImportedFileName = "ItemImportFile";
        private const string vs_ErrorFileName = "ItemImportErrorFile";
        private const string vs_ImportedRptFileName = "ItemImportRptFile";
        private const string vs_ErrorRptFileName = "ItemImportErrorRptFile";
        private const string vs_BoundFileName = "ItemImportBindedFile";
        private const string vs_BoundRptFileName = "ItemImportBindedRptFile";

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
            ColumnNames.AddRange("Department,Category,ItemNo,ItemName,Barcode,Retail Price,Cost Price,Service Item,Inventory Item,Non Discountable,Give Commission,Balance qty".Split(','));
            ColumnTypes.AddRange("System.String,System.String,System.String,System.String,System.String,System.Decimal,System.Decimal,System.Boolean,System.Boolean,System.Boolean,System.Boolean,System.Decimal".Split(','));

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

            DataTable ExcelData;
            #region *) Get Excel Data
            string Status = "";

            ExcelData = ExcelDataLogic.OpenExcelFile(FileName, ColumnNames.ToArray(), ColumnTypes.ToArray(), out Status);
            #endregion

            if (Status != "") Logger.writeLog(Status);

            for (int Counter = 0; Counter < LblList.Count; Counter++)
                ExcelData.Columns[LblList[Counter].Label].ColumnName = "Attributes" + LblList[Counter].AttributesNo.ToString();

            TheReport.SetDataSource(ExcelData);
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
                ReportDocument TheReport = new ReportDocument();
                #region *) Get Report Template
                TheReport.Load(ReportName);
                #endregion

                DataTable ExcelData;
                #region *) Get Excel Data
                string Status = "";

                ExcelData = ExcelDataLogic.OpenExcelFile(FileName, ColumnNames.ToArray(), ColumnTypes.ToArray(), out Status);
                #endregion

                DataTable Err = new DataTable();
                Err = ExcelData.Clone();
                if (!Err.Columns.Contains("Err"))
                    Err.Columns.Add("Err", Type.GetType("System.String"));
                Err.Rows.Clear();

                #region *) Try to Save Data, Put all the error into Err DataTable
                QueryCommandCollection Cmds = new QueryCommandCollection();

                ItemCollection IT = new ItemCollection();
                CategoryCollection IC = new CategoryCollection();
                ItemDepartmentCollection II = new ItemDepartmentCollection();

                II.LoadAndCloseReader(SubSonic.DataService.GetReader(new QueryCommand("SELECT * FROM ItemDepartment")));
                IC.LoadAndCloseReader(SubSonic.DataService.GetReader(new QueryCommand("SELECT * FROM Category")));
                IT.LoadAndCloseReader(SubSonic.DataService.GetReader(new QueryCommand("SELECT * FROM Item")));

                List<string> listItemDepartment = new List<string>();
                List<string> listCategory = new List<string>();
                List<string> listItem = new List<string>();

                bool anyError = false;

                for (int Counter = 0; Counter < LblList.Count; Counter++)
                    ExcelData.Columns[LblList[Counter].Label].ColumnName = "Attributes" + LblList[Counter].AttributesNo.ToString();

                foreach (DataRow Rw in ExcelData.Rows)
                {
                    QueryCommandCollection LocalCmds = new QueryCommandCollection();
                    string ErrorText;
                    ErrorText = "";

                    Dictionary<string, object> Cels = new Dictionary<string, object>();
                    for (int Counter = 0; Counter < ExcelData.Columns.Count; Counter++)
                        if (LblList.Count(Fnc => Fnc.Label == ExcelData.Columns[Counter].ColumnName) > 0)
                            Cels.Add("Attributes" + LblList.First(Fnc => Fnc.Label == ExcelData.Columns[Counter].ColumnName).AttributesNo, Rw[Counter]);
                        else
                            Cels.Add(ExcelData.Columns[Counter].ColumnName, Rw[Counter]);

                    #region *) Repair: Remove Null Values
                    //foreach(string OneKey in Cels.Keys)
                    //    if (Cels[OneKey] == null || Cels[OneKey].ToString() == "") Cels[OneKey] = Tabel.Columns[Cel.ColumnIndex].DefaultCellStyle.NullValue;
                    ////foreach (object[] Cel in Rw.ItemArray)
                    ////    if (Cel == null || Cel.ToString() == "") Cel = Tabel.Columns[Cel.ColumnIndex].DefaultCellStyle.NullValue;
                    #endregion

                    string CurrDepartmentValue = Cels["Department"] == null ? "" : Cels["Department"].ToString();
                    #region *) Validation: Check Null [Department field]
                    /* Not Needed */
                    #endregion
                    ItemDepartment DepartmentRw = II.FirstOrDefault(Fnc => Fnc.ItemDepartmentID == CurrDepartmentValue);
                    #region *) Core: Upsert ItemDepartment
                    if (DepartmentRw == null)
                    {   /// Insert
                        DepartmentRw = new ItemDepartment();

                        DepartmentRw.ItemDepartmentID = CurrDepartmentValue;
                        DepartmentRw.DepartmentName = CurrDepartmentValue;
                        DepartmentRw.Remark = null;
                        //DepartmentRw.CreatedOn = DateTime.Now;
                        //DepartmentRw.CreatedBy = "Item Importer";
                        //DepartmentRw.ModifiedOn = DateTime.Now;
                        //DepartmentRw.ModifiedBy = "Item Importer";
                        DepartmentRw.Deleted = false;

                        II.Add(DepartmentRw);
                        LocalCmds.Add(DepartmentRw.GetSaveCommand("Web Item Importer"));

                        listItemDepartment.Add(CurrDepartmentValue);
                    }
                    else
                    {   /// Update - Not relevant
                        if (!listItemDepartment.Contains(CurrDepartmentValue))
                        {   /// not First time update
                        }
                        else
                        {   /// First time update
                        }
                    }
                    #endregion

                    string CurrCategoryValue = Cels["Category"] == null ? "" : Cels["Category"].ToString();
                    #region *) Validation: Check Null [Category field]
                    if (string.IsNullOrEmpty(CurrCategoryValue))
                    {
                        ErrorText += "Category cannot be null\n";
                    }
                    #endregion
                    if (!(string.IsNullOrEmpty(CurrCategoryValue) || string.IsNullOrEmpty(CurrDepartmentValue)))
                    {
                        Category CategoryRw = IC.FirstOrDefault(Fnc => Fnc.CategoryName == CurrCategoryValue);
                        #region *) Core: Upsert Category
                        if (CategoryRw == null)
                        {   /// Insert
                            CategoryRw = new Category();

                            CategoryRw.CategoryName = CurrCategoryValue;
                            CategoryRw.Remarks = "";
                            CategoryRw.CategoryId = "";
                            CategoryRw.IsDiscountable = true;
                            CategoryRw.IsForSale = true;
                            CategoryRw.IsGST = true;
                            CategoryRw.AccountCategory = "";
                            CategoryRw.ItemDepartmentId = CurrDepartmentValue;
                            //CategoryRw.CreatedOn = DateTime.Now;
                            //CategoryRw.CreatedBy = "Item Importer";
                            //CategoryRw.ModifiedOn = DateTime.Now;
                            //CategoryRw.ModifiedBy = "Item Importer";
                            CategoryRw.Deleted = false;
                            //CategoryRw.ItemDepartmentRow = this.itemBeautyDS1.ItemDepartment.FindByItemDepartmentID(CurrDepartmentValue);

                            IC.Add(CategoryRw);
                            LocalCmds.Add(CategoryRw.GetSaveCommand("Web Item Importer"));

                            listCategory.Add(CurrCategoryValue);
                        }
                        else
                        {   /// Update - Not relevant
                            if (listCategory.Contains(CurrCategoryValue))
                            {   /// not First time update
                                if (CategoryRw.ItemDepartmentId != CurrDepartmentValue)
                                {
                                    ErrorText += "Category just assigned to Department [" + CategoryRw.ItemDepartmentId + "]\n";
                                }
                            }
                            else
                            {   /// First time update
                                CategoryRw.ItemDepartmentId = CurrDepartmentValue;
                                //CategoryRw.ModifiedBy = "ItemImporter";
                                //CategoryRw.ModifiedOn = DateTime.Now;
                                LocalCmds.Add(CategoryRw.GetSaveCommand("Web Item Importer"));
                                listCategory.Add(CurrCategoryValue);
                            }
                        }
                        #endregion
                    }

                    string CurrItemValue = Cels["ItemNo"] == null ? "" : Cels["ItemNo"].ToString();
                    #region *) Validation: Check Null [ItemNo field]
                    if (string.IsNullOrEmpty(CurrItemValue))
                    {
                        ErrorText += "ItemNo cannot be null\n";
                        anyError = true;
                    }
                    #endregion
                    if (!(string.IsNullOrEmpty(CurrCategoryValue) || string.IsNullOrEmpty(CurrDepartmentValue)))
                    {
                        Item ItemRw = IT.FirstOrDefault(Fnc => Fnc.ItemNo.Trim() == CurrItemValue.Trim());
                        #region *) Core: Upsert Item
                        if (ItemRw == null)
                        {   /// Insert
                            ItemRw = new Item();

                            ItemRw.ItemNo = CurrItemValue.Trim();
                            ItemRw.ItemName = Cels["ItemName"].ToString();
                            ItemRw.Barcode = Cels["Barcode"].ToString();
                            ItemRw.CategoryName = CurrCategoryValue;

                            ItemRw.RetailPrice = string.IsNullOrEmpty(Cels["Retail Price"].ToString()) ?
                                0 : ItemRw.RetailPrice = decimal.Parse(Cels["Retail Price"].ToString().Replace("$", "").Replace(",", ""));
                            ItemRw.FactoryPrice = string.IsNullOrEmpty(Cels["Cost Price"].ToString()) ?
                                0 : ItemRw.FactoryPrice = decimal.Parse(Cels["Cost Price"].ToString().Replace("$", "").Replace(",", ""));
                            ItemRw.MinimumPrice = 0;
                            ItemRw.ItemDesc = null;
                            //bool TempValue = false;
                            { ItemRw.IsServiceItem = false; }
                            { ItemRw.IsInInventory = true; }
                            { ItemRw.IsNonDiscountable = false; }
                            { ItemRw.IsCommission = true; }
                            if (Cels.ContainsKey("Attributes1")) ItemRw.Attributes1 = (Cels["Attributes1"] == null) ? null : Cels["Attributes1"].ToString();
                            if (Cels.ContainsKey("Attributes2")) ItemRw.Attributes2 = (Cels["Attributes2"] == null) ? null : Cels["Attributes2"].ToString();
                            if (Cels.ContainsKey("Attributes3")) ItemRw.Attributes3 = (Cels["Attributes3"] == null) ? null : Cels["Attributes3"].ToString();
                            if (Cels.ContainsKey("Attributes4")) ItemRw.Attributes4 = (Cels["Attributes4"] == null) ? null : Cels["Attributes4"].ToString();
                            if (Cels.ContainsKey("Attributes5")) ItemRw.Attributes5 = (Cels["Attributes5"] == null) ? null : Cels["Attributes5"].ToString();
                            if (Cels.ContainsKey("Attributes6")) ItemRw.Attributes6 = (Cels["Attributes6"] == null) ? null : Cels["Attributes6"].ToString();
                            if (Cels.ContainsKey("Attributes7")) ItemRw.Attributes7 = (Cels["Attributes7"] == null) ? null : Cels["Attributes7"].ToString();

                            if (Cels.ContainsKey("EnglishAttributes1")) ItemRw.Userfld1 = (Cels["EnglishAttributes1"] == null) ? null : Cels["EnglishAttributes1"].ToString();
                            if (Cels.ContainsKey("EnglishAttributes2")) ItemRw.Userfld2 = (Cels["EnglishAttributes2"] == null) ? null : Cels["EnglishAttributes2"].ToString();
                            if (Cels.ContainsKey("EnglishAttributes3")) ItemRw.Userfld3 = (Cels["EnglishAttributes3"] == null) ? null : Cels["EnglishAttributes3"].ToString();
                            if (Cels.ContainsKey("EnglishAttributes4")) ItemRw.Userfld4 = (Cels["EnglishAttributes4"] == null) ? null : Cels["EnglishAttributes4"].ToString();
                            if (Cels.ContainsKey("EnglishAttributes5")) ItemRw.Userfld5 = (Cels["EnglishAttributes5"] == null) ? null : Cels["EnglishAttributes5"].ToString();
                            if (Cels.ContainsKey("EnglishAttributes6")) ItemRw.Userfld6 = (Cels["EnglishAttributes6"] == null) ? null : Cels["EnglishAttributes6"].ToString();
                            if (Cels.ContainsKey("EnglishAttributes7")) ItemRw.Userfld7 = (Cels["EnglishAttributes7"] == null) ? null : Cels["EnglishAttributes7"].ToString();

                            if (Cels.ContainsKey("EnglishCategory"))
                                if (!string.IsNullOrEmpty(Cels["EnglishCategory"].ToString()))
                                    ItemRw.ItemDesc = Cels["cEnglishCategory"].ToString();


                            ItemRw.UniqueID = Guid.NewGuid();
                            //ItemRw.CreatedOn = DateTime.Now;
                            //ItemRw.CreatedBy = "Item Importer";
                            //ItemRw.ModifiedOn = DateTime.Now;
                            //ItemRw.ModifiedBy = "Item Importer";
                            //if (bool.TryParse(Cels["Active"].ToString(), out TempValue))
                            //{ ItemRw.Deleted = !TempValue; }
                            //else
                            //{ ItemRw.Deleted = false; }
                            ItemRw.Deleted = false;
                            ItemRw.GSTRule = 3;
                            //ItemRw.CategoryRow = this.itemBeautyDS1.Category.FindByCategoryName(CurrCategoryValue);

                            IT.Add(ItemRw);
                            LocalCmds.Add(ItemRw.GetSaveCommand("Web Item Importer"));

                            listItem.Add(CurrItemValue);
                        }
                        else
                        {   /// Update
                            if (listItem.Contains(CurrItemValue))
                            {   /// not First time update
                                //if (ItemRw.CategoryName != CurrDepartmentValue)
                                {
                                    ErrorText += "Duplicated Item No. Each individual Item must have unique Item No." + Environment.NewLine;
                                }
                            }
                            else
                            {   /// First time update
                                ItemRw.ItemName = Cels["ItemName"].ToString();
                                ItemRw.Barcode = Cels["Barcode"].ToString();
                                ItemRw.CategoryName = CurrCategoryValue;
                                ItemRw.RetailPrice = string.IsNullOrEmpty(Cels["Retail Price"].ToString()) ?
                                    0 : ItemRw.RetailPrice = decimal.Parse(Cels["Retail Price"].ToString().Replace("$", "").Replace(",", ""));
                                ItemRw.FactoryPrice = string.IsNullOrEmpty(Cels["Cost Price"].ToString()) ?
                                    0 : ItemRw.FactoryPrice = decimal.Parse(Cels["Cost Price"].ToString().Replace("$", "").Replace(",", ""));
                                //bool TempValue = false;
                                //if (bool.TryParse(Cels["Active"].ToString(), out TempValue))
                                //{ ItemRw.Deleted = !TempValue; }
                                //else
                                //{ ItemRw.Deleted = false; }
                                ItemRw.Deleted = false;
                                if (Cels.ContainsKey("Attributes1")) ItemRw.Attributes1 = (Cels["Attributes1"] == null) ? null : Cels["Attributes1"].ToString();
                                if (Cels.ContainsKey("Attributes2")) ItemRw.Attributes2 = (Cels["Attributes2"] == null) ? null : Cels["Attributes2"].ToString();
                                if (Cels.ContainsKey("Attributes3")) ItemRw.Attributes3 = (Cels["Attributes3"] == null) ? null : Cels["Attributes3"].ToString();
                                if (Cels.ContainsKey("Attributes4")) ItemRw.Attributes4 = (Cels["Attributes4"] == null) ? null : Cels["Attributes4"].ToString();
                                if (Cels.ContainsKey("Attributes5")) ItemRw.Attributes5 = (Cels["Attributes5"] == null) ? null : Cels["Attributes5"].ToString();
                                if (Cels.ContainsKey("Attributes6")) ItemRw.Attributes6 = (Cels["Attributes6"] == null) ? null : Cels["Attributes6"].ToString();
                                if (Cels.ContainsKey("Attributes7")) ItemRw.Attributes7 = (Cels["Attributes7"] == null) ? null : Cels["Attributes7"].ToString();

                                if (Cels.ContainsKey("EnglishAttributes1")) ItemRw.Userfld1 = (Cels["EnglishAttributes1"] == null) ? null : Cels["EnglishAttributes1"].ToString();
                                if (Cels.ContainsKey("EnglishAttributes2")) ItemRw.Userfld2 = (Cels["EnglishAttributes2"] == null) ? null : Cels["EnglishAttributes2"].ToString();
                                if (Cels.ContainsKey("EnglishAttributes3")) ItemRw.Userfld3 = (Cels["EnglishAttributes3"] == null) ? null : Cels["EnglishAttributes3"].ToString();
                                if (Cels.ContainsKey("EnglishAttributes4")) ItemRw.Userfld4 = (Cels["EnglishAttributes4"] == null) ? null : Cels["EnglishAttributes4"].ToString();
                                if (Cels.ContainsKey("EnglishAttributes5")) ItemRw.Userfld5 = (Cels["EnglishAttributes5"] == null) ? null : Cels["EnglishAttributes5"].ToString();
                                if (Cels.ContainsKey("EnglishAttributes6")) ItemRw.Userfld6 = (Cels["EnglishAttributes6"] == null) ? null : Cels["EnglishAttributes6"].ToString();
                                if (Cels.ContainsKey("EnglishAttributes7")) ItemRw.Userfld7 = (Cels["EnglishAttributes7"] == null) ? null : Cels["EnglishAttributes7"].ToString();

                                if (Cels.ContainsKey("EnglishCategory"))
                                    if (!string.IsNullOrEmpty(Cels["EnglishCategory"].ToString()))
                                        ItemRw.ItemDesc = Cels["cEnglishCategory"].ToString();

                                //ItemRw.ModifiedOn = DateTime.Now;
                                //ItemRw.ModifiedBy = "Item Importer";
                                ItemRw.GSTRule = 3;

                                LocalCmds.Add(ItemRw.GetSaveCommand("Web Item Importer"));

                                listItem.Add(CurrItemValue);
                            }
                        }
                        #endregion
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
