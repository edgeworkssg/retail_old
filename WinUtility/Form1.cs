using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;

namespace WinUtility
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            dataGridView1.DataSource =
            ReportController.FetchStockReportBreakdownByLocation("", false, "", "", "");
            dataGridView1.Refresh();
            return;
            */
            //ReportController.FetchStockReport("",
            /*
            dataGridView1.DataSource = ReportController.FetchProductSalesReportByDate
                (new DateTime(2010, 1, 1), new DateTime(2011, 12, 1), "ALL","");
            dataGridView1.Refresh();
            return;*/
            string SQL = "select orderdate,isnull(nullif(b.userfld1,''),c.salespersonid) as salespersonid,  " +
                            "b.OrderHdrID as orderhdrid, sum(b.amount) as amount " +
                            "from orderhdr a " +
                            "inner join orderdet b " +
                            "on a.orderhdrid = b.orderhdrid " +
                            "inner join SalesCommissionRecord c " +
                            "on a.OrderHdrID = c.OrderHdrID " +
                            "inner join item d " +
                            "on b.itemno = d.itemno " +
                            "where OrderDate > '2011-6-1' and orderdate < '2011-7-1 00:00' " +
                            "and a.isvoided=0 and b.isvoided=0 and isCommission=1 " +
                            "and IsInInventory = 1 " + 
                            "group by orderdate,isnull(nullif(b.userfld1,''),c.salespersonid),b.orderhdrid order by orderdate asc";
            dataGridView1.DataSource = ReportController.Pivot(DataService.GetReader(new QueryCommand(SQL, "PowerPOS")), "OrderHdrId", "SalesPersonID", "Amount");
            dataGridView1.Refresh();


            ExportController.ExportToCSV(dataGridView1, "c:\\tmp\\Product" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
            //dataGridView1.DataSource = ReportController.FetchCustomerCreditReport();
            //dataGridView1.Refresh();
            /*
            dataGridView1.DataSource = ReportController.FetchInventoryStockOutReportPivotByReason
                (dateTimePicker1.Value, dateTimePicker2.Value,
                txtLocation.Text, txtReason.Text, txtSearch.Text);
            dataGridView1.Refresh();
            */

            /*
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();

            ArrayList extraField = new ArrayList();
            extraField.Add("CategoryName");
            extraField.Add("ItemName");
            Hashtable whereList = new Hashtable();
            whereList.Add("CategoryName", "Body Care");            
                ReportController r = new ReportController();
                DataTable dt = r.FetchDynamicSalesReport(dateTimePicker1.Value, 
                dateTimePicker2.Value, extraField, textBox1.Text, whereList);


            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
            */
            /*
            string duplicate;
            if (!MembershipController.IsNRICAlreadyExist(textBox1.Text, out duplicate))
            {
                MessageBox.Show("Dont Exist!");
            }
            else
            {
                MessageBox.Show("It is a duplicate. " + duplicate);
            }
            */
            /*           
            dataGridView1.DataSource = UtilityController.FetchCounterClosingReport(true, true, 
                dateTimePicker1.Value, dateTimePicker2.Value, "", "", "", 0, "%", "%", "0", "", "");
            
            dataGridView1.Refresh();
              */
        }

    }
}