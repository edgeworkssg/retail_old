using System;
using System.Collections.Generic;
using System.Text;
using PowerPOS;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Data;
using PowerPOS.Container;

namespace POSDevices
{
    public class PrintReceiptException : ApplicationException
    {
        // Default constructor
        public PrintReceiptException()
        {
        }
        // Constructor accepting a single string message
        public PrintReceiptException(string message)
            : base(message)
        {
        }
        // Constructor accepting a string message and an
        // inner exception which will be wrapped by this
        // custom exception class
        public PrintReceiptException(string message,
        Exception inner)
            : base(message, inner)
        {
        }
    }

    public class ReceiptSetup
    {
        public static String Company;
        public static String PrinterName;
        public static int PageWidth;
        public static int Padding;
        public static Font companyFont;
        public static Font headerFont;
        public static Font printFont;
        public static Font printFontBold;
        public static Font largerPrintFont;
        public static Font largerPrintFontBold;
        public static Font BarcodeFont;
        public static Font totalFont;
        public static Font termsConditionFont;
        public static String ReceiptHeader, ReceiptFooter;
        public static int RowHeight;
        public static String[] Items;
        public static int n;
        public static Double[] Prices;
        public static int[] Qty;
        public static bool PriceIncludeGST;
        public static Double GST;
        public static decimal change;
        //SICC
        public static String PrinterNameValidation;


        public static void LoadReceiptSetup()
        {
            try
            {
                String FontName;
                int FontSize, termConditionFontSize;
                DataSet ds = new DataSet();

                #region *) Fetch: Load all Settings from XML file
                /* We still need this info later */
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\Receipt.xml");
                #endregion

                bool isDBSettingLoaded = false;
                #region *) Fetch: Load all Settings from DB - If can't find, take from XML and save the value into DB
                try
                {
                    PrinterName = AppSetting.GetSetting(AppSetting.SettingsName.Print.PrinterName);
                    PrinterNameValidation = AppSetting.GetSetting(AppSetting.SettingsName.SICC.DefaultPrinterName);
                    if (PrinterName == null)
                    {
                        PrinterName = ds.Tables[0].Rows[0]["PrinterName"].ToString();
                        AppSetting.SetSetting(AppSetting.SettingsName.Print.PrinterName, PrinterName);
                    }

                    isDBSettingLoaded = true;
                }
                catch (Exception X)
                {
                    Logger.writeLog(X);
                }
                #endregion

                #region *) Fetch: If load from DB completely failed, assign settings from XML (Backup Plan)
                if (!isDBSettingLoaded)
                {
                    PrinterName = ds.Tables[0].Rows[0]["PrinterName"].ToString();
                }
                #endregion

                Company = ds.Tables[0].Rows[0]["Company"].ToString();
                PageWidth = int.Parse(ds.Tables[0].Rows[0]["PageWidth"].ToString());
                Padding = int.Parse(ds.Tables[0].Rows[0]["Padding"].ToString());
                FontName = ds.Tables[0].Rows[0]["CompanyFontName"].ToString();
                FontSize = int.Parse(ds.Tables[0].Rows[0]["CompanyFontSize"].ToString());
                termConditionFontSize = int.Parse(ds.Tables[0].Rows[0]["termConditionFontSize"].ToString());
                companyFont = new Font(FontName, FontSize, FontStyle.Bold);
                FontName = ds.Tables[0].Rows[0]["HeaderFontName"].ToString();
                FontSize = int.Parse(ds.Tables[0].Rows[0]["HeaderFontSize"].ToString());
                headerFont = new Font(FontName, FontSize, FontStyle.Bold);
                FontName = ds.Tables[0].Rows[0]["FontName"].ToString();
                FontSize = int.Parse(ds.Tables[0].Rows[0]["FontSize"].ToString());
                printFont = new Font(FontName, FontSize);
                printFontBold = new Font(FontName, FontSize, FontStyle.Bold);
                largerPrintFont = new Font(FontName, FontSize + 5);
                largerPrintFontBold = new Font(FontName, FontSize+1, FontStyle.Bold);
                termsConditionFont = new Font(FontName, termConditionFontSize);
                BarcodeFont = new Font("Free 3 of 9", 23);
                totalFont = new Font(FontName, FontSize, FontStyle.Bold);
                RowHeight = int.Parse(ds.Tables[0].Rows[0]["RowHeight"].ToString());
                GST = double.Parse(ds.Tables[0].Rows[0]["GST"].ToString());
                PriceIncludeGST = bool.Parse(ds.Tables[0].Rows[0]["PriceIncludeGST"].ToString());
                ReceiptHeader = ds.Tables[0].Rows[0]["ReceiptHeader"].ToString();
                ReceiptFooter = ds.Tables[0].Rows[0]["ReceiptFooter"].ToString();
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Initialise Receipt failed!", E);
            }
        }

    }

    public partial class Receipt
    {
        private String Company;
        private String PrinterName;
        private String PrinterNameValidation;
        private int PageWidth;
        private int Padding;
        private Font companyFont;
        private Font headerFont;
        private Font printFont;
        private Font printFontBold;
        private Font largerPrintFont;
        private Font largerPrintFontBold;
        private Font BarcodeFont;
        private Font totalFont;
        private Font termsConditionFont;
        private String ReceiptHeader, ReceiptFooter;
        private int RowHeight;
        private String[] Items;
        private int n;
        private Double[] Prices;
        private int[] Qty;
        private bool PriceIncludeGST;
        private Double GST;
        private decimal change;
        private POSController pos;
        public QuoteController qos;
        private CounterCloseLog closeLog;
        //private bool PrintProductSalesReport;
        private DateTime _startTime, _endTime;
        private bool PrintItemDepartment;
        private bool PrintDiscount;
        public bool PrintCategorySalesReport = false;
        public bool PrintProductSalesReport;
        private void printCollectionPage(Object sender,
                PrintPageEventArgs e)
        {
            String Counter = closeLog.PointOfSale.PointOfSaleName;
            String Cashier = closeLog.Cashier;

            decimal Collection = closeLog.TotalSystemRecorded;
            decimal ActualCollected = closeLog.TotalActualCollected;
            decimal OpeningAmount = closeLog.OpeningBalance;
            decimal CashIn = closeLog.CashIn;
            decimal CashOut = closeLog.CashOut;
            bool enableNETSIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
            bool enableNETSCashCard = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCashCard), false);
            bool enableNETSFlashPay = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSFlashPay), false);
            bool enableNETSATMCard = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSATMCard), false);
            bool hideActualCollecton = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.HideActualCollection), false);

            try
            {
                RowHeight = (int)printFont.GetHeight();

                int y = 0;
                System.Drawing.StringFormat RightAlign = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                RightAlign.Alignment = System.Drawing.StringAlignment.Far;

                System.Drawing.StringFormat CenterAlign = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                CenterAlign.Alignment = System.Drawing.StringAlignment.Center;
                //Set the variables

                if (CompanyInfo.ReceiptName != "")
                {
                    e.Graphics.DrawString(CompanyInfo.ReceiptName, headerFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += (int)headerFont.GetHeight();
                }

                e.Graphics.DrawString("Counter - " + Counter, headerFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight + 5;

                //String Period = closeLog.StartTime.ToString("dd MMM yyyy HH:mm:ss") + " - " + closeLog.EndTime.ToString("dd MMM yyyy HH:mm:ss");
                e.Graphics.DrawString("From: " + closeLog.StartTime.ToString("dd MMM yyyy HH:mm:ss"), printFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight;

                e.Graphics.DrawString("To: " + closeLog.EndTime.ToString("dd MMM yyyy HH:mm:ss"), printFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight + 5;


                //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 10;

                e.Graphics.DrawString("Cashier:", printFont,
                                      Brushes.Black, Padding, y);
                e.Graphics.DrawString(closeLog.Cashier, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight + 5;

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.ShowSalesWithoutCategoryFilter), false))
                {
                    string categoryfilterName = AppSetting.GetSetting(AppSetting.SettingsName.Print.CategoryFilterName);
                    DataTable dt = ReportController.FetchProductCategorySalesReport(
                        closeLog.StartTime, closeLog.EndTime,
                        closeLog.PointOfSale.PointOfSaleName,
                        "", categoryfilterName.Split(','), "",
                        false, "CategoryName", "ASC");
                    decimal totalCategoryFilterSalesAmount = 0;
                    if (dt.Rows.Count > 0)
                    {
                        //decimal.TryParse(dt.Rows[0]["TotalAmount"].ToString(), out totalCategoryFilterSalesAmount);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            decimal ta;
                            decimal.TryParse(dt.Rows[i]["TotalAmount"].ToString(), out ta);
                            totalCategoryFilterSalesAmount += ta;
                        }
                    }
                    //e.Graphics.DrawString("Sales Excl. " + categoryfilterName + ":", printFont, Brushes.Black, Padding, y);
                    int TextWidth = (int)e.Graphics.MeasureString(String.Format("{0:N}", Collection - totalCategoryFilterSalesAmount), printFont, PageWidth - Padding).Width;
                    int TextHeight = (int)e.Graphics.MeasureString("Sales Excl. " + categoryfilterName + ":", printFont, PageWidth - Padding - TextWidth).Height;
                    e.Graphics.DrawString("Sales Excl. " + categoryfilterName + ":", printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding - TextWidth, TextHeight));
                    e.Graphics.DrawString(String.Format("{0:N}", Collection - totalCategoryFilterSalesAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += TextHeight;

                    //e.Graphics.DrawString(categoryfilterName + ":", printFont, Brushes.Black, Padding, y);
                    //e.Graphics.DrawString(String.Format("{0:N}", totalCategoryFilterSalesAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    //y += RowHeight;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        decimal ta;
                        decimal.TryParse(dt.Rows[i]["TotalAmount"].ToString(), out ta);
                        e.Graphics.DrawString(dt.Rows[i]["CategoryName"].ToString() + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", ta), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.ShowSalesWithoutItemDeptFilter), false))
                {
                    string itemDeptfilterName = AppSetting.GetSetting(AppSetting.SettingsName.Print.ItemDeptFilterName);
                    DataTable dt = ReportController.FetchDepartmentSalesReport(
                        closeLog.StartTime, closeLog.EndTime,
                        closeLog.PointOfSale.PointOfSaleName,
                        "", itemDeptfilterName,
                        false, "DepartmentName", "ASC");
                    decimal totalCategoryFilterSalesAmount = 0;
                    if (dt.Rows.Count > 0)
                    {
                        decimal.TryParse(dt.Rows[0]["TotalAmount"].ToString(), out totalCategoryFilterSalesAmount);
                    }
                    e.Graphics.DrawString("Sales Excl. " + itemDeptfilterName + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", Collection - totalCategoryFilterSalesAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    e.Graphics.DrawString(itemDeptfilterName + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", totalCategoryFilterSalesAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;


                }

                e.Graphics.DrawString("Total Sales:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", Collection + CashOut), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 10;

                string tmp = string.Empty;

                decimal GSTRecorded = ClosingController.GetTotalGSTRecorded(closeLog.StartTime, closeLog.EndTime,
                         closeLog.PointOfSaleID.ToString());
                if (GSTRecorded > 0)
                {
                    string gstText = "GST";
                    if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith)))
                        gstText = AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith);

                    e.Graphics.DrawString("Total " + gstText + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", GSTRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    y += 10;
                }

                string counttransaction = ClosingController.GetTotalNumberOfOrder(closeLog.StartTime, closeLog.EndTime,
                       closeLog.PointOfSaleID.ToString());

                e.Graphics.DrawString("No of Bill:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(counttransaction, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                int countVoid = 0;
                decimal amountVoid = 0;
                ClosingController.GetTotalNumberOfVoidedOrder(closeLog, out countVoid, out amountVoid);

                e.Graphics.DrawString("No of Void: ", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(countVoid.ToString("N0"), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                y += 10;


                if (closeLog.TotalForeignCurrency.GetValueOrDefault(0) != 0)
                {
                    string deffCurr = AppSetting.GetSetting(AppSetting.SettingsName.Currency.DefaultCurrency);
                    string deffCurrSymbol = "$";
                    Currency deffCurrData = new Currency(Currency.Columns.CurrencyCode, deffCurr);
                    if (!deffCurrData.IsNew && !string.IsNullOrEmpty(deffCurrData.CurrencySymbol))
                        deffCurrSymbol = deffCurrData.CurrencySymbol;

                    e.Graphics.DrawString("Foreign Currency in " + deffCurr + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0}{1}", deffCurrSymbol, closeLog.TotalForeignCurrency.GetValueOrDefault(0).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                    y += 10;
                }

                if (PowerPOS.Feature.Package.isAvailable)
                {
                    e.Graphics.DrawString("Points Redemption:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", closeLog.PointRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                    y += 10;

                    e.Graphics.DrawString("Package Redemption:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", closeLog.PackageRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                    y += 10;
                }


                decimal installmentAmount =
                    ReceiptController.FetchTotalInstallment(closeLog.StartTime,
                    closeLog.EndTime, closeLog.PointOfSaleID);
                if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText)))
                    e.Graphics.DrawString("Installment:", printFont, Brushes.Black, Padding, y);
                else
                    e.Graphics.DrawString(AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText) + ":", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", installmentAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;
                y += 10;

                e.Graphics.DrawString("Payment Type Breakdown:", printFont, Brushes.Black, Padding, y);
                y += RowHeight + 5;
                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += RowHeight;

                bool isAllCollectedZero = true;

                #region *) CASH
                decimal TotalCashRecorded = CashIn + closeLog.CashRecorded.GetValueOrDefault(0);
                decimal TotalCashCollected = closeLog.CashCollected;
                if (TotalCashCollected != 0 || TotalCashRecorded != 0 || (CashOut != 0))
                {
                    e.Graphics.DrawString("CASH:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", TotalCashRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    e.Graphics.DrawString("\tCash In:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", CashIn), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    e.Graphics.DrawString("\tSales:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", closeLog.CashRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    e.Graphics.DrawString("Cash Out:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", CashOut), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    //if (closeLog.ClosingCashOut != 0)
                    //{
                    //    e.Graphics.DrawString("Closing Cash Out:", printFont, Brushes.Black, Padding, y);
                    //    e.Graphics.DrawString(String.Format("{0:N}", closeLog.ClosingCashOut), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    //    y += RowHeight;
                    //}

                    e.Graphics.DrawString("Cash Balance:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", TotalCashRecorded - (CashOut)), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    if ((closeLog.CashCollected + closeLog.OpeningBalance != 0))
                        isAllCollectedZero = false;

                    if (!hideActualCollecton && closeLog.CashCollected != 0 && (closeLog.CashCollected + closeLog.OpeningBalance != 0))
                    {
                        e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.CashCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.IsShowCashDenominationOnReceipt), false))
                        {
                            CounterCloseDetCollection ccdcol = ReceiptController.FetchCounterCloseDet(closeLog.CounterCloseID);

                            if (ccdcol.Count > 0)
                            {
                                foreach (CounterCloseDet cc in ccdcol)
                                {
                                    e.Graphics.DrawString(string.Format("  {0} x {1}", cc.UnitDisplayedName, cc.TotalCount), printFont, Brushes.Black, Padding, y);
                                    e.Graphics.DrawString(String.Format("{0:N} ", cc.TotalAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding - 35, 30), RightAlign);
                                    y += RowHeight;
                                }
                            }
                            
                        }

                        e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", TotalCashCollected - TotalCashRecorded + CashOut), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                    y += 10;
                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += RowHeight;
                }

                if (!string.IsNullOrEmpty(closeLog.ForeignCurrency1) && (closeLog.ForeignCurrency1Recorded.GetValueOrDefault(0) != 0 || closeLog.ForeignCurrency1Collected.GetValueOrDefault(0) != 0))
                {
                    Currency curr = new Currency(Currency.Columns.CurrencyCode, closeLog.ForeignCurrency1.Replace("CASH-", ""));
                    string currSymbol = "$";
                    if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                        currSymbol = curr.CurrencySymbol;
                    e.Graphics.DrawString(closeLog.ForeignCurrency1 + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(string.Format("{0}{1}", currSymbol, closeLog.ForeignCurrency1Recorded.GetValueOrDefault(0).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    if (closeLog.ForeignCurrency1Collected.GetValueOrDefault(0) != 0)
                        isAllCollectedZero = false;

                    if (!hideActualCollecton && closeLog.ForeignCurrency1Collected.GetValueOrDefault(0) != 0)
                    {
                        e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.ForeignCurrency1Collected.GetValueOrDefault(0).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", (closeLog.ForeignCurrency1Collected.GetValueOrDefault(0) - closeLog.ForeignCurrency1Recorded.GetValueOrDefault(0)).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                    y += 10;

                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += RowHeight;
                }

                if (!string.IsNullOrEmpty(closeLog.ForeignCurrency2) && (closeLog.ForeignCurrency2Recorded.GetValueOrDefault(0) != 0 || closeLog.ForeignCurrency2Collected.GetValueOrDefault(0) != 0))
                {
                    Currency curr = new Currency(Currency.Columns.CurrencyCode, closeLog.ForeignCurrency2.Replace("CASH-", ""));
                    string currSymbol = "$";
                    if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                        currSymbol = curr.CurrencySymbol;
                    e.Graphics.DrawString("\t" + closeLog.ForeignCurrency2 + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(string.Format("{0}{1}", currSymbol, closeLog.ForeignCurrency2Recorded.GetValueOrDefault(0).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    if (closeLog.ForeignCurrency2Collected.GetValueOrDefault(0) != 0)
                        isAllCollectedZero = false;

                    if (!hideActualCollecton && closeLog.ForeignCurrency2Collected.GetValueOrDefault(0) != 0)
                    {
                        e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.ForeignCurrency2Collected.GetValueOrDefault(0).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", (closeLog.ForeignCurrency2Collected.GetValueOrDefault(0) - closeLog.ForeignCurrency2Recorded.GetValueOrDefault(0)).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                    y += 10;

                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += RowHeight;
                }

                if (!string.IsNullOrEmpty(closeLog.ForeignCurrency3) && (closeLog.ForeignCurrency3Recorded.GetValueOrDefault(0) != 0 || closeLog.ForeignCurrency3Collected.GetValueOrDefault(0) != 0))
                {
                    isAllCollectedZero = false;
                    Currency curr = new Currency(Currency.Columns.CurrencyCode, closeLog.ForeignCurrency3.Replace("CASH-", ""));
                    string currSymbol = "$";
                    if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                        currSymbol = curr.CurrencySymbol;
                    e.Graphics.DrawString("\t" + closeLog.ForeignCurrency3 + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(string.Format("{0}{1}", currSymbol, closeLog.ForeignCurrency3Recorded.GetValueOrDefault(0).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    if (closeLog.ForeignCurrency3Collected.GetValueOrDefault(0) != 0)
                        isAllCollectedZero = false;

                    if (!hideActualCollecton && closeLog.ForeignCurrency3Collected.GetValueOrDefault(0) != 0)
                    {
                        e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.ForeignCurrency3Collected.GetValueOrDefault(0).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }

                    e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", (closeLog.ForeignCurrency3Collected.GetValueOrDefault(0) - closeLog.ForeignCurrency3Recorded.GetValueOrDefault(0)).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                    y += 10;

                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += RowHeight;
                }

                if (!string.IsNullOrEmpty(closeLog.ForeignCurrency4) && (closeLog.ForeignCurrency4Recorded.GetValueOrDefault(0) != 0 || closeLog.ForeignCurrency4Collected.GetValueOrDefault(0) != 0))
                {
                    Currency curr = new Currency(Currency.Columns.CurrencyCode, closeLog.ForeignCurrency4.Replace("CASH-", ""));
                    string currSymbol = "$";
                    if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                        currSymbol = curr.CurrencySymbol;
                    e.Graphics.DrawString("\t" + closeLog.ForeignCurrency4 + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(string.Format("{0}{1}", currSymbol, closeLog.ForeignCurrency4Recorded.GetValueOrDefault(0).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    if (closeLog.ForeignCurrency4Collected.GetValueOrDefault(0) != 0)
                        isAllCollectedZero = false;

                    if (!hideActualCollecton && closeLog.ForeignCurrency4Collected.GetValueOrDefault(0) != 0)
                    {
                        e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.ForeignCurrency4Collected.GetValueOrDefault(0).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;


                        e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", (closeLog.ForeignCurrency4Collected.GetValueOrDefault(0) - closeLog.ForeignCurrency4Recorded.GetValueOrDefault(0)).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }

                    y += 10;

                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += RowHeight;
                }

                if (!string.IsNullOrEmpty(closeLog.ForeignCurrency5) && (closeLog.ForeignCurrency5Recorded.GetValueOrDefault(0) != 0 || closeLog.ForeignCurrency5Collected.GetValueOrDefault(0) != 0))
                {
                    Currency curr = new Currency(Currency.Columns.CurrencyCode, closeLog.ForeignCurrency5.Replace("CASH-", ""));
                    string currSymbol = "$";
                    if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                        currSymbol = curr.CurrencySymbol;
                    e.Graphics.DrawString("\t" + closeLog.ForeignCurrency5 + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(string.Format("{0}{1}", currSymbol, closeLog.ForeignCurrency5Recorded.GetValueOrDefault(0).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    if (closeLog.ForeignCurrency5Collected.GetValueOrDefault(0) != 0)
                        isAllCollectedZero = false;

                    if (!hideActualCollecton && closeLog.ForeignCurrency5Collected.GetValueOrDefault(0) != 0)
                    {
                        e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.ForeignCurrency5Collected.GetValueOrDefault(0).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", (closeLog.ForeignCurrency5Collected.GetValueOrDefault(0) - closeLog.ForeignCurrency5Recorded.GetValueOrDefault(0)).ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                    y += 10;

                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += RowHeight;
                }

                #endregion

                #region *) NETS
                if (enableNETSATMCard && closeLog.NetsATMCardRecorded != 0 || closeLog.NetsATMCardCollected != 0)
                {
                    e.Graphics.DrawString(POSController.PAY_NETSATMCard + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", closeLog.NetsATMCardRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    if (closeLog.NetsATMCardCollected != 0)
                        isAllCollectedZero = false;

                    if (!hideActualCollecton && closeLog.NetsATMCardCollected != 0)
                    {
                        isAllCollectedZero = false;
                        e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.NetsATMCardCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.NetsATMCardCollected - closeLog.NetsATMCardRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                    y += 10;

                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += RowHeight;
                }
                if (enableNETSCashCard && closeLog.NetsCashCardRecorded != 0 || closeLog.NetsCashCardCollected != 0)
                {
                    e.Graphics.DrawString(POSController.PAY_NETSCashCard + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", closeLog.NetsCashCardRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    if (closeLog.NetsCashCardCollected != 0)
                        isAllCollectedZero = false;

                    if (!hideActualCollecton && closeLog.NetsCashCardCollected != 0)
                    {
                        isAllCollectedZero = false;
                        e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.NetsCashCardCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;


                        e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.NetsATMCardCollected - closeLog.NetsATMCardRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }

                    y += 10;

                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += RowHeight;
                }
                if (enableNETSFlashPay && closeLog.NetsFlashPayRecorded != 0 || closeLog.NetsFlashPayCollected != 0)
                {
                    e.Graphics.DrawString(POSController.PAY_NETSFlashPay + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", closeLog.NetsFlashPayRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    if (closeLog.NetsFlashPayCollected != 0)
                        isAllCollectedZero = false;

                    if (!hideActualCollecton && closeLog.NetsFlashPayCollected != 0)
                    {
                        isAllCollectedZero = false;
                        e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.NetsFlashPayCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.NetsFlashPayCollected - closeLog.NetsFlashPayRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                    y += 10;

                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += RowHeight;
                }

                #endregion

                #region *) ANOTHER PAYMENT
                tmp = PaymentTypesController.FetchPaymentByID("1");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration) && (closeLog.NetsRecorded != 0 || closeLog.NetsCollected != 0))
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.NetsRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        if (closeLog.NetsCollected != 0)
                            isAllCollectedZero = false;

                        if (!hideActualCollecton && closeLog.NetsCollected != 0)
                        {

                            e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.NetsCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;

                            e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.NetsCollected - closeLog.NetsRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;
                        }
                        y += 10;

                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        y += RowHeight;
                    }
                }
                tmp = PaymentTypesController.FetchPaymentByID("2");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration) && (closeLog.VisaRecorded != 0 || closeLog.VisaCollected != 0))
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.VisaRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        if (closeLog.VisaCollected != 0)
                            isAllCollectedZero = false;

                        if (!hideActualCollecton && closeLog.VisaCollected != 0)
                        {
                            e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.VisaCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;

                            e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.VisaCollected - closeLog.VisaRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;
                        }
                        y += 10;

                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        y += RowHeight;
                    }
                }
                tmp = PaymentTypesController.FetchPaymentByID("3");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration) && (closeLog.ChinaNetsRecorded != 0 || closeLog.ChinaNetsCollected != 0))
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.ChinaNetsRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        if (closeLog.ChinaNetsCollected != 0)
                            isAllCollectedZero = false;

                        if (!hideActualCollecton && closeLog.ChinaNetsCollected != 0)
                        {
                            e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.ChinaNetsCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;


                            e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.ChinaNetsCollected - closeLog.ChinaNetsRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;
                        }
                        y += 10;

                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        y += RowHeight;
                    }
                }
                tmp = PaymentTypesController.FetchPaymentByID("4");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration) && (closeLog.AmexRecorded != 0 || closeLog.AmexCollected != 0))
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.AmexRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        if (closeLog.AmexCollected != 0)
                            isAllCollectedZero = false;

                        if (!hideActualCollecton && closeLog.AmexCollected != 0)
                        {
                            e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.AmexCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;


                            e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.AmexCollected - closeLog.AmexRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;
                        }
                        y += 10;

                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        y += RowHeight;
                    }
                }
                tmp = PaymentTypesController.FetchPaymentByID("5");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration) && (closeLog.Pay5Recorded != 0 || closeLog.Pay5Collected != 0))
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay5Recorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        if (closeLog.Pay5Collected != 0)
                            isAllCollectedZero = false;

                        if (!hideActualCollecton && closeLog.Pay5Collected != 0)
                        {
                            e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay5Collected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;

                            e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay5Collected - closeLog.Pay5Recorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;
                        }
                        y += 10;

                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        y += RowHeight;
                    }
                }
                tmp = PaymentTypesController.FetchPaymentByID("6");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration) && (closeLog.Pay6Recorded != 0 || closeLog.Pay6Collected != 0))
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay6Recorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        if (closeLog.Pay6Collected != 0)
                            isAllCollectedZero = false;

                        if (!hideActualCollecton && closeLog.Pay6Collected != 0)
                        {
                            e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay6Collected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;

                            e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay6Collected - closeLog.Pay6Recorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;
                        }
                        y += 10;

                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        y += RowHeight;
                    }
                }

                tmp = PaymentTypesController.FetchPaymentByID("7");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration) && (closeLog.Pay7Recorded != 0 || closeLog.Pay7Collected != 0))
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay7Recorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        if (closeLog.Pay7Collected != 0)
                            isAllCollectedZero = false;

                        if (!hideActualCollecton && closeLog.Pay7Collected != 0)
                        {
                            e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay7Collected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;

                            e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay7Collected - closeLog.Pay7Recorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;
                        }
                        y += 10;

                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        y += RowHeight;
                    }
                }

                tmp = PaymentTypesController.FetchPaymentByID("8");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration) && (closeLog.Pay8Recorded != 0 || closeLog.Pay8Collected != 0))
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay8Recorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        if (closeLog.Pay8Collected != 0)
                            isAllCollectedZero = false;

                        if (!hideActualCollecton && closeLog.Pay8Collected > 0)
                        {
                            e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay8Collected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;

                            e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay8Collected - closeLog.Pay8Recorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;
                        }
                        y += 10;

                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        y += RowHeight;
                    }
                }

                tmp = PaymentTypesController.FetchPaymentByID("9");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration) && (closeLog.Pay9Recorded != 0 || closeLog.Pay9Collected != 0))
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay9Recorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        if (closeLog.Pay9Collected != 0)
                            isAllCollectedZero = false;

                        if (!hideActualCollecton && closeLog.Pay9Collected != 0)
                        {
                            e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay9Collected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;


                            e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay9Collected - closeLog.Pay9Recorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;
                        }
                        y += 10;

                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        y += RowHeight;
                    }
                }

                tmp = PaymentTypesController.FetchPaymentByID("10");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration) && (closeLog.Pay10Recorded != 0 || closeLog.Pay10Collected != 0))
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay10Recorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        if (closeLog.Pay10Collected != 0)
                            isAllCollectedZero = false;

                        if (!hideActualCollecton && closeLog.Pay10Collected != 0)
                        {
                            e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay10Collected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;


                            e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", closeLog.Pay10Collected - closeLog.Pay10Recorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;
                        }
                        y += 10;

                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        y += RowHeight;
                    }
                }

                if (closeLog.Userfloat1 != 0 || closeLog.Userfloat2 != 0)
                {
                    e.Graphics.DrawString("CHEQUE:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", closeLog.Userfloat1), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    if (closeLog.Userfloat2 != 0)
                        isAllCollectedZero = false;

                    if (!hideActualCollecton && closeLog.Userfloat2 != 0)
                    {
                        e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.Userfloat2), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", closeLog.Userfloat2 - closeLog.Userfloat1), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }

                    y += 10;

                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += RowHeight;
                }
                #endregion

                /*
                decimal voucherValue;
                if (closeLog.VoucherRecorded.HasValue)
                    voucherValue = closeLog.VoucherRecorded.Value;
                else 
                    voucherValue = 0;

                e.Graphics.DrawString("Voucher Type 1:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", voucherValue), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight; y += 10;
                
                DataTable dt = ReportController.FetchProductSalesReport(closeLog.StartTime, closeLog.EndTime,
                    (new Item(POSController.VOUCHER_BARCODE)).ItemName,
                    closeLog.PointOfSale.PointOfSaleName, "", "", closeLog.PointOfSale.DepartmentID.Value.ToString(), false, "TotalQuantity", "DESC");

                if (dt.Rows.Count > 0)
                {
                    e.Graphics.DrawString("Voucher Type 2:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", decimal.Parse(dt.Rows[0]["TotalAmount"].ToString())), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                    y += 10;
                }

                dt = ReportController.FetchProductSalesReport
                    (closeLog.StartTime, closeLog.EndTime, (new Item(MembershipController.REDEEM_BARCODE)).ItemName,
                   closeLog.PointOfSale.PointOfSaleName, "", "", closeLog.PointOfSale.DepartmentID.Value.ToString(), false, "TotalQuantity", "DESC");

                if (dt.Rows.Count > 0)
                {
                    e.Graphics.DrawString("REDEMPTION:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", decimal.Parse(dt.Rows[0]["TotalAmount"].ToString())), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                    y += 10;
                }
                
                y += RowHeight;
                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += RowHeight;
                y += 10;*/

                y += 10;
                e.Graphics.DrawString("Opening Balance:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", OpeningAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                //e.Graphics.DrawString("Closing Cash Out:", printFont, Brushes.Black, Padding, y);
                //e.Graphics.DrawString(String.Format("{0:N}", closeLog.ClosingCashOut), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                //y += RowHeight;

                e.Graphics.DrawString("Deposit Bag No:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", closeLog.DepositBagNo), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 10;
                if (!isAllCollectedZero)
                {
                    e.Graphics.DrawString("Surplus (+/-):", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", closeLog.Variance), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                }

                //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 3;
                //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                /*
                y += RowHeight+5;
                e.Graphics.DrawString(DateTime.Now.ToString(), printFont, Brushes.Black, Padding, y);
                
                y += RowHeight + 5;
                 dt = ReportController.FetchProductSalesReport(closeLog.StartTime, closeLog.EndTime, "",
                    closeLog.PointOfSale.PointOfSaleName, "", "", closeLog.PointOfSale.DepartmentID.Value.ToString(), false, "TotalQuantity", "DESC");
                if (dt != null)
                {
                    e.Graphics.DrawString
                    ("Item",
                        printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString
                        ("\t\t" + "Qty",
                            printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString("Total",
                        printFont, Brushes.Black,
                        new RectangleF(Padding, y, PageWidth - Padding, 30),
                        RightAlign);
                    y += RowHeight + 2;
                    string item;
                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        item = dt.Rows[i]["ItemNo"].ToString() + "-" + dt.Rows[i]["ItemName"].ToString();
                        if (item.Length <= 40)
                        {
                            e.Graphics.DrawString(item,
                                printFont, Brushes.Black, Padding, y);
                            y += RowHeight + 2;
                        }
                        else
                        {
                            e.Graphics.DrawString(item.Substring(0, 40),
                                printFont, Brushes.Black, Padding, y);
                            y += RowHeight + 2;
                            e.Graphics.DrawString(item.Substring(41),
                                printFont, Brushes.Black, Padding, y);
                            y += RowHeight + 2;
                        }
                        
                        e.Graphics.DrawString
                            ("\t\t" + dt.Rows[i]["TotalQuantity"].ToString(),
                            printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", decimal.Parse(dt.Rows[i]["TotalAmount"].ToString())),
                            printFont, Brushes.Black,
                            new RectangleF(Padding, y, PageWidth - Padding, 30),
                            RightAlign);
                        y += RowHeight + 2;
                    }
                }
                y += RowHeight + 2;
                
                //Sales Person Report
                dt = 
                    ReportController.FetchSalesPersonSalesByDate
                    (closeLog.StartTime, closeLog.EndTime, closeLog.PointOfSale.OutletName, 
                    closeLog.PointOfSale.PointOfSaleName, "%");

                e.Graphics.DrawString
                    ("Salesman",
                        printFont, Brushes.Black, Padding, y);

                e.Graphics.DrawString("Sales($$)",
                    printFont, Brushes.Black,
                    new RectangleF(Padding, y, PageWidth - Padding, 30),
                    RightAlign);

                y += RowHeight + 1;
                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    e.Graphics.DrawString
                        (dt.Rows[i]["SalesPersonName"].ToString() + "\t\t" ,
                        printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}",
                        decimal.Parse(dt.Rows[i]["salesamount"].ToString())),
                        printFont, Brushes.Black,
                        new RectangleF(Padding, y, PageWidth - Padding, 30),
                        RightAlign);
                    y += RowHeight + 2;

                }
                y += RowHeight + 4;
                */

                #region Print Discount

                if (PrintDiscount)
                {
                    DataTable discDT = ReportController.FetchDiscountReport(closeLog.StartTime, closeLog.EndTime, closeLog.PointOfSaleID);
                    if (discDT != null && discDT.Rows.Count > 0 && discDT.Columns.Count > 1)
                    {
                        y += RowHeight;

                        e.Graphics.DrawString("Discount Price : ", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", ((decimal)discDT.Rows[0][1])), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        e.Graphics.DrawString("Discount (%) : ", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", ((decimal)discDT.Rows[0][0])), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                }

                #endregion

                #region *) Copied from Restaurant: Print Product Saved Report
                if (PrintItemDepartment)
                {
                    DataTable dt = ReportController.FetchDepartmentSalesReport(
                        closeLog.StartTime, closeLog.EndTime,
                        closeLog.PointOfSale.PointOfSaleName,
                        "", "",
                        false, "DepartmentName", "ASC");

                    if (dt != null)
                    {
                        y += RowHeight;

                        e.Graphics.DrawString("Department Sales Report", headerFont,
                          Brushes.Black, Padding, y);
                        y += RowHeight + 5;

                        e.Graphics.DrawString
                        ("Department",
                            printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString
                            ("\t\t" + "Qty",
                                printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString("Total",
                            printFont, Brushes.Black,
                            new RectangleF(Padding, y, PageWidth - Padding, 30),
                            RightAlign);
                        y += RowHeight;
                        string item;
                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            item = dt.Rows[i]["DepartmentName"].ToString();
                            int TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                            e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                            y += TextHeight;
                            e.Graphics.DrawString
                                ("\t\t" + dt.Rows[i]["TotalQuantity"].ToString(),
                                printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", decimal.Parse(dt.Rows[i]["TotalAmount"].ToString())),
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                RightAlign);
                            y += RowHeight;
                        }
                    }
                }

                if (PrintProductSalesReport)
                {
                    string categoryName = "";
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.UseCategoryFilterOnClosingReport), false))
                    {
                        categoryName = AppSetting.GetSetting(AppSetting.SettingsName.Print.CategoryFilterName) == null ? "" : AppSetting.GetSetting(AppSetting.SettingsName.Print.CategoryFilterName);
                    }
                    DataTable dt = ReportController.FetchProductCategorySalesReport(
                        closeLog.StartTime, closeLog.EndTime,
                        closeLog.PointOfSale.PointOfSaleName,
                        "", categoryName.Split(','), "",
                        false, "CategoryName", "ASC");

                    if (dt != null)
                    {
                        y += RowHeight;

                        e.Graphics.DrawString("Category Sales Report", headerFont,
                          Brushes.Black, Padding, y);
                        y += RowHeight + 5;

                        e.Graphics.DrawString
                        ("Category",
                            printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString
                            ("\t\t" + "Qty",
                                printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString("Total",
                            printFont, Brushes.Black,
                            new RectangleF(Padding, y, PageWidth - Padding, 30),
                            RightAlign);
                        y += RowHeight;
                        string item;
                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            item = dt.Rows[i]["CategoryName"].ToString();
                            int TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                            e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                            y += TextHeight;
                            //if (item.Length < 20)
                            //{
                            //    e.Graphics.DrawString(item,
                            //        printFont, Brushes.Black, Padding, y);
                            //    y += RowHeight;
                            //}
                            //else
                            //{
                            //    e.Graphics.DrawString(item.Substring(0, 20),
                            //        printFont, Brushes.Black, Padding, y);
                            //    y += RowHeight;
                            //    e.Graphics.DrawString(item.Substring(20),
                            //        printFont, Brushes.Black, Padding, y);
                            //    y += RowHeight;
                            //}

                            e.Graphics.DrawString
                                ("\t\t" + dt.Rows[i]["TotalQuantity"].ToString(),
                                printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", decimal.Parse(dt.Rows[i]["TotalAmount"].ToString())),
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                RightAlign);
                            y += RowHeight;
                        }
                    }
                    y += RowHeight;
                    dt = ReportController.FetchProductSalesReport
                        (closeLog.StartTime, closeLog.EndTime, "",
                        closeLog.PointOfSale.PointOfSaleName, "", categoryName.Split(','),
                        "", false,
                        "CategoryName", "ASC");
                    if (dt != null)
                    {
                        string item;
                        string categoryname = "";

                        e.Graphics.DrawString("Product Sales Report", headerFont,
                            Brushes.Black, Padding, y);

                        y += RowHeight + 5;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (categoryname != dt.Rows[i]["CategoryName"].ToString())
                            {
                                categoryname = dt.Rows[i]["CategoryName"].ToString();
                                if (categoryname.Length < 20)
                                {

                                    e.Graphics.DrawString(categoryname,
                                        printFont, Brushes.Black, Padding, y);
                                    y += RowHeight;
                                }
                                else
                                {
                                    e.Graphics.DrawString(categoryname.Substring(0, 20),
                                    printFont, Brushes.Black, Padding, y);
                                    y += RowHeight;
                                    e.Graphics.DrawString(categoryname.Substring(20),
                                        printFont, Brushes.Black, Padding, y);
                                    y += RowHeight;
                                }
                                e.Graphics.DrawString
                                ("Item", printFont, Brushes.Black, Padding, y);
                                e.Graphics.DrawString
                                    ("\t\t" + "Qty", printFont, Brushes.Black, Padding, y);
                                e.Graphics.DrawString("Total",
                                    printFont, Brushes.Black,
                                    new RectangleF(Padding, y, PageWidth - Padding, 30),
                                    RightAlign);
                                y += RowHeight;
                                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                            }
                            item = dt.Rows[i]["ItemNo"].ToString() + "-" + dt.Rows[i]["ItemName"].ToString();
                            int TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                            e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                            y += TextHeight;
                            //if (item.Length <= 20)
                            //{
                            //    e.Graphics.DrawString(item,
                            //        printFont, Brushes.Black, Padding, y);
                            //    y += RowHeight;
                            //}
                            //else
                            //{
                            //    e.Graphics.DrawString(item.Substring(0, 20),
                            //        printFont, Brushes.Black, Padding, y);
                            //    y += RowHeight;
                            //    e.Graphics.DrawString(item.Substring(20),
                            //        printFont, Brushes.Black, Padding, y);
                            //    y += RowHeight;
                            //}

                            e.Graphics.DrawString
                                ("\t\t" + dt.Rows[i]["TotalQuantity"].ToString(),
                                printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", decimal.Parse(dt.Rows[i]["TotalAmount"].ToString())),
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                RightAlign);
                            y += RowHeight;
                        }
                    }

                    y += RowHeight;
                }
                #endregion

                //Closing
                e.Graphics.DrawString("Closed By:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", closeLog.Cashier), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight + 40;
                e.Graphics.DrawLine(new Pen(Color.Black), 5, y, PageWidth - 5, y);
                y += RowHeight;
                e.Graphics.DrawString("Verified By:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", closeLog.Supervisor), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight + 40;
                e.Graphics.DrawLine(new Pen(Color.Black), 5, y, PageWidth - 5, y);
                y += RowHeight;
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print page failed!", E);
            }
        }



        private void printXreportPage(Object sender,
                        PrintPageEventArgs e)
        {

            try
            {

                bool enableNETSIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
                bool enableNETSCashCard = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCashCard), false);
                bool enableNETSFlashPay = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSFlashPay), false);
                bool enableNETSATMCard = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSATMCard), false);
                bool hideActualCollecton = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.HideActualCollection), false);

                Dictionary<string, decimal> ForeignCurrency = new Dictionary<string, decimal>();
                CashRecordingController crCtrl = new CashRecordingController();
                decimal CashIn, CashOut, OpeningAmount, totalPaid;
                decimal TotalCashColleted, TotalPay1Colleted, TotalPay2Colleted, TotalVoucherColleted, TotalPay4Colleted, TotalPay3Colleted;
                decimal TotalPay5Colleted, TotalPay6Colleted, TotalPay7Colleted, TotalPay8Colleted, TotalPay9Colleted, TotalPay10Colleted;
                decimal TotalChequeCollected, TotalPointCollected, TotalPackageCollected, TotalSMFCollected, TotalPAMedCollected, TotalPWFCollected;
                decimal TotalNETSCashCardCollected, TotalNETSFlashPayCollected, TotalNETSATMCardCollected, TotalForeignCurrency1, TotalForeignCurrency2;
                decimal TotalForeignCurrency3, TotalForeignCurrency4, TotalForeignCurrency5, TotalForeignCurrency;

                //getthepaymenttype
                ReceiptController.GetSystemCollectedBreakdownByPaymentType(
                _startTime, _endTime, PointOfSaleInfo.PointOfSaleID,
                 out TotalCashColleted,
                 out TotalPay1Colleted,
                 out TotalPay2Colleted,
                 out TotalVoucherColleted,
                 out TotalPay3Colleted,
                 out TotalPay4Colleted,
                 out TotalPay5Colleted,
                 out TotalPay6Colleted,
                 out TotalPay7Colleted,
                 out TotalPay8Colleted,
                 out TotalPay9Colleted,
                 out TotalPay10Colleted,
                 out TotalChequeCollected,
                 out TotalPointCollected,
                 out TotalPackageCollected,
                 out TotalSMFCollected,
                 out TotalPAMedCollected,
                 out TotalPWFCollected,
                 out TotalNETSCashCardCollected,
                 out TotalNETSFlashPayCollected,
                 out TotalNETSATMCardCollected,
                 out TotalForeignCurrency,
                 out ForeignCurrency);

                RowHeight = (int)printFont.GetHeight();

                int y = 0;
                System.Drawing.StringFormat RightAlign = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                RightAlign.Alignment = System.Drawing.StringAlignment.Far;

                System.Drawing.StringFormat CenterAlign = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                CenterAlign.Alignment = System.Drawing.StringAlignment.Center;
                //Set the variables

                e.Graphics.DrawString("Sales Summary Report", headerFont,
                                      Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), CenterAlign);
                y += RowHeight + 5;

                if (CompanyInfo.ReceiptName != "")
                {
                    e.Graphics.DrawString(CompanyInfo.ReceiptName, headerFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += (int)headerFont.GetHeight();
                }

                e.Graphics.DrawString("Counter - " + PointOfSaleInfo.PointOfSaleName, headerFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight + 5;

                e.Graphics.DrawString("From: " + _startTime.ToString("dd MMM yyyy HH:mm:ss"), printFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight;

                e.Graphics.DrawString("To: " + _endTime.ToString("dd MMM yyyy HH:mm:ss"), printFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight + 5;

                y += 10;

                e.Graphics.DrawString("Cashier:", printFont,
                                      Brushes.Black, Padding, y);
                e.Graphics.DrawString(UserInfo.username, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight + 5;

                crCtrl.fetchCashRecordingSummary(_startTime, _endTime, PointOfSaleInfo.PointOfSaleID, out CashIn, out CashOut, out OpeningAmount);
                decimal installmentAmount = ReceiptController.FetchTotalInstallment(_startTime, _endTime, PointOfSaleInfo.PointOfSaleID, out totalPaid);
                decimal Collection = ReceiptController.GetTotalSystemCollected(_startTime, _endTime, PointOfSaleInfo.PointOfSaleID);
                Collection += TotalVoucherColleted;
                Collection += (CashIn - CashOut);
                Collection -= installmentAmount;
                Collection -= totalPaid;
                Collection -= (TotalPointCollected + TotalPackageCollected);

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.ShowSalesWithoutCategoryFilter), false))
                {
                    string categoryfilterName = AppSetting.GetSetting(AppSetting.SettingsName.Print.CategoryFilterName);
                    DataTable dt = ReportController.FetchProductCategorySalesReport(
                        closeLog.StartTime, closeLog.EndTime,
                        closeLog.PointOfSale.PointOfSaleName,
                        "", categoryfilterName, "",
                        false, "CategoryName", "ASC");
                    decimal totalCategoryFilterSalesAmount = 0;
                    if (dt.Rows.Count > 0)
                    {
                        decimal.TryParse(dt.Rows[0]["TotalAmount"].ToString(), out totalCategoryFilterSalesAmount);
                    }
                    e.Graphics.DrawString("Sales Excl. " + categoryfilterName + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", Collection - totalCategoryFilterSalesAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    e.Graphics.DrawString(categoryfilterName + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", totalCategoryFilterSalesAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;


                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.ShowSalesWithoutItemDeptFilter), false))
                {
                    string itemDeptfilterName = AppSetting.GetSetting(AppSetting.SettingsName.Print.ItemDeptFilterName);
                    DataTable dt = ReportController.FetchDepartmentSalesReport(
                        closeLog.StartTime, closeLog.EndTime,
                        closeLog.PointOfSale.PointOfSaleName,
                        "", itemDeptfilterName,
                        false, "DepartmentName", "ASC");
                    decimal totalCategoryFilterSalesAmount = 0;
                    if (dt.Rows.Count > 0)
                    {
                        decimal.TryParse(dt.Rows[0]["TotalAmount"].ToString(), out totalCategoryFilterSalesAmount);
                    }
                    e.Graphics.DrawString("Sales Excl. " + itemDeptfilterName + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", Collection - totalCategoryFilterSalesAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    e.Graphics.DrawString(itemDeptfilterName + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", totalCategoryFilterSalesAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                }

                e.Graphics.DrawString("Total Sales:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", Collection + CashOut), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                y += 10;

                string tmp = string.Empty;

                string counttransaction = ClosingController.GetTotalNumberOfOrder(_startTime, _endTime,
                       PointOfSaleInfo.PointOfSaleID.ToString());

                e.Graphics.DrawString("No of Bill:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(counttransaction, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                y += 10;


                if (TotalForeignCurrency != 0)
                {
                    string deffCurr = AppSetting.GetSetting(AppSetting.SettingsName.Currency.DefaultCurrency);
                    string deffCurrSymbol = "$";
                    Currency deffCurrData = new Currency(Currency.Columns.CurrencyCode, deffCurr);
                    if (!deffCurrData.IsNew && !string.IsNullOrEmpty(deffCurrData.CurrencySymbol))
                        deffCurrSymbol = deffCurrData.CurrencySymbol;

                    e.Graphics.DrawString("Foreign Currency in " + deffCurr + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0}{1}", deffCurrSymbol, TotalForeignCurrency.ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                    y += 10;
                }

                if (PowerPOS.Feature.Package.isAvailable)
                {
                    e.Graphics.DrawString("Points Redemption:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", TotalPointCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                    y += 10;

                    e.Graphics.DrawString("Package Redemption:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", TotalPackageCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                    y += 10;
                }
                
                decimal GSTRecorded = ClosingController.GetTotalGSTRecorded(_startTime, _endTime, PointOfSaleInfo.PointOfSaleID.ToString());
                if (GSTRecorded > 0)
                {
                    e.Graphics.DrawString("Total GST:", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", GSTRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    y += 10;
                }

                e.Graphics.DrawString("Installment:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", installmentAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;
                y += 10;

                e.Graphics.DrawString("System Recorded Collection:", printFont, Brushes.Black, Padding, y);
                y += RowHeight + 5;
                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += RowHeight;

                e.Graphics.DrawString("CASH:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", TotalCashColleted), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;
                y += 10;

                int count = 0;
                foreach (var item in ForeignCurrency)
                {
                    count++;
                    if (count == 1)
                    {
                        Currency curr = new Currency(Currency.Columns.CurrencyCode, item.Key.Replace("CASH-", ""));
                        string currSymbol = "$";
                        if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                            currSymbol = curr.CurrencySymbol;
                        e.Graphics.DrawString(item.Key + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(string.Format("{0}{1}", currSymbol, item.Value.ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }

                    if (count == 2)
                    {
                        Currency curr = new Currency(Currency.Columns.CurrencyCode, item.Key.Replace("CASH-", ""));
                        string currSymbol = "$";
                        if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                            currSymbol = curr.CurrencySymbol;
                        e.Graphics.DrawString(item.Key + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(string.Format("{0}{1}", currSymbol, item.Value.ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }

                    if (count == 3)
                    {
                        Currency curr = new Currency(Currency.Columns.CurrencyCode, item.Key.Replace("CASH-", ""));
                        string currSymbol = "$";
                        if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                            currSymbol = curr.CurrencySymbol;
                        e.Graphics.DrawString(item.Key + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(string.Format("{0}{1}", currSymbol, item.Value.ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }

                    if (count == 4)
                    {
                        Currency curr = new Currency(Currency.Columns.CurrencyCode, item.Key.Replace("CASH-", ""));
                        string currSymbol = "$";
                        if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                            currSymbol = curr.CurrencySymbol;
                        e.Graphics.DrawString(item.Key + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(string.Format("{0}{1}", currSymbol, item.Value.ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }

                    if (count == 5)
                    {
                        Currency curr = new Currency(Currency.Columns.CurrencyCode, item.Key.Replace("CASH-", ""));
                        string currSymbol = "$";
                        if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                            currSymbol = curr.CurrencySymbol;
                        e.Graphics.DrawString(item.Key + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(string.Format("{0}{1}", currSymbol, item.Value.ToString("N2")), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }
                }

                if (enableNETSATMCard)
                {
                    e.Graphics.DrawString(POSController.PAY_NETSATMCard + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", TotalNETSATMCardCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                    y += 10;
                }
                if (enableNETSCashCard)
                {
                    e.Graphics.DrawString(POSController.PAY_NETSCashCard + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", TotalNETSCashCardCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                    y += 10;
                }
                if (enableNETSFlashPay)
                {
                    e.Graphics.DrawString(POSController.PAY_NETSFlashPay + ":", printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", TotalNETSFlashPayCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                    y += 10;
                }

                tmp = PaymentTypesController.FetchPaymentByID("1");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if ((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration)
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", TotalPay1Colleted), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }
                }
                tmp = PaymentTypesController.FetchPaymentByID("2");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if ((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration)
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", TotalPay2Colleted), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }
                }
                tmp = PaymentTypesController.FetchPaymentByID("3");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if ((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration)
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", TotalPay3Colleted), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }
                }
                tmp = PaymentTypesController.FetchPaymentByID("4");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if ((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration)
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", TotalPay4Colleted), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }
                }
                tmp = PaymentTypesController.FetchPaymentByID("5");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if ((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration)
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", TotalPay5Colleted), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }
                }
                tmp = PaymentTypesController.FetchPaymentByID("6");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if ((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration)
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", TotalPay6Colleted), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }
                }

                tmp = PaymentTypesController.FetchPaymentByID("7");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if ((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration)
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", TotalPay7Colleted), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }
                }

                tmp = PaymentTypesController.FetchPaymentByID("8");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if ((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration)
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", TotalPay8Colleted), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }
                }

                tmp = PaymentTypesController.FetchPaymentByID("9");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if ((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration)
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", TotalPay9Colleted), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }
                }

                tmp = PaymentTypesController.FetchPaymentByID("10");
                if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
                {
                    if ((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNETSIntegration)
                    {
                        e.Graphics.DrawString(tmp + ":", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", TotalPay10Colleted), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        y += 10;
                    }
                }

                e.Graphics.DrawString("CHEQUE:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", TotalChequeCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;
                y += 10;

                y += RowHeight;
                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += RowHeight;
                y += 10;

                e.Graphics.DrawString("Opening Balance:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", OpeningAmount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Cash In:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", CashIn), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Cash Out:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", CashOut), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                // e.Graphics.DrawString("Closing Cash Out:", printFont, Brushes.Black, Padding, y);
                // e.Graphics.DrawString(String.Format("{0:N}", closeLog.ClosingCashOut), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                // y += RowHeight;

                // e.Graphics.DrawString("Deposit Bag No:", printFont, Brushes.Black, Padding, y);
                // e.Graphics.DrawString(String.Format("{0:N}", closeLog.DepositBagNo), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                // y += RowHeight;

                // y += 10;

                // e.Graphics.DrawString("Surplus (+/-):", printFont, Brushes.Black, Padding, y);
                // e.Graphics.DrawString(String.Format("{0:N}", closeLog.Variance), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                // y += RowHeight;

                // y += 3;
                #region Print Discount

                if (PrintDiscount)
                {
                    DataTable discDT = ReportController.FetchDiscountReport(_startTime, _endTime, PointOfSaleInfo.PointOfSaleID);
                    if (discDT != null && discDT.Rows.Count > 0 && discDT.Columns.Count > 1)
                    {
                        y += RowHeight;

                        e.Graphics.DrawString("Discount Price : ", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", ((decimal)discDT.Rows[0][1])), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        e.Graphics.DrawString("Discount (%) : ", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", ((decimal)discDT.Rows[0][0])), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                }

                #endregion

                #region *) Copied from Restaurant: Print Product Saved Report
                if (PrintItemDepartment)
                {
                    DataTable dt = ReportController.FetchDepartmentSalesReport(
                        _startTime, _endTime,
                        PointOfSaleInfo.PointOfSaleName,
                        "", "",
                        false, "DepartmentName", "ASC");

                    if (dt != null)
                    {
                        y += RowHeight;

                        e.Graphics.DrawString("Department Sales Report", headerFont,
                          Brushes.Black, Padding, y);
                        y += RowHeight + 5;

                        e.Graphics.DrawString
                        ("Department",
                            printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString
                            ("\t\t" + "Qty",
                                printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString("Total",
                            printFont, Brushes.Black,
                            new RectangleF(Padding, y, PageWidth - Padding, 30),
                            RightAlign);
                        y += RowHeight;
                        string item;
                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            item = dt.Rows[i]["DepartmentName"].ToString();
                            int TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                            e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                            y += TextHeight;
                            e.Graphics.DrawString
                                ("\t\t" + dt.Rows[i]["TotalQuantity"].ToString(),
                                printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", decimal.Parse(dt.Rows[i]["TotalAmount"].ToString())),
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                RightAlign);
                            y += RowHeight;
                        }
                    }
                }

                if (PrintProductSalesReport)
                {
                    string categoryName = "";
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.UseCategoryFilterOnClosingReport), false))
                    {
                        categoryName = AppSetting.GetSetting(AppSetting.SettingsName.Print.CategoryFilterName) == null ? "" : AppSetting.GetSetting(AppSetting.SettingsName.Print.CategoryFilterName);
                    }
                    DataTable dt = ReportController.FetchProductCategorySalesReport(
                        _startTime, _endTime,
                        PointOfSaleInfo.PointOfSaleName,
                        "", categoryName, "",
                        false, "CategoryName", "ASC");

                    if (dt != null)
                    {
                        y += RowHeight;

                        e.Graphics.DrawString("Category Sales Report", headerFont,
                          Brushes.Black, Padding, y);
                        y += RowHeight + 5;

                        e.Graphics.DrawString
                        ("Category",
                            printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString
                            ("\t\t" + "Qty",
                                printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString("Total",
                            printFont, Brushes.Black,
                            new RectangleF(Padding, y, PageWidth - Padding, 30),
                            RightAlign);
                        y += RowHeight;
                        string item;
                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            item = dt.Rows[i]["CategoryName"].ToString();
                            int TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                            e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                            y += TextHeight;

                            e.Graphics.DrawString
                                ("\t\t" + dt.Rows[i]["TotalQuantity"].ToString(),
                                printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", decimal.Parse(dt.Rows[i]["TotalAmount"].ToString())),
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                RightAlign);
                            y += RowHeight;
                        }
                    }
                    y += RowHeight;
                    dt = ReportController.FetchProductSalesReport
                        (_startTime, _endTime, "",
                        PointOfSaleInfo.PointOfSaleName, "", categoryName,
                        "", false,
                        "CategoryName", "ASC");
                    if (dt != null)
                    {
                        string item;
                        string categoryname = "";

                        e.Graphics.DrawString("Product Sales Report", headerFont,
                            Brushes.Black, Padding, y);

                        y += RowHeight + 5;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (categoryname != dt.Rows[i]["CategoryName"].ToString())
                            {
                                categoryname = dt.Rows[i]["CategoryName"].ToString();
                                if (categoryname.Length < 20)
                                {

                                    e.Graphics.DrawString(categoryname,
                                        printFont, Brushes.Black, Padding, y);
                                    y += RowHeight;
                                }
                                else
                                {
                                    e.Graphics.DrawString(categoryname.Substring(0, 20),
                                    printFont, Brushes.Black, Padding, y);
                                    y += RowHeight;
                                    e.Graphics.DrawString(categoryname.Substring(20),
                                        printFont, Brushes.Black, Padding, y);
                                    y += RowHeight;
                                }
                                e.Graphics.DrawString
                                ("Item", printFont, Brushes.Black, Padding, y);
                                e.Graphics.DrawString
                                    ("\t\t" + "Qty", printFont, Brushes.Black, Padding, y);
                                e.Graphics.DrawString("Total",
                                    printFont, Brushes.Black,
                                    new RectangleF(Padding, y, PageWidth - Padding, 30),
                                    RightAlign);
                                y += RowHeight;
                                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                            }
                            item = dt.Rows[i]["ItemNo"].ToString() + "-" + dt.Rows[i]["ItemName"].ToString();
                            int TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                            e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                            y += TextHeight;

                            e.Graphics.DrawString
                                ("\t\t" + dt.Rows[i]["TotalQuantity"].ToString(),
                                printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", decimal.Parse(dt.Rows[i]["TotalAmount"].ToString())),
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                RightAlign);
                            y += RowHeight;
                        }
                    }

                    y += RowHeight;
                }
                #endregion

                //Closing
                //e.Graphics.DrawString("Closed By:", printFont, Brushes.Black, Padding, y);
                //e.Graphics.DrawString(String.Format("{0:N}", UserInfo.username), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                //y += RowHeight + 40;
                e.Graphics.DrawLine(new Pen(Color.Black), 5, y, PageWidth - 5, y);
                y += RowHeight;
                //e.Graphics.DrawString("Verified By:", printFont, Brushes.Black, Padding, y);
                //e.Graphics.DrawString(String.Format("{0:N}", closeLog.Supervisor), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                //y += RowHeight + 40;
                //e.Graphics.DrawLine(new Pen(Color.Black), 5, y, PageWidth - 5, y);
                //y += RowHeight;
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print page failed!", E);
            }
        }
        #region "Old Yifei Code"
        public void PrintReceipt(String[] items, Double[] prices, int[] qty, int N)
        {
            try
            {
                n = N;
                Items = new String[n];
                Prices = new Double[n];
                Qty = new int[n];

                for (int i = 0; i < n; i++)
                {
                    Items[i] = items[i];
                    Prices[i] = prices[i];
                    Qty[i] = qty[i];
                }

                PrintDocument printDoc = new PrintDocument();
                if (PrinterName.CompareTo("") != 0)
                {
                    printDoc.PrinterSettings.PrinterName = PrinterName;
                }
                printDoc.PrintPage += new PrintPageEventHandler(printReceiptPage);
                printDoc.Print();
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print Receipt failed!", E);
            }
        }

        public void PrintCounterCloseCollection(CounterCloseLog clog, bool printProductSalesReport, bool printDiscount)
        {
            try
            {
                PrintProductSalesReport = printProductSalesReport;
                PrintDiscount = printDiscount;
                closeLog = clog;
                PrintItemDepartment = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintItemDepartmentOnCheckOut), false);
                /*Counter = counter;
                Cashier = cashier;
                Period = period;
                Collection = collection;
                ActualCollected = floatBalance;
                OpeningAmount = openingAmount;
                CashIn = cashIn;
                CashOut = cashOut;*/
                string rptSetting = AppSetting.GetSetting(AppSetting.SettingsName.Print.ClosingReceiptFileLocation);

                if (rptSetting == "" || rptSetting == null)
                {
                    PrintDocument printDoc = new PrintDocument();
                    if (PrinterName.CompareTo("") != 0)
                    {
                        printDoc.PrinterSettings.PrinterName = PrinterName;
                    }
                    PrintController printController = new StandardPrintController();
                    printDoc.PrintController = printController;
                    printDoc.PrintPage += new PrintPageEventHandler(printCollectionPage);
                    printDoc.Print();
                }
                else
                {
                    int NumOfCopies = 0;
                    int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Print.ClosingReceiptNumOfCopies), out NumOfCopies);
                    GenericReport.NewPrint.PrintController.PrintClosingReport(PrinterName, NumOfCopies, closeLog, false);
                }
            }
            catch (Exception e)
            {
                throw new PrintReceiptException("Print Cash Collection failed!", e);
            }
        }
        public void PrintXReport(DateTime startDate, DateTime endDate, bool printProductSalesReport, bool printDiscount)
        {
            try
            {
                PrintProductSalesReport = printProductSalesReport;
                PrintDiscount = printDiscount;
                _startTime = startDate;
                _endTime = endDate;

                PrintDocument printDoc = new PrintDocument();
                if (PrinterName.CompareTo("") != 0)
                {
                    printDoc.PrinterSettings.PrinterName = PrinterName;
                }
                printDoc.PrintPage += new PrintPageEventHandler(printXreportPage);
                printDoc.Print();

            }
            catch (Exception e)
            {
                throw new PrintReceiptException("Print Cash Collection failed!", e);
            }
        }
        private void printReceiptPage(Object sender,
                PrintPageEventArgs e)
        {
            try
            {
                int y = 0;
                Double Total = 0;

                System.Drawing.StringFormat RightAlign = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                RightAlign.Alignment = System.Drawing.StringAlignment.Far;

                System.Drawing.StringFormat CenterAlign = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                CenterAlign.Alignment = System.Drawing.StringAlignment.Center;

                if (Company.CompareTo("") != 0)
                {
                    e.Graphics.DrawString(Company, headerFont,
                                          Brushes.Black, Padding, y);
                    y += RowHeight + 5;
                }

                if (ReceiptHeader.CompareTo("") != 0)
                {
                    e.Graphics.DrawString(ReceiptHeader, headerFont,
                          Brushes.Black, Padding, y);
                    y += RowHeight + 5;
                }

                //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 10;

                for (int i = 0; i < n; i++)
                {
                    e.Graphics.DrawString(Items[i], printFont, Brushes.Black, Padding, y);
                    y += RowHeight;

                    Total += Prices[i] * Qty[i];
                    e.Graphics.DrawString(String.Format("{0:N}", Prices[i]) + "\t\t" + Qty[i], printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", Prices[i] * Qty[i]), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight + 5;
                }

                //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 10;

                if (GST == 0)
                {
                    e.Graphics.DrawString("Total: " + String.Format("{0:N}", Total), totalFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                }
                else
                {
                    if (PriceIncludeGST)
                    {
                        e.Graphics.DrawString("Total: " + String.Format("{0:N}", Total), totalFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        e.Graphics.DrawString("Inclusive GST " + GST * 100 + "%: " + String.Format("{0:N}", GST * Total), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                    else
                    {
                        e.Graphics.DrawString("Sub Total: " + String.Format("{0:N}", Total), totalFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        e.Graphics.DrawString("GST " + GST * 100 + "%: " + String.Format("{0:N}", Total * GST), totalFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                        //e.Graphics.DrawLine(new Pen(Color.Black), PageWidth / 2, y, PageWidth, y);
                        y += 10;
                        e.Graphics.DrawString("Total: " + String.Format("{0:N}", Total * (1 + GST)), totalFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                }
                y += RowHeight;

                if (ReceiptFooter.CompareTo("") != 0)
                {
                    e.Graphics.DrawString(ReceiptFooter, printFont,
                          Brushes.Black, 10, y);
                    y += 30;
                }
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print page failed!", E);
            }
        }
        #endregion

        public void PrintQuotationAHAVAReceipt
            (QuoteController mypos, decimal changeAmount, ReceiptSizes size, int NumOfCopies)
        {
            try
            {

                string prt = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.A4PrinterName);
                if (prt != null && prt != "")
                {
                    qos = mypos;
                    //for (int i = 0; i < NumOfCopies; i++)
                    GenericReport.NewPrint.PrintController.PrintInvoiceQuotation(prt, NumOfCopies, GenericReport.PageType.A4, qos, reprint);
                    //GenericReport.PageController.GenerateReport(pos, GenericReport.PageType.A4, false, GenericReport.ReportType.INVOICE);                   
                }
            }
            catch (Exception E)
            {
                Logger.writeLog("Print Receipt failed! " + E.Message);
                return;
            }
        }

        public Receipt()
        {
            try
            {
                if (ReceiptSetup.PrinterName == null) ReceiptSetup.LoadReceiptSetup();

                PrinterName = ReceiptSetup.PrinterName;
                PrinterNameValidation = ReceiptSetup.PrinterNameValidation;
                Company = ReceiptSetup.Company;
                PageWidth = ReceiptSetup.PageWidth;
                Padding = ReceiptSetup.Padding;
                companyFont = ReceiptSetup.companyFont;
                headerFont = ReceiptSetup.headerFont;
                printFont = ReceiptSetup.printFont;
                printFontBold = ReceiptSetup.printFontBold;
                largerPrintFont = ReceiptSetup.largerPrintFont;
                largerPrintFontBold = ReceiptSetup.largerPrintFontBold;
                termsConditionFont = ReceiptSetup.termsConditionFont;
                BarcodeFont = ReceiptSetup.BarcodeFont;
                totalFont = ReceiptSetup.totalFont;
                RowHeight = ReceiptSetup.RowHeight;
                GST = ReceiptSetup.GST;
                PriceIncludeGST = ReceiptSetup.PriceIncludeGST;
                ReceiptHeader = ReceiptSetup.ReceiptHeader;
                ReceiptFooter = ReceiptSetup.ReceiptFooter;
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Initialise Receipt failed!", E);
            }
        }
    }

}
