using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using System.Data;
using GenericReport.LocalDAL;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.IO;
using PowerPOS.Container;

namespace GenericReport.NewPrint
{
    public partial class PrintController
    {

        public static void PrintInvoiceQuotation(string PrinterN, int NumOfCopies, int TypeOfPage, QuoteController pos, bool reprint)
        {
            try
            {
                string ReceiptFileLocation;
                #region *) Fetch: Get Template's Path
                ReceiptFileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.QuotationReceiptFileLocation);
                #endregion

                ReportDocument Inst = new ReportDocument();

                bool ReportLoaded = false;
                if (ReceiptFileLocation != null && ReceiptFileLocation.ToLower().EndsWith(".rpt") && File.Exists(ReceiptFileLocation))
                {
                    try
                    {
                        Inst = GetQuotation(pos, ReceiptFileLocation, reprint);
                        ReportLoaded = true;
                    }
                    catch (Exception X)
                    {
                        CommonUILib.HandleException(X);
                    }
                }

                if (ReportLoaded)
                {


                    Inst.PrintOptions.PrinterName = PrinterN;
                    Inst.PrintToPrinter(NumOfCopies, true, 0, 0);

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.Print_SaveReceiptAsDocument), false))
                    {
                        string docPath = AppSetting.GetSetting(AppSetting.SettingsName.Print.PDFPath);
                        CrystalDecisions.Shared.ExportFormatType type = new CrystalDecisions.Shared.ExportFormatType();
                        type = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                        if (!Directory.Exists(docPath + "Quotation\\"))
                        {
                            Directory.CreateDirectory(docPath + "Quotation\\");
                        }
                        Inst.ExportToDisk(type, docPath + "Quotation\\" + pos.GetCustomizedRefNo() + ".pdf");
                    }

                    /*CrystalDecisions.Shared.ExportFormatType type = new CrystalDecisions.Shared.ExportFormatType();
                    type = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                    Inst.ExportToDisk(type, "C:\\Retail\\Doc\\" + pos.GetCustomizedRefNo() + ".pdf");*/
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Failed to print. Cannot find report specified");
                }
            }
            catch (Exception X)
            {
                System.Windows.Forms.MessageBox.Show("Failed to print.");
                Logger.writeLog(X);
            }
        }

        private static ReportDocument GetQuotation(QuoteController pos, string ReceiptLocation, bool reprint)
        {
            try
            {
                ReportDocument Printer = new ReportDocument();
                Printer.Load(ReceiptLocation);

                string strLogoPath = ConfigurationSettings.AppSettings["LogoPath"];
                string strReportLogoName = ConfigurationSettings.AppSettings["LogoFileName"];
                strLogoPath += "\\" + strReportLogoName;

                int LogoWidth = 0, LogoHeight = 0;

                PrintingInfo Infos = new PrintingInfo();
                Infos.AssignDataForQuotation("Receipt", pos, strLogoPath, out LogoWidth, out LogoHeight, reprint);

                Printer.SetDataSource((DataTable)Infos.DocumentInfo);
                for (int Counter = 0; Counter < Printer.Subreports.Count; Counter++)
                {
                    if (Printer.Subreports[Counter].Name == "PaymentType")
                        Printer.Subreports[Counter].SetDataSource((DataTable)Infos.PaymentInfo);
                    if (Printer.Subreports[Counter].Name == "RemainingPoints")
                        Printer.Subreports[Counter].SetDataSource((DataTable)Infos.PointInfo);
                    if (Printer.Subreports[Counter].Name == "DeliveryInfo")
                        Printer.Subreports[Counter].SetDataSource((DataTable)Infos.DeliveryInfo);
                }

                return Printer;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                System.Windows.Forms.MessageBox.Show("Error woi!!");
                return null;
            }
        }

        public static ReportDocument GetInvoice(POSController pos, string ReceiptLocation, bool reprint)
        {
            return GetInvoice(pos, ReceiptLocation, reprint, false);
        }

        public static ReportDocument GetInvoice(POSController pos, string ReceiptLocation, bool reprint, bool isPreview)
        {
            try
            {
                ReportDocument Printer = new ReportDocument();
                Printer.Load(ReceiptLocation);

                string strLogoPath = ConfigurationSettings.AppSettings["LogoPath"];
                string strReportLogoName = ConfigurationSettings.AppSettings["LogoFileName"];
                strLogoPath += "\\" + strReportLogoName;

                int LogoWidth = 0, LogoHeight = 0;

                PrintingInfo Infos = new PrintingInfo();
                Infos.AssignData("Receipt", pos, strLogoPath, out LogoWidth, out LogoHeight, reprint, isPreview);

                Printer.SetDataSource((DataTable)Infos.DocumentInfo);
                for (int Counter = 0; Counter < Printer.Subreports.Count; Counter++)
                {
                    if (Printer.Subreports[Counter].Name == "PaymentType")
                        Printer.Subreports[Counter].SetDataSource((DataTable)Infos.PaymentInfo);
                    if (Printer.Subreports[Counter].Name == "RemainingPoints")
                        Printer.Subreports[Counter].SetDataSource((DataTable)Infos.PointInfo);
                    if (Printer.Subreports[Counter].Name == "DeliveryInfo")
                        Printer.Subreports[Counter].SetDataSource((DataTable)Infos.DeliveryInfo);
                }

                return Printer;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                System.Windows.Forms.MessageBox.Show("Print Failed");
                return null;
            }
        }

        private static bool CheckIfExist(ReportDocument printer, string id)
        {

            var crParameterdef = printer.DataDefinition.FormulaFields;

            foreach (CrystalDecisions.CrystalReports.Engine.FormulaFieldDefinition def in crParameterdef)
            {
                if (def.Name.Equals(id))
                    return true;
            }

            return false;
        }

        private static ReportDocument GetCounterClose(CounterCloseLog close, string ReceiptLocation, bool reprint)
        {
            try
            {
                ReportDocument Printer = new ReportDocument();
                Printer.Load(ReceiptLocation);

                string temp = "";
                for (int i = 1; i <= 10; i++)
                {
                    temp = PaymentTypesController.FetchPaymentByID(i.ToString());
                    if (temp != "" && temp != null)
                    {
                        if (CheckIfExist(Printer, "Payment" + i.ToString() + "Label"))
                            Printer.DataDefinition.FormulaFields["Payment" + i.ToString() + "Label"].Text = "'" + temp + "'";
                    }
                    else
                    {

                        if (CheckIfExist(Printer, "Payment" + i.ToString() + "Label"))
                            Printer.DataDefinition.FormulaFields["Payment" + i.ToString() + "Label"].Text = "''";
                    }
                }
                DataTable dt = ReportController.FetchCounterClosingReportByCounterCloseID(close.CounterCloseID);
                decimal installmentAmount =
                    ReceiptController.FetchTotalInstallment(close.StartTime,
                    close.EndTime, close.PointOfSaleID);
                dt.Rows[0]["Userfloat5"] = installmentAmount;
                dt.Rows[0]["Userfld9"] = CompanyInfo.ReceiptName;
                Printer.SetDataSource(dt);

                for (int Counter = 0; Counter < Printer.Subreports.Count; Counter++)
                {
                    if (Printer.Subreports[Counter].Name == "ProductCategoryReport.rpt")
                    {
                        DataTable dt1 = ReportController.FetchProductCategorySalesReport(
                    close.StartTime, close.EndTime,
                    close.PointOfSale.PointOfSaleName,
                    "", "", "",
                    false, "CategoryName", "ASC");
                        Printer.Subreports[Counter].SetDataSource(dt1);
                    }
                    if (Printer.Subreports[Counter].Name == "ProductSalesReport")
                    {
                        DataTable dt1 =
                            ReportController.FetchProductSalesReport
                    (close.StartTime, close.EndTime, "",
                    close.PointOfSale.PointOfSaleName, "", "",
                    "", false,
                    "CategoryName", "ASC");

                        Printer.Subreports[Counter].SetDataSource(dt1);
                    }
                }

                return Printer;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                System.Windows.Forms.MessageBox.Show("Print Failed!!");
                return null;
            }
        }

        public static ReportDocument GetInvoice(POSController pos, ReportDocument Printer, bool reprint)
        {
            return GetInvoice(pos, Printer, reprint, false);
        }

        public static ReportDocument GetInvoice(POSController pos, ReportDocument Printer, bool reprint, bool isPreview)
        {
            try
            {
                bool ShowPrice = true;
                bool ShowPayment = true;

                //ReportClass Printer = new A5();

                string strLogoPath = ConfigurationSettings.AppSettings["LogoPath"];
                string strReportLogoName = ConfigurationSettings.AppSettings["LogoFileName"];
                strLogoPath += "\\" + strReportLogoName;

                //LoadLogo(Printer, strLogoPath);

                int LogoWidth = 0, LogoHeight = 0;

                PrintingInfo Infos = new PrintingInfo();
                Infos.AssignData("Receipt", pos, strLogoPath, out LogoWidth, out LogoHeight, reprint, isPreview);

                Printer.SetDataSource((DataTable)Infos.DocumentInfo);
                if (ShowPrice && ShowPayment)
                {
                    Printer.Subreports["PaymentType"].SetDataSource((DataTable)Infos.PaymentInfo);
                }

                bool HaveLogoObject = false;
                for (int Counter = 0; Counter < Printer.ReportDefinition.Sections["Header"].ReportObjects.Count; Counter++)
                    if (Printer.ReportDefinition.Sections["Header"].ReportObjects[Counter].Name.ToLower() == "companylogo")
                    { HaveLogoObject = true; break; }

                if (HaveLogoObject)
                {
                    Printer.ReportDefinition.Sections["Header"].ReportObjects["companylogo"].Width = LogoWidth * 2;
                    Printer.ReportDefinition.Sections["Header"].ReportObjects["companylogo"].Height = LogoHeight * 2;
                }

                int StartPage = 0;
                int Endpage = 0;

                return Printer;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                System.Windows.Forms.MessageBox.Show("Print Failed!!");
                return null;
            }
        }

        private static void HidePrice(ReportClass Printer)
        {
            Printer.ReportDefinition.Sections["Header"].ReportObjects["tDisc"].ObjectFormat.EnableSuppress = false;
            Printer.ReportDefinition.Sections["Header"].ReportObjects["tUnitPrice"].ObjectFormat.EnableSuppress = false;
            Printer.ReportDefinition.Sections["Header"].ReportObjects["tLineTotal"].ObjectFormat.EnableSuppress = false;
            Printer.ReportDefinition.Sections["Header"].ReportObjects["DiscPercent1"].ObjectFormat.EnableSuppress = false;
            Printer.ReportDefinition.Sections["Details"].ReportObjects["UnitPrice1"].ObjectFormat.EnableSuppress = false;
            Printer.ReportDefinition.Sections["Details"].ReportObjects["LineTotal1"].ObjectFormat.EnableSuppress = false;

            Printer.ReportDefinition.Sections["Header"].ReportObjects["tQty"].Left = 7080;
            Printer.ReportDefinition.Sections["Details"].ReportObjects["Quantity1"].Left = 7080;
            Printer.ReportDefinition.Sections["Details"].ReportObjects["tDesc"].Width = 4480;
            Printer.ReportDefinition.Sections["Details"].ReportObjects["Description1"].Width = 4480;
        }

        public static void PrintInvoice(string PrinterName, int NumOfCopies, int TypeOfPage, POSController pos, bool reprint)
        {
            string ReceiptFileLocation;
            #region *) Fetch: Get Template's Path
            if (pos.IsADelivery)
                ReceiptFileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.DeliveryOrderFileLocation);
            else
                ReceiptFileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.ReceiptFileLocation);
            #endregion
            
            PrintInvoice(PrinterName, NumOfCopies, TypeOfPage, pos, reprint, ReceiptFileLocation);
        }

        public static void PrintInvoice(string PrinterName, int NumOfCopies, int TypeOfPage, POSController pos, bool reprint, string ReceiptFileLocation)
        {
            try
            {
                ReportDocument Inst = new ReportDocument();

                bool ReportLoaded = false;
                if (ReceiptFileLocation != null && ReceiptFileLocation.ToLower().EndsWith(".rpt") && File.Exists(ReceiptFileLocation))
                {
                    try
                    {
                        Inst = GetInvoice(pos, ReceiptFileLocation, reprint);
                        ReportLoaded = true;
                    }
                    catch (Exception X)
                    {
                        CommonUILib.HandleException(X);
                    }
                }

                if (!ReportLoaded)
                {
                    if (TypeOfPage == PageType.A5)
                        Inst = GetInvoice(pos, new A5(), reprint);
                    else if (TypeOfPage == PageType.A4)
                        Inst = GetInvoice(pos, new A4(), reprint);
                    else
                        Inst = A5Controller.GetInvoice(pos, reprint);
                }

                Inst.PrintOptions.PrinterName = PrinterName;
                for (int i = 0; i < NumOfCopies; i++)
                {
                    Inst.PrintToPrinter(1, true, 0, 0);
                }
            }
            catch (Exception X)
            {
                System.Windows.Forms.MessageBox.Show("Failed to print.");
                Logger.writeLog(X);
            }
        }

        public static void PrintAdditionalInvoice(string PrinterName, int NumOfCopies, int TypeOfPage, POSController pos, bool reprint)
        {
            try
            {
                string ReceiptFileLocation;
                #region *) Fetch: Get Template's Path
                ReceiptFileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.AdditionalReceipt);
                PrinterName = AppSetting.GetSetting(AppSetting.SettingsName.Print.AdditionalPrinter);
                #endregion

                ReportDocument Inst = new ReportDocument();

                bool ReportLoaded = false;
                if (ReceiptFileLocation != null && ReceiptFileLocation.ToLower().EndsWith(".rpt") && File.Exists(ReceiptFileLocation))
                {
                    try
                    {
                        Inst = GetInvoice(pos, ReceiptFileLocation, reprint);
                        ReportLoaded = true;
                    }
                    catch (Exception X)
                    {
                        CommonUILib.HandleException(X);
                    }
                }

                if (!ReportLoaded)
                {
                    Inst = GetInvoice(pos, new A4(), reprint);
                }

                Inst.PrintOptions.PrinterName = PrinterName;
                for (int i = 0; i < NumOfCopies; i++)
                {
                    Inst.PrintToPrinter(1, true, 0, 0);
                }
            }
            catch (Exception X)
            {
                System.Windows.Forms.MessageBox.Show("Failed to print.");
                Logger.writeLog(X);
            }
        }

        public static void PrintClosingReport(string PrinterName, int NumOfCopies, CounterCloseLog close, bool reprint)
        {
            try
            {
                string ReceiptFileLocation;
                #region *) Fetch: Get Template's Path
                ReceiptFileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.ClosingReceiptFileLocation);
                #endregion

                ReportDocument Inst = new ReportDocument();

                bool ReportLoaded = false;
                if (ReceiptFileLocation != null && ReceiptFileLocation.ToLower().EndsWith(".rpt") && File.Exists(ReceiptFileLocation))
                {
                    try
                    {
                        //Inst = GetInvoice(pos, ReceiptFileLocation, reprint);
                        Inst = GetCounterClose(close, ReceiptFileLocation, reprint);
                        ReportLoaded = true;
                    }
                    catch (Exception X)
                    {
                        CommonUILib.HandleException(X);
                    }
                }

                if (!ReportLoaded)
                {

                    /*if (TypeOfPage == PageType.A5)
                        Inst = GetInvoice(pos, new A5(), reprint);
                    else if (TypeOfPage == PageType.A4)
                        Inst = GetInvoice(pos, new A4(), reprint);
                    else
                        Inst = A5Controller.GetInvoice(pos, reprint);*/
                }

                Inst.PrintOptions.PrinterName = PrinterName;
                Inst.PrintToPrinter(NumOfCopies, true, 0, 0);
            }
            catch (Exception X)
            {
                System.Windows.Forms.MessageBox.Show("Failed to print.");
                Logger.writeLog(X);
            }
        }

        public static ReportDocument GetPreOrderMail(string orderHdrID, string orderDetID, string ReceiptLocation)
        {
            try
            {
                ReportDocument Printer = new ReportDocument();
                Printer.Load(ReceiptLocation);
                Printer.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;

                DataTable dt = ReportController.GetInfoPreOrderForEmail(orderHdrID, orderDetID);
                Printer.SetDataSource(dt);

                return Printer;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                System.Windows.Forms.MessageBox.Show("Print Failed!!");
                return null;
            }
        }

        public static void PrintInvoiceFormalOnlyCopy(string PrinterName, int NumOfCopies, int TypeOfPage, POSController pos, bool reprint)
        {
            try
            {
                string ReceiptFileLocation;
                #region *) Fetch: Get Template's Path

                ReceiptFileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.FormalReceiptFileLocation);

                #endregion

                ReportDocument Inst = new ReportDocument();

                bool ReportLoaded = false;
                if (ReceiptFileLocation != null && ReceiptFileLocation.ToLower().EndsWith(".rpt") && File.Exists(ReceiptFileLocation))
                {
                    try
                    {
                        Inst = GetInvoice(pos, ReceiptFileLocation, reprint);
                        ReportLoaded = true;
                    }
                    catch (Exception X)
                    {
                        CommonUILib.HandleException(X);
                    }
                }

                if (!ReportLoaded)
                {
                    if (TypeOfPage == PageType.A5)
                        Inst = GetInvoice(pos, new A5(), reprint);
                    else if (TypeOfPage == PageType.A4)
                        Inst = GetInvoice(pos, new A4(), reprint);
                    else
                        Inst = A5Controller.GetInvoice(pos, reprint);
                }

                Inst.PrintOptions.PrinterName = PrinterName;
                Inst.PrintToPrinter(NumOfCopies, true, 0, 0);

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.Print_SaveReceiptAsDocument), false))
                {
                    string docPath = AppSetting.GetSetting(AppSetting.SettingsName.Print.PDFPath);
                    CrystalDecisions.Shared.ExportFormatType type = new CrystalDecisions.Shared.ExportFormatType();
                    type = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                    if (!Directory.Exists(docPath + "Formal\\"))
                    {
                        Directory.CreateDirectory(docPath + "Formal\\");
                    }
                    Inst.ExportToDisk(type, docPath + "Formal\\" + pos.GetCustomizedRefNo() + ".pdf");
                }
            }
            catch (Exception X)
            {
                System.Windows.Forms.MessageBox.Show("Failed to print.");
                Logger.writeLog(X);
            }
        }

        public static ReportDocument GetReportDocForPreview(POSController pos)
        {
            ReportDocument Inst = new ReportDocument();
            try
            {
                string ReceiptFileLocation;
                ReceiptFileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.ReceiptFileLocation);

                bool ReportLoaded = false;
                if (ReceiptFileLocation != null && ReceiptFileLocation.ToLower().EndsWith(".rpt") && File.Exists(ReceiptFileLocation))
                {
                    try
                    {
                        Inst = GetInvoice(pos, ReceiptFileLocation, false, true);
                        ReportLoaded = true;
                    }
                    catch (Exception X)
                    {
                        CommonUILib.HandleException(X);
                    }
                }

                if (!ReportLoaded)
                {
                    Inst = GetInvoice(pos, new A4(), false, true);
                }
                return Inst;
            }
            catch (Exception X)
            {
                System.Windows.Forms.MessageBox.Show("Failed to print.");
                Logger.writeLog(X);
                return null;
            }

        }
    }
}
