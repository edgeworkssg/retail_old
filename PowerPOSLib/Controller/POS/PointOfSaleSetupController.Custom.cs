using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SubSonic;
using PowerPOS;
using PowerPOS.Container;
using PowerPOS.Controller;
using System.Linq;

/// <summary>
/// Summary description for PointOfSaleController
/// </summary>
namespace PowerPOS
{
    public partial class PointOfSaleController
    {
        public const string XMLFILENAME = "\\PointOfSaleInfo.XML";
        /*
        public static void SavePointOfSaleInfo(string PointOfSaleName)
        {
            try
            {                
                PointOfSale myPointOfSale = new PointOfSale(PointOfSale.Columns.PointOfSaleName, PointOfSaleName);
                if (myPointOfSale == null || myPointOfSale.PointOfSaleID == 0)
                {
                    return;
                }
                PointOfSaleInfo.PointOfSaleID = myPointOfSale.PointOfSaleID;
                PointOfSaleInfo.PointOfSaleName = myPointOfSale.PointOfSaleName;
                PointOfSaleInfo.OutletName = myPointOfSale.OutletName;
                PointOfSaleInfo.InventoryLocationID = PointOfSaleController.GetInventoryLocation(PointOfSaleInfo.PointOfSaleID);

                DataSet ds = new DataSet();
                DataRow[] dr;
                

                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);

                dr = ds.Tables[0].Select();
                if (dr != null)
                {
                    dr[0]["PointOfSaleID"] = PointOfSaleInfo.PointOfSaleID;
                    dr[0]["NETsTerminalID"] = PointOfSaleInfo.NETsTerminalID;
                }
                else
                {
                    throw new Exception("File PointOfSale.XML file is invalid.");
                }
                ds.WriteXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);  
            }
            catch (PointOfSaleControllerException ex) //application level exception
            {
                Logger.writeLog("SetPointOfSaleInfo");
                Logger.writeLog(ex);
            }
            catch (Exception ex) //system exception
            {
                Logger.writeLog("SetPointOfSaleInfo");
                Logger.writeLog(ex);
            }
        }
        public static void GetPointOfSaleInfo()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
                PointOfSaleInfo.PointOfSaleID = int.Parse(ds.Tables[0].Rows[0]["PointOfSaleID"].ToString());                
                PointOfSaleInfo.NETsTerminalID = ds.Tables[0].Rows[0]["NETSTerminalID"].ToString();
                PointOfSale myPointOfSale = new PointOfSale(PointOfSaleInfo.PointOfSaleID);
                PointOfSaleInfo.PointOfSaleName = myPointOfSale.PointOfSaleName;
                PointOfSaleInfo.DepartmentID = myPointOfSale.DepartmentID.Value;
                PointOfSaleInfo.OutletName = myPointOfSale.OutletName;
                PointOfSaleInfo.InventoryLocationID = PointOfSaleController.GetInventoryLocation(PointOfSaleInfo.OutletName);

                QuickAccessCategoryGroup = QuickAccessCategoryController.FetchCategories
                (PointOfSaleInfo.CategoryGroupName, PointOfSaleInfo.PointOfSaleID);
            }
            catch (PointOfSaleControllerException ex) //application level exception
            {
                Logger.writeLog("SetPointOfSaleInfo");
                Logger.writeLog(ex);
            }
            catch (Exception ex) //system exception
            {
                Logger.writeLog("SetPointOfSaleInfo");
                Logger.writeLog(ex);
            }
        }
        */
        public static void SaveNETSTerminalID(string NetsID)
        {
            Query qr = Setting.CreateQuery();
            qr.QueryType = QueryType.Update;
            qr.AddUpdateSetting(Setting.Columns.NETSTerminalID, NetsID);
            qr.Execute();
        }
        public static void SavePointOfSaleID(int POSID)
        {
            Query qr = Setting.CreateQuery();
            qr.QueryType = QueryType.Update;
            qr.AddUpdateSetting(Setting.Columns.PointOfSaleID, POSID);
            qr.Execute();
        }
        public static void SavePointOfSaleInfo
            (
                int PointOfSaleID,
                string NETSTerminalID,
                int EZLinkTerminalID,
                string EZLinkTerminalPwd,
                string COMPort,
                int DataBits,
                int BaudRate,
                int HandShake,
                int Parity,
                int StopBits,
                bool IsEZLinkTerminal,
                bool printQuickCashReceipt,
                bool printEZLinkReceipt,
                bool printQuickCashWithEZLinkReceipt,
                bool promptSalesPerson,
                bool useMembership,
                bool allowLineDisc,
                bool allowOverallDisc,
                bool allowChangeCashier,
                bool allowFeedBack
            )
        {
            SettingCollection st = new SettingCollection();
            st.Load();

            if (st.Count == 0)
            {
                Logger.writeLog("Setting table has not been set/data deleted");
                Setting stTmp = new Setting();
                stTmp.PointOfSaleID=0;
                stTmp.WsUrl ="http://localhost:8080/";
                stTmp.NETSTerminalID="";
                stTmp.EZLinkTerminalID=1002001;
                stTmp.IsEZLinkTerminal=false;
                stTmp.EZLinkTerminalPwd="888888";
                stTmp.EZLinkCOMPort="COM1";
                stTmp.EZLinkBaudRate=9600;
                stTmp.EZLinkDataBits=8;
                stTmp.EZLinkParity=0;
                stTmp.EZLinkStopBits=1;
                stTmp.EZLinkHandShake=0;
                stTmp.PrintQuickCashReceipt=false;
                stTmp.PrintEZLinkReceipt=false;
                stTmp.PrintQuickCashWithEZLink=false;
                stTmp.PromptSalesPerson=true ;
                stTmp.UseMembership=true;
                stTmp.AllowLineDisc=true;
                stTmp.AllowOverallDisc=true;
                stTmp.AllowChangeCashier=true;
                stTmp.AllowFeedBack=true;
                stTmp.SQLServerName="localhost\\SQLEXPRESS";
                stTmp.DBName="";
                stTmp.IntegrateWithInventory=false;
                stTmp.Save();
                st = new SettingCollection();
                st.Add(stTmp);
            }

            try
            {

                st[0].PointOfSaleID = PointOfSaleID;
                st[0].NETSTerminalID = NETSTerminalID;
                st[0].EZLinkTerminalID = EZLinkTerminalID;
                st[0].EZLinkTerminalPwd = EZLinkTerminalPwd;
                st[0].EZLinkCOMPort = COMPort;
                st[0].EZLinkDataBits = DataBits;
                st[0].EZLinkBaudRate = BaudRate;
                st[0].EZLinkHandShake = HandShake;
                st[0].EZLinkParity = Parity;
                st[0].EZLinkStopBits = StopBits;
                st[0].IsEZLinkTerminal = IsEZLinkTerminal;
                st[0].PrintQuickCashReceipt = printQuickCashReceipt;
                st[0].PrintEZLinkReceipt = printEZLinkReceipt;
                st[0].PrintQuickCashWithEZLink = printQuickCashWithEZLinkReceipt;
                st[0].PromptSalesPerson = promptSalesPerson;
                st[0].UseMembership = useMembership;
                st[0].AllowFeedBack = allowFeedBack;
                st[0].AllowOverallDisc = allowOverallDisc;
                st[0].AllowLineDisc = allowLineDisc;
                st[0].AllowChangeCashier = allowChangeCashier;
                st[0].Save("SYSTEM");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
        public static void GetPointOfSaleInfo()
        {
            try
            {
                //Print Setting
                PrintSettingInfo.receiptSetting = ReceiptSettingController.GetReceiptPrinterSetting();                

                //Load Sync Related
                SyncClientController.Load_WS_URL();
                
                GSTOverride.LoadGSTRule();
                string status;
                PaymentTypesController.LoadPaymentTypes(out status);
                DeliveryTimeController.LoadDeliveryTimes(out status);
                
                SettingCollection st = new SettingCollection();
                st.Load();
                if (st.Count == 0)
                {
                    //throw new Exception("Setting table has not been set/data deleted");
                    
                        Logger.writeLog("Setting table has not been set/data deleted");
                        Setting stTmp = new Setting();
                        stTmp.PointOfSaleID = 0;
                        stTmp.WsUrl = "http://localhost:8080/";
                        stTmp.NETSTerminalID = "";
                        stTmp.EZLinkTerminalID = 1002001;
                        stTmp.IsEZLinkTerminal = false;
                        stTmp.EZLinkTerminalPwd = "888888";
                        stTmp.EZLinkCOMPort = "COM1";
                        stTmp.EZLinkBaudRate = 9600;
                        stTmp.EZLinkDataBits = 8;
                        stTmp.EZLinkParity = 0;
                        stTmp.EZLinkStopBits = 1;
                        stTmp.EZLinkHandShake = 0;
                        stTmp.PrintQuickCashReceipt = false;
                        stTmp.PrintEZLinkReceipt = false;
                        stTmp.PrintQuickCashWithEZLink = false;
                        stTmp.PromptSalesPerson = true;
                        stTmp.UseMembership = true;
                        stTmp.AllowLineDisc = true;
                        stTmp.AllowOverallDisc = true;
                        stTmp.AllowChangeCashier = true;
                        stTmp.AllowFeedBack = true;
                        stTmp.SQLServerName = "localhost\\SQLEXPRESS";
                        stTmp.DBName = "";
                        stTmp.IntegrateWithInventory = false;
                        stTmp.Save();
                        st = new SettingCollection();
                        st.Add(stTmp);
                    
                }
                SyncClientController.WS_URL = st[0].WsUrl;
                PointOfSaleInfo.WS_URL = st[0].WsUrl;
                PointOfSaleInfo.PointOfSaleID = st[0].PointOfSaleID;
                PointOfSale myPointOfSale = new PointOfSale(PointOfSaleInfo.PointOfSaleID);
            
                PointOfSaleInfo.PointOfSaleName = myPointOfSale.PointOfSaleName;
                PointOfSaleInfo.MembershipPrefixCode = myPointOfSale.MembershipCode;
                //PointOfSaleInfo.DepartmentID = myPointOfSale.DepartmentID.Value;
                PointOfSaleInfo.DepartmentID = 0;
                PointOfSaleInfo.OutletName = myPointOfSale.OutletName;
                PointOfSaleInfo.InventoryLocationID = PointOfSaleController.GetInventoryLocation(PointOfSaleInfo.OutletName);
                PointOfSaleInfo.InventoryLocationName = (new InventoryLocation(PointOfSaleInfo.InventoryLocationID)).InventoryLocationName.ToString();
                PointOfSaleInfo.QuickAccessCategory =
                QuickAccessController.FetchCategories(myPointOfSale.PointOfSaleID,
                myPointOfSale.QuickAccessGroupID);
                
                PointOfSaleInfo.IsEZLinkTerminal = st[0].IsEZLinkTerminal;
                PointOfSaleInfo.EZLinkTerminalPwd = st[0].EZLinkTerminalPwd.ToString();
                PointOfSaleInfo.EZLinkTerminalID = st[0].EZLinkTerminalID.ToString();
                PointOfSaleInfo.NETsTerminalID = st[0].NETSTerminalID;
                PointOfSaleInfo.promptSalesPerson = st[0].PromptSalesPerson;
                PointOfSaleInfo.useMembership = st[0].UseMembership;
                PointOfSaleInfo.allowLineDisc = st[0].AllowLineDisc;
                PointOfSaleInfo.allowChangeCashier = st[0].AllowChangeCashier;
                PointOfSaleInfo.DBName = st[0].DBName;
                PointOfSaleInfo.SQLServerName = st[0].SQLServerName;
                
                PrintSettingInfo.PrintQuickCashReceipt = st[0].PrintQuickCashReceipt;
                PrintSettingInfo.PrintQuickEZLinkReceipt = st[0].PrintEZLinkReceipt;
                PrintSettingInfo.PrintQuickCashWithEZLinkReceipt = st[0].PrintQuickCashWithEZLink;

                if (st[0].IntegrateWithInventory.HasValue)
                {
                    PointOfSaleInfo.IntegrateWithInventory = st[0].IntegrateWithInventory.Value;
                }
                else
                {
                    PointOfSaleInfo.IntegrateWithInventory = false;
                }
                //Company Info
                CompanyCollection cmpCol = new CompanyCollection();
                cmpCol.Load();
                if (cmpCol.Count > 0)
                {
                    CompanyInfo.CompanyName = cmpCol[0].CompanyName;
                    CompanyInfo.GSTRegNo = cmpCol[0].GSTRegNo;
                    CompanyInfo.ReceiptName = cmpCol[0].ReceiptName;
                }   
             
                //set the comm port                
                EZLinkComPortInfo.COMPort = st[0].EZLinkCOMPort;
                EZLinkComPortInfo.BaudRate = st[0].EZLinkBaudRate.Value;
                EZLinkComPortInfo.DataBits = st[0].EZLinkDataBits.Value;
                switch (st[0].EZLinkHandShake.Value)
                {
                    case (int)System.IO.Ports.Handshake.None:
                        EZLinkComPortInfo.HandShake = (int)System.IO.Ports.Handshake.None;
                        break;
                    case (int)System.IO.Ports.Handshake.RequestToSend:
                        EZLinkComPortInfo.HandShake = System.IO.Ports.Handshake.RequestToSend;
                        break;
                    case (int)System.IO.Ports.Handshake.RequestToSendXOnXOff:
                        EZLinkComPortInfo.HandShake = System.IO.Ports.Handshake.RequestToSendXOnXOff;
                        break;
                    case (int)System.IO.Ports.Handshake.XOnXOff:
                        EZLinkComPortInfo.HandShake = System.IO.Ports.Handshake.XOnXOff;
                        break;
                }
                switch (st[0].EZLinkParity)
                {
                    case (int)System.IO.Ports.Parity.None:
                        EZLinkComPortInfo.Parity = System.IO.Ports.Parity.None;
                        break;
                    case (int)System.IO.Ports.Parity.Even:
                        EZLinkComPortInfo.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case (int)System.IO.Ports.Parity.Odd:
                        EZLinkComPortInfo.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    case (int)System.IO.Ports.Parity.Space:
                        EZLinkComPortInfo.Parity = System.IO.Ports.Parity.Space;
                        break;
                    case (int)System.IO.Ports.Parity.Mark:
                        EZLinkComPortInfo.Parity = System.IO.Ports.Parity.Mark;
                        break;
                }

                switch (st[0].EZLinkStopBits)
                {
                    case (int)System.IO.Ports.StopBits.None:
                        EZLinkComPortInfo.StopBits = System.IO.Ports.StopBits.None;
                        break;
                    case (int)System.IO.Ports.StopBits.One:
                        EZLinkComPortInfo.StopBits = System.IO.Ports.StopBits.One;
                        break;
                    case (int)System.IO.Ports.StopBits.OnePointFive:
                        EZLinkComPortInfo.StopBits = System.IO.Ports.StopBits.OnePointFive;
                        break;
                    case (int)System.IO.Ports.StopBits.Two:
                        EZLinkComPortInfo.StopBits = System.IO.Ports.StopBits.Two;
                        break;
                }
                /*
                if (PointOfSaleInfo.IsEZLinkTerminal)
                {
                    try
                    {
                        EZlinkMessageController.openCommPort();
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                    }
                }*/
            }
            catch (PointOfSaleControllerException ex) //application level exception
            {
                //Logger.writeLog("SetPointOfSaleInfo");
                Logger.writeLog(ex);
            }
            catch (Exception ex) //system exception
            {
                //Logger.writeLog("SetPointOfSaleInfo");
                Logger.writeLog(ex);
            }
        }
        public PointOfSaleController()
        {
            //
            // TODO: Add constructor logic here
            //

        }        
        public static ArrayList FetchPointOfSaleNames()
        {
            ArrayList ar = new ArrayList();
            ar.Add("");
            Query qr = PointOfSale.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = PointOfSale.Columns.PointOfSaleName;
            IDataReader rdr = qr.ExecuteReader();
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return ar;
        }

        public static ArrayList FetchPointOfSaleNames(string DeptID)
        {
            if (DeptID == "")
            {
                return FetchPointOfSaleNames();
            }
            ArrayList ar = new ArrayList();
            ar.Add("");
            Query qr = PointOfSale.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = PointOfSale.Columns.PointOfSaleName;
            qr.AddWhere(PointOfSale.Columns.DepartmentID, DeptID);
            IDataReader rdr = qr.ExecuteReader();
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return ar;
        }
        public static ArrayList FetchOutletNames()
        {
            ArrayList ar = new ArrayList();
            ar.Add("ALL");

            Query qr = Outlet.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = PointOfSale.Columns.OutletName;
            IDataReader rdr = qr.ExecuteReader();
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }
            return ar;
        }
        public static ListItem[] FetchOutletNames(bool AskForBreakDown)
        {
            ListItemCollection ar = new ListItemCollection();
            ListItem[] lrList;
            ListItem lr = new ListItem();
            lr.Text = "ALL";
            lr.Value = "";
            ar.Add(lr);

            if (AskForBreakDown)
            {
                lr = new ListItem();
                lr.Text = "ALL - BreakDown";
                lr.Value = "%";
                ar.Add(lr);
            }

            Query qr = Outlet.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = PointOfSale.Columns.OutletName;
            IDataReader rdr = qr.ExecuteReader();
            while (rdr.Read())
            {
                lr = new ListItem();
                lr.Text = rdr.GetValue(0).ToString();
                lr.Value = rdr.GetValue(0).ToString();                
                ar.Add(lr);
            }
            
            lrList = new ListItem[ar.Count];
            for (int i = 0; i < ar.Count; i++)
            {
                lrList[i] = ar[i];
            }
            rdr.Close();
            return lrList;
        }

        public static ListItem[] FetchOutletNames(bool includeBreakdown, string userName)
        {
            var outletData = OutletController.FetchByUserNameForReport(includeBreakdown, true, userName);
            ListItem[] result = new ListItem[outletData.Count];
            for (int i = 0; i < outletData.Count; i++)
            {
                ListItem lr = new ListItem();
                lr.Text = outletData[i].OutletName;
                lr.Value = outletData[i].OutletName;
                //lr.Selected = (outletData[i].OutletName == "ALL - BreakDown");
                result[i] = lr;
            }
            return result;
        }

        public static ListItem[] FetchPointOfSaleNames(bool includeBreakdown, string userName)
        {
            var outletData = PointOfSaleController.FetchByUserNameForReport(includeBreakdown, true, userName,"ALL");
            ListItem[] result = new ListItem[outletData.Count];
            for (int i = 0; i < outletData.Count; i++)
            {
                ListItem lr = new ListItem();
                lr.Text = outletData[i].PointOfSaleName;
                lr.Value = outletData[i].PointOfSaleID.ToString();
                //lr.Selected = (outletData[i].OutletName == "ALL - BreakDown");
                result[i] = lr;
            }
            return result;
        }

        public static ListItem[] FetchPointOfSaleNames(bool includeBreakdown, string userName, string outletName)
        {
            var outletData = PointOfSaleController.FetchByUserNameForReport(includeBreakdown, true, userName, outletName);
            ListItem[] result = new ListItem[outletData.Count];
            for (int i = 0; i < outletData.Count; i++)
            {
                ListItem lr = new ListItem();
                lr.Text = outletData[i].PointOfSaleName;
                lr.Value = outletData[i].PointOfSaleID.ToString();
                //lr.Selected = (outletData[i].OutletName == "ALL - BreakDown");
                result[i] = lr;
            }
            return result;
        }

        public static int GetInventoryLocation(int PointOfSaleID)
        {
            PointOfSale pt = new PointOfSale(PointOfSaleID);
            if (pt != null && pt.Outlet != null && pt.Outlet.InventoryLocationID.HasValue)
                return pt.Outlet.InventoryLocationID.Value;
            else
                return -1;
        }

        public static int GetInventoryLocation(string OutletName)
        {
            Outlet myOutlet = new Outlet(OutletName);
            if (myOutlet != null && myOutlet.InventoryLocationID != null)
            {
                return (int)myOutlet.InventoryLocationID;
            }
            return -1;
        }
        public static ListItem[] FetchOutletNamesForDropDown()
        {
            ListItemCollection ar = new ListItemCollection();
            ListItem[] lrList;
            ListItem lr;

            Query qr = Outlet.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = PointOfSale.Columns.OutletName;
            IDataReader rdr = qr.ExecuteReader();
            while (rdr.Read())
            {
                lr = new ListItem();
                lr.Text = rdr.GetValue(0).ToString();
                lr.Value = rdr.GetValue(0).ToString();
                ar.Add(lr);
            }
            rdr.Close();
            lrList = new ListItem[ar.Count];
            for (int i = 0; i < ar.Count; i++)
            {
                lrList[i] = ar[i];
            }

            return lrList;
        }
        public static DataTable GetCountry(string Region)
        {
            if (Region == "ALL") Region = "%";

            string SQLString =
                "DECLARE @Region VARCHAR(100); " +
                "SET @Region = '" + Region + "'; " +
                "SELECT DISTINCT UserFld1 AS Val, UserFld1 AS Txt " +
                "FROM PointOfSale " +
                "WHERE ISNULL(UserFld2,'') LIKE @Region ";
            //"ORDER BY PointOfSaleName, PointOfSaleID ";

            DataTable Rst = new DataTable();
            Rst.Load(DataService.GetReader(new QueryCommand(SQLString)));

            DataRow Rw = Rst.NewRow();
            Rw["Val"] = "ALL";
            Rw["Txt"] = "ALL";
            Rst.Rows.InsertAt(Rw, 0);

            return Rst;
        }
        public static DataTable GetPointOfSale(string Country)
        {
            if (Country == "ALL") Country = "%";

            string SQLString =
                "DECLARE @Country VARCHAR(100); " +
                "SET @Country = '" + Country + "'; " +
                "SELECT PointOfSaleID AS Val, PointOfSaleName AS Txt " +
                "FROM PointOfSale " +
                "WHERE ISNULL(UserFld1,'') LIKE @Country " +
                "ORDER BY PointOfSaleName, PointOfSaleID ";

            DataTable Rst = new DataTable();
            Rst.Load(DataService.GetReader(new QueryCommand(SQLString)));

            DataRow Rw = Rst.NewRow();
            Rw["Val"] = 0;
            Rw["Txt"] = "ALL";
            Rst.Rows.InsertAt(Rw, 0);

            return Rst;
        }
        public static DataTable GetTemplates(string Country)
        {
            if (Country == "ALL") Country = "%";

            string SQLString =
                "DECLARE @Country VARCHAR(100); " +
                "SET @Country = '" + Country + "'; " +
                "SELECT DISTINCT TemplateName AS Val, TemplateName AS Txt " +
                "FROM DemandTemplate " +
                "WHERE Country LIKE @Country ";
            //"ORDER BY PointOfSaleName, PointOfSaleID ";

            DataTable Rst = new DataTable();
            Rst.Load(DataService.GetReader(new QueryCommand(SQLString)));

            /*
            DataRow Rw = Rst.NewRow();
            Rw["Val"] = "ALL";
            Rw["Txt"] = "--ALL--";
            Rst.Rows.InsertAt(Rw, 0);
            */

            return Rst;
        }
        public static ArrayList FetchPointOfSaleNamesByOutlet(string OutletName)
        {
            if (OutletName == "")
            {
                return FetchPointOfSaleNames();
            }
            ArrayList ar = new ArrayList();
            ar.Add("");
            Query qr = PointOfSale.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = PointOfSale.Columns.PointOfSaleName;
            qr.AddWhere(PointOfSale.Columns.OutletName, OutletName);
            IDataReader rdr = qr.ExecuteReader();
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }
            return ar;

        }
    }
    public class PointOfSaleControllerException : ApplicationException
    {
        // Default constructor
        public PointOfSaleControllerException()
        {
        }
        // Constructor accepting a single string message
        public PointOfSaleControllerException(string message)
            : base(message)
        {
        }
        // Constructor accepting a string message and an
        // inner exception which will be wrapped by this
        // custom exception class
        public PointOfSaleControllerException(string message,
        Exception inner)
            : base(message, inner)
        {
        }
    }   
}