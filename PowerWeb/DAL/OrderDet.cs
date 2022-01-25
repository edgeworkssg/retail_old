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
	/// Strongly-typed collection for the OrderDet class.
	/// </summary>
    [Serializable]
	public partial class OrderDetCollection : ActiveList<OrderDet, OrderDetCollection>
	{	   
		public OrderDetCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>OrderDetCollection</returns>
		public OrderDetCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                OrderDet o = this[i];
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
	/// This is an ActiveRecord class which wraps the OrderDet table.
	/// </summary>
	[Serializable]
	public partial class OrderDet : ActiveRecord<OrderDet>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public OrderDet()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public OrderDet(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public OrderDet(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public OrderDet(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("OrderDet", TableType.Table, DataService.GetInstance("PowerPOS"));
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
				colvarQuantity.IsNullable = true;
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
				colvarOrderHdrID.IsForeignKey = true;
				colvarOrderHdrID.IsReadOnly = false;
				colvarOrderHdrID.DefaultSetting = @"";
				
					colvarOrderHdrID.ForeignKeyTableName = "OrderHdr";
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
				colvarUserfld2.MaxLength = -1;
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
				
				TableSchema.TableColumn colvarUserfld11 = new TableSchema.TableColumn(schema);
				colvarUserfld11.ColumnName = "Userfld11";
				colvarUserfld11.DataType = DbType.String;
				colvarUserfld11.MaxLength = 500;
				colvarUserfld11.AutoIncrement = false;
				colvarUserfld11.IsNullable = true;
				colvarUserfld11.IsPrimaryKey = false;
				colvarUserfld11.IsForeignKey = false;
				colvarUserfld11.IsReadOnly = false;
				colvarUserfld11.DefaultSetting = @"";
				colvarUserfld11.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld11);
				
				TableSchema.TableColumn colvarUserfld12 = new TableSchema.TableColumn(schema);
				colvarUserfld12.ColumnName = "Userfld12";
				colvarUserfld12.DataType = DbType.String;
				colvarUserfld12.MaxLength = 500;
				colvarUserfld12.AutoIncrement = false;
				colvarUserfld12.IsNullable = true;
				colvarUserfld12.IsPrimaryKey = false;
				colvarUserfld12.IsForeignKey = false;
				colvarUserfld12.IsReadOnly = false;
				colvarUserfld12.DefaultSetting = @"";
				colvarUserfld12.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld12);
				
				TableSchema.TableColumn colvarUserfld13 = new TableSchema.TableColumn(schema);
				colvarUserfld13.ColumnName = "Userfld13";
				colvarUserfld13.DataType = DbType.String;
				colvarUserfld13.MaxLength = 500;
				colvarUserfld13.AutoIncrement = false;
				colvarUserfld13.IsNullable = true;
				colvarUserfld13.IsPrimaryKey = false;
				colvarUserfld13.IsForeignKey = false;
				colvarUserfld13.IsReadOnly = false;
				colvarUserfld13.DefaultSetting = @"";
				colvarUserfld13.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld13);
				
				TableSchema.TableColumn colvarUserfld14 = new TableSchema.TableColumn(schema);
				colvarUserfld14.ColumnName = "Userfld14";
				colvarUserfld14.DataType = DbType.String;
				colvarUserfld14.MaxLength = 500;
				colvarUserfld14.AutoIncrement = false;
				colvarUserfld14.IsNullable = true;
				colvarUserfld14.IsPrimaryKey = false;
				colvarUserfld14.IsForeignKey = false;
				colvarUserfld14.IsReadOnly = false;
				colvarUserfld14.DefaultSetting = @"";
				colvarUserfld14.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld14);
				
				TableSchema.TableColumn colvarUserfld15 = new TableSchema.TableColumn(schema);
				colvarUserfld15.ColumnName = "Userfld15";
				colvarUserfld15.DataType = DbType.String;
				colvarUserfld15.MaxLength = 500;
				colvarUserfld15.AutoIncrement = false;
				colvarUserfld15.IsNullable = true;
				colvarUserfld15.IsPrimaryKey = false;
				colvarUserfld15.IsForeignKey = false;
				colvarUserfld15.IsReadOnly = false;
				colvarUserfld15.DefaultSetting = @"";
				colvarUserfld15.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld15);
				
				TableSchema.TableColumn colvarUserfld16 = new TableSchema.TableColumn(schema);
				colvarUserfld16.ColumnName = "Userfld16";
				colvarUserfld16.DataType = DbType.String;
				colvarUserfld16.MaxLength = 500;
				colvarUserfld16.AutoIncrement = false;
				colvarUserfld16.IsNullable = true;
				colvarUserfld16.IsPrimaryKey = false;
				colvarUserfld16.IsForeignKey = false;
				colvarUserfld16.IsReadOnly = false;
				colvarUserfld16.DefaultSetting = @"";
				colvarUserfld16.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld16);
				
				TableSchema.TableColumn colvarUserfld17 = new TableSchema.TableColumn(schema);
				colvarUserfld17.ColumnName = "Userfld17";
				colvarUserfld17.DataType = DbType.String;
				colvarUserfld17.MaxLength = 500;
				colvarUserfld17.AutoIncrement = false;
				colvarUserfld17.IsNullable = true;
				colvarUserfld17.IsPrimaryKey = false;
				colvarUserfld17.IsForeignKey = false;
				colvarUserfld17.IsReadOnly = false;
				colvarUserfld17.DefaultSetting = @"";
				colvarUserfld17.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld17);
				
				TableSchema.TableColumn colvarUserfld18 = new TableSchema.TableColumn(schema);
				colvarUserfld18.ColumnName = "Userfld18";
				colvarUserfld18.DataType = DbType.String;
				colvarUserfld18.MaxLength = 500;
				colvarUserfld18.AutoIncrement = false;
				colvarUserfld18.IsNullable = true;
				colvarUserfld18.IsPrimaryKey = false;
				colvarUserfld18.IsForeignKey = false;
				colvarUserfld18.IsReadOnly = false;
				colvarUserfld18.DefaultSetting = @"";
				colvarUserfld18.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld18);
				
				TableSchema.TableColumn colvarUserfld19 = new TableSchema.TableColumn(schema);
				colvarUserfld19.ColumnName = "Userfld19";
				colvarUserfld19.DataType = DbType.String;
				colvarUserfld19.MaxLength = 500;
				colvarUserfld19.AutoIncrement = false;
				colvarUserfld19.IsNullable = true;
				colvarUserfld19.IsPrimaryKey = false;
				colvarUserfld19.IsForeignKey = false;
				colvarUserfld19.IsReadOnly = false;
				colvarUserfld19.DefaultSetting = @"";
				colvarUserfld19.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld19);
				
				TableSchema.TableColumn colvarUserfld20 = new TableSchema.TableColumn(schema);
				colvarUserfld20.ColumnName = "Userfld20";
				colvarUserfld20.DataType = DbType.String;
				colvarUserfld20.MaxLength = 500;
				colvarUserfld20.AutoIncrement = false;
				colvarUserfld20.IsNullable = true;
				colvarUserfld20.IsPrimaryKey = false;
				colvarUserfld20.IsForeignKey = false;
				colvarUserfld20.IsReadOnly = false;
				colvarUserfld20.DefaultSetting = @"";
				colvarUserfld20.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld20);
				
				TableSchema.TableColumn colvarUserfloat6 = new TableSchema.TableColumn(schema);
				colvarUserfloat6.ColumnName = "Userfloat6";
				colvarUserfloat6.DataType = DbType.Currency;
				colvarUserfloat6.MaxLength = 0;
				colvarUserfloat6.AutoIncrement = false;
				colvarUserfloat6.IsNullable = true;
				colvarUserfloat6.IsPrimaryKey = false;
				colvarUserfloat6.IsForeignKey = false;
				colvarUserfloat6.IsReadOnly = false;
				colvarUserfloat6.DefaultSetting = @"";
				colvarUserfloat6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat6);
				
				TableSchema.TableColumn colvarUserfloat7 = new TableSchema.TableColumn(schema);
				colvarUserfloat7.ColumnName = "Userfloat7";
				colvarUserfloat7.DataType = DbType.Currency;
				colvarUserfloat7.MaxLength = 0;
				colvarUserfloat7.AutoIncrement = false;
				colvarUserfloat7.IsNullable = true;
				colvarUserfloat7.IsPrimaryKey = false;
				colvarUserfloat7.IsForeignKey = false;
				colvarUserfloat7.IsReadOnly = false;
				colvarUserfloat7.DefaultSetting = @"";
				colvarUserfloat7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat7);
				
				TableSchema.TableColumn colvarUserfloat8 = new TableSchema.TableColumn(schema);
				colvarUserfloat8.ColumnName = "Userfloat8";
				colvarUserfloat8.DataType = DbType.Currency;
				colvarUserfloat8.MaxLength = 0;
				colvarUserfloat8.AutoIncrement = false;
				colvarUserfloat8.IsNullable = true;
				colvarUserfloat8.IsPrimaryKey = false;
				colvarUserfloat8.IsForeignKey = false;
				colvarUserfloat8.IsReadOnly = false;
				colvarUserfloat8.DefaultSetting = @"";
				colvarUserfloat8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat8);
				
				TableSchema.TableColumn colvarUserfloat9 = new TableSchema.TableColumn(schema);
				colvarUserfloat9.ColumnName = "Userfloat9";
				colvarUserfloat9.DataType = DbType.Currency;
				colvarUserfloat9.MaxLength = 0;
				colvarUserfloat9.AutoIncrement = false;
				colvarUserfloat9.IsNullable = true;
				colvarUserfloat9.IsPrimaryKey = false;
				colvarUserfloat9.IsForeignKey = false;
				colvarUserfloat9.IsReadOnly = false;
				colvarUserfloat9.DefaultSetting = @"";
				colvarUserfloat9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat9);
				
				TableSchema.TableColumn colvarUserfloat10 = new TableSchema.TableColumn(schema);
				colvarUserfloat10.ColumnName = "Userfloat10";
				colvarUserfloat10.DataType = DbType.Currency;
				colvarUserfloat10.MaxLength = 0;
				colvarUserfloat10.AutoIncrement = false;
				colvarUserfloat10.IsNullable = true;
				colvarUserfloat10.IsPrimaryKey = false;
				colvarUserfloat10.IsForeignKey = false;
				colvarUserfloat10.IsReadOnly = false;
				colvarUserfloat10.DefaultSetting = @"";
				colvarUserfloat10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat10);
				
				TableSchema.TableColumn colvarUserflag6 = new TableSchema.TableColumn(schema);
				colvarUserflag6.ColumnName = "Userflag6";
				colvarUserflag6.DataType = DbType.Boolean;
				colvarUserflag6.MaxLength = 0;
				colvarUserflag6.AutoIncrement = false;
				colvarUserflag6.IsNullable = true;
				colvarUserflag6.IsPrimaryKey = false;
				colvarUserflag6.IsForeignKey = false;
				colvarUserflag6.IsReadOnly = false;
				colvarUserflag6.DefaultSetting = @"";
				colvarUserflag6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag6);
				
				TableSchema.TableColumn colvarUserflag7 = new TableSchema.TableColumn(schema);
				colvarUserflag7.ColumnName = "Userflag7";
				colvarUserflag7.DataType = DbType.Boolean;
				colvarUserflag7.MaxLength = 0;
				colvarUserflag7.AutoIncrement = false;
				colvarUserflag7.IsNullable = true;
				colvarUserflag7.IsPrimaryKey = false;
				colvarUserflag7.IsForeignKey = false;
				colvarUserflag7.IsReadOnly = false;
				colvarUserflag7.DefaultSetting = @"";
				colvarUserflag7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag7);
				
				TableSchema.TableColumn colvarUserflag8 = new TableSchema.TableColumn(schema);
				colvarUserflag8.ColumnName = "Userflag8";
				colvarUserflag8.DataType = DbType.Boolean;
				colvarUserflag8.MaxLength = 0;
				colvarUserflag8.AutoIncrement = false;
				colvarUserflag8.IsNullable = true;
				colvarUserflag8.IsPrimaryKey = false;
				colvarUserflag8.IsForeignKey = false;
				colvarUserflag8.IsReadOnly = false;
				colvarUserflag8.DefaultSetting = @"";
				colvarUserflag8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag8);
				
				TableSchema.TableColumn colvarUserflag9 = new TableSchema.TableColumn(schema);
				colvarUserflag9.ColumnName = "Userflag9";
				colvarUserflag9.DataType = DbType.Boolean;
				colvarUserflag9.MaxLength = 0;
				colvarUserflag9.AutoIncrement = false;
				colvarUserflag9.IsNullable = true;
				colvarUserflag9.IsPrimaryKey = false;
				colvarUserflag9.IsForeignKey = false;
				colvarUserflag9.IsReadOnly = false;
				colvarUserflag9.DefaultSetting = @"";
				colvarUserflag9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag9);
				
				TableSchema.TableColumn colvarUserflag10 = new TableSchema.TableColumn(schema);
				colvarUserflag10.ColumnName = "Userflag10";
				colvarUserflag10.DataType = DbType.Boolean;
				colvarUserflag10.MaxLength = 0;
				colvarUserflag10.AutoIncrement = false;
				colvarUserflag10.IsNullable = true;
				colvarUserflag10.IsPrimaryKey = false;
				colvarUserflag10.IsForeignKey = false;
				colvarUserflag10.IsReadOnly = false;
				colvarUserflag10.DefaultSetting = @"";
				colvarUserflag10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag10);
				
				TableSchema.TableColumn colvarUserint6 = new TableSchema.TableColumn(schema);
				colvarUserint6.ColumnName = "Userint6";
				colvarUserint6.DataType = DbType.Int32;
				colvarUserint6.MaxLength = 0;
				colvarUserint6.AutoIncrement = false;
				colvarUserint6.IsNullable = true;
				colvarUserint6.IsPrimaryKey = false;
				colvarUserint6.IsForeignKey = false;
				colvarUserint6.IsReadOnly = false;
				colvarUserint6.DefaultSetting = @"";
				colvarUserint6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint6);
				
				TableSchema.TableColumn colvarUserint7 = new TableSchema.TableColumn(schema);
				colvarUserint7.ColumnName = "Userint7";
				colvarUserint7.DataType = DbType.Int32;
				colvarUserint7.MaxLength = 0;
				colvarUserint7.AutoIncrement = false;
				colvarUserint7.IsNullable = true;
				colvarUserint7.IsPrimaryKey = false;
				colvarUserint7.IsForeignKey = false;
				colvarUserint7.IsReadOnly = false;
				colvarUserint7.DefaultSetting = @"";
				colvarUserint7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint7);
				
				TableSchema.TableColumn colvarUserint8 = new TableSchema.TableColumn(schema);
				colvarUserint8.ColumnName = "Userint8";
				colvarUserint8.DataType = DbType.Int32;
				colvarUserint8.MaxLength = 0;
				colvarUserint8.AutoIncrement = false;
				colvarUserint8.IsNullable = true;
				colvarUserint8.IsPrimaryKey = false;
				colvarUserint8.IsForeignKey = false;
				colvarUserint8.IsReadOnly = false;
				colvarUserint8.DefaultSetting = @"";
				colvarUserint8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint8);
				
				TableSchema.TableColumn colvarUserint9 = new TableSchema.TableColumn(schema);
				colvarUserint9.ColumnName = "Userint9";
				colvarUserint9.DataType = DbType.Int32;
				colvarUserint9.MaxLength = 0;
				colvarUserint9.AutoIncrement = false;
				colvarUserint9.IsNullable = true;
				colvarUserint9.IsPrimaryKey = false;
				colvarUserint9.IsForeignKey = false;
				colvarUserint9.IsReadOnly = false;
				colvarUserint9.DefaultSetting = @"";
				colvarUserint9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint9);
				
				TableSchema.TableColumn colvarUserint10 = new TableSchema.TableColumn(schema);
				colvarUserint10.ColumnName = "Userint10";
				colvarUserint10.DataType = DbType.Int32;
				colvarUserint10.MaxLength = 0;
				colvarUserint10.AutoIncrement = false;
				colvarUserint10.IsNullable = true;
				colvarUserint10.IsPrimaryKey = false;
				colvarUserint10.IsForeignKey = false;
				colvarUserint10.IsReadOnly = false;
				colvarUserint10.DefaultSetting = @"";
				colvarUserint10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint10);
				
				TableSchema.TableColumn colvarDataIndex = new TableSchema.TableColumn(schema);
				colvarDataIndex.ColumnName = "DataIndex";
				colvarDataIndex.DataType = DbType.Int64;
				colvarDataIndex.MaxLength = 0;
				colvarDataIndex.AutoIncrement = false;
				colvarDataIndex.IsNullable = true;
				colvarDataIndex.IsPrimaryKey = false;
				colvarDataIndex.IsForeignKey = false;
				colvarDataIndex.IsReadOnly = false;
				colvarDataIndex.DefaultSetting = @"";
				colvarDataIndex.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDataIndex);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("OrderDet",schema);
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
		public decimal? Quantity 
		{
			get { return GetColumnValue<decimal?>(Columns.Quantity); }
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
		  
		[XmlAttribute("Userfld11")]
		[Bindable(true)]
		public string Userfld11 
		{
			get { return GetColumnValue<string>(Columns.Userfld11); }
			set { SetColumnValue(Columns.Userfld11, value); }
		}
		  
		[XmlAttribute("Userfld12")]
		[Bindable(true)]
		public string Userfld12 
		{
			get { return GetColumnValue<string>(Columns.Userfld12); }
			set { SetColumnValue(Columns.Userfld12, value); }
		}
		  
		[XmlAttribute("Userfld13")]
		[Bindable(true)]
		public string Userfld13 
		{
			get { return GetColumnValue<string>(Columns.Userfld13); }
			set { SetColumnValue(Columns.Userfld13, value); }
		}
		  
		[XmlAttribute("Userfld14")]
		[Bindable(true)]
		public string Userfld14 
		{
			get { return GetColumnValue<string>(Columns.Userfld14); }
			set { SetColumnValue(Columns.Userfld14, value); }
		}
		  
		[XmlAttribute("Userfld15")]
		[Bindable(true)]
		public string Userfld15 
		{
			get { return GetColumnValue<string>(Columns.Userfld15); }
			set { SetColumnValue(Columns.Userfld15, value); }
		}
		  
		[XmlAttribute("Userfld16")]
		[Bindable(true)]
		public string Userfld16 
		{
			get { return GetColumnValue<string>(Columns.Userfld16); }
			set { SetColumnValue(Columns.Userfld16, value); }
		}
		  
		[XmlAttribute("Userfld17")]
		[Bindable(true)]
		public string Userfld17 
		{
			get { return GetColumnValue<string>(Columns.Userfld17); }
			set { SetColumnValue(Columns.Userfld17, value); }
		}
		  
		[XmlAttribute("Userfld18")]
		[Bindable(true)]
		public string Userfld18 
		{
			get { return GetColumnValue<string>(Columns.Userfld18); }
			set { SetColumnValue(Columns.Userfld18, value); }
		}
		  
		[XmlAttribute("Userfld19")]
		[Bindable(true)]
		public string Userfld19 
		{
			get { return GetColumnValue<string>(Columns.Userfld19); }
			set { SetColumnValue(Columns.Userfld19, value); }
		}
		  
		[XmlAttribute("Userfld20")]
		[Bindable(true)]
		public string Userfld20 
		{
			get { return GetColumnValue<string>(Columns.Userfld20); }
			set { SetColumnValue(Columns.Userfld20, value); }
		}
		  
		[XmlAttribute("Userfloat6")]
		[Bindable(true)]
		public decimal? Userfloat6 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat6); }
			set { SetColumnValue(Columns.Userfloat6, value); }
		}
		  
		[XmlAttribute("Userfloat7")]
		[Bindable(true)]
		public decimal? Userfloat7 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat7); }
			set { SetColumnValue(Columns.Userfloat7, value); }
		}
		  
		[XmlAttribute("Userfloat8")]
		[Bindable(true)]
		public decimal? Userfloat8 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat8); }
			set { SetColumnValue(Columns.Userfloat8, value); }
		}
		  
		[XmlAttribute("Userfloat9")]
		[Bindable(true)]
		public decimal? Userfloat9 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat9); }
			set { SetColumnValue(Columns.Userfloat9, value); }
		}
		  
		[XmlAttribute("Userfloat10")]
		[Bindable(true)]
		public decimal? Userfloat10 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat10); }
			set { SetColumnValue(Columns.Userfloat10, value); }
		}
		  
		[XmlAttribute("Userflag6")]
		[Bindable(true)]
		public bool? Userflag6 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag6); }
			set { SetColumnValue(Columns.Userflag6, value); }
		}
		  
		[XmlAttribute("Userflag7")]
		[Bindable(true)]
		public bool? Userflag7 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag7); }
			set { SetColumnValue(Columns.Userflag7, value); }
		}
		  
		[XmlAttribute("Userflag8")]
		[Bindable(true)]
		public bool? Userflag8 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag8); }
			set { SetColumnValue(Columns.Userflag8, value); }
		}
		  
		[XmlAttribute("Userflag9")]
		[Bindable(true)]
		public bool? Userflag9 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag9); }
			set { SetColumnValue(Columns.Userflag9, value); }
		}
		  
		[XmlAttribute("Userflag10")]
		[Bindable(true)]
		public bool? Userflag10 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag10); }
			set { SetColumnValue(Columns.Userflag10, value); }
		}
		  
		[XmlAttribute("Userint6")]
		[Bindable(true)]
		public int? Userint6 
		{
			get { return GetColumnValue<int?>(Columns.Userint6); }
			set { SetColumnValue(Columns.Userint6, value); }
		}
		  
		[XmlAttribute("Userint7")]
		[Bindable(true)]
		public int? Userint7 
		{
			get { return GetColumnValue<int?>(Columns.Userint7); }
			set { SetColumnValue(Columns.Userint7, value); }
		}
		  
		[XmlAttribute("Userint8")]
		[Bindable(true)]
		public int? Userint8 
		{
			get { return GetColumnValue<int?>(Columns.Userint8); }
			set { SetColumnValue(Columns.Userint8, value); }
		}
		  
		[XmlAttribute("Userint9")]
		[Bindable(true)]
		public int? Userint9 
		{
			get { return GetColumnValue<int?>(Columns.Userint9); }
			set { SetColumnValue(Columns.Userint9, value); }
		}
		  
		[XmlAttribute("Userint10")]
		[Bindable(true)]
		public int? Userint10 
		{
			get { return GetColumnValue<int?>(Columns.Userint10); }
			set { SetColumnValue(Columns.Userint10, value); }
		}
		  
		[XmlAttribute("DataIndex")]
		[Bindable(true)]
		public long? DataIndex 
		{
			get { return GetColumnValue<long?>(Columns.DataIndex); }
			set { SetColumnValue(Columns.DataIndex, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		public PowerPOS.OrderDetTransferCollection OrderDetTransferRecords()
		{
			return new PowerPOS.OrderDetTransferCollection().Where(OrderDetTransfer.Columns.OrderDetID, OrderDetID).Load();
		}
		public PowerPOS.OrderDetUOMConversionCollection OrderDetUOMConversionRecords()
		{
			return new PowerPOS.OrderDetUOMConversionCollection().Where(OrderDetUOMConversion.Columns.OrderDetID, OrderDetID).Load();
		}
		public PowerPOS.PackageDetCollection PackageDetRecords()
		{
			return new PowerPOS.PackageDetCollection().Where(PackageDet.Columns.OrderDetID, OrderDetID).Load();
		}
		public PowerPOS.WarrantyCollection WarrantyRecords()
		{
			return new PowerPOS.WarrantyCollection().Where(Warranty.Columns.OrderDetId, OrderDetID).Load();
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Item ActiveRecord object related to this OrderDet
		/// 
		/// </summary>
		public PowerPOS.Item Item
		{
			get { return PowerPOS.Item.FetchByID(this.ItemNo); }
			set { SetColumnValue("ItemNo", value.ItemNo); }
		}
		
		
		/// <summary>
		/// Returns a OrderHdr ActiveRecord object related to this OrderDet
		/// 
		/// </summary>
		public PowerPOS.OrderHdr OrderHdr
		{
			get { return PowerPOS.OrderHdr.FetchByID(this.OrderHdrID); }
			set { SetColumnValue("OrderHdrID", value.OrderHdrID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varOrderDetID,string varItemNo,DateTime varOrderDetDate,decimal? varQuantity,decimal varUnitPrice,decimal varDiscount,decimal varAmount,decimal? varGrossSales,bool varIsFreeOfCharge,decimal? varCostOfGoodSold,bool varIsPromo,double varPromoDiscount,decimal varPromoUnitPrice,decimal varPromoAmount,bool varIsPromoFreeOfCharge,bool? varUsePromoPrice,int? varPromoHdrID,int? varPromoDetID,string varVoucherNo,bool varIsVoided,bool varIsSpecial,string varRemark,bool? varIsEventPrice,int? varSpecialEventID,string varOrderHdrID,bool? varIsPreOrder,bool varIsExchange,bool? varIsPendingCollection,bool? varGiveCommission,string varInventoryHdrRefNo,string varExchangeDetRefNo,decimal varOriginalRetailPrice,DateTime? varModifiedOn,string varModifiedBy,string varCreatedBy,DateTime? varCreatedOn,Guid varUniqueID,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5,decimal? varGSTAmount,string varUserfld11,string varUserfld12,string varUserfld13,string varUserfld14,string varUserfld15,string varUserfld16,string varUserfld17,string varUserfld18,string varUserfld19,string varUserfld20,decimal? varUserfloat6,decimal? varUserfloat7,decimal? varUserfloat8,decimal? varUserfloat9,decimal? varUserfloat10,bool? varUserflag6,bool? varUserflag7,bool? varUserflag8,bool? varUserflag9,bool? varUserflag10,int? varUserint6,int? varUserint7,int? varUserint8,int? varUserint9,int? varUserint10,long? varDataIndex)
		{
			OrderDet item = new OrderDet();
			
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
			
			item.Userfld11 = varUserfld11;
			
			item.Userfld12 = varUserfld12;
			
			item.Userfld13 = varUserfld13;
			
			item.Userfld14 = varUserfld14;
			
			item.Userfld15 = varUserfld15;
			
			item.Userfld16 = varUserfld16;
			
			item.Userfld17 = varUserfld17;
			
			item.Userfld18 = varUserfld18;
			
			item.Userfld19 = varUserfld19;
			
			item.Userfld20 = varUserfld20;
			
			item.Userfloat6 = varUserfloat6;
			
			item.Userfloat7 = varUserfloat7;
			
			item.Userfloat8 = varUserfloat8;
			
			item.Userfloat9 = varUserfloat9;
			
			item.Userfloat10 = varUserfloat10;
			
			item.Userflag6 = varUserflag6;
			
			item.Userflag7 = varUserflag7;
			
			item.Userflag8 = varUserflag8;
			
			item.Userflag9 = varUserflag9;
			
			item.Userflag10 = varUserflag10;
			
			item.Userint6 = varUserint6;
			
			item.Userint7 = varUserint7;
			
			item.Userint8 = varUserint8;
			
			item.Userint9 = varUserint9;
			
			item.Userint10 = varUserint10;
			
			item.DataIndex = varDataIndex;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varOrderDetID,string varItemNo,DateTime varOrderDetDate,decimal? varQuantity,decimal varUnitPrice,decimal varDiscount,decimal varAmount,decimal? varGrossSales,bool varIsFreeOfCharge,decimal? varCostOfGoodSold,bool varIsPromo,double varPromoDiscount,decimal varPromoUnitPrice,decimal varPromoAmount,bool varIsPromoFreeOfCharge,bool? varUsePromoPrice,int? varPromoHdrID,int? varPromoDetID,string varVoucherNo,bool varIsVoided,bool varIsSpecial,string varRemark,bool? varIsEventPrice,int? varSpecialEventID,string varOrderHdrID,bool? varIsPreOrder,bool varIsExchange,bool? varIsPendingCollection,bool? varGiveCommission,string varInventoryHdrRefNo,string varExchangeDetRefNo,decimal varOriginalRetailPrice,DateTime? varModifiedOn,string varModifiedBy,string varCreatedBy,DateTime? varCreatedOn,Guid varUniqueID,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5,decimal? varGSTAmount,string varUserfld11,string varUserfld12,string varUserfld13,string varUserfld14,string varUserfld15,string varUserfld16,string varUserfld17,string varUserfld18,string varUserfld19,string varUserfld20,decimal? varUserfloat6,decimal? varUserfloat7,decimal? varUserfloat8,decimal? varUserfloat9,decimal? varUserfloat10,bool? varUserflag6,bool? varUserflag7,bool? varUserflag8,bool? varUserflag9,bool? varUserflag10,int? varUserint6,int? varUserint7,int? varUserint8,int? varUserint9,int? varUserint10,long? varDataIndex)
		{
			OrderDet item = new OrderDet();
			
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
			
				item.Userfld11 = varUserfld11;
			
				item.Userfld12 = varUserfld12;
			
				item.Userfld13 = varUserfld13;
			
				item.Userfld14 = varUserfld14;
			
				item.Userfld15 = varUserfld15;
			
				item.Userfld16 = varUserfld16;
			
				item.Userfld17 = varUserfld17;
			
				item.Userfld18 = varUserfld18;
			
				item.Userfld19 = varUserfld19;
			
				item.Userfld20 = varUserfld20;
			
				item.Userfloat6 = varUserfloat6;
			
				item.Userfloat7 = varUserfloat7;
			
				item.Userfloat8 = varUserfloat8;
			
				item.Userfloat9 = varUserfloat9;
			
				item.Userfloat10 = varUserfloat10;
			
				item.Userflag6 = varUserflag6;
			
				item.Userflag7 = varUserflag7;
			
				item.Userflag8 = varUserflag8;
			
				item.Userflag9 = varUserflag9;
			
				item.Userflag10 = varUserflag10;
			
				item.Userint6 = varUserint6;
			
				item.Userint7 = varUserint7;
			
				item.Userint8 = varUserint8;
			
				item.Userint9 = varUserint9;
			
				item.Userint10 = varUserint10;
			
				item.DataIndex = varDataIndex;
			
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
        
        
        
        public static TableSchema.TableColumn Userfld11Column
        {
            get { return Schema.Columns[63]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld12Column
        {
            get { return Schema.Columns[64]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld13Column
        {
            get { return Schema.Columns[65]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld14Column
        {
            get { return Schema.Columns[66]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld15Column
        {
            get { return Schema.Columns[67]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld16Column
        {
            get { return Schema.Columns[68]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld17Column
        {
            get { return Schema.Columns[69]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld18Column
        {
            get { return Schema.Columns[70]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld19Column
        {
            get { return Schema.Columns[71]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld20Column
        {
            get { return Schema.Columns[72]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat6Column
        {
            get { return Schema.Columns[73]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat7Column
        {
            get { return Schema.Columns[74]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat8Column
        {
            get { return Schema.Columns[75]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat9Column
        {
            get { return Schema.Columns[76]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat10Column
        {
            get { return Schema.Columns[77]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag6Column
        {
            get { return Schema.Columns[78]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag7Column
        {
            get { return Schema.Columns[79]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag8Column
        {
            get { return Schema.Columns[80]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag9Column
        {
            get { return Schema.Columns[81]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag10Column
        {
            get { return Schema.Columns[82]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint6Column
        {
            get { return Schema.Columns[83]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint7Column
        {
            get { return Schema.Columns[84]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint8Column
        {
            get { return Schema.Columns[85]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint9Column
        {
            get { return Schema.Columns[86]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint10Column
        {
            get { return Schema.Columns[87]; }
        }
        
        
        
        public static TableSchema.TableColumn DataIndexColumn
        {
            get { return Schema.Columns[88]; }
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
			 public static string Userfld11 = @"Userfld11";
			 public static string Userfld12 = @"Userfld12";
			 public static string Userfld13 = @"Userfld13";
			 public static string Userfld14 = @"Userfld14";
			 public static string Userfld15 = @"Userfld15";
			 public static string Userfld16 = @"Userfld16";
			 public static string Userfld17 = @"Userfld17";
			 public static string Userfld18 = @"Userfld18";
			 public static string Userfld19 = @"Userfld19";
			 public static string Userfld20 = @"Userfld20";
			 public static string Userfloat6 = @"Userfloat6";
			 public static string Userfloat7 = @"Userfloat7";
			 public static string Userfloat8 = @"Userfloat8";
			 public static string Userfloat9 = @"Userfloat9";
			 public static string Userfloat10 = @"Userfloat10";
			 public static string Userflag6 = @"Userflag6";
			 public static string Userflag7 = @"Userflag7";
			 public static string Userflag8 = @"Userflag8";
			 public static string Userflag9 = @"Userflag9";
			 public static string Userflag10 = @"Userflag10";
			 public static string Userint6 = @"Userint6";
			 public static string Userint7 = @"Userint7";
			 public static string Userint8 = @"Userint8";
			 public static string Userint9 = @"Userint9";
			 public static string Userint10 = @"Userint10";
			 public static string DataIndex = @"DataIndex";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
}
        #endregion
	}
}
