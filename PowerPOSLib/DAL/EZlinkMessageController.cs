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
    /// Controller class for EZlinkMessage
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EZlinkMessageController
    {
        // Preload our schema..
        EZlinkMessage thisSchemaLoad = new EZlinkMessage();
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
        public EZlinkMessageCollection FetchAll()
        {
            EZlinkMessageCollection coll = new EZlinkMessageCollection();
            Query qry = new Query(EZlinkMessage.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EZlinkMessageCollection FetchByID(object MsgName)
        {
            EZlinkMessageCollection coll = new EZlinkMessageCollection().Where("MsgName", MsgName).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EZlinkMessageCollection FetchByQuery(Query qry)
        {
            EZlinkMessageCollection coll = new EZlinkMessageCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object MsgName)
        {
            return (EZlinkMessage.Delete(MsgName) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object MsgName)
        {
            return (EZlinkMessage.Destroy(MsgName) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string MsgName,string MsgContent,bool Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid UniqueID)
	    {
		    EZlinkMessage item = new EZlinkMessage();
		    
            item.MsgName = MsgName;
            
            item.MsgContent = MsgContent;
            
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
	    public void Update(string MsgName,string MsgContent,bool Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid UniqueID)
	    {
		    EZlinkMessage item = new EZlinkMessage();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.MsgName = MsgName;
				
			item.MsgContent = MsgContent;
				
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
