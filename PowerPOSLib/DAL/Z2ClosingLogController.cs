using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
namespace PowerPOS
{
    /// <summary>
    /// Controller class for Z2ClosingLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class Z2ClosingLogController
    {
        // Preload our schema..
        Z2ClosingLog thisSchemaLoad = new Z2ClosingLog();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public Z2ClosingLogCollection FetchAll()
        {
            Z2ClosingLogCollection coll = new Z2ClosingLogCollection();
            Query qry = new Query(Z2ClosingLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Z2ClosingLogCollection FetchByID(object Z2ClosingLogID)
        {
            Z2ClosingLogCollection coll = new Z2ClosingLogCollection().Where("Z2ClosingLogID", Z2ClosingLogID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public Z2ClosingLogCollection FetchByQuery(Query qry)
        {
            Z2ClosingLogCollection coll = new Z2ClosingLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Z2ClosingLogID)
        {
            return (Z2ClosingLog.Delete(Z2ClosingLogID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Z2ClosingLogID)
        {
            return (Z2ClosingLog.Destroy(Z2ClosingLogID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Z2ClosingLogID,decimal FloatBalance,DateTime StartTime,DateTime EndTime,string Cashier,string Supervisor,decimal CashIn,decimal CashOut,decimal OpeningBalance,decimal TotalSystemRecorded,decimal CashCollected,decimal? CashRecorded,decimal NetsCollected,decimal? NetsRecorded,string NetsTerminalID,decimal VisaCollected,decimal? VisaRecorded,string VisaBatchNo,decimal AmexCollected,decimal? AmexRecorded,string AmexBatchNo,decimal ChinaNetsCollected,decimal? ChinaNetsRecorded,string ChinaNetsTerminalID,decimal VoucherCollected,decimal? VoucherRecorded,string DepositBagNo,decimal TotalActualCollected,decimal ClosingCashOut,decimal Variance,int PointOfSaleID,DateTime CreatedOn,string CreatedBy,DateTime ModifiedOn,string ModifiedBy,Guid UniqueID)
	    {
		    Z2ClosingLog item = new Z2ClosingLog();
		    
            item.Z2ClosingLogID = Z2ClosingLogID;
            
            item.FloatBalance = FloatBalance;
            
            item.StartTime = StartTime;
            
            item.EndTime = EndTime;
            
            item.Cashier = Cashier;
            
            item.Supervisor = Supervisor;
            
            item.CashIn = CashIn;
            
            item.CashOut = CashOut;
            
            item.OpeningBalance = OpeningBalance;
            
            item.TotalSystemRecorded = TotalSystemRecorded;
            
            item.CashCollected = CashCollected;
            
            item.CashRecorded = CashRecorded;
            
            item.NetsCollected = NetsCollected;
            
            item.NetsRecorded = NetsRecorded;
            
            item.NetsTerminalID = NetsTerminalID;
            
            item.VisaCollected = VisaCollected;
            
            item.VisaRecorded = VisaRecorded;
            
            item.VisaBatchNo = VisaBatchNo;
            
            item.AmexCollected = AmexCollected;
            
            item.AmexRecorded = AmexRecorded;
            
            item.AmexBatchNo = AmexBatchNo;
            
            item.ChinaNetsCollected = ChinaNetsCollected;
            
            item.ChinaNetsRecorded = ChinaNetsRecorded;
            
            item.ChinaNetsTerminalID = ChinaNetsTerminalID;
            
            item.VoucherCollected = VoucherCollected;
            
            item.VoucherRecorded = VoucherRecorded;
            
            item.DepositBagNo = DepositBagNo;
            
            item.TotalActualCollected = TotalActualCollected;
            
            item.ClosingCashOut = ClosingCashOut;
            
            item.Variance = Variance;
            
            item.PointOfSaleID = PointOfSaleID;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.UniqueID = UniqueID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string Z2ClosingLogID,decimal FloatBalance,DateTime StartTime,DateTime EndTime,string Cashier,string Supervisor,decimal CashIn,decimal CashOut,decimal OpeningBalance,decimal TotalSystemRecorded,decimal CashCollected,decimal? CashRecorded,decimal NetsCollected,decimal? NetsRecorded,string NetsTerminalID,decimal VisaCollected,decimal? VisaRecorded,string VisaBatchNo,decimal AmexCollected,decimal? AmexRecorded,string AmexBatchNo,decimal ChinaNetsCollected,decimal? ChinaNetsRecorded,string ChinaNetsTerminalID,decimal VoucherCollected,decimal? VoucherRecorded,string DepositBagNo,decimal TotalActualCollected,decimal ClosingCashOut,decimal Variance,int PointOfSaleID,DateTime CreatedOn,string CreatedBy,DateTime ModifiedOn,string ModifiedBy,Guid UniqueID)
	    {
		    Z2ClosingLog item = new Z2ClosingLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Z2ClosingLogID = Z2ClosingLogID;
				
			item.FloatBalance = FloatBalance;
				
			item.StartTime = StartTime;
				
			item.EndTime = EndTime;
				
			item.Cashier = Cashier;
				
			item.Supervisor = Supervisor;
				
			item.CashIn = CashIn;
				
			item.CashOut = CashOut;
				
			item.OpeningBalance = OpeningBalance;
				
			item.TotalSystemRecorded = TotalSystemRecorded;
				
			item.CashCollected = CashCollected;
				
			item.CashRecorded = CashRecorded;
				
			item.NetsCollected = NetsCollected;
				
			item.NetsRecorded = NetsRecorded;
				
			item.NetsTerminalID = NetsTerminalID;
				
			item.VisaCollected = VisaCollected;
				
			item.VisaRecorded = VisaRecorded;
				
			item.VisaBatchNo = VisaBatchNo;
				
			item.AmexCollected = AmexCollected;
				
			item.AmexRecorded = AmexRecorded;
				
			item.AmexBatchNo = AmexBatchNo;
				
			item.ChinaNetsCollected = ChinaNetsCollected;
				
			item.ChinaNetsRecorded = ChinaNetsRecorded;
				
			item.ChinaNetsTerminalID = ChinaNetsTerminalID;
				
			item.VoucherCollected = VoucherCollected;
				
			item.VoucherRecorded = VoucherRecorded;
				
			item.DepositBagNo = DepositBagNo;
				
			item.TotalActualCollected = TotalActualCollected;
				
			item.ClosingCashOut = ClosingCashOut;
				
			item.Variance = Variance;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.UniqueID = UniqueID;
				
	        item.Save(UserName);
	    }
    }
}
