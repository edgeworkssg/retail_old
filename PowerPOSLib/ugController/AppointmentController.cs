using System;
using System.Collections.Generic;
using System.Transactions;
using PowerPOS.Container;
using SubSonic;
using System.Linq;
using System.Data;
using PowerPOS.AppointmentRealTimeController;
using System.ComponentModel;

namespace PowerPOS
{
	public partial class AppointmentController
    {
	    public static void SaveAppointment(Appointment appointment, IList<AppointmentItem> items)
	    {
            try
            {
                //QueryCommandCollection col = new QueryCommandCollection();
                using (var transaction = new TransactionScope())
                {
                    appointment.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                    appointment.IsServerUpdate = true;
                    appointment.Save(UserInfo.username);
                    //col.Add(appointment.GetSaveCommand(UserInfo.username));

                    Logger.writeLog("Check point 2" + appointment.Duration);
                    var oldItems = new AppointmentItemCollection();
                    oldItems.Where(AppointmentItem.Columns.AppointmentId, appointment.Id);
                    oldItems.Load();

                    foreach (var oldItem in oldItems)
                    {
                        var i = FindItem(items, oldItem);
                        if (i < 0)
                            AppointmentItem.Delete(oldItem.Id);
                    }

                    foreach (var item in items)
                    {
                        var i = FindItem(oldItems, item);

                        if (i >= 0)
                        {
                            oldItems[i].ItemNo = item.ItemNo;
                            oldItems[i].Quantity = item.Quantity;
                            oldItems[i].UnitPrice = item.UnitPrice;
                        }
                        else
                            oldItems.Add(item);
                    }

                    if (appointment.Deleted == true)
                        foreach (var oldItem in oldItems)
                            oldItem.Deleted = true;

                    foreach (var oldItem in oldItems)
                    {
                        oldItem.AppointmentId = appointment.Id;
                        //col.Add(oldItem.GetSaveCommand(UserInfo.username));
                    }

                    oldItems.SaveAll(UserInfo.username);
                    Logger.writeLog("Checkpoint 3");
                    
                    transaction.Complete();
                }
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); }
	    }

		private static int FindItem(IList<AppointmentItem> items, AppointmentItem item)
		{
			if (item.Id == Guid.Empty)
				return -1;

			for (int i = 0; i < items.Count; i++)
				if (items[i].Id == item.Id) return i;

			return -1;
		}

        public static bool CheckCollision(string appointmentID, string userName, DateTime date, int duration, out string message)
        {
            bool isSuccess = true;

            message = "Your appointment collide with the others";

            var appointments = new AppointmentCollection();
            appointments.Where(Appointment.Columns.SalesPersonID, Comparison.Equals, userName);
            appointments.Where(Appointment.Columns.Deleted, false);
            appointments.OrderByAsc(Appointment.Columns.StartTime);
            appointments.Load();

            var existApp = appointments.Where(o => o.Id.ToString() == appointmentID).FirstOrDefault();
            if (existApp != null)
                appointments.Remove(existApp);

            for (int i = 0; i < appointments.Count; i++)
            {
                if (appointments[i].Id.ToString() == appointmentID) continue;

                var dtrA = new DateTimeRange();
                dtrA.Start = appointments[i].StartTime;
                dtrA.End = appointments[i].StartTime.AddMinutes(appointments[i].Duration);

                var dtrB = new DateTimeRange();
                dtrB.Start = date;
                dtrB.End = date.AddMinutes(duration);

                if (dtrA.Intersects(dtrB))
                {
                    isSuccess = false;
                }
            }

           return isSuccess;
        }


        public static bool CheckCollisionResource(string appointmentID, string userName, string ResourceID, DateTime date, int duration, out string message)
        {
            bool isSuccessResource = true;
            message = "Your resource is full.";

            var appointmentResources = new AppointmentCollection();
            appointmentResources.Where(Appointment.Columns.ResourceID, Comparison.Equals, ResourceID);
            appointmentResources.Where(Appointment.Columns.Deleted, false);
            appointmentResources.OrderByAsc(Appointment.Columns.StartTime);
            appointmentResources.Load();

            var existAppResource = appointmentResources.Where(o => o.Id.ToString() == appointmentID).FirstOrDefault();
            if (existAppResource != null)
                appointmentResources.Remove(existAppResource);
            int intersects = 0;
            
            for (int i = 0; i < appointmentResources.Count; i++)
            {
                if (appointmentResources[i].Id.ToString() == appointmentID) continue;

                var dtrA = new DateTimeRange();
                dtrA.Start = appointmentResources[i].StartTime;
                dtrA.End = appointmentResources[i].StartTime.AddMinutes(appointmentResources[i].Duration);

                var dtrB = new DateTimeRange();
                dtrB.Start = date;
                dtrB.End = date.AddMinutes(duration);

                if (dtrA.Intersects(dtrB))
                {
                    intersects++;
                }
            }

            Resource res = new Resource(ResourceID);
            if (intersects >= res.Capacity)
            {
                isSuccessResource = false;
            }

            return isSuccessResource;
        }

        public static DateTime GetSuggestedDate(string userName, DateTime startTime, int duration)
        {
            DateTime date = startTime;

            var appointments = new AppointmentCollection();
            appointments.Where(Appointment.Columns.SalesPersonID, Comparison.Equals, userName);
            appointments.Where(Appointment.Columns.Deleted, false);
            appointments.OrderByAsc(Appointment.Columns.StartTime);
            appointments.Load();
            var theLastAppoinment = appointments.Where(o => o.StartTime.Date == date.Date).OrderByDescending(o => o.StartTime).ToList().FirstOrDefault();
            if (theLastAppoinment != null)
                date = theLastAppoinment.StartTime.AddMinutes(theLastAppoinment.Duration);

            return date;
        }

        public static bool IsPartialPayment(string orderHdrID, out decimal outstanding)
        {
            bool isPartialPayment = false;
            outstanding = 0;
            try
            {
                string sql = @"
                DECLARE @OrderHdrID AS VARCHAR(50);
                DECLARE @InstPayment AS MONEY;

                SET @OrderHdrID = '{0}';
                SET @InstPayment = 0;

                SELECT  @InstPayment = SUM(OD.Amount)
                FROM	OrderHdr OH
		                INNER JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
                WHERE	ISNULL(OH.IsVoided,0) = 0 AND ISNULL(OD.IsVoided,0) = 0
		                AND OD.ItemNo = 'INST_PAYMENT'
		                AND OD.Userfld3 = @OrderHdrID
                		
                SELECT   OH.OrderHdrID
		                ,OH.NettAmount 
		                ,SUM(CASE WHEN RD.PaymentType = 'Installment' THEN RD.Amount ELSE 0 END) INSTALLMENT
		                ,SUM(CASE WHEN RD.PaymentType <> 'Installment' THEN RD.Amount ELSE 0 END) + ISNULL(@InstPayment,0) PAYMENT
		                ,OH.NettAmount - SUM(CASE WHEN RD.PaymentType <> 'Installment' THEN RD.Amount ELSE 0 END) - ISNULL(@InstPayment,0) OUTSTANDING		
                FROM	OrderHdr OH
		                INNER JOIN ReceiptHdr RH ON RH.OrderHdrID = OH.OrderHdrID
		                INNER JOIN ReceiptDet RD ON RD.ReceiptHdrID  = RH.ReceiptHdrID
                WHERE	ISNULL(OH.IsVoided,0) = 0
		                AND OH.OrderHdrID = @OrderHdrID
                GROUP BY OH.OrderHdrID
		                ,OH.NettAmount";
                sql = string.Format(sql, orderHdrID);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));

                if (dt.Rows.Count > 0 && dt.Columns.Contains("OUTSTANDING"))
                {
                    outstanding = (decimal)dt.Rows[0]["OUTSTANDING"];
                    isPartialPayment = outstanding > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                isPartialPayment = false;
            }

            return isPartialPayment;
        }

        public static bool SendAppointment(Appointment app, BackgroundWorker SyncAppointmentThread, out string status)
        {
            status = "";

            try
            {
                SyncAppointmentRealTimeController ssc = new SyncAppointmentRealTimeController();

                DateTime ModifiedOn = DateTime.Now;

                AppointmentCollection appcol = new AppointmentCollection();
                app.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                appcol.Add(app);

                AppointmentItemCollection appitemcol = new AppointmentItemCollection();
                appitemcol.Where(AppointmentItem.Columns.AppointmentId, app.Id);
                appitemcol.Load();

                if (!ssc.SendAppointmentToServer(appcol, appitemcol, out status))
                {
                    throw new Exception(status);
                }

                if (SyncAppointmentThread != null && !SyncAppointmentThread.IsBusy)
                    SyncAppointmentThread.RunWorkerAsync();

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }
        }

        public static bool SendAppointment(List<Appointment> app, BackgroundWorker SyncAppointmentThread, out string status)
        {
            status = "";

            try
            {
                SyncAppointmentRealTimeController ssc = new SyncAppointmentRealTimeController();

                //AppointmentCollection appcol = new AppointmentCollection();
                foreach (Appointment ap in app)
                {
                    ap.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                    //appcol.Add(ap);
                    if (!SendAppointment(ap, SyncAppointmentThread, out status))
                    {
                        throw new Exception(status);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }
        }

        public static bool SendAppointment(Appointment app, List<AppointmentItem> items, BackgroundWorker SyncAppointmentThread, out string status)
        {
            status = "";

            try
            {
                SyncAppointmentRealTimeController ssc = new SyncAppointmentRealTimeController();

                AppointmentCollection appcol = new AppointmentCollection();
                app.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                appcol.Add(app);

                AppointmentItemCollection appitemcol = new AppointmentItemCollection();
                foreach (AppointmentItem item in items)
                {
                    appitemcol.Add(item);
                }


                if (!ssc.SendAppointmentToServer(appcol, appitemcol,  out status))
                {
                    throw new Exception(status);
                }

                if (SyncAppointmentThread != null && !SyncAppointmentThread.IsBusy)
                    SyncAppointmentThread.RunWorkerAsync();
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }
        }

        public static bool SendAppointment(Appointment app, List<AppointmentItem> items, BackgroundWorker SyncAppointmentThread, Membership member, out string status)
        {
             status = "";

            try
            {
                SyncAppointmentRealTimeController ssc = new SyncAppointmentRealTimeController();

                AppointmentCollection appcol = new AppointmentCollection();
                app.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                appcol.Add(app);

                AppointmentItemCollection appitemcol = new AppointmentItemCollection();
                foreach (AppointmentItem item in items)
                {
                    appitemcol.Add(item);
                }

                MembershipCollection members = new MembershipCollection();
                if (member != null)
                {
                    members.Add(member);
                }

               
                if (!ssc.SendAppointmentToServer(appcol, appitemcol, members, out status))
                {
                    throw new Exception(status);
                }

                if (SyncAppointmentThread != null && !SyncAppointmentThread.IsBusy)
                    SyncAppointmentThread.RunWorkerAsync();
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }
        }

        public static bool UpdateAppointmentIsUpdated(string listID)
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
                        QueryCommand cmd = new QueryCommand("Update Appointment set isServerUpdate = 0 where id = '" + lst[i] + "'");
                        col.Add(cmd);
                    }
                    DataService.ExecuteTransaction(col);
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Update Appointment IsUpdateServer Failed. " + ex.Message);
                return false;
            }
        }

    }
}
