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
	/// Strongly-typed collection for the TabletCollection class.
	/// </summary>
    [Serializable]
	public partial class TabletCollectionCollection : ActiveList<TabletCollection, TabletCollectionCollection>
	{	   
		public TabletCollectionCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TabletCollectionCollection</returns>
		public TabletCollectionCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TabletCollection o = this[i];
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
	/// This is an ActiveRecord class which wraps the TabletCollection table.
	/// </summary>
	[Serializable]
	public partial class TabletCollection : ActiveRecord<TabletCollection>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TabletCollection()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TabletCollection(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TabletCollection(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TabletCollection(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("TabletCollection", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarTabletCollectionID = new TableSchema.TableColumn(schema);
				colvarTabletCollectionID.ColumnName = "TabletCollectionID";
				colvarTabletCollectionID.DataType = DbType.Int32;
				colvarTabletCollectionID.MaxLength = 0;
				colvarTabletCollectionID.AutoIncrement = true;
				colvarTabletCollectionID.IsNullable = false;
				colvarTabletCollectionID.IsPrimaryKey = true;
				colvarTabletCollectionID.IsForeignKey = false;
				colvarTabletCollectionID.IsReadOnly = false;
				colvarTabletCollectionID.DefaultSetting = @"";
				colvarTabletCollectionID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTabletCollectionID);
				
				TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
				colvarPointOfSaleID.ColumnName = "PointOfSaleID";
				colvarPointOfSaleID.DataType = DbType.Int32;
				colvarPointOfSaleID.MaxLength = 0;
				colvarPointOfSaleID.AutoIncrement = false;
				colvarPointOfSaleID.IsNullable = false;
				colvarPointOfSaleID.IsPrimaryKey = false;
				colvarPointOfSaleID.IsForeignKey = false;
				colvarPointOfSaleID.IsReadOnly = false;
				colvarPointOfSaleID.DefaultSetting = @"";
				colvarPointOfSaleID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointOfSaleID);
				
				TableSchema.TableColumn colvarTerminalID = new TableSchema.TableColumn(schema);
				colvarTerminalID.ColumnName = "TerminalID";
				colvarTerminalID.DataType = DbType.Int32;
				colvarTerminalID.MaxLength = 0;
				colvarTerminalID.AutoIncrement = false;
				colvarTerminalID.IsNullable = false;
				colvarTerminalID.IsPrimaryKey = false;
				colvarTerminalID.IsForeignKey = false;
				colvarTerminalID.IsReadOnly = false;
				colvarTerminalID.DefaultSetting = @"";
				colvarTerminalID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTerminalID);
				
				TableSchema.TableColumn colvarStatus = new TableSchema.TableColumn(schema);
				colvarStatus.ColumnName = "Status";
				colvarStatus.DataType = DbType.AnsiString;
				colvarStatus.MaxLength = 50;
				colvarStatus.AutoIncrement = false;
				colvarStatus.IsNullable = false;
				colvarStatus.IsPrimaryKey = false;
				colvarStatus.IsForeignKey = false;
				colvarStatus.IsReadOnly = false;
				colvarStatus.DefaultSetting = @"";
				colvarStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatus);
				
				TableSchema.TableColumn colvarOpenTime = new TableSchema.TableColumn(schema);
				colvarOpenTime.ColumnName = "OpenTime";
				colvarOpenTime.DataType = DbType.DateTime;
				colvarOpenTime.MaxLength = 0;
				colvarOpenTime.AutoIncrement = false;
				colvarOpenTime.IsNullable = true;
				colvarOpenTime.IsPrimaryKey = false;
				colvarOpenTime.IsForeignKey = false;
				colvarOpenTime.IsReadOnly = false;
				colvarOpenTime.DefaultSetting = @"";
				colvarOpenTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOpenTime);
				
				TableSchema.TableColumn colvarCloseTime = new TableSchema.TableColumn(schema);
				colvarCloseTime.ColumnName = "CloseTime";
				colvarCloseTime.DataType = DbType.DateTime;
				colvarCloseTime.MaxLength = 0;
				colvarCloseTime.AutoIncrement = false;
				colvarCloseTime.IsNullable = true;
				colvarCloseTime.IsPrimaryKey = false;
				colvarCloseTime.IsForeignKey = false;
				colvarCloseTime.IsReadOnly = false;
				colvarCloseTime.DefaultSetting = @"";
				colvarCloseTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCloseTime);
				
				TableSchema.TableColumn colvarOpenBy = new TableSchema.TableColumn(schema);
				colvarOpenBy.ColumnName = "OpenBy";
				colvarOpenBy.DataType = DbType.AnsiString;
				colvarOpenBy.MaxLength = 50;
				colvarOpenBy.AutoIncrement = false;
				colvarOpenBy.IsNullable = true;
				colvarOpenBy.IsPrimaryKey = false;
				colvarOpenBy.IsForeignKey = false;
				colvarOpenBy.IsReadOnly = false;
				colvarOpenBy.DefaultSetting = @"";
				colvarOpenBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOpenBy);
				
				TableSchema.TableColumn colvarCloseBy = new TableSchema.TableColumn(schema);
				colvarCloseBy.ColumnName = "CloseBy";
				colvarCloseBy.DataType = DbType.AnsiString;
				colvarCloseBy.MaxLength = 50;
				colvarCloseBy.AutoIncrement = false;
				colvarCloseBy.IsNullable = true;
				colvarCloseBy.IsPrimaryKey = false;
				colvarCloseBy.IsForeignKey = false;
				colvarCloseBy.IsReadOnly = false;
				colvarCloseBy.DefaultSetting = @"";
				colvarCloseBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCloseBy);
				
				TableSchema.TableColumn colvarSupervisor = new TableSchema.TableColumn(schema);
				colvarSupervisor.ColumnName = "Supervisor";
				colvarSupervisor.DataType = DbType.AnsiString;
				colvarSupervisor.MaxLength = 50;
				colvarSupervisor.AutoIncrement = false;
				colvarSupervisor.IsNullable = true;
				colvarSupervisor.IsPrimaryKey = false;
				colvarSupervisor.IsForeignKey = false;
				colvarSupervisor.IsReadOnly = false;
				colvarSupervisor.DefaultSetting = @"";
				colvarSupervisor.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSupervisor);
				
				TableSchema.TableColumn colvarCashRecorded = new TableSchema.TableColumn(schema);
				colvarCashRecorded.ColumnName = "CashRecorded";
				colvarCashRecorded.DataType = DbType.Currency;
				colvarCashRecorded.MaxLength = 0;
				colvarCashRecorded.AutoIncrement = false;
				colvarCashRecorded.IsNullable = true;
				colvarCashRecorded.IsPrimaryKey = false;
				colvarCashRecorded.IsForeignKey = false;
				colvarCashRecorded.IsReadOnly = false;
				colvarCashRecorded.DefaultSetting = @"";
				colvarCashRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCashRecorded);
				
				TableSchema.TableColumn colvarPay1Recorded = new TableSchema.TableColumn(schema);
				colvarPay1Recorded.ColumnName = "Pay1Recorded";
				colvarPay1Recorded.DataType = DbType.Currency;
				colvarPay1Recorded.MaxLength = 0;
				colvarPay1Recorded.AutoIncrement = false;
				colvarPay1Recorded.IsNullable = true;
				colvarPay1Recorded.IsPrimaryKey = false;
				colvarPay1Recorded.IsForeignKey = false;
				colvarPay1Recorded.IsReadOnly = false;
				colvarPay1Recorded.DefaultSetting = @"";
				colvarPay1Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay1Recorded);
				
				TableSchema.TableColumn colvarPay2Recorded = new TableSchema.TableColumn(schema);
				colvarPay2Recorded.ColumnName = "Pay2Recorded";
				colvarPay2Recorded.DataType = DbType.Currency;
				colvarPay2Recorded.MaxLength = 0;
				colvarPay2Recorded.AutoIncrement = false;
				colvarPay2Recorded.IsNullable = true;
				colvarPay2Recorded.IsPrimaryKey = false;
				colvarPay2Recorded.IsForeignKey = false;
				colvarPay2Recorded.IsReadOnly = false;
				colvarPay2Recorded.DefaultSetting = @"";
				colvarPay2Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay2Recorded);
				
				TableSchema.TableColumn colvarPay3Recorded = new TableSchema.TableColumn(schema);
				colvarPay3Recorded.ColumnName = "Pay3Recorded";
				colvarPay3Recorded.DataType = DbType.Currency;
				colvarPay3Recorded.MaxLength = 0;
				colvarPay3Recorded.AutoIncrement = false;
				colvarPay3Recorded.IsNullable = true;
				colvarPay3Recorded.IsPrimaryKey = false;
				colvarPay3Recorded.IsForeignKey = false;
				colvarPay3Recorded.IsReadOnly = false;
				colvarPay3Recorded.DefaultSetting = @"";
				colvarPay3Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay3Recorded);
				
				TableSchema.TableColumn colvarPay4Recorded = new TableSchema.TableColumn(schema);
				colvarPay4Recorded.ColumnName = "Pay4Recorded";
				colvarPay4Recorded.DataType = DbType.Currency;
				colvarPay4Recorded.MaxLength = 0;
				colvarPay4Recorded.AutoIncrement = false;
				colvarPay4Recorded.IsNullable = true;
				colvarPay4Recorded.IsPrimaryKey = false;
				colvarPay4Recorded.IsForeignKey = false;
				colvarPay4Recorded.IsReadOnly = false;
				colvarPay4Recorded.DefaultSetting = @"";
				colvarPay4Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay4Recorded);
				
				TableSchema.TableColumn colvarPay5Recorded = new TableSchema.TableColumn(schema);
				colvarPay5Recorded.ColumnName = "Pay5Recorded";
				colvarPay5Recorded.DataType = DbType.Currency;
				colvarPay5Recorded.MaxLength = 0;
				colvarPay5Recorded.AutoIncrement = false;
				colvarPay5Recorded.IsNullable = true;
				colvarPay5Recorded.IsPrimaryKey = false;
				colvarPay5Recorded.IsForeignKey = false;
				colvarPay5Recorded.IsReadOnly = false;
				colvarPay5Recorded.DefaultSetting = @"";
				colvarPay5Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay5Recorded);
				
				TableSchema.TableColumn colvarPay6Recorded = new TableSchema.TableColumn(schema);
				colvarPay6Recorded.ColumnName = "Pay6Recorded";
				colvarPay6Recorded.DataType = DbType.Currency;
				colvarPay6Recorded.MaxLength = 0;
				colvarPay6Recorded.AutoIncrement = false;
				colvarPay6Recorded.IsNullable = true;
				colvarPay6Recorded.IsPrimaryKey = false;
				colvarPay6Recorded.IsForeignKey = false;
				colvarPay6Recorded.IsReadOnly = false;
				colvarPay6Recorded.DefaultSetting = @"";
				colvarPay6Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay6Recorded);
				
				TableSchema.TableColumn colvarPay7Recorded = new TableSchema.TableColumn(schema);
				colvarPay7Recorded.ColumnName = "Pay7Recorded";
				colvarPay7Recorded.DataType = DbType.Currency;
				colvarPay7Recorded.MaxLength = 0;
				colvarPay7Recorded.AutoIncrement = false;
				colvarPay7Recorded.IsNullable = true;
				colvarPay7Recorded.IsPrimaryKey = false;
				colvarPay7Recorded.IsForeignKey = false;
				colvarPay7Recorded.IsReadOnly = false;
				colvarPay7Recorded.DefaultSetting = @"";
				colvarPay7Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay7Recorded);
				
				TableSchema.TableColumn colvarPay8Recorded = new TableSchema.TableColumn(schema);
				colvarPay8Recorded.ColumnName = "Pay8Recorded";
				colvarPay8Recorded.DataType = DbType.Currency;
				colvarPay8Recorded.MaxLength = 0;
				colvarPay8Recorded.AutoIncrement = false;
				colvarPay8Recorded.IsNullable = true;
				colvarPay8Recorded.IsPrimaryKey = false;
				colvarPay8Recorded.IsForeignKey = false;
				colvarPay8Recorded.IsReadOnly = false;
				colvarPay8Recorded.DefaultSetting = @"";
				colvarPay8Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay8Recorded);
				
				TableSchema.TableColumn colvarPay9Recorded = new TableSchema.TableColumn(schema);
				colvarPay9Recorded.ColumnName = "Pay9Recorded";
				colvarPay9Recorded.DataType = DbType.Currency;
				colvarPay9Recorded.MaxLength = 0;
				colvarPay9Recorded.AutoIncrement = false;
				colvarPay9Recorded.IsNullable = true;
				colvarPay9Recorded.IsPrimaryKey = false;
				colvarPay9Recorded.IsForeignKey = false;
				colvarPay9Recorded.IsReadOnly = false;
				colvarPay9Recorded.DefaultSetting = @"";
				colvarPay9Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay9Recorded);
				
				TableSchema.TableColumn colvarPay10Recorded = new TableSchema.TableColumn(schema);
				colvarPay10Recorded.ColumnName = "Pay10Recorded";
				colvarPay10Recorded.DataType = DbType.Currency;
				colvarPay10Recorded.MaxLength = 0;
				colvarPay10Recorded.AutoIncrement = false;
				colvarPay10Recorded.IsNullable = true;
				colvarPay10Recorded.IsPrimaryKey = false;
				colvarPay10Recorded.IsForeignKey = false;
				colvarPay10Recorded.IsReadOnly = false;
				colvarPay10Recorded.DefaultSetting = @"";
				colvarPay10Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay10Recorded);
				
				TableSchema.TableColumn colvarVoucherRecorded = new TableSchema.TableColumn(schema);
				colvarVoucherRecorded.ColumnName = "VoucherRecorded";
				colvarVoucherRecorded.DataType = DbType.Currency;
				colvarVoucherRecorded.MaxLength = 0;
				colvarVoucherRecorded.AutoIncrement = false;
				colvarVoucherRecorded.IsNullable = true;
				colvarVoucherRecorded.IsPrimaryKey = false;
				colvarVoucherRecorded.IsForeignKey = false;
				colvarVoucherRecorded.IsReadOnly = false;
				colvarVoucherRecorded.DefaultSetting = @"";
				colvarVoucherRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVoucherRecorded);
				
				TableSchema.TableColumn colvarChequeRecorded = new TableSchema.TableColumn(schema);
				colvarChequeRecorded.ColumnName = "ChequeRecorded";
				colvarChequeRecorded.DataType = DbType.Currency;
				colvarChequeRecorded.MaxLength = 0;
				colvarChequeRecorded.AutoIncrement = false;
				colvarChequeRecorded.IsNullable = true;
				colvarChequeRecorded.IsPrimaryKey = false;
				colvarChequeRecorded.IsForeignKey = false;
				colvarChequeRecorded.IsReadOnly = false;
				colvarChequeRecorded.DefaultSetting = @"";
				colvarChequeRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChequeRecorded);
				
				TableSchema.TableColumn colvarPointRecorded = new TableSchema.TableColumn(schema);
				colvarPointRecorded.ColumnName = "PointRecorded";
				colvarPointRecorded.DataType = DbType.Currency;
				colvarPointRecorded.MaxLength = 0;
				colvarPointRecorded.AutoIncrement = false;
				colvarPointRecorded.IsNullable = true;
				colvarPointRecorded.IsPrimaryKey = false;
				colvarPointRecorded.IsForeignKey = false;
				colvarPointRecorded.IsReadOnly = false;
				colvarPointRecorded.DefaultSetting = @"";
				colvarPointRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointRecorded);
				
				TableSchema.TableColumn colvarPackageRecorded = new TableSchema.TableColumn(schema);
				colvarPackageRecorded.ColumnName = "PackageRecorded";
				colvarPackageRecorded.DataType = DbType.Currency;
				colvarPackageRecorded.MaxLength = 0;
				colvarPackageRecorded.AutoIncrement = false;
				colvarPackageRecorded.IsNullable = true;
				colvarPackageRecorded.IsPrimaryKey = false;
				colvarPackageRecorded.IsForeignKey = false;
				colvarPackageRecorded.IsReadOnly = false;
				colvarPackageRecorded.DefaultSetting = @"";
				colvarPackageRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackageRecorded);
				
				TableSchema.TableColumn colvarSMFRecorded = new TableSchema.TableColumn(schema);
				colvarSMFRecorded.ColumnName = "SMFRecorded";
				colvarSMFRecorded.DataType = DbType.Currency;
				colvarSMFRecorded.MaxLength = 0;
				colvarSMFRecorded.AutoIncrement = false;
				colvarSMFRecorded.IsNullable = true;
				colvarSMFRecorded.IsPrimaryKey = false;
				colvarSMFRecorded.IsForeignKey = false;
				colvarSMFRecorded.IsReadOnly = false;
				colvarSMFRecorded.DefaultSetting = @"";
				colvarSMFRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSMFRecorded);
				
				TableSchema.TableColumn colvarPAMedRecorded = new TableSchema.TableColumn(schema);
				colvarPAMedRecorded.ColumnName = "PAMedRecorded";
				colvarPAMedRecorded.DataType = DbType.Currency;
				colvarPAMedRecorded.MaxLength = 0;
				colvarPAMedRecorded.AutoIncrement = false;
				colvarPAMedRecorded.IsNullable = true;
				colvarPAMedRecorded.IsPrimaryKey = false;
				colvarPAMedRecorded.IsForeignKey = false;
				colvarPAMedRecorded.IsReadOnly = false;
				colvarPAMedRecorded.DefaultSetting = @"";
				colvarPAMedRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPAMedRecorded);
				
				TableSchema.TableColumn colvarPWFRecorded = new TableSchema.TableColumn(schema);
				colvarPWFRecorded.ColumnName = "PWFRecorded";
				colvarPWFRecorded.DataType = DbType.Currency;
				colvarPWFRecorded.MaxLength = 0;
				colvarPWFRecorded.AutoIncrement = false;
				colvarPWFRecorded.IsNullable = true;
				colvarPWFRecorded.IsPrimaryKey = false;
				colvarPWFRecorded.IsForeignKey = false;
				colvarPWFRecorded.IsReadOnly = false;
				colvarPWFRecorded.DefaultSetting = @"";
				colvarPWFRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPWFRecorded);
				
				TableSchema.TableColumn colvarNetsATMCardRecorded = new TableSchema.TableColumn(schema);
				colvarNetsATMCardRecorded.ColumnName = "NetsATMCardRecorded";
				colvarNetsATMCardRecorded.DataType = DbType.Currency;
				colvarNetsATMCardRecorded.MaxLength = 0;
				colvarNetsATMCardRecorded.AutoIncrement = false;
				colvarNetsATMCardRecorded.IsNullable = true;
				colvarNetsATMCardRecorded.IsPrimaryKey = false;
				colvarNetsATMCardRecorded.IsForeignKey = false;
				colvarNetsATMCardRecorded.IsReadOnly = false;
				colvarNetsATMCardRecorded.DefaultSetting = @"";
				colvarNetsATMCardRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNetsATMCardRecorded);
				
				TableSchema.TableColumn colvarNetsCashCardRecorded = new TableSchema.TableColumn(schema);
				colvarNetsCashCardRecorded.ColumnName = "NetsCashCardRecorded";
				colvarNetsCashCardRecorded.DataType = DbType.Currency;
				colvarNetsCashCardRecorded.MaxLength = 0;
				colvarNetsCashCardRecorded.AutoIncrement = false;
				colvarNetsCashCardRecorded.IsNullable = true;
				colvarNetsCashCardRecorded.IsPrimaryKey = false;
				colvarNetsCashCardRecorded.IsForeignKey = false;
				colvarNetsCashCardRecorded.IsReadOnly = false;
				colvarNetsCashCardRecorded.DefaultSetting = @"";
				colvarNetsCashCardRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNetsCashCardRecorded);
				
				TableSchema.TableColumn colvarNetsFlashPayRecorded = new TableSchema.TableColumn(schema);
				colvarNetsFlashPayRecorded.ColumnName = "NetsFlashPayRecorded";
				colvarNetsFlashPayRecorded.DataType = DbType.Currency;
				colvarNetsFlashPayRecorded.MaxLength = 0;
				colvarNetsFlashPayRecorded.AutoIncrement = false;
				colvarNetsFlashPayRecorded.IsNullable = true;
				colvarNetsFlashPayRecorded.IsPrimaryKey = false;
				colvarNetsFlashPayRecorded.IsForeignKey = false;
				colvarNetsFlashPayRecorded.IsReadOnly = false;
				colvarNetsFlashPayRecorded.DefaultSetting = @"";
				colvarNetsFlashPayRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNetsFlashPayRecorded);
				
				TableSchema.TableColumn colvarForeignCurrency1 = new TableSchema.TableColumn(schema);
				colvarForeignCurrency1.ColumnName = "ForeignCurrency1";
				colvarForeignCurrency1.DataType = DbType.String;
				colvarForeignCurrency1.MaxLength = 200;
				colvarForeignCurrency1.AutoIncrement = false;
				colvarForeignCurrency1.IsNullable = true;
				colvarForeignCurrency1.IsPrimaryKey = false;
				colvarForeignCurrency1.IsForeignKey = false;
				colvarForeignCurrency1.IsReadOnly = false;
				colvarForeignCurrency1.DefaultSetting = @"";
				colvarForeignCurrency1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency1);
				
				TableSchema.TableColumn colvarForeignCurrency1Recorded = new TableSchema.TableColumn(schema);
				colvarForeignCurrency1Recorded.ColumnName = "ForeignCurrency1Recorded";
				colvarForeignCurrency1Recorded.DataType = DbType.Currency;
				colvarForeignCurrency1Recorded.MaxLength = 0;
				colvarForeignCurrency1Recorded.AutoIncrement = false;
				colvarForeignCurrency1Recorded.IsNullable = true;
				colvarForeignCurrency1Recorded.IsPrimaryKey = false;
				colvarForeignCurrency1Recorded.IsForeignKey = false;
				colvarForeignCurrency1Recorded.IsReadOnly = false;
				colvarForeignCurrency1Recorded.DefaultSetting = @"";
				colvarForeignCurrency1Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency1Recorded);
				
				TableSchema.TableColumn colvarForeignCurrency2 = new TableSchema.TableColumn(schema);
				colvarForeignCurrency2.ColumnName = "ForeignCurrency2";
				colvarForeignCurrency2.DataType = DbType.String;
				colvarForeignCurrency2.MaxLength = 200;
				colvarForeignCurrency2.AutoIncrement = false;
				colvarForeignCurrency2.IsNullable = true;
				colvarForeignCurrency2.IsPrimaryKey = false;
				colvarForeignCurrency2.IsForeignKey = false;
				colvarForeignCurrency2.IsReadOnly = false;
				colvarForeignCurrency2.DefaultSetting = @"";
				colvarForeignCurrency2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency2);
				
				TableSchema.TableColumn colvarForeignCurrency2Recorded = new TableSchema.TableColumn(schema);
				colvarForeignCurrency2Recorded.ColumnName = "ForeignCurrency2Recorded";
				colvarForeignCurrency2Recorded.DataType = DbType.Currency;
				colvarForeignCurrency2Recorded.MaxLength = 0;
				colvarForeignCurrency2Recorded.AutoIncrement = false;
				colvarForeignCurrency2Recorded.IsNullable = true;
				colvarForeignCurrency2Recorded.IsPrimaryKey = false;
				colvarForeignCurrency2Recorded.IsForeignKey = false;
				colvarForeignCurrency2Recorded.IsReadOnly = false;
				colvarForeignCurrency2Recorded.DefaultSetting = @"";
				colvarForeignCurrency2Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency2Recorded);
				
				TableSchema.TableColumn colvarForeignCurrency3 = new TableSchema.TableColumn(schema);
				colvarForeignCurrency3.ColumnName = "ForeignCurrency3";
				colvarForeignCurrency3.DataType = DbType.String;
				colvarForeignCurrency3.MaxLength = 200;
				colvarForeignCurrency3.AutoIncrement = false;
				colvarForeignCurrency3.IsNullable = true;
				colvarForeignCurrency3.IsPrimaryKey = false;
				colvarForeignCurrency3.IsForeignKey = false;
				colvarForeignCurrency3.IsReadOnly = false;
				colvarForeignCurrency3.DefaultSetting = @"";
				colvarForeignCurrency3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency3);
				
				TableSchema.TableColumn colvarForeignCurrency3Recorded = new TableSchema.TableColumn(schema);
				colvarForeignCurrency3Recorded.ColumnName = "ForeignCurrency3Recorded";
				colvarForeignCurrency3Recorded.DataType = DbType.Currency;
				colvarForeignCurrency3Recorded.MaxLength = 0;
				colvarForeignCurrency3Recorded.AutoIncrement = false;
				colvarForeignCurrency3Recorded.IsNullable = true;
				colvarForeignCurrency3Recorded.IsPrimaryKey = false;
				colvarForeignCurrency3Recorded.IsForeignKey = false;
				colvarForeignCurrency3Recorded.IsReadOnly = false;
				colvarForeignCurrency3Recorded.DefaultSetting = @"";
				colvarForeignCurrency3Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency3Recorded);
				
				TableSchema.TableColumn colvarForeignCurrency4 = new TableSchema.TableColumn(schema);
				colvarForeignCurrency4.ColumnName = "ForeignCurrency4";
				colvarForeignCurrency4.DataType = DbType.String;
				colvarForeignCurrency4.MaxLength = 200;
				colvarForeignCurrency4.AutoIncrement = false;
				colvarForeignCurrency4.IsNullable = true;
				colvarForeignCurrency4.IsPrimaryKey = false;
				colvarForeignCurrency4.IsForeignKey = false;
				colvarForeignCurrency4.IsReadOnly = false;
				colvarForeignCurrency4.DefaultSetting = @"";
				colvarForeignCurrency4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency4);
				
				TableSchema.TableColumn colvarForeignCurrency4Recorded = new TableSchema.TableColumn(schema);
				colvarForeignCurrency4Recorded.ColumnName = "ForeignCurrency4Recorded";
				colvarForeignCurrency4Recorded.DataType = DbType.Currency;
				colvarForeignCurrency4Recorded.MaxLength = 0;
				colvarForeignCurrency4Recorded.AutoIncrement = false;
				colvarForeignCurrency4Recorded.IsNullable = true;
				colvarForeignCurrency4Recorded.IsPrimaryKey = false;
				colvarForeignCurrency4Recorded.IsForeignKey = false;
				colvarForeignCurrency4Recorded.IsReadOnly = false;
				colvarForeignCurrency4Recorded.DefaultSetting = @"";
				colvarForeignCurrency4Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency4Recorded);
				
				TableSchema.TableColumn colvarForeignCurrency5 = new TableSchema.TableColumn(schema);
				colvarForeignCurrency5.ColumnName = "ForeignCurrency5";
				colvarForeignCurrency5.DataType = DbType.String;
				colvarForeignCurrency5.MaxLength = 200;
				colvarForeignCurrency5.AutoIncrement = false;
				colvarForeignCurrency5.IsNullable = true;
				colvarForeignCurrency5.IsPrimaryKey = false;
				colvarForeignCurrency5.IsForeignKey = false;
				colvarForeignCurrency5.IsReadOnly = false;
				colvarForeignCurrency5.DefaultSetting = @"";
				colvarForeignCurrency5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency5);
				
				TableSchema.TableColumn colvarForeignCurrency5Recorded = new TableSchema.TableColumn(schema);
				colvarForeignCurrency5Recorded.ColumnName = "ForeignCurrency5Recorded";
				colvarForeignCurrency5Recorded.DataType = DbType.Currency;
				colvarForeignCurrency5Recorded.MaxLength = 0;
				colvarForeignCurrency5Recorded.AutoIncrement = false;
				colvarForeignCurrency5Recorded.IsNullable = true;
				colvarForeignCurrency5Recorded.IsPrimaryKey = false;
				colvarForeignCurrency5Recorded.IsForeignKey = false;
				colvarForeignCurrency5Recorded.IsReadOnly = false;
				colvarForeignCurrency5Recorded.DefaultSetting = @"";
				colvarForeignCurrency5Recorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency5Recorded);
				
				TableSchema.TableColumn colvarTotalForeignCurrency = new TableSchema.TableColumn(schema);
				colvarTotalForeignCurrency.ColumnName = "TotalForeignCurrency";
				colvarTotalForeignCurrency.DataType = DbType.Currency;
				colvarTotalForeignCurrency.MaxLength = 0;
				colvarTotalForeignCurrency.AutoIncrement = false;
				colvarTotalForeignCurrency.IsNullable = true;
				colvarTotalForeignCurrency.IsPrimaryKey = false;
				colvarTotalForeignCurrency.IsForeignKey = false;
				colvarTotalForeignCurrency.IsReadOnly = false;
				colvarTotalForeignCurrency.DefaultSetting = @"";
				colvarTotalForeignCurrency.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTotalForeignCurrency);
				
				TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
				colvarCreatedOn.ColumnName = "CreatedOn";
				colvarCreatedOn.DataType = DbType.DateTime;
				colvarCreatedOn.MaxLength = 0;
				colvarCreatedOn.AutoIncrement = false;
				colvarCreatedOn.IsNullable = false;
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
				colvarCreatedBy.IsNullable = false;
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
				colvarModifiedOn.IsNullable = false;
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
				colvarModifiedBy.IsNullable = false;
				colvarModifiedBy.IsPrimaryKey = false;
				colvarModifiedBy.IsForeignKey = false;
				colvarModifiedBy.IsReadOnly = false;
				colvarModifiedBy.DefaultSetting = @"";
				colvarModifiedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedBy);
				
				TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
				colvarUniqueID.ColumnName = "UniqueID";
				colvarUniqueID.DataType = DbType.Guid;
				colvarUniqueID.MaxLength = 0;
				colvarUniqueID.AutoIncrement = false;
				colvarUniqueID.IsNullable = false;
				colvarUniqueID.IsPrimaryKey = false;
				colvarUniqueID.IsForeignKey = false;
				colvarUniqueID.IsReadOnly = false;
				
						colvarUniqueID.DefaultSetting = @"(newid())";
				colvarUniqueID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueID);
				
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
				
				TableSchema.TableColumn colvarUserfld2 = new TableSchema.TableColumn(schema);
				colvarUserfld2.ColumnName = "userfld2";
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
				colvarUserfld3.ColumnName = "userfld3";
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
				colvarUserfld4.ColumnName = "userfld4";
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
				colvarUserfld5.ColumnName = "userfld5";
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
				colvarUserfld6.ColumnName = "userfld6";
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
				colvarUserfld7.ColumnName = "userfld7";
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
				colvarUserfld8.ColumnName = "userfld8";
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
				colvarUserfld9.ColumnName = "userfld9";
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
				colvarUserfld10.ColumnName = "userfld10";
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
				
				TableSchema.TableColumn colvarUserflag1 = new TableSchema.TableColumn(schema);
				colvarUserflag1.ColumnName = "userflag1";
				colvarUserflag1.DataType = DbType.Boolean;
				colvarUserflag1.MaxLength = 0;
				colvarUserflag1.AutoIncrement = false;
				colvarUserflag1.IsNullable = true;
				colvarUserflag1.IsPrimaryKey = false;
				colvarUserflag1.IsForeignKey = false;
				colvarUserflag1.IsReadOnly = false;
				colvarUserflag1.DefaultSetting = @"";
				colvarUserflag1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag1);
				
				TableSchema.TableColumn colvarUserflag2 = new TableSchema.TableColumn(schema);
				colvarUserflag2.ColumnName = "userflag2";
				colvarUserflag2.DataType = DbType.Boolean;
				colvarUserflag2.MaxLength = 0;
				colvarUserflag2.AutoIncrement = false;
				colvarUserflag2.IsNullable = true;
				colvarUserflag2.IsPrimaryKey = false;
				colvarUserflag2.IsForeignKey = false;
				colvarUserflag2.IsReadOnly = false;
				colvarUserflag2.DefaultSetting = @"";
				colvarUserflag2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag2);
				
				TableSchema.TableColumn colvarUserflag3 = new TableSchema.TableColumn(schema);
				colvarUserflag3.ColumnName = "userflag3";
				colvarUserflag3.DataType = DbType.Boolean;
				colvarUserflag3.MaxLength = 0;
				colvarUserflag3.AutoIncrement = false;
				colvarUserflag3.IsNullable = true;
				colvarUserflag3.IsPrimaryKey = false;
				colvarUserflag3.IsForeignKey = false;
				colvarUserflag3.IsReadOnly = false;
				colvarUserflag3.DefaultSetting = @"";
				colvarUserflag3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag3);
				
				TableSchema.TableColumn colvarUserflag4 = new TableSchema.TableColumn(schema);
				colvarUserflag4.ColumnName = "userflag4";
				colvarUserflag4.DataType = DbType.Boolean;
				colvarUserflag4.MaxLength = 0;
				colvarUserflag4.AutoIncrement = false;
				colvarUserflag4.IsNullable = true;
				colvarUserflag4.IsPrimaryKey = false;
				colvarUserflag4.IsForeignKey = false;
				colvarUserflag4.IsReadOnly = false;
				colvarUserflag4.DefaultSetting = @"";
				colvarUserflag4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag4);
				
				TableSchema.TableColumn colvarUserflag5 = new TableSchema.TableColumn(schema);
				colvarUserflag5.ColumnName = "userflag5";
				colvarUserflag5.DataType = DbType.Boolean;
				colvarUserflag5.MaxLength = 0;
				colvarUserflag5.AutoIncrement = false;
				colvarUserflag5.IsNullable = true;
				colvarUserflag5.IsPrimaryKey = false;
				colvarUserflag5.IsForeignKey = false;
				colvarUserflag5.IsReadOnly = false;
				colvarUserflag5.DefaultSetting = @"";
				colvarUserflag5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag5);
				
				TableSchema.TableColumn colvarUserfloat1 = new TableSchema.TableColumn(schema);
				colvarUserfloat1.ColumnName = "userfloat1";
				colvarUserfloat1.DataType = DbType.Currency;
				colvarUserfloat1.MaxLength = 0;
				colvarUserfloat1.AutoIncrement = false;
				colvarUserfloat1.IsNullable = true;
				colvarUserfloat1.IsPrimaryKey = false;
				colvarUserfloat1.IsForeignKey = false;
				colvarUserfloat1.IsReadOnly = false;
				colvarUserfloat1.DefaultSetting = @"";
				colvarUserfloat1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat1);
				
				TableSchema.TableColumn colvarUserfloat2 = new TableSchema.TableColumn(schema);
				colvarUserfloat2.ColumnName = "userfloat2";
				colvarUserfloat2.DataType = DbType.Currency;
				colvarUserfloat2.MaxLength = 0;
				colvarUserfloat2.AutoIncrement = false;
				colvarUserfloat2.IsNullable = true;
				colvarUserfloat2.IsPrimaryKey = false;
				colvarUserfloat2.IsForeignKey = false;
				colvarUserfloat2.IsReadOnly = false;
				colvarUserfloat2.DefaultSetting = @"";
				colvarUserfloat2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat2);
				
				TableSchema.TableColumn colvarUserfloat3 = new TableSchema.TableColumn(schema);
				colvarUserfloat3.ColumnName = "userfloat3";
				colvarUserfloat3.DataType = DbType.Currency;
				colvarUserfloat3.MaxLength = 0;
				colvarUserfloat3.AutoIncrement = false;
				colvarUserfloat3.IsNullable = true;
				colvarUserfloat3.IsPrimaryKey = false;
				colvarUserfloat3.IsForeignKey = false;
				colvarUserfloat3.IsReadOnly = false;
				colvarUserfloat3.DefaultSetting = @"";
				colvarUserfloat3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat3);
				
				TableSchema.TableColumn colvarUserfloat4 = new TableSchema.TableColumn(schema);
				colvarUserfloat4.ColumnName = "userfloat4";
				colvarUserfloat4.DataType = DbType.Currency;
				colvarUserfloat4.MaxLength = 0;
				colvarUserfloat4.AutoIncrement = false;
				colvarUserfloat4.IsNullable = true;
				colvarUserfloat4.IsPrimaryKey = false;
				colvarUserfloat4.IsForeignKey = false;
				colvarUserfloat4.IsReadOnly = false;
				colvarUserfloat4.DefaultSetting = @"";
				colvarUserfloat4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat4);
				
				TableSchema.TableColumn colvarUserfloat5 = new TableSchema.TableColumn(schema);
				colvarUserfloat5.ColumnName = "userfloat5";
				colvarUserfloat5.DataType = DbType.Currency;
				colvarUserfloat5.MaxLength = 0;
				colvarUserfloat5.AutoIncrement = false;
				colvarUserfloat5.IsNullable = true;
				colvarUserfloat5.IsPrimaryKey = false;
				colvarUserfloat5.IsForeignKey = false;
				colvarUserfloat5.IsReadOnly = false;
				colvarUserfloat5.DefaultSetting = @"";
				colvarUserfloat5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat5);
				
				TableSchema.TableColumn colvarUserint1 = new TableSchema.TableColumn(schema);
				colvarUserint1.ColumnName = "userint1";
				colvarUserint1.DataType = DbType.Int32;
				colvarUserint1.MaxLength = 0;
				colvarUserint1.AutoIncrement = false;
				colvarUserint1.IsNullable = true;
				colvarUserint1.IsPrimaryKey = false;
				colvarUserint1.IsForeignKey = false;
				colvarUserint1.IsReadOnly = false;
				colvarUserint1.DefaultSetting = @"";
				colvarUserint1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint1);
				
				TableSchema.TableColumn colvarUserint2 = new TableSchema.TableColumn(schema);
				colvarUserint2.ColumnName = "userint2";
				colvarUserint2.DataType = DbType.Int32;
				colvarUserint2.MaxLength = 0;
				colvarUserint2.AutoIncrement = false;
				colvarUserint2.IsNullable = true;
				colvarUserint2.IsPrimaryKey = false;
				colvarUserint2.IsForeignKey = false;
				colvarUserint2.IsReadOnly = false;
				colvarUserint2.DefaultSetting = @"";
				colvarUserint2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint2);
				
				TableSchema.TableColumn colvarUserint3 = new TableSchema.TableColumn(schema);
				colvarUserint3.ColumnName = "userint3";
				colvarUserint3.DataType = DbType.Int32;
				colvarUserint3.MaxLength = 0;
				colvarUserint3.AutoIncrement = false;
				colvarUserint3.IsNullable = true;
				colvarUserint3.IsPrimaryKey = false;
				colvarUserint3.IsForeignKey = false;
				colvarUserint3.IsReadOnly = false;
				colvarUserint3.DefaultSetting = @"";
				colvarUserint3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint3);
				
				TableSchema.TableColumn colvarUserint4 = new TableSchema.TableColumn(schema);
				colvarUserint4.ColumnName = "userint4";
				colvarUserint4.DataType = DbType.Int32;
				colvarUserint4.MaxLength = 0;
				colvarUserint4.AutoIncrement = false;
				colvarUserint4.IsNullable = true;
				colvarUserint4.IsPrimaryKey = false;
				colvarUserint4.IsForeignKey = false;
				colvarUserint4.IsReadOnly = false;
				colvarUserint4.DefaultSetting = @"";
				colvarUserint4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint4);
				
				TableSchema.TableColumn colvarUserint5 = new TableSchema.TableColumn(schema);
				colvarUserint5.ColumnName = "userint5";
				colvarUserint5.DataType = DbType.Int32;
				colvarUserint5.MaxLength = 0;
				colvarUserint5.AutoIncrement = false;
				colvarUserint5.IsNullable = true;
				colvarUserint5.IsPrimaryKey = false;
				colvarUserint5.IsForeignKey = false;
				colvarUserint5.IsReadOnly = false;
				colvarUserint5.DefaultSetting = @"";
				colvarUserint5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint5);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("TabletCollection",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("TabletCollectionID")]
		[Bindable(true)]
		public int TabletCollectionID 
		{
			get { return GetColumnValue<int>(Columns.TabletCollectionID); }
			set { SetColumnValue(Columns.TabletCollectionID, value); }
		}
		  
		[XmlAttribute("PointOfSaleID")]
		[Bindable(true)]
		public int PointOfSaleID 
		{
			get { return GetColumnValue<int>(Columns.PointOfSaleID); }
			set { SetColumnValue(Columns.PointOfSaleID, value); }
		}
		  
		[XmlAttribute("TerminalID")]
		[Bindable(true)]
		public int TerminalID 
		{
			get { return GetColumnValue<int>(Columns.TerminalID); }
			set { SetColumnValue(Columns.TerminalID, value); }
		}
		  
		[XmlAttribute("Status")]
		[Bindable(true)]
		public string Status 
		{
			get { return GetColumnValue<string>(Columns.Status); }
			set { SetColumnValue(Columns.Status, value); }
		}
		  
		[XmlAttribute("OpenTime")]
		[Bindable(true)]
		public DateTime? OpenTime 
		{
			get { return GetColumnValue<DateTime?>(Columns.OpenTime); }
			set { SetColumnValue(Columns.OpenTime, value); }
		}
		  
		[XmlAttribute("CloseTime")]
		[Bindable(true)]
		public DateTime? CloseTime 
		{
			get { return GetColumnValue<DateTime?>(Columns.CloseTime); }
			set { SetColumnValue(Columns.CloseTime, value); }
		}
		  
		[XmlAttribute("OpenBy")]
		[Bindable(true)]
		public string OpenBy 
		{
			get { return GetColumnValue<string>(Columns.OpenBy); }
			set { SetColumnValue(Columns.OpenBy, value); }
		}
		  
		[XmlAttribute("CloseBy")]
		[Bindable(true)]
		public string CloseBy 
		{
			get { return GetColumnValue<string>(Columns.CloseBy); }
			set { SetColumnValue(Columns.CloseBy, value); }
		}
		  
		[XmlAttribute("Supervisor")]
		[Bindable(true)]
		public string Supervisor 
		{
			get { return GetColumnValue<string>(Columns.Supervisor); }
			set { SetColumnValue(Columns.Supervisor, value); }
		}
		  
		[XmlAttribute("CashRecorded")]
		[Bindable(true)]
		public decimal? CashRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.CashRecorded); }
			set { SetColumnValue(Columns.CashRecorded, value); }
		}
		  
		[XmlAttribute("Pay1Recorded")]
		[Bindable(true)]
		public decimal? Pay1Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay1Recorded); }
			set { SetColumnValue(Columns.Pay1Recorded, value); }
		}
		  
		[XmlAttribute("Pay2Recorded")]
		[Bindable(true)]
		public decimal? Pay2Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay2Recorded); }
			set { SetColumnValue(Columns.Pay2Recorded, value); }
		}
		  
		[XmlAttribute("Pay3Recorded")]
		[Bindable(true)]
		public decimal? Pay3Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay3Recorded); }
			set { SetColumnValue(Columns.Pay3Recorded, value); }
		}
		  
		[XmlAttribute("Pay4Recorded")]
		[Bindable(true)]
		public decimal? Pay4Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay4Recorded); }
			set { SetColumnValue(Columns.Pay4Recorded, value); }
		}
		  
		[XmlAttribute("Pay5Recorded")]
		[Bindable(true)]
		public decimal? Pay5Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay5Recorded); }
			set { SetColumnValue(Columns.Pay5Recorded, value); }
		}
		  
		[XmlAttribute("Pay6Recorded")]
		[Bindable(true)]
		public decimal? Pay6Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay6Recorded); }
			set { SetColumnValue(Columns.Pay6Recorded, value); }
		}
		  
		[XmlAttribute("Pay7Recorded")]
		[Bindable(true)]
		public decimal? Pay7Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay7Recorded); }
			set { SetColumnValue(Columns.Pay7Recorded, value); }
		}
		  
		[XmlAttribute("Pay8Recorded")]
		[Bindable(true)]
		public decimal? Pay8Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay8Recorded); }
			set { SetColumnValue(Columns.Pay8Recorded, value); }
		}
		  
		[XmlAttribute("Pay9Recorded")]
		[Bindable(true)]
		public decimal? Pay9Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay9Recorded); }
			set { SetColumnValue(Columns.Pay9Recorded, value); }
		}
		  
		[XmlAttribute("Pay10Recorded")]
		[Bindable(true)]
		public decimal? Pay10Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay10Recorded); }
			set { SetColumnValue(Columns.Pay10Recorded, value); }
		}
		  
		[XmlAttribute("VoucherRecorded")]
		[Bindable(true)]
		public decimal? VoucherRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.VoucherRecorded); }
			set { SetColumnValue(Columns.VoucherRecorded, value); }
		}
		  
		[XmlAttribute("ChequeRecorded")]
		[Bindable(true)]
		public decimal? ChequeRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.ChequeRecorded); }
			set { SetColumnValue(Columns.ChequeRecorded, value); }
		}
		  
		[XmlAttribute("PointRecorded")]
		[Bindable(true)]
		public decimal? PointRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.PointRecorded); }
			set { SetColumnValue(Columns.PointRecorded, value); }
		}
		  
		[XmlAttribute("PackageRecorded")]
		[Bindable(true)]
		public decimal? PackageRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.PackageRecorded); }
			set { SetColumnValue(Columns.PackageRecorded, value); }
		}
		  
		[XmlAttribute("SMFRecorded")]
		[Bindable(true)]
		public decimal? SMFRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.SMFRecorded); }
			set { SetColumnValue(Columns.SMFRecorded, value); }
		}
		  
		[XmlAttribute("PAMedRecorded")]
		[Bindable(true)]
		public decimal? PAMedRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.PAMedRecorded); }
			set { SetColumnValue(Columns.PAMedRecorded, value); }
		}
		  
		[XmlAttribute("PWFRecorded")]
		[Bindable(true)]
		public decimal? PWFRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.PWFRecorded); }
			set { SetColumnValue(Columns.PWFRecorded, value); }
		}
		  
		[XmlAttribute("NetsATMCardRecorded")]
		[Bindable(true)]
		public decimal? NetsATMCardRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.NetsATMCardRecorded); }
			set { SetColumnValue(Columns.NetsATMCardRecorded, value); }
		}
		  
		[XmlAttribute("NetsCashCardRecorded")]
		[Bindable(true)]
		public decimal? NetsCashCardRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.NetsCashCardRecorded); }
			set { SetColumnValue(Columns.NetsCashCardRecorded, value); }
		}
		  
		[XmlAttribute("NetsFlashPayRecorded")]
		[Bindable(true)]
		public decimal? NetsFlashPayRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.NetsFlashPayRecorded); }
			set { SetColumnValue(Columns.NetsFlashPayRecorded, value); }
		}
		  
		[XmlAttribute("ForeignCurrency1")]
		[Bindable(true)]
		public string ForeignCurrency1 
		{
			get { return GetColumnValue<string>(Columns.ForeignCurrency1); }
			set { SetColumnValue(Columns.ForeignCurrency1, value); }
		}
		  
		[XmlAttribute("ForeignCurrency1Recorded")]
		[Bindable(true)]
		public decimal? ForeignCurrency1Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.ForeignCurrency1Recorded); }
			set { SetColumnValue(Columns.ForeignCurrency1Recorded, value); }
		}
		  
		[XmlAttribute("ForeignCurrency2")]
		[Bindable(true)]
		public string ForeignCurrency2 
		{
			get { return GetColumnValue<string>(Columns.ForeignCurrency2); }
			set { SetColumnValue(Columns.ForeignCurrency2, value); }
		}
		  
		[XmlAttribute("ForeignCurrency2Recorded")]
		[Bindable(true)]
		public decimal? ForeignCurrency2Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.ForeignCurrency2Recorded); }
			set { SetColumnValue(Columns.ForeignCurrency2Recorded, value); }
		}
		  
		[XmlAttribute("ForeignCurrency3")]
		[Bindable(true)]
		public string ForeignCurrency3 
		{
			get { return GetColumnValue<string>(Columns.ForeignCurrency3); }
			set { SetColumnValue(Columns.ForeignCurrency3, value); }
		}
		  
		[XmlAttribute("ForeignCurrency3Recorded")]
		[Bindable(true)]
		public decimal? ForeignCurrency3Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.ForeignCurrency3Recorded); }
			set { SetColumnValue(Columns.ForeignCurrency3Recorded, value); }
		}
		  
		[XmlAttribute("ForeignCurrency4")]
		[Bindable(true)]
		public string ForeignCurrency4 
		{
			get { return GetColumnValue<string>(Columns.ForeignCurrency4); }
			set { SetColumnValue(Columns.ForeignCurrency4, value); }
		}
		  
		[XmlAttribute("ForeignCurrency4Recorded")]
		[Bindable(true)]
		public decimal? ForeignCurrency4Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.ForeignCurrency4Recorded); }
			set { SetColumnValue(Columns.ForeignCurrency4Recorded, value); }
		}
		  
		[XmlAttribute("ForeignCurrency5")]
		[Bindable(true)]
		public string ForeignCurrency5 
		{
			get { return GetColumnValue<string>(Columns.ForeignCurrency5); }
			set { SetColumnValue(Columns.ForeignCurrency5, value); }
		}
		  
		[XmlAttribute("ForeignCurrency5Recorded")]
		[Bindable(true)]
		public decimal? ForeignCurrency5Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.ForeignCurrency5Recorded); }
			set { SetColumnValue(Columns.ForeignCurrency5Recorded, value); }
		}
		  
		[XmlAttribute("TotalForeignCurrency")]
		[Bindable(true)]
		public decimal? TotalForeignCurrency 
		{
			get { return GetColumnValue<decimal?>(Columns.TotalForeignCurrency); }
			set { SetColumnValue(Columns.TotalForeignCurrency, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime CreatedOn 
		{
			get { return GetColumnValue<DateTime>(Columns.CreatedOn); }
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
		public DateTime ModifiedOn 
		{
			get { return GetColumnValue<DateTime>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		  
		[XmlAttribute("UniqueID")]
		[Bindable(true)]
		public Guid UniqueID 
		{
			get { return GetColumnValue<Guid>(Columns.UniqueID); }
			set { SetColumnValue(Columns.UniqueID, value); }
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
		  
		[XmlAttribute("Userflag1")]
		[Bindable(true)]
		public bool? Userflag1 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag1); }
			set { SetColumnValue(Columns.Userflag1, value); }
		}
		  
		[XmlAttribute("Userflag2")]
		[Bindable(true)]
		public bool? Userflag2 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag2); }
			set { SetColumnValue(Columns.Userflag2, value); }
		}
		  
		[XmlAttribute("Userflag3")]
		[Bindable(true)]
		public bool? Userflag3 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag3); }
			set { SetColumnValue(Columns.Userflag3, value); }
		}
		  
		[XmlAttribute("Userflag4")]
		[Bindable(true)]
		public bool? Userflag4 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag4); }
			set { SetColumnValue(Columns.Userflag4, value); }
		}
		  
		[XmlAttribute("Userflag5")]
		[Bindable(true)]
		public bool? Userflag5 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag5); }
			set { SetColumnValue(Columns.Userflag5, value); }
		}
		  
		[XmlAttribute("Userfloat1")]
		[Bindable(true)]
		public decimal? Userfloat1 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat1); }
			set { SetColumnValue(Columns.Userfloat1, value); }
		}
		  
		[XmlAttribute("Userfloat2")]
		[Bindable(true)]
		public decimal? Userfloat2 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat2); }
			set { SetColumnValue(Columns.Userfloat2, value); }
		}
		  
		[XmlAttribute("Userfloat3")]
		[Bindable(true)]
		public decimal? Userfloat3 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat3); }
			set { SetColumnValue(Columns.Userfloat3, value); }
		}
		  
		[XmlAttribute("Userfloat4")]
		[Bindable(true)]
		public decimal? Userfloat4 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat4); }
			set { SetColumnValue(Columns.Userfloat4, value); }
		}
		  
		[XmlAttribute("Userfloat5")]
		[Bindable(true)]
		public decimal? Userfloat5 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat5); }
			set { SetColumnValue(Columns.Userfloat5, value); }
		}
		  
		[XmlAttribute("Userint1")]
		[Bindable(true)]
		public int? Userint1 
		{
			get { return GetColumnValue<int?>(Columns.Userint1); }
			set { SetColumnValue(Columns.Userint1, value); }
		}
		  
		[XmlAttribute("Userint2")]
		[Bindable(true)]
		public int? Userint2 
		{
			get { return GetColumnValue<int?>(Columns.Userint2); }
			set { SetColumnValue(Columns.Userint2, value); }
		}
		  
		[XmlAttribute("Userint3")]
		[Bindable(true)]
		public int? Userint3 
		{
			get { return GetColumnValue<int?>(Columns.Userint3); }
			set { SetColumnValue(Columns.Userint3, value); }
		}
		  
		[XmlAttribute("Userint4")]
		[Bindable(true)]
		public int? Userint4 
		{
			get { return GetColumnValue<int?>(Columns.Userint4); }
			set { SetColumnValue(Columns.Userint4, value); }
		}
		  
		[XmlAttribute("Userint5")]
		[Bindable(true)]
		public int? Userint5 
		{
			get { return GetColumnValue<int?>(Columns.Userint5); }
			set { SetColumnValue(Columns.Userint5, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varPointOfSaleID,int varTerminalID,string varStatus,DateTime? varOpenTime,DateTime? varCloseTime,string varOpenBy,string varCloseBy,string varSupervisor,decimal? varCashRecorded,decimal? varPay1Recorded,decimal? varPay2Recorded,decimal? varPay3Recorded,decimal? varPay4Recorded,decimal? varPay5Recorded,decimal? varPay6Recorded,decimal? varPay7Recorded,decimal? varPay8Recorded,decimal? varPay9Recorded,decimal? varPay10Recorded,decimal? varVoucherRecorded,decimal? varChequeRecorded,decimal? varPointRecorded,decimal? varPackageRecorded,decimal? varSMFRecorded,decimal? varPAMedRecorded,decimal? varPWFRecorded,decimal? varNetsATMCardRecorded,decimal? varNetsCashCardRecorded,decimal? varNetsFlashPayRecorded,string varForeignCurrency1,decimal? varForeignCurrency1Recorded,string varForeignCurrency2,decimal? varForeignCurrency2Recorded,string varForeignCurrency3,decimal? varForeignCurrency3Recorded,string varForeignCurrency4,decimal? varForeignCurrency4Recorded,string varForeignCurrency5,decimal? varForeignCurrency5Recorded,decimal? varTotalForeignCurrency,DateTime varCreatedOn,string varCreatedBy,DateTime varModifiedOn,string varModifiedBy,Guid varUniqueID,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5)
		{
			TabletCollection item = new TabletCollection();
			
			item.PointOfSaleID = varPointOfSaleID;
			
			item.TerminalID = varTerminalID;
			
			item.Status = varStatus;
			
			item.OpenTime = varOpenTime;
			
			item.CloseTime = varCloseTime;
			
			item.OpenBy = varOpenBy;
			
			item.CloseBy = varCloseBy;
			
			item.Supervisor = varSupervisor;
			
			item.CashRecorded = varCashRecorded;
			
			item.Pay1Recorded = varPay1Recorded;
			
			item.Pay2Recorded = varPay2Recorded;
			
			item.Pay3Recorded = varPay3Recorded;
			
			item.Pay4Recorded = varPay4Recorded;
			
			item.Pay5Recorded = varPay5Recorded;
			
			item.Pay6Recorded = varPay6Recorded;
			
			item.Pay7Recorded = varPay7Recorded;
			
			item.Pay8Recorded = varPay8Recorded;
			
			item.Pay9Recorded = varPay9Recorded;
			
			item.Pay10Recorded = varPay10Recorded;
			
			item.VoucherRecorded = varVoucherRecorded;
			
			item.ChequeRecorded = varChequeRecorded;
			
			item.PointRecorded = varPointRecorded;
			
			item.PackageRecorded = varPackageRecorded;
			
			item.SMFRecorded = varSMFRecorded;
			
			item.PAMedRecorded = varPAMedRecorded;
			
			item.PWFRecorded = varPWFRecorded;
			
			item.NetsATMCardRecorded = varNetsATMCardRecorded;
			
			item.NetsCashCardRecorded = varNetsCashCardRecorded;
			
			item.NetsFlashPayRecorded = varNetsFlashPayRecorded;
			
			item.ForeignCurrency1 = varForeignCurrency1;
			
			item.ForeignCurrency1Recorded = varForeignCurrency1Recorded;
			
			item.ForeignCurrency2 = varForeignCurrency2;
			
			item.ForeignCurrency2Recorded = varForeignCurrency2Recorded;
			
			item.ForeignCurrency3 = varForeignCurrency3;
			
			item.ForeignCurrency3Recorded = varForeignCurrency3Recorded;
			
			item.ForeignCurrency4 = varForeignCurrency4;
			
			item.ForeignCurrency4Recorded = varForeignCurrency4Recorded;
			
			item.ForeignCurrency5 = varForeignCurrency5;
			
			item.ForeignCurrency5Recorded = varForeignCurrency5Recorded;
			
			item.TotalForeignCurrency = varTotalForeignCurrency;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.UniqueID = varUniqueID;
			
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
			
			item.Userflag1 = varUserflag1;
			
			item.Userflag2 = varUserflag2;
			
			item.Userflag3 = varUserflag3;
			
			item.Userflag4 = varUserflag4;
			
			item.Userflag5 = varUserflag5;
			
			item.Userfloat1 = varUserfloat1;
			
			item.Userfloat2 = varUserfloat2;
			
			item.Userfloat3 = varUserfloat3;
			
			item.Userfloat4 = varUserfloat4;
			
			item.Userfloat5 = varUserfloat5;
			
			item.Userint1 = varUserint1;
			
			item.Userint2 = varUserint2;
			
			item.Userint3 = varUserint3;
			
			item.Userint4 = varUserint4;
			
			item.Userint5 = varUserint5;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varTabletCollectionID,int varPointOfSaleID,int varTerminalID,string varStatus,DateTime? varOpenTime,DateTime? varCloseTime,string varOpenBy,string varCloseBy,string varSupervisor,decimal? varCashRecorded,decimal? varPay1Recorded,decimal? varPay2Recorded,decimal? varPay3Recorded,decimal? varPay4Recorded,decimal? varPay5Recorded,decimal? varPay6Recorded,decimal? varPay7Recorded,decimal? varPay8Recorded,decimal? varPay9Recorded,decimal? varPay10Recorded,decimal? varVoucherRecorded,decimal? varChequeRecorded,decimal? varPointRecorded,decimal? varPackageRecorded,decimal? varSMFRecorded,decimal? varPAMedRecorded,decimal? varPWFRecorded,decimal? varNetsATMCardRecorded,decimal? varNetsCashCardRecorded,decimal? varNetsFlashPayRecorded,string varForeignCurrency1,decimal? varForeignCurrency1Recorded,string varForeignCurrency2,decimal? varForeignCurrency2Recorded,string varForeignCurrency3,decimal? varForeignCurrency3Recorded,string varForeignCurrency4,decimal? varForeignCurrency4Recorded,string varForeignCurrency5,decimal? varForeignCurrency5Recorded,decimal? varTotalForeignCurrency,DateTime varCreatedOn,string varCreatedBy,DateTime varModifiedOn,string varModifiedBy,Guid varUniqueID,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5)
		{
			TabletCollection item = new TabletCollection();
			
				item.TabletCollectionID = varTabletCollectionID;
			
				item.PointOfSaleID = varPointOfSaleID;
			
				item.TerminalID = varTerminalID;
			
				item.Status = varStatus;
			
				item.OpenTime = varOpenTime;
			
				item.CloseTime = varCloseTime;
			
				item.OpenBy = varOpenBy;
			
				item.CloseBy = varCloseBy;
			
				item.Supervisor = varSupervisor;
			
				item.CashRecorded = varCashRecorded;
			
				item.Pay1Recorded = varPay1Recorded;
			
				item.Pay2Recorded = varPay2Recorded;
			
				item.Pay3Recorded = varPay3Recorded;
			
				item.Pay4Recorded = varPay4Recorded;
			
				item.Pay5Recorded = varPay5Recorded;
			
				item.Pay6Recorded = varPay6Recorded;
			
				item.Pay7Recorded = varPay7Recorded;
			
				item.Pay8Recorded = varPay8Recorded;
			
				item.Pay9Recorded = varPay9Recorded;
			
				item.Pay10Recorded = varPay10Recorded;
			
				item.VoucherRecorded = varVoucherRecorded;
			
				item.ChequeRecorded = varChequeRecorded;
			
				item.PointRecorded = varPointRecorded;
			
				item.PackageRecorded = varPackageRecorded;
			
				item.SMFRecorded = varSMFRecorded;
			
				item.PAMedRecorded = varPAMedRecorded;
			
				item.PWFRecorded = varPWFRecorded;
			
				item.NetsATMCardRecorded = varNetsATMCardRecorded;
			
				item.NetsCashCardRecorded = varNetsCashCardRecorded;
			
				item.NetsFlashPayRecorded = varNetsFlashPayRecorded;
			
				item.ForeignCurrency1 = varForeignCurrency1;
			
				item.ForeignCurrency1Recorded = varForeignCurrency1Recorded;
			
				item.ForeignCurrency2 = varForeignCurrency2;
			
				item.ForeignCurrency2Recorded = varForeignCurrency2Recorded;
			
				item.ForeignCurrency3 = varForeignCurrency3;
			
				item.ForeignCurrency3Recorded = varForeignCurrency3Recorded;
			
				item.ForeignCurrency4 = varForeignCurrency4;
			
				item.ForeignCurrency4Recorded = varForeignCurrency4Recorded;
			
				item.ForeignCurrency5 = varForeignCurrency5;
			
				item.ForeignCurrency5Recorded = varForeignCurrency5Recorded;
			
				item.TotalForeignCurrency = varTotalForeignCurrency;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.UniqueID = varUniqueID;
			
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
			
				item.Userflag1 = varUserflag1;
			
				item.Userflag2 = varUserflag2;
			
				item.Userflag3 = varUserflag3;
			
				item.Userflag4 = varUserflag4;
			
				item.Userflag5 = varUserflag5;
			
				item.Userfloat1 = varUserfloat1;
			
				item.Userfloat2 = varUserfloat2;
			
				item.Userfloat3 = varUserfloat3;
			
				item.Userfloat4 = varUserfloat4;
			
				item.Userfloat5 = varUserfloat5;
			
				item.Userint1 = varUserint1;
			
				item.Userint2 = varUserint2;
			
				item.Userint3 = varUserint3;
			
				item.Userint4 = varUserint4;
			
				item.Userint5 = varUserint5;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn TabletCollectionIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PointOfSaleIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TerminalIDColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn OpenTimeColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CloseTimeColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn OpenByColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CloseByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn SupervisorColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn CashRecordedColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay1RecordedColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay2RecordedColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay3RecordedColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay4RecordedColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay5RecordedColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay6RecordedColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay7RecordedColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay8RecordedColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay9RecordedColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay10RecordedColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn VoucherRecordedColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn ChequeRecordedColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn PointRecordedColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn PackageRecordedColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn SMFRecordedColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn PAMedRecordedColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn PWFRecordedColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsATMCardRecordedColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsCashCardRecordedColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsFlashPayRecordedColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency1Column
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency1RecordedColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency2Column
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency2RecordedColumn
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency3Column
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency3RecordedColumn
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency4Column
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency4RecordedColumn
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency5Column
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency5RecordedColumn
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn TotalForeignCurrencyColumn
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[41]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[42]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[43]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[44]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[45]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld1Column
        {
            get { return Schema.Columns[46]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld2Column
        {
            get { return Schema.Columns[47]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld3Column
        {
            get { return Schema.Columns[48]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld4Column
        {
            get { return Schema.Columns[49]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld5Column
        {
            get { return Schema.Columns[50]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld6Column
        {
            get { return Schema.Columns[51]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld7Column
        {
            get { return Schema.Columns[52]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld8Column
        {
            get { return Schema.Columns[53]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld9Column
        {
            get { return Schema.Columns[54]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld10Column
        {
            get { return Schema.Columns[55]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag1Column
        {
            get { return Schema.Columns[56]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag2Column
        {
            get { return Schema.Columns[57]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag3Column
        {
            get { return Schema.Columns[58]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag4Column
        {
            get { return Schema.Columns[59]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag5Column
        {
            get { return Schema.Columns[60]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat1Column
        {
            get { return Schema.Columns[61]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat2Column
        {
            get { return Schema.Columns[62]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat3Column
        {
            get { return Schema.Columns[63]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat4Column
        {
            get { return Schema.Columns[64]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat5Column
        {
            get { return Schema.Columns[65]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint1Column
        {
            get { return Schema.Columns[66]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint2Column
        {
            get { return Schema.Columns[67]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint3Column
        {
            get { return Schema.Columns[68]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint4Column
        {
            get { return Schema.Columns[69]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint5Column
        {
            get { return Schema.Columns[70]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string TabletCollectionID = @"TabletCollectionID";
			 public static string PointOfSaleID = @"PointOfSaleID";
			 public static string TerminalID = @"TerminalID";
			 public static string Status = @"Status";
			 public static string OpenTime = @"OpenTime";
			 public static string CloseTime = @"CloseTime";
			 public static string OpenBy = @"OpenBy";
			 public static string CloseBy = @"CloseBy";
			 public static string Supervisor = @"Supervisor";
			 public static string CashRecorded = @"CashRecorded";
			 public static string Pay1Recorded = @"Pay1Recorded";
			 public static string Pay2Recorded = @"Pay2Recorded";
			 public static string Pay3Recorded = @"Pay3Recorded";
			 public static string Pay4Recorded = @"Pay4Recorded";
			 public static string Pay5Recorded = @"Pay5Recorded";
			 public static string Pay6Recorded = @"Pay6Recorded";
			 public static string Pay7Recorded = @"Pay7Recorded";
			 public static string Pay8Recorded = @"Pay8Recorded";
			 public static string Pay9Recorded = @"Pay9Recorded";
			 public static string Pay10Recorded = @"Pay10Recorded";
			 public static string VoucherRecorded = @"VoucherRecorded";
			 public static string ChequeRecorded = @"ChequeRecorded";
			 public static string PointRecorded = @"PointRecorded";
			 public static string PackageRecorded = @"PackageRecorded";
			 public static string SMFRecorded = @"SMFRecorded";
			 public static string PAMedRecorded = @"PAMedRecorded";
			 public static string PWFRecorded = @"PWFRecorded";
			 public static string NetsATMCardRecorded = @"NetsATMCardRecorded";
			 public static string NetsCashCardRecorded = @"NetsCashCardRecorded";
			 public static string NetsFlashPayRecorded = @"NetsFlashPayRecorded";
			 public static string ForeignCurrency1 = @"ForeignCurrency1";
			 public static string ForeignCurrency1Recorded = @"ForeignCurrency1Recorded";
			 public static string ForeignCurrency2 = @"ForeignCurrency2";
			 public static string ForeignCurrency2Recorded = @"ForeignCurrency2Recorded";
			 public static string ForeignCurrency3 = @"ForeignCurrency3";
			 public static string ForeignCurrency3Recorded = @"ForeignCurrency3Recorded";
			 public static string ForeignCurrency4 = @"ForeignCurrency4";
			 public static string ForeignCurrency4Recorded = @"ForeignCurrency4Recorded";
			 public static string ForeignCurrency5 = @"ForeignCurrency5";
			 public static string ForeignCurrency5Recorded = @"ForeignCurrency5Recorded";
			 public static string TotalForeignCurrency = @"TotalForeignCurrency";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string UniqueID = @"UniqueID";
			 public static string Userfld1 = @"userfld1";
			 public static string Userfld2 = @"userfld2";
			 public static string Userfld3 = @"userfld3";
			 public static string Userfld4 = @"userfld4";
			 public static string Userfld5 = @"userfld5";
			 public static string Userfld6 = @"userfld6";
			 public static string Userfld7 = @"userfld7";
			 public static string Userfld8 = @"userfld8";
			 public static string Userfld9 = @"userfld9";
			 public static string Userfld10 = @"userfld10";
			 public static string Userflag1 = @"userflag1";
			 public static string Userflag2 = @"userflag2";
			 public static string Userflag3 = @"userflag3";
			 public static string Userflag4 = @"userflag4";
			 public static string Userflag5 = @"userflag5";
			 public static string Userfloat1 = @"userfloat1";
			 public static string Userfloat2 = @"userfloat2";
			 public static string Userfloat3 = @"userfloat3";
			 public static string Userfloat4 = @"userfloat4";
			 public static string Userfloat5 = @"userfloat5";
			 public static string Userint1 = @"userint1";
			 public static string Userint2 = @"userint2";
			 public static string Userint3 = @"userint3";
			 public static string Userint4 = @"userint4";
			 public static string Userint5 = @"userint5";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
