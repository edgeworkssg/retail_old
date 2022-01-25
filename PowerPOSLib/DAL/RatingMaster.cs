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
	/// Strongly-typed collection for the RatingMaster class.
	/// </summary>
    [Serializable]
	public partial class RatingMasterCollection : ActiveList<RatingMaster, RatingMasterCollection>
	{	   
		public RatingMasterCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>RatingMasterCollection</returns>
		public RatingMasterCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                RatingMaster o = this[i];
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
	/// This is an ActiveRecord class which wraps the RatingMaster table.
	/// </summary>
	[Serializable]
	public partial class RatingMaster : ActiveRecord<RatingMaster>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public RatingMaster()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public RatingMaster(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public RatingMaster(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public RatingMaster(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("RatingMaster", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarRating = new TableSchema.TableColumn(schema);
				colvarRating.ColumnName = "Rating";
				colvarRating.DataType = DbType.Int32;
				colvarRating.MaxLength = 0;
				colvarRating.AutoIncrement = true;
				colvarRating.IsNullable = false;
				colvarRating.IsPrimaryKey = true;
				colvarRating.IsForeignKey = false;
				colvarRating.IsReadOnly = false;
				colvarRating.DefaultSetting = @"";
				colvarRating.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRating);
				
				TableSchema.TableColumn colvarRatingName = new TableSchema.TableColumn(schema);
				colvarRatingName.ColumnName = "RatingName";
				colvarRatingName.DataType = DbType.AnsiString;
				colvarRatingName.MaxLength = 50;
				colvarRatingName.AutoIncrement = false;
				colvarRatingName.IsNullable = true;
				colvarRatingName.IsPrimaryKey = false;
				colvarRatingName.IsForeignKey = false;
				colvarRatingName.IsReadOnly = false;
				colvarRatingName.DefaultSetting = @"";
				colvarRatingName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRatingName);
				
				TableSchema.TableColumn colvarRatingImage = new TableSchema.TableColumn(schema);
				colvarRatingImage.ColumnName = "RatingImage";
				colvarRatingImage.DataType = DbType.Binary;
				colvarRatingImage.MaxLength = -1;
				colvarRatingImage.AutoIncrement = false;
				colvarRatingImage.IsNullable = true;
				colvarRatingImage.IsPrimaryKey = false;
				colvarRatingImage.IsForeignKey = false;
				colvarRatingImage.IsReadOnly = false;
				colvarRatingImage.DefaultSetting = @"";
				colvarRatingImage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRatingImage);
				
				TableSchema.TableColumn colvarRatingType = new TableSchema.TableColumn(schema);
				colvarRatingType.ColumnName = "RatingType";
				colvarRatingType.DataType = DbType.AnsiString;
				colvarRatingType.MaxLength = 50;
				colvarRatingType.AutoIncrement = false;
				colvarRatingType.IsNullable = true;
				colvarRatingType.IsPrimaryKey = false;
				colvarRatingType.IsForeignKey = false;
				colvarRatingType.IsReadOnly = false;
				colvarRatingType.DefaultSetting = @"";
				colvarRatingType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRatingType);
				
				TableSchema.TableColumn colvarWeight = new TableSchema.TableColumn(schema);
				colvarWeight.ColumnName = "Weight";
				colvarWeight.DataType = DbType.Int32;
				colvarWeight.MaxLength = 0;
				colvarWeight.AutoIncrement = false;
				colvarWeight.IsNullable = true;
				colvarWeight.IsPrimaryKey = false;
				colvarWeight.IsForeignKey = false;
				colvarWeight.IsReadOnly = false;
				colvarWeight.DefaultSetting = @"";
				colvarWeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWeight);
				
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
				
				TableSchema.TableColumn colvarRatingImageUrl = new TableSchema.TableColumn(schema);
				colvarRatingImageUrl.ColumnName = "RatingImageUrl";
				colvarRatingImageUrl.DataType = DbType.AnsiString;
				colvarRatingImageUrl.MaxLength = -1;
				colvarRatingImageUrl.AutoIncrement = false;
				colvarRatingImageUrl.IsNullable = true;
				colvarRatingImageUrl.IsPrimaryKey = false;
				colvarRatingImageUrl.IsForeignKey = false;
				colvarRatingImageUrl.IsReadOnly = false;
				colvarRatingImageUrl.DefaultSetting = @"";
				colvarRatingImageUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRatingImageUrl);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("RatingMaster",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Rating")]
		[Bindable(true)]
		public int Rating 
		{
			get { return GetColumnValue<int>(Columns.Rating); }
			set { SetColumnValue(Columns.Rating, value); }
		}
		  
		[XmlAttribute("RatingName")]
		[Bindable(true)]
		public string RatingName 
		{
			get { return GetColumnValue<string>(Columns.RatingName); }
			set { SetColumnValue(Columns.RatingName, value); }
		}
		  
		[XmlAttribute("RatingImage")]
		[Bindable(true)]
		public byte[] RatingImage 
		{
			get { return GetColumnValue<byte[]>(Columns.RatingImage); }
			set { SetColumnValue(Columns.RatingImage, value); }
		}
		  
		[XmlAttribute("RatingType")]
		[Bindable(true)]
		public string RatingType 
		{
			get { return GetColumnValue<string>(Columns.RatingType); }
			set { SetColumnValue(Columns.RatingType, value); }
		}
		  
		[XmlAttribute("Weight")]
		[Bindable(true)]
		public int? Weight 
		{
			get { return GetColumnValue<int?>(Columns.Weight); }
			set { SetColumnValue(Columns.Weight, value); }
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
		  
		[XmlAttribute("RatingImageUrl")]
		[Bindable(true)]
		public string RatingImageUrl 
		{
			get { return GetColumnValue<string>(Columns.RatingImageUrl); }
			set { SetColumnValue(Columns.RatingImageUrl, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varRatingName,byte[] varRatingImage,string varRatingType,int? varWeight,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varRatingImageUrl)
		{
			RatingMaster item = new RatingMaster();
			
			item.RatingName = varRatingName;
			
			item.RatingImage = varRatingImage;
			
			item.RatingType = varRatingType;
			
			item.Weight = varWeight;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.RatingImageUrl = varRatingImageUrl;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varRating,string varRatingName,byte[] varRatingImage,string varRatingType,int? varWeight,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varRatingImageUrl)
		{
			RatingMaster item = new RatingMaster();
			
				item.Rating = varRating;
			
				item.RatingName = varRatingName;
			
				item.RatingImage = varRatingImage;
			
				item.RatingType = varRatingType;
			
				item.Weight = varWeight;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.RatingImageUrl = varRatingImageUrl;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn RatingColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn RatingNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn RatingImageColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn RatingTypeColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn WeightColumn
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
        
        
        
        public static TableSchema.TableColumn RatingImageUrlColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Rating = @"Rating";
			 public static string RatingName = @"RatingName";
			 public static string RatingImage = @"RatingImage";
			 public static string RatingType = @"RatingType";
			 public static string Weight = @"Weight";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string RatingImageUrl = @"RatingImageUrl";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
