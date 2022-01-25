using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;
using PowerPOS.Container;

namespace PowerPOS
{
    public partial class InstallmentController
    {
        public const string INSTALLMENT_ITEM = "INSTALLMENT";

        public struct Status
        {
            public const string OUTSTANDING = "Outstanding";
            public const string PAID = "Paid";
            public const string FORFEITED = "Forfeited";
            public const string REFUNDED = "Refunded";
        }

        public static DataTable FetchItems(
            string membershipNo,
            string firstName, string lastName, string nameToAppear,
            string membershipGroup, string block, string buildingName,
            string streetName, string unitNo, string city, string country, string zipCode,
            DateTime? dueDate, string sortColumn, string sortDir)
        {
            ViewInstallmentMembershipItemCollection installments = new ViewInstallmentMembershipItemCollection();

            if (!String.IsNullOrEmpty(membershipNo)) { installments.Where(ViewInstallmentMembershipItem.Columns.MembershipNo, membershipNo); }

            if (!String.IsNullOrEmpty(firstName)) { installments.Where(ViewInstallmentMembershipItem.Columns.FirstName, firstName); }
            if (!String.IsNullOrEmpty(lastName)) { installments.Where(ViewInstallmentMembershipItem.Columns.LastName, lastName); }
            if (!String.IsNullOrEmpty(nameToAppear)) { installments.Where(ViewInstallmentMembershipItem.Columns.NameToAppear, nameToAppear); }

            if (!String.IsNullOrEmpty(membershipGroup)) { installments.Where(ViewInstallmentMembershipItem.Columns.GroupName, membershipGroup); }
            if (!String.IsNullOrEmpty(block)) { installments.Where(ViewInstallmentMembershipItem.Columns.Block, block); }
            if (!String.IsNullOrEmpty(buildingName)) { installments.Where(ViewInstallmentMembershipItem.Columns.BuildingName, buildingName); }
            if (!String.IsNullOrEmpty(streetName)) { installments.Where(ViewInstallmentMembershipItem.Columns.StreetName, streetName); }
            if (!String.IsNullOrEmpty(unitNo)) { installments.Where(ViewInstallmentMembershipItem.Columns.UnitNo, unitNo); }
            if (!String.IsNullOrEmpty(city)) { installments.Where(ViewInstallmentMembershipItem.Columns.City, city); }
            if (!String.IsNullOrEmpty(country)) { installments.Where(ViewInstallmentMembershipItem.Columns.Country, country); }
            if (!String.IsNullOrEmpty(zipCode)) { installments.Where(ViewInstallmentMembershipItem.Columns.ZipCode, zipCode); }
            if (dueDate != null) { installments.Where(ViewInstallmentMembershipItem.Columns.DueDate, dueDate); }

            // Sorting.
            SubSonic.TableSchema.TableColumn t = ViewInstallmentMembershipItem.Schema.GetColumn(sortColumn);
            if (t != null)
            {
                if (sortDir.Trim().ToUpper() == "ASC")
                {
                    installments.OrderByAsc(sortColumn);
                }
                else if (sortDir.Trim().ToUpper() == "DESC")
                {
                    installments.OrderByDesc(sortColumn);
                }
            }

            return installments.Load().ToDataTable();
        }

        public static QueryCommand Update
            (string installmentDetRefNo, decimal paymentAmount,
             string modifiedBy, out string status)
        {
            InstallmentDetailCollection installmentDetails = new InstallmentDetailCollection()
                .Where(InstallmentDetail.Columns.InstallmentDetRefNo, installmentDetRefNo).Load();
            if (installmentDetails.Count <= 0)
            {
                status = "Invalid reference number.";
            }

            InstallmentDetail installmentDetail = installmentDetails[0];
            if (paymentAmount > installmentDetail.OutstandingAmount)
            {
                status = "Payment amount is more than outstanding amount";
            }

            decimal? newOutstandingAmount = installmentDetail.OutstandingAmount - paymentAmount;

            string newStatus = installmentDetail.Status;
            if (newOutstandingAmount == 0)
            {
                newStatus = InstallmentController.Status.PAID;
            }
            else
            {
                newStatus = InstallmentController.Status.OUTSTANDING;
            }

            Query qry = new Query("InstallmentDetail");

            qry.AddUpdateSetting(InstallmentDetail.Columns.OutstandingAmount, newOutstandingAmount);
            qry.AddUpdateSetting(InstallmentDetail.Columns.Status, newStatus);

            qry.AddWhere(InstallmentDetail.Columns.InstallmentDetRefNo, installmentDetRefNo);

            status = "";

            return qry.BuildUpdateCommand();
        }

        public static void UpdateInstallmentAmount(string installmentDetRefNo, decimal installmentAmount,
            decimal outstandingAmount, string status)
        {
            Query qry = new Query("InstallmentDetail");

            qry.AddUpdateSetting(InstallmentDetail.Columns.InstallmentAmount, installmentAmount);
            qry.AddUpdateSetting(InstallmentDetail.Columns.OutstandingAmount, outstandingAmount);
            qry.AddUpdateSetting(InstallmentDetail.Columns.Status, status);

            qry.AddWhere(InstallmentDetail.Columns.InstallmentDetRefNo, installmentDetRefNo);
            qry.Execute();
        }

        public static DataTable GetOutstandingInstallmentDetails
            (string membershipNo, string nametoappear, string nric, string installmentRefNo, string orderHdrId,
            bool showOutstandingOnly, string sortColumn, string sortDir)
        {
            ViewInstallmentMembershipItemCollection installments = new ViewInstallmentMembershipItemCollection();
            if (membershipNo != "")
            {
                installments.Where(ViewInstallmentMembershipItem.Columns.MembershipNo, membershipNo);
            }
            if (nametoappear != "")
            {
                installments.Where(ViewInstallmentMembershipItem.Columns.NameToAppear, Comparison.Like, "%" + nametoappear + "%");
            }
            if (nric != "")
            {
                installments.Where(ViewInstallmentMembershipItem.Columns.Nric, Comparison.Like, "%" + nric + "%");
            }
            if (nric != "")
            {
                installments.Where(ViewInstallmentMembershipItem.Columns.InstallmentDetRefNo, Comparison.Like, "%" + installmentRefNo + "%");
            }
            if (orderHdrId != "")
            {
                installments.Where(ViewInstallmentMembershipItem.Columns.OrderHdrId, Comparison.Like, "%" + orderHdrId + "%");
            }

            if (showOutstandingOnly)
            {
                installments.Where(ViewInstallmentMembershipItem.Columns.DueDate, SubSonic.Comparison.LessThan, DateTime.Today);
            }

            installments.Where(ViewInstallmentMembershipItem.Columns.OutstandingAmount, SubSonic.Comparison.GreaterThan, 0);

            // Sorting.
            SubSonic.TableSchema.TableColumn t = ViewInstallmentMembershipItem.Schema.GetColumn(sortColumn);
            if (t != null)
            {
                if (sortDir.Trim().ToUpper() == "ASC")
                {
                    installments.OrderByAsc(sortColumn);
                }
                else if (sortDir.Trim().ToUpper() == "DESC")
                {
                    installments.OrderByDesc(sortColumn);
                }
            }

            return installments.Load().ToDataTable();
        }

        public static bool CreateInstallment(
            Installment instHeader,
            InstallmentDetailCollection instDetail)
        {
            try
            {
                instHeader.InstallmentCreatedDate = DateTime.Now;

                //validations
                //what to validate?


                //ensure they link
                instHeader.InstallmentRefNo =
                    UtilityController.CreateNewGenericHdrRefNo
                    ("Installment", "InstallmentRefNo", PointOfSaleInfo.PointOfSaleID);
                instHeader.Deleted = false;
                //save together using query transaction
                QueryCommandCollection cmd = new QueryCommandCollection();
                cmd.Add(instHeader.GetInsertCommand(UserInfo.username));

                for (int i = 0; i < instDetail.Count; i++)
                {
                    instDetail[i].InstallmentRefNo = instHeader.InstallmentRefNo;
                    instDetail[i].InstallmentDetRefNo = instHeader.InstallmentRefNo + "." + i.ToString();
                    instDetail[i].Status = Status.OUTSTANDING;
                    instDetail[i].Deleted = false;
                    cmd.Add(instDetail[i].GetInsertCommand(UserInfo.username));
                }

                DataService.ExecuteTransaction(cmd);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool UpdateInstallment(
            Installment instHeader,
            InstallmentDetailCollection instDetail)
        {
            try
            {
                instHeader.InstallmentCreatedDate = DateTime.Now;

                //validations
                //what to validate?


                //ensure they link


                //save together using query transaction
                QueryCommandCollection cmd = new QueryCommandCollection();
                cmd.Add(instHeader.GetUpdateCommand(UserInfo.username));
                DataService.ExecuteTransaction(cmd);


                cmd = new QueryCommandCollection();
                Query qr = InstallmentDetail.CreateQuery();
                qr.QueryType = QueryType.Delete;
                qr.AddWhere(InstallmentDetail.Columns.InstallmentRefNo, instHeader.InstallmentRefNo);
                cmd.Add(qr.BuildDeleteCommand());
                DataService.ExecuteTransaction(cmd);
                for (int i = 0; i < instDetail.Count; i++)
                {
                    instDetail[i].InstallmentRefNo = instHeader.InstallmentRefNo;
                    instDetail[i].InstallmentDetRefNo = instHeader.InstallmentRefNo + "." + i.ToString();

                    cmd.Add(instDetail[i].GetInsertCommand(UserInfo.username));
                }

                DataService.ExecuteTransaction(cmd);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        //public static bool UpdateInsatllmentServerByOrderHdr(string OrderHdrId, out string status)
        //{
        //    //hasInstallment = pos.hasPaymentType(POSController.PAY_INSTALLMENT);
        //    POSController pos = new POSController(orderHdrID);

        //    OrderHdr OrderHdrTemp = new OrderHdr("OrderHdrID", orderHdrID);
        //    InstallmentCollection InstallmentRec = OrderHdrTemp.InstallmentRecords().Load();

        //    if (InstallmentRec.Count>0 )
        //    {
        //        //edit installment
        //        Installment inst = new Installment("userfld1", OrderHdrId);
        //        InstallmentDetailCollection instColl = inst.InstallmentDetailRecords();
        //        ReceiptHdrCollection receiptHdrColl = OrderHdrTemp.ReceiptHdrRecords().Load();

        //        Query qr = new Query("ReceiptDet");
        //        qr.AddWhere(ReceiptDet.Columns.ReceiptHDrId,OrderHdrId);
        //        ReceiptDetCollection myRcptDet = new ReceiptDetController().FetchByQuery(qr).All();

        //        decimal rcpDetInstAmount = 0;
        //        for (int i = 0; i < myRcptDet.Count; i++)
        //        {
        //            if (myRcptDet[i].PaymentType.ToUpper() != POSController.PAY_INSTALLMENT) continue;
        //            rcpDetInstAmount = myRcptDet[i].Amount;
        //        }

        //        decimal? totalInstallmentPaid = 0;
        //        foreach (var instDetail in instColl)
        //        {
        //            if (instDetail.InstallmentAmount < 0)
        //                totalInstallmentPaid = totalInstallmentPaid + (instDetail.InstallmentAmount * -1);
        //            else
        //                instDetail.InstallmentAmount = rcpDetInstAmount;
        //        }

        //        if (rcpDetInstAmount < totalInstallmentPaid)
        //        {
        //            MessageBox.Show("Cannot change payment type, Installment Paid greater than new Installment Amount", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        else
        //        {
        //            decimal lastTotalAmount = inst.TotalAmount;
        //            inst.CurrentBalance = rcpDetInstAmount - totalInstallmentPaid;
        //            inst.TotalAmount = rcpDetInstAmount;
        //            //inst.ModifiedOn = DateTime.Now;
        //            foreach (var instDet in instColl)
        //            {
        //                instDet.OutstandingAmount = instDet.OutstandingAmount - (lastTotalAmount - rcpDetInstAmount);
        //            }
        //        }
        //        InstallmentController.UpdateInstallment(inst, instColl);
        //    }


        //    pos.SavePaymentTypes();

        //    if (hasInstallment &&
        //        !pos.hasPaymentType(POSController.PAY_INSTALLMENT))
        //    {
        //        Installment.Delete(Installment.Columns.OrderHdrId, pos.GetSavedRefNo().Substring(2));
        //    }

        //    //If installment changed
        //    if (hasInstallment)
        //    {
        //        //Installment inst = new Installment("userfld1", pos.GetSavedRefNo());
        //        //InstallmentDetailCollection instDetail = new InstallmentDetail("InstallmentRefNo", inst.InstallmentRefNo);

        //        //InstallmentController.UpdateInstallment(
        //        string errMsg = "";
        //        InstallmentController.UpdateInstallmentByOrderHdr(pos.GetSavedRefNo().Substring(2), out errMsg);
        //    }
        //}
        public static bool UpdateInsatllmentServerByOrderHdr(string OrderHdrId, out string status)
        {
            bool hasInstallment;
            POSController pos = new POSController(OrderHdrId);
            hasInstallment = pos.hasPaymentType(POSController.PAY_INSTALLMENT);
            if (hasInstallment && pos.hasPaymentType(POSController.PAY_INSTALLMENT))
            {
                //edit installment
                Installment inst = new Installment("userfld1", pos.GetSavedRefNo());
                InstallmentDetailCollection instColl = inst.InstallmentDetailRecords();
                ReceiptDetCollection myRcptDet = pos.recDet;
                decimal rcpDetInstAmount = 0;
                for (int i = 0; i < myRcptDet.Count; i++)
                {
                    if (myRcptDet[i].PaymentType.ToUpper() != POSController.PAY_INSTALLMENT) continue;
                    rcpDetInstAmount = myRcptDet[i].Amount;
                }

                decimal? totalInstallmentPaid = 0;
                foreach (var instDetail in instColl)
                {
                    if (instDetail.InstallmentAmount < 0)
                        totalInstallmentPaid = totalInstallmentPaid + (instDetail.InstallmentAmount * -1);
                    else
                        instDetail.InstallmentAmount = rcpDetInstAmount;
                }

                if (rcpDetInstAmount < totalInstallmentPaid)
                {
                    Logger.writeLog("Cannot change payment type, Installment Paid greater than new Installment Amount");
                }
                else
                {
                    decimal lastTotalAmount = inst.TotalAmount;
                    inst.CurrentBalance = rcpDetInstAmount - totalInstallmentPaid;
                    inst.TotalAmount = rcpDetInstAmount;
                    //inst.ModifiedOn = DateTime.Now;
                    foreach (var instDet in instColl)
                    {
                        instDet.OutstandingAmount = instDet.OutstandingAmount - (lastTotalAmount - rcpDetInstAmount);
                    }
                }
                InstallmentController.UpdateInstallment(inst, instColl);
            }


            pos.SavePaymentTypes();

            if (hasInstallment &&
                !pos.hasPaymentType(POSController.PAY_INSTALLMENT))
            {
                Installment.Delete(Installment.Columns.OrderHdrId, pos.GetSavedRefNo().Substring(2));
            }

            //If installment changed
            if (hasInstallment)
            {
                //Installment inst = new Installment("userfld1", pos.GetSavedRefNo());
                //InstallmentDetailCollection instDetail = new InstallmentDetail("InstallmentRefNo", inst.InstallmentRefNo);

                //InstallmentController.UpdateInstallment(
                string errMsg = "";
                InstallmentController.UpdateInstallmentByOrderHdr(pos.GetSavedRefNo().Substring(2), out errMsg);
            }
            status = "";
            return true;
        }
        public static bool UpdateInstallmentByOrderHdr(string orderHdrID, out string status)
        {
            status = "";
            try
            {
                POSController pos = new POSController(orderHdrID);
                if (pos != null && pos.GetUnsavedCustomRefNo() != "" && !pos.IsVoided())
                {
                    Installment instExisting = new Installment(Installment.Columns.OrderHdrId, orderHdrID);

                    bool hasInstallment = false;

                    if (!instExisting.IsNew)
                        hasInstallment = true;

                    QueryCommandCollection cmdColl = new QueryCommandCollection();
                    OrderDetCollection myOrderDet = pos.myOrderDet;
                    ReceiptDetCollection myRcptDet = pos.recDet;

                    // edit payment edit amount or delete, delete all installment 
                    if (hasInstallment && !pos.HasRefundInstallment())
                    {
                        QueryCommandCollection cmdDelete = new QueryCommandCollection();
                        Query qr = InstallmentDetail.CreateQuery();
                        qr.QueryType = QueryType.Delete;
                        qr.AddWhere(InstallmentDetail.Columns.InstallmentRefNo, instExisting.InstallmentRefNo);
                        cmdDelete.Add(qr.BuildDeleteCommand());

                        Query qr2 = Installment.CreateQuery();
                        qr2.QueryType = QueryType.Delete;
                        qr2.AddWhere(Installment.Columns.InstallmentRefNo, instExisting.InstallmentRefNo);
                        cmdDelete.Add(qr2.BuildDeleteCommand());
                        
                        DataService.ExecuteTransaction(cmdDelete);                        
                    }
                    
                    #region *) If Installment record exists then skip
                    InstallmentDetail tmpdet = new InstallmentDetail("OrderHdrID", orderHdrID);
                    if (tmpdet != null && tmpdet.OrderHdrID == orderHdrID) return true;
                    #endregion
                    
                    for (int i = 0; i < myRcptDet.Count; i++)
                    {
                        if (myRcptDet[i].PaymentType.ToUpper() != POSController.PAY_INSTALLMENT) continue;
                        if (myRcptDet[i].PaymentType.ToUpper() == POSController.PAY_INSTALLMENT && myRcptDet[i].Amount < 0)
                        {
                            List<string> refno = new List<string>();

                            for (int j = 0; j < myOrderDet.Count; j++)
                            {
                                // for payment refund and credit note
                                if (myOrderDet[j].IsVoided) continue;
                                if (myOrderDet[j].ItemNo != "CREDIT_NOTE" && string.IsNullOrEmpty(myOrderDet[j].ReturnedReceiptNo)) continue;
                                if (myOrderDet[j].ItemNo == "CREDIT_NOTE" && string.IsNullOrEmpty(myOrderDet[j].InstRefNo)) continue;

                                string refind = "";

                                if (myOrderDet[j].ItemNo == "CREDIT_NOTE" && !string.IsNullOrEmpty(myOrderDet[j].InstRefNo))
                                {
                                    refind = myOrderDet[j].InstRefNo;
                                }
                                else if (myOrderDet[j].ItemNo != "CREDIT_NOTE" && !string.IsNullOrEmpty(myOrderDet[j].ReturnedReceiptNo))
                                {
                                    OrderHdr oh = new OrderHdr(OrderHdr.Columns.Userfld5, myOrderDet[j].ReturnedReceiptNo);
                                    if (!oh.IsNew)
                                        refind = oh.OrderHdrID;
                                    else
                                    {
                                        OrderHdr oh2 = new OrderHdr(OrderHdr.Columns.OrderRefNo, myOrderDet[j].ReturnedReceiptNo);
                                        if (!oh2.IsNew)
                                            refind = oh2.OrderHdrID;
                                    }
                                }

                                if (!string.IsNullOrEmpty(refind))
                                {
                                    var f = (from r in refno where r.Equals(refind) select r).Count();

                                    if (f == 0)
                                        refno.Add(refind);
                                }
                            }

                            if (refno.Count > 0)
                            {
                                foreach (string it in refno)
                                {
                                    // Installment Payment ==> Update the installment balance and create installment details
                                    Installment ins = new Installment(it);
                                    if (ins != null && ins.InstallmentRefNo == it)
                                    {
                                        ins.CurrentBalance -= Math.Abs(myRcptDet[i].Amount);
                                        cmdColl.Add(ins.GetUpdateCommand("SYNC"));

                                        string sql = "SELECT COUNT(*) FROM InstallmentDetail WHERE InstallmentRefNo = '{0}'";
                                        sql = string.Format(sql, ins.InstallmentRefNo);
                                        int count = 0;
                                        object obj = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));
                                        if (obj != null && obj is int)
                                            count = (int)obj;

                                        InstallmentDetail insDet = new InstallmentDetail();
                                        insDet.InstallmentDetRefNo = ins.InstallmentRefNo + "." + count;
                                        insDet.InstallmentRefNo = ins.InstallmentRefNo;
                                        insDet.InstallmentAmount = -Math.Abs(myRcptDet[i].Amount);
                                        insDet.OutstandingAmount = ins.CurrentBalance;
                                        insDet.OrderHdrID = pos.myOrderHdr.OrderHdrID;
                                        insDet.CustomRefNo = pos.myOrderHdr.Userfld5;
                                        insDet.Deleted = false;
                                        cmdColl.Add(insDet.GetInsertCommand("SYNC"));
                                    }
                                }
                            }
                        }
                        else
                        {

                            // Paid by INSTALLMENT, insert to Installment and InstallmentDetail
                            Installment ins = new Installment();
                            ins.InstallmentRefNo = pos.myOrderHdr.OrderHdrID;
                            ins.OrderHdrId = pos.myOrderHdr.OrderHdrID;
                            ins.MembershipNo = pos.myOrderHdr.MembershipNo;
                            ins.InstallmentCreatedDate = pos.myOrderHdr.OrderDate;
                            ins.TotalAmount = myRcptDet[i].Amount;
                            ins.CurrentBalance = myRcptDet[i].Amount;
                            ins.CustomRefNo = pos.myOrderHdr.Userfld5;
                            ins.Deleted = false;
                            cmdColl.Add(ins.GetInsertCommand("SYNC"));

                            InstallmentDetail insDet = new InstallmentDetail();
                            insDet.InstallmentDetRefNo = ins.InstallmentRefNo + ".0";
                            insDet.InstallmentRefNo = ins.InstallmentRefNo;
                            insDet.InstallmentAmount = myRcptDet[i].Amount;
                            insDet.OutstandingAmount = ins.TotalAmount;
                            insDet.OrderHdrID = pos.myOrderHdr.OrderHdrID;
                            insDet.CustomRefNo = pos.myOrderHdr.Userfld5;
                            insDet.Deleted = false;
                            cmdColl.Add(insDet.GetInsertCommand("SYNC"));
                        }
                    }

                    for (int i = 0; i < myOrderDet.Count; i++)
                    {
                        if (myOrderDet[i].IsVoided) continue;
                        if (myOrderDet[i].ItemNo.ToUpper() != "INST_PAYMENT") continue;
                        if (string.IsNullOrEmpty(myOrderDet[i].InstRefNo)) continue;

                        // Installment Payment ==> Update the installment balance and create installment details
                        if (myOrderDet[i].InstRefNo.Contains(","))
                        {
                            Logger.writeLog("LOG Test " + myOrderDet[i].InstRefNo);
                            var allOrderHdr = myOrderDet[i].InstRefNo.Split(',').ToList();
                            foreach (var instRef in allOrderHdr)
                            {
                                Installment ins = new Installment(instRef);
                                if (ins != null && ins.InstallmentRefNo == instRef)
                                {
                                    var instDetAmount = -ins.CurrentBalance;
                                    ins.CurrentBalance = 0;
                                    cmdColl.Add(ins.GetUpdateCommand("SYNC"));

                                    string sql = "SELECT COUNT(*) FROM InstallmentDetail WHERE InstallmentRefNo = '{0}'";
                                    sql = string.Format(sql, ins.InstallmentRefNo);
                                    int count = 0;
                                    object obj = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));
                                    if (obj != null && obj is int)
                                        count = (int)obj;

                                    InstallmentDetail insDet = new InstallmentDetail();
                                    insDet.InstallmentDetRefNo = ins.InstallmentRefNo + "." + count;
                                    insDet.InstallmentRefNo = ins.InstallmentRefNo;
                                    insDet.InstallmentAmount = instDetAmount;
                                    insDet.OutstandingAmount = ins.CurrentBalance;
                                    insDet.OrderHdrID = pos.myOrderHdr.OrderHdrID;
                                    insDet.CustomRefNo = pos.myOrderHdr.Userfld5;
                                    insDet.Deleted = false;
                                    cmdColl.Add(insDet.GetInsertCommand("SYNC"));
                                }
                            }
                        }
                        else
                        {
                            Installment ins = new Installment(myOrderDet[i].InstRefNo);
                            if (ins != null && ins.InstallmentRefNo == myOrderDet[i].InstRefNo)
                            {
                                ins.CurrentBalance -= myOrderDet[i].Amount;
                                cmdColl.Add(ins.GetUpdateCommand("SYNC"));

                                string sql = "SELECT COUNT(*) FROM InstallmentDetail WHERE InstallmentRefNo = '{0}'";
                                sql = string.Format(sql, ins.InstallmentRefNo);
                                int count = 0;
                                object obj = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));
                                if (obj != null && obj is int)
                                    count = (int)obj;

                                InstallmentDetail insDet = new InstallmentDetail();
                                insDet.InstallmentDetRefNo = ins.InstallmentRefNo + "." + count;
                                insDet.InstallmentRefNo = ins.InstallmentRefNo;
                                insDet.InstallmentAmount = -myOrderDet[i].Amount;
                                insDet.OutstandingAmount = ins.CurrentBalance;
                                insDet.OrderHdrID = pos.myOrderHdr.OrderHdrID;
                                insDet.CustomRefNo = pos.myOrderHdr.Userfld5;
                                insDet.Deleted = false;
                                cmdColl.Add(insDet.GetInsertCommand("SYNC"));
                            }
                        }
                    }
                    

                    if (cmdColl.Count > 0) DataService.ExecuteTransaction(cmdColl);
                }
                else if (pos != null && pos.GetUnsavedCustomRefNo() != "" && pos.IsVoided())
                {
                    //revert Installment

                    QueryCommandCollection cmdColl = new QueryCommandCollection();
                    OrderDetCollection myOrderDet = pos.myOrderDet;
                    ReceiptDetCollection myRcptDet = pos.recDet;

                    for (int i = 0; i < myRcptDet.Count; i++)
                    {
                        if (myRcptDet[i].PaymentType.ToUpper() != POSController.PAY_INSTALLMENT) continue;
                        if (myRcptDet[i].PaymentType.ToUpper() == POSController.PAY_INSTALLMENT && myRcptDet[i].Amount < 0)
                        {
                            Query qr = InstallmentDetail.CreateQuery();
                            qr.AddWhere(InstallmentDetail.Columns.OrderHdrID, pos.myOrderHdr.OrderHdrID);

                            InstallmentDetailCollection insDetColl = new InstallmentDetailCollection();
                            insDetColl.Load(qr.ExecuteReader());
                            foreach (InstallmentDetail insDet in insDetColl)
                            {
                                insDet.Deleted = true;
                                cmdColl.Add(insDet.GetUpdateCommand("SYNC"));

                                Installment ins = new Installment(insDet.InstallmentRefNo);
                                if (!ins.IsNew)
                                {
                                    ins.CurrentBalance += insDet.InstallmentAmount;
                                    cmdColl.Add(ins.GetUpdateCommand("SYNC"));
                                }
                            }


                        }
                        else
                        {

                            // Paid by INSTALLMENT, void the Installment and InstallmentDetail (set Deleted = 1)
                            Installment ins = new Installment("OrderHdrID", pos.myOrderHdr.OrderHdrID);
                            if (ins != null && ins.OrderHdrId == pos.myOrderHdr.OrderHdrID)
                            {
                                ins.Deleted = true;
                                cmdColl.Add(ins.GetUpdateCommand("SYNC"));
                            }

                            InstallmentDetailCollection insDetColl = ins.InstallmentDetailRecords();
                            foreach (InstallmentDetail insDet in insDetColl)
                            {
                                insDet.Deleted = true;
                                cmdColl.Add(insDet.GetUpdateCommand("SYNC"));
                            }
                        }
                    }

                    for (int i = 0; i < myOrderDet.Count; i++)
                    {
                        if (myOrderDet[i].IsVoided) continue;
                        if (myOrderDet[i].ItemNo.ToUpper() != "INST_PAYMENT" && myOrderDet[i].ItemNo.ToUpper() != Installment.CreditNote) continue;
                        if (string.IsNullOrEmpty(myOrderDet[i].InstRefNo)) continue;

                        // Installment Payment ==> Update the installment balance and void installment details
                        if (myOrderDet[i].InstRefNo.Contains(","))
                        {
                            Logger.writeLog("LOG Test " + myOrderDet[i].InstRefNo);
                            var allOrderHdr = myOrderDet[i].InstRefNo.Split(',').ToList();

                            foreach (var instRef in allOrderHdr)
                            {
                                Installment ins = new Installment(instRef);
                                if (ins != null && ins.InstallmentRefNo == instRef)
                                {


                                    InstallmentDetailCollection insDetColl = new InstallmentDetailCollection();
                                    insDetColl.Where(InstallmentDetail.Columns.InstallmentRefNo, instRef);
                                    insDetColl.Where(InstallmentDetail.Columns.Deleted, 0);
                                    insDetColl.Where(InstallmentDetail.Columns.OrderHdrID, SubSonic.Comparison.Equals, pos.myOrderHdr.OrderHdrID);
                                    insDetColl.Load();

                                    if (insDetColl != null)
                                    {
                                        InstallmentDetail insDet = insDetColl[0];
                                        insDet.Deleted = true;
                                        cmdColl.Add(insDet.GetUpdateCommand("SYNC"));
                                    }

                                    InstallmentDetailCollection instDet = new InstallmentDetailCollection();
                                    instDet.Where(InstallmentDetail.Columns.InstallmentRefNo, instRef);
                                    instDet.Where(InstallmentDetail.Columns.Deleted, 0);
                                    instDet.Where(InstallmentDetail.Columns.OrderHdrID, SubSonic.Comparison.NotEquals, pos.myOrderHdr.OrderHdrID);
                                    instDet.OrderByDesc(InstallmentDetail.Columns.InstallmentDetRefNo);
                                    instDet.Load();

                                    ins.CurrentBalance = instDet[0].OutstandingAmount;
                                    cmdColl.Add(ins.GetUpdateCommand("SYNC"));
                                }
                            }
                        }
                        else
                        {

                            Installment ins = new Installment(myOrderDet[i].InstRefNo);
                            if (ins != null && ins.InstallmentRefNo == myOrderDet[i].InstRefNo)
                            {
                                ins.CurrentBalance += myOrderDet[i].Amount;
                                cmdColl.Add(ins.GetUpdateCommand("SYNC"));

                                InstallmentDetail insDet = new InstallmentDetail("OrderHdrID", pos.myOrderHdr.OrderHdrID);
                                if (insDet != null && insDet.OrderHdrID == pos.myOrderHdr.OrderHdrID)
                                {
                                    insDet.Deleted = true;
                                    cmdColl.Add(insDet.GetUpdateCommand("SYNC"));
                                }
                            }
                        }
                    }

                    if (cmdColl.Count > 0) DataService.ExecuteTransaction(cmdColl);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Update Installment Balance failed for OrderHdrID " + orderHdrID);
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }


        public static DataSet GetInstallmentListByCustomer(string MembershipNo, bool ShowOutstandingOnly, out string status)
        {
            status = "";
            DataSet ds = new DataSet();

            try
            {
                //                string sql = @"
                //                                SELECT InstallmentRefNo, MembershipNo, 
                //                                    OrderHdrId, userfld1 as [CustomReceiptNo], 
                //                                    InstallmentCreatedDate, 
                //                                    CurrentBalance, TotalAmount, isnull(userfld1, '') as PaymentTerm ,
                //                                    (TotalAmount - CurrentBalance) as PaidAmount 
                //                                FROM Installment 
                //                                WHERE MembershipNo = '{0}' AND ISNULL(Deleted, 0) = 0
                //                              ";

                string sql = @"
                                SELECT Ins.InstallmentRefNo, Ins.MembershipNo, 
                                    Ins.OrderHdrId, Ins.userfld1 as [CustomReceiptNo], 
                                    Ins.InstallmentCreatedDate, 
                                    Ins.CurrentBalance, Ins.TotalAmount, isnull(rd.userfld1, '') as PaymentTerm ,
                                    (Ins.TotalAmount - Ins.CurrentBalance) as PaidAmount  
                                FROM Installment Ins 
								left join orderhdr oh on Ins.Orderhdrid = oh.OrderHdrID 
								left join ReceiptHdr rh on oh.OrderHdrID = rh.OrderHdrID 
								left join receiptdet rd on rh.ReceiptHdrID = rd.ReceiptHdrID and rd.PaymentType='INSTALLMENT' 
                                WHERE ins.MembershipNo = '{0}' AND ISNULL(Deleted, 0) = 0 
                    ";
                sql = string.Format(sql, MembershipNo);

                if (ShowOutstandingOnly)
                    sql += " AND CurrentBalance > 0 ";

                ds = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS"));
            }
            catch (Exception ex)
            {
                Logger.writeLog("Get Installment List failed for MembershipNo " + MembershipNo);
                Logger.writeLog(ex);
                status = ex.Message;
            }

            return ds;
        }

        public static DataSet GetInstallmentDetailByOrderHdrID(string MembershipNo, string OrderHdrID, bool ShowAllTransactions, out string status)
        {
            status = "";
            DataSet ds = new DataSet();

            try
            {
                string sql = @"
                                DECLARE @membershipNo varchar(50) 
                                SET @membershipNo = '{0}' 

                                SELECT id.OrderHdrID AS ReceiptNo, 
                                       id.OrderHdrID AS OrderRefNo, 
                                       id.userfld1 AS CustomOrderRefNo, 
                                       ih.OrderHdrID AS PaymentFor, 
                                       ih.OrderHdrID AS PaymentRefNo, 
                                       ih.userfld1 AS CustomPaymentRefNo, 
                                       id.CreatedOn AS ReceiptDate, 
                                       CASE WHEN id.InstallmentAmount > 0 THEN id.InstallmentAmount ELSE 0 END Credit, 
                                       CASE WHEN id.InstallmentAmount < 0 THEN -id.InstallmentAmount ELSE 0 END Debit 
                                FROM Installment ih 
                                    INNER JOIN InstallmentDetail id ON id.InstallmentRefNo = ih.InstallmentRefNo 
                                WHERE ih.MembershipNo = @membershipNo 
                                    AND ISNULL(ih.Deleted, 0) = 0 AND ISNULL(id.Deleted, 0) = 0 
                                    {1}
                                ORDER BY id.OrderHdrID
                              ";

                string whr = "AND ih.OrderHdrID = '" + OrderHdrID + "' ";

                if (ShowAllTransactions)
                    sql = string.Format(sql, MembershipNo, "");
                else
                    sql = string.Format(sql, MembershipNo, whr);

                ds = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS"));
            }
            catch (Exception ex)
            {
                Logger.writeLog("Get Installment Detail failed for MembershipNo " + MembershipNo);
                Logger.writeLog(ex);
                status = ex.Message;
            }

            return ds;
        }
    }
}
