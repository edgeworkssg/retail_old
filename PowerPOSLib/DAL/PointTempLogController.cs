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
    /// Controller class for PointTempLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PointTempLogController
    {
        // Preload our schema..
        PointTempLog thisSchemaLoad = new PointTempLog();
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
        public PointTempLogCollection FetchAll()
        {
            PointTempLogCollection coll = new PointTempLogCollection();
            Query qry = new Query(PointTempLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PointTempLogCollection FetchByID(object TempID)
        {
            PointTempLogCollection coll = new PointTempLogCollection().Where("TempID", TempID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PointTempLogCollection FetchByQuery(Query qry)
        {
            PointTempLogCollection coll = new PointTempLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object TempID)
        {
            return (PointTempLog.Delete(TempID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object TempID)
        {
            return (PointTempLog.Destroy(TempID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string OrderHdrID,string MembershipNo,decimal PointAllocated,string RefNo,string PointType,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy)
	    {
		    PointTempLog item = new PointTempLog();
		    
            item.OrderHdrID = OrderHdrID;
            
            item.MembershipNo = MembershipNo;
            
            item.PointAllocated = PointAllocated;
            
            item.RefNo = RefNo;
            
            item.PointType = PointType;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedBy = ModifiedBy;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int TempID,string OrderHdrID,string MembershipNo,decimal PointAllocated,string RefNo,string PointType,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy)
	    {
		    PointTempLog item = new PointTempLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.TempID = TempID;
				
			item.OrderHdrID = OrderHdrID;
				
			item.MembershipNo = MembershipNo;
				
			item.PointAllocated = PointAllocated;
				
			item.RefNo = RefNo;
				
			item.PointType = PointType;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
