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
    /// Controller class for RedemptionItem
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RedemptionItemController
    {
        // Preload our schema..
        RedemptionItem thisSchemaLoad = new RedemptionItem();
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
        public RedemptionItemCollection FetchAll()
        {
            RedemptionItemCollection coll = new RedemptionItemCollection();
            Query qry = new Query(RedemptionItem.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RedemptionItemCollection FetchByID(object RedemptionID)
        {
            RedemptionItemCollection coll = new RedemptionItemCollection().Where("RedemptionID", RedemptionID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RedemptionItemCollection FetchByQuery(Query qry)
        {
            RedemptionItemCollection coll = new RedemptionItemCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object RedemptionID)
        {
            return (RedemptionItem.Delete(RedemptionID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object RedemptionID)
        {
            return (RedemptionItem.Destroy(RedemptionID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Description,string ItemNo,decimal PointRequired,DateTime ValidStartDate,DateTime ValidEndDate,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    RedemptionItem item = new RedemptionItem();
		    
            item.Description = Description;
            
            item.ItemNo = ItemNo;
            
            item.PointRequired = PointRequired;
            
            item.ValidStartDate = ValidStartDate;
            
            item.ValidEndDate = ValidEndDate;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int RedemptionID,string Description,string ItemNo,decimal PointRequired,DateTime ValidStartDate,DateTime ValidEndDate,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    RedemptionItem item = new RedemptionItem();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.RedemptionID = RedemptionID;
				
			item.Description = Description;
				
			item.ItemNo = ItemNo;
				
			item.PointRequired = PointRequired;
				
			item.ValidStartDate = ValidStartDate;
				
			item.ValidEndDate = ValidEndDate;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
