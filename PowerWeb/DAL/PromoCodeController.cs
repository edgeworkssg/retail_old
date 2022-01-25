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
    /// Controller class for PromoCode
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PromoCodeController
    {
        // Preload our schema..
        PromoCode thisSchemaLoad = new PromoCode();
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
        public PromoCodeCollection FetchAll()
        {
            PromoCodeCollection coll = new PromoCodeCollection();
            Query qry = new Query(PromoCode.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PromoCodeCollection FetchByID(object PromoCodeID)
        {
            PromoCodeCollection coll = new PromoCodeCollection().Where("PromoCodeID", PromoCodeID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PromoCodeCollection FetchByQuery(Query qry)
        {
            PromoCodeCollection coll = new PromoCodeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PromoCodeID)
        {
            return (PromoCode.Delete(PromoCodeID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PromoCodeID)
        {
            return (PromoCode.Destroy(PromoCodeID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string PromoCodeName,DateTime? ValidStartDate,DateTime? ValidEndDate,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    PromoCode item = new PromoCode();
		    
            item.PromoCodeName = PromoCodeName;
            
            item.ValidStartDate = ValidStartDate;
            
            item.ValidEndDate = ValidEndDate;
            
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
	    public void Update(int PromoCodeID,string PromoCodeName,DateTime? ValidStartDate,DateTime? ValidEndDate,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    PromoCode item = new PromoCode();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PromoCodeID = PromoCodeID;
				
			item.PromoCodeName = PromoCodeName;
				
			item.ValidStartDate = ValidStartDate;
				
			item.ValidEndDate = ValidEndDate;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
