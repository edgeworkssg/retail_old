using SubSonic;
using PowerPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using PowerPOSActivation;

namespace WinPowerPOS
{
    partial class frmLoadingScreen : Form
    {

        public bool LicenseChecked;
        string status = "";
        public frmLoadingScreen()
        {
            InitializeComponent();
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void Processor_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            txtStatus.Text = e.UserState.ToString();
        }

        private void Processor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool res = false;
            if (!bool.TryParse(e.Result.ToString(), out res))
                res = false;
            if (res == false)
            {
                MessageBox.Show(status);
                Application.Exit();
            }
            if (!string.IsNullOrEmpty(status))
                MessageBox.Show(status);
            this.Close();
        }

        private void Processor_DoWork(object sender, DoWorkEventArgs e)
        {
            string SQLString;

            Processor.ReportProgress(0, "Checking License");
            
            //SQLString = ""; // Do something here
            //QueryCommand Cmd = new QueryCommand(SQLString);
            //SubSonic.DataService.ExecuteQuery(Cmd);

            #region *) License Checking

            e.Result = true;

            if (!PowerPOSActivation.POSActivationController.CheckProgramValidity(out status))
            {
                e.Result = false;
            }
            else
            {
                try
                {
                    PowerPOSActivation.POSActivationController.UpdatePOSType(
                        UtilityController.GetPOSType(),
                        UtilityController.GetCurrentVersion());
                }
                catch (Exception ex)
                {

                }
            }            


            #endregion

            #region *) POS Update Downloader

            BackgroundWorker updater = new BackgroundWorker();
            updater.DoWork += new DoWorkEventHandler(updater_DoWork);
            updater.RunWorkerCompleted += new RunWorkerCompletedEventHandler(updater_RunWorkerCompleted);
            updater.RunWorkerAsync();

            #endregion

            Processor.ReportProgress(50, "Update Database Structure");
            DbUtilityController.UpdateDBStructure();
            //ApplyPromotionController.viewPromoMasterDetailAny = new ViewPromoMasterDetailAnyCollection().Load().ToDataTable();

            #region Check if old promo Exist
            ApplyPromotionController.hasOldPromo = false;
            PromoCampaignHdrCollection promoCol = new PromoCampaignHdrCollection();
            promoCol.Where(PromoCampaignHdr.Columns.CampaignType, Comparison.NotEquals, "AnyXOffAllItems");
            promoCol.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.LessOrEquals, DateTime.Now);
            promoCol.Where(PromoCampaignHdr.Columns.DateTo, Comparison.GreaterThan, DateTime.Now);
            promoCol.Load();
            if (promoCol.Count > 0)
                ApplyPromotionController.hasOldPromo = true;
            #endregion

            Processor.ReportProgress(100, "initialization finished");
        }

        void updater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Logger.writeLog("Download updater completed!");
        }

        void updater_DoWork(object sender, DoWorkEventArgs e)
        {
            POSVersionChecker.DownloadNewVersion();
        }

        private void frmLoadingScreen_Load(object sender, EventArgs e)
        {
            string title = "EQuipPOS Retail";

            //Load Rounding Preference that the customer like
            if (AppSetting.GetSettingFromDBAndConfigFile("LoadingScreenTitle") != null)
            {
                title = AppSetting.GetSettingFromDBAndConfigFile("LoadingScreenTitle").ToString();

                if (string.IsNullOrEmpty(title))
                {
                    title = "EQuipPOS Retail";
                }
            }

            labelEquipPOSTitle.Text = title;

            Processor.RunWorkerAsync();
        }

    }
}
