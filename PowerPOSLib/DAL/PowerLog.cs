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
	/// Strongly-typed collection for the PowerLog class.
	/// </summary>
    [Serializable]
	public partial class PowerLogCollection : ActiveList<PowerLog, PowerLogCollection>
	{	   
		public PowerLogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PowerLogCollection</returns>
		public PowerLogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PowerLog o = this[i];
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
	/// This is an ActiveRecord class which wraps the PowerLog table.
	/// </summary>
	[Serializable]
	public partial class PowerLog : ActiveRecord<PowerLog>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PowerLog()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PowerLog(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PowerLog(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PowerLog(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("PowerLog", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarLogId = new TableSchema.TableColumn(schema);
				colvarLogId.ColumnName = "LogId";
				colvarLogId.DataType = DbType.Int32;
				colvarLogId.MaxLength = 0;
				colvarLogId.AutoIncrement = true;
				colvarLogId.IsNullable = false;
				colvarLogId.IsPrimaryKey = true;
				colvarLogId.IsForeignKey = false;
				colvarLogId.IsReadOnly = false;
				colvarLogId.DefaultSetting = @"";
				colvarLogId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLogId);
				
				TableSchema.TableColumn colvarLogDate = new TableSchema.TableColumn(schema);
				colvarLogDate.ColumnName = "LogDate";
				colvarLogDate.DataType = DbType.DateTime;
				colvarLogDate.MaxLength = 0;
				colvarLogDate.AutoIncrement = false;
				colvarLogDate.IsNullable = false;
				colvarLogDate.IsPrimaryKey = false;
				colvarLogDate.IsForeignKey = false;
				colvarLogDate.IsReadOnly = false;
				
						colvarLogDate.DefaultSetting = @"(getdate())";
				colvarLogDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLogDate);
				
				TableSchema.TableColumn colvarLogMsg = new TableSchema.TableColumn(schema);
				colvarLogMsg.ColumnName = "LogMsg";
				colvarLogMsg.DataType = DbType.AnsiString;
				colvarLogMsg.MaxLength = -1;
				colvarLogMsg.AutoIncrement = false;
				colvarLogMsg.IsNullable = true;
				colvarLogMsg.IsPrimaryKey = false;
				colvarLogMsg.IsForeignKey = false;
				colvarLogMsg.IsReadOnly = false;
				colvarLogMsg.DefaultSetting = @"";
				colvarLogMsg.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLogMsg);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("PowerLog",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("LogId")]
		[Bindable(true)]
		public int LogId 
		{
			get { return GetColumnValue<int>(Columns.LogId); }
			set { SetColumnValue(Columns.LogId, value); }
		}
		  
		[XmlAttribute("LogDate")]
		[Bindable(true)]
		public DateTime LogDate 
		{
			get { return GetColumnValue<DateTime>(Columns.LogDate); }
			set { SetColumnValue(Columns.LogDate, value); }
		}
		  
		[XmlAttribute("LogMsg")]
		[Bindable(true)]
		public string LogMsg 
		{
			get { return GetColumnValue<string>(Columns.LogMsg); }
			set { SetColumnValue(Columns.LogMsg, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varLogDate,string varLogMsg)
		{
			PowerLog item = new PowerLog();
			
			item.LogDate = varLogDate;
			
			item.LogMsg = varLogMsg;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varLogId,DateTime varLogDate,string varLogMsg)
		{
			PowerLog item = new PowerLog();
			
				item.LogId = varLogId;
			
				item.LogDate = varLogDate;
			
				item.LogMsg = varLogMsg;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn LogIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn LogDateColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn LogMsgColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string LogId = @"LogId";
			 public static string LogDate = @"LogDate";
			 public static string LogMsg = @"LogMsg";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
