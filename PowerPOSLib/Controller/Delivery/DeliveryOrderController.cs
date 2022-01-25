using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using PowerPOS;
using SubSonic;
using System.Transactions;
using PowerPOS.Container;
namespace PowerPOS
{
    partial class DeliveryOrderController 
    {
        //public static string CreateNewOrderNo(int PointOfSaleID) // Unused, duplicate of DeliveryController.CreateNewDeliveryNo
        //{
        //    int runningNo = 0;

        //    IDataReader ds = PowerPOS.SPs.GetNewDeliveryOrderHdrNoByPointOfSaleID(PointOfSaleID).GetReader();
        //    while (ds.Read())
        //    {
        //        if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
        //        {
        //            runningNo = 0;
        //        }
        //    }
        //    ds.Close();
        //    runningNo += 1;

        //    //YYMMDDSSSSNNNN                
        //    //YY - year
        //    //MM - month
        //    //DD - day
        //    //SSSS - PointOfSale ID
        //    //NNNN - Running No
        //    return DateTime.Now.ToString("yyMMdd") + PointOfSaleID.ToString().PadLeft(4, '0') + runningNo.ToString().PadLeft(4, '0');
        //}
        public static void CheckTimeSlotAvailable(int personAssigned, DateTime timeSlotFrom)
        {
            IDataReader dt1 = PowerPOS.DeliveryOrder.FetchByParameter(Constants.ORDER_PERSON_ASSIGNED, personAssigned);
            DataTable dt = new DataTable();
            dt.Load(dt1);
            dt1.Close();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (timeSlotFrom >= Convert.ToDateTime(dr[Constants.ORDER_TIME_SLOT_FROM]) &&
                        timeSlotFrom < Convert.ToDateTime(dr[Constants.ORDER_TIME_SLOT_TO]))
                        throw new System.ApplicationException(Constants.SLOT_UNAVAILABLE);
                }
            }
        }
        public static void CheckTimeSlotAvailable(int personAssigned, DateTime timeSlotFrom, string DOHDRID)
        {
            IDataReader dt1 = PowerPOS.DeliveryOrder.FetchByParameter(Constants.ORDER_PERSON_ASSIGNED, personAssigned);
            DataTable dt = new DataTable();
            dt.Load(dt1);
            dt1.Close();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (timeSlotFrom >= Convert.ToDateTime(dr[Constants.ORDER_TIME_SLOT_FROM]) &&
                        timeSlotFrom < Convert.ToDateTime(dr[Constants.ORDER_TIME_SLOT_TO]) &&
                        dr["OrderNumber"].ToString() != DOHDRID)
                            throw new System.ApplicationException(Constants.SLOT_UNAVAILABLE);
                }
            }
        }

        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        public static void SaveOrder(DeliveryOrder delOrder, DeliveryOrderDetailCollection delOrderDetailCol)
        {
            try
            {

                //Here all the business rules can be written......

                //Business Rule 1
                //Create the slot date time using the Delievery date and slot time 
                //DateTime varTimeSlotFrom = new DateTime(delOrder.DeliveryDate.Value.Year, delOrder.DeliveryDate.Value.Month, delOrder.DeliveryDate.Value.Day, delOrder.TimeSlotFrom.Value.Hour, delOrder.TimeSlotFrom.Value.Minute, 0);
                //DateTime varTimeSlotTo = new DateTime(delOrder.DeliveryDate.Value.Year, delOrder.DeliveryDate.Value.Month, delOrder.DeliveryDate.Value.Day, delOrder.TimeSlotTo.Value.Hour, delOrder.TimeSlotTo.Value.Minute, 0);

                //Check whether the slot is available for the pseronnel delivery to be assigned
                //CheckTimeSlotAvailable(delOrder.PersonAssigned.Value, varTimeSlotFrom);
                //Business Rule 1

                QueryCommandCollection cmd = new QueryCommandCollection();
                delOrder.UniqueID = Guid.NewGuid();
                delOrder.IsServerUpdate = true;
                cmd.Add(delOrder.GetInsertCommand(UserInfo.username));
                for (int i = 0; i < delOrderDetailCol.Count; i++)
                {
                    delOrderDetailCol[i].UniqueID = Guid.NewGuid();
                    cmd.Add(delOrderDetailCol[i].GetInsertCommand(UserInfo.username));
                }
                DataService.ExecuteTransaction(cmd);

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.DO_UseCustomNo), false))
                {
                    CustNumUpdate();
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number.Equals(Constants.PRIMARY_KEY_VOILATION))
                {
                    throw new System.ApplicationException(Constants.ORDERNUMBER_ALREADY_EXISTS);
                }
                else
                    throw new System.ApplicationException(sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        public static void UpdateOrder(DeliveryOrder delOrder, DeliveryOrderDetailCollection delOrderDetailCol)
        {
            try
            {

                //DateTime varTimeSlotFrom = new DateTime(delOrder.DeliveryDate.Value.Year, delOrder.DeliveryDate.Value.Month, delOrder.DeliveryDate.Value.Day, delOrder.TimeSlotFrom.Value.Hour, delOrder.TimeSlotFrom.Value.Minute, 0);
                //DateTime varTimeSlotTo = new DateTime(delOrder.DeliveryDate.Value.Year, delOrder.DeliveryDate.Value.Month, delOrder.DeliveryDate.Value.Day, delOrder.TimeSlotTo.Value.Hour, delOrder.TimeSlotTo.Value.Minute, 0);
                //CheckTimeSlotAvailable(delOrder.PersonAssigned.Value, varTimeSlotFrom,delOrder.OrderNumber);

                delOrder.IsServerUpdate = true;
                QueryCommandCollection cmd = new QueryCommandCollection();
                cmd.Add(delOrder.GetUpdateCommand(UserInfo.username));

                for (int i = 0; i < delOrderDetailCol.Count; i++)
                {
                    //Create Query to update all
                    cmd.Add(delOrderDetailCol[i].GetUpdateCommand(UserInfo.username));
                }

                DataService.ExecuteTransaction(cmd);
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number.Equals(Constants.PRIMARY_KEY_VOILATION))
                {
                    throw new System.ApplicationException(Constants.ORDERNUMBER_ALREADY_EXISTS);
                }
                else
                    throw new System.ApplicationException(sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException(ex.Message);
            }
        }
        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        public static void UnblockOrder(DeliveryOrder delOrder)
        {
            try
            {
                //Set only the date and remove the time to unblock the slot
                delOrder.TimeSlotFrom = delOrder.DeliveryDate.Value.Date;
                delOrder.TimeSlotTo = delOrder.DeliveryDate.Value.Date;
                delOrder.Save();

                //update using the Order Controller
                //DeliveryOrderController dvc = new DeliveryOrderController();
                //dvc.Update(delOrder.OrderNumber, delOrder.DeliveryDate, delOrder.PersonAssigned, delOrder.MemberId,
                //            delOrder.TimeSlotFrom, delOrder.TimeSlotTo, delOrder.Status);
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number.Equals(Constants.PRIMARY_KEY_VOILATION))
                {
                    throw new System.ApplicationException(Constants.ORDERNUMBER_ALREADY_EXISTS);
                }
                else
                    throw new System.ApplicationException(sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Save multiple Delivery Order
        /// </summary>
        public static void SaveMultipleOrder(ref DeliveryOrderCollection delOrderHdrColl, ref DeliveryOrderDetailCollection delOrderDetColl)
        {
            try
            {
                DeliveryOrderDetailCollection doDetsTemp = new DeliveryOrderDetailCollection();
                foreach (DeliveryOrder doHdr in delOrderHdrColl)
                {
                    DeliveryOrderDetailCollection doDets = new DeliveryOrderDetailCollection();
                    doDets = delOrderDetColl.Clone().Where("Dohdrid", doHdr.OrderNumber).Filter();

                    DeliveryController doCtrl = new DeliveryController();
                    doHdr.OrderNumber = doCtrl.myDeliveryOrderHdr.OrderNumber;
                    doHdr.PurchaseOrderRefNo = doCtrl.myDeliveryOrderHdr.PurchaseOrderRefNo;
                    doHdr.Deleted = false;

                    for (int i = 0; i < doDets.Count; i++)
                    {
                        doDets[i].Dohdrid = doHdr.OrderNumber;
                        doDets[i].DetailsID = doHdr.OrderNumber + "." + i.ToString();
                    }

                    DeliveryOrderController.SaveOrder(doHdr, doDets);

                    doDetsTemp.AddRange(doDets);
                }

                // Update delOrderDetColl
                delOrderDetColl = doDetsTemp;
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number.Equals(Constants.PRIMARY_KEY_VOILATION))
                {
                    throw new System.ApplicationException(Constants.ORDERNUMBER_ALREADY_EXISTS);
                }
                else
                    throw new System.ApplicationException(sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Save multiple Delivery Order
        /// </summary>
        public static void SaveMultipleOrder(DeliveryOrderCollection delOrderHdrColl, DeliveryOrderDetailCollection delOrderDetColl)
        {
            SaveMultipleOrder(ref delOrderHdrColl, ref delOrderDetColl);
        }


        /// <summary>
        /// Update the DO_CurrentReceiptNo
        /// </summary>
        public static void CustNumUpdate()
        {
            try
            {
                #region customNo Update
                int runningNo = 0;

                string selectmaxno = "select AppSettingValue from AppSetting where AppSettingKey='DO_CurrentReceiptNo'";
                string currentReceiptNo = DataService.ExecuteScalar(new QueryCommand(selectmaxno)).ToString();

                int.TryParse(currentReceiptNo, out runningNo);

                string updatemaxnum1 = "update appsetting set AppSettingValue='" + ++runningNo + "' where AppSettingKey='DO_CurrentReceiptNo'";
                DataService.ExecuteQuery(new QueryCommand(updatemaxnum1));

                //default max receiptno is 4
                string sql2 = "select case AppSettingValue when '' then '4' else AppSettingValue end from AppSetting where AppSettingKey='DO_ReceiptLength'";
                QueryCommand Qcmd2 = new QueryCommand(sql2);
                int maxReceiptNo = 4;
                int.TryParse(DataService.ExecuteScalar(Qcmd2).ToString(), out maxReceiptNo);

                //check if it has reached the max no for that digit (99,999,9999.. etc)
                bool maximumReached = false;
                if (maxReceiptNo != 0)
                {
                    maximumReached = true;
                    for (int i = 0; i < maxReceiptNo; i++)
                    {
                        if (currentReceiptNo[i] != '9')
                        {
                            maximumReached = false;
                            break;
                        }
                    }
                }

                //if it has reached, update the maxreceiptno
                if (maximumReached)
                {
                    string sql3 = "update appsetting set AppSettingValue = " + ++maxReceiptNo + " where AppSettingKey='DO_ReceiptLength'";
                    QueryCommand Qcmd3 = new QueryCommand(sql3);
                    DataService.ExecuteQuery(Qcmd3);
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Check if all items in a Delivery Order is cancelled (Deleted = true)
        /// </summary>
        public static bool IsAllItemsCancelled(DeliveryOrder delOrder)
        {
            foreach (DeliveryOrderDetail dod in delOrder.DeliveryOrderDetails())
            {
                if (!dod.Deleted.GetValueOrDefault(false)) return false;
            }
            return true;
        }

        /// <summary>
        /// Cancel DeliveryOrderDetails that is related to this returned item
        /// </summary>
        public static void CancelDeliveryOfReturnedItem(string returnedReceiptNo, string itemNo)
        {
            string sql = @"
                            SELECT dod.*
                            FROM OrderHdr oh
                                INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID
                                INNER JOIN DeliveryOrderDetails dod ON dod.OrderDetID = od.OrderDetID
                            WHERE oh.userfld5 = @invno AND od.ItemNo = @itemno
                         ";
            DeliveryOrderDetailCollection dodColl = new InlineQuery().ExecuteAsCollection<DeliveryOrderDetailCollection>(sql, returnedReceiptNo, itemNo);
            foreach (DeliveryOrderDetail dod in dodColl)
            {
                dod.Deleted = true;
                dod.Save(UserInfo.username);

                DeliveryOrder delOrder = new DeliveryOrder(dod.Dohdrid);
                if (IsAllItemsCancelled(delOrder))
                {
                    // If all items in a DO is cancelled, then cancel the DO as well
                    delOrder.Deleted = true;
                    delOrder.Save(UserInfo.username);
                }
            }
        }

        public static bool UpdateDeliveryOrderIsUpdated(string listID)
        {
            bool result = true;
            if (listID == "") return true;
            try
            {
                string[] lst = listID.Split(',');
                if (lst.GetLength(0) > 0)
                {
                    QueryCommandCollection col = new QueryCommandCollection();
                    for (int i = 0; i < lst.GetLength(0); i++)
                    {
                        QueryCommand cmd = new QueryCommand("Update DeliveryOrder set IsServerUpdate = 0 where OrderNumber = '" + lst[i] + "'");
                        col.Add(cmd);
                    }
                    DataService.ExecuteTransaction(col);
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Update DeliveryOrder IsUpdateServer Failed. " + ex.Message);
                return false;
            }
        }
    }
}
