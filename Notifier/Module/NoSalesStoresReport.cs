using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;
using System.Collections;
using Notifier.Properties;
using System.IO;
using System.Reflection;

namespace Notifier.Module
{
    public class NoSalesStoresReport: MailBase
    {
        public override string ModuleReport
        {
            get
            {
                return "NoSalesStoresReport";
            }
        }
        public override string ReportFilename
        {
            get { return "NoSalesStores.rpt"; }
        }

        public DateTime Date { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public int PointOfSale { get; set; }

        public NoSalesStoresReport(): base()
        {
                
        }

        private static NoSalesStoresReport instance;
        public static NoSalesStoresReport Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NoSalesStoresReport();
                }

                return instance;
            }
        }

        private DataTable GetNoSalesStores()
        {
            DataTable dt = new DataTable();
            try 
            {          
                string SQL = @"
                            DECLARE @Date DATETIME; 
                            SET @Date = GetDate();  

                            SELECT Outlet.OutletName,PointOfSale.userfld2  AS  Region ,PointOfSale.userfld1 AS Country      
                            FROM Outlet                  
                            INNER JOIN PointOfSale ON PointOfSale.OutletName = Outlet.OutletName     
                            WHERE Outlet.OutletName NOT IN    
                            (
	                            SELECT  DISTINCT PointOfSale.OutletName         
	                            FROM OrderDet 
		                            INNER JOIN OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID    
		                            INNER JOIN PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID 
	                            WHERE OrderDet.OrderDetDate  BETWEEN  DATEADD(DD,-3,@Date)  AND   @Date   
	                            AND ISNULL(OrderHdr.IsVoided, 0) <> 1     
	                            AND ISNULL(OrderDet.IsVoided, 0) <> 1    
                            ) 
                            Order by Country
                            ";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                dt = DataService.GetDataSet(cmd).Tables[0];            
            }
            catch (Exception ex)
            {
                Console.Write("Error in GetNoSalesSores method: "+ex.Message);
            }    
            return dt;
        }

        private bool ExportToExcel(DataTable dt, string filepath)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                ArrayList headerName = new ArrayList();

                foreach (DataColumn Col in dt.Columns)
                    headerName.Add(Col.ColumnName);

                ArrayList ColumnList = new ArrayList();
                for (int m = 0; m < dt.Columns.Count; m++)
                {
                    ColumnList.Add(m);
                }
                RKLib.ExportData.Export a = new RKLib.ExportData.Export("Win");
                a.ExportDetails(dt, (int[])ColumnList.ToArray(typeof(int)),
                    (string[])headerName.ToArray(typeof(string)),
                    RKLib.ExportData.Export.ExportFormat.Excel, filepath);
                return true;
            }
            return false;
        }

        private bool ISFileBeingUsed(FileInfo fileName)
        {
            FileStream stream = null;
            try
            {
                stream = fileName.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            }
            catch (System.IO.IOException exp)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        public bool VerificationReportData()
        {
            try
            {
                if (Settings.Default.UseCrystalReport)
                {
                    string exportedPath = this.GenerateReport(string.Format(@"{0}\Reports\{1}", Directory.GetParent(Assembly.GetAssembly(this.GetType()).Location), this.ReportFilename), CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<h3>NO SALES STORES REPORT</h3>");
                    sb.AppendLine();
                    sb.AppendLine();
                    if (Settings.Default.UseLocalMailSetting)
                    {
                        this.SentLocalNotificationMail(Settings.Default.ProfitLossReport_MailSubject, true, sb.ToString(), exportedPath);
                    }
                    else
                    {
                        this.Message.Body = sb.ToString();
                        this.Message.Subject = Settings.Default.ProfitLossReport_MailSubject;

                        foreach (string item in this.GetMailReceivers())
                        {
                            this.Message.To.Add(item);
                        }

                        this.SentNotificationMail(true, exportedPath);
                    }
                    return true;
                }
                else
                {
                    DataTable dt = this.GetNoSalesStores();
                    if (dt.Rows != null && dt.Rows.Count > 0)
                    {
                        string fileName = @"\NoSalesStoresReport" + DateTime.Now.ToString("dd-MMM-yyyy-HHmmss") + ".xls";
                        this.ExportToExcel(dt, fileName);
                        string emailBody = "This is a notification email of the stores that have no sales the last 3 days.";
                        string subject = Settings.Default.NoSalesStoresReport_MailSubject;
                        if (Settings.Default.UseLocalMailSetting)
                        {
                            this.SentLocalNotificationMail(subject, true, emailBody, fileName);
                        }
                        else
                        {
                            this.Message.Subject = Settings.Default.NoSalesStoresReport_MailSubject;

                            foreach (string item in this.GetMailReceivers())
                            {
                                this.Message.To.Add(item);
                            }

                            this.SentNotificationMail(subject, true, emailBody, fileName);
                        }
                        FileInfo fileInfo = new FileInfo(fileName);
                        while (true)
                        {
                            if (!ISFileBeingUsed(fileInfo))
                            {
                                File.Delete(fileName);
                                break;
                            }
                            else
                                continue;
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (MailBaseException mbex)
            {
                throw mbex;
            }
            catch (Exception ex)
            {
                throw new NoSalesStoresReportException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name)
                    ,ex);
            }
        }

        public override SortedList<string, string> GetCRParam()
        {
            try
            {
                SortedList<string, string> parameters = new SortedList<string, string>();
                #region *) Get the Filter Value that User Keyed in
                parameters.Add("Date", this.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("Country", this.Country);
                parameters.Add("Region", this.Region);
                parameters.Add("PointOfSale", this.PointOfSale.ToString());
                #endregion
                return parameters;
            }
            catch (Exception ex)
            {
                throw new ProfitLossReportException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }
    }

    [global::System.Serializable]
    public class NoSalesStoresReportException : Exception
    {
        public NoSalesStoresReportException() { }
        public NoSalesStoresReportException(string message) : base(message) { }
        public NoSalesStoresReportException(string message, Exception inner) : base(message, inner) { }
        protected NoSalesStoresReportException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
