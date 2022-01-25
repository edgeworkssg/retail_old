<%@ WebHandler Language="C#" Class="UpdateCompanySetting" %>

using System;
using System.Web;
using System.Linq;
using SubSonic;
using PowerPOS;
using Edgeworks.Model;

public class UpdateCompanySetting : IHttpHandler {
    
    public void ProcessRequest (HttpContext context)
    {
        ResultModel result = new ResultModel();
        try
        {
            string authHeader = context.Request.Headers["Authorization"] + "";
            //authHeader = authHeader.ToLower().Replace("bearer ", "");
            if (string.IsNullOrEmpty(authHeader))
                throw new AuthorizationException("Invalid authorization");

            var auth_companyID = UserController.DecryptData(authHeader).ConvertToGuid();
            if (auth_companyID == Guid.Empty)
                throw new AuthorizationException("Invalid authorization");

            HttpContext.Current.Response.ContentType = "application/json;charset=utf-8";
            System.IO.Stream postData = context.Request.InputStream;
            System.IO.StreamReader sRead = new System.IO.StreamReader(postData);
            string jsonInput = sRead.ReadToEnd();
            sRead.Close();

            var input = jsonInput.ConvertFromJSON<System.Collections.Generic.Dictionary<string, object>>();
            if (input == null)
                throw new ValidationException("Input cannot be empty");

            Guid companyID = Guid.Empty;
            if (input.ContainsKey("CompanyID"))
                companyID = (input["CompanyID"] + "").ConvertToGuid();
            if (companyID == Guid.Empty)
                throw new ValidationException("Invalid company ID");
            if (companyID != auth_companyID)
                throw new AuthorizationException("Invalid authorization");


            string companyName = "";
            if (input.ContainsKey("CompanyName"))
                companyName = input["CompanyName"] + "";
            if (string.IsNullOrEmpty(companyName))
                throw new ValidationException("Invalid Company name");

            
            string customerMasterURL = "";
            if (input.ContainsKey("CustomerMasterURL"))
                customerMasterURL = input["CustomerMasterURL"] + "";
            if (string.IsNullOrEmpty(customerMasterURL))
                throw new ValidationException("Invalid Customer Master URL");


            Guid apiCallerID = Guid.Empty;
            if (input.ContainsKey("APICallerID"))
                apiCallerID = (input["APICallerID"] + "").ConvertToGuid();            
            if (apiCallerID == Guid.Empty)
                throw new ValidationException("Invalid API ID");


            string apiPrivateKey = "";
            if (input.ContainsKey("APIPrivateKey"))
                apiPrivateKey = input["APIPrivateKey"] + "";
            if (string.IsNullOrEmpty(apiPrivateKey))
                throw new ValidationException("Invalid API Private Key");


            string defaultAccountID = "";
            if (input.ContainsKey("DefaultAccountID"))
                defaultAccountID = input["DefaultAccountID"] + "";
            if (string.IsNullOrEmpty(defaultAccountID))
                throw new ValidationException("Invalid Account ID");


            string defaultAccountEmail = "";
            if (input.ContainsKey("DefaultAccountEmail"))
                defaultAccountEmail = input["DefaultAccountEmail"] + "";
            if (string.IsNullOrEmpty(defaultAccountEmail))
                throw new ValidationException("Invalid Email");


            string defaultAccountPassword = "";
            if (input.ContainsKey("DefaultAccountPassword"))
                defaultAccountPassword = input["DefaultAccountPassword"] + "";
            if (string.IsNullOrEmpty(defaultAccountPassword))
                throw new ValidationException("Invalid Password");
            

            QueryCommandCollection qmc = new QueryCommandCollection();

            var comp = new Company(companyID);
            if (comp.IsNew)
            {
                qmc.Add(new QueryCommand("DELETE Company"));
                comp = new Company();
                comp.CompanyID = companyID;
                comp.ReceiptName = companyName;
            }
            comp.CompanyName = companyName;
            comp.Deleted = false;
            if (comp.IsNew)
                qmc.Add(comp.GetInsertCommand("API"));
            else
                qmc.Add(comp.GetUpdateCommand("API"));

            var user = new PowerPOS.UserMst(UserMst.UserColumns.Email, defaultAccountEmail);
            if (user.IsNew)
            {
                var userGroupColl = new UserGroupController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false).ToList();
                UserGroup ug = userGroupColl.Where(o => o.GroupID == 3).FirstOrDefault();
                if (ug == null)
                    ug = userGroupColl.FirstOrDefault();

                if (ug == null)
                {
                    ug = new UserGroup();
                    ug.GroupName = "Admin";
                    ug.Deleted = false;
                    ug.Save("API");
                }

                user.GroupName = ug.GroupID;
                user.UserName = defaultAccountID;
                user.Email = defaultAccountEmail;
                user.DepartmentID = 0;
                user.SalesPersonGroupID = 0;
                user.IsASalesPerson = false;
                user.Userfld1 = "ALL";
                user.Password = defaultAccountPassword;
            }
            user.Deleted = false;
            if (user.IsNew)
                qmc.Add(user.GetInsertCommand("API"));
            else
                qmc.Add(user.GetUpdateCommand("API"));

            DataService.ExecuteTransaction(qmc);

            AppSetting.SetSetting("CompanyID", companyID.ToString());
            AppSetting.SetSetting("CustomerMasterURL", customerMasterURL);
            AppSetting.SetSetting("APICallerID", apiCallerID.ToString());
            AppSetting.SetSetting("APIPrivateKey", apiPrivateKey);

            result.Status = StatusCode.SUCCESS.ToString();
            result.ResultCode = (int)StatusCode.SUCCESS;
        }
        catch (CustomException ex)
        {
            result.Status = ex.Code.ToString();
            result.ResultCode = (int)ex.Code;
            result.Message = ex.Message;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            result.Status = StatusCode.ERROR.ToString();
            result.ResultCode = (int)StatusCode.ERROR;
            result.Message = ex.Message;
        }

        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Write(result.ConvertToJSON());
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}

