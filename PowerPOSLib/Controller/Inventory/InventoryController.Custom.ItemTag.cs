using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
	public partial class InventoryController
	{
        public bool IsSerialNoValid(List<string> listInvDet, out string message)
        {
            bool isValid = true;
            message = "";

            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo), false))
                return true;

            try
            {
                foreach (var det in InvDet)
                {
                    if (!listInvDet.Contains(det.InventoryDetRefNo)) 
                        continue;

                    var item = det.Item;
                    var serialNo = det.SerialNo;
                    if (item.IsUseSerialNo && (serialNo == null || serialNo.Count == 0))
                    {
                        isValid = false;
                        message += string.Format("Item {0} requires to enter serial no", item.ItemName);
                        message += Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                Logger.writeLog(ex);
            }

            return isValid;
        }

        public bool checkSerialNoIsExist(Item _item, int inventoryLocationID, string serialNo, int qty, out string _message)
        {
            _message = "";
            List<string> _serialNo = serialNo.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (_serialNo.Count != qty)
            {
                _message = string.Format("Serial No count ({0}) did not tally with entered qty ({1}). Please check!", _serialNo.Count, qty);
                return false;
            }

            ItemTagModel input = new ItemTagModel();
            input.ItemNo = _item.ItemNo;
            input.InventoryLocationID = inventoryLocationID;
            input.SerialNoColl = _serialNo;
            var inputColl = new List<ItemTagModel>();
            inputColl.Add(input);
            bool isValid = false;

            isValid = ItemTagController.CheckSerialNoIsExistHelper(inputColl, out _message);

            return isValid;
        }

        public bool checkSerialNoIsExistInPurchaseOrder(string PurchaseOrderHeaderRefNo, string itemNo, string serialNo, int qty, out string _message)
        {
            _message = "";
            List<string> _serialNo = serialNo.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (_serialNo.Count != qty)
            {
                _message = string.Format("Serial No count ({0}) did not tally with entered qty ({1}). Please check!", _serialNo.Count, qty);
                return false;
            }

            ///check if serial no exist 
            ///

            PurchaseOrderController por = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
            
            
            bool isValid = false;

            isValid = por.IsSerialNoExist(itemNo, _serialNo, out _message);

            return isValid;
        }
	}
}
