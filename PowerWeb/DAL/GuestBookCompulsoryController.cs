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
    /// Controller class for GuestBookCompulsory
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class GuestBookCompulsoryController
    {
        // Preload our schema..
        GuestBookCompulsory thisSchemaLoad = new GuestBookCompulsory();
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
        public GuestBookCompulsoryCollection FetchAll()
        {
            GuestBookCompulsoryCollection coll = new GuestBookCompulsoryCollection();
            Query qry = new Query(GuestBookCompulsory.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public GuestBookCompulsoryCollection FetchByID(object GuestBookCompulsoryID)
        {
            GuestBookCompulsoryCollection coll = new GuestBookCompulsoryCollection().Where("GuestBookCompulsoryID", GuestBookCompulsoryID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public GuestBookCompulsoryCollection FetchByQuery(Query qry)
        {
            GuestBookCompulsoryCollection coll = new GuestBookCompulsoryCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object GuestBookCompulsoryID)
        {
            return (GuestBookCompulsory.Delete(GuestBookCompulsoryID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object GuestBookCompulsoryID)
        {
            return (GuestBookCompulsory.Destroy(GuestBookCompulsoryID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string FieldName,bool? IsCompulsory,bool? IsVisible,string Remark,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,string Label,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5)
	    {
		    GuestBookCompulsory item = new GuestBookCompulsory();
		    
            item.FieldName = FieldName;
            
            item.IsCompulsory = IsCompulsory;
            
            item.IsVisible = IsVisible;
            
            item.Remark = Remark;
            
            item.Deleted = Deleted;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Label = Label;
            
            item.Userfld1 = Userfld1;
            
            item.Userfld2 = Userfld2;
            
            item.Userfld3 = Userfld3;
            
            item.Userfld4 = Userfld4;
            
            item.Userfld5 = Userfld5;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int GuestBookCompulsoryID,string FieldName,bool? IsCompulsory,bool? IsVisible,string Remark,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,string Label,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5)
	    {
		    GuestBookCompulsory item = new GuestBookCompulsory();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.GuestBookCompulsoryID = GuestBookCompulsoryID;
				
			item.FieldName = FieldName;
				
			item.IsCompulsory = IsCompulsory;
				
			item.IsVisible = IsVisible;
				
			item.Remark = Remark;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Label = Label;
				
			item.Userfld1 = Userfld1;
				
			item.Userfld2 = Userfld2;
				
			item.Userfld3 = Userfld3;
				
			item.Userfld4 = Userfld4;
				
			item.Userfld5 = Userfld5;
				
	        item.Save(UserName);
	    }
    }
}
