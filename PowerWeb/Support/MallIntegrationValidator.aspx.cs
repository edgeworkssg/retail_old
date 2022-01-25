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
using System.Web.Script.Serialization;
using PowerPOS.Container;
using SubSonic;
using System.Collections.Generic;
using System.Drawing;

namespace PowerWeb.Support
{
    public partial class MallIntegrationValidator : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetFormSetting();
            if (!this.IsPostBack)
            {
                ddlOutlet_SelectedIndexChanged(ddlOutlet, new EventArgs());
            }
        }

        private void SetFormSetting()
        {
            try
            {
                lblStatus.Text = "";
                lblSuccessStatus.Text = "";
                string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                lblPOS.Text = posText;
                lblOutlet.Text = outletText;
                lblPOSCode.Text = posText + " "+LanguageManager.GetTranslation("Code");
                lblOutletCode.Text = outletText + " " + LanguageManager.GetTranslation("Code");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void btnValidate_Click(object sender, EventArgs e)
        {

            lblStatus.Text = "";
            lblSuccessStatus.Text = "";
            PointOfSale pos = new PointOfSale(PointOfSale.Columns.PointOfSaleID, ddlTenant.SelectedValue);

            if ((ddlTenant.SelectedValue+"").GetIntValue() == 0)
            {
                lblStatus.Text = LanguageManager.GetTranslation("Please select")+" " + LanguageManager.GetTranslation(LabelController.PointOfSaleText);
                ddlOutlet.SelectedValue = pos.OutletName;
                ddlOutlet_SelectedIndexChanged(ddlOutlet, new EventArgs());
                ddlTenant.SelectedValue = pos.PointOfSaleID + "";
                ddlTenant_SelectedIndexChanged(ddlTenant, new EventArgs());
                return;
            }

            if (fufileValidator.HasFile)
            {
                if (!fufileValidator.PostedFile.FileName.ToLower().EndsWith(".csv"))
                {
                    lblStatus.Text = LanguageManager.GetTranslation("Please provide file with correct format. Please use csv File.");
                    AccessLogController.AddTenantHistory(pos.TenantMachineID, pos.PointOfSaleID, AccessSource.WEB, (Session["UserName"] + ""), "Interface File Submission Failed. Error : Incorrect file format");
                }
                else
                {
                    try
                    {

                        DataTable message;
                        var data = new ArrayList();
                        string path = Server.MapPath("~/");
                        string path1 = path + "Uploads\\" + fufileValidator.FileName;
                        if (File.Exists(path1))
                            File.Delete(path1);
                        fufileValidator.SaveAs(path1);

                        string tenantCode = "";
                        if (!ExcelController.ImportExcelCSVWithDelimiter(';', path1, out message, false))
                            throw new Exception(LanguageManager.GetTranslation("Invalid CSV Format."));

                        if (message.Columns.Count != 8)
                            throw new Exception(LanguageManager.GetTranslation("File Format is wrong. number of columns is not match with the format specification."));

                        foreach (DataRow dr in message.Rows)
                        {
                            tenantCode = dr[1].ToString();
                            var tmpRow = new
                            {
                                MallCode = dr[0].ToString(),
                                TenantCode = dr[1].ToString(),
                                Date = dr[2].ToString(),
                                Hour = dr[3].ToString(),
                                TransactionCount = dr[4].ToString(),
                                TotalSalesAfterTax = dr[5].ToString(),
                                TotalSalesBeforeTax = dr[6].ToString(),
                                TotalTax = dr[7].ToString()
                            };
                            data.Add(tmpRow);
                        }
                        //string tenantCodeToCheck = "test";
                        //UserMst usm = new UserMst(Session["UserName"]);
                        //if (usm.Userfld5 == null || usm.Userfld5 == "")
                        //{

                        //}
                        //else
                        //{
                        //    int posID;
                        //    if (int.TryParse(usm.Userfld5, out posID))
                        //    {
                        //        string sqlString = "Select TenantMachineID From PointOfSale where PointOfSaleID = " + posID;
                        //        object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                        //        if (tmp != null && tmp.ToString() != "" && tmp.ToString() != "ALL")
                        //        {
                        //            tenantCodeToCheck = tmp.ToString();
                        //        }
                        //    }
                        //}

                        string status = "";
                        if (!pos.TenantMachineID.ToLower().Equals(tenantCode.ToLower()))
                        {
                            status = string.Format(LanguageManager.GetTranslation("Tenant Code Mismatch. Tenant Code :") + "{0}, " + LanguageManager.GetTranslation("Uploaded Tenant Code :") + "{1}", pos.TenantMachineID, tenantCode);
                            AccessLogController.AddTenantHistory(pos.TenantMachineID, pos.PointOfSaleID, AccessSource.WEB, (Session["UserName"] + ""), LanguageManager.GetTranslation("Interface File Submission Failed. Error :") + status);
                            lblStatus.Text = status;
                        }
                        else
                        {
                            bool result = MallIntegrationProviderController.validateFile(tenantCode, new JavaScriptSerializer().Serialize(data), out status);
                            if (!result)
                            {
                                lblStatus.Text = status;
                                AccessLogController.AddTenantHistory(pos.TenantMachineID, pos.PointOfSaleID, AccessSource.WEB, (Session["UserName"] + ""), LanguageManager.GetTranslation("Interface File Submission Failed. Error :") + status);
                            }
                            else
                            {
                                //string statusText = "";
                                if (data.Count > 0)
                                {
                                    if (!SendEmail(tenantCode, fufileValidator.PostedFile.InputStream, fufileValidator.PostedFile.FileName))
                                    {

                                    }
                                }
                                pos.InterfaceValidationStatus = "Passed";
                                pos.Save(Session["UserName"] + "");
                                lblSuccessStatus.Text = LanguageManager.GetTranslation("We have successfully validated your Interface File Submission. Email confirmation will be sent to you shortly. Thank you.");
                                AccessLogController.AddTenantHistory(pos.TenantMachineID, pos.PointOfSaleID, AccessSource.WEB, (Session["UserName"] + ""), "Interface File Submission Success.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AccessLogController.AddTenantHistory(pos.TenantMachineID, pos.PointOfSaleID, AccessSource.WEB, (Session["UserName"] + ""), "Interface File Submission Failed. Error :" + ex.Message);
                        lblStatus.Text = ex.Message;
                        Logger.writeLog(ex);
                    }
                }
            }
            else
            {
                AccessLogController.AddTenantHistory(pos.TenantMachineID, pos.PointOfSaleID, AccessSource.WEB, (Session["UserName"] + ""), "Interface File Submission Failed. Error : File cannot uploaded");
            }
            ddlOutlet.SelectedValue = pos.OutletName;
            ddlOutlet_SelectedIndexChanged(ddlOutlet, new EventArgs());
            ddlTenant.SelectedValue = pos.PointOfSaleID + "";
            ddlTenant_SelectedIndexChanged(ddlTenant, new EventArgs());
        }

        public bool SendEmail(string tenantCode, Stream fileToAttach, string filename)
        {
            try
            {
                bool isSuccess = true;
                string mailTo = "";
                string sqlString = "Select PointOfSaleName, OutletName, RetailerName, RetailerLevel, ShopNo, RetailerContactPerson, VendorContactName, RetailerEmail, VendorEmail from PointOfSale where TenantMachineID = '" + tenantCode + "'";
                DataSet ds = DataService.GetDataSet(new QueryCommand(sqlString));
                ArrayList emails = new ArrayList();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["RetailerEmail"] != null && ds.Tables[0].Rows[0]["RetailerEmail"].ToString() != "")
                    {
                        emails.Add(ds.Tables[0].Rows[0]["RetailerEmail"].ToString());
                        
                    }
                    if (ds.Tables[0].Rows[0]["VendorEmail"] != null && ds.Tables[0].Rows[0]["VendorEmail"].ToString() != "")
                    {
                        emails.Add(ds.Tables[0].Rows[0]["VendorEmail"].ToString());
                    }
                }

                if (emails.Count > 0)
                {
                    
                    var attachment = new Dictionary<string, Stream>();
                    attachment.Add(filename, fileToAttach);
                    string subject = ds.Tables[0].Rows[0]["PointOfSaleName"].ToString() + " ";
                    subject += ds.Tables[0].Rows[0]["ShopNo"].ToString() + " at ";
                    subject += ds.Tables[0].Rows[0]["OutletName"].ToString();
                    /*if (ds.Tables[0].Rows[0]["RetailerLevel"] != null && ds.Tables[0].Rows[0]["RetailerLevel"].ToString() != "")
                    {
                        subject += " lvl " + ds.Tables[0].Rows[0]["RetailerLevel"];
                    }*/
                    subject += " - "+LanguageManager.GetTranslation("Successful Interface File Submission Notice");

                    string bodyMail = "<html><body><p>";
                    bodyMail += LanguageManager.GetTranslation("Dear all,");
                    bodyMail += "<BR/>";
                    bodyMail += "<BR/>";
                    bodyMail += LanguageManager.GetTranslation("Your Interface File Submission has been validated successfully.");
                    bodyMail += "<BR/>";
                    bodyMail += LanguageManager.GetTranslation("You may proceed to submit your hourly sales data.");
                    bodyMail += "<BR/>";
                    bodyMail += "<BR/>";
                    bodyMail += LanguageManager.GetTranslation("Please ensure the data to reach us by 9AM next business day.");
                    bodyMail += "<BR/>";
                    bodyMail += LanguageManager.GetTranslation("Thank you.");
                    bodyMail += "<BR/>";
                    bodyMail += "<BR/>";
                    bodyMail += LanguageManager.GetTranslation("--- This file is computer generated. Please do not reply. ---");
                    bodyMail += "</p></body></html>";
                    string mailCC = AppSetting.GetSetting("MallIntegration_MallManagementTeamEmail");
                    string mailBcc = AppSetting.GetSetting("MallIntegration_EdgeworksTeamEmail"); 
                    var ms = new MassEmail();
                    string status = "";
                    Logger.writeLog("Send Email!!!!");
                    var result = ms.SendEmailsWithCC(
                        emails,
                        AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail),
                        subject,
                        bodyMail,
                        bodyMail,
                        AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SMTP),
                        AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Username),
                        AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Password),
                        false,
                        attachment, AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Port), 
                        mailBcc, mailCC, 
                        out status);
                isSuccess = result!=null && result.Count == 0;
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Sending Email" + ex.Message);
                return false;
            }
        }

        protected void ddlOutlet_Init(object sender, EventArgs e)
        {
            try
            {
                var data = OutletController.FetchByUserNameForReport(false, false, (Session["UserName"] + ""));
                if (data.Count > 1)
                    data.Insert(0, new Outlet { OutletName = "" });
                ddlOutlet.DataSource = data;// OutletController.FetchByUserNameForReport(false, false, (Session["UserName"] + ""));
                ddlOutlet.DataBind();
                ddlOutlet.SelectedIndex = 0;
                ddlOutlet_SelectedIndexChanged(ddlOutlet, new EventArgs());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void ddlOutlet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string outletText = LabelController.OutletText;
                Outlet outlet = new Outlet(Outlet.Columns.OutletName, ddlOutlet.SelectedValue);
                lblOutletCode.Text = string.Format("{0} Code : {1}", outletText, outlet.MallCode);

                var data = PointOfSaleController.FetchByUserNameForReport(false, false, (Session["UserName"] + ""), ddlOutlet.SelectedValue);
                if (data.Count > 1)
                    data.Insert(0, new PointOfSale { PointOfSaleID = 0, PointOfSaleName = "" });
                ddlTenant.DataSource = data; // PointOfSaleController.FetchByUserNameForReport(false, false, (Session["UserName"] + ""), ddlOutlet.SelectedValue);
                ddlTenant.DataBind();
                ddlTenant.SelectedIndex = 0;
                ddlTenant_SelectedIndexChanged(ddlTenant, new EventArgs());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void ddlTenant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                string posText = LabelController.PointOfSaleText;
                PointOfSale pos = new PointOfSale(PointOfSale.Columns.PointOfSaleID, ddlTenant.SelectedValue);
                lblPOSCode.Text = string.Format("{0} Code : {1}", posText, pos.TenantMachineID);
                lblValidationStatus.Text = pos.InterfaceValidationStatus;

                string outletText = LabelController.OutletText;
                Outlet outlet = pos.Outlet;
                lblOutletCode.Text = string.Format("{0} Code : {1}", outletText, outlet.MallCode);
                ddlOutlet.SelectedValue = outlet.OutletName;

                if (lblValidationStatus.Text.ToLower().Equals("passed"))
                    lblValidationStatus.ForeColor = Color.Green;
                else if (lblValidationStatus.Text.ToLower().Equals("failed"))
                    lblValidationStatus.ForeColor = Color.Red;
                else
                    lblValidationStatus.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            string fileName = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.InterfaceFileName);
            string fileContent = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.InterfaceFileSource);
            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(fileContent) && File.Exists(fileContent))
            {
                try
                {
                    try
                    {
                        PointOfSale pos = new PointOfSale(PointOfSale.Columns.PointOfSaleID, ddlTenant.SelectedValue);
                        AccessLogController.AddTenantHistory(pos.TenantMachineID, pos.PointOfSaleID, AccessSource.WEB, Session["UserName"] + "", "Download specification file : " + fileName);
                    }
                    catch (Exception exx)
                    {
                        Logger.writeLog(exx);
                    }

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", fileName));
                    Response.Buffer = true;
                    Response.Clear();
                    Response.BinaryWrite(File.ReadAllBytes(fileContent));
                    Response.End();

                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    lblStatus.Text = LanguageManager.GetTranslation("Error When Downloading File :") + ex.Message;
                }
            }
            else
            {
                lblStatus.Text = LanguageManager.GetTranslation("File not exist!"); 
            }
        }
    }
}
