using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;
using System.IO;
using PowerPOS.Container;

namespace PowerPOS
{
    
    public partial class CashRecordingController
    {
        public static int CASHRECORDINGTYPE_CASHOUT = 2;
        public static int CASHRECORDINGTYPE_CASHIN = 1;
        public static int CASHRECORDINGTYPE_OPENINGBALANCE = 3;


        #region Cash Out
        public static bool createCashOutFromOrder(string remark, decimal amount, DateTime dt, string username, string supName, out QueryCommand qc)
        {
            qc = new QueryCommand("");
            try
            {
                string newCashRefNo = "";
                if (!getNewCashRecordingRefNo(PointOfSaleInfo.PointOfSaleID, out newCashRefNo))
                {
                    return false;
                }

                CashRecording cr = new CashRecording();
                
                cr.Amount = amount;
                cr.CashierName = username;
                cr.CashRecordingTime = dt;
                cr.CashRecordingTypeId = CASHRECORDINGTYPE_CASHOUT;
                cr.CashRecRefNo = newCashRefNo;
                cr.Remark = remark;
                cr.SupervisorName = supName;
                cr.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                cr.UniqueID = Guid.NewGuid();
                qc = cr.GetInsertCommand(username);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool getNewCashRecordingRefNo(int PointOfSaleID, out string result)
        {
            try
            {
                int runningNo = 0;
                IDataReader ds = PowerPOS.SPs.GetNewCashRecRefNoByPointOfSaleID(PointOfSaleID).GetReader();
                while (ds.Read())
                {
                    if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
                    {
                        runningNo = 0;
                    }
                }
                ds.Close();
                runningNo += 1;

                //CRYYMMDDSSSSNNNN                
                //YY - year
                //MM - month
                //DD - day
                //SSSS - PointOfSale ID
                //NNNN - Running No
                result = "CR" + DateTime.Now.ToString("yyMMdd") + PointOfSaleID.ToString().PadLeft(4, '0') + runningNo.ToString().PadLeft(4, '0');
                return true;
            }
            catch (Exception ex)
            {
                result = "";
                Logger.writeLog("Get new recording no failed." + ex.Message);
                return false;
            }

            
        }
        #endregion

        
    }
}
