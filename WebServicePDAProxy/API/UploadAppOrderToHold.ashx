<%@ WebHandler Language="C#" Class="UploadAppOrderToHold" %>

using System;
using System.Web;

public class UploadAppOrderToHold : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        HttpContext.Current.Response.ContentType = "application/json;charset=utf-8";
        System.IO.Stream postData = context.Request.InputStream;

        #region *) Authorization

        var authKey = System.Configuration.ConfigurationManager.AppSettings["UploadOrder_AuthorizationKey"] + "";
        if (!string.IsNullOrEmpty(authKey))
        {
            string authHeader = context.Request.Headers["Authorization"] + "";
            if (authHeader.StartsWith("Bearer "))
                authHeader = authHeader.Replace("Bearer ", "");
            else
                authHeader = "";
            if (authKey != authHeader)
            {
                var resultData = new
                {
                    resultCode = -1,
                    resultMessage = "Error : Invalid authorization key"
                };
                var resultStr = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(resultData);
                context.Response.Write(resultStr);
                return;
            }
        }   
        
        #endregion

        System.IO.StreamReader sRead = new System.IO.StreamReader(postData);
        string jsonInput = sRead.ReadToEnd();
        sRead.Close();

        if (string.IsNullOrEmpty(jsonInput))
        {
            var resultData = new
            {
                resultCode = -1,
                resultMessage = "Error : Invalid input"
            };
            var resultStr = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(resultData);
            context.Response.Write(resultStr);
            return;             
        }
        
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        WebServicePDAProxy.POSIntegration.AutoLogin();
        context.Response.Write(WebServicePDAProxy.POSIntegration.UploadAppOrderToHold(jsonInput));             
    }
 
    public bool IsReusable {
        get { 
            return false;
        }
    }

}