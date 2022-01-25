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
    /// Controller class for MembershipCustomFields
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MembershipCustomFieldController
    {
        // Preload our schema..
        MembershipCustomField thisSchemaLoad = new MembershipCustomField();
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
        public MembershipCustomFieldCollection FetchAll()
        {
            MembershipCustomFieldCollection coll = new MembershipCustomFieldCollection();
            Query qry = new Query(MembershipCustomField.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipCustomFieldCollection FetchByID(object MembershipCustomFieldID)
        {
            MembershipCustomFieldCollection coll = new MembershipCustomFieldCollection().Where("MembershipCustomFieldID", MembershipCustomFieldID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipCustomFieldCollection FetchByQuery(Query qry)
        {
            MembershipCustomFieldCollection coll = new MembershipCustomFieldCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object MembershipCustomFieldID)
        {
            return (MembershipCustomField.Delete(MembershipCustomFieldID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object MembershipCustomFieldID)
        {
            return (MembershipCustomField.Destroy(MembershipCustomFieldID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid MembershipCustomFieldID,string FieldName,string Label,string Type,string EnumList,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    MembershipCustomField item = new MembershipCustomField();
		    
            item.MembershipCustomFieldID = MembershipCustomFieldID;
            
            item.FieldName = FieldName;
            
            item.Label = Label;
            
            item.Type = Type;
            
            item.EnumList = EnumList;
            
            item.Deleted = Deleted;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid MembershipCustomFieldID,string FieldName,string Label,string Type,string EnumList,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    MembershipCustomField item = new MembershipCustomField();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.MembershipCustomFieldID = MembershipCustomFieldID;
				
			item.FieldName = FieldName;
				
			item.Label = Label;
				
			item.Type = Type;
				
			item.EnumList = EnumList;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
