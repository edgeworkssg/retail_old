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
	/// Strongly-typed collection for the AppSettingTombstone class.
	/// </summary>
    [Serializable]
	public partial class AppSettingTombstoneCollection : ActiveList<AppSettingTombstone, AppSettingTombstoneCollection>
	{	   
		public AppSettingTombstoneCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AppSettingTombstoneCollection</returns>
		public AppSettingTombstoneCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AppSettingTombstone o = this[i];
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
	/// This is an ActiveRecord class which wraps the AppSetting_Tombstone table.
	/// </summary>
	[Serializable]
	public partial class AppSettingTombstone : ActiveRecord<AppSettingTombstone>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AppSettingTombstone()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AppSettingTombstone(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AppSettingTombstone(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AppSettingTombstone(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("AppSetting_Tombstone", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarAppSettingId = new TableSchema.TableColumn(schema);
				colvarAppSettingId.ColumnName = "AppSettingId";
				colvarAppSettingId.DataType = DbType.Guid;
				colvarAppSettingId.MaxLength = 0;
				colvarAppSettingId.AutoIncrement = false;
				colvarAppSettingId.IsNullable = false;
				colvarAppSettingId.IsPrimaryKey = true;
				colvarAppSettingId.IsForeignKey = false;
				colvarAppSettingId.IsReadOnly = false;
				colvarAppSettingId.DefaultSetting = @"";
				colvarAppSettingId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAppSettingId);
				
				TableSchema.TableColumn colvarDeletionDate = new TableSchema.TableColumn(schema);
				colvarDeletionDate.ColumnName = "DeletionDate";
				colvarDeletionDate.DataType = DbType.DateTime;
				colvarDeletionDate.MaxLength = 0;
				colvarDeletionDate.AutoIncrement = false;
				colvarDeletionDate.IsNullable = true;
				colvarDeletionDate.IsPrimaryKey = false;
				colvarDeletionDate.IsForeignKey = false;
				colvarDeletionDate.IsReadOnly = false;
				colvarDeletionDate.DefaultSetting = @"";
				colvarDeletionDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeletionDate);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("AppSetting_Tombstone",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("AppSettingId")]
		[Bindable(true)]
		public Guid AppSettingId 
		{
			get { return GetColumnValue<Guid>(Columns.AppSettingId); }
			set { SetColumnValue(Columns.AppSettingId, value); }
		}
		  
		[XmlAttribute("DeletionDate")]
		[Bindable(true)]
		public DateTime? DeletionDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.DeletionDate); }
			set { SetColumnValue(Columns.DeletionDate, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varAppSettingId,DateTime? varDeletionDate)
		{
			AppSettingTombstone item = new AppSettingTombstone();
			
			item.AppSettingId = varAppSettingId;
			
			item.DeletionDate = varDeletionDate;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varAppSettingId,DateTime? varDeletionDate)
		{
			AppSettingTombstone item = new AppSettingTombstone();
			
				item.AppSettingId = varAppSettingId;
			
				item.DeletionDate = varDeletionDate;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn AppSettingIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletionDateColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string AppSettingId = @"AppSettingId";
			 public static string DeletionDate = @"DeletionDate";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
