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
	/// Strongly-typed collection for the ItemDepartment class.
	/// </summary>
    [Serializable]
	public partial class ItemDepartmentCollection : ActiveList<ItemDepartment, ItemDepartmentCollection>
	{	   
		public ItemDepartmentCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ItemDepartmentCollection</returns>
		public ItemDepartmentCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ItemDepartment o = this[i];
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
	/// This is an ActiveRecord class which wraps the ItemDepartment table.
	/// </summary>
	[Serializable]
	public partial class ItemDepartment : ActiveRecord<ItemDepartment>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ItemDepartment()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ItemDepartment(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ItemDepartment(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ItemDepartment(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ItemDepartment", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarItemDepartmentID = new TableSchema.TableColumn(schema);
				colvarItemDepartmentID.ColumnName = "ItemDepartmentID";
				colvarItemDepartmentID.DataType = DbType.AnsiString;
				colvarItemDepartmentID.MaxLength = 50;
				colvarItemDepartmentID.AutoIncrement = false;
				colvarItemDepartmentID.IsNullable = false;
				colvarItemDepartmentID.IsPrimaryKey = true;
				colvarItemDepartmentID.IsForeignKey = false;
				colvarItemDepartmentID.IsReadOnly = false;
				colvarItemDepartmentID.DefaultSetting = @"";
				colvarItemDepartmentID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemDepartmentID);
				
				TableSchema.TableColumn colvarDepartmentName = new TableSchema.TableColumn(schema);
				colvarDepartmentName.ColumnName = "DepartmentName";
				colvarDepartmentName.DataType = DbType.String;
				colvarDepartmentName.MaxLength = 50;
				colvarDepartmentName.AutoIncrement = false;
				colvarDepartmentName.IsNullable = false;
				colvarDepartmentName.IsPrimaryKey = false;
				colvarDepartmentName.IsForeignKey = false;
				colvarDepartmentName.IsReadOnly = false;
				colvarDepartmentName.DefaultSetting = @"";
				colvarDepartmentName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDepartmentName);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.String;
				colvarRemark.MaxLength = -1;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = true;
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
				
				TableSchema.TableColumn colvarDepartmentOrder = new TableSchema.TableColumn(schema);
				colvarDepartmentOrder.ColumnName = "DepartmentOrder";
				colvarDepartmentOrder.DataType = DbType.Int32;
				colvarDepartmentOrder.MaxLength = 0;
				colvarDepartmentOrder.AutoIncrement = false;
				colvarDepartmentOrder.IsNullable = true;
				colvarDepartmentOrder.IsPrimaryKey = false;
				colvarDepartmentOrder.IsForeignKey = false;
				colvarDepartmentOrder.IsReadOnly = false;
				colvarDepartmentOrder.DefaultSetting = @"";
				colvarDepartmentOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDepartmentOrder);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("ItemDepartment",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("ItemDepartmentID")]
		[Bindable(true)]
		public string ItemDepartmentID 
		{
			get { return GetColumnValue<string>(Columns.ItemDepartmentID); }
			set { SetColumnValue(Columns.ItemDepartmentID, value); }
		}
		  
		[XmlAttribute("DepartmentName")]
		[Bindable(true)]
		public string DepartmentName 
		{
			get { return GetColumnValue<string>(Columns.DepartmentName); }
			set { SetColumnValue(Columns.DepartmentName, value); }
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
		  
		[XmlAttribute("DepartmentOrder")]
		[Bindable(true)]
		public int? DepartmentOrder 
		{
			get { return GetColumnValue<int?>(Columns.DepartmentOrder); }
			set { SetColumnValue(Columns.DepartmentOrder, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		public PowerPOS.CategoryCollection CategoryRecords()
		{
			return new PowerPOS.CategoryCollection().Where(Category.Columns.ItemDepartmentId, ItemDepartmentID).Load();
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varItemDepartmentID,string varDepartmentName,string varRemark,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted,int? varDepartmentOrder)
		{
			ItemDepartment item = new ItemDepartment();
			
			item.ItemDepartmentID = varItemDepartmentID;
			
			item.DepartmentName = varDepartmentName;
			
			item.Remark = varRemark;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedOn = varModifiedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.DepartmentOrder = varDepartmentOrder;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varItemDepartmentID,string varDepartmentName,string varRemark,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted,int? varDepartmentOrder)
		{
			ItemDepartment item = new ItemDepartment();
			
				item.ItemDepartmentID = varItemDepartmentID;
			
				item.DepartmentName = varDepartmentName;
			
				item.Remark = varRemark;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedOn = varModifiedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.DepartmentOrder = varDepartmentOrder;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ItemDepartmentIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn DepartmentNameColumn
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
        
        
        
        public static TableSchema.TableColumn DepartmentOrderColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ItemDepartmentID = @"ItemDepartmentID";
			 public static string DepartmentName = @"DepartmentName";
			 public static string Remark = @"Remark";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string DepartmentOrder = @"DepartmentOrder";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
}
        #endregion
	}
}
