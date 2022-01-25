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
    /// Controller class for CashDeviceInfo
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CashDeviceInfoController
    {
        // Preload our schema..
        CashDeviceInfo thisSchemaLoad = new CashDeviceInfo();
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
        public CashDeviceInfoCollection FetchAll()
        {
            CashDeviceInfoCollection coll = new CashDeviceInfoCollection();
            Query qry = new Query(CashDeviceInfo.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CashDeviceInfoCollection FetchByID(object CashDeviceInfoID)
        {
            CashDeviceInfoCollection coll = new CashDeviceInfoCollection().Where("CashDeviceInfoID", CashDeviceInfoID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CashDeviceInfoCollection FetchByQuery(Query qry)
        {
            CashDeviceInfoCollection coll = new CashDeviceInfoCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object CashDeviceInfoID)
        {
            return (CashDeviceInfo.Delete(CashDeviceInfoID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object CashDeviceInfoID)
        {
            return (CashDeviceInfo.Destroy(CashDeviceInfoID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string DeviceID,string DeviceName,string CasseteID,string CasseteName,string Denomination,int Count,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,string DeviceType)
	    {
		    CashDeviceInfo item = new CashDeviceInfo();
		    
            item.DeviceID = DeviceID;
            
            item.DeviceName = DeviceName;
            
            item.CasseteID = CasseteID;
            
            item.CasseteName = CasseteName;
            
            item.Denomination = Denomination;
            
            item.Count = Count;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.Userfld1 = Userfld1;
            
            item.Userfld2 = Userfld2;
            
            item.Userfld3 = Userfld3;
            
            item.Userfld4 = Userfld4;
            
            item.Userfld5 = Userfld5;
            
            item.Userfld6 = Userfld6;
            
            item.Userfld7 = Userfld7;
            
            item.Userfld8 = Userfld8;
            
            item.Userfld9 = Userfld9;
            
            item.Userfld10 = Userfld10;
            
            item.DeviceType = DeviceType;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int CashDeviceInfoID,string DeviceID,string DeviceName,string CasseteID,string CasseteName,string Denomination,int Count,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,string DeviceType)
	    {
		    CashDeviceInfo item = new CashDeviceInfo();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.CashDeviceInfoID = CashDeviceInfoID;
				
			item.DeviceID = DeviceID;
				
			item.DeviceName = DeviceName;
				
			item.CasseteID = CasseteID;
				
			item.CasseteName = CasseteName;
				
			item.Denomination = Denomination;
				
			item.Count = Count;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.Userfld1 = Userfld1;
				
			item.Userfld2 = Userfld2;
				
			item.Userfld3 = Userfld3;
				
			item.Userfld4 = Userfld4;
				
			item.Userfld5 = Userfld5;
				
			item.Userfld6 = Userfld6;
				
			item.Userfld7 = Userfld7;
				
			item.Userfld8 = Userfld8;
				
			item.Userfld9 = Userfld9;
				
			item.Userfld10 = Userfld10;
				
			item.DeviceType = DeviceType;
				
	        item.Save(UserName);
	    }
    }
}
