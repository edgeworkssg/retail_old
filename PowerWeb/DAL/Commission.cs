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
	/// Strongly-typed collection for the Commission class.
	/// </summary>
    [Serializable]
	public partial class CommissionCollection : ActiveList<Commission, CommissionCollection>
	{	   
		public CommissionCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>CommissionCollection</returns>
		public CommissionCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Commission o = this[i];
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
	/// This is an ActiveRecord class which wraps the Commission table.
	/// </summary>
	[Serializable]
	public partial class Commission : ActiveRecord<Commission>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Commission()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Commission(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Commission(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Commission(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Commission", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarCommissionID = new TableSchema.TableColumn(schema);
				colvarCommissionID.ColumnName = "CommissionID";
				colvarCommissionID.DataType = DbType.Int32;
				colvarCommissionID.MaxLength = 0;
				colvarCommissionID.AutoIncrement = true;
				colvarCommissionID.IsNullable = false;
				colvarCommissionID.IsPrimaryKey = true;
				colvarCommissionID.IsForeignKey = false;
				colvarCommissionID.IsReadOnly = false;
				colvarCommissionID.DefaultSetting = @"";
				colvarCommissionID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCommissionID);
				
				TableSchema.TableColumn colvarSalesGroupID = new TableSchema.TableColumn(schema);
				colvarSalesGroupID.ColumnName = "SalesGroupID";
				colvarSalesGroupID.DataType = DbType.Int32;
				colvarSalesGroupID.MaxLength = 0;
				colvarSalesGroupID.AutoIncrement = false;
				colvarSalesGroupID.IsNullable = true;
				colvarSalesGroupID.IsPrimaryKey = false;
				colvarSalesGroupID.IsForeignKey = false;
				colvarSalesGroupID.IsReadOnly = false;
				colvarSalesGroupID.DefaultSetting = @"";
				colvarSalesGroupID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSalesGroupID);
				
				TableSchema.TableColumn colvarCommissionType = new TableSchema.TableColumn(schema);
				colvarCommissionType.ColumnName = "CommissionType";
				colvarCommissionType.DataType = DbType.String;
				colvarCommissionType.MaxLength = 20;
				colvarCommissionType.AutoIncrement = false;
				colvarCommissionType.IsNullable = true;
				colvarCommissionType.IsPrimaryKey = false;
				colvarCommissionType.IsForeignKey = false;
				colvarCommissionType.IsReadOnly = false;
				colvarCommissionType.DefaultSetting = @"";
				colvarCommissionType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCommissionType);
				
				TableSchema.TableColumn colvarCategoryName = new TableSchema.TableColumn(schema);
				colvarCategoryName.ColumnName = "CategoryName";
				colvarCategoryName.DataType = DbType.String;
				colvarCategoryName.MaxLength = 250;
				colvarCategoryName.AutoIncrement = false;
				colvarCategoryName.IsNullable = true;
				colvarCategoryName.IsPrimaryKey = false;
				colvarCategoryName.IsForeignKey = false;
				colvarCategoryName.IsReadOnly = false;
				colvarCategoryName.DefaultSetting = @"";
				colvarCategoryName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCategoryName);
				
				TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
				colvarItemNo.ColumnName = "ItemNo";
				colvarItemNo.DataType = DbType.String;
				colvarItemNo.MaxLength = 50;
				colvarItemNo.AutoIncrement = false;
				colvarItemNo.IsNullable = true;
				colvarItemNo.IsPrimaryKey = false;
				colvarItemNo.IsForeignKey = false;
				colvarItemNo.IsReadOnly = false;
				colvarItemNo.DefaultSetting = @"";
				colvarItemNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemNo);
				
				TableSchema.TableColumn colvarCommissionBasedOn = new TableSchema.TableColumn(schema);
				colvarCommissionBasedOn.ColumnName = "CommissionBasedOn";
				colvarCommissionBasedOn.DataType = DbType.String;
				colvarCommissionBasedOn.MaxLength = 50;
				colvarCommissionBasedOn.AutoIncrement = false;
				colvarCommissionBasedOn.IsNullable = true;
				colvarCommissionBasedOn.IsPrimaryKey = false;
				colvarCommissionBasedOn.IsForeignKey = false;
				colvarCommissionBasedOn.IsReadOnly = false;
				colvarCommissionBasedOn.DefaultSetting = @"";
				colvarCommissionBasedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCommissionBasedOn);
				
				TableSchema.TableColumn colvarQuantityFrom = new TableSchema.TableColumn(schema);
				colvarQuantityFrom.ColumnName = "QuantityFrom";
				colvarQuantityFrom.DataType = DbType.Double;
				colvarQuantityFrom.MaxLength = 0;
				colvarQuantityFrom.AutoIncrement = false;
				colvarQuantityFrom.IsNullable = true;
				colvarQuantityFrom.IsPrimaryKey = false;
				colvarQuantityFrom.IsForeignKey = false;
				colvarQuantityFrom.IsReadOnly = false;
				colvarQuantityFrom.DefaultSetting = @"";
				colvarQuantityFrom.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuantityFrom);
				
				TableSchema.TableColumn colvarQuantityTo = new TableSchema.TableColumn(schema);
				colvarQuantityTo.ColumnName = "QuantityTo";
				colvarQuantityTo.DataType = DbType.Double;
				colvarQuantityTo.MaxLength = 0;
				colvarQuantityTo.AutoIncrement = false;
				colvarQuantityTo.IsNullable = true;
				colvarQuantityTo.IsPrimaryKey = false;
				colvarQuantityTo.IsForeignKey = false;
				colvarQuantityTo.IsReadOnly = false;
				colvarQuantityTo.DefaultSetting = @"";
				colvarQuantityTo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuantityTo);
				
				TableSchema.TableColumn colvarAmountCommission = new TableSchema.TableColumn(schema);
				colvarAmountCommission.ColumnName = "AmountCommission";
				colvarAmountCommission.DataType = DbType.Currency;
				colvarAmountCommission.MaxLength = 0;
				colvarAmountCommission.AutoIncrement = false;
				colvarAmountCommission.IsNullable = true;
				colvarAmountCommission.IsPrimaryKey = false;
				colvarAmountCommission.IsForeignKey = false;
				colvarAmountCommission.IsReadOnly = false;
				colvarAmountCommission.DefaultSetting = @"";
				colvarAmountCommission.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmountCommission);
				
				TableSchema.TableColumn colvarAmountFrom = new TableSchema.TableColumn(schema);
				colvarAmountFrom.ColumnName = "AmountFrom";
				colvarAmountFrom.DataType = DbType.Currency;
				colvarAmountFrom.MaxLength = 0;
				colvarAmountFrom.AutoIncrement = false;
				colvarAmountFrom.IsNullable = true;
				colvarAmountFrom.IsPrimaryKey = false;
				colvarAmountFrom.IsForeignKey = false;
				colvarAmountFrom.IsReadOnly = false;
				colvarAmountFrom.DefaultSetting = @"";
				colvarAmountFrom.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmountFrom);
				
				TableSchema.TableColumn colvarAmountTo = new TableSchema.TableColumn(schema);
				colvarAmountTo.ColumnName = "AmountTo";
				colvarAmountTo.DataType = DbType.Currency;
				colvarAmountTo.MaxLength = 0;
				colvarAmountTo.AutoIncrement = false;
				colvarAmountTo.IsNullable = true;
				colvarAmountTo.IsPrimaryKey = false;
				colvarAmountTo.IsForeignKey = false;
				colvarAmountTo.IsReadOnly = false;
				colvarAmountTo.DefaultSetting = @"";
				colvarAmountTo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmountTo);
				
				TableSchema.TableColumn colvarPercentageCommission = new TableSchema.TableColumn(schema);
				colvarPercentageCommission.ColumnName = "PercentageCommission";
				colvarPercentageCommission.DataType = DbType.Double;
				colvarPercentageCommission.MaxLength = 0;
				colvarPercentageCommission.AutoIncrement = false;
				colvarPercentageCommission.IsNullable = true;
				colvarPercentageCommission.IsPrimaryKey = false;
				colvarPercentageCommission.IsForeignKey = false;
				colvarPercentageCommission.IsReadOnly = false;
				colvarPercentageCommission.DefaultSetting = @"";
				colvarPercentageCommission.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPercentageCommission);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.String;
				colvarCreatedBy.MaxLength = 50;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = true;
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
				colvarCreatedOn.IsNullable = true;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				colvarCreatedOn.DefaultSetting = @"";
				colvarCreatedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedOn);
				
				TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
				colvarModifiedBy.ColumnName = "ModifiedBy";
				colvarModifiedBy.DataType = DbType.String;
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Commission",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("CommissionID")]
		[Bindable(true)]
		public int CommissionID 
		{
			get { return GetColumnValue<int>(Columns.CommissionID); }
			set { SetColumnValue(Columns.CommissionID, value); }
		}
		  
		[XmlAttribute("SalesGroupID")]
		[Bindable(true)]
		public int? SalesGroupID 
		{
			get { return GetColumnValue<int?>(Columns.SalesGroupID); }
			set { SetColumnValue(Columns.SalesGroupID, value); }
		}
		  
		[XmlAttribute("CommissionType")]
		[Bindable(true)]
		public string CommissionType 
		{
			get { return GetColumnValue<string>(Columns.CommissionType); }
			set { SetColumnValue(Columns.CommissionType, value); }
		}
		  
		[XmlAttribute("CategoryName")]
		[Bindable(true)]
		public string CategoryName 
		{
			get { return GetColumnValue<string>(Columns.CategoryName); }
			set { SetColumnValue(Columns.CategoryName, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
		}
		  
		[XmlAttribute("CommissionBasedOn")]
		[Bindable(true)]
		public string CommissionBasedOn 
		{
			get { return GetColumnValue<string>(Columns.CommissionBasedOn); }
			set { SetColumnValue(Columns.CommissionBasedOn, value); }
		}
		  
		[XmlAttribute("QuantityFrom")]
		[Bindable(true)]
		public double? QuantityFrom 
		{
			get { return GetColumnValue<double?>(Columns.QuantityFrom); }
			set { SetColumnValue(Columns.QuantityFrom, value); }
		}
		  
		[XmlAttribute("QuantityTo")]
		[Bindable(true)]
		public double? QuantityTo 
		{
			get { return GetColumnValue<double?>(Columns.QuantityTo); }
			set { SetColumnValue(Columns.QuantityTo, value); }
		}
		  
		[XmlAttribute("AmountCommission")]
		[Bindable(true)]
		public decimal? AmountCommission 
		{
			get { return GetColumnValue<decimal?>(Columns.AmountCommission); }
			set { SetColumnValue(Columns.AmountCommission, value); }
		}
		  
		[XmlAttribute("AmountFrom")]
		[Bindable(true)]
		public decimal? AmountFrom 
		{
			get { return GetColumnValue<decimal?>(Columns.AmountFrom); }
			set { SetColumnValue(Columns.AmountFrom, value); }
		}
		  
		[XmlAttribute("AmountTo")]
		[Bindable(true)]
		public decimal? AmountTo 
		{
			get { return GetColumnValue<decimal?>(Columns.AmountTo); }
			set { SetColumnValue(Columns.AmountTo, value); }
		}
		  
		[XmlAttribute("PercentageCommission")]
		[Bindable(true)]
		public double? PercentageCommission 
		{
			get { return GetColumnValue<double?>(Columns.PercentageCommission); }
			set { SetColumnValue(Columns.PercentageCommission, value); }
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
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
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
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int? varSalesGroupID,string varCommissionType,string varCategoryName,string varItemNo,string varCommissionBasedOn,double? varQuantityFrom,double? varQuantityTo,decimal? varAmountCommission,decimal? varAmountFrom,decimal? varAmountTo,double? varPercentageCommission,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn)
		{
			Commission item = new Commission();
			
			item.SalesGroupID = varSalesGroupID;
			
			item.CommissionType = varCommissionType;
			
			item.CategoryName = varCategoryName;
			
			item.ItemNo = varItemNo;
			
			item.CommissionBasedOn = varCommissionBasedOn;
			
			item.QuantityFrom = varQuantityFrom;
			
			item.QuantityTo = varQuantityTo;
			
			item.AmountCommission = varAmountCommission;
			
			item.AmountFrom = varAmountFrom;
			
			item.AmountTo = varAmountTo;
			
			item.PercentageCommission = varPercentageCommission;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.ModifiedOn = varModifiedOn;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varCommissionID,int? varSalesGroupID,string varCommissionType,string varCategoryName,string varItemNo,string varCommissionBasedOn,double? varQuantityFrom,double? varQuantityTo,decimal? varAmountCommission,decimal? varAmountFrom,decimal? varAmountTo,double? varPercentageCommission,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn)
		{
			Commission item = new Commission();
			
				item.CommissionID = varCommissionID;
			
				item.SalesGroupID = varSalesGroupID;
			
				item.CommissionType = varCommissionType;
			
				item.CategoryName = varCategoryName;
			
				item.ItemNo = varItemNo;
			
				item.CommissionBasedOn = varCommissionBasedOn;
			
				item.QuantityFrom = varQuantityFrom;
			
				item.QuantityTo = varQuantityTo;
			
				item.AmountCommission = varAmountCommission;
			
				item.AmountFrom = varAmountFrom;
			
				item.AmountTo = varAmountTo;
			
				item.PercentageCommission = varPercentageCommission;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.ModifiedOn = varModifiedOn;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn CommissionIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn SalesGroupIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn CommissionTypeColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CategoryNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CommissionBasedOnColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn QuantityFromColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn QuantityToColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn AmountCommissionColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn AmountFromColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn AmountToColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn PercentageCommissionColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string CommissionID = @"CommissionID";
			 public static string SalesGroupID = @"SalesGroupID";
			 public static string CommissionType = @"CommissionType";
			 public static string CategoryName = @"CategoryName";
			 public static string ItemNo = @"ItemNo";
			 public static string CommissionBasedOn = @"CommissionBasedOn";
			 public static string QuantityFrom = @"QuantityFrom";
			 public static string QuantityTo = @"QuantityTo";
			 public static string AmountCommission = @"AmountCommission";
			 public static string AmountFrom = @"AmountFrom";
			 public static string AmountTo = @"AmountTo";
			 public static string PercentageCommission = @"PercentageCommission";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
