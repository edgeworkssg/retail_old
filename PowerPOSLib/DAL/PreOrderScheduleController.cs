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
    /// Controller class for PreOrderSchedule
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PreOrderScheduleController
    {
        // Preload our schema..
        PreOrderSchedule thisSchemaLoad = new PreOrderSchedule();
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
        public PreOrderScheduleCollection FetchAll()
        {
            PreOrderScheduleCollection coll = new PreOrderScheduleCollection();
            Query qry = new Query(PreOrderSchedule.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PreOrderScheduleCollection FetchByID(object PreOrderID)
        {
            PreOrderScheduleCollection coll = new PreOrderScheduleCollection().Where("PreOrderID", PreOrderID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PreOrderScheduleCollection FetchByQuery(Query qry)
        {
            PreOrderScheduleCollection coll = new PreOrderScheduleCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PreOrderID)
        {
            return (PreOrderSchedule.Delete(PreOrderID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PreOrderID)
        {
            return (PreOrderSchedule.Destroy(PreOrderID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ItemNo,DateTime? ValidFrom,DateTime? ValidTo,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    PreOrderSchedule item = new PreOrderSchedule();
		    
            item.ItemNo = ItemNo;
            
            item.ValidFrom = ValidFrom;
            
            item.ValidTo = ValidTo;
            
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
	    public void Update(int PreOrderID,string ItemNo,DateTime? ValidFrom,DateTime? ValidTo,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    PreOrderSchedule item = new PreOrderSchedule();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PreOrderID = PreOrderID;
				
			item.ItemNo = ItemNo;
				
			item.ValidFrom = ValidFrom;
				
			item.ValidTo = ValidTo;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
