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
	/// Strongly-typed collection for the RatingFeedback class.
	/// </summary>
    [Serializable]
	public partial class RatingFeedbackCollection : ActiveList<RatingFeedback, RatingFeedbackCollection>
	{	   
		public RatingFeedbackCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>RatingFeedbackCollection</returns>
		public RatingFeedbackCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                RatingFeedback o = this[i];
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
	/// This is an ActiveRecord class which wraps the RatingFeedback table.
	/// </summary>
	[Serializable]
	public partial class RatingFeedback : ActiveRecord<RatingFeedback>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public RatingFeedback()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public RatingFeedback(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public RatingFeedback(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public RatingFeedback(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("RatingFeedback", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarRatingFeedbackID = new TableSchema.TableColumn(schema);
				colvarRatingFeedbackID.ColumnName = "RatingFeedbackID";
				colvarRatingFeedbackID.DataType = DbType.Int32;
				colvarRatingFeedbackID.MaxLength = 0;
				colvarRatingFeedbackID.AutoIncrement = true;
				colvarRatingFeedbackID.IsNullable = false;
				colvarRatingFeedbackID.IsPrimaryKey = true;
				colvarRatingFeedbackID.IsForeignKey = false;
				colvarRatingFeedbackID.IsReadOnly = false;
				colvarRatingFeedbackID.DefaultSetting = @"";
				colvarRatingFeedbackID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRatingFeedbackID);
				
				TableSchema.TableColumn colvarSelectionText = new TableSchema.TableColumn(schema);
				colvarSelectionText.ColumnName = "SelectionText";
				colvarSelectionText.DataType = DbType.AnsiString;
				colvarSelectionText.MaxLength = 50;
				colvarSelectionText.AutoIncrement = false;
				colvarSelectionText.IsNullable = true;
				colvarSelectionText.IsPrimaryKey = false;
				colvarSelectionText.IsForeignKey = false;
				colvarSelectionText.IsReadOnly = false;
				colvarSelectionText.DefaultSetting = @"";
				colvarSelectionText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSelectionText);
				
				TableSchema.TableColumn colvarSelectionImage = new TableSchema.TableColumn(schema);
				colvarSelectionImage.ColumnName = "SelectionImage";
				colvarSelectionImage.DataType = DbType.Binary;
				colvarSelectionImage.MaxLength = -1;
				colvarSelectionImage.AutoIncrement = false;
				colvarSelectionImage.IsNullable = true;
				colvarSelectionImage.IsPrimaryKey = false;
				colvarSelectionImage.IsForeignKey = false;
				colvarSelectionImage.IsReadOnly = false;
				colvarSelectionImage.DefaultSetting = @"";
				colvarSelectionImage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSelectionImage);
				
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
				
				TableSchema.TableColumn colvarSelectionImageUrl = new TableSchema.TableColumn(schema);
				colvarSelectionImageUrl.ColumnName = "SelectionImageUrl";
				colvarSelectionImageUrl.DataType = DbType.AnsiString;
				colvarSelectionImageUrl.MaxLength = -1;
				colvarSelectionImageUrl.AutoIncrement = false;
				colvarSelectionImageUrl.IsNullable = true;
				colvarSelectionImageUrl.IsPrimaryKey = false;
				colvarSelectionImageUrl.IsForeignKey = false;
				colvarSelectionImageUrl.IsReadOnly = false;
				colvarSelectionImageUrl.DefaultSetting = @"";
				colvarSelectionImageUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSelectionImageUrl);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("RatingFeedback",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("RatingFeedbackID")]
		[Bindable(true)]
		public int RatingFeedbackID 
		{
			get { return GetColumnValue<int>(Columns.RatingFeedbackID); }
			set { SetColumnValue(Columns.RatingFeedbackID, value); }
		}
		  
		[XmlAttribute("SelectionText")]
		[Bindable(true)]
		public string SelectionText 
		{
			get { return GetColumnValue<string>(Columns.SelectionText); }
			set { SetColumnValue(Columns.SelectionText, value); }
		}
		  
		[XmlAttribute("SelectionImage")]
		[Bindable(true)]
		public byte[] SelectionImage 
		{
			get { return GetColumnValue<byte[]>(Columns.SelectionImage); }
			set { SetColumnValue(Columns.SelectionImage, value); }
		}
		  
		[XmlAttribute("RatingType")]
		[Bindable(true)]
		public string RatingType 
		{
			get { return GetColumnValue<string>(Columns.RatingType); }
			set { SetColumnValue(Columns.RatingType, value); }
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
		  
		[XmlAttribute("SelectionImageUrl")]
		[Bindable(true)]
		public string SelectionImageUrl 
		{
			get { return GetColumnValue<string>(Columns.SelectionImageUrl); }
			set { SetColumnValue(Columns.SelectionImageUrl, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varSelectionText,byte[] varSelectionImage,string varRatingType,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varSelectionImageUrl)
		{
			RatingFeedback item = new RatingFeedback();
			
			item.SelectionText = varSelectionText;
			
			item.SelectionImage = varSelectionImage;
			
			item.RatingType = varRatingType;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.SelectionImageUrl = varSelectionImageUrl;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varRatingFeedbackID,string varSelectionText,byte[] varSelectionImage,string varRatingType,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varSelectionImageUrl)
		{
			RatingFeedback item = new RatingFeedback();
			
				item.RatingFeedbackID = varRatingFeedbackID;
			
				item.SelectionText = varSelectionText;
			
				item.SelectionImage = varSelectionImage;
			
				item.RatingType = varRatingType;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.SelectionImageUrl = varSelectionImageUrl;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn RatingFeedbackIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn SelectionTextColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn SelectionImageColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn RatingTypeColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn SelectionImageUrlColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string RatingFeedbackID = @"RatingFeedbackID";
			 public static string SelectionText = @"SelectionText";
			 public static string SelectionImage = @"SelectionImage";
			 public static string RatingType = @"RatingType";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string SelectionImageUrl = @"SelectionImageUrl";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
