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
    /// Controller class for CounterCloseLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CounterCloseLogController
    {
        // Preload our schema..
        CounterCloseLog thisSchemaLoad = new CounterCloseLog();
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
        public CounterCloseLogCollection FetchAll()
        {
            CounterCloseLogCollection coll = new CounterCloseLogCollection();
            Query qry = new Query(CounterCloseLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CounterCloseLogCollection FetchByID(object CounterCloseLogID)
        {
            CounterCloseLogCollection coll = new CounterCloseLogCollection().Where("CounterCloseLogID", CounterCloseLogID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CounterCloseLogCollection FetchByQuery(Query qry)
        {
            CounterCloseLogCollection coll = new CounterCloseLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object CounterCloseLogID)
        {
            return (CounterCloseLog.Delete(CounterCloseLogID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object CounterCloseLogID)
        {
            return (CounterCloseLog.Destroy(CounterCloseLogID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string CounterCloseID,decimal FloatBalance,DateTime StartTime,DateTime EndTime,string Cashier,string Supervisor,decimal CashIn,decimal CashOut,decimal OpeningBalance,decimal TotalSystemRecorded,decimal CashCollected,decimal? CashRecorded,decimal NetsCollected,decimal? NetsRecorded,string NetsTerminalID,decimal VisaCollected,decimal? VisaRecorded,string VisaBatchNo,decimal AmexCollected,decimal? AmexRecorded,string AmexBatchNo,decimal ChinaNetsCollected,decimal? ChinaNetsRecorded,string ChinaNetsTerminalID,decimal VoucherCollected,decimal? VoucherRecorded,string DepositBagNo,decimal TotalActualCollected,decimal ClosingCashOut,decimal Variance,int PointOfSaleID,DateTime CreatedOn,string CreatedBy,DateTime ModifiedOn,string ModifiedBy,Guid UniqueID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,decimal? Userfloat6,decimal? Userfloat7,decimal? Userfloat8,decimal? Userfloat9,decimal? Userfloat10,decimal? Userfloat11,decimal? Userfloat12,decimal? Userfloat13,decimal? Userfloat14,decimal? Userfloat15,decimal? NetsCashCardCollected,decimal? NetsCashCardRecorded,decimal? NetsFlashPayCollected,decimal? NetsFlashPayRecorded,decimal? NetsATMCardCollected,decimal? NetsATMCardRecorded,string ForeignCurrency1,decimal? ForeignCurrency1Recorded,decimal? ForeignCurrency1Collected,string ForeignCurrency2,decimal? ForeignCurrency2Recorded,decimal? ForeignCurrency2Collected,string ForeignCurrency3,decimal? ForeignCurrency3Recorded,decimal? ForeignCurrency3Collected,string ForeignCurrency4,decimal? ForeignCurrency4Recorded,decimal? ForeignCurrency4Collected,string ForeignCurrency5,decimal? ForeignCurrency5Recorded,decimal? ForeignCurrency5Collected,decimal? TotalForeignCurrency,decimal? Pay7Collected,decimal? Pay7Recorded,decimal? Pay8Collected,decimal? Pay8Recorded,decimal? Pay9Collected,decimal? Pay9Recorded,decimal? Pay10Collected,decimal? Pay10Recorded)
	    {
		    CounterCloseLog item = new CounterCloseLog();
		    
            item.CounterCloseID = CounterCloseID;
            
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
            
            item.Userfld1 = Userfld1;
            
            item.Userfld2 = Userfld2;
            
            item.Userfld3 = Userfld3;
            
            item.Userfld4 = Userfld4;
            
            item.Userfld5 = Userfld5;
            
            item.Userfld6 = Userfld6;
            
            item.Userfld7 = Userfld7;
            
            item.Userfld8 = Userfld8;
            
            item.Userfld9 = Userfld9;
            
            item.Userfld10 = Userfld10;
            
            item.Userflag1 = Userflag1;
            
            item.Userflag2 = Userflag2;
            
            item.Userflag3 = Userflag3;
            
            item.Userflag4 = Userflag4;
            
            item.Userflag5 = Userflag5;
            
            item.Userfloat1 = Userfloat1;
            
            item.Userfloat2 = Userfloat2;
            
            item.Userfloat3 = Userfloat3;
            
            item.Userfloat4 = Userfloat4;
            
            item.Userfloat5 = Userfloat5;
            
            item.Userint1 = Userint1;
            
            item.Userint2 = Userint2;
            
            item.Userint3 = Userint3;
            
            item.Userint4 = Userint4;
            
            item.Userint5 = Userint5;
            
            item.Userfloat6 = Userfloat6;
            
            item.Userfloat7 = Userfloat7;
            
            item.Userfloat8 = Userfloat8;
            
            item.Userfloat9 = Userfloat9;
            
            item.Userfloat10 = Userfloat10;
            
            item.Userfloat11 = Userfloat11;
            
            item.Userfloat12 = Userfloat12;
            
            item.Userfloat13 = Userfloat13;
            
            item.Userfloat14 = Userfloat14;
            
            item.Userfloat15 = Userfloat15;
            
            item.NetsCashCardCollected = NetsCashCardCollected;
            
            item.NetsCashCardRecorded = NetsCashCardRecorded;
            
            item.NetsFlashPayCollected = NetsFlashPayCollected;
            
            item.NetsFlashPayRecorded = NetsFlashPayRecorded;
            
            item.NetsATMCardCollected = NetsATMCardCollected;
            
            item.NetsATMCardRecorded = NetsATMCardRecorded;
            
            item.ForeignCurrency1 = ForeignCurrency1;
            
            item.ForeignCurrency1Recorded = ForeignCurrency1Recorded;
            
            item.ForeignCurrency1Collected = ForeignCurrency1Collected;
            
            item.ForeignCurrency2 = ForeignCurrency2;
            
            item.ForeignCurrency2Recorded = ForeignCurrency2Recorded;
            
            item.ForeignCurrency2Collected = ForeignCurrency2Collected;
            
            item.ForeignCurrency3 = ForeignCurrency3;
            
            item.ForeignCurrency3Recorded = ForeignCurrency3Recorded;
            
            item.ForeignCurrency3Collected = ForeignCurrency3Collected;
            
            item.ForeignCurrency4 = ForeignCurrency4;
            
            item.ForeignCurrency4Recorded = ForeignCurrency4Recorded;
            
            item.ForeignCurrency4Collected = ForeignCurrency4Collected;
            
            item.ForeignCurrency5 = ForeignCurrency5;
            
            item.ForeignCurrency5Recorded = ForeignCurrency5Recorded;
            
            item.ForeignCurrency5Collected = ForeignCurrency5Collected;
            
            item.TotalForeignCurrency = TotalForeignCurrency;
            
            item.Pay7Collected = Pay7Collected;
            
            item.Pay7Recorded = Pay7Recorded;
            
            item.Pay8Collected = Pay8Collected;
            
            item.Pay8Recorded = Pay8Recorded;
            
            item.Pay9Collected = Pay9Collected;
            
            item.Pay9Recorded = Pay9Recorded;
            
            item.Pay10Collected = Pay10Collected;
            
            item.Pay10Recorded = Pay10Recorded;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int CounterCloseLogID,string CounterCloseID,decimal FloatBalance,DateTime StartTime,DateTime EndTime,string Cashier,string Supervisor,decimal CashIn,decimal CashOut,decimal OpeningBalance,decimal TotalSystemRecorded,decimal CashCollected,decimal? CashRecorded,decimal NetsCollected,decimal? NetsRecorded,string NetsTerminalID,decimal VisaCollected,decimal? VisaRecorded,string VisaBatchNo,decimal AmexCollected,decimal? AmexRecorded,string AmexBatchNo,decimal ChinaNetsCollected,decimal? ChinaNetsRecorded,string ChinaNetsTerminalID,decimal VoucherCollected,decimal? VoucherRecorded,string DepositBagNo,decimal TotalActualCollected,decimal ClosingCashOut,decimal Variance,int PointOfSaleID,DateTime CreatedOn,string CreatedBy,DateTime ModifiedOn,string ModifiedBy,Guid UniqueID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,decimal? Userfloat6,decimal? Userfloat7,decimal? Userfloat8,decimal? Userfloat9,decimal? Userfloat10,decimal? Userfloat11,decimal? Userfloat12,decimal? Userfloat13,decimal? Userfloat14,decimal? Userfloat15,decimal? NetsCashCardCollected,decimal? NetsCashCardRecorded,decimal? NetsFlashPayCollected,decimal? NetsFlashPayRecorded,decimal? NetsATMCardCollected,decimal? NetsATMCardRecorded,string ForeignCurrency1,decimal? ForeignCurrency1Recorded,decimal? ForeignCurrency1Collected,string ForeignCurrency2,decimal? ForeignCurrency2Recorded,decimal? ForeignCurrency2Collected,string ForeignCurrency3,decimal? ForeignCurrency3Recorded,decimal? ForeignCurrency3Collected,string ForeignCurrency4,decimal? ForeignCurrency4Recorded,decimal? ForeignCurrency4Collected,string ForeignCurrency5,decimal? ForeignCurrency5Recorded,decimal? ForeignCurrency5Collected,decimal? TotalForeignCurrency,decimal? Pay7Collected,decimal? Pay7Recorded,decimal? Pay8Collected,decimal? Pay8Recorded,decimal? Pay9Collected,decimal? Pay9Recorded,decimal? Pay10Collected,decimal? Pay10Recorded)
	    {
		    CounterCloseLog item = new CounterCloseLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.CounterCloseLogID = CounterCloseLogID;
				
			item.CounterCloseID = CounterCloseID;
				
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
				
			item.Userfld1 = Userfld1;
				
			item.Userfld2 = Userfld2;
				
			item.Userfld3 = Userfld3;
				
			item.Userfld4 = Userfld4;
				
			item.Userfld5 = Userfld5;
				
			item.Userfld6 = Userfld6;
				
			item.Userfld7 = Userfld7;
				
			item.Userfld8 = Userfld8;
				
			item.Userfld9 = Userfld9;
				
			item.Userfld10 = Userfld10;
				
			item.Userflag1 = Userflag1;
				
			item.Userflag2 = Userflag2;
				
			item.Userflag3 = Userflag3;
				
			item.Userflag4 = Userflag4;
				
			item.Userflag5 = Userflag5;
				
			item.Userfloat1 = Userfloat1;
				
			item.Userfloat2 = Userfloat2;
				
			item.Userfloat3 = Userfloat3;
				
			item.Userfloat4 = Userfloat4;
				
			item.Userfloat5 = Userfloat5;
				
			item.Userint1 = Userint1;
				
			item.Userint2 = Userint2;
				
			item.Userint3 = Userint3;
				
			item.Userint4 = Userint4;
				
			item.Userint5 = Userint5;
				
			item.Userfloat6 = Userfloat6;
				
			item.Userfloat7 = Userfloat7;
				
			item.Userfloat8 = Userfloat8;
				
			item.Userfloat9 = Userfloat9;
				
			item.Userfloat10 = Userfloat10;
				
			item.Userfloat11 = Userfloat11;
				
			item.Userfloat12 = Userfloat12;
				
			item.Userfloat13 = Userfloat13;
				
			item.Userfloat14 = Userfloat14;
				
			item.Userfloat15 = Userfloat15;
				
			item.NetsCashCardCollected = NetsCashCardCollected;
				
			item.NetsCashCardRecorded = NetsCashCardRecorded;
				
			item.NetsFlashPayCollected = NetsFlashPayCollected;
				
			item.NetsFlashPayRecorded = NetsFlashPayRecorded;
				
			item.NetsATMCardCollected = NetsATMCardCollected;
				
			item.NetsATMCardRecorded = NetsATMCardRecorded;
				
			item.ForeignCurrency1 = ForeignCurrency1;
				
			item.ForeignCurrency1Recorded = ForeignCurrency1Recorded;
				
			item.ForeignCurrency1Collected = ForeignCurrency1Collected;
				
			item.ForeignCurrency2 = ForeignCurrency2;
				
			item.ForeignCurrency2Recorded = ForeignCurrency2Recorded;
				
			item.ForeignCurrency2Collected = ForeignCurrency2Collected;
				
			item.ForeignCurrency3 = ForeignCurrency3;
				
			item.ForeignCurrency3Recorded = ForeignCurrency3Recorded;
				
			item.ForeignCurrency3Collected = ForeignCurrency3Collected;
				
			item.ForeignCurrency4 = ForeignCurrency4;
				
			item.ForeignCurrency4Recorded = ForeignCurrency4Recorded;
				
			item.ForeignCurrency4Collected = ForeignCurrency4Collected;
				
			item.ForeignCurrency5 = ForeignCurrency5;
				
			item.ForeignCurrency5Recorded = ForeignCurrency5Recorded;
				
			item.ForeignCurrency5Collected = ForeignCurrency5Collected;
				
			item.TotalForeignCurrency = TotalForeignCurrency;
				
			item.Pay7Collected = Pay7Collected;
				
			item.Pay7Recorded = Pay7Recorded;
				
			item.Pay8Collected = Pay8Collected;
				
			item.Pay8Recorded = Pay8Recorded;
				
			item.Pay9Collected = Pay9Collected;
				
			item.Pay9Recorded = Pay9Recorded;
				
			item.Pay10Collected = Pay10Collected;
				
			item.Pay10Recorded = Pay10Recorded;
				
	        item.Save(UserName);
	    }
    }
}
