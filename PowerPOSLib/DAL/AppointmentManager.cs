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
	/// Strongly-typed collection for the AppointmentManager class.
	/// </summary>
    [Serializable]
	public partial class AppointmentManagerCollection : ActiveList<AppointmentManager, AppointmentManagerCollection>
	{	   
		public AppointmentManagerCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AppointmentManagerCollection</returns>
		public AppointmentManagerCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AppointmentManager o = this[i];
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
	/// This is an ActiveRecord class which wraps the AppointmentManager table.
	/// </summary>
	[Serializable]
	public partial class AppointmentManager : ActiveRecord<AppointmentManager>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AppointmentManager()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AppointmentManager(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AppointmentManager(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AppointmentManager(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("AppointmentManager", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarAppointmentID = new TableSchema.TableColumn(schema);
				colvarAppointmentID.ColumnName = "AppointmentID";
				colvarAppointmentID.DataType = DbType.Int32;
				colvarAppointmentID.MaxLength = 0;
				colvarAppointmentID.AutoIncrement = true;
				colvarAppointmentID.IsNullable = false;
				colvarAppointmentID.IsPrimaryKey = true;
				colvarAppointmentID.IsForeignKey = false;
				colvarAppointmentID.IsReadOnly = false;
				colvarAppointmentID.DefaultSetting = @"";
				colvarAppointmentID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAppointmentID);
				
				TableSchema.TableColumn colvarAppointmentDate = new TableSchema.TableColumn(schema);
				colvarAppointmentDate.ColumnName = "AppointmentDate";
				colvarAppointmentDate.DataType = DbType.AnsiString;
				colvarAppointmentDate.MaxLength = 0;
				colvarAppointmentDate.AutoIncrement = false;
				colvarAppointmentDate.IsNullable = true;
				colvarAppointmentDate.IsPrimaryKey = false;
				colvarAppointmentDate.IsForeignKey = false;
				colvarAppointmentDate.IsReadOnly = false;
				colvarAppointmentDate.DefaultSetting = @"";
				colvarAppointmentDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAppointmentDate);
				
				TableSchema.TableColumn colvarStartTime = new TableSchema.TableColumn(schema);
				colvarStartTime.ColumnName = "StartTime";
				colvarStartTime.DataType = DbType.AnsiString;
				colvarStartTime.MaxLength = 20;
				colvarStartTime.AutoIncrement = false;
				colvarStartTime.IsNullable = true;
				colvarStartTime.IsPrimaryKey = false;
				colvarStartTime.IsForeignKey = false;
				colvarStartTime.IsReadOnly = false;
				colvarStartTime.DefaultSetting = @"";
				colvarStartTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStartTime);
				
				TableSchema.TableColumn colvarEndTime = new TableSchema.TableColumn(schema);
				colvarEndTime.ColumnName = "EndTime";
				colvarEndTime.DataType = DbType.AnsiString;
				colvarEndTime.MaxLength = 20;
				colvarEndTime.AutoIncrement = false;
				colvarEndTime.IsNullable = true;
				colvarEndTime.IsPrimaryKey = false;
				colvarEndTime.IsForeignKey = false;
				colvarEndTime.IsReadOnly = false;
				colvarEndTime.DefaultSetting = @"";
				colvarEndTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEndTime);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 500;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarSalesPersonID = new TableSchema.TableColumn(schema);
				colvarSalesPersonID.ColumnName = "SalesPersonID";
				colvarSalesPersonID.DataType = DbType.AnsiString;
				colvarSalesPersonID.MaxLength = 100;
				colvarSalesPersonID.AutoIncrement = false;
				colvarSalesPersonID.IsNullable = true;
				colvarSalesPersonID.IsPrimaryKey = false;
				colvarSalesPersonID.IsForeignKey = false;
				colvarSalesPersonID.IsReadOnly = false;
				colvarSalesPersonID.DefaultSetting = @"";
				colvarSalesPersonID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSalesPersonID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("AppointmentManager",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("AppointmentID")]
		[Bindable(true)]
		public int AppointmentID 
		{
			get { return GetColumnValue<int>(Columns.AppointmentID); }
			set { SetColumnValue(Columns.AppointmentID, value); }
		}
		  
		[XmlAttribute("AppointmentDate")]
		[Bindable(true)]
		public string AppointmentDate 
		{
			get { return GetColumnValue<string>(Columns.AppointmentDate); }
			set { SetColumnValue(Columns.AppointmentDate, value); }
		}
		  
		[XmlAttribute("StartTime")]
		[Bindable(true)]
		public string StartTime 
		{
			get { return GetColumnValue<string>(Columns.StartTime); }
			set { SetColumnValue(Columns.StartTime, value); }
		}
		  
		[XmlAttribute("EndTime")]
		[Bindable(true)]
		public string EndTime 
		{
			get { return GetColumnValue<string>(Columns.EndTime); }
			set { SetColumnValue(Columns.EndTime, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("SalesPersonID")]
		[Bindable(true)]
		public string SalesPersonID 
		{
			get { return GetColumnValue<string>(Columns.SalesPersonID); }
			set { SetColumnValue(Columns.SalesPersonID, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varAppointmentDate,string varStartTime,string varEndTime,string varDescription,string varSalesPersonID)
		{
			AppointmentManager item = new AppointmentManager();
			
			item.AppointmentDate = varAppointmentDate;
			
			item.StartTime = varStartTime;
			
			item.EndTime = varEndTime;
			
			item.Description = varDescription;
			
			item.SalesPersonID = varSalesPersonID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varAppointmentID,string varAppointmentDate,string varStartTime,string varEndTime,string varDescription,string varSalesPersonID)
		{
			AppointmentManager item = new AppointmentManager();
			
				item.AppointmentID = varAppointmentID;
			
				item.AppointmentDate = varAppointmentDate;
			
				item.StartTime = varStartTime;
			
				item.EndTime = varEndTime;
			
				item.Description = varDescription;
			
				item.SalesPersonID = varSalesPersonID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn AppointmentIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn AppointmentDateColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn StartTimeColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn EndTimeColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn SalesPersonIDColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string AppointmentID = @"AppointmentID";
			 public static string AppointmentDate = @"AppointmentDate";
			 public static string StartTime = @"StartTime";
			 public static string EndTime = @"EndTime";
			 public static string Description = @"Description";
			 public static string SalesPersonID = @"SalesPersonID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
