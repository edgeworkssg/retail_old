using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.OrderForms
{
    public partial class frmEditReason : Form
    {
        DataSet ds = new DataSet();
        int maxReasonID = 0;
        public frmEditReason()
        {
            InitializeComponent();
        }

        private void LoadCashRecordingReasons()
        {
            //string status = "";
            try
            {
                ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\CashRecordingReason.xml");
                if (ds.Tables.Count == 0)
                {
                    DataTable dt = new DataTable("Reason");
                    dt.Columns.Add("ID");
                    dt.Columns.Add("Name");
                    ds.Tables.Add(dt);
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (int.Parse(dr[0].ToString()) > maxReasonID)
                        maxReasonID = int.Parse(dr[0].ToString());
                }
               
                dgvReason.DataSource = ds.Tables[0];
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); }
        }

        private void frmEditReason_Load(object sender, EventArgs e)
        {
            LoadCashRecordingReasons();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmKeyboard fKey = new frmKeyboard();
            fKey.textMessage = "Please Input Reason";
            DialogResult dr = fKey.ShowDialog();
            if (dr == DialogResult.OK)
            {
                DataRow d = ds.Tables[0].NewRow();
                d[0] = maxReasonID + 1;
                d[1] = fKey.value;
                ds.Tables[0].Rows.Add(d);
                maxReasonID++;
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int ct = dgvReason.SelectedCells.Count;
            if (ct > 0)
            {
                int rowno = dgvReason.SelectedCells[0].RowIndex;
                if (rowno >= 0)
                {
                    string ReasonName = (string)dgvReason.Rows[rowno].Cells[1].Value;
                    string ID = (string)dgvReason.Rows[rowno].Cells[0].Value;
                    DialogResult dr = MessageBox.Show("Are you sure want to delete Reason : " + ReasonName + "? ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        foreach (DataRow d in ds.Tables[0].Rows)
                        {
                            if (d[0] == ID)
                            {
                                ds.Tables[0].Rows.Remove(d);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ds.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "\\CashRecordingReason.xml");
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
