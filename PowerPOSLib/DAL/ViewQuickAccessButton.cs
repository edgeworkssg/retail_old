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
    /// Strongly-typed collection for the ViewQuickAccessButton class.
    /// </summary>
    [Serializable]
    public partial class ViewQuickAccessButtonCollection : ReadOnlyList<ViewQuickAccessButton, ViewQuickAccessButtonCollection>
    {        
        public ViewQuickAccessButtonCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewQuickAccessButtons view.
    /// </summary>
    [Serializable]
    public partial class ViewQuickAccessButton : ReadOnlyRecord<ViewQuickAccessButton>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewQuickAccessButtons", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarQuickAccessGroupName = new TableSchema.TableColumn(schema);
                colvarQuickAccessGroupName.ColumnName = "QuickAccessGroupName";
                colvarQuickAccessGroupName.DataType = DbType.AnsiString;
                colvarQuickAccessGroupName.MaxLength = 250;
                colvarQuickAccessGroupName.AutoIncrement = false;
                colvarQuickAccessGroupName.IsNullable = true;
                colvarQuickAccessGroupName.IsPrimaryKey = false;
                colvarQuickAccessGroupName.IsForeignKey = false;
                colvarQuickAccessGroupName.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuickAccessGroupName);
                
                TableSchema.TableColumn colvarQuickAccessCatName = new TableSchema.TableColumn(schema);
                colvarQuickAccessCatName.ColumnName = "QuickAccessCatName";
                colvarQuickAccessCatName.DataType = DbType.String;
                colvarQuickAccessCatName.MaxLength = 50;
                colvarQuickAccessCatName.AutoIncrement = false;
                colvarQuickAccessCatName.IsNullable = false;
                colvarQuickAccessCatName.IsPrimaryKey = false;
                colvarQuickAccessCatName.IsForeignKey = false;
                colvarQuickAccessCatName.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuickAccessCatName);
                
                TableSchema.TableColumn colvarQuickAccessGroupID = new TableSchema.TableColumn(schema);
                colvarQuickAccessGroupID.ColumnName = "QuickAccessGroupID";
                colvarQuickAccessGroupID.DataType = DbType.Guid;
                colvarQuickAccessGroupID.MaxLength = 0;
                colvarQuickAccessGroupID.AutoIncrement = false;
                colvarQuickAccessGroupID.IsNullable = false;
                colvarQuickAccessGroupID.IsPrimaryKey = false;
                colvarQuickAccessGroupID.IsForeignKey = false;
                colvarQuickAccessGroupID.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuickAccessGroupID);
                
                TableSchema.TableColumn colvarQuickAccessCategoryID = new TableSchema.TableColumn(schema);
                colvarQuickAccessCategoryID.ColumnName = "QuickAccessCategoryID";
                colvarQuickAccessCategoryID.DataType = DbType.Guid;
                colvarQuickAccessCategoryID.MaxLength = 0;
                colvarQuickAccessCategoryID.AutoIncrement = false;
                colvarQuickAccessCategoryID.IsNullable = false;
                colvarQuickAccessCategoryID.IsPrimaryKey = false;
                colvarQuickAccessCategoryID.IsForeignKey = false;
                colvarQuickAccessCategoryID.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuickAccessCategoryID);
                
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
                
                TableSchema.TableColumn colvarRow = new TableSchema.TableColumn(schema);
                colvarRow.ColumnName = "row";
                colvarRow.DataType = DbType.Int32;
                colvarRow.MaxLength = 0;
                colvarRow.AutoIncrement = false;
                colvarRow.IsNullable = false;
                colvarRow.IsPrimaryKey = false;
                colvarRow.IsForeignKey = false;
                colvarRow.IsReadOnly = false;
                
                schema.Columns.Add(colvarRow);
                
                TableSchema.TableColumn colvarCol = new TableSchema.TableColumn(schema);
                colvarCol.ColumnName = "col";
                colvarCol.DataType = DbType.Int32;
                colvarCol.MaxLength = 0;
                colvarCol.AutoIncrement = false;
                colvarCol.IsNullable = false;
                colvarCol.IsPrimaryKey = false;
                colvarCol.IsForeignKey = false;
                colvarCol.IsReadOnly = false;
                
                schema.Columns.Add(colvarCol);
                
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
                
                TableSchema.TableColumn colvarForeColor = new TableSchema.TableColumn(schema);
                colvarForeColor.ColumnName = "ForeColor";
                colvarForeColor.DataType = DbType.AnsiString;
                colvarForeColor.MaxLength = 50;
                colvarForeColor.AutoIncrement = false;
                colvarForeColor.IsNullable = true;
                colvarForeColor.IsPrimaryKey = false;
                colvarForeColor.IsForeignKey = false;
                colvarForeColor.IsReadOnly = false;
                
                schema.Columns.Add(colvarForeColor);
                
                TableSchema.TableColumn colvarBackColor = new TableSchema.TableColumn(schema);
                colvarBackColor.ColumnName = "BackColor";
                colvarBackColor.DataType = DbType.AnsiString;
                colvarBackColor.MaxLength = 50;
                colvarBackColor.AutoIncrement = false;
                colvarBackColor.IsNullable = true;
                colvarBackColor.IsPrimaryKey = false;
                colvarBackColor.IsForeignKey = false;
                colvarBackColor.IsReadOnly = false;
                
                schema.Columns.Add(colvarBackColor);
                
                TableSchema.TableColumn colvarQuickAccessGroupMapID = new TableSchema.TableColumn(schema);
                colvarQuickAccessGroupMapID.ColumnName = "QuickAccessGroupMapID";
                colvarQuickAccessGroupMapID.DataType = DbType.Guid;
                colvarQuickAccessGroupMapID.MaxLength = 0;
                colvarQuickAccessGroupMapID.AutoIncrement = false;
                colvarQuickAccessGroupMapID.IsNullable = false;
                colvarQuickAccessGroupMapID.IsPrimaryKey = false;
                colvarQuickAccessGroupMapID.IsForeignKey = false;
                colvarQuickAccessGroupMapID.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuickAccessGroupMapID);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewQuickAccessButtons",schema);
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
	    public ViewQuickAccessButton()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewQuickAccessButton(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewQuickAccessButton(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewQuickAccessButton(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("QuickAccessGroupName")]
        [Bindable(true)]
        public string QuickAccessGroupName 
	    {
		    get
		    {
			    return GetColumnValue<string>("QuickAccessGroupName");
		    }
            set 
		    {
			    SetColumnValue("QuickAccessGroupName", value);
            }
        }
	      
        [XmlAttribute("QuickAccessCatName")]
        [Bindable(true)]
        public string QuickAccessCatName 
	    {
		    get
		    {
			    return GetColumnValue<string>("QuickAccessCatName");
		    }
            set 
		    {
			    SetColumnValue("QuickAccessCatName", value);
            }
        }
	      
        [XmlAttribute("QuickAccessGroupID")]
        [Bindable(true)]
        public Guid QuickAccessGroupID 
	    {
		    get
		    {
			    return GetColumnValue<Guid>("QuickAccessGroupID");
		    }
            set 
		    {
			    SetColumnValue("QuickAccessGroupID", value);
            }
        }
	      
        [XmlAttribute("QuickAccessCategoryID")]
        [Bindable(true)]
        public Guid QuickAccessCategoryID 
	    {
		    get
		    {
			    return GetColumnValue<Guid>("QuickAccessCategoryID");
		    }
            set 
		    {
			    SetColumnValue("QuickAccessCategoryID", value);
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
	      
        [XmlAttribute("Row")]
        [Bindable(true)]
        public int Row 
	    {
		    get
		    {
			    return GetColumnValue<int>("row");
		    }
            set 
		    {
			    SetColumnValue("row", value);
            }
        }
	      
        [XmlAttribute("Col")]
        [Bindable(true)]
        public int Col 
	    {
		    get
		    {
			    return GetColumnValue<int>("col");
		    }
            set 
		    {
			    SetColumnValue("col", value);
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
	      
        [XmlAttribute("ForeColor")]
        [Bindable(true)]
        public string ForeColor 
	    {
		    get
		    {
			    return GetColumnValue<string>("ForeColor");
		    }
            set 
		    {
			    SetColumnValue("ForeColor", value);
            }
        }
	      
        [XmlAttribute("BackColor")]
        [Bindable(true)]
        public string BackColor 
	    {
		    get
		    {
			    return GetColumnValue<string>("BackColor");
		    }
            set 
		    {
			    SetColumnValue("BackColor", value);
            }
        }
	      
        [XmlAttribute("QuickAccessGroupMapID")]
        [Bindable(true)]
        public Guid QuickAccessGroupMapID 
	    {
		    get
		    {
			    return GetColumnValue<Guid>("QuickAccessGroupMapID");
		    }
            set 
		    {
			    SetColumnValue("QuickAccessGroupMapID", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string QuickAccessGroupName = @"QuickAccessGroupName";
            
            public static string QuickAccessCatName = @"QuickAccessCatName";
            
            public static string QuickAccessGroupID = @"QuickAccessGroupID";
            
            public static string QuickAccessCategoryID = @"QuickAccessCategoryID";
            
            public static string ItemNo = @"ItemNo";
            
            public static string Row = @"row";
            
            public static string Col = @"col";
            
            public static string ItemName = @"ItemName";
            
            public static string CategoryName = @"CategoryName";
            
            public static string ForeColor = @"ForeColor";
            
            public static string BackColor = @"BackColor";
            
            public static string QuickAccessGroupMapID = @"QuickAccessGroupMapID";
            
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
