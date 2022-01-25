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
// <auto-generated />
namespace PowerPOS
{
    /// <summary>
    /// Controller class for OrderDetAttributes
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class OrderDetAttributeController
    {
        // Preload our schema..
        OrderDetAttribute thisSchemaLoad = new OrderDetAttribute();
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
        public OrderDetAttributeCollection FetchAll()
        {
            OrderDetAttributeCollection coll = new OrderDetAttributeCollection();
            Query qry = new Query(OrderDetAttribute.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public OrderDetAttributeCollection FetchByID(object OrderDetAttributesID)
        {
            OrderDetAttributeCollection coll = new OrderDetAttributeCollection().Where("OrderDetAttributesID", OrderDetAttributesID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public OrderDetAttributeCollection FetchByQuery(Query qry)
        {
            OrderDetAttributeCollection coll = new OrderDetAttributeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object OrderDetAttributesID)
        {
            return (OrderDetAttribute.Delete(OrderDetAttributesID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object OrderDetAttributesID)
        {
            return (OrderDetAttribute.Destroy(OrderDetAttributesID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string OrderDetAttributesID,string OrderDetID,string AttributesCode,string UserField1,string UserField2,string UserField3,string UserField4,string UserField5,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    OrderDetAttribute item = new OrderDetAttribute();
		    
            item.OrderDetAttributesID = OrderDetAttributesID;
            
            item.OrderDetID = OrderDetID;
            
            item.AttributesCode = AttributesCode;
            
            item.UserField1 = UserField1;
            
            item.UserField2 = UserField2;
            
            item.UserField3 = UserField3;
            
            item.UserField4 = UserField4;
            
            item.UserField5 = UserField5;
            
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
	    public void Update(string OrderDetAttributesID,string OrderDetID,string AttributesCode,string UserField1,string UserField2,string UserField3,string UserField4,string UserField5,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    OrderDetAttribute item = new OrderDetAttribute();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.OrderDetAttributesID = OrderDetAttributesID;
				
			item.OrderDetID = OrderDetID;
				
			item.AttributesCode = AttributesCode;
				
			item.UserField1 = UserField1;
				
			item.UserField2 = UserField2;
				
			item.UserField3 = UserField3;
				
			item.UserField4 = UserField4;
				
			item.UserField5 = UserField5;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
