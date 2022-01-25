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
    /// Controller class for ActivePayment
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ActivePaymentController
    {
        // Preload our schema..
        ActivePayment thisSchemaLoad = new ActivePayment();
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
        public ActivePaymentCollection FetchAll()
        {
            ActivePaymentCollection coll = new ActivePaymentCollection();
            Query qry = new Query(ActivePayment.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ActivePaymentCollection FetchByID(object PaymentID)
        {
            ActivePaymentCollection coll = new ActivePaymentCollection().Where("PaymentID", PaymentID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ActivePaymentCollection FetchByQuery(Query qry)
        {
            ActivePaymentCollection coll = new ActivePaymentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PaymentID)
        {
            return (ActivePayment.Delete(PaymentID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PaymentID)
        {
            return (ActivePayment.Destroy(PaymentID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int PaymentID,string PaymentName,bool IsActive,string Remarks,bool? AllowChange,bool? AllowExtra,bool? ShowRemark1,string LabelRemark1,bool? ShowRemark2,string LabelRemark2,bool? ShowRemark3,string LabelRemark3,bool? ShowRemark4,string LabelRemark4,bool? ShowRemark5,string LabelRemark5,string Userfld1,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool? Deleted)
	    {
		    ActivePayment item = new ActivePayment();
		    
            item.PaymentID = PaymentID;
            
            item.PaymentName = PaymentName;
            
            item.IsActive = IsActive;
            
            item.Remarks = Remarks;
            
            item.AllowChange = AllowChange;
            
            item.AllowExtra = AllowExtra;
            
            item.ShowRemark1 = ShowRemark1;
            
            item.LabelRemark1 = LabelRemark1;
            
            item.ShowRemark2 = ShowRemark2;
            
            item.LabelRemark2 = LabelRemark2;
            
            item.ShowRemark3 = ShowRemark3;
            
            item.LabelRemark3 = LabelRemark3;
            
            item.ShowRemark4 = ShowRemark4;
            
            item.LabelRemark4 = LabelRemark4;
            
            item.ShowRemark5 = ShowRemark5;
            
            item.LabelRemark5 = LabelRemark5;
            
            item.Userfld1 = Userfld1;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int PaymentID,string PaymentName,bool IsActive,string Remarks,bool? AllowChange,bool? AllowExtra,bool? ShowRemark1,string LabelRemark1,bool? ShowRemark2,string LabelRemark2,bool? ShowRemark3,string LabelRemark3,bool? ShowRemark4,string LabelRemark4,bool? ShowRemark5,string LabelRemark5,string Userfld1,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool? Deleted)
	    {
		    ActivePayment item = new ActivePayment();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PaymentID = PaymentID;
				
			item.PaymentName = PaymentName;
				
			item.IsActive = IsActive;
				
			item.Remarks = Remarks;
				
			item.AllowChange = AllowChange;
				
			item.AllowExtra = AllowExtra;
				
			item.ShowRemark1 = ShowRemark1;
				
			item.LabelRemark1 = LabelRemark1;
				
			item.ShowRemark2 = ShowRemark2;
				
			item.LabelRemark2 = LabelRemark2;
				
			item.ShowRemark3 = ShowRemark3;
				
			item.LabelRemark3 = LabelRemark3;
				
			item.ShowRemark4 = ShowRemark4;
				
			item.LabelRemark4 = LabelRemark4;
				
			item.ShowRemark5 = ShowRemark5;
				
			item.LabelRemark5 = LabelRemark5;
				
			item.Userfld1 = Userfld1;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
