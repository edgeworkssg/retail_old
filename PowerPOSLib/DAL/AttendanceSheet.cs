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
	/// Strongly-typed collection for the AttendanceSheet class.
	/// </summary>
    [Serializable]
	public partial class AttendanceSheetCollection : ActiveList<AttendanceSheet, AttendanceSheetCollection>
	{	   
		public AttendanceSheetCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AttendanceSheetCollection</returns>
		public AttendanceSheetCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AttendanceSheet o = this[i];
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
	/// This is an ActiveRecord class which wraps the AttendanceSheet table.
	/// </summary>
	[Serializable]
	public partial class AttendanceSheet : ActiveRecord<AttendanceSheet>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AttendanceSheet()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AttendanceSheet(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AttendanceSheet(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AttendanceSheet(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("AttendanceSheet", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarAttendanceID = new TableSchema.TableColumn(schema);
				colvarAttendanceID.ColumnName = "AttendanceID";
				colvarAttendanceID.DataType = DbType.AnsiString;
				colvarAttendanceID.MaxLength = 14;
				colvarAttendanceID.AutoIncrement = false;
				colvarAttendanceID.IsNullable = false;
				colvarAttendanceID.IsPrimaryKey = true;
				colvarAttendanceID.IsForeignKey = true;
				colvarAttendanceID.IsReadOnly = false;
				colvarAttendanceID.DefaultSetting = @"";
				
					colvarAttendanceID.ForeignKeyTableName = "AttendanceSheet";
				schema.Columns.Add(colvarAttendanceID);
				
				TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
				colvarMembershipNo.ColumnName = "MembershipNo";
				colvarMembershipNo.DataType = DbType.AnsiString;
				colvarMembershipNo.MaxLength = 50;
				colvarMembershipNo.AutoIncrement = false;
				colvarMembershipNo.IsNullable = true;
				colvarMembershipNo.IsPrimaryKey = false;
				colvarMembershipNo.IsForeignKey = false;
				colvarMembershipNo.IsReadOnly = false;
				colvarMembershipNo.DefaultSetting = @"";
				colvarMembershipNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembershipNo);
				
				TableSchema.TableColumn colvarCourseID = new TableSchema.TableColumn(schema);
				colvarCourseID.ColumnName = "CourseID";
				colvarCourseID.DataType = DbType.Int32;
				colvarCourseID.MaxLength = 0;
				colvarCourseID.AutoIncrement = false;
				colvarCourseID.IsNullable = true;
				colvarCourseID.IsPrimaryKey = false;
				colvarCourseID.IsForeignKey = true;
				colvarCourseID.IsReadOnly = false;
				colvarCourseID.DefaultSetting = @"";
				
					colvarCourseID.ForeignKeyTableName = "Course";
				schema.Columns.Add(colvarCourseID);
				
				TableSchema.TableColumn colvarDateX = new TableSchema.TableColumn(schema);
				colvarDateX.ColumnName = "Date";
				colvarDateX.DataType = DbType.DateTime;
				colvarDateX.MaxLength = 0;
				colvarDateX.AutoIncrement = false;
				colvarDateX.IsNullable = true;
				colvarDateX.IsPrimaryKey = false;
				colvarDateX.IsForeignKey = false;
				colvarDateX.IsReadOnly = false;
				colvarDateX.DefaultSetting = @"";
				colvarDateX.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateX);
				
				TableSchema.TableColumn colvarStatus = new TableSchema.TableColumn(schema);
				colvarStatus.ColumnName = "Status";
				colvarStatus.DataType = DbType.String;
				colvarStatus.MaxLength = 50;
				colvarStatus.AutoIncrement = false;
				colvarStatus.IsNullable = true;
				colvarStatus.IsPrimaryKey = false;
				colvarStatus.IsForeignKey = false;
				colvarStatus.IsReadOnly = false;
				colvarStatus.DefaultSetting = @"";
				colvarStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatus);
				
				TableSchema.TableColumn colvarReasonForLeave = new TableSchema.TableColumn(schema);
				colvarReasonForLeave.ColumnName = "ReasonForLeave";
				colvarReasonForLeave.DataType = DbType.String;
				colvarReasonForLeave.MaxLength = -1;
				colvarReasonForLeave.AutoIncrement = false;
				colvarReasonForLeave.IsNullable = true;
				colvarReasonForLeave.IsPrimaryKey = false;
				colvarReasonForLeave.IsForeignKey = false;
				colvarReasonForLeave.IsReadOnly = false;
				colvarReasonForLeave.DefaultSetting = @"";
				colvarReasonForLeave.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReasonForLeave);
				
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
				
				TableSchema.TableColumn colvarLastEditDate = new TableSchema.TableColumn(schema);
				colvarLastEditDate.ColumnName = "LastEditDate";
				colvarLastEditDate.DataType = DbType.DateTime;
				colvarLastEditDate.MaxLength = 0;
				colvarLastEditDate.AutoIncrement = false;
				colvarLastEditDate.IsNullable = true;
				colvarLastEditDate.IsPrimaryKey = false;
				colvarLastEditDate.IsForeignKey = false;
				colvarLastEditDate.IsReadOnly = false;
				
						colvarLastEditDate.DefaultSetting = @"(getutcdate())";
				colvarLastEditDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastEditDate);
				
				TableSchema.TableColumn colvarCreationDate = new TableSchema.TableColumn(schema);
				colvarCreationDate.ColumnName = "CreationDate";
				colvarCreationDate.DataType = DbType.DateTime;
				colvarCreationDate.MaxLength = 0;
				colvarCreationDate.AutoIncrement = false;
				colvarCreationDate.IsNullable = true;
				colvarCreationDate.IsPrimaryKey = false;
				colvarCreationDate.IsForeignKey = false;
				colvarCreationDate.IsReadOnly = false;
				
						colvarCreationDate.DefaultSetting = @"(getutcdate())";
				colvarCreationDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreationDate);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("AttendanceSheet",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("AttendanceID")]
		[Bindable(true)]
		public string AttendanceID 
		{
			get { return GetColumnValue<string>(Columns.AttendanceID); }
			set { SetColumnValue(Columns.AttendanceID, value); }
		}
		  
		[XmlAttribute("MembershipNo")]
		[Bindable(true)]
		public string MembershipNo 
		{
			get { return GetColumnValue<string>(Columns.MembershipNo); }
			set { SetColumnValue(Columns.MembershipNo, value); }
		}
		  
		[XmlAttribute("CourseID")]
		[Bindable(true)]
		public int? CourseID 
		{
			get { return GetColumnValue<int?>(Columns.CourseID); }
			set { SetColumnValue(Columns.CourseID, value); }
		}
		  
		[XmlAttribute("DateX")]
		[Bindable(true)]
		public DateTime? DateX 
		{
			get { return GetColumnValue<DateTime?>(Columns.DateX); }
			set { SetColumnValue(Columns.DateX, value); }
		}
		  
		[XmlAttribute("Status")]
		[Bindable(true)]
		public string Status 
		{
			get { return GetColumnValue<string>(Columns.Status); }
			set { SetColumnValue(Columns.Status, value); }
		}
		  
		[XmlAttribute("ReasonForLeave")]
		[Bindable(true)]
		public string ReasonForLeave 
		{
			get { return GetColumnValue<string>(Columns.ReasonForLeave); }
			set { SetColumnValue(Columns.ReasonForLeave, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
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
		  
		[XmlAttribute("LastEditDate")]
		[Bindable(true)]
		public DateTime? LastEditDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.LastEditDate); }
			set { SetColumnValue(Columns.LastEditDate, value); }
		}
		  
		[XmlAttribute("CreationDate")]
		[Bindable(true)]
		public DateTime? CreationDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreationDate); }
			set { SetColumnValue(Columns.CreationDate, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		public PowerPOS.AttendanceSheetCollection ChildAttendanceSheetRecords()
		{
			return new PowerPOS.AttendanceSheetCollection().Where(AttendanceSheet.Columns.AttendanceID, AttendanceID).Load();
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Course ActiveRecord object related to this AttendanceSheet
		/// 
		/// </summary>
		public PowerPOS.Course Course
		{
			get { return PowerPOS.Course.FetchByID(this.CourseID); }
			set { SetColumnValue("CourseID", value.Id); }
		}
		
		
		/// <summary>
		/// Returns a AttendanceSheet ActiveRecord object related to this AttendanceSheet
		/// 
		/// </summary>
		public PowerPOS.AttendanceSheet ParentAttendanceSheet
		{
			get { return PowerPOS.AttendanceSheet.FetchByID(this.AttendanceID); }
			set { SetColumnValue("AttendanceID", value.AttendanceID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varAttendanceID,string varMembershipNo,int? varCourseID,DateTime? varDateX,string varStatus,string varReasonForLeave,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted,DateTime? varLastEditDate,DateTime? varCreationDate)
		{
			AttendanceSheet item = new AttendanceSheet();
			
			item.AttendanceID = varAttendanceID;
			
			item.MembershipNo = varMembershipNo;
			
			item.CourseID = varCourseID;
			
			item.DateX = varDateX;
			
			item.Status = varStatus;
			
			item.ReasonForLeave = varReasonForLeave;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedOn = varModifiedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.LastEditDate = varLastEditDate;
			
			item.CreationDate = varCreationDate;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varAttendanceID,string varMembershipNo,int? varCourseID,DateTime? varDateX,string varStatus,string varReasonForLeave,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted,DateTime? varLastEditDate,DateTime? varCreationDate)
		{
			AttendanceSheet item = new AttendanceSheet();
			
				item.AttendanceID = varAttendanceID;
			
				item.MembershipNo = varMembershipNo;
			
				item.CourseID = varCourseID;
			
				item.DateX = varDateX;
			
				item.Status = varStatus;
			
				item.ReasonForLeave = varReasonForLeave;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedOn = varModifiedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.LastEditDate = varLastEditDate;
			
				item.CreationDate = varCreationDate;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn AttendanceIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn CourseIDColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DateXColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ReasonForLeaveColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn LastEditDateColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn CreationDateColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string AttendanceID = @"AttendanceID";
			 public static string MembershipNo = @"MembershipNo";
			 public static string CourseID = @"CourseID";
			 public static string DateX = @"Date";
			 public static string Status = @"Status";
			 public static string ReasonForLeave = @"ReasonForLeave";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string LastEditDate = @"LastEditDate";
			 public static string CreationDate = @"CreationDate";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
}
        #endregion
	}
}
