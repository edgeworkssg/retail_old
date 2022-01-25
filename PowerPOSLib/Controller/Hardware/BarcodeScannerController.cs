using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using PowerPOS;
using PowerPOS.EZLink;
using System.Threading;
using System.IO;

namespace PowerPOSLib.Controller
{
    public class BarcodeScannedArgs : EventArgs
    {
        private string _scannedBarcode;
        
        public BarcodeScannedArgs(string barcode)
        {
            _scannedBarcode = barcode;
        }

        public string lastBarcode
        {
            get
            {
                return _scannedBarcode;
            }
        }
        
    }

    public class DeviceStateChangedArgs : EventArgs
    {
        private bool isenabled = false;

        public DeviceStateChangedArgs(bool _isEnabled)
        {
            isenabled = _isEnabled;
        }

        public bool IsEnabled
        {
            get
            {
                return isenabled;
            }
        }

    }

    public class LoggingArgs : EventArgs
    {
        private string log = "";

        public LoggingArgs(string _log)
        {
            log = _log;
        }

        public string Log
        {
            get
            {
                return log;
            }
        }

    }

    public delegate void BarcodeScannedHandler(BarcodeScannerController m, BarcodeScannedArgs e);

    public delegate void DeviceStateChangedHandler(BarcodeScannerController m, DeviceStateChangedArgs e);

    public delegate void LoggingHandler(BarcodeScannerController m, LoggingArgs e);

    public class BarcodeScannerController : IDisposable
    {

        byte[] enableDeviceCommand; //= AppSetting.GetSetting(AppSetting.SettingsName.BarcodeScanner.enableCommand);
        byte[] disableDeviceCommand; //= AppSetting.GetSetting(AppSetting.SettingsName.BarcodeScanner.disableCommand);
        public bool isActive = false;

        SerialPort _serialPort;
        public string COMPort = AppSetting.GetSetting(AppSetting.SettingsName.BarcodeScanner.COMPort);
        byte[] response;
        int respIdx = 0;

        public string currentBarcode;
        bool isConnected;
        private bool disposed = false;

        //Events
        public event BarcodeScannedHandler BarcodeScanned;
        public event DeviceStateChangedHandler DeviceStateChanged;
        public event LoggingHandler Logged;

        public const byte ACK = 0x06;

        public byte[] enableCommand;
        public byte[] disableCommand;

        public BarcodeScannerController()
        {
            _serialPort = new SerialPort();
            disableDeviceCommand = Utilities.hexStrToBytesArray(AppSetting.GetSetting(AppSetting.SettingsName.BarcodeScanner.disableCommand));
            
            enableDeviceCommand = Utilities.hexStrToBytesArray(AppSetting.GetSetting(AppSetting.SettingsName.BarcodeScanner.enableCommand));
            _serialPort.DataReceived += OnScan;

        }

        public void OnScan(object sender, SerialDataReceivedEventArgs args)
        {
            SerialPort port = sender as SerialPort;
            string line = port.ReadExisting();
            //string tmp = Utilities.bytesArrayToASCIIString(line);
            //Thread.Sleep(1000);
            isActive = false;
            OnDeviceStateChanged(new DeviceStateChangedArgs(isActive));
            OnBarcodeScanned(new BarcodeScannedArgs(line));
            
        }


        public bool enableDevice()
        {
            try
            {
                //if (isComPortExist(COMPort))
                //    return false;

                if (!_serialPort.IsOpen)
                    return false;

                //byte[] tmp = enableDeviceCommand;
                //OnLogging(new LoggingArgs("Sending Enable Command"));
                _serialPort.Write(enableDeviceCommand, 0, enableDeviceCommand.Length);

                #region Obsolete
                /*bool isACK = false;
                while (!isACK)
                {
                    int dataLength = _serialPort.BytesToRead;
                    byte[] data = new byte[dataLength];
                    if (_serialPort.Read(data, 0, dataLength) != 0)
                    {
                        Logger.writeLog("Received : " + Utilities.bytesArrayToHex(data));
                        if (data[0] == ACK)
                        {
                            isACK = true;
                        }
                    }
                }*/
                //_serialPort.Write(enableDeviceCommand);

                /*Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                string argument = enableDeviceCommand;//"/c start /min D:\\Adi\\enableDevice.bat";
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                proc.StartInfo.Arguments = argument;
                //proc.StartInfo.CreateNoWindow = true;
                //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.Verb = "runas";
                proc.Start();*/



                /*string argument = "start /min D:\\Adi\\disableDevice.bat ^& exit";
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                proc.StartInfo.Arguments = argument;
                proc.StartInfo.UseShellExecute = true;
                proc.Start();*/
                /*ProcessStartInfo startInfo = new ProcessStartInfo(enableDeviceCommand);
                startInfo.CreateNoWindow = true;
                startInfo.WindowStyle = ProcessWindowStyle.Minimized;
                
                System.Diagnostics.Process.Start(   );*/
                //ExecuteCMD(enableDeviceCommand);
                #endregion

                isActive = true;
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool disableDevice()
        {
            try
            {
                #region Obsolete
                /*string status1 = "";
                
                bool res1 = this.closePort(out status1);
                while (_serialPort.IsOpen)
                {
                    Thread.Sleep(10);
                }*/

                /*ProcessStartInfo startInfo = new ProcessStartInfo(disableDeviceCommand);
                startInfo.CreateNoWindow = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //startInfo.
                var process = System.Diagnostics.Process.Start(startInfo);*/


                /*Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                string argument = "/c start /min D:\\Adi\\disableDevice.bat";//disableDeviceCommand;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                proc.StartInfo.Arguments = argument;
                //proc.StartInfo.CreateNoWindow = true;
                //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.Verb = "runas";
                proc.Start();
                proc.WaitForExit()*/
                //ExecuteCMD(disableDeviceCommand);
                #endregion
                //byte[] tmp = Utilities.hexStrToBytesArray(disableDeviceCommand);
                //OnLogging(new LoggingArgs("Sending Disable Command"));
                _serialPort.Write(disableDeviceCommand, 0, disableDeviceCommand.Length);
                
                isActive = false;
                OnDeviceStateChanged(new DeviceStateChangedArgs(isActive));
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool initialize(out string resultStatus)
        {
            resultStatus = "";
            try
            {
                string resultSetupSerialPort = "";

                if (!setupSerialPort(COMPort, out resultSetupSerialPort))
                {
                    resultStatus = "Error Setup COM PORT. " + resultSetupSerialPort;
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                resultStatus = "ERROR. " + ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool init(out string resultStatus)
        {
            resultStatus = "";
            try
            {
                if (enableDevice())
                {
                    OnDeviceStateChanged(new DeviceStateChangedArgs(true));
                    isActive = true;
                    isConnected = true;

                }
                #region Obsolete
                //Thread.Sleep(500);
                
                /*string resultSetupSerialPort = "";
                
                if (setupSerialPort(COMPort, out resultSetupSerialPort))
                {
                    OnDeviceStateChanged(new DeviceStateChangedArgs(true));
                    isConnected = true;
                    isActive = true;
                }
                else
                {
                    resultStatus = "Failed setup serial port " + COMPort + ". " + resultSetupSerialPort;
                    OnLogging(new LoggingArgs("Failed setup serial port " + COMPort + ". " + resultSetupSerialPort));
                    isConnected = false;
                }*/

                //string scannedBarcode = "";
                /*while (isActive && isConnected)
                {
                    string status = "";
                    scannedBarcode = getBarcodeString(out status);
                    if (!string.IsNullOrEmpty(scannedBarcode))
                        break;
                    Thread.Sleep(50);
                }*/
                //if (isActive) // Check the is active so we can cancel the waiting
                //    OnBarcodeScanned(new BarcodeScannedArgs(scannedBarcode));

                //disableDevice();
                /*while (true)
                {
                    if (!isComPortExist(COMPort))
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }*/
                //Thread.Sleep(1000);
                //isActive = false;
                //OnDeviceStateChanged(new DeviceStateChangedArgs(false));
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                resultStatus = "ERROR. "+ex.Message;
                Logger.writeLog(ex);
                return false; 
            }
        }

        public string getBarcodeString(out string status)
        {

            status = "";
            string res = "";
            try
            {
                
                int dataLength = _serialPort.BytesToRead;                
                byte[] data = new byte[dataLength];
                if (_serialPort.Read(data, 0, dataLength) != 0)
                {
                    //OnLogging(new LoggingArgs("Initial Read : " + Utilities.bytesArrayToASCIIString(data)));
                    //getResponseFromReadBuffer(data);
                    //OnLogging(new LoggingArgs(response));
                    //if (response != null && response.Length > 0)
                    //{

                    string tmp = Utilities.bytesArrayToASCIIString(data);

                    if (!string.IsNullOrEmpty(tmp))
                        res = tmp;
                    /*}
                    else
                    {
                        status = "Error. Cannot get response from machine";
                        Logger.writeLog(status);
                        res = "";
                    }*/
                    //}
                }
                
                //string status1 = "";
                //bool res1 = this.closePort(out status1);

                return res;
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex);
                string status1 = "";
                bool res1 = this.closePort(out status1);
                return "";
            }

        }

        protected virtual void OnBarcodeScanned(BarcodeScannedArgs e)
        {
            if (BarcodeScanned != null)
            {
                BarcodeScanned(this, e);//Raise the event
            }
        }

        protected virtual void OnDeviceStateChanged(DeviceStateChangedArgs e)
        {
            if (DeviceStateChanged != null)
            {
                DeviceStateChanged(this, e);//Raise the event
            }
        }

        protected virtual void OnLogging(LoggingArgs e)
        {
            if (Logged != null)
            {
                Logged(this, e);//Raise the event
            }
        }

        private bool isComPortExist(string portName)
        {
            string[] ports = SerialPort.GetPortNames();

            foreach (string prtName in ports)
            {
                if (prtName == portName)
                    return true;
            }
            return false;
            //SerialPort port = new SerialPort(ports[0]);

        }

        #region Functions to Setup Serial Port
        /// <summary>
        /// Setup serial port then open it
        /// </summary>
        /// <param name="portName">Port name to be opened</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool setupSerialPort(string portName, out string status)
        {
            bool waitingToConnect = true;
            status = "";

            if (_serialPort.IsOpen)
                return true;
            if (setSerialPortParams(portName, out status))
            {
                if (!openPort(out status))
                {
                    OnLogging(new LoggingArgs("Error Opening Port : " + status));
                    return false;
                    //return true;
                }
                /*else
                {
                    
                    
                }*/
            }
            Thread.Sleep(10);
            
            return true;
        }

        /// <summary>
        /// To close serial port
        /// </summary>
        /// <returns>True if successful, otherwise false</returns>
        private bool closePort(out string status)
        {
            status = "OK";
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();

                    Logger.writeLog("Port (" + _serialPort.PortName + ") closed!");
                    return true;
                }
                else
                {
                    GC.SuppressFinalize(_serialPort);
                }
            }
            catch (Exception ex)
            {
                status = "Error. Cannot Close Port";
            }

            return false;
        }

        /// <summary>
        /// To open serial port
        /// </summary>
        /// <returns>True if succesful, otherwise false</returns>
        private bool openPort(out string status)
        {
            status = "OK";
            try
            {
                //_serialPort.DiscardInBuffer();
                //_serialPort.DiscardOutBuffer();

                //if (attachReceiveEventHandler())
                //{
                if (_serialPort.IsOpen)
                { _serialPort.Close(); }
                
                _serialPort.Open();
                

                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();

                while (!_serialPort.IsOpen)
                {
                    Thread.Sleep(10);
                }
                //Logger.writeLog("Port (" + _serialPort.PortName + ") opened!");
                status = "OK";
                return true;
                //}
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                GC.ReRegisterForFinalize(_serialPort);
            }
            finally
            {
                if (_serialPort.IsOpen)
                {
                    GC.SuppressFinalize(_serialPort.BaseStream);
                }
            }

            return false;
        }

        /// <summary>
        /// Set the parameters for serial port, to be called before opening serial port
        /// </summary>
        /// <param name="portName">Portname to be opened</param>
        /// <returns>True if succesful, otherwise false</returns>
        private bool setSerialPortParams(string portName, out string status)
        {
            try
            {
                status = "OK";
                if (!string.IsNullOrEmpty(portName) )
                {
                    _serialPort.PortName = portName;
                    _serialPort.BaudRate = EZLinkAPI.DEFAULT_BAUDRATE;
                    _serialPort.Parity = EZLinkAPI.DEFAULT_PARITY;
                    _serialPort.DataBits = EZLinkAPI.DEFAULT_DATABITS;
                    _serialPort.StopBits = EZLinkAPI.DEFAULT_STOPBITS;
                    //_serialPort.Handshake = NetsAPI.DEFAULT_HANDSHAKE;

                    _serialPort.ReadTimeout = EZLinkAPI.DEFAULT_TIMEOUT;
                    _serialPort.WriteTimeout = EZLinkAPI.DEFAULT_TIMEOUT;

                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "ERROR. " + ex.Message.ToString();
            }

            return false;
        }
        #endregion

        #region Serial Port operations

        /// <summary>
        /// Reset read buffer so that previous received response can be cleared
        /// </summary>
        private void resetReadBuffer()
        {
            //readBuffer = new byte[1024];
            respIdx = 0;
            //finishedReading = false;
        }


        public const byte STX = 0x02;
        public const byte ETX = 0x0D;
        /// <summary>
        /// Response from NETS machine will include 0x06 (ACK) byte, therefore retrieve the needed
        /// response from read buffer
        /// </summary>
        private bool getResponseFromReadBuffer(byte[] data)
        {
            bool res = true;
            int startIdx = 0;
            int etx = 0;
            etx = startIdx + 1;
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


        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                Stream sbase = null;
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    sbase = _serialPort.BaseStream;
                    _serialPort.Close();
                }
                /*
                ** because of the issue with the FTDI USB serial device,
                ** the call to the stream's finalize is suppressed
                **
                ** an attempt to un-suppress the stream's finalize is made
                ** here, but if it fails, the exception is caught and
                ** ignored
                */
                if (sbase != null)
                {
                    GC.ReRegisterForFinalize(sbase);
                }

                if (_serialPort != null)
                {
                    GC.ReRegisterForFinalize(_serialPort);

                    if (disposing)
                    {
                        _serialPort.Dispose();
                    }
                }

                this.disposed = true;
            }
        }

        #endregion

        #region CMD

        //public void ExecuteCMD(object command)
        //{
        //    try
        //    {
        //        System.Diagnostics.ProcessStartInfo procStartInfo =
        //            new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);
        //        procStartInfo.RedirectStandardOutput = true;
        //        procStartInfo.UseShellExecute = false;
        //        procStartInfo.Verb = "runas";
        //        procStartInfo.CreateNoWindow = true;
        //        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        //        proc.StartInfo = procStartInfo;
        //        proc.Start();
        //        string result = proc.StandardOutput.ReadToEnd();

        //        Logger.writeLog(string.Format("CMD {0} Result : {1}", command, result));
        //    }
        //    catch (Exception objException)
        //    {
        //        Logger.writeLog(objException);
        //    }
        //}

        //public void ExecuteCMD(string command)
        //{

        //    ProcessStartInfo procStartInfo = new ProcessStartInfo()
        //    {
        //        RedirectStandardError = true,
        //        RedirectStandardOutput = true,
        //        UseShellExecute = false,
        //        CreateNoWindow = true,
        //        FileName = "runas.exe",
        //        Arguments = "/user:Administrator \"cmd /K " + command + "\""
        //    };

        //    using (Process proc = new Process())
        //    {
        //        proc.StartInfo = procStartInfo;
        //        proc.Start();

        //        //string output = proc.StandardOutput.ReadToEnd();

        //        //if (string.IsNullOrEmpty(output))
        //        //    output = proc.StandardError.ReadToEnd();

        //        //return output;
        //    }
        //}

        #endregion
    }
}
