using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;
using System.IO;
using System.Configuration;

namespace PowerPOS
{
    public class ClosingController
    {
        public static DataTable FetchForeignCurrencyForSettlement()
        {
            //get all the payment ref no from receiptdet that is not voided
            //split the array
            //loop through and sum by payment type
            return null;

        }
        public static DateTime FetchOpeningShift(int PointOfSaleID)
        {
            DateTime? firstTransaction = null;
            //cashierName = "-";
            //amount = 0;
            DateTime LastClosing = FetchLastClosingTime(PointOfSaleID);

            CashRecordingCollection cashRecordingcol = new CashRecordingCollection();
            cashRecordingcol.Where(CashRecording.Columns.CashRecordingTypeId, 3);
            cashRecordingcol.Where(CashRecording.Columns.CashRecordingTime,Comparison.GreaterThan, LastClosing);
            cashRecordingcol.Where(CashRecording.Columns.PointOfSaleID, PointOfSaleID);
            cashRecordingcol.OrderByAsc(CashRecording.Columns.CashRecordingTime);
            cashRecordingcol.Load();

            //firstTransaction = LastClosing; //new DateTime(1979, 11, 3);
            try
            {
                Where whr = new Where();
                Query qr = new Query("OrderHdr");
                qr.Top = "1";
                qr.OrderBy = OrderBy.Asc("OrderDate");
                qr.QueryType = QueryType.Select;
                qr.AddWhere("OrderDate", Comparison.GreaterThan, LastClosing);
                qr.AddWhere("PointOfSaleID", PointOfSaleID);

                DataSet ds = qr.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    firstTransaction = DateTime.Parse(ds.Tables[0].Rows[0]["OrderDate"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            if (cashRecordingcol.Count > 0)
            {
                //if (cashRecordingcol[0].CashRecordingTime > firstTransaction)
                //    return firstTransaction;
                //else
                //    return cashRecordingcol[0].CashRecordingTime;


                if (firstTransaction != null)
                {
                    if (cashRecordingcol[0].CashRecordingTime > firstTransaction)
                        return (DateTime)firstTransaction;
                }
                return cashRecordingcol[0].CashRecordingTime;

            }
            else
            {
                ////Start at the beginning of the day if the closing was yesterday
                //if (LastClosing.Date < firstTransaction.Date)
                //    return firstTransaction.Date;
                ////If closing is in the same day, return closing day (24 Hour style)
                //return LastClosing;

                if (firstTransaction != null)
                {
                    if (LastClosing.Date < ((DateTime)firstTransaction).Date)
                        return ((DateTime)firstTransaction).Date;
                }
                return LastClosing;
            }
        }

        public static DateTime FetchLastClosingTime(int PointOfSaleID)
        {
            
            DateTime closingtime;
            //cashierName = "-";
            //amount = 0;
            closingtime = new DateTime(1979, 11, 3);
            try
            {
                Query qr = new Query("CounterCloseLog");
                qr.Top = "1";
                qr.OrderBy = OrderBy.Desc("EndTime");
                qr.QueryType = QueryType.Select;
                qr.AddWhere("EndTime", Comparison.LessOrEquals, DateTime.Now);
                qr.AddWhere("PointOfSaleID", PointOfSaleID);                

                DataSet ds = qr.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    closingtime = DateTime.Parse(ds.Tables[0].Rows[0]["EndTime"].ToString());
                }
                return closingtime;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return (new DateTime(1979, 11, 3));
            }
        }

        public static decimal FetchVoucherCollected(int PointOfSaleID, DateTime startTime, DateTime endTime)
        {
            Query qr = ViewTransactionDetail.CreateQuery();
            
            object total = 
            qr.AND(ViewTransactionDetail.Columns.PointOfSaleID, PointOfSaleID)
            .AND(ViewTransactionDetail.Columns.OrderDetDate, Comparison.GreaterOrEquals, startTime)
            .AND(ViewTransactionDetail.Columns.OrderDetDate, Comparison.LessOrEquals, endTime)
            .AND(ViewTransactionDetail.Columns.IsLineVoided, false)
            .AND(ViewTransactionDetail.Columns.IsVoided, false)
            .AND(ViewTransactionDetail.Columns.Amount, Comparison.LessOrEquals, 0)
            .AND(ViewTransactionDetail.Columns.ItemNo, POSController.VOUCHER_BARCODE).
            GetSum(ViewTransactionDetail.Columns.Amount);

            if (total is decimal)
                return Math.Abs((decimal)total);
            else 
                return 0;
        }

        public static void DeleteSavedClosing()
        {
            Query qr = SavedClosing.CreateQuery();
            qr.QueryType = QueryType.Delete;            
            qr.Execute();

        }

        public static bool SavePartiallyEnteredClosing
            (string FloatBalance, string NetsCollected, string NetsTerminalID, string VisaCollected, 
                string VisaBatchNo, string AmexCollected, string AmexBatchNo, string ChinaNetsCollected, 
            string ChinaNetsTerminalID, string VoucherCollected, string ChequeCollected, string DepositBagNo, 
            string PointOfSaleID, string C100, string C50, string C10, string C5, string C2, string C1, string C050, string C020, string C010, string C005)
        {
            try
            {
                //delete existing ones
                QueryCommandCollection cmd = new QueryCommandCollection();

                Query qr = SavedClosing.CreateQuery();
                cmd.Add(qr.BuildDeleteCommand());

                //insert a new ones
                SavedClosing newClosing = new SavedClosing();
                newClosing.FloatBalance=FloatBalance;
                newClosing.NetsCollected=NetsCollected;
                newClosing.NetsTerminalID=NetsTerminalID;
                newClosing.VisaCollected=VisaCollected;
                newClosing.VisaBatchNo=VisaBatchNo;
                newClosing.AmexCollected=AmexCollected;
                newClosing.AmexBatchNo=AmexBatchNo;
                newClosing.ChinaNetsCollected=ChinaNetsCollected;
                newClosing.ChinaNetsTerminalID=ChinaNetsTerminalID;
                newClosing.VoucherCollected=VoucherCollected;
                newClosing.ChequeCollected=ChequeCollected;
                newClosing.DepositBagNo=DepositBagNo;
                newClosing.PointOfSaleID=PointOfSaleID;
                newClosing.C100=C100;
                newClosing.C50=C50;
                newClosing.C10=C10;
                newClosing.C5=C5;
                newClosing.C2=C2;
                newClosing.C1=C1;
                newClosing.C050=C050;
                newClosing.C020=C020;
                newClosing.C010=C010;
                newClosing.C005 = C005;

                cmd.Add(newClosing.GetInsertCommand("SYSTEM"));

                DataService.ExecuteTransaction(cmd);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        //Load Saved Data
        public static bool FetchSavedClosing
                (out string FloatBalance, out  string NetsCollected, out  
                 string NetsTerminalID, out  string VisaCollected, out  string VisaBatchNo, out  
                 string AmexCollected, out  string AmexBatchNo, out  string ChinaNetsCollected, out  
                 string ChinaNetsTerminalID, out  string VoucherCollected, out  string ChequeCollected, out  
                 string DepositBagNo, out  string PointOfSaleID, out  string C100, out  string C50, out  string C10, out  string C5, out  string C2, out  
                 string C1, out  string C050, out  string C020, out  string C010, out  string C005)
        {
            SavedClosingCollection s = new SavedClosingCollection();
            s.Load();

            if (s.Count > 0)
            {
                SavedClosing newClosing = s[0];

                FloatBalance = newClosing.FloatBalance;
                NetsCollected = newClosing.NetsCollected;
                NetsTerminalID = newClosing.NetsTerminalID;
                VisaCollected = newClosing.VisaCollected;
                VisaBatchNo = newClosing.VisaBatchNo;
                AmexCollected = newClosing.AmexCollected;
                AmexBatchNo = newClosing.AmexBatchNo;
                ChinaNetsCollected = newClosing.ChinaNetsCollected;
                ChinaNetsTerminalID = newClosing.ChinaNetsTerminalID;
                VoucherCollected = newClosing.VoucherCollected;
                ChequeCollected = newClosing.ChequeCollected;
                DepositBagNo = newClosing.DepositBagNo;
                PointOfSaleID = newClosing.PointOfSaleID;
                C100 = newClosing.C100;
                C50 = newClosing.C50;
                C10 = newClosing.C10;
                C5 = newClosing.C5;
                C2 = newClosing.C2;
                C1 = newClosing.C1;
                C050 = newClosing.C050;
                C020 = newClosing.C020;
                C010 = newClosing.C010;
                C005 = newClosing.C005;

                return true;
            }

            FloatBalance = "";
            NetsCollected = "";
            NetsTerminalID = "";
            VisaCollected = "";
            VisaBatchNo = "";
            AmexCollected = "";
            AmexBatchNo = "";
            ChinaNetsCollected = "";
            ChinaNetsTerminalID = "";
            VoucherCollected = "";
            ChequeCollected = "";
            DepositBagNo = "";
            PointOfSaleID = "";
            C100 = "";
            C50 = "";
            C10 = "";
            C5 = "";
            C2 = "";
            C1 = "";
            C050 = "";
            C020 = "";
            C010 = "";
            C005 = "";

            return false;
        }

        public static void WriteLastSaved(int PointOfSaleID, DateTime savedDate)
        {
            string filepath = "CLOSING" + PointOfSaleID.ToString() + ".txt";

            StreamWriter f = File.CreateText(filepath);
            f.WriteLine(savedDate.ToString("dd MMM yyyy HH:mm:ss"));
            f.Close();
            return;
        }

        public static DateTime ReadLastSaved(int PointOfSaleID)
        {
            string filepath = "CLOSING" + PointOfSaleID.ToString() + ".txt";
            if (File.Exists(filepath))
            {
                StreamReader f = File.OpenText(filepath);
                //read first line;
                string result = f.ReadLine();
                f.Close();
                DateTime returnDate;
                if (result != null &&
                    DateTime.TryParse(result, out returnDate))
                {
                    return returnDate;
                }
                else
                {
                    return (new DateTime(2007, 1, 1));
                }
            }
            return (new DateTime(2007, 1, 1));
        }

        public static bool DeleteSaved(int PointOfSaleID)
        {
            string filepath = "CLOSING" + PointOfSaleID.ToString() + ".txt";
            File.Delete(filepath);
            return true;
        }

        public string FormatEmailOutput(CounterCloseLog myCounter, bool isInclusiveGST, bool includeProductSalesReport)
        {
            string output = "<html><body><p>";
            output += "Counter - " + myCounter.PointOfSale.PointOfSaleName;
            output += "<BR/>";
            output += "From: " + myCounter.StartTime.ToString("dd-MMM-yyyy HH:mm:ss");
            output += "<BR/>";
            output += "To: " + myCounter.EndTime.ToString("dd-MMM-yyyy HH:mm:ss");
            output += "<BR/>";
            //output += "**************************************";
            output += "Cashier: " + myCounter.Cashier;
            output += "<BR/>";

            #region Sales Without Item Dept
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.ShowSalesWithoutItemDeptFilter), false))
            {
                string itemDeptfilterName = AppSetting.GetSetting(AppSetting.SettingsName.Print.ItemDeptFilterName);
                DataTable dt = ReportController.FetchDepartmentSalesReport(
                    myCounter.StartTime, myCounter.EndTime,
                    myCounter.PointOfSale.PointOfSaleName,
                    "", itemDeptfilterName,
                    false, "DepartmentName", "ASC");
                decimal totalCategoryFilterSalesAmount = 0;
                if (dt.Rows.Count > 0)
                {
                    decimal.TryParse(dt.Rows[0]["TotalAmount"].ToString(), out totalCategoryFilterSalesAmount);
                }
                output += "Sales Excl. " + itemDeptfilterName + ":";
                output += String.Format("{0:N}", myCounter.TotalSystemRecorded - totalCategoryFilterSalesAmount);
                output += "<BR/>";

                output += itemDeptfilterName + ":";
                output += String.Format("{0:N}", totalCategoryFilterSalesAmount);
                output += "<BR/>";


            }
            #endregion

            decimal totalSales = ReceiptController.GetTotalSystemCollected(myCounter.StartTime, myCounter.EndTime, myCounter.PointOfSaleID);
            output += "Total Sales: " + String.Format("{0:N}", totalSales);
            output += "<BR/>";

            //output += "<BR/>";            
            output += "Total Actual Collected: " + String.Format("{0:N}", myCounter.TotalActualCollected);
            output += "<BR/>";

            string counttransaction = ClosingController.GetTotalNumberOfOrder(myCounter.StartTime, myCounter.EndTime,
                       myCounter.PointOfSaleID.ToString());

            output += "No of Bill: ";
            output += counttransaction;
            output += "<BR/>";

            int countVoid = 0;
            decimal amountVoid = 0;
            ClosingController.GetTotalNumberOfVoidedOrder(myCounter, out countVoid, out amountVoid);

            output += "No of Void: ";
            output += countVoid.ToString("N0");
            output += "<BR/>";

            //output += "<BR/>";
            if (PowerPOS.Feature.Package.isAvailable)
            {
                output += "Points Redemption: " + String.Format("{0:N}", myCounter.PointRecorded);
                output += "<BR/>";
                output += "Package Redemption: " + String.Format("{0:N}", myCounter.PackageRecorded);
                output += "<BR/>";
            }
            decimal installmentAmount =
                ReceiptController.FetchTotalInstallment(myCounter.StartTime, myCounter.EndTime, myCounter.PointOfSaleID);
            if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText)))
                output += "Installment: " + String.Format("{0:N}", installmentAmount);
            else
                output += AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText) + ": " + String.Format("{0:N}", installmentAmount);

            output += "<BR/>";
            output += "<BR/>";
            output += "Actual Collection: ";
            output += "<BR/>";
            output += "**************************************";
            output += "<BR/>";
            output += "&nbsp;&nbsp;CASH: " + String.Format("{0:N}", myCounter.CashCollected);
            output += "<BR/>";
            string tmp = PaymentTypesController.FetchPaymentByID("1");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                output += "&nbsp;&nbsp;" + tmp + ": " + String.Format("{0:N}", myCounter.NetsCollected);
                output += "<BR/>";
            }
            tmp = PaymentTypesController.FetchPaymentByID("2");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                output += "&nbsp;&nbsp;" + tmp + ": " + String.Format("{0:N}", myCounter.VisaCollected);
                output += "<BR/>";
            }
            tmp = PaymentTypesController.FetchPaymentByID("3");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                output += "&nbsp;&nbsp;" + tmp + ": " + String.Format("{0:N}", myCounter.ChinaNetsCollected);
                output += "<BR/>";
            }
            tmp = PaymentTypesController.FetchPaymentByID("4");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                output += "&nbsp;&nbsp;" + tmp + ": " + String.Format("{0:N}", myCounter.AmexCollected);
                output += "<BR/>";
            }
            tmp = PaymentTypesController.FetchPaymentByID("5");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                output += "&nbsp;&nbsp;" + tmp + ": " + String.Format("{0:N}", myCounter.Pay5Collected);
                output += "<BR/>";
            }
            tmp = PaymentTypesController.FetchPaymentByID("6");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                output += "&nbsp;&nbsp;" + tmp + ": " + String.Format("{0:N}", myCounter.Pay6Collected);
                output += "<BR/>";
            }
            output += "&nbsp;&nbsp;CHEQUE: " + String.Format("{0:N}", myCounter.Userfloat2);
            output += "<BR/>";
            output += "<BR/>";
            output += "System Recorded Collection: ";
            output += "<BR/>";
            output += "**************************************";
            output += "<BR/>";
            output += "&nbsp;&nbsp;CASH: " + String.Format("{0:N}", myCounter.CashRecorded);
            output += "<BR/>";
            tmp = PaymentTypesController.FetchPaymentByID("1");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                output += "&nbsp;&nbsp;" + tmp + ": " + String.Format("{0:N}", myCounter.NetsRecorded);
                output += "<BR/>";
            }
            tmp = PaymentTypesController.FetchPaymentByID("2");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                output += "&nbsp;&nbsp;" + tmp + ": " + String.Format("{0:N}", myCounter.VisaRecorded);
                output += "<BR/>";
            }
            tmp = PaymentTypesController.FetchPaymentByID("3");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                output += "&nbsp;&nbsp;" + tmp + ": " + String.Format("{0:N}", myCounter.ChinaNetsRecorded);
                output += "<BR/>";
            }
            tmp = PaymentTypesController.FetchPaymentByID("4");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                output += "&nbsp;&nbsp;" + tmp + ": " + String.Format("{0:N}", myCounter.AmexRecorded);
                output += "<BR/>";
            }
            tmp = PaymentTypesController.FetchPaymentByID("5");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                output += "&nbsp;&nbsp;" + tmp + ": " + String.Format("{0:N}", myCounter.Pay5Recorded);
                output += "<BR/>";
            }
            tmp = PaymentTypesController.FetchPaymentByID("6");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                output += "&nbsp;&nbsp;" + tmp + ": " + String.Format("{0:N}", myCounter.Pay6Recorded);
                output += "<BR/>";
            }
            output += "&nbsp;&nbsp;CHEQUE: " + String.Format("{0:N}", myCounter.Userfloat1);
            output += "<BR/>";
            output += "<BR/>";
            output += "**************************************";
            output += "<BR/>";
            output += "Opening Balance: " + String.Format("{0:N}", myCounter.OpeningBalance);
            output += "<BR/>";
            output += "Cash In: " + String.Format("{0:N}", myCounter.CashIn);
            output += "<BR/>";
            output += "Cash Out: " + String.Format("{0:N}", myCounter.CashOut);
            output += "<BR/>";
            output += "Closing Cash Out: " + String.Format("{0:N}", myCounter.ClosingCashOut);
            output += "<BR/>";
            output += "Deposit Bag No: " + String.Format("{0:N}", myCounter.DepositBagNo);
            output += "<BR/>";
            output += "Surplus (+/-): " + String.Format("{0:N}", myCounter.Variance);
            output += "<BR/>";
            output += "<BR/>";



            if (includeProductSalesReport)
            {
                DataTable dt2 = ReportController.FetchProductCategorySalesReport(
                            myCounter.StartTime, myCounter.EndTime,
                            myCounter.PointOfSale.PointOfSaleName,
                            "", "", "",
                            false, "CategoryName", "ASC");

                output += "CATEGORY SALES REPORT";
                output += "<BR/>";
                output += "**************************************";
                output += "<BR/>";
                output += "<table><tr><th>Category</th><td>Qty.</td><td>Total</td></tr>";
                for (int i = 0; i <= (dt2.Rows.Count - 1); i++)
                {
                    string categoryname = dt2.Rows[i]["CategoryName"].ToString();

                    decimal tqty = Convert.ToDecimal(dt2.Rows[i]["TotalQuantity"].ToString());
                    string totalamount = dt2.Rows[i]["TotalAmount"].ToString();

                    output += "<tr><td>" + categoryname + "</td><td>" + tqty + "</td><td>" + totalamount + "</td></tr>";
                }

                output += "</table>";
                output += "<BR/>";
                output += "<BR/>";
                output += "PRODUCT SALES REPORT";
                output += "<BR/>";
                output += "**************************************";
                output += "<BR/>";



                DataTable dt = ReportController.FetchProductSalesReport
                                        (myCounter.StartTime, myCounter.EndTime, "",
                                        myCounter.PointOfSale.PointOfSaleName, "", "",
                                        "", false,
                                        "CategoryName", "ASC");




                output += "<table><tr><th>Item</th><td>Qty.</td><td>Total</td></tr>";

                for (int i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    string Item = dt.Rows[i]["ItemNo"].ToString() + "-" + dt.Rows[i]["ItemName"].ToString();

                    decimal tqty = Convert.ToDecimal(dt.Rows[i]["TotalQuantity"].ToString());
                    string totalamount = dt.Rows[i]["TotalAmount"].ToString();

                    output += "<tr><td>" + Item + "</td><td>" + tqty + "</td><td>" + totalamount + "</td></tr>";
                }

                output += "</table>";
                output += "<BR/>";

                output += "**************************************";
                output += "<BR/>";
                output += "Closed By: " + myCounter.Cashier;
                output += "<BR/>";

                output += "Verified By: " + myCounter.Supervisor;
                output += "<BR/>";
                output += "</p></body></html>";
            }

            return output;
        }

        //public string FormatEmailOutput2(CounterCloseLog myCounter)
        //{
        //    CounterCloseLog closeLog = myCounter;

        //    DataTable dt = ReportController.FetchProductSalesReport
        //                (closeLog.StartTime, closeLog.EndTime, "",
        //                closeLog.PointOfSale.PointOfSaleName, "", "",
        //                "", false,
        //                "CategoryName", "ASC");


        //    string dname = dt.Rows[0]["DepartmentName"].ToString();
        //    string ino = dt.Rows[0]["ItemNo"].ToString();
        //    string iname = dt.Rows[0]["ItemName"].ToString();
        //    //Int32 tqty = Convert.ToInt32(dt.Rows[0]["TotalQuantity"].ToString());
        //    //string a1 = dt.Rows[0]["Attributes1"].ToString();
        //    //string a2 = dt.Rows[0]["Attributes2"].ToString();
        //    //string a3 = dt.Rows[0]["Attributes3"].ToString();
        //    //string a4 = dt.Rows[0]["Attributes4"].ToString();
        //    //string a5 = dt.Rows[0]["Attributes5"].ToString();
        //    //string a6 = dt.Rows[0]["Attributes6"].ToString();
        //    //string a7 = dt.Rows[0]["Attributes7"].ToString();
        //    //string totalamount = dt.Rows[0]["TotalAmount"].ToString();
        //    //string gstamt = dt.Rows[0]["GSTAmount"].ToString();
        //    //string discount = dt.Rows[0]["Discount"].ToString();
        //    //string tamntwithoutgst = dt.Rows[0]["TotalAmountWithoutGST"].ToString();
        //    //string posname = dt.Rows[0]["PointOfSaleName"].ToString();
        //    string outletname = dt.Rows[0]["OutletName"].ToString();
                        

        //    DataTable dt2 = ReportController.FetchProductCategorySalesReport(
        //                closeLog.StartTime, closeLog.EndTime,
        //                closeLog.PointOfSale.PointOfSaleName,
        //                "", "", "",
        //                false, "CategoryName", "ASC");

        //    string categoryname = dt2.Rows[0]["CategoryName"].ToString();
            
        //    Int32 tqty = Convert.ToInt32(dt.Rows[0]["TotalQuantity"].ToString());
        //    string totalamount = dt2.Rows[0]["TotalAmount"].ToString();
            



        //    string output = "<html><body><p>";
        //    //output += "Counter - " + posname;
        //    //output += "<BR/>";

        //    output += "Category Name: " + categoryname;
        //    output += "<BR/>";
        //    output += "Outlet Name: " + outletname;
        //    output += "<BR/>";
        //    output += "From: " + closeLog.StartTime.ToString("dd-MMM-yyyy HH:mm:ss");
        //    output += "<BR/>";
        //    output += "To: " + closeLog.EndTime.ToString("dd-MMM-yyyy HH:mm:ss");
        //    output += "<BR/>";
        //    //output += "**************************************";
        //    output += "DepartmentName: " + dname;
        //    output += "<BR/>";
        //    //output += "<BR/>";            
        //    output += "Item Number: " + ino;
        //    output += "<BR/>";
        //    output += "Item Name: " + iname;
        //    output += "<BR/>";

        //    output += "**************************************";
        //    output += "<BR/>";
        //    output += "&nbsp;&nbsp;Total Quantity: " +tqty;
           
        //    output += "<BR/>";
            
                       
        //    //output += "&nbsp;&nbsp;CHEQUE: " + String.Format("{0:N}", myCounter.Userfloat1);
        //    output += "<BR/>";
        //    //output += "<BR/>";
           
        //    output += "<BR/>";
        //    output += "Total Amount: " + String.Format("{0:N}", totalamount);
        //    output += "<BR/>";

        //    output += "Closed By: " + closeLog.Cashier;
        //    output += "<BR/>";

        //    output += "Verified By: " + closeLog.Supervisor;
        //    output += "<BR/>";
        //    output += "</p></body></html>";

        //    return output; 
        //}

        #region z2closing
        public string GetFirstReceiptNo(CounterCloseLog p)
        {
            string SQL = "Select min(orderhdrid) from orderhdr where orderdate > '" +
                            p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and orderdate < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and pointofsaleid = " + p.PointOfSaleID +
                            " and isVoided = 0";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null && obj.ToString().Length > 10) return obj.ToString().Substring(10);
            return "";
        }

        public string GetLastReceiptNo(CounterCloseLog p)
        {
            string SQL = "Select max(orderhdrid) from orderhdr where orderdate > '" +
                            p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and orderdate < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and pointofsaleid = " + p.PointOfSaleID +
                            " and isVoided = 0";
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null && obj.ToString().Length > 10) return obj.ToString().Substring(10);
            return "";
        }

        public string GetTotalNumberOfOrder(CounterCloseLog p)
        {
            string SQL = "Select count(orderhdrid) from orderhdr where orderdate > '" +
                            p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and orderdate < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and pointofsaleid = " + p.PointOfSaleID +
                            " and isVoided = 0";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null) return obj.ToString();
            return "0";
        }

        public static string GetTotalNumberOfOrder(DateTime StartTime, DateTime EndTime, string PointOfSaleID)
        {
            string SQL = "Select count(orderhdrid) from orderhdr where orderdate > '" +
                            StartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and orderdate < '" + EndTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and pointofsaleid = " + PointOfSaleID +
                            " and isVoided = 0";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null) return obj.ToString();
            return "0";
        }

        public string GetTotalNumberOfCancelledItem(CounterCloseLog p)
        {
            string SQL = "select sum(cast(substring(refno,CharIndex(',',refno,1)+1,100) as int)) " +
                            "from cancelationlog " +
                            "where [type] = 'CancelItem' " +
                            "and Charindex(',',refno,1) > 0 " +
                            "and createdon >= '" +
                            p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and createdon < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null) return obj.ToString();
            return "";
        }

        public string GetTotalNumberOfAmendedOrder(CounterCloseLog p)
        {
            string SQL = "select count(CancelLog) from cancelationlog where [type] = 'AmendBill' and createdon >= '" +
                            p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and createdon < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null) return obj.ToString();
            return "";
        }

        public static void GetTotalNumberOfVoidedOrder(CounterCloseLog p, out int count, out decimal amount)
        {
            try
            {

                string SQL = "Select isnull(count(orderhdrid),0), isnull(sum(nettamount),0) from orderhdr where orderdate > '" +
                                p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                                "' and orderdate < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") +
                                "' and pointofsaleid = " + p.PointOfSaleID +
                                " and isVoided = 1";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                DataTable dt = DataService.GetDataSet(cmd).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    count = (int)dt.Rows[0][0];
                    amount = (decimal)dt.Rows[0][1];
                }
                else
                {
                    count = 0;
                    amount = 0.0M;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                count = 0;
                amount = 0.0M;
            }
        }

        public string GetTotalNumberOfCustomer(CounterCloseLog p)
        {
            string SQL = "Select sum(pax) from orderhdr where orderdate > '" +
                            p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and orderdate < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and pointofsaleid = " + p.PointOfSaleID +
                            " and isVoided = 0";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null) return obj.ToString();
            return "0";
        }

        public string GetTotalSalesBeforeRefund(CounterCloseLog p)
        {
            string SQL = "Select sum(ISNULL(NettAmount,0)) from orderhdr where orderdate > '" +
                            p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and orderdate < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and pointofsaleid = " + p.PointOfSaleID +
                            " and isVoided = 0 and nettamount > 0 ";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null) return obj.ToString();
            return "0";
        }

        public string GetTotalRefund(CounterCloseLog p)
        {
            string SQL = "Select sum(ISNULL(NettAmount,0)) from orderhdr where orderdate > '" +
                            p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and orderdate < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and pointofsaleid = " + p.PointOfSaleID +
                            " and isVoided = 0 and nettamount < 0";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null) return obj.ToString();
            return "0";
        }

        public static string GetTotalNumberOfCustomer(DateTime StartTime, DateTime EndTime, string PointOfSaleID)
        {
            string SQL = "Select sum(pax) from orderhdr where orderdate > '" +
                            StartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and orderdate < '" + EndTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and pointofsaleid = " + PointOfSaleID +
                            " and isVoided = 0";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null) return obj.ToString();
            return "0";
        }

        public decimal GetTotalOverallDiscount(DateTime startDate, DateTime endDate, int PointOfSaleID)
        {
            try
            {
                decimal subTotalDiscount;
                string SQL = "select sum(isnull(discdollar,0)) from ordersubtotal where subtotaltype like '%discount%' and orderhdrid in (select orderhdrid from orderhdr where isvoided=0 and orderdate > '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'  AND PointOfSaleID=" + PointOfSaleID.ToString() + ")";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                object obj = DataService.ExecuteScalar(cmd);
                if (obj != null && obj is decimal)
                {
                    subTotalDiscount = (decimal)obj;
                }
                else
                {
                    subTotalDiscount = 0.0M;
                }
                return subTotalDiscount;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return 0.0M;
            }
        }

        public decimal GetTotalLineDiscount(DateTime startDate, DateTime endDate, int PointOfSaleID)
        {
            decimal subTotalDiscount = 0.0M;
            try
            {
                //get Item discount
                string SQL = "select Sum(case when (unitprice * case when useweight = 1 then weight else quantity end) > amount then (unitprice*case when useweight = 1 then weight else quantity end)-Amount else 0 end ) " +
                        "from orderdet a inner join orderhdr b on a.orderhdrid = b.orderhdrid " +
                        "where a.isvoided=0 and b.isvoided=0" +
                        "and b.orderdate > '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and b.orderdate < '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND pointofsaleid=" + PointOfSaleID.ToString();
                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                Object obj = DataService.ExecuteScalar(cmd);
                if (obj != null && obj is decimal)
                {
                    subTotalDiscount += (decimal)obj;
                }
                else
                {
                    subTotalDiscount += 0.0M;
                }
                return subTotalDiscount;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return 0.0M;
            }
        }

        public decimal GetTotalGST(CounterCloseLog p, bool inclusiveGST)
        {
            try
            {
                if (inclusiveGST)
                {
                    string SQL = "select (sum(nettamount) * 0.07)/ 1.07 from " +
                                 "orderhdr a " +
                                 "where a.isvoided=0 and " +
                                 "orderdate > '" + p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND PointOfSaleID=" + p.PointOfSaleID + "";

                    QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                    object obj = DataService.ExecuteScalar(cmd);
                    if (obj != null && obj is decimal)
                    {
                        return (decimal)obj;
                    }
                    else
                    {
                        return 0.0M;
                    }
                }
                else
                {
                    string SQL = "select sum(discdollar) from ordersubtotal where name = 'GST' and orderhdrid in (select orderhdrid from orderhdr where IsVoided = 0 AND orderdate > '" + p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND PointOfSaleID=" + p.PointOfSaleID + ")";
                    QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                    object obj = DataService.ExecuteScalar(cmd);
                    if (obj != null && obj is decimal)
                    {
                        return (decimal)obj;
                    }
                    else
                    {
                        return 0.0M;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return 0.0M;
            }
        }

        public decimal GetTotalRounding(DateTime startDate, DateTime endDate, int PointOfSaleID)
        {
            try
            {
                string SQL = "select sum(discdollar) from ordersubtotal where name = 'Rounding' and orderhdrid in (select orderhdrid from orderhdr where IsVoided = 0 AND orderdate > '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND PointOfSaleID=" + PointOfSaleID.ToString() + ")";
                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                object obj = DataService.ExecuteScalar(cmd);
                if (obj != null && obj is decimal)
                {
                    return (decimal)obj;
                }
                else
                {
                    return 0.0M;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return 0.0M;
            }
        }

        public decimal GetTotalRounding(CounterCloseLog p)
        {
            try
            {
                string SQL = "select sum(discdollar) from ordersubtotal where name = 'Rounding' and orderhdrid in (select orderhdrid from orderhdr where IsVoided = 0 AND orderdate > '" + p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND PointOfSaleID=" + p.PointOfSaleID.ToString() + ")";
                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                object obj = DataService.ExecuteScalar(cmd);
                if (obj != null && obj is decimal)
                {
                    return (decimal)obj;
                }
                else
                {
                    return 0.0M;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return 0.0M;
            }
        }

        public DataTable GetTotalOfSubTotal
            (DateTime startDate, DateTime endDate, int PointOfSaleID)
        {
            try
            {
                string SQL = "select isnull(sum(discdollar),0) as amount,name, SubTotalType from ordersubtotal where " +
                             "orderhdrid in (select orderhdrid from orderhdr where isvoided = 0 and orderdate > '" +
                             startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" +
                             endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'  AND pointofsaleid=" +
                             PointOfSaleID.ToString() + ") AND Not [Name] = 'Rounding' AND Not [Name] = 'Svc Charge' AND Not [Name] = 'GST' " +
                             "Group By name, SubTotalType order by name asc";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                return DataService.GetDataSet(cmd).Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public decimal GetTotalServiceCharge(DateTime startDate, DateTime endDate, int PointOfSaleID)
        {
            try
            {
                string SQL = "select sum(discdollar) from ordersubtotal where name = 'Svc Charge' and orderhdrid in (select orderhdrid from orderhdr where IsVoided = 0 AND orderdate > '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'  AND pointofsaleid=" + PointOfSaleID.ToString() + ")";
                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                object obj = DataService.ExecuteScalar(cmd);
                if (obj != null && obj is decimal)
                {
                    return (decimal)obj;
                }
                else
                {
                    return 0.0M;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return 0.0M;
            }
        }

        public decimal GetTotalServiceCharge(CounterCloseLog p)
        {
            try
            {
                string SQL = "select sum(discdollar) from ordersubtotal where name = 'Svc Charge' and orderhdrid in (select orderhdrid from orderhdr where IsVoided = 0 AND orderdate > '" + p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" + p.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'  AND pointofsaleid=" + p.PointOfSaleID.ToString() + ")";
                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                object obj = DataService.ExecuteScalar(cmd);
                if (obj != null && obj is decimal)
                {
                    return (decimal)obj;
                }
                else
                {
                    return 0.0M;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return 0.0M;
            }
        }

        public decimal GetTotalGST(DateTime startDate, DateTime endDate, int PointOfSaleID)
        {
            try
            {
                string SQL = String.Empty;
                if (ConfigurationManager.AppSettings.Get("PriceInclusiveGST").ToUpper() == "YES")
                {
                    SQL = "select (sum(nettamount) " + // - " +
                        //"ISNULL((select sum(discdollar) from ordersubtotal where name = 'Rounding' and orderhdrid in (select orderhdrid from orderhdr where IsVoided = 0 AND orderdate > '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND PointOfSaleID=" + PointOfSaleID + ")),0)  - " +
                        //"ISNULL((select sum(discdollar) from ordersubtotal where name = 'Svc Charge' and orderhdrid in (select orderhdrid from orderhdr where IsVoided = 0 AND orderdate > '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'  AND pointofsaleid=" + PointOfSaleID + ")),0) - " +
                        //"ISNULL((select sum(discdollar) from ordersubtotal where subtotaltype like '%discount%' and orderhdrid in (select orderhdrid from orderhdr where isvoided=0 and orderdate > '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'  AND PointOfSaleID=" + PointOfSaleID + ")),0)  " +
                        //")
                   ")* GST /(100+GST)  as TotalGST " +
                   "from orderhdr where isvoided=0 and orderdate > '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and orderdate < '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and pointofsaleid=1 " +
                   "group by gst";
                }
                else
                {
                    SQL = "select ISNULL(sum(discdollar),0) from ordersubtotal where name = 'GST' and orderhdrid in (select orderhdrid from orderhdr where IsVoided = 0 AND orderdate > '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND orderdate < '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND PointOfSaleID=" + PointOfSaleID + ")";
                }

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                object obj = DataService.ExecuteScalar(cmd);
                obj = String.Format("{0:0.00}", obj);
                if (obj != null)
                {
                    return Convert.ToDecimal(obj);
                }
                else
                {
                    return 0.0M;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return 0.0M;
            }
        }

        public decimal GetGrossSales(DateTime startDate, DateTime endDate, int PointOfSaleID)
        {
            decimal GrossSales;
            //get Item discount
            string SQL = "select sum(a.grosssales * quantity) " +
                    "from orderdet a inner join orderhdr b on a.orderhdrid = b.orderhdrid " +
                    "where a.isvoided=0 and b.isvoided=0" +
                    "and b.orderdate > '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and b.orderdate < '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and pointofsaleid=" + PointOfSaleID;
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null && obj is decimal)
            {
                GrossSales = (decimal)obj;
            }
            else
            {
                GrossSales = 0.0M;
            }
            return GrossSales;
        }

        public decimal GetNettSales(DateTime startDate, DateTime endDate, int PointOfSaleID)
        {
            decimal GrossSales;
            //get Item discount
            string SQL = "select sum(nettamount) " +
                    "from orderhdr " +
                    "where isvoided=0 " +
                    "and orderdate > '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and orderdate < '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and pointofsaleid=" + PointOfSaleID;
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null && obj is decimal)
            {
                GrossSales = (decimal)obj;
            }
            else
            {
                GrossSales = 0.0M;
            }
            return GrossSales;

        }

        public static DataTable getCounterCloseLogList(DateTime startDate, DateTime endDate, int PointOfSaleID)
        {
            DataTable dt = new DataTable();
            string sqlString = "Select * from CounterCloseLog where startTime >= '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                               " and endTime <= '" + endDate.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss") + "' and Pointofsaleid = " + PointOfSaleID;
            dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
            return dt;
        }

        public static decimal GetTotalGSTRecorded(DateTime StartTime, DateTime EndTime, string PointOfSaleID)
        {
            decimal gstrecorded = 0;

            string SQL = "Select sum(GSTAmount) from orderhdr where orderdate > '" +
                            StartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and orderdate < '" + EndTime.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and pointofsaleid = " + PointOfSaleID +
                            " and isVoided = 0";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null && obj is decimal)
            {
                gstrecorded = (decimal)obj;
            }
            else
            {
                gstrecorded = 0.0M;
            }
            return gstrecorded;
        }

        #endregion
    }
}
