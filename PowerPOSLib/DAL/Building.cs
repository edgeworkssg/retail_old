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
	/// Strongly-typed collection for the Building class.
	/// </summary>
    [Serializable]
	public partial class BuildingCollection : ActiveList<Building, BuildingCollection>
	{	   
		public BuildingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>BuildingCollection</returns>
		public BuildingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Building o = this[i];
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
	/// This is an ActiveRecord class which wraps the Building table.
	/// </summary>
	[Serializable]
	public partial class Building : ActiveRecord<Building>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Building()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Building(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Building(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Building(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Building", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarBuildingName = new TableSchema.TableColumn(schema);
				colvarBuildingName.ColumnName = "Building_Name";
				colvarBuildingName.DataType = DbType.AnsiString;
				colvarBuildingName.MaxLength = 50;
				colvarBuildingName.AutoIncrement = false;
				colvarBuildingName.IsNullable = false;
				colvarBuildingName.IsPrimaryKey = true;
				colvarBuildingName.IsForeignKey = false;
				colvarBuildingName.IsReadOnly = false;
				colvarBuildingName.DefaultSetting = @"";
				colvarBuildingName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBuildingName);
				
				TableSchema.TableColumn colvarCity = new TableSchema.TableColumn(schema);
				colvarCity.ColumnName = "City";
				colvarCity.DataType = DbType.AnsiString;
				colvarCity.MaxLength = 50;
				colvarCity.AutoIncrement = false;
				colvarCity.IsNullable = true;
				colvarCity.IsPrimaryKey = false;
				colvarCity.IsForeignKey = false;
				colvarCity.IsReadOnly = false;
				colvarCity.DefaultSetting = @"";
				colvarCity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCity);
				
				TableSchema.TableColumn colvarCountry = new TableSchema.TableColumn(schema);
				colvarCountry.ColumnName = "Country";
				colvarCountry.DataType = DbType.AnsiString;
				colvarCountry.MaxLength = 15;
				colvarCountry.AutoIncrement = false;
				colvarCountry.IsNullable = true;
				colvarCountry.IsPrimaryKey = false;
				colvarCountry.IsForeignKey = false;
				colvarCountry.IsReadOnly = false;
				colvarCountry.DefaultSetting = @"";
				colvarCountry.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCountry);
				
				TableSchema.TableColumn colvarAddressLine1 = new TableSchema.TableColumn(schema);
				colvarAddressLine1.ColumnName = "Address_Line_1";
				colvarAddressLine1.DataType = DbType.AnsiString;
				colvarAddressLine1.MaxLength = 150;
				colvarAddressLine1.AutoIncrement = false;
				colvarAddressLine1.IsNullable = true;
				colvarAddressLine1.IsPrimaryKey = false;
				colvarAddressLine1.IsForeignKey = false;
				colvarAddressLine1.IsReadOnly = false;
				colvarAddressLine1.DefaultSetting = @"";
				colvarAddressLine1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAddressLine1);
				
				TableSchema.TableColumn colvarAddressLine2 = new TableSchema.TableColumn(schema);
				colvarAddressLine2.ColumnName = "Address_Line_2";
				colvarAddressLine2.DataType = DbType.AnsiString;
				colvarAddressLine2.MaxLength = 150;
				colvarAddressLine2.AutoIncrement = false;
				colvarAddressLine2.IsNullable = true;
				colvarAddressLine2.IsPrimaryKey = false;
				colvarAddressLine2.IsForeignKey = false;
				colvarAddressLine2.IsReadOnly = false;
				colvarAddressLine2.DefaultSetting = @"";
				colvarAddressLine2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAddressLine2);
				
				TableSchema.TableColumn colvarPinCode = new TableSchema.TableColumn(schema);
				colvarPinCode.ColumnName = "PinCode";
				colvarPinCode.DataType = DbType.Int32;
				colvarPinCode.MaxLength = 0;
				colvarPinCode.AutoIncrement = false;
				colvarPinCode.IsNullable = true;
				colvarPinCode.IsPrimaryKey = false;
				colvarPinCode.IsForeignKey = false;
				colvarPinCode.IsReadOnly = false;
				colvarPinCode.DefaultSetting = @"";
				colvarPinCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPinCode);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Building",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("BuildingName")]
		[Bindable(true)]
		public string BuildingName 
		{
			get { return GetColumnValue<string>(Columns.BuildingName); }
			set { SetColumnValue(Columns.BuildingName, value); }
		}
		  
		[XmlAttribute("City")]
		[Bindable(true)]
		public string City 
		{
			get { return GetColumnValue<string>(Columns.City); }
			set { SetColumnValue(Columns.City, value); }
		}
		  
		[XmlAttribute("Country")]
		[Bindable(true)]
		public string Country 
		{
			get { return GetColumnValue<string>(Columns.Country); }
			set { SetColumnValue(Columns.Country, value); }
		}
		  
		[XmlAttribute("AddressLine1")]
		[Bindable(true)]
		public string AddressLine1 
		{
			get { return GetColumnValue<string>(Columns.AddressLine1); }
			set { SetColumnValue(Columns.AddressLine1, value); }
		}
		  
		[XmlAttribute("AddressLine2")]
		[Bindable(true)]
		public string AddressLine2 
		{
			get { return GetColumnValue<string>(Columns.AddressLine2); }
			set { SetColumnValue(Columns.AddressLine2, value); }
		}
		  
		[XmlAttribute("PinCode")]
		[Bindable(true)]
		public int? PinCode 
		{
			get { return GetColumnValue<int?>(Columns.PinCode); }
			set { SetColumnValue(Columns.PinCode, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varBuildingName,string varCity,string varCountry,string varAddressLine1,string varAddressLine2,int? varPinCode)
		{
			Building item = new Building();
			
			item.BuildingName = varBuildingName;
			
			item.City = varCity;
			
			item.Country = varCountry;
			
			item.AddressLine1 = varAddressLine1;
			
			item.AddressLine2 = varAddressLine2;
			
			item.PinCode = varPinCode;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varBuildingName,string varCity,string varCountry,string varAddressLine1,string varAddressLine2,int? varPinCode)
		{
			Building item = new Building();
			
				item.BuildingName = varBuildingName;
			
				item.City = varCity;
			
				item.Country = varCountry;
			
				item.AddressLine1 = varAddressLine1;
			
				item.AddressLine2 = varAddressLine2;
			
				item.PinCode = varPinCode;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn BuildingNameColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn CityColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn CountryColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn AddressLine1Column
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn AddressLine2Column
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn PinCodeColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string BuildingName = @"Building_Name";
			 public static string City = @"City";
			 public static string Country = @"Country";
			 public static string AddressLine1 = @"Address_Line_1";
			 public static string AddressLine2 = @"Address_Line_2";
			 public static string PinCode = @"PinCode";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
