using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS.Container;
using WinPowerPOS.OrderForms;


namespace PowerPOS
{
    public partial class frmEditPromoItem : Form
    {
        public POSController pos;
        public int PromoHdrID;
        public bool isSuccessful;
        DataTable dtPromo;
        public frmEditPromoItem()
        {
            InitializeComponent();
            //this.pos = pos;
            
        }

        private void frmLineInfo_Load(object sender, EventArgs e)
        {
            isSuccessful = false;
            PromoCampaignHdr promo = new PromoCampaignHdr(PromoCampaignHdr.Columns.PromoCampaignHdrID, PromoHdrID);
            if (promo != null && promo.PromoCampaignName != "")
            {
                //load the details
                string status = "";
                lblPromoName.Text = promo.PromoCampaignName;
                dtPromo = pos.FetchUnSavedOrderItemsForPromo(PromoHdrID, out status); 
                BindGrid();
            }

            
        }

        private void BindGrid()
        {
            string status = "";
            dgvPurchase.AutoGenerateColumns = false;
            dgvPurchase.DataSource = dtPromo;
            dgvPurchase.Refresh();
            if (dgvPurchase.Rows.Count > 0)
                dgvPurchase.Rows[0].Selected = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isSuccessful = false;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in dtPromo.Rows)
            {
                OrderDet od = (OrderDet)pos.myOrderDet.Find(dr["ID"].ToString());
                if (od != null)
                {
                    decimal tmp = 0;
                    if (decimal.TryParse(dr["Quantity"].ToString(), out tmp))
                    {
                        if (tmp == 0)
                        {
                            od.IsVoided = true;
                        }
                        else
                        {
                            od.Quantity = tmp;
                        }
                    }
                    bool LineisVoided= false;
                    if (bool.TryParse(dr["IsVoided"].ToString(), out LineisVoided))
                    {
                        if (!od.IsVoided)
                            od.IsVoided = LineisVoided;

                    }
                }
            }
            isSuccessful = true;
            this.Close();
        }
        public const int QTY_COL = 3;
        public const int PLUS_COL = 5;
        public const int MIN_COL = 6;
        public const int VOID_COL = 7;

        private void dgvPurchase_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string status = "";
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == QTY_COL)
                {
                    string IDTemp = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();

                    frmKeypad fKeypad = new frmKeypad();
                    fKeypad.initialValue = dgvPurchase.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
                    fKeypad.textMessage = "Please enter Quantity";
                    fKeypad.ShowDialog();
                    if (fKeypad.value != "")
                    {
                        decimal tmp = 0;
                        if (decimal.TryParse(fKeypad.value, out tmp))
                        {
                            foreach (DataRow dr in dtPromo.Rows)
                            {
                                if (dr["ID"].ToString() == IDTemp)
                                {
                                    dr["Quantity"] = tmp.ToString("N0");
                                }

                            }
                        }
                    }

                }
                if (e.ColumnIndex == PLUS_COL)
                {
                    string IDTemp = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    foreach (DataRow dr in dtPromo.Rows)
                    {
                        if (dr["ID"].ToString() == IDTemp)
                        {
                            decimal tmp = 0;
                            if (decimal.TryParse(dr["Quantity"].ToString(), out tmp))
                            {
                                tmp++;
                                dr["Quantity"] = tmp.ToString("N0");
                            }
                        }
                    }
                }
                else if (e.ColumnIndex == MIN_COL)
                {
                    string IDTemp = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    foreach (DataRow dr in dtPromo.Rows)
                    {
                        if (dr["ID"].ToString() == IDTemp)
                        {
                            decimal tmp = 0;
                            if (decimal.TryParse(dr["Quantity"].ToString(), out tmp))
                            {
                                if (tmp > 0)
                                    tmp--;
                                dr["Quantity"] = tmp.ToString("N0");
                            }
                        }
                    }
                }
                else if (e.ColumnIndex == VOID_COL)
                {
                    string IDTemp = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    foreach (DataRow dr in dtPromo.Rows)
                    {
                        if (dr["ID"].ToString() == IDTemp)
                        {
                            bool res = false;
                            if (bool.TryParse(dr["IsVoided"].ToString(), out res))
                            {
                                dr["IsVoided"] = !res;
                                BindGrid();
                            } 
                        }
                    }
                    
                }
            }
        }

        private void dgvPurchase_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            
        }

        private void dgvPurchase_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string idTemp = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
            foreach (DataRow dr in dtPromo.Rows)
            {
                if (dr["ID"].ToString() == idTemp)
                {
                    bool isvoided = false;
                    if (bool.TryParse(dr["IsVoided"].ToString(), out isvoided) && isvoided)
                    {
                        Font f = dgvPurchase.DefaultCellStyle.Font;
                        for (int i = 0; i < dgvPurchase.ColumnCount; i++)
                        {
                            if ((dgvPurchase.Columns[i].Visible))
                            {
                                dgvPurchase.Rows[e.RowIndex].Cells[i].Style.ForeColor = System.Drawing.Color.White;
                                dgvPurchase.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.DarkRed;
                                dgvPurchase.Rows[e.RowIndex].Cells[i].Style.Font = new Font(f, FontStyle.Strikeout);
                            }
                        }
                    }
                    else
                    {
                        Font f = dgvPurchase.DefaultCellStyle.Font;
                        for (int i = 0; i < dgvPurchase.ColumnCount; i++)
                        {
                            if ((dgvPurchase.Columns[i].Visible))
                            {
                                dgvPurchase.Rows[e.RowIndex].Cells[i].Style.ForeColor = System.Drawing.Color.Black;
                                dgvPurchase.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                dgvPurchase.Rows[e.RowIndex].Cells[i].Style.Font = new Font(f,FontStyle.Regular);
                            }
                        }
                    }
                }
            }
        }
    }
}
