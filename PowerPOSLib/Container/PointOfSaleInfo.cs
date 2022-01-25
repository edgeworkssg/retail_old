using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace PowerPOS.Container
{
    public class PointOfSaleInfo
    {
        public static int PointOfSaleID;
        public static string PointOfSaleName; //for display purpose   
        public static string OutletName;
        public static string NETsTerminalID;
        public static int InventoryLocationID;
        public static string InventoryLocationName;
        public static int DepartmentID;
        public static int DepartmentName;
        public static DataTable QuickAccessCategory;
        public static string EZLinkTerminalID;
        public static bool IsEZLinkTerminal;
        public static string EZLinkTerminalPwd;
        public static bool promptSalesPerson;
        public static bool useMembership;
        public static bool allowLineDisc;
        public static bool allowChangeCashier;
        //public static bool allowFeedback;
        public static string  SQLServerName;
        public static string DBName;
        public static bool IntegrateWithInventory;
        public static string MembershipPrefixCode;
        public static string WS_URL;
        public static DataTable PaymentTypes;
        public static DataTable DeliveryTimes;
    }
}
