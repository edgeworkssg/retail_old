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
    /// Controller class for CashTransactionLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CashTransactionLogController
    {
        // Preload our schema..
        CashTransactionLog thisSchemaLoad = new CashTransactionLog();
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
        public CashTransactionLogCollection FetchAll()
        {
            CashTransactionLogCollection coll = new CashTransactionLogCollection();
            Query qry = new Query(CashTransactionLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CashTransactionLogCollection FetchByID(object Id)
        {
            CashTransactionLogCollection coll = new CashTransactionLogCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CashTransactionLogCollection FetchByQuery(Query qry)
        {
            CashTransactionLogCollection coll = new CashTransactionLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (CashTransactionLog.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (CashTransactionLog.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string TransactionID,string TransactionType,string Currency,int Denomination,int Quantity,decimal? Amount,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    CashTransactionLog item = new CashTransactionLog();
		    
            item.TransactionID = TransactionID;
            
            item.TransactionType = TransactionType;
            
            item.Currency = Currency;
            
            item.Denomination = Denomination;
            
            item.Quantity = Quantity;
            
            item.Amount = Amount;
            
            item.Deleted = Deleted;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string TransactionID,string TransactionType,string Currency,int Denomination,int Quantity,decimal? Amount,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    CashTransactionLog item = new CashTransactionLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TransactionID = TransactionID;
				
			item.TransactionType = TransactionType;
				
			item.Currency = Currency;
				
			item.Denomination = Denomination;
				
			item.Quantity = Quantity;
				
			item.Amount = Amount;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
