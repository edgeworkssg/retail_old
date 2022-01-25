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
	/// Strongly-typed collection for the CounterCloseLog class.
	/// </summary>
    [Serializable]
	public partial class CounterCloseLogCollection : ActiveList<CounterCloseLog, CounterCloseLogCollection>
	{	   
		public CounterCloseLogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>CounterCloseLogCollection</returns>
		public CounterCloseLogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                CounterCloseLog o = this[i];
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
	/// This is an ActiveRecord class which wraps the CounterCloseLog table.
	/// </summary>
	[Serializable]
	public partial class CounterCloseLog : ActiveRecord<CounterCloseLog>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public CounterCloseLog()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public CounterCloseLog(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public CounterCloseLog(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public CounterCloseLog(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("CounterCloseLog", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarCounterCloseLogID = new TableSchema.TableColumn(schema);
				colvarCounterCloseLogID.ColumnName = "CounterCloseLogID";
				colvarCounterCloseLogID.DataType = DbType.Int32;
				colvarCounterCloseLogID.MaxLength = 0;
				colvarCounterCloseLogID.AutoIncrement = true;
				colvarCounterCloseLogID.IsNullable = false;
				colvarCounterCloseLogID.IsPrimaryKey = true;
				colvarCounterCloseLogID.IsForeignKey = false;
				colvarCounterCloseLogID.IsReadOnly = false;
				colvarCounterCloseLogID.DefaultSetting = @"";
				colvarCounterCloseLogID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCounterCloseLogID);
				
				TableSchema.TableColumn colvarCounterCloseID = new TableSchema.TableColumn(schema);
				colvarCounterCloseID.ColumnName = "CounterCloseID";
				colvarCounterCloseID.DataType = DbType.AnsiString;
				colvarCounterCloseID.MaxLength = 16;
				colvarCounterCloseID.AutoIncrement = false;
				colvarCounterCloseID.IsNullable = false;
				colvarCounterCloseID.IsPrimaryKey = false;
				colvarCounterCloseID.IsForeignKey = false;
				colvarCounterCloseID.IsReadOnly = false;
				colvarCounterCloseID.DefaultSetting = @"";
				colvarCounterCloseID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCounterCloseID);
				
				TableSchema.TableColumn colvarFloatBalance = new TableSchema.TableColumn(schema);
				colvarFloatBalance.ColumnName = "FloatBalance";
				colvarFloatBalance.DataType = DbType.Currency;
				colvarFloatBalance.MaxLength = 0;
				colvarFloatBalance.AutoIncrement = false;
				colvarFloatBalance.IsNullable = false;
				colvarFloatBalance.IsPrimaryKey = false;
				colvarFloatBalance.IsForeignKey = false;
				colvarFloatBalance.IsReadOnly = false;
				colvarFloatBalance.DefaultSetting = @"";
				colvarFloatBalance.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFloatBalance);
				
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
				
				TableSchema.TableColumn colvarEndTime = new TableSchema.TableColumn(schema);
				colvarEndTime.ColumnName = "EndTime";
				colvarEndTime.DataType = DbType.DateTime;
				colvarEndTime.MaxLength = 0;
				colvarEndTime.AutoIncrement = false;
				colvarEndTime.IsNullable = false;
				colvarEndTime.IsPrimaryKey = false;
				colvarEndTime.IsForeignKey = false;
				colvarEndTime.IsReadOnly = false;
				colvarEndTime.DefaultSetting = @"";
				colvarEndTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEndTime);
				
				TableSchema.TableColumn colvarCashier = new TableSchema.TableColumn(schema);
				colvarCashier.ColumnName = "Cashier";
				colvarCashier.DataType = DbType.AnsiString;
				colvarCashier.MaxLength = 50;
				colvarCashier.AutoIncrement = false;
				colvarCashier.IsNullable = false;
				colvarCashier.IsPrimaryKey = false;
				colvarCashier.IsForeignKey = false;
				colvarCashier.IsReadOnly = false;
				colvarCashier.DefaultSetting = @"";
				colvarCashier.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCashier);
				
				TableSchema.TableColumn colvarSupervisor = new TableSchema.TableColumn(schema);
				colvarSupervisor.ColumnName = "Supervisor";
				colvarSupervisor.DataType = DbType.AnsiString;
				colvarSupervisor.MaxLength = 50;
				colvarSupervisor.AutoIncrement = false;
				colvarSupervisor.IsNullable = false;
				colvarSupervisor.IsPrimaryKey = false;
				colvarSupervisor.IsForeignKey = false;
				colvarSupervisor.IsReadOnly = false;
				colvarSupervisor.DefaultSetting = @"";
				colvarSupervisor.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSupervisor);
				
				TableSchema.TableColumn colvarCashIn = new TableSchema.TableColumn(schema);
				colvarCashIn.ColumnName = "CashIn";
				colvarCashIn.DataType = DbType.Currency;
				colvarCashIn.MaxLength = 0;
				colvarCashIn.AutoIncrement = false;
				colvarCashIn.IsNullable = false;
				colvarCashIn.IsPrimaryKey = false;
				colvarCashIn.IsForeignKey = false;
				colvarCashIn.IsReadOnly = false;
				colvarCashIn.DefaultSetting = @"";
				colvarCashIn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCashIn);
				
				TableSchema.TableColumn colvarCashOut = new TableSchema.TableColumn(schema);
				colvarCashOut.ColumnName = "CashOut";
				colvarCashOut.DataType = DbType.Currency;
				colvarCashOut.MaxLength = 0;
				colvarCashOut.AutoIncrement = false;
				colvarCashOut.IsNullable = false;
				colvarCashOut.IsPrimaryKey = false;
				colvarCashOut.IsForeignKey = false;
				colvarCashOut.IsReadOnly = false;
				colvarCashOut.DefaultSetting = @"";
				colvarCashOut.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCashOut);
				
				TableSchema.TableColumn colvarOpeningBalance = new TableSchema.TableColumn(schema);
				colvarOpeningBalance.ColumnName = "OpeningBalance";
				colvarOpeningBalance.DataType = DbType.Currency;
				colvarOpeningBalance.MaxLength = 0;
				colvarOpeningBalance.AutoIncrement = false;
				colvarOpeningBalance.IsNullable = false;
				colvarOpeningBalance.IsPrimaryKey = false;
				colvarOpeningBalance.IsForeignKey = false;
				colvarOpeningBalance.IsReadOnly = false;
				colvarOpeningBalance.DefaultSetting = @"";
				colvarOpeningBalance.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOpeningBalance);
				
				TableSchema.TableColumn colvarTotalSystemRecorded = new TableSchema.TableColumn(schema);
				colvarTotalSystemRecorded.ColumnName = "TotalSystemRecorded";
				colvarTotalSystemRecorded.DataType = DbType.Currency;
				colvarTotalSystemRecorded.MaxLength = 0;
				colvarTotalSystemRecorded.AutoIncrement = false;
				colvarTotalSystemRecorded.IsNullable = false;
				colvarTotalSystemRecorded.IsPrimaryKey = false;
				colvarTotalSystemRecorded.IsForeignKey = false;
				colvarTotalSystemRecorded.IsReadOnly = false;
				colvarTotalSystemRecorded.DefaultSetting = @"";
				colvarTotalSystemRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTotalSystemRecorded);
				
				TableSchema.TableColumn colvarCashCollected = new TableSchema.TableColumn(schema);
				colvarCashCollected.ColumnName = "CashCollected";
				colvarCashCollected.DataType = DbType.Currency;
				colvarCashCollected.MaxLength = 0;
				colvarCashCollected.AutoIncrement = false;
				colvarCashCollected.IsNullable = false;
				colvarCashCollected.IsPrimaryKey = false;
				colvarCashCollected.IsForeignKey = false;
				colvarCashCollected.IsReadOnly = false;
				colvarCashCollected.DefaultSetting = @"";
				colvarCashCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCashCollected);
				
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
				
				TableSchema.TableColumn colvarNetsCollected = new TableSchema.TableColumn(schema);
				colvarNetsCollected.ColumnName = "NetsCollected";
				colvarNetsCollected.DataType = DbType.Currency;
				colvarNetsCollected.MaxLength = 0;
				colvarNetsCollected.AutoIncrement = false;
				colvarNetsCollected.IsNullable = false;
				colvarNetsCollected.IsPrimaryKey = false;
				colvarNetsCollected.IsForeignKey = false;
				colvarNetsCollected.IsReadOnly = false;
				colvarNetsCollected.DefaultSetting = @"";
				colvarNetsCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNetsCollected);
				
				TableSchema.TableColumn colvarNetsRecorded = new TableSchema.TableColumn(schema);
				colvarNetsRecorded.ColumnName = "NetsRecorded";
				colvarNetsRecorded.DataType = DbType.Currency;
				colvarNetsRecorded.MaxLength = 0;
				colvarNetsRecorded.AutoIncrement = false;
				colvarNetsRecorded.IsNullable = true;
				colvarNetsRecorded.IsPrimaryKey = false;
				colvarNetsRecorded.IsForeignKey = false;
				colvarNetsRecorded.IsReadOnly = false;
				colvarNetsRecorded.DefaultSetting = @"";
				colvarNetsRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNetsRecorded);
				
				TableSchema.TableColumn colvarNetsTerminalID = new TableSchema.TableColumn(schema);
				colvarNetsTerminalID.ColumnName = "NetsTerminalID";
				colvarNetsTerminalID.DataType = DbType.AnsiString;
				colvarNetsTerminalID.MaxLength = 50;
				colvarNetsTerminalID.AutoIncrement = false;
				colvarNetsTerminalID.IsNullable = true;
				colvarNetsTerminalID.IsPrimaryKey = false;
				colvarNetsTerminalID.IsForeignKey = false;
				colvarNetsTerminalID.IsReadOnly = false;
				colvarNetsTerminalID.DefaultSetting = @"";
				colvarNetsTerminalID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNetsTerminalID);
				
				TableSchema.TableColumn colvarVisaCollected = new TableSchema.TableColumn(schema);
				colvarVisaCollected.ColumnName = "VisaCollected";
				colvarVisaCollected.DataType = DbType.Currency;
				colvarVisaCollected.MaxLength = 0;
				colvarVisaCollected.AutoIncrement = false;
				colvarVisaCollected.IsNullable = false;
				colvarVisaCollected.IsPrimaryKey = false;
				colvarVisaCollected.IsForeignKey = false;
				colvarVisaCollected.IsReadOnly = false;
				colvarVisaCollected.DefaultSetting = @"";
				colvarVisaCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVisaCollected);
				
				TableSchema.TableColumn colvarVisaRecorded = new TableSchema.TableColumn(schema);
				colvarVisaRecorded.ColumnName = "VisaRecorded";
				colvarVisaRecorded.DataType = DbType.Currency;
				colvarVisaRecorded.MaxLength = 0;
				colvarVisaRecorded.AutoIncrement = false;
				colvarVisaRecorded.IsNullable = true;
				colvarVisaRecorded.IsPrimaryKey = false;
				colvarVisaRecorded.IsForeignKey = false;
				colvarVisaRecorded.IsReadOnly = false;
				colvarVisaRecorded.DefaultSetting = @"";
				colvarVisaRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVisaRecorded);
				
				TableSchema.TableColumn colvarVisaBatchNo = new TableSchema.TableColumn(schema);
				colvarVisaBatchNo.ColumnName = "VisaBatchNo";
				colvarVisaBatchNo.DataType = DbType.AnsiString;
				colvarVisaBatchNo.MaxLength = 50;
				colvarVisaBatchNo.AutoIncrement = false;
				colvarVisaBatchNo.IsNullable = true;
				colvarVisaBatchNo.IsPrimaryKey = false;
				colvarVisaBatchNo.IsForeignKey = false;
				colvarVisaBatchNo.IsReadOnly = false;
				colvarVisaBatchNo.DefaultSetting = @"";
				colvarVisaBatchNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVisaBatchNo);
				
				TableSchema.TableColumn colvarAmexCollected = new TableSchema.TableColumn(schema);
				colvarAmexCollected.ColumnName = "AmexCollected";
				colvarAmexCollected.DataType = DbType.Currency;
				colvarAmexCollected.MaxLength = 0;
				colvarAmexCollected.AutoIncrement = false;
				colvarAmexCollected.IsNullable = false;
				colvarAmexCollected.IsPrimaryKey = false;
				colvarAmexCollected.IsForeignKey = false;
				colvarAmexCollected.IsReadOnly = false;
				colvarAmexCollected.DefaultSetting = @"";
				colvarAmexCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmexCollected);
				
				TableSchema.TableColumn colvarAmexRecorded = new TableSchema.TableColumn(schema);
				colvarAmexRecorded.ColumnName = "AmexRecorded";
				colvarAmexRecorded.DataType = DbType.Currency;
				colvarAmexRecorded.MaxLength = 0;
				colvarAmexRecorded.AutoIncrement = false;
				colvarAmexRecorded.IsNullable = true;
				colvarAmexRecorded.IsPrimaryKey = false;
				colvarAmexRecorded.IsForeignKey = false;
				colvarAmexRecorded.IsReadOnly = false;
				colvarAmexRecorded.DefaultSetting = @"";
				colvarAmexRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmexRecorded);
				
				TableSchema.TableColumn colvarAmexBatchNo = new TableSchema.TableColumn(schema);
				colvarAmexBatchNo.ColumnName = "AmexBatchNo";
				colvarAmexBatchNo.DataType = DbType.AnsiString;
				colvarAmexBatchNo.MaxLength = 50;
				colvarAmexBatchNo.AutoIncrement = false;
				colvarAmexBatchNo.IsNullable = true;
				colvarAmexBatchNo.IsPrimaryKey = false;
				colvarAmexBatchNo.IsForeignKey = false;
				colvarAmexBatchNo.IsReadOnly = false;
				colvarAmexBatchNo.DefaultSetting = @"";
				colvarAmexBatchNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmexBatchNo);
				
				TableSchema.TableColumn colvarChinaNetsCollected = new TableSchema.TableColumn(schema);
				colvarChinaNetsCollected.ColumnName = "ChinaNetsCollected";
				colvarChinaNetsCollected.DataType = DbType.Currency;
				colvarChinaNetsCollected.MaxLength = 0;
				colvarChinaNetsCollected.AutoIncrement = false;
				colvarChinaNetsCollected.IsNullable = false;
				colvarChinaNetsCollected.IsPrimaryKey = false;
				colvarChinaNetsCollected.IsForeignKey = false;
				colvarChinaNetsCollected.IsReadOnly = false;
				colvarChinaNetsCollected.DefaultSetting = @"";
				colvarChinaNetsCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChinaNetsCollected);
				
				TableSchema.TableColumn colvarChinaNetsRecorded = new TableSchema.TableColumn(schema);
				colvarChinaNetsRecorded.ColumnName = "ChinaNetsRecorded";
				colvarChinaNetsRecorded.DataType = DbType.Currency;
				colvarChinaNetsRecorded.MaxLength = 0;
				colvarChinaNetsRecorded.AutoIncrement = false;
				colvarChinaNetsRecorded.IsNullable = true;
				colvarChinaNetsRecorded.IsPrimaryKey = false;
				colvarChinaNetsRecorded.IsForeignKey = false;
				colvarChinaNetsRecorded.IsReadOnly = false;
				colvarChinaNetsRecorded.DefaultSetting = @"";
				colvarChinaNetsRecorded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChinaNetsRecorded);
				
				TableSchema.TableColumn colvarChinaNetsTerminalID = new TableSchema.TableColumn(schema);
				colvarChinaNetsTerminalID.ColumnName = "ChinaNetsTerminalID";
				colvarChinaNetsTerminalID.DataType = DbType.AnsiString;
				colvarChinaNetsTerminalID.MaxLength = 50;
				colvarChinaNetsTerminalID.AutoIncrement = false;
				colvarChinaNetsTerminalID.IsNullable = true;
				colvarChinaNetsTerminalID.IsPrimaryKey = false;
				colvarChinaNetsTerminalID.IsForeignKey = false;
				colvarChinaNetsTerminalID.IsReadOnly = false;
				colvarChinaNetsTerminalID.DefaultSetting = @"";
				colvarChinaNetsTerminalID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChinaNetsTerminalID);
				
				TableSchema.TableColumn colvarVoucherCollected = new TableSchema.TableColumn(schema);
				colvarVoucherCollected.ColumnName = "VoucherCollected";
				colvarVoucherCollected.DataType = DbType.Currency;
				colvarVoucherCollected.MaxLength = 0;
				colvarVoucherCollected.AutoIncrement = false;
				colvarVoucherCollected.IsNullable = false;
				colvarVoucherCollected.IsPrimaryKey = false;
				colvarVoucherCollected.IsForeignKey = false;
				colvarVoucherCollected.IsReadOnly = false;
				colvarVoucherCollected.DefaultSetting = @"";
				colvarVoucherCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVoucherCollected);
				
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
				
				TableSchema.TableColumn colvarDepositBagNo = new TableSchema.TableColumn(schema);
				colvarDepositBagNo.ColumnName = "DepositBagNo";
				colvarDepositBagNo.DataType = DbType.AnsiString;
				colvarDepositBagNo.MaxLength = 50;
				colvarDepositBagNo.AutoIncrement = false;
				colvarDepositBagNo.IsNullable = true;
				colvarDepositBagNo.IsPrimaryKey = false;
				colvarDepositBagNo.IsForeignKey = false;
				colvarDepositBagNo.IsReadOnly = false;
				colvarDepositBagNo.DefaultSetting = @"";
				colvarDepositBagNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDepositBagNo);
				
				TableSchema.TableColumn colvarTotalActualCollected = new TableSchema.TableColumn(schema);
				colvarTotalActualCollected.ColumnName = "TotalActualCollected";
				colvarTotalActualCollected.DataType = DbType.Currency;
				colvarTotalActualCollected.MaxLength = 0;
				colvarTotalActualCollected.AutoIncrement = false;
				colvarTotalActualCollected.IsNullable = false;
				colvarTotalActualCollected.IsPrimaryKey = false;
				colvarTotalActualCollected.IsForeignKey = false;
				colvarTotalActualCollected.IsReadOnly = false;
				colvarTotalActualCollected.DefaultSetting = @"";
				colvarTotalActualCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTotalActualCollected);
				
				TableSchema.TableColumn colvarClosingCashOut = new TableSchema.TableColumn(schema);
				colvarClosingCashOut.ColumnName = "ClosingCashOut";
				colvarClosingCashOut.DataType = DbType.Currency;
				colvarClosingCashOut.MaxLength = 0;
				colvarClosingCashOut.AutoIncrement = false;
				colvarClosingCashOut.IsNullable = false;
				colvarClosingCashOut.IsPrimaryKey = false;
				colvarClosingCashOut.IsForeignKey = false;
				colvarClosingCashOut.IsReadOnly = false;
				colvarClosingCashOut.DefaultSetting = @"";
				colvarClosingCashOut.ForeignKeyTableName = "";
				schema.Columns.Add(colvarClosingCashOut);
				
				TableSchema.TableColumn colvarVariance = new TableSchema.TableColumn(schema);
				colvarVariance.ColumnName = "Variance";
				colvarVariance.DataType = DbType.Currency;
				colvarVariance.MaxLength = 0;
				colvarVariance.AutoIncrement = false;
				colvarVariance.IsNullable = false;
				colvarVariance.IsPrimaryKey = false;
				colvarVariance.IsForeignKey = false;
				colvarVariance.IsReadOnly = false;
				colvarVariance.DefaultSetting = @"";
				colvarVariance.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVariance);
				
				TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
				colvarPointOfSaleID.ColumnName = "PointOfSaleID";
				colvarPointOfSaleID.DataType = DbType.Int32;
				colvarPointOfSaleID.MaxLength = 0;
				colvarPointOfSaleID.AutoIncrement = false;
				colvarPointOfSaleID.IsNullable = false;
				colvarPointOfSaleID.IsPrimaryKey = false;
				colvarPointOfSaleID.IsForeignKey = true;
				colvarPointOfSaleID.IsReadOnly = false;
				colvarPointOfSaleID.DefaultSetting = @"";
				
					colvarPointOfSaleID.ForeignKeyTableName = "PointOfSale";
				schema.Columns.Add(colvarPointOfSaleID);
				
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
				
				TableSchema.TableColumn colvarUserfloat6 = new TableSchema.TableColumn(schema);
				colvarUserfloat6.ColumnName = "userfloat6";
				colvarUserfloat6.DataType = DbType.Currency;
				colvarUserfloat6.MaxLength = 0;
				colvarUserfloat6.AutoIncrement = false;
				colvarUserfloat6.IsNullable = true;
				colvarUserfloat6.IsPrimaryKey = false;
				colvarUserfloat6.IsForeignKey = false;
				colvarUserfloat6.IsReadOnly = false;
				colvarUserfloat6.DefaultSetting = @"";
				colvarUserfloat6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat6);
				
				TableSchema.TableColumn colvarUserfloat7 = new TableSchema.TableColumn(schema);
				colvarUserfloat7.ColumnName = "userfloat7";
				colvarUserfloat7.DataType = DbType.Currency;
				colvarUserfloat7.MaxLength = 0;
				colvarUserfloat7.AutoIncrement = false;
				colvarUserfloat7.IsNullable = true;
				colvarUserfloat7.IsPrimaryKey = false;
				colvarUserfloat7.IsForeignKey = false;
				colvarUserfloat7.IsReadOnly = false;
				colvarUserfloat7.DefaultSetting = @"";
				colvarUserfloat7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat7);
				
				TableSchema.TableColumn colvarUserfloat8 = new TableSchema.TableColumn(schema);
				colvarUserfloat8.ColumnName = "userfloat8";
				colvarUserfloat8.DataType = DbType.Currency;
				colvarUserfloat8.MaxLength = 0;
				colvarUserfloat8.AutoIncrement = false;
				colvarUserfloat8.IsNullable = true;
				colvarUserfloat8.IsPrimaryKey = false;
				colvarUserfloat8.IsForeignKey = false;
				colvarUserfloat8.IsReadOnly = false;
				colvarUserfloat8.DefaultSetting = @"";
				colvarUserfloat8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat8);
				
				TableSchema.TableColumn colvarUserfloat9 = new TableSchema.TableColumn(schema);
				colvarUserfloat9.ColumnName = "userfloat9";
				colvarUserfloat9.DataType = DbType.Currency;
				colvarUserfloat9.MaxLength = 0;
				colvarUserfloat9.AutoIncrement = false;
				colvarUserfloat9.IsNullable = true;
				colvarUserfloat9.IsPrimaryKey = false;
				colvarUserfloat9.IsForeignKey = false;
				colvarUserfloat9.IsReadOnly = false;
				colvarUserfloat9.DefaultSetting = @"";
				colvarUserfloat9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat9);
				
				TableSchema.TableColumn colvarUserfloat10 = new TableSchema.TableColumn(schema);
				colvarUserfloat10.ColumnName = "userfloat10";
				colvarUserfloat10.DataType = DbType.Currency;
				colvarUserfloat10.MaxLength = 0;
				colvarUserfloat10.AutoIncrement = false;
				colvarUserfloat10.IsNullable = true;
				colvarUserfloat10.IsPrimaryKey = false;
				colvarUserfloat10.IsForeignKey = false;
				colvarUserfloat10.IsReadOnly = false;
				colvarUserfloat10.DefaultSetting = @"";
				colvarUserfloat10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat10);
				
				TableSchema.TableColumn colvarUserfloat11 = new TableSchema.TableColumn(schema);
				colvarUserfloat11.ColumnName = "userfloat11";
				colvarUserfloat11.DataType = DbType.Currency;
				colvarUserfloat11.MaxLength = 0;
				colvarUserfloat11.AutoIncrement = false;
				colvarUserfloat11.IsNullable = true;
				colvarUserfloat11.IsPrimaryKey = false;
				colvarUserfloat11.IsForeignKey = false;
				colvarUserfloat11.IsReadOnly = false;
				colvarUserfloat11.DefaultSetting = @"";
				colvarUserfloat11.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat11);
				
				TableSchema.TableColumn colvarUserfloat12 = new TableSchema.TableColumn(schema);
				colvarUserfloat12.ColumnName = "userfloat12";
				colvarUserfloat12.DataType = DbType.Currency;
				colvarUserfloat12.MaxLength = 0;
				colvarUserfloat12.AutoIncrement = false;
				colvarUserfloat12.IsNullable = true;
				colvarUserfloat12.IsPrimaryKey = false;
				colvarUserfloat12.IsForeignKey = false;
				colvarUserfloat12.IsReadOnly = false;
				colvarUserfloat12.DefaultSetting = @"";
				colvarUserfloat12.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat12);
				
				TableSchema.TableColumn colvarUserfloat13 = new TableSchema.TableColumn(schema);
				colvarUserfloat13.ColumnName = "userfloat13";
				colvarUserfloat13.DataType = DbType.Currency;
				colvarUserfloat13.MaxLength = 0;
				colvarUserfloat13.AutoIncrement = false;
				colvarUserfloat13.IsNullable = true;
				colvarUserfloat13.IsPrimaryKey = false;
				colvarUserfloat13.IsForeignKey = false;
				colvarUserfloat13.IsReadOnly = false;
				colvarUserfloat13.DefaultSetting = @"";
				colvarUserfloat13.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat13);
				
				TableSchema.TableColumn colvarUserfloat14 = new TableSchema.TableColumn(schema);
				colvarUserfloat14.ColumnName = "userfloat14";
				colvarUserfloat14.DataType = DbType.Currency;
				colvarUserfloat14.MaxLength = 0;
				colvarUserfloat14.AutoIncrement = false;
				colvarUserfloat14.IsNullable = true;
				colvarUserfloat14.IsPrimaryKey = false;
				colvarUserfloat14.IsForeignKey = false;
				colvarUserfloat14.IsReadOnly = false;
				colvarUserfloat14.DefaultSetting = @"";
				colvarUserfloat14.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat14);
				
				TableSchema.TableColumn colvarUserfloat15 = new TableSchema.TableColumn(schema);
				colvarUserfloat15.ColumnName = "userfloat15";
				colvarUserfloat15.DataType = DbType.Currency;
				colvarUserfloat15.MaxLength = 0;
				colvarUserfloat15.AutoIncrement = false;
				colvarUserfloat15.IsNullable = true;
				colvarUserfloat15.IsPrimaryKey = false;
				colvarUserfloat15.IsForeignKey = false;
				colvarUserfloat15.IsReadOnly = false;
				colvarUserfloat15.DefaultSetting = @"";
				colvarUserfloat15.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat15);
				
				TableSchema.TableColumn colvarNetsCashCardCollected = new TableSchema.TableColumn(schema);
				colvarNetsCashCardCollected.ColumnName = "NetsCashCardCollected";
				colvarNetsCashCardCollected.DataType = DbType.Currency;
				colvarNetsCashCardCollected.MaxLength = 0;
				colvarNetsCashCardCollected.AutoIncrement = false;
				colvarNetsCashCardCollected.IsNullable = true;
				colvarNetsCashCardCollected.IsPrimaryKey = false;
				colvarNetsCashCardCollected.IsForeignKey = false;
				colvarNetsCashCardCollected.IsReadOnly = false;
				colvarNetsCashCardCollected.DefaultSetting = @"";
				colvarNetsCashCardCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNetsCashCardCollected);
				
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
				
				TableSchema.TableColumn colvarNetsFlashPayCollected = new TableSchema.TableColumn(schema);
				colvarNetsFlashPayCollected.ColumnName = "NetsFlashPayCollected";
				colvarNetsFlashPayCollected.DataType = DbType.Currency;
				colvarNetsFlashPayCollected.MaxLength = 0;
				colvarNetsFlashPayCollected.AutoIncrement = false;
				colvarNetsFlashPayCollected.IsNullable = true;
				colvarNetsFlashPayCollected.IsPrimaryKey = false;
				colvarNetsFlashPayCollected.IsForeignKey = false;
				colvarNetsFlashPayCollected.IsReadOnly = false;
				colvarNetsFlashPayCollected.DefaultSetting = @"";
				colvarNetsFlashPayCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNetsFlashPayCollected);
				
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
				
				TableSchema.TableColumn colvarNetsATMCardCollected = new TableSchema.TableColumn(schema);
				colvarNetsATMCardCollected.ColumnName = "NetsATMCardCollected";
				colvarNetsATMCardCollected.DataType = DbType.Currency;
				colvarNetsATMCardCollected.MaxLength = 0;
				colvarNetsATMCardCollected.AutoIncrement = false;
				colvarNetsATMCardCollected.IsNullable = true;
				colvarNetsATMCardCollected.IsPrimaryKey = false;
				colvarNetsATMCardCollected.IsForeignKey = false;
				colvarNetsATMCardCollected.IsReadOnly = false;
				colvarNetsATMCardCollected.DefaultSetting = @"";
				colvarNetsATMCardCollected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNetsATMCardCollected);
				
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
				
				TableSchema.TableColumn colvarForeignCurrency1Collected = new TableSchema.TableColumn(schema);
				colvarForeignCurrency1Collected.ColumnName = "ForeignCurrency1Collected";
				colvarForeignCurrency1Collected.DataType = DbType.Currency;
				colvarForeignCurrency1Collected.MaxLength = 0;
				colvarForeignCurrency1Collected.AutoIncrement = false;
				colvarForeignCurrency1Collected.IsNullable = true;
				colvarForeignCurrency1Collected.IsPrimaryKey = false;
				colvarForeignCurrency1Collected.IsForeignKey = false;
				colvarForeignCurrency1Collected.IsReadOnly = false;
				colvarForeignCurrency1Collected.DefaultSetting = @"";
				colvarForeignCurrency1Collected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency1Collected);
				
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
				
				TableSchema.TableColumn colvarForeignCurrency2Collected = new TableSchema.TableColumn(schema);
				colvarForeignCurrency2Collected.ColumnName = "ForeignCurrency2Collected";
				colvarForeignCurrency2Collected.DataType = DbType.Currency;
				colvarForeignCurrency2Collected.MaxLength = 0;
				colvarForeignCurrency2Collected.AutoIncrement = false;
				colvarForeignCurrency2Collected.IsNullable = true;
				colvarForeignCurrency2Collected.IsPrimaryKey = false;
				colvarForeignCurrency2Collected.IsForeignKey = false;
				colvarForeignCurrency2Collected.IsReadOnly = false;
				colvarForeignCurrency2Collected.DefaultSetting = @"";
				colvarForeignCurrency2Collected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency2Collected);
				
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
				
				TableSchema.TableColumn colvarForeignCurrency3Collected = new TableSchema.TableColumn(schema);
				colvarForeignCurrency3Collected.ColumnName = "ForeignCurrency3Collected";
				colvarForeignCurrency3Collected.DataType = DbType.Currency;
				colvarForeignCurrency3Collected.MaxLength = 0;
				colvarForeignCurrency3Collected.AutoIncrement = false;
				colvarForeignCurrency3Collected.IsNullable = true;
				colvarForeignCurrency3Collected.IsPrimaryKey = false;
				colvarForeignCurrency3Collected.IsForeignKey = false;
				colvarForeignCurrency3Collected.IsReadOnly = false;
				colvarForeignCurrency3Collected.DefaultSetting = @"";
				colvarForeignCurrency3Collected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency3Collected);
				
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
				
				TableSchema.TableColumn colvarForeignCurrency4Collected = new TableSchema.TableColumn(schema);
				colvarForeignCurrency4Collected.ColumnName = "ForeignCurrency4Collected";
				colvarForeignCurrency4Collected.DataType = DbType.Currency;
				colvarForeignCurrency4Collected.MaxLength = 0;
				colvarForeignCurrency4Collected.AutoIncrement = false;
				colvarForeignCurrency4Collected.IsNullable = true;
				colvarForeignCurrency4Collected.IsPrimaryKey = false;
				colvarForeignCurrency4Collected.IsForeignKey = false;
				colvarForeignCurrency4Collected.IsReadOnly = false;
				colvarForeignCurrency4Collected.DefaultSetting = @"";
				colvarForeignCurrency4Collected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency4Collected);
				
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
				
				TableSchema.TableColumn colvarForeignCurrency5Collected = new TableSchema.TableColumn(schema);
				colvarForeignCurrency5Collected.ColumnName = "ForeignCurrency5Collected";
				colvarForeignCurrency5Collected.DataType = DbType.Currency;
				colvarForeignCurrency5Collected.MaxLength = 0;
				colvarForeignCurrency5Collected.AutoIncrement = false;
				colvarForeignCurrency5Collected.IsNullable = true;
				colvarForeignCurrency5Collected.IsPrimaryKey = false;
				colvarForeignCurrency5Collected.IsForeignKey = false;
				colvarForeignCurrency5Collected.IsReadOnly = false;
				colvarForeignCurrency5Collected.DefaultSetting = @"";
				colvarForeignCurrency5Collected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeignCurrency5Collected);
				
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
				
				TableSchema.TableColumn colvarPay7Collected = new TableSchema.TableColumn(schema);
				colvarPay7Collected.ColumnName = "Pay7Collected";
				colvarPay7Collected.DataType = DbType.Currency;
				colvarPay7Collected.MaxLength = 0;
				colvarPay7Collected.AutoIncrement = false;
				colvarPay7Collected.IsNullable = true;
				colvarPay7Collected.IsPrimaryKey = false;
				colvarPay7Collected.IsForeignKey = false;
				colvarPay7Collected.IsReadOnly = false;
				colvarPay7Collected.DefaultSetting = @"";
				colvarPay7Collected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay7Collected);
				
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
				
				TableSchema.TableColumn colvarPay8Collected = new TableSchema.TableColumn(schema);
				colvarPay8Collected.ColumnName = "Pay8Collected";
				colvarPay8Collected.DataType = DbType.Currency;
				colvarPay8Collected.MaxLength = 0;
				colvarPay8Collected.AutoIncrement = false;
				colvarPay8Collected.IsNullable = true;
				colvarPay8Collected.IsPrimaryKey = false;
				colvarPay8Collected.IsForeignKey = false;
				colvarPay8Collected.IsReadOnly = false;
				colvarPay8Collected.DefaultSetting = @"";
				colvarPay8Collected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay8Collected);
				
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
				
				TableSchema.TableColumn colvarPay9Collected = new TableSchema.TableColumn(schema);
				colvarPay9Collected.ColumnName = "Pay9Collected";
				colvarPay9Collected.DataType = DbType.Currency;
				colvarPay9Collected.MaxLength = 0;
				colvarPay9Collected.AutoIncrement = false;
				colvarPay9Collected.IsNullable = true;
				colvarPay9Collected.IsPrimaryKey = false;
				colvarPay9Collected.IsForeignKey = false;
				colvarPay9Collected.IsReadOnly = false;
				colvarPay9Collected.DefaultSetting = @"";
				colvarPay9Collected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay9Collected);
				
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
				
				TableSchema.TableColumn colvarPay10Collected = new TableSchema.TableColumn(schema);
				colvarPay10Collected.ColumnName = "Pay10Collected";
				colvarPay10Collected.DataType = DbType.Currency;
				colvarPay10Collected.MaxLength = 0;
				colvarPay10Collected.AutoIncrement = false;
				colvarPay10Collected.IsNullable = true;
				colvarPay10Collected.IsPrimaryKey = false;
				colvarPay10Collected.IsForeignKey = false;
				colvarPay10Collected.IsReadOnly = false;
				colvarPay10Collected.DefaultSetting = @"";
				colvarPay10Collected.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPay10Collected);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("CounterCloseLog",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("CounterCloseLogID")]
		[Bindable(true)]
		public int CounterCloseLogID 
		{
			get { return GetColumnValue<int>(Columns.CounterCloseLogID); }
			set { SetColumnValue(Columns.CounterCloseLogID, value); }
		}
		  
		[XmlAttribute("CounterCloseID")]
		[Bindable(true)]
		public string CounterCloseID 
		{
			get { return GetColumnValue<string>(Columns.CounterCloseID); }
			set { SetColumnValue(Columns.CounterCloseID, value); }
		}
		  
		[XmlAttribute("FloatBalance")]
		[Bindable(true)]
		public decimal FloatBalance 
		{
			get { return GetColumnValue<decimal>(Columns.FloatBalance); }
			set { SetColumnValue(Columns.FloatBalance, value); }
		}
		  
		[XmlAttribute("StartTime")]
		[Bindable(true)]
		public DateTime StartTime 
		{
			get { return GetColumnValue<DateTime>(Columns.StartTime); }
			set { SetColumnValue(Columns.StartTime, value); }
		}
		  
		[XmlAttribute("EndTime")]
		[Bindable(true)]
		public DateTime EndTime 
		{
			get { return GetColumnValue<DateTime>(Columns.EndTime); }
			set { SetColumnValue(Columns.EndTime, value); }
		}
		  
		[XmlAttribute("Cashier")]
		[Bindable(true)]
		public string Cashier 
		{
			get { return GetColumnValue<string>(Columns.Cashier); }
			set { SetColumnValue(Columns.Cashier, value); }
		}
		  
		[XmlAttribute("Supervisor")]
		[Bindable(true)]
		public string Supervisor 
		{
			get { return GetColumnValue<string>(Columns.Supervisor); }
			set { SetColumnValue(Columns.Supervisor, value); }
		}
		  
		[XmlAttribute("CashIn")]
		[Bindable(true)]
		public decimal CashIn 
		{
			get { return GetColumnValue<decimal>(Columns.CashIn); }
			set { SetColumnValue(Columns.CashIn, value); }
		}
		  
		[XmlAttribute("CashOut")]
		[Bindable(true)]
		public decimal CashOut 
		{
			get { return GetColumnValue<decimal>(Columns.CashOut); }
			set { SetColumnValue(Columns.CashOut, value); }
		}
		  
		[XmlAttribute("OpeningBalance")]
		[Bindable(true)]
		public decimal OpeningBalance 
		{
			get { return GetColumnValue<decimal>(Columns.OpeningBalance); }
			set { SetColumnValue(Columns.OpeningBalance, value); }
		}
		  
		[XmlAttribute("TotalSystemRecorded")]
		[Bindable(true)]
		public decimal TotalSystemRecorded 
		{
			get { return GetColumnValue<decimal>(Columns.TotalSystemRecorded); }
			set { SetColumnValue(Columns.TotalSystemRecorded, value); }
		}
		  
		[XmlAttribute("CashCollected")]
		[Bindable(true)]
		public decimal CashCollected 
		{
			get { return GetColumnValue<decimal>(Columns.CashCollected); }
			set { SetColumnValue(Columns.CashCollected, value); }
		}
		  
		[XmlAttribute("CashRecorded")]
		[Bindable(true)]
		public decimal? CashRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.CashRecorded); }
			set { SetColumnValue(Columns.CashRecorded, value); }
		}
		  
		[XmlAttribute("NetsCollected")]
		[Bindable(true)]
		public decimal NetsCollected 
		{
			get { return GetColumnValue<decimal>(Columns.NetsCollected); }
			set { SetColumnValue(Columns.NetsCollected, value); }
		}
		  
		[XmlAttribute("NetsRecorded")]
		[Bindable(true)]
		public decimal? NetsRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.NetsRecorded); }
			set { SetColumnValue(Columns.NetsRecorded, value); }
		}
		  
		[XmlAttribute("NetsTerminalID")]
		[Bindable(true)]
		public string NetsTerminalID 
		{
			get { return GetColumnValue<string>(Columns.NetsTerminalID); }
			set { SetColumnValue(Columns.NetsTerminalID, value); }
		}
		  
		[XmlAttribute("VisaCollected")]
		[Bindable(true)]
		public decimal VisaCollected 
		{
			get { return GetColumnValue<decimal>(Columns.VisaCollected); }
			set { SetColumnValue(Columns.VisaCollected, value); }
		}
		  
		[XmlAttribute("VisaRecorded")]
		[Bindable(true)]
		public decimal? VisaRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.VisaRecorded); }
			set { SetColumnValue(Columns.VisaRecorded, value); }
		}
		  
		[XmlAttribute("VisaBatchNo")]
		[Bindable(true)]
		public string VisaBatchNo 
		{
			get { return GetColumnValue<string>(Columns.VisaBatchNo); }
			set { SetColumnValue(Columns.VisaBatchNo, value); }
		}
		  
		[XmlAttribute("AmexCollected")]
		[Bindable(true)]
		public decimal AmexCollected 
		{
			get { return GetColumnValue<decimal>(Columns.AmexCollected); }
			set { SetColumnValue(Columns.AmexCollected, value); }
		}
		  
		[XmlAttribute("AmexRecorded")]
		[Bindable(true)]
		public decimal? AmexRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.AmexRecorded); }
			set { SetColumnValue(Columns.AmexRecorded, value); }
		}
		  
		[XmlAttribute("AmexBatchNo")]
		[Bindable(true)]
		public string AmexBatchNo 
		{
			get { return GetColumnValue<string>(Columns.AmexBatchNo); }
			set { SetColumnValue(Columns.AmexBatchNo, value); }
		}
		  
		[XmlAttribute("ChinaNetsCollected")]
		[Bindable(true)]
		public decimal ChinaNetsCollected 
		{
			get { return GetColumnValue<decimal>(Columns.ChinaNetsCollected); }
			set { SetColumnValue(Columns.ChinaNetsCollected, value); }
		}
		  
		[XmlAttribute("ChinaNetsRecorded")]
		[Bindable(true)]
		public decimal? ChinaNetsRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.ChinaNetsRecorded); }
			set { SetColumnValue(Columns.ChinaNetsRecorded, value); }
		}
		  
		[XmlAttribute("ChinaNetsTerminalID")]
		[Bindable(true)]
		public string ChinaNetsTerminalID 
		{
			get { return GetColumnValue<string>(Columns.ChinaNetsTerminalID); }
			set { SetColumnValue(Columns.ChinaNetsTerminalID, value); }
		}
		  
		[XmlAttribute("VoucherCollected")]
		[Bindable(true)]
		public decimal VoucherCollected 
		{
			get { return GetColumnValue<decimal>(Columns.VoucherCollected); }
			set { SetColumnValue(Columns.VoucherCollected, value); }
		}
		  
		[XmlAttribute("VoucherRecorded")]
		[Bindable(true)]
		public decimal? VoucherRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.VoucherRecorded); }
			set { SetColumnValue(Columns.VoucherRecorded, value); }
		}
		  
		[XmlAttribute("DepositBagNo")]
		[Bindable(true)]
		public string DepositBagNo 
		{
			get { return GetColumnValue<string>(Columns.DepositBagNo); }
			set { SetColumnValue(Columns.DepositBagNo, value); }
		}
		  
		[XmlAttribute("TotalActualCollected")]
		[Bindable(true)]
		public decimal TotalActualCollected 
		{
			get { return GetColumnValue<decimal>(Columns.TotalActualCollected); }
			set { SetColumnValue(Columns.TotalActualCollected, value); }
		}
		  
		[XmlAttribute("ClosingCashOut")]
		[Bindable(true)]
		public decimal ClosingCashOut 
		{
			get { return GetColumnValue<decimal>(Columns.ClosingCashOut); }
			set { SetColumnValue(Columns.ClosingCashOut, value); }
		}
		  
		[XmlAttribute("Variance")]
		[Bindable(true)]
		public decimal Variance 
		{
			get { return GetColumnValue<decimal>(Columns.Variance); }
			set { SetColumnValue(Columns.Variance, value); }
		}
		  
		[XmlAttribute("PointOfSaleID")]
		[Bindable(true)]
		public int PointOfSaleID 
		{
			get { return GetColumnValue<int>(Columns.PointOfSaleID); }
			set { SetColumnValue(Columns.PointOfSaleID, value); }
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
		  
		[XmlAttribute("Userfloat6")]
		[Bindable(true)]
		public decimal? Userfloat6 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat6); }
			set { SetColumnValue(Columns.Userfloat6, value); }
		}
		  
		[XmlAttribute("Userfloat7")]
		[Bindable(true)]
		public decimal? Userfloat7 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat7); }
			set { SetColumnValue(Columns.Userfloat7, value); }
		}
		  
		[XmlAttribute("Userfloat8")]
		[Bindable(true)]
		public decimal? Userfloat8 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat8); }
			set { SetColumnValue(Columns.Userfloat8, value); }
		}
		  
		[XmlAttribute("Userfloat9")]
		[Bindable(true)]
		public decimal? Userfloat9 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat9); }
			set { SetColumnValue(Columns.Userfloat9, value); }
		}
		  
		[XmlAttribute("Userfloat10")]
		[Bindable(true)]
		public decimal? Userfloat10 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat10); }
			set { SetColumnValue(Columns.Userfloat10, value); }
		}
		  
		[XmlAttribute("Userfloat11")]
		[Bindable(true)]
		public decimal? Userfloat11 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat11); }
			set { SetColumnValue(Columns.Userfloat11, value); }
		}
		  
		[XmlAttribute("Userfloat12")]
		[Bindable(true)]
		public decimal? Userfloat12 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat12); }
			set { SetColumnValue(Columns.Userfloat12, value); }
		}
		  
		[XmlAttribute("Userfloat13")]
		[Bindable(true)]
		public decimal? Userfloat13 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat13); }
			set { SetColumnValue(Columns.Userfloat13, value); }
		}
		  
		[XmlAttribute("Userfloat14")]
		[Bindable(true)]
		public decimal? Userfloat14 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat14); }
			set { SetColumnValue(Columns.Userfloat14, value); }
		}
		  
		[XmlAttribute("Userfloat15")]
		[Bindable(true)]
		public decimal? Userfloat15 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat15); }
			set { SetColumnValue(Columns.Userfloat15, value); }
		}
		  
		[XmlAttribute("NetsCashCardCollected")]
		[Bindable(true)]
		public decimal? NetsCashCardCollected 
		{
			get { return GetColumnValue<decimal?>(Columns.NetsCashCardCollected); }
			set { SetColumnValue(Columns.NetsCashCardCollected, value); }
		}
		  
		[XmlAttribute("NetsCashCardRecorded")]
		[Bindable(true)]
		public decimal? NetsCashCardRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.NetsCashCardRecorded); }
			set { SetColumnValue(Columns.NetsCashCardRecorded, value); }
		}
		  
		[XmlAttribute("NetsFlashPayCollected")]
		[Bindable(true)]
		public decimal? NetsFlashPayCollected 
		{
			get { return GetColumnValue<decimal?>(Columns.NetsFlashPayCollected); }
			set { SetColumnValue(Columns.NetsFlashPayCollected, value); }
		}
		  
		[XmlAttribute("NetsFlashPayRecorded")]
		[Bindable(true)]
		public decimal? NetsFlashPayRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.NetsFlashPayRecorded); }
			set { SetColumnValue(Columns.NetsFlashPayRecorded, value); }
		}
		  
		[XmlAttribute("NetsATMCardCollected")]
		[Bindable(true)]
		public decimal? NetsATMCardCollected 
		{
			get { return GetColumnValue<decimal?>(Columns.NetsATMCardCollected); }
			set { SetColumnValue(Columns.NetsATMCardCollected, value); }
		}
		  
		[XmlAttribute("NetsATMCardRecorded")]
		[Bindable(true)]
		public decimal? NetsATMCardRecorded 
		{
			get { return GetColumnValue<decimal?>(Columns.NetsATMCardRecorded); }
			set { SetColumnValue(Columns.NetsATMCardRecorded, value); }
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
		  
		[XmlAttribute("ForeignCurrency1Collected")]
		[Bindable(true)]
		public decimal? ForeignCurrency1Collected 
		{
			get { return GetColumnValue<decimal?>(Columns.ForeignCurrency1Collected); }
			set { SetColumnValue(Columns.ForeignCurrency1Collected, value); }
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
		  
		[XmlAttribute("ForeignCurrency2Collected")]
		[Bindable(true)]
		public decimal? ForeignCurrency2Collected 
		{
			get { return GetColumnValue<decimal?>(Columns.ForeignCurrency2Collected); }
			set { SetColumnValue(Columns.ForeignCurrency2Collected, value); }
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
		  
		[XmlAttribute("ForeignCurrency3Collected")]
		[Bindable(true)]
		public decimal? ForeignCurrency3Collected 
		{
			get { return GetColumnValue<decimal?>(Columns.ForeignCurrency3Collected); }
			set { SetColumnValue(Columns.ForeignCurrency3Collected, value); }
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
		  
		[XmlAttribute("ForeignCurrency4Collected")]
		[Bindable(true)]
		public decimal? ForeignCurrency4Collected 
		{
			get { return GetColumnValue<decimal?>(Columns.ForeignCurrency4Collected); }
			set { SetColumnValue(Columns.ForeignCurrency4Collected, value); }
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
		  
		[XmlAttribute("ForeignCurrency5Collected")]
		[Bindable(true)]
		public decimal? ForeignCurrency5Collected 
		{
			get { return GetColumnValue<decimal?>(Columns.ForeignCurrency5Collected); }
			set { SetColumnValue(Columns.ForeignCurrency5Collected, value); }
		}
		  
		[XmlAttribute("TotalForeignCurrency")]
		[Bindable(true)]
		public decimal? TotalForeignCurrency 
		{
			get { return GetColumnValue<decimal?>(Columns.TotalForeignCurrency); }
			set { SetColumnValue(Columns.TotalForeignCurrency, value); }
		}
		  
		[XmlAttribute("Pay7Collected")]
		[Bindable(true)]
		public decimal? Pay7Collected 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay7Collected); }
			set { SetColumnValue(Columns.Pay7Collected, value); }
		}
		  
		[XmlAttribute("Pay7Recorded")]
		[Bindable(true)]
		public decimal? Pay7Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay7Recorded); }
			set { SetColumnValue(Columns.Pay7Recorded, value); }
		}
		  
		[XmlAttribute("Pay8Collected")]
		[Bindable(true)]
		public decimal? Pay8Collected 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay8Collected); }
			set { SetColumnValue(Columns.Pay8Collected, value); }
		}
		  
		[XmlAttribute("Pay8Recorded")]
		[Bindable(true)]
		public decimal? Pay8Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay8Recorded); }
			set { SetColumnValue(Columns.Pay8Recorded, value); }
		}
		  
		[XmlAttribute("Pay9Collected")]
		[Bindable(true)]
		public decimal? Pay9Collected 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay9Collected); }
			set { SetColumnValue(Columns.Pay9Collected, value); }
		}
		  
		[XmlAttribute("Pay9Recorded")]
		[Bindable(true)]
		public decimal? Pay9Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay9Recorded); }
			set { SetColumnValue(Columns.Pay9Recorded, value); }
		}
		  
		[XmlAttribute("Pay10Collected")]
		[Bindable(true)]
		public decimal? Pay10Collected 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay10Collected); }
			set { SetColumnValue(Columns.Pay10Collected, value); }
		}
		  
		[XmlAttribute("Pay10Recorded")]
		[Bindable(true)]
		public decimal? Pay10Recorded 
		{
			get { return GetColumnValue<decimal?>(Columns.Pay10Recorded); }
			set { SetColumnValue(Columns.Pay10Recorded, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a PointOfSale ActiveRecord object related to this CounterCloseLog
		/// 
		/// </summary>
		public PowerPOS.PointOfSale PointOfSale
		{
			get { return PowerPOS.PointOfSale.FetchByID(this.PointOfSaleID); }
			set { SetColumnValue("PointOfSaleID", value.PointOfSaleID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varCounterCloseID,decimal varFloatBalance,DateTime varStartTime,DateTime varEndTime,string varCashier,string varSupervisor,decimal varCashIn,decimal varCashOut,decimal varOpeningBalance,decimal varTotalSystemRecorded,decimal varCashCollected,decimal? varCashRecorded,decimal varNetsCollected,decimal? varNetsRecorded,string varNetsTerminalID,decimal varVisaCollected,decimal? varVisaRecorded,string varVisaBatchNo,decimal varAmexCollected,decimal? varAmexRecorded,string varAmexBatchNo,decimal varChinaNetsCollected,decimal? varChinaNetsRecorded,string varChinaNetsTerminalID,decimal varVoucherCollected,decimal? varVoucherRecorded,string varDepositBagNo,decimal varTotalActualCollected,decimal varClosingCashOut,decimal varVariance,int varPointOfSaleID,DateTime varCreatedOn,string varCreatedBy,DateTime varModifiedOn,string varModifiedBy,Guid varUniqueID,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5,decimal? varUserfloat6,decimal? varUserfloat7,decimal? varUserfloat8,decimal? varUserfloat9,decimal? varUserfloat10,decimal? varUserfloat11,decimal? varUserfloat12,decimal? varUserfloat13,decimal? varUserfloat14,decimal? varUserfloat15,decimal? varNetsCashCardCollected,decimal? varNetsCashCardRecorded,decimal? varNetsFlashPayCollected,decimal? varNetsFlashPayRecorded,decimal? varNetsATMCardCollected,decimal? varNetsATMCardRecorded,string varForeignCurrency1,decimal? varForeignCurrency1Recorded,decimal? varForeignCurrency1Collected,string varForeignCurrency2,decimal? varForeignCurrency2Recorded,decimal? varForeignCurrency2Collected,string varForeignCurrency3,decimal? varForeignCurrency3Recorded,decimal? varForeignCurrency3Collected,string varForeignCurrency4,decimal? varForeignCurrency4Recorded,decimal? varForeignCurrency4Collected,string varForeignCurrency5,decimal? varForeignCurrency5Recorded,decimal? varForeignCurrency5Collected,decimal? varTotalForeignCurrency,decimal? varPay7Collected,decimal? varPay7Recorded,decimal? varPay8Collected,decimal? varPay8Recorded,decimal? varPay9Collected,decimal? varPay9Recorded,decimal? varPay10Collected,decimal? varPay10Recorded)
		{
			CounterCloseLog item = new CounterCloseLog();
			
			item.CounterCloseID = varCounterCloseID;
			
			item.FloatBalance = varFloatBalance;
			
			item.StartTime = varStartTime;
			
			item.EndTime = varEndTime;
			
			item.Cashier = varCashier;
			
			item.Supervisor = varSupervisor;
			
			item.CashIn = varCashIn;
			
			item.CashOut = varCashOut;
			
			item.OpeningBalance = varOpeningBalance;
			
			item.TotalSystemRecorded = varTotalSystemRecorded;
			
			item.CashCollected = varCashCollected;
			
			item.CashRecorded = varCashRecorded;
			
			item.NetsCollected = varNetsCollected;
			
			item.NetsRecorded = varNetsRecorded;
			
			item.NetsTerminalID = varNetsTerminalID;
			
			item.VisaCollected = varVisaCollected;
			
			item.VisaRecorded = varVisaRecorded;
			
			item.VisaBatchNo = varVisaBatchNo;
			
			item.AmexCollected = varAmexCollected;
			
			item.AmexRecorded = varAmexRecorded;
			
			item.AmexBatchNo = varAmexBatchNo;
			
			item.ChinaNetsCollected = varChinaNetsCollected;
			
			item.ChinaNetsRecorded = varChinaNetsRecorded;
			
			item.ChinaNetsTerminalID = varChinaNetsTerminalID;
			
			item.VoucherCollected = varVoucherCollected;
			
			item.VoucherRecorded = varVoucherRecorded;
			
			item.DepositBagNo = varDepositBagNo;
			
			item.TotalActualCollected = varTotalActualCollected;
			
			item.ClosingCashOut = varClosingCashOut;
			
			item.Variance = varVariance;
			
			item.PointOfSaleID = varPointOfSaleID;
			
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
			
			item.Userfloat6 = varUserfloat6;
			
			item.Userfloat7 = varUserfloat7;
			
			item.Userfloat8 = varUserfloat8;
			
			item.Userfloat9 = varUserfloat9;
			
			item.Userfloat10 = varUserfloat10;
			
			item.Userfloat11 = varUserfloat11;
			
			item.Userfloat12 = varUserfloat12;
			
			item.Userfloat13 = varUserfloat13;
			
			item.Userfloat14 = varUserfloat14;
			
			item.Userfloat15 = varUserfloat15;
			
			item.NetsCashCardCollected = varNetsCashCardCollected;
			
			item.NetsCashCardRecorded = varNetsCashCardRecorded;
			
			item.NetsFlashPayCollected = varNetsFlashPayCollected;
			
			item.NetsFlashPayRecorded = varNetsFlashPayRecorded;
			
			item.NetsATMCardCollected = varNetsATMCardCollected;
			
			item.NetsATMCardRecorded = varNetsATMCardRecorded;
			
			item.ForeignCurrency1 = varForeignCurrency1;
			
			item.ForeignCurrency1Recorded = varForeignCurrency1Recorded;
			
			item.ForeignCurrency1Collected = varForeignCurrency1Collected;
			
			item.ForeignCurrency2 = varForeignCurrency2;
			
			item.ForeignCurrency2Recorded = varForeignCurrency2Recorded;
			
			item.ForeignCurrency2Collected = varForeignCurrency2Collected;
			
			item.ForeignCurrency3 = varForeignCurrency3;
			
			item.ForeignCurrency3Recorded = varForeignCurrency3Recorded;
			
			item.ForeignCurrency3Collected = varForeignCurrency3Collected;
			
			item.ForeignCurrency4 = varForeignCurrency4;
			
			item.ForeignCurrency4Recorded = varForeignCurrency4Recorded;
			
			item.ForeignCurrency4Collected = varForeignCurrency4Collected;
			
			item.ForeignCurrency5 = varForeignCurrency5;
			
			item.ForeignCurrency5Recorded = varForeignCurrency5Recorded;
			
			item.ForeignCurrency5Collected = varForeignCurrency5Collected;
			
			item.TotalForeignCurrency = varTotalForeignCurrency;
			
			item.Pay7Collected = varPay7Collected;
			
			item.Pay7Recorded = varPay7Recorded;
			
			item.Pay8Collected = varPay8Collected;
			
			item.Pay8Recorded = varPay8Recorded;
			
			item.Pay9Collected = varPay9Collected;
			
			item.Pay9Recorded = varPay9Recorded;
			
			item.Pay10Collected = varPay10Collected;
			
			item.Pay10Recorded = varPay10Recorded;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varCounterCloseLogID,string varCounterCloseID,decimal varFloatBalance,DateTime varStartTime,DateTime varEndTime,string varCashier,string varSupervisor,decimal varCashIn,decimal varCashOut,decimal varOpeningBalance,decimal varTotalSystemRecorded,decimal varCashCollected,decimal? varCashRecorded,decimal varNetsCollected,decimal? varNetsRecorded,string varNetsTerminalID,decimal varVisaCollected,decimal? varVisaRecorded,string varVisaBatchNo,decimal varAmexCollected,decimal? varAmexRecorded,string varAmexBatchNo,decimal varChinaNetsCollected,decimal? varChinaNetsRecorded,string varChinaNetsTerminalID,decimal varVoucherCollected,decimal? varVoucherRecorded,string varDepositBagNo,decimal varTotalActualCollected,decimal varClosingCashOut,decimal varVariance,int varPointOfSaleID,DateTime varCreatedOn,string varCreatedBy,DateTime varModifiedOn,string varModifiedBy,Guid varUniqueID,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5,decimal? varUserfloat6,decimal? varUserfloat7,decimal? varUserfloat8,decimal? varUserfloat9,decimal? varUserfloat10,decimal? varUserfloat11,decimal? varUserfloat12,decimal? varUserfloat13,decimal? varUserfloat14,decimal? varUserfloat15,decimal? varNetsCashCardCollected,decimal? varNetsCashCardRecorded,decimal? varNetsFlashPayCollected,decimal? varNetsFlashPayRecorded,decimal? varNetsATMCardCollected,decimal? varNetsATMCardRecorded,string varForeignCurrency1,decimal? varForeignCurrency1Recorded,decimal? varForeignCurrency1Collected,string varForeignCurrency2,decimal? varForeignCurrency2Recorded,decimal? varForeignCurrency2Collected,string varForeignCurrency3,decimal? varForeignCurrency3Recorded,decimal? varForeignCurrency3Collected,string varForeignCurrency4,decimal? varForeignCurrency4Recorded,decimal? varForeignCurrency4Collected,string varForeignCurrency5,decimal? varForeignCurrency5Recorded,decimal? varForeignCurrency5Collected,decimal? varTotalForeignCurrency,decimal? varPay7Collected,decimal? varPay7Recorded,decimal? varPay8Collected,decimal? varPay8Recorded,decimal? varPay9Collected,decimal? varPay9Recorded,decimal? varPay10Collected,decimal? varPay10Recorded)
		{
			CounterCloseLog item = new CounterCloseLog();
			
				item.CounterCloseLogID = varCounterCloseLogID;
			
				item.CounterCloseID = varCounterCloseID;
			
				item.FloatBalance = varFloatBalance;
			
				item.StartTime = varStartTime;
			
				item.EndTime = varEndTime;
			
				item.Cashier = varCashier;
			
				item.Supervisor = varSupervisor;
			
				item.CashIn = varCashIn;
			
				item.CashOut = varCashOut;
			
				item.OpeningBalance = varOpeningBalance;
			
				item.TotalSystemRecorded = varTotalSystemRecorded;
			
				item.CashCollected = varCashCollected;
			
				item.CashRecorded = varCashRecorded;
			
				item.NetsCollected = varNetsCollected;
			
				item.NetsRecorded = varNetsRecorded;
			
				item.NetsTerminalID = varNetsTerminalID;
			
				item.VisaCollected = varVisaCollected;
			
				item.VisaRecorded = varVisaRecorded;
			
				item.VisaBatchNo = varVisaBatchNo;
			
				item.AmexCollected = varAmexCollected;
			
				item.AmexRecorded = varAmexRecorded;
			
				item.AmexBatchNo = varAmexBatchNo;
			
				item.ChinaNetsCollected = varChinaNetsCollected;
			
				item.ChinaNetsRecorded = varChinaNetsRecorded;
			
				item.ChinaNetsTerminalID = varChinaNetsTerminalID;
			
				item.VoucherCollected = varVoucherCollected;
			
				item.VoucherRecorded = varVoucherRecorded;
			
				item.DepositBagNo = varDepositBagNo;
			
				item.TotalActualCollected = varTotalActualCollected;
			
				item.ClosingCashOut = varClosingCashOut;
			
				item.Variance = varVariance;
			
				item.PointOfSaleID = varPointOfSaleID;
			
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
			
				item.Userfloat6 = varUserfloat6;
			
				item.Userfloat7 = varUserfloat7;
			
				item.Userfloat8 = varUserfloat8;
			
				item.Userfloat9 = varUserfloat9;
			
				item.Userfloat10 = varUserfloat10;
			
				item.Userfloat11 = varUserfloat11;
			
				item.Userfloat12 = varUserfloat12;
			
				item.Userfloat13 = varUserfloat13;
			
				item.Userfloat14 = varUserfloat14;
			
				item.Userfloat15 = varUserfloat15;
			
				item.NetsCashCardCollected = varNetsCashCardCollected;
			
				item.NetsCashCardRecorded = varNetsCashCardRecorded;
			
				item.NetsFlashPayCollected = varNetsFlashPayCollected;
			
				item.NetsFlashPayRecorded = varNetsFlashPayRecorded;
			
				item.NetsATMCardCollected = varNetsATMCardCollected;
			
				item.NetsATMCardRecorded = varNetsATMCardRecorded;
			
				item.ForeignCurrency1 = varForeignCurrency1;
			
				item.ForeignCurrency1Recorded = varForeignCurrency1Recorded;
			
				item.ForeignCurrency1Collected = varForeignCurrency1Collected;
			
				item.ForeignCurrency2 = varForeignCurrency2;
			
				item.ForeignCurrency2Recorded = varForeignCurrency2Recorded;
			
				item.ForeignCurrency2Collected = varForeignCurrency2Collected;
			
				item.ForeignCurrency3 = varForeignCurrency3;
			
				item.ForeignCurrency3Recorded = varForeignCurrency3Recorded;
			
				item.ForeignCurrency3Collected = varForeignCurrency3Collected;
			
				item.ForeignCurrency4 = varForeignCurrency4;
			
				item.ForeignCurrency4Recorded = varForeignCurrency4Recorded;
			
				item.ForeignCurrency4Collected = varForeignCurrency4Collected;
			
				item.ForeignCurrency5 = varForeignCurrency5;
			
				item.ForeignCurrency5Recorded = varForeignCurrency5Recorded;
			
				item.ForeignCurrency5Collected = varForeignCurrency5Collected;
			
				item.TotalForeignCurrency = varTotalForeignCurrency;
			
				item.Pay7Collected = varPay7Collected;
			
				item.Pay7Recorded = varPay7Recorded;
			
				item.Pay8Collected = varPay8Collected;
			
				item.Pay8Recorded = varPay8Recorded;
			
				item.Pay9Collected = varPay9Collected;
			
				item.Pay9Recorded = varPay9Recorded;
			
				item.Pay10Collected = varPay10Collected;
			
				item.Pay10Recorded = varPay10Recorded;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn CounterCloseLogIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn CounterCloseIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn FloatBalanceColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn StartTimeColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn EndTimeColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CashierColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn SupervisorColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CashInColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CashOutColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn OpeningBalanceColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn TotalSystemRecordedColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn CashCollectedColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn CashRecordedColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsCollectedColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsRecordedColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsTerminalIDColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn VisaCollectedColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn VisaRecordedColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn VisaBatchNoColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn AmexCollectedColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn AmexRecordedColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn AmexBatchNoColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn ChinaNetsCollectedColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn ChinaNetsRecordedColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn ChinaNetsTerminalIDColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn VoucherCollectedColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn VoucherRecordedColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn DepositBagNoColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn TotalActualCollectedColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn ClosingCashOutColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn VarianceColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn PointOfSaleIDColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld1Column
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld2Column
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld3Column
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld4Column
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld5Column
        {
            get { return Schema.Columns[41]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld6Column
        {
            get { return Schema.Columns[42]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld7Column
        {
            get { return Schema.Columns[43]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld8Column
        {
            get { return Schema.Columns[44]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld9Column
        {
            get { return Schema.Columns[45]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld10Column
        {
            get { return Schema.Columns[46]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag1Column
        {
            get { return Schema.Columns[47]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag2Column
        {
            get { return Schema.Columns[48]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag3Column
        {
            get { return Schema.Columns[49]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag4Column
        {
            get { return Schema.Columns[50]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag5Column
        {
            get { return Schema.Columns[51]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat1Column
        {
            get { return Schema.Columns[52]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat2Column
        {
            get { return Schema.Columns[53]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat3Column
        {
            get { return Schema.Columns[54]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat4Column
        {
            get { return Schema.Columns[55]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat5Column
        {
            get { return Schema.Columns[56]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint1Column
        {
            get { return Schema.Columns[57]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint2Column
        {
            get { return Schema.Columns[58]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint3Column
        {
            get { return Schema.Columns[59]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint4Column
        {
            get { return Schema.Columns[60]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint5Column
        {
            get { return Schema.Columns[61]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat6Column
        {
            get { return Schema.Columns[62]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat7Column
        {
            get { return Schema.Columns[63]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat8Column
        {
            get { return Schema.Columns[64]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat9Column
        {
            get { return Schema.Columns[65]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat10Column
        {
            get { return Schema.Columns[66]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat11Column
        {
            get { return Schema.Columns[67]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat12Column
        {
            get { return Schema.Columns[68]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat13Column
        {
            get { return Schema.Columns[69]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat14Column
        {
            get { return Schema.Columns[70]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat15Column
        {
            get { return Schema.Columns[71]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsCashCardCollectedColumn
        {
            get { return Schema.Columns[72]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsCashCardRecordedColumn
        {
            get { return Schema.Columns[73]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsFlashPayCollectedColumn
        {
            get { return Schema.Columns[74]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsFlashPayRecordedColumn
        {
            get { return Schema.Columns[75]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsATMCardCollectedColumn
        {
            get { return Schema.Columns[76]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsATMCardRecordedColumn
        {
            get { return Schema.Columns[77]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency1Column
        {
            get { return Schema.Columns[78]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency1RecordedColumn
        {
            get { return Schema.Columns[79]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency1CollectedColumn
        {
            get { return Schema.Columns[80]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency2Column
        {
            get { return Schema.Columns[81]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency2RecordedColumn
        {
            get { return Schema.Columns[82]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency2CollectedColumn
        {
            get { return Schema.Columns[83]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency3Column
        {
            get { return Schema.Columns[84]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency3RecordedColumn
        {
            get { return Schema.Columns[85]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency3CollectedColumn
        {
            get { return Schema.Columns[86]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency4Column
        {
            get { return Schema.Columns[87]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency4RecordedColumn
        {
            get { return Schema.Columns[88]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency4CollectedColumn
        {
            get { return Schema.Columns[89]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency5Column
        {
            get { return Schema.Columns[90]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency5RecordedColumn
        {
            get { return Schema.Columns[91]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeignCurrency5CollectedColumn
        {
            get { return Schema.Columns[92]; }
        }
        
        
        
        public static TableSchema.TableColumn TotalForeignCurrencyColumn
        {
            get { return Schema.Columns[93]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay7CollectedColumn
        {
            get { return Schema.Columns[94]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay7RecordedColumn
        {
            get { return Schema.Columns[95]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay8CollectedColumn
        {
            get { return Schema.Columns[96]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay8RecordedColumn
        {
            get { return Schema.Columns[97]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay9CollectedColumn
        {
            get { return Schema.Columns[98]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay9RecordedColumn
        {
            get { return Schema.Columns[99]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay10CollectedColumn
        {
            get { return Schema.Columns[100]; }
        }
        
        
        
        public static TableSchema.TableColumn Pay10RecordedColumn
        {
            get { return Schema.Columns[101]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string CounterCloseLogID = @"CounterCloseLogID";
			 public static string CounterCloseID = @"CounterCloseID";
			 public static string FloatBalance = @"FloatBalance";
			 public static string StartTime = @"StartTime";
			 public static string EndTime = @"EndTime";
			 public static string Cashier = @"Cashier";
			 public static string Supervisor = @"Supervisor";
			 public static string CashIn = @"CashIn";
			 public static string CashOut = @"CashOut";
			 public static string OpeningBalance = @"OpeningBalance";
			 public static string TotalSystemRecorded = @"TotalSystemRecorded";
			 public static string CashCollected = @"CashCollected";
			 public static string CashRecorded = @"CashRecorded";
			 public static string NetsCollected = @"NetsCollected";
			 public static string NetsRecorded = @"NetsRecorded";
			 public static string NetsTerminalID = @"NetsTerminalID";
			 public static string VisaCollected = @"VisaCollected";
			 public static string VisaRecorded = @"VisaRecorded";
			 public static string VisaBatchNo = @"VisaBatchNo";
			 public static string AmexCollected = @"AmexCollected";
			 public static string AmexRecorded = @"AmexRecorded";
			 public static string AmexBatchNo = @"AmexBatchNo";
			 public static string ChinaNetsCollected = @"ChinaNetsCollected";
			 public static string ChinaNetsRecorded = @"ChinaNetsRecorded";
			 public static string ChinaNetsTerminalID = @"ChinaNetsTerminalID";
			 public static string VoucherCollected = @"VoucherCollected";
			 public static string VoucherRecorded = @"VoucherRecorded";
			 public static string DepositBagNo = @"DepositBagNo";
			 public static string TotalActualCollected = @"TotalActualCollected";
			 public static string ClosingCashOut = @"ClosingCashOut";
			 public static string Variance = @"Variance";
			 public static string PointOfSaleID = @"PointOfSaleID";
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
			 public static string Userfloat6 = @"userfloat6";
			 public static string Userfloat7 = @"userfloat7";
			 public static string Userfloat8 = @"userfloat8";
			 public static string Userfloat9 = @"userfloat9";
			 public static string Userfloat10 = @"userfloat10";
			 public static string Userfloat11 = @"userfloat11";
			 public static string Userfloat12 = @"userfloat12";
			 public static string Userfloat13 = @"userfloat13";
			 public static string Userfloat14 = @"userfloat14";
			 public static string Userfloat15 = @"userfloat15";
			 public static string NetsCashCardCollected = @"NetsCashCardCollected";
			 public static string NetsCashCardRecorded = @"NetsCashCardRecorded";
			 public static string NetsFlashPayCollected = @"NetsFlashPayCollected";
			 public static string NetsFlashPayRecorded = @"NetsFlashPayRecorded";
			 public static string NetsATMCardCollected = @"NetsATMCardCollected";
			 public static string NetsATMCardRecorded = @"NetsATMCardRecorded";
			 public static string ForeignCurrency1 = @"ForeignCurrency1";
			 public static string ForeignCurrency1Recorded = @"ForeignCurrency1Recorded";
			 public static string ForeignCurrency1Collected = @"ForeignCurrency1Collected";
			 public static string ForeignCurrency2 = @"ForeignCurrency2";
			 public static string ForeignCurrency2Recorded = @"ForeignCurrency2Recorded";
			 public static string ForeignCurrency2Collected = @"ForeignCurrency2Collected";
			 public static string ForeignCurrency3 = @"ForeignCurrency3";
			 public static string ForeignCurrency3Recorded = @"ForeignCurrency3Recorded";
			 public static string ForeignCurrency3Collected = @"ForeignCurrency3Collected";
			 public static string ForeignCurrency4 = @"ForeignCurrency4";
			 public static string ForeignCurrency4Recorded = @"ForeignCurrency4Recorded";
			 public static string ForeignCurrency4Collected = @"ForeignCurrency4Collected";
			 public static string ForeignCurrency5 = @"ForeignCurrency5";
			 public static string ForeignCurrency5Recorded = @"ForeignCurrency5Recorded";
			 public static string ForeignCurrency5Collected = @"ForeignCurrency5Collected";
			 public static string TotalForeignCurrency = @"TotalForeignCurrency";
			 public static string Pay7Collected = @"Pay7Collected";
			 public static string Pay7Recorded = @"Pay7Recorded";
			 public static string Pay8Collected = @"Pay8Collected";
			 public static string Pay8Recorded = @"Pay8Recorded";
			 public static string Pay9Collected = @"Pay9Collected";
			 public static string Pay9Recorded = @"Pay9Recorded";
			 public static string Pay10Collected = @"Pay10Collected";
			 public static string Pay10Recorded = @"Pay10Recorded";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
