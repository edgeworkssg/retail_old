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
	/// Strongly-typed collection for the ActivePayment class.
	/// </summary>
    [Serializable]
	public partial class ActivePaymentCollection : ActiveList<ActivePayment, ActivePaymentCollection>
	{	   
		public ActivePaymentCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ActivePaymentCollection</returns>
		public ActivePaymentCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ActivePayment o = this[i];
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
	/// This is an ActiveRecord class which wraps the ActivePayment table.
	/// </summary>
	[Serializable]
	public partial class ActivePayment : ActiveRecord<ActivePayment>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ActivePayment()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ActivePayment(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ActivePayment(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ActivePayment(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ActivePayment", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPaymentID = new TableSchema.TableColumn(schema);
				colvarPaymentID.ColumnName = "PaymentID";
				colvarPaymentID.DataType = DbType.Int32;
				colvarPaymentID.MaxLength = 0;
				colvarPaymentID.AutoIncrement = false;
				colvarPaymentID.IsNullable = false;
				colvarPaymentID.IsPrimaryKey = true;
				colvarPaymentID.IsForeignKey = false;
				colvarPaymentID.IsReadOnly = false;
				colvarPaymentID.DefaultSetting = @"";
				colvarPaymentID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPaymentID);
				
				TableSchema.TableColumn colvarPaymentName = new TableSchema.TableColumn(schema);
				colvarPaymentName.ColumnName = "PaymentName";
				colvarPaymentName.DataType = DbType.String;
				colvarPaymentName.MaxLength = 250;
				colvarPaymentName.AutoIncrement = false;
				colvarPaymentName.IsNullable = false;
				colvarPaymentName.IsPrimaryKey = false;
				colvarPaymentName.IsForeignKey = false;
				colvarPaymentName.IsReadOnly = false;
				colvarPaymentName.DefaultSetting = @"";
				colvarPaymentName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPaymentName);
				
				TableSchema.TableColumn colvarIsActive = new TableSchema.TableColumn(schema);
				colvarIsActive.ColumnName = "IsActive";
				colvarIsActive.DataType = DbType.Boolean;
				colvarIsActive.MaxLength = 0;
				colvarIsActive.AutoIncrement = false;
				colvarIsActive.IsNullable = false;
				colvarIsActive.IsPrimaryKey = false;
				colvarIsActive.IsForeignKey = false;
				colvarIsActive.IsReadOnly = false;
				colvarIsActive.DefaultSetting = @"";
				colvarIsActive.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsActive);
				
				TableSchema.TableColumn colvarRemarks = new TableSchema.TableColumn(schema);
				colvarRemarks.ColumnName = "Remarks";
				colvarRemarks.DataType = DbType.AnsiString;
				colvarRemarks.MaxLength = 50;
				colvarRemarks.AutoIncrement = false;
				colvarRemarks.IsNullable = true;
				colvarRemarks.IsPrimaryKey = false;
				colvarRemarks.IsForeignKey = false;
				colvarRemarks.IsReadOnly = false;
				colvarRemarks.DefaultSetting = @"";
				colvarRemarks.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemarks);
				
				TableSchema.TableColumn colvarAllowChange = new TableSchema.TableColumn(schema);
				colvarAllowChange.ColumnName = "AllowChange";
				colvarAllowChange.DataType = DbType.Boolean;
				colvarAllowChange.MaxLength = 0;
				colvarAllowChange.AutoIncrement = false;
				colvarAllowChange.IsNullable = true;
				colvarAllowChange.IsPrimaryKey = false;
				colvarAllowChange.IsForeignKey = false;
				colvarAllowChange.IsReadOnly = false;
				colvarAllowChange.DefaultSetting = @"";
				colvarAllowChange.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAllowChange);
				
				TableSchema.TableColumn colvarAllowExtra = new TableSchema.TableColumn(schema);
				colvarAllowExtra.ColumnName = "AllowExtra";
				colvarAllowExtra.DataType = DbType.Boolean;
				colvarAllowExtra.MaxLength = 0;
				colvarAllowExtra.AutoIncrement = false;
				colvarAllowExtra.IsNullable = true;
				colvarAllowExtra.IsPrimaryKey = false;
				colvarAllowExtra.IsForeignKey = false;
				colvarAllowExtra.IsReadOnly = false;
				colvarAllowExtra.DefaultSetting = @"";
				colvarAllowExtra.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAllowExtra);
				
				TableSchema.TableColumn colvarShowRemark1 = new TableSchema.TableColumn(schema);
				colvarShowRemark1.ColumnName = "ShowRemark1";
				colvarShowRemark1.DataType = DbType.Boolean;
				colvarShowRemark1.MaxLength = 0;
				colvarShowRemark1.AutoIncrement = false;
				colvarShowRemark1.IsNullable = true;
				colvarShowRemark1.IsPrimaryKey = false;
				colvarShowRemark1.IsForeignKey = false;
				colvarShowRemark1.IsReadOnly = false;
				colvarShowRemark1.DefaultSetting = @"";
				colvarShowRemark1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowRemark1);
				
				TableSchema.TableColumn colvarLabelRemark1 = new TableSchema.TableColumn(schema);
				colvarLabelRemark1.ColumnName = "LabelRemark1";
				colvarLabelRemark1.DataType = DbType.String;
				colvarLabelRemark1.MaxLength = 150;
				colvarLabelRemark1.AutoIncrement = false;
				colvarLabelRemark1.IsNullable = true;
				colvarLabelRemark1.IsPrimaryKey = false;
				colvarLabelRemark1.IsForeignKey = false;
				colvarLabelRemark1.IsReadOnly = false;
				colvarLabelRemark1.DefaultSetting = @"";
				colvarLabelRemark1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLabelRemark1);
				
				TableSchema.TableColumn colvarShowRemark2 = new TableSchema.TableColumn(schema);
				colvarShowRemark2.ColumnName = "ShowRemark2";
				colvarShowRemark2.DataType = DbType.Boolean;
				colvarShowRemark2.MaxLength = 0;
				colvarShowRemark2.AutoIncrement = false;
				colvarShowRemark2.IsNullable = true;
				colvarShowRemark2.IsPrimaryKey = false;
				colvarShowRemark2.IsForeignKey = false;
				colvarShowRemark2.IsReadOnly = false;
				colvarShowRemark2.DefaultSetting = @"";
				colvarShowRemark2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowRemark2);
				
				TableSchema.TableColumn colvarLabelRemark2 = new TableSchema.TableColumn(schema);
				colvarLabelRemark2.ColumnName = "LabelRemark2";
				colvarLabelRemark2.DataType = DbType.String;
				colvarLabelRemark2.MaxLength = 150;
				colvarLabelRemark2.AutoIncrement = false;
				colvarLabelRemark2.IsNullable = true;
				colvarLabelRemark2.IsPrimaryKey = false;
				colvarLabelRemark2.IsForeignKey = false;
				colvarLabelRemark2.IsReadOnly = false;
				colvarLabelRemark2.DefaultSetting = @"";
				colvarLabelRemark2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLabelRemark2);
				
				TableSchema.TableColumn colvarShowRemark3 = new TableSchema.TableColumn(schema);
				colvarShowRemark3.ColumnName = "ShowRemark3";
				colvarShowRemark3.DataType = DbType.Boolean;
				colvarShowRemark3.MaxLength = 0;
				colvarShowRemark3.AutoIncrement = false;
				colvarShowRemark3.IsNullable = true;
				colvarShowRemark3.IsPrimaryKey = false;
				colvarShowRemark3.IsForeignKey = false;
				colvarShowRemark3.IsReadOnly = false;
				colvarShowRemark3.DefaultSetting = @"";
				colvarShowRemark3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowRemark3);
				
				TableSchema.TableColumn colvarLabelRemark3 = new TableSchema.TableColumn(schema);
				colvarLabelRemark3.ColumnName = "LabelRemark3";
				colvarLabelRemark3.DataType = DbType.String;
				colvarLabelRemark3.MaxLength = 150;
				colvarLabelRemark3.AutoIncrement = false;
				colvarLabelRemark3.IsNullable = true;
				colvarLabelRemark3.IsPrimaryKey = false;
				colvarLabelRemark3.IsForeignKey = false;
				colvarLabelRemark3.IsReadOnly = false;
				colvarLabelRemark3.DefaultSetting = @"";
				colvarLabelRemark3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLabelRemark3);
				
				TableSchema.TableColumn colvarShowRemark4 = new TableSchema.TableColumn(schema);
				colvarShowRemark4.ColumnName = "ShowRemark4";
				colvarShowRemark4.DataType = DbType.Boolean;
				colvarShowRemark4.MaxLength = 0;
				colvarShowRemark4.AutoIncrement = false;
				colvarShowRemark4.IsNullable = true;
				colvarShowRemark4.IsPrimaryKey = false;
				colvarShowRemark4.IsForeignKey = false;
				colvarShowRemark4.IsReadOnly = false;
				colvarShowRemark4.DefaultSetting = @"";
				colvarShowRemark4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowRemark4);
				
				TableSchema.TableColumn colvarLabelRemark4 = new TableSchema.TableColumn(schema);
				colvarLabelRemark4.ColumnName = "LabelRemark4";
				colvarLabelRemark4.DataType = DbType.String;
				colvarLabelRemark4.MaxLength = 150;
				colvarLabelRemark4.AutoIncrement = false;
				colvarLabelRemark4.IsNullable = true;
				colvarLabelRemark4.IsPrimaryKey = false;
				colvarLabelRemark4.IsForeignKey = false;
				colvarLabelRemark4.IsReadOnly = false;
				colvarLabelRemark4.DefaultSetting = @"";
				colvarLabelRemark4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLabelRemark4);
				
				TableSchema.TableColumn colvarShowRemark5 = new TableSchema.TableColumn(schema);
				colvarShowRemark5.ColumnName = "ShowRemark5";
				colvarShowRemark5.DataType = DbType.Boolean;
				colvarShowRemark5.MaxLength = 0;
				colvarShowRemark5.AutoIncrement = false;
				colvarShowRemark5.IsNullable = true;
				colvarShowRemark5.IsPrimaryKey = false;
				colvarShowRemark5.IsForeignKey = false;
				colvarShowRemark5.IsReadOnly = false;
				colvarShowRemark5.DefaultSetting = @"";
				colvarShowRemark5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowRemark5);
				
				TableSchema.TableColumn colvarLabelRemark5 = new TableSchema.TableColumn(schema);
				colvarLabelRemark5.ColumnName = "LabelRemark5";
				colvarLabelRemark5.DataType = DbType.String;
				colvarLabelRemark5.MaxLength = 150;
				colvarLabelRemark5.AutoIncrement = false;
				colvarLabelRemark5.IsNullable = true;
				colvarLabelRemark5.IsPrimaryKey = false;
				colvarLabelRemark5.IsForeignKey = false;
				colvarLabelRemark5.IsReadOnly = false;
				colvarLabelRemark5.DefaultSetting = @"";
				colvarLabelRemark5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLabelRemark5);
				
				TableSchema.TableColumn colvarUserfld1 = new TableSchema.TableColumn(schema);
				colvarUserfld1.ColumnName = "userfld1";
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("ActivePayment",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PaymentID")]
		[Bindable(true)]
		public int PaymentID 
		{
			get { return GetColumnValue<int>(Columns.PaymentID); }
			set { SetColumnValue(Columns.PaymentID, value); }
		}
		  
		[XmlAttribute("PaymentName")]
		[Bindable(true)]
		public string PaymentName 
		{
			get { return GetColumnValue<string>(Columns.PaymentName); }
			set { SetColumnValue(Columns.PaymentName, value); }
		}
		  
		[XmlAttribute("IsActive")]
		[Bindable(true)]
		public bool IsActive 
		{
			get { return GetColumnValue<bool>(Columns.IsActive); }
			set { SetColumnValue(Columns.IsActive, value); }
		}
		  
		[XmlAttribute("Remarks")]
		[Bindable(true)]
		public string Remarks 
		{
			get { return GetColumnValue<string>(Columns.Remarks); }
			set { SetColumnValue(Columns.Remarks, value); }
		}
		  
		[XmlAttribute("AllowChange")]
		[Bindable(true)]
		public bool? AllowChange 
		{
			get { return GetColumnValue<bool?>(Columns.AllowChange); }
			set { SetColumnValue(Columns.AllowChange, value); }
		}
		  
		[XmlAttribute("AllowExtra")]
		[Bindable(true)]
		public bool? AllowExtra 
		{
			get { return GetColumnValue<bool?>(Columns.AllowExtra); }
			set { SetColumnValue(Columns.AllowExtra, value); }
		}
		  
		[XmlAttribute("ShowRemark1")]
		[Bindable(true)]
		public bool? ShowRemark1 
		{
			get { return GetColumnValue<bool?>(Columns.ShowRemark1); }
			set { SetColumnValue(Columns.ShowRemark1, value); }
		}
		  
		[XmlAttribute("LabelRemark1")]
		[Bindable(true)]
		public string LabelRemark1 
		{
			get { return GetColumnValue<string>(Columns.LabelRemark1); }
			set { SetColumnValue(Columns.LabelRemark1, value); }
		}
		  
		[XmlAttribute("ShowRemark2")]
		[Bindable(true)]
		public bool? ShowRemark2 
		{
			get { return GetColumnValue<bool?>(Columns.ShowRemark2); }
			set { SetColumnValue(Columns.ShowRemark2, value); }
		}
		  
		[XmlAttribute("LabelRemark2")]
		[Bindable(true)]
		public string LabelRemark2 
		{
			get { return GetColumnValue<string>(Columns.LabelRemark2); }
			set { SetColumnValue(Columns.LabelRemark2, value); }
		}
		  
		[XmlAttribute("ShowRemark3")]
		[Bindable(true)]
		public bool? ShowRemark3 
		{
			get { return GetColumnValue<bool?>(Columns.ShowRemark3); }
			set { SetColumnValue(Columns.ShowRemark3, value); }
		}
		  
		[XmlAttribute("LabelRemark3")]
		[Bindable(true)]
		public string LabelRemark3 
		{
			get { return GetColumnValue<string>(Columns.LabelRemark3); }
			set { SetColumnValue(Columns.LabelRemark3, value); }
		}
		  
		[XmlAttribute("ShowRemark4")]
		[Bindable(true)]
		public bool? ShowRemark4 
		{
			get { return GetColumnValue<bool?>(Columns.ShowRemark4); }
			set { SetColumnValue(Columns.ShowRemark4, value); }
		}
		  
		[XmlAttribute("LabelRemark4")]
		[Bindable(true)]
		public string LabelRemark4 
		{
			get { return GetColumnValue<string>(Columns.LabelRemark4); }
			set { SetColumnValue(Columns.LabelRemark4, value); }
		}
		  
		[XmlAttribute("ShowRemark5")]
		[Bindable(true)]
		public bool? ShowRemark5 
		{
			get { return GetColumnValue<bool?>(Columns.ShowRemark5); }
			set { SetColumnValue(Columns.ShowRemark5, value); }
		}
		  
		[XmlAttribute("LabelRemark5")]
		[Bindable(true)]
		public string LabelRemark5 
		{
			get { return GetColumnValue<string>(Columns.LabelRemark5); }
			set { SetColumnValue(Columns.LabelRemark5, value); }
		}
		  
		[XmlAttribute("Userfld1")]
		[Bindable(true)]
		public string Userfld1 
		{
			get { return GetColumnValue<string>(Columns.Userfld1); }
			set { SetColumnValue(Columns.Userfld1, value); }
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
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varPaymentID,string varPaymentName,bool varIsActive,string varRemarks,bool? varAllowChange,bool? varAllowExtra,bool? varShowRemark1,string varLabelRemark1,bool? varShowRemark2,string varLabelRemark2,bool? varShowRemark3,string varLabelRemark3,bool? varShowRemark4,string varLabelRemark4,bool? varShowRemark5,string varLabelRemark5,string varUserfld1,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,bool? varDeleted)
		{
			ActivePayment item = new ActivePayment();
			
			item.PaymentID = varPaymentID;
			
			item.PaymentName = varPaymentName;
			
			item.IsActive = varIsActive;
			
			item.Remarks = varRemarks;
			
			item.AllowChange = varAllowChange;
			
			item.AllowExtra = varAllowExtra;
			
			item.ShowRemark1 = varShowRemark1;
			
			item.LabelRemark1 = varLabelRemark1;
			
			item.ShowRemark2 = varShowRemark2;
			
			item.LabelRemark2 = varLabelRemark2;
			
			item.ShowRemark3 = varShowRemark3;
			
			item.LabelRemark3 = varLabelRemark3;
			
			item.ShowRemark4 = varShowRemark4;
			
			item.LabelRemark4 = varLabelRemark4;
			
			item.ShowRemark5 = varShowRemark5;
			
			item.LabelRemark5 = varLabelRemark5;
			
			item.Userfld1 = varUserfld1;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.Deleted = varDeleted;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varPaymentID,string varPaymentName,bool varIsActive,string varRemarks,bool? varAllowChange,bool? varAllowExtra,bool? varShowRemark1,string varLabelRemark1,bool? varShowRemark2,string varLabelRemark2,bool? varShowRemark3,string varLabelRemark3,bool? varShowRemark4,string varLabelRemark4,bool? varShowRemark5,string varLabelRemark5,string varUserfld1,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,bool? varDeleted)
		{
			ActivePayment item = new ActivePayment();
			
				item.PaymentID = varPaymentID;
			
				item.PaymentName = varPaymentName;
			
				item.IsActive = varIsActive;
			
				item.Remarks = varRemarks;
			
				item.AllowChange = varAllowChange;
			
				item.AllowExtra = varAllowExtra;
			
				item.ShowRemark1 = varShowRemark1;
			
				item.LabelRemark1 = varLabelRemark1;
			
				item.ShowRemark2 = varShowRemark2;
			
				item.LabelRemark2 = varLabelRemark2;
			
				item.ShowRemark3 = varShowRemark3;
			
				item.LabelRemark3 = varLabelRemark3;
			
				item.ShowRemark4 = varShowRemark4;
			
				item.LabelRemark4 = varLabelRemark4;
			
				item.ShowRemark5 = varShowRemark5;
			
				item.LabelRemark5 = varLabelRemark5;
			
				item.Userfld1 = varUserfld1;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.Deleted = varDeleted;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn PaymentIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PaymentNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn IsActiveColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarksColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn AllowChangeColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn AllowExtraColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowRemark1Column
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn LabelRemark1Column
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowRemark2Column
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn LabelRemark2Column
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowRemark3Column
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn LabelRemark3Column
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowRemark4Column
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn LabelRemark4Column
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowRemark5Column
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn LabelRemark5Column
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld1Column
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PaymentID = @"PaymentID";
			 public static string PaymentName = @"PaymentName";
			 public static string IsActive = @"IsActive";
			 public static string Remarks = @"Remarks";
			 public static string AllowChange = @"AllowChange";
			 public static string AllowExtra = @"AllowExtra";
			 public static string ShowRemark1 = @"ShowRemark1";
			 public static string LabelRemark1 = @"LabelRemark1";
			 public static string ShowRemark2 = @"ShowRemark2";
			 public static string LabelRemark2 = @"LabelRemark2";
			 public static string ShowRemark3 = @"ShowRemark3";
			 public static string LabelRemark3 = @"LabelRemark3";
			 public static string ShowRemark4 = @"ShowRemark4";
			 public static string LabelRemark4 = @"LabelRemark4";
			 public static string ShowRemark5 = @"ShowRemark5";
			 public static string LabelRemark5 = @"LabelRemark5";
			 public static string Userfld1 = @"userfld1";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string Deleted = @"Deleted";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
