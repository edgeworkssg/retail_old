using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeighingMachine;
using System.Threading;
using PowerPOS;
using System.IO.Ports;

namespace WinPowerPOS.OrderForms
{
    public partial class frmLoadWeighing : Form
    {
        private BackgroundWorker worker;
        private string _uom;
        public decimal Weight = 0;
        public bool IsSuccess = false;
        public string Status = "";
        public frmLoadWeighing(string uom)
        {
            InitializeComponent();
            _uom = uom;
        }

        private void theTimer_Tick(object sender, EventArgs e)
        {
            if (lblDot.Text.Length < 10)
                lblDot.Text += ".";
            else
                lblDot.Text = "";
        }

        private void frmLoadWeighing_Load(object sender, EventArgs e)
        {
            theTimer.Start();
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DoWork();
        }

        private void DoWork()
        {
            Weighing wm = new Weighing();
            string comPort = AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.COMPort);
            string cmd = AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.CommandToExecute);
            string result = "";
            string weightStr = "";
            string uomStr = "";

            IsSuccess = false;
            int reTry = 0;
            while (!IsSuccess && reTry <= 3)
            {
                weightStr = "";
                uomStr = "";

                IsSuccess = wm.GetWeight(comPort, cmd + "\r\n", out result);
                Logger.writeLog(">> Weighing Machine Result :" + result);

                if (IsSuccess)
                {
                    //data = "\n00.200KG\r\nS00\r";
                    result = result.Split(new string[] { "\r\n" }, StringSplitOptions.None)[0];
                    Logger.writeLog(">> Weighing Machine Result (Splitted):" + result);
                    for (int k = 0; k < result.Length; k++)
                    {
                        if (result[k] == ' ') break;
                        if (char.IsNumber(result[k]) || result[k] == '.')
                            weightStr += result[k].ToString();
                        else
                            uomStr += result[k].ToString();
                    }

                    if (string.IsNullOrEmpty(result) || !char.IsNumber(result[1]) || weightStr.GetDecimalValue() <= 0)
                        IsSuccess = false;
                }

                if (!IsSuccess)
                {
                    Thread.Sleep(3000);
                    reTry++;
                }
            }
            if (IsSuccess)
            {
                try
                {
                    uomStr = uomStr.Trim().Trim(("\r\n").ToCharArray()).Trim().Trim(("\r").ToCharArray()).Trim(("\n").ToCharArray());
                    if (!uomStr.ToLower().Trim().Equals(_uom.ToLower().Trim()))
                    {
                        IsSuccess = false;
                        //MessageBox.Show(string.Format("Weighing machine UOM ({0}) is not tally with Product UOM ({1})", uomStr, _uom));
                        Status = string.Format("Weighing machine UOM ({0}) is not tally with Product UOM ({1})", uomStr, _uom);
                    }
                    else
                    {
                        IsSuccess = true;
                        Weight = weightStr.GetDecimalValue();
                    }
                }
                catch (Exception ex)
                {
                    IsSuccess = false;
                    Logger.writeLog(ex);
                    Status = "Please try again";
                    //MessageBox.Show("Please try again");
                }
            }
            else
            {
                IsSuccess = false;
                Logger.writeLog("Error Weighing : " + result);
                Status = "Please try again";
                //MessageBox.Show("Please try again");
            } 
        }

        private void frmLoadWeighing_Shown(object sender, EventArgs e)
        {
            if(worker!=null)
                worker.RunWorkerAsync();
        }
    }
}
