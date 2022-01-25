using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPIntegration.API.MONEYWORKS
{
    public class Moneyworks: IERPIntegration
    {
        #region IERPIntegration Members

        public bool DoStockTake(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ExportOrderPayment(out string status)
        {
            return MoneyWorks_Sale.DoSendSaleData(out status);           
        }

        public bool ExportInventory(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ImportItem(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ImportMember(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool Connect(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool checkStatus(out string status)
        {
            status = "no implementation";
            return true;
        }


        public bool ImportInventory(out string status)
        {            
            bool result= MoneyWorks_Inventory.DoSyncInventory(out status);
            return result;
        }

        #endregion

        #region IERPIntegration Members


        public bool ExportOrderRpt(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ExportGoodsOrder(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ExportStockReturn(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ImportOrderApproval(out string status)
        {
            status = "no implementation";
            return true;
        }

        #endregion
    }
}
