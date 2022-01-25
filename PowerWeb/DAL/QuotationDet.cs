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
	/// Strongly-typed collection for the QuotationDet class.
	/// </summary>
    [Serializable]
	public partial class QuotationDetCollection : ActiveList<QuotationDet, QuotationDetCollection>
	{	   
		public QuotationDetCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>QuotationDetCollection</returns>
		public QuotationDetCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                QuotationDet o = this[i];
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
	/// This is an ActiveRecord class which wraps the QuotationDet table.
	/// </summary>
	[Serializable]
	public partial class QuotationDet : ActiveRecord<QuotationDet>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public QuotationDet()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public QuotationDet(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public QuotationDet(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public QuotationDet(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("QuotationDet", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarOrderDetID = new TableSchema.TableColumn(schema);
				colvarOrderDetID.ColumnName = "OrderDetID";
				colvarOrderDetID.DataType = DbType.AnsiString;
				colvarOrderDetID.MaxLength = 18;
				colvarOrderDetID.AutoIncrement = false;
				colvarOrderDetID.IsNullable = false;
				colvarOrderDetID.IsPrimaryKey = true;
				colvarOrderDetID.IsForeignKey = false;
				colvarOrderDetID.IsReadOnly = false;
				colvarOrderDetID.DefaultSetting = @"";
				colvarOrderDetID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderDetID);
				
				TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
				colvarItemNo.ColumnName = "ItemNo";
				colvarItemNo.DataType = DbType.AnsiString;
				colvarItemNo.MaxLength = 50;
				colvarItemNo.AutoIncrement = false;
				colvarItemNo.IsNullable = false;
				colvarItemNo.IsPrimaryKey = false;
				colvarItemNo.IsForeignKey = true;
				colvarItemNo.IsReadOnly = false;
				colvarItemNo.DefaultSetting = @"";
				
					colvarItemNo.ForeignKeyTableName = "Item";
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
				colvarOrderDetDate.DefaultSetting = @"";
				colvarOrderDetDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderDetDate);
				
				TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
				colvarQuantity.ColumnName = "Quantity";
				colvarQuantity.DataType = DbType.Decimal;
				colvarQuantity.MaxLength = 0;
				colvarQuantity.AutoIncrement = false;
				colvarQuantity.IsNullable = false;
				colvarQuantity.IsPrimaryKey = false;
				colvarQuantity.IsForeignKey = false;
				colvarQuantity.IsReadOnly = false;
				colvarQuantity.DefaultSetting = @"";
				colvarQuantity.ForeignKeyTableName = "";
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
				colvarUnitPrice.DefaultSetting = @"";
				colvarUnitPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUnitPrice);
				
				TableSchema.TableColumn colvarDiscount = new TableSchema.TableColumn(schema);
				colvarDiscount.ColumnName = "Discount";
				colvarDiscount.DataType = DbType.Currency;
				colvarDiscount.MaxLength = 0;
				colvarDiscount.AutoIncrement = false;
				colvarDiscount.IsNullable = false;
				colvarDiscount.IsPrimaryKey = false;
				colvarDiscount.IsForeignKey = false;
				colvarDiscount.IsReadOnly = false;
				
						colvarDiscount.DefaultSetting = @"((0))";
				colvarDiscount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDiscount);
				
				TableSchema.TableColumn colvarAmount = new TableSchema.TableColumn(schema);
				colvarAmount.ColumnName = "Amount";
				colvarAmount.DataType = DbType.Currency;
				colvarAmount.MaxLength = 0;
				colvarAmount.AutoIncrement = false;
				colvarAmount.IsNullable = false;
				colvarAmount.IsPrimaryKey = false;
				colvarAmount.IsForeignKey = false;
				colvarAmount.IsReadOnly = false;
				
						colvarAmount.DefaultSetting = @"((0))";
				colvarAmount.ForeignKeyTableName = "";
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
				colvarGrossSales.DefaultSetting = @"";
				colvarGrossSales.ForeignKeyTableName = "";
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
				colvarIsFreeOfCharge.DefaultSetting = @"";
				colvarIsFreeOfCharge.ForeignKeyTableName = "";
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
				colvarCostOfGoodSold.DefaultSetting = @"";
				colvarCostOfGoodSold.ForeignKeyTableName = "";
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
				colvarIsPromo.DefaultSetting = @"";
				colvarIsPromo.ForeignKeyTableName = "";
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
				colvarPromoDiscount.DefaultSetting = @"";
				colvarPromoDiscount.ForeignKeyTableName = "";
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
				
						colvarPromoUnitPrice.DefaultSetting = @"((0))";
				colvarPromoUnitPrice.ForeignKeyTableName = "";
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
				colvarPromoAmount.DefaultSetting = @"";
				colvarPromoAmount.ForeignKeyTableName = "";
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
				colvarIsPromoFreeOfCharge.DefaultSetting = @"";
				colvarIsPromoFreeOfCharge.ForeignKeyTableName = "";
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
				colvarUsePromoPrice.DefaultSetting = @"";
				colvarUsePromoPrice.ForeignKeyTableName = "";
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
				colvarPromoHdrID.DefaultSetting = @"";
				colvarPromoHdrID.ForeignKeyTableName = "";
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
				colvarPromoDetID.DefaultSetting = @"";
				colvarPromoDetID.ForeignKeyTableName = "";
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
				colvarVoucherNo.DefaultSetting = @"";
				colvarVoucherNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVoucherNo);
				
				TableSchema.TableColumn colvarIsVoided = new TableSchema.TableColumn(schema);
				colvarIsVoided.ColumnName = "IsVoided";
				colvarIsVoided.DataType = DbType.Boolean;
				colvarIsVoided.MaxLength = 0;
				colvarIsVoided.AutoIncrement = false;
				colvarIsVoided.IsNullable = false;
				colvarIsVoided.IsPrimaryKey = false;
				colvarIsVoided.IsForeignKey = false;
				colvarIsVoided.IsReadOnly = false;
				colvarIsVoided.DefaultSetting = @"";
				colvarIsVoided.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsVoided);
				
				TableSchema.TableColumn colvarIsSpecial = new TableSchema.TableColumn(schema);
				colvarIsSpecial.ColumnName = "IsSpecial";
				colvarIsSpecial.DataType = DbType.Boolean;
				colvarIsSpecial.MaxLength = 0;
				colvarIsSpecial.AutoIncrement = false;
				colvarIsSpecial.IsNullable = false;
				colvarIsSpecial.IsPrimaryKey = false;
				colvarIsSpecial.IsForeignKey = false;
				colvarIsSpecial.IsReadOnly = false;
				colvarIsSpecial.DefaultSetting = @"";
				colvarIsSpecial.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsSpecial);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.String;
				colvarRemark.MaxLength = -1;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = true;
				colvarRemark.IsPrimaryKey = false;
				colvarRemark.IsForeignKey = false;
				colvarRemark.IsReadOnly = false;
				colvarRemark.DefaultSetting = @"";
				colvarRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemark);
				
				TableSchema.TableColumn colvarIsEventPrice = new TableSchema.TableColumn(schema);
				colvarIsEventPrice.ColumnName = "IsEventPrice";
				colvarIsEventPrice.DataType = DbType.Boolean;
				colvarIsEventPrice.MaxLength = 0;
				colvarIsEventPrice.AutoIncrement = false;
				colvarIsEventPrice.IsNullable = true;
				colvarIsEventPrice.IsPrimaryKey = false;
				colvarIsEventPrice.IsForeignKey = false;
				colvarIsEventPrice.IsReadOnly = false;
				colvarIsEventPrice.DefaultSetting = @"";
				colvarIsEventPrice.ForeignKeyTableName = "";
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
				colvarSpecialEventID.DefaultSetting = @"";
				colvarSpecialEventID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSpecialEventID);
				
				TableSchema.TableColumn colvarOrderHdrID = new TableSchema.TableColumn(schema);
				colvarOrderHdrID.ColumnName = "OrderHdrID";
				colvarOrderHdrID.DataType = DbType.AnsiString;
				colvarOrderHdrID.MaxLength = 14;
				colvarOrderHdrID.AutoIncrement = false;
				colvarOrderHdrID.IsNullable = false;
				colvarOrderHdrID.IsPrimaryKey = false;
				colvarOrderHdrID.IsForeignKey = false;
				colvarOrderHdrID.IsReadOnly = false;
				colvarOrderHdrID.DefaultSetting = @"";
				colvarOrderHdrID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderHdrID);
				
				TableSchema.TableColumn colvarIsPreOrder = new TableSchema.TableColumn(schema);
				colvarIsPreOrder.ColumnName = "IsPreOrder";
				colvarIsPreOrder.DataType = DbType.Boolean;
				colvarIsPreOrder.MaxLength = 0;
				colvarIsPreOrder.AutoIncrement = false;
				colvarIsPreOrder.IsNullable = true;
				colvarIsPreOrder.IsPrimaryKey = false;
				colvarIsPreOrder.IsForeignKey = false;
				colvarIsPreOrder.IsReadOnly = false;
				colvarIsPreOrder.DefaultSetting = @"";
				colvarIsPreOrder.ForeignKeyTableName = "";
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
				colvarIsExchange.DefaultSetting = @"";
				colvarIsExchange.ForeignKeyTableName = "";
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
				colvarIsPendingCollection.DefaultSetting = @"";
				colvarIsPendingCollection.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsPendingCollection);
				
				TableSchema.TableColumn colvarGiveCommission = new TableSchema.TableColumn(schema);
				colvarGiveCommission.ColumnName = "giveCommission";
				colvarGiveCommission.DataType = DbType.Boolean;
				colvarGiveCommission.MaxLength = 0;
				colvarGiveCommission.AutoIncrement = false;
				colvarGiveCommission.IsNullable = true;
				colvarGiveCommission.IsPrimaryKey = false;
				colvarGiveCommission.IsForeignKey = false;
				colvarGiveCommission.IsReadOnly = false;
				colvarGiveCommission.DefaultSetting = @"";
				colvarGiveCommission.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGiveCommission);
				
				TableSchema.TableColumn colvarInventoryHdrRefNo = new TableSchema.TableColumn(schema);
				colvarInventoryHdrRefNo.ColumnName = "InventoryHdrRefNo";
				colvarInventoryHdrRefNo.DataType = DbType.AnsiString;
				colvarInventoryHdrRefNo.MaxLength = 50;
				colvarInventoryHdrRefNo.AutoIncrement = false;
				colvarInventoryHdrRefNo.IsNullable = true;
				colvarInventoryHdrRefNo.IsPrimaryKey = false;
				colvarInventoryHdrRefNo.IsForeignKey = false;
				colvarInventoryHdrRefNo.IsReadOnly = false;
				colvarInventoryHdrRefNo.DefaultSetting = @"";
				colvarInventoryHdrRefNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInventoryHdrRefNo);
				
				TableSchema.TableColumn colvarExchangeDetRefNo = new TableSchema.TableColumn(schema);
				colvarExchangeDetRefNo.ColumnName = "ExchangeDetRefNo";
				colvarExchangeDetRefNo.DataType = DbType.AnsiString;
				colvarExchangeDetRefNo.MaxLength = 50;
				colvarExchangeDetRefNo.AutoIncrement = false;
				colvarExchangeDetRefNo.IsNullable = true;
				colvarExchangeDetRefNo.IsPrimaryKey = false;
				colvarExchangeDetRefNo.IsForeignKey = false;
				colvarExchangeDetRefNo.IsReadOnly = false;
				colvarExchangeDetRefNo.DefaultSetting = @"";
				colvarExchangeDetRefNo.ForeignKeyTableName = "";
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
				
						colvarOriginalRetailPrice.DefaultSetting = @"((0))";
				colvarOriginalRetailPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOriginalRetailPrice);
				
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
				
				TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
				colvarUniqueID.ColumnName = "UniqueID";
				colvarUniqueID.DataType = DbType.Guid;
				colvarUniqueID.MaxLength = 0;
				colvarUniqueID.AutoIncrement = false;
				colvarUniqueID.IsNullable = false;
				colvarUniqueID.IsPrimaryKey = false;
				colvarUniqueID.IsForeignKey = false;
				colvarUniqueID.IsReadOnly = false;
				
						colvarUniqueID.DefaultSetting = @"(newid())";
				colvarUniqueID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueID);
				
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
				
				TableSchema.TableColumn colvarGSTAmount = new TableSchema.TableColumn(schema);
				colvarGSTAmount.ColumnName = "GSTAmount";
				colvarGSTAmount.DataType = DbType.Currency;
				colvarGSTAmount.MaxLength = 0;
				colvarGSTAmount.AutoIncrement = false;
				colvarGSTAmount.IsNullable = true;
				colvarGSTAmount.IsPrimaryKey = false;
				colvarGSTAmount.IsForeignKey = false;
				colvarGSTAmount.IsReadOnly = false;
				colvarGSTAmount.DefaultSetting = @"";
				colvarGSTAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGSTAmount);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("QuotationDet",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("OrderDetID")]
		[Bindable(true)]
		public string OrderDetID 
		{
			get { return GetColumnValue<string>(Columns.OrderDetID); }
			set { SetColumnValue(Columns.OrderDetID, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
		}
		  
		[XmlAttribute("OrderDetDate")]
		[Bindable(true)]
		public DateTime OrderDetDate 
		{
			get { return GetColumnValue<DateTime>(Columns.OrderDetDate); }
			set { SetColumnValue(Columns.OrderDetDate, value); }
		}
		  
		[XmlAttribute("Quantity")]
		[Bindable(true)]
		public decimal Quantity 
		{
			get { return GetColumnValue<decimal>(Columns.Quantity); }
			set { SetColumnValue(Columns.Quantity, value); }
		}
		  
		[XmlAttribute("UnitPrice")]
		[Bindable(true)]
		public decimal UnitPrice 
		{
			get { return GetColumnValue<decimal>(Columns.UnitPrice); }
			set { SetColumnValue(Columns.UnitPrice, value); }
		}
		  
		[XmlAttribute("Discount")]
		[Bindable(true)]
		public decimal Discount 
		{
			get { return GetColumnValue<decimal>(Columns.Discount); }
			set { SetColumnValue(Columns.Discount, value); }
		}
		  
		[XmlAttribute("Amount")]
		[Bindable(true)]
		public decimal Amount 
		{
			get { return GetColumnValue<decimal>(Columns.Amount); }
			set { SetColumnValue(Columns.Amount, value); }
		}
		  
		[XmlAttribute("GrossSales")]
		[Bindable(true)]
		public decimal? GrossSales 
		{
			get { return GetColumnValue<decimal?>(Columns.GrossSales); }
			set { SetColumnValue(Columns.GrossSales, value); }
		}
		  
		[XmlAttribute("IsFreeOfCharge")]
		[Bindable(true)]
		public bool IsFreeOfCharge 
		{
			get { return GetColumnValue<bool>(Columns.IsFreeOfCharge); }
			set { SetColumnValue(Columns.IsFreeOfCharge, value); }
		}
		  
		[XmlAttribute("CostOfGoodSold")]
		[Bindable(true)]
		public decimal? CostOfGoodSold 
		{
			get { return GetColumnValue<decimal?>(Columns.CostOfGoodSold); }
			set { SetColumnValue(Columns.CostOfGoodSold, value); }
		}
		  
		[XmlAttribute("IsPromo")]
		[Bindable(true)]
		public bool IsPromo 
		{
			get { return GetColumnValue<bool>(Columns.IsPromo); }
			set { SetColumnValue(Columns.IsPromo, value); }
		}
		  
		[XmlAttribute("PromoDiscount")]
		[Bindable(true)]
		public double PromoDiscount 
		{
			get { return GetColumnValue<double>(Columns.PromoDiscount); }
			set { SetColumnValue(Columns.PromoDiscount, value); }
		}
		  
		[XmlAttribute("PromoUnitPrice")]
		[Bindable(true)]
		public decimal PromoUnitPrice 
		{
			get { return GetColumnValue<decimal>(Columns.PromoUnitPrice); }
			set { SetColumnValue(Columns.PromoUnitPrice, value); }
		}
		  
		[XmlAttribute("PromoAmount")]
		[Bindable(true)]
		public decimal PromoAmount 
		{
			get { return GetColumnValue<decimal>(Columns.PromoAmount); }
			set { SetColumnValue(Columns.PromoAmount, value); }
		}
		  
		[XmlAttribute("IsPromoFreeOfCharge")]
		[Bindable(true)]
		public bool IsPromoFreeOfCharge 
		{
			get { return GetColumnValue<bool>(Columns.IsPromoFreeOfCharge); }
			set { SetColumnValue(Columns.IsPromoFreeOfCharge, value); }
		}
		  
		[XmlAttribute("UsePromoPrice")]
		[Bindable(true)]
		public bool? UsePromoPrice 
		{
			get { return GetColumnValue<bool?>(Columns.UsePromoPrice); }
			set { SetColumnValue(Columns.UsePromoPrice, value); }
		}
		  
		[XmlAttribute("PromoHdrID")]
		[Bindable(true)]
		public int? PromoHdrID 
		{
			get { return GetColumnValue<int?>(Columns.PromoHdrID); }
			set { SetColumnValue(Columns.PromoHdrID, value); }
		}
		  
		[XmlAttribute("PromoDetID")]
		[Bindable(true)]
		public int? PromoDetID 
		{
			get { return GetColumnValue<int?>(Columns.PromoDetID); }
			set { SetColumnValue(Columns.PromoDetID, value); }
		}
		  
		[XmlAttribute("VoucherNo")]
		[Bindable(true)]
		public string VoucherNo 
		{
			get { return GetColumnValue<string>(Columns.VoucherNo); }
			set { SetColumnValue(Columns.VoucherNo, value); }
		}
		  
		[XmlAttribute("IsVoided")]
		[Bindable(true)]
		public bool IsVoided 
		{
			get { return GetColumnValue<bool>(Columns.IsVoided); }
			set { SetColumnValue(Columns.IsVoided, value); }
		}
		  
		[XmlAttribute("IsSpecial")]
		[Bindable(true)]
		public bool IsSpecial 
		{
			get { return GetColumnValue<bool>(Columns.IsSpecial); }
			set { SetColumnValue(Columns.IsSpecial, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
		}
		  
		[XmlAttribute("IsEventPrice")]
		[Bindable(true)]
		public bool? IsEventPrice 
		{
			get { return GetColumnValue<bool?>(Columns.IsEventPrice); }
			set { SetColumnValue(Columns.IsEventPrice, value); }
		}
		  
		[XmlAttribute("SpecialEventID")]
		[Bindable(true)]
		public int? SpecialEventID 
		{
			get { return GetColumnValue<int?>(Columns.SpecialEventID); }
			set { SetColumnValue(Columns.SpecialEventID, value); }
		}
		  
		[XmlAttribute("OrderHdrID")]
		[Bindable(true)]
		public string OrderHdrID 
		{
			get { return GetColumnValue<string>(Columns.OrderHdrID); }
			set { SetColumnValue(Columns.OrderHdrID, value); }
		}
		  
		[XmlAttribute("IsPreOrder")]
		[Bindable(true)]
		public bool? IsPreOrder 
		{
			get { return GetColumnValue<bool?>(Columns.IsPreOrder); }
			set { SetColumnValue(Columns.IsPreOrder, value); }
		}
		  
		[XmlAttribute("IsExchange")]
		[Bindable(true)]
		public bool IsExchange 
		{
			get { return GetColumnValue<bool>(Columns.IsExchange); }
			set { SetColumnValue(Columns.IsExchange, value); }
		}
		  
		[XmlAttribute("IsPendingCollection")]
		[Bindable(true)]
		public bool? IsPendingCollection 
		{
			get { return GetColumnValue<bool?>(Columns.IsPendingCollection); }
			set { SetColumnValue(Columns.IsPendingCollection, value); }
		}
		  
		[XmlAttribute("GiveCommission")]
		[Bindable(true)]
		public bool? GiveCommission 
		{
			get { return GetColumnValue<bool?>(Columns.GiveCommission); }
			set { SetColumnValue(Columns.GiveCommission, value); }
		}
		  
		[XmlAttribute("InventoryHdrRefNo")]
		[Bindable(true)]
		public string InventoryHdrRefNo 
		{
			get { return GetColumnValue<string>(Columns.InventoryHdrRefNo); }
			set { SetColumnValue(Columns.InventoryHdrRefNo, value); }
		}
		  
		[XmlAttribute("ExchangeDetRefNo")]
		[Bindable(true)]
		public string ExchangeDetRefNo 
		{
			get { return GetColumnValue<string>(Columns.ExchangeDetRefNo); }
			set { SetColumnValue(Columns.ExchangeDetRefNo, value); }
		}
		  
		[XmlAttribute("OriginalRetailPrice")]
		[Bindable(true)]
		public decimal OriginalRetailPrice 
		{
			get { return GetColumnValue<decimal>(Columns.OriginalRetailPrice); }
			set { SetColumnValue(Columns.OriginalRetailPrice, value); }
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
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("UniqueID")]
		[Bindable(true)]
		public Guid UniqueID 
		{
			get { return GetColumnValue<Guid>(Columns.UniqueID); }
			set { SetColumnValue(Columns.UniqueID, value); }
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
		  
		[XmlAttribute("GSTAmount")]
		[Bindable(true)]
		public decimal? GSTAmount 
		{
			get { return GetColumnValue<decimal?>(Columns.GSTAmount); }
			set { SetColumnValue(Columns.GSTAmount, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Item ActiveRecord object related to this QuotationDet
		/// 
		/// </summary>
		public PowerPOS.Item Item
		{
			get { return PowerPOS.Item.FetchByID(this.ItemNo); }
			set { SetColumnValue("ItemNo", value.ItemNo); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varOrderDetID,string varItemNo,DateTime varOrderDetDate,decimal varQuantity,decimal varUnitPrice,decimal varDiscount,decimal varAmount,decimal? varGrossSales,bool varIsFreeOfCharge,decimal? varCostOfGoodSold,bool varIsPromo,double varPromoDiscount,decimal varPromoUnitPrice,decimal varPromoAmount,bool varIsPromoFreeOfCharge,bool? varUsePromoPrice,int? varPromoHdrID,int? varPromoDetID,string varVoucherNo,bool varIsVoided,bool varIsSpecial,string varRemark,bool? varIsEventPrice,int? varSpecialEventID,string varOrderHdrID,bool? varIsPreOrder,bool varIsExchange,bool? varIsPendingCollection,bool? varGiveCommission,string varInventoryHdrRefNo,string varExchangeDetRefNo,decimal varOriginalRetailPrice,DateTime? varModifiedOn,string varModifiedBy,string varCreatedBy,DateTime? varCreatedOn,Guid varUniqueID,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5,decimal? varGSTAmount)
		{
			QuotationDet item = new QuotationDet();
			
			item.OrderDetID = varOrderDetID;
			
			item.ItemNo = varItemNo;
			
			item.OrderDetDate = varOrderDetDate;
			
			item.Quantity = varQuantity;
			
			item.UnitPrice = varUnitPrice;
			
			item.Discount = varDiscount;
			
			item.Amount = varAmount;
			
			item.GrossSales = varGrossSales;
			
			item.IsFreeOfCharge = varIsFreeOfCharge;
			
			item.CostOfGoodSold = varCostOfGoodSold;
			
			item.IsPromo = varIsPromo;
			
			item.PromoDiscount = varPromoDiscount;
			
			item.PromoUnitPrice = varPromoUnitPrice;
			
			item.PromoAmount = varPromoAmount;
			
			item.IsPromoFreeOfCharge = varIsPromoFreeOfCharge;
			
			item.UsePromoPrice = varUsePromoPrice;
			
			item.PromoHdrID = varPromoHdrID;
			
			item.PromoDetID = varPromoDetID;
			
			item.VoucherNo = varVoucherNo;
			
			item.IsVoided = varIsVoided;
			
			item.IsSpecial = varIsSpecial;
			
			item.Remark = varRemark;
			
			item.IsEventPrice = varIsEventPrice;
			
			item.SpecialEventID = varSpecialEventID;
			
			item.OrderHdrID = varOrderHdrID;
			
			item.IsPreOrder = varIsPreOrder;
			
			item.IsExchange = varIsExchange;
			
			item.IsPendingCollection = varIsPendingCollection;
			
			item.GiveCommission = varGiveCommission;
			
			item.InventoryHdrRefNo = varInventoryHdrRefNo;
			
			item.ExchangeDetRefNo = varExchangeDetRefNo;
			
			item.OriginalRetailPrice = varOriginalRetailPrice;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.UniqueID = varUniqueID;
			
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
			
			item.GSTAmount = varGSTAmount;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varOrderDetID,string varItemNo,DateTime varOrderDetDate,decimal varQuantity,decimal varUnitPrice,decimal varDiscount,decimal varAmount,decimal? varGrossSales,bool varIsFreeOfCharge,decimal? varCostOfGoodSold,bool varIsPromo,double varPromoDiscount,decimal varPromoUnitPrice,decimal varPromoAmount,bool varIsPromoFreeOfCharge,bool? varUsePromoPrice,int? varPromoHdrID,int? varPromoDetID,string varVoucherNo,bool varIsVoided,bool varIsSpecial,string varRemark,bool? varIsEventPrice,int? varSpecialEventID,string varOrderHdrID,bool? varIsPreOrder,bool varIsExchange,bool? varIsPendingCollection,bool? varGiveCommission,string varInventoryHdrRefNo,string varExchangeDetRefNo,decimal varOriginalRetailPrice,DateTime? varModifiedOn,string varModifiedBy,string varCreatedBy,DateTime? varCreatedOn,Guid varUniqueID,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5,decimal? varGSTAmount)
		{
			QuotationDet item = new QuotationDet();
			
				item.OrderDetID = varOrderDetID;
			
				item.ItemNo = varItemNo;
			
				item.OrderDetDate = varOrderDetDate;
			
				item.Quantity = varQuantity;
			
				item.UnitPrice = varUnitPrice;
			
				item.Discount = varDiscount;
			
				item.Amount = varAmount;
			
				item.GrossSales = varGrossSales;
			
				item.IsFreeOfCharge = varIsFreeOfCharge;
			
				item.CostOfGoodSold = varCostOfGoodSold;
			
				item.IsPromo = varIsPromo;
			
				item.PromoDiscount = varPromoDiscount;
			
				item.PromoUnitPrice = varPromoUnitPrice;
			
				item.PromoAmount = varPromoAmount;
			
				item.IsPromoFreeOfCharge = varIsPromoFreeOfCharge;
			
				item.UsePromoPrice = varUsePromoPrice;
			
				item.PromoHdrID = varPromoHdrID;
			
				item.PromoDetID = varPromoDetID;
			
				item.VoucherNo = varVoucherNo;
			
				item.IsVoided = varIsVoided;
			
				item.IsSpecial = varIsSpecial;
			
				item.Remark = varRemark;
			
				item.IsEventPrice = varIsEventPrice;
			
				item.SpecialEventID = varSpecialEventID;
			
				item.OrderHdrID = varOrderHdrID;
			
				item.IsPreOrder = varIsPreOrder;
			
				item.IsExchange = varIsExchange;
			
				item.IsPendingCollection = varIsPendingCollection;
			
				item.GiveCommission = varGiveCommission;
			
				item.InventoryHdrRefNo = varInventoryHdrRefNo;
			
				item.ExchangeDetRefNo = varExchangeDetRefNo;
			
				item.OriginalRetailPrice = varOriginalRetailPrice;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.UniqueID = varUniqueID;
			
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
			
				item.GSTAmount = varGSTAmount;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn OrderDetIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderDetDateColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn QuantityColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn UnitPriceColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DiscountColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn AmountColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn GrossSalesColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn IsFreeOfChargeColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn CostOfGoodSoldColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn IsPromoColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn PromoDiscountColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn PromoUnitPriceColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn PromoAmountColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn IsPromoFreeOfChargeColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn UsePromoPriceColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn PromoHdrIDColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn PromoDetIDColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn VoucherNoColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn IsVoidedColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn IsSpecialColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn IsEventPriceColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn SpecialEventIDColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderHdrIDColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn IsPreOrderColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn IsExchangeColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn IsPendingCollectionColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn GiveCommissionColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn InventoryHdrRefNoColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn ExchangeDetRefNoColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn OriginalRetailPriceColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld1Column
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld2Column
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld3Column
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld4Column
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld5Column
        {
            get { return Schema.Columns[41]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld6Column
        {
            get { return Schema.Columns[42]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld7Column
        {
            get { return Schema.Columns[43]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld8Column
        {
            get { return Schema.Columns[44]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld9Column
        {
            get { return Schema.Columns[45]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld10Column
        {
            get { return Schema.Columns[46]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag1Column
        {
            get { return Schema.Columns[47]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag2Column
        {
            get { return Schema.Columns[48]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag3Column
        {
            get { return Schema.Columns[49]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag4Column
        {
            get { return Schema.Columns[50]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag5Column
        {
            get { return Schema.Columns[51]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat1Column
        {
            get { return Schema.Columns[52]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat2Column
        {
            get { return Schema.Columns[53]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat3Column
        {
            get { return Schema.Columns[54]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat4Column
        {
            get { return Schema.Columns[55]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat5Column
        {
            get { return Schema.Columns[56]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint1Column
        {
            get { return Schema.Columns[57]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint2Column
        {
            get { return Schema.Columns[58]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint3Column
        {
            get { return Schema.Columns[59]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint4Column
        {
            get { return Schema.Columns[60]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint5Column
        {
            get { return Schema.Columns[61]; }
        }
        
        
        
        public static TableSchema.TableColumn GSTAmountColumn
        {
            get { return Schema.Columns[62]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string OrderDetID = @"OrderDetID";
			 public static string ItemNo = @"ItemNo";
			 public static string OrderDetDate = @"OrderDetDate";
			 public static string Quantity = @"Quantity";
			 public static string UnitPrice = @"UnitPrice";
			 public static string Discount = @"Discount";
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
			 public static string IsVoided = @"IsVoided";
			 public static string IsSpecial = @"IsSpecial";
			 public static string Remark = @"Remark";
			 public static string IsEventPrice = @"IsEventPrice";
			 public static string SpecialEventID = @"SpecialEventID";
			 public static string OrderHdrID = @"OrderHdrID";
			 public static string IsPreOrder = @"IsPreOrder";
			 public static string IsExchange = @"IsExchange";
			 public static string IsPendingCollection = @"IsPendingCollection";
			 public static string GiveCommission = @"giveCommission";
			 public static string InventoryHdrRefNo = @"InventoryHdrRefNo";
			 public static string ExchangeDetRefNo = @"ExchangeDetRefNo";
			 public static string OriginalRetailPrice = @"OriginalRetailPrice";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string UniqueID = @"UniqueID";
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
			 public static string GSTAmount = @"GSTAmount";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
