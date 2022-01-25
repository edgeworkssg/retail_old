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
	/// Strongly-typed collection for the ItemBaseLevel class.
	/// </summary>
    [Serializable]
	public partial class ItemBaseLevelCollection : ActiveList<ItemBaseLevel, ItemBaseLevelCollection>
	{	   
		public ItemBaseLevelCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ItemBaseLevelCollection</returns>
		public ItemBaseLevelCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ItemBaseLevel o = this[i];
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
	/// This is an ActiveRecord class which wraps the ItemBaseLevel table.
	/// </summary>
	[Serializable]
	public partial class ItemBaseLevel : ActiveRecord<ItemBaseLevel>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ItemBaseLevel()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ItemBaseLevel(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ItemBaseLevel(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ItemBaseLevel(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ItemBaseLevel", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarBaseLevelID = new TableSchema.TableColumn(schema);
				colvarBaseLevelID.ColumnName = "BaseLevelID";
				colvarBaseLevelID.DataType = DbType.Int32;
				colvarBaseLevelID.MaxLength = 0;
				colvarBaseLevelID.AutoIncrement = true;
				colvarBaseLevelID.IsNullable = false;
				colvarBaseLevelID.IsPrimaryKey = true;
				colvarBaseLevelID.IsForeignKey = false;
				colvarBaseLevelID.IsReadOnly = false;
				colvarBaseLevelID.DefaultSetting = @"";
				colvarBaseLevelID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBaseLevelID);
				
				TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
				colvarItemNo.ColumnName = "ItemNo";
				colvarItemNo.DataType = DbType.AnsiString;
				colvarItemNo.MaxLength = 50;
				colvarItemNo.AutoIncrement = false;
				colvarItemNo.IsNullable = false;
				colvarItemNo.IsPrimaryKey = false;
				colvarItemNo.IsForeignKey = true;
				colvarItemNo.IsReadOnly = false;
				colvarItemNo.DefaultSetting = @"";
				
					colvarItemNo.ForeignKeyTableName = "Item";
				schema.Columns.Add(colvarItemNo);
				
				TableSchema.TableColumn colvarBaseLevelQuantity = new TableSchema.TableColumn(schema);
				colvarBaseLevelQuantity.ColumnName = "BaseLevelQuantity";
				colvarBaseLevelQuantity.DataType = DbType.Int32;
				colvarBaseLevelQuantity.MaxLength = 0;
				colvarBaseLevelQuantity.AutoIncrement = false;
				colvarBaseLevelQuantity.IsNullable = false;
				colvarBaseLevelQuantity.IsPrimaryKey = false;
				colvarBaseLevelQuantity.IsForeignKey = false;
				colvarBaseLevelQuantity.IsReadOnly = false;
				colvarBaseLevelQuantity.DefaultSetting = @"";
				colvarBaseLevelQuantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBaseLevelQuantity);
				
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
				colvarDeleted.IsNullable = false;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				
						colvarDeleted.DefaultSetting = @"((0))";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				TableSchema.TableColumn colvarInventoryLocationID = new TableSchema.TableColumn(schema);
				colvarInventoryLocationID.ColumnName = "InventoryLocationID";
				colvarInventoryLocationID.DataType = DbType.Int32;
				colvarInventoryLocationID.MaxLength = 0;
				colvarInventoryLocationID.AutoIncrement = false;
				colvarInventoryLocationID.IsNullable = true;
				colvarInventoryLocationID.IsPrimaryKey = false;
				colvarInventoryLocationID.IsForeignKey = true;
				colvarInventoryLocationID.IsReadOnly = false;
				colvarInventoryLocationID.DefaultSetting = @"";
				
					colvarInventoryLocationID.ForeignKeyTableName = "InventoryLocation";
				schema.Columns.Add(colvarInventoryLocationID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("ItemBaseLevel",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("BaseLevelID")]
		[Bindable(true)]
		public int BaseLevelID 
		{
			get { return GetColumnValue<int>(Columns.BaseLevelID); }
			set { SetColumnValue(Columns.BaseLevelID, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
		}
		  
		[XmlAttribute("BaseLevelQuantity")]
		[Bindable(true)]
		public int BaseLevelQuantity 
		{
			get { return GetColumnValue<int>(Columns.BaseLevelQuantity); }
			set { SetColumnValue(Columns.BaseLevelQuantity, value); }
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
		public bool Deleted 
		{
			get { return GetColumnValue<bool>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("InventoryLocationID")]
		[Bindable(true)]
		public int? InventoryLocationID 
		{
			get { return GetColumnValue<int?>(Columns.InventoryLocationID); }
			set { SetColumnValue(Columns.InventoryLocationID, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a InventoryLocation ActiveRecord object related to this ItemBaseLevel
		/// 
		/// </summary>
		public PowerPOS.InventoryLocation InventoryLocation
		{
			get { return PowerPOS.InventoryLocation.FetchByID(this.InventoryLocationID); }
			set { SetColumnValue("InventoryLocationID", value.InventoryLocationID); }
		}
		
		
		/// <summary>
		/// Returns a Item ActiveRecord object related to this ItemBaseLevel
		/// 
		/// </summary>
		public PowerPOS.Item Item
		{
			get { return PowerPOS.Item.FetchByID(this.ItemNo); }
			set { SetColumnValue("ItemNo", value.ItemNo); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varItemNo,int varBaseLevelQuantity,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool varDeleted,int? varInventoryLocationID)
		{
			ItemBaseLevel item = new ItemBaseLevel();
			
			item.ItemNo = varItemNo;
			
			item.BaseLevelQuantity = varBaseLevelQuantity;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.InventoryLocationID = varInventoryLocationID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varBaseLevelID,string varItemNo,int varBaseLevelQuantity,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool varDeleted,int? varInventoryLocationID)
		{
			ItemBaseLevel item = new ItemBaseLevel();
			
				item.BaseLevelID = varBaseLevelID;
			
				item.ItemNo = varItemNo;
			
				item.BaseLevelQuantity = varBaseLevelQuantity;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.InventoryLocationID = varInventoryLocationID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn BaseLevelIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn BaseLevelQuantityColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
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
        
        
        
        public static TableSchema.TableColumn InventoryLocationIDColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string BaseLevelID = @"BaseLevelID";
			 public static string ItemNo = @"ItemNo";
			 public static string BaseLevelQuantity = @"BaseLevelQuantity";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string InventoryLocationID = @"InventoryLocationID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
