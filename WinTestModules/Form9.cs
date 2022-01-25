using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;
using System.Transactions;

namespace WinTestModules
{
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DataTable dt = ReportController.FetchStockCardReport(new DateTime(2007, 9, 1, 13, 4, 0), new DateTime(2007, 9, 1, 13, 4, 0), 0, "%", "%");           
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();

            /*
            OrderHdrCollection oHdr = new OrderHdrCollection();
            oHdr.Where(OrderHdr.Columns.IsVoided, true);
            oHdr.Where(OrderHdr.Columns.OrderDate, Comparison.GreaterOrEquals, new DateTime(2007, 09, 01, 13, 00, 00));
            oHdr.Where(OrderHdr.Columns.OrderDate, Comparison.LessOrEquals, dateTimePicker2.Value);
            oHdr.Load();
            */
            /*
            ViewErrornousSalesStockOutCollection v = new ViewErrornousSalesStockOutCollection(); 
            v.Where(ViewErrornousSalesStockOut.Columns.InventoryDate, Comparison.GreaterOrEquals, new DateTime(2007,09,01, 13,00,00));
            v.Where(ViewErrornousSalesStockOut.Columns.InventoryDate, Comparison.LessOrEquals, dateTimePicker2.Value );
            v.Load();
            */
            /*
            OrderDetCollection v = new OrderDetCollection();
            v.Where(OrderDet.Columns.IsVoided, true);
            v.Where(OrderDet.Columns.OrderDetDate, Comparison.GreaterOrEquals, new DateTime(2007, 09, 01, 13, 00, 00));
            v.Where(OrderDet.Columns.OrderDetDate, Comparison.LessOrEquals, dateTimePicker2.Value);
            v.Load();

                using (TransactionScope ts = new TransactionScope())
                {
            
                    for (int i = 0; i < v.Count; i++)
                    {
                        InventoryController.UndoStockOut(v[i].InventoryHdrRefNo);
                    }
                    ts.Complete();
                }
            */
            MessageBox.Show("Done");
        }
    }
}