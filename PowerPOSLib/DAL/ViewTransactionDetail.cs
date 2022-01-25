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
    /// Strongly-typed collection for the ViewTransactionDetail class.
    /// </summary>
    [Serializable]
    public partial class ViewTransactionDetailCollection : ReadOnlyList<ViewTransactionDetail, ViewTransactionDetailCollection>
    {        
        public ViewTransactionDetailCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewTransactionDetail view.
    /// </summary>
    [Serializable]
    public partial class ViewTransactionDetail : ReadOnlyRecord<ViewTransactionDetail>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewTransactionDetail", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
                colvarItemNo.ColumnName = "ItemNo";
                colvarItemNo.DataType = DbType.AnsiString;
                colvarItemNo.MaxLength = 50;
                colvarItemNo.AutoIncrement = false;
                colvarItemNo.IsNullable = false;
                colvarItemNo.IsPrimaryKey = false;
                colvarItemNo.IsForeignKey = false;
                colvarItemNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemNo);
                
                TableSchema.TableColumn colvarOrderDetDate = new TableSchema.TableColumn(schema);
                colvarOrderDetDate.ColumnName = "OrderDetDate";
                colvarOrderDetDate.DataType = DbType.DateTime;
                colvarOrderDetDate.MaxLength = 0;
                colvarOrderDetDate.AutoIncrement = false;
                colvarOrderDetDate.IsNullable = false;
                colvarOrderDetDate.IsPrimaryKey = false;
                colvarOrderDetDate.IsForeignKey = false;
                colvarOrderDetDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDetDate);
                
                TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
                colvarQuantity.ColumnName = "Quantity";
                colvarQuantity.DataType = DbType.Decimal;
                colvarQuantity.MaxLength = 0;
                colvarQuantity.AutoIncrement = false;
                colvarQuantity.IsNullable = true;
                colvarQuantity.IsPrimaryKey = false;
                colvarQuantity.IsForeignKey = false;
                colvarQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuantity);
                
                TableSchema.TableColumn colvarUnitPrice = new TableSchema.TableColumn(schema);
                colvarUnitPrice.ColumnName = "UnitPrice";
                colvarUnitPrice.DataType = DbType.Currency;
                colvarUnitPrice.MaxLength = 0;
                colvarUnitPrice.AutoIncrement = false;
                colvarUnitPrice.IsNullable = false;
                colvarUnitPrice.IsPrimaryKey = false;
                colvarUnitPrice.IsForeignKey = false;
                colvarUnitPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarUnitPrice);
                
                TableSchema.TableColumn colvarDiscount = new TableSchema.TableColumn(schema);
                colvarDiscount.ColumnName = "Discount";
                colvarDiscount.DataType = DbType.Currency;
                colvarDiscount.MaxLength = 0;
                colvarDiscount.AutoIncrement = false;
                colvarDiscount.IsNullable = false;
                colvarDiscount.IsPrimaryKey = false;
                colvarDiscount.IsForeignKey = false;
                colvarDiscount.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscount);
                
                TableSchema.TableColumn colvarAmount = new TableSchema.TableColumn(schema);
                colvarAmount.ColumnName = "Amount";
                colvarAmount.DataType = DbType.Currency;
                colvarAmount.MaxLength = 0;
                colvarAmount.AutoIncrement = false;
                colvarAmount.IsNullable = false;
                colvarAmount.IsPrimaryKey = false;
                colvarAmount.IsForeignKey = false;
                colvarAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarAmount);
                
                TableSchema.TableColumn colvarIsPromo = new TableSchema.TableColumn(schema);
                colvarIsPromo.ColumnName = "IsPromo";
                colvarIsPromo.DataType = DbType.Boolean;
                colvarIsPromo.MaxLength = 0;
                colvarIsPromo.AutoIncrement = false;
                colvarIsPromo.IsNullable = false;
                colvarIsPromo.IsPrimaryKey = false;
                colvarIsPromo.IsForeignKey = false;
                colvarIsPromo.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsPromo);
                
                TableSchema.TableColumn colvarPromoDiscount = new TableSchema.TableColumn(schema);
                colvarPromoDiscount.ColumnName = "PromoDiscount";
                colvarPromoDiscount.DataType = DbType.Double;
                colvarPromoDiscount.MaxLength = 0;
                colvarPromoDiscount.AutoIncrement = false;
                colvarPromoDiscount.IsNullable = false;
                colvarPromoDiscount.IsPrimaryKey = false;
                colvarPromoDiscount.IsForeignKey = false;
                colvarPromoDiscount.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoDiscount);
                
                TableSchema.TableColumn colvarPromoAmount = new TableSchema.TableColumn(schema);
                colvarPromoAmount.ColumnName = "PromoAmount";
                colvarPromoAmount.DataType = DbType.Currency;
                colvarPromoAmount.MaxLength = 0;
                colvarPromoAmount.AutoIncrement = false;
                colvarPromoAmount.IsNullable = false;
                colvarPromoAmount.IsPrimaryKey = false;
                colvarPromoAmount.IsForeignKey = false;
                colvarPromoAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoAmount);
                
                TableSchema.TableColumn colvarIsPromoFreeOfCharge = new TableSchema.TableColumn(schema);
                colvarIsPromoFreeOfCharge.ColumnName = "IsPromoFreeOfCharge";
                colvarIsPromoFreeOfCharge.DataType = DbType.Boolean;
                colvarIsPromoFreeOfCharge.MaxLength = 0;
                colvarIsPromoFreeOfCharge.AutoIncrement = false;
                colvarIsPromoFreeOfCharge.IsNullable = false;
                colvarIsPromoFreeOfCharge.IsPrimaryKey = false;
                colvarIsPromoFreeOfCharge.IsForeignKey = false;
                colvarIsPromoFreeOfCharge.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsPromoFreeOfCharge);
                
                TableSchema.TableColumn colvarIsFreeOfCharge = new TableSchema.TableColumn(schema);
                colvarIsFreeOfCharge.ColumnName = "IsFreeOfCharge";
                colvarIsFreeOfCharge.DataType = DbType.Boolean;
                colvarIsFreeOfCharge.MaxLength = 0;
                colvarIsFreeOfCharge.AutoIncrement = false;
                colvarIsFreeOfCharge.IsNullable = false;
                colvarIsFreeOfCharge.IsPrimaryKey = false;
                colvarIsFreeOfCharge.IsForeignKey = false;
                colvarIsFreeOfCharge.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsFreeOfCharge);
                
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
                
                TableSchema.TableColumn colvarOrderRefNo = new TableSchema.TableColumn(schema);
                colvarOrderRefNo.ColumnName = "OrderRefNo";
                colvarOrderRefNo.DataType = DbType.AnsiString;
                colvarOrderRefNo.MaxLength = 50;
                colvarOrderRefNo.AutoIncrement = false;
                colvarOrderRefNo.IsNullable = false;
                colvarOrderRefNo.IsPrimaryKey = false;
                colvarOrderRefNo.IsForeignKey = false;
                colvarOrderRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderRefNo);
                
                TableSchema.TableColumn colvarItemName = new TableSchema.TableColumn(schema);
                colvarItemName.ColumnName = "ItemName";
                colvarItemName.DataType = DbType.String;
                colvarItemName.MaxLength = 300;
                colvarItemName.AutoIncrement = false;
                colvarItemName.IsNullable = false;
                colvarItemName.IsPrimaryKey = false;
                colvarItemName.IsForeignKey = false;
                colvarItemName.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemName);
                
                TableSchema.TableColumn colvarNettAmount = new TableSchema.TableColumn(schema);
                colvarNettAmount.ColumnName = "NettAmount";
                colvarNettAmount.DataType = DbType.Currency;
                colvarNettAmount.MaxLength = 0;
                colvarNettAmount.AutoIncrement = false;
                colvarNettAmount.IsNullable = false;
                colvarNettAmount.IsPrimaryKey = false;
                colvarNettAmount.IsForeignKey = false;
                colvarNettAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarNettAmount);
                
                TableSchema.TableColumn colvarIsVoided = new TableSchema.TableColumn(schema);
                colvarIsVoided.ColumnName = "IsVoided";
                colvarIsVoided.DataType = DbType.Boolean;
                colvarIsVoided.MaxLength = 0;
                colvarIsVoided.AutoIncrement = false;
                colvarIsVoided.IsNullable = false;
                colvarIsVoided.IsPrimaryKey = false;
                colvarIsVoided.IsForeignKey = false;
                colvarIsVoided.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsVoided);
                
                TableSchema.TableColumn colvarIsLineVoided = new TableSchema.TableColumn(schema);
                colvarIsLineVoided.ColumnName = "IsLineVoided";
                colvarIsLineVoided.DataType = DbType.Boolean;
                colvarIsLineVoided.MaxLength = 0;
                colvarIsLineVoided.AutoIncrement = false;
                colvarIsLineVoided.IsNullable = false;
                colvarIsLineVoided.IsPrimaryKey = false;
                colvarIsLineVoided.IsForeignKey = false;
                colvarIsLineVoided.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsLineVoided);
                
                TableSchema.TableColumn colvarCategoryName = new TableSchema.TableColumn(schema);
                colvarCategoryName.ColumnName = "CategoryName";
                colvarCategoryName.DataType = DbType.String;
                colvarCategoryName.MaxLength = 250;
                colvarCategoryName.AutoIncrement = false;
                colvarCategoryName.IsNullable = false;
                colvarCategoryName.IsPrimaryKey = false;
                colvarCategoryName.IsForeignKey = false;
                colvarCategoryName.IsReadOnly = false;
                
                schema.Columns.Add(colvarCategoryName);
                
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
                
                TableSchema.TableColumn colvarInventoryHdrRefNo = new TableSchema.TableColumn(schema);
                colvarInventoryHdrRefNo.ColumnName = "InventoryHdrRefNo";
                colvarInventoryHdrRefNo.DataType = DbType.AnsiString;
                colvarInventoryHdrRefNo.MaxLength = 50;
                colvarInventoryHdrRefNo.AutoIncrement = false;
                colvarInventoryHdrRefNo.IsNullable = true;
                colvarInventoryHdrRefNo.IsPrimaryKey = false;
                colvarInventoryHdrRefNo.IsForeignKey = false;
                colvarInventoryHdrRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryHdrRefNo);
                
                TableSchema.TableColumn colvarIsPreOrder = new TableSchema.TableColumn(schema);
                colvarIsPreOrder.ColumnName = "IsPreOrder";
                colvarIsPreOrder.DataType = DbType.Boolean;
                colvarIsPreOrder.MaxLength = 0;
                colvarIsPreOrder.AutoIncrement = false;
                colvarIsPreOrder.IsNullable = true;
                colvarIsPreOrder.IsPrimaryKey = false;
                colvarIsPreOrder.IsForeignKey = false;
                colvarIsPreOrder.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsPreOrder);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewTransactionDetail",schema);
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
	    public ViewTransactionDetail()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewTransactionDetail(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewTransactionDetail(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewTransactionDetail(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("ItemNo")]
        [Bindable(true)]
        public string ItemNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemNo");
		    }
            set 
		    {
			    SetColumnValue("ItemNo", value);
            }
        }
	      
        [XmlAttribute("OrderDetDate")]
        [Bindable(true)]
        public DateTime OrderDetDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("OrderDetDate");
		    }
            set 
		    {
			    SetColumnValue("OrderDetDate", value);
            }
        }
	      
        [XmlAttribute("Quantity")]
        [Bindable(true)]
        public decimal? Quantity 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Quantity");
		    }
            set 
		    {
			    SetColumnValue("Quantity", value);
            }
        }
	      
        [XmlAttribute("UnitPrice")]
        [Bindable(true)]
        public decimal UnitPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("UnitPrice");
		    }
            set 
		    {
			    SetColumnValue("UnitPrice", value);
            }
        }
	      
        [XmlAttribute("Discount")]
        [Bindable(true)]
        public decimal Discount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("Discount");
		    }
            set 
		    {
			    SetColumnValue("Discount", value);
            }
        }
	      
        [XmlAttribute("Amount")]
        [Bindable(true)]
        public decimal Amount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("Amount");
		    }
            set 
		    {
			    SetColumnValue("Amount", value);
            }
        }
	      
        [XmlAttribute("IsPromo")]
        [Bindable(true)]
        public bool IsPromo 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsPromo");
		    }
            set 
		    {
			    SetColumnValue("IsPromo", value);
            }
        }
	      
        [XmlAttribute("PromoDiscount")]
        [Bindable(true)]
        public double PromoDiscount 
	    {
		    get
		    {
			    return GetColumnValue<double>("PromoDiscount");
		    }
            set 
		    {
			    SetColumnValue("PromoDiscount", value);
            }
        }
	      
        [XmlAttribute("PromoAmount")]
        [Bindable(true)]
        public decimal PromoAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("PromoAmount");
		    }
            set 
		    {
			    SetColumnValue("PromoAmount", value);
            }
        }
	      
        [XmlAttribute("IsPromoFreeOfCharge")]
        [Bindable(true)]
        public bool IsPromoFreeOfCharge 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsPromoFreeOfCharge");
		    }
            set 
		    {
			    SetColumnValue("IsPromoFreeOfCharge", value);
            }
        }
	      
        [XmlAttribute("IsFreeOfCharge")]
        [Bindable(true)]
        public bool IsFreeOfCharge 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsFreeOfCharge");
		    }
            set 
		    {
			    SetColumnValue("IsFreeOfCharge", value);
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
	      
        [XmlAttribute("OrderRefNo")]
        [Bindable(true)]
        public string OrderRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderRefNo");
		    }
            set 
		    {
			    SetColumnValue("OrderRefNo", value);
            }
        }
	      
        [XmlAttribute("ItemName")]
        [Bindable(true)]
        public string ItemName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemName");
		    }
            set 
		    {
			    SetColumnValue("ItemName", value);
            }
        }
	      
        [XmlAttribute("NettAmount")]
        [Bindable(true)]
        public decimal NettAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("NettAmount");
		    }
            set 
		    {
			    SetColumnValue("NettAmount", value);
            }
        }
	      
        [XmlAttribute("IsVoided")]
        [Bindable(true)]
        public bool IsVoided 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsVoided");
		    }
            set 
		    {
			    SetColumnValue("IsVoided", value);
            }
        }
	      
        [XmlAttribute("IsLineVoided")]
        [Bindable(true)]
        public bool IsLineVoided 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsLineVoided");
		    }
            set 
		    {
			    SetColumnValue("IsLineVoided", value);
            }
        }
	      
        [XmlAttribute("CategoryName")]
        [Bindable(true)]
        public string CategoryName 
	    {
		    get
		    {
			    return GetColumnValue<string>("CategoryName");
		    }
            set 
		    {
			    SetColumnValue("CategoryName", value);
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
	      
        [XmlAttribute("InventoryHdrRefNo")]
        [Bindable(true)]
        public string InventoryHdrRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("InventoryHdrRefNo");
		    }
            set 
		    {
			    SetColumnValue("InventoryHdrRefNo", value);
            }
        }
	      
        [XmlAttribute("IsPreOrder")]
        [Bindable(true)]
        public bool? IsPreOrder 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsPreOrder");
		    }
            set 
		    {
			    SetColumnValue("IsPreOrder", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string ItemNo = @"ItemNo";
            
            public static string OrderDetDate = @"OrderDetDate";
            
            public static string Quantity = @"Quantity";
            
            public static string UnitPrice = @"UnitPrice";
            
            public static string Discount = @"Discount";
            
            public static string Amount = @"Amount";
            
            public static string IsPromo = @"IsPromo";
            
            public static string PromoDiscount = @"PromoDiscount";
            
            public static string PromoAmount = @"PromoAmount";
            
            public static string IsPromoFreeOfCharge = @"IsPromoFreeOfCharge";
            
            public static string IsFreeOfCharge = @"IsFreeOfCharge";
            
            public static string PointOfSaleName = @"PointOfSaleName";
            
            public static string OrderRefNo = @"OrderRefNo";
            
            public static string ItemName = @"ItemName";
            
            public static string NettAmount = @"NettAmount";
            
            public static string IsVoided = @"IsVoided";
            
            public static string IsLineVoided = @"IsLineVoided";
            
            public static string CategoryName = @"CategoryName";
            
            public static string OutletName = @"OutletName";
            
            public static string DepartmentID = @"DepartmentID";
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string InventoryHdrRefNo = @"InventoryHdrRefNo";
            
            public static string IsPreOrder = @"IsPreOrder";
            
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
