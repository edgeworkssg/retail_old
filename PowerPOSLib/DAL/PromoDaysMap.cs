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
	/// Strongly-typed collection for the PromoDaysMap class.
	/// </summary>
    [Serializable]
	public partial class PromoDaysMapCollection : ActiveList<PromoDaysMap, PromoDaysMapCollection>
	{	   
		public PromoDaysMapCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PromoDaysMapCollection</returns>
		public PromoDaysMapCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PromoDaysMap o = this[i];
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
	/// This is an ActiveRecord class which wraps the PromoDaysMap table.
	/// </summary>
	[Serializable]
	public partial class PromoDaysMap : ActiveRecord<PromoDaysMap>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PromoDaysMap()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PromoDaysMap(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PromoDaysMap(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PromoDaysMap(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("PromoDaysMap", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPromoDaysID = new TableSchema.TableColumn(schema);
				colvarPromoDaysID.ColumnName = "PromoDaysID";
				colvarPromoDaysID.DataType = DbType.Int32;
				colvarPromoDaysID.MaxLength = 0;
				colvarPromoDaysID.AutoIncrement = true;
				colvarPromoDaysID.IsNullable = false;
				colvarPromoDaysID.IsPrimaryKey = true;
				colvarPromoDaysID.IsForeignKey = false;
				colvarPromoDaysID.IsReadOnly = false;
				colvarPromoDaysID.DefaultSetting = @"";
				colvarPromoDaysID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPromoDaysID);
				
				TableSchema.TableColumn colvarPromoCampaignHdrID = new TableSchema.TableColumn(schema);
				colvarPromoCampaignHdrID.ColumnName = "PromoCampaignHdrID";
				colvarPromoCampaignHdrID.DataType = DbType.Int32;
				colvarPromoCampaignHdrID.MaxLength = 0;
				colvarPromoCampaignHdrID.AutoIncrement = false;
				colvarPromoCampaignHdrID.IsNullable = true;
				colvarPromoCampaignHdrID.IsPrimaryKey = false;
				colvarPromoCampaignHdrID.IsForeignKey = false;
				colvarPromoCampaignHdrID.IsReadOnly = false;
				colvarPromoCampaignHdrID.DefaultSetting = @"";
				colvarPromoCampaignHdrID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPromoCampaignHdrID);
				
				TableSchema.TableColumn colvarDaysPromo = new TableSchema.TableColumn(schema);
				colvarDaysPromo.ColumnName = "DaysPromo";
				colvarDaysPromo.DataType = DbType.AnsiString;
				colvarDaysPromo.MaxLength = 20;
				colvarDaysPromo.AutoIncrement = false;
				colvarDaysPromo.IsNullable = true;
				colvarDaysPromo.IsPrimaryKey = false;
				colvarDaysPromo.IsForeignKey = false;
				colvarDaysPromo.IsReadOnly = false;
				colvarDaysPromo.DefaultSetting = @"";
				colvarDaysPromo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDaysPromo);
				
				TableSchema.TableColumn colvarDaysNumber = new TableSchema.TableColumn(schema);
				colvarDaysNumber.ColumnName = "DaysNumber";
				colvarDaysNumber.DataType = DbType.Int32;
				colvarDaysNumber.MaxLength = 0;
				colvarDaysNumber.AutoIncrement = false;
				colvarDaysNumber.IsNullable = true;
				colvarDaysNumber.IsPrimaryKey = false;
				colvarDaysNumber.IsForeignKey = false;
				colvarDaysNumber.IsReadOnly = false;
				colvarDaysNumber.DefaultSetting = @"";
				colvarDaysNumber.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDaysNumber);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.AnsiString;
				colvarCreatedBy.MaxLength = 50;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = true;
				colvarCreatedBy.IsPrimaryKey = false;
				colvarCreatedBy.IsForeignKey = false;
				colvarCreatedBy.IsReadOnly = false;
				colvarCreatedBy.DefaultSetting = @"";
				colvarCreatedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedBy);
				
				TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
				colvarCreatedOn.ColumnName = "CreatedOn";
				colvarCreatedOn.DataType = DbType.DateTime;
				colvarCreatedOn.MaxLength = 0;
				colvarCreatedOn.AutoIncrement = false;
				colvarCreatedOn.IsNullable = true;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				colvarCreatedOn.DefaultSetting = @"";
				colvarCreatedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedOn);
				
				TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
				colvarModifiedBy.ColumnName = "ModifiedBy";
				colvarModifiedBy.DataType = DbType.AnsiString;
				colvarModifiedBy.MaxLength = 50;
				colvarModifiedBy.AutoIncrement = false;
				colvarModifiedBy.IsNullable = true;
				colvarModifiedBy.IsPrimaryKey = false;
				colvarModifiedBy.IsForeignKey = false;
				colvarModifiedBy.IsReadOnly = false;
				colvarModifiedBy.DefaultSetting = @"";
				colvarModifiedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedBy);
				
				TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
				colvarModifiedOn.ColumnName = "ModifiedOn";
				colvarModifiedOn.DataType = DbType.DateTime;
				colvarModifiedOn.MaxLength = 0;
				colvarModifiedOn.AutoIncrement = false;
				colvarModifiedOn.IsNullable = true;
				colvarModifiedOn.IsPrimaryKey = false;
				colvarModifiedOn.IsForeignKey = false;
				colvarModifiedOn.IsReadOnly = false;
				colvarModifiedOn.DefaultSetting = @"";
				colvarModifiedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedOn);
				
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
				DataService.Providers["PowerPOS"].AddSchema("PromoDaysMap",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PromoDaysID")]
		[Bindable(true)]
		public int PromoDaysID 
		{
			get { return GetColumnValue<int>(Columns.PromoDaysID); }
			set { SetColumnValue(Columns.PromoDaysID, value); }
		}
		  
		[XmlAttribute("PromoCampaignHdrID")]
		[Bindable(true)]
		public int? PromoCampaignHdrID 
		{
			get { return GetColumnValue<int?>(Columns.PromoCampaignHdrID); }
			set { SetColumnValue(Columns.PromoCampaignHdrID, value); }
		}
		  
		[XmlAttribute("DaysPromo")]
		[Bindable(true)]
		public string DaysPromo 
		{
			get { return GetColumnValue<string>(Columns.DaysPromo); }
			set { SetColumnValue(Columns.DaysPromo, value); }
		}
		  
		[XmlAttribute("DaysNumber")]
		[Bindable(true)]
		public int? DaysNumber 
		{
			get { return GetColumnValue<int?>(Columns.DaysNumber); }
			set { SetColumnValue(Columns.DaysNumber, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
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
		public static void Insert(int? varPromoCampaignHdrID,string varDaysPromo,int? varDaysNumber,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,bool? varDeleted)
		{
			PromoDaysMap item = new PromoDaysMap();
			
			item.PromoCampaignHdrID = varPromoCampaignHdrID;
			
			item.DaysPromo = varDaysPromo;
			
			item.DaysNumber = varDaysNumber;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.Deleted = varDeleted;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varPromoDaysID,int? varPromoCampaignHdrID,string varDaysPromo,int? varDaysNumber,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,bool? varDeleted)
		{
			PromoDaysMap item = new PromoDaysMap();
			
				item.PromoDaysID = varPromoDaysID;
			
				item.PromoCampaignHdrID = varPromoCampaignHdrID;
			
				item.DaysPromo = varDaysPromo;
			
				item.DaysNumber = varDaysNumber;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.Deleted = varDeleted;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn PromoDaysIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PromoCampaignHdrIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DaysPromoColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DaysNumberColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PromoDaysID = @"PromoDaysID";
			 public static string PromoCampaignHdrID = @"PromoCampaignHdrID";
			 public static string DaysPromo = @"DaysPromo";
			 public static string DaysNumber = @"DaysNumber";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string Deleted = @"Deleted";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
