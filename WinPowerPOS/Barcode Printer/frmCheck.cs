using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using BarcodePrinter;

namespace WinPowerPOS.BarcodePrinter
{
    public partial class frmCheck : Form
    {
        public frmCheck()
        {
            InitializeComponent();
        }

        private void frmCheck_Load(object sender, EventArgs e)
        {
            string SQLString = "Select ItemNo, Barcode From Item Where Deleted = 0 and CategoryName <> 'SYSTEM'";
            DataTable dt = new DataTable();
            dt.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(SQLString)));
            dt.Columns.Add("EncodedDataLength");
            Code123Auto code = new Code123Auto();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["EncodedDataLength"] = code.EncodeCode128Auto(dt.Rows[i]["Barcode"].ToString()).Length;
            }
            dgv.DataSource = dt;
        }
    }
}
