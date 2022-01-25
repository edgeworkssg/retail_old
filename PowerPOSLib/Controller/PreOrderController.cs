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
    public class PreOrderController
    {
        public PreOrderController()
        {

        }

        public bool insertPreOrderItem(string itemno, DateTime startDate, DateTime endDate)
        {
            Query qr = PreOrderSchedule.CreateQuery();
            object count = qr.WHERE(PreOrderSchedule.Columns.ItemNo, itemno).
                AND(PreOrderSchedule.Columns.ValidFrom, Comparison.LessOrEquals, DateTime.Now).AND(PreOrderSchedule.Columns.ValidTo, Comparison.GreaterOrEquals, DateTime.Now).
                GetCount(PreOrderSchedule.Columns.ItemNo);

            if (count is Int32 && (((int)count) > 0))
                return false;
            PreOrderSchedule.Insert(itemno, startDate, endDate, false, DateTime.Now, UserInfo.username, DateTime.Now, UserInfo.username);
            return true;
        }

        public void deletePreOrderItem(string preOrderID)
        {
            PreOrderSchedule.Delete(PreOrderSchedule.Columns.PreOrderID, preOrderID);
        }

        public ViewPreOrderScheduleCollection fetchPreOrderSchedule()
        {
            ViewPreOrderScheduleCollection v = new ViewPreOrderScheduleCollection();
            v.Where(ViewPreOrderSchedule.Columns.Deleted, false);
            v.Where(ViewPreOrderSchedule.Columns.ValidTo, Comparison.GreaterOrEquals, DateTime.Now);
            v.Where(ViewPreOrderSchedule.Columns.ValidFrom, Comparison.LessOrEquals, DateTime.Now);
            v.Load();
            return v;
        }

        internal static bool IsItemDefaultPreOrder(string itemno)
        {
            bool defaultPreOrder = false;

            Query qr = PreOrderSchedule.CreateQuery();
            Object count = qr.WHERE(PreOrderSchedule.Columns.ItemNo, itemno).AND(PreOrderSchedule.Columns.ValidFrom,
                Comparison.LessOrEquals, DateTime.Now).AND(PreOrderSchedule.Columns.ValidTo, Comparison.GreaterOrEquals,
                DateTime.Now).GetCount(PreOrderSchedule.Columns.ItemNo);
            if (count is Int32 && ((Int32)count) > 0) defaultPreOrder = true;

            return defaultPreOrder;
        }

        public static bool SendEmailNotify(string membershipno, string orderHdrID, string orderDetID, out string status)
        {
            status = "";
            try
            {
                var member = new Membership(membershipno);

                POSController pos = new POSController();
                OrderHdr oh = new OrderHdr(orderHdrID);
                OrderDetCollection odcol = new OrderDetCollection();
                OrderDet od = new OrderDet(orderDetID);
                odcol.Add(od);

                pos.myOrderHdr = oh;
                pos.myOrderDet = odcol;

                string EmailTo = "";
                string EmailSubject = "";
                string EmailBody = "";
                string EmailBcc = "";
                if (member != null)
                {
                    if (!string.IsNullOrEmpty(member.Email))
                        EmailTo = member.Email;
                    else
                        EmailTo = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_DefaultMailTo);

                    string useForReceiptNo = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_ReceiptNoInEmailReceipt);
                    string receiptNo = "";
                    if (string.IsNullOrEmpty(useForReceiptNo) || useForReceiptNo.ToLower() == "orderhdrid")
                        receiptNo = oh.OrderHdrID;
                    else if (useForReceiptNo.ToLower() == "custom invoice no")
                        receiptNo = oh.Userfld5;
                    else if (useForReceiptNo.ToLower() == "line info")
                        receiptNo = od.LineInfo;
                    else
                        receiptNo = oh.OrderHdrID;

                    EmailSubject = string.Format("Receipt {0} for purchase at {1}", receiptNo, CompanyInfo.CompanyName);
                    EmailBody = "";
                }

                #region *) Send BCC if necessary
                bool sendBcc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_BccToOwner), false);
                string ownerEmail = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_OwnerEmailAddress);
                if (sendBcc && !string.IsNullOrEmpty(ownerEmail))
                {
                    EmailBcc = ownerEmail;
                }
                #endregion

                //if (!POSDeviceController.SendMailNotifyDelivery(pos, EmailTo, EmailSubject, EmailBody, EmailBcc))
                //    throw new Exception("Send Email Failed");

                return true;
            }
            catch (Exception ex)
            {
                status = "Error Notify: " + ex.Message;
                Logger.writeLog("Error Notify: " + ex.Message);
                return false;
            }
        }

        public static bool SetDeliveryAsDeliveredStatus(string orderdetid, string username, out string status)
        {
            status = "";
            try
            {
                string sql = @"
                    UPDATE do
                    SET do.IsDelivered = 1, do.IsServerUpdate = 1, do.ModifiedBy = @ModifiedBy, do.ModifiedOn = GETDATE(), do.DeliveredDate = GETDATE() 
                    FROM DeliveryOrderDetails dod
                        INNER JOIN DeliveryOrder do ON do.OrderNumber = dod.DOHDRID
                    WHERE dod.OrderDetID = @OrderDetID
                        AND ISNULL(dod.Deleted, 0) = 0 AND ISNULL(do.Deleted, 0) = 0
                        AND ISNULL(do.IsDelivered, 0) = 0
                 ";
                QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
                cmd.Parameters.Add("@ModifiedBy", username, DbType.String);
                cmd.Parameters.Add("@OrderDetID", orderdetid, DbType.String);
                DataService.ExecuteQuery(cmd);

                return true;
            }
            catch (Exception ex)
            {
                status = "Error Change Delivery Status :" + ex.Message;
                Logger.writeLog("Error Change Delivery Status :" + ex.Message);

                return false;
            }
        }

        public static bool CancelPreOrder(string orderHdrID, string orderDetID, string username, out string status)
        {
            status = "";
            try
            {
                QueryCommandCollection cmdColl = new QueryCommandCollection();
                QueryCommand cmd;

                POSController pos = new POSController(orderHdrID);
                OrderHdr oh = new OrderHdr(orderHdrID);
                OrderDet od = new OrderDet(orderDetID);

                decimal depositBalance = 0;
                if (pos != null && pos.hasPreOrder())
                {
                    OrderDetCollection refOD = pos.myOrderDet;
                    foreach (OrderDet tmpOD in refOD)
                    {
                        if (tmpOD.IsVoided) continue;
                        if (!tmpOD.IsPreOrder.GetValueOrDefault(false)) continue;

                        depositBalance += tmpOD.DepositAmount;
                    }
                }

                // Update OrderDet
                string sql = "UPDATE OrderDet SET IsPreOrder = 0, ModifiedOn = GETDATE(), ModifiedBy = @ModifiedBy WHERE OrderDetID = @OrderDetID";
                cmd = new QueryCommand(sql, "PowerPOS");
                cmd.Parameters.Add("@ModifiedBy", username, DbType.String);
                cmd.Parameters.Add("@OrderDetID", orderDetID, DbType.String);
                cmdColl.Add(cmd);

                // Update OrderHdr
                sql = "UPDATE OrderHdr SET ModifiedOn = GETDATE(), ModifiedBy = @ModifiedBy WHERE OrderHdrID = @OrderHdrID";
                cmd = new QueryCommand(sql, "PowerPOS");
                cmd.Parameters.Add("@ModifiedBy", UserInfo.username, DbType.String);
                cmd.Parameters.Add("@OrderHdrID", orderHdrID, DbType.String);
                cmdColl.Add(cmd);

                //decimal amount = od.Amount;
                //decimal refundDeposit = 0;
                ////recalculate installment and deposit
                //if (oh != null)
                //{
                //    #region calculate installment move to update installment
                //    ReceiptDetCollection myRcptDet = pos.recDet;

                //    Installment ins = new Installment(Installment.Columns.OrderHdrId, oh.OrderHdrID);
                //    if (ins != null && ins.InstallmentRefNo == oh.OrderHdrID)
                //    {
                //        decimal amountToDeduct = ins.CurrentBalance > amount ? amount : ins.CurrentBalance.GetValueOrDefault(0);

                //        ins.CurrentBalance -= Math.Abs(amountToDeduct);
                //        cmdColl.Add(ins.GetUpdateCommand("SYNC"));

                //        string sql2 = "SELECT COUNT(*) FROM InstallmentDetail WHERE InstallmentRefNo = '{0}'";
                //        sql2 = string.Format(sql2, ins.InstallmentRefNo);
                //        int count = 0;
                //        object obj = DataService.ExecuteScalar(new QueryCommand(sql2, "PowerPOS"));
                //        if (obj != null && obj is int)
                //            count = (int)obj;

                //        InstallmentDetail insDet = new InstallmentDetail();
                //        insDet.InstallmentDetRefNo = ins.InstallmentRefNo + "." + count;
                //        insDet.InstallmentRefNo = ins.InstallmentRefNo;
                //        insDet.InstallmentAmount = amountToDeduct;
                //        insDet.OutstandingAmount = ins.CurrentBalance;
                //        insDet.OrderHdrID = pos.myOrderHdr.OrderHdrID;
                //        insDet.CustomRefNo = pos.myOrderHdr.Userfld5;
                //        insDet.Deleted = false;
                //        cmdColl.Add(insDet.GetInsertCommand("SYNC"));
                //    }


                //    #endregion

                //    if (refundDeposit != 0)
                //    {
                //        depositBalance = depositBalance - amount;

                //        if (pos != null && pos.hasPreOrder())
                //        {
                //            OrderDetCollection refOD = pos.myOrderDet;
                //            decimal remainingAmt = depositBalance;

                //            //reset deposit amount
                //            foreach (OrderDet tmpOD in refOD)
                //            {
                //                if (tmpOD.IsVoided) continue;
                //                if (!tmpOD.IsPreOrder.GetValueOrDefault(false)) continue;

                //                tmpOD.DepositAmount = 0;
                //            }

                //            //reasigin deposit amount
                //            if (depositBalance > 0)
                //            {
                //                foreach (OrderDet tmpOD in refOD)
                //                {
                //                    OrderDet odRefund = new OrderDet(tmpOD.OrderDetID);
                //                    if (odRefund.IsVoided) continue;
                //                    if (!odRefund.IsPreOrder.GetValueOrDefault(false)) continue;
                //                    if (odRefund.Amount == null) od.Amount = 0;
                //                    if (odRefund.DepositAmount == null) odRefund.DepositAmount = 0;

                //                    if (odRefund.Amount > 0 && odRefund.Amount > odRefund.DepositAmount)
                //                    {
                //                        decimal discrepancy = odRefund.Amount - odRefund.DepositAmount;
                //                        if (discrepancy <= remainingAmt)
                //                        {
                //                            odRefund.DepositAmount += discrepancy;
                //                            remainingAmt -= discrepancy;
                //                        }
                //                        else
                //                        {
                //                            odRefund.DepositAmount += remainingAmt;
                //                            remainingAmt = 0;
                //                        }

                //                        cmdColl.Add(odRefund.GetUpdateCommand(UserInfo.username));
                //                        if (remainingAmt <= 0) break;
                //                    }
                //                }
                //            }
                //        }
                //    }                    
                //}

                DataService.ExecuteTransaction(cmdColl);

                return true;
            }
            catch (Exception ex)
            {
                status = "Error when cancel Order" + ex.Message;
                Logger.writeLog("Error when cancel Order" + ex.Message);

                return false;
            }
        }

        public static bool ResetDepositAmount(string orderHdrID, out string status)
        {
            status = "";
            try
            {
                POSController pos = new POSController(orderHdrID);

                if (pos != null && string.IsNullOrEmpty(pos.myOrderHdr.OrderType) && pos.hasPreOrder())
                {
                    #region *) Reset Deposit

                    QueryCommandCollection cmdColl = new QueryCommandCollection();
                    OrderDetCollection refOD = pos.myOrderDet;

                    foreach (OrderDet tmpOD in refOD)
                    {
                        OrderDet od = new OrderDet(tmpOD.OrderDetID);
                        if (od.IsVoided) continue;
                        if (!od.IsPreOrder.GetValueOrDefault(false)) continue;

                        od.DepositAmount = 0;

                        cmdColl.Add(od.GetUpdateCommand(UserInfo.username));

                    }

                    //cmdColl.Add(pos.myOrderHdr.GetUpdateCommand(UserInfo.username));


                    DataService.ExecuteTransaction(cmdColl);
                    #endregion
                }
                else
                    throw new Exception("Order doesn't exist");

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Assign Auto Deposit :" + ex.Message);
                status = "Error Assign Auto Deposit :" + ex.Message;
                return false;
            }
        }

        public static bool AssignAutoDeposit(string orderHdrID, out string status)
        {
            status = "";
            try
            {
                POSController pos = new POSController(orderHdrID);

                if (pos == null)
                    throw new Exception("Order didn't exist");

                if (string.IsNullOrEmpty(pos.myOrderHdr.OrderType) && pos.hasPreOrder())
                {
                    #region *) Assign Deposit Amount to OrderDet
                    decimal depositBalance = POSController.GetDepositBalance(pos.myOrderHdr.OrderHdrID, out status);

                    QueryCommandCollection cmdColl = new QueryCommandCollection();
                    OrderDetCollection refOD = pos.myOrderDet;
                    decimal remainingAmt = depositBalance;
                    if (depositBalance > 0)
                    {
                        foreach (OrderDet tmpOD in refOD)
                        {
                            OrderDet od = new OrderDet(tmpOD.OrderDetID);
                            if (od.IsVoided) continue;
                            if (!od.IsPreOrder.GetValueOrDefault(false)) continue;
                            if (od.Amount == null) od.Amount = 0;
                            if (od.DepositAmount == null) od.DepositAmount = 0;

                            if (od.Amount > 0 && od.Amount > od.DepositAmount)
                            {
                                decimal discrepancy = od.Amount - od.DepositAmount;
                                if (discrepancy <= remainingAmt)
                                {
                                    od.DepositAmount += discrepancy;
                                    remainingAmt -= discrepancy;
                                }
                                else
                                {
                                    od.DepositAmount += remainingAmt;
                                    remainingAmt = 0;
                                }

                                cmdColl.Add(od.GetUpdateCommand(UserInfo.username));
                                if (remainingAmt <= 0) break;
                            }
                        }
                        cmdColl.Add(pos.myOrderHdr.GetUpdateCommand(UserInfo.username));
                    }

                    DataService.ExecuteTransaction(cmdColl);
                    #endregion
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Assign Auto Deposit :" + ex.Message);
                status = "Error Assign Auto Deposit :" + ex.Message;
                return false;
            }
        }

        public static bool RefundPreOrder(string orderHdrID, out string status)
        {
            status = "";
            try
            {
                POSController pos = new POSController(orderHdrID);

                if (pos == null)
                    throw new Exception("Order didn't exist");

                if (pos.hasPreOrder())
                {
                    QueryCommandCollection cmdColl = new QueryCommandCollection();

                    QueryCommand cmd;

                    foreach (OrderDet od in pos.myOrderDet)
                    {
                        //check if refund

                        if (od.IsPreOrder.GetValueOrDefault(false) && od.Quantity.GetValueOrDefault(0) < 0)
                        {
                            //update installment
                            string orderhdrref = od.ReturnedReceiptNo;
                            OrderHdr oh = new OrderHdr(OrderHdr.Columns.Userfld5, orderhdrref);

                            POSController posRefund = new POSController(oh.OrderHdrID);

                            decimal depositBalance = 0;
                            if (posRefund != null && posRefund.hasPreOrder())
                            {
                                OrderDetCollection refOD = posRefund.myOrderDet;
                                foreach (OrderDet tmpOD in refOD)
                                {
                                    if (tmpOD.IsVoided) continue;
                                    if (!tmpOD.IsPreOrder.GetValueOrDefault(false)) continue;

                                    depositBalance += tmpOD.DepositAmount;
                                }
                            }

                            // Update OrderDet
                            string sql = "UPDATE OrderDet SET IsPreOrder = 0, ModifiedOn = GETDATE(), ModifiedBy = @ModifiedBy WHERE OrderDetID = @OrderDetID";
                            cmd = new QueryCommand(sql, "PowerPOS");
                            cmd.Parameters.Add("@ModifiedBy", UserInfo.username, DbType.String);
                            cmd.Parameters.Add("@OrderDetID", od.OrderDetID, DbType.String);
                            cmdColl.Add(cmd);

                            // Update OrderDet
                            sql = "UPDATE OrderDet SET IsPreOrder = 0, ModifiedOn = GETDATE(), ModifiedBy = @ModifiedBy WHERE OrderDetID = @OrderDetID";
                            cmd = new QueryCommand(sql, "PowerPOS");
                            cmd.Parameters.Add("@ModifiedBy", UserInfo.username, DbType.String);
                            cmd.Parameters.Add("@OrderDetID", od.RefundOrderDetID, DbType.String);
                            cmdColl.Add(cmd);

                            // Update OrderHdr
                            sql = "UPDATE OrderHdr SET ModifiedOn = GETDATE(), ModifiedBy = @ModifiedBy WHERE OrderHdrID = @OrderHdrID";
                            cmd = new QueryCommand(sql, "PowerPOS");
                            cmd.Parameters.Add("@ModifiedBy", UserInfo.username, DbType.String);
                            cmd.Parameters.Add("@OrderHdrID", od.OrderHdrID, DbType.String);
                            cmdColl.Add(cmd);

                            decimal amount = od.Amount;
                            decimal refundDeposit = 0;
                            //recalculate installment and deposit
                            if (oh != null)
                            {
                                #region calculate installment move to update installment
                                //ReceiptDetCollection myRcptDet = pos.recDet;

                                //for (int i = 0; i < myRcptDet.Count; i++)
                                //{
                                //    if (myRcptDet[i].PaymentType.ToUpper() != POSController.PAY_INSTALLMENT) continue;
                                //    if (myRcptDet[i].PaymentType.ToUpper() == POSController.PAY_INSTALLMENT && myRcptDet[i].Amount >= 0) continue;

                                //    refundDeposit = Math.Abs(amount) - Math.Abs(myRcptDet[i].Amount);

                                //    Installment ins = new Installment(Installment.Columns.OrderHdrId, oh.OrderHdrID);
                                //    if (ins != null && ins.InstallmentRefNo == oh.OrderHdrID)
                                //    {
                                //        ins.CurrentBalance -= Math.Abs(myRcptDet[i].Amount);
                                //        cmdColl.Add(ins.GetUpdateCommand("SYNC"));

                                //        string sql2 = "SELECT COUNT(*) FROM InstallmentDetail WHERE InstallmentRefNo = '{0}'";
                                //        sql2 = string.Format(sql2, ins.InstallmentRefNo);
                                //        int count = 0;
                                //        object obj = DataService.ExecuteScalar(new QueryCommand(sql2, "PowerPOS"));
                                //        if (obj != null && obj is int)
                                //            count = (int)obj;

                                //        InstallmentDetail insDet = new InstallmentDetail();
                                //        insDet.InstallmentDetRefNo = ins.InstallmentRefNo + "." + count;
                                //        insDet.InstallmentRefNo = ins.InstallmentRefNo;
                                //        insDet.InstallmentAmount = myRcptDet[i].Amount;
                                //        insDet.OutstandingAmount = ins.CurrentBalance;
                                //        insDet.OrderHdrID = pos.myOrderHdr.OrderHdrID;
                                //        insDet.CustomRefNo = pos.myOrderHdr.Userfld5;
                                //        insDet.Deleted = false;
                                //        cmdColl.Add(insDet.GetInsertCommand("SYNC"));
                                //    }
                                //}

                                //if (cmdColl.Count > 0)
                                //    DataService.ExecuteTransaction(cmdColl);
                                #endregion

                                if (refundDeposit != 0)
                                {
                                    depositBalance = depositBalance - refundDeposit;

                                    if (posRefund != null && posRefund.hasPreOrder())
                                    {
                                        OrderDetCollection refOD = posRefund.myOrderDet;
                                        decimal remainingAmt = depositBalance;

                                        //reset deposit amount
                                        foreach (OrderDet tmpOD in refOD)
                                        {
                                            if (tmpOD.IsVoided) continue;
                                            if (!tmpOD.IsPreOrder.GetValueOrDefault(false)) continue;

                                            tmpOD.DepositAmount = 0;
                                        }

                                        //reasigin deposit amount
                                        if (depositBalance > 0)
                                        {
                                            foreach (OrderDet tmpOD in refOD)
                                            {
                                                OrderDet odRefund = new OrderDet(tmpOD.OrderDetID);
                                                if (odRefund.IsVoided) continue;
                                                if (!odRefund.IsPreOrder.GetValueOrDefault(false)) continue;
                                                if (odRefund.Amount == null) od.Amount = 0;
                                                if (odRefund.DepositAmount == null) odRefund.DepositAmount = 0;

                                                if (odRefund.Amount > 0 && odRefund.Amount > odRefund.DepositAmount)
                                                {
                                                    decimal discrepancy = odRefund.Amount - odRefund.DepositAmount;
                                                    if (discrepancy <= remainingAmt)
                                                    {
                                                        odRefund.DepositAmount += discrepancy;
                                                        remainingAmt -= discrepancy;
                                                    }
                                                    else
                                                    {
                                                        odRefund.DepositAmount += remainingAmt;
                                                        remainingAmt = 0;
                                                    }

                                                    cmdColl.Add(odRefund.GetUpdateCommand(UserInfo.username));
                                                    if (remainingAmt <= 0) break;
                                                }
                                            }
                                            cmdColl.Add(posRefund.myOrderHdr.GetUpdateCommand(UserInfo.username));
                                        }
                                    }
                                }
                            }

                            if (cmdColl.Count > 0)
                                DataService.ExecuteTransaction(cmdColl);

                            //adjustment IN on the Inventory
                            DeliveryOrderDetail dod = new DeliveryOrderDetail(DeliveryOrderDetail.Columns.OrderDetID, od.OrderDetID);
                            if (!dod.IsNew)
                            {
                                dod.DoAdjustmentIn();
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Refund Pre Order :" + ex.Message);
                status = "Error Refund Pre Order :" + ex.Message;
                return false;
            }
        }
    }
}
