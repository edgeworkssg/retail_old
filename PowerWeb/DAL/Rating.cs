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
	/// Strongly-typed collection for the Rating class.
	/// </summary>
    [Serializable]
	public partial class RatingCollection : ActiveList<Rating, RatingCollection>
	{	   
		public RatingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>RatingCollection</returns>
		public RatingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Rating o = this[i];
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
	/// This is an ActiveRecord class which wraps the Rating table.
	/// </summary>
	[Serializable]
	public partial class Rating : ActiveRecord<Rating>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Rating()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Rating(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Rating(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Rating(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Rating", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarRatingID = new TableSchema.TableColumn(schema);
				colvarRatingID.ColumnName = "RatingID";
				colvarRatingID.DataType = DbType.Int32;
				colvarRatingID.MaxLength = 0;
				colvarRatingID.AutoIncrement = true;
				colvarRatingID.IsNullable = false;
				colvarRatingID.IsPrimaryKey = true;
				colvarRatingID.IsForeignKey = false;
				colvarRatingID.IsReadOnly = false;
				colvarRatingID.DefaultSetting = @"";
				colvarRatingID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRatingID);
				
				TableSchema.TableColumn colvarPosid = new TableSchema.TableColumn(schema);
				colvarPosid.ColumnName = "POSID";
				colvarPosid.DataType = DbType.Int32;
				colvarPosid.MaxLength = 0;
				colvarPosid.AutoIncrement = false;
				colvarPosid.IsNullable = true;
				colvarPosid.IsPrimaryKey = false;
				colvarPosid.IsForeignKey = false;
				colvarPosid.IsReadOnly = false;
				colvarPosid.DefaultSetting = @"";
				colvarPosid.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPosid);
				
				TableSchema.TableColumn colvarRatingX = new TableSchema.TableColumn(schema);
				colvarRatingX.ColumnName = "Rating";
				colvarRatingX.DataType = DbType.Int32;
				colvarRatingX.MaxLength = 0;
				colvarRatingX.AutoIncrement = false;
				colvarRatingX.IsNullable = true;
				colvarRatingX.IsPrimaryKey = false;
				colvarRatingX.IsForeignKey = false;
				colvarRatingX.IsReadOnly = false;
				colvarRatingX.DefaultSetting = @"";
				colvarRatingX.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRatingX);
				
				TableSchema.TableColumn colvarStaff = new TableSchema.TableColumn(schema);
				colvarStaff.ColumnName = "Staff";
				colvarStaff.DataType = DbType.AnsiString;
				colvarStaff.MaxLength = 50;
				colvarStaff.AutoIncrement = false;
				colvarStaff.IsNullable = true;
				colvarStaff.IsPrimaryKey = false;
				colvarStaff.IsForeignKey = false;
				colvarStaff.IsReadOnly = false;
				colvarStaff.DefaultSetting = @"";
				colvarStaff.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStaff);
				
				TableSchema.TableColumn colvarTimestamp = new TableSchema.TableColumn(schema);
				colvarTimestamp.ColumnName = "Timestamp";
				colvarTimestamp.DataType = DbType.DateTime;
				colvarTimestamp.MaxLength = 0;
				colvarTimestamp.AutoIncrement = false;
				colvarTimestamp.IsNullable = true;
				colvarTimestamp.IsPrimaryKey = false;
				colvarTimestamp.IsForeignKey = false;
				colvarTimestamp.IsReadOnly = false;
				colvarTimestamp.DefaultSetting = @"";
				colvarTimestamp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTimestamp);
				
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
				
				TableSchema.TableColumn colvarUniqueId = new TableSchema.TableColumn(schema);
				colvarUniqueId.ColumnName = "UniqueId";
				colvarUniqueId.DataType = DbType.AnsiString;
				colvarUniqueId.MaxLength = 100;
				colvarUniqueId.AutoIncrement = false;
				colvarUniqueId.IsNullable = true;
				colvarUniqueId.IsPrimaryKey = false;
				colvarUniqueId.IsForeignKey = false;
				colvarUniqueId.IsReadOnly = false;
				colvarUniqueId.DefaultSetting = @"";
				colvarUniqueId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueId);
				
				TableSchema.TableColumn colvarOrderHdrID = new TableSchema.TableColumn(schema);
				colvarOrderHdrID.ColumnName = "OrderHdrID";
				colvarOrderHdrID.DataType = DbType.AnsiString;
				colvarOrderHdrID.MaxLength = 14;
				colvarOrderHdrID.AutoIncrement = false;
				colvarOrderHdrID.IsNullable = true;
				colvarOrderHdrID.IsPrimaryKey = false;
				colvarOrderHdrID.IsForeignKey = false;
				colvarOrderHdrID.IsReadOnly = false;
				colvarOrderHdrID.DefaultSetting = @"";
				colvarOrderHdrID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderHdrID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Rating",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("RatingID")]
		[Bindable(true)]
		public int RatingID 
		{
			get { return GetColumnValue<int>(Columns.RatingID); }
			set { SetColumnValue(Columns.RatingID, value); }
		}
		  
		[XmlAttribute("Posid")]
		[Bindable(true)]
		public int? Posid 
		{
			get { return GetColumnValue<int?>(Columns.Posid); }
			set { SetColumnValue(Columns.Posid, value); }
		}
		  
		[XmlAttribute("RatingX")]
		[Bindable(true)]
		public int? RatingX 
		{
			get { return GetColumnValue<int?>(Columns.RatingX); }
			set { SetColumnValue(Columns.RatingX, value); }
		}
		  
		[XmlAttribute("Staff")]
		[Bindable(true)]
		public string Staff 
		{
			get { return GetColumnValue<string>(Columns.Staff); }
			set { SetColumnValue(Columns.Staff, value); }
		}
		  
		[XmlAttribute("Timestamp")]
		[Bindable(true)]
		public DateTime? Timestamp 
		{
			get { return GetColumnValue<DateTime?>(Columns.Timestamp); }
			set { SetColumnValue(Columns.Timestamp, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("UniqueId")]
		[Bindable(true)]
		public string UniqueId 
		{
			get { return GetColumnValue<string>(Columns.UniqueId); }
			set { SetColumnValue(Columns.UniqueId, value); }
		}
		  
		[XmlAttribute("OrderHdrID")]
		[Bindable(true)]
		public string OrderHdrID 
		{
			get { return GetColumnValue<string>(Columns.OrderHdrID); }
			set { SetColumnValue(Columns.OrderHdrID, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int? varPosid,int? varRatingX,string varStaff,DateTime? varTimestamp,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varUniqueId,string varOrderHdrID)
		{
			Rating item = new Rating();
			
			item.Posid = varPosid;
			
			item.RatingX = varRatingX;
			
			item.Staff = varStaff;
			
			item.Timestamp = varTimestamp;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.UniqueId = varUniqueId;
			
			item.OrderHdrID = varOrderHdrID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varRatingID,int? varPosid,int? varRatingX,string varStaff,DateTime? varTimestamp,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varUniqueId,string varOrderHdrID)
		{
			Rating item = new Rating();
			
				item.RatingID = varRatingID;
			
				item.Posid = varPosid;
			
				item.RatingX = varRatingX;
			
				item.Staff = varStaff;
			
				item.Timestamp = varTimestamp;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.UniqueId = varUniqueId;
			
				item.OrderHdrID = varOrderHdrID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn RatingIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PosidColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn RatingXColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn StaffColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TimestampColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIdColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderHdrIDColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string RatingID = @"RatingID";
			 public static string Posid = @"POSID";
			 public static string RatingX = @"Rating";
			 public static string Staff = @"Staff";
			 public static string Timestamp = @"Timestamp";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string UniqueId = @"UniqueId";
			 public static string OrderHdrID = @"OrderHdrID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
