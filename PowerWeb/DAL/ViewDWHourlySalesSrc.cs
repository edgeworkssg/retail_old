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
    /// Strongly-typed collection for the ViewDWHourlySalesSrc class.
    /// </summary>
    [Serializable]
    public partial class ViewDWHourlySalesSrcCollection : ReadOnlyList<ViewDWHourlySalesSrc, ViewDWHourlySalesSrcCollection>
    {        
        public ViewDWHourlySalesSrcCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the viewDW_HourlySales_src view.
    /// </summary>
    [Serializable]
    public partial class ViewDWHourlySalesSrc : ReadOnlyRecord<ViewDWHourlySalesSrc>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("viewDW_HourlySales_src", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
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
                
                TableSchema.TableColumn colvarOutletName = new TableSchema.TableColumn(schema);
                colvarOutletName.ColumnName = "OutletName";
                colvarOutletName.DataType = DbType.AnsiString;
                colvarOutletName.MaxLength = 50;
                colvarOutletName.AutoIncrement = false;
                colvarOutletName.IsNullable = true;
                colvarOutletName.IsPrimaryKey = false;
                colvarOutletName.IsForeignKey = false;
                colvarOutletName.IsReadOnly = false;
                
                schema.Columns.Add(colvarOutletName);
                
                TableSchema.TableColumn colvarPax = new TableSchema.TableColumn(schema);
                colvarPax.ColumnName = "Pax";
                colvarPax.DataType = DbType.Int32;
                colvarPax.MaxLength = 0;
                colvarPax.AutoIncrement = false;
                colvarPax.IsNullable = true;
                colvarPax.IsPrimaryKey = false;
                colvarPax.IsForeignKey = false;
                colvarPax.IsReadOnly = false;
                
                schema.Columns.Add(colvarPax);
                
                TableSchema.TableColumn colvarBill = new TableSchema.TableColumn(schema);
                colvarBill.ColumnName = "Bill";
                colvarBill.DataType = DbType.Int32;
                colvarBill.MaxLength = 0;
                colvarBill.AutoIncrement = false;
                colvarBill.IsNullable = true;
                colvarBill.IsPrimaryKey = false;
                colvarBill.IsForeignKey = false;
                colvarBill.IsReadOnly = false;
                
                schema.Columns.Add(colvarBill);
                
                TableSchema.TableColumn colvarGrossAmount = new TableSchema.TableColumn(schema);
                colvarGrossAmount.ColumnName = "GrossAmount";
                colvarGrossAmount.DataType = DbType.Decimal;
                colvarGrossAmount.MaxLength = 0;
                colvarGrossAmount.AutoIncrement = false;
                colvarGrossAmount.IsNullable = true;
                colvarGrossAmount.IsPrimaryKey = false;
                colvarGrossAmount.IsForeignKey = false;
                colvarGrossAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarGrossAmount);
                
                TableSchema.TableColumn colvarDisc = new TableSchema.TableColumn(schema);
                colvarDisc.ColumnName = "Disc";
                colvarDisc.DataType = DbType.Decimal;
                colvarDisc.MaxLength = 0;
                colvarDisc.AutoIncrement = false;
                colvarDisc.IsNullable = true;
                colvarDisc.IsPrimaryKey = false;
                colvarDisc.IsForeignKey = false;
                colvarDisc.IsReadOnly = false;
                
                schema.Columns.Add(colvarDisc);
                
                TableSchema.TableColumn colvarAfterDisc = new TableSchema.TableColumn(schema);
                colvarAfterDisc.ColumnName = "AfterDisc";
                colvarAfterDisc.DataType = DbType.Currency;
                colvarAfterDisc.MaxLength = 0;
                colvarAfterDisc.AutoIncrement = false;
                colvarAfterDisc.IsNullable = true;
                colvarAfterDisc.IsPrimaryKey = false;
                colvarAfterDisc.IsForeignKey = false;
                colvarAfterDisc.IsReadOnly = false;
                
                schema.Columns.Add(colvarAfterDisc);
                
                TableSchema.TableColumn colvarSvcCharge = new TableSchema.TableColumn(schema);
                colvarSvcCharge.ColumnName = "SvcCharge";
                colvarSvcCharge.DataType = DbType.Int32;
                colvarSvcCharge.MaxLength = 0;
                colvarSvcCharge.AutoIncrement = false;
                colvarSvcCharge.IsNullable = false;
                colvarSvcCharge.IsPrimaryKey = false;
                colvarSvcCharge.IsForeignKey = false;
                colvarSvcCharge.IsReadOnly = false;
                
                schema.Columns.Add(colvarSvcCharge);
                
                TableSchema.TableColumn colvarBefGST = new TableSchema.TableColumn(schema);
                colvarBefGST.ColumnName = "BefGST";
                colvarBefGST.DataType = DbType.Currency;
                colvarBefGST.MaxLength = 0;
                colvarBefGST.AutoIncrement = false;
                colvarBefGST.IsNullable = true;
                colvarBefGST.IsPrimaryKey = false;
                colvarBefGST.IsForeignKey = false;
                colvarBefGST.IsReadOnly = false;
                
                schema.Columns.Add(colvarBefGST);
                
                TableSchema.TableColumn colvarGst = new TableSchema.TableColumn(schema);
                colvarGst.ColumnName = "GST";
                colvarGst.DataType = DbType.Currency;
                colvarGst.MaxLength = 0;
                colvarGst.AutoIncrement = false;
                colvarGst.IsNullable = true;
                colvarGst.IsPrimaryKey = false;
                colvarGst.IsForeignKey = false;
                colvarGst.IsReadOnly = false;
                
                schema.Columns.Add(colvarGst);
                
                TableSchema.TableColumn colvarRounding = new TableSchema.TableColumn(schema);
                colvarRounding.ColumnName = "Rounding";
                colvarRounding.DataType = DbType.Currency;
                colvarRounding.MaxLength = 0;
                colvarRounding.AutoIncrement = false;
                colvarRounding.IsNullable = true;
                colvarRounding.IsPrimaryKey = false;
                colvarRounding.IsForeignKey = false;
                colvarRounding.IsReadOnly = false;
                
                schema.Columns.Add(colvarRounding);
                
                TableSchema.TableColumn colvarNettAmount = new TableSchema.TableColumn(schema);
                colvarNettAmount.ColumnName = "NettAmount";
                colvarNettAmount.DataType = DbType.Currency;
                colvarNettAmount.MaxLength = 0;
                colvarNettAmount.AutoIncrement = false;
                colvarNettAmount.IsNullable = true;
                colvarNettAmount.IsPrimaryKey = false;
                colvarNettAmount.IsForeignKey = false;
                colvarNettAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarNettAmount);
                
                TableSchema.TableColumn colvarPointSale = new TableSchema.TableColumn(schema);
                colvarPointSale.ColumnName = "PointSale";
                colvarPointSale.DataType = DbType.Currency;
                colvarPointSale.MaxLength = 0;
                colvarPointSale.AutoIncrement = false;
                colvarPointSale.IsNullable = true;
                colvarPointSale.IsPrimaryKey = false;
                colvarPointSale.IsForeignKey = false;
                colvarPointSale.IsReadOnly = false;
                
                schema.Columns.Add(colvarPointSale);
                
                TableSchema.TableColumn colvarInstallmentPaymentSale = new TableSchema.TableColumn(schema);
                colvarInstallmentPaymentSale.ColumnName = "InstallmentPaymentSale";
                colvarInstallmentPaymentSale.DataType = DbType.Currency;
                colvarInstallmentPaymentSale.MaxLength = 0;
                colvarInstallmentPaymentSale.AutoIncrement = false;
                colvarInstallmentPaymentSale.IsNullable = true;
                colvarInstallmentPaymentSale.IsPrimaryKey = false;
                colvarInstallmentPaymentSale.IsForeignKey = false;
                colvarInstallmentPaymentSale.IsReadOnly = false;
                
                schema.Columns.Add(colvarInstallmentPaymentSale);
                
                TableSchema.TableColumn colvarRegenerate = new TableSchema.TableColumn(schema);
                colvarRegenerate.ColumnName = "Regenerate";
                colvarRegenerate.DataType = DbType.Int32;
                colvarRegenerate.MaxLength = 0;
                colvarRegenerate.AutoIncrement = false;
                colvarRegenerate.IsNullable = false;
                colvarRegenerate.IsPrimaryKey = false;
                colvarRegenerate.IsForeignKey = false;
                colvarRegenerate.IsReadOnly = false;
                
                schema.Columns.Add(colvarRegenerate);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("viewDW_HourlySales_src",schema);
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
	    public ViewDWHourlySalesSrc()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewDWHourlySalesSrc(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewDWHourlySalesSrc(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewDWHourlySalesSrc(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
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
	      
        [XmlAttribute("Pax")]
        [Bindable(true)]
        public int? Pax 
	    {
		    get
		    {
			    return GetColumnValue<int?>("Pax");
		    }
            set 
		    {
			    SetColumnValue("Pax", value);
            }
        }
	      
        [XmlAttribute("Bill")]
        [Bindable(true)]
        public int? Bill 
	    {
		    get
		    {
			    return GetColumnValue<int?>("Bill");
		    }
            set 
		    {
			    SetColumnValue("Bill", value);
            }
        }
	      
        [XmlAttribute("GrossAmount")]
        [Bindable(true)]
        public decimal? GrossAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("GrossAmount");
		    }
            set 
		    {
			    SetColumnValue("GrossAmount", value);
            }
        }
	      
        [XmlAttribute("Disc")]
        [Bindable(true)]
        public decimal? Disc 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Disc");
		    }
            set 
		    {
			    SetColumnValue("Disc", value);
            }
        }
	      
        [XmlAttribute("AfterDisc")]
        [Bindable(true)]
        public decimal? AfterDisc 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("AfterDisc");
		    }
            set 
		    {
			    SetColumnValue("AfterDisc", value);
            }
        }
	      
        [XmlAttribute("SvcCharge")]
        [Bindable(true)]
        public int SvcCharge 
	    {
		    get
		    {
			    return GetColumnValue<int>("SvcCharge");
		    }
            set 
		    {
			    SetColumnValue("SvcCharge", value);
            }
        }
	      
        [XmlAttribute("BefGST")]
        [Bindable(true)]
        public decimal? BefGST 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("BefGST");
		    }
            set 
		    {
			    SetColumnValue("BefGST", value);
            }
        }
	      
        [XmlAttribute("Gst")]
        [Bindable(true)]
        public decimal? Gst 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("GST");
		    }
            set 
		    {
			    SetColumnValue("GST", value);
            }
        }
	      
        [XmlAttribute("Rounding")]
        [Bindable(true)]
        public decimal? Rounding 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Rounding");
		    }
            set 
		    {
			    SetColumnValue("Rounding", value);
            }
        }
	      
        [XmlAttribute("NettAmount")]
        [Bindable(true)]
        public decimal? NettAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("NettAmount");
		    }
            set 
		    {
			    SetColumnValue("NettAmount", value);
            }
        }
	      
        [XmlAttribute("PointSale")]
        [Bindable(true)]
        public decimal? PointSale 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("PointSale");
		    }
            set 
		    {
			    SetColumnValue("PointSale", value);
            }
        }
	      
        [XmlAttribute("InstallmentPaymentSale")]
        [Bindable(true)]
        public decimal? InstallmentPaymentSale 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("InstallmentPaymentSale");
		    }
            set 
		    {
			    SetColumnValue("InstallmentPaymentSale", value);
            }
        }
	      
        [XmlAttribute("Regenerate")]
        [Bindable(true)]
        public int Regenerate 
	    {
		    get
		    {
			    return GetColumnValue<int>("Regenerate");
		    }
            set 
		    {
			    SetColumnValue("Regenerate", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string OrderDate = @"OrderDate";
            
            public static string OutletName = @"OutletName";
            
            public static string Pax = @"Pax";
            
            public static string Bill = @"Bill";
            
            public static string GrossAmount = @"GrossAmount";
            
            public static string Disc = @"Disc";
            
            public static string AfterDisc = @"AfterDisc";
            
            public static string SvcCharge = @"SvcCharge";
            
            public static string BefGST = @"BefGST";
            
            public static string Gst = @"GST";
            
            public static string Rounding = @"Rounding";
            
            public static string NettAmount = @"NettAmount";
            
            public static string PointSale = @"PointSale";
            
            public static string InstallmentPaymentSale = @"InstallmentPaymentSale";
            
            public static string Regenerate = @"Regenerate";
            
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
