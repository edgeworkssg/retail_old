using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;

namespace WinTestModules
{
    public partial class frmAdjustStockIn : Form
    {
        public frmAdjustStockIn()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string status;
            DateTime currentDate = new DateTime(2007,12,1);
            for (int i = 25; i > 0; i--)
            {
                PromotionAdminController.InsertDiscountByItem("Countdown1" + i, currentDate, currentDate.AddHours(24), "918774", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown2" + i, currentDate, currentDate.AddHours(24), "918775", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown3" + i, currentDate, currentDate.AddHours(24), "922909", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown4" + i, currentDate, currentDate.AddHours(24), "931008", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown5" + i, currentDate, currentDate.AddHours(24), "931016", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown6" + i, currentDate, currentDate.AddHours(24), "931031", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown7" + i, currentDate, currentDate.AddHours(24), "931032", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown8" + i, currentDate, currentDate.AddHours(24), "931035", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown9" + i, currentDate, currentDate.AddHours(24), "931050", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown10" + i, currentDate, currentDate.AddHours(24), "932005", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown11" + i, currentDate, currentDate.AddHours(24), "932006", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown12" + i, currentDate, currentDate.AddHours(24), "932022", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown13" + i, currentDate, currentDate.AddHours(24), "932075", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown14" + i, currentDate, currentDate.AddHours(24), "932079", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown15" + i, currentDate, currentDate.AddHours(24), "932083", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown16" + i, currentDate, currentDate.AddHours(24), "932084", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown17" + i, currentDate, currentDate.AddHours(24), "932086", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown18" + i, currentDate, currentDate.AddHours(24), "932087", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown19" + i, currentDate, currentDate.AddHours(24), "933004", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown20" + i, currentDate, currentDate.AddHours(24), "933016", i, false);
                //PromotionAdminController.InsertDiscountByItem("Countdown" + i, currentDate, currentDate.AddHours(24), "933026", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown21" + i, currentDate, currentDate.AddHours(24), "933075", i, false);
                //PromotionAdminController.InsertDiscountByItem("Countdown" + i, currentDate, currentDate.AddHours(24), "933118", i, false);
                PromotionAdminController.InsertDiscountByItem("Countdown22" + i, currentDate, currentDate.AddHours(24), "976901", i, false);


                currentDate = currentDate.AddDays(1);
            }
            MessageBox.Show("Done");
            /*
            InventoryController ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            ctrl.SetInventoryHeaderInfo("", "",
                "Transfer", 0.0M, 0.0, 0.0M);

            ctrl.AddItemIntoInventory("806901", 1, out status);
            ctrl.AddItemIntoInventory("859001", 9, out status);
            ctrl.AddItemIntoInventory("859101", 2, out status);
            ctrl.AddItemIntoInventory("822601", 3, out status);
            ctrl.AddItemIntoInventory("822801", 7, out status);
            ctrl.AddItemIntoInventory("822701", 2, out status);
            ctrl.AddItemIntoInventory("822043", 1, out status);
            ctrl.AddItemIntoInventory("922409", 5, out status);
            ctrl.AddItemIntoInventory("922643", 2, out status);
            ctrl.AddItemIntoInventory("922843", 2, out status);
            ctrl.AddItemIntoInventory("922743", 2, out status);
            ctrl.AddItemIntoInventory("930209", 3, out status);
            ctrl.AddItemIntoInventory("959201", 2, out status);
            ctrl.AddItemIntoInventory("933016", 5, out status);
            ctrl.AddItemIntoInventory("976901", 3, out status);
            ctrl.AddItemIntoInventory("933118", 30, out status);
            ctrl.AddItemIntoInventory("933026", 30, out status);
            ctrl.AddItemIntoInventory("932075", 7, out status);
            ctrl.AddItemIntoInventory("895043", 5, out status);
            ctrl.AddItemIntoInventory("893043", 2, out status);
            ctrl.AddItemIntoInventory("894043", 2, out status);
            ctrl.AddItemIntoInventory("993043", 1, out status);
            ctrl.AddItemIntoInventory("914043", 10, out status);
            ctrl.AddItemIntoInventory("913043", 10, out status);
            ctrl.AddItemIntoInventory("839018", 11, out status);
            ctrl.AddItemIntoInventory("811018", 1, out status);
            ctrl.AddItemIntoInventory("814018", 3, out status);
            ctrl.AddItemIntoInventory("813018", 4, out status);
            ctrl.AddItemIntoInventory("812018", 12, out status);
            ctrl.AddItemIntoInventory("803529", 39, out status);
            ctrl.AddItemIntoInventory("818043", 11, out status);
            ctrl.AddItemIntoInventory("810018", 3, out status);
            ctrl.AddItemIntoInventory("873018", 1, out status);
            ctrl.AddItemIntoInventory("869001", 9, out status);
            ctrl.AddItemIntoInventory("867018", 3, out status);
            ctrl.AddItemIntoInventory("837001", 3, out status);
            ctrl.AddItemIntoInventory("868001", 13, out status);
            ctrl.AddItemIntoInventory("841043", 8, out status);
            ctrl.AddItemIntoInventory("877043", 3, out status);
            ctrl.AddItemIntoInventory("844043", 4, out status);
            ctrl.AddItemIntoInventory("846043", 3, out status);
            ctrl.AddItemIntoInventory("845043", 3, out status);
            ctrl.AddItemIntoInventory("871043", 14, out status);
            ctrl.AddItemIntoInventory("866043", 4, out status);
            ctrl.AddItemIntoInventory("842043", 1, out status);
            ctrl.AddItemIntoInventory("879043", 5, out status);
            ctrl.AddItemIntoInventory("848043", 10, out status);
            ctrl.AddItemIntoInventory("847043", 7, out status);
            ctrl.AddItemIntoInventory("874043", 17, out status);
            ctrl.AddItemIntoInventory("864043", 5, out status);
            ctrl.AddItemIntoInventory("865043", 9, out status);
            ctrl.AddItemIntoInventory("863043", 2, out status);
            ctrl.AddItemIntoInventory("862043", 5, out status);
            ctrl.AddItemIntoInventory("891043", 2, out status);
            ctrl.AddItemIntoInventory("878043", 2, out status);
            ctrl.AddItemIntoInventory("892043", 3, out status);
            ctrl.AddItemIntoInventory("931079", 60, out status);
            ctrl.AddItemIntoInventory("941043", 15, out status);
            ctrl.AddItemIntoInventory("891099", 1, out status);
            ctrl.AddItemIntoInventory("977043", 20, out status);
            ctrl.AddItemIntoInventory("978043", 4, out status);
            ctrl.AddItemIntoInventory("944043", 3, out status);
            ctrl.AddItemIntoInventory("945043", 30, out status);
            ctrl.AddItemIntoInventory("958043", 14, out status);
            ctrl.AddItemIntoInventory("971043", 4, out status);
            ctrl.AddItemIntoInventory("957043", 12, out status);
            ctrl.AddItemIntoInventory("956043", 7, out status);
            ctrl.AddItemIntoInventory("967043", 2, out status);
            ctrl.AddItemIntoInventory("942043", 24, out status);
            ctrl.AddItemIntoInventory("974043", 7, out status);
            ctrl.AddItemIntoInventory("948043", 8, out status);
            ctrl.AddItemIntoInventory("947043", 10, out status);
            ctrl.AddItemIntoInventory("974143", 15, out status);
            ctrl.AddItemIntoInventory("965043", 30, out status);
            ctrl.AddItemIntoInventory("961043", 2, out status);
            ctrl.AddItemIntoInventory("962043", 17, out status);
            ctrl.AddItemIntoInventory("880004", 4, out status);
            ctrl.AddItemIntoInventory("881001", 3, out status);
            ctrl.AddItemIntoInventory("980009", 26, out status);
            ctrl.AddItemIntoInventory("981009", 21, out status);
            ctrl.AddItemIntoInventory("983009", 23, out status);            
 
            bool c = ctrl.TransferOut("admin", 7, 13,out status);
            MessageBox.Show(c.ToString() + " " + status);
            */
        }
    }
}
/* - wholesale oct
 *             ctrl.AddItemIntoInventory("822109", 29, out status);
            ctrl.AddItemIntoInventory("907404", 29, out status);
            ctrl.AddItemIntoInventory("822009", 29, out status);
            ctrl.AddItemIntoInventory("958009", 125, out status);
            ctrl.AddItemIntoInventory("957004", 125, out status);
            ctrl.AddItemIntoInventory("956004", 125, out status);
*/
/* - August
            ctrl.AddItemIntoInventory("931035", 2, out status);
            ctrl.AddItemIntoInventory("897018", 18, out status);
            ctrl.AddItemIntoInventory("898018", 18, out status);
            ctrl.AddItemIntoInventory("839018", 92, out status);
            ctrl.AddItemIntoInventory("803529", 41, out status);
            ctrl.AddItemIntoInventory("800029", 1, out status);
            ctrl.AddItemIntoInventory("875143", 1, out status);
            ctrl.AddItemIntoInventory("872018", 1, out status);
            ctrl.AddItemIntoInventory("931079", 90, out status);
            ctrl.AddItemIntoInventory("941004", 2, out status);
            ctrl.AddItemIntoInventory("973004", 2, out status);
            ctrl.AddItemIntoInventory("958009", 29, out status);
            ctrl.AddItemIntoInventory("957009", 25, out status);
            ctrl.AddItemIntoInventory("957004", 124, out status);
            ctrl.AddItemIntoInventory("956004", 124, out status);
            ctrl.AddItemIntoInventory("942004", 2, out status);
            ctrl.AddItemIntoInventory("965004", 1, out status);
            ctrl.AddItemIntoInventory("824234", 2, out status);
            */


/* - Wholesale
ctrl.AddItemIntoInventory("806901", 2, out status);
ctrl.AddItemIntoInventory("822143", 1, out status);
ctrl.AddItemIntoInventory("976901", 1, out status);
ctrl.AddItemIntoInventory("931035", 3, out status);
ctrl.AddItemIntoInventory("932087", 2, out status);
ctrl.AddItemIntoInventory("894043", 1, out status);
ctrl.AddItemIntoInventory("839018", 2, out status);
ctrl.AddItemIntoInventory("818043", 1, out status);
ctrl.AddItemIntoInventory("873018", 3, out status);
ctrl.AddItemIntoInventory("858043", 1, out status);
ctrl.AddItemIntoInventory("845043", 2, out status);
ctrl.AddItemIntoInventory("842043", 2, out status);
ctrl.AddItemIntoInventory("865043", 2, out status);
ctrl.AddItemIntoInventory("863043", 1, out status);
ctrl.AddItemIntoInventory("857043", 4, out status);
ctrl.AddItemIntoInventory("891043", 2, out status);
ctrl.AddItemIntoInventory("878043", 2, out status);
ctrl.AddItemIntoInventory("856043", 4, out status);
ctrl.AddItemIntoInventory("881001", 2, out status);
*/
/*             --Bethany
            ctrl.AddItemIntoInventory("859001", 1, out status);
            ctrl.AddItemIntoInventory("859101", 1, out status);
            ctrl.AddItemIntoInventory("931032", 4, out status);
            ctrl.AddItemIntoInventory("976901", 2, out status);
            ctrl.AddItemIntoInventory("931035", 1, out status);
            ctrl.AddItemIntoInventory("931016", 1, out status);
            ctrl.AddItemIntoInventory("897018", 2, out status);
            ctrl.AddItemIntoInventory("895043", 3, out status);
            ctrl.AddItemIntoInventory("995043", 1, out status);
            ctrl.AddItemIntoInventory("993043", 1, out status);
            ctrl.AddItemIntoInventory("913043", 2, out status);
            ctrl.AddItemIntoInventory("839018", 8, out status);
            ctrl.AddItemIntoInventory("813018", 2, out status);
            ctrl.AddItemIntoInventory("812018", 3, out status);
            ctrl.AddItemIntoInventory("800029", 1, out status);
            ctrl.AddItemIntoInventory("873018", 3, out status);
            ctrl.AddItemIntoInventory("858043", 1, out status);
            ctrl.AddItemIntoInventory("841043", 3, out status);
            ctrl.AddItemIntoInventory("877043", 2, out status);
            ctrl.AddItemIntoInventory("845043", 3, out status);
            ctrl.AddItemIntoInventory("871043", 2, out status);
            ctrl.AddItemIntoInventory("866043", 1, out status);
            ctrl.AddItemIntoInventory("842043", 2, out status);
            ctrl.AddItemIntoInventory("879043", 2, out status);
            ctrl.AddItemIntoInventory("865043", 2, out status);
            ctrl.AddItemIntoInventory("857043", 2, out status);
            ctrl.AddItemIntoInventory("891043", 1, out status);
            ctrl.AddItemIntoInventory("878043", 1, out status);
            ctrl.AddItemIntoInventory("856043", 1, out status);
            ctrl.AddItemIntoInventory("892043", 1, out status);
            ctrl.AddItemIntoInventory("973043", 2, out status);
            ctrl.AddItemIntoInventory("958043", 2, out status);
            ctrl.AddItemIntoInventory("971043", 1, out status);
            ctrl.AddItemIntoInventory("957004", 4, out status);
            ctrl.AddItemIntoInventory("957043", 2, out status);
            ctrl.AddItemIntoInventory("956004", 4, out status);
            ctrl.AddItemIntoInventory("956043", 2, out status);
            ctrl.AddItemIntoInventory("947043", 1, out status);
            ctrl.AddItemIntoInventory("965043", 1, out status);
            ctrl.AddItemIntoInventory("883001", 1, out status);
            ctrl.AddItemIntoInventory("980009", 1, out status);
            */
/*          FOP
            ctrl.AddItemIntoInventory("806901", 3, out status);
            ctrl.AddItemIntoInventory("859210", 3, out status);
            ctrl.AddItemIntoInventory("859101", 1, out status);
            ctrl.AddItemIntoInventory("822601", 2, out status);
            ctrl.AddItemIntoInventory("822143", 4, out status);
            ctrl.AddItemIntoInventory("822043", 1, out status);
            ctrl.AddItemIntoInventory("931032", 3, out status);
            ctrl.AddItemIntoInventory("976901", 1, out status);
            ctrl.AddItemIntoInventory("932083", 7, out status);
            ctrl.AddItemIntoInventory("932087", 3, out status);
            ctrl.AddItemIntoInventory("897018", 3, out status);
            ctrl.AddItemIntoInventory("895043", 3, out status);
            ctrl.AddItemIntoInventory("894043", 1, out status);
            ctrl.AddItemIntoInventory("898018", 2, out status);
            ctrl.AddItemIntoInventory("896043", 2, out status);
            ctrl.AddItemIntoInventory("839018", 6, out status);
            ctrl.AddItemIntoInventory("813018", 1, out status);
            ctrl.AddItemIntoInventory("812018", 1, out status);
            ctrl.AddItemIntoInventory("803529", 27, out status);
            ctrl.AddItemIntoInventory("800029", 2, out status);
            ctrl.AddItemIntoInventory("818043", 2, out status);
            ctrl.AddItemIntoInventory("873018", 10, out status);
            ctrl.AddItemIntoInventory("858043", 21, out status);
            ctrl.AddItemIntoInventory("841043", 8, out status);
            ctrl.AddItemIntoInventory("877043", 10, out status);
            ctrl.AddItemIntoInventory("844043", 6, out status);
            ctrl.AddItemIntoInventory("846043", 3, out status);
            ctrl.AddItemIntoInventory("845043", 5, out status);
            ctrl.AddItemIntoInventory("871043", 3, out status);
            ctrl.AddItemIntoInventory("866043", 6, out status);
            ctrl.AddItemIntoInventory("842043", 5, out status);
            ctrl.AddItemIntoInventory("879043", 3, out status);
            ctrl.AddItemIntoInventory("848043", 4, out status);
            ctrl.AddItemIntoInventory("847043", 12, out status);
            ctrl.AddItemIntoInventory("874043", 7, out status);
            ctrl.AddItemIntoInventory("864043", 1, out status);
            ctrl.AddItemIntoInventory("865043", 9, out status);
            ctrl.AddItemIntoInventory("863043", 1, out status);
            ctrl.AddItemIntoInventory("862043", 9, out status);
            ctrl.AddItemIntoInventory("857043", 9, out status);
            ctrl.AddItemIntoInventory("856043", 15, out status);
            */
/*          SUNTEC
 * 
            ctrl.SetInventoryDate(new DateTime(2007,07,21));
            ctrl.AddItemIntoInventory("806901", 	1	, out status);
            ctrl.AddItemIntoInventory("859001", 	1	, out status);
            ctrl.AddItemIntoInventory("859101", 	1	, out status);
            ctrl.AddItemIntoInventory("822601", 	1	, out status);
            ctrl.AddItemIntoInventory("822143", 	1	, out status);
            ctrl.AddItemIntoInventory("822043", 	1	, out status);
            ctrl.AddItemIntoInventory("931031", 	1	, out status);
            ctrl.AddItemIntoInventory("839018", 	16	, out status);
            ctrl.AddItemIntoInventory("814018", 	5	, out status);
            ctrl.AddItemIntoInventory("812018", 	1	, out status);
            ctrl.AddItemIntoInventory("803529", 	18	, out status);
            ctrl.AddItemIntoInventory("810018", 	3	, out status);
            ctrl.AddItemIntoInventory("858043", 	7	, out status);
            ctrl.AddItemIntoInventory("877043", 	2	, out status);
            ctrl.AddItemIntoInventory("842043", 	2	, out status);
            ctrl.AddItemIntoInventory("847043", 	5	, out status);
            ctrl.AddItemIntoInventory("860043", 	1	, out status);
            ctrl.AddItemIntoInventory("865043", 	1	, out status);
            ctrl.AddItemIntoInventory("857043", 	2	, out status);
            ctrl.AddItemIntoInventory("891043", 	2	, out status);
            ctrl.AddItemIntoInventory("856043", 	6	, out status);
            ctrl.AddItemIntoInventory("824234", 1	, out status);
            ctrl.AddItemIntoInventory("880004", 1	, out status);

 */
/*
ctrl.AddItemIntoInventory("931035", 2, out status);
ctrl.AddItemIntoInventory("897018", 18, out status);
ctrl.AddItemIntoInventory("898018", 18, out status);
ctrl.AddItemIntoInventory("839018", 92, out status);
ctrl.AddItemIntoInventory("803529", 41, out status);
ctrl.AddItemIntoInventory("800029", 1, out status);
ctrl.AddItemIntoInventory("875143", 1, out status);
ctrl.AddItemIntoInventory("872018", 1, out status);
ctrl.AddItemIntoInventory("931079", 90, out status);
ctrl.AddItemIntoInventory("941004", 2, out status);
ctrl.AddItemIntoInventory("973004", 2, out status);
ctrl.AddItemIntoInventory("958009", 29, out status);
ctrl.AddItemIntoInventory("957009", 25, out status);
ctrl.AddItemIntoInventory("957004", 124, out status);
ctrl.AddItemIntoInventory("956004", 124, out status);
ctrl.AddItemIntoInventory("942004", 2, out status);
ctrl.AddItemIntoInventory("965004", 1, out status);
ctrl.AddItemIntoInventory("824234", 2, out status);	
*/
           /*
            ctrl.AddItemIntoInventory("806901", 3, out status);
            ctrl.AddItemIntoInventory("859210", 3, out status);
            ctrl.AddItemIntoInventory("859101", 1, out status);
            ctrl.AddItemIntoInventory("822601", 4, out status);
            ctrl.AddItemIntoInventory("822143", 4, out status);
            ctrl.AddItemIntoInventory("822043", 1, out status);
            ctrl.AddItemIntoInventory("931032", 1, out status);
            ctrl.AddItemIntoInventory("976901", 1, out status);
            ctrl.AddItemIntoInventory("932083", 7, out status);
            ctrl.AddItemIntoInventory("932087", 3, out status);
            ctrl.AddItemIntoInventory("897018", 3, out status);            
            ctrl.AddItemIntoInventory("893043", 3, out status);
            ctrl.AddItemIntoInventory("894043", 1, out status);
            ctrl.AddItemIntoInventory("898018", 2, out status);
            ctrl.AddItemIntoInventory("896043", 2, out status);
            ctrl.AddItemIntoInventory("839018", 26, out status);
            ctrl.AddItemIntoInventory("813018", 3, out status);
            ctrl.AddItemIntoInventory("812018", 3, out status);
            ctrl.AddItemIntoInventory("803529", 21, out status);
            ctrl.AddItemIntoInventory("800029", 2, out status);
            ctrl.AddItemIntoInventory("818043", 4, out status);
            ctrl.AddItemIntoInventory("873018", 10, out status);
            ctrl.AddItemIntoInventory("858043", 21, out status);
            ctrl.AddItemIntoInventory("841043", 8, out status);
            ctrl.AddItemIntoInventory("877043", 13, out status);
            ctrl.AddItemIntoInventory("844043", 4, out status);
            ctrl.AddItemIntoInventory("846043", 3, out status);            
            ctrl.AddItemIntoInventory("871043", 3, out status);
            ctrl.AddItemIntoInventory("866043", 6, out status);
            ctrl.AddItemIntoInventory("842043", 5, out status);
            ctrl.AddItemIntoInventory("879043", 1, out status);
            ctrl.AddItemIntoInventory("848043", 4, out status);
            ctrl.AddItemIntoInventory("847043", 8, out status);            
            ctrl.AddItemIntoInventory("864043", 1, out status);
            ctrl.AddItemIntoInventory("865043", 13, out status);
            ctrl.AddItemIntoInventory("863043", 1, out status);
            ctrl.AddItemIntoInventory("862043", 9, out status);
            ctrl.AddItemIntoInventory("857043", 9, out status);
            ctrl.AddItemIntoInventory("856043", 15, out status);
           */
            /*
            ctrl.AddItemIntoInventory("931035", 2, out status);
            ctrl.AddItemIntoInventory("858043", 6, out status);
            ctrl.AddItemIntoInventory("866043", 1, out status);
             */
            /*
            ctrl.AddItemIntoInventory("812018", 1, out status);
            ctrl.AddItemIntoInventory("812018", 1, out status);
            ctrl.AddItemIntoInventory("812018", 1, out status);
            ctrl.AddItemIntoInventory("839018", 2, out status);
            ctrl.AddItemIntoInventory("856043", 1, out status);
            ctrl.AddItemIntoInventory("878043", 1, out status);
            ctrl.AddItemIntoInventory("913043", 2, out status);
            ctrl.AddItemIntoInventory("947043", 1, out status);
            ctrl.AddItemIntoInventory("956004", 4, out status);
            ctrl.AddItemIntoInventory("956043", 2, out status);
            ctrl.AddItemIntoInventory("957004", 4, out status);
            ctrl.AddItemIntoInventory("957043", 2, out status);
            ctrl.AddItemIntoInventory("958043", 2, out status);
            ctrl.AddItemIntoInventory("965043", 1, out status);
            ctrl.AddItemIntoInventory("971043", 1, out status);
            ctrl.AddItemIntoInventory("973043", 2, out status);
            ctrl.AddItemIntoInventory("980009", 1, out status);
            ctrl.AddItemIntoInventory("993043", 1, out status);
            ctrl.AddItemIntoInventory("995043", 1, out status);
            */
            /*
            ctrl.AddItemIntoInventory("845043", 1, out status);
            ctrl.AddItemIntoInventory("874043", 3, out status);
            ctrl.AddItemIntoInventory("874043", 1, out status);
            ctrl.AddItemIntoInventory("874043", 1, out status);
            ctrl.AddItemIntoInventory("874043", 1, out status);
            ctrl.AddItemIntoInventory("874043", 1, out status);
            ctrl.AddItemIntoInventory("877043", 1, out status);
            ctrl.AddItemIntoInventory("893043", 1, out status);
            ctrl.AddItemIntoInventory("895043", 1, out status);
            */
            /*
            ctrl.AddItemIntoInventory("806901", 1, out status);
            ctrl.AddItemIntoInventory("859001", 1, out status);
            ctrl.AddItemIntoInventory("859101", 1, out status);
            ctrl.AddItemIntoInventory("822601", 1, out status);
            ctrl.AddItemIntoInventory("822143", 1, out status);
            ctrl.AddItemIntoInventory("822043", 1, out status);
            ctrl.AddItemIntoInventory("931031", 1, out status);
            ctrl.AddItemIntoInventory("839018", 16, out status);
            ctrl.AddItemIntoInventory("814018", 5, out status);
            ctrl.AddItemIntoInventory("812018", 1, out status);
            ctrl.AddItemIntoInventory("803529", 18, out status);
            ctrl.AddItemIntoInventory("810018", 3, out status);
            ctrl.AddItemIntoInventory("858043", 7, out status);
            ctrl.AddItemIntoInventory("877043", 2, out status);
            ctrl.AddItemIntoInventory("842043", 2, out status);
            ctrl.AddItemIntoInventory("847043", 5, out status);
            ctrl.AddItemIntoInventory("860043", 1, out status);
            ctrl.AddItemIntoInventory("865043", 1, out status);
            ctrl.AddItemIntoInventory("857043", 10, out status);
            ctrl.AddItemIntoInventory("891043", 2, out status);
            ctrl.AddItemIntoInventory("856043", 6, out status);
            ctrl.AddItemIntoInventory("824334", 2, out status);
            ctrl.AddItemIntoInventory("880004", 1, out status);
             */ 
            /*
            ctrl.AddItemIntoInventory("806901", 9, out status);
            ctrl.AddItemIntoInventory("859210", 10, out status);
            ctrl.AddItemIntoInventory("859001", 11, out status);
            ctrl.AddItemIntoInventory("859101", 8, out status);
            ctrl.AddItemIntoInventory("822601", 7, out status);
            ctrl.AddItemIntoInventory("822801", 13, out status);
            ctrl.AddItemIntoInventory("822701", 8, out status);
            ctrl.AddItemIntoInventory("822143", 10, out status);
            ctrl.AddItemIntoInventory("822043", 9, out status);
            ctrl.AddItemIntoInventory("922409", 5, out status);
            ctrl.AddItemIntoInventory("922643", 18, out status);
            ctrl.AddItemIntoInventory("922843", 18, out status);
            ctrl.AddItemIntoInventory("922743", 18, out status);
            ctrl.AddItemIntoInventory("930209", 7, out status);
            ctrl.AddItemIntoInventory("922309", 10, out status);
            ctrl.AddItemIntoInventory("959201", 8, out status);
            ctrl.AddItemIntoInventory("959001", 10, out status);
            ctrl.AddItemIntoInventory("959101", 10, out status);
            ctrl.AddItemIntoInventory("933016", 11, out status);
            ctrl.AddItemIntoInventory("976901", 17, out status);
            ctrl.AddItemIntoInventory("933118", 30, out status);
            ctrl.AddItemIntoInventory("933026", 119, out status);
            ctrl.AddItemIntoInventory("932075", 15, out status);
            ctrl.AddItemIntoInventory("932087", 6, out status);
            ctrl.AddItemIntoInventory("895043", 5, out status);
            ctrl.AddItemIntoInventory("893043", 3, out status);
            ctrl.AddItemIntoInventory("894043", 3, out status);
            ctrl.AddItemIntoInventory("995043", 20, out status);
            ctrl.AddItemIntoInventory("996043", 10, out status);
            ctrl.AddItemIntoInventory("993043", 19, out status);
            ctrl.AddItemIntoInventory("994043", 20, out status);
            ctrl.AddItemIntoInventory("811018", 1, out status);
            ctrl.AddItemIntoInventory("812018", 1, out status);
            ctrl.AddItemIntoInventory("803529", 10, out status);
            ctrl.AddItemIntoInventory("867018", 1, out status);
            ctrl.AddItemIntoInventory("879043", 1, out status);
            ctrl.AddItemIntoInventory("848043", 2, out status);
            ctrl.AddItemIntoInventory("864043", 2, out status);
            ctrl.AddItemIntoInventory("865043", 4, out status);
            ctrl.AddItemIntoInventory("941043", 5, out status);
            ctrl.AddItemIntoInventory("978043", 6, out status);
            ctrl.AddItemIntoInventory("944043", 17, out status);
            ctrl.AddItemIntoInventory("958043", 10, out status);
            ctrl.AddItemIntoInventory("971043", 6, out status);
            ctrl.AddItemIntoInventory("957043", 10, out status);
            ctrl.AddItemIntoInventory("956043", 3, out status);
            ctrl.AddItemIntoInventory("967043", 8, out status);
            ctrl.AddItemIntoInventory("942043", 5, out status);
            ctrl.AddItemIntoInventory("974043", 13, out status);
            ctrl.AddItemIntoInventory("948043", 12, out status);
            ctrl.AddItemIntoInventory("947043", 10, out status);
            ctrl.AddItemIntoInventory("974143", 5, out status);
            ctrl.AddItemIntoInventory("965043", 10, out status);
            ctrl.AddItemIntoInventory("961043", 18, out status);
            ctrl.AddItemIntoInventory("962043", 3, out status);
            ctrl.AddItemIntoInventory("880004", 11, out status);
            ctrl.AddItemIntoInventory("881001", 12, out status);
            ctrl.AddItemIntoInventory("883001", 15, out status);
            ctrl.AddItemIntoInventory("980009", 4, out status);
            ctrl.AddItemIntoInventory("981009", 9, out status);
            ctrl.AddItemIntoInventory("983009", 7, out status);
            */
            
            
            /*
            InventoryController ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            ctrl.SetInventoryHeaderInfo("", "",
                "Transfer", 0.0M, 0.0, 0.0M);

            ctrl.AddItemIntoInventory("810018", 1, out status);
            ctrl.AddItemIntoInventory("810018", 1, out status);
            ctrl.AddItemIntoInventory("810018", 1, out status);
            ctrl.AddItemIntoInventory("839018", 1, out status);
            ctrl.AddItemIntoInventory("839018", 1, out status);
            ctrl.AddItemIntoInventory("846043", 1, out status);
            ctrl.AddItemIntoInventory("846043", 1, out status);
            ctrl.AddItemIntoInventory("846043", 1, out status);
            ctrl.AddItemIntoInventory("847043", 1, out status);
            ctrl.AddItemIntoInventory("847043", 1, out status);
            ctrl.AddItemIntoInventory("847043", 1, out status);
            ctrl.AddItemIntoInventory("847043", 1, out status);
            ctrl.AddItemIntoInventory("866043", 1, out status);
            ctrl.AddItemIntoInventory("866043", 1, out status);
            ctrl.AddItemIntoInventory("866043", 1, out status);
            ctrl.AddItemIntoInventory("868001", 5, out status);
            ctrl.AddItemIntoInventory("869001", 1, out status);
            ctrl.AddItemIntoInventory("869001", 4, out status);
            ctrl.AddItemIntoInventory("878043", 1, out status);
            ctrl.AddItemIntoInventory("878043", 1, out status);
            ctrl.AddItemIntoInventory("891043", 1, out status);
            ctrl.AddItemIntoInventory("891043", 1, out status);
            ctrl.AddItemIntoInventory("891099", 1, out status);
            ctrl.AddItemIntoInventory("892043", 1, out status);
            ctrl.AddItemIntoInventory("892043", 1, out status);
            ctrl.AddItemIntoInventory("892043", 1, out status);
            ctrl.AddItemIntoInventory("957043", 12, out status);
            ctrl.AddItemIntoInventory("958043", 14, out status);
            ctrl.AddItemIntoInventory("965043", 30, out status);

            bool c = ctrl.TransferOut("admin", 7, 13, out status);
            */
            /*
            InventoryController ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            ctrl.SetInventoryHeaderInfo("", "",
                "Transfer", 0.0M, 0.0, 0.0M);

            ctrl.AddItemIntoInventory("806909", 29, out status);
            ctrl.AddItemIntoInventory("931035", 2, out status);
            ctrl.AddItemIntoInventory("898018", 18, out status);
            ctrl.AddItemIntoInventory("931079", 90, out status);
            ctrl.AddItemIntoInventory("941004", 2, out status);
            ctrl.AddItemIntoInventory("973004", 2, out status);
            ctrl.AddItemIntoInventory("958009", 40, out status);
            ctrl.AddItemIntoInventory("957009", 25, out status);
            ctrl.AddItemIntoInventory("957004", 125, out status);
            ctrl.AddItemIntoInventory("956004", 126, out status);
            ctrl.AddItemIntoInventory("942004", 2, out status);

            bool c = ctrl.TransferOut("admin", 10, 7, out status);*/
            /*
            InventoryController ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            ctrl.SetInventoryHeaderInfo("", "",
                "Stock Take Adjustment", 0.0M, 0.0, 0.0M);

            ctrl.AddItemIntoInventory("803529", 18, out status);
            ctrl.AddItemIntoInventory("805743", 1, out status);
            ctrl.AddItemIntoInventory("811018", 2, out status);
            ctrl.AddItemIntoInventory("812018", 3, out status);
            ctrl.AddItemIntoInventory("813018", 2, out status);
            ctrl.AddItemIntoInventory("814018", 1, out status);
            ctrl.AddItemIntoInventory("818043", 1, out status);
            ctrl.AddItemIntoInventory("822601", 1, out status);
            ctrl.AddItemIntoInventory("824234", 3, out status);
            ctrl.AddItemIntoInventory("839018", 54, out status);
            ctrl.AddItemIntoInventory("840043", 1, out status);
            ctrl.AddItemIntoInventory("841043", 1, out status);
            ctrl.AddItemIntoInventory("841099", 1, out status);
            ctrl.AddItemIntoInventory("844043", 1, out status);
            ctrl.AddItemIntoInventory("845043", 3, out status);
            ctrl.AddItemIntoInventory("846043", 1, out status);
            ctrl.AddItemIntoInventory("847043", 1, out status);
            ctrl.AddItemIntoInventory("856043", 3, out status);
            ctrl.AddItemIntoInventory("857043", 1, out status);
            ctrl.AddItemIntoInventory("859001", 1, out status);
            ctrl.AddItemIntoInventory("862043", 2, out status);
            ctrl.AddItemIntoInventory("865043", 5, out status);
            ctrl.AddItemIntoInventory("868001", 6, out status);
            ctrl.AddItemIntoInventory("871043", 1, out status);
            ctrl.AddItemIntoInventory("872018", 20, out status);
            ctrl.AddItemIntoInventory("873018", 1, out status);
            ctrl.AddItemIntoInventory("874043", 7, out status);
            ctrl.AddItemIntoInventory("875143", 20, out status);
            ctrl.AddItemIntoInventory("876143", 1, out status);
            ctrl.AddItemIntoInventory("877043", 1, out status);
            ctrl.AddItemIntoInventory("878043", 2, out status);
            ctrl.AddItemIntoInventory("883001", 2, out status);
            ctrl.AddItemIntoInventory("883099", 1, out status);
            ctrl.AddItemIntoInventory("891043", 1, out status);
            ctrl.AddItemIntoInventory("893099", 1, out status);
            ctrl.AddItemIntoInventory("894043", 6, out status);
            ctrl.AddItemIntoInventory("895043", 2, out status);
            ctrl.AddItemIntoInventory("897018", 21, out status);
            ctrl.AddItemIntoInventory("898018", 2, out status);
            ctrl.AddItemIntoInventory("907404", 4, out status);
            ctrl.AddItemIntoInventory("907501", 242, out status);
            ctrl.AddItemIntoInventory("907604", 3, out status);
            ctrl.AddItemIntoInventory("916043", 2, out status);
            ctrl.AddItemIntoInventory("922409", 3, out status);
            ctrl.AddItemIntoInventory("922643", 4, out status);
            ctrl.AddItemIntoInventory("922743", 13, out status);
            ctrl.AddItemIntoInventory("931032", 2, out status);
            ctrl.AddItemIntoInventory("931035", 2, out status);
            ctrl.AddItemIntoInventory("931079", 609, out status);
            ctrl.AddItemIntoInventory("932083", 1, out status);
            ctrl.AddItemIntoInventory("932087", 1, out status);
            ctrl.AddItemIntoInventory("941004", 97, out status);
            ctrl.AddItemIntoInventory("941043", 6, out status);
            ctrl.AddItemIntoInventory("942004", 21, out status);
            ctrl.AddItemIntoInventory("944043", 2, out status);
            ctrl.AddItemIntoInventory("947009", 513, out status);
            ctrl.AddItemIntoInventory("947043", 12, out status);
            ctrl.AddItemIntoInventory("948009", 13, out status);
            ctrl.AddItemIntoInventory("948043", 9, out status);
            ctrl.AddItemIntoInventory("956004", 483, out status);
            ctrl.AddItemIntoInventory("956009", 1205, out status);
            ctrl.AddItemIntoInventory("957004", 456, out status);
            ctrl.AddItemIntoInventory("957009", 38, out status);
            ctrl.AddItemIntoInventory("959101", 117, out status);
            ctrl.AddItemIntoInventory("962009", 368, out status);
            ctrl.AddItemIntoInventory("962043", 7, out status);
            ctrl.AddItemIntoInventory("965004", 515, out status);
            ctrl.AddItemIntoInventory("967043", 7, out status);
            ctrl.AddItemIntoInventory("971043", 11, out status);
            ctrl.AddItemIntoInventory("973004", 300, out status);
            ctrl.AddItemIntoInventory("973043", 19, out status);
            ctrl.AddItemIntoInventory("974143", 12, out status);
            ctrl.AddItemIntoInventory("980009", 14, out status);
            ctrl.AddItemIntoInventory("981009", 16, out status);
            ctrl.AddItemIntoInventory("983009", 15, out status);
            ctrl.AddItemIntoInventory("993043", 5, out status);
            ctrl.AddItemIntoInventory("995043", 10, out status);

            ctrl.SetInventoryDate(new DateTime(2007, 8, 31));
            bool c = ctrl.StockOut("admin",4, 7, true, true, out status);
            MessageBox.Show(c.ToString() + " " + status);
            */
            /*
            InventoryController ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            ctrl.SetInventoryHeaderInfo("", "",
                "Stocks damaged when delivered to Spore", 0.0M, 0.0, 0.0M);

            ctrl.AddItemIntoInventory("817018", 1, out status);
            ctrl.AddItemIntoInventory("817018", 62, out status);
            ctrl.AddItemIntoInventory("817018", 91, out status);
            ctrl.AddItemIntoInventory("810018", 1, out status);
            ctrl.SetInventoryDate(new DateTime(2007, 11, 15));
            bool c = ctrl.StockOut("admin", 4, 7, false, true, out status);
            MessageBox.Show(c.ToString() + " " + status);
            */
            /*
            
            InventoryController ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            ctrl.SetInventoryHeaderInfo("", "",
                "Hamper for Women's Fair Stage Segment", 0.0M, 0.0, 0.0M);
            
            ctrl.AddItemIntoInventory("800029",	1, out status);
            ctrl.AddItemIntoInventory("858043",	1, out status);
            ctrl.AddItemIntoInventory("805743",	1, out status);
            ctrl.AddItemIntoInventory("818043",	1, out status);
            ctrl.AddItemIntoInventory("898018",	1, out status);
            ctrl.AddItemIntoInventory("897018",	1, out status);
            ctrl.AddItemIntoInventory("894043", 1, out status);
            ctrl.SetInventoryDate(new DateTime(2007, 11, 15));
            bool c = ctrl.StockOut("admin", 7, 7, false, true, out status);
            MessageBox.Show(c.ToString() + " " + status);


            ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            ctrl.SetInventoryHeaderInfo("", "", 
                "Sponsorship to Her World Women's Fair", 0.0M, 0.0, 0.0M);
            ctrl.AddItemIntoInventory("942004",	400	, out status);
            ctrl.AddItemIntoInventory("958009", 400, out status);            
            ctrl.SetInventoryDate(new DateTime(2007,11,15));
            c = ctrl.StockOut("admin", 7, 7, false, true, out status);
            MessageBox.Show(c.ToString() + " " + status);

            ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            ctrl.SetInventoryHeaderInfo("", "",
                "Stolen during Women's Fair Roadshow", 0.0M, 0.0, 0.0M);            
            ctrl.AddItemIntoInventory("891099",	1	, out status);
            ctrl.SetInventoryDate(new DateTime(2007, 11, 15));
            c = ctrl.StockOut("admin", 3, 7, false, true, out status);
            MessageBox.Show(c.ToString() + " " + status);

            ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            ctrl.SetInventoryHeaderInfo("", "",
                "taken by Mrs Yeo - Gift for someone", 0.0M, 0.0, 0.0M);            
            ctrl.AddItemIntoInventory("897018",	1	, out status);
            ctrl.SetInventoryDate(new DateTime(2007, 11, 15));
            c = ctrl.StockOut("admin", 12, 7, false, true, out status);
            MessageBox.Show(c.ToString() + " " + status);

            ctrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            ctrl.SetInventoryHeaderInfo("", "",
                "Prizes for stage segment", 0.0M, 0.0, 0.0M);
            ctrl.AddItemIntoInventory("933026", 1, out status);
            ctrl.AddItemIntoInventory("803529", 1, out status);
            ctrl.SetInventoryDate(new DateTime(2007, 11, 15));
            c = ctrl.StockOut("admin", 7,13, false, true, out status);
            */
            /* Transfer
            
            ctrl.AddItemIntoInventory("956004", 36, out status);
            ctrl.AddItemIntoInventory("956004", 18, out status);
            ctrl.AddItemIntoInventory("956004", 31, out status);
            ctrl.AddItemIntoInventory("956004", 40, out status);

            ctrl.AddItemIntoInventory("957004", 36, out status);
            ctrl.AddItemIntoInventory("957004", 18, out status);
            ctrl.AddItemIntoInventory("957004", 31, out status);
            ctrl.AddItemIntoInventory("957004", 40, out status);

            ctrl.AddItemIntoInventory("957009", 18, out status);
            
            ctrl.AddItemIntoInventory("956009", 18, out status);
            
            ctrl.AddItemIntoInventory("958009", 18, out status);                        
            ctrl.AddItemIntoInventory("958009", 31, out status);                        
            ctrl.AddItemIntoInventory("958009", 40, out status);
            ctrl.AddItemIntoInventory("958009", 36, out status);
            ctrl.AddItemIntoInventory("958009", 18, out status);
                        
            ctrl.AddItemIntoInventory("822109", 29, out status);
            ctrl.AddItemIntoInventory("822009", 29, out status);
            ctrl.AddItemIntoInventory("907404", 29, out status);
             
            bool c =  ctrl.TransferOut("admin", 7, 10, out status);
             */



            /* --STOCK IN
            ctrl.SetInventoryHeaderInfo("", "", "Adjustment for Stock Out error after stock take 01 Sep 2007", 0, 1, 0);
            ctrl.AddItemIntoInventory("881001", 2, 15.4M, out status);
            ctrl.AddItemIntoInventory("894043",1,9.8338M,out status);
            ctrl.AddItemIntoInventory("895043",1,7.7756M,out status);
            ctrl.AddItemIntoInventory("931035",1,14.562M,out status);
            ctrl.AddItemIntoInventory("931050",1,20.4803M,out status);
            ctrl.AddItemIntoInventory("976901",1,29.0462M,out status);
            ctrl.AddItemIntoInventory("897018",2,0,out status);
            ctrl.AddItemIntoInventory("916043",2,0,out status);
            ctrl.AddItemIntoInventory("922643",1,0,out status);
            ctrl.AddItemIntoInventory("922743",2,0,out status);
            ctrl.AddItemIntoInventory("941004",2,0,out status);
            ctrl.AddItemIntoInventory("941043",1,0,out status);
            ctrl.AddItemIntoInventory("944043",2,0,out status);
            ctrl.AddItemIntoInventory("948043",2,0,out status);
            ctrl.AddItemIntoInventory("959101",4,0,out status);
            ctrl.AddItemIntoInventory("962043",2,0,out status);
            ctrl.AddItemIntoInventory("971043",1,0,out status);
            ctrl.AddItemIntoInventory("973043",4,0,out status);
            ctrl.AddItemIntoInventory("974143",1,0,out status);
            ctrl.AddItemIntoInventory("980009",2,0,out status);
            ctrl.AddItemIntoInventory("981009",2,0,out status);
            ctrl.AddItemIntoInventory("983009",3,0,out status);
            ctrl.AddItemIntoInventory("993043",1,0,out status);
            ctrl.AddItemIntoInventory("995043", 2, 0, out status);
            bool c = ctrl.StockIn("SYSTEM", 7, false, false, out status);
             * */