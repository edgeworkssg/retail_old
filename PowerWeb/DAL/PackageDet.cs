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
	/// Strongly-typed collection for the PackageDet class.
	/// </summary>
    [Serializable]
	public partial class PackageDetCollection : ActiveList<PackageDet, PackageDetCollection>
	{	   
		public PackageDetCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PackageDetCollection</returns>
		public PackageDetCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PackageDet o = this[i];
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
	/// This is an ActiveRecord class which wraps the PackageDet table.
	/// </summary>
	[Serializable]
	public partial class PackageDet : ActiveRecord<PackageDet>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PackageDet()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PackageDet(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PackageDet(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PackageDet(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("PackageDet", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPackageDetID = new TableSchema.TableColumn(schema);
				colvarPackageDetID.ColumnName = "PackageDetID";
				colvarPackageDetID.DataType = DbType.AnsiString;
				colvarPackageDetID.MaxLength = 50;
				colvarPackageDetID.AutoIncrement = false;
				colvarPackageDetID.IsNullable = false;
				colvarPackageDetID.IsPrimaryKey = true;
				colvarPackageDetID.IsForeignKey = false;
				colvarPackageDetID.IsReadOnly = false;
				colvarPackageDetID.DefaultSetting = @"";
				colvarPackageDetID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackageDetID);
				
				TableSchema.TableColumn colvarOrderDetID = new TableSchema.TableColumn(schema);
				colvarOrderDetID.ColumnName = "OrderDetID";
				colvarOrderDetID.DataType = DbType.AnsiString;
				colvarOrderDetID.MaxLength = 18;
				colvarOrderDetID.AutoIncrement = false;
				colvarOrderDetID.IsNullable = false;
				colvarOrderDetID.IsPrimaryKey = false;
				colvarOrderDetID.IsForeignKey = true;
				colvarOrderDetID.IsReadOnly = false;
				colvarOrderDetID.DefaultSetting = @"";
				
					colvarOrderDetID.ForeignKeyTableName = "OrderDet";
				schema.Columns.Add(colvarOrderDetID);
				
				TableSchema.TableColumn colvarPackageHdrID = new TableSchema.TableColumn(schema);
				colvarPackageHdrID.ColumnName = "PackageHdrID";
				colvarPackageHdrID.DataType = DbType.AnsiString;
				colvarPackageHdrID.MaxLength = 50;
				colvarPackageHdrID.AutoIncrement = false;
				colvarPackageHdrID.IsNullable = false;
				colvarPackageHdrID.IsPrimaryKey = false;
				colvarPackageHdrID.IsForeignKey = true;
				colvarPackageHdrID.IsReadOnly = false;
				colvarPackageHdrID.DefaultSetting = @"";
				
					colvarPackageHdrID.ForeignKeyTableName = "PackageHdr";
				schema.Columns.Add(colvarPackageHdrID);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.AnsiString;
				colvarRemark.MaxLength = -1;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = false;
				colvarRemark.IsPrimaryKey = false;
				colvarRemark.IsForeignKey = false;
				colvarRemark.IsReadOnly = false;
				colvarRemark.DefaultSetting = @"";
				colvarRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemark);
				
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
				colvarDeleted.IsNullable = false;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				
						colvarDeleted.DefaultSetting = @"((0))";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("PackageDet",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PackageDetID")]
		[Bindable(true)]
		public string PackageDetID 
		{
			get { return GetColumnValue<string>(Columns.PackageDetID); }
			set { SetColumnValue(Columns.PackageDetID, value); }
		}
		  
		[XmlAttribute("OrderDetID")]
		[Bindable(true)]
		public string OrderDetID 
		{
			get { return GetColumnValue<string>(Columns.OrderDetID); }
			set { SetColumnValue(Columns.OrderDetID, value); }
		}
		  
		[XmlAttribute("PackageHdrID")]
		[Bindable(true)]
		public string PackageHdrID 
		{
			get { return GetColumnValue<string>(Columns.PackageHdrID); }
			set { SetColumnValue(Columns.PackageHdrID, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
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
		public bool Deleted 
		{
			get { return GetColumnValue<bool>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a OrderDet ActiveRecord object related to this PackageDet
		/// 
		/// </summary>
		public PowerPOS.OrderDet OrderDet
		{
			get { return PowerPOS.OrderDet.FetchByID(this.OrderDetID); }
			set { SetColumnValue("OrderDetID", value.OrderDetID); }
		}
		
		
		/// <summary>
		/// Returns a PackageHdr ActiveRecord object related to this PackageDet
		/// 
		/// </summary>
		public PowerPOS.PackageHdr PackageHdr
		{
			get { return PowerPOS.PackageHdr.FetchByID(this.PackageHdrID); }
			set { SetColumnValue("PackageHdrID", value.PackageHdrId); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varPackageDetID,string varOrderDetID,string varPackageHdrID,string varRemark,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool varDeleted)
		{
			PackageDet item = new PackageDet();
			
			item.PackageDetID = varPackageDetID;
			
			item.OrderDetID = varOrderDetID;
			
			item.PackageHdrID = varPackageHdrID;
			
			item.Remark = varRemark;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varPackageDetID,string varOrderDetID,string varPackageHdrID,string varRemark,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool varDeleted)
		{
			PackageDet item = new PackageDet();
			
				item.PackageDetID = varPackageDetID;
			
				item.OrderDetID = varOrderDetID;
			
				item.PackageHdrID = varPackageHdrID;
			
				item.Remark = varRemark;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn PackageDetIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderDetIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn PackageHdrIDColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
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
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PackageDetID = @"PackageDetID";
			 public static string OrderDetID = @"OrderDetID";
			 public static string PackageHdrID = @"PackageHdrID";
			 public static string Remark = @"Remark";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
