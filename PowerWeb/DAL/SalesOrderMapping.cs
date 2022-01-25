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
	/// Strongly-typed collection for the SalesOrderMapping class.
	/// </summary>
    [Serializable]
	public partial class SalesOrderMappingCollection : ActiveList<SalesOrderMapping, SalesOrderMappingCollection>
	{	   
		public SalesOrderMappingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SalesOrderMappingCollection</returns>
		public SalesOrderMappingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SalesOrderMapping o = this[i];
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
	/// This is an ActiveRecord class which wraps the SalesOrderMapping table.
	/// </summary>
	[Serializable]
	public partial class SalesOrderMapping : ActiveRecord<SalesOrderMapping>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SalesOrderMapping()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SalesOrderMapping(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SalesOrderMapping(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SalesOrderMapping(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SalesOrderMapping", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarSalesOrderMappingID = new TableSchema.TableColumn(schema);
				colvarSalesOrderMappingID.ColumnName = "SalesOrderMappingID";
				colvarSalesOrderMappingID.DataType = DbType.Int32;
				colvarSalesOrderMappingID.MaxLength = 0;
				colvarSalesOrderMappingID.AutoIncrement = true;
				colvarSalesOrderMappingID.IsNullable = false;
				colvarSalesOrderMappingID.IsPrimaryKey = true;
				colvarSalesOrderMappingID.IsForeignKey = false;
				colvarSalesOrderMappingID.IsReadOnly = false;
				colvarSalesOrderMappingID.DefaultSetting = @"";
				colvarSalesOrderMappingID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSalesOrderMappingID);
				
				TableSchema.TableColumn colvarOrderDetID = new TableSchema.TableColumn(schema);
				colvarOrderDetID.ColumnName = "OrderDetID";
				colvarOrderDetID.DataType = DbType.AnsiString;
				colvarOrderDetID.MaxLength = 50;
				colvarOrderDetID.AutoIncrement = false;
				colvarOrderDetID.IsNullable = true;
				colvarOrderDetID.IsPrimaryKey = false;
				colvarOrderDetID.IsForeignKey = false;
				colvarOrderDetID.IsReadOnly = false;
				colvarOrderDetID.DefaultSetting = @"";
				colvarOrderDetID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderDetID);
				
				TableSchema.TableColumn colvarPurchaseOrderDetRefNo = new TableSchema.TableColumn(schema);
				colvarPurchaseOrderDetRefNo.ColumnName = "PurchaseOrderDetRefNo";
				colvarPurchaseOrderDetRefNo.DataType = DbType.AnsiString;
				colvarPurchaseOrderDetRefNo.MaxLength = 50;
				colvarPurchaseOrderDetRefNo.AutoIncrement = false;
				colvarPurchaseOrderDetRefNo.IsNullable = true;
				colvarPurchaseOrderDetRefNo.IsPrimaryKey = false;
				colvarPurchaseOrderDetRefNo.IsForeignKey = false;
				colvarPurchaseOrderDetRefNo.IsReadOnly = false;
				colvarPurchaseOrderDetRefNo.DefaultSetting = @"";
				colvarPurchaseOrderDetRefNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPurchaseOrderDetRefNo);
				
				TableSchema.TableColumn colvarQty = new TableSchema.TableColumn(schema);
				colvarQty.ColumnName = "Qty";
				colvarQty.DataType = DbType.Decimal;
				colvarQty.MaxLength = 0;
				colvarQty.AutoIncrement = false;
				colvarQty.IsNullable = true;
				colvarQty.IsPrimaryKey = false;
				colvarQty.IsForeignKey = false;
				colvarQty.IsReadOnly = false;
				colvarQty.DefaultSetting = @"";
				colvarQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQty);
				
				TableSchema.TableColumn colvarQtyApproved = new TableSchema.TableColumn(schema);
				colvarQtyApproved.ColumnName = "QtyApproved";
				colvarQtyApproved.DataType = DbType.Decimal;
				colvarQtyApproved.MaxLength = 0;
				colvarQtyApproved.AutoIncrement = false;
				colvarQtyApproved.IsNullable = true;
				colvarQtyApproved.IsPrimaryKey = false;
				colvarQtyApproved.IsForeignKey = false;
				colvarQtyApproved.IsReadOnly = false;
				colvarQtyApproved.DefaultSetting = @"";
				colvarQtyApproved.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQtyApproved);
				
				TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
				colvarDeleted.ColumnName = "Deleted";
				colvarDeleted.DataType = DbType.Boolean;
				colvarDeleted.MaxLength = 0;
				colvarDeleted.AutoIncrement = false;
				colvarDeleted.IsNullable = false;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				colvarDeleted.DefaultSetting = @"";
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("SalesOrderMapping",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("SalesOrderMappingID")]
		[Bindable(true)]
		public int SalesOrderMappingID 
		{
			get { return GetColumnValue<int>(Columns.SalesOrderMappingID); }
			set { SetColumnValue(Columns.SalesOrderMappingID, value); }
		}
		  
		[XmlAttribute("OrderDetID")]
		[Bindable(true)]
		public string OrderDetID 
		{
			get { return GetColumnValue<string>(Columns.OrderDetID); }
			set { SetColumnValue(Columns.OrderDetID, value); }
		}
		  
		[XmlAttribute("PurchaseOrderDetRefNo")]
		[Bindable(true)]
		public string PurchaseOrderDetRefNo 
		{
			get { return GetColumnValue<string>(Columns.PurchaseOrderDetRefNo); }
			set { SetColumnValue(Columns.PurchaseOrderDetRefNo, value); }
		}
		  
		[XmlAttribute("Qty")]
		[Bindable(true)]
		public decimal? Qty 
		{
			get { return GetColumnValue<decimal?>(Columns.Qty); }
			set { SetColumnValue(Columns.Qty, value); }
		}
		  
		[XmlAttribute("QtyApproved")]
		[Bindable(true)]
		public decimal? QtyApproved 
		{
			get { return GetColumnValue<decimal?>(Columns.QtyApproved); }
			set { SetColumnValue(Columns.QtyApproved, value); }
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
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varOrderDetID,string varPurchaseOrderDetRefNo,decimal? varQty,decimal? varQtyApproved,bool varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			SalesOrderMapping item = new SalesOrderMapping();
			
			item.OrderDetID = varOrderDetID;
			
			item.PurchaseOrderDetRefNo = varPurchaseOrderDetRefNo;
			
			item.Qty = varQty;
			
			item.QtyApproved = varQtyApproved;
			
			item.Deleted = varDeleted;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varSalesOrderMappingID,string varOrderDetID,string varPurchaseOrderDetRefNo,decimal? varQty,decimal? varQtyApproved,bool varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			SalesOrderMapping item = new SalesOrderMapping();
			
				item.SalesOrderMappingID = varSalesOrderMappingID;
			
				item.OrderDetID = varOrderDetID;
			
				item.PurchaseOrderDetRefNo = varPurchaseOrderDetRefNo;
			
				item.Qty = varQty;
			
				item.QtyApproved = varQtyApproved;
			
				item.Deleted = varDeleted;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn SalesOrderMappingIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderDetIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn PurchaseOrderDetRefNoColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn QtyColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn QtyApprovedColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
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
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string SalesOrderMappingID = @"SalesOrderMappingID";
			 public static string OrderDetID = @"OrderDetID";
			 public static string PurchaseOrderDetRefNo = @"PurchaseOrderDetRefNo";
			 public static string Qty = @"Qty";
			 public static string QtyApproved = @"QtyApproved";
			 public static string Deleted = @"Deleted";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
