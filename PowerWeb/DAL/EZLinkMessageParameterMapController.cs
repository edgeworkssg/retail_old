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
    /// Controller class for EZLinkMessageParameterMap
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EZLinkMessageParameterMapController
    {
        // Preload our schema..
        EZLinkMessageParameterMap thisSchemaLoad = new EZLinkMessageParameterMap();
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
        public EZLinkMessageParameterMapCollection FetchAll()
        {
            EZLinkMessageParameterMapCollection coll = new EZLinkMessageParameterMapCollection();
            Query qry = new Query(EZLinkMessageParameterMap.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EZLinkMessageParameterMapCollection FetchByID(object MapID)
        {
            EZLinkMessageParameterMapCollection coll = new EZLinkMessageParameterMapCollection().Where("MapID", MapID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EZLinkMessageParameterMapCollection FetchByQuery(Query qry)
        {
            EZLinkMessageParameterMapCollection coll = new EZLinkMessageParameterMapCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object MapID)
        {
            return (EZLinkMessageParameterMap.Delete(MapID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object MapID)
        {
            return (EZLinkMessageParameterMap.Destroy(MapID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string MsgName,string ParameterName,int StartingIndex,bool Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid UniqueID)
	    {
		    EZLinkMessageParameterMap item = new EZLinkMessageParameterMap();
		    
            item.MsgName = MsgName;
            
            item.ParameterName = ParameterName;
            
            item.StartingIndex = StartingIndex;
            
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
	    public void Update(int MapID,string MsgName,string ParameterName,int StartingIndex,bool Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid UniqueID)
	    {
		    EZLinkMessageParameterMap item = new EZLinkMessageParameterMap();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.MapID = MapID;
				
			item.MsgName = MsgName;
				
			item.ParameterName = ParameterName;
				
			item.StartingIndex = StartingIndex;
				
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
