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
    /// Controller class for AppPendingProcess
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AppPendingProcessController
    {
        // Preload our schema..
        AppPendingProcess thisSchemaLoad = new AppPendingProcess();
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
        public AppPendingProcessCollection FetchAll()
        {
            AppPendingProcessCollection coll = new AppPendingProcessCollection();
            Query qry = new Query(AppPendingProcess.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppPendingProcessCollection FetchByID(object AppPendingProcessID)
        {
            AppPendingProcessCollection coll = new AppPendingProcessCollection().Where("AppPendingProcessID", AppPendingProcessID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppPendingProcessCollection FetchByQuery(Query qry)
        {
            AppPendingProcessCollection coll = new AppPendingProcessCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object AppPendingProcessID)
        {
            return (AppPendingProcess.Delete(AppPendingProcessID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object AppPendingProcessID)
        {
            return (AppPendingProcess.Destroy(AppPendingProcessID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid AppPendingProcessID,string ProcessType,string ProcessSubType,string DataInput,string AssemblyName,string ClassName,string MethodName,Guid? ProcessorID,string ProcessStatus,string ProcessMessage,int? ProcessedCount,DateTime? SubmittedTime,DateTime? StartProcessingTime,DateTime? EndProcessingTime,string UserName,bool? EnableNotification,string CallbackURL,string Remark,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5)
	    {
		    AppPendingProcess item = new AppPendingProcess();
		    
            item.AppPendingProcessID = AppPendingProcessID;
            
            item.ProcessType = ProcessType;
            
            item.ProcessSubType = ProcessSubType;
            
            item.DataInput = DataInput;
            
            item.AssemblyName = AssemblyName;
            
            item.ClassName = ClassName;
            
            item.MethodName = MethodName;
            
            item.ProcessorID = ProcessorID;
            
            item.ProcessStatus = ProcessStatus;
            
            item.ProcessMessage = ProcessMessage;
            
            item.ProcessedCount = ProcessedCount;
            
            item.SubmittedTime = SubmittedTime;
            
            item.StartProcessingTime = StartProcessingTime;
            
            item.EndProcessingTime = EndProcessingTime;
            
            item.UserName = UserName;
            
            item.EnableNotification = EnableNotification;
            
            item.CallbackURL = CallbackURL;
            
            item.Remark = Remark;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
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
	    public void Update(Guid AppPendingProcessID,string ProcessType,string ProcessSubType,string DataInput,string AssemblyName,string ClassName,string MethodName,Guid? ProcessorID,string ProcessStatus,string ProcessMessage,int? ProcessedCount,DateTime? SubmittedTime,DateTime? StartProcessingTime,DateTime? EndProcessingTime,string UserName,bool? EnableNotification,string CallbackURL,string Remark,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5)
	    {
		    AppPendingProcess item = new AppPendingProcess();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.AppPendingProcessID = AppPendingProcessID;
				
			item.ProcessType = ProcessType;
				
			item.ProcessSubType = ProcessSubType;
				
			item.DataInput = DataInput;
				
			item.AssemblyName = AssemblyName;
				
			item.ClassName = ClassName;
				
			item.MethodName = MethodName;
				
			item.ProcessorID = ProcessorID;
				
			item.ProcessStatus = ProcessStatus;
				
			item.ProcessMessage = ProcessMessage;
				
			item.ProcessedCount = ProcessedCount;
				
			item.SubmittedTime = SubmittedTime;
				
			item.StartProcessingTime = StartProcessingTime;
				
			item.EndProcessingTime = EndProcessingTime;
				
			item.UserName = UserName;
				
			item.EnableNotification = EnableNotification;
				
			item.CallbackURL = CallbackURL;
				
			item.Remark = Remark;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
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
