using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.IO;

namespace ERPIntegration.API.SYNERGIX
{
    public static class Synergix_http
    {
        public enum CommandType { POST, GET };        

        public static bool callAPI(CommandType cmd,string restPath, string bodyRequest,out string status)
        {
            string BASE_URL = ConfigurationManager.AppSettings["synergix_server"];
            string TOKEN = ConfigurationManager.AppSettings["synergix_token"];

            bool resultBool = false;
            string callUrl= BASE_URL + restPath + "?securityToken=" + TOKEN;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(callUrl);
            if (cmd == CommandType.GET)
            {
                request.Method = "GET";
            }
            else if (cmd == CommandType.POST)
            {
                request.Method = "POST";
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(bodyRequest);
                request.ContentType = "application/json";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            else
            {
                status = "INVALID COMMAND";
                return false;
            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    status = responseStr;                    
                    resultBool = true;
                }
                else
                {
                    status = "";
                    resultBool = false;
                }
            }
            catch (Exception e)
            {
                status = e.Message;
                resultBool = false;
            }
            return resultBool;
        }


    }
}
