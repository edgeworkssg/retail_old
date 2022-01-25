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
	/// Strongly-typed collection for the SyncCommand class.
	/// </summary>
    [Serializable]
	public partial class SyncCommandCollection : ActiveList<SyncCommand, SyncCommandCollection>
	{	   
		public SyncCommandCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SyncCommandCollection</returns>
		public SyncCommandCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SyncCommand o = this[i];
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
	/// This is an ActiveRecord class which wraps the SyncCommand table.
	/// </summary>
	[Serializable]
	public partial class SyncCommand : ActiveRecord<SyncCommand>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SyncCommand()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SyncCommand(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SyncCommand(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SyncCommand(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SyncCommand", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarSyncCommandID = new TableSchema.TableColumn(schema);
				colvarSyncCommandID.ColumnName = "SyncCommandID";
				colvarSyncCommandID.DataType = DbType.Guid;
				colvarSyncCommandID.MaxLength = 0;
				colvarSyncCommandID.AutoIncrement = false;
				colvarSyncCommandID.IsNullable = false;
				colvarSyncCommandID.IsPrimaryKey = true;
				colvarSyncCommandID.IsForeignKey = false;
				colvarSyncCommandID.IsReadOnly = false;
				colvarSyncCommandID.DefaultSetting = @"";
				colvarSyncCommandID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSyncCommandID);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 100;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = false;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarTheCommand = new TableSchema.TableColumn(schema);
				colvarTheCommand.ColumnName = "TheCommand";
				colvarTheCommand.DataType = DbType.Binary;
				colvarTheCommand.MaxLength = 2147483647;
				colvarTheCommand.AutoIncrement = false;
				colvarTheCommand.IsNullable = false;
				colvarTheCommand.IsPrimaryKey = false;
				colvarTheCommand.IsForeignKey = false;
				colvarTheCommand.IsReadOnly = false;
				colvarTheCommand.DefaultSetting = @"";
				colvarTheCommand.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTheCommand);
				
				TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
				colvarCreatedOn.ColumnName = "CreatedOn";
				colvarCreatedOn.DataType = DbType.DateTime;
				colvarCreatedOn.MaxLength = 0;
				colvarCreatedOn.AutoIncrement = false;
				colvarCreatedOn.IsNullable = false;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				colvarCreatedOn.DefaultSetting = @"";
				colvarCreatedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedOn);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.AnsiString;
				colvarCreatedBy.MaxLength = 50;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = false;
				colvarCreatedBy.IsPrimaryKey = false;
				colvarCreatedBy.IsForeignKey = false;
				colvarCreatedBy.IsReadOnly = false;
				colvarCreatedBy.DefaultSetting = @"";
				colvarCreatedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedBy);
				
				TableSchema.TableColumn colvarExecutedOn = new TableSchema.TableColumn(schema);
				colvarExecutedOn.ColumnName = "ExecutedOn";
				colvarExecutedOn.DataType = DbType.DateTime;
				colvarExecutedOn.MaxLength = 0;
				colvarExecutedOn.AutoIncrement = false;
				colvarExecutedOn.IsNullable = true;
				colvarExecutedOn.IsPrimaryKey = false;
				colvarExecutedOn.IsForeignKey = false;
				colvarExecutedOn.IsReadOnly = false;
				colvarExecutedOn.DefaultSetting = @"";
				colvarExecutedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarExecutedOn);
				
				TableSchema.TableColumn colvarExecutedBy = new TableSchema.TableColumn(schema);
				colvarExecutedBy.ColumnName = "ExecutedBy";
				colvarExecutedBy.DataType = DbType.AnsiString;
				colvarExecutedBy.MaxLength = 50;
				colvarExecutedBy.AutoIncrement = false;
				colvarExecutedBy.IsNullable = true;
				colvarExecutedBy.IsPrimaryKey = false;
				colvarExecutedBy.IsForeignKey = false;
				colvarExecutedBy.IsReadOnly = false;
				colvarExecutedBy.DefaultSetting = @"";
				colvarExecutedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarExecutedBy);
				
				TableSchema.TableColumn colvarRemarks = new TableSchema.TableColumn(schema);
				colvarRemarks.ColumnName = "Remarks";
				colvarRemarks.DataType = DbType.AnsiString;
				colvarRemarks.MaxLength = 200;
				colvarRemarks.AutoIncrement = false;
				colvarRemarks.IsNullable = true;
				colvarRemarks.IsPrimaryKey = false;
				colvarRemarks.IsForeignKey = false;
				colvarRemarks.IsReadOnly = false;
				colvarRemarks.DefaultSetting = @"";
				colvarRemarks.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemarks);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("SyncCommand",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("SyncCommandID")]
		[Bindable(true)]
		public Guid SyncCommandID 
		{
			get { return GetColumnValue<Guid>(Columns.SyncCommandID); }
			set { SetColumnValue(Columns.SyncCommandID, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("TheCommand")]
		[Bindable(true)]
		public byte[] TheCommand 
		{
			get { return GetColumnValue<byte[]>(Columns.TheCommand); }
			set { SetColumnValue(Columns.TheCommand, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime CreatedOn 
		{
			get { return GetColumnValue<DateTime>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("ExecutedOn")]
		[Bindable(true)]
		public DateTime? ExecutedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ExecutedOn); }
			set { SetColumnValue(Columns.ExecutedOn, value); }
		}
		  
		[XmlAttribute("ExecutedBy")]
		[Bindable(true)]
		public string ExecutedBy 
		{
			get { return GetColumnValue<string>(Columns.ExecutedBy); }
			set { SetColumnValue(Columns.ExecutedBy, value); }
		}
		  
		[XmlAttribute("Remarks")]
		[Bindable(true)]
		public string Remarks 
		{
			get { return GetColumnValue<string>(Columns.Remarks); }
			set { SetColumnValue(Columns.Remarks, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varSyncCommandID,string varDescription,byte[] varTheCommand,DateTime varCreatedOn,string varCreatedBy,DateTime? varExecutedOn,string varExecutedBy,string varRemarks)
		{
			SyncCommand item = new SyncCommand();
			
			item.SyncCommandID = varSyncCommandID;
			
			item.Description = varDescription;
			
			item.TheCommand = varTheCommand;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ExecutedOn = varExecutedOn;
			
			item.ExecutedBy = varExecutedBy;
			
			item.Remarks = varRemarks;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varSyncCommandID,string varDescription,byte[] varTheCommand,DateTime varCreatedOn,string varCreatedBy,DateTime? varExecutedOn,string varExecutedBy,string varRemarks)
		{
			SyncCommand item = new SyncCommand();
			
				item.SyncCommandID = varSyncCommandID;
			
				item.Description = varDescription;
			
				item.TheCommand = varTheCommand;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ExecutedOn = varExecutedOn;
			
				item.ExecutedBy = varExecutedBy;
			
				item.Remarks = varRemarks;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn SyncCommandIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TheCommandColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ExecutedOnColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ExecutedByColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarksColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string SyncCommandID = @"SyncCommandID";
			 public static string Description = @"Description";
			 public static string TheCommand = @"TheCommand";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ExecutedOn = @"ExecutedOn";
			 public static string ExecutedBy = @"ExecutedBy";
			 public static string Remarks = @"Remarks";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
