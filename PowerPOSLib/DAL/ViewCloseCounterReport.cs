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
namespace PowerPOS{
    /// <summary>
    /// Strongly-typed collection for the ViewCloseCounterReport class.
    /// </summary>
    [Serializable]
    public partial class ViewCloseCounterReportCollection : ReadOnlyList<ViewCloseCounterReport, ViewCloseCounterReportCollection>
    {        
        public ViewCloseCounterReportCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewCloseCounterReport view.
    /// </summary>
    [Serializable]
    public partial class ViewCloseCounterReport : ReadOnlyRecord<ViewCloseCounterReport>, IReadOnlyRecord
    {
    
	    #region Default Settings
	    protected static void SetSQLProps() 
	    {
		    GetTableSchema();
	    }
	    #endregion
        #region Schema Accessor
	    public static TableSchema.Table Schema
        {
            get
            {
                if (BaseSchema == null)
                {
                    SetSQLProps();
                }
                return BaseSchema;
            }
        }
    	
        private static void GetTableSchema() 
        {
            if(!IsSchemaInitialized)
            {
                //Schema declaration
                TableSchema.Table schema = new TableSchema.Table("ViewCloseCounterReport", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarCounterCloseID = new TableSchema.TableColumn(schema);
                colvarCounterCloseID.ColumnName = "CounterCloseID";
                colvarCounterCloseID.DataType = DbType.AnsiString;
                colvarCounterCloseID.MaxLength = 16;
                colvarCounterCloseID.AutoIncrement = false;
                colvarCounterCloseID.IsNullable = false;
                colvarCounterCloseID.IsPrimaryKey = false;
                colvarCounterCloseID.IsForeignKey = false;
                colvarCounterCloseID.IsReadOnly = false;
                
                schema.Columns.Add(colvarCounterCloseID);
                
                TableSchema.TableColumn colvarStartTime = new TableSchema.TableColumn(schema);
                colvarStartTime.ColumnName = "StartTime";
                colvarStartTime.DataType = DbType.DateTime;
                colvarStartTime.MaxLength = 0;
                colvarStartTime.AutoIncrement = false;
                colvarStartTime.IsNullable = false;
                colvarStartTime.IsPrimaryKey = false;
                colvarStartTime.IsForeignKey = false;
                colvarStartTime.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarEndTime);
                
                TableSchema.TableColumn colvarCashier = new TableSchema.TableColumn(schema);
                colvarCashier.ColumnName = "Cashier";
                colvarCashier.DataType = DbType.AnsiString;
                colvarCashier.MaxLength = 50;
                colvarCashier.AutoIncrement = false;
                colvarCashier.IsNullable = true;
                colvarCashier.IsPrimaryKey = false;
                colvarCashier.IsForeignKey = false;
                colvarCashier.IsReadOnly = false;
                
                schema.Columns.Add(colvarCashier);
                
                TableSchema.TableColumn colvarTotalSystemRecorded = new TableSchema.TableColumn(schema);
                colvarTotalSystemRecorded.ColumnName = "TotalSystemRecorded";
                colvarTotalSystemRecorded.DataType = DbType.Currency;
                colvarTotalSystemRecorded.MaxLength = 0;
                colvarTotalSystemRecorded.AutoIncrement = false;
                colvarTotalSystemRecorded.IsNullable = false;
                colvarTotalSystemRecorded.IsPrimaryKey = false;
                colvarTotalSystemRecorded.IsForeignKey = false;
                colvarTotalSystemRecorded.IsReadOnly = false;
                
                schema.Columns.Add(colvarTotalSystemRecorded);
                
                TableSchema.TableColumn colvarTotalActualCollected = new TableSchema.TableColumn(schema);
                colvarTotalActualCollected.ColumnName = "TotalActualCollected";
                colvarTotalActualCollected.DataType = DbType.Currency;
                colvarTotalActualCollected.MaxLength = 0;
                colvarTotalActualCollected.AutoIncrement = false;
                colvarTotalActualCollected.IsNullable = false;
                colvarTotalActualCollected.IsPrimaryKey = false;
                colvarTotalActualCollected.IsForeignKey = false;
                colvarTotalActualCollected.IsReadOnly = false;
                
                schema.Columns.Add(colvarTotalActualCollected);
                
                TableSchema.TableColumn colvarVariance = new TableSchema.TableColumn(schema);
                colvarVariance.ColumnName = "Variance";
                colvarVariance.DataType = DbType.Currency;
                colvarVariance.MaxLength = 0;
                colvarVariance.AutoIncrement = false;
                colvarVariance.IsNullable = false;
                colvarVariance.IsPrimaryKey = false;
                colvarVariance.IsForeignKey = false;
                colvarVariance.IsReadOnly = false;
                
                schema.Columns.Add(colvarVariance);
                
                TableSchema.TableColumn colvarCashRecorded = new TableSchema.TableColumn(schema);
                colvarCashRecorded.ColumnName = "CashRecorded";
                colvarCashRecorded.DataType = DbType.Currency;
                colvarCashRecorded.MaxLength = 0;
                colvarCashRecorded.AutoIncrement = false;
                colvarCashRecorded.IsNullable = true;
                colvarCashRecorded.IsPrimaryKey = false;
                colvarCashRecorded.IsForeignKey = false;
                colvarCashRecorded.IsReadOnly = false;
                
                schema.Columns.Add(colvarCashRecorded);
                
                TableSchema.TableColumn colvarCashCollected = new TableSchema.TableColumn(schema);
                colvarCashCollected.ColumnName = "CashCollected";
                colvarCashCollected.DataType = DbType.Currency;
                colvarCashCollected.MaxLength = 0;
                colvarCashCollected.AutoIncrement = false;
                colvarCashCollected.IsNullable = false;
                colvarCashCollected.IsPrimaryKey = false;
                colvarCashCollected.IsForeignKey = false;
                colvarCashCollected.IsReadOnly = false;
                
                schema.Columns.Add(colvarCashCollected);
                
                TableSchema.TableColumn colvarNetsRecorded = new TableSchema.TableColumn(schema);
                colvarNetsRecorded.ColumnName = "NetsRecorded";
                colvarNetsRecorded.DataType = DbType.Currency;
                colvarNetsRecorded.MaxLength = 0;
                colvarNetsRecorded.AutoIncrement = false;
                colvarNetsRecorded.IsNullable = true;
                colvarNetsRecorded.IsPrimaryKey = false;
                colvarNetsRecorded.IsForeignKey = false;
                colvarNetsRecorded.IsReadOnly = false;
                
                schema.Columns.Add(colvarNetsRecorded);
                
                TableSchema.TableColumn colvarNetsCollected = new TableSchema.TableColumn(schema);
                colvarNetsCollected.ColumnName = "NetsCollected";
                colvarNetsCollected.DataType = DbType.Currency;
                colvarNetsCollected.MaxLength = 0;
                colvarNetsCollected.AutoIncrement = false;
                colvarNetsCollected.IsNullable = false;
                colvarNetsCollected.IsPrimaryKey = false;
                colvarNetsCollected.IsForeignKey = false;
                colvarNetsCollected.IsReadOnly = false;
                
                schema.Columns.Add(colvarNetsCollected);
                
                TableSchema.TableColumn colvarNetsTerminalID = new TableSchema.TableColumn(schema);
                colvarNetsTerminalID.ColumnName = "NetsTerminalID";
                colvarNetsTerminalID.DataType = DbType.AnsiString;
                colvarNetsTerminalID.MaxLength = 50;
                colvarNetsTerminalID.AutoIncrement = false;
                colvarNetsTerminalID.IsNullable = true;
                colvarNetsTerminalID.IsPrimaryKey = false;
                colvarNetsTerminalID.IsForeignKey = false;
                colvarNetsTerminalID.IsReadOnly = false;
                
                schema.Columns.Add(colvarNetsTerminalID);
                
                TableSchema.TableColumn colvarVisaRecorded = new TableSchema.TableColumn(schema);
                colvarVisaRecorded.ColumnName = "VisaRecorded";
                colvarVisaRecorded.DataType = DbType.Currency;
                colvarVisaRecorded.MaxLength = 0;
                colvarVisaRecorded.AutoIncrement = false;
                colvarVisaRecorded.IsNullable = true;
                colvarVisaRecorded.IsPrimaryKey = false;
                colvarVisaRecorded.IsForeignKey = false;
                colvarVisaRecorded.IsReadOnly = false;
                
                schema.Columns.Add(colvarVisaRecorded);
                
                TableSchema.TableColumn colvarVisaCollected = new TableSchema.TableColumn(schema);
                colvarVisaCollected.ColumnName = "VisaCollected";
                colvarVisaCollected.DataType = DbType.Currency;
                colvarVisaCollected.MaxLength = 0;
                colvarVisaCollected.AutoIncrement = false;
                colvarVisaCollected.IsNullable = false;
                colvarVisaCollected.IsPrimaryKey = false;
                colvarVisaCollected.IsForeignKey = false;
                colvarVisaCollected.IsReadOnly = false;
                
                schema.Columns.Add(colvarVisaCollected);
                
                TableSchema.TableColumn colvarVisaBatchNo = new TableSchema.TableColumn(schema);
                colvarVisaBatchNo.ColumnName = "VisaBatchNo";
                colvarVisaBatchNo.DataType = DbType.AnsiString;
                colvarVisaBatchNo.MaxLength = 50;
                colvarVisaBatchNo.AutoIncrement = false;
                colvarVisaBatchNo.IsNullable = true;
                colvarVisaBatchNo.IsPrimaryKey = false;
                colvarVisaBatchNo.IsForeignKey = false;
                colvarVisaBatchNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarVisaBatchNo);
                
                TableSchema.TableColumn colvarAmexRecorded = new TableSchema.TableColumn(schema);
                colvarAmexRecorded.ColumnName = "AmexRecorded";
                colvarAmexRecorded.DataType = DbType.Currency;
                colvarAmexRecorded.MaxLength = 0;
                colvarAmexRecorded.AutoIncrement = false;
                colvarAmexRecorded.IsNullable = true;
                colvarAmexRecorded.IsPrimaryKey = false;
                colvarAmexRecorded.IsForeignKey = false;
                colvarAmexRecorded.IsReadOnly = false;
                
                schema.Columns.Add(colvarAmexRecorded);
                
                TableSchema.TableColumn colvarAmexCollected = new TableSchema.TableColumn(schema);
                colvarAmexCollected.ColumnName = "AmexCollected";
                colvarAmexCollected.DataType = DbType.Currency;
                colvarAmexCollected.MaxLength = 0;
                colvarAmexCollected.AutoIncrement = false;
                colvarAmexCollected.IsNullable = false;
                colvarAmexCollected.IsPrimaryKey = false;
                colvarAmexCollected.IsForeignKey = false;
                colvarAmexCollected.IsReadOnly = false;
                
                schema.Columns.Add(colvarAmexCollected);
                
                TableSchema.TableColumn colvarAmexBatchNo = new TableSchema.TableColumn(schema);
                colvarAmexBatchNo.ColumnName = "AmexBatchNo";
                colvarAmexBatchNo.DataType = DbType.AnsiString;
                colvarAmexBatchNo.MaxLength = 50;
                colvarAmexBatchNo.AutoIncrement = false;
                colvarAmexBatchNo.IsNullable = true;
                colvarAmexBatchNo.IsPrimaryKey = false;
                colvarAmexBatchNo.IsForeignKey = false;
                colvarAmexBatchNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarAmexBatchNo);
                
                TableSchema.TableColumn colvarChinaNetsRecorded = new TableSchema.TableColumn(schema);
                colvarChinaNetsRecorded.ColumnName = "ChinaNetsRecorded";
                colvarChinaNetsRecorded.DataType = DbType.Currency;
                colvarChinaNetsRecorded.MaxLength = 0;
                colvarChinaNetsRecorded.AutoIncrement = false;
                colvarChinaNetsRecorded.IsNullable = true;
                colvarChinaNetsRecorded.IsPrimaryKey = false;
                colvarChinaNetsRecorded.IsForeignKey = false;
                colvarChinaNetsRecorded.IsReadOnly = false;
                
                schema.Columns.Add(colvarChinaNetsRecorded);
                
                TableSchema.TableColumn colvarChinaNetsCollected = new TableSchema.TableColumn(schema);
                colvarChinaNetsCollected.ColumnName = "ChinaNetsCollected";
                colvarChinaNetsCollected.DataType = DbType.Currency;
                colvarChinaNetsCollected.MaxLength = 0;
                colvarChinaNetsCollected.AutoIncrement = false;
                colvarChinaNetsCollected.IsNullable = false;
                colvarChinaNetsCollected.IsPrimaryKey = false;
                colvarChinaNetsCollected.IsForeignKey = false;
                colvarChinaNetsCollected.IsReadOnly = false;
                
                schema.Columns.Add(colvarChinaNetsCollected);
                
                TableSchema.TableColumn colvarChinaNetsTerminalID = new TableSchema.TableColumn(schema);
                colvarChinaNetsTerminalID.ColumnName = "ChinaNetsTerminalID";
                colvarChinaNetsTerminalID.DataType = DbType.AnsiString;
                colvarChinaNetsTerminalID.MaxLength = 50;
                colvarChinaNetsTerminalID.AutoIncrement = false;
                colvarChinaNetsTerminalID.IsNullable = true;
                colvarChinaNetsTerminalID.IsPrimaryKey = false;
                colvarChinaNetsTerminalID.IsForeignKey = false;
                colvarChinaNetsTerminalID.IsReadOnly = false;
                
                schema.Columns.Add(colvarChinaNetsTerminalID);
                
                TableSchema.TableColumn colvarVoucherRecorded = new TableSchema.TableColumn(schema);
                colvarVoucherRecorded.ColumnName = "VoucherRecorded";
                colvarVoucherRecorded.DataType = DbType.Currency;
                colvarVoucherRecorded.MaxLength = 0;
                colvarVoucherRecorded.AutoIncrement = false;
                colvarVoucherRecorded.IsNullable = true;
                colvarVoucherRecorded.IsPrimaryKey = false;
                colvarVoucherRecorded.IsForeignKey = false;
                colvarVoucherRecorded.IsReadOnly = false;
                
                schema.Columns.Add(colvarVoucherRecorded);
                
                TableSchema.TableColumn colvarVoucherCollected = new TableSchema.TableColumn(schema);
                colvarVoucherCollected.ColumnName = "VoucherCollected";
                colvarVoucherCollected.DataType = DbType.Currency;
                colvarVoucherCollected.MaxLength = 0;
                colvarVoucherCollected.AutoIncrement = false;
                colvarVoucherCollected.IsNullable = false;
                colvarVoucherCollected.IsPrimaryKey = false;
                colvarVoucherCollected.IsForeignKey = false;
                colvarVoucherCollected.IsReadOnly = false;
                
                schema.Columns.Add(colvarVoucherCollected);
                
                TableSchema.TableColumn colvarChequeRecorded = new TableSchema.TableColumn(schema);
                colvarChequeRecorded.ColumnName = "ChequeRecorded";
                colvarChequeRecorded.DataType = DbType.Currency;
                colvarChequeRecorded.MaxLength = 0;
                colvarChequeRecorded.AutoIncrement = false;
                colvarChequeRecorded.IsNullable = true;
                colvarChequeRecorded.IsPrimaryKey = false;
                colvarChequeRecorded.IsForeignKey = false;
                colvarChequeRecorded.IsReadOnly = false;
                
                schema.Columns.Add(colvarChequeRecorded);
                
                TableSchema.TableColumn colvarChequeCollected = new TableSchema.TableColumn(schema);
                colvarChequeCollected.ColumnName = "ChequeCollected";
                colvarChequeCollected.DataType = DbType.Currency;
                colvarChequeCollected.MaxLength = 0;
                colvarChequeCollected.AutoIncrement = false;
                colvarChequeCollected.IsNullable = true;
                colvarChequeCollected.IsPrimaryKey = false;
                colvarChequeCollected.IsForeignKey = false;
                colvarChequeCollected.IsReadOnly = false;
                
                schema.Columns.Add(colvarChequeCollected);
                
                TableSchema.TableColumn colvarClosingCashOut = new TableSchema.TableColumn(schema);
                colvarClosingCashOut.ColumnName = "ClosingCashOut";
                colvarClosingCashOut.DataType = DbType.Currency;
                colvarClosingCashOut.MaxLength = 0;
                colvarClosingCashOut.AutoIncrement = false;
                colvarClosingCashOut.IsNullable = false;
                colvarClosingCashOut.IsPrimaryKey = false;
                colvarClosingCashOut.IsForeignKey = false;
                colvarClosingCashOut.IsReadOnly = false;
                
                schema.Columns.Add(colvarClosingCashOut);
                
                TableSchema.TableColumn colvarDepositBagNo = new TableSchema.TableColumn(schema);
                colvarDepositBagNo.ColumnName = "DepositBagNo";
                colvarDepositBagNo.DataType = DbType.AnsiString;
                colvarDepositBagNo.MaxLength = 50;
                colvarDepositBagNo.AutoIncrement = false;
                colvarDepositBagNo.IsNullable = true;
                colvarDepositBagNo.IsPrimaryKey = false;
                colvarDepositBagNo.IsForeignKey = false;
                colvarDepositBagNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarDepositBagNo);
                
                TableSchema.TableColumn colvarSupervisor = new TableSchema.TableColumn(schema);
                colvarSupervisor.ColumnName = "Supervisor";
                colvarSupervisor.DataType = DbType.AnsiString;
                colvarSupervisor.MaxLength = 50;
                colvarSupervisor.AutoIncrement = false;
                colvarSupervisor.IsNullable = true;
                colvarSupervisor.IsPrimaryKey = false;
                colvarSupervisor.IsForeignKey = false;
                colvarSupervisor.IsReadOnly = false;
                
                schema.Columns.Add(colvarSupervisor);
                
                TableSchema.TableColumn colvarOpeningBalance = new TableSchema.TableColumn(schema);
                colvarOpeningBalance.ColumnName = "OpeningBalance";
                colvarOpeningBalance.DataType = DbType.Currency;
                colvarOpeningBalance.MaxLength = 0;
                colvarOpeningBalance.AutoIncrement = false;
                colvarOpeningBalance.IsNullable = false;
                colvarOpeningBalance.IsPrimaryKey = false;
                colvarOpeningBalance.IsForeignKey = false;
                colvarOpeningBalance.IsReadOnly = false;
                
                schema.Columns.Add(colvarOpeningBalance);
                
                TableSchema.TableColumn colvarCashIn = new TableSchema.TableColumn(schema);
                colvarCashIn.ColumnName = "CashIn";
                colvarCashIn.DataType = DbType.Currency;
                colvarCashIn.MaxLength = 0;
                colvarCashIn.AutoIncrement = false;
                colvarCashIn.IsNullable = false;
                colvarCashIn.IsPrimaryKey = false;
                colvarCashIn.IsForeignKey = false;
                colvarCashIn.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarCashOut);
                
                TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
                colvarPointOfSaleID.ColumnName = "PointOfSaleID";
                colvarPointOfSaleID.DataType = DbType.Int32;
                colvarPointOfSaleID.MaxLength = 0;
                colvarPointOfSaleID.AutoIncrement = false;
                colvarPointOfSaleID.IsNullable = false;
                colvarPointOfSaleID.IsPrimaryKey = false;
                colvarPointOfSaleID.IsForeignKey = false;
                colvarPointOfSaleID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPointOfSaleID);
                
                TableSchema.TableColumn colvarPointOfSaleName = new TableSchema.TableColumn(schema);
                colvarPointOfSaleName.ColumnName = "PointOfSaleName";
                colvarPointOfSaleName.DataType = DbType.AnsiString;
                colvarPointOfSaleName.MaxLength = 50;
                colvarPointOfSaleName.AutoIncrement = false;
                colvarPointOfSaleName.IsNullable = false;
                colvarPointOfSaleName.IsPrimaryKey = false;
                colvarPointOfSaleName.IsForeignKey = false;
                colvarPointOfSaleName.IsReadOnly = false;
                
                schema.Columns.Add(colvarPointOfSaleName);
                
                TableSchema.TableColumn colvarFloatBalance = new TableSchema.TableColumn(schema);
                colvarFloatBalance.ColumnName = "FloatBalance";
                colvarFloatBalance.DataType = DbType.Currency;
                colvarFloatBalance.MaxLength = 0;
                colvarFloatBalance.AutoIncrement = false;
                colvarFloatBalance.IsNullable = false;
                colvarFloatBalance.IsPrimaryKey = false;
                colvarFloatBalance.IsForeignKey = false;
                colvarFloatBalance.IsReadOnly = false;
                
                schema.Columns.Add(colvarFloatBalance);
                
                TableSchema.TableColumn colvarDepartmentID = new TableSchema.TableColumn(schema);
                colvarDepartmentID.ColumnName = "DepartmentID";
                colvarDepartmentID.DataType = DbType.Int32;
                colvarDepartmentID.MaxLength = 0;
                colvarDepartmentID.AutoIncrement = false;
                colvarDepartmentID.IsNullable = true;
                colvarDepartmentID.IsPrimaryKey = false;
                colvarDepartmentID.IsForeignKey = false;
                colvarDepartmentID.IsReadOnly = false;
                
                schema.Columns.Add(colvarDepartmentID);
                
                TableSchema.TableColumn colvarOutletName = new TableSchema.TableColumn(schema);
                colvarOutletName.ColumnName = "OutletName";
                colvarOutletName.DataType = DbType.AnsiString;
                colvarOutletName.MaxLength = 50;
                colvarOutletName.AutoIncrement = false;
                colvarOutletName.IsNullable = false;
                colvarOutletName.IsPrimaryKey = false;
                colvarOutletName.IsForeignKey = false;
                colvarOutletName.IsReadOnly = false;
                
                schema.Columns.Add(colvarOutletName);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewCloseCounterReport",schema);
            }
        }
        #endregion
        
        #region Query Accessor
	    public static Query CreateQuery()
	    {
		    return new Query(Schema);
	    }
	    #endregion
	    
	    #region .ctors
	    public ViewCloseCounterReport()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewCloseCounterReport(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewCloseCounterReport(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewCloseCounterReport(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("CounterCloseID")]
        [Bindable(true)]
        public string CounterCloseID 
	    {
		    get
		    {
			    return GetColumnValue<string>("CounterCloseID");
		    }
            set 
		    {
			    SetColumnValue("CounterCloseID", value);
            }
        }
	      
        [XmlAttribute("StartTime")]
        [Bindable(true)]
        public DateTime StartTime 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("StartTime");
		    }
            set 
		    {
			    SetColumnValue("StartTime", value);
            }
        }
	      
        [XmlAttribute("EndTime")]
        [Bindable(true)]
        public DateTime EndTime 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("EndTime");
		    }
            set 
		    {
			    SetColumnValue("EndTime", value);
            }
        }
	      
        [XmlAttribute("Cashier")]
        [Bindable(true)]
        public string Cashier 
	    {
		    get
		    {
			    return GetColumnValue<string>("Cashier");
		    }
            set 
		    {
			    SetColumnValue("Cashier", value);
            }
        }
	      
        [XmlAttribute("TotalSystemRecorded")]
        [Bindable(true)]
        public decimal TotalSystemRecorded 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("TotalSystemRecorded");
		    }
            set 
		    {
			    SetColumnValue("TotalSystemRecorded", value);
            }
        }
	      
        [XmlAttribute("TotalActualCollected")]
        [Bindable(true)]
        public decimal TotalActualCollected 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("TotalActualCollected");
		    }
            set 
		    {
			    SetColumnValue("TotalActualCollected", value);
            }
        }
	      
        [XmlAttribute("Variance")]
        [Bindable(true)]
        public decimal Variance 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("Variance");
		    }
            set 
		    {
			    SetColumnValue("Variance", value);
            }
        }
	      
        [XmlAttribute("CashRecorded")]
        [Bindable(true)]
        public decimal? CashRecorded 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("CashRecorded");
		    }
            set 
		    {
			    SetColumnValue("CashRecorded", value);
            }
        }
	      
        [XmlAttribute("CashCollected")]
        [Bindable(true)]
        public decimal CashCollected 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("CashCollected");
		    }
            set 
		    {
			    SetColumnValue("CashCollected", value);
            }
        }
	      
        [XmlAttribute("NetsRecorded")]
        [Bindable(true)]
        public decimal? NetsRecorded 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("NetsRecorded");
		    }
            set 
		    {
			    SetColumnValue("NetsRecorded", value);
            }
        }
	      
        [XmlAttribute("NetsCollected")]
        [Bindable(true)]
        public decimal NetsCollected 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("NetsCollected");
		    }
            set 
		    {
			    SetColumnValue("NetsCollected", value);
            }
        }
	      
        [XmlAttribute("NetsTerminalID")]
        [Bindable(true)]
        public string NetsTerminalID 
	    {
		    get
		    {
			    return GetColumnValue<string>("NetsTerminalID");
		    }
            set 
		    {
			    SetColumnValue("NetsTerminalID", value);
            }
        }
	      
        [XmlAttribute("VisaRecorded")]
        [Bindable(true)]
        public decimal? VisaRecorded 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("VisaRecorded");
		    }
            set 
		    {
			    SetColumnValue("VisaRecorded", value);
            }
        }
	      
        [XmlAttribute("VisaCollected")]
        [Bindable(true)]
        public decimal VisaCollected 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("VisaCollected");
		    }
            set 
		    {
			    SetColumnValue("VisaCollected", value);
            }
        }
	      
        [XmlAttribute("VisaBatchNo")]
        [Bindable(true)]
        public string VisaBatchNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("VisaBatchNo");
		    }
            set 
		    {
			    SetColumnValue("VisaBatchNo", value);
            }
        }
	      
        [XmlAttribute("AmexRecorded")]
        [Bindable(true)]
        public decimal? AmexRecorded 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("AmexRecorded");
		    }
            set 
		    {
			    SetColumnValue("AmexRecorded", value);
            }
        }
	      
        [XmlAttribute("AmexCollected")]
        [Bindable(true)]
        public decimal AmexCollected 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("AmexCollected");
		    }
            set 
		    {
			    SetColumnValue("AmexCollected", value);
            }
        }
	      
        [XmlAttribute("AmexBatchNo")]
        [Bindable(true)]
        public string AmexBatchNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("AmexBatchNo");
		    }
            set 
		    {
			    SetColumnValue("AmexBatchNo", value);
            }
        }
	      
        [XmlAttribute("ChinaNetsRecorded")]
        [Bindable(true)]
        public decimal? ChinaNetsRecorded 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("ChinaNetsRecorded");
		    }
            set 
		    {
			    SetColumnValue("ChinaNetsRecorded", value);
            }
        }
	      
        [XmlAttribute("ChinaNetsCollected")]
        [Bindable(true)]
        public decimal ChinaNetsCollected 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("ChinaNetsCollected");
		    }
            set 
		    {
			    SetColumnValue("ChinaNetsCollected", value);
            }
        }
	      
        [XmlAttribute("ChinaNetsTerminalID")]
        [Bindable(true)]
        public string ChinaNetsTerminalID 
	    {
		    get
		    {
			    return GetColumnValue<string>("ChinaNetsTerminalID");
		    }
            set 
		    {
			    SetColumnValue("ChinaNetsTerminalID", value);
            }
        }
	      
        [XmlAttribute("VoucherRecorded")]
        [Bindable(true)]
        public decimal? VoucherRecorded 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("VoucherRecorded");
		    }
            set 
		    {
			    SetColumnValue("VoucherRecorded", value);
            }
        }
	      
        [XmlAttribute("VoucherCollected")]
        [Bindable(true)]
        public decimal VoucherCollected 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("VoucherCollected");
		    }
            set 
		    {
			    SetColumnValue("VoucherCollected", value);
            }
        }
	      
        [XmlAttribute("ChequeRecorded")]
        [Bindable(true)]
        public decimal? ChequeRecorded 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("ChequeRecorded");
		    }
            set 
		    {
			    SetColumnValue("ChequeRecorded", value);
            }
        }
	      
        [XmlAttribute("ChequeCollected")]
        [Bindable(true)]
        public decimal? ChequeCollected 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("ChequeCollected");
		    }
            set 
		    {
			    SetColumnValue("ChequeCollected", value);
            }
        }
	      
        [XmlAttribute("ClosingCashOut")]
        [Bindable(true)]
        public decimal ClosingCashOut 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("ClosingCashOut");
		    }
            set 
		    {
			    SetColumnValue("ClosingCashOut", value);
            }
        }
	      
        [XmlAttribute("DepositBagNo")]
        [Bindable(true)]
        public string DepositBagNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("DepositBagNo");
		    }
            set 
		    {
			    SetColumnValue("DepositBagNo", value);
            }
        }
	      
        [XmlAttribute("Supervisor")]
        [Bindable(true)]
        public string Supervisor 
	    {
		    get
		    {
			    return GetColumnValue<string>("Supervisor");
		    }
            set 
		    {
			    SetColumnValue("Supervisor", value);
            }
        }
	      
        [XmlAttribute("OpeningBalance")]
        [Bindable(true)]
        public decimal OpeningBalance 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("OpeningBalance");
		    }
            set 
		    {
			    SetColumnValue("OpeningBalance", value);
            }
        }
	      
        [XmlAttribute("CashIn")]
        [Bindable(true)]
        public decimal CashIn 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("CashIn");
		    }
            set 
		    {
			    SetColumnValue("CashIn", value);
            }
        }
	      
        [XmlAttribute("CashOut")]
        [Bindable(true)]
        public decimal CashOut 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("CashOut");
		    }
            set 
		    {
			    SetColumnValue("CashOut", value);
            }
        }
	      
        [XmlAttribute("PointOfSaleID")]
        [Bindable(true)]
        public int PointOfSaleID 
	    {
		    get
		    {
			    return GetColumnValue<int>("PointOfSaleID");
		    }
            set 
		    {
			    SetColumnValue("PointOfSaleID", value);
            }
        }
	      
        [XmlAttribute("PointOfSaleName")]
        [Bindable(true)]
        public string PointOfSaleName 
	    {
		    get
		    {
			    return GetColumnValue<string>("PointOfSaleName");
		    }
            set 
		    {
			    SetColumnValue("PointOfSaleName", value);
            }
        }
	      
        [XmlAttribute("FloatBalance")]
        [Bindable(true)]
        public decimal FloatBalance 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("FloatBalance");
		    }
            set 
		    {
			    SetColumnValue("FloatBalance", value);
            }
        }
	      
        [XmlAttribute("DepartmentID")]
        [Bindable(true)]
        public int? DepartmentID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("DepartmentID");
		    }
            set 
		    {
			    SetColumnValue("DepartmentID", value);
            }
        }
	      
        [XmlAttribute("OutletName")]
        [Bindable(true)]
        public string OutletName 
	    {
		    get
		    {
			    return GetColumnValue<string>("OutletName");
		    }
            set 
		    {
			    SetColumnValue("OutletName", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string CounterCloseID = @"CounterCloseID";
            
            public static string StartTime = @"StartTime";
            
            public static string EndTime = @"EndTime";
            
            public static string Cashier = @"Cashier";
            
            public static string TotalSystemRecorded = @"TotalSystemRecorded";
            
            public static string TotalActualCollected = @"TotalActualCollected";
            
            public static string Variance = @"Variance";
            
            public static string CashRecorded = @"CashRecorded";
            
            public static string CashCollected = @"CashCollected";
            
            public static string NetsRecorded = @"NetsRecorded";
            
            public static string NetsCollected = @"NetsCollected";
            
            public static string NetsTerminalID = @"NetsTerminalID";
            
            public static string VisaRecorded = @"VisaRecorded";
            
            public static string VisaCollected = @"VisaCollected";
            
            public static string VisaBatchNo = @"VisaBatchNo";
            
            public static string AmexRecorded = @"AmexRecorded";
            
            public static string AmexCollected = @"AmexCollected";
            
            public static string AmexBatchNo = @"AmexBatchNo";
            
            public static string ChinaNetsRecorded = @"ChinaNetsRecorded";
            
            public static string ChinaNetsCollected = @"ChinaNetsCollected";
            
            public static string ChinaNetsTerminalID = @"ChinaNetsTerminalID";
            
            public static string VoucherRecorded = @"VoucherRecorded";
            
            public static string VoucherCollected = @"VoucherCollected";
            
            public static string ChequeRecorded = @"ChequeRecorded";
            
            public static string ChequeCollected = @"ChequeCollected";
            
            public static string ClosingCashOut = @"ClosingCashOut";
            
            public static string DepositBagNo = @"DepositBagNo";
            
            public static string Supervisor = @"Supervisor";
            
            public static string OpeningBalance = @"OpeningBalance";
            
            public static string CashIn = @"CashIn";
            
            public static string CashOut = @"CashOut";
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string PointOfSaleName = @"PointOfSaleName";
            
            public static string FloatBalance = @"FloatBalance";
            
            public static string DepartmentID = @"DepartmentID";
            
            public static string OutletName = @"OutletName";
            
	    }
	    #endregion
	    
	    
	    #region IAbstractRecord Members
        public new CT GetColumnValue<CT>(string columnName) {
            return base.GetColumnValue<CT>(columnName);
        }
        public object GetColumnValue(string columnName) {
            return base.GetColumnValue<object>(columnName);
        }
        #endregion
	    
    }
}
