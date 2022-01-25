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
	/// Strongly-typed collection for the ReceiptSetting class.
	/// </summary>
    [Serializable]
	public partial class ReceiptSettingCollection : ActiveList<ReceiptSetting, ReceiptSettingCollection>
	{	   
		public ReceiptSettingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ReceiptSettingCollection</returns>
		public ReceiptSettingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ReceiptSetting o = this[i];
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
	/// This is an ActiveRecord class which wraps the ReceiptSetting table.
	/// </summary>
	[Serializable]
	public partial class ReceiptSetting : ActiveRecord<ReceiptSetting>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ReceiptSetting()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ReceiptSetting(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ReceiptSetting(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ReceiptSetting(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ReceiptSetting", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarSettingId = new TableSchema.TableColumn(schema);
				colvarSettingId.ColumnName = "settingId";
				colvarSettingId.DataType = DbType.Guid;
				colvarSettingId.MaxLength = 0;
				colvarSettingId.AutoIncrement = false;
				colvarSettingId.IsNullable = false;
				colvarSettingId.IsPrimaryKey = true;
				colvarSettingId.IsForeignKey = false;
				colvarSettingId.IsReadOnly = false;
				
						colvarSettingId.DefaultSetting = @"(newid())";
				colvarSettingId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSettingId);
				
				TableSchema.TableColumn colvarSettingRemark = new TableSchema.TableColumn(schema);
				colvarSettingRemark.ColumnName = "settingRemark";
				colvarSettingRemark.DataType = DbType.String;
				colvarSettingRemark.MaxLength = 10;
				colvarSettingRemark.AutoIncrement = false;
				colvarSettingRemark.IsNullable = false;
				colvarSettingRemark.IsPrimaryKey = false;
				colvarSettingRemark.IsForeignKey = false;
				colvarSettingRemark.IsReadOnly = false;
				colvarSettingRemark.DefaultSetting = @"";
				colvarSettingRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSettingRemark);
				
				TableSchema.TableColumn colvarPrintReceipt = new TableSchema.TableColumn(schema);
				colvarPrintReceipt.ColumnName = "printReceipt";
				colvarPrintReceipt.DataType = DbType.Boolean;
				colvarPrintReceipt.MaxLength = 0;
				colvarPrintReceipt.AutoIncrement = false;
				colvarPrintReceipt.IsNullable = false;
				colvarPrintReceipt.IsPrimaryKey = false;
				colvarPrintReceipt.IsForeignKey = false;
				colvarPrintReceipt.IsReadOnly = false;
				colvarPrintReceipt.DefaultSetting = @"";
				colvarPrintReceipt.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPrintReceipt);
				
				TableSchema.TableColumn colvarUseOutletAddress = new TableSchema.TableColumn(schema);
				colvarUseOutletAddress.ColumnName = "useOutletAddress";
				colvarUseOutletAddress.DataType = DbType.Boolean;
				colvarUseOutletAddress.MaxLength = 0;
				colvarUseOutletAddress.AutoIncrement = false;
				colvarUseOutletAddress.IsNullable = false;
				colvarUseOutletAddress.IsPrimaryKey = false;
				colvarUseOutletAddress.IsForeignKey = false;
				colvarUseOutletAddress.IsReadOnly = false;
				colvarUseOutletAddress.DefaultSetting = @"";
				colvarUseOutletAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUseOutletAddress);
				
				TableSchema.TableColumn colvarReceiptAddress1 = new TableSchema.TableColumn(schema);
				colvarReceiptAddress1.ColumnName = "receiptAddress1";
				colvarReceiptAddress1.DataType = DbType.String;
				colvarReceiptAddress1.MaxLength = 250;
				colvarReceiptAddress1.AutoIncrement = false;
				colvarReceiptAddress1.IsNullable = true;
				colvarReceiptAddress1.IsPrimaryKey = false;
				colvarReceiptAddress1.IsForeignKey = false;
				colvarReceiptAddress1.IsReadOnly = false;
				colvarReceiptAddress1.DefaultSetting = @"";
				colvarReceiptAddress1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReceiptAddress1);
				
				TableSchema.TableColumn colvarReceiptAddress2 = new TableSchema.TableColumn(schema);
				colvarReceiptAddress2.ColumnName = "receiptAddress2";
				colvarReceiptAddress2.DataType = DbType.String;
				colvarReceiptAddress2.MaxLength = 250;
				colvarReceiptAddress2.AutoIncrement = false;
				colvarReceiptAddress2.IsNullable = true;
				colvarReceiptAddress2.IsPrimaryKey = false;
				colvarReceiptAddress2.IsForeignKey = false;
				colvarReceiptAddress2.IsReadOnly = false;
				colvarReceiptAddress2.DefaultSetting = @"";
				colvarReceiptAddress2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReceiptAddress2);
				
				TableSchema.TableColumn colvarReceiptAddress3 = new TableSchema.TableColumn(schema);
				colvarReceiptAddress3.ColumnName = "receiptAddress3";
				colvarReceiptAddress3.DataType = DbType.String;
				colvarReceiptAddress3.MaxLength = 250;
				colvarReceiptAddress3.AutoIncrement = false;
				colvarReceiptAddress3.IsNullable = true;
				colvarReceiptAddress3.IsPrimaryKey = false;
				colvarReceiptAddress3.IsForeignKey = false;
				colvarReceiptAddress3.IsReadOnly = false;
				colvarReceiptAddress3.DefaultSetting = @"";
				colvarReceiptAddress3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReceiptAddress3);
				
				TableSchema.TableColumn colvarReceiptAddress4 = new TableSchema.TableColumn(schema);
				colvarReceiptAddress4.ColumnName = "receiptAddress4";
				colvarReceiptAddress4.DataType = DbType.String;
				colvarReceiptAddress4.MaxLength = 250;
				colvarReceiptAddress4.AutoIncrement = false;
				colvarReceiptAddress4.IsNullable = true;
				colvarReceiptAddress4.IsPrimaryKey = false;
				colvarReceiptAddress4.IsForeignKey = false;
				colvarReceiptAddress4.IsReadOnly = false;
				colvarReceiptAddress4.DefaultSetting = @"";
				colvarReceiptAddress4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReceiptAddress4);
				
				TableSchema.TableColumn colvarShowMembershipInfo = new TableSchema.TableColumn(schema);
				colvarShowMembershipInfo.ColumnName = "showMembershipInfo";
				colvarShowMembershipInfo.DataType = DbType.Boolean;
				colvarShowMembershipInfo.MaxLength = 0;
				colvarShowMembershipInfo.AutoIncrement = false;
				colvarShowMembershipInfo.IsNullable = false;
				colvarShowMembershipInfo.IsPrimaryKey = false;
				colvarShowMembershipInfo.IsForeignKey = false;
				colvarShowMembershipInfo.IsReadOnly = false;
				colvarShowMembershipInfo.DefaultSetting = @"";
				colvarShowMembershipInfo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowMembershipInfo);
				
				TableSchema.TableColumn colvarShowSalesPersonInfo = new TableSchema.TableColumn(schema);
				colvarShowSalesPersonInfo.ColumnName = "showSalesPersonInfo";
				colvarShowSalesPersonInfo.DataType = DbType.Boolean;
				colvarShowSalesPersonInfo.MaxLength = 0;
				colvarShowSalesPersonInfo.AutoIncrement = false;
				colvarShowSalesPersonInfo.IsNullable = false;
				colvarShowSalesPersonInfo.IsPrimaryKey = false;
				colvarShowSalesPersonInfo.IsForeignKey = false;
				colvarShowSalesPersonInfo.IsReadOnly = false;
				colvarShowSalesPersonInfo.DefaultSetting = @"";
				colvarShowSalesPersonInfo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowSalesPersonInfo);
				
				TableSchema.TableColumn colvarSalesPersonTitle = new TableSchema.TableColumn(schema);
				colvarSalesPersonTitle.ColumnName = "SalesPersonTitle";
				colvarSalesPersonTitle.DataType = DbType.String;
				colvarSalesPersonTitle.MaxLength = 250;
				colvarSalesPersonTitle.AutoIncrement = false;
				colvarSalesPersonTitle.IsNullable = true;
				colvarSalesPersonTitle.IsPrimaryKey = false;
				colvarSalesPersonTitle.IsForeignKey = false;
				colvarSalesPersonTitle.IsReadOnly = false;
				colvarSalesPersonTitle.DefaultSetting = @"";
				colvarSalesPersonTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSalesPersonTitle);
				
				TableSchema.TableColumn colvarTermCondition1 = new TableSchema.TableColumn(schema);
				colvarTermCondition1.ColumnName = "TermCondition1";
				colvarTermCondition1.DataType = DbType.String;
				colvarTermCondition1.MaxLength = 250;
				colvarTermCondition1.AutoIncrement = false;
				colvarTermCondition1.IsNullable = true;
				colvarTermCondition1.IsPrimaryKey = false;
				colvarTermCondition1.IsForeignKey = false;
				colvarTermCondition1.IsReadOnly = false;
				colvarTermCondition1.DefaultSetting = @"";
				colvarTermCondition1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTermCondition1);
				
				TableSchema.TableColumn colvarTermCondition2 = new TableSchema.TableColumn(schema);
				colvarTermCondition2.ColumnName = "TermCondition2";
				colvarTermCondition2.DataType = DbType.String;
				colvarTermCondition2.MaxLength = 250;
				colvarTermCondition2.AutoIncrement = false;
				colvarTermCondition2.IsNullable = true;
				colvarTermCondition2.IsPrimaryKey = false;
				colvarTermCondition2.IsForeignKey = false;
				colvarTermCondition2.IsReadOnly = false;
				colvarTermCondition2.DefaultSetting = @"";
				colvarTermCondition2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTermCondition2);
				
				TableSchema.TableColumn colvarTermCondition3 = new TableSchema.TableColumn(schema);
				colvarTermCondition3.ColumnName = "TermCondition3";
				colvarTermCondition3.DataType = DbType.String;
				colvarTermCondition3.MaxLength = 250;
				colvarTermCondition3.AutoIncrement = false;
				colvarTermCondition3.IsNullable = true;
				colvarTermCondition3.IsPrimaryKey = false;
				colvarTermCondition3.IsForeignKey = false;
				colvarTermCondition3.IsReadOnly = false;
				colvarTermCondition3.DefaultSetting = @"";
				colvarTermCondition3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTermCondition3);
				
				TableSchema.TableColumn colvarTermCondition4 = new TableSchema.TableColumn(schema);
				colvarTermCondition4.ColumnName = "TermCondition4";
				colvarTermCondition4.DataType = DbType.String;
				colvarTermCondition4.MaxLength = 250;
				colvarTermCondition4.AutoIncrement = false;
				colvarTermCondition4.IsNullable = true;
				colvarTermCondition4.IsPrimaryKey = false;
				colvarTermCondition4.IsForeignKey = false;
				colvarTermCondition4.IsReadOnly = false;
				colvarTermCondition4.DefaultSetting = @"";
				colvarTermCondition4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTermCondition4);
				
				TableSchema.TableColumn colvarTermCondition5 = new TableSchema.TableColumn(schema);
				colvarTermCondition5.ColumnName = "TermCondition5";
				colvarTermCondition5.DataType = DbType.String;
				colvarTermCondition5.MaxLength = 250;
				colvarTermCondition5.AutoIncrement = false;
				colvarTermCondition5.IsNullable = true;
				colvarTermCondition5.IsPrimaryKey = false;
				colvarTermCondition5.IsForeignKey = false;
				colvarTermCondition5.IsReadOnly = false;
				colvarTermCondition5.DefaultSetting = @"";
				colvarTermCondition5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTermCondition5);
				
				TableSchema.TableColumn colvarTermCondition6 = new TableSchema.TableColumn(schema);
				colvarTermCondition6.ColumnName = "TermCondition6";
				colvarTermCondition6.DataType = DbType.String;
				colvarTermCondition6.MaxLength = 250;
				colvarTermCondition6.AutoIncrement = false;
				colvarTermCondition6.IsNullable = true;
				colvarTermCondition6.IsPrimaryKey = false;
				colvarTermCondition6.IsForeignKey = false;
				colvarTermCondition6.IsReadOnly = false;
				colvarTermCondition6.DefaultSetting = @"";
				colvarTermCondition6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTermCondition6);
				
				TableSchema.TableColumn colvarNumOfCopies = new TableSchema.TableColumn(schema);
				colvarNumOfCopies.ColumnName = "NumOfCopies";
				colvarNumOfCopies.DataType = DbType.Int32;
				colvarNumOfCopies.MaxLength = 0;
				colvarNumOfCopies.AutoIncrement = false;
				colvarNumOfCopies.IsNullable = true;
				colvarNumOfCopies.IsPrimaryKey = false;
				colvarNumOfCopies.IsForeignKey = false;
				colvarNumOfCopies.IsReadOnly = false;
				colvarNumOfCopies.DefaultSetting = @"";
				colvarNumOfCopies.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNumOfCopies);
				
				TableSchema.TableColumn colvarPaperSize = new TableSchema.TableColumn(schema);
				colvarPaperSize.ColumnName = "PaperSize";
				colvarPaperSize.DataType = DbType.Int32;
				colvarPaperSize.MaxLength = 0;
				colvarPaperSize.AutoIncrement = false;
				colvarPaperSize.IsNullable = true;
				colvarPaperSize.IsPrimaryKey = false;
				colvarPaperSize.IsForeignKey = false;
				colvarPaperSize.IsReadOnly = false;
				colvarPaperSize.DefaultSetting = @"";
				colvarPaperSize.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPaperSize);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("ReceiptSetting",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("SettingId")]
		[Bindable(true)]
		public Guid SettingId 
		{
			get { return GetColumnValue<Guid>(Columns.SettingId); }
			set { SetColumnValue(Columns.SettingId, value); }
		}
		  
		[XmlAttribute("SettingRemark")]
		[Bindable(true)]
		public string SettingRemark 
		{
			get { return GetColumnValue<string>(Columns.SettingRemark); }
			set { SetColumnValue(Columns.SettingRemark, value); }
		}
		  
		[XmlAttribute("PrintReceipt")]
		[Bindable(true)]
		public bool PrintReceipt 
		{
			get { return GetColumnValue<bool>(Columns.PrintReceipt); }
			set { SetColumnValue(Columns.PrintReceipt, value); }
		}
		  
		[XmlAttribute("UseOutletAddress")]
		[Bindable(true)]
		public bool UseOutletAddress 
		{
			get { return GetColumnValue<bool>(Columns.UseOutletAddress); }
			set { SetColumnValue(Columns.UseOutletAddress, value); }
		}
		  
		[XmlAttribute("ReceiptAddress1")]
		[Bindable(true)]
		public string ReceiptAddress1 
		{
			get { return GetColumnValue<string>(Columns.ReceiptAddress1); }
			set { SetColumnValue(Columns.ReceiptAddress1, value); }
		}
		  
		[XmlAttribute("ReceiptAddress2")]
		[Bindable(true)]
		public string ReceiptAddress2 
		{
			get { return GetColumnValue<string>(Columns.ReceiptAddress2); }
			set { SetColumnValue(Columns.ReceiptAddress2, value); }
		}
		  
		[XmlAttribute("ReceiptAddress3")]
		[Bindable(true)]
		public string ReceiptAddress3 
		{
			get { return GetColumnValue<string>(Columns.ReceiptAddress3); }
			set { SetColumnValue(Columns.ReceiptAddress3, value); }
		}
		  
		[XmlAttribute("ReceiptAddress4")]
		[Bindable(true)]
		public string ReceiptAddress4 
		{
			get { return GetColumnValue<string>(Columns.ReceiptAddress4); }
			set { SetColumnValue(Columns.ReceiptAddress4, value); }
		}
		  
		[XmlAttribute("ShowMembershipInfo")]
		[Bindable(true)]
		public bool ShowMembershipInfo 
		{
			get { return GetColumnValue<bool>(Columns.ShowMembershipInfo); }
			set { SetColumnValue(Columns.ShowMembershipInfo, value); }
		}
		  
		[XmlAttribute("ShowSalesPersonInfo")]
		[Bindable(true)]
		public bool ShowSalesPersonInfo 
		{
			get { return GetColumnValue<bool>(Columns.ShowSalesPersonInfo); }
			set { SetColumnValue(Columns.ShowSalesPersonInfo, value); }
		}
		  
		[XmlAttribute("SalesPersonTitle")]
		[Bindable(true)]
		public string SalesPersonTitle 
		{
			get { return GetColumnValue<string>(Columns.SalesPersonTitle); }
			set { SetColumnValue(Columns.SalesPersonTitle, value); }
		}
		  
		[XmlAttribute("TermCondition1")]
		[Bindable(true)]
		public string TermCondition1 
		{
			get { return GetColumnValue<string>(Columns.TermCondition1); }
			set { SetColumnValue(Columns.TermCondition1, value); }
		}
		  
		[XmlAttribute("TermCondition2")]
		[Bindable(true)]
		public string TermCondition2 
		{
			get { return GetColumnValue<string>(Columns.TermCondition2); }
			set { SetColumnValue(Columns.TermCondition2, value); }
		}
		  
		[XmlAttribute("TermCondition3")]
		[Bindable(true)]
		public string TermCondition3 
		{
			get { return GetColumnValue<string>(Columns.TermCondition3); }
			set { SetColumnValue(Columns.TermCondition3, value); }
		}
		  
		[XmlAttribute("TermCondition4")]
		[Bindable(true)]
		public string TermCondition4 
		{
			get { return GetColumnValue<string>(Columns.TermCondition4); }
			set { SetColumnValue(Columns.TermCondition4, value); }
		}
		  
		[XmlAttribute("TermCondition5")]
		[Bindable(true)]
		public string TermCondition5 
		{
			get { return GetColumnValue<string>(Columns.TermCondition5); }
			set { SetColumnValue(Columns.TermCondition5, value); }
		}
		  
		[XmlAttribute("TermCondition6")]
		[Bindable(true)]
		public string TermCondition6 
		{
			get { return GetColumnValue<string>(Columns.TermCondition6); }
			set { SetColumnValue(Columns.TermCondition6, value); }
		}
		  
		[XmlAttribute("NumOfCopies")]
		[Bindable(true)]
		public int? NumOfCopies 
		{
			get { return GetColumnValue<int?>(Columns.NumOfCopies); }
			set { SetColumnValue(Columns.NumOfCopies, value); }
		}
		  
		[XmlAttribute("PaperSize")]
		[Bindable(true)]
		public int? PaperSize 
		{
			get { return GetColumnValue<int?>(Columns.PaperSize); }
			set { SetColumnValue(Columns.PaperSize, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
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
		public static void Insert(Guid varSettingId,string varSettingRemark,bool varPrintReceipt,bool varUseOutletAddress,string varReceiptAddress1,string varReceiptAddress2,string varReceiptAddress3,string varReceiptAddress4,bool varShowMembershipInfo,bool varShowSalesPersonInfo,string varSalesPersonTitle,string varTermCondition1,string varTermCondition2,string varTermCondition3,string varTermCondition4,string varTermCondition5,string varTermCondition6,int? varNumOfCopies,int? varPaperSize,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			ReceiptSetting item = new ReceiptSetting();
			
			item.SettingId = varSettingId;
			
			item.SettingRemark = varSettingRemark;
			
			item.PrintReceipt = varPrintReceipt;
			
			item.UseOutletAddress = varUseOutletAddress;
			
			item.ReceiptAddress1 = varReceiptAddress1;
			
			item.ReceiptAddress2 = varReceiptAddress2;
			
			item.ReceiptAddress3 = varReceiptAddress3;
			
			item.ReceiptAddress4 = varReceiptAddress4;
			
			item.ShowMembershipInfo = varShowMembershipInfo;
			
			item.ShowSalesPersonInfo = varShowSalesPersonInfo;
			
			item.SalesPersonTitle = varSalesPersonTitle;
			
			item.TermCondition1 = varTermCondition1;
			
			item.TermCondition2 = varTermCondition2;
			
			item.TermCondition3 = varTermCondition3;
			
			item.TermCondition4 = varTermCondition4;
			
			item.TermCondition5 = varTermCondition5;
			
			item.TermCondition6 = varTermCondition6;
			
			item.NumOfCopies = varNumOfCopies;
			
			item.PaperSize = varPaperSize;
			
			item.Deleted = varDeleted;
			
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
		public static void Update(Guid varSettingId,string varSettingRemark,bool varPrintReceipt,bool varUseOutletAddress,string varReceiptAddress1,string varReceiptAddress2,string varReceiptAddress3,string varReceiptAddress4,bool varShowMembershipInfo,bool varShowSalesPersonInfo,string varSalesPersonTitle,string varTermCondition1,string varTermCondition2,string varTermCondition3,string varTermCondition4,string varTermCondition5,string varTermCondition6,int? varNumOfCopies,int? varPaperSize,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			ReceiptSetting item = new ReceiptSetting();
			
				item.SettingId = varSettingId;
			
				item.SettingRemark = varSettingRemark;
			
				item.PrintReceipt = varPrintReceipt;
			
				item.UseOutletAddress = varUseOutletAddress;
			
				item.ReceiptAddress1 = varReceiptAddress1;
			
				item.ReceiptAddress2 = varReceiptAddress2;
			
				item.ReceiptAddress3 = varReceiptAddress3;
			
				item.ReceiptAddress4 = varReceiptAddress4;
			
				item.ShowMembershipInfo = varShowMembershipInfo;
			
				item.ShowSalesPersonInfo = varShowSalesPersonInfo;
			
				item.SalesPersonTitle = varSalesPersonTitle;
			
				item.TermCondition1 = varTermCondition1;
			
				item.TermCondition2 = varTermCondition2;
			
				item.TermCondition3 = varTermCondition3;
			
				item.TermCondition4 = varTermCondition4;
			
				item.TermCondition5 = varTermCondition5;
			
				item.TermCondition6 = varTermCondition6;
			
				item.NumOfCopies = varNumOfCopies;
			
				item.PaperSize = varPaperSize;
			
				item.Deleted = varDeleted;
			
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
        
        
        public static TableSchema.TableColumn SettingIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn SettingRemarkColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn PrintReceiptColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn UseOutletAddressColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ReceiptAddress1Column
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ReceiptAddress2Column
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ReceiptAddress3Column
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ReceiptAddress4Column
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowMembershipInfoColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowSalesPersonInfoColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn SalesPersonTitleColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn TermCondition1Column
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn TermCondition2Column
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn TermCondition3Column
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn TermCondition4Column
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn TermCondition5Column
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn TermCondition6Column
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn NumOfCopiesColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn PaperSizeColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string SettingId = @"settingId";
			 public static string SettingRemark = @"settingRemark";
			 public static string PrintReceipt = @"printReceipt";
			 public static string UseOutletAddress = @"useOutletAddress";
			 public static string ReceiptAddress1 = @"receiptAddress1";
			 public static string ReceiptAddress2 = @"receiptAddress2";
			 public static string ReceiptAddress3 = @"receiptAddress3";
			 public static string ReceiptAddress4 = @"receiptAddress4";
			 public static string ShowMembershipInfo = @"showMembershipInfo";
			 public static string ShowSalesPersonInfo = @"showSalesPersonInfo";
			 public static string SalesPersonTitle = @"SalesPersonTitle";
			 public static string TermCondition1 = @"TermCondition1";
			 public static string TermCondition2 = @"TermCondition2";
			 public static string TermCondition3 = @"TermCondition3";
			 public static string TermCondition4 = @"TermCondition4";
			 public static string TermCondition5 = @"TermCondition5";
			 public static string TermCondition6 = @"TermCondition6";
			 public static string NumOfCopies = @"NumOfCopies";
			 public static string PaperSize = @"PaperSize";
			 public static string Deleted = @"Deleted";
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
