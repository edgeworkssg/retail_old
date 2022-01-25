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
    /// Controller class for EZLinkUnCfmLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EZLinkUnCfmLogController
    {
        // Preload our schema..
        EZLinkUnCfmLog thisSchemaLoad = new EZLinkUnCfmLog();
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
        public EZLinkUnCfmLogCollection FetchAll()
        {
            EZLinkUnCfmLogCollection coll = new EZLinkUnCfmLogCollection();
            Query qry = new Query(EZLinkUnCfmLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EZLinkUnCfmLogCollection FetchByID(object RecordID)
        {
            EZLinkUnCfmLogCollection coll = new EZLinkUnCfmLogCollection().Where("RecordID", RecordID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EZLinkUnCfmLogCollection FetchByQuery(Query qry)
        {
            EZLinkUnCfmLogCollection coll = new EZLinkUnCfmLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object RecordID)
        {
            return (EZLinkUnCfmLog.Delete(RecordID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object RecordID)
        {
            return (EZLinkUnCfmLog.Destroy(RecordID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string CardID,string OrderDate,decimal UnConfirmAmt,string ReceiptNo,bool Deleted,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,Guid UniqueID)
	    {
		    EZLinkUnCfmLog item = new EZLinkUnCfmLog();
		    
            item.CardID = CardID;
            
            item.OrderDate = OrderDate;
            
            item.UnConfirmAmt = UnConfirmAmt;
            
            item.ReceiptNo = ReceiptNo;
            
            item.Deleted = Deleted;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedBy = ModifiedBy;
            
            item.UniqueID = UniqueID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int RecordID,string CardID,string OrderDate,decimal UnConfirmAmt,string ReceiptNo,bool Deleted,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,Guid UniqueID)
	    {
		    EZLinkUnCfmLog item = new EZLinkUnCfmLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.RecordID = RecordID;
				
			item.CardID = CardID;
				
			item.OrderDate = OrderDate;
				
			item.UnConfirmAmt = UnConfirmAmt;
				
			item.ReceiptNo = ReceiptNo;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
			item.UniqueID = UniqueID;
				
	        item.Save(UserName);
	    }
    }
}
