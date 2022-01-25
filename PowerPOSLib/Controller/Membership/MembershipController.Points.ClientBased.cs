using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using SubSonic;
using System.Data;
using System.Collections;
using PowerPOS.Container;
using System.IO;
using System.Net.NetworkInformation;
using System.Transactions;

/// This file is to define all function to modify points that is located in client

namespace PowerPOS
{
    public partial class MembershipController
    {
        /// <summary>
        /// Add Points based on Sales Nett Amount (Please check item validity using function [IsItemToPoint] before using this function)
        /// </summary>
        /// <param name="MembershipNo">Customer's membership number</param>
        /// <param name="SalesNettAmount">Nett Amount of Customer's Invoice</param>
        /// <param name="TransactionDate">The date of transaction</param>
        /// <param name="OrderHdrID">OrderHdrID - For logging purpose</param>
        /// <param name="status">Error string (If any)</param>
        /// <param name="isSendToServer">Should this function update points on server?</param>
        /// <returns>True if success, otherwise False</returns>
        /// <remarks>
        /// Add Points using [ws.AddPoints]     --> Server Update
        /// Add Points using [AddPoints_Final]  --> Local Update
        /// </remarks>
        public static bool AllocatePointsFromSales(string MembershipNo, decimal SalesNettAmount,
            DateTime TransactionDate, string OrderHdrID, string UserName, out string Status, bool isSendToServer)
        {
            try
            {
                Membership member = new Membership(Membership.Columns.MembershipNo, MembershipNo);
                #region *) Validation: Check if MembershipNo is valid in the system [Exit if False]
                if (!member.IsLoaded)
                { Status = "Membership Number do not exist."; return false; }
                #endregion

                #region *) Validation: Check if MembershipNo is expired [NotImplemented]
                #endregion

                #region *) Initialize: Load POINT_PERCENTAGE value
                POINT_PERCENTAGE = member.MembershipGroup.Userfloat1.GetValueOrDefault(0);
                #endregion

                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                #region *) Core: Add Point using [AddPoints_Final] - Logger inside
                if (isSendToServer)
                {
                    if (!ws.AddPoint(MembershipNo, OrderHdrID, TransactionDate, 0,
                        SalesNettAmount * POINT_PERCENTAGE, "SYSTEM", out Status))
                    { return false; }
                }
                else
                {
                    if (!AddPoints_Final(MembershipNo, OrderHdrID, TransactionDate, 0,
                        SalesNettAmount * POINT_PERCENTAGE, "SYSTEM", out Status))
                    { return false; }
                }
                #endregion

                Status = "";
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                Status = "Some error occured";
                return false;
            }
        }

        /// <summary>
        /// Allocate bought points
        /// </summary>
        /// <param name="MembershipNo">Customer's membership number</param>
        /// <param name="Points">Points to be allocated</param>
        /// <param name="TransactionDate">The date of transaction</param>
        /// <param name="OrderHdrID">OrderHdrID - For logging purpose</param>
        /// <param name="UserName">Name of employee that do transaction</param>
        /// <param name="status">Error string (If any)</param>
        /// <param name="isSendToServer">Should this function update points on server?</param>
        /// <returns>True if success, otherwise False</returns>
        /// <remarks>
        /// Add Points using [ws.AddPoints]     --> Server Update
        /// Add Points using [AddPoints_Final]  --> Local Update
        /// </remarks>
        public static bool AllocatePoints(string MembershipNo, decimal Points,
            DateTime TransactionDate, string OrderHdrID, string UserName, out string Status, bool isSendToServer)
        {
            try
            {
                Membership member = new Membership(Membership.Columns.MembershipNo, MembershipNo);
                #region *) Validation: Check if MembershipNo is valid in the system [Exit if False]
                if (!member.IsLoaded)
                { Status = "Membership Number [" + MembershipNo + "] do not exist."; return false; }
                #endregion

                #region *) Validation: Check if MembershipNo is expired [NotImplemented]
                #endregion

                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                #region *) Core: Add Point using [AddPoints_Final] - Logger inside
                if (isSendToServer)
                {
                    if (!ws.AddPoint(MembershipNo, OrderHdrID, TransactionDate,
                        0, Points, UserName, out Status))
                    { return false; }
                }
                else
                {
                    if (!AddPoints_Final(MembershipNo, OrderHdrID, TransactionDate,
                        0, Points, UserName, out Status))
                    { return false; }
                }
                #endregion

                Status = "";
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                Status = "Some error occured";
                return false;
            }
        }

        /// <summary>
        /// Deduct Points based o
        /// Add Points based on Sales Nett Amount (Please check item validity using function [IsItemToPoint] before using this function)
        /// </summary>
        /// <param name="MembershipNo"></param>
        /// <param name="UsedPoints"></param>
        /// <param name="OrderHdrID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static bool UsePointsFromSales(string MembershipNo, decimal DollarAmount, DateTime TransactionDate,
            string OrderHdrID, out string Status, bool isSendToServer)
        {
            /// 1 Dollars = 1 Points
            decimal PointPerDollars = 1;

            try
            {
                Membership member = new Membership(Membership.Columns.MembershipNo, MembershipNo);
                #region *) Validation: Check if MembershipNo is valid in the system [Exit if False]
                if (!member.IsLoaded)
                { Status = "Membership Number do not exist."; return false; }
                #endregion

                #region *) Validation: Check if MembershipNo is expired [NotImplemented]
                #endregion

                decimal PointAmount = 0;
                #region *) Initialize: Load HowMuchIsTheWorthOfOnePoints Value
                PointAmount = PointPerDollars * DollarAmount;
                #endregion

                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                #region *) Core: Deduct Point using [DeductPoints_Final] - Logger inside
                if (isSendToServer)
                {
                    if (!ws.DeductPoints(MembershipNo, OrderHdrID, TransactionDate, PointAmount, "POINTS", "SYSTEM", out Status))
                    { return false; }
                }
                else
                {
                    if (!DeductPoints_Final(MembershipNo, OrderHdrID, TransactionDate, PointAmount, "POINTS", "SYSTEM", out Status))
                    { return false; }
                }
                #endregion

                Status = "";
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                Status = "Some error occured";
                return false;
            }
        }

        /// <summary>
        /// Deduct Points based o
        /// Add Points based on Sales Nett Amount (Please check item validity using function [IsItemToPoint] before using this function)
        /// </summary>
        /// <param name="MembershipNo"></param>
        /// <param name="UsedPoints"></param>
        /// <param name="OrderHdrID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static bool UsePoints(string MembershipNo, decimal Points, DateTime TransactionDate,
            string OrderHdrID, string packageRefNo, string UserName, out string Status, bool isSendToServer)
        {
            try
            {
                Membership member = new Membership(Membership.Columns.MembershipNo, MembershipNo);
                #region *) Validation: Check if MembershipNo is valid in the system [Exit if False]
                if (!member.IsLoaded)
                { Status = "Membership Number do not exist."; return false; }
                #endregion

                #region *) Validation: Check if MembershipNo is expired [NotImplemented]
                #endregion

                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                #region *) Core: Deduct Point using [DeductPoints_Final] - Logger inside
                if (isSendToServer)
                {
                    if (!ws.DeductPoints(MembershipNo, OrderHdrID, TransactionDate, Points, packageRefNo, UserName, out Status))
                    { return false; }
                }
                else
                {
                    if (!DeductPoints_Final(MembershipNo, OrderHdrID, TransactionDate, Points, packageRefNo, UserName, out Status))
                    { return false; }
                }
                #endregion

                Status = "";
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                Status = "Some error occured";
                return false;
            }
        }

        public static DataTable GetHistory_Point_Client(string MembershipNo, string PackageName
            , out DateTime StartValidPeriod, out DateTime EndValidPeriod, out decimal RemainingPoint)
        {
            StartValidPeriod = new DateTime(2000, 1, 1);
            EndValidPeriod = new DateTime(2100, 1, 1);
            RemainingPoint = 0;

            try
            {
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;

                return ws.GetHistory_Point(MembershipNo, PackageName, out StartValidPeriod, out EndValidPeriod, out RemainingPoint);
            }
            catch (Exception X)
            {
                Logger.writeLog(X);

                System.Windows.Forms.MessageBox.Show("Cannot connect to server.\nPlease try again.", "Connection Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                return null;
            }
        }

        public static string[] GetHistory_Buttons_Client(string MembershipNo)
        {
            try
            {
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;

                //return ws.GetHistory_Buttons(MembershipNo);
                return ws.getPackageList(MembershipNo);
            }
            catch (Exception X)
            {
                Logger.writeLog(X);

                return null;
            }
        }
    }
}
