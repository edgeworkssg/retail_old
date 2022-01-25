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
    /// Controller class for QuickAccessCategory
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class QuickAccessCategoryController
    {
        // Preload our schema..
        QuickAccessCategory thisSchemaLoad = new QuickAccessCategory();
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
        public QuickAccessCategoryCollection FetchAll()
        {
            QuickAccessCategoryCollection coll = new QuickAccessCategoryCollection();
            Query qry = new Query(QuickAccessCategory.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public QuickAccessCategoryCollection FetchByID(object QuickAccessCategoryId)
        {
            QuickAccessCategoryCollection coll = new QuickAccessCategoryCollection().Where("QuickAccessCategoryId", QuickAccessCategoryId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public QuickAccessCategoryCollection FetchByQuery(Query qry)
        {
            QuickAccessCategoryCollection coll = new QuickAccessCategoryCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object QuickAccessCategoryId)
        {
            return (QuickAccessCategory.Delete(QuickAccessCategoryId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object QuickAccessCategoryId)
        {
            return (QuickAccessCategory.Destroy(QuickAccessCategoryId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid QuickAccessCategoryId,string QuickAccessCatName,int PriorityLevel,int? PointOfSaleID,string ForeColor,string BackColor,string Label,bool ShowLabel,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    QuickAccessCategory item = new QuickAccessCategory();
		    
            item.QuickAccessCategoryId = QuickAccessCategoryId;
            
            item.QuickAccessCatName = QuickAccessCatName;
            
            item.PriorityLevel = PriorityLevel;
            
            item.PointOfSaleID = PointOfSaleID;
            
            item.ForeColor = ForeColor;
            
            item.BackColor = BackColor;
            
            item.Label = Label;
            
            item.ShowLabel = ShowLabel;
            
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
	    public void Update(Guid QuickAccessCategoryId,string QuickAccessCatName,int PriorityLevel,int? PointOfSaleID,string ForeColor,string BackColor,string Label,bool ShowLabel,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    QuickAccessCategory item = new QuickAccessCategory();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.QuickAccessCategoryId = QuickAccessCategoryId;
				
			item.QuickAccessCatName = QuickAccessCatName;
				
			item.PriorityLevel = PriorityLevel;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.ForeColor = ForeColor;
				
			item.BackColor = BackColor;
				
			item.Label = Label;
				
			item.ShowLabel = ShowLabel;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
