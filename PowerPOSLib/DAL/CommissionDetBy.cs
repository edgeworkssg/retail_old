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
	/// Strongly-typed collection for the CommissionDetBy class.
	/// </summary>
    [Serializable]
	public partial class CommissionDetByCollection : ActiveList<CommissionDetBy, CommissionDetByCollection>
	{	   
		public CommissionDetByCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>CommissionDetByCollection</returns>
		public CommissionDetByCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                CommissionDetBy o = this[i];
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
	/// This is an ActiveRecord class which wraps the CommissionDetBy table.
	/// </summary>
	[Serializable]
	public partial class CommissionDetBy : ActiveRecord<CommissionDetBy>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public CommissionDetBy()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public CommissionDetBy(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public CommissionDetBy(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public CommissionDetBy(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("CommissionDetBy", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarCommissionDetByID = new TableSchema.TableColumn(schema);
				colvarCommissionDetByID.ColumnName = "CommissionDetByID";
				colvarCommissionDetByID.DataType = DbType.Int32;
				colvarCommissionDetByID.MaxLength = 0;
				colvarCommissionDetByID.AutoIncrement = true;
				colvarCommissionDetByID.IsNullable = false;
				colvarCommissionDetByID.IsPrimaryKey = true;
				colvarCommissionDetByID.IsForeignKey = false;
				colvarCommissionDetByID.IsReadOnly = false;
				colvarCommissionDetByID.DefaultSetting = @"";
				colvarCommissionDetByID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCommissionDetByID);
				
				TableSchema.TableColumn colvarCommissionHdrID = new TableSchema.TableColumn(schema);
				colvarCommissionHdrID.ColumnName = "CommissionHdrID";
				colvarCommissionHdrID.DataType = DbType.Int32;
				colvarCommissionHdrID.MaxLength = 0;
				colvarCommissionHdrID.AutoIncrement = false;
				colvarCommissionHdrID.IsNullable = true;
				colvarCommissionHdrID.IsPrimaryKey = false;
				colvarCommissionHdrID.IsForeignKey = false;
				colvarCommissionHdrID.IsReadOnly = false;
				colvarCommissionHdrID.DefaultSetting = @"";
				colvarCommissionHdrID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCommissionHdrID);
				
				TableSchema.TableColumn colvarFrom = new TableSchema.TableColumn(schema);
				colvarFrom.ColumnName = "From";
				colvarFrom.DataType = DbType.Decimal;
				colvarFrom.MaxLength = 0;
				colvarFrom.AutoIncrement = false;
				colvarFrom.IsNullable = true;
				colvarFrom.IsPrimaryKey = false;
				colvarFrom.IsForeignKey = false;
				colvarFrom.IsReadOnly = false;
				colvarFrom.DefaultSetting = @"";
				colvarFrom.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFrom);
				
				TableSchema.TableColumn colvarToX = new TableSchema.TableColumn(schema);
				colvarToX.ColumnName = "To";
				colvarToX.DataType = DbType.Decimal;
				colvarToX.MaxLength = 0;
				colvarToX.AutoIncrement = false;
				colvarToX.IsNullable = true;
				colvarToX.IsPrimaryKey = false;
				colvarToX.IsForeignKey = false;
				colvarToX.IsReadOnly = false;
				colvarToX.DefaultSetting = @"";
				colvarToX.ForeignKeyTableName = "";
				schema.Columns.Add(colvarToX);
				
				TableSchema.TableColumn colvarValueX = new TableSchema.TableColumn(schema);
				colvarValueX.ColumnName = "Value";
				colvarValueX.DataType = DbType.Decimal;
				colvarValueX.MaxLength = 0;
				colvarValueX.AutoIncrement = false;
				colvarValueX.IsNullable = true;
				colvarValueX.IsPrimaryKey = false;
				colvarValueX.IsForeignKey = false;
				colvarValueX.IsReadOnly = false;
				colvarValueX.DefaultSetting = @"";
				colvarValueX.ForeignKeyTableName = "";
				schema.Columns.Add(colvarValueX);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("CommissionDetBy",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("CommissionDetByID")]
		[Bindable(true)]
		public int CommissionDetByID 
		{
			get { return GetColumnValue<int>(Columns.CommissionDetByID); }
			set { SetColumnValue(Columns.CommissionDetByID, value); }
		}
		  
		[XmlAttribute("CommissionHdrID")]
		[Bindable(true)]
		public int? CommissionHdrID 
		{
			get { return GetColumnValue<int?>(Columns.CommissionHdrID); }
			set { SetColumnValue(Columns.CommissionHdrID, value); }
		}
		  
		[XmlAttribute("From")]
		[Bindable(true)]
		public decimal? From 
		{
			get { return GetColumnValue<decimal?>(Columns.From); }
			set { SetColumnValue(Columns.From, value); }
		}
		  
		[XmlAttribute("ToX")]
		[Bindable(true)]
		public decimal? ToX 
		{
			get { return GetColumnValue<decimal?>(Columns.ToX); }
			set { SetColumnValue(Columns.ToX, value); }
		}
		  
		[XmlAttribute("ValueX")]
		[Bindable(true)]
		public decimal? ValueX 
		{
			get { return GetColumnValue<decimal?>(Columns.ValueX); }
			set { SetColumnValue(Columns.ValueX, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int? varCommissionHdrID,decimal? varFrom,decimal? varToX,decimal? varValueX)
		{
			CommissionDetBy item = new CommissionDetBy();
			
			item.CommissionHdrID = varCommissionHdrID;
			
			item.From = varFrom;
			
			item.ToX = varToX;
			
			item.ValueX = varValueX;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varCommissionDetByID,int? varCommissionHdrID,decimal? varFrom,decimal? varToX,decimal? varValueX)
		{
			CommissionDetBy item = new CommissionDetBy();
			
				item.CommissionDetByID = varCommissionDetByID;
			
				item.CommissionHdrID = varCommissionHdrID;
			
				item.From = varFrom;
			
				item.ToX = varToX;
			
				item.ValueX = varValueX;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn CommissionDetByIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn CommissionHdrIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn FromColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ToXColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ValueXColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string CommissionDetByID = @"CommissionDetByID";
			 public static string CommissionHdrID = @"CommissionHdrID";
			 public static string From = @"From";
			 public static string ToX = @"To";
			 public static string ValueX = @"Value";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
