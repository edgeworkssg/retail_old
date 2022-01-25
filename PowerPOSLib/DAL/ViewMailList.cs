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
    /// Strongly-typed collection for the ViewMailList class.
    /// </summary>
    [Serializable]
    public partial class ViewMailListCollection : ReadOnlyList<ViewMailList, ViewMailListCollection>
    {        
        public ViewMailListCollection() {}

    }

    /// <summary>
    /// This is  Read-only wrapper class for the ViewMailList view.
    /// </summary>
    [Serializable]
    public partial class ViewMailList : ReadOnlyRecord<ViewMailList> 
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
                TableSchema.Table schema = new TableSchema.Table("ViewMailList", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = "dbo";
                //columns
                
                TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
                colvarMembershipNo.ColumnName = "MembershipNo";
                colvarMembershipNo.DataType = DbType.String;
                colvarMembershipNo.MaxLength = 200;
                colvarMembershipNo.AutoIncrement = false;
                colvarMembershipNo.IsNullable = false;
                colvarMembershipNo.IsPrimaryKey = false;
                colvarMembershipNo.IsForeignKey = false;
                colvarMembershipNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarMembershipNo);
                
                TableSchema.TableColumn colvarFirstName = new TableSchema.TableColumn(schema);
                colvarFirstName.ColumnName = "FirstName";
                colvarFirstName.DataType = DbType.String;
                colvarFirstName.MaxLength = 200;
                colvarFirstName.AutoIncrement = false;
                colvarFirstName.IsNullable = true;
                colvarFirstName.IsPrimaryKey = false;
                colvarFirstName.IsForeignKey = false;
                colvarFirstName.IsReadOnly = false;
                
                schema.Columns.Add(colvarFirstName);
                
                TableSchema.TableColumn colvarTotalPoints = new TableSchema.TableColumn(schema);
                colvarTotalPoints.ColumnName = "TotalPoints";
                colvarTotalPoints.DataType = DbType.Currency;
                colvarTotalPoints.MaxLength = 0;
                colvarTotalPoints.AutoIncrement = false;
                colvarTotalPoints.IsNullable = true;
                colvarTotalPoints.IsPrimaryKey = false;
                colvarTotalPoints.IsForeignKey = false;
                colvarTotalPoints.IsReadOnly = false;
                
                schema.Columns.Add(colvarTotalPoints);
                
                TableSchema.TableColumn colvarTotalAmount = new TableSchema.TableColumn(schema);
                colvarTotalAmount.ColumnName = "TotalAmount";
                colvarTotalAmount.DataType = DbType.Currency;
                colvarTotalAmount.MaxLength = 0;
                colvarTotalAmount.AutoIncrement = false;
                colvarTotalAmount.IsNullable = true;
                colvarTotalAmount.IsPrimaryKey = false;
                colvarTotalAmount.IsForeignKey = false;
                colvarTotalAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarTotalAmount);
                
                TableSchema.TableColumn colvarStreetName = new TableSchema.TableColumn(schema);
                colvarStreetName.ColumnName = "StreetName";
                colvarStreetName.DataType = DbType.String;
                colvarStreetName.MaxLength = 50;
                colvarStreetName.AutoIncrement = false;
                colvarStreetName.IsNullable = true;
                colvarStreetName.IsPrimaryKey = false;
                colvarStreetName.IsForeignKey = false;
                colvarStreetName.IsReadOnly = false;
                
                schema.Columns.Add(colvarStreetName);
                
                TableSchema.TableColumn colvarZipCode = new TableSchema.TableColumn(schema);
                colvarZipCode.ColumnName = "ZipCode";
                colvarZipCode.DataType = DbType.String;
                colvarZipCode.MaxLength = 50;
                colvarZipCode.AutoIncrement = false;
                colvarZipCode.IsNullable = true;
                colvarZipCode.IsPrimaryKey = false;
                colvarZipCode.IsForeignKey = false;
                colvarZipCode.IsReadOnly = false;
                
                schema.Columns.Add(colvarZipCode);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewMailList",schema);
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
	    public ViewMailList()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }

        public ViewMailList(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}

			MarkNew();
	    }

	    
	    public ViewMailList(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }

    	 
	    public ViewMailList(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }

        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("MembershipNo")]
        public string MembershipNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("MembershipNo");
		    }

            set 
		    {
			    SetColumnValue("MembershipNo", value);
            }

        }

	      
        [XmlAttribute("FirstName")]
        public string FirstName 
	    {
		    get
		    {
			    return GetColumnValue<string>("FirstName");
		    }

            set 
		    {
			    SetColumnValue("FirstName", value);
            }

        }

	      
        [XmlAttribute("TotalPoints")]
        public decimal? TotalPoints 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("TotalPoints");
		    }

            set 
		    {
			    SetColumnValue("TotalPoints", value);
            }

        }

	      
        [XmlAttribute("TotalAmount")]
        public decimal? TotalAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("TotalAmount");
		    }

            set 
		    {
			    SetColumnValue("TotalAmount", value);
            }

        }

	      
        [XmlAttribute("StreetName")]
        public string StreetName 
	    {
		    get
		    {
			    return GetColumnValue<string>("StreetName");
		    }

            set 
		    {
			    SetColumnValue("StreetName", value);
            }

        }

	      
        [XmlAttribute("ZipCode")]
        public string ZipCode 
	    {
		    get
		    {
			    return GetColumnValue<string>("ZipCode");
		    }

            set 
		    {
			    SetColumnValue("ZipCode", value);
            }

        }

	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string MembershipNo = @"MembershipNo";
            
            public static string FirstName = @"FirstName";
            
            public static string TotalPoints = @"TotalPoints";
            
            public static string TotalAmount = @"TotalAmount";
            
            public static string StreetName = @"StreetName";
            
            public static string ZipCode = @"ZipCode";
            
	    }

	    #endregion
    }

}

