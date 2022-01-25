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
	/// Strongly-typed collection for the MembershipRemark class.
	/// </summary>
    [Serializable]
	public partial class MembershipRemarkCollection : ActiveList<MembershipRemark, MembershipRemarkCollection>
	{	   
		public MembershipRemarkCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MembershipRemarkCollection</returns>
		public MembershipRemarkCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MembershipRemark o = this[i];
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
	/// This is an ActiveRecord class which wraps the MembershipRemark table.
	/// </summary>
	[Serializable]
	public partial class MembershipRemark : ActiveRecord<MembershipRemark>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public MembershipRemark()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public MembershipRemark(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public MembershipRemark(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public MembershipRemark(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("MembershipRemark", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarMemberRemarkId = new TableSchema.TableColumn(schema);
				colvarMemberRemarkId.ColumnName = "MemberRemarkId";
				colvarMemberRemarkId.DataType = DbType.AnsiString;
				colvarMemberRemarkId.MaxLength = 14;
				colvarMemberRemarkId.AutoIncrement = false;
				colvarMemberRemarkId.IsNullable = false;
				colvarMemberRemarkId.IsPrimaryKey = true;
				colvarMemberRemarkId.IsForeignKey = false;
				colvarMemberRemarkId.IsReadOnly = false;
				colvarMemberRemarkId.DefaultSetting = @"";
				colvarMemberRemarkId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMemberRemarkId);
				
				TableSchema.TableColumn colvarRemarkDate = new TableSchema.TableColumn(schema);
				colvarRemarkDate.ColumnName = "RemarkDate";
				colvarRemarkDate.DataType = DbType.DateTime;
				colvarRemarkDate.MaxLength = 0;
				colvarRemarkDate.AutoIncrement = false;
				colvarRemarkDate.IsNullable = false;
				colvarRemarkDate.IsPrimaryKey = false;
				colvarRemarkDate.IsForeignKey = false;
				colvarRemarkDate.IsReadOnly = false;
				colvarRemarkDate.DefaultSetting = @"";
				colvarRemarkDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemarkDate);
				
				TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
				colvarMembershipNo.ColumnName = "MembershipNo";
				colvarMembershipNo.DataType = DbType.AnsiString;
				colvarMembershipNo.MaxLength = 50;
				colvarMembershipNo.AutoIncrement = false;
				colvarMembershipNo.IsNullable = false;
				colvarMembershipNo.IsPrimaryKey = false;
				colvarMembershipNo.IsForeignKey = true;
				colvarMembershipNo.IsReadOnly = false;
				colvarMembershipNo.DefaultSetting = @"";
				
					colvarMembershipNo.ForeignKeyTableName = "Membership";
				schema.Columns.Add(colvarMembershipNo);
				
				TableSchema.TableColumn colvarRemarkCategoryId = new TableSchema.TableColumn(schema);
				colvarRemarkCategoryId.ColumnName = "RemarkCategoryId";
				colvarRemarkCategoryId.DataType = DbType.Int32;
				colvarRemarkCategoryId.MaxLength = 0;
				colvarRemarkCategoryId.AutoIncrement = false;
				colvarRemarkCategoryId.IsNullable = false;
				colvarRemarkCategoryId.IsPrimaryKey = false;
				colvarRemarkCategoryId.IsForeignKey = false;
				colvarRemarkCategoryId.IsReadOnly = false;
				colvarRemarkCategoryId.DefaultSetting = @"";
				colvarRemarkCategoryId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemarkCategoryId);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.String;
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
				colvarDeleted.DefaultSetting = @"";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("MembershipRemark",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("MemberRemarkId")]
		[Bindable(true)]
		public string MemberRemarkId 
		{
			get { return GetColumnValue<string>(Columns.MemberRemarkId); }
			set { SetColumnValue(Columns.MemberRemarkId, value); }
		}
		  
		[XmlAttribute("RemarkDate")]
		[Bindable(true)]
		public DateTime RemarkDate 
		{
			get { return GetColumnValue<DateTime>(Columns.RemarkDate); }
			set { SetColumnValue(Columns.RemarkDate, value); }
		}
		  
		[XmlAttribute("MembershipNo")]
		[Bindable(true)]
		public string MembershipNo 
		{
			get { return GetColumnValue<string>(Columns.MembershipNo); }
			set { SetColumnValue(Columns.MembershipNo, value); }
		}
		  
		[XmlAttribute("RemarkCategoryId")]
		[Bindable(true)]
		public int RemarkCategoryId 
		{
			get { return GetColumnValue<int>(Columns.RemarkCategoryId); }
			set { SetColumnValue(Columns.RemarkCategoryId, value); }
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
		/// Returns a Membership ActiveRecord object related to this MembershipRemark
		/// 
		/// </summary>
		public PowerPOS.Membership Membership
		{
			get { return PowerPOS.Membership.FetchByID(this.MembershipNo); }
			set { SetColumnValue("MembershipNo", value.MembershipNo); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varMemberRemarkId,DateTime varRemarkDate,string varMembershipNo,int varRemarkCategoryId,string varRemark,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool varDeleted)
		{
			MembershipRemark item = new MembershipRemark();
			
			item.MemberRemarkId = varMemberRemarkId;
			
			item.RemarkDate = varRemarkDate;
			
			item.MembershipNo = varMembershipNo;
			
			item.RemarkCategoryId = varRemarkCategoryId;
			
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
		public static void Update(string varMemberRemarkId,DateTime varRemarkDate,string varMembershipNo,int varRemarkCategoryId,string varRemark,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool varDeleted)
		{
			MembershipRemark item = new MembershipRemark();
			
				item.MemberRemarkId = varMemberRemarkId;
			
				item.RemarkDate = varRemarkDate;
			
				item.MembershipNo = varMembershipNo;
			
				item.RemarkCategoryId = varRemarkCategoryId;
			
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
        
        
        public static TableSchema.TableColumn MemberRemarkIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkDateColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipNoColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkCategoryIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
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
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string MemberRemarkId = @"MemberRemarkId";
			 public static string RemarkDate = @"RemarkDate";
			 public static string MembershipNo = @"MembershipNo";
			 public static string RemarkCategoryId = @"RemarkCategoryId";
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
