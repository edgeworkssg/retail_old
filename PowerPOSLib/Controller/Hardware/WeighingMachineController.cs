using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS.EZLink;
using System.IO.Ports;
using PowerPOS;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

namespace PowerPOSLib.Controller
{
    public class WeightChangedArgs : EventArgs
    {
        private int _currentWeight;
        //private int _previousWeight;
        private int _diff;

        

        public WeightChangedArgs(int currentWeight, int diff)
        {
            _currentWeight = currentWeight;
            //_previousWeight = previousWeight;
            _diff = diff;
        }

        public int currentWeight
        {
            get
            {
                return _currentWeight;
            }
        }

        public int diff
        {
            get
            {
                return _diff;
            }
        }
    }

    public class StatusChangedArgs : EventArgs
    {
        private string _currentStatus;
        //private int _previousWeight;


        public StatusChangedArgs(string status)
        {
            _currentStatus = status;
        }

        public string currentStatus
        {
            get
            {
                return _currentStatus;
            }
        }

    }

    public delegate void WeightChangedHandler(WeighingMachineController m, WeightChangedArgs e);
    public delegate void StatusChangedHandler(WeighingMachineController m, StatusChangedArgs e);

    public class WeighingMachineController : IDisposable
    {
        //COM Port
        SerialPort _serialPort;
        public string COMPort = AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.COMPort);
        byte[] response;
        int respIdx = 0;

        //variable
        bool isConnected = false;
        int _currentWeight = 0;
        int stableWeight = 0;
        private bool disposed = false;

        public int readCount = 5;
        
        //Events
        public event WeightChangedHandler WeightChanged;
        
        public event StatusChangedHandler StatusChanged;

        private int weightTolerance = 4;

        public int waitTime=100;

        public WeighingMachineController()
        {
            _serialPort = new SerialPort();
            isConnected = false;
            _currentWeight = 0;
            stableWeight = 0;
            string toleranceTemp = AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.Tolerance);
            if (!int.TryParse(toleranceTemp, out weightTolerance))
                weightTolerance = 4;

            string tmp = ConfigurationManager.AppSettings["WeighingMachineReadCount"];
            if (!int.TryParse(tmp, out readCount))
                readCount = 5;
        }

        #region Getter
        public string getStatus()
        {
            if (isConnected)
                return "Connected";
            else
                return "Disconnected";
        }

        public int getWeight()
        {
            return _currentWeight;
        }
        #endregion

        public bool init(out string status)
        {
            try
            {
                int errorCount = 0;
                status = "";
                int stableRunningCount = 0;

                if (!setupSerialPort(COMPort, out status))
                {
                    isConnected = false;
                    return false;
                }
                
                isConnected = true;
                stableRunningCount = 0;
                _currentWeight = 0;
                stableWeight = 0;
                StatusChangedArgs e = new StatusChangedArgs(getStatus());
                OnStatusChanged(e);
                
                
                while (true)
                {
                    /*if (!isConnected)
                    {
                        if (setupSerialPort(COMPort, out status))
                        {
                            isConnected = true;
                            stableRunningCount = 0;
                            _currentWeight = 0;
                            stableWeight = 0;
                            e = new StatusChangedArgs(getStatus());
                            OnStatusChanged(e);
                        }
                    }
                    else
                    {*/
                        int tmp = getWeight(out status);
                        if (!status.ToLower().StartsWith("error"))
                        {
                            if (!isConnected)
                            {
                                isConnected = true;
                                stableRunningCount = 0;
                                StatusChangedArgs se = new StatusChangedArgs(getStatus());
                                OnStatusChanged(se);
                            }
                            if (tmp != -1)
                            {
                                if (Math.Abs(stableWeight - tmp) > 0)
                                {

                                    stableWeight = tmp;
                                    stableRunningCount = 1;
                                    //displayMsg("Stable Weight 1 : " + stableWeight.ToString("N0"));
                                }
                                else
                                {
                                    stableRunningCount++;
                                    //displayMsg("Stable Weight " + stableRunningCount.ToString());
                                    if (stableRunningCount >= readCount)
                                    {
                                        //if (Math.Abs(_currentWeight - stableWeight) > 4)
                                        //{
                                            //displayMsg("Weight : " + tmp.ToString("N0") + ", Diff : " + (tmp - _currentWeight).ToString("N2"));
                                            WeightChangedArgs we = new WeightChangedArgs(
                                            stableWeight, stableWeight - _currentWeight);
                                            _currentWeight = stableWeight;
                                            OnWeightChanged(we);
                                        //}
                                        stableRunningCount = 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            errorCount++;
                            if (errorCount >= 3)
                            {
                                if (isConnected)
                                {
                                    isConnected = false;
                                    errorCount = 0;

                                    StatusChangedArgs se = new StatusChangedArgs(getStatus());
                                    OnStatusChanged(se);
                                }
                            }
                        }
                    //}

                    Thread.Sleep(waitTime);
                }
                return true; 


            }
            catch (Exception ex)
            {
                //Logger.writeLog("Error On Get Weight Function : " + ex.Message);
                isConnected = false;
                //string status1 = "";
                //closePort(out status1);
                status = "Error On Get Weight Function : " + ex.Message;
                return false;
            }
        }

        protected virtual void OnWeightChanged(WeightChangedArgs e)
        {
            if (WeightChanged != null)
            {
                WeightChanged(this, e);//Raise the event
            }
        }

        protected virtual void OnStatusChanged(StatusChangedArgs e)
        {
            if (StatusChanged != null)
            {
                StatusChanged(this, e);//Raise the event
            }
        }

        #region payment controller
        public int getWeight(out string status)
        {

            status = "";
            int res = 0;
            try
            {
                
                    int dataLength = _serialPort.BytesToRead;
                    
                    byte[] data = new byte[dataLength];
                    if (_serialPort.Read(data, 0, dataLength) != 0)
                    {
                        getResponseFromReadBuffer(data);

                        if (response != null && response.Length > 0)
                        {
                            byte[] retval = new byte[6];
                            
                            int startIdx = 5;
                            Array.Copy(response, startIdx, retval, 0, retval.Length);
                            string tmp = Utilities.bytesArrayToASCIIString(retval);

                            if (!int.TryParse(tmp, out res))
                                res = -1;
                        }
                        else
                        {
                            status = "Error. Cannot get response from machine";
                            Logger.writeLog(status);
                            res = 0;
                        }
                }
                else
                {
                    status = "Error. Cannot connect to the port";
                    Logger.writeLog(status);
                    res = -1;
                }
                return res;
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                //string status1 = "";
                //bool res1 = this.closePort(out status1);
                return -1;
            }
            
        }

        
        
        #endregion

        #region Functions to Setup Serial Port
        /// <summary>
        /// Setup serial port then open it
        /// </summary>
        /// <param name="portName">Port name to be opened</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool setupSerialPort(string portName, out string status)
        {
            status = "";

            if (setSerialPortParams(portName, out status))
            {
                if (openPort(out status))
                {
                    return true;
                }
            }

            return false;
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
                    Logger.writeLog("> Port (" + _serialPort.PortName + ") closed!");
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
                //if (_serialPort.IsOpen)
                //{
                _serialPort.Close(); //}

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
                Logger.writeLog("ERROR Connecting To Weighing Machine." + ex.Message.ToString());
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
                
                if (!string.IsNullOrEmpty(portName))
                {
                    _serialPort.Close();
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
            while (startIdx < data.Length)
            {
                if (data[startIdx] == STX)
                    break;
                ++startIdx;

            }
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
                if (_serialPort != null)
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

        
    }
}
