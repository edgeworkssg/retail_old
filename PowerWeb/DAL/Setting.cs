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
	/// Strongly-typed collection for the Setting class.
	/// </summary>
    [Serializable]
	public partial class SettingCollection : ActiveList<Setting, SettingCollection>
	{	   
		public SettingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SettingCollection</returns>
		public SettingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Setting o = this[i];
                foreach (SubSonic.Where w in this.wheres)
                {
                    bool remove = false;
                    System.Reflection.PropertyInfo pi = o.GetType().GetProperty(w.ColumnName);
                    if (pi.CanRead)
                    {
                        object val = pi.GetValue(o, null);
                        switch (w.Comparison)
                        {
                            case SubSonic.Comparison.Equals:
                                if (!val.Equals(w.ParameterValue))
                                {
                                    remove = true;
                                }
                                break;
                        }
                    }
                    if (remove)
                    {
                        this.Remove(o);
                        break;
                    }
                }
            }
            return this;
        }
		
		
	}
	/// <summary>
	/// This is an ActiveRecord class which wraps the Setting table.
	/// </summary>
	[Serializable]
	public partial class Setting : ActiveRecord<Setting>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Setting()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Setting(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Setting(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Setting(string columnName, object columnValue)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByParam(columnName,columnValue);
		}
		
		protected static void SetSQLProps() { GetTableSchema(); }
		
		#endregion
		
		#region Schema and Query Accessor	
		public static Query CreateQuery() { return new Query(Schema); }
		public static TableSchema.Table Schema
		{
			get
			{
				if (BaseSchema == null)
					SetSQLProps();
				return BaseSchema;
			}
		}
		
		private static void GetTableSchema() 
		{
			if(!IsSchemaInitialized)
			{
				//Schema declaration
				TableSchema.Table schema = new TableSchema.Table("Setting", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarSettingID = new TableSchema.TableColumn(schema);
				colvarSettingID.ColumnName = "SettingID";
				colvarSettingID.DataType = DbType.Int32;
				colvarSettingID.MaxLength = 0;
				colvarSettingID.AutoIncrement = true;
				colvarSettingID.IsNullable = false;
				colvarSettingID.IsPrimaryKey = true;
				colvarSettingID.IsForeignKey = false;
				colvarSettingID.IsReadOnly = false;
				colvarSettingID.DefaultSetting = @"";
				colvarSettingID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSettingID);
				
				TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
				colvarPointOfSaleID.ColumnName = "PointOfSaleID";
				colvarPointOfSaleID.DataType = DbType.Int32;
				colvarPointOfSaleID.MaxLength = 0;
				colvarPointOfSaleID.AutoIncrement = false;
				colvarPointOfSaleID.IsNullable = false;
				colvarPointOfSaleID.IsPrimaryKey = false;
				colvarPointOfSaleID.IsForeignKey = false;
				colvarPointOfSaleID.IsReadOnly = false;
				colvarPointOfSaleID.DefaultSetting = @"";
				colvarPointOfSaleID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointOfSaleID);
				
				TableSchema.TableColumn colvarWsUrl = new TableSchema.TableColumn(schema);
				colvarWsUrl.ColumnName = "WS_URL";
				colvarWsUrl.DataType = DbType.AnsiString;
				colvarWsUrl.MaxLength = 200;
				colvarWsUrl.AutoIncrement = false;
				colvarWsUrl.IsNullable = true;
				colvarWsUrl.IsPrimaryKey = false;
				colvarWsUrl.IsForeignKey = false;
				colvarWsUrl.IsReadOnly = false;
				colvarWsUrl.DefaultSetting = @"";
				colvarWsUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWsUrl);
				
				TableSchema.TableColumn colvarNETSTerminalID = new TableSchema.TableColumn(schema);
				colvarNETSTerminalID.ColumnName = "NETSTerminalID";
				colvarNETSTerminalID.DataType = DbType.AnsiString;
				colvarNETSTerminalID.MaxLength = 50;
				colvarNETSTerminalID.AutoIncrement = false;
				colvarNETSTerminalID.IsNullable = true;
				colvarNETSTerminalID.IsPrimaryKey = false;
				colvarNETSTerminalID.IsForeignKey = false;
				colvarNETSTerminalID.IsReadOnly = false;
				colvarNETSTerminalID.DefaultSetting = @"";
				colvarNETSTerminalID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNETSTerminalID);
				
				TableSchema.TableColumn colvarEZLinkTerminalID = new TableSchema.TableColumn(schema);
				colvarEZLinkTerminalID.ColumnName = "EZLinkTerminalID";
				colvarEZLinkTerminalID.DataType = DbType.Int32;
				colvarEZLinkTerminalID.MaxLength = 0;
				colvarEZLinkTerminalID.AutoIncrement = false;
				colvarEZLinkTerminalID.IsNullable = false;
				colvarEZLinkTerminalID.IsPrimaryKey = false;
				colvarEZLinkTerminalID.IsForeignKey = false;
				colvarEZLinkTerminalID.IsReadOnly = false;
				
						colvarEZLinkTerminalID.DefaultSetting = @"((0))";
				colvarEZLinkTerminalID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEZLinkTerminalID);
				
				TableSchema.TableColumn colvarIsEZLinkTerminal = new TableSchema.TableColumn(schema);
				colvarIsEZLinkTerminal.ColumnName = "IsEZLinkTerminal";
				colvarIsEZLinkTerminal.DataType = DbType.Boolean;
				colvarIsEZLinkTerminal.MaxLength = 0;
				colvarIsEZLinkTerminal.AutoIncrement = false;
				colvarIsEZLinkTerminal.IsNullable = false;
				colvarIsEZLinkTerminal.IsPrimaryKey = false;
				colvarIsEZLinkTerminal.IsForeignKey = false;
				colvarIsEZLinkTerminal.IsReadOnly = false;
				
						colvarIsEZLinkTerminal.DefaultSetting = @"((0))";
				colvarIsEZLinkTerminal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsEZLinkTerminal);
				
				TableSchema.TableColumn colvarEZLinkTerminalPwd = new TableSchema.TableColumn(schema);
				colvarEZLinkTerminalPwd.ColumnName = "EZLinkTerminalPwd";
				colvarEZLinkTerminalPwd.DataType = DbType.AnsiString;
				colvarEZLinkTerminalPwd.MaxLength = 50;
				colvarEZLinkTerminalPwd.AutoIncrement = false;
				colvarEZLinkTerminalPwd.IsNullable = true;
				colvarEZLinkTerminalPwd.IsPrimaryKey = false;
				colvarEZLinkTerminalPwd.IsForeignKey = false;
				colvarEZLinkTerminalPwd.IsReadOnly = false;
				colvarEZLinkTerminalPwd.DefaultSetting = @"";
				colvarEZLinkTerminalPwd.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEZLinkTerminalPwd);
				
				TableSchema.TableColumn colvarEZLinkCOMPort = new TableSchema.TableColumn(schema);
				colvarEZLinkCOMPort.ColumnName = "EZLinkCOMPort";
				colvarEZLinkCOMPort.DataType = DbType.AnsiString;
				colvarEZLinkCOMPort.MaxLength = 5;
				colvarEZLinkCOMPort.AutoIncrement = false;
				colvarEZLinkCOMPort.IsNullable = true;
				colvarEZLinkCOMPort.IsPrimaryKey = false;
				colvarEZLinkCOMPort.IsForeignKey = false;
				colvarEZLinkCOMPort.IsReadOnly = false;
				colvarEZLinkCOMPort.DefaultSetting = @"";
				colvarEZLinkCOMPort.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEZLinkCOMPort);
				
				TableSchema.TableColumn colvarEZLinkBaudRate = new TableSchema.TableColumn(schema);
				colvarEZLinkBaudRate.ColumnName = "EZLinkBaudRate";
				colvarEZLinkBaudRate.DataType = DbType.Int32;
				colvarEZLinkBaudRate.MaxLength = 0;
				colvarEZLinkBaudRate.AutoIncrement = false;
				colvarEZLinkBaudRate.IsNullable = true;
				colvarEZLinkBaudRate.IsPrimaryKey = false;
				colvarEZLinkBaudRate.IsForeignKey = false;
				colvarEZLinkBaudRate.IsReadOnly = false;
				colvarEZLinkBaudRate.DefaultSetting = @"";
				colvarEZLinkBaudRate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEZLinkBaudRate);
				
				TableSchema.TableColumn colvarEZLinkDataBits = new TableSchema.TableColumn(schema);
				colvarEZLinkDataBits.ColumnName = "EZLinkDataBits";
				colvarEZLinkDataBits.DataType = DbType.Int32;
				colvarEZLinkDataBits.MaxLength = 0;
				colvarEZLinkDataBits.AutoIncrement = false;
				colvarEZLinkDataBits.IsNullable = true;
				colvarEZLinkDataBits.IsPrimaryKey = false;
				colvarEZLinkDataBits.IsForeignKey = false;
				colvarEZLinkDataBits.IsReadOnly = false;
				colvarEZLinkDataBits.DefaultSetting = @"";
				colvarEZLinkDataBits.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEZLinkDataBits);
				
				TableSchema.TableColumn colvarEZLinkParity = new TableSchema.TableColumn(schema);
				colvarEZLinkParity.ColumnName = "EZLinkParity";
				colvarEZLinkParity.DataType = DbType.Int32;
				colvarEZLinkParity.MaxLength = 0;
				colvarEZLinkParity.AutoIncrement = false;
				colvarEZLinkParity.IsNullable = true;
				colvarEZLinkParity.IsPrimaryKey = false;
				colvarEZLinkParity.IsForeignKey = false;
				colvarEZLinkParity.IsReadOnly = false;
				colvarEZLinkParity.DefaultSetting = @"";
				colvarEZLinkParity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEZLinkParity);
				
				TableSchema.TableColumn colvarEZLinkStopBits = new TableSchema.TableColumn(schema);
				colvarEZLinkStopBits.ColumnName = "EZLinkStopBits";
				colvarEZLinkStopBits.DataType = DbType.Int32;
				colvarEZLinkStopBits.MaxLength = 0;
				colvarEZLinkStopBits.AutoIncrement = false;
				colvarEZLinkStopBits.IsNullable = true;
				colvarEZLinkStopBits.IsPrimaryKey = false;
				colvarEZLinkStopBits.IsForeignKey = false;
				colvarEZLinkStopBits.IsReadOnly = false;
				colvarEZLinkStopBits.DefaultSetting = @"";
				colvarEZLinkStopBits.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEZLinkStopBits);
				
				TableSchema.TableColumn colvarEZLinkHandShake = new TableSchema.TableColumn(schema);
				colvarEZLinkHandShake.ColumnName = "EZLinkHandShake";
				colvarEZLinkHandShake.DataType = DbType.Int32;
				colvarEZLinkHandShake.MaxLength = 0;
				colvarEZLinkHandShake.AutoIncrement = false;
				colvarEZLinkHandShake.IsNullable = true;
				colvarEZLinkHandShake.IsPrimaryKey = false;
				colvarEZLinkHandShake.IsForeignKey = false;
				colvarEZLinkHandShake.IsReadOnly = false;
				colvarEZLinkHandShake.DefaultSetting = @"";
				colvarEZLinkHandShake.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEZLinkHandShake);
				
				TableSchema.TableColumn colvarPrintQuickCashReceipt = new TableSchema.TableColumn(schema);
				colvarPrintQuickCashReceipt.ColumnName = "PrintQuickCashReceipt";
				colvarPrintQuickCashReceipt.DataType = DbType.Boolean;
				colvarPrintQuickCashReceipt.MaxLength = 0;
				colvarPrintQuickCashReceipt.AutoIncrement = false;
				colvarPrintQuickCashReceipt.IsNullable = false;
				colvarPrintQuickCashReceipt.IsPrimaryKey = false;
				colvarPrintQuickCashReceipt.IsForeignKey = false;
				colvarPrintQuickCashReceipt.IsReadOnly = false;
				
						colvarPrintQuickCashReceipt.DefaultSetting = @"((0))";
				colvarPrintQuickCashReceipt.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPrintQuickCashReceipt);
				
				TableSchema.TableColumn colvarPrintEZLinkReceipt = new TableSchema.TableColumn(schema);
				colvarPrintEZLinkReceipt.ColumnName = "PrintEZLinkReceipt";
				colvarPrintEZLinkReceipt.DataType = DbType.Boolean;
				colvarPrintEZLinkReceipt.MaxLength = 0;
				colvarPrintEZLinkReceipt.AutoIncrement = false;
				colvarPrintEZLinkReceipt.IsNullable = false;
				colvarPrintEZLinkReceipt.IsPrimaryKey = false;
				colvarPrintEZLinkReceipt.IsForeignKey = false;
				colvarPrintEZLinkReceipt.IsReadOnly = false;
				
						colvarPrintEZLinkReceipt.DefaultSetting = @"((0))";
				colvarPrintEZLinkReceipt.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPrintEZLinkReceipt);
				
				TableSchema.TableColumn colvarPrintQuickCashWithEZLink = new TableSchema.TableColumn(schema);
				colvarPrintQuickCashWithEZLink.ColumnName = "PrintQuickCashWithEZLink";
				colvarPrintQuickCashWithEZLink.DataType = DbType.Boolean;
				colvarPrintQuickCashWithEZLink.MaxLength = 0;
				colvarPrintQuickCashWithEZLink.AutoIncrement = false;
				colvarPrintQuickCashWithEZLink.IsNullable = false;
				colvarPrintQuickCashWithEZLink.IsPrimaryKey = false;
				colvarPrintQuickCashWithEZLink.IsForeignKey = false;
				colvarPrintQuickCashWithEZLink.IsReadOnly = false;
				
						colvarPrintQuickCashWithEZLink.DefaultSetting = @"((0))";
				colvarPrintQuickCashWithEZLink.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPrintQuickCashWithEZLink);
				
				TableSchema.TableColumn colvarPromptSalesPerson = new TableSchema.TableColumn(schema);
				colvarPromptSalesPerson.ColumnName = "promptSalesPerson";
				colvarPromptSalesPerson.DataType = DbType.Boolean;
				colvarPromptSalesPerson.MaxLength = 0;
				colvarPromptSalesPerson.AutoIncrement = false;
				colvarPromptSalesPerson.IsNullable = false;
				colvarPromptSalesPerson.IsPrimaryKey = false;
				colvarPromptSalesPerson.IsForeignKey = false;
				colvarPromptSalesPerson.IsReadOnly = false;
				
						colvarPromptSalesPerson.DefaultSetting = @"((0))";
				colvarPromptSalesPerson.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPromptSalesPerson);
				
				TableSchema.TableColumn colvarUseMembership = new TableSchema.TableColumn(schema);
				colvarUseMembership.ColumnName = "useMembership";
				colvarUseMembership.DataType = DbType.Boolean;
				colvarUseMembership.MaxLength = 0;
				colvarUseMembership.AutoIncrement = false;
				colvarUseMembership.IsNullable = false;
				colvarUseMembership.IsPrimaryKey = false;
				colvarUseMembership.IsForeignKey = false;
				colvarUseMembership.IsReadOnly = false;
				
						colvarUseMembership.DefaultSetting = @"((0))";
				colvarUseMembership.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUseMembership);
				
				TableSchema.TableColumn colvarAllowLineDisc = new TableSchema.TableColumn(schema);
				colvarAllowLineDisc.ColumnName = "allowLineDisc";
				colvarAllowLineDisc.DataType = DbType.Boolean;
				colvarAllowLineDisc.MaxLength = 0;
				colvarAllowLineDisc.AutoIncrement = false;
				colvarAllowLineDisc.IsNullable = false;
				colvarAllowLineDisc.IsPrimaryKey = false;
				colvarAllowLineDisc.IsForeignKey = false;
				colvarAllowLineDisc.IsReadOnly = false;
				
						colvarAllowLineDisc.DefaultSetting = @"((0))";
				colvarAllowLineDisc.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAllowLineDisc);
				
				TableSchema.TableColumn colvarAllowOverallDisc = new TableSchema.TableColumn(schema);
				colvarAllowOverallDisc.ColumnName = "allowOverallDisc";
				colvarAllowOverallDisc.DataType = DbType.Boolean;
				colvarAllowOverallDisc.MaxLength = 0;
				colvarAllowOverallDisc.AutoIncrement = false;
				colvarAllowOverallDisc.IsNullable = false;
				colvarAllowOverallDisc.IsPrimaryKey = false;
				colvarAllowOverallDisc.IsForeignKey = false;
				colvarAllowOverallDisc.IsReadOnly = false;
				
						colvarAllowOverallDisc.DefaultSetting = @"((0))";
				colvarAllowOverallDisc.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAllowOverallDisc);
				
				TableSchema.TableColumn colvarAllowChangeCashier = new TableSchema.TableColumn(schema);
				colvarAllowChangeCashier.ColumnName = "allowChangeCashier";
				colvarAllowChangeCashier.DataType = DbType.Boolean;
				colvarAllowChangeCashier.MaxLength = 0;
				colvarAllowChangeCashier.AutoIncrement = false;
				colvarAllowChangeCashier.IsNullable = false;
				colvarAllowChangeCashier.IsPrimaryKey = false;
				colvarAllowChangeCashier.IsForeignKey = false;
				colvarAllowChangeCashier.IsReadOnly = false;
				
						colvarAllowChangeCashier.DefaultSetting = @"((0))";
				colvarAllowChangeCashier.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAllowChangeCashier);
				
				TableSchema.TableColumn colvarAllowFeedBack = new TableSchema.TableColumn(schema);
				colvarAllowFeedBack.ColumnName = "allowFeedBack";
				colvarAllowFeedBack.DataType = DbType.Boolean;
				colvarAllowFeedBack.MaxLength = 0;
				colvarAllowFeedBack.AutoIncrement = false;
				colvarAllowFeedBack.IsNullable = false;
				colvarAllowFeedBack.IsPrimaryKey = false;
				colvarAllowFeedBack.IsForeignKey = false;
				colvarAllowFeedBack.IsReadOnly = false;
				
						colvarAllowFeedBack.DefaultSetting = @"((0))";
				colvarAllowFeedBack.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAllowFeedBack);
				
				TableSchema.TableColumn colvarSQLServerName = new TableSchema.TableColumn(schema);
				colvarSQLServerName.ColumnName = "SQLServerName";
				colvarSQLServerName.DataType = DbType.AnsiString;
				colvarSQLServerName.MaxLength = 250;
				colvarSQLServerName.AutoIncrement = false;
				colvarSQLServerName.IsNullable = true;
				colvarSQLServerName.IsPrimaryKey = false;
				colvarSQLServerName.IsForeignKey = false;
				colvarSQLServerName.IsReadOnly = false;
				colvarSQLServerName.DefaultSetting = @"";
				colvarSQLServerName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSQLServerName);
				
				TableSchema.TableColumn colvarDBName = new TableSchema.TableColumn(schema);
				colvarDBName.ColumnName = "DBName";
				colvarDBName.DataType = DbType.AnsiString;
				colvarDBName.MaxLength = 50;
				colvarDBName.AutoIncrement = false;
				colvarDBName.IsNullable = true;
				colvarDBName.IsPrimaryKey = false;
				colvarDBName.IsForeignKey = false;
				colvarDBName.IsReadOnly = false;
				colvarDBName.DefaultSetting = @"";
				colvarDBName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDBName);
				
				TableSchema.TableColumn colvarIntegrateWithInventory = new TableSchema.TableColumn(schema);
				colvarIntegrateWithInventory.ColumnName = "IntegrateWithInventory";
				colvarIntegrateWithInventory.DataType = DbType.Boolean;
				colvarIntegrateWithInventory.MaxLength = 0;
				colvarIntegrateWithInventory.AutoIncrement = false;
				colvarIntegrateWithInventory.IsNullable = true;
				colvarIntegrateWithInventory.IsPrimaryKey = false;
				colvarIntegrateWithInventory.IsForeignKey = false;
				colvarIntegrateWithInventory.IsReadOnly = false;
				colvarIntegrateWithInventory.DefaultSetting = @"";
				colvarIntegrateWithInventory.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIntegrateWithInventory);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Setting",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("SettingID")]
		[Bindable(true)]
		public int SettingID 
		{
			get { return GetColumnValue<int>(Columns.SettingID); }
			set { SetColumnValue(Columns.SettingID, value); }
		}
		  
		[XmlAttribute("PointOfSaleID")]
		[Bindable(true)]
		public int PointOfSaleID 
		{
			get { return GetColumnValue<int>(Columns.PointOfSaleID); }
			set { SetColumnValue(Columns.PointOfSaleID, value); }
		}
		  
		[XmlAttribute("WsUrl")]
		[Bindable(true)]
		public string WsUrl 
		{
			get { return GetColumnValue<string>(Columns.WsUrl); }
			set { SetColumnValue(Columns.WsUrl, value); }
		}
		  
		[XmlAttribute("NETSTerminalID")]
		[Bindable(true)]
		public string NETSTerminalID 
		{
			get { return GetColumnValue<string>(Columns.NETSTerminalID); }
			set { SetColumnValue(Columns.NETSTerminalID, value); }
		}
		  
		[XmlAttribute("EZLinkTerminalID")]
		[Bindable(true)]
		public int EZLinkTerminalID 
		{
			get { return GetColumnValue<int>(Columns.EZLinkTerminalID); }
			set { SetColumnValue(Columns.EZLinkTerminalID, value); }
		}
		  
		[XmlAttribute("IsEZLinkTerminal")]
		[Bindable(true)]
		public bool IsEZLinkTerminal 
		{
			get { return GetColumnValue<bool>(Columns.IsEZLinkTerminal); }
			set { SetColumnValue(Columns.IsEZLinkTerminal, value); }
		}
		  
		[XmlAttribute("EZLinkTerminalPwd")]
		[Bindable(true)]
		public string EZLinkTerminalPwd 
		{
			get { return GetColumnValue<string>(Columns.EZLinkTerminalPwd); }
			set { SetColumnValue(Columns.EZLinkTerminalPwd, value); }
		}
		  
		[XmlAttribute("EZLinkCOMPort")]
		[Bindable(true)]
		public string EZLinkCOMPort 
		{
			get { return GetColumnValue<string>(Columns.EZLinkCOMPort); }
			set { SetColumnValue(Columns.EZLinkCOMPort, value); }
		}
		  
		[XmlAttribute("EZLinkBaudRate")]
		[Bindable(true)]
		public int? EZLinkBaudRate 
		{
			get { return GetColumnValue<int?>(Columns.EZLinkBaudRate); }
			set { SetColumnValue(Columns.EZLinkBaudRate, value); }
		}
		  
		[XmlAttribute("EZLinkDataBits")]
		[Bindable(true)]
		public int? EZLinkDataBits 
		{
			get { return GetColumnValue<int?>(Columns.EZLinkDataBits); }
			set { SetColumnValue(Columns.EZLinkDataBits, value); }
		}
		  
		[XmlAttribute("EZLinkParity")]
		[Bindable(true)]
		public int? EZLinkParity 
		{
			get { return GetColumnValue<int?>(Columns.EZLinkParity); }
			set { SetColumnValue(Columns.EZLinkParity, value); }
		}
		  
		[XmlAttribute("EZLinkStopBits")]
		[Bindable(true)]
		public int? EZLinkStopBits 
		{
			get { return GetColumnValue<int?>(Columns.EZLinkStopBits); }
			set { SetColumnValue(Columns.EZLinkStopBits, value); }
		}
		  
		[XmlAttribute("EZLinkHandShake")]
		[Bindable(true)]
		public int? EZLinkHandShake 
		{
			get { return GetColumnValue<int?>(Columns.EZLinkHandShake); }
			set { SetColumnValue(Columns.EZLinkHandShake, value); }
		}
		  
		[XmlAttribute("PrintQuickCashReceipt")]
		[Bindable(true)]
		public bool PrintQuickCashReceipt 
		{
			get { return GetColumnValue<bool>(Columns.PrintQuickCashReceipt); }
			set { SetColumnValue(Columns.PrintQuickCashReceipt, value); }
		}
		  
		[XmlAttribute("PrintEZLinkReceipt")]
		[Bindable(true)]
		public bool PrintEZLinkReceipt 
		{
			get { return GetColumnValue<bool>(Columns.PrintEZLinkReceipt); }
			set { SetColumnValue(Columns.PrintEZLinkReceipt, value); }
		}
		  
		[XmlAttribute("PrintQuickCashWithEZLink")]
		[Bindable(true)]
		public bool PrintQuickCashWithEZLink 
		{
			get { return GetColumnValue<bool>(Columns.PrintQuickCashWithEZLink); }
			set { SetColumnValue(Columns.PrintQuickCashWithEZLink, value); }
		}
		  
		[XmlAttribute("PromptSalesPerson")]
		[Bindable(true)]
		public bool PromptSalesPerson 
		{
			get { return GetColumnValue<bool>(Columns.PromptSalesPerson); }
			set { SetColumnValue(Columns.PromptSalesPerson, value); }
		}
		  
		[XmlAttribute("UseMembership")]
		[Bindable(true)]
		public bool UseMembership 
		{
			get { return GetColumnValue<bool>(Columns.UseMembership); }
			set { SetColumnValue(Columns.UseMembership, value); }
		}
		  
		[XmlAttribute("AllowLineDisc")]
		[Bindable(true)]
		public bool AllowLineDisc 
		{
			get { return GetColumnValue<bool>(Columns.AllowLineDisc); }
			set { SetColumnValue(Columns.AllowLineDisc, value); }
		}
		  
		[XmlAttribute("AllowOverallDisc")]
		[Bindable(true)]
		public bool AllowOverallDisc 
		{
			get { return GetColumnValue<bool>(Columns.AllowOverallDisc); }
			set { SetColumnValue(Columns.AllowOverallDisc, value); }
		}
		  
		[XmlAttribute("AllowChangeCashier")]
		[Bindable(true)]
		public bool AllowChangeCashier 
		{
			get { return GetColumnValue<bool>(Columns.AllowChangeCashier); }
			set { SetColumnValue(Columns.AllowChangeCashier, value); }
		}
		  
		[XmlAttribute("AllowFeedBack")]
		[Bindable(true)]
		public bool AllowFeedBack 
		{
			get { return GetColumnValue<bool>(Columns.AllowFeedBack); }
			set { SetColumnValue(Columns.AllowFeedBack, value); }
		}
		  
		[XmlAttribute("SQLServerName")]
		[Bindable(true)]
		public string SQLServerName 
		{
			get { return GetColumnValue<string>(Columns.SQLServerName); }
			set { SetColumnValue(Columns.SQLServerName, value); }
		}
		  
		[XmlAttribute("DBName")]
		[Bindable(true)]
		public string DBName 
		{
			get { return GetColumnValue<string>(Columns.DBName); }
			set { SetColumnValue(Columns.DBName, value); }
		}
		  
		[XmlAttribute("IntegrateWithInventory")]
		[Bindable(true)]
		public bool? IntegrateWithInventory 
		{
			get { return GetColumnValue<bool?>(Columns.IntegrateWithInventory); }
			set { SetColumnValue(Columns.IntegrateWithInventory, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varPointOfSaleID,string varWsUrl,string varNETSTerminalID,int varEZLinkTerminalID,bool varIsEZLinkTerminal,string varEZLinkTerminalPwd,string varEZLinkCOMPort,int? varEZLinkBaudRate,int? varEZLinkDataBits,int? varEZLinkParity,int? varEZLinkStopBits,int? varEZLinkHandShake,bool varPrintQuickCashReceipt,bool varPrintEZLinkReceipt,bool varPrintQuickCashWithEZLink,bool varPromptSalesPerson,bool varUseMembership,bool varAllowLineDisc,bool varAllowOverallDisc,bool varAllowChangeCashier,bool varAllowFeedBack,string varSQLServerName,string varDBName,bool? varIntegrateWithInventory)
		{
			Setting item = new Setting();
			
			item.PointOfSaleID = varPointOfSaleID;
			
			item.WsUrl = varWsUrl;
			
			item.NETSTerminalID = varNETSTerminalID;
			
			item.EZLinkTerminalID = varEZLinkTerminalID;
			
			item.IsEZLinkTerminal = varIsEZLinkTerminal;
			
			item.EZLinkTerminalPwd = varEZLinkTerminalPwd;
			
			item.EZLinkCOMPort = varEZLinkCOMPort;
			
			item.EZLinkBaudRate = varEZLinkBaudRate;
			
			item.EZLinkDataBits = varEZLinkDataBits;
			
			item.EZLinkParity = varEZLinkParity;
			
			item.EZLinkStopBits = varEZLinkStopBits;
			
			item.EZLinkHandShake = varEZLinkHandShake;
			
			item.PrintQuickCashReceipt = varPrintQuickCashReceipt;
			
			item.PrintEZLinkReceipt = varPrintEZLinkReceipt;
			
			item.PrintQuickCashWithEZLink = varPrintQuickCashWithEZLink;
			
			item.PromptSalesPerson = varPromptSalesPerson;
			
			item.UseMembership = varUseMembership;
			
			item.AllowLineDisc = varAllowLineDisc;
			
			item.AllowOverallDisc = varAllowOverallDisc;
			
			item.AllowChangeCashier = varAllowChangeCashier;
			
			item.AllowFeedBack = varAllowFeedBack;
			
			item.SQLServerName = varSQLServerName;
			
			item.DBName = varDBName;
			
			item.IntegrateWithInventory = varIntegrateWithInventory;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varSettingID,int varPointOfSaleID,string varWsUrl,string varNETSTerminalID,int varEZLinkTerminalID,bool varIsEZLinkTerminal,string varEZLinkTerminalPwd,string varEZLinkCOMPort,int? varEZLinkBaudRate,int? varEZLinkDataBits,int? varEZLinkParity,int? varEZLinkStopBits,int? varEZLinkHandShake,bool varPrintQuickCashReceipt,bool varPrintEZLinkReceipt,bool varPrintQuickCashWithEZLink,bool varPromptSalesPerson,bool varUseMembership,bool varAllowLineDisc,bool varAllowOverallDisc,bool varAllowChangeCashier,bool varAllowFeedBack,string varSQLServerName,string varDBName,bool? varIntegrateWithInventory)
		{
			Setting item = new Setting();
			
				item.SettingID = varSettingID;
			
				item.PointOfSaleID = varPointOfSaleID;
			
				item.WsUrl = varWsUrl;
			
				item.NETSTerminalID = varNETSTerminalID;
			
				item.EZLinkTerminalID = varEZLinkTerminalID;
			
				item.IsEZLinkTerminal = varIsEZLinkTerminal;
			
				item.EZLinkTerminalPwd = varEZLinkTerminalPwd;
			
				item.EZLinkCOMPort = varEZLinkCOMPort;
			
				item.EZLinkBaudRate = varEZLinkBaudRate;
			
				item.EZLinkDataBits = varEZLinkDataBits;
			
				item.EZLinkParity = varEZLinkParity;
			
				item.EZLinkStopBits = varEZLinkStopBits;
			
				item.EZLinkHandShake = varEZLinkHandShake;
			
				item.PrintQuickCashReceipt = varPrintQuickCashReceipt;
			
				item.PrintEZLinkReceipt = varPrintEZLinkReceipt;
			
				item.PrintQuickCashWithEZLink = varPrintQuickCashWithEZLink;
			
				item.PromptSalesPerson = varPromptSalesPerson;
			
				item.UseMembership = varUseMembership;
			
				item.AllowLineDisc = varAllowLineDisc;
			
				item.AllowOverallDisc = varAllowOverallDisc;
			
				item.AllowChangeCashier = varAllowChangeCashier;
			
				item.AllowFeedBack = varAllowFeedBack;
			
				item.SQLServerName = varSQLServerName;
			
				item.DBName = varDBName;
			
				item.IntegrateWithInventory = varIntegrateWithInventory;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn SettingIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PointOfSaleIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn WsUrlColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn NETSTerminalIDColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn EZLinkTerminalIDColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IsEZLinkTerminalColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn EZLinkTerminalPwdColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn EZLinkCOMPortColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn EZLinkBaudRateColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn EZLinkDataBitsColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn EZLinkParityColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn EZLinkStopBitsColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn EZLinkHandShakeColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn PrintQuickCashReceiptColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn PrintEZLinkReceiptColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn PrintQuickCashWithEZLinkColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn PromptSalesPersonColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn UseMembershipColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn AllowLineDiscColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn AllowOverallDiscColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn AllowChangeCashierColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn AllowFeedBackColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn SQLServerNameColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn DBNameColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn IntegrateWithInventoryColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string SettingID = @"SettingID";
			 public static string PointOfSaleID = @"PointOfSaleID";
			 public static string WsUrl = @"WS_URL";
			 public static string NETSTerminalID = @"NETSTerminalID";
			 public static string EZLinkTerminalID = @"EZLinkTerminalID";
			 public static string IsEZLinkTerminal = @"IsEZLinkTerminal";
			 public static string EZLinkTerminalPwd = @"EZLinkTerminalPwd";
			 public static string EZLinkCOMPort = @"EZLinkCOMPort";
			 public static string EZLinkBaudRate = @"EZLinkBaudRate";
			 public static string EZLinkDataBits = @"EZLinkDataBits";
			 public static string EZLinkParity = @"EZLinkParity";
			 public static string EZLinkStopBits = @"EZLinkStopBits";
			 public static string EZLinkHandShake = @"EZLinkHandShake";
			 public static string PrintQuickCashReceipt = @"PrintQuickCashReceipt";
			 public static string PrintEZLinkReceipt = @"PrintEZLinkReceipt";
			 public static string PrintQuickCashWithEZLink = @"PrintQuickCashWithEZLink";
			 public static string PromptSalesPerson = @"promptSalesPerson";
			 public static string UseMembership = @"useMembership";
			 public static string AllowLineDisc = @"allowLineDisc";
			 public static string AllowOverallDisc = @"allowOverallDisc";
			 public static string AllowChangeCashier = @"allowChangeCashier";
			 public static string AllowFeedBack = @"allowFeedBack";
			 public static string SQLServerName = @"SQLServerName";
			 public static string DBName = @"DBName";
			 public static string IntegrateWithInventory = @"IntegrateWithInventory";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
