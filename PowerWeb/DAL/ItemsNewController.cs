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
    /// Controller class for ItemsNew
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ItemsNewController
    {
        // Preload our schema..
        ItemsNew thisSchemaLoad = new ItemsNew();
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
        public ItemsNewCollection FetchAll()
        {
            ItemsNewCollection coll = new ItemsNewCollection();
            Query qry = new Query(ItemsNew.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemsNewCollection FetchByID(object Id)
        {
            ItemsNewCollection coll = new ItemsNewCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemsNewCollection FetchByQuery(Query qry)
        {
            ItemsNewCollection coll = new ItemsNewCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ItemsNew.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ItemsNew.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int Id,string Title,string Description,string Members,DateTime? StartDate,DateTime? EndDate,bool? IsAllDay,int? Place)
	    {
		    ItemsNew item = new ItemsNew();
		    
            item.Id = Id;
            
            item.Title = Title;
            
            item.Description = Description;
            
            item.Members = Members;
            
            item.StartDate = StartDate;
            
            item.EndDate = EndDate;
            
            item.IsAllDay = IsAllDay;
            
            item.Place = Place;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Title,string Description,string Members,DateTime? StartDate,DateTime? EndDate,bool? IsAllDay,int? Place)
	    {
		    ItemsNew item = new ItemsNew();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Title = Title;
				
			item.Description = Description;
				
			item.Members = Members;
				
			item.StartDate = StartDate;
				
			item.EndDate = EndDate;
				
			item.IsAllDay = IsAllDay;
				
			item.Place = Place;
				
	        item.Save(UserName);
	    }
    }
}
