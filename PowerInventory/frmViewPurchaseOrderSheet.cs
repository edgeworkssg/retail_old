using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace PowerInventory
{    
    public partial class frmViewPurchaseOrderSheet : Form
    {
        public bool showOnHand;
        public bool showCostPrice;
        public PurchaseOdrController invCtrl;
        public string StockActivity;
        public PrintOutParameters1 printOutParameters;
        public frmViewPurchaseOrderSheet()
        {
            showOnHand = false;
            showCostPrice = false;
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            string status;
            
            //Load the report
            //DataTable dt = invCtrl.FetchUnSavedInventoryItems(showOnHand, showCostPrice, out status);
            DataTable dt;

            string FileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.PurchaseOrderFileLocation);
            bool ReportLoaded = false;
            if (FileLocation != null && FileLocation.ToLower().EndsWith(".rpt") && File.Exists(FileLocation))
            {
                try
                {
                    //Inst = GetInvoice(pos, ReceiptFileLocation, reprint);
                    ReportLoaded = true;
                }
                catch (Exception X)
                {
                    CommonUILib.HandleException(X);
                }
            }
            if (!ReportLoaded)
            {
                PurchaseOrderSheet invSheet = new PurchaseOrderSheet();

                bool showPacking = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false);
                bool showUOM = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowUOM), false);
                bool showCurrency = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false);
                dt = invCtrl.FetchUnSavedPurchaseOrderItemsWithDetailDeleted(out status);
                dt.Columns.Add("No");
                dt.Columns["No"].SetOrdinal(0);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["No"] = (i + 1).ToString();
                }
                //Balance Qty issue?

                invSheet.SetDataSource(dt);
                if (invSheet.DataDefinition.FormulaFields["StockActivity"] != null)
                    invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + "PURCHASE ORDER" + "\"";

                if (invSheet.DataDefinition.FormulaFields["CompanyName"] != null)
                    invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                //Basic Info
                if (invSheet.DataDefinition.FormulaFields["refno"] != null)
                    invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + (string.IsNullOrEmpty(invCtrl.GetCustomRefNo()) ? invCtrl.GetPurchaseOrderHdrRefNo() : invCtrl.GetCustomRefNo()) + "\"";

                if (invSheet.DataDefinition.FormulaFields["inventorydate"] != null)
                    invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetPurchaseOrderDate().ToString("dd MMM yyyy") + "\"";

                if (invSheet.DataDefinition.FormulaFields["inventorylocation"] != null)
                    invSheet.DataDefinition.FormulaFields["inventorylocation"].Text = "\"" + invCtrl.GetInventoryLocation() + "\"";

                if (invSheet.DataDefinition.FormulaFields["remark"] != null)
                    invSheet.DataDefinition.FormulaFields["remark"].Text = "\"" + invCtrl.GetRemark().Replace("\r\n", "___").Replace("\n", "___").Replace("\"", "" + "\"\"" + "") + "\"";

                if (invCtrl.IsNew())
                {
                    if (invSheet.DataDefinition.FormulaFields["unsaved"] != null)
                        invSheet.DataDefinition.FormulaFields["unsaved"].Text = "\"UNCONFIRMED\"";
                }

                try
                {
                    //Custom fields
                    for (int i = 0; i < invSheet.DataDefinition.FormulaFields.Count; i++)
                    {
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField1Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField1Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField1Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField1Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField2Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField2Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField2Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField2Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField3Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField3Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField3Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField3Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField4Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField4Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField4Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField4Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField5Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField5Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField5Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField5Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField6Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField6Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField6Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField6Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField7Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField7Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField7Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField7Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField8Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField8Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField8Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField8Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField9Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField9Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField9Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField9Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField10Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField10Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField10Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField10Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField11Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField11Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField11Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField11Value.Replace("\r\n", "___") + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField12Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField12Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField12Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField12Value.Replace("\r\n", "___") + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField13Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField13Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField13Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField13Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField14Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField14Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField14Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField14Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField15Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField15Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField15Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField15Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField16Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField16Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField16Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField16Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField17Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField17Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField17Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField17Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField18Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField18Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField18Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField18Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField19Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField19Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField19Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField19Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField20Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField20Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField20Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField20Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierAddress2")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.SupplierAddress2 + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierAddress3")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.SupplierAddress3 + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "ContactPerson1")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.ContactPerson1 + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "ContactPerson2")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.ContactPerson2 + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "ContactPerson3")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.ContactPerson3 + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierCode")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.SupplierCode + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierEmail")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.SupplierEmail + "\"";
                        //invSheet.DataDefinition.FormulaFields["Remark"].Text = "\"" + printOutParameters.Remark + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "PrintedBy")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.PrintedBy + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "PurchaseOrderDate")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.PurchaseOrderDate + "\"";
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }

                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;

                /*if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.Print_SaveReceiptAsDocument), false))
                {
                    string docPath = AppSetting.GetSetting(AppSetting.SettingsName.Print.PDFPath);
                    CrystalDecisions.Shared.ExportFormatType type = new CrystalDecisions.Shared.ExportFormatType();
                    type = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                    invSheet.ExportToDisk(type, docPath + "PurchaseOrder\\" + invCtrl.GetPurchaseOrderHdrRefNo() + ".pdf");
                }*/
                //invSheet.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "C:\\Retail\\Doc\\PurchaseOrder\\" + invCtrl.GetPurchaseOrderHdrRefNo() + ".pdf");
                crystalReportViewer1.ReportSource = invSheet;
                crystalReportViewer1.RefreshReport();
            }
            else
            {
                ReportDocument invSheet = new ReportDocument();
                invSheet.Load(FileLocation);

                bool showPacking = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false);
                bool showUOM = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowUOM), false);
                bool showCurrency = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false);
                dt = invCtrl.FetchUnSavedPurchaseOrderItemsWithDetailDeleted(out status);
                dt.Columns.Add("No");
                dt.Columns["No"].SetOrdinal(0);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["No"] = (i + 1).ToString();
                }
                //Balance Qty issue?

                invSheet.SetDataSource(dt);
                if (invSheet.DataDefinition.FormulaFields["StockActivity"] != null)
                    invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + "PURCHASE ORDER" + "\"";

                if (invSheet.DataDefinition.FormulaFields["CompanyName"] != null)
                    invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                //Basic Info
                if (invSheet.DataDefinition.FormulaFields["refno"] != null)
                    invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + (string.IsNullOrEmpty(invCtrl.GetCustomRefNo()) ? invCtrl.GetPurchaseOrderHdrRefNo() : invCtrl.GetCustomRefNo()) + "\"";

                if (invSheet.DataDefinition.FormulaFields["inventorydate"] != null)
                    invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetPurchaseOrderDate().ToString("dd MMM yyyy") + "\"";

                if (invSheet.DataDefinition.FormulaFields["inventorylocation"] != null)
                    invSheet.DataDefinition.FormulaFields["inventorylocation"].Text = "\"" + invCtrl.GetInventoryLocation() + "\"";

                if (invSheet.DataDefinition.FormulaFields["remark"] != null)
                    invSheet.DataDefinition.FormulaFields["remark"].Text = "\"" + invCtrl.GetRemark().Replace("\r\n", "___").Replace("\n", "___").Replace("\"", "" + "\"\"" + "") + "\"";

                if (invCtrl.IsNew())
                {
                    if (invSheet.DataDefinition.FormulaFields["unsaved"] != null)
                        invSheet.DataDefinition.FormulaFields["unsaved"].Text = "\"UNCONFIRMED\"";
                }

                try
                {
                    //Custom fields
                    for (int i = 0; i < invSheet.DataDefinition.FormulaFields.Count; i++)
                    {
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField1Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField1Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField1Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField1Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField2Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField2Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField2Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField2Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField3Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField3Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField3Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField3Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField4Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField4Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField4Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField4Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField5Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField5Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField5Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField5Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField6Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField6Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField6Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField6Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField7Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField7Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField7Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField7Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField8Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField8Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField8Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField8Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField9Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField9Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField9Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField9Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField10Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField10Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField10Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField10Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField11Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField11Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField11Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField11Value.Replace("\r\n", "___") + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField12Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField12Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField12Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField12Value.Replace("\r\n", "___") + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField13Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField13Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField13Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField13Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField14Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField14Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField14Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField14Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField15Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField15Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField15Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField15Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField16Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField16Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField16Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField16Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField17Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField17Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField17Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField17Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField18Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField18Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField18Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField18Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField19Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField19Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField19Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField19Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField20Label")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField20Label + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "UserField20Value")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.UserField20Value + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierAddress2")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.SupplierAddress2 + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierAddress3")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.SupplierAddress3 + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "ContactPerson1")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.ContactPerson1 + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "ContactPerson2")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.ContactPerson2 + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "ContactPerson3")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.ContactPerson3 + "\"";

                        if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierCode")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.SupplierCode + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierEmail")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.SupplierEmail + "\"";
                        //invSheet.DataDefinition.FormulaFields["Remark"].Text = "\"" + printOutParameters.Remark + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "PrintedBy")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.PrintedBy + "\"";
                        if (invSheet.DataDefinition.FormulaFields[i].Name == "PurchaseOrderDate")
                            invSheet.DataDefinition.FormulaFields[i].Text = "\"" + printOutParameters.PurchaseOrderDate + "\"";
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }

                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;

                /*if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.Print_SaveReceiptAsDocument), false))
                {
                    string docPath = AppSetting.GetSetting(AppSetting.SettingsName.Print.PDFPath);
                    CrystalDecisions.Shared.ExportFormatType type = new CrystalDecisions.Shared.ExportFormatType();
                    type = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                    invSheet.ExportToDisk(type, docPath + "PurchaseOrder\\" + invCtrl.GetPurchaseOrderHdrRefNo() + ".pdf");
                }*/
                //invSheet.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "C:\\Retail\\Doc\\PurchaseOrder\\" + invCtrl.GetPurchaseOrderHdrRefNo() + ".pdf");
                crystalReportViewer1.ReportSource = invSheet;
                crystalReportViewer1.RefreshReport();
            }
        }
    }
    public class PrintOutParameters1
    {
        public string UserField1Label;
        public string UserField1Value;
        public string UserField2Label;
        public string UserField2Value;
        public string UserField3Label;
        public string UserField3Value;
        public string UserField4Label;
        public string UserField4Value;
        public string UserField5Label;
        public string UserField5Value;
        public string UserField6Label;
        public string UserField6Value;

        public string UserField7Label;
        public string UserField7Value;
        public string UserField8Label;
        public string UserField8Value;
        public string UserField9Label;
        public string UserField9Value;
        public string UserField10Label;
        public string UserField10Value;
        public string UserField11Label;
        public string UserField11Value;
        public string UserField12Label;
        public string UserField12Value;
        public string UserField13Label;
        public string UserField13Value;
        public string UserField14Label;
        public string UserField14Value;
        public string UserField15Label;
        public string UserField15Value;
        public string UserField16Label;
        public string UserField16Value;
        public string UserField17Label;
        public string UserField17Value;
        public string UserField18Label;
        public string UserField18Value;
        public string UserField19Label;
        public string UserField19Value;
        public string UserField20Label;
        public string UserField20Value;

        public string SupplierAddress2;
        public string SupplierAddress3;
        public string ContactPerson1;
        public string ContactPerson2;
        public string ContactPerson3;
        public string SupplierCode;
        public string SupplierEmail;
        public string PrintedBy;
        public string PurchaseOrderDate;

    }
}
