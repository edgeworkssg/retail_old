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
    /// Strongly-typed collection for the ViewPreOrderSchedule class.
    /// </summary>
    [Serializable]
    public partial class ViewPreOrderScheduleCollection : ReadOnlyList<ViewPreOrderSchedule, ViewPreOrderScheduleCollection>
    {        
        public ViewPreOrderScheduleCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewPreOrderSchedule view.
    /// </summary>
    [Serializable]
    public partial class ViewPreOrderSchedule : ReadOnlyRecord<ViewPreOrderSchedule>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewPreOrderSchedule", TableType.View, DataService.GetInstance("PowerPOS"));
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
                
                TableSchema.TableColumn colvarValidFrom = new TableSchema.TableColumn(schema);
                colvarValidFrom.ColumnName = "ValidFrom";
                colvarValidFrom.DataType = DbType.DateTime;
                colvarValidFrom.MaxLength = 0;
                colvarValidFrom.AutoIncrement = false;
                colvarValidFrom.IsNullable = true;
                colvarValidFrom.IsPrimaryKey = false;
                colvarValidFrom.IsForeignKey = false;
                colvarValidFrom.IsReadOnly = false;
                
                schema.Columns.Add(colvarValidFrom);
                
                TableSchema.TableColumn colvarValidTo = new TableSchema.TableColumn(schema);
                colvarValidTo.ColumnName = "ValidTo";
                colvarValidTo.DataType = DbType.DateTime;
                colvarValidTo.MaxLength = 0;
                colvarValidTo.AutoIncrement = false;
                colvarValidTo.IsNullable = true;
                colvarValidTo.IsPrimaryKey = false;
                colvarValidTo.IsForeignKey = false;
                colvarValidTo.IsReadOnly = false;
                
                schema.Columns.Add(colvarValidTo);
                
                TableSchema.TableColumn colvarPreOrderID = new TableSchema.TableColumn(schema);
                colvarPreOrderID.ColumnName = "PreOrderID";
                colvarPreOrderID.DataType = DbType.Int32;
                colvarPreOrderID.MaxLength = 0;
                colvarPreOrderID.AutoIncrement = false;
                colvarPreOrderID.IsNullable = false;
                colvarPreOrderID.IsPrimaryKey = false;
                colvarPreOrderID.IsForeignKey = false;
                colvarPreOrderID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPreOrderID);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewPreOrderSchedule",schema);
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
	    public ViewPreOrderSchedule()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewPreOrderSchedule(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewPreOrderSchedule(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewPreOrderSchedule(string columnName, object columnValue)
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
	      
        [XmlAttribute("ValidFrom")]
        [Bindable(true)]
        public DateTime? ValidFrom 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ValidFrom");
		    }
            set 
		    {
			    SetColumnValue("ValidFrom", value);
            }
        }
	      
        [XmlAttribute("ValidTo")]
        [Bindable(true)]
        public DateTime? ValidTo 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ValidTo");
		    }
            set 
		    {
			    SetColumnValue("ValidTo", value);
            }
        }
	      
        [XmlAttribute("PreOrderID")]
        [Bindable(true)]
        public int PreOrderID 
	    {
		    get
		    {
			    return GetColumnValue<int>("PreOrderID");
		    }
            set 
		    {
			    SetColumnValue("PreOrderID", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string ItemNo = @"ItemNo";
            
            public static string ItemName = @"ItemName";
            
            public static string Deleted = @"Deleted";
            
            public static string CategoryName = @"CategoryName";
            
            public static string ValidFrom = @"ValidFrom";
            
            public static string ValidTo = @"ValidTo";
            
            public static string PreOrderID = @"PreOrderID";
            
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
