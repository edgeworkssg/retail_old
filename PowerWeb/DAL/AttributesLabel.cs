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
	/// Strongly-typed collection for the AttributesLabel class.
	/// </summary>
    [Serializable]
	public partial class AttributesLabelCollection : ActiveList<AttributesLabel, AttributesLabelCollection>
	{	   
		public AttributesLabelCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AttributesLabelCollection</returns>
		public AttributesLabelCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AttributesLabel o = this[i];
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
	/// This is an ActiveRecord class which wraps the AttributesLabel table.
	/// </summary>
	[Serializable]
	public partial class AttributesLabel : ActiveRecord<AttributesLabel>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AttributesLabel()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AttributesLabel(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AttributesLabel(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AttributesLabel(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("AttributesLabel", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarAttributesNo = new TableSchema.TableColumn(schema);
				colvarAttributesNo.ColumnName = "AttributesNo";
				colvarAttributesNo.DataType = DbType.Int32;
				colvarAttributesNo.MaxLength = 0;
				colvarAttributesNo.AutoIncrement = false;
				colvarAttributesNo.IsNullable = false;
				colvarAttributesNo.IsPrimaryKey = true;
				colvarAttributesNo.IsForeignKey = false;
				colvarAttributesNo.IsReadOnly = false;
				colvarAttributesNo.DefaultSetting = @"";
				colvarAttributesNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAttributesNo);
				
				TableSchema.TableColumn colvarLabel = new TableSchema.TableColumn(schema);
				colvarLabel.ColumnName = "Label";
				colvarLabel.DataType = DbType.String;
				colvarLabel.MaxLength = -1;
				colvarLabel.AutoIncrement = false;
				colvarLabel.IsNullable = false;
				colvarLabel.IsPrimaryKey = false;
				colvarLabel.IsForeignKey = false;
				colvarLabel.IsReadOnly = false;
				colvarLabel.DefaultSetting = @"";
				colvarLabel.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLabel);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("AttributesLabel",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("AttributesNo")]
		[Bindable(true)]
		public int AttributesNo 
		{
			get { return GetColumnValue<int>(Columns.AttributesNo); }
			set { SetColumnValue(Columns.AttributesNo, value); }
		}
		  
		[XmlAttribute("Label")]
		[Bindable(true)]
		public string Label 
		{
			get { return GetColumnValue<string>(Columns.Label); }
			set { SetColumnValue(Columns.Label, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varAttributesNo,string varLabel)
		{
			AttributesLabel item = new AttributesLabel();
			
			item.AttributesNo = varAttributesNo;
			
			item.Label = varLabel;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varAttributesNo,string varLabel)
		{
			AttributesLabel item = new AttributesLabel();
			
				item.AttributesNo = varAttributesNo;
			
				item.Label = varLabel;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn AttributesNoColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn LabelColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string AttributesNo = @"AttributesNo";
			 public static string Label = @"Label";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
