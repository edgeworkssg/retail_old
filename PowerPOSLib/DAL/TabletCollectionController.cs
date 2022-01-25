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
    /// Controller class for TabletCollection
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TabletCollectionController
    {
        // Preload our schema..
        TabletCollection thisSchemaLoad = new TabletCollection();
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
        public TabletCollectionCollection FetchAll()
        {
            TabletCollectionCollection coll = new TabletCollectionCollection();
            Query qry = new Query(TabletCollection.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TabletCollectionCollection FetchByID(object TabletCollectionID)
        {
            TabletCollectionCollection coll = new TabletCollectionCollection().Where("TabletCollectionID", TabletCollectionID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TabletCollectionCollection FetchByQuery(Query qry)
        {
            TabletCollectionCollection coll = new TabletCollectionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object TabletCollectionID)
        {
            return (TabletCollection.Delete(TabletCollectionID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object TabletCollectionID)
        {
            return (TabletCollection.Destroy(TabletCollectionID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int PointOfSaleID,int TerminalID,string Status,DateTime? OpenTime,DateTime? CloseTime,string OpenBy,string CloseBy,string Supervisor,decimal? CashRecorded,decimal? Pay1Recorded,decimal? Pay2Recorded,decimal? Pay3Recorded,decimal? Pay4Recorded,decimal? Pay5Recorded,decimal? Pay6Recorded,decimal? Pay7Recorded,decimal? Pay8Recorded,decimal? Pay9Recorded,decimal? Pay10Recorded,decimal? VoucherRecorded,decimal? ChequeRecorded,decimal? PointRecorded,decimal? PackageRecorded,decimal? SMFRecorded,decimal? PAMedRecorded,decimal? PWFRecorded,decimal? NetsATMCardRecorded,decimal? NetsCashCardRecorded,decimal? NetsFlashPayRecorded,string ForeignCurrency1,decimal? ForeignCurrency1Recorded,string ForeignCurrency2,decimal? ForeignCurrency2Recorded,string ForeignCurrency3,decimal? ForeignCurrency3Recorded,string ForeignCurrency4,decimal? ForeignCurrency4Recorded,string ForeignCurrency5,decimal? ForeignCurrency5Recorded,decimal? TotalForeignCurrency,DateTime CreatedOn,string CreatedBy,DateTime ModifiedOn,string ModifiedBy,Guid UniqueID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5)
	    {
		    TabletCollection item = new TabletCollection();
		    
            item.PointOfSaleID = PointOfSaleID;
            
            item.TerminalID = TerminalID;
            
            item.Status = Status;
            
            item.OpenTime = OpenTime;
            
            item.CloseTime = CloseTime;
            
            item.OpenBy = OpenBy;
            
            item.CloseBy = CloseBy;
            
            item.Supervisor = Supervisor;
            
            item.CashRecorded = CashRecorded;
            
            item.Pay1Recorded = Pay1Recorded;
            
            item.Pay2Recorded = Pay2Recorded;
            
            item.Pay3Recorded = Pay3Recorded;
            
            item.Pay4Recorded = Pay4Recorded;
            
            item.Pay5Recorded = Pay5Recorded;
            
            item.Pay6Recorded = Pay6Recorded;
            
            item.Pay7Recorded = Pay7Recorded;
            
            item.Pay8Recorded = Pay8Recorded;
            
            item.Pay9Recorded = Pay9Recorded;
            
            item.Pay10Recorded = Pay10Recorded;
            
            item.VoucherRecorded = VoucherRecorded;
            
            item.ChequeRecorded = ChequeRecorded;
            
            item.PointRecorded = PointRecorded;
            
            item.PackageRecorded = PackageRecorded;
            
            item.SMFRecorded = SMFRecorded;
            
            item.PAMedRecorded = PAMedRecorded;
            
            item.PWFRecorded = PWFRecorded;
            
            item.NetsATMCardRecorded = NetsATMCardRecorded;
            
            item.NetsCashCardRecorded = NetsCashCardRecorded;
            
            item.NetsFlashPayRecorded = NetsFlashPayRecorded;
            
            item.ForeignCurrency1 = ForeignCurrency1;
            
            item.ForeignCurrency1Recorded = ForeignCurrency1Recorded;
            
            item.ForeignCurrency2 = ForeignCurrency2;
            
            item.ForeignCurrency2Recorded = ForeignCurrency2Recorded;
            
            item.ForeignCurrency3 = ForeignCurrency3;
            
            item.ForeignCurrency3Recorded = ForeignCurrency3Recorded;
            
            item.ForeignCurrency4 = ForeignCurrency4;
            
            item.ForeignCurrency4Recorded = ForeignCurrency4Recorded;
            
            item.ForeignCurrency5 = ForeignCurrency5;
            
            item.ForeignCurrency5Recorded = ForeignCurrency5Recorded;
            
            item.TotalForeignCurrency = TotalForeignCurrency;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.UniqueID = UniqueID;
            
            item.Userfld1 = Userfld1;
            
            item.Userfld2 = Userfld2;
            
            item.Userfld3 = Userfld3;
            
            item.Userfld4 = Userfld4;
            
            item.Userfld5 = Userfld5;
            
            item.Userfld6 = Userfld6;
            
            item.Userfld7 = Userfld7;
            
            item.Userfld8 = Userfld8;
            
            item.Userfld9 = Userfld9;
            
            item.Userfld10 = Userfld10;
            
            item.Userflag1 = Userflag1;
            
            item.Userflag2 = Userflag2;
            
            item.Userflag3 = Userflag3;
            
            item.Userflag4 = Userflag4;
            
            item.Userflag5 = Userflag5;
            
            item.Userfloat1 = Userfloat1;
            
            item.Userfloat2 = Userfloat2;
            
            item.Userfloat3 = Userfloat3;
            
            item.Userfloat4 = Userfloat4;
            
            item.Userfloat5 = Userfloat5;
            
            item.Userint1 = Userint1;
            
            item.Userint2 = Userint2;
            
            item.Userint3 = Userint3;
            
            item.Userint4 = Userint4;
            
            item.Userint5 = Userint5;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int TabletCollectionID,int PointOfSaleID,int TerminalID,string Status,DateTime? OpenTime,DateTime? CloseTime,string OpenBy,string CloseBy,string Supervisor,decimal? CashRecorded,decimal? Pay1Recorded,decimal? Pay2Recorded,decimal? Pay3Recorded,decimal? Pay4Recorded,decimal? Pay5Recorded,decimal? Pay6Recorded,decimal? Pay7Recorded,decimal? Pay8Recorded,decimal? Pay9Recorded,decimal? Pay10Recorded,decimal? VoucherRecorded,decimal? ChequeRecorded,decimal? PointRecorded,decimal? PackageRecorded,decimal? SMFRecorded,decimal? PAMedRecorded,decimal? PWFRecorded,decimal? NetsATMCardRecorded,decimal? NetsCashCardRecorded,decimal? NetsFlashPayRecorded,string ForeignCurrency1,decimal? ForeignCurrency1Recorded,string ForeignCurrency2,decimal? ForeignCurrency2Recorded,string ForeignCurrency3,decimal? ForeignCurrency3Recorded,string ForeignCurrency4,decimal? ForeignCurrency4Recorded,string ForeignCurrency5,decimal? ForeignCurrency5Recorded,decimal? TotalForeignCurrency,DateTime CreatedOn,string CreatedBy,DateTime ModifiedOn,string ModifiedBy,Guid UniqueID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5)
	    {
		    TabletCollection item = new TabletCollection();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.TabletCollectionID = TabletCollectionID;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.TerminalID = TerminalID;
				
			item.Status = Status;
				
			item.OpenTime = OpenTime;
				
			item.CloseTime = CloseTime;
				
			item.OpenBy = OpenBy;
				
			item.CloseBy = CloseBy;
				
			item.Supervisor = Supervisor;
				
			item.CashRecorded = CashRecorded;
				
			item.Pay1Recorded = Pay1Recorded;
				
			item.Pay2Recorded = Pay2Recorded;
				
			item.Pay3Recorded = Pay3Recorded;
				
			item.Pay4Recorded = Pay4Recorded;
				
			item.Pay5Recorded = Pay5Recorded;
				
			item.Pay6Recorded = Pay6Recorded;
				
			item.Pay7Recorded = Pay7Recorded;
				
			item.Pay8Recorded = Pay8Recorded;
				
			item.Pay9Recorded = Pay9Recorded;
				
			item.Pay10Recorded = Pay10Recorded;
				
			item.VoucherRecorded = VoucherRecorded;
				
			item.ChequeRecorded = ChequeRecorded;
				
			item.PointRecorded = PointRecorded;
				
			item.PackageRecorded = PackageRecorded;
				
			item.SMFRecorded = SMFRecorded;
				
			item.PAMedRecorded = PAMedRecorded;
				
			item.PWFRecorded = PWFRecorded;
				
			item.NetsATMCardRecorded = NetsATMCardRecorded;
				
			item.NetsCashCardRecorded = NetsCashCardRecorded;
				
			item.NetsFlashPayRecorded = NetsFlashPayRecorded;
				
			item.ForeignCurrency1 = ForeignCurrency1;
				
			item.ForeignCurrency1Recorded = ForeignCurrency1Recorded;
				
			item.ForeignCurrency2 = ForeignCurrency2;
				
			item.ForeignCurrency2Recorded = ForeignCurrency2Recorded;
				
			item.ForeignCurrency3 = ForeignCurrency3;
				
			item.ForeignCurrency3Recorded = ForeignCurrency3Recorded;
				
			item.ForeignCurrency4 = ForeignCurrency4;
				
			item.ForeignCurrency4Recorded = ForeignCurrency4Recorded;
				
			item.ForeignCurrency5 = ForeignCurrency5;
				
			item.ForeignCurrency5Recorded = ForeignCurrency5Recorded;
				
			item.TotalForeignCurrency = TotalForeignCurrency;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.UniqueID = UniqueID;
				
			item.Userfld1 = Userfld1;
				
			item.Userfld2 = Userfld2;
				
			item.Userfld3 = Userfld3;
				
			item.Userfld4 = Userfld4;
				
			item.Userfld5 = Userfld5;
				
			item.Userfld6 = Userfld6;
				
			item.Userfld7 = Userfld7;
				
			item.Userfld8 = Userfld8;
				
			item.Userfld9 = Userfld9;
				
			item.Userfld10 = Userfld10;
				
			item.Userflag1 = Userflag1;
				
			item.Userflag2 = Userflag2;
				
			item.Userflag3 = Userflag3;
				
			item.Userflag4 = Userflag4;
				
			item.Userflag5 = Userflag5;
				
			item.Userfloat1 = Userfloat1;
				
			item.Userfloat2 = Userfloat2;
				
			item.Userfloat3 = Userfloat3;
				
			item.Userfloat4 = Userfloat4;
				
			item.Userfloat5 = Userfloat5;
				
			item.Userint1 = Userint1;
				
			item.Userint2 = Userint2;
				
			item.Userint3 = Userint3;
				
			item.Userint4 = Userint4;
				
			item.Userint5 = Userint5;
				
	        item.Save(UserName);
	    }
    }
}
