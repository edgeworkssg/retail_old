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
    /// Controller class for AppointmentItem
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AppointmentItemController
    {
        // Preload our schema..
        AppointmentItem thisSchemaLoad = new AppointmentItem();
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
        public AppointmentItemCollection FetchAll()
        {
            AppointmentItemCollection coll = new AppointmentItemCollection();
            Query qry = new Query(AppointmentItem.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppointmentItemCollection FetchByID(object Id)
        {
            AppointmentItemCollection coll = new AppointmentItemCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppointmentItemCollection FetchByQuery(Query qry)
        {
            AppointmentItemCollection coll = new AppointmentItemCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (AppointmentItem.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (AppointmentItem.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid Id,Guid AppointmentId,string ItemNo,decimal UnitPrice,int Quantity,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool? Deleted)
	    {
		    AppointmentItem item = new AppointmentItem();
		    
            item.Id = Id;
            
            item.AppointmentId = AppointmentId;
            
            item.ItemNo = ItemNo;
            
            item.UnitPrice = UnitPrice;
            
            item.Quantity = Quantity;
            
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
	    public void Update(Guid Id,Guid AppointmentId,string ItemNo,decimal UnitPrice,int Quantity,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool? Deleted)
	    {
		    AppointmentItem item = new AppointmentItem();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.AppointmentId = AppointmentId;
				
			item.ItemNo = ItemNo;
				
			item.UnitPrice = UnitPrice;
				
			item.Quantity = Quantity;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
