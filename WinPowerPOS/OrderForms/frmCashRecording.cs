using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using POSDevices;
using System.Collections;
using WinPowerPOS.LoginForms;
using SubSonic;
using System.IO;
using System.Drawing.Imaging;

namespace WinPowerPOS.OrderForms
{
    public partial class frmCashRecording : Form
    {
        public decimal openingBal;
        private frmMain formMain;
        private bool cashDrawerAfterEnter = false;
        public BackgroundWorker SyncCashRecordingThread;
        public int defaultType = -1;

        #region "Form Load and Init"
        public frmCashRecording()
        {
            InitializeComponent();
            cashDrawerAfterEnter = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.KickCashDrawerAfterAmountEntered), false);
        }
        public frmCashRecording(frmMain formMain)
        {
            this.formMain = formMain;
            InitializeComponent();
            cashDrawerAfterEnter = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.KickCashDrawerAfterAmountEntered), false);

        }

        private void LoadCashRecordingReasons()
        {
            string status = "";
            try
            {
                cbReason.Items.Clear();
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\CashRecordingReason.xml");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cbReason.Items.Add(dr[1].ToString());
                }
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); }
        }

        private void CashRecording_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.writeLog("Load First");
                openingBal = CashRecordingController.GetTotalFloatAmount(PointOfSaleInfo.PointOfSaleID);
                //if (openingBal == 0.0M) 
                //{
                if (!cashDrawerAfterEnter)
                {
                    Logger.writeLog("Kick Drawer");
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.UseCustomKickDrawer), false))
                    {
                        #region *) Core: Run External Script (for Landlord Integration)
                        if (AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath) != null
                            && AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath) != "")
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath).ToString());
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog("Unable start remote process: " + AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath).ToString() + " " + ex.Message);
                            }
                        }
                        #endregion
                    }
                    else
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.UseFlyTechCashDrawer), false))
                        {
                            FlyTechCashDrawer cdrw = new FlyTechCashDrawer();
                            cdrw.OpenDrawer(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.KickDrawerPort));
                        }
                        else
                        {
                            CashDrawer drw = new CashDrawer();
                            drw.OpenDrawer();
                        }
                }
                //}

                lblTotalOpeningAmt.Text = openingBal.ToString("N");

                //Load combo box....
                CashRecordingTypeCollection types = new CashRecordingTypeCollection();

                Logger.writeLog("Load Cash Recording Type");
                //Dont include closing balance
                types.Where(CashRecordingType.Columns.CashRecordingTypeId, SubSonic.Comparison.NotEquals, 4);               
                cmbTrans.DataSource = types.OrderByDesc(CashRecording.Columns.CashRecordingTypeId).Load();
                cmbTrans.Refresh();

                txtCashier.Text = PowerPOS.Container.UserInfo.username;
                txtCounter.Text = PointOfSaleInfo.PointOfSaleName;
                //Create new cash recording ID
                int runningNo = 0;
                int PointOfSaleID = PointOfSaleInfo.PointOfSaleID; //Get value from XML
                IDataReader ds = PowerPOS.SPs.GetNewCashRecRefNoByPointOfSaleID(PointOfSaleID).GetReader();
                while (ds.Read())
                {
                    if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
                    {
                        runningNo = 0;
                    }
                }
                ds.Close();
                runningNo += 1;

                //CRYYMMDDSSSSNNNN                
                //YY - year
                //MM - month
                //DD - day
                //SSSS - PointOfSale ID
                //NNNN - Running No
                txtRefNo.Text = "CR" + DateTime.Now.ToString("yyMMdd") + PointOfSaleID.ToString().PadLeft(4, '0') + runningNo.ToString().PadLeft(4, '0');

                //Datetime
                txtDate.Text = DateTime.Now.ToString("dd MMM yyyy hh:mm:ss");

                txtCounter.Text = PointOfSaleInfo.PointOfSaleName; //PointOfSaleID.ToString();

                //Set the cash recording type to be correct
                if (defaultType > -1)
                {
                    cmbTrans.SelectedIndex = defaultType;
                    cmbTrans.Enabled = false;
                }

                //Logger.writeLog("Load Cash Recording Reason");
                LoadCashRecordingReasons();
                //Logger.writeLog("Load Cash Recording Reason Finish");
                txtAmt.Select();
            }
            catch (OpenDrawerException ex)
            {
                //log...
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message, "", MessageBoxButtons.OK,MessageBoxIcon.Error );
            }
            catch (Exception ex)
            {
                //log...
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message, "", MessageBoxButtons.OK,MessageBoxIcon.Error );
            }
        }

        #endregion

       
        private void btnOk_Click(object sender, EventArgs e)
        {
            
            bool IsAuthorized;
            string supName;
            try
            {
                decimal amt;

                if (!decimal.TryParse(txtAmt.Text, out amt) || amt < 0)
                {
                    MessageBox.Show("Please enter a valid amount", "", MessageBoxButtons.OK,MessageBoxIcon.Error );
                    return;
                }
                if (String.Compare(cmbTrans.SelectedValue.ToString(), "opening balance", true) == 0 && openingBal > 0)
                {
                    DialogResult dr = MessageBox.Show("WARNING. Float amount has been set previously at " +
                        openingBal.ToString("N") + ". Are you sure you want to set ADDITIONAL float amount?",
                        "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dr == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (String.Compare(cmbTrans.SelectedValue.ToString().ToLower(), "cash out", true) == 0 && txtRemark.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter remark. Remark is compulsory for cash out.");
                    return;
                }

                #region *) Authorization: Check Supervisor ID
                bool Prompt = false;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnOpeningBalance), true) &&
                    String.Compare(cmbTrans.SelectedValue.ToString(), "opening balance", true) == 0)
                {
                    Prompt = true;
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnCashIn), true) &&
                    String.Compare(cmbTrans.SelectedValue.ToString(), "cash in", true) == 0)
                {
                    Prompt = true;
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnCashOut), true) &&
                    String.Compare(cmbTrans.SelectedValue.ToString(), "cash out", true) == 0)
                {
                    Prompt = true;
                }
                if (Prompt)
                {
                    //bool IsAuthorized;
                    string SupID = "-";
                    #region *) Authorization: Check Supervisor ID
                    if (Prompt)
                    {
                        string useMagenticStripReader = AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForAuthorizing);
                        if (useMagenticStripReader != null && useMagenticStripReader.ToLower() == "yes")
                        {

                            frmReadMSR f = new frmReadMSR();
                            f.privilegeName = PrivilegesController.CASH_RECORDING;
                            f.loginType = LoginType.Authorizing;
                            f.ShowDialog();
                            IsAuthorized = f.IsAuthorized;
                            supName = f.buffer;
                            f.Dispose();
                        }
                        else
                        {
                            //Ask for verification....
                            //Prompt Supervisor Password
                            LoginForms.frmSupervisorLogin sl = new LoginForms.frmSupervisorLogin();
                            sl.privilegeName = PrivilegesController.CASH_RECORDING;
                            sl.ShowDialog();
                            SupID = sl.mySupervisor;
                            supName = sl.mySupervisor; 
                            IsAuthorized = sl.IsAuthorized;
                        }
                    }
                    else
                    { IsAuthorized = true; supName = UserInfo.username; }                    
                }
                else
                { 
                    IsAuthorized = true;
                    supName = UserInfo.username;
                }
                #endregion

                if (IsAuthorized)
                {
                    if (cashDrawerAfterEnter)
                    {
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.UseCustomKickDrawer), false))
                        {
                            #region *) Core: Run External Script (for Landlord Integration)
                            if (AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath) != null
                                && AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath) != "")
                            {
                                try
                                {
                                    System.Diagnostics.Process.Start(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath).ToString());
                                }
                                catch (Exception ex)
                                {
                                    Logger.writeLog("Unable start remote process: " + AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath).ToString() + " " + ex.Message);
                                }
                            }
                            #endregion
                        }
                        else
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.UseFlyTechCashDrawer), false))
                            {
                                FlyTechCashDrawer cdrw = new FlyTechCashDrawer();
                                cdrw.OpenDrawer(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.KickDrawerPort));
                            }
                            else
                            {
                                CashDrawer drw = new CashDrawer();
                                drw.OpenDrawer();
                            }
                    }

                    //add form signature
                    Bitmap signature = null;

                    //Create Cash Recording into DB
                    CashRecording cr = new CashRecording();
                    CashRecordingType ctype = new CashRecordingType(CashRecordingType.Columns.CashRecordingTypeName, cmbTrans.SelectedValue.ToString());
                    cr.Amount = amt;
                    cr.CashierName = txtCashier.Text;
                    cr.CashRecordingTime = DateTime.Now;
                    cr.CashRecordingTypeId = ctype.CashRecordingTypeId;
                    cr.CashRecRefNo = txtRefNo.Text;
                    cr.Remark = txtRemark.Text;
                    cr.SupervisorName = supName;
                    cr.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                    cr.UniqueID = Guid.NewGuid();
                    cr.Save(txtCashier.Text);
                    if (SyncCashRecordingThread != null && !SyncCashRecordingThread.IsBusy)
                    {
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncCashRecording), false))
                            SyncCashRecordingThread.RunWorkerAsync();
                    }
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("CASH Recording : {0}", amt.ToString("N")), "");
                    POSDeviceController.PrintCashRecordingSlip(cr);
                    MessageBox.Show("Cash Recording has been saved successfully.");
                    this.Close();
                    if (formMain != null)
                    {
                        formMain.Visible = true;
                        formMain.Show();
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Save unsuccessful: " + ex.Message, "", MessageBoxButtons.OK,MessageBoxIcon.Error);                
            }
        }
        #endregion 

        #region "Close Form"
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void txtAmt_Click(object sender, EventArgs e)
        {
            frmKeypad f = new frmKeypad();
            f.IsInteger = false;
            f.initialValue = ((TextBox)sender).Text;
            f.ShowDialog();

            ((TextBox)sender).Text = f.value.ToString();         
        }

        private void cbReason_SelectedValueChanged(object sender, EventArgs e)
        {
            
            if (cbReason.Text != "")
            {
                txtRemark.Text = cbReason.Text; 
            }
        }

        private void cbReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnEditCashRecording_Click(object sender, EventArgs e)
        {
            frmEditReason f = new frmEditReason();
            f.ShowDialog();
            LoadCashRecordingReasons();
        }

    }
}