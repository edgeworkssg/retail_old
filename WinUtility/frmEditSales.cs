using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
namespace WinUtility
{
    public partial class frmEditSales : Form
    {
        public frmEditSales()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            MembershipCollection mbr = new MembershipCollection();
            mbr.Load();
            for (int i = 0; i < mbr.Count; i++)
            {
                mbr[i].MembershipNo = MembershipController.getNewMembershipNo(1);
                mbr[i].Save();
            }
            MessageBox.Show("DONNE!");
            return;
            */
            DataSet ds = new DataSet();
            ds.ReadXml("d:\\test\\member9.xml");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)            
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    ds.Tables[0].Rows[i][j] = ds.Tables[0].Rows[i][j].ToString().Replace('"', '\'');
                    ds.Tables[0].Rows[i][j] = ds.Tables[0].Rows[i][j].ToString().Replace("\n", " ");
                }
            }
            /*
            TmpdatumCollection t = new TmpdatumCollection();
            t.Load(ds.Tables[0]);
            for (int i = 0; i < t.Count; i++)
            {
                t[i].IsNew = true;
                t[i].Save();
            }*/
            ExportController.ExportToCSV(ds.Tables[0], "d:\\test\\member9.csv");
            /*

            string status = "";
            VoucherController.CreateBatchVoucherNo(712160, 714159, "", "", 10, 6, 
                new DateTime(2008, 11, 1), new DateTime(2018, 11, 1), out status);
            MessageBox.Show("Done! > " + status);            
             */ 
        }
    }
}
