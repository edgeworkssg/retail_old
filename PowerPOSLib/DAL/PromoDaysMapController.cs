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
    /// Controller class for PromoDaysMap
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PromoDaysMapController
    {
        // Preload our schema..
        PromoDaysMap thisSchemaLoad = new PromoDaysMap();
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
        public PromoDaysMapCollection FetchAll()
        {
            PromoDaysMapCollection coll = new PromoDaysMapCollection();
            Query qry = new Query(PromoDaysMap.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PromoDaysMapCollection FetchByID(object PromoDaysID)
        {
            PromoDaysMapCollection coll = new PromoDaysMapCollection().Where("PromoDaysID", PromoDaysID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PromoDaysMapCollection FetchByQuery(Query qry)
        {
            PromoDaysMapCollection coll = new PromoDaysMapCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PromoDaysID)
        {
            return (PromoDaysMap.Delete(PromoDaysID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PromoDaysID)
        {
            return (PromoDaysMap.Destroy(PromoDaysID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int? PromoCampaignHdrID,string DaysPromo,int? DaysNumber,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool? Deleted)
	    {
		    PromoDaysMap item = new PromoDaysMap();
		    
            item.PromoCampaignHdrID = PromoCampaignHdrID;
            
            item.DaysPromo = DaysPromo;
            
            item.DaysNumber = DaysNumber;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int PromoDaysID,int? PromoCampaignHdrID,string DaysPromo,int? DaysNumber,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool? Deleted)
	    {
		    PromoDaysMap item = new PromoDaysMap();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PromoDaysID = PromoDaysID;
				
			item.PromoCampaignHdrID = PromoCampaignHdrID;
				
			item.DaysPromo = DaysPromo;
				
			item.DaysNumber = DaysNumber;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
