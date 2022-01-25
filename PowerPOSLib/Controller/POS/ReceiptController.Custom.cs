using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SubSonic;
namespace PowerPOS
{
    public partial class ReceiptController
    {
        public static string LoadLastTransaction(int PointOfSaleID, string cashierID)
        {
            /*Query qr = OrderHdr.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = OrderHdr.Columns.OrderHdrID;
            qr.Top = "1";
            Object obj = qr.WHERE(OrderHdr.Columns.PointOfSaleID, PointOfSaleID).
                AND(OrderHdr.Columns.CashierID, cashierID).ORDER_BY(OrderHdr.Columns.OrderDate, "DESC").ExecuteScalar();

            if (obj == null)
                return "";

            return obj.ToString();*/
            string sqlString = "Select top 1 OrderHdrID from OrderHdr where createdon < '" +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' order by createdon DESC ";
            DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["OrderHdrID"].ToString();
            }
            return "";
        }

        //Re-model this using ViewReceiptSummary
        public static decimal GetTotalSystemCollected
            (DateTime startDate, DateTime endDate, int PointOfSaleId)
        {
            //try
            //{
            //    SubSonic.Where where = new Where();

            //    object obj = new SubSonic.Select("SUM(ROUND(ReceiptHdr.Amount,2))")
            //                    .From(ReceiptHdr.Schema.TableName)
            //                    .InnerJoin(OrderHdr.Schema.TableName, OrderHdr.Columns.OrderHdrID, ReceiptHdr.Schema.TableName, ReceiptHdr.Columns.OrderHdrID)
            //                    .Where(OrderHdr.IsVoidedColumn).IsEqualTo(0)
            //                    .And(ReceiptHdr.ReceiptDateColumn).IsBetweenAnd(startDate, endDate)
            //                    .And(ReceiptHdr.PointOfSaleIDColumn).IsEqualTo(PointOfSaleId)
            //                    .And(ReceiptHdr.IsVoidedColumn).IsEqualTo(0)
            //                    .ExecuteScalar();

            //    //query  
            //    Decimal value;
            //    if (Decimal.TryParse(obj.ToString(), out value))
            //        return value;
            //    else
            //        return 0;
            //}
            //catch
            //{
            //    throw;
            //}
            decimal totalAmount = 0;

            try
            {
                string sql = @"SELECT   ISNULL(SUM(ROUND(RD.Amount,2)),0) TotalAmount
                                FROM	ReceiptHdr RH
		                                LEFT JOIN ReceiptDet RD ON RD.ReceiptHdrID = RH.ReceiptHdrID
		                                LEFT JOIN OrderHdr OH ON OH.OrderHdrID = RH.OrderHdrID
                                WHERE	OH.IsVoided = 0	AND RH.IsVoided = 0
		                                AND RH.PointOfSaleID = {0}
		                                AND RH.ReceiptDate BETWEEN CAST('{1}' AS DATETIME) 
							                                   AND CAST('{2}' AS DATETIME)
		                                AND RD.PaymentType NOT LIKE 'CASH-%'";
                sql = string.Format(sql, PointOfSaleId
                                       , startDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                                       , endDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                if (dt.Rows.Count > 0)
                    totalAmount = (decimal)dt.Rows[0]["TotalAmount"];
            }
            catch (Exception ex)
            {
                totalAmount = 0;
                Logger.writeLog(ex);
            }

            return totalAmount;
        }

        //Re-model this using ViewReceiptSummary
        public static void GetSystemCollectedBreakdownByPaymentType
            (DateTime startDate, DateTime endDate, int PointOfSaleId,
                 out decimal TotalCashColleted,
                 out decimal TotalPay1Colleted,
                 out decimal TotalPay2Colleted,
                 out decimal TotalVoucherColleted,
                out decimal TotalPay3Colleted,     
                out decimal TotalPay4Colleted,
                out decimal TotalPay5Colleted,
                out decimal TotalPay6Colleted,
                out decimal TotalPay7Colleted,
            out decimal TotalPay8Colleted,
            out decimal TotalPay9Colleted,
            out decimal TotalPay10Colleted,
            out decimal TotalChequeCollected,
                 out decimal TotalPointCollected,
                 out decimal TotalPackageCollected,
                out decimal TotalSMFCollected,
                out decimal TotalPAMedCollected,
                out decimal TotalPWFCollected,
            out decimal TotalNETSCashCardCollected,
            out decimal TotalNETSFlashPayCollected,
            out decimal TotalNETSATMCardCollected,
            out decimal TotalForeignCurrency,
            out Dictionary<string,decimal> ForeignCurrencyDetail
            )
        {
            try
            {                
                TotalCashColleted = 0;
                TotalPay1Colleted = 0;
                TotalPay2Colleted = 0;
                TotalPay3Colleted = 0;
                TotalPay4Colleted = 0;
                TotalPay5Colleted = 0;
                TotalPay6Colleted = 0;
                TotalPay7Colleted = 0;
                TotalPay8Colleted = 0;
                TotalPay9Colleted = 0;
                TotalPay10Colleted = 0;
                TotalVoucherColleted = 0;
                
                TotalChequeCollected = 0;
                TotalPointCollected = 0;
                TotalPackageCollected = 0;

                TotalSMFCollected = 0;
                TotalPAMedCollected = 0;
                TotalPWFCollected = 0;

                TotalNETSCashCardCollected = 0;
                TotalNETSFlashPayCollected = 0;
                TotalNETSATMCardCollected = 0;
                TotalForeignCurrency = 0;

                ForeignCurrencyDetail = new Dictionary<string, decimal>();

                ReceiptHdrCollection rHdr = new ReceiptHdrCollection();
                #region *) Quarry: Load all receipts (ReceiptHdr)
                //startDate = new DateTime(2014, 12, 23, 0, 0, 0);
                //endDate = new DateTime(2014, 12, 24, 11, 21, 24);
                rHdr.BetweenAnd(ReceiptHdr.Columns.ReceiptDate, startDate, endDate);
                rHdr.Where(ReceiptHdr.Columns.PointOfSaleID, PointOfSaleId);
                rHdr.Where(ReceiptHdr.Columns.IsVoided, false);
                rHdr.Load();
                #endregion

                for (int i = 0; i < rHdr.Count; i++)
                {
                    ReceiptDetCollection rDet = new ReceiptDetCollection();
                    #region *) Quary: Load all payments (ReceiptDet)
                    rDet.Where(ReceiptDet.Columns.ReceiptHdrID, rHdr[i].ReceiptHdrID);
                    rDet.Load();
                    #endregion                    

                    for (int j = 0; j < rDet.Count; j++)
                    {
                        switch (rDet[j].PaymentType)
                        {
                            case POSController.PAY_CASH:
                                TotalCashColleted += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                break;
                            /*case POSController.PAY_VOUCHER:
                                TotalVoucherColleted += rDet[j].Amount;
                                break;                            */
                            case POSController.PAY_CHEQUE:
                                TotalChequeCollected += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                break;
                            case POSController.PAY_POINTS:
                                TotalPointCollected += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                break;
                            case POSController.PAY_PACKAGE:
                                TotalPackageCollected += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                break;
                            case POSController.PAY_SMF:
                                TotalSMFCollected += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                break;
                            case POSController.PAY_PAMEDIFUND:
                                TotalPAMedCollected += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                break;
                            case POSController.PAY_PWF:
                                TotalPWFCollected += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                break;
                            case POSController.PAY_NETSFlashPay:
                                TotalNETSFlashPayCollected += rDet[j].Amount;
                                break;
                            case POSController.PAY_NETSCashCard:
                                TotalNETSCashCardCollected += rDet[j].Amount;
                                break;
                            case POSController.PAY_NETSATMCard:
                                TotalNETSATMCardCollected += rDet[j].Amount;
                                break;
                            default:                                
                                if (rDet[j].PaymentType == PaymentTypesController.FetchPaymentByID("1"))
                                {
                                    TotalPay1Colleted += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                }
                                else if (rDet[j].PaymentType == PaymentTypesController.FetchPaymentByID("2"))
                                {
                                    TotalPay2Colleted += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                }
                                else if (rDet[j].PaymentType == PaymentTypesController.FetchPaymentByID("3"))
                                {                                
                                    TotalPay3Colleted += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                }
                                else if (rDet[j].PaymentType == PaymentTypesController.FetchPaymentByID("4"))
                                {
                                    TotalPay4Colleted += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                }
                                else if (rDet[j].PaymentType == PaymentTypesController.FetchPaymentByID("5"))
                                {
                                    TotalPay5Colleted += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                }
                                else if (rDet[j].PaymentType == PaymentTypesController.FetchPaymentByID("6"))
                                {
                                    TotalPay6Colleted += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                }
                                else if (rDet[j].PaymentType == PaymentTypesController.FetchPaymentByID("7"))
                                {
                                    TotalPay7Colleted += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                }
                                else if (rDet[j].PaymentType == PaymentTypesController.FetchPaymentByID("8"))
                                {
                                    TotalPay8Colleted += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                }
                                else if (rDet[j].PaymentType == PaymentTypesController.FetchPaymentByID("9"))
                                {
                                    TotalPay9Colleted += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                }
                                else if (rDet[j].PaymentType == PaymentTypesController.FetchPaymentByID("10"))
                                {
                                    TotalPay10Colleted += Math.Round(rDet[j].Amount, 2, MidpointRounding.AwayFromZero);
                                }
                                else if (rDet[j].PaymentType.ToUpper().StartsWith("CASH-"))
                                {
                                    if (ForeignCurrencyDetail.ContainsKey(rDet[j].PaymentType))
                                        ForeignCurrencyDetail[rDet[j].PaymentType] += rDet[j].ForeignAmountReceived.GetValueOrDefault(0);
                                    else
                                        ForeignCurrencyDetail.Add(rDet[j].PaymentType, rDet[j].ForeignAmountReceived.GetValueOrDefault(0));
                                    TotalForeignCurrency += rDet[j].Amount;
                                }
                                break;                            
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static DataTable FetchReceiptDetail
            (DateTime startDate, DateTime endDate, string paymentType, int PointOfSaleId)
        {
            ViewReceiptDetCollection recDet = new ViewReceiptDetCollection();
            recDet.BetweenAnd(ViewReceiptDet.Columns.ReceiptDate, startDate, endDate);
            recDet.Where(ViewReceiptDet.Columns.PointOfSaleID, PointOfSaleId);
            recDet.Where(ViewReceiptDet.Columns.IsVoided, false);
            recDet.Where(ViewReceiptDet.Columns.PaymentType, SubSonic.Comparison.Like, paymentType);
            recDet.Load();
            DataTable dt = recDet.ToDataTable();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Amount"] = decimal.Parse(dt.Rows[i]["Amount"].ToString()).ToString("N2");
            }
            return dt;
        }


        public static bool ChangePaymentMode(string ReceiptDetID, string newPaymentMode, out string status)
        {
            status = "";
            try
            {
                Query qry = new Query("ReceiptDet");
                qry.AddUpdateSetting(ReceiptDet.Columns.PaymentType, newPaymentMode);
                qry.WHERE(ReceiptDet.Columns.ReceiptDetID, ReceiptDetID).Execute();

                return true;
            }
            catch (Exception ex)
            {
                status = "Error while updating payment mode. Detailed error:" + ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public static decimal FetchTotalInstallment(DateTime startDate, DateTime endDate, int pointOfSaleID)
        {

            Query qr = ViewReceiptDet.CreateQuery();
            object obj =
            qr.WHERE(ViewReceiptDet.Columns.PaymentType, POSController.PAY_INSTALLMENT).
                AND(ViewReceiptDet.Columns.IsVoided, false).
                AND(ViewReceiptDet.Columns.ReceiptDate, Comparison.GreaterOrEquals, startDate).
                AND(ViewReceiptDet.Columns.ReceiptDate, Comparison.LessOrEquals, endDate).
                AND(ViewReceiptDet.Columns.PointOfSaleID, pointOfSaleID).
                GetSum(ViewReceiptDet.Columns.Amount);
            decimal totalInstallment;

            string qryPaid =
                "select isnull(sum(amount),0) val from orderdet where itemno like 'INST_PAYMENT' " +
                "and isnull(userfld3,'') in (select orderhdrid from orderhdr where orderdate > '" +
                startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and orderdate < '" + 
                endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'and isvoided = 0)";
            decimal totalPaid;
            object tmp = 
                DataService.ExecuteScalar(new QueryCommand(qryPaid));
            if (decimal.TryParse(obj.ToString(), out totalInstallment) )
            {
                if (decimal.TryParse(tmp.ToString(), out totalPaid))
                {
                    return totalInstallment - totalPaid;
                }
                else
                {
                    return totalInstallment;
                }
            }

            return 0.0M;

        }

        public static decimal FetchTotalInstallment(DateTime startDate, DateTime endDate, int pointOfSaleID, out decimal totalPaid)
        {

            Query qr = ViewReceiptDet.CreateQuery();
            object obj =
            qr.WHERE(ViewReceiptDet.Columns.PaymentType, POSController.PAY_INSTALLMENT).
                AND(ViewReceiptDet.Columns.IsVoided, false).
                AND(ViewReceiptDet.Columns.ReceiptDate, Comparison.GreaterOrEquals, startDate).
                AND(ViewReceiptDet.Columns.ReceiptDate, Comparison.LessOrEquals, endDate).
                AND(ViewReceiptDet.Columns.PointOfSaleID, pointOfSaleID).
                GetSum(ViewReceiptDet.Columns.Amount);
            decimal totalInstallment;
            totalPaid = 0;
            string qryPaid =
                "select isnull(sum(amount),0) val from orderdet where itemno like 'INST_PAYMENT' " +
                "and isnull(userfld3,'') in (select orderhdrid from orderhdr where orderdate > '" +
                startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and orderdate < '" +
                endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'and isvoided = 0)";
            
            object tmp =
                DataService.ExecuteScalar(new QueryCommand(qryPaid));
            if (decimal.TryParse(obj.ToString(), out totalInstallment))
            {
                if (decimal.TryParse(tmp.ToString(), out totalPaid))
                {
                    return totalInstallment - totalPaid;
                }
                else
                {
                    return totalInstallment;
                }
            }

            return 0.0M;

        }

        public static DataTable FetchForeignCurrencyPayment(DateTime startDate, DateTime endDate, int pointOfSaleID)
        {
            try
            {
                string SQL = "select a.orderhdrid,a.nettamount, " +
                            "b.amount,b.paymentrefno from  " +
                            "orderhdr a inner join " +
                            "receiptdet b " +
                            "on a.orderhdrid = b.receipthdrid " +
                            "where a.isvoided=0 " +
                            "and orderdate > '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and orderdate < '" +
                            endDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            "and paymenttype = 'FOREIGN CURRENCY' " +
                            "order by a.createdon desc ";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                DataTable dt = DataService.GetDataSet(cmd).Tables[0];
                dt.Columns.Add("ForeignCurrencyCode");
                dt.Columns.Add("ExchangeRate", System.Type.GetType("System.Decimal"));
                dt.Columns.Add("ForeignCurrencyAmount", System.Type.GetType("System.Decimal"));
                dt.Columns.Add("Change", System.Type.GetType("System.Decimal"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[] splitted = dt.Rows[i]["PaymentRefNo"].ToString().Split(',');
                    if (splitted.Length == 4)
                    {
                        dt.Rows[i]["ForeignCurrencyCode"] = splitted[0].ToString();
                        dt.Rows[i]["ExchangeRate"] = Decimal.Parse(splitted[1].ToString());
                        dt.Rows[i]["ForeignCurrencyAmount"] = Decimal.Parse(splitted[2].ToString());
                        dt.Rows[i]["Change"] = Decimal.Parse(splitted[3].ToString());
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static CounterCloseDetCollection FetchCounterCloseDet(string CounterCloseID)
        {
            CounterCloseDetCollection col = new CounterCloseDetCollection();

            try
            {
                Query qr = CounterCloseDet.CreateQuery();
                qr.AddWhere(CounterCloseDet.Columns.CounterCloseID, CounterCloseID);
                qr.AddWhere(CounterCloseDet.Columns.Deleted, false);
                qr.ORDER_BY(CounterCloseDet.Columns.UnitValue, "ASC");

                col.LoadAndCloseReader(qr.ExecuteReader());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
            }

            return col;
        }
    }
}
