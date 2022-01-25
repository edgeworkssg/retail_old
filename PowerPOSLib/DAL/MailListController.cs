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
    /// Controller class for MailList
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MailListController
    {
        // Preload our schema..
        MailList thisSchemaLoad = new MailList();
        private string userName = string.Empty;
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
        public MailListCollection FetchAll()
        {
            MailListCollection coll = new MailListCollection();
            Query qry = new Query(MailList.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailListCollection FetchByID(object MembershipNo)
        {
            MailListCollection coll = new MailListCollection().Where("MembershipNo", MembershipNo).Load();
            return coll;
        }

		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailListCollection FetchByQuery(Query qry)
        {
            MailListCollection coll = new MailListCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object MembershipNo)
        {
            return (MailList.Delete(MembershipNo) == 1);
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object MembershipNo)
        {
            return (MailList.Destroy(MembershipNo) == 1);
        }

        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string MembershipNo,string FirstName,decimal? TotalPoints,decimal? TotalAmount)
	    {
		    MailList item = new MailList();
		    
            item.MembershipNo = MembershipNo;
            
            item.FirstName = FirstName;
            
            item.TotalPoints = TotalPoints;
            
            item.TotalAmount = TotalAmount;
            
	    
		    item.Save(UserName);
	    }

    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string MembershipNo,string FirstName,decimal? TotalPoints,decimal? TotalAmount)
	    {
		    MailList item = new MailList();
		    
				item.MembershipNo = MembershipNo;
				
				item.FirstName = FirstName;
				
				item.TotalPoints = TotalPoints;
				
				item.TotalAmount = TotalAmount;
				
		    item.MarkOld();
		    item.Save(UserName);
	    }

    }

}

