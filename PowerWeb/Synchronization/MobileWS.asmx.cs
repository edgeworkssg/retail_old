using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Web.Services.Protocols;
using System.IO;
//using System.Xml.Linq;
using SubSonic;
using PowerPOS;
using PowerPOS.Container;
using PowerWeb.BLL.Helper;
using CrystalDecisions.CrystalReports.Engine;
using SpreadsheetLight;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace PowerWeb.Synchronization
{
    /// <summary>
    /// Summary description for MobileWS
    /// </summary>
    [WebService(Namespace = "http://www.edgeworks.com.sg/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    //[ScriptService]
    public class MobileWS : System.Web.Services.WebService
    {
        public const string ERR_CLINIC_FROZEN = "This Inventory Location IS frozen. No changes can be made to database.";
        public const string ERR_CLINIC_NOT_FROZEN = "This Inventory Location is NOT frozen. No stock take activity can be made.";
        public const string ERR_CLINIC_DELETED = "This Inventory Location IS deleted.";
        public const int DIGIT_DECIMAL = 2;

        public MobileWS()
        {
            //InitializeComponent(); 
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        }

        public string[] GoodsOrderType = new string[] { "Replenish", "Special Order", "Back Order", "Pre Order" };
        #region *) Login
        [WebMethod]
        public string Login(string username, string password)
        {
            LoginResult res = new LoginResult();
            try
            {

                string role = "", deptID = "", status = "";
                UserMst user;

                res.result = UserController.login(username, password, LoginType.Login, out user, out role, out deptID, out status);
                if (res.result)
                {
                    res.UserName = user.UserName;
                    res.DisplayName = user.DisplayName;
                    res.PointOfSaleID = user.PointOfSaleID.ToString();

                    // Change UserToken everytime user log in
                    user.UserToken = Guid.NewGuid().ToString().ToUpper();
                    user.Save();
                    res.UserToken = user.UserToken;

                    //PointOfSaleInfo.PointOfSaleID = user.PointOfSaleID;
                    //PointOfSale pos = new PointOfSale(user.PointOfSaleID);
                    //if (pos != null && pos.Outlet != null && pos.Outlet.InventoryLocation != null && pos.Outlet.InventoryLocation.Deleted.GetValueOrDefault(false) == false)
                    //{
                    //    res.InventoryLocation = pos.Outlet.InventoryLocation;
                    //}
                    //else
                    //{
                    //    //res.UserName = "";
                    //    //res.Role = "";
                    //    //res.DeptID = "";
                    //    //res.PointOfSaleID = "";
                    //    //res.InventoryLocation = null;
                    //    res.result = false;
                    //    res.status = "This user is registered to a Outlet that does not exist or has been deleted.";
                    //    return new JavaScriptSerializer().Serialize(res);
                    //}
                }
                res.Role = role;
                res.DeptID = deptID;
                res.status = status;

                DataTable dtPrivileges = UserController.FetchGroupPrivilegesWithUsername(role, user.UserName);
                List<string> privileges = new List<string>();
                foreach (DataRow dr in dtPrivileges.Rows)
                {
                    privileges.Add(dr["PrivilegeName"].ToString().Trim());
                }

                res.Privileges = privileges;

                CompanyCollection companies = new CompanyCollection().Load();
                if (companies.Count > 0)
                {
                    res.CompanyName = companies[0].CompanyName;
                }
                else
                {
                    res.CompanyName = "";
                }

                return new JavaScriptSerializer().Serialize(res);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                //res.Role = ""; res.DeptID = ""; res.InventoryLocation = null; res.PointOfSaleID = "";
                res.status = ex.Message;
                res.result = false;
                return new JavaScriptSerializer().Serialize(res);
            }
        }

        [WebMethod]
        public string CheckUserToken(string userToken)
        {
            UserMst user = new UserMst(UserMst.UserColumns.UserToken, userToken);
            bool valid = false;
            if (user != null && user.UserToken == userToken)
            {
                PointOfSaleInfo.PointOfSaleID = user.PointOfSaleID;
                PointOfSale pos = new PointOfSale(user.PointOfSaleID);
                if (pos != null && pos.Outlet != null && pos.Outlet.InventoryLocation != null && pos.Outlet.InventoryLocation.Deleted.GetValueOrDefault(false) == false)
                {
                    valid = true;
                }
            }
            var result = new
            {
                valid = valid
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangePassword(string username, string oldpassword, string newpassword)
        {
            string status = "";
            try
            {
                UserController.ChangePassword(username, oldpassword, newpassword, out status);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string LoginWebOrder(string username, string password)
        {
            LoginResult res = new LoginResult();
            try
            {

                string role = "", deptID = "", status = "";
                UserMst user;

                res.result = UserController.login(username, password, LoginType.Login, out user, out role, out deptID, out status);
                if (res.result)
                {
                    res.UserName = user.UserName;
                    res.DisplayName = user.DisplayName;
                    res.PointOfSaleID = user.PointOfSaleID.ToString();

                    // Change UserToken everytime user log in
                    user.UserToken = Guid.NewGuid().ToString().ToUpper();
                    user.Save();
                    res.UserToken = user.UserToken;

                    PointOfSaleInfo.PointOfSaleID = user.PointOfSaleID;
                    PointOfSale pos = new PointOfSale(user.PointOfSaleID);
                    if (pos != null && pos.Outlet != null && pos.Outlet.InventoryLocation != null && pos.Outlet.InventoryLocation.Deleted.GetValueOrDefault(false) == false)
                    {
                        res.InventoryLocation = pos.Outlet.InventoryLocation;
                    }
                    else
                    {
                        //res.UserName = "";
                        //res.Role = "";
                        //res.DeptID = "";
                        //res.PointOfSaleID = "";
                        //res.InventoryLocation = null;
                        res.result = false;
                        res.status = "This user is registered to a Outlet that does not exist or has been deleted.";
                        return new JavaScriptSerializer().Serialize(res);
                    }
                }
                res.Role = role;
                res.DeptID = deptID;
                res.status = status;
                if (res.result)
                {
                    DataTable dtPrivileges = UserController.FetchGroupPrivilegesWithUsername(role, user.UserName);
                    List<string> privileges = new List<string>();
                    foreach (DataRow dr in dtPrivileges.Rows)
                    {
                        privileges.Add(dr["PrivilegeName"].ToString().Trim());
                    }

                    res.Privileges = privileges;
                }

                if (res.Privileges.Contains("Allow Change Inventory Location"))
                {
                    InventoryLocationCollection colinv = new InventoryLocationCollection();
                    colinv.Add(new InventoryLocation() { InventoryLocationID = 0, InventoryLocationName = "ALL" });

                    InventoryLocationCollection col = new InventoryLocationCollection();
                    col.Where(InventoryLocation.Columns.Deleted, false);
                    col.Load();
                    colinv.AddRange(col);

                    res.InventoryLocationCollection = colinv;
                }
                else
                {
                    InventoryLocationCollection col = new InventoryLocationCollection();

                    col.Add(new InventoryLocation() { InventoryLocationID = 0, InventoryLocationName = "ALL" });

                    string[] assignedPOS = user.AssignedOutletList;
                    string query = @"SELECT DISTINCT *
                                    FROM(
                                    SELECT  IL.*
                                    FROM	Outlet OU
		                                    INNER JOIN InventoryLocation IL on OU.InventoryLocationID = IL.InventoryLocationID AND IL.Deleted = 0
                                    WHERE	ISNULL(OU.Deleted,0) = 0
		                                    AND OU.OutletName IN ('{0}')
                                    UNION
                                    SELECT il.*
                                    FROM usermst um
                                    inner join PointOfSale po on um.userint1 = po.pointofsaleid
                                    inner join Outlet o on o.OutletName = po.OutletName
                                    inner join InventoryLocation il on il.InventoryLocationID = o.InventoryLocationID
                                    where um.username = '{1}'
                                    ) IL
                                    ORDER BY IL.InventoryLocationName
                                    ";
                    query = string.Format(query, string.Join("','", assignedPOS), user.UserName);
                    DataTable dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

                    InventoryLocationCollection outcol = new InventoryLocationCollection();
                    outcol.Load(dt);
                    col.AddRange(outcol);

                    res.InventoryLocationCollection = col;
                }

                CompanyCollection companies = new CompanyCollection().Load();
                if (companies.Count > 0)
                {
                    res.CompanyName = companies[0].CompanyName;
                }
                else
                {
                    res.CompanyName = "";
                }

                return new JavaScriptSerializer().Serialize(res);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                //res.Role = ""; res.DeptID = ""; res.InventoryLocation = null; res.PointOfSaleID = "";
                res.status = ex.Message;
                res.result = false;
                return new JavaScriptSerializer().Serialize(res);
            }
        }

        [WebMethod]
        public string LoginWebOrderSupplierPortal(string username, string password)
        {
            LoginResult res = new LoginResult();
            try
            {

                string role = "", deptID = "", status = "";
                UserMst user;

                res.result = UserController.login(username, password, LoginType.Login, out user, out role, out deptID, out status);
                if (res.result)
                {
                    res.UserName = user.UserName;
                    res.DisplayName = user.DisplayName;
                    res.PointOfSaleID = user.PointOfSaleID.ToString();

                    // Change UserToken everytime user log in
                    user.UserToken = Guid.NewGuid().ToString().ToUpper();
                    user.Save();
                    res.UserToken = user.UserToken;

                    res.isSupplier = user.IsSupplier;
                    res.isRestrictedSupplierList = user.IsRestrictedSupplierList;
                    res.isUseUserPortal = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseSupplierPortal), false);
                    res.isUseTransferApproval = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.UseTransferApproval), false);
                }
                else
                    throw new Exception(status);

                res.Role = role;
                res.DeptID = deptID;
                res.status = status;


                if (res.result)
                {
                    DataTable dtPrivileges = UserController.FetchGroupPrivilegesWithUsername(role, user.UserName);
                    List<string> privileges = new List<string>();
                    foreach (DataRow dr in dtPrivileges.Rows)
                    {
                        privileges.Add(dr["PrivilegeName"].ToString().Trim());
                    }

                    res.Privileges = privileges;
                }

                bool isShowUserOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.ShowOutlet), false);

                if (res.Privileges.Contains("Allow Change Inventory Location") || !isShowUserOutlet)
                {
                    InventoryLocationCollection colinv = new InventoryLocationCollection();


                    InventoryLocationCollection col = new InventoryLocationCollection();
                    col.Where(InventoryLocation.Columns.Deleted, false);
                    col.Load();
                    if (col.Count() > 1)
                        colinv.Add(new InventoryLocation() { InventoryLocationID = 0, InventoryLocationName = "ALL" });

                    colinv.AddRange(col);
                    res.InventoryLocationCollection = colinv;
                }
                else
                {
                    InventoryLocationCollection col = new InventoryLocationCollection();



                    string[] assignedPOS = user.AssignedOutletList;
                    string query = @"SELECT DISTINCT *
                                    FROM(
                                    SELECT  IL.*
                                    FROM	Outlet OU
		                                    INNER JOIN InventoryLocation IL on OU.InventoryLocationID = IL.InventoryLocationID AND IL.Deleted = 0
                                    WHERE	ISNULL(OU.Deleted,0) = 0
		                                    AND OU.OutletName IN ('{0}')
                                    UNION
                                    SELECT il.*
                                    FROM usermst um
                                    inner join PointOfSale po on um.userint1 = po.pointofsaleid
                                    inner join Outlet o on o.OutletName = po.OutletName
                                    inner join InventoryLocation il on il.InventoryLocationID = o.InventoryLocationID
                                    where um.username = '{1}'
                                    ) IL
                                    ORDER BY IL.InventoryLocationName
                                    ";
                    query = string.Format(query, string.Join("','", assignedPOS), user.UserName);
                    DataTable dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

                    InventoryLocationCollection outcol = new InventoryLocationCollection();
                    outcol.Load(dt);

                    if (outcol.Count() > 1)
                        col.Add(new InventoryLocation() { InventoryLocationID = 0, InventoryLocationName = "ALL" });

                    col.AddRange(outcol);

                    res.InventoryLocationCollection = col;
                }

                if (res.InventoryLocationCollection.Count() > 0)
                {
                    res.InventoryLocation = res.InventoryLocationCollection[0];
                }
                else
                {
                    res.InventoryLocation = new InventoryLocation() { InventoryLocationID = 0, InventoryLocationName = "ALL" };
                    res.InventoryLocationCollection.Add(new InventoryLocation() { InventoryLocationID = 0, InventoryLocationName = "ALL" });
                }

                CompanyCollection companies = new CompanyCollection().Load();
                if (companies.Count > 0)
                {
                    res.CompanyName = companies[0].CompanyName;
                }
                else
                {
                    res.CompanyName = "";
                }

                return new JavaScriptSerializer().Serialize(res);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                //res.Role = ""; res.DeptID = ""; res.InventoryLocation = null; res.PointOfSaleID = "";
                res.status = ex.Message;
                res.result = false;
                return new JavaScriptSerializer().Serialize(res);
            }
        }

        [WebMethod(CacheDuration = 0)]
        public DataSet GetDataTable(string tableName, bool syncAll)
        {
            return SynchronizationController.GetDataTable(tableName, syncAll);
        }

        [WebMethod]
        public string GetNotifications(string username)
        {
            string status = "";
            List<object> notifications = new List<object>();

            try
            {
                bool allowChangeLocation = false;
                int invLocID = 0;

                UserMst user = new UserMst(username);
                if (user == null || user.GroupName == 0 || user.UserGroup == null || string.IsNullOrEmpty(user.UserGroup.GroupName))
                {
                    status = "User or role not found.";
                }

                PointOfSale pos = new PointOfSale(user.PointOfSaleID);
                if (pos != null && pos.Outlet != null && pos.Outlet.InventoryLocation != null)
                {
                    invLocID = pos.Outlet.InventoryLocation.InventoryLocationID;
                }
                else
                {
                    status = "Invalid Point Of Sale ID.";
                }

                if (status == "")
                {
                    DataTable dtPrivileges = UserController.FetchGroupPrivilegesWithUsername(user.UserGroup.GroupName, user.UserName);
                    List<string> privileges = new List<string>();
                    foreach (DataRow dr in dtPrivileges.Rows)
                    {
                        privileges.Add(dr["PrivilegeName"].ToString().Trim());
                    }

                    if (privileges.Exists(p => p == "Allow Change Inventory Location"))
                        allowChangeLocation = true;

                    if (privileges.Exists(p => p == "Goods Ordering"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Order")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Pending");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " pending item(s) in Goods Order.",
                                menu = "goodsordering"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Order Approval"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Order")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Submitted");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " submitted item(s) in Order Approval.",
                                menu = "orderapproval"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Goods Receive"))
                    {
                        // For Goods Order
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Order")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Approved");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " approved order(s) waiting for goods receive.",
                                menu = "goodsreceive"
                            };
                            notifications.Add(notifObj);
                        }

                        // For Stock Transfer
                        qry = new Select().From("PurchaseOrderHeader")
                                  .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Transfer")
                                  .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Submitted");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " stock transfer(s) waiting for goods receive.",
                                menu = "goodsreceive"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Stock Return"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Return")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Pending");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " pending item(s) in Stock Return.",
                                menu = "stockreturn"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Return Approval"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Return")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Submitted");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " submitted item(s) in Return Approval.",
                                menu = "returnapproval"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Stock Adjustment"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).Like("Adjustment%")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Pending");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " pending item(s) in Stock Adjustment.",
                                menu = "stockadjustment"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Adjustment Approval"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).Like("Adjustment%")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Submitted");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " submitted item(s) in Adjustment Approval.",
                                menu = "adjustmentapproval"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Stock Transfer"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Transfer")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Pending");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " pending item(s) in Stock Transfer.",
                                menu = "stocktransfer"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Stock Take Approval"))
                    {
                        SqlQuery qry = new Select().From("StockTakeDoc")
                                       .Where("Status").IsEqualTo("Pending");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " stock take document(s) waiting for approval.",
                                menu = "stocktakeapproval"
                            };
                            notifications.Add(notifObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                notifications = notifications,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string LoginByEmail(string email, string password)
        {
            LoginResult res = new LoginResult();
            string username = "";
            string sql = "SELECT UserName FROM UserMst WHERE userfld8 = @Email and Password = @Password";

            QueryCommand qc = new QueryCommand(sql);
            qc.AddParameter("@Email", email);
            qc.AddParameter("@Password", UserController.EncryptData(password));

            IDataReader rdr = DataService.GetReader(qc);
            if (rdr.Read())
            {
                username = rdr["UserName"].ToString();

                return Login(username.ToLower(), password);
            }

            res.status = "Invalid Email or Password";
            res.result = false;
            return new JavaScriptSerializer().Serialize(res);
        }

        #endregion

        #region *) Optimal Stock / ItemBaseLevel

        [WebMethod]
        public string DeleteItemOptimalStock(string data)
        {
            var status = "";
            try
            {
                List<Viewitembaselevel> dataFiles = new JavaScriptSerializer().Deserialize<List<Viewitembaselevel>>(data);
                foreach (Viewitembaselevel dataFile in dataFiles)
                {
                    ItemBaseLevel id = new ItemBaseLevel(dataFile.BaseLevelID);
                    id.Deleted = true;
                    id.Save("Sync");
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string CopyItemOptimalStock(int fromInventoryLocationID, int toInventoryLocationID, bool overwrite)
        {
            var status = "";

            try
            {
                //load the item from inventory loc id
                ItemBaseLevelCollection fromCol = new ItemBaseLevelCollection();
                fromCol.Where(ItemBaseLevel.Columns.InventoryLocationID, fromInventoryLocationID);
                fromCol.Where(ItemBaseLevel.Columns.Deleted, false);
                fromCol.Load();

                if (fromCol.Count > 0)
                {
                    QueryCommandCollection cmd = new QueryCommandCollection();
                    foreach (ItemBaseLevel IbFrom in fromCol)
                    {
                        //check exist
                        ItemBaseLevelCollection toCol = new ItemBaseLevelCollection();
                        toCol.Where(ItemBaseLevel.Columns.InventoryLocationID, toInventoryLocationID);
                        toCol.Where(ItemBaseLevel.Columns.Deleted, false);
                        toCol.Where(ItemBaseLevel.Columns.ItemNo, IbFrom.ItemNo);
                        toCol.Load();

                        if (toCol.Count > 0)
                        {
                            //Exist
                            if (overwrite)
                            {
                                ItemBaseLevel UpdItem = toCol[0];
                                UpdItem.BaseLevelQuantity = IbFrom.BaseLevelQuantity;
                                cmd.Add(UpdItem.GetUpdateCommand("Copy"));
                            }
                        }
                        else
                        {
                            ItemBaseLevel newItem = new ItemBaseLevel();
                            newItem.InventoryLocationID = toInventoryLocationID;
                            newItem.ItemNo = IbFrom.ItemNo;
                            newItem.BaseLevelQuantity = IbFrom.BaseLevelQuantity;
                            newItem.Deleted = false;
                            cmd.Add(newItem.GetInsertCommand("Copy"));
                        }
                    }
                    DataService.ExecuteTransaction(cmd);
                }

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetItemBaseLevelList(string filter, int skip, int take, string sortBy, bool isAscending)
        {

            var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);

            string status = "";
            ViewitembaselevelCollection col = new ViewitembaselevelCollection();
            var tmpResult = (from it in col select it);
            try
            {
                string CategoryName = "";

                if (!param.ContainsKey("categoryName") || (param.ContainsKey("categoryName") && param["categoryName"].ToUpper() == ""))
                {
                    CategoryName = "ALL";
                }
                else
                {
                    CategoryName = param["categoryName"];
                }

                string query = @"SELECT * FROM ViewItemBaseLevel 
                                WHERE (InventoryLocationName = '{0}' or '{0}' = 'ALL')
                                      AND (Category = '{1}' or '{1}' = 'ALL')              
                                       AND ItemNo + ISNULL(ItemName,'') like '%' + '{2}' + '%' 
                                Order By {3} {4}";

                query = string.Format(query, !param.ContainsKey("inventoryLocation") ? "ALL" : param["inventoryLocation"]
                                           , CategoryName
                                           , param.ContainsKey("itemNo") ? param["itemNo"] : "%"
                                           , sortBy
                                           , isAscending ? "ASC" : "DESC");

                DataTable dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];
                col.Load(dt);
                tmpResult = (from it in col select it);
                if (skip > 0) tmpResult = tmpResult.Skip(skip);
                if (take > 0) tmpResult = tmpResult.Take(take);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = tmpResult,
                status = status
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string AddItemBaseLevel(string data, string username)
        {
            string status = "";
            Logger.writeLog("Add Item Base Level : " + data);
            ItemOptimalStockObj tmpDetails = new JavaScriptSerializer().Deserialize<ItemOptimalStockObj>(data);

            ItemBaseLevel newItem = new ItemBaseLevel();
            if (tmpDetails != null && tmpDetails.ItemNo != "")
            {
                //ItemBaseLevel
                if (tmpDetails.BaseLevelID.ToString() == "")
                {
                    //add
                    newItem = new ItemBaseLevel();
                    newItem.ItemNo = tmpDetails.ItemNo;
                    newItem.InventoryLocationID = tmpDetails.InventoryLocationID;
                    newItem.BaseLevelQuantity = tmpDetails.BaseLevelQuantity;
                    newItem.Deleted = false;
                    newItem.Save(username);
                }
                else
                {
                    newItem = new ItemBaseLevel(tmpDetails.BaseLevelID);
                    newItem.ItemNo = tmpDetails.ItemNo;
                    newItem.InventoryLocationID = tmpDetails.InventoryLocationID;
                    newItem.BaseLevelQuantity = tmpDetails.BaseLevelQuantity;
                    newItem.Deleted = false;
                    newItem.Save(username);
                }

            }
            else
            {
                status = "Error";
            }

            ViewitembaselevelCollection col = new ViewitembaselevelCollection();
            col.Where(Viewitembaselevel.Columns.BaseLevelID, newItem.BaseLevelID);
            col.Load();
            var result = new
            {
                Item = col[0],
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string AddItemBaseLevelByCategory(string data, string username)
        {
            string status = "";
            Logger.writeLog("Add Item Base Level By Category: " + data);
            CategoryOptimalStockObj tmpDetails = new JavaScriptSerializer().Deserialize<CategoryOptimalStockObj>(data);
            QueryCommandCollection qcmd = new QueryCommandCollection();
            try
            {

                string query = @"select i.ItemNo, i.CategoryName, ib.BaseLevelID, ib.InventoryLocationID 
                            from Item i left join
                            ItemBaseLevel ib on i.itemno = ib.ItemNo  and ib.InventoryLocationID = {0}
                            where ISNULL(i.Deleted,0) = 0 and ISNULL(ib.Deleted,0) = 0 
                            and i.CategoryName = '{1}'";

                query = string.Format(query, tmpDetails.InventoryLocationID.ToString(), tmpDetails.CategoryName);
                DataTable dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!DBNull.Value.Equals(dt.Rows[i]["BaseLevelID"]))
                    {
                        int BaseLevelID = dt.Rows[i]["BaseLevelID"].ToString().GetIntValue();
                        ItemBaseLevel newItem = new ItemBaseLevel(BaseLevelID);
                        newItem.BaseLevelQuantity = tmpDetails.BaseLevelQuantity;
                        newItem.Deleted = false;
                        qcmd.Add(newItem.GetUpdateCommand(username));
                    }
                    else
                    {
                        ItemBaseLevel newItem = new ItemBaseLevel();
                        newItem.ItemNo = dt.Rows[i]["ItemNo"].ToString();
                        newItem.InventoryLocationID = tmpDetails.InventoryLocationID;
                        newItem.BaseLevelQuantity = tmpDetails.BaseLevelQuantity;
                        newItem.Deleted = false;
                        qcmd.Add(newItem.GetSaveCommand(username));
                    }
                }

                if (qcmd.Count() > 0)
                    DataService.ExecuteTransaction(qcmd);
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetItemBaseLevel(string BaseLevelID)
        {
            string status = "";
            //Logger.writeLog(BaseLevelID);
            ViewitembaselevelCollection col = new ViewitembaselevelCollection();
            col.Where(Viewitembaselevel.Columns.BaseLevelID, Convert.ToInt16(BaseLevelID));
            col.Load();

            //ItemBaseLevel item = new ItemBaseLevel(Convert.ToInt16(BaseLevelID));
            if (col.Count < 1)
            {
                status = "Item not found.";
            }

            var result = new
            {
                Item = col[0],
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        public int GetOptimalStock(string itemno, int InventoryLocationID, out string status)
        {
            status = "";
            int WHBal = 0;

            try
            {
                if (InventoryLocationID == 0)
                {
                    status = "Warehouse not found.";
                }
                if (status == "")
                {
                    WHBal = ItemBaseLevelController.getOptimalStockLevel(itemno, InventoryLocationID);
                }
                return WHBal;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return 0;
            }
        }

        #endregion

        #region *) Inventory Stock In/Out/Transfer

        [WebMethod]
        public string StockIn(string PurchaseOrderHeaderRefNo, string detail,
                string username, int StockInReasonID, int InventoryLocationID,
                bool IsAdjustment, bool CalculateCOGS, string Remark)
        {
            string res = "";
            string key = string.Format("STOCKIN_{0}", PurchaseOrderHeaderRefNo);
            
            try
            {
                bool isProcessStockIn = AppSetting.CastBool(AppSetting.GetSetting(key), false);
                if (isProcessStockIn)
                {
                    var result = new
                    {
                        status = string.Format("{0} still process to stock in", PurchaseOrderHeaderRefNo)
                    };

                    return new JavaScriptSerializer().Serialize(result); 
                }

                AppSetting.SetSetting(key, true.ToString());

                res = StockInHelper(PurchaseOrderHeaderRefNo, detail, username, StockInReasonID, InventoryLocationID, IsAdjustment, CalculateCOGS, Remark);

                AppSetting.SetSetting(key, false.ToString());
            }
            catch (Exception ex)
            {
                AppSetting.SetSetting(key, false.ToString());
                Logger.writeLog(ex);
            }

            return res;
        }

        public string StockInHelper(string PurchaseOrderHeaderRefNo, string detail,
                string username, int StockInReasonID, int InventoryLocationID,
                bool IsAdjustment, bool CalculateCOGS, string Remark)
        {
            string status = "";
            string newRefNo = "";

            Logger.writeLog(detail);
            QueryCommandCollection cmd = new QueryCommandCollection();
            try
            {
                Logger.writeLog(string.Format("Detail for GR {0}: {1}", PurchaseOrderHeaderRefNo, detail));
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Outlet not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = ERR_CLINIC_DELETED;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = ERR_CLINIC_FROZEN;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                {
                    status = "Document Number not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }
                else
                {
                    if ((poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH") && poHdr.Status != "Approved")
                    {
                        status = "Cannot do Stock In. Please check the document's status.";
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                }

                List<InventoryDet> tmpDetails = new JavaScriptSerializer().Deserialize<List<InventoryDet>>(detail);

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);
                ctrl.SetInventoryHeaderInfo(PurchaseOrderHeaderRefNo, "", Remark, 0, 0, 0);

                // Stock In Reason is stored in StockOutReasonID column
                if (StockInReasonID > -1) ctrl.setInventoryStockOutReasonID(StockInReasonID);



                foreach (var det in tmpDetails)
                {
                    //ctrl.AddItemIntoInventoryStockIn(det.ItemNo, det.Quantity.GetValueOrDefault(0), det.FactoryPrice, out status);
                    decimal quantity = det.Quantity.GetValueOrDefault(0); 
                    decimal quantityReceived = InventoryController.GetReceivedQtyForPurchaseOrderByItemNo(poHdr.PurchaseOrderHeaderRefNo, det.ItemNo);
                    if (quantity <= quantityReceived)
                        continue;

                    #region *) So_GetQtyApproved
                    decimal QtyApproved = 0;
                    string sqlQty = @"select isnull(pd.Userint1,0) QtyApproved
                                      from purchaseorderheader ph
                                      inner join purchaseorderdetail pd WITH(NOLOCK) on ph.purchaseorderheaderrefno = pd.purchaseorderheaderrefno
                                      where ph.purchaseorderheaderrefno = @purchaseorderheaderrefno and pd.Itemno = @itemno ";
                    QueryCommand qc = new QueryCommand(sqlQty);
                    qc.AddParameter("@purchaseorderheaderrefno", poHdr.PurchaseOrderHeaderRefNo, DbType.String);
                    qc.AddParameter("@itemno", det.ItemNo, DbType.String);
                    DataTable dtQty = new DataTable();
                    dtQty.Load(DataService.GetReader(qc));
                    if (dtQty.Rows.Count > 0)
                        QtyApproved = (dtQty.Rows[0]["QtyApproved"] + "").GetDecimalValue();
                    #endregion

                    if (quantity > QtyApproved) 
                        throw new Exception(string.Format("Please Check Item :{0} Cannot Received: {1} Large Then Qty Approved:{2} ", det.ItemNo, quantity, QtyApproved.ToString("N0")));   

                    ctrl.AddItemIntoInventoryStockIn(det.ItemNo, quantity - quantityReceived, det.FactoryPrice, out status);

                    if (!string.IsNullOrEmpty(det.SerialNoString))
                    {
                        //Split into string
                        string tmp = det.SerialNoString;
                        List<String> _serialNo = tmp.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                        string message = "";
                        if (!ctrl.checkSerialNoIsExistInPurchaseOrder(PurchaseOrderHeaderRefNo, det.ItemNo, det.SerialNoString, det.Quantity.GetValueOrDefault(0).GetIntValue(), out message))
                        {
                            status = "Stock In Failed." + message;
                            return new JavaScriptSerializer().Serialize(new { status = status });
                        }

                        ctrl.InvDet[ctrl.InvDet.Count - 1].SerialNo = _serialNo;
                    }


                }

                var listInvDet = new List<string>();
                for (int i = 0; i < ctrl.InvDet.Count; i++)
                {
                    listInvDet.Add(ctrl.InvDet[i].InventoryDetRefNo);
                }

                string msgSerialNo = "";
                if (!ctrl.IsSerialNoValid(listInvDet, out msgSerialNo))
                {

                    status = "Stock In Failed." + msgSerialNo;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                string statusFromSetting = AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.StatusAllTallyReceived);
                string StatusAllTally = string.IsNullOrEmpty(statusFromSetting) ? "Received" : statusFromSetting;
                string StatusPartially = StatusAllTally.ToLower() == "Received" ? "Posted" : "Received";

                if (status == "")
                {
                    ctrl.InvHdr.VendorInvoiceNo = poHdr.ShipVia;
                    QueryCommandCollection stockInQcc = new QueryCommandCollection(); 
                    if (ctrl.StockIn(username, InventoryLocationID, IsAdjustment,
                        CalculateCOGS, out status, out stockInQcc))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();

                        if (poHdr != null && (poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH"))
                        {
                            // If "Goods Receive" then update the Status to "Posted"
                            poHdr.Status = StatusPartially;
                            //poHdr.Save(username);
                            cmd.Add(poHdr.GetSaveCommand(username));
                        }
                    }
                   if (stockInQcc.Count == 0)
                       throw new Exception("Failed to do stock in");

                   cmd.AddRange(stockInQcc);
                }

                //update status if all tally
                string querychecked = @"select pd.itemno, isnull(pd.userint1,0)QtyApproved, sum(isnull(id.quantity,0)) as ReceivedQty
                                        from purchaseorderheader ph
                                        inner join purchaseorderdetail pd WITH(NOLOCK) on ph.purchaseorderheaderrefno = pd.purchaseorderheaderrefno
                                        left join inventoryhdr ih WITH(NOLOCK) on ih.purchaseorderno = ph.purchaseorderheaderrefno
                                        left join inventorydet id WITH(NOLOCK) on ih.inventoryhdrrefno = id.inventoryhdrrefno and id.itemno = pd.itemno
                                        where ph.purchaseorderheaderrefno = '{0}' AND pd.quantity > 0 and ih.Movementtype = 'Stock In'
                                        group by pd.itemno, pd.userint1
                                       having sum(isnull(id.quantity,0)) < isnull(pd.userint1,0) ";
                querychecked = string.Format(querychecked, poHdr.PurchaseOrderHeaderRefNo);
                DataTable ds = DataService.GetDataSet(new QueryCommand(querychecked)).Tables[0];
                if (poHdr != null && (poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH") && ds.Rows.Count == 0)
                {
                    // If "Goods Receive" then update the Status to "Posted"
                    poHdr.Status = StatusAllTally;
                    //poHdr.Save();
                    cmd.Add(poHdr.GetSaveCommand(username));
                }

                DataService.ExecuteTransaction(cmd);

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                newRefNo = "";
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string StockOut(string PurchaseOrderHeaderRefNo, string detail,
                string username, int StockOutReasonID, int InventoryLocationID,
                bool IsAdjustment, bool deductRemainingQty, string Remark)
        {
            string status = "";
            string newRefNo = "";

            try
            {
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Clinic not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = ERR_CLINIC_DELETED;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = ERR_CLINIC_FROZEN;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (!string.IsNullOrEmpty(PurchaseOrderHeaderRefNo))
                {
                    PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                    if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                    {
                        status = "Document Number not found.";
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                    else
                    {
                        if (poHdr.POType.ToUpper() == "RETURN" && poHdr.Status != "Approved")
                        {
                            status = "Cannot do Stock Out. Please check the document's status.";
                            return new JavaScriptSerializer().Serialize(new { status = status });
                        }
                    }
                }

                List<InventoryDet> tmpDetails = new JavaScriptSerializer().Deserialize<List<InventoryDet>>(detail);

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);
                ctrl.SetInventoryHeaderInfo(PurchaseOrderHeaderRefNo, "", Remark, 0, 0, 0);

                foreach (var det in tmpDetails)
                {
                    ctrl.AddItemIntoInventory(det.ItemNo, det.Quantity.GetValueOrDefault(0), out status);
                }

                if (status == "")
                {
                    if (ctrl.StockOut(username, StockOutReasonID, InventoryLocationID, IsAdjustment, deductRemainingQty, out status))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                newRefNo = "";
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string StockReturn(string PurchaseOrderHeaderRefNo, string detail,
                string username, int StockOutReasonID, int InventoryLocationID,
                bool IsAdjustment, bool calculateCOGS, string Remark)
        {
            string status = "";
            string newRefNo = "";

            try
            {
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Outlet not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = ERR_CLINIC_DELETED;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = ERR_CLINIC_FROZEN;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (!string.IsNullOrEmpty(PurchaseOrderHeaderRefNo))
                {
                    PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                    if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                    {
                        status = "Document Number not found.";
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                    else
                    {
                        if (poHdr.POType.ToUpper() == "RETURN" && poHdr.Status != "Approved")
                        {
                            status = "Cannot do Stock Return. Please check the document's status.";
                            return new JavaScriptSerializer().Serialize(new { status = status });
                        }
                    }
                }

                PurchaseOrderController poCtrl = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
                List<InventoryDet> tmpDetails = new JavaScriptSerializer().Deserialize<List<InventoryDet>>(detail);

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);
                ctrl.SetInventoryHeaderInfo(PurchaseOrderHeaderRefNo, "", Remark, 0, 0, 0);
                ctrl.SetVendorInvoiceNo(poCtrl.GetInvoiceNo());

                foreach (var det in tmpDetails)
                {
                    decimal factoryPrice = 0;
                    foreach (PurchaseOrderDetail poDet in poCtrl.GetPODetail())
                    {
                        if (poDet.ItemNo == det.ItemNo)
                            factoryPrice = poDet.FactoryPrice.GetValueOrDefault(0);
                    }
                    ctrl.AddItemIntoInventoryStockReturn(det.ItemNo, det.Quantity.GetValueOrDefault(0), factoryPrice, false, out status);
                }

                if (status == "")
                {
                    bool isReturnToWH = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.StockReturnWillReturnStockToWarehouse), false);
                    if (ctrl.StockReturn(username, StockOutReasonID, InventoryLocationID, IsAdjustment, calculateCOGS, isReturnToWH, poCtrl.getWarehouseID(), out status))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                newRefNo = "";
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string StockTransfer(string PurchaseOrderHeaderRefNo, string detail,
                string username, int FromInventoryLocationID, int ToInventoryLocationID)
        {
            string status = "";
            string newRefNo = "";
            string remark = "";

            try
            {
                InventoryLocation invLocFrom = new InventoryLocation(FromInventoryLocationID);
                if (invLocFrom == null || invLocFrom.InventoryLocationID != FromInventoryLocationID)
                {
                    status = "From Clinic not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                InventoryLocation invLocTo = new InventoryLocation(ToInventoryLocationID);
                if (invLocTo == null || invLocTo.InventoryLocationID != ToInventoryLocationID)
                {
                    status = "To Clinic not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (InventoryLocationController.IsDeleted(FromInventoryLocationID))
                {
                    status = invLocFrom.InventoryLocationName + ": " + ERR_CLINIC_DELETED;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (InventoryLocationController.IsDeleted(ToInventoryLocationID))
                {
                    status = invLocTo.InventoryLocationName + ": " + ERR_CLINIC_DELETED;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (StockTakeController.IsInventoryLocationFrozen(FromInventoryLocationID))
                {
                    status = invLocFrom.InventoryLocationName + ": " + ERR_CLINIC_FROZEN;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (StockTakeController.IsInventoryLocationFrozen(ToInventoryLocationID))
                {
                    status = invLocTo.InventoryLocationName + ": " + ERR_CLINIC_FROZEN;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                {
                    status = "Document Number not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }
                else
                {
                    if (poHdr.POType.ToUpper() == "TRANSFER" && poHdr.Status != "Submitted")
                    {
                        status = "Cannot do Stock Transfer. Please check the document's status.";
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                }

                List<InventoryDet> tmpDetails = new JavaScriptSerializer().Deserialize<List<InventoryDet>>(detail);

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);

                remark = poHdr.Remark;
                ctrl.SetInventoryHeaderInfo(PurchaseOrderHeaderRefNo, "", remark, 0, 0, 0);

                foreach (var det in tmpDetails)
                {
                    ctrl.AddItemIntoInventoryStockIn(det.ItemNo, det.Quantity.GetValueOrDefault(0), det.FactoryPrice, out status);
                }

                if (status == "")
                {
                    if (ctrl.TransferOutAutoReceive(username, FromInventoryLocationID, ToInventoryLocationID, out status))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();

                        if (poHdr.POType.ToUpper() == "TRANSFER")
                        {
                            // If "Goods Receive" then update the Status to "Posted"
                            poHdr.Status = "Posted";
                            poHdr.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                newRefNo = "";
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        #endregion

        #region *) Purchase Order

        [WebMethod]
        public decimal GetWarehouseBalance(string itemNo, DateTime date, out string status)
        {
            status = "";
            decimal WHBal = 0;

            try
            {
                int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;
                if (invLocID == 0)
                {
                    status = "Warehouse not found.";
                }
                if (status == "")
                {
                    WHBal = InventoryController.GetStockBalanceQtyByItemSummaryByDate(itemNo, invLocID, date, out status);
                }
                return WHBal;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return 0;
            }
        }

        [WebMethod]
        public decimal GetWarehouseBalanceByLocID(int locationID, string itemNo, DateTime date, out string status)
        {
            status = "";
            decimal WHBal = 0;

            try
            {
                int invLocID = new InventoryLocation(locationID).InventoryLocationID;
                if (invLocID == 0)
                {
                    status = "Warehouse not found.";
                }
                if (status == "")
                {
                    WHBal = InventoryController.GetStockBalanceQtyByItemSummaryByDate(itemNo, invLocID, date, out status);
                }
                return WHBal;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return 0;
            }
        }

        public static string getNewPurchaseOrderRefNo(string POType, string InventoryLocationName)
        {
            int runningNo = 0;
            string PurchaseOrderRefNo;
            string header;

            if (POType.ToUpper() == "RETURN")
                header = "RT";
            else if (POType.ToUpper() == "TRANSFER")
                header = "ST";
            else if (POType.ToUpper().StartsWith("ADJUSTMENT"))
                header = "AD";
            else
                header = "SO";

            header += InventoryLocationName.Left(3) + DateTime.Now.ToString("yy");

            Query qr = PurchaseOrderHeader.CreateQuery();
            qr.AddWhere(PurchaseOrderHeader.Columns.PurchaseOrderHeaderRefNo, Comparison.Like, header + "%");
            qr.QueryType = QueryType.Select;
            qr.Top = "1";
            qr.SetSelectList("PurchaseOrderHeaderRefNo");
            qr.OrderBy = OrderBy.Desc("PurchaseOrderHeaderRefNo");

            DataSet ds = qr.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                runningNo = int.Parse(ds.Tables[0].Rows[0][0].ToString().Substring(header.Length));
            }
            else
            {
                runningNo = 0;
            }
            runningNo += 1;

            //SO1231400001 or RT1231400001
            //SOPPPYYNNNNN
            //PPP - Inventory Location ID
            //YY - year
            //NNNNN - Running No
            PurchaseOrderRefNo = header + runningNo.ToString().PadLeft(5, '0');

            return PurchaseOrderRefNo;
        }

        public string ValidatePurchaseOrderDetail(PurchaseOrderDetail poDet, PurchaseOrderHeader poHdr)
        {
            string status = "";

            try
            {
                string poType = poHdr.POType.ToUpper();

                if (string.IsNullOrEmpty(poDet.ItemNo))
                {
                    status = "Please enter Item.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                Item item = new Item(poDet.ItemNo);
                if (item == null || string.IsNullOrEmpty(item.ItemNo))
                {
                    status = "Item not found: " + poDet.ItemNo;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }


                if ((poType == "ORDER" || poType == "REPLENISH") && item.Deleted.GetValueOrDefault(false) == true)
                {
                    status = "Item not found: " + poDet.ItemNo;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (poType == "ORDER" || poType == "REPLENISH")
                {
                    int supplierID = poHdr.SupplierID.GetValueOrDefault(0);
                    string queryW = "Select count(*) from Supplier where ISNULL(Userflag1,0) = 1 and ISNULL(Deleted,0) = 0";
                    int countWarehouse = (int)DataService.ExecuteScalar(new QueryCommand(queryW));

                    string queSupplier = string.Format("Select * from ItemSupplierMap where ItemNo ='{0}' and ISNULL(deleted,0) = 0", poDet.ItemNo);
                    ItemSupplierMapCollection itsCol = new ItemSupplierMapCollection();
                    itsCol.LoadAndCloseReader(DataService.GetReader(new QueryCommand(queSupplier)));

                    //if no item on item supplier map then can add
                    if (itsCol.Count() > 0)
                    {
                        //check if there is no item on supplier list than can not add except if only have one warehouse
                        if (supplierID != 0 && countWarehouse > 1)
                        {
                            var list = (from i in itsCol where i.SupplierID == supplierID select i).ToList();

                            //if there is no item on the supplier list then can't add
                            if (list.Count() == 0)
                            {
                                status = "Item " + poDet.ItemNo + " not found on supplier list ";
                                return new JavaScriptSerializer().Serialize(new { status = status });
                            }
                        }
                    }
                }

                // Check duplicate ItemNo
                SqlQuery qry = new Select().From("PurchaseOrderDetail")
                                   .Where("PurchaseOrderHeaderRefNo").IsEqualTo(poDet.PurchaseOrderHeaderRefNo)
                                   .And("ItemNo").IsEqualTo(poDet.ItemNo.Trim());
                if (poType == "RETURN" || poType.StartsWith("ADJUSTMENT"))
                {
                    if (poDet.ExpiryDate == null)
                        qry.And(PurchaseOrderDetail.Columns.ExpiryDate).IsNull();
                    else
                        qry.And(PurchaseOrderDetail.Columns.ExpiryDate).IsEqualTo(poDet.ExpiryDate);
                }
                int count = qry.GetRecordCount();

                if (poDet.Quantity <= 0 && poDet.RejectQty <= 0)
                {
                    status = poDet.Item.ItemName + ": Quantity must be greater than zero.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (poType.StartsWith("ADJUSTMENT") && !string.IsNullOrEmpty(poHdr.InventoryStockOutReason.ReasonName) && poHdr.InventoryStockOutReason.ReasonName.ToUpper() == "EXPIRED")
                {
                    // Check expiry date
                    DateTime expDate;
                    int maxExpiryMonth = int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["ExpiryDateMaximumMonthForAdjustment"]);
                    if (poDet.ExpiryDate != null)
                    {
                        expDate = (DateTime)poDet.ExpiryDate;
                        if (expDate > DateTime.Today.AddMonths(maxExpiryMonth))
                        {
                            status = string.Format(poDet.Item.ItemName + ": Expiry Date must be less than {0} months from today", maxExpiryMonth);
                            return new JavaScriptSerializer().Serialize(new { status = status });
                        }
                    }
                    else
                    {
                        status = poDet.Item.ItemName + ": Please enter a valid date for Expiry Date.";
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                }

                if (poType == "RETURN" || poType == "ADJUSTMENT OUT" || poType == "TRANSFER")
                {
                    bool AllowDeductQtyNotSuffice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AllowDeductInvQtyNotSufficient), false);

                    Item itm = new Item(poDet.ItemNo);
                    decimal divider = 1;
                    if (itm.NonInventoryProduct)
                    {
                        divider = (itm.DeductConvType ? (1 / itm.DeductConvRate == 0 ? 1 : itm.DeductConvRate) : (itm.DeductConvRate));
                    }
                    string ItemNo = itm.NonInventoryProduct ? itm.DeductedItem : itm.ItemNo;

                    // Out qty can't be greater than quantity in clinic
                    int stockBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(ItemNo, (int)poHdr.InventoryLocationID, DateTime.Now, out status).GetIntValue();
                    if (status != "")
                        return new JavaScriptSerializer().Serialize(new { status = status });

                    decimal totalQty = 0;
                    foreach (PurchaseOrderDetail det in poHdr.PurchaseOrderDetailRecords())
                    {
                        if (det.ItemNo == poDet.ItemNo)
                            totalQty += det.Quantity ?? 0;
                    }

                    if (poHdr.Status == "Pending") totalQty += poDet.Quantity ?? 0;
                    if (!AllowDeductQtyNotSuffice && (totalQty * divider) > stockBalance)
                    {
                        if (poType == "RETURN")
                            status = poDet.Item.ItemName + ": Total Returned Quantity is greater than Quantity in Outlet.";
                        else if (poType == "ADJUSTMENT OUT")
                            status = poDet.Item.ItemName + ": Total Adjustment Out Quantity is greater than Quantity in Outlet.";
                        else if (poType == "TRANSFER")
                            status = poDet.Item.ItemName + ": Total Transfer Quantity is greater than Quantity in Outlet.";
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            return "";
        }

        [WebMethod]
        public bool GenerateEDIOnApproval(string PurchaseOrderHeaderRefNo)
        {
            PurchaseOrderController poCtrl = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
            PurchaseOrderHeader poHdr = poCtrl.GetPOHeader();
            PurchaseOrderDetailCollection poDets = poCtrl.GetPODetail();

            if (poHdr != null && poDets != null &&
                poHdr.PurchaseOrderHeaderRefNo == PurchaseOrderHeaderRefNo &&
                poDets.Count > 0 &&
                poHdr.Status == "Approved")
            {
                string exportDirectory = "";
                try
                {
                    exportDirectory = Server.MapPath(ConfigurationManager.AppSettings["EDIDirectory"]);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("is not a valid virtual path."))
                        exportDirectory = ConfigurationManager.AppSettings["EDIDirectory"];
                }

                if (!exportDirectory.EndsWith("\\")) exportDirectory += "\\";
                string fileName = PurchaseOrderHeaderRefNo.Substring(0, 2) + "_" +
                                  PurchaseOrderHeaderRefNo.Substring(2, 3) + "_" +
                                  PurchaseOrderHeaderRefNo.Substring(5, 2) + "_" +
                                  PurchaseOrderHeaderRefNo.Substring(7, 5) + ".sh";

                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(exportDirectory);
                if (!dir.Exists) dir.Create();

                string d = "|";  // delimiter
                int i = 1;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportDirectory + fileName, false))
                {
                    string str = "SHPHDR" + d;   //1.SHPHDR
                    str += i + d;   //2.Interface Record ID
                    str += PurchaseOrderHeaderRefNo.Left(25) + d;   //3.Shipment ID
                    str += "PSPL" + d;   //4.Warehouse
                    str += d + d + d;   //Column 5-7
                    str += "Y" + d;   //8.Allocate Complete
                    str += d + d + d + d + d + d + d + d + d;   //Column 9-17
                    str += "PSPL" + d;   //18.Company
                    str += d + d + d + d + d + d + d + d + d + d + d + d;   //Column 19-30
                    str += d + d + d + d + d + d + d + d + d + d;   //Column 31-40
                    str += d + d + d + d + d + d + d + d + d + d;   //Column 41-50
                    str += d + d + d + d + d + d + d + d + d + d;   //Column 51-60
                    str += d + d + d + d + d + d + d + d + d + d;   //Column 61-70
                    str += d + d + d + d + d + d + d + d + d + d;   //Column 71-80
                    str += d + d + d + d + d + d + d + d + d + d;   //Column 81-90
                    str += d + d + d;   //Column 91-93
                    str += poHdr.PurchaseOrderDate.ToString("yyyyMMdd") + d;   //94.Planned Ship Date
                    str += d + d;   //Column 95-96
                    str += "Standard" + d;   //97.Process Type
                    str += d + d;   //Column 98-99
                    str += (poHdr.ApprovalDate.Hour < 12) ? (AddWorkingDays(poHdr.ApprovalDate, 2).ToString("yyyyMMdd")) + d : (AddWorkingDays(poHdr.ApprovalDate, 3).ToString("yyyyMMdd")) + d;   //100.Requested Delivery Date
                    str += "By" + d;   //101.Requested Delivery Type
                    str += d + d;   //Column 102-103
                    str += poHdr.PurchaseOrderDate.ToString("yyyyMMdd") + d;   //104.Scheduled Ship Date
                    InventoryLocation il = new InventoryLocation(poHdr.InventoryLocationID);
                    str += il.InventoryLocationName.Left(25) + d;   //105.Ship To
                    str += il.Address1.Left(50) + d;   //106.Ship To Address 1
                    str += il.Address2.Left(50) + d;   //107.Ship To Address 2
                    str += il.Address3.Left(50) + d;   //108.Ship To Address 3
                    str += d;   //Column 109
                    str += "Singapore" + d;   //110.Ship To City
                    str += "SG" + d;   //111.Ship To Country
                    str += d + d;   //Column 112-113
                    str += (il.InventoryLocationName + " - " + il.DisplayedName).Left(50) + d;   //114.Ship To Name
                    str += d;   //Column 115
                    str += il.PostalCode.Left(25) + d;   //116.Ship To Postal Code
                    str += "-" + d;   //117.Ship To State
                    str += d + d + d + d + d + d + d + d + d + d + d + d + d;   //Column 118-130
                    str += d + d + d + d + d + d + d + d + d + d;   //Column 131-140
                    str += d + d + d + d + d;   //Column 141-145 (col 145 still has delimiter after it)

                    // Write the Header
                    file.WriteLine(str);

                    for (int j = 0; j < poDets.Count; j++)
                    {
                        if (poDets[j].QtyApproved > 0)
                        {
                            i++;
                            str = "SHPDETL" + d;   //1.SHPDETL
                            str += (j + 1) + d;   //2.ERP Order Line Number
                            str += "1" + d;   //3.Interface Link ID
                            str += i + d;   //4.Interface Record ID
                            str += poDets[j].ItemNo.Left(25) + d;   //5.Item
                            str += poDets[j].QtyApproved + d;   //6.Total Qty
                            str += d + d + d + d;   //Column 7-10
                            str += d + d + d + d + d + d + d + d + d + d;   //Column 11-20
                            str += d + d + d + d + d + d + d + d + d + d;   //Column 21-30
                            str += d + d + d + d + d + d + d + d + d + d;   //Column 31-40
                            str += d + d + d + d + d + d + d + d + d + d;   //Column 41-50
                            str += d + d + d + d + d + d + d + d + d + d;   //Column 51-60
                            str += d + d + d + d + d + d + d + d + d + d;   //Column 61-70
                            str += d + d + d + d + d + d + d + d + d + d;   //Column 71-80
                            str += d + d + d + d + d + d + d + d + d + d;   //Column 81-90
                            str += d + d + d + d + d + d + d + d + d + d;   //Column 91-100
                            str += d + d + d + d + d + d + d + d + d + d;   //Column 101-110
                            str += d + d + d + d + d + d + d + d + d + d;   //Column 111-120
                            str += d + d + d + d + d;   //Column 121-125

                            // Write the Details
                            file.WriteLine(str);
                        }
                    }
                }

                Logger.writeLog(string.Format("Generated {0} file.", fileName));
                return true;

            }

            return false;
        }

        [WebMethod]
        public string SavePurchaseOrderHeader(string data, string username)
        {
            var status = "";
            string BackOrderNo = "";
            PurchaseOrderHeader dataPO;
            PurchaseOrderHeader tmpPO = new PurchaseOrderHeader();
            Logger.writeLog(data);
            try
            {
                dataPO = new JavaScriptSerializer().Deserialize<PurchaseOrderHeader>(data);

                if (dataPO.InventoryLocationID.GetValueOrDefault(0) == 0)
                {
                    status = "Invalid Inventory Location ID";
                }

                if (status == "")
                {
                    if (InventoryLocationController.IsDeleted((int)dataPO.InventoryLocationID))
                    {
                        status = ERR_CLINIC_DELETED;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    if (dataPO.DestInventoryLocationID != 0 && InventoryLocationController.IsDeleted(dataPO.DestInventoryLocationID))
                    {
                        status = dataPO.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_DELETED;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    if (StockTakeController.IsInventoryLocationFrozen((int)dataPO.InventoryLocationID))
                    {
                        status = ERR_CLINIC_FROZEN;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    if (dataPO.DestInventoryLocationID != 0 && StockTakeController.IsInventoryLocationFrozen(dataPO.DestInventoryLocationID))
                    {
                        status = dataPO.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_FROZEN;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    InventoryLocation il = new InventoryLocation(dataPO.InventoryLocationID);
                    if (string.IsNullOrEmpty(dataPO.PurchaseOrderHeaderRefNo))
                    {
                        dataPO.PurchaseOrderHeaderRefNo = getNewPurchaseOrderRefNo(dataPO.POType, il.InventoryLocationName);
                    }

                    if (dataPO.Status == "Submitted")
                    {
                        if (dataPO.POType.ToUpper() != "BACK ORDER")
                        {
                            if (dataPO.PurchaseOrderDetailRecords().Count == 0)
                            {
                                status = "Please insert at least 1 item.";
                                return new JavaScriptSerializer().Serialize(new { status = status });
                            }

                            if (dataPO.POType.ToUpper() == "RETURN" || dataPO.POType.ToUpper() == "TRANSFER" || dataPO.POType.StartsWith("ADJUSTMENT"))
                            {
                                if (string.IsNullOrEmpty(dataPO.InventoryStockOutReason.ReasonName))
                                {
                                    status = "Please select Reason first.";
                                    return new JavaScriptSerializer().Serialize(new { status = status });
                                }

                                if (!string.IsNullOrEmpty(dataPO.InventoryStockOutReason.ReasonName)
                                    && (dataPO.InventoryStockOutReason.ReasonName.ToUpper() == "OTHER" || dataPO.InventoryStockOutReason.ReasonName.ToUpper() == "OTHERS")
                                    && (string.IsNullOrEmpty(dataPO.Remark) || dataPO.Remark.Trim() == ""))
                                {
                                    status = "Remark is compulsory if Reason is 'Others'.";
                                    return new JavaScriptSerializer().Serialize(new { status = status });
                                }
                            }

                            if (dataPO.POType.ToUpper() == "TRANSFER")
                            {
                                if (dataPO.DestInventoryLocation.InventoryLocationID == 0)
                                {
                                    status = "Please select a valid 'To Clinic'.";
                                    return new JavaScriptSerializer().Serialize(new { status = status });
                                }

                                if (dataPO.InventoryLocationID.GetValueOrDefault(0) == dataPO.DestInventoryLocationID)
                                {
                                    status = "'From Clinic' and 'To Clinic' cannot be the same.";
                                    return new JavaScriptSerializer().Serialize(new { status = status });
                                }
                            }

                            // Re-validate PODetail on submit
                            foreach (PurchaseOrderDetail poDet in dataPO.PurchaseOrderDetailRecords())
                            {
                                status = ValidatePurchaseOrderDetail(poDet, dataPO);
                                if (status != "")
                                    return status;  // No need to serialize it, it has already been serialized
                            }
                        }
                    }

                    if (dataPO.Status == "Submitted" || dataPO.Status == "Cancelled")
                        dataPO.PurchaseOrderDate = DateTime.Now;

                    tmpPO = new PurchaseOrderHeader(dataPO.PurchaseOrderHeaderRefNo);

                    if (dataPO.POType.ToUpper() == "TRANSFER" || dataPO.POType.ToUpper() == "BACK ORDER")
                    {
                        if (dataPO.Status == "Cancelled" && tmpPO.Status == "Posted")
                        {
                            status = "Cannot cancel this document because the status is Posted.";
                            return new JavaScriptSerializer().Serialize(new { status = status });
                        }
                    }
                    else
                    {
                        if ((dataPO.Status == "Submitted" || dataPO.Status == "Cancelled") && tmpPO.Status != "Pending")
                        {
                            status = "Cannot change this document's status because it is already " + tmpPO.Status;
                            return new JavaScriptSerializer().Serialize(new { status = status });
                        }
                    }


                    foreach (TableSchema.TableColumn col in tmpPO.GetSchema().Columns)
                    {
                        if (col.ColumnName != "ModifiedOn" && col.ColumnName != "ModifiedBy")
                        {
                            tmpPO.SetColumnValue(col.ColumnName, dataPO.GetColumnValue(col.ColumnName));
                        }
                    }

                    #region *) If POType is Back Order
                    //-> find if there is any existing back order for this outlet with status = Submitted
                    //-> If not found, then don't auto approve, just save the order
                    //-> If found, then auto reject this order, and add the current order details into the existing back order for this outlet
                    if (dataPO.POType.ToUpper() == "BACK ORDER")
                    {
                        //check for existing BackOrder for this inventorylocation
                        PurchaseOrderHeaderCollection bCol = new PurchaseOrderHeaderCollection();
                        bCol.Where(PurchaseOrderHeader.UserColumns.Status, "Submitted");
                        bCol.Where(PurchaseOrderHeader.UserColumns.POType, "Back Order");
                        bCol.Where(PurchaseOrderHeader.Columns.InventoryLocationID, dataPO.InventoryLocationID);
                        bCol.Load();

                        if (bCol.Count > 0)
                        {
                            PurchaseOrderController backOrder = new PurchaseOrderController(bCol[0].PurchaseOrderHeaderRefNo);
                            PurchaseOrderController currentOrder = new PurchaseOrderController(dataPO.PurchaseOrderHeaderRefNo);
                            PurchaseOrderDetail pod = new PurchaseOrderDetail();
                            foreach (PurchaseOrderDetail od in currentOrder.GetPODetail())
                            {
                                if (backOrder.AddItemIntoPurchaseOrder(od.ItemNo, od.Quantity ?? 0, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, out status, out pod))
                                {
                                    SalesOrderMappingController.DistributeBackOrderSalesMapping(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, od.Quantity ?? 0);
                                }
                            }

                            BackOrderNo = backOrder.GetPurchaseOrderHeaderRefNo();
                            tmpPO.Status = "Cancelled";
                        }
                    }
                    #endregion

                    tmpPO.Save(username);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                PurchaseOrderHeader = tmpPO,
                backOrderNo = BackOrderNo,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);

        }

        [WebMethod]
        public string SavePurchaseOrderHeaderCreateItems(string data, string username, string createItems)
        {
            var status = "";
            PurchaseOrderHeader dataPO;
            PurchaseOrderHeader tmpPO = new PurchaseOrderHeader();
            Logger.writeLog("Create Items :" + data);
            try
            {
                dataPO = new JavaScriptSerializer().Deserialize<PurchaseOrderHeader>(data);

                if (dataPO.InventoryLocationID.GetValueOrDefault(0) == 0)
                {
                    status = "Invalid Inventory Location ID";
                }

                if (status == "")
                {
                    if (InventoryLocationController.IsDeleted((int)dataPO.InventoryLocationID))
                    {
                        status = ERR_CLINIC_DELETED;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    if (dataPO.DestInventoryLocationID != 0 && InventoryLocationController.IsDeleted(dataPO.DestInventoryLocationID))
                    {
                        status = dataPO.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_DELETED;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    if (StockTakeController.IsInventoryLocationFrozen((int)dataPO.InventoryLocationID))
                    {
                        status = ERR_CLINIC_FROZEN;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    if (dataPO.DestInventoryLocationID != 0 && StockTakeController.IsInventoryLocationFrozen(dataPO.DestInventoryLocationID))
                    {
                        status = dataPO.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_FROZEN;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    InventoryLocation il = new InventoryLocation(dataPO.InventoryLocationID);
                    if (string.IsNullOrEmpty(dataPO.PurchaseOrderHeaderRefNo))
                    {
                        dataPO.PurchaseOrderHeaderRefNo = getNewPurchaseOrderRefNo(dataPO.POType, il.InventoryLocationName);
                    }

                    if (dataPO.Status == "Submitted")
                    {
                        if (dataPO.POType.ToUpper() != "BACK ORDER")
                        {
                            if (dataPO.PurchaseOrderDetailRecords().Count == 0)
                            {
                                status = "Please insert at least 1 item.";
                                return new JavaScriptSerializer().Serialize(new { status = status });
                            }

                            if (dataPO.POType.ToUpper() == "RETURN" || dataPO.POType.ToUpper() == "TRANSFER" || dataPO.POType.StartsWith("ADJUSTMENT"))
                            {
                                if (string.IsNullOrEmpty(dataPO.InventoryStockOutReason.ReasonName))
                                {
                                    status = "Please select Reason first.";
                                    return new JavaScriptSerializer().Serialize(new { status = status });
                                }

                                if (!string.IsNullOrEmpty(dataPO.InventoryStockOutReason.ReasonName)
                                    && (dataPO.InventoryStockOutReason.ReasonName.ToUpper() == "OTHER" || dataPO.InventoryStockOutReason.ReasonName.ToUpper() == "OTHERS")
                                    && (string.IsNullOrEmpty(dataPO.Remark) || dataPO.Remark.Trim() == ""))
                                {
                                    status = "Remark is compulsory if Reason is 'Others'.";
                                    return new JavaScriptSerializer().Serialize(new { status = status });
                                }
                            }

                            if (dataPO.POType.ToUpper() == "TRANSFER")
                            {
                                if (dataPO.DestInventoryLocation.InventoryLocationID == 0)
                                {
                                    status = "Please select a valid 'To Clinic'.";
                                    return new JavaScriptSerializer().Serialize(new { status = status });
                                }

                                if (dataPO.InventoryLocationID.GetValueOrDefault(0) == dataPO.DestInventoryLocationID)
                                {
                                    status = "'From Clinic' and 'To Clinic' cannot be the same.";
                                    return new JavaScriptSerializer().Serialize(new { status = status });
                                }
                            }

                            // Re-validate PODetail on submit
                            foreach (PurchaseOrderDetail poDet in dataPO.PurchaseOrderDetailRecords())
                            {
                                status = ValidatePurchaseOrderDetail(poDet, dataPO);
                                if (status != "")
                                    return status;  // No need to serialize it, it has already been serialized
                            }
                        }
                    }



                    if (dataPO.Status == "Submitted" || dataPO.Status == "Cancelled")
                        dataPO.PurchaseOrderDate = DateTime.Now;

                    tmpPO = new PurchaseOrderHeader(dataPO.PurchaseOrderHeaderRefNo);

                    if (dataPO.POType.ToUpper() == "TRANSFER" || dataPO.POType.ToUpper() == "BACK ORDER")
                    {
                        if (dataPO.Status == "Cancelled" && tmpPO.Status == "Posted")
                        {
                            status = "Cannot cancel this document because the status is Posted.";
                            return new JavaScriptSerializer().Serialize(new { status = status });
                        }
                    }
                    else
                    {
                        if ((dataPO.Status == "Submitted" || dataPO.Status == "Cancelled") && tmpPO.Status != "Pending")
                        {
                            status = "Cannot change this document's status because it is already " + tmpPO.Status;
                            return new JavaScriptSerializer().Serialize(new { status = status });
                        }
                    }


                    foreach (TableSchema.TableColumn col in tmpPO.GetSchema().Columns)
                    {
                        if (col.ColumnName != "ModifiedOn" && col.ColumnName != "ModifiedBy")
                        {
                            tmpPO.SetColumnValue(col.ColumnName, dataPO.GetColumnValue(col.ColumnName));
                        }
                    }

                    tmpPO.Save(username);

                    PurchaseOrderController por = new PurchaseOrderController(tmpPO.PurchaseOrderHeaderRefNo);

                    if (por != null && por.GetPurchaseOrderHeaderRefNo() == tmpPO.PurchaseOrderHeaderRefNo)
                    {
                        QueryCommandCollection cmd = new QueryCommandCollection();
                        if (createItems.ToLower() == "true")
                        {
                            //Create Item
                            DataTable dt = ItemBaseLevelController.FetchDataByInventoryLocation((int)dataPO.InventoryLocationID);
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    PurchaseOrderDetail AddedItem = new PurchaseOrderDetail();
                                    decimal outletBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(dr["ItemNo"].ToString(), (int)dataPO.InventoryLocationID, DateTime.Now, out status);
                                    int optStock = GetOptimalStock(dr["ItemNo"].ToString(), (int)dataPO.InventoryLocationID, out status);
                                    if (dataPO.POType.ToUpper() == "RETURN")
                                    {
                                        if (outletBalance - optStock > 0)
                                        {
                                            por.AddItemIntoPurchaseOrder(dr["ItemNo"].ToString(), outletBalance - optStock, DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss"), out status, out AddedItem);
                                        }
                                    }
                                    else
                                    {
                                        if (optStock - outletBalance > 0)
                                        {
                                            por.AddItemIntoPurchaseOrder(dr["ItemNo"].ToString(), optStock - outletBalance, DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss"), out status, out AddedItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                PurchaseOrderHeader = tmpPO,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);

        }

        [WebMethod]
        public string GetPurchaseOrderHeaderList(string filter, int skip, int take, string sortBy, bool isAscending)
        {
            Logger.writeLog("Get Purchase Order List : " + filter);
            var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);

            PurchaseOrderHeaderCollection poHdrColl = new PurchaseOrderHeaderCollection();
            if (!(param.ContainsKey("showBackOrderWithReadyStock")) || param["showBackOrderWithReadyStock"].ToLower() != "true")
            {
                if (param.ContainsKey("potype"))
                    poHdrColl.Where(PurchaseOrderHeader.UserColumns.POType, Comparison.Equals, param["potype"]);

                if (param.ContainsKey("PurchaseOrderHeaderrefno"))
                    poHdrColl.Where("PurchaseOrderHeaderRefNo", Comparison.Like, "%" + param["PurchaseOrderHeaderrefno"] + "%");

                if (param.ContainsKey("purchaseorderheaderrefno"))
                    poHdrColl.Where("PurchaseOrderHeaderRefNo", Comparison.Like, "%" + param["purchaseorderheaderrefno"] + "%");

                if (param.ContainsKey("startdate"))
                    poHdrColl.Where("PurchaseOrderDate", Comparison.GreaterOrEquals, param["startdate"]);

                if (param.ContainsKey("enddate"))
                    poHdrColl.Where("PurchaseOrderDate", Comparison.LessOrEquals, param["enddate"]);

                if (param.ContainsKey("inventorylocationid") && param["inventorylocationid"] != "0")
                    poHdrColl.Where("InventoryLocationID", Comparison.Equals, param["inventorylocationid"]);

                if (param.ContainsKey("destinventorylocationid") && param["destinventorylocationid"] != "0")
                    poHdrColl.Where(PurchaseOrderHeader.UserColumns.DestInventoryLocationID, Comparison.Equals, param["destinventorylocationid"]);

                if (param.ContainsKey("supplierid"))
                {
                    //select all
                    if (param["supplierid"] == "0")
                    {
                        if (param.ContainsKey("username") && !string.IsNullOrEmpty(param["username"]))
                        {

                            UserMst us = new UserMst(UserMst.Columns.UserName, param["username"]);

                            if (us != null && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseSupplierPortal), false) && us.IsSupplier && us.IsRestrictedSupplierList)
                            {
                                string query = string.Format("SELECT * FROM Supplier WHERE ISNULL(deleted,0) = 0 and userfld4 = '{0}'", us.UserName);
                                QueryCommand qc = new QueryCommand(query);
                                SupplierCollection colSup = new SupplierCollection();
                                colSup.LoadAndCloseReader(DataService.GetReader(qc));

                                List<int> re = colSup.Select(s => s.SupplierID).ToList<int>();

                                poHdrColl.Where(PurchaseOrderHeader.Columns.SupplierID, Comparison.In, re);
                            }
                        }
                    }
                    else
                    {
                        poHdrColl.Where(PurchaseOrderHeader.Columns.SupplierID, param["supplierid"].ToString().GetIntValue());
                    }
                }

                if (param.ContainsKey("status") && param["status"] != "ALL")
                {
                    poHdrColl.Where(PurchaseOrderHeader.UserColumns.Status, Comparison.Equals, param["status"]);
                }
                else
                {
                    if (param.ContainsKey("frompage") && (param["frompage"] == "OrderApproval" || param["frompage"] == "ReturnApproval" || param["frompage"] == "AdjustmentApproval"))
                        poHdrColl.Where(PurchaseOrderHeader.UserColumns.Status, Comparison.NotIn, new string[] { "Pending", "Cancelled" });
                }

                poHdrColl.Load();
            }
            else
            {
                poHdrColl.Where(PurchaseOrderHeader.UserColumns.Status, Comparison.NotIn, new string[] { "Pending", "Cancelled", "Approved", "Rejected" });
                poHdrColl.Where(PurchaseOrderHeader.UserColumns.POType, Comparison.Equals, "Back Order");
                poHdrColl.Load();
            }

            int[] tempDelete = new int[poHdrColl.Count];
            if (param.ContainsKey("showBackOrderWithReadyStock") && param["showBackOrderWithReadyStock"].ToLower() == "true")
            {

                for (int i = 0; i < poHdrColl.Count; i++)
                {
                    PurchaseOrderHeader poh = poHdrColl[i];
                    if (poh.Userfld1 == "Approved" || poh.Userfld1 == "Rejected" || poh.Userfld1 == "Posted")
                    {
                        tempDelete[i] = 1;
                        continue;
                    }

                    if (!PurchaseOrderController.IsAvailableToOrder(poh.PurchaseOrderHeaderRefNo))
                    {
                        tempDelete[i] = 1;
                    }
                    else
                    {
                        tempDelete[i] = 0;
                    }
                }

                for (int j = poHdrColl.Count - 1; j >= 0; j--)
                {
                    if (tempDelete[j] == 1)
                    {
                        poHdrColl.RemoveAt(j);
                    }
                }

            }

            var tmpResult = (from po in poHdrColl select po);
            tmpResult = tmpResult.Sort(sortBy, ((isAscending) ? "asc" : "desc"));

            if (skip > 0) tmpResult = tmpResult.Skip(skip);
            if (take > 0) tmpResult = tmpResult.Take(take);

            var result = new
            {
                records = tmpResult.ToList(),
                totalRecords = poHdrColl.Count
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetPurchaseOrderHeaderListWithOutletName(string filter, int skip, int take, string sortBy, bool isAscending)
        {
            Logger.writeLog("Get Purchase Order List : " + filter);
            var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);


            ViewPurchaseOrderHeaderCollection poHdrColl = new ViewPurchaseOrderHeaderCollection();
            if (!(param.ContainsKey("showBackOrderWithReadyStock")) || param["showBackOrderWithReadyStock"].ToLower() != "true")
            {
                if (param.ContainsKey("potype"))
                    poHdrColl.Where(ViewPurchaseOrderHeader.Columns.POType, Comparison.Equals, param["potype"]);

                if (param.ContainsKey("PurchaseOrderHeaderrefno"))
                    poHdrColl.Where("PurchaseOrderHeaderRefNo", Comparison.Like, "%" + param["PurchaseOrderHeaderrefno"] + "%");

                if (param.ContainsKey("purchaseorderheaderrefno"))
                    poHdrColl.Where("PurchaseOrderHeaderRefNo", Comparison.Like, "%" + param["purchaseorderheaderrefno"] + "%");

                if (param.ContainsKey("startdate"))
                    poHdrColl.Where("PurchaseOrderDate", Comparison.GreaterOrEquals, param["startdate"]);

                if (param.ContainsKey("enddate"))
                    poHdrColl.Where("PurchaseOrderDate", Comparison.LessOrEquals, param["enddate"]);

                if (param.ContainsKey("inventorylocationid") && param["inventorylocationid"] != "0")
                    poHdrColl.Where("InventoryLocationID", Comparison.Equals, param["inventorylocationid"]);

                if (param.ContainsKey("destinventorylocationid") && param["destinventorylocationid"] != "0")
                    poHdrColl.Where(ViewPurchaseOrderHeader.Columns.DestInventoryLocationID, Comparison.Equals, param["destinventorylocationid"]);

                if (param.ContainsKey("supplierid"))
                {
                    //select all
                    if (param["supplierid"] == "0")
                    {
                        if (param.ContainsKey("username") && !string.IsNullOrEmpty(param["username"]))
                        {

                            UserMst us = new UserMst(UserMst.Columns.UserName, param["username"]);

                            if (us != null && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseSupplierPortal), false) && us.IsSupplier && us.IsRestrictedSupplierList)
                            {
                                string query = string.Format("SELECT * FROM Supplier WHERE ISNULL(deleted,0) = 0 and userfld4 = '{0}'", us.UserName);
                                QueryCommand qc = new QueryCommand(query);
                                SupplierCollection colSup = new SupplierCollection();
                                colSup.LoadAndCloseReader(DataService.GetReader(qc));

                                List<int> re = colSup.Select(s => s.SupplierID).ToList<int>();

                                poHdrColl.Where(ViewPurchaseOrderHeader.Columns.SupplierID, Comparison.In, re);
                            }
                        }
                    }
                    else
                    {
                        poHdrColl.Where(ViewPurchaseOrderHeader.Columns.SupplierID, param["supplierid"].ToString().GetIntValue());
                    }
                }

                if (param.ContainsKey("status") && param["status"] != "ALL")
                {
                    poHdrColl.Where(ViewPurchaseOrderHeader.Columns.Status, Comparison.Equals, param["status"]);
                }
                else
                {
                    if (param.ContainsKey("frompage") && (param["frompage"] == "OrderApproval" || param["frompage"] == "ReturnApproval" || param["frompage"] == "AdjustmentApproval"))
                        poHdrColl.Where(ViewPurchaseOrderHeader.Columns.Status, Comparison.NotIn, new string[] { "Pending", "Cancelled" });
                }

                poHdrColl.Load();
            }
            else
            {
                poHdrColl.Where(ViewPurchaseOrderHeader.Columns.Status, Comparison.NotIn, new string[] { "Pending", "Cancelled", "Approved", "Rejected" });
                poHdrColl.Where(ViewPurchaseOrderHeader.Columns.POType, Comparison.Equals, "Back Order");
                poHdrColl.Load();
            }

            int[] tempDelete = new int[poHdrColl.Count];
            if (param.ContainsKey("showBackOrderWithReadyStock") && param["showBackOrderWithReadyStock"].ToLower() == "true")
            {

                for (int i = 0; i < poHdrColl.Count; i++)
                {
                    ViewPurchaseOrderHeader poh = poHdrColl[i];
                    if (poh.Status == "Approved" || poh.Status == "Rejected" || poh.Status == "Posted")
                    {
                        tempDelete[i] = 1;
                        continue;
                    }

                    if (!PurchaseOrderController.IsAvailableToOrder(poh.PurchaseOrderHeaderRefNo))
                    {
                        tempDelete[i] = 1;
                    }
                    else
                    {
                        tempDelete[i] = 0;
                    }
                }

                for (int j = poHdrColl.Count - 1; j >= 0; j--)
                {
                    if (tempDelete[j] == 1)
                    {
                        poHdrColl.RemoveAt(j);
                    }
                }

            }

            var tmpResult = (from po in poHdrColl select po);
            tmpResult = tmpResult.Sort(sortBy, ((isAscending) ? "asc" : "desc"));

            if (skip > 0) tmpResult = tmpResult.Skip(skip);
            if (take > 0) tmpResult = tmpResult.Take(take);

            List<ViewPurchaseOrderObject> objColl = new List<ViewPurchaseOrderObject>();
            foreach (ViewPurchaseOrderHeader vw in tmpResult)
            {
                string ReasonName = string.Empty;
                if (vw.ReasonID != null && vw.ReasonID != 0)
                {
                    InventoryStockOutReason ras = new InventoryStockOutReason(vw.ReasonID);

                    if (ras != null && string.IsNullOrEmpty(ras.ReasonName))
                        ReasonName = ras.ReasonName;
                }

                objColl.Add(new ViewPurchaseOrderObject()
                {
                    PurchaseOrderHeaderRefNo = vw.PurchaseOrderHeaderRefNo,
                    RequestedBy = vw.RequestedBy,
                    PurchaseOrderDate = vw.PurchaseOrderDate,
                    DepartmentID = vw.DepartmentID,
                    PaymentTermID = vw.PaymentTermID,
                    ShipVia = vw.ShipVia,
                    ShipTo = vw.ShipTo,
                    DateNeededBy = vw.DateNeededBy,
                    SupplierID = vw.SupplierID,
                    UserName = vw.UserName,
                    Remark = vw.Remark,
                    InventoryLocationID = vw.InventoryLocationID,
                    InventoryLocationName = vw.InventoryLocationName,
                    CreatedBy = vw.CreatedBy,
                    CreatedOn = vw.CreatedOn,
                    ModifiedBy = vw.ModifiedBy,
                    ModifiedOn = vw.ModifiedOn,
                    Status = vw.Status,
                    POType = vw.POType,
                    ApprovalDate = string.IsNullOrEmpty(vw.ApprovalDate) ? DateTime.MinValue : Convert.ToDateTime(vw.ApprovalDate),
                    ApprovedBy = vw.ApprovedBy,
                    SpecialValidFrom = vw.SpecialValidFrom,
                    SpecialValidTo = vw.SpecialValidTo,
                    ApprovalStatus = vw.ApprovalStatus,
                    SalesPersonID = vw.SalesPersonID,
                    PriceLevel = vw.PriceLevel,
                    ReasonID = vw.ReasonID,
                    DestInventoryLocationID = vw.DestInventoryLocationID,
                    WarehouseID = vw.WarehouseID,
                    IsAutoStockIn = vw.IsAutoStockIn,
                    OrderFromName = vw.OrderFromName,
                    InventoryLocation = new InventoryLocationObject() { InventoryLocationID = vw.InventoryLocationID, InventoryLocationName = vw.InventoryLocationName },
                    InventoryStockOutReason = new InventoryStockOutReasonObject() { ReasonID = vw.ReasonID, ReasonName = ReasonName }
                });
            }


            var result = new
            {
                records = objColl,
                totalRecords = poHdrColl.Count
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetPurchaseOrderHeader(string PurchaseOrderHeaderRefNo)
        {
            PurchaseOrderHeader tmpPOHdr = new PurchaseOrderHeader();
            string status = "";

            try
            {
                tmpPOHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                if (!tmpPOHdr.IsLoaded)
                {
                    status = "Not found";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (InventoryLocationController.IsDeleted((int)tmpPOHdr.InventoryLocationID))
                {
                    status = ERR_CLINIC_DELETED;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                PurchaseOrderHeader = tmpPOHdr,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetPurchaseOrderDetailList(string PurchaseOrderHeaderRefNo)
        {
            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);

            string status = "";
            var poDetails = new ArrayList();
            foreach (PurchaseOrderDetail poDet in poController.GetPODetail())
            {
                DateTime paramWHBalDate;
                if (poController.GetPOHeader().Status == "Submitted")
                    paramWHBalDate = DateTime.Now;
                else
                    paramWHBalDate = poDet.ModifiedOn.GetValueOrDefault(DateTime.Now);

                DateTime paramStockBalDate;
                if (poController.GetPOHeader().Status == "Pending")
                    paramStockBalDate = DateTime.Now;
                else
                    paramStockBalDate = poDet.CreatedOn.GetValueOrDefault(DateTime.Now);

                ItemDepartmentObject idp = new ItemDepartmentObject()
                {
                    ItemDepartmentID = poDet.Item.Category.ItemDepartmentId,
                    DepartmentName = poDet.Item.Category.ItemDepartment.DepartmentName,
                    DepartmentOrder = poDet.Item.Category.ItemDepartment.DepartmentOrder
                };

                CategoryObject cat = new CategoryObject()
                {
                    CategoryName = poDet.Item.CategoryName,
                    ItemDepartment = idp
                };

                ItemObject it = new ItemObject()
                {
                    CategoryName = poDet.Item.CategoryName,
                    ItemNo = poDet.Item.ItemNo,
                    ItemName = poDet.Item.ItemName,
                    ItemDepartmentID = poDet.Item.Category.ItemDepartmentId,
                    UOM = poDet.Item.UOM,
                    BaseLevel = poDet.Item.BaseLevel,
                    P1Price = poDet.Item.P1Price,
                    P2Price = poDet.Item.P2Price,
                    P3Price = poDet.Item.P3Price,
                    P4Price = poDet.Item.P4Price,
                    P5Price = poDet.Item.P5Price,
                    Category = cat

                };

                POrderDetailObject poD = new POrderDetailObject()
                {
                    PurchaseOrderHeaderRefNo = poDet.PurchaseOrderHeaderRefNo,
                    PurchaseOrderDetailRefNo = poDet.PurchaseOrderDetailRefNo,
                    ItemNo = poDet.ItemNo,
                    Quantity = poDet.Quantity,
                    OriginalQuantity = poDet.OriginalQuantity,
                    RejectQty = poDet.RejectQty,
                    QtyApproved = poDet.QtyApproved,
                    FactoryPrice = poDet.FactoryPrice,
                    Remark = poDet.Remark,
                    Status = poDet.Status,
                    SalesQty = poDet.SalesQty,
                    Amount = poDet.Amount,
                    GSTAmount = poDet.GSTAmount,
                    DiscountDetail = poDet.DiscountDetail,
                    ExpiryDate = poDet.ExpiryDate,
                    Item = it,
                    SerialNo = poDet.SerialNo
                };

                var tmpRow = new
                {
                    PurchaseOrderDetail = poD,
                    StockBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(poDet.ItemNo, poController.GetInventoryLocationID(), paramStockBalDate, out status),
                    WarehouseBalance = (poController.GetPOHeader().WarehouseID == 0) ? GetWarehouseBalance(poDet.ItemNo, paramWHBalDate, out status) : GetWarehouseBalanceByLocID(poController.GetPOHeader().WarehouseID, poDet.ItemNo, paramWHBalDate, out status),
                    OptimalStock = GetOptimalStock(poDet.ItemNo, (int)poController.GetPOHeader().InventoryLocationID, out status)
                };

                poDetails.Add(tmpRow);
            }

            var result = new
            {
                records = poDetails,
                status = status
            };
            //new JavaScriptSerializer().Serialize(result)

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string GetPurchaseOrderDetailListSupplierPortal(string PurchaseOrderHeaderRefNo, string username, int NumOfDays, bool isShowSalesQty)
        {
            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);

            string status = "";
            var poDetails = new ArrayList();
            foreach (PurchaseOrderDetail poDet in poController.GetPODetail())
            {
                DateTime paramWHBalDate;
                if (poController.GetPOHeader().Status == "Submitted")
                    paramWHBalDate = DateTime.Now;
                else
                    paramWHBalDate = poDet.ModifiedOn.GetValueOrDefault(DateTime.Now);

                DateTime paramStockBalDate;
                if (poController.GetPOHeader().Status == "Pending")
                    paramStockBalDate = DateTime.Now;
                else
                    paramStockBalDate = poDet.CreatedOn.GetValueOrDefault(DateTime.Now);

                ItemDepartmentObject idp = new ItemDepartmentObject()
                {
                    ItemDepartmentID = poDet.Item.Category.ItemDepartmentId,
                    DepartmentName = poDet.Item.Category.ItemDepartment.DepartmentName,
                    DepartmentOrder = poDet.Item.Category.ItemDepartment.DepartmentOrder
                };

                CategoryObject cat = new CategoryObject()
                {
                    CategoryName = poDet.Item.CategoryName,
                    ItemDepartment = idp
                };

                ItemObject it = new ItemObject()
                {
                    CategoryName = poDet.Item.CategoryName,
                    ItemNo = poDet.Item.ItemNo,
                    ItemName = poDet.Item.ItemName,
                    ItemDepartmentID = poDet.Item.Category.ItemDepartmentId,
                    UOM = poDet.Item.UOM,
                    BaseLevel = poDet.Item.BaseLevel,
                    P1Price = poDet.Item.P1Price,
                    P2Price = poDet.Item.P2Price,
                    P3Price = poDet.Item.P3Price,
                    P4Price = poDet.Item.P4Price,
                    P5Price = poDet.Item.P5Price,
                    Category = cat
                };

                POrderDetailObject poD = new POrderDetailObject()
                {
                    PurchaseOrderHeaderRefNo = poDet.PurchaseOrderHeaderRefNo,
                    PurchaseOrderDetailRefNo = poDet.PurchaseOrderDetailRefNo,
                    ItemNo = poDet.ItemNo,
                    Quantity = poDet.Quantity,
                    OriginalQuantity = poDet.OriginalQuantity,
                    RejectQty = poDet.RejectQty,
                    FactoryPrice = poDet.FactoryPrice,
                    QtyApproved = poDet.QtyApproved,
                    Remark = poDet.Remark,
                    Status = poDet.Status,
                    SalesQty = poDet.SalesQty,
                    Amount = poDet.Amount,
                    GSTAmount = poDet.GSTAmount,
                    DiscountDetail = poDet.DiscountDetail,
                    ExpiryDate = poDet.ExpiryDate,
                    SerialNo = poDet.SerialNo,
                    Item = it
                };

                decimal salesPeriod1 = 0;
                decimal salesPeriod2 = 0;
                decimal salesPeriod3 = 0;

                if (isShowSalesQty)
                {
                    salesPeriod1 = InventoryController.InventoryFetchItemSales(poDet.ItemNo, poDet.PurchaseOrderHeader.SupplierID.GetValueOrDefault(0), "1",
                        NumOfDays.ToString(), poDet.PurchaseOrderHeader.InventoryLocationID.GetValueOrDefault(0), username, out status);
                    salesPeriod2 = InventoryController.InventoryFetchItemSales(poDet.ItemNo, poDet.PurchaseOrderHeader.SupplierID.GetValueOrDefault(0), "2",
                        NumOfDays.ToString(), poDet.PurchaseOrderHeader.InventoryLocationID.GetValueOrDefault(0), username, out status);
                    salesPeriod3 = InventoryController.InventoryFetchItemSales(poDet.ItemNo, poDet.PurchaseOrderHeader.SupplierID.GetValueOrDefault(0), "3",
                        NumOfDays.ToString(), poDet.PurchaseOrderHeader.InventoryLocationID.GetValueOrDefault(0), username, out status);
                }

                var tmpRow = new
                {
                    PurchaseOrderDetail = poD,
                    StockBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(poDet.ItemNo, poController.GetInventoryLocationID(), paramStockBalDate, out status),
                    WarehouseBalance = (poController.GetPOHeader().WarehouseID == 0) ? GetWarehouseBalance(poDet.ItemNo, paramWHBalDate, out status) : GetWarehouseBalanceByLocID(poController.GetPOHeader().WarehouseID, poDet.ItemNo, paramWHBalDate, out status),
                    OptimalStock = GetOptimalStock(poDet.ItemNo, (int)poController.GetPOHeader().InventoryLocationID, out status),
                    SalesPeriod1 = salesPeriod1,
                    SalesPeriod2 = salesPeriod2,
                    SalesPeriod3 = salesPeriod3
                };

                poDetails.Add(tmpRow);
            }

            var result = new
            {
                records = poDetails,
                status = status
            };
            //new JavaScriptSerializer().Serialize(result)

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string LoadItemsFromOptimalStock(string PurchaseOrderHeaderRefNo)
        {
            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
            string status = "";
            //Create Items 
            DataTable dt = ItemBaseLevelController.FetchDataByInventoryLocation((int)poController.GetInventoryLocationID(), poController.GetPOHeader().SupplierID.GetValueOrDefault(0));

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    PurchaseOrderDetail AddedItem = new PurchaseOrderDetail();
                    decimal outletBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(dr["ItemNo"].ToString(),
                            poController.GetInventoryLocationID(), DateTime.Now, out status);
                    int optStock = GetOptimalStock(dr["ItemNo"].ToString(), (int)poController.GetPOHeader().InventoryLocationID, out status);
                    if (poController.GetPOHeader().POType.ToUpper() == "RETURN")
                    {
                        if (outletBalance - optStock > 0)
                        {
                            poController.AddItemIntoPurchaseOrder(dr["ItemNo"].ToString(), outletBalance - optStock, DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss"), out status, out AddedItem);
                        }
                    }
                    else
                    {
                        if (optStock - outletBalance > 0)
                        {
                            poController.AddItemIntoPurchaseOrder(dr["ItemNo"].ToString(), optStock - outletBalance, DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss"), out status, out AddedItem);
                        }

                    }
                }
            }

            var poDetails = new ArrayList();
            foreach (PurchaseOrderDetail poDet in poController.GetPODetail())
            {
                DateTime paramWHBalDate;
                if (poController.GetPOHeader().Status == "Submitted")
                    paramWHBalDate = DateTime.Now;
                else
                    paramWHBalDate = poDet.ModifiedOn.GetValueOrDefault(DateTime.Now);

                DateTime paramStockBalDate;
                if (poController.GetPOHeader().Status == "Pending")
                    paramStockBalDate = DateTime.Now;
                else
                    paramStockBalDate = poDet.CreatedOn.GetValueOrDefault(DateTime.Now);

                var tmpRow = new
                {
                    PurchaseOrderDetail = poDet,
                    StockBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(poDet.ItemNo, poController.GetInventoryLocationID(), paramStockBalDate, out status),
                    WarehouseBalance = (poController.GetPOHeader().WarehouseID == 0) ? GetWarehouseBalance(poDet.ItemNo, paramWHBalDate, out status) : GetWarehouseBalanceByLocID(poController.GetPOHeader().WarehouseID, poDet.ItemNo, paramWHBalDate, out status),
                    OptimalStock = GetOptimalStock(poDet.ItemNo, (int)poController.GetPOHeader().InventoryLocationID, out status)
                };

                poDetails.Add(tmpRow);
            }

            var result = new
            {
                records = poDetails,
                status = status
            };
            //new JavaScriptSerializer().Serialize(result)

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string LoadItemsFromOptimalStockSupplierPortal(string PurchaseOrderHeaderRefNo, string username)
        {
            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
            string status = "";
            //Create Items 
            DataTable dt = ItemBaseLevelController.FetchDataByInventoryLocationSupplierPortal((int)poController.GetInventoryLocationID(), poController.GetPOHeader().SupplierID.GetValueOrDefault(0), username);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    PurchaseOrderDetail AddedItem = new PurchaseOrderDetail();
                    decimal outletBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(dr["ItemNo"].ToString(),
                            poController.GetInventoryLocationID(), DateTime.Now, out status);
                    int optStock = GetOptimalStock(dr["ItemNo"].ToString(), (int)poController.GetPOHeader().InventoryLocationID, out status);
                    if (poController.GetPOHeader().POType.ToUpper() == "RETURN")
                    {
                        if (outletBalance - optStock > 0)
                        {
                            poController.AddItemIntoPurchaseOrder(dr["ItemNo"].ToString(), outletBalance - optStock, DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss"), out status, out AddedItem);
                        }
                    }
                    else
                    {
                        if (optStock - outletBalance > 0)
                        {
                            poController.AddItemIntoPurchaseOrder(dr["ItemNo"].ToString(), optStock - outletBalance, DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss"), out status, out AddedItem);
                        }

                    }
                }
            }

            var poDetails = new ArrayList();
            foreach (PurchaseOrderDetail poDet in poController.GetPODetail())
            {
                DateTime paramWHBalDate;
                if (poController.GetPOHeader().Status == "Submitted")
                    paramWHBalDate = DateTime.Now;
                else
                    paramWHBalDate = poDet.ModifiedOn.GetValueOrDefault(DateTime.Now);

                DateTime paramStockBalDate;
                if (poController.GetPOHeader().Status == "Pending")
                    paramStockBalDate = DateTime.Now;
                else
                    paramStockBalDate = poDet.CreatedOn.GetValueOrDefault(DateTime.Now);

                var tmpRow = new
                {
                    PurchaseOrderDetail = poDet,
                    StockBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(poDet.ItemNo, poController.GetInventoryLocationID(), paramStockBalDate, out status),
                    WarehouseBalance = (poController.GetPOHeader().WarehouseID == 0) ? GetWarehouseBalance(poDet.ItemNo, paramWHBalDate, out status) : GetWarehouseBalanceByLocID(poController.GetPOHeader().WarehouseID, poDet.ItemNo, paramWHBalDate, out status),
                    OptimalStock = GetOptimalStock(poDet.ItemNo, (int)poController.GetPOHeader().InventoryLocationID, out status)
                };

                poDetails.Add(tmpRow);
            }

            var result = new
            {
                records = poDetails,
                status = status
            };
            //new JavaScriptSerializer().Serialize(result)

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string LoadItemsFromSales(string PurchaseOrderHeaderRefNo, DateTime startDate, DateTime endDate)
        {

            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
            string status = "";

            InventoryLocation il = new InventoryLocation(poController.GetInventoryLocationID());
            Outlet o = new Outlet(Outlet.Columns.InventoryLocationID, il.InventoryLocationID);
            PointOfSaleCollection pColl = new PointOfSaleCollection();
            pColl.Where(PointOfSale.Columns.OutletName, o.OutletName);
            pColl.Load();


            foreach (PointOfSale p in pColl)
            {
                //Create Items 
                DataTable dt = PurchaseOrderController.FetchSalesDetailDatabyDate(startDate, endDate.AddDays(1).AddSeconds(-1), p.PointOfSaleID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PurchaseOrderDetail AddedItem = new PurchaseOrderDetail();
                        if (poController.GetPOHeader().POType.ToUpper() == "REPLENISH")
                        {
                            if (poController.AddItemIntoPurchaseOrder(dr["ItemNo"].ToString(), Convert.ToDecimal(dr["Quantity"].ToString()),
                                DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss"), dr["OrderDetID"].ToString(), out status, out AddedItem))
                            {
                                //insert into salesorder mapping
                                SalesOrderMappingController.AddSalesOrderMapping(dr["OrderDetID"].ToString(), AddedItem.PurchaseOrderDetailRefNo, Convert.ToDecimal(dr["Quantity"].ToString()));
                                POSController.UpdatePurchaseOrderReference(dr["OrderDetID"].ToString(), AddedItem.PurchaseOrderDetailRefNo);
                            }
                        }
                    }
                }

            }

            var poDetails = new ArrayList();
            foreach (PurchaseOrderDetail poDet in poController.GetPODetail())
            {
                DateTime paramWHBalDate;
                if (poController.GetPOHeader().Status == "Submitted")
                    paramWHBalDate = DateTime.Now;
                else
                    paramWHBalDate = poDet.ModifiedOn.GetValueOrDefault(DateTime.Now);

                DateTime paramStockBalDate;
                if (poController.GetPOHeader().Status == "Pending")
                    paramStockBalDate = DateTime.Now;
                else
                    paramStockBalDate = poDet.CreatedOn.GetValueOrDefault(DateTime.Now);

                var tmpRow = new
                {
                    PurchaseOrderDetail = poDet,
                    StockBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(poDet.ItemNo, poController.GetInventoryLocationID(), paramStockBalDate, out status),
                    WarehouseBalance = (poController.GetPOHeader().WarehouseID == 0) ? GetWarehouseBalance(poDet.ItemNo, paramWHBalDate, out status) : GetWarehouseBalanceByLocID(poController.GetPOHeader().WarehouseID, poDet.ItemNo, paramWHBalDate, out status),
                    OptimalStock = GetOptimalStock(poDet.ItemNo, (int)poController.GetPOHeader().InventoryLocationID, out status)
                };

                poDetails.Add(tmpRow);
            }

            var result = new
            {
                records = poDetails,
                status = status
            };
            //new JavaScriptSerializer().Serialize(result)

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string LoadItemsFromSalesSupplierPortal(string PurchaseOrderHeaderRefNo, DateTime startDate, DateTime endDate, string username)
        {

            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
            string status = "";

            InventoryLocation il = new InventoryLocation(poController.GetInventoryLocationID());
            Outlet o = new Outlet(Outlet.Columns.InventoryLocationID, il.InventoryLocationID);
            PointOfSaleCollection pColl = new PointOfSaleCollection();
            pColl.Where(PointOfSale.Columns.OutletName, o.OutletName);
            pColl.Load();


            foreach (PointOfSale p in pColl)
            {
                //Create Items 
                DataTable dt = PurchaseOrderController.FetchSalesDetailDatabyDateSupplierPortal(startDate, endDate.AddDays(1).AddSeconds(-1), p.PointOfSaleID, 0, username);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PurchaseOrderDetail AddedItem = new PurchaseOrderDetail();
                        if (poController.GetPOHeader().POType.ToUpper() == "REPLENISH")
                        {
                            if (poController.AddItemIntoPurchaseOrder(dr["ItemNo"].ToString(), Convert.ToDecimal(dr["Quantity"].ToString()),
                                DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss"), dr["OrderDetID"].ToString(), out status, out AddedItem))
                            {
                                //insert into salesorder mapping
                                SalesOrderMappingController.AddSalesOrderMapping(dr["OrderDetID"].ToString(), AddedItem.PurchaseOrderDetailRefNo, Convert.ToDecimal(dr["Quantity"].ToString()));
                                POSController.UpdatePurchaseOrderReference(dr["OrderDetID"].ToString(), AddedItem.PurchaseOrderDetailRefNo);
                            }
                        }
                    }
                }

            }

            var poDetails = new ArrayList();
            foreach (PurchaseOrderDetail poDet in poController.GetPODetail())
            {
                DateTime paramWHBalDate;
                if (poController.GetPOHeader().Status == "Submitted")
                    paramWHBalDate = DateTime.Now;
                else
                    paramWHBalDate = poDet.ModifiedOn.GetValueOrDefault(DateTime.Now);

                DateTime paramStockBalDate;
                if (poController.GetPOHeader().Status == "Pending")
                    paramStockBalDate = DateTime.Now;
                else
                    paramStockBalDate = poDet.CreatedOn.GetValueOrDefault(DateTime.Now);

                var tmpRow = new
                {
                    PurchaseOrderDetail = poDet,
                    StockBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(poDet.ItemNo, poController.GetInventoryLocationID(), paramStockBalDate, out status),
                    WarehouseBalance = (poController.GetPOHeader().WarehouseID == 0) ? GetWarehouseBalance(poDet.ItemNo, paramWHBalDate, out status) : GetWarehouseBalanceByLocID(poController.GetPOHeader().WarehouseID, poDet.ItemNo, paramWHBalDate, out status),
                    OptimalStock = GetOptimalStock(poDet.ItemNo, (int)poController.GetPOHeader().InventoryLocationID, out status)
                };

                poDetails.Add(tmpRow);
            }

            var result = new
            {
                records = poDetails,
                status = status
            };
            //new JavaScriptSerializer().Serialize(result)

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string FetchPurchaseOrderDetail(string PurchaseOrderHeaderRefNo)
        {
            DateTime startExec = DateTime.Now;
            string status = "";
            var poDetails = new ArrayList();
            bool showDeptFilter = false;

            try
            {
                int whInvLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;
                showDeptFilter = false;//(AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrder.GroupItemsByDept), false));
                PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
                DateTime paramWHBalDate;
                if (poController.GetPOHeader().Status == "Submitted")
                    paramWHBalDate = DateTime.Now;
                else
                    paramWHBalDate = poController.InvHdr.PurchaseOrderDate;

                DateTime paramStockBalDate;
                if (poController.GetPOHeader().Status == "Pending")
                    paramStockBalDate = DateTime.Now;
                else
                    paramStockBalDate = poController.InvHdr.PurchaseOrderDate;
                DataTable dt = new DataTable();
                dt = new DataTable();/*SPs.FetchPurchaseOrderDetail(poController.InvHdr.PurchaseOrderHeaderRefNo
                                    , paramStockBalDate
                                    , paramWHBalDate
                                    , poController.InvHdr.InventoryLocationID
                                    , whInvLocID).GetDataSet().Tables[0];*/
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    poDetails.Add(new
                    {
                        PurchaseOrderHeaderRefNo = (dt.Rows[i]["PurchaseOrderHeaderRefNo"] + ""),
                        PurchaseOrderDetailRefNo = (dt.Rows[i]["PurchaseOrderDetailRefNo"] + ""),
                        ItemDepartmentID = (dt.Rows[i]["ItemDepartmentID"] + ""),
                        DepartmentName = (dt.Rows[i]["DepartmentName"] + ""),
                        DepartmentOrder = (dt.Rows[i]["DepartmentOrder"] + ""),
                        CategoryName = (dt.Rows[i]["CategoryName"] + ""),
                        ItemNo = (dt.Rows[i]["ItemNo"] + ""),
                        ItemName = (dt.Rows[i]["ItemName"] + ""),
                        Quantity = (dt.Rows[i]["Quantity"] + "").GetDecimalValue(),
                        StockBalance = (dt.Rows[i]["StockBalance"] + "").GetDecimalValue(),
                        QtyApproved = (dt.Rows[i]["QtyApproved"] + "").GetDecimalValue(),
                        OptimalStock = (dt.Rows[i]["OptimalStock"] + "").GetDecimalValue(),
                        WarehouseBalance = (dt.Rows[i]["WarehouseBalance"] + "").GetDecimalValue(),
                        UOM = (dt.Rows[i]["UOM"] + ""),
                        BaseLevel = (dt.Rows[i]["BaseLevel"] + "").GetDecimalValue(),
                        Remark = (dt.Rows[i]["Remark"] + ""),
                        Status = (dt.Rows[i]["Status"] + ""),
                        SalesQty = (dt.Rows[i]["SalesQty"] + "").GetDecimalValue(),
                        PendingDeliveryQty = (dt.Rows[i]["PendingDeliveryQty"] + "").GetDecimalValue()
                    });

                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            var result = new
            {
                records = poDetails,
                status = status,
                showDeptFilter = showDeptFilter
            };
            Logger.writeLog(">>>>FetchPurchaseOrderDetail :" + DateTime.Now.Subtract(startExec).ToString());

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SavePurchaseOrderDetail(string data, string username)
        {
            PurchaseOrderDetail dataPODet = new JavaScriptSerializer().Deserialize<PurchaseOrderDetail>(data);

            PurchaseOrderController poController = new PurchaseOrderController(dataPODet.PurchaseOrderHeaderRefNo);
            PurchaseOrderDetail poDet = new PurchaseOrderDetail();
            string status = "";
            int stockBalance = 0;
            int optimalStock = 0;
            PurchaseOrderHeader poHdr = poController.GetPOHeader();

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (poHdr.DestInventoryLocationID != 0 && InventoryLocationController.IsDeleted(poHdr.DestInventoryLocationID))
            {
                status = poHdr.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (poHdr.DestInventoryLocationID != 0 && StockTakeController.IsInventoryLocationFrozen(poHdr.DestInventoryLocationID))
            {
                status = poHdr.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            status = ValidatePurchaseOrderDetail(dataPODet, poHdr);
            if (status != "")
                return status;  // No need to serialize it, it has already been serialized

            if (status == "")
            {
                stockBalance = InventoryController.GetStockBalanceQtyByItem(dataPODet.ItemNo, poController.GetInventoryLocationID(), out status).GetIntValue();
                optimalStock = ItemBaseLevelController.getOptimalStockLevel(dataPODet.ItemNo, poController.GetInventoryLocationID());
                Logger.writeLog("Optimal Stock for " + dataPODet.ItemNo + "-" + poController.GetInventoryLocationID().ToString() + ": " + optimalStock.ToString());
                poController.AddItemIntoPurchaseOrder(dataPODet.ItemNo.ToUpper(), dataPODet.Quantity ?? 0, dataPODet.ExpiryDate == null ? DateTime.Now.AddDays(7).ToString("yyyy-MM-dd") : dataPODet.ExpiryDate.ToString(), out status, out poDet);
            }

            var result = new
            {
                PurchaseOrderDetail = poDet,
                StockBalance = stockBalance,
                OptimalStock = optimalStock,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string DeletePurchaseOrderDetail(string data)
        {
            PurchaseOrderDetail dataPODet = new JavaScriptSerializer().Deserialize<PurchaseOrderDetail>(data);

            PurchaseOrderController poController = new PurchaseOrderController(dataPODet.PurchaseOrderHeaderRefNo);
            string status = "";

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (poController.GetPOHeader().DestInventoryLocationID != 0 && StockTakeController.IsInventoryLocationFrozen(poController.GetPOHeader().DestInventoryLocationID))
            {
                status = poController.GetPOHeader().DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            poController.DeleteFromPurchaseOrderDetail(dataPODet.PurchaseOrderDetailRefNo, out status);

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string DeletePurchaseOrder(string PurchaseOrderHeaderRefNo)
        {
            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
            string status = "";

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            poController.DeleteFromPurchaseOrder(PurchaseOrderHeaderRefNo, out status);

            var result = new
            {
                status = status

            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangePODetailQty(string data)
        {
            PurchaseOrderDetail dataPODet = new JavaScriptSerializer().Deserialize<PurchaseOrderDetail>(data);

            PurchaseOrderController poController = new PurchaseOrderController(dataPODet.PurchaseOrderHeaderRefNo);
            string status = "";

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            PurchaseOrderHeader poHdr = poController.GetPOHeader();
            if (poHdr.Status != "Pending")
            {
                status = "Cannot make the changes because this document is already " + poHdr.Status;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            poController.ChangeItemQty(dataPODet.PurchaseOrderDetailRefNo, dataPODet.Quantity ?? 0, out status);

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangePODetailExpiryDate(string data)
        {
            PurchaseOrderDetail dataPODet = new JavaScriptSerializer().Deserialize<PurchaseOrderDetail>(data);

            PurchaseOrderController poController = new PurchaseOrderController(dataPODet.PurchaseOrderHeaderRefNo);
            string status = "";

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            PurchaseOrderHeader poHdr = poController.GetPOHeader();
            if (poHdr.Status != "Pending")
            {
                status = "Cannot make the changes because this document is already " + poHdr.Status;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            poController.ChangeItemExpiryDate(dataPODet.PurchaseOrderDetailRefNo, (DateTime)dataPODet.ExpiryDate, out status);

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangePORemarkHeader(string PurchaseOrderHeaderRefNo, string Remarks, string username)
        {
            string status = "";

            try
            {
                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);

                if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                {
                    status = "Purchase Order Header not found";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                poHdr.Remark = Remarks;
                poHdr.Save(username);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangePOQtyApproved(string data)
        {
            PurchaseOrderDetail dataPODet = new JavaScriptSerializer().Deserialize<PurchaseOrderDetail>(data);

            PurchaseOrderController poController = new PurchaseOrderController(dataPODet.PurchaseOrderHeaderRefNo);
            string status = "";

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            PurchaseOrderHeader poHdr = poController.GetPOHeader();
            if (poHdr.Status != "Submitted")
            {
                status = "Cannot make the changes because this document is already " + poHdr.Status;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            //poController.ChangeItemQtyApproved(dataPODet.PurchaseOrderDetailRefNo, dataPODet.QtyApproved, out status);
            poController.UpdateDetailStatus(dataPODet.PurchaseOrderDetailRefNo, (decimal)dataPODet.QtyApproved, dataPODet.Remark, dataPODet.Status, out status);

            var result = new
            {
                status = status,
                detail = new PurchaseOrderDetail(dataPODet.PurchaseOrderDetailRefNo)
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string PurchaseOrderApproval(string data, string PurchaseOrderHeaderRefNo, string username)
        {
            List<PurchaseOrderDetail> dataPODetColl = new JavaScriptSerializer().Deserialize<List<PurchaseOrderDetail>>(data);

            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
            PurchaseOrderController backOrderController = new PurchaseOrderController();

            backOrderController.InvHdr.PurchaseOrderHeaderRefNo = getNewPurchaseOrderRefNo("Back Order", poController.GetInventoryLocation());
            backOrderController.InvHdr.PurchaseOrderDate = DateTime.Now;
            backOrderController.InvHdr.DateNeededBy = DateTime.Now.AddDays(1);
            backOrderController.InvHdr.POType = "Back Order";
            backOrderController.InvHdr.Status = "Submitted";
            backOrderController.InvHdr.Discount = 0;
            backOrderController.InvHdr.InventoryLocationID = poController.GetInventoryLocationID();
            backOrderController.InvHdr.RequestedBy = poController.GetPOHeader().RequestedBy;

            string status = "";
            string poHdrStatus = "";

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            PurchaseOrderHeader poHdr = poController.GetPOHeader();
            if (poHdr.Status != "Submitted")
            {
                status = "Cannot change this document's status because it is already " + poHdr.Status;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            string poType = poController.GetPOHeader().POType.ToUpper();
            foreach (var poDet in dataPODetColl)
            {
                if (poType.ToLower() == "replenish")
                {
                    if (poDet.Status == "Approved" && poDet.Item.Deleted.GetValueOrDefault(false) == true)
                    {
                        status = "Item not found: " + poDet.ItemNo;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    decimal whBal = GetWarehouseBalance(poDet.ItemNo, DateTime.Now, out status);
                    if (status != "")
                    {
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    decimal totalQtyApproved = dataPODetColl.Where(d => d.ItemNo == poDet.ItemNo).Sum(d => d.QtyApproved);
                    if (totalQtyApproved > whBal)
                    {
                        if (poType.ToLower() != "back order")
                        {
                            backOrderController.AddItemIntoInventory(poDet.ItemNo, whBal < 0 ? totalQtyApproved : totalQtyApproved - whBal, out status);
                            poDet.QtyApproved = poDet.QtyApproved - whBal;
                        }
                        else
                        {
                            status = poDet.Item.ItemName + ": Approved Quantity is greater than Quantity in Warehouse.";
                            return new JavaScriptSerializer().Serialize(new { status = status });
                        }
                        //status = poDet.Item.ItemName + ": Approved Quantity is greater than Warehouse Balance";
                        //return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                    if (poDet.QtyApproved > 0)
                        poDet.Status = "Approved";
                    else
                        poDet.Status = "Rejected";

                }

                if (poType == "RETURN" || poType == "ADJUSTMENT OUT")
                {
                    if (poDet.QtyApproved > 0)
                        poDet.Status = "Approved";
                    else
                        poDet.Status = "Rejected";
                    decimal stockBalance = InventoryController.GetStockBalanceQtyByItem(poDet.ItemNo, poController.GetInventoryLocationID(), out status).GetIntValue();
                    if (status != "")
                    {
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    decimal totalQtyApproved = dataPODetColl.Where(d => d.ItemNo == poDet.ItemNo).Sum(d => d.QtyApproved);
                    if (totalQtyApproved > stockBalance)
                    {
                        status = poDet.Item.ItemName + ": Approved Quantity is greater than Quantity in Clinic.";
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                }

                if ((poType == "ORDER" || poType == "REPLENISH") || poType == "RETURN" || poType.StartsWith("ADJUSTMENT"))
                {

                    if (poDet.QtyApproved != poDet.Quantity && (string.IsNullOrEmpty(poDet.Remark) || poDet.Remark.Trim() == ""))
                    {
                        if (poType == "ORDER" || poType == "REPLENISH")
                            status = poDet.Item.ItemName + ": Remarks is mandatory if Approved Quantity is not equal to Ordered Quantity";
                        else if (poType == "RETURN")
                            status = poDet.Item.ItemName + ": Remarks is mandatory if Approved Quantity is not equal to Returned Quantity";
                        else if (poType.StartsWith("ADJUSTMENT"))
                            status = poDet.Item.ItemName + ": Remarks is mandatory if Approved Quantity is not equal to Adjustment Quantity";

                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                }

                poController.UpdateDetailStatus(poDet.PurchaseOrderDetailRefNo, poDet.QtyApproved, poDet.Remark, poDet.Status, out status);
            }
            poController.UpdateHeaderStatus(out poHdrStatus, out status, username);

            // Refresh the PurchaseOrderHeader and pass to result
            poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
            var result = new
            {
                PurchaseOrderHeader = poHdr,
                status = status
            };

            if (poHdr.POType.ToLower() == "replenish" && poHdr.Status == "Approved")
            {
                GenerateEDIOnApproval(PurchaseOrderHeaderRefNo);

                if (backOrderController.GetPODetail().Count > 0)
                {
                    if (!backOrderController.CreateOrder(UserInfo.username, backOrderController.GetInventoryLocationID(), out status))
                    {
                        status += "Cannot Create Back Order.";
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                }

                int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;
                ArrayList list = new ArrayList();
                foreach (var poDet in dataPODetColl)
                {
                    if (poDet.QtyApproved > 0 && poDet.Status == "Approved")
                        list.Add(new { ItemNo = poDet.ItemNo, Quantity = poDet.QtyApproved });
                }
                if (invLocID != 0 && list.Count > 0)
                {
                    string dataStockOut = new JavaScriptSerializer().Serialize(list);
                    StockOut("", dataStockOut, username, 0, invLocID, false, true, "Order Approval: " + poHdr.PurchaseOrderHeaderRefNo);
                }
            }

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string PurchaseOrderApprovalGR(string data, string PurchaseOrderHeaderRefNo, string username, string autoStockIn)
        {
            Logger.writeLog("PurchaseOrderApprovalGR :" + data);
            List<PurchaseOrderDetail> dataPODetColl = new JavaScriptSerializer().Deserialize<List<PurchaseOrderDetail>>(data);

            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);

            string status = "";
            string BackOrderNumber = "";

            UserInfo.username = username;

            //Validate qty Approved change to Reject qty
            PurchaseOrderHeader oh = poController.GetPOHeader();
            string poType = oh.POType;
            if (poType.ToLower() == "replenish" || poType.ToLower() == "back order" || poType.ToLower() == "special order")
            {
                foreach (PurchaseOrderDetail od in poController.GetPODetail())
                {
                    decimal totalQtyApproved = dataPODetColl.Where(d => d.ItemNo == od.ItemNo).Sum(d => d.QtyApproved);
                    od.QtyApproved = totalQtyApproved;
                    if (od.QtyApproved == od.Quantity)
                        continue;

                    if (od.QtyApproved < od.Quantity)
                    {
                        od.OriginalQuantity = od.Quantity.GetValueOrDefault(0);
                        od.RejectQty = od.Quantity.GetValueOrDefault(0) - od.QtyApproved;
                        od.Quantity = od.QtyApproved;
                    }
                }
            }

            Logger.writeLog("PurchaseOrderApprovalGR autoStockIn: " + autoStockIn.ToUpper());
            if (PurchaseOrderController.PurchaseOrderApprovalXY(poController, PurchaseOrderHeaderRefNo, autoStockIn.ToUpper() == "YES" ? true : false, out status, out BackOrderNumber))
            {
                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                var result = new
                {
                    PurchaseOrderHeader = poHdr,
                    status = status,
                    backOrderNo = BackOrderNumber
                };

                return new JavaScriptSerializer().Serialize(result);
            }
            else
            {
                if (status.ToLower().Contains("violation"))
                    status = "Unable to do transaction. Please try to resend again.";
                return new JavaScriptSerializer().Serialize(new { status = status });
            }


            /*
            #region define backorder controller

            PurchaseOrderController backOrderController = new PurchaseOrderController();
            if (poController.InvHdr.POType == "Back Order")
            {
                backOrderController.InvHdr.PurchaseOrderHeaderRefNo = getNewPurchaseOrderRefNo("Back Order", poController.GetInventoryLocation());
                backOrderController.InvHdr.PurchaseOrderDate = DateTime.Now;
                backOrderController.InvHdr.DateNeededBy = DateTime.Now.AddDays(1);
                backOrderController.InvHdr.POType = "Back Order";
                backOrderController.InvHdr.Status = "Submitted";
                backOrderController.InvHdr.Discount = 0;
                backOrderController.InvHdr.InventoryLocationID = poController.GetInventoryLocationID();
                backOrderController.InvHdr.RequestedBy = poController.GetPOHeader().RequestedBy;
                backOrderController.InvHdr.Userfld10 = poController.GetPOHeader().Userfld10;
            }
            else
            {
                PurchaseOrderHeaderCollection bCol = new PurchaseOrderHeaderCollection();
                bCol.Where(PurchaseOrderHeader.Columns.Userfld1, "Submitted");
                bCol.Where(PurchaseOrderHeader.Columns.Userfld2, "Back Order");
                bCol.Where(PurchaseOrderHeader.Columns.InventoryLocationID, poController.GetInventoryLocationID());
                bCol.Load();

                if (bCol.Count > 0)
                {
                    backOrderController = new PurchaseOrderController(bCol[0].PurchaseOrderHeaderRefNo);
                }
                else
                {
                    backOrderController.InvHdr.PurchaseOrderHeaderRefNo = getNewPurchaseOrderRefNo("Back Order", poController.GetInventoryLocation());
                    backOrderController.InvHdr.PurchaseOrderDate = DateTime.Now;
                    backOrderController.InvHdr.DateNeededBy = DateTime.Now.AddDays(1);
                    backOrderController.InvHdr.POType = "Back Order";
                    backOrderController.InvHdr.Status = "Submitted";
                    backOrderController.InvHdr.Discount = 0;
                    backOrderController.InvHdr.InventoryLocationID = poController.GetInventoryLocationID();
                    backOrderController.InvHdr.RequestedBy = poController.GetPOHeader().RequestedBy;
                    backOrderController.InvHdr.Userfld10 = poController.GetPOHeader().Userfld10;
                }
            }

            #endregion

            #region validation
            string status = "";
            string poHdrStatus = "";

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            PurchaseOrderHeader poHdr = poController.GetPOHeader();
            if (poHdr.Status != "Submitted")
            {
                status = "Cannot change this document's status because it is already " + poHdr.Status;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }
            #endregion

            string poType = poController.GetPOHeader().POType.ToUpper();
            poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
            var result = new
            {
                PurchaseOrderHeader = poHdr,
                status = status
            };
            
                try
                {
                    foreach (var poDet in dataPODetColl)
                    {

                        if (poType.ToLower() == "replenish" || poType.ToLower() == "back order" || poType.ToLower() == "special order")
                        {
                            if (poDet.Status == "Approved" && poDet.Item.Deleted.GetValueOrDefault(false) == true)
                            {
                                status = "Item not found: " + poDet.ItemNo;
                                return new JavaScriptSerializer().Serialize(new { status = status });
                            }


                            int whBal = GetWarehouseBalance(poDet.ItemNo, DateTime.Now, out status);
                            if (status != "")
                            {
                                return new JavaScriptSerializer().Serialize(new { status = status });
                            }

                            decimal totalQtyApproved = dataPODetColl.Where(d => d.ItemNo == poDet.ItemNo).Sum(d => d.QtyApproved);
                            if (totalQtyApproved > whBal)
                            {
                                PurchaseOrderDetail AddedItem = new PurchaseOrderDetail();
                                string salesrefno = "";
                                PurchaseOrderDetail poDetRef = new PurchaseOrderDetail(poDet.PurchaseOrderDetailRefNo);
                                if (poType.ToLower() == "back order")
                                {
                                    salesrefno = poDetRef.Userfld6;
                                }
                                else
                                {
                                    salesrefno = poDet.PurchaseOrderDetailRefNo;
                                }

                                if (whBal <= 0)
                                {

                                    if (backOrderController.AddItemIntoPurchaseOrder(poDet.ItemNo, totalQtyApproved, DateTime.Today.AddDays(365).ToString("yyyy-MM-dd"), salesrefno, out status, out AddedItem))
                                    {
                                        SalesOrderMappingController.DistributeBackOrderSalesMapping(poDet.PurchaseOrderDetailRefNo, AddedItem.PurchaseOrderDetailRefNo, totalQtyApproved);
                                    }
                                    poDet.QtyApproved = 0;
                                }
                                else
                                {

                                    if (backOrderController.AddItemIntoPurchaseOrder(poDet.ItemNo, totalQtyApproved - whBal, DateTime.Today.AddDays(365).ToString("yyyy-MM-dd"), salesrefno, out status, out AddedItem))
                                    {
                                        SalesOrderMappingController.DistributeBackOrderSalesMapping(poDet.PurchaseOrderDetailRefNo, AddedItem.PurchaseOrderDetailRefNo, totalQtyApproved - whBal);
                                    }
                                    poDet.QtyApproved = whBal;

                                }
                                //status = poDet.Item.ItemName + ": Approved Quantity is greater than Warehouse Balance";
                                //return new JavaScriptSerializer().Serialize(new { status = status });
                            }
                            else
                            {
                                //create backorder if total approved / quantity
                                PurchaseOrderDetail AddedItem = new PurchaseOrderDetail();
                                string salesrefno = "";
                                PurchaseOrderDetail poDetRef = new PurchaseOrderDetail(poDet.PurchaseOrderDetailRefNo);
                                if (poType.ToLower() == "back order")
                                {
                                    salesrefno = poDetRef.Userfld6;
                                }
                                else
                                {
                                    salesrefno = poDet.PurchaseOrderDetailRefNo;
                                }

                                if (totalQtyApproved < poDet.Quantity)
                                {
                                    if (backOrderController.AddItemIntoPurchaseOrder(poDet.ItemNo, (poDet.Quantity ?? 0) - totalQtyApproved, DateTime.Today.AddDays(365).ToString("yyyy-MM-dd"), salesrefno, out status, out AddedItem))
                                    {
                                        SalesOrderMappingController.DistributeBackOrderSalesMapping(poDet.PurchaseOrderDetailRefNo, AddedItem.PurchaseOrderDetailRefNo, (poDet.Quantity ?? 0) - totalQtyApproved);
                                    }
                                }
                                else
                                {
                                    SalesOrderMappingController.ApproveAllSalesOrderMapping(poDet.PurchaseOrderDetailRefNo);
                                }


                            }
                            if (poDet.QtyApproved > 0)
                                poDet.Status = "Approved";
                            else
                                poDet.Status = "Rejected";

                        }

                        if (poType == "RETURN" || poType == "ADJUSTMENT OUT")
                        {
                            int stockBalance = InventoryController.GetStockBalanceQtyByItem(poDet.ItemNo, poController.GetInventoryLocationID(), out status).GetIntValue();
                            if (status != "")
                            {
                                return new JavaScriptSerializer().Serialize(new { status = status });
                            }

                            decimal totalQtyApproved = dataPODetColl.Where(d => d.ItemNo == poDet.ItemNo).Sum(d => d.QtyApproved);
                            if (totalQtyApproved > stockBalance)
                            {

                                //status = poDet.Item.ItemName + ": Approved Quantity is greater than Quantity in Clinic.";
                                //return new JavaScriptSerializer().Serialize(new { status = status });
                            }
                        }

                        if (poType == "ORDER" || poType == "RETURN" || poType.StartsWith("ADJUSTMENT"))
                        {
                            if (poDet.QtyApproved != poDet.Quantity && (string.IsNullOrEmpty(poDet.Remark) || poDet.Remark.Trim() == ""))
                            {
                                if (poType == "ORDER")
                                    status = poDet.Item.ItemName + ": Remarks is mandatory if Approved Quantity is not equal to Ordered Quantity";
                                else if (poType == "RETURN")
                                    status = poDet.Item.ItemName + ": Remarks is mandatory if Approved Quantity is not equal to Returned Quantity";
                                else if (poType.StartsWith("ADJUSTMENT"))
                                    status = poDet.Item.ItemName + ": Remarks is mandatory if Approved Quantity is not equal to Adjustment Quantity";

                                return new JavaScriptSerializer().Serialize(new { status = status });
                            }
                        }

                        poController.UpdateDetailStatus(poDet.PurchaseOrderDetailRefNo, poDet.QtyApproved, poDet.Remark, poDet.Status, out status);
                    }

                    string oldPOStatus = poController.GetPOHeader().Status; // save the initial status for rollback
                    poController.UpdateHeaderStatus(out poHdrStatus, out status, username);

                    // Refresh the PurchaseOrderHeader and pass to result
                    poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                    result = new
                    {
                        PurchaseOrderHeader = poHdr,
                        status = status
                    };

                    if ((poHdr.POType == "Replenish" || poHdr.POType == "Extra Order" || poHdr.POType == "Back Order" || poHdr.POType == "Pre Order") && (poHdr.Status == "Approved" || poHdr.Status == "Rejected"))
                    {
                        //GenerateEDIOnApproval(PurchaseOrderHeaderRefNo);

                        if (backOrderController.GetPODetail().Count > 0)
                        {
                            if (!backOrderController.CreateOrder(UserInfo.username, backOrderController.GetInventoryLocationID(), out status))
                            {
                                status += "Cannot Create Back Order.";
                                poHdr.Status = oldPOStatus;
                                poHdr.Save();
                                return new JavaScriptSerializer().Serialize(new { status = status });
                            }
                        }

                        int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;
                        ArrayList list = new ArrayList();
                        foreach (var poDet in dataPODetColl)
                        {
                            if (poDet.QtyApproved > 0 && poDet.Status == "Approved")
                                list.Add(new { ItemNo = poDet.ItemNo, Quantity = poDet.QtyApproved });
                        }
                        if (invLocID != 0 && list.Count > 0)
                        {
                            string stockOutRefNo = ""; string stockInRefNo = "";
                            string dataStockOut = new JavaScriptSerializer().Serialize(list);
                            string temp = StockOut(poHdr.PurchaseOrderHeaderRefNo, dataStockOut, username, 0, invLocID, false, true, "Order Approval: " + poHdr.PurchaseOrderHeaderRefNo);
                            //Logger.writeLog("Stock Out Result : " + temp);
                            StockResult StockOutres = (StockResult)new JavaScriptSerializer().Deserialize<StockResult>(temp);
                            if (StockOutres != null)
                                stockOutRefNo = StockOutres.newRefNo;

                            if (autoStockIn != null && autoStockIn.ToLower() == "yes")
                            {
                                temp = StockIn(poHdr.PurchaseOrderHeaderRefNo, dataStockOut, username, 0, (int)poHdr.InventoryLocationID, false, true, "Order Approval: " + poHdr.PurchaseOrderHeaderRefNo);
                                //Logger.writeLog("Stock In Result : " + temp);
                                StockResult StockInres = new JavaScriptSerializer().Deserialize<StockResult>(temp);
                                if (StockInres != null)
                                    stockInRefNo = StockInres.newRefNo;

                            }
                            //Update Header Ref No
                            poController.UpdateStockOutInRefNo(stockOutRefNo, stockInRefNo);
                        }
                    }
                    else if (poHdr.POType.ToUpper() == "RETURN" && poHdr.Status.ToUpper() == "APPROVED")
                    {
                        int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;
                        ArrayList list = new ArrayList();
                        foreach (var poDet in dataPODetColl)
                        {
                            if (poDet.QtyApproved > 0 && poDet.Status == "Approved")
                                list.Add(new { ItemNo = poDet.ItemNo, Quantity = poDet.QtyApproved });
                        }
                        if (invLocID != 0 && list.Count > 0)
                        {
                            // Do Stock In for warehouse
                            string stockInRefNo = "";
                            string dataStockIn = new JavaScriptSerializer().Serialize(list);
                            string temp = StockIn(poHdr.PurchaseOrderHeaderRefNo, dataStockIn, username, 0, invLocID, false, true, "Order Approval: " + poHdr.PurchaseOrderHeaderRefNo);
                            //Logger.writeLog("Stock In Result : " + temp);
                            StockResult StockInres = (StockResult)new JavaScriptSerializer().Deserialize<StockResult>(temp);
                            if (StockInres != null)
                                stockInRefNo = StockInres.newRefNo;

                            //Update Header Ref No
                            poController.UpdateStockOutInRefNo(null, stockInRefNo);
                        }
                    }
                    //ts.Complete();
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex.Message);
                    status = "Error. Cannot Process Purchase Order." + ex.Message;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }*
            

            return new JavaScriptSerializer().Serialize(result);*/
        }

        [WebMethod]
        public string PurchaseOrderApprovalGRWithPriceLevel(string data, string PurchaseOrderHeaderRefNo, string username, string autoStockIn, string priceLevel)
        {
            Logger.writeLog("PurchaseOrderApprovalGRWithPriceLevel :" + data);
            List<PurchaseOrderDetail> dataPODetColl = new JavaScriptSerializer().Deserialize<List<PurchaseOrderDetail>>(data);

            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);

            string status = "";
            string BackOrderNumber = "";

            UserInfo.username = username;

            if (!string.IsNullOrEmpty(priceLevel))
            {
                poController.GetPOHeader().PriceLevel = priceLevel;
            }

            foreach (PurchaseOrderDetail od in poController.GetPODetail())
            {
                #region *) OBSOLETE: user can manually edit each line's factory price
                //if (!string.IsNullOrEmpty(priceLevel))
                //{
                //    Item item = new Item(od.ItemNo);
                //    if (item != null && item.ItemNo == od.ItemNo)
                //    {
                //        if (priceLevel == "P1" && item.P1Price.HasValue)
                //            od.FactoryPrice = item.P1Price.Value;
                //        else if (priceLevel == "P2" && item.P2Price.HasValue)
                //            od.FactoryPrice = item.P2Price.Value;
                //        else if (priceLevel == "P3" && item.P3Price.HasValue)
                //            od.FactoryPrice = item.P3Price.Value;
                //        else if (priceLevel == "P4" && item.P4Price.HasValue)
                //            od.FactoryPrice = item.P4Price.Value;
                //        else if (priceLevel == "P5" && item.P5Price.HasValue)
                //            od.FactoryPrice = item.P5Price.Value;
                //    }
                //}
                #endregion

                od.FactoryPrice = dataPODetColl.SingleOrDefault(d => d.PurchaseOrderDetailRefNo == od.PurchaseOrderDetailRefNo).FactoryPrice;

                //Validate qty Approved change to Reject qty
                decimal totalQtyApproved = dataPODetColl.Where(d => d.ItemNo == od.ItemNo).Sum(d => d.QtyApproved);
                od.QtyApproved = totalQtyApproved;
                if (od.QtyApproved == od.Quantity)
                    continue;

                if (od.QtyApproved < od.Quantity)
                {
                    od.OriginalQuantity = od.Quantity.GetValueOrDefault(0);
                    od.RejectQty = od.Quantity.GetValueOrDefault(0) - od.QtyApproved;
                    od.Quantity = od.QtyApproved;
                }
            }

            Logger.writeLog("PurchaseOrderApprovalGRWithPriceLevel autoStockIn: " + autoStockIn.ToUpper());
            if (PurchaseOrderController.PurchaseOrderApprovalXY(poController, PurchaseOrderHeaderRefNo, autoStockIn.ToUpper() == "YES" ? true : false, out status, out BackOrderNumber))
            {
                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                var result = new
                {
                    PurchaseOrderHeader = poHdr,
                    status = status,
                    backOrderNo = BackOrderNumber
                };

                return new JavaScriptSerializer().Serialize(result);
            }
            else
            {
                if (status.ToLower().Contains("violation"))
                    status = "Unable to do transaction. Please try to resend again.";
                return new JavaScriptSerializer().Serialize(new { status = status });
            }
        }

        [WebMethod]
        public string PurchaseOrderApprovalGRWithPriceLevelOrderFrom(string data, string PurchaseOrderHeaderRefNo, string username, string autoStockIn, string priceLevel)
        {
            Logger.writeLog("PurchaseOrderApprovalGRWithPriceLevel :" + data);
            List<PurchaseOrderDetail> dataPODetColl = new JavaScriptSerializer().Deserialize<List<PurchaseOrderDetail>>(data);

            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);

            string status = "";
            string BackOrderNumber = "";

            UserInfo.username = username;

            if (!string.IsNullOrEmpty(priceLevel))
            {
                poController.GetPOHeader().PriceLevel = priceLevel;
            }

            foreach (PurchaseOrderDetail od in poController.GetPODetail())
            {
                od.FactoryPrice = dataPODetColl.SingleOrDefault(d => d.PurchaseOrderDetailRefNo == od.PurchaseOrderDetailRefNo).FactoryPrice;

                //Validate qty Approved change to Reject qty
                decimal totalQtyApproved = dataPODetColl.Where(d => d.ItemNo == od.ItemNo).Sum(d => d.QtyApproved);
                od.QtyApproved = totalQtyApproved;
                if (od.QtyApproved == od.Quantity)
                    continue;

                if (od.QtyApproved < od.Quantity)
                {
                    od.OriginalQuantity = od.Quantity.GetValueOrDefault(0);
                    od.RejectQty = od.Quantity.GetValueOrDefault(0) - od.QtyApproved;
                    od.Quantity = od.QtyApproved;
                }
            }

            Logger.writeLog("PurchaseOrderApprovalGRWithPriceLevel autoStockIn: " + autoStockIn.ToUpper());
            if (PurchaseOrderController.PurchaseOrderApprovalWithOrderFrom(poController, PurchaseOrderHeaderRefNo, autoStockIn.ToUpper() == "YES" ? true : false, out status, out BackOrderNumber))
            {
                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                var result = new
                {
                    PurchaseOrderHeader = poHdr,
                    status = status,
                    backOrderNo = BackOrderNumber
                };

                return new JavaScriptSerializer().Serialize(result);
            }
            else
            {
                if (status.ToLower().Contains("violation"))
                    status = "Unable to do transaction. Please try to resend again.";
                return new JavaScriptSerializer().Serialize(new { status = status });
            }
        }

        [WebMethod]
        public string PurchaseOrderApprovalAutoApprove(string PurchaseOrderHeaderRefNo, string username, string autoStockIn)
        {
            string status = "";
            string BackOrderNumber = "";
            if (PurchaseOrderController.PurchaseOrderApprovalXY(PurchaseOrderHeaderRefNo, autoStockIn.ToUpper() == "Y" ? true : false, out status, out BackOrderNumber))
            {
                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                var result = new
                {
                    PurchaseOrderHeader = poHdr,
                    status = status,
                    backOrderNo = BackOrderNumber
                };

                return new JavaScriptSerializer().Serialize(result);
            }
            else
            {
                if (status.ToLower().Contains("violation"))
                    status = "Unable to do transaction. Please try to resend again.";
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            // Refresh the PurchaseOrderHeader and pass to result

        }

        [WebMethod]
        public string ChangeCreditInvoiceNo(string PurchaseOrderHeaderRefNo, string CreditInvoiceNo, string username)
        {
            string status = "";

            try
            {
                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);

                if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                {
                    status = "Purchase Order Header not found";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                poHdr.ShipVia = CreditInvoiceNo;
                poHdr.Save(username);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ClearPurchaseOrderReference(string PurchaseOrderHeaderRefNo)
        {
            string status = "";
            PurchaseOrderController por = new PurchaseOrderController(PurchaseOrderHeaderRefNo);

            foreach (PurchaseOrderDetail pod in por.GetPODetail())
            {
                OrderDetCollection odCol = new OrderDetCollection();
                odCol.Where(OrderDet.Columns.Userfld9, pod.PurchaseOrderDetailRefNo);
                odCol.Load();

                foreach (OrderDet od in odCol)
                {
                    POSController.UpdatePurchaseOrderReference(od.OrderDetID, "");
                }
                //remove ref from salesordermapping
                SalesOrderMappingController.ClearPurchaseOrderReference(pod.PurchaseOrderDetailRefNo);
            }
            return new JavaScriptSerializer().Serialize(status);
        }

        [WebMethod]
        public string ClearPurchaseOrderDetailReference(string PurchaseOrderDetailRefNo)
        {
            string status = "";
            OrderDetCollection odCol = new OrderDetCollection();
            odCol.Where(OrderDet.Columns.Userfld9, PurchaseOrderDetailRefNo);
            odCol.Load();

            foreach (OrderDet od in odCol)
            {
                POSController.UpdatePurchaseOrderReference(od.OrderDetID, "");
            }
            SalesOrderMappingController.ClearPurchaseOrderReference(PurchaseOrderDetailRefNo);
            return new JavaScriptSerializer().Serialize(status);

            // Refresh the PurchaseOrderHeader and pass to result

        }

        [WebMethod]
        public string CreateBackOrder(string data, string PurchaseOrderHeaderRefNo, string username)
        {
            Logger.writeLog("Create Back Order");
            Logger.writeLog(data);
            Logger.writeLog(PurchaseOrderHeaderRefNo);
            List<PurchaseOrderDetail> dataPODetColl = new JavaScriptSerializer().Deserialize<List<PurchaseOrderDetail>>(data);

            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
            string status = "";

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            PurchaseOrderHeader poHdr = poController.GetPOHeader();
            if (poHdr.Status != "Submitted")
            {
                status = "Cannot change this document's status because it is already " + poHdr.Status;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            string poType = poController.GetPOHeader().POType.ToUpper();
            int ct = 0;
            foreach (var poDet in dataPODetColl)
            {
                decimal WarehouseBalance = GetWarehouseBalance(poDet.ItemNo, poHdr.PurchaseOrderDate, out status);
                if (poDet.Quantity - WarehouseBalance > 0)
                {
                    PurchaseOrderDetail poDetTemp = new PurchaseOrderDetail();
                    poController.AddItemIntoPurchaseOrder(poDet.ItemNo.ToUpper(), WarehouseBalance - (poDet.Quantity ?? 0), poDet.ExpiryDate == null ? DateTime.Now.AddDays(7).ToString("yyyy-MM-dd") : poDet.ExpiryDate.ToString(), out status, out poDetTemp);
                }

            }
            poController.GetPODetail().SaveAll();
            // Refresh the PurchaseOrderHeader and pass to result
            poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
            var result = new
            {
                PurchaseOrderHeader = poHdr,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetItemsToReceive(string PurchaseOrderHeaderRefNo, int InventoryLocationID)
        {
            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);

            string status = "";
            var poDetails = new ArrayList();
            bool hasBeenReceived = false;
            bool isEverReceived = false;

            PurchaseOrderHeader poHeader = poController.GetPOHeader();
            if (poHeader == null || poHeader.PurchaseOrderHeaderRefNo.ToUpper() != PurchaseOrderHeaderRefNo.ToUpper() || (poHeader.POType.ToUpper() != "ORDER" && poHeader.POType.ToUpper() != "TRANSFER" && poHeader.POType.ToUpper() != "REPLENISH"))
            {
                status = "Document Number not found.";
            }
            else if ((poHeader.POType.ToUpper() == "ORDER" || poHeader.POType.ToUpper() == "REPLENISH") && ((poHeader.Status != "Approved" && poHeader.Status != "Received" && poHeader.Status != "Posted") || (InventoryLocationID != 0 && poHeader.InventoryLocationID != InventoryLocationID)))
            {
                // To do Goods Receive for "Goods Order":
                // - the status must be Approved or Posted
                // - Clinic that receives the goods is InventoryLocationID
                status = "Document Number not found.";
            }
            //else if (poHeader.POType.ToUpper() == "TRANSFER" && ((poHeader.Status != "Submitted" && poHeader.Status != "Posted") || (InventoryLocationID != 0 && poHeader.DestInventoryLocationID != InventoryLocationID)))
            //{
            //    // To do Goods Receive for "Stock Transfer": 
            //    // - the status must be Submitted or Posted (stock transfer don't need approval)
            //    // - Clinic that receives the goods is DestInventoryLocationID
            //    status = "Document Number not found.";
            //}
            else if (InventoryLocationController.IsDeleted(poHeader.InventoryLocationID.GetValueOrDefault(0)))
            {
                status = poController.GetInventoryLocation() + ": " + ERR_CLINIC_DELETED;
            }
            //else if (poHeader.POType.ToUpper() == "TRANSFER" && InventoryLocationController.IsDeleted(poHeader.DestInventoryLocation.InventoryLocationID))
            //{
            //    // POType = TRANSFER, check the DestInventoryLocationID
            //    status = poHeader.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_DELETED;
            //}
            else
            {
                foreach (PurchaseOrderDetail poDet in poController.GetPODetail())
                {
                    if ((poHeader.POType.ToUpper() == "ORDER" || poHeader.POType.ToUpper() == "REPLENISH") && poDet.QtyApproved > 0 && poDet.Status == "Approved")
                    {
                        ItemObject it = new ItemObject()
                        {
                            CategoryName = poDet.Item.CategoryName,
                            ItemNo = poDet.Item.ItemNo,
                            ItemName = poDet.Item.ItemName,
                            ItemDepartmentID = poDet.Item.Category.ItemDepartmentId,
                            BaseLevel = poDet.Item.BaseLevel,
                            UOM = poDet.Item.UOM,
                            P1Price = poDet.Item.P1Price,
                            P2Price = poDet.Item.P2Price,
                            P3Price = poDet.Item.P3Price,
                            P4Price = poDet.Item.P4Price,
                            P5Price = poDet.Item.P5Price,
                            Barcode = poDet.Item.Barcode,
                            Deleted = poDet.Item.Deleted
                        };

                        string serialNo = String.IsNullOrEmpty(poDet.SerialNo) ? "" : poDet.SerialNo;
                        POrderDetailObject det = new POrderDetailObject()
                        {
                            PurchaseOrderHeaderRefNo = poDet.PurchaseOrderHeaderRefNo,
                            PurchaseOrderDetailRefNo = poDet.PurchaseOrderDetailRefNo,
                            Price = poDet.Price,
                            Discount = poDet.Discount,
                            ItemNo = poDet.ItemNo,
                            FactoryPrice = poDet.FactoryPrice,
                            Quantity = poDet.Quantity,
                            QtyApproved = poDet.QtyApproved,
                            OriginalQuantity = poDet.OriginalQuantity,
                            RejectQty = poDet.RejectQty,
                            Remark = poDet.Remark,
                            Status = poDet.Status,
                            SalesQty = poDet.SalesQty,
                            Amount = poDet.Amount,
                            GSTAmount = poDet.GSTAmount,
                            DiscountDetail = poDet.DiscountDetail,
                            ExpiryDate = poDet.ExpiryDate,
                            Item = it,
                            CostOfGoods = poDet.CostOfGoods,
                            SerialNo = serialNo
                        };
                        var tmpRow = new
                        {
                            PurchaseOrderDetail = poDet,
                            //QtyOutstanding = poDet.QtyApproved - InventoryController.GetReceivedQtyForPurchaseOrderByItemNo(poDet.PurchaseOrderHeaderRefNo, poDet.ItemNo)
                            QtyReceived = InventoryController.GetReceivedQtyForPurchaseOrderByItemNo(poDet.PurchaseOrderHeaderRefNo, poDet.ItemNo)
                        };

                        poDetails.Add(tmpRow);
                    }
                    //else if (poHeader.POType.ToUpper() == "TRANSFER" && poDet.Quantity > 0)
                    //{
                    //    poDet.QtyApproved = poDet.Quantity ?? 0;
                    //    poDet.MarkClean();
                    //    var tmpRow = new
                    //    {
                    //        PurchaseOrderDetail = poDet,
                    //        //QtyOutstanding = poDet.QtyApproved - InventoryController.GetReceivedQtyForPurchaseOrderByItemNo(poDet.PurchaseOrderHeaderRefNo, poDet.ItemNo)
                    //        QtyReceived = InventoryController.GetReceivedQtyForPurchaseOrderByItemNo(poDet.PurchaseOrderHeaderRefNo, poDet.ItemNo)
                    //    };

                    //    poDetails.Add(tmpRow);
                    //}
                }

                InventoryHdrCollection invHdr = new InventoryHdrCollection();
                invHdr.Where("PurchaseOrderNo", PurchaseOrderHeaderRefNo);
                invHdr.Where("MovementType", "Stock In");
                invHdr.Load();
                if (invHdr.Count > 0)
                    isEverReceived = true;

                if (poHeader.Status == "Received" || poHeader.Status == "Posted")
                    hasBeenReceived = true;
            }

            InventoryLocation fromInv = new InventoryLocation(poHeader.InventoryLocationID.GetValueOrDefault(0));
            InventoryLocation toInv = new InventoryLocation(poHeader.InventoryLocationID.GetValueOrDefault(0));

            var result = new
            {
                records = poDetails,
                InventoryLocation = new InventoryLocationObject() { InventoryLocationID = fromInv.InventoryLocationID, InventoryLocationName = fromInv.InventoryLocationName },
                DestInventoryLocation = new InventoryLocationObject() { InventoryLocationID = toInv.InventoryLocationID, InventoryLocationName = toInv.InventoryLocationName },
                POType = poHeader.POType,
                hasBeenReceived = hasBeenReceived,
                isEverReceived = isEverReceived,
                status = status
            };

            //return new JavaScriptSerializer().Serialize(result);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string RevertPOHeaderStatus(string PurchaseOrderHeaderRefNo, string poStatus)
        {
            string status = "";

            try
            {
                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                if (poHdr == null || string.IsNullOrEmpty(poHdr.PurchaseOrderHeaderRefNo))
                {
                    status = "Document Number not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                poHdr.Status = poStatus;
                poHdr.Save();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangePODetailFactoryPrice(string PurchaseOrderDetailRefNo, decimal price, string username)
        {
            string status = "";

            try
            {
                PurchaseOrderDetail poDet = new PurchaseOrderDetail(PurchaseOrderDetailRefNo);

                if (poDet == null || poDet.PurchaseOrderDetailRefNo != PurchaseOrderDetailRefNo)
                {
                    status = "Purchase Order Detail not found";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                poDet.FactoryPrice = price;
                poDet.Save(username);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangePODetailFactoryPriceAll(string PurchaseOrderHeaderRefNo, string levelPrice, string username)
        {
            string status = "";

            try
            {

                QueryCommandCollection qcol = new QueryCommandCollection();

                PurchaseOrderHeader poh = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                if (!poh.IsNew)
                {
                    poh.PriceLevel = levelPrice;
                    qcol.Add(poh.GetUpdateCommand(username));
                }


                string query = string.Format("SELECT * FROM PurchaseOrderDetail WHERE PurchaseOrderHeaderRefNo = '{0}'", PurchaseOrderHeaderRefNo);
                QueryCommand qc = new QueryCommand(query);
                PurchaseOrderDetailCollection poDet = new PurchaseOrderDetailCollection();
                poDet.LoadAndCloseReader(DataService.GetReader(qc));


                if (poDet.Count() > 0)
                {
                    foreach (PurchaseOrderDetail pd in poDet)
                    {
                        switch (levelPrice.ToLower())
                        {
                            case "p1": pd.FactoryPrice = pd.Item.P1Price.GetValueOrDefault(0);
                                break;
                            case "p2": pd.FactoryPrice = pd.Item.P2Price.GetValueOrDefault(0);
                                break;
                            case "p3": pd.FactoryPrice = pd.Item.P3Price.GetValueOrDefault(0);
                                break;
                            case "p4": pd.FactoryPrice = pd.Item.P4Price.GetValueOrDefault(0);
                                break;
                            case "p5": pd.FactoryPrice = pd.Item.P5Price.GetValueOrDefault(0);
                                break;
                            default: pd.FactoryPrice = 0;
                                break;
                        }
                        qcol.Add(pd.GetUpdateCommand(username));
                    }

                    if (qcol.Count() > 0)
                        DataService.ExecuteTransaction(qcol);
                }


            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetPurchaseHeaderSetting()
        {
            string status = "";
            string EditableAutoStockIn = "";
            string IsAutoStockIn = "";
            string TextBeautyAdvisors = "";
            string IsLockSalesPersonGR = "";

            try
            {
                EditableAutoStockIn = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EditableAutoStockIn), false).ToString();
                IsAutoStockIn = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.IsAutoStockIn), false).ToString();
                TextBeautyAdvisors = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.TextBeautyAdvisors);
                IsLockSalesPersonGR = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.IsLockSalesPersonGR), false).ToString();
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error get setting: " + ex.Message);
                status = "Error get setting: " + ex.Message;
            }

            var result = new
            {
                status = status,
                EditableAutoStockIn = EditableAutoStockIn,
                IsAutoStockIn = IsAutoStockIn,
                TextBeautyAdvisors = string.IsNullOrEmpty(TextBeautyAdvisors) ? "Beauty Advisor" : TextBeautyAdvisors,
                IsLockSalesPersonGR = IsLockSalesPersonGR
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        #endregion

        #region *) Stock Take

        public static string getNewStockTakeDocRefNo(string InventoryLocationName)
        {
            int runningNo = 0;
            string StockTakeDocRefNo;
            string header;

            header = "ST";
            header += InventoryLocationName.Left(3) + DateTime.Now.ToString("yyMMdd");

            //ST123131130
            //STPPPYYMMDD
            //PPP - Inventory Location ID
            //YY - year
            //MM - month
            //DD - day
            StockTakeDocRefNo = header;

            return StockTakeDocRefNo;
        }

        [WebMethod]
        public string GetStockTakeDocList(string filter, int skip, int take, string sortBy, bool isAscending)
        {
            var result = new
            {
                records = "",
                totalRecords = 0
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetStockTakeDoc(string StockTakeDocRefNo)
        {
            string status = "";
            try
            {
                status = "Not found";
                return new JavaScriptSerializer().Serialize(new { status = status });
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            status = "Not found";
            return new JavaScriptSerializer().Serialize(new { status = status });
        }

        [WebMethod]
        public string SaveStockTakeDoc(string data, string username)
        {
            var status = "";
            /*StockTakeDoc dataDoc;
            StockTakeDoc tmpDoc = new StockTakeDoc();

            try
            {
                dataDoc = new JavaScriptSerializer().Deserialize<StockTakeDoc>(data);

                if (dataDoc.InventoryLocationID.GetValueOrDefault(0) == 0)
                {
                    status = "Invalid Inventory Location ID";
                }

                if (status == "")
                {
                    if (InventoryLocationController.IsDeleted(dataDoc.InventoryLocation.InventoryLocationID))
                    {
                        status = ERR_CLINIC_DELETED;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    if (string.IsNullOrEmpty(dataDoc.StockTakeDocRefNo))
                    {
                        dataDoc.StockTakeDocRefNo = getNewStockTakeDocRefNo(dataDoc.InventoryLocation.InventoryLocationName);
                    }

                    tmpDoc = new StockTakeDoc(dataDoc.StockTakeDocRefNo);

                    if (tmpDoc != null && tmpDoc.StockTakeDocRefNo == dataDoc.StockTakeDocRefNo)
                    {
                        status = "Document Number " + dataDoc.StockTakeDocRefNo + " is already created.";
                    }
                    else
                    {
                        foreach (TableSchema.TableColumn col in tmpDoc.GetSchema().Columns)
                        {
                            if (col.ColumnName != "ModifiedOn" && col.ColumnName != "ModifiedBy")
                            {
                                tmpDoc.SetColumnValue(col.ColumnName, dataDoc.GetColumnValue(col.ColumnName));
                            }
                        }

                        tmpDoc.Save(username);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                StockTakeDoc = tmpDoc,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);*/
            status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });

        }

        [WebMethod]
        public string GetStockTakeList(string StockTakeDocRefNo)
        {
            string status = "";
            status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            List<ViewStockTake> stockTakes = new List<ViewStockTake>();

            try
            {
                StockTakeDoc stDoc = new StockTakeDoc(StockTakeDocRefNo);
                if (stDoc == null || stDoc.StockTakeDocRefNo != StockTakeDocRefNo)
                {
                    status = "Document Number not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                int invLocID = stDoc.InventoryLocation.InventoryLocationID;

                stockTakes = new Select().From("ViewStockTake")
                             .Where(ViewStockTake.Columns.StockTakeDocRefNo).IsEqualTo(StockTakeDocRefNo)
                             .OrderAsc(ViewStockTake.Columns.ItemNo)
                             .ExecuteTypedList<ViewStockTake>();

                foreach (ViewStockTake st in stockTakes)
                {
                    // Need to specify these dummy values or else will get error.
                    st.SystemBalQty = 0;
                    st.Defi = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = stockTakes,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);*/
        }

        [WebMethod]
        public string SaveStockTakeNEW(string Detail, string StockTakeDocRefNo, DateTime StockTakeDate, string Status, string TakenBy, string VerifiedBy, string UserName)
        {
            //string resultStatus = "";
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*try
            {
                int InventoryLocationID = 0;

                if (string.IsNullOrEmpty(StockTakeDocRefNo))
                {
                    resultStatus = "Please enter Document Number.";
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                StockTakeDoc stDoc = new StockTakeDoc(StockTakeDocRefNo);
                if (stDoc == null || string.IsNullOrEmpty(stDoc.StockTakeDocRefNo))
                {
                    resultStatus = "Document Number not found.";
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                if (stDoc.Status != "Pending")
                {
                    resultStatus = "This Document Number has been " + stDoc.Status;
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                stDoc.TakenBy = TakenBy;
                stDoc.VerifiedBy = VerifiedBy;

                if (Status.ToUpper().Equals("CANCELLED"))
                {
                    stDoc.Status = Status;
                    stDoc.Save(UserName);
                    var theResult = new
                    {
                        status = resultStatus
                    };
                    return new JavaScriptSerializer().Serialize(theResult);
                }

                InventoryLocationID = stDoc.InventoryLocationID.GetValueOrDefault(0);
                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    resultStatus = ERR_CLINIC_DELETED;
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                //if (!StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                //{
                //    resultStatus = ERR_CLINIC_NOT_FROZEN;
                //    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                //}

                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID == 0)
                {
                    resultStatus = "Please select a valid outlet.";
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                if (stDoc.InventoryLocationID != invLoc.InventoryLocationID)
                {
                    resultStatus = "Selected Outlet does not match the Outlet of the Document.";
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                List<StockTake> stockTakes = new JavaScriptSerializer().Deserialize<List<StockTake>>(Detail);
                if (stockTakes.Count == 0)
                {
                    resultStatus = "Please insert at least 1 item.";
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                QueryCommandCollection cmd = new QueryCommandCollection();
                stDoc.StockTakeDocDate = StockTakeDate;
                stDoc.Status = Status;
                cmd.Add(stDoc.GetUpdateCommand(UserName));

                string deleteQuery = string.Format("DELETE StockTake WHERE userfld1 = '{0}'", stDoc.StockTakeDocRefNo);
                cmd.Add(new QueryCommand(deleteQuery));

                foreach (StockTake st in stockTakes)
                {
                    Item item = new Item(st.ItemNo);
                    if (item == null || item.ItemNo != st.ItemNo || item.Deleted.GetValueOrDefault(false) == true)
                    {
                        resultStatus = "Item not found: " + st.ItemNo;
                        return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                    }

                    double BalQtyAtEntry = InventoryController.GetStockBalanceQtyByItemByDate(StockTakeDate, st.ItemNo, InventoryLocationID, out resultStatus);
                    double AdjustmentQty = st.StockTakeQty.GetValueOrDefault(0) - BalQtyAtEntry;

                    StockTake newST = new StockTake();
                    newST.IsAdjusted = false;
                    newST.ItemNo = st.ItemNo;
                    newST.StockTakeDate = StockTakeDate;
                    newST.StockTakeQty = st.StockTakeQty;
                    newST.BalQtyAtEntry = BalQtyAtEntry;
                    newST.AdjustmentQty = AdjustmentQty;
                    newST.QtyFromMainMenu = st.QtyFromMainMenu;
                    newST.QtyMaterial = st.QtyMaterial;
                    newST.CostOfGoods = Convert.ToDecimal(AdjustmentQty) * ItemSummaryController.GetAvgCostPrice(st.ItemNo, stDoc.InventoryLocationID.GetValueOrDefault(0));
                    newST.TakenBy = TakenBy;
                    newST.VerifiedBy = VerifiedBy;
                    newST.Marked = false;
                    newST.InventoryLocationID = InventoryLocationID;
                    newST.StockTakeDocRefNo = StockTakeDocRefNo;
                    newST.Remark = st.Remark;
                    newST.Status = "Pending";
                    newST.FixedQty = st.FixedQty;
                    newST.LooseQty = st.LooseQty;
                    newST.UniqueID = Guid.NewGuid();
                    cmd.Add(newST.GetInsertCommand(UserName));
                }

                DataService.ExecuteTransaction(cmd);

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                resultStatus = ex.Message;
            }

            var result = new
            {
                status = resultStatus
            };

            return new JavaScriptSerializer().Serialize(result);
             * */
        }


        [WebMethod]
        public string GetAllLocationWithOutstandingStockTake()
        {
            string status = "";
            status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            InventoryLocationCollection inv = new InventoryLocationCollection();

            try
            {
                inv = StockTakeController.GetAllLocationWithOutstandingStockTake();
                if (inv.Count == 0)
                {
                    status = "There is no Stock Take data to approve. All Stock Take has been approved.";
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = inv,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);*/
        }

        [WebMethod]
        public string GetStockTakeListForApproval(string filter) //, int skip, int take, string sortBy, bool isAscending
        {
            int invLocID = -1;
            string itemName = "%%";
            string StockTakeDocRefNo = "";
            //int count;
            string status = "";
            status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            List<ViewStockTake> stockTakes = new List<ViewStockTake>();

            try
            {
                var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);

                if (param.ContainsKey("inventorylocationid"))
                    int.TryParse(param["inventorylocationid"], out invLocID);

                if (invLocID == 0)
                {
                    status = "Please select a valid clinic.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (param.ContainsKey("stocktakedocrefno"))
                    StockTakeDocRefNo = param["stocktakedocrefno"];

                if (string.IsNullOrEmpty(StockTakeDocRefNo) || StockTakeDocRefNo.Trim() == "")
                {
                    status = "Please enter a Document Number.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                StockTakeDoc stDoc = new StockTakeDoc(StockTakeDocRefNo);
                if (stDoc == null || stDoc.StockTakeDocRefNo != StockTakeDocRefNo)
                {
                    status = "Document Number not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (stDoc.Status != "Pending")
                {
                    status = "This Document Number has been " + stDoc.Status;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                //if (param.ContainsKey("itemname") && !string.IsNullOrEmpty(param["itemname"]))
                //    itemName = param["itemname"];
                //else
                //    itemName = "%%";

                DataTable dt;
                StockTakeController stCtrl = new StockTakeController();
                dt = stCtrl.FetchByLocationWithFilter(invLocID, false, itemName);

                // Convert from DataTable to List of ViewStockTake
                foreach (DataRow dr in dt.Rows)
                {
                    ViewStockTake st = new ViewStockTake();
                    foreach (DataColumn col in dt.Columns)
                    {
                        st.SetColumnValue(col.ColumnName, dr[col.ColumnName]);
                    }

                    if (st.StockTakeDocRefNo == StockTakeDocRefNo)
                        stockTakes.Add(st);
                }

                stockTakes = stockTakes.Sort(ViewStockTake.Columns.ItemNo, "asc");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = stockTakes,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);*/
        }

        [WebMethod]
        public string DeleteStockTake(string data)
        {
            string status = "";
            status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            try
            {
                ArrayList stockTakeIDs = new JavaScriptSerializer().Deserialize<ArrayList>(data);
                foreach (int id in stockTakeIDs)
                {
                    StockTake.Delete(id);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);*/
        }

        [WebMethod]
        public string StockTakeApproval(string StockTakeDocRefNo, int InventoryLocationID, string docStatus, string username)
        {
            string status = "";
            status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            try
            {
                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = ERR_CLINIC_DELETED;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (!StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = ERR_CLINIC_NOT_FROZEN;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (string.IsNullOrEmpty(StockTakeDocRefNo) || StockTakeDocRefNo.Trim() == "")
                {
                    status = "Please enter a Document Number.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                StockTakeDoc stDoc = new StockTakeDoc(StockTakeDocRefNo);
                if (stDoc == null || stDoc.StockTakeDocRefNo != StockTakeDocRefNo)
                {
                    status = "Document Number not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (stDoc.Status != "Pending")
                {
                    status = "This Document Number has been " + stDoc.Status;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID == 0)
                {
                    status = "Please select a valid clinic.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (stDoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Selected Clinic does not match the Clinic of the Document.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (!StockTakeController.IsThereUnAdjustedStockTakeForInventoryLocation(InventoryLocationID))
                {
                    status = "This Clinic does not have pending stock take.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (docStatus == "Approved")
                {
                    // Check if there's invalid item
                    string sql = "SELECT st.ItemNo " +
                                 "FROM StockTake st " +
                                 "     LEFT JOIN Item it ON st.ItemNo = it.ItemNo AND it.Deleted = 0 " +
                                 "WHERE st.userfld1 = @StockTakeDocRefNo " +
                                 "      AND st.userfld2 = 'Pending' " +
                                 "      AND st.InventoryLocationID = @InventoryLocationID " +
                                 "      AND it.ItemNo IS NULL ";
                    List<Item> itemsNotFound = new InlineQuery().ExecuteTypedList<Item>(sql, StockTakeDocRefNo, InventoryLocationID);

                    if (itemsNotFound.Count > 0)
                    {
                        List<string> itemNo = itemsNotFound.ConvertAll<string>(i => i.ItemNo);

                        status = "Cannot approve this document. The following Drug Code is not found in the database:<br />" + string.Join(", ", itemNo.ToArray());
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    Query qr;
                    qr = StockTake.CreateQuery();
                    qr.QueryType = QueryType.Update;
                    //qr.AddWhere(StockTake.Columns.StockTakeID, Comparison.In, stockTakeIDs);
                    qr.AddWhere(StockTake.UserColumns.StockTakeDocRefNo, StockTakeDocRefNo);
                    qr.AddWhere(StockTake.UserColumns.Status, "Pending");
                    qr.AddWhere(StockTake.Columns.InventoryLocationID, InventoryLocationID);
                    qr.AddUpdateSetting(StockTake.Columns.Marked, true);
                    qr.Execute();

                    StockTakeController stCtrl = new StockTakeController();
                    if (stCtrl.CorrectStockTakeDiscrepancy(username, InventoryLocationID))
                    {
                        stDoc.Status = "Approved";
                        stDoc.Save(username);
                    }
                    else
                    {
                        status = "There's an error when approving the stock take.";
                    };
                }
                else if (docStatus == "Rejected")
                {
                    Query qr;
                    qr = StockTake.CreateQuery();
                    qr.QueryType = QueryType.Update;
                    qr.AddWhere(StockTake.UserColumns.StockTakeDocRefNo, StockTakeDocRefNo);
                    qr.AddWhere(StockTake.UserColumns.Status, "Pending");
                    qr.AddWhere(StockTake.Columns.InventoryLocationID, InventoryLocationID);
                    qr.AddWhere(StockTake.Columns.IsAdjusted, false);
                    qr.AddUpdateSetting(StockTake.UserColumns.Status, "Rejected");
                    qr.Execute();

                    stDoc.Status = "Rejected";
                    stDoc.Save(username);
                }
                else
                {
                    status = "Invalid Document status.";
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
             * */
        }

        [WebMethod]
        public string StockTakeUpdateMarked(int StockTakeID, bool bit)
        {
            string status = "";
            status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            try
            {
                StockTakeController stCtrl = new StockTakeController();
                if (!stCtrl.updateMarked(StockTakeID, bit))
                {
                    status = "Update failed. Please reload the data and try again.";
                };
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
             * */
        }

        [WebMethod]
        public string UpdateStockTakeQty(int StockTakeID, double Qty, string UserName)
        {
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*string resultStatus = "";
            List<ViewStockTake> updatedData = new List<ViewStockTake>();

            try
            {
                StockTake st = new StockTake(StockTake.Columns.StockTakeID, StockTakeID);
                if (st.IsNew)
                {
                    resultStatus = "StockTake not found: " + StockTakeID;
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                StockTakeDoc stDoc = new StockTakeDoc(StockTakeDoc.Columns.StockTakeDocRefNo, st.Userfld1);
                if (stDoc.IsNew)
                {
                    resultStatus = "Stock Take Doc Ref No not found: " + st.Userfld1;
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                st.QtyMaterial = Convert.ToDecimal(Qty);
                st.StockTakeQty = Convert.ToDouble(st.QtyMaterial.GetValueOrDefault(0) + st.QtyFromMainMenu.GetValueOrDefault(0));

                st.BalQtyAtEntry = InventoryController.GetStockBalanceQtyByItemByDate(stDoc.StockTakeDocDate, st.ItemNo, stDoc.InventoryLocationID.GetValueOrDefault(0), out resultStatus);
                st.AdjustmentQty = (st.StockTakeQty - st.BalQtyAtEntry);
                st.CostOfGoods = Convert.ToDecimal(st.AdjustmentQty) * ItemSummaryController.GetAvgCostPrice(st.ItemNo, stDoc.InventoryLocationID.GetValueOrDefault(0));

                QueryCommandCollection qmc = new QueryCommandCollection();
                qmc.Add(st.GetUpdateCommand(UserName));
                updatedData.Add(new ViewStockTake
                {
                    StockTakeID = st.StockTakeID,
                    QtyMaterial = st.QtyMaterial,
                    QtyFromMainMenu = st.QtyFromMainMenu,
                    StockTakeQty = st.StockTakeQty,
                    BalQtyAtEntry = st.BalQtyAtEntry,
                    AdjustmentQty = st.AdjustmentQty,
                    CostOfGoods = st.CostOfGoods,
                    SystemBalQty = 0,
                    Defi = 0
                });
                var theItem = new Item(Item.Columns.ItemNo, st.ItemNo);
                if (!theItem.IsInInventory)
                {
                    string query = @"SELECT   VRD.mainItemNo
                                        ,VRD.ItemNo
                                        ,VRD.qtyInBuyUOM
                                FROM	viewRecipeDetail VRD
                                WHERE	VRD.mainItemNo = '{0}'";
                    query = string.Format(query, theItem.ItemNo);
                    DataTable dtRecipe = new DataTable();
                    dtRecipe.Load(DataService.GetReader(new QueryCommand(query)));
                    Query qr = new Query("StockTake");
                    qr.AddWhere(StockTake.Columns.Userfld1, stDoc.StockTakeDocRefNo);
                    var stList = new StockTakeController().FetchByQuery(qr).ToList();
                    for (int count = 0; count < dtRecipe.Rows.Count; count++)
                    {
                        string recipeItemNo = (string)dtRecipe.Rows[count]["ItemNo"];
                        decimal recipeQty = (dtRecipe.Rows[count]["qtyInBuyUOM"] + "").GetDecimalValue();
                        var stRecipe = stList.Where(o => o.ItemNo == recipeItemNo).FirstOrDefault();
                        if (stRecipe != null)
                        {
                            if (!stRecipe.QtyMaterial.HasValue)
                                stRecipe.QtyMaterial = 0;
                            stRecipe.QtyFromMainMenu = recipeQty * Convert.ToDecimal(Qty);
                            stRecipe.StockTakeQty = Convert.ToDouble(stRecipe.QtyMaterial.GetValueOrDefault(0) + stRecipe.QtyFromMainMenu.GetValueOrDefault(0));

                            stRecipe.BalQtyAtEntry = InventoryController.GetStockBalanceQtyByItemByDate(stDoc.StockTakeDocDate, stRecipe.ItemNo, stDoc.InventoryLocationID.GetValueOrDefault(0), out resultStatus);
                            stRecipe.AdjustmentQty = (stRecipe.StockTakeQty - stRecipe.BalQtyAtEntry);
                            stRecipe.CostOfGoods = Convert.ToDecimal(stRecipe.AdjustmentQty) * ItemSummaryController.GetAvgCostPrice(stRecipe.ItemNo, stDoc.InventoryLocationID.GetValueOrDefault(0));
                            updatedData.Add(new ViewStockTake
                            {
                                StockTakeID = stRecipe.StockTakeID,
                                QtyMaterial = stRecipe.QtyMaterial,
                                QtyFromMainMenu = stRecipe.QtyFromMainMenu,
                                StockTakeQty = stRecipe.StockTakeQty,
                                BalQtyAtEntry = stRecipe.BalQtyAtEntry,
                                AdjustmentQty = stRecipe.AdjustmentQty,
                                CostOfGoods = stRecipe.CostOfGoods,
                                SystemBalQty = 0,
                                Defi = 0
                            });
                            qmc.Add(stRecipe.GetUpdateCommand(UserName));
                        }
                    }
                }
                DataService.ExecuteTransaction(qmc);
            }
            catch (Exception ex)
            {
                resultStatus = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
                updatedData.Clear();
            }
            var result = new
            {
                updatedData = updatedData,
                status = resultStatus
            };
            try
            {
                Logger.writeLog(">>> UpdateStockTakeQty : " + new JavaScriptSerializer().Serialize(result));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return new JavaScriptSerializer().Serialize(result);*/
        }

        [WebMethod]
        public string UpdateStockTakeRemark(int StockTakeID, string remarks, string UserName)
        {
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*string resultStatus = "";

            try
            {
                StockTake st = new StockTake(StockTake.Columns.StockTakeID, StockTakeID);
                if (st.IsNew)
                {
                    resultStatus = "StockTake not found: " + StockTakeID;
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                st.Remark = remarks;
                st.Save(UserName);
            }
            catch (Exception ex)
            {
                resultStatus = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            var result = new
            {
                status = resultStatus
            };

            return new JavaScriptSerializer().Serialize(result);*/
        }

        [WebMethod]
        public string DeleteStockTakeALL(string ListStockTakeID, string UserName)
        {
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            string resultStatus = "";

            try
            {
                string[] allID = ListStockTakeID.Split(',');
                for (int i = 0; i < allID.Length; i++)
                {
                    string stockTakeID = allID[i] + "";
                    if (!string.IsNullOrEmpty(stockTakeID)
                        && stockTakeID.GetInt32Value() > 0)
                    {
                        DeleteStockTakeNEW(stockTakeID.GetInt32Value(), UserName);
                    }
                }
            }
            catch (Exception ex)
            {
                resultStatus = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            var result = new
            {
                status = resultStatus
            };

            return new JavaScriptSerializer().Serialize(result);*/
        }

        [WebMethod]
        public string DeleteStockTakeNEW(int StockTakeID, string UserName)
        {
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            string resultStatus = "";
            QueryCommandCollection qmc = new QueryCommandCollection();

            try
            {
                StockTake st = new StockTake(StockTake.Columns.StockTakeID, StockTakeID);
                StockTakeDoc stDoc = new StockTakeDoc(StockTakeDoc.Columns.StockTakeDocRefNo, st.Userfld1);
                var theItem = new Item(Item.Columns.ItemNo, st.ItemNo);
                string queryDel = string.Format("DELETE StockTake WHERE StockTakeID = '{0}'", StockTakeID);
                //DataService.ExecuteQuery(new QueryCommand(queryDel));
                qmc.Add(new QueryCommand(queryDel));
                if (!theItem.IsInInventory)
                {
                    string query = @"SELECT   VRD.mainItemNo
                                        ,VRD.ItemNo
                                        ,VRD.qtyInBuyUOM
                                FROM	viewRecipeDetail VRD
                                WHERE	VRD.mainItemNo = '{0}'";
                    query = string.Format(query, theItem.ItemNo);

                    DataTable dtRecipe = new DataTable();
                    dtRecipe.Load(DataService.GetReader(new QueryCommand(query)));
                    Query qr = new Query("StockTake");
                    qr.AddWhere(StockTake.Columns.Userfld1, stDoc.StockTakeDocRefNo);

                    var stList = new StockTakeController().FetchByQuery(qr).ToList();
                    for (int count = 0; count < dtRecipe.Rows.Count; count++)
                    {
                        string recipeItemNo = (string)dtRecipe.Rows[count]["ItemNo"];
                        decimal recipeQty = (dtRecipe.Rows[count]["qtyInBuyUOM"] + "").GetDecimalValue();
                        var stRecipe = stList.Where(o => o.ItemNo == recipeItemNo).FirstOrDefault();
                        if (stRecipe != null)
                        {
                            stRecipe.QtyFromMainMenu = 0;
                            stRecipe.StockTakeQty = Convert.ToDouble(stRecipe.QtyMaterial.GetValueOrDefault(0) + stRecipe.QtyFromMainMenu.GetValueOrDefault(0));

                            //stRecipe.BalQtyAtEntry = InventoryController.GetStockBalanceQtyByItemByDate(stDoc.StockTakeDocDate, stRecipe.ItemNo, stDoc.InventoryLocationID.GetValueOrDefault(0), out resultStatus);
                            stRecipe.AdjustmentQty = (stRecipe.StockTakeQty - stRecipe.BalQtyAtEntry);
                            stRecipe.CostOfGoods = Convert.ToDecimal(stRecipe.AdjustmentQty) * ItemSummaryController.GetAvgCostPrice(stRecipe.ItemNo, stDoc.InventoryLocationID.GetValueOrDefault(0));
                            qmc.Add(stRecipe.GetUpdateCommand(UserName));
                        }
                    }
                }
                DataService.ExecuteTransaction(qmc);
            }
            catch (Exception ex)
            {
                resultStatus = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            var result = new
            {
                status = resultStatus
            };

            return new JavaScriptSerializer().Serialize(result);
             */
        }

        [WebMethod]
        public string AddItemToStockTakeByCategory(string StockTakeDocRefNo, string CategoryName, string UserName)
        {
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            string resultStatus = "";

            try
            {
                StockTakeDoc stDoc = new StockTakeDoc(StockTakeDoc.Columns.StockTakeDocRefNo, StockTakeDocRefNo);
                if (stDoc.IsNew)
                {
                    resultStatus = "Stock Take Doc Ref No not found: " + StockTakeDocRefNo;
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                string query = @"SELECT DISTINCT TAB.ItemNo, TAB.IsInInventory
                            FROM (
	                            SELECT   I.*
	                            FROM	Item I 
			                            LEFT JOIN Category C ON C.CategoryName = I.CategoryName
	                            WHERE	I.Deleted = 0 AND I.IsInInventory = 1
	                            UNION ALL
	                            SELECT   I.*
	                            FROM	Item I 
			                            INNER JOIN RecipeHeader RH ON RH.ItemNo = I.ItemNo
			                            LEFT JOIN Category C ON C.CategoryName = I.CategoryName
	                            WHERE	I.Deleted = 0 AND I.IsInInventory = 0 AND ISNULL(RH.Deleted,0) = 0
                            ) TAB 
                            WHERE TAB.CategoryName = '{0}'";
                query = string.Format(query, CategoryName);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(query)));
                StockTakeCollection stList = new StockTakeCollection();
                stList.Where(StockTake.Columns.Userfld1, Comparison.Equals, stDoc.StockTakeDocRefNo);
                stList.Load();

                QueryCommandCollection qmc = new QueryCommandCollection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string itemNo = (string)dt.Rows[i]["ItemNo"];
                    bool IsInInventory = (bool)dt.Rows[i]["IsInInventory"];
                    if (stList.Where(o => o.ItemNo == itemNo).FirstOrDefault() == null)
                    {
                        StockTake newST = InitStockTake(stDoc, itemNo, 0);
                        newST.StockTakeQty = null;
                        newST.BalQtyAtEntry = null;
                        newST.AdjustmentQty = null;
                        newST.CostOfGoods = 0;
                        newST.QtyFromMainMenu = null;
                        newST.QtyMaterial = null;
                        qmc.Add(newST.GetInsertCommand(UserName));

                        if (!IsInInventory)
                        {
                            query = @"SELECT   VRD.mainItemNo
                                        ,VRD.ItemNo
                                        ,VRD.qtyInBuyUOM
                                FROM	viewRecipeDetail VRD
                                WHERE	VRD.mainItemNo = '{0}'";
                            query = string.Format(query, itemNo);
                            DataTable dtRecipe = new DataTable();
                            dtRecipe.Load(DataService.GetReader(new QueryCommand(query)));
                            for (int k = 0; k < dtRecipe.Rows.Count; k++)
                            {
                                string recipeItemNo = (string)dtRecipe.Rows[k]["ItemNo"];
                                if (stList.Where(o => o.ItemNo == recipeItemNo).FirstOrDefault() == null &&
                                    dt.AsEnumerable().Where(o => o.Field<string>("ItemNo") == recipeItemNo).FirstOrDefault() == null)
                                {
                                    StockTake newSTRecipe = InitStockTake(stDoc, recipeItemNo, 0);
                                    newSTRecipe.StockTakeQty = null;
                                    newSTRecipe.BalQtyAtEntry = null;
                                    newSTRecipe.AdjustmentQty = null;
                                    newSTRecipe.CostOfGoods = 0;
                                    newSTRecipe.QtyFromMainMenu = null;
                                    newSTRecipe.QtyMaterial = null;
                                    qmc.Add(newSTRecipe.GetInsertCommand(UserName));
                                }
                            }
                        }
                    }
                }
                DataService.ExecuteTransaction(qmc);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                resultStatus = "ERROR : " + ex.Message;
            }

            var result = new
            {
                status = resultStatus
            };

            return new JavaScriptSerializer().Serialize(result);
             */
        }

        [WebMethod]
        public string AddItemToStockTakeAll(string StockTakeDocRefNo, string UserName)
        {
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            string resultStatus = "";

            try
            {
                StockTakeDoc stDoc = new StockTakeDoc(StockTakeDoc.Columns.StockTakeDocRefNo, StockTakeDocRefNo);
                if (stDoc.IsNew)
                {
                    resultStatus = "Stock Take Doc Ref No not found: " + StockTakeDocRefNo;
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                string query = @"SELECT DISTINCT TAB.ItemNo, TAB.IsInInventory
                            FROM (
	                            SELECT   I.*
	                            FROM	Item I 
			                            LEFT JOIN Category C ON C.CategoryName = I.CategoryName
	                            WHERE	I.Deleted = 0 AND I.IsInInventory = 1
	                            UNION ALL
	                            SELECT   I.*
	                            FROM	Item I 
			                            INNER JOIN RecipeHeader RH ON RH.ItemNo = I.ItemNo
			                            LEFT JOIN Category C ON C.CategoryName = I.CategoryName
	                            WHERE	I.Deleted = 0 AND I.IsInInventory = 0 AND ISNULL(RH.Deleted,0) = 0
                            ) TAB";
                query = string.Format(query);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(query)));
                StockTakeCollection stList = new StockTakeCollection();
                stList.Where(StockTake.Columns.Userfld1, Comparison.Equals, stDoc.StockTakeDocRefNo);
                stList.Load();

                QueryCommandCollection qmc = new QueryCommandCollection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string itemNo = (string)dt.Rows[i]["ItemNo"];
                    bool IsInInventory = (bool)dt.Rows[i]["IsInInventory"];
                    if (stList.Where(o => o.ItemNo == itemNo).FirstOrDefault() == null)
                    {
                        StockTake newST = InitStockTake(stDoc, itemNo, 0);
                        newST.StockTakeQty = null;
                        newST.BalQtyAtEntry = null;
                        newST.AdjustmentQty = null;
                        newST.CostOfGoods = 0;
                        newST.QtyFromMainMenu = null;
                        newST.QtyMaterial = null;
                        qmc.Add(newST.GetInsertCommand(UserName));

                        if (!IsInInventory)
                        {
                            query = @"SELECT   VRD.mainItemNo
                                        ,VRD.ItemNo
                                        ,VRD.qtyInBuyUOM
                                FROM	viewRecipeDetail VRD
                                WHERE	VRD.mainItemNo = '{0}'";
                            query = string.Format(query, itemNo);
                            DataTable dtRecipe = new DataTable();
                            dtRecipe.Load(DataService.GetReader(new QueryCommand(query)));
                            for (int k = 0; k < dtRecipe.Rows.Count; k++)
                            {
                                string recipeItemNo = (string)dtRecipe.Rows[k]["ItemNo"];
                                if (stList.Where(o => o.ItemNo == recipeItemNo).FirstOrDefault() == null &&
                                    dt.AsEnumerable().Where(o => o.Field<string>("ItemNo") == recipeItemNo).FirstOrDefault() == null)
                                {
                                    StockTake newSTRecipe = InitStockTake(stDoc, recipeItemNo, 0);
                                    newSTRecipe.StockTakeQty = null;
                                    newSTRecipe.BalQtyAtEntry = null;
                                    newSTRecipe.AdjustmentQty = null;
                                    newSTRecipe.CostOfGoods = 0;
                                    newSTRecipe.QtyFromMainMenu = null;
                                    newSTRecipe.QtyMaterial = null;
                                    qmc.Add(newSTRecipe.GetInsertCommand(UserName));
                                }
                            }
                        }
                    }
                }
                DataService.ExecuteTransaction(qmc);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                resultStatus = "ERROR : " + ex.Message;
            }

            var result = new
            {
                status = resultStatus
            };

            return new JavaScriptSerializer().Serialize(result);*/
        }

        [WebMethod]
        public string AddItemToStockTake(string ItemNo, string StockTakeDocRefNo, string UserName)
        {
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            string resultStatus = "";
            Item item = new Item(ItemNo);
            try
            {
                if (item == null || item.ItemNo != ItemNo || item.IsNew || item.Deleted.GetValueOrDefault(false) == true)
                {
                    resultStatus = "Item not found: " + ItemNo;
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                StockTakeDoc stDoc = new StockTakeDoc(StockTakeDoc.Columns.StockTakeDocRefNo, StockTakeDocRefNo);
                if (stDoc.IsNew)
                {
                    resultStatus = "Stock Take Doc Ref No not found: " + StockTakeDocRefNo;
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }
                StockTakeCollection stList = new StockTakeCollection();
                stList.Where(StockTake.Columns.Userfld1, stDoc.StockTakeDocRefNo);
                stList.Load();
                QueryCommandCollection qmc = new QueryCommandCollection();
                //for every new item, create insert statement
                if (stList.Where(o => o.ItemNo == item.ItemNo).FirstOrDefault() == null)
                {
                    StockTake newST = InitStockTake(stDoc, item.ItemNo, 0);
                    newST.StockTakeQty = null;
                    newST.BalQtyAtEntry = null;
                    newST.AdjustmentQty = null;
                    newST.CostOfGoods = 0;
                    newST.QtyFromMainMenu = null;
                    newST.QtyMaterial = null;
                    qmc.Add(newST.GetInsertCommand(UserName));
                }

                if (!item.IsInInventory)
                {
                    string query = @"SELECT   VRD.mainItemNo
                                        ,VRD.ItemNo
                                        ,VRD.qtyInBuyUOM
                                FROM	viewRecipeDetail VRD
                                WHERE	VRD.mainItemNo = '{0}'";
                    query = string.Format(query, item.ItemNo);
                    DataTable dtRecipe = new DataTable();
                    dtRecipe.Load(DataService.GetReader(new QueryCommand(query)));
                    Logger.writeLog("#####" + dtRecipe.Rows.Count);
                    for (int k = 0; k < dtRecipe.Rows.Count; k++)
                    {
                        string recipeItemNo = (string)dtRecipe.Rows[k]["ItemNo"];
                        Logger.writeLog(">>>> stList " + stList.Count);
                        if (stList.Where(o => o.ItemNo == recipeItemNo).FirstOrDefault() == null)
                        {
                            StockTake newSTRecipe = InitStockTake(stDoc, recipeItemNo, 0);
                            newSTRecipe.StockTakeQty = null;
                            newSTRecipe.BalQtyAtEntry = null;
                            newSTRecipe.AdjustmentQty = null;
                            newSTRecipe.CostOfGoods = 0;
                            newSTRecipe.QtyFromMainMenu = null;
                            newSTRecipe.QtyMaterial = null;
                            qmc.Add(newSTRecipe.GetInsertCommand(UserName));
                        }
                    }
                }
                DataService.ExecuteTransaction(qmc);
            }
            catch (Exception ex)
            {
                resultStatus = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            var result = new
            {
                status = resultStatus
            };

            return new JavaScriptSerializer().Serialize(result);*/
        }

        [WebMethod]
        public string ExportDataForStockTake(string StockTakeDocRefNo, string categoryName, string itemDeptID)
        {
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            string resultStatus = "";
            byte[] data = null;
            string itemDeptName = "";
            try
            {
                ItemDepartment id = new ItemDepartment(ItemDepartment.Columns.ItemDepartmentID, itemDeptID);
                if (!id.IsNew)
                    itemDeptName = id.DepartmentName;
                else
                    itemDeptName = "ALL";

                StockTakeDoc std = new StockTakeDoc(StockTakeDoc.Columns.StockTakeDocRefNo, StockTakeDocRefNo);
                if (!std.IsNew)
                {
                    string sql = @"SELECT   '{2}' LocationID
		                                ,'{3}' [Stock Take Date Time]
		                                ,TAB.[Department Code]
		                                ,TAB.[Department Name]
		                                ,TAB.[Category Code]
		                                ,TAB.[Category Name]
		                                ,TAB.ItemNo
		                                ,TAB.ItemName
		                                ,TAB.UOM
		                                ,TAB.Quantity
                                FROM	(
                                SELECT   ID.ItemDepartmentID [Department Code]
                                        ,ID.DepartmentName [Department Name]
                                        ,C.CategoryID [Category Code]
                                        ,C.CategoryName [Category Name]
                                        ,I.ItemNo [ItemNo]
                                        ,I.ItemName [ItemName]
                                        ,I.Userfld1 [UOM]
                                        ,NULL Quantity
                                FROM	Item I
                                        LEFT JOIN Category C ON C.CategoryName = I.CategoryName
                                        LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.itemdepartmentid
                                WHERE	I.IsInInventory = 1 AND ISNULL(I.Deleted,0) = 0 		
                                        AND ( ISNULL(ID.ItemDepartmentID,'ALL') = '{0}' OR '{0}' = 'ALL' )
                                        AND ( ISNULL(C.CategoryName,'ALL') = '{1}' OR '{1}' = 'ALL' )		
                                UNION ALL
                                SELECT   ID.ItemDepartmentID [Department Code]
                                        ,ID.DepartmentName [Department Name]
                                        ,C.CategoryID [Category Code]
                                        ,C.CategoryName [Category Name]
                                        ,I.ItemNo [ItemNo]
                                        ,I.ItemName [ItemName]
                                        ,I.Userfld1 [UOM]
                                        ,NULL Quantity
                                FROM	RecipeHeader RH
		                                INNER JOIN Item I ON I.ItemNo = RH.ItemNo
                                        LEFT JOIN Category C ON C.CategoryName = I.CategoryName
                                        LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.itemdepartmentid
                                WHERE	I.IsInInventory = 0 AND ISNULL(I.Deleted,0) = 0 	
		                                AND ISNULL(RH.Deleted,0) = 0
                                        AND ( ISNULL(ID.ItemDepartmentID,'ALL') = '{0}' OR '{0}' = 'ALL' )
                                        AND ( ISNULL(C.CategoryName,'ALL') = '{1}' OR '{1}' = 'ALL' )	
                                        ) TAB
                                ORDER BY TAB.[Department Code], TAB.[Category Name],TAB.ItemNo";
                    sql = string.Format(sql, itemDeptID
                                           , categoryName
                                           , std.InventoryLocation.InventoryLocationName
                                           , std.StockTakeDocDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    Logger.writeLog(sql);
                    DataTable dt = new DataTable();
                    dt.Load(DataService.GetReader(new QueryCommand(sql)));

                    SLDocument sl = new SLDocument();
                    var style = sl.CreateStyle();
                    style = sl.CreateStyle();
                    style.FormatCode = "yyyy-MM-dd HH:mm:ss";
                    sl.SetColumnStyle(2, style);

                    int iStartRowIndex = 1;
                    int iStartColumnIndex = 1;
                    sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, dt, true);
                    int iEndRowIndex = iStartRowIndex + dt.Rows.Count + 1 - 1;
                    int iEndColumnIndex = iStartColumnIndex + dt.Columns.Count - 1;
                    SLTable table = sl.CreateTable(iStartRowIndex, iStartColumnIndex, iEndRowIndex, iEndColumnIndex);
                    table.SetTableStyle(SLTableStyleTypeValues.Medium2);
                    table.HasTotalRow = false;
                    sl.InsertTable(table);

                    sl.AutoFitColumn(1, iEndColumnIndex);
                    sl.FreezePanes(1, 0);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        sl.SaveAs(ms);
                        data = ms.ToArray();
                    }
                }
                else
                {
                    resultStatus = "Stock Take Doc RefNo Not Found: " + StockTakeDocRefNo;
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }
            }
            catch (Exception ex)
            {
                resultStatus = ex.Message;
                Logger.writeLog(ex);
            }

            var result = new
            {
                Data = data,
                FileName = string.Format("{0}_{1}_{2}.xlsx", StockTakeDocRefNo, itemDeptName, categoryName),
                status = resultStatus
            };

            return new JavaScriptSerializer().Serialize(result);*/
        }

        [WebMethod]
        public string ImportStockTakeData(string StockTakeDocRefNo, string filePath, string UserName)
        {
            //DateTime startExec = DateTime.Now;
            //DateTime startExecCont = DateTime.Now;
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            string resultStatus = "";
            byte[] data = null;
            DateTime stockTakeDate = DateTime.MinValue;
            bool isDateSame = true;
            bool gotError = false;
            DataTable dtBalanceQtyByDate = new DataTable();
            List<string> listItemNo = new List<string>();
            try
            {
                StockTakeDoc stDoc = new StockTakeDoc(StockTakeDoc.Columns.StockTakeDocRefNo, StockTakeDocRefNo);
                if (stDoc.IsNew)
                {
                    resultStatus = "Stock Take Doc Ref No not found: " + StockTakeDocRefNo;
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }
                if (!File.Exists(filePath))
                {
                    resultStatus = "File not found: " + StockTakeDocRefNo;
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }

                DataTable dtStrongTyping = new DataTable();
                dtStrongTyping.Columns.Add("LocationID", typeof(string)); // 0
                dtStrongTyping.Columns.Add("Stock Take Date Time", typeof(DateTime));//1
                dtStrongTyping.Columns.Add("Department Code", typeof(string));//2
                dtStrongTyping.Columns.Add("Department Name", typeof(string));//3
                dtStrongTyping.Columns.Add("Category Code", typeof(string));//4
                dtStrongTyping.Columns.Add("Category Name", typeof(string));  //5
                dtStrongTyping.Columns.Add("ItemNo", typeof(string));  //6
                dtStrongTyping.Columns.Add("ItemName", typeof(string));  //7
                dtStrongTyping.Columns.Add("UOM", typeof(string));  //8
                dtStrongTyping.Columns.Add("Quantity", typeof(string));  //9
                dtStrongTyping.Columns.Add("Status", typeof(string)); //10
                List<string> listDate = new List<string>();
                using (FileStream fs = File.Open(filePath, FileMode.Open))
                {
                    using (SLDocument sl = new SLDocument(fs, "Sheet1"))
                    {
                        SLWorksheetStatistics stats = sl.GetWorksheetStatistics();
                        int iStartColumnIndex = stats.StartColumnIndex;
                        for (int row = stats.StartRowIndex + 1; row <= stats.EndRowIndex; ++row)
                        {
                            dtStrongTyping.Rows.Add(
                                sl.GetCellValueAsString(row, iStartColumnIndex),
                                sl.GetCellValueAsDateTime(row, iStartColumnIndex + 1),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 2),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 3),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 4),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 5),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 6),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 7),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 8),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 9),
                                "");
                            listItemNo.Add(sl.GetCellValueAsString(row, iStartColumnIndex + 6));
                            listDate.Add(sl.GetCellValueAsString(row, iStartColumnIndex + 1));
                        }
                    }
                }

                #region *) Set The Stock Take Date

                try
                {
                    for (int i = 0; i < dtStrongTyping.Rows.Count; i++)
                    {
                        var theDate = (DateTime)(dtStrongTyping.Rows[i]["Stock Take Date Time"]);
                        stDoc.StockTakeDocDate = theDate;
                    }
                }
                catch (Exception exDate)
                {
                    Logger.writeLog(exDate);
                }

                #endregion

                #region *) Fill Item Balance Qty By Date

                try
                {

                    string balQtyQuery = @"SELECT   ID.ItemNo
		                                    ,SUM(CASE WHEN IH.MovementType LIKE '% In' 
				                                      THEN ID.Quantity 
				                                      ELSE -1 * ID.Quantity END) Qty 
                                    FROM	InventoryHdr IH 
		                                    LEFT JOIN InventoryDet ID ON ID.InventoryHdrRefNo = IH.InventoryHdrRefNo
                                    WHERE	IH.InventoryLocationID = {0}
		                                    AND IH.InventoryDate <= '{1}'
		                                    AND ID.ItemNo IN
		                                    (
                                                '{2}'	
		                                    )
                                    GROUP BY ID.ItemNo";
                    balQtyQuery = string.Format(balQtyQuery, stDoc.InventoryLocationID.GetValueOrDefault(0)
                                                           , stDoc.StockTakeDocDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                                                           , string.Join("','", listItemNo.ToArray()));
                    Logger.writeLog(string.Format(">> Stock Take ({0}) Current Balance Qty : {1} ", stDoc.StockTakeDocRefNo, balQtyQuery));
                    dtBalanceQtyByDate.Load(DataService.GetReader(new QueryCommand(balQtyQuery)));
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }

                #endregion

                //Logger.writeLog(string.Format(">> Import Stock Take (Read Excel File) : {0}", DateTime.Now.Subtract(startExecCont)));
                //startExecCont = DateTime.Now;

                #region *) Validation
                List<StockTake> listStockTake = new List<StockTake>();
                bool isDateError = false;
                int errorDateIndex = -1;
                for (int i = 0; i < dtStrongTyping.Rows.Count; i++)
                {
                    //DateTime startExecValidation = DateTime.Now;

                    string itemNo = (string)dtStrongTyping.Rows[i]["ItemNo"];
                    string qtyStr = (string)dtStrongTyping.Rows[i]["Quantity"];
                    var theDate = (DateTime)(dtStrongTyping.Rows[i]["Stock Take Date Time"]);
                    Logger.writeLog(string.Format(">> STOCK TAKE DATE {0} : {1} ({2})", itemNo, theDate, (dtStrongTyping.Rows[i]["Stock Take Date Time"]).ToString()));
                    decimal qty = 0;
                    string lineStatus = "";
                    RecipeHeader rh = null;
                    Item theItem = null;
                    if (!string.IsNullOrEmpty(qtyStr))
                    {
                        if (theDate.Date.ToString("yyyy-MM-dd") == "1900-01-01")
                        {
                            isDateError = true;
                            errorDateIndex = i;
                            //lineStatus += "stock take date is invalid, should be in YYYY-MM-DD hh:mm:ss AM/PM, ";
                            //gotError = true;
                        }
                        else
                        {
                            if (stockTakeDate.Date == DateTime.MinValue.Date)
                            {
                                stockTakeDate = theDate;
                            }
                            else
                            {
                                if (stockTakeDate != theDate)
                                {
                                    //lineStatus = "All stock take date must be same, ";
                                    //isDateSame = false;
                                    Logger.writeLog(string.Format("Stock Take Date Error On Row : {0} [{1}]-[{2}]"
                                        , i
                                        , stockTakeDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                                        , theDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                    throw new Exception("All stock take date must be same");
                                }
                            }
                        }

                        if (!decimal.TryParse(qtyStr, out qty))
                        {
                            lineStatus += "Invalid Qty, ";
                            gotError = true;
                        }
                        theItem = new Item(Item.Columns.ItemNo, itemNo);
                        if (theItem.IsNew || theItem.Deleted.GetValueOrDefault(false))
                        {
                            lineStatus += "Item not found, ";
                            gotError = true;
                        }
                        else
                        {
                            if (!theItem.IsInInventory)
                            {
                                rh = new RecipeHeader(RecipeHeader.Columns.ItemNo, theItem.ItemNo);
                                if (rh.IsNew || rh.Deleted.GetValueOrDefault(false))
                                {
                                    lineStatus = "Not Inventory Item, ";
                                    gotError = true;
                                }
                            }
                        }
                        //Logger.writeLog(string.Format(">> Import Stock Take (Validate Input Breakdown - {1}) : {0}",
                        //    DateTime.Now.Subtract(startExecValidation), itemNo));
                        //startExecValidation = DateTime.Now;

                        if (string.IsNullOrEmpty(lineStatus))
                        {
                            if (theItem != null)
                            {
                                bool res = true;
                                var st = InitStockTake(stDoc, itemNo, Convert.ToDouble(qty));
                                double balQtyAtEntry = 0;
                                try
                                {
                                    balQtyAtEntry = (from o in dtBalanceQtyByDate.AsEnumerable()
                                                     where o.Field<string>("ItemNo") == itemNo
                                                     select o.Field<double?>("Qty"))
                                                      .FirstOrDefault()
                                                      .GetValueOrDefault(0);
                                }
                                catch (Exception X1)
                                {
                                    Logger.writeLog(X1);
                                }
                                listStockTake = AddItemToStockTakeList(stDoc, listStockTake, st, balQtyAtEntry, out res);
                                if (!res)
                                {
                                    lineStatus = "Failed Getting Balance Qty";
                                    gotError = true;
                                    //return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                                }
                                //Logger.writeLog(string.Format(">> Import Stock Take (Add Item Breakdown - {1}) : {0}",
                                //    DateTime.Now.Subtract(startExecValidation), itemNo));
                                //startExecValidation = DateTime.Now;

                                //stockTakeDate = ((string)dtStrongTyping.Rows[i]["Quantity"]).G
                                if (rh != null && !rh.IsNew)
                                {
                                    string query = @"SELECT   VRD.mainItemNo
		                                                ,VRD.ItemNo
		                                                ,VRD.qtyInBuyUOM
                                                FROM	viewRecipeDetail VRD
                                                WHERE	VRD.mainItemNo = '{0}'";
                                    query = string.Format(query, theItem.ItemNo);
                                    DataTable dtRecipe = new DataTable();
                                    dtRecipe.Load(DataService.GetReader(new QueryCommand(query)));
                                    for (int count = 0; count < dtRecipe.Rows.Count; count++)
                                    {
                                        string recipeItemNo = (string)dtRecipe.Rows[count]["ItemNo"];
                                        decimal recipeQty = (dtRecipe.Rows[count]["qtyInBuyUOM"] + "").GetDecimalValue();
                                        var recipeItem = InitStockTake(stDoc, recipeItemNo, 0);
                                        recipeItem.QtyFromMainMenu = (qty * recipeQty);
                                        recipeItem.StockTakeQty = Convert.ToDouble(recipeItem.QtyFromMainMenu.GetValueOrDefault(0));
                                        balQtyAtEntry = 0;
                                        try
                                        {
                                            balQtyAtEntry = (from o in dtBalanceQtyByDate.AsEnumerable()
                                                             where o.Field<string>("ItemNo") == recipeItemNo
                                                             select o.Field<double?>("Qty"))
                                                                    .FirstOrDefault()
                                                                    .GetValueOrDefault(0);
                                        }
                                        catch (Exception X2)
                                        {
                                            Logger.writeLog(X2);
                                        }
                                        listStockTake = AddItemToStockTakeList(stDoc, listStockTake, recipeItem, balQtyAtEntry, out res);
                                        if (!res)
                                        {
                                            lineStatus = "Failed Getting Balance Qty";
                                            gotError = true;
                                        }
                                    }
                                    //Logger.writeLog(string.Format(">> Import Stock Take (Add Recipe Item Breakdown - {1}) : {0}",
                                    //    DateTime.Now.Subtract(startExecValidation), itemNo));
                                    //startExecValidation = DateTime.Now;
                                }

                                if (string.IsNullOrEmpty(lineStatus))
                                {
                                    dtStrongTyping.Rows[i]["Status"] = lineStatus;
                                }
                            }
                        }
                        else
                        {
                            dtStrongTyping.Rows[i]["Status"] = lineStatus;
                        }
                    }
                }

                #endregion

                //Logger.writeLog(string.Format(">> Import Stock Take (Validation) : {0}", DateTime.Now.Subtract(startExecCont)));
                //startExecCont = DateTime.Now;

                using (MemoryStream ms = new MemoryStream())
                {
                    SLDocument sl = new SLDocument();
                    var style = sl.CreateStyle();
                    style = sl.CreateStyle();
                    style.FormatCode = "yyyy-MM-dd HH:mm:ss";
                    sl.SetColumnStyle(2, style);

                    int iStartRowIndex = 1;
                    int iStartColumnIndex = 1;
                    sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, dtStrongTyping, true);
                    int iEndRowIndex = iStartRowIndex + dtStrongTyping.Rows.Count + 1 - 1;
                    int iEndColumnIndex = iStartColumnIndex + dtStrongTyping.Columns.Count - 1;
                    SLTable table = sl.CreateTable(iStartRowIndex, iStartColumnIndex, iEndRowIndex, iEndColumnIndex);
                    table.SetTableStyle(SLTableStyleTypeValues.Medium2);
                    table.HasTotalRow = false;
                    sl.InsertTable(table);

                    sl.AutoFitColumn(1, iEndColumnIndex);
                    sl.FreezePanes(1, 0);

                    sl.SaveAs(ms);
                    data = ms.ToArray();
                }

                //Logger.writeLog(string.Format(">> Import Stock Take (Set Output Excel) : {0}", DateTime.Now.Subtract(startExecCont)));
                //startExecCont = DateTime.Now;
                if (isDateError)
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    string dateError = "";
                    if (errorDateIndex >= 0 && errorDateIndex < listDate.Count)
                    {
                        try
                        {
                            dateError = string.Format("'{0}' ", listDate[errorDateIndex]);
                        }
                        catch (Exception exxx)
                        {
                            Logger.writeLog(exxx);
                        }
                    }
                    resultStatus = string.Format("Stock take date {0}is invalid, should be in YYYY-MM-DD hh:mm:ss", dateError);
                    return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                }
                else
                {
                    if (listStockTake.Count > 0)
                    {
                        #region *) Importing
                        if (!gotError)
                        {
                            QueryCommandCollection cmd = new QueryCommandCollection();
                            string deleteQuery = string.Format("DELETE StockTake WHERE userfld1 = '{0}'", stDoc.StockTakeDocRefNo);
                            cmd.Add(new QueryCommand(deleteQuery));

                            stDoc.StockTakeDocDate = stockTakeDate;
                            cmd.Add(stDoc.GetUpdateCommand(UserName));

                            foreach (var st in listStockTake)
                            {
                                st.StockTakeDate = stockTakeDate;
                                cmd.Add(st.GetInsertCommand(UserName));
                            }

                            DataService.ExecuteTransaction(cmd);
                        }
                        #endregion
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                    else
                    {
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                        resultStatus = "There's no valid item to import";
                        return new JavaScriptSerializer().Serialize(new { status = resultStatus });
                    }
                }
                //Logger.writeLog(string.Format(">> Import Stock Take (Execute Save Command) : {0}", DateTime.Now.Subtract(startExecCont)));
                //startExecCont = DateTime.Now;
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                resultStatus = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            string fname = "Result.xlsx";
            if (gotError)
            {
                fname = "Error_Result.xlsx";
            }

            var result = new
            {
                Data = data,
                StockTakeDate = stockTakeDate,
                FileName = string.Format(fname),
                status = resultStatus
            };
            //Logger.writeLog(string.Format(">> Import Stock Take Finish : {0}", DateTime.Now.Subtract(startExec)));
            return new JavaScriptSerializer().Serialize(result);*/
        }

        /*private StockTake InitStockTake(StockTakeDoc stDoc, string itemNo, double qty)
        {
            string status = "Method not yet implemented.";
            return null;
            
            //string resultStatus = "";
            StockTake st = new StockTake();
            st.ItemNo = itemNo;
            st.IsAdjusted = false;
            st.StockTakeDate = stDoc.StockTakeDocDate;
            st.StockTakeQty = qty;
            st.QtyMaterial = Convert.ToDecimal(qty);
            st.BalQtyAtEntry = 0;// InventoryController.GetStockBalanceQtyByItemByDate(stDoc.StockTakeDocDate, itemNo, stDoc.InventoryLocationID.GetValueOrDefault(0), out resultStatus);
            st.AdjustmentQty = 0;// (qty - st.BalQtyAtEntry);
            st.CostOfGoods = 0;// Convert.ToDecimal(st.AdjustmentQty) * ItemSummaryController.FetchCostPriceForStockTake(st.ItemNo, stDoc.InventoryLocationID.GetValueOrDefault(0));
            st.TakenBy = stDoc.TakenBy + "";
            st.VerifiedBy = stDoc.VerifiedBy + "";
            st.Marked = false;
            st.InventoryLocationID = stDoc.InventoryLocationID.GetValueOrDefault(0);
            st.StockTakeDocRefNo = stDoc.StockTakeDocRefNo;
            st.Status = "Pending";
            st.FixedQty = 0;
            st.LooseQty = 0;
            st.UniqueID = Guid.NewGuid();
            st.Remark = "";
            return st;
        }*/

        /*private List<StockTake> AddItemToStockTakeList(StockTakeDoc stDoc, List<StockTake> data, StockTake st, double balQtyAtEntry, out bool res)
        {
            string status = "Method not yet implemented.";
            res = false;
            return null;
            
            string resultStatus = "";
            bool isFound = false;
            res = true;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].ItemNo == st.ItemNo)
                {
                    data[i].QtyFromMainMenu = data[i].QtyFromMainMenu.GetValueOrDefault(0) + st.QtyFromMainMenu.GetValueOrDefault(0);
                    data[i].QtyMaterial = data[i].QtyMaterial.GetValueOrDefault(0) + st.QtyMaterial.GetValueOrDefault(0);
                    data[i].StockTakeQty = Convert.ToDouble(data[i].QtyFromMainMenu.GetValueOrDefault(0) +
                                                    data[i].QtyMaterial.GetValueOrDefault(0));
                    data[i].BalQtyAtEntry = balQtyAtEntry;//InventoryController.GetStockBalanceQtyByItemByDate(stDoc.StockTakeDocDate, data[i].ItemNo, stDoc.InventoryLocationID.GetValueOrDefault(0), out resultStatus);
                    if (resultStatus != "") res = false;
                    data[i].AdjustmentQty = (data[i].StockTakeQty - data[i].BalQtyAtEntry);
                    data[i].CostOfGoods = Convert.ToDecimal(data[i].AdjustmentQty) * ItemSummaryController.GetAvgCostPrice(data[i].ItemNo, stDoc.InventoryLocationID.GetValueOrDefault(0));
                    isFound = true;
                    break;
                }
            }
            if (!isFound)
            {
                st.StockTakeQty = Convert.ToDouble(st.QtyFromMainMenu.GetValueOrDefault(0) + st.QtyMaterial.GetValueOrDefault(0));
                st.BalQtyAtEntry = balQtyAtEntry;// InventoryController.GetStockBalanceQtyByItemByDate(stDoc.StockTakeDocDate, st.ItemNo, stDoc.InventoryLocationID.GetValueOrDefault(0), out resultStatus);
                if (resultStatus != "") res = false;
                st.AdjustmentQty = (st.StockTakeQty - st.BalQtyAtEntry);
                st.CostOfGoods = Convert.ToDecimal(st.AdjustmentQty) * ItemSummaryController.GetAvgCostPrice(st.ItemNo, stDoc.InventoryLocationID.GetValueOrDefault(0));
                if (res)
                    data.Add(st);
            }

            return data;
             
        }*/

        [WebMethod]
        public string SaveStockTake(string detail, string StockTakeDocRefNo,
                DateTime StockTakeDate, string TakenBy, string VerifiedBy,
                int InventoryLocationID, string username)
        {
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            string status = "";

            try
            {
                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = ERR_CLINIC_DELETED;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (!StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = ERR_CLINIC_NOT_FROZEN;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (string.IsNullOrEmpty(StockTakeDocRefNo))
                {
                    status = "Please enter Document Number.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                StockTakeDoc stDoc = new StockTakeDoc(StockTakeDocRefNo);
                if (stDoc == null || string.IsNullOrEmpty(stDoc.StockTakeDocRefNo))
                {
                    status = "Document Number not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (stDoc.Status != "Pending")
                {
                    status = "This Document Number has been " + stDoc.Status;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID == 0)
                {
                    status = "Please select a valid outlet.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (stDoc.InventoryLocationID != invLoc.InventoryLocationID)
                {
                    status = "Selected Outlet does not match the Outlet of the Document.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (string.IsNullOrEmpty(TakenBy))
                {
                    status = "Please specify stock take personnel.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (string.IsNullOrEmpty(VerifiedBy))
                {
                    status = "Please specify stock take verifier.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                List<StockTake> stockTakes = new JavaScriptSerializer().Deserialize<List<StockTake>>(detail);
                if (stockTakes.Count == 0)
                {
                    status = "Please insert at least 1 item.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                QueryCommandCollection cmd = new QueryCommandCollection();
                foreach (StockTake st in stockTakes)
                {
                    Item item = new Item(st.ItemNo);
                    if (item == null || item.ItemNo != st.ItemNo || item.Deleted.GetValueOrDefault(false) == true)
                    {
                        status = "Item not found: " + st.ItemNo;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    double BalQtyAtEntry = InventoryController.GetStockBalanceQtyByItemByDate(StockTakeDate, st.ItemNo, InventoryLocationID, out status);
                    double AdjustmentQty = st.StockTakeQty.GetValueOrDefault(0) - BalQtyAtEntry;

                    //check if stocktake entry exist
                    SqlQuery qry = new Select().Top("1").From("StockTake")
                                   .Where(StockTake.Columns.ItemNo).IsEqualTo(st.ItemNo)
                                   .And(StockTake.Columns.InventoryLocationID).IsEqualTo(InventoryLocationID)
                                   .And(StockTake.Columns.IsAdjusted).IsEqualTo(false)
                                   .And(StockTake.UserColumns.Status).IsEqualTo("Pending");
                    StockTake tmpST = qry.ExecuteSingle<StockTake>();

                    if (tmpST != null && tmpST.StockTakeID != 0)
                    {
                        //if exist, update the count qty

                        tmpST.StockTakeQty = st.StockTakeQty;
                        tmpST.BalQtyAtEntry = BalQtyAtEntry;
                        tmpST.AdjustmentQty = AdjustmentQty;
                        tmpST.TakenBy = TakenBy;
                        tmpST.VerifiedBy = VerifiedBy;
                        tmpST.StockTakeDate = StockTakeDate;
                        tmpST.StockTakeDocRefNo = StockTakeDocRefNo;
                        tmpST.Remark = st.Remark;
                        tmpST.FixedQty = st.FixedQty;
                        tmpST.LooseQty = st.LooseQty;
                        cmd.Add(tmpST.GetUpdateCommand(username));
                    }
                    else
                    {
                        //for every new item, create insert statement
                        StockTake newST = new StockTake();
                        newST.IsAdjusted = false;
                        newST.ItemNo = st.ItemNo;
                        newST.StockTakeDate = StockTakeDate;
                        newST.StockTakeQty = st.StockTakeQty;
                        newST.BalQtyAtEntry = BalQtyAtEntry;
                        newST.AdjustmentQty = AdjustmentQty;
                        newST.CostOfGoods = InventoryController.FetchAverageCostOfGoodsLeftByItemNo(BalQtyAtEntry, st.ItemNo, InventoryLocationID);
                        newST.TakenBy = TakenBy;
                        newST.VerifiedBy = VerifiedBy;
                        newST.Marked = false;
                        newST.InventoryLocationID = InventoryLocationID;
                        newST.StockTakeDocRefNo = StockTakeDocRefNo;
                        newST.Remark = st.Remark;
                        newST.Status = "Pending";
                        newST.FixedQty = st.FixedQty;
                        newST.LooseQty = st.LooseQty;
                        newST.UniqueID = Guid.NewGuid();
                        cmd.Add(newST.GetInsertCommand(username));
                    }
                }

                DataService.ExecuteTransaction(cmd);

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);*/
        }

        #endregion

        #region Stock Transfer

        [WebMethod]
        public string FetchStockTransferList(string filter, int take, int skip, string orderBy, string orderDir)
        {
            int fromInvLocID = 0;
            int toInvLocID = 0;
            string docNo = "";
            string status = "";

            var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);

            if (param.ContainsKey("FromInventoryLocationID"))
                fromInvLocID = (param["FromInventoryLocationID"] + "").GetIntValue();
            if (param.ContainsKey("ToInventoryLocationID"))
                toInvLocID = (param["ToInventoryLocationID"] + "").GetIntValue();
            if (param.ContainsKey("StockTransferHdrRefNo"))
                docNo = (param["StockTransferHdrRefNo"] + "");
            if (param.ContainsKey("Status"))
                status = (param["Status"] + "");

            int totalData = 0;
            var data = StockTransferController.FetchTransferList(fromInvLocID, toInvLocID, docNo, status, take, skip, orderBy, orderDir, out totalData);

            var result = new
            {
                records = data,
                totalRecords = totalData
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SaveStockTransferHdr(string data, string userName)
        {
            TransferHdr hdr = new JavaScriptSerializer().Deserialize<TransferHdr>(data);
            string status = "";
            string stockTransferHdrRefNo = "";

            StockTransferController.SaveTransferHdr(hdr, userName, out stockTransferHdrRefNo, out status);

            TransferHdr transferData = new TransferHdr();
            if (string.IsNullOrEmpty(status))
                transferData = StockTransferController.FetchTransferHdr(stockTransferHdrRefNo, out status);

            var result = new
            {
                status = status,
                data = transferData
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string StockTransferApproval(string data, string stockTransferHdrRefNo, string username, string priceLevel, bool autoStockIn)
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.StockTransferWillGoThroughWarehouse), false))
                return StockTransferController.StockTransferApprovalThroughWarehouse(data, stockTransferHdrRefNo, username, priceLevel, autoStockIn);
            else
                return StockTransferController.StockTransferApproval(data, stockTransferHdrRefNo, username, priceLevel);
        }

        [WebMethod]
        public string FetchStockTransferHdr(string stockTransferHdrRefNo)
        {
            string status = "";

            var transferHdrData = StockTransferController.FetchTransferHdr(stockTransferHdrRefNo, out status);

            var result = new
            {
                status = status,
                hdrData = transferHdrData
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string FetchStockTransferDet(string stockTransferHdrRefNo)
        {
            var data = StockTransferController.FetchTransferDet(stockTransferHdrRefNo);

            var result = new
            {
                records = data,
                status = ""
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string AddStockTransferItem(string stockTransferHdrRefNo, string itemNo, double qty, string userName)
        {
            string status = "";
            TransferDet transferDet = new TransferDet();

            bool isSuccess = StockTransferController.AddTransferItem(stockTransferHdrRefNo, itemNo, qty, userName, out transferDet, out status);

            var result = new
            {
                transferDet = transferDet,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string AddAllTransferItems(string stockTransferHdrRefNo, string userName)
        {
            string status = "";
            bool isSuccess = StockTransferController.AddAllTransferItems(stockTransferHdrRefNo, userName, out status);

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangeStockTransferQty(string stockTransferDetRefNo, double qty, string userName)
        {
            string status = "";
            bool isSuccess = StockTransferController.ChangeItemQty(stockTransferDetRefNo, qty, userName, out status);

            var result = new
            {
                isSuccess = isSuccess,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string DeleteStockTransferItem(string stockTransferDetRefNo, string userName)
        {
            string status = "";

            List<string> itemToDelete = new JavaScriptSerializer().Deserialize<List<string>>(stockTransferDetRefNo);
            if (itemToDelete.Count > 0)
                StockTransferController.DeleteTransferItem(itemToDelete, userName, out status);

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangeFullFilledQty(string stockTransferDetRefNo, double fullFilledQty, string remark, string userName)
        {
            string status = "";
            TransferDet transDet = new TransferDet();
            bool isSuccess = StockTransferController.ChangeFullFilledQty(stockTransferDetRefNo, fullFilledQty, remark, userName, out status, out transDet);

            var result = new
            {
                transferDet = transDet,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string RejectTransferDetail(string stockTransferDetRefNo, string userName)
        {
            string status = "";
            List<string> itemToDelete = new JavaScriptSerializer().Deserialize<List<string>>(stockTransferDetRefNo);

            if (itemToDelete.Count > 0)
                StockTransferController.RejectTransferDetail(itemToDelete, userName, out status);

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SetTallyAllStockTransfer(string stockTransferDetRefNo, string userName)
        {
            string status = "";
            List<string> itemToDelete = new JavaScriptSerializer().Deserialize<List<string>>(stockTransferDetRefNo);
            if (itemToDelete.Count > 0)
                StockTransferController.SetTallyAllStockTransfer(itemToDelete, userName, out status);

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ReceiveStockTransfer(string stockTransferHdrRefNo, string userName)
        {
            string status = "";
            bool isSuccess = false;

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.StockTransferWillGoThroughWarehouse), false))
                isSuccess = StockTransferController.ReceiveStockTransferThroughWarehouse(stockTransferHdrRefNo, userName, out status);
            else
                isSuccess = StockTransferController.ReceiveStockTransfer(stockTransferHdrRefNo, userName, out status);

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ReceiveAllTallyStockTransfer(string stockTransferHdrRefNo, string userName)
        {
            string status = "";

            bool isSuccess = StockTransferController.SetTallyAllStockTransfer(stockTransferHdrRefNo, userName, out status);
            //if(isSuccess)
            //    isSuccess = StockTransferController.ReceiveStockTransfer(stockTransferHdrRefNo, userName, out status);
            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }


        [WebMethod]
        public string RevertStockTransferHdr(string stockTransferHdrRefNo, string userName)
        {
            string status = "";
            TransferHdr transferHdrData = null;
            try
            {
                StockTransferHdr sth = new StockTransferHdr(stockTransferHdrRefNo);
                if (sth.IsNew)
                    throw new Exception("Stock Transfer not found");

                if (sth.Status != "Submitted")
                    throw new Exception("Cannot revert stock transfer when status is not Submitted");


                sth.Status = "Pending";
                sth.Save(userName);

                transferHdrData = StockTransferController.FetchTransferHdr(stockTransferHdrRefNo, out status);
            }
            catch (Exception ex)
            {
                status = "Error Revert Status :" + ex.Message;
            }

            var result = new
            {
                status = status,
                hdrData = transferHdrData
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangeCreditInvoiceNoST(string StockTransferHdrRefNo, string CreditInvoiceNo, string userName)
        {
            string status = "";
            TransferHdr transferHdrData = null;
            try
            {
                if (!StockTransferController.ChangeCreditInvoiceNo(StockTransferHdrRefNo, CreditInvoiceNo, userName, out status))
                    throw new Exception(status);

                transferHdrData = StockTransferController.FetchTransferHdr(StockTransferHdrRefNo, out status);
            }
            catch (Exception ex)
            {
                status = "Error Change Credit Invoice No Status :" + ex.Message;
            }

            var result = new
            {
                status = status,
                hdrData = transferHdrData
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangeInvoiceNoST(string StockTransferHdrRefNo, string InvoiceNo, string userName)
        {
            string status = "";
            TransferHdr transferHdrData = null;
            try
            {
                if (!StockTransferController.ChangeInvoiceNo(StockTransferHdrRefNo, InvoiceNo, userName, out status))
                    throw new Exception(status);

                transferHdrData = StockTransferController.FetchTransferHdr(StockTransferHdrRefNo, out status);
            }
            catch (Exception ex)
            {
                status = "Error Change Invoice No Status :" + ex.Message;
            }

            var result = new
            {
                status = status,
                hdrData = transferHdrData
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangeSTDetailFactoryPrice(string StockTransferDetRefNo, decimal price, string username)
        {
            string status = "";

            try
            {
                StockTransferDet poDet = new StockTransferDet(StockTransferDetRefNo);

                if (poDet == null || poDet.StockTransferDetRefNo != StockTransferDetRefNo)
                {
                    status = "Stock Transfer Detail not found";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                poDet.FactoryPrice = price;
                poDet.Save(username);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangeSTDetailFactoryPriceAll(string StockTransferHdrRefNo, string levelPrice, string username)
        {
            string status = "";

            try
            {

                QueryCommandCollection qcol = new QueryCommandCollection();

                StockTransferHdr poh = new StockTransferHdr(StockTransferHdrRefNo);
                if (!poh.IsNew)
                {
                    poh.PriceLevel = levelPrice;
                    qcol.Add(poh.GetUpdateCommand(username));
                }


                string query = string.Format("SELECT * FROM StockTransferDet WHERE StockTransferHdrRefNo = '{0}'", StockTransferHdrRefNo);
                QueryCommand qc = new QueryCommand(query);
                StockTransferDetCollection poDet = new StockTransferDetCollection();
                poDet.LoadAndCloseReader(DataService.GetReader(qc));


                if (poDet.Count() > 0)
                {
                    foreach (StockTransferDet pd in poDet)
                    {
                        switch (levelPrice.ToLower())
                        {
                            case "p1": pd.FactoryPrice = pd.Item.P1Price.GetValueOrDefault(0);
                                break;
                            case "p2": pd.FactoryPrice = pd.Item.P2Price.GetValueOrDefault(0);
                                break;
                            case "p3": pd.FactoryPrice = pd.Item.P3Price.GetValueOrDefault(0);
                                break;
                            case "p4": pd.FactoryPrice = pd.Item.P4Price.GetValueOrDefault(0);
                                break;
                            case "p5": pd.FactoryPrice = pd.Item.P5Price.GetValueOrDefault(0);
                                break;
                            default: pd.FactoryPrice = 0;
                                break;
                        }
                        qcol.Add(pd.GetUpdateCommand(username));
                    }

                    if (qcol.Count() > 0)
                        DataService.ExecuteTransaction(qcol);
                }


            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        #endregion

        #region *) Inventory Location Frozen

        [WebMethod]
        public string IsInventoryLocationFrozen(int InventoryLocationID)
        {
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            bool isFrozen = false;
            string status = "";

            try
            {
                isFrozen = StockTakeController.IsInventoryLocationFrozen(InventoryLocationID);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                isFrozen = isFrozen,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
             */
        }

        [WebMethod]
        public string UpdateInventoryLocationIsFrozen(int InventoryLocationID, bool IsFrozen, string username)
        {
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            string status = "";

            try
            {
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Clinic not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (IsFrozen == true)
                {
                    SqlQuery qry;

                    // Check for unfinished Goods Order
                    qry = new Select().From("PurchaseOrderHeader")
                          .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Order")
                          .And(PurchaseOrderHeader.UserColumns.Status).In("Pending", "Submitted", "Approved")
                          .And(PurchaseOrderHeader.Columns.InventoryLocationID).IsEqualTo(InventoryLocationID);
                    if (qry.GetRecordCount() > 0)
                    {
                        status = "There is pending Goods Order. Cannot freeze this Clinic: " + invLoc.InventoryLocationName;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    // Check for unfinished Stock Return
                    qry = new Select().From("PurchaseOrderHeader")
                          .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Return")
                          .And(PurchaseOrderHeader.UserColumns.Status).In("Pending", "Submitted")
                          .And(PurchaseOrderHeader.Columns.InventoryLocationID).IsEqualTo(InventoryLocationID);
                    if (qry.GetRecordCount() > 0)
                    {
                        status = "There is pending Stock Return. Cannot freeze this Clinic: " + invLoc.InventoryLocationName;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    // Check for unfinished Stock Adjustment
                    qry = new Select().From("PurchaseOrderHeader")
                          .Where(PurchaseOrderHeader.UserColumns.POType).Like("Adjustment%")
                          .And(PurchaseOrderHeader.UserColumns.Status).In("Pending", "Submitted")
                          .And(PurchaseOrderHeader.Columns.InventoryLocationID).IsEqualTo(InventoryLocationID);
                    if (qry.GetRecordCount() > 0)
                    {
                        status = "There is pending Stock Adjustment. Cannot freeze this Clinic: " + invLoc.InventoryLocationName;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    // Check for unfinished Stock Transfer
                    qry = new Select().From("PurchaseOrderHeader")
                          .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Transfer")
                          .And(PurchaseOrderHeader.UserColumns.Status).In("Pending", "Submitted")
                          .AndExpression(PurchaseOrderHeader.Columns.InventoryLocationID).IsEqualTo(InventoryLocationID).Or(PurchaseOrderHeader.UserColumns.DestInventoryLocationID).IsEqualTo(InventoryLocationID).CloseExpression();
                    if (qry.GetRecordCount() > 0)
                    {
                        status = "There is pending Stock Transfer. Cannot freeze this Clinic: " + invLoc.InventoryLocationName;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                }
                else
                {
                    if (StockTakeController.IsThereUnAdjustedStockTakeForInventoryLocation(InventoryLocationID))
                    {
                        status = "There is pending Stock Take. Cannot unfreeze this Clinic: " + invLoc.InventoryLocationName;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                }

                invLoc.IsFrozen = IsFrozen;
                invLoc.Save(username);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);*/
        }

        [WebMethod]
        public string UpdateInventoryLocationIsFrozen_All(bool IsFrozen, string username)
        {
            string status = "Method not yet implemented.";
            return new JavaScriptSerializer().Serialize(new { status = status });
            /*
            string status = "";
            List<string> resultCollection = new List<string>();

            try
            {
                List<InventoryLocation> invColl = new List<InventoryLocation>();
                invColl = new Select().From("InventoryLocation")
                          .Where(InventoryLocation.Columns.Deleted).IsEqualTo(false)
                          .And(InventoryLocation.UserColumns.IsFrozen).IsNotEqualTo(IsFrozen)
                          .OrderAsc(InventoryLocation.Columns.InventoryLocationName)
                          .ExecuteTypedList<InventoryLocation>();
                //InventoryLocationCollection invColl = new InventoryLocationCollection();
                //invColl.Where(i => i.Deleted == false && i.IsFrozen != IsFrozen);
                //invColl.OrderByAsc("InventoryLocationName");
                //invColl.Load();

                foreach (InventoryLocation loc in invColl)
                {
                    string res = UpdateInventoryLocationIsFrozen(loc.InventoryLocationID, IsFrozen, username);
                    resultCollection.Add(res);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = resultCollection,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
             * */
        }



        #endregion

        #region *) AppSetting

        public string GetSetting(string AppSettingKey)
        {
            return AppSetting.GetSetting(AppSettingKey);
        }

        [WebMethod]
        public string GetSetting(string AppSettingKey, string boolValue)
        {
            if (boolValue.ToLower() == "yes")
            {
                return AppSetting.CastBool(AppSetting.GetSetting(AppSettingKey), false).ToString();
            }
            else
            {
                return AppSetting.GetSetting(AppSettingKey);
            }
        }

        [WebMethod]
        public string GetSettings(string data)
        {
            List<String> keys = new JavaScriptSerializer().Deserialize<List<String>>(data);
            List<String> values = new List<string>();

            foreach (var key in keys)
                values.Add(AppSetting.GetSetting(key));


            return new JavaScriptSerializer().Serialize(values);
        }

        #endregion

        #region *) Getters

        [WebMethod]
        public string SearchItem(string name, bool IsInventoryItemOnly, bool ShowSystemItem, bool ShowDeletedItem, int skip, int take)
        {
            ItemController itemCtrl = new ItemController();
            ViewItemCollection items = itemCtrl.SearchItem("%" + name + "%", IsInventoryItemOnly, ShowSystemItem, ShowDeletedItem);
            var tmpResult = (from it in items select it);

            if (skip > 0) tmpResult = tmpResult.Skip(skip);
            if (take > 0) tmpResult = tmpResult.Take(take);

            var result = new
            {
                records = tmpResult,
                totalRecords = items.Count
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SearchItemUserPortal(string name, string username, bool isSupplier, bool isRestrictedSupplier, int supplierID, bool isShowSalesQty,
            int numOfDays, int InventoryLocationID, bool IsInventoryItemOnly, bool ShowSystemItem, bool ShowDeletedItem, int skip, int take)
        {
            bool isUseUserPortal = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseSupplierPortal), false);

            DataTable tmpResult = new DataTable();

            tmpResult.Columns.Add("CategoryName", typeof(string));
            tmpResult.Columns.Add("Category_ID", typeof(string));
            tmpResult.Columns.Add("ItemDepartmentId", typeof(string));
            tmpResult.Columns.Add("DepartmentName", typeof(string));
            tmpResult.Columns.Add("IsGST", typeof(bool));
            tmpResult.Columns.Add("ItemNo", typeof(string));
            tmpResult.Columns.Add("ItemName", typeof(string));
            tmpResult.Columns.Add("Barcode", typeof(string));
            tmpResult.Columns.Add("RetailPrice", typeof(decimal));
            tmpResult.Columns.Add("FactoryPrice", typeof(decimal));
            tmpResult.Columns.Add("MinimumPrice", typeof(decimal));
            tmpResult.Columns.Add("ItemDesc", typeof(string));
            tmpResult.Columns.Add("IsInInventory", typeof(bool));
            tmpResult.Columns.Add("IsNonDiscountable", typeof(bool));
            tmpResult.Columns.Add("Deleted", typeof(bool));
            tmpResult.Columns.Add("Attributes1", typeof(string));
            tmpResult.Columns.Add("Attributes2", typeof(string));
            tmpResult.Columns.Add("Attributes3", typeof(string));
            tmpResult.Columns.Add("Attributes4", typeof(string));
            tmpResult.Columns.Add("Attributes5", typeof(string));
            tmpResult.Columns.Add("Attributes6", typeof(string));
            tmpResult.Columns.Add("Attributes7", typeof(string));
            tmpResult.Columns.Add("Attributes8", typeof(string));
            tmpResult.Columns.Add("Userflag1", typeof(bool));
            tmpResult.Columns.Add("OnHand", typeof(decimal));
            tmpResult.Columns.Add("SalesPeriod1", typeof(decimal));
            tmpResult.Columns.Add("SalesPeriod2", typeof(decimal));
            tmpResult.Columns.Add("SalesPeriod3", typeof(decimal));

            ItemController itemCtrl = new ItemController();
            DataTable items = itemCtrl.SearchItemUserPortal("%" + name + "%", isSupplier, isRestrictedSupplier,
                        supplierID, username, InventoryLocationID, IsInventoryItemOnly, ShowSystemItem, ShowDeletedItem, "");

            string status = "";
            for (int i = skip; i < skip + take && i < items.Rows.Count; i++)
            {
                DataRow dR = tmpResult.NewRow();

                string itemNo = items.Rows[i]["ItemNo"].ToString();
                dR["CategoryName"] = items.Rows[i]["CategoryName"];
                dR["Category_ID"] = items.Rows[i]["Category_ID"];
                dR["ItemDepartmentId"] = items.Rows[i]["ItemDepartmentId"];
                dR["DepartmentName"] = items.Rows[i]["DepartmentName"];
                dR["IsGST"] = items.Rows[i]["IsGST"];
                dR["ItemNo"] = items.Rows[i]["ItemNo"];
                dR["ItemName"] = items.Rows[i]["ItemName"];
                dR["Barcode"] = items.Rows[i]["Barcode"];
                dR["RetailPrice"] = items.Rows[i]["RetailPrice"];
                dR["FactoryPrice"] = items.Rows[i]["FactoryPrice"];
                dR["MinimumPrice"] = items.Rows[i]["MinimumPrice"];
                dR["ItemDesc"] = items.Rows[i]["ItemDesc"];
                dR["IsInInventory"] = items.Rows[i]["IsInInventory"];
                dR["IsNonDiscountable"] = items.Rows[i]["IsNonDiscountable"];
                dR["Deleted"] = items.Rows[i]["Deleted"];
                dR["Attributes1"] = items.Rows[i]["Attributes1"];
                dR["Attributes2"] = items.Rows[i]["Attributes2"];
                dR["Attributes3"] = items.Rows[i]["Attributes3"];
                dR["Attributes4"] = items.Rows[i]["Attributes4"];
                dR["Attributes5"] = items.Rows[i]["Attributes5"];
                dR["Attributes6"] = items.Rows[i]["Attributes6"];
                dR["Attributes7"] = items.Rows[i]["Attributes7"];
                dR["Attributes8"] = items.Rows[i]["Attributes8"];
                dR["Userflag1"] = items.Rows[i]["Userflag1"];
                dR["OnHand"] = items.Rows[i]["OnHand"];

                decimal salesPeriod1 = 0;
                decimal salesPeriod2 = 0;
                decimal salesPeriod3 = 0;
                if (isShowSalesQty)
                {
                    salesPeriod1 = InventoryController.InventoryFetchItemSales(itemNo, supplierID, "1",
                            numOfDays.ToString(), InventoryLocationID, username, out status);
                    salesPeriod2 = InventoryController.InventoryFetchItemSales(itemNo, supplierID, "2",
                        numOfDays.ToString(), InventoryLocationID, username, out status);
                    salesPeriod3 = InventoryController.InventoryFetchItemSales(itemNo, supplierID, "3",
                        numOfDays.ToString(), InventoryLocationID, username, out status);
                }

                dR["SalesPeriod1"] = salesPeriod1;
                dR["SalesPeriod2"] = salesPeriod2;
                dR["SalesPeriod3"] = salesPeriod3;

                tmpResult.Rows.Add(dR);
            }

            var result = new
             {
                 records = tmpResult,
                 totalRecords = items.Rows.Count
             };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            //}
        }

        [WebMethod]
        public string GetPointOfSales()
        {
            string status = "";
            PointOfSaleCollection pos = new PointOfSaleCollection();

            try
            {
                pos.Where(p => p.Deleted == false);
                pos.OrderByAsc("PointOfSaleName");
                pos.Load();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = pos,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetCategoryList(string filter)
        {
            string status = "";
            List<Category> categoryList = new List<Category>();
            int totalRecords = 0;

            try
            {
                var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);

                CategoryCollection posColl = new CategoryCollection();
                posColl.Where("Deleted", false);

                if (param.ContainsKey("categoryName") && param["categoryName"] != "ALL")
                    posColl.Where("categoryName", Comparison.Like, param["categoryName"] + "%");

                posColl.OrderByAsc(Category.Columns.CategoryName);
                posColl.Load();
                foreach (Category pos in posColl)
                {
                    categoryList.Add(pos);
                }

                totalRecords = categoryList.Count;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = categoryList,
                totalRecords = totalRecords,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetItem(string ItemNo)
        {
            string status = "";

            Item item = new Item(ItemNo);
            if (item == null || item.ItemNo != ItemNo || item.Deleted.GetValueOrDefault(false) == true)
            {
                status = "Item not found.";
            }

            var result = new
            {
                Item = item,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetItemNo(string barcode)
        {
            string status = "";
            ItemController itemLogic = new ItemController();
            bool IsItemNo;

            ViewItem theItem = itemLogic.FetchByBarcode(barcode, out IsItemNo);
            if (theItem != null && theItem.IsLoaded && !theItem.IsNew && !theItem.Deleted.GetValueOrDefault(false))
            {
                // Item found
                var result = new
                {
                    ItemNo = theItem.ItemNo,
                    status = status
                };

                return new JavaScriptSerializer().Serialize(result);
            }
            else
            {
                status = "Item not found.";
                var result = new
                {
                    ItemNo = "",
                    status = status
                };

                return new JavaScriptSerializer().Serialize(result);
            }
        }

        [WebMethod]
        public string GetInventoryLocations()
        {
            string status = "";
            InventoryLocationCollection inv = new InventoryLocationCollection();

            try
            {
                inv.Where(i => i.Deleted == false);
                inv.OrderByAsc("InventoryLocationName");
                inv.Load();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = inv,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetOutlets(String username)
        {
            UserMst user = new UserMst(UserMst.Columns.UserName, username);

            List<Outlet> outlets = new List<Outlet>();
            {
                if (!string.IsNullOrEmpty(user.Userfld1))
                {
                    var tmp = user.Userfld1.Split(new char[] { ',' });
                    foreach (var t in tmp)
                    {
                        Outlet outlet = new Outlet(t);
                        if (!outlet.IsNew) outlets.Add(outlet);
                    }


                    if (tmp.Count() == 1 && tmp.FirstOrDefault().ToLower().Equals("all"))
                    {
                        OutletCollection oc = new OutletCollection();
                        oc.Load();
                        var lst = oc.ToList();
                        foreach (var l in lst)
                        {
                            outlets.Add(l);
                        }
                    }
                }
            }

            return new JavaScriptSerializer().Serialize(outlets);
        }

        [WebMethod]
        public string GetOutletsAll()
        {
            string status = "";
            List<string> outlets = new List<string>();
            try
            {
                OutletCollection col = new OutletCollection();
                col.Where(Outlet.Columns.Deleted, false);
                col.OrderByAsc(Outlet.Columns.OutletName);
                col.Load();

                foreach (var outl in col)
                {
                    outlets.Add(outl.OutletName);
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }


            var result = new
            {
                records = outlets,
                status = status
            };
            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetStockOutReasons()
        {
            string status = "";
            InventoryStockOutReasonCollection cols = new InventoryStockOutReasonCollection();

            try
            {
                cols.Where(i => i.Deleted == false);
                cols.Load();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = cols,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SearchUser(string name, bool ShowDeletedUser, int skip, int take)
        {
            ItemController itemCtrl = new ItemController();
            ViewUserCollection items = UserController.FetchUsers(name, ShowDeletedUser);
            var tmpResult = (from it in items select it);

            if (skip > 0) tmpResult = tmpResult.Skip(skip);
            if (take > 0) tmpResult = tmpResult.Take(take);

            var result = new
            {
                records = tmpResult,
                totalRecords = items.Count
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        #endregion

        #region *) Misc
        public DateTime AddWorkingDays(DateTime date, int days)
        {
            for (int i = 1; i <= days; i++)
            {
                date = date.AddDays(1);
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    i--;
            }
            return date;
        }

        /*
        [WebMethod(CacheDuration = 0)]
        public DataSet GetDataTable(string tableName, bool syncAll)
        {
            return SynchronizationController.GetDataTable(tableName, syncAll);
        }

        [WebMethod]
        public string GetNotifications(string username)
        {
            string status = "";
            List<object> notifications = new List<object>();

            try
            {
                bool allowChangeLocation = false;
                int invLocID = 0;

                UserMst user = new UserMst(username);
                if (user == null || user.GroupName == 0 || user.UserGroup == null || string.IsNullOrEmpty(user.UserGroup.GroupName))
                {
                    status = "User or role not found.";
                }

                PointOfSale pos = new PointOfSale(user.PointOfSaleID);
                if (pos != null && pos.Outlet != null && pos.Outlet.InventoryLocation != null)
                {
                    invLocID = pos.Outlet.InventoryLocation.InventoryLocationID;
                }
                else
                {
                    status = "Invalid Point Of Sale ID.";
                }

                if (status == "")
                {
                    DataTable dtPrivileges = UserController.FetchGroupPrivileges(user.UserGroup.GroupName);
                    List<string> privileges = new List<string>();
                    foreach (DataRow dr in dtPrivileges.Rows)
                    {
                        privileges.Add(dr["PrivilegeName"].ToString().Trim());
                    }

                    if (privileges.Exists(p => p == "Allow Change Inventory Location"))
                        allowChangeLocation = true;

                    if (privileges.Exists(p => p == "Goods Ordering"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Order")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Pending");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " pending item(s) in Goods Order.",
                                menu = "goodsordering"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Order Approval"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Order")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Submitted");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " submitted item(s) in Order Approval.",
                                menu = "orderapproval"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Goods Receive"))
                    {
                        // For Goods Order
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Order")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Approved");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " approved order(s) waiting for goods receive.",
                                menu = "goodsreceive"
                            };
                            notifications.Add(notifObj);
                        }

                        // For Stock Transfer
                        qry = new Select().From("PurchaseOrderHeader")
                                  .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Transfer")
                                  .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Submitted");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " stock transfer(s) waiting for goods receive.",
                                menu = "goodsreceive"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Stock Return"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Return")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Pending");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " pending item(s) in Stock Return.",
                                menu = "stockreturn"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Return Approval"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Return")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Submitted");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " submitted item(s) in Return Approval.",
                                menu = "returnapproval"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Stock Adjustment"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).Like("Adjustment%")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Pending");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " pending item(s) in Stock Adjustment.",
                                menu = "stockadjustment"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Adjustment Approval"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).Like("Adjustment%")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Submitted");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " submitted item(s) in Adjustment Approval.",
                                menu = "adjustmentapproval"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Stock Transfer"))
                    {
                        SqlQuery qry = new Select().From("PurchaseOrderHeader")
                                       .Where(PurchaseOrderHeader.UserColumns.POType).IsEqualTo("Transfer")
                                       .And(PurchaseOrderHeader.UserColumns.Status).IsEqualTo("Pending");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " pending item(s) in Stock Transfer.",
                                menu = "stocktransfer"
                            };
                            notifications.Add(notifObj);
                        }
                    }

                    if (privileges.Exists(p => p == "Stock Take Approval"))
                    {
                        SqlQuery qry = new Select().From("StockTakeDoc")
                                       .Where("Status").IsEqualTo("Pending");

                        if (!allowChangeLocation) qry.And("InventoryLocationID").IsEqualTo(invLocID);

                        int count = qry.GetRecordCount();
                        if (count > 0)
                        {
                            var notifObj = new
                            {
                                message = count.ToString() + " stock take document(s) waiting for approval.",
                                menu = "stocktakeapproval"
                            };
                            notifications.Add(notifObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                notifications = notifications,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }
        */
        #endregion

        #region *) Email Notification

        [WebMethod]
        public string GetEmailNotificationList(string filter, int skip, int take, string sortBy, bool isAscending)
        {
            var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);

            EmailNotificationCollection notifColl = new EmailNotificationCollection();

            SqlQuery qry = new Select().From("EmailNotification");
            qry.Where("EmailAddress").IsNotNull(); // Just a dummy WHERE to make sure there's already a WHERE clause

            if (param.ContainsKey("emailaddress"))
                qry.And("EmailAddress").Like("%" + param["emailaddress"] + "%");

            if (param.ContainsKey("name"))
                qry.And("Name").Like("%" + param["name"] + "%");

            if (param.ContainsKey("modulename"))
                qry.And("Module").Like("%" + param["modulename"] + "%");

            if (param.ContainsKey("active"))
                qry.And("Deleted").IsNotEqualTo(Convert.ToBoolean(param["active"]));

            if (sortBy == "ModuleX") sortBy = "Module";

            if (isAscending)
                qry.OrderAsc(sortBy);
            else
                qry.OrderDesc(sortBy);

            // Get total records before being paged
            var totalRecords = qry.GetRecordCount();

            if (take > 0)
                qry.Paged((skip / take) + 1, take);

            notifColl = qry.ExecuteAsCollection<EmailNotificationCollection>();

            var result = new
            {
                records = notifColl,
                totalRecords = totalRecords
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetEmailNotification(string EmailAddress)
        {
            EmailNotification notif = new EmailNotification();
            string status = "";

            try
            {
                notif = new EmailNotification(EmailAddress);
                if (!notif.IsLoaded) status = "Not found";
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                EmailNotification = notif,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SaveEmailNotification(string data, string username)
        {
            var status = "";
            EmailNotification dataNotif;
            EmailNotification tmpNotif = new EmailNotification();

            try
            {
                dataNotif = new JavaScriptSerializer().Deserialize<EmailNotification>(data);

                if (string.IsNullOrEmpty(dataNotif.EmailAddress) || dataNotif.EmailAddress.Trim() == "")
                {
                    status = "Email Address cannot be empty.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (string.IsNullOrEmpty(dataNotif.Name) || dataNotif.Name.Trim() == "")
                {
                    status = "Name cannot be empty.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                tmpNotif = new EmailNotification(dataNotif.EmailAddress);

                if (dataNotif.IsNew && tmpNotif.EmailAddress.ToLower().Trim() == dataNotif.EmailAddress.ToLower().Trim())
                {
                    // If it should be a new record, but duplicate EmailAddress is found, throw error
                    status = "Duplicate email address found in database.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                foreach (TableSchema.TableColumn col in tmpNotif.GetSchema().Columns)
                {
                    if (col.ColumnName != "ModifiedOn" && col.ColumnName != "ModifiedBy")
                    {
                        tmpNotif.SetColumnValue(col.ColumnName, dataNotif.GetColumnValue(col.ColumnName));
                    }
                }

                tmpNotif.Save(username);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                EmailNotification = tmpNotif,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string DeleteEmailNotification(string EmailAddress)
        {
            EmailNotificationController ctrl = new EmailNotificationController();
            string status = "";

            try
            {
                ctrl.Destroy(EmailAddress);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        #endregion

        #region *) Auto Print
        /*
        [WebMethod]
        public int GetNewOrderCountAfter(string timeStamp)
        {
            int totalRecords = 0;

            try
            {
                Query qry = null;
                string[] poType = new string[] { "Replenish", "Special Order", "Back Order", "Pre Order", "Return" };
                bool autoApprove = AppSetting.CastBool(AppSetting.GetSetting("AutoApproveOrder"), false);
                if (autoApprove)
                {
                    qry = new Query("PurchaseOrderHeader")
                            .AddWhere(PurchaseOrderHeader.UserColumns.ApprovalDate, Comparison.GreaterThan, timeStamp)
                            .AddWhere(PurchaseOrderHeader.UserColumns.POType, Comparison.In, poType)
                            .AddWhere(PurchaseOrderHeader.UserColumns.Status, Comparison.In, new string[] { "Approved", "Posted", "Rejected" })
                            .ORDER_BY(PurchaseOrderHeader.UserColumns.ApprovalDate, "ASC")
                            .ORDER_BY(PurchaseOrderHeader.Columns.PurchaseOrderHeaderRefNo, "ASC");
                }
                else
                {
                    qry = new Query("PurchaseOrderHeader")
                           .AddWhere(PurchaseOrderHeader.Columns.PurchaseOrderDate, Comparison.GreaterThan, timeStamp)
                           .AddWhere(PurchaseOrderHeader.UserColumns.POType, Comparison.In, poType)
                           .AddWhere(PurchaseOrderHeader.UserColumns.Status, "Submitted")
                           .ORDER_BY(PurchaseOrderHeader.Columns.PurchaseOrderDate, "ASC")
                           .ORDER_BY(PurchaseOrderHeader.Columns.PurchaseOrderHeaderRefNo, "ASC");
                }
                totalRecords = qry.GetRecordCount();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }

            return totalRecords;
        }

        [WebMethod]
        public List<AutoPrintData> GetNewOrderAfter(string timeStamp, int maxOrder)
        {
            // For auto print feature of newly submitted orders.

            DataSet ds = new DataSet();
            List<AutoPrintData> printData = new List<AutoPrintData>();

            try
            {
                Query qry = null;
                string[] poType = new string[] { "Replenish", "Special Order", "Back Order", "Pre Order", "Return" };
                bool autoApprove = AppSetting.CastBool(AppSetting.GetSetting("AutoApproveOrder"), false);
                if (autoApprove)
                {
                    qry = new Query("PurchaseOrderHeader")
                            .AddWhere(PurchaseOrderHeader.UserColumns.ApprovalDate, Comparison.GreaterThan, timeStamp)
                            .AddWhere(PurchaseOrderHeader.UserColumns.POType, Comparison.In, poType)
                            .AddWhere(PurchaseOrderHeader.UserColumns.Status, Comparison.In, new string[] { "Approved", "Posted", "Rejected" })
                            .ORDER_BY(PurchaseOrderHeader.UserColumns.ApprovalDate, "ASC")
                            .ORDER_BY(PurchaseOrderHeader.Columns.PurchaseOrderHeaderRefNo, "ASC");
                }
                else
                {
                    qry = new Query("PurchaseOrderHeader")
                            .AddWhere(PurchaseOrderHeader.Columns.PurchaseOrderDate, Comparison.GreaterThan, timeStamp)
                            .AddWhere(PurchaseOrderHeader.UserColumns.POType, Comparison.In, poType)
                            .AddWhere(PurchaseOrderHeader.UserColumns.Status, "Submitted")
                            .ORDER_BY(PurchaseOrderHeader.Columns.PurchaseOrderDate, "ASC")
                            .ORDER_BY(PurchaseOrderHeader.Columns.PurchaseOrderHeaderRefNo, "ASC");
                }

                PurchaseOrderHeaderCollection poHdrColl = new PurchaseOrderHeaderCollection();
                poHdrColl.LoadAndCloseReader(qry.ExecuteReader());

                for (int i = 0; i < maxOrder; i++)
                {
                    if (i + 1 > poHdrColl.Count) break;

                    string sql;
                    QueryCommand cmd;
                    DataTable dt;
                    DateTime orderTimeStamp;

                    ds = new DataSet();

                    PowerReport.CRReport cr;
                    if (autoApprove)
                    {
                        cr = new PowerReport.CRReport(Server.MapPath("~/Bin/Reports/OrderPrintout.rpt"));
                        orderTimeStamp = poHdrColl[i].ApprovalDate;
                    }
                    else
                    {
                        cr = new PowerReport.CRReport(Server.MapPath("~/Bin/Reports/SubmittedOrderPrintout.rpt"));
                        orderTimeStamp = poHdrColl[i].PurchaseOrderDate;
                    }

                    #region *) Get Main Report's data
                    sql = cr.SQLString;
                    sql = sql.Replace("{?DocNo}", "'" + poHdrColl[i].PurchaseOrderHeaderRefNo + "'");
                    cmd = new QueryCommand(sql, "PowerPOS");
                    dt = DataService.GetDataSet(cmd).Tables[0];
                    dt.TableName = "Main Report";
                    ds.Tables.Add(dt.Copy());
                    #endregion

                    #region *) Get Subreport's data if any
                    ReportDocument rpt = cr.GetReportWithoutData();
                    for (int j = 0; j < rpt.Subreports.Count; j++)
                    {
                        cr = new PowerReport.CRReport(rpt.Subreports[j]);
                        sql = cr.SQLString;
                        sql = sql.Replace("{?DocNo}", "'" + poHdrColl[i].PurchaseOrderHeaderRefNo + "'");
                        cmd = new QueryCommand(sql, "PowerPOS");
                        dt = DataService.GetDataSet(cmd).Tables[0];
                        dt.TableName = rpt.Subreports[j].Name;
                        ds.Tables.Add(dt.Copy());
                    }
                    #endregion

                    printData.Add(new AutoPrintData() { TimeStamp = orderTimeStamp, DataSet = ds });
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }

            return printData;
        }
        */
        #endregion

        #region *) Report
        /*
        [WebMethod]
        public string GetParamsForReport(string queryName)
        {
            string status = "";
            QueryParamCollection queryParams = new QueryParamCollection();

            try
            {
                queryParams.Where("QueryName", queryName);
                queryParams.Load();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                queryParams = queryParams,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetReportDataByQueryName(string queryName)
        {
            string status = "";
            List<string> columns = new List<string>();
            List<ArrayList> rows = new List<ArrayList>();

            try
            {
                string querySql = new QueryForReport(queryName).QuerySql;
                if (string.IsNullOrEmpty(querySql))
                {
                    status = "Query is empty or not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                QueryParamCollection queryParams = new QueryParamCollection();
                queryParams.Where("QueryName", queryName);
                queryParams.Load();

                foreach (var param in queryParams)
                {
                    string tes = param.ParamName;
                }

                DataTable dt = new DataTable();
                QueryCommand cmd = new QueryCommand(querySql, "PowerPOS");

                dt = DataService.GetDataSet(cmd).Tables[0];

                // Extract the column name
                foreach (DataColumn dc in dt.Columns)
                {
                    columns.Add(dc.ColumnName);
                }

                // Extract the row
                foreach (DataRow dr in dt.Rows)
                {
                    ArrayList tmpRow = new ArrayList();
                    foreach (string colName in columns)
                    {
                        object value = (dr[colName].GetType() == typeof(DateTime)) ? ((DateTime)dr[colName]).ToString("yyyy-MM-dd hh:mm:ss.fff") : dr[colName];
                        tmpRow.Add(value);
                    }
                    rows.Add(tmpRow);
                }

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                columns = columns,
                rows = rows
            };

            return new JavaScriptSerializer().Serialize(result);
        }
        */
        #endregion

        #region *) Guest Book

        [WebMethod]
        public string GetGuestBookName()
        {
            string GuestBookName = "";
            string status = "";

            try
            {
                GuestBookName = AppSetting.GetSetting(AppSetting.SettingsName.GuestBook.GuestBookName);
            }
            catch (Exception ex)
            {
                Logger.writeLog("error get guest book name" + ex.Message);
                status = ex.Message;
            }

            var result = new
            {
                data = string.IsNullOrEmpty(GuestBookName) ? "Guest Book" : GuestBookName,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SaveGuestBookName(string newGuestBookName)
        {

            string status = "";

            try
            {
                AppSetting.SetSetting(AppSetting.SettingsName.GuestBook.GuestBookName, newGuestBookName);
            }
            catch (Exception ex)
            {
                Logger.writeLog("error save guest book name" + ex.Message);
                status = ex.Message;
            }

            var result = new
            {
                data = newGuestBookName,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string CheckInSubmit(string mobileNo)
        {
            string status = "";

            try
            {
                MembershipCollection col = new MembershipCollection();

                col.Where(Membership.Columns.Mobile, mobileNo);
                col.Where(Membership.Columns.Deleted, false);
                col.Load();

                if (col.Count() == 0)
                    throw new Exception("Your mobile no. is not registered.<br/> Please sign up first.");

                string query = "Select * from GuestBook where MembershipNo = '" + col[0].MembershipNo + "' and (InTime is not null and OutTime is null) and ISNULL(deleted, 0) = 0";
                DataSet ds = DataService.GetDataSet(new QueryCommand(query));

                if (ds.Tables[0].Rows.Count > 0)
                    throw new Exception("You already checked in");

                GuestBook gb = new GuestBook();
                gb.MembershipNo = col[0].MembershipNo;
                gb.InTime = DateTime.Now;
                gb.Deleted = false;
                gb.Save("APP_GUESTBOOK");
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog("Error check in :" + ex.Message);
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string CheckInSubmitWithOutlet(string mobileNo, string outletName)
        {
            string status = "";

            try
            {
                MembershipCollection col = new MembershipCollection();

                col.Where(Membership.Columns.Mobile, mobileNo);
                col.Where(Membership.Columns.Deleted, false);
                col.Load();

                if (col.Count() == 0)
                    throw new Exception("Your mobile no. is not registered.<br/> Please sign up first.");

                string query = "Select * from GuestBook where MembershipNo = '" + col[0].MembershipNo + "' and (InTime is not null and OutTime is null) and ISNULL(deleted, 0) = 0";
                DataSet ds = DataService.GetDataSet(new QueryCommand(query));

                if (ds.Tables[0].Rows.Count > 0)
                    throw new Exception("You already checked in");

                GuestBook gb = new GuestBook();
                gb.MembershipNo = col[0].MembershipNo;
                gb.InTime = DateTime.Now;
                gb.OutletName = outletName;
                gb.Deleted = false;
                gb.Save("APP_GUESTBOOK");
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog("Error check in :" + ex.Message);
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string CheckOutSubmit(string mobileNo)
        {
            string status = "";

            try
            {
                MembershipCollection col = new MembershipCollection();

                col.Where(Membership.Columns.Mobile, mobileNo);
                col.Where(Membership.Columns.Deleted, false);
                col.Load();

                if (col.Count() == 0)
                    throw new Exception("Your mobile no. is not registered.<br/> Please sign up first.");

                string query = "Select * from GuestBook where MembershipNo = '" + col[0].MembershipNo + "' and (InTime is not null and OutTime is null) and ISNULL(deleted, 0) = 0";
                DataSet ds = DataService.GetDataSet(new QueryCommand(query));

                if (ds.Tables[0].Rows.Count == 0)
                    throw new Exception("You are not checked in yet");

                GuestBookCollection gbCol = new GuestBookCollection();
                gbCol.Load(ds.Tables[0]);

                gbCol[0].OutTime = DateTime.Now;
                gbCol[0].Save("APP_GUESTBOOK");
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog("Error check out :" + ex.Message);
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string CheckOutSubmitWithOutlet(string mobileNo, string outletName)
        {
            string status = "";

            try
            {
                MembershipCollection col = new MembershipCollection();

                col.Where(Membership.Columns.Mobile, mobileNo);
                col.Where(Membership.Columns.Deleted, false);
                col.Load();

                if (col.Count() == 0)
                    throw new Exception("Your mobile no. is not registered.<br/> Please sign up first.");

                string query = "Select * from GuestBook where MembershipNo = '" + col[0].MembershipNo + "' and (InTime is not null and OutTime is null) and ISNULL(deleted, 0) = 0 and OutletName = '" + outletName + "'";
                DataSet ds = DataService.GetDataSet(new QueryCommand(query));

                if (ds.Tables[0].Rows.Count == 0)
                    throw new Exception("You are not checked in yet");

                GuestBookCollection gbCol = new GuestBookCollection();
                gbCol.Load(ds.Tables[0]);

                gbCol[0].OutTime = DateTime.Now;
                gbCol[0].Save("APP_GUESTBOOK");
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog("Error check out :" + ex.Message);
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }


        [WebMethod]
        public string CheckMemberExist(string nric)
        {
            string status = "";
            Membership member = null;
            try
            {
                MembershipCollection col = new MembershipCollection();

                col.Where(Membership.Columns.Nric, nric);
                col.Where(Membership.Columns.Deleted, false);
                col.Load();

                if (col.Count() == 0)
                    throw new Exception("NRIC not found");

                member = col[0];
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog("Error check in :" + ex.Message);
            }

            var result = new
            {
                data = member == null ? "" : member.MembershipNo,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ResetPassCode(string membershipNo, string passCode)
        {
            string status = "";

            try
            {
                Membership col = new Membership(membershipNo);

                col.PassCode = passCode;
                col.Save();
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog("Error save pass code :" + ex.Message);
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SaveNewMembership(string data)
        {
            string status = "";

            SettingCollection st = new SettingCollection();
            st.Load();
            int pos = 0;
            if (st != null)
                pos = st[0].PointOfSaleID;

            PointOfSale myPointOfSale = new PointOfSale(pos);

            MembershipObj tmpDetails = new JavaScriptSerializer().Deserialize<MembershipObj>(data);

            string[] partsOfTheName = tmpDetails.GivenName.Split(' ');
            string firstName = "";
            string lastName = "";
            int i = 0;

            foreach (string name in partsOfTheName)
            {
                if (i == 0)
                {
                    firstName = name;
                }
                else
                {
                    lastName += name + " ";
                }

                i++;
            }
            lastName = lastName.Trim();

            try
            {
                string strSql = "select top 1 a.MembershipGroupId from MembershipGroup a where a.GroupName = 'Normal'";
                QueryCommand cmd = new QueryCommand(strSql, "PowerPOS");
                int defaultMembershipId = (int)DataService.ExecuteScalar(cmd);

                string knowFrom = "";
                if (tmpDetails.chkFriends != null && tmpDetails.chkFriends.ToLower().Equals("true"))
                    knowFrom += ",Friends";
                if (tmpDetails.chkMagazines != null && tmpDetails.chkMagazines.ToLower().Equals("true"))
                    knowFrom += ",Magazines";
                if (tmpDetails.chkOnlineSearch != null && tmpDetails.chkOnlineSearch.ToLower().Equals("true"))
                    knowFrom += ",Online Search";
                if (tmpDetails.chkOnlineMedia != null && tmpDetails.chkOnlineMedia.ToLower().Equals("true"))
                    knowFrom += ",Online Media";
                if (tmpDetails.chkOther != null && tmpDetails.chkOther.ToLower().Equals("true"))
                    knowFrom += ",Other";
                if (knowFrom.Length > 0)
                    knowFrom = knowFrom.Remove(0, 1);

                /*check duplicate membership*/
                string query = "Select * from Membership where (NRIC = '" + tmpDetails.NRIC.Trim() + "' or Mobile = '" + tmpDetails.ContactNo + "') and ISNULL(Deleted,0) = 0 ";
                DataSet ds = DataService.GetDataSet(new QueryCommand(query));
                MembershipCollection col = new MembershipCollection();
                col.Load(ds.Tables[0]);

                if (col.Count > 0)
                    throw new Exception(string.Format("Member with NRIC equal {0} or Contact No equal {1} already registered. Cannot registered duplicate member", tmpDetails.NRIC.Trim(), tmpDetails.ContactNo));

                PowerPOS.Membership member = new PowerPOS.Membership();
                member.MembershipNo = MembershipController.getNewMembershipNo(myPointOfSale.MembershipCode);
                member.NameToAppear = tmpDetails.GivenName.Trim() + " " + tmpDetails.Surname.Trim();
                member.FirstName = firstName;
                member.LastName = tmpDetails.Surname;
                member.Email = tmpDetails.Email;
                member.Nric = tmpDetails.NRIC.ToUpper();
                member.StreetName = tmpDetails.HomeAddress;
                member.Mobile = tmpDetails.ContactNo;
                member.Child1Surname = "";
                member.Child1GivenName = tmpDetails.Child1GivenName;
                member.Child1DateOfBirth = tmpDetails.Child1DateBirth;
                member.Child2Surname = "";
                member.Child2GivenName = tmpDetails.Child2GivenName;
                member.Child2DateOfBirth = tmpDetails.Child2DateBirth;
                member.KnowFrom = knowFrom;
                member.PassCode = tmpDetails.PassCode;
                member.Nationality = tmpDetails.Nationality;
                member.Remarks = tmpDetails.Remark;
                //member.ExpiryDate = DateTime.ParseExact(expiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (tmpDetails.IsLifeTimeMember != null && tmpDetails.IsLifeTimeMember.ToLower().Equals("true"))
                {
                    member.ExpiryDate = new DateTime(2500, 1, 1);
                }
                else
                {
                    member.ExpiryDate = DateTime.Now.AddYears(1);
                }
                member.UniqueID = Guid.NewGuid();
                member.MembershipGroupId = defaultMembershipId;
                member.Deleted = false;
                member.Userflag5 = true;
                member.Save("edgeworks");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Error save member :" + ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SaveNewMembershipWithOutlet(string data, string outletName)
        {
            string status = "";
            PointOfSale myPointOfSale;
            string membershipcode = "";
            bool useDefaultMembershipCode = false;


            OutletCollection pol = new OutletCollection();
            pol.Where(Outlet.Columns.Deleted, false);
            pol.Where(Outlet.Columns.OutletName, outletName);
            pol.Load();

            if (pol.Count > 0)
            {
                membershipcode = pol[0].PrefixMembership;

                if (string.IsNullOrEmpty(membershipcode))
                {
                    useDefaultMembershipCode = true;
                }
            }
            else
            {
                useDefaultMembershipCode = true;
            }

            if (useDefaultMembershipCode)
            {
                SettingCollection st = new SettingCollection();
                st.Load();
                int pos = 0;
                if (st != null)
                    pos = st[0].PointOfSaleID;

                myPointOfSale = new PointOfSale(pos);
                membershipcode = myPointOfSale.MembershipCode;
            }

            MembershipObj tmpDetails = new JavaScriptSerializer().Deserialize<MembershipObj>(data);

            string[] partsOfTheName = tmpDetails.GivenName.Split(' ');
            string firstName = "";
            string lastName = "";
            int i = 0;

            foreach (string name in partsOfTheName)
            {
                if (i == 0)
                {
                    firstName = name;
                }
                else
                {
                    lastName += name + " ";
                }

                i++;
            }
            lastName = lastName.Trim();

            try
            {
                string strSql = "select top 1 a.MembershipGroupId from MembershipGroup a where a.GroupName = 'Normal'";
                QueryCommand cmd = new QueryCommand(strSql, "PowerPOS");
                int defaultMembershipId = (int)DataService.ExecuteScalar(cmd);

                string knowFrom = "";
                if (tmpDetails.chkFriends != null && tmpDetails.chkFriends.ToLower().Equals("true"))
                    knowFrom += ",Friends";
                if (tmpDetails.chkMagazines != null && tmpDetails.chkMagazines.ToLower().Equals("true"))
                    knowFrom += ",Magazines";
                if (tmpDetails.chkOnlineSearch != null && tmpDetails.chkOnlineSearch.ToLower().Equals("true"))
                    knowFrom += ",Online Search";
                if (tmpDetails.chkOnlineMedia != null && tmpDetails.chkOnlineMedia.ToLower().Equals("true"))
                    knowFrom += ",Online Media";
                if (tmpDetails.chkOther != null && tmpDetails.chkOther.ToLower().Equals("true"))
                    knowFrom += ",Other";
                if (knowFrom.Length > 0)
                    knowFrom = knowFrom.Remove(0, 1);

                /*check duplicate membership, check compulsory*/
                string queryCol = "select * from guestbookcompulsory where fieldname in ('NRIC', 'ContactNo') and (IsCompulsory = 0 or IsVisible = 0)";

                DataSet dscol = DataService.GetDataSet(new QueryCommand(queryCol));
                GuestBookCompulsoryCollection gbcol = new GuestBookCompulsoryCollection();
                gbcol.Load(dscol.Tables[0]);

                if (gbcol.Count == 1)
                {
                    if (gbcol[0].FieldName == "NRIC")
                    {
                        //check only by contactno 
                        string query = "Select * from Membership where Mobile = '" + tmpDetails.ContactNo + "' and ISNULL(Deleted,0) = 0 ";
                        DataSet ds = DataService.GetDataSet(new QueryCommand(query));
                        MembershipCollection col = new MembershipCollection();
                        col.Load(ds.Tables[0]);

                        if (col.Count > 0)
                            throw new Exception(string.Format("Member with Contact No equal {1} already registered. Cannot registered duplicate member", tmpDetails.NRIC.Trim(), tmpDetails.ContactNo));
                    }
                    else
                    {
                        //check only by NRIC
                        string query = "Select * from Membership where NRIC = '" + tmpDetails.NRIC.Trim() + "' and ISNULL(Deleted,0) = 0 ";
                        DataSet ds = DataService.GetDataSet(new QueryCommand(query));
                        MembershipCollection col = new MembershipCollection();
                        col.Load(ds.Tables[0]);

                        if (col.Count > 0)
                            throw new Exception(string.Format("Member with NRIC equal {0} already registered. Cannot registered duplicate member", tmpDetails.NRIC.Trim(), tmpDetails.ContactNo));

                    }
                }
                else
                {
                    //check by booth 
                    string query = "Select * from Membership where (NRIC = '" + tmpDetails.NRIC.Trim() + "' or Mobile = '" + tmpDetails.ContactNo + "') and ISNULL(Deleted,0) = 0 ";
                    DataSet ds = DataService.GetDataSet(new QueryCommand(query));
                    MembershipCollection col = new MembershipCollection();
                    col.Load(ds.Tables[0]);

                    if (col.Count > 0)
                        throw new Exception(string.Format("Member with NRIC equal {0} or Contact No equal {1} already registered. Cannot registered duplicate member", tmpDetails.NRIC.Trim(), tmpDetails.ContactNo));
                }

                PowerPOS.Membership member = new PowerPOS.Membership();
                member.MembershipNo = MembershipController.getNewMembershipNo(membershipcode);
                member.NameToAppear = tmpDetails.GivenName.Trim() + " " + tmpDetails.Surname.Trim();
                member.FirstName = firstName;
                member.LastName = tmpDetails.Surname;
                member.Email = tmpDetails.Email;
                member.Nric = tmpDetails.NRIC.ToUpper();
                member.StreetName = tmpDetails.HomeAddress;
                member.Mobile = tmpDetails.ContactNo;
                member.Child1Surname = "";
                member.Child1GivenName = tmpDetails.Child1GivenName;
                member.Child1DateOfBirth = tmpDetails.Child1DateBirth;
                member.Child2Surname = "";
                member.Child2GivenName = tmpDetails.Child2GivenName;
                member.Child2DateOfBirth = tmpDetails.Child2DateBirth;
                member.KnowFrom = knowFrom;
                member.PassCode = tmpDetails.PassCode;
                member.Nationality = tmpDetails.Nationality;
                member.Remarks = tmpDetails.Remark;
                //member.ExpiryDate = DateTime.ParseExact(expiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (tmpDetails.IsLifeTimeMember != null && tmpDetails.IsLifeTimeMember.ToLower().Equals("true"))
                {
                    member.ExpiryDate = new DateTime(2500, 1, 1);
                }
                else
                {
                    member.ExpiryDate = DateTime.Now.AddYears(1);
                }
                member.UniqueID = Guid.NewGuid();
                member.MembershipGroupId = defaultMembershipId;
                member.Deleted = false;
                member.Userflag5 = true;

                if (MembershipController.checkAdditionalFieldExist())
                {
                    //loading additional fields collection and control
                    MembershipCustomFieldCollection cr = new MembershipCustomFieldCollection();
                    cr.Where(MembershipCustomField.Columns.Deleted, false);
                    cr.Sort(MembershipCustomField.Columns.FieldName, true);
                    cr.Load();

                    if (cr != null && cr.Count > 0)
                    {
                        for (int j = 0; j < cr.Count; j++)
                        {
                            //column name in membership table is stored after comma
                            //string field = cr[i].FieldName.Substring(cr[i].FieldName.LastIndexOf(',') + 1);
                            string[] field = cr[j].FieldName.Split(',');
                            //field name column name consist of 4 ((0)index,(1)fieldname,
                            //(2)column name in membership table, and (3)order it is saved in that field of membership table)

                            //get the value from field in membership table
                            string da = (string)member.GetColumnValue(field[2]);

                            //if the data is empty (has never been processed before)
                            if (da == "" || da == null)
                            {
                                for (int k = 0; k < cr.Count; k++)
                                {
                                    string[] field2 = cr[k].FieldName.Split(',');
                                    if (field2[2] == field[2]) //if it is the same, concatenate to populate the field
                                    {
                                        string toBeWritten = '"' + field2[1] + '"' + ":" + ",";
                                        da = string.Concat(toBeWritten);
                                        member.SetColumnValue(field2[2], data);
                                    }
                                }
                            }

                            //there's value now
                            var value = JObject.Parse(data)[field[1]];
                            string[] splitData = da.Split(',');
                            if (splitData != null)
                            {
                                switch (cr[j].Type.ToLower())
                                {
                                    //only change the word where it is stored
                                    case "string":
                                        //save value from textbox

                                        splitData[Convert.ToInt32(field[3]) - 1] = '"' + field[1] + '"' + ":" + '"' + value.ToString() + '"';
                                        break;

                                    case "boolean":
                                        //save value from checkbox
                                        splitData[Convert.ToInt32(field[3]) - 1] = '"' + field[1] + '"' + ":" + '"' + value.ToString() + '"';
                                        //mbr.SetColumnValue(field, ((CheckBox)ctrl[i]).Checked);
                                        break;

                                    case "enum":
                                        //save value from combobox  
                                        splitData[Convert.ToInt32(field[3]) - 1] = '"' + field[1] + '"' + ":" + '"' + value.ToString() + '"';
                                        //mbr.SetColumnValue(field, ((DropDownList)ctrl[i]).SelectedValue);
                                        break;

                                    case "date":
                                        //save value from date textbox    
                                        DateTime dateOfBirth;
                                        if (DateTime.TryParse(value.ToString(), out dateOfBirth))
                                            splitData[Convert.ToInt32(field[3]) - 1] = '"' + field[1] + '"' + ":" + '"' + dateOfBirth.ToString("dd MMM yyyy") + '"';
                                        //mbr.SetColumnValue(field, dateOfBirth.ToString("dd MMM yyyy"));
                                        break;

                                    //case default:
                                    //    break;
                                }

                                da = ""; //clear data string
                                //fill in with new value
                                da = string.Join(",", splitData);
                                //write to the column in tmembership table
                                member.SetColumnValue(field[2], da);
                            }
                        }
                    }

                }
                member.Save("edgeworks");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Error save member :" + ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetGuestBookCompulsory()
        {
            string status = "";
            DataTable dt = new DataTable();

            GuestBookCompulsoryCollection col = new GuestBookCompulsoryCollection();
            try
            {
                string query = "select * from GuestBookCompulsory where ISNULL(Deleted,0) = 0";
                DataSet ds = DataService.GetDataSet(new QueryCommand(query));

                col.Load(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Error save member :" + ex.Message;
            }

            var result = new
            {
                data = col,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string CheckOutAutomatic()
        {
            string status = "";

            try
            {
                string autcheckoutinterval = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.AutoCheckOutIntervalGuestBook) ?? "0";
                if (string.IsNullOrEmpty(autcheckoutinterval))
                    autcheckoutinterval = "0";
                decimal interval = 0;
                if (!Decimal.TryParse(autcheckoutinterval, out interval))
                {
                    throw new Exception("Please set the check out interval correctly");
                }

                if (interval > 0)
                {
                    string query = "UPDATE guestbook SET OutTime = GETDATE(), ModifiedOn = GetDate(), ModifiedBy = 'SCRIPT' where DATEDIFF(hour, InTime, GetDate()) >= " + interval.ToString() + " and isnull(OutTime,'') =''";
                    DataService.ExecuteQuery(new QueryCommand(query));
                }

            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog("Error Check Out Automatic :" + ex.Message);
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetAdditionalInformation()
        {
            string status = "";
            MembershipCustomFieldCollection cr = new MembershipCustomFieldCollection();

            try
            {

                cr.Where(MembershipCustomField.Columns.Deleted, false);
                cr.Sort(MembershipCustomField.Columns.FieldName, true);
                cr.Load();

            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog("Error Get Additional Information:" + ex.Message);
            }

            var result = new
            {
                records = cr,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        #endregion

        #region *) Appointment and Room Reservation

        [WebMethod]
        public string GetAppointmentList(string dateAppointment, int skip, int take, string sortBy, bool isAscending)
        {
            string status = "";
            DataTable table = new DataTable();
            List<AppointmentResult> objReturn = new List<AppointmentResult>();
            try
            {
                DateTime Day = DateTime.Parse(dateAppointment);
                string SortOrder = "ASC";

                if (!isAscending)
                    SortOrder = "DESC";

                string Query = "Select * " +
                          " from ( " +
                          " select ROW_NUMBER() OVER (ORDER BY {0} {1}) AS 'RowNumber', * " +
                          " from ( " +
                          " select r.ResourceID, m.StartTime, r.ResourceName, ISNULL(u.DisplayName,'') as Staff, s.MembershipNo " +
                          " ,ISNULL(s.NameToAppear,'') as Customer, isnull(m.listStr,'') as ItemName, m.Duration, m.Id as AppointmentID " +
                          " , m.CheckInByWho, m.CheckOutByWho, m.Remark, m.CheckInTime, m.CheckOutTime " +
                          " from [resource] r " +
                          " inner join  " +
                          " (	 " +
                          "     select *, STUFF((SELECT  ',' + ItemName FROM AppointmentItem EE inner join Item i on EE.ItemNo = i.ItemNo WHERE EE.AppointmentId=E.Id and ISNULL(EE.Deleted,0) = 0 ORDER BY ItemName FOR XML PATH('')), 1, 1, '') AS listStr " +
                          "     from Appointment E " +
                          "     WHERE ISNULL(E.Deleted,0) = 0 and Convert(date,E.StartTime) = '{2}' " +
                          " )m on m.ResourceID = r.ResourceID " +
                          " left outer join UserMst u on m.SalesPersonID = u.UserName " +
                          " left outer join Membership s on s.MembershipNo = m.MembershipNo " +
                          " where ISNULL(r.Deleted,0) = 0 and r.resourceid != 'ROOM_DEFAULT' and ISNULL(u.Deleted,0) = 0 and ISNULL(s.Deleted,0) = 0 " +
                          " ) ex " +
                          " ) t ";
                string query2 = string.Format(Query, sortBy, SortOrder, Day.ToString("yyyy-MM-dd"));

                DataSet ds = DataService.GetDataSet(new QueryCommand(query2));
                table = ds.Tables[0];

                for (int index = skip; index < skip + take && index < table.Rows.Count; index++)
                {
                    var checkintime = new DateTime();
                    var checkouttime = new DateTime();
                    var starttime = new DateTime();
                    if (table.Rows[index]["StartTime"] != null && table.Rows[index]["StartTime"].ToString() != "")
                        starttime = (DateTime)table.Rows[index]["StartTime"];

                    if (table.Rows[index]["CheckInTime"] != null && table.Rows[index]["CheckInTime"].ToString() != "")
                        checkintime = (DateTime)table.Rows[index]["CheckInTime"];

                    if (table.Rows[index]["CheckOutTime"] != null && table.Rows[index]["CheckOutTime"].ToString() != "")
                        checkouttime = (DateTime)table.Rows[index]["CheckOutTime"];

                    objReturn.Add(new AppointmentResult()
                    {
                        AppointmentID = table.Rows[index]["AppointmentID"].ToString(),
                        MembershipNo = table.Rows[index]["MembershipNo"].ToString(),
                        Customer = table.Rows[index]["Customer"].ToString(),
                        ItemName = table.Rows[index]["ItemName"].ToString(),
                        ResourceName = table.Rows[index]["ResourceName"].ToString(),
                        Duration = Convert.ToInt32(table.Rows[index]["Duration"].ToString() ?? "0"),
                        StartTime = starttime.ToString("MM/dd/yyyy HH:mm:ss"),
                        Staff = table.Rows[index]["Staff"].ToString(),
                        CheckInByWho = table.Rows[index]["CheckInByWho"].ToString(),
                        CheckOutByWho = table.Rows[index]["CheckOutByWho"].ToString(),
                        CheckInTime = table.Rows[index]["CheckInTime"].ToString() == "" ? "-" : checkintime.ToString("MM/dd/yyyy HH:mm:ss"),
                        CheckOutTime = table.Rows[index]["CheckOutTime"].ToString() == "" ? "-" : checkouttime.ToString("MM/dd/yyyy HH:mm:ss"),
                        Remark = table.Rows[index]["Description"].ToString()
                    });
                }

            }
            catch (Exception ex)
            {
                status = "Error get data: " + ex.Message;
            }


            var result = new
            {
                status = status,
                data = objReturn,
                totalRecords = table.Rows.Count
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetAppointmentListWithResource(string dateAppointment, string resourceID, int skip, int take, string sortBy, bool isAscending)
        {
            string status = "";
            DataTable table = new DataTable();
            List<AppointmentResult> objReturn = new List<AppointmentResult>();
            try
            {
                DateTime Day = DateTime.Parse(dateAppointment);
                string SortOrder = "ASC";

                if (!isAscending)
                    SortOrder = "DESC";

                string Query = "Select * " +
                          " from ( " +
                          " select ROW_NUMBER() OVER (ORDER BY {0} {1}) AS 'RowNumber', * " +
                          " from ( " +
                          " select r.ResourceID, m.StartTime, r.ResourceName, ISNULL(u.DisplayName,'') as Staff, s.MembershipNo " +
                          " ,ISNULL(s.NameToAppear,'') as Customer, isnull(m.listStr,'') as ItemName, m.Duration, m.Id as AppointmentID " +
                          " , m.CheckInByWho, m.CheckOutByWho, m.Remark, m.CheckInTime, m.CheckOutTime, m.Description  " +
                          " from [resource] r " +
                          " inner join  " +
                          " (	 " +
                          "     select *, STUFF((SELECT  ',' + ItemName FROM AppointmentItem EE inner join Item i on EE.ItemNo = i.ItemNo WHERE EE.AppointmentId=E.Id and ISNULL(EE.Deleted,0) = 0 ORDER BY ItemName FOR XML PATH('')), 1, 1, '') AS listStr " +
                          "     from Appointment E " +
                          "     WHERE ISNULL(E.Deleted,0) = 0 and Convert(date,E.StartTime) = '{2}' " +
                          " )m on m.ResourceID = r.ResourceID " +
                          " left outer join UserMst u on m.SalesPersonID = u.UserName " +
                          " left outer join Membership s on s.MembershipNo = m.MembershipNo " +
                          " where ISNULL(r.Deleted,0) = 0 and r.resourceid != 'ROOM_DEFAULT' and (r.resourceId = '{3}' or '{3}' = '') and ISNULL(u.Deleted,0) = 0 and ISNULL(s.Deleted,0) = 0 " +
                          " ) ex " +
                          " ) t ";
                string query2 = string.Format(Query, sortBy, SortOrder, Day.ToString("yyyy-MM-dd"), resourceID);

                DataSet ds = DataService.GetDataSet(new QueryCommand(query2));
                table = ds.Tables[0];

                for (int index = skip; index < skip + take && index < table.Rows.Count; index++)
                {
                    var checkintime = new DateTime();
                    var checkouttime = new DateTime();
                    var starttime = new DateTime();
                    if (table.Rows[index]["StartTime"] != null && table.Rows[index]["StartTime"].ToString() != "")
                        starttime = (DateTime)table.Rows[index]["StartTime"];

                    if (table.Rows[index]["CheckInTime"] != null && table.Rows[index]["CheckInTime"].ToString() != "")
                        checkintime = (DateTime)table.Rows[index]["CheckInTime"];

                    if (table.Rows[index]["CheckOutTime"] != null && table.Rows[index]["CheckOutTime"].ToString() != "")
                        checkouttime = (DateTime)table.Rows[index]["CheckOutTime"];

                    objReturn.Add(new AppointmentResult()
                    {
                        AppointmentID = table.Rows[index]["AppointmentID"].ToString(),
                        MembershipNo = table.Rows[index]["MembershipNo"].ToString(),
                        Customer = table.Rows[index]["Customer"].ToString(),
                        ItemName = table.Rows[index]["ItemName"].ToString(),
                        ResourceName = table.Rows[index]["ResourceName"].ToString(),
                        Duration = Convert.ToInt32(table.Rows[index]["Duration"].ToString() ?? "0"),
                        StartTime = starttime.ToString("MM/dd/yyyy HH:mm:ss"),
                        Staff = table.Rows[index]["Staff"].ToString(),
                        CheckInByWho = table.Rows[index]["CheckInByWho"].ToString(),
                        CheckOutByWho = table.Rows[index]["CheckOutByWho"].ToString(),
                        CheckInTime = table.Rows[index]["CheckInTime"].ToString() == "" ? "-" : checkintime.ToString("MM/dd/yyyy HH:mm:ss"),
                        CheckOutTime = table.Rows[index]["CheckOutTime"].ToString() == "" ? "-" : checkouttime.ToString("MM/dd/yyyy HH:mm:ss"),
                        Remark = table.Rows[index]["Description"].ToString()
                    });
                }

            }
            catch (Exception ex)
            {
                status = "Error get data: " + ex.Message;
            }


            var result = new
            {
                status = status,
                data = objReturn,
                totalRecords = table.Rows.Count
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SaveAppointmentRemarks(string appointmentID, string remark)
        {
            string status = "";
            try
            {
                PowerPOS.Appointment appo = new PowerPOS.Appointment(appointmentID);
                appo.Description = remark;
                appo.IsServerUpdate = true;
                appo.Save();
            }
            catch (Exception ex)
            {
                status = "Error save remarks : " + ex.Message;
            }


            var result = new
            {
                status = status,
                data = remark
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string CheckInAppointment(string appointmentID, string userName, string checkIn)
        {
            string status = "";
            DateTime dateCheckIn = new DateTime();
            try
            {
                if (!DateTime.TryParse(checkIn, out dateCheckIn))
                {
                    throw new Exception("Invalid Date");
                }

                PowerPOS.Appointment appo = new PowerPOS.Appointment(appointmentID);

                if (!string.IsNullOrEmpty(appo.CheckInByWho))
                    throw new Exception("You are already checked in");

                appo.CheckInByWho = userName;
                appo.CheckInTime = dateCheckIn;
                appo.IsServerUpdate = true;

                appo.Save();
            }
            catch (Exception ex)
            {
                status = "Error Check In : " + ex.Message;
            }


            var result = new
            {
                status = status,
                data = new CheckInResult() { CheckInByWho = userName, CheckInTime = dateCheckIn.ToString("MM/dd/yyyy HH:mm:ss") }
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string UpdateCheckInTime(string appointmentID, string checkIn)
        {
            string status = "";
            DateTime dateCheckIn = new DateTime();
            try
            {
                if (!DateTime.TryParse(checkIn, out dateCheckIn))
                {
                    throw new Exception("Invalid Date");
                }

                PowerPOS.Appointment appo = new PowerPOS.Appointment(appointmentID);

                appo.CheckInTime = dateCheckIn;
                appo.IsServerUpdate = true;

                appo.Save();
            }
            catch (Exception ex)
            {
                status = "Error Check In : " + ex.Message;
            }


            var result = new
            {
                status = status,
                data = new CheckInResult() { CheckInTime = dateCheckIn.ToString("MM/dd/yyyy HH:mm:ss") }
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string CheckOutAppointment(string appointmentID, string userName)
        {
            string status = "";
            DateTime dateCheckOut = DateTime.Now;
            try
            {
                PowerPOS.Appointment appo = new PowerPOS.Appointment(appointmentID);

                if (appo.CheckInByWho == null || appo.CheckInByWho == "")
                    throw new Exception("You are not checked in yet");

                appo.CheckOutByWho = userName;
                appo.CheckOutTime = dateCheckOut;
                appo.IsServerUpdate = true;

                appo.Save();
            }
            catch (Exception ex)
            {
                status = "Error Check Out : " + ex.Message;
            }


            var result = new
            {
                status = status,
                data = new CheckOutResult() { CheckOutTime = dateCheckOut.ToString("dd-MM-yyyy HH:mm:ss"), CheckOutByWho = userName }
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetResourceList()
        {
            string status = "";
            List<ResourceResult> col = new List<ResourceResult>();
            try
            {
                string query = "Select * from Resource where ISNULL(Deleted,0) = 0 and ResourceID != 'ROOM_DEFAULT'";

                DataSet ds = DataService.GetDataSet(new QueryCommand(query));
                DataTable dt = ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    col.Add(new ResourceResult()
                    {
                        ResourceID = dt.Rows[i]["ResourceID"].ToString(),
                        ResourceName = dt.Rows[i]["ResourceName"].ToString()
                    });
                }

            }
            catch (Exception ex)
            {
                status = "Error Get Resource: " + ex.Message;
            }

            var result = new
            {
                status = status,
                data = col
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangeRoom(string appointmentID, string resourceID)
        {
            PowerPOS.Appointment app = new PowerPOS.Appointment(appointmentID);
            Resource res = new Resource(resourceID);
            string status = "";
            string message = "";

            try
            {
                if (!AppointmentController.CheckCollisionResource(app.Id.ToString(), app.SalesPersonID, res.ResourceID
                            , app.StartTime, app.Duration, out message))
                {
                    throw new Exception("Room is booked");
                }
                else
                {
                    app.ResourceID = res.ResourceID;
                    app.IsServerUpdate = true;
                    app.Save();
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            var result = new
            {
                status = status,
                data = res
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ExtendAppointment(string appointmentID, int duration)
        {
            PowerPOS.Appointment app = new PowerPOS.Appointment(appointmentID);
            string status = "";
            string message = "";

            try
            {

                if (!AppointmentController.CheckCollision(app.Id.ToString(), app.SalesPersonID, app.StartTime.AddMinutes((double)app.Duration), duration, out message))
                {
                    throw new Exception(message);
                }
                else
                {
                    if (!AppointmentController.CheckCollisionResource(app.Id.ToString(), app.SalesPersonID, app.ResourceID
                                , app.StartTime.AddMinutes((double)app.Duration), duration, out message))
                    {
                        throw new Exception("Room is booked");
                    }
                    else
                    {
                        app.Duration = app.Duration + duration;
                        if (app.TimeExtension == null)
                        {
                            app.TimeExtension = duration;
                        }
                        else
                        {
                            app.TimeExtension += duration;
                        }
                        app.IsServerUpdate = true;
                        app.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetMemberPastTransaction(string membershipNo)
        {
            List<PastTransactionResult> col = new List<PastTransactionResult>();
            string status = "";

            try
            {
                Membership member = new Membership(membershipNo);

                DataTable dt = member.GetPastTransaction(50);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    col.Add(new PastTransactionResult()
                    {
                        ItemNo = dt.Rows[i]["ItemNo"].ToString(),
                        OrderDate = dt.Rows[i]["OrderDate"].ToString(),
                        ItemName = dt.Rows[i]["ItemName"].ToString(),
                        SalesPerson = dt.Rows[i]["SalesPerson"].ToString(),
                        UnitPrice = dt.Rows[i]["UnitPrice"].ToString(),
                        Quantity = dt.Rows[i]["Quantity"].ToString(),
                        Amount = dt.Rows[i]["Amount"].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                status = "Error Get Transaction : " + ex.Message;
            }

            var result = new
            {
                status = status,
                data = col
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetAppointment(string appointmentID)
        {
            string status = "";
            DataTable dt = new DataTable();
            List<AppointmentResult> objReturn = new List<AppointmentResult>();
            try
            {
                dt = ResourceController.GetAppointment(appointmentID);

                if (dt.Rows.Count > 0)
                {
                    objReturn.Add(new AppointmentResult()
                    {
                        AppointmentID = dt.Rows[0]["AppointmentID"].ToString(),
                        MembershipNo = dt.Rows[0]["MembershipNo"].ToString(),
                        Customer = dt.Rows[0]["Customer"].ToString(),
                        ItemName = dt.Rows[0]["ItemName"].ToString(),
                        ResourceName = dt.Rows[0]["ResourceName"].ToString(),
                        Duration = Convert.ToInt32(dt.Rows[0]["Duration"].ToString() ?? "0"),
                        StartTime = dt.Rows[0]["StartTime"].ToString(),
                        Staff = dt.Rows[0]["Staff"].ToString(),
                        CheckInByWho = dt.Rows[0]["CheckInByWho"].ToString(),
                        CheckOutByWho = dt.Rows[0]["CheckOutByWho"].ToString(),
                        CheckInTime = dt.Rows[0]["CheckInTime"].ToString(),
                        CheckOutTime = dt.Rows[0]["CheckOutTime"].ToString(),
                        Remark = dt.Rows[0]["Description"].ToString()
                    });
                }


            }
            catch (Exception ex)
            {
                status = "Error get data: " + ex.Message;
            }


            var result = new
            {
                status = status,
                data = objReturn
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        #endregion

        #region *) Android Inventory

        [WebMethod]
        public string LoginAndroid(string username, string password)
        {
            LoginResult res = new LoginResult();
            try
            {

                string role = "", deptID = "", status = "";
                UserMst user;

                res.result = UserController.login(username, password, LoginType.Login, out user, out role, out deptID, out status);
                if (res.result)
                {
                    res.UserName = user.UserName;
                    res.DisplayName = user.DisplayName;
                    res.PointOfSaleID = user.AssignedPOS ?? "ALL";
                }
                res.Role = role;
                res.DeptID = deptID;
                res.status = status;

                DataTable dtPrivileges = UserController.FetchGroupPrivilegesWithUsername(role, user.UserName);
                List<string> privileges = new List<string>();
                foreach (DataRow dr in dtPrivileges.Rows)
                {
                    privileges.Add(dr["PrivilegeName"].ToString().Trim());
                }

                res.Privileges = privileges;

                CompanyCollection companies = new CompanyCollection().Load();
                if (companies.Count > 0)
                {
                    res.CompanyName = companies[0].CompanyName;
                }
                else
                {
                    res.CompanyName = "";
                }

                return new JavaScriptSerializer().Serialize(res);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                //res.Role = ""; res.DeptID = ""; res.InventoryLocation = null; res.PointOfSaleID = "";
                res.status = ex.Message;
                res.result = false;
                return new JavaScriptSerializer().Serialize(res);
            }
        }

        [WebMethod]
        public string GetItemTableRealTime(DateTime lastModifiedOn, string OutletName, int count)
        {
            DataSet dt = SynchronizationController.FetchProductRealTime(lastModifiedOn, OutletName, count);

            return Newtonsoft.Json.JsonConvert.SerializeObject(dt.Tables[0]);
        }

        [WebMethod]
        public string GetItemTableRealTimePaging(DateTime lastModifiedOn, string OutletName, int count, int skip)
        {
            DataSet dt = SynchronizationController.FetchProductRealTime(lastModifiedOn, OutletName, count, skip);

            return Newtonsoft.Json.JsonConvert.SerializeObject(dt.Tables[0]);
        }


        public class MembershipCustomer
        {
            public string MembershipNo { get; set; }
            public string NameToAppear { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public bool Deleted { get; set; }
            public string CreatedOn { get; set; }
            public string ModifiedOn { get; set; }
        }

        [WebMethod]
        public string GetListCustomer(DateTime lastModifiedOn)
        {
            string status = "";
            SqlConnection Sqlcon = new SqlConnection();
            //MembershipCollection cols = FetchAll(true);
            Sqlcon.ConnectionString = ConfigurationManager.ConnectionStrings["PowerPOSDBString"].ConnectionString;
            Sqlcon.Open();




            DataSet ds = new DataSet();
            try
            {
                //cols.Where(Membership.Columns.ModifiedOn, Comparison.GreaterOrEquals, lastModifiedOn);

                //cols.Load();
                SqlCommand Sqlcom = new SqlCommand();
                Sqlcom.CommandText = "Select MembershipNo,NameToAppear, FirstName, LastName,Email,  Deleted, CreatedOn, ModifiedOn from Membership Where ModifiedOn >=@lastModifedOn";
                Sqlcom.CommandType = CommandType.Text;
                Sqlcom.Connection = Sqlcon;
                Sqlcom.Parameters.AddWithValue("@lastModifedOn", lastModifiedOn);
                SqlDataAdapter SqlDap = new SqlDataAdapter(Sqlcom);

                SqlDap.Fill(ds);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            //var result = new
            //{
            //    records = cols,
            //    status = status
            //};
            DataTable dt = ds.Tables[0];

            List<MembershipCustomer> target = dt.AsEnumerable()
                            .Select(row => new MembershipCustomer
                            {
                                MembershipNo = String.IsNullOrEmpty(row.Field<string>(0)) ? "" : row.Field<string>(0),
                                NameToAppear = String.IsNullOrEmpty(row.Field<string>(1)) ? "" : row.Field<string>(1),
                                FirstName = String.IsNullOrEmpty(row.Field<string>(2)) ? "" : row.Field<string>(2),
                                LastName = String.IsNullOrEmpty(row.Field<string>(3)) ? "" : row.Field<string>(3),
                                Email = String.IsNullOrEmpty(row.Field<string>(4)) ? "" : row.Field<string>(4),
                                Deleted = row.Field<bool?>(5).HasValue ? row.Field<bool>(5) : false,
                                CreatedOn = row.Field<DateTime>(6).ToString("yyyy-MM-dd hh:mm:ss"),
                                ModifiedOn = row.Field<DateTime>(7).ToString("yyyy-MM-dd hh:mm:ss"),
                                // assuming column 0's type is Nullable<long>
                                // = row.Field<long?>(0).GetValueOrDefault(),
                                //CardName = String.IsNullOrEmpty(row.Field<string>(1))
                                //    ? "not found"
                                //    : row.Field<string>(1),
                            }).ToList();

            var result = new
            {
                records = target,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }




        [WebMethod]
        public string GetAlternateBarcodeTable()
        {
            string status = "";
            AlternateBarcodeCollection col = new AlternateBarcodeCollection();
            try
            {
                col.Where(i => i.Deleted == false);
                col.Load();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = col.Select(x => new { BarcodeID = x.BarcodeID, Barcode = x.Barcode, ItemNo = x.ItemNo }).ToList(),
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetItemQuantityTriggerTable()
        {
            string status = "";
            ItemQuantityTriggerCollection col = new ItemQuantityTriggerCollection();
            try
            {
                col.Load();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = col.Select(x => new
                {
                    TriggerID = x.TriggerID,
                    ItemNo = x.ItemNo,
                    TriggerQuantity = x.TriggerQuantity,
                    TriggerLevel = x.TriggerLevel,
                    CreatedOn = x.CreatedOn,
                    CreatedBy = x.CreatedBy,
                    ModifiedOn = x.ModifiedOn,
                    ModifiedBy = x.ModifiedBy,
                    Deleted = x.Deleted,
                    InventoryLocationID = x.InventoryLocationID,
                    userfld1 = x.Userfld1,
                    userfld2 = x.Userfld2,
                    userfld3 = x.Userfld3,
                    userfld4 = x.Userfld4,
                    userfld5 = x.Userfld5,
                    userfld6 = x.Userfld6,
                    userfld7 = x.Userfld7,
                    userfld8 = x.Userfld8,
                    userfld9 = x.Userfld9,
                    userfld10 = x.Userfld10
                }).ToList(),
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetItemQuantityTriggerRealTimePaging(DateTime lastModifiedOn, int inventoryLocationID, int count, int skip)
        {
            DataSet dt = SynchronizationController.FetchItemQuantityTriggerRealTime(lastModifiedOn, inventoryLocationID, count, skip);

            return Newtonsoft.Json.JsonConvert.SerializeObject(dt.Tables[0]);
        }

        [WebMethod]
        public string GetItemQuantityTriggerRecordCountAfterTimestamp(DateTime lastModifiedOn, int inventoryLocationID)
        {
            int count = SynchronizationController.GetItemQuantityTriggerTableRealTimeCount(lastModifiedOn, inventoryLocationID);

            var result = new
            {
                data = count
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetItemRecordCountAfterTimestamp(DateTime lastModifiedOn, string OutletName)
        {
            int count = SynchronizationController.GetItemTableRealTimeCount(lastModifiedOn, OutletName);

            var result = new
            {
                data = count
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string StockInAndroid(string detail, string username, int InventoryLocationID,
            bool IsAdjustment, bool CalculateCOGS, string stockInDate, string uniqueID)
        {
            string status = "";
            string newRefNo = "";
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            List<InventoryDet> tmpDetails = new JavaScriptSerializer().Deserialize<List<InventoryDet>>(detail);

            IDataReader rdr = InventoryHdr.CreateQuery().WHERE(InventoryHdr.UniqueIDColumn.ColumnName, uniqueID).ExecuteReader();
            if (rdr.Read())
            {
                newRefNo = rdr[InventoryHdr.InventoryHdrRefNoColumn.ColumnName].ToString();
            }
            else
            {
                try
                {
                    PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                    string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                    if (strCostingMethod.ToLower() == "fifo")
                        CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                    else if (strCostingMethod.ToLower() == "fixed avg")
                        CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                    else
                        CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                    InventoryController ctrl = new InventoryController(CostingMethod);
                    ctrl.SetInventoryHeaderInfo("", "", "", 0, 0, 0);
                    ctrl.SetInventoryDate(DateTime.ParseExact(stockInDate, "dd-MM-yyyy HH:mm:ss", provider));
                    ctrl.InvHdr.UniqueID = new Guid(uniqueID);

                    foreach (var det in tmpDetails)
                    {
                        ctrl.AddItemIntoInventoryForStockIn(det.ItemNo, det.Quantity.GetValueOrDefault(0), det.FactoryPrice, det.Userfld1, out status);
                    }



                    if (ctrl.StockIn(username, InventoryLocationID, IsAdjustment, CalculateCOGS, out status))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();
                    }
                    else
                    {
                        newRefNo = "";
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    status = ex.Message;
                    newRefNo = "";
                }
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string StockInAndroidCM(string detail, string username, int InventoryLocationID,
            bool IsAdjustment, bool CalculateCOGS, string stockInDate, string uniqueID, string jsonParam)
        {
            StockInCMParam param = new JavaScriptSerializer().Deserialize<StockInCMParam>(jsonParam);

            string status = "";
            string newRefNo = "";
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            List<InventoryDet> tmpDetails = new JavaScriptSerializer().Deserialize<List<InventoryDet>>(detail);

            IDataReader rdr = InventoryHdr.CreateQuery().WHERE(InventoryHdr.UniqueIDColumn.ColumnName, uniqueID).ExecuteReader();
            if (rdr.Read())
            {
                newRefNo = rdr[InventoryHdr.InventoryHdrRefNoColumn.ColumnName].ToString();
            }
            else
            {
                try
                {
                    PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                    string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                    if (strCostingMethod.ToLower() == "fifo")
                        CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                    else if (strCostingMethod.ToLower() == "fixed avg")
                        CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                    else
                        CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                    InventoryController ctrl = new InventoryController(CostingMethod);
                    ctrl.SetInventoryHeaderInfo("", "", "", 0, 0, 0);
                    ctrl.SetInventoryDate(DateTime.ParseExact(stockInDate, "dd-MM-yyyy HH:mm:ss", provider));
                    ctrl.InvHdr.UniqueID = new Guid(uniqueID);
                    if (param.InvoiceNo != null)
                        ctrl.InvHdr.Userfld1 = param.InvoiceNo;
                    if (param.SupplierID > 0)
                        ctrl.InvHdr.Supplier = param.SupplierID.ToString();
                    if (param.Remark != null)
                        ctrl.SetRemark(param.Remark);
                    if (param.PurchaseOrderNo != null)
                        ctrl.SetPurchaseOrder(param.PurchaseOrderNo);

                    if (param.SupplierID > 0)
                    {
                        // Follow GST from Supplier. If not exists, set to Inclusive
                        Supplier s = new Supplier(param.SupplierID);
                        if (s != null && s.SupplierID == param.SupplierID)
                        {
                            ctrl.SetGSTRule(s.Userint1 ?? 2);
                        }
                    }
                    else
                    {
                        // No supplier specified, set to Inclusive
                        ctrl.SetGSTRule(2);
                    }

                    foreach (var det in tmpDetails)
                    {
                        //Adi validate 0
                        if (det.Quantity == 0)
                            continue;

                        ctrl.AddItemIntoInventoryForStockIn(det.ItemNo, det.Quantity.GetValueOrDefault(0), det.FactoryPrice, det.Userfld1, out status);
                    }

                    ctrl.CalculateAdditionalCost();
                    if (!ctrl.SetGST(ctrl.InvHdr.GSTRule, true, out status))
                    {
                        throw new Exception(string.Format("Error: {0}", status));
                    }


                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.StockInSaveToFile), false))
                    {
                        /*if (ctrl.getUniqueID() == Guid.Empty)
                        {
                            ctrl.createNewGUID();
                        }*/
                        //Logger.writeLog("CP1");
                        ctrl.SetInventoryLocation(InventoryLocationID);
                        ctrl.SetInventoryHdrUserName(username);
                        //Logger.writeLog("CP2");
                        DataSet myDataSet = new DataSet();
                        myDataSet.Tables.Add(ctrl.InvHdrToDataTable());
                        myDataSet.Tables.Add(ctrl.InvDetToDataTable());
                        string remark = "GOODS RECEIVE";
                        string suppliername = ctrl.getSupplierName();
                        string invLocName = "";
                        InventoryLocation il = new InventoryLocation(InventoryLocationID);
                        if (il != null && il.InventoryLocationName != null && il.InventoryLocationName != "")
                        {
                            invLocName = il.InventoryLocationName;
                        }
                        UserInfo.username = username;
                        //Logger.writeLog("CP3");
                        SynchronizationController.SaveInventoryFile(myDataSet, "GOODS RECEIVE", remark + "-" + il.InventoryLocationName + "-" + suppliername, false);
                        newRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
                    }
                    else
                    {
                        if (ctrl.StockIn(username, InventoryLocationID, IsAdjustment, CalculateCOGS, out status))
                        {
                            newRefNo = ctrl.GetInvHdrRefNo();
                        }
                        else
                        {
                            newRefNo = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    status = ex.Message;
                    newRefNo = "";
                }
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string StockOutAndroid(string detail, string username, int StockOutReasonID,
        int InventoryLocationID, bool IsAdjustment, bool deductRemainingQty, string remark, string stockOutDate, string uniqueID)
        {
            string status = "";
            string newRefNo = "";
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            List<InventoryDet> tmpDetails = new JavaScriptSerializer().Deserialize<List<InventoryDet>>(detail);

            IDataReader rdr = InventoryHdr.CreateQuery().WHERE(InventoryHdr.UniqueIDColumn.ColumnName, uniqueID).ExecuteReader();
            if (rdr.Read())
            {
                newRefNo = rdr[InventoryHdr.InventoryHdrRefNoColumn.ColumnName].ToString();
            }
            else
            {
                try
                {
                    PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                    string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                    if (strCostingMethod.ToLower() == "fifo")
                        CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                    else if (strCostingMethod.ToLower() == "fixed avg")
                        CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                    else
                        CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                    InventoryController ctrl = new InventoryController(CostingMethod);
                    ctrl.SetInventoryHeaderInfo("", "", "", 0, 0, 0);
                    ctrl.SetRemark(remark);
                    ctrl.SetInventoryDate(DateTime.ParseExact(stockOutDate, "dd-MM-yyyy HH:mm:ss", provider));
                    ctrl.SetInventoryLocation(InventoryLocationID);
                    ctrl.InvHdr.UniqueID = new Guid(uniqueID);

                    foreach (var det in tmpDetails)
                    {
                        ctrl.AddItemIntoInventoryForStockOut(det.ItemNo, det.Quantity.GetValueOrDefault(0), det.Userfld1, out status);
                    }

                    if (ctrl.StockOut(username, StockOutReasonID, InventoryLocationID, IsAdjustment, deductRemainingQty, out status))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();
                    }
                    else
                    {
                        newRefNo = "";
                    }
                }
                catch (Exception ex)
                {
                    newRefNo = "";
                    Logger.writeLog(ex);
                    status = ex.Message;
                }
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string StockTakeAndroid(string detail, string username, string takenBy, string verifiedBy, int InventoryLocationID, string stockTakeDate, string uniqueID)
        {
            Logger.writeLog("JSON=" + detail + "; Username=" + username + "; takenBy=" + takenBy + "; verifiedBy=" + verifiedBy + "; InventoryLocationID=" + InventoryLocationID + "; stockTakeDate=" + stockTakeDate + "; uniqueID=" + uniqueID);

            string status = "";
            bool success = false;
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            List<InventoryDet> tmpDetails = new JavaScriptSerializer().Deserialize<List<InventoryDet>>(detail);

            try
            {
                string sql = @"
                    SELECT  COUNT(*) RowNo 
                    FROM	StockTake
                    WHERE	USerfld2 = @UniqueID";
                QueryCommand cmd = new QueryCommand(sql);
                cmd.AddParameter("@UniqueID", uniqueID, DbType.String);

                DataTable dtExist = new DataTable();
                dtExist.Load(DataService.GetReader(cmd));

                if (dtExist.Rows.Count > 0)
                {
                    int rowNo = (dtExist.Rows[0]["RowNo"] + "").GetInt32Value();
                    if (rowNo > 0)
                    {
                        var res = new
                        {
                            success = true
                        };

                        return new JavaScriptSerializer().Serialize(res);
                    }
                }

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);
                ctrl.SetInventoryHeaderInfo("", "", "", 0, 0, 0);
                ctrl.SetInventoryDate(DateTime.ParseExact(stockTakeDate, "dd-MM-yyyy HH:mm:ss", provider));
                ctrl.SetInventoryLocation(InventoryLocationID);
                ctrl.InvHdr.UniqueID = new Guid(uniqueID);

                foreach (var det in tmpDetails)
                {
                    ctrl.AddItemIntoInventoryStockTakeWithBatchNo(det.ItemNo, det.Quantity.GetValueOrDefault(0), det.CostOfGoods, det.Userfld1, out status);
                }

                if (ctrl.CreateStockTakeEntriesWithBatchNo(username, takenBy, verifiedBy, out status))
                {
                    success = true;
                    status = "";
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                success = false;
                status = ex.Message;
            }

            var result = new
            {
                success = success,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string StockTransferAndroid(string detail, string username, int InventoryLocationID, int TransferInventoryLocationID, string stockTransferDate, string uniqueID)
        {
            string status = "";
            string newRefNo = "";
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            List<InventoryDet> tmpDetails = new JavaScriptSerializer().Deserialize<List<InventoryDet>>(detail);

            IDataReader rdr = InventoryHdr.CreateQuery().WHERE(InventoryHdr.UniqueIDColumn.ColumnName, uniqueID).ExecuteReader();
            if (rdr.Read())
            {
                newRefNo = rdr[InventoryHdr.InventoryHdrRefNoColumn.ColumnName].ToString();
            }
            else
            {
                try
                {
                    PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                    string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                    if (strCostingMethod.ToLower() == "fifo")
                        CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                    else if (strCostingMethod.ToLower() == "fixed avg")
                        CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                    else
                        CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                    InventoryController ctrl = new InventoryController(CostingMethod);
                    ctrl.SetInventoryHeaderInfo("", "", "", 0, 0, 0);
                    ctrl.SetInventoryDate(DateTime.ParseExact(stockTransferDate, "dd-MM-yyyy HH:mm:ss", provider));
                    ctrl.InvHdr.UniqueID = new Guid(uniqueID);

                    foreach (var det in tmpDetails)
                    {
                        ctrl.AddItemIntoInventoryForStockIn(det.ItemNo, det.Quantity.GetValueOrDefault(0), det.FactoryPrice, det.Userfld1, out status);
                    }

                    if (ctrl.TransferOutAutoReceive(username, InventoryLocationID, TransferInventoryLocationID, out status))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();
                    }
                    else
                    {
                        newRefNo = "";
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    status = ex.Message;
                    newRefNo = "";
                }
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetWarehouseBalanceAndroid(string itemNo, string date, int InventoryLocationID)
        {
            string status = "";
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            decimal WHBal = 0;

            try
            {
                WHBal = InventoryController.GetStockBalanceQtyByItemSummaryByDate(itemNo, InventoryLocationID, DateTime.ParseExact(date, "dd-MM-yyyy HH:mm:ss", provider), out status);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                stock = WHBal,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetPromotionAndroid(string itemNo, string date, int InventoryLocationID)
        {
            List<Object> results = new List<object>();
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            InventoryLocation inventoryLoc = new InventoryLocation("InventoryLocationID", InventoryLocationID);

            DateTime myDate = DateTime.Now;

            try
            {
                myDate = DateTime.ParseExact(date, "dd-MM-yyyy", provider);
            }
            catch (Exception e)
            {
            }

            Query q1 = Outlet.CreateQuery().WHERE("InventoryLocationID = " + InventoryLocationID).AND("Deleted = false");
            IDataReader rdr = q1.ExecuteReader();
            while (rdr.Read())
            {
                Outlet a = new Outlet("OutletName", rdr[Outlet.Columns.OutletName].ToString());

                Query q2 = PointOfSale.CreateQuery().WHERE("OutletName = " + a.OutletName);
                IDataReader rdr2 = q2.ExecuteReader();
                if (rdr2.Read())
                {
                    PointOfSale pos = new PointOfSale("PointOfSaleID", rdr2[PointOfSale.Columns.PointOfSaleID].ToString());
                    if (pos != null)
                    {
                        var ds = SPs.FetchAllPossiblePromoAnyXofAllItems("'" + itemNo + "'", true, pos.PointOfSaleID).GetDataSet();
                        DataTable dt = ds.Tables[0];

                        //looping for every promo
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int PromoCampaignHdrId = Int32.Parse(dt.Rows[i][0].ToString() ?? "0");
                            PromoCampaignHdr promo = new PromoCampaignHdr("PromoCampaignHdrID", PromoCampaignHdrId);

                            if (myDate >= promo.DateFrom && myDate <= promo.DateTo)
                            {
                                results.Add(new
                                {
                                    PromoCampaignName = promo.PromoCampaignName,
                                    From = promo.DateFrom.ToString("dd-MM-yyyy HH:mm:ss"),
                                    To = promo.DateTo.ToString("dd-MM-yyyy HH:mm:ss"),
                                    Outlet = a.OutletName
                                });
                            }
                        }
                    }
                }
            }

            return new JavaScriptSerializer().Serialize(results);
        }

        [WebMethod]
        public string GetParValue(string itemNo, string field)
        {
            string status = "";
            string tmp = "";

            try
            {
                Query q = ItemQuantityTrigger.CreateQuery().WHERE("ItemNo = " + itemNo + "");
                IDataReader rdr = q.ExecuteReader();
                if (rdr.Read())
                {
                    switch (field.ToLower())
                    {
                        case "userfld1":
                            tmp = rdr[ItemQuantityTrigger.Columns.Userfld1].ToString();
                            break;
                        case "userfld2":
                            tmp = rdr[ItemQuantityTrigger.Columns.Userfld2].ToString();
                            break;
                        case "userfld3":
                            tmp = rdr[ItemQuantityTrigger.Columns.Userfld3].ToString();
                            break;
                        case "userfld4":
                            tmp = rdr[ItemQuantityTrigger.Columns.Userfld4].ToString();
                            break;
                        case "userfld5":
                            tmp = rdr[ItemQuantityTrigger.Columns.Userfld5].ToString();
                            break;
                        case "userfld6":
                            tmp = rdr[ItemQuantityTrigger.Columns.Userfld6].ToString();
                            break;
                        case "userfld7":
                            tmp = rdr[ItemQuantityTrigger.Columns.Userfld7].ToString();
                            break;
                        case "userfld8":
                            tmp = rdr[ItemQuantityTrigger.Columns.Userfld8].ToString();
                            break;
                        case "userfld9":
                            tmp = rdr[ItemQuantityTrigger.Columns.Userfld9].ToString();
                            break;
                        case "userfld10":
                            tmp = rdr[ItemQuantityTrigger.Columns.Userfld10].ToString();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                ParValue = tmp,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetSuppliers()
        {
            string status = "";
            SupplierCollection coll = new SupplierCollection();

            try
            {
                coll.Where(i => i.Deleted == null || i.Deleted == false);
                coll.Load();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = coll.Select(x => new
                {
                    SupplierID = x.SupplierID,
                    SupplierName = x.SupplierName,
                    CustomerAddress = x.CustomerAddress,
                    ShipToAddress = x.ShipToAddress,
                    BillToAddress = x.BillToAddress,
                    ContactNo1 = x.ContactNo1,
                    ContactNo2 = x.ContactNo2,
                    ContactNo3 = x.ContactNo3,
                    AccountNo = x.AccountNo,
                    IsWarehouse = x.IsWarehouse,
                    WarehouseID = x.WarehouseID
                }),
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetSuppliersUserPortal(string username)
        {
            string status = "";
            DataTable dt = new DataTable();

            try
            {
                UserMst us = new UserMst(UserMst.Columns.UserName, username);

                string query = @"select 0 as SupplierID, 'ALL' as SupplierName,NULL as CustomerAddress, NULL as ShiptoAddress, NULL as BillToAddress
	                                    , NULL as ContactNo1, NULL as ContactNo2, NULL as ContactNo2, NULL as ContactNo3, NULL as AccountNo
	                                    , NULL as  IsWarehouse, NULL as  WarehouseID
                                    UNION
                                    select SupplierID, SupplierName, CustomerAddress, ShiptoAddress, BillToAddress
	                                    , ContactNo1, ContactNo2, ContactNo2, ContactNo3,AccountNo
	                                    , ISNULL(userflag1, 0) as IsWarehouse, Userint2 as WarehouseID
                                    from supplier
                                    where ISNULL(deleted,0) = 0 {0}";


                bool isRestricted = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseSupplierPortal), false) && us != null && us.IsSupplier && us.IsRestrictedSupplierList;

                string restrictquery = "";
                if (isRestricted)
                    restrictquery = string.Format(" AND ISNULL(userfld4,'') = '{0}'", us.UserName);

                query = string.Format(query, restrictquery);

                dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];


            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = dt,
                status = status
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string GetSuppliersUserPortalWithoutWarehouse(string username)
        {
            string status = "";
            DataTable dt = new DataTable();

            try
            {
                UserMst us = new UserMst(UserMst.Columns.UserName, username);

                string query = @"select 0 as SupplierID, 'ALL' as SupplierName,NULL as CustomerAddress, NULL as ShiptoAddress, NULL as BillToAddress
	                                    , NULL as ContactNo1, NULL as ContactNo2, NULL as ContactNo2, NULL as ContactNo3, NULL as AccountNo
	                                    , NULL as  IsWarehouse, NULL as  WarehouseID
                                    UNION
                                    select SupplierID, SupplierName, CustomerAddress, ShiptoAddress, BillToAddress
	                                    , ContactNo1, ContactNo2, ContactNo2, ContactNo3,AccountNo
	                                    , ISNULL(userflag1, 0) as IsWarehouse, Userint2 as WarehouseID
                                    from supplier
                                    where ISNULL(deleted,0) = 0 and ISNULL(userflag1,0) = 0 {0}";


                bool isRestricted = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseSupplierPortal), false) && us != null && us.IsSupplier && us.IsRestrictedSupplierList;

                string restrictquery = "";
                if (isRestricted)
                    restrictquery = string.Format(" AND ISNULL(userfld4,'') = '{0}'", us.UserName);

                query = string.Format(query, restrictquery);

                dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];


            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = dt,
                status = status
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string GetWarehouseList()
        {
            string status = "";
            DataTable dt = new DataTable();

            try
            {
                string querySelect = "select * from supplier where ISNULL(deleted,0) = 0 and ISNULL(userflag1,0) = 1 ";
                DataTable dtCheck = DataService.GetDataSet(new QueryCommand(querySelect)).Tables[0];

                string query = "";

                if (dtCheck.Rows.Count == 1)
                {
                    query = @"      select SupplierID, SupplierName, CustomerAddress, ShiptoAddress, BillToAddress
	                                    , ContactNo1, ContactNo2, ContactNo2, ContactNo3,AccountNo
	                                    , ISNULL(userflag1, 0) as IsWarehouse, Userint2 as WarehouseID
                                    from supplier
                                    where ISNULL(deleted,0) = 0 and ISNULL(userflag1,0) = 1 ";
                }
                else
                {
                    query = @"select 0 as SupplierID, 'ALL' as SupplierName,NULL as CustomerAddress, NULL as ShiptoAddress, NULL as BillToAddress
	                                    , NULL as ContactNo1, NULL as ContactNo2, NULL as ContactNo2, NULL as ContactNo3, NULL as AccountNo
	                                    , NULL as  IsWarehouse, NULL as  WarehouseID
                                    UNION
                                    select SupplierID, SupplierName, CustomerAddress, ShiptoAddress, BillToAddress
	                                    , ContactNo1, ContactNo2, ContactNo2, ContactNo3,AccountNo
	                                    , ISNULL(userflag1, 0) as IsWarehouse, Userint2 as WarehouseID
                                    from supplier
                                    where ISNULL(deleted,0) = 0 and ISNULL(userflag1,0) = 1 ";
                }
                dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = dt,
                status = status
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string NewProduct(string username, string parameters)
        {
            var result = new NewProductResult
            {
                status = "",
                resultItemNo = "",
                resultMatrixMode = ""
            };
            Logger.writeLog("Update Price via APP:" + parameters);

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            NewProductParam _ps = new JavaScriptSerializer().Deserialize<NewProductParam>(parameters);

            ItemController itemLogic = new ItemController();

            #region Validations

            if (!_ps.IsNew.HasValue)
            {
                result.status = "IsNew is required";
                return new JavaScriptSerializer().Serialize(result);
            }

            if (!_ps.IsNew.Value && (_ps.ItemNo == null || _ps.ItemNo == ""))
            {
                result.status = "ItemNo is required";
                return new JavaScriptSerializer().Serialize(result);
            }

            if (_ps.IsNew.Value)
            {
                if (_ps.ItemName == null || _ps.ItemName == "")
                {
                    result.status = "ItemName is required";
                    return new JavaScriptSerializer().Serialize(result);
                }

                if (_ps.Barcode == null || _ps.Barcode == "")
                {
                    result.status = "Barcode is required";
                    return new JavaScriptSerializer().Serialize(result);
                }

                if (itemLogic.CheckIfBarcodeExists(_ps.Barcode, _ps.ItemNo))
                {
                    result.status = "Barcode is duplicated";
                    return new JavaScriptSerializer().Serialize(result);
                }

                if (_ps.CategoryName == null || _ps.CategoryName == "")
                {
                    result.status = "CategoryName is required";
                    return new JavaScriptSerializer().Serialize(result);
                }
            }

            #endregion

            bool DisplayPrice1 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice1), true);
            bool DisplayPrice2 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice2), true);
            bool DisplayPrice3 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice3), true);
            bool DisplayPrice4 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice4), true);
            bool DisplayPrice5 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice5), true);

            string P1Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P1DiscountName);
            string P2Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P2DiscountName);
            string P3Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P3DiscountName);
            string P4Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P4DiscountName);
            string P5Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P5DiscountName);

            if (_ps.IsNew.Value)
            {
                if (_ps.ItemNo == "")
                {
                    int pointOfSaleID = 0;
                    if (Int32.TryParse(ConfigurationManager.AppSettings.Get("PointOfSaleID").ToString(), out pointOfSaleID))
                        PointOfSaleInfo.PointOfSaleID = pointOfSaleID;

                    _ps.ItemNo = ItemController.getNewItemRefNo();
                }
            }

            bool isSuccess = false;

            try
            {
                QueryCommandCollection cmdCol = new QueryCommandCollection();
                QueryCommand mycmd;

                //System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                //to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                Item item;

                #region *) Matrix Mode
                //matrix mode
                if (_ps.Matrix.GetValueOrDefault(false))
                {
                    result.status = "Matrix Mode is not implemented yet";
                    return new JavaScriptSerializer().Serialize(result);
                }
                #endregion

                #region *) Normal Mode
                else
                {
                    Item originalItem = new Item();

                    if (!(_ps.IsNew.GetValueOrDefault(false)))
                    {
                        item = new Item(_ps.ItemNo);
                        item.IsNew = false;
                        item.CopyTo(originalItem);
                        originalItem.IsNew = false;
                    }
                    else
                    {
                        _ps.DefaultNewProduct();

                        item = new Item();
                        item.ItemNo = _ps.ItemNo;
                        item.IsNew = true;
                        item.UniqueID = Guid.NewGuid();
                    }
                    item.Userflag1 = false;
                    item.Userfld1 = _ps.UOM;

                    int countAltBar = 0;
                    string queryBarcode = "select count(*) as Co from AlternateBarcode where Barcode = '{0}' and ItemNo = '{1}' and ISNULL(deleted,0) = 0";
                    queryBarcode = string.Format(queryBarcode, _ps.Barcode, _ps.ItemNo);
                    using (IDataReader rdrB = DataService.GetReader(new QueryCommand(queryBarcode)))
                    {
                        if (rdrB.Read())
                            int.TryParse(rdrB["Co"].ToString(), out countAltBar);

                        if (countAltBar == 0)
                            item.Barcode = _ps.Barcode;
                    }

                    item.ItemName = _ps.ItemName;
                    if (item.IsNew)
                        item.IsInInventory = true;
                    item.CategoryName = _ps.CategoryName;
                    //item.MinimumPrice = _ps.MinimumPrice.GetValueOrDefault(0m);


                    #region *) If multioutlet then set as Outlet, else set as Product Master

                    int countOutlet = 0;
                    QueryCommand cmdCountOutlet = new QueryCommand(@"SELECT COUNT(*) as c FROM Outlet WHERE ISNULL(Deleted, 0) = 0");
                    using (IDataReader rdr = DataService.GetReader(cmdCountOutlet))
                    {
                        if (rdr.Read())
                            int.TryParse(rdr["c"].ToString(), out countOutlet);

                        if (countOutlet > 1)
                        {
                            _ps.ApplicableTo = "Outlet";
                            if (string.IsNullOrEmpty(_ps.OutletName))
                            {
                                result.status = "OutletName is required";
                                return new JavaScriptSerializer().Serialize(result);
                            }
                        }
                        else
                        {
                            _ps.ApplicableTo = "Product Master";
                        }
                    }

                    if (AppSetting.GetSetting(AppSetting.SettingsName.Mobile.UpdateProductApplicableTo) != null &&
                        AppSetting.GetSetting(AppSetting.SettingsName.Mobile.UpdateProductApplicableTo).ToUpper() == "PRODUCT MASTER")
                    {
                        _ps.ApplicableTo = "Product Master";
                    }


                    #endregion


                    Logger.writeLog("applicable to" + _ps.ApplicableTo);
                    Logger.writeLog("Update price");
                    if (_ps.ApplicableTo == "Product Master")
                    {
                        item.RetailPrice = _ps.RetailPrice.GetValueOrDefault(0m);
                        item.FactoryPrice = _ps.FactoryPrice.GetValueOrDefault(0m);
                        item.Deleted = _ps.Deleted.GetValueOrDefault(false);
                    }
                    else if (_ps.ApplicableTo == "Outlet Group")
                    {
                        item.RetailPrice = _ps.RetailPrice.GetValueOrDefault(0m);
                        Query qr = new Query("OutletGroupItemMap");
                        qr.AddWhere(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                        qr.AddWhere(OutletGroupItemMap.Columns.OutletGroupID, _ps.OutletGroupID.GetValueOrDefault());

                        OutletGroupItemMap col = new OutletGroupItemMapController().FetchByQuery(qr).FirstOrDefault();

                        if (col != null)
                        {
                            col.RetailPrice = _ps.RetailPrice.GetValueOrDefault(0m);
                            col.CostPrice = _ps.FactoryPrice.GetValueOrDefault(0m);
                            col.Deleted = _ps.Deleted.GetValueOrDefault(false);
                            //col.IsItemDeleted = ;
                            col.Save(username);
                        }
                        else
                        {
                            col = new OutletGroupItemMap();
                            col.OutletGroupID = _ps.OutletGroupID.GetValueOrDefault();
                            col.ItemNo = item.ItemNo;
                            col.RetailPrice = _ps.RetailPrice.GetValueOrDefault(0m);
                            col.CostPrice = _ps.FactoryPrice.GetValueOrDefault(0m);
                            col.Deleted = _ps.Deleted.GetValueOrDefault(false);
                            //col.IsItemDeleted = ;
                            col.Save(username);
                        }

                    }
                    else if (_ps.ApplicableTo == "Outlet")
                    {
                        if (item.IsNew)
                        {
                            item.RetailPrice = _ps.RetailPrice.GetValueOrDefault(0m);
                            item.FactoryPrice = _ps.FactoryPrice.GetValueOrDefault(0m);
                        }

                        Query qr = new Query("OutletGroupItemMap");
                        qr.AddWhere(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                        qr.AddWhere(OutletGroupItemMap.Columns.OutletName, _ps.OutletName);

                        OutletGroupItemMap col = new OutletGroupItemMapController().FetchByQuery(qr).FirstOrDefault();

                        if (col != null)
                        {
                            col.RetailPrice = _ps.RetailPrice.GetValueOrDefault(0m);
                            col.Deleted = _ps.Deleted.GetValueOrDefault(false);
                            col.CostPrice = _ps.FactoryPrice.GetValueOrDefault(0m);
                            //col.IsItemDeleted = chkDeleted.Checked;
                            col.Save(username);
                        }
                        else
                        {
                            col = new OutletGroupItemMap();
                            col.OutletName = _ps.OutletName;
                            col.ItemNo = item.ItemNo;
                            col.RetailPrice = _ps.RetailPrice.GetValueOrDefault(0m);
                            col.CostPrice = _ps.FactoryPrice.GetValueOrDefault(0m);
                            col.Deleted = _ps.Deleted.GetValueOrDefault(false);
                            //col.IsItemDeleted = chkDeleted.Checked;
                            col.Save(username);
                        }
                    }

                    #region no need to update
                    //item.IsNonDiscountable = _ps.IsNonDiscountable.GetValueOrDefault(false);

                    //item.IsCommission = _ps.IsCommission.GetValueOrDefault(false);

                    //item.AutoCaptureWeight = _ps.AutoCaptureWeight.GetValueOrDefault(false);

                    //item.NonInventoryProduct = false;

                    //if (_ps.IsService.GetValueOrDefault(false))
                    //{
                    //    item.IsInInventory = false;
                    //    item.IsServiceItem = true;
                    //    item.PointGetAmount = 0;
                    //    item.PointGetMode = Item.PointMode.None;
                    //    item.PointRedeemAmount = 0;
                    //    item.PointRedeemMode = _ps.IsPointRedeemable.GetValueOrDefault(false) ? Item.PointMode.Dollar : Item.PointMode.None;
                    //    item.Userfloat3 = null; /// Course Breakdown Price
                    //}
                    //else if (_ps.IsPoint.GetValueOrDefault(false))
                    //{
                    //    item.IsInInventory = false;
                    //    item.IsServiceItem = false;
                    //    item.PointGetAmount = _ps.PointGet.GetValueOrDefault(0m);
                    //    item.PointGetMode = Item.PointMode.Dollar;
                    //    item.PointRedeemAmount = 0;
                    //    item.PointRedeemMode = Item.PointMode.None;
                    //    item.Userfloat3 = null; /// Course Breakdown Price
                    //}
                    //else if (_ps.IsCourse.GetValueOrDefault(false))
                    //{
                    //    item.IsInInventory = false;
                    //    item.IsServiceItem = false;
                    //    item.PointGetAmount = _ps.TimesGet.GetValueOrDefault(0m);
                    //    item.PointGetMode = Item.PointMode.Times;
                    //    item.PointRedeemAmount = 0;
                    //    item.PointRedeemMode = Item.PointMode.None;
                    //    item.Userfloat3 = _ps.BreakdownPrice.GetValueOrDefault(0m); // Course Breakdown Price
                    //    item.IsOpenPricePackage = _ps.IsOpenPricePackage.GetValueOrDefault(false);
                    //}
                    //else if (_ps.IsOpenPricePackage.GetValueOrDefault(false))
                    //{
                    //    item.IsInInventory = true;
                    //    item.IsServiceItem = true;
                    //    item.PointGetAmount = 0;
                    //    item.PointGetMode = Item.PointMode.None;
                    //    item.PointRedeemAmount = 0;
                    //    item.PointRedeemMode = _ps.IsPointRedeemable.GetValueOrDefault(false) ? Item.PointMode.Dollar : Item.PointMode.None;
                    //    item.Userfloat3 = null; /// Course Breakdown Price
                    //}
                    //else if (_ps.IsNonInventoryProduct.GetValueOrDefault(false))
                    //{
                    //    item.IsInInventory = false;
                    //    item.IsServiceItem = false;
                    //    item.NonInventoryProduct = true;
                    //    item.PointGetAmount = 0;
                    //    item.PointGetMode = Item.PointMode.None;
                    //    item.PointRedeemAmount = 0;
                    //    item.PointRedeemMode = _ps.IsPointRedeemable.GetValueOrDefault(false) ? Item.PointMode.Dollar : Item.PointMode.None;
                    //    item.Userfloat3 = null; /// Course Breakdown Price
                    //    item.DeductedItem = _ps.DeductedItemNo;
                    //    item.DeductConvRate = _ps.DeductConvRate.GetValueOrDefault(0m);
                    //    item.DeductConvType = _ps.DeductConvType.GetValueOrDefault(false);
                    //}
                    //else /// Categorized as Product
                    //{
                    //    item.IsInInventory = true;
                    //    item.IsServiceItem = false;
                    //    item.PointGetAmount = 0;
                    //    item.PointGetMode = Item.PointMode.None;
                    //    item.PointRedeemAmount = 0;
                    //    item.PointRedeemMode = _ps.IsPointRedeemable.GetValueOrDefault(false) ? Item.PointMode.Dollar : Item.PointMode.None;
                    //    item.Userfloat3 = null; /// Course Breakdown Price
                    //}
                    #endregion

                    #region save discount
                    if (DisplayPrice1 && !string.IsNullOrEmpty(P1Name))
                    {
                        if (_ps.ApplicableTo == "Product Master")
                        {
                            if (string.IsNullOrEmpty(_ps.P1))
                            {
                                item.Userfloat6 = null;
                            }
                            else
                            {
                                item.Userfloat6 = decimal.Parse(_ps.P1); // Promotion Prce
                                if (item.Userfloat6 < 0)
                                {
                                    item.Userfloat6 = null;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                decimal? p1 = null;
                                if (!string.IsNullOrEmpty(_ps.P1))
                                {
                                    p1 = decimal.Parse(_ps.P1);

                                    if (p1 < 0)
                                        p1 = null;
                                }

                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                if (_ps.ApplicableTo == "Outlet Group")
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, _ps.OutletGroupID.GetValueOrDefault());
                                    ogl.Load();

                                }
                                else
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, _ps.OutletName);
                                    ogl.Load();
                                }

                                if (ogl.Count > 0)
                                {
                                    OutletGroupItemMap sd = ogl[0];
                                    sd.P1 = p1;
                                    sd.Save();
                                }


                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex.Message);
                                item.Userfloat6 = null;
                            }
                        }
                    }

                    if (DisplayPrice2 && !string.IsNullOrEmpty(P2Name))
                    {
                        if (_ps.ApplicableTo == "Product Master")
                        {
                            if (string.IsNullOrEmpty(_ps.P2))
                            {
                                item.Userfloat7 = null;
                            }
                            else
                            {
                                item.Userfloat7 = decimal.Parse(_ps.P2); // Promotion Prce
                                if (item.Userfloat7 < 0)
                                {
                                    item.Userfloat7 = null;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                decimal? p2 = null;
                                if (!string.IsNullOrEmpty(_ps.P2))
                                {
                                    p2 = decimal.Parse(_ps.P2);

                                    if (p2 < 0)
                                        p2 = null;
                                }

                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                if (_ps.ApplicableTo == "Outlet Group")
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, _ps.OutletGroupID.GetValueOrDefault());
                                    ogl.Load();

                                }
                                else
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, _ps.OutletName);
                                    ogl.Load();
                                }

                                if (ogl.Count > 0)
                                {
                                    OutletGroupItemMap sd = ogl[0];
                                    sd.P2 = p2;
                                    sd.Save();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex.Message);
                                item.Userfloat7 = null;
                            }
                        }
                    }

                    if (DisplayPrice3 && !string.IsNullOrEmpty(P3Name))
                    {
                        if (_ps.ApplicableTo == "Product Master")
                        {
                            if (string.IsNullOrEmpty(_ps.P3))
                            {
                                item.Userfloat8 = null;
                            }
                            else
                            {
                                item.Userfloat8 = decimal.Parse(_ps.P3); // Promotion Prce
                                if (item.Userfloat8 < 0)
                                {
                                    item.Userfloat8 = null;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                decimal? p3 = null;
                                if (!string.IsNullOrEmpty(_ps.P3))
                                {
                                    p3 = decimal.Parse(_ps.P3);

                                    if (p3 < 0)
                                        p3 = null;
                                }

                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                if (_ps.ApplicableTo == "Outlet Group")
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, _ps.OutletGroupID.GetValueOrDefault());
                                    ogl.Load();

                                }
                                else
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, _ps.OutletName);
                                    ogl.Load();
                                }

                                if (ogl.Count > 0)
                                {
                                    OutletGroupItemMap sd = ogl[0];
                                    sd.P3 = p3;
                                    sd.Save();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex.Message);
                                item.Userfloat8 = null;
                            }
                        }
                    }

                    if (DisplayPrice4 && !string.IsNullOrEmpty(P4Name))
                    {
                        if (_ps.ApplicableTo == "Product Master")
                        {
                            if (string.IsNullOrEmpty(_ps.P4))
                            {
                                item.Userfloat9 = null;
                            }
                            else
                            {
                                item.Userfloat9 = decimal.Parse(_ps.P4); // Promotion Prce
                                if (item.Userfloat9 < 0)
                                {
                                    item.Userfloat9 = null;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                decimal? p4 = null;
                                if (!string.IsNullOrEmpty(_ps.P4))
                                {
                                    p4 = decimal.Parse(_ps.P4);

                                    if (p4 < 0)
                                        p4 = null;
                                }

                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                if (_ps.ApplicableTo == "Outlet Group")
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, _ps.OutletGroupID.GetValueOrDefault());
                                    ogl.Load();

                                }
                                else
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, _ps.OutletName);
                                    ogl.Load();
                                }

                                if (ogl.Count > 0)
                                {
                                    OutletGroupItemMap sd = ogl[0];
                                    sd.P4 = p4;
                                    sd.Save();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex.Message);
                                item.Userfloat9 = null;
                            }
                        }
                    }

                    if (DisplayPrice5 && !string.IsNullOrEmpty(P5Name))
                    {
                        if (_ps.ApplicableTo == "Product Master")
                        {
                            if (string.IsNullOrEmpty(_ps.P5))
                            {
                                item.Userfloat10 = null;
                            }
                            else
                            {
                                item.Userfloat10 = decimal.Parse(_ps.P5); // Promotion Prce
                                if (item.Userfloat10 < 0)
                                {
                                    item.Userfloat10 = null;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                decimal? p5 = null;
                                if (!string.IsNullOrEmpty(_ps.P5))
                                {
                                    p5 = decimal.Parse(_ps.P5);

                                    if (p5 < 0)
                                        p5 = null;
                                }

                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                if (_ps.ApplicableTo == "Outlet Group")
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, _ps.OutletGroupID.GetValueOrDefault());
                                    ogl.Load();

                                }
                                else
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, _ps.OutletName);
                                    ogl.Load();
                                }

                                if (ogl.Count > 0)
                                {
                                    OutletGroupItemMap sd = ogl[0];
                                    sd.P5 = p5;
                                    sd.Save();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex.Message);
                                item.Userfloat10 = null;
                            }
                        }
                    }
                    #endregion

                    #region Don't need this section because the parameter is null

                    #region Find GSTRule Index


                    if (_ps.IsNew.GetValueOrDefault(false) && AppSetting.GetSetting(AppSetting.SettingsName.Item.DefaultGSTSetting) != "")
                    {
                        int GSTRuleIndex = 0;

                        String[] GSTRules = new String[] { "", "Exclusive GST", "Inclusive GST", "Non GST" };
                        for (int i = 0; i < GSTRules.Count(); i++)
                        {
                            if (GSTRules[i] == AppSetting.GetSetting(AppSetting.SettingsName.Item.DefaultGSTSetting))
                            {
                                GSTRuleIndex = i;
                                break;
                            }
                        }
                        item.GSTRule = GSTRuleIndex;
                    }

                    #endregion

                    #region obsolete
                    //item.GSTRule = GSTRuleIndex;
                    //item.ItemDesc = _ps.ItemDesc;
                    //item.Attributes1 = _ps.Attribute1;
                    //item.Attributes2 = _ps.Attribute2;
                    //item.Attributes3 = _ps.Attribute3;
                    //item.Attributes4 = _ps.Attribute4;
                    //item.Attributes5 = _ps.Attribute5;
                    //item.Attributes6 = _ps.Attribute6;
                    //item.Attributes7 = _ps.Attribute7;
                    //item.Attributes8 = _ps.Attribute8;
                    ////if (fuItemPicture.HasFile)
                    ////    item.ItemImage = ImageCompressor.ResizeAndCompressImage(new MemoryStream(fuItemPicture.FileBytes));
                    //item.Remark = _ps.Remark;
                    //item.AllowPreOrder = _ps.IsPreOrder.GetValueOrDefault(false);
                    //item.CapQty = _ps.CapQty.GetValueOrDefault(0);

                    //item.IsVendorDelivery = _ps.IsVendorDelivery.GetValueOrDefault(false);
                    //item.IsPAMedifund = _ps.IsPAMedifund.GetValueOrDefault(false);
                    //item.IsSMF = _ps.IsSMF.GetValueOrDefault(false);
                    //item.IsConsignment = _ps.IsConsignment.GetValueOrDefault(false);
                    /*------------------CUSTOM CODE----------------------------
                    if (FileUpload1.HasFile)
                    {
                        ItemController itemCtr = new ItemController();
                        itemCtr.UploadPicture(
                          id, FileUpload1.
                          FileBytes, FileUpload1.FileName.Split('.')[1].ToUpper());
                    }*/

                    #endregion

                    #endregion

                    if (_ps.ApplicableTo == "Outlet" && _ps.IsNew.GetValueOrDefault(false)) // if add item to outlet
                        item.Deleted = true;
                    else
                        if (_ps.ApplicableTo == "Outlet") // Edit outlet item
                            item.Deleted = item.Deleted;
                        else
                            item.Deleted = _ps.Deleted.GetValueOrDefault(false); // normal item
                    if (item.GetSaveCommand(username) != null)
                        cmdCol.Add(item.GetSaveCommand(username));

                    //Save supplier
                    QueryCommand c = null;
                    if (_ps.SupplierID.HasValue)
                        SaveSupplier(item.ItemNo, _ps.SupplierID.GetValueOrDefault(), item.FactoryPrice);

                    if (c != null)
                        cmdCol.Add(c);

                    _ps.IsNew = false;

                    AccessLogController.AddLog(AccessSource.WEB, username, "-", "UPDATE Item " + item.ItemNo, "");
                    DataService.ExecuteTransaction(cmdCol);
                    UpdateLastGeneratedBarcode(username, _ps);

                    #region *) Audit Log
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.AuditLog.ProductSetup), false))
                    {
                        Logger.writeLog("new price : " + item.RetailPrice);
                        Logger.writeLog("old price : " + originalItem.RetailPrice);

                        string operation = originalItem.IsNew ? "INSERT" : "UPDATE";
                        if (item.RetailPrice != originalItem.RetailPrice)
                            AuditLogController.AddLog(operation, "Item", "ItemNo", item.ItemNo, "RetailPrice = " + originalItem.RetailPrice.ToString("N2"), "RetailPrice = " + item.RetailPrice.ToString("N2"), username);
                    }
                    #endregion

                    result.resultItemNo = item.ItemNo;
                    result.resultMatrixMode = "No";
                    isSuccess = true;
                }
                #endregion
            }

            catch (Exception x)
            {
                isSuccess = false;
                result.status = x.Message;
                Logger.writeLog(x);
            }

            return new JavaScriptSerializer().Serialize(result);
        }

        private QueryCommand SaveSupplier(string itemno, int newSupplierID, decimal factoryPrice)
        {
            //QueryCommand qc = new QueryCommand();
            ItemSupplierMapCollection ismCol = new ItemSupplierMapCollection();
            ismCol.Where(ItemSupplierMap.Columns.ItemNo, itemno);
            ismCol.Load();

            if (newSupplierID == -1)
            {
                //if set the supplier to empty and item supplier map no data yet then no need to do anything
                if (ismCol.Count == 0)
                {
                    return null;
                }

                ///if set the supplier to empty and item supplier map exist then update deleted
                if (ismCol.Count > 0)
                {
                    ismCol[0].Deleted = true;
                    return ismCol[0].GetUpdateCommand(Session["UserName"].ToString());
                }
            }
            else
            {
                //if set the supplier have value and item supplier map no data yet then no need to do anything
                if (ismCol.Count > 0)
                {
                    ismCol[0].SupplierID = newSupplierID;
                    ismCol[0].Deleted = false;
                    return ismCol[0].GetUpdateCommand(Session["UserName"].ToString());
                }

                ///if set the supplier to empty and item supplier map exist then update deleted
                if (ismCol.Count == 0)
                {
                    //new ItemSupplierMap
                    ItemSupplierMap ism = new ItemSupplierMap();
                    ism.SupplierID = newSupplierID;
                    ism.ItemNo = itemno;
                    ism.CostPrice = factoryPrice;
                    ism.Deleted = false;
                    return ism.GetInsertCommand(Session["UserName"].ToString());
                }
            }
            return null;
        }

        private void UpdateLastGeneratedBarcode(string username, NewProductParam _ps)
        {
            try
            {
                if (_ps.GenerateBarcode.GetValueOrDefault() && !string.IsNullOrEmpty(_ps.Barcode))
                {
                    string opt = AppSetting.GetSetting(AppSetting.SettingsName.Item.OptionAutoGenerateBarcode);
                    switch (opt)
                    {
                        case "A":
                            {
                                string prefix = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.Item.BarcodePrefix));
                                if (string.IsNullOrEmpty(prefix))
                                    AppSetting.SetSetting(AppSetting.SettingsName.Item.LastBarcodeGenerated, _ps.Barcode);
                                else
                                {
                                    if (_ps.Barcode.StartsWith(prefix))
                                        AppSetting.SetSetting(AppSetting.SettingsName.Item.LastBarcodeGenerated, _ps.Barcode.Replace(prefix, ""));
                                }
                            }
                            break;
                        case "B":
                            {
                                Category ct = new Category(_ps.CategoryName);
                                if (!ct.IsNew)
                                {
                                    string prefix = ct.BarcodePrefix;
                                    if (!string.IsNullOrEmpty(prefix) && _ps.Barcode.StartsWith(prefix))
                                    {
                                        Query qr = Category.CreateQuery();
                                        qr.QueryType = QueryType.Update;
                                        qr.AddWhere(Category.Columns.CategoryName, _ps.CategoryName);
                                        qr.AddUpdateSetting(Category.UserColumns.LastBarcodeGenerated, _ps.Barcode.Replace(prefix, ""));
                                        qr.AddUpdateSetting(Category.Columns.ModifiedBy, username);
                                        qr.AddUpdateSetting(Category.Columns.ModifiedOn, DateTime.Now);
                                        qr.Execute();
                                        AccessLogController.AddLog(AccessSource.WEB, username, "-", string.Format("UPDATE Category : {0}", _ps.CategoryName), "");
                                    }
                                }
                            }
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        [WebMethod]
        public string GetCategoryNames()
        {
            string status = "";
            ArrayList tmps = ItemController.FetchCategoryNames();

            var result = new
            {
                records = tmps.Cast<string>().ToList(),
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SaveItemQuantityTrigger(string username, int inventoryLocationID, string itemNo, string parameters)
        {
            var result = new SaveItemQuantityTriggerResult
            {
                status = "",
                resultTriggerID = 0
            };

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            SaveItemQuantityTriggerParam _ps = new JavaScriptSerializer().Deserialize<SaveItemQuantityTriggerParam>(parameters);

            var coll = new ItemQuantityTriggerCollection();
            coll.Where(ItemQuantityTrigger.Columns.ItemNo, itemNo);
            coll.Where(ItemQuantityTrigger.Columns.InventoryLocationID, inventoryLocationID);
            coll.Load();

            ItemQuantityTrigger tmp = coll.FirstOrDefault();
            if (tmp != null)
            {
                try
                {
                    QueryCommandCollection cmdCol = new QueryCommandCollection();

                    System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                    to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                    using (System.Transactions.TransactionScope transScope =
                    new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                    {
                        ItemQuantityTrigger iqt = new ItemQuantityTrigger(tmp.TriggerID);

                        if (_ps.TriggerQuantity.HasValue)
                            iqt.TriggerQuantity = _ps.TriggerQuantity.Value;
                        if (_ps.TriggerLevel != null)
                            iqt.TriggerLevel = _ps.TriggerLevel;
                        if (_ps.Deleted.HasValue)
                            iqt.Deleted = _ps.Deleted.Value;
                        if (_ps.userfld1 != null)
                            iqt.Userfld1 = _ps.userfld1;
                        if (_ps.userfld2 != null)
                            iqt.Userfld2 = _ps.userfld2;
                        if (_ps.userfld3 != null)
                            iqt.Userfld3 = _ps.userfld3;
                        if (_ps.userfld4 != null)
                            iqt.Userfld4 = _ps.userfld4;
                        if (_ps.userfld5 != null)
                            iqt.Userfld5 = _ps.userfld5;
                        if (_ps.userfld6 != null)
                            iqt.Userfld6 = _ps.userfld6;
                        if (_ps.userfld7 != null)
                            iqt.Userfld7 = _ps.userfld7;
                        if (_ps.userfld8 != null)
                            iqt.Userfld8 = _ps.userfld8;
                        if (_ps.userfld9 != null)
                            iqt.Userfld9 = _ps.userfld9;
                        if (_ps.userfld10 != null)
                            iqt.Userfld10 = _ps.userfld10;

                        cmdCol.Add(iqt.GetSaveCommand(username));

                        AccessLogController.AddLog(AccessSource.WEB, username + "", "-", "UPDATE ItemQuantityTrigger " + iqt.TriggerID, "");
                        DataService.ExecuteTransaction(cmdCol);

                        transScope.Complete();

                        result.resultTriggerID = iqt.TriggerID;
                    }
                }
                catch (Exception x)
                {
                    result.status = x.Message;
                    Logger.writeLog(x);
                }
            }
            else
            {
                try
                {
                    QueryCommandCollection cmdCol = new QueryCommandCollection();

                    System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                    to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                    using (System.Transactions.TransactionScope transScope =
                    new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                    {
                        ItemQuantityTrigger iqt = new ItemQuantityTrigger();

                        iqt.ItemNo = itemNo;
                        iqt.InventoryLocationID = inventoryLocationID;

                        if (_ps.TriggerQuantity.HasValue)
                            iqt.TriggerQuantity = _ps.TriggerQuantity.Value;
                        if (_ps.TriggerLevel != null)
                            iqt.TriggerLevel = _ps.TriggerLevel;
                        if (_ps.Deleted.HasValue)
                            iqt.Deleted = _ps.Deleted.Value;
                        if (_ps.userfld1 != null)
                            iqt.Userfld1 = _ps.userfld1;
                        if (_ps.userfld2 != null)
                            iqt.Userfld2 = _ps.userfld2;
                        if (_ps.userfld3 != null)
                            iqt.Userfld3 = _ps.userfld3;
                        if (_ps.userfld4 != null)
                            iqt.Userfld4 = _ps.userfld4;
                        if (_ps.userfld5 != null)
                            iqt.Userfld5 = _ps.userfld5;
                        if (_ps.userfld6 != null)
                            iqt.Userfld6 = _ps.userfld6;
                        if (_ps.userfld7 != null)
                            iqt.Userfld7 = _ps.userfld7;
                        if (_ps.userfld8 != null)
                            iqt.Userfld8 = _ps.userfld8;
                        if (_ps.userfld9 != null)
                            iqt.Userfld9 = _ps.userfld9;
                        if (_ps.userfld10 != null)
                            iqt.Userfld10 = _ps.userfld10;

                        cmdCol.Add(iqt.GetSaveCommand(username));

                        AccessLogController.AddLog(AccessSource.WEB, username + "", "-", "INSERT ItemQuantityTrigger " + iqt.TriggerID, "");
                        DataService.ExecuteTransaction(cmdCol);

                        transScope.Complete();

                        result.resultTriggerID = iqt.TriggerID;
                    }
                }
                catch (Exception x)
                {
                    result.status = x.Message;
                    Logger.writeLog(x);
                }
            }

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetItemSupplierMapRealTimePaging(DateTime lastModifiedOn, int count, int skip)
        {
            DataSet dt = SynchronizationController.FetchItemSupplierMapRealTime(lastModifiedOn, count, skip);

            return Newtonsoft.Json.JsonConvert.SerializeObject(dt.Tables[0]);
        }

        [WebMethod]
        public string GetItemSupplierMapRecordCountAfterTimestamp(DateTime lastModifiedOn)
        {
            int count = SynchronizationController.GetItemSupplierMapTableRealTimeCount(lastModifiedOn);

            var result = new
            {
                data = count
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetPromoRealTimePaging(DateTime lastModifiedOn, int count, int skip)
        {
            DataSet ds = SynchronizationController.FetchPromoRealTime(lastModifiedOn, count, skip);

            return JsonConvert.SerializeObject(ds, Formatting.Indented);
        }

        [WebMethod]
        public string GetPromoCountAfterTimestamp(DateTime lastModifiedOn)
        {
            DataSet ds = SynchronizationController.FetchPromoRealTime(lastModifiedOn, int.MaxValue, 0);

            var result = new
            {
                data = ds.Tables[0].Rows.Count
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetItemGroupRealTimePaging(DateTime lastModifiedOn, int count, int skip)
        {
            DataSet ds = SynchronizationController.FetchItemGroupRealTime(lastModifiedOn, count, skip);

            return JsonConvert.SerializeObject(ds, Formatting.Indented);
        }

        [WebMethod]
        public string GetItemGroupCountAfterTimestamp(DateTime lastModifiedOn)
        {
            DataSet ds = SynchronizationController.FetchItemGroupRealTime(lastModifiedOn, int.MaxValue, 0);

            var result = new
            {
                data = ds.Tables[0].Rows.Count
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        #region *) Purchase Order To Supplier
        [WebMethod]
        public string PurchaseOrderAndroid(string detail, string username, int InventoryLocationID, int supplierID, string poDate, string remark, string uniqueID)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;

            string status = "";
            string newRefNo = "";

            Logger.writeLog(detail);

            try
            {
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Inventory Location not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = ERR_CLINIC_DELETED;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = ERR_CLINIC_FROZEN;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                IDataReader rdr = PurchaseOrderHdr.CreateQuery().WHERE(PurchaseOrderHdr.UniqueIDColumn.ColumnName, uniqueID).ExecuteReader();
                if (rdr.Read())
                {
                    newRefNo = rdr[PurchaseOrderHdr.PurchaseOrderHdrRefNoColumn.ColumnName].ToString();
                }
                else
                {

                    List<PurchaseOrderDet> tmpDetails = new JavaScriptSerializer().Deserialize<List<PurchaseOrderDet>>(detail);
                    PurchaseOdrController ctrl = new PurchaseOdrController();

                    foreach (var det in tmpDetails)
                    {
                        //check qty
                        decimal qty = det.Quantity.GetValueOrDefault(0);
                        if (!ctrl.AddItemToPurchaseOrderByPackingSize(det.ItemNo, qty, det.PackingSizeName, det.PackingSizeUOM <= 0 ? 1m : det.PackingSizeUOM, det.PackingSizeCost, det.GSTRule, out status))
                        {
                            Logger.writeLog("Save PO failed. Issue on save detail item ");
                            return new JavaScriptSerializer().Serialize(new { status = status });
                        }
                    }

                    DateTime purchaseOrderDate = DateTime.Now;
                    try
                    {
                        purchaseOrderDate = DateTime.ParseExact(poDate, "dd-MM-yyyy HH:mm:ss", provider);
                    }
                    catch (FormatException ex)
                    {
                        Logger.writeLog("Save PO failed. Cannot Parse PO Date ");
                        status = "Save PO failed. Cannot Parse PO Date ";
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                    ctrl.SetPurchaseOrderDate(purchaseOrderDate);
                    ctrl.SetPurchaseOrderHeaderInfo(supplierID.ToString(), remark, 0, 1, 0);
                    ctrl.InvHdr.UniqueID = new Guid(uniqueID);
                    if (!ctrl.CreateOrder(username, InventoryLocationID, out status))
                    {
                        Logger.writeLog("Save PO failed. " + status);
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                    newRefNo = ctrl.InvHdr.PurchaseOrderHdrRefNo;
                }
                var result = new
                {
                    newRefNo = newRefNo,
                    status = status
                };

                return new JavaScriptSerializer().Serialize(result);

            }
            catch (Exception ex)
            {
                Logger.writeLog("Save PO failed. " + ex.Message);
                return new JavaScriptSerializer().Serialize(new { status = status });
            }
        }

        #endregion

        [WebMethod]
        public string GetPurchaseOrder(string no)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;

            PurchaseOdrController ctrl = new PurchaseOdrController();
            string status = "";
            bool result = ctrl.LoadConfirmedPurchaseOrderControllerForStockIn(no, out status);
            if (result)
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AllowLoadUnapprovedPOInGR), false) || ctrl.isApproved())
                {
                    List<POItemDetail> dets = new List<POItemDetail>();

                    DataTable dt = ctrl.InvDetToDataTable();
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var dr = dt.Rows[i];

                            Item it = new Item(dr["ItemNo"] != null ? dr["ItemNo"].ToString() : "");
                            PurchaseOrderDet tmp = new PurchaseOrderDet(dr["PurchaseOrderDetRefNo"] != null ? dr["PurchaseOrderDetRefNo"].ToString() : "");
                            if (it != null && !it.IsNew && tmp != null && !tmp.IsNew)
                            {
                                dets.Add(new POItemDetail
                                {
                                    Id = tmp.PurchaseOrderDetRefNo,
                                    ItemNo = it.ItemNo,
                                    ItemName = it.ItemName,
                                    Qty = Convert.ToDouble(tmp.Quantity),
                                    UOM = it.UOM,
                                    ItemPackingSize = tmp.PackingSizeName,
                                    ItemPackingSizeUOM = Convert.ToDouble(tmp.PackingQuantity),
                                    CostPrice = Convert.ToDouble(tmp.PackingSizeCost),
                                    CostOfGoods = Convert.ToDouble(tmp.FactoryPrice),
                                    RetailPrice = Convert.ToDouble(tmp.RetailPrice),
                                    GSTRule = Convert.ToString(tmp.GSTRule)
                                });
                            }
                        }
                    }

                    return new JavaScriptSerializer().Serialize(new
                    {
                        Id = ctrl.GetPurchaseOrderHdrRefNo(),
                        Date = ctrl.GetPurchaseOrderDate().ToString(),
                        SupplierID = ctrl.getSupplier(),
                        Remark = ctrl.GetRemark(),
                        Status = status,
                        InventoryLocationId = ctrl.GetInventoryLocationID(),
                        Details = dets
                    });
                }
                else
                {
                    status = "Purchase Order is Unapproved. Please approve this purchase order before proceed";
                    return new JavaScriptSerializer().Serialize(new { Status = status });
                }
            }
            else
            {
                return new JavaScriptSerializer().Serialize(new { Status = status });
            }
        }

        #endregion

        #region *) Rating System

        [WebMethod]
        public string GetLastTimestampRating(int posid)
        {
            QueryCommand qc = new QueryCommand("SELECT ISNULL(MAX(UniqueId),'" + posid + "_0') as mx FROM rating WHERE POSID = " + posid);
            object obj = DataService.ExecuteScalar(qc);

            if (obj != null && obj.ToString() != "")
                return obj.ToString();

            return "";
        }

        [WebMethod]
        public bool SyncRating(String data)
        {
            QueryCommandCollection cmd = new QueryCommandCollection();

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            List<RatingClz> ratings = new JavaScriptSerializer().Deserialize<List<RatingClz>>(data);

            try
            {
                foreach (var rating in ratings)
                {
                    var dtTimestamp = DateTime.ParseExact(rating.Timestamp, "dd-MM-yyyy HH:mm:ss", provider);
                    var dtCreatedOn = DateTime.ParseExact(rating.CreatedOn, "dd-MM-yyyy HH:mm:ss", provider);
                    var staff = "";
                    var orderhdrid = "";
                    DateTime latestOrderDate = DateTime.Now;
                    DateTime latestLoginActivity = DateTime.Now;
                    QueryCommand qc = new QueryCommand("select top 1 orderhdrid, cashierID, orderdate from orderhdr where pointofsaleid='" + rating.POSID + "' and orderdate <= CONVERT(DATETIME, '" + dtTimestamp.ToString("dd-MM-yyyy HH:mm:ss") + "', 105) order by orderdate desc");
                    DataSet dsOrderHdr = DataService.GetDataSet(qc);
                    if (dsOrderHdr.Tables[0].Rows.Count > 0)
                    {
                        staff = dsOrderHdr.Tables[0].Rows[0]["cashierID"].ToString();
                        if (!DateTime.TryParse(dsOrderHdr.Tables[0].Rows[0]["orderdate"].ToString(), out latestOrderDate))
                        {
                            latestOrderDate = new DateTime(2016, 1, 1);
                        }
                        orderhdrid = dsOrderHdr.Tables[0].Rows[0]["orderhdrid"].ToString();
                    }

                    //if (obj != null)
                    //    staff = obj.ToString();
                    //Logger.writeLog("OrderHdr : " + orderhdrid);
                    QueryCommand qc1 = new QueryCommand("select top 1 username, logindatetime from LoginActivity where pointofsaleid='" + rating.POSID + "' and logindatetime <= CONVERT(DATETIME, '" + dtTimestamp.ToString("dd-MM-yyyy HH:mm:ss") + "', 105) and lower(loginType) = 'login' order by logindatetime desc");
                    DataSet dsLoginActivity = DataService.GetDataSet(qc1);
                    //if (obj != null)
                    //    staff = obj.ToString();
                    if (dsLoginActivity.Tables[0].Rows.Count > 0)
                    {
                        if (!DateTime.TryParse(dsLoginActivity.Tables[0].Rows[0]["logindatetime"].ToString(), out latestLoginActivity))
                        {
                            latestLoginActivity = new DateTime(2016, 1, 1);
                        }

                        if (latestLoginActivity > latestOrderDate)
                        {
                            staff = dsLoginActivity.Tables[0].Rows[0]["username"].ToString();
                        }
                    }

                    //Check OrderHdrID 
                    if (orderhdrid != "")
                    {
                        QueryCommand qCheckOrderHdrID = new QueryCommand("Select count(*) from rating where OrderhdrID = '" + orderhdrid + "'");
                        object totalOrderHdrID = DataService.ExecuteScalar(qCheckOrderHdrID);
                        if (totalOrderHdrID != null)
                        {
                            int tmp = 0;
                            //Logger.writeLog(totalOrderHdrID.ToString());
                            if (int.TryParse(totalOrderHdrID.ToString(), out tmp))
                            {
                                if (tmp > 0)
                                    orderhdrid = "";
                            }
                        }
                    }
                    cmd.Add(new QueryCommand("INSERT INTO rating (POSID, Rating, Staff, Timestamp, CreatedOn, CreatedBy, UniqueID, OrderHdrID) VALUES (" + rating.POSID + ", " + rating.Rating + ", '" + staff + "', CONVERT(DATETIME, '" + dtTimestamp.ToString("dd-MM-yyyy HH:mm:ss") + "', 105), CONVERT(DATETIME, '" + dtCreatedOn.ToString("dd-MM-yyyy HH:mm:ss") + "', 105), '" + rating.CreatedBy + "', '" + rating.UniqueId + "', '" + orderhdrid + "')"));
                }

                SubSonic.DataService.ExecuteTransaction(cmd);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        [WebMethod]
        public string GetRatingMaster()
        {
            string status = "";
            RatingMasterCollection col = new RatingMasterCollection();
            try
            {
                col.Where(RatingMaster.Columns.Deleted, false);
                col.Load();

            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            var result = new
            {
                status = status,
                data = col
            };

            return new JavaScriptSerializer().Serialize(result);


        }

        [WebMethod]
        public string GetLatestLogger(int POSID)
        {
            string msg = "";
            UserMst user = new UserMst();

            try
            {
                /*string query = @"select top 1  um.* 
                                from orderhdr la 
                                inner join usermst um on la.cashierid = um.username
                                where  la.pointofsaleid = {0}
                                order by orderdate desc";

                string qry = string.Format(query, POSID.ToString());
                DataSet ds = DataService.GetDataSet(new QueryCommand(qry));

                if (ds != null)
                {
                    user = new UserMst(ds.Tables[0].Rows[0][0].ToString());
                }*/
                var staff = "";
                DateTime latestOrderDate = DateTime.Now;
                DateTime latestLoginActivity = DateTime.Now;
                QueryCommand qc = new QueryCommand("select top 1 orderhdrid, cashierID, orderdate from orderhdr where pointofsaleid='" + POSID.ToString() + "' order by orderdate desc");
                DataSet dsOrderHdr = DataService.GetDataSet(qc);
                if (dsOrderHdr.Tables[0].Rows.Count > 0)
                {
                    staff = dsOrderHdr.Tables[0].Rows[0]["cashierID"].ToString();
                    if (!DateTime.TryParse(dsOrderHdr.Tables[0].Rows[0]["orderdate"].ToString(), out latestOrderDate))
                    {
                        latestOrderDate = new DateTime(2016, 1, 1);
                    }
                }

                //if (obj != null)
                //    staff = obj.ToString();

                QueryCommand qc1 = new QueryCommand("select top 1 username, logindatetime from LoginActivity where pointofsaleid='" + POSID.ToString() + "' and lower(loginType) = 'login' order by logindatetime desc");
                DataSet dsLoginActivity = DataService.GetDataSet(qc1);
                if (dsLoginActivity.Tables[0].Rows.Count > 0)
                {
                    if (!DateTime.TryParse(dsLoginActivity.Tables[0].Rows[0]["logindatetime"].ToString(), out latestLoginActivity))
                    {
                        latestLoginActivity = new DateTime(2016, 1, 1);
                    }

                    if (latestLoginActivity > latestOrderDate)
                    {
                        staff = dsLoginActivity.Tables[0].Rows[0]["username"].ToString();
                    }
                }
                if (staff != "")
                {
                    user = new UserMst(staff);
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            var result = new
            {
                status = msg,
                user = user
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        #endregion

        #region *) Record Data

        [WebMethod]
        public String SyncRecordData(string data, string username)
        {
            List<string> tmps = new List<string>();
            List<string> tmps2 = new List<string>();
            string status = "";

            QueryCommandCollection cmd = new QueryCommandCollection();

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            RecordData record = new JavaScriptSerializer().Deserialize<RecordData>(data);

            try
            {
                var dtTimestamp = record.Timestamp;

                int count = 0;

                QueryCommand qc = new QueryCommand("SELECT count(*) AS c FROM RecordData WHERE UniqueId='" + record.RecordDataID + "'");
                object obj = DataService.ExecuteScalar(qc);
                if (obj != null)
                {
                    try
                    {
                        count = Int32.Parse(obj.ToString());
                    }
                    catch (Exception)
                    {
                        count = 0;
                    }
                }

                if (count <= 0)
                {
                    cmd.Add(new QueryCommand("INSERT INTO RecordData (InventoryLocationID, Val1, Val2, Val3, Val4, Val5, Val6, Val7, Val8, Val9, Val10, InventoryHdrRefNo, Timestamp, CreatedOn, CreatedBy, UniqueID) VALUES (" + record.InventoryLocationID + ", '" + record.Val1 + "', '" + record.Val2 + "', '" + record.Val3 + "', '" + record.Val4 + "', '" + record.Val5 + "', '" + record.Val6 + "', '" + record.Val7 + "', '" + record.Val8 + "', '" + record.Val9 + "', '" + record.Val10 + "', '" + record.InventoryHdrRefNo + "', CONVERT(DATETIME, '" + dtTimestamp.ToString("dd-MM-yyyy HH:mm:ss") + "', 105), CONVERT(DATETIME, '" + dtTimestamp.ToString("dd-MM-yyyy HH:mm:ss") + "', 105), '" + username + "', '" + record.RecordDataID + "')"));
                    tmps.Add(record.RecordDataID);
                }
                else
                {
                    tmps2.Add(record.RecordDataID);
                }

                SubSonic.DataService.ExecuteTransaction(cmd);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    status = e.InnerException.Message;

                tmps2.AddRange(tmps);
                tmps.Clear();

                Logger.writeLog(e);
            }

            var result = new
            {
                recorded = tmps,
                notrecorded = tmps2,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        #endregion

        #region Xiao Yuan

        [WebMethod]
        public string SavePurchaseOrderDetailXY(string data, string username)
        {
            PurchaseOrderDetail dataPODet = new JavaScriptSerializer().Deserialize<PurchaseOrderDetail>(data);

            PurchaseOrderController poController = new PurchaseOrderController(dataPODet.PurchaseOrderHeaderRefNo);
            PurchaseOrderDetail poDet = new PurchaseOrderDetail();
            string status = "";
            int stockBalance = 0;
            int optimalStock = 0;
            PurchaseOrderHeader poHdr = poController.GetPOHeader();

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (poHdr.DestInventoryLocationID != 0 && InventoryLocationController.IsDeleted(poHdr.DestInventoryLocationID))
            {
                status = poHdr.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (poHdr.DestInventoryLocationID != 0 && StockTakeController.IsInventoryLocationFrozen(poHdr.DestInventoryLocationID))
            {
                status = poHdr.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            status = ValidatePurchaseOrderDetail(dataPODet, poHdr);
            if (status != "")
                return status;  // No need to serialize it, it has already been serialized

            if (status == "")
            {
                stockBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(dataPODet.ItemNo, poController.GetInventoryLocationID(), DateTime.Now, out status).GetIntValue();
                optimalStock = ItemBaseLevelController.getOptimalStockLevel(dataPODet.ItemNo, poController.GetInventoryLocationID());
                Logger.writeLog("Optimal Stock for " + dataPODet.ItemNo + "-" + poController.GetInventoryLocationID().ToString() + ": " + optimalStock.ToString());
                poController.AddItemIntoPurchaseOrderWithRejectQty(dataPODet.ItemNo.ToUpper(), dataPODet.Quantity ?? 0, dataPODet.RejectQty, dataPODet.ExpiryDate.HasValue ? dataPODet.ExpiryDate.Value.ToString("yyyy-MM-dd") : null, out status, out poDet);
            }

            var result = new
            {
                PurchaseOrderDetail = poDet,
                StockBalance = stockBalance,
                OptimalStock = optimalStock,
                status = status
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string SavePurchaseOrderDetailXYSupplierPortal(string data, string username, int numOfDays, bool isShowSalesQty)
        {
            PurchaseOrderDetail dataPODet = new JavaScriptSerializer().Deserialize<PurchaseOrderDetail>(data);

            PurchaseOrderController poController = new PurchaseOrderController(dataPODet.PurchaseOrderHeaderRefNo);
            PurchaseOrderDetail poDet = new PurchaseOrderDetail();
            string status = "";
            int stockBalance = 0;
            int optimalStock = 0;
            PurchaseOrderHeader poHdr = poController.GetPOHeader();

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (poHdr.DestInventoryLocationID != 0 && InventoryLocationController.IsDeleted(poHdr.DestInventoryLocationID))
            {
                status = poHdr.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (poHdr.DestInventoryLocationID != 0 && StockTakeController.IsInventoryLocationFrozen(poHdr.DestInventoryLocationID))
            {
                status = poHdr.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            status = ValidatePurchaseOrderDetail(dataPODet, poHdr);
            if (status != "")
                return status;  // No need to serialize it, it has already been serialized

            if (status == "")
            {
                stockBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(dataPODet.ItemNo, poController.GetInventoryLocationID(), DateTime.Now, out status).GetIntValue();
                optimalStock = ItemBaseLevelController.getOptimalStockLevel(dataPODet.ItemNo, poController.GetInventoryLocationID());
                Logger.writeLog("Optimal Stock for " + dataPODet.ItemNo + "-" + poController.GetInventoryLocationID().ToString() + ": " + optimalStock.ToString());
                poController.AddItemIntoPurchaseOrderWithRejectQty(dataPODet.ItemNo.ToUpper(), dataPODet.Quantity ?? 0, dataPODet.RejectQty, dataPODet.ExpiryDate.HasValue ? dataPODet.ExpiryDate.Value.ToString("yyyy-MM-dd") : null, out status, out poDet);
            }

            decimal salesPeriod1 = 0;
            decimal salesPeriod2 = 0;
            decimal salesPeriod3 = 0;

            if (isShowSalesQty)
            {
                salesPeriod1 = InventoryController.InventoryFetchItemSales(poDet.ItemNo, poDet.PurchaseOrderHeader.SupplierID.GetValueOrDefault(0), "1",
                    numOfDays.ToString(), poDet.PurchaseOrderHeader.InventoryLocationID.GetValueOrDefault(0), username, out status);
                salesPeriod2 = InventoryController.InventoryFetchItemSales(poDet.ItemNo, poDet.PurchaseOrderHeader.SupplierID.GetValueOrDefault(0), "2",
                    numOfDays.ToString(), poDet.PurchaseOrderHeader.InventoryLocationID.GetValueOrDefault(0), username, out status);
                salesPeriod3 = InventoryController.InventoryFetchItemSales(poDet.ItemNo, poDet.PurchaseOrderHeader.SupplierID.GetValueOrDefault(0), "3",
                    numOfDays.ToString(), poDet.PurchaseOrderHeader.InventoryLocationID.GetValueOrDefault(0), username, out status);
            }

            var result = new
            {
                PurchaseOrderDetail = poDet,
                StockBalance = stockBalance,
                OptimalStock = optimalStock,
                SalesPeriod1 = salesPeriod1,
                SalesPeriod2 = salesPeriod2,
                SalesPeriod3 = salesPeriod3,
                status = status
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public decimal GetInventorySalesWithRange(string itemNo, int supplierID, string numDays, string rangeeOfDay, int inventoryLocationID, string username)
        {
            string status = "";
            return InventoryController.InventoryFetchItemSales(itemNo, supplierID, numDays, rangeeOfDay, inventoryLocationID, username, out status);
        }

        [WebMethod]
        public string LoadItemsFromSalesXY(string PurchaseOrderHeaderRefNo, DateTime startDate, DateTime endDate)
        {

            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
            string status = "";

            poController.SetDateLoadFromSales(startDate, endDate);

            InventoryLocation il = new InventoryLocation(poController.GetInventoryLocationID());
            Outlet o = new Outlet(Outlet.Columns.InventoryLocationID, il.InventoryLocationID);
            PointOfSaleCollection pColl = new PointOfSaleCollection();
            pColl.Where(PointOfSale.Columns.OutletName, o.OutletName);
            pColl.Load();

            string username = "";

            foreach (PointOfSale p in pColl)
            {
                //Create Items 
                DataTable dt = PurchaseOrderController.FetchSalesDetailDatabyDate(startDate, endDate.AddDays(1).AddSeconds(-1), p.PointOfSaleID, poController.GetPOHeader().SupplierID.GetValueOrDefault(0));
                string user = PurchaseOrderController.FetchSalesPersonByDate(startDate, endDate.AddDays(1).AddSeconds(-1), p.PointOfSaleID);

                if (!string.IsNullOrEmpty(user))
                {
                    username = user;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PurchaseOrderDetail AddedItem = new PurchaseOrderDetail();
                        if (poController.GetPOHeader().POType.ToUpper() == "REPLENISH")
                        {
                            if (poController.AddItemIntoPurchaseOrder(dr["ItemNo"].ToString(), Convert.ToDecimal(dr["Quantity"].ToString()),
                                DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss"), dr["OrderDetID"].ToString(), out status, out AddedItem))
                            {
                                //insert into salesorder mapping
                                SalesOrderMappingController.AddSalesOrderMapping(dr["OrderDetID"].ToString(), AddedItem.PurchaseOrderDetailRefNo, Convert.ToDecimal(dr["Quantity"].ToString()));
                                POSController.UpdatePurchaseOrderReference(dr["OrderDetID"].ToString(), AddedItem.PurchaseOrderDetailRefNo);
                            }
                        }
                    }
                }

            }



            if (string.IsNullOrEmpty(poController.getSalesPersonID()) && !string.IsNullOrEmpty(username))
            {
                poController.SetSalesPersonID(username);
            }

            var poDetails = new ArrayList();
            foreach (PurchaseOrderDetail poDet in poController.GetPODetail())
            {
                DateTime paramWHBalDate;
                if (poController.GetPOHeader().Status == "Submitted")
                    paramWHBalDate = DateTime.Now;
                else
                    paramWHBalDate = poDet.ModifiedOn.GetValueOrDefault(DateTime.Now);

                DateTime paramStockBalDate;
                if (poController.GetPOHeader().Status == "Pending")
                    paramStockBalDate = DateTime.Now;
                else
                    paramStockBalDate = poDet.CreatedOn.GetValueOrDefault(DateTime.Now);

                var tmpRow = new
                {
                    PurchaseOrderDetail = poDet,
                    StockBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(poDet.ItemNo, poController.GetInventoryLocationID(), paramStockBalDate, out status),
                    WarehouseBalance = (poController.GetPOHeader().WarehouseID == 0) ? GetWarehouseBalance(poDet.ItemNo, paramWHBalDate, out status) : GetWarehouseBalanceByLocID(poController.GetPOHeader().WarehouseID, poDet.ItemNo, paramWHBalDate, out status),
                    OptimalStock = GetOptimalStock(poDet.ItemNo, (int)poController.GetPOHeader().InventoryLocationID, out status)
                };

                poDetails.Add(tmpRow);
            }

            var result = new
            {
                records = poDetails,
                salesPerson = string.IsNullOrEmpty(poController.getSalesPersonID()) ? null : new UserMst(poController.getSalesPersonID()),
                status = status
            };
            //new JavaScriptSerializer().Serialize(result)

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string SavePurchaseOrderHeaderXY(string data, string username)
        {
            var status = "";
            PurchaseOrderHeader dataPO;
            PurchaseOrderHeader tmpPO = new PurchaseOrderHeader();
            Logger.writeLog(data);
            string BackOrderNo = "";
            try
            {
                dataPO = new JavaScriptSerializer().Deserialize<PurchaseOrderHeader>(data);

                if (dataPO.InventoryLocationID.GetValueOrDefault(0) == 0)
                {
                    status = "Invalid Inventory Location ID";
                }

                if (status == "")
                {
                    if (InventoryLocationController.IsDeleted((int)dataPO.InventoryLocationID))
                    {
                        status = ERR_CLINIC_DELETED;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    if (dataPO.DestInventoryLocationID != 0 && InventoryLocationController.IsDeleted(dataPO.DestInventoryLocationID))
                    {
                        status = dataPO.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_DELETED;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    if (StockTakeController.IsInventoryLocationFrozen((int)dataPO.InventoryLocationID))
                    {
                        status = ERR_CLINIC_FROZEN;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    if (dataPO.DestInventoryLocationID != 0 && StockTakeController.IsInventoryLocationFrozen(dataPO.DestInventoryLocationID))
                    {
                        status = dataPO.DestInventoryLocation.InventoryLocationName + ": " + ERR_CLINIC_FROZEN;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    InventoryLocation il = new InventoryLocation(dataPO.InventoryLocationID);
                    if (string.IsNullOrEmpty(dataPO.PurchaseOrderHeaderRefNo))
                    {
                        dataPO.PurchaseOrderHeaderRefNo = getNewPurchaseOrderRefNo(dataPO.POType, il.InventoryLocationName);
                    }

                    if (dataPO.SupplierID == 0)
                        dataPO.SupplierID = null;

                    if (dataPO.POType.ToUpper() != "BACK ORDER")
                    {
                        if (dataPO.Status == "Submitted")
                        {
                            if (dataPO.POType.ToUpper() != "BACK ORDER")
                            {
                                if (dataPO.PurchaseOrderDetailRecords().Count == 0)
                                {
                                    status = "Please insert at least 1 item.";
                                    return new JavaScriptSerializer().Serialize(new { status = status });
                                }

                                if (dataPO.POType.ToUpper() == "RETURN" || dataPO.POType.ToUpper() == "TRANSFER" || dataPO.POType.StartsWith("ADJUSTMENT"))
                                {
                                    if (string.IsNullOrEmpty(dataPO.InventoryStockOutReason.ReasonName))
                                    {
                                        status = "Please select Reason first.";
                                        return new JavaScriptSerializer().Serialize(new { status = status });
                                    }

                                    if (!string.IsNullOrEmpty(dataPO.InventoryStockOutReason.ReasonName)
                                        && (dataPO.InventoryStockOutReason.ReasonName.ToUpper() == "OTHER" || dataPO.InventoryStockOutReason.ReasonName.ToUpper() == "OTHERS")
                                        && (string.IsNullOrEmpty(dataPO.Remark) || dataPO.Remark.Trim() == ""))
                                    {
                                        status = "Remark is compulsory if Reason is 'Others'.";
                                        return new JavaScriptSerializer().Serialize(new { status = status });
                                    }
                                }

                                if (dataPO.POType.ToUpper() == "TRANSFER")
                                {
                                    if (dataPO.DestInventoryLocation.InventoryLocationID == 0)
                                    {
                                        status = "Please select a valid 'To Clinic'.";
                                        return new JavaScriptSerializer().Serialize(new { status = status });
                                    }

                                    if (dataPO.InventoryLocationID.GetValueOrDefault(0) == dataPO.DestInventoryLocationID)
                                    {
                                        status = "'From Clinic' and 'To Clinic' cannot be the same.";
                                        return new JavaScriptSerializer().Serialize(new { status = status });
                                    }

                                    string priceLevel = dataPO.PriceLevel;
                                    foreach (PurchaseOrderDetail od in dataPO.PurchaseOrderDetailRecords())
                                    {
                                        if (!string.IsNullOrEmpty(priceLevel))
                                        {
                                            Item item = new Item(od.ItemNo);
                                            if (item != null && item.ItemNo == od.ItemNo)
                                            {
                                                if (priceLevel == "P1" && item.P1Price.HasValue)
                                                    od.FactoryPrice = item.P1Price.Value;
                                                else if (priceLevel == "P2" && item.P2Price.HasValue)
                                                    od.FactoryPrice = item.P2Price.Value;
                                                else if (priceLevel == "P3" && item.P3Price.HasValue)
                                                    od.FactoryPrice = item.P3Price.Value;
                                                else if (priceLevel == "P4" && item.P4Price.HasValue)
                                                    od.FactoryPrice = item.P4Price.Value;
                                                else if (priceLevel == "P5" && item.P5Price.HasValue)
                                                    od.FactoryPrice = item.P5Price.Value;
                                            }

                                            od.Save(username);
                                        }
                                        else
                                        {
                                            od.FactoryPrice = 0;
                                            od.Save(username);
                                        }
                                    }
                                }

                                // Re-validate PODetail on submit
                                foreach (PurchaseOrderDetail poDet in dataPO.PurchaseOrderDetailRecords())
                                {
                                    status = ValidatePurchaseOrderDetail(poDet, dataPO);
                                    if (status != "")
                                        return status;  // No need to serialize it, it has already been serialized
                                }
                            }

                        }

                        if (dataPO.Status == "Submitted" || dataPO.Status == "Cancelled")
                            dataPO.PurchaseOrderDate = DateTime.Now;

                        tmpPO = new PurchaseOrderHeader(dataPO.PurchaseOrderHeaderRefNo);

                        if (dataPO.POType.ToUpper() == "TRANSFER" || dataPO.POType.ToUpper() == "BACK ORDER")
                        {
                            if (dataPO.Status == "Cancelled" && tmpPO.Status == "Posted")
                            {
                                status = "Cannot cancel this document because the status is Posted.";
                                return new JavaScriptSerializer().Serialize(new { status = status });
                            }
                        }
                        else
                        {
                            if ((dataPO.Status == "Submitted" || dataPO.Status == "Cancelled") && tmpPO.Status != "Pending")
                            {
                                status = "Cannot change this document's status because it is already " + tmpPO.Status;
                                return new JavaScriptSerializer().Serialize(new { status = status });
                            }
                        }


                        foreach (TableSchema.TableColumn col in tmpPO.GetSchema().Columns)
                        {
                            if (col.ColumnName != "ModifiedOn" && col.ColumnName != "ModifiedBy")
                            {
                                tmpPO.SetColumnValue(col.ColumnName, dataPO.GetColumnValue(col.ColumnName));
                            }
                        }


                        #region *) Get Default Price Level from Inventory Location
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.ShowPriceLevelForWebOrder), false)
                            && string.IsNullOrEmpty(tmpPO.PriceLevel)
                            && tmpPO.Status == "Submitted"
                            && (tmpPO.POType.ToUpper() == "REPLENISH" || tmpPO.POType.ToUpper() == "RETURN")
                            && tmpPO.InventoryLocationID.HasValue)
                        {
                            InventoryLocation iloc = new InventoryLocation(tmpPO.InventoryLocationID.Value);
                            if (iloc != null && iloc.InventoryLocationID == tmpPO.InventoryLocationID.Value)
                            {
                                tmpPO.PriceLevel = iloc.DefaultPriceLevel;

                                foreach (PurchaseOrderDetail od in tmpPO.PurchaseOrderDetailRecords())
                                {
                                    if (!string.IsNullOrEmpty(tmpPO.PriceLevel))
                                    {
                                        Item item = od.Item;
                                        if (item != null && item.ItemNo == od.ItemNo)
                                        {
                                            if (tmpPO.PriceLevel == "P1" && item.P1Price.HasValue)
                                                od.FactoryPrice = item.P1Price.Value;
                                            else if (tmpPO.PriceLevel == "P2" && item.P2Price.HasValue)
                                                od.FactoryPrice = item.P2Price.Value;
                                            else if (tmpPO.PriceLevel == "P3" && item.P3Price.HasValue)
                                                od.FactoryPrice = item.P3Price.Value;
                                            else if (tmpPO.PriceLevel == "P4" && item.P4Price.HasValue)
                                                od.FactoryPrice = item.P4Price.Value;
                                            else if (tmpPO.PriceLevel == "P5" && item.P5Price.HasValue)
                                                od.FactoryPrice = item.P5Price.Value;
                                        }

                                        od.Save(username);
                                    }
                                    else
                                    {
                                        if (tmpPO.POType.ToUpper() == "RETURN")
                                            od.FactoryPrice = od.Item.AvgCostPrice;
                                        else
                                            od.FactoryPrice = 0;
                                        od.Save(username);
                                    }
                                }
                            }
                        }
                        #endregion

                        tmpPO.Save(username);
                    }
                    else
                    {
                        if (dataPO.PurchaseOrderDetailRecords().Count == 0)
                        {
                            status = "Please insert at least 1 item.";
                            return new JavaScriptSerializer().Serialize(new { status = status });
                        }

                        //init Back Order 
                        PurchaseOrderController backOrder = new PurchaseOrderController();

                        //check for existing BackOrder for this inventorylocation
                        PurchaseOrderHeaderCollection bCol = new PurchaseOrderHeaderCollection();
                        bCol.Where(PurchaseOrderHeader.Columns.Userfld1, "Submitted");
                        bCol.Where(PurchaseOrderHeader.Columns.Userfld2, "Back Order");
                        bCol.Where(PurchaseOrderHeader.Columns.InventoryLocationID, dataPO.InventoryLocationID);
                        bCol.Load();

                        if (bCol.Count > 0)
                        {
                            //if there is existing back order then save the back order into the new one.. 
                            backOrder = new PurchaseOrderController(bCol[0].PurchaseOrderHeaderRefNo);
                            foreach (PurchaseOrderDetail poDet in dataPO.PurchaseOrderDetailRecords())
                            {
                                status = ValidatePurchaseOrderDetail(poDet, dataPO);
                                if (status != "")
                                    return status;  // No need to serialize it, it has already been serialized
                                PurchaseOrderDetail pod = new PurchaseOrderDetail();
                                //Save Item in Existing Backorder 
                                if (!backOrder.AddItemIntoPurchaseOrder(poDet.ItemNo, poDet.Quantity ?? 0, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), poDet.PurchaseOrderDetailRefNo, out status, out pod))
                                {
                                    return status;
                                }

                                //Cancel Existing PO
                                dataPO.Status = "Cancelled";
                                tmpPO = new PurchaseOrderHeader(dataPO.PurchaseOrderHeaderRefNo);
                                foreach (TableSchema.TableColumn col in tmpPO.GetSchema().Columns)
                                {
                                    if (col.ColumnName != "ModifiedOn" && col.ColumnName != "ModifiedBy")
                                    {
                                        tmpPO.SetColumnValue(col.ColumnName, dataPO.GetColumnValue(col.ColumnName));
                                    }
                                }

                                tmpPO.Save(username);
                                BackOrderNo = backOrder.GetPurchaseOrderHeaderRefNo();
                            }
                        }
                        else
                        {
                            if (dataPO.Status == "Submitted")
                            {
                                if (dataPO.PurchaseOrderDetailRecords().Count == 0)
                                {
                                    status = "Please insert at least 1 item.";
                                    return new JavaScriptSerializer().Serialize(new { status = status });
                                }

                            }

                            if (dataPO.Status == "Submitted" || dataPO.Status == "Cancelled")
                                dataPO.PurchaseOrderDate = DateTime.Now;

                            tmpPO = new PurchaseOrderHeader(dataPO.PurchaseOrderHeaderRefNo);

                            if (dataPO.POType.ToUpper() == "TRANSFER" || dataPO.POType.ToUpper() == "BACK ORDER")
                            {
                                if (dataPO.Status == "Cancelled" && tmpPO.Status == "Posted")
                                {
                                    status = "Cannot cancel this document because the status is Posted.";
                                    return new JavaScriptSerializer().Serialize(new { status = status });
                                }
                            }
                            else
                            {
                                if ((dataPO.Status == "Submitted" || dataPO.Status == "Cancelled") && tmpPO.Status != "Pending")
                                {
                                    status = "Cannot change this document's status because it is already " + tmpPO.Status;
                                    return new JavaScriptSerializer().Serialize(new { status = status });
                                }
                            }


                            foreach (TableSchema.TableColumn col in tmpPO.GetSchema().Columns)
                            {
                                if (col.ColumnName != "ModifiedOn" && col.ColumnName != "ModifiedBy")
                                {
                                    tmpPO.SetColumnValue(col.ColumnName, dataPO.GetColumnValue(col.ColumnName));
                                }
                            }

                            tmpPO.Save(username);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                PurchaseOrderHeader = tmpPO,
                backOrderNo = BackOrderNo,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);

        }

        [WebMethod]
        public string PurchaseOrderApprovalAutoApproveXY(string PurchaseOrderHeaderRefNo, string username, string autoStockIn)
        {
            string status = "";
            string BackOrderNumber = "";
            UserInfo.username = username;
            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
            if (PurchaseOrderController.PurchaseOrderApprovalXY(poController, PurchaseOrderHeaderRefNo, autoStockIn.ToUpper() == "Y" ? true : false, out status, out BackOrderNumber))
            {
                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                var result = new
                {
                    PurchaseOrderHeader = poHdr,
                    status = status,
                    backOrderNo = BackOrderNumber
                };

                return new JavaScriptSerializer().Serialize(result);
            }
            else
            {
                if (status.ToLower().Contains("violation"))
                    status = "Unable to do transaction. Please try to resend again.";
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            // Refresh the PurchaseOrderHeader and pass to result

        }

        [WebMethod]
        public string PurchaseOrderApprovalAutoApproveWithOrderFrom(string PurchaseOrderHeaderRefNo, string username, string autoStockIn)
        {
            string status = "";
            string BackOrderNumber = "";
            UserInfo.username = username;
            PurchaseOrderController poController = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
            if (PurchaseOrderController.PurchaseOrderApprovalWithOrderFrom(poController, PurchaseOrderHeaderRefNo, autoStockIn.ToUpper() == "Y" ? true : false, out status, out BackOrderNumber))
            {
                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                var result = new
                {
                    PurchaseOrderHeader = poHdr,
                    status = status,
                    backOrderNo = BackOrderNumber
                };

                return new JavaScriptSerializer().Serialize(result);
            }
            else
            {
                if (status.ToLower().Contains("violation"))
                    status = "Unable to do transaction. Please try to resend again.";
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            // Refresh the PurchaseOrderHeader and pass to result

        }

        [WebMethod]
        public string ChangePODetailQtyXY(string data)
        {
            PurchaseOrderDetail dataPODet = new JavaScriptSerializer().Deserialize<PurchaseOrderDetail>(data);

            PurchaseOrderController poController = new PurchaseOrderController(dataPODet.PurchaseOrderHeaderRefNo);
            string status = "";

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            PurchaseOrderHeader poHdr = poController.GetPOHeader();
            if (poHdr.Status != "Pending")
            {
                status = "Cannot make the changes because this document is already " + poHdr.Status;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            poController.ChangeItemQtyWithRejectQty(dataPODet.PurchaseOrderDetailRefNo, dataPODet.Quantity ?? 0, dataPODet.RejectQty, dataPODet.OriginalQuantity, out status);

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string UpdateSalesPersonPurchaseOrder(string PurchaseOrderHeaderRefNo, string username)
        {
            string status = "";
            UserMst user = null;
            if (!String.IsNullOrEmpty(username))
                user = new UserMst(username);

            try
            {
                PurchaseOrderHeader head = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);

                if (head != null)
                {
                    if (!String.IsNullOrEmpty(username))
                    {
                        head.SalesPersonID = username;
                        head.Save();
                    }
                    else
                    {
                        head.SalesPersonID = "";
                        head.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            var result = new
            {
                status = status,
                salesPerson = user
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangeApprovedPOApprovedQty(string data)
        {
            PurchaseOrderDetail dataPODet = new JavaScriptSerializer().Deserialize<PurchaseOrderDetail>(data);

            PurchaseOrderController poController = new PurchaseOrderController(dataPODet.PurchaseOrderHeaderRefNo);
            string status = "";

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            PurchaseOrderHeader poHdr = poController.GetPOHeader();
            if (poHdr.Status != "Approved")
            {
                status = "Cannot make the changes because this document is not approved yet";
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            //poController.ChangeItemQtyApproved(dataPODet.PurchaseOrderDetailRefNo, dataPODet.QtyApproved, out status);
            //poController.UpdateDetailStatus(dataPODet.PurchaseOrderDetailRefNo, (decimal)dataPODet.QtyApproved, dataPODet.Remark, dataPODet.Status, out status);
            poController.ChangeApprovedPOApprovedQty(dataPODet.PurchaseOrderDetailRefNo, (decimal)dataPODet.QtyApproved, out status);

            var result = new
            {
                status = status,
                detail = new PurchaseOrderDetail(dataPODet.PurchaseOrderDetailRefNo)
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string ChangeApprovedPOApprovedQtyWithOrderFrom(string data)
        {
            PurchaseOrderDetail dataPODet = new JavaScriptSerializer().Deserialize<PurchaseOrderDetail>(data);

            PurchaseOrderController poController = new PurchaseOrderController(dataPODet.PurchaseOrderHeaderRefNo);
            string status = "";

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            PurchaseOrderHeader poHdr = poController.GetPOHeader();
            if (poHdr.Status != "Approved")
            {
                status = "Cannot make the changes because this document is not approved yet";
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            //poController.ChangeItemQtyApproved(dataPODet.PurchaseOrderDetailRefNo, dataPODet.QtyApproved, out status);
            //poController.UpdateDetailStatus(dataPODet.PurchaseOrderDetailRefNo, (decimal)dataPODet.QtyApproved, dataPODet.Remark, dataPODet.Status, out status);
            poController.ChangeApprovedPOApprovedQtyWithOrderFrom(dataPODet.PurchaseOrderDetailRefNo, (decimal)dataPODet.QtyApproved, out status);

            var result = new
            {
                status = status,
                detail = new PurchaseOrderDetail(dataPODet.PurchaseOrderDetailRefNo)
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string ChangeSerialNo(string data)
        {
            PurchaseOrderDetail dataPODet = new JavaScriptSerializer().Deserialize<PurchaseOrderDetail>(data);

            PurchaseOrderController poController = new PurchaseOrderController(dataPODet.PurchaseOrderHeaderRefNo);
            string status = "";

            if (InventoryLocationController.IsDeleted(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_DELETED;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (StockTakeController.IsInventoryLocationFrozen(poController.GetInventoryLocationID()))
            {
                status = ERR_CLINIC_FROZEN;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            if (string.IsNullOrEmpty(dataPODet.SerialNo))
            {
                var resultEmpty = new
                {
                    status = "",
                    detail = new PurchaseOrderDetail(dataPODet.PurchaseOrderDetailRefNo)
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(resultEmpty);
            }
            List<string> _serialNo = dataPODet.SerialNo.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (_serialNo.Count != dataPODet.QtyApproved)
            {
                status = string.Format("Serial No count ({0}) did not tally with entered qty ({1}). Do you want to override entered qty?", _serialNo.Count, dataPODet.QtyApproved);
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            //poController.ChangeItemQtyApproved(dataPODet.PurchaseOrderDetailRefNo, dataPODet.QtyApproved, out status);
            //poController.UpdateDetailStatus(dataPODet.PurchaseOrderDetailRefNo, (decimal)dataPODet.QtyApproved, dataPODet.Remark, dataPODet.Status, out status);
            poController.ChangeSerialNo(dataPODet.PurchaseOrderDetailRefNo, dataPODet.SerialNo, out status);

            var result = new
            {
                status = status,
                detail = new PurchaseOrderDetail(dataPODet.PurchaseOrderDetailRefNo)
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string GetItemList(string filter)
        {
            string status = "";
            List<ItemTemp> itemList = new List<ItemTemp>();
            int totalRecords = 0;

            try
            {
                var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);

                string query = "Select ItemNo, ItemName, CategoryName from Item where ISNULL(Deleted,0) = 0 ";


                if (param.ContainsKey("itemName"))
                {
                    if (param["itemName"].ToUpper() != "ALL" && param["itemName"] != "")
                        query += "and ItemName like '%" + param["itemName"] + "%' ";
                }

                if (param.ContainsKey("categoryName"))
                {
                    if (param["categoryName"].ToUpper() != "ALL" && param["categoryName"] != "")
                        query += "and CategoryName like '%" + param["categoryName"] + "%' ";
                }
                query += "and ISNULL(IsInInventory,0) = 1 ";


                DataSet ds = DataService.GetDataSet(new QueryCommand(query));
                ItemCollection posColl = new ItemCollection();
                posColl.Load(ds.Tables[0]);

                foreach (Item pos in posColl)
                {
                    ItemTemp i = new ItemTemp();
                    i.ItemNo = pos.ItemNo;
                    i.ItemName = pos.ItemName;
                    //i.CategoryName = pos.CategoryName;
                    itemList.Add(i);
                }

                totalRecords = itemList.Count;


            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = itemList,
                totalRecords = totalRecords,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetPriceLevel()
        {
            string status = "";
            List<object> levelList = new List<object>();

            try
            {
                bool DisplayPrice1 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice1), false);
                bool DisplayPrice2 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice2), false);
                bool DisplayPrice3 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice3), false);
                bool DisplayPrice4 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice4), false);
                bool DisplayPrice5 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice5), false);
                string P1Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P1DiscountName);
                string P2Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P2DiscountName);
                string P3Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P3DiscountName);
                string P4Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P4DiscountName);
                string P5Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P5DiscountName);

                if (DisplayPrice1 && !string.IsNullOrEmpty(P1Name))
                    levelList.Add(new { Key = "P1", Value = P1Name });

                if (DisplayPrice2 && !string.IsNullOrEmpty(P2Name))
                    levelList.Add(new { Key = "P2", Value = P2Name });

                if (DisplayPrice3 && !string.IsNullOrEmpty(P3Name))
                    levelList.Add(new { Key = "P3", Value = P3Name });

                if (DisplayPrice4 && !string.IsNullOrEmpty(P4Name))
                    levelList.Add(new { Key = "P4", Value = P4Name });

                if (DisplayPrice5 && !string.IsNullOrEmpty(P5Name))
                    levelList.Add(new { Key = "P5", Value = P5Name });
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = levelList,
                status = status
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
        #endregion


        #region *) Recipe
        [WebMethod]
        public string GetItemListForRecipe(string filter, bool IsInventoryItemOnly, bool IsRecipeHeader)
        {
            string status = "";
            List<Item> itemList = new List<Item>();
            int totalRecords = 0;

            try
            {
                var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);

                ItemCollection itemColl = new ItemCollection();
                string query = "Select * from Item where ISNULL(Deleted,0) = 0 ";


                if (param.ContainsKey("itemName"))
                {
                    if (param["itemName"].ToUpper() != "ALL" && param["itemName"] != "")
                        query += "and ItemName like '%" + param["itemName"] + "%' ";
                }

                if (param.ContainsKey("categoryName"))
                {
                    if (param["categoryName"].ToUpper() != "ALL" && param["categoryName"] != "")
                        query += "and CategoryName like '%" + param["categoryName"] + "%' ";
                }

                if (IsInventoryItemOnly)
                {
                    query += "and ISNULL(IsInInventory,0) = 1 ";
                }

                if (IsRecipeHeader)
                {
                    query += "and ItemNo in (Select itemno from recipeheader) ";
                }

                query += "order by LTRIM(RTRIM(ItemName))";

                DataSet ds = DataService.GetDataSet(new QueryCommand(query));
                itemColl.Load(ds.Tables[0]);

                foreach (Item pos in itemColl)
                {
                    itemList.Add(pos);
                }

                totalRecords = itemList.Count;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = itemList,
                totalRecords = totalRecords,
                status = status
            };
            string retVal = "";
            try
            {
                retVal = JsonConvert.SerializeObject(result);
                //retVal = new JavaScriptSerializer().Serialize(result);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return retVal;
        }

        [WebMethod]
        public string GetRecipeHeaderList(string filter)
        {
            string status = "";
            object itemList = new object();
            int totalRecords = 0;

            try
            {
                var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);

                string sql = @"SELECT   I.ItemNo
		                            ,I.ItemName
                            FROM	RecipeHeader RH
		                            INNER JOIN (
			                            SELECT   I.ItemNo
					                            ,I.ItemName
			                            FROM	Item I
			                            WHERE	ISNULL(I.Deleted,0) = 0
			                            --UNION ALL
			                            --SELECT   ATT.AttributesCode ItemNo
					                    --        ,ATT.AttributesName ItemName
			                            --FROM	Attributes ATT
			                            --WHERE	ISNULL(ATT.Deleted,0) = 0
		                            ) I ON I.ItemNo = RH.ItemNo
                            WHERE	ISNULL(RH.Deleted,0) = 0
                            ORDER BY I.ItemName";
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));

                itemList = (from o in dt.AsEnumerable()
                            select new
                            {
                                ItemNo = o.Field<string>("ItemNo"),
                                ItemName = o.Field<string>("ItemName")
                            }).ToList();
                totalRecords = dt.Rows.Count;
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message;
                Logger.writeLog(ex);
            }

            var result = new
            {
                records = itemList,
                totalRecords = totalRecords,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SearchRecipeHeader(string filter, int skip, int take, string sortBy, bool isAscending, bool isHaveWrongConversion)
        {

            List<SearchResultRecipeHdr> tmpResult = new List<SearchResultRecipeHdr>();
            DataTable dt = new DataTable();

            Logger.writeLog("Start Search Recipe");

            try
            {
                string query = @"
                                    SELECT	 vr.RecipeHeaderID
		                                    ,vr.RecipeHeaderNo
		                                    ,vr.RecipeName
		                                    ,COUNT(vr.RecipeDetailID) AS TotalMaterial
		                                    ,ISNULL(I.RetailPrice, 0) RetailPrice
		                                    ,CASE WHEN SUM(CASE WHEN (vr.ConversionRate IS NULL AND ISNULL(vr.UOM,'') != ISNULL(vr.MaterialUOM,'')) THEN 1 ELSE 0 END) > 0 THEN 1 ELSE 0 END AS IsHaveWrongConv
                                    FROM	ViewRecipe vr 
		                                    LEFT JOIN Item I ON vr.RecipeHeaderNo = I.ItemNo 
                                    {0}   
                                    GROUP BY  vr.RecipeHeaderID
		                                     ,vr.RecipeHeaderNo
		                                     ,vr.RecipeName
		                                     ,ISNULL(I.RetailPrice, 0)   
                                    {1} {2}
                                ";

                string where = "";

                var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);

                if (param.ContainsKey("recipeHeaderName"))
                {
                    if (param["recipeHeaderName"].ToUpper() != "ALL" && param["recipeHeaderName"].ToUpper() != "")
                        where += string.IsNullOrEmpty(where) ? " vr.RecipeName LIKE N'%" + param["recipeHeaderName"] + "%' " : " AND vr.RecipeName LIKE N'%" + param["recipeHeaderName"] + "%' ";
                }

                if (param.ContainsKey("itemName"))
                {
                    if (param["itemName"].ToUpper() != "ALL" && param["itemName"].ToUpper() != "")
                        where += string.IsNullOrEmpty(where) ? " vr.ItemName LIKE N'%" + param["itemName"] + "%' " : " AND vr.ItemName LIKE N'%" + param["itemName"] + "%' ";
                }


                string having = "";
                if (isHaveWrongConversion)
                    having += " HAVING CASE WHEN SUM(CASE WHEN (vr.ConversionRate IS NULL AND ISNULL(vr.UOM,'') != ISNULL(vr.MaterialUOM,'')) THEN 1 ELSE 0 END) > 0 THEN 1 ELSE 0 END = 1 ";

                where = string.IsNullOrEmpty(where) ? "" : "WHERE " + where;
                string orderby = string.Format("ORDER BY {0} {1}", sortBy, isAscending ? "ASC" : "DESC");

                query = string.Format(query, where, having, orderby);

                Logger.writeLog(query);

                dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

                Logger.writeLog("Get View Recipe Done");

                for (int i = skip; i < skip + take && i < dt.Rows.Count; i++)
                {
                    tmpResult.Add(new SearchResultRecipeHdr()
                    {
                        RecipeHeaderID = dt.Rows[i]["RecipeHeaderID"].ToString(),
                        RecipeHeaderNo = dt.Rows[i]["RecipeHeaderNo"].ToString(),
                        RecipeName = dt.Rows[i]["RecipeName"].ToString(),
                        TotalMaterial = dt.Rows[i]["TotalMaterial"].ToString().GetIntValue(),
                        IsHaveWrongConv = dt.Rows[i]["IsHaveWrongConv"].ToString().GetIntValue(),
                        RetailPrice = dt.Rows[i]["RetailPrice"].ToString().GetDecimalValue(),
                        TotalMaterialCost = RecipeController.GetTotalCostRecipeHeader(dt.Rows[i]["RecipeHeaderID"].ToString())
                    });
                }

                Logger.writeLog("Search Recipe Done");
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error search recipe :" + ex.Message);
            }

            var result = new
            {
                records = tmpResult,
                totalRecords = dt.Rows.Count
            };
            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SaveRecipeWithConvRate(string data, string username)
        {
            string status = "";
            //Logger.writeLog("Save Recipe : " + data);
            bool IsError = false;
            RecipeHeaderWithConvRateObj tmpDetails = new JavaScriptSerializer().Deserialize<RecipeHeaderWithConvRateObj>(data);

            RecipeHeader newRecipe = new RecipeHeader();
            try
            {
                if (tmpDetails != null && tmpDetails.ItemNo != "")
                {
                    #region *) Validation

                    var rdList = (from o in tmpDetails.Details
                                  where o.Qty < 0
                                  select new RecipeDetail
                                  {
                                      ItemNo = o.ItemNo,
                                      Qty = o.Qty,
                                      Uom = o.Uom
                                  }).ToList();

                    if (rdList.Count > 0)
                    {
                        if (tmpDetails.Type == "Attribute")
                        {
                            string errorStatus = "";
                            if (!RecipeController.ValidateRecipeSetup(tmpDetails.ItemNo, rdList, out errorStatus))
                            {
                                //throw new Exception(errorStatus);
                                status = errorStatus;
                            }
                        }
                        else
                        {
                            //throw new Exception("Negative quantity only valid for attribute item");
                            status = "Negative quantity only valid for attribute ite";
                        }
                    }

                    #endregion

                    System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                    to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                    using (System.Transactions.TransactionScope transScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                    {
                        RecipeHeader rhead = null;
                        //save header
                        if (tmpDetails.RecipeHeaderID != null && tmpDetails.RecipeHeaderID != "")
                        {
                            /*edit mode*/
                            rhead = new RecipeHeader(tmpDetails.RecipeHeaderID);
                        }
                        else
                        {
                            /*new*/
                            rhead = new RecipeHeader();
                            rhead.RecipeHeaderID = RecipeController.GetNewRecipeHeaderID();
                        }

                        /*check duplicate item no*/
                        RecipeHeaderCollection col = new RecipeHeaderCollection();
                        col.Where(RecipeHeader.Columns.ItemNo, Comparison.Equals, tmpDetails.ItemNo);
                        col.Where(RecipeHeader.Columns.RecipeHeaderID, Comparison.NotEquals, rhead.RecipeHeaderID);
                        col.Where(RecipeHeader.Columns.Deleted, Comparison.Equals, false);
                        col.Load();

                        if (col.Count > 0)
                        {
                            throw new Exception("Duplicate Item No");
                        }

                        rhead.RecipeName = tmpDetails.RecipeName;
                        rhead.ItemNo = tmpDetails.ItemNo;
                        rhead.Type = tmpDetails.Type;
                        rhead.Deleted = false;
                        rhead.Save(username);


                        string strquery = "update RecipeDetail Set Deleted = 1, modifiedon = GETDATE() where RecipeHeaderID = '" + rhead.RecipeHeaderID + "'";
                        DataService.ExecuteQuery(new QueryCommand(strquery));

                        foreach (RecipeDetailWithConvRateObj det in tmpDetails.Details)
                        {
                            var detcol = new RecipeDetailCollection();
                            string query = "Select * from RecipeDetail where RecipeHeaderID = '" + rhead.RecipeHeaderID + "' and ItemNo ='" + det.ItemNo + "'";
                            DataSet ds = DataService.GetDataSet(new QueryCommand(query));

                            RecipeDetail detail = null;

                            detcol.Load(ds.Tables[0]);

                            if (detcol.Count > 0)
                            {
                                detail = detcol[0];
                            }
                            else
                            {
                                detail = new RecipeDetail();
                                detail.RecipeHeaderID = rhead.RecipeHeaderID;
                                detail.RecipeDetailID = RecipeController.GetNewRecipeDetailID(rhead.RecipeHeaderID, 0);
                            }

                            detail.ItemNo = det.ItemNo;
                            detail.Qty = det.Qty;
                            detail.IsPacking = det.IsPacking;
                            detail.Uom = det.Uom;
                            detail.Deleted = false;

                            //Check UOM Conversion
                            if (!string.IsNullOrEmpty(det.Uom) && !string.IsNullOrEmpty(det.MaterialUOM)
                                && det.Uom != det.MaterialUOM
                                && det.ConversionRate > 0)
                            {

                                int ConversionHdrID = 0;
                                UOMConversionHdrCollection headcol = new UOMConversionHdrCollection();
                                headcol.Where(UOMConversionHdr.Columns.ItemNo, det.ItemNo);
                                headcol.Where(UOMConversionHdr.Columns.Deleted, false);
                                headcol.Load();

                                if (headcol.Count() == 0)
                                {
                                    UOMConversionHdr header = new UOMConversionHdr();
                                    header.Deleted = false;
                                    header.ItemNo = det.ItemNo;
                                    header.Save(username);

                                    ConversionHdrID = header.ConversionHdrID;
                                }
                                else
                                {
                                    ConversionHdrID = headcol[0].ConversionHdrID;
                                }

                                UOMConversionDetCollection detuom = new UOMConversionDetCollection();
                                detuom.Where(UOMConversionDet.Columns.ConversionHdrID, ConversionHdrID);
                                detuom.Where(UOMConversionDet.Columns.FromUOM, det.MaterialUOM);
                                detuom.Where(UOMConversionDet.Columns.ToUOM, det.Uom);
                                detuom.Where(UOMConversionDet.Columns.Deleted, false);
                                detuom.Load();

                                UOMConversionDet uod;

                                if (detuom.Count() == 0)
                                {
                                    uod = new UOMConversionDet();
                                    uod.ConversionHdrID = ConversionHdrID;
                                    uod.FromUOM = det.MaterialUOM;
                                    uod.ToUOM = det.Uom;
                                    uod.Remark = String.Format("{0}{1}/{2}", det.ConversionRate.ToString(), det.Uom, det.MaterialUOM);
                                    uod.ConversionRate = det.ConversionRate ?? 0;
                                    uod.Deleted = false;
                                    uod.Save(username);

                                }
                                else
                                {
                                    uod = detuom[0];
                                    decimal convrate = det.ConversionRate ?? 0;
                                    if (uod.ToUOM != det.Uom || uod.ConversionRate != convrate)
                                    {
                                        uod.ToUOM = det.Uom;
                                        uod.Remark = String.Format("{0}{1}/{2}", det.ConversionRate.ToString(), det.Uom, det.MaterialUOM);
                                        uod.ConversionRate = det.ConversionRate ?? 0;
                                        uod.Deleted = false;
                                        uod.Save(username);
                                    }
                                }
                            }

                            detail.Save(username);
                        }

                        string cyclicItemNo = "";
                        if (RecipeController.IsCylicRecipe(rhead.ItemNo, out cyclicItemNo))
                            throw new Exception("Cyclic Recipe Found : " + cyclicItemNo);

                        transScope.Complete();
                    }
                }
                else
                {
                    throw new Exception("Data is empty");
                }
            }
            catch (Exception ex)
            {
                IsError = true;
                status = "Error: " + ex.Message;
            }


            var result = new
            {
                error = IsError,
                status = status
            };


            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetRecipeHeader(string recipeHeaderID)
        {
            string status = "";
            //RecipeHeader objReturn = new RecipeHeader();
            RecipeHeader recipe = new RecipeHeader(recipeHeaderID);
            if (recipe == null || recipe.RecipeHeaderID != recipeHeaderID || recipe.Deleted.GetValueOrDefault(false) == true)
            {
                status = "Item not found.";
            }

            //Item item = new Item(recipe.ItemNo);

            //objReturn.RecipeHeaderID = recipe.RecipeHeaderID;
            //objReturn.Deleted = recipe.Deleted;
            //objReturn.ItemNo = recipe.ItemNo;
            //objReturn.Type = recipe.Type;

            //if (item != null && !item.IsNew)
            //    objReturn.RecipeName = item.ItemName;
            //else
            //{
            //    PowerPOS.Attribute att = new PowerPOS.Attribute(recipe.ItemNo);
            //    if (!att.IsNew)
            //    {
            //        objReturn.RecipeName = att.AttributesName;
            //        //objReturn.Userfld1 = "Attribute";
            //    }
            //}

            var obj = new
            {
                RecipeHeaderID = recipe.RecipeHeaderID,
                ItemNo = recipe.ItemNo,
                RecipeName = recipe.RecipeName,
                Type = recipe.Type,
                Deleted = recipe.Deleted.GetValueOrDefault(false),
            };

            var result = new
            {
                recipe = obj,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetRecipeDetail(string recipeHeaderID)
        {
            string status = "";

            List<RecipeDetailWithConvRateObj> detail = new List<RecipeDetailWithConvRateObj>();
            try
            {
                ViewRecipeCollection col = new ViewRecipeCollection();
                col.Where(ViewRecipe.Columns.RecipeHeaderID, recipeHeaderID);
                col.Load();

                for (int i = 0; i < col.Count; i++)
                {
                    detail.Add(new RecipeDetailWithConvRateObj()
                    {
                        RecipeDetailID = col[i].RecipeDetailID,
                        RecipeHeaderNo = col[i].RecipeHeaderNo,
                        RecipeName = col[i].RecipeName,
                        ItemNo = col[i].ItemNo,
                        ItemName = col[i].ItemName,
                        Qty = col[i].Qty,
                        Uom = col[i].Uom,
                        MaterialUOM = col[i].MaterialUOM,
                        IsPacking = col[i].IsPacking,
                        ConversionRate = col[i].ConversionRate,
                        CostRate = Math.Round(RecipeController.GetTotalCostItemNo(col[i].ItemNo, 0), 4)
                    });
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            var result = new
            {
                records = detail,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string DeleteRecipeHeader(string recipeHeaderID, string username)
        {
            string status = "";
            try
            {
                RecipeHeader recipe = new RecipeHeader(recipeHeaderID);
                if (recipe == null || recipe.RecipeHeaderID != recipeHeaderID || recipe.Deleted.GetValueOrDefault(false) == true)
                {
                    throw new Exception("Item not found.");
                }
                else
                {
                    recipe.Deleted = true;
                    recipe.Save(username);
                }
            }
            catch (Exception ex)
            {
                status = "Error: " + ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetUOMListByItemNo(string itemNo)
        {
            string status = "";
            List<string> uomList = new List<string>();

            try
            {
                uomList = ItemController.GetUOMListByItemNo(itemNo);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                records = uomList,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SearchRecipeMainItem(string name, bool showProductOnly, int skip, int take)
        {
            object result = new object();

            try
            {
                string sql = @"
                                DECLARE @Search NVARCHAR(500);
                                DECLARE @ShowProductOnly BIT;

                                SET @Search = '{0}';
                                SET @ShowProductOnly = {1};

                                SELECT * 
                                FROM (
                                    SELECT   I.ItemNo
		                                    ,'Product' Type
		                                    ,I.ItemName
                                            ,ISNULL(I.Userfld1,'') Uom
                                            ,ISNULL(I.FactoryPrice,0) FactoryPrice
                                    FROM	Item I
                                    WHERE	ISNULL(I.Deleted,0) = 0 and I.CategoryName <> 'SYSTEM'
                                ) TAB
                                WHERE	TAB.ItemNo LIKE '%'+@Search+'%'
		                                OR TAB.ItemName LIKE '%'+@Search+'%'
                                ORDER BY TAB.Type, TAB.ItemName";
                sql = string.Format(sql, name, showProductOnly ? "1" : "0");

                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));

                var tmpResult = (from o in dt.AsEnumerable()
                                 select new
                                 {
                                     ItemNo = o.Field<string>("ItemNo"),
                                     Type = o.Field<string>("Type"),
                                     ItemName = o.Field<string>("ItemName"),
                                     Uom = o.Field<string>("Uom"),
                                     FactoryPrice = o.Field<decimal>("FactoryPrice")
                                 }).ToList();

                if (skip > 0) tmpResult = tmpResult.Skip(skip).ToList();
                if (take > 0) tmpResult = tmpResult.Take(take).ToList();

                result = new
                {
                    records = tmpResult,
                    totalRecords = dt.Rows.Count
                };

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetCostRateByItemNo(string itemNo)
        {
            decimal cost = 0;
            string status = "";
            try
            {
                cost = RecipeController.GetTotalCostItemNo(itemNo, 0);
            }
            catch (Exception ex)
            {
                Logger.writeLog("error get cost rate:" + ex.Message);
                status = ex.Message;
            }

            var result = new
            {
                record = cost,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetConversionRate(string itemno, string fromUOM, string toUOM)
        {
            string status = "";
            decimal convertrate = 0;
            try
            {
                //                string query = @"
                //                select ISNULL(cd.ConversionRate, 0) as ConversionRate 
                //                from UOMConversionHdr ch inner join
                //                UOMConversionDet cd on ch.ConversionHdrID = cd.ConversionHdrID 
                //                where  ch.ItemNo ='{0}' and LOWER(cd.FromUOM) = '{1}' and LOWER(cd.ToUOM) = '{2}' 
                //	                and  ISNULL(cd.Deleted,0) = 0 and ISNULL(ch.Deleted,0) = 0
                //            ";

                //                query = string.Format(query, itemno, fromUOM, toUOM);
                //                convertrate = DataService.ExecuteScalar(new QueryCommand(query)).ToString().GetDecimalValue();

                convertrate = ItemController.GetConversionRate(itemno, fromUOM, toUOM);
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }


            var result = new
            {
                record = convertrate,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetUOMList()
        {
            string status = "";
            List<string> uomList = new List<string>();
            IDataReader rdr = null;

            try
            {
                #region *) SQL String
                string sql = @"
                            SELECT DISTINCT(LTRIM(RTRIM(Userfld1))) FROM Item WHERE ISNULL(Userfld1, '') <> ''
                            UNION
                            SELECT DISTINCT(LTRIM(RTRIM(FromUOM))) FROM UOMConversionDet
                            UNION
                            SELECT DISTINCT(LTRIM(RTRIM(ToUOM))) FROM UOMConversionDet
                         ";
                #endregion
                rdr = new InlineQuery().ExecuteReader(sql);
                while (rdr.Read())
                {
                    uomList.Add(rdr[0].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }
            finally
            {
                if (rdr != null)
                {
                    if (!rdr.IsClosed) rdr.Close();
                    rdr.Dispose();
                }
            }

            var result = new
            {
                records = uomList,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetUOMConversionList(string filter, int skip, int take, string sortBy, bool isAscending)
        {
            string status = "";
            List<UOMConversionObj> list = new List<UOMConversionObj>();
            int totalRecords = 0;
            IDataReader rdr = null;

            try
            {
                #region *) SQL string
                string sql = @"
                            SELECT det.ConversionDetID, hdr.ItemNo, itm.ItemName, itm.CategoryName, det.FromUOM, det.ToUOM, det.ConversionRate, det.Remark 
                            FROM UOMConversionHdr hdr
                                INNER JOIN UOMConversionDet det ON det.ConversionHdrID = hdr.ConversionHdrID
                                INNER JOIN Item itm ON itm.ItemNo = hdr.ItemNo
                            WHERE hdr.Deleted = 0 AND det.Deleted = 0
                         ";
                #endregion

                QueryCommand cmd = new QueryCommand(sql, "PowerPOS");

                var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);
                if (param.ContainsKey("categoryname") && param["categoryname"] != "ALL")
                {
                    sql += @"
                            AND itm.CategoryName = @CategoryName
                        ";
                    cmd.AddParameter("@CategoryName", param["categoryname"], DbType.String);
                }
                if (param.ContainsKey("itemno") && param["itemno"] != "ALL")
                {
                    sql += @"
                            AND hdr.ItemNo = @ItemNo
                        ";
                    cmd.AddParameter("@ItemNo", param["itemno"], DbType.String);
                }
                if (param.ContainsKey("uom") && param["uom"] != "ALL")
                {
                    sql += @"
                            AND (det.FromUOM = @UOM OR det.ToUOM = @UOM)
                        ";
                    cmd.AddParameter("@UOM", param["uom"], DbType.String);
                }

                cmd.CommandSql = sql;
                rdr = DataService.GetReader(cmd);
                while (rdr.Read())
                {
                    list.Add(new UOMConversionObj
                    {
                        ConversionDetID = (int)rdr["ConversionDetID"],
                        ItemNo = rdr["ItemNo"].ToString(),
                        ItemName = rdr["ItemName"].ToString(),
                        CategoryName = rdr["CategoryName"].ToString(),
                        FromUOM = rdr["FromUOM"].ToString(),
                        ToUOM = rdr["ToUOM"].ToString(),
                        ConversionRate = (decimal)rdr["ConversionRate"],
                        Remark = rdr["Remark"].ToString()
                    });
                }

                list = list.Sort(sortBy, ((isAscending) ? "asc" : "desc"));
                totalRecords = list.Count;

                if (skip > 0) list = list.Skip(skip).ToList();
                if (take > 0) list = list.Take(take).ToList();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }
            finally
            {
                if (rdr != null)
                {
                    if (!rdr.IsClosed) rdr.Close();
                    rdr.Dispose();
                }
            }

            var result = new
            {
                records = list,
                totalRecords = totalRecords,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SaveUOMConversion(string data, string username)
        {
            string status = "";
            UOMConversionObj tmp = new UOMConversionObj();

            try
            {
                UOMConversionHdr convHdr;
                UOMConversionDet convDet;

                tmp = new JavaScriptSerializer().Deserialize<UOMConversionObj>(data);
                if (tmp.ConversionDetID == null)
                {
                    // Add new
                    convDet = new UOMConversionDet();
                    convDet.Deleted = false;
                }
                else
                {
                    // Edit
                    convDet = new UOMConversionDet(tmp.ConversionDetID);
                    if (convDet.ConversionDetID != tmp.ConversionDetID)
                    {
                        status = "Conversion detail not found.";
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }
                }

                if (string.IsNullOrEmpty(tmp.ItemNo) || tmp.ItemNo.Trim() == "")
                {
                    status = "Item cannot be empty.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                Item item = new Item(tmp.ItemNo);
                if (string.IsNullOrEmpty(item.ItemNo))
                {
                    status = "Item not found.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (string.IsNullOrEmpty(tmp.FromUOM) || tmp.FromUOM.Trim() == "")
                {
                    status = "'From UOM' cannot be empty.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (string.IsNullOrEmpty(tmp.ToUOM) || tmp.ToUOM.Trim() == "")
                {
                    status = "'To UOM' cannot be empty.";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                convHdr = new UOMConversionHdr("ItemNo", tmp.ItemNo);
                if (convHdr.ItemNo != tmp.ItemNo)
                {
                    convHdr.ItemNo = tmp.ItemNo;
                    convHdr.IsNew = true;
                }
                convHdr.Deleted = false;
                convHdr.Save(username);

                convDet.ConversionHdrID = convHdr.ConversionHdrID;
                convDet.FromUOM = tmp.FromUOM;
                convDet.ToUOM = tmp.ToUOM;
                convDet.ConversionRate = tmp.ConversionRate;
                convDet.Remark = tmp.Remark;
                convDet.Save(username);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                data = tmp,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string DeleteUOMConversion(string data, string username)
        {
            var status = "";

            try
            {
                List<UOMConversionObj> ucList = new JavaScriptSerializer().Deserialize<List<UOMConversionObj>>(data);
                foreach (UOMConversionObj ucObj in ucList)
                {
                    UOMConversionDet.Delete("ConversionDetID", ucObj.ConversionDetID, username);
                }

                #region *) SQL string
                string sql = @"
                            UPDATE UOMConversionHdr 
                            SET Deleted = 1, ModifiedOn = GETDATE(), ModifiedBy = @username 
                            WHERE NOT EXISTS ( 
                                    SELECT * FROM UOMConversionDet 
                                    WHERE UOMConversionDet.ConversionHdrID = UOMConversionHdr.ConversionHdrID 
                                        AND UOMConversionDet.Deleted = 0 
                                  ) 
                         ";
                #endregion
                QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
                cmd.AddParameter("@username", username, DbType.String);
                DataService.ExecuteQuery(cmd);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SearchCookItem(string name, int skip, int take)
        {
            int totalRecord = 0;
            object resultData = new object();

            try
            {
                string sql = @"SELECT   I.ItemNo
		                            ,I.ItemName
		                            ,I.CategoryName
                            FROM	Item I
		                            INNER JOIN Category CTG ON CTG.CategoryName = I.CategoryName
		                            INNER JOIN RecipeHeader RH ON RH.ItemNo = I.ItemNo
                            WHERE	ISNULL(I.Deleted,0) = 0
		                            AND ISNULL(I.IsInInventory,0) = 1
		                            AND ISNULL(RH.Deleted,0) = 0
		                            AND (I.ItemNo LIKE N'%{0}%'
			                            OR I.ItemName LIKE N'%{0}%')
                            ORDER BY I.CategoryName, I.ItemName";
                sql = string.Format(sql, name);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));

                var tmpResult = (from o in dt.AsEnumerable()
                                 select new
                                 {
                                     ItemNo = o.Field<string>("ItemNo"),
                                     ItemName = o.Field<string>("ItemName"),
                                     CategoryName = o.Field<string>("CategoryName")
                                 }).AsEnumerable();
                if (skip > 0) tmpResult = tmpResult.Skip(skip);
                if (take > 0) tmpResult = tmpResult.Take(take);
                resultData = tmpResult.ToList();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            var result = new
            {
                records = resultData,
                totalRecords = totalRecord
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string FetchCookItem(string filter, int skip, int take, string sortBy, bool isAscending)
        {
            var param = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(filter);

            int inventoryLocationID = 0;
            string documentNo = "";
            string status = "";
            DateTime startDate = new DateTime(1990, 1, 1);
            DateTime endDate = new DateTime(2100, 1, 1);


            if (param.ContainsKey("inventoryLocationID"))
                inventoryLocationID = param["inventoryLocationID"].GetIntValue();

            if (param.ContainsKey("documentNo"))
                documentNo = param["documentNo"];

            if (param.ContainsKey("status"))
                status = param["status"];

            if (param.ContainsKey("startdate"))
                startDate = param["startdate"].GetDateTimeValue("yyyy-MM-dd"); ;

            if (param.ContainsKey("enddate"))
                endDate = param["enddate"].GetDateTimeValue("yyyy-MM-dd");



            var dt = ReportController.FetchItemCookReport(startDate, endDate, inventoryLocationID, "ALL", 0, "", documentNo, status, sortBy, isAscending ? "ASC" : "DESC");

            var data = (from o in dt.AsEnumerable()
                        select new
                        {
                            DocumentNo = o.Field<string>("DocumentNo"),
                            ItemCookHistoryID = o.Field<int>("ItemCookHistoryID"),
                            CookDate = o.Field<DateTime>("CookDate"),
                            InventoryLocationName = o.Field<string>("InventoryLocationName"),
                            DepartmentName = o.Field<string>("DepartmentName"),
                            CategoryName = o.Field<string>("CategoryName"),
                            ItemName = o.Field<string>("ItemName"),
                            Quantity = o.Field<decimal>("Quantity"),
                            Status = o.Field<string>("Status"),
                            COG = o.Field<decimal>("COG")
                        }).AsEnumerable();

            int count = data.Count();

            if (skip > 0) data = data.Skip(skip);
            if (take > 0) data = data.Take(take);

            var result = new
            {
                records = data,
                totalRecords = count
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string CookItem(string itemNo, decimal quantity, int pointOfSaleID, int inventoryLocationID, string remarks, string userName)
        {
            string status = "";
            bool isSuccess = ItemCookingController.CookItemHelper(itemNo, quantity, pointOfSaleID, inventoryLocationID, remarks, userName, out status);

            var result = new
            {
                isSuccess = isSuccess,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string CookItemWithDetail(int itemCookHistoryID, int pointOfSaleID, string username)
        {
            string status = "";
            bool isSuccess = ItemCookingController.CookItemWithDetail(itemCookHistoryID, pointOfSaleID, username, out status);

            var result = new
            {
                isSuccess = isSuccess,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string SaveItemCookHistory(string data, string username)
        {
            var status = "";
            ItemCookHistoryObject dataPO;
            ItemCookHistoryObject tmpPO = new ItemCookHistoryObject();
            List<ItemCookDetailObject> detailObj = new List<ItemCookDetailObject>();

            Logger.writeLog("Create Item Cook History :" + data);

            QueryCommandCollection col = new QueryCommandCollection();

            try
            {
                dataPO = new JavaScriptSerializer().Deserialize<ItemCookHistoryObject>(data);
                ItemCookHistory ich = new ItemCookHistory();

                if (dataPO.InventoryLocationID == 0)
                {
                    status = "Invalid Inventory Location ID";
                }

                if (status == "")
                {
                    if (InventoryLocationController.IsDeleted((int)dataPO.InventoryLocationID))
                    {
                        status = ERR_CLINIC_DELETED;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    if (StockTakeController.IsInventoryLocationFrozen((int)dataPO.InventoryLocationID))
                    {
                        status = ERR_CLINIC_FROZEN;
                        return new JavaScriptSerializer().Serialize(new { status = status });
                    }

                    InventoryLocation il = new InventoryLocation(dataPO.InventoryLocationID);
                    if (string.IsNullOrEmpty(dataPO.DocumentNo))
                    {
                        string invCode = !string.IsNullOrEmpty(il.DisplayedName) ? il.DisplayedName : il.InventoryLocationName;
                        string newDocumentNo = ItemCookingController.getNewDocumentNo(invCode);

                        ich.DocumentNo = newDocumentNo;
                        dataPO.DocumentNo = newDocumentNo;
                        ich.Status = "Pending";
                        ich.CookDate = DateTime.Now;
                        ich.UniqueID = new Guid();
                        ich.InventoryLocationID = il.InventoryLocationID;
                        ich.Deleted = false;
                        ich.ItemNo = null;

                        ich.Save(username);

                        dataPO.ItemCookHistoryID = ich.ItemCookHistoryID;

                    }

                    ich.Quantity = dataPO.Quantity;
                    if (!string.IsNullOrEmpty(dataPO.ItemNo) && dataPO.ItemNo != ich.ItemNo)
                    {
                        string query = string.Format("Delete from ItemCookDetail where ItemCookHistoryID = {0}", ich.ItemCookHistoryID);
                        col.Add(new QueryCommand(query));

                        ich.ItemNo = dataPO.ItemNo;
                        dataPO.ItemNo = ich.ItemNo;
                        dataPO.ItemName = ich.Item.ItemName;
                        dataPO.CategoryName = ich.Item.CategoryName;

                        decimal totalCost = 0;
                        RecipeHeader rh = new RecipeHeader(RecipeHeader.Columns.ItemNo, ich.ItemNo);
                        if (rh != null)
                        {

                            Query qr = RecipeDetail.CreateQuery();
                            qr.AddWhere(RecipeDetail.Columns.RecipeHeaderID, rh.RecipeHeaderID);
                            qr.AddWhere(RecipeDetail.Columns.Deleted, false);

                            RecipeDetailCollection rdCol = new RecipeDetailCollection();
                            rdCol.LoadAndCloseReader(DataService.GetReader(qr.BuildCommand()));

                            if (rdCol.Count() > 0)
                            {
                                foreach (var rd in rdCol)
                                {
                                    Item it = new Item(rd.ItemNo);

                                    if (!it.IsNew && it.Deleted.GetValueOrDefault(false) == false)
                                    {
                                        ItemCookDetail icd = new ItemCookDetail();
                                        icd.ItemCookHistoryID = ich.ItemCookHistoryID;
                                        icd.ItemNo = rd.ItemNo;
                                        icd.Uom = rd.Uom;
                                        icd.OriginalQty = rd.Qty;
                                        icd.Qty = 0;
                                        icd.UnitPrice = it.FactoryPrice;
                                        icd.TotalCost = icd.Qty * icd.UnitPrice;
                                        icd.Deleted = false;
                                        icd.IsLoadFromRecipe = true;

                                        col.Add(icd.GetSaveCommand(username));
                                        totalCost += (icd.OriginalQty.GetValueOrDefault(0) * icd.UnitPrice.GetValueOrDefault(0));
                                        Logger.writeLog(string.Format("{0}x{1}={2}", icd.OriginalQty.GetValueOrDefault(0), icd.UnitPrice.GetValueOrDefault(0), (icd.OriginalQty.GetValueOrDefault(0) * icd.UnitPrice.GetValueOrDefault(0)).ToString()));
                                    }
                                }

                            }

                        }

                        ich.COG = totalCost;
                        dataPO.COG = totalCost;
                    }
                    col.Add(ich.GetSaveCommand(username));

                    if (col.Count > 0)
                        DataService.ExecuteTransaction(col);

                    Query qr2 = ItemCookDetail.CreateQuery();
                    qr2.AddWhere(ItemCookDetail.Columns.ItemCookHistoryID, ich.ItemCookHistoryID);
                    qr2.AddWhere(ItemCookDetail.Columns.Deleted, false);
                    qr2.ORDER_BY("ItemCookDetailId", "asc");

                    ItemCookDetailCollection icdCol = new ItemCookDetailCollection();
                    icdCol.LoadAndCloseReader(DataService.GetReader(qr2.BuildCommand()));

                    if (icdCol.Count > 0)
                    {
                        foreach (var icd in icdCol)
                        {
                            Item it = new Item(icd.ItemNo);

                            detailObj.Add(new ItemCookDetailObject()
                            {
                                ItemCookDetailID = icd.ItemCookDetailId,
                                ItemCookHistoryID = icd.ItemCookHistoryID.GetValueOrDefault(0),
                                ItemNo = icd.ItemNo,
                                ItemName = it.ItemName,
                                Qty = icd.Qty.GetValueOrDefault(0),
                                OriginalQty = icd.OriginalQty.GetValueOrDefault(0),
                                UnitPrice = icd.UnitPrice.GetValueOrDefault(0),
                                TotalCost = icd.TotalCost.GetValueOrDefault(0),
                                UOM = icd.Uom,
                                IsLoadFromRecipe = icd.IsLoadFromRecipe
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                ItemCookHistory = dataPO,
                detail = detailObj,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);

        }

        [WebMethod]
        public string GetItemCookHistory(string DocumentNo)
        {
            ItemCookHistory tmpPOHdr = new ItemCookHistory();
            ItemCookHistoryObject ichObj = new ItemCookHistoryObject();
            List<ItemCookDetailObject> detailObj = new List<ItemCookDetailObject>();
            string status = "";

            try
            {
                tmpPOHdr = new ItemCookHistory(ItemCookHistory.UserColumns.DocumentNo, DocumentNo);
                if (!tmpPOHdr.IsLoaded)
                {
                    status = "Not found";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (InventoryLocationController.IsDeleted((int)tmpPOHdr.InventoryLocationID))
                {
                    status = ERR_CLINIC_DELETED;
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                ichObj.DocumentNo = tmpPOHdr.DocumentNo;
                ichObj.Status = tmpPOHdr.Status;
                ichObj.COG = tmpPOHdr.COG;
                ichObj.ItemCookHistoryID = tmpPOHdr.ItemCookHistoryID;
                ichObj.CookDate = tmpPOHdr.CookDate.GetValueOrDefault(DateTime.Now);
                ichObj.ItemName = string.IsNullOrEmpty(tmpPOHdr.ItemNo) ? "" : tmpPOHdr.Item.ItemName;
                ichObj.ItemNo = tmpPOHdr.ItemNo;
                ichObj.Quantity = tmpPOHdr.Quantity.GetValueOrDefault(0);
                ichObj.InventoryLocationID = tmpPOHdr.InventoryLocationID.GetValueOrDefault(0);
                ichObj.InventoryLocationName = tmpPOHdr.InventoryLocationID.GetValueOrDefault(0) != 0 ? tmpPOHdr.InventoryLocation.InventoryLocationName : "";
                ichObj.DepartmentName = string.IsNullOrEmpty(tmpPOHdr.ItemNo) ? "" : tmpPOHdr.Item.Category.ItemDepartment.DepartmentName;
                ichObj.CategoryName = string.IsNullOrEmpty(tmpPOHdr.ItemNo) ? "" : tmpPOHdr.Item.CategoryName;


                Query qr = ItemCookDetail.CreateQuery();
                qr.AddWhere(ItemCookDetail.Columns.ItemCookHistoryID, ichObj.ItemCookHistoryID);
                qr.AddWhere(ItemCookDetail.Columns.Deleted, false);
                qr.ORDER_BY("ItemCookDetailId", "asc");

                ItemCookDetailCollection icdCol = new ItemCookDetailCollection();
                icdCol.LoadAndCloseReader(DataService.GetReader(qr.BuildCommand()));

                if (icdCol.Count > 0)
                {
                    foreach (var icd in icdCol)
                    {
                        Item it = new Item(icd.ItemNo);
                        if (!it.IsNew && it.Deleted.GetValueOrDefault(false) == false)
                        {
                            detailObj.Add(new ItemCookDetailObject()
                            {
                                ItemCookDetailID = icd.ItemCookDetailId,
                                ItemCookHistoryID = icd.ItemCookHistoryID.GetValueOrDefault(0),
                                ItemNo = icd.ItemNo,
                                ItemName = it.ItemName,
                                Qty = icd.Qty.GetValueOrDefault(0),
                                OriginalQty = icd.OriginalQty.GetValueOrDefault(0),
                                UnitPrice = icd.UnitPrice.GetValueOrDefault(0),
                                TotalCost = icd.TotalCost.GetValueOrDefault(0),
                                UOM = icd.Uom,
                                IsLoadFromRecipe = icd.IsLoadFromRecipe
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                ItemCookHistory = ichObj,
                detail = detailObj,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string CancelItemCookHistory(string ItemCookHistoryID, string username)
        {
            ItemCookHistory tmpPOHdr = new ItemCookHistory();
            string status = "";

            try
            {
                tmpPOHdr = new ItemCookHistory(ItemCookHistory.Columns.ItemCookHistoryID, ItemCookHistoryID);
                if (!tmpPOHdr.IsLoaded)
                {
                    status = "Not found";
                    return new JavaScriptSerializer().Serialize(new { status = status });
                }

                if (tmpPOHdr.Status != "Pending")
                {
                    throw new Exception("Only can Cancel for Production with status Pending.");
                }

                tmpPOHdr.Status = "Cancelled";
                tmpPOHdr.Save(username);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string DeleteItemCookDetail(string data, string username)
        {
            var param = new JavaScriptSerializer().Deserialize<ItemCookDetailObject>(data);
            string status = "";
            try
            {
                int ItemCookHistoryID = 0;
                Query qr = ItemCookDetail.CreateQuery();
                qr.AddWhere(ItemCookDetail.Columns.ItemCookDetailId, param.ItemCookDetailID);

                ItemCookDetailCollection odCol = new ItemCookDetailCollection();
                odCol.LoadAndCloseReader(DataService.GetReader(qr.BuildCommand()));

                foreach (ItemCookDetail od in odCol)
                {
                    ItemCookHistoryID = od.ItemCookHistoryID.GetValueOrDefault(0);
                    od.Deleted = true;
                    od.Save(username);
                }

                Query qr2 = ItemCookDetail.CreateQuery();
                qr2.AddWhere(ItemCookDetail.Columns.ItemCookHistoryID, ItemCookHistoryID);

                ItemCookDetailCollection odCol2 = new ItemCookDetailCollection();
                odCol2.LoadAndCloseReader(DataService.GetReader(qr2.BuildCommand()));

                decimal totalCost = 0;
                foreach (ItemCookDetail od2 in odCol2)
                {
                    totalCost += od2.Qty.GetValueOrDefault(0) * od2.UnitPrice.GetValueOrDefault(0);
                }

                ItemCookHistory ich = new ItemCookHistory(ItemCookHistoryID);
                if (!ich.IsNew)
                {
                    ich.COG = ich.Quantity.GetValueOrDefault(0) == 0 ? 0 : totalCost / ich.Quantity.GetValueOrDefault(1);
                    ich.Save(username);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new { status = status };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string AddItemCookDetail(string data, string username)
        {
            ItemCookDetailObject detail = new ItemCookDetailObject();
            string status = "";
            try
            {

                var ich = new JavaScriptSerializer().Deserialize<ItemCookDetailObject>(data);


                ItemCookDetail icd = new ItemCookDetail();
                icd.ItemCookHistoryID = ich.ItemCookHistoryID;
                icd.ItemNo = ich.ItemNo;
                icd.Uom = ich.UOM;
                icd.OriginalQty = ich.OriginalQty;
                icd.Qty = ich.Qty;
                icd.UnitPrice = ich.UnitPrice;
                icd.TotalCost = icd.Qty * icd.UnitPrice;
                icd.Deleted = false;
                icd.IsLoadFromRecipe = false;

                icd.Save(username);

                Item it = new Item(icd.ItemNo);

                detail = new ItemCookDetailObject()
                {
                    ItemCookDetailID = icd.ItemCookDetailId,
                    ItemCookHistoryID = icd.ItemCookHistoryID.GetValueOrDefault(0),
                    ItemNo = icd.ItemNo,
                    ItemName = it.ItemName,
                    Qty = icd.Qty.GetValueOrDefault(0),
                    OriginalQty = icd.OriginalQty.GetValueOrDefault(0),
                    UnitPrice = icd.UnitPrice.GetValueOrDefault(0),
                    TotalCost = icd.TotalCost.GetValueOrDefault(0),
                    UOM = icd.Uom,
                    IsLoadFromRecipe = icd.IsLoadFromRecipe
                };

                Query qr2 = ItemCookDetail.CreateQuery();
                qr2.AddWhere(ItemCookDetail.Columns.ItemCookHistoryID, ich.ItemCookHistoryID);

                ItemCookDetailCollection odCol2 = new ItemCookDetailCollection();
                odCol2.LoadAndCloseReader(DataService.GetReader(qr2.BuildCommand()));

                decimal totalCost = 0;
                foreach (ItemCookDetail od2 in odCol2)
                {
                    totalCost += od2.Qty.GetValueOrDefault(0) * od2.UnitPrice.GetValueOrDefault(0);
                }

                ItemCookHistory ich2 = new ItemCookHistory(ich.ItemCookHistoryID);
                if (!ich2.IsNew)
                {
                    ich2.COG = ich2.Quantity.GetValueOrDefault(0) == 0 ? 0 : totalCost / ich2.Quantity.GetValueOrDefault(1);
                    ich2.Save(username);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                status = status,
                detail = detail
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangeQtyItemCookHistory(int itemCookHistoryID, decimal qty, string username)
        {
            string status = "";
            try
            {
                QueryCommandCollection col = new QueryCommandCollection();
                ItemCookHistory header = new ItemCookHistory(itemCookHistoryID);

                if (header.IsNew)
                    throw new Exception("Item Cook History not exist");

                header.Quantity = qty;

                Query qr = ItemCookDetail.CreateQuery();
                qr.AddWhere(ItemCookDetail.Columns.ItemCookHistoryID, header.ItemCookHistoryID);
                qr.AddWhere(ItemCookDetail.Columns.Deleted, false);

                ItemCookDetailCollection det = new ItemCookDetailCollection();
                det.LoadAndCloseReader(DataService.GetReader(qr.BuildCommand()));

                decimal totalCost = 0;

                if (det.Count > 0)
                {
                    foreach (ItemCookDetail icd in det)
                    {
                        if (icd.IsLoadFromRecipe)
                        {
                            icd.Qty = qty * icd.OriginalQty;
                        }
                        else
                        {
                            icd.Qty = icd.Qty;
                        }
                        icd.TotalCost = icd.Qty * icd.UnitPrice;

                        totalCost += icd.Qty.GetValueOrDefault(0) * icd.UnitPrice.GetValueOrDefault(0);
                        col.Add(icd.GetUpdateCommand(username));
                    }
                }

                header.COG = header.Quantity.GetValueOrDefault(0) == 0 ? 0 : totalCost / header.Quantity.GetValueOrDefault(1);
                col.Add(header.GetSaveCommand(username));

                if (col.Count > 0)
                    DataService.ExecuteTransaction(col);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string ChangeQtyItemCookDetail(int itemCookDetailID, decimal qty, string username)
        {
            string status = "";
            try
            {
                QueryCommandCollection col = new QueryCommandCollection();
                ItemCookDetail detail = new ItemCookDetail(itemCookDetailID);

                if (detail.IsNew)
                    throw new Exception("Item Cook Detail not exist");

                ItemCookHistory header = new ItemCookHistory(detail.ItemCookHistoryID);

                detail.Qty = qty;
                detail.TotalCost = detail.Qty * detail.UnitPrice;
                detail.Save(username);

                Query qr = ItemCookDetail.CreateQuery();
                qr.AddWhere(ItemCookDetail.Columns.ItemCookHistoryID, detail.ItemCookHistoryID);
                qr.AddWhere(ItemCookDetail.Columns.Deleted, false);

                ItemCookDetailCollection det = new ItemCookDetailCollection();
                det.LoadAndCloseReader(DataService.GetReader(qr.BuildCommand()));

                decimal totalCost = 0;

                if (det.Count > 0)
                {
                    foreach (ItemCookDetail icd in det)
                    {
                        totalCost += icd.Qty.GetValueOrDefault(0) * icd.UnitPrice.GetValueOrDefault(0);
                    }
                }

                header.COG = header.Quantity.GetValueOrDefault(0) == 0 ? 0 : totalCost / header.Quantity.GetValueOrDefault(1);
                col.Add(header.GetUpdateCommand(username));

                if (col.Count > 0)
                    DataService.ExecuteTransaction(col);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return new JavaScriptSerializer().Serialize(new { status = status });
            }

            var result = new
            {
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }


        #endregion

        #region *) Mobile Reports

        [WebMethod]
        public string GetReportHourlySales(string outlet, string startDateStr, string endDateStr, bool isShowBeforeGST)
        {
            GetReportDailySalesResult result = new GetReportDailySalesResult
            {
                status = "",
                details = new List<GetReportDailySalesItem>()
            };
            string outletList = "ALL";
            AuthorizationStatus authStat = Authenticate(out outletList);
            if (authStat != AuthorizationStatus.VALID_TOKEN)
            {
                result.status = authStat.ToString().Replace("_", " ");
                return new JavaScriptSerializer().Serialize(result);
            }


            DateTime now = DateTime.Now;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            DateTime.TryParse(startDateStr, out startDate);
            DateTime.TryParse(endDateStr, out endDate);


            string sql = "";
            if (endDate.Year >= now.Year && endDate.Month >= now.Month && endDate.Day >= now.Day)
            {
                sql = @"
            SELECT
				OrderDate = order_hour, 
				CASE WHEN @OutletName = 'ALL' or @OutletName = 'ALL - BreakDown' then 'ALL' else OutletName end OutletName,
				Bill = SUM(Bill),
				NettAmount = SUM(NettAmount)
			from 
			(
				SELECT
					datepart(hour,orderdate) order_hour,
					OutletName = CASE @OutletName WHEN 'ALL' THEN 'ALL' ELSE OutletName END, 
					Bill,
					NettAmount 
				FROM viewDW_HourlySales
				WHERE
					OrderDate >= @StartDate and OrderDate < @EndDate
					AND (OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
				UNION ALL
				SELECT
					datepart(hour,orderdate) order_hour,
					OutletName, 
					Bill,
					NettAmount 
				FROM viewDW_HourlySales_today_sales_src
				WHERE
					OrderDate >= @StartDate and OrderDate < @EndDate
					AND (OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
			) x
			GROUP BY x.order_hour, CASE WHEN @OutletName = 'ALL' or @OutletName = 'ALL - BreakDown' then 'ALL' else OutletName end 
			ORDER BY OrderDate ASC";
            }
            else
            {
                sql = @"
            SELECT
				OrderDate = order_hour, 
				CASE WHEN @OutletName = 'ALL' or @OutletName = 'ALL - BreakDown' then 'ALL' else OutletName end OutletName,
				Bill = SUM(Bill),
				NettAmount = SUM(NettAmount)
			from 
			(
				SELECT
					datepart(hour,orderdate) order_hour,
					OutletName = CASE @OutletName WHEN 'ALL' THEN 'ALL' ELSE OutletName END, 
					Bill,
					NettAmount 
				FROM viewDW_HourlySales
				WHERE
					OrderDate >= @StartDate and OrderDate < @EndDate
					AND (OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
			) x
			GROUP BY x.order_hour, CASE WHEN @OutletName = 'ALL' or @OutletName = 'ALL - BreakDown' then 'ALL' else OutletName end 
			ORDER BY OrderDate ASC";
            }

            if (!string.IsNullOrEmpty(outletList) && outletList != "ALL" && (outlet.ToUpper() == "ALL" || outlet.ToUpper() == "ALL - BREAKDOWN"))
            {
                outlet = splitOutlets(outletList);
                sql = sql.Replace("@OutletNameList", outlet);
            }
            else
            {
                sql = sql.Replace("@OutletNameList", "@OutletName");
            }


            QueryCommand qc = new QueryCommand(sql);
            qc.AddParameter("@StartDate", startDate.ToString("yyyy-MM-dd"));
            qc.AddParameter("@EndDate", endDate.AddDays(1).ToString("yyyy-MM-dd"));
            qc.AddParameter("@OutletName", outlet);
            qc.AddParameter("@BeforeGST", isShowBeforeGST ? 1 : 0);

            IDataReader rdr = DataService.GetReader(qc);
            while (rdr.Read())
            {
                int bill = 0;
                double nettAmount = 0;

                int.TryParse(rdr["Bill"].ToString(), out bill);
                double.TryParse(rdr["NettAmount"].ToString(), out nettAmount);

                result.details.Add(new GetReportDailySalesItem
                {
                    SalesDate = rdr["OrderDate"].ToString(),
                    TotalSales = nettAmount,
                    TotalTrans = bill,
                    OutletName = rdr["OutletName"].ToString(),
                });
            }

            return new JavaScriptSerializer().Serialize(result);
        }

        [WebMethod]
        public string GetReportDailySales(string outlet, string startDateStr, string endDateStr, bool isShowBeforeGST)
        {

            //Logger.writeLog("Start Get Report Daily Sales :");
            GetReportDailySalesResult result = new GetReportDailySalesResult
            {
                status = "",
                details = new List<GetReportDailySalesItem>()
            };

            //Logger.writeLog("Start Authenticate :");
            string outletList = "ALL";
            AuthorizationStatus authStat = Authenticate(out outletList);
            if (authStat != AuthorizationStatus.VALID_TOKEN)
            {
                result.status = authStat.ToString().Replace("_", " ");
                return new JavaScriptSerializer().Serialize(result);
            }
            //Logger.writeLog("Finish Authenticate :");

            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime now = DateTime.Now;

            DateTime.TryParse(startDateStr, out startDate);
            DateTime.TryParse(endDateStr, out endDate);

            IDataReader rdr = null;

            try
            {
                string sql = "";
                if (endDate.Year >= now.Year && endDate.Month >= now.Month && endDate.Day >= now.Day)
                {
                    sql = @"
			        SELECT 
				        OrderDate = DS.OrderDate, 
				        OutletName = CASE @OutletName WHEN 'ALL' THEN 'ALL' ELSE DS.OutletName END,
				        Bill = DS.Bill, 
				        NettAmount = CASE @BeforeGST WHEN 1 THEN DS.BefGST ELSE DS.NettAmount END
			        FROM viewDW_DailySales DS
			        WHERE 
				        OrderDate >= @StartDate and OrderDate < @EndDate
				        AND (DS.OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
			        UNION ALL
			        SELECT 
				        OrderDate, 
				        OutletName, 
				        Bill, 
				        NettAmount 
			        FROM viewDW_DailySales_today_sales_src
			        WHERE
				        OrderDate >= @StartDate and OrderDate < @EndDate
				        AND (OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
		        ";
                }
                else
                {
                    sql = @"
			        SELECT 
				        OrderDate = DS.OrderDate, 
				        OutletName = CASE @OutletName WHEN 'ALL' THEN 'ALL' ELSE DS.OutletName END,
				        Bill = DS.Bill, 
				        NettAmount = CASE @BeforeGST WHEN 1 THEN DS.BefGST ELSE DS.NettAmount END
			        FROM viewDW_DailySales DS
			        WHERE 
				        OrderDate >= @StartDate and OrderDate < @EndDate
				        AND (DS.OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
		        ";
                }

                if (!string.IsNullOrEmpty(outletList) && outletList != "ALL" && (outlet.ToUpper() == "ALL" || outlet.ToUpper() == "ALL - BREAKDOWN"))
                {
                    outlet = splitOutlets(outletList);
                    sql = sql.Replace("@OutletNameList", outlet);
                }
                else
                {
                    sql = sql.Replace("@OutletNameList", "@OutletName");
                }

                QueryCommand qc = new QueryCommand(sql);
                qc.AddParameter("@StartDate", startDate.ToString("yyyy-MM-dd"));
                qc.AddParameter("@EndDate", endDate.AddDays(1).ToString("yyyy-MM-dd"));
                qc.AddParameter("@OutletName", outlet);
                qc.AddParameter("@BeforeGST", isShowBeforeGST ? 1 : 0);

                //Logger.writeLog("Start Query :");

                rdr = DataService.GetReader(qc);
                //Logger.writeLog("End Query :");
                while (rdr.Read())
                {
                    int bill = 0;
                    double nettAmount = 0;

                    int.TryParse(rdr["Bill"].ToString(), out bill);
                    double.TryParse(rdr["NettAmount"].ToString(), out nettAmount);

                    result.details.Add(new GetReportDailySalesItem
                    {
                        SalesDate = rdr["OrderDate"].ToString(),
                        TotalSales = nettAmount,
                        TotalTrans = bill,
                        OutletName = rdr["OutletName"].ToString(),
                    });
                }
                //Logger.writeLog("Finish Update Object");

                return new JavaScriptSerializer().Serialize(result);
                //rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.writeLog("GetReportDailySales Error." + ex.Message);
                return "";
            }
            finally
            {
                rdr.Close();
            }
            //return temp;

        }

        private string splitOutlets(string outlets)
        {
            try
            {
                string[] outletList = outlets.Split(',');
                string result = "";
                foreach (string outletName in outletList)
                {
                    result += "'" + outletName + "',";
                }
                result = result.Left(result.Length - 1);
                return result;
            }
            catch (Exception ex) { Logger.writeLog("Error Parsing Outlet List For POS Report. " + ex.Message); return ""; }
        }

        [WebMethod]
        public string GetReportMonthlySales(string outlet, string startDateStr, string endDateStr, bool isShowBeforeGST)
        {
            GetReportDailySalesResult result = new GetReportDailySalesResult
            {
                status = "",
                details = new List<GetReportDailySalesItem>()
            };
            string outletList = "ALL";
            AuthorizationStatus authStat = Authenticate(out outletList);
            if (authStat != AuthorizationStatus.VALID_TOKEN)
            {
                result.status = authStat.ToString().Replace("_", " ");
                return new JavaScriptSerializer().Serialize(result);
            }


            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime now = DateTime.Now;

            DateTime.TryParse(startDateStr, out startDate);
            DateTime.TryParse(endDateStr, out endDate);


            IDataReader rdr = null;
            try
            {
                string sql = "";
                if (endDate.Year >= now.Year && endDate.Month >= now.Month && endDate.Day >= now.Day)
                {
                    sql = @"
                SELECT
                    x.OrderDate, 
                    x.Outletname, 
                    Bill = SUM(x.Bill),
                    NettAmount = SUM(x.NettAmount)
                FROM (
                    SELECT 
                        OrderDate = DS.OrderDate, 
                        OutletName = CASE @OutletName WHEN 'ALL' THEN 'ALL' ELSE DS.OutletName END,
                        Bill = DS.Bill, 
                        NettAmount = CASE @BeforeGST WHEN 1 THEN DS.BefGST ELSE DS.NettAmount END
                    FROM viewDW_MonthlySales DS
                    WHERE 
                        OrderDate >= @StartDate and OrderDate < @EndDate  
                        AND (DS.OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                        
                    UNION ALL
                    SELECT 
                        CAST(CONVERT(VARCHAR,DATEPART(Year,OrderDate))+'-'+CONVERT(VARCHAR,DATEPART(MONTH,OrderDate))+'-01' as DATE) OrderDate,
                        OutletName = CASE @OutletName WHEN 'ALL' THEN 'ALL' ELSE OutletName END,
                        Bill, 
                        NettAmount
                    FROM viewDW_DailySales_today_sales_src
                    WHERE 
                        (OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                        AND OrderDate >= @StartDate and OrderDate < @EndDate  
                        
                ) x
                GROUP BY x.OrderDate, x.OutletName
                ORDER BY x.OrderDate, x.OutletName
            ";
                }
                else
                {
                    sql = @"
                SELECT
                    x.OrderDate, 
                    x.Outletname, 
                    Bill = SUM(x.Bill),
                    NettAmount = SUM(x.NettAmount)
                FROM (
                    SELECT 
                        OrderDate = DS.OrderDate, 
                        OutletName = CASE @OutletName WHEN 'ALL' THEN 'ALL' ELSE DS.OutletName END,
                        Bill = DS.Bill, 
                        NettAmount = CASE @BeforeGST WHEN 1 THEN DS.BefGST ELSE DS.NettAmount END
                    FROM viewDW_MonthlySales DS
                    WHERE 
                        OrderDate >= @StartDate and OrderDate < @EndDate  
                        AND (DS.OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                ) x
                GROUP BY x.OrderDate, x.OutletName
                ORDER BY x.OrderDate, x.OutletName
            ";
                }

                if (!string.IsNullOrEmpty(outletList) && outletList != "ALL" && (outlet.ToUpper() == "ALL" || outlet.ToUpper() == "ALL - BREAKDOWN"))
                {
                    outlet = splitOutlets(outletList);
                    sql = sql.Replace("@OutletNameList", outlet);
                }
                else
                {
                    sql = sql.Replace("@OutletNameList", "@OutletName");
                }


                QueryCommand qc = new QueryCommand(sql);
                qc.AddParameter("@StartDate", startDate.ToString("yyyy-MM-dd"));
                qc.AddParameter("@EndDate", endDate.AddDays(1).ToString("yyyy-MM-dd"));
                qc.AddParameter("@OutletName", outlet);
                qc.AddParameter("@BeforeGST", isShowBeforeGST ? 1 : 0);

                rdr = DataService.GetReader(qc);
                while (rdr.Read())
                {
                    int bill = 0;
                    double nettAmount = 0;

                    int.TryParse(rdr["Bill"].ToString(), out bill);
                    double.TryParse(rdr["NettAmount"].ToString(), out nettAmount);

                    result.details.Add(new GetReportDailySalesItem
                    {
                        SalesDate = rdr["OrderDate"].ToString(),
                        TotalSales = nettAmount,
                        TotalTrans = bill,
                        OutletName = rdr["OutletName"].ToString(),
                    });
                }

                return new JavaScriptSerializer().Serialize(result);
            }
            catch (Exception ex)
            {
                Logger.writeLog("GetReportDailySales Error." + ex.Message);
                return "";
            }
            finally
            {
                rdr.Close();
            }
        }

        [WebMethod]
        public string GetReportDailyProduct(string outlet, string startDateStr, string endDateStr, int pageNo, int limit)
        {
            GetReportProductSalesResult result = new GetReportProductSalesResult
            {
                status = "",
                details = new List<GetReportProductSalesItem>()
            };
            string outletList = "ALL";
            AuthorizationStatus authStat = Authenticate(out outletList);
            if (authStat != AuthorizationStatus.VALID_TOKEN)
            {
                result.status = authStat.ToString().Replace("_", " ");
                return new JavaScriptSerializer().Serialize(result);
            }


            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime now = DateTime.Now;

            DateTime.TryParse(startDateStr, out startDate);
            DateTime.TryParse(endDateStr, out endDate);

            IDataReader rdr = null;

            try
            {
                string sql = "";
                if (endDate.Year >= now.Year && endDate.Month >= now.Month && endDate.Day >= now.Day)
                {
                    sql = @"
                SELECT 
                    x.Itemno,
                    I.ItemName,  
                    x.Quantity, 
                    x.Amount
                FROM (
                    SELECT 
                        record_no = ROW_NUMBER() OVER (ORDER BY SUM(Amount) DESC), 
                        ItemNo, 
                        Quantity = SUM(Quantity), 
                        Amount = SUM(Amount)
                    FROM 
                    (
                        SELECT 
                            ItemNo, 
                            Quantity, 
                            Amount
                        FROM viewDW_DailyProductSales
                        WHERE
                            OrderDate >= @StartDate and OrderDate < @EndDate 
                            AND (OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                        UNION ALL 
                        SELECT 
                            ItemNo, 
                            Quantity,
                            Amount
                        FROM viewDW_DailyProductSales_today_src
                        WHERE
                            (OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                            AND OrderDate >= @StartDate and OrderDate < @EndDate  
                    ) y 
                    GROUP BY ItemNo
                ) x
                LEFT JOIN Item I on X.ItemNo = I.ItemNo 
                WHERE 
                    x.record_no between @PageStart and @PageEnd
            ";
                }
                else
                {
                    sql = @"
                SELECT 
                    x.Itemno,
                    I.ItemName,  
                    x.Quantity, 
                    x.Amount
                FROM (
                    SELECT 
                        record_no = ROW_NUMBER() OVER (ORDER BY SUM(Amount) DESC), 
                        ItemNo, 
                        Quantity = SUM(Quantity), 
                        Amount = SUM(Amount)
                    FROM 
                    (
                        SELECT 
                            ItemNo, 
                            Quantity, 
                            Amount
                        FROM viewDW_DailyProductSales
                        WHERE
                            OrderDate >= @StartDate and OrderDate < @EndDate 
                            AND (OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                    ) y 
                    GROUP BY ItemNo
                ) x
                LEFT JOIN Item I on X.ItemNo = I.ItemNo 
                WHERE 
                    x.record_no between @PageStart and @PageEnd
            ";
                }

                if (!string.IsNullOrEmpty(outletList) && outletList != "ALL" && (outlet.ToUpper() == "ALL" || outlet.ToUpper() == "ALL - BREAKDOWN"))
                {
                    outlet = splitOutlets(outletList);
                    sql = sql.Replace("@OutletNameList", outlet);
                }
                else
                {
                    sql = sql.Replace("@OutletNameList", "@OutletName");
                }


                QueryCommand qc = new QueryCommand(sql);
                qc.AddParameter("@StartDate", startDate.ToString("yyyy-MM-dd"));
                qc.AddParameter("@EndDate", endDate.AddDays(1).ToString("yyyy-MM-dd"));
                qc.AddParameter("@OutletName", outlet);
                qc.AddParameter("@PageStart", (pageNo * limit) + 1);
                qc.AddParameter("@PageEnd", (pageNo + 1) * limit);

                rdr = DataService.GetReader(qc);
                while (rdr.Read())
                {
                    double quantity = 0;
                    double amount = 0;

                    double.TryParse(rdr["Quantity"].ToString(), out quantity);
                    double.TryParse(rdr["Amount"].ToString(), out amount);

                    result.details.Add(new GetReportProductSalesItem
                    {
                        ItemNo = rdr["ItemNo"].ToString(),
                        ItemName = rdr["ItemName"].ToString(),
                        Quantity = quantity,
                        Amount = amount
                    });
                }

                return new JavaScriptSerializer().Serialize(result);
            }
            catch (Exception ex)
            {
                Logger.writeLog("GetReportDailySales Error." + ex.Message);
                return "";
            }
            finally
            {
                rdr.Close();
            }
        }

        [WebMethod]
        public string GetReportDailyProductCategory(string outlet, string startDateStr, string endDateStr, int pageNo, int limit)
        {
            GetReportProductCategorySalesResult result = new GetReportProductCategorySalesResult
            {
                status = "",
                details = new List<GetReportProductCategorySalesItem>()
            };

            string outletList = "ALL";
            AuthorizationStatus authStat = Authenticate(out outletList);
            if (authStat != AuthorizationStatus.VALID_TOKEN)
            {
                result.status = authStat.ToString().Replace("_", " ");
                return new JavaScriptSerializer().Serialize(result);
            }

            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime now = DateTime.Now;

            DateTime.TryParse(startDateStr, out startDate);
            DateTime.TryParse(endDateStr, out endDate);

            IDataReader rdr = null;

            try
            {
                string sql = "";
                if (endDate.Year >= now.Year && endDate.Month >= now.Month && endDate.Day >= now.Day)
                {
                    sql = @"
			    SELECT
				    x.Categoryname, 
				    x.Quantity, 
				    x.Amount
			    from (
				    SELECT 
					    record_no = ROW_NUMBER() OVER (ORDER BY SUM(Amount) DESC), 
					    Categoryname, 
					    Quantity = sum(Quantity), 
					    Amount = sum(Amount)
				    FROM 
				    (
					    SELECT
						    CategoryName, 
						    Quantity, 
						    Amount,
						    OutletName
					    FROM viewDW_DailyProductCategorySales
					    WHERE 
						    OrderDate >= @StartDate and OrderDate < @EndDate
						    AND (OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
					    UNION ALL
					    SELECT 
						    CategoryName,
						    Quantity, 
						    Amount,
						    OutletName
					    FROM viewDW_DailyProductCategorySales_today_src
					    WHERE
						    OrderDate >= @StartDate and OrderDate < @EndDate
						    AND (OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
				    ) y 
				    GROUP BY CategoryName
			    ) x
			    WHERE 
				    x.record_no between @PageStart and @PageEnd
		    ";
                }
                else
                {
                    sql = @"
			    SELECT
				    x.Categoryname, 
				    x.Quantity, 
				    x.Amount
			    from (
				    SELECT 
					    record_no = ROW_NUMBER() OVER (ORDER BY SUM(Amount) DESC), 
					    Categoryname, 
					    Quantity = sum(Quantity), 
					    Amount = sum(Amount)
				    FROM 
				    (
					    SELECT
						    CategoryName, 
						    Quantity, 
						    Amount,
						    OutletName
					    FROM viewDW_DailyProductCategorySales
					    WHERE 
						    OrderDate >= @StartDate and OrderDate < @EndDate
						    AND (OutletName in (@OutletNameList) OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
				    ) y 
				    GROUP BY CategoryName
			    ) x
			    WHERE 
				    x.record_no between @PageStart and @PageEnd
		    ";
                }

                if (!string.IsNullOrEmpty(outletList) && outletList != "ALL" && (outlet.ToUpper() == "ALL" || outlet.ToUpper() == "ALL - BREAKDOWN"))
                {
                    outlet = splitOutlets(outletList);
                    sql = sql.Replace("@OutletNameList", outlet);
                }
                else
                {
                    sql = sql.Replace("@OutletNameList", "@OutletName");
                }

                QueryCommand qc = new QueryCommand(sql);
                qc.AddParameter("@StartDate", startDate.ToString("yyyy-MM-dd"));
                qc.AddParameter("@EndDate", endDate.AddDays(1).ToString("yyyy-MM-dd"));
                qc.AddParameter("@OutletName", outlet);
                qc.AddParameter("@PageStart", (pageNo * limit) + 1);
                qc.AddParameter("@PageEnd", (pageNo + 1) * limit);

                rdr = DataService.GetReader(qc);
                while (rdr.Read())
                {
                    double quantity = 0;
                    double amount = 0;

                    double.TryParse(rdr["Quantity"].ToString(), out quantity);
                    double.TryParse(rdr["Amount"].ToString(), out amount);

                    result.details.Add(new GetReportProductCategorySalesItem
                    {
                        CategoryName = rdr["CategoryName"].ToString(),
                        Quantity = quantity,
                        Amount = amount
                    });
                }

                return new JavaScriptSerializer().Serialize(result);
            }
            catch (Exception ex)
            {
                Logger.writeLog("GetReportDailySales Error." + ex.Message);
                return "";
            }
            finally
            {
                rdr.Close();
            }
        }

        #endregion

        #region Helpers

        private AuthorizationStatus Authenticate(out string outletList)
        {
            AuthorizationStatus status = AuthorizationStatus.NO_AUTHORIZATION;
            outletList = "ALL";
            string email = this.Context.Request.Headers["Email"];
            string authorization = this.Context.Request.Headers["Authorization"];
            if (authorization != null)
            {
                string token = authorization.Replace("Bearer", "").Trim();

                status = IsValidateToken(token, email, out outletList);
            }

            return status;
        }

        private AuthorizationStatus IsValidateToken(string token, string email, out string outletList)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;

            string UserName = null;
            string userfld9 = null;
            outletList = "";
            {
                QueryCommand qc = new QueryCommand("SELECT UserName, userfld9, userfld1 FROM UserMst WHERE userfld8=@Email");
                qc.AddParameter("@Email", email);

                IDataReader rdr = DataService.GetReader(qc);
                if (rdr.Read())
                {
                    UserName = rdr["UserName"].ToString();
                    userfld9 = rdr["userfld9"].ToString();
                    outletList = rdr["userfld1"].ToString();
                }
                else
                    return AuthorizationStatus.UNREGISTERED_EMAIL;
            }

            //Logger.writeLog("Outlet Lookup : " + UserName + " : " + outletList);

            if (token == userfld9)
                return AuthorizationStatus.VALID_TOKEN;

            string URI = ConfigurationManager.AppSettings["POSReportCustomerMasterURL"] + "";
            if (string.IsNullOrEmpty(URI))
                URI = "http://equipweb.biz/CustomerMaster/api/Token/ValidateToken";

            string parameters = "token=" + token;

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(URI, parameters);

                ValidateTokenResult record = new JavaScriptSerializer().Deserialize<ValidateTokenResult>(HtmlResult);
                if (record != null)
                {
                    if (email == record.email)
                    {
                        QueryCommand qc = new QueryCommand("UPDATE UserMst SET userfld9 = @userfld9 WHERE UserName = @UserName");
                        qc.AddParameter("@UserName", UserName);
                        qc.AddParameter("@userfld9", token);

                        int rowAffected = DataService.ExecuteQuery(qc);
                    }
                    else
                        return AuthorizationStatus.UNAUTHORIZED_ACCESS;
                }
            }

            return AuthorizationStatus.VALID_TOKEN;
        }

        #endregion
    }

    public class ItemCookHistoryObject
    {
        public string DocumentNo { get; set; }
        public string Status { get; set; }
        public decimal COG { get; set; }
        public int ItemCookHistoryID { get; set; }
        public DateTime CookDate { get; set; }
        public int InventoryLocationID { get; set; }
        public string InventoryLocationName { get; set; }
        public string DepartmentName { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
        public string ItemNo { get; set; }
        public decimal Quantity { get; set; }
    }

    public class ItemCookDetailObject
    {
        public int ItemCookDetailID { get; set; }
        public int ItemCookHistoryID { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        public decimal OriginalQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalCost { get; set; }
        public string UOM { get; set; }
        public bool IsLoadFromRecipe { get; set; }
    }

    public class StockResult
    {
        public string newRefNo { get; set; }
        public string status { get; set; }
    }

    public class ItemOptimalStockObj
    {
        public string BaseLevelID { get; set; }
        public string ItemNo { get; set; }
        public int BaseLevelQuantity { get; set; }
        public int InventoryLocationID { get; set; }
    }

    public class CategoryOptimalStockObj
    {
        public string CategoryName { get; set; }
        public int BaseLevelQuantity { get; set; }
        public int InventoryLocationID { get; set; }
    }

    public class LoginResult
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public string DeptID { get; set; }
        public string PointOfSaleID { get; set; }
        public InventoryLocation InventoryLocation { get; set; }
        public List<string> Privileges { get; set; }
        public string CompanyName { get; set; }
        public string UserToken { get; set; }
        public bool result { get; set; }
        public string status { get; set; }
        public InventoryLocationCollection InventoryLocationCollection { get; set; }
        public bool isSupplier { get; set; }
        public bool isRestrictedSupplierList { get; set; }
        public bool isUseUserPortal { get; set; }
        public bool isUseTransferApproval { get; set; }
    }

    public class TestReportResult
    {
        public ArrayList rows { get; set; }
        public ArrayList columns { get; set; }
    }

    public class AutoPrintData
    {
        public DateTime TimeStamp { get; set; }
        public DataSet DataSet { get; set; }
    }

    public class MembershipObj
    {
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string NRIC { get; set; }
        public string HomeAddress { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string PassCode { get; set; }
        public string ConfirmPassCode { get; set; }
        public string Child1GivenName { get; set; }
        public string Child1DateBirth { get; set; }
        public string Child2GivenName { get; set; }
        public string Child2DateBirth { get; set; }
        public string chkMagazines { get; set; }
        public string chkOnlineSearch { get; set; }
        public string chkOnlineMedia { get; set; }
        public string chkFriends { get; set; }
        public string chkOther { get; set; }
        public string chkAgreement { get; set; }
        public string IsLifeTimeMember { get; set; }
        public string Nationality { get; set; }
        public string Remark { get; set; }
    }

    public class AppointmentResult
    {
        public string AppointmentID { get; set; }
        public string MembershipNo { get; set; }
        public string Customer { get; set; }
        public string ItemName { get; set; }
        public string ResourceName { get; set; }
        public int Duration { get; set; }
        public string StartTime { get; set; }
        public string Staff { get; set; }
        public string CheckInByWho { get; set; }
        public string CheckOutByWho { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string Remark { get; set; }
    }

    public class ResourceResult
    {
        public string ResourceID { get; set; }
        public string ResourceName { get; set; }
    }

    public class PastTransactionResult
    {

        public string ItemNo { get; set; }
        public string OrderDate { get; set; }
        public string ItemName { get; set; }
        public string SalesPerson { get; set; }
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public string Amount { get; set; }
    }

    public class CheckInResult
    {
        public string CheckInByWho { get; set; }
        public string CheckInTime { get; set; }
    }

    public class CheckOutResult
    {
        public string CheckOutByWho { get; set; }
        public string CheckOutTime { get; set; }
    }

    public class RatingClz
    {
        public int RatingID { get; set; }
        public int POSID { get; set; }
        public int Rating { get; set; }
        public string Staff { get; set; }
        public string Timestamp { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int Deleted { get; set; }
        public string UniqueId { get; set; }
    }

    public class RecordData
    {
        public String RecordDataID { get; set; }
        public int InventoryLocationID { get; set; }
        public string Val1 { get; set; }
        public string Val2 { get; set; }
        public string Val3 { get; set; }
        public string Val4 { get; set; }
        public string Val5 { get; set; }
        public string Val6 { get; set; }
        public string Val7 { get; set; }
        public string Val8 { get; set; }
        public string Val9 { get; set; }
        public string Val10 { get; set; }
        public string InventoryHdrRefNo { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ItemTemp
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        //public string CategoryName { get; set; }
    }

    public class StockInCMParam
    {
        public int SupplierID { get; set; }
        public string InvoiceNo { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string Remark { get; set; }
    }

    public class NewProductParam
    {
        public Nullable<bool> IsNew { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string Barcode { get; set; }
        public string UOM { get; set; }
        public string CategoryName { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public Nullable<decimal> FactoryPrice { get; set; }
        public Nullable<decimal> MinimumPrice { get; set; }
        public Nullable<bool> Matrix { get; set; }
        public string ApplicableTo { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public Nullable<int> OutletGroupID { get; set; }
        public string OutletName { get; set; }
        public Nullable<bool> IsNonDiscountable { get; set; }
        public Nullable<bool> IsCommission { get; set; }
        public Nullable<bool> IsService { get; set; }
        public Nullable<bool> IsPoint { get; set; }
        public Nullable<bool> IsPointRedeemable { get; set; }
        public Nullable<decimal> PointGet { get; set; }
        public Nullable<bool> IsCourse { get; set; }
        public Nullable<decimal> TimesGet { get; set; }
        public Nullable<decimal> BreakdownPrice { get; set; }
        public Nullable<bool> IsOpenPricePackage { get; set; }
        public Nullable<bool> IsOpenPriceProduct { get; set; }
        public Nullable<bool> IsNonInventoryProduct { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }
        public string P4 { get; set; }
        public string P5 { get; set; }
        public string GSTRule { get; set; }
        public string ItemDesc { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; }
        public string Attribute5 { get; set; }
        public string Attribute6 { get; set; }
        public string Attribute7 { get; set; }
        public string Attribute8 { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsPreOrder { get; set; }
        public Nullable<int> CapQty { get; set; }
        public Nullable<bool> IsVendorDelivery { get; set; }
        public Nullable<bool> IsPAMedifund { get; set; }
        public Nullable<bool> IsSMF { get; set; }
        public Nullable<bool> GenerateBarcode { get; set; }
        public Nullable<bool> AutoCaptureWeight { get; set; }
        public string DeductedItemNo { get; set; }
        public Nullable<decimal> DeductConvRate { get; set; }
        public Nullable<bool> DeductConvType { get; set; }
        public Nullable<bool> IsConsignment { get; set; }
        public Nullable<int> SupplierID { get; set; }

        public void DefaultNewProduct()
        {
            #region Init Values

            if (!IsNew.HasValue)
                IsNew = true;

            if (ItemNo == null)
                ItemNo = "";
            if (ItemName == null)
                ItemName = "";
            if (Barcode == null)
                Barcode = "";
            if (UOM == null)
                UOM = "";
            if (CategoryName == null)
                CategoryName = "";
            if (!RetailPrice.HasValue)
                RetailPrice = 0;
            if (!FactoryPrice.HasValue)
                FactoryPrice = 0;

            if (!Deleted.HasValue)
                Deleted = false;

            if (Attribute1 == null)
                Attribute1 = "";
            if (Attribute2 == null)
                Attribute2 = "";
            if (Attribute3 == null)
                Attribute3 = "";
            if (Attribute4 == null)
                Attribute4 = "";
            if (Attribute5 == null)
                Attribute5 = "";
            if (Attribute6 == null)
                Attribute6 = "";
            if (Attribute7 == null)
                Attribute7 = "";
            if (Attribute8 == null)
                Attribute8 = "";

            if (ItemDesc == null)
                ItemDesc = "";
            if (Remark == null)
                Remark = "";

            #endregion

            #region Defaults

            if (!IsCommission.HasValue)
                IsCommission = false;
            if (!IsNonDiscountable.HasValue)
                IsNonDiscountable = false;
            if (!IsPointRedeemable.HasValue)
                IsPointRedeemable = false;

            #region if item is a product

            if (!IsService.HasValue)
                IsService = false;
            if (!IsPoint.HasValue)
                IsPoint = false;
            if (!IsCourse.HasValue)
                IsCourse = false;
            if (!IsOpenPricePackage.HasValue)
                IsOpenPricePackage = false;

            #endregion

            if (!Matrix.HasValue)
                Matrix = false;

            if (GSTRule == null)
                GSTRule = AppSetting.GetSetting("Item_DefaultGSTSetting");

            if (ApplicableTo == "")
                ApplicableTo = "Product Master";

            #endregion
        }
    }

    public class NewProductResult
    {
        public string status { get; set; }
        public string resultItemNo { get; set; }
        public string resultMatrixMode { get; set; }
    }

    public class SaveItemQuantityTriggerParam
    {
        public Nullable<int> TriggerQuantity { get; set; }
        public string TriggerLevel { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string userfld1 { get; set; }
        public string userfld2 { get; set; }
        public string userfld3 { get; set; }
        public string userfld4 { get; set; }
        public string userfld5 { get; set; }
        public string userfld6 { get; set; }
        public string userfld7 { get; set; }
        public string userfld8 { get; set; }
        public string userfld9 { get; set; }
        public string userfld10 { get; set; }
    }

    public class SaveItemQuantityTriggerResult
    {
        public string status { get; set; }
        public int resultTriggerID { get; set; }
    }

    public class GetReportDailySalesResult
    {
        public string status { get; set; }
        public List<GetReportDailySalesItem> details { get; set; }
    }

    public class GetReportDailySalesItem
    {
        public string SalesDate { get; set; }
        public double TotalSales { get; set; }
        public int TotalTrans { get; set; }
        public string OutletName { get; set; }
    }

    public class GetReportProductSalesResult
    {
        public string status { get; set; }
        public List<GetReportProductSalesItem> details { get; set; }
    }

    public class GetReportProductSalesItem
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }
    }

    public class GetReportProductCategorySalesResult
    {
        public string status { get; set; }
        public List<GetReportProductCategorySalesItem> details { get; set; }
    }

    public class GetReportProductCategorySalesItem
    {
        public string CategoryName { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }
    }

    public class ValidateTokenResult
    {
        public string status { get; set; }
        public string email { get; set; }
    }



    public enum AuthorizationStatus
    {
        INVALID_TOKEN,
        UNREGISTERED_EMAIL,
        UNAUTHORIZED_ACCESS,
        VALID_TOKEN,
        NO_AUTHORIZATION,
    }

    public class POItemDetail
    {
        public string Id;

        public string ItemNo;

        public string ItemName;

        public String UOM;

        public double RetailPrice;

        public double CostOfGoods;

        public string BatchNo;

        public DateTime ExpiryDate;

        public double Qty;

        public string ItemPackingSize;

        public double ItemPackingSizeUOM;

        public double CostPrice;

        public string GSTRule;
    }

    public class POrderDetailObject
    {

        public string PurchaseOrderHeaderRefNo { get; set; }
        public string PurchaseOrderDetailRefNo { get; set; }
        public string ItemNo { get; set; }
        public decimal? FactoryPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal? Quantity { get; set; }
        public decimal OriginalQuantity { get; set; }
        public decimal QtyApproved { get; set; }
        public decimal RejectQty { get; set; }
        public decimal OptimalStock { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public decimal SalesQty { get; set; }
        public decimal? Amount { get; set; }
        public decimal? GSTAmount { get; set; }
        public string DiscountDetail { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public ItemObject Item { get; set; }
        public decimal? CostOfGoods { get; set; }
        public string SerialNo { get; set; }
    }

    public class ItemObject
    {
        public string CategoryName { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string ItemDepartmentID { get; set; }
        public int BaseLevel { get; set; }
        public string UOM { get; set; }
        public decimal? P1Price { get; set; }
        public decimal? P2Price { get; set; }
        public decimal? P3Price { get; set; }
        public decimal? P4Price { get; set; }
        public decimal? P5Price { get; set; }
        public CategoryObject Category { get; set; }
        public string Barcode { get; set; }
        public bool? Deleted { get; set; }

    }

    public class ViewPurchaseOrderObject
    {
        public string PurchaseOrderHeaderRefNo { get; set; }
        public string RequestedBy { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public int? DepartmentID { get; set; }
        public int? PaymentTermID { get; set; }
        public string ShipVia { get; set; }
        public string ShipTo { get; set; }
        public DateTime? DateNeededBy { get; set; }
        public int? SupplierID { get; set; }
        public string UserName { get; set; }
        public string Remark { get; set; }
        public int? InventoryLocationID { get; set; }
        public string InventoryLocationName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Status { get; set; }
        public string POType { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string ApprovedBy { get; set; }
        public string SpecialValidFrom { get; set; }
        public string SpecialValidTo { get; set; }
        public string ApprovalStatus { get; set; }
        public string SalesPersonID { get; set; }
        public string PriceLevel { get; set; }
        public int? ReasonID { get; set; }
        public int DestInventoryLocationID { get; set; }
        public int WarehouseID { get; set; }
        public bool IsAutoStockIn { get; set; }
        public string OrderFromName { get; set; }
        public InventoryLocationObject InventoryLocation { get; set; }
        public InventoryStockOutReasonObject InventoryStockOutReason { get; set; }
    }

    public class InventoryLocationObject
    {
        public int? InventoryLocationID { get; set; }
        public string InventoryLocationName { get; set; }
    }

    public class ItemDepartmentObject
    {
        public string ItemDepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int? DepartmentOrder { get; set; }
    }

    public class CategoryObject
    {
        public ItemDepartmentObject ItemDepartment { get; set; }
        public string CategoryName { get; set; }
    }

    public class InventoryStockOutReasonObject
    {
        public int? ReasonID { get; set; }
        public string ReasonName { get; set; }
    }

    public class SearchResultRecipeHdr
    {
        public string RecipeHeaderID { get; set; }
        public string RecipeHeaderNo { get; set; }
        public string RecipeName { get; set; }
        public int TotalMaterial { get; set; }
        public int IsHaveWrongConv { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal TotalMaterialCost { get; set; }
    }

    public class RecipeHeaderWithConvRateObj
    {
        public string RecipeHeaderID { get; set; }
        public string RecipeName { get; set; }
        public string ItemNo { get; set; }
        public string Type { get; set; }
        public List<RecipeDetailWithConvRateObj> Details { get; set; }
    }

    public class RecipeDetailWithConvRateObj
    {
        public string RecipeDetailID { get; set; }
        public string RecipeHeaderNo { get; set; }
        public string RecipeName { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        public string Uom { get; set; }
        public string MaterialUOM { get; set; }
        public decimal? ConversionRate { get; set; }
        public bool IsPacking { get; set; }
        public decimal CostRate { get; set; }
    }

    public class UOMConversionObj
    {
        public int ConversionDetID { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string FromUOM { get; set; }
        public string ToUOM { get; set; }
        public decimal ConversionRate { get; set; }
        public string Remark { get; set; }
    }
}
