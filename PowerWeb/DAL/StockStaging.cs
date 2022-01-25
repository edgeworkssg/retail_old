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
	/// Strongly-typed collection for the StockStaging class.
	/// </summary>
    [Serializable]
	public partial class StockStagingCollection : ActiveList<StockStaging, StockStagingCollection>
	{	   
		public StockStagingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>StockStagingCollection</returns>
		public StockStagingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                StockStaging o = this[i];
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
	/// This is an ActiveRecord class which wraps the StockStaging table.
	/// </summary>
	[Serializable]
	public partial class StockStaging : ActiveRecord<StockStaging>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public StockStaging()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public StockStaging(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public StockStaging(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public StockStaging(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("StockStaging", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "ID";
				colvarId.DataType = DbType.Int32;
				colvarId.MaxLength = 0;
				colvarId.AutoIncrement = true;
				colvarId.IsNullable = false;
				colvarId.IsPrimaryKey = true;
				colvarId.IsForeignKey = false;
				colvarId.IsReadOnly = false;
				colvarId.DefaultSetting = @"";
				colvarId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarId);
				
				TableSchema.TableColumn colvarInventoryDate = new TableSchema.TableColumn(schema);
				colvarInventoryDate.ColumnName = "InventoryDate";
				colvarInventoryDate.DataType = DbType.DateTime;
				colvarInventoryDate.MaxLength = 0;
				colvarInventoryDate.AutoIncrement = false;
				colvarInventoryDate.IsNullable = true;
				colvarInventoryDate.IsPrimaryKey = false;
				colvarInventoryDate.IsForeignKey = false;
				colvarInventoryDate.IsReadOnly = false;
				colvarInventoryDate.DefaultSetting = @"";
				colvarInventoryDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInventoryDate);
				
				TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
				colvarItemNo.ColumnName = "ItemNo";
				colvarItemNo.DataType = DbType.AnsiString;
				colvarItemNo.MaxLength = 50;
				colvarItemNo.AutoIncrement = false;
				colvarItemNo.IsNullable = true;
				colvarItemNo.IsPrimaryKey = false;
				colvarItemNo.IsForeignKey = false;
				colvarItemNo.IsReadOnly = false;
				colvarItemNo.DefaultSetting = @"";
				colvarItemNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemNo);
				
				TableSchema.TableColumn colvarInventoryLocationID = new TableSchema.TableColumn(schema);
				colvarInventoryLocationID.ColumnName = "InventoryLocationID";
				colvarInventoryLocationID.DataType = DbType.Int32;
				colvarInventoryLocationID.MaxLength = 0;
				colvarInventoryLocationID.AutoIncrement = false;
				colvarInventoryLocationID.IsNullable = true;
				colvarInventoryLocationID.IsPrimaryKey = false;
				colvarInventoryLocationID.IsForeignKey = false;
				colvarInventoryLocationID.IsReadOnly = false;
				colvarInventoryLocationID.DefaultSetting = @"";
				colvarInventoryLocationID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInventoryLocationID);
				
				TableSchema.TableColumn colvarBalanceQty = new TableSchema.TableColumn(schema);
				colvarBalanceQty.ColumnName = "BalanceQty";
				colvarBalanceQty.DataType = DbType.Double;
				colvarBalanceQty.MaxLength = 0;
				colvarBalanceQty.AutoIncrement = false;
				colvarBalanceQty.IsNullable = true;
				colvarBalanceQty.IsPrimaryKey = false;
				colvarBalanceQty.IsForeignKey = false;
				colvarBalanceQty.IsReadOnly = false;
				colvarBalanceQty.DefaultSetting = @"";
				colvarBalanceQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBalanceQty);
				
				TableSchema.TableColumn colvarCostPriceByItem = new TableSchema.TableColumn(schema);
				colvarCostPriceByItem.ColumnName = "CostPriceByItem";
				colvarCostPriceByItem.DataType = DbType.Currency;
				colvarCostPriceByItem.MaxLength = 0;
				colvarCostPriceByItem.AutoIncrement = false;
				colvarCostPriceByItem.IsNullable = true;
				colvarCostPriceByItem.IsPrimaryKey = false;
				colvarCostPriceByItem.IsForeignKey = false;
				colvarCostPriceByItem.IsReadOnly = false;
				colvarCostPriceByItem.DefaultSetting = @"";
				colvarCostPriceByItem.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPriceByItem);
				
				TableSchema.TableColumn colvarCostPriceByItemInvLoc = new TableSchema.TableColumn(schema);
				colvarCostPriceByItemInvLoc.ColumnName = "CostPriceByItemInvLoc";
				colvarCostPriceByItemInvLoc.DataType = DbType.Currency;
				colvarCostPriceByItemInvLoc.MaxLength = 0;
				colvarCostPriceByItemInvLoc.AutoIncrement = false;
				colvarCostPriceByItemInvLoc.IsNullable = true;
				colvarCostPriceByItemInvLoc.IsPrimaryKey = false;
				colvarCostPriceByItemInvLoc.IsForeignKey = false;
				colvarCostPriceByItemInvLoc.IsReadOnly = false;
				colvarCostPriceByItemInvLoc.DefaultSetting = @"";
				colvarCostPriceByItemInvLoc.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPriceByItemInvLoc);
				
				TableSchema.TableColumn colvarCostPriceByItemInvGroup = new TableSchema.TableColumn(schema);
				colvarCostPriceByItemInvGroup.ColumnName = "CostPriceByItemInvGroup";
				colvarCostPriceByItemInvGroup.DataType = DbType.Currency;
				colvarCostPriceByItemInvGroup.MaxLength = 0;
				colvarCostPriceByItemInvGroup.AutoIncrement = false;
				colvarCostPriceByItemInvGroup.IsNullable = true;
				colvarCostPriceByItemInvGroup.IsPrimaryKey = false;
				colvarCostPriceByItemInvGroup.IsForeignKey = false;
				colvarCostPriceByItemInvGroup.IsReadOnly = false;
				colvarCostPriceByItemInvGroup.DefaultSetting = @"";
				colvarCostPriceByItemInvGroup.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPriceByItemInvGroup);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("StockStaging",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Id")]
		[Bindable(true)]
		public int Id 
		{
			get { return GetColumnValue<int>(Columns.Id); }
			set { SetColumnValue(Columns.Id, value); }
		}
		  
		[XmlAttribute("InventoryDate")]
		[Bindable(true)]
		public DateTime? InventoryDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.InventoryDate); }
			set { SetColumnValue(Columns.InventoryDate, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
		}
		  
		[XmlAttribute("InventoryLocationID")]
		[Bindable(true)]
		public int? InventoryLocationID 
		{
			get { return GetColumnValue<int?>(Columns.InventoryLocationID); }
			set { SetColumnValue(Columns.InventoryLocationID, value); }
		}
		  
		[XmlAttribute("BalanceQty")]
		[Bindable(true)]
		public double? BalanceQty 
		{
			get { return GetColumnValue<double?>(Columns.BalanceQty); }
			set { SetColumnValue(Columns.BalanceQty, value); }
		}
		  
		[XmlAttribute("CostPriceByItem")]
		[Bindable(true)]
		public decimal? CostPriceByItem 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPriceByItem); }
			set { SetColumnValue(Columns.CostPriceByItem, value); }
		}
		  
		[XmlAttribute("CostPriceByItemInvLoc")]
		[Bindable(true)]
		public decimal? CostPriceByItemInvLoc 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPriceByItemInvLoc); }
			set { SetColumnValue(Columns.CostPriceByItemInvLoc, value); }
		}
		  
		[XmlAttribute("CostPriceByItemInvGroup")]
		[Bindable(true)]
		public decimal? CostPriceByItemInvGroup 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPriceByItemInvGroup); }
			set { SetColumnValue(Columns.CostPriceByItemInvGroup, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime? varInventoryDate,string varItemNo,int? varInventoryLocationID,double? varBalanceQty,decimal? varCostPriceByItem,decimal? varCostPriceByItemInvLoc,decimal? varCostPriceByItemInvGroup)
		{
			StockStaging item = new StockStaging();
			
			item.InventoryDate = varInventoryDate;
			
			item.ItemNo = varItemNo;
			
			item.InventoryLocationID = varInventoryLocationID;
			
			item.BalanceQty = varBalanceQty;
			
			item.CostPriceByItem = varCostPriceByItem;
			
			item.CostPriceByItemInvLoc = varCostPriceByItemInvLoc;
			
			item.CostPriceByItemInvGroup = varCostPriceByItemInvGroup;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime? varInventoryDate,string varItemNo,int? varInventoryLocationID,double? varBalanceQty,decimal? varCostPriceByItem,decimal? varCostPriceByItemInvLoc,decimal? varCostPriceByItemInvGroup)
		{
			StockStaging item = new StockStaging();
			
				item.Id = varId;
			
				item.InventoryDate = varInventoryDate;
			
				item.ItemNo = varItemNo;
			
				item.InventoryLocationID = varInventoryLocationID;
			
				item.BalanceQty = varBalanceQty;
			
				item.CostPriceByItem = varCostPriceByItem;
			
				item.CostPriceByItemInvLoc = varCostPriceByItemInvLoc;
			
				item.CostPriceByItemInvGroup = varCostPriceByItemInvGroup;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn InventoryDateColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn InventoryLocationIDColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BalanceQtyColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPriceByItemColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPriceByItemInvLocColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPriceByItemInvGroupColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"ID";
			 public static string InventoryDate = @"InventoryDate";
			 public static string ItemNo = @"ItemNo";
			 public static string InventoryLocationID = @"InventoryLocationID";
			 public static string BalanceQty = @"BalanceQty";
			 public static string CostPriceByItem = @"CostPriceByItem";
			 public static string CostPriceByItemInvLoc = @"CostPriceByItemInvLoc";
			 public static string CostPriceByItemInvGroup = @"CostPriceByItemInvGroup";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
