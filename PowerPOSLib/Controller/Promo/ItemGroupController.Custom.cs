using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS;
using SubSonic;
using System.ComponentModel;

namespace PowerPOS
{
    
    public partial class ItemGroupController
    {
        public static bool InsertItemGroup(string ItemGroupName, DataTable Items, string Barcode, out string status)
        {
            try
            {
                //Check Item Itemgroupname has already been in use
                Where whr = new Where();
                whr.ColumnName = ItemGroup.Columns.ItemGroupName;
                whr.Comparison = Comparison.Equals;
                whr.ParameterName = "@ItemGroupName";
                whr.ParameterValue = ItemGroupName;
                whr.TableName = "ItemGroup";
                if ((int)ItemGroup.CreateQuery().GetCount(ItemGroup.Columns.ItemGroupId, whr) > 0)
                {
                    status = "Item Group Name has already been taken up. Please specify a new Item Group Name.";
                    return false;
                }

                ItemGroup at = new ItemGroup();
                at.ItemGroupName = ItemGroupName;
                at.Barcode = Barcode;
                at.Deleted = false;
                at.Save();

                ItemGroupMap myMap;
                for (int i = 0; i < Items.Rows.Count; i++)
                {
                    myMap = new ItemGroupMap();
                    myMap.ItemNo = Items.Rows[i]["ItemNo"].ToString();
                    myMap.ItemGroupID = at.ItemGroupId;
                    myMap.UnitQty = int.Parse(Items.Rows[i]["UnitQty"].ToString());
                    myMap.Deleted = false;
                    myMap.Save();
                }
                status = "";
                return true;
            }
            catch (Exception ex)
            {
                status = "Unknown error has been encountered. Detail:" + ex.Message;
                Logger.writeLog(ex);
                return false;
             }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ViewItemGroupMapCollection FetchItemGroupMapByName(object ItemGroupID)
        {            
            ViewItemGroupMapCollection coll = new ViewItemGroupMapCollection().Where("ItemGroupID", ItemGroupID).Load();
            return coll;
        }
    }
}
