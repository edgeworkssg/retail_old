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
    /// Controller class for EZLinkMessageParameter
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EZLinkMessageParameterController
    {
        // Preload our schema..
        EZLinkMessageParameter thisSchemaLoad = new EZLinkMessageParameter();
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
        public EZLinkMessageParameterCollection FetchAll()
        {
            EZLinkMessageParameterCollection coll = new EZLinkMessageParameterCollection();
            Query qry = new Query(EZLinkMessageParameter.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EZLinkMessageParameterCollection FetchByID(object ParameterName)
        {
            EZLinkMessageParameterCollection coll = new EZLinkMessageParameterCollection().Where("ParameterName", ParameterName).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EZLinkMessageParameterCollection FetchByQuery(Query qry)
        {
            EZLinkMessageParameterCollection coll = new EZLinkMessageParameterCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ParameterName)
        {
            return (EZLinkMessageParameter.Delete(ParameterName) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ParameterName)
        {
            return (EZLinkMessageParameter.Destroy(ParameterName) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ParameterName,int ParameterLength,bool Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid UniqueID)
	    {
		    EZLinkMessageParameter item = new EZLinkMessageParameter();
		    
            item.ParameterName = ParameterName;
            
            item.ParameterLength = ParameterLength;
            
            item.Deleted = Deleted;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.UniqueID = UniqueID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string ParameterName,int ParameterLength,bool Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid UniqueID)
	    {
		    EZLinkMessageParameter item = new EZLinkMessageParameter();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ParameterName = ParameterName;
				
			item.ParameterLength = ParameterLength;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.UniqueID = UniqueID;
				
	        item.Save(UserName);
	    }
    }
}
