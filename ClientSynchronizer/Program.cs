using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using System.Globalization;

namespace ClientSynchronizer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Time zone
                CultureInfo ct = new CultureInfo("");
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy-MM-dd";
                ct.DateTimeFormat = dtFormat;
                System.Threading.Thread.CurrentThread.CurrentCulture = ct;
                System.Threading.Thread.CurrentThread.CurrentUICulture = ct;


                if (!SyncClientController.SendNewSignUpWithoutOrder())
                {
                    Logger.writeLog("Member Sync Failed");
                    EmailSender.SendEmailError(new Exception("Member Sync : Failed"));
                }

                // Client sync routine
                if (!OrderSync.StartOrderSync())
                {
                    Logger.writeLog("Order Sync Failed");
                    EmailSender.SendEmailError(new Exception("Order Sync : Failed"));
                }

                if (!LogsSync.StartLogsSync())
                {
                    Logger.writeLog("Logs Sync Failed");
                    EmailSender.SendEmailError(new Exception("Logs Sync : Failed"));
                }

                SyncClientController.deductInventory();

                if (!InventorySync.GetCurrentInventory())
                {
                    Logger.writeLog("DateBlocking Sync Failed");
                    EmailSender.SendEmailError(new Exception("Inventory Sync : Failed"));
                }

                if (!SyncClientController.UpdateOrderDetFromDownloadedInventoryHdr())
                {
                    Logger.writeLog("UpdateOrderDetFromDownloadedInventoryHdr Sync Failed");
                    EmailSender.SendEmailError(new Exception("UpdateOrderDetFromDownloadedInventoryHdr Sync : Failed"));
                }

                if (!InventorySync.DeleteInventoryDetFromVoidedOrder())
                {
                    Logger.writeLog("DeleteInventoryDetFromVoidedOrder Sync Failed");
                    EmailSender.SendEmailError(new Exception(""));
                }

                if (!OrderSync.UpdateOrderHdrUserFld1236ByPosIDViewBillLimitAndIsVoided())
                {
                    Logger.writeLog("UpdateOrderHdrUserFld1236ByPosIDViewBillLimitAndIsVoided Sync Failed");
                    EmailSender.SendEmailError(new Exception("UpdateOrderHdrUserFld1236ByPosIDViewBillLimitAndIsVoided Sync : Failed"));
                }

                // Send Delivery Order data if using "Cash & Carry / Pre-Order" feature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PromptSelectOrderType), false))
                {
                    if (!SyncClientController.SendDeliveryOrderToServer(DateTime.Today.AddDays(-3), DateTime.Now, true))
                    {
                        Logger.writeLog("SendDeliveryOrderToServer Sync Failed");
                    }
                }

                SyncClientController.Load_WS_URL();
                if (!SyncClientController.GetMemberships(false))
                {
                    Logger.writeLog("Get Membership Sync Failed");
                    EmailSender.SendEmailError(new Exception("Get Membership : Failed"));
                }

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
    }
}
