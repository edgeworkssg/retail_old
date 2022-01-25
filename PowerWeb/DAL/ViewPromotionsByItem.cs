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
    /// Strongly-typed collection for the ViewPromotionsByItem class.
    /// </summary>
    [Serializable]
    public partial class ViewPromotionsByItemCollection : ReadOnlyList<ViewPromotionsByItem, ViewPromotionsByItemCollection>
    {        
        public ViewPromotionsByItemCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewPromotionsByItem view.
    /// </summary>
    [Serializable]
    public partial class ViewPromotionsByItem : ReadOnlyRecord<ViewPromotionsByItem>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewPromotionsByItem", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarPromoCampaignHdrID = new TableSchema.TableColumn(schema);
                colvarPromoCampaignHdrID.ColumnName = "PromoCampaignHdrID";
                colvarPromoCampaignHdrID.DataType = DbType.Int32;
                colvarPromoCampaignHdrID.MaxLength = 0;
                colvarPromoCampaignHdrID.AutoIncrement = false;
                colvarPromoCampaignHdrID.IsNullable = false;
                colvarPromoCampaignHdrID.IsPrimaryKey = false;
                colvarPromoCampaignHdrID.IsForeignKey = false;
                colvarPromoCampaignHdrID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoCampaignHdrID);
                
                TableSchema.TableColumn colvarPromoCampaignDetID = new TableSchema.TableColumn(schema);
                colvarPromoCampaignDetID.ColumnName = "PromoCampaignDetID";
                colvarPromoCampaignDetID.DataType = DbType.Int32;
                colvarPromoCampaignDetID.MaxLength = 0;
                colvarPromoCampaignDetID.AutoIncrement = false;
                colvarPromoCampaignDetID.IsNullable = false;
                colvarPromoCampaignDetID.IsPrimaryKey = false;
                colvarPromoCampaignDetID.IsForeignKey = false;
                colvarPromoCampaignDetID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoCampaignDetID);
                
                TableSchema.TableColumn colvarItemGroupID = new TableSchema.TableColumn(schema);
                colvarItemGroupID.ColumnName = "ItemGroupID";
                colvarItemGroupID.DataType = DbType.Int32;
                colvarItemGroupID.MaxLength = 0;
                colvarItemGroupID.AutoIncrement = false;
                colvarItemGroupID.IsNullable = true;
                colvarItemGroupID.IsPrimaryKey = false;
                colvarItemGroupID.IsForeignKey = false;
                colvarItemGroupID.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemGroupID);
                
                TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
                colvarItemNo.ColumnName = "ItemNo";
                colvarItemNo.DataType = DbType.AnsiString;
                colvarItemNo.MaxLength = 50;
                colvarItemNo.AutoIncrement = false;
                colvarItemNo.IsNullable = true;
                colvarItemNo.IsPrimaryKey = false;
                colvarItemNo.IsForeignKey = false;
                colvarItemNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemNo);
                
                TableSchema.TableColumn colvarCategoryName = new TableSchema.TableColumn(schema);
                colvarCategoryName.ColumnName = "CategoryName";
                colvarCategoryName.DataType = DbType.String;
                colvarCategoryName.MaxLength = 250;
                colvarCategoryName.AutoIncrement = false;
                colvarCategoryName.IsNullable = true;
                colvarCategoryName.IsPrimaryKey = false;
                colvarCategoryName.IsForeignKey = false;
                colvarCategoryName.IsReadOnly = false;
                
                schema.Columns.Add(colvarCategoryName);
                
                TableSchema.TableColumn colvarFromQuantity = new TableSchema.TableColumn(schema);
                colvarFromQuantity.ColumnName = "FromQuantity";
                colvarFromQuantity.DataType = DbType.Int32;
                colvarFromQuantity.MaxLength = 0;
                colvarFromQuantity.AutoIncrement = false;
                colvarFromQuantity.IsNullable = true;
                colvarFromQuantity.IsPrimaryKey = false;
                colvarFromQuantity.IsForeignKey = false;
                colvarFromQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarFromQuantity);
                
                TableSchema.TableColumn colvarToQuantity = new TableSchema.TableColumn(schema);
                colvarToQuantity.ColumnName = "ToQuantity";
                colvarToQuantity.DataType = DbType.Int32;
                colvarToQuantity.MaxLength = 0;
                colvarToQuantity.AutoIncrement = false;
                colvarToQuantity.IsNullable = true;
                colvarToQuantity.IsPrimaryKey = false;
                colvarToQuantity.IsForeignKey = false;
                colvarToQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarToQuantity);
                
                TableSchema.TableColumn colvarMinQuantity = new TableSchema.TableColumn(schema);
                colvarMinQuantity.ColumnName = "MinQuantity";
                colvarMinQuantity.DataType = DbType.Int32;
                colvarMinQuantity.MaxLength = 0;
                colvarMinQuantity.AutoIncrement = false;
                colvarMinQuantity.IsNullable = false;
                colvarMinQuantity.IsPrimaryKey = false;
                colvarMinQuantity.IsForeignKey = false;
                colvarMinQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarMinQuantity);
                
                TableSchema.TableColumn colvarUnitQty = new TableSchema.TableColumn(schema);
                colvarUnitQty.ColumnName = "UnitQty";
                colvarUnitQty.DataType = DbType.Int32;
                colvarUnitQty.MaxLength = 0;
                colvarUnitQty.AutoIncrement = false;
                colvarUnitQty.IsNullable = true;
                colvarUnitQty.IsPrimaryKey = false;
                colvarUnitQty.IsForeignKey = false;
                colvarUnitQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarUnitQty);
                
                TableSchema.TableColumn colvarPromoCampaignName = new TableSchema.TableColumn(schema);
                colvarPromoCampaignName.ColumnName = "PromoCampaignName";
                colvarPromoCampaignName.DataType = DbType.AnsiString;
                colvarPromoCampaignName.MaxLength = 50;
                colvarPromoCampaignName.AutoIncrement = false;
                colvarPromoCampaignName.IsNullable = false;
                colvarPromoCampaignName.IsPrimaryKey = false;
                colvarPromoCampaignName.IsForeignKey = false;
                colvarPromoCampaignName.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoCampaignName);
                
                TableSchema.TableColumn colvarCampaignType = new TableSchema.TableColumn(schema);
                colvarCampaignType.ColumnName = "CampaignType";
                colvarCampaignType.DataType = DbType.AnsiString;
                colvarCampaignType.MaxLength = 50;
                colvarCampaignType.AutoIncrement = false;
                colvarCampaignType.IsNullable = false;
                colvarCampaignType.IsPrimaryKey = false;
                colvarCampaignType.IsForeignKey = false;
                colvarCampaignType.IsReadOnly = false;
                
                schema.Columns.Add(colvarCampaignType);
                
                TableSchema.TableColumn colvarDateFrom = new TableSchema.TableColumn(schema);
                colvarDateFrom.ColumnName = "DateFrom";
                colvarDateFrom.DataType = DbType.DateTime;
                colvarDateFrom.MaxLength = 0;
                colvarDateFrom.AutoIncrement = false;
                colvarDateFrom.IsNullable = false;
                colvarDateFrom.IsPrimaryKey = false;
                colvarDateFrom.IsForeignKey = false;
                colvarDateFrom.IsReadOnly = false;
                
                schema.Columns.Add(colvarDateFrom);
                
                TableSchema.TableColumn colvarDateTo = new TableSchema.TableColumn(schema);
                colvarDateTo.ColumnName = "DateTo";
                colvarDateTo.DataType = DbType.DateTime;
                colvarDateTo.MaxLength = 0;
                colvarDateTo.AutoIncrement = false;
                colvarDateTo.IsNullable = false;
                colvarDateTo.IsPrimaryKey = false;
                colvarDateTo.IsForeignKey = false;
                colvarDateTo.IsReadOnly = false;
                
                schema.Columns.Add(colvarDateTo);
                
                TableSchema.TableColumn colvarPromoPrice = new TableSchema.TableColumn(schema);
                colvarPromoPrice.ColumnName = "PromoPrice";
                colvarPromoPrice.DataType = DbType.Currency;
                colvarPromoPrice.MaxLength = 0;
                colvarPromoPrice.AutoIncrement = false;
                colvarPromoPrice.IsNullable = true;
                colvarPromoPrice.IsPrimaryKey = false;
                colvarPromoPrice.IsForeignKey = false;
                colvarPromoPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoPrice);
                
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
                
                TableSchema.TableColumn colvarFreeQty = new TableSchema.TableColumn(schema);
                colvarFreeQty.ColumnName = "FreeQty";
                colvarFreeQty.DataType = DbType.Int32;
                colvarFreeQty.MaxLength = 0;
                colvarFreeQty.AutoIncrement = false;
                colvarFreeQty.IsNullable = true;
                colvarFreeQty.IsPrimaryKey = false;
                colvarFreeQty.IsForeignKey = false;
                colvarFreeQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarFreeQty);
                
                TableSchema.TableColumn colvarFreeItemNo = new TableSchema.TableColumn(schema);
                colvarFreeItemNo.ColumnName = "FreeItemNo";
                colvarFreeItemNo.DataType = DbType.AnsiString;
                colvarFreeItemNo.MaxLength = 50;
                colvarFreeItemNo.AutoIncrement = false;
                colvarFreeItemNo.IsNullable = true;
                colvarFreeItemNo.IsPrimaryKey = false;
                colvarFreeItemNo.IsForeignKey = false;
                colvarFreeItemNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarFreeItemNo);
                
                TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
                colvarRemark.ColumnName = "Remark";
                colvarRemark.DataType = DbType.AnsiString;
                colvarRemark.MaxLength = 250;
                colvarRemark.AutoIncrement = false;
                colvarRemark.IsNullable = true;
                colvarRemark.IsPrimaryKey = false;
                colvarRemark.IsForeignKey = false;
                colvarRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemark);
                
                TableSchema.TableColumn colvarEnabled = new TableSchema.TableColumn(schema);
                colvarEnabled.ColumnName = "Enabled";
                colvarEnabled.DataType = DbType.Boolean;
                colvarEnabled.MaxLength = 0;
                colvarEnabled.AutoIncrement = false;
                colvarEnabled.IsNullable = false;
                colvarEnabled.IsPrimaryKey = false;
                colvarEnabled.IsForeignKey = false;
                colvarEnabled.IsReadOnly = false;
                
                schema.Columns.Add(colvarEnabled);
                
                TableSchema.TableColumn colvarForNonMembersAlso = new TableSchema.TableColumn(schema);
                colvarForNonMembersAlso.ColumnName = "ForNonMembersAlso";
                colvarForNonMembersAlso.DataType = DbType.Boolean;
                colvarForNonMembersAlso.MaxLength = 0;
                colvarForNonMembersAlso.AutoIncrement = false;
                colvarForNonMembersAlso.IsNullable = true;
                colvarForNonMembersAlso.IsPrimaryKey = false;
                colvarForNonMembersAlso.IsForeignKey = false;
                colvarForNonMembersAlso.IsReadOnly = false;
                
                schema.Columns.Add(colvarForNonMembersAlso);
                
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
                
                TableSchema.TableColumn colvarForAllLocations = new TableSchema.TableColumn(schema);
                colvarForAllLocations.ColumnName = "ForAllLocations";
                colvarForAllLocations.DataType = DbType.Boolean;
                colvarForAllLocations.MaxLength = 0;
                colvarForAllLocations.AutoIncrement = false;
                colvarForAllLocations.IsNullable = true;
                colvarForAllLocations.IsPrimaryKey = false;
                colvarForAllLocations.IsForeignKey = false;
                colvarForAllLocations.IsReadOnly = false;
                
                schema.Columns.Add(colvarForAllLocations);
                
                TableSchema.TableColumn colvarUsePrice = new TableSchema.TableColumn(schema);
                colvarUsePrice.ColumnName = "UsePrice";
                colvarUsePrice.DataType = DbType.Boolean;
                colvarUsePrice.MaxLength = 0;
                colvarUsePrice.AutoIncrement = false;
                colvarUsePrice.IsNullable = false;
                colvarUsePrice.IsPrimaryKey = false;
                colvarUsePrice.IsForeignKey = false;
                colvarUsePrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarUsePrice);
                
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
                
                TableSchema.TableColumn colvarItemCategoryName = new TableSchema.TableColumn(schema);
                colvarItemCategoryName.ColumnName = "ItemCategoryName";
                colvarItemCategoryName.DataType = DbType.String;
                colvarItemCategoryName.MaxLength = 250;
                colvarItemCategoryName.AutoIncrement = false;
                colvarItemCategoryName.IsNullable = false;
                colvarItemCategoryName.IsPrimaryKey = false;
                colvarItemCategoryName.IsForeignKey = false;
                colvarItemCategoryName.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemCategoryName);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewPromotionsByItem",schema);
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
	    public ViewPromotionsByItem()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewPromotionsByItem(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewPromotionsByItem(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewPromotionsByItem(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("PromoCampaignHdrID")]
        [Bindable(true)]
        public int PromoCampaignHdrID 
	    {
		    get
		    {
			    return GetColumnValue<int>("PromoCampaignHdrID");
		    }
            set 
		    {
			    SetColumnValue("PromoCampaignHdrID", value);
            }
        }
	      
        [XmlAttribute("PromoCampaignDetID")]
        [Bindable(true)]
        public int PromoCampaignDetID 
	    {
		    get
		    {
			    return GetColumnValue<int>("PromoCampaignDetID");
		    }
            set 
		    {
			    SetColumnValue("PromoCampaignDetID", value);
            }
        }
	      
        [XmlAttribute("ItemGroupID")]
        [Bindable(true)]
        public int? ItemGroupID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("ItemGroupID");
		    }
            set 
		    {
			    SetColumnValue("ItemGroupID", value);
            }
        }
	      
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
	      
        [XmlAttribute("FromQuantity")]
        [Bindable(true)]
        public int? FromQuantity 
	    {
		    get
		    {
			    return GetColumnValue<int?>("FromQuantity");
		    }
            set 
		    {
			    SetColumnValue("FromQuantity", value);
            }
        }
	      
        [XmlAttribute("ToQuantity")]
        [Bindable(true)]
        public int? ToQuantity 
	    {
		    get
		    {
			    return GetColumnValue<int?>("ToQuantity");
		    }
            set 
		    {
			    SetColumnValue("ToQuantity", value);
            }
        }
	      
        [XmlAttribute("MinQuantity")]
        [Bindable(true)]
        public int MinQuantity 
	    {
		    get
		    {
			    return GetColumnValue<int>("MinQuantity");
		    }
            set 
		    {
			    SetColumnValue("MinQuantity", value);
            }
        }
	      
        [XmlAttribute("UnitQty")]
        [Bindable(true)]
        public int? UnitQty 
	    {
		    get
		    {
			    return GetColumnValue<int?>("UnitQty");
		    }
            set 
		    {
			    SetColumnValue("UnitQty", value);
            }
        }
	      
        [XmlAttribute("PromoCampaignName")]
        [Bindable(true)]
        public string PromoCampaignName 
	    {
		    get
		    {
			    return GetColumnValue<string>("PromoCampaignName");
		    }
            set 
		    {
			    SetColumnValue("PromoCampaignName", value);
            }
        }
	      
        [XmlAttribute("CampaignType")]
        [Bindable(true)]
        public string CampaignType 
	    {
		    get
		    {
			    return GetColumnValue<string>("CampaignType");
		    }
            set 
		    {
			    SetColumnValue("CampaignType", value);
            }
        }
	      
        [XmlAttribute("DateFrom")]
        [Bindable(true)]
        public DateTime DateFrom 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("DateFrom");
		    }
            set 
		    {
			    SetColumnValue("DateFrom", value);
            }
        }
	      
        [XmlAttribute("DateTo")]
        [Bindable(true)]
        public DateTime DateTo 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("DateTo");
		    }
            set 
		    {
			    SetColumnValue("DateTo", value);
            }
        }
	      
        [XmlAttribute("PromoPrice")]
        [Bindable(true)]
        public decimal? PromoPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("PromoPrice");
		    }
            set 
		    {
			    SetColumnValue("PromoPrice", value);
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
	      
        [XmlAttribute("FreeQty")]
        [Bindable(true)]
        public int? FreeQty 
	    {
		    get
		    {
			    return GetColumnValue<int?>("FreeQty");
		    }
            set 
		    {
			    SetColumnValue("FreeQty", value);
            }
        }
	      
        [XmlAttribute("FreeItemNo")]
        [Bindable(true)]
        public string FreeItemNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("FreeItemNo");
		    }
            set 
		    {
			    SetColumnValue("FreeItemNo", value);
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
	      
        [XmlAttribute("Enabled")]
        [Bindable(true)]
        public bool Enabled 
	    {
		    get
		    {
			    return GetColumnValue<bool>("Enabled");
		    }
            set 
		    {
			    SetColumnValue("Enabled", value);
            }
        }
	      
        [XmlAttribute("ForNonMembersAlso")]
        [Bindable(true)]
        public bool? ForNonMembersAlso 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("ForNonMembersAlso");
		    }
            set 
		    {
			    SetColumnValue("ForNonMembersAlso", value);
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
	      
        [XmlAttribute("ForAllLocations")]
        [Bindable(true)]
        public bool? ForAllLocations 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("ForAllLocations");
		    }
            set 
		    {
			    SetColumnValue("ForAllLocations", value);
            }
        }
	      
        [XmlAttribute("UsePrice")]
        [Bindable(true)]
        public bool UsePrice 
	    {
		    get
		    {
			    return GetColumnValue<bool>("UsePrice");
		    }
            set 
		    {
			    SetColumnValue("UsePrice", value);
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
	      
        [XmlAttribute("ItemCategoryName")]
        [Bindable(true)]
        public string ItemCategoryName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemCategoryName");
		    }
            set 
		    {
			    SetColumnValue("ItemCategoryName", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string PromoCampaignHdrID = @"PromoCampaignHdrID";
            
            public static string PromoCampaignDetID = @"PromoCampaignDetID";
            
            public static string ItemGroupID = @"ItemGroupID";
            
            public static string ItemNo = @"ItemNo";
            
            public static string CategoryName = @"CategoryName";
            
            public static string FromQuantity = @"FromQuantity";
            
            public static string ToQuantity = @"ToQuantity";
            
            public static string MinQuantity = @"MinQuantity";
            
            public static string UnitQty = @"UnitQty";
            
            public static string PromoCampaignName = @"PromoCampaignName";
            
            public static string CampaignType = @"CampaignType";
            
            public static string DateFrom = @"DateFrom";
            
            public static string DateTo = @"DateTo";
            
            public static string PromoPrice = @"PromoPrice";
            
            public static string PromoDiscount = @"PromoDiscount";
            
            public static string FreeQty = @"FreeQty";
            
            public static string FreeItemNo = @"FreeItemNo";
            
            public static string Remark = @"Remark";
            
            public static string Enabled = @"Enabled";
            
            public static string ForNonMembersAlso = @"ForNonMembersAlso";
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string ForAllLocations = @"ForAllLocations";
            
            public static string UsePrice = @"UsePrice";
            
            public static string ItemName = @"ItemName";
            
            public static string ItemCategoryName = @"ItemCategoryName";
            
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
