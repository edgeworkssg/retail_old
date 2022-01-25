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
	/// Strongly-typed collection for the SavedClosing class.
	/// </summary>
    [Serializable]
	public partial class SavedClosingCollection : ActiveList<SavedClosing, SavedClosingCollection>
	{	   
		public SavedClosingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SavedClosingCollection</returns>
		public SavedClosingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SavedClosing o = this[i];
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
	/// This is an ActiveRecord class which wraps the SavedClosing table.
	/// </summary>
	[Serializable]
	public partial class SavedClosing : ActiveRecord<SavedClosing>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SavedClosing()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SavedClosing(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SavedClosing(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SavedClosing(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SavedClosing", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarSavedID = new TableSchema.TableColumn(schema);
				colvarSavedID.ColumnName = "SavedID";
				colvarSavedID.DataType = DbType.Int32;
				colvarSavedID.MaxLength = 0;
				colvarSavedID.AutoIncrement = true;
				colvarSavedID.IsNullable = false;
				colvarSavedID.IsPrimaryKey = true;
				colvarSavedID.IsForeignKey = false;
				colvarSavedID.IsReadOnly = false;
				colvarSavedID.DefaultSetting = @"";
				colvarSavedID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSavedID);
				
				TableSchema.TableColumn colvarFloatBalance = new TableSchema.TableColumn(schema);
				colvarFloatBalance.ColumnName = "FloatBalance";
				colvarFloatBalance.DataType = DbType.AnsiString;
				colvarFloatBalance.MaxLength = 50;
				colvarFloatBalance.AutoIncrement = false;
				colvarFloatBalance.IsNullable = false;
				colvarFloatBalance.IsPrimaryKey = false;
				colvarFloatBalance.IsForeignKey = false;
				colvarFloatBalance.IsReadOnly = false;
				colvarFloatBalance.DefaultSetting = @"";
				colvarFloatBalance.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFloatBalance);
				
				TableSchema.TableColumn colvarNetsCollected = new TableSchema.TableColumn(schema);
				colvarNetsCollected.ColumnName = "NetsCollected";
				colvarNetsCollected.DataType = DbType.AnsiString;
				colvarNetsCollected.MaxLength = 50;
				colvarNetsCollected.AutoIncrement = false;
				colvarNetsCollected.IsNullable = false;
				colvarNetsCollected.IsPrimaryKey = false;
				colvarNetsCollected.IsForeignKey = false;
				colvarNetsCollected.IsReadOnly = false;
				colvarNetsCollected.DefaultSetting = @"";
				colvarNetsCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNetsCollected);
				
				TableSchema.TableColumn colvarNetsTerminalID = new TableSchema.TableColumn(schema);
				colvarNetsTerminalID.ColumnName = "NetsTerminalID";
				colvarNetsTerminalID.DataType = DbType.AnsiString;
				colvarNetsTerminalID.MaxLength = 50;
				colvarNetsTerminalID.AutoIncrement = false;
				colvarNetsTerminalID.IsNullable = false;
				colvarNetsTerminalID.IsPrimaryKey = false;
				colvarNetsTerminalID.IsForeignKey = false;
				colvarNetsTerminalID.IsReadOnly = false;
				colvarNetsTerminalID.DefaultSetting = @"";
				colvarNetsTerminalID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNetsTerminalID);
				
				TableSchema.TableColumn colvarVisaCollected = new TableSchema.TableColumn(schema);
				colvarVisaCollected.ColumnName = "VisaCollected";
				colvarVisaCollected.DataType = DbType.AnsiString;
				colvarVisaCollected.MaxLength = 50;
				colvarVisaCollected.AutoIncrement = false;
				colvarVisaCollected.IsNullable = false;
				colvarVisaCollected.IsPrimaryKey = false;
				colvarVisaCollected.IsForeignKey = false;
				colvarVisaCollected.IsReadOnly = false;
				colvarVisaCollected.DefaultSetting = @"";
				colvarVisaCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVisaCollected);
				
				TableSchema.TableColumn colvarVisaBatchNo = new TableSchema.TableColumn(schema);
				colvarVisaBatchNo.ColumnName = "VisaBatchNo";
				colvarVisaBatchNo.DataType = DbType.AnsiString;
				colvarVisaBatchNo.MaxLength = 50;
				colvarVisaBatchNo.AutoIncrement = false;
				colvarVisaBatchNo.IsNullable = false;
				colvarVisaBatchNo.IsPrimaryKey = false;
				colvarVisaBatchNo.IsForeignKey = false;
				colvarVisaBatchNo.IsReadOnly = false;
				colvarVisaBatchNo.DefaultSetting = @"";
				colvarVisaBatchNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVisaBatchNo);
				
				TableSchema.TableColumn colvarAmexCollected = new TableSchema.TableColumn(schema);
				colvarAmexCollected.ColumnName = "AmexCollected";
				colvarAmexCollected.DataType = DbType.AnsiString;
				colvarAmexCollected.MaxLength = 50;
				colvarAmexCollected.AutoIncrement = false;
				colvarAmexCollected.IsNullable = false;
				colvarAmexCollected.IsPrimaryKey = false;
				colvarAmexCollected.IsForeignKey = false;
				colvarAmexCollected.IsReadOnly = false;
				colvarAmexCollected.DefaultSetting = @"";
				colvarAmexCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmexCollected);
				
				TableSchema.TableColumn colvarAmexBatchNo = new TableSchema.TableColumn(schema);
				colvarAmexBatchNo.ColumnName = "AmexBatchNo";
				colvarAmexBatchNo.DataType = DbType.AnsiString;
				colvarAmexBatchNo.MaxLength = 50;
				colvarAmexBatchNo.AutoIncrement = false;
				colvarAmexBatchNo.IsNullable = false;
				colvarAmexBatchNo.IsPrimaryKey = false;
				colvarAmexBatchNo.IsForeignKey = false;
				colvarAmexBatchNo.IsReadOnly = false;
				colvarAmexBatchNo.DefaultSetting = @"";
				colvarAmexBatchNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmexBatchNo);
				
				TableSchema.TableColumn colvarChinaNetsCollected = new TableSchema.TableColumn(schema);
				colvarChinaNetsCollected.ColumnName = "ChinaNetsCollected";
				colvarChinaNetsCollected.DataType = DbType.AnsiString;
				colvarChinaNetsCollected.MaxLength = 50;
				colvarChinaNetsCollected.AutoIncrement = false;
				colvarChinaNetsCollected.IsNullable = false;
				colvarChinaNetsCollected.IsPrimaryKey = false;
				colvarChinaNetsCollected.IsForeignKey = false;
				colvarChinaNetsCollected.IsReadOnly = false;
				colvarChinaNetsCollected.DefaultSetting = @"";
				colvarChinaNetsCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChinaNetsCollected);
				
				TableSchema.TableColumn colvarChinaNetsTerminalID = new TableSchema.TableColumn(schema);
				colvarChinaNetsTerminalID.ColumnName = "ChinaNetsTerminalID";
				colvarChinaNetsTerminalID.DataType = DbType.AnsiString;
				colvarChinaNetsTerminalID.MaxLength = 50;
				colvarChinaNetsTerminalID.AutoIncrement = false;
				colvarChinaNetsTerminalID.IsNullable = false;
				colvarChinaNetsTerminalID.IsPrimaryKey = false;
				colvarChinaNetsTerminalID.IsForeignKey = false;
				colvarChinaNetsTerminalID.IsReadOnly = false;
				colvarChinaNetsTerminalID.DefaultSetting = @"";
				colvarChinaNetsTerminalID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChinaNetsTerminalID);
				
				TableSchema.TableColumn colvarVoucherCollected = new TableSchema.TableColumn(schema);
				colvarVoucherCollected.ColumnName = "VoucherCollected";
				colvarVoucherCollected.DataType = DbType.AnsiString;
				colvarVoucherCollected.MaxLength = 50;
				colvarVoucherCollected.AutoIncrement = false;
				colvarVoucherCollected.IsNullable = false;
				colvarVoucherCollected.IsPrimaryKey = false;
				colvarVoucherCollected.IsForeignKey = false;
				colvarVoucherCollected.IsReadOnly = false;
				colvarVoucherCollected.DefaultSetting = @"";
				colvarVoucherCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVoucherCollected);
				
				TableSchema.TableColumn colvarChequeCollected = new TableSchema.TableColumn(schema);
				colvarChequeCollected.ColumnName = "ChequeCollected";
				colvarChequeCollected.DataType = DbType.AnsiString;
				colvarChequeCollected.MaxLength = 50;
				colvarChequeCollected.AutoIncrement = false;
				colvarChequeCollected.IsNullable = false;
				colvarChequeCollected.IsPrimaryKey = false;
				colvarChequeCollected.IsForeignKey = false;
				colvarChequeCollected.IsReadOnly = false;
				colvarChequeCollected.DefaultSetting = @"";
				colvarChequeCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChequeCollected);
				
				TableSchema.TableColumn colvarDepositBagNo = new TableSchema.TableColumn(schema);
				colvarDepositBagNo.ColumnName = "DepositBagNo";
				colvarDepositBagNo.DataType = DbType.AnsiString;
				colvarDepositBagNo.MaxLength = 50;
				colvarDepositBagNo.AutoIncrement = false;
				colvarDepositBagNo.IsNullable = false;
				colvarDepositBagNo.IsPrimaryKey = false;
				colvarDepositBagNo.IsForeignKey = false;
				colvarDepositBagNo.IsReadOnly = false;
				colvarDepositBagNo.DefaultSetting = @"";
				colvarDepositBagNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDepositBagNo);
				
				TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
				colvarPointOfSaleID.ColumnName = "PointOfSaleID";
				colvarPointOfSaleID.DataType = DbType.AnsiString;
				colvarPointOfSaleID.MaxLength = 50;
				colvarPointOfSaleID.AutoIncrement = false;
				colvarPointOfSaleID.IsNullable = false;
				colvarPointOfSaleID.IsPrimaryKey = false;
				colvarPointOfSaleID.IsForeignKey = false;
				colvarPointOfSaleID.IsReadOnly = false;
				colvarPointOfSaleID.DefaultSetting = @"";
				colvarPointOfSaleID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointOfSaleID);
				
				TableSchema.TableColumn colvarC100 = new TableSchema.TableColumn(schema);
				colvarC100.ColumnName = "c100";
				colvarC100.DataType = DbType.AnsiString;
				colvarC100.MaxLength = 50;
				colvarC100.AutoIncrement = false;
				colvarC100.IsNullable = false;
				colvarC100.IsPrimaryKey = false;
				colvarC100.IsForeignKey = false;
				colvarC100.IsReadOnly = false;
				colvarC100.DefaultSetting = @"";
				colvarC100.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC100);
				
				TableSchema.TableColumn colvarC50 = new TableSchema.TableColumn(schema);
				colvarC50.ColumnName = "c50";
				colvarC50.DataType = DbType.AnsiString;
				colvarC50.MaxLength = 50;
				colvarC50.AutoIncrement = false;
				colvarC50.IsNullable = false;
				colvarC50.IsPrimaryKey = false;
				colvarC50.IsForeignKey = false;
				colvarC50.IsReadOnly = false;
				colvarC50.DefaultSetting = @"";
				colvarC50.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC50);
				
				TableSchema.TableColumn colvarC10 = new TableSchema.TableColumn(schema);
				colvarC10.ColumnName = "c10";
				colvarC10.DataType = DbType.AnsiString;
				colvarC10.MaxLength = 50;
				colvarC10.AutoIncrement = false;
				colvarC10.IsNullable = false;
				colvarC10.IsPrimaryKey = false;
				colvarC10.IsForeignKey = false;
				colvarC10.IsReadOnly = false;
				colvarC10.DefaultSetting = @"";
				colvarC10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC10);
				
				TableSchema.TableColumn colvarC5 = new TableSchema.TableColumn(schema);
				colvarC5.ColumnName = "c5";
				colvarC5.DataType = DbType.AnsiString;
				colvarC5.MaxLength = 50;
				colvarC5.AutoIncrement = false;
				colvarC5.IsNullable = false;
				colvarC5.IsPrimaryKey = false;
				colvarC5.IsForeignKey = false;
				colvarC5.IsReadOnly = false;
				colvarC5.DefaultSetting = @"";
				colvarC5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC5);
				
				TableSchema.TableColumn colvarC2 = new TableSchema.TableColumn(schema);
				colvarC2.ColumnName = "c2";
				colvarC2.DataType = DbType.AnsiString;
				colvarC2.MaxLength = 50;
				colvarC2.AutoIncrement = false;
				colvarC2.IsNullable = false;
				colvarC2.IsPrimaryKey = false;
				colvarC2.IsForeignKey = false;
				colvarC2.IsReadOnly = false;
				colvarC2.DefaultSetting = @"";
				colvarC2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC2);
				
				TableSchema.TableColumn colvarC1 = new TableSchema.TableColumn(schema);
				colvarC1.ColumnName = "c1";
				colvarC1.DataType = DbType.AnsiString;
				colvarC1.MaxLength = 50;
				colvarC1.AutoIncrement = false;
				colvarC1.IsNullable = false;
				colvarC1.IsPrimaryKey = false;
				colvarC1.IsForeignKey = false;
				colvarC1.IsReadOnly = false;
				colvarC1.DefaultSetting = @"";
				colvarC1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC1);
				
				TableSchema.TableColumn colvarC050 = new TableSchema.TableColumn(schema);
				colvarC050.ColumnName = "c050";
				colvarC050.DataType = DbType.AnsiString;
				colvarC050.MaxLength = 50;
				colvarC050.AutoIncrement = false;
				colvarC050.IsNullable = false;
				colvarC050.IsPrimaryKey = false;
				colvarC050.IsForeignKey = false;
				colvarC050.IsReadOnly = false;
				colvarC050.DefaultSetting = @"";
				colvarC050.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC050);
				
				TableSchema.TableColumn colvarC020 = new TableSchema.TableColumn(schema);
				colvarC020.ColumnName = "c020";
				colvarC020.DataType = DbType.AnsiString;
				colvarC020.MaxLength = 50;
				colvarC020.AutoIncrement = false;
				colvarC020.IsNullable = false;
				colvarC020.IsPrimaryKey = false;
				colvarC020.IsForeignKey = false;
				colvarC020.IsReadOnly = false;
				colvarC020.DefaultSetting = @"";
				colvarC020.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC020);
				
				TableSchema.TableColumn colvarC010 = new TableSchema.TableColumn(schema);
				colvarC010.ColumnName = "c010";
				colvarC010.DataType = DbType.AnsiString;
				colvarC010.MaxLength = 50;
				colvarC010.AutoIncrement = false;
				colvarC010.IsNullable = false;
				colvarC010.IsPrimaryKey = false;
				colvarC010.IsForeignKey = false;
				colvarC010.IsReadOnly = false;
				colvarC010.DefaultSetting = @"";
				colvarC010.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC010);
				
				TableSchema.TableColumn colvarC005 = new TableSchema.TableColumn(schema);
				colvarC005.ColumnName = "c005";
				colvarC005.DataType = DbType.AnsiString;
				colvarC005.MaxLength = 50;
				colvarC005.AutoIncrement = false;
				colvarC005.IsNullable = false;
				colvarC005.IsPrimaryKey = false;
				colvarC005.IsForeignKey = false;
				colvarC005.IsReadOnly = false;
				colvarC005.DefaultSetting = @"";
				colvarC005.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC005);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("SavedClosing",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("SavedID")]
		[Bindable(true)]
		public int SavedID 
		{
			get { return GetColumnValue<int>(Columns.SavedID); }
			set { SetColumnValue(Columns.SavedID, value); }
		}
		  
		[XmlAttribute("FloatBalance")]
		[Bindable(true)]
		public string FloatBalance 
		{
			get { return GetColumnValue<string>(Columns.FloatBalance); }
			set { SetColumnValue(Columns.FloatBalance, value); }
		}
		  
		[XmlAttribute("NetsCollected")]
		[Bindable(true)]
		public string NetsCollected 
		{
			get { return GetColumnValue<string>(Columns.NetsCollected); }
			set { SetColumnValue(Columns.NetsCollected, value); }
		}
		  
		[XmlAttribute("NetsTerminalID")]
		[Bindable(true)]
		public string NetsTerminalID 
		{
			get { return GetColumnValue<string>(Columns.NetsTerminalID); }
			set { SetColumnValue(Columns.NetsTerminalID, value); }
		}
		  
		[XmlAttribute("VisaCollected")]
		[Bindable(true)]
		public string VisaCollected 
		{
			get { return GetColumnValue<string>(Columns.VisaCollected); }
			set { SetColumnValue(Columns.VisaCollected, value); }
		}
		  
		[XmlAttribute("VisaBatchNo")]
		[Bindable(true)]
		public string VisaBatchNo 
		{
			get { return GetColumnValue<string>(Columns.VisaBatchNo); }
			set { SetColumnValue(Columns.VisaBatchNo, value); }
		}
		  
		[XmlAttribute("AmexCollected")]
		[Bindable(true)]
		public string AmexCollected 
		{
			get { return GetColumnValue<string>(Columns.AmexCollected); }
			set { SetColumnValue(Columns.AmexCollected, value); }
		}
		  
		[XmlAttribute("AmexBatchNo")]
		[Bindable(true)]
		public string AmexBatchNo 
		{
			get { return GetColumnValue<string>(Columns.AmexBatchNo); }
			set { SetColumnValue(Columns.AmexBatchNo, value); }
		}
		  
		[XmlAttribute("ChinaNetsCollected")]
		[Bindable(true)]
		public string ChinaNetsCollected 
		{
			get { return GetColumnValue<string>(Columns.ChinaNetsCollected); }
			set { SetColumnValue(Columns.ChinaNetsCollected, value); }
		}
		  
		[XmlAttribute("ChinaNetsTerminalID")]
		[Bindable(true)]
		public string ChinaNetsTerminalID 
		{
			get { return GetColumnValue<string>(Columns.ChinaNetsTerminalID); }
			set { SetColumnValue(Columns.ChinaNetsTerminalID, value); }
		}
		  
		[XmlAttribute("VoucherCollected")]
		[Bindable(true)]
		public string VoucherCollected 
		{
			get { return GetColumnValue<string>(Columns.VoucherCollected); }
			set { SetColumnValue(Columns.VoucherCollected, value); }
		}
		  
		[XmlAttribute("ChequeCollected")]
		[Bindable(true)]
		public string ChequeCollected 
		{
			get { return GetColumnValue<string>(Columns.ChequeCollected); }
			set { SetColumnValue(Columns.ChequeCollected, value); }
		}
		  
		[XmlAttribute("DepositBagNo")]
		[Bindable(true)]
		public string DepositBagNo 
		{
			get { return GetColumnValue<string>(Columns.DepositBagNo); }
			set { SetColumnValue(Columns.DepositBagNo, value); }
		}
		  
		[XmlAttribute("PointOfSaleID")]
		[Bindable(true)]
		public string PointOfSaleID 
		{
			get { return GetColumnValue<string>(Columns.PointOfSaleID); }
			set { SetColumnValue(Columns.PointOfSaleID, value); }
		}
		  
		[XmlAttribute("C100")]
		[Bindable(true)]
		public string C100 
		{
			get { return GetColumnValue<string>(Columns.C100); }
			set { SetColumnValue(Columns.C100, value); }
		}
		  
		[XmlAttribute("C50")]
		[Bindable(true)]
		public string C50 
		{
			get { return GetColumnValue<string>(Columns.C50); }
			set { SetColumnValue(Columns.C50, value); }
		}
		  
		[XmlAttribute("C10")]
		[Bindable(true)]
		public string C10 
		{
			get { return GetColumnValue<string>(Columns.C10); }
			set { SetColumnValue(Columns.C10, value); }
		}
		  
		[XmlAttribute("C5")]
		[Bindable(true)]
		public string C5 
		{
			get { return GetColumnValue<string>(Columns.C5); }
			set { SetColumnValue(Columns.C5, value); }
		}
		  
		[XmlAttribute("C2")]
		[Bindable(true)]
		public string C2 
		{
			get { return GetColumnValue<string>(Columns.C2); }
			set { SetColumnValue(Columns.C2, value); }
		}
		  
		[XmlAttribute("C1")]
		[Bindable(true)]
		public string C1 
		{
			get { return GetColumnValue<string>(Columns.C1); }
			set { SetColumnValue(Columns.C1, value); }
		}
		  
		[XmlAttribute("C050")]
		[Bindable(true)]
		public string C050 
		{
			get { return GetColumnValue<string>(Columns.C050); }
			set { SetColumnValue(Columns.C050, value); }
		}
		  
		[XmlAttribute("C020")]
		[Bindable(true)]
		public string C020 
		{
			get { return GetColumnValue<string>(Columns.C020); }
			set { SetColumnValue(Columns.C020, value); }
		}
		  
		[XmlAttribute("C010")]
		[Bindable(true)]
		public string C010 
		{
			get { return GetColumnValue<string>(Columns.C010); }
			set { SetColumnValue(Columns.C010, value); }
		}
		  
		[XmlAttribute("C005")]
		[Bindable(true)]
		public string C005 
		{
			get { return GetColumnValue<string>(Columns.C005); }
			set { SetColumnValue(Columns.C005, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varFloatBalance,string varNetsCollected,string varNetsTerminalID,string varVisaCollected,string varVisaBatchNo,string varAmexCollected,string varAmexBatchNo,string varChinaNetsCollected,string varChinaNetsTerminalID,string varVoucherCollected,string varChequeCollected,string varDepositBagNo,string varPointOfSaleID,string varC100,string varC50,string varC10,string varC5,string varC2,string varC1,string varC050,string varC020,string varC010,string varC005)
		{
			SavedClosing item = new SavedClosing();
			
			item.FloatBalance = varFloatBalance;
			
			item.NetsCollected = varNetsCollected;
			
			item.NetsTerminalID = varNetsTerminalID;
			
			item.VisaCollected = varVisaCollected;
			
			item.VisaBatchNo = varVisaBatchNo;
			
			item.AmexCollected = varAmexCollected;
			
			item.AmexBatchNo = varAmexBatchNo;
			
			item.ChinaNetsCollected = varChinaNetsCollected;
			
			item.ChinaNetsTerminalID = varChinaNetsTerminalID;
			
			item.VoucherCollected = varVoucherCollected;
			
			item.ChequeCollected = varChequeCollected;
			
			item.DepositBagNo = varDepositBagNo;
			
			item.PointOfSaleID = varPointOfSaleID;
			
			item.C100 = varC100;
			
			item.C50 = varC50;
			
			item.C10 = varC10;
			
			item.C5 = varC5;
			
			item.C2 = varC2;
			
			item.C1 = varC1;
			
			item.C050 = varC050;
			
			item.C020 = varC020;
			
			item.C010 = varC010;
			
			item.C005 = varC005;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varSavedID,string varFloatBalance,string varNetsCollected,string varNetsTerminalID,string varVisaCollected,string varVisaBatchNo,string varAmexCollected,string varAmexBatchNo,string varChinaNetsCollected,string varChinaNetsTerminalID,string varVoucherCollected,string varChequeCollected,string varDepositBagNo,string varPointOfSaleID,string varC100,string varC50,string varC10,string varC5,string varC2,string varC1,string varC050,string varC020,string varC010,string varC005)
		{
			SavedClosing item = new SavedClosing();
			
				item.SavedID = varSavedID;
			
				item.FloatBalance = varFloatBalance;
			
				item.NetsCollected = varNetsCollected;
			
				item.NetsTerminalID = varNetsTerminalID;
			
				item.VisaCollected = varVisaCollected;
			
				item.VisaBatchNo = varVisaBatchNo;
			
				item.AmexCollected = varAmexCollected;
			
				item.AmexBatchNo = varAmexBatchNo;
			
				item.ChinaNetsCollected = varChinaNetsCollected;
			
				item.ChinaNetsTerminalID = varChinaNetsTerminalID;
			
				item.VoucherCollected = varVoucherCollected;
			
				item.ChequeCollected = varChequeCollected;
			
				item.DepositBagNo = varDepositBagNo;
			
				item.PointOfSaleID = varPointOfSaleID;
			
				item.C100 = varC100;
			
				item.C50 = varC50;
			
				item.C10 = varC10;
			
				item.C5 = varC5;
			
				item.C2 = varC2;
			
				item.C1 = varC1;
			
				item.C050 = varC050;
			
				item.C020 = varC020;
			
				item.C010 = varC010;
			
				item.C005 = varC005;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn SavedIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn FloatBalanceColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsCollectedColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsTerminalIDColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn VisaCollectedColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn VisaBatchNoColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn AmexCollectedColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn AmexBatchNoColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ChinaNetsCollectedColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ChinaNetsTerminalIDColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn VoucherCollectedColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn ChequeCollectedColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn DepositBagNoColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn PointOfSaleIDColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn C100Column
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn C50Column
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn C10Column
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn C5Column
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn C2Column
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn C1Column
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn C050Column
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn C020Column
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn C010Column
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn C005Column
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string SavedID = @"SavedID";
			 public static string FloatBalance = @"FloatBalance";
			 public static string NetsCollected = @"NetsCollected";
			 public static string NetsTerminalID = @"NetsTerminalID";
			 public static string VisaCollected = @"VisaCollected";
			 public static string VisaBatchNo = @"VisaBatchNo";
			 public static string AmexCollected = @"AmexCollected";
			 public static string AmexBatchNo = @"AmexBatchNo";
			 public static string ChinaNetsCollected = @"ChinaNetsCollected";
			 public static string ChinaNetsTerminalID = @"ChinaNetsTerminalID";
			 public static string VoucherCollected = @"VoucherCollected";
			 public static string ChequeCollected = @"ChequeCollected";
			 public static string DepositBagNo = @"DepositBagNo";
			 public static string PointOfSaleID = @"PointOfSaleID";
			 public static string C100 = @"c100";
			 public static string C50 = @"c50";
			 public static string C10 = @"c10";
			 public static string C5 = @"c5";
			 public static string C2 = @"c2";
			 public static string C1 = @"c1";
			 public static string C050 = @"c050";
			 public static string C020 = @"c020";
			 public static string C010 = @"c010";
			 public static string C005 = @"c005";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
