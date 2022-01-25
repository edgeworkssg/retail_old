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
	/// Strongly-typed collection for the Company class.
	/// </summary>
    [Serializable]
	public partial class CompanyCollection : ActiveList<Company, CompanyCollection>
	{	   
		public CompanyCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>CompanyCollection</returns>
		public CompanyCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Company o = this[i];
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
	/// This is an ActiveRecord class which wraps the Company table.
	/// </summary>
	[Serializable]
	public partial class Company : ActiveRecord<Company>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Company()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Company(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Company(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Company(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Company", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarCompanyID = new TableSchema.TableColumn(schema);
				colvarCompanyID.ColumnName = "CompanyID";
				colvarCompanyID.DataType = DbType.Guid;
				colvarCompanyID.MaxLength = 0;
				colvarCompanyID.AutoIncrement = false;
				colvarCompanyID.IsNullable = false;
				colvarCompanyID.IsPrimaryKey = true;
				colvarCompanyID.IsForeignKey = false;
				colvarCompanyID.IsReadOnly = false;
				
						colvarCompanyID.DefaultSetting = @"(newid())";
				colvarCompanyID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCompanyID);
				
				TableSchema.TableColumn colvarCompanyName = new TableSchema.TableColumn(schema);
				colvarCompanyName.ColumnName = "CompanyName";
				colvarCompanyName.DataType = DbType.AnsiString;
				colvarCompanyName.MaxLength = -1;
				colvarCompanyName.AutoIncrement = false;
				colvarCompanyName.IsNullable = false;
				colvarCompanyName.IsPrimaryKey = false;
				colvarCompanyName.IsForeignKey = false;
				colvarCompanyName.IsReadOnly = false;
				colvarCompanyName.DefaultSetting = @"";
				colvarCompanyName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCompanyName);
				
				TableSchema.TableColumn colvarReceiptName = new TableSchema.TableColumn(schema);
				colvarReceiptName.ColumnName = "ReceiptName";
				colvarReceiptName.DataType = DbType.AnsiString;
				colvarReceiptName.MaxLength = 50;
				colvarReceiptName.AutoIncrement = false;
				colvarReceiptName.IsNullable = true;
				colvarReceiptName.IsPrimaryKey = false;
				colvarReceiptName.IsForeignKey = false;
				colvarReceiptName.IsReadOnly = false;
				colvarReceiptName.DefaultSetting = @"";
				colvarReceiptName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReceiptName);
				
				TableSchema.TableColumn colvarGSTRegNo = new TableSchema.TableColumn(schema);
				colvarGSTRegNo.ColumnName = "GSTRegNo";
				colvarGSTRegNo.DataType = DbType.AnsiString;
				colvarGSTRegNo.MaxLength = 50;
				colvarGSTRegNo.AutoIncrement = false;
				colvarGSTRegNo.IsNullable = true;
				colvarGSTRegNo.IsPrimaryKey = false;
				colvarGSTRegNo.IsForeignKey = false;
				colvarGSTRegNo.IsReadOnly = false;
				colvarGSTRegNo.DefaultSetting = @"";
				colvarGSTRegNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGSTRegNo);
				
				TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
				colvarCreatedOn.ColumnName = "CreatedOn";
				colvarCreatedOn.DataType = DbType.DateTime;
				colvarCreatedOn.MaxLength = 0;
				colvarCreatedOn.AutoIncrement = false;
				colvarCreatedOn.IsNullable = true;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				colvarCreatedOn.DefaultSetting = @"";
				colvarCreatedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedOn);
				
				TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
				colvarModifiedOn.ColumnName = "ModifiedOn";
				colvarModifiedOn.DataType = DbType.DateTime;
				colvarModifiedOn.MaxLength = 0;
				colvarModifiedOn.AutoIncrement = false;
				colvarModifiedOn.IsNullable = true;
				colvarModifiedOn.IsPrimaryKey = false;
				colvarModifiedOn.IsForeignKey = false;
				colvarModifiedOn.IsReadOnly = false;
				colvarModifiedOn.DefaultSetting = @"";
				colvarModifiedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedOn);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.AnsiString;
				colvarCreatedBy.MaxLength = 50;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = true;
				colvarCreatedBy.IsPrimaryKey = false;
				colvarCreatedBy.IsForeignKey = false;
				colvarCreatedBy.IsReadOnly = false;
				colvarCreatedBy.DefaultSetting = @"";
				colvarCreatedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedBy);
				
				TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
				colvarModifiedBy.ColumnName = "ModifiedBy";
				colvarModifiedBy.DataType = DbType.AnsiString;
				colvarModifiedBy.MaxLength = 50;
				colvarModifiedBy.AutoIncrement = false;
				colvarModifiedBy.IsNullable = true;
				colvarModifiedBy.IsPrimaryKey = false;
				colvarModifiedBy.IsForeignKey = false;
				colvarModifiedBy.IsReadOnly = false;
				colvarModifiedBy.DefaultSetting = @"";
				colvarModifiedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedBy);
				
				TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
				colvarDeleted.ColumnName = "Deleted";
				colvarDeleted.DataType = DbType.Boolean;
				colvarDeleted.MaxLength = 0;
				colvarDeleted.AutoIncrement = false;
				colvarDeleted.IsNullable = true;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				colvarDeleted.DefaultSetting = @"";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				TableSchema.TableColumn colvarAddress1 = new TableSchema.TableColumn(schema);
				colvarAddress1.ColumnName = "Address1";
				colvarAddress1.DataType = DbType.AnsiString;
				colvarAddress1.MaxLength = 50;
				colvarAddress1.AutoIncrement = false;
				colvarAddress1.IsNullable = true;
				colvarAddress1.IsPrimaryKey = false;
				colvarAddress1.IsForeignKey = false;
				colvarAddress1.IsReadOnly = false;
				colvarAddress1.DefaultSetting = @"";
				colvarAddress1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAddress1);
				
				TableSchema.TableColumn colvarAddress2 = new TableSchema.TableColumn(schema);
				colvarAddress2.ColumnName = "Address2";
				colvarAddress2.DataType = DbType.AnsiString;
				colvarAddress2.MaxLength = 50;
				colvarAddress2.AutoIncrement = false;
				colvarAddress2.IsNullable = true;
				colvarAddress2.IsPrimaryKey = false;
				colvarAddress2.IsForeignKey = false;
				colvarAddress2.IsReadOnly = false;
				colvarAddress2.DefaultSetting = @"";
				colvarAddress2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAddress2);
				
				TableSchema.TableColumn colvarCity = new TableSchema.TableColumn(schema);
				colvarCity.ColumnName = "City";
				colvarCity.DataType = DbType.AnsiString;
				colvarCity.MaxLength = 50;
				colvarCity.AutoIncrement = false;
				colvarCity.IsNullable = true;
				colvarCity.IsPrimaryKey = false;
				colvarCity.IsForeignKey = false;
				colvarCity.IsReadOnly = false;
				colvarCity.DefaultSetting = @"";
				colvarCity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCity);
				
				TableSchema.TableColumn colvarCountry = new TableSchema.TableColumn(schema);
				colvarCountry.ColumnName = "Country";
				colvarCountry.DataType = DbType.AnsiString;
				colvarCountry.MaxLength = 50;
				colvarCountry.AutoIncrement = false;
				colvarCountry.IsNullable = true;
				colvarCountry.IsPrimaryKey = false;
				colvarCountry.IsForeignKey = false;
				colvarCountry.IsReadOnly = false;
				colvarCountry.DefaultSetting = @"";
				colvarCountry.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCountry);
				
				TableSchema.TableColumn colvarZipCode = new TableSchema.TableColumn(schema);
				colvarZipCode.ColumnName = "ZipCode";
				colvarZipCode.DataType = DbType.AnsiString;
				colvarZipCode.MaxLength = 50;
				colvarZipCode.AutoIncrement = false;
				colvarZipCode.IsNullable = true;
				colvarZipCode.IsPrimaryKey = false;
				colvarZipCode.IsForeignKey = false;
				colvarZipCode.IsReadOnly = false;
				colvarZipCode.DefaultSetting = @"";
				colvarZipCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarZipCode);
				
				TableSchema.TableColumn colvarMobile = new TableSchema.TableColumn(schema);
				colvarMobile.ColumnName = "Mobile";
				colvarMobile.DataType = DbType.AnsiString;
				colvarMobile.MaxLength = 50;
				colvarMobile.AutoIncrement = false;
				colvarMobile.IsNullable = true;
				colvarMobile.IsPrimaryKey = false;
				colvarMobile.IsForeignKey = false;
				colvarMobile.IsReadOnly = false;
				colvarMobile.DefaultSetting = @"";
				colvarMobile.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMobile);
				
				TableSchema.TableColumn colvarFax = new TableSchema.TableColumn(schema);
				colvarFax.ColumnName = "Fax";
				colvarFax.DataType = DbType.AnsiString;
				colvarFax.MaxLength = 50;
				colvarFax.AutoIncrement = false;
				colvarFax.IsNullable = true;
				colvarFax.IsPrimaryKey = false;
				colvarFax.IsForeignKey = false;
				colvarFax.IsReadOnly = false;
				colvarFax.DefaultSetting = @"";
				colvarFax.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFax);
				
				TableSchema.TableColumn colvarCompanyImage = new TableSchema.TableColumn(schema);
				colvarCompanyImage.ColumnName = "CompanyImage";
				colvarCompanyImage.DataType = DbType.Binary;
				colvarCompanyImage.MaxLength = -1;
				colvarCompanyImage.AutoIncrement = false;
				colvarCompanyImage.IsNullable = true;
				colvarCompanyImage.IsPrimaryKey = false;
				colvarCompanyImage.IsForeignKey = false;
				colvarCompanyImage.IsReadOnly = false;
				colvarCompanyImage.DefaultSetting = @"";
				colvarCompanyImage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCompanyImage);
				
				TableSchema.TableColumn colvarUserfld1 = new TableSchema.TableColumn(schema);
				colvarUserfld1.ColumnName = "userfld1";
				colvarUserfld1.DataType = DbType.AnsiString;
				colvarUserfld1.MaxLength = 50;
				colvarUserfld1.AutoIncrement = false;
				colvarUserfld1.IsNullable = true;
				colvarUserfld1.IsPrimaryKey = false;
				colvarUserfld1.IsForeignKey = false;
				colvarUserfld1.IsReadOnly = false;
				colvarUserfld1.DefaultSetting = @"";
				colvarUserfld1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld1);
				
				TableSchema.TableColumn colvarUserfld2 = new TableSchema.TableColumn(schema);
				colvarUserfld2.ColumnName = "userfld2";
				colvarUserfld2.DataType = DbType.AnsiString;
				colvarUserfld2.MaxLength = 50;
				colvarUserfld2.AutoIncrement = false;
				colvarUserfld2.IsNullable = true;
				colvarUserfld2.IsPrimaryKey = false;
				colvarUserfld2.IsForeignKey = false;
				colvarUserfld2.IsReadOnly = false;
				colvarUserfld2.DefaultSetting = @"";
				colvarUserfld2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld2);
				
				TableSchema.TableColumn colvarUserfld3 = new TableSchema.TableColumn(schema);
				colvarUserfld3.ColumnName = "userfld3";
				colvarUserfld3.DataType = DbType.AnsiString;
				colvarUserfld3.MaxLength = 50;
				colvarUserfld3.AutoIncrement = false;
				colvarUserfld3.IsNullable = true;
				colvarUserfld3.IsPrimaryKey = false;
				colvarUserfld3.IsForeignKey = false;
				colvarUserfld3.IsReadOnly = false;
				colvarUserfld3.DefaultSetting = @"";
				colvarUserfld3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld3);
				
				TableSchema.TableColumn colvarUserfld4 = new TableSchema.TableColumn(schema);
				colvarUserfld4.ColumnName = "userfld4";
				colvarUserfld4.DataType = DbType.AnsiString;
				colvarUserfld4.MaxLength = 50;
				colvarUserfld4.AutoIncrement = false;
				colvarUserfld4.IsNullable = true;
				colvarUserfld4.IsPrimaryKey = false;
				colvarUserfld4.IsForeignKey = false;
				colvarUserfld4.IsReadOnly = false;
				colvarUserfld4.DefaultSetting = @"";
				colvarUserfld4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld4);
				
				TableSchema.TableColumn colvarUserfld5 = new TableSchema.TableColumn(schema);
				colvarUserfld5.ColumnName = "userfld5";
				colvarUserfld5.DataType = DbType.AnsiString;
				colvarUserfld5.MaxLength = 50;
				colvarUserfld5.AutoIncrement = false;
				colvarUserfld5.IsNullable = true;
				colvarUserfld5.IsPrimaryKey = false;
				colvarUserfld5.IsForeignKey = false;
				colvarUserfld5.IsReadOnly = false;
				colvarUserfld5.DefaultSetting = @"";
				colvarUserfld5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld5);
				
				TableSchema.TableColumn colvarUserflag1 = new TableSchema.TableColumn(schema);
				colvarUserflag1.ColumnName = "userflag1";
				colvarUserflag1.DataType = DbType.Boolean;
				colvarUserflag1.MaxLength = 0;
				colvarUserflag1.AutoIncrement = false;
				colvarUserflag1.IsNullable = true;
				colvarUserflag1.IsPrimaryKey = false;
				colvarUserflag1.IsForeignKey = false;
				colvarUserflag1.IsReadOnly = false;
				colvarUserflag1.DefaultSetting = @"";
				colvarUserflag1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag1);
				
				TableSchema.TableColumn colvarUserflag2 = new TableSchema.TableColumn(schema);
				colvarUserflag2.ColumnName = "userflag2";
				colvarUserflag2.DataType = DbType.Boolean;
				colvarUserflag2.MaxLength = 0;
				colvarUserflag2.AutoIncrement = false;
				colvarUserflag2.IsNullable = true;
				colvarUserflag2.IsPrimaryKey = false;
				colvarUserflag2.IsForeignKey = false;
				colvarUserflag2.IsReadOnly = false;
				colvarUserflag2.DefaultSetting = @"";
				colvarUserflag2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag2);
				
				TableSchema.TableColumn colvarUserflag3 = new TableSchema.TableColumn(schema);
				colvarUserflag3.ColumnName = "userflag3";
				colvarUserflag3.DataType = DbType.Boolean;
				colvarUserflag3.MaxLength = 0;
				colvarUserflag3.AutoIncrement = false;
				colvarUserflag3.IsNullable = true;
				colvarUserflag3.IsPrimaryKey = false;
				colvarUserflag3.IsForeignKey = false;
				colvarUserflag3.IsReadOnly = false;
				colvarUserflag3.DefaultSetting = @"";
				colvarUserflag3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag3);
				
				TableSchema.TableColumn colvarUserflag4 = new TableSchema.TableColumn(schema);
				colvarUserflag4.ColumnName = "userflag4";
				colvarUserflag4.DataType = DbType.Boolean;
				colvarUserflag4.MaxLength = 0;
				colvarUserflag4.AutoIncrement = false;
				colvarUserflag4.IsNullable = true;
				colvarUserflag4.IsPrimaryKey = false;
				colvarUserflag4.IsForeignKey = false;
				colvarUserflag4.IsReadOnly = false;
				colvarUserflag4.DefaultSetting = @"";
				colvarUserflag4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag4);
				
				TableSchema.TableColumn colvarUserflag5 = new TableSchema.TableColumn(schema);
				colvarUserflag5.ColumnName = "userflag5";
				colvarUserflag5.DataType = DbType.Boolean;
				colvarUserflag5.MaxLength = 0;
				colvarUserflag5.AutoIncrement = false;
				colvarUserflag5.IsNullable = true;
				colvarUserflag5.IsPrimaryKey = false;
				colvarUserflag5.IsForeignKey = false;
				colvarUserflag5.IsReadOnly = false;
				colvarUserflag5.DefaultSetting = @"";
				colvarUserflag5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag5);
				
				TableSchema.TableColumn colvarUserfloat1 = new TableSchema.TableColumn(schema);
				colvarUserfloat1.ColumnName = "userfloat1";
				colvarUserfloat1.DataType = DbType.Currency;
				colvarUserfloat1.MaxLength = 0;
				colvarUserfloat1.AutoIncrement = false;
				colvarUserfloat1.IsNullable = true;
				colvarUserfloat1.IsPrimaryKey = false;
				colvarUserfloat1.IsForeignKey = false;
				colvarUserfloat1.IsReadOnly = false;
				colvarUserfloat1.DefaultSetting = @"";
				colvarUserfloat1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat1);
				
				TableSchema.TableColumn colvarUserfloat2 = new TableSchema.TableColumn(schema);
				colvarUserfloat2.ColumnName = "userfloat2";
				colvarUserfloat2.DataType = DbType.Currency;
				colvarUserfloat2.MaxLength = 0;
				colvarUserfloat2.AutoIncrement = false;
				colvarUserfloat2.IsNullable = true;
				colvarUserfloat2.IsPrimaryKey = false;
				colvarUserfloat2.IsForeignKey = false;
				colvarUserfloat2.IsReadOnly = false;
				colvarUserfloat2.DefaultSetting = @"";
				colvarUserfloat2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat2);
				
				TableSchema.TableColumn colvarUserfloat3 = new TableSchema.TableColumn(schema);
				colvarUserfloat3.ColumnName = "userfloat3";
				colvarUserfloat3.DataType = DbType.Currency;
				colvarUserfloat3.MaxLength = 0;
				colvarUserfloat3.AutoIncrement = false;
				colvarUserfloat3.IsNullable = true;
				colvarUserfloat3.IsPrimaryKey = false;
				colvarUserfloat3.IsForeignKey = false;
				colvarUserfloat3.IsReadOnly = false;
				colvarUserfloat3.DefaultSetting = @"";
				colvarUserfloat3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat3);
				
				TableSchema.TableColumn colvarUserfloat4 = new TableSchema.TableColumn(schema);
				colvarUserfloat4.ColumnName = "userfloat4";
				colvarUserfloat4.DataType = DbType.Currency;
				colvarUserfloat4.MaxLength = 0;
				colvarUserfloat4.AutoIncrement = false;
				colvarUserfloat4.IsNullable = true;
				colvarUserfloat4.IsPrimaryKey = false;
				colvarUserfloat4.IsForeignKey = false;
				colvarUserfloat4.IsReadOnly = false;
				colvarUserfloat4.DefaultSetting = @"";
				colvarUserfloat4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat4);
				
				TableSchema.TableColumn colvarUserfloat5 = new TableSchema.TableColumn(schema);
				colvarUserfloat5.ColumnName = "userfloat5";
				colvarUserfloat5.DataType = DbType.Currency;
				colvarUserfloat5.MaxLength = 0;
				colvarUserfloat5.AutoIncrement = false;
				colvarUserfloat5.IsNullable = true;
				colvarUserfloat5.IsPrimaryKey = false;
				colvarUserfloat5.IsForeignKey = false;
				colvarUserfloat5.IsReadOnly = false;
				colvarUserfloat5.DefaultSetting = @"";
				colvarUserfloat5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat5);
				
				TableSchema.TableColumn colvarUserint1 = new TableSchema.TableColumn(schema);
				colvarUserint1.ColumnName = "userint1";
				colvarUserint1.DataType = DbType.Int32;
				colvarUserint1.MaxLength = 0;
				colvarUserint1.AutoIncrement = false;
				colvarUserint1.IsNullable = true;
				colvarUserint1.IsPrimaryKey = false;
				colvarUserint1.IsForeignKey = false;
				colvarUserint1.IsReadOnly = false;
				colvarUserint1.DefaultSetting = @"";
				colvarUserint1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint1);
				
				TableSchema.TableColumn colvarUserint2 = new TableSchema.TableColumn(schema);
				colvarUserint2.ColumnName = "userint2";
				colvarUserint2.DataType = DbType.Int32;
				colvarUserint2.MaxLength = 0;
				colvarUserint2.AutoIncrement = false;
				colvarUserint2.IsNullable = true;
				colvarUserint2.IsPrimaryKey = false;
				colvarUserint2.IsForeignKey = false;
				colvarUserint2.IsReadOnly = false;
				colvarUserint2.DefaultSetting = @"";
				colvarUserint2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint2);
				
				TableSchema.TableColumn colvarUserint3 = new TableSchema.TableColumn(schema);
				colvarUserint3.ColumnName = "userint3";
				colvarUserint3.DataType = DbType.Int32;
				colvarUserint3.MaxLength = 0;
				colvarUserint3.AutoIncrement = false;
				colvarUserint3.IsNullable = true;
				colvarUserint3.IsPrimaryKey = false;
				colvarUserint3.IsForeignKey = false;
				colvarUserint3.IsReadOnly = false;
				colvarUserint3.DefaultSetting = @"";
				colvarUserint3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint3);
				
				TableSchema.TableColumn colvarUserint4 = new TableSchema.TableColumn(schema);
				colvarUserint4.ColumnName = "userint4";
				colvarUserint4.DataType = DbType.Int32;
				colvarUserint4.MaxLength = 0;
				colvarUserint4.AutoIncrement = false;
				colvarUserint4.IsNullable = true;
				colvarUserint4.IsPrimaryKey = false;
				colvarUserint4.IsForeignKey = false;
				colvarUserint4.IsReadOnly = false;
				colvarUserint4.DefaultSetting = @"";
				colvarUserint4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint4);
				
				TableSchema.TableColumn colvarUserint5 = new TableSchema.TableColumn(schema);
				colvarUserint5.ColumnName = "userint5";
				colvarUserint5.DataType = DbType.Int32;
				colvarUserint5.MaxLength = 0;
				colvarUserint5.AutoIncrement = false;
				colvarUserint5.IsNullable = true;
				colvarUserint5.IsPrimaryKey = false;
				colvarUserint5.IsForeignKey = false;
				colvarUserint5.IsReadOnly = false;
				colvarUserint5.DefaultSetting = @"";
				colvarUserint5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint5);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Company",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("CompanyID")]
		[Bindable(true)]
		public Guid CompanyID 
		{
			get { return GetColumnValue<Guid>(Columns.CompanyID); }
			set { SetColumnValue(Columns.CompanyID, value); }
		}
		  
		[XmlAttribute("CompanyName")]
		[Bindable(true)]
		public string CompanyName 
		{
			get { return GetColumnValue<string>(Columns.CompanyName); }
			set { SetColumnValue(Columns.CompanyName, value); }
		}
		  
		[XmlAttribute("ReceiptName")]
		[Bindable(true)]
		public string ReceiptName 
		{
			get { return GetColumnValue<string>(Columns.ReceiptName); }
			set { SetColumnValue(Columns.ReceiptName, value); }
		}
		  
		[XmlAttribute("GSTRegNo")]
		[Bindable(true)]
		public string GSTRegNo 
		{
			get { return GetColumnValue<string>(Columns.GSTRegNo); }
			set { SetColumnValue(Columns.GSTRegNo, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("Address1")]
		[Bindable(true)]
		public string Address1 
		{
			get { return GetColumnValue<string>(Columns.Address1); }
			set { SetColumnValue(Columns.Address1, value); }
		}
		  
		[XmlAttribute("Address2")]
		[Bindable(true)]
		public string Address2 
		{
			get { return GetColumnValue<string>(Columns.Address2); }
			set { SetColumnValue(Columns.Address2, value); }
		}
		  
		[XmlAttribute("City")]
		[Bindable(true)]
		public string City 
		{
			get { return GetColumnValue<string>(Columns.City); }
			set { SetColumnValue(Columns.City, value); }
		}
		  
		[XmlAttribute("Country")]
		[Bindable(true)]
		public string Country 
		{
			get { return GetColumnValue<string>(Columns.Country); }
			set { SetColumnValue(Columns.Country, value); }
		}
		  
		[XmlAttribute("ZipCode")]
		[Bindable(true)]
		public string ZipCode 
		{
			get { return GetColumnValue<string>(Columns.ZipCode); }
			set { SetColumnValue(Columns.ZipCode, value); }
		}
		  
		[XmlAttribute("Mobile")]
		[Bindable(true)]
		public string Mobile 
		{
			get { return GetColumnValue<string>(Columns.Mobile); }
			set { SetColumnValue(Columns.Mobile, value); }
		}
		  
		[XmlAttribute("Fax")]
		[Bindable(true)]
		public string Fax 
		{
			get { return GetColumnValue<string>(Columns.Fax); }
			set { SetColumnValue(Columns.Fax, value); }
		}
		  
		[XmlAttribute("CompanyImage")]
		[Bindable(true)]
		public byte[] CompanyImage 
		{
			get { return GetColumnValue<byte[]>(Columns.CompanyImage); }
			set { SetColumnValue(Columns.CompanyImage, value); }
		}
		  
		[XmlAttribute("Userfld1")]
		[Bindable(true)]
		public string Userfld1 
		{
			get { return GetColumnValue<string>(Columns.Userfld1); }
			set { SetColumnValue(Columns.Userfld1, value); }
		}
		  
		[XmlAttribute("Userfld2")]
		[Bindable(true)]
		public string Userfld2 
		{
			get { return GetColumnValue<string>(Columns.Userfld2); }
			set { SetColumnValue(Columns.Userfld2, value); }
		}
		  
		[XmlAttribute("Userfld3")]
		[Bindable(true)]
		public string Userfld3 
		{
			get { return GetColumnValue<string>(Columns.Userfld3); }
			set { SetColumnValue(Columns.Userfld3, value); }
		}
		  
		[XmlAttribute("Userfld4")]
		[Bindable(true)]
		public string Userfld4 
		{
			get { return GetColumnValue<string>(Columns.Userfld4); }
			set { SetColumnValue(Columns.Userfld4, value); }
		}
		  
		[XmlAttribute("Userfld5")]
		[Bindable(true)]
		public string Userfld5 
		{
			get { return GetColumnValue<string>(Columns.Userfld5); }
			set { SetColumnValue(Columns.Userfld5, value); }
		}
		  
		[XmlAttribute("Userflag1")]
		[Bindable(true)]
		public bool? Userflag1 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag1); }
			set { SetColumnValue(Columns.Userflag1, value); }
		}
		  
		[XmlAttribute("Userflag2")]
		[Bindable(true)]
		public bool? Userflag2 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag2); }
			set { SetColumnValue(Columns.Userflag2, value); }
		}
		  
		[XmlAttribute("Userflag3")]
		[Bindable(true)]
		public bool? Userflag3 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag3); }
			set { SetColumnValue(Columns.Userflag3, value); }
		}
		  
		[XmlAttribute("Userflag4")]
		[Bindable(true)]
		public bool? Userflag4 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag4); }
			set { SetColumnValue(Columns.Userflag4, value); }
		}
		  
		[XmlAttribute("Userflag5")]
		[Bindable(true)]
		public bool? Userflag5 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag5); }
			set { SetColumnValue(Columns.Userflag5, value); }
		}
		  
		[XmlAttribute("Userfloat1")]
		[Bindable(true)]
		public decimal? Userfloat1 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat1); }
			set { SetColumnValue(Columns.Userfloat1, value); }
		}
		  
		[XmlAttribute("Userfloat2")]
		[Bindable(true)]
		public decimal? Userfloat2 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat2); }
			set { SetColumnValue(Columns.Userfloat2, value); }
		}
		  
		[XmlAttribute("Userfloat3")]
		[Bindable(true)]
		public decimal? Userfloat3 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat3); }
			set { SetColumnValue(Columns.Userfloat3, value); }
		}
		  
		[XmlAttribute("Userfloat4")]
		[Bindable(true)]
		public decimal? Userfloat4 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat4); }
			set { SetColumnValue(Columns.Userfloat4, value); }
		}
		  
		[XmlAttribute("Userfloat5")]
		[Bindable(true)]
		public decimal? Userfloat5 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat5); }
			set { SetColumnValue(Columns.Userfloat5, value); }
		}
		  
		[XmlAttribute("Userint1")]
		[Bindable(true)]
		public int? Userint1 
		{
			get { return GetColumnValue<int?>(Columns.Userint1); }
			set { SetColumnValue(Columns.Userint1, value); }
		}
		  
		[XmlAttribute("Userint2")]
		[Bindable(true)]
		public int? Userint2 
		{
			get { return GetColumnValue<int?>(Columns.Userint2); }
			set { SetColumnValue(Columns.Userint2, value); }
		}
		  
		[XmlAttribute("Userint3")]
		[Bindable(true)]
		public int? Userint3 
		{
			get { return GetColumnValue<int?>(Columns.Userint3); }
			set { SetColumnValue(Columns.Userint3, value); }
		}
		  
		[XmlAttribute("Userint4")]
		[Bindable(true)]
		public int? Userint4 
		{
			get { return GetColumnValue<int?>(Columns.Userint4); }
			set { SetColumnValue(Columns.Userint4, value); }
		}
		  
		[XmlAttribute("Userint5")]
		[Bindable(true)]
		public int? Userint5 
		{
			get { return GetColumnValue<int?>(Columns.Userint5); }
			set { SetColumnValue(Columns.Userint5, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varCompanyID,string varCompanyName,string varReceiptName,string varGSTRegNo,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted,string varAddress1,string varAddress2,string varCity,string varCountry,string varZipCode,string varMobile,string varFax,byte[] varCompanyImage,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5)
		{
			Company item = new Company();
			
			item.CompanyID = varCompanyID;
			
			item.CompanyName = varCompanyName;
			
			item.ReceiptName = varReceiptName;
			
			item.GSTRegNo = varGSTRegNo;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedOn = varModifiedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.Address1 = varAddress1;
			
			item.Address2 = varAddress2;
			
			item.City = varCity;
			
			item.Country = varCountry;
			
			item.ZipCode = varZipCode;
			
			item.Mobile = varMobile;
			
			item.Fax = varFax;
			
			item.CompanyImage = varCompanyImage;
			
			item.Userfld1 = varUserfld1;
			
			item.Userfld2 = varUserfld2;
			
			item.Userfld3 = varUserfld3;
			
			item.Userfld4 = varUserfld4;
			
			item.Userfld5 = varUserfld5;
			
			item.Userflag1 = varUserflag1;
			
			item.Userflag2 = varUserflag2;
			
			item.Userflag3 = varUserflag3;
			
			item.Userflag4 = varUserflag4;
			
			item.Userflag5 = varUserflag5;
			
			item.Userfloat1 = varUserfloat1;
			
			item.Userfloat2 = varUserfloat2;
			
			item.Userfloat3 = varUserfloat3;
			
			item.Userfloat4 = varUserfloat4;
			
			item.Userfloat5 = varUserfloat5;
			
			item.Userint1 = varUserint1;
			
			item.Userint2 = varUserint2;
			
			item.Userint3 = varUserint3;
			
			item.Userint4 = varUserint4;
			
			item.Userint5 = varUserint5;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varCompanyID,string varCompanyName,string varReceiptName,string varGSTRegNo,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted,string varAddress1,string varAddress2,string varCity,string varCountry,string varZipCode,string varMobile,string varFax,byte[] varCompanyImage,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5)
		{
			Company item = new Company();
			
				item.CompanyID = varCompanyID;
			
				item.CompanyName = varCompanyName;
			
				item.ReceiptName = varReceiptName;
			
				item.GSTRegNo = varGSTRegNo;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedOn = varModifiedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.Address1 = varAddress1;
			
				item.Address2 = varAddress2;
			
				item.City = varCity;
			
				item.Country = varCountry;
			
				item.ZipCode = varZipCode;
			
				item.Mobile = varMobile;
			
				item.Fax = varFax;
			
				item.CompanyImage = varCompanyImage;
			
				item.Userfld1 = varUserfld1;
			
				item.Userfld2 = varUserfld2;
			
				item.Userfld3 = varUserfld3;
			
				item.Userfld4 = varUserfld4;
			
				item.Userfld5 = varUserfld5;
			
				item.Userflag1 = varUserflag1;
			
				item.Userflag2 = varUserflag2;
			
				item.Userflag3 = varUserflag3;
			
				item.Userflag4 = varUserflag4;
			
				item.Userflag5 = varUserflag5;
			
				item.Userfloat1 = varUserfloat1;
			
				item.Userfloat2 = varUserfloat2;
			
				item.Userfloat3 = varUserfloat3;
			
				item.Userfloat4 = varUserfloat4;
			
				item.Userfloat5 = varUserfloat5;
			
				item.Userint1 = varUserint1;
			
				item.Userint2 = varUserint2;
			
				item.Userint3 = varUserint3;
			
				item.Userint4 = varUserint4;
			
				item.Userint5 = varUserint5;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn CompanyIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn CompanyNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ReceiptNameColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn GSTRegNoColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn Address1Column
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn Address2Column
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn CityColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn CountryColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn ZipCodeColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn MobileColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn FaxColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn CompanyImageColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld1Column
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld2Column
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld3Column
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld4Column
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld5Column
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag1Column
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag2Column
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag3Column
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag4Column
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag5Column
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat1Column
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat2Column
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat3Column
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat4Column
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat5Column
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint1Column
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint2Column
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint3Column
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint4Column
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint5Column
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string CompanyID = @"CompanyID";
			 public static string CompanyName = @"CompanyName";
			 public static string ReceiptName = @"ReceiptName";
			 public static string GSTRegNo = @"GSTRegNo";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string Address1 = @"Address1";
			 public static string Address2 = @"Address2";
			 public static string City = @"City";
			 public static string Country = @"Country";
			 public static string ZipCode = @"ZipCode";
			 public static string Mobile = @"Mobile";
			 public static string Fax = @"Fax";
			 public static string CompanyImage = @"CompanyImage";
			 public static string Userfld1 = @"userfld1";
			 public static string Userfld2 = @"userfld2";
			 public static string Userfld3 = @"userfld3";
			 public static string Userfld4 = @"userfld4";
			 public static string Userfld5 = @"userfld5";
			 public static string Userflag1 = @"userflag1";
			 public static string Userflag2 = @"userflag2";
			 public static string Userflag3 = @"userflag3";
			 public static string Userflag4 = @"userflag4";
			 public static string Userflag5 = @"userflag5";
			 public static string Userfloat1 = @"userfloat1";
			 public static string Userfloat2 = @"userfloat2";
			 public static string Userfloat3 = @"userfloat3";
			 public static string Userfloat4 = @"userfloat4";
			 public static string Userfloat5 = @"userfloat5";
			 public static string Userint1 = @"userint1";
			 public static string Userint2 = @"userint2";
			 public static string Userint3 = @"userint3";
			 public static string Userint4 = @"userint4";
			 public static string Userint5 = @"userint5";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
