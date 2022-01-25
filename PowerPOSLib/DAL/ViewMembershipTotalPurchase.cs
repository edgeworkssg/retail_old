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
    /// Strongly-typed collection for the ViewMembershipTotalPurchase class.
    /// </summary>
    [Serializable]
    public partial class ViewMembershipTotalPurchaseCollection : ReadOnlyList<ViewMembershipTotalPurchase, ViewMembershipTotalPurchaseCollection>
    {        
        public ViewMembershipTotalPurchaseCollection() {}

    }

    /// <summary>
    /// This is  Read-only wrapper class for the ViewMembershipTotalPurchase view.
    /// </summary>
    [Serializable]
    public partial class ViewMembershipTotalPurchase : ReadOnlyRecord<ViewMembershipTotalPurchase> 
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
                TableSchema.Table schema = new TableSchema.Table("ViewMembershipTotalPurchase", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = "dbo";
                //columns
                
                TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
                colvarMembershipNo.ColumnName = "MembershipNo";
                colvarMembershipNo.DataType = DbType.String;
                colvarMembershipNo.MaxLength = 50;
                colvarMembershipNo.AutoIncrement = false;
                colvarMembershipNo.IsNullable = false;
                colvarMembershipNo.IsPrimaryKey = false;
                colvarMembershipNo.IsForeignKey = false;
                colvarMembershipNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarMembershipNo);
                
                TableSchema.TableColumn colvarNameToAppear = new TableSchema.TableColumn(schema);
                colvarNameToAppear.ColumnName = "NameToAppear";
                colvarNameToAppear.DataType = DbType.String;
                colvarNameToAppear.MaxLength = 50;
                colvarNameToAppear.AutoIncrement = false;
                colvarNameToAppear.IsNullable = true;
                colvarNameToAppear.IsPrimaryKey = false;
                colvarNameToAppear.IsForeignKey = false;
                colvarNameToAppear.IsReadOnly = false;
                
                schema.Columns.Add(colvarNameToAppear);
                
                TableSchema.TableColumn colvarExpiryDate = new TableSchema.TableColumn(schema);
                colvarExpiryDate.ColumnName = "ExpiryDate";
                colvarExpiryDate.DataType = DbType.DateTime;
                colvarExpiryDate.MaxLength = 0;
                colvarExpiryDate.AutoIncrement = false;
                colvarExpiryDate.IsNullable = true;
                colvarExpiryDate.IsPrimaryKey = false;
                colvarExpiryDate.IsForeignKey = false;
                colvarExpiryDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpiryDate);
                
                TableSchema.TableColumn colvarGroupName = new TableSchema.TableColumn(schema);
                colvarGroupName.ColumnName = "GroupName";
                colvarGroupName.DataType = DbType.String;
                colvarGroupName.MaxLength = 50;
                colvarGroupName.AutoIncrement = false;
                colvarGroupName.IsNullable = false;
                colvarGroupName.IsPrimaryKey = false;
                colvarGroupName.IsForeignKey = false;
                colvarGroupName.IsReadOnly = false;
                
                schema.Columns.Add(colvarGroupName);
                
                TableSchema.TableColumn colvarDateOfBirth = new TableSchema.TableColumn(schema);
                colvarDateOfBirth.ColumnName = "DateOfBirth";
                colvarDateOfBirth.DataType = DbType.DateTime;
                colvarDateOfBirth.MaxLength = 0;
                colvarDateOfBirth.AutoIncrement = false;
                colvarDateOfBirth.IsNullable = true;
                colvarDateOfBirth.IsPrimaryKey = false;
                colvarDateOfBirth.IsForeignKey = false;
                colvarDateOfBirth.IsReadOnly = false;
                
                schema.Columns.Add(colvarDateOfBirth);
                
                TableSchema.TableColumn colvarPurchaseAmt = new TableSchema.TableColumn(schema);
                colvarPurchaseAmt.ColumnName = "PurchaseAmt";
                colvarPurchaseAmt.DataType = DbType.Currency;
                colvarPurchaseAmt.MaxLength = 0;
                colvarPurchaseAmt.AutoIncrement = false;
                colvarPurchaseAmt.IsNullable = true;
                colvarPurchaseAmt.IsPrimaryKey = false;
                colvarPurchaseAmt.IsForeignKey = false;
                colvarPurchaseAmt.IsReadOnly = false;
                
                schema.Columns.Add(colvarPurchaseAmt);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewMembershipTotalPurchase",schema);
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
	    public ViewMembershipTotalPurchase()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }

        public ViewMembershipTotalPurchase(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}

			MarkNew();
	    }

	    
	    public ViewMembershipTotalPurchase(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }

    	 
	    public ViewMembershipTotalPurchase(string columnName, object columnValue)
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

	      
        [XmlAttribute("NameToAppear")]
        public string NameToAppear 
	    {
		    get
		    {
			    return GetColumnValue<string>("NameToAppear");
		    }

            set 
		    {
			    SetColumnValue("NameToAppear", value);
            }

        }

	      
        [XmlAttribute("ExpiryDate")]
        public DateTime? ExpiryDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ExpiryDate");
		    }

            set 
		    {
			    SetColumnValue("ExpiryDate", value);
            }

        }

	      
        [XmlAttribute("GroupName")]
        public string GroupName 
	    {
		    get
		    {
			    return GetColumnValue<string>("GroupName");
		    }

            set 
		    {
			    SetColumnValue("GroupName", value);
            }

        }

	      
        [XmlAttribute("DateOfBirth")]
        public DateTime? DateOfBirth 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("DateOfBirth");
		    }

            set 
		    {
			    SetColumnValue("DateOfBirth", value);
            }

        }

	      
        [XmlAttribute("PurchaseAmt")]
        public decimal? PurchaseAmt 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("PurchaseAmt");
		    }

            set 
		    {
			    SetColumnValue("PurchaseAmt", value);
            }

        }

	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string MembershipNo = @"MembershipNo";
            
            public static string NameToAppear = @"NameToAppear";
            
            public static string ExpiryDate = @"ExpiryDate";
            
            public static string GroupName = @"GroupName";
            
            public static string DateOfBirth = @"DateOfBirth";
            
            public static string PurchaseAmt = @"PurchaseAmt";
            
	    }

	    #endregion
    }

}

