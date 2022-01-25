using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using PowerPOSLib.Controller;

namespace WinPowerPOS.KioskForms
{
    public partial class frmScanBarcode : Form
    {
        public DateTime startScanTime;
        public DateTime endScanTime;
        public bool isReadmode = false;
        public string BarcodeResult = "";
        private BarcodeScannerController barcodeScanner = null;
        private static frmStartKiosk _mainForm = null;

        private bool isBarcodeEnabled = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.BarcodeScanner.Enabled), false);

        public frmScanBarcode(frmStartKiosk mainForm, string title)
        {
            InitializeComponent();
            ctrlScanBarcode.CancelClick += new EventHandler(ctrlScanBarcode_CancelClick);
            ctrlScanBarcode.Title = title;
            barcodeScanner = new BarcodeScannerController();
            barcodeScanner.BarcodeScanned += new BarcodeScannedHandler(barcodeScanner_BarcodeScanned);
            _mainForm = mainForm;
            this.KeyPreview = false;
        }

        void barcodeScanner_BarcodeScanned(BarcodeScannerController m, BarcodeScannedArgs e)
        {
            SetText(e.lastBarcode);
            /*
            barcodeScanner.BarcodeScanned -= barcodeScanner_BarcodeScanned;
           
                this.DialogResult = DialogResult.OK;
                this.Close();*/
            
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                BarcodeResult = (text + "").Trim().ToUpper();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }



        void ctrlScanBarcode_CancelClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmScanBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                if (!isReadmode)
                {
                    BarcodeResult = "";
                    startScanTime = DateTime.Now;
                    BarcodeResult += e.KeyChar.ToString();
                    isReadmode = true;
                }
                else
                {
                    BarcodeResult += e.KeyChar.ToString();
                }
            }
            else
            {
                endScanTime = DateTime.Now;
                int buffertime = (AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.ClearKeyboardBufferTime)+"").GetIntValue();
                if(buffertime==0)
                    buffertime = 1500;

                if ((endScanTime.Hour * 24 * 60 * 1000 + endScanTime.Minute * 60 * 1000 + endScanTime.Second * 1000 + endScanTime.Millisecond) -
                    (startScanTime.Hour * 24 * 60 * 1000 + startScanTime.Minute * 60 * 1000 + startScanTime.Second * 1000 + endScanTime.Millisecond) > buffertime)
                {
                    MessageBox.Show("Barcode is invalid");
                    isReadmode = false;
                    return;
                }
                BarcodeResult = BarcodeResult.ToUpper().Trim();
                barcodeScanner.BarcodeScanned -= barcodeScanner_BarcodeScanned;
                this.DialogResult = DialogResult.OK;
                this.Close();

                isReadmode = false;
            }
        }

        private void frmScanBarcode_Shown(object sender, EventArgs e)
        {
            this.Focus();
            if(isBarcodeEnabled)
                InitBarcodeScanner();
        }

        private void bwBarcode_DoWork(object sender, DoWorkEventArgs e)
        {
            //if(isBarcodeEnabled)
                
        }

        private void InitBarcodeScanner()
        {
            AppendLog(string.Format("Init Barcode scanner on port : " + barcodeScanner.COMPort));
            string barcodeStatus = "";
            if (barcodeScanner.init(out barcodeStatus))
                _mainForm.AppendLog("Init barcode completed successfuly");
            else
                _mainForm.AppendLog("Init barcode failed. " + barcodeStatus);
        }

        public void AppendLog(string item)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    DateTime dt = DateTime.Now;

                    Logger.writeStaffAssistLog("INFO", item);

                    if (_mainForm.BufferLog.Count > 1000)
                        _mainForm.BufferLog.Dequeue();

                    _mainForm.BufferLog.Enqueue(dt.ToString("dd-MM-yyyy hh:mm:ss") + " - " + item);

                    _mainForm.Notify();
                });
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void frmScanBarcode_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void frmScanBarcode_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (barcodeScanner != null)
            {
                //barcodeScanner.disableDevice();
                barcodeScanner.Dispose();
            }
        }
    }
}
