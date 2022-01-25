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
	/// Strongly-typed collection for the PointTempLog class.
	/// </summary>
    [Serializable]
	public partial class PointTempLogCollection : ActiveList<PointTempLog, PointTempLogCollection>
	{	   
		public PointTempLogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PointTempLogCollection</returns>
		public PointTempLogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PointTempLog o = this[i];
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
	/// This is an ActiveRecord class which wraps the PointTempLog table.
	/// </summary>
	[Serializable]
	public partial class PointTempLog : ActiveRecord<PointTempLog>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PointTempLog()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PointTempLog(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PointTempLog(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PointTempLog(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("PointTempLog", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarTempID = new TableSchema.TableColumn(schema);
				colvarTempID.ColumnName = "TempID";
				colvarTempID.DataType = DbType.Int32;
				colvarTempID.MaxLength = 0;
				colvarTempID.AutoIncrement = true;
				colvarTempID.IsNullable = false;
				colvarTempID.IsPrimaryKey = true;
				colvarTempID.IsForeignKey = false;
				colvarTempID.IsReadOnly = false;
				colvarTempID.DefaultSetting = @"";
				colvarTempID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTempID);
				
				TableSchema.TableColumn colvarOrderHdrID = new TableSchema.TableColumn(schema);
				colvarOrderHdrID.ColumnName = "OrderHdrID";
				colvarOrderHdrID.DataType = DbType.AnsiString;
				colvarOrderHdrID.MaxLength = 50;
				colvarOrderHdrID.AutoIncrement = false;
				colvarOrderHdrID.IsNullable = false;
				colvarOrderHdrID.IsPrimaryKey = false;
				colvarOrderHdrID.IsForeignKey = false;
				colvarOrderHdrID.IsReadOnly = false;
				colvarOrderHdrID.DefaultSetting = @"";
				colvarOrderHdrID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderHdrID);
				
				TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
				colvarMembershipNo.ColumnName = "MembershipNo";
				colvarMembershipNo.DataType = DbType.AnsiString;
				colvarMembershipNo.MaxLength = 50;
				colvarMembershipNo.AutoIncrement = false;
				colvarMembershipNo.IsNullable = false;
				colvarMembershipNo.IsPrimaryKey = false;
				colvarMembershipNo.IsForeignKey = false;
				colvarMembershipNo.IsReadOnly = false;
				colvarMembershipNo.DefaultSetting = @"";
				colvarMembershipNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembershipNo);
				
				TableSchema.TableColumn colvarPointAllocated = new TableSchema.TableColumn(schema);
				colvarPointAllocated.ColumnName = "PointAllocated";
				colvarPointAllocated.DataType = DbType.Currency;
				colvarPointAllocated.MaxLength = 0;
				colvarPointAllocated.AutoIncrement = false;
				colvarPointAllocated.IsNullable = false;
				colvarPointAllocated.IsPrimaryKey = false;
				colvarPointAllocated.IsForeignKey = false;
				colvarPointAllocated.IsReadOnly = false;
				colvarPointAllocated.DefaultSetting = @"";
				colvarPointAllocated.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointAllocated);
				
				TableSchema.TableColumn colvarRefNo = new TableSchema.TableColumn(schema);
				colvarRefNo.ColumnName = "RefNo";
				colvarRefNo.DataType = DbType.AnsiString;
				colvarRefNo.MaxLength = -1;
				colvarRefNo.AutoIncrement = false;
				colvarRefNo.IsNullable = true;
				colvarRefNo.IsPrimaryKey = false;
				colvarRefNo.IsForeignKey = false;
				colvarRefNo.IsReadOnly = false;
				colvarRefNo.DefaultSetting = @"";
				colvarRefNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRefNo);
				
				TableSchema.TableColumn colvarPointType = new TableSchema.TableColumn(schema);
				colvarPointType.ColumnName = "PointType";
				colvarPointType.DataType = DbType.AnsiString;
				colvarPointType.MaxLength = 50;
				colvarPointType.AutoIncrement = false;
				colvarPointType.IsNullable = true;
				colvarPointType.IsPrimaryKey = false;
				colvarPointType.IsForeignKey = false;
				colvarPointType.IsReadOnly = false;
				colvarPointType.DefaultSetting = @"";
				colvarPointType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointType);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("PointTempLog",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("TempID")]
		[Bindable(true)]
		public int TempID 
		{
			get { return GetColumnValue<int>(Columns.TempID); }
			set { SetColumnValue(Columns.TempID, value); }
		}
		  
		[XmlAttribute("OrderHdrID")]
		[Bindable(true)]
		public string OrderHdrID 
		{
			get { return GetColumnValue<string>(Columns.OrderHdrID); }
			set { SetColumnValue(Columns.OrderHdrID, value); }
		}
		  
		[XmlAttribute("MembershipNo")]
		[Bindable(true)]
		public string MembershipNo 
		{
			get { return GetColumnValue<string>(Columns.MembershipNo); }
			set { SetColumnValue(Columns.MembershipNo, value); }
		}
		  
		[XmlAttribute("PointAllocated")]
		[Bindable(true)]
		public decimal PointAllocated 
		{
			get { return GetColumnValue<decimal>(Columns.PointAllocated); }
			set { SetColumnValue(Columns.PointAllocated, value); }
		}
		  
		[XmlAttribute("RefNo")]
		[Bindable(true)]
		public string RefNo 
		{
			get { return GetColumnValue<string>(Columns.RefNo); }
			set { SetColumnValue(Columns.RefNo, value); }
		}
		  
		[XmlAttribute("PointType")]
		[Bindable(true)]
		public string PointType 
		{
			get { return GetColumnValue<string>(Columns.PointType); }
			set { SetColumnValue(Columns.PointType, value); }
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
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varOrderHdrID,string varMembershipNo,decimal varPointAllocated,string varRefNo,string varPointType,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy)
		{
			PointTempLog item = new PointTempLog();
			
			item.OrderHdrID = varOrderHdrID;
			
			item.MembershipNo = varMembershipNo;
			
			item.PointAllocated = varPointAllocated;
			
			item.RefNo = varRefNo;
			
			item.PointType = varPointType;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedOn = varModifiedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedBy = varModifiedBy;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varTempID,string varOrderHdrID,string varMembershipNo,decimal varPointAllocated,string varRefNo,string varPointType,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy)
		{
			PointTempLog item = new PointTempLog();
			
				item.TempID = varTempID;
			
				item.OrderHdrID = varOrderHdrID;
			
				item.MembershipNo = varMembershipNo;
			
				item.PointAllocated = varPointAllocated;
			
				item.RefNo = varRefNo;
			
				item.PointType = varPointType;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedOn = varModifiedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedBy = varModifiedBy;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn TempIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderHdrIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipNoColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn PointAllocatedColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn RefNoColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn PointTypeColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string TempID = @"TempID";
			 public static string OrderHdrID = @"OrderHdrID";
			 public static string MembershipNo = @"MembershipNo";
			 public static string PointAllocated = @"PointAllocated";
			 public static string RefNo = @"RefNo";
			 public static string PointType = @"PointType";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedBy = @"ModifiedBy";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
