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
    /// Controller class for RatingFeedback
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RatingFeedbackController
    {
        // Preload our schema..
        RatingFeedback thisSchemaLoad = new RatingFeedback();
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
        public RatingFeedbackCollection FetchAll()
        {
            RatingFeedbackCollection coll = new RatingFeedbackCollection();
            Query qry = new Query(RatingFeedback.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RatingFeedbackCollection FetchByID(object RatingFeedbackID)
        {
            RatingFeedbackCollection coll = new RatingFeedbackCollection().Where("RatingFeedbackID", RatingFeedbackID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RatingFeedbackCollection FetchByQuery(Query qry)
        {
            RatingFeedbackCollection coll = new RatingFeedbackCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object RatingFeedbackID)
        {
            return (RatingFeedback.Delete(RatingFeedbackID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object RatingFeedbackID)
        {
            return (RatingFeedback.Destroy(RatingFeedbackID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string SelectionText,byte[] SelectionImage,string RatingType,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string SelectionImageUrl)
	    {
		    RatingFeedback item = new RatingFeedback();
		    
            item.SelectionText = SelectionText;
            
            item.SelectionImage = SelectionImage;
            
            item.RatingType = RatingType;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.SelectionImageUrl = SelectionImageUrl;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int RatingFeedbackID,string SelectionText,byte[] SelectionImage,string RatingType,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string SelectionImageUrl)
	    {
		    RatingFeedback item = new RatingFeedback();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.RatingFeedbackID = RatingFeedbackID;
				
			item.SelectionText = SelectionText;
				
			item.SelectionImage = SelectionImage;
				
			item.RatingType = RatingType;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.SelectionImageUrl = SelectionImageUrl;
				
	        item.Save(UserName);
	    }
    }
}
