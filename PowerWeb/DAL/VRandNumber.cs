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
    /// Strongly-typed collection for the VRandNumber class.
    /// </summary>
    [Serializable]
    public partial class VRandNumberCollection : ReadOnlyList<VRandNumber, VRandNumberCollection>
    {        
        public VRandNumberCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the vRandNumber view.
    /// </summary>
    [Serializable]
    public partial class VRandNumber : ReadOnlyRecord<VRandNumber>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("vRandNumber", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarRandNumber = new TableSchema.TableColumn(schema);
                colvarRandNumber.ColumnName = "RandNumber";
                colvarRandNumber.DataType = DbType.Double;
                colvarRandNumber.MaxLength = 0;
                colvarRandNumber.AutoIncrement = false;
                colvarRandNumber.IsNullable = true;
                colvarRandNumber.IsPrimaryKey = false;
                colvarRandNumber.IsForeignKey = false;
                colvarRandNumber.IsReadOnly = false;
                
                schema.Columns.Add(colvarRandNumber);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("vRandNumber",schema);
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
	    public VRandNumber()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public VRandNumber(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public VRandNumber(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public VRandNumber(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("RandNumber")]
        [Bindable(true)]
        public double? RandNumber 
	    {
		    get
		    {
			    return GetColumnValue<double?>("RandNumber");
		    }
            set 
		    {
			    SetColumnValue("RandNumber", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string RandNumber = @"RandNumber";
            
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
