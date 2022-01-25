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
    /// Controller class for RecordData
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RecordDatumController
    {
        // Preload our schema..
        RecordDatum thisSchemaLoad = new RecordDatum();
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
        public RecordDatumCollection FetchAll()
        {
            RecordDatumCollection coll = new RecordDatumCollection();
            Query qry = new Query(RecordDatum.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RecordDatumCollection FetchByID(object RecordDataID)
        {
            RecordDatumCollection coll = new RecordDatumCollection().Where("RecordDataID", RecordDataID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RecordDatumCollection FetchByQuery(Query qry)
        {
            RecordDatumCollection coll = new RecordDatumCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object RecordDataID)
        {
            return (RecordDatum.Delete(RecordDataID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object RecordDataID)
        {
            return (RecordDatum.Destroy(RecordDataID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int? InventoryLocationID,string Val1,string Val2,string Val3,string Val4,string Val5,string Val6,string Val7,string Val8,string Val9,string Val10,string InventoryHdrRefNo,DateTime? Timestamp,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string UniqueId)
	    {
		    RecordDatum item = new RecordDatum();
		    
            item.InventoryLocationID = InventoryLocationID;
            
            item.Val1 = Val1;
            
            item.Val2 = Val2;
            
            item.Val3 = Val3;
            
            item.Val4 = Val4;
            
            item.Val5 = Val5;
            
            item.Val6 = Val6;
            
            item.Val7 = Val7;
            
            item.Val8 = Val8;
            
            item.Val9 = Val9;
            
            item.Val10 = Val10;
            
            item.InventoryHdrRefNo = InventoryHdrRefNo;
            
            item.Timestamp = Timestamp;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.UniqueId = UniqueId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int RecordDataID,int? InventoryLocationID,string Val1,string Val2,string Val3,string Val4,string Val5,string Val6,string Val7,string Val8,string Val9,string Val10,string InventoryHdrRefNo,DateTime? Timestamp,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string UniqueId)
	    {
		    RecordDatum item = new RecordDatum();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.RecordDataID = RecordDataID;
				
			item.InventoryLocationID = InventoryLocationID;
				
			item.Val1 = Val1;
				
			item.Val2 = Val2;
				
			item.Val3 = Val3;
				
			item.Val4 = Val4;
				
			item.Val5 = Val5;
				
			item.Val6 = Val6;
				
			item.Val7 = Val7;
				
			item.Val8 = Val8;
				
			item.Val9 = Val9;
				
			item.Val10 = Val10;
				
			item.InventoryHdrRefNo = InventoryHdrRefNo;
				
			item.Timestamp = Timestamp;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.UniqueId = UniqueId;
				
	        item.Save(UserName);
	    }
    }
}
