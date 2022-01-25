using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;

namespace PowerPOS
{
    [System.ComponentModel.DataObject]
    public partial class UtilityController
    {
        public static string GetCurrentVersion()
        {
            string versionNo = "";
            try
            {
                System.Version version = Assembly.GetExecutingAssembly().GetName().Version;
                versionNo = version.Major.ToString() + "." + version.Minor.ToString() + "." + version.Build.ToString() + "." + version.Revision.ToString();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return versionNo;
        }

        public static string GetCurrentVersionMajorMinor()
        {
            string versionNo = "";
            try
            {
                System.Version version = Assembly.GetExecutingAssembly().GetName().Version;
                versionNo = version.Major.ToString() + "." + version.Minor.ToString();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return versionNo;
        }

        public static string GetPOSType()
        {
            string posType = "";
            try
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
                posType = versionInfo.ProductName;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return posType;
        }

        public static DateTime GetCurrentTime()
        {
            return DateTime.UtcNow.AddHours(8); //local singapore time...

            
        }
        public static DataTable FetchCounterClosingReport
            (bool useFromEndDate, bool useToEndDate, DateTime FromEndDate, DateTime ToEndDate,
            string ViewCloseCounterReportRefNo, string Cashier, string supervisorId,
            int PointOfSaleID, string PointOfSaleName, string outletName, string deptID, 
            string SortColumn, string SortDir)
        {
            ViewCloseCounterReportCollection myViewCloseCounterReport = new ViewCloseCounterReportCollection();
            if (outletName == "ALL")
            {
                outletName = "%";
            }

            if (outletName == "ALL - Breakdown")
            {
                outletName = "%";
                PointOfSaleID = 0;
                PointOfSaleName = "%";
            }
            if (useFromEndDate & useToEndDate)
            {
                myViewCloseCounterReport.BetweenAnd(ViewCloseCounterReport.Columns.EndTime, FromEndDate, ToEndDate);
            }
            else if (useFromEndDate)
            {
                myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.EndTime, SubSonic.Comparison.GreaterOrEquals, FromEndDate);
            }
            else if (useToEndDate)
            {
                myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.EndTime, SubSonic.Comparison.LessOrEquals, ToEndDate);
            }
            if (ViewCloseCounterReportRefNo != "")
            {
                myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.CounterCloseID, SubSonic.Comparison.Like, ViewCloseCounterReportRefNo);
            }

            if (Cashier != "")
            {
                myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.Cashier, SubSonic.Comparison.Like, Cashier);
            }

            if (supervisorId != "")
            {
                myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.Supervisor, SubSonic.Comparison.Like, supervisorId);
            }
            if (outletName != "")
            {
                myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.OutletName, SubSonic.Comparison.Like, outletName);
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.PointOfSaleID, PointOfSaleID);
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    myViewCloseCounterReport.Where(ViewTransaction.Columns.PointOfSaleName, SubSonic.Comparison.Like, PointOfSaleName);
                }
            }
            if (deptID != "0")
            {
                myViewCloseCounterReport.Where(ViewCloseCounterReport.Columns.DepartmentID, SubSonic.Comparison.Like, deptID);
            }

            SubSonic.TableSchema.TableColumn t = ViewCloseCounterReport.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewCloseCounterReport.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewCloseCounterReport.OrderByDesc(SortColumn);
                }
            }

            DataTable dt = myViewCloseCounterReport.Load().ToDataTable();
            DataTable tmp;
            dt.Columns.Add("NettSales");
            dt.Columns.Add("Defi");
            dt.Columns.Add("ProductSales");
            dt.Columns.Add("Defi2");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tmp = 
                    ReportController.FetchProfitLossReport(
                    DateTime.Parse(dt.Rows[i]["StartTime"].ToString()),
                    DateTime.Parse(dt.Rows[i]["EndTime"].ToString()),
                    dt.Rows[i]["OutletName"].ToString(),                     
                     deptID, "", "");

                if (tmp.Rows.Count > 0)
                {
                    dt.Rows[i]["NettSales"] =
                        tmp.Rows[0]["NettSalesBeforeGST"].ToString();

                    dt.Rows[i]["Defi"] = 
                        (decimal.Parse(dt.Rows[i]["NettSales"].ToString()) - decimal.Parse(dt.Rows[i]["TotalSystemRecorded"].ToString())).ToString();
                }

                tmp = ReportController.FetchProductSalesReport(
                    DateTime.Parse(dt.Rows[i]["StartTime"].ToString()),
                    DateTime.Parse(dt.Rows[i]["EndTime"].ToString()),
                    "%", new PointOfSale(int.Parse(dt.Rows[i]["PointOfSaleID"].ToString())).PointOfSaleName,
                    "%", "%", deptID, false, "ItemNo", "ASC");

                if (tmp.Rows.Count > 0)
                {
                    dt.Rows[i]["ProductSales"] =
                        decimal.Parse(tmp.Compute("SUM(TotalAmount)","").ToString());

                    dt.Rows[i]["Defi2"] =
                        (decimal.Parse(dt.Rows[i]["ProductSales"].ToString()) - decimal.Parse(dt.Rows[i]["TotalSystemRecorded"].ToString())).ToString();
                }    
            }
            return dt;
        }

        public static DataTable FetchUserWithPassword()
        {
            UserMstCollection cr = new UserMstCollection();
            cr.Load();
            DataTable dt = cr.ToDataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["password"] = UserController.DecryptData(dt.Rows[i]["password"].ToString());                
            }
            return dt;
        }

        public static int GetQuarterFromDate(System.DateTime dDate)
        {
            //Get the current month 
            int i = dDate.Month;

            //Based on the current month return the quarter 
            if (i <= 3)
            { return 1; }
            else if (i >= 4 && i <= 6)
            { return 2; }
            else if (i >= 7 && i <= 9)
            { return 3; }
            else if (i >= 10 && i <= 12)
            { return 4; }
            else
                //Something probably is wrong  
                return 0;
        }

        public static string CreateNewGenericHdrRefNo
            (string TableName, string KeyColumnName, int PointOfSaleID)
        {
            int runningNo = 0;

            IDataReader ds = PowerPOS.SPs.GetNewGenericHdrNoByPointOfSaleID(TableName, KeyColumnName,
                PointOfSaleID.ToString()).GetReader();
            while (ds.Read())
            {
                if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
                {
                    runningNo = 0;
                }
            }
            ds.Close();
            runningNo += 1;

            //YYMMDDSSSSNNNN                
            //YY - year
            //MM - month
            //DD - day
            //SSSS - PointOfSale ID
            //NNNN - Running No
            return DateTime.Now.ToString("yyMMdd") +
                PointOfSaleID.ToString().PadLeft(4, '0') +
                runningNo.ToString().PadLeft(4, '0');
        }

        public static DataTable FetchDataBaseLog(bool useStartDate, bool useEndDate,
            DateTime StartDate, DateTime EndDate, string logMessage, string SortColumn, string SortDir)
        {            
            PowerLogCollection myPowerLog = new PowerLogCollection();
            if (useStartDate & useEndDate)
            {
                myPowerLog.BetweenAnd(PowerLog.Columns.LogDate, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myPowerLog.Where(PowerLog.Columns.LogDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myPowerLog.Where(PowerLog.Columns.LogDate, SubSonic.Comparison.LessOrEquals, EndDate);
            }
            
            if (logMessage != "")
            {
                myPowerLog.Where(PowerLog.Columns.LogMsg, SubSonic.Comparison.Like, logMessage);
            }            

            SubSonic.TableSchema.TableColumn t = PowerLog.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myPowerLog.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myPowerLog.OrderByDesc(SortColumn);
                }
            }
            else
            {
                myPowerLog.OrderByDesc(PowerLog.Columns.LogDate);
            }
            return myPowerLog.Load().ToDataTable();
        }

        public static DataTable RoundToTwoDecimalDigits(DataTable dataTable)
        {
            try
            {
                DataTable dt = dataTable.Copy();
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.DataType == Type.GetType("System.Decimal"))
                        {
                            decimal amount;
                            if (decimal.TryParse(row[col].ToString(), out amount))
                                row[col] = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return dataTable;
            }
        }

        public static int GetMinOrderDateYear()
        {
            int year = DateTime.Now.Year;

            try
            {
                string sql = "select year(min(orderdate)) from orderhdr";
                object obj = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));
                if (obj is int)
                    year = (int)obj;
                
                return year;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return year;
            }
        }

        /*
        public static void DeleteOldData(int howManyMonthsBehind)
        {
            string[,] tables = { { "OrderSubTotal", "CreatedOn" },
                                 { "OrderDetAttributes", "CreatedOn" },
                                 { "OrderHdr", "OrderDate" }, 
                                 { "InventoryHdr", "InventoryDate" }, 
                                 { "PurchaseOrderHeader", "PurchaseOrderDate" }, 
                                 { "CounterCloseLog", "EndTime" }, 
                                 { "CashRecording", "CashRecordingTime" }, 
                                 { "StockTake", "StockTakeDate" },
                                 { "LoginActivity", "LoginDateTime" },
                                 { "SpecialActivityLog", "ActivityTime" },
                                 { "VoidLog", "CreatedOn" },
                                 { "EZLinkUnCfmLog", "OrderDate" },
                                 { "MeterReading", "ReadingDate" },
                                 { "RentalCollection", "CollectionDate" },
                                 { "PowerLog", "LogDate" },
                                 { "EZLinkMsgLog", "msgDate" },
                                 { "WarningMsg", "CreatedOn" }
                                  };

            DateTime CutOffDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (howManyMonthsBehind > 0)
                howManyMonthsBehind = -howManyMonthsBehind;

            if (howManyMonthsBehind != 0)
            {
                CutOffDate = CutOffDate.AddMonths(howManyMonthsBehind);
            }
            
            for (int i = 0; i < tables.Length / 2; i++)
            {
                Query qr = new Query(tables[i, 0]);
                qr.QueryType = QueryType.Delete;
                if (howManyMonthsBehind != 0)
                {
                    qr.AddWhere(tables[i, 1], Comparison.LessOrEquals, CutOffDate);
                }
                qr.CommandTimeout = 500000;
                qr.Execute();
            }
        }
        
        public static void CleanUpAllTablesForMultiLocationClient()
        {
            //NOTICE: THIS FUNCTION WILL DELETE SYSTEM ITEMS AND MAY CAUSE PROGRAM NOT TO WORK
            //ONLY USE THIS FOR MULTILOCATION CLIENT
            string[] tables = 
            {
                "LoginActivity","Camera","InventoryStockOutReason","Stall","SyncRequest",
                "PointOfSale","Outlet","InventoryLocation",                                                
                "PromoCampaignDet","PromoCampaignHdr","PromoDiscountTier",                
                "AlternateBarcode","EventItemMap","EventLocationMap",
                "SpecialEvent","ItemGroupMap","ItemGroup","ItemAttributesMap","ItemSetMealMap",
                "Item","Category","GST","Bank",
                "RentalPaymentType","MeterReadingCategory","GroupUserPrivilege",
                "UserPrivilege","UserMst","UserGroup","Department"};

            for (int i = 0; i < tables.Length; i++)
            {
                Query qr = new Query(tables[i]);
                qr.QueryType = QueryType.Delete;
                qr.CommandTimeout = 500000;
                qr.Execute();
                Logger.writeLog(tables[i] + " completely deleted");
            }

        }
        */
    }
}