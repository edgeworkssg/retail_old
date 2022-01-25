using System;
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
    public partial class frmViewInventorySheet : Form
    {
        public bool showOnHand;
        public bool showCostPrice;
        public bool showAlternateCostPrice;
        public InventoryController invCtrl;
        public string StockActivity;
        public PrintOutParameters printOutParameters;
        public frmViewInventorySheet()
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
            if (StockActivity == "GOODS RECEIVE")
            {
                string rptLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.GoodsReceiveReportFileLocation);
                bool ReportLoaded = false;
                if (rptLocation != null && rptLocation.ToLower().EndsWith(".rpt") && File.Exists(rptLocation))
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
                    if (showOnHand)
                    {
                        var invSheet = new InventorySheetGR();

                        dt = invCtrl.FetchUnSavedInventoryItems(showOnHand, showCostPrice, out status);
                        dt.Columns.Add("No");
                        dt.Columns["No"].SetOrdinal(0);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["No"] = (i + 1).ToString();
                        }
                        //Balance Qty issue?

                        invSheet.SetDataSource(dt);
                        invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                        invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                        //Basic Info
                        invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + (string.IsNullOrEmpty(invCtrl.GetCustomRefNo()) ? invCtrl.GetInvHdrRefNo() : invCtrl.GetCustomRefNo()) + "\"";
                        invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
                        invSheet.DataDefinition.FormulaFields["inventorylocation"].Text = "\"" + invCtrl.GetInventoryLocation() + "\"";
                        invSheet.DataDefinition.FormulaFields["remark"].Text = "\"" + invCtrl.GetRemark().Replace("\r\n", "___").Replace("\n", "___").Replace("\"", "" + "\"\"" + "") + "\"";

                        if (invCtrl.IsNew())
                        {
                            invSheet.DataDefinition.FormulaFields["unsaved"].Text = "\"UNCONFIRMED\"";
                        }

                        if (!string.IsNullOrEmpty(invCtrl.InvHdr.Supplier))
                        {
                            Supplier sup = new Supplier(invCtrl.InvHdr.Supplier);
                            if (sup != null && sup.SupplierID.ToString() == invCtrl.InvHdr.Supplier)
                            {
                                for (int i = 0; i < invSheet.DataDefinition.FormulaFields.Count; i++)
                                {
                                    // SupplierName is already included in UserField2Value

                                    if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierAddress")
                                        invSheet.DataDefinition.FormulaFields[i].Text = "\"" + sup.CustomerAddress + "\"";
                                    if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierPhone")
                                        invSheet.DataDefinition.FormulaFields[i].Text = "\"" + sup.ContactNo1 + "\"";
                                    if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierFax")
                                        invSheet.DataDefinition.FormulaFields[i].Text = "\"" + sup.ContactNo3 + "\"";
                                }
                            }
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

                        invSheet.DataDefinition.FormulaFields["CustomField1Label"].Text = "\"" + printOutParameters.CustomField1Label + "\"";
                        invSheet.DataDefinition.FormulaFields["CustomField1Value"].Text = "\"" + printOutParameters.CustomField1Value + "\"";

                        invSheet.DataDefinition.FormulaFields["CustomField2Label"].Text = "\"" + printOutParameters.CustomField2Label + "\"";
                        invSheet.DataDefinition.FormulaFields["CustomField2Value"].Text = "\"" + printOutParameters.CustomField2Value + "\"";

                        invSheet.DataDefinition.FormulaFields["CustomField3Label"].Text = "\"" + printOutParameters.CustomField3Label + "\"";
                        invSheet.DataDefinition.FormulaFields["CustomField3Value"].Text = "\"" + printOutParameters.CustomField3Value + "\"";

                        invSheet.DataDefinition.FormulaFields["CustomField4Label"].Text = "\"" + printOutParameters.CustomField4Label + "\"";
                        invSheet.DataDefinition.FormulaFields["CustomField4Value"].Text = "\"" + printOutParameters.CustomField4Value + "\"";

                        invSheet.DataDefinition.FormulaFields["CustomField5Label"].Text = "\"" + printOutParameters.CustomField5Label + "\"";
                        invSheet.DataDefinition.FormulaFields["CustomField5Value"].Text = "\"" + printOutParameters.CustomField5Value + "\"";

                        invSheet.DataDefinition.FormulaFields["AdditionalCost1Label"].Text = "\"" + printOutParameters.AdditionalCost1Label + "\"";
                        invSheet.DataDefinition.FormulaFields["AdditionalCost1Value"].Text = "\"" + printOutParameters.AdditionalCost1Value + "\"";

                        invSheet.DataDefinition.FormulaFields["AdditionalCost2Label"].Text = "\"" + printOutParameters.AdditionalCost2Label + "\"";
                        invSheet.DataDefinition.FormulaFields["AdditionalCost2Value"].Text = "\"" + printOutParameters.AdditionalCost2Value + "\"";

                        invSheet.DataDefinition.FormulaFields["AdditionalCost3Label"].Text = "\"" + printOutParameters.AdditionalCost3Label + "\"";
                        invSheet.DataDefinition.FormulaFields["AdditionalCost3Value"].Text = "\"" + printOutParameters.AdditionalCost3Value + "\"";

                        invSheet.DataDefinition.FormulaFields["AdditionalCost4Label"].Text = "\"" + printOutParameters.AdditionalCost4Label + "\"";
                        invSheet.DataDefinition.FormulaFields["AdditionalCost4Value"].Text = "\"" + printOutParameters.AdditionalCost4Value + "\"";

                        invSheet.DataDefinition.FormulaFields["AdditionalCost5Label"].Text = "\"" + printOutParameters.AdditionalCost5Label + "\"";
                        invSheet.DataDefinition.FormulaFields["AdditionalCost5Value"].Text = "\"" + printOutParameters.AdditionalCost5Value + "\"";

                        invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        crystalReportViewer1.ReportSource = invSheet;
                        crystalReportViewer1.RefreshReport();
                    }
                    else
                    {
                        //not show on hand (use rpt that are no balance qty)
                        var invSheet = new InventorySheetGRNoStockOnHand();

                        dt = invCtrl.FetchUnSavedInventoryItems(showOnHand, showCostPrice, out status);
                        dt.Columns.Add("No");
                        dt.Columns["No"].SetOrdinal(0);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["No"] = (i + 1).ToString();
                        }
                        //Balance Qty issue?

                        invSheet.SetDataSource(dt);
                        invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                        invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                        //Basic Info
                        invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + (string.IsNullOrEmpty(invCtrl.GetCustomRefNo()) ? invCtrl.GetInvHdrRefNo() : invCtrl.GetCustomRefNo()) + "\"";
                        invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
                        invSheet.DataDefinition.FormulaFields["inventorylocation"].Text = "\"" + invCtrl.GetInventoryLocation() + "\"";
                        invSheet.DataDefinition.FormulaFields["remark"].Text = "\"" + invCtrl.GetRemark().Replace("\r\n", "___").Replace("\n", "___").Replace("\"", "" + "\"\"" + "") + "\"";

                        if (invCtrl.IsNew())
                        {
                            invSheet.DataDefinition.FormulaFields["unsaved"].Text = "\"UNCONFIRMED\"";
                        }

                        if (!string.IsNullOrEmpty(invCtrl.InvHdr.Supplier))
                        {
                            Supplier sup = new Supplier(invCtrl.InvHdr.Supplier);
                            if (sup != null && sup.SupplierID.ToString() == invCtrl.InvHdr.Supplier)
                            {
                                for (int i = 0; i < invSheet.DataDefinition.FormulaFields.Count; i++)
                                {
                                    // SupplierName is already included in UserField2Value

                                    if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierAddress")
                                        invSheet.DataDefinition.FormulaFields[i].Text = "\"" + sup.CustomerAddress + "\"";
                                    if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierPhone")
                                        invSheet.DataDefinition.FormulaFields[i].Text = "\"" + sup.ContactNo1 + "\"";
                                    if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierFax")
                                        invSheet.DataDefinition.FormulaFields[i].Text = "\"" + sup.ContactNo3 + "\"";
                                }
                            }
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

                        invSheet.DataDefinition.FormulaFields["CustomField1Label"].Text = "\"" + printOutParameters.CustomField1Label + "\"";
                        invSheet.DataDefinition.FormulaFields["CustomField1Value"].Text = "\"" + printOutParameters.CustomField1Value + "\"";

                        invSheet.DataDefinition.FormulaFields["CustomField2Label"].Text = "\"" + printOutParameters.CustomField2Label + "\"";
                        invSheet.DataDefinition.FormulaFields["CustomField2Value"].Text = "\"" + printOutParameters.CustomField2Value + "\"";

                        invSheet.DataDefinition.FormulaFields["CustomField3Label"].Text = "\"" + printOutParameters.CustomField3Label + "\"";
                        invSheet.DataDefinition.FormulaFields["CustomField3Value"].Text = "\"" + printOutParameters.CustomField3Value + "\"";

                        invSheet.DataDefinition.FormulaFields["CustomField4Label"].Text = "\"" + printOutParameters.CustomField4Label + "\"";
                        invSheet.DataDefinition.FormulaFields["CustomField4Value"].Text = "\"" + printOutParameters.CustomField4Value + "\"";

                        invSheet.DataDefinition.FormulaFields["CustomField5Label"].Text = "\"" + printOutParameters.CustomField5Label + "\"";
                        invSheet.DataDefinition.FormulaFields["CustomField5Value"].Text = "\"" + printOutParameters.CustomField5Value + "\"";

                        invSheet.DataDefinition.FormulaFields["AdditionalCost1Label"].Text = "\"" + printOutParameters.AdditionalCost1Label + "\"";
                        invSheet.DataDefinition.FormulaFields["AdditionalCost1Value"].Text = "\"" + printOutParameters.AdditionalCost1Value + "\"";

                        invSheet.DataDefinition.FormulaFields["AdditionalCost2Label"].Text = "\"" + printOutParameters.AdditionalCost2Label + "\"";
                        invSheet.DataDefinition.FormulaFields["AdditionalCost2Value"].Text = "\"" + printOutParameters.AdditionalCost2Value + "\"";

                        invSheet.DataDefinition.FormulaFields["AdditionalCost3Label"].Text = "\"" + printOutParameters.AdditionalCost3Label + "\"";
                        invSheet.DataDefinition.FormulaFields["AdditionalCost3Value"].Text = "\"" + printOutParameters.AdditionalCost3Value + "\"";

                        invSheet.DataDefinition.FormulaFields["AdditionalCost4Label"].Text = "\"" + printOutParameters.AdditionalCost4Label + "\"";
                        invSheet.DataDefinition.FormulaFields["AdditionalCost4Value"].Text = "\"" + printOutParameters.AdditionalCost4Value + "\"";

                        invSheet.DataDefinition.FormulaFields["AdditionalCost5Label"].Text = "\"" + printOutParameters.AdditionalCost5Label + "\"";
                        invSheet.DataDefinition.FormulaFields["AdditionalCost5Value"].Text = "\"" + printOutParameters.AdditionalCost5Value + "\"";

                        invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        crystalReportViewer1.ReportSource = invSheet;
                        crystalReportViewer1.RefreshReport();
                    }
                }
                else
                {
                    //using rpt.
                    ReportDocument invSheet = new ReportDocument();
                    invSheet.Load(rptLocation);

                    dt = invCtrl.FetchUnSavedInventoryItems(showOnHand, showCostPrice, out status);
                    dt.Columns.Add("No");
                    dt.Columns["No"].SetOrdinal(0);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["No"] = (i + 1).ToString();
                    }
                    //Balance Qty issue?

                    invSheet.SetDataSource(dt);
                    invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                    invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                    //Basic Info
                    invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + (string.IsNullOrEmpty(invCtrl.GetCustomRefNo()) ? invCtrl.GetInvHdrRefNo() : invCtrl.GetCustomRefNo()) + "\"";
                    invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
                    invSheet.DataDefinition.FormulaFields["inventorylocation"].Text = "\"" + invCtrl.GetInventoryLocation() + "\"";
                    invSheet.DataDefinition.FormulaFields["remark"].Text = "\"" + invCtrl.GetRemark().Replace("\r\n", "___").Replace("\n", "___").Replace("\"", "" + "\"\"" + "") + "\"";

                    if (invCtrl.IsNew())
                    {
                        invSheet.DataDefinition.FormulaFields["unsaved"].Text = "\"UNCONFIRMED\"";
                    }

                    if (!string.IsNullOrEmpty(invCtrl.InvHdr.Supplier))
                    {
                        Supplier sup = new Supplier(invCtrl.InvHdr.Supplier);
                        if (sup != null && sup.SupplierID.ToString() == invCtrl.InvHdr.Supplier)
                        {
                            for (int i = 0; i < invSheet.DataDefinition.FormulaFields.Count; i++)
                            {
                                // SupplierName is already included in UserField2Value

                                if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierAddress")
                                    invSheet.DataDefinition.FormulaFields[i].Text = "\"" + sup.CustomerAddress + "\"";
                                if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierPhone")
                                    invSheet.DataDefinition.FormulaFields[i].Text = "\"" + sup.ContactNo1 + "\"";
                                if (invSheet.DataDefinition.FormulaFields[i].Name == "SupplierFax")
                                    invSheet.DataDefinition.FormulaFields[i].Text = "\"" + sup.ContactNo3 + "\"";
                            }
                        }
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
                    invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                    crystalReportViewer1.ReportSource = invSheet;
                    crystalReportViewer1.RefreshReport();
                }
            }
            else
            {
                if (StockActivity == "STOCK TAKE")
                {
                    string rptLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.StockTakeReportFileLocation);
                    bool ReportLoaded = false;
                    if (rptLocation != null && rptLocation.ToLower().EndsWith(".rpt") && File.Exists(rptLocation))
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
                        if (showOnHand)
                        {

                            InventorySheet invSheet = new InventorySheet();

                            dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, out status);
                            dt.Columns.Add("No");
                            dt.Columns["No"].SetOrdinal(0);

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["No"] = (i + 1).ToString();
                            }
                            //Balance Qty issue?

                            invSheet.SetDataSource(dt);
                            invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                            invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                            //Basic Info
                            invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                            invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                            invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                            crystalReportViewer1.ReportSource = invSheet;
                            crystalReportViewer1.RefreshReport();

                        }
                        else
                        {
                            InventorySheetNoBalanceQty invSheet = new InventorySheetNoBalanceQty();

                            dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, out status);
                            dt.Columns.Add("No");
                            dt.Columns["No"].SetOrdinal(0);

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["No"] = (i + 1).ToString();
                            }
                            //Balance Qty issue?

                            invSheet.SetDataSource(dt);
                            invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                            invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                            //Basic Info
                            invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                            invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                            invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                            crystalReportViewer1.ReportSource = invSheet;
                            crystalReportViewer1.RefreshReport();

                        }
                    }
                    else
                    {
                        //using rpt.
                        ReportDocument invSheet = new ReportDocument();
                        invSheet.Load(rptLocation);

                        dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, out status);
                        dt.Columns.Add("No");
                        dt.Columns["No"].SetOrdinal(0);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["No"] = (i + 1).ToString();
                        }
                        invSheet.SetDataSource(dt);
                        invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                        invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                        //Basic Info
                        invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                        invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                        invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        crystalReportViewer1.ReportSource = invSheet;
                        crystalReportViewer1.RefreshReport();

                    }
                }
                else if (StockActivity == "STOCK ISSUE") 
                {
                    string rptLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.StockIssueReportFileLocation);
                    bool ReportLoaded = false;
                    if (rptLocation != null && rptLocation.ToLower().EndsWith(".rpt") && File.Exists(rptLocation))
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
                        rptLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.OtherStockActivityReportFileLocation);
                        if (rptLocation != null && rptLocation.ToLower().EndsWith(".rpt") && File.Exists(rptLocation))
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
                    }

                    if (!ReportLoaded)
                    {
                        if (showOnHand)
                        {
                            if (showCostPrice || showAlternateCostPrice)
                            {
                                InventorySheetWithCostPrice invSheet = new InventorySheetWithCostPrice();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                            else
                            {
                                InventorySheet invSheet = new InventorySheet();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                        }
                        else
                        {

                            if (showCostPrice || showAlternateCostPrice)
                            {
                                InventorySheetNoBalanceQtyWithCostPrice invSheet = new InventorySheetNoBalanceQtyWithCostPrice();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                            else
                            {
                                InventorySheetNoBalanceQty invSheet = new InventorySheetNoBalanceQty();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                        }
                    }
                    else
                    {
                        //using rpt.
                        ReportDocument invSheet = new ReportDocument();
                        invSheet.Load(rptLocation);

                        dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                        dt.Columns.Add("No");
                        dt.Columns["No"].SetOrdinal(0);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["No"] = (i + 1).ToString();
                        }
                        //Balance Qty issue?

                        invSheet.SetDataSource(dt);
                        invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                        invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                        //Basic Info
                        invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                        invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                        invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        crystalReportViewer1.ReportSource = invSheet;
                        crystalReportViewer1.RefreshReport();
                    }
                }
                else if (StockActivity == "STOCK ADJUSTMENT")
                {
                    string rptLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.StockAdjustmentReportFileLocation);
                    bool ReportLoaded = false;
                    if (rptLocation != null && rptLocation.ToLower().EndsWith(".rpt") && File.Exists(rptLocation))
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
                        rptLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.OtherStockActivityReportFileLocation);
                        if (rptLocation != null && rptLocation.ToLower().EndsWith(".rpt") && File.Exists(rptLocation))
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
                    }

                    if (!ReportLoaded)
                    {
                        if (showOnHand)
                        {
                            if (showCostPrice || showAlternateCostPrice)
                            {
                                InventorySheetWithCostPrice invSheet = new InventorySheetWithCostPrice();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                            else
                            {
                                InventorySheet invSheet = new InventorySheet();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                        }
                        else
                        {

                            if (showCostPrice || showAlternateCostPrice)
                            {
                                InventorySheetNoBalanceQtyWithCostPrice invSheet = new InventorySheetNoBalanceQtyWithCostPrice();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                            else
                            {
                                InventorySheetNoBalanceQty invSheet = new InventorySheetNoBalanceQty();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                        }
                    }
                    else
                    {
                        //using rpt.
                        ReportDocument invSheet = new ReportDocument();
                        invSheet.Load(rptLocation);

                        dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                        dt.Columns.Add("No");
                        dt.Columns["No"].SetOrdinal(0);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["No"] = (i + 1).ToString();
                        }
                        //Balance Qty issue?

                        invSheet.SetDataSource(dt);
                        invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                        invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                        //Basic Info
                        invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                        invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                        invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        crystalReportViewer1.ReportSource = invSheet;
                        crystalReportViewer1.RefreshReport();
                    }
                }
                else if (StockActivity == "TRANSFER")
                {
                    string rptLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.StockTransferReportFileLocation);
                    bool ReportLoaded = false;
                    if (rptLocation != null && rptLocation.ToLower().EndsWith(".rpt") && File.Exists(rptLocation))
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
                        rptLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.OtherStockActivityReportFileLocation);
                        if (rptLocation != null && rptLocation.ToLower().EndsWith(".rpt") && File.Exists(rptLocation))
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
                    }

                    if (!ReportLoaded)
                    {
                        if (showOnHand)
                        {
                            if (showCostPrice || showAlternateCostPrice)
                            {
                                InventorySheetWithCostPrice invSheet = new InventorySheetWithCostPrice();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                            else
                            {
                                InventorySheet invSheet = new InventorySheet();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                        }
                        else
                        {

                            if (showCostPrice || showAlternateCostPrice)
                            {
                                InventorySheetNoBalanceQtyWithCostPrice invSheet = new InventorySheetNoBalanceQtyWithCostPrice();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                            else
                            {
                                InventorySheetNoBalanceQty invSheet = new InventorySheetNoBalanceQty();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                        }
                    }
                    else
                    {
                        //using rpt.
                        ReportDocument invSheet = new ReportDocument();
                        invSheet.Load(rptLocation);

                        dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                        dt.Columns.Add("No");
                        dt.Columns["No"].SetOrdinal(0);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["No"] = (i + 1).ToString();
                        }
                        //Balance Qty issue?

                        invSheet.SetDataSource(dt);
                        invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                        invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                        //Basic Info
                        invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                        invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                        invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        crystalReportViewer1.ReportSource = invSheet;
                        crystalReportViewer1.RefreshReport();
                    }
                }
                else if (StockActivity == "STOCK RETURN")
                {
                    string rptLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.StockReturnReportFileLocation);
                    bool ReportLoaded = false;
                    if (rptLocation != null && rptLocation.ToLower().EndsWith(".rpt") && File.Exists(rptLocation))
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
                        rptLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.OtherStockActivityReportFileLocation);
                        if (rptLocation != null && rptLocation.ToLower().EndsWith(".rpt") && File.Exists(rptLocation))
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
                    }

                    if (!ReportLoaded)
                    {
                        if (showOnHand)
                        {
                            if (showCostPrice || showAlternateCostPrice)
                            {
                                InventorySheetWithCostPrice invSheet = new InventorySheetWithCostPrice();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                            else
                            {
                                InventorySheet invSheet = new InventorySheet();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                        }
                        else
                        {

                            if (showCostPrice || showAlternateCostPrice)
                            {
                                InventorySheetNoBalanceQtyWithCostPrice invSheet = new InventorySheetNoBalanceQtyWithCostPrice();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                            else
                            {
                                InventorySheetNoBalanceQty invSheet = new InventorySheetNoBalanceQty();

                                dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                                dt.Columns.Add("No");
                                dt.Columns["No"].SetOrdinal(0);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dt.Rows[i]["No"] = (i + 1).ToString();
                                }
                                //Balance Qty issue?

                                invSheet.SetDataSource(dt);
                                invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                                invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                                //Basic Info
                                invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                                invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                                invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                crystalReportViewer1.ReportSource = invSheet;
                                crystalReportViewer1.RefreshReport();
                            }
                        }
                    }
                    else
                    {
                        //using rpt.
                        ReportDocument invSheet = new ReportDocument();
                        invSheet.Load(rptLocation);

                        dt = invCtrl.FetchMergedInventoryItems(showOnHand, showCostPrice, showAlternateCostPrice, out status);
                        dt.Columns.Add("No");
                        dt.Columns["No"].SetOrdinal(0);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["No"] = (i + 1).ToString();
                        }
                        //Balance Qty issue?

                        invSheet.SetDataSource(dt);
                        invSheet.DataDefinition.FormulaFields["StockActivity"].Text = "\"" + StockActivity + "\"";
                        invSheet.DataDefinition.FormulaFields["CompanyName"].Text = "\"" + CompanyInfo.CompanyName + "\"";

                        //Basic Info
                        invSheet.DataDefinition.FormulaFields["refno"].Text = "\"" + invCtrl.GetInvHdrRefNo() + "\"";
                        invSheet.DataDefinition.FormulaFields["inventorydate"].Text = "\"" + invCtrl.GetInventoryDate().ToString("dd MMM yyyy HH:mm:ss") + "\"";
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
                        invSheet.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        crystalReportViewer1.ReportSource = invSheet;
                        crystalReportViewer1.RefreshReport();
                    }
                }
                else
                {
                    MessageBox.Show("Stock Activity not recognized.");
                }
            }
        }
    }
    public class PrintOutParameters
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
        public string CustomField1Label;
        public string CustomField1Value;
        public string CustomField2Label;
        public string CustomField2Value;
        public string CustomField3Label;
        public string CustomField3Value;
        public string CustomField4Label;
        public string CustomField4Value;
        public string CustomField5Label;
        public string CustomField5Value;
        public string AdditionalCost1Label;
        public string AdditionalCost1Value;
        public string AdditionalCost2Label;
        public string AdditionalCost2Value;
        public string AdditionalCost3Label;
        public string AdditionalCost3Value;
        public string AdditionalCost4Label;
        public string AdditionalCost4Value;
        public string AdditionalCost5Label;
        public string AdditionalCost5Value;
    }
}
