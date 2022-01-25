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
    /// Strongly-typed collection for the ViewRecipe class.
    /// </summary>
    [Serializable]
    public partial class ViewRecipeCollection : ReadOnlyList<ViewRecipe, ViewRecipeCollection>
    {        
        public ViewRecipeCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewRecipe view.
    /// </summary>
    [Serializable]
    public partial class ViewRecipe : ReadOnlyRecord<ViewRecipe>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewRecipe", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarRecipeHeaderID = new TableSchema.TableColumn(schema);
                colvarRecipeHeaderID.ColumnName = "RecipeHeaderID";
                colvarRecipeHeaderID.DataType = DbType.AnsiString;
                colvarRecipeHeaderID.MaxLength = 64;
                colvarRecipeHeaderID.AutoIncrement = false;
                colvarRecipeHeaderID.IsNullable = false;
                colvarRecipeHeaderID.IsPrimaryKey = false;
                colvarRecipeHeaderID.IsForeignKey = false;
                colvarRecipeHeaderID.IsReadOnly = false;
                
                schema.Columns.Add(colvarRecipeHeaderID);
                
                TableSchema.TableColumn colvarRecipeHeaderNo = new TableSchema.TableColumn(schema);
                colvarRecipeHeaderNo.ColumnName = "RecipeHeaderNo";
                colvarRecipeHeaderNo.DataType = DbType.AnsiString;
                colvarRecipeHeaderNo.MaxLength = 50;
                colvarRecipeHeaderNo.AutoIncrement = false;
                colvarRecipeHeaderNo.IsNullable = false;
                colvarRecipeHeaderNo.IsPrimaryKey = false;
                colvarRecipeHeaderNo.IsForeignKey = false;
                colvarRecipeHeaderNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarRecipeHeaderNo);
                
                TableSchema.TableColumn colvarRecipeName = new TableSchema.TableColumn(schema);
                colvarRecipeName.ColumnName = "RecipeName";
                colvarRecipeName.DataType = DbType.String;
                colvarRecipeName.MaxLength = 300;
                colvarRecipeName.AutoIncrement = false;
                colvarRecipeName.IsNullable = false;
                colvarRecipeName.IsPrimaryKey = false;
                colvarRecipeName.IsForeignKey = false;
                colvarRecipeName.IsReadOnly = false;
                
                schema.Columns.Add(colvarRecipeName);
                
                TableSchema.TableColumn colvarRecipeDetailID = new TableSchema.TableColumn(schema);
                colvarRecipeDetailID.ColumnName = "RecipeDetailID";
                colvarRecipeDetailID.DataType = DbType.AnsiString;
                colvarRecipeDetailID.MaxLength = 64;
                colvarRecipeDetailID.AutoIncrement = false;
                colvarRecipeDetailID.IsNullable = false;
                colvarRecipeDetailID.IsPrimaryKey = false;
                colvarRecipeDetailID.IsForeignKey = false;
                colvarRecipeDetailID.IsReadOnly = false;
                
                schema.Columns.Add(colvarRecipeDetailID);
                
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
                
                TableSchema.TableColumn colvarQty = new TableSchema.TableColumn(schema);
                colvarQty.ColumnName = "Qty";
                colvarQty.DataType = DbType.Decimal;
                colvarQty.MaxLength = 0;
                colvarQty.AutoIncrement = false;
                colvarQty.IsNullable = false;
                colvarQty.IsPrimaryKey = false;
                colvarQty.IsForeignKey = false;
                colvarQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarQty);
                
                TableSchema.TableColumn colvarIsPacking = new TableSchema.TableColumn(schema);
                colvarIsPacking.ColumnName = "IsPacking";
                colvarIsPacking.DataType = DbType.Boolean;
                colvarIsPacking.MaxLength = 0;
                colvarIsPacking.AutoIncrement = false;
                colvarIsPacking.IsNullable = false;
                colvarIsPacking.IsPrimaryKey = false;
                colvarIsPacking.IsForeignKey = false;
                colvarIsPacking.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsPacking);
                
                TableSchema.TableColumn colvarUom = new TableSchema.TableColumn(schema);
                colvarUom.ColumnName = "UOM";
                colvarUom.DataType = DbType.String;
                colvarUom.MaxLength = 50;
                colvarUom.AutoIncrement = false;
                colvarUom.IsNullable = true;
                colvarUom.IsPrimaryKey = false;
                colvarUom.IsForeignKey = false;
                colvarUom.IsReadOnly = false;
                
                schema.Columns.Add(colvarUom);
                
                TableSchema.TableColumn colvarMaterialUOM = new TableSchema.TableColumn(schema);
                colvarMaterialUOM.ColumnName = "MaterialUOM";
                colvarMaterialUOM.DataType = DbType.AnsiString;
                colvarMaterialUOM.MaxLength = 50;
                colvarMaterialUOM.AutoIncrement = false;
                colvarMaterialUOM.IsNullable = false;
                colvarMaterialUOM.IsPrimaryKey = false;
                colvarMaterialUOM.IsForeignKey = false;
                colvarMaterialUOM.IsReadOnly = false;
                
                schema.Columns.Add(colvarMaterialUOM);
                
                TableSchema.TableColumn colvarConversionRate = new TableSchema.TableColumn(schema);
                colvarConversionRate.ColumnName = "ConversionRate";
                colvarConversionRate.DataType = DbType.Decimal;
                colvarConversionRate.MaxLength = 0;
                colvarConversionRate.AutoIncrement = false;
                colvarConversionRate.IsNullable = true;
                colvarConversionRate.IsPrimaryKey = false;
                colvarConversionRate.IsForeignKey = false;
                colvarConversionRate.IsReadOnly = false;
                
                schema.Columns.Add(colvarConversionRate);
                
                TableSchema.TableColumn colvarConversionDetID = new TableSchema.TableColumn(schema);
                colvarConversionDetID.ColumnName = "ConversionDetID";
                colvarConversionDetID.DataType = DbType.Int32;
                colvarConversionDetID.MaxLength = 0;
                colvarConversionDetID.AutoIncrement = false;
                colvarConversionDetID.IsNullable = true;
                colvarConversionDetID.IsPrimaryKey = false;
                colvarConversionDetID.IsForeignKey = false;
                colvarConversionDetID.IsReadOnly = false;
                
                schema.Columns.Add(colvarConversionDetID);
                
                TableSchema.TableColumn colvarSearch = new TableSchema.TableColumn(schema);
                colvarSearch.ColumnName = "Search";
                colvarSearch.DataType = DbType.String;
                colvarSearch.MaxLength = 553;
                colvarSearch.AutoIncrement = false;
                colvarSearch.IsNullable = false;
                colvarSearch.IsPrimaryKey = false;
                colvarSearch.IsForeignKey = false;
                colvarSearch.IsReadOnly = false;
                
                schema.Columns.Add(colvarSearch);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewRecipe",schema);
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
	    public ViewRecipe()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewRecipe(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewRecipe(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewRecipe(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("RecipeHeaderID")]
        [Bindable(true)]
        public string RecipeHeaderID 
	    {
		    get
		    {
			    return GetColumnValue<string>("RecipeHeaderID");
		    }
            set 
		    {
			    SetColumnValue("RecipeHeaderID", value);
            }
        }
	      
        [XmlAttribute("RecipeHeaderNo")]
        [Bindable(true)]
        public string RecipeHeaderNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("RecipeHeaderNo");
		    }
            set 
		    {
			    SetColumnValue("RecipeHeaderNo", value);
            }
        }
	      
        [XmlAttribute("RecipeName")]
        [Bindable(true)]
        public string RecipeName 
	    {
		    get
		    {
			    return GetColumnValue<string>("RecipeName");
		    }
            set 
		    {
			    SetColumnValue("RecipeName", value);
            }
        }
	      
        [XmlAttribute("RecipeDetailID")]
        [Bindable(true)]
        public string RecipeDetailID 
	    {
		    get
		    {
			    return GetColumnValue<string>("RecipeDetailID");
		    }
            set 
		    {
			    SetColumnValue("RecipeDetailID", value);
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
	      
        [XmlAttribute("Qty")]
        [Bindable(true)]
        public decimal Qty 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("Qty");
		    }
            set 
		    {
			    SetColumnValue("Qty", value);
            }
        }
	      
        [XmlAttribute("IsPacking")]
        [Bindable(true)]
        public bool IsPacking 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsPacking");
		    }
            set 
		    {
			    SetColumnValue("IsPacking", value);
            }
        }
	      
        [XmlAttribute("Uom")]
        [Bindable(true)]
        public string Uom 
	    {
		    get
		    {
			    return GetColumnValue<string>("UOM");
		    }
            set 
		    {
			    SetColumnValue("UOM", value);
            }
        }
	      
        [XmlAttribute("MaterialUOM")]
        [Bindable(true)]
        public string MaterialUOM 
	    {
		    get
		    {
			    return GetColumnValue<string>("MaterialUOM");
		    }
            set 
		    {
			    SetColumnValue("MaterialUOM", value);
            }
        }
	      
        [XmlAttribute("ConversionRate")]
        [Bindable(true)]
        public decimal? ConversionRate 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("ConversionRate");
		    }
            set 
		    {
			    SetColumnValue("ConversionRate", value);
            }
        }
	      
        [XmlAttribute("ConversionDetID")]
        [Bindable(true)]
        public int? ConversionDetID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("ConversionDetID");
		    }
            set 
		    {
			    SetColumnValue("ConversionDetID", value);
            }
        }
	      
        [XmlAttribute("Search")]
        [Bindable(true)]
        public string Search 
	    {
		    get
		    {
			    return GetColumnValue<string>("Search");
		    }
            set 
		    {
			    SetColumnValue("Search", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string RecipeHeaderID = @"RecipeHeaderID";
            
            public static string RecipeHeaderNo = @"RecipeHeaderNo";
            
            public static string RecipeName = @"RecipeName";
            
            public static string RecipeDetailID = @"RecipeDetailID";
            
            public static string ItemNo = @"ItemNo";
            
            public static string ItemName = @"ItemName";
            
            public static string Qty = @"Qty";
            
            public static string IsPacking = @"IsPacking";
            
            public static string Uom = @"UOM";
            
            public static string MaterialUOM = @"MaterialUOM";
            
            public static string ConversionRate = @"ConversionRate";
            
            public static string ConversionDetID = @"ConversionDetID";
            
            public static string Search = @"Search";
            
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
