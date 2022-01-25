using SubSonic;
using PowerPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinUtility
{
    public partial class frmRecreateStock : Form
    {
        public frmRecreateStock()
        {
            InitializeComponent();
        }

        private void btnRegenerate_Click(object sender, EventArgs e)
        {
            StockTakeCollection allStockTake = new StockTakeCollection();
            allStockTake.LoadAndCloseReader(DataService.GetReader(new QueryCommand("SELECT * FROM Tmp_StockTake ORDER BY StockTakeDate")));

            OrderHdrCollection allOrderHdr = new OrderHdrCollection();
            allOrderHdr.LoadAndCloseReader(DataService.GetReader(new QueryCommand("SELECT * FROM OrderHdr ORDER BY OrderDate")));

            InventoryHdrCollection allInventoryHdr = new InventoryHdrCollection();
            allInventoryHdr.LoadAndCloseReader(DataService.GetReader(new QueryCommand("SELECT * FROM Tmp_InventoryHdr WHERE UserName <> 'SYSTEM' AND InventoryHdrRefNo NOT LIKE 'ST%' ORDER BY InventoryDate")));

            while (allStockTake.Count > 0 || allOrderHdr.Count > 0 || allInventoryHdr.Count > 0)
            {
                DateTime Time_StockTake = DateTime.MaxValue;
                DateTime Time_Inventory = DateTime.MaxValue;
                DateTime Time_Order = DateTime.MaxValue;

                if (allStockTake.Count > 0) Time_StockTake = allStockTake[0].StockTakeDate;
                if (allInventoryHdr.Count > 0) Time_Inventory = allInventoryHdr[0].InventoryDate;
                if (allOrderHdr.Count > 0) Time_Order = allOrderHdr[0].OrderDate;

                if (Time_Inventory <= Time_Order && Time_Inventory <= Time_StockTake)
                {
                    InventoryController Ctl;

                    string CostMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod).ToLower();
                    if (CostMethod == "fifo")
                        Ctl = new InventoryController(PowerPOS.Container.CostingMethods.FIFO);
                    if (CostMethod == "fixed avg")
                        Ctl = new InventoryController(PowerPOS.Container.CostingMethods.FixedAvg);
                    else
                        Ctl = new InventoryController(PowerPOS.Container.CostingMethods.FIFO);

                    //Ctl.SetInventoryHeaderInfo
                    //    (allInventoryHdr[0].PurchaseOrderNo, txtSupplier.Text, txtRemark.Text,
                    //    freightCharges, exchangeRate, discount);


                }
                else if (Time_Order <= Time_Inventory && Time_Order <= Time_StockTake)
                {
                }
                else
                {
                }
            }

            ItemCollection allItems = new ItemCollection();
            allItems.LoadAndCloseReader(DataService.GetReader(new QueryCommand("SELECT * FROM Item WHERE IsInInventory = 1 AND CategoryName <> 'SYSTEM'")));

            foreach (Item myItem in allItems)
            {

            }
        }
    }
}
