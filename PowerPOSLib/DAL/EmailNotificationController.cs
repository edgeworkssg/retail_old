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
    /// Controller class for EmailNotification
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EmailNotificationController
    {
        // Preload our schema..
        EmailNotification thisSchemaLoad = new EmailNotification();
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
        public EmailNotificationCollection FetchAll()
        {
            EmailNotificationCollection coll = new EmailNotificationCollection();
            Query qry = new Query(EmailNotification.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EmailNotificationCollection FetchByID(object EmailAddress)
        {
            EmailNotificationCollection coll = new EmailNotificationCollection().Where("EmailAddress", EmailAddress).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EmailNotificationCollection FetchByQuery(Query qry)
        {
            EmailNotificationCollection coll = new EmailNotificationCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object EmailAddress)
        {
            return (EmailNotification.Delete(EmailAddress) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object EmailAddress)
        {
            return (EmailNotification.Destroy(EmailAddress) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string EmailAddress,string Name,string ModuleX,bool? Deleted)
	    {
		    EmailNotification item = new EmailNotification();
		    
            item.EmailAddress = EmailAddress;
            
            item.Name = Name;
            
            item.ModuleX = ModuleX;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string EmailAddress,string Name,string ModuleX,bool? Deleted)
	    {
		    EmailNotification item = new EmailNotification();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.EmailAddress = EmailAddress;
				
			item.Name = Name;
				
			item.ModuleX = ModuleX;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
