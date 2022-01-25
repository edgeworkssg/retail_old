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
    /// Strongly-typed collection for the ViewPackage class.
    /// </summary>
    [Serializable]
    public partial class ViewPackageCollection : ReadOnlyList<ViewPackage, ViewPackageCollection>
    {        
        public ViewPackageCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewPackages view.
    /// </summary>
    [Serializable]
    public partial class ViewPackage : ReadOnlyRecord<ViewPackage>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewPackages", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarPackageHdrId = new TableSchema.TableColumn(schema);
                colvarPackageHdrId.ColumnName = "PackageHdrId";
                colvarPackageHdrId.DataType = DbType.AnsiString;
                colvarPackageHdrId.MaxLength = 50;
                colvarPackageHdrId.AutoIncrement = false;
                colvarPackageHdrId.IsNullable = false;
                colvarPackageHdrId.IsPrimaryKey = false;
                colvarPackageHdrId.IsForeignKey = false;
                colvarPackageHdrId.IsReadOnly = false;
                
                schema.Columns.Add(colvarPackageHdrId);
                
                TableSchema.TableColumn colvarOrderHdrId = new TableSchema.TableColumn(schema);
                colvarOrderHdrId.ColumnName = "OrderHdrId";
                colvarOrderHdrId.DataType = DbType.AnsiString;
                colvarOrderHdrId.MaxLength = 14;
                colvarOrderHdrId.AutoIncrement = false;
                colvarOrderHdrId.IsNullable = false;
                colvarOrderHdrId.IsPrimaryKey = false;
                colvarOrderHdrId.IsForeignKey = false;
                colvarOrderHdrId.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderHdrId);
                
                TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
                colvarRemark.ColumnName = "Remark";
                colvarRemark.DataType = DbType.String;
                colvarRemark.MaxLength = 250;
                colvarRemark.AutoIncrement = false;
                colvarRemark.IsNullable = false;
                colvarRemark.IsPrimaryKey = false;
                colvarRemark.IsForeignKey = false;
                colvarRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemark);
                
                TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
                colvarCreatedOn.ColumnName = "CreatedOn";
                colvarCreatedOn.DataType = DbType.DateTime;
                colvarCreatedOn.MaxLength = 0;
                colvarCreatedOn.AutoIncrement = false;
                colvarCreatedOn.IsNullable = true;
                colvarCreatedOn.IsPrimaryKey = false;
                colvarCreatedOn.IsForeignKey = false;
                colvarCreatedOn.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarModifiedBy);
                
                TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
                colvarDeleted.ColumnName = "Deleted";
                colvarDeleted.DataType = DbType.Boolean;
                colvarDeleted.MaxLength = 0;
                colvarDeleted.AutoIncrement = false;
                colvarDeleted.IsNullable = false;
                colvarDeleted.IsPrimaryKey = false;
                colvarDeleted.IsForeignKey = false;
                colvarDeleted.IsReadOnly = false;
                
                schema.Columns.Add(colvarDeleted);
                
                TableSchema.TableColumn colvarOrderRefNo = new TableSchema.TableColumn(schema);
                colvarOrderRefNo.ColumnName = "OrderRefNo";
                colvarOrderRefNo.DataType = DbType.AnsiString;
                colvarOrderRefNo.MaxLength = 50;
                colvarOrderRefNo.AutoIncrement = false;
                colvarOrderRefNo.IsNullable = false;
                colvarOrderRefNo.IsPrimaryKey = false;
                colvarOrderRefNo.IsForeignKey = false;
                colvarOrderRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderRefNo);
                
                TableSchema.TableColumn colvarDiscount = new TableSchema.TableColumn(schema);
                colvarDiscount.ColumnName = "Discount";
                colvarDiscount.DataType = DbType.Decimal;
                colvarDiscount.MaxLength = 0;
                colvarDiscount.AutoIncrement = false;
                colvarDiscount.IsNullable = false;
                colvarDiscount.IsPrimaryKey = false;
                colvarDiscount.IsForeignKey = false;
                colvarDiscount.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscount);
                
                TableSchema.TableColumn colvarInventoryHdrRefNo = new TableSchema.TableColumn(schema);
                colvarInventoryHdrRefNo.ColumnName = "InventoryHdrRefNo";
                colvarInventoryHdrRefNo.DataType = DbType.AnsiString;
                colvarInventoryHdrRefNo.MaxLength = 50;
                colvarInventoryHdrRefNo.AutoIncrement = false;
                colvarInventoryHdrRefNo.IsNullable = true;
                colvarInventoryHdrRefNo.IsPrimaryKey = false;
                colvarInventoryHdrRefNo.IsForeignKey = false;
                colvarInventoryHdrRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryHdrRefNo);
                
                TableSchema.TableColumn colvarCashierID = new TableSchema.TableColumn(schema);
                colvarCashierID.ColumnName = "CashierID";
                colvarCashierID.DataType = DbType.AnsiString;
                colvarCashierID.MaxLength = 50;
                colvarCashierID.AutoIncrement = false;
                colvarCashierID.IsNullable = false;
                colvarCashierID.IsPrimaryKey = false;
                colvarCashierID.IsForeignKey = false;
                colvarCashierID.IsReadOnly = false;
                
                schema.Columns.Add(colvarCashierID);
                
                TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
                colvarPointOfSaleID.ColumnName = "PointOfSaleID";
                colvarPointOfSaleID.DataType = DbType.Int32;
                colvarPointOfSaleID.MaxLength = 0;
                colvarPointOfSaleID.AutoIncrement = false;
                colvarPointOfSaleID.IsNullable = false;
                colvarPointOfSaleID.IsPrimaryKey = false;
                colvarPointOfSaleID.IsForeignKey = false;
                colvarPointOfSaleID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPointOfSaleID);
                
                TableSchema.TableColumn colvarOrderDate = new TableSchema.TableColumn(schema);
                colvarOrderDate.ColumnName = "OrderDate";
                colvarOrderDate.DataType = DbType.DateTime;
                colvarOrderDate.MaxLength = 0;
                colvarOrderDate.AutoIncrement = false;
                colvarOrderDate.IsNullable = false;
                colvarOrderDate.IsPrimaryKey = false;
                colvarOrderDate.IsForeignKey = false;
                colvarOrderDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDate);
                
                TableSchema.TableColumn colvarGrossAmount = new TableSchema.TableColumn(schema);
                colvarGrossAmount.ColumnName = "GrossAmount";
                colvarGrossAmount.DataType = DbType.Currency;
                colvarGrossAmount.MaxLength = 0;
                colvarGrossAmount.AutoIncrement = false;
                colvarGrossAmount.IsNullable = false;
                colvarGrossAmount.IsPrimaryKey = false;
                colvarGrossAmount.IsForeignKey = false;
                colvarGrossAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarGrossAmount);
                
                TableSchema.TableColumn colvarNettAmount = new TableSchema.TableColumn(schema);
                colvarNettAmount.ColumnName = "NettAmount";
                colvarNettAmount.DataType = DbType.Currency;
                colvarNettAmount.MaxLength = 0;
                colvarNettAmount.AutoIncrement = false;
                colvarNettAmount.IsNullable = false;
                colvarNettAmount.IsPrimaryKey = false;
                colvarNettAmount.IsForeignKey = false;
                colvarNettAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarNettAmount);
                
                TableSchema.TableColumn colvarDiscountAmount = new TableSchema.TableColumn(schema);
                colvarDiscountAmount.ColumnName = "DiscountAmount";
                colvarDiscountAmount.DataType = DbType.Currency;
                colvarDiscountAmount.MaxLength = 0;
                colvarDiscountAmount.AutoIncrement = false;
                colvarDiscountAmount.IsNullable = false;
                colvarDiscountAmount.IsPrimaryKey = false;
                colvarDiscountAmount.IsForeignKey = false;
                colvarDiscountAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscountAmount);
                
                TableSchema.TableColumn colvarGst = new TableSchema.TableColumn(schema);
                colvarGst.ColumnName = "GST";
                colvarGst.DataType = DbType.Double;
                colvarGst.MaxLength = 0;
                colvarGst.AutoIncrement = false;
                colvarGst.IsNullable = false;
                colvarGst.IsPrimaryKey = false;
                colvarGst.IsForeignKey = false;
                colvarGst.IsReadOnly = false;
                
                schema.Columns.Add(colvarGst);
                
                TableSchema.TableColumn colvarIsVoided = new TableSchema.TableColumn(schema);
                colvarIsVoided.ColumnName = "IsVoided";
                colvarIsVoided.DataType = DbType.Boolean;
                colvarIsVoided.MaxLength = 0;
                colvarIsVoided.AutoIncrement = false;
                colvarIsVoided.IsNullable = false;
                colvarIsVoided.IsPrimaryKey = false;
                colvarIsVoided.IsForeignKey = false;
                colvarIsVoided.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsVoided);
                
                TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
                colvarMembershipNo.ColumnName = "MembershipNo";
                colvarMembershipNo.DataType = DbType.AnsiString;
                colvarMembershipNo.MaxLength = 50;
                colvarMembershipNo.AutoIncrement = false;
                colvarMembershipNo.IsNullable = true;
                colvarMembershipNo.IsPrimaryKey = false;
                colvarMembershipNo.IsForeignKey = false;
                colvarMembershipNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarMembershipNo);
                
                TableSchema.TableColumn colvarExpr1 = new TableSchema.TableColumn(schema);
                colvarExpr1.ColumnName = "Expr1";
                colvarExpr1.DataType = DbType.String;
                colvarExpr1.MaxLength = -1;
                colvarExpr1.AutoIncrement = false;
                colvarExpr1.IsNullable = true;
                colvarExpr1.IsPrimaryKey = false;
                colvarExpr1.IsForeignKey = false;
                colvarExpr1.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr1);
                
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
                
                TableSchema.TableColumn colvarOrderDetDate = new TableSchema.TableColumn(schema);
                colvarOrderDetDate.ColumnName = "OrderDetDate";
                colvarOrderDetDate.DataType = DbType.DateTime;
                colvarOrderDetDate.MaxLength = 0;
                colvarOrderDetDate.AutoIncrement = false;
                colvarOrderDetDate.IsNullable = false;
                colvarOrderDetDate.IsPrimaryKey = false;
                colvarOrderDetDate.IsForeignKey = false;
                colvarOrderDetDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDetDate);
                
                TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
                colvarQuantity.ColumnName = "Quantity";
                colvarQuantity.DataType = DbType.Int32;
                colvarQuantity.MaxLength = 0;
                colvarQuantity.AutoIncrement = false;
                colvarQuantity.IsNullable = false;
                colvarQuantity.IsPrimaryKey = false;
                colvarQuantity.IsForeignKey = false;
                colvarQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuantity);
                
                TableSchema.TableColumn colvarUnitPrice = new TableSchema.TableColumn(schema);
                colvarUnitPrice.ColumnName = "UnitPrice";
                colvarUnitPrice.DataType = DbType.Currency;
                colvarUnitPrice.MaxLength = 0;
                colvarUnitPrice.AutoIncrement = false;
                colvarUnitPrice.IsNullable = false;
                colvarUnitPrice.IsPrimaryKey = false;
                colvarUnitPrice.IsForeignKey = false;
                colvarUnitPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarUnitPrice);
                
                TableSchema.TableColumn colvarExpr2 = new TableSchema.TableColumn(schema);
                colvarExpr2.ColumnName = "Expr2";
                colvarExpr2.DataType = DbType.Currency;
                colvarExpr2.MaxLength = 0;
                colvarExpr2.AutoIncrement = false;
                colvarExpr2.IsNullable = false;
                colvarExpr2.IsPrimaryKey = false;
                colvarExpr2.IsForeignKey = false;
                colvarExpr2.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr2);
                
                TableSchema.TableColumn colvarAmount = new TableSchema.TableColumn(schema);
                colvarAmount.ColumnName = "Amount";
                colvarAmount.DataType = DbType.Currency;
                colvarAmount.MaxLength = 0;
                colvarAmount.AutoIncrement = false;
                colvarAmount.IsNullable = false;
                colvarAmount.IsPrimaryKey = false;
                colvarAmount.IsForeignKey = false;
                colvarAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarAmount);
                
                TableSchema.TableColumn colvarGrossSales = new TableSchema.TableColumn(schema);
                colvarGrossSales.ColumnName = "GrossSales";
                colvarGrossSales.DataType = DbType.Currency;
                colvarGrossSales.MaxLength = 0;
                colvarGrossSales.AutoIncrement = false;
                colvarGrossSales.IsNullable = true;
                colvarGrossSales.IsPrimaryKey = false;
                colvarGrossSales.IsForeignKey = false;
                colvarGrossSales.IsReadOnly = false;
                
                schema.Columns.Add(colvarGrossSales);
                
                TableSchema.TableColumn colvarIsFreeOfCharge = new TableSchema.TableColumn(schema);
                colvarIsFreeOfCharge.ColumnName = "IsFreeOfCharge";
                colvarIsFreeOfCharge.DataType = DbType.Boolean;
                colvarIsFreeOfCharge.MaxLength = 0;
                colvarIsFreeOfCharge.AutoIncrement = false;
                colvarIsFreeOfCharge.IsNullable = false;
                colvarIsFreeOfCharge.IsPrimaryKey = false;
                colvarIsFreeOfCharge.IsForeignKey = false;
                colvarIsFreeOfCharge.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsFreeOfCharge);
                
                TableSchema.TableColumn colvarCostOfGoodSold = new TableSchema.TableColumn(schema);
                colvarCostOfGoodSold.ColumnName = "CostOfGoodSold";
                colvarCostOfGoodSold.DataType = DbType.Currency;
                colvarCostOfGoodSold.MaxLength = 0;
                colvarCostOfGoodSold.AutoIncrement = false;
                colvarCostOfGoodSold.IsNullable = true;
                colvarCostOfGoodSold.IsPrimaryKey = false;
                colvarCostOfGoodSold.IsForeignKey = false;
                colvarCostOfGoodSold.IsReadOnly = false;
                
                schema.Columns.Add(colvarCostOfGoodSold);
                
                TableSchema.TableColumn colvarIsPromo = new TableSchema.TableColumn(schema);
                colvarIsPromo.ColumnName = "IsPromo";
                colvarIsPromo.DataType = DbType.Boolean;
                colvarIsPromo.MaxLength = 0;
                colvarIsPromo.AutoIncrement = false;
                colvarIsPromo.IsNullable = false;
                colvarIsPromo.IsPrimaryKey = false;
                colvarIsPromo.IsForeignKey = false;
                colvarIsPromo.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsPromo);
                
                TableSchema.TableColumn colvarPromoDiscount = new TableSchema.TableColumn(schema);
                colvarPromoDiscount.ColumnName = "PromoDiscount";
                colvarPromoDiscount.DataType = DbType.Double;
                colvarPromoDiscount.MaxLength = 0;
                colvarPromoDiscount.AutoIncrement = false;
                colvarPromoDiscount.IsNullable = false;
                colvarPromoDiscount.IsPrimaryKey = false;
                colvarPromoDiscount.IsForeignKey = false;
                colvarPromoDiscount.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoDiscount);
                
                TableSchema.TableColumn colvarPromoUnitPrice = new TableSchema.TableColumn(schema);
                colvarPromoUnitPrice.ColumnName = "PromoUnitPrice";
                colvarPromoUnitPrice.DataType = DbType.Currency;
                colvarPromoUnitPrice.MaxLength = 0;
                colvarPromoUnitPrice.AutoIncrement = false;
                colvarPromoUnitPrice.IsNullable = false;
                colvarPromoUnitPrice.IsPrimaryKey = false;
                colvarPromoUnitPrice.IsForeignKey = false;
                colvarPromoUnitPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoUnitPrice);
                
                TableSchema.TableColumn colvarPromoAmount = new TableSchema.TableColumn(schema);
                colvarPromoAmount.ColumnName = "PromoAmount";
                colvarPromoAmount.DataType = DbType.Currency;
                colvarPromoAmount.MaxLength = 0;
                colvarPromoAmount.AutoIncrement = false;
                colvarPromoAmount.IsNullable = false;
                colvarPromoAmount.IsPrimaryKey = false;
                colvarPromoAmount.IsForeignKey = false;
                colvarPromoAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoAmount);
                
                TableSchema.TableColumn colvarIsPromoFreeOfCharge = new TableSchema.TableColumn(schema);
                colvarIsPromoFreeOfCharge.ColumnName = "IsPromoFreeOfCharge";
                colvarIsPromoFreeOfCharge.DataType = DbType.Boolean;
                colvarIsPromoFreeOfCharge.MaxLength = 0;
                colvarIsPromoFreeOfCharge.AutoIncrement = false;
                colvarIsPromoFreeOfCharge.IsNullable = false;
                colvarIsPromoFreeOfCharge.IsPrimaryKey = false;
                colvarIsPromoFreeOfCharge.IsForeignKey = false;
                colvarIsPromoFreeOfCharge.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsPromoFreeOfCharge);
                
                TableSchema.TableColumn colvarUsePromoPrice = new TableSchema.TableColumn(schema);
                colvarUsePromoPrice.ColumnName = "UsePromoPrice";
                colvarUsePromoPrice.DataType = DbType.Boolean;
                colvarUsePromoPrice.MaxLength = 0;
                colvarUsePromoPrice.AutoIncrement = false;
                colvarUsePromoPrice.IsNullable = true;
                colvarUsePromoPrice.IsPrimaryKey = false;
                colvarUsePromoPrice.IsForeignKey = false;
                colvarUsePromoPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarUsePromoPrice);
                
                TableSchema.TableColumn colvarPromoHdrID = new TableSchema.TableColumn(schema);
                colvarPromoHdrID.ColumnName = "PromoHdrID";
                colvarPromoHdrID.DataType = DbType.Int32;
                colvarPromoHdrID.MaxLength = 0;
                colvarPromoHdrID.AutoIncrement = false;
                colvarPromoHdrID.IsNullable = true;
                colvarPromoHdrID.IsPrimaryKey = false;
                colvarPromoHdrID.IsForeignKey = false;
                colvarPromoHdrID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoHdrID);
                
                TableSchema.TableColumn colvarPromoDetID = new TableSchema.TableColumn(schema);
                colvarPromoDetID.ColumnName = "PromoDetID";
                colvarPromoDetID.DataType = DbType.Int32;
                colvarPromoDetID.MaxLength = 0;
                colvarPromoDetID.AutoIncrement = false;
                colvarPromoDetID.IsNullable = true;
                colvarPromoDetID.IsPrimaryKey = false;
                colvarPromoDetID.IsForeignKey = false;
                colvarPromoDetID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoDetID);
                
                TableSchema.TableColumn colvarVoucherNo = new TableSchema.TableColumn(schema);
                colvarVoucherNo.ColumnName = "VoucherNo";
                colvarVoucherNo.DataType = DbType.AnsiString;
                colvarVoucherNo.MaxLength = 50;
                colvarVoucherNo.AutoIncrement = false;
                colvarVoucherNo.IsNullable = true;
                colvarVoucherNo.IsPrimaryKey = false;
                colvarVoucherNo.IsForeignKey = false;
                colvarVoucherNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarVoucherNo);
                
                TableSchema.TableColumn colvarExpr3 = new TableSchema.TableColumn(schema);
                colvarExpr3.ColumnName = "Expr3";
                colvarExpr3.DataType = DbType.Boolean;
                colvarExpr3.MaxLength = 0;
                colvarExpr3.AutoIncrement = false;
                colvarExpr3.IsNullable = false;
                colvarExpr3.IsPrimaryKey = false;
                colvarExpr3.IsForeignKey = false;
                colvarExpr3.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr3);
                
                TableSchema.TableColumn colvarIsSpecial = new TableSchema.TableColumn(schema);
                colvarIsSpecial.ColumnName = "IsSpecial";
                colvarIsSpecial.DataType = DbType.Boolean;
                colvarIsSpecial.MaxLength = 0;
                colvarIsSpecial.AutoIncrement = false;
                colvarIsSpecial.IsNullable = false;
                colvarIsSpecial.IsPrimaryKey = false;
                colvarIsSpecial.IsForeignKey = false;
                colvarIsSpecial.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsSpecial);
                
                TableSchema.TableColumn colvarExpr4 = new TableSchema.TableColumn(schema);
                colvarExpr4.ColumnName = "Expr4";
                colvarExpr4.DataType = DbType.String;
                colvarExpr4.MaxLength = -1;
                colvarExpr4.AutoIncrement = false;
                colvarExpr4.IsNullable = true;
                colvarExpr4.IsPrimaryKey = false;
                colvarExpr4.IsForeignKey = false;
                colvarExpr4.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr4);
                
                TableSchema.TableColumn colvarIsEventPrice = new TableSchema.TableColumn(schema);
                colvarIsEventPrice.ColumnName = "IsEventPrice";
                colvarIsEventPrice.DataType = DbType.Boolean;
                colvarIsEventPrice.MaxLength = 0;
                colvarIsEventPrice.AutoIncrement = false;
                colvarIsEventPrice.IsNullable = true;
                colvarIsEventPrice.IsPrimaryKey = false;
                colvarIsEventPrice.IsForeignKey = false;
                colvarIsEventPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsEventPrice);
                
                TableSchema.TableColumn colvarSpecialEventID = new TableSchema.TableColumn(schema);
                colvarSpecialEventID.ColumnName = "SpecialEventID";
                colvarSpecialEventID.DataType = DbType.Int32;
                colvarSpecialEventID.MaxLength = 0;
                colvarSpecialEventID.AutoIncrement = false;
                colvarSpecialEventID.IsNullable = true;
                colvarSpecialEventID.IsPrimaryKey = false;
                colvarSpecialEventID.IsForeignKey = false;
                colvarSpecialEventID.IsReadOnly = false;
                
                schema.Columns.Add(colvarSpecialEventID);
                
                TableSchema.TableColumn colvarExpr5 = new TableSchema.TableColumn(schema);
                colvarExpr5.ColumnName = "Expr5";
                colvarExpr5.DataType = DbType.AnsiString;
                colvarExpr5.MaxLength = 14;
                colvarExpr5.AutoIncrement = false;
                colvarExpr5.IsNullable = false;
                colvarExpr5.IsPrimaryKey = false;
                colvarExpr5.IsForeignKey = false;
                colvarExpr5.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr5);
                
                TableSchema.TableColumn colvarIsPreOrder = new TableSchema.TableColumn(schema);
                colvarIsPreOrder.ColumnName = "IsPreOrder";
                colvarIsPreOrder.DataType = DbType.Boolean;
                colvarIsPreOrder.MaxLength = 0;
                colvarIsPreOrder.AutoIncrement = false;
                colvarIsPreOrder.IsNullable = true;
                colvarIsPreOrder.IsPrimaryKey = false;
                colvarIsPreOrder.IsForeignKey = false;
                colvarIsPreOrder.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsPreOrder);
                
                TableSchema.TableColumn colvarIsExchange = new TableSchema.TableColumn(schema);
                colvarIsExchange.ColumnName = "IsExchange";
                colvarIsExchange.DataType = DbType.Boolean;
                colvarIsExchange.MaxLength = 0;
                colvarIsExchange.AutoIncrement = false;
                colvarIsExchange.IsNullable = false;
                colvarIsExchange.IsPrimaryKey = false;
                colvarIsExchange.IsForeignKey = false;
                colvarIsExchange.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsExchange);
                
                TableSchema.TableColumn colvarIsPendingCollection = new TableSchema.TableColumn(schema);
                colvarIsPendingCollection.ColumnName = "IsPendingCollection";
                colvarIsPendingCollection.DataType = DbType.Boolean;
                colvarIsPendingCollection.MaxLength = 0;
                colvarIsPendingCollection.AutoIncrement = false;
                colvarIsPendingCollection.IsNullable = true;
                colvarIsPendingCollection.IsPrimaryKey = false;
                colvarIsPendingCollection.IsForeignKey = false;
                colvarIsPendingCollection.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsPendingCollection);
                
                TableSchema.TableColumn colvarExpr6 = new TableSchema.TableColumn(schema);
                colvarExpr6.ColumnName = "Expr6";
                colvarExpr6.DataType = DbType.AnsiString;
                colvarExpr6.MaxLength = 50;
                colvarExpr6.AutoIncrement = false;
                colvarExpr6.IsNullable = true;
                colvarExpr6.IsPrimaryKey = false;
                colvarExpr6.IsForeignKey = false;
                colvarExpr6.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr6);
                
                TableSchema.TableColumn colvarExchangeDetRefNo = new TableSchema.TableColumn(schema);
                colvarExchangeDetRefNo.ColumnName = "ExchangeDetRefNo";
                colvarExchangeDetRefNo.DataType = DbType.AnsiString;
                colvarExchangeDetRefNo.MaxLength = 50;
                colvarExchangeDetRefNo.AutoIncrement = false;
                colvarExchangeDetRefNo.IsNullable = true;
                colvarExchangeDetRefNo.IsPrimaryKey = false;
                colvarExchangeDetRefNo.IsForeignKey = false;
                colvarExchangeDetRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarExchangeDetRefNo);
                
                TableSchema.TableColumn colvarOriginalRetailPrice = new TableSchema.TableColumn(schema);
                colvarOriginalRetailPrice.ColumnName = "OriginalRetailPrice";
                colvarOriginalRetailPrice.DataType = DbType.Currency;
                colvarOriginalRetailPrice.MaxLength = 0;
                colvarOriginalRetailPrice.AutoIncrement = false;
                colvarOriginalRetailPrice.IsNullable = false;
                colvarOriginalRetailPrice.IsPrimaryKey = false;
                colvarOriginalRetailPrice.IsForeignKey = false;
                colvarOriginalRetailPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarOriginalRetailPrice);
                
                TableSchema.TableColumn colvarGSTAmount = new TableSchema.TableColumn(schema);
                colvarGSTAmount.ColumnName = "GSTAmount";
                colvarGSTAmount.DataType = DbType.Currency;
                colvarGSTAmount.MaxLength = 0;
                colvarGSTAmount.AutoIncrement = false;
                colvarGSTAmount.IsNullable = true;
                colvarGSTAmount.IsPrimaryKey = false;
                colvarGSTAmount.IsForeignKey = false;
                colvarGSTAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarGSTAmount);
                
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
                
                TableSchema.TableColumn colvarBarcode = new TableSchema.TableColumn(schema);
                colvarBarcode.ColumnName = "Barcode";
                colvarBarcode.DataType = DbType.AnsiString;
                colvarBarcode.MaxLength = 100;
                colvarBarcode.AutoIncrement = false;
                colvarBarcode.IsNullable = true;
                colvarBarcode.IsPrimaryKey = false;
                colvarBarcode.IsForeignKey = false;
                colvarBarcode.IsReadOnly = false;
                
                schema.Columns.Add(colvarBarcode);
                
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
                
                TableSchema.TableColumn colvarRetailPrice = new TableSchema.TableColumn(schema);
                colvarRetailPrice.ColumnName = "RetailPrice";
                colvarRetailPrice.DataType = DbType.Currency;
                colvarRetailPrice.MaxLength = 0;
                colvarRetailPrice.AutoIncrement = false;
                colvarRetailPrice.IsNullable = false;
                colvarRetailPrice.IsPrimaryKey = false;
                colvarRetailPrice.IsForeignKey = false;
                colvarRetailPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarRetailPrice);
                
                TableSchema.TableColumn colvarFactoryPrice = new TableSchema.TableColumn(schema);
                colvarFactoryPrice.ColumnName = "FactoryPrice";
                colvarFactoryPrice.DataType = DbType.Currency;
                colvarFactoryPrice.MaxLength = 0;
                colvarFactoryPrice.AutoIncrement = false;
                colvarFactoryPrice.IsNullable = false;
                colvarFactoryPrice.IsPrimaryKey = false;
                colvarFactoryPrice.IsForeignKey = false;
                colvarFactoryPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarFactoryPrice);
                
                TableSchema.TableColumn colvarMinimumPrice = new TableSchema.TableColumn(schema);
                colvarMinimumPrice.ColumnName = "MinimumPrice";
                colvarMinimumPrice.DataType = DbType.Currency;
                colvarMinimumPrice.MaxLength = 0;
                colvarMinimumPrice.AutoIncrement = false;
                colvarMinimumPrice.IsNullable = false;
                colvarMinimumPrice.IsPrimaryKey = false;
                colvarMinimumPrice.IsForeignKey = false;
                colvarMinimumPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarMinimumPrice);
                
                TableSchema.TableColumn colvarItemDesc = new TableSchema.TableColumn(schema);
                colvarItemDesc.ColumnName = "ItemDesc";
                colvarItemDesc.DataType = DbType.String;
                colvarItemDesc.MaxLength = 250;
                colvarItemDesc.AutoIncrement = false;
                colvarItemDesc.IsNullable = true;
                colvarItemDesc.IsPrimaryKey = false;
                colvarItemDesc.IsForeignKey = false;
                colvarItemDesc.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemDesc);
                
                TableSchema.TableColumn colvarIsServiceItem = new TableSchema.TableColumn(schema);
                colvarIsServiceItem.ColumnName = "IsServiceItem";
                colvarIsServiceItem.DataType = DbType.Boolean;
                colvarIsServiceItem.MaxLength = 0;
                colvarIsServiceItem.AutoIncrement = false;
                colvarIsServiceItem.IsNullable = true;
                colvarIsServiceItem.IsPrimaryKey = false;
                colvarIsServiceItem.IsForeignKey = false;
                colvarIsServiceItem.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsServiceItem);
                
                TableSchema.TableColumn colvarIsInInventory = new TableSchema.TableColumn(schema);
                colvarIsInInventory.ColumnName = "IsInInventory";
                colvarIsInInventory.DataType = DbType.Boolean;
                colvarIsInInventory.MaxLength = 0;
                colvarIsInInventory.AutoIncrement = false;
                colvarIsInInventory.IsNullable = false;
                colvarIsInInventory.IsPrimaryKey = false;
                colvarIsInInventory.IsForeignKey = false;
                colvarIsInInventory.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsInInventory);
                
                TableSchema.TableColumn colvarIsNonDiscountable = new TableSchema.TableColumn(schema);
                colvarIsNonDiscountable.ColumnName = "IsNonDiscountable";
                colvarIsNonDiscountable.DataType = DbType.Boolean;
                colvarIsNonDiscountable.MaxLength = 0;
                colvarIsNonDiscountable.AutoIncrement = false;
                colvarIsNonDiscountable.IsNullable = false;
                colvarIsNonDiscountable.IsPrimaryKey = false;
                colvarIsNonDiscountable.IsForeignKey = false;
                colvarIsNonDiscountable.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsNonDiscountable);
                
                TableSchema.TableColumn colvarIsCourse = new TableSchema.TableColumn(schema);
                colvarIsCourse.ColumnName = "IsCourse";
                colvarIsCourse.DataType = DbType.Boolean;
                colvarIsCourse.MaxLength = 0;
                colvarIsCourse.AutoIncrement = false;
                colvarIsCourse.IsNullable = true;
                colvarIsCourse.IsPrimaryKey = false;
                colvarIsCourse.IsForeignKey = false;
                colvarIsCourse.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsCourse);
                
                TableSchema.TableColumn colvarCourseTypeID = new TableSchema.TableColumn(schema);
                colvarCourseTypeID.ColumnName = "CourseTypeID";
                colvarCourseTypeID.DataType = DbType.AnsiString;
                colvarCourseTypeID.MaxLength = 50;
                colvarCourseTypeID.AutoIncrement = false;
                colvarCourseTypeID.IsNullable = true;
                colvarCourseTypeID.IsPrimaryKey = false;
                colvarCourseTypeID.IsForeignKey = false;
                colvarCourseTypeID.IsReadOnly = false;
                
                schema.Columns.Add(colvarCourseTypeID);
                
                TableSchema.TableColumn colvarBrand = new TableSchema.TableColumn(schema);
                colvarBrand.ColumnName = "Brand";
                colvarBrand.DataType = DbType.String;
                colvarBrand.MaxLength = 50;
                colvarBrand.AutoIncrement = false;
                colvarBrand.IsNullable = true;
                colvarBrand.IsPrimaryKey = false;
                colvarBrand.IsForeignKey = false;
                colvarBrand.IsReadOnly = false;
                
                schema.Columns.Add(colvarBrand);
                
                TableSchema.TableColumn colvarProductLine = new TableSchema.TableColumn(schema);
                colvarProductLine.ColumnName = "ProductLine";
                colvarProductLine.DataType = DbType.AnsiString;
                colvarProductLine.MaxLength = 50;
                colvarProductLine.AutoIncrement = false;
                colvarProductLine.IsNullable = true;
                colvarProductLine.IsPrimaryKey = false;
                colvarProductLine.IsForeignKey = false;
                colvarProductLine.IsReadOnly = false;
                
                schema.Columns.Add(colvarProductLine);
                
                TableSchema.TableColumn colvarAttributes1 = new TableSchema.TableColumn(schema);
                colvarAttributes1.ColumnName = "Attributes1";
                colvarAttributes1.DataType = DbType.String;
                colvarAttributes1.MaxLength = -1;
                colvarAttributes1.AutoIncrement = false;
                colvarAttributes1.IsNullable = true;
                colvarAttributes1.IsPrimaryKey = false;
                colvarAttributes1.IsForeignKey = false;
                colvarAttributes1.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes1);
                
                TableSchema.TableColumn colvarAttributes2 = new TableSchema.TableColumn(schema);
                colvarAttributes2.ColumnName = "Attributes2";
                colvarAttributes2.DataType = DbType.String;
                colvarAttributes2.MaxLength = -1;
                colvarAttributes2.AutoIncrement = false;
                colvarAttributes2.IsNullable = true;
                colvarAttributes2.IsPrimaryKey = false;
                colvarAttributes2.IsForeignKey = false;
                colvarAttributes2.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes2);
                
                TableSchema.TableColumn colvarAttributes3 = new TableSchema.TableColumn(schema);
                colvarAttributes3.ColumnName = "Attributes3";
                colvarAttributes3.DataType = DbType.String;
                colvarAttributes3.MaxLength = -1;
                colvarAttributes3.AutoIncrement = false;
                colvarAttributes3.IsNullable = true;
                colvarAttributes3.IsPrimaryKey = false;
                colvarAttributes3.IsForeignKey = false;
                colvarAttributes3.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes3);
                
                TableSchema.TableColumn colvarAttributes4 = new TableSchema.TableColumn(schema);
                colvarAttributes4.ColumnName = "Attributes4";
                colvarAttributes4.DataType = DbType.String;
                colvarAttributes4.MaxLength = -1;
                colvarAttributes4.AutoIncrement = false;
                colvarAttributes4.IsNullable = true;
                colvarAttributes4.IsPrimaryKey = false;
                colvarAttributes4.IsForeignKey = false;
                colvarAttributes4.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes4);
                
                TableSchema.TableColumn colvarAttributes5 = new TableSchema.TableColumn(schema);
                colvarAttributes5.ColumnName = "Attributes5";
                colvarAttributes5.DataType = DbType.String;
                colvarAttributes5.MaxLength = -1;
                colvarAttributes5.AutoIncrement = false;
                colvarAttributes5.IsNullable = true;
                colvarAttributes5.IsPrimaryKey = false;
                colvarAttributes5.IsForeignKey = false;
                colvarAttributes5.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes5);
                
                TableSchema.TableColumn colvarAttributes6 = new TableSchema.TableColumn(schema);
                colvarAttributes6.ColumnName = "Attributes6";
                colvarAttributes6.DataType = DbType.String;
                colvarAttributes6.MaxLength = -1;
                colvarAttributes6.AutoIncrement = false;
                colvarAttributes6.IsNullable = true;
                colvarAttributes6.IsPrimaryKey = false;
                colvarAttributes6.IsForeignKey = false;
                colvarAttributes6.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes6);
                
                TableSchema.TableColumn colvarAttributes7 = new TableSchema.TableColumn(schema);
                colvarAttributes7.ColumnName = "Attributes7";
                colvarAttributes7.DataType = DbType.String;
                colvarAttributes7.MaxLength = -1;
                colvarAttributes7.AutoIncrement = false;
                colvarAttributes7.IsNullable = true;
                colvarAttributes7.IsPrimaryKey = false;
                colvarAttributes7.IsForeignKey = false;
                colvarAttributes7.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes7);
                
                TableSchema.TableColumn colvarAttributes8 = new TableSchema.TableColumn(schema);
                colvarAttributes8.ColumnName = "Attributes8";
                colvarAttributes8.DataType = DbType.String;
                colvarAttributes8.MaxLength = -1;
                colvarAttributes8.AutoIncrement = false;
                colvarAttributes8.IsNullable = true;
                colvarAttributes8.IsPrimaryKey = false;
                colvarAttributes8.IsForeignKey = false;
                colvarAttributes8.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes8);
                
                TableSchema.TableColumn colvarExpr7 = new TableSchema.TableColumn(schema);
                colvarExpr7.ColumnName = "Expr7";
                colvarExpr7.DataType = DbType.String;
                colvarExpr7.MaxLength = -1;
                colvarExpr7.AutoIncrement = false;
                colvarExpr7.IsNullable = true;
                colvarExpr7.IsPrimaryKey = false;
                colvarExpr7.IsForeignKey = false;
                colvarExpr7.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr7);
                
                TableSchema.TableColumn colvarProductionDate = new TableSchema.TableColumn(schema);
                colvarProductionDate.ColumnName = "ProductionDate";
                colvarProductionDate.DataType = DbType.DateTime;
                colvarProductionDate.MaxLength = 0;
                colvarProductionDate.AutoIncrement = false;
                colvarProductionDate.IsNullable = true;
                colvarProductionDate.IsPrimaryKey = false;
                colvarProductionDate.IsForeignKey = false;
                colvarProductionDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarProductionDate);
                
                TableSchema.TableColumn colvarIsGST = new TableSchema.TableColumn(schema);
                colvarIsGST.ColumnName = "IsGST";
                colvarIsGST.DataType = DbType.Boolean;
                colvarIsGST.MaxLength = 0;
                colvarIsGST.AutoIncrement = false;
                colvarIsGST.IsNullable = true;
                colvarIsGST.IsPrimaryKey = false;
                colvarIsGST.IsForeignKey = false;
                colvarIsGST.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsGST);
                
                TableSchema.TableColumn colvarHasWarranty = new TableSchema.TableColumn(schema);
                colvarHasWarranty.ColumnName = "hasWarranty";
                colvarHasWarranty.DataType = DbType.Boolean;
                colvarHasWarranty.MaxLength = 0;
                colvarHasWarranty.AutoIncrement = false;
                colvarHasWarranty.IsNullable = true;
                colvarHasWarranty.IsPrimaryKey = false;
                colvarHasWarranty.IsForeignKey = false;
                colvarHasWarranty.IsReadOnly = false;
                
                schema.Columns.Add(colvarHasWarranty);
                
                TableSchema.TableColumn colvarIsDelivery = new TableSchema.TableColumn(schema);
                colvarIsDelivery.ColumnName = "IsDelivery";
                colvarIsDelivery.DataType = DbType.Boolean;
                colvarIsDelivery.MaxLength = 0;
                colvarIsDelivery.AutoIncrement = false;
                colvarIsDelivery.IsNullable = true;
                colvarIsDelivery.IsPrimaryKey = false;
                colvarIsDelivery.IsForeignKey = false;
                colvarIsDelivery.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsDelivery);
                
                TableSchema.TableColumn colvarGSTRule = new TableSchema.TableColumn(schema);
                colvarGSTRule.ColumnName = "GSTRule";
                colvarGSTRule.DataType = DbType.Int32;
                colvarGSTRule.MaxLength = 0;
                colvarGSTRule.AutoIncrement = false;
                colvarGSTRule.IsNullable = true;
                colvarGSTRule.IsPrimaryKey = false;
                colvarGSTRule.IsForeignKey = false;
                colvarGSTRule.IsReadOnly = false;
                
                schema.Columns.Add(colvarGSTRule);
                
                TableSchema.TableColumn colvarPackageDetID = new TableSchema.TableColumn(schema);
                colvarPackageDetID.ColumnName = "PackageDetID";
                colvarPackageDetID.DataType = DbType.AnsiString;
                colvarPackageDetID.MaxLength = 50;
                colvarPackageDetID.AutoIncrement = false;
                colvarPackageDetID.IsNullable = false;
                colvarPackageDetID.IsPrimaryKey = false;
                colvarPackageDetID.IsForeignKey = false;
                colvarPackageDetID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPackageDetID);
                
                TableSchema.TableColumn colvarOrderDetID = new TableSchema.TableColumn(schema);
                colvarOrderDetID.ColumnName = "OrderDetID";
                colvarOrderDetID.DataType = DbType.AnsiString;
                colvarOrderDetID.MaxLength = 18;
                colvarOrderDetID.AutoIncrement = false;
                colvarOrderDetID.IsNullable = false;
                colvarOrderDetID.IsPrimaryKey = false;
                colvarOrderDetID.IsForeignKey = false;
                colvarOrderDetID.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDetID);
                
                TableSchema.TableColumn colvarExpr8 = new TableSchema.TableColumn(schema);
                colvarExpr8.ColumnName = "Expr8";
                colvarExpr8.DataType = DbType.AnsiString;
                colvarExpr8.MaxLength = -1;
                colvarExpr8.AutoIncrement = false;
                colvarExpr8.IsNullable = false;
                colvarExpr8.IsPrimaryKey = false;
                colvarExpr8.IsForeignKey = false;
                colvarExpr8.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr8);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewPackages",schema);
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
	    public ViewPackage()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewPackage(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewPackage(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewPackage(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("PackageHdrId")]
        [Bindable(true)]
        public string PackageHdrId 
	    {
		    get
		    {
			    return GetColumnValue<string>("PackageHdrId");
		    }
            set 
		    {
			    SetColumnValue("PackageHdrId", value);
            }
        }
	      
        [XmlAttribute("OrderHdrId")]
        [Bindable(true)]
        public string OrderHdrId 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderHdrId");
		    }
            set 
		    {
			    SetColumnValue("OrderHdrId", value);
            }
        }
	      
        [XmlAttribute("Remark")]
        [Bindable(true)]
        public string Remark 
	    {
		    get
		    {
			    return GetColumnValue<string>("Remark");
		    }
            set 
		    {
			    SetColumnValue("Remark", value);
            }
        }
	      
        [XmlAttribute("CreatedOn")]
        [Bindable(true)]
        public DateTime? CreatedOn 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("CreatedOn");
		    }
            set 
		    {
			    SetColumnValue("CreatedOn", value);
            }
        }
	      
        [XmlAttribute("CreatedBy")]
        [Bindable(true)]
        public string CreatedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("CreatedBy");
		    }
            set 
		    {
			    SetColumnValue("CreatedBy", value);
            }
        }
	      
        [XmlAttribute("ModifiedOn")]
        [Bindable(true)]
        public DateTime? ModifiedOn 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ModifiedOn");
		    }
            set 
		    {
			    SetColumnValue("ModifiedOn", value);
            }
        }
	      
        [XmlAttribute("ModifiedBy")]
        [Bindable(true)]
        public string ModifiedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("ModifiedBy");
		    }
            set 
		    {
			    SetColumnValue("ModifiedBy", value);
            }
        }
	      
        [XmlAttribute("Deleted")]
        [Bindable(true)]
        public bool Deleted 
	    {
		    get
		    {
			    return GetColumnValue<bool>("Deleted");
		    }
            set 
		    {
			    SetColumnValue("Deleted", value);
            }
        }
	      
        [XmlAttribute("OrderRefNo")]
        [Bindable(true)]
        public string OrderRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderRefNo");
		    }
            set 
		    {
			    SetColumnValue("OrderRefNo", value);
            }
        }
	      
        [XmlAttribute("Discount")]
        [Bindable(true)]
        public decimal Discount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("Discount");
		    }
            set 
		    {
			    SetColumnValue("Discount", value);
            }
        }
	      
        [XmlAttribute("InventoryHdrRefNo")]
        [Bindable(true)]
        public string InventoryHdrRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("InventoryHdrRefNo");
		    }
            set 
		    {
			    SetColumnValue("InventoryHdrRefNo", value);
            }
        }
	      
        [XmlAttribute("CashierID")]
        [Bindable(true)]
        public string CashierID 
	    {
		    get
		    {
			    return GetColumnValue<string>("CashierID");
		    }
            set 
		    {
			    SetColumnValue("CashierID", value);
            }
        }
	      
        [XmlAttribute("PointOfSaleID")]
        [Bindable(true)]
        public int PointOfSaleID 
	    {
		    get
		    {
			    return GetColumnValue<int>("PointOfSaleID");
		    }
            set 
		    {
			    SetColumnValue("PointOfSaleID", value);
            }
        }
	      
        [XmlAttribute("OrderDate")]
        [Bindable(true)]
        public DateTime OrderDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("OrderDate");
		    }
            set 
		    {
			    SetColumnValue("OrderDate", value);
            }
        }
	      
        [XmlAttribute("GrossAmount")]
        [Bindable(true)]
        public decimal GrossAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("GrossAmount");
		    }
            set 
		    {
			    SetColumnValue("GrossAmount", value);
            }
        }
	      
        [XmlAttribute("NettAmount")]
        [Bindable(true)]
        public decimal NettAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("NettAmount");
		    }
            set 
		    {
			    SetColumnValue("NettAmount", value);
            }
        }
	      
        [XmlAttribute("DiscountAmount")]
        [Bindable(true)]
        public decimal DiscountAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("DiscountAmount");
		    }
            set 
		    {
			    SetColumnValue("DiscountAmount", value);
            }
        }
	      
        [XmlAttribute("Gst")]
        [Bindable(true)]
        public double Gst 
	    {
		    get
		    {
			    return GetColumnValue<double>("GST");
		    }
            set 
		    {
			    SetColumnValue("GST", value);
            }
        }
	      
        [XmlAttribute("IsVoided")]
        [Bindable(true)]
        public bool IsVoided 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsVoided");
		    }
            set 
		    {
			    SetColumnValue("IsVoided", value);
            }
        }
	      
        [XmlAttribute("MembershipNo")]
        [Bindable(true)]
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
	      
        [XmlAttribute("Expr1")]
        [Bindable(true)]
        public string Expr1 
	    {
		    get
		    {
			    return GetColumnValue<string>("Expr1");
		    }
            set 
		    {
			    SetColumnValue("Expr1", value);
            }
        }
	      
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
	      
        [XmlAttribute("OrderDetDate")]
        [Bindable(true)]
        public DateTime OrderDetDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("OrderDetDate");
		    }
            set 
		    {
			    SetColumnValue("OrderDetDate", value);
            }
        }
	      
        [XmlAttribute("Quantity")]
        [Bindable(true)]
        public int Quantity 
	    {
		    get
		    {
			    return GetColumnValue<int>("Quantity");
		    }
            set 
		    {
			    SetColumnValue("Quantity", value);
            }
        }
	      
        [XmlAttribute("UnitPrice")]
        [Bindable(true)]
        public decimal UnitPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("UnitPrice");
		    }
            set 
		    {
			    SetColumnValue("UnitPrice", value);
            }
        }
	      
        [XmlAttribute("Expr2")]
        [Bindable(true)]
        public decimal Expr2 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("Expr2");
		    }
            set 
		    {
			    SetColumnValue("Expr2", value);
            }
        }
	      
        [XmlAttribute("Amount")]
        [Bindable(true)]
        public decimal Amount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("Amount");
		    }
            set 
		    {
			    SetColumnValue("Amount", value);
            }
        }
	      
        [XmlAttribute("GrossSales")]
        [Bindable(true)]
        public decimal? GrossSales 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("GrossSales");
		    }
            set 
		    {
			    SetColumnValue("GrossSales", value);
            }
        }
	      
        [XmlAttribute("IsFreeOfCharge")]
        [Bindable(true)]
        public bool IsFreeOfCharge 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsFreeOfCharge");
		    }
            set 
		    {
			    SetColumnValue("IsFreeOfCharge", value);
            }
        }
	      
        [XmlAttribute("CostOfGoodSold")]
        [Bindable(true)]
        public decimal? CostOfGoodSold 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("CostOfGoodSold");
		    }
            set 
		    {
			    SetColumnValue("CostOfGoodSold", value);
            }
        }
	      
        [XmlAttribute("IsPromo")]
        [Bindable(true)]
        public bool IsPromo 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsPromo");
		    }
            set 
		    {
			    SetColumnValue("IsPromo", value);
            }
        }
	      
        [XmlAttribute("PromoDiscount")]
        [Bindable(true)]
        public double PromoDiscount 
	    {
		    get
		    {
			    return GetColumnValue<double>("PromoDiscount");
		    }
            set 
		    {
			    SetColumnValue("PromoDiscount", value);
            }
        }
	      
        [XmlAttribute("PromoUnitPrice")]
        [Bindable(true)]
        public decimal PromoUnitPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("PromoUnitPrice");
		    }
            set 
		    {
			    SetColumnValue("PromoUnitPrice", value);
            }
        }
	      
        [XmlAttribute("PromoAmount")]
        [Bindable(true)]
        public decimal PromoAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("PromoAmount");
		    }
            set 
		    {
			    SetColumnValue("PromoAmount", value);
            }
        }
	      
        [XmlAttribute("IsPromoFreeOfCharge")]
        [Bindable(true)]
        public bool IsPromoFreeOfCharge 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsPromoFreeOfCharge");
		    }
            set 
		    {
			    SetColumnValue("IsPromoFreeOfCharge", value);
            }
        }
	      
        [XmlAttribute("UsePromoPrice")]
        [Bindable(true)]
        public bool? UsePromoPrice 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("UsePromoPrice");
		    }
            set 
		    {
			    SetColumnValue("UsePromoPrice", value);
            }
        }
	      
        [XmlAttribute("PromoHdrID")]
        [Bindable(true)]
        public int? PromoHdrID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("PromoHdrID");
		    }
            set 
		    {
			    SetColumnValue("PromoHdrID", value);
            }
        }
	      
        [XmlAttribute("PromoDetID")]
        [Bindable(true)]
        public int? PromoDetID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("PromoDetID");
		    }
            set 
		    {
			    SetColumnValue("PromoDetID", value);
            }
        }
	      
        [XmlAttribute("VoucherNo")]
        [Bindable(true)]
        public string VoucherNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("VoucherNo");
		    }
            set 
		    {
			    SetColumnValue("VoucherNo", value);
            }
        }
	      
        [XmlAttribute("Expr3")]
        [Bindable(true)]
        public bool Expr3 
	    {
		    get
		    {
			    return GetColumnValue<bool>("Expr3");
		    }
            set 
		    {
			    SetColumnValue("Expr3", value);
            }
        }
	      
        [XmlAttribute("IsSpecial")]
        [Bindable(true)]
        public bool IsSpecial 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsSpecial");
		    }
            set 
		    {
			    SetColumnValue("IsSpecial", value);
            }
        }
	      
        [XmlAttribute("Expr4")]
        [Bindable(true)]
        public string Expr4 
	    {
		    get
		    {
			    return GetColumnValue<string>("Expr4");
		    }
            set 
		    {
			    SetColumnValue("Expr4", value);
            }
        }
	      
        [XmlAttribute("IsEventPrice")]
        [Bindable(true)]
        public bool? IsEventPrice 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsEventPrice");
		    }
            set 
		    {
			    SetColumnValue("IsEventPrice", value);
            }
        }
	      
        [XmlAttribute("SpecialEventID")]
        [Bindable(true)]
        public int? SpecialEventID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("SpecialEventID");
		    }
            set 
		    {
			    SetColumnValue("SpecialEventID", value);
            }
        }
	      
        [XmlAttribute("Expr5")]
        [Bindable(true)]
        public string Expr5 
	    {
		    get
		    {
			    return GetColumnValue<string>("Expr5");
		    }
            set 
		    {
			    SetColumnValue("Expr5", value);
            }
        }
	      
        [XmlAttribute("IsPreOrder")]
        [Bindable(true)]
        public bool? IsPreOrder 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsPreOrder");
		    }
            set 
		    {
			    SetColumnValue("IsPreOrder", value);
            }
        }
	      
        [XmlAttribute("IsExchange")]
        [Bindable(true)]
        public bool IsExchange 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsExchange");
		    }
            set 
		    {
			    SetColumnValue("IsExchange", value);
            }
        }
	      
        [XmlAttribute("IsPendingCollection")]
        [Bindable(true)]
        public bool? IsPendingCollection 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsPendingCollection");
		    }
            set 
		    {
			    SetColumnValue("IsPendingCollection", value);
            }
        }
	      
        [XmlAttribute("Expr6")]
        [Bindable(true)]
        public string Expr6 
	    {
		    get
		    {
			    return GetColumnValue<string>("Expr6");
		    }
            set 
		    {
			    SetColumnValue("Expr6", value);
            }
        }
	      
        [XmlAttribute("ExchangeDetRefNo")]
        [Bindable(true)]
        public string ExchangeDetRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("ExchangeDetRefNo");
		    }
            set 
		    {
			    SetColumnValue("ExchangeDetRefNo", value);
            }
        }
	      
        [XmlAttribute("OriginalRetailPrice")]
        [Bindable(true)]
        public decimal OriginalRetailPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("OriginalRetailPrice");
		    }
            set 
		    {
			    SetColumnValue("OriginalRetailPrice", value);
            }
        }
	      
        [XmlAttribute("GSTAmount")]
        [Bindable(true)]
        public decimal? GSTAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("GSTAmount");
		    }
            set 
		    {
			    SetColumnValue("GSTAmount", value);
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
	      
        [XmlAttribute("Barcode")]
        [Bindable(true)]
        public string Barcode 
	    {
		    get
		    {
			    return GetColumnValue<string>("Barcode");
		    }
            set 
		    {
			    SetColumnValue("Barcode", value);
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
	      
        [XmlAttribute("RetailPrice")]
        [Bindable(true)]
        public decimal RetailPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("RetailPrice");
		    }
            set 
		    {
			    SetColumnValue("RetailPrice", value);
            }
        }
	      
        [XmlAttribute("FactoryPrice")]
        [Bindable(true)]
        public decimal FactoryPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("FactoryPrice");
		    }
            set 
		    {
			    SetColumnValue("FactoryPrice", value);
            }
        }
	      
        [XmlAttribute("MinimumPrice")]
        [Bindable(true)]
        public decimal MinimumPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("MinimumPrice");
		    }
            set 
		    {
			    SetColumnValue("MinimumPrice", value);
            }
        }
	      
        [XmlAttribute("ItemDesc")]
        [Bindable(true)]
        public string ItemDesc 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemDesc");
		    }
            set 
		    {
			    SetColumnValue("ItemDesc", value);
            }
        }
	      
        [XmlAttribute("IsServiceItem")]
        [Bindable(true)]
        public bool? IsServiceItem 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsServiceItem");
		    }
            set 
		    {
			    SetColumnValue("IsServiceItem", value);
            }
        }
	      
        [XmlAttribute("IsInInventory")]
        [Bindable(true)]
        public bool IsInInventory 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsInInventory");
		    }
            set 
		    {
			    SetColumnValue("IsInInventory", value);
            }
        }
	      
        [XmlAttribute("IsNonDiscountable")]
        [Bindable(true)]
        public bool IsNonDiscountable 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsNonDiscountable");
		    }
            set 
		    {
			    SetColumnValue("IsNonDiscountable", value);
            }
        }
	      
        [XmlAttribute("IsCourse")]
        [Bindable(true)]
        public bool? IsCourse 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsCourse");
		    }
            set 
		    {
			    SetColumnValue("IsCourse", value);
            }
        }
	      
        [XmlAttribute("CourseTypeID")]
        [Bindable(true)]
        public string CourseTypeID 
	    {
		    get
		    {
			    return GetColumnValue<string>("CourseTypeID");
		    }
            set 
		    {
			    SetColumnValue("CourseTypeID", value);
            }
        }
	      
        [XmlAttribute("Brand")]
        [Bindable(true)]
        public string Brand 
	    {
		    get
		    {
			    return GetColumnValue<string>("Brand");
		    }
            set 
		    {
			    SetColumnValue("Brand", value);
            }
        }
	      
        [XmlAttribute("ProductLine")]
        [Bindable(true)]
        public string ProductLine 
	    {
		    get
		    {
			    return GetColumnValue<string>("ProductLine");
		    }
            set 
		    {
			    SetColumnValue("ProductLine", value);
            }
        }
	      
        [XmlAttribute("Attributes1")]
        [Bindable(true)]
        public string Attributes1 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes1");
		    }
            set 
		    {
			    SetColumnValue("Attributes1", value);
            }
        }
	      
        [XmlAttribute("Attributes2")]
        [Bindable(true)]
        public string Attributes2 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes2");
		    }
            set 
		    {
			    SetColumnValue("Attributes2", value);
            }
        }
	      
        [XmlAttribute("Attributes3")]
        [Bindable(true)]
        public string Attributes3 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes3");
		    }
            set 
		    {
			    SetColumnValue("Attributes3", value);
            }
        }
	      
        [XmlAttribute("Attributes4")]
        [Bindable(true)]
        public string Attributes4 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes4");
		    }
            set 
		    {
			    SetColumnValue("Attributes4", value);
            }
        }
	      
        [XmlAttribute("Attributes5")]
        [Bindable(true)]
        public string Attributes5 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes5");
		    }
            set 
		    {
			    SetColumnValue("Attributes5", value);
            }
        }
	      
        [XmlAttribute("Attributes6")]
        [Bindable(true)]
        public string Attributes6 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes6");
		    }
            set 
		    {
			    SetColumnValue("Attributes6", value);
            }
        }
	      
        [XmlAttribute("Attributes7")]
        [Bindable(true)]
        public string Attributes7 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes7");
		    }
            set 
		    {
			    SetColumnValue("Attributes7", value);
            }
        }
	      
        [XmlAttribute("Attributes8")]
        [Bindable(true)]
        public string Attributes8 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes8");
		    }
            set 
		    {
			    SetColumnValue("Attributes8", value);
            }
        }
	      
        [XmlAttribute("Expr7")]
        [Bindable(true)]
        public string Expr7 
	    {
		    get
		    {
			    return GetColumnValue<string>("Expr7");
		    }
            set 
		    {
			    SetColumnValue("Expr7", value);
            }
        }
	      
        [XmlAttribute("ProductionDate")]
        [Bindable(true)]
        public DateTime? ProductionDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ProductionDate");
		    }
            set 
		    {
			    SetColumnValue("ProductionDate", value);
            }
        }
	      
        [XmlAttribute("IsGST")]
        [Bindable(true)]
        public bool? IsGST 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsGST");
		    }
            set 
		    {
			    SetColumnValue("IsGST", value);
            }
        }
	      
        [XmlAttribute("HasWarranty")]
        [Bindable(true)]
        public bool? HasWarranty 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("hasWarranty");
		    }
            set 
		    {
			    SetColumnValue("hasWarranty", value);
            }
        }
	      
        [XmlAttribute("IsDelivery")]
        [Bindable(true)]
        public bool? IsDelivery 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsDelivery");
		    }
            set 
		    {
			    SetColumnValue("IsDelivery", value);
            }
        }
	      
        [XmlAttribute("GSTRule")]
        [Bindable(true)]
        public int? GSTRule 
	    {
		    get
		    {
			    return GetColumnValue<int?>("GSTRule");
		    }
            set 
		    {
			    SetColumnValue("GSTRule", value);
            }
        }
	      
        [XmlAttribute("PackageDetID")]
        [Bindable(true)]
        public string PackageDetID 
	    {
		    get
		    {
			    return GetColumnValue<string>("PackageDetID");
		    }
            set 
		    {
			    SetColumnValue("PackageDetID", value);
            }
        }
	      
        [XmlAttribute("OrderDetID")]
        [Bindable(true)]
        public string OrderDetID 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderDetID");
		    }
            set 
		    {
			    SetColumnValue("OrderDetID", value);
            }
        }
	      
        [XmlAttribute("Expr8")]
        [Bindable(true)]
        public string Expr8 
	    {
		    get
		    {
			    return GetColumnValue<string>("Expr8");
		    }
            set 
		    {
			    SetColumnValue("Expr8", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string PackageHdrId = @"PackageHdrId";
            
            public static string OrderHdrId = @"OrderHdrId";
            
            public static string Remark = @"Remark";
            
            public static string CreatedOn = @"CreatedOn";
            
            public static string CreatedBy = @"CreatedBy";
            
            public static string ModifiedOn = @"ModifiedOn";
            
            public static string ModifiedBy = @"ModifiedBy";
            
            public static string Deleted = @"Deleted";
            
            public static string OrderRefNo = @"OrderRefNo";
            
            public static string Discount = @"Discount";
            
            public static string InventoryHdrRefNo = @"InventoryHdrRefNo";
            
            public static string CashierID = @"CashierID";
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string OrderDate = @"OrderDate";
            
            public static string GrossAmount = @"GrossAmount";
            
            public static string NettAmount = @"NettAmount";
            
            public static string DiscountAmount = @"DiscountAmount";
            
            public static string Gst = @"GST";
            
            public static string IsVoided = @"IsVoided";
            
            public static string MembershipNo = @"MembershipNo";
            
            public static string Expr1 = @"Expr1";
            
            public static string ItemNo = @"ItemNo";
            
            public static string OrderDetDate = @"OrderDetDate";
            
            public static string Quantity = @"Quantity";
            
            public static string UnitPrice = @"UnitPrice";
            
            public static string Expr2 = @"Expr2";
            
            public static string Amount = @"Amount";
            
            public static string GrossSales = @"GrossSales";
            
            public static string IsFreeOfCharge = @"IsFreeOfCharge";
            
            public static string CostOfGoodSold = @"CostOfGoodSold";
            
            public static string IsPromo = @"IsPromo";
            
            public static string PromoDiscount = @"PromoDiscount";
            
            public static string PromoUnitPrice = @"PromoUnitPrice";
            
            public static string PromoAmount = @"PromoAmount";
            
            public static string IsPromoFreeOfCharge = @"IsPromoFreeOfCharge";
            
            public static string UsePromoPrice = @"UsePromoPrice";
            
            public static string PromoHdrID = @"PromoHdrID";
            
            public static string PromoDetID = @"PromoDetID";
            
            public static string VoucherNo = @"VoucherNo";
            
            public static string Expr3 = @"Expr3";
            
            public static string IsSpecial = @"IsSpecial";
            
            public static string Expr4 = @"Expr4";
            
            public static string IsEventPrice = @"IsEventPrice";
            
            public static string SpecialEventID = @"SpecialEventID";
            
            public static string Expr5 = @"Expr5";
            
            public static string IsPreOrder = @"IsPreOrder";
            
            public static string IsExchange = @"IsExchange";
            
            public static string IsPendingCollection = @"IsPendingCollection";
            
            public static string Expr6 = @"Expr6";
            
            public static string ExchangeDetRefNo = @"ExchangeDetRefNo";
            
            public static string OriginalRetailPrice = @"OriginalRetailPrice";
            
            public static string GSTAmount = @"GSTAmount";
            
            public static string ItemName = @"ItemName";
            
            public static string Barcode = @"Barcode";
            
            public static string CategoryName = @"CategoryName";
            
            public static string RetailPrice = @"RetailPrice";
            
            public static string FactoryPrice = @"FactoryPrice";
            
            public static string MinimumPrice = @"MinimumPrice";
            
            public static string ItemDesc = @"ItemDesc";
            
            public static string IsServiceItem = @"IsServiceItem";
            
            public static string IsInInventory = @"IsInInventory";
            
            public static string IsNonDiscountable = @"IsNonDiscountable";
            
            public static string IsCourse = @"IsCourse";
            
            public static string CourseTypeID = @"CourseTypeID";
            
            public static string Brand = @"Brand";
            
            public static string ProductLine = @"ProductLine";
            
            public static string Attributes1 = @"Attributes1";
            
            public static string Attributes2 = @"Attributes2";
            
            public static string Attributes3 = @"Attributes3";
            
            public static string Attributes4 = @"Attributes4";
            
            public static string Attributes5 = @"Attributes5";
            
            public static string Attributes6 = @"Attributes6";
            
            public static string Attributes7 = @"Attributes7";
            
            public static string Attributes8 = @"Attributes8";
            
            public static string Expr7 = @"Expr7";
            
            public static string ProductionDate = @"ProductionDate";
            
            public static string IsGST = @"IsGST";
            
            public static string HasWarranty = @"hasWarranty";
            
            public static string IsDelivery = @"IsDelivery";
            
            public static string GSTRule = @"GSTRule";
            
            public static string PackageDetID = @"PackageDetID";
            
            public static string OrderDetID = @"OrderDetID";
            
            public static string Expr8 = @"Expr8";
            
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
