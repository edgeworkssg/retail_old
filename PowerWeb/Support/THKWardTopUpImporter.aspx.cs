using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using System.Data;
using SpreadsheetLight;
using PowerWeb.BLL.Controller;
using System.IO;

namespace PowerWeb.Support
{
    public partial class THKWardTopUpImporter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void ddlInventoryLocation_Init(object sender, EventArgs e)
        {
           
            string username = "";
            UserMst usr = new UserMst();
            if (Session["UserName"] != null)
            {
                username = Session["UserName"].ToString();
                usr = new UserMst(username);
            }
            var data = new InventoryLocationController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false).OrderBy(o => o.InventoryLocationName).ToList();
            if (!String.IsNullOrEmpty(usr.AssignedOutlet) && usr.AssignedOutlet != "ALL")
            {
                InventoryLocationCollection invLocListToDelete = new InventoryLocationCollection();
                foreach (InventoryLocation invLoc in data)
                {
                    bool isFound = false;
                    foreach (string tmp in usr.AssignedOutletList)
                    {
                        if (tmp == invLoc.InventoryLocationName)
                            isFound = true;
                    }
                    if (!isFound)
                        invLocListToDelete.Add(invLoc);
                }

                foreach (InventoryLocation invLocToDelete in invLocListToDelete)
                {
                    data.Remove(invLocToDelete);
                }
            }
            
            var ddl = (DropDownList)sender;
            ddl.DataValueField = InventoryLocation.Columns.InventoryLocationID;
            ddl.DataTextField = InventoryLocation.Columns.InventoryLocationName;
            ddl.DataSource = data;
            ddl.DataBind();
        }

        protected void btnExportBlank_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataTable dt = GetInitialDataTable();
            ExportData("WardTopUpFile", dt);
        }

        protected DataTable GetInitialDataTable()
        {
            DataTable dtInput = new DataTable();
            dtInput.Columns.Add("Department OU", typeof(string)); // 0
            dtInput.Columns.Add("Nursing OU", typeof(string));//1
            dtInput.Columns.Add("Plant", typeof(string));//2
            dtInput.Columns.Add("Storage Location", typeof(string));//3
            dtInput.Columns.Add("Material Code", typeof(string)); // 4
            dtInput.Columns.Add("Material Description", typeof(string));//5
            dtInput.Columns.Add("Par Value", typeof(string));//6
            dtInput.Columns.Add("UOM", typeof(string));//7

            return dtInput;
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

        protected void btnImport_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            if (ddlInventoryLocation.SelectedValue == "0")
            {
                lblStatus.Text = "Please Select Inventory Location";
            }
            else
            {
                if (fuItemImporter.HasFile)
                {
                    lblStatus.Text = "";
                    using (SLDocument SLMain = new SLDocument(fuItemImporter.PostedFile.InputStream))
                    {
                        foreach (var name in SLMain.GetSheetNames())
                        {
                            DataTable dtInput = GetInitialDataTable();

                            #region *) Read Excel
                            try
                            {
                                using (SLDocument sl = new SLDocument(fuItemImporter.PostedFile.InputStream, name))
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
                                            dr[index] = sl.GetCellValueAsString(row, index + 1);
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
                                bool isSuccess = LowQtyImporterController.ImportWardTopUpdata(Session["UserName"] + "",
                                    dtInput, ddlInventoryLocation.SelectedValue.GetIntValue(), out resultDt, out status);
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
                        }

                        #endregion
                    }
                }
            }
        }

        protected void btnExportItemWardTopUp_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            if (ddlInventoryLocation.SelectedValue == "0")
            {
                lblStatus.Text = "Please Select Inventory Location";
            }
            else
            {
                DataTable dt = LowQtyImporterController.FetchItemWardTopUp(ddlInventoryLocation.SelectedValue.GetIntValue());
                ExportData("WardTopUpFile", dt);
            }
        }
    }
}
