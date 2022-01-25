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
    /// Strongly-typed collection for the ViewPurchaseOrderHeader class.
    /// </summary>
    [Serializable]
    public partial class ViewPurchaseOrderHeaderCollection : ReadOnlyList<ViewPurchaseOrderHeader, ViewPurchaseOrderHeaderCollection>
    {        
        public ViewPurchaseOrderHeaderCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewPurchaseOrderHeader view.
    /// </summary>
    [Serializable]
    public partial class ViewPurchaseOrderHeader : ReadOnlyRecord<ViewPurchaseOrderHeader>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewPurchaseOrderHeader", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarPurchaseOrderHeaderRefNo = new TableSchema.TableColumn(schema);
                colvarPurchaseOrderHeaderRefNo.ColumnName = "PurchaseOrderHeaderRefNo";
                colvarPurchaseOrderHeaderRefNo.DataType = DbType.AnsiString;
                colvarPurchaseOrderHeaderRefNo.MaxLength = 50;
                colvarPurchaseOrderHeaderRefNo.AutoIncrement = false;
                colvarPurchaseOrderHeaderRefNo.IsNullable = false;
                colvarPurchaseOrderHeaderRefNo.IsPrimaryKey = false;
                colvarPurchaseOrderHeaderRefNo.IsForeignKey = false;
                colvarPurchaseOrderHeaderRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarPurchaseOrderHeaderRefNo);
                
                TableSchema.TableColumn colvarRequestedBy = new TableSchema.TableColumn(schema);
                colvarRequestedBy.ColumnName = "RequestedBy";
                colvarRequestedBy.DataType = DbType.AnsiString;
                colvarRequestedBy.MaxLength = 50;
                colvarRequestedBy.AutoIncrement = false;
                colvarRequestedBy.IsNullable = false;
                colvarRequestedBy.IsPrimaryKey = false;
                colvarRequestedBy.IsForeignKey = false;
                colvarRequestedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarRequestedBy);
                
                TableSchema.TableColumn colvarPurchaseOrderDate = new TableSchema.TableColumn(schema);
                colvarPurchaseOrderDate.ColumnName = "PurchaseOrderDate";
                colvarPurchaseOrderDate.DataType = DbType.DateTime;
                colvarPurchaseOrderDate.MaxLength = 0;
                colvarPurchaseOrderDate.AutoIncrement = false;
                colvarPurchaseOrderDate.IsNullable = false;
                colvarPurchaseOrderDate.IsPrimaryKey = false;
                colvarPurchaseOrderDate.IsForeignKey = false;
                colvarPurchaseOrderDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarPurchaseOrderDate);
                
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
                
                TableSchema.TableColumn colvarPaymentTermID = new TableSchema.TableColumn(schema);
                colvarPaymentTermID.ColumnName = "PaymentTermID";
                colvarPaymentTermID.DataType = DbType.Int32;
                colvarPaymentTermID.MaxLength = 0;
                colvarPaymentTermID.AutoIncrement = false;
                colvarPaymentTermID.IsNullable = true;
                colvarPaymentTermID.IsPrimaryKey = false;
                colvarPaymentTermID.IsForeignKey = false;
                colvarPaymentTermID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPaymentTermID);
                
                TableSchema.TableColumn colvarShipVia = new TableSchema.TableColumn(schema);
                colvarShipVia.ColumnName = "ShipVia";
                colvarShipVia.DataType = DbType.AnsiString;
                colvarShipVia.MaxLength = 50;
                colvarShipVia.AutoIncrement = false;
                colvarShipVia.IsNullable = true;
                colvarShipVia.IsPrimaryKey = false;
                colvarShipVia.IsForeignKey = false;
                colvarShipVia.IsReadOnly = false;
                
                schema.Columns.Add(colvarShipVia);
                
                TableSchema.TableColumn colvarShipTo = new TableSchema.TableColumn(schema);
                colvarShipTo.ColumnName = "ShipTo";
                colvarShipTo.DataType = DbType.AnsiString;
                colvarShipTo.MaxLength = 50;
                colvarShipTo.AutoIncrement = false;
                colvarShipTo.IsNullable = true;
                colvarShipTo.IsPrimaryKey = false;
                colvarShipTo.IsForeignKey = false;
                colvarShipTo.IsReadOnly = false;
                
                schema.Columns.Add(colvarShipTo);
                
                TableSchema.TableColumn colvarDateNeededBy = new TableSchema.TableColumn(schema);
                colvarDateNeededBy.ColumnName = "DateNeededBy";
                colvarDateNeededBy.DataType = DbType.DateTime;
                colvarDateNeededBy.MaxLength = 0;
                colvarDateNeededBy.AutoIncrement = false;
                colvarDateNeededBy.IsNullable = true;
                colvarDateNeededBy.IsPrimaryKey = false;
                colvarDateNeededBy.IsForeignKey = false;
                colvarDateNeededBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarDateNeededBy);
                
                TableSchema.TableColumn colvarSupplierID = new TableSchema.TableColumn(schema);
                colvarSupplierID.ColumnName = "SupplierID";
                colvarSupplierID.DataType = DbType.Int32;
                colvarSupplierID.MaxLength = 0;
                colvarSupplierID.AutoIncrement = false;
                colvarSupplierID.IsNullable = true;
                colvarSupplierID.IsPrimaryKey = false;
                colvarSupplierID.IsForeignKey = false;
                colvarSupplierID.IsReadOnly = false;
                
                schema.Columns.Add(colvarSupplierID);
                
                TableSchema.TableColumn colvarUserName = new TableSchema.TableColumn(schema);
                colvarUserName.ColumnName = "UserName";
                colvarUserName.DataType = DbType.AnsiString;
                colvarUserName.MaxLength = 50;
                colvarUserName.AutoIncrement = false;
                colvarUserName.IsNullable = true;
                colvarUserName.IsPrimaryKey = false;
                colvarUserName.IsForeignKey = false;
                colvarUserName.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserName);
                
                TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
                colvarRemark.ColumnName = "Remark";
                colvarRemark.DataType = DbType.String;
                colvarRemark.MaxLength = 50;
                colvarRemark.AutoIncrement = false;
                colvarRemark.IsNullable = true;
                colvarRemark.IsPrimaryKey = false;
                colvarRemark.IsForeignKey = false;
                colvarRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemark);
                
                TableSchema.TableColumn colvarInventoryLocationID = new TableSchema.TableColumn(schema);
                colvarInventoryLocationID.ColumnName = "InventoryLocationID";
                colvarInventoryLocationID.DataType = DbType.Int32;
                colvarInventoryLocationID.MaxLength = 0;
                colvarInventoryLocationID.AutoIncrement = false;
                colvarInventoryLocationID.IsNullable = true;
                colvarInventoryLocationID.IsPrimaryKey = false;
                colvarInventoryLocationID.IsForeignKey = false;
                colvarInventoryLocationID.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryLocationID);
                
                TableSchema.TableColumn colvarInventoryLocationName = new TableSchema.TableColumn(schema);
                colvarInventoryLocationName.ColumnName = "InventoryLocationName";
                colvarInventoryLocationName.DataType = DbType.AnsiString;
                colvarInventoryLocationName.MaxLength = 50;
                colvarInventoryLocationName.AutoIncrement = false;
                colvarInventoryLocationName.IsNullable = true;
                colvarInventoryLocationName.IsPrimaryKey = false;
                colvarInventoryLocationName.IsForeignKey = false;
                colvarInventoryLocationName.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryLocationName);
                
                TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
                colvarCreatedBy.ColumnName = "CreatedBy";
                colvarCreatedBy.DataType = DbType.AnsiString;
                colvarCreatedBy.MaxLength = 50;
                colvarCreatedBy.AutoIncrement = false;
                colvarCreatedBy.IsNullable = true;
                colvarCreatedBy.IsPrimaryKey = false;
                colvarCreatedBy.IsForeignKey = false;
                colvarCreatedBy.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarCreatedOn);
                
                TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
                colvarModifiedBy.ColumnName = "ModifiedBy";
                colvarModifiedBy.DataType = DbType.AnsiString;
                colvarModifiedBy.MaxLength = 50;
                colvarModifiedBy.AutoIncrement = false;
                colvarModifiedBy.IsNullable = true;
                colvarModifiedBy.IsPrimaryKey = false;
                colvarModifiedBy.IsForeignKey = false;
                colvarModifiedBy.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarModifiedOn);
                
                TableSchema.TableColumn colvarStatus = new TableSchema.TableColumn(schema);
                colvarStatus.ColumnName = "Status";
                colvarStatus.DataType = DbType.AnsiString;
                colvarStatus.MaxLength = 50;
                colvarStatus.AutoIncrement = false;
                colvarStatus.IsNullable = false;
                colvarStatus.IsPrimaryKey = false;
                colvarStatus.IsForeignKey = false;
                colvarStatus.IsReadOnly = false;
                
                schema.Columns.Add(colvarStatus);
                
                TableSchema.TableColumn colvarPOType = new TableSchema.TableColumn(schema);
                colvarPOType.ColumnName = "POType";
                colvarPOType.DataType = DbType.AnsiString;
                colvarPOType.MaxLength = 50;
                colvarPOType.AutoIncrement = false;
                colvarPOType.IsNullable = false;
                colvarPOType.IsPrimaryKey = false;
                colvarPOType.IsForeignKey = false;
                colvarPOType.IsReadOnly = false;
                
                schema.Columns.Add(colvarPOType);
                
                TableSchema.TableColumn colvarApprovalDate = new TableSchema.TableColumn(schema);
                colvarApprovalDate.ColumnName = "ApprovalDate";
                colvarApprovalDate.DataType = DbType.AnsiString;
                colvarApprovalDate.MaxLength = 50;
                colvarApprovalDate.AutoIncrement = false;
                colvarApprovalDate.IsNullable = false;
                colvarApprovalDate.IsPrimaryKey = false;
                colvarApprovalDate.IsForeignKey = false;
                colvarApprovalDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarApprovalDate);
                
                TableSchema.TableColumn colvarApprovedBy = new TableSchema.TableColumn(schema);
                colvarApprovedBy.ColumnName = "ApprovedBy";
                colvarApprovedBy.DataType = DbType.AnsiString;
                colvarApprovedBy.MaxLength = 50;
                colvarApprovedBy.AutoIncrement = false;
                colvarApprovedBy.IsNullable = false;
                colvarApprovedBy.IsPrimaryKey = false;
                colvarApprovedBy.IsForeignKey = false;
                colvarApprovedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarApprovedBy);
                
                TableSchema.TableColumn colvarSpecialValidFrom = new TableSchema.TableColumn(schema);
                colvarSpecialValidFrom.ColumnName = "SpecialValidFrom";
                colvarSpecialValidFrom.DataType = DbType.AnsiString;
                colvarSpecialValidFrom.MaxLength = 50;
                colvarSpecialValidFrom.AutoIncrement = false;
                colvarSpecialValidFrom.IsNullable = false;
                colvarSpecialValidFrom.IsPrimaryKey = false;
                colvarSpecialValidFrom.IsForeignKey = false;
                colvarSpecialValidFrom.IsReadOnly = false;
                
                schema.Columns.Add(colvarSpecialValidFrom);
                
                TableSchema.TableColumn colvarSpecialValidTo = new TableSchema.TableColumn(schema);
                colvarSpecialValidTo.ColumnName = "SpecialValidTo";
                colvarSpecialValidTo.DataType = DbType.AnsiString;
                colvarSpecialValidTo.MaxLength = 50;
                colvarSpecialValidTo.AutoIncrement = false;
                colvarSpecialValidTo.IsNullable = false;
                colvarSpecialValidTo.IsPrimaryKey = false;
                colvarSpecialValidTo.IsForeignKey = false;
                colvarSpecialValidTo.IsReadOnly = false;
                
                schema.Columns.Add(colvarSpecialValidTo);
                
                TableSchema.TableColumn colvarApprovalStatus = new TableSchema.TableColumn(schema);
                colvarApprovalStatus.ColumnName = "ApprovalStatus";
                colvarApprovalStatus.DataType = DbType.AnsiString;
                colvarApprovalStatus.MaxLength = 50;
                colvarApprovalStatus.AutoIncrement = false;
                colvarApprovalStatus.IsNullable = false;
                colvarApprovalStatus.IsPrimaryKey = false;
                colvarApprovalStatus.IsForeignKey = false;
                colvarApprovalStatus.IsReadOnly = false;
                
                schema.Columns.Add(colvarApprovalStatus);
                
                TableSchema.TableColumn colvarSalesPersonID = new TableSchema.TableColumn(schema);
                colvarSalesPersonID.ColumnName = "SalesPersonID";
                colvarSalesPersonID.DataType = DbType.AnsiString;
                colvarSalesPersonID.MaxLength = 50;
                colvarSalesPersonID.AutoIncrement = false;
                colvarSalesPersonID.IsNullable = false;
                colvarSalesPersonID.IsPrimaryKey = false;
                colvarSalesPersonID.IsForeignKey = false;
                colvarSalesPersonID.IsReadOnly = false;
                
                schema.Columns.Add(colvarSalesPersonID);
                
                TableSchema.TableColumn colvarPriceLevel = new TableSchema.TableColumn(schema);
                colvarPriceLevel.ColumnName = "PriceLevel";
                colvarPriceLevel.DataType = DbType.AnsiString;
                colvarPriceLevel.MaxLength = 50;
                colvarPriceLevel.AutoIncrement = false;
                colvarPriceLevel.IsNullable = false;
                colvarPriceLevel.IsPrimaryKey = false;
                colvarPriceLevel.IsForeignKey = false;
                colvarPriceLevel.IsReadOnly = false;
                
                schema.Columns.Add(colvarPriceLevel);
                
                TableSchema.TableColumn colvarReasonID = new TableSchema.TableColumn(schema);
                colvarReasonID.ColumnName = "ReasonID";
                colvarReasonID.DataType = DbType.Int32;
                colvarReasonID.MaxLength = 0;
                colvarReasonID.AutoIncrement = false;
                colvarReasonID.IsNullable = true;
                colvarReasonID.IsPrimaryKey = false;
                colvarReasonID.IsForeignKey = false;
                colvarReasonID.IsReadOnly = false;
                
                schema.Columns.Add(colvarReasonID);
                
                TableSchema.TableColumn colvarDestInventoryLocationID = new TableSchema.TableColumn(schema);
                colvarDestInventoryLocationID.ColumnName = "DestInventoryLocationID";
                colvarDestInventoryLocationID.DataType = DbType.Int32;
                colvarDestInventoryLocationID.MaxLength = 0;
                colvarDestInventoryLocationID.AutoIncrement = false;
                colvarDestInventoryLocationID.IsNullable = false;
                colvarDestInventoryLocationID.IsPrimaryKey = false;
                colvarDestInventoryLocationID.IsForeignKey = false;
                colvarDestInventoryLocationID.IsReadOnly = false;
                
                schema.Columns.Add(colvarDestInventoryLocationID);
                
                TableSchema.TableColumn colvarWarehouseID = new TableSchema.TableColumn(schema);
                colvarWarehouseID.ColumnName = "WarehouseID";
                colvarWarehouseID.DataType = DbType.Int32;
                colvarWarehouseID.MaxLength = 0;
                colvarWarehouseID.AutoIncrement = false;
                colvarWarehouseID.IsNullable = false;
                colvarWarehouseID.IsPrimaryKey = false;
                colvarWarehouseID.IsForeignKey = false;
                colvarWarehouseID.IsReadOnly = false;
                
                schema.Columns.Add(colvarWarehouseID);
                
                TableSchema.TableColumn colvarIsAutoStockIn = new TableSchema.TableColumn(schema);
                colvarIsAutoStockIn.ColumnName = "IsAutoStockIn";
                colvarIsAutoStockIn.DataType = DbType.Boolean;
                colvarIsAutoStockIn.MaxLength = 0;
                colvarIsAutoStockIn.AutoIncrement = false;
                colvarIsAutoStockIn.IsNullable = false;
                colvarIsAutoStockIn.IsPrimaryKey = false;
                colvarIsAutoStockIn.IsForeignKey = false;
                colvarIsAutoStockIn.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsAutoStockIn);
                
                TableSchema.TableColumn colvarOrderFromName = new TableSchema.TableColumn(schema);
                colvarOrderFromName.ColumnName = "OrderFromName";
                colvarOrderFromName.DataType = DbType.String;
                colvarOrderFromName.MaxLength = 50;
                colvarOrderFromName.AutoIncrement = false;
                colvarOrderFromName.IsNullable = false;
                colvarOrderFromName.IsPrimaryKey = false;
                colvarOrderFromName.IsForeignKey = false;
                colvarOrderFromName.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderFromName);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewPurchaseOrderHeader",schema);
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
	    public ViewPurchaseOrderHeader()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewPurchaseOrderHeader(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewPurchaseOrderHeader(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewPurchaseOrderHeader(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("PurchaseOrderHeaderRefNo")]
        [Bindable(true)]
        public string PurchaseOrderHeaderRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("PurchaseOrderHeaderRefNo");
		    }
            set 
		    {
			    SetColumnValue("PurchaseOrderHeaderRefNo", value);
            }
        }
	      
        [XmlAttribute("RequestedBy")]
        [Bindable(true)]
        public string RequestedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("RequestedBy");
		    }
            set 
		    {
			    SetColumnValue("RequestedBy", value);
            }
        }
	      
        [XmlAttribute("PurchaseOrderDate")]
        [Bindable(true)]
        public DateTime PurchaseOrderDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("PurchaseOrderDate");
		    }
            set 
		    {
			    SetColumnValue("PurchaseOrderDate", value);
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
	      
        [XmlAttribute("PaymentTermID")]
        [Bindable(true)]
        public int? PaymentTermID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("PaymentTermID");
		    }
            set 
		    {
			    SetColumnValue("PaymentTermID", value);
            }
        }
	      
        [XmlAttribute("ShipVia")]
        [Bindable(true)]
        public string ShipVia 
	    {
		    get
		    {
			    return GetColumnValue<string>("ShipVia");
		    }
            set 
		    {
			    SetColumnValue("ShipVia", value);
            }
        }
	      
        [XmlAttribute("ShipTo")]
        [Bindable(true)]
        public string ShipTo 
	    {
		    get
		    {
			    return GetColumnValue<string>("ShipTo");
		    }
            set 
		    {
			    SetColumnValue("ShipTo", value);
            }
        }
	      
        [XmlAttribute("DateNeededBy")]
        [Bindable(true)]
        public DateTime? DateNeededBy 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("DateNeededBy");
		    }
            set 
		    {
			    SetColumnValue("DateNeededBy", value);
            }
        }
	      
        [XmlAttribute("SupplierID")]
        [Bindable(true)]
        public int? SupplierID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("SupplierID");
		    }
            set 
		    {
			    SetColumnValue("SupplierID", value);
            }
        }
	      
        [XmlAttribute("UserName")]
        [Bindable(true)]
        public string UserName 
	    {
		    get
		    {
			    return GetColumnValue<string>("UserName");
		    }
            set 
		    {
			    SetColumnValue("UserName", value);
            }
        }
	      
        [XmlAttribute("Remark")]
        [Bindable(true)]
        public string Remark 
	    {
		    get
		    {
			    return GetColumnValue<string>("Remark");
		    }
            set 
		    {
			    SetColumnValue("Remark", value);
            }
        }
	      
        [XmlAttribute("InventoryLocationID")]
        [Bindable(true)]
        public int? InventoryLocationID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("InventoryLocationID");
		    }
            set 
		    {
			    SetColumnValue("InventoryLocationID", value);
            }
        }
	      
        [XmlAttribute("InventoryLocationName")]
        [Bindable(true)]
        public string InventoryLocationName 
	    {
		    get
		    {
			    return GetColumnValue<string>("InventoryLocationName");
		    }
            set 
		    {
			    SetColumnValue("InventoryLocationName", value);
            }
        }
	      
        [XmlAttribute("CreatedBy")]
        [Bindable(true)]
        public string CreatedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("CreatedBy");
		    }
            set 
		    {
			    SetColumnValue("CreatedBy", value);
            }
        }
	      
        [XmlAttribute("CreatedOn")]
        [Bindable(true)]
        public DateTime? CreatedOn 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("CreatedOn");
		    }
            set 
		    {
			    SetColumnValue("CreatedOn", value);
            }
        }
	      
        [XmlAttribute("ModifiedBy")]
        [Bindable(true)]
        public string ModifiedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("ModifiedBy");
		    }
            set 
		    {
			    SetColumnValue("ModifiedBy", value);
            }
        }
	      
        [XmlAttribute("ModifiedOn")]
        [Bindable(true)]
        public DateTime? ModifiedOn 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ModifiedOn");
		    }
            set 
		    {
			    SetColumnValue("ModifiedOn", value);
            }
        }
	      
        [XmlAttribute("Status")]
        [Bindable(true)]
        public string Status 
	    {
		    get
		    {
			    return GetColumnValue<string>("Status");
		    }
            set 
		    {
			    SetColumnValue("Status", value);
            }
        }
	      
        [XmlAttribute("POType")]
        [Bindable(true)]
        public string POType 
	    {
		    get
		    {
			    return GetColumnValue<string>("POType");
		    }
            set 
		    {
			    SetColumnValue("POType", value);
            }
        }
	      
        [XmlAttribute("ApprovalDate")]
        [Bindable(true)]
        public string ApprovalDate 
	    {
		    get
		    {
			    return GetColumnValue<string>("ApprovalDate");
		    }
            set 
		    {
			    SetColumnValue("ApprovalDate", value);
            }
        }
	      
        [XmlAttribute("ApprovedBy")]
        [Bindable(true)]
        public string ApprovedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("ApprovedBy");
		    }
            set 
		    {
			    SetColumnValue("ApprovedBy", value);
            }
        }
	      
        [XmlAttribute("SpecialValidFrom")]
        [Bindable(true)]
        public string SpecialValidFrom 
	    {
		    get
		    {
			    return GetColumnValue<string>("SpecialValidFrom");
		    }
            set 
		    {
			    SetColumnValue("SpecialValidFrom", value);
            }
        }
	      
        [XmlAttribute("SpecialValidTo")]
        [Bindable(true)]
        public string SpecialValidTo 
	    {
		    get
		    {
			    return GetColumnValue<string>("SpecialValidTo");
		    }
            set 
		    {
			    SetColumnValue("SpecialValidTo", value);
            }
        }
	      
        [XmlAttribute("ApprovalStatus")]
        [Bindable(true)]
        public string ApprovalStatus 
	    {
		    get
		    {
			    return GetColumnValue<string>("ApprovalStatus");
		    }
            set 
		    {
			    SetColumnValue("ApprovalStatus", value);
            }
        }
	      
        [XmlAttribute("SalesPersonID")]
        [Bindable(true)]
        public string SalesPersonID 
	    {
		    get
		    {
			    return GetColumnValue<string>("SalesPersonID");
		    }
            set 
		    {
			    SetColumnValue("SalesPersonID", value);
            }
        }
	      
        [XmlAttribute("PriceLevel")]
        [Bindable(true)]
        public string PriceLevel 
	    {
		    get
		    {
			    return GetColumnValue<string>("PriceLevel");
		    }
            set 
		    {
			    SetColumnValue("PriceLevel", value);
            }
        }
	      
        [XmlAttribute("ReasonID")]
        [Bindable(true)]
        public int? ReasonID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("ReasonID");
		    }
            set 
		    {
			    SetColumnValue("ReasonID", value);
            }
        }
	      
        [XmlAttribute("DestInventoryLocationID")]
        [Bindable(true)]
        public int DestInventoryLocationID 
	    {
		    get
		    {
			    return GetColumnValue<int>("DestInventoryLocationID");
		    }
            set 
		    {
			    SetColumnValue("DestInventoryLocationID", value);
            }
        }
	      
        [XmlAttribute("WarehouseID")]
        [Bindable(true)]
        public int WarehouseID 
	    {
		    get
		    {
			    return GetColumnValue<int>("WarehouseID");
		    }
            set 
		    {
			    SetColumnValue("WarehouseID", value);
            }
        }
	      
        [XmlAttribute("IsAutoStockIn")]
        [Bindable(true)]
        public bool IsAutoStockIn 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsAutoStockIn");
		    }
            set 
		    {
			    SetColumnValue("IsAutoStockIn", value);
            }
        }
	      
        [XmlAttribute("OrderFromName")]
        [Bindable(true)]
        public string OrderFromName 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderFromName");
		    }
            set 
		    {
			    SetColumnValue("OrderFromName", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string PurchaseOrderHeaderRefNo = @"PurchaseOrderHeaderRefNo";
            
            public static string RequestedBy = @"RequestedBy";
            
            public static string PurchaseOrderDate = @"PurchaseOrderDate";
            
            public static string DepartmentID = @"DepartmentID";
            
            public static string PaymentTermID = @"PaymentTermID";
            
            public static string ShipVia = @"ShipVia";
            
            public static string ShipTo = @"ShipTo";
            
            public static string DateNeededBy = @"DateNeededBy";
            
            public static string SupplierID = @"SupplierID";
            
            public static string UserName = @"UserName";
            
            public static string Remark = @"Remark";
            
            public static string InventoryLocationID = @"InventoryLocationID";
            
            public static string InventoryLocationName = @"InventoryLocationName";
            
            public static string CreatedBy = @"CreatedBy";
            
            public static string CreatedOn = @"CreatedOn";
            
            public static string ModifiedBy = @"ModifiedBy";
            
            public static string ModifiedOn = @"ModifiedOn";
            
            public static string Status = @"Status";
            
            public static string POType = @"POType";
            
            public static string ApprovalDate = @"ApprovalDate";
            
            public static string ApprovedBy = @"ApprovedBy";
            
            public static string SpecialValidFrom = @"SpecialValidFrom";
            
            public static string SpecialValidTo = @"SpecialValidTo";
            
            public static string ApprovalStatus = @"ApprovalStatus";
            
            public static string SalesPersonID = @"SalesPersonID";
            
            public static string PriceLevel = @"PriceLevel";
            
            public static string ReasonID = @"ReasonID";
            
            public static string DestInventoryLocationID = @"DestInventoryLocationID";
            
            public static string WarehouseID = @"WarehouseID";
            
            public static string IsAutoStockIn = @"IsAutoStockIn";
            
            public static string OrderFromName = @"OrderFromName";
            
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
