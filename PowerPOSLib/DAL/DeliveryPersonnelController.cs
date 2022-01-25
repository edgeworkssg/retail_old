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
    /// Controller class for Delivery_Personnel
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class DeliveryPersonnelController
    {
        // Preload our schema..
        DeliveryPersonnel thisSchemaLoad = new DeliveryPersonnel();
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
        public DeliveryPersonnelCollection FetchAll()
        {
            DeliveryPersonnelCollection coll = new DeliveryPersonnelCollection();
            Query qry = new Query(DeliveryPersonnel.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DeliveryPersonnelCollection FetchByID(object PersonnelId)
        {
            DeliveryPersonnelCollection coll = new DeliveryPersonnelCollection().Where("Personnel_ID", PersonnelId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public DeliveryPersonnelCollection FetchByQuery(Query qry)
        {
            DeliveryPersonnelCollection coll = new DeliveryPersonnelCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PersonnelId)
        {
            return (DeliveryPersonnel.Delete(PersonnelId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PersonnelId)
        {
            return (DeliveryPersonnel.Destroy(PersonnelId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int PersonnelId,string PersonnelName,string Remarks,DateTime? TimeFrom,DateTime? TimeTo)
	    {
		    DeliveryPersonnel item = new DeliveryPersonnel();
		    
            item.PersonnelId = PersonnelId;
            
            item.PersonnelName = PersonnelName;
            
            item.Remarks = Remarks;
            
            item.TimeFrom = TimeFrom;
            
            item.TimeTo = TimeTo;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int PersonnelId,string PersonnelName,string Remarks,DateTime? TimeFrom,DateTime? TimeTo)
	    {
		    DeliveryPersonnel item = new DeliveryPersonnel();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PersonnelId = PersonnelId;
				
			item.PersonnelName = PersonnelName;
				
			item.Remarks = Remarks;
				
			item.TimeFrom = TimeFrom;
				
			item.TimeTo = TimeTo;
				
	        item.Save(UserName);
	    }
    }
}
