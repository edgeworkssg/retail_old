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
    /// Controller class for RecipeHeader
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RecipeHeaderController
    {
        // Preload our schema..
        RecipeHeader thisSchemaLoad = new RecipeHeader();
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
        public RecipeHeaderCollection FetchAll()
        {
            RecipeHeaderCollection coll = new RecipeHeaderCollection();
            Query qry = new Query(RecipeHeader.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RecipeHeaderCollection FetchByID(object RecipeHeaderID)
        {
            RecipeHeaderCollection coll = new RecipeHeaderCollection().Where("RecipeHeaderID", RecipeHeaderID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RecipeHeaderCollection FetchByQuery(Query qry)
        {
            RecipeHeaderCollection coll = new RecipeHeaderCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object RecipeHeaderID)
        {
            return (RecipeHeader.Delete(RecipeHeaderID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object RecipeHeaderID)
        {
            return (RecipeHeader.Destroy(RecipeHeaderID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string RecipeHeaderID,string RecipeName,string ItemNo,string CreatedBy,DateTime CreatedOn,string ModifiedBy,DateTime? ModifiedOn,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,bool? Deleted)
	    {
		    RecipeHeader item = new RecipeHeader();
		    
            item.RecipeHeaderID = RecipeHeaderID;
            
            item.RecipeName = RecipeName;
            
            item.ItemNo = ItemNo;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.Userfld1 = Userfld1;
            
            item.Userfld2 = Userfld2;
            
            item.Userfld3 = Userfld3;
            
            item.Userfld4 = Userfld4;
            
            item.Userfld5 = Userfld5;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string RecipeHeaderID,string RecipeName,string ItemNo,string CreatedBy,DateTime CreatedOn,string ModifiedBy,DateTime? ModifiedOn,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,bool? Deleted)
	    {
		    RecipeHeader item = new RecipeHeader();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.RecipeHeaderID = RecipeHeaderID;
				
			item.RecipeName = RecipeName;
				
			item.ItemNo = ItemNo;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.Userfld1 = Userfld1;
				
			item.Userfld2 = Userfld2;
				
			item.Userfld3 = Userfld3;
				
			item.Userfld4 = Userfld4;
				
			item.Userfld5 = Userfld5;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
