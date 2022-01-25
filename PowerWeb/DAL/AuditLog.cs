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
	/// Strongly-typed collection for the AuditLog class.
	/// </summary>
    [Serializable]
	public partial class AuditLogCollection : ActiveList<AuditLog, AuditLogCollection>
	{	   
		public AuditLogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AuditLogCollection</returns>
		public AuditLogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AuditLog o = this[i];
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
	/// This is an ActiveRecord class which wraps the AuditLog table.
	/// </summary>
	[Serializable]
	public partial class AuditLog : ActiveRecord<AuditLog>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AuditLog()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AuditLog(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AuditLog(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AuditLog(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("AuditLog", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarAuditLogID = new TableSchema.TableColumn(schema);
				colvarAuditLogID.ColumnName = "AuditLogID";
				colvarAuditLogID.DataType = DbType.Guid;
				colvarAuditLogID.MaxLength = 0;
				colvarAuditLogID.AutoIncrement = false;
				colvarAuditLogID.IsNullable = false;
				colvarAuditLogID.IsPrimaryKey = true;
				colvarAuditLogID.IsForeignKey = false;
				colvarAuditLogID.IsReadOnly = false;
				colvarAuditLogID.DefaultSetting = @"";
				colvarAuditLogID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAuditLogID);
				
				TableSchema.TableColumn colvarLogDate = new TableSchema.TableColumn(schema);
				colvarLogDate.ColumnName = "LogDate";
				colvarLogDate.DataType = DbType.DateTime;
				colvarLogDate.MaxLength = 0;
				colvarLogDate.AutoIncrement = false;
				colvarLogDate.IsNullable = true;
				colvarLogDate.IsPrimaryKey = false;
				colvarLogDate.IsForeignKey = false;
				colvarLogDate.IsReadOnly = false;
				colvarLogDate.DefaultSetting = @"";
				colvarLogDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLogDate);
				
				TableSchema.TableColumn colvarOperation = new TableSchema.TableColumn(schema);
				colvarOperation.ColumnName = "Operation";
				colvarOperation.DataType = DbType.AnsiString;
				colvarOperation.MaxLength = -1;
				colvarOperation.AutoIncrement = false;
				colvarOperation.IsNullable = true;
				colvarOperation.IsPrimaryKey = false;
				colvarOperation.IsForeignKey = false;
				colvarOperation.IsReadOnly = false;
				colvarOperation.DefaultSetting = @"";
				colvarOperation.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOperation);
				
				TableSchema.TableColumn colvarTableName = new TableSchema.TableColumn(schema);
				colvarTableName.ColumnName = "TableName";
				colvarTableName.DataType = DbType.AnsiString;
				colvarTableName.MaxLength = -1;
				colvarTableName.AutoIncrement = false;
				colvarTableName.IsNullable = true;
				colvarTableName.IsPrimaryKey = false;
				colvarTableName.IsForeignKey = false;
				colvarTableName.IsReadOnly = false;
				colvarTableName.DefaultSetting = @"";
				colvarTableName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTableName);
				
				TableSchema.TableColumn colvarPrimaryKeyCol = new TableSchema.TableColumn(schema);
				colvarPrimaryKeyCol.ColumnName = "PrimaryKeyCol";
				colvarPrimaryKeyCol.DataType = DbType.AnsiString;
				colvarPrimaryKeyCol.MaxLength = -1;
				colvarPrimaryKeyCol.AutoIncrement = false;
				colvarPrimaryKeyCol.IsNullable = true;
				colvarPrimaryKeyCol.IsPrimaryKey = false;
				colvarPrimaryKeyCol.IsForeignKey = false;
				colvarPrimaryKeyCol.IsReadOnly = false;
				colvarPrimaryKeyCol.DefaultSetting = @"";
				colvarPrimaryKeyCol.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPrimaryKeyCol);
				
				TableSchema.TableColumn colvarPrimaryKeyVal = new TableSchema.TableColumn(schema);
				colvarPrimaryKeyVal.ColumnName = "PrimaryKeyVal";
				colvarPrimaryKeyVal.DataType = DbType.AnsiString;
				colvarPrimaryKeyVal.MaxLength = -1;
				colvarPrimaryKeyVal.AutoIncrement = false;
				colvarPrimaryKeyVal.IsNullable = true;
				colvarPrimaryKeyVal.IsPrimaryKey = false;
				colvarPrimaryKeyVal.IsForeignKey = false;
				colvarPrimaryKeyVal.IsReadOnly = false;
				colvarPrimaryKeyVal.DefaultSetting = @"";
				colvarPrimaryKeyVal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPrimaryKeyVal);
				
				TableSchema.TableColumn colvarOldValues = new TableSchema.TableColumn(schema);
				colvarOldValues.ColumnName = "OldValues";
				colvarOldValues.DataType = DbType.String;
				colvarOldValues.MaxLength = -1;
				colvarOldValues.AutoIncrement = false;
				colvarOldValues.IsNullable = true;
				colvarOldValues.IsPrimaryKey = false;
				colvarOldValues.IsForeignKey = false;
				colvarOldValues.IsReadOnly = false;
				colvarOldValues.DefaultSetting = @"";
				colvarOldValues.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOldValues);
				
				TableSchema.TableColumn colvarNewValues = new TableSchema.TableColumn(schema);
				colvarNewValues.ColumnName = "NewValues";
				colvarNewValues.DataType = DbType.String;
				colvarNewValues.MaxLength = -1;
				colvarNewValues.AutoIncrement = false;
				colvarNewValues.IsNullable = true;
				colvarNewValues.IsPrimaryKey = false;
				colvarNewValues.IsForeignKey = false;
				colvarNewValues.IsReadOnly = false;
				colvarNewValues.DefaultSetting = @"";
				colvarNewValues.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNewValues);
				
				TableSchema.TableColumn colvarRemarks = new TableSchema.TableColumn(schema);
				colvarRemarks.ColumnName = "Remarks";
				colvarRemarks.DataType = DbType.String;
				colvarRemarks.MaxLength = -1;
				colvarRemarks.AutoIncrement = false;
				colvarRemarks.IsNullable = true;
				colvarRemarks.IsPrimaryKey = false;
				colvarRemarks.IsForeignKey = false;
				colvarRemarks.IsReadOnly = false;
				colvarRemarks.DefaultSetting = @"";
				colvarRemarks.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemarks);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("AuditLog",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("AuditLogID")]
		[Bindable(true)]
		public Guid AuditLogID 
		{
			get { return GetColumnValue<Guid>(Columns.AuditLogID); }
			set { SetColumnValue(Columns.AuditLogID, value); }
		}
		  
		[XmlAttribute("LogDate")]
		[Bindable(true)]
		public DateTime? LogDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.LogDate); }
			set { SetColumnValue(Columns.LogDate, value); }
		}
		  
		[XmlAttribute("Operation")]
		[Bindable(true)]
		public string Operation 
		{
			get { return GetColumnValue<string>(Columns.Operation); }
			set { SetColumnValue(Columns.Operation, value); }
		}
		  
		[XmlAttribute("TableName")]
		[Bindable(true)]
		public string TableName 
		{
			get { return GetColumnValue<string>(Columns.TableName); }
			set { SetColumnValue(Columns.TableName, value); }
		}
		  
		[XmlAttribute("PrimaryKeyCol")]
		[Bindable(true)]
		public string PrimaryKeyCol 
		{
			get { return GetColumnValue<string>(Columns.PrimaryKeyCol); }
			set { SetColumnValue(Columns.PrimaryKeyCol, value); }
		}
		  
		[XmlAttribute("PrimaryKeyVal")]
		[Bindable(true)]
		public string PrimaryKeyVal 
		{
			get { return GetColumnValue<string>(Columns.PrimaryKeyVal); }
			set { SetColumnValue(Columns.PrimaryKeyVal, value); }
		}
		  
		[XmlAttribute("OldValues")]
		[Bindable(true)]
		public string OldValues 
		{
			get { return GetColumnValue<string>(Columns.OldValues); }
			set { SetColumnValue(Columns.OldValues, value); }
		}
		  
		[XmlAttribute("NewValues")]
		[Bindable(true)]
		public string NewValues 
		{
			get { return GetColumnValue<string>(Columns.NewValues); }
			set { SetColumnValue(Columns.NewValues, value); }
		}
		  
		[XmlAttribute("Remarks")]
		[Bindable(true)]
		public string Remarks 
		{
			get { return GetColumnValue<string>(Columns.Remarks); }
			set { SetColumnValue(Columns.Remarks, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varAuditLogID,DateTime? varLogDate,string varOperation,string varTableName,string varPrimaryKeyCol,string varPrimaryKeyVal,string varOldValues,string varNewValues,string varRemarks)
		{
			AuditLog item = new AuditLog();
			
			item.AuditLogID = varAuditLogID;
			
			item.LogDate = varLogDate;
			
			item.Operation = varOperation;
			
			item.TableName = varTableName;
			
			item.PrimaryKeyCol = varPrimaryKeyCol;
			
			item.PrimaryKeyVal = varPrimaryKeyVal;
			
			item.OldValues = varOldValues;
			
			item.NewValues = varNewValues;
			
			item.Remarks = varRemarks;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varAuditLogID,DateTime? varLogDate,string varOperation,string varTableName,string varPrimaryKeyCol,string varPrimaryKeyVal,string varOldValues,string varNewValues,string varRemarks)
		{
			AuditLog item = new AuditLog();
			
				item.AuditLogID = varAuditLogID;
			
				item.LogDate = varLogDate;
			
				item.Operation = varOperation;
			
				item.TableName = varTableName;
			
				item.PrimaryKeyCol = varPrimaryKeyCol;
			
				item.PrimaryKeyVal = varPrimaryKeyVal;
			
				item.OldValues = varOldValues;
			
				item.NewValues = varNewValues;
			
				item.Remarks = varRemarks;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn AuditLogIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn LogDateColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn OperationColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TableNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn PrimaryKeyColColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn PrimaryKeyValColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn OldValuesColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn NewValuesColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarksColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string AuditLogID = @"AuditLogID";
			 public static string LogDate = @"LogDate";
			 public static string Operation = @"Operation";
			 public static string TableName = @"TableName";
			 public static string PrimaryKeyCol = @"PrimaryKeyCol";
			 public static string PrimaryKeyVal = @"PrimaryKeyVal";
			 public static string OldValues = @"OldValues";
			 public static string NewValues = @"NewValues";
			 public static string Remarks = @"Remarks";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
