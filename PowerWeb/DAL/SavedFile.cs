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
	/// Strongly-typed collection for the SavedFile class.
	/// </summary>
    [Serializable]
	public partial class SavedFileCollection : ActiveList<SavedFile, SavedFileCollection>
	{	   
		public SavedFileCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SavedFileCollection</returns>
		public SavedFileCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SavedFile o = this[i];
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
	/// This is an ActiveRecord class which wraps the SavedFiles table.
	/// </summary>
	[Serializable]
	public partial class SavedFile : ActiveRecord<SavedFile>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SavedFile()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SavedFile(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SavedFile(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SavedFile(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SavedFiles", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarSaveID = new TableSchema.TableColumn(schema);
				colvarSaveID.ColumnName = "SaveID";
				colvarSaveID.DataType = DbType.Int32;
				colvarSaveID.MaxLength = 0;
				colvarSaveID.AutoIncrement = true;
				colvarSaveID.IsNullable = false;
				colvarSaveID.IsPrimaryKey = true;
				colvarSaveID.IsForeignKey = false;
				colvarSaveID.IsReadOnly = false;
				colvarSaveID.DefaultSetting = @"";
				colvarSaveID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSaveID);
				
				TableSchema.TableColumn colvarSaveName = new TableSchema.TableColumn(schema);
				colvarSaveName.ColumnName = "SaveName";
				colvarSaveName.DataType = DbType.AnsiString;
				colvarSaveName.MaxLength = -1;
				colvarSaveName.AutoIncrement = false;
				colvarSaveName.IsNullable = false;
				colvarSaveName.IsPrimaryKey = false;
				colvarSaveName.IsForeignKey = false;
				colvarSaveName.IsReadOnly = false;
				colvarSaveName.DefaultSetting = @"";
				colvarSaveName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSaveName);
				
				TableSchema.TableColumn colvarSavedBy = new TableSchema.TableColumn(schema);
				colvarSavedBy.ColumnName = "SavedBy";
				colvarSavedBy.DataType = DbType.AnsiString;
				colvarSavedBy.MaxLength = -1;
				colvarSavedBy.AutoIncrement = false;
				colvarSavedBy.IsNullable = true;
				colvarSavedBy.IsPrimaryKey = false;
				colvarSavedBy.IsForeignKey = false;
				colvarSavedBy.IsReadOnly = false;
				colvarSavedBy.DefaultSetting = @"";
				colvarSavedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSavedBy);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "remark";
				colvarRemark.DataType = DbType.AnsiString;
				colvarRemark.MaxLength = -1;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = false;
				colvarRemark.IsPrimaryKey = false;
				colvarRemark.IsForeignKey = false;
				colvarRemark.IsReadOnly = false;
				colvarRemark.DefaultSetting = @"";
				colvarRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemark);
				
				TableSchema.TableColumn colvarMovementType = new TableSchema.TableColumn(schema);
				colvarMovementType.ColumnName = "MovementType";
				colvarMovementType.DataType = DbType.AnsiString;
				colvarMovementType.MaxLength = 250;
				colvarMovementType.AutoIncrement = false;
				colvarMovementType.IsNullable = true;
				colvarMovementType.IsPrimaryKey = false;
				colvarMovementType.IsForeignKey = false;
				colvarMovementType.IsReadOnly = false;
				colvarMovementType.DefaultSetting = @"";
				colvarMovementType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMovementType);
				
				TableSchema.TableColumn colvarSavedDate = new TableSchema.TableColumn(schema);
				colvarSavedDate.ColumnName = "savedDate";
				colvarSavedDate.DataType = DbType.DateTime;
				colvarSavedDate.MaxLength = 0;
				colvarSavedDate.AutoIncrement = false;
				colvarSavedDate.IsNullable = true;
				colvarSavedDate.IsPrimaryKey = false;
				colvarSavedDate.IsForeignKey = false;
				colvarSavedDate.IsReadOnly = false;
				colvarSavedDate.DefaultSetting = @"";
				colvarSavedDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSavedDate);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("SavedFiles",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("SaveID")]
		[Bindable(true)]
		public int SaveID 
		{
			get { return GetColumnValue<int>(Columns.SaveID); }
			set { SetColumnValue(Columns.SaveID, value); }
		}
		  
		[XmlAttribute("SaveName")]
		[Bindable(true)]
		public string SaveName 
		{
			get { return GetColumnValue<string>(Columns.SaveName); }
			set { SetColumnValue(Columns.SaveName, value); }
		}
		  
		[XmlAttribute("SavedBy")]
		[Bindable(true)]
		public string SavedBy 
		{
			get { return GetColumnValue<string>(Columns.SavedBy); }
			set { SetColumnValue(Columns.SavedBy, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
		}
		  
		[XmlAttribute("MovementType")]
		[Bindable(true)]
		public string MovementType 
		{
			get { return GetColumnValue<string>(Columns.MovementType); }
			set { SetColumnValue(Columns.MovementType, value); }
		}
		  
		[XmlAttribute("SavedDate")]
		[Bindable(true)]
		public DateTime? SavedDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.SavedDate); }
			set { SetColumnValue(Columns.SavedDate, value); }
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
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varSaveName,string varSavedBy,string varRemark,string varMovementType,DateTime? varSavedDate,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted)
		{
			SavedFile item = new SavedFile();
			
			item.SaveName = varSaveName;
			
			item.SavedBy = varSavedBy;
			
			item.Remark = varRemark;
			
			item.MovementType = varMovementType;
			
			item.SavedDate = varSavedDate;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varSaveID,string varSaveName,string varSavedBy,string varRemark,string varMovementType,DateTime? varSavedDate,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted)
		{
			SavedFile item = new SavedFile();
			
				item.SaveID = varSaveID;
			
				item.SaveName = varSaveName;
			
				item.SavedBy = varSavedBy;
			
				item.Remark = varRemark;
			
				item.MovementType = varMovementType;
			
				item.SavedDate = varSavedDate;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn SaveIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn SaveNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn SavedByColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn MovementTypeColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn SavedDateColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string SaveID = @"SaveID";
			 public static string SaveName = @"SaveName";
			 public static string SavedBy = @"SavedBy";
			 public static string Remark = @"remark";
			 public static string MovementType = @"MovementType";
			 public static string SavedDate = @"savedDate";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
