using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using TPCashCLR;
using System.Threading;
using System.IO.Ports;
using PowerPOSLib.Controller;
using System.Configuration;
using System.Diagnostics;
//using HardwareHelperLib;



namespace WinUtility
{
    public partial class Form2 : Form
    {

        static int DEFAULT_RESPONSE_LENGTH = 1024;
        
        SerialPort _serialPort;

        byte[] response;
        byte[] readBuffer = new byte[DEFAULT_RESPONSE_LENGTH];
        int respIdx = 0;
        bool finishedReading = true;
        bool isBarcodeConnected = false;

        private bool isRunning = false;

        BarcodeScannerController bs = new BarcodeScannerController();

        public Form2()
        {
            InitializeComponent();
        }

        //GRGCashRecyclerController c;

        

        private void Form1_Load(object sender, EventArgs e)
        {
            /*c = new GRGCashRecyclerController();*/
            _serialPort = new SerialPort();

            bs.COMPort = "COM3";
            bs.BarcodeScanned += new BarcodeScannedHandler(Barcode_OnBarcodeChanged);
            bs.DeviceStateChanged += new DeviceStateChangedHandler(Barcode_OnDeviceStateChanged);
            bs.Logged += new LoggingHandler(Barcode_OnLog);
        }

        public void displayMsg(string s)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(displayMsg);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                this.textBox1.AppendText(s + "\n");

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*string status = "";
            if (c.initParam(out status))
            {
                displayMsg(status);
                //displayMsg("Interface Version : " + c.getInterfaceVersion());
                //displayMsg("Library Version : " + c.getLibraryVersion());
            }
            else
            {
                displayMsg(status);
            }*/
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            /*string status = "";
            if (c.connect(out status))
            {
                displayMsg("Success Connecting");
            }
            else
            {
                displayMsg("Failed Connect." + status);
            }*/
            /*displayMsg("Connect... 127.0.0.1" );
            string status = "";
            try
            {
                if (c != null)
                {
                    SystemInfo info = c.connect(out status);
                    
                    displayMsg("Connect...Success");
                    //displaySystemInfo(info);
                }
                else
                    return;
            }
            catch (Exception exc)
            {
                displayMsg("Exception. Unable to connect " + exc.Message);
                return;
            }


            displayMsg("Sign On...");
            try
            {
                if (adapt != null)
                    adapt.SignOn("1234", "1234");
                else
                    return;

                displayMsg("SignOn...Success");
            }
            catch (Exception exc)
            {
                displayMsg(exc.Message);
                return;
            }


            displayMsg("GetSystemConfiguration...");
            try
            {
                if (adapt != null)
                {
                    SystemConfigurationData config = adapt.GetSystemConfiguration();
                    m_SystemConfig = config.data;
                    displaySystemConfiguration(m_SystemConfig);
                }
                else
                    return;
            }
            catch (Exception exc)
            {
                displayMsg(exc.Message);
            }
            displayMsg("GetSystemConfiguration...Success");
            */

        }
        //For text display from different threads.
        delegate void SetTextCallback(string text);

        private void button1_Click_1(object sender, EventArgs e)
        {
            /*try
            {
                string cashUnit = "";
                string status = "";
                bool result = c.getCashUnitInfo(out cashUnit, out status);
                displayMsg("Get Cash Unit : " + result.ToString() +"," +  cashUnit);
                displayMsg(status);
            }
            catch (Exception ex)
            {
                displayMsg(ex.Message);

            }*/
            /*int tmp = 0;
            string status = "";
            if (int.TryParse(textBox2.Text, out tmp))
            {
                if (c.machineStatus)
                {
                    decimal collectedAmount = 0;
                    decimal change = 0;
                    if (!c.collectCashPayment(tmp, out change, out status))
                    {
                        MessageBox.Show("Error Collecting Payment " + status);
                    }
                    else
                    {
                        while (c.totalDepositAmount < tmp)
                        {
                            Thread.Sleep(1000);
                            displayMsg("Total Amount Received = " + c.totalDepositAmount.ToString("N2"));
                        }
                        c.stopDeposit(out status);
                        
                    }
                    
                }
                else
                {
                    MessageBox.Show("Disconnected");
                }


            }
            else
            {
                MessageBox.Show("Error Parsing the Amount");
            }*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //string status = "";
            //c.ConfirmState(out status);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            /*int tmp = 0;
            string status = "";
            if (int.TryParse(textBox2.Text, out tmp))
            {
                if (c.machineStatus)
                {
                    decimal collectedAmount = 0;
                    decimal change = 0;
                    if (!c.DispenseAmount(tmp, out status))
                    {
                        MessageBox.Show("Error Collecting Payment " + status);
                    }
                    else
                    {
                        while (c.dispenseDoneAmount < tmp)
                        {
                            Thread.Sleep(1000);
                            displayMsg("Total Amount Dispensed = " + c.dispenseDoneAmount.ToString("N2"));
                        }
                        c.EndTransaction(out status);
                        
                    }

                }
                else
                {
                    MessageBox.Show("Disconnected");
                }


            }
            else
            {
                MessageBox.Show("Error Parsing the Amount");
            }*/
        }

        private bool setSerialPortParams(string portName, out string status)
        {
            try
            {
                status = "OK";
                
                if (!string.IsNullOrEmpty(portName))
                {
                    _serialPort.PortName = portName;
                    _serialPort.BaudRate = 9600;
                    _serialPort.Parity = 0;
                    _serialPort.DataBits = 8;
                    _serialPort.StopBits = StopBits.One;
                    //_serialPort.Handshake = NetsAPI.DEFAULT_HANDSHAKE;

                    _serialPort.ReadTimeout = 2000;
                    _serialPort.WriteTimeout = 2000;

                    return true;
                }
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
            }

            return false;
        }

        private void Weight_OnWeightChanged(object sender, WeightChangedArgs e)
        {
            displayMsg("Current Weight: " + e.currentWeight.ToString("N0") + ", diff :" + e.diff.ToString("N0"));
        }

        private void Weight_OnStatusChanged(object sender, StatusChangedArgs e)
        {
            displayMsg("Current Status: " + e.currentStatus);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            isRunning = true;
            c = new PowerPOSLib.Controller.WeighingMachineController();
            c.WeightChanged += new WeightChangedHandler(Weight_OnWeightChanged);
            c.StatusChanged += new StatusChangedHandler(Weight_OnStatusChanged);
            backgroundWorker1.RunWorkerAsync();
            /*string status = "";
            bool connected = false;
            if (setSerialPortParams(txtPort.Text, out status))
            {
                if (openPort(out status))
                {
                    MessageBox.Show("Connected");
                    connected = true;
                }
                else
                {
                    MessageBox.Show("Open Port Failed");
                }
            }
            else
            {
                MessageBox.Show("failed");
            }

            while (connected)
            {
                int dataLength = _serialPort.BytesToRead;
                byte[] data = new byte[dataLength];
                if (_serialPort.Read(data, 0, dataLength) != 0)
                {
                    if ((respIdx + dataLength) > DEFAULT_RESPONSE_LENGTH)
                    {
                        resetReadBuffer();
                        //readBuffer = new 
                        readBuffer = new byte[respIdx + dataLength];
                    }
                    //processResponse(data);
                    getResponseFromReadBuffer(data);
                    displayMsg("Received : " + Utilities.bytesArrayToHex(data));
                    displayMsg("Data Length : " + dataLength.ToString());
                    if (response.Length > 0)
                    {
                        
                        displayMsg("Resp : " + Utilities.bytesArrayToHex(response));
                        processResponse(response);
                    }
                }
                Thread.Sleep(3000);
            }*/
            

        }

        private void processResponse(byte[] data)
        {
            
            byte[] retval = new byte[4];

            int startIdx =  5;

            Array.Copy(response, startIdx, retval, 0, retval.Length);

            string tmp = Utilities.bytesArrayToASCIIString(retval);
            displayMsg("Weight:" + tmp);
        }

        public const byte STX = 0x02;
        public const byte ETX = 0x0D;

        //byte[] response;
        private bool getResponseFromReadBuffer(byte[] data)
        {
            bool res = true;
            int startIdx = 0;
            int etx = 0;
            while (startIdx < data.Length) 
            {
                if (data[startIdx] == STX)
                    break;
                ++startIdx;
                
            }
            etx = startIdx+1;
            while (etx < data.Length)
            {
                if (data[etx] == ETX)
                    break;
                ++etx;
            }
            //etx += 2;
            if (startIdx == data.Length || etx == data.Length)
            {
                return false;
            }
            else
            {
                response = new byte[etx - startIdx];
                //Logger.writeLog("Response Length : " + response.Length.ToString());
                Array.Copy(data, startIdx, response, 0, response.Length);
                return true;
            }

        }

        private void resetReadBuffer()
        {
            readBuffer = new byte[DEFAULT_RESPONSE_LENGTH];
            respIdx = 0;
            //finishedReading = false;
        }

        private bool openPort(out string status)
        {
            status = "OK";
            bool ok = true ;
            try
            {

                if (_serialPort.IsOpen)
                { _serialPort.Close(); Thread.Sleep(10); }
                else
                {
                    
                }

                _serialPort.Open();

                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();

                if (_serialPort.IsOpen)
                {
                    Logger.writeLog("Port (" + _serialPort.PortName + ") opened!");
                    status = "OK";
                    return true;
                }
                //}
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                ok = false;
                GC.ReRegisterForFinalize(_serialPort);
                
            }
            GC.SuppressFinalize(_serialPort);
            return false;
        }
        int _currentWeight = 0;
        int stableWeight = 0;
        PowerPOSLib.Controller.WeighingMachineController c = new PowerPOSLib.Controller.WeighingMachineController();
        

        private void bw1_DoWork(object sender, DoWorkEventArgs e)
        {
            string resultStatus = "";
            bs.init(out resultStatus);
            //c.init();
            /*string status = "";
            int stableRunningCount = 0;
            if (c.setupSerialPort(c.COMPort))
            {
                while (isRunning)
                {

                    //PowerPOSLib.Controller.WeighingMachineController c = new PowerPOSLib.Controller.WeighingMachineController();
                    int tmp = c.getWeight(out status);
                    if (!status.StartsWith("Error"))
                    {
                        if (tmp != -1)
                        {
                            if (Math.Abs(stableWeight - tmp) > 4)
                            {
                                
                                stableWeight = tmp;
                                stableRunningCount = 1;
                                //displayMsg("Stable Weight 1 : " + stableWeight.ToString("N0"));
                            }
                            else
                            {
                                stableRunningCount++;
                                //displayMsg("Stable Weight " + stableRunningCount.ToString());
                                if (stableRunningCount >= 3)
                                {
                                    if (Math.Abs(_currentWeight - stableWeight) > 4)
                                    {
                                        displayMsg("Weight : " + tmp.ToString("N0") + ", Diff : " + (tmp - _currentWeight).ToString("N2"));
                                        _currentWeight = stableWeight;
                                    }
                                    stableRunningCount = 0;
                                }
                            }
                        }
                        /*if (tmp != -1 && Math.Abs(_currentWeight - tmp) > 2)
                        {
                            //displayMsg("Weight : " + tmp.ToString("N0") + ", Diff : " + (tmp - _currentWeight).ToString("N2"));
                            //_currentWeight = tmp;
                        
                        }
                    }


                    Thread.Sleep(100);
                }
            }*/
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string status = "";
            if (setSerialPortParams(txtPort.Text, out status))
            {
                //_serialPort.Close();
                GC.SuppressFinalize(_serialPort);
                GC.ReRegisterForFinalize(_serialPort);
                
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            
            /*HH_Lib hwh = new HH_Lib();
            string[] HardwareList = hwh.GetAll();
            string[] devices = new string[1];
            devices[0] = txtHardwareName.Text;
            hwh.SetDeviceState(devices, false);*/
        }

        private void button9_Click(object sender, EventArgs e)
        {

            bs.COMPort = txtHardwareName.Text;
            displayMsg(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : Start Init");
            string status = "";
            if(!isBarcodeConnected)
            bs.init(out  status);
            /*if (BarcodeScannerController)
            {
                //displayMsg("BW Busy. No Rerun");
                return;
            }

            if (isBarcodeConnected)
            {
                //displayMsg("Barcode Connected. No Rerun");
                return;
            }
            
                
                bw1.RunWorkerAsync();*/

                
            

            /*string status = "";
            bool connected = false;
            if (setSerialPortParams("COM5", out status))
            {
                if (openPort(out status))
                {
                    MessageBox.Show("Connected");
                    connected = true;
                }
                else
                {
                    MessageBox.Show("Open Port Failed");
                }
            }
            else
            {
                MessageBox.Show("failed");
            }


            while (connected)
            {
                int dataLength = _serialPort.BytesToRead;
                byte[] data = new byte[dataLength];
                if (_serialPort.Read(data, 0, dataLength) != 0)
                {
                    if ((respIdx + dataLength) > DEFAULT_RESPONSE_LENGTH)
                    {
                        resetReadBuffer();
                        //readBuffer = new 
                        readBuffer = new byte[respIdx + dataLength];
                    }
                    //processResponse(data);
                    //getResponseFromReadBuffer(data);
                    displayMsg("Received : " + Utilities.bytesArrayToHex(data));
                    displayMsg("Data Length : " + dataLength.ToString());
                    
                    //if (response.Length > 0)
                    //{

                        //displayMsg("Resp : " + Utilities.bytesArrayToHex(response));
                        //processResponse(response);
                    //}
                }
                Thread.Sleep(100);
            }*/
        }

        string enableDevice = "D:\\Adi\\EnableDeviceShortcut.lnk";
        string disableDevice = "D:\\Adi\\DisableDeviceShortcut.lnk";

        private void button10_Click(object sender, EventArgs e)
        {
            /*ProcessStartInfo startInfo = new ProcessStartInfo(enableDevice);
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            System.Diagnostics.Process.Start(startInfo);*/
            //System.Diagnostics.Process.Start(enableDevice);
            bs.enableDevice();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            /*ProcessStartInfo startInfo = new ProcessStartInfo(disableDevice);
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            System.Diagnostics.Process.Start(startInfo);*/
            //bw1.CancelAsync();
            bs.disableDevice();
            //bs.isActive = false;
        }

        private void Barcode_OnBarcodeChanged(object sender, BarcodeScannedArgs e)
        {
            displayMsg("Barcode Scanned: " + e.lastBarcode);
            string status = "";
            Thread.Sleep(100);
            bs.init(out status);
            //timer1.Interval = 1000;
            //timer1.Enabled = true;
        }

        private void Barcode_OnDeviceStateChanged(object sender, DeviceStateChangedArgs e)
        {
            isBarcodeConnected = e.IsEnabled;
            displayMsg("Enabled: " + e.IsEnabled.ToString());

        }

        private void Barcode_OnLog(object sender, LoggingArgs e)
        {
//            isBarcodeConnected = e.IsEnabled;
            displayMsg("Response: " + e.Log);

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            c.Dispose();
            bs.Dispose();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string status1 = "";
            if (!c.init( out status1))
            {
                displayMsg(status1);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (c.getStatus()
        }

        private void btnNetsCheckStatus_Click(object sender, EventArgs e)
        {
            /*NetsController nets = new NetsController();
            
            string status = "";
            bool machineStatus = false;
            if (nets.setupSerialPort(txtNetsComPort.Text))
            {
                nets.requestTerminalStatus(out status, out machineStatus);
                displayMsg("Machine : " + machineStatus.ToString() + ", status : " + status);
            }*/
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //button9_Click(sender, e);
            if (!isBarcodeConnected)
            button9.PerformClick();
            //timer1.Enabled = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void bw1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //timer1.Enabled = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            bs.disableDevice();
            bs.Dispose();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            bs = new BarcodeScannerController();
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {

        }

    }
}