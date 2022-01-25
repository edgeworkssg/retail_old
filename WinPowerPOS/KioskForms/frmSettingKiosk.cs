using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS.Container;
using PowerPOSLib.Controller;
using PowerPOS;

namespace WinPowerPOS.KioskForms
{
    public partial class frmSettingKiosk : Form
    {
        private frmStartKiosk frmStartKiosk;
        private string userName = "";
        WeighingMachineController wmController;

        public frmSettingKiosk(frmStartKiosk frmStartKiosk, string userName)
        {
            InitializeComponent();
            this.userName = userName;
            this.frmStartKiosk = frmStartKiosk;

            wmController = frmStartKiosk.GetWeightingMachineController();
            if (wmController != null)
            {
                wmController.StatusChanged += new StatusChangedHandler(wmController_StatusChanged);
            }


            btnGeneral.SetContent("GENERAL");
            btnMenuButton.SetContent("HARDWARE");
            btnPayment.SetContent("PAYMENT");
            btnAdmin.SetContent("ADMIN");
            btnLog.SetContent("LOG");

            #region LOAD GENERAL

            settingGeneral.Load();

            #endregion

            #region LOAD HARDWARE

            settingHardware.Load();
            settingHardware.SetWMContoller(wmController);

            #endregion

            #region LOAD ADMIN
            if (userName.ToLower().Equals("edgeworks"))
            {
                settingAdmin.Load();
            }
            else
            {
                hostAdmin.Visible = false;
                //hostLog.Visible = false;
            }

            #endregion

            #region LOAD PAYMENT

            settingPayment.Load();

            #endregion

            btnGeneral.OnClicked += new MenuButton.OnClickEventHandler(btnGeneral_OnClicked);
            btnMenuButton.OnClicked += new MenuButton.OnClickEventHandler(btnMenuButton_OnClicked);
            btnPayment.OnClicked += new MenuButton.OnClickEventHandler(btnPayment_OnClicked);
            btnAdmin.OnClicked += new MenuButton.OnClickEventHandler(btnAdmin_OnClicked);
            btnLog.OnClicked += new MenuButton.OnClickEventHandler(btnLog_OnClicked);
            btnDone.OnClicked += new DoneButton.OnClickEventHandler(btnDone_OnClicked);

            btnGeneral_OnClicked(null, new EventArgs());
        }

        public void wmController_StatusChanged(WeighingMachineController m, StatusChangedArgs e)
        {
            if (frmStartKiosk.State == "PROCESSING")
                return;

            if (frmStartKiosk.SubState != "OPEN SETTING PAGE")
                return;

            settingHardware.setWeighingMachineStatus(e.currentStatus.ToUpper() == "CONNECTED");
        }

        void btnDone_OnClicked(object sender, EventArgs args)
        {
            // DO SAVE
            #region SAVE GENERAL

            settingGeneral.Save();

            #endregion

            #region SAVE HARDWARE

            settingHardware.Save();

            frmStartKiosk.AppendLog("Weighing scale enabled is " + AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.UseWeighingMachine), false));

            #endregion

            #region SAVE ADMIN

            if (userName.ToLower().Equals("edgeworks"))
                settingAdmin.Save();
            
            #endregion

            #region SAVE ADMIN

            settingPayment.Save();

            #endregion

            if (wmController != null)
                wmController.StatusChanged -= new StatusChangedHandler(wmController_StatusChanged);

            Close();
        }
        

        void btnGeneral_OnClicked(object sender, EventArgs args)
        {
            btnGeneral.SetState(true);
            btnMenuButton.SetState(false);
            btnPayment.SetState(false);
            btnAdmin.SetState(false);
            btnLog.SetState(false);

            pnlGeneral.BringToFront();
        }

        void btnMenuButton_OnClicked(object sender, EventArgs args)
        {

            btnGeneral.SetState(false);
            btnMenuButton.SetState(true);
            btnPayment.SetState(false);
            btnAdmin.SetState(false);
            btnLog.SetState(false);

            pnlMenuButton.BringToFront();
        }

        void btnPayment_OnClicked(object sender, EventArgs args)
        {
            btnGeneral.SetState(false);
            btnMenuButton.SetState(false);
            btnPayment.SetState(true);
            btnAdmin.SetState(false);
            btnLog.SetState(false);

            pnlPayment.BringToFront();
        }

        void btnAdmin_OnClicked(object sender, EventArgs args)
        {
            btnGeneral.SetState(false);
            btnMenuButton.SetState(false);
            btnPayment.SetState(false);
            btnAdmin.SetState(true);
            btnLog.SetState(false);

            pnlAdmin.BringToFront();
        }

        void btnLog_OnClicked(object sender, EventArgs args)
        {
            btnGeneral.SetState(false);
            btnMenuButton.SetState(false);
            btnPayment.SetState(false);
            btnAdmin.SetState(false);
            btnLog.SetState(true);
            ctrlPanelLog.LogContent = string.Join(Environment.NewLine, frmStartKiosk.BufferLog.ToArray());
            pnlLog.BringToFront();
        }
    }
}
