using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPIntegration.API.NAVISION
{
    public class Navision : IERPIntegration
    {
        private List<string> arguments;
        public Navision(List<string> args)
        {
            this.arguments = args;
        }


        #region IERPIntegration Members
        
        public bool ExportOrderPayment(out string status)
        {
            return Navision_Sales.DoSendSales(arguments, out status);
        }

        public bool ExportGoodsOrder(out string status)
        {
            return Navision_SO.Generate_NAV_SO(arguments, out status);
        }

        public bool ExportStockReturn(out string status)
        {
            return Navision_PO.Generate_NAV_PO(arguments, out status);
        }

        public bool ImportOrderApproval(out string status)
        {
            return Navision_DO.Import_NAV_DO(arguments, out status);
        }

        #endregion


        #region No Implementation

        public bool DoStockTake(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ExportInventory(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ExportOrderRpt(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ImportInventory(out string status)
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

        #endregion
    }
}
