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
	/// Strongly-typed collection for the Z2ClosingLog class.
	/// </summary>
    [Serializable]
	public partial class Z2ClosingLogCollection : ActiveList<Z2ClosingLog, Z2ClosingLogCollection>
	{	   
		public Z2ClosingLogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>Z2ClosingLogCollection</returns>
		public Z2ClosingLogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Z2ClosingLog o = this[i];
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
	/// This is an ActiveRecord class which wraps the Z2ClosingLog table.
	/// </summary>
	[Serializable]
	public partial class Z2ClosingLog : ActiveRecord<Z2ClosingLog>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Z2ClosingLog()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Z2ClosingLog(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Z2ClosingLog(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Z2ClosingLog(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Z2ClosingLog", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarZ2ClosingLogID = new TableSchema.TableColumn(schema);
				colvarZ2ClosingLogID.ColumnName = "Z2ClosingLogID";
				colvarZ2ClosingLogID.DataType = DbType.AnsiString;
				colvarZ2ClosingLogID.MaxLength = 14;
				colvarZ2ClosingLogID.AutoIncrement = false;
				colvarZ2ClosingLogID.IsNullable = false;
				colvarZ2ClosingLogID.IsPrimaryKey = true;
				colvarZ2ClosingLogID.IsForeignKey = false;
				colvarZ2ClosingLogID.IsReadOnly = false;
				colvarZ2ClosingLogID.DefaultSetting = @"";
				colvarZ2ClosingLogID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarZ2ClosingLogID);
				
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
				colvarPointOfSaleID.IsForeignKey = false;
				colvarPointOfSaleID.IsReadOnly = false;
				colvarPointOfSaleID.DefaultSetting = @"";
				colvarPointOfSaleID.ForeignKeyTableName = "";
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Z2ClosingLog",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Z2ClosingLogID")]
		[Bindable(true)]
		public string Z2ClosingLogID 
		{
			get { return GetColumnValue<string>(Columns.Z2ClosingLogID); }
			set { SetColumnValue(Columns.Z2ClosingLogID, value); }
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
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varZ2ClosingLogID,decimal varFloatBalance,DateTime varStartTime,DateTime varEndTime,string varCashier,string varSupervisor,decimal varCashIn,decimal varCashOut,decimal varOpeningBalance,decimal varTotalSystemRecorded,decimal varCashCollected,decimal? varCashRecorded,decimal varNetsCollected,decimal? varNetsRecorded,string varNetsTerminalID,decimal varVisaCollected,decimal? varVisaRecorded,string varVisaBatchNo,decimal varAmexCollected,decimal? varAmexRecorded,string varAmexBatchNo,decimal varChinaNetsCollected,decimal? varChinaNetsRecorded,string varChinaNetsTerminalID,decimal varVoucherCollected,decimal? varVoucherRecorded,string varDepositBagNo,decimal varTotalActualCollected,decimal varClosingCashOut,decimal varVariance,int varPointOfSaleID,DateTime varCreatedOn,string varCreatedBy,DateTime varModifiedOn,string varModifiedBy,Guid varUniqueID)
		{
			Z2ClosingLog item = new Z2ClosingLog();
			
			item.Z2ClosingLogID = varZ2ClosingLogID;
			
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
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varZ2ClosingLogID,decimal varFloatBalance,DateTime varStartTime,DateTime varEndTime,string varCashier,string varSupervisor,decimal varCashIn,decimal varCashOut,decimal varOpeningBalance,decimal varTotalSystemRecorded,decimal varCashCollected,decimal? varCashRecorded,decimal varNetsCollected,decimal? varNetsRecorded,string varNetsTerminalID,decimal varVisaCollected,decimal? varVisaRecorded,string varVisaBatchNo,decimal varAmexCollected,decimal? varAmexRecorded,string varAmexBatchNo,decimal varChinaNetsCollected,decimal? varChinaNetsRecorded,string varChinaNetsTerminalID,decimal varVoucherCollected,decimal? varVoucherRecorded,string varDepositBagNo,decimal varTotalActualCollected,decimal varClosingCashOut,decimal varVariance,int varPointOfSaleID,DateTime varCreatedOn,string varCreatedBy,DateTime varModifiedOn,string varModifiedBy,Guid varUniqueID)
		{
			Z2ClosingLog item = new Z2ClosingLog();
			
				item.Z2ClosingLogID = varZ2ClosingLogID;
			
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
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn Z2ClosingLogIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn FloatBalanceColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn StartTimeColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn EndTimeColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CashierColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn SupervisorColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CashInColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CashOutColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn OpeningBalanceColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn TotalSystemRecordedColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn CashCollectedColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn CashRecordedColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsCollectedColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsRecordedColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn NetsTerminalIDColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn VisaCollectedColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn VisaRecordedColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn VisaBatchNoColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn AmexCollectedColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn AmexRecordedColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn AmexBatchNoColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn ChinaNetsCollectedColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn ChinaNetsRecordedColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn ChinaNetsTerminalIDColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn VoucherCollectedColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn VoucherRecordedColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn DepositBagNoColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn TotalActualCollectedColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn ClosingCashOutColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn VarianceColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn PointOfSaleIDColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Z2ClosingLogID = @"Z2ClosingLogID";
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
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
