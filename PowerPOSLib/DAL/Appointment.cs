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
	/// Strongly-typed collection for the Appointment class.
	/// </summary>
    [Serializable]
	public partial class AppointmentCollection : ActiveList<Appointment, AppointmentCollection>
	{	   
		public AppointmentCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AppointmentCollection</returns>
		public AppointmentCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Appointment o = this[i];
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
	/// This is an ActiveRecord class which wraps the Appointment table.
	/// </summary>
	[Serializable]
	public partial class Appointment : ActiveRecord<Appointment>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Appointment()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Appointment(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Appointment(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Appointment(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Appointment", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "Id";
				colvarId.DataType = DbType.Guid;
				colvarId.MaxLength = 0;
				colvarId.AutoIncrement = false;
				colvarId.IsNullable = false;
				colvarId.IsPrimaryKey = true;
				colvarId.IsForeignKey = false;
				colvarId.IsReadOnly = false;
				
						colvarId.DefaultSetting = @"(newid())";
				colvarId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarId);
				
				TableSchema.TableColumn colvarStartTime = new TableSchema.TableColumn(schema);
				colvarStartTime.ColumnName = "StartTime";
				colvarStartTime.DataType = DbType.DateTime;
				colvarStartTime.MaxLength = 0;
				colvarStartTime.AutoIncrement = false;
				colvarStartTime.IsNullable = false;
				colvarStartTime.IsPrimaryKey = false;
				colvarStartTime.IsForeignKey = false;
				colvarStartTime.IsReadOnly = false;
				colvarStartTime.DefaultSetting = @"";
				colvarStartTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStartTime);
				
				TableSchema.TableColumn colvarDuration = new TableSchema.TableColumn(schema);
				colvarDuration.ColumnName = "Duration";
				colvarDuration.DataType = DbType.Int32;
				colvarDuration.MaxLength = 0;
				colvarDuration.AutoIncrement = false;
				colvarDuration.IsNullable = false;
				colvarDuration.IsPrimaryKey = false;
				colvarDuration.IsForeignKey = false;
				colvarDuration.IsReadOnly = false;
				colvarDuration.DefaultSetting = @"";
				colvarDuration.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDuration);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 500;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarBackColor = new TableSchema.TableColumn(schema);
				colvarBackColor.ColumnName = "BackColor";
				colvarBackColor.DataType = DbType.Int32;
				colvarBackColor.MaxLength = 0;
				colvarBackColor.AutoIncrement = false;
				colvarBackColor.IsNullable = false;
				colvarBackColor.IsPrimaryKey = false;
				colvarBackColor.IsForeignKey = false;
				colvarBackColor.IsReadOnly = false;
				
						colvarBackColor.DefaultSetting = @"((-2127584))";
				colvarBackColor.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBackColor);
				
				TableSchema.TableColumn colvarFontColor = new TableSchema.TableColumn(schema);
				colvarFontColor.ColumnName = "FontColor";
				colvarFontColor.DataType = DbType.Int32;
				colvarFontColor.MaxLength = 0;
				colvarFontColor.AutoIncrement = false;
				colvarFontColor.IsNullable = false;
				colvarFontColor.IsPrimaryKey = false;
				colvarFontColor.IsForeignKey = false;
				colvarFontColor.IsReadOnly = false;
				
						colvarFontColor.DefaultSetting = @"((-1))";
				colvarFontColor.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFontColor);
				
				TableSchema.TableColumn colvarSalesPersonID = new TableSchema.TableColumn(schema);
				colvarSalesPersonID.ColumnName = "SalesPersonID";
				colvarSalesPersonID.DataType = DbType.AnsiString;
				colvarSalesPersonID.MaxLength = 50;
				colvarSalesPersonID.AutoIncrement = false;
				colvarSalesPersonID.IsNullable = false;
				colvarSalesPersonID.IsPrimaryKey = false;
				colvarSalesPersonID.IsForeignKey = true;
				colvarSalesPersonID.IsReadOnly = false;
				colvarSalesPersonID.DefaultSetting = @"";
				
					colvarSalesPersonID.ForeignKeyTableName = "UserMst";
				schema.Columns.Add(colvarSalesPersonID);
				
				TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
				colvarMembershipNo.ColumnName = "MembershipNo";
				colvarMembershipNo.DataType = DbType.AnsiString;
				colvarMembershipNo.MaxLength = 50;
				colvarMembershipNo.AutoIncrement = false;
				colvarMembershipNo.IsNullable = true;
				colvarMembershipNo.IsPrimaryKey = false;
				colvarMembershipNo.IsForeignKey = true;
				colvarMembershipNo.IsReadOnly = false;
				colvarMembershipNo.DefaultSetting = @"";
				
					colvarMembershipNo.ForeignKeyTableName = "Membership";
				schema.Columns.Add(colvarMembershipNo);
				
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
				
				TableSchema.TableColumn colvarOrganization = new TableSchema.TableColumn(schema);
				colvarOrganization.ColumnName = "Organization";
				colvarOrganization.DataType = DbType.String;
				colvarOrganization.MaxLength = 200;
				colvarOrganization.AutoIncrement = false;
				colvarOrganization.IsNullable = true;
				colvarOrganization.IsPrimaryKey = false;
				colvarOrganization.IsForeignKey = false;
				colvarOrganization.IsReadOnly = false;
				colvarOrganization.DefaultSetting = @"";
				colvarOrganization.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrganization);
				
				TableSchema.TableColumn colvarPickUpLocation = new TableSchema.TableColumn(schema);
				colvarPickUpLocation.ColumnName = "PickUpLocation";
				colvarPickUpLocation.DataType = DbType.String;
				colvarPickUpLocation.MaxLength = 200;
				colvarPickUpLocation.AutoIncrement = false;
				colvarPickUpLocation.IsNullable = true;
				colvarPickUpLocation.IsPrimaryKey = false;
				colvarPickUpLocation.IsForeignKey = false;
				colvarPickUpLocation.IsReadOnly = false;
				colvarPickUpLocation.DefaultSetting = @"";
				colvarPickUpLocation.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPickUpLocation);
				
				TableSchema.TableColumn colvarNoOfChildren = new TableSchema.TableColumn(schema);
				colvarNoOfChildren.ColumnName = "NoOfChildren";
				colvarNoOfChildren.DataType = DbType.Int32;
				colvarNoOfChildren.MaxLength = 0;
				colvarNoOfChildren.AutoIncrement = false;
				colvarNoOfChildren.IsNullable = true;
				colvarNoOfChildren.IsPrimaryKey = false;
				colvarNoOfChildren.IsForeignKey = false;
				colvarNoOfChildren.IsReadOnly = false;
				colvarNoOfChildren.DefaultSetting = @"";
				colvarNoOfChildren.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNoOfChildren);
				
				TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
				colvarPointOfSaleID.ColumnName = "PointOfSaleID";
				colvarPointOfSaleID.DataType = DbType.Int32;
				colvarPointOfSaleID.MaxLength = 0;
				colvarPointOfSaleID.AutoIncrement = false;
				colvarPointOfSaleID.IsNullable = true;
				colvarPointOfSaleID.IsPrimaryKey = false;
				colvarPointOfSaleID.IsForeignKey = false;
				colvarPointOfSaleID.IsReadOnly = false;
				colvarPointOfSaleID.DefaultSetting = @"";
				colvarPointOfSaleID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointOfSaleID);
				
				TableSchema.TableColumn colvarResourceID = new TableSchema.TableColumn(schema);
				colvarResourceID.ColumnName = "ResourceID";
				colvarResourceID.DataType = DbType.AnsiString;
				colvarResourceID.MaxLength = 50;
				colvarResourceID.AutoIncrement = false;
				colvarResourceID.IsNullable = true;
				colvarResourceID.IsPrimaryKey = false;
				colvarResourceID.IsForeignKey = false;
				colvarResourceID.IsReadOnly = false;
				colvarResourceID.DefaultSetting = @"";
				colvarResourceID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarResourceID);
				
				TableSchema.TableColumn colvarCheckInByWho = new TableSchema.TableColumn(schema);
				colvarCheckInByWho.ColumnName = "CheckInByWho";
				colvarCheckInByWho.DataType = DbType.String;
				colvarCheckInByWho.MaxLength = 50;
				colvarCheckInByWho.AutoIncrement = false;
				colvarCheckInByWho.IsNullable = true;
				colvarCheckInByWho.IsPrimaryKey = false;
				colvarCheckInByWho.IsForeignKey = false;
				colvarCheckInByWho.IsReadOnly = false;
				colvarCheckInByWho.DefaultSetting = @"";
				colvarCheckInByWho.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCheckInByWho);
				
				TableSchema.TableColumn colvarCheckOutByWho = new TableSchema.TableColumn(schema);
				colvarCheckOutByWho.ColumnName = "CheckOutByWho";
				colvarCheckOutByWho.DataType = DbType.String;
				colvarCheckOutByWho.MaxLength = 50;
				colvarCheckOutByWho.AutoIncrement = false;
				colvarCheckOutByWho.IsNullable = true;
				colvarCheckOutByWho.IsPrimaryKey = false;
				colvarCheckOutByWho.IsForeignKey = false;
				colvarCheckOutByWho.IsReadOnly = false;
				colvarCheckOutByWho.DefaultSetting = @"";
				colvarCheckOutByWho.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCheckOutByWho);
				
				TableSchema.TableColumn colvarCheckInTime = new TableSchema.TableColumn(schema);
				colvarCheckInTime.ColumnName = "CheckInTime";
				colvarCheckInTime.DataType = DbType.DateTime;
				colvarCheckInTime.MaxLength = 0;
				colvarCheckInTime.AutoIncrement = false;
				colvarCheckInTime.IsNullable = true;
				colvarCheckInTime.IsPrimaryKey = false;
				colvarCheckInTime.IsForeignKey = false;
				colvarCheckInTime.IsReadOnly = false;
				colvarCheckInTime.DefaultSetting = @"";
				colvarCheckInTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCheckInTime);
				
				TableSchema.TableColumn colvarCheckOutTime = new TableSchema.TableColumn(schema);
				colvarCheckOutTime.ColumnName = "CheckOutTime";
				colvarCheckOutTime.DataType = DbType.DateTime;
				colvarCheckOutTime.MaxLength = 0;
				colvarCheckOutTime.AutoIncrement = false;
				colvarCheckOutTime.IsNullable = true;
				colvarCheckOutTime.IsPrimaryKey = false;
				colvarCheckOutTime.IsForeignKey = false;
				colvarCheckOutTime.IsReadOnly = false;
				colvarCheckOutTime.DefaultSetting = @"";
				colvarCheckOutTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCheckOutTime);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.String;
				colvarRemark.MaxLength = -1;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = true;
				colvarRemark.IsPrimaryKey = false;
				colvarRemark.IsForeignKey = false;
				colvarRemark.IsReadOnly = false;
				colvarRemark.DefaultSetting = @"";
				colvarRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemark);
				
				TableSchema.TableColumn colvarIsServerUpdate = new TableSchema.TableColumn(schema);
				colvarIsServerUpdate.ColumnName = "IsServerUpdate";
				colvarIsServerUpdate.DataType = DbType.Boolean;
				colvarIsServerUpdate.MaxLength = 0;
				colvarIsServerUpdate.AutoIncrement = false;
				colvarIsServerUpdate.IsNullable = true;
				colvarIsServerUpdate.IsPrimaryKey = false;
				colvarIsServerUpdate.IsForeignKey = false;
				colvarIsServerUpdate.IsReadOnly = false;
				
						colvarIsServerUpdate.DefaultSetting = @"((0))";
				colvarIsServerUpdate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsServerUpdate);
				
				TableSchema.TableColumn colvarTimeExtension = new TableSchema.TableColumn(schema);
				colvarTimeExtension.ColumnName = "TimeExtension";
				colvarTimeExtension.DataType = DbType.Int32;
				colvarTimeExtension.MaxLength = 0;
				colvarTimeExtension.AutoIncrement = false;
				colvarTimeExtension.IsNullable = true;
				colvarTimeExtension.IsPrimaryKey = false;
				colvarTimeExtension.IsForeignKey = false;
				colvarTimeExtension.IsReadOnly = false;
				colvarTimeExtension.DefaultSetting = @"";
				colvarTimeExtension.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTimeExtension);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Appointment",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Id")]
		[Bindable(true)]
		public Guid Id 
		{
			get { return GetColumnValue<Guid>(Columns.Id); }
			set { SetColumnValue(Columns.Id, value); }
		}
		  
		[XmlAttribute("StartTime")]
		[Bindable(true)]
		public DateTime StartTime 
		{
			get { return GetColumnValue<DateTime>(Columns.StartTime); }
			set { SetColumnValue(Columns.StartTime, value); }
		}
		  
		[XmlAttribute("Duration")]
		[Bindable(true)]
		public int Duration 
		{
			get { return GetColumnValue<int>(Columns.Duration); }
			set { SetColumnValue(Columns.Duration, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("BackColor")]
		[Bindable(true)]
		public int BackColor 
		{
			get { return GetColumnValue<int>(Columns.BackColor); }
			set { SetColumnValue(Columns.BackColor, value); }
		}
		  
		[XmlAttribute("FontColor")]
		[Bindable(true)]
		public int FontColor 
		{
			get { return GetColumnValue<int>(Columns.FontColor); }
			set { SetColumnValue(Columns.FontColor, value); }
		}
		  
		[XmlAttribute("SalesPersonID")]
		[Bindable(true)]
		public string SalesPersonID 
		{
			get { return GetColumnValue<string>(Columns.SalesPersonID); }
			set { SetColumnValue(Columns.SalesPersonID, value); }
		}
		  
		[XmlAttribute("MembershipNo")]
		[Bindable(true)]
		public string MembershipNo 
		{
			get { return GetColumnValue<string>(Columns.MembershipNo); }
			set { SetColumnValue(Columns.MembershipNo, value); }
		}
		  
		[XmlAttribute("OrderHdrID")]
		[Bindable(true)]
		public string OrderHdrID 
		{
			get { return GetColumnValue<string>(Columns.OrderHdrID); }
			set { SetColumnValue(Columns.OrderHdrID, value); }
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
		  
		[XmlAttribute("Organization")]
		[Bindable(true)]
		public string Organization 
		{
			get { return GetColumnValue<string>(Columns.Organization); }
			set { SetColumnValue(Columns.Organization, value); }
		}
		  
		[XmlAttribute("PickUpLocation")]
		[Bindable(true)]
		public string PickUpLocation 
		{
			get { return GetColumnValue<string>(Columns.PickUpLocation); }
			set { SetColumnValue(Columns.PickUpLocation, value); }
		}
		  
		[XmlAttribute("NoOfChildren")]
		[Bindable(true)]
		public int? NoOfChildren 
		{
			get { return GetColumnValue<int?>(Columns.NoOfChildren); }
			set { SetColumnValue(Columns.NoOfChildren, value); }
		}
		  
		[XmlAttribute("PointOfSaleID")]
		[Bindable(true)]
		public int? PointOfSaleID 
		{
			get { return GetColumnValue<int?>(Columns.PointOfSaleID); }
			set { SetColumnValue(Columns.PointOfSaleID, value); }
		}
		  
		[XmlAttribute("ResourceID")]
		[Bindable(true)]
		public string ResourceID 
		{
			get { return GetColumnValue<string>(Columns.ResourceID); }
			set { SetColumnValue(Columns.ResourceID, value); }
		}
		  
		[XmlAttribute("CheckInByWho")]
		[Bindable(true)]
		public string CheckInByWho 
		{
			get { return GetColumnValue<string>(Columns.CheckInByWho); }
			set { SetColumnValue(Columns.CheckInByWho, value); }
		}
		  
		[XmlAttribute("CheckOutByWho")]
		[Bindable(true)]
		public string CheckOutByWho 
		{
			get { return GetColumnValue<string>(Columns.CheckOutByWho); }
			set { SetColumnValue(Columns.CheckOutByWho, value); }
		}
		  
		[XmlAttribute("CheckInTime")]
		[Bindable(true)]
		public DateTime? CheckInTime 
		{
			get { return GetColumnValue<DateTime?>(Columns.CheckInTime); }
			set { SetColumnValue(Columns.CheckInTime, value); }
		}
		  
		[XmlAttribute("CheckOutTime")]
		[Bindable(true)]
		public DateTime? CheckOutTime 
		{
			get { return GetColumnValue<DateTime?>(Columns.CheckOutTime); }
			set { SetColumnValue(Columns.CheckOutTime, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
		}
		  
		[XmlAttribute("IsServerUpdate")]
		[Bindable(true)]
		public bool? IsServerUpdate 
		{
			get { return GetColumnValue<bool?>(Columns.IsServerUpdate); }
			set { SetColumnValue(Columns.IsServerUpdate, value); }
		}
		  
		[XmlAttribute("TimeExtension")]
		[Bindable(true)]
		public int? TimeExtension 
		{
			get { return GetColumnValue<int?>(Columns.TimeExtension); }
			set { SetColumnValue(Columns.TimeExtension, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		public PowerPOS.AppointmentItemCollection AppointmentItemRecords()
		{
			return new PowerPOS.AppointmentItemCollection().Where(AppointmentItem.Columns.AppointmentId, Id).Load();
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a UserMst ActiveRecord object related to this Appointment
		/// 
		/// </summary>
		public PowerPOS.UserMst UserMst
		{
			get { return PowerPOS.UserMst.FetchByID(this.SalesPersonID); }
			set { SetColumnValue("SalesPersonID", value.UserName); }
		}
		
		
		/// <summary>
		/// Returns a Membership ActiveRecord object related to this Appointment
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
		public static void Insert(Guid varId,DateTime varStartTime,int varDuration,string varDescription,int varBackColor,int varFontColor,string varSalesPersonID,string varMembershipNo,string varOrderHdrID,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,bool? varDeleted,string varOrganization,string varPickUpLocation,int? varNoOfChildren,int? varPointOfSaleID,string varResourceID,string varCheckInByWho,string varCheckOutByWho,DateTime? varCheckInTime,DateTime? varCheckOutTime,string varRemark,bool? varIsServerUpdate,int? varTimeExtension)
		{
			Appointment item = new Appointment();
			
			item.Id = varId;
			
			item.StartTime = varStartTime;
			
			item.Duration = varDuration;
			
			item.Description = varDescription;
			
			item.BackColor = varBackColor;
			
			item.FontColor = varFontColor;
			
			item.SalesPersonID = varSalesPersonID;
			
			item.MembershipNo = varMembershipNo;
			
			item.OrderHdrID = varOrderHdrID;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.Deleted = varDeleted;
			
			item.Organization = varOrganization;
			
			item.PickUpLocation = varPickUpLocation;
			
			item.NoOfChildren = varNoOfChildren;
			
			item.PointOfSaleID = varPointOfSaleID;
			
			item.ResourceID = varResourceID;
			
			item.CheckInByWho = varCheckInByWho;
			
			item.CheckOutByWho = varCheckOutByWho;
			
			item.CheckInTime = varCheckInTime;
			
			item.CheckOutTime = varCheckOutTime;
			
			item.Remark = varRemark;
			
			item.IsServerUpdate = varIsServerUpdate;
			
			item.TimeExtension = varTimeExtension;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varId,DateTime varStartTime,int varDuration,string varDescription,int varBackColor,int varFontColor,string varSalesPersonID,string varMembershipNo,string varOrderHdrID,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,bool? varDeleted,string varOrganization,string varPickUpLocation,int? varNoOfChildren,int? varPointOfSaleID,string varResourceID,string varCheckInByWho,string varCheckOutByWho,DateTime? varCheckInTime,DateTime? varCheckOutTime,string varRemark,bool? varIsServerUpdate,int? varTimeExtension)
		{
			Appointment item = new Appointment();
			
				item.Id = varId;
			
				item.StartTime = varStartTime;
			
				item.Duration = varDuration;
			
				item.Description = varDescription;
			
				item.BackColor = varBackColor;
			
				item.FontColor = varFontColor;
			
				item.SalesPersonID = varSalesPersonID;
			
				item.MembershipNo = varMembershipNo;
			
				item.OrderHdrID = varOrderHdrID;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.Deleted = varDeleted;
			
				item.Organization = varOrganization;
			
				item.PickUpLocation = varPickUpLocation;
			
				item.NoOfChildren = varNoOfChildren;
			
				item.PointOfSaleID = varPointOfSaleID;
			
				item.ResourceID = varResourceID;
			
				item.CheckInByWho = varCheckInByWho;
			
				item.CheckOutByWho = varCheckOutByWho;
			
				item.CheckInTime = varCheckInTime;
			
				item.CheckOutTime = varCheckOutTime;
			
				item.Remark = varRemark;
			
				item.IsServerUpdate = varIsServerUpdate;
			
				item.TimeExtension = varTimeExtension;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn StartTimeColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DurationColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BackColorColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn FontColorColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn SalesPersonIDColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipNoColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderHdrIDColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn OrganizationColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn PickUpLocationColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn NoOfChildrenColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn PointOfSaleIDColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn ResourceIDColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn CheckInByWhoColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn CheckOutByWhoColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn CheckInTimeColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn CheckOutTimeColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn IsServerUpdateColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn TimeExtensionColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string StartTime = @"StartTime";
			 public static string Duration = @"Duration";
			 public static string Description = @"Description";
			 public static string BackColor = @"BackColor";
			 public static string FontColor = @"FontColor";
			 public static string SalesPersonID = @"SalesPersonID";
			 public static string MembershipNo = @"MembershipNo";
			 public static string OrderHdrID = @"OrderHdrID";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string Deleted = @"Deleted";
			 public static string Organization = @"Organization";
			 public static string PickUpLocation = @"PickUpLocation";
			 public static string NoOfChildren = @"NoOfChildren";
			 public static string PointOfSaleID = @"PointOfSaleID";
			 public static string ResourceID = @"ResourceID";
			 public static string CheckInByWho = @"CheckInByWho";
			 public static string CheckOutByWho = @"CheckOutByWho";
			 public static string CheckInTime = @"CheckInTime";
			 public static string CheckOutTime = @"CheckOutTime";
			 public static string Remark = @"Remark";
			 public static string IsServerUpdate = @"IsServerUpdate";
			 public static string TimeExtension = @"TimeExtension";
						
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
