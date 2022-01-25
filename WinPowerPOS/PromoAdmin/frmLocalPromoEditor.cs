using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;
namespace WinPowerPOS.PromoAdmin
{
    public partial class frmLocalPromoEditor : Form
    {
        public frmLocalPromoEditor()
        {
            InitializeComponent();
        }

        private void frmLocalPromoEditor_Load(object sender, EventArgs e)
        {
            dgvPromo.AutoGenerateColumns = false;
            dgvPromo.DataSource =
                PromotionAdminController.FetchAllPromoByDate(DateTime.Now);
            dgvPromo.Refresh();
        }

        private void dgvPromo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bool IsEnabled;

            if (e.ColumnIndex == 5) //Mark as IsSpecial.... 
            {
                if (!bool.TryParse(dgvPromo.Rows[e.RowIndex].Cells["Enabled"].Value.ToString(), out IsEnabled))
                {
                    return;
                }
                try
                {
                    int PromoLineID = int.Parse(dgvPromo.Rows[e.RowIndex].Cells[0].Value.ToString());
                    dgvPromo.Rows[e.RowIndex].Cells[5].Value = !IsEnabled;

                    PromotionAdminController.EnablePromo(PromoLineID, !IsEnabled);
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    dgvPromo.Rows[e.RowIndex].Cells[5].Value = IsEnabled;
                    MessageBox.Show("Error Encountered. Please try again!");
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /*
                private void dgvPromo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
                {
                    try
                    {
                        if (bool.Parse(dgvPromo.Rows[e.RowIndex].Cells["IsVoided"].Value.ToString()) == true)
                        {
                            for (int i = 0; i < dgvPromo.ColumnCount; i++)
                            {
                                if ((dgvPurchase.Columns[i].Visible))
                                {
                                    dgvPurchase.Rows[e.RowIndex].Cells[i].Style.ForeColor = System.Drawing.Color.White;
                                    dgvPurchase.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.DarkRed;
                                }
                            }
                        }             
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.");
                    }
                }
    
         */
    }
}
