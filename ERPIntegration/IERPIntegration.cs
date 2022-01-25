using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPIntegration
{
    public interface IERPIntegration
    {
        //Event

        //Method
        bool DoStockTake(out string status);
        bool ExportOrderPayment(out string status);
        bool ExportInventory(out string status);
        bool ExportOrderRpt(out string status);
        bool ImportInventory(out string status);
        bool ImportItem(out string status);
        bool ImportMember(out string status);
        bool ExportGoodsOrder(out string status);
        bool ExportStockReturn(out string status);
        bool ImportOrderApproval(out string status);
        

        //Connection & Status
        bool Connect(out string status);
        bool checkStatus(out string status);

        //
    }
}
