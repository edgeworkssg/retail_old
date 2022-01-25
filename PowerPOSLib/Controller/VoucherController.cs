using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;

namespace PowerPOS
{
    public partial class VoucherController
    {
        public const int VOUCHER_PRINTED = 1;
        public const int VOUCHER_SOLD = 2;
        public const int VOUCHER_REDEEMED = 3;
        public VoucherController() 
        {            

        }

        public static bool CreateBatchVoucherNo(
            int StartNumber, int endNumber,
            string PreFix, string SuFix, decimal amount, int numberOfDigit, DateTime issueDate,
            DateTime expiryDate, out string status)
        {
            return CreateBatchVoucherNo(StartNumber, endNumber, PreFix, SuFix, amount, numberOfDigit, issueDate, expiryDate, null, null, out status);
        }

        public static bool CreateBatchVoucherNo(
            int StartNumber, int endNumber, 
            string PreFix, string SuFix, decimal amount, int numberOfDigit, DateTime issueDate,
            DateTime expiryDate, int? voucherHeaderID, string outlet, out string status)
        {
            try
            {
                QueryCommandCollection cmd = new QueryCommandCollection();
                for (int i = StartNumber; i < endNumber + 1; i++)
                {
                    string voucherNo = PreFix + i.ToString().PadLeft(numberOfDigit, '0') + SuFix;

                    #region *) Validate if using VoucherHeaderID
                    if (voucherHeaderID.HasValue)
                    {
                        VoucherCollection vcoll = new VoucherCollection();
                        vcoll.Where(Voucher.Columns.VoucherNo, voucherNo);
                        vcoll.Where(Voucher.Columns.VoucherHeaderID, voucherHeaderID.Value);
                        vcoll.Where(Voucher.Columns.Deleted, false);
                        vcoll.Load();
                        if (vcoll.Count > 0) continue; // Don't insert if already existed
                    }
                    #endregion

                    Voucher v = new Voucher();
                    v.VoucherNo = voucherNo;
                    v.Amount = amount;
                    v.IsNew = true;
                    v.VoucherStatusId = VOUCHER_PRINTED;
                    v.DateIssued = issueDate;
                    v.ExpiryDate = expiryDate;
                    v.VoucherHeaderID = voucherHeaderID;
                    v.Outlet = outlet;
                    cmd.Add(v.GetSaveCommand(UserInfo.username));
                }
                DataService.ExecuteTransaction(cmd);
                status = "";
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public static bool CancelBatchVoucherNo(int voucherHeaderID, string username, out string status)
        {
            try
            {
                status = "";
                string sql = "UPDATE Vouchers SET Deleted = 1, DateCanceled = GETDATE(), ModifiedOn = GETDATE(), ModifiedBy = '{2}' WHERE VoucherHeaderID = {0} AND VoucherStatusId <> {1} AND Deleted = 0";
                sql = string.Format(sql, voucherHeaderID, VOUCHER_REDEEMED, username);
                DataService.ExecuteQuery(new QueryCommand(sql, "PowerPOS"));
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public static bool ChangeBatchVoucherOutlet(int voucherHeaderID, string username, out string status)
        {
            try
            {
                status = "";
                string sql = "UPDATE Vouchers SET Outlet = (SELECT Outlet FROM VoucherHeader WHERE VoucherHeaderID = Vouchers.VoucherHeaderID), ModifiedOn = GETDATE(), ModifiedBy = '{1}' WHERE VoucherHeaderID = {0} AND Deleted = 0";
                sql = string.Format(sql, voucherHeaderID, username);
                DataService.ExecuteQuery(new QueryCommand(sql, "PowerPOS"));
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }
    }
}
