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
	/// Strongly-typed collection for the RecyclerNotesBalance class.
	/// </summary>
    [Serializable]
	public partial class RecyclerNotesBalanceCollection : ActiveList<RecyclerNotesBalance, RecyclerNotesBalanceCollection>
	{	   
		public RecyclerNotesBalanceCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>RecyclerNotesBalanceCollection</returns>
		public RecyclerNotesBalanceCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                RecyclerNotesBalance o = this[i];
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
	/// This is an ActiveRecord class which wraps the RecyclerNotesBalance table.
	/// </summary>
	[Serializable]
	public partial class RecyclerNotesBalance : ActiveRecord<RecyclerNotesBalance>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public RecyclerNotesBalance()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public RecyclerNotesBalance(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public RecyclerNotesBalance(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public RecyclerNotesBalance(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("RecyclerNotesBalance", TableType.Table, DataService.GetInstance("PowerPOS"));
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
				
				TableSchema.TableColumn colvarC1Denomination = new TableSchema.TableColumn(schema);
				colvarC1Denomination.ColumnName = "C1Denomination";
				colvarC1Denomination.DataType = DbType.Int32;
				colvarC1Denomination.MaxLength = 0;
				colvarC1Denomination.AutoIncrement = false;
				colvarC1Denomination.IsNullable = true;
				colvarC1Denomination.IsPrimaryKey = false;
				colvarC1Denomination.IsForeignKey = false;
				colvarC1Denomination.IsReadOnly = false;
				colvarC1Denomination.DefaultSetting = @"";
				colvarC1Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC1Denomination);
				
				TableSchema.TableColumn colvarC1Quantity = new TableSchema.TableColumn(schema);
				colvarC1Quantity.ColumnName = "C1Quantity";
				colvarC1Quantity.DataType = DbType.Int32;
				colvarC1Quantity.MaxLength = 0;
				colvarC1Quantity.AutoIncrement = false;
				colvarC1Quantity.IsNullable = true;
				colvarC1Quantity.IsPrimaryKey = false;
				colvarC1Quantity.IsForeignKey = false;
				colvarC1Quantity.IsReadOnly = false;
				colvarC1Quantity.DefaultSetting = @"";
				colvarC1Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC1Quantity);
				
				TableSchema.TableColumn colvarC2Denomination = new TableSchema.TableColumn(schema);
				colvarC2Denomination.ColumnName = "C2Denomination";
				colvarC2Denomination.DataType = DbType.Int32;
				colvarC2Denomination.MaxLength = 0;
				colvarC2Denomination.AutoIncrement = false;
				colvarC2Denomination.IsNullable = true;
				colvarC2Denomination.IsPrimaryKey = false;
				colvarC2Denomination.IsForeignKey = false;
				colvarC2Denomination.IsReadOnly = false;
				colvarC2Denomination.DefaultSetting = @"";
				colvarC2Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC2Denomination);
				
				TableSchema.TableColumn colvarC2Quantity = new TableSchema.TableColumn(schema);
				colvarC2Quantity.ColumnName = "C2Quantity";
				colvarC2Quantity.DataType = DbType.Int32;
				colvarC2Quantity.MaxLength = 0;
				colvarC2Quantity.AutoIncrement = false;
				colvarC2Quantity.IsNullable = true;
				colvarC2Quantity.IsPrimaryKey = false;
				colvarC2Quantity.IsForeignKey = false;
				colvarC2Quantity.IsReadOnly = false;
				colvarC2Quantity.DefaultSetting = @"";
				colvarC2Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC2Quantity);
				
				TableSchema.TableColumn colvarC3Denomination = new TableSchema.TableColumn(schema);
				colvarC3Denomination.ColumnName = "C3Denomination";
				colvarC3Denomination.DataType = DbType.Int32;
				colvarC3Denomination.MaxLength = 0;
				colvarC3Denomination.AutoIncrement = false;
				colvarC3Denomination.IsNullable = true;
				colvarC3Denomination.IsPrimaryKey = false;
				colvarC3Denomination.IsForeignKey = false;
				colvarC3Denomination.IsReadOnly = false;
				colvarC3Denomination.DefaultSetting = @"";
				colvarC3Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC3Denomination);
				
				TableSchema.TableColumn colvarC3Quantity = new TableSchema.TableColumn(schema);
				colvarC3Quantity.ColumnName = "C3Quantity";
				colvarC3Quantity.DataType = DbType.Int32;
				colvarC3Quantity.MaxLength = 0;
				colvarC3Quantity.AutoIncrement = false;
				colvarC3Quantity.IsNullable = true;
				colvarC3Quantity.IsPrimaryKey = false;
				colvarC3Quantity.IsForeignKey = false;
				colvarC3Quantity.IsReadOnly = false;
				colvarC3Quantity.DefaultSetting = @"";
				colvarC3Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC3Quantity);
				
				TableSchema.TableColumn colvarC4Denomination = new TableSchema.TableColumn(schema);
				colvarC4Denomination.ColumnName = "C4Denomination";
				colvarC4Denomination.DataType = DbType.Int32;
				colvarC4Denomination.MaxLength = 0;
				colvarC4Denomination.AutoIncrement = false;
				colvarC4Denomination.IsNullable = true;
				colvarC4Denomination.IsPrimaryKey = false;
				colvarC4Denomination.IsForeignKey = false;
				colvarC4Denomination.IsReadOnly = false;
				colvarC4Denomination.DefaultSetting = @"";
				colvarC4Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC4Denomination);
				
				TableSchema.TableColumn colvarC4Quantity = new TableSchema.TableColumn(schema);
				colvarC4Quantity.ColumnName = "C4Quantity";
				colvarC4Quantity.DataType = DbType.Int32;
				colvarC4Quantity.MaxLength = 0;
				colvarC4Quantity.AutoIncrement = false;
				colvarC4Quantity.IsNullable = true;
				colvarC4Quantity.IsPrimaryKey = false;
				colvarC4Quantity.IsForeignKey = false;
				colvarC4Quantity.IsReadOnly = false;
				colvarC4Quantity.DefaultSetting = @"";
				colvarC4Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC4Quantity);
				
				TableSchema.TableColumn colvarC5Denomination = new TableSchema.TableColumn(schema);
				colvarC5Denomination.ColumnName = "C5Denomination";
				colvarC5Denomination.DataType = DbType.Int32;
				colvarC5Denomination.MaxLength = 0;
				colvarC5Denomination.AutoIncrement = false;
				colvarC5Denomination.IsNullable = true;
				colvarC5Denomination.IsPrimaryKey = false;
				colvarC5Denomination.IsForeignKey = false;
				colvarC5Denomination.IsReadOnly = false;
				colvarC5Denomination.DefaultSetting = @"";
				colvarC5Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC5Denomination);
				
				TableSchema.TableColumn colvarC5Quantity = new TableSchema.TableColumn(schema);
				colvarC5Quantity.ColumnName = "C5Quantity";
				colvarC5Quantity.DataType = DbType.Int32;
				colvarC5Quantity.MaxLength = 0;
				colvarC5Quantity.AutoIncrement = false;
				colvarC5Quantity.IsNullable = true;
				colvarC5Quantity.IsPrimaryKey = false;
				colvarC5Quantity.IsForeignKey = false;
				colvarC5Quantity.IsReadOnly = false;
				colvarC5Quantity.DefaultSetting = @"";
				colvarC5Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC5Quantity);
				
				TableSchema.TableColumn colvarC6Denomination = new TableSchema.TableColumn(schema);
				colvarC6Denomination.ColumnName = "C6Denomination";
				colvarC6Denomination.DataType = DbType.Int32;
				colvarC6Denomination.MaxLength = 0;
				colvarC6Denomination.AutoIncrement = false;
				colvarC6Denomination.IsNullable = true;
				colvarC6Denomination.IsPrimaryKey = false;
				colvarC6Denomination.IsForeignKey = false;
				colvarC6Denomination.IsReadOnly = false;
				colvarC6Denomination.DefaultSetting = @"";
				colvarC6Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC6Denomination);
				
				TableSchema.TableColumn colvarC6Quantity = new TableSchema.TableColumn(schema);
				colvarC6Quantity.ColumnName = "C6Quantity";
				colvarC6Quantity.DataType = DbType.Int32;
				colvarC6Quantity.MaxLength = 0;
				colvarC6Quantity.AutoIncrement = false;
				colvarC6Quantity.IsNullable = true;
				colvarC6Quantity.IsPrimaryKey = false;
				colvarC6Quantity.IsForeignKey = false;
				colvarC6Quantity.IsReadOnly = false;
				colvarC6Quantity.DefaultSetting = @"";
				colvarC6Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC6Quantity);
				
				TableSchema.TableColumn colvarC7Denomination = new TableSchema.TableColumn(schema);
				colvarC7Denomination.ColumnName = "C7Denomination";
				colvarC7Denomination.DataType = DbType.Int32;
				colvarC7Denomination.MaxLength = 0;
				colvarC7Denomination.AutoIncrement = false;
				colvarC7Denomination.IsNullable = true;
				colvarC7Denomination.IsPrimaryKey = false;
				colvarC7Denomination.IsForeignKey = false;
				colvarC7Denomination.IsReadOnly = false;
				colvarC7Denomination.DefaultSetting = @"";
				colvarC7Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC7Denomination);
				
				TableSchema.TableColumn colvarC7Quantity = new TableSchema.TableColumn(schema);
				colvarC7Quantity.ColumnName = "C7Quantity";
				colvarC7Quantity.DataType = DbType.Int32;
				colvarC7Quantity.MaxLength = 0;
				colvarC7Quantity.AutoIncrement = false;
				colvarC7Quantity.IsNullable = true;
				colvarC7Quantity.IsPrimaryKey = false;
				colvarC7Quantity.IsForeignKey = false;
				colvarC7Quantity.IsReadOnly = false;
				colvarC7Quantity.DefaultSetting = @"";
				colvarC7Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC7Quantity);
				
				TableSchema.TableColumn colvarC8Denomination = new TableSchema.TableColumn(schema);
				colvarC8Denomination.ColumnName = "C8Denomination";
				colvarC8Denomination.DataType = DbType.Int32;
				colvarC8Denomination.MaxLength = 0;
				colvarC8Denomination.AutoIncrement = false;
				colvarC8Denomination.IsNullable = true;
				colvarC8Denomination.IsPrimaryKey = false;
				colvarC8Denomination.IsForeignKey = false;
				colvarC8Denomination.IsReadOnly = false;
				colvarC8Denomination.DefaultSetting = @"";
				colvarC8Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC8Denomination);
				
				TableSchema.TableColumn colvarC8Quantity = new TableSchema.TableColumn(schema);
				colvarC8Quantity.ColumnName = "C8Quantity";
				colvarC8Quantity.DataType = DbType.Int32;
				colvarC8Quantity.MaxLength = 0;
				colvarC8Quantity.AutoIncrement = false;
				colvarC8Quantity.IsNullable = true;
				colvarC8Quantity.IsPrimaryKey = false;
				colvarC8Quantity.IsForeignKey = false;
				colvarC8Quantity.IsReadOnly = false;
				colvarC8Quantity.DefaultSetting = @"";
				colvarC8Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC8Quantity);
				
				TableSchema.TableColumn colvarC9Denomination = new TableSchema.TableColumn(schema);
				colvarC9Denomination.ColumnName = "C9Denomination";
				colvarC9Denomination.DataType = DbType.Int32;
				colvarC9Denomination.MaxLength = 0;
				colvarC9Denomination.AutoIncrement = false;
				colvarC9Denomination.IsNullable = true;
				colvarC9Denomination.IsPrimaryKey = false;
				colvarC9Denomination.IsForeignKey = false;
				colvarC9Denomination.IsReadOnly = false;
				colvarC9Denomination.DefaultSetting = @"";
				colvarC9Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC9Denomination);
				
				TableSchema.TableColumn colvarC9Quantity = new TableSchema.TableColumn(schema);
				colvarC9Quantity.ColumnName = "C9Quantity";
				colvarC9Quantity.DataType = DbType.Int32;
				colvarC9Quantity.MaxLength = 0;
				colvarC9Quantity.AutoIncrement = false;
				colvarC9Quantity.IsNullable = true;
				colvarC9Quantity.IsPrimaryKey = false;
				colvarC9Quantity.IsForeignKey = false;
				colvarC9Quantity.IsReadOnly = false;
				colvarC9Quantity.DefaultSetting = @"";
				colvarC9Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC9Quantity);
				
				TableSchema.TableColumn colvarC10Denomination = new TableSchema.TableColumn(schema);
				colvarC10Denomination.ColumnName = "C10Denomination";
				colvarC10Denomination.DataType = DbType.Int32;
				colvarC10Denomination.MaxLength = 0;
				colvarC10Denomination.AutoIncrement = false;
				colvarC10Denomination.IsNullable = true;
				colvarC10Denomination.IsPrimaryKey = false;
				colvarC10Denomination.IsForeignKey = false;
				colvarC10Denomination.IsReadOnly = false;
				colvarC10Denomination.DefaultSetting = @"";
				colvarC10Denomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC10Denomination);
				
				TableSchema.TableColumn colvarC10Quantity = new TableSchema.TableColumn(schema);
				colvarC10Quantity.ColumnName = "C10Quantity";
				colvarC10Quantity.DataType = DbType.Int32;
				colvarC10Quantity.MaxLength = 0;
				colvarC10Quantity.AutoIncrement = false;
				colvarC10Quantity.IsNullable = true;
				colvarC10Quantity.IsPrimaryKey = false;
				colvarC10Quantity.IsForeignKey = false;
				colvarC10Quantity.IsReadOnly = false;
				colvarC10Quantity.DefaultSetting = @"";
				colvarC10Quantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarC10Quantity);
				
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
				DataService.Providers["PowerPOS"].AddSchema("RecyclerNotesBalance",schema);
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
		  
		[XmlAttribute("C1Denomination")]
		[Bindable(true)]
		public int? C1Denomination 
		{
			get { return GetColumnValue<int?>(Columns.C1Denomination); }
			set { SetColumnValue(Columns.C1Denomination, value); }
		}
		  
		[XmlAttribute("C1Quantity")]
		[Bindable(true)]
		public int? C1Quantity 
		{
			get { return GetColumnValue<int?>(Columns.C1Quantity); }
			set { SetColumnValue(Columns.C1Quantity, value); }
		}
		  
		[XmlAttribute("C2Denomination")]
		[Bindable(true)]
		public int? C2Denomination 
		{
			get { return GetColumnValue<int?>(Columns.C2Denomination); }
			set { SetColumnValue(Columns.C2Denomination, value); }
		}
		  
		[XmlAttribute("C2Quantity")]
		[Bindable(true)]
		public int? C2Quantity 
		{
			get { return GetColumnValue<int?>(Columns.C2Quantity); }
			set { SetColumnValue(Columns.C2Quantity, value); }
		}
		  
		[XmlAttribute("C3Denomination")]
		[Bindable(true)]
		public int? C3Denomination 
		{
			get { return GetColumnValue<int?>(Columns.C3Denomination); }
			set { SetColumnValue(Columns.C3Denomination, value); }
		}
		  
		[XmlAttribute("C3Quantity")]
		[Bindable(true)]
		public int? C3Quantity 
		{
			get { return GetColumnValue<int?>(Columns.C3Quantity); }
			set { SetColumnValue(Columns.C3Quantity, value); }
		}
		  
		[XmlAttribute("C4Denomination")]
		[Bindable(true)]
		public int? C4Denomination 
		{
			get { return GetColumnValue<int?>(Columns.C4Denomination); }
			set { SetColumnValue(Columns.C4Denomination, value); }
		}
		  
		[XmlAttribute("C4Quantity")]
		[Bindable(true)]
		public int? C4Quantity 
		{
			get { return GetColumnValue<int?>(Columns.C4Quantity); }
			set { SetColumnValue(Columns.C4Quantity, value); }
		}
		  
		[XmlAttribute("C5Denomination")]
		[Bindable(true)]
		public int? C5Denomination 
		{
			get { return GetColumnValue<int?>(Columns.C5Denomination); }
			set { SetColumnValue(Columns.C5Denomination, value); }
		}
		  
		[XmlAttribute("C5Quantity")]
		[Bindable(true)]
		public int? C5Quantity 
		{
			get { return GetColumnValue<int?>(Columns.C5Quantity); }
			set { SetColumnValue(Columns.C5Quantity, value); }
		}
		  
		[XmlAttribute("C6Denomination")]
		[Bindable(true)]
		public int? C6Denomination 
		{
			get { return GetColumnValue<int?>(Columns.C6Denomination); }
			set { SetColumnValue(Columns.C6Denomination, value); }
		}
		  
		[XmlAttribute("C6Quantity")]
		[Bindable(true)]
		public int? C6Quantity 
		{
			get { return GetColumnValue<int?>(Columns.C6Quantity); }
			set { SetColumnValue(Columns.C6Quantity, value); }
		}
		  
		[XmlAttribute("C7Denomination")]
		[Bindable(true)]
		public int? C7Denomination 
		{
			get { return GetColumnValue<int?>(Columns.C7Denomination); }
			set { SetColumnValue(Columns.C7Denomination, value); }
		}
		  
		[XmlAttribute("C7Quantity")]
		[Bindable(true)]
		public int? C7Quantity 
		{
			get { return GetColumnValue<int?>(Columns.C7Quantity); }
			set { SetColumnValue(Columns.C7Quantity, value); }
		}
		  
		[XmlAttribute("C8Denomination")]
		[Bindable(true)]
		public int? C8Denomination 
		{
			get { return GetColumnValue<int?>(Columns.C8Denomination); }
			set { SetColumnValue(Columns.C8Denomination, value); }
		}
		  
		[XmlAttribute("C8Quantity")]
		[Bindable(true)]
		public int? C8Quantity 
		{
			get { return GetColumnValue<int?>(Columns.C8Quantity); }
			set { SetColumnValue(Columns.C8Quantity, value); }
		}
		  
		[XmlAttribute("C9Denomination")]
		[Bindable(true)]
		public int? C9Denomination 
		{
			get { return GetColumnValue<int?>(Columns.C9Denomination); }
			set { SetColumnValue(Columns.C9Denomination, value); }
		}
		  
		[XmlAttribute("C9Quantity")]
		[Bindable(true)]
		public int? C9Quantity 
		{
			get { return GetColumnValue<int?>(Columns.C9Quantity); }
			set { SetColumnValue(Columns.C9Quantity, value); }
		}
		  
		[XmlAttribute("C10Denomination")]
		[Bindable(true)]
		public int? C10Denomination 
		{
			get { return GetColumnValue<int?>(Columns.C10Denomination); }
			set { SetColumnValue(Columns.C10Denomination, value); }
		}
		  
		[XmlAttribute("C10Quantity")]
		[Bindable(true)]
		public int? C10Quantity 
		{
			get { return GetColumnValue<int?>(Columns.C10Quantity); }
			set { SetColumnValue(Columns.C10Quantity, value); }
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
		public static void Insert(string varDeviceID,int? varR1Denomination,int? varR1Quantity,int? varR2Denomination,int? varR2Quantity,int? varR3Denomination,int? varR3Quantity,int? varR4Denomination,int? varR4Quantity,int? varR5Denomination,int? varR5Quantity,int? varC1Denomination,int? varC1Quantity,int? varC2Denomination,int? varC2Quantity,int? varC3Denomination,int? varC3Quantity,int? varC4Denomination,int? varC4Quantity,int? varC5Denomination,int? varC5Quantity,int? varC6Denomination,int? varC6Quantity,int? varC7Denomination,int? varC7Quantity,int? varC8Denomination,int? varC8Quantity,int? varC9Denomination,int? varC9Quantity,int? varC10Denomination,int? varC10Quantity,string varLastMachineEventID)
		{
			RecyclerNotesBalance item = new RecyclerNotesBalance();
			
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
			
			item.C1Denomination = varC1Denomination;
			
			item.C1Quantity = varC1Quantity;
			
			item.C2Denomination = varC2Denomination;
			
			item.C2Quantity = varC2Quantity;
			
			item.C3Denomination = varC3Denomination;
			
			item.C3Quantity = varC3Quantity;
			
			item.C4Denomination = varC4Denomination;
			
			item.C4Quantity = varC4Quantity;
			
			item.C5Denomination = varC5Denomination;
			
			item.C5Quantity = varC5Quantity;
			
			item.C6Denomination = varC6Denomination;
			
			item.C6Quantity = varC6Quantity;
			
			item.C7Denomination = varC7Denomination;
			
			item.C7Quantity = varC7Quantity;
			
			item.C8Denomination = varC8Denomination;
			
			item.C8Quantity = varC8Quantity;
			
			item.C9Denomination = varC9Denomination;
			
			item.C9Quantity = varC9Quantity;
			
			item.C10Denomination = varC10Denomination;
			
			item.C10Quantity = varC10Quantity;
			
			item.LastMachineEventID = varLastMachineEventID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varDeviceID,int? varR1Denomination,int? varR1Quantity,int? varR2Denomination,int? varR2Quantity,int? varR3Denomination,int? varR3Quantity,int? varR4Denomination,int? varR4Quantity,int? varR5Denomination,int? varR5Quantity,int? varC1Denomination,int? varC1Quantity,int? varC2Denomination,int? varC2Quantity,int? varC3Denomination,int? varC3Quantity,int? varC4Denomination,int? varC4Quantity,int? varC5Denomination,int? varC5Quantity,int? varC6Denomination,int? varC6Quantity,int? varC7Denomination,int? varC7Quantity,int? varC8Denomination,int? varC8Quantity,int? varC9Denomination,int? varC9Quantity,int? varC10Denomination,int? varC10Quantity,string varLastMachineEventID)
		{
			RecyclerNotesBalance item = new RecyclerNotesBalance();
			
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
			
				item.C1Denomination = varC1Denomination;
			
				item.C1Quantity = varC1Quantity;
			
				item.C2Denomination = varC2Denomination;
			
				item.C2Quantity = varC2Quantity;
			
				item.C3Denomination = varC3Denomination;
			
				item.C3Quantity = varC3Quantity;
			
				item.C4Denomination = varC4Denomination;
			
				item.C4Quantity = varC4Quantity;
			
				item.C5Denomination = varC5Denomination;
			
				item.C5Quantity = varC5Quantity;
			
				item.C6Denomination = varC6Denomination;
			
				item.C6Quantity = varC6Quantity;
			
				item.C7Denomination = varC7Denomination;
			
				item.C7Quantity = varC7Quantity;
			
				item.C8Denomination = varC8Denomination;
			
				item.C8Quantity = varC8Quantity;
			
				item.C9Denomination = varC9Denomination;
			
				item.C9Quantity = varC9Quantity;
			
				item.C10Denomination = varC10Denomination;
			
				item.C10Quantity = varC10Quantity;
			
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
        
        
        
        public static TableSchema.TableColumn C1DenominationColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn C1QuantityColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn C2DenominationColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn C2QuantityColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn C3DenominationColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn C3QuantityColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn C4DenominationColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn C4QuantityColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn C5DenominationColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn C5QuantityColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn C6DenominationColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn C6QuantityColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn C7DenominationColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn C7QuantityColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn C8DenominationColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn C8QuantityColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn C9DenominationColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn C9QuantityColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn C10DenominationColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn C10QuantityColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn LastMachineEventIDColumn
        {
            get { return Schema.Columns[31]; }
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
			 public static string C1Denomination = @"C1Denomination";
			 public static string C1Quantity = @"C1Quantity";
			 public static string C2Denomination = @"C2Denomination";
			 public static string C2Quantity = @"C2Quantity";
			 public static string C3Denomination = @"C3Denomination";
			 public static string C3Quantity = @"C3Quantity";
			 public static string C4Denomination = @"C4Denomination";
			 public static string C4Quantity = @"C4Quantity";
			 public static string C5Denomination = @"C5Denomination";
			 public static string C5Quantity = @"C5Quantity";
			 public static string C6Denomination = @"C6Denomination";
			 public static string C6Quantity = @"C6Quantity";
			 public static string C7Denomination = @"C7Denomination";
			 public static string C7Quantity = @"C7Quantity";
			 public static string C8Denomination = @"C8Denomination";
			 public static string C8Quantity = @"C8Quantity";
			 public static string C9Denomination = @"C9Denomination";
			 public static string C9Quantity = @"C9Quantity";
			 public static string C10Denomination = @"C10Denomination";
			 public static string C10Quantity = @"C10Quantity";
			 public static string LastMachineEventID = @"LastMachineEventID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
