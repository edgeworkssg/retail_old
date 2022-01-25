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
    /// Controller class for PromoOutlet
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PromoOutletController
    {
        // Preload our schema..
        PromoOutlet thisSchemaLoad = new PromoOutlet();
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
        public PromoOutletCollection FetchAll()
        {
            PromoOutletCollection coll = new PromoOutletCollection();
            Query qry = new Query(PromoOutlet.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PromoOutletCollection FetchByID(object PromoOutletID)
        {
            PromoOutletCollection coll = new PromoOutletCollection().Where("PromoOutletID", PromoOutletID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PromoOutletCollection FetchByQuery(Query qry)
        {
            PromoOutletCollection coll = new PromoOutletCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PromoOutletID)
        {
            return (PromoOutlet.Delete(PromoOutletID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PromoOutletID)
        {
            return (PromoOutlet.Destroy(PromoOutletID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int? PromoCampaignHdrID,string OutletName,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    PromoOutlet item = new PromoOutlet();
		    
            item.PromoCampaignHdrID = PromoCampaignHdrID;
            
            item.OutletName = OutletName;
            
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
	    public void Update(int PromoOutletID,int? PromoCampaignHdrID,string OutletName,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    PromoOutlet item = new PromoOutlet();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PromoOutletID = PromoOutletID;
				
			item.PromoCampaignHdrID = PromoCampaignHdrID;
				
			item.OutletName = OutletName;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
