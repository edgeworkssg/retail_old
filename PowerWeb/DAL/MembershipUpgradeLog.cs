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
	/// Strongly-typed collection for the MembershipUpgradeLog class.
	/// </summary>
    [Serializable]
	public partial class MembershipUpgradeLogCollection : ActiveList<MembershipUpgradeLog, MembershipUpgradeLogCollection>
	{	   
		public MembershipUpgradeLogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MembershipUpgradeLogCollection</returns>
		public MembershipUpgradeLogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MembershipUpgradeLog o = this[i];
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
	/// This is an ActiveRecord class which wraps the MembershipUpgradeLog table.
	/// </summary>
	[Serializable]
	public partial class MembershipUpgradeLog : ActiveRecord<MembershipUpgradeLog>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public MembershipUpgradeLog()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public MembershipUpgradeLog(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public MembershipUpgradeLog(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public MembershipUpgradeLog(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("MembershipUpgradeLog", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarOrderHdrID = new TableSchema.TableColumn(schema);
				colvarOrderHdrID.ColumnName = "OrderHdrID";
				colvarOrderHdrID.DataType = DbType.AnsiString;
				colvarOrderHdrID.MaxLength = 50;
				colvarOrderHdrID.AutoIncrement = false;
				colvarOrderHdrID.IsNullable = false;
				colvarOrderHdrID.IsPrimaryKey = true;
				colvarOrderHdrID.IsForeignKey = false;
				colvarOrderHdrID.IsReadOnly = false;
				colvarOrderHdrID.DefaultSetting = @"";
				colvarOrderHdrID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderHdrID);
				
				TableSchema.TableColumn colvarIsVitaMixPrevValue = new TableSchema.TableColumn(schema);
				colvarIsVitaMixPrevValue.ColumnName = "IsVitaMixPrevValue";
				colvarIsVitaMixPrevValue.DataType = DbType.Boolean;
				colvarIsVitaMixPrevValue.MaxLength = 0;
				colvarIsVitaMixPrevValue.AutoIncrement = false;
				colvarIsVitaMixPrevValue.IsNullable = true;
				colvarIsVitaMixPrevValue.IsPrimaryKey = false;
				colvarIsVitaMixPrevValue.IsForeignKey = false;
				colvarIsVitaMixPrevValue.IsReadOnly = false;
				colvarIsVitaMixPrevValue.DefaultSetting = @"";
				colvarIsVitaMixPrevValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsVitaMixPrevValue);
				
				TableSchema.TableColumn colvarIsWaterFilterPrevValue = new TableSchema.TableColumn(schema);
				colvarIsWaterFilterPrevValue.ColumnName = "IsWaterFilterPrevValue";
				colvarIsWaterFilterPrevValue.DataType = DbType.Boolean;
				colvarIsWaterFilterPrevValue.MaxLength = 0;
				colvarIsWaterFilterPrevValue.AutoIncrement = false;
				colvarIsWaterFilterPrevValue.IsNullable = true;
				colvarIsWaterFilterPrevValue.IsPrimaryKey = false;
				colvarIsWaterFilterPrevValue.IsForeignKey = false;
				colvarIsWaterFilterPrevValue.IsReadOnly = false;
				colvarIsWaterFilterPrevValue.DefaultSetting = @"";
				colvarIsWaterFilterPrevValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsWaterFilterPrevValue);
				
				TableSchema.TableColumn colvarIsJuicePlusPrevValue = new TableSchema.TableColumn(schema);
				colvarIsJuicePlusPrevValue.ColumnName = "IsJuicePlusPrevValue";
				colvarIsJuicePlusPrevValue.DataType = DbType.Boolean;
				colvarIsJuicePlusPrevValue.MaxLength = 0;
				colvarIsJuicePlusPrevValue.AutoIncrement = false;
				colvarIsJuicePlusPrevValue.IsNullable = true;
				colvarIsJuicePlusPrevValue.IsPrimaryKey = false;
				colvarIsJuicePlusPrevValue.IsForeignKey = false;
				colvarIsJuicePlusPrevValue.IsReadOnly = false;
				colvarIsJuicePlusPrevValue.DefaultSetting = @"";
				colvarIsJuicePlusPrevValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsJuicePlusPrevValue);
				
				TableSchema.TableColumn colvarIsYoungPrevValue = new TableSchema.TableColumn(schema);
				colvarIsYoungPrevValue.ColumnName = "IsYoungPrevValue";
				colvarIsYoungPrevValue.DataType = DbType.Boolean;
				colvarIsYoungPrevValue.MaxLength = 0;
				colvarIsYoungPrevValue.AutoIncrement = false;
				colvarIsYoungPrevValue.IsNullable = true;
				colvarIsYoungPrevValue.IsPrimaryKey = false;
				colvarIsYoungPrevValue.IsForeignKey = false;
				colvarIsYoungPrevValue.IsReadOnly = false;
				colvarIsYoungPrevValue.DefaultSetting = @"";
				colvarIsYoungPrevValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsYoungPrevValue);
				
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
				
				TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
				colvarUniqueID.ColumnName = "UniqueID";
				colvarUniqueID.DataType = DbType.Guid;
				colvarUniqueID.MaxLength = 0;
				colvarUniqueID.AutoIncrement = false;
				colvarUniqueID.IsNullable = true;
				colvarUniqueID.IsPrimaryKey = false;
				colvarUniqueID.IsForeignKey = false;
				colvarUniqueID.IsReadOnly = false;
				colvarUniqueID.DefaultSetting = @"";
				colvarUniqueID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("MembershipUpgradeLog",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("OrderHdrID")]
		[Bindable(true)]
		public string OrderHdrID 
		{
			get { return GetColumnValue<string>(Columns.OrderHdrID); }
			set { SetColumnValue(Columns.OrderHdrID, value); }
		}
		  
		[XmlAttribute("IsVitaMixPrevValue")]
		[Bindable(true)]
		public bool? IsVitaMixPrevValue 
		{
			get { return GetColumnValue<bool?>(Columns.IsVitaMixPrevValue); }
			set { SetColumnValue(Columns.IsVitaMixPrevValue, value); }
		}
		  
		[XmlAttribute("IsWaterFilterPrevValue")]
		[Bindable(true)]
		public bool? IsWaterFilterPrevValue 
		{
			get { return GetColumnValue<bool?>(Columns.IsWaterFilterPrevValue); }
			set { SetColumnValue(Columns.IsWaterFilterPrevValue, value); }
		}
		  
		[XmlAttribute("IsJuicePlusPrevValue")]
		[Bindable(true)]
		public bool? IsJuicePlusPrevValue 
		{
			get { return GetColumnValue<bool?>(Columns.IsJuicePlusPrevValue); }
			set { SetColumnValue(Columns.IsJuicePlusPrevValue, value); }
		}
		  
		[XmlAttribute("IsYoungPrevValue")]
		[Bindable(true)]
		public bool? IsYoungPrevValue 
		{
			get { return GetColumnValue<bool?>(Columns.IsYoungPrevValue); }
			set { SetColumnValue(Columns.IsYoungPrevValue, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
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
		  
		[XmlAttribute("UniqueID")]
		[Bindable(true)]
		public Guid? UniqueID 
		{
			get { return GetColumnValue<Guid?>(Columns.UniqueID); }
			set { SetColumnValue(Columns.UniqueID, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varOrderHdrID,bool? varIsVitaMixPrevValue,bool? varIsWaterFilterPrevValue,bool? varIsJuicePlusPrevValue,bool? varIsYoungPrevValue,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted,Guid? varUniqueID)
		{
			MembershipUpgradeLog item = new MembershipUpgradeLog();
			
			item.OrderHdrID = varOrderHdrID;
			
			item.IsVitaMixPrevValue = varIsVitaMixPrevValue;
			
			item.IsWaterFilterPrevValue = varIsWaterFilterPrevValue;
			
			item.IsJuicePlusPrevValue = varIsJuicePlusPrevValue;
			
			item.IsYoungPrevValue = varIsYoungPrevValue;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedOn = varModifiedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.UniqueID = varUniqueID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varOrderHdrID,bool? varIsVitaMixPrevValue,bool? varIsWaterFilterPrevValue,bool? varIsJuicePlusPrevValue,bool? varIsYoungPrevValue,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted,Guid? varUniqueID)
		{
			MembershipUpgradeLog item = new MembershipUpgradeLog();
			
				item.OrderHdrID = varOrderHdrID;
			
				item.IsVitaMixPrevValue = varIsVitaMixPrevValue;
			
				item.IsWaterFilterPrevValue = varIsWaterFilterPrevValue;
			
				item.IsJuicePlusPrevValue = varIsJuicePlusPrevValue;
			
				item.IsYoungPrevValue = varIsYoungPrevValue;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedOn = varModifiedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.UniqueID = varUniqueID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn OrderHdrIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn IsVitaMixPrevValueColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn IsWaterFilterPrevValueColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn IsJuicePlusPrevValueColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IsYoungPrevValueColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string OrderHdrID = @"OrderHdrID";
			 public static string IsVitaMixPrevValue = @"IsVitaMixPrevValue";
			 public static string IsWaterFilterPrevValue = @"IsWaterFilterPrevValue";
			 public static string IsJuicePlusPrevValue = @"IsJuicePlusPrevValue";
			 public static string IsYoungPrevValue = @"IsYoungPrevValue";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string UniqueID = @"UniqueID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
