using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using System.Configuration;
using System.IO;
using SubSonic;
using System.Data;

namespace ERPIntegration.API.NAVISION
{
    public static class Navision_Sales
    {
        struct MessageType
        {
            public static string INFO = "NAV_INVPAY_INFO";
            public static string ERROR = "NAV_INVPAY_ERROR";
        }

        private static List<string> GetOrderHeaders(DateTime orderDate)
        {
            List<string> listID = new List<string>();
            string sql = @"
                            SELECT OrderHdrID 
                            FROM OrderHdr 
                            WHERE CONVERT(date, OrderDate) = @OrderDate 
                                AND IsVoided = 0 AND ISNULL(" + OrderHdr.UserColumns.IsExported + @", 0) = 0
                            ORDER BY OrderHdrID
                          ";
            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
            cmd.Parameters.Add("@OrderDate", orderDate.Date, DbType.Date);
            DataTable dt = DataService.GetDataSet(cmd).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                listID.Add(dr[0] + "");
            }

            return listID;
        }

        private static void GenerateOrder(List<string> listID, string fileName)
        {
            // NAV_INV

            Console.WriteLine();
            Helper.WriteLog("Generating csv file for Order...", MessageType.INFO);

            List<string> OutputFileValue = new List<string>();

            for (int i = 0; i < listID.Count; i++)
            {
                Helper.WriteLog(string.Format("Processing order #{0} : {1}.", (i + 1).ToString(), listID[i]), MessageType.INFO);
                string sql = @"
                                SELECT '""' + LEFT(REPLACE(OH.MembershipNo, '""', '\""'), 10) + '""' -- 1 cust_code
                                       + ',' + '""' + LEFT(REPLACE(MBR.NameToAppear, '""', '\""'), 128) + '""' -- 2 cust_name
                                       + ',' + CONVERT(varchar(50), OH.OrderDate, 103) -- 3 order_date
                                       + ',' + '""' + LEFT(REPLACE(LOC.InventoryLocationName, '""', '\""'), 10) + '""' -- 4 whs_code
                                       + ',' + OH.OrderHdrID -- 5 Order_number
                                       + ',' + '""' + LEFT(REPLACE(CASE WHEN DI.ItemNo IS NULL THEN IT.Attributes1 ELSE DI.Attributes1 END, '""', '\""'), 25) + '""' -- 6 prod_code
                                       + ',' + CONVERT(varchar(max), CONVERT(decimal(18, 5), CASE WHEN DI.ItemNo IS NULL THEN OD.Quantity ELSE OD.Quantity * IT.userfloat5 END)) -- 7 qty_ordered
                                       + ',' + '""' + LEFT(REPLACE(ISNULL(CASE WHEN DI.ItemNo IS NULL THEN IT.Attributes2 ELSE DI.Attributes2 END, ''), '""', '\""'), 10) + '""' -- 8 UOM
                                       + ',' + CONVERT(varchar(max), OD.Amount) -- 9 net_amount
                                       + ',' + CONVERT(varchar(max), ROUND(OD.Discount / 100 * OD.GrossSales, 2)) -- 10 discount_amount
                                       + ',' + CONVERT(varchar(max), OD.Amount + ROUND(OD.Discount / 100 * OD.GrossSales, 2)) -- 11 gross_amount
                                       + ',SGD' -- 12 Currency
                                FROM OrderHdr OH
                                    INNER JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
                                    INNER JOIN Item IT ON IT.ItemNo = OD.ItemNo
                                    LEFT JOIN Item DI ON IT.Userflag7 = 1 AND IT.Userfld8 = DI.ItemNo
                                    INNER JOIN Membership MBR ON MBR.MembershipNo = OH.MembershipNo
                                    INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                                    INNER JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                    INNER JOIN InventoryLocation LOC ON LOC.InventoryLocationID = OU.InventoryLocationID
                                WHERE OD.IsVoided = 0 AND OH.OrderHdrID = '{0}' 
                              ";
                sql = string.Format(sql, listID[i]);
                DataTable dt = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS")).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        OutputFileValue.Add(dr[0] + "");
                    }
                }
            }

            if (OutputFileValue.Count > 0)
            {
                #region *) Output: Create transaction file
                Helper.WriteLog("Saving csv file for Order...", MessageType.INFO);
                using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    TextWriter tw = new StreamWriter(stream);
                    tw.Write(string.Join(Environment.NewLine, OutputFileValue.ToArray()));
                    tw.Flush();
                    tw.Close();
                }
                #endregion

                #region *) Set OrderHdr.IsExported = true (OrderHdr.Userflag2)
                Helper.WriteLog("Marking Order as exported...", MessageType.INFO);
                QueryCommandCollection cmdColl = new QueryCommandCollection();
                foreach (string orderHdrID in listID)
                {
                    Query qry = new Query("OrderHdr");
                    qry.QueryType = QueryType.Update;
                    qry.AddUpdateSetting(OrderHdr.UserColumns.IsExported, true);
                    qry.AddWhere(OrderHdr.Columns.OrderHdrID, orderHdrID);
                    cmdColl.Add(qry.BuildUpdateCommand());
                }
                DataService.ExecuteTransaction(cmdColl);
                Helper.WriteLog("Complete marking Order as exported", MessageType.INFO);
                #endregion
            }

            Helper.WriteLog("Generating csv file for Order completed successfully.", MessageType.INFO);
        }

        private static void GeneratePayment(List<string> listID, string fileName)
        {
            // NAV_Pay

            Console.WriteLine();
            Helper.WriteLog("Generating csv file for Payment...", MessageType.INFO);

            List<string> OutputFileValue = new List<string>();

            for (int i = 0; i < listID.Count; i++)
            {
                Helper.WriteLog(string.Format("Processing payment #{0} : {1}.", (i + 1).ToString(), listID[i]), MessageType.INFO);
                string sql = @"
                                SELECT '""' + LEFT(REPLACE(RD.PaymentType, '""', '\""'), 10) + '""' -- 1 payment_term
                                       + ',' + CONVERT(varchar(50), OH.OrderDate, 103) -- 2	Date
                                       + ',' + CONVERT(varchar(max), RD.Amount) -- 3 Amount
                                       + ',' + '' -- 4 GL account
                                       + ',SGD' -- 5 Currency
                                       + ',' + '""' + LEFT(REPLACE(OH.MembershipNo, '""', '\""'), 10) + '""' -- 6 customer no
                                       + ',' + '""' + LEFT(REPLACE(MBR.NameToAppear, '""', '\""'), 25) + '""' -- 7 customer name
                                FROM OrderHdr OH
                                    INNER JOIN Membership MBR ON MBR.MembershipNo = OH.MembershipNo
                                    INNER JOIN ReceiptHdr RH ON RH.OrderHdrID = OH.OrderHdrID
                                    INNER JOIN ReceiptDet RD ON RD.ReceiptHdrID = RH.ReceiptHdrID
                                WHERE OH.OrderHdrID = '{0}' 
                                ORDER BY OH.OrderHdrID
                              ";
                sql = string.Format(sql, listID[i]);
                DataTable dt = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS")).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        OutputFileValue.Add(dr[0] + "");
                    }
                }
            }

            if (OutputFileValue.Count > 0)
            {
                #region *) Output: Create transaction file
                Helper.WriteLog("Saving csv file for Payment...", MessageType.INFO);
                using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    TextWriter tw = new StreamWriter(stream);
                    tw.Write(string.Join(Environment.NewLine, OutputFileValue.ToArray()));
                    tw.Flush();
                    tw.Close();
                }
                #endregion
            }

            Helper.WriteLog("Generating csv file for Order completed successfully.", MessageType.INFO);
        }

        public static bool DoSendSales(List<string> arguments, out string status)
        {
            status = "";
            bool result = false;

            try
            {
                string localDirectory = ConfigurationManager.AppSettings["LocalDirectory"];
                string dirOrder, dirPayment;
                DateTime orderDate = DateTime.Today;

                string ar = arguments.Find(s => s.ToLower().StartsWith("-date:"));
                if (!string.IsNullOrEmpty(ar))
                {
                    DateTime tmpDate;
                    if (DateTime.TryParse(ar.Remove(0, 6), out tmpDate))
                    {
                        orderDate = tmpDate;
                    }
                    else
                    {
                        status = "Invalid date provided in arguments.";
                        return false;
                    }
                }

                Helper.WriteLog("Starting Export Order & Payment module", MessageType.INFO);

                if (localDirectory.Contains('|'))
                {
                    string[] tmpDir = localDirectory.Split('|');
                    dirOrder = tmpDir[0];
                    dirPayment = tmpDir[1];
                }
                else
                {
                    dirOrder = localDirectory;
                    dirPayment = localDirectory;
                }

                DirectoryInfo fileDirOrder = new DirectoryInfo(dirOrder);
                DirectoryInfo fileDirPayment = new DirectoryInfo(dirPayment);

                if (!fileDirOrder.Exists)
                {
                    Helper.WriteLog(string.Format("Directory {0} does not exist.", dirOrder), MessageType.ERROR);
                    return false;
                }

                if (!fileDirPayment.Exists)
                {
                    Helper.WriteLog(string.Format("Directory {0} does not exist.", dirPayment), MessageType.ERROR);
                    return false;
                }

                List<string> listID = GetOrderHeaders(orderDate);
                Helper.WriteLog(string.Format("Found {0} order(s) to export.", listID.Count), MessageType.INFO);

                if (listID.Count > 0)
                {
                    string orderFile = Path.Combine(fileDirOrder.FullName, "INV_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv");
                    GenerateOrder(listID, orderFile);

                    string paymentFile = Path.Combine(fileDirPayment.FullName, "PMT_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv");
                    GeneratePayment(listID, paymentFile);
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            return result;
        }
    }
}
