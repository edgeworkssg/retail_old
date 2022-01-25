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
    /// Strongly-typed collection for the ViewCategory class.
    /// </summary>
    [Serializable]
    public partial class ViewCategoryCollection : ReadOnlyList<ViewCategory, ViewCategoryCollection>
    {        
        public ViewCategoryCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewCategory view.
    /// </summary>
    [Serializable]
    public partial class ViewCategory : ReadOnlyRecord<ViewCategory>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewCategory", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
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
                
                TableSchema.TableColumn colvarRemarks = new TableSchema.TableColumn(schema);
                colvarRemarks.ColumnName = "Remarks";
                colvarRemarks.DataType = DbType.AnsiString;
                colvarRemarks.MaxLength = 250;
                colvarRemarks.AutoIncrement = false;
                colvarRemarks.IsNullable = true;
                colvarRemarks.IsPrimaryKey = false;
                colvarRemarks.IsForeignKey = false;
                colvarRemarks.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemarks);
                
                TableSchema.TableColumn colvarCategoryId = new TableSchema.TableColumn(schema);
                colvarCategoryId.ColumnName = "Category_ID";
                colvarCategoryId.DataType = DbType.AnsiString;
                colvarCategoryId.MaxLength = 50;
                colvarCategoryId.AutoIncrement = false;
                colvarCategoryId.IsNullable = true;
                colvarCategoryId.IsPrimaryKey = false;
                colvarCategoryId.IsForeignKey = false;
                colvarCategoryId.IsReadOnly = false;
                
                schema.Columns.Add(colvarCategoryId);
                
                TableSchema.TableColumn colvarIsDiscountable = new TableSchema.TableColumn(schema);
                colvarIsDiscountable.ColumnName = "IsDiscountable";
                colvarIsDiscountable.DataType = DbType.Boolean;
                colvarIsDiscountable.MaxLength = 0;
                colvarIsDiscountable.AutoIncrement = false;
                colvarIsDiscountable.IsNullable = false;
                colvarIsDiscountable.IsPrimaryKey = false;
                colvarIsDiscountable.IsForeignKey = false;
                colvarIsDiscountable.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsDiscountable);
                
                TableSchema.TableColumn colvarIsForSale = new TableSchema.TableColumn(schema);
                colvarIsForSale.ColumnName = "IsForSale";
                colvarIsForSale.DataType = DbType.Boolean;
                colvarIsForSale.MaxLength = 0;
                colvarIsForSale.AutoIncrement = false;
                colvarIsForSale.IsNullable = false;
                colvarIsForSale.IsPrimaryKey = false;
                colvarIsForSale.IsForeignKey = false;
                colvarIsForSale.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsForSale);
                
                TableSchema.TableColumn colvarIsGST = new TableSchema.TableColumn(schema);
                colvarIsGST.ColumnName = "IsGST";
                colvarIsGST.DataType = DbType.Boolean;
                colvarIsGST.MaxLength = 0;
                colvarIsGST.AutoIncrement = false;
                colvarIsGST.IsNullable = false;
                colvarIsGST.IsPrimaryKey = false;
                colvarIsGST.IsForeignKey = false;
                colvarIsGST.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsGST);
                
                TableSchema.TableColumn colvarAccountCategory = new TableSchema.TableColumn(schema);
                colvarAccountCategory.ColumnName = "AccountCategory";
                colvarAccountCategory.DataType = DbType.AnsiString;
                colvarAccountCategory.MaxLength = 250;
                colvarAccountCategory.AutoIncrement = false;
                colvarAccountCategory.IsNullable = true;
                colvarAccountCategory.IsPrimaryKey = false;
                colvarAccountCategory.IsForeignKey = false;
                colvarAccountCategory.IsReadOnly = false;
                
                schema.Columns.Add(colvarAccountCategory);
                
                TableSchema.TableColumn colvarDepartmentName = new TableSchema.TableColumn(schema);
                colvarDepartmentName.ColumnName = "DepartmentName";
                colvarDepartmentName.DataType = DbType.String;
                colvarDepartmentName.MaxLength = 50;
                colvarDepartmentName.AutoIncrement = false;
                colvarDepartmentName.IsNullable = false;
                colvarDepartmentName.IsPrimaryKey = false;
                colvarDepartmentName.IsForeignKey = false;
                colvarDepartmentName.IsReadOnly = false;
                
                schema.Columns.Add(colvarDepartmentName);
                
                TableSchema.TableColumn colvarItemDepartmentID = new TableSchema.TableColumn(schema);
                colvarItemDepartmentID.ColumnName = "ItemDepartmentID";
                colvarItemDepartmentID.DataType = DbType.AnsiString;
                colvarItemDepartmentID.MaxLength = 50;
                colvarItemDepartmentID.AutoIncrement = false;
                colvarItemDepartmentID.IsNullable = false;
                colvarItemDepartmentID.IsPrimaryKey = false;
                colvarItemDepartmentID.IsForeignKey = false;
                colvarItemDepartmentID.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemDepartmentID);
                
                TableSchema.TableColumn colvarItemRemark = new TableSchema.TableColumn(schema);
                colvarItemRemark.ColumnName = "ItemRemark";
                colvarItemRemark.DataType = DbType.String;
                colvarItemRemark.MaxLength = -1;
                colvarItemRemark.AutoIncrement = false;
                colvarItemRemark.IsNullable = true;
                colvarItemRemark.IsPrimaryKey = false;
                colvarItemRemark.IsForeignKey = false;
                colvarItemRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemRemark);
                
                TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
                colvarDeleted.ColumnName = "Deleted";
                colvarDeleted.DataType = DbType.Boolean;
                colvarDeleted.MaxLength = 0;
                colvarDeleted.AutoIncrement = false;
                colvarDeleted.IsNullable = true;
                colvarDeleted.IsPrimaryKey = false;
                colvarDeleted.IsForeignKey = false;
                colvarDeleted.IsReadOnly = false;
                
                schema.Columns.Add(colvarDeleted);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewCategory",schema);
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
	    public ViewCategory()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewCategory(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewCategory(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewCategory(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
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
	      
        [XmlAttribute("Remarks")]
        [Bindable(true)]
        public string Remarks 
	    {
		    get
		    {
			    return GetColumnValue<string>("Remarks");
		    }
            set 
		    {
			    SetColumnValue("Remarks", value);
            }
        }
	      
        [XmlAttribute("CategoryId")]
        [Bindable(true)]
        public string CategoryId 
	    {
		    get
		    {
			    return GetColumnValue<string>("Category_ID");
		    }
            set 
		    {
			    SetColumnValue("Category_ID", value);
            }
        }
	      
        [XmlAttribute("IsDiscountable")]
        [Bindable(true)]
        public bool IsDiscountable 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsDiscountable");
		    }
            set 
		    {
			    SetColumnValue("IsDiscountable", value);
            }
        }
	      
        [XmlAttribute("IsForSale")]
        [Bindable(true)]
        public bool IsForSale 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsForSale");
		    }
            set 
		    {
			    SetColumnValue("IsForSale", value);
            }
        }
	      
        [XmlAttribute("IsGST")]
        [Bindable(true)]
        public bool IsGST 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsGST");
		    }
            set 
		    {
			    SetColumnValue("IsGST", value);
            }
        }
	      
        [XmlAttribute("AccountCategory")]
        [Bindable(true)]
        public string AccountCategory 
	    {
		    get
		    {
			    return GetColumnValue<string>("AccountCategory");
		    }
            set 
		    {
			    SetColumnValue("AccountCategory", value);
            }
        }
	      
        [XmlAttribute("DepartmentName")]
        [Bindable(true)]
        public string DepartmentName 
	    {
		    get
		    {
			    return GetColumnValue<string>("DepartmentName");
		    }
            set 
		    {
			    SetColumnValue("DepartmentName", value);
            }
        }
	      
        [XmlAttribute("ItemDepartmentID")]
        [Bindable(true)]
        public string ItemDepartmentID 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemDepartmentID");
		    }
            set 
		    {
			    SetColumnValue("ItemDepartmentID", value);
            }
        }
	      
        [XmlAttribute("ItemRemark")]
        [Bindable(true)]
        public string ItemRemark 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemRemark");
		    }
            set 
		    {
			    SetColumnValue("ItemRemark", value);
            }
        }
	      
        [XmlAttribute("Deleted")]
        [Bindable(true)]
        public bool? Deleted 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("Deleted");
		    }
            set 
		    {
			    SetColumnValue("Deleted", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string CategoryName = @"CategoryName";
            
            public static string Remarks = @"Remarks";
            
            public static string CategoryId = @"Category_ID";
            
            public static string IsDiscountable = @"IsDiscountable";
            
            public static string IsForSale = @"IsForSale";
            
            public static string IsGST = @"IsGST";
            
            public static string AccountCategory = @"AccountCategory";
            
            public static string DepartmentName = @"DepartmentName";
            
            public static string ItemDepartmentID = @"ItemDepartmentID";
            
            public static string ItemRemark = @"ItemRemark";
            
            public static string Deleted = @"Deleted";
            
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
