using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.Collections;
namespace WinTestModules
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PointOfSaleController.GetPointOfSaleInfo();

            //ExportController.getDataTableFromXLS("c:\\marina square\\abc.xls");
            ExportController.ImportInventoryFromExcel("d:\\test\\abc.csv");
            MessageBox.Show("DONE");
            /*
            PromotionAdminController.InsertBuyXatThePriceOfY
                ("Test123aa", DateTime.Now.AddDays(-3), DateTime.Now.AddDays(5), "SPA Set 2", 1, 1, 0.0, true);

            Application.Exit();
            */
            /*
            PointOfSaleController.GetPointOfSaleInfo();
            POSController pos = new POSController();            
            
            //Scenario 1 ->
            //1 promo, correct item
            string status;
            pos.AddItemToOrder(new Item("I000000005"), 1, 0, true, out status);
            pos.AddItemToOrder(new Item("I000000009"), 1, 0, true, out status);
            pos.AddItemToOrder(new Item("I000000011"), 1, 0, true, out status);

            pos.AddItemToOrder(new Item("I000000028"), 1, 0, true, out status);
            pos.AddItemToOrder(new Item("I000000029"), 1, 0, true, out status);
            
            pos.AddItemToOrder(new Item("I000000002"), 1, 0, true, out status);
            pos.AddItemToOrder(new Item("I000000001"), 1, 0, true, out status);            
            pos.AddItemToOrder(new Item("I000000004"), 1, 0, true, out status);

            ArrayList focItem,promolineID;
            int FOCPromoQty, MultiplierQty;

            ApplyPromotionController apl = new ApplyPromotionController(pos);
            ViewPromoMasterDetail v = apl.ValidBuyXAtThePriceOfYPromo
                (pos.FetchUnsavedOrderDet(), out focItem, out promolineID, out FOCPromoQty, out MultiplierQty);

            pos.ApplyPromo();
            
            int i = 0;
            i += 2;

            i += focItem.Count;
            i = v.PromoCampaignHdrID;
            //Scenario 2 ->
            //1 promo, insufficient item
            

            //Scenario 3 ->
            //1 promo, extra item

            /*
            String staTUS;
            MembershipController.AddPoints("0707000001", DateTime.Parse("2006-08-31"), 
                DateTime.Parse("2008-08-31"), 3000, out staTUS);
            //MembershipController.RedeemAHAVAMemberPoints("oxx", "0710010007", DateTime.Parse("2006-08-31"), out staTUS);
            */
            /*
            string status;
            int currentBal =
                InventoryController.GetStockBalanceQtyByItem("906609B", 7, out status);
            int yestBal =
                InventoryController.GetStockBalanceQtyByItemByDate("906609B", 7, DateTime.Now.AddDays(-1), out status);
            string s = "";*/
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}