using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Serialization;


namespace WincorTest
{
   
    
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string status ="";
            
            Integration.Integration integration = new Integration.Integration();
            integration.Url = "http://Localhost:2233/Synchronization/Integration.asmx";
            string salesdata = "{R2240,2014-12-04 11:39:00,I00000001,1,22.0000,0,0,22}";
            string paymentdata = "{cod,22}";
            status = integration.SendSales(salesdata, "9", "Website", paymentdata);
            /*{
                MessageBox.Show("Success");
            }
            else
            {*/
                MessageBox.Show("Error" + status);
            /*}*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string status;
            Integration.Integration integration = new Integration.Integration();
            //integration.Url = "http://edgeworks.dyndns-ip.com/auras-test/Synchronization/Integration.asmx";
            integration.Url = "http://localhost:2234/Synchronization/Integration.asmx";
            status = "";
            try
            {
                bool res = integration.AddProduct("1001", "Test002-M-Red", "Test002", "Test002", "Test002",
                    true, 12, 1, "", "", "Inclusive", "M0001", "Test002", "M", "Red", "", "", "", "", true, true, true, out status);
            }
            catch (Exception ex)
            {
                MessageBox.Show(status + " " + ex.Message);
            }
            //status = integration. (salesdata, "9", "Website", paymentdata);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string status = "";
            string URL = "http://auras.techatrium.com/index.php?route=api/checkout/AddSale";

            string salesdata = "{15042100490002,2015-4-20 11:49:00,1009,1,100.0000,0,0,100}";
            string paymentdata = "{CASH,100.0000,}";
            string postData = "";

            postData += "orderdata=" + salesdata + "&";
            postData += "membershipno=51&";
            //postData += "cashier=POS&";
            postData += "paymentdata="+paymentdata;
            /*var result = new
            {
                salesData = salesdata,
                membershipNo = "9",
                cashier = "POS",
                paymentData = paymentdata
            };*/

            byte[] data = Encoding.ASCII.GetBytes(postData);
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Console.Out.WriteLine(responseString);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(ex.Message);
            }

            /*Integration.Integration integration = new Integration.Integration();
            integration.Url = "http://Localhost:2233/Synchronization/Integration.asmx";*/
            
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string status = "";
            string URL = "http://auras.techatrium.com/index.php?route=api/product/updateProductVariantInventory";

            //string salesdata = "{R2240,2015-12-20 11:39:00,1006,1,22.0000,0,0,22}";
            //string paymentdata = "{cod,22}";
            /*var result = new
            {
                productVariantID = "994",
                quantity = 10,
            };*/
            //string postdata = "";
            var postData = "productVariantID=1008";
            postData += "&quantity=15";
            var data = Encoding.ASCII.GetBytes(postData);
            var request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }


            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Console.Out.WriteLine(responseString);
                //responseReader.Close();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string status = "";
            string URL = "http://auras.techatrium.com/index.php?route=api/account/getListCustomerRedeemableGift";

            //string postdata = "";
            var postData = "membershipno=96";
            //postData += "&quantity=15";
            var data = Encoding.ASCII.GetBytes(postData);
            var request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //JavaScriptSerializer js = new JavaScriptSerializer();
                //DataTable data1 = js.Deserialize<DataTable>(responseString);
                List<GiftItem> dataPODetColl = new JavaScriptSerializer().Deserialize<List<GiftItem>>(responseString);
                MessageBox.Show(dataPODetColl.Count.ToString());
                //responseReader.Close();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //bool result = false;
            try
            {
               // string URL = "http://auras.techatrium.com/index.php?route=api/account/getListCustomerRedeemableGift";
                //string settingURL = AppSetting.GetSetting(AppSetting.SettingsName.Auras.WebURL);
                string URL = "http://auras.techatrium.com/index.php?route=api/checkout/validateCoupon";

                var postData = "coupon_code=18Htfn&";
                postData += "total=100";

                var data = Encoding.ASCII.GetBytes(postData);
                var request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentType = "application/json";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                MessageBox.Show(responseString);
                var result = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(responseString.ToString());
                //bool isSuccess = false;
                if (result.ContainsKey("voucheramount"))
                {
                    MessageBox.Show(result["voucheramount"]);
                }
                
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

            string status;
            Integration.Integration integration = new Integration.Integration();
            //integration.Url = "http://edgeworks.dyndns-ip.com/auras-test/Synchronization/Integration.asmx";
            integration.Url = "http://equipweb.biz/lapiz/Synchronization/Integration.asmx";
            status = "";
            try
            {
                DateTime dt = new DateTime(2016,2,5);
                //string tmp = integration.DownloadMemberDataNewOrUpdated(dt);
                //richTextBox1.Text = tmp;
                //richTextBox1.WordWrap = true;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://equipweb.biz/lapiz/Synchronization/Integration.asmx?op=DownloadMemberDataNewOrUpdated");
                request.Headers.Add(@"SOAP:Action");
                request.Method = "POST";
                request.ContentType = "text/xml;charset=\"utf-8\"";
                
                request.Accept = "text/xml";
                
                string sSOAPRequest = @"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body><DownloadMemberDataNewOrUpdated xmlns=""http://www.edgeworks.com.sg/""><SinceLastTimestamp>" + "2016-02-05" + "</SinceLastTimestamp></DownloadMemberDataNewOrUpdated></soap:Body></soap:Envelope>";
                Stream requestStream = request.GetRequestStream();
                StreamWriter streamWriter = new StreamWriter(requestStream, Encoding.ASCII);
                streamWriter.Write(sSOAPRequest.ToString());
                streamWriter.Close();
                HttpWebResponse wr = (HttpWebResponse)request.GetResponse();
                StreamReader srd = new StreamReader(wr.GetResponseStream()); 
                string resulXmlFromWebService = srd.ReadToEnd();

                richTextBox1.Text = resulXmlFromWebService;
                //request.send
                
                
                /*using (var response = (HttpWebResponse)request.GetResponse())
                {

                }*/

            }
            catch (Exception ex)
            {
                MessageBox.Show(status + " " + ex.Message);
            }
        }
    }

    public class GiftItem
    {
        public int voucher_id { get; set; }
        public int gift_id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public int points { get; set; }
        public string type { get; set; }
    }
}
