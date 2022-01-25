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
	/// Strongly-typed collection for the CommissionHdr class.
	/// </summary>
    [Serializable]
	public partial class CommissionHdrCollection : ActiveList<CommissionHdr, CommissionHdrCollection>
	{	   
		public CommissionHdrCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>CommissionHdrCollection</returns>
		public CommissionHdrCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                CommissionHdr o = this[i];
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
	/// This is an ActiveRecord class which wraps the CommissionHdr table.
	/// </summary>
	[Serializable]
	public partial class CommissionHdr : ActiveRecord<CommissionHdr>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public CommissionHdr()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public CommissionHdr(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public CommissionHdr(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public CommissionHdr(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("CommissionHdr", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarCommissionHdrID = new TableSchema.TableColumn(schema);
				colvarCommissionHdrID.ColumnName = "CommissionHdrID";
				colvarCommissionHdrID.DataType = DbType.Int32;
				colvarCommissionHdrID.MaxLength = 0;
				colvarCommissionHdrID.AutoIncrement = true;
				colvarCommissionHdrID.IsNullable = false;
				colvarCommissionHdrID.IsPrimaryKey = true;
				colvarCommissionHdrID.IsForeignKey = false;
				colvarCommissionHdrID.IsReadOnly = false;
				colvarCommissionHdrID.DefaultSetting = @"";
				colvarCommissionHdrID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCommissionHdrID);
				
				TableSchema.TableColumn colvarSchemeName = new TableSchema.TableColumn(schema);
				colvarSchemeName.ColumnName = "SchemeName";
				colvarSchemeName.DataType = DbType.String;
				colvarSchemeName.MaxLength = 50;
				colvarSchemeName.AutoIncrement = false;
				colvarSchemeName.IsNullable = true;
				colvarSchemeName.IsPrimaryKey = false;
				colvarSchemeName.IsForeignKey = false;
				colvarSchemeName.IsReadOnly = false;
				colvarSchemeName.DefaultSetting = @"";
				colvarSchemeName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSchemeName);
				
				TableSchema.TableColumn colvarIsProduct = new TableSchema.TableColumn(schema);
				colvarIsProduct.ColumnName = "IsProduct";
				colvarIsProduct.DataType = DbType.Int32;
				colvarIsProduct.MaxLength = 0;
				colvarIsProduct.AutoIncrement = false;
				colvarIsProduct.IsNullable = true;
				colvarIsProduct.IsPrimaryKey = false;
				colvarIsProduct.IsForeignKey = false;
				colvarIsProduct.IsReadOnly = false;
				colvarIsProduct.DefaultSetting = @"";
				colvarIsProduct.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsProduct);
				
				TableSchema.TableColumn colvarProductWeight = new TableSchema.TableColumn(schema);
				colvarProductWeight.ColumnName = "ProductWeight";
				colvarProductWeight.DataType = DbType.Decimal;
				colvarProductWeight.MaxLength = 0;
				colvarProductWeight.AutoIncrement = false;
				colvarProductWeight.IsNullable = true;
				colvarProductWeight.IsPrimaryKey = false;
				colvarProductWeight.IsForeignKey = false;
				colvarProductWeight.IsReadOnly = false;
				colvarProductWeight.DefaultSetting = @"";
				colvarProductWeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarProductWeight);
				
				TableSchema.TableColumn colvarIsService = new TableSchema.TableColumn(schema);
				colvarIsService.ColumnName = "IsService";
				colvarIsService.DataType = DbType.Int32;
				colvarIsService.MaxLength = 0;
				colvarIsService.AutoIncrement = false;
				colvarIsService.IsNullable = true;
				colvarIsService.IsPrimaryKey = false;
				colvarIsService.IsForeignKey = false;
				colvarIsService.IsReadOnly = false;
				colvarIsService.DefaultSetting = @"";
				colvarIsService.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsService);
				
				TableSchema.TableColumn colvarServiceWeight = new TableSchema.TableColumn(schema);
				colvarServiceWeight.ColumnName = "ServiceWeight";
				colvarServiceWeight.DataType = DbType.Decimal;
				colvarServiceWeight.MaxLength = 0;
				colvarServiceWeight.AutoIncrement = false;
				colvarServiceWeight.IsNullable = true;
				colvarServiceWeight.IsPrimaryKey = false;
				colvarServiceWeight.IsForeignKey = false;
				colvarServiceWeight.IsReadOnly = false;
				colvarServiceWeight.DefaultSetting = @"";
				colvarServiceWeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarServiceWeight);
				
				TableSchema.TableColumn colvarIsPointSold = new TableSchema.TableColumn(schema);
				colvarIsPointSold.ColumnName = "IsPointSold";
				colvarIsPointSold.DataType = DbType.Int32;
				colvarIsPointSold.MaxLength = 0;
				colvarIsPointSold.AutoIncrement = false;
				colvarIsPointSold.IsNullable = true;
				colvarIsPointSold.IsPrimaryKey = false;
				colvarIsPointSold.IsForeignKey = false;
				colvarIsPointSold.IsReadOnly = false;
				colvarIsPointSold.DefaultSetting = @"";
				colvarIsPointSold.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsPointSold);
				
				TableSchema.TableColumn colvarPointSoldWeight = new TableSchema.TableColumn(schema);
				colvarPointSoldWeight.ColumnName = "PointSoldWeight";
				colvarPointSoldWeight.DataType = DbType.Decimal;
				colvarPointSoldWeight.MaxLength = 0;
				colvarPointSoldWeight.AutoIncrement = false;
				colvarPointSoldWeight.IsNullable = true;
				colvarPointSoldWeight.IsPrimaryKey = false;
				colvarPointSoldWeight.IsForeignKey = false;
				colvarPointSoldWeight.IsReadOnly = false;
				colvarPointSoldWeight.DefaultSetting = @"";
				colvarPointSoldWeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointSoldWeight);
				
				TableSchema.TableColumn colvarIsPackageSold = new TableSchema.TableColumn(schema);
				colvarIsPackageSold.ColumnName = "IsPackageSold";
				colvarIsPackageSold.DataType = DbType.Int32;
				colvarIsPackageSold.MaxLength = 0;
				colvarIsPackageSold.AutoIncrement = false;
				colvarIsPackageSold.IsNullable = true;
				colvarIsPackageSold.IsPrimaryKey = false;
				colvarIsPackageSold.IsForeignKey = false;
				colvarIsPackageSold.IsReadOnly = false;
				colvarIsPackageSold.DefaultSetting = @"";
				colvarIsPackageSold.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsPackageSold);
				
				TableSchema.TableColumn colvarPackageSoldWeight = new TableSchema.TableColumn(schema);
				colvarPackageSoldWeight.ColumnName = "PackageSoldWeight";
				colvarPackageSoldWeight.DataType = DbType.Decimal;
				colvarPackageSoldWeight.MaxLength = 0;
				colvarPackageSoldWeight.AutoIncrement = false;
				colvarPackageSoldWeight.IsNullable = true;
				colvarPackageSoldWeight.IsPrimaryKey = false;
				colvarPackageSoldWeight.IsForeignKey = false;
				colvarPackageSoldWeight.IsReadOnly = false;
				colvarPackageSoldWeight.DefaultSetting = @"";
				colvarPackageSoldWeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackageSoldWeight);
				
				TableSchema.TableColumn colvarIsPointRedeem = new TableSchema.TableColumn(schema);
				colvarIsPointRedeem.ColumnName = "IsPointRedeem";
				colvarIsPointRedeem.DataType = DbType.Int32;
				colvarIsPointRedeem.MaxLength = 0;
				colvarIsPointRedeem.AutoIncrement = false;
				colvarIsPointRedeem.IsNullable = true;
				colvarIsPointRedeem.IsPrimaryKey = false;
				colvarIsPointRedeem.IsForeignKey = false;
				colvarIsPointRedeem.IsReadOnly = false;
				colvarIsPointRedeem.DefaultSetting = @"";
				colvarIsPointRedeem.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsPointRedeem);
				
				TableSchema.TableColumn colvarPointRedeemWeight = new TableSchema.TableColumn(schema);
				colvarPointRedeemWeight.ColumnName = "PointRedeemWeight";
				colvarPointRedeemWeight.DataType = DbType.Decimal;
				colvarPointRedeemWeight.MaxLength = 0;
				colvarPointRedeemWeight.AutoIncrement = false;
				colvarPointRedeemWeight.IsNullable = true;
				colvarPointRedeemWeight.IsPrimaryKey = false;
				colvarPointRedeemWeight.IsForeignKey = false;
				colvarPointRedeemWeight.IsReadOnly = false;
				colvarPointRedeemWeight.DefaultSetting = @"";
				colvarPointRedeemWeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointRedeemWeight);
				
				TableSchema.TableColumn colvarIsPackageRedeem = new TableSchema.TableColumn(schema);
				colvarIsPackageRedeem.ColumnName = "IsPackageRedeem";
				colvarIsPackageRedeem.DataType = DbType.Int32;
				colvarIsPackageRedeem.MaxLength = 0;
				colvarIsPackageRedeem.AutoIncrement = false;
				colvarIsPackageRedeem.IsNullable = true;
				colvarIsPackageRedeem.IsPrimaryKey = false;
				colvarIsPackageRedeem.IsForeignKey = false;
				colvarIsPackageRedeem.IsReadOnly = false;
				colvarIsPackageRedeem.DefaultSetting = @"";
				colvarIsPackageRedeem.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsPackageRedeem);
				
				TableSchema.TableColumn colvarPackageRedeemWeight = new TableSchema.TableColumn(schema);
				colvarPackageRedeemWeight.ColumnName = "PackageRedeemWeight";
				colvarPackageRedeemWeight.DataType = DbType.Decimal;
				colvarPackageRedeemWeight.MaxLength = 0;
				colvarPackageRedeemWeight.AutoIncrement = false;
				colvarPackageRedeemWeight.IsNullable = true;
				colvarPackageRedeemWeight.IsPrimaryKey = false;
				colvarPackageRedeemWeight.IsForeignKey = false;
				colvarPackageRedeemWeight.IsReadOnly = false;
				colvarPackageRedeemWeight.DefaultSetting = @"";
				colvarPackageRedeemWeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackageRedeemWeight);
				
				TableSchema.TableColumn colvarIsDeductionFor2ndSalesPerson = new TableSchema.TableColumn(schema);
				colvarIsDeductionFor2ndSalesPerson.ColumnName = "IsDeductionFor2ndSalesPerson";
				colvarIsDeductionFor2ndSalesPerson.DataType = DbType.Int32;
				colvarIsDeductionFor2ndSalesPerson.MaxLength = 0;
				colvarIsDeductionFor2ndSalesPerson.AutoIncrement = false;
				colvarIsDeductionFor2ndSalesPerson.IsNullable = true;
				colvarIsDeductionFor2ndSalesPerson.IsPrimaryKey = false;
				colvarIsDeductionFor2ndSalesPerson.IsForeignKey = false;
				colvarIsDeductionFor2ndSalesPerson.IsReadOnly = false;
				colvarIsDeductionFor2ndSalesPerson.DefaultSetting = @"";
				colvarIsDeductionFor2ndSalesPerson.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsDeductionFor2ndSalesPerson);
				
				TableSchema.TableColumn colvarDeductionBy = new TableSchema.TableColumn(schema);
				colvarDeductionBy.ColumnName = "DeductionBy";
				colvarDeductionBy.DataType = DbType.AnsiString;
				colvarDeductionBy.MaxLength = 20;
				colvarDeductionBy.AutoIncrement = false;
				colvarDeductionBy.IsNullable = true;
				colvarDeductionBy.IsPrimaryKey = false;
				colvarDeductionBy.IsForeignKey = false;
				colvarDeductionBy.IsReadOnly = false;
				colvarDeductionBy.DefaultSetting = @"";
				colvarDeductionBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeductionBy);
				
				TableSchema.TableColumn colvarDeductionValue = new TableSchema.TableColumn(schema);
				colvarDeductionValue.ColumnName = "DeductionValue";
				colvarDeductionValue.DataType = DbType.Decimal;
				colvarDeductionValue.MaxLength = 0;
				colvarDeductionValue.AutoIncrement = false;
				colvarDeductionValue.IsNullable = true;
				colvarDeductionValue.IsPrimaryKey = false;
				colvarDeductionValue.IsForeignKey = false;
				colvarDeductionValue.IsReadOnly = false;
				colvarDeductionValue.DefaultSetting = @"";
				colvarDeductionValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeductionValue);
				
				TableSchema.TableColumn colvarSalesGroupID = new TableSchema.TableColumn(schema);
				colvarSalesGroupID.ColumnName = "SalesGroupID";
				colvarSalesGroupID.DataType = DbType.Int32;
				colvarSalesGroupID.MaxLength = 0;
				colvarSalesGroupID.AutoIncrement = false;
				colvarSalesGroupID.IsNullable = true;
				colvarSalesGroupID.IsPrimaryKey = false;
				colvarSalesGroupID.IsForeignKey = false;
				colvarSalesGroupID.IsReadOnly = false;
				colvarSalesGroupID.DefaultSetting = @"";
				colvarSalesGroupID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSalesGroupID);
				
				TableSchema.TableColumn colvarCommissionBy = new TableSchema.TableColumn(schema);
				colvarCommissionBy.ColumnName = "CommissionBy";
				colvarCommissionBy.DataType = DbType.AnsiString;
				colvarCommissionBy.MaxLength = 20;
				colvarCommissionBy.AutoIncrement = false;
				colvarCommissionBy.IsNullable = true;
				colvarCommissionBy.IsPrimaryKey = false;
				colvarCommissionBy.IsForeignKey = false;
				colvarCommissionBy.IsReadOnly = false;
				colvarCommissionBy.DefaultSetting = @"";
				colvarCommissionBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCommissionBy);
				
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
				
				TableSchema.TableColumn colvarUserfld6 = new TableSchema.TableColumn(schema);
				colvarUserfld6.ColumnName = "userfld6";
				colvarUserfld6.DataType = DbType.AnsiString;
				colvarUserfld6.MaxLength = 50;
				colvarUserfld6.AutoIncrement = false;
				colvarUserfld6.IsNullable = true;
				colvarUserfld6.IsPrimaryKey = false;
				colvarUserfld6.IsForeignKey = false;
				colvarUserfld6.IsReadOnly = false;
				colvarUserfld6.DefaultSetting = @"";
				colvarUserfld6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld6);
				
				TableSchema.TableColumn colvarUserfld7 = new TableSchema.TableColumn(schema);
				colvarUserfld7.ColumnName = "userfld7";
				colvarUserfld7.DataType = DbType.AnsiString;
				colvarUserfld7.MaxLength = 50;
				colvarUserfld7.AutoIncrement = false;
				colvarUserfld7.IsNullable = true;
				colvarUserfld7.IsPrimaryKey = false;
				colvarUserfld7.IsForeignKey = false;
				colvarUserfld7.IsReadOnly = false;
				colvarUserfld7.DefaultSetting = @"";
				colvarUserfld7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld7);
				
				TableSchema.TableColumn colvarUserfld8 = new TableSchema.TableColumn(schema);
				colvarUserfld8.ColumnName = "userfld8";
				colvarUserfld8.DataType = DbType.AnsiString;
				colvarUserfld8.MaxLength = 50;
				colvarUserfld8.AutoIncrement = false;
				colvarUserfld8.IsNullable = true;
				colvarUserfld8.IsPrimaryKey = false;
				colvarUserfld8.IsForeignKey = false;
				colvarUserfld8.IsReadOnly = false;
				colvarUserfld8.DefaultSetting = @"";
				colvarUserfld8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld8);
				
				TableSchema.TableColumn colvarUserfld9 = new TableSchema.TableColumn(schema);
				colvarUserfld9.ColumnName = "userfld9";
				colvarUserfld9.DataType = DbType.AnsiString;
				colvarUserfld9.MaxLength = 50;
				colvarUserfld9.AutoIncrement = false;
				colvarUserfld9.IsNullable = true;
				colvarUserfld9.IsPrimaryKey = false;
				colvarUserfld9.IsForeignKey = false;
				colvarUserfld9.IsReadOnly = false;
				colvarUserfld9.DefaultSetting = @"";
				colvarUserfld9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld9);
				
				TableSchema.TableColumn colvarUserfld10 = new TableSchema.TableColumn(schema);
				colvarUserfld10.ColumnName = "userfld10";
				colvarUserfld10.DataType = DbType.AnsiString;
				colvarUserfld10.MaxLength = 50;
				colvarUserfld10.AutoIncrement = false;
				colvarUserfld10.IsNullable = true;
				colvarUserfld10.IsPrimaryKey = false;
				colvarUserfld10.IsForeignKey = false;
				colvarUserfld10.IsReadOnly = false;
				colvarUserfld10.DefaultSetting = @"";
				colvarUserfld10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld10);
				
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
				DataService.Providers["PowerPOS"].AddSchema("CommissionHdr",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("CommissionHdrID")]
		[Bindable(true)]
		public int CommissionHdrID 
		{
			get { return GetColumnValue<int>(Columns.CommissionHdrID); }
			set { SetColumnValue(Columns.CommissionHdrID, value); }
		}
		  
		[XmlAttribute("SchemeName")]
		[Bindable(true)]
		public string SchemeName 
		{
			get { return GetColumnValue<string>(Columns.SchemeName); }
			set { SetColumnValue(Columns.SchemeName, value); }
		}
		  
		[XmlAttribute("IsProduct")]
		[Bindable(true)]
		public int? IsProduct 
		{
			get { return GetColumnValue<int?>(Columns.IsProduct); }
			set { SetColumnValue(Columns.IsProduct, value); }
		}
		  
		[XmlAttribute("ProductWeight")]
		[Bindable(true)]
		public decimal? ProductWeight 
		{
			get { return GetColumnValue<decimal?>(Columns.ProductWeight); }
			set { SetColumnValue(Columns.ProductWeight, value); }
		}
		  
		[XmlAttribute("IsService")]
		[Bindable(true)]
		public int? IsService 
		{
			get { return GetColumnValue<int?>(Columns.IsService); }
			set { SetColumnValue(Columns.IsService, value); }
		}
		  
		[XmlAttribute("ServiceWeight")]
		[Bindable(true)]
		public decimal? ServiceWeight 
		{
			get { return GetColumnValue<decimal?>(Columns.ServiceWeight); }
			set { SetColumnValue(Columns.ServiceWeight, value); }
		}
		  
		[XmlAttribute("IsPointSold")]
		[Bindable(true)]
		public int? IsPointSold 
		{
			get { return GetColumnValue<int?>(Columns.IsPointSold); }
			set { SetColumnValue(Columns.IsPointSold, value); }
		}
		  
		[XmlAttribute("PointSoldWeight")]
		[Bindable(true)]
		public decimal? PointSoldWeight 
		{
			get { return GetColumnValue<decimal?>(Columns.PointSoldWeight); }
			set { SetColumnValue(Columns.PointSoldWeight, value); }
		}
		  
		[XmlAttribute("IsPackageSold")]
		[Bindable(true)]
		public int? IsPackageSold 
		{
			get { return GetColumnValue<int?>(Columns.IsPackageSold); }
			set { SetColumnValue(Columns.IsPackageSold, value); }
		}
		  
		[XmlAttribute("PackageSoldWeight")]
		[Bindable(true)]
		public decimal? PackageSoldWeight 
		{
			get { return GetColumnValue<decimal?>(Columns.PackageSoldWeight); }
			set { SetColumnValue(Columns.PackageSoldWeight, value); }
		}
		  
		[XmlAttribute("IsPointRedeem")]
		[Bindable(true)]
		public int? IsPointRedeem 
		{
			get { return GetColumnValue<int?>(Columns.IsPointRedeem); }
			set { SetColumnValue(Columns.IsPointRedeem, value); }
		}
		  
		[XmlAttribute("PointRedeemWeight")]
		[Bindable(true)]
		public decimal? PointRedeemWeight 
		{
			get { return GetColumnValue<decimal?>(Columns.PointRedeemWeight); }
			set { SetColumnValue(Columns.PointRedeemWeight, value); }
		}
		  
		[XmlAttribute("IsPackageRedeem")]
		[Bindable(true)]
		public int? IsPackageRedeem 
		{
			get { return GetColumnValue<int?>(Columns.IsPackageRedeem); }
			set { SetColumnValue(Columns.IsPackageRedeem, value); }
		}
		  
		[XmlAttribute("PackageRedeemWeight")]
		[Bindable(true)]
		public decimal? PackageRedeemWeight 
		{
			get { return GetColumnValue<decimal?>(Columns.PackageRedeemWeight); }
			set { SetColumnValue(Columns.PackageRedeemWeight, value); }
		}
		  
		[XmlAttribute("IsDeductionFor2ndSalesPerson")]
		[Bindable(true)]
		public int? IsDeductionFor2ndSalesPerson 
		{
			get { return GetColumnValue<int?>(Columns.IsDeductionFor2ndSalesPerson); }
			set { SetColumnValue(Columns.IsDeductionFor2ndSalesPerson, value); }
		}
		  
		[XmlAttribute("DeductionBy")]
		[Bindable(true)]
		public string DeductionBy 
		{
			get { return GetColumnValue<string>(Columns.DeductionBy); }
			set { SetColumnValue(Columns.DeductionBy, value); }
		}
		  
		[XmlAttribute("DeductionValue")]
		[Bindable(true)]
		public decimal? DeductionValue 
		{
			get { return GetColumnValue<decimal?>(Columns.DeductionValue); }
			set { SetColumnValue(Columns.DeductionValue, value); }
		}
		  
		[XmlAttribute("SalesGroupID")]
		[Bindable(true)]
		public int? SalesGroupID 
		{
			get { return GetColumnValue<int?>(Columns.SalesGroupID); }
			set { SetColumnValue(Columns.SalesGroupID, value); }
		}
		  
		[XmlAttribute("CommissionBy")]
		[Bindable(true)]
		public string CommissionBy 
		{
			get { return GetColumnValue<string>(Columns.CommissionBy); }
			set { SetColumnValue(Columns.CommissionBy, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
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
		  
		[XmlAttribute("Userfld6")]
		[Bindable(true)]
		public string Userfld6 
		{
			get { return GetColumnValue<string>(Columns.Userfld6); }
			set { SetColumnValue(Columns.Userfld6, value); }
		}
		  
		[XmlAttribute("Userfld7")]
		[Bindable(true)]
		public string Userfld7 
		{
			get { return GetColumnValue<string>(Columns.Userfld7); }
			set { SetColumnValue(Columns.Userfld7, value); }
		}
		  
		[XmlAttribute("Userfld8")]
		[Bindable(true)]
		public string Userfld8 
		{
			get { return GetColumnValue<string>(Columns.Userfld8); }
			set { SetColumnValue(Columns.Userfld8, value); }
		}
		  
		[XmlAttribute("Userfld9")]
		[Bindable(true)]
		public string Userfld9 
		{
			get { return GetColumnValue<string>(Columns.Userfld9); }
			set { SetColumnValue(Columns.Userfld9, value); }
		}
		  
		[XmlAttribute("Userfld10")]
		[Bindable(true)]
		public string Userfld10 
		{
			get { return GetColumnValue<string>(Columns.Userfld10); }
			set { SetColumnValue(Columns.Userfld10, value); }
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
		public static void Insert(string varSchemeName,int? varIsProduct,decimal? varProductWeight,int? varIsService,decimal? varServiceWeight,int? varIsPointSold,decimal? varPointSoldWeight,int? varIsPackageSold,decimal? varPackageSoldWeight,int? varIsPointRedeem,decimal? varPointRedeemWeight,int? varIsPackageRedeem,decimal? varPackageRedeemWeight,int? varIsDeductionFor2ndSalesPerson,string varDeductionBy,decimal? varDeductionValue,int? varSalesGroupID,string varCommissionBy,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5)
		{
			CommissionHdr item = new CommissionHdr();
			
			item.SchemeName = varSchemeName;
			
			item.IsProduct = varIsProduct;
			
			item.ProductWeight = varProductWeight;
			
			item.IsService = varIsService;
			
			item.ServiceWeight = varServiceWeight;
			
			item.IsPointSold = varIsPointSold;
			
			item.PointSoldWeight = varPointSoldWeight;
			
			item.IsPackageSold = varIsPackageSold;
			
			item.PackageSoldWeight = varPackageSoldWeight;
			
			item.IsPointRedeem = varIsPointRedeem;
			
			item.PointRedeemWeight = varPointRedeemWeight;
			
			item.IsPackageRedeem = varIsPackageRedeem;
			
			item.PackageRedeemWeight = varPackageRedeemWeight;
			
			item.IsDeductionFor2ndSalesPerson = varIsDeductionFor2ndSalesPerson;
			
			item.DeductionBy = varDeductionBy;
			
			item.DeductionValue = varDeductionValue;
			
			item.SalesGroupID = varSalesGroupID;
			
			item.CommissionBy = varCommissionBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.Userfld1 = varUserfld1;
			
			item.Userfld2 = varUserfld2;
			
			item.Userfld3 = varUserfld3;
			
			item.Userfld4 = varUserfld4;
			
			item.Userfld5 = varUserfld5;
			
			item.Userfld6 = varUserfld6;
			
			item.Userfld7 = varUserfld7;
			
			item.Userfld8 = varUserfld8;
			
			item.Userfld9 = varUserfld9;
			
			item.Userfld10 = varUserfld10;
			
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
		public static void Update(int varCommissionHdrID,string varSchemeName,int? varIsProduct,decimal? varProductWeight,int? varIsService,decimal? varServiceWeight,int? varIsPointSold,decimal? varPointSoldWeight,int? varIsPackageSold,decimal? varPackageSoldWeight,int? varIsPointRedeem,decimal? varPointRedeemWeight,int? varIsPackageRedeem,decimal? varPackageRedeemWeight,int? varIsDeductionFor2ndSalesPerson,string varDeductionBy,decimal? varDeductionValue,int? varSalesGroupID,string varCommissionBy,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5)
		{
			CommissionHdr item = new CommissionHdr();
			
				item.CommissionHdrID = varCommissionHdrID;
			
				item.SchemeName = varSchemeName;
			
				item.IsProduct = varIsProduct;
			
				item.ProductWeight = varProductWeight;
			
				item.IsService = varIsService;
			
				item.ServiceWeight = varServiceWeight;
			
				item.IsPointSold = varIsPointSold;
			
				item.PointSoldWeight = varPointSoldWeight;
			
				item.IsPackageSold = varIsPackageSold;
			
				item.PackageSoldWeight = varPackageSoldWeight;
			
				item.IsPointRedeem = varIsPointRedeem;
			
				item.PointRedeemWeight = varPointRedeemWeight;
			
				item.IsPackageRedeem = varIsPackageRedeem;
			
				item.PackageRedeemWeight = varPackageRedeemWeight;
			
				item.IsDeductionFor2ndSalesPerson = varIsDeductionFor2ndSalesPerson;
			
				item.DeductionBy = varDeductionBy;
			
				item.DeductionValue = varDeductionValue;
			
				item.SalesGroupID = varSalesGroupID;
			
				item.CommissionBy = varCommissionBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.Userfld1 = varUserfld1;
			
				item.Userfld2 = varUserfld2;
			
				item.Userfld3 = varUserfld3;
			
				item.Userfld4 = varUserfld4;
			
				item.Userfld5 = varUserfld5;
			
				item.Userfld6 = varUserfld6;
			
				item.Userfld7 = varUserfld7;
			
				item.Userfld8 = varUserfld8;
			
				item.Userfld9 = varUserfld9;
			
				item.Userfld10 = varUserfld10;
			
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
        
        
        public static TableSchema.TableColumn CommissionHdrIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn SchemeNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn IsProductColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ProductWeightColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IsServiceColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ServiceWeightColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn IsPointSoldColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn PointSoldWeightColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn IsPackageSoldColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn PackageSoldWeightColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn IsPointRedeemColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn PointRedeemWeightColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn IsPackageRedeemColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn PackageRedeemWeightColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn IsDeductionFor2ndSalesPersonColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn DeductionByColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn DeductionValueColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn SalesGroupIDColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn CommissionByColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld1Column
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld2Column
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld3Column
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld4Column
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld5Column
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld6Column
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld7Column
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld8Column
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld9Column
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld10Column
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag1Column
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag2Column
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag3Column
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag4Column
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag5Column
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat1Column
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat2Column
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat3Column
        {
            get { return Schema.Columns[41]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat4Column
        {
            get { return Schema.Columns[42]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat5Column
        {
            get { return Schema.Columns[43]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint1Column
        {
            get { return Schema.Columns[44]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint2Column
        {
            get { return Schema.Columns[45]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint3Column
        {
            get { return Schema.Columns[46]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint4Column
        {
            get { return Schema.Columns[47]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint5Column
        {
            get { return Schema.Columns[48]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string CommissionHdrID = @"CommissionHdrID";
			 public static string SchemeName = @"SchemeName";
			 public static string IsProduct = @"IsProduct";
			 public static string ProductWeight = @"ProductWeight";
			 public static string IsService = @"IsService";
			 public static string ServiceWeight = @"ServiceWeight";
			 public static string IsPointSold = @"IsPointSold";
			 public static string PointSoldWeight = @"PointSoldWeight";
			 public static string IsPackageSold = @"IsPackageSold";
			 public static string PackageSoldWeight = @"PackageSoldWeight";
			 public static string IsPointRedeem = @"IsPointRedeem";
			 public static string PointRedeemWeight = @"PointRedeemWeight";
			 public static string IsPackageRedeem = @"IsPackageRedeem";
			 public static string PackageRedeemWeight = @"PackageRedeemWeight";
			 public static string IsDeductionFor2ndSalesPerson = @"IsDeductionFor2ndSalesPerson";
			 public static string DeductionBy = @"DeductionBy";
			 public static string DeductionValue = @"DeductionValue";
			 public static string SalesGroupID = @"SalesGroupID";
			 public static string CommissionBy = @"CommissionBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string Userfld1 = @"userfld1";
			 public static string Userfld2 = @"userfld2";
			 public static string Userfld3 = @"userfld3";
			 public static string Userfld4 = @"userfld4";
			 public static string Userfld5 = @"userfld5";
			 public static string Userfld6 = @"userfld6";
			 public static string Userfld7 = @"userfld7";
			 public static string Userfld8 = @"userfld8";
			 public static string Userfld9 = @"userfld9";
			 public static string Userfld10 = @"userfld10";
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
