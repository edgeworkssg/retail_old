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
	/// Strongly-typed collection for the SalesCommissionDetailsDeduction class.
	/// </summary>
    [Serializable]
	public partial class SalesCommissionDetailsDeductionCollection : ActiveList<SalesCommissionDetailsDeduction, SalesCommissionDetailsDeductionCollection>
	{	   
		public SalesCommissionDetailsDeductionCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SalesCommissionDetailsDeductionCollection</returns>
		public SalesCommissionDetailsDeductionCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SalesCommissionDetailsDeduction o = this[i];
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
	/// This is an ActiveRecord class which wraps the SalesCommissionDetails_Deduction table.
	/// </summary>
	[Serializable]
	public partial class SalesCommissionDetailsDeduction : ActiveRecord<SalesCommissionDetailsDeduction>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SalesCommissionDetailsDeduction()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SalesCommissionDetailsDeduction(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SalesCommissionDetailsDeduction(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SalesCommissionDetailsDeduction(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SalesCommissionDetails_Deduction", TableType.Table, DataService.GetInstance("PowerPOS"));
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
				colvarMonth.IsNullable = true;
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
				colvarStaff.IsNullable = true;
				colvarStaff.IsPrimaryKey = false;
				colvarStaff.IsForeignKey = false;
				colvarStaff.IsReadOnly = false;
				colvarStaff.DefaultSetting = @"";
				colvarStaff.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStaff);
				
				TableSchema.TableColumn colvarDeduction = new TableSchema.TableColumn(schema);
				colvarDeduction.ColumnName = "Deduction";
				colvarDeduction.DataType = DbType.Currency;
				colvarDeduction.MaxLength = 0;
				colvarDeduction.AutoIncrement = false;
				colvarDeduction.IsNullable = true;
				colvarDeduction.IsPrimaryKey = false;
				colvarDeduction.IsForeignKey = false;
				colvarDeduction.IsReadOnly = false;
				colvarDeduction.DefaultSetting = @"";
				colvarDeduction.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeduction);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("SalesCommissionDetails_Deduction",schema);
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
		  
		[XmlAttribute("Deduction")]
		[Bindable(true)]
		public decimal? Deduction 
		{
			get { return GetColumnValue<decimal?>(Columns.Deduction); }
			set { SetColumnValue(Columns.Deduction, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varMonth,string varStaff,decimal? varDeduction)
		{
			SalesCommissionDetailsDeduction item = new SalesCommissionDetailsDeduction();
			
			item.Month = varMonth;
			
			item.Staff = varStaff;
			
			item.Deduction = varDeduction;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varMonth,string varStaff,decimal? varDeduction)
		{
			SalesCommissionDetailsDeduction item = new SalesCommissionDetailsDeduction();
			
				item.Id = varId;
			
				item.Month = varMonth;
			
				item.Staff = varStaff;
			
				item.Deduction = varDeduction;
			
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
        
        
        
        public static TableSchema.TableColumn DeductionColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"ID";
			 public static string Month = @"Month";
			 public static string Staff = @"Staff";
			 public static string Deduction = @"Deduction";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
