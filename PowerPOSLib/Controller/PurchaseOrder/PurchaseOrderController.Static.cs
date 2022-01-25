using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using System.Collections;
using PowerPOS.Container;
using System.Linq;
using SubSonic;
using System.ComponentModel;

namespace PowerPOS
{
    public class PackingSize
    {
        public string PackingSizeName { set; get; }
        public decimal PackingSizeUOM { set; get; }
        public decimal PackingSizeCostPrice { set; get; }
        public string BaseUOM { set; get; }
    }
    public partial class PurchaseOrderController
    {
        public static List<PackingSize> FetchPackingSizeByItemNoAndSupplier(string itemNo, int supplierID)
        {
            bool showPackingSize = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false);
            List<PackingSize> packingSize = new List<PackingSize>();
            Item theItem = new Item(itemNo);

            if (!theItem.IsNew && !string.IsNullOrEmpty(theItem.UOM))
            {
                packingSize.Add(new PackingSize
                {
                    PackingSizeName = theItem.UOM,
                    PackingSizeUOM = 1,
                    PackingSizeCostPrice = theItem.FactoryPrice,
                    BaseUOM = theItem.UOM
                });
            }

            if (showPackingSize)
            {
                Query qr = new Query("ItemSupplierMap");
                qr.AddWhere(ItemSupplierMap.Columns.ItemNo, itemNo);
                qr.AddWhere(ItemSupplierMap.Columns.SupplierID, supplierID);
                ItemSupplierMap ism = new ItemSupplierMapController().FetchByQuery(qr).FirstOrDefault();
                if (ism != null)
                {
                    int packingSizeNo = (AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.MaxPackingSizeOnItemSupplierMap) + "").GetIntValue();
                    if (packingSizeNo >= 1 && !string.IsNullOrEmpty(ism.PackingSize1) && ism.PackingSizeUOM1.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize1,
                            PackingSizeUOM = ism.PackingSizeUOM1.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice1.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 2
                        && !string.IsNullOrEmpty(ism.PackingSize2)
                        && ism.PackingSizeUOM2.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize2,
                            PackingSizeUOM = ism.PackingSizeUOM2.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice2.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 3
                        && !string.IsNullOrEmpty(ism.PackingSize3)
                        && ism.PackingSizeUOM3.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize3,
                            PackingSizeUOM = ism.PackingSizeUOM3.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice3.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 4
                        && !string.IsNullOrEmpty(ism.PackingSize4)
                        && ism.PackingSizeUOM4.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize4,
                            PackingSizeUOM = ism.PackingSizeUOM4.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice4.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 5
                        && !string.IsNullOrEmpty(ism.PackingSize5)
                        && ism.PackingSizeUOM5.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize5,
                            PackingSizeUOM = ism.PackingSizeUOM5.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice5.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 6
                        && !string.IsNullOrEmpty(ism.PackingSize6)
                        && ism.PackingSizeUOM6.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize6,
                            PackingSizeUOM = ism.PackingSizeUOM6.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice6.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 7
                        && !string.IsNullOrEmpty(ism.PackingSize7)
                        && ism.PackingSizeUOM7.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize7,
                            PackingSizeUOM = ism.PackingSizeUOM7.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice7.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 8
                        && !string.IsNullOrEmpty(ism.PackingSize8)
                        && ism.PackingSizeUOM8.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize8,
                            PackingSizeUOM = ism.PackingSizeUOM8.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice8.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 9
                        && !string.IsNullOrEmpty(ism.PackingSize9)
                        && ism.PackingSizeUOM9.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize9,
                            PackingSizeUOM = ism.PackingSizeUOM9.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice9.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 10
                        && !string.IsNullOrEmpty(ism.PackingSize10)
                        && ism.PackingSizeUOM10.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize10,
                            PackingSizeUOM = ism.PackingSizeUOM10.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice10.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                }
            }

            return packingSize;
        }

        public static List<PackingSize> FetchPackingSizeByItemNoAndSupplierNew(string itemNo, int supplierID)
        {
            bool showPackingSize = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false);
            List<PackingSize> packingSize = new List<PackingSize>();
            Item theItem = new Item(itemNo);

            if (!theItem.IsNew && !string.IsNullOrEmpty(theItem.UOM))
            {
                packingSize.Add(new PackingSize
                {
                    PackingSizeName = theItem.UOM,
                    PackingSizeUOM = 1,
                    PackingSizeCostPrice = theItem.FactoryPrice,
                    BaseUOM = theItem.UOM
                });
            }

            if (showPackingSize)
            {
                Query qr = new Query("ItemSupplierMap");
                qr.AddWhere(ItemSupplierMap.Columns.ItemNo, itemNo);
                qr.AddWhere(ItemSupplierMap.Columns.SupplierID, supplierID);
                ItemSupplierMap ism = new ItemSupplierMapController().FetchByQuery(qr).FirstOrDefault();
                if (ism != null)
                {
                    int packingSizeNo = (AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.MaxPackingSizeOnItemSupplierMap) + "").GetIntValue();
                    if (packingSizeNo >= 1 && !string.IsNullOrEmpty(ism.PackingSize1) && ism.PackingSizeUOM1.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize1,
                            PackingSizeUOM = ism.PackingSizeUOM1.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice1.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 2
                        && !string.IsNullOrEmpty(ism.PackingSize2)
                        && ism.PackingSizeUOM2.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize2,
                            PackingSizeUOM = ism.PackingSizeUOM2.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice2.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 3
                        && !string.IsNullOrEmpty(ism.PackingSize3)
                        && ism.PackingSizeUOM3.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize3,
                            PackingSizeUOM = ism.PackingSizeUOM3.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice3.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 4
                        && !string.IsNullOrEmpty(ism.PackingSize4)
                        && ism.PackingSizeUOM4.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize4,
                            PackingSizeUOM = ism.PackingSizeUOM4.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice4.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 5
                        && !string.IsNullOrEmpty(ism.PackingSize5)
                        && ism.PackingSizeUOM5.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize5,
                            PackingSizeUOM = ism.PackingSizeUOM5.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice5.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 6
                        && !string.IsNullOrEmpty(ism.PackingSize6)
                        && ism.PackingSizeUOM6.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize6,
                            PackingSizeUOM = ism.PackingSizeUOM6.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice6.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 7
                        && !string.IsNullOrEmpty(ism.PackingSize7)
                        && ism.PackingSizeUOM7.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize7,
                            PackingSizeUOM = ism.PackingSizeUOM7.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice7.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 8
                        && !string.IsNullOrEmpty(ism.PackingSize8)
                        && ism.PackingSizeUOM8.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize8,
                            PackingSizeUOM = ism.PackingSizeUOM8.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice8.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 9
                        && !string.IsNullOrEmpty(ism.PackingSize9)
                        && ism.PackingSizeUOM9.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize9,
                            PackingSizeUOM = ism.PackingSizeUOM9.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice9.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    //if (packingSizeNo >= 10
                    //    && !string.IsNullOrEmpty(ism.PackingSize10)
                    //    && ism.PackingSizeUOM10.GetValueOrDefault(0) > 0)
                    //{
                    //    packingSize.Add(new PackingSize
                    //    {
                    //        PackingSizeName = ism.PackingSize10,
                    //        PackingSizeUOM = ism.PackingSizeUOM10.GetValueOrDefault(0),
                    //        PackingSizeCostPrice = ism.CostPrice10.GetValueOrDefault(0),
                    //        BaseUOM = theItem.UOM
                    //    });
                    //}
                }

                string status = "";
                DataTable dt = ItemSupplierMapController.FetchItemSupplierMapNew(theItem.ItemNo, supplierID, out status);

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j]["ItemNo"].ToString() != theItem.ItemNo)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = dt.Rows[j]["UOM"].ToString(),
                            PackingSizeUOM = dt.Rows[j]["DeductConvType"].ToString() == "1" ? (1 / (dt.Rows[j]["DeductConvRate"].ToString().GetDecimalValue() == 0 ? 1 : dt.Rows[j]["DeductConvRate"].ToString().GetDecimalValue())) : dt.Rows[j]["DeductConvRate"].ToString().GetDecimalValue(),
                            PackingSizeCostPrice = dt.Rows[j]["CostPrice"].ToString().GetDecimalValue(),
                            BaseUOM = theItem.UOM
                        });
                    }
                }
            }

            return packingSize;
        }

        public static List<PackingSize> FetchPackingSizeByItemNoAndSupplierForDropdown(string itemNo, int supplierID)
        {
            bool showPackingSize = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false);
            List<PackingSize> packingSize = new List<PackingSize>();
            Item theItem = new Item(itemNo);

            if (!theItem.IsNew && !string.IsNullOrEmpty(theItem.UOM))
            {
                packingSize.Add(new PackingSize
                {
                    PackingSizeName = theItem.UOM,
                    PackingSizeUOM = 1,
                    PackingSizeCostPrice = theItem.FactoryPrice,
                    BaseUOM = theItem.UOM
                });
            }

            if (showPackingSize)
            {
                Query qr = new Query("ItemSupplierMap");
                qr.AddWhere(ItemSupplierMap.Columns.ItemNo, itemNo);
                qr.AddWhere(ItemSupplierMap.Columns.SupplierID, supplierID);
                ItemSupplierMap ism = new ItemSupplierMapController().FetchByQuery(qr).FirstOrDefault();
                if (ism != null)
                {
                    int packingSizeNo = 10;
                    if (packingSizeNo >= 1 && !string.IsNullOrEmpty(ism.PackingSize1) && ism.PackingSizeUOM1.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize1,
                            PackingSizeUOM = ism.PackingSizeUOM1.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice1.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 2
                        && !string.IsNullOrEmpty(ism.PackingSize2)
                        && ism.PackingSizeUOM2.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize2,
                            PackingSizeUOM = ism.PackingSizeUOM2.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice2.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 3
                        && !string.IsNullOrEmpty(ism.PackingSize3)
                        && ism.PackingSizeUOM3.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize3,
                            PackingSizeUOM = ism.PackingSizeUOM3.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice3.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 4
                        && !string.IsNullOrEmpty(ism.PackingSize4)
                        && ism.PackingSizeUOM4.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize4,
                            PackingSizeUOM = ism.PackingSizeUOM4.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice4.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 5
                        && !string.IsNullOrEmpty(ism.PackingSize5)
                        && ism.PackingSizeUOM5.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize5,
                            PackingSizeUOM = ism.PackingSizeUOM5.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice5.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 6
                        && !string.IsNullOrEmpty(ism.PackingSize6)
                        && ism.PackingSizeUOM6.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize6,
                            PackingSizeUOM = ism.PackingSizeUOM6.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice6.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 7
                        && !string.IsNullOrEmpty(ism.PackingSize7)
                        && ism.PackingSizeUOM7.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize7,
                            PackingSizeUOM = ism.PackingSizeUOM7.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice7.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 8
                        && !string.IsNullOrEmpty(ism.PackingSize8)
                        && ism.PackingSizeUOM8.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize8,
                            PackingSizeUOM = ism.PackingSizeUOM8.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice8.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 9
                        && !string.IsNullOrEmpty(ism.PackingSize9)
                        && ism.PackingSizeUOM9.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize9,
                            PackingSizeUOM = ism.PackingSizeUOM9.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice9.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    //if (packingSizeNo >= 10
                    //    && !string.IsNullOrEmpty(ism.PackingSize10)
                    //    && ism.PackingSizeUOM10.GetValueOrDefault(0) > 0)
                    //{
                    //    packingSize.Add(new PackingSize
                    //    {
                    //        PackingSizeName = ism.PackingSize10,
                    //        PackingSizeUOM = ism.PackingSizeUOM10.GetValueOrDefault(0),
                    //        PackingSizeCostPrice = ism.CostPrice10.GetValueOrDefault(0),
                    //        BaseUOM = theItem.UOM
                    //    });
                    //}
                }
            }

            return packingSize;
        }

        public static List<PackingSize> FetchPackingSizeByItemNoAndSupplierForDropdownNew(string itemNo, int supplierID)
        {
            bool showPackingSize = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false);
            List<PackingSize> packingSize = new List<PackingSize>();
            Item theItem = new Item(itemNo);

            if (!theItem.IsNew && !string.IsNullOrEmpty(theItem.UOM))
            {
                packingSize.Add(new PackingSize
                {
                    PackingSizeName = theItem.UOM,
                    PackingSizeUOM = 1,
                    PackingSizeCostPrice = theItem.FactoryPrice,
                    BaseUOM = theItem.UOM
                });
            }

            if (showPackingSize)
            {
                Query qr = new Query("ItemSupplierMap");
                qr.AddWhere(ItemSupplierMap.Columns.ItemNo, itemNo);
                qr.AddWhere(ItemSupplierMap.Columns.SupplierID, supplierID);
                ItemSupplierMap ism = new ItemSupplierMapController().FetchByQuery(qr).FirstOrDefault();
                if (ism != null)
                {
                    int packingSizeNo = 10;
                    if (packingSizeNo >= 1 && !string.IsNullOrEmpty(ism.PackingSize1) && ism.PackingSizeUOM1.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize1,
                            PackingSizeUOM = ism.PackingSizeUOM1.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice1.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 2
                        && !string.IsNullOrEmpty(ism.PackingSize2)
                        && ism.PackingSizeUOM2.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize2,
                            PackingSizeUOM = ism.PackingSizeUOM2.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice2.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 3
                        && !string.IsNullOrEmpty(ism.PackingSize3)
                        && ism.PackingSizeUOM3.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize3,
                            PackingSizeUOM = ism.PackingSizeUOM3.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice3.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 4
                        && !string.IsNullOrEmpty(ism.PackingSize4)
                        && ism.PackingSizeUOM4.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize4,
                            PackingSizeUOM = ism.PackingSizeUOM4.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice4.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 5
                        && !string.IsNullOrEmpty(ism.PackingSize5)
                        && ism.PackingSizeUOM5.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize5,
                            PackingSizeUOM = ism.PackingSizeUOM5.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice5.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 6
                        && !string.IsNullOrEmpty(ism.PackingSize6)
                        && ism.PackingSizeUOM6.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize6,
                            PackingSizeUOM = ism.PackingSizeUOM6.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice6.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 7
                        && !string.IsNullOrEmpty(ism.PackingSize7)
                        && ism.PackingSizeUOM7.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize7,
                            PackingSizeUOM = ism.PackingSizeUOM7.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice7.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 8
                        && !string.IsNullOrEmpty(ism.PackingSize8)
                        && ism.PackingSizeUOM8.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize8,
                            PackingSizeUOM = ism.PackingSizeUOM8.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice8.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 9
                        && !string.IsNullOrEmpty(ism.PackingSize9)
                        && ism.PackingSizeUOM9.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize9,
                            PackingSizeUOM = ism.PackingSizeUOM9.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice9.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                    if (packingSizeNo >= 10
                        && !string.IsNullOrEmpty(ism.PackingSize10)
                        && ism.PackingSizeUOM10.GetValueOrDefault(0) > 0)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = ism.PackingSize10,
                            PackingSizeUOM = ism.PackingSizeUOM10.GetValueOrDefault(0),
                            PackingSizeCostPrice = ism.CostPrice10.GetValueOrDefault(0),
                            BaseUOM = theItem.UOM
                        });
                    }
                }
            


                string status = "";
                DataTable dt = ItemSupplierMapController.FetchItemSupplierMapNew(theItem.ItemNo, supplierID, out status);

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j]["ItemNo"].ToString() != theItem.ItemNo)
                    {
                        packingSize.Add(new PackingSize
                        {
                            PackingSizeName = dt.Rows[j]["UOM"].ToString(),
                            PackingSizeUOM = dt.Rows[j]["DeductConvType"].ToString() == "1" ? (1 / (dt.Rows[j]["DeductConvRate"].ToString().GetDecimalValue() == 0 ? 1 : dt.Rows[j]["DeductConvRate"].ToString().GetDecimalValue())) : dt.Rows[j]["DeductConvRate"].ToString().GetDecimalValue(),
                            PackingSizeCostPrice = dt.Rows[j]["CostPrice"].ToString().GetDecimalValue(),
                            BaseUOM = theItem.UOM
                        });
                    }
                }

            }

            return packingSize;
        }

        public static PackingSize FetchPackingSizeIndividualy(string itemNo, int SupplierID, string UOM)
        {
            List<PackingSize> data = FetchPackingSizeByItemNoAndSupplierNew(itemNo, SupplierID);

            PackingSize objReturn = new PackingSize();

            var c = (from f in data where f.PackingSizeName == UOM select f).FirstOrDefault();

            if (c != null)
                objReturn = c;

            return objReturn;
        }

    }
}
