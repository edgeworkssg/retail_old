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
	/// Strongly-typed collection for the TestTable2 class.
	/// </summary>
    [Serializable]
	public partial class TestTable2Collection : ActiveList<TestTable2, TestTable2Collection>
	{	   
		public TestTable2Collection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TestTable2Collection</returns>
		public TestTable2Collection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TestTable2 o = this[i];
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
	/// This is an ActiveRecord class which wraps the TestTable2 table.
	/// </summary>
	[Serializable]
	public partial class TestTable2 : ActiveRecord<TestTable2>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TestTable2()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TestTable2(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TestTable2(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TestTable2(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("TestTable2", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarColumn4 = new TableSchema.TableColumn(schema);
				colvarColumn4.ColumnName = "Column4";
				colvarColumn4.DataType = DbType.String;
				colvarColumn4.MaxLength = 10;
				colvarColumn4.AutoIncrement = false;
				colvarColumn4.IsNullable = false;
				colvarColumn4.IsPrimaryKey = true;
				colvarColumn4.IsForeignKey = false;
				colvarColumn4.IsReadOnly = false;
				colvarColumn4.DefaultSetting = @"";
				colvarColumn4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarColumn4);
				
				TableSchema.TableColumn colvarColumn1 = new TableSchema.TableColumn(schema);
				colvarColumn1.ColumnName = "Column1";
				colvarColumn1.DataType = DbType.String;
				colvarColumn1.MaxLength = 10;
				colvarColumn1.AutoIncrement = false;
				colvarColumn1.IsNullable = true;
				colvarColumn1.IsPrimaryKey = false;
				colvarColumn1.IsForeignKey = true;
				colvarColumn1.IsReadOnly = false;
				colvarColumn1.DefaultSetting = @"";
				
					colvarColumn1.ForeignKeyTableName = "TestTable";
				schema.Columns.Add(colvarColumn1);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("TestTable2",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Column4")]
		[Bindable(true)]
		public string Column4 
		{
			get { return GetColumnValue<string>(Columns.Column4); }
			set { SetColumnValue(Columns.Column4, value); }
		}
		  
		[XmlAttribute("Column1")]
		[Bindable(true)]
		public string Column1 
		{
			get { return GetColumnValue<string>(Columns.Column1); }
			set { SetColumnValue(Columns.Column1, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a TestTable ActiveRecord object related to this TestTable2
		/// 
		/// </summary>
		public PowerPOS.TestTable TestTable
		{
			get { return PowerPOS.TestTable.FetchByID(this.Column1); }
			set { SetColumnValue("Column1", value.Column1); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varColumn4,string varColumn1)
		{
			TestTable2 item = new TestTable2();
			
			item.Column4 = varColumn4;
			
			item.Column1 = varColumn1;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varColumn4,string varColumn1)
		{
			TestTable2 item = new TestTable2();
			
				item.Column4 = varColumn4;
			
				item.Column1 = varColumn1;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn Column4Column
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn Column1Column
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Column4 = @"Column4";
			 public static string Column1 = @"Column1";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
