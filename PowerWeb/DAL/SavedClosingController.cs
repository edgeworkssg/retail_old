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
    /// Controller class for SavedClosing
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SavedClosingController
    {
        // Preload our schema..
        SavedClosing thisSchemaLoad = new SavedClosing();
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
        public SavedClosingCollection FetchAll()
        {
            SavedClosingCollection coll = new SavedClosingCollection();
            Query qry = new Query(SavedClosing.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SavedClosingCollection FetchByID(object SavedID)
        {
            SavedClosingCollection coll = new SavedClosingCollection().Where("SavedID", SavedID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SavedClosingCollection FetchByQuery(Query qry)
        {
            SavedClosingCollection coll = new SavedClosingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object SavedID)
        {
            return (SavedClosing.Delete(SavedID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object SavedID)
        {
            return (SavedClosing.Destroy(SavedID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string FloatBalance,string NetsCollected,string NetsTerminalID,string VisaCollected,string VisaBatchNo,string AmexCollected,string AmexBatchNo,string ChinaNetsCollected,string ChinaNetsTerminalID,string VoucherCollected,string ChequeCollected,string DepositBagNo,string PointOfSaleID,string C100,string C50,string C10,string C5,string C2,string C1,string C050,string C020,string C010,string C005)
	    {
		    SavedClosing item = new SavedClosing();
		    
            item.FloatBalance = FloatBalance;
            
            item.NetsCollected = NetsCollected;
            
            item.NetsTerminalID = NetsTerminalID;
            
            item.VisaCollected = VisaCollected;
            
            item.VisaBatchNo = VisaBatchNo;
            
            item.AmexCollected = AmexCollected;
            
            item.AmexBatchNo = AmexBatchNo;
            
            item.ChinaNetsCollected = ChinaNetsCollected;
            
            item.ChinaNetsTerminalID = ChinaNetsTerminalID;
            
            item.VoucherCollected = VoucherCollected;
            
            item.ChequeCollected = ChequeCollected;
            
            item.DepositBagNo = DepositBagNo;
            
            item.PointOfSaleID = PointOfSaleID;
            
            item.C100 = C100;
            
            item.C50 = C50;
            
            item.C10 = C10;
            
            item.C5 = C5;
            
            item.C2 = C2;
            
            item.C1 = C1;
            
            item.C050 = C050;
            
            item.C020 = C020;
            
            item.C010 = C010;
            
            item.C005 = C005;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int SavedID,string FloatBalance,string NetsCollected,string NetsTerminalID,string VisaCollected,string VisaBatchNo,string AmexCollected,string AmexBatchNo,string ChinaNetsCollected,string ChinaNetsTerminalID,string VoucherCollected,string ChequeCollected,string DepositBagNo,string PointOfSaleID,string C100,string C50,string C10,string C5,string C2,string C1,string C050,string C020,string C010,string C005)
	    {
		    SavedClosing item = new SavedClosing();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.SavedID = SavedID;
				
			item.FloatBalance = FloatBalance;
				
			item.NetsCollected = NetsCollected;
				
			item.NetsTerminalID = NetsTerminalID;
				
			item.VisaCollected = VisaCollected;
				
			item.VisaBatchNo = VisaBatchNo;
				
			item.AmexCollected = AmexCollected;
				
			item.AmexBatchNo = AmexBatchNo;
				
			item.ChinaNetsCollected = ChinaNetsCollected;
				
			item.ChinaNetsTerminalID = ChinaNetsTerminalID;
				
			item.VoucherCollected = VoucherCollected;
				
			item.ChequeCollected = ChequeCollected;
				
			item.DepositBagNo = DepositBagNo;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.C100 = C100;
				
			item.C50 = C50;
				
			item.C10 = C10;
				
			item.C5 = C5;
				
			item.C2 = C2;
				
			item.C1 = C1;
				
			item.C050 = C050;
				
			item.C020 = C020;
				
			item.C010 = C010;
				
			item.C005 = C005;
				
	        item.Save(UserName);
	    }
    }
}
