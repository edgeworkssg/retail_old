using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class POSController
    {
        public string FundingMethod = "";

        public bool IsFundingSelected()
        {
            if (string.IsNullOrEmpty(FundingMethod) || FundingMethod.ToUpper() == "NOFUNDING")
                return false;
            else
                return true;
        }

        public bool Funding_ApplyPAMedifund(out string status, out decimal FundingAmt)
        {
            status = "";
            FundingAmt = 0;

            try
            {
                decimal percentage = AppSetting.CastDecimal(AppSetting.GetSetting(AppSetting.SettingsName.Funding.PAMedPercentage), 0);
                OrderDetCollection oDets = FetchUnsavedOrderDet();
                foreach (OrderDet od in oDets)
                {
                    if (od.IsVoided) continue;
                    if (od.Item.IsPAMedifund)
                    {
                        int sign = Math.Sign(od.Amount);

                        #region *) Calculate Funding Amount (funding only applicable to 1 qty)
                        decimal cap = od.Item.Category.PAMedifundCap;
                        if (cap > 0)
                        {
                            decimal amt = Math.Round((percentage / 100) * (od.Amount / od.Quantity.GetValueOrDefault(0)), 2, MidpointRounding.AwayFromZero);
                            if (amt <= cap)
                                od.FundingAmount = amt * sign;
                            else
                                od.FundingAmount = cap * sign;
                            FundingAmt += od.FundingAmount;
                        }
                        else
                        {
                            // fully paid by funding
                            od.FundingAmount = (od.Amount / od.Quantity.GetValueOrDefault(0)) * sign;
                            FundingAmt += od.FundingAmount;
                        }
                        #endregion

                        #region *) Add to receipt line
                        if (Math.Abs(od.FundingAmount) > 0)
                        {
                            decimal change;
                            if (!AddReceiptLinePayment(od.FundingAmount, PAY_PAMEDIFUND, "",0,"",0, out change, out status))
                            {
                                Logger.writeLog(status);
                                return false;
                            }
                        }
                        #endregion
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public bool Funding_ApplySMF(out string status, out decimal FundingAmt)
        {
            status = "";
            FundingAmt = 0;

            try
            {
                decimal percentage = AppSetting.CastDecimal(AppSetting.GetSetting(AppSetting.SettingsName.Funding.SMFPercentage), 0);
                OrderDetCollection oDets = FetchUnsavedOrderDet();
                foreach (OrderDet od in oDets)
                {
                    if (od.IsVoided) continue;
                    if (od.Item.IsSMF)
                    {
                        int sign = Math.Sign(od.Amount);

                        #region *) Calculate Funding Amount (funding only applicable to 1 qty)
                        decimal cap = od.Item.Category.SMFCap;
                        if (cap > 0)
                        {
                            decimal amt = Math.Round((percentage / 100) * (od.Amount / od.Quantity.GetValueOrDefault(0)), 2, MidpointRounding.AwayFromZero);
                            if (amt <= cap)
                                od.FundingAmount = amt * sign;
                            else
                                od.FundingAmount = cap * sign;
                            FundingAmt += od.FundingAmount;
                        }
                        else
                        {
                            // fully paid by funding
                            od.FundingAmount = (od.Amount / od.Quantity.GetValueOrDefault(0)) * sign;
                            FundingAmt += od.FundingAmount;
                        }
                        #endregion

                        #region *) Add to receipt line
                        if (Math.Abs(od.FundingAmount) > 0)
                        {
                            decimal change;
                            if (!AddReceiptLinePayment(od.FundingAmount, PAY_SMF, "",0,"",0, out change, out status))
                            {
                                Logger.writeLog(status);
                                return false;
                            }
                        }
                        #endregion
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public decimal GetTotalFundingAmount()
        {
            decimal total = 0;
            OrderDetCollection oDets = FetchUnsavedOrderDet();
            foreach (OrderDet od in oDets)
            {
                total += od.FundingAmount;
            }
            return total;
        }

        public bool HasPAMedifundItem()
        {
            OrderDetCollection oDets = FetchUnsavedOrderDet();
            foreach (OrderDet od in oDets)
            {
                if (od.Item.IsPAMedifund) return true;
            }
            return false;
        }

        public bool HasSMFItem()
        {
            OrderDetCollection oDets = FetchUnsavedOrderDet();
            foreach (OrderDet od in oDets)
            {
                if (od.Item.IsSMF) return true;
            }
            return false;
        }

        public bool HasCreditNote()
        {
            OrderDetCollection oDets = FetchUnsavedOrderDet();
            foreach (OrderDet od in oDets)
            {
                if (od.ItemNo == Installment.CreditNote) return true;
            }
            return false;
        }

        public bool HasRefundInstallment()
        {
            ReceiptDetCollection oDets = FetchReceiptDet();
            foreach (ReceiptDet od in oDets)
            {
                if (od.PaymentType == Installment.CreditName && od.Amount < 0) return true;
            }
            return false;
        }

        public void ClearFundingAmount()
        {
            OrderDetCollection oDets = FetchUnsavedOrderDet();
            foreach (OrderDet od in oDets)
            {
                od.FundingAmount = 0;
            }
        }
    }
}
