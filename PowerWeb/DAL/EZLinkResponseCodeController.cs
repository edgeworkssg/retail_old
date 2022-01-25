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
    /// Controller class for EZLinkResponseCode
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EZLinkResponseCodeController
    {
        // Preload our schema..
        EZLinkResponseCode thisSchemaLoad = new EZLinkResponseCode();
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
        public EZLinkResponseCodeCollection FetchAll()
        {
            EZLinkResponseCodeCollection coll = new EZLinkResponseCodeCollection();
            Query qry = new Query(EZLinkResponseCode.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EZLinkResponseCodeCollection FetchByID(object ResponseCode)
        {
            EZLinkResponseCodeCollection coll = new EZLinkResponseCodeCollection().Where("ResponseCode", ResponseCode).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EZLinkResponseCodeCollection FetchByQuery(Query qry)
        {
            EZLinkResponseCodeCollection coll = new EZLinkResponseCodeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ResponseCode)
        {
            return (EZLinkResponseCode.Delete(ResponseCode) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ResponseCode)
        {
            return (EZLinkResponseCode.Destroy(ResponseCode) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ResponseCode,string ResponseMessage,bool Deleted,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,Guid UniqueID)
	    {
		    EZLinkResponseCode item = new EZLinkResponseCode();
		    
            item.ResponseCode = ResponseCode;
            
            item.ResponseMessage = ResponseMessage;
            
            item.Deleted = Deleted;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedBy = ModifiedBy;
            
            item.UniqueID = UniqueID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string ResponseCode,string ResponseMessage,bool Deleted,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,Guid UniqueID)
	    {
		    EZLinkResponseCode item = new EZLinkResponseCode();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ResponseCode = ResponseCode;
				
			item.ResponseMessage = ResponseMessage;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
			item.UniqueID = UniqueID;
				
	        item.Save(UserName);
	    }
    }
}
