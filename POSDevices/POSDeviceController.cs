using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SubSonic;
using PowerPOS;
using PowerPOS.Container;
using System.IO;
using System.Configuration;
using System.Data;

/// <remarks>
///     *) Fully Migrated
/// </remarks>
namespace POSDevices
{
    public enum ReceiptSizes
    {
        A4 = 0,
        Receipt = 1,
        A5 = 2
    }
    public class POSDeviceController
    {
        public static void PrintAgapeBabySticker(POSController mypos, int NumOfCopies)
        {
            Receipt rcpt = new Receipt();
            rcpt.reprint = false;
            rcpt.PrintAgapeBabySticker(mypos, NumOfCopies);
        }

        public static void PrintFormalAHAVATransactionReceipt(POSController mypos, decimal change, bool reprint, ReceiptSizes size, int NumOfCopies, bool OnlySaveCopy)
        {
            //dont allow printing receipt if it is not set as always print, unless if it is reprint
            //if (!reprint && !PrintSettingInfo.receiptSetting.PrintReceipt)
            //    return;
            Receipt rcpt = new Receipt();
            rcpt.reprint = reprint;
            rcpt.PrintFormalAHAVAReceiptOnlyCopy(mypos, change, size, NumOfCopies);
        }

        public static void PrintAHAVATransactionReceipt(POSController mypos, decimal change, bool reprint, ReceiptSizes size, int NumOfCopies)
        {
            PrintAHAVATransactionReceipt(mypos, change, reprint, size, NumOfCopies, false);
        }

        public static void PrintQuotation(QuoteController mypos, decimal change, bool reprint, ReceiptSizes size, int NumOfCopies)
        {
            //dont allow printing receipt if it is not set as always print, unless if it is reprint
            if (!reprint && !PrintSettingInfo.receiptSetting.PrintReceipt)
                return;
            Receipt rcpt = new Receipt();
            rcpt.reprint = reprint;
            rcpt.PrintQuotationAHAVAReceipt(mypos, change, size, NumOfCopies);
        }

        public static void PrintAHAVATransactionReceipt(POSController mypos, decimal change, bool reprint, ReceiptSizes size, int NumOfCopies, bool printToAdditionalPrinter)
        {
            Receipt rcpt = new Receipt();
            rcpt.reprint = reprint;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.UseAdditionalPrinter), false) && !reprint)
            {
                //Print to additional Printer
                rcpt.PrintToAdditionalPrinter(mypos, change, size, 1);
            }
            //dont allow printing receipt if it is not set as always print, unless if it is reprint
            if (!reprint && !PrintSettingInfo.receiptSetting.PrintReceipt)
                return;

            if (printToAdditionalPrinter)
                rcpt.PrintToAdditionalPrinter(mypos, change, size, NumOfCopies);
            else
                rcpt.PrintAHAVAReceipt(mypos, change, size, NumOfCopies);
            
            if (mypos.IsADelivery)
            {
                bool PrintInvoiceOnDelivery = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintInvoiceOnDelivery), false);
                if (PrintInvoiceOnDelivery)
                {
                    POSController poscopy = mypos;
                    poscopy.SetAsDelivery(false);
                    rcpt.PrintAHAVAReceipt(poscopy, change, size, NumOfCopies);
                }
            }
            

        }

        public static bool SendMailReceipt(POSController myPos, string mailTo, string subject, string bodyMail, string mailBcc)
        {
            bool isSuccess = false;
            try
            {
                Receipt rcpt = new Receipt();
                rcpt.reprint = true;
                var orderHdr = myPos.myOrderHdr;
                var streamPDF = rcpt.GetReceiptPDFStream(myPos);
                var attachment = new Dictionary<string, Stream>();
                attachment.Add("receipt.pdf", streamPDF);

                #region *) Pre Order Receipt
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SendPreOrderReceiptToEmail), false))
                {
                    OrderDetCollection detcol = myPos.myOrderDet;
                    int i = 1;
                    foreach (var it in detcol)
                    {
                        if (!it.IsPreOrder.GetValueOrDefault(false)) continue;
                        var streamPdfDet = rcpt.GetPreOrderMail(it.OrderHdrID, it.OrderDetID);
                        string ereceiptno = "";
                        if (!string.IsNullOrEmpty(it.LineInfo))
                        {
                            ereceiptno = string.Format("{0}.{1}", it.LineInfo, i.ToString());
                        }
                        else
                        {
                            ereceiptno = string.Format("{0}.{1}", it.OrderHdrID, i.ToString());
                        }
                        attachment.Add(string.Format("{0}.pdf", ereceiptno), streamPdfDet);
                        i++;
                    }
                }
                #endregion

                string status = "";
                var ms = new MassEmail();
                var result = ms.SendEmails(
                    new ArrayList { mailTo },
                    AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail),
                    subject,
                    bodyMail,
                    bodyMail,
                    AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SMTP),
                    AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Username),
                    AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Password),
                    false,
                    attachment, AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Port), 
                    mailBcc,
                    out status);
                isSuccess = result!=null && result.Count == 0;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send Mail Receipt failed! " + ex.Message);
            }

            return isSuccess;
        }

        public static bool SendMailNotifyDelivery(POSController myPos, string mailTo, string subject, string bodyMail, string mailBcc)
        {
            bool isSuccess = false;
            try
            {
                //Receipt rcpt = new Receipt();
                var orderHdr = myPos.myOrderHdr;
                var attachment = new Dictionary<string, Stream>();
                string itemstring = "";

                OrderDetCollection detcol = myPos.myOrderDet;
                int i = 1;
                foreach (var it in detcol)
                {
                    var streamPdfDet = Receipt.GetPreOrderNotifyMail(it.OrderHdrID, it.OrderDetID);
                    string ereceiptno = "";
                    if (it.LineInfo != null && !string.IsNullOrEmpty(it.LineInfo))
                    {
                        ereceiptno = string.Format("{0}.{1}", it.LineInfo, i.ToString());
                    }
                    else
                    {
                        ereceiptno = it.OrderHdrID;
                    }
                    attachment.Add(string.Format("{0}.pdf", ereceiptno), streamPdfDet);
                    i++;

                    itemstring += string.Format("{0} - {1},", it.Item.ItemName, it.Quantity);
                }

                //** setting body mail */
                //Membership member = new Membership(myPos.myOrderHdr.MembershipNo);

                ReceiptSettingCollection TheAddress = new ReceiptSettingCollection();
                TheAddress.Load();
                string CompanyAddress = "";
                if (TheAddress[0].ReceiptAddress1 != null && TheAddress[0].ReceiptAddress1 != "") CompanyAddress = CompanyAddress + "~" + TheAddress[0].ReceiptAddress1;
                if (TheAddress[0].ReceiptAddress2 != null && TheAddress[0].ReceiptAddress2 != "") CompanyAddress = CompanyAddress + "~" + TheAddress[0].ReceiptAddress2;
                if (TheAddress[0].ReceiptAddress3 != null && TheAddress[0].ReceiptAddress3 != "") CompanyAddress = CompanyAddress + "~" + TheAddress[0].ReceiptAddress3;
                if (TheAddress[0].ReceiptAddress4 != null && TheAddress[0].ReceiptAddress4 != "") CompanyAddress = CompanyAddress + "~" + TheAddress[0].ReceiptAddress4;
                CompanyAddress = CompanyAddress.Trim('~').Replace("~", Environment.NewLine);

                CompanyAddress = string.IsNullOrEmpty(CompanyAddress) ? "our office" : CompanyAddress;

                DateTime datedelivered = DateTime.Now;

                DeliveryOrderCollection orderhdr = new DeliveryOrderCollection();
                orderhdr.Where(DeliveryOrder.Columns.SalesOrderRefNo, myPos.myOrderHdr.OrderHdrID);
                orderhdr.OrderByDesc(DeliveryOrder.Columns.OrderNumber);
                orderhdr.Load();

                if (orderhdr.Count > 0 && orderhdr[0].DeliveryDate != null)
                {
                    datedelivered = (DateTime) orderhdr[0].DeliveryDate;
                }


                bodyMail = "Dear Sir / Madam, <br/>";
                bodyMail += string.Format("You have preordered item {0}. The item are ready to collect at {1} from {2} onwards.<br/>", itemstring.Substring(0, itemstring.Length - 1), CompanyAddress, datedelivered.ToString("dd MMMM yyyy"));
                bodyMail += "Thank you";


                string status = "";
                var ms = new MassEmail();
                var result = ms.SendEmails(
                    new ArrayList { mailTo },
                    AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail),
                    subject,
                    bodyMail,
                    bodyMail,
                    AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SMTP),
                    AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Username),
                    AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Password),
                    false,
                    attachment, AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Port),
                    mailBcc,
                    out status);
                isSuccess = result != null && result.Count == 0;

                if (isSuccess)
                {
                    foreach (OrderDet det in detcol)
                    {
                        det.IsNotified = true;
                        det.Save(UserInfo.username);

                        if (!PointOfSaleInfo.IntegrateWithInventory)
                        {
                            DataTable dt = SyncClientController.GetDataTableStructureForUpdate();
                            dt.Rows.Add("OrderDet", "OrderDetID", det.OrderDetID, "System.String", OrderDet.UserColumns.IsNotified, true.ToString(), "System.Boolean");
                            dt.Rows.Add("OrderDet", "OrderDetID", det.OrderDetID, "System.String", OrderDet.Columns.ModifiedOn, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "System.DateTime");
                            dt.Rows.Add("OrderDet", "OrderDetID", det.OrderDetID, "System.String", OrderDet.Columns.ModifiedBy, UserInfo.username, "System.String");
                            if (!SyncClientController.ExecuteUpdateInServer(dt))
                            {
                                Logger.writeLog("Failed to update OrderDet.IsNotified in server.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send Mail Receipt failed! " + ex.Message);
            }

            return isSuccess;
        }

        public static void PrintPackageLogReceipt(PackageRedemptionLog PrintItem, bool reprint)
        {
            if (!reprint && !PrintSettingInfo.receiptSetting.PrintReceipt)
                return;
            Receipt rcpt = new Receipt();
            rcpt.PrintPackageRedemption(PrintItem);
        }

        public static void PrintCloseCounterReport(CounterCloseLog myCounter, bool PrintProductSalesReport, bool printDiscount)
        {
            Receipt rcpt = new Receipt();
            rcpt.PrintCounterCloseCollection(myCounter, PrintProductSalesReport, printDiscount);
        }

        public static void PrintXReport(DateTime startTime, DateTime  endTime, bool PrintProductSalesReport, bool PrintDiscount)
        {
            Receipt rcpt = new Receipt();
            
            rcpt.PrintXReport(startTime, endTime, PrintProductSalesReport, PrintDiscount);
        }

        public static void PrintCashRecordingSlip(
            CashRecording myCashRecording)
        {
            if (myCashRecording == null || myCashRecording.CashRecordingType.CashRecordingTypeName.ToLower() == "opening balance")
            {
                return;
            }
            Receipt rcpt = new Receipt();
            rcpt.myCashRecording = myCashRecording;
            rcpt.PrintCashRecordingSlip();
        }
        /*
        public static void PrintTransactionReceipt(OrderDetCollection myDet)
        {
            Receipt rcpt = new Receipt();
            ArrayList itemnos = new ArrayList();
            ArrayList itemprices = new ArrayList();
            ArrayList qty = new ArrayList();             
            for (int k = 0; k < myDet.Count; k++)
            {
                itemnos.Add(new Item("itemno", myDet[k].ItemNo.ToString()).ItemName.ToString());
                itemprices.Add((double)myDet[k].UnitPrice);
                qty.Add((int)myDet[k].Quantity);
            }
            string[] myItemNames = new string[myDet.Count];
            double[] myPrices = new double[myDet.Count];
            int[] myqtys = new int[myDet.Count];
            itemnos.CopyTo(myItemNames);
            itemprices.CopyTo(myPrices);
            qty.CopyTo(myqtys);

            rcpt.PrintReceipt(myItemNames, myPrices, myqtys, myItemNames.Length);
        }
         */

        #region z2closing
        /*public static void PrintChangChengCloseCounterReport(CounterCloseLog myCounter, bool printCategorySalesReport, bool printProductSalesReport)
        {
            try
            {
                Receipt rcpt = new Receipt();
                rcpt.PrintCategorySalesReport = printCategorySalesReport;
                rcpt.PrintProductSalesReport = printProductSalesReport;
                rcpt.PrintChangChengCashCollection(myCounter);
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error printing CCMW Close Counter Receipt:");
                Logger.writeLog(ex);
            }
        }*/

        public static void PrintZ2Report(Z2ClosingLog z2LogReport)
        {
            try
            {
                Receipt rcpt = new Receipt();
                rcpt.PrintZ2Report(z2LogReport);
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error printing Close Counter Receipt:");
                Logger.writeLog(ex);
            }
        }

        public static void PrintEJCloseCounterReport(CounterCloseLog myCounter)
        {
            try
            {
                Receipt rcpt = new Receipt();
                rcpt.PrintEJReport(myCounter);
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error printing CCMW Close Counter Receipt:");
                Logger.writeLog(ex);
            }
        } 

        #endregion

        #region SICC 
        public static void PrintAHAVAValidationReceipt(POSController mypos, decimal change, bool reprint, ReceiptSizes size, int NumOfCopies)
        {
            Receipt rcpt = new Receipt();
            rcpt.reprint = reprint;
            rcpt.PrintAHAVAValidationReceipt(mypos, change, size, NumOfCopies);
        }
        #endregion
    }
}
