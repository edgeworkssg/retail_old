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
	/// Strongly-typed collection for the EmailNotification class.
	/// </summary>
    [Serializable]
	public partial class EmailNotificationCollection : ActiveList<EmailNotification, EmailNotificationCollection>
	{	   
		public EmailNotificationCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>EmailNotificationCollection</returns>
		public EmailNotificationCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                EmailNotification o = this[i];
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
	/// This is an ActiveRecord class which wraps the EmailNotification table.
	/// </summary>
	[Serializable]
	public partial class EmailNotification : ActiveRecord<EmailNotification>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public EmailNotification()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public EmailNotification(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public EmailNotification(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public EmailNotification(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("EmailNotification", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarEmailAddress = new TableSchema.TableColumn(schema);
				colvarEmailAddress.ColumnName = "EmailAddress";
				colvarEmailAddress.DataType = DbType.AnsiString;
				colvarEmailAddress.MaxLength = 50;
				colvarEmailAddress.AutoIncrement = false;
				colvarEmailAddress.IsNullable = false;
				colvarEmailAddress.IsPrimaryKey = true;
				colvarEmailAddress.IsForeignKey = false;
				colvarEmailAddress.IsReadOnly = false;
				colvarEmailAddress.DefaultSetting = @"";
				colvarEmailAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEmailAddress);
				
				TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
				colvarName.ColumnName = "Name";
				colvarName.DataType = DbType.AnsiString;
				colvarName.MaxLength = 50;
				colvarName.AutoIncrement = false;
				colvarName.IsNullable = false;
				colvarName.IsPrimaryKey = false;
				colvarName.IsForeignKey = false;
				colvarName.IsReadOnly = false;
				colvarName.DefaultSetting = @"";
				colvarName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarName);
				
				TableSchema.TableColumn colvarModuleX = new TableSchema.TableColumn(schema);
				colvarModuleX.ColumnName = "Module";
				colvarModuleX.DataType = DbType.AnsiString;
				colvarModuleX.MaxLength = 50;
				colvarModuleX.AutoIncrement = false;
				colvarModuleX.IsNullable = true;
				colvarModuleX.IsPrimaryKey = false;
				colvarModuleX.IsForeignKey = false;
				colvarModuleX.IsReadOnly = false;
				colvarModuleX.DefaultSetting = @"";
				colvarModuleX.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModuleX);
				
				TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
				colvarDeleted.ColumnName = "Deleted";
				colvarDeleted.DataType = DbType.Boolean;
				colvarDeleted.MaxLength = 0;
				colvarDeleted.AutoIncrement = false;
				colvarDeleted.IsNullable = true;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				colvarDeleted.DefaultSetting = @"";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("EmailNotification",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("EmailAddress")]
		[Bindable(true)]
		public string EmailAddress 
		{
			get { return GetColumnValue<string>(Columns.EmailAddress); }
			set { SetColumnValue(Columns.EmailAddress, value); }
		}
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("ModuleX")]
		[Bindable(true)]
		public string ModuleX 
		{
			get { return GetColumnValue<string>(Columns.ModuleX); }
			set { SetColumnValue(Columns.ModuleX, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varEmailAddress,string varName,string varModuleX,bool? varDeleted)
		{
			EmailNotification item = new EmailNotification();
			
			item.EmailAddress = varEmailAddress;
			
			item.Name = varName;
			
			item.ModuleX = varModuleX;
			
			item.Deleted = varDeleted;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varEmailAddress,string varName,string varModuleX,bool? varDeleted)
		{
			EmailNotification item = new EmailNotification();
			
				item.EmailAddress = varEmailAddress;
			
				item.Name = varName;
			
				item.ModuleX = varModuleX;
			
				item.Deleted = varDeleted;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn EmailAddressColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ModuleXColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string EmailAddress = @"EmailAddress";
			 public static string Name = @"Name";
			 public static string ModuleX = @"Module";
			 public static string Deleted = @"Deleted";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
