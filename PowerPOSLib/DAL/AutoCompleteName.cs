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
	/// Strongly-typed collection for the AutoCompleteName class.
	/// </summary>
    [Serializable]
	public partial class AutoCompleteNameCollection : ActiveList<AutoCompleteName, AutoCompleteNameCollection>
	{	   
		public AutoCompleteNameCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AutoCompleteNameCollection</returns>
		public AutoCompleteNameCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AutoCompleteName o = this[i];
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
	/// This is an ActiveRecord class which wraps the AutoCompleteNames table.
	/// </summary>
	[Serializable]
	public partial class AutoCompleteName : ActiveRecord<AutoCompleteName>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AutoCompleteName()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AutoCompleteName(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AutoCompleteName(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AutoCompleteName(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("AutoCompleteNames", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarNames = new TableSchema.TableColumn(schema);
				colvarNames.ColumnName = "Names";
				colvarNames.DataType = DbType.AnsiString;
				colvarNames.MaxLength = 200;
				colvarNames.AutoIncrement = false;
				colvarNames.IsNullable = false;
				colvarNames.IsPrimaryKey = true;
				colvarNames.IsForeignKey = false;
				colvarNames.IsReadOnly = false;
				colvarNames.DefaultSetting = @"";
				colvarNames.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNames);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("AutoCompleteNames",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Names")]
		[Bindable(true)]
		public string Names 
		{
			get { return GetColumnValue<string>(Columns.Names); }
			set { SetColumnValue(Columns.Names, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varNames)
		{
			AutoCompleteName item = new AutoCompleteName();
			
			item.Names = varNames;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varNames)
		{
			AutoCompleteName item = new AutoCompleteName();
			
				item.Names = varNames;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn NamesColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Names = @"Names";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
