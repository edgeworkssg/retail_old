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
	/// Strongly-typed collection for the UOMConversionDet class.
	/// </summary>
    [Serializable]
	public partial class UOMConversionDetCollection : ActiveList<UOMConversionDet, UOMConversionDetCollection>
	{	   
		public UOMConversionDetCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>UOMConversionDetCollection</returns>
		public UOMConversionDetCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                UOMConversionDet o = this[i];
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
	/// This is an ActiveRecord class which wraps the UOMConversionDet table.
	/// </summary>
	[Serializable]
	public partial class UOMConversionDet : ActiveRecord<UOMConversionDet>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public UOMConversionDet()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public UOMConversionDet(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public UOMConversionDet(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public UOMConversionDet(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("UOMConversionDet", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarConversionDetID = new TableSchema.TableColumn(schema);
				colvarConversionDetID.ColumnName = "ConversionDetID";
				colvarConversionDetID.DataType = DbType.Int32;
				colvarConversionDetID.MaxLength = 0;
				colvarConversionDetID.AutoIncrement = true;
				colvarConversionDetID.IsNullable = false;
				colvarConversionDetID.IsPrimaryKey = true;
				colvarConversionDetID.IsForeignKey = false;
				colvarConversionDetID.IsReadOnly = false;
				colvarConversionDetID.DefaultSetting = @"";
				colvarConversionDetID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarConversionDetID);
				
				TableSchema.TableColumn colvarConversionHdrID = new TableSchema.TableColumn(schema);
				colvarConversionHdrID.ColumnName = "ConversionHdrID";
				colvarConversionHdrID.DataType = DbType.Int32;
				colvarConversionHdrID.MaxLength = 0;
				colvarConversionHdrID.AutoIncrement = false;
				colvarConversionHdrID.IsNullable = false;
				colvarConversionHdrID.IsPrimaryKey = false;
				colvarConversionHdrID.IsForeignKey = true;
				colvarConversionHdrID.IsReadOnly = false;
				colvarConversionHdrID.DefaultSetting = @"";
				
					colvarConversionHdrID.ForeignKeyTableName = "UOMConversionHdr";
				schema.Columns.Add(colvarConversionHdrID);
				
				TableSchema.TableColumn colvarFromUOM = new TableSchema.TableColumn(schema);
				colvarFromUOM.ColumnName = "FromUOM";
				colvarFromUOM.DataType = DbType.String;
				colvarFromUOM.MaxLength = 50;
				colvarFromUOM.AutoIncrement = false;
				colvarFromUOM.IsNullable = false;
				colvarFromUOM.IsPrimaryKey = false;
				colvarFromUOM.IsForeignKey = false;
				colvarFromUOM.IsReadOnly = false;
				colvarFromUOM.DefaultSetting = @"";
				colvarFromUOM.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFromUOM);
				
				TableSchema.TableColumn colvarToUOM = new TableSchema.TableColumn(schema);
				colvarToUOM.ColumnName = "ToUOM";
				colvarToUOM.DataType = DbType.String;
				colvarToUOM.MaxLength = 50;
				colvarToUOM.AutoIncrement = false;
				colvarToUOM.IsNullable = false;
				colvarToUOM.IsPrimaryKey = false;
				colvarToUOM.IsForeignKey = false;
				colvarToUOM.IsReadOnly = false;
				colvarToUOM.DefaultSetting = @"";
				colvarToUOM.ForeignKeyTableName = "";
				schema.Columns.Add(colvarToUOM);
				
				TableSchema.TableColumn colvarConversionRate = new TableSchema.TableColumn(schema);
				colvarConversionRate.ColumnName = "ConversionRate";
				colvarConversionRate.DataType = DbType.Decimal;
				colvarConversionRate.MaxLength = 0;
				colvarConversionRate.AutoIncrement = false;
				colvarConversionRate.IsNullable = false;
				colvarConversionRate.IsPrimaryKey = false;
				colvarConversionRate.IsForeignKey = false;
				colvarConversionRate.IsReadOnly = false;
				colvarConversionRate.DefaultSetting = @"";
				colvarConversionRate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarConversionRate);
				
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
				colvarDeleted.IsNullable = false;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				colvarDeleted.DefaultSetting = @"";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.String;
				colvarRemark.MaxLength = 200;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = true;
				colvarRemark.IsPrimaryKey = false;
				colvarRemark.IsForeignKey = false;
				colvarRemark.IsReadOnly = false;
				colvarRemark.DefaultSetting = @"";
				colvarRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemark);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("UOMConversionDet",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("ConversionDetID")]
		[Bindable(true)]
		public int ConversionDetID 
		{
			get { return GetColumnValue<int>(Columns.ConversionDetID); }
			set { SetColumnValue(Columns.ConversionDetID, value); }
		}
		  
		[XmlAttribute("ConversionHdrID")]
		[Bindable(true)]
		public int ConversionHdrID 
		{
			get { return GetColumnValue<int>(Columns.ConversionHdrID); }
			set { SetColumnValue(Columns.ConversionHdrID, value); }
		}
		  
		[XmlAttribute("FromUOM")]
		[Bindable(true)]
		public string FromUOM 
		{
			get { return GetColumnValue<string>(Columns.FromUOM); }
			set { SetColumnValue(Columns.FromUOM, value); }
		}
		  
		[XmlAttribute("ToUOM")]
		[Bindable(true)]
		public string ToUOM 
		{
			get { return GetColumnValue<string>(Columns.ToUOM); }
			set { SetColumnValue(Columns.ToUOM, value); }
		}
		  
		[XmlAttribute("ConversionRate")]
		[Bindable(true)]
		public decimal ConversionRate 
		{
			get { return GetColumnValue<decimal>(Columns.ConversionRate); }
			set { SetColumnValue(Columns.ConversionRate, value); }
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
		public bool Deleted 
		{
			get { return GetColumnValue<bool>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a UOMConversionHdr ActiveRecord object related to this UOMConversionDet
		/// 
		/// </summary>
		public PowerPOS.UOMConversionHdr UOMConversionHdr
		{
			get { return PowerPOS.UOMConversionHdr.FetchByID(this.ConversionHdrID); }
			set { SetColumnValue("ConversionHdrID", value.ConversionHdrID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varConversionHdrID,string varFromUOM,string varToUOM,decimal varConversionRate,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,bool varDeleted,string varRemark)
		{
			UOMConversionDet item = new UOMConversionDet();
			
			item.ConversionHdrID = varConversionHdrID;
			
			item.FromUOM = varFromUOM;
			
			item.ToUOM = varToUOM;
			
			item.ConversionRate = varConversionRate;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.Deleted = varDeleted;
			
			item.Remark = varRemark;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varConversionDetID,int varConversionHdrID,string varFromUOM,string varToUOM,decimal varConversionRate,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,bool varDeleted,string varRemark)
		{
			UOMConversionDet item = new UOMConversionDet();
			
				item.ConversionDetID = varConversionDetID;
			
				item.ConversionHdrID = varConversionHdrID;
			
				item.FromUOM = varFromUOM;
			
				item.ToUOM = varToUOM;
			
				item.ConversionRate = varConversionRate;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.Deleted = varDeleted;
			
				item.Remark = varRemark;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ConversionDetIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn ConversionHdrIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn FromUOMColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ToUOMColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ConversionRateColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ConversionDetID = @"ConversionDetID";
			 public static string ConversionHdrID = @"ConversionHdrID";
			 public static string FromUOM = @"FromUOM";
			 public static string ToUOM = @"ToUOM";
			 public static string ConversionRate = @"ConversionRate";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string Deleted = @"Deleted";
			 public static string Remark = @"Remark";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
