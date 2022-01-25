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
	/// Strongly-typed collection for the DwHourlyProductSale class.
	/// </summary>
    [Serializable]
	public partial class DwHourlyProductSaleCollection : ActiveList<DwHourlyProductSale, DwHourlyProductSaleCollection>
	{	   
		public DwHourlyProductSaleCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>DwHourlyProductSaleCollection</returns>
		public DwHourlyProductSaleCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                DwHourlyProductSale o = this[i];
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
	/// This is an ActiveRecord class which wraps the DW_HourlyProductSales table.
	/// </summary>
	[Serializable]
	public partial class DwHourlyProductSale : ActiveRecord<DwHourlyProductSale>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public DwHourlyProductSale()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public DwHourlyProductSale(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public DwHourlyProductSale(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public DwHourlyProductSale(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("DW_HourlyProductSales", TableType.Table, DataService.GetInstance("PowerPOS"));
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
				
				TableSchema.TableColumn colvarOrderdate = new TableSchema.TableColumn(schema);
				colvarOrderdate.ColumnName = "orderdate";
				colvarOrderdate.DataType = DbType.DateTime;
				colvarOrderdate.MaxLength = 0;
				colvarOrderdate.AutoIncrement = false;
				colvarOrderdate.IsNullable = false;
				colvarOrderdate.IsPrimaryKey = false;
				colvarOrderdate.IsForeignKey = false;
				colvarOrderdate.IsReadOnly = false;
				colvarOrderdate.DefaultSetting = @"";
				colvarOrderdate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderdate);
				
				TableSchema.TableColumn colvarOutletname = new TableSchema.TableColumn(schema);
				colvarOutletname.ColumnName = "outletname";
				colvarOutletname.DataType = DbType.AnsiString;
				colvarOutletname.MaxLength = 50;
				colvarOutletname.AutoIncrement = false;
				colvarOutletname.IsNullable = false;
				colvarOutletname.IsPrimaryKey = false;
				colvarOutletname.IsForeignKey = false;
				colvarOutletname.IsReadOnly = false;
				colvarOutletname.DefaultSetting = @"";
				colvarOutletname.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOutletname);
				
				TableSchema.TableColumn colvarItemno = new TableSchema.TableColumn(schema);
				colvarItemno.ColumnName = "itemno";
				colvarItemno.DataType = DbType.AnsiString;
				colvarItemno.MaxLength = 50;
				colvarItemno.AutoIncrement = false;
				colvarItemno.IsNullable = false;
				colvarItemno.IsPrimaryKey = false;
				colvarItemno.IsForeignKey = false;
				colvarItemno.IsReadOnly = false;
				colvarItemno.DefaultSetting = @"";
				colvarItemno.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemno);
				
				TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
				colvarQuantity.ColumnName = "quantity";
				colvarQuantity.DataType = DbType.Decimal;
				colvarQuantity.MaxLength = 0;
				colvarQuantity.AutoIncrement = false;
				colvarQuantity.IsNullable = true;
				colvarQuantity.IsPrimaryKey = false;
				colvarQuantity.IsForeignKey = false;
				colvarQuantity.IsReadOnly = false;
				colvarQuantity.DefaultSetting = @"";
				colvarQuantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuantity);
				
				TableSchema.TableColumn colvarAmount = new TableSchema.TableColumn(schema);
				colvarAmount.ColumnName = "amount";
				colvarAmount.DataType = DbType.Currency;
				colvarAmount.MaxLength = 0;
				colvarAmount.AutoIncrement = false;
				colvarAmount.IsNullable = true;
				colvarAmount.IsPrimaryKey = false;
				colvarAmount.IsForeignKey = false;
				colvarAmount.IsReadOnly = false;
				colvarAmount.DefaultSetting = @"";
				colvarAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmount);
				
				TableSchema.TableColumn colvarRegenerate = new TableSchema.TableColumn(schema);
				colvarRegenerate.ColumnName = "regenerate";
				colvarRegenerate.DataType = DbType.Int32;
				colvarRegenerate.MaxLength = 0;
				colvarRegenerate.AutoIncrement = false;
				colvarRegenerate.IsNullable = true;
				colvarRegenerate.IsPrimaryKey = false;
				colvarRegenerate.IsForeignKey = false;
				colvarRegenerate.IsReadOnly = false;
				colvarRegenerate.DefaultSetting = @"";
				colvarRegenerate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRegenerate);
				
				TableSchema.TableColumn colvarLastUpdateOn = new TableSchema.TableColumn(schema);
				colvarLastUpdateOn.ColumnName = "LastUpdateOn";
				colvarLastUpdateOn.DataType = DbType.DateTime;
				colvarLastUpdateOn.MaxLength = 0;
				colvarLastUpdateOn.AutoIncrement = false;
				colvarLastUpdateOn.IsNullable = true;
				colvarLastUpdateOn.IsPrimaryKey = false;
				colvarLastUpdateOn.IsForeignKey = false;
				colvarLastUpdateOn.IsReadOnly = false;
				colvarLastUpdateOn.DefaultSetting = @"";
				colvarLastUpdateOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastUpdateOn);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("DW_HourlyProductSales",schema);
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
		  
		[XmlAttribute("Orderdate")]
		[Bindable(true)]
		public DateTime Orderdate 
		{
			get { return GetColumnValue<DateTime>(Columns.Orderdate); }
			set { SetColumnValue(Columns.Orderdate, value); }
		}
		  
		[XmlAttribute("Outletname")]
		[Bindable(true)]
		public string Outletname 
		{
			get { return GetColumnValue<string>(Columns.Outletname); }
			set { SetColumnValue(Columns.Outletname, value); }
		}
		  
		[XmlAttribute("Itemno")]
		[Bindable(true)]
		public string Itemno 
		{
			get { return GetColumnValue<string>(Columns.Itemno); }
			set { SetColumnValue(Columns.Itemno, value); }
		}
		  
		[XmlAttribute("Quantity")]
		[Bindable(true)]
		public decimal? Quantity 
		{
			get { return GetColumnValue<decimal?>(Columns.Quantity); }
			set { SetColumnValue(Columns.Quantity, value); }
		}
		  
		[XmlAttribute("Amount")]
		[Bindable(true)]
		public decimal? Amount 
		{
			get { return GetColumnValue<decimal?>(Columns.Amount); }
			set { SetColumnValue(Columns.Amount, value); }
		}
		  
		[XmlAttribute("Regenerate")]
		[Bindable(true)]
		public int? Regenerate 
		{
			get { return GetColumnValue<int?>(Columns.Regenerate); }
			set { SetColumnValue(Columns.Regenerate, value); }
		}
		  
		[XmlAttribute("LastUpdateOn")]
		[Bindable(true)]
		public DateTime? LastUpdateOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.LastUpdateOn); }
			set { SetColumnValue(Columns.LastUpdateOn, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varOrderdate,string varOutletname,string varItemno,decimal? varQuantity,decimal? varAmount,int? varRegenerate,DateTime? varLastUpdateOn)
		{
			DwHourlyProductSale item = new DwHourlyProductSale();
			
			item.Orderdate = varOrderdate;
			
			item.Outletname = varOutletname;
			
			item.Itemno = varItemno;
			
			item.Quantity = varQuantity;
			
			item.Amount = varAmount;
			
			item.Regenerate = varRegenerate;
			
			item.LastUpdateOn = varLastUpdateOn;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varOrderdate,string varOutletname,string varItemno,decimal? varQuantity,decimal? varAmount,int? varRegenerate,DateTime? varLastUpdateOn)
		{
			DwHourlyProductSale item = new DwHourlyProductSale();
			
				item.Id = varId;
			
				item.Orderdate = varOrderdate;
			
				item.Outletname = varOutletname;
			
				item.Itemno = varItemno;
			
				item.Quantity = varQuantity;
			
				item.Amount = varAmount;
			
				item.Regenerate = varRegenerate;
			
				item.LastUpdateOn = varLastUpdateOn;
			
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
        
        
        
        public static TableSchema.TableColumn OrderdateColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn OutletnameColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemnoColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn QuantityColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn AmountColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn RegenerateColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn LastUpdateOnColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"ID";
			 public static string Orderdate = @"orderdate";
			 public static string Outletname = @"outletname";
			 public static string Itemno = @"itemno";
			 public static string Quantity = @"quantity";
			 public static string Amount = @"amount";
			 public static string Regenerate = @"regenerate";
			 public static string LastUpdateOn = @"LastUpdateOn";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
