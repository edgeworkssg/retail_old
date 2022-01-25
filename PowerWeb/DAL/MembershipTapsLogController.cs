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
    /// Controller class for MembershipTapsLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MembershipTapsLogController
    {
        // Preload our schema..
        MembershipTapsLog thisSchemaLoad = new MembershipTapsLog();
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
        public MembershipTapsLogCollection FetchAll()
        {
            MembershipTapsLogCollection coll = new MembershipTapsLogCollection();
            Query qry = new Query(MembershipTapsLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipTapsLogCollection FetchByID(object TapsLogID)
        {
            MembershipTapsLogCollection coll = new MembershipTapsLogCollection().Where("TapsLogID", TapsLogID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipTapsLogCollection FetchByQuery(Query qry)
        {
            MembershipTapsLogCollection coll = new MembershipTapsLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object TapsLogID)
        {
            return (MembershipTapsLog.Delete(TapsLogID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object TapsLogID)
        {
            return (MembershipTapsLog.Destroy(TapsLogID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string MembershipNo,DateTime ActivityDate,string UserName,decimal? Amount,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    MembershipTapsLog item = new MembershipTapsLog();
		    
            item.MembershipNo = MembershipNo;
            
            item.ActivityDate = ActivityDate;
            
            item.UserName = UserName;
            
            item.Amount = Amount;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int TapsLogID,string MembershipNo,DateTime ActivityDate,string UserName,decimal? Amount,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    MembershipTapsLog item = new MembershipTapsLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.TapsLogID = TapsLogID;
				
			item.MembershipNo = MembershipNo;
				
			item.ActivityDate = ActivityDate;
				
			item.UserName = UserName;
				
			item.Amount = Amount;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
