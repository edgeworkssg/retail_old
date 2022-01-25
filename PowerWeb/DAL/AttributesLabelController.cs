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
    /// Controller class for AttributesLabel
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AttributesLabelController
    {
        // Preload our schema..
        AttributesLabel thisSchemaLoad = new AttributesLabel();
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
        public AttributesLabelCollection FetchAll()
        {
            AttributesLabelCollection coll = new AttributesLabelCollection();
            Query qry = new Query(AttributesLabel.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AttributesLabelCollection FetchByID(object AttributesNo)
        {
            AttributesLabelCollection coll = new AttributesLabelCollection().Where("AttributesNo", AttributesNo).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AttributesLabelCollection FetchByQuery(Query qry)
        {
            AttributesLabelCollection coll = new AttributesLabelCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object AttributesNo)
        {
            return (AttributesLabel.Delete(AttributesNo) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object AttributesNo)
        {
            return (AttributesLabel.Destroy(AttributesNo) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int AttributesNo,string Label)
	    {
		    AttributesLabel item = new AttributesLabel();
		    
            item.AttributesNo = AttributesNo;
            
            item.Label = Label;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int AttributesNo,string Label)
	    {
		    AttributesLabel item = new AttributesLabel();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.AttributesNo = AttributesNo;
				
			item.Label = Label;
				
	        item.Save(UserName);
	    }
    }
}
