using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using PowerPOS;
using PowerPOS.Container;
using POSDevices;
using System.Drawing;
using SubSonic;
using System.Drawing.Printing;
namespace Synchronizer
{
    class Program
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_MINIMIZE = 6;
        private const int SW_MAXIMIZE = 3;
        private const int SW_RESTORE = 9;

        static void Main(string[] args)
        {
            //PowerPOS.Controller.SyncClientController.Load_WS_URL();
            PointOfSaleController.GetPointOfSaleInfo();
            SyncClientController.Load_WS_URL();

            Console.WriteLine("Sending to:" + SyncClientController.WS_URL);
            Logger.writeLog("Sending to:" + SyncClientController.WS_URL);

            DateTime startDate = DateTime.Now, endDate = DateTime.Now;

            int syncDays = 0;

            if (args.Length == 0)
            {
                startDate = DateTime.Today.AddDays(-3);
                endDate = DateTime.Today;
            }
            else if (int.TryParse(args[0], out syncDays))
            {
                if (syncDays < 2) return;
                endDate = DateTime.Now;
                startDate = DateTime.Today.AddDays(-syncDays);
            }
            else
            {
                startDate = DateTime.Parse(args[0]);
                endDate = DateTime.Parse(args[1]);
            }

            
            SyncClientController.GetBasicInfoFromServer(true, true, true, true, true, true, true,true,true);
            SyncClientController.GetDeliveryOrderFromServer(startDate, endDate);
            SendData(startDate, endDate);
            
        }

        static bool SendData(DateTime StartDate, DateTime EndDate)
        {
            SyncClientController.Load_WS_URL();
            if (SyncClientController.WS_URL == "")
            {
                return true;
            }

            bool TotalResult = SyncClientController.SendLogsToServer(StartDate, EndDate.AddSeconds(5)); ;

            bool result = false;
            while (DateTime.Compare(StartDate, EndDate) <= 0)
            {
                result = SyncClientController.
                    SendOrderCCMW(StartDate, StartDate.AddHours(8));
                TotalResult &= result;

                // Send Delivery Order data if using "Cash & Carry / Pre-Order" feature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PromptSelectOrderType), false))
                {
                    result = SyncClientController.SendDeliveryOrderToServer(StartDate, StartDate.AddHours(8), false);
                    TotalResult &= result;
                }

                Logger.writeLog("Sending Sales from " +
                StartDate.ToString("dd/MM/yy HH:mm:ss") +
                " to " + StartDate.AddHours(8).ToString("dd/MM/yy HH:mm:ss")
                + " POS ID " + PointOfSaleInfo.PointOfSaleID + " completed");
                StartDate = StartDate.AddHours(8);
            }
            SyncClientController.deductInventory();
      
            Logger.writeLog("Sending result:" + TotalResult);

            return TotalResult;
        }
    }
}
