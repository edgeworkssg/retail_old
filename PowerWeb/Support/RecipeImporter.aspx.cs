using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using PowerPOS;
using System.Data;
using SpreadsheetLight;
using System.IO;

namespace PowerWeb.Support
{
    public partial class RecipeImporter : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                trImportData.Visible = false;
                UserMst um = new UserMst(Session["UserName"] + "");
                if (!um.IsNew)
                {
                    trImportData.Visible = um.IsHavePrivilegesFor("IMPORT RECIPES");
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
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
            //for (int i = 2; i <= iEndRowIndex; i++)
            //{

            //}

            sl.FreezePanes(1, 0);

            if (dt.Columns[dt.Columns.Count - 1].ColumnName == "Status")
            {
                SLStyle styleWarp = sl.CreateStyle();
                styleWarp.SetWrapText(true);
                sl.SetColumnStyle(dt.Columns.Count - 1, styleWarp);
            }

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "_Result.xlsx");
            sl.SaveAs(Response.OutputStream);
            Response.End();
        }

        protected void btnExportBlank_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataTable dt = RecipeImporterController.FetchItem("XX__XX", "XX__XX");
            ExportData("RecipeImporter", dt);
        }

        protected void btnExportItemDept_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataTable dt = RecipeImporterController.FetchItem(ddlDepartment.SelectedValue, ddlCategory.SelectedValue);

            string itemDept = ddlDepartment.SelectedItem.Text;
            string category = ddlCategory.SelectedItem.Text;


            if (dt.Rows.Count > 0)
                ExportData(string.Format("Recipe_ItemDept_{0}_Ctg_{1}", itemDept, category), dt);
            else
                lblStatus.Text = "Data not found";
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            if (fuItemImporter.HasFile)
            {
                DataTable dtInput = new DataTable();
                dtInput.Columns.Add("Department Code", typeof(string)); // 0
                dtInput.Columns.Add("Department Name", typeof(string));//1
                dtInput.Columns.Add("Category Code", typeof(string));//2
                dtInput.Columns.Add("Category Name", typeof(string));//3
                dtInput.Columns.Add("ItemNo", typeof(string));//4
                dtInput.Columns.Add("ItemName", typeof(string));//5
                dtInput.Columns.Add("MaterialItemNo", typeof(string));//6           
                dtInput.Columns.Add("MaterialItemName", typeof(string));//7
                dtInput.Columns.Add("Qty", typeof(string));//8
                dtInput.Columns.Add("UOM", typeof(string));//9
                dtInput.Columns.Add("IsPacking", typeof(string));//9

                #region *) Read Excel

                try
                {
                    using (SLDocument sl = new SLDocument(fuItemImporter.PostedFile.InputStream, "Sheet1"))
                    {
                        SLWorksheetStatistics stats = sl.GetWorksheetStatistics();
                        int iStartColumnIndex = stats.StartColumnIndex;
                        for (int row = stats.StartRowIndex + 1; row <= stats.EndRowIndex; ++row)
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
                                sl.GetCellValueAsString(row, iStartColumnIndex + 10));
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
                    bool isSuccess = RecipeImporterController.ImportData(Session["UserName"] + "",
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
        protected void ddlDepartment_Init(object sender, EventArgs e)
        {
            var data = new ItemDepartmentController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false).OrderBy(o => o.DepartmentName).ToList();
            data.Insert(0, new ItemDepartment { ItemDepartmentID = "ALL", DepartmentName = "ALL" });
            var ddl = (DropDownList)sender;
            ddl.DataValueField = ItemDepartment.Columns.ItemDepartmentID;
            ddl.DataTextField = ItemDepartment.Columns.DepartmentName;
            ddl.DataSource = data;
            ddl.DataBind();
        }
        protected void ddlCategory_Init(object sender, EventArgs e)
        {
            var data = new CategoryController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false).OrderBy(o => o.CategoryName).ToList();

            var finalData = (from o in data
                             select new CategoryData
                             {
                                 CategoryID = o.CategoryId,
                                 CategoryName = o.CategoryName
                             }).ToList();

            finalData.Insert(0, new CategoryData { CategoryID = "ALL", CategoryName = "ALL" });
            var ddl = (DropDownList)sender;
            ddl.DataValueField = Category.Columns.CategoryName;
            ddl.DataTextField = Category.Columns.CategoryName;
            ddl.DataSource = finalData;
            ddl.DataBind();
        }
        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = new CategoryController().FetchAll()
                   .Where(o => o.Deleted.GetValueOrDefault(false) == false)
                   .Where(o => o.ItemDepartmentId == ddlDepartment.SelectedValue || ddlDepartment.SelectedValue == "0")
                   .OrderBy(o => o.CategoryName)
                   .ToList();

            var finalData = (from o in data
                             select new CategoryData
                             {
                                 CategoryID = o.CategoryId,
                                 CategoryName = o.CategoryName
                             }).ToList();

            finalData.Insert(0, new CategoryData { CategoryID = "ALL", CategoryName = "ALL" });

            var ddl = ddlCategory;
            ddl.DataValueField = Category.Columns.CategoryName;
            ddl.DataTextField = Category.Columns.CategoryName;
            ddl.DataSource = finalData;
            ddl.DataBind();
        }
    }

    public class CategoryData
    {
        public string CategoryID { set; get; }
        public string CategoryName { set; get; }
    }
}
