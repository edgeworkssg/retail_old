using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SpreadsheetLight;
using System.Data;
using PowerPOS;
using PowerWeb.BLL.Controller;
using System.IO;

namespace PowerWeb.Support
{
    public partial class LowQtyImporter : System.Web.UI.Page
    {
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

            sl.AutoFitColumn(1, iEndColumnIndex);

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

        protected void ddlInventoryLocation_Init(object sender, EventArgs e)
        {
            var data = new InventoryLocationController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false).OrderBy(o => o.InventoryLocationName).ToList();
            data.Insert(0, new InventoryLocation { InventoryLocationID = 0, InventoryLocationName = "ALL" });
            var ddl = (DropDownList)sender;
            ddl.DataValueField = InventoryLocation.Columns.InventoryLocationID;
            ddl.DataTextField = InventoryLocation.Columns.InventoryLocationName;
            ddl.DataSource = data;
            ddl.DataBind();
        }

        protected DataTable GetInitialDataTable()
        {
            DataTable dtInput = new DataTable();
            dtInput.Columns.Add("Department Code", typeof(string)); // 0
            dtInput.Columns.Add("Department Name", typeof(string));//1
            dtInput.Columns.Add("Category Code", typeof(string));//2
            dtInput.Columns.Add("Category Name", typeof(string));//3
            dtInput.Columns.Add("Item No", typeof(string)); // 4
            dtInput.Columns.Add("Item Name", typeof(string));//5
            dtInput.Columns.Add("Trigger Quantity", typeof(string));//6
            dtInput.Columns.Add("Inventory Location", typeof(string));//7
            dtInput.Columns.Add("Deleted", typeof(string));//8

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1)))
            {
                dtInput.Columns.Add(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1), typeof(string));//9
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2)))
            {
                dtInput.Columns.Add(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2), typeof(string));//10   
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3)))
            {
                dtInput.Columns.Add(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3), typeof(string));//11
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4)))
            {
                dtInput.Columns.Add(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4), typeof(string));//12
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5)))
            {
                dtInput.Columns.Add(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5), typeof(string));//13
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6)))
            {
                dtInput.Columns.Add(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6), typeof(string));//14
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7)))
            {
                dtInput.Columns.Add(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7), typeof(string));//15
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8)))
            {
                dtInput.Columns.Add(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8), typeof(string));//16
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9)))
            {
                dtInput.Columns.Add(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9), typeof(string));//17
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10)))
            {
                dtInput.Columns.Add(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10), typeof(string));//18
            }

            return dtInput;
        }

        protected void ddlDepartment_Init(object sender, EventArgs e)
        {
            var data = new ItemDepartmentController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false && (o.DepartmentName + "").ToUpper() != "SYSTEM").OrderBy(o => o.DepartmentName).ToList();
            data.Insert(0, new ItemDepartment { ItemDepartmentID = "ALL", DepartmentName = "ALL" });
            var ddl = (DropDownList)sender;
            ddl.DataValueField = ItemDepartment.Columns.ItemDepartmentID;
            ddl.DataTextField = ItemDepartment.Columns.DepartmentName;
            ddl.DataSource = data;
            ddl.DataBind();
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = new CategoryController().FetchAll()
                           .Where(o => o.Deleted.GetValueOrDefault(false) == false)
                           .Where(o => o.ItemDepartmentId == ddlDepartment.SelectedValue || ddlDepartment.SelectedIndex == 0)
                           .OrderBy(o => o.CategoryName)
                           .ToList();
            data.Insert(0, new Category { CategoryId = "0", CategoryName = "ALL" });
            var ddl = ddlCategory;
            ddl.DataValueField = Category.Columns.CategoryName;
            ddl.DataTextField = Category.Columns.CategoryName;
            ddl.DataSource = data;
            ddl.DataBind();
        }

        protected void ddlCategory_Init(object sender, EventArgs e)
        {
            var data = new CategoryController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false && (o.CategoryName + "").ToUpper() != "SYSTEM").OrderBy(o => o.CategoryName).ToList();
            data.Insert(0, new Category { CategoryId = "0", CategoryName = "ALL" });
            var ddl = (DropDownList)sender;
            ddl.DataValueField = Category.Columns.CategoryName;
            ddl.DataTextField = Category.Columns.CategoryName;
            ddl.DataSource = data;
            ddl.DataBind();
        }

        protected void btnExportBlank_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataTable dt = GetInitialDataTable();
            ExportData("LowQtyImporter", dt);
        }

        protected void btnExportItemQuanity_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";

            DataTable dt = LowQtyImporterController.FetchItemWithExisting(ddlInventoryLocation.SelectedValue.GetIntValue(), ddlDepartment.SelectedValue, ddlCategory.SelectedValue, cbIsExportExistingItemOnly.Checked);
            ExportData("LowQtyImporter", dt);
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
                        int iStartRowIndex = stats.StartRowIndex + 1;
                        int iEndRowIndex = stats.EndRowIndex;
                        int iEndRowDataIndex = iEndRowIndex;
                        int iNumberColumn = stats.NumberOfColumns;
                        for (int i = iEndRowIndex; i >= iStartRowIndex; i--)
                        {
                            if (sl.HasCellValue(i, 5))
                            {
                                iEndRowDataIndex = i;
                                break;
                            }
                        }

                        if (dtInput.Columns.Count != iNumberColumn)
                        {
                            throw new Exception("The number of columns doesn't match. ");
                        }

                        for (int row = iStartRowIndex; row <= iEndRowDataIndex; ++row)
                        {
                            DataRow dr = dtInput.NewRow();
                            for (int index = 0; index < iNumberColumn; index++)
                            {
                                dr[index] = sl.GetCellValueAsString(row,index + 1);
                            }

                            dtInput.Rows.Add(dr);
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
                    bool isSuccess = LowQtyImporterController.ImportData(Session["UserName"] + "",
                        dtInput, out resultDt, out status);
                    if (isSuccess)
                    {
                        lblStatus.Text = "Data imported";
                        ExportData(Path.GetFileNameWithoutExtension(fuItemImporter.FileName), resultDt);
                    }
                    else
                    {
                        lblStatus.Text = "ERROR : " + status;
                        if (resultDt.Rows.Count > 0)
                            ExportData(Path.GetFileNameWithoutExtension(fuItemImporter.FileName), resultDt);
                    }
                }
                else
                {
                    lblStatus.Text = "No Data Imported";
                }

                #endregion
            }
        }
    }
}
