using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeighingMachine;
using PowerPOS;
using System.Threading;

namespace WinPowerPOS.KioskForms
{
    public partial class frmScanWeight : Form
    {
        private BackgroundWorker worker;

        public decimal Weight;

        public bool IsSuccess;

        private bool Cancelled;

        public frmScanWeight()
        {
            InitializeComponent();

            IsSuccess = false;
            Weight = new decimal(0.0f);
            Cancelled = false;

            #region Weight Background Worker

            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            worker.RunWorkerAsync();

            #endregion
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DoWork();
        }

        private void DoWork()
        {
            Weighing wm = new Weighing();
            string comPort = AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.COMPort);
            string cmd = AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.CommandToExecute);
            string result = "";
            
            string Status = "";
            string _uom = "";

            IsSuccess = wm.GetWeight(comPort, cmd + "\r\n", out result);
            Logger.writeStaffAssistLog("INFO", ">> Weighing Machine Result :" + result);
            int reTry = 0;
            while (!IsSuccess && reTry < 5 && !Cancelled)
            {
                Thread.Sleep(3000);
                reTry++;
                IsSuccess = wm.GetWeight(comPort, cmd, out result);
                Logger.writeStaffAssistLog("INFO", ">> Weighing Machine Result :" + result);
            }
            if (IsSuccess)
            {
                try
                {
                    //data = "0.21KG\r\nS00";
                    result = result.Split(new string[] { "\r\n" }, StringSplitOptions.None)[0];
                    Logger.writeLog(">> Weighing Machine Result (Splitted):" + result);
                    string weightStr = "";
                    string uomStr = "";
                    for (int k = 0; k < result.Length; k++)
                    {
                        if (result[k] == ' ') break;
                        if (char.IsNumber(result[k]) || result[k] == '.')
                            weightStr += result[k].ToString();
                        else
                            uomStr += result[k].ToString();
                    }
                    uomStr = uomStr.Trim().Trim(("\r\n").ToCharArray()).Trim().Trim(("\r").ToCharArray()).Trim(("\n").ToCharArray());
                    if (!uomStr.ToLower().Trim().Equals(_uom.ToLower().Trim()))
                    {
                        IsSuccess = false;
                        Status = string.Format("Weighing machine UOM ({0}) is not tally with Product UOM ({1})", uomStr, _uom);
                    }
                    else
                    {
                        IsSuccess = true;
                        Weight = weightStr.GetDecimalValue();

                        this.DialogResult = DialogResult.OK;
                    }
                }
                catch (Exception ex)
                {
                    IsSuccess = false;
                    Logger.writeLog(ex);
                    Status = "Please try again";
                }
            }
            else
            {
                IsSuccess = false;
                Status = "Please try again";

                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancelled = true;

            Logger.writeStaffAssistLog("INFO", "Error Weighing : Cancelled by user");

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
