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
    /// Strongly-typed collection for the ViewDWHourlyPaymentSrc class.
    /// </summary>
    [Serializable]
    public partial class ViewDWHourlyPaymentSrcCollection : ReadOnlyList<ViewDWHourlyPaymentSrc, ViewDWHourlyPaymentSrcCollection>
    {        
        public ViewDWHourlyPaymentSrcCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the viewDW_HourlyPayment_src view.
    /// </summary>
    [Serializable]
    public partial class ViewDWHourlyPaymentSrc : ReadOnlyRecord<ViewDWHourlyPaymentSrc>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("viewDW_HourlyPayment_src", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarOrderDate = new TableSchema.TableColumn(schema);
                colvarOrderDate.ColumnName = "OrderDate";
                colvarOrderDate.DataType = DbType.DateTime;
                colvarOrderDate.MaxLength = 0;
                colvarOrderDate.AutoIncrement = false;
                colvarOrderDate.IsNullable = true;
                colvarOrderDate.IsPrimaryKey = false;
                colvarOrderDate.IsForeignKey = false;
                colvarOrderDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDate);
                
                TableSchema.TableColumn colvarOutletName = new TableSchema.TableColumn(schema);
                colvarOutletName.ColumnName = "OutletName";
                colvarOutletName.DataType = DbType.AnsiString;
                colvarOutletName.MaxLength = 50;
                colvarOutletName.AutoIncrement = false;
                colvarOutletName.IsNullable = true;
                colvarOutletName.IsPrimaryKey = false;
                colvarOutletName.IsForeignKey = false;
                colvarOutletName.IsReadOnly = false;
                
                schema.Columns.Add(colvarOutletName);
                
                TableSchema.TableColumn colvarPayByCash = new TableSchema.TableColumn(schema);
                colvarPayByCash.ColumnName = "PayByCash";
                colvarPayByCash.DataType = DbType.Currency;
                colvarPayByCash.MaxLength = 0;
                colvarPayByCash.AutoIncrement = false;
                colvarPayByCash.IsNullable = true;
                colvarPayByCash.IsPrimaryKey = false;
                colvarPayByCash.IsForeignKey = false;
                colvarPayByCash.IsReadOnly = false;
                
                schema.Columns.Add(colvarPayByCash);
                
                TableSchema.TableColumn colvarPay01 = new TableSchema.TableColumn(schema);
                colvarPay01.ColumnName = "Pay01";
                colvarPay01.DataType = DbType.Currency;
                colvarPay01.MaxLength = 0;
                colvarPay01.AutoIncrement = false;
                colvarPay01.IsNullable = true;
                colvarPay01.IsPrimaryKey = false;
                colvarPay01.IsForeignKey = false;
                colvarPay01.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay01);
                
                TableSchema.TableColumn colvarPay02 = new TableSchema.TableColumn(schema);
                colvarPay02.ColumnName = "Pay02";
                colvarPay02.DataType = DbType.Currency;
                colvarPay02.MaxLength = 0;
                colvarPay02.AutoIncrement = false;
                colvarPay02.IsNullable = true;
                colvarPay02.IsPrimaryKey = false;
                colvarPay02.IsForeignKey = false;
                colvarPay02.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay02);
                
                TableSchema.TableColumn colvarPay03 = new TableSchema.TableColumn(schema);
                colvarPay03.ColumnName = "Pay03";
                colvarPay03.DataType = DbType.Currency;
                colvarPay03.MaxLength = 0;
                colvarPay03.AutoIncrement = false;
                colvarPay03.IsNullable = true;
                colvarPay03.IsPrimaryKey = false;
                colvarPay03.IsForeignKey = false;
                colvarPay03.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay03);
                
                TableSchema.TableColumn colvarPay04 = new TableSchema.TableColumn(schema);
                colvarPay04.ColumnName = "Pay04";
                colvarPay04.DataType = DbType.Currency;
                colvarPay04.MaxLength = 0;
                colvarPay04.AutoIncrement = false;
                colvarPay04.IsNullable = true;
                colvarPay04.IsPrimaryKey = false;
                colvarPay04.IsForeignKey = false;
                colvarPay04.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay04);
                
                TableSchema.TableColumn colvarPay05 = new TableSchema.TableColumn(schema);
                colvarPay05.ColumnName = "Pay05";
                colvarPay05.DataType = DbType.Currency;
                colvarPay05.MaxLength = 0;
                colvarPay05.AutoIncrement = false;
                colvarPay05.IsNullable = true;
                colvarPay05.IsPrimaryKey = false;
                colvarPay05.IsForeignKey = false;
                colvarPay05.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay05);
                
                TableSchema.TableColumn colvarPay06 = new TableSchema.TableColumn(schema);
                colvarPay06.ColumnName = "Pay06";
                colvarPay06.DataType = DbType.Currency;
                colvarPay06.MaxLength = 0;
                colvarPay06.AutoIncrement = false;
                colvarPay06.IsNullable = true;
                colvarPay06.IsPrimaryKey = false;
                colvarPay06.IsForeignKey = false;
                colvarPay06.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay06);
                
                TableSchema.TableColumn colvarPay07 = new TableSchema.TableColumn(schema);
                colvarPay07.ColumnName = "Pay07";
                colvarPay07.DataType = DbType.Currency;
                colvarPay07.MaxLength = 0;
                colvarPay07.AutoIncrement = false;
                colvarPay07.IsNullable = true;
                colvarPay07.IsPrimaryKey = false;
                colvarPay07.IsForeignKey = false;
                colvarPay07.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay07);
                
                TableSchema.TableColumn colvarPay08 = new TableSchema.TableColumn(schema);
                colvarPay08.ColumnName = "Pay08";
                colvarPay08.DataType = DbType.Currency;
                colvarPay08.MaxLength = 0;
                colvarPay08.AutoIncrement = false;
                colvarPay08.IsNullable = true;
                colvarPay08.IsPrimaryKey = false;
                colvarPay08.IsForeignKey = false;
                colvarPay08.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay08);
                
                TableSchema.TableColumn colvarPay09 = new TableSchema.TableColumn(schema);
                colvarPay09.ColumnName = "Pay09";
                colvarPay09.DataType = DbType.Currency;
                colvarPay09.MaxLength = 0;
                colvarPay09.AutoIncrement = false;
                colvarPay09.IsNullable = true;
                colvarPay09.IsPrimaryKey = false;
                colvarPay09.IsForeignKey = false;
                colvarPay09.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay09);
                
                TableSchema.TableColumn colvarPay10 = new TableSchema.TableColumn(schema);
                colvarPay10.ColumnName = "Pay10";
                colvarPay10.DataType = DbType.Currency;
                colvarPay10.MaxLength = 0;
                colvarPay10.AutoIncrement = false;
                colvarPay10.IsNullable = true;
                colvarPay10.IsPrimaryKey = false;
                colvarPay10.IsForeignKey = false;
                colvarPay10.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay10);
                
                TableSchema.TableColumn colvarPay11 = new TableSchema.TableColumn(schema);
                colvarPay11.ColumnName = "Pay11";
                colvarPay11.DataType = DbType.Currency;
                colvarPay11.MaxLength = 0;
                colvarPay11.AutoIncrement = false;
                colvarPay11.IsNullable = true;
                colvarPay11.IsPrimaryKey = false;
                colvarPay11.IsForeignKey = false;
                colvarPay11.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay11);
                
                TableSchema.TableColumn colvarPay12 = new TableSchema.TableColumn(schema);
                colvarPay12.ColumnName = "Pay12";
                colvarPay12.DataType = DbType.Currency;
                colvarPay12.MaxLength = 0;
                colvarPay12.AutoIncrement = false;
                colvarPay12.IsNullable = true;
                colvarPay12.IsPrimaryKey = false;
                colvarPay12.IsForeignKey = false;
                colvarPay12.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay12);
                
                TableSchema.TableColumn colvarPay13 = new TableSchema.TableColumn(schema);
                colvarPay13.ColumnName = "Pay13";
                colvarPay13.DataType = DbType.Currency;
                colvarPay13.MaxLength = 0;
                colvarPay13.AutoIncrement = false;
                colvarPay13.IsNullable = true;
                colvarPay13.IsPrimaryKey = false;
                colvarPay13.IsForeignKey = false;
                colvarPay13.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay13);
                
                TableSchema.TableColumn colvarPay14 = new TableSchema.TableColumn(schema);
                colvarPay14.ColumnName = "Pay14";
                colvarPay14.DataType = DbType.Currency;
                colvarPay14.MaxLength = 0;
                colvarPay14.AutoIncrement = false;
                colvarPay14.IsNullable = true;
                colvarPay14.IsPrimaryKey = false;
                colvarPay14.IsForeignKey = false;
                colvarPay14.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay14);
                
                TableSchema.TableColumn colvarPay15 = new TableSchema.TableColumn(schema);
                colvarPay15.ColumnName = "Pay15";
                colvarPay15.DataType = DbType.Currency;
                colvarPay15.MaxLength = 0;
                colvarPay15.AutoIncrement = false;
                colvarPay15.IsNullable = true;
                colvarPay15.IsPrimaryKey = false;
                colvarPay15.IsForeignKey = false;
                colvarPay15.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay15);
                
                TableSchema.TableColumn colvarPay16 = new TableSchema.TableColumn(schema);
                colvarPay16.ColumnName = "Pay16";
                colvarPay16.DataType = DbType.Currency;
                colvarPay16.MaxLength = 0;
                colvarPay16.AutoIncrement = false;
                colvarPay16.IsNullable = true;
                colvarPay16.IsPrimaryKey = false;
                colvarPay16.IsForeignKey = false;
                colvarPay16.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay16);
                
                TableSchema.TableColumn colvarPay17 = new TableSchema.TableColumn(schema);
                colvarPay17.ColumnName = "Pay17";
                colvarPay17.DataType = DbType.Currency;
                colvarPay17.MaxLength = 0;
                colvarPay17.AutoIncrement = false;
                colvarPay17.IsNullable = true;
                colvarPay17.IsPrimaryKey = false;
                colvarPay17.IsForeignKey = false;
                colvarPay17.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay17);
                
                TableSchema.TableColumn colvarPay18 = new TableSchema.TableColumn(schema);
                colvarPay18.ColumnName = "Pay18";
                colvarPay18.DataType = DbType.Currency;
                colvarPay18.MaxLength = 0;
                colvarPay18.AutoIncrement = false;
                colvarPay18.IsNullable = true;
                colvarPay18.IsPrimaryKey = false;
                colvarPay18.IsForeignKey = false;
                colvarPay18.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay18);
                
                TableSchema.TableColumn colvarPay19 = new TableSchema.TableColumn(schema);
                colvarPay19.ColumnName = "Pay19";
                colvarPay19.DataType = DbType.Currency;
                colvarPay19.MaxLength = 0;
                colvarPay19.AutoIncrement = false;
                colvarPay19.IsNullable = true;
                colvarPay19.IsPrimaryKey = false;
                colvarPay19.IsForeignKey = false;
                colvarPay19.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay19);
                
                TableSchema.TableColumn colvarPay20 = new TableSchema.TableColumn(schema);
                colvarPay20.ColumnName = "Pay20";
                colvarPay20.DataType = DbType.Currency;
                colvarPay20.MaxLength = 0;
                colvarPay20.AutoIncrement = false;
                colvarPay20.IsNullable = true;
                colvarPay20.IsPrimaryKey = false;
                colvarPay20.IsForeignKey = false;
                colvarPay20.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay20);
                
                TableSchema.TableColumn colvarPay21 = new TableSchema.TableColumn(schema);
                colvarPay21.ColumnName = "Pay21";
                colvarPay21.DataType = DbType.Currency;
                colvarPay21.MaxLength = 0;
                colvarPay21.AutoIncrement = false;
                colvarPay21.IsNullable = true;
                colvarPay21.IsPrimaryKey = false;
                colvarPay21.IsForeignKey = false;
                colvarPay21.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay21);
                
                TableSchema.TableColumn colvarPay22 = new TableSchema.TableColumn(schema);
                colvarPay22.ColumnName = "Pay22";
                colvarPay22.DataType = DbType.Currency;
                colvarPay22.MaxLength = 0;
                colvarPay22.AutoIncrement = false;
                colvarPay22.IsNullable = true;
                colvarPay22.IsPrimaryKey = false;
                colvarPay22.IsForeignKey = false;
                colvarPay22.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay22);
                
                TableSchema.TableColumn colvarPay23 = new TableSchema.TableColumn(schema);
                colvarPay23.ColumnName = "Pay23";
                colvarPay23.DataType = DbType.Currency;
                colvarPay23.MaxLength = 0;
                colvarPay23.AutoIncrement = false;
                colvarPay23.IsNullable = true;
                colvarPay23.IsPrimaryKey = false;
                colvarPay23.IsForeignKey = false;
                colvarPay23.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay23);
                
                TableSchema.TableColumn colvarPay24 = new TableSchema.TableColumn(schema);
                colvarPay24.ColumnName = "Pay24";
                colvarPay24.DataType = DbType.Currency;
                colvarPay24.MaxLength = 0;
                colvarPay24.AutoIncrement = false;
                colvarPay24.IsNullable = true;
                colvarPay24.IsPrimaryKey = false;
                colvarPay24.IsForeignKey = false;
                colvarPay24.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay24);
                
                TableSchema.TableColumn colvarPay25 = new TableSchema.TableColumn(schema);
                colvarPay25.ColumnName = "Pay25";
                colvarPay25.DataType = DbType.Currency;
                colvarPay25.MaxLength = 0;
                colvarPay25.AutoIncrement = false;
                colvarPay25.IsNullable = true;
                colvarPay25.IsPrimaryKey = false;
                colvarPay25.IsForeignKey = false;
                colvarPay25.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay25);
                
                TableSchema.TableColumn colvarPay26 = new TableSchema.TableColumn(schema);
                colvarPay26.ColumnName = "Pay26";
                colvarPay26.DataType = DbType.Currency;
                colvarPay26.MaxLength = 0;
                colvarPay26.AutoIncrement = false;
                colvarPay26.IsNullable = true;
                colvarPay26.IsPrimaryKey = false;
                colvarPay26.IsForeignKey = false;
                colvarPay26.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay26);
                
                TableSchema.TableColumn colvarPay27 = new TableSchema.TableColumn(schema);
                colvarPay27.ColumnName = "Pay27";
                colvarPay27.DataType = DbType.Currency;
                colvarPay27.MaxLength = 0;
                colvarPay27.AutoIncrement = false;
                colvarPay27.IsNullable = true;
                colvarPay27.IsPrimaryKey = false;
                colvarPay27.IsForeignKey = false;
                colvarPay27.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay27);
                
                TableSchema.TableColumn colvarPay28 = new TableSchema.TableColumn(schema);
                colvarPay28.ColumnName = "Pay28";
                colvarPay28.DataType = DbType.Currency;
                colvarPay28.MaxLength = 0;
                colvarPay28.AutoIncrement = false;
                colvarPay28.IsNullable = true;
                colvarPay28.IsPrimaryKey = false;
                colvarPay28.IsForeignKey = false;
                colvarPay28.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay28);
                
                TableSchema.TableColumn colvarPay29 = new TableSchema.TableColumn(schema);
                colvarPay29.ColumnName = "Pay29";
                colvarPay29.DataType = DbType.Currency;
                colvarPay29.MaxLength = 0;
                colvarPay29.AutoIncrement = false;
                colvarPay29.IsNullable = true;
                colvarPay29.IsPrimaryKey = false;
                colvarPay29.IsForeignKey = false;
                colvarPay29.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay29);
                
                TableSchema.TableColumn colvarPay30 = new TableSchema.TableColumn(schema);
                colvarPay30.ColumnName = "Pay30";
                colvarPay30.DataType = DbType.Currency;
                colvarPay30.MaxLength = 0;
                colvarPay30.AutoIncrement = false;
                colvarPay30.IsNullable = true;
                colvarPay30.IsPrimaryKey = false;
                colvarPay30.IsForeignKey = false;
                colvarPay30.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay30);
                
                TableSchema.TableColumn colvarPay31 = new TableSchema.TableColumn(schema);
                colvarPay31.ColumnName = "Pay31";
                colvarPay31.DataType = DbType.Currency;
                colvarPay31.MaxLength = 0;
                colvarPay31.AutoIncrement = false;
                colvarPay31.IsNullable = true;
                colvarPay31.IsPrimaryKey = false;
                colvarPay31.IsForeignKey = false;
                colvarPay31.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay31);
                
                TableSchema.TableColumn colvarPay32 = new TableSchema.TableColumn(schema);
                colvarPay32.ColumnName = "Pay32";
                colvarPay32.DataType = DbType.Currency;
                colvarPay32.MaxLength = 0;
                colvarPay32.AutoIncrement = false;
                colvarPay32.IsNullable = true;
                colvarPay32.IsPrimaryKey = false;
                colvarPay32.IsForeignKey = false;
                colvarPay32.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay32);
                
                TableSchema.TableColumn colvarPay33 = new TableSchema.TableColumn(schema);
                colvarPay33.ColumnName = "Pay33";
                colvarPay33.DataType = DbType.Currency;
                colvarPay33.MaxLength = 0;
                colvarPay33.AutoIncrement = false;
                colvarPay33.IsNullable = true;
                colvarPay33.IsPrimaryKey = false;
                colvarPay33.IsForeignKey = false;
                colvarPay33.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay33);
                
                TableSchema.TableColumn colvarPay34 = new TableSchema.TableColumn(schema);
                colvarPay34.ColumnName = "Pay34";
                colvarPay34.DataType = DbType.Currency;
                colvarPay34.MaxLength = 0;
                colvarPay34.AutoIncrement = false;
                colvarPay34.IsNullable = true;
                colvarPay34.IsPrimaryKey = false;
                colvarPay34.IsForeignKey = false;
                colvarPay34.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay34);
                
                TableSchema.TableColumn colvarPay35 = new TableSchema.TableColumn(schema);
                colvarPay35.ColumnName = "Pay35";
                colvarPay35.DataType = DbType.Currency;
                colvarPay35.MaxLength = 0;
                colvarPay35.AutoIncrement = false;
                colvarPay35.IsNullable = true;
                colvarPay35.IsPrimaryKey = false;
                colvarPay35.IsForeignKey = false;
                colvarPay35.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay35);
                
                TableSchema.TableColumn colvarPay36 = new TableSchema.TableColumn(schema);
                colvarPay36.ColumnName = "Pay36";
                colvarPay36.DataType = DbType.Currency;
                colvarPay36.MaxLength = 0;
                colvarPay36.AutoIncrement = false;
                colvarPay36.IsNullable = true;
                colvarPay36.IsPrimaryKey = false;
                colvarPay36.IsForeignKey = false;
                colvarPay36.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay36);
                
                TableSchema.TableColumn colvarPay37 = new TableSchema.TableColumn(schema);
                colvarPay37.ColumnName = "Pay37";
                colvarPay37.DataType = DbType.Currency;
                colvarPay37.MaxLength = 0;
                colvarPay37.AutoIncrement = false;
                colvarPay37.IsNullable = true;
                colvarPay37.IsPrimaryKey = false;
                colvarPay37.IsForeignKey = false;
                colvarPay37.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay37);
                
                TableSchema.TableColumn colvarPay38 = new TableSchema.TableColumn(schema);
                colvarPay38.ColumnName = "Pay38";
                colvarPay38.DataType = DbType.Currency;
                colvarPay38.MaxLength = 0;
                colvarPay38.AutoIncrement = false;
                colvarPay38.IsNullable = true;
                colvarPay38.IsPrimaryKey = false;
                colvarPay38.IsForeignKey = false;
                colvarPay38.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay38);
                
                TableSchema.TableColumn colvarPay39 = new TableSchema.TableColumn(schema);
                colvarPay39.ColumnName = "Pay39";
                colvarPay39.DataType = DbType.Currency;
                colvarPay39.MaxLength = 0;
                colvarPay39.AutoIncrement = false;
                colvarPay39.IsNullable = true;
                colvarPay39.IsPrimaryKey = false;
                colvarPay39.IsForeignKey = false;
                colvarPay39.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay39);
                
                TableSchema.TableColumn colvarPay40 = new TableSchema.TableColumn(schema);
                colvarPay40.ColumnName = "Pay40";
                colvarPay40.DataType = DbType.Currency;
                colvarPay40.MaxLength = 0;
                colvarPay40.AutoIncrement = false;
                colvarPay40.IsNullable = true;
                colvarPay40.IsPrimaryKey = false;
                colvarPay40.IsForeignKey = false;
                colvarPay40.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay40);
                
                TableSchema.TableColumn colvarPayOthers = new TableSchema.TableColumn(schema);
                colvarPayOthers.ColumnName = "PayOthers";
                colvarPayOthers.DataType = DbType.Currency;
                colvarPayOthers.MaxLength = 0;
                colvarPayOthers.AutoIncrement = false;
                colvarPayOthers.IsNullable = true;
                colvarPayOthers.IsPrimaryKey = false;
                colvarPayOthers.IsForeignKey = false;
                colvarPayOthers.IsReadOnly = false;
                
                schema.Columns.Add(colvarPayOthers);
                
                TableSchema.TableColumn colvarPointAllocated = new TableSchema.TableColumn(schema);
                colvarPointAllocated.ColumnName = "PointAllocated";
                colvarPointAllocated.DataType = DbType.Int32;
                colvarPointAllocated.MaxLength = 0;
                colvarPointAllocated.AutoIncrement = false;
                colvarPointAllocated.IsNullable = false;
                colvarPointAllocated.IsPrimaryKey = false;
                colvarPointAllocated.IsForeignKey = false;
                colvarPointAllocated.IsReadOnly = false;
                
                schema.Columns.Add(colvarPointAllocated);
                
                TableSchema.TableColumn colvarPayByInstallment = new TableSchema.TableColumn(schema);
                colvarPayByInstallment.ColumnName = "PayByInstallment";
                colvarPayByInstallment.DataType = DbType.Currency;
                colvarPayByInstallment.MaxLength = 0;
                colvarPayByInstallment.AutoIncrement = false;
                colvarPayByInstallment.IsNullable = true;
                colvarPayByInstallment.IsPrimaryKey = false;
                colvarPayByInstallment.IsForeignKey = false;
                colvarPayByInstallment.IsReadOnly = false;
                
                schema.Columns.Add(colvarPayByInstallment);
                
                TableSchema.TableColumn colvarPayByPoint = new TableSchema.TableColumn(schema);
                colvarPayByPoint.ColumnName = "PayByPoint";
                colvarPayByPoint.DataType = DbType.Currency;
                colvarPayByPoint.MaxLength = 0;
                colvarPayByPoint.AutoIncrement = false;
                colvarPayByPoint.IsNullable = true;
                colvarPayByPoint.IsPrimaryKey = false;
                colvarPayByPoint.IsForeignKey = false;
                colvarPayByPoint.IsReadOnly = false;
                
                schema.Columns.Add(colvarPayByPoint);
                
                TableSchema.TableColumn colvarTotalPayment = new TableSchema.TableColumn(schema);
                colvarTotalPayment.ColumnName = "TotalPayment";
                colvarTotalPayment.DataType = DbType.Currency;
                colvarTotalPayment.MaxLength = 0;
                colvarTotalPayment.AutoIncrement = false;
                colvarTotalPayment.IsNullable = true;
                colvarTotalPayment.IsPrimaryKey = false;
                colvarTotalPayment.IsForeignKey = false;
                colvarTotalPayment.IsReadOnly = false;
                
                schema.Columns.Add(colvarTotalPayment);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("viewDW_HourlyPayment_src",schema);
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
	    public ViewDWHourlyPaymentSrc()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewDWHourlyPaymentSrc(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewDWHourlyPaymentSrc(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewDWHourlyPaymentSrc(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("OrderDate")]
        [Bindable(true)]
        public DateTime? OrderDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("OrderDate");
		    }
            set 
		    {
			    SetColumnValue("OrderDate", value);
            }
        }
	      
        [XmlAttribute("OutletName")]
        [Bindable(true)]
        public string OutletName 
	    {
		    get
		    {
			    return GetColumnValue<string>("OutletName");
		    }
            set 
		    {
			    SetColumnValue("OutletName", value);
            }
        }
	      
        [XmlAttribute("PayByCash")]
        [Bindable(true)]
        public decimal? PayByCash 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("PayByCash");
		    }
            set 
		    {
			    SetColumnValue("PayByCash", value);
            }
        }
	      
        [XmlAttribute("Pay01")]
        [Bindable(true)]
        public decimal? Pay01 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay01");
		    }
            set 
		    {
			    SetColumnValue("Pay01", value);
            }
        }
	      
        [XmlAttribute("Pay02")]
        [Bindable(true)]
        public decimal? Pay02 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay02");
		    }
            set 
		    {
			    SetColumnValue("Pay02", value);
            }
        }
	      
        [XmlAttribute("Pay03")]
        [Bindable(true)]
        public decimal? Pay03 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay03");
		    }
            set 
		    {
			    SetColumnValue("Pay03", value);
            }
        }
	      
        [XmlAttribute("Pay04")]
        [Bindable(true)]
        public decimal? Pay04 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay04");
		    }
            set 
		    {
			    SetColumnValue("Pay04", value);
            }
        }
	      
        [XmlAttribute("Pay05")]
        [Bindable(true)]
        public decimal? Pay05 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay05");
		    }
            set 
		    {
			    SetColumnValue("Pay05", value);
            }
        }
	      
        [XmlAttribute("Pay06")]
        [Bindable(true)]
        public decimal? Pay06 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay06");
		    }
            set 
		    {
			    SetColumnValue("Pay06", value);
            }
        }
	      
        [XmlAttribute("Pay07")]
        [Bindable(true)]
        public decimal? Pay07 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay07");
		    }
            set 
		    {
			    SetColumnValue("Pay07", value);
            }
        }
	      
        [XmlAttribute("Pay08")]
        [Bindable(true)]
        public decimal? Pay08 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay08");
		    }
            set 
		    {
			    SetColumnValue("Pay08", value);
            }
        }
	      
        [XmlAttribute("Pay09")]
        [Bindable(true)]
        public decimal? Pay09 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay09");
		    }
            set 
		    {
			    SetColumnValue("Pay09", value);
            }
        }
	      
        [XmlAttribute("Pay10")]
        [Bindable(true)]
        public decimal? Pay10 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay10");
		    }
            set 
		    {
			    SetColumnValue("Pay10", value);
            }
        }
	      
        [XmlAttribute("Pay11")]
        [Bindable(true)]
        public decimal? Pay11 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay11");
		    }
            set 
		    {
			    SetColumnValue("Pay11", value);
            }
        }
	      
        [XmlAttribute("Pay12")]
        [Bindable(true)]
        public decimal? Pay12 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay12");
		    }
            set 
		    {
			    SetColumnValue("Pay12", value);
            }
        }
	      
        [XmlAttribute("Pay13")]
        [Bindable(true)]
        public decimal? Pay13 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay13");
		    }
            set 
		    {
			    SetColumnValue("Pay13", value);
            }
        }
	      
        [XmlAttribute("Pay14")]
        [Bindable(true)]
        public decimal? Pay14 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay14");
		    }
            set 
		    {
			    SetColumnValue("Pay14", value);
            }
        }
	      
        [XmlAttribute("Pay15")]
        [Bindable(true)]
        public decimal? Pay15 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay15");
		    }
            set 
		    {
			    SetColumnValue("Pay15", value);
            }
        }
	      
        [XmlAttribute("Pay16")]
        [Bindable(true)]
        public decimal? Pay16 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay16");
		    }
            set 
		    {
			    SetColumnValue("Pay16", value);
            }
        }
	      
        [XmlAttribute("Pay17")]
        [Bindable(true)]
        public decimal? Pay17 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay17");
		    }
            set 
		    {
			    SetColumnValue("Pay17", value);
            }
        }
	      
        [XmlAttribute("Pay18")]
        [Bindable(true)]
        public decimal? Pay18 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay18");
		    }
            set 
		    {
			    SetColumnValue("Pay18", value);
            }
        }
	      
        [XmlAttribute("Pay19")]
        [Bindable(true)]
        public decimal? Pay19 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay19");
		    }
            set 
		    {
			    SetColumnValue("Pay19", value);
            }
        }
	      
        [XmlAttribute("Pay20")]
        [Bindable(true)]
        public decimal? Pay20 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay20");
		    }
            set 
		    {
			    SetColumnValue("Pay20", value);
            }
        }
	      
        [XmlAttribute("Pay21")]
        [Bindable(true)]
        public decimal? Pay21 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay21");
		    }
            set 
		    {
			    SetColumnValue("Pay21", value);
            }
        }
	      
        [XmlAttribute("Pay22")]
        [Bindable(true)]
        public decimal? Pay22 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay22");
		    }
            set 
		    {
			    SetColumnValue("Pay22", value);
            }
        }
	      
        [XmlAttribute("Pay23")]
        [Bindable(true)]
        public decimal? Pay23 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay23");
		    }
            set 
		    {
			    SetColumnValue("Pay23", value);
            }
        }
	      
        [XmlAttribute("Pay24")]
        [Bindable(true)]
        public decimal? Pay24 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay24");
		    }
            set 
		    {
			    SetColumnValue("Pay24", value);
            }
        }
	      
        [XmlAttribute("Pay25")]
        [Bindable(true)]
        public decimal? Pay25 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay25");
		    }
            set 
		    {
			    SetColumnValue("Pay25", value);
            }
        }
	      
        [XmlAttribute("Pay26")]
        [Bindable(true)]
        public decimal? Pay26 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay26");
		    }
            set 
		    {
			    SetColumnValue("Pay26", value);
            }
        }
	      
        [XmlAttribute("Pay27")]
        [Bindable(true)]
        public decimal? Pay27 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay27");
		    }
            set 
		    {
			    SetColumnValue("Pay27", value);
            }
        }
	      
        [XmlAttribute("Pay28")]
        [Bindable(true)]
        public decimal? Pay28 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay28");
		    }
            set 
		    {
			    SetColumnValue("Pay28", value);
            }
        }
	      
        [XmlAttribute("Pay29")]
        [Bindable(true)]
        public decimal? Pay29 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay29");
		    }
            set 
		    {
			    SetColumnValue("Pay29", value);
            }
        }
	      
        [XmlAttribute("Pay30")]
        [Bindable(true)]
        public decimal? Pay30 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay30");
		    }
            set 
		    {
			    SetColumnValue("Pay30", value);
            }
        }
	      
        [XmlAttribute("Pay31")]
        [Bindable(true)]
        public decimal? Pay31 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay31");
		    }
            set 
		    {
			    SetColumnValue("Pay31", value);
            }
        }
	      
        [XmlAttribute("Pay32")]
        [Bindable(true)]
        public decimal? Pay32 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay32");
		    }
            set 
		    {
			    SetColumnValue("Pay32", value);
            }
        }
	      
        [XmlAttribute("Pay33")]
        [Bindable(true)]
        public decimal? Pay33 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay33");
		    }
            set 
		    {
			    SetColumnValue("Pay33", value);
            }
        }
	      
        [XmlAttribute("Pay34")]
        [Bindable(true)]
        public decimal? Pay34 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay34");
		    }
            set 
		    {
			    SetColumnValue("Pay34", value);
            }
        }
	      
        [XmlAttribute("Pay35")]
        [Bindable(true)]
        public decimal? Pay35 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay35");
		    }
            set 
		    {
			    SetColumnValue("Pay35", value);
            }
        }
	      
        [XmlAttribute("Pay36")]
        [Bindable(true)]
        public decimal? Pay36 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay36");
		    }
            set 
		    {
			    SetColumnValue("Pay36", value);
            }
        }
	      
        [XmlAttribute("Pay37")]
        [Bindable(true)]
        public decimal? Pay37 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay37");
		    }
            set 
		    {
			    SetColumnValue("Pay37", value);
            }
        }
	      
        [XmlAttribute("Pay38")]
        [Bindable(true)]
        public decimal? Pay38 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay38");
		    }
            set 
		    {
			    SetColumnValue("Pay38", value);
            }
        }
	      
        [XmlAttribute("Pay39")]
        [Bindable(true)]
        public decimal? Pay39 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay39");
		    }
            set 
		    {
			    SetColumnValue("Pay39", value);
            }
        }
	      
        [XmlAttribute("Pay40")]
        [Bindable(true)]
        public decimal? Pay40 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Pay40");
		    }
            set 
		    {
			    SetColumnValue("Pay40", value);
            }
        }
	      
        [XmlAttribute("PayOthers")]
        [Bindable(true)]
        public decimal? PayOthers 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("PayOthers");
		    }
            set 
		    {
			    SetColumnValue("PayOthers", value);
            }
        }
	      
        [XmlAttribute("PointAllocated")]
        [Bindable(true)]
        public int PointAllocated 
	    {
		    get
		    {
			    return GetColumnValue<int>("PointAllocated");
		    }
            set 
		    {
			    SetColumnValue("PointAllocated", value);
            }
        }
	      
        [XmlAttribute("PayByInstallment")]
        [Bindable(true)]
        public decimal? PayByInstallment 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("PayByInstallment");
		    }
            set 
		    {
			    SetColumnValue("PayByInstallment", value);
            }
        }
	      
        [XmlAttribute("PayByPoint")]
        [Bindable(true)]
        public decimal? PayByPoint 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("PayByPoint");
		    }
            set 
		    {
			    SetColumnValue("PayByPoint", value);
            }
        }
	      
        [XmlAttribute("TotalPayment")]
        [Bindable(true)]
        public decimal? TotalPayment 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("TotalPayment");
		    }
            set 
		    {
			    SetColumnValue("TotalPayment", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string OrderDate = @"OrderDate";
            
            public static string OutletName = @"OutletName";
            
            public static string PayByCash = @"PayByCash";
            
            public static string Pay01 = @"Pay01";
            
            public static string Pay02 = @"Pay02";
            
            public static string Pay03 = @"Pay03";
            
            public static string Pay04 = @"Pay04";
            
            public static string Pay05 = @"Pay05";
            
            public static string Pay06 = @"Pay06";
            
            public static string Pay07 = @"Pay07";
            
            public static string Pay08 = @"Pay08";
            
            public static string Pay09 = @"Pay09";
            
            public static string Pay10 = @"Pay10";
            
            public static string Pay11 = @"Pay11";
            
            public static string Pay12 = @"Pay12";
            
            public static string Pay13 = @"Pay13";
            
            public static string Pay14 = @"Pay14";
            
            public static string Pay15 = @"Pay15";
            
            public static string Pay16 = @"Pay16";
            
            public static string Pay17 = @"Pay17";
            
            public static string Pay18 = @"Pay18";
            
            public static string Pay19 = @"Pay19";
            
            public static string Pay20 = @"Pay20";
            
            public static string Pay21 = @"Pay21";
            
            public static string Pay22 = @"Pay22";
            
            public static string Pay23 = @"Pay23";
            
            public static string Pay24 = @"Pay24";
            
            public static string Pay25 = @"Pay25";
            
            public static string Pay26 = @"Pay26";
            
            public static string Pay27 = @"Pay27";
            
            public static string Pay28 = @"Pay28";
            
            public static string Pay29 = @"Pay29";
            
            public static string Pay30 = @"Pay30";
            
            public static string Pay31 = @"Pay31";
            
            public static string Pay32 = @"Pay32";
            
            public static string Pay33 = @"Pay33";
            
            public static string Pay34 = @"Pay34";
            
            public static string Pay35 = @"Pay35";
            
            public static string Pay36 = @"Pay36";
            
            public static string Pay37 = @"Pay37";
            
            public static string Pay38 = @"Pay38";
            
            public static string Pay39 = @"Pay39";
            
            public static string Pay40 = @"Pay40";
            
            public static string PayOthers = @"PayOthers";
            
            public static string PointAllocated = @"PointAllocated";
            
            public static string PayByInstallment = @"PayByInstallment";
            
            public static string PayByPoint = @"PayByPoint";
            
            public static string TotalPayment = @"TotalPayment";
            
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
