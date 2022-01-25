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
    /// Controller class for Setting
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SettingController
    {
        // Preload our schema..
        Setting thisSchemaLoad = new Setting();
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
        public SettingCollection FetchAll()
        {
            SettingCollection coll = new SettingCollection();
            Query qry = new Query(Setting.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SettingCollection FetchByID(object SettingID)
        {
            SettingCollection coll = new SettingCollection().Where("SettingID", SettingID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SettingCollection FetchByQuery(Query qry)
        {
            SettingCollection coll = new SettingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object SettingID)
        {
            return (Setting.Delete(SettingID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object SettingID)
        {
            return (Setting.Destroy(SettingID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int PointOfSaleID,string WsUrl,string NETSTerminalID,int EZLinkTerminalID,bool IsEZLinkTerminal,string EZLinkTerminalPwd,string EZLinkCOMPort,int? EZLinkBaudRate,int? EZLinkDataBits,int? EZLinkParity,int? EZLinkStopBits,int? EZLinkHandShake,bool PrintQuickCashReceipt,bool PrintEZLinkReceipt,bool PrintQuickCashWithEZLink,bool PromptSalesPerson,bool UseMembership,bool AllowLineDisc,bool AllowOverallDisc,bool AllowChangeCashier,bool AllowFeedBack,string SQLServerName,string DBName,bool? IntegrateWithInventory)
	    {
		    Setting item = new Setting();
		    
            item.PointOfSaleID = PointOfSaleID;
            
            item.WsUrl = WsUrl;
            
            item.NETSTerminalID = NETSTerminalID;
            
            item.EZLinkTerminalID = EZLinkTerminalID;
            
            item.IsEZLinkTerminal = IsEZLinkTerminal;
            
            item.EZLinkTerminalPwd = EZLinkTerminalPwd;
            
            item.EZLinkCOMPort = EZLinkCOMPort;
            
            item.EZLinkBaudRate = EZLinkBaudRate;
            
            item.EZLinkDataBits = EZLinkDataBits;
            
            item.EZLinkParity = EZLinkParity;
            
            item.EZLinkStopBits = EZLinkStopBits;
            
            item.EZLinkHandShake = EZLinkHandShake;
            
            item.PrintQuickCashReceipt = PrintQuickCashReceipt;
            
            item.PrintEZLinkReceipt = PrintEZLinkReceipt;
            
            item.PrintQuickCashWithEZLink = PrintQuickCashWithEZLink;
            
            item.PromptSalesPerson = PromptSalesPerson;
            
            item.UseMembership = UseMembership;
            
            item.AllowLineDisc = AllowLineDisc;
            
            item.AllowOverallDisc = AllowOverallDisc;
            
            item.AllowChangeCashier = AllowChangeCashier;
            
            item.AllowFeedBack = AllowFeedBack;
            
            item.SQLServerName = SQLServerName;
            
            item.DBName = DBName;
            
            item.IntegrateWithInventory = IntegrateWithInventory;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int SettingID,int PointOfSaleID,string WsUrl,string NETSTerminalID,int EZLinkTerminalID,bool IsEZLinkTerminal,string EZLinkTerminalPwd,string EZLinkCOMPort,int? EZLinkBaudRate,int? EZLinkDataBits,int? EZLinkParity,int? EZLinkStopBits,int? EZLinkHandShake,bool PrintQuickCashReceipt,bool PrintEZLinkReceipt,bool PrintQuickCashWithEZLink,bool PromptSalesPerson,bool UseMembership,bool AllowLineDisc,bool AllowOverallDisc,bool AllowChangeCashier,bool AllowFeedBack,string SQLServerName,string DBName,bool? IntegrateWithInventory)
	    {
		    Setting item = new Setting();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.SettingID = SettingID;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.WsUrl = WsUrl;
				
			item.NETSTerminalID = NETSTerminalID;
				
			item.EZLinkTerminalID = EZLinkTerminalID;
				
			item.IsEZLinkTerminal = IsEZLinkTerminal;
				
			item.EZLinkTerminalPwd = EZLinkTerminalPwd;
				
			item.EZLinkCOMPort = EZLinkCOMPort;
				
			item.EZLinkBaudRate = EZLinkBaudRate;
				
			item.EZLinkDataBits = EZLinkDataBits;
				
			item.EZLinkParity = EZLinkParity;
				
			item.EZLinkStopBits = EZLinkStopBits;
				
			item.EZLinkHandShake = EZLinkHandShake;
				
			item.PrintQuickCashReceipt = PrintQuickCashReceipt;
				
			item.PrintEZLinkReceipt = PrintEZLinkReceipt;
				
			item.PrintQuickCashWithEZLink = PrintQuickCashWithEZLink;
				
			item.PromptSalesPerson = PromptSalesPerson;
				
			item.UseMembership = UseMembership;
				
			item.AllowLineDisc = AllowLineDisc;
				
			item.AllowOverallDisc = AllowOverallDisc;
				
			item.AllowChangeCashier = AllowChangeCashier;
				
			item.AllowFeedBack = AllowFeedBack;
				
			item.SQLServerName = SQLServerName;
				
			item.DBName = DBName;
				
			item.IntegrateWithInventory = IntegrateWithInventory;
				
	        item.Save(UserName);
	    }
    }
}
