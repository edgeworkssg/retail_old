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
	/// Strongly-typed collection for the EZLinkMsgLog class.
	/// </summary>
    [Serializable]
	public partial class EZLinkMsgLogCollection : ActiveList<EZLinkMsgLog, EZLinkMsgLogCollection>
	{	   
		public EZLinkMsgLogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>EZLinkMsgLogCollection</returns>
		public EZLinkMsgLogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                EZLinkMsgLog o = this[i];
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
	/// This is an ActiveRecord class which wraps the EZLinkMsgLog table.
	/// </summary>
	[Serializable]
	public partial class EZLinkMsgLog : ActiveRecord<EZLinkMsgLog>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public EZLinkMsgLog()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public EZLinkMsgLog(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public EZLinkMsgLog(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public EZLinkMsgLog(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("EZLinkMsgLog", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarMsgLogID = new TableSchema.TableColumn(schema);
				colvarMsgLogID.ColumnName = "msgLogID";
				colvarMsgLogID.DataType = DbType.Int32;
				colvarMsgLogID.MaxLength = 0;
				colvarMsgLogID.AutoIncrement = true;
				colvarMsgLogID.IsNullable = false;
				colvarMsgLogID.IsPrimaryKey = true;
				colvarMsgLogID.IsForeignKey = false;
				colvarMsgLogID.IsReadOnly = false;
				colvarMsgLogID.DefaultSetting = @"";
				colvarMsgLogID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMsgLogID);
				
				TableSchema.TableColumn colvarMsgDate = new TableSchema.TableColumn(schema);
				colvarMsgDate.ColumnName = "msgDate";
				colvarMsgDate.DataType = DbType.DateTime;
				colvarMsgDate.MaxLength = 0;
				colvarMsgDate.AutoIncrement = false;
				colvarMsgDate.IsNullable = false;
				colvarMsgDate.IsPrimaryKey = false;
				colvarMsgDate.IsForeignKey = false;
				colvarMsgDate.IsReadOnly = false;
				colvarMsgDate.DefaultSetting = @"";
				colvarMsgDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMsgDate);
				
				TableSchema.TableColumn colvarMsgContent = new TableSchema.TableColumn(schema);
				colvarMsgContent.ColumnName = "msgContent";
				colvarMsgContent.DataType = DbType.AnsiString;
				colvarMsgContent.MaxLength = 250;
				colvarMsgContent.AutoIncrement = false;
				colvarMsgContent.IsNullable = false;
				colvarMsgContent.IsPrimaryKey = false;
				colvarMsgContent.IsForeignKey = false;
				colvarMsgContent.IsReadOnly = false;
				colvarMsgContent.DefaultSetting = @"";
				colvarMsgContent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMsgContent);
				
				TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
				colvarDeleted.ColumnName = "Deleted";
				colvarDeleted.DataType = DbType.Boolean;
				colvarDeleted.MaxLength = 0;
				colvarDeleted.AutoIncrement = false;
				colvarDeleted.IsNullable = false;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				
						colvarDeleted.DefaultSetting = @"((0))";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
				colvarUniqueID.ColumnName = "uniqueID";
				colvarUniqueID.DataType = DbType.Guid;
				colvarUniqueID.MaxLength = 0;
				colvarUniqueID.AutoIncrement = false;
				colvarUniqueID.IsNullable = false;
				colvarUniqueID.IsPrimaryKey = false;
				colvarUniqueID.IsForeignKey = false;
				colvarUniqueID.IsReadOnly = false;
				
						colvarUniqueID.DefaultSetting = @"(newid())";
				colvarUniqueID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("EZLinkMsgLog",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("MsgLogID")]
		[Bindable(true)]
		public int MsgLogID 
		{
			get { return GetColumnValue<int>(Columns.MsgLogID); }
			set { SetColumnValue(Columns.MsgLogID, value); }
		}
		  
		[XmlAttribute("MsgDate")]
		[Bindable(true)]
		public DateTime MsgDate 
		{
			get { return GetColumnValue<DateTime>(Columns.MsgDate); }
			set { SetColumnValue(Columns.MsgDate, value); }
		}
		  
		[XmlAttribute("MsgContent")]
		[Bindable(true)]
		public string MsgContent 
		{
			get { return GetColumnValue<string>(Columns.MsgContent); }
			set { SetColumnValue(Columns.MsgContent, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool Deleted 
		{
			get { return GetColumnValue<bool>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("UniqueID")]
		[Bindable(true)]
		public Guid UniqueID 
		{
			get { return GetColumnValue<Guid>(Columns.UniqueID); }
			set { SetColumnValue(Columns.UniqueID, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varMsgDate,string varMsgContent,bool varDeleted,Guid varUniqueID)
		{
			EZLinkMsgLog item = new EZLinkMsgLog();
			
			item.MsgDate = varMsgDate;
			
			item.MsgContent = varMsgContent;
			
			item.Deleted = varDeleted;
			
			item.UniqueID = varUniqueID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varMsgLogID,DateTime varMsgDate,string varMsgContent,bool varDeleted,Guid varUniqueID)
		{
			EZLinkMsgLog item = new EZLinkMsgLog();
			
				item.MsgLogID = varMsgLogID;
			
				item.MsgDate = varMsgDate;
			
				item.MsgContent = varMsgContent;
			
				item.Deleted = varDeleted;
			
				item.UniqueID = varUniqueID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn MsgLogIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn MsgDateColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn MsgContentColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string MsgLogID = @"msgLogID";
			 public static string MsgDate = @"msgDate";
			 public static string MsgContent = @"msgContent";
			 public static string Deleted = @"Deleted";
			 public static string UniqueID = @"uniqueID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
