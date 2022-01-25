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
    public partial class CheckWrongStockOutQty : Form
    {
        public CheckWrongStockOutQty()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            DataSet ds = SPs.FetchStockOutThatIsNotSales().GetDataSet();
            InventoryHdrCollection myHdr = new InventoryHdrCollection();
            myHdr.Load(ds.Tables[0]);
            InventoryDetCollection myDets;
            for (int i = 0; i < myHdr.Count; i++)
            {
                myDets = myHdr[i].InventoryDetRecords();
                for (int j=0;j <myDets.Count;j++) 
                {
                    InventoryController.UndoStockOut(
                        myHdr[i].InventoryHdrRefNo,
                        myDets[j].ItemNo, myDets[j].Quantity.GetValueOrDefault(0));
                }
                if (myDets.Count == 0)
                {
                    InventoryHdr.Delete("InventoryHdrRefNo", myHdr[i].InventoryHdrRefNo);
                }
                else
                {
                    Logger.writeLog(myHdr[i].InventoryHdrRefNo);
                }                
            }
        }
    }
}