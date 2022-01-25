using System;
using System.Collections.Generic;
using System.Text;
using PowerPOS;
using PowerPOS.Container;
using SubSonic;
using System.Data;
namespace PowerPOS.ServerScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            #region *) Assign Setting: Costing Method
            string tmpCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (tmpCostingMethod == null)
            {
                AppSetting.SetSetting(AppSetting.SettingsName.Inventory.CostingMethod, InventoryController.CostingTypes.FIFO);
                tmpCostingMethod = InventoryController.CostingTypes.FIFO;
            }
            tmpCostingMethod = tmpCostingMethod.ToLower();
            if (tmpCostingMethod == InventoryController.CostingTypes.FIFO)
                PowerPOS.Container.InventorySettings.CostingMethod = CostingMethods.FIFO;
            else if (tmpCostingMethod == InventoryController.CostingTypes.FixedAvg)
                PowerPOS.Container.InventorySettings.CostingMethod = CostingMethods.FixedAvg;
            else if (tmpCostingMethod == InventoryController.CostingTypes.WeightedAvg)
                PowerPOS.Container.InventorySettings.CostingMethod = CostingMethods.WeightedAvg;
            #endregion
         
            InventoryController.AssignStockOutToConfirmedOrderUsingTransaction(false); //assign inventory to sales that has just been transfered in
            InventoryController.GenerateInventoryHdrForAdjustedSales(); //fixed past data: create InventoryHdr from all sales marked as "Adjusted"
            MembershipController.updateExpiryDateFromRenewalRequest();
            //SalesPersonController.InsertLastMonthCommissionToHistory(); //roll salesman transaction into history                        
            //MembershipController.processRedemptionLog(); //Process point deductions
            
            MembershipController.AllocatePointsFromSales(); //Allocate sales;

            return;
                                               
        }        
    }
}
