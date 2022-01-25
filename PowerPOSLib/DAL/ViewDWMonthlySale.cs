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
    /// Strongly-typed collection for the ViewDWMonthlySale class.
    /// </summary>
    [Serializable]
    public partial class ViewDWMonthlySaleCollection : ReadOnlyList<ViewDWMonthlySale, ViewDWMonthlySaleCollection>
    {        
        public ViewDWMonthlySaleCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the viewDW_MonthlySales view.
    /// </summary>
    [Serializable]
    public partial class ViewDWMonthlySale : ReadOnlyRecord<ViewDWMonthlySale>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("viewDW_MonthlySales", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarOrderdate = new TableSchema.TableColumn(schema);
                colvarOrderdate.ColumnName = "orderdate";
                colvarOrderdate.DataType = DbType.AnsiString;
                colvarOrderdate.MaxLength = 0;
                colvarOrderdate.AutoIncrement = false;
                colvarOrderdate.IsNullable = true;
                colvarOrderdate.IsPrimaryKey = false;
                colvarOrderdate.IsForeignKey = false;
                colvarOrderdate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderdate);
                
                TableSchema.TableColumn colvarOutletname = new TableSchema.TableColumn(schema);
                colvarOutletname.ColumnName = "outletname";
                colvarOutletname.DataType = DbType.AnsiString;
                colvarOutletname.MaxLength = 50;
                colvarOutletname.AutoIncrement = false;
                colvarOutletname.IsNullable = false;
                colvarOutletname.IsPrimaryKey = false;
                colvarOutletname.IsForeignKey = false;
                colvarOutletname.IsReadOnly = false;
                
                schema.Columns.Add(colvarOutletname);
                
                TableSchema.TableColumn colvarPax = new TableSchema.TableColumn(schema);
                colvarPax.ColumnName = "pax";
                colvarPax.DataType = DbType.Int32;
                colvarPax.MaxLength = 0;
                colvarPax.AutoIncrement = false;
                colvarPax.IsNullable = true;
                colvarPax.IsPrimaryKey = false;
                colvarPax.IsForeignKey = false;
                colvarPax.IsReadOnly = false;
                
                schema.Columns.Add(colvarPax);
                
                TableSchema.TableColumn colvarBill = new TableSchema.TableColumn(schema);
                colvarBill.ColumnName = "bill";
                colvarBill.DataType = DbType.Int32;
                colvarBill.MaxLength = 0;
                colvarBill.AutoIncrement = false;
                colvarBill.IsNullable = true;
                colvarBill.IsPrimaryKey = false;
                colvarBill.IsForeignKey = false;
                colvarBill.IsReadOnly = false;
                
                schema.Columns.Add(colvarBill);
                
                TableSchema.TableColumn colvarGrossamount = new TableSchema.TableColumn(schema);
                colvarGrossamount.ColumnName = "grossamount";
                colvarGrossamount.DataType = DbType.Currency;
                colvarGrossamount.MaxLength = 0;
                colvarGrossamount.AutoIncrement = false;
                colvarGrossamount.IsNullable = true;
                colvarGrossamount.IsPrimaryKey = false;
                colvarGrossamount.IsForeignKey = false;
                colvarGrossamount.IsReadOnly = false;
                
                schema.Columns.Add(colvarGrossamount);
                
                TableSchema.TableColumn colvarDisc = new TableSchema.TableColumn(schema);
                colvarDisc.ColumnName = "disc";
                colvarDisc.DataType = DbType.Currency;
                colvarDisc.MaxLength = 0;
                colvarDisc.AutoIncrement = false;
                colvarDisc.IsNullable = true;
                colvarDisc.IsPrimaryKey = false;
                colvarDisc.IsForeignKey = false;
                colvarDisc.IsReadOnly = false;
                
                schema.Columns.Add(colvarDisc);
                
                TableSchema.TableColumn colvarAfterdisc = new TableSchema.TableColumn(schema);
                colvarAfterdisc.ColumnName = "afterdisc";
                colvarAfterdisc.DataType = DbType.Currency;
                colvarAfterdisc.MaxLength = 0;
                colvarAfterdisc.AutoIncrement = false;
                colvarAfterdisc.IsNullable = true;
                colvarAfterdisc.IsPrimaryKey = false;
                colvarAfterdisc.IsForeignKey = false;
                colvarAfterdisc.IsReadOnly = false;
                
                schema.Columns.Add(colvarAfterdisc);
                
                TableSchema.TableColumn colvarSvccharge = new TableSchema.TableColumn(schema);
                colvarSvccharge.ColumnName = "svccharge";
                colvarSvccharge.DataType = DbType.Currency;
                colvarSvccharge.MaxLength = 0;
                colvarSvccharge.AutoIncrement = false;
                colvarSvccharge.IsNullable = true;
                colvarSvccharge.IsPrimaryKey = false;
                colvarSvccharge.IsForeignKey = false;
                colvarSvccharge.IsReadOnly = false;
                
                schema.Columns.Add(colvarSvccharge);
                
                TableSchema.TableColumn colvarBefgst = new TableSchema.TableColumn(schema);
                colvarBefgst.ColumnName = "befgst";
                colvarBefgst.DataType = DbType.Currency;
                colvarBefgst.MaxLength = 0;
                colvarBefgst.AutoIncrement = false;
                colvarBefgst.IsNullable = true;
                colvarBefgst.IsPrimaryKey = false;
                colvarBefgst.IsForeignKey = false;
                colvarBefgst.IsReadOnly = false;
                
                schema.Columns.Add(colvarBefgst);
                
                TableSchema.TableColumn colvarGst = new TableSchema.TableColumn(schema);
                colvarGst.ColumnName = "gst";
                colvarGst.DataType = DbType.Currency;
                colvarGst.MaxLength = 0;
                colvarGst.AutoIncrement = false;
                colvarGst.IsNullable = true;
                colvarGst.IsPrimaryKey = false;
                colvarGst.IsForeignKey = false;
                colvarGst.IsReadOnly = false;
                
                schema.Columns.Add(colvarGst);
                
                TableSchema.TableColumn colvarRounding = new TableSchema.TableColumn(schema);
                colvarRounding.ColumnName = "rounding";
                colvarRounding.DataType = DbType.Currency;
                colvarRounding.MaxLength = 0;
                colvarRounding.AutoIncrement = false;
                colvarRounding.IsNullable = true;
                colvarRounding.IsPrimaryKey = false;
                colvarRounding.IsForeignKey = false;
                colvarRounding.IsReadOnly = false;
                
                schema.Columns.Add(colvarRounding);
                
                TableSchema.TableColumn colvarNettamount = new TableSchema.TableColumn(schema);
                colvarNettamount.ColumnName = "nettamount";
                colvarNettamount.DataType = DbType.Currency;
                colvarNettamount.MaxLength = 0;
                colvarNettamount.AutoIncrement = false;
                colvarNettamount.IsNullable = true;
                colvarNettamount.IsPrimaryKey = false;
                colvarNettamount.IsForeignKey = false;
                colvarNettamount.IsReadOnly = false;
                
                schema.Columns.Add(colvarNettamount);
                
                TableSchema.TableColumn colvarPointSale = new TableSchema.TableColumn(schema);
                colvarPointSale.ColumnName = "pointSale";
                colvarPointSale.DataType = DbType.Currency;
                colvarPointSale.MaxLength = 0;
                colvarPointSale.AutoIncrement = false;
                colvarPointSale.IsNullable = true;
                colvarPointSale.IsPrimaryKey = false;
                colvarPointSale.IsForeignKey = false;
                colvarPointSale.IsReadOnly = false;
                
                schema.Columns.Add(colvarPointSale);
                
                TableSchema.TableColumn colvarInstallmentPaymentSale = new TableSchema.TableColumn(schema);
                colvarInstallmentPaymentSale.ColumnName = "installmentPaymentSale";
                colvarInstallmentPaymentSale.DataType = DbType.Currency;
                colvarInstallmentPaymentSale.MaxLength = 0;
                colvarInstallmentPaymentSale.AutoIncrement = false;
                colvarInstallmentPaymentSale.IsNullable = true;
                colvarInstallmentPaymentSale.IsPrimaryKey = false;
                colvarInstallmentPaymentSale.IsForeignKey = false;
                colvarInstallmentPaymentSale.IsReadOnly = false;
                
                schema.Columns.Add(colvarInstallmentPaymentSale);
                
                TableSchema.TableColumn colvarPayByCash = new TableSchema.TableColumn(schema);
                colvarPayByCash.ColumnName = "payByCash";
                colvarPayByCash.DataType = DbType.Currency;
                colvarPayByCash.MaxLength = 0;
                colvarPayByCash.AutoIncrement = false;
                colvarPayByCash.IsNullable = true;
                colvarPayByCash.IsPrimaryKey = false;
                colvarPayByCash.IsForeignKey = false;
                colvarPayByCash.IsReadOnly = false;
                
                schema.Columns.Add(colvarPayByCash);
                
                TableSchema.TableColumn colvarPay01 = new TableSchema.TableColumn(schema);
                colvarPay01.ColumnName = "pay01";
                colvarPay01.DataType = DbType.Currency;
                colvarPay01.MaxLength = 0;
                colvarPay01.AutoIncrement = false;
                colvarPay01.IsNullable = true;
                colvarPay01.IsPrimaryKey = false;
                colvarPay01.IsForeignKey = false;
                colvarPay01.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay01);
                
                TableSchema.TableColumn colvarPay02 = new TableSchema.TableColumn(schema);
                colvarPay02.ColumnName = "pay02";
                colvarPay02.DataType = DbType.Currency;
                colvarPay02.MaxLength = 0;
                colvarPay02.AutoIncrement = false;
                colvarPay02.IsNullable = true;
                colvarPay02.IsPrimaryKey = false;
                colvarPay02.IsForeignKey = false;
                colvarPay02.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay02);
                
                TableSchema.TableColumn colvarPay03 = new TableSchema.TableColumn(schema);
                colvarPay03.ColumnName = "pay03";
                colvarPay03.DataType = DbType.Currency;
                colvarPay03.MaxLength = 0;
                colvarPay03.AutoIncrement = false;
                colvarPay03.IsNullable = true;
                colvarPay03.IsPrimaryKey = false;
                colvarPay03.IsForeignKey = false;
                colvarPay03.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay03);
                
                TableSchema.TableColumn colvarPay04 = new TableSchema.TableColumn(schema);
                colvarPay04.ColumnName = "pay04";
                colvarPay04.DataType = DbType.Currency;
                colvarPay04.MaxLength = 0;
                colvarPay04.AutoIncrement = false;
                colvarPay04.IsNullable = true;
                colvarPay04.IsPrimaryKey = false;
                colvarPay04.IsForeignKey = false;
                colvarPay04.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay04);
                
                TableSchema.TableColumn colvarPay05 = new TableSchema.TableColumn(schema);
                colvarPay05.ColumnName = "pay05";
                colvarPay05.DataType = DbType.Currency;
                colvarPay05.MaxLength = 0;
                colvarPay05.AutoIncrement = false;
                colvarPay05.IsNullable = true;
                colvarPay05.IsPrimaryKey = false;
                colvarPay05.IsForeignKey = false;
                colvarPay05.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay05);
                
                TableSchema.TableColumn colvarPay06 = new TableSchema.TableColumn(schema);
                colvarPay06.ColumnName = "pay06";
                colvarPay06.DataType = DbType.Currency;
                colvarPay06.MaxLength = 0;
                colvarPay06.AutoIncrement = false;
                colvarPay06.IsNullable = true;
                colvarPay06.IsPrimaryKey = false;
                colvarPay06.IsForeignKey = false;
                colvarPay06.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay06);
                
                TableSchema.TableColumn colvarPay07 = new TableSchema.TableColumn(schema);
                colvarPay07.ColumnName = "pay07";
                colvarPay07.DataType = DbType.Currency;
                colvarPay07.MaxLength = 0;
                colvarPay07.AutoIncrement = false;
                colvarPay07.IsNullable = true;
                colvarPay07.IsPrimaryKey = false;
                colvarPay07.IsForeignKey = false;
                colvarPay07.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay07);
                
                TableSchema.TableColumn colvarPay08 = new TableSchema.TableColumn(schema);
                colvarPay08.ColumnName = "pay08";
                colvarPay08.DataType = DbType.Currency;
                colvarPay08.MaxLength = 0;
                colvarPay08.AutoIncrement = false;
                colvarPay08.IsNullable = true;
                colvarPay08.IsPrimaryKey = false;
                colvarPay08.IsForeignKey = false;
                colvarPay08.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay08);
                
                TableSchema.TableColumn colvarPay09 = new TableSchema.TableColumn(schema);
                colvarPay09.ColumnName = "pay09";
                colvarPay09.DataType = DbType.Currency;
                colvarPay09.MaxLength = 0;
                colvarPay09.AutoIncrement = false;
                colvarPay09.IsNullable = true;
                colvarPay09.IsPrimaryKey = false;
                colvarPay09.IsForeignKey = false;
                colvarPay09.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay09);
                
                TableSchema.TableColumn colvarPay10 = new TableSchema.TableColumn(schema);
                colvarPay10.ColumnName = "pay10";
                colvarPay10.DataType = DbType.Currency;
                colvarPay10.MaxLength = 0;
                colvarPay10.AutoIncrement = false;
                colvarPay10.IsNullable = true;
                colvarPay10.IsPrimaryKey = false;
                colvarPay10.IsForeignKey = false;
                colvarPay10.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay10);
                
                TableSchema.TableColumn colvarPay11 = new TableSchema.TableColumn(schema);
                colvarPay11.ColumnName = "pay11";
                colvarPay11.DataType = DbType.Currency;
                colvarPay11.MaxLength = 0;
                colvarPay11.AutoIncrement = false;
                colvarPay11.IsNullable = true;
                colvarPay11.IsPrimaryKey = false;
                colvarPay11.IsForeignKey = false;
                colvarPay11.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay11);
                
                TableSchema.TableColumn colvarPay12 = new TableSchema.TableColumn(schema);
                colvarPay12.ColumnName = "pay12";
                colvarPay12.DataType = DbType.Currency;
                colvarPay12.MaxLength = 0;
                colvarPay12.AutoIncrement = false;
                colvarPay12.IsNullable = true;
                colvarPay12.IsPrimaryKey = false;
                colvarPay12.IsForeignKey = false;
                colvarPay12.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay12);
                
                TableSchema.TableColumn colvarPay13 = new TableSchema.TableColumn(schema);
                colvarPay13.ColumnName = "pay13";
                colvarPay13.DataType = DbType.Currency;
                colvarPay13.MaxLength = 0;
                colvarPay13.AutoIncrement = false;
                colvarPay13.IsNullable = true;
                colvarPay13.IsPrimaryKey = false;
                colvarPay13.IsForeignKey = false;
                colvarPay13.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay13);
                
                TableSchema.TableColumn colvarPay14 = new TableSchema.TableColumn(schema);
                colvarPay14.ColumnName = "pay14";
                colvarPay14.DataType = DbType.Currency;
                colvarPay14.MaxLength = 0;
                colvarPay14.AutoIncrement = false;
                colvarPay14.IsNullable = true;
                colvarPay14.IsPrimaryKey = false;
                colvarPay14.IsForeignKey = false;
                colvarPay14.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay14);
                
                TableSchema.TableColumn colvarPay15 = new TableSchema.TableColumn(schema);
                colvarPay15.ColumnName = "pay15";
                colvarPay15.DataType = DbType.Currency;
                colvarPay15.MaxLength = 0;
                colvarPay15.AutoIncrement = false;
                colvarPay15.IsNullable = true;
                colvarPay15.IsPrimaryKey = false;
                colvarPay15.IsForeignKey = false;
                colvarPay15.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay15);
                
                TableSchema.TableColumn colvarPay16 = new TableSchema.TableColumn(schema);
                colvarPay16.ColumnName = "pay16";
                colvarPay16.DataType = DbType.Currency;
                colvarPay16.MaxLength = 0;
                colvarPay16.AutoIncrement = false;
                colvarPay16.IsNullable = true;
                colvarPay16.IsPrimaryKey = false;
                colvarPay16.IsForeignKey = false;
                colvarPay16.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay16);
                
                TableSchema.TableColumn colvarPay17 = new TableSchema.TableColumn(schema);
                colvarPay17.ColumnName = "pay17";
                colvarPay17.DataType = DbType.Currency;
                colvarPay17.MaxLength = 0;
                colvarPay17.AutoIncrement = false;
                colvarPay17.IsNullable = true;
                colvarPay17.IsPrimaryKey = false;
                colvarPay17.IsForeignKey = false;
                colvarPay17.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay17);
                
                TableSchema.TableColumn colvarPay18 = new TableSchema.TableColumn(schema);
                colvarPay18.ColumnName = "pay18";
                colvarPay18.DataType = DbType.Currency;
                colvarPay18.MaxLength = 0;
                colvarPay18.AutoIncrement = false;
                colvarPay18.IsNullable = true;
                colvarPay18.IsPrimaryKey = false;
                colvarPay18.IsForeignKey = false;
                colvarPay18.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay18);
                
                TableSchema.TableColumn colvarPay19 = new TableSchema.TableColumn(schema);
                colvarPay19.ColumnName = "pay19";
                colvarPay19.DataType = DbType.Currency;
                colvarPay19.MaxLength = 0;
                colvarPay19.AutoIncrement = false;
                colvarPay19.IsNullable = true;
                colvarPay19.IsPrimaryKey = false;
                colvarPay19.IsForeignKey = false;
                colvarPay19.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay19);
                
                TableSchema.TableColumn colvarPay20 = new TableSchema.TableColumn(schema);
                colvarPay20.ColumnName = "pay20";
                colvarPay20.DataType = DbType.Currency;
                colvarPay20.MaxLength = 0;
                colvarPay20.AutoIncrement = false;
                colvarPay20.IsNullable = true;
                colvarPay20.IsPrimaryKey = false;
                colvarPay20.IsForeignKey = false;
                colvarPay20.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay20);
                
                TableSchema.TableColumn colvarPay21 = new TableSchema.TableColumn(schema);
                colvarPay21.ColumnName = "pay21";
                colvarPay21.DataType = DbType.Currency;
                colvarPay21.MaxLength = 0;
                colvarPay21.AutoIncrement = false;
                colvarPay21.IsNullable = true;
                colvarPay21.IsPrimaryKey = false;
                colvarPay21.IsForeignKey = false;
                colvarPay21.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay21);
                
                TableSchema.TableColumn colvarPay22 = new TableSchema.TableColumn(schema);
                colvarPay22.ColumnName = "pay22";
                colvarPay22.DataType = DbType.Currency;
                colvarPay22.MaxLength = 0;
                colvarPay22.AutoIncrement = false;
                colvarPay22.IsNullable = true;
                colvarPay22.IsPrimaryKey = false;
                colvarPay22.IsForeignKey = false;
                colvarPay22.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay22);
                
                TableSchema.TableColumn colvarPay23 = new TableSchema.TableColumn(schema);
                colvarPay23.ColumnName = "pay23";
                colvarPay23.DataType = DbType.Currency;
                colvarPay23.MaxLength = 0;
                colvarPay23.AutoIncrement = false;
                colvarPay23.IsNullable = true;
                colvarPay23.IsPrimaryKey = false;
                colvarPay23.IsForeignKey = false;
                colvarPay23.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay23);
                
                TableSchema.TableColumn colvarPay24 = new TableSchema.TableColumn(schema);
                colvarPay24.ColumnName = "pay24";
                colvarPay24.DataType = DbType.Currency;
                colvarPay24.MaxLength = 0;
                colvarPay24.AutoIncrement = false;
                colvarPay24.IsNullable = true;
                colvarPay24.IsPrimaryKey = false;
                colvarPay24.IsForeignKey = false;
                colvarPay24.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay24);
                
                TableSchema.TableColumn colvarPay25 = new TableSchema.TableColumn(schema);
                colvarPay25.ColumnName = "pay25";
                colvarPay25.DataType = DbType.Currency;
                colvarPay25.MaxLength = 0;
                colvarPay25.AutoIncrement = false;
                colvarPay25.IsNullable = true;
                colvarPay25.IsPrimaryKey = false;
                colvarPay25.IsForeignKey = false;
                colvarPay25.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay25);
                
                TableSchema.TableColumn colvarPay26 = new TableSchema.TableColumn(schema);
                colvarPay26.ColumnName = "pay26";
                colvarPay26.DataType = DbType.Currency;
                colvarPay26.MaxLength = 0;
                colvarPay26.AutoIncrement = false;
                colvarPay26.IsNullable = true;
                colvarPay26.IsPrimaryKey = false;
                colvarPay26.IsForeignKey = false;
                colvarPay26.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay26);
                
                TableSchema.TableColumn colvarPay27 = new TableSchema.TableColumn(schema);
                colvarPay27.ColumnName = "pay27";
                colvarPay27.DataType = DbType.Currency;
                colvarPay27.MaxLength = 0;
                colvarPay27.AutoIncrement = false;
                colvarPay27.IsNullable = true;
                colvarPay27.IsPrimaryKey = false;
                colvarPay27.IsForeignKey = false;
                colvarPay27.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay27);
                
                TableSchema.TableColumn colvarPay28 = new TableSchema.TableColumn(schema);
                colvarPay28.ColumnName = "pay28";
                colvarPay28.DataType = DbType.Currency;
                colvarPay28.MaxLength = 0;
                colvarPay28.AutoIncrement = false;
                colvarPay28.IsNullable = true;
                colvarPay28.IsPrimaryKey = false;
                colvarPay28.IsForeignKey = false;
                colvarPay28.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay28);
                
                TableSchema.TableColumn colvarPay29 = new TableSchema.TableColumn(schema);
                colvarPay29.ColumnName = "pay29";
                colvarPay29.DataType = DbType.Currency;
                colvarPay29.MaxLength = 0;
                colvarPay29.AutoIncrement = false;
                colvarPay29.IsNullable = true;
                colvarPay29.IsPrimaryKey = false;
                colvarPay29.IsForeignKey = false;
                colvarPay29.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay29);
                
                TableSchema.TableColumn colvarPay30 = new TableSchema.TableColumn(schema);
                colvarPay30.ColumnName = "pay30";
                colvarPay30.DataType = DbType.Currency;
                colvarPay30.MaxLength = 0;
                colvarPay30.AutoIncrement = false;
                colvarPay30.IsNullable = true;
                colvarPay30.IsPrimaryKey = false;
                colvarPay30.IsForeignKey = false;
                colvarPay30.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay30);
                
                TableSchema.TableColumn colvarPay31 = new TableSchema.TableColumn(schema);
                colvarPay31.ColumnName = "pay31";
                colvarPay31.DataType = DbType.Currency;
                colvarPay31.MaxLength = 0;
                colvarPay31.AutoIncrement = false;
                colvarPay31.IsNullable = true;
                colvarPay31.IsPrimaryKey = false;
                colvarPay31.IsForeignKey = false;
                colvarPay31.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay31);
                
                TableSchema.TableColumn colvarPay32 = new TableSchema.TableColumn(schema);
                colvarPay32.ColumnName = "pay32";
                colvarPay32.DataType = DbType.Currency;
                colvarPay32.MaxLength = 0;
                colvarPay32.AutoIncrement = false;
                colvarPay32.IsNullable = true;
                colvarPay32.IsPrimaryKey = false;
                colvarPay32.IsForeignKey = false;
                colvarPay32.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay32);
                
                TableSchema.TableColumn colvarPay33 = new TableSchema.TableColumn(schema);
                colvarPay33.ColumnName = "pay33";
                colvarPay33.DataType = DbType.Currency;
                colvarPay33.MaxLength = 0;
                colvarPay33.AutoIncrement = false;
                colvarPay33.IsNullable = true;
                colvarPay33.IsPrimaryKey = false;
                colvarPay33.IsForeignKey = false;
                colvarPay33.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay33);
                
                TableSchema.TableColumn colvarPay34 = new TableSchema.TableColumn(schema);
                colvarPay34.ColumnName = "pay34";
                colvarPay34.DataType = DbType.Currency;
                colvarPay34.MaxLength = 0;
                colvarPay34.AutoIncrement = false;
                colvarPay34.IsNullable = true;
                colvarPay34.IsPrimaryKey = false;
                colvarPay34.IsForeignKey = false;
                colvarPay34.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay34);
                
                TableSchema.TableColumn colvarPay35 = new TableSchema.TableColumn(schema);
                colvarPay35.ColumnName = "pay35";
                colvarPay35.DataType = DbType.Currency;
                colvarPay35.MaxLength = 0;
                colvarPay35.AutoIncrement = false;
                colvarPay35.IsNullable = true;
                colvarPay35.IsPrimaryKey = false;
                colvarPay35.IsForeignKey = false;
                colvarPay35.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay35);
                
                TableSchema.TableColumn colvarPay36 = new TableSchema.TableColumn(schema);
                colvarPay36.ColumnName = "pay36";
                colvarPay36.DataType = DbType.Currency;
                colvarPay36.MaxLength = 0;
                colvarPay36.AutoIncrement = false;
                colvarPay36.IsNullable = true;
                colvarPay36.IsPrimaryKey = false;
                colvarPay36.IsForeignKey = false;
                colvarPay36.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay36);
                
                TableSchema.TableColumn colvarPay37 = new TableSchema.TableColumn(schema);
                colvarPay37.ColumnName = "pay37";
                colvarPay37.DataType = DbType.Currency;
                colvarPay37.MaxLength = 0;
                colvarPay37.AutoIncrement = false;
                colvarPay37.IsNullable = true;
                colvarPay37.IsPrimaryKey = false;
                colvarPay37.IsForeignKey = false;
                colvarPay37.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay37);
                
                TableSchema.TableColumn colvarPay38 = new TableSchema.TableColumn(schema);
                colvarPay38.ColumnName = "pay38";
                colvarPay38.DataType = DbType.Currency;
                colvarPay38.MaxLength = 0;
                colvarPay38.AutoIncrement = false;
                colvarPay38.IsNullable = true;
                colvarPay38.IsPrimaryKey = false;
                colvarPay38.IsForeignKey = false;
                colvarPay38.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay38);
                
                TableSchema.TableColumn colvarPay39 = new TableSchema.TableColumn(schema);
                colvarPay39.ColumnName = "pay39";
                colvarPay39.DataType = DbType.Currency;
                colvarPay39.MaxLength = 0;
                colvarPay39.AutoIncrement = false;
                colvarPay39.IsNullable = true;
                colvarPay39.IsPrimaryKey = false;
                colvarPay39.IsForeignKey = false;
                colvarPay39.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay39);
                
                TableSchema.TableColumn colvarPay40 = new TableSchema.TableColumn(schema);
                colvarPay40.ColumnName = "pay40";
                colvarPay40.DataType = DbType.Currency;
                colvarPay40.MaxLength = 0;
                colvarPay40.AutoIncrement = false;
                colvarPay40.IsNullable = true;
                colvarPay40.IsPrimaryKey = false;
                colvarPay40.IsForeignKey = false;
                colvarPay40.IsReadOnly = false;
                
                schema.Columns.Add(colvarPay40);
                
                TableSchema.TableColumn colvarPayOthers = new TableSchema.TableColumn(schema);
                colvarPayOthers.ColumnName = "payOthers";
                colvarPayOthers.DataType = DbType.Currency;
                colvarPayOthers.MaxLength = 0;
                colvarPayOthers.AutoIncrement = false;
                colvarPayOthers.IsNullable = true;
                colvarPayOthers.IsPrimaryKey = false;
                colvarPayOthers.IsForeignKey = false;
                colvarPayOthers.IsReadOnly = false;
                
                schema.Columns.Add(colvarPayOthers);
                
                TableSchema.TableColumn colvarTotalpayment = new TableSchema.TableColumn(schema);
                colvarTotalpayment.ColumnName = "totalpayment";
                colvarTotalpayment.DataType = DbType.Currency;
                colvarTotalpayment.MaxLength = 0;
                colvarTotalpayment.AutoIncrement = false;
                colvarTotalpayment.IsNullable = true;
                colvarTotalpayment.IsPrimaryKey = false;
                colvarTotalpayment.IsForeignKey = false;
                colvarTotalpayment.IsReadOnly = false;
                
                schema.Columns.Add(colvarTotalpayment);
                
                TableSchema.TableColumn colvarPointAllocated = new TableSchema.TableColumn(schema);
                colvarPointAllocated.ColumnName = "pointAllocated";
                colvarPointAllocated.DataType = DbType.Currency;
                colvarPointAllocated.MaxLength = 0;
                colvarPointAllocated.AutoIncrement = false;
                colvarPointAllocated.IsNullable = true;
                colvarPointAllocated.IsPrimaryKey = false;
                colvarPointAllocated.IsForeignKey = false;
                colvarPointAllocated.IsReadOnly = false;
                
                schema.Columns.Add(colvarPointAllocated);
                
                TableSchema.TableColumn colvarPayByInstallment = new TableSchema.TableColumn(schema);
                colvarPayByInstallment.ColumnName = "payByInstallment";
                colvarPayByInstallment.DataType = DbType.Currency;
                colvarPayByInstallment.MaxLength = 0;
                colvarPayByInstallment.AutoIncrement = false;
                colvarPayByInstallment.IsNullable = true;
                colvarPayByInstallment.IsPrimaryKey = false;
                colvarPayByInstallment.IsForeignKey = false;
                colvarPayByInstallment.IsReadOnly = false;
                
                schema.Columns.Add(colvarPayByInstallment);
                
                TableSchema.TableColumn colvarPayByPoint = new TableSchema.TableColumn(schema);
                colvarPayByPoint.ColumnName = "payByPoint";
                colvarPayByPoint.DataType = DbType.Currency;
                colvarPayByPoint.MaxLength = 0;
                colvarPayByPoint.AutoIncrement = false;
                colvarPayByPoint.IsNullable = true;
                colvarPayByPoint.IsPrimaryKey = false;
                colvarPayByPoint.IsForeignKey = false;
                colvarPayByPoint.IsReadOnly = false;
                
                schema.Columns.Add(colvarPayByPoint);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("viewDW_MonthlySales",schema);
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
	    public ViewDWMonthlySale()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewDWMonthlySale(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewDWMonthlySale(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewDWMonthlySale(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("Orderdate")]
        [Bindable(true)]
        public string Orderdate 
	    {
		    get
		    {
			    return GetColumnValue<string>("orderdate");
		    }
            set 
		    {
			    SetColumnValue("orderdate", value);
            }
        }
	      
        [XmlAttribute("Outletname")]
        [Bindable(true)]
        public string Outletname 
	    {
		    get
		    {
			    return GetColumnValue<string>("outletname");
		    }
            set 
		    {
			    SetColumnValue("outletname", value);
            }
        }
	      
        [XmlAttribute("Pax")]
        [Bindable(true)]
        public int? Pax 
	    {
		    get
		    {
			    return GetColumnValue<int?>("pax");
		    }
            set 
		    {
			    SetColumnValue("pax", value);
            }
        }
	      
        [XmlAttribute("Bill")]
        [Bindable(true)]
        public int? Bill 
	    {
		    get
		    {
			    return GetColumnValue<int?>("bill");
		    }
            set 
		    {
			    SetColumnValue("bill", value);
            }
        }
	      
        [XmlAttribute("Grossamount")]
        [Bindable(true)]
        public decimal? Grossamount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("grossamount");
		    }
            set 
		    {
			    SetColumnValue("grossamount", value);
            }
        }
	      
        [XmlAttribute("Disc")]
        [Bindable(true)]
        public decimal? Disc 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("disc");
		    }
            set 
		    {
			    SetColumnValue("disc", value);
            }
        }
	      
        [XmlAttribute("Afterdisc")]
        [Bindable(true)]
        public decimal? Afterdisc 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("afterdisc");
		    }
            set 
		    {
			    SetColumnValue("afterdisc", value);
            }
        }
	      
        [XmlAttribute("Svccharge")]
        [Bindable(true)]
        public decimal? Svccharge 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("svccharge");
		    }
            set 
		    {
			    SetColumnValue("svccharge", value);
            }
        }
	      
        [XmlAttribute("Befgst")]
        [Bindable(true)]
        public decimal? Befgst 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("befgst");
		    }
            set 
		    {
			    SetColumnValue("befgst", value);
            }
        }
	      
        [XmlAttribute("Gst")]
        [Bindable(true)]
        public decimal? Gst 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("gst");
		    }
            set 
		    {
			    SetColumnValue("gst", value);
            }
        }
	      
        [XmlAttribute("Rounding")]
        [Bindable(true)]
        public decimal? Rounding 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("rounding");
		    }
            set 
		    {
			    SetColumnValue("rounding", value);
            }
        }
	      
        [XmlAttribute("Nettamount")]
        [Bindable(true)]
        public decimal? Nettamount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("nettamount");
		    }
            set 
		    {
			    SetColumnValue("nettamount", value);
            }
        }
	      
        [XmlAttribute("PointSale")]
        [Bindable(true)]
        public decimal? PointSale 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pointSale");
		    }
            set 
		    {
			    SetColumnValue("pointSale", value);
            }
        }
	      
        [XmlAttribute("InstallmentPaymentSale")]
        [Bindable(true)]
        public decimal? InstallmentPaymentSale 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("installmentPaymentSale");
		    }
            set 
		    {
			    SetColumnValue("installmentPaymentSale", value);
            }
        }
	      
        [XmlAttribute("PayByCash")]
        [Bindable(true)]
        public decimal? PayByCash 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("payByCash");
		    }
            set 
		    {
			    SetColumnValue("payByCash", value);
            }
        }
	      
        [XmlAttribute("Pay01")]
        [Bindable(true)]
        public decimal? Pay01 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay01");
		    }
            set 
		    {
			    SetColumnValue("pay01", value);
            }
        }
	      
        [XmlAttribute("Pay02")]
        [Bindable(true)]
        public decimal? Pay02 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay02");
		    }
            set 
		    {
			    SetColumnValue("pay02", value);
            }
        }
	      
        [XmlAttribute("Pay03")]
        [Bindable(true)]
        public decimal? Pay03 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay03");
		    }
            set 
		    {
			    SetColumnValue("pay03", value);
            }
        }
	      
        [XmlAttribute("Pay04")]
        [Bindable(true)]
        public decimal? Pay04 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay04");
		    }
            set 
		    {
			    SetColumnValue("pay04", value);
            }
        }
	      
        [XmlAttribute("Pay05")]
        [Bindable(true)]
        public decimal? Pay05 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay05");
		    }
            set 
		    {
			    SetColumnValue("pay05", value);
            }
        }
	      
        [XmlAttribute("Pay06")]
        [Bindable(true)]
        public decimal? Pay06 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay06");
		    }
            set 
		    {
			    SetColumnValue("pay06", value);
            }
        }
	      
        [XmlAttribute("Pay07")]
        [Bindable(true)]
        public decimal? Pay07 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay07");
		    }
            set 
		    {
			    SetColumnValue("pay07", value);
            }
        }
	      
        [XmlAttribute("Pay08")]
        [Bindable(true)]
        public decimal? Pay08 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay08");
		    }
            set 
		    {
			    SetColumnValue("pay08", value);
            }
        }
	      
        [XmlAttribute("Pay09")]
        [Bindable(true)]
        public decimal? Pay09 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay09");
		    }
            set 
		    {
			    SetColumnValue("pay09", value);
            }
        }
	      
        [XmlAttribute("Pay10")]
        [Bindable(true)]
        public decimal? Pay10 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay10");
		    }
            set 
		    {
			    SetColumnValue("pay10", value);
            }
        }
	      
        [XmlAttribute("Pay11")]
        [Bindable(true)]
        public decimal? Pay11 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay11");
		    }
            set 
		    {
			    SetColumnValue("pay11", value);
            }
        }
	      
        [XmlAttribute("Pay12")]
        [Bindable(true)]
        public decimal? Pay12 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay12");
		    }
            set 
		    {
			    SetColumnValue("pay12", value);
            }
        }
	      
        [XmlAttribute("Pay13")]
        [Bindable(true)]
        public decimal? Pay13 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay13");
		    }
            set 
		    {
			    SetColumnValue("pay13", value);
            }
        }
	      
        [XmlAttribute("Pay14")]
        [Bindable(true)]
        public decimal? Pay14 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay14");
		    }
            set 
		    {
			    SetColumnValue("pay14", value);
            }
        }
	      
        [XmlAttribute("Pay15")]
        [Bindable(true)]
        public decimal? Pay15 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay15");
		    }
            set 
		    {
			    SetColumnValue("pay15", value);
            }
        }
	      
        [XmlAttribute("Pay16")]
        [Bindable(true)]
        public decimal? Pay16 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay16");
		    }
            set 
		    {
			    SetColumnValue("pay16", value);
            }
        }
	      
        [XmlAttribute("Pay17")]
        [Bindable(true)]
        public decimal? Pay17 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay17");
		    }
            set 
		    {
			    SetColumnValue("pay17", value);
            }
        }
	      
        [XmlAttribute("Pay18")]
        [Bindable(true)]
        public decimal? Pay18 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay18");
		    }
            set 
		    {
			    SetColumnValue("pay18", value);
            }
        }
	      
        [XmlAttribute("Pay19")]
        [Bindable(true)]
        public decimal? Pay19 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay19");
		    }
            set 
		    {
			    SetColumnValue("pay19", value);
            }
        }
	      
        [XmlAttribute("Pay20")]
        [Bindable(true)]
        public decimal? Pay20 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay20");
		    }
            set 
		    {
			    SetColumnValue("pay20", value);
            }
        }
	      
        [XmlAttribute("Pay21")]
        [Bindable(true)]
        public decimal? Pay21 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay21");
		    }
            set 
		    {
			    SetColumnValue("pay21", value);
            }
        }
	      
        [XmlAttribute("Pay22")]
        [Bindable(true)]
        public decimal? Pay22 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay22");
		    }
            set 
		    {
			    SetColumnValue("pay22", value);
            }
        }
	      
        [XmlAttribute("Pay23")]
        [Bindable(true)]
        public decimal? Pay23 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay23");
		    }
            set 
		    {
			    SetColumnValue("pay23", value);
            }
        }
	      
        [XmlAttribute("Pay24")]
        [Bindable(true)]
        public decimal? Pay24 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay24");
		    }
            set 
		    {
			    SetColumnValue("pay24", value);
            }
        }
	      
        [XmlAttribute("Pay25")]
        [Bindable(true)]
        public decimal? Pay25 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay25");
		    }
            set 
		    {
			    SetColumnValue("pay25", value);
            }
        }
	      
        [XmlAttribute("Pay26")]
        [Bindable(true)]
        public decimal? Pay26 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay26");
		    }
            set 
		    {
			    SetColumnValue("pay26", value);
            }
        }
	      
        [XmlAttribute("Pay27")]
        [Bindable(true)]
        public decimal? Pay27 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay27");
		    }
            set 
		    {
			    SetColumnValue("pay27", value);
            }
        }
	      
        [XmlAttribute("Pay28")]
        [Bindable(true)]
        public decimal? Pay28 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay28");
		    }
            set 
		    {
			    SetColumnValue("pay28", value);
            }
        }
	      
        [XmlAttribute("Pay29")]
        [Bindable(true)]
        public decimal? Pay29 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay29");
		    }
            set 
		    {
			    SetColumnValue("pay29", value);
            }
        }
	      
        [XmlAttribute("Pay30")]
        [Bindable(true)]
        public decimal? Pay30 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay30");
		    }
            set 
		    {
			    SetColumnValue("pay30", value);
            }
        }
	      
        [XmlAttribute("Pay31")]
        [Bindable(true)]
        public decimal? Pay31 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay31");
		    }
            set 
		    {
			    SetColumnValue("pay31", value);
            }
        }
	      
        [XmlAttribute("Pay32")]
        [Bindable(true)]
        public decimal? Pay32 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay32");
		    }
            set 
		    {
			    SetColumnValue("pay32", value);
            }
        }
	      
        [XmlAttribute("Pay33")]
        [Bindable(true)]
        public decimal? Pay33 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay33");
		    }
            set 
		    {
			    SetColumnValue("pay33", value);
            }
        }
	      
        [XmlAttribute("Pay34")]
        [Bindable(true)]
        public decimal? Pay34 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay34");
		    }
            set 
		    {
			    SetColumnValue("pay34", value);
            }
        }
	      
        [XmlAttribute("Pay35")]
        [Bindable(true)]
        public decimal? Pay35 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay35");
		    }
            set 
		    {
			    SetColumnValue("pay35", value);
            }
        }
	      
        [XmlAttribute("Pay36")]
        [Bindable(true)]
        public decimal? Pay36 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay36");
		    }
            set 
		    {
			    SetColumnValue("pay36", value);
            }
        }
	      
        [XmlAttribute("Pay37")]
        [Bindable(true)]
        public decimal? Pay37 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay37");
		    }
            set 
		    {
			    SetColumnValue("pay37", value);
            }
        }
	      
        [XmlAttribute("Pay38")]
        [Bindable(true)]
        public decimal? Pay38 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay38");
		    }
            set 
		    {
			    SetColumnValue("pay38", value);
            }
        }
	      
        [XmlAttribute("Pay39")]
        [Bindable(true)]
        public decimal? Pay39 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay39");
		    }
            set 
		    {
			    SetColumnValue("pay39", value);
            }
        }
	      
        [XmlAttribute("Pay40")]
        [Bindable(true)]
        public decimal? Pay40 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pay40");
		    }
            set 
		    {
			    SetColumnValue("pay40", value);
            }
        }
	      
        [XmlAttribute("PayOthers")]
        [Bindable(true)]
        public decimal? PayOthers 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("payOthers");
		    }
            set 
		    {
			    SetColumnValue("payOthers", value);
            }
        }
	      
        [XmlAttribute("Totalpayment")]
        [Bindable(true)]
        public decimal? Totalpayment 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("totalpayment");
		    }
            set 
		    {
			    SetColumnValue("totalpayment", value);
            }
        }
	      
        [XmlAttribute("PointAllocated")]
        [Bindable(true)]
        public decimal? PointAllocated 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("pointAllocated");
		    }
            set 
		    {
			    SetColumnValue("pointAllocated", value);
            }
        }
	      
        [XmlAttribute("PayByInstallment")]
        [Bindable(true)]
        public decimal? PayByInstallment 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("payByInstallment");
		    }
            set 
		    {
			    SetColumnValue("payByInstallment", value);
            }
        }
	      
        [XmlAttribute("PayByPoint")]
        [Bindable(true)]
        public decimal? PayByPoint 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("payByPoint");
		    }
            set 
		    {
			    SetColumnValue("payByPoint", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string Orderdate = @"orderdate";
            
            public static string Outletname = @"outletname";
            
            public static string Pax = @"pax";
            
            public static string Bill = @"bill";
            
            public static string Grossamount = @"grossamount";
            
            public static string Disc = @"disc";
            
            public static string Afterdisc = @"afterdisc";
            
            public static string Svccharge = @"svccharge";
            
            public static string Befgst = @"befgst";
            
            public static string Gst = @"gst";
            
            public static string Rounding = @"rounding";
            
            public static string Nettamount = @"nettamount";
            
            public static string PointSale = @"pointSale";
            
            public static string InstallmentPaymentSale = @"installmentPaymentSale";
            
            public static string PayByCash = @"payByCash";
            
            public static string Pay01 = @"pay01";
            
            public static string Pay02 = @"pay02";
            
            public static string Pay03 = @"pay03";
            
            public static string Pay04 = @"pay04";
            
            public static string Pay05 = @"pay05";
            
            public static string Pay06 = @"pay06";
            
            public static string Pay07 = @"pay07";
            
            public static string Pay08 = @"pay08";
            
            public static string Pay09 = @"pay09";
            
            public static string Pay10 = @"pay10";
            
            public static string Pay11 = @"pay11";
            
            public static string Pay12 = @"pay12";
            
            public static string Pay13 = @"pay13";
            
            public static string Pay14 = @"pay14";
            
            public static string Pay15 = @"pay15";
            
            public static string Pay16 = @"pay16";
            
            public static string Pay17 = @"pay17";
            
            public static string Pay18 = @"pay18";
            
            public static string Pay19 = @"pay19";
            
            public static string Pay20 = @"pay20";
            
            public static string Pay21 = @"pay21";
            
            public static string Pay22 = @"pay22";
            
            public static string Pay23 = @"pay23";
            
            public static string Pay24 = @"pay24";
            
            public static string Pay25 = @"pay25";
            
            public static string Pay26 = @"pay26";
            
            public static string Pay27 = @"pay27";
            
            public static string Pay28 = @"pay28";
            
            public static string Pay29 = @"pay29";
            
            public static string Pay30 = @"pay30";
            
            public static string Pay31 = @"pay31";
            
            public static string Pay32 = @"pay32";
            
            public static string Pay33 = @"pay33";
            
            public static string Pay34 = @"pay34";
            
            public static string Pay35 = @"pay35";
            
            public static string Pay36 = @"pay36";
            
            public static string Pay37 = @"pay37";
            
            public static string Pay38 = @"pay38";
            
            public static string Pay39 = @"pay39";
            
            public static string Pay40 = @"pay40";
            
            public static string PayOthers = @"payOthers";
            
            public static string Totalpayment = @"totalpayment";
            
            public static string PointAllocated = @"pointAllocated";
            
            public static string PayByInstallment = @"payByInstallment";
            
            public static string PayByPoint = @"payByPoint";
            
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
