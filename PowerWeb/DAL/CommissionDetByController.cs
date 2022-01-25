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
    /// Controller class for CommissionDetBy
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CommissionDetByController
    {
        // Preload our schema..
        CommissionDetBy thisSchemaLoad = new CommissionDetBy();
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
        public CommissionDetByCollection FetchAll()
        {
            CommissionDetByCollection coll = new CommissionDetByCollection();
            Query qry = new Query(CommissionDetBy.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CommissionDetByCollection FetchByID(object CommissionDetByID)
        {
            CommissionDetByCollection coll = new CommissionDetByCollection().Where("CommissionDetByID", CommissionDetByID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CommissionDetByCollection FetchByQuery(Query qry)
        {
            CommissionDetByCollection coll = new CommissionDetByCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object CommissionDetByID)
        {
            return (CommissionDetBy.Delete(CommissionDetByID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object CommissionDetByID)
        {
            return (CommissionDetBy.Destroy(CommissionDetByID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int? CommissionHdrID,decimal? From,decimal? ToX,decimal? ValueX)
	    {
		    CommissionDetBy item = new CommissionDetBy();
		    
            item.CommissionHdrID = CommissionHdrID;
            
            item.From = From;
            
            item.ToX = ToX;
            
            item.ValueX = ValueX;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int CommissionDetByID,int? CommissionHdrID,decimal? From,decimal? ToX,decimal? ValueX)
	    {
		    CommissionDetBy item = new CommissionDetBy();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.CommissionDetByID = CommissionDetByID;
				
			item.CommissionHdrID = CommissionHdrID;
				
			item.From = From;
				
			item.ToX = ToX;
				
			item.ValueX = ValueX;
				
	        item.Save(UserName);
	    }
    }
}
