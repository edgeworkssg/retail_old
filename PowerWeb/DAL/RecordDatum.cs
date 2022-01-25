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
	/// Strongly-typed collection for the RecordDatum class.
	/// </summary>
    [Serializable]
	public partial class RecordDatumCollection : ActiveList<RecordDatum, RecordDatumCollection>
	{	   
		public RecordDatumCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>RecordDatumCollection</returns>
		public RecordDatumCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                RecordDatum o = this[i];
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
	/// This is an ActiveRecord class which wraps the RecordData table.
	/// </summary>
	[Serializable]
	public partial class RecordDatum : ActiveRecord<RecordDatum>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public RecordDatum()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public RecordDatum(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public RecordDatum(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public RecordDatum(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("RecordData", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarRecordDataID = new TableSchema.TableColumn(schema);
				colvarRecordDataID.ColumnName = "RecordDataID";
				colvarRecordDataID.DataType = DbType.Int32;
				colvarRecordDataID.MaxLength = 0;
				colvarRecordDataID.AutoIncrement = true;
				colvarRecordDataID.IsNullable = false;
				colvarRecordDataID.IsPrimaryKey = true;
				colvarRecordDataID.IsForeignKey = false;
				colvarRecordDataID.IsReadOnly = false;
				colvarRecordDataID.DefaultSetting = @"";
				colvarRecordDataID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRecordDataID);
				
				TableSchema.TableColumn colvarInventoryLocationID = new TableSchema.TableColumn(schema);
				colvarInventoryLocationID.ColumnName = "InventoryLocationID";
				colvarInventoryLocationID.DataType = DbType.Int32;
				colvarInventoryLocationID.MaxLength = 0;
				colvarInventoryLocationID.AutoIncrement = false;
				colvarInventoryLocationID.IsNullable = true;
				colvarInventoryLocationID.IsPrimaryKey = false;
				colvarInventoryLocationID.IsForeignKey = false;
				colvarInventoryLocationID.IsReadOnly = false;
				colvarInventoryLocationID.DefaultSetting = @"";
				colvarInventoryLocationID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInventoryLocationID);
				
				TableSchema.TableColumn colvarVal1 = new TableSchema.TableColumn(schema);
				colvarVal1.ColumnName = "Val1";
				colvarVal1.DataType = DbType.AnsiString;
				colvarVal1.MaxLength = 50;
				colvarVal1.AutoIncrement = false;
				colvarVal1.IsNullable = true;
				colvarVal1.IsPrimaryKey = false;
				colvarVal1.IsForeignKey = false;
				colvarVal1.IsReadOnly = false;
				colvarVal1.DefaultSetting = @"";
				colvarVal1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVal1);
				
				TableSchema.TableColumn colvarVal2 = new TableSchema.TableColumn(schema);
				colvarVal2.ColumnName = "Val2";
				colvarVal2.DataType = DbType.AnsiString;
				colvarVal2.MaxLength = 50;
				colvarVal2.AutoIncrement = false;
				colvarVal2.IsNullable = true;
				colvarVal2.IsPrimaryKey = false;
				colvarVal2.IsForeignKey = false;
				colvarVal2.IsReadOnly = false;
				colvarVal2.DefaultSetting = @"";
				colvarVal2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVal2);
				
				TableSchema.TableColumn colvarVal3 = new TableSchema.TableColumn(schema);
				colvarVal3.ColumnName = "Val3";
				colvarVal3.DataType = DbType.AnsiString;
				colvarVal3.MaxLength = 50;
				colvarVal3.AutoIncrement = false;
				colvarVal3.IsNullable = true;
				colvarVal3.IsPrimaryKey = false;
				colvarVal3.IsForeignKey = false;
				colvarVal3.IsReadOnly = false;
				colvarVal3.DefaultSetting = @"";
				colvarVal3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVal3);
				
				TableSchema.TableColumn colvarVal4 = new TableSchema.TableColumn(schema);
				colvarVal4.ColumnName = "Val4";
				colvarVal4.DataType = DbType.AnsiString;
				colvarVal4.MaxLength = 50;
				colvarVal4.AutoIncrement = false;
				colvarVal4.IsNullable = true;
				colvarVal4.IsPrimaryKey = false;
				colvarVal4.IsForeignKey = false;
				colvarVal4.IsReadOnly = false;
				colvarVal4.DefaultSetting = @"";
				colvarVal4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVal4);
				
				TableSchema.TableColumn colvarVal5 = new TableSchema.TableColumn(schema);
				colvarVal5.ColumnName = "Val5";
				colvarVal5.DataType = DbType.AnsiString;
				colvarVal5.MaxLength = 50;
				colvarVal5.AutoIncrement = false;
				colvarVal5.IsNullable = true;
				colvarVal5.IsPrimaryKey = false;
				colvarVal5.IsForeignKey = false;
				colvarVal5.IsReadOnly = false;
				colvarVal5.DefaultSetting = @"";
				colvarVal5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVal5);
				
				TableSchema.TableColumn colvarVal6 = new TableSchema.TableColumn(schema);
				colvarVal6.ColumnName = "Val6";
				colvarVal6.DataType = DbType.AnsiString;
				colvarVal6.MaxLength = 50;
				colvarVal6.AutoIncrement = false;
				colvarVal6.IsNullable = true;
				colvarVal6.IsPrimaryKey = false;
				colvarVal6.IsForeignKey = false;
				colvarVal6.IsReadOnly = false;
				colvarVal6.DefaultSetting = @"";
				colvarVal6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVal6);
				
				TableSchema.TableColumn colvarVal7 = new TableSchema.TableColumn(schema);
				colvarVal7.ColumnName = "Val7";
				colvarVal7.DataType = DbType.AnsiString;
				colvarVal7.MaxLength = 50;
				colvarVal7.AutoIncrement = false;
				colvarVal7.IsNullable = true;
				colvarVal7.IsPrimaryKey = false;
				colvarVal7.IsForeignKey = false;
				colvarVal7.IsReadOnly = false;
				colvarVal7.DefaultSetting = @"";
				colvarVal7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVal7);
				
				TableSchema.TableColumn colvarVal8 = new TableSchema.TableColumn(schema);
				colvarVal8.ColumnName = "Val8";
				colvarVal8.DataType = DbType.AnsiString;
				colvarVal8.MaxLength = 50;
				colvarVal8.AutoIncrement = false;
				colvarVal8.IsNullable = true;
				colvarVal8.IsPrimaryKey = false;
				colvarVal8.IsForeignKey = false;
				colvarVal8.IsReadOnly = false;
				colvarVal8.DefaultSetting = @"";
				colvarVal8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVal8);
				
				TableSchema.TableColumn colvarVal9 = new TableSchema.TableColumn(schema);
				colvarVal9.ColumnName = "Val9";
				colvarVal9.DataType = DbType.AnsiString;
				colvarVal9.MaxLength = 50;
				colvarVal9.AutoIncrement = false;
				colvarVal9.IsNullable = true;
				colvarVal9.IsPrimaryKey = false;
				colvarVal9.IsForeignKey = false;
				colvarVal9.IsReadOnly = false;
				colvarVal9.DefaultSetting = @"";
				colvarVal9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVal9);
				
				TableSchema.TableColumn colvarVal10 = new TableSchema.TableColumn(schema);
				colvarVal10.ColumnName = "Val10";
				colvarVal10.DataType = DbType.AnsiString;
				colvarVal10.MaxLength = 50;
				colvarVal10.AutoIncrement = false;
				colvarVal10.IsNullable = true;
				colvarVal10.IsPrimaryKey = false;
				colvarVal10.IsForeignKey = false;
				colvarVal10.IsReadOnly = false;
				colvarVal10.DefaultSetting = @"";
				colvarVal10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVal10);
				
				TableSchema.TableColumn colvarInventoryHdrRefNo = new TableSchema.TableColumn(schema);
				colvarInventoryHdrRefNo.ColumnName = "InventoryHdrRefNo";
				colvarInventoryHdrRefNo.DataType = DbType.AnsiString;
				colvarInventoryHdrRefNo.MaxLength = 50;
				colvarInventoryHdrRefNo.AutoIncrement = false;
				colvarInventoryHdrRefNo.IsNullable = true;
				colvarInventoryHdrRefNo.IsPrimaryKey = false;
				colvarInventoryHdrRefNo.IsForeignKey = false;
				colvarInventoryHdrRefNo.IsReadOnly = false;
				colvarInventoryHdrRefNo.DefaultSetting = @"";
				colvarInventoryHdrRefNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInventoryHdrRefNo);
				
				TableSchema.TableColumn colvarTimestamp = new TableSchema.TableColumn(schema);
				colvarTimestamp.ColumnName = "Timestamp";
				colvarTimestamp.DataType = DbType.DateTime;
				colvarTimestamp.MaxLength = 0;
				colvarTimestamp.AutoIncrement = false;
				colvarTimestamp.IsNullable = true;
				colvarTimestamp.IsPrimaryKey = false;
				colvarTimestamp.IsForeignKey = false;
				colvarTimestamp.IsReadOnly = false;
				colvarTimestamp.DefaultSetting = @"";
				colvarTimestamp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTimestamp);
				
				TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
				colvarCreatedOn.ColumnName = "CreatedOn";
				colvarCreatedOn.DataType = DbType.DateTime;
				colvarCreatedOn.MaxLength = 0;
				colvarCreatedOn.AutoIncrement = false;
				colvarCreatedOn.IsNullable = true;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				colvarCreatedOn.DefaultSetting = @"";
				colvarCreatedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedOn);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.AnsiString;
				colvarCreatedBy.MaxLength = 50;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = true;
				colvarCreatedBy.IsPrimaryKey = false;
				colvarCreatedBy.IsForeignKey = false;
				colvarCreatedBy.IsReadOnly = false;
				colvarCreatedBy.DefaultSetting = @"";
				colvarCreatedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedBy);
				
				TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
				colvarModifiedOn.ColumnName = "ModifiedOn";
				colvarModifiedOn.DataType = DbType.DateTime;
				colvarModifiedOn.MaxLength = 0;
				colvarModifiedOn.AutoIncrement = false;
				colvarModifiedOn.IsNullable = true;
				colvarModifiedOn.IsPrimaryKey = false;
				colvarModifiedOn.IsForeignKey = false;
				colvarModifiedOn.IsReadOnly = false;
				colvarModifiedOn.DefaultSetting = @"";
				colvarModifiedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedOn);
				
				TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
				colvarModifiedBy.ColumnName = "ModifiedBy";
				colvarModifiedBy.DataType = DbType.AnsiString;
				colvarModifiedBy.MaxLength = 50;
				colvarModifiedBy.AutoIncrement = false;
				colvarModifiedBy.IsNullable = true;
				colvarModifiedBy.IsPrimaryKey = false;
				colvarModifiedBy.IsForeignKey = false;
				colvarModifiedBy.IsReadOnly = false;
				colvarModifiedBy.DefaultSetting = @"";
				colvarModifiedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedBy);
				
				TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
				colvarDeleted.ColumnName = "Deleted";
				colvarDeleted.DataType = DbType.Boolean;
				colvarDeleted.MaxLength = 0;
				colvarDeleted.AutoIncrement = false;
				colvarDeleted.IsNullable = true;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				colvarDeleted.DefaultSetting = @"";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				TableSchema.TableColumn colvarUniqueId = new TableSchema.TableColumn(schema);
				colvarUniqueId.ColumnName = "UniqueId";
				colvarUniqueId.DataType = DbType.AnsiString;
				colvarUniqueId.MaxLength = 100;
				colvarUniqueId.AutoIncrement = false;
				colvarUniqueId.IsNullable = true;
				colvarUniqueId.IsPrimaryKey = false;
				colvarUniqueId.IsForeignKey = false;
				colvarUniqueId.IsReadOnly = false;
				colvarUniqueId.DefaultSetting = @"";
				colvarUniqueId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueId);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("RecordData",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("RecordDataID")]
		[Bindable(true)]
		public int RecordDataID 
		{
			get { return GetColumnValue<int>(Columns.RecordDataID); }
			set { SetColumnValue(Columns.RecordDataID, value); }
		}
		  
		[XmlAttribute("InventoryLocationID")]
		[Bindable(true)]
		public int? InventoryLocationID 
		{
			get { return GetColumnValue<int?>(Columns.InventoryLocationID); }
			set { SetColumnValue(Columns.InventoryLocationID, value); }
		}
		  
		[XmlAttribute("Val1")]
		[Bindable(true)]
		public string Val1 
		{
			get { return GetColumnValue<string>(Columns.Val1); }
			set { SetColumnValue(Columns.Val1, value); }
		}
		  
		[XmlAttribute("Val2")]
		[Bindable(true)]
		public string Val2 
		{
			get { return GetColumnValue<string>(Columns.Val2); }
			set { SetColumnValue(Columns.Val2, value); }
		}
		  
		[XmlAttribute("Val3")]
		[Bindable(true)]
		public string Val3 
		{
			get { return GetColumnValue<string>(Columns.Val3); }
			set { SetColumnValue(Columns.Val3, value); }
		}
		  
		[XmlAttribute("Val4")]
		[Bindable(true)]
		public string Val4 
		{
			get { return GetColumnValue<string>(Columns.Val4); }
			set { SetColumnValue(Columns.Val4, value); }
		}
		  
		[XmlAttribute("Val5")]
		[Bindable(true)]
		public string Val5 
		{
			get { return GetColumnValue<string>(Columns.Val5); }
			set { SetColumnValue(Columns.Val5, value); }
		}
		  
		[XmlAttribute("Val6")]
		[Bindable(true)]
		public string Val6 
		{
			get { return GetColumnValue<string>(Columns.Val6); }
			set { SetColumnValue(Columns.Val6, value); }
		}
		  
		[XmlAttribute("Val7")]
		[Bindable(true)]
		public string Val7 
		{
			get { return GetColumnValue<string>(Columns.Val7); }
			set { SetColumnValue(Columns.Val7, value); }
		}
		  
		[XmlAttribute("Val8")]
		[Bindable(true)]
		public string Val8 
		{
			get { return GetColumnValue<string>(Columns.Val8); }
			set { SetColumnValue(Columns.Val8, value); }
		}
		  
		[XmlAttribute("Val9")]
		[Bindable(true)]
		public string Val9 
		{
			get { return GetColumnValue<string>(Columns.Val9); }
			set { SetColumnValue(Columns.Val9, value); }
		}
		  
		[XmlAttribute("Val10")]
		[Bindable(true)]
		public string Val10 
		{
			get { return GetColumnValue<string>(Columns.Val10); }
			set { SetColumnValue(Columns.Val10, value); }
		}
		  
		[XmlAttribute("InventoryHdrRefNo")]
		[Bindable(true)]
		public string InventoryHdrRefNo 
		{
			get { return GetColumnValue<string>(Columns.InventoryHdrRefNo); }
			set { SetColumnValue(Columns.InventoryHdrRefNo, value); }
		}
		  
		[XmlAttribute("Timestamp")]
		[Bindable(true)]
		public DateTime? Timestamp 
		{
			get { return GetColumnValue<DateTime?>(Columns.Timestamp); }
			set { SetColumnValue(Columns.Timestamp, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("UniqueId")]
		[Bindable(true)]
		public string UniqueId 
		{
			get { return GetColumnValue<string>(Columns.UniqueId); }
			set { SetColumnValue(Columns.UniqueId, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int? varInventoryLocationID,string varVal1,string varVal2,string varVal3,string varVal4,string varVal5,string varVal6,string varVal7,string varVal8,string varVal9,string varVal10,string varInventoryHdrRefNo,DateTime? varTimestamp,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varUniqueId)
		{
			RecordDatum item = new RecordDatum();
			
			item.InventoryLocationID = varInventoryLocationID;
			
			item.Val1 = varVal1;
			
			item.Val2 = varVal2;
			
			item.Val3 = varVal3;
			
			item.Val4 = varVal4;
			
			item.Val5 = varVal5;
			
			item.Val6 = varVal6;
			
			item.Val7 = varVal7;
			
			item.Val8 = varVal8;
			
			item.Val9 = varVal9;
			
			item.Val10 = varVal10;
			
			item.InventoryHdrRefNo = varInventoryHdrRefNo;
			
			item.Timestamp = varTimestamp;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.UniqueId = varUniqueId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varRecordDataID,int? varInventoryLocationID,string varVal1,string varVal2,string varVal3,string varVal4,string varVal5,string varVal6,string varVal7,string varVal8,string varVal9,string varVal10,string varInventoryHdrRefNo,DateTime? varTimestamp,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varUniqueId)
		{
			RecordDatum item = new RecordDatum();
			
				item.RecordDataID = varRecordDataID;
			
				item.InventoryLocationID = varInventoryLocationID;
			
				item.Val1 = varVal1;
			
				item.Val2 = varVal2;
			
				item.Val3 = varVal3;
			
				item.Val4 = varVal4;
			
				item.Val5 = varVal5;
			
				item.Val6 = varVal6;
			
				item.Val7 = varVal7;
			
				item.Val8 = varVal8;
			
				item.Val9 = varVal9;
			
				item.Val10 = varVal10;
			
				item.InventoryHdrRefNo = varInventoryHdrRefNo;
			
				item.Timestamp = varTimestamp;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.UniqueId = varUniqueId;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn RecordDataIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn InventoryLocationIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn Val1Column
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn Val2Column
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn Val3Column
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn Val4Column
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn Val5Column
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn Val6Column
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn Val7Column
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn Val8Column
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn Val9Column
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn Val10Column
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn InventoryHdrRefNoColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn TimestampColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIdColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string RecordDataID = @"RecordDataID";
			 public static string InventoryLocationID = @"InventoryLocationID";
			 public static string Val1 = @"Val1";
			 public static string Val2 = @"Val2";
			 public static string Val3 = @"Val3";
			 public static string Val4 = @"Val4";
			 public static string Val5 = @"Val5";
			 public static string Val6 = @"Val6";
			 public static string Val7 = @"Val7";
			 public static string Val8 = @"Val8";
			 public static string Val9 = @"Val9";
			 public static string Val10 = @"Val10";
			 public static string InventoryHdrRefNo = @"InventoryHdrRefNo";
			 public static string Timestamp = @"Timestamp";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string UniqueId = @"UniqueId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
