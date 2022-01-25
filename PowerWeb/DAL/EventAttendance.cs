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
	/// Strongly-typed collection for the EventAttendance class.
	/// </summary>
    [Serializable]
	public partial class EventAttendanceCollection : ActiveList<EventAttendance, EventAttendanceCollection>
	{	   
		public EventAttendanceCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>EventAttendanceCollection</returns>
		public EventAttendanceCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                EventAttendance o = this[i];
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
	/// This is an ActiveRecord class which wraps the EventAttendance table.
	/// </summary>
	[Serializable]
	public partial class EventAttendance : ActiveRecord<EventAttendance>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public EventAttendance()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public EventAttendance(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public EventAttendance(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public EventAttendance(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("EventAttendance", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarEventAttendanceID = new TableSchema.TableColumn(schema);
				colvarEventAttendanceID.ColumnName = "EventAttendanceID";
				colvarEventAttendanceID.DataType = DbType.Int32;
				colvarEventAttendanceID.MaxLength = 0;
				colvarEventAttendanceID.AutoIncrement = false;
				colvarEventAttendanceID.IsNullable = false;
				colvarEventAttendanceID.IsPrimaryKey = true;
				colvarEventAttendanceID.IsForeignKey = false;
				colvarEventAttendanceID.IsReadOnly = false;
				colvarEventAttendanceID.DefaultSetting = @"";
				colvarEventAttendanceID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEventAttendanceID);
				
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
				
				TableSchema.TableColumn colvarSpecialEventId = new TableSchema.TableColumn(schema);
				colvarSpecialEventId.ColumnName = "SpecialEventId";
				colvarSpecialEventId.DataType = DbType.Int32;
				colvarSpecialEventId.MaxLength = 0;
				colvarSpecialEventId.AutoIncrement = false;
				colvarSpecialEventId.IsNullable = false;
				colvarSpecialEventId.IsPrimaryKey = false;
				colvarSpecialEventId.IsForeignKey = true;
				colvarSpecialEventId.IsReadOnly = false;
				colvarSpecialEventId.DefaultSetting = @"";
				
					colvarSpecialEventId.ForeignKeyTableName = "SpecialEvent";
				schema.Columns.Add(colvarSpecialEventId);
				
				TableSchema.TableColumn colvarArrivalDate = new TableSchema.TableColumn(schema);
				colvarArrivalDate.ColumnName = "ArrivalDate";
				colvarArrivalDate.DataType = DbType.DateTime;
				colvarArrivalDate.MaxLength = 0;
				colvarArrivalDate.AutoIncrement = false;
				colvarArrivalDate.IsNullable = false;
				colvarArrivalDate.IsPrimaryKey = false;
				colvarArrivalDate.IsForeignKey = false;
				colvarArrivalDate.IsReadOnly = false;
				colvarArrivalDate.DefaultSetting = @"";
				colvarArrivalDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarArrivalDate);
				
				TableSchema.TableColumn colvarMovementType = new TableSchema.TableColumn(schema);
				colvarMovementType.ColumnName = "MovementType";
				colvarMovementType.DataType = DbType.AnsiString;
				colvarMovementType.MaxLength = 50;
				colvarMovementType.AutoIncrement = false;
				colvarMovementType.IsNullable = false;
				colvarMovementType.IsPrimaryKey = false;
				colvarMovementType.IsForeignKey = false;
				colvarMovementType.IsReadOnly = false;
				colvarMovementType.DefaultSetting = @"";
				colvarMovementType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMovementType);
				
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
				colvarCreatedBy.DataType = DbType.DateTime;
				colvarCreatedBy.MaxLength = 0;
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
				colvarModifiedBy.DataType = DbType.DateTime;
				colvarModifiedBy.MaxLength = 0;
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
				
				TableSchema.TableColumn colvarUserFld1 = new TableSchema.TableColumn(schema);
				colvarUserFld1.ColumnName = "UserFld1";
				colvarUserFld1.DataType = DbType.AnsiString;
				colvarUserFld1.MaxLength = 50;
				colvarUserFld1.AutoIncrement = false;
				colvarUserFld1.IsNullable = true;
				colvarUserFld1.IsPrimaryKey = false;
				colvarUserFld1.IsForeignKey = false;
				colvarUserFld1.IsReadOnly = false;
				colvarUserFld1.DefaultSetting = @"";
				colvarUserFld1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld1);
				
				TableSchema.TableColumn colvarUserFld2 = new TableSchema.TableColumn(schema);
				colvarUserFld2.ColumnName = "UserFld2";
				colvarUserFld2.DataType = DbType.AnsiString;
				colvarUserFld2.MaxLength = 50;
				colvarUserFld2.AutoIncrement = false;
				colvarUserFld2.IsNullable = true;
				colvarUserFld2.IsPrimaryKey = false;
				colvarUserFld2.IsForeignKey = false;
				colvarUserFld2.IsReadOnly = false;
				colvarUserFld2.DefaultSetting = @"";
				colvarUserFld2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld2);
				
				TableSchema.TableColumn colvarUserFld3 = new TableSchema.TableColumn(schema);
				colvarUserFld3.ColumnName = "UserFld3";
				colvarUserFld3.DataType = DbType.AnsiString;
				colvarUserFld3.MaxLength = 50;
				colvarUserFld3.AutoIncrement = false;
				colvarUserFld3.IsNullable = true;
				colvarUserFld3.IsPrimaryKey = false;
				colvarUserFld3.IsForeignKey = false;
				colvarUserFld3.IsReadOnly = false;
				colvarUserFld3.DefaultSetting = @"";
				colvarUserFld3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld3);
				
				TableSchema.TableColumn colvarUserFld4 = new TableSchema.TableColumn(schema);
				colvarUserFld4.ColumnName = "UserFld4";
				colvarUserFld4.DataType = DbType.AnsiString;
				colvarUserFld4.MaxLength = 50;
				colvarUserFld4.AutoIncrement = false;
				colvarUserFld4.IsNullable = true;
				colvarUserFld4.IsPrimaryKey = false;
				colvarUserFld4.IsForeignKey = false;
				colvarUserFld4.IsReadOnly = false;
				colvarUserFld4.DefaultSetting = @"";
				colvarUserFld4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld4);
				
				TableSchema.TableColumn colvarUserFld5 = new TableSchema.TableColumn(schema);
				colvarUserFld5.ColumnName = "UserFld5";
				colvarUserFld5.DataType = DbType.AnsiString;
				colvarUserFld5.MaxLength = 50;
				colvarUserFld5.AutoIncrement = false;
				colvarUserFld5.IsNullable = true;
				colvarUserFld5.IsPrimaryKey = false;
				colvarUserFld5.IsForeignKey = false;
				colvarUserFld5.IsReadOnly = false;
				colvarUserFld5.DefaultSetting = @"";
				colvarUserFld5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld5);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("EventAttendance",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("EventAttendanceID")]
		[Bindable(true)]
		public int EventAttendanceID 
		{
			get { return GetColumnValue<int>(Columns.EventAttendanceID); }
			set { SetColumnValue(Columns.EventAttendanceID, value); }
		}
		  
		[XmlAttribute("MembershipNo")]
		[Bindable(true)]
		public string MembershipNo 
		{
			get { return GetColumnValue<string>(Columns.MembershipNo); }
			set { SetColumnValue(Columns.MembershipNo, value); }
		}
		  
		[XmlAttribute("SpecialEventId")]
		[Bindable(true)]
		public int SpecialEventId 
		{
			get { return GetColumnValue<int>(Columns.SpecialEventId); }
			set { SetColumnValue(Columns.SpecialEventId, value); }
		}
		  
		[XmlAttribute("ArrivalDate")]
		[Bindable(true)]
		public DateTime ArrivalDate 
		{
			get { return GetColumnValue<DateTime>(Columns.ArrivalDate); }
			set { SetColumnValue(Columns.ArrivalDate, value); }
		}
		  
		[XmlAttribute("MovementType")]
		[Bindable(true)]
		public string MovementType 
		{
			get { return GetColumnValue<string>(Columns.MovementType); }
			set { SetColumnValue(Columns.MovementType, value); }
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
		public DateTime? CreatedBy 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedBy); }
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
		public DateTime? ModifiedBy 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("UserFld1")]
		[Bindable(true)]
		public string UserFld1 
		{
			get { return GetColumnValue<string>(Columns.UserFld1); }
			set { SetColumnValue(Columns.UserFld1, value); }
		}
		  
		[XmlAttribute("UserFld2")]
		[Bindable(true)]
		public string UserFld2 
		{
			get { return GetColumnValue<string>(Columns.UserFld2); }
			set { SetColumnValue(Columns.UserFld2, value); }
		}
		  
		[XmlAttribute("UserFld3")]
		[Bindable(true)]
		public string UserFld3 
		{
			get { return GetColumnValue<string>(Columns.UserFld3); }
			set { SetColumnValue(Columns.UserFld3, value); }
		}
		  
		[XmlAttribute("UserFld4")]
		[Bindable(true)]
		public string UserFld4 
		{
			get { return GetColumnValue<string>(Columns.UserFld4); }
			set { SetColumnValue(Columns.UserFld4, value); }
		}
		  
		[XmlAttribute("UserFld5")]
		[Bindable(true)]
		public string UserFld5 
		{
			get { return GetColumnValue<string>(Columns.UserFld5); }
			set { SetColumnValue(Columns.UserFld5, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Membership ActiveRecord object related to this EventAttendance
		/// 
		/// </summary>
		public PowerPOS.Membership Membership
		{
			get { return PowerPOS.Membership.FetchByID(this.MembershipNo); }
			set { SetColumnValue("MembershipNo", value.MembershipNo); }
		}
		
		
		/// <summary>
		/// Returns a SpecialEvent ActiveRecord object related to this EventAttendance
		/// 
		/// </summary>
		public PowerPOS.SpecialEvent SpecialEvent
		{
			get { return PowerPOS.SpecialEvent.FetchByID(this.SpecialEventId); }
			set { SetColumnValue("SpecialEventId", value.EventId); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varEventAttendanceID,string varMembershipNo,int varSpecialEventId,DateTime varArrivalDate,string varMovementType,DateTime? varCreatedOn,DateTime? varCreatedBy,DateTime? varModifiedOn,DateTime? varModifiedBy,bool? varDeleted,string varUserFld1,string varUserFld2,string varUserFld3,string varUserFld4,string varUserFld5)
		{
			EventAttendance item = new EventAttendance();
			
			item.EventAttendanceID = varEventAttendanceID;
			
			item.MembershipNo = varMembershipNo;
			
			item.SpecialEventId = varSpecialEventId;
			
			item.ArrivalDate = varArrivalDate;
			
			item.MovementType = varMovementType;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.UserFld1 = varUserFld1;
			
			item.UserFld2 = varUserFld2;
			
			item.UserFld3 = varUserFld3;
			
			item.UserFld4 = varUserFld4;
			
			item.UserFld5 = varUserFld5;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varEventAttendanceID,string varMembershipNo,int varSpecialEventId,DateTime varArrivalDate,string varMovementType,DateTime? varCreatedOn,DateTime? varCreatedBy,DateTime? varModifiedOn,DateTime? varModifiedBy,bool? varDeleted,string varUserFld1,string varUserFld2,string varUserFld3,string varUserFld4,string varUserFld5)
		{
			EventAttendance item = new EventAttendance();
			
				item.EventAttendanceID = varEventAttendanceID;
			
				item.MembershipNo = varMembershipNo;
			
				item.SpecialEventId = varSpecialEventId;
			
				item.ArrivalDate = varArrivalDate;
			
				item.MovementType = varMovementType;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.UserFld1 = varUserFld1;
			
				item.UserFld2 = varUserFld2;
			
				item.UserFld3 = varUserFld3;
			
				item.UserFld4 = varUserFld4;
			
				item.UserFld5 = varUserFld5;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn EventAttendanceIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn SpecialEventIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ArrivalDateColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn MovementTypeColumn
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
        
        
        
        public static TableSchema.TableColumn UserFld1Column
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld2Column
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld3Column
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld4Column
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld5Column
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string EventAttendanceID = @"EventAttendanceID";
			 public static string MembershipNo = @"MembershipNo";
			 public static string SpecialEventId = @"SpecialEventId";
			 public static string ArrivalDate = @"ArrivalDate";
			 public static string MovementType = @"MovementType";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string UserFld1 = @"UserFld1";
			 public static string UserFld2 = @"UserFld2";
			 public static string UserFld3 = @"UserFld3";
			 public static string UserFld4 = @"UserFld4";
			 public static string UserFld5 = @"UserFld5";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
