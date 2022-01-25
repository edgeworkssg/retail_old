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
    /// Controller class for CommissionBasedOnPercentage
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CommissionBasedOnPercentageController
    {
        // Preload our schema..
        CommissionBasedOnPercentage thisSchemaLoad = new CommissionBasedOnPercentage();
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
        public CommissionBasedOnPercentageCollection FetchAll()
        {
            CommissionBasedOnPercentageCollection coll = new CommissionBasedOnPercentageCollection();
            Query qry = new Query(CommissionBasedOnPercentage.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CommissionBasedOnPercentageCollection FetchByID(object UniqueID)
        {
            CommissionBasedOnPercentageCollection coll = new CommissionBasedOnPercentageCollection().Where("UniqueID", UniqueID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CommissionBasedOnPercentageCollection FetchByQuery(Query qry)
        {
            CommissionBasedOnPercentageCollection coll = new CommissionBasedOnPercentageCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object UniqueID)
        {
            return (CommissionBasedOnPercentage.Delete(UniqueID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object UniqueID)
        {
            return (CommissionBasedOnPercentage.Destroy(UniqueID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int SalesGroupID,string CommissionType,decimal? LowerLimit,decimal? UpperLimit,decimal PercentCommission,string CreatedBy,DateTime CreatedOn,string ModifiedBy,DateTime? ModifiedOn)
	    {
		    CommissionBasedOnPercentage item = new CommissionBasedOnPercentage();
		    
            item.SalesGroupID = SalesGroupID;
            
            item.CommissionType = CommissionType;
            
            item.LowerLimit = LowerLimit;
            
            item.UpperLimit = UpperLimit;
            
            item.PercentCommission = PercentCommission;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int UniqueID,int SalesGroupID,string CommissionType,decimal? LowerLimit,decimal? UpperLimit,decimal PercentCommission,string CreatedBy,DateTime CreatedOn,string ModifiedBy,DateTime? ModifiedOn)
	    {
		    CommissionBasedOnPercentage item = new CommissionBasedOnPercentage();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.UniqueID = UniqueID;
				
			item.SalesGroupID = SalesGroupID;
				
			item.CommissionType = CommissionType;
				
			item.LowerLimit = LowerLimit;
				
			item.UpperLimit = UpperLimit;
				
			item.PercentCommission = PercentCommission;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
	        item.Save(UserName);
	    }
    }
}
