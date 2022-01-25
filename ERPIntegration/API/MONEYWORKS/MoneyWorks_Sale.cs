using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PowerPOS;
using System.IO;
using System.Security.Permissions;
using System.Data;
using SubSonic;
using System.Xml;
using System.Net;

namespace ERPIntegration.API.MONEYWORKS
{
    public static class MoneyWorks_Sale
    {
        public static bool DoSendSaleData(out string status)
        {
            DateTime StartDate, EndDate;
            bool result = false;

            string strStartDate = ConfigurationManager.AppSettings["StartDate"];
            string strEndDate = ConfigurationManager.AppSettings["EndDate"];

            if (!DateTime.TryParse(strStartDate, out StartDate) || string.IsNullOrEmpty(strStartDate))
            {
                DateTime startDate = DateTime.Today.AddDays(-1).Add(TimeSpan.Parse("00:00:00"));
                strStartDate = startDate.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime.TryParse(strStartDate, out StartDate);
            }

            if (!DateTime.TryParse(strEndDate, out EndDate) || string.IsNullOrEmpty(strEndDate))
            {
                DateTime endDate = DateTime.Today.AddDays(-1).Add(TimeSpan.Parse("23:59:59"));
                strEndDate = endDate.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime.TryParse(strEndDate, out EndDate);
            }

            Console.WriteLine("Send Data {0}-{1}", StartDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Logger.writeLog(string.Format("Send Data {0}-{1}", StartDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate.ToString("yyyy-MM-dd HH:mm:ss")));


            string filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GM.xml";

            Console.WriteLine("Build file xml");
            Logger.writeLog("Build file xml");
            if (SendDataIntegration(StartDate, EndDate, out status))
            {
                Console.WriteLine("Send file xml");
                Logger.writeLog("Send file xml");
                if (File.Exists(filePath))
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    FileIOPermission filePermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, filePath);

                    if (PostFiles(filePath, out status))
                    {
                        result = true;
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        Console.WriteLine(status);
                        Logger.writeLog("Status send to MoneyWorks POS System:" + status);
                    }
                }
            }
            else
            {
                Console.WriteLine(status);
                Logger.writeLog(status);
            }
            return result;

        }

        public static bool SendDataIntegration(DateTime StartTime, DateTime EndTime, out string status)
        {
            status = "";

            try
            {
                string query = @"
                select 
                    oh.OrderDate, oh.pointofsaleid as POSID, oh.userfld5 as ReceiptNo, oh.userfld5 as OrderRefNo,oh.NettAmount,oh.GrossAmount,
                    po.PointOfSaleName,po.userfld7 as detilaccount,po.userfld8 as namecode,
                    i.ItemNo, i.ItemName ,
                    isnull(nullif(oh.isvoided,' '), od.isvoided) as IsVoided,oh.ModifiedOn,
                    od.OrderDetID,od.Amount,od.UnitPrice,od.userfloat4 as DiscountItem,od.GrossSales, od.Quantity, ISNULL(NULLIF(od.userfld4,' '),'') as LineInfo,
                    od.GSTAmount,ISNULL(nullif(od.userfld13,' '),'') as ReceiptRefund,
                    rd.PaymentType
                from orderdet od
                inner join orderhdr oh on od.OrderHdrID = oh.OrderHdrID
                inner join item i on od.ItemNo = i.ItemNo
                inner join pointofsale po on po.pointofsaleid = oh.PointOfSaleID
                left join (
                    select receipthdrid, MAX(paymenttype) PaymentType from receiptdet group by ReceiptHdrID
                )rd on oh.OrderHdrID = rd.receipthdrid
                where oh.modifiedon between '{0}' and '{1}'
                    and i.CategoryName != 'SYSTEM'
                    and oh.PointOfSaleId= {2}
                    order by oh.modifiedon asc,oh.OrderRefNo
                ";
                //ISNULL(NULLIF(i.Barcode,' '),i.ItemNo) as 
                query = string.Format(query, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"), ConfigurationManager.AppSettings["PosID"]);
                //query = @"SELECT PaymentType FROM receiptdet group by PaymentType";

                DataTable dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];
                string filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GM.xml";

                if (dt.Rows.Count > 0)
                {
                    //if (File.Exists(filePath))
                    //{
                    //    File.SetAttributes(filePath, FileAttributes.Normal);
                    //    FileIOPermission filePermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, filePath);

                    XmlWriterSettings settings = new XmlWriterSettings();

                    settings.OmitXmlDeclaration = false;
                    settings.Indent = true;
                    settings.IndentChars = "\t";
                    settings.Encoding = Encoding.UTF8;


                    //using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    //{
                    using (XmlWriter writer = XmlWriter.Create(filePath, settings))
                    {

                        writer.WriteStartDocument();
                        writer.WriteStartElement("table");
                        writer.WriteAttributeString("name", "Transaction");

                        string currentRefNo = "";
                        int currentIndex = 0;
                        float totalItemAmount = 0;
                        decimal saleAmount = 0;
                        string isVoided = "0";
                        while (dt.Rows.Count > currentIndex)
                        {
                            isVoided = dt.Rows[currentIndex]["IsVoided"].ToString();
                            if (isVoided == "True")
                            {
                                currentIndex += 1;
                            }
                            else
                                if (isVoided == "False")
                                {
                                    currentRefNo = dt.Rows[currentIndex]["OrderRefNo"].ToString();
                                    writer.WriteStartElement("transaction");
                                    totalItemAmount = 0;

                                    DateTime orderDate = new DateTime();
                                    if (!DateTime.TryParse(dt.Rows[currentIndex]["OrderDate"].ToString(), out orderDate))
                                    {
                                        orderDate = DateTime.Now;
                                    }

                                    writer.WriteElementString("ourref", dt.Rows[currentIndex]["OrderRefNo"].ToString());
                                    writer.WriteElementString("transdate", orderDate.ToString("yyyyMMdd"));
                                    writer.WriteElementString("duedate", orderDate.ToString("yyyyMMdd"));

                                    //writer.WriteElementString("description", "Sale at " + dt.Rows[currentIndex]["PointOfSaleName"].ToString());
                                    writer.WriteStartElement("description");
                                    writer.WriteCData("Sale at " + dt.Rows[currentIndex]["PointOfSaleName"].ToString());
                                    writer.WriteEndElement();

                                    if (isVoided == "False")
                                        writer.WriteElementString("contra", ConfigurationManager.AppSettings["Contra"]);
                                    else
                                        writer.WriteElementString("contra", ConfigurationManager.AppSettings["ContraVoid"]);
                                    writer.WriteElementString("namecode", dt.Rows[currentIndex]["namecode"].ToString());
                                    writer.WriteElementString("paymentmethod", PaymentMethodStringToCode(dt.Rows[currentIndex]["PaymentType"].ToString()));
                                    if (isVoided == "False")
                                        writer.WriteElementString("type", "DII");
                                    else
                                        writer.WriteElementString("type", "CII");

                                    //add SalesPerson
                                    writer.WriteElementString("salesperson", "Retail");

                                    writer.WriteStartElement("subfile");
                                    writer.WriteAttributeString("name", "Detail");

                                    writer.WriteStartElement("detail");
                                    writer.WriteElementString("detail.account", dt.Rows[currentIndex]["detilaccount"].ToString());
                                    writer.WriteElementString("detail.stockcode", dt.Rows[currentIndex]["ItemNo"].ToString());

                                    //add TC = 'G'
                                    writer.WriteElementString("detail.taxcode", "G");

                                    //writer.WriteElementString("detail.description", dt.Rows[currentIndex]["ItemName"].ToString());
                                    writer.WriteStartElement("detail.description");
                                    writer.WriteCData(dt.Rows[currentIndex]["ItemName"].ToString());
                                    writer.WriteEndElement();

                                    //add TC = 'G'
                                    writer.WriteElementString("detail.taxcode", "G");

                                    writer.WriteElementString("detail.stocklocation", ConfigurationManager.AppSettings["Location"]);
                                    writer.WriteElementString("detail.stockqty", dt.Rows[currentIndex]["Quantity"].ToString());

                                    if (isVoided == "False")
                                    {

                                    }
                                    else
                                    {
                                        decimal amountnet = 0;
                                        if (!decimal.TryParse(dt.Rows[currentIndex]["Amount"].ToString(), out amountnet))
                                        {
                                            throw new Exception("Error. Cannot Parse Amount");
                                        }
                                        amountnet *= -1;
                                        writer.WriteElementString("detail.net", amountnet.ToString());
                                        writer.WriteElementString("detail.gross", amountnet.ToString());
                                    }

                                    decimal unitPrice = 0;
                                    if (!decimal.TryParse(dt.Rows[currentIndex]["UnitPrice"].ToString(), out unitPrice))
                                    {
                                        throw new Exception("Error. Cannot Parse Unit Price");
                                    }

                                    decimal amountAsGross = 0;
                                    if (!decimal.TryParse(dt.Rows[currentIndex]["Amount"].ToString(), out amountAsGross))
                                    {
                                        throw new Exception("Error. Cannot Parse Amount");
                                    }

                                    decimal gstAmount = 0;
                                    if (!decimal.TryParse(dt.Rows[currentIndex]["GSTAmount"].ToString(), out gstAmount))
                                    {
                                        throw new Exception("Error. Cannot Parse GST Amount");
                                    }

                                    writer.WriteElementString("detail.unitprice", unitPrice.ToString());
                                    writer.WriteElementString("detail.gross", amountAsGross.ToString());
                                    writer.WriteElementString("detail.tax", gstAmount.ToString());
                                    decimal netVal = amountAsGross - gstAmount;
                                    writer.WriteElementString("detail.net", netVal.ToString());

                                    saleAmount = 0;
                                    if (!decimal.TryParse(dt.Rows[currentIndex]["Amount"].ToString(), out saleAmount))
                                    {
                                        throw new Exception("Error. Cannot Parse Amount");
                                    }
                                    if (isVoided == "True")
                                        saleAmount *= -1;
                                    totalItemAmount += (float)saleAmount;
                                    writer.WriteEndElement(); // close detail element

                                    int nextIndex = currentIndex + 1;
                                    if (dt.Rows.Count > nextIndex)
                                    {
                                        string nextRefNo = dt.Rows[nextIndex]["OrderRefNo"].ToString();
                                        while ((nextRefNo == currentRefNo) && (dt.Rows.Count > nextIndex))
                                        {
                                            // updete currentIndex
                                            currentIndex = nextIndex;
                                            currentRefNo = dt.Rows[currentIndex]["OrderRefNo"].ToString();
                                            // add next element
                                            writer.WriteStartElement("detail");
                                            writer.WriteElementString("detail.account", dt.Rows[nextIndex]["detilaccount"].ToString());
                                            writer.WriteElementString("detail.stockcode", dt.Rows[nextIndex]["ItemNo"].ToString());

                                            //writer.WriteElementString("detail.description", dt.Rows[nextIndex]["ItemName"].ToString());
                                            writer.WriteStartElement("detail.description");
                                            writer.WriteCData(dt.Rows[nextIndex]["ItemName"].ToString());
                                            writer.WriteEndElement();

                                            //add TC = 'G'
                                            writer.WriteElementString("detail.taxcode", "G");

                                            writer.WriteElementString("detail.stocklocation", ConfigurationManager.AppSettings["Location"]);
                                            writer.WriteElementString("detail.stockqty", dt.Rows[nextIndex]["Quantity"].ToString());
                                            if (isVoided == "False")
                                            {

                                            }
                                            else
                                            {
                                                decimal amountnet = 0;
                                                if (!decimal.TryParse(dt.Rows[nextIndex]["Amount"].ToString(), out amountnet))
                                                {
                                                    throw new Exception("Error. Cannot Parse Amount");
                                                }
                                                amountnet *= -1;
                                                writer.WriteElementString("detail.net", amountnet.ToString());
                                                writer.WriteElementString("detail.gross", amountnet.ToString());
                                            }

                                            unitPrice = 0;
                                            if (!decimal.TryParse(dt.Rows[currentIndex]["UnitPrice"].ToString(), out unitPrice))
                                            {
                                                throw new Exception("Error. Cannot Parse Unit Price");
                                            }

                                            amountAsGross = 0;
                                            if (!decimal.TryParse(dt.Rows[currentIndex]["Amount"].ToString(), out amountAsGross))
                                            {
                                                throw new Exception("Error. Cannot Parse Amount");
                                            }

                                            gstAmount = 0;
                                            if (!decimal.TryParse(dt.Rows[currentIndex]["GSTAmount"].ToString(), out gstAmount))
                                            {
                                                throw new Exception("Error. Cannot Parse GST Amount");
                                            }

                                            writer.WriteElementString("detail.unitprice", unitPrice.ToString());
                                            writer.WriteElementString("detail.gross", amountAsGross.ToString());
                                            writer.WriteElementString("detail.tax", gstAmount.ToString());
                                            netVal = amountAsGross - gstAmount;
                                            writer.WriteElementString("detail.net", netVal.ToString());

                                            saleAmount = 0;
                                            if (!decimal.TryParse(dt.Rows[nextIndex]["Amount"].ToString(), out saleAmount))
                                            {
                                                throw new Exception("Error. Cannot Parse Amount");
                                            }
                                            if (isVoided == "True")
                                                saleAmount *= -1;
                                            totalItemAmount += (float)saleAmount;
                                            writer.WriteEndElement(); // close detail element

                                            nextIndex = currentIndex + 1;
                                            if (dt.Rows.Count > nextIndex)
                                                nextRefNo = dt.Rows[nextIndex]["OrderRefNo"].ToString();
                                            else
                                                nextRefNo = "";
                                        }
                                        // end of current transaction
                                        // close sub file element, close transaction elemen
                                        writer.WriteEndElement();
                                        writer.WriteElementString("gross", totalItemAmount.ToString());
                                        writer.WriteEndElement();
                                        // updete currentIndex
                                        currentIndex = nextIndex;
                                    }
                                    else
                                    {
                                        // end of transaction records, close table element

                                        writer.WriteEndElement();
                                        writer.WriteElementString("gross", totalItemAmount.ToString());
                                        // updete currentIndex
                                        currentIndex = nextIndex;
                                    }

                                }

                        }

                        //end of transaction tag
                        writer.WriteEndDocument();

                        XmlDocument doc = new XmlDocument();
                        doc.Save(writer);

                    }
                    //}
                    //}
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error send to MoneyWorks System:" + ex.Message);
                status = ex.Message;
                return false;
            }

        }

        public static bool PostFiles(string fileUrl, out string status)
        {
            bool result = false;
            status = "";
            string xmlString = System.IO.File.ReadAllText(fileUrl);
            string baseUrl = "";
            if (ConfigurationManager.AppSettings["SSL"] == "true")
                baseUrl = "https://";
            else
                baseUrl = "http://";

            baseUrl += ConfigurationManager.AppSettings["Server"] + ":"
                + ConfigurationManager.AppSettings["Port"] + "/Rest/"
                + ConfigurationManager.AppSettings["Username"] + ":"
                + ConfigurationManager.AppSettings["Password"] + "@"
                + System.Uri.EscapeUriString(ConfigurationManager.AppSettings["Document"])
                + "/import"
                ;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(xmlString);
            request.ContentType = "application/xml";
            request.ContentLength = bytes.Length;
            request.Method = "POST";

            HttpWebResponse response;
            try
            {
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    status = responseStr;
                    result = true;
                }
                else
                    status = "";
            }
            catch (Exception e)
            {
                status = e.Message;
            }
            return result;
        }

        public static String PaymentMethodStringToCode(String payStr)
        {
            string[] arr1 = ConfigurationManager.AppSettings["PaymentCash"].Split(',');
            string[] arr2 = ConfigurationManager.AppSettings["PaymentCheque"].Split(',');
            string[] arr3 = ConfigurationManager.AppSettings["PaymentElectronic"].Split(',');
            string[] arr4 = ConfigurationManager.AppSettings["PaymentCC"].Split(',');
            string[] arr5 = ConfigurationManager.AppSettings["PaymentReserved5"].Split(',');
            string[] arr6 = ConfigurationManager.AppSettings["PaymentReserved6"].Split(',');
            string[] arr7 = ConfigurationManager.AppSettings["PaymentReserved7"].Split(',');

            if (payStr == "")
                return "0";
            else if (arr1.Contains<string>(payStr))
                return "1";
            else if (arr2.Contains<string>(payStr))
                return "2";
            else if (arr3.Contains<string>(payStr))
                return "3";
            else if (arr4.Contains<string>(payStr))
                return "4";
            else if (arr5.Contains<string>(payStr))
                return "5";
            else if (arr6.Contains<string>(payStr))
                return "6";
            else
                return "7";
        }
    }
}
