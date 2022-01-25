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
using SubSonic;
using SubSonic.Utilities;

namespace PowerWeb.Support
{
    public partial class MembershipImporter : PageBase
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

            sl.AutoFitColumn(iStartColumnIndex, iEndColumnIndex);

            sl.FreezePanes(1, 0);

            if (dt.Columns[dt.Columns.Count - 1].ColumnName == "Status")
            {
                SLStyle styleWarp = sl.CreateStyle();
                styleWarp.SetWrapText(true);
                sl.SetColumnStyle(dt.Columns.Count, styleWarp);
            }

            foreach (DataColumn col in dt.Columns)
            {
                // Format the date column
                if (col.ColumnName == "Date Of Birth" || col.ColumnName == "Subscription Date" || col.ColumnName == "Expiry Date")
                {
                    SLStyle styleWarp = sl.CreateStyle();
                    styleWarp.FormatCode = "dd-MMM-yyyy";
                    styleWarp.Alignment.Horizontal = DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Left;
                    sl.SetColumnStyle(col.Ordinal + 1, styleWarp);
                }
            }

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "_Result.xlsx");
            sl.SaveAs(Response.OutputStream);
            Response.End();
        }

        protected void ddlDepartment_Init(object sender, EventArgs e)
        {
            var data = new MembershipGroupController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false).OrderBy(o => o.GroupName).ToList();
            data.Insert(0, new MembershipGroup { MembershipGroupId = 0, GroupName = "ALL" });
            var ddl = (DropDownList)sender;
            ddl.DataValueField = MembershipGroup.Columns.MembershipGroupId;
            ddl.DataTextField = MembershipGroup.Columns.GroupName;
            ddl.DataSource = data;
            ddl.DataBind();
        }

        protected void btnExportBlank_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataTable dt = MembershipImporterController.FetchMembership(-1);
            ExportData("MembershipImporter", dt);
        }

        protected void btnExportItemDept_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataTable dt = MembershipImporterController.FetchMembership(ddlDepartment.SelectedValue.GetIntValue());

            string groupName = ddlDepartment.SelectedItem.Text;

            if (dt.Rows.Count > 0)
                ExportData(string.Format("Membership_GroupName_{0}", groupName), dt);
            else
                lblStatus.Text = LanguageManager.GetTranslation("Data not found");
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";

            if (fuItemImporter.HasFile)
            {
                DataTable dtInput = new DataTable();
                dtInput.Columns.Add("Membership No", typeof(string)); // 0
                dtInput.Columns.Add("Group", typeof(string));//1
                dtInput.Columns.Add("Name To Appear", typeof(string));//2
                dtInput.Columns.Add("Chinese Name", typeof(string));//3
                dtInput.Columns.Add("First Name", typeof(string));//4
                dtInput.Columns.Add("Last Name", typeof(string));//5
                dtInput.Columns.Add("Christian Name", typeof(string));//6           
                dtInput.Columns.Add("Gender", typeof(string));//7
                dtInput.Columns.Add("Staff", typeof(string));//8
                dtInput.Columns.Add("Date Of Birth", typeof(string));//9
                dtInput.Columns.Add("Email", typeof(string));//10
                dtInput.Columns.Add("NRIC", typeof(string));//11
                dtInput.Columns.Add("Address1", typeof(string));//12
                dtInput.Columns.Add("Address2", typeof(string));//13
                dtInput.Columns.Add("Zip Code", typeof(string));//14
                dtInput.Columns.Add("City", typeof(string));//15         
                dtInput.Columns.Add("Country", typeof(string));//16
                dtInput.Columns.Add("Nationality", typeof(string));//17
                dtInput.Columns.Add("Mobile", typeof(string));//18
                dtInput.Columns.Add("Home", typeof(string));//19
                dtInput.Columns.Add("Occupation", typeof(string));//20
                dtInput.Columns.Add("Subscription Date", typeof(string));//21
                dtInput.Columns.Add("Expiry Date", typeof(string));//22
                dtInput.Columns.Add("Remarks", typeof(string));//23
                dtInput.Columns.Add("Deleted", typeof(string));//24

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
                        for (int i = iEndRowIndex; i >= iStartRowIndex; i--)
                        {
                            if (sl.HasCellValue(i, 1))
                            {
                                iEndRowDataIndex = i;
                                break;
                            }
                        }

                        for (int row = iStartRowIndex; row <= iEndRowDataIndex; ++row)
                        {
                            DataRow dr = dtInput.NewRow();
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
                                sl.GetCellValueAsDateTime(row, iStartColumnIndex + 9).ToString("yyyy-MM-dd HH:mm:ss") == "1900-01-01 00:00:00" ? "" : sl.GetCellValueAsDateTime(row, iStartColumnIndex + 9).ToString("dd-MMM-yyyy"),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 10),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 11),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 12),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 13),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 14),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 15),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 16),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 17),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 18),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 19),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 20),
                                sl.GetCellValueAsDateTime(row, iStartColumnIndex + 21).ToString("yyyy-MM-dd HH:mm:ss") == "1900-01-01 00:00:00" ? "" : sl.GetCellValueAsDateTime(row, iStartColumnIndex + 21).ToString("dd-MMM-yyyy"),
                                sl.GetCellValueAsDateTime(row, iStartColumnIndex + 22).ToString("yyyy-MM-dd HH:mm:ss") == "1900-01-01 00:00:00" ? "" : sl.GetCellValueAsDateTime(row, iStartColumnIndex + 22).ToString("dd-MMM-yyyy"),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 23),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 24));
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
                    bool isSuccess = MembershipImporterController.ImportData(Session["UserName"] + "", dtInput, out resultDt, out status);
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
