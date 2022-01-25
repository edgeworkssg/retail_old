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
    /// Controller class for AttendanceSheet_Tombstone
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AttendanceSheetTombstoneController
    {
        // Preload our schema..
        AttendanceSheetTombstone thisSchemaLoad = new AttendanceSheetTombstone();
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
        public AttendanceSheetTombstoneCollection FetchAll()
        {
            AttendanceSheetTombstoneCollection coll = new AttendanceSheetTombstoneCollection();
            Query qry = new Query(AttendanceSheetTombstone.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AttendanceSheetTombstoneCollection FetchByID(object AttendanceID)
        {
            AttendanceSheetTombstoneCollection coll = new AttendanceSheetTombstoneCollection().Where("AttendanceID", AttendanceID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AttendanceSheetTombstoneCollection FetchByQuery(Query qry)
        {
            AttendanceSheetTombstoneCollection coll = new AttendanceSheetTombstoneCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object AttendanceID)
        {
            return (AttendanceSheetTombstone.Delete(AttendanceID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object AttendanceID)
        {
            return (AttendanceSheetTombstone.Destroy(AttendanceID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string AttendanceID,DateTime? DeletionDate)
	    {
		    AttendanceSheetTombstone item = new AttendanceSheetTombstone();
		    
            item.AttendanceID = AttendanceID;
            
            item.DeletionDate = DeletionDate;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string AttendanceID,DateTime? DeletionDate)
	    {
		    AttendanceSheetTombstone item = new AttendanceSheetTombstone();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.AttendanceID = AttendanceID;
				
			item.DeletionDate = DeletionDate;
				
	        item.Save(UserName);
	    }
    }
}
