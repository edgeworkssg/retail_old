using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PowerPOS
{
    public class PrivilegesController
    {               

        #region "Constants"

        public const string CASH_RECORDING = "Cash Recording";
        public const string DEDUCT_POINT = "Deduct Point";
        public const string CLOSE_COUNTER = "Check Out";
        public const string VOID_BILL = "Void Bill";
        public const string SELECT_SALES_PERSON = "Select Sales Person";
        public const string GIVE_DISCOUNT = "Give Discount";
        public const string CASH_OUT_COLLECTION_REPORT = "Cash Out Collection Report";
        public const string GIVE_REFUND = "Give Refund";
        public const string GIVE_MANUAL_DISCOUNT = "Give Manual Discount";
        public const string CREATE_APPOINTMENT = "Create Appointment";
        public const string VIEW_APPOINTMENT = "View Appointment";
        public const string ACCEPT_CLOSE_COUNTER = "Accept Close Counter";
        public const string ADD_NEW_ITEM = "Add New Item";
        public const string CLEAR_ORDER = "Clear Order";
        public const string NO_SALES = "No Sales";
        public const string ALLOW_DO_PAST_VOID = "Allow Do Past Void";
        public const string CHANGE_PAST_PAYMENT_TYPE = "Change Past Payment Type";
        //public const string CASH_BILL = "CashBill";
        //public const string INVENTORY_TRANSACTION = "InventoryTransaction";
        //public const string PRODUCT_SETUP = "ProductSetup";
        //public const string VIEW_PAST_BILL = "ViewPastBill";        
        //public const string APPLY_TAX = "ApplyTax";        
        //public const string GIVE_DISCOUNT = "GiveDiscount";
        //public const string CHANGE_UNIT_PRICE = "ChangeUnitPrice";        
        //public const string EDIT_BILL = "Edit Bill";
        //public const string REPRINT_BILL = "RePrint Bill";
        //public const string EDIT_INVENTORY = "Edit Inventory";

        #endregion
        
        public static bool HasPrivilege(string privilegeName, DataTable privileges)
        {
            try
            {
                for (int i = 0; i < privileges.Rows.Count; i++)
                {
                    if (privilegeName.ToUpper() == privileges.Rows[i]["PrivilegeName"].ToString().ToUpper())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (PrivilegesControllerException ex)
            {
               Logger.writeLog(ex);
               return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool IsPageInPrivilege(string FormName, DataTable privileges)
        {
            try
            {
                if (FormName.Contains("CRReport.aspx?r="))
                {
                    string reportName = FormName.Replace("CRReport.aspx?r=", string.Empty);
                    string[] rList = reportName.Split('&');
                    string rname = rList[0];
                    if (privileges.Select("FormName ='" + rname + "'").Length > 0)
                    { return true; }
                    else
                    { return false; }
                }
                else if (privileges.Select("FormName ='" + FormName + "'").Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (PrivilegesControllerException ex)
            {
                Logger.writeLog(ex);
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        public class PrivilegesControllerException : ApplicationException
        {
            // Default constructor
            public PrivilegesControllerException()
            {
            }
            // Constructor accepting a single string message
            public PrivilegesControllerException(string message)
                : base(message)
            {
            }
            // Constructor accepting a string message and an
            // inner exception which will be wrapped by this
            // custom exception class
            public PrivilegesControllerException(string message,
            Exception inner)
                : base(message, inner)
            {
            }
        } 
    }
}
