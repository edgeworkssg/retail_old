using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Drawing.Printing;
using CrystalDecisions.CrystalReports.Engine;
using SubSonic;
using PowerPOS;


namespace PowerWeb.Reports
{
    public partial class Print : System.Web.UI.Page
    {
        public static string maxid;
        public static string lastYear;

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                exlbl.Text = "";
            }

        }

        protected void printbtn_Click(object sender, EventArgs e)
        {
            
            string checkrcpt = "select count(orderhdrid) from orderhdr where userfld5='" + receipttxt.Text + "'";
            int count=Convert.ToInt32(SubSonic.DataService.ExecuteScalar(new QueryCommand(checkrcpt)));

            if (count == 1)
            {
                Process();
            }
            else
                exlbl.Text = "This Receipt doesn't exist!";
        }

        private void Process()
        {
            string testPrinterName = string.Empty;
            try
            {
                DateTime today = DateTime.Now;
                string twodigityr = today.ToString("yy");

                //create document number
                string maxidfetch = "select right(MAX(isnull(userfld1,'00000000')),8) from OrderHdr";
                maxid = DataService.ExecuteScalar(new QueryCommand(maxidfetch)).ToString();

                //check year digit from last document
                string sql = "select left(MAX(isnull(userfld1,'0')),2) from OrderHdr";
                lastYear = DataService.ExecuteScalar(new QueryCommand(sql)).ToString();
                //Logger.writeLog(maxid + "," + lastYear);
                //check if the year has changed, if it is, reset document number
                if(twodigityr != lastYear)
                    maxid = "00000000";

                int numOfCopies;

                if (noctxt.Text != "" && Int32.TryParse(noctxt.Text, out numOfCopies))
                {
                    //increase maxid
                    //maxid = (int.Parse(maxid) + 1).ToString();

                    string insertprintdata = "Declare @Userfld1 varchar(20); Select @Userfld1=Userfld1 from OrderHdr where OrderHdrID='" + receipttxt.Text + "'" +
                        " If @Userfld1 is null begin Update OrderHdr set userfld1='" + (twodigityr + maxid) + "'+1 ,userfld2=" + numOfCopies + ",ModifiedOn='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where OrderHdrID='" + receipttxt.Text + "'" +
                    " END Else Begin Update  OrderHdr set userfld2=userfld2+" + numOfCopies + ",ModifiedOn='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where OrderHdrID='" + receipttxt.Text + "'" +
                    " END";

                    DataService.ExecuteQuery(new QueryCommand(insertprintdata));

                    string docfetch = "select userfld1 from orderhdr where userfld5='" + receipttxt.Text + "'";
                    string docnumber = DataService.ExecuteScalar(new QueryCommand(docfetch)).ToString();
                    string encryptdocno = PowerPOS.UserController.EncryptData(docnumber);
                    //string modifiedencrypted = encryptdocno.Substring(0, 10);
                    string firstnumber = encryptdocno.Substring(0, encryptdocno.Length-4);
                    string secondnumber = encryptdocno.Substring(encryptdocno.Length - 4,4);

                    string insertencrypted = " update orderhdr set userfld3='" + encryptdocno + "' where userfld5='" + receipttxt.Text + "'";
                    DataService.ExecuteQuery(new QueryCommand(insertencrypted));


                    //Logger.writeLog(insertencrypted);
                    //Create report document
                    ReportDocument crystalReport = new ReportDocument();

                    //Load crystal report made in design view
                    Logger.writeLog(Server.MapPath("~/Reports/FormTemplate.rpt"));
                    crystalReport.Load(Server.MapPath("~/Reports/FormTemplate.rpt"));

                    //Provide parameter values
                    crystalReport.SetParameterValue("inputNumber", docnumber);
                    crystalReport.SetParameterValue("ConfNo", firstnumber);
                    crystalReport.SetParameterValue("Confno2", secondnumber);
                    //CrystalReportViewer1.ReportSource = crystalReport;
                    
                    //crystalReport.PrintOptions.PrinterName = AppSetting.GetSetting(AppSetting.SettingsName.Print.PrinterNameForm);//"\\\\server\\Epson EPL-6200L";
                    //Logger.writeLog("GetDefaultPrinter");
                    crystalReport.PrintOptions.PrinterName = GetDefaultPrinter();
                    testPrinterName = crystalReport.PrintOptions.PrinterName;
                    //Logger.writeLog(testPrinterName);
                    crystalReport.PrintToPrinter(numOfCopies, true, 0, 0);
                }
                //clear the form
                receipttxt.Text = "";
                noctxt.Text = "";
                exlbl.Text = "<p>Default Printer Name: [" + testPrinterName + "]</p><p>Print Status: Print Successful.</p>";
                
            }
            catch (Exception ex)
            {
                Logger.writeLog("Printer : " + testPrinterName + ", Msg: " + ex.Message);
                exlbl.Text = "<p>Default Printer Name: [" + testPrinterName + "]</p><p>Error Message: [" + ex.Message + "]</p><p>Error Stack: [" + ex.StackTrace + "]</p>";
                exlbl.Visible = true;
            }
        }

        private string GetDefaultPrinter()
        {
            if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.SICC.DefaultPrinterName)))
            {

                PrinterSettings settings = new PrinterSettings();
                foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                    settings.PrinterName = printer;
                    if (settings.IsDefaultPrinter)
                        return printer;
                }
            }
            else
            {
                return AppSetting.GetSetting(AppSetting.SettingsName.SICC.DefaultPrinterName);
            }
            return string.Empty;
        }
    }
}
