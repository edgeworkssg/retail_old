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
    public partial class ItemImporter : PageBase
    {
        
        private string ApplicableTo
        {
            set
            {
                Session["Product_ApplicableTo"] = value;
            }
            get
            {
                return Session["Product_ApplicableTo"] + "";
            }
        }

        private bool SetUserOutletAssignment()
        {
            var outletList = OutletController.FetchByUserNameForReport(false, false, Session["UserName"] + "");
            var allOutlet = OutletController.FetchAll(false, false);
            bool isAssignedToAll = outletList.Count >= allOutlet.Count;
            ddlApplicableTo.Enabled = isAssignedToAll;
            int selectedIndex = ddlOutletList.SelectedIndex;
            string selectedValue = ddlOutletList.SelectedValue;
            if (!isAssignedToAll)
            {
                ddlApplicableTo.SelectedIndex = 2;
                ddlOutletList.Items.Clear();
                foreach (var ou in outletList)
                    ddlOutletList.Items.Add(ou.OutletName);
                ApplicableTo = "Outlet";
                if (ddlOutletList.Items.Count > 0)
                {
                    if (selectedIndex < 0)
                    {
                        ddlOutletList.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlOutletList.SelectedValue = selectedValue;
                    }

                }
            }
            else {
                ApplicableTo = "Product Master";
            }

            ddlApplicableTo.SelectedValue = ApplicableTo;
            return isAssignedToAll;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bool isEnableAppTo = SetUserOutletAssignment();

                //ddlApplicableTo.SelectedItem.Text = "Product Master";
                //ApplicableTo = ddlApplicableTo.SelectedValue;
                
                ddlOutletList.Enabled = !isEnableAppTo;
            }
        }

        private void ExportData(string fileName, DataTable dt)
        {
            bool displaySupplier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplaySupplier), true);

            fileName = fileName.Replace(" ", "_");
            SLDocument sl = new SLDocument();
            var style = sl.CreateStyle();
            style.FormatCode = "###";
            int iStartRowIndex = 1;
            int iStartColumnIndex = 2;
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

            if (dt.Columns[dt.Columns.Count - 1].ColumnName == "Status")
            {
                SLStyle styleWarp = sl.CreateStyle();
                styleWarp.SetWrapText(true);
                sl.SetColumnStyle(dt.Columns.Count, styleWarp);
            }

            if (!displaySupplier)
            {
                sl.HideColumn(31);
            }

            int iTypeStartRow = 1;
            sl.SetCellValue("A" + iTypeStartRow, "Product");
            sl.SetCellValue("A" + (iTypeStartRow + 1), "Open Price Product");
            sl.SetCellValue("A" + (iTypeStartRow + 2), "Service");
            sl.SetCellValue("A" + (iTypeStartRow + 3), "Point Package");
            sl.SetCellValue("A" + (iTypeStartRow + 4), "Course Package");
            sl.SetCellValue("A" + (iTypeStartRow + 4), "Non Inventory Product");

            SLDataValidation dv;
            dv = sl.CreateDataValidation(1, 17, 500, 17);
            dv.AllowList(string.Format("$A${0}:$A${1}", iTypeStartRow, (iTypeStartRow + 4)), true, true);
            sl.AddDataValidation(dv);

            int iYesNoStartRow = iTypeStartRow + 5;
            sl.SetCellValue("A" + iYesNoStartRow, "Exclusive");
            sl.SetCellValue("A" + (iYesNoStartRow + 1), "Inclusive");
            sl.SetCellValue("A" + (iYesNoStartRow + 2), "Non GST");

            //int iYesNoStartRow = iTypeStartRow + 5;
            //sl.SetCellValue("A" + iYesNoStartRow, "Exclusive");
            //sl.SetCellValue("A" + (iYesNoStartRow + 1), "Inclusive");
            //sl.SetCellValue("A" + (iYesNoStartRow + 2), "Non GST");

            dv = sl.CreateDataValidation(1, 12, 500, 12);
            dv.AllowList(string.Format("$A${0}:$A${1}", iYesNoStartRow, (iYesNoStartRow + 2)), true, true);
            sl.AddDataValidation(dv);

            int iFixCOGStartRow = iTypeStartRow + 8;
            sl.SetCellValue("A" + iFixCOGStartRow, "MARGIN_PERCENTAGE");
            sl.SetCellValue("A" + (iFixCOGStartRow + 1), "VALUE");

            dv = sl.CreateDataValidation(1, 34, 500, 34);
            dv.AllowList(string.Format("$A${0}:$A${1}", iFixCOGStartRow, (iFixCOGStartRow + 1)), true, true);
            sl.AddDataValidation(dv);

            sl.HideColumn(1);
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "_Result.xlsx");
            sl.SaveAs(Response.OutputStream);
            Response.End();
        }

        protected void ddlDepartment_Init(object sender, EventArgs e)
        {
            var data = new ItemDepartmentController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false && (o.DepartmentName+"").ToUpper() != "SYSTEM").OrderBy(o => o.DepartmentName).ToList();
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
            DataTable dt = ItemImporterController.FetchItem("XX__XX", "XX__XX");
            ExportData("ItemImporter", dt);
        }

        protected void ddlApplicableTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplicableTo = ddlApplicableTo.SelectedValue;
            ddlOutletList.Items.Clear();
            if (ddlApplicableTo.SelectedItem.Text == "Product Master")
            {
                ddlOutletList.Enabled = false;
            }
            else if (ddlApplicableTo.SelectedItem.Text == "Outlet Group")
            {
                Query qryctrlOutletGroup = OutletGroup.CreateQuery();
                qryctrlOutletGroup.OrderBy = SubSonic.OrderBy.Asc("OutletGroupID");
                Utility.LoadDropDown(ddlOutletList, qryctrlOutletGroup.ExecuteReader(), true);
                ddlOutletList.Items.Insert(0, new ListItem("(Not Specified)", String.Empty));
                ddlOutletList.Enabled = true;
            }
            else if (ddlApplicableTo.SelectedItem.Text == "Outlet")
            {
                OutletCollection outColl = new OutletCollection();
                outColl.Where(PowerPOS.Outlet.Columns.Deleted, false);
                outColl.OrderByAsc("OutletName");
                outColl.Load();
                foreach (Outlet ou in outColl)
                    ddlOutletList.Items.Add(new ListItem(ou.OutletName, ou.OutletName));
                ddlOutletList.Items.Insert(0, new ListItem("(Not Specified)", String.Empty));
                ddlOutletList.Enabled = true;
            }
        }

        protected void btnExportItemDept_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataTable dt = ItemImporterController.FetchItemWithOutletOption(ddlDepartment.SelectedValue, ddlCategory.SelectedValue, ApplicableTo, ddlOutletList.SelectedValue);

            string itemDept = ddlDepartment.SelectedItem.Text;
            string category = ddlCategory.SelectedItem.Text;


            if (dt.Rows.Count > 0)
                ExportData(string.Format("Item_ItemDept_{0}_Ctg_{1}", itemDept, category), dt);
            else
                lblStatus.Text = LanguageManager.GetTranslation("Data not found");
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";

            if ((ApplicableTo == "Outlet" || ApplicableTo == "Outlet Group") && ddlOutletList.SelectedValue == "")
            {
                lblStatus.Text = "Please select Outlet Group or Outlet Name";
                return;
            }

            if (fuItemImporter.HasFile)
            {
                DataTable dtInput = new DataTable();
                dtInput.Columns.Add("Department Code", typeof(string)); // 0
                dtInput.Columns.Add("Department Name", typeof(string));//1
                dtInput.Columns.Add("Category Code", typeof(string));//2
                dtInput.Columns.Add("Category Name", typeof(string));//3
                dtInput.Columns.Add("Item No", typeof(string));//4
                dtInput.Columns.Add("Item Name", typeof(string));//5
                dtInput.Columns.Add("Barcode", typeof(string));//6           
                dtInput.Columns.Add("Retail Price", typeof(string));//7
                dtInput.Columns.Add("Cost Price", typeof(string));//8
                dtInput.Columns.Add("UOM", typeof(string));//9
                dtInput.Columns.Add("GST Rule", typeof(string));//10
                dtInput.Columns.Add("Is Non Discountable", typeof(string));//11
                dtInput.Columns.Add("Point Redeemable", typeof(string));//12
                dtInput.Columns.Add("Give Commission", typeof(string));//13
                dtInput.Columns.Add("Allow Pre Order", typeof(string));//14
                dtInput.Columns.Add("Item Type", typeof(string));//15         
                dtInput.Columns.Add("Point Get", typeof(string));//16
                dtInput.Columns.Add("Breakdown Price", typeof(string));//17
                dtInput.Columns.Add("Description", typeof(string));//18
                dtInput.Columns.Add("Attributes1", typeof(string));//19
                dtInput.Columns.Add("Attributes2", typeof(string));//20
                dtInput.Columns.Add("Attributes3", typeof(string));//21
                dtInput.Columns.Add("Attributes4", typeof(string));//22
                dtInput.Columns.Add("Attributes5", typeof(string));//23
                dtInput.Columns.Add("Attributes6", typeof(string));//24
                dtInput.Columns.Add("Attributes7", typeof(string));//25
                dtInput.Columns.Add("Attributes8", typeof(string));//26
                dtInput.Columns.Add("Remark", typeof(string));//27
                dtInput.Columns.Add("Deleted", typeof(string));//28
                dtInput.Columns.Add("Supplier Name", typeof(string));//29
                dtInput.Columns.Add("Is Matrix Item", typeof(string));//30
                dtInput.Columns.Add("Is Using Fixed Value for COG", typeof(string));//31
                dtInput.Columns.Add("Fixed COG Type", typeof(string));//32
                dtInput.Columns.Add("Fix COG Value", typeof(string));//33
                if(AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayNonInventoryProduct), false))
                {
                    dtInput.Columns.Add("Deduction Item No", typeof(string));//34
                    dtInput.Columns.Add("Deduction Qty", typeof(string));//35
                    dtInput.Columns.Add("Deduction Type", typeof(string));//36
                }
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice1), true))
                {
                    dtInput.Columns.Add("Userfloat6", typeof(string));//37
                }
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice2), true))
                {
                    dtInput.Columns.Add("Userfloat7", typeof(string));//38
                }
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice3), true))
                {
                    dtInput.Columns.Add("Userfloat8", typeof(string));//39
                } 
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice4), true))
                {
                    dtInput.Columns.Add("Userfloat9", typeof(string));//40
                }
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice5), true))
                {
                    dtInput.Columns.Add("Userfloat10", typeof(string));//41
                }
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayMinimumSellingPrice), false))
                {
                    dtInput.Columns.Add("Minimum Price", typeof(string));//42
                }

                #region *) Read Excel

                try
                {
                    using (SLDocument sl = new SLDocument(fuItemImporter.PostedFile.InputStream, "Sheet1"))
                    {
                        SLWorksheetStatistics stats = sl.GetWorksheetStatistics();
                        int iStartColumnIndex = 2;
                        int iStartRowIndex =  stats.StartRowIndex + 1;
                        int iEndRowIndex =  stats.EndRowIndex;
                        int iEndRowDataIndex = iEndRowIndex;
                        for (int i = iEndRowIndex; i >= iStartRowIndex; i--)
                        {
                            if (sl.HasCellValue(i, 6))
                            {
                                iEndRowDataIndex = i;
                                break;
                            }
                        }


                        for (int row = iStartRowIndex; row <= iEndRowDataIndex; ++row)
                        {

                            DataRow dr = dtInput.NewRow();

                            dr["Department Code"] = sl.GetCellValueAsString(row, iStartColumnIndex);
                            dr["Department Name"] = sl.GetCellValueAsString(row, iStartColumnIndex + 1);
                            dr["Category Code"] = sl.GetCellValueAsString(row, iStartColumnIndex + 2);
                            dr["Category Name"] = sl.GetCellValueAsString(row, iStartColumnIndex + 3);
                            dr["Item No"] = sl.GetCellValueAsString(row, iStartColumnIndex + 4);
                            dr["Item Name"] = sl.GetCellValueAsString(row, iStartColumnIndex + 5);
                            dr["Barcode"] = sl.GetCellValueAsString(row, iStartColumnIndex + 6);
                            dr["Retail Price"] = sl.GetCellValueAsString(row, iStartColumnIndex + 7);
                            dr["Cost Price"] = sl.GetCellValueAsString(row, iStartColumnIndex + 8);
                            dr["UOM"] = sl.GetCellValueAsString(row, iStartColumnIndex + 9);
                            dr["GST Rule"] = sl.GetCellValueAsString(row, iStartColumnIndex + 10);
                            dr["Is Non Discountable"] = sl.GetCellValueAsString(row, iStartColumnIndex + 11);
                            dr["Point Redeemable"] = sl.GetCellValueAsString(row, iStartColumnIndex + 12);
                            dr["Give Commission"] = sl.GetCellValueAsString(row, iStartColumnIndex + 13);
                            dr["Allow Pre Order"] = sl.GetCellValueAsString(row, iStartColumnIndex + 14);
                            dr["Item Type"] = sl.GetCellValueAsString(row, iStartColumnIndex + 15);
                            dr["Point Get"] = sl.GetCellValueAsString(row, iStartColumnIndex + 16);
                            dr["Breakdown Price"] = sl.GetCellValueAsString(row, iStartColumnIndex + 17);
                            dr["Description"] = sl.GetCellValueAsString(row, iStartColumnIndex + 18);
                            dr["Attributes1"] = sl.GetCellValueAsString(row, iStartColumnIndex + 19);
                            dr["Attributes2"] = sl.GetCellValueAsString(row, iStartColumnIndex + 20);
                            dr["Attributes3"] = sl.GetCellValueAsString(row, iStartColumnIndex + 21);
                            dr["Attributes4"] = sl.GetCellValueAsString(row, iStartColumnIndex + 22);
                            dr["Attributes5"] = sl.GetCellValueAsString(row, iStartColumnIndex + 23);
                            dr["Attributes6"] = sl.GetCellValueAsString(row, iStartColumnIndex + 24);
                            dr["Attributes7"] = sl.GetCellValueAsString(row, iStartColumnIndex + 25);
                            dr["Attributes8"] = sl.GetCellValueAsString(row, iStartColumnIndex + 26);
                            dr["Remark"] = sl.GetCellValueAsString(row, iStartColumnIndex + 27);
                            dr["Deleted"] = sl.GetCellValueAsString(row, iStartColumnIndex + 28);
                            dr["Supplier Name"] = sl.GetCellValueAsString(row, iStartColumnIndex + 29);
                            dr["Is Matrix Item"] = sl.GetCellValueAsString(row, iStartColumnIndex + 30);
                            dr["Is Using Fixed Value for COG"] = sl.GetCellValueAsString(row, iStartColumnIndex + 31);
                            dr["Fixed COG Type"] = sl.GetCellValueAsString(row, iStartColumnIndex + 32);
                            dr["Fix COG Value"] = sl.GetCellValueAsString(row, iStartColumnIndex + 33);
                            var lastIndex = 33;
                            if (dtInput.Columns.Contains("Deduction Item No"))
                            {
                                //dr["Deduction Item No"] = sl.GetCellValueAsString(row, iStartColumnIndex + 34);
                                dr["Deduction Item No"] = sl.GetCellValueAsString(row, iStartColumnIndex + lastIndex+1);
                                lastIndex = lastIndex + 1;
                            }
                            if (dtInput.Columns.Contains("Deduction Qty"))
                            {
                                //dr["Deduction Qty"] = sl.GetCellValueAsString(row, iStartColumnIndex + 35);
                                dr["Deduction Qty"] = sl.GetCellValueAsString(row, iStartColumnIndex + lastIndex + 1);
                                lastIndex = lastIndex + 1;
                            }
                            if (dtInput.Columns.Contains("Deduction Type"))
                            {
                                //dr["Deduction Type"] = sl.GetCellValueAsString(row, iStartColumnIndex + 36);
                                dr["Deduction Type"] = sl.GetCellValueAsString(row, iStartColumnIndex + lastIndex + 1);
                                lastIndex = lastIndex + 1;
                            }
                            if (dtInput.Columns.Contains("Userfloat6"))
                            {
                                //dr["Userfloat6"] = sl.GetCellValueAsString(row, iStartColumnIndex + 37);
                                dr["Userfloat6"] = sl.GetCellValueAsString(row, iStartColumnIndex + lastIndex + 1);
                                lastIndex = lastIndex + 1;
                            }
                            if (dtInput.Columns.Contains("Userfloat7"))
                            {
                                //dr["Userfloat7"] = sl.GetCellValueAsString(row, iStartColumnIndex + 38);
                                dr["Userfloat7"] = sl.GetCellValueAsString(row, iStartColumnIndex + lastIndex + 1);
                                lastIndex = lastIndex + 1;
                            }
                            if (dtInput.Columns.Contains("Userfloat8"))
                            {
                                //dr["Userfloat8"] = sl.GetCellValueAsString(row, iStartColumnIndex + 39);
                                dr["Userfloat8"] = sl.GetCellValueAsString(row, iStartColumnIndex + lastIndex + 1);
                                lastIndex = lastIndex + 1;
                            }
                            if (dtInput.Columns.Contains("Userfloat9"))
                            {
                                //dr["Userfloat9"] = sl.GetCellValueAsString(row, iStartColumnIndex + 40);
                                dr["Userfloat9"] = sl.GetCellValueAsString(row, iStartColumnIndex + lastIndex + 1);
                                lastIndex = lastIndex + 1;
                            }
                            if (dtInput.Columns.Contains("Userfloat10"))
                            {
                                //dr["Userfloat10"] = sl.GetCellValueAsString(row, iStartColumnIndex + 41);
                                dr["Userfloat10"] = sl.GetCellValueAsString(row, iStartColumnIndex + lastIndex + 1);
                                lastIndex = lastIndex + 1;
                            }
                            if (dtInput.Columns.Contains("Minimum Price"))
                            {
                                //dr["Userfloat10"] = sl.GetCellValueAsString(row, iStartColumnIndex + 41);
                                dr["Minimum Price"] = sl.GetCellValueAsString(row, iStartColumnIndex + lastIndex + 1);
                                lastIndex = lastIndex + 1;
                            }


                            dtInput.Rows.Add(dr);
                            //dtInput.Rows.Add(
                            //    sl.GetCellValueAsString(row, iStartColumnIndex),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 1),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 2),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 3),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 4),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 5),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 6),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 7),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 8),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 9),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 10),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 11),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 12),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 13),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 14),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 15),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 16),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 17),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 18),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 19),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 20),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 21),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 22),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 23),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 24),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 25),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 26),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 27),                                
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 28),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 29),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 30),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 31),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 32),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 33),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 34),
                            //    sl.GetCellValueAsString(row, iStartColumnIndex + 35));

                            
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
                    bool isSuccess = ItemImporterController.ImportDataWithOutletOption(Session["UserName"] + "", ApplicableTo, ddlOutletList.SelectedValue,
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
