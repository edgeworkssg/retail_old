using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Serialization;

using PowerPOS;
using Newtonsoft.Json;

namespace PowerWeb.Synchronization
{
    /// <summary>
    /// Summary description for MallIntegration
    /// </summary>
    [WebService(Namespace = "http://www.edgeworks.com.sg/")]
    [WebServiceBinding(Name="MallIntegrationService", ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MallIntegration : System.Web.Services.WebService
    {
        public UserCredentials tenant;
        
        [WebMethod]
        [SoapDocumentMethod(Binding = "MallIntegrationService")]
        [SoapHeader("tenant", Required = true)]
        public string SendData(string data)
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
                        AccessLogController.AddTenantHistory(tenant.userName, 0, AccessSource.WEBServices, tenant.userName, "Web Service Submission Failed. Error : Invalid CSV file format");
                        Logger.writeLog("Error Parsing Data Sent, " + data + "." + ex.Message);
                        return JsonConvert.SerializeObject(new { result = false, status = "Error Parsing the data." });
                    }
                    PointOfSale thePointOfSale = new PointOfSale(PointOfSale.Columns.TenantMachineID, tenant.userName);

                    if (tmp.Count <= 0)
                    {
                        AccessLogController.AddTenantHistory(thePointOfSale.TenantMachineID, thePointOfSale.PointOfSaleID, AccessSource.WEBServices, tenant.userName, "Web Service Submission Failed. Error : Empty Data");
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
                                    AccessLogController.AddTenantHistory(thePointOfSale.TenantMachineID, thePointOfSale.PointOfSaleID, AccessSource.WEBServices, tenant.userName, "Web Service Submission Failed. Error : Invalid Date");
                                    return JsonConvert.SerializeObject(new { result = false, status = "Error." + status });
                                }

                                #endregion

                                int posid = 0;
                                #region Check Tenant ID
                                if (!MallIntegrationProviderController.CheckTenantID(tmp[i].TenantCode, out posid))
                                {

                                    status = "Error. Invalid Tenant Code.";
                                    AccessLogController.AddTenantHistory(thePointOfSale.TenantMachineID, thePointOfSale.PointOfSaleID, AccessSource.WEBServices, tenant.userName, "Web Service Submission Failed. Error : Invalid Tenant Code");
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
                                    AccessLogController.AddTenantHistory(thePointOfSale.TenantMachineID, thePointOfSale.PointOfSaleID, AccessSource.WEBServices, tenant.userName, "Web Service Submission Failed. Error :" + status);
                                    return JsonConvert.SerializeObject(new { result = false, status = status });
                                }
                                //pos.AddItemToOrder()
                            }
                            transScope.Complete();
                        }

                        //Confirm Order

                    }
                    AccessLogController.AddTenantHistory(thePointOfSale.TenantMachineID, thePointOfSale.PointOfSaleID, AccessSource.WEBServices, tenant.userName, "Web Service Submission Success.");
                    return JsonConvert.SerializeObject(new { result = true, status = "" });
                }
                else
                {
                    AccessLogController.AddTenantHistory(tenant.userName, 0, AccessSource.WEBServices, tenant.userName, "Web Service Submission Failed. Error :" + status);
                    return JsonConvert.SerializeObject(new { result = false, status = status });
                }
            }
            else
            {
                AccessLogController.AddTenantHistory(tenant.userName, 0, AccessSource.WEBServices, tenant.userName, "Web Service Submission Failed. Error :" + status);
                return JsonConvert.SerializeObject(new { result = false, status = status });
            }
            
        }

    }

    public class UserCredentials : System.Web.Services.Protocols.SoapHeader
    {
        public string mallCode;
        public string userName;
        public string password;
    }

    public class MallIntegrationData
    {
        public string MallCode;
        public string TenantCode;
        public String Date;
        public int Hour;
        public int TransactionCount;
        public decimal TotalSalesAfterTax;
        public decimal TotalSalesBeforeTax;
        public decimal TotalTax;
    }
}
