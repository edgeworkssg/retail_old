using System;
namespace PowerPOSLib.Controller.Inventory
{
    interface IInventoryController
    {
        bool AddItemIntoInventory(string ItemID, out string status);
        bool AddItemIntoInventory(string ItemID, int Qty, out string status);
        bool AddItemIntoInventory(string ItemID, int Qty, decimal COGS, out string status);
        bool AddItemIntoInventoryByBarcode(string Barcode, int Qty, out string status);
        bool AddItemIntoInventoryByBarcode(string Barcode, out string status);
        bool ChangeFactoryPrice(string ID, decimal newFactoryPrice, out string status);
        bool ChangeFactoryPriceByItemNo(string ItemNo, decimal newFactoryPrice, out string status);
        bool ChangeItemQty(string ID, int newQty, out string status);
        bool ChangeItemRemark(string ID, string Remark, out string status);
        bool ChangeQuantityByItemNo(string ItemNo, int newQty, out string status);
        bool ChangeRemarkByItemNo(string ItemNo, string remark, out string status);
        bool ChangeStockInRefNoByItemNo(string ItemNo, string StockInRefNo, out string status);
        bool CopyItemsFrom(string InventoryRefNo, out string status);
        bool CopyItemsFromPurchaseOrder(string PurchaseOrderHeaderRefNo, out string status);
        bool CopyItemsFromSaveItem(string SaveItemName, out string status);
        bool CorrectStockOutDiscrepancy(string username, int LocationID);
        bool CorrectTransferDiscrepancy(string username);
        bool DeleteFromInventoryDetail(string ID, out string status);
        bool DistributeNewItemNo();
        PowerPOS.ViewInventoryActivityCollection FetchInventoryStockOutDiscrepancy(int LocationID);
        PowerPOS.ViewInventoryTransferDiscrepancyCollection FetchTransferDiscrepancy();
        System.Data.DataTable FetchUnSavedInventoryGroupByItemNo(out string status);
        System.Data.DataTable FetchUnSavedInventoryItems(out string status);
        string GetInvHdrRefNo();
        int GetRollUpQuantityByItem(string ItemNo);
        bool ReceiveTransfer(string username, int LocationTransferID, out string status);
        bool SetInventoryDate(DateTime InventoryDate);
        bool SetInventoryHeaderInfo(string PurchaseOrderNo, string Supplier, string Remark, decimal freightCharges, double ExchangeRate, decimal Discount);
        bool SetPurchaseOrder(string PurchaseOrderNo);
        bool StockIn(string username, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS, out string status);
        bool StockInNonTransaction(string username, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS, out string status);
        bool StockOut(string username, int StockOutReasonID, int InventoryLocationID, bool IsAdjustment, bool deductRemainingQty, out string status);
        bool StockOutNonTransaction(string username, int StockOutReasonID, int InventoryLocationID, bool IsAdjustment, bool deductRemainingQty, out string status);
        bool TransferOut(string username, int FromInventoryLocationID, int ToInventoryLocationID, out string status);
        void UpdateInventoryDiscrepancyCostOfGoods(string InventoryDetRefNo, decimal CostOfGoods);
        void UpdateTransferDiscrepancyInformation(int DiscrepancyID, decimal CostOfGoods, string DiscrepancyReason, string DiscrepancyRemark);
    }
}
