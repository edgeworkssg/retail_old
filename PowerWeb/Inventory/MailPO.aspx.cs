using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PowerPOS;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using PowerPOS.Container;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PowerWeb.Inventory
{
    public partial class MailPO : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string refNo = Request.QueryString["RefNo"] + "";
                bool isLoaded = false;
                ReportDocument rd = new ReportDocument();
                PurchaseOdrController ctrl = new PurchaseOdrController();
                if (!string.IsNullOrEmpty(refNo))
                    isLoaded = BindReport(refNo, out rd, out ctrl);
                if (!isLoaded)
                {
                    btnSend.Enabled = false;
                    CloseWindow("");
                }
            }
        }

        private void CloseWindow(string addScript)
        {
            string script = addScript+"window.close();";
            ClientScript.RegisterStartupScript(typeof(Page), "CloseWindow","<script type=\"text/javascript\">" + script + "</script>"); 
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMail.Text.Trim()))
                    throw new Exception(LanguageManager.GetTranslation("Mail destination cannot be empty"));

                if (string.IsNullOrEmpty(txtSubject.Text.Trim()))
                    throw new Exception(LanguageManager.GetTranslation("Subject cannot be empty"));
                
                string mailServer = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SMTP);
                int port = (AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Port) + "").GetIntValue();
                string mailSender = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail);
                string mailBbc = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.EdgeworksTeamEmail) + "";
                string userName = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Username);
                string password = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Password);

                var fromAddress = new MailAddress(mailSender);
                var listTo = txtMail.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var listCC = txtCC.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var listBcc = mailBbc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                string subject = txtSubject.Text;
                string body = txtMessage.Text;

                ReportDocument rd = new ReportDocument();
                PurchaseOdrController ctrl = new PurchaseOdrController();
                bool isLoaded = BindReport(Request.QueryString["RefNo"] + "", out rd, out ctrl);
                if (!isLoaded)
                {
                    throw new Exception(LanguageManager.GetTranslation("PO not loaded"));
                }
                else
                {
                    var smtp = new SmtpClient
                    {
                        Host = mailServer,
                        Port = port,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new NetworkCredential(userName, password),
                        Timeout = 20000
                    };
                    using (var message = new MailMessage())
                    {
                        message.Subject = subject;
                        message.Body = Regex.Replace(body, @"\r\n?|\n", "<br />");
                        message.From = fromAddress;
                        message.IsBodyHtml = true;
                        foreach (var item in listTo)
                        {
                            if (!string.IsNullOrEmpty(item))
                                message.To.Add(new MailAddress(item));
                        }
                        foreach (var item in listCC)
                        {
                            if (!string.IsNullOrEmpty(item))
                                message.CC.Add(new MailAddress(item));
                        }
                        foreach (var item in listBcc)
                        {
                            if (!string.IsNullOrEmpty(item))
                                message.Bcc.Add(new MailAddress(item));
                        }

                        var streamFile = rd.ExportToStream(ExportFormatType.PortableDocFormat);

                        message.Attachments.Add(new Attachment(streamFile, string.Format("PO_{0}.pdf", lblPORefNo.Text)));

                        smtp.Send(message);
                        ctrl.SetPOEmailStatusAndSave(true);
                        lblStatus.Text = LanguageManager.GetTranslation("Email Sent!");
                        CloseWindow("alert('" + LanguageManager.GetTranslation("Email Sent!") + "');"); 
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error : " + ex.Message;
                Logger.writeLog(ex);
            }
        }

        private bool BindReport(string refNo, out ReportDocument invSheet, out PurchaseOdrController invCtrl)
        {
            bool isFind = false;
            invSheet = new ReportDocument();
            invCtrl = new PurchaseOdrController();
            try
            {
                if (!string.IsNullOrEmpty(refNo))
                    isFind = invCtrl.LoadConfirmedPurchaseOrderController(refNo);
                if (!isFind)
                    return isFind;

                lblPORefNo.Text = invCtrl.GetCustomRefNo() ;
                btnSend.Enabled = isFind;
                Supplier supp = new Supplier(Supplier.Columns.SupplierID, invCtrl.InvHdr.Supplier);

                txtCC.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.POMailCC);
                txtMessage.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.POMailContent);
                txtSubject.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.POMailSubject);
                txtMail.Text = supp.ContactNo2;

                #region *) Load Fields
                PrintOutParameters1 printOutParameters = new PrintOutParameters1();
                printOutParameters.UserField1Label = "Purchase Order No";
                printOutParameters.UserField1Value = invCtrl.InvHdr.PurchaseOrderHdrRefNo;
                if (printOutParameters.UserField1Value == "") printOutParameters.UserField1Value = "-";
                printOutParameters.UserField2Label = "Supplier";
                printOutParameters.UserField2Value = supp.SupplierName;
                if (printOutParameters.UserField2Value == "") printOutParameters.UserField2Value = "-";
                printOutParameters.UserField3Label = "Freight Charges";
                printOutParameters.UserField3Value = invCtrl.InvHdr.FreightCharge.GetValueOrDefault(0).ToString("N2");
                if (printOutParameters.UserField3Value == "") printOutParameters.UserField3Value = "-";
                printOutParameters.UserField4Label = "Discount";
                printOutParameters.UserField4Value = invCtrl.InvHdr.Discount.GetValueOrDefault(0).ToString("N2");
                if (printOutParameters.UserField4Value == "") printOutParameters.UserField4Value = "-";
                printOutParameters.UserField5Label = "Exchange Rate";
                printOutParameters.UserField5Value = invCtrl.InvHdr.ExchangeRate.ToString();
                if (printOutParameters.UserField5Value == "") printOutParameters.UserField5Value = "-";

                try
                {
                    printOutParameters.UserField7Label = "Supplier Address";
                    printOutParameters.UserField7Value = string.IsNullOrEmpty(supp.CustomerAddress) ? "-" : supp.CustomerAddress;

                    printOutParameters.UserField8Label = "Telp Number";
                    printOutParameters.UserField8Value = string.IsNullOrEmpty(supp.ContactNo1) ? "-" : supp.ContactNo1;

                    printOutParameters.UserField9Label = "Fax Number";
                    printOutParameters.UserField9Value = string.IsNullOrEmpty(supp.ContactNo3) ? "-" : supp.ContactNo3;

                    printOutParameters.UserField10Label = "Delivery Time";
                    printOutParameters.UserField10Value = string.IsNullOrEmpty(invCtrl.getDeliveryTimeFormatted()) ? "-" : invCtrl.getDeliveryTimeFormatted();

                    printOutParameters.UserField11Label = "Delivery Address";
                    printOutParameters.UserField11Value = string.IsNullOrEmpty(invCtrl.InvHdr.Userfld4) ? "-" : invCtrl.InvHdr.Userfld4;

                    printOutParameters.UserField12Label = "Payment Term";
                    printOutParameters.UserField12Value = string.IsNullOrEmpty(invCtrl.InvHdr.Userfld5) ? "-" : invCtrl.InvHdr.Userfld5;

                    printOutParameters.UserField13Label = "Receiving Time";
                    printOutParameters.UserField13Value = string.IsNullOrEmpty(invCtrl.getReceivingTime()) ? "-" : invCtrl.getReceivingTime();

                    string displayName = invCtrl.InvHdr.UserName;
                    UserMst um = new UserMst(UserMst.Columns.UserName, invCtrl.InvHdr.UserName);
                    if (!um.IsNew && !string.IsNullOrEmpty(um.DisplayName))
                        displayName = um.DisplayName;

                    printOutParameters.UserField14Label = "UserName";
                    printOutParameters.UserField14Value = displayName;

                    printOutParameters.UserField15Label = "PO Role";
                    printOutParameters.UserField15Value = string.IsNullOrEmpty(invCtrl.InvHdr.Userfld8) ? "-" : invCtrl.InvHdr.Userfld8;

                    printOutParameters.UserField16Label = "PO Company";
                    printOutParameters.UserField16Value = string.IsNullOrEmpty(invCtrl.InvHdr.Userfld9) ? "-" : invCtrl.InvHdr.Userfld9;

                    printOutParameters.UserField17Label = "Approval Status";
                    printOutParameters.UserField17Value = string.IsNullOrEmpty(invCtrl.InvHdr.Userfld7) ? "Submitted" : invCtrl.InvHdr.Userfld7;

                    printOutParameters.UserField18Label = "GST";
                    printOutParameters.UserField18Value = string.IsNullOrEmpty(invCtrl.InvHdr.Userfld10) ? "0" : invCtrl.InvHdr.Userfld10;

                    printOutParameters.UserField19Label = "Min Purchase";
                    printOutParameters.UserField19Value = invCtrl.InvHdr.Userfloat1.HasValue ? invCtrl.InvHdr.Userfloat1.ToString() : "0";

                    printOutParameters.UserField20Label = "Delivery Charge";
                    printOutParameters.UserField20Value = invCtrl.InvHdr.Userfloat2.HasValue ? invCtrl.InvHdr.Userfloat2.ToString() : "0";

                    printOutParameters.SupplierAddress2 = string.IsNullOrEmpty(supp.ShipToAddress) ? "-" : supp.ShipToAddress;
                    printOutParameters.SupplierAddress3 = string.IsNullOrEmpty(supp.BillToAddress) ? "-" : supp.BillToAddress;
                    printOutParameters.ContactPerson1 = string.IsNullOrEmpty(supp.ContactPerson1) ? "-" : supp.ContactPerson1;
                    printOutParameters.ContactPerson2 = string.IsNullOrEmpty(supp.ContactPerson2) ? "-" : supp.ContactPerson2;
                    printOutParameters.ContactPerson3 = string.IsNullOrEmpty(supp.ContactPerson3) ? "-" : supp.ContactPerson3;
                    printOutParameters.SupplierCode = string.IsNullOrEmpty(supp.SupplierCode) ? "-" : supp.SupplierCode;
                    printOutParameters.SupplierEmail = string.IsNullOrEmpty(supp.ContactNo2) ? "-" : supp.ContactNo2;
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                #endregion

                string status;

                //Load the report
                DataTable dt;

                string FileLocation = Server.MapPath("~/Inventory/PO.rpt");
                invSheet.Load(FileLocation);

                dt = invCtrl.FetchUnSavedPurchaseOrderItemsWithDetailDeleted(out status);
                dt.Columns.Add("No");
                dt.Columns["No"].SetOrdinal(0);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["No"] = (i + 1).ToString();
                }
                //Balance Qty issue?

                invSheet.SetDataSource(dt);
                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + "PURCHASE ORDER" + "\"";
                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                //Basic Info
                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + (string.IsNullOrEmpty(invCtrl.GetCustomRefNo()) ? invCtrl.GetPurchaseOrderHdrRefNo() : invCtrl.GetCustomRefNo()) + "\"";
                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetPurchaseOrderDate().ToString("dd MMM yyyy") + "\"";
                invSheet.DataDefinition.FormulaFields["inventorylocation"].Text = "\"" + invCtrl.GetInventoryLocation() + "\"";
                invSheet.DataDefinition.FormulaFields["remark"].Text = "\"" + invCtrl.GetRemark().Replace("\r\n", "___").Replace("\n", "___").Replace("\"", "" + "\"\"" + "") + "\"";

                if (invCtrl.IsNew())
                {
                    invSheet.DataDefinition.FormulaFields["unsaved"].Text = "\"UNCONFIRMED\"";
                }

                //Custom fields
                invSheet.DataDefinition.FormulaFields["UserField1Label"].Text = "\"" + printOutParameters.UserField1Label + "\"";
                invSheet.DataDefinition.FormulaFields["UserField1Value"].Text = "\"" + printOutParameters.UserField1Value + "\"";

                invSheet.DataDefinition.FormulaFields["UserField2Label"].Text = "\"" + printOutParameters.UserField2Label + "\"";
                invSheet.DataDefinition.FormulaFields["UserField2Value"].Text = "\"" + printOutParameters.UserField2Value + "\"";

                invSheet.DataDefinition.FormulaFields["UserField3Label"].Text = "\"" + printOutParameters.UserField3Label + "\"";
                invSheet.DataDefinition.FormulaFields["UserField3Value"].Text = "\"" + printOutParameters.UserField3Value + "\"";

                invSheet.DataDefinition.FormulaFields["UserField4Label"].Text = "\"" + printOutParameters.UserField4Label + "\"";
                invSheet.DataDefinition.FormulaFields["UserField4Value"].Text = "\"" + printOutParameters.UserField4Value + "\"";

                invSheet.DataDefinition.FormulaFields["UserField5Label"].Text = "\"" + printOutParameters.UserField5Label + "\"";
                invSheet.DataDefinition.FormulaFields["UserField5Value"].Text = "\"" + printOutParameters.UserField5Value + "\"";

                invSheet.DataDefinition.FormulaFields["UserField6Label"].Text = "\"" + printOutParameters.UserField6Label + "\"";
                invSheet.DataDefinition.FormulaFields["UserField6Value"].Text = "\"" + printOutParameters.UserField6Value + "\"";

                try
                {

                    invSheet.DataDefinition.FormulaFields["UserField7Label"].Text = "\"" + printOutParameters.UserField7Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField7Value"].Text = "\"" + printOutParameters.UserField7Value + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField8Label"].Text = "\"" + printOutParameters.UserField8Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField8Value"].Text = "\"" + printOutParameters.UserField8Value + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField9Label"].Text = "\"" + printOutParameters.UserField9Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField9Value"].Text = "\"" + printOutParameters.UserField9Value + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField10Label"].Text = "\"" + printOutParameters.UserField10Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField10Value"].Text = "\"" + printOutParameters.UserField10Value + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField11Label"].Text = "\"" + printOutParameters.UserField11Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField11Value"].Text = "\"" + printOutParameters.UserField11Value.Replace("\r\n", "___").Replace("\n", "___").Replace("\"", "" + "\"\"" + "") + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField12Label"].Text = "\"" + printOutParameters.UserField12Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField12Value"].Text = "\"" + printOutParameters.UserField12Value.Replace("\r\n", "___").Replace("\n", "___").Replace("\"", "" + "\"\"" + "") + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField13Label"].Text = "\"" + printOutParameters.UserField13Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField13Value"].Text = "\"" + printOutParameters.UserField13Value + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField14Label"].Text = "\"" + printOutParameters.UserField14Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField14Value"].Text = "\"" + printOutParameters.UserField14Value + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField15Label"].Text = "\"" + printOutParameters.UserField15Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField15Value"].Text = "\"" + printOutParameters.UserField15Value + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField16Label"].Text = "\"" + printOutParameters.UserField16Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField16Value"].Text = "\"" + printOutParameters.UserField16Value + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField17Label"].Text = "\"" + printOutParameters.UserField17Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField17Value"].Text = "\"" + printOutParameters.UserField17Value + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField18Label"].Text = "\"" + printOutParameters.UserField18Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField18Value"].Text = "\"" + printOutParameters.UserField18Value + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField19Label"].Text = "\"" + printOutParameters.UserField19Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField19Value"].Text = "\"" + printOutParameters.UserField19Value + "\"";

                    invSheet.DataDefinition.FormulaFields["UserField20Label"].Text = "\"" + printOutParameters.UserField20Label + "\"";
                    invSheet.DataDefinition.FormulaFields["UserField20Value"].Text = "\"" + printOutParameters.UserField20Value + "\"";

                    invSheet.DataDefinition.FormulaFields["SupplierAddress2"].Text = "\"" + printOutParameters.SupplierAddress2 + "\"";
                    invSheet.DataDefinition.FormulaFields["SupplierAddress3"].Text = "\"" + printOutParameters.SupplierAddress3 + "\"";
                    invSheet.DataDefinition.FormulaFields["ContactPerson1"].Text = "\"" + printOutParameters.ContactPerson1 + "\"";
                    invSheet.DataDefinition.FormulaFields["ContactPerson2"].Text = "\"" + printOutParameters.ContactPerson2 + "\"";
                    invSheet.DataDefinition.FormulaFields["ContactPerson3"].Text = "\"" + printOutParameters.ContactPerson3 + "\"";

                    invSheet.DataDefinition.FormulaFields["SupplierCode"].Text = "\"" + printOutParameters.SupplierCode + "\"";
                    invSheet.DataDefinition.FormulaFields["SupplierEmail"].Text = "\"" + printOutParameters.SupplierEmail + "\"";
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                crReport.ReportSource = invSheet;
                crReport.RefreshReport();
                divReport.Visible = false;
            }
            catch (Exception exx)
            {
                Logger.writeLog(exx);
                isFind = false;
            }
            return isFind;
        }
    }
}
