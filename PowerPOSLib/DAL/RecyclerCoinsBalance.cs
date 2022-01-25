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
namespace PowerPOS
{
	/// <summary>
	/// Strongly-typed collection for the RecyclerCoinsBalance class.
	/// </summary>
    [Serializable]
	public partial class RecyclerCoinsBalanceCollection : ActiveList<RecyclerCoinsBalance, RecyclerCoinsBalanceCollection>
	{	   
		public RecyclerCoinsBalanceCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>RecyclerCoinsBalanceCollection</returns>
		public RecyclerCoinsBalanceCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                RecyclerCoinsBalance o = this[i];
                foreach (SubSonic.Where w in this.wheres)
                {
                    bool remove = false;
                    System.Reflection.PropertyInfo pi = o.GetType().GetProperty(w.ColumnName);
                    if (pi.CanRead)
                    {
                        object val = pi.GetValue(o, null);
                        switch (w.Comparison)
                        {
                            case SubSonic.Comparison.Equals:
                                if (!val.Equals(w.ParameterValue))
                                {
                                    remove = true;
                                }
                                break;
                        }
                    }
                    if (remove)
                    {
                        this.Remove(o);
                        break;
                    }
                }
            }
            return this;
        }
		
		
	}
	/// <summary>
	/// This is an ActiveRecord class which wraps the RecyclerCoinsBalance table.
	/// </summary>
	[Serializable]
	public partial class RecyclerCoinsBalance : ActiveRecord<RecyclerCoinsBalance>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public RecyclerCoinsBalance()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public RecyclerCoinsBalance(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public RecyclerCoinsBalance(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public RecyclerCoinsBalance(string columnName, object columnValue)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByParam(columnName,columnValue);
		}
		
		protected static void SetSQLProps() { GetTableSchema(); }
		
		#endregion
		
		#region Schema and Query Accessor	
		public static Query CreateQuery() { return new Query(Schema); }
		public static TableSchema.Table Schema
		{
			get
			{
				if (BaseSchema == null)
					SetSQLProps();
				return BaseSchema;
			}
		}
		
		private static void GetTableSchema() 
		{
			if(!IsSchemaInitialized)
			{
				//Schema declaration
				TableSchema.Table schema = new TableSchema.Table("RecyclerCoinsBalance", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarDeviceID = new TableSchema.TableColumn(schema);
				colvarDeviceID.ColumnName = "DeviceID";
				colvarDeviceID.DataType = DbType.AnsiString;
				colvarDeviceID.MaxLength = 10;
				colvarDeviceID.AutoIncrement = false;
				colvarDeviceID.IsNullable = false;
				colvarDeviceID.IsPrimaryKey = true;
				colvarDeviceID.IsForeignKey = false;
				colvarDeviceID.IsReadOnly = false;
				colvarDeviceID.DefaultSetting = @"";
				colvarDeviceID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeviceID);
				
				TableSchema.TableColumn colvarR1Denomination = new TableSchema.TableColumn(schema);
				colvarR1Denomination.ColumnName = "R1Denomination";
				colvarR1Denomination.DataType = DbType.Int32;
				colvarR1Denomination.MaxLength = 0;
				colvarR1Denomination.AutoIncrement = false;
				colvarR1Denomination.IsNullable = true;
				colvarR1Denomination.IsPrimaryKey = false;
				colvarR1Denomination.IsForeignKey = false;
				colvarR1Denomination.IsReadOnly = false;
				colvarR1Denomination.DefaultSetting = @"";
				colvarR1Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR1Denomination);
				
				TableSchema.TableColumn colvarR1Quantity = new TableSchema.TableColumn(schema);
				colvarR1Quantity.ColumnName = "R1Quantity";
				colvarR1Quantity.DataType = DbType.Int32;
				colvarR1Quantity.MaxLength = 0;
				colvarR1Quantity.AutoIncrement = false;
				colvarR1Quantity.IsNullable = true;
				colvarR1Quantity.IsPrimaryKey = false;
				colvarR1Quantity.IsForeignKey = false;
				colvarR1Quantity.IsReadOnly = false;
				colvarR1Quantity.DefaultSetting = @"";
				colvarR1Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR1Quantity);
				
				TableSchema.TableColumn colvarR2Denomination = new TableSchema.TableColumn(schema);
				colvarR2Denomination.ColumnName = "R2Denomination";
				colvarR2Denomination.DataType = DbType.Int32;
				colvarR2Denomination.MaxLength = 0;
				colvarR2Denomination.AutoIncrement = false;
				colvarR2Denomination.IsNullable = true;
				colvarR2Denomination.IsPrimaryKey = false;
				colvarR2Denomination.IsForeignKey = false;
				colvarR2Denomination.IsReadOnly = false;
				colvarR2Denomination.DefaultSetting = @"";
				colvarR2Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR2Denomination);
				
				TableSchema.TableColumn colvarR2Quantity = new TableSchema.TableColumn(schema);
				colvarR2Quantity.ColumnName = "R2Quantity";
				colvarR2Quantity.DataType = DbType.Int32;
				colvarR2Quantity.MaxLength = 0;
				colvarR2Quantity.AutoIncrement = false;
				colvarR2Quantity.IsNullable = true;
				colvarR2Quantity.IsPrimaryKey = false;
				colvarR2Quantity.IsForeignKey = false;
				colvarR2Quantity.IsReadOnly = false;
				colvarR2Quantity.DefaultSetting = @"";
				colvarR2Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR2Quantity);
				
				TableSchema.TableColumn colvarR3Denomination = new TableSchema.TableColumn(schema);
				colvarR3Denomination.ColumnName = "R3Denomination";
				colvarR3Denomination.DataType = DbType.Int32;
				colvarR3Denomination.MaxLength = 0;
				colvarR3Denomination.AutoIncrement = false;
				colvarR3Denomination.IsNullable = true;
				colvarR3Denomination.IsPrimaryKey = false;
				colvarR3Denomination.IsForeignKey = false;
				colvarR3Denomination.IsReadOnly = false;
				colvarR3Denomination.DefaultSetting = @"";
				colvarR3Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR3Denomination);
				
				TableSchema.TableColumn colvarR3Quantity = new TableSchema.TableColumn(schema);
				colvarR3Quantity.ColumnName = "R3Quantity";
				colvarR3Quantity.DataType = DbType.Int32;
				colvarR3Quantity.MaxLength = 0;
				colvarR3Quantity.AutoIncrement = false;
				colvarR3Quantity.IsNullable = true;
				colvarR3Quantity.IsPrimaryKey = false;
				colvarR3Quantity.IsForeignKey = false;
				colvarR3Quantity.IsReadOnly = false;
				colvarR3Quantity.DefaultSetting = @"";
				colvarR3Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR3Quantity);
				
				TableSchema.TableColumn colvarR4Denomination = new TableSchema.TableColumn(schema);
				colvarR4Denomination.ColumnName = "R4Denomination";
				colvarR4Denomination.DataType = DbType.Int32;
				colvarR4Denomination.MaxLength = 0;
				colvarR4Denomination.AutoIncrement = false;
				colvarR4Denomination.IsNullable = true;
				colvarR4Denomination.IsPrimaryKey = false;
				colvarR4Denomination.IsForeignKey = false;
				colvarR4Denomination.IsReadOnly = false;
				colvarR4Denomination.DefaultSetting = @"";
				colvarR4Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR4Denomination);
				
				TableSchema.TableColumn colvarR4Quantity = new TableSchema.TableColumn(schema);
				colvarR4Quantity.ColumnName = "R4Quantity";
				colvarR4Quantity.DataType = DbType.Int32;
				colvarR4Quantity.MaxLength = 0;
				colvarR4Quantity.AutoIncrement = false;
				colvarR4Quantity.IsNullable = true;
				colvarR4Quantity.IsPrimaryKey = false;
				colvarR4Quantity.IsForeignKey = false;
				colvarR4Quantity.IsReadOnly = false;
				colvarR4Quantity.DefaultSetting = @"";
				colvarR4Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR4Quantity);
				
				TableSchema.TableColumn colvarR5Denomination = new TableSchema.TableColumn(schema);
				colvarR5Denomination.ColumnName = "R5Denomination";
				colvarR5Denomination.DataType = DbType.Int32;
				colvarR5Denomination.MaxLength = 0;
				colvarR5Denomination.AutoIncrement = false;
				colvarR5Denomination.IsNullable = true;
				colvarR5Denomination.IsPrimaryKey = false;
				colvarR5Denomination.IsForeignKey = false;
				colvarR5Denomination.IsReadOnly = false;
				colvarR5Denomination.DefaultSetting = @"";
				colvarR5Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR5Denomination);
				
				TableSchema.TableColumn colvarR5Quantity = new TableSchema.TableColumn(schema);
				colvarR5Quantity.ColumnName = "R5Quantity";
				colvarR5Quantity.DataType = DbType.Int32;
				colvarR5Quantity.MaxLength = 0;
				colvarR5Quantity.AutoIncrement = false;
				colvarR5Quantity.IsNullable = true;
				colvarR5Quantity.IsPrimaryKey = false;
				colvarR5Quantity.IsForeignKey = false;
				colvarR5Quantity.IsReadOnly = false;
				colvarR5Quantity.DefaultSetting = @"";
				colvarR5Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR5Quantity);
				
				TableSchema.TableColumn colvarR6Denomination = new TableSchema.TableColumn(schema);
				colvarR6Denomination.ColumnName = "R6Denomination";
				colvarR6Denomination.DataType = DbType.Int32;
				colvarR6Denomination.MaxLength = 0;
				colvarR6Denomination.AutoIncrement = false;
				colvarR6Denomination.IsNullable = true;
				colvarR6Denomination.IsPrimaryKey = false;
				colvarR6Denomination.IsForeignKey = false;
				colvarR6Denomination.IsReadOnly = false;
				colvarR6Denomination.DefaultSetting = @"";
				colvarR6Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR6Denomination);
				
				TableSchema.TableColumn colvarR6Quantity = new TableSchema.TableColumn(schema);
				colvarR6Quantity.ColumnName = "R6Quantity";
				colvarR6Quantity.DataType = DbType.Int32;
				colvarR6Quantity.MaxLength = 0;
				colvarR6Quantity.AutoIncrement = false;
				colvarR6Quantity.IsNullable = true;
				colvarR6Quantity.IsPrimaryKey = false;
				colvarR6Quantity.IsForeignKey = false;
				colvarR6Quantity.IsReadOnly = false;
				colvarR6Quantity.DefaultSetting = @"";
				colvarR6Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR6Quantity);
				
				TableSchema.TableColumn colvarR7Denomination = new TableSchema.TableColumn(schema);
				colvarR7Denomination.ColumnName = "R7Denomination";
				colvarR7Denomination.DataType = DbType.Int32;
				colvarR7Denomination.MaxLength = 0;
				colvarR7Denomination.AutoIncrement = false;
				colvarR7Denomination.IsNullable = true;
				colvarR7Denomination.IsPrimaryKey = false;
				colvarR7Denomination.IsForeignKey = false;
				colvarR7Denomination.IsReadOnly = false;
				colvarR7Denomination.DefaultSetting = @"";
				colvarR7Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR7Denomination);
				
				TableSchema.TableColumn colvarR7Quantity = new TableSchema.TableColumn(schema);
				colvarR7Quantity.ColumnName = "R7Quantity";
				colvarR7Quantity.DataType = DbType.Int32;
				colvarR7Quantity.MaxLength = 0;
				colvarR7Quantity.AutoIncrement = false;
				colvarR7Quantity.IsNullable = true;
				colvarR7Quantity.IsPrimaryKey = false;
				colvarR7Quantity.IsForeignKey = false;
				colvarR7Quantity.IsReadOnly = false;
				colvarR7Quantity.DefaultSetting = @"";
				colvarR7Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR7Quantity);
				
				TableSchema.TableColumn colvarR8Denomination = new TableSchema.TableColumn(schema);
				colvarR8Denomination.ColumnName = "R8Denomination";
				colvarR8Denomination.DataType = DbType.Int32;
				colvarR8Denomination.MaxLength = 0;
				colvarR8Denomination.AutoIncrement = false;
				colvarR8Denomination.IsNullable = true;
				colvarR8Denomination.IsPrimaryKey = false;
				colvarR8Denomination.IsForeignKey = false;
				colvarR8Denomination.IsReadOnly = false;
				colvarR8Denomination.DefaultSetting = @"";
				colvarR8Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR8Denomination);
				
				TableSchema.TableColumn colvarR8Quantity = new TableSchema.TableColumn(schema);
				colvarR8Quantity.ColumnName = "R8Quantity";
				colvarR8Quantity.DataType = DbType.Int32;
				colvarR8Quantity.MaxLength = 0;
				colvarR8Quantity.AutoIncrement = false;
				colvarR8Quantity.IsNullable = true;
				colvarR8Quantity.IsPrimaryKey = false;
				colvarR8Quantity.IsForeignKey = false;
				colvarR8Quantity.IsReadOnly = false;
				colvarR8Quantity.DefaultSetting = @"";
				colvarR8Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR8Quantity);
				
				TableSchema.TableColumn colvarR9Denomination = new TableSchema.TableColumn(schema);
				colvarR9Denomination.ColumnName = "R9Denomination";
				colvarR9Denomination.DataType = DbType.Int32;
				colvarR9Denomination.MaxLength = 0;
				colvarR9Denomination.AutoIncrement = false;
				colvarR9Denomination.IsNullable = true;
				colvarR9Denomination.IsPrimaryKey = false;
				colvarR9Denomination.IsForeignKey = false;
				colvarR9Denomination.IsReadOnly = false;
				colvarR9Denomination.DefaultSetting = @"";
				colvarR9Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR9Denomination);
				
				TableSchema.TableColumn colvarR9Quantity = new TableSchema.TableColumn(schema);
				colvarR9Quantity.ColumnName = "R9Quantity";
				colvarR9Quantity.DataType = DbType.Int32;
				colvarR9Quantity.MaxLength = 0;
				colvarR9Quantity.AutoIncrement = false;
				colvarR9Quantity.IsNullable = true;
				colvarR9Quantity.IsPrimaryKey = false;
				colvarR9Quantity.IsForeignKey = false;
				colvarR9Quantity.IsReadOnly = false;
				colvarR9Quantity.DefaultSetting = @"";
				colvarR9Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR9Quantity);
				
				TableSchema.TableColumn colvarR10Denomination = new TableSchema.TableColumn(schema);
				colvarR10Denomination.ColumnName = "R10Denomination";
				colvarR10Denomination.DataType = DbType.Int32;
				colvarR10Denomination.MaxLength = 0;
				colvarR10Denomination.AutoIncrement = false;
				colvarR10Denomination.IsNullable = true;
				colvarR10Denomination.IsPrimaryKey = false;
				colvarR10Denomination.IsForeignKey = false;
				colvarR10Denomination.IsReadOnly = false;
				colvarR10Denomination.DefaultSetting = @"";
				colvarR10Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR10Denomination);
				
				TableSchema.TableColumn colvarR10Quantity = new TableSchema.TableColumn(schema);
				colvarR10Quantity.ColumnName = "R10Quantity";
				colvarR10Quantity.DataType = DbType.Int32;
				colvarR10Quantity.MaxLength = 0;
				colvarR10Quantity.AutoIncrement = false;
				colvarR10Quantity.IsNullable = true;
				colvarR10Quantity.IsPrimaryKey = false;
				colvarR10Quantity.IsForeignKey = false;
				colvarR10Quantity.IsReadOnly = false;
				colvarR10Quantity.DefaultSetting = @"";
				colvarR10Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarR10Quantity);
				
				TableSchema.TableColumn colvarLastMachineEventID = new TableSchema.TableColumn(schema);
				colvarLastMachineEventID.ColumnName = "LastMachineEventID";
				colvarLastMachineEventID.DataType = DbType.AnsiString;
				colvarLastMachineEventID.MaxLength = 50;
				colvarLastMachineEventID.AutoIncrement = false;
				colvarLastMachineEventID.IsNullable = true;
				colvarLastMachineEventID.IsPrimaryKey = false;
				colvarLastMachineEventID.IsForeignKey = false;
				colvarLastMachineEventID.IsReadOnly = false;
				colvarLastMachineEventID.DefaultSetting = @"";
				colvarLastMachineEventID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastMachineEventID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("RecyclerCoinsBalance",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("DeviceID")]
		[Bindable(true)]
		public string DeviceID 
		{
			get { return GetColumnValue<string>(Columns.DeviceID); }
			set { SetColumnValue(Columns.DeviceID, value); }
		}
		  
		[XmlAttribute("R1Denomination")]
		[Bindable(true)]
		public int? R1Denomination 
		{
			get { return GetColumnValue<int?>(Columns.R1Denomination); }
			set { SetColumnValue(Columns.R1Denomination, value); }
		}
		  
		[XmlAttribute("R1Quantity")]
		[Bindable(true)]
		public int? R1Quantity 
		{
			get { return GetColumnValue<int?>(Columns.R1Quantity); }
			set { SetColumnValue(Columns.R1Quantity, value); }
		}
		  
		[XmlAttribute("R2Denomination")]
		[Bindable(true)]
		public int? R2Denomination 
		{
			get { return GetColumnValue<int?>(Columns.R2Denomination); }
			set { SetColumnValue(Columns.R2Denomination, value); }
		}
		  
		[XmlAttribute("R2Quantity")]
		[Bindable(true)]
		public int? R2Quantity 
		{
			get { return GetColumnValue<int?>(Columns.R2Quantity); }
			set { SetColumnValue(Columns.R2Quantity, value); }
		}
		  
		[XmlAttribute("R3Denomination")]
		[Bindable(true)]
		public int? R3Denomination 
		{
			get { return GetColumnValue<int?>(Columns.R3Denomination); }
			set { SetColumnValue(Columns.R3Denomination, value); }
		}
		  
		[XmlAttribute("R3Quantity")]
		[Bindable(true)]
		public int? R3Quantity 
		{
			get { return GetColumnValue<int?>(Columns.R3Quantity); }
			set { SetColumnValue(Columns.R3Quantity, value); }
		}
		  
		[XmlAttribute("R4Denomination")]
		[Bindable(true)]
		public int? R4Denomination 
		{
			get { return GetColumnValue<int?>(Columns.R4Denomination); }
			set { SetColumnValue(Columns.R4Denomination, value); }
		}
		  
		[XmlAttribute("R4Quantity")]
		[Bindable(true)]
		public int? R4Quantity 
		{
			get { return GetColumnValue<int?>(Columns.R4Quantity); }
			set { SetColumnValue(Columns.R4Quantity, value); }
		}
		  
		[XmlAttribute("R5Denomination")]
		[Bindable(true)]
		public int? R5Denomination 
		{
			get { return GetColumnValue<int?>(Columns.R5Denomination); }
			set { SetColumnValue(Columns.R5Denomination, value); }
		}
		  
		[XmlAttribute("R5Quantity")]
		[Bindable(true)]
		public int? R5Quantity 
		{
			get { return GetColumnValue<int?>(Columns.R5Quantity); }
			set { SetColumnValue(Columns.R5Quantity, value); }
		}
		  
		[XmlAttribute("R6Denomination")]
		[Bindable(true)]
		public int? R6Denomination 
		{
			get { return GetColumnValue<int?>(Columns.R6Denomination); }
			set { SetColumnValue(Columns.R6Denomination, value); }
		}
		  
		[XmlAttribute("R6Quantity")]
		[Bindable(true)]
		public int? R6Quantity 
		{
			get { return GetColumnValue<int?>(Columns.R6Quantity); }
			set { SetColumnValue(Columns.R6Quantity, value); }
		}
		  
		[XmlAttribute("R7Denomination")]
		[Bindable(true)]
		public int? R7Denomination 
		{
			get { return GetColumnValue<int?>(Columns.R7Denomination); }
			set { SetColumnValue(Columns.R7Denomination, value); }
		}
		  
		[XmlAttribute("R7Quantity")]
		[Bindable(true)]
		public int? R7Quantity 
		{
			get { return GetColumnValue<int?>(Columns.R7Quantity); }
			set { SetColumnValue(Columns.R7Quantity, value); }
		}
		  
		[XmlAttribute("R8Denomination")]
		[Bindable(true)]
		public int? R8Denomination 
		{
			get { return GetColumnValue<int?>(Columns.R8Denomination); }
			set { SetColumnValue(Columns.R8Denomination, value); }
		}
		  
		[XmlAttribute("R8Quantity")]
		[Bindable(true)]
		public int? R8Quantity 
		{
			get { return GetColumnValue<int?>(Columns.R8Quantity); }
			set { SetColumnValue(Columns.R8Quantity, value); }
		}
		  
		[XmlAttribute("R9Denomination")]
		[Bindable(true)]
		public int? R9Denomination 
		{
			get { return GetColumnValue<int?>(Columns.R9Denomination); }
			set { SetColumnValue(Columns.R9Denomination, value); }
		}
		  
		[XmlAttribute("R9Quantity")]
		[Bindable(true)]
		public int? R9Quantity 
		{
			get { return GetColumnValue<int?>(Columns.R9Quantity); }
			set { SetColumnValue(Columns.R9Quantity, value); }
		}
		  
		[XmlAttribute("R10Denomination")]
		[Bindable(true)]
		public int? R10Denomination 
		{
			get { return GetColumnValue<int?>(Columns.R10Denomination); }
			set { SetColumnValue(Columns.R10Denomination, value); }
		}
		  
		[XmlAttribute("R10Quantity")]
		[Bindable(true)]
		public int? R10Quantity 
		{
			get { return GetColumnValue<int?>(Columns.R10Quantity); }
			set { SetColumnValue(Columns.R10Quantity, value); }
		}
		  
		[XmlAttribute("LastMachineEventID")]
		[Bindable(true)]
		public string LastMachineEventID 
		{
			get { return GetColumnValue<string>(Columns.LastMachineEventID); }
			set { SetColumnValue(Columns.LastMachineEventID, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varDeviceID,int? varR1Denomination,int? varR1Quantity,int? varR2Denomination,int? varR2Quantity,int? varR3Denomination,int? varR3Quantity,int? varR4Denomination,int? varR4Quantity,int? varR5Denomination,int? varR5Quantity,int? varR6Denomination,int? varR6Quantity,int? varR7Denomination,int? varR7Quantity,int? varR8Denomination,int? varR8Quantity,int? varR9Denomination,int? varR9Quantity,int? varR10Denomination,int? varR10Quantity,string varLastMachineEventID)
		{
			RecyclerCoinsBalance item = new RecyclerCoinsBalance();
			
			item.DeviceID = varDeviceID;
			
			item.R1Denomination = varR1Denomination;
			
			item.R1Quantity = varR1Quantity;
			
			item.R2Denomination = varR2Denomination;
			
			item.R2Quantity = varR2Quantity;
			
			item.R3Denomination = varR3Denomination;
			
			item.R3Quantity = varR3Quantity;
			
			item.R4Denomination = varR4Denomination;
			
			item.R4Quantity = varR4Quantity;
			
			item.R5Denomination = varR5Denomination;
			
			item.R5Quantity = varR5Quantity;
			
			item.R6Denomination = varR6Denomination;
			
			item.R6Quantity = varR6Quantity;
			
			item.R7Denomination = varR7Denomination;
			
			item.R7Quantity = varR7Quantity;
			
			item.R8Denomination = varR8Denomination;
			
			item.R8Quantity = varR8Quantity;
			
			item.R9Denomination = varR9Denomination;
			
			item.R9Quantity = varR9Quantity;
			
			item.R10Denomination = varR10Denomination;
			
			item.R10Quantity = varR10Quantity;
			
			item.LastMachineEventID = varLastMachineEventID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varDeviceID,int? varR1Denomination,int? varR1Quantity,int? varR2Denomination,int? varR2Quantity,int? varR3Denomination,int? varR3Quantity,int? varR4Denomination,int? varR4Quantity,int? varR5Denomination,int? varR5Quantity,int? varR6Denomination,int? varR6Quantity,int? varR7Denomination,int? varR7Quantity,int? varR8Denomination,int? varR8Quantity,int? varR9Denomination,int? varR9Quantity,int? varR10Denomination,int? varR10Quantity,string varLastMachineEventID)
		{
			RecyclerCoinsBalance item = new RecyclerCoinsBalance();
			
				item.DeviceID = varDeviceID;
			
				item.R1Denomination = varR1Denomination;
			
				item.R1Quantity = varR1Quantity;
			
				item.R2Denomination = varR2Denomination;
			
				item.R2Quantity = varR2Quantity;
			
				item.R3Denomination = varR3Denomination;
			
				item.R3Quantity = varR3Quantity;
			
				item.R4Denomination = varR4Denomination;
			
				item.R4Quantity = varR4Quantity;
			
				item.R5Denomination = varR5Denomination;
			
				item.R5Quantity = varR5Quantity;
			
				item.R6Denomination = varR6Denomination;
			
				item.R6Quantity = varR6Quantity;
			
				item.R7Denomination = varR7Denomination;
			
				item.R7Quantity = varR7Quantity;
			
				item.R8Denomination = varR8Denomination;
			
				item.R8Quantity = varR8Quantity;
			
				item.R9Denomination = varR9Denomination;
			
				item.R9Quantity = varR9Quantity;
			
				item.R10Denomination = varR10Denomination;
			
				item.R10Quantity = varR10Quantity;
			
				item.LastMachineEventID = varLastMachineEventID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn DeviceIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn R1DenominationColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn R1QuantityColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn R2DenominationColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn R2QuantityColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn R3DenominationColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn R3QuantityColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn R4DenominationColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn R4QuantityColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn R5DenominationColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn R5QuantityColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn R6DenominationColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn R6QuantityColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn R7DenominationColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn R7QuantityColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn R8DenominationColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn R8QuantityColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn R9DenominationColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn R9QuantityColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn R10DenominationColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn R10QuantityColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn LastMachineEventIDColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string DeviceID = @"DeviceID";
			 public static string R1Denomination = @"R1Denomination";
			 public static string R1Quantity = @"R1Quantity";
			 public static string R2Denomination = @"R2Denomination";
			 public static string R2Quantity = @"R2Quantity";
			 public static string R3Denomination = @"R3Denomination";
			 public static string R3Quantity = @"R3Quantity";
			 public static string R4Denomination = @"R4Denomination";
			 public static string R4Quantity = @"R4Quantity";
			 public static string R5Denomination = @"R5Denomination";
			 public static string R5Quantity = @"R5Quantity";
			 public static string R6Denomination = @"R6Denomination";
			 public static string R6Quantity = @"R6Quantity";
			 public static string R7Denomination = @"R7Denomination";
			 public static string R7Quantity = @"R7Quantity";
			 public static string R8Denomination = @"R8Denomination";
			 public static string R8Quantity = @"R8Quantity";
			 public static string R9Denomination = @"R9Denomination";
			 public static string R9Quantity = @"R9Quantity";
			 public static string R10Denomination = @"R10Denomination";
			 public static string R10Quantity = @"R10Quantity";
			 public static string LastMachineEventID = @"LastMachineEventID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
