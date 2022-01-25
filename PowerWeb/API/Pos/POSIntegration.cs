using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PowerPOS;
using PowerPOSLib;
using System.Web.Script.Serialization;
using System.Globalization;
using PowerPOS.Container;
using System.Threading;
using System.Web.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PowerWeb.API.Pos
{
    public class POSIntegration
    {


        public static void AutoLogin()
        {
            try
            {
                PowerPOS.PointOfSaleController.GetPointOfSaleInfo();
                UserMst myUser = new UserMst(UserMst.Columns.UserName, "edgeworks");
                UserInfo.username = myUser.UserName;
                UserInfo.role = myUser.UserGroup.GroupName;
                UserInfo.displayName = myUser.DisplayName;
                UserInfo.privileges = UserController.FetchGroupPrivileges(UserInfo.role);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }


        public static string UploadAppOrderToHoldTest(string jsonInput)
        {
            Logger.writeLog("UploadAppOrderToHold json : " + jsonInput);
            bool isSuccess = false;
            string message = "";
            string result = "";

            try
            {
                var transHdr = new JavaScriptSerializer().Deserialize<AppTransactionHdr>(jsonInput);
                //transHdr.Items = new List<AppTransactionDet>();


                PowerPOS.Membership myUser = new PowerPOS.Membership(PowerPOS.Membership.Columns.MembershipNo, transHdr.MembershipNo);
                if (myUser.IsNew)
                    throw new Exception("MembershipNo :" + transHdr.MembershipNo + "Not Found");

                var memberName = myUser.NameToAppear;

                isSuccess = true;
                message = "REFNO : " + transHdr.RefNo + " \n " + "QUEUENO :" + transHdr.QueueNo + " \n " + "MEMBERSHIPNO : " + transHdr.MembershipNo + "NAME : " + memberName + " \n ";

                foreach (var item in transHdr.Items)
                {
                    PowerPOS.Item it = new PowerPOS.Item(PowerPOS.Item.Columns.ItemNo, item.ItemNo);
                    if (it.IsNew)
                        throw new Exception("ItemNo :" + item.ItemNo + " Not Found");
                    message += "ITEMNO : " + it.ItemNo + " \n " + "ITEMNAME : " + " \n " + "ITEMPRICE : " + it.RetailPrice.ToString() + " \n ";
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.writeLog("UploadAppOrderToHold failed : " + jsonInput);
                Logger.writeLog(ex);
                message = "Error : " + ex.Message;
            }

            var resultData = new
            {
                resultCode = isSuccess ? 0 : -1,
                resultMessage = message
            };
            result = new JavaScriptSerializer().Serialize(resultData);

            Logger.writeLog("UploadAppOrderToHold result : " + result);
            return result;
        }

        private static POSController InitPOSController(AppTransactionHdr transHdr, out string message)
        {
            POSController pos = null;
            message = "";

            try
            {
                string status = "";

                #region *) Load Settings

                string uploadOrder_CashierID = System.Configuration.ConfigurationManager.AppSettings["uploadOrder_CashierID"] + "";

                if (AppSetting.GetSettingFromDBAndConfigFile("RoundingPreference") != null)
                {
                    POSController.RoundingPreference = AppSetting.GetSettingFromDBAndConfigFile("RoundingPreference").ToString();
                }
                else
                {
                    POSController.RoundingPreference = "";
                }
                #endregion


                #region *) Validation



                if (transHdr.Items == null || transHdr.Items.Count == 0)
                    throw new Exception("Item(s) cannot be empty");

                if (string.IsNullOrEmpty(transHdr.MembershipNo))
                    throw new Exception("MembershipNo cannot be empty");

                if (string.IsNullOrEmpty(transHdr.Username))
                    throw new Exception("Cashier Username cannot be empty");

                uploadOrder_CashierID = transHdr.Username + "";

                PowerPOS.Membership myUser = new PowerPOS.Membership(PowerPOS.Membership.Columns.MembershipNo, transHdr.MembershipNo);
                if (myUser.IsNew)
                    throw new Exception("MembershipNo :" + transHdr.MembershipNo + " Not Found");

                #endregion




                #region *) Init POS Controller
                pos = new POSController();
                UserMst um = new UserMst(uploadOrder_CashierID);
                if (um.IsNew)
                    throw new Exception("Cashier :" + uploadOrder_CashierID + " Not Found");

                pos.AssignMembership(transHdr.MembershipNo, out status);
                #endregion


                #region *) Add Order Details
                ViewItem myItem;
                foreach (var det in transHdr.Items)
                {

                    PowerPOS.Item it = new PowerPOS.Item(PowerPOS.Item.Columns.ItemNo, det.ItemNo);
                    if (it.IsNew)
                        throw new Exception("ItemNo :" + det.ItemNo + " Not Found");

                    decimal PreferedDiscount = pos.GetPreferredDiscount();
                    bool ApplyPromo = true;
                    bool UseWeight = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseWeight), false);
                    int detQty;
                    if (!UseWeight)
                    {
                        detQty = decimal.ToInt32(det.Qty);
                        det.Qty = (decimal)detQty;
                    }
                    pos.AddItemToOrderWithPriceMode(it, det.Qty, (decimal)PreferedDiscount, ApplyPromo, "NORMAL", true, out status);
                }
                pos.myOrderHdr.Remark = "Queue No : " + transHdr.QueueNo + " , Cashier: " + uploadOrder_CashierID;
                pos.myOrderDet[0].LineInfo = "Queue No : " + transHdr.QueueNo + " , Cashier: " + uploadOrder_CashierID;
                #endregion


            }
            catch (Exception ex)
            {
                pos = null;
                Logger.writeLog(ex);
                message = ex.Message;
            }


            return pos;
        }

        public static string UploadAppOrderToHold(string jsonInput)
        {
            Logger.writeLog("UploadAppOrderToHold json : " + jsonInput);
            bool isSuccess = false;
            string message = "";
            string result = "";


            try
            {
                var transHdr = new JavaScriptSerializer().Deserialize<AppTransactionHdr>(jsonInput);

                string status = "";

                var pos = InitPOSController(transHdr, out status);
                if (pos == null)
                    throw new Exception(string.IsNullOrEmpty(status) ? "Init POS Failed" : status);

                HoldController instance = new HoldController();
                instance.SaveHold(pos);

                isSuccess = true;
                message = "success";
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.writeLog("UploadAppOrderToHold failed : " + jsonInput);
                Logger.writeLog(ex);
                message = "Error : " + ex.Message;
            }

            var resultData = new
            {
                resultCode = isSuccess ? 0 : -1,
                resultMessage = message
            };
            result = new JavaScriptSerializer().Serialize(resultData);

            Logger.writeLog("UploadAppOrderToHold result : " + result);
            return result;
        }

    }



    public class AppTransactionHdr
    {
        public string RefNo { get; set; }
        public string QueueNo { get; set; }
        public string MembershipNo { get; set; }
        public string Username { get; set; }


        public List<AppTransactionDet> Items { get; set; }
    }

    public class AppTransactionDet
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
    }

}
