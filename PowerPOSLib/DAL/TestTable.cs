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
	/// Strongly-typed collection for the TestTable class.
	/// </summary>
    [Serializable]
	public partial class TestTableCollection : ActiveList<TestTable, TestTableCollection>
	{	   
		public TestTableCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TestTableCollection</returns>
		public TestTableCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TestTable o = this[i];
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
	/// This is an ActiveRecord class which wraps the TestTable table.
	/// </summary>
	[Serializable]
	public partial class TestTable : ActiveRecord<TestTable>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TestTable()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TestTable(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TestTable(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TestTable(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("TestTable", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarColumn1 = new TableSchema.TableColumn(schema);
				colvarColumn1.ColumnName = "Column1";
				colvarColumn1.DataType = DbType.String;
				colvarColumn1.MaxLength = 10;
				colvarColumn1.AutoIncrement = false;
				colvarColumn1.IsNullable = false;
				colvarColumn1.IsPrimaryKey = true;
				colvarColumn1.IsForeignKey = false;
				colvarColumn1.IsReadOnly = false;
				colvarColumn1.DefaultSetting = @"";
				colvarColumn1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarColumn1);
				
				TableSchema.TableColumn colvarColumn2 = new TableSchema.TableColumn(schema);
				colvarColumn2.ColumnName = "Column2";
				colvarColumn2.DataType = DbType.String;
				colvarColumn2.MaxLength = 10;
				colvarColumn2.AutoIncrement = false;
				colvarColumn2.IsNullable = true;
				colvarColumn2.IsPrimaryKey = false;
				colvarColumn2.IsForeignKey = false;
				colvarColumn2.IsReadOnly = false;
				colvarColumn2.DefaultSetting = @"";
				colvarColumn2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarColumn2);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("TestTable",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Column1")]
		[Bindable(true)]
		public string Column1 
		{
			get { return GetColumnValue<string>(Columns.Column1); }
			set { SetColumnValue(Columns.Column1, value); }
		}
		  
		[XmlAttribute("Column2")]
		[Bindable(true)]
		public string Column2 
		{
			get { return GetColumnValue<string>(Columns.Column2); }
			set { SetColumnValue(Columns.Column2, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		public PowerPOS.TestTable2Collection TestTable2Records()
		{
			return new PowerPOS.TestTable2Collection().Where(TestTable2.Columns.Column1, Column1).Load();
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varColumn1,string varColumn2)
		{
			TestTable item = new TestTable();
			
			item.Column1 = varColumn1;
			
			item.Column2 = varColumn2;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varColumn1,string varColumn2)
		{
			TestTable item = new TestTable();
			
				item.Column1 = varColumn1;
			
				item.Column2 = varColumn2;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn Column1Column
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn Column2Column
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Column1 = @"Column1";
			 public static string Column2 = @"Column2";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
}
        #endregion
	}
}
