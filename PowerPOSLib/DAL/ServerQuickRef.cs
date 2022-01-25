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
	/// Strongly-typed collection for the ServerQuickRef class.
	/// </summary>
    [Serializable]
	public partial class ServerQuickRefCollection : ActiveList<ServerQuickRef, ServerQuickRefCollection>
	{	   
		public ServerQuickRefCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ServerQuickRefCollection</returns>
		public ServerQuickRefCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ServerQuickRef o = this[i];
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
	/// This is an ActiveRecord class which wraps the ServerQuickRef table.
	/// </summary>
	[Serializable]
	public partial class ServerQuickRef : ActiveRecord<ServerQuickRef>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ServerQuickRef()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ServerQuickRef(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ServerQuickRef(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ServerQuickRef(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ServerQuickRef", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarRefID = new TableSchema.TableColumn(schema);
				colvarRefID.ColumnName = "RefID";
				colvarRefID.DataType = DbType.Int32;
				colvarRefID.MaxLength = 0;
				colvarRefID.AutoIncrement = true;
				colvarRefID.IsNullable = false;
				colvarRefID.IsPrimaryKey = true;
				colvarRefID.IsForeignKey = false;
				colvarRefID.IsReadOnly = false;
				colvarRefID.DefaultSetting = @"";
				colvarRefID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRefID);
				
				TableSchema.TableColumn colvarTableName = new TableSchema.TableColumn(schema);
				colvarTableName.ColumnName = "TableName";
				colvarTableName.DataType = DbType.AnsiString;
				colvarTableName.MaxLength = 50;
				colvarTableName.AutoIncrement = false;
				colvarTableName.IsNullable = false;
				colvarTableName.IsPrimaryKey = false;
				colvarTableName.IsForeignKey = false;
				colvarTableName.IsReadOnly = false;
				colvarTableName.DefaultSetting = @"";
				colvarTableName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTableName);
				
				TableSchema.TableColumn colvarLastModifiedon = new TableSchema.TableColumn(schema);
				colvarLastModifiedon.ColumnName = "LastModifiedon";
				colvarLastModifiedon.DataType = DbType.DateTime;
				colvarLastModifiedon.MaxLength = 0;
				colvarLastModifiedon.AutoIncrement = false;
				colvarLastModifiedon.IsNullable = false;
				colvarLastModifiedon.IsPrimaryKey = false;
				colvarLastModifiedon.IsForeignKey = false;
				colvarLastModifiedon.IsReadOnly = false;
				colvarLastModifiedon.DefaultSetting = @"";
				colvarLastModifiedon.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastModifiedon);
				
				TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
				colvarPointOfSaleID.ColumnName = "PointOfSaleID";
				colvarPointOfSaleID.DataType = DbType.Int32;
				colvarPointOfSaleID.MaxLength = 0;
				colvarPointOfSaleID.AutoIncrement = false;
				colvarPointOfSaleID.IsNullable = true;
				colvarPointOfSaleID.IsPrimaryKey = false;
				colvarPointOfSaleID.IsForeignKey = false;
				colvarPointOfSaleID.IsReadOnly = false;
				colvarPointOfSaleID.DefaultSetting = @"";
				colvarPointOfSaleID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointOfSaleID);
				
				TableSchema.TableColumn colvarOutlet = new TableSchema.TableColumn(schema);
				colvarOutlet.ColumnName = "Outlet";
				colvarOutlet.DataType = DbType.AnsiString;
				colvarOutlet.MaxLength = 50;
				colvarOutlet.AutoIncrement = false;
				colvarOutlet.IsNullable = true;
				colvarOutlet.IsPrimaryKey = false;
				colvarOutlet.IsForeignKey = false;
				colvarOutlet.IsReadOnly = false;
				colvarOutlet.DefaultSetting = @"";
				colvarOutlet.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOutlet);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("ServerQuickRef",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("RefID")]
		[Bindable(true)]
		public int RefID 
		{
			get { return GetColumnValue<int>(Columns.RefID); }
			set { SetColumnValue(Columns.RefID, value); }
		}
		  
		[XmlAttribute("TableName")]
		[Bindable(true)]
		public string TableName 
		{
			get { return GetColumnValue<string>(Columns.TableName); }
			set { SetColumnValue(Columns.TableName, value); }
		}
		  
		[XmlAttribute("LastModifiedon")]
		[Bindable(true)]
		public DateTime LastModifiedon 
		{
			get { return GetColumnValue<DateTime>(Columns.LastModifiedon); }
			set { SetColumnValue(Columns.LastModifiedon, value); }
		}
		  
		[XmlAttribute("PointOfSaleID")]
		[Bindable(true)]
		public int? PointOfSaleID 
		{
			get { return GetColumnValue<int?>(Columns.PointOfSaleID); }
			set { SetColumnValue(Columns.PointOfSaleID, value); }
		}
		  
		[XmlAttribute("Outlet")]
		[Bindable(true)]
		public string Outlet 
		{
			get { return GetColumnValue<string>(Columns.Outlet); }
			set { SetColumnValue(Columns.Outlet, value); }
		}
		  
		[XmlAttribute("InventoryLocationID")]
		[Bindable(true)]
		public int? InventoryLocationID 
		{
			get { return GetColumnValue<int?>(Columns.InventoryLocationID); }
			set { SetColumnValue(Columns.InventoryLocationID, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varTableName,DateTime varLastModifiedon,int? varPointOfSaleID,string varOutlet,int? varInventoryLocationID)
		{
			ServerQuickRef item = new ServerQuickRef();
			
			item.TableName = varTableName;
			
			item.LastModifiedon = varLastModifiedon;
			
			item.PointOfSaleID = varPointOfSaleID;
			
			item.Outlet = varOutlet;
			
			item.InventoryLocationID = varInventoryLocationID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varRefID,string varTableName,DateTime varLastModifiedon,int? varPointOfSaleID,string varOutlet,int? varInventoryLocationID)
		{
			ServerQuickRef item = new ServerQuickRef();
			
				item.RefID = varRefID;
			
				item.TableName = varTableName;
			
				item.LastModifiedon = varLastModifiedon;
			
				item.PointOfSaleID = varPointOfSaleID;
			
				item.Outlet = varOutlet;
			
				item.InventoryLocationID = varInventoryLocationID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn RefIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn TableNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn LastModifiedonColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn PointOfSaleIDColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn OutletColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn InventoryLocationIDColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string RefID = @"RefID";
			 public static string TableName = @"TableName";
			 public static string LastModifiedon = @"LastModifiedon";
			 public static string PointOfSaleID = @"PointOfSaleID";
			 public static string Outlet = @"Outlet";
			 public static string InventoryLocationID = @"InventoryLocationID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
