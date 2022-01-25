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
	/// Strongly-typed collection for the CashDeviceInfo class.
	/// </summary>
    [Serializable]
	public partial class CashDeviceInfoCollection : ActiveList<CashDeviceInfo, CashDeviceInfoCollection>
	{	   
		public CashDeviceInfoCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>CashDeviceInfoCollection</returns>
		public CashDeviceInfoCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                CashDeviceInfo o = this[i];
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
	/// This is an ActiveRecord class which wraps the CashDeviceInfo table.
	/// </summary>
	[Serializable]
	public partial class CashDeviceInfo : ActiveRecord<CashDeviceInfo>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public CashDeviceInfo()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public CashDeviceInfo(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public CashDeviceInfo(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public CashDeviceInfo(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("CashDeviceInfo", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarCashDeviceInfoID = new TableSchema.TableColumn(schema);
				colvarCashDeviceInfoID.ColumnName = "CashDeviceInfoID";
				colvarCashDeviceInfoID.DataType = DbType.Int32;
				colvarCashDeviceInfoID.MaxLength = 0;
				colvarCashDeviceInfoID.AutoIncrement = true;
				colvarCashDeviceInfoID.IsNullable = false;
				colvarCashDeviceInfoID.IsPrimaryKey = true;
				colvarCashDeviceInfoID.IsForeignKey = false;
				colvarCashDeviceInfoID.IsReadOnly = false;
				colvarCashDeviceInfoID.DefaultSetting = @"";
				colvarCashDeviceInfoID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCashDeviceInfoID);
				
				TableSchema.TableColumn colvarDeviceID = new TableSchema.TableColumn(schema);
				colvarDeviceID.ColumnName = "DeviceID";
				colvarDeviceID.DataType = DbType.AnsiString;
				colvarDeviceID.MaxLength = 3;
				colvarDeviceID.AutoIncrement = false;
				colvarDeviceID.IsNullable = false;
				colvarDeviceID.IsPrimaryKey = false;
				colvarDeviceID.IsForeignKey = false;
				colvarDeviceID.IsReadOnly = false;
				colvarDeviceID.DefaultSetting = @"";
				colvarDeviceID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeviceID);
				
				TableSchema.TableColumn colvarDeviceName = new TableSchema.TableColumn(schema);
				colvarDeviceName.ColumnName = "DeviceName";
				colvarDeviceName.DataType = DbType.AnsiString;
				colvarDeviceName.MaxLength = 50;
				colvarDeviceName.AutoIncrement = false;
				colvarDeviceName.IsNullable = false;
				colvarDeviceName.IsPrimaryKey = false;
				colvarDeviceName.IsForeignKey = false;
				colvarDeviceName.IsReadOnly = false;
				colvarDeviceName.DefaultSetting = @"";
				colvarDeviceName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeviceName);
				
				TableSchema.TableColumn colvarCasseteID = new TableSchema.TableColumn(schema);
				colvarCasseteID.ColumnName = "CasseteID";
				colvarCasseteID.DataType = DbType.AnsiString;
				colvarCasseteID.MaxLength = 50;
				colvarCasseteID.AutoIncrement = false;
				colvarCasseteID.IsNullable = false;
				colvarCasseteID.IsPrimaryKey = false;
				colvarCasseteID.IsForeignKey = false;
				colvarCasseteID.IsReadOnly = false;
				colvarCasseteID.DefaultSetting = @"";
				colvarCasseteID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCasseteID);
				
				TableSchema.TableColumn colvarCasseteName = new TableSchema.TableColumn(schema);
				colvarCasseteName.ColumnName = "CasseteName";
				colvarCasseteName.DataType = DbType.AnsiString;
				colvarCasseteName.MaxLength = 50;
				colvarCasseteName.AutoIncrement = false;
				colvarCasseteName.IsNullable = false;
				colvarCasseteName.IsPrimaryKey = false;
				colvarCasseteName.IsForeignKey = false;
				colvarCasseteName.IsReadOnly = false;
				colvarCasseteName.DefaultSetting = @"";
				colvarCasseteName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCasseteName);
				
				TableSchema.TableColumn colvarDenomination = new TableSchema.TableColumn(schema);
				colvarDenomination.ColumnName = "Denomination";
				colvarDenomination.DataType = DbType.AnsiString;
				colvarDenomination.MaxLength = 50;
				colvarDenomination.AutoIncrement = false;
				colvarDenomination.IsNullable = false;
				colvarDenomination.IsPrimaryKey = false;
				colvarDenomination.IsForeignKey = false;
				colvarDenomination.IsReadOnly = false;
				colvarDenomination.DefaultSetting = @"";
				colvarDenomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDenomination);
				
				TableSchema.TableColumn colvarCount = new TableSchema.TableColumn(schema);
				colvarCount.ColumnName = "Count";
				colvarCount.DataType = DbType.Int32;
				colvarCount.MaxLength = 0;
				colvarCount.AutoIncrement = false;
				colvarCount.IsNullable = false;
				colvarCount.IsPrimaryKey = false;
				colvarCount.IsForeignKey = false;
				colvarCount.IsReadOnly = false;
				colvarCount.DefaultSetting = @"";
				colvarCount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCount);
				
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
				
				TableSchema.TableColumn colvarUserfld1 = new TableSchema.TableColumn(schema);
				colvarUserfld1.ColumnName = "Userfld1";
				colvarUserfld1.DataType = DbType.AnsiString;
				colvarUserfld1.MaxLength = 50;
				colvarUserfld1.AutoIncrement = false;
				colvarUserfld1.IsNullable = true;
				colvarUserfld1.IsPrimaryKey = false;
				colvarUserfld1.IsForeignKey = false;
				colvarUserfld1.IsReadOnly = false;
				colvarUserfld1.DefaultSetting = @"";
				colvarUserfld1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld1);
				
				TableSchema.TableColumn colvarUserfld2 = new TableSchema.TableColumn(schema);
				colvarUserfld2.ColumnName = "Userfld2";
				colvarUserfld2.DataType = DbType.AnsiString;
				colvarUserfld2.MaxLength = 50;
				colvarUserfld2.AutoIncrement = false;
				colvarUserfld2.IsNullable = true;
				colvarUserfld2.IsPrimaryKey = false;
				colvarUserfld2.IsForeignKey = false;
				colvarUserfld2.IsReadOnly = false;
				colvarUserfld2.DefaultSetting = @"";
				colvarUserfld2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld2);
				
				TableSchema.TableColumn colvarUserfld3 = new TableSchema.TableColumn(schema);
				colvarUserfld3.ColumnName = "Userfld3";
				colvarUserfld3.DataType = DbType.AnsiString;
				colvarUserfld3.MaxLength = 50;
				colvarUserfld3.AutoIncrement = false;
				colvarUserfld3.IsNullable = true;
				colvarUserfld3.IsPrimaryKey = false;
				colvarUserfld3.IsForeignKey = false;
				colvarUserfld3.IsReadOnly = false;
				colvarUserfld3.DefaultSetting = @"";
				colvarUserfld3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld3);
				
				TableSchema.TableColumn colvarUserfld4 = new TableSchema.TableColumn(schema);
				colvarUserfld4.ColumnName = "Userfld4";
				colvarUserfld4.DataType = DbType.AnsiString;
				colvarUserfld4.MaxLength = 50;
				colvarUserfld4.AutoIncrement = false;
				colvarUserfld4.IsNullable = true;
				colvarUserfld4.IsPrimaryKey = false;
				colvarUserfld4.IsForeignKey = false;
				colvarUserfld4.IsReadOnly = false;
				colvarUserfld4.DefaultSetting = @"";
				colvarUserfld4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld4);
				
				TableSchema.TableColumn colvarUserfld5 = new TableSchema.TableColumn(schema);
				colvarUserfld5.ColumnName = "Userfld5";
				colvarUserfld5.DataType = DbType.AnsiString;
				colvarUserfld5.MaxLength = 50;
				colvarUserfld5.AutoIncrement = false;
				colvarUserfld5.IsNullable = true;
				colvarUserfld5.IsPrimaryKey = false;
				colvarUserfld5.IsForeignKey = false;
				colvarUserfld5.IsReadOnly = false;
				colvarUserfld5.DefaultSetting = @"";
				colvarUserfld5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld5);
				
				TableSchema.TableColumn colvarUserfld6 = new TableSchema.TableColumn(schema);
				colvarUserfld6.ColumnName = "Userfld6";
				colvarUserfld6.DataType = DbType.AnsiString;
				colvarUserfld6.MaxLength = 50;
				colvarUserfld6.AutoIncrement = false;
				colvarUserfld6.IsNullable = true;
				colvarUserfld6.IsPrimaryKey = false;
				colvarUserfld6.IsForeignKey = false;
				colvarUserfld6.IsReadOnly = false;
				colvarUserfld6.DefaultSetting = @"";
				colvarUserfld6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld6);
				
				TableSchema.TableColumn colvarUserfld7 = new TableSchema.TableColumn(schema);
				colvarUserfld7.ColumnName = "Userfld7";
				colvarUserfld7.DataType = DbType.AnsiString;
				colvarUserfld7.MaxLength = 50;
				colvarUserfld7.AutoIncrement = false;
				colvarUserfld7.IsNullable = true;
				colvarUserfld7.IsPrimaryKey = false;
				colvarUserfld7.IsForeignKey = false;
				colvarUserfld7.IsReadOnly = false;
				colvarUserfld7.DefaultSetting = @"";
				colvarUserfld7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld7);
				
				TableSchema.TableColumn colvarUserfld8 = new TableSchema.TableColumn(schema);
				colvarUserfld8.ColumnName = "Userfld8";
				colvarUserfld8.DataType = DbType.AnsiString;
				colvarUserfld8.MaxLength = 50;
				colvarUserfld8.AutoIncrement = false;
				colvarUserfld8.IsNullable = true;
				colvarUserfld8.IsPrimaryKey = false;
				colvarUserfld8.IsForeignKey = false;
				colvarUserfld8.IsReadOnly = false;
				colvarUserfld8.DefaultSetting = @"";
				colvarUserfld8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld8);
				
				TableSchema.TableColumn colvarUserfld9 = new TableSchema.TableColumn(schema);
				colvarUserfld9.ColumnName = "Userfld9";
				colvarUserfld9.DataType = DbType.AnsiString;
				colvarUserfld9.MaxLength = 50;
				colvarUserfld9.AutoIncrement = false;
				colvarUserfld9.IsNullable = true;
				colvarUserfld9.IsPrimaryKey = false;
				colvarUserfld9.IsForeignKey = false;
				colvarUserfld9.IsReadOnly = false;
				colvarUserfld9.DefaultSetting = @"";
				colvarUserfld9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld9);
				
				TableSchema.TableColumn colvarUserfld10 = new TableSchema.TableColumn(schema);
				colvarUserfld10.ColumnName = "Userfld10";
				colvarUserfld10.DataType = DbType.AnsiString;
				colvarUserfld10.MaxLength = 50;
				colvarUserfld10.AutoIncrement = false;
				colvarUserfld10.IsNullable = true;
				colvarUserfld10.IsPrimaryKey = false;
				colvarUserfld10.IsForeignKey = false;
				colvarUserfld10.IsReadOnly = false;
				colvarUserfld10.DefaultSetting = @"";
				colvarUserfld10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld10);
				
				TableSchema.TableColumn colvarDeviceType = new TableSchema.TableColumn(schema);
				colvarDeviceType.ColumnName = "DeviceType";
				colvarDeviceType.DataType = DbType.AnsiString;
				colvarDeviceType.MaxLength = 50;
				colvarDeviceType.AutoIncrement = false;
				colvarDeviceType.IsNullable = true;
				colvarDeviceType.IsPrimaryKey = false;
				colvarDeviceType.IsForeignKey = false;
				colvarDeviceType.IsReadOnly = false;
				colvarDeviceType.DefaultSetting = @"";
				colvarDeviceType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeviceType);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("CashDeviceInfo",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("CashDeviceInfoID")]
		[Bindable(true)]
		public int CashDeviceInfoID 
		{
			get { return GetColumnValue<int>(Columns.CashDeviceInfoID); }
			set { SetColumnValue(Columns.CashDeviceInfoID, value); }
		}
		  
		[XmlAttribute("DeviceID")]
		[Bindable(true)]
		public string DeviceID 
		{
			get { return GetColumnValue<string>(Columns.DeviceID); }
			set { SetColumnValue(Columns.DeviceID, value); }
		}
		  
		[XmlAttribute("DeviceName")]
		[Bindable(true)]
		public string DeviceName 
		{
			get { return GetColumnValue<string>(Columns.DeviceName); }
			set { SetColumnValue(Columns.DeviceName, value); }
		}
		  
		[XmlAttribute("CasseteID")]
		[Bindable(true)]
		public string CasseteID 
		{
			get { return GetColumnValue<string>(Columns.CasseteID); }
			set { SetColumnValue(Columns.CasseteID, value); }
		}
		  
		[XmlAttribute("CasseteName")]
		[Bindable(true)]
		public string CasseteName 
		{
			get { return GetColumnValue<string>(Columns.CasseteName); }
			set { SetColumnValue(Columns.CasseteName, value); }
		}
		  
		[XmlAttribute("Denomination")]
		[Bindable(true)]
		public string Denomination 
		{
			get { return GetColumnValue<string>(Columns.Denomination); }
			set { SetColumnValue(Columns.Denomination, value); }
		}
		  
		[XmlAttribute("Count")]
		[Bindable(true)]
		public int Count 
		{
			get { return GetColumnValue<int>(Columns.Count); }
			set { SetColumnValue(Columns.Count, value); }
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
		  
		[XmlAttribute("Userfld1")]
		[Bindable(true)]
		public string Userfld1 
		{
			get { return GetColumnValue<string>(Columns.Userfld1); }
			set { SetColumnValue(Columns.Userfld1, value); }
		}
		  
		[XmlAttribute("Userfld2")]
		[Bindable(true)]
		public string Userfld2 
		{
			get { return GetColumnValue<string>(Columns.Userfld2); }
			set { SetColumnValue(Columns.Userfld2, value); }
		}
		  
		[XmlAttribute("Userfld3")]
		[Bindable(true)]
		public string Userfld3 
		{
			get { return GetColumnValue<string>(Columns.Userfld3); }
			set { SetColumnValue(Columns.Userfld3, value); }
		}
		  
		[XmlAttribute("Userfld4")]
		[Bindable(true)]
		public string Userfld4 
		{
			get { return GetColumnValue<string>(Columns.Userfld4); }
			set { SetColumnValue(Columns.Userfld4, value); }
		}
		  
		[XmlAttribute("Userfld5")]
		[Bindable(true)]
		public string Userfld5 
		{
			get { return GetColumnValue<string>(Columns.Userfld5); }
			set { SetColumnValue(Columns.Userfld5, value); }
		}
		  
		[XmlAttribute("Userfld6")]
		[Bindable(true)]
		public string Userfld6 
		{
			get { return GetColumnValue<string>(Columns.Userfld6); }
			set { SetColumnValue(Columns.Userfld6, value); }
		}
		  
		[XmlAttribute("Userfld7")]
		[Bindable(true)]
		public string Userfld7 
		{
			get { return GetColumnValue<string>(Columns.Userfld7); }
			set { SetColumnValue(Columns.Userfld7, value); }
		}
		  
		[XmlAttribute("Userfld8")]
		[Bindable(true)]
		public string Userfld8 
		{
			get { return GetColumnValue<string>(Columns.Userfld8); }
			set { SetColumnValue(Columns.Userfld8, value); }
		}
		  
		[XmlAttribute("Userfld9")]
		[Bindable(true)]
		public string Userfld9 
		{
			get { return GetColumnValue<string>(Columns.Userfld9); }
			set { SetColumnValue(Columns.Userfld9, value); }
		}
		  
		[XmlAttribute("Userfld10")]
		[Bindable(true)]
		public string Userfld10 
		{
			get { return GetColumnValue<string>(Columns.Userfld10); }
			set { SetColumnValue(Columns.Userfld10, value); }
		}
		  
		[XmlAttribute("DeviceType")]
		[Bindable(true)]
		public string DeviceType 
		{
			get { return GetColumnValue<string>(Columns.DeviceType); }
			set { SetColumnValue(Columns.DeviceType, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varDeviceID,string varDeviceName,string varCasseteID,string varCasseteName,string varDenomination,int varCount,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,string varDeviceType)
		{
			CashDeviceInfo item = new CashDeviceInfo();
			
			item.DeviceID = varDeviceID;
			
			item.DeviceName = varDeviceName;
			
			item.CasseteID = varCasseteID;
			
			item.CasseteName = varCasseteName;
			
			item.Denomination = varDenomination;
			
			item.Count = varCount;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.Userfld1 = varUserfld1;
			
			item.Userfld2 = varUserfld2;
			
			item.Userfld3 = varUserfld3;
			
			item.Userfld4 = varUserfld4;
			
			item.Userfld5 = varUserfld5;
			
			item.Userfld6 = varUserfld6;
			
			item.Userfld7 = varUserfld7;
			
			item.Userfld8 = varUserfld8;
			
			item.Userfld9 = varUserfld9;
			
			item.Userfld10 = varUserfld10;
			
			item.DeviceType = varDeviceType;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varCashDeviceInfoID,string varDeviceID,string varDeviceName,string varCasseteID,string varCasseteName,string varDenomination,int varCount,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,string varDeviceType)
		{
			CashDeviceInfo item = new CashDeviceInfo();
			
				item.CashDeviceInfoID = varCashDeviceInfoID;
			
				item.DeviceID = varDeviceID;
			
				item.DeviceName = varDeviceName;
			
				item.CasseteID = varCasseteID;
			
				item.CasseteName = varCasseteName;
			
				item.Denomination = varDenomination;
			
				item.Count = varCount;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.Userfld1 = varUserfld1;
			
				item.Userfld2 = varUserfld2;
			
				item.Userfld3 = varUserfld3;
			
				item.Userfld4 = varUserfld4;
			
				item.Userfld5 = varUserfld5;
			
				item.Userfld6 = varUserfld6;
			
				item.Userfld7 = varUserfld7;
			
				item.Userfld8 = varUserfld8;
			
				item.Userfld9 = varUserfld9;
			
				item.Userfld10 = varUserfld10;
			
				item.DeviceType = varDeviceType;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn CashDeviceInfoIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn DeviceIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DeviceNameColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CasseteIDColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CasseteNameColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DenominationColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CountColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld1Column
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld2Column
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld3Column
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld4Column
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld5Column
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld6Column
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld7Column
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld8Column
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld9Column
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld10Column
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn DeviceTypeColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string CashDeviceInfoID = @"CashDeviceInfoID";
			 public static string DeviceID = @"DeviceID";
			 public static string DeviceName = @"DeviceName";
			 public static string CasseteID = @"CasseteID";
			 public static string CasseteName = @"CasseteName";
			 public static string Denomination = @"Denomination";
			 public static string Count = @"Count";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string Userfld1 = @"Userfld1";
			 public static string Userfld2 = @"Userfld2";
			 public static string Userfld3 = @"Userfld3";
			 public static string Userfld4 = @"Userfld4";
			 public static string Userfld5 = @"Userfld5";
			 public static string Userfld6 = @"Userfld6";
			 public static string Userfld7 = @"Userfld7";
			 public static string Userfld8 = @"Userfld8";
			 public static string Userfld9 = @"Userfld9";
			 public static string Userfld10 = @"Userfld10";
			 public static string DeviceType = @"DeviceType";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
