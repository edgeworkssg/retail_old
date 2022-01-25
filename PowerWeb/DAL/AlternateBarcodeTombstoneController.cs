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
    /// Controller class for AlternateBarcode_Tombstone
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AlternateBarcodeTombstoneController
    {
        // Preload our schema..
        AlternateBarcodeTombstone thisSchemaLoad = new AlternateBarcodeTombstone();
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
        public AlternateBarcodeTombstoneCollection FetchAll()
        {
            AlternateBarcodeTombstoneCollection coll = new AlternateBarcodeTombstoneCollection();
            Query qry = new Query(AlternateBarcodeTombstone.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AlternateBarcodeTombstoneCollection FetchByID(object BarcodeID)
        {
            AlternateBarcodeTombstoneCollection coll = new AlternateBarcodeTombstoneCollection().Where("BarcodeID", BarcodeID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AlternateBarcodeTombstoneCollection FetchByQuery(Query qry)
        {
            AlternateBarcodeTombstoneCollection coll = new AlternateBarcodeTombstoneCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object BarcodeID)
        {
            return (AlternateBarcodeTombstone.Delete(BarcodeID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object BarcodeID)
        {
            return (AlternateBarcodeTombstone.Destroy(BarcodeID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int BarcodeID,DateTime? DeletionDate)
	    {
		    AlternateBarcodeTombstone item = new AlternateBarcodeTombstone();
		    
            item.BarcodeID = BarcodeID;
            
            item.DeletionDate = DeletionDate;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int BarcodeID,DateTime? DeletionDate)
	    {
		    AlternateBarcodeTombstone item = new AlternateBarcodeTombstone();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.BarcodeID = BarcodeID;
				
			item.DeletionDate = DeletionDate;
				
	        item.Save(UserName);
	    }
    }
}
