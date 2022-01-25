using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;

namespace PowerEdge
{
    public partial class frmUtilityBreakName : Form
    {
        public frmUtilityBreakName()
        {
            InitializeComponent();
        }

        private void frmUtilityBreakName_Load(object sender, EventArgs e)
        {
            Query qr = Membership.CreateQuery();
            qr.SelectList = "NameToAppear";
            DataTable dt = qr.ExecuteDataSet().Tables[0];
            DataTable dtBreak = new DataTable();
            dtBreak.Columns.Add("Name");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string[] name = dt.Rows[i]["NameToAppear"].ToString().Split(' ');
                for (int j = 0; j < name.Length; j++)
                {
                    DataRow dr = dtBreak.NewRow();
                    dr["Name"] = name[j].ToString();
                    dtBreak.Rows.Add(dr);
                }
            }
            ExportController.ExportToCSV(dtBreak, "D:\\test\\AHAVANames.csv");
            MessageBox.Show("Done");
        }
    }
}
