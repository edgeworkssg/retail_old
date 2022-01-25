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
    /// Controller class for ReceiptSetting
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ReceiptSettingController
    {
        // Preload our schema..
        ReceiptSetting thisSchemaLoad = new ReceiptSetting();
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
        public ReceiptSettingCollection FetchAll()
        {
            ReceiptSettingCollection coll = new ReceiptSettingCollection();
            Query qry = new Query(ReceiptSetting.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ReceiptSettingCollection FetchByID(object SettingId)
        {
            ReceiptSettingCollection coll = new ReceiptSettingCollection().Where("settingId", SettingId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ReceiptSettingCollection FetchByQuery(Query qry)
        {
            ReceiptSettingCollection coll = new ReceiptSettingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object SettingId)
        {
            return (ReceiptSetting.Delete(SettingId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object SettingId)
        {
            return (ReceiptSetting.Destroy(SettingId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid SettingId,string SettingRemark,bool PrintReceipt,bool UseOutletAddress,string ReceiptAddress1,string ReceiptAddress2,string ReceiptAddress3,string ReceiptAddress4,bool ShowMembershipInfo,bool ShowSalesPersonInfo,string SalesPersonTitle,string TermCondition1,string TermCondition2,string TermCondition3,string TermCondition4,string TermCondition5,string TermCondition6,int? NumOfCopies,int? PaperSize,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    ReceiptSetting item = new ReceiptSetting();
		    
            item.SettingId = SettingId;
            
            item.SettingRemark = SettingRemark;
            
            item.PrintReceipt = PrintReceipt;
            
            item.UseOutletAddress = UseOutletAddress;
            
            item.ReceiptAddress1 = ReceiptAddress1;
            
            item.ReceiptAddress2 = ReceiptAddress2;
            
            item.ReceiptAddress3 = ReceiptAddress3;
            
            item.ReceiptAddress4 = ReceiptAddress4;
            
            item.ShowMembershipInfo = ShowMembershipInfo;
            
            item.ShowSalesPersonInfo = ShowSalesPersonInfo;
            
            item.SalesPersonTitle = SalesPersonTitle;
            
            item.TermCondition1 = TermCondition1;
            
            item.TermCondition2 = TermCondition2;
            
            item.TermCondition3 = TermCondition3;
            
            item.TermCondition4 = TermCondition4;
            
            item.TermCondition5 = TermCondition5;
            
            item.TermCondition6 = TermCondition6;
            
            item.NumOfCopies = NumOfCopies;
            
            item.PaperSize = PaperSize;
            
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
	    public void Update(Guid SettingId,string SettingRemark,bool PrintReceipt,bool UseOutletAddress,string ReceiptAddress1,string ReceiptAddress2,string ReceiptAddress3,string ReceiptAddress4,bool ShowMembershipInfo,bool ShowSalesPersonInfo,string SalesPersonTitle,string TermCondition1,string TermCondition2,string TermCondition3,string TermCondition4,string TermCondition5,string TermCondition6,int? NumOfCopies,int? PaperSize,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    ReceiptSetting item = new ReceiptSetting();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.SettingId = SettingId;
				
			item.SettingRemark = SettingRemark;
				
			item.PrintReceipt = PrintReceipt;
				
			item.UseOutletAddress = UseOutletAddress;
				
			item.ReceiptAddress1 = ReceiptAddress1;
				
			item.ReceiptAddress2 = ReceiptAddress2;
				
			item.ReceiptAddress3 = ReceiptAddress3;
				
			item.ReceiptAddress4 = ReceiptAddress4;
				
			item.ShowMembershipInfo = ShowMembershipInfo;
				
			item.ShowSalesPersonInfo = ShowSalesPersonInfo;
				
			item.SalesPersonTitle = SalesPersonTitle;
				
			item.TermCondition1 = TermCondition1;
				
			item.TermCondition2 = TermCondition2;
				
			item.TermCondition3 = TermCondition3;
				
			item.TermCondition4 = TermCondition4;
				
			item.TermCondition5 = TermCondition5;
				
			item.TermCondition6 = TermCondition6;
				
			item.NumOfCopies = NumOfCopies;
				
			item.PaperSize = PaperSize;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
