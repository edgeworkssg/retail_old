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
    public partial class TallyQty : Form
    {
        public TallyQty()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ItemCollection itctrl = new ItemCollection();
            itctrl.Where(Item.Columns.IsInInventory, true);
            itctrl.Load();

            DataTable dt = new DataTable("TEST");
            dt.Columns.Add("ItemNo");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Sales");
            dt.Columns.Add("Stock Out");
            dt.Columns.Add("defi");
            DataRow dr; 
            for (int i = 0; i < itctrl.Count; i++)
            {
                dr = dt.NewRow();
                dr["ItemNo"] = itctrl[i].ItemNo;
                dr["ItemName"] = itctrl[i].ItemName;

                DataTable tmp = ReportController.FetchProductSalesReport(
                        dateTimePicker1.Value, dateTimePicker2.Value, itctrl[i].ItemName
                        , "ALL",
                        "ALL", "%", "%", false, "ItemNo", "ASC");
                                
                if (tmp.Rows.Count > 0)
                {
                    dr["SALES"] = tmp.Rows[0]["TotalQuantity"].ToString();

                    /*
                    Query qr1 = ViewSalesDetail.CreateQuery();
                    qr1.AddWhere(ViewSalesDetail.Columns.OrderDate, Comparison.GreaterOrEquals, dateTimePicker1.Value);
                    qr1.AddWhere(ViewSalesDetail.Columns.OrderDate, Comparison.LessOrEquals, dateTimePicker2.Value);
                    qr1.AddWhere(ViewSalesDetail.Columns.ItemNo, itctrl[i].ItemNo);
                    qr1.QueryType = QueryType.Select;
                    */
                    int a, b;
                    a = 0; b = 0;
                    int.TryParse(tmp.Rows[0]["TotalQuantity"].ToString(), out a);
                    //DataSet ds1 = qr1.ExecuteDataSet();                    
                    //int.TryParse(ds1.Tables[0].Rows[0]["TotalQuantity"].ToString(), out a);

                    Query qr = new Query("ViewInventoryActivityByItemNo");
                    qr.AddWhere(ViewInventoryActivityByItemNo.Columns.InventoryDate, Comparison.GreaterOrEquals, dateTimePicker1.Value);
                    qr.AddWhere(ViewInventoryActivityByItemNo.Columns.InventoryDate, Comparison.LessOrEquals, dateTimePicker2.Value);
                    qr.AddWhere(ViewInventoryActivityByItemNo.Columns.ItemNo, itctrl[i].ItemNo);
                    qr.AddWhere(ViewInventoryActivityByItemNo.Columns.MovementType, "Stock Out");
                    qr.AddWhere(ViewInventoryActivityByItemNo.Columns.StockOutReasonID, 0);
                    qr.QueryType = QueryType.Select;

                    DataSet ds = qr.ExecuteDataSet();
                    dr["Stock Out"] = ds.Tables[0].Compute("SUM(Quantity)", "").ToString();
                    int.TryParse(ds.Tables[0].Compute("SUM(Quantity)", "").ToString(), out b);
                    dr["defi"] = (a - b).ToString();
                    //if ((a-b) > 0)
                        dt.Rows.Add(dr);
                }
            }
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
        }
    }
}