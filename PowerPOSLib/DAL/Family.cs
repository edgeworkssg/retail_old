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
	/// Strongly-typed collection for the Family class.
	/// </summary>
    [Serializable]
	public partial class FamilyCollection : ActiveList<Family, FamilyCollection>
	{	   
		public FamilyCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>FamilyCollection</returns>
		public FamilyCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Family o = this[i];
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
	/// This is an ActiveRecord class which wraps the Family table.
	/// </summary>
	[Serializable]
	public partial class Family : ActiveRecord<Family>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Family()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Family(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Family(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Family(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Family", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarFamilyID = new TableSchema.TableColumn(schema);
				colvarFamilyID.ColumnName = "FamilyID";
				colvarFamilyID.DataType = DbType.AnsiString;
				colvarFamilyID.MaxLength = 50;
				colvarFamilyID.AutoIncrement = false;
				colvarFamilyID.IsNullable = false;
				colvarFamilyID.IsPrimaryKey = true;
				colvarFamilyID.IsForeignKey = false;
				colvarFamilyID.IsReadOnly = false;
				colvarFamilyID.DefaultSetting = @"";
				colvarFamilyID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFamilyID);
				
				TableSchema.TableColumn colvarFamilyName = new TableSchema.TableColumn(schema);
				colvarFamilyName.ColumnName = "FamilyName";
				colvarFamilyName.DataType = DbType.String;
				colvarFamilyName.MaxLength = 250;
				colvarFamilyName.AutoIncrement = false;
				colvarFamilyName.IsNullable = false;
				colvarFamilyName.IsPrimaryKey = false;
				colvarFamilyName.IsForeignKey = false;
				colvarFamilyName.IsReadOnly = false;
				colvarFamilyName.DefaultSetting = @"";
				colvarFamilyName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFamilyName);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.String;
				colvarRemark.MaxLength = -1;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = false;
				colvarRemark.IsPrimaryKey = false;
				colvarRemark.IsForeignKey = false;
				colvarRemark.IsReadOnly = false;
				colvarRemark.DefaultSetting = @"";
				colvarRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemark);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Family",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("FamilyID")]
		[Bindable(true)]
		public string FamilyID 
		{
			get { return GetColumnValue<string>(Columns.FamilyID); }
			set { SetColumnValue(Columns.FamilyID, value); }
		}
		  
		[XmlAttribute("FamilyName")]
		[Bindable(true)]
		public string FamilyName 
		{
			get { return GetColumnValue<string>(Columns.FamilyName); }
			set { SetColumnValue(Columns.FamilyName, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
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
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varFamilyID,string varFamilyName,string varRemark,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted)
		{
			Family item = new Family();
			
			item.FamilyID = varFamilyID;
			
			item.FamilyName = varFamilyName;
			
			item.Remark = varRemark;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedOn = varModifiedOn;
			
			item.CreatedBy = varCreatedBy;
			
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
		public static void Update(string varFamilyID,string varFamilyName,string varRemark,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted)
		{
			Family item = new Family();
			
				item.FamilyID = varFamilyID;
			
				item.FamilyName = varFamilyName;
			
				item.Remark = varRemark;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedOn = varModifiedOn;
			
				item.CreatedBy = varCreatedBy;
			
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
        
        
        public static TableSchema.TableColumn FamilyIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn FamilyNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string FamilyID = @"FamilyID";
			 public static string FamilyName = @"FamilyName";
			 public static string Remark = @"Remark";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string CreatedBy = @"CreatedBy";
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
