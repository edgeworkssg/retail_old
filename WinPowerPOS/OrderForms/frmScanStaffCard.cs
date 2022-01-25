using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
//using WinRestaurantUI;

namespace WinPowerPOS.OrderForms
{
    
    public partial class frmScanStaffCard : Form
    {
        
        private POSController pos;
        public string membershipNo;
        public string EncryptedCardID;
        public string title;
        public string txtMessage;
        //public ScanMode Mode = ScanMode.Payment;
        public DateTime startScanTime;
        public DateTime endScanTime;
        public bool isReadmode = false;
        public string tmpCardID = "";

        public frmScanStaffCard()
        {
            InitializeComponent();
        }

        public void applyLanguageTranslation()
        {
            /*string langID = AppSetting.GetSetting(AppSetting.SettingsName.LanguageSetting);
            langID = String.IsNullOrEmpty(langID) ? "ENG" : langID;*/

            //var dc = TextLanguageController.FetchByLangAndStartWithID(langID, "ScanStaffCard.");

            /*if (dc.ContainsKey("NETSConfirmation.Title")) this.Text = dc["NETSConfirmation.Title"];
            if (dc.ContainsKey("NETSConfirmation.NETSPaymentFailed")) lblHeader.Text = dc["NETSConfirmation.NETSPaymentFailed"];
            if (dc.ContainsKey("NETSConfirmation.NETSPaymentFailed")) lblStatus.Text = dc["NETSConfirmation.NETSPaymentFailed"];
            if (dc.ContainsKey("NETSConfirmation.Retry")) btnRetry.Text = dc["NETSConfirmation.Retry"];
            if (dc.ContainsKey("NETSConfirmation.Manual")) btnManual.Text = dc["NETSConfirmation.Manual"];
            if (dc.ContainsKey("NETSConfirmation.Cancel")) btnCancel.Text = dc["NETSConfirmation.Cancel"];*/
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            //normal discount
            this.DialogResult = DialogResult.No;
        }

        private void frmNETSConfirmation_Shown(object sender, EventArgs e)
        {
            this.Text = title;
            this.lblMessage.Text = txtMessage;
            //txtBarcode.Focus();
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
            }
        }

        private void frmScanStaffCard_Load(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void frmScanStaffCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                if (!isReadmode)
                {
                    tmpCardID = "";
                    startScanTime = DateTime.Now;
                    tmpCardID += e.KeyChar.ToString();
                    isReadmode = true;
                }
                else
                {
                    tmpCardID += e.KeyChar.ToString();

                }
            }
            else
            {
                endScanTime = DateTime.Now;
                int buffertime = 0;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Order.ClearKeyboardBufferTime), out buffertime))
                {
                    buffertime = 1500;
                }
                if ((endScanTime.Hour * 24 * 60 * 1000 + endScanTime.Minute * 60 * 1000 + endScanTime.Second * 1000 + endScanTime.Millisecond) - 
                    (startScanTime.Hour * 24 * 60 * 1000 + startScanTime.Minute * 60 * 1000 + startScanTime.Second * 1000 + endScanTime.Millisecond) > buffertime)
                {
                    MessageBox.Show("Token is invalid");
                    isReadmode = false;
                    //this.DialogResult = DialogResult.Cancel;
                    return;
                }

                String encryptedData = tmpCardID;
                UserMstCollection members = new UserMstCollection();
                members.Where(Membership.Columns.Userfld7, encryptedData);
                members.Load();
                if (members.Count > 0)
                {
                    EncryptedCardID = encryptedData;
                    membershipNo = members[0].UserName;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Token is invalid");
                }
                isReadmode = false;
            }
        }

        private void frmScanStaffCard_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
