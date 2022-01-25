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
    /// Controller class for BillInfo
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class BillInfoController
    {
        // Preload our schema..
        BillInfo thisSchemaLoad = new BillInfo();
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
        public BillInfoCollection FetchAll()
        {
            BillInfoCollection coll = new BillInfoCollection();
            Query qry = new Query(BillInfo.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public BillInfoCollection FetchByID(object BillInfoID)
        {
            BillInfoCollection coll = new BillInfoCollection().Where("BillInfoID", BillInfoID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public BillInfoCollection FetchByQuery(Query qry)
        {
            BillInfoCollection coll = new BillInfoCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object BillInfoID)
        {
            return (BillInfo.Delete(BillInfoID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object BillInfoID)
        {
            return (BillInfo.Destroy(BillInfoID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string BillInfoName,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid? UniqueID,bool? Deleted)
	    {
		    BillInfo item = new BillInfo();
		    
            item.BillInfoName = BillInfoName;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.UniqueID = UniqueID;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int BillInfoID,string BillInfoName,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid? UniqueID,bool? Deleted)
	    {
		    BillInfo item = new BillInfo();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.BillInfoID = BillInfoID;
				
			item.BillInfoName = BillInfoName;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.UniqueID = UniqueID;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
