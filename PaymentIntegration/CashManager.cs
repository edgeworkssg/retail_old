using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICashManagement;
using System.Configuration;
using SubSonic;
using System.Data;
using PowerPOS.Container;
//using CashManagementLib;


namespace PowerPOS
{
    public class CashReturnedArgs : EventArgs
    {
        private int r1, r2, r3, r4;
        private decimal amt;

        public CashReturnedArgs(int _R1, int _R2, int _R3, int _R4, decimal _amt)
        {
            r1 = _R1;
            r2 = _R2;
            r3 = _R3;
            r4 = _R4;
            amt = _amt;
        }

        public int R1
        {
            get
            {
                return r1;
            }
        }
        public int R2
        {
            get
            {
                return r2;
            }
        }
        public int R3
        {
            get
            {
                return r3;
            }
        }
        public int R4
        {
            get
            {
                return r4;
            }
        }
        public decimal Amt
        {
            get
            {
                return amt;
            }
        }
    }

    
    
    public class CashLogArgs : EventArgs
    {
        private string log;

        public CashLogArgs(string _log)
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

    public class AcceptCashCompletedArgs : EventArgs
    {
        private string errorMsg;
        private Boolean success;

        public AcceptCashCompletedArgs(Boolean _success, string _errorMsg)
        {
            success = _success;
            errorMsg = _errorMsg;
        }
        public string ErrorMsg
        {
            get
            {
                return errorMsg;
            }
        }
        public Boolean Success
        {
            get
            {
                return success;
            }
        }
    }

    public class CashReceivedArgs : EventArgs
    {
        private decimal denomination;
        private decimal amount;

        public CashReceivedArgs(decimal _denomination, decimal _amount)
        {
            denomination = _denomination;
            amount = _amount;
        }
        public decimal Denomination
        {
            get
            {
                return denomination;
            }
        }
        public decimal Amount
        {
            get
            {
                return amount;
            }
        }
    }

    public class CashErrorArgs : EventArgs
    {
        private string errorLog;

        public CashErrorArgs(string _errorlog)
        {
            errorLog = _errorlog;
        }
        public string ErrorLog
        {
            get
            {
                return errorLog;
            }
        }
    }

    public class CashStatusChangedArgs: EventArgs
    {
        private CashStatusInfo info;

        public CashStatusChangedArgs(CashStatusInfo _info)
        {
            info = _info;
        }
        public CashStatusInfo Info
        {
            get
            {
                return info;
            }
        }
    }

    public class OpeningBalanceOverwrittenArgs : EventArgs
    {
        private CashStatusInfo info;
        private decimal totalDiffNotes = 0;
        private decimal totalDiffCoins = 0;
        private decimal totalNotesEscrow = 0;
        private string refno = "";

        public OpeningBalanceOverwrittenArgs(CashStatusInfo _info)
        {
            info = _info;
            totalDiffNotes = _info.NoteRecyclersTotalAmount;
            totalDiffCoins = _info.CoinRecyclersTotalAmount;
            totalNotesEscrow = _info.NoteEscrowTotalAmount;
            refno = _info.MachineID;
        }
        public CashStatusInfo Info
        {
            get
            {
                return info;
            }
        }

        public decimal TotalDiffNotes
        {
            get { return totalDiffNotes; }
        }
        public decimal TotalDiffCoins
        {
            get { return totalDiffCoins; }
        }
        public decimal TotalNotesEscrow
        {
            get { return totalNotesEscrow; }
        }

        public string Refno
        {
            get { return refno; }
        }
    }

    public class ReceiveFloatCompletedArgs : EventArgs
    {
        
        private string lastTransactionID;
        private string beforeSnapshot, afterSnapshot;

        public ReceiveFloatCompletedArgs(string _lastTransactionID, string _beforeSnapshot, string _afterSnapshot)
        {
            lastTransactionID = _lastTransactionID;
            beforeSnapshot = _beforeSnapshot;
            afterSnapshot = _afterSnapshot;
        }

        public string LastTransactionID
        {
            get
            {
                return lastTransactionID;
            }
        }
        public string BeforeSnapshot
        {
            get
            {
                return beforeSnapshot;
            }
        }
        public string AfterSnapshot
        {
            get
            {
                return afterSnapshot;
            }
        }
        
    }

    public class NoteCheckOutCompletedArgs : EventArgs
    {

        private string lastTransactionID;
        private string beforeSnapshot, afterSnapshot;

        public NoteCheckOutCompletedArgs(string _lastTransactionID, string _beforeSnapshot, string _afterSnapshot)
        {
            lastTransactionID = _lastTransactionID;
            beforeSnapshot = _beforeSnapshot;
            afterSnapshot = _afterSnapshot;
        }

        public string LastTransactionID
        {
            get
            {
                return lastTransactionID;
            }
        }
        public string BeforeSnapshot
        {
            get
            {
                return beforeSnapshot;
            }
        }
        public string AfterSnapshot
        {
            get
            {
                return afterSnapshot;
            }
        }

    }

    public class MachineErrorArgs : EventArgs
    {

        private string errorCode;
        private string url;

        public MachineErrorArgs(string _errorCode, string _url)
        {
            errorCode = _errorCode;
            url = _url;
            //afterSnapshot = _afterSnapshot;
        }

        public string ErrorCode
        {
            get
            {
                return errorCode;
            }
        }
        public string URL
        {
            get
            {
                return url;
            }
        }
    }

    public class ReceiveCashErrorArgs : EventArgs
    {

        private string errorCode;
        private string errorMessage;
        private string url;

        public ReceiveCashErrorArgs(string _errorCode, string _errorMessage, string _url)
        {
            errorCode = _errorCode;
            errorMessage = _errorMessage;
            url = _url;
            //afterSnapshot = _afterSnapshot;
        }

        public string ErrorCode
        {
            get
            {
                return errorCode;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
        }
        public string URL
        {
            get
            {
                return url;
            }
        }
    }

    public delegate void CashReturnedHandler(CashManager m, CashReturnedArgs e);
    public delegate void CashCollectedHandler(CashManager m);
    public delegate void CashLogHandler(CashManager m, CashLogArgs e);
    public delegate void CashReceivedHandler(CashManager m, CashReceivedArgs e);
    public delegate void CashErrorHandler(CashManager m, CashErrorArgs e);
    public delegate void CashOutNoteHandler(CashManager m);
    public delegate void CashMachineConnectedHandler(CashManager m);
    public delegate void CashMachineDisconnectedHandler(CashManager m);
    public delegate void CashStatusChangedHandler(CashManager m, CashStatusChangedArgs e);
    public delegate void ReceiveFloatCompletedHandler(CashManager m, ReceiveFloatCompletedArgs e);
    public delegate void NoteCheckoutCompletedHandler(CashManager m, NoteCheckOutCompletedArgs e);
    public delegate void MachineErrorHandler(CashManager m, MachineErrorArgs e);
    public delegate void CashInEscrowHandler(CashManager m);
    public delegate void AcceptCashCompletedHandler(CashManager m, AcceptCashCompletedArgs e);
    public delegate void OpeningBalanceOverwrittenHandler(CashManager m, OpeningBalanceOverwrittenArgs e);
    public delegate void ReceiveCashErrorHandler(CashManager m, ReceiveCashErrorArgs e);


    public class CashManager
    {
        #region constants
        public const string MACHINETYPE_NOTES = "NOTES";
        public const string MACHINETYPE_COINS = "COINS";

        #endregion


        private ICashMachine m_Cash;
        public bool isDispensing = false;
        public bool isAccepting = false;
        public bool isReceivingFloat = false;
        public bool isConnected = false;
        public bool isConnecting = false;

        private CashStatusInfo test;
        private string currentState;
        private string currentLog;
        private decimal amountToPay;
        private decimal amountReceived;
        private CashStatusInfo m_CashStatus;

        public string orderRefno = "";

        public bool isMachineError = false;

        //Events
        public event CashReturnedHandler CashReturned;
        public event CashCollectedHandler CashCollected;
        public event CashLogHandler CashLogged;
        public event CashReceivedHandler CashReceived;
        public event CashErrorHandler CashError;
        public event CashOutNoteHandler NoteCashOut;
        public event CashMachineConnectedHandler CashMachineConnected;
        public event CashMachineDisconnectedHandler CashMachineDisconnected;
        public event CashStatusChangedHandler CashStatusChanged;
        public event ReceiveFloatCompletedHandler ReceiveFloatCompleted;
        public event NoteCheckoutCompletedHandler NoteCheckoutCompleted;
        public event MachineErrorHandler MachineError;
        public event CashInEscrowHandler CashInEscrow;
        public event AcceptCashCompletedHandler AcceptCashCompleted;
        public event OpeningBalanceOverwrittenHandler OpeningBalanceOverwritten;
        public event ReceiveCashErrorHandler ReceiveCashError;
 

        //CashMachineSnapshot snapShot;

        #region Init 
        public CashManager()
        {
            //m_Cash = new ICashMachine();
        }

        public bool Init(string url, out string status)
        {
            status = "";
            try{
                //m_Cash = CashMachinePlugin.GetInstance<ICashMachine>(url);
                m_Cash = CashMachinePlugin.GetCashMachine(url);
                //Event for Cash Dispense
                m_Cash.CashReturned += new ICashMachine.CashReturnedEventHandler(cashReturnedEvent);
                m_Cash.CashOutCollected += new ICashMachine.CashOutCollectedEventHandler(cashOutCollectedEvent);
                m_Cash.CashLog += new ICashMachine.CashLogEventHandler(cashLogEvent);
                m_Cash.AcceptCashCompleted += new ICashMachine.AcceptCashCompletedEventHandler(AcceptCashCompletedEvent);

                //Event for Cash Deposit
                m_Cash.CashInEscrow += new ICashMachine.CashInEscrowEventHandler(CashInEscrowEvent);
                m_Cash.CashReceived += new ICashMachine.CashReceivedEventHandler(cashReceivedEvent);
                //Event for Cash Error 
                m_Cash.CashError += new ICashMachine.CashErrorEventHandler(cashErrorEvent);

                //Event for Event Status
                m_Cash.Connected += new ICashMachine.ConnectedEventHandler(connectedEvent);
                m_Cash.Disconnected += new ICashMachine.DisconnectedEventHandler(disconnectedEvent);
                m_Cash.CashOutNote += new ICashMachine.CashOutNoteEventHandler(cashOutNoteEvent);
                m_Cash.CashStatusChanged += new ICashMachine.CashStatusChangedEventHandler(CashStatusChangedEvent);

                //Opening Balance 
                m_Cash.OpeningBalance += new ICashMachine.OpeningBalanceEventHandler(openingBalanceEvent);
                m_Cash.OpeningBalanceOverwritten += new ICashMachine.OpeningBalanceOverwrittenEventHandler(openingBalanceOverwrittenEvent);

                //Float Finished
                m_Cash.ReceiveFloatFromEntranceCompleted += new ICashMachine.ReceiveFloatFromEntranceCompletedEventHandler(ReceiveFloatFromEntranceCompleted);
                m_Cash.NoteCheckOutCompleted += new ICashMachine.NoteCheckOutCompletedEventHandler(NoteCheckOutCompletedEvent);
                m_Cash.CoinCheckOutCompleted += new ICashMachine.CoinCheckOutCompletedEventHandler(CoinCheckOutCompletedEvent);

                m_Cash.MachineError += new ICashMachine.MachineErrorEventHandler(MachineErrorEvent);
                m_Cash.ReceiveCashError += new ICashMachine.ReceiveCashErrorEventHandler(ReceiveCashErrorEvent);
                //Check CashSnapshot
                /*snapShot = new CashMachineSnapshot("1");
                if (snapShot == null || snapShot.MachineID == null || snapShot.MachineID == "")
                {
                    snapShot = new CashMachineSnapshot();
                    snapShot.MachineID = "1";
                    snapShot.NoteCashBox = "";
                    snapShot.Save();
                        snapShot = new CashMachineSnapshot("1");
                }*/
                
                
                return true;
            }
            catch (Exception ex)
            {
                status = "Plugin Error: " + ex.Message;
                return false;
            }
            
            
        }

        public void dispose()
        {
            if (m_Cash != null)
                m_Cash.Dispose();
        }

        public bool Connect(out string status)
        {
            status = "";
            string CoinComPort = AppSetting.GetSetting(AppSetting.SettingsName.CashRecycler.COMPortCashMachine);
            if (!m_Cash.Connect("",CoinComPort,ref status))
            {
                return false;
            }
            isConnecting = true;
            //isConnected = true;
            return true;
        }
        #endregion

        #region *) Dispense & Accept Cash Method
        public bool Deposit(decimal amount, string transactionNo, out string status)
        {
            status = "";

            if (!isConnected)
            {
                if (isConnecting)
                {
                    status = "Machine is still connecting. Please wait!";
                    return false;
                }
                else
                {
                    status = "Machine is not connected";
                    return false;
                }
            }
            if (isDispensing)
            {
                status = "Mode is dispensing Notes";
                return false;
            }

            string depositStatus = "";

            isAccepting = true;
            if (!m_Cash.AcceptCash(amount, ref depositStatus))
            {
                status = "Accept Error." + depositStatus;
                return false;
            }
            amountToPay = amount;
            orderRefno = transactionNo;
            return true;
        }

        public bool Dispense(decimal amount, out string status)
        {
            status = "";

            if (!isConnected)
            {
                status = "Machine is not connected";
                return false;
            }
            if (isAccepting)
            {
                status = "Mode is accepting Notes";
                return false;
            }

            string dispenseStatus = "";

            isDispensing = true;
            if (!m_Cash.Dispense(amount, ref dispenseStatus))
            {
                status = "Dispense Error." + dispenseStatus;
                return false;
            }
            //amountToPay = amount;
            return true;
        }

        public bool ReceiveFloat(decimal amount, out string status)
        {
            status = "";

            if (!isConnected)
            {
                status = "Machine is not connected";
                return false;
            }
            if (isDispensing)
            {
                status = "Mode is dispensing Notes";
                return false;
            }

            string depositStatus = "";

            isReceivingFloat = true;
            if (!m_Cash.ReceiveFloatFromEntrance(amount, ref depositStatus))
            {
                status = "Receive Float Error." + depositStatus;
                return false;
            }
            amountToPay = amount;
            return true;
        }

        #endregion

        #region Cancel Transaction
        public bool CancelAndKeepCash(out string status)
        {
            status = "";
            string depositStatus = "";
            if (!m_Cash.CancelAcceptKeepCash(ref depositStatus,""))
            {
                status = "Cancel Accept Return Cash Error." + depositStatus;
                return false;
            }
            isAccepting = false;
            return true;
        }

        public bool CancelAndKeepCash(out string status, string RefNo)
        {
            status = "";
            string depositStatus = "";
            if (!m_Cash.CancelAcceptKeepCash(ref depositStatus, RefNo))
            {
                status = "Cancel Accept Return Cash Error." + depositStatus;
                return false;
            }
            isAccepting = false;
            return true;
        }

        public bool CancelAndReturnCash(out string status)
        {
            status = "";
            string depositStatus = "";
            try
            {
                if (!m_Cash.CancelAcceptReturnCash(ref depositStatus,0,0, ""))
                {
                    status = "Cancel Accept Return Cash Error." + depositStatus;
                    return false;
                }
                isAccepting = false;
            }
            catch (Exception ex)
            {
                status = "Cancel Accept and Return Cash Error" + ex.Message;
                return false;
            }
            return true;
        }

        public bool CancelAndReturnCash(out string status, decimal coinsAmount, decimal notesAmount, string refNo)
        {
            status = "";
            string depositStatus = "";
            try
            {
                if (!m_Cash.CancelAcceptReturnCash(ref depositStatus, coinsAmount, notesAmount, refNo))
                {
                    status = "Cancel Accept Return Cash Error." + depositStatus;
                    return false;
                }
                isAccepting = false;
            }
            catch (Exception ex)
            {
                status = "Cancel Accept and Return Cash Error" + ex.Message;
                return false;
            }
            return true;
        }

        public bool CancelFloatReceiving(out string status)
        {
            status = "";
            string depositStatus = "";
            try
            {
                if (!m_Cash.CancelNoteReceiveFloatKeepCash(ref depositStatus))
                {
                    status = "Cancel Receive Note." + depositStatus;
                    return false;
                }
                isAccepting = false;
                isReceivingFloat = false;
            }
            catch (Exception ex)
            {
                status = "Cancel Receive Note Error" + ex.Message;
                return false;
            }
            return true;
        }
        #endregion

        #region ReturnCoin
        public bool ReturnCoin(out string status)
        {
            status = "";

            if (!isConnected)
            {
                status = "Machine is not connected";
                return false;
            }
            if (!m_Cash.ReturnCoin(ref status))
            {
                return false;
            }
            return true;

        }
       

        public bool isSupportReturnCoin()
        {
            return m_Cash.isSupportReturnCoin();
        }
        #endregion

        #region Events
        #region dispense
        private void cashReturnedEvent(int R1, int R2, int R3, int R4, decimal amt)
        {
            //Log the state in the log
            

            //raise event to the  application  
            CashReturnedArgs returnArgs = new CashReturnedArgs(R1, R2, R3, R4, amt);
            OnCashReturned(returnArgs);

        }

        protected virtual void OnCashReturned(CashReturnedArgs e)
        {
            if (CashReturned != null)
            {
                CashReturned(this, e);//Raise the event
            }
        }

        private void cashOutCollectedEvent()
        {
            if (isDispensing)
            {
                isDispensing = false;
            }

            if (isAccepting)
            {
                isAccepting = false;
            }
            CashCollected(this);
            return;
        }
        #endregion

        #region Accepting cash
        protected virtual void OnCashReceived(CashReceivedArgs e)
        {
            if (CashReceived != null)
            {
                CashReceived(this, e);//Raise the event
            }
        }

        private void cashReceivedEvent(decimal denomination, decimal amount)
        {
            OnCashLogged(new CashLogArgs("Received : " + denomination.ToString() + ", Total Amount : " + amount.ToString("N2")));
            if (isAccepting) 
            {
            //update control
                if (amount >= amountToPay)
                {
                    //accepting completed, update controls
                    isAccepting = false;

                    //Update Order Ref No 
                    string beforeSnapshot, afterSnapshot;
                    string tmpTransactionID = getLastOrderTransactionID(out beforeSnapshot, out afterSnapshot);
                    if (!String.IsNullOrEmpty(tmpTransactionID))
                    {
                        CashManagementLib.CashMachineTransaction c = new CashManagementLib.CashMachineTransaction(tmpTransactionID);
                        if (c != null && c.TransactionID == tmpTransactionID)
                        {
                            c.OrderRefNo = orderRefno;
                            c.Save(UserInfo.username);
                        }
                    }
                }

                
            }

            if (isReceivingFloat)
            {
                //update control
                if (amount >= amountToPay)
                {
                    //accepting completed, update controls
                    isReceivingFloat = false;
                    
                }
            }  
            //this.m_CashStatus = 

            OnCashReceived(new CashReceivedArgs(denomination, amount));
        }

        protected virtual void OnCashInEscrow()
        {
            if (CashInEscrow != null)
            {
                CashInEscrow(this);//Raise the event
            }
        }
        private void CashInEscrowEvent(int Count2, int Count5, int Count10, int Count50, bool IsUnknown,decimal totalAmount)
        {
            OnCashInEscrow();
        }
        

        protected virtual void OnCashOutNote()
        {
            if (NoteCashOut != null)
            {
                NoteCashOut(this);//Raise the event
            }
        }
        private void cashOutNoteEvent()
        {
            OnCashOutNote();
        }

        protected virtual void OnReceiveFloatCompleted(ReceiveFloatCompletedArgs e)
        {
            if (ReceiveFloatCompleted != null)
            {
                ReceiveFloatCompleted(this, e);//Raise the event
            }
        }

        private void ReceiveFloatFromEntranceCompleted(decimal totalAmount, int R1, int R2, int R3, int R4)
        {
            //get last transactionID
            string beforeSnapshot, afterSnapshot;
            string lastTransactionID = getLastFloatTransactionID(out beforeSnapshot, out afterSnapshot);

            OnReceiveFloatCompleted(new ReceiveFloatCompletedArgs(lastTransactionID, beforeSnapshot, afterSnapshot));
        }

        private string getLastFloatTransactionID(out string beforeSnapshot, out string afterSnapshot)
        {
            string sqlString = "select top 1 TransactionID, BeforeSnapshot, AfterSnapshot from cashMachineTransaction where TransactionType = 'FLOAT' order by TransactionDate desc";
            DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];

            beforeSnapshot = "";
            afterSnapshot = "";

            if (dt.Rows.Count <= 0)
                return "";

            beforeSnapshot = dt.Rows[0]["BeforeSnapshot"].ToString();
            afterSnapshot = dt.Rows[0]["AfterSnapshot"].ToString();
            return dt.Rows[0]["TransactionID"].ToString();
        }

        private string getLastCheckOutTransactionID(out string beforeSnapshot, out string afterSnapshot)
        {
            string sqlString = "select top 1 TransactionID, BeforeSnapshot, AfterSnapshot from cashMachineTransaction where TransactionType = 'CHECKOUT' order by TransactionDate desc";
            DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];

            beforeSnapshot = "";
            afterSnapshot = "";

            if (dt.Rows.Count <= 0)
                return "";

            beforeSnapshot = dt.Rows[0]["BeforeSnapshot"].ToString();
            afterSnapshot = dt.Rows[0]["AfterSnapshot"].ToString();
            return dt.Rows[0]["TransactionID"].ToString();
        }

        private string getLastOrderTransactionID(out string beforeSnapshot, out string afterSnapshot)
        {
            string sqlString = "select top 1 TransactionID, BeforeSnapshot, AfterSnapshot from cashMachineTransaction where TransactionType = 'SALES' order by TransactionDate desc";
            DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];

            beforeSnapshot = "";
            afterSnapshot = "";

            if (dt.Rows.Count <= 0)
                return "";

            beforeSnapshot = dt.Rows[0]["BeforeSnapshot"].ToString();
            afterSnapshot = dt.Rows[0]["AfterSnapshot"].ToString();
            return dt.Rows[0]["TransactionID"].ToString();
        }

        #endregion

        #region Log and Error
        protected virtual void OnCashLogged(CashLogArgs e)
        {
            if (CashLogged != null)
            {
                CashLogged(this, e);//Raise the event
            }
        }

        private void cashLogEvent(string log)
        {
            //Save the log in current log 
            currentLog += log;
            currentLog += "\n";
            Logger.writeLog("Cash Machine." + log);
            OnCashLogged(new CashLogArgs(log));
        }
        
        
        protected virtual void OnAcceptCashCompleted(AcceptCashCompletedArgs e)
        {
            if (AcceptCashCompleted != null)
            {
                AcceptCashCompleted(this, e);//Raise the event
            }
        }
        private void AcceptCashCompletedEvent(Boolean success, string errorMsg)
        {
            Logger.writeLog("Accept Cash Completed");

            OnAcceptCashCompleted(new AcceptCashCompletedArgs (success, errorMsg));
        }




        protected virtual void OnCashError(CashErrorArgs e)
        {
            if (CashError != null)
            {
                CashError(this, e);//Raise the event
            }
        }

        private void cashErrorEvent(string errorLog)
        {
            OnCashLogged(new CashLogArgs("Error" + errorLog));
            OnCashError(new CashErrorArgs(errorLog));
        }
        #endregion

        #region Status
        protected virtual void OnCashConnected()
        {
            string stat = "";
            //m_Cash.RefreshStatus(ref stat);
            if (CashMachineConnected != null)
            {
                CashMachineConnected(this);//Raise the event
            }
            
        }

        protected virtual void OnCashDisconnected()
        {
            if (CashMachineDisconnected != null)
            {
                CashMachineDisconnected(this);//Raise the event
            }
        }

        private void connectedEvent()
        {
            //Save the log in current log 
            currentState = "Connected";
            isConnected = true;
            isConnecting = false;
            OnCashConnected();
        }

        private void disconnectedEvent(string disconnectedStatus)
        {
            //Save the log in current log 
            currentState = disconnectedStatus;
            isConnected = false;
            isConnecting = false;
            OnCashDisconnected();
        }

        

        private CashStatusInfo currentSnapshot;

        private void UpdateCurrentSnapShot()
        {

            /*if (snapShot == null)
                return;

            snapShot.NoteCashBox = Newtonsoft.Json.JsonConvert.SerializeObject(currentSnapshot);
            snapShot.Save();*/
        }

        private void OnCashStatusChanged(CashStatusChangedArgs e)
        {
            if (CashStatusChanged != null)
            {
                CashStatusChanged(this, e);//Raise the event
            }

        }

        private void CashStatusChangedEvent(CashStatusInfo status)
        {
            //Update Current Snapshot
            /*currentSnapshot = status;
            UpdateCurrentSnapShot();
            */

            m_CashStatus = status;
            OnCashStatusChanged(new CashStatusChangedArgs(status));
        }

        private void openingBalanceEvent(CashStatusInfo e)
        {
            //update mCashStatus
            m_CashStatus = e;
        }

        private void OnOpeningBalanceOverwritten(OpeningBalanceOverwrittenArgs e)
        {
            if (OpeningBalanceOverwritten != null)
            {
                OpeningBalanceOverwritten(this, e);//Raise the event
            }

        }

        private void openingBalanceOverwrittenEvent(CashStatusInfo e)
        {
            //update mCashStatus
            m_CashStatus = e;
            OnOpeningBalanceOverwritten(new OpeningBalanceOverwrittenArgs(e));
        }

        #endregion
        #region Check Out
        public bool NoteCheckOut(decimal amount, out string status)
        {
            status = "";

            if (!isConnected)
            {
                status = "Machine is not connected";
                return false;
            }
            
            string depositStatus = "";
            //isReceivingFloat = true;
            if (!m_Cash.NoteCheckOut(ref depositStatus, amount))
            {
                status = "Check Out Error." + depositStatus;
                return false;
            }
            //amountToPay = amount;
            return true;
        }

        public bool NoteCheckOutExcludeFloat(int R1, int R2, int R3, int R4, out string status)
        {
            status = "";

            if (!isConnected)
            {
                status = "Machine is not connected";
                return false;
            }

            string depositStatus = "";
            //isReceivingFloat = true;
            if (!m_Cash.NoteCheckOutExcludeFloat(ref depositStatus, R1,R2, R3, R4))
            {
                status = "Check Out Error." + depositStatus;
                return false;
            }
            //amountToPay = amount;
            return true;
        }

        public bool calculateR1toR4Deduction(int D2, int D5, int D10, int D50, out int R1, out int R2, out int R3, out int R4)
        {
            R1 =0; R2=0; R3= 0; R4 = 0;
            
                for (int i = 0; i < m_CashStatus.NoteRecyclers.Count; i++)
                {
                    int diff = 0;
                    if (m_CashStatus.NoteRecyclers[i].Denomination == 2)
                    {
                        if (m_CashStatus.NoteRecyclers[i].Pieces >= D2)
                            diff = D2;
                        else
                        {
                            diff = m_CashStatus.NoteRecyclers[i].Pieces;
                            D2 -= m_CashStatus.NoteRecyclers[i].Pieces;
                        }
                    }
                    if (m_CashStatus.NoteRecyclers[i].Denomination == 5)
                    {
                        if (m_CashStatus.NoteRecyclers[i].Pieces >= D5)
                            diff = D5;
                        else
                        {
                            diff = m_CashStatus.NoteRecyclers[i].Pieces;
                            D10 -= m_CashStatus.NoteRecyclers[i].Pieces;
                        }
                    }
                    if (m_CashStatus.NoteRecyclers[i].Denomination == 10)
                    {
                        if (m_CashStatus.NoteRecyclers[i].Pieces >= D10)
                            diff = D10;
                        else
                        {
                            diff = m_CashStatus.NoteRecyclers[i].Pieces;
                            D10 -= m_CashStatus.NoteRecyclers[i].Pieces;
                        }
                    }
                    if (m_CashStatus.NoteRecyclers[i].Denomination == 50)
                    {
                        if (m_CashStatus.NoteRecyclers[i].Pieces >= D50)
                            diff = D50;
                        else
                        {
                            diff = m_CashStatus.NoteRecyclers[i].Pieces;
                            D50 -= m_CashStatus.NoteRecyclers[i].Pieces;
                        }
                    }

                    if (i == 0)
                    {
                        R1 = diff;
                    }
                    if (i == 1)
                    {
                        R2 = diff;
                    }
                    if (i == 2)
                    {
                        R3 = diff;
                    }
                    if (i == 3)
                    {
                        R4 = diff;
                    }
                }
            
            
            
            return true;
        }

        public bool CoinCheckOut(decimal checkOutAmount, int floatR1, int floatR2, int floatR3, int floatR4, out string status)
        {
            status = "";

            if (!isConnected)
            {
                status = "Machine is not connected";
                return false;
            }

            string depositStatus = "";
            //isReceivingFloat = true;
            
            
            if (!m_Cash.CoinCheckOutExcludeFloat(ref depositStatus, floatR1, floatR2, floatR3, floatR4))
            {
                status = "Coin Check Out Error." + depositStatus;
                return false;
            }
            //amountToPay = amount;
            return true;
        }

        private void NoteCheckOutCompletedEvent(int r1, int r2, int r3, int r4)
        {
            //get last transactionID
            string beforeSnapshot, afterSnapshot;
            string lastTransactionID = getLastCheckOutTransactionID(out beforeSnapshot, out afterSnapshot);

            OnNoteCheckOutCompleted(new NoteCheckOutCompletedArgs(lastTransactionID, beforeSnapshot, afterSnapshot));
        }

        private void CoinCheckOutCompletedEvent(int r1, int r2, int r3, int r4)
        {
            //get last transactionID
            string beforeSnapshot, afterSnapshot;
            string lastTransactionID = getLastCheckOutTransactionID(out beforeSnapshot, out afterSnapshot);

            OnNoteCheckOutCompleted(new NoteCheckOutCompletedArgs(lastTransactionID, beforeSnapshot, afterSnapshot));
        }

        protected virtual void OnNoteCheckOutCompleted(NoteCheckOutCompletedArgs e)
        {
            if (NoteCheckoutCompleted != null)
            {
                NoteCheckoutCompleted(this, e);//Raise the event
            }
        }

        #endregion

        #endregion

        #region Lock and Unlock Notes Coins
        public bool UnlockNotes(out string status)
        {
            status = "";

            if (!isConnected)
            {
                status = "Machine is not connected";
                return false;
            }

            if (!m_Cash.NoteCashBoxUnlock(ref status))
            {
               return false;
            }
            
            return true;
        }

        public bool UnlockCoins(out string status)
        {
            status = "";

            if (!isConnected)
            {
                status = "Machine is not connected";
                return false;
            }

            if (!m_Cash.CoinCashBoxUnlock(ref status))
            {
                return false;
            }
            return true;
        }
        #endregion

        public bool Reset(out string status)
        {
            status = "";

            if (!isConnected)
            {
                status = "Machine is not connected";
                return false;
            }

            if (!m_Cash.Reset(ref status))
            {
                return false;
            }
            return true;
        }

        private void MachineErrorEvent(string ErrorCode, string Message, string URL)
        {
            //get last transactionID
            isMachineError = true;
            onMachineError(new MachineErrorArgs(ErrorCode, URL));
        }

        protected virtual void onMachineError(MachineErrorArgs e)
        {
            if (MachineError != null)
            {
                MachineError(this, e);//Raise the event
            }
        }

        private void ReceiveCashErrorEvent(string ErrorCode, string Message, string URL)
        {
            //get last transactionID
            isMachineError = true;
            onReceiveCashError(new ReceiveCashErrorArgs(ErrorCode, Message, URL));
        }

        protected virtual void onReceiveCashError(ReceiveCashErrorArgs e)
        {
            if (ReceiveCashError != null)
            {
                ReceiveCashError(this, e);//Raise the event
            }
        }

        #region Current Status To Datatable
        public DataTable getNoteRecyclerCurrentStatus()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Denomination", typeof(string));
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("AmountF", typeof(decimal));
            dt.Columns.Add("Level", typeof(string));
            dt.Columns.Add("DenominationDec", typeof(decimal));
            foreach (BoxStatusInfo b in m_CashStatus.NoteRecyclers)
            {
                DataRow dr = dt.NewRow();
                dr["Denomination"] = b.Denomination.ToString("N0");
                dr["Count"] = b.Pieces;
                dr["AmountF"] = b.Denomination * b.Pieces;
                dr["Level"] = b.Level.ToString();
                dr["DenominationDec"] = b.Denomination;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable getCoinRecyclerCurrentStatus()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Denomination", typeof(string));
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("AmountF", typeof(decimal));
            dt.Columns.Add("Level", typeof(string));
            foreach (BoxStatusInfo b in m_CashStatus.CoinRecyclers)
            {
                DataRow dr = dt.NewRow();
                dr["Denomination"] = b.Denomination.ToString("N2");
                dr["Count"] = b.Pieces;
                dr["AmountF"] = b.Denomination * b.Pieces;
                dr["Level"] = b.Level.ToString();
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable getNoteCashBoxCurrentStatus()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Denomination", typeof(string));
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("AmountF", typeof(decimal));
            dt.Columns.Add("Level", typeof(string));
            dt.Columns.Add("DenominationDec", typeof(decimal));
            foreach (BoxStatusInfo b in m_CashStatus.NoteCashBox)
            {
                DataRow dr = dt.NewRow();
                dr["Denomination"] = b.Denomination.ToString("N0");
                dr["Count"] = b.Pieces;
                dr["AmountF"] = b.Denomination * b.Pieces;
                dr["Level"] = b.Level.ToString();
                dr["DenominationDec"] = b.Denomination;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable getCoinCashBoxCurrentStatus()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Denomination", typeof(string));
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("AmountF", typeof(decimal));
            dt.Columns.Add("Level", typeof(string));
            foreach (BoxStatusInfo b in m_CashStatus.CoinCashBox)
            {
                DataRow dr = dt.NewRow();
                dr["Denomination"] = b.Denomination.ToString("N2");
                dr["Count"] = b.Pieces;
                dr["AmountF"] = b.Denomination * b.Pieces;
                dr["Level"] = b.Level.ToString();
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public decimal getNoteRecyclerTotalAmount()
        {
            return m_CashStatus.NoteRecyclersTotalAmount;
        }

        public decimal getCoinRecyclerTotalAmount()
        {
            return m_CashStatus.CoinRecyclersTotalAmount;
        }

        public decimal getNoteCashBoxTotalAmount()
        {
            return m_CashStatus.NoteCashBoxTotalAmount;
        }

        public decimal getCoinCashBoxTotalAmount()
        {
            return m_CashStatus.CoinCashBoxTotalAmount;
        }

        

        public bool getTransactionDetails(string transactionID, out DataTable NoteRecyclerDiff, out DataTable CoinRecyclerDiff, out DataTable NoteCashBoxDiff, out DataTable CoinCashBoxDiff
            , out DateTime transDate, out string transType, out string transStatus, out decimal amt, out decimal amtReceived, out decimal amtReturned, out decimal balance)
        {

            //Define new DataTable
            NoteRecyclerDiff = new DataTable();
            CoinRecyclerDiff = new DataTable();
            NoteCashBoxDiff = new DataTable();
            CoinCashBoxDiff = new DataTable();
            transDate = DateTime.Now;
            transType = "";
            transStatus = "";
            amt = 0;
            amtReceived = 0;
            amtReturned = 0;
            balance = 0;

            CashManagementLib.CashMachineTransaction c = new CashManagementLib.CashMachineTransaction(transactionID);
            //string sqlString = "Select * from CashMachineTransaction where TransactionID = '" + transactionID + "'";
            //DataTable dtTransaction = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];

            //Check if got transaction data
            if (c == null || c.TransactionID == null || c.TransactionID == "")
                return false;
            //Check Before Snapshot
            if (c.BeforeSnapshot == null)
                return false;
            string beforeSnapshot = c.BeforeSnapshot;

            if (c.AfterSnapshot == null)
                return false;
            string afterSnapshot = c.AfterSnapshot;

            transDate = c.TransactionDate.GetValueOrDefault(DateTime.Now);
            transType = c.TransactionType;
            transStatus = c.Status;
            amt = c.Amount;
            amtReceived = c.AmountReceived.GetValueOrDefault(0);
            amtReturned = c.AmountReturned.GetValueOrDefault(0);
            balance = c.Balance.GetValueOrDefault(0);
           
            //Check Before Snapshot
            CashManagementLib.CashMachineSnapshotHistory snapshotBefore = new CashManagementLib.CashMachineSnapshotHistory(beforeSnapshot);
            CashManagementLib.CashMachineSnapshotHistory snapshotAfter = new CashManagementLib.CashMachineSnapshotHistory(afterSnapshot);

            if (snapshotBefore == null || snapshotBefore.SnapshotHistoryID == null)
                return false;

            if (snapshotAfter == null || snapshotAfter.SnapshotHistoryID == null)
                return false;

            try
            {
                NoteRecyclerDiff = ChangeDetailsDiffToDataTable(snapshotBefore.NoteRecyclerDenomination, snapshotBefore.NoteRecyclerQuantity, snapshotAfter.NoteRecyclerDenomination, snapshotAfter.NoteRecyclerQuantity, true) ;
                CoinRecyclerDiff = ChangeDetailsDiffToDataTable(snapshotBefore.CoinRecyclerDenomination, snapshotBefore.CoinRecyclerQuantity, snapshotAfter.CoinRecyclerDenomination, snapshotAfter.CoinRecyclerQuantity, false);
                NoteCashBoxDiff = ChangeDetailsDiffToDataTable(snapshotBefore.NoteCashBoxDenomination, snapshotBefore.NoteCashBoxQuantity, snapshotAfter.NoteCashBoxDenomination, snapshotAfter.NoteCashBoxQuantity, true );
                CoinCashBoxDiff = ChangeDetailsDiffToDataTable(snapshotBefore.CoinCashBoxDenomination, snapshotBefore.CoinCashBoxQuantity, snapshotAfter.CoinCashBoxDenomination, snapshotAfter.CoinCashBoxQuantity, false);

            }
            catch (Exception ex)
            {
                Logger.writeLog("Error getting Details Data for the details." + ex.Message);
            }

            return true;

        }

        private DataTable ChangeDetailsDiffToDataTable(string BefDenominationString, string befCountString, string aftDenominationString, string afCountString, bool isNotes)
        {
            string[] befDenominations = BefDenominationString.Split(';');
            string[] befCounts = befCountString.Split(';');

            string[] aftDenominations = aftDenominationString.Split(';');
            string[] aftCounts = afCountString.Split(';');

            if (befDenominations.GetLength(0) != aftDenominations.GetLength(0))
                return new DataTable();

            //string[] states = stateString.Split(';');
            DataTable dt = new DataTable();
            dt.Columns.Add("Denomination", typeof(string));
            dt.Columns.Add("Before", typeof(int));
            dt.Columns.Add("After", typeof(int));
            dt.Columns.Add("Diff", typeof(int));
            dt.Columns.Add("DenominationDec", typeof(decimal));
            for (int i = 0; i < befDenominations.GetLength(0) - 1; i++)
            {
                decimal tmpDenom = 0;
                int tmpBefCount = 0;
                int tmpAftCount = 0;
                if (!decimal.TryParse(befDenominations[i], out tmpDenom))
                    return new DataTable();
                if (!int.TryParse(befCounts[i], out tmpBefCount))
                    return new DataTable();
                if (!int.TryParse(aftCounts[i], out tmpAftCount))
                    return new DataTable();

                DataRow dr = dt.NewRow();
                if (isNotes)
                    dr["Denomination"] = tmpDenom.ToString("N0");
                else
                    dr["Denomination"] = tmpDenom.ToString("N2");
                dr["Before"] = tmpBefCount;
                dr["After"] = tmpAftCount;
                dr["Diff"] = tmpAftCount - tmpBefCount;
                dr["DenominationDec"] = tmpDenom;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private DataTable ChangeToDataTable(string DenominationString, string countString, string levelString)
        {
            string[] denominations = DenominationString.Split(';');
            string[] levels = levelString.Split(';');
            string[] counts = countString.Split(';');
            //string[] states = stateString.Split(';');
            DataTable dt = new DataTable();
            dt.Columns.Add("Denomination", typeof(string));
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("AmountF", typeof(decimal));

            if (denominations.GetLength(0) == 0)
            {
                return dt;
            }

            for (int i = 0; i < denominations.GetLength(0) - 1; i++)
            {
                decimal tmpDenom = 0;
                int tmpCount = 0;
                if (!decimal.TryParse(denominations[i], out tmpDenom))
                    return dt;
                if (!int.TryParse(counts[i], out tmpCount))
                    return dt;

                DataRow dr = dt.NewRow();
                dr["Denomination"] = tmpDenom.ToString("N2");
                dr["Count"] = tmpCount;
                dr["AmountF"] = tmpDenom * tmpCount;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        #endregion

        #region Get Difference for cash snapshot history
        public bool getRecyclerDifference(string beforeSnapshot, string afterSnapshot, out DataTable DiffNotesTable, out DataTable DiffCoinTable)
        {
            
            
            DiffNotesTable = new DataTable();
            DiffCoinTable = new DataTable();

            if (String.IsNullOrEmpty(beforeSnapshot) || String.IsNullOrEmpty(afterSnapshot))
                return false;

            CashManagementLib.CashMachineSnapshotHistory bef = new CashManagementLib.CashMachineSnapshotHistory(beforeSnapshot);
            CashManagementLib.CashMachineSnapshotHistory aft = new CashManagementLib.CashMachineSnapshotHistory(afterSnapshot);

            if (bef == null || bef.SnapshotHistoryID == null)
                return false;

            if (aft == null || aft.SnapshotHistoryID == null)
                return false;

            DiffNotesTable = ChangeDetailsDiffToDataTable(bef.NoteRecyclerDenomination, bef.NoteRecyclerQuantity, aft.NoteRecyclerDenomination, aft.NoteRecyclerQuantity, true);
            DiffCoinTable = ChangeDetailsDiffToDataTable(bef.CoinRecyclerDenomination, bef.CoinRecyclerQuantity, aft.CoinRecyclerDenomination, aft.CoinRecyclerQuantity, false);

            return true;
        }

        public bool SaveToNotesCheckOut(string beforeSnapshot, string afterSnapshot, DateTime startTime, DateTime endTime, decimal amount
            , decimal startBalanceAmount, decimal floatAmount, decimal salesAmount, out string noteCheckOutID)
        {
            DataTable DiffNotesTable = new DataTable();
            noteCheckOutID = "";

            if (String.IsNullOrEmpty(beforeSnapshot) || String.IsNullOrEmpty(afterSnapshot))
                return false;

            try 
            {
            CashManagementLib.CashMachineSnapshotHistory bef = new CashManagementLib.CashMachineSnapshotHistory(beforeSnapshot);
            CashManagementLib.CashMachineSnapshotHistory aft = new CashManagementLib.CashMachineSnapshotHistory(afterSnapshot);

            if (bef == null || bef.SnapshotHistoryID == null)
                return false;

            if (aft == null || aft.SnapshotHistoryID == null)
                return false;

            DiffNotesTable = ChangeDetailsDiffToDataTable(bef.NoteRecyclerDenomination, bef.NoteRecyclerQuantity, aft.NoteRecyclerDenomination, aft.NoteRecyclerQuantity, true);
            
            NotesCheckOut n = new NotesCheckOut();
            n.NotesCheckOutID = getNewNotesCheckOutID();
            n.PointofsaleID = PointOfSaleInfo.PointOfSaleID;
            n.MachineType = MACHINETYPE_NOTES;
            n.StartTime = startTime;
            n.EndTime = endTime;
            n.Username = UserInfo.username;
            n.Amount = bef.NoteRecyclerTotalAmount.GetValueOrDefault(0) + bef.NoteCashBoxTotalAmount.GetValueOrDefault(0) - aft.NoteRecyclerTotalAmount.GetValueOrDefault(0);
            n.StartBalanceAmount = startBalanceAmount;
            n.FloatIn = floatAmount;
            n.SalesNotes = salesAmount;
            //n.NewBalanceR1Denomination = ;
        
                
            for (int i = 0; i < DiffNotesTable.Rows.Count; i++)
            {
                decimal denom = 0;
                int ct = 0;
                if (!decimal.TryParse(DiffNotesTable.Rows[i]["Denominationdec"].ToString(), out denom))
                {
                    continue;
                }
                if (!int.TryParse(DiffNotesTable.Rows[i]["After"].ToString(), out ct))
                {
                    continue;
                }
                if (i == 0)
                {
                    n.NewBalanceR1Denomination = denom.ToString();
                    n.NewBalanceR1Count = ct;
                    n.NewBalanceR1TotalAmount = denom * ct;
                }
                if (i == 1)
                {
                    n.NewBalanceR2Denomination = denom.ToString();
                    n.NewBalanceR2Count = ct;
                    n.NewBalanceR2TotalAmount = denom * ct;
                }
                if (i == 2)
                {
                    n.NewBalanceR3Denomination = denom.ToString();
                    n.NewBalanceR3Count = ct;
                    n.NewBalanceR3TotalAmount = denom * ct;
                }
                if (i == 3)
                {
                    n.NewBalanceR4Denomination = denom.ToString();
                    n.NewBalanceR4Count = ct;
                    n.NewBalanceR4TotalAmount = denom * ct;
                }
            }
            n.Save();
            noteCheckOutID = n.NotesCheckOutID;

            return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Failed Save Check Out" + ex.Message);
                return false;
            }
            
        }

        public bool SaveToNotesCheckOut(string beforeSnapshot, string afterSnapshot, DateTime startTime, DateTime endTime, decimal amount
            , decimal startBalanceAmount, decimal floatAmount, decimal salesAmount, decimal adjustmentAmount, out string noteCheckOutID)
        {
            DataTable DiffNotesTable = new DataTable();
            noteCheckOutID = "";

            if (String.IsNullOrEmpty(beforeSnapshot) || String.IsNullOrEmpty(afterSnapshot))
                return false;

            try 
            {
            CashManagementLib.CashMachineSnapshotHistory bef = new CashManagementLib.CashMachineSnapshotHistory(beforeSnapshot);
            CashManagementLib.CashMachineSnapshotHistory aft = new CashManagementLib.CashMachineSnapshotHistory(afterSnapshot);

            if (bef == null || bef.SnapshotHistoryID == null)
                return false;

            if (aft == null || aft.SnapshotHistoryID == null)
                return false;

            DiffNotesTable = ChangeDetailsDiffToDataTable(bef.NoteRecyclerDenomination, bef.NoteRecyclerQuantity, aft.NoteRecyclerDenomination, aft.NoteRecyclerQuantity, true);
            
            NotesCheckOut n = new NotesCheckOut();
            n.NotesCheckOutID = getNewNotesCheckOutID();
            n.PointofsaleID = PointOfSaleInfo.PointOfSaleID;
            n.MachineType = MACHINETYPE_NOTES;
            n.StartTime = startTime;
            n.EndTime = endTime;
            n.Username = UserInfo.username;
            n.Amount = bef.NoteRecyclerTotalAmount.GetValueOrDefault(0) + bef.NoteCashBoxTotalAmount.GetValueOrDefault(0) - aft.NoteRecyclerTotalAmount.GetValueOrDefault(0);
            n.StartBalanceAmount = startBalanceAmount;
            n.FloatIn = floatAmount;
            n.SalesNotes = salesAmount;
            n.TotalAdjustmentAmount = adjustmentAmount;
            //n.NewBalanceR1Denomination = ;
        
                
            for (int i = 0; i < DiffNotesTable.Rows.Count; i++)
            {
                decimal denom = 0;
                int ct = 0;
                if (!decimal.TryParse(DiffNotesTable.Rows[i]["Denominationdec"].ToString(), out denom))
                {
                    continue;
                }
                if (!int.TryParse(DiffNotesTable.Rows[i]["After"].ToString(), out ct))
                {
                    continue;
                }
                if (i == 0)
                {
                    n.NewBalanceR1Denomination = denom.ToString();
                    n.NewBalanceR1Count = ct;
                    n.NewBalanceR1TotalAmount = denom * ct;
                }
                if (i == 1)
                {
                    n.NewBalanceR2Denomination = denom.ToString();
                    n.NewBalanceR2Count = ct;
                    n.NewBalanceR2TotalAmount = denom * ct;
                }
                if (i == 2)
                {
                    n.NewBalanceR3Denomination = denom.ToString();
                    n.NewBalanceR3Count = ct;
                    n.NewBalanceR3TotalAmount = denom * ct;
                }
                if (i == 3)
                {
                    n.NewBalanceR4Denomination = denom.ToString();
                    n.NewBalanceR4Count = ct;
                    n.NewBalanceR4TotalAmount = denom * ct;
                }
            }
            n.Save();
            noteCheckOutID = n.NotesCheckOutID;

            return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Failed Save Check Out" + ex.Message);
                return false;
            }
            
        }

        public bool SaveToCoinsCheckOut(string beforeSnapshot, string afterSnapshot, DateTime startTime, DateTime endTime, decimal amount
            , decimal startBalanceAmount, decimal floatAmount, decimal salesAmount, decimal adjustmentAmount, out string coinCheckOutID)
        {
            DataTable DiffNotesTable = new DataTable();
            coinCheckOutID = "";

            if (String.IsNullOrEmpty(beforeSnapshot) || String.IsNullOrEmpty(afterSnapshot))
                return false;

            try
            {
                CashManagementLib.CashMachineSnapshotHistory bef = new CashManagementLib.CashMachineSnapshotHistory(beforeSnapshot);
                CashManagementLib.CashMachineSnapshotHistory aft = new CashManagementLib.CashMachineSnapshotHistory(afterSnapshot);

                if (bef == null || bef.SnapshotHistoryID == null)
                    return false;

                if (aft == null || aft.SnapshotHistoryID == null)
                    return false;

                DiffNotesTable = ChangeDetailsDiffToDataTable(bef.CoinRecyclerDenomination, bef.CoinRecyclerQuantity, aft.CoinRecyclerDenomination, aft.CoinRecyclerQuantity, true);

                NotesCheckOut n = new NotesCheckOut();
                n.NotesCheckOutID = getNewNotesCheckOutID();
                n.PointofsaleID = PointOfSaleInfo.PointOfSaleID;
                n.Username = UserInfo.username;
                n.StartTime = startTime;
                n.EndTime = endTime;
                n.MachineType = MACHINETYPE_COINS;
                n.Amount = bef.CoinRecyclerTotalAmount.GetValueOrDefault(0) - aft.CoinRecyclerTotalAmount.GetValueOrDefault(0);
                n.StartBalanceAmount = startBalanceAmount;
                //n.NewBalanceR1Denomination = ;
                n.FloatIn = floatAmount;
                n.SalesNotes = salesAmount;
                n.TotalAdjustmentAmount = adjustmentAmount;

                for (int i = 0; i < DiffNotesTable.Rows.Count; i++)
                {
                    decimal denom = 0;
                    int ct = 0;
                    if (!decimal.TryParse(DiffNotesTable.Rows[i]["Denominationdec"].ToString(), out denom))
                    {
                        continue;
                    }
                    if (!int.TryParse(DiffNotesTable.Rows[i]["After"].ToString(), out ct))
                    {
                        continue;
                    }
                    if (i == 0)
                    {
                        n.NewBalanceR1Denomination = denom.ToString();
                        n.NewBalanceR1Count = ct;
                        n.NewBalanceR1TotalAmount = denom * ct;
                    }
                    if (i == 1)
                    {
                        n.NewBalanceR2Denomination = denom.ToString();
                        n.NewBalanceR2Count = ct;
                        n.NewBalanceR2TotalAmount = denom * ct;
                    }
                    if (i == 2)
                    {
                        n.NewBalanceR3Denomination = denom.ToString();
                        n.NewBalanceR3Count = ct;
                        n.NewBalanceR3TotalAmount = denom * ct;
                    }
                    if (i == 3)
                    {
                        n.NewBalanceR4Denomination = denom.ToString();
                        n.NewBalanceR4Count = ct;
                        n.NewBalanceR4TotalAmount = denom * ct;
                    }
                }
                n.Save();

                coinCheckOutID = n.NotesCheckOutID;
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Failed Save Check Out" + ex.Message);
                return false;
            }

        }

        private DataTable ChangeDiffToDataTable(string BefDenominationString, string befCountString, string aftDenominationString, string afCountString)
        {
            string[] befDenominations = BefDenominationString.Split(';');
            string[] befCounts = befCountString.Split(';');

            string[] aftDenominations = aftDenominationString.Split(';');
            string[] aftCounts = afCountString.Split(';');

            if (befDenominations.GetLength(0) != aftDenominations.GetLength(0))
                return new DataTable();

            //string[] states = stateString.Split(';');
            DataTable dt = new DataTable();
            dt.Columns.Add("Denomination", typeof(string));
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("AmountF", typeof(decimal));

            for (int i = 0; i < befDenominations.GetLength(0) - 1; i++)
            {
                decimal tmpDenom = 0;
                int tmpBefCount = 0;
                int tmpAftCount = 0;
                if (!decimal.TryParse(befDenominations[i], out tmpDenom))
                    return new DataTable();
                if (!int.TryParse(befCounts[i], out tmpBefCount))
                    return new DataTable();
                if (!int.TryParse(aftCounts[i], out tmpAftCount))
                    return new DataTable();

                DataRow dr = dt.NewRow();
                dr["Denomination"] = tmpDenom.ToString("N2");
                dr["Count"] = tmpAftCount - tmpBefCount;
                dr["AmountF"] = tmpDenom * (tmpAftCount - tmpBefCount);
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion

        #region getter
        public string getNewNotesCheckOutID()
        {
            string result = "";
            try
            {
                int runningNo = 0;
                IDataReader ds = PowerPOS.SPs.GetNewNoteCheckOutIDByPointOfSaleID(PointOfSaleInfo.PointOfSaleID).GetReader();
                while (ds.Read())
                {
                    if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
                    {
                        runningNo = 0;
                    }
                }
                ds.Close();
                runningNo += 1;
                result = "NC" + DateTime.Now.ToString("yyMMdd") + PointOfSaleInfo.PointOfSaleID.ToString().PadLeft(4, '0') + runningNo.ToString().PadLeft(4, '0');
                return result;

            }
            catch (Exception ex)
            {
                Logger.writeLog("Failed to getnew Checkout ID." + ex.Message);
                return "";
            }
        }

        public int getNotesDenominationCount(decimal x)
        {

            return m_CashStatus.get_NoteRecyclerDenominationCount(x);
        }

        public int getCoinsDenominationCount(decimal x)
        {
            return m_CashStatus.get_CoinRecyclersDenominationCount(x);
        }
        #endregion

        #region save Check Out
        public bool SaveCheckOutData()
        {
            return true;
        }

        public DateTime getNotesCheckOutStartTime(string checkOutMode)
        {
            try
            {
                string sqlString = "select max(EndTime) from notesCheckOut where machineType = '" + checkOutMode + "'";
                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                DateTime res = new DateTime(1900,1,1);
                if (!DateTime.TryParse(tmp.ToString(), out res))
                {
                    return new DateTime(1900, 1, 1);
                }

                return res;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Get First Transaction. " + ex.Message);
                return new DateTime(1900, 1, 1);
            }
        }

        private DateTime getFirstTransactionForCheckOut()
        {
            try
            {
                string sqlString = "select min(OrderDate) from OrderHdr";
                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                DateTime res = new DateTime(1900, 1, 1);
                if (!DateTime.TryParse(tmp.ToString(), out res))
                {
                    return new DateTime(1900, 1, 1);
                }

                return res;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Get First Transaction. " + ex.Message);
                return new DateTime(1900, 1, 1);
            }
        }

        public decimal getTotalFloatIn(DateTime StartDate)
        {
            try 
            {
                string sqlString = "select sum(isnull(notesin,0)- isnull(notesout,0)) from cashmachinetransaction where transactiontype like 'FLOAT' and transactiondate > '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and status = 'Success'";
                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                decimal res = 0;
                if (!decimal.TryParse(tmp.ToString(), out res))
                {
                    return 0;
                }
                return res;
            }catch(Exception ex)
            {
                Logger.writeLog("Error Get Total Float In. " + ex.Message);
                return -1;
            }
        }

        public decimal getTotalCoinsFloatIn(DateTime StartDate)
        {
            try
            {
                string sqlString = "select sum(isnull(coinsin,0) - isnull(coinsout,0)) from cashmachinetransaction where transactiontype like 'FLOAT' and transactiondate > '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and status = 'Success'";
                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                decimal res = 0;
                if (!decimal.TryParse(tmp.ToString(), out res))
                {
                    return 0;
                }
                return res;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Get Total Float In Coins. " + ex.Message);
                return -1;
            }
        }

        public decimal getTotalSalesNotes(DateTime StartDate)
        {
            try
            {
                string sqlString = "select sum(isnull(notesin,0) - isnull(notesout,0)) from cashmachinetransaction where transactiontype like 'SALES' and transactiondate > '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and status = 'Success'";
                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                decimal res = 0;
                if (!decimal.TryParse(tmp.ToString(), out res))
                {
                    return 0;
                }
                return res;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Get Total Sales Notes. " + ex.Message);
                return -1;
            }
        }

        public static decimal getTotalSalesNotes(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string sqlString = "select sum(isnull(notesin,0) - isnull(notesout,0)) from cashmachinetransaction where transactiontype like 'SALES' and transactiondate > '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and status = 'Success'" +
                    " and transactiondate < '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" ;
                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                decimal res = 0;
                if (!decimal.TryParse(tmp.ToString(), out res))
                {
                    return 0;
                }
                return res;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Get Total Sales Notes. " + ex.Message);
                return -1;
            }
        }

        public decimal getTotalAdjustmentNotes(DateTime StartDate)
        {
            try
            {
                string sqlString = "select sum(isnull(notesin,0) - isnull(notesout,0)) from cashmachinetransaction where (transactiontype like 'ADJUSTMENT' or transactiontype like 'TAKEOUT') and transactiondate > '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and status = 'Success'";
                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                decimal res = 0;
                if (!decimal.TryParse(tmp.ToString(), out res))
                {
                    return 0;
                }
                return res;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Get Total Sales Notes. " + ex.Message);
                return -1;
            }
        }

        public decimal getTotalSalesCoins(DateTime StartDate)
        {
            try
            {
                string sqlString = "select sum(isnull(coinsIn,0) - isnull(coinsout,0)) from cashmachinetransaction where transactiontype like 'SALES' and transactiondate > '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and status = 'Success'";

                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                decimal res = 0;
                if (!decimal.TryParse(tmp.ToString(), out res))
                {
                    return 0;
                }
                return res;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Get Total Sales Coins. " + ex.Message);
                return -1;
            }
        }

        public decimal getTotalSalesCoins(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                string sqlString = "select sum(isnull(coinsIn,0) - isnull(coinsout,0)) from cashmachinetransaction where transactiontype like 'SALES' and transactiondate > '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and status = 'Success'" +
                    " and transactiondate < '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" ;

                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                decimal res = 0;
                if (!decimal.TryParse(tmp.ToString(), out res))
                {
                    return 0;
                }
                return res;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Get Total Sales Coins. " + ex.Message);
                return -1;
            }
        }

        public decimal getTotalAdjustmentCoins(DateTime StartDate)
        {
            try
            {
                string sqlString = "select sum(isnull(coinsin,0) - isnull(coinsout,0)) from cashmachinetransaction where (transactiontype like 'ADJUSTMENT' or transactiontype like 'TAKEOUT') and transactiondate > '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and status = 'Success'";
                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                decimal res = 0;
                if (!decimal.TryParse(tmp.ToString(), out res))
                {
                    return 0;
                }
                return res;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Get Total Adjustment Notes. " + ex.Message);
                return -1;
            }
        }

        public bool hasLowRecycler(out string status)
        {
            status = "";
            try
            {
                return m_Cash.HasLowRecycler(ref status);
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error on get low recycler" + ex.Message);
                return false;    
            }
        }

        public bool hasFullCashbox(out string status)
        {
            status = "";
            try
            {
                return m_Cash.HasHighCashBox(ref status);
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error on get low recycler" + ex.Message );
                return false;
            }
        }

        #endregion

        #region getDataForReport
        public bool getNotesCheckOutData(DateTime startTime, DateTime endTime, out DataTable result)
        {
            result = new DataTable();

            try
            {
                string sqlString = "Select Top 20 NotesCheckOutID, MachineType, Amount, StartTime, EndTime from NotesCheckOut Order By EndTime desc";
                result = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return false;
            }

            return true;
        }

        public bool getTransactionData(DateTime startTime, DateTime endTime, out DataTable result)
        {
            result = new DataTable();

            try
            {
                string sqlString = @"DECLARE @StartDate DATETIME;
                        DECLARE @EndDate DATETIME;
                        DECLARE @Status VARCHAR(50);

                        SET @StartDate =  '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                        SET @EndDate = '" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + @"'

                        Select TransactionID, TransactionType, TransactionDate, Amount, ISNULL(NotesIn,0) + ISNULL(CoinsIn,0) as AmountReceived, 
                        ISNULL(CoinsIn,0) + ISNULL(CoinsOut,0) As AmountReturned, ISNULL(Status,'Failed') Status 
                        from CashMachineTransaction Where (TransactionType = 'SALES' or TransactionType = 'FLOAT')  
                        and TransactionDate Between @StartDate and @EndDate
                        Order By TransactionDate desc";
                result = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return false;
            }

            return true;
        }

        public bool getDiscrepanciesData(DateTime startTime, DateTime endTime, bool unreturnedOnly, out DataTable result, out Decimal TotalDiscrepanciesAmount)
        {
            result = new DataTable();
            TotalDiscrepanciesAmount = 0;
            try
            {
                string sqlString = @"DECLARE @StartDate DATETIME;
                        DECLARE @EndDate DATETIME;
                        DECLARE @Status VARCHAR(50);

                        SET @StartDate =  '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                        SET @EndDate = '" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + @"'

                        Select TransactionID, TransactionType, TransactionDate, Amount, ISNULL(NotesIn,0) + ISNULL(CoinsIn,0) as AmountReceived, 
                        ISNULL(CoinsIn,0) + ISNULL(CoinsOut,0) As AmountReturned, CASE WHEN ISNULL(returnTransactionID,'') = '' Then 'Un-Returned' Else 'Returned' End Status 
                        from CashMachineTransaction Where TransactionType = 'ADJUSTMENT' 
                        and TransactionDate Between @StartDate and @EndDate ";
                if (unreturnedOnly)
                {
                    sqlString = sqlString + " and isnull(returnTransactionID,'') = '' ";
                }
                sqlString += " Order By TransactionDate desc";
                        //and status 
                        
                result = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    decimal tmp = 0;
                    if (decimal.TryParse(result.Rows[i]["Amount"].ToString(), out tmp))
                        TotalDiscrepanciesAmount += tmp;
                }

                    return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return false;
            }

            return true;
        }

        public bool ReturnTransaction(string transactionID, out string status)
        {
            status = "";
            try
            {
                //get transaction and amount
                string sqlString = "Select transactionid, isnull(notesin,0) as notesin, isnull(coinsin,0) as coinsin from CashMachineTransaction where TransactionID = '" + transactionID + "'";
                DataSet ds = DataService.GetDataSet(new QueryCommand(sqlString));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    status = "Cannot Find Transaction";
                    return false;
                }

                //if found
                //get notes amount and coins amount
                decimal notesamount = 0;
                decimal coinsamount = 0;
                if (!decimal.TryParse(ds.Tables[0].Rows[0]["notesin"].ToString(), out notesamount))
                    notesamount = 0;

                if (!decimal.TryParse(ds.Tables[0].Rows[0]["coinsin"].ToString(), out coinsamount))
                    coinsamount = 0;

                if (!m_Cash.CancelAcceptReturnCash(ref status, coinsamount, notesamount, transactionID))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                status = "Error Return Transaction : " + ex.Message;
                return false;
            }
        }

        #endregion

    }
}
