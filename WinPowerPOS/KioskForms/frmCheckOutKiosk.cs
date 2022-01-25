using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using SubSonic;
using POSDevices;
using System.Collections;

namespace WinPowerPOS.KioskForms
{
    public partial class frmCheckOutKiosk : Form
    {
        DateTime startTime, endTime;
        public frmCheckOutKiosk()
        {
            InitializeComponent();
        }
        decimal TotalCashColleted, TotalPay1Colleted, TotalPay2Colleted, TotalVoucherColleted, TotalPay3Colleted, TotalPay4Colleted;
        decimal TotalPay5Colleted, TotalPay6Colleted, TotalPay7Colleted, TotalPay8Colleted, TotalPay9Colleted, TotalPay10Colleted;
        decimal TotalChequeCollected, TotalPointCollected, TotalPackageCollected, TotalSMFCollected, TotalPAMedCollected, TotalPWFCollected;
        decimal TotalNETSCashCardCollected, TotalNETSFlashPayCollected, TotalNETSATMCardCollected, TotalForeignCurrency;
        Dictionary<string, decimal> ForeignCurrency = new Dictionary<string, decimal>();

        public UserMst currentUser;
        int PointOfSaleID;
        public string status;
        public frmStartKiosk fStartKiosk;

        static int TOTAL_BUTTONS = 5;
        bool enableNETSIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);

        private void LoadPaymentTypeLabels()
        {
            for (int Counter = 1; Counter <= TOTAL_BUTTONS; Counter++)
            {
                //Fetch Payment Types and check in the 
                string PaymentType = PaymentTypesController.FetchPaymentByID(Counter.ToString());
                switch (Counter)
                {
                    case 1: if (PaymentType == "" || (enableNETSIntegration && PaymentType.ToUpper().Contains("NETS")))
                        {
                            checkOut1.spPay1.Visibility = System.Windows.Visibility.Collapsed;
                            checkOut1.lblPay1.Text = PaymentType;
                        }
                        else
                        {
                            checkOut1.spPay1.Visibility = System.Windows.Visibility.Visible;
                            checkOut1.lblPay1.Text = PaymentType;
                        }
                            break;
                    case 2: if (PaymentType == "" || (enableNETSIntegration && PaymentType.ToUpper().Contains("NETS")))
                            {
                                checkOut1.spPay2.Visibility = System.Windows.Visibility.Collapsed;
                                checkOut1.lblPay2.Text = PaymentType;
                            }
                            else
                            {
                                checkOut1.spPay2.Visibility = System.Windows.Visibility.Visible;
                                checkOut1.lblPay2.Text = PaymentType;
                            }
                            break;
                    case 3: if (PaymentType == "" || (enableNETSIntegration && PaymentType.ToUpper().Contains("NETS")))
                            {
                                checkOut1.spPay3.Visibility = System.Windows.Visibility.Collapsed;
                                checkOut1.lblPay3.Text = PaymentType;
                            }
                            else
                            {
                                checkOut1.spPay3.Visibility = System.Windows.Visibility.Visible;
                                checkOut1.lblPay3.Text = PaymentType;
                            }
                            break;
                    case 4: if (PaymentType == "" || (enableNETSIntegration && PaymentType.ToUpper().Contains("NETS")))
                            {
                                checkOut1.spPay4.Visibility = System.Windows.Visibility.Collapsed;
                                checkOut1.lblPay4.Text = PaymentType;
                            }
                            else
                            {
                                checkOut1.spPay4.Visibility = System.Windows.Visibility.Visible;
                                checkOut1.lblPay4.Text = PaymentType;
                            }
                            break;
                    case 5: if (PaymentType == "" || (enableNETSIntegration && PaymentType.ToUpper().Contains("NETS")))
                            {
                                checkOut1.spPay5.Visibility = System.Windows.Visibility.Collapsed;
                                checkOut1.lblPay5.Text = PaymentType;
                            }
                            else
                            {
                                checkOut1.spPay5.Visibility = System.Windows.Visibility.Visible;
                                checkOut1.lblPay5.Text = PaymentType;
                            }
                            break;
                default : break;
                }
            }

            #region If Nets Enabled then show the breakdown
            if (enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCashCard), false))
                this.checkOut1.spPayNetsCashCard.Visibility = System.Windows.Visibility.Visible;
            else
                this.checkOut1.spPayNetsCashCard.Visibility = System.Windows.Visibility.Collapsed;

            if (enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSFlashPay), false))
                this.checkOut1.spPayNetsFlashPay.Visibility = System.Windows.Visibility.Visible;
            else
                this.checkOut1.spPayNetsFlashPay.Visibility = System.Windows.Visibility.Collapsed;
            
            if (enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSATMCard), false))
                this.checkOut1.spPayNetsATMCard.Visibility = System.Windows.Visibility.Visible; 
            else
                this.checkOut1.spPayNetsATMCard.Visibility = System.Windows.Visibility.Collapsed;
            #endregion

            #region if Cash Enabled then Show Cash
            checkOut1.spCash.Visibility = System.Windows.Visibility.Collapsed;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.EnableNotes), false))
                checkOut1.spCash.Visibility = System.Windows.Visibility.Visible;
            #endregion

        }


        private void frmCheckOutKiosk_Load(object sender, EventArgs e)
        {
            startTime = ClosingController.FetchOpeningShift(PointOfSaleInfo.PointOfSaleID);
            endTime = DateTime.Now;
            this.checkOut1.txtStartTime.Text = startTime.ToString("dd MMM yyyy hh:mm:ss tt");
            this.checkOut1.txtEndTime.Text = endTime.ToString("dd MMM yyyy hh:mm:ss tt");
            this.checkOut1.txtCashier.Text = "Cashier : " + currentUser.UserName;
            PointOfSaleID = PointOfSaleInfo.PointOfSaleID;

            #region Load Payment Types
            string status1="";
            //Load Payment Types and Assign it
            PaymentTypesController.LoadPaymentTypes(out status1);
            LoadPaymentTypeLabels();
            #endregion

            ctrlExit.ButtonText = "X";
            ctrlExit.ButtonColor = "#CC5151";
            ctrlExit.ButtonHoverColor = "#CC7070";
            ctrlExit.ButtonPressColor = "#A54242";
            ctrlExit.ControlClick += new EventHandler(checkOut1_CancelClick);

            ctrlAccept.OnClicked += new Accept.OnClickEventHandler(checkOut1_AcceptClick);


            displayCollectedOfIndividualPaymentMode();
            //checkOut1.CancelClick += new EventHandler(checkOut1_CancelClick);
            //checkOut1.AcceptClick += new EventHandler(checkOut1_AcceptClick);
            
        }

        private void displayCollectedOfIndividualPaymentMode()
        {
            ReceiptController.GetSystemCollectedBreakdownByPaymentType(
                    startTime, endTime, PointOfSaleID,
                     out TotalCashColleted,
                     out TotalPay1Colleted,
                     out TotalPay2Colleted,
                     out TotalVoucherColleted,
                     out TotalPay3Colleted,
                     out TotalPay4Colleted,
                     out TotalPay5Colleted,
                     out TotalPay6Colleted,
                     out TotalPay7Colleted,
                     out TotalPay8Colleted,
                     out TotalPay9Colleted,
                     out TotalPay10Colleted,
                     out TotalChequeCollected,
                     out TotalPointCollected,
                     out TotalPackageCollected,
                     out TotalSMFCollected,
                     out TotalPAMedCollected,
                     out TotalPWFCollected,
                     out TotalNETSCashCardCollected,
                     out TotalNETSFlashPayCollected,
                     out TotalNETSATMCardCollected,
                     out TotalForeignCurrency,
                     out ForeignCurrency);

            decimal systemCollected = TotalNETSATMCardCollected + TotalNETSFlashPayCollected + TotalNETSCashCardCollected + TotalPay1Colleted + TotalPay2Colleted + TotalPay3Colleted
                + TotalPay4Colleted + TotalPay5Colleted + TotalCashColleted;
            this.checkOut1.txtTotal.Text = systemCollected.ToString("N2");
            this.checkOut1.txtNetsATMCard.Text = TotalNETSATMCardCollected.ToString("N2");
            this.checkOut1.txtNetsFlashPay.Text = TotalNETSFlashPayCollected.ToString("N2");
            this.checkOut1.txtNetsCashCard.Text = TotalNETSCashCardCollected.ToString("N2");
            this.checkOut1.txtPay1Recorded.Text = TotalPay1Colleted.ToString("N2");
            this.checkOut1.txtPay2Recorded.Text = TotalPay2Colleted.ToString("N2");
            this.checkOut1.txtPay3Recorded.Text = TotalPay3Colleted.ToString("N2");
            this.checkOut1.txtPay4Recorded.Text = TotalPay4Colleted.ToString("N2");
            this.checkOut1.txtPay5Recorded.Text = TotalPay5Colleted.ToString("N2");
            this.checkOut1.txtCASH.Text = TotalCashColleted.ToString("N2");

        }

        void checkOut1_CancelClick(object sender, EventArgs e)
        {
            
            DialogResult = DialogResult.Cancel;
            Close();
        }


        
        void checkOut1_AcceptClick(object sender, EventArgs e)
        {

            QueryCommandCollection cmd = new QueryCommandCollection() ;
            try
            {
                //Save Check Out
                CounterCloseLog myCounter = new CounterCloseLog();
                int runningNo = 0;
                IDataReader ds = PowerPOS.SPs.GetNewCounterCloseIDByPointOfSaleID(PointOfSaleInfo.PointOfSaleID).GetReader();
                while (ds.Read())
                {
                    if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
                    {
                        runningNo = 0;
                    }
                }
                ds.Close();
                runningNo += 1;
                string CounterClosedLogId = "CL" + DateTime.Now.ToString("yyMMdd") + PointOfSaleInfo.PointOfSaleID.ToString().PadLeft(4, '0') + runningNo.ToString().PadLeft(4, '0');
                myCounter.CounterCloseID = CounterClosedLogId;
                myCounter.Cashier = currentUser.UserName;
                myCounter.FloatBalance = 0;
                myCounter.CashRecorded = TotalCashColleted;
                myCounter.NetsRecorded = TotalPay1Colleted;
                myCounter.VisaRecorded = TotalPay2Colleted;
                myCounter.ChinaNetsRecorded = TotalPay3Colleted;
                myCounter.AmexRecorded = TotalPay4Colleted;
                myCounter.Pay5Recorded = TotalPay5Colleted;
                myCounter.Pay6Recorded = TotalPay6Colleted;
                myCounter.Pay7Recorded = TotalPay7Colleted;
                myCounter.Pay8Recorded = TotalPay8Colleted;
                myCounter.Pay9Recorded = TotalPay9Colleted;
                myCounter.Pay10Recorded = TotalPay10Colleted;
                myCounter.ChequeRecorded = 0;
                myCounter.CashCollected = TotalCashColleted;
                myCounter.NetsCollected = TotalPay1Colleted;
                myCounter.VisaCollected = TotalPay2Colleted;
                myCounter.ChinaNetsCollected = TotalPay3Colleted;
                myCounter.AmexCollected = TotalPay4Colleted;
                myCounter.Pay5Collected = TotalPay5Colleted;
                myCounter.Pay6Collected = TotalPay6Colleted;
                myCounter.Pay7Collected = TotalPay7Colleted;
                myCounter.Pay8Collected = TotalPay8Colleted;
                myCounter.Pay9Collected = TotalPay9Colleted;
                myCounter.Pay10Collected = TotalPay10Colleted;
                myCounter.VoucherCollected = TotalVoucherColleted;
                myCounter.VoucherRecorded = TotalVoucherColleted;
                myCounter.ChequeRecorded = TotalChequeCollected;
                myCounter.ChequeCollected = TotalChequeCollected;
                myCounter.PointRecorded = 0;
                myCounter.PackageRecorded = 0;
                myCounter.SMFRecorded = TotalSMFCollected;
                myCounter.SMFCollected = TotalSMFCollected;
                myCounter.PAMedRecorded = TotalPAMedCollected;
                myCounter.PAMedCollected = TotalPAMedCollected;
                myCounter.PWFRecorded = TotalPWFCollected;
                myCounter.PWFCollected = TotalPWFCollected;
                myCounter.NetsATMCardCollected = TotalNETSATMCardCollected;
                myCounter.NetsATMCardRecorded = TotalNETSATMCardCollected;
                myCounter.NetsCashCardCollected = TotalNETSCashCardCollected;
                myCounter.NetsCashCardRecorded = TotalNETSCashCardCollected;
                myCounter.NetsFlashPayCollected = TotalNETSFlashPayCollected;
                myCounter.NetsFlashPayRecorded = TotalNETSFlashPayCollected;

                if (enableNETSIntegration)
                {
                    myCounter.NetsRecorded = myCounter.NetsATMCardRecorded.GetValueOrDefault(0)
                                            + myCounter.NetsCashCardRecorded.GetValueOrDefault(0)
                                            + myCounter.NetsFlashPayRecorded.GetValueOrDefault(0);
                    myCounter.NetsCollected = myCounter.NetsATMCardCollected.GetValueOrDefault(0)
                                            + myCounter.NetsCashCardCollected.GetValueOrDefault(0)
                                            + myCounter.NetsFlashPayCollected.GetValueOrDefault(0);
                }
                myCounter.TotalForeignCurrency = 0;
                myCounter.NetsTerminalID = "";// txtNetsTerminalID.Text;
                myCounter.VisaBatchNo = ""; // txtVisaBatchNo.Text;
                myCounter.AmexBatchNo = ""; // txtAmexBatchNo.Text;
                myCounter.ChinaNetsTerminalID = ""; //txtChinaNetsTerminalID.Text;
                myCounter.DepositBagNo = "";
                myCounter.CashIn = 0;
                myCounter.CashOut = 0;
                myCounter.ClosingCashOut = 0;
                myCounter.StartTime = startTime;
                myCounter.EndTime = endTime;
                myCounter.OpeningBalance = 0;
                myCounter.Supervisor = currentUser.UserName;
                myCounter.TotalSystemRecorded = myCounter.NetsRecorded.GetValueOrDefault(0) + myCounter.CashRecorded.GetValueOrDefault(0) + myCounter.ChinaNetsRecorded.GetValueOrDefault(0) 
                    + myCounter.VisaRecorded.GetValueOrDefault(0) + myCounter.AmexRecorded.GetValueOrDefault(0) + myCounter.Pay5Recorded.GetValueOrDefault(0);
                myCounter.TotalActualCollected = myCounter.TotalSystemRecorded;
                myCounter.Variance = 0;
                myCounter.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                myCounter.UniqueID = Guid.NewGuid();

                QueryCommand mycmd = myCounter.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);

                DataService.ExecuteTransaction(cmd);

                #region *) Notification: Success
                MessageBox.Show("Counter Closed Successfully.");
                #endregion

                PointOfSaleInfo.NETsTerminalID = ""; // txtNetsTerminalID.Text;
                //PointOfSaleController.SaveNETSTerminalID(txtNetsTerminalID.Text);
                ClosingController.DeleteSavedClosing();
                ClosingController.DeleteSaved(PointOfSaleInfo.PointOfSaleID);

                #region *) Core: Print Counter Close Report
                bool PrintProductSalesReport = false;
                PrintProductSalesReport = (AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport") != null && AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport").ToString().ToUpper() == "YES");
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintCounterCloseReport), true))
                {
                    try
                    {
                        bool printDiscount = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintDiscountOnCounterCloseReport), false));
                        PrintProductSalesReport = (AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport") != null && AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport").ToString().ToUpper() == "YES");
                        POSDeviceController.PrintCloseCounterReport(new CounterCloseLog(CounterCloseLog.Columns.CounterCloseID, CounterClosedLogId), PrintProductSalesReport, printDiscount);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to print settlement. Please try again at Closing Report section");
                        Logger.writeLog(ex);
                    }
                }
                #endregion

                #region *) Core: Send Emails (Closing Information)
                try
                {
                    ArrayList emails = new ArrayList();
                    EmailNotificationCollection em = new EmailNotificationCollection();
                    em.Where(EmailNotification.Columns.Deleted, false);
                    em.Load();
                    if (em.Count > 0)
                    {
                        for (int i = 0; i < em.Count; i++)
                            emails.Add(em[i].EmailAddress);

                        PowerPOS.MassEmail s = new PowerPOS.MassEmail();
                        string status = "", htmlBody = "";
                        ClosingController cl = new ClosingController();
                        bool isInclusiveGST = false;
                        if (AppSetting.GetSettingFromDBAndConfigFile("PriceInclusiveGST") != null &&
                            AppSetting.GetSettingFromDBAndConfigFile("PriceInclusiveGST").ToString() == "yes")
                        {
                            isInclusiveGST = true;
                        }

                        htmlBody = cl.FormatEmailOutput(myCounter, isInclusiveGST, PrintProductSalesReport);
                        s.SendEmails(emails, "", AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail),
                            "Settlement Summary " + myCounter.EndTime.ToString("dd MMM yyyy HH:mm") +
                            " " + myCounter.PointOfSale.PointOfSaleName,
                            htmlBody, htmlBody,
                             AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SMTP),
                            AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail),
                            AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Password), false, "",
                            AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Port), out status);
                    }

                    //Run Sync
                    fStartKiosk.SyncLogData();
                }
                catch (Exception X)
                {
                    CommonUILib.HandleException(X);
                }
                #endregion
            }
            catch (Exception ex)
            {
                status = "Error Save Check Out";
                DialogResult = DialogResult.Abort;
                Close();
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

    }
}
