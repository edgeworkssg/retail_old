using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Serialization;

using PowerPOS;
using Newtonsoft.Json;
using System.Data;

namespace PowerWeb.Synchronization
{
    /// <summary>
    /// Summary description for MallIntegrationServer
    /// </summary>
    [WebService(Namespace = "http://www.edgeworks.com.sg")]
    [WebServiceBinding(Name = "MallIntegrationService", ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MallIntegrationServer : System.Web.Services.WebService
    {
        public UserCredentials tenant;

        [WebMethod]
        [SoapDocumentMethod(Binding = "MallIntegrationService")]
        [SoapHeader("tenant", Required = true)]
        public string SendDataFromServer(string data)
        {
            UserMst usr = new UserMst();
            string status;

            if (MallIntegrationProviderController.CheckLogin(tenant.mallCode, tenant.userName, tenant.password, out status))
            {
                bool isCorrect = MallIntegrationProviderController.validateFile(tenant.userName, data, out status);
                if (isCorrect)
                {
                    List<MallIntegrationData> tmp;
                    try
                    {
                        tmp = new JavaScriptSerializer().Deserialize<List<MallIntegrationData>>(data);

                    }
                    catch (Exception ex)
                    {
                        AccessLogController.AddLog(AccessSource.WEB, tenant.userName + "", tenant.mallCode + "", "Web Service Submission Failed. Error : Invalid CSV file format", "");
                        Logger.writeLog("Error Parsing Data Sent, " + data + "." + ex.Message);
                        return JsonConvert.SerializeObject(new { result = false, status = "Error Parsing the data." });
                    }

                    if (tmp.Count <= 0)
                    {
                        AccessLogController.AddLog(AccessSource.WEB, tenant.userName + "", tenant.mallCode + "", "Web Service Submission Failed. Error : Empty Data", "");
                        return JsonConvert.SerializeObject(new { result = false, status = "Error. Data Is Empty." });
                    }
                    else
                    {
                        //got data...
                        //Parsing and Insert
                        System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                        to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                        using (System.Transactions.TransactionScope transScope =
                        new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                        {
                            for (int i = 0; i < tmp.Count; i++)
                            {
                                MallIntegrationProviderController pos = new MallIntegrationProviderController();

                                #region Check Date
                                DateTime orderDate;
                                if (!MallIntegrationProviderController.CheckDate(tmp[i].Date, tmp[i].Hour, out status, out orderDate))
                                {
                                    AccessLogController.AddLog(AccessSource.WEB, tenant.userName + "", tenant.mallCode + "", "Web Service Submission Failed. Error : Invalid Date", "");
                                    return JsonConvert.SerializeObject(new { result = false, status = "Error." + status });
                                }

                                #endregion

                                int posid = 0;
                                #region Check Tenant ID
                                if (!MallIntegrationProviderController.CheckTenantID(tmp[i].TenantCode, out posid))
                                {

                                    status = "Error. Invalid Tenant Code.";
                                    AccessLogController.AddLog(AccessSource.WEB, tenant.userName + "", tenant.mallCode + "", "Web Service Submission Failed. Error : Invalid Tenant Code", "");
                                    return JsonConvert.SerializeObject(new { result = false, status = "Invalid Tenant Code" });
                                }
                                #endregion

                                #region Checkif OrderHdr is Exist
                                OrderHdrCollection ohCol = new OrderHdrCollection();
                                ohCol.Where(OrderHdr.Columns.PointOfSaleID, posid);
                                ohCol.Where(OrderHdr.Columns.OrderDate, orderDate);
                                ohCol.Where(OrderHdr.Columns.IsVoided, false);
                                ohCol.Load();
                                if (ohCol.Count > 0)
                                {
                                    foreach (OrderHdr oh in ohCol)
                                    {
                                        oh.IsVoided = true;
                                        oh.Save("SYNC");
                                    }
                                }
                                #endregion

                                string orderHdrID = "";
                                if (!pos.InsertMallIntegration(tmp[i].TenantCode, orderDate, tmp[i].TransactionCount, tmp[i].TotalSalesAfterTax, tmp[i].TotalTax,
                                    tenant.userName, "", posid, out status, out orderHdrID))
                                {
                                    AccessLogController.AddLog(AccessSource.WEB, tenant.userName + "", tenant.mallCode + "", "Web Service Submission Failed. Error :" + status, "");
                                    return JsonConvert.SerializeObject(new { result = false, status = status });
                                }
                                //pos.AddItemToOrder()
                            }
                           transScope.Complete();
                        }

                        //Confirm Order

                    }
                    AccessLogController.AddLog(AccessSource.WEB, tenant.userName + "", tenant.mallCode + "", "Web Service Submission Success.", "");
                    return JsonConvert.SerializeObject(new { result = true, status = "" });
                }
                else
                {
                    AccessLogController.AddLog(AccessSource.WEB, tenant.userName + "", tenant.mallCode + "", "Web Service Submission Failed. Error :" + status, "");
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }
            }
            else
            {
                AccessLogController.AddLog(AccessSource.WEB, tenant.userName + "", tenant.mallCode + "", "Web Service Submission Failed. Error :" + status, "");
                return JsonConvert.SerializeObject(new { result = false, status = status });
            }
        }

        [WebMethod]
        [SoapDocumentMethod(Binding = "MallIntegrationService")]
        [SoapHeader("tenant", Required = true)]
        public string getAPIKey(string tenantCode)
        {
            string result = "";
            try
            {
                string status;
                UserMst usr;
                string role, DeptID;
                
                if (UserController.login(tenant.userName, tenant.password, LoginType.Login, out usr, out role, out DeptID, out status ))
                {
                    DataTable privileges = UserController.FetchGroupPrivilegesWithUsername(role, tenant.userName);
                    if (PrivilegesController.HasPrivilege("Mall Service Access", privileges))
                    {
                        // return API Key

                        PointOfSaleCollection p = new PointOfSaleCollection();
                        p.Where(PointOfSale.Columns.TenantMachineID, tenantCode);
                        p.Load();
                        if (p.Count > 0)
                        {
                            result = UserController.DecryptData(p[0].ApiKey);
                        }
                        
                    }
                    
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return "";
            }
           
        }


        

    }
    

    
}
