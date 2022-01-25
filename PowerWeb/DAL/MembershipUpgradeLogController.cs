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
    /// Controller class for MembershipUpgradeLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MembershipUpgradeLogController
    {
        // Preload our schema..
        MembershipUpgradeLog thisSchemaLoad = new MembershipUpgradeLog();
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
        public MembershipUpgradeLogCollection FetchAll()
        {
            MembershipUpgradeLogCollection coll = new MembershipUpgradeLogCollection();
            Query qry = new Query(MembershipUpgradeLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipUpgradeLogCollection FetchByID(object OrderHdrID)
        {
            MembershipUpgradeLogCollection coll = new MembershipUpgradeLogCollection().Where("OrderHdrID", OrderHdrID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipUpgradeLogCollection FetchByQuery(Query qry)
        {
            MembershipUpgradeLogCollection coll = new MembershipUpgradeLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object OrderHdrID)
        {
            return (MembershipUpgradeLog.Delete(OrderHdrID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object OrderHdrID)
        {
            return (MembershipUpgradeLog.Destroy(OrderHdrID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string OrderHdrID,bool? IsVitaMixPrevValue,bool? IsWaterFilterPrevValue,bool? IsJuicePlusPrevValue,bool? IsYoungPrevValue,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted,Guid? UniqueID)
	    {
		    MembershipUpgradeLog item = new MembershipUpgradeLog();
		    
            item.OrderHdrID = OrderHdrID;
            
            item.IsVitaMixPrevValue = IsVitaMixPrevValue;
            
            item.IsWaterFilterPrevValue = IsWaterFilterPrevValue;
            
            item.IsJuicePlusPrevValue = IsJuicePlusPrevValue;
            
            item.IsYoungPrevValue = IsYoungPrevValue;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.UniqueID = UniqueID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string OrderHdrID,bool? IsVitaMixPrevValue,bool? IsWaterFilterPrevValue,bool? IsJuicePlusPrevValue,bool? IsYoungPrevValue,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted,Guid? UniqueID)
	    {
		    MembershipUpgradeLog item = new MembershipUpgradeLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.OrderHdrID = OrderHdrID;
				
			item.IsVitaMixPrevValue = IsVitaMixPrevValue;
				
			item.IsWaterFilterPrevValue = IsWaterFilterPrevValue;
				
			item.IsJuicePlusPrevValue = IsJuicePlusPrevValue;
				
			item.IsYoungPrevValue = IsYoungPrevValue;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.UniqueID = UniqueID;
				
	        item.Save(UserName);
	    }
    }
}
