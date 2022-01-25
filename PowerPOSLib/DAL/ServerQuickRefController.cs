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
    /// Controller class for ServerQuickRef
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ServerQuickRefController
    {
        // Preload our schema..
        ServerQuickRef thisSchemaLoad = new ServerQuickRef();
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
        public ServerQuickRefCollection FetchAll()
        {
            ServerQuickRefCollection coll = new ServerQuickRefCollection();
            Query qry = new Query(ServerQuickRef.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ServerQuickRefCollection FetchByID(object RefID)
        {
            ServerQuickRefCollection coll = new ServerQuickRefCollection().Where("RefID", RefID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ServerQuickRefCollection FetchByQuery(Query qry)
        {
            ServerQuickRefCollection coll = new ServerQuickRefCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object RefID)
        {
            return (ServerQuickRef.Delete(RefID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object RefID)
        {
            return (ServerQuickRef.Destroy(RefID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string TableName,DateTime LastModifiedon,int? PointOfSaleID,string Outlet,int? InventoryLocationID)
	    {
		    ServerQuickRef item = new ServerQuickRef();
		    
            item.TableName = TableName;
            
            item.LastModifiedon = LastModifiedon;
            
            item.PointOfSaleID = PointOfSaleID;
            
            item.Outlet = Outlet;
            
            item.InventoryLocationID = InventoryLocationID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int RefID,string TableName,DateTime LastModifiedon,int? PointOfSaleID,string Outlet,int? InventoryLocationID)
	    {
		    ServerQuickRef item = new ServerQuickRef();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.RefID = RefID;
				
			item.TableName = TableName;
				
			item.LastModifiedon = LastModifiedon;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.Outlet = Outlet;
				
			item.InventoryLocationID = InventoryLocationID;
				
	        item.Save(UserName);
	    }
    }
}
