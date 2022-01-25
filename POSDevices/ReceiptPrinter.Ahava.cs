using System;
using System.Collections.Generic;
using System.Text;
using PowerPOS;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Data;
using PowerPOS.Container;
using SubSonic;
using System.Collections;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using System.Reflection;
using Newtonsoft.Json;
using PowerPOSLib.Controller;
using PowerPOS.Nets;

/// <remarks>
///     *) Fully Migrated
/// </remarks>
namespace POSDevices
{
    public partial class Receipt
    {
        public CashRecording myCashRecording;
        private PackageRedemptionLog myPackageLog;
        public bool reprint = false;
        public void PrintInstallmentReceipt
            (Installment myHdr, InstallmentDetailCollection myDet)
        {
            try
            {
                InstallmentRpt receiptA41 = new InstallmentRpt();
                DataSet ds1 = new DataSet();
                InstallmentCollection tmp = new InstallmentCollection();
                tmp.Add(myHdr);
                ds1.Tables.Add(tmp.ToDataTable(
                    ));
                ds1.Tables.Add(myDet.ToDataTable());
                MembershipCollection mbr = new MembershipCollection();
                mbr.Where(Membership.Columns.MembershipNo, myHdr.MembershipNo);
                mbr.Load();
                ds1.Tables.Add(mbr.ToDataTable());
                receiptA41.SetDataSource(ds1);
                DataSet ds = GetCompanyInfoAddressDataSet();
                receiptA41.Subreports["CompanyInfoReport.rpt"].SetDataSource(ds);
                receiptA41.DataDefinition.FormulaFields["InstallmentRefNo"].Text = "\"" + myHdr.InstallmentRefNo + "\"";
                receiptA41.PrintToPrinter(1, false, 0, 0);

            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print Receipt failed!", E);
            }
        }

        public void PrintAgapeBabySticker
            (POSController mypos, int NumOfCopies)
        {
            try
            {
                {
                    pos = mypos;
                    change = 0;
                    PrintDocument printDoc = new PrintDocument();
                    if (PrinterName.CompareTo("") != 0)
                    {
                        printDoc.PrinterSettings.PrinterName = PrinterName;
                    }
                    printDoc.PrintPage += new PrintPageEventHandler(HandlerAgapeBabySticker);
                    for (int i = 0; i < NumOfCopies; i++)
                    {
                        printDoc.Print();
                    }
                }
            }
            catch (Exception E)
            {
                Logger.writeLog("Print Receipt failed! " + E.Message);
                return;
            }
        }


        private void HandlerAgapeBabySticker(Object sender,
           PrintPageEventArgs e)
        {
            try
            {
                int PageHeight = e.MarginBounds.Height;
                int y = 0;

                //Double Total = 0;
                int RowHeight = (int)printFont.GetHeight();

                Membership Destination = pos.GetMemberInfo();

                if (Destination.MembershipNo == "WALK-IN")
                { Logger.writeLog("Print Delivery Sticker terminated - Membership is WALK IN"); return; }

                #region *) Print Destination
                PrintLine(ref e, ref y, companyFont, "DELIVER TO :");
                PrintLine(ref e, ref y, printFont, Destination.NameToAppear);
                PrintLine(ref e, ref y, printFont, Destination.Block);
                PrintLine(ref e, ref y, printFont, Destination.StreetName);
                PrintLine(ref e, ref y, printFont, Destination.StreetName2);
                PrintLine(ref e, ref y, printFont, Destination.UnitNo);
                PrintLine(ref e, ref y, printFont, Destination.BuildingName);
                PrintLine(ref e, ref y, printFont, ((Destination.City == null || Destination.City == "") ? "Singapore" : Destination.City) + " " + (Destination.ZipCode == null ? "" : Destination.ZipCode));
                PrintLine(ref e, ref y, printFont, ((Destination.Mobile != null && Destination.Mobile != "") ? "Tel " + Destination.Mobile : ""));
                #endregion

                y += RowHeight;

                #region *) Print Sender
                PrintLine(ref e, ref y, companyFont, "FROM :");
                PrintLine(ref e, ref y, printFont, PrintSettingInfo.receiptSetting.ReceiptAddress1);
                PrintLine(ref e, ref y, printFont, PrintSettingInfo.receiptSetting.ReceiptAddress2);
                PrintLine(ref e, ref y, printFont, PrintSettingInfo.receiptSetting.ReceiptAddress3);
                PrintLine(ref e, ref y, printFont, PrintSettingInfo.receiptSetting.ReceiptAddress4);
                #endregion
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print page failed!", E);
            }
        }

        private void PrintLine(ref PrintPageEventArgs e, ref int y, Font PrintedFont, string PrintedItem)
        {
            if (PrintedItem == null || PrintedItem.Trim() == "") return;

            int TextHeight = (int)e.Graphics.MeasureString(PrintedItem, printFont, PageWidth).Height;
            e.Graphics.DrawString(PrintedItem, PrintedFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
            y += TextHeight;
        }

        public void PrintAHAVAReceipt
            (POSController mypos, decimal changeAmount, ReceiptSizes size, int NumOfCopies)
        {
            try
            {
                if (size == ReceiptSizes.Receipt)
                {
                    pos = mypos;
                    change = changeAmount;
                    PrintDocument printDoc = new PrintDocument();
                    if (PrinterName.CompareTo("") != 0)
                    {
                        printDoc.PrinterSettings.PrinterName = PrinterName;
                    }
                    printDoc.PrintPage += new PrintPageEventHandler(printAHAVAPOSReceipt);
                    for (int i = 0; i < NumOfCopies; i++)
                    {
                        printDoc.Print();
                    }
                }
                else if (size == ReceiptSizes.A4)
                {
                    pos = mypos;
                    //for (int i = 0; i < NumOfCopies; i++)
                        GenericReport.NewPrint.PrintController.PrintInvoice(PrinterName, NumOfCopies, GenericReport.PageType.A4, pos, reprint);
                    //GenericReport.PageController.GenerateReport(pos, GenericReport.PageType.A4, false, GenericReport.ReportType.INVOICE);                   
                }
                else if (size == ReceiptSizes.A5)
                {
                    pos = mypos;
                    //for (int i = 0; i < NumOfCopies; i++)
                        GenericReport.NewPrint.PrintController.PrintInvoice(PrinterName, NumOfCopies, GenericReport.PageType.A5, pos, reprint);
                }
            }
            catch (Exception E)
            {
                Logger.writeLog("Print Receipt failed! " + E.Message);
                return;
            }
        }

        public void PrintFormalAHAVAReceiptOnlyCopy
            (POSController mypos, decimal changeAmount, ReceiptSizes size, int NumOfCopies)
        {
            try
            {

                pos = mypos;
                string A4PrinterName = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.A4PrinterName);
                GenericReport.NewPrint.PrintController.PrintInvoiceFormalOnlyCopy(A4PrinterName, NumOfCopies, GenericReport.PageType.A4, pos, reprint);
            }
            catch (Exception E)
            {
                Logger.writeLog("Print Receipt failed! " + E.Message);
                return;
            }
        }

        public void PrintToAdditionalPrinter
            (POSController mypos, decimal changeAmount, ReceiptSizes size, int NumOfCopies)
        {
            try
            {
                pos = mypos;
                GenericReport.NewPrint.PrintController.PrintAdditionalInvoice(PrinterName, NumOfCopies, GenericReport.PageType.A4, pos, reprint);
            }
            catch (Exception E)
            {
                Logger.writeLog("Print Receipt failed! " + E.Message);
                return;
            }
        }

        public static ReportDocument GetDeliveryInvoice(POSController pos, string ReceiptLocation, bool reprint)
        {
            string ReceiptFileLocation;

            ReportDocument receiptA41 = new ReportDocument();
            //DeliveryOrderReport receiptA41 = new DeliveryOrderReport();

            #region *) Fetch: Get Template's Path
            ReceiptFileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.DeliveryOrderFileLocation);
            #endregion

            bool ReportLoaded = false;
            if (ReceiptFileLocation != null && ReceiptFileLocation.ToLower().EndsWith(".rpt") && File.Exists(ReceiptFileLocation))
            {

                receiptA41.Load(ReceiptFileLocation);
                DataTable dt = new DeliveryOrderDS.DeliveryOrderDTDataTable();

                string sqlstring = @"select do.PurchaseOrderRefNo as OrderNumber, do.DeliveryDate, 
                        ISNULL(m.StreetName,'') + ISNULL(m.streetName2,'') as MembershipAddress, 
                        ISNULL(do.DeliveryAddress,'') as DeliveryAddress,  
                        do.SalesOrderRefno, do.MembershipNo, dod.ItemNo, i.ItemName + ' ' + ISNULL(od.Remark, '') as [ItemName], 
                        dod.Quantity, m.NameToAppear, CASE WHEN ISNULL(do.HomeNo,'') = '' THEN do.MobileNo ELSE do.HomeNo END as Home, 
                        m.Fax, do.remark, sr.salespersonID,  
                        ISNULL(ap.AppSettingValue,'') as Terms, do.RecipientName as ContactPerson, dod.Remarks as LineRemarks,    
                        m.ZipCode as MembershipPostalCode, m.Mobile + ' - ' + m.Home as MembershipPhoneNo, 
                        do.PostalCode as DeliveryPostalCode, do.MobileNo + ' - ' + do.HomeNo as DeliveryPhoneNo, 
                        oh.userfld5 as InvoiceNo, oh.OrderDate as InvoiceDate, pos.OutletName,   
                        od.UnitPrice, od.Amount as LineAmount, oh.GrossAmount, oh.NettAmount, oh.GSTAmount, 
                        od.GrossSales as LineGrossAmount, od.GSTAmount as LineGSTAmount, 
                        ISNULL(od.userfloat1,0) as LineDepositAmount, od.Amount - ISNULL(od.userfloat1,0) as LineOutstanding 
		                ,do.TimeSlotFrom
		                ,do.TimeSlotTo, do.PostalCode, do.UnitNo 
                        ,oh.userfld1 as OHUserfld1 ,oh.userfld2 as OHUserfld2 ,oh.userfld3 as OHUserfld3 ,oh.userfld4 as OHUserfld4 ,oh.userfld5 as OHUserfld5
                        ,oh.userfld6 as OHUserfld6 ,oh.userfld7 as OHUserfld7 ,oh.userfld1 as OHUserfld8 ,oh.userfld9 as OHUserfld9 ,oh.userfld10 as OHUserfld10 
                        ,i.Attributes1, i.Attributes2, i.Attributes3, i.Attributes4, i.Attributes5, i.Attributes6, i.Attributes7, i.Attributes8 
                        ,dod.Serial_Number as SerialNumber     
                        from deliveryorder do 
                        LEFT JOIN deliveryorderdetails dod on do.ordernumber = dod.dohdrid 
                        LEFT JOIN Membership m on do.membershipno = m.membershipno 
                        LEFT JOIN Item i on dod.itemno = i.itemno 
                        LEFT JOIN Orderhdr oh on do.SalesOrderRefNo = oh.orderhdrid 
                        LEFT JOIN SalesCommissionRecord sr on sr.orderhdrid = do.SalesOrderRefNo 
                        LEFT JOIN appsetting ap on ap.AppSettingKey = 'DeliveryTerms' 
                        LEFT JOIN PointOfSale pos on pos.PointOfSaleID = oh.PointOfSaleID 
                        LEFT JOIN OrderDet od on od.OrderDetID = dod.OrderDetID  
                        WHERE oh.orderhdrid  = '" + pos.myOrderHdr.OrderHdrID + "'";

                DataSet ds = DataService.GetDataSet(new QueryCommand(sqlstring));

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = dt.NewRow();

                        row["OrderNumber"] = ds.Tables[0].Rows[i]["OrderNumber"];
                        row["MembershipNo"] = ds.Tables[0].Rows[i]["MembershipNo"];
                        row["NameToAppear"] = ds.Tables[0].Rows[i]["NameToAppear"];
                        row["DeliveryAddress"] = ds.Tables[0].Rows[i]["DeliveryAddress"];
                        row["ItemNo"] = ds.Tables[0].Rows[i]["ItemNo"];
                        row["ItemName"] = ds.Tables[0].Rows[i]["ItemName"];
                        row["Quantity"] = ds.Tables[0].Rows[i]["Quantity"];
                        row["SerialNumber"] = ds.Tables[0].Rows[i]["SerialNumber"] == null ? "" : ds.Tables[0].Rows[i]["SerialNumber"];
                        //row["Signature"] = ds.Tables[0].Rows[i]["Signature"];
                        row["Signature"] = null;
                        row["MembershipAddress"] = ds.Tables[0].Rows[i]["MembershipAddress"];
                        row["DeliveryDate"] = ds.Tables[0].Rows[i]["DeliveryDate"];
                        row["SalesOrderRefNo"] = ds.Tables[0].Rows[i]["SalesOrderRefNo"];
                        row["Home"] = ds.Tables[0].Rows[i]["Home"];
                        row["Fax"] = ds.Tables[0].Rows[i]["Fax"];
                        row["SalesPersonID"] = ds.Tables[0].Rows[i]["SalesPersonID"];
                        row["Terms"] = ds.Tables[0].Rows[i]["Terms"];
                        row["Remark"] = ds.Tables[0].Rows[i]["remark"];
                        row["PostalCode"] = ds.Tables[0].Rows[i]["PostalCode"];
                        row["UnitNo"] = ds.Tables[0].Rows[i]["UnitNo"];

                        row["ContactPerson"] = ds.Tables[0].Rows[i]["ContactPerson"];
                        row["LineRemarks"] = ds.Tables[0].Rows[i]["LineRemarks"];
                        row["MembershipPostalCode"] = ds.Tables[0].Rows[i]["MembershipPostalCode"];
                        row["MembershipPhoneNo"] = ds.Tables[0].Rows[i]["MembershipPhoneNo"];
                        row["DeliveryPoscalCode"] = ds.Tables[0].Rows[i]["DeliveryPostalCode"];
                        row["InvoiceNo"] = ds.Tables[0].Rows[i]["InvoiceNo"];
                        row["InvoiceDate"] = ds.Tables[0].Rows[i]["InvoiceDate"];
                        row["OutletName"] = ds.Tables[0].Rows[i]["OutletName"];
                        row["UnitPrice"] = ds.Tables[0].Rows[i]["UnitPrice"];
                        row["GrossAmount"] = ds.Tables[0].Rows[i]["GrossAmount"];
                        row["GSTAmount"] = ds.Tables[0].Rows[i]["GSTAmount"];
                        row["LineGrossAmount"] = ds.Tables[0].Rows[i]["LineGrossAmount"];
                        row["LineDepositAmount"] = ds.Tables[0].Rows[i]["LineDepositAmount"];
                        row["LineOutstanding"] = ds.Tables[0].Rows[i]["LineOutstanding"];
                        row["OHUserfld1"] = ds.Tables[0].Rows[i]["OHUserfld1"];
                        row["OHUserfld2"] = ds.Tables[0].Rows[i]["OHUserfld2"];
                        row["OHUserfld3"] = ds.Tables[0].Rows[i]["OHUserfld3"];
                        row["OHUserfld4"] = ds.Tables[0].Rows[i]["OHUserfld4"];
                        row["OHUserfld5"] = ds.Tables[0].Rows[i]["OHUserfld5"];
                        row["OHUserfld6"] = ds.Tables[0].Rows[i]["OHUserfld6"];
                        row["OHUserfld7"] = ds.Tables[0].Rows[i]["OHUserfld7"];
                        row["OHUserfld8"] = ds.Tables[0].Rows[i]["OHUserfld8"];
                        row["OHUserfld9"] = ds.Tables[0].Rows[i]["OHUserfld9"];
                        row["OHUserfld10"] = ds.Tables[0].Rows[i]["OHUserfld10"];

                        row["ItemAttributes1"] = ds.Tables[0].Rows[i]["Attributes1"];
                        row["ItemAttributes2"] = ds.Tables[0].Rows[i]["Attributes2"];
                        row["ItemAttributes3"] = ds.Tables[0].Rows[i]["Attributes3"];
                        row["ItemAttributes4"] = ds.Tables[0].Rows[i]["Attributes4"];
                        row["ItemAttributes5"] = ds.Tables[0].Rows[i]["Attributes5"];
                        row["ItemAttributes6"] = ds.Tables[0].Rows[i]["Attributes6"];
                        row["ItemAttributes7"] = ds.Tables[0].Rows[i]["Attributes7"];
                        row["ItemAttributes8"] = ds.Tables[0].Rows[i]["Attributes8"];

                        DateTime timeSlotFrom, timeSlotTo;
                        if (DateTime.TryParse(ds.Tables[0].Rows[i]["TimeSlotFrom"].ToString(), out timeSlotFrom) && DateTime.TryParse(ds.Tables[0].Rows[i]["TimeSlotTo"].ToString(), out timeSlotTo))
                        {
                            row["DeliveryTime"] = timeSlotFrom.ToString("htt") + " - " + timeSlotTo.ToString("htt");
                            row["TimeSlotFrom"] = timeSlotFrom;
                            row["TimeSlotTo"] = timeSlotTo;
                        }

                        dt.Rows.Add(row);
                    }
                }
                //receiptA41.SetDataSource();
                receiptA41.SetDataSource(dt);
                ReportLoaded = true;
                return receiptA41;
            }
            return receiptA41;
        }

        public void PrintDeliveryOrder
            (DeliveryOrder myHdr, DeliveryOrderDetailCollection myDet, POSController mypos)
        {
            try
            {
                string ReceiptFileLocation;
                ReportDocument receiptA41 = new ReportDocument();
                //DeliveryOrderReport receiptA41 = new DeliveryOrderReport();

                #region *) Fetch: Get Template's Path
                ReceiptFileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.DeliveryOrderFileLocation);
                #endregion

                bool ReportLoaded = false;
                if (ReceiptFileLocation != null && ReceiptFileLocation.ToLower().EndsWith(".rpt") && File.Exists(ReceiptFileLocation))
                {
                   
                        receiptA41.Load(ReceiptFileLocation);
                        DataTable dt = new DeliveryOrderDS.DeliveryOrderDTDataTable();

                        //string sqlstring = "select do.OrderNumber, m.MembershipNo, m.NameToAppear, do.DeliveryAddress, dod.ItemNo,i.ItemName, dod.Quantity,dod.Serial_Number as SerialNumber " +
                        //                   ", do.DeliveryDate, do.SalesOrderRefno " +
                        //                   ",ISNULL(m.StreetName,'') + ' ' + ISNULL(m.streetName2,'') + ' ' + ISNULL(m.Country,'') + ' ' + ISNULL(m.ZipCode,'') as MembershipAddress " +
                        //                   ",CASE WHEN ISNULL(do.HomeNo,'') = '' THEN do.MobileNo ELSE do.HomeNo END as Home, m.Fax, do.remark, '' as salespersonID " +
                        //                   ", ISNULL(ap.AppSettingValue,'') as Terms, do.TimeSlotFrom, do.TimeSlotTo, do.PostalCode, do.UnitNo " +
                        //                   "from DeliveryOrder do  " +
                        //                   "inner join DeliveryOrderDetails dod on do.OrderNumber = dod.DOHDRID " +
                        //                   "inner join item i on dod.ItemNo = i.ItemNo " +
                        //                   "inner join Membership m on do.MembershipNo = m.MembershipNo " +
                        //                   "LEFT JOIN appsetting ap on ap.AppSettingKey = 'DeliveryTerms' " +
                        //                   "where ISNULL(do.Deleted,0) = 0 and ISNULL(i.Deleted,0) = 0 and ISNULL(m.Deleted,0) = 0 " +
                        //                   "and do.OrderNumber = '" + myHdr.OrderNumber + "'";
                        string sqlstring = @"select do.PurchaseOrderRefNo as OrderNumber, do.DeliveryDate, 
                        ISNULL(m.StreetName,'') + ISNULL(m.streetName2,'') as MembershipAddress, 
                        ISNULL(do.DeliveryAddress,'') as DeliveryAddress,  
                        do.SalesOrderRefno, do.MembershipNo, dod.ItemNo, i.ItemName + ' ' + ISNULL(od.Remark, '') as [ItemName], 
                        dod.Quantity, m.NameToAppear, CASE WHEN ISNULL(do.HomeNo,'') = '' THEN do.MobileNo ELSE do.HomeNo END as Home, 
                        m.Fax, do.remark, sr.salespersonID,  
                        ISNULL(ap.AppSettingValue,'') as Terms, do.RecipientName as ContactPerson, dod.Remarks as LineRemarks,    
                        m.ZipCode as MembershipPostalCode, m.Mobile + ' - ' + m.Home as MembershipPhoneNo, 
                        do.PostalCode as DeliveryPostalCode, do.MobileNo + ' - ' + do.HomeNo as DeliveryPhoneNo, 
                        oh.userfld5 as InvoiceNo, oh.OrderDate as InvoiceDate, pos.OutletName,   
                        od.UnitPrice, od.Amount as LineAmount, oh.GrossAmount, oh.NettAmount, oh.GSTAmount, 
                        od.GrossSales as LineGrossAmount, od.GSTAmount as LineGSTAmount, 
                        ISNULL(od.userfloat1,0) as LineDepositAmount, od.Amount - ISNULL(od.userfloat1,0) as LineOutstanding 
		                ,do.TimeSlotFrom
		                ,do.TimeSlotTo, do.PostalCode, do.UnitNo 
                        ,oh.userfld1 as OHUserfld1 ,oh.userfld2 as OHUserfld2 ,oh.userfld3 as OHUserfld3 ,oh.userfld4 as OHUserfld4 ,oh.userfld5 as OHUserfld5
                        ,oh.userfld6 as OHUserfld6 ,oh.userfld7 as OHUserfld7 ,oh.userfld1 as OHUserfld8 ,oh.userfld9 as OHUserfld9 ,oh.userfld10 as OHUserfld10 
                        ,i.Attributes1, i.Attributes2, i.Attributes3, i.Attributes4, i.Attributes5, i.Attributes6, i.Attributes7, i.Attributes8 
                        ,dod.Serial_Number as SerialNumber     
                        from deliveryorder do 
                        LEFT JOIN deliveryorderdetails dod on do.ordernumber = dod.dohdrid 
                        LEFT JOIN Membership m on do.membershipno = m.membershipno 
                        LEFT JOIN Item i on dod.itemno = i.itemno 
                        LEFT JOIN Orderhdr oh on do.SalesOrderRefNo = oh.orderhdrid 
                        LEFT JOIN SalesCommissionRecord sr on sr.orderhdrid = do.SalesOrderRefNo 
                        LEFT JOIN appsetting ap on ap.AppSettingKey = 'DeliveryTerms' 
                        LEFT JOIN PointOfSale pos on pos.PointOfSaleID = oh.PointOfSaleID 
                        LEFT JOIN OrderDet od on od.OrderDetID = dod.OrderDetID  
                        WHERE do.ordernumber = '" + myHdr.OrderNumber + "'";
                        
                        DataSet ds = DataService.GetDataSet(new QueryCommand(sqlstring));

                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                DataRow row = dt.NewRow();

                                row["OrderNumber"] = ds.Tables[0].Rows[i]["OrderNumber"];
                                row["MembershipNo"] = ds.Tables[0].Rows[i]["MembershipNo"];
                                row["NameToAppear"] = ds.Tables[0].Rows[i]["NameToAppear"];
                                row["DeliveryAddress"] = ds.Tables[0].Rows[i]["DeliveryAddress"];
                                row["ItemNo"] = ds.Tables[0].Rows[i]["ItemNo"];
                                row["ItemName"] = ds.Tables[0].Rows[i]["ItemName"];
                                row["Quantity"] = ds.Tables[0].Rows[i]["Quantity"];
                                row["SerialNumber"] = ds.Tables[0].Rows[i]["SerialNumber"] ==null ? "" : ds.Tables[0].Rows[i]["SerialNumber"];
                                //row["Signature"] = ds.Tables[0].Rows[i]["Signature"];
                                row["Signature"] = null;
                                row["MembershipAddress"] = ds.Tables[0].Rows[i]["MembershipAddress"];
                                row["DeliveryDate"] = ds.Tables[0].Rows[i]["DeliveryDate"];
                                row["SalesOrderRefNo"] = ds.Tables[0].Rows[i]["SalesOrderRefNo"];
                                row["Home"] = ds.Tables[0].Rows[i]["Home"];
                                row["Fax"] = ds.Tables[0].Rows[i]["Fax"];
                                row["SalesPersonID"] = ds.Tables[0].Rows[i]["SalesPersonID"];
                                row["Terms"] = ds.Tables[0].Rows[i]["Terms"];
                                row["Remark"] = ds.Tables[0].Rows[i]["remark"];
                                row["PostalCode"] = ds.Tables[0].Rows[i]["PostalCode"];
                                row["UnitNo"] = ds.Tables[0].Rows[i]["UnitNo"];

                                row["ContactPerson"] = ds.Tables[0].Rows[i]["ContactPerson"];
                                row["LineRemarks"] = ds.Tables[0].Rows[i]["LineRemarks"];
                                row["MembershipPostalCode"] = ds.Tables[0].Rows[i]["MembershipPostalCode"];
                                row["MembershipPhoneNo"] = ds.Tables[0].Rows[i]["MembershipPhoneNo"];
                                row["DeliveryPoscalCode"] = ds.Tables[0].Rows[i]["DeliveryPostalCode"];
                                row["InvoiceNo"] = ds.Tables[0].Rows[i]["InvoiceNo"];
                                row["InvoiceDate"] = ds.Tables[0].Rows[i]["InvoiceDate"];
                                row["OutletName"] = ds.Tables[0].Rows[i]["OutletName"];
                                row["UnitPrice"] = ds.Tables[0].Rows[i]["UnitPrice"];
                                row["GrossAmount"] = ds.Tables[0].Rows[i]["GrossAmount"];
                                row["GSTAmount"] = ds.Tables[0].Rows[i]["GSTAmount"];
                                row["LineGrossAmount"] = ds.Tables[0].Rows[i]["LineGrossAmount"];
                                row["LineDepositAmount"] = ds.Tables[0].Rows[i]["LineDepositAmount"];
                                row["LineOutstanding"] = ds.Tables[0].Rows[i]["LineOutstanding"];
                                row["OHUserfld1"] = ds.Tables[0].Rows[i]["OHUserfld1"];
                                row["OHUserfld2"] = ds.Tables[0].Rows[i]["OHUserfld2"];
                                row["OHUserfld3"] = ds.Tables[0].Rows[i]["OHUserfld3"];
                                row["OHUserfld4"] = ds.Tables[0].Rows[i]["OHUserfld4"];
                                row["OHUserfld5"] = ds.Tables[0].Rows[i]["OHUserfld5"];
                                row["OHUserfld6"] = ds.Tables[0].Rows[i]["OHUserfld6"];
                                row["OHUserfld7"] = ds.Tables[0].Rows[i]["OHUserfld7"];
                                row["OHUserfld8"] = ds.Tables[0].Rows[i]["OHUserfld8"];
                                row["OHUserfld9"] = ds.Tables[0].Rows[i]["OHUserfld9"];
                                row["OHUserfld10"] = ds.Tables[0].Rows[i]["OHUserfld10"];

                                row["ItemAttributes1"] = ds.Tables[0].Rows[i]["Attributes1"];
                                row["ItemAttributes2"] = ds.Tables[0].Rows[i]["Attributes2"];
                                row["ItemAttributes3"] = ds.Tables[0].Rows[i]["Attributes3"];
                                row["ItemAttributes4"] = ds.Tables[0].Rows[i]["Attributes4"];
                                row["ItemAttributes5"] = ds.Tables[0].Rows[i]["Attributes5"];
                                row["ItemAttributes6"] = ds.Tables[0].Rows[i]["Attributes6"];
                                row["ItemAttributes7"] = ds.Tables[0].Rows[i]["Attributes7"];
                                row["ItemAttributes8"] = ds.Tables[0].Rows[i]["Attributes8"];

                                DateTime timeSlotFrom, timeSlotTo;
                                if (DateTime.TryParse(ds.Tables[0].Rows[i]["TimeSlotFrom"].ToString(), out timeSlotFrom) && DateTime.TryParse(ds.Tables[0].Rows[i]["TimeSlotTo"].ToString(), out timeSlotTo))
                                {
                                    row["DeliveryTime"] = timeSlotFrom.ToString("htt") + " - " + timeSlotTo.ToString("htt");
                                     row["TimeSlotFrom"] = timeSlotFrom;
                                     row["TimeSlotTo"] = timeSlotTo;
                                }

                                dt.Rows.Add(row);
                            }
                        }
                        //receiptA41.SetDataSource();
                        receiptA41.SetDataSource(dt);
                        ReportLoaded = true;

                        // Prefer to print to A4 printer
                        string a4printer = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.A4PrinterName);
                        if (!string.IsNullOrEmpty(a4printer)) PrinterName = a4printer;

                        receiptA41.PrintOptions.PrinterName = PrinterName;
                        receiptA41.PrintToPrinter(1, false, 0, 0);

                        
                   
                }
                

                
                

                //receiptA41.SetDataSource(dt);
                //DataSet dsCompany = GetCompanyInfoAddressDataSet();
                //receiptA41.Subreports["CompanyInfoReport.rpt"].SetDataSource(dsCompany);
                //receiptA41.DataDefinition.FormulaFields["OrderNumber"].Text = "\"" + myHdr.OrderNumber + "\"";
                //receiptA41.PrintToPrinter(1, false, 0, 0);

            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print Receipt failed!", E);
            }
        }

        public void PrintPreOrderDelivery (DataTable dt)
        {
            try
            {
                string ReceiptFileLocation;
                ReportDocument receiptA41 = new ReportDocument();
                //DeliveryOrderReport receiptA41 = new DeliveryOrderReport();

                #region *) Fetch: Get Template's Path
                ReceiptFileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintPreOrderTemplateReport);
                #endregion

                bool ReportLoaded = false;
                if (ReceiptFileLocation != null && ReceiptFileLocation.ToLower().EndsWith(".rpt") && File.Exists(ReceiptFileLocation))
                {

                    receiptA41.Load(ReceiptFileLocation);
                    receiptA41.SetDataSource(dt);
                    ReportLoaded = true;

                    // Prefer to print to A4 printer
                    string a4printer = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.A4PrinterName);
                    if (!string.IsNullOrEmpty(a4printer)) PrinterName = a4printer;

                    receiptA41.PrintOptions.PrinterName = PrinterName;
                    receiptA41.PrintToPrinter(1, false, 0, 0);
                }

            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print Pre Order failed!", E);
            }
        }

        public Stream GetReceiptPDFStream(POSController myPos)
        {
            Stream st = null;
            string ReceiptFileLocation;
            #region *) Fetch: Get Template's Path
            if (myPos.IsADelivery)
                ReceiptFileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.DeliveryOrderFileLocation);
            else
                ReceiptFileLocation = AppSetting.GetSetting(AppSetting.SettingsName.Print.ReceiptFileLocation);
            #endregion
            ReportDocument Inst = new ReportDocument();

            bool ReportLoaded = false;
            
            if (ReceiptFileLocation != null && ReceiptFileLocation.ToLower().EndsWith(".rpt") && File.Exists(ReceiptFileLocation))
            {
                try
                {
                    if (myPos.IsADelivery)
                    {
                        var rptDoc = GetDeliveryInvoice(myPos, ReceiptFileLocation, reprint);
                        st = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    }
                    else
                    {
                        var rptDoc = GenericReport.NewPrint.PrintController.GetInvoice(myPos, ReceiptFileLocation, reprint);
                        st = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    }

                    
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
            }
            else
            {
                var rptDoc = GenericReport.NewPrint.PrintController.GetInvoice(myPos, new GenericReport.NewPrint.A5(), true);
                st = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            }
            return st;
            
        }

        public Stream GetPreOrderMail(string orderHdrID, string orderDetID)
        {
            Stream st = null;
            try
            {
                string ReceiptLocation = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.PreOrderReceiptTemplate);
                if (string.IsNullOrEmpty(ReceiptLocation))
                {
                    ReceiptLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\Reports\\EmailPreOrder.rpt";
                    ReceiptLocation = ReceiptLocation.Replace("file:\\", "");
                }

                var rptDoc = GenericReport.NewPrint.PrintController.GetPreOrderMail(orderHdrID, orderDetID, ReceiptLocation);
                st = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return st;
        }

        public static Stream GetPreOrderNotifyMail(string orderHdrID, string orderDetID)
        {
            Stream st = null;
            try
            {
                string ReceiptLocation = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.PreOrderNotifyTemplate);
                if (string.IsNullOrEmpty(ReceiptLocation))
                {
                    ReceiptLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\Reports\\EmailNotifyPreOrder.rpt";
                    ReceiptLocation = ReceiptLocation.Replace("file:\\", "");
                }

                var rptDoc = GenericReport.NewPrint.PrintController.GetPreOrderMail(orderHdrID, orderDetID, ReceiptLocation);
                st = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return st;
        }

        private static DataSet companyInfoDataSet;

        private static DataSet SetCompanyInfoAddressDataSet()
        {
            companyInfoDataSet = new DataSet();
            companyInfoDataSet.Tables.Add((new CompanyCollection().Load().ToDataTable()));
            ReceiptSettingCollection c = new ReceiptSettingCollection();
            c.Load();
            //c.Add(PowerPOS.Container.PrintSettingInfo.receiptSetting);
            companyInfoDataSet.Tables.Add(c.ToDataTable());

            return companyInfoDataSet;
        }

        private static DataSet GetCompanyInfoAddressDataSet()
        {
            if (companyInfoDataSet == null)
                companyInfoDataSet = SetCompanyInfoAddressDataSet();

            return companyInfoDataSet;
        }

        public void PrintPreOrderReceipt(POSController mypos)
        {
            try
            {
                pos = mypos;
                PrintDocument printDoc = new PrintDocument();
                if (PrinterName.CompareTo("") != 0)
                {
                    printDoc.PrinterSettings.PrinterName = PrinterName;
                }
                printDoc.PrintPage += new PrintPageEventHandler(printPreOrderReceipt);
                printDoc.Print();
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print Receipt failed!", E);
            }
        }

        public void PrintNewSignUpReceipt(POSController mypos)
        {
            try
            {
                pos = mypos;
                PrintDocument printDoc = new PrintDocument();
                if (PrinterName.CompareTo("") != 0)
                {
                    printDoc.PrinterSettings.PrinterName = PrinterName;
                }
                printDoc.PrintPage += new PrintPageEventHandler(printNewSignUpReceipt);
                printDoc.Print();
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print Receipt failed!", E);
            }
        }

        private void printAHAVAPOSReceipt(Object sender,
                PrintPageEventArgs e)
        {
            string status;
            try
            {
                bool useWeight = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseWeight), false);
                decimal balancepayment = 0;
                int PageHeight = e.MarginBounds.Height;
                int y = 0;

                //Double Total = 0;
                int RowHeight = (int)printFont.GetHeight();

                System.Drawing.StringFormat RightAlign =
                    new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                RightAlign.Alignment = System.Drawing.StringAlignment.Far;

                System.Drawing.StringFormat CenterAlign =
                    new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                CenterAlign.Alignment = System.Drawing.StringAlignment.Center;

                #region *) REPRINT
                if (reprint)
                {
                    e.Graphics.DrawString("**REPRINT**", companyFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += (int)companyFont.GetHeight();
                }
                #endregion

                #region *) VOIDED
                if (pos.IsVoided())
                {
                    e.Graphics.DrawString("**VOIDED**", companyFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += (int)companyFont.GetHeight();
                }
                #endregion

                #region *) Print Logo
                if (System.IO.File.Exists("Logo.jpg"))
                {
                    Bitmap MyBitmap = new Bitmap("Logo.jpg");
                    int xPos;
                    xPos = (PageWidth / 2) - MyBitmap.Width / 2;
                    if (xPos < 0) xPos = 0;
                    e.Graphics.DrawImage(MyBitmap, new RectangleF(xPos, y,
                                 MyBitmap.Width, MyBitmap.Height));
                    y += MyBitmap.Height;
                }
                #endregion
                /*
                if (Company.CompareTo("") != 0)
                {
                    e.Graphics.DrawString(Company, headerFont,
                                          Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                              CenterAlign);
                    y += RowHeight + 5;
                }*/                

                #region *) Print Receipt Name
                if (CompanyInfo.ReceiptName != "")
                {
                    e.Graphics.DrawString(CompanyInfo.ReceiptName, companyFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += (int)companyFont.GetHeight();
                }
                #endregion
                #region *) Print Receipt Address
                if (PrintSettingInfo.receiptSetting.ReceiptAddress1 != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.ReceiptAddress1, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }

                if (PrintSettingInfo.receiptSetting.ReceiptAddress2 != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.ReceiptAddress2, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                if (PrintSettingInfo.receiptSetting.ReceiptAddress3 != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.ReceiptAddress3, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                if (PrintSettingInfo.receiptSetting.ReceiptAddress4 != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.ReceiptAddress4, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                #endregion
                y += RowHeight+5;

                #region *) Print receipt No
                e.Graphics.DrawString("Ref No : " + pos.GetCustomizedRefNo(), printFont,
                Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30));
                y += RowHeight + 2;
                #endregion

                #region *) Print Cashier Name
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintCashierOnTheReceipt), true))
                {
                    e.Graphics.DrawString
                    ("Cashier :" + pos.GetCashierName(),
                    printFont, Brushes.Black, Padding, y);
                    y += RowHeight + 5;
                }
                #endregion


                #region *) Print POS Name
                if (PointOfSaleInfo.PointOfSaleName != "")
                {
                    e.Graphics.DrawString("POS ID : " + PointOfSaleInfo.PointOfSaleName, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30));
                    y += (int)companyFont.GetHeight();
                }
                #endregion

                #region *) Print GST Info
                if (CompanyInfo.GSTRegNo != null && CompanyInfo.GSTRegNo != "" && CompanyInfo.GSTRegNo != "-")
                {
                    e.Graphics.DrawString("GST Reg no. " + CompanyInfo.GSTRegNo, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                #endregion
                y += 10;
                string item;
                #region *) Print Item Header
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
                #endregion
                y += RowHeight + 2;
                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 2;
                //OrderDetCollection det = new OrderDetCollection();
                //det.Where(OrderDet.Columns.OrderHdrID, SubSonic.Comparison.Equals, pos.myOrderHdr.OrderHdrID).Load();
                //det.Sort(OrderDet.Columns.CreatedOn, true);
                OrderDetCollection det = pos.FetchOrderDetForReceipt(pos.myOrderHdr.OrderHdrID);
                
                ReceiptDetCollection RcptDet = pos.FetchUnsavedReceipt(out status);
                #region *) Print Item Detail

                

                for (int i = 0; i < det.Count; i++)
                {
                    if (det[i].IsVoided) continue;

                    if ((det[i].ItemNo == "R0001") && (det[i].Amount == 0)) continue;

                    if (det[i].ItemNo == "INST_PAYMENT")
                        balancepayment += det[i].Amount;

                    //item = det[i].ItemNo + "-" + det[i].Item.ItemName + " " + det[i].Remark;
                    item = det[i].Item.ItemName + " " + det[i].Remark;
                    int TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                    e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                    y += TextHeight;
                    string ShowLineSalesPersonOnReceipt = AppSetting.GetSetting("ShowLineSalesPersonOnReceipt");
                    if (ShowLineSalesPersonOnReceipt != null && ShowLineSalesPersonOnReceipt.ToLower() == "yes")
                    {
                        if (det[i].SalesPerson != null && det[i].SalesPerson != "")
                        {
                            item = "(by) " + det[i].SalesPerson;
                            TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                            e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                            y += TextHeight;
                        }
                    }
                    if (det[i].Item.PointGetMode == Item.PointMode.Times && RcptDet.Count == 1 && RcptDet[0].PaymentType == POSController.PAY_PACKAGE)
                    {
                        //e.Graphics.DrawString
                        //    (String.Format("{0:N}", det[i].UnitPrice),
                        //    printFont, Brushes.Black, Padding, y);
                        //e.Graphics.DrawString
                        //    ("\t\t" + det[i].Quantity,
                        //    printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(det[i].Quantity.GetValueOrDefault(0).ToString("N0") + " Times",
                            printFont, Brushes.Black,
                            new RectangleF(Padding, y, PageWidth - Padding, 30),
                            RightAlign);
                    }
                    else
                    {
                        e.Graphics.DrawString
                            (String.Format("{0:N}", det[i].UnitPrice),
                            printFont, Brushes.Black, Padding, y);
                        if (useWeight)
                        {
                            e.Graphics.DrawString
                                ("\t\t" + det[i].Quantity.GetValueOrDefault(0).ToString("N2") + det[i].UOM,
                                printFont, Brushes.Black, Padding, y);
                        }
                        else
                        {
                            e.Graphics.DrawString
                                ("\t\t" + det[i].Quantity.GetValueOrDefault(0).ToString("N0"),
                                printFont, Brushes.Black, Padding, y); 
                        }
                        e.Graphics.DrawString(String.Format("{0:N}", det[i].Amount),
                            printFont, Brushes.Black,
                            new RectangleF(Padding, y, PageWidth - Padding, 30),
                            RightAlign);
                    }
                    y += RowHeight + 2;
                    if (det[i].GrossSales != 0)
                    {
                        //int disc = (int)((((decimal)(det[i].GrossSales - det[i].Amount)) / det[i].GrossSales) * 100);
                        if (string.IsNullOrEmpty(det[i].DiscountDetail) || det[i].DiscountDetail.Equals("0%"))
                        {
                            if (det[i].IsPromo)
                            {
                                if (det[i].PromoDiscount > 0)
                                {
                                    e.Graphics.DrawString("   Discount:\t" + det[i].PromoDiscount.ToString("N0") + "%", printFont, Brushes.Black, Padding, y);
                                    y += RowHeight + 2;
                                }
                            }
                            else
                            {
                                if (det[i].Discount > 0)
                                {
                                    e.Graphics.DrawString("   Discount:\t" + det[i].Discount.ToString("N0") + "%", printFont, Brushes.Black, Padding, y);
                                    y += RowHeight + 2;
                                }
                            }
                        }
                        else
                        {
                            e.Graphics.DrawString("   Discount:\t" + det[i].DiscountDetail, printFont, Brushes.Black, Padding, y);
                            y += RowHeight + 2;
                        }

                        //y += RowHeight + 2;
                    }

                    y += 3;
                }
                #endregion
                //y += 10;
                /*
                if (pos.getOverallDisc() > 0)
                {
                    e.Graphics.DrawString("Overall Discount:\t" +
                       pos.getOverallDisc().ToString("N0") + "%",
                        printFont, Brushes.Black, Padding, y);

                    e.Graphics.DrawString(String.Format("{0:N}", pos.getOverallDiscAmount()),
                            printFont, Brushes.Black,
                            new RectangleF(Padding, y, PageWidth - Padding, 30),
                            RightAlign);
                    y += RowHeight * 2;
                }
                */

                decimal GrossAmt = pos.CalculateLineGrossAmountForReceipt();
                decimal TotalAmt = pos.CalculateTotalAmount(out status);
                decimal myGST = (decimal)pos.getGST(out status) / 100;

                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);

                #region *) Print SubTotal
                e.Graphics.DrawString("Total Amount: " + String.Format("{0:N}", GrossAmt), totalFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += (int)totalFont.GetHeight() + 2;

                decimal totalDiscount = pos.CalculateTotalDiscount();
                if (totalDiscount != 0.0M)
                {
                    e.Graphics.DrawString("Total Discount: " + String.Format("{0:N}", totalDiscount), totalFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += (int)totalFont.GetHeight() + 2;
                }
                if (totalDiscount != 0.0M)
                {
                    e.Graphics.DrawString("Nett Total: " + String.Format("{0:N}", TotalAmt), totalFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += (int)totalFont.GetHeight() + 2;
                }

                decimal totalGST = pos.GetGSTAmount();
                if (totalGST != 0.0M)
                {
                    //e.Graphics.DrawString("Amount Before GST: " + String.Format("{0:N}", TotalAmt - totalGST), totalFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    //y += (int)totalFont.GetHeight() + 2;

                    string gstText = "GST";
                    if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith)))
                        gstText = AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith);

                    e.Graphics.DrawString(gstText + ": " + String.Format("{0:N}", totalGST), totalFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += (int)totalFont.GetHeight() + 2;
                }
                #endregion

                y += (int)totalFont.GetHeight() + 8;

                if (!(RcptDet.Count == 1 && RcptDet[0].PaymentType == POSController.PAY_PACKAGE))
                {
                    #region *) Print Payment Header
                    e.Graphics.DrawString
                    ("Payment Mode",
                    printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString("Amount",
                        printFont, Brushes.Black,
                        new RectangleF(Padding, y, PageWidth - Padding, 30),
                        RightAlign);
                    y += RowHeight;
                    #endregion
                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);

                    #region *) Default Currency

                    string defCurrCode = AppSetting.GetSetting(AppSetting.SettingsName.Currency.DefaultCurrency);
                    if (string.IsNullOrEmpty(defCurrCode))
                        defCurrCode = "SGD";
                    string defCurrSymbol = "$";
                    Currency defCurr = new Currency(Currency.Columns.CurrencyCode, defCurrCode);
                    if (!defCurr.IsNew && !string.IsNullOrEmpty(defCurr.CurrencySymbol))
                        defCurrSymbol = defCurr.CurrencySymbol;

                    #endregion

                    #region *) Print Payment Detail
                    for (int i = 0; i < RcptDet.Count; i++)
                    {
                        if (RcptDet[i].PaymentType == POSController.PAY_INSTALLMENT)
                            balancepayment += RcptDet[i].Amount;

                        if (RcptDet[i].PaymentType.ToUpper().StartsWith("CASH-"))
                        {
                            decimal payable = RcptDet[i].Amount;
                            decimal returned = RcptDet[i].Change.GetValueOrDefault(0);
                            decimal collected = payable + returned;

                            e.Graphics.DrawString(string.Format("CASH ({0})",defCurrCode), printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(string.Format("{0} {1}", defCurrSymbol, payable.ToString("N2")),
                                printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;

                            e.Graphics.DrawString(string.Format("Cash Tenderd ({0})", defCurrCode), printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(string.Format("{0} {1}", defCurrSymbol, collected.ToString("N2")),
                                printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;

                            if (returned > 0)
                            {
                                e.Graphics.DrawString(string.Format("Change ({0})", defCurrCode), printFont, Brushes.Black, Padding, y);
                                e.Graphics.DrawString(string.Format("{0} {1}", defCurrSymbol, returned.ToString("N2")),
                                    printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                                y += RowHeight;
                            }
                            y += RowHeight;
                            string currCode = RcptDet[i].PaymentType.ToUpper().Replace("CASH-","");
                            string currSymbol = "$";
                            Currency curr = new Currency(Currency.Columns.CurrencyCode, currCode);
                            if(!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                                currSymbol = curr.CurrencySymbol;
                            payable = RcptDet[i].ForeignAmountReceived.GetValueOrDefault(0);
                            returned = RcptDet[i].ForeignAmountReturned.GetValueOrDefault(0);
                            collected = payable + returned;

                            e.Graphics.DrawString(string.Format("Conversion Rate"), printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(string.Format("{0} {1}", currSymbol, RcptDet[i].ForeignCurrencyRate.GetValueOrDefault(0).ToString("N2")),
                                printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;
                            y += RowHeight;

                            e.Graphics.DrawString(string.Format("CASH ({0})", currCode), printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(string.Format("{0} {1}", currSymbol, payable.ToString("N2")),
                                printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;

                            e.Graphics.DrawString(string.Format("Cash Tenderd ({0})", currCode), printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(string.Format("{0} {1}", currSymbol, collected.ToString("N2")),
                                printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                            y += RowHeight;

                            if (returned > 0)
                            {
                                e.Graphics.DrawString(string.Format("Change ({0})", currCode), printFont, Brushes.Black, Padding, y);
                                e.Graphics.DrawString(string.Format("{0} {1}", currSymbol, returned.ToString("N2")),
                                    printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                                y += RowHeight;
                            }
                        }
                        else if (RcptDet[i].PaymentType == POSController.PAY_PACKAGE)
                        {
                            /// Do not print anything
                        }
                        else
                        {
                            if (RcptDet[i].PaymentType == POSController.PAY_CASH &&
                                !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Currency.DefaultCurrency)))
                            {
                                e.Graphics.DrawString
                                    (RcptDet[i].PaymentType + string.Format(" ({0})",defCurrCode),
                                     printFont, Brushes.Black, Padding, y);
                            }
                            else
                            {
                                //e.Graphics.DrawString
                                //    (RcptDet[i].PaymentType,
                                //     printFont, Brushes.Black, Padding, y); 

                                string paymenttype = RcptDet[i].PaymentType;
                                if (paymenttype == POSController.PAY_INSTALLMENT && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText)))
                                    paymenttype = AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText);

                                if (!string.IsNullOrEmpty(RcptDet[i].Userfld1))
                                {
                                    // Get Points name
                                    Item itm = new Item(RcptDet[i].Userfld1);
                                    if (itm != null && itm.ItemNo == RcptDet[i].Userfld1)
                                    {
                                        paymenttype = paymenttype + " (" + itm.ItemName + ")";
                                    }
                                }
                                e.Graphics.DrawString
                                    (paymenttype,
                                     printFont, Brushes.Black, Padding, y); 
                            }
                            if (RcptDet[i].PaymentType == POSController.PAY_POINTS) /// Do not show [$] sign
                                e.Graphics.DrawString(RcptDet[i].Amount.ToString("N2") + " Pt",
                                    printFont, Brushes.Black,
                                    new RectangleF(Padding, y, PageWidth - Padding, 30),
                                    RightAlign);
                            else
                                e.Graphics.DrawString(string.Format("{0:N}", RcptDet[i].Amount),
                                    printFont, Brushes.Black,
                                    new RectangleF(Padding, y, PageWidth - Padding, 30),
                                    RightAlign);
                            y += RowHeight;
                        }
                        if (RcptDet[i].PaymentType == POSController.PAY_CASH)
                        {
                            string currencyCode = "";
                            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Currency.DefaultCurrency)))
                                currencyCode = string.Format(" ({0})", defCurrCode);

                            e.Graphics.DrawString
                            ("Cash Tendered" + currencyCode,
                             printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", RcptDet[i].Amount + RcptDet[i].Change),
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                RightAlign);
                            y += RowHeight;
                            e.Graphics.DrawString
                            ("Change" + currencyCode,
                             printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", RcptDet[i].Change),
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                RightAlign);
                            y += RowHeight;
                        }
                        else if (RcptDet[i].PaymentType == POSController.PAY_CHEQUE)
                        {
                            e.Graphics.DrawString
                            ("Cheque No",
                             printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", RcptDet[i].Userfld1),
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                RightAlign);
                            y += RowHeight;

                            e.Graphics.DrawString
                            ("Bank",
                             printFont, Brushes.Black, Padding, y);
                            e.Graphics.DrawString(String.Format("{0:N}", RcptDet[i].Userfld2),
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                RightAlign);
                            y += RowHeight;
                        }
                    }
                    #endregion
                    y += RowHeight + 2;
                }

                y += RowHeight + 3;

                #region *) Print NETS Response Info
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintNETSResponseInfoOnTheReceipt), false))
                {
                    string netsCC_VISA = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_VISA) ?? "";
                    string netsCC_MASTER = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_MASTER) ?? "";
                    string netsCC_AMEX = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_AMEX) ?? "";

                    for (int i = 0; i < RcptDet.Count; i++)
                    {
                        if (RcptDet[i].PaymentType.ToUpper().Trim() == POSController.PAY_NETSATMCard.ToUpper().Trim() ||
                            RcptDet[i].PaymentType.ToUpper().Trim() == POSController.PAY_NETSCashCard.ToUpper().Trim() ||
                            RcptDet[i].PaymentType.ToUpper().Trim() == POSController.PAY_NETSFlashPay.ToUpper().Trim() ||
                            RcptDet[i].PaymentType.ToUpper().Trim() == netsCC_VISA.ToUpper().Trim() ||
                            RcptDet[i].PaymentType.ToUpper().Trim() == netsCC_MASTER.ToUpper().Trim() ||
                            RcptDet[i].PaymentType.ToUpper().Trim() == netsCC_AMEX.ToUpper().Trim())
                        {
                            if (!string.IsNullOrEmpty(RcptDet[i].NETSResponseInfo))
                            {
                                string headerName = "";
                                if (RcptDet[i].PaymentType.ToUpper().Trim() == POSController.PAY_NETSATMCard.ToUpper().Trim())
                                    headerName = "NETS";
                                if (RcptDet[i].PaymentType.ToUpper().Trim() == POSController.PAY_NETSCashCard.ToUpper().Trim())
                                    headerName = "CashCard";
                                if (RcptDet[i].PaymentType.ToUpper().Trim() == POSController.PAY_NETSFlashPay.ToUpper().Trim())
                                    headerName = "NETS FlashPay";
                                if (RcptDet[i].PaymentType.ToUpper().Trim() == netsCC_VISA.ToUpper().Trim())
                                    headerName = "VISA";
                                if (RcptDet[i].PaymentType.ToUpper().Trim() == netsCC_MASTER.ToUpper().Trim())
                                    headerName = "MASTER";
                                if (RcptDet[i].PaymentType.ToUpper().Trim() == netsCC_AMEX.ToUpper().Trim())
                                    headerName = "AMEX";
                                e.Graphics.DrawString("- " + headerName + " Transaction Information -", printFont,
                                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                    CenterAlign);
                                y += RowHeight + 2;

                                Dictionary<string, string> responseInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(RcptDet[i].NETSResponseInfo);

                                #region *) TEMPORARY FIX
                                // JUST TEMPORARY
                                if (RcptDet[i].PaymentType.ToUpper().Trim() == netsCC_VISA.ToUpper().Trim() ||
                                    RcptDet[i].PaymentType.ToUpper().Trim() == netsCC_MASTER.ToUpper().Trim() ||
                                    RcptDet[i].PaymentType.ToUpper().Trim() == netsCC_AMEX.ToUpper().Trim())
                                {
                                    responseInfo.Add("Entry Type", "Contact");
                                }
                                #endregion

                                List<IntegratedReceiptFields.ReceiptField> receiptFields = NetsController.GetReceiptFields(RcptDet[i].PaymentType);
                                Dictionary<string, string> details = null;
                                if (RcptDet[i].PaymentType.ToUpper().Trim() == POSController.PAY_NETSFlashPay.ToUpper().Trim() )
                                {
                                    if (responseInfo.ContainsKey("Trans Data"))
                                    {
                                        string str = responseInfo["Trans Data"];
                                        string cepasVersion = "CEPAS 2.0";

                                        if (responseInfo.ContainsKey("CEPAS Version"))
                                        {
                                            cepasVersion = responseInfo["CEPAS Version"];
                                        }

                                        TransDataFormat transData = new TransDataFormat(str, cepasVersion);
                                        details = transData.getDetails();
                                    }
                                }

                                foreach (var rf in receiptFields)
                                {
                                    // transactionamount 
                                    if (RcptDet[i].PaymentType.ToUpper().Trim() == POSController.PAY_NETSFlashPay.ToUpper().Trim() && rf.Name == "Transaction Amount")
                                    {
                                        if (details.ContainsKey("Transaction Amount"))
                                        {
                                            e.Graphics.DrawString
                                            ("Purchase" + ":",
                                            largerPrintFontBold, Brushes.Black, Padding, y);
                                            e.Graphics.DrawString(details["Transaction Amount"],
                                                largerPrintFontBold, Brushes.Black,
                                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                                RightAlign);
                                            y += RowHeight+2;
                                        }
                                        continue;
                                    }

                                    // Transaction Date 
                                    if (RcptDet[i].PaymentType.ToUpper().Trim() == POSController.PAY_NETSFlashPay.ToUpper().Trim() && rf.Name == "Transaction Date")
                                    {
                                        if (details.ContainsKey("Transaction Date"))
                                        {
                                            e.Graphics.DrawString
                                            ("Transaction Date" + ":",
                                            printFont, Brushes.Black, Padding, y);
                                            e.Graphics.DrawString(details["Transaction Date"],
                                                printFont, Brushes.Black,
                                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                                RightAlign);
                                            y += RowHeight;
                                        }
                                        continue;
                                    }

                                    // Transaction Time 
                                    if (RcptDet[i].PaymentType.ToUpper().Trim() == POSController.PAY_NETSFlashPay.ToUpper().Trim() && rf.Name == "Transaction Time")
                                    {
                                        if (details.ContainsKey("Transaction Time"))
                                        {
                                            e.Graphics.DrawString
                                            ("Transaction Time" + ":",
                                            printFont, Brushes.Black, Padding, y);
                                            e.Graphics.DrawString(details["Transaction Time"],
                                                printFont, Brushes.Black,
                                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                                RightAlign);
                                            y += RowHeight;
                                        }
                                        continue;
                                    }

                                    if (responseInfo.ContainsKey(rf.Name))
                                    {
                                        if (rf.Name == "CAN")
                                        {
                                            string str = responseInfo[rf.Name];
                                            string result = str;
                                            if (str.Length == 16)
                                            {
                                                result = String.Format("{0} {1} {2} {3}", str.Substring(0, 4), str.Substring(4, 4), str.Substring(8, 4), str.Substring(12));
                                            }
                                            e.Graphics.DrawString
                                            ("CAN" + ":",
                                            printFont, Brushes.Black, Padding, y);
                                            e.Graphics.DrawString(result,
                                                printFont, Brushes.Black,
                                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                                RightAlign);
                                            y += RowHeight;
                                        }
                                        else
                                        {
                                            if (rf.Name == "Transaction Amount")
                                            {
                                                e.Graphics.DrawString
                                                    (rf.Description + ":",
                                                    largerPrintFontBold, Brushes.Black, Padding, y);
                                                e.Graphics.DrawString(responseInfo[rf.Name],
                                                    largerPrintFontBold, Brushes.Black,
                                                    new RectangleF(Padding, y, PageWidth - Padding, 30),
                                                    RightAlign);
                                                y += RowHeight + 2;
                                            }
                                            else
                                            {
                                                e.Graphics.DrawString
                                                    (rf.Description + ":",
                                                    printFont, Brushes.Black, Padding, y);
                                                e.Graphics.DrawString(responseInfo[rf.Name],
                                                    printFont, Brushes.Black,
                                                    new RectangleF(Padding, y, PageWidth - Padding, 30),
                                                    RightAlign);
                                                y += RowHeight;
                                            }
                                        }
                                    }
                                }
                                

                            }
                        }
                    }
                }
                #endregion

                y += RowHeight + 3;


                /*
                if (CompanyInfo.GSTRegNo != null && CompanyInfo.GSTRegNo != "" && CompanyInfo.GSTRegNo != "-")
                {
                    e.Graphics.DrawString
                                ("GST " + myGST * 100 + "%: $" + pos.GetGSTAmount().ToString("N2"),
                                printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight * 2;
                }*/
                
                #region *) Print Sales Person Info
                if (PrintSettingInfo.receiptSetting.ShowSalesPersonInfo)
                {
                    if (AppSetting.GetSettingFromDBAndConfigFile("ReceiptSalesPersonMessage") != null &&
                        AppSetting.GetSettingFromDBAndConfigFile("ReceiptSalesPersonMessage") != "")
                    {
                        item = AppSetting.GetSettingFromDBAndConfigFile("ReceiptSalesPersonMessage") + " " + pos.GetSalesPerson();
                    }
                    else
                    {
                        item = "Your " + PrintSettingInfo.receiptSetting.SalesPersonTitle + " was " + pos.GetSalesPerson();
                    }
                    int TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                    e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                    y += TextHeight;
                }
                #endregion


                #region Print Remarks
                string printRemark = "";
                if (AppSetting.GetSetting("PrintRemarkOnReceipt") != null)
                {
                    printRemark = AppSetting.GetSetting("PrintRemarkOnReceipt").ToString();
                }
                if (pos.GetRemarks() != "" && printRemark.ToLower() != "no")
                {
                    int TextHeight = (int)e.Graphics.MeasureString(pos.GetRemarks(), printFont, PageWidth).Height;
                    e.Graphics.DrawString(pos.GetRemarks(), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                    y += TextHeight;
                    y += 5;
                }

                if (pos.GetReturnedReceiptNo() != "")
                {
                    int TextHeight = (int)e.Graphics.MeasureString("Returned Receipt No : " + pos.GetReturnedReceiptNo(), printFont, PageWidth).Height;
                    e.Graphics.DrawString("Original Receipt No : " + pos.GetReturnedReceiptNo(), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                    y += TextHeight;
                    y += 5;
                }
                #endregion

                #region Membership Information
                if (PrintSettingInfo.receiptSetting.ShowMembershipInfo && pos.MembershipApplied())
                //if (PrintSettingInfo.receiptSetting.ShowMembershipInfo && pos.MembershipApplied_PlusWalkIn)
                {
                    string membershipNo = "-";
                    string membershipName = "-";
                    string BirthdayMessage = "";
                    decimal pointInitial = 0;
                    decimal pointChanged = 0;
                    decimal pointRemaining = 0;
                    //if (pos.MembershipApplied())
                    //{
                    Membership myMember = pos.GetMemberInfo();
                    membershipNo = myMember.MembershipNo;
                    membershipName = myMember.NameToAppear;
                    if (myMember.DateOfBirth.HasValue)
                    {
                        if (myMember.DateOfBirth.Value.Month == DateTime.Today.Month)
                        {
                            BirthdayMessage = "Happy Birthday!";
                        }
                    }

                    //OrderHdr myOrderHdr = new OrderHdr(OrderHdr.Columns.OrderRefNo, pos.GetUnsavedRefNo());
                    //pointInitial = myOrderHdr.InitialPoint.GetValueOrDefault(0);
                    //pointChanged = myOrderHdr.PointAmount.GetValueOrDefault(0);
                    //pointRemaining = pointInitial + pointChanged;
                    //}
                    
                    #region *) Get Initial Point and Points Earned
                    if (!string.IsNullOrEmpty(membershipNo) && membershipNo != "WALK-IN")
                    {
                        PowerPOS.Feature.Package.GetInitialPointsAndPointsEarned(membershipNo, pos, out pointInitial, out pointChanged);
                        pointRemaining = pointInitial + pointChanged;
                    }
                    #endregion

                    e.Graphics.DrawString
                    ("Membership No:" + membershipNo,
                    printFont, Brushes.Black, Padding, y);
                    y += RowHeight;

                    item = "Name:" + membershipName;
                    int TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                    e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                    y += RowHeight;

                    if (pointChanged != 0)
                    {
                        y += RowHeight;

                        e.Graphics.DrawString("Today Point Information:", totalFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                        y += (int)totalFont.GetHeight();

                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);

                        e.Graphics.DrawString("Opening Balance", printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                        e.Graphics.DrawString(pointInitial.ToString("N2"), printFont, Brushes.Black, new RectangleF(Padding + 100, y, PageWidth - (Padding + 100), TextHeight), RightAlign);
                        y += TextHeight;

                        if (pointChanged > 0)
                            e.Graphics.DrawString("Point Earned", printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                        else if (pointChanged < 0)
                            e.Graphics.DrawString("Point Used", printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                        e.Graphics.DrawString(Math.Abs(pointChanged).ToString("N2"), printFont, Brushes.Black, new RectangleF(Padding + 100, y, PageWidth - (Padding + 100), TextHeight), RightAlign);
                        y += TextHeight;

                        e.Graphics.DrawString("Remaining Point", printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                        e.Graphics.DrawString(pointRemaining.ToString("N2"), printFont, Brushes.Black, new RectangleF(Padding + 100, y, PageWidth - (Padding + 100), TextHeight), RightAlign);
                        y += TextHeight;

                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);

                        y += TextHeight;
                    }
                    
                    e.Graphics.DrawString
                    (BirthdayMessage,
                    printFont, Brushes.Black, Padding, y);
                    y += TextHeight;
                }
                #endregion

                #region Balance Payment

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintBalancePaymentOnTheReceipt), false))
                {
                    e.Graphics.DrawString(string.Format("Balance Payment: {0:N}", balancepayment.ToString("N2")),printFont, Brushes.Black, Padding, y);
                    y += RowHeight;
                }

                #endregion

                //e.Graphics.DrawString("THANK YOU! PLEASE COME AGAIN.", printFont,
                //Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                //CenterAlign);


                #region *) Print Term&Condition
                if (PrintSettingInfo.receiptSetting.TermCondition1.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition1, termsConditionFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 90),
                                  CenterAlign);
                    y += (int)termsConditionFont.GetHeight();
                }

                if (PrintSettingInfo.receiptSetting.TermCondition2.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition2, termsConditionFont,
                        Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 90),
                        CenterAlign);
                    y += (int)termsConditionFont.GetHeight();
                }

                if (PrintSettingInfo.receiptSetting.TermCondition3.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition3, termsConditionFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 90),
                    CenterAlign);
                    y += (int)termsConditionFont.GetHeight();
                }

                if (PrintSettingInfo.receiptSetting.TermCondition4.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition4, termsConditionFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                    CenterAlign);
                    y += (int)termsConditionFont.GetHeight();
                }

                if (PrintSettingInfo.receiptSetting.TermCondition5.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition5, termsConditionFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                    CenterAlign);
                    y += (int)termsConditionFont.GetHeight();
                }

                if (PrintSettingInfo.receiptSetting.TermCondition6.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition6, termsConditionFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                    CenterAlign);
                    y += (int)termsConditionFont.GetHeight();
                }
                #endregion
                y += RowHeight + 2;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.ShowReceiptNoBarcode), false))
                {
                    int barcodeFontSize = 0;
                    if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.BarcodeFontSize), out barcodeFontSize))
                    {
                        barcodeFontSize = 22;
                    }
                    Font BarcodeFont = new Font("Free 3 Of 9 Extended", barcodeFontSize);
                    
                    int barcodeHeight = (int)e.Graphics.MeasureString("*" + pos.GetCustomizedRefNo() +"*", BarcodeFont).Height;
                    e.Graphics.DrawString("*" + pos.GetCustomizedRefNo() + "*", BarcodeFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), CenterAlign);
                    y += barcodeHeight;

                }

                /*e.Graphics.DrawString("Ref no:" + pos.GetCustomizedRefNo(), printFont,
                Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                CenterAlign);*/
                /*e.Graphics.DrawString(pos.GetCustomizedRefNo(), printFont,
                Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                CenterAlign);
                y += RowHeight + 2;*/
                e.Graphics.DrawString(pos.GetOrderDate().ToString("dd MMM yyyy HH:mm:ss"), printFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                CenterAlign);
                y += 2 * (RowHeight + 2);
                /*
                e.Graphics.DrawString("*" + pos.GetUnsavedRefNo() + "*", BarcodeFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                CenterAlign);
                y += (int)BarcodeFont.GetHeight();
                */
                bool HidePrintPackageBalanceOnReceipt = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.HidePrintPackageBalanceOnReceipt), false);

                if (PrintSettingInfo.receiptSetting.ShowMembershipInfo && pos.MembershipApplied() && pos.GetMemberInfo().MembershipNo != "WALK-IN")
                {
                    DataTable CurrentAmounts;
                    PowerPOS.Feature.Package.GetCurrentAmountLocal(pos.CurrentMember.MembershipNo, DateTime.Now, out CurrentAmounts, out status);

                    #region *) Make sure the PointTempLog table is cleaned
                    PointTempLogController.Clean();
                    #endregion

                    // Include point changes from PointTempLog into the CurrentAmounts.
                    // (if PointTempLog got data that means the point has not been synced to posdb yet)
                    DataTable dtPTL;
                    PowerPOS.Feature.Package.AllocateNewlyEarnedPointsToCurrentAmounts(CurrentAmounts, pos.myOrderHdr.MembershipNo, out dtPTL, out status);
                    CurrentAmounts = dtPTL.Copy();

                    status = ""; /// Clear aja.. :)

                    #region *) Print Current Point/Package Info Detail
                    if (!HidePrintPackageBalanceOnReceipt)
                    {
                        if (CurrentAmounts != null && CurrentAmounts.Rows.Count > 0)
                        {
                            y += RowHeight + 2;
                            e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                            y += RowHeight + 2;

                            #region *) Print Current Point/Package Info Header
                            e.Graphics.DrawString
                            ("Current Balance",
                            companyFont, Brushes.Black, Padding, y);
                            y += (int)companyFont.GetHeight();
                            y += (int)printFont.GetHeight();
                            Membership m = new Membership(pos.CurrentMember.MembershipNo);
                            string tmpDate = m.LastSyncPointsDate;
                            if (tmpDate == null || tmpDate == "") tmpDate = DateTime.Now.ToString("dd MMM yy");
                            e.Graphics.DrawString(tmpDate,
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y - RowHeight, PageWidth - Padding, 30),
                                RightAlign);
                            #endregion
                            e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);

                            for (int i = 0; i < CurrentAmounts.Rows.Count; i++)
                            {
                                item = CurrentAmounts.Rows[i][0].ToString();
                                int TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                                e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                                y += TextHeight;

                                decimal Points = 0;
                                decimal.TryParse(CurrentAmounts.Rows[i][1].ToString(), out Points);

                                e.Graphics.DrawString
                                    (String.Format("{0:N}", "Remaining :"),
                                    printFont, Brushes.Black, Padding + RowHeight, y);
                                if (CurrentAmounts.Rows[i][2].ToString() == Item.PointMode.Times) /// Do not show [$] sign
                                    e.Graphics.DrawString(Points.ToString("N0") + " Times",
                                        printFont, Brushes.Black,
                                        new RectangleF(Padding, y, PageWidth - Padding, 30),
                                        RightAlign);
                                else
                                    e.Graphics.DrawString(Points.ToString("N2") + " Pt",
                                        printFont, Brushes.Black,
                                        new RectangleF(Padding, y, PageWidth - Padding, 30),
                                        RightAlign);

                                y += RowHeight;

                                y += RowHeight / 2;
                            }

                            e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                            y += RowHeight + 2;
                        }
                    }
                    #endregion

                    #region Print Signature
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false)
                    || AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailable), false))
                    {
                        //receipt ref num is the image name
                        string signatureFile = Application.StartupPath + "\\Signature\\sign" + pos.GetUnsavedRefNo() + ".jpg";

                        //take the image from the folder and print it
                        if (System.IO.File.Exists(signatureFile))
                        {
                            e.Graphics.DrawString("Signature", printFont, Brushes.Black, Padding, y);
                            y += RowHeight;

                            Bitmap MyBitmap = new Bitmap(signatureFile);
                            int xPos;
                            xPos = (PageWidth / 2) - MyBitmap.Width / 2;
                            if (xPos < 0) xPos = 0;
                            e.Graphics.DrawImage(MyBitmap, new RectangleF(xPos, y,
                                         MyBitmap.Width, MyBitmap.Height));
                            y += MyBitmap.Height;
                        }
                    }
                    #endregion
                    y += RowHeight + 2;
                }
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print page failed!", E);
            }
        }

        private void printPreOrderReceipt(Object sender,
                        PrintPageEventArgs e)
        {
            try
            {
                int y = 0;

                //Double Total = 0;

                System.Drawing.StringFormat RightAlign =
                    new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                RightAlign.Alignment = System.Drawing.StringAlignment.Far;

                System.Drawing.StringFormat CenterAlign =
                    new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                CenterAlign.Alignment = System.Drawing.StringAlignment.Center;

                if (System.IO.File.Exists("Logo.jpg"))
                {
                    Bitmap MyBitmap = new Bitmap("Logo.jpg");
                    int xPos;
                    xPos = (PageWidth / 2) - MyBitmap.Width / 2;
                    if (xPos < 0) xPos = 0;
                    e.Graphics.DrawImage(MyBitmap, new RectangleF(xPos, y,
                                 MyBitmap.Width, MyBitmap.Height));
                    y += MyBitmap.Height;
                }
                y += 5;

                if (CompanyInfo.ReceiptName != "")
                {
                    e.Graphics.DrawString(CompanyInfo.ReceiptName, headerFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                if (PrintSettingInfo.receiptSetting.ReceiptAddress1 != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.ReceiptAddress1, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }

                if (PrintSettingInfo.receiptSetting.ReceiptAddress2 != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.ReceiptAddress2, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                if (PrintSettingInfo.receiptSetting.ReceiptAddress3 != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.ReceiptAddress3, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                if (PrintSettingInfo.receiptSetting.ReceiptAddress4 != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.ReceiptAddress4, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                if (CompanyInfo.GSTRegNo != "")
                {
                    e.Graphics.DrawString("GST Reg no. " + CompanyInfo.GSTRegNo, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }

                /*
                if (CompanyInfo.ReceiptName != "")
                {
                    e.Graphics.DrawString(CompanyInfo.ReceiptName, headerFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                y += 10;*/

                string item;
                e.Graphics.DrawString
                    ("Pre-Order Item",
                        printFont, Brushes.Black, Padding, y);

                y += RowHeight + 2;
                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                OrderDetCollection det = pos.FetchUnsavedOrderDet();
                for (int i = 0; i < det.Count; i++)
                {
                    if (!det[i].IsVoided && det[i].IsPreOrder.HasValue && det[i].IsPreOrder.Value)
                    {
                        item = det[i].ItemNo + "-" + det[i].Item.ItemName;
                        if (item.Length <= 35)
                        {
                            e.Graphics.DrawString(item,
                                printFont, Brushes.Black, Padding, y);
                            y += RowHeight + 2;
                        }
                        else
                        {
                            e.Graphics.DrawString(item.Substring(0, 35),
                                printFont, Brushes.Black, Padding, y);
                            y += RowHeight + 2;
                            e.Graphics.DrawString(item.Substring(35),
                                printFont, Brushes.Black, Padding, y);
                            y += RowHeight + 2;
                        }

                        e.Graphics.DrawString
                            ("Quantity:",
                            printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString
                            ("\t\t" + det[i].Quantity,
                            printFont, Brushes.Black, Padding, y);
                        y += RowHeight + 2;
                    }
                }
                y += RowHeight * 2;

                //Show membership
                if (pos.MembershipApplied())
                {
                    if (pos.CurrentMember != null)
                    {
                        e.Graphics.DrawString
                            ("Membership No:" + pos.CurrentMember.MembershipNo,
                            printFont, Brushes.Black, Padding, y);
                        y += RowHeight;

                    }
                }
                if (pos.hasPreOrder())
                {
                    PreOrderRecord pr = pos.getPreOrderInfo();
                    e.Graphics.DrawString
                        ("Name :" + pr.Name,
                        printFont, Brushes.Black, Padding, y);
                    y += RowHeight;
                    e.Graphics.DrawString
                        ("Contact No:" + pr.ContactNo,
                        printFont, Brushes.Black, Padding, y);
                    y += RowHeight;

                }
                y += RowHeight;

                if (PrintSettingInfo.receiptSetting.TermCondition1.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition1, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 90),
                                  CenterAlign);
                    y += RowHeight;
                }

                if (PrintSettingInfo.receiptSetting.TermCondition2.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition2, printFont,
                        Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 90),
                        CenterAlign);
                    y += RowHeight;
                }

                if (PrintSettingInfo.receiptSetting.TermCondition3.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition3, printFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 90),
                    CenterAlign);
                    y += RowHeight;
                }

                if (PrintSettingInfo.receiptSetting.TermCondition4.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition4, printFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                    CenterAlign);
                    y += RowHeight;
                }

                if (PrintSettingInfo.receiptSetting.TermCondition5.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition5, printFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                    CenterAlign);
                    y += RowHeight;
                }

                if (PrintSettingInfo.receiptSetting.TermCondition6.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition6, printFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                    CenterAlign);
                    y += RowHeight;
                }
                e.Graphics.DrawString("Ref no:" + pos.GetUnsavedRefNo(), printFont,
                Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                CenterAlign);
                y += RowHeight + 2;
                e.Graphics.DrawString(DateTime.Now.ToString("dd MMM yyyy HH:mm:ss"), printFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                CenterAlign);
                y += RowHeight + 2;
                y += RowHeight + 2;
                e.Graphics.DrawString("*" + pos.GetUnsavedRefNo() + "*", BarcodeFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                CenterAlign);

            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print page failed!", E);
            }
        }

        private void printNewSignUpReceipt(Object sender,
                        PrintPageEventArgs e)
        {
            try
            {
                int y = 0;

                System.Drawing.StringFormat RightAlign =
                    new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                RightAlign.Alignment = System.Drawing.StringAlignment.Far;

                System.Drawing.StringFormat CenterAlign =
                    new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                CenterAlign.Alignment = System.Drawing.StringAlignment.Center;

                if (System.IO.File.Exists("Logo.jpg"))
                {
                    Bitmap MyBitmap = new Bitmap("Logo.jpg");
                    int xPos;
                    xPos = (PageWidth / 2) - MyBitmap.Width / 2;
                    if (xPos < 0) xPos = 0;
                    e.Graphics.DrawImage(MyBitmap, new RectangleF(xPos, y,
                                 MyBitmap.Width, MyBitmap.Height));
                    y += MyBitmap.Height;
                }
                y += 5;

                if (CompanyInfo.ReceiptName != "")
                {
                    e.Graphics.DrawString(CompanyInfo.ReceiptName, headerFont,
                              Brushes.Black, new RectangleF(Padding, y,
                                  PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                y += 30;
                e.Graphics.DrawString
                    ("New Sign up/Renewal",
                        largerPrintFont, Brushes.Black, Padding, y);
                y += RowHeight * 2;
                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);

                Membership myMember = pos.CurrentMember;

                e.Graphics.DrawString
                          ("No:" + myMember.MembershipNo,
                              largerPrintFont, Brushes.Black, Padding, y);
                y += 30;
                e.Graphics.DrawString
                          ("Name:" + myMember.NameToAppear,
                              largerPrintFont, Brushes.Black, Padding, y);
                y += 30;
                e.Graphics.DrawString
                          ("Expiry:" + myMember.ExpiryDate.Value.ToString("dd MMM yy"),
                              largerPrintFont, Brushes.Black, Padding, y);
                y += 30;

                e.Graphics.DrawString
                          ("Contact:" + myMember.Mobile,
                              largerPrintFont, Brushes.Black, Padding, y);
                y += 30;
                e.Graphics.DrawString("*" + myMember.MembershipNo + "*", BarcodeFont,
                                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                CenterAlign);
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print page failed!", E);
            }
        }
        
        public void PrintPackageRedemption(PackageRedemptionLog PrintItem)
        {
            try
            {
                myPackageLog = PrintItem;

                PrintDocument printDoc = new PrintDocument();
                if (PrinterName.CompareTo("") != 0)
                {
                    printDoc.PrinterSettings.PrinterName = PrinterName;
                }
                printDoc.PrintPage += new PrintPageEventHandler(printPackagePOSReceipt);

                printDoc.Print();
            }
            catch (Exception X)
            {
                Logger.writeLog("Print Receipt failed! Funtion [ReceiptPrinter.Ahava.cs\\PrintPackageRedemption] - Details on the next Log");
                Logger.writeLog(X);
                return;
            }
        }

        private void printPackagePOSReceipt(Object sender,
                PrintPageEventArgs e)
        {
            try
            {
                int y = 0;

                int RowHeight = (int)printFont.GetHeight();

                StringFormat RightAlign = new StringFormat(StringFormat.GenericTypographic);
                RightAlign.Alignment = StringAlignment.Far;

                StringFormat CenterAlign = new StringFormat(StringFormat.GenericTypographic);
                CenterAlign.Alignment = StringAlignment.Center;

                #region *) Print Logo
                if (File.Exists("Logo.jpg"))
                {
                    Bitmap MyBitmap = new Bitmap("Logo.jpg");
                    int xPos;
                    xPos = (PageWidth / 2) - MyBitmap.Width / 2;
                    if (xPos < 0) xPos = 0;
                    e.Graphics.DrawImage(MyBitmap, new RectangleF(xPos, y,
                                 MyBitmap.Width, MyBitmap.Height));
                    y += MyBitmap.Height;
                }
                #endregion

                #region *) Print Company Name
                if (CompanyInfo.ReceiptName != "")
                {
                    e.Graphics.DrawString(CompanyInfo.ReceiptName, companyFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += (int)companyFont.GetHeight();
                }
                #endregion
                #region *) Print Company Address
                if (PrintSettingInfo.receiptSetting.ReceiptAddress1 != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.ReceiptAddress1, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }

                if (PrintSettingInfo.receiptSetting.ReceiptAddress2 != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.ReceiptAddress2, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                if (PrintSettingInfo.receiptSetting.ReceiptAddress3 != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.ReceiptAddress3, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                if (PrintSettingInfo.receiptSetting.ReceiptAddress4 != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.ReceiptAddress4, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                #endregion
                #region *) Print GST Registration No
                if (CompanyInfo.GSTRegNo != null && CompanyInfo.GSTRegNo != "" && CompanyInfo.GSTRegNo != "-")
                {
                    e.Graphics.DrawString("GST Reg no. " + CompanyInfo.GSTRegNo, printFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                  CenterAlign);
                    y += RowHeight;
                }
                #endregion

                string item;
                #region *) Print Column Header for Items data
                y += 10;
                e.Graphics.DrawString
                    ("Package",
                        printFont, Brushes.Black, Padding, y);
                //e.Graphics.DrawString
                //    ("\t\t" + "Left",
                //        printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString("Remaining",
                    printFont, Brushes.Black,
                    new RectangleF(Padding, y, PageWidth - Padding, 30),
                    RightAlign);
                y += RowHeight + 2;
                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                #endregion
                #region *) Print Items Data
                y += 2;

                string[] JointInfo = myPackageLog.Name.Split(new char[] { ',' }, 3);

                #region *) Retrieve Package Info
                item = JointInfo[0];
                #endregion

                int TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                y += TextHeight;

                e.Graphics.DrawString(String.Format("{0:N}", (JointInfo.Length > 1) ? JointInfo[1] : "0"),
                    printFont, Brushes.Black,
                    new RectangleF(Padding, y, PageWidth - Padding, 30),
                    RightAlign);
                y += RowHeight + 2;
                #endregion

                string status;

                #region *) Print Stylist Info
                if (PrintSettingInfo.receiptSetting.ShowSalesPersonInfo)
                {
                    y += RowHeight + 5;

                    item = "Your " + PrintSettingInfo.receiptSetting.SalesPersonTitle + " was " + myPackageLog.StylistId;
                    TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                    e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                    y += TextHeight;
                }
                #endregion

                #region *) Print Cashier Info [Not Implemented]
                //e.Graphics.DrawString
                //("Cashier:" + pos.GetCashierName(),
                //printFont, Brushes.Black, Padding, y);
                y += RowHeight + 5;
                #endregion

                #region *) Print Membership Info
                if (PrintSettingInfo.receiptSetting.ShowMembershipInfo)
                {
                    bool ActionResult;
                    string membershipNo = "-";
                    string membershipName = "-";
                    string BirthdayMessage = "";
                    string pointRemaining = "-";

                    Membership myMember = new Membership(myPackageLog.MembershipNo);

                    membershipNo = string.IsNullOrEmpty(myMember.MembershipNo) ? myPackageLog.MembershipNo : myMember.MembershipNo;
                    membershipNo = string.IsNullOrEmpty(membershipNo) ? "-" : membershipNo;

                    e.Graphics.DrawString
                    ("Membership No:" + membershipNo,
                    printFont, Brushes.Black, Padding, y);
                    y += RowHeight;

                    if (!(myMember == null | myMember.IsNew | !myMember.IsLoaded))
                    {
                        membershipName = myMember.NameToAppear;
                        if (myMember.DateOfBirth.HasValue)
                        {
                            if (myMember.DateOfBirth.Value.Month == DateTime.Today.Month)
                            {
                                BirthdayMessage = "Happy Birthday!";
                            }
                        }

                        item = "Name:" + membershipName;
                        TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                        e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                        y += RowHeight;

                        e.Graphics.DrawString
                        (BirthdayMessage,
                        printFont, Brushes.Black, Padding, y);
                        y += RowHeight;
                    }
                    y += RowHeight;
                }
                #endregion

                e.Graphics.DrawString("THANK YOU! PLEASE COME AGAIN.", printFont,
                Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                CenterAlign);

                y += RowHeight + 3;

                #region *) Print Terms&Condition
                if (PrintSettingInfo.receiptSetting.TermCondition1.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition1, termsConditionFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 90),
                                  CenterAlign);
                    y += (int)termsConditionFont.GetHeight();
                }

                if (PrintSettingInfo.receiptSetting.TermCondition2.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition2, termsConditionFont,
                        Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 90),
                        CenterAlign);
                    y += (int)termsConditionFont.GetHeight();
                }

                if (PrintSettingInfo.receiptSetting.TermCondition3.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition3, termsConditionFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 90),
                    CenterAlign);
                    y += (int)termsConditionFont.GetHeight();
                }

                if (PrintSettingInfo.receiptSetting.TermCondition4.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition4, termsConditionFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                    CenterAlign);
                    y += (int)termsConditionFont.GetHeight();
                }

                if (PrintSettingInfo.receiptSetting.TermCondition5.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition5, termsConditionFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                    CenterAlign);
                    y += (int)termsConditionFont.GetHeight();
                }

                if (PrintSettingInfo.receiptSetting.TermCondition6.Trim() != "")
                {
                    e.Graphics.DrawString(PrintSettingInfo.receiptSetting.TermCondition6, termsConditionFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                    CenterAlign);
                    y += (int)termsConditionFont.GetHeight();
                }
                #endregion
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print page failed!", E);
            }
        }


        private void printCashRecordingSlipHandler(Object sender,
                PrintPageEventArgs e)
        {
            try
            {
                int y = 0;

                int RowHeight = (int)printFont.GetHeight();

                StringFormat RightAlign = new StringFormat(StringFormat.GenericTypographic);
                RightAlign.Alignment = StringAlignment.Far;

                StringFormat CenterAlign = new StringFormat(StringFormat.GenericTypographic);
                CenterAlign.Alignment = StringAlignment.Center;

                
                #region *) Print Company Name
                if (CompanyInfo.ReceiptName != "")
                {
                    int TxtHeight = (int)e.Graphics.MeasureString(CompanyInfo.ReceiptName, companyFont, PageWidth - Padding).Height;
                    e.Graphics.DrawString(CompanyInfo.ReceiptName, companyFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TxtHeight),
                                  CenterAlign);
                    y += TxtHeight;
                }
                else
                {
                    int TxtHeight = (int)e.Graphics.MeasureString("", companyFont, PageWidth - Padding).Height;
                    e.Graphics.DrawString("", companyFont,
                              Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TxtHeight),
                                  CenterAlign);
                    y += TxtHeight;
                }
                #endregion

                int TextHeight = (int)e.Graphics.MeasureString("Cashier   : " + UserInfo.username, printFont, PageWidth).Height;
                e.Graphics.DrawString("Cashier   : " + UserInfo.username, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                y += TextHeight;

                TextHeight = printFont.Height;
                e.Graphics.DrawString("Time       : " + myCashRecording.CashRecordingTime.ToString("d MMM yyyy"), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));

                y += TextHeight;

                e.Graphics.DrawString("                " + myCashRecording.CashRecordingTime.ToString("h:mm:ss tt"), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));

                y += TextHeight;

                if (myCashRecording != null && myCashRecording.IsLoaded)
                {
                    TextHeight = (int)e.Graphics.MeasureString("", printFont, PageWidth).Height;
                    e.Graphics.DrawString(myCashRecording.CashRecordingType.CashRecordingTypeName + " : " + String.Format("{0:N}", myCashRecording.Amount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                    y += TextHeight;

                    //(int)e.Graphics.MeasureString(items[p], kitchenFont, e.PageBounds.Width - (2 * Padding)).Height
                    TextHeight = (int)e.Graphics.MeasureString("Remarks  : " + myCashRecording.Remark, printFont, PageWidth - Padding).Height;
                    y += RowHeight + 2;
                    e.Graphics.DrawString("Remarks  : " + myCashRecording.Remark, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                    y += TextHeight;
                    y += RowHeight + 2;

                    if (myCashRecording.CashRecordingType.CashRecordingTypeName.ToLower() == "cash out" || myCashRecording.CashRecordingType.CashRecordingTypeName.ToLower() == "cash in")
                    {
                        if (myCashRecording.SupervisorName != null && myCashRecording.SupervisorName != string.Empty)
                        {
                            e.Graphics.DrawString("Authorized by  : " + myCashRecording.SupervisorName, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                            y += RowHeight + 2;
                        }
                        else
                        {
                            e.Graphics.DrawString("Authorized by  : -", printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                            y += RowHeight + 2;
                        }
                    }

                    e.Graphics.DrawString("Staff Signature: ", printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                    y += RowHeight;
                    y += 2;
                    y += RowHeight * 7;
                    e.Graphics.DrawLine(new Pen(Color.Black), Padding, y, PageWidth - Padding, y);
                }
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print page failed!", E);
            }
        }

        private Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        internal void PrintCashRecordingSlip()
        {
            try
            {
                
                PrintDocument printDoc = new PrintDocument();
                if (PrinterName.CompareTo("") != 0)
                {
                    printDoc.PrinterSettings.PrinterName = PrinterName;
                }
                printDoc.PrintPage += new PrintPageEventHandler(printCashRecordingSlipHandler);

                printDoc.Print();
            }
            catch (Exception X)
            {
                Logger.writeLog("Print Receipt failed! Funtion [ReceiptPrinter.Ahava.cs\\PrintCashRecordingSlip] - Details on the next Log");
                Logger.writeLog(X);
                return;
            }
        }

        #region z2closing
        /*internal void PrintChangChengCashCollection(CounterCloseLog clog)
        {
            try
            {
                closeLog = clog;

                PrintDocument printDoc = new PrintDocument();
                if (PrinterName.CompareTo("") != 0)
                {
                    printDoc.PrinterSettings.PrinterName = PrinterName;
                }

                printDoc.PrintPage += new PrintPageEventHandler(printRestaurantCollectionPageEventHandler);
                printDoc.Print();

            }
            catch (Exception e)
            {
                throw new PrintReceiptException("Print Cash Collection failed!", e);
            }
        }

        private void printRestaurantCollectionPageEventHandler(Object sender, PrintPageEventArgs e)
        {
            String Counter = closeLog.PointOfSale.PointOfSaleName;
            String Cashier = closeLog.Cashier;
            //String Period = closeLog.StartTime.ToString("dd-MM-yy HH:mm:ss") + " - " + closeLog.EndTime.ToString("dd-MM-yy HH:mm:ss");
            decimal TotalRecorded = closeLog.TotalSystemRecorded;
            decimal ActualCollected = closeLog.TotalActualCollected;
            decimal OpeningAmount = closeLog.OpeningBalance;
            decimal CashIn = closeLog.CashIn;
            decimal CashOut = closeLog.CashOut;

            try
            {
                string curCulture = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
                RowHeight = (int)printFont.GetHeight() + 3;
                System.Globalization.NumberFormatInfo currencyFormat =
                    new System.Globalization.CultureInfo(curCulture).NumberFormat;
                currencyFormat.CurrencyNegativePattern = 1;

                int y = 0;
                System.Drawing.StringFormat RightAlign = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                RightAlign.Alignment = System.Drawing.StringAlignment.Far;

                System.Drawing.StringFormat CenterAlign = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                CenterAlign.Alignment = System.Drawing.StringAlignment.Center;
                //Set the variables

                #region *) Print Company Name
                if (Company.CompareTo("") != 0)
                {
                    e.Graphics.DrawString(Company, headerFont,
                                          Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), CenterAlign);
                    y += (int)headerFont.GetHeight();
                }
                #endregion

                #region *) Print Counter Name - Start Time - End Time
                e.Graphics.DrawString("Counter - " + Counter, printFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight;

                e.Graphics.DrawString("Start " + closeLog.StartTime.ToString("dd-MMM-yyyy HH:mm:ss"), printFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight;
                e.Graphics.DrawString("End " + closeLog.EndTime.ToString("dd-MMM-yyyy HH:mm:ss"), printFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight;
                #endregion

                ClosingController cl = new ClosingController();

                e.Graphics.DrawString("Receipt No:" + cl.GetFirstReceiptNo(closeLog) + " to " + cl.GetLastReceiptNo(closeLog)
                    , printFont, Brushes.Black, Padding, y);
                y += RowHeight;
                e.Graphics.DrawString("Total Receipts:" + cl.GetTotalNumberOfOrder(closeLog)
                    , printFont, Brushes.Black, Padding, y);
                y += RowHeight;
                int count; decimal amount;
                cl.GetTotalNumberOfVoidedOrder(closeLog, out count, out amount);
                e.Graphics.DrawString("Voided Bill:" + count.ToString() + " ($" + amount.ToString("N2") + ")"
                    , printFont, Brushes.Black, Padding, y);
                y += RowHeight;

                e.Graphics.DrawString("Amended Bill:" + cl.GetTotalNumberOfAmendedOrder(closeLog)
                    , printFont, Brushes.Black, Padding, y);
                y += RowHeight;

                e.Graphics.DrawString("Cancelled Item:" + cl.GetTotalNumberOfCancelledItem(closeLog)
                    , printFont, Brushes.Black, Padding, y);
                y += RowHeight;

                e.Graphics.DrawString("Total #Customer:" + cl.GetTotalNumberOfCustomer(closeLog)
                    , printFont, Brushes.Black, Padding, y);
                y += RowHeight;
                e.Graphics.DrawLine(new Pen(Color.Black), 5, y, PageWidth - 5, y);
                e.Graphics.DrawString("Total Sales:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", TotalRecorded),
                    printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                    RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Total Collected:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", ActualCollected), printFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Surplus (+/-):", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", closeLog.TotalActualCollected - closeLog.TotalSystemRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Float:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", closeLog.FloatBalance), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Cash In:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", closeLog.CashIn), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Cash Out:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", closeLog.CashOut), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Float Balance:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", (closeLog.OpeningBalance + closeLog.CashIn - closeLog.CashOut)), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Total Cash Recorded:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", (closeLog.OpeningBalance + closeLog.CashIn - closeLog.CashOut) + closeLog.CashRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Cash In Tray:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", closeLog.ClosingCashOut),
                    printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                    RightAlign);
                y += RowHeight;
                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 3;

                decimal overallDiscount, lineDiscount;
                overallDiscount = cl.GetTotalOverallDiscount(closeLog.StartTime, closeLog.EndTime, closeLog.PointOfSaleID);
                lineDiscount = cl.GetTotalLineDiscount(closeLog.StartTime, closeLog.EndTime, closeLog.PointOfSaleID);

                e.Graphics.DrawString("Gross Sales:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", TotalRecorded + overallDiscount + lineDiscount),
                    printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                //e.Graphics.DrawString("Cash In Tray:", printFont, Brushes.Black, Padding, y);
                //e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", closeLog.ClosingCashOut),
                //    printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                //    RightAlign);
                //y += RowHeight;

                e.Graphics.DrawString("Total Discount:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", overallDiscount + lineDiscount), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                y += 3;
                e.Graphics.DrawLine(new Pen(Color.Black), PageWidth - 75, y, PageWidth, y);
                y += 3;

                decimal NettSales = TotalRecorded;
                //e.Graphics.DrawString("Gross Sales (w/o Discount):", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", TotalRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;




                decimal GSTTotal = cl.GetTotalGST(closeLog, PriceIncludeGST);
                e.Graphics.DrawString("Total GST:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", GSTTotal), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;
                NettSales -= GSTTotal;

                //if (isInclusiveGST)
                //{
                //    e.Graphics.DrawString("Nett Sales w/o GST:", printFont, Brushes.Black, Padding, y);
                //    e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", TotalRecorded - GSTTotal), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                //    y += RowHeight;
                //}

                e.Graphics.DrawString("Service Charge:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", cl.GetTotalServiceCharge(closeLog)), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                NettSales -= cl.GetTotalServiceCharge(closeLog);
                y += RowHeight;

                DataTable dtSubTotal = cl.GetTotalOfSubTotal(closeLog.StartTime, closeLog.EndTime, closeLog.PointOfSaleID);
                if (dtSubTotal != null && dtSubTotal.Rows.Count > 0)
                {
                    //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    //y += 3;
                    for (int i = 0; i < dtSubTotal.Rows.Count; i++)
                    {
                        if (dtSubTotal.Rows[i]["SubTotalType"].ToString().Contains("discount")) continue;
                        if (dtSubTotal.Rows[i]["Name"].ToString().Contains("Rounding")) continue;

                        e.Graphics.DrawString(dtSubTotal.Rows[i]["Name"].ToString(), printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}",
                            (decimal)dtSubTotal.Rows[i]["Amount"]), printFont, Brushes.Black,
                            new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        NettSales -= (decimal)dtSubTotal.Rows[i]["Amount"];
                        y += RowHeight;
                    }
                }

                y += 3;
                e.Graphics.DrawLine(new Pen(Color.Black), PageWidth - 75, y, PageWidth, y);
                y += 3;

                e.Graphics.DrawString("Nett Sales:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", NettSales), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                y += 3;
                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 3;

                e.Graphics.DrawString("ROUNDING:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", cl.GetTotalRounding(closeLog)), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                NettSales -= cl.GetTotalRounding(closeLog);
                y += RowHeight;

                bool FoundDisc = false;

                if (dtSubTotal != null && dtSubTotal.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSubTotal.Rows.Count; i++)
                    {
                        if (!dtSubTotal.Rows[i]["SubTotalType"].ToString().Contains("discount")) continue;

                        if (!FoundDisc)
                        {
                            //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                            //y += 3;
                            //e.Graphics.DrawString("DISCOUNTS:", printFont, Brushes.Black, Padding, y);
                            //y += RowHeight;
                        }

                        e.Graphics.DrawString(dtSubTotal.Rows[i]["Name"].ToString(), printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}",
                            (decimal)dtSubTotal.Rows[i]["Amount"]), printFont, Brushes.Black,
                            new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                }


                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 3;
                e.Graphics.DrawString("Cash:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", closeLog.CashRecorded), printFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", closeLog.CashCollected), printFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", closeLog.CashCollected - closeLog.CashRecorded), printFont,
                    Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;


                //Show multiple payment mode
                for (int i = 1; i < 11; i++)
                {
                    object collected = closeLog.GetColumnValue("Collected" + i.ToString());
                    object recorded = closeLog.GetColumnValue("Recorded" + i.ToString());
                    object label = closeLog.GetColumnValue("Label" + i.ToString());
                    if (collected != null && recorded != null && label != null && ((decimal)collected > 0 || (decimal)recorded > 0))
                    {
                        e.Graphics.DrawString(label.ToString(), printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", (decimal)recorded), printFont,
                            Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        e.Graphics.DrawString("Collected:", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", (decimal)collected), printFont,
                            Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;

                        e.Graphics.DrawString("Surplus(+/-):", printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", (decimal)collected - (decimal)recorded), printFont,
                            Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                        y += RowHeight;
                    }
                }

                e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 3;
                e.Graphics.DrawString("Deposit Bag No:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(closeLog.DepositBagNo, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                //e.Graphics.DrawString(DateTime.Now.ToString(), printFont, Brushes.Black, Padding, y);
                //y += RowHeight;
                DataTable dt;
                if (PrintCategorySalesReport)
                {
                    dt = ReportController.FetchProductCategorySalesReport(
                         closeLog.StartTime, closeLog.EndTime,
                         closeLog.PointOfSale.PointOfSaleName,
                         "%", "%",
                         closeLog.PointOfSale.DepartmentID.Value.ToString(),
                         false, "CategoryName", "ASC");

                    if (dt != null)
                    {
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
                            if (item.Length < 20)
                            {
                                e.Graphics.DrawString(item,
                                    printFont, Brushes.Black, Padding, y);
                                y += RowHeight;
                            }
                            else
                            {
                                e.Graphics.DrawString(item.Substring(0, 20),
                                    printFont, Brushes.Black, Padding, y);
                                y += RowHeight;
                                e.Graphics.DrawString(item.Substring(20),
                                    printFont, Brushes.Black, Padding, y);
                                y += RowHeight;
                            }

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
                if (PrintProductSalesReport)
                {
                    dt = ReportController.FetchProductSalesReport
                        (closeLog.StartTime, closeLog.EndTime, "",
                        closeLog.PointOfSale.PointOfSaleName, "", "",
                        closeLog.PointOfSale.DepartmentID.Value.ToString(), false,
                        "CategoryName", "ASC");
                    if (dt != null)
                    {
                        string item;
                        string categoryname = "";

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
                            if (item.Length <= 20)
                            {
                                e.Graphics.DrawString(item,
                                    printFont, Brushes.Black, Padding, y);
                                y += RowHeight;
                            }
                            else
                            {
                                e.Graphics.DrawString(item.Substring(0, 20),
                                    printFont, Brushes.Black, Padding, y);
                                y += RowHeight;
                                e.Graphics.DrawString(item.Substring(20),
                                    printFont, Brushes.Black, Padding, y);
                                y += RowHeight;
                            }

                            string PrintedQty = dt.Rows[i]["TotalQuantity"].ToString();
                            if (decimal.Parse(dt.Rows[i]["TotalWeight"].ToString()) != 0)
                                PrintedQty += " (" + decimal.Parse(dt.Rows[i]["TotalWeight"].ToString()).ToString("N2")
                                    + " " + dt.Rows[i]["WeightUOM"].ToString() + ")";
                            e.Graphics.DrawString
                                ("\t\t" + PrintedQty,
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

                e.Graphics.DrawString("Closed By:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", new UserMst(closeLog.Cashier).DisplayName), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight + 40;
                e.Graphics.DrawLine(new Pen(Color.Black), 5, y, PageWidth - 5, y);
                y += RowHeight;
                e.Graphics.DrawString("Verified By:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", new UserMst(closeLog.Supervisor).DisplayName), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight + 40;
                e.Graphics.DrawLine(new Pen(Color.Black), 5, y, PageWidth - 5, y);
                y += RowHeight;
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print page failed!", E);
            }
        }*/

        private Z2ClosingLog z2Log;

        public void PrintZ2Report(Z2ClosingLog clog)
        {
            try
            {
                z2Log = clog;
                PrintDocument printDoc = new PrintDocument();
                if (PrinterName.CompareTo("") != 0)
                {
                    printDoc.PrinterSettings.PrinterName = PrinterName;
                }
                printDoc.PrintPage += new PrintPageEventHandler(printCoffeeshopZ2ReportPageEventHandler);
                printDoc.Print();

            }
            catch (Exception e)
            {
                throw new PrintReceiptException("Print Z2 Report failed!", e);
            }
        }

        private void printCoffeeshopZ2ReportPageEventHandler(Object sender, PrintPageEventArgs e)
        {

            String Counter = new PointOfSale(z2Log.PointOfSaleID).PointOfSaleName;
            String Cashier = z2Log.Cashier;
            String Period = z2Log.StartTime.ToString("dd MMM yyyy HH:mm:ss") + " - " + z2Log.EndTime.ToString("dd MMM yyyy HH:mm:ss");
            string startDate = string.Format("Start Date : {0:dd MMM yyyy HH:mm:ss}", z2Log.StartTime);
            string endDate = string.Format("End Date : {0:dd MMM yyyy HH:mm:ss}", z2Log.EndTime);
            decimal TotalRecorded = z2Log.TotalSystemRecorded;
            decimal ActualCollected = z2Log.TotalActualCollected;
            decimal OpeningAmount = z2Log.OpeningBalance;
            decimal CashIn = z2Log.CashIn;
            decimal CashOut = z2Log.CashOut;


            try
            {
                string curCulture = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();

                System.Globalization.NumberFormatInfo currencyFormat =
                    new System.Globalization.CultureInfo(curCulture).NumberFormat;
                currencyFormat.CurrencyNegativePattern = 1;

                int y = 0;
                System.Drawing.StringFormat RightAlign = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                RightAlign.Alignment = System.Drawing.StringAlignment.Far;

                System.Drawing.StringFormat CenterAlign = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                CenterAlign.Alignment = System.Drawing.StringAlignment.Center;
                //Set the variables

                if (Company.CompareTo("") != 0)
                {
                    e.Graphics.DrawString(Company, headerFont,
                                          Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), CenterAlign);
                    y += RowHeight + 5;
                }

                e.Graphics.DrawString("Counter - " + Counter, headerFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight + 5;

                e.Graphics.DrawString(startDate, printFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight + 5;

                e.Graphics.DrawString(endDate, printFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight + 5;

                //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 10;

                e.Graphics.DrawString("Z2 Report", headerFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight + 5;


                //string temp = ClosingController.GetTotalNumberOfCustomer(z2Log.StartTime, z2Log.EndTime, PointOfSaleInfo.PointOfSaleID.ToString());
                //e.Graphics.DrawString("No of Pax:", printFont, Brushes.Black, Padding, y);
                //e.Graphics.DrawString(temp, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                //y += RowHeight;

                string temp1 = ClosingController.GetTotalNumberOfOrder(z2Log.StartTime, z2Log.EndTime, PointOfSaleInfo.PointOfSaleID.ToString());
                e.Graphics.DrawString("No of Bill:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(temp1, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                e.Graphics.DrawString("Total Recorded:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", TotalRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 10;

                e.Graphics.DrawString("Total Collected:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", ActualCollected), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;
                y += 10;

                e.Graphics.DrawString("Surplus (+/-):", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", z2Log.TotalActualCollected - z2Log.TotalSystemRecorded), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;
                y += 10;
               
                e.Graphics.DrawString("Closing Cash Out:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format(currencyFormat, "{0:N}", z2Log.ClosingCashOut), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;


                e.Graphics.DrawString("Deposit Bag No:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(z2Log.DepositBagNo, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight;

                //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 10;


                //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                y += 3;
                //e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);                
                e.Graphics.DrawString(DateTime.Now.ToString(), printFont, Brushes.Black, Padding, y);


                y += RowHeight + 5;
                DataTable dt;

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Z2Report.ShowClosingBreakdownOnZ2Printout), true))
                {
                    e.Graphics.DrawString("Closing Logs", printFontBold, Brushes.Black, Padding, y);
                    //e.Graphics.DrawString(z2Log.DepositBagNo, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;

                    dt = ClosingController.getCounterCloseLogList(z2Log.StartTime, z2Log.EndTime, z2Log.PointOfSaleID);
                    if (dt.Rows.Count > 0)
                    {
                        e.Graphics.DrawString
                       ("Closing Date",
                           printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString("Total",
                            printFont, Brushes.Black,
                            new RectangleF(Padding, y, PageWidth - Padding, 30),
                            RightAlign);
                        y += RowHeight;
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        e.Graphics.DrawString(Convert.ToDateTime(dr["EndTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss"), printFont, Brushes.Black, Padding, y);
                        e.Graphics.DrawString(String.Format("{0:N}", decimal.Parse(dr["TotalActualCollected"].ToString())),
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                RightAlign);
                        y += RowHeight;
                    }
                }

                y += RowHeight + 5;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Z2Report.PrintCategorySalesReportOnZ2Printout), true))
                {
                    dt = ReportController.FetchProductCategorySalesReport(
                         z2Log.StartTime, z2Log.EndTime,
                         Counter,
                         "%", "%",
                         "",
                         false, "CategoryName", "ASC");
                    if (dt != null)
                    {
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
                        y += RowHeight + 2;
                        string item;
                        e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            item = dt.Rows[i]["CategoryName"].ToString();
                            if (item.Length <= 30)
                            {
                                e.Graphics.DrawString(item,
                                    printFont, Brushes.Black, Padding, y);
                                y += RowHeight + 2;
                            }
                            else
                            {
                                e.Graphics.DrawString(item.Substring(0, 30),
                                    printFont, Brushes.Black, Padding, y);
                                y += RowHeight + 2;
                                e.Graphics.DrawString(item.Substring(30),
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
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Z2Report.PrintProductSalesReportOnZ2Printout), true))
                {
                    y += RowHeight + 5;
                    dt = ReportController.FetchProductSalesReport
                        (z2Log.StartTime, z2Log.EndTime, "",
                        Counter, "", "", "", false, "CategoryName", "ASC");
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
                            if (item.Length <= 30)
                            {
                                e.Graphics.DrawString(item,
                                    printFont, Brushes.Black, Padding, y);
                                y += RowHeight + 2;
                            }
                            else
                            {
                                e.Graphics.DrawString(item.Substring(0, 30),
                                    printFont, Brushes.Black, Padding, y);
                                y += RowHeight + 2;
                                e.Graphics.DrawString(item.Substring(30),
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
                }

                y += RowHeight + 2;

                e.Graphics.DrawString("Closed By:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", new UserMst(z2Log.Cashier).DisplayName), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight + 40;
                e.Graphics.DrawLine(new Pen(Color.Black), 5, y, PageWidth - 5, y);
                y += RowHeight;
                e.Graphics.DrawString("Verified By:", printFont, Brushes.Black, Padding, y);
                e.Graphics.DrawString(String.Format("{0:N}", new UserMst(z2Log.Supervisor).DisplayName), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                y += RowHeight + 40;
                e.Graphics.DrawLine(new Pen(Color.Black), 5, y, PageWidth - 5, y);
                y += RowHeight;
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print page failed!", E);
            }
        }

        public void PrintEJReport(CounterCloseLog myLog)
        {
            try
            {
                closeLog = myLog;
                PrintDocument printDoc = new PrintDocument();
                if (PrinterName.CompareTo("") != 0)
                {
                    printDoc.PrinterSettings.PrinterName = PrinterName;
                }

                printDoc.PrinterSettings.PrinterName = PrinterName;
                printDoc.PrintPage += new PrintPageEventHandler(printEJReportEventHandler);
                printDoc.Print();

            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print Receipt failed!", E);
            }
        }

        private void printEJReportEventHandler(Object sender, PrintPageEventArgs e)
        {
            try
            {
                int RowHeight = (int)printFont.GetHeight();
                int y = 0;

                System.Drawing.StringFormat RightAlign = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                RightAlign.Alignment = System.Drawing.StringAlignment.Far;

                System.Drawing.StringFormat CenterAlign = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                CenterAlign.Alignment = System.Drawing.StringAlignment.Center;

                string SQL = "select orderhdrid from orderhdr where pointofsaleid = " + PointOfSaleInfo.PointOfSaleID + " and orderdate > '"
                    + closeLog.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" + closeLog.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                DataTable mydt = DataService.GetDataSet(cmd).Tables[0];
                //smaller
                printFont = new Font(printFont.FontFamily, printFont.Size - 1);
                e.Graphics.DrawString("EJ Report", headerFont,
                          Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                              CenterAlign);
                y += (int)headerFont.GetHeight();


                if (Company.CompareTo("") != 0)
                {
                    e.Graphics.DrawString(Company, headerFont,
                                          Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), CenterAlign);
                    y += (int)headerFont.GetHeight();
                }

                e.Graphics.DrawString("??/Counter - " + closeLog.PointOfSale.PointOfSaleName, printFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight;

                e.Graphics.DrawString("Start " + closeLog.StartTime.ToString("dd-MMM-yyyy HH:mm:ss"), printFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight;
                e.Graphics.DrawString("End " + closeLog.EndTime.ToString("dd-MMM-yyyy HH:mm:ss"), printFont,
                                      Brushes.Black, Padding, y);
                y += RowHeight;

                for (int m = 0; m < mydt.Rows.Count; m++)
                {
                    //Print Receipt No
                    pos = new POSController(mydt.Rows[m][0].ToString());
                    y += 4;
                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += 4;
                    string rcptno = pos.GetUnsavedRefNo();
                    if (rcptno != "ORX")
                    {
                        e.Graphics.DrawString
                        ("Receipt No:" + rcptno,
                            printFont, Brushes.Black, Padding, y);
                        y += RowHeight;
                    }

                    if (pos.IsVoided())
                    {
                        e.Graphics.DrawString("VOID", printFont,
                                  Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                      CenterAlign);
                        y += RowHeight;
                    }
                    if (pos.GetHeaderRemark() != null && pos.GetHeaderRemark() != "")
                    {
                        e.Graphics.DrawString(pos.GetHeaderRemark(), printFont,
                                  Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30),
                                      CenterAlign);
                        y += RowHeight;
                    }
                    string item;
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
                    y += RowHeight;
                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);

                    string status = "";
                    DataTable dt = pos.FetchUnSavedOrderItems(out status);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["IsVoided"].ToString().ToLower() == "false")
                        {
                            //print name
                            item = dt.Rows[i]["ItemName"].ToString();
                            int TextHeight = (int)e.Graphics.MeasureString(item, printFont, PageWidth).Height;
                            e.Graphics.DrawString(item, printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
                            y += TextHeight;

                            e.Graphics.DrawString
                                 (dt.Rows[i]["price"].ToString(),
                                 printFont, Brushes.Black, Padding, y);
                            
                            e.Graphics.DrawString
                                ("\t\t" + dt.Rows[i]["Quantity"].ToString(),
                                printFont, Brushes.Black, Padding, y);
                            

                            e.Graphics.DrawString(dt.Rows[i]["Amount"].ToString(),
                                printFont, Brushes.Black,
                                new RectangleF(Padding, y, PageWidth - Padding, 30),
                                RightAlign);
                            y += RowHeight;
                        }
                    }
                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += 4;
                    decimal TotalAmt = pos.CalculateSubTotalAmount(out status);
                    e.Graphics.DrawString
                                 ("SUB TOTAL",
                                 printFont, Brushes.Black, Padding, y);
                    e.Graphics.DrawString(String.Format("{0:N}", TotalAmt), printFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, 30), RightAlign);
                    y += RowHeight;
                   
                    y += 2;
                    e.Graphics.DrawLine(new Pen(Color.Black), 0, y, PageWidth, y);
                    y += 2;

                    ReceiptDetCollection RcptDet = pos.FetchUnsavedReceipt(out status);
                    if (RcptDet.Count > 0)
                    {
                        for (int i = 0; i < RcptDet.Count; i++)
                        {
                            e.Graphics.DrawString
                                (RcptDet[i].PaymentType,
                                 printFont, Brushes.Black, Padding, y);
                            if (RcptDet[i].PaymentType == POSController.PAY_CASH)
                            {
                                e.Graphics.DrawString(String.Format("{0:N}", RcptDet[i].Amount + change),
                                    printFont, Brushes.Black,
                                    new RectangleF(Padding, y, PageWidth - Padding, 30),
                                    RightAlign);

                                y += RowHeight;
                            }
                            else
                            {
                                e.Graphics.DrawString(String.Format("{0:N}", RcptDet[i].Amount),
                                    printFont, Brushes.Black,
                                    new RectangleF(Padding, y, PageWidth - Padding, 30),
                                    RightAlign);

                                y += RowHeight;
                            }
                        }
                    }
                }
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print page failed!", E);
            }
        }
        #endregion

        #region SICC Validation Printing
        public void PrintAHAVAValidationReceipt
            (POSController mypos, decimal changeAmount, ReceiptSizes size, int NumOfCopies)
        {
            try
            {

                if (size == ReceiptSizes.A4) //default validation print is in A4
                {
                    pos = mypos;

                    pos = mypos;
                    change = changeAmount;
                    PrintDocument printDoc = new PrintDocument();
                    if (PrinterName.CompareTo("") != 0)
                    {
                        printDoc.PrinterSettings.PrinterName = PrinterNameValidation;
                    }
                    printDoc.PrintPage += new PrintPageEventHandler(printAHAVAPOSValidationReceipt);
                    int ValidationNumOfCopies = 1;
                    if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.SICC.ValidationNumOfCopies), out ValidationNumOfCopies))
                    {
                        ValidationNumOfCopies = 1;
                    }
                    //else
                    for (int i = 0; i < ValidationNumOfCopies; i++)
                    {
                        printDoc.Print();
                    }

                    //GenericReport.NewPrint.PrintController.PrintInvoice(PrinterName, NumOfCopies, GenericReport.PageType.A4, pos);
                    //GenericReport.PageController.GenerateReport(pos, GenericReport.PageType.A4, false, GenericReport.ReportType.INVOICE);                   
                }
                else if (size == ReceiptSizes.Receipt)
                {

                }
            }
            catch (Exception E)
            {
                Logger.writeLog("Print Validation Receipt failed! " + E.Message);
                return;
            }
        }

        private void printAHAVAPOSValidationReceipt(Object sender,
                PrintPageEventArgs e)
        {
            string status;
            try
            {
                int PageHeight = e.MarginBounds.Height;
                int y = 0;

                //Double Total = 0;
                int RowHeight = (int)printFont.GetHeight();

                StringFormat RightAlign =
                    new StringFormat(StringFormat.GenericTypographic);
                RightAlign.Alignment = System.Drawing.StringAlignment.Far;

                /*
                System.Drawing.StringFormat CenterAlign =
                    new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                CenterAlign.Alignment = System.Drawing.StringAlignment.Center;
                */
                //getting total amount
                decimal TotalAmt = pos.CalculateTotalAmount(out status);

                OrderDetCollection det = pos.FetchUnsavedOrderDet();
                ReceiptDetCollection RcptDet = pos.FetchUnsavedReceipt(out status);
                //TRG= TRAINING , REG= REGISTER

                string mode = "";
                if (AppSetting.GetSetting("mode") != null)
                    mode = AppSetting.GetSetting("mode").ToUpper();

                //if (mode.ToUpper() == "REGISTER")
                //    mode = "REG";
                //else if (mode.ToUpper() == "TRAINING")
                //    mode = "TRG";

                //pos.myOrderHdr.GrossAmount.ToString();
                string total = TotalAmt.ToString("N2");
                //if (total.Length > 6)
                //    total = total.Substring(0,6);


                string cashierId = pos.GetCashierID();

                string productCode = "";
                string productDesc = "";
                string paymentType = "";

                string receiptNum = pos.GetUnsavedCustomRefNo();

                string trimcashierId;
                if (cashierId.Length > 3)
                    trimcashierId = cashierId.Substring(0, 3);
                else
                    trimcashierId = cashierId;

                string trimproductCode;

                string trimpaymentType;
                paymentType = RcptDet[0].PaymentType;
                if (paymentType.Length > 4)
                    trimpaymentType = paymentType.Substring(0, 4);
                else
                    trimpaymentType = paymentType;


                //string trimreceiptNum = receiptNum.Substring(receiptNum.Length - 8);

                string trimreceiptNum = ""; 
                if (receiptNum.Length == 8)
                    trimreceiptNum = receiptNum;
                else if (receiptNum.Length > 8)
                    trimreceiptNum = receiptNum.Substring(receiptNum.Length - 8);
                else if (receiptNum.Length < 8)
                    trimreceiptNum = receiptNum.PadLeft(8,'0'); //Adi
                int yValidation = 0;
                int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.SICC.ValidationY), out yValidation);
                y += yValidation;
                //for (int i = 0; i < det.Count; i++)
                //{
                //if (det[i].IsVoided) continue;

                //productCode = det[i].Item.ItemNo;
                //productDesc = det[i].Item.ItemName;

                //if (productCode.Length > 10)
                //    trimproductCode = productCode.Substring(0, 10);
                //else
                //    trimproductCode = productCode;

                //string trimproductDesc;
                /*if (productDesc.Length > 15)
                    trimproductDesc = productDesc.Substring(0, 15);
                else
                    trimproductDesc = productDesc;*/

                /*#region *) Print Validation -- if it isCommission item
                //Correct Format : <MODE><DDMMYY><HHMM><Cashier Code><Product Code><Product Description><Receipt NO><Payment Term><Total>
                if (Convert.ToBoolean(det[i].Item.IsCommission) == true)
                {
                    int xValidation = 0;
                    int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Print.validation_x), out xValidation);
                        
                    //y += RowHeight + 1028;
                    y += RowHeight + 10;
                    e.Graphics.DrawString
                        ("     " + mode +  DateTime.Now.ToString("ddMMyyHHmm")
                        + trimcashierId + trimproductCode + trimreceiptNum+ "    " + trimpaymentType+total,
                        printFont, Brushes.Black, xValidation + Padding, y);
                }
                #endregion
            }*/
                int xValidation = 0;
                int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.SICC.ValidationX), out xValidation);
                y += RowHeight + 10;
                e.Graphics.DrawString
                    ("     " + mode + DateTime.Now.ToString("ddMMyyHHmm")
                    + trimcashierId + trimreceiptNum + trimpaymentType + "    " + total,
                    printFont, Brushes.Black, xValidation + Padding, y);
            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Print Validation failed! " + E.Message, E);
            }
        }

        #endregion 
    }
}
