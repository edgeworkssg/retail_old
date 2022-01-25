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
	/// Strongly-typed collection for the CommissionBasedOnQty class.
	/// </summary>
    [Serializable]
	public partial class CommissionBasedOnQtyCollection : ActiveList<CommissionBasedOnQty, CommissionBasedOnQtyCollection>
	{	   
		public CommissionBasedOnQtyCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>CommissionBasedOnQtyCollection</returns>
		public CommissionBasedOnQtyCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                CommissionBasedOnQty o = this[i];
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
	/// This is an ActiveRecord class which wraps the CommissionBasedOnQty table.
	/// </summary>
	[Serializable]
	public partial class CommissionBasedOnQty : ActiveRecord<CommissionBasedOnQty>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public CommissionBasedOnQty()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public CommissionBasedOnQty(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public CommissionBasedOnQty(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public CommissionBasedOnQty(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("CommissionBasedOnQty", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
				colvarUniqueID.ColumnName = "UniqueID";
				colvarUniqueID.DataType = DbType.Int32;
				colvarUniqueID.MaxLength = 0;
				colvarUniqueID.AutoIncrement = true;
				colvarUniqueID.IsNullable = false;
				colvarUniqueID.IsPrimaryKey = true;
				colvarUniqueID.IsForeignKey = false;
				colvarUniqueID.IsReadOnly = false;
				colvarUniqueID.DefaultSetting = @"";
				colvarUniqueID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueID);
				
				TableSchema.TableColumn colvarSalesGroupID = new TableSchema.TableColumn(schema);
				colvarSalesGroupID.ColumnName = "SalesGroupID";
				colvarSalesGroupID.DataType = DbType.Int32;
				colvarSalesGroupID.MaxLength = 0;
				colvarSalesGroupID.AutoIncrement = false;
				colvarSalesGroupID.IsNullable = false;
				colvarSalesGroupID.IsPrimaryKey = false;
				colvarSalesGroupID.IsForeignKey = false;
				colvarSalesGroupID.IsReadOnly = false;
				colvarSalesGroupID.DefaultSetting = @"";
				colvarSalesGroupID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSalesGroupID);
				
				TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
				colvarItemNo.ColumnName = "ItemNo";
				colvarItemNo.DataType = DbType.AnsiString;
				colvarItemNo.MaxLength = 50;
				colvarItemNo.AutoIncrement = false;
				colvarItemNo.IsNullable = false;
				colvarItemNo.IsPrimaryKey = false;
				colvarItemNo.IsForeignKey = false;
				colvarItemNo.IsReadOnly = false;
				colvarItemNo.DefaultSetting = @"";
				colvarItemNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemNo);
				
				TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
				colvarQuantity.ColumnName = "Quantity";
				colvarQuantity.DataType = DbType.Decimal;
				colvarQuantity.MaxLength = 0;
				colvarQuantity.AutoIncrement = false;
				colvarQuantity.IsNullable = false;
				colvarQuantity.IsPrimaryKey = false;
				colvarQuantity.IsForeignKey = false;
				colvarQuantity.IsReadOnly = false;
				colvarQuantity.DefaultSetting = @"";
				colvarQuantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuantity);
				
				TableSchema.TableColumn colvarAmountCommission = new TableSchema.TableColumn(schema);
				colvarAmountCommission.ColumnName = "AmountCommission";
				colvarAmountCommission.DataType = DbType.Decimal;
				colvarAmountCommission.MaxLength = 0;
				colvarAmountCommission.AutoIncrement = false;
				colvarAmountCommission.IsNullable = false;
				colvarAmountCommission.IsPrimaryKey = false;
				colvarAmountCommission.IsForeignKey = false;
				colvarAmountCommission.IsReadOnly = false;
				colvarAmountCommission.DefaultSetting = @"";
				colvarAmountCommission.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmountCommission);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.AnsiString;
				colvarCreatedBy.MaxLength = 50;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = false;
				colvarCreatedBy.IsPrimaryKey = false;
				colvarCreatedBy.IsForeignKey = false;
				colvarCreatedBy.IsReadOnly = false;
				colvarCreatedBy.DefaultSetting = @"";
				colvarCreatedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedBy);
				
				TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
				colvarCreatedOn.ColumnName = "CreatedOn";
				colvarCreatedOn.DataType = DbType.DateTime;
				colvarCreatedOn.MaxLength = 0;
				colvarCreatedOn.AutoIncrement = false;
				colvarCreatedOn.IsNullable = false;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				colvarCreatedOn.DefaultSetting = @"";
				colvarCreatedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedOn);
				
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
				
				TableSchema.TableColumn colvarCommissionType = new TableSchema.TableColumn(schema);
				colvarCommissionType.ColumnName = "CommissionType";
				colvarCommissionType.DataType = DbType.AnsiString;
				colvarCommissionType.MaxLength = 25;
				colvarCommissionType.AutoIncrement = false;
				colvarCommissionType.IsNullable = true;
				colvarCommissionType.IsPrimaryKey = false;
				colvarCommissionType.IsForeignKey = false;
				colvarCommissionType.IsReadOnly = false;
				colvarCommissionType.DefaultSetting = @"";
				colvarCommissionType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCommissionType);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("CommissionBasedOnQty",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("UniqueID")]
		[Bindable(true)]
		public int UniqueID 
		{
			get { return GetColumnValue<int>(Columns.UniqueID); }
			set { SetColumnValue(Columns.UniqueID, value); }
		}
		  
		[XmlAttribute("SalesGroupID")]
		[Bindable(true)]
		public int SalesGroupID 
		{
			get { return GetColumnValue<int>(Columns.SalesGroupID); }
			set { SetColumnValue(Columns.SalesGroupID, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
		}
		  
		[XmlAttribute("Quantity")]
		[Bindable(true)]
		public decimal Quantity 
		{
			get { return GetColumnValue<decimal>(Columns.Quantity); }
			set { SetColumnValue(Columns.Quantity, value); }
		}
		  
		[XmlAttribute("AmountCommission")]
		[Bindable(true)]
		public decimal AmountCommission 
		{
			get { return GetColumnValue<decimal>(Columns.AmountCommission); }
			set { SetColumnValue(Columns.AmountCommission, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime CreatedOn 
		{
			get { return GetColumnValue<DateTime>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("CommissionType")]
		[Bindable(true)]
		public string CommissionType 
		{
			get { return GetColumnValue<string>(Columns.CommissionType); }
			set { SetColumnValue(Columns.CommissionType, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varSalesGroupID,string varItemNo,decimal varQuantity,decimal varAmountCommission,string varCreatedBy,DateTime varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,string varCommissionType)
		{
			CommissionBasedOnQty item = new CommissionBasedOnQty();
			
			item.SalesGroupID = varSalesGroupID;
			
			item.ItemNo = varItemNo;
			
			item.Quantity = varQuantity;
			
			item.AmountCommission = varAmountCommission;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.CommissionType = varCommissionType;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varUniqueID,int varSalesGroupID,string varItemNo,decimal varQuantity,decimal varAmountCommission,string varCreatedBy,DateTime varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,string varCommissionType)
		{
			CommissionBasedOnQty item = new CommissionBasedOnQty();
			
				item.UniqueID = varUniqueID;
			
				item.SalesGroupID = varSalesGroupID;
			
				item.ItemNo = varItemNo;
			
				item.Quantity = varQuantity;
			
				item.AmountCommission = varAmountCommission;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.CommissionType = varCommissionType;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn SalesGroupIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn QuantityColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn AmountCommissionColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn CommissionTypeColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string UniqueID = @"UniqueID";
			 public static string SalesGroupID = @"SalesGroupID";
			 public static string ItemNo = @"ItemNo";
			 public static string Quantity = @"Quantity";
			 public static string AmountCommission = @"AmountCommission";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string CommissionType = @"CommissionType";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
