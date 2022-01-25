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
	/// Strongly-typed collection for the ServicingAppointment class.
	/// </summary>
    [Serializable]
	public partial class ServicingAppointmentCollection : ActiveList<ServicingAppointment, ServicingAppointmentCollection>
	{	   
		public ServicingAppointmentCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ServicingAppointmentCollection</returns>
		public ServicingAppointmentCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ServicingAppointment o = this[i];
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
	/// This is an ActiveRecord class which wraps the ServicingAppointment table.
	/// </summary>
	[Serializable]
	public partial class ServicingAppointment : ActiveRecord<ServicingAppointment>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ServicingAppointment()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ServicingAppointment(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ServicingAppointment(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ServicingAppointment(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ServicingAppointment", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarServiceRefNo = new TableSchema.TableColumn(schema);
				colvarServiceRefNo.ColumnName = "ServiceRefNo";
				colvarServiceRefNo.DataType = DbType.AnsiString;
				colvarServiceRefNo.MaxLength = 50;
				colvarServiceRefNo.AutoIncrement = false;
				colvarServiceRefNo.IsNullable = false;
				colvarServiceRefNo.IsPrimaryKey = true;
				colvarServiceRefNo.IsForeignKey = false;
				colvarServiceRefNo.IsReadOnly = false;
				colvarServiceRefNo.DefaultSetting = @"";
				colvarServiceRefNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarServiceRefNo);
				
				TableSchema.TableColumn colvarDateOfServicing = new TableSchema.TableColumn(schema);
				colvarDateOfServicing.ColumnName = "DateOfServicing";
				colvarDateOfServicing.DataType = DbType.DateTime;
				colvarDateOfServicing.MaxLength = 0;
				colvarDateOfServicing.AutoIncrement = false;
				colvarDateOfServicing.IsNullable = true;
				colvarDateOfServicing.IsPrimaryKey = false;
				colvarDateOfServicing.IsForeignKey = false;
				colvarDateOfServicing.IsReadOnly = false;
				colvarDateOfServicing.DefaultSetting = @"";
				colvarDateOfServicing.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateOfServicing);
				
				TableSchema.TableColumn colvarLocation = new TableSchema.TableColumn(schema);
				colvarLocation.ColumnName = "Location";
				colvarLocation.DataType = DbType.String;
				colvarLocation.MaxLength = 50;
				colvarLocation.AutoIncrement = false;
				colvarLocation.IsNullable = true;
				colvarLocation.IsPrimaryKey = false;
				colvarLocation.IsForeignKey = false;
				colvarLocation.IsReadOnly = false;
				colvarLocation.DefaultSetting = @"";
				colvarLocation.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLocation);
				
				TableSchema.TableColumn colvarPreferredTiming = new TableSchema.TableColumn(schema);
				colvarPreferredTiming.ColumnName = "PreferredTiming";
				colvarPreferredTiming.DataType = DbType.DateTime;
				colvarPreferredTiming.MaxLength = 0;
				colvarPreferredTiming.AutoIncrement = false;
				colvarPreferredTiming.IsNullable = true;
				colvarPreferredTiming.IsPrimaryKey = false;
				colvarPreferredTiming.IsForeignKey = false;
				colvarPreferredTiming.IsReadOnly = false;
				colvarPreferredTiming.DefaultSetting = @"";
				colvarPreferredTiming.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPreferredTiming);
				
				TableSchema.TableColumn colvarSerialNo = new TableSchema.TableColumn(schema);
				colvarSerialNo.ColumnName = "SerialNo";
				colvarSerialNo.DataType = DbType.AnsiString;
				colvarSerialNo.MaxLength = 50;
				colvarSerialNo.AutoIncrement = false;
				colvarSerialNo.IsNullable = true;
				colvarSerialNo.IsPrimaryKey = false;
				colvarSerialNo.IsForeignKey = false;
				colvarSerialNo.IsReadOnly = false;
				colvarSerialNo.DefaultSetting = @"";
				colvarSerialNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSerialNo);
				
				TableSchema.TableColumn colvarStatus = new TableSchema.TableColumn(schema);
				colvarStatus.ColumnName = "Status";
				colvarStatus.DataType = DbType.String;
				colvarStatus.MaxLength = 50;
				colvarStatus.AutoIncrement = false;
				colvarStatus.IsNullable = true;
				colvarStatus.IsPrimaryKey = false;
				colvarStatus.IsForeignKey = false;
				colvarStatus.IsReadOnly = false;
				
						colvarStatus.DefaultSetting = @"(N'Created')";
				colvarStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatus);
				
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
				colvarCreatedBy.DataType = DbType.String;
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
				colvarModifiedBy.DataType = DbType.String;
				colvarModifiedBy.MaxLength = 50;
				colvarModifiedBy.AutoIncrement = false;
				colvarModifiedBy.IsNullable = true;
				colvarModifiedBy.IsPrimaryKey = false;
				colvarModifiedBy.IsForeignKey = false;
				colvarModifiedBy.IsReadOnly = false;
				colvarModifiedBy.DefaultSetting = @"";
				colvarModifiedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedBy);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("ServicingAppointment",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("ServiceRefNo")]
		[Bindable(true)]
		public string ServiceRefNo 
		{
			get { return GetColumnValue<string>(Columns.ServiceRefNo); }
			set { SetColumnValue(Columns.ServiceRefNo, value); }
		}
		  
		[XmlAttribute("DateOfServicing")]
		[Bindable(true)]
		public DateTime? DateOfServicing 
		{
			get { return GetColumnValue<DateTime?>(Columns.DateOfServicing); }
			set { SetColumnValue(Columns.DateOfServicing, value); }
		}
		  
		[XmlAttribute("Location")]
		[Bindable(true)]
		public string Location 
		{
			get { return GetColumnValue<string>(Columns.Location); }
			set { SetColumnValue(Columns.Location, value); }
		}
		  
		[XmlAttribute("PreferredTiming")]
		[Bindable(true)]
		public DateTime? PreferredTiming 
		{
			get { return GetColumnValue<DateTime?>(Columns.PreferredTiming); }
			set { SetColumnValue(Columns.PreferredTiming, value); }
		}
		  
		[XmlAttribute("SerialNo")]
		[Bindable(true)]
		public string SerialNo 
		{
			get { return GetColumnValue<string>(Columns.SerialNo); }
			set { SetColumnValue(Columns.SerialNo, value); }
		}
		  
		[XmlAttribute("Status")]
		[Bindable(true)]
		public string Status 
		{
			get { return GetColumnValue<string>(Columns.Status); }
			set { SetColumnValue(Columns.Status, value); }
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
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varServiceRefNo,DateTime? varDateOfServicing,string varLocation,DateTime? varPreferredTiming,string varSerialNo,string varStatus,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			ServicingAppointment item = new ServicingAppointment();
			
			item.ServiceRefNo = varServiceRefNo;
			
			item.DateOfServicing = varDateOfServicing;
			
			item.Location = varLocation;
			
			item.PreferredTiming = varPreferredTiming;
			
			item.SerialNo = varSerialNo;
			
			item.Status = varStatus;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varServiceRefNo,DateTime? varDateOfServicing,string varLocation,DateTime? varPreferredTiming,string varSerialNo,string varStatus,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			ServicingAppointment item = new ServicingAppointment();
			
				item.ServiceRefNo = varServiceRefNo;
			
				item.DateOfServicing = varDateOfServicing;
			
				item.Location = varLocation;
			
				item.PreferredTiming = varPreferredTiming;
			
				item.SerialNo = varSerialNo;
			
				item.Status = varStatus;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ServiceRefNoColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn DateOfServicingColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn LocationColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn PreferredTimingColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn SerialNoColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ServiceRefNo = @"ServiceRefNo";
			 public static string DateOfServicing = @"DateOfServicing";
			 public static string Location = @"Location";
			 public static string PreferredTiming = @"PreferredTiming";
			 public static string SerialNo = @"SerialNo";
			 public static string Status = @"Status";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
