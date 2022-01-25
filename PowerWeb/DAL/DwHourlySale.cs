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
	/// Strongly-typed collection for the DwHourlySale class.
	/// </summary>
    [Serializable]
	public partial class DwHourlySaleCollection : ActiveList<DwHourlySale, DwHourlySaleCollection>
	{	   
		public DwHourlySaleCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>DwHourlySaleCollection</returns>
		public DwHourlySaleCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                DwHourlySale o = this[i];
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
	/// This is an ActiveRecord class which wraps the DW_HourlySales table.
	/// </summary>
	[Serializable]
	public partial class DwHourlySale : ActiveRecord<DwHourlySale>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public DwHourlySale()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public DwHourlySale(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public DwHourlySale(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public DwHourlySale(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("DW_HourlySales", TableType.Table, DataService.GetInstance("PowerPOS"));
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
				
				TableSchema.TableColumn colvarPax = new TableSchema.TableColumn(schema);
				colvarPax.ColumnName = "pax";
				colvarPax.DataType = DbType.Int32;
				colvarPax.MaxLength = 0;
				colvarPax.AutoIncrement = false;
				colvarPax.IsNullable = true;
				colvarPax.IsPrimaryKey = false;
				colvarPax.IsForeignKey = false;
				colvarPax.IsReadOnly = false;
				colvarPax.DefaultSetting = @"";
				colvarPax.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPax);
				
				TableSchema.TableColumn colvarBill = new TableSchema.TableColumn(schema);
				colvarBill.ColumnName = "bill";
				colvarBill.DataType = DbType.Int32;
				colvarBill.MaxLength = 0;
				colvarBill.AutoIncrement = false;
				colvarBill.IsNullable = true;
				colvarBill.IsPrimaryKey = false;
				colvarBill.IsForeignKey = false;
				colvarBill.IsReadOnly = false;
				colvarBill.DefaultSetting = @"";
				colvarBill.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBill);
				
				TableSchema.TableColumn colvarGrossamount = new TableSchema.TableColumn(schema);
				colvarGrossamount.ColumnName = "grossamount";
				colvarGrossamount.DataType = DbType.Currency;
				colvarGrossamount.MaxLength = 0;
				colvarGrossamount.AutoIncrement = false;
				colvarGrossamount.IsNullable = true;
				colvarGrossamount.IsPrimaryKey = false;
				colvarGrossamount.IsForeignKey = false;
				colvarGrossamount.IsReadOnly = false;
				colvarGrossamount.DefaultSetting = @"";
				colvarGrossamount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGrossamount);
				
				TableSchema.TableColumn colvarDisc = new TableSchema.TableColumn(schema);
				colvarDisc.ColumnName = "disc";
				colvarDisc.DataType = DbType.Currency;
				colvarDisc.MaxLength = 0;
				colvarDisc.AutoIncrement = false;
				colvarDisc.IsNullable = true;
				colvarDisc.IsPrimaryKey = false;
				colvarDisc.IsForeignKey = false;
				colvarDisc.IsReadOnly = false;
				colvarDisc.DefaultSetting = @"";
				colvarDisc.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisc);
				
				TableSchema.TableColumn colvarAfterdisc = new TableSchema.TableColumn(schema);
				colvarAfterdisc.ColumnName = "afterdisc";
				colvarAfterdisc.DataType = DbType.Currency;
				colvarAfterdisc.MaxLength = 0;
				colvarAfterdisc.AutoIncrement = false;
				colvarAfterdisc.IsNullable = true;
				colvarAfterdisc.IsPrimaryKey = false;
				colvarAfterdisc.IsForeignKey = false;
				colvarAfterdisc.IsReadOnly = false;
				colvarAfterdisc.DefaultSetting = @"";
				colvarAfterdisc.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAfterdisc);
				
				TableSchema.TableColumn colvarSvccharge = new TableSchema.TableColumn(schema);
				colvarSvccharge.ColumnName = "svccharge";
				colvarSvccharge.DataType = DbType.Currency;
				colvarSvccharge.MaxLength = 0;
				colvarSvccharge.AutoIncrement = false;
				colvarSvccharge.IsNullable = true;
				colvarSvccharge.IsPrimaryKey = false;
				colvarSvccharge.IsForeignKey = false;
				colvarSvccharge.IsReadOnly = false;
				colvarSvccharge.DefaultSetting = @"";
				colvarSvccharge.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSvccharge);
				
				TableSchema.TableColumn colvarBefgst = new TableSchema.TableColumn(schema);
				colvarBefgst.ColumnName = "befgst";
				colvarBefgst.DataType = DbType.Currency;
				colvarBefgst.MaxLength = 0;
				colvarBefgst.AutoIncrement = false;
				colvarBefgst.IsNullable = true;
				colvarBefgst.IsPrimaryKey = false;
				colvarBefgst.IsForeignKey = false;
				colvarBefgst.IsReadOnly = false;
				colvarBefgst.DefaultSetting = @"";
				colvarBefgst.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBefgst);
				
				TableSchema.TableColumn colvarGst = new TableSchema.TableColumn(schema);
				colvarGst.ColumnName = "gst";
				colvarGst.DataType = DbType.Currency;
				colvarGst.MaxLength = 0;
				colvarGst.AutoIncrement = false;
				colvarGst.IsNullable = true;
				colvarGst.IsPrimaryKey = false;
				colvarGst.IsForeignKey = false;
				colvarGst.IsReadOnly = false;
				colvarGst.DefaultSetting = @"";
				colvarGst.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGst);
				
				TableSchema.TableColumn colvarRounding = new TableSchema.TableColumn(schema);
				colvarRounding.ColumnName = "rounding";
				colvarRounding.DataType = DbType.Currency;
				colvarRounding.MaxLength = 0;
				colvarRounding.AutoIncrement = false;
				colvarRounding.IsNullable = true;
				colvarRounding.IsPrimaryKey = false;
				colvarRounding.IsForeignKey = false;
				colvarRounding.IsReadOnly = false;
				colvarRounding.DefaultSetting = @"";
				colvarRounding.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRounding);
				
				TableSchema.TableColumn colvarNettamount = new TableSchema.TableColumn(schema);
				colvarNettamount.ColumnName = "nettamount";
				colvarNettamount.DataType = DbType.Currency;
				colvarNettamount.MaxLength = 0;
				colvarNettamount.AutoIncrement = false;
				colvarNettamount.IsNullable = true;
				colvarNettamount.IsPrimaryKey = false;
				colvarNettamount.IsForeignKey = false;
				colvarNettamount.IsReadOnly = false;
				colvarNettamount.DefaultSetting = @"";
				colvarNettamount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNettamount);
				
				TableSchema.TableColumn colvarPointSale = new TableSchema.TableColumn(schema);
				colvarPointSale.ColumnName = "pointSale";
				colvarPointSale.DataType = DbType.Currency;
				colvarPointSale.MaxLength = 0;
				colvarPointSale.AutoIncrement = false;
				colvarPointSale.IsNullable = true;
				colvarPointSale.IsPrimaryKey = false;
				colvarPointSale.IsForeignKey = false;
				colvarPointSale.IsReadOnly = false;
				colvarPointSale.DefaultSetting = @"";
				colvarPointSale.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointSale);
				
				TableSchema.TableColumn colvarInstallmentPaymentSale = new TableSchema.TableColumn(schema);
				colvarInstallmentPaymentSale.ColumnName = "installmentPaymentSale";
				colvarInstallmentPaymentSale.DataType = DbType.Currency;
				colvarInstallmentPaymentSale.MaxLength = 0;
				colvarInstallmentPaymentSale.AutoIncrement = false;
				colvarInstallmentPaymentSale.IsNullable = true;
				colvarInstallmentPaymentSale.IsPrimaryKey = false;
				colvarInstallmentPaymentSale.IsForeignKey = false;
				colvarInstallmentPaymentSale.IsReadOnly = false;
				colvarInstallmentPaymentSale.DefaultSetting = @"";
				colvarInstallmentPaymentSale.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInstallmentPaymentSale);
				
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
				DataService.Providers["PowerPOS"].AddSchema("DW_HourlySales",schema);
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
		  
		[XmlAttribute("Pax")]
		[Bindable(true)]
		public int? Pax 
		{
			get { return GetColumnValue<int?>(Columns.Pax); }
			set { SetColumnValue(Columns.Pax, value); }
		}
		  
		[XmlAttribute("Bill")]
		[Bindable(true)]
		public int? Bill 
		{
			get { return GetColumnValue<int?>(Columns.Bill); }
			set { SetColumnValue(Columns.Bill, value); }
		}
		  
		[XmlAttribute("Grossamount")]
		[Bindable(true)]
		public decimal? Grossamount 
		{
			get { return GetColumnValue<decimal?>(Columns.Grossamount); }
			set { SetColumnValue(Columns.Grossamount, value); }
		}
		  
		[XmlAttribute("Disc")]
		[Bindable(true)]
		public decimal? Disc 
		{
			get { return GetColumnValue<decimal?>(Columns.Disc); }
			set { SetColumnValue(Columns.Disc, value); }
		}
		  
		[XmlAttribute("Afterdisc")]
		[Bindable(true)]
		public decimal? Afterdisc 
		{
			get { return GetColumnValue<decimal?>(Columns.Afterdisc); }
			set { SetColumnValue(Columns.Afterdisc, value); }
		}
		  
		[XmlAttribute("Svccharge")]
		[Bindable(true)]
		public decimal? Svccharge 
		{
			get { return GetColumnValue<decimal?>(Columns.Svccharge); }
			set { SetColumnValue(Columns.Svccharge, value); }
		}
		  
		[XmlAttribute("Befgst")]
		[Bindable(true)]
		public decimal? Befgst 
		{
			get { return GetColumnValue<decimal?>(Columns.Befgst); }
			set { SetColumnValue(Columns.Befgst, value); }
		}
		  
		[XmlAttribute("Gst")]
		[Bindable(true)]
		public decimal? Gst 
		{
			get { return GetColumnValue<decimal?>(Columns.Gst); }
			set { SetColumnValue(Columns.Gst, value); }
		}
		  
		[XmlAttribute("Rounding")]
		[Bindable(true)]
		public decimal? Rounding 
		{
			get { return GetColumnValue<decimal?>(Columns.Rounding); }
			set { SetColumnValue(Columns.Rounding, value); }
		}
		  
		[XmlAttribute("Nettamount")]
		[Bindable(true)]
		public decimal? Nettamount 
		{
			get { return GetColumnValue<decimal?>(Columns.Nettamount); }
			set { SetColumnValue(Columns.Nettamount, value); }
		}
		  
		[XmlAttribute("PointSale")]
		[Bindable(true)]
		public decimal? PointSale 
		{
			get { return GetColumnValue<decimal?>(Columns.PointSale); }
			set { SetColumnValue(Columns.PointSale, value); }
		}
		  
		[XmlAttribute("InstallmentPaymentSale")]
		[Bindable(true)]
		public decimal? InstallmentPaymentSale 
		{
			get { return GetColumnValue<decimal?>(Columns.InstallmentPaymentSale); }
			set { SetColumnValue(Columns.InstallmentPaymentSale, value); }
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
		public static void Insert(DateTime varOrderdate,string varOutletname,int? varPax,int? varBill,decimal? varGrossamount,decimal? varDisc,decimal? varAfterdisc,decimal? varSvccharge,decimal? varBefgst,decimal? varGst,decimal? varRounding,decimal? varNettamount,decimal? varPointSale,decimal? varInstallmentPaymentSale,int? varRegenerate,DateTime? varLastUpdateOn)
		{
			DwHourlySale item = new DwHourlySale();
			
			item.Orderdate = varOrderdate;
			
			item.Outletname = varOutletname;
			
			item.Pax = varPax;
			
			item.Bill = varBill;
			
			item.Grossamount = varGrossamount;
			
			item.Disc = varDisc;
			
			item.Afterdisc = varAfterdisc;
			
			item.Svccharge = varSvccharge;
			
			item.Befgst = varBefgst;
			
			item.Gst = varGst;
			
			item.Rounding = varRounding;
			
			item.Nettamount = varNettamount;
			
			item.PointSale = varPointSale;
			
			item.InstallmentPaymentSale = varInstallmentPaymentSale;
			
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
		public static void Update(int varId,DateTime varOrderdate,string varOutletname,int? varPax,int? varBill,decimal? varGrossamount,decimal? varDisc,decimal? varAfterdisc,decimal? varSvccharge,decimal? varBefgst,decimal? varGst,decimal? varRounding,decimal? varNettamount,decimal? varPointSale,decimal? varInstallmentPaymentSale,int? varRegenerate,DateTime? varLastUpdateOn)
		{
			DwHourlySale item = new DwHourlySale();
			
				item.Id = varId;
			
				item.Orderdate = varOrderdate;
			
				item.Outletname = varOutletname;
			
				item.Pax = varPax;
			
				item.Bill = varBill;
			
				item.Grossamount = varGrossamount;
			
				item.Disc = varDisc;
			
				item.Afterdisc = varAfterdisc;
			
				item.Svccharge = varSvccharge;
			
				item.Befgst = varBefgst;
			
				item.Gst = varGst;
			
				item.Rounding = varRounding;
			
				item.Nettamount = varNettamount;
			
				item.PointSale = varPointSale;
			
				item.InstallmentPaymentSale = varInstallmentPaymentSale;
			
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
        
        
        
        public static TableSchema.TableColumn PaxColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BillColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn GrossamountColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DiscColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn AfterdiscColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn SvcchargeColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn BefgstColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn GstColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn RoundingColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn NettamountColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn PointSaleColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn InstallmentPaymentSaleColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn RegenerateColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn LastUpdateOnColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"ID";
			 public static string Orderdate = @"orderdate";
			 public static string Outletname = @"outletname";
			 public static string Pax = @"pax";
			 public static string Bill = @"bill";
			 public static string Grossamount = @"grossamount";
			 public static string Disc = @"disc";
			 public static string Afterdisc = @"afterdisc";
			 public static string Svccharge = @"svccharge";
			 public static string Befgst = @"befgst";
			 public static string Gst = @"gst";
			 public static string Rounding = @"rounding";
			 public static string Nettamount = @"nettamount";
			 public static string PointSale = @"pointSale";
			 public static string InstallmentPaymentSale = @"installmentPaymentSale";
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
