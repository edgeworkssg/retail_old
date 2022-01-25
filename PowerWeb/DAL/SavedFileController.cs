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
    /// Controller class for SavedFiles
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SavedFileController
    {
        // Preload our schema..
        SavedFile thisSchemaLoad = new SavedFile();
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
        public SavedFileCollection FetchAll()
        {
            SavedFileCollection coll = new SavedFileCollection();
            Query qry = new Query(SavedFile.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SavedFileCollection FetchByID(object SaveID)
        {
            SavedFileCollection coll = new SavedFileCollection().Where("SaveID", SaveID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SavedFileCollection FetchByQuery(Query qry)
        {
            SavedFileCollection coll = new SavedFileCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object SaveID)
        {
            return (SavedFile.Delete(SaveID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object SaveID)
        {
            return (SavedFile.Destroy(SaveID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string SaveName,string SavedBy,string Remark,string MovementType,DateTime? SavedDate,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    SavedFile item = new SavedFile();
		    
            item.SaveName = SaveName;
            
            item.SavedBy = SavedBy;
            
            item.Remark = Remark;
            
            item.MovementType = MovementType;
            
            item.SavedDate = SavedDate;
            
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
	    public void Update(int SaveID,string SaveName,string SavedBy,string Remark,string MovementType,DateTime? SavedDate,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    SavedFile item = new SavedFile();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.SaveID = SaveID;
				
			item.SaveName = SaveName;
				
			item.SavedBy = SavedBy;
				
			item.Remark = Remark;
				
			item.MovementType = MovementType;
				
			item.SavedDate = SavedDate;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
