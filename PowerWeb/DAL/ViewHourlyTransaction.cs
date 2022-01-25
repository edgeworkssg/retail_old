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
    /// Strongly-typed collection for the ViewHourlyTransaction class.
    /// </summary>
    [Serializable]
    public partial class ViewHourlyTransactionCollection : ReadOnlyList<ViewHourlyTransaction, ViewHourlyTransactionCollection>
    {        
        public ViewHourlyTransactionCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewHourlyTransaction view.
    /// </summary>
    [Serializable]
    public partial class ViewHourlyTransaction : ReadOnlyRecord<ViewHourlyTransaction>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewHourlyTransaction", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarHour = new TableSchema.TableColumn(schema);
                colvarHour.ColumnName = "Hour";
                colvarHour.DataType = DbType.Int32;
                colvarHour.MaxLength = 0;
                colvarHour.AutoIncrement = false;
                colvarHour.IsNullable = true;
                colvarHour.IsPrimaryKey = false;
                colvarHour.IsForeignKey = false;
                colvarHour.IsReadOnly = false;
                
                schema.Columns.Add(colvarHour);
                
                TableSchema.TableColumn colvarOrderDay = new TableSchema.TableColumn(schema);
                colvarOrderDay.ColumnName = "OrderDay";
                colvarOrderDay.DataType = DbType.Int32;
                colvarOrderDay.MaxLength = 0;
                colvarOrderDay.AutoIncrement = false;
                colvarOrderDay.IsNullable = true;
                colvarOrderDay.IsPrimaryKey = false;
                colvarOrderDay.IsForeignKey = false;
                colvarOrderDay.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDay);
                
                TableSchema.TableColumn colvarAmount = new TableSchema.TableColumn(schema);
                colvarAmount.ColumnName = "Amount";
                colvarAmount.DataType = DbType.Currency;
                colvarAmount.MaxLength = 0;
                colvarAmount.AutoIncrement = false;
                colvarAmount.IsNullable = true;
                colvarAmount.IsPrimaryKey = false;
                colvarAmount.IsForeignKey = false;
                colvarAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarAmount);
                
                TableSchema.TableColumn colvarWeekDayName = new TableSchema.TableColumn(schema);
                colvarWeekDayName.ColumnName = "WeekDayName";
                colvarWeekDayName.DataType = DbType.String;
                colvarWeekDayName.MaxLength = 30;
                colvarWeekDayName.AutoIncrement = false;
                colvarWeekDayName.IsNullable = true;
                colvarWeekDayName.IsPrimaryKey = false;
                colvarWeekDayName.IsForeignKey = false;
                colvarWeekDayName.IsReadOnly = false;
                
                schema.Columns.Add(colvarWeekDayName);
                
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
                
                TableSchema.TableColumn colvarOrderDate = new TableSchema.TableColumn(schema);
                colvarOrderDate.ColumnName = "OrderDate";
                colvarOrderDate.DataType = DbType.DateTime;
                colvarOrderDate.MaxLength = 0;
                colvarOrderDate.AutoIncrement = false;
                colvarOrderDate.IsNullable = true;
                colvarOrderDate.IsPrimaryKey = false;
                colvarOrderDate.IsForeignKey = false;
                colvarOrderDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDate);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewHourlyTransaction",schema);
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
	    public ViewHourlyTransaction()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewHourlyTransaction(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewHourlyTransaction(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewHourlyTransaction(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("Hour")]
        [Bindable(true)]
        public int? Hour 
	    {
		    get
		    {
			    return GetColumnValue<int?>("Hour");
		    }
            set 
		    {
			    SetColumnValue("Hour", value);
            }
        }
	      
        [XmlAttribute("OrderDay")]
        [Bindable(true)]
        public int? OrderDay 
	    {
		    get
		    {
			    return GetColumnValue<int?>("OrderDay");
		    }
            set 
		    {
			    SetColumnValue("OrderDay", value);
            }
        }
	      
        [XmlAttribute("Amount")]
        [Bindable(true)]
        public decimal? Amount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Amount");
		    }
            set 
		    {
			    SetColumnValue("Amount", value);
            }
        }
	      
        [XmlAttribute("WeekDayName")]
        [Bindable(true)]
        public string WeekDayName 
	    {
		    get
		    {
			    return GetColumnValue<string>("WeekDayName");
		    }
            set 
		    {
			    SetColumnValue("WeekDayName", value);
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
	      
        [XmlAttribute("OrderDate")]
        [Bindable(true)]
        public DateTime? OrderDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("OrderDate");
		    }
            set 
		    {
			    SetColumnValue("OrderDate", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string Hour = @"Hour";
            
            public static string OrderDay = @"OrderDay";
            
            public static string Amount = @"Amount";
            
            public static string WeekDayName = @"WeekDayName";
            
            public static string OutletName = @"OutletName";
            
            public static string OrderDate = @"OrderDate";
            
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
