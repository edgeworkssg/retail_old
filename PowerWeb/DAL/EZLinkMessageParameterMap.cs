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
	/// Strongly-typed collection for the EZLinkMessageParameterMap class.
	/// </summary>
    [Serializable]
	public partial class EZLinkMessageParameterMapCollection : ActiveList<EZLinkMessageParameterMap, EZLinkMessageParameterMapCollection>
	{	   
		public EZLinkMessageParameterMapCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>EZLinkMessageParameterMapCollection</returns>
		public EZLinkMessageParameterMapCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                EZLinkMessageParameterMap o = this[i];
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
	/// This is an ActiveRecord class which wraps the EZLinkMessageParameterMap table.
	/// </summary>
	[Serializable]
	public partial class EZLinkMessageParameterMap : ActiveRecord<EZLinkMessageParameterMap>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public EZLinkMessageParameterMap()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public EZLinkMessageParameterMap(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public EZLinkMessageParameterMap(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public EZLinkMessageParameterMap(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("EZLinkMessageParameterMap", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarMapID = new TableSchema.TableColumn(schema);
				colvarMapID.ColumnName = "MapID";
				colvarMapID.DataType = DbType.Int32;
				colvarMapID.MaxLength = 0;
				colvarMapID.AutoIncrement = true;
				colvarMapID.IsNullable = false;
				colvarMapID.IsPrimaryKey = true;
				colvarMapID.IsForeignKey = false;
				colvarMapID.IsReadOnly = false;
				colvarMapID.DefaultSetting = @"";
				colvarMapID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMapID);
				
				TableSchema.TableColumn colvarMsgName = new TableSchema.TableColumn(schema);
				colvarMsgName.ColumnName = "MsgName";
				colvarMsgName.DataType = DbType.AnsiString;
				colvarMsgName.MaxLength = 50;
				colvarMsgName.AutoIncrement = false;
				colvarMsgName.IsNullable = false;
				colvarMsgName.IsPrimaryKey = false;
				colvarMsgName.IsForeignKey = false;
				colvarMsgName.IsReadOnly = false;
				colvarMsgName.DefaultSetting = @"";
				colvarMsgName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMsgName);
				
				TableSchema.TableColumn colvarParameterName = new TableSchema.TableColumn(schema);
				colvarParameterName.ColumnName = "ParameterName";
				colvarParameterName.DataType = DbType.AnsiString;
				colvarParameterName.MaxLength = 50;
				colvarParameterName.AutoIncrement = false;
				colvarParameterName.IsNullable = false;
				colvarParameterName.IsPrimaryKey = false;
				colvarParameterName.IsForeignKey = false;
				colvarParameterName.IsReadOnly = false;
				colvarParameterName.DefaultSetting = @"";
				colvarParameterName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarParameterName);
				
				TableSchema.TableColumn colvarStartingIndex = new TableSchema.TableColumn(schema);
				colvarStartingIndex.ColumnName = "startingIndex";
				colvarStartingIndex.DataType = DbType.Int32;
				colvarStartingIndex.MaxLength = 0;
				colvarStartingIndex.AutoIncrement = false;
				colvarStartingIndex.IsNullable = false;
				colvarStartingIndex.IsPrimaryKey = false;
				colvarStartingIndex.IsForeignKey = false;
				colvarStartingIndex.IsReadOnly = false;
				colvarStartingIndex.DefaultSetting = @"";
				colvarStartingIndex.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStartingIndex);
				
				TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
				colvarDeleted.ColumnName = "Deleted";
				colvarDeleted.DataType = DbType.Boolean;
				colvarDeleted.MaxLength = 0;
				colvarDeleted.AutoIncrement = false;
				colvarDeleted.IsNullable = false;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				
						colvarDeleted.DefaultSetting = @"((0))";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
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
				
				TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
				colvarUniqueID.ColumnName = "UniqueID";
				colvarUniqueID.DataType = DbType.Guid;
				colvarUniqueID.MaxLength = 0;
				colvarUniqueID.AutoIncrement = false;
				colvarUniqueID.IsNullable = false;
				colvarUniqueID.IsPrimaryKey = false;
				colvarUniqueID.IsForeignKey = false;
				colvarUniqueID.IsReadOnly = false;
				
						colvarUniqueID.DefaultSetting = @"(newid())";
				colvarUniqueID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("EZLinkMessageParameterMap",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("MapID")]
		[Bindable(true)]
		public int MapID 
		{
			get { return GetColumnValue<int>(Columns.MapID); }
			set { SetColumnValue(Columns.MapID, value); }
		}
		  
		[XmlAttribute("MsgName")]
		[Bindable(true)]
		public string MsgName 
		{
			get { return GetColumnValue<string>(Columns.MsgName); }
			set { SetColumnValue(Columns.MsgName, value); }
		}
		  
		[XmlAttribute("ParameterName")]
		[Bindable(true)]
		public string ParameterName 
		{
			get { return GetColumnValue<string>(Columns.ParameterName); }
			set { SetColumnValue(Columns.ParameterName, value); }
		}
		  
		[XmlAttribute("StartingIndex")]
		[Bindable(true)]
		public int StartingIndex 
		{
			get { return GetColumnValue<int>(Columns.StartingIndex); }
			set { SetColumnValue(Columns.StartingIndex, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool Deleted 
		{
			get { return GetColumnValue<bool>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
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
		  
		[XmlAttribute("UniqueID")]
		[Bindable(true)]
		public Guid UniqueID 
		{
			get { return GetColumnValue<Guid>(Columns.UniqueID); }
			set { SetColumnValue(Columns.UniqueID, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varMsgName,string varParameterName,int varStartingIndex,bool varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,Guid varUniqueID)
		{
			EZLinkMessageParameterMap item = new EZLinkMessageParameterMap();
			
			item.MsgName = varMsgName;
			
			item.ParameterName = varParameterName;
			
			item.StartingIndex = varStartingIndex;
			
			item.Deleted = varDeleted;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.UniqueID = varUniqueID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varMapID,string varMsgName,string varParameterName,int varStartingIndex,bool varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,Guid varUniqueID)
		{
			EZLinkMessageParameterMap item = new EZLinkMessageParameterMap();
			
				item.MapID = varMapID;
			
				item.MsgName = varMsgName;
			
				item.ParameterName = varParameterName;
			
				item.StartingIndex = varStartingIndex;
			
				item.Deleted = varDeleted;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.UniqueID = varUniqueID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn MapIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn MsgNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ParameterNameColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn StartingIndexColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string MapID = @"MapID";
			 public static string MsgName = @"MsgName";
			 public static string ParameterName = @"ParameterName";
			 public static string StartingIndex = @"startingIndex";
			 public static string Deleted = @"Deleted";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string UniqueID = @"UniqueID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
