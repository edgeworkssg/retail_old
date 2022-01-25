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
    /// Strongly-typed collection for the ViewSalesPersonLastMonthCommission class.
    /// </summary>
    [Serializable]
    public partial class ViewSalesPersonLastMonthCommissionCollection : ReadOnlyList<ViewSalesPersonLastMonthCommission, ViewSalesPersonLastMonthCommissionCollection>
    {        
        public ViewSalesPersonLastMonthCommissionCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewSalesPersonLastMonthCommission view.
    /// </summary>
    [Serializable]
    public partial class ViewSalesPersonLastMonthCommission : ReadOnlyRecord<ViewSalesPersonLastMonthCommission>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewSalesPersonLastMonthCommission", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarSalesAmount = new TableSchema.TableColumn(schema);
                colvarSalesAmount.ColumnName = "SalesAmount";
                colvarSalesAmount.DataType = DbType.Currency;
                colvarSalesAmount.MaxLength = 0;
                colvarSalesAmount.AutoIncrement = false;
                colvarSalesAmount.IsNullable = true;
                colvarSalesAmount.IsPrimaryKey = false;
                colvarSalesAmount.IsForeignKey = false;
                colvarSalesAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarSalesAmount);
                
                TableSchema.TableColumn colvarSalesPersonName = new TableSchema.TableColumn(schema);
                colvarSalesPersonName.ColumnName = "SalesPersonName";
                colvarSalesPersonName.DataType = DbType.AnsiString;
                colvarSalesPersonName.MaxLength = 50;
                colvarSalesPersonName.AutoIncrement = false;
                colvarSalesPersonName.IsNullable = false;
                colvarSalesPersonName.IsPrimaryKey = false;
                colvarSalesPersonName.IsForeignKey = false;
                colvarSalesPersonName.IsReadOnly = false;
                
                schema.Columns.Add(colvarSalesPersonName);
                
                TableSchema.TableColumn colvarGroupName = new TableSchema.TableColumn(schema);
                colvarGroupName.ColumnName = "GroupName";
                colvarGroupName.DataType = DbType.AnsiString;
                colvarGroupName.MaxLength = 50;
                colvarGroupName.AutoIncrement = false;
                colvarGroupName.IsNullable = false;
                colvarGroupName.IsPrimaryKey = false;
                colvarGroupName.IsForeignKey = false;
                colvarGroupName.IsReadOnly = false;
                
                schema.Columns.Add(colvarGroupName);
                
                TableSchema.TableColumn colvarExpectedWorkingHours = new TableSchema.TableColumn(schema);
                colvarExpectedWorkingHours.ColumnName = "ExpectedWorkingHours";
                colvarExpectedWorkingHours.DataType = DbType.Int32;
                colvarExpectedWorkingHours.MaxLength = 0;
                colvarExpectedWorkingHours.AutoIncrement = false;
                colvarExpectedWorkingHours.IsNullable = false;
                colvarExpectedWorkingHours.IsPrimaryKey = false;
                colvarExpectedWorkingHours.IsForeignKey = false;
                colvarExpectedWorkingHours.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpectedWorkingHours);
                
                TableSchema.TableColumn colvarRate = new TableSchema.TableColumn(schema);
                colvarRate.ColumnName = "rate";
                colvarRate.DataType = DbType.Double;
                colvarRate.MaxLength = 0;
                colvarRate.AutoIncrement = false;
                colvarRate.IsNullable = false;
                colvarRate.IsPrimaryKey = false;
                colvarRate.IsForeignKey = false;
                colvarRate.IsReadOnly = false;
                
                schema.Columns.Add(colvarRate);
                
                TableSchema.TableColumn colvarCommissionAmount = new TableSchema.TableColumn(schema);
                colvarCommissionAmount.ColumnName = "CommissionAmount";
                colvarCommissionAmount.DataType = DbType.Double;
                colvarCommissionAmount.MaxLength = 0;
                colvarCommissionAmount.AutoIncrement = false;
                colvarCommissionAmount.IsNullable = true;
                colvarCommissionAmount.IsPrimaryKey = false;
                colvarCommissionAmount.IsForeignKey = false;
                colvarCommissionAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarCommissionAmount);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewSalesPersonLastMonthCommission",schema);
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
	    public ViewSalesPersonLastMonthCommission()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewSalesPersonLastMonthCommission(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewSalesPersonLastMonthCommission(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewSalesPersonLastMonthCommission(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("SalesAmount")]
        [Bindable(true)]
        public decimal? SalesAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("SalesAmount");
		    }
            set 
		    {
			    SetColumnValue("SalesAmount", value);
            }
        }
	      
        [XmlAttribute("SalesPersonName")]
        [Bindable(true)]
        public string SalesPersonName 
	    {
		    get
		    {
			    return GetColumnValue<string>("SalesPersonName");
		    }
            set 
		    {
			    SetColumnValue("SalesPersonName", value);
            }
        }
	      
        [XmlAttribute("GroupName")]
        [Bindable(true)]
        public string GroupName 
	    {
		    get
		    {
			    return GetColumnValue<string>("GroupName");
		    }
            set 
		    {
			    SetColumnValue("GroupName", value);
            }
        }
	      
        [XmlAttribute("ExpectedWorkingHours")]
        [Bindable(true)]
        public int ExpectedWorkingHours 
	    {
		    get
		    {
			    return GetColumnValue<int>("ExpectedWorkingHours");
		    }
            set 
		    {
			    SetColumnValue("ExpectedWorkingHours", value);
            }
        }
	      
        [XmlAttribute("Rate")]
        [Bindable(true)]
        public double Rate 
	    {
		    get
		    {
			    return GetColumnValue<double>("rate");
		    }
            set 
		    {
			    SetColumnValue("rate", value);
            }
        }
	      
        [XmlAttribute("CommissionAmount")]
        [Bindable(true)]
        public double? CommissionAmount 
	    {
		    get
		    {
			    return GetColumnValue<double?>("CommissionAmount");
		    }
            set 
		    {
			    SetColumnValue("CommissionAmount", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string SalesAmount = @"SalesAmount";
            
            public static string SalesPersonName = @"SalesPersonName";
            
            public static string GroupName = @"GroupName";
            
            public static string ExpectedWorkingHours = @"ExpectedWorkingHours";
            
            public static string Rate = @"rate";
            
            public static string CommissionAmount = @"CommissionAmount";
            
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
