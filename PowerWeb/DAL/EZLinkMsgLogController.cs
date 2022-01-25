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
    /// Controller class for EZLinkMsgLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EZLinkMsgLogController
    {
        // Preload our schema..
        EZLinkMsgLog thisSchemaLoad = new EZLinkMsgLog();
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
        public EZLinkMsgLogCollection FetchAll()
        {
            EZLinkMsgLogCollection coll = new EZLinkMsgLogCollection();
            Query qry = new Query(EZLinkMsgLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EZLinkMsgLogCollection FetchByID(object MsgLogID)
        {
            EZLinkMsgLogCollection coll = new EZLinkMsgLogCollection().Where("msgLogID", MsgLogID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EZLinkMsgLogCollection FetchByQuery(Query qry)
        {
            EZLinkMsgLogCollection coll = new EZLinkMsgLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object MsgLogID)
        {
            return (EZLinkMsgLog.Delete(MsgLogID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object MsgLogID)
        {
            return (EZLinkMsgLog.Destroy(MsgLogID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime MsgDate,string MsgContent,bool Deleted,Guid UniqueID)
	    {
		    EZLinkMsgLog item = new EZLinkMsgLog();
		    
            item.MsgDate = MsgDate;
            
            item.MsgContent = MsgContent;
            
            item.Deleted = Deleted;
            
            item.UniqueID = UniqueID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int MsgLogID,DateTime MsgDate,string MsgContent,bool Deleted,Guid UniqueID)
	    {
		    EZLinkMsgLog item = new EZLinkMsgLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.MsgLogID = MsgLogID;
				
			item.MsgDate = MsgDate;
				
			item.MsgContent = MsgContent;
				
			item.Deleted = Deleted;
				
			item.UniqueID = UniqueID;
				
	        item.Save(UserName);
	    }
    }
}
