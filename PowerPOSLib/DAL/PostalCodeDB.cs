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
	/// Strongly-typed collection for the PostalCodeDB class.
	/// </summary>
    [Serializable]
	public partial class PostalCodeDBCollection : ActiveList<PostalCodeDB, PostalCodeDBCollection>
	{	   
		public PostalCodeDBCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PostalCodeDBCollection</returns>
		public PostalCodeDBCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PostalCodeDB o = this[i];
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
	/// This is an ActiveRecord class which wraps the PostalCodeDB table.
	/// </summary>
	[Serializable]
	public partial class PostalCodeDB : ActiveRecord<PostalCodeDB>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PostalCodeDB()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PostalCodeDB(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PostalCodeDB(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PostalCodeDB(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("PostalCodeDB", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarCountry = new TableSchema.TableColumn(schema);
				colvarCountry.ColumnName = "Country";
				colvarCountry.DataType = DbType.AnsiString;
				colvarCountry.MaxLength = 50;
				colvarCountry.AutoIncrement = false;
				colvarCountry.IsNullable = true;
				colvarCountry.IsPrimaryKey = false;
				colvarCountry.IsForeignKey = false;
				colvarCountry.IsReadOnly = false;
				colvarCountry.DefaultSetting = @"";
				colvarCountry.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCountry);
				
				TableSchema.TableColumn colvarLanguage = new TableSchema.TableColumn(schema);
				colvarLanguage.ColumnName = "Language";
				colvarLanguage.DataType = DbType.AnsiString;
				colvarLanguage.MaxLength = 50;
				colvarLanguage.AutoIncrement = false;
				colvarLanguage.IsNullable = true;
				colvarLanguage.IsPrimaryKey = false;
				colvarLanguage.IsForeignKey = false;
				colvarLanguage.IsReadOnly = false;
				colvarLanguage.DefaultSetting = @"";
				colvarLanguage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLanguage);
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "ID";
				colvarId.DataType = DbType.Int32;
				colvarId.MaxLength = 0;
				colvarId.AutoIncrement = false;
				colvarId.IsNullable = true;
				colvarId.IsPrimaryKey = false;
				colvarId.IsForeignKey = false;
				colvarId.IsReadOnly = false;
				colvarId.DefaultSetting = @"";
				colvarId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarId);
				
				TableSchema.TableColumn colvarRegion1 = new TableSchema.TableColumn(schema);
				colvarRegion1.ColumnName = "Region1";
				colvarRegion1.DataType = DbType.AnsiString;
				colvarRegion1.MaxLength = -1;
				colvarRegion1.AutoIncrement = false;
				colvarRegion1.IsNullable = true;
				colvarRegion1.IsPrimaryKey = false;
				colvarRegion1.IsForeignKey = false;
				colvarRegion1.IsReadOnly = false;
				colvarRegion1.DefaultSetting = @"";
				colvarRegion1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRegion1);
				
				TableSchema.TableColumn colvarRegion2 = new TableSchema.TableColumn(schema);
				colvarRegion2.ColumnName = "Region2";
				colvarRegion2.DataType = DbType.AnsiString;
				colvarRegion2.MaxLength = -1;
				colvarRegion2.AutoIncrement = false;
				colvarRegion2.IsNullable = true;
				colvarRegion2.IsPrimaryKey = false;
				colvarRegion2.IsForeignKey = false;
				colvarRegion2.IsReadOnly = false;
				colvarRegion2.DefaultSetting = @"";
				colvarRegion2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRegion2);
				
				TableSchema.TableColumn colvarRegion3 = new TableSchema.TableColumn(schema);
				colvarRegion3.ColumnName = "Region3";
				colvarRegion3.DataType = DbType.AnsiString;
				colvarRegion3.MaxLength = -1;
				colvarRegion3.AutoIncrement = false;
				colvarRegion3.IsNullable = true;
				colvarRegion3.IsPrimaryKey = false;
				colvarRegion3.IsForeignKey = false;
				colvarRegion3.IsReadOnly = false;
				colvarRegion3.DefaultSetting = @"";
				colvarRegion3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRegion3);
				
				TableSchema.TableColumn colvarRegion4 = new TableSchema.TableColumn(schema);
				colvarRegion4.ColumnName = "Region4";
				colvarRegion4.DataType = DbType.AnsiString;
				colvarRegion4.MaxLength = -1;
				colvarRegion4.AutoIncrement = false;
				colvarRegion4.IsNullable = true;
				colvarRegion4.IsPrimaryKey = false;
				colvarRegion4.IsForeignKey = false;
				colvarRegion4.IsReadOnly = false;
				colvarRegion4.DefaultSetting = @"";
				colvarRegion4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRegion4);
				
				TableSchema.TableColumn colvarZip = new TableSchema.TableColumn(schema);
				colvarZip.ColumnName = "ZIP";
				colvarZip.DataType = DbType.AnsiString;
				colvarZip.MaxLength = 50;
				colvarZip.AutoIncrement = false;
				colvarZip.IsNullable = false;
				colvarZip.IsPrimaryKey = true;
				colvarZip.IsForeignKey = false;
				colvarZip.IsReadOnly = false;
				colvarZip.DefaultSetting = @"";
				colvarZip.ForeignKeyTableName = "";
				schema.Columns.Add(colvarZip);
				
				TableSchema.TableColumn colvarCity = new TableSchema.TableColumn(schema);
				colvarCity.ColumnName = "City";
				colvarCity.DataType = DbType.AnsiString;
				colvarCity.MaxLength = -1;
				colvarCity.AutoIncrement = false;
				colvarCity.IsNullable = true;
				colvarCity.IsPrimaryKey = false;
				colvarCity.IsForeignKey = false;
				colvarCity.IsReadOnly = false;
				colvarCity.DefaultSetting = @"";
				colvarCity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCity);
				
				TableSchema.TableColumn colvarArea1 = new TableSchema.TableColumn(schema);
				colvarArea1.ColumnName = "Area1";
				colvarArea1.DataType = DbType.AnsiString;
				colvarArea1.MaxLength = -1;
				colvarArea1.AutoIncrement = false;
				colvarArea1.IsNullable = true;
				colvarArea1.IsPrimaryKey = false;
				colvarArea1.IsForeignKey = false;
				colvarArea1.IsReadOnly = false;
				colvarArea1.DefaultSetting = @"";
				colvarArea1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarArea1);
				
				TableSchema.TableColumn colvarArea2 = new TableSchema.TableColumn(schema);
				colvarArea2.ColumnName = "Area2";
				colvarArea2.DataType = DbType.AnsiString;
				colvarArea2.MaxLength = -1;
				colvarArea2.AutoIncrement = false;
				colvarArea2.IsNullable = true;
				colvarArea2.IsPrimaryKey = false;
				colvarArea2.IsForeignKey = false;
				colvarArea2.IsReadOnly = false;
				colvarArea2.DefaultSetting = @"";
				colvarArea2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarArea2);
				
				TableSchema.TableColumn colvarLat = new TableSchema.TableColumn(schema);
				colvarLat.ColumnName = "Lat";
				colvarLat.DataType = DbType.Decimal;
				colvarLat.MaxLength = 0;
				colvarLat.AutoIncrement = false;
				colvarLat.IsNullable = true;
				colvarLat.IsPrimaryKey = false;
				colvarLat.IsForeignKey = false;
				colvarLat.IsReadOnly = false;
				colvarLat.DefaultSetting = @"";
				colvarLat.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLat);
				
				TableSchema.TableColumn colvarLng = new TableSchema.TableColumn(schema);
				colvarLng.ColumnName = "Lng";
				colvarLng.DataType = DbType.Decimal;
				colvarLng.MaxLength = 0;
				colvarLng.AutoIncrement = false;
				colvarLng.IsNullable = true;
				colvarLng.IsPrimaryKey = false;
				colvarLng.IsForeignKey = false;
				colvarLng.IsReadOnly = false;
				colvarLng.DefaultSetting = @"";
				colvarLng.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLng);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("PostalCodeDB",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Country")]
		[Bindable(true)]
		public string Country 
		{
			get { return GetColumnValue<string>(Columns.Country); }
			set { SetColumnValue(Columns.Country, value); }
		}
		  
		[XmlAttribute("Language")]
		[Bindable(true)]
		public string Language 
		{
			get { return GetColumnValue<string>(Columns.Language); }
			set { SetColumnValue(Columns.Language, value); }
		}
		  
		[XmlAttribute("Id")]
		[Bindable(true)]
		public int? Id 
		{
			get { return GetColumnValue<int?>(Columns.Id); }
			set { SetColumnValue(Columns.Id, value); }
		}
		  
		[XmlAttribute("Region1")]
		[Bindable(true)]
		public string Region1 
		{
			get { return GetColumnValue<string>(Columns.Region1); }
			set { SetColumnValue(Columns.Region1, value); }
		}
		  
		[XmlAttribute("Region2")]
		[Bindable(true)]
		public string Region2 
		{
			get { return GetColumnValue<string>(Columns.Region2); }
			set { SetColumnValue(Columns.Region2, value); }
		}
		  
		[XmlAttribute("Region3")]
		[Bindable(true)]
		public string Region3 
		{
			get { return GetColumnValue<string>(Columns.Region3); }
			set { SetColumnValue(Columns.Region3, value); }
		}
		  
		[XmlAttribute("Region4")]
		[Bindable(true)]
		public string Region4 
		{
			get { return GetColumnValue<string>(Columns.Region4); }
			set { SetColumnValue(Columns.Region4, value); }
		}
		  
		[XmlAttribute("Zip")]
		[Bindable(true)]
		public string Zip 
		{
			get { return GetColumnValue<string>(Columns.Zip); }
			set { SetColumnValue(Columns.Zip, value); }
		}
		  
		[XmlAttribute("City")]
		[Bindable(true)]
		public string City 
		{
			get { return GetColumnValue<string>(Columns.City); }
			set { SetColumnValue(Columns.City, value); }
		}
		  
		[XmlAttribute("Area1")]
		[Bindable(true)]
		public string Area1 
		{
			get { return GetColumnValue<string>(Columns.Area1); }
			set { SetColumnValue(Columns.Area1, value); }
		}
		  
		[XmlAttribute("Area2")]
		[Bindable(true)]
		public string Area2 
		{
			get { return GetColumnValue<string>(Columns.Area2); }
			set { SetColumnValue(Columns.Area2, value); }
		}
		  
		[XmlAttribute("Lat")]
		[Bindable(true)]
		public decimal? Lat 
		{
			get { return GetColumnValue<decimal?>(Columns.Lat); }
			set { SetColumnValue(Columns.Lat, value); }
		}
		  
		[XmlAttribute("Lng")]
		[Bindable(true)]
		public decimal? Lng 
		{
			get { return GetColumnValue<decimal?>(Columns.Lng); }
			set { SetColumnValue(Columns.Lng, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varCountry,string varLanguage,int? varId,string varRegion1,string varRegion2,string varRegion3,string varRegion4,string varZip,string varCity,string varArea1,string varArea2,decimal? varLat,decimal? varLng)
		{
			PostalCodeDB item = new PostalCodeDB();
			
			item.Country = varCountry;
			
			item.Language = varLanguage;
			
			item.Id = varId;
			
			item.Region1 = varRegion1;
			
			item.Region2 = varRegion2;
			
			item.Region3 = varRegion3;
			
			item.Region4 = varRegion4;
			
			item.Zip = varZip;
			
			item.City = varCity;
			
			item.Area1 = varArea1;
			
			item.Area2 = varArea2;
			
			item.Lat = varLat;
			
			item.Lng = varLng;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varCountry,string varLanguage,int? varId,string varRegion1,string varRegion2,string varRegion3,string varRegion4,string varZip,string varCity,string varArea1,string varArea2,decimal? varLat,decimal? varLng)
		{
			PostalCodeDB item = new PostalCodeDB();
			
				item.Country = varCountry;
			
				item.Language = varLanguage;
			
				item.Id = varId;
			
				item.Region1 = varRegion1;
			
				item.Region2 = varRegion2;
			
				item.Region3 = varRegion3;
			
				item.Region4 = varRegion4;
			
				item.Zip = varZip;
			
				item.City = varCity;
			
				item.Area1 = varArea1;
			
				item.Area2 = varArea2;
			
				item.Lat = varLat;
			
				item.Lng = varLng;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn CountryColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn LanguageColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn Region1Column
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn Region2Column
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn Region3Column
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn Region4Column
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ZipColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CityColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn Area1Column
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn Area2Column
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn LatColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn LngColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Country = @"Country";
			 public static string Language = @"Language";
			 public static string Id = @"ID";
			 public static string Region1 = @"Region1";
			 public static string Region2 = @"Region2";
			 public static string Region3 = @"Region3";
			 public static string Region4 = @"Region4";
			 public static string Zip = @"ZIP";
			 public static string City = @"City";
			 public static string Area1 = @"Area1";
			 public static string Area2 = @"Area2";
			 public static string Lat = @"Lat";
			 public static string Lng = @"Lng";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
