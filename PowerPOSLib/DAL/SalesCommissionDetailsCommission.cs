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
	/// Strongly-typed collection for the SalesCommissionDetailsCommission class.
	/// </summary>
    [Serializable]
	public partial class SalesCommissionDetailsCommissionCollection : ActiveList<SalesCommissionDetailsCommission, SalesCommissionDetailsCommissionCollection>
	{	   
		public SalesCommissionDetailsCommissionCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SalesCommissionDetailsCommissionCollection</returns>
		public SalesCommissionDetailsCommissionCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SalesCommissionDetailsCommission o = this[i];
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
	/// This is an ActiveRecord class which wraps the SalesCommissionDetails_Commission table.
	/// </summary>
	[Serializable]
	public partial class SalesCommissionDetailsCommission : ActiveRecord<SalesCommissionDetailsCommission>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SalesCommissionDetailsCommission()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SalesCommissionDetailsCommission(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SalesCommissionDetailsCommission(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SalesCommissionDetailsCommission(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SalesCommissionDetails_Commission", TableType.Table, DataService.GetInstance("PowerPOS"));
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
				
				TableSchema.TableColumn colvarMonth = new TableSchema.TableColumn(schema);
				colvarMonth.ColumnName = "Month";
				colvarMonth.DataType = DbType.String;
				colvarMonth.MaxLength = 50;
				colvarMonth.AutoIncrement = false;
				colvarMonth.IsNullable = false;
				colvarMonth.IsPrimaryKey = false;
				colvarMonth.IsForeignKey = false;
				colvarMonth.IsReadOnly = false;
				colvarMonth.DefaultSetting = @"";
				colvarMonth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMonth);
				
				TableSchema.TableColumn colvarStaff = new TableSchema.TableColumn(schema);
				colvarStaff.ColumnName = "Staff";
				colvarStaff.DataType = DbType.String;
				colvarStaff.MaxLength = 50;
				colvarStaff.AutoIncrement = false;
				colvarStaff.IsNullable = false;
				colvarStaff.IsPrimaryKey = false;
				colvarStaff.IsForeignKey = false;
				colvarStaff.IsReadOnly = false;
				colvarStaff.DefaultSetting = @"";
				colvarStaff.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStaff);
				
				TableSchema.TableColumn colvarScheme = new TableSchema.TableColumn(schema);
				colvarScheme.ColumnName = "Scheme";
				colvarScheme.DataType = DbType.String;
				colvarScheme.MaxLength = 50;
				colvarScheme.AutoIncrement = false;
				colvarScheme.IsNullable = false;
				colvarScheme.IsPrimaryKey = false;
				colvarScheme.IsForeignKey = false;
				colvarScheme.IsReadOnly = false;
				colvarScheme.DefaultSetting = @"";
				colvarScheme.ForeignKeyTableName = "";
				schema.Columns.Add(colvarScheme);
				
				TableSchema.TableColumn colvarCommissionText = new TableSchema.TableColumn(schema);
				colvarCommissionText.ColumnName = "CommissionText";
				colvarCommissionText.DataType = DbType.String;
				colvarCommissionText.MaxLength = 200;
				colvarCommissionText.AutoIncrement = false;
				colvarCommissionText.IsNullable = true;
				colvarCommissionText.IsPrimaryKey = false;
				colvarCommissionText.IsForeignKey = false;
				colvarCommissionText.IsReadOnly = false;
				colvarCommissionText.DefaultSetting = @"";
				colvarCommissionText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCommissionText);
				
				TableSchema.TableColumn colvarCommissionValue = new TableSchema.TableColumn(schema);
				colvarCommissionValue.ColumnName = "CommissionValue";
				colvarCommissionValue.DataType = DbType.Currency;
				colvarCommissionValue.MaxLength = 0;
				colvarCommissionValue.AutoIncrement = false;
				colvarCommissionValue.IsNullable = true;
				colvarCommissionValue.IsPrimaryKey = false;
				colvarCommissionValue.IsForeignKey = false;
				colvarCommissionValue.IsReadOnly = false;
				colvarCommissionValue.DefaultSetting = @"";
				colvarCommissionValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCommissionValue);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("SalesCommissionDetails_Commission",schema);
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
		  
		[XmlAttribute("Month")]
		[Bindable(true)]
		public string Month 
		{
			get { return GetColumnValue<string>(Columns.Month); }
			set { SetColumnValue(Columns.Month, value); }
		}
		  
		[XmlAttribute("Staff")]
		[Bindable(true)]
		public string Staff 
		{
			get { return GetColumnValue<string>(Columns.Staff); }
			set { SetColumnValue(Columns.Staff, value); }
		}
		  
		[XmlAttribute("Scheme")]
		[Bindable(true)]
		public string Scheme 
		{
			get { return GetColumnValue<string>(Columns.Scheme); }
			set { SetColumnValue(Columns.Scheme, value); }
		}
		  
		[XmlAttribute("CommissionText")]
		[Bindable(true)]
		public string CommissionText 
		{
			get { return GetColumnValue<string>(Columns.CommissionText); }
			set { SetColumnValue(Columns.CommissionText, value); }
		}
		  
		[XmlAttribute("CommissionValue")]
		[Bindable(true)]
		public decimal? CommissionValue 
		{
			get { return GetColumnValue<decimal?>(Columns.CommissionValue); }
			set { SetColumnValue(Columns.CommissionValue, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varMonth,string varStaff,string varScheme,string varCommissionText,decimal? varCommissionValue)
		{
			SalesCommissionDetailsCommission item = new SalesCommissionDetailsCommission();
			
			item.Month = varMonth;
			
			item.Staff = varStaff;
			
			item.Scheme = varScheme;
			
			item.CommissionText = varCommissionText;
			
			item.CommissionValue = varCommissionValue;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varMonth,string varStaff,string varScheme,string varCommissionText,decimal? varCommissionValue)
		{
			SalesCommissionDetailsCommission item = new SalesCommissionDetailsCommission();
			
				item.Id = varId;
			
				item.Month = varMonth;
			
				item.Staff = varStaff;
			
				item.Scheme = varScheme;
			
				item.CommissionText = varCommissionText;
			
				item.CommissionValue = varCommissionValue;
			
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
        
        
        
        public static TableSchema.TableColumn MonthColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn StaffColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn SchemeColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CommissionTextColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CommissionValueColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"ID";
			 public static string Month = @"Month";
			 public static string Staff = @"Staff";
			 public static string Scheme = @"Scheme";
			 public static string CommissionText = @"CommissionText";
			 public static string CommissionValue = @"CommissionValue";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
