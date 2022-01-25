using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using System.Globalization;
using System.Collections;
using System.Configuration;
using System.IO;

namespace ClientSynchronizer
{
    public class EmailSender
    {
        public static void SendEmailError(Exception ex)
        {
            try
            {
                string maxSyncFailedCount_str = AppSetting.GetSetting("MaxSyncFailedCount");
                string maxHourToResetFailCount_str = AppSetting.GetSetting("MaxHourToResetFailCount");
                string lastSyncFailed_str = AppSetting.GetSetting("LastSyncFailed");
                string syncFailedCount_str = AppSetting.GetSetting("SyncFailedCount");

                int maxSyncFailedCount = 3;
                int maxHourToResetFailCount = 3;
                DateTime lastSyncFailed = DateTime.Now;
                int syncFailedCount = 0;

                if (!string.IsNullOrEmpty(maxSyncFailedCount_str))
                    int.TryParse(maxSyncFailedCount_str, out maxSyncFailedCount);
                else
                    AppSetting.SetSetting("MaxSyncFailedCount", maxSyncFailedCount.ToString());

                if (!string.IsNullOrEmpty(maxHourToResetFailCount_str))
                    int.TryParse(maxHourToResetFailCount_str, out maxHourToResetFailCount);
                else
                    AppSetting.SetSetting("MaxHourToResetFailCount", maxHourToResetFailCount.ToString());

                if (!string.IsNullOrEmpty(lastSyncFailed_str))
                    DateTime.TryParseExact(lastSyncFailed_str, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out lastSyncFailed);
                if (!string.IsNullOrEmpty(syncFailedCount_str))
                    int.TryParse(syncFailedCount_str, out syncFailedCount);

                syncFailedCount += 1;
                if (syncFailedCount >= maxSyncFailedCount && DateTime.Now.Subtract(lastSyncFailed).TotalHours <= maxHourToResetFailCount)
                {
                    SendMail(ex);
                    syncFailedCount = 0;
                    AppSetting.SetSetting("LastSyncFailed", "");
                    AppSetting.SetSetting("SyncFailedCount", "0");
                }
                else
                {
                    AppSetting.SetSetting("LastSyncFailed", DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
                    if (syncFailedCount >= 3)
                        syncFailedCount = 1;
                    AppSetting.SetSetting("SyncFailedCount", syncFailedCount.ToString());
                }
            }
            catch (Exception e)
            {
                Logger.writeLog(e);
            }
        }

        public static void SendMail(Exception ex)
        {
            string status = "";
            string bodyMail = "- SYNC Failed at " + DateTime.Now;
            bodyMail += "<br/> " + ex.ToString() + "Innr:" + ex.InnerException;
            var ms = new MassEmail();
            var attachment = new Dictionary<string, Stream>();
            var result = ms.SendEmails(
                new ArrayList { AppSetting.GetSetting(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_DefaultMailTo)) },
                AppSetting.GetSetting(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail)),
                "Sync Failed",
                bodyMail,
                bodyMail,
                AppSetting.GetSetting(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SMTP)),
               AppSetting.GetSetting(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Username)),
                AppSetting.GetSetting(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Password)),
                false, attachment, AppSetting.GetSetting(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Port)),
                out status);
        }
    }
}
