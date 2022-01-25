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
using PowerPOS.Container;

namespace WinPowerPOS.WarrantyForms
{
    public partial class frmCreateWarranty : Form
    {
        public frmCreateWarranty()
        {
            InitializeComponent();
        }
        private int selectedIndex;
        public string orderHdrID;
        public string membershipNo;
        public bool IsSuccessful;
        public bool IsReadOnly;
        private WarrantyCollection warCol;

        private void frmCreateWarranty_Load(object sender, EventArgs e)
        {
            
            selectedIndex = -1;
            dgvWarranty.AutoGenerateColumns = false;
            dtpDateOfPurchase.Value = DateTime.Today;
            dtpDateOfExpiry.Value = DateTime.Today.AddYears(1);

            //Load Items from OrderHDR
            LoadWarranty();
            //Create Warranty Collection
            BindGrid();

            if (dgvWarranty.Rows.Count > 0)
            {
                dgvWarranty.Rows[0].Selected = true;
                dgvWarranty_CellClick(this, new DataGridViewCellEventArgs(0, 0));
            }

            if (IsReadOnly)
            {
                txtItemName.ReadOnly = true;
                txtSerialNo.ReadOnly = true;
                txtModelNo.ReadOnly = true;
                txtProductIdentificationNo.ReadOnly = true;
                dtpDateOfExpiry.Enabled = false;
                dtpDateOfPurchase.Enabled = false;
                btnDone.Enabled = false;
                btnOK.Enabled = false;
            }
        }

        private void BindGrid()
        {
            if (warCol != null)
            {
                dgvWarranty.DataSource = warCol.ToDataTable();
                dgvWarranty.Refresh();
            }
        }

        private void LoadWarranty()
        {
            //check if warranty information already existing in the system
            Query qr = Warranty.CreateQuery();
            int count = qr.WHERE(Warranty.Columns.OrderDetId, Comparison.Like, orderHdrID + "%").GetCount(Warranty.Columns.SerialNo);
            if (count > 0)
            {
                //if yes - load
                warCol = new WarrantyCollection();
                warCol.Where(Warranty.Columns.OrderDetId, Comparison.Like, orderHdrID + "%");
                warCol.Load();
            }
            else
            {
                //if no- insert a new one
                //Load Transaction Detail
                OrderDetCollection tmp = new OrderDetCollection();
                tmp.Where(OrderDet.Columns.OrderHdrID, orderHdrID);
                tmp.Load();

                warCol = new WarrantyCollection();
                Warranty tmpWar;
                //For every warranty item and each quantity, 
                //create warranty Collection with blank....
                for (int i = 0; i < tmp.Count; i++)
                {
                    if (tmp[i].IsVoided == false &&
                        tmp[i].Item.HasWarranty.HasValue &&
                        tmp[i].Item.HasWarranty.Value == true)
                    {
                        //repeat by quantity
                        for (int j = 0; j < tmp[i].Quantity; j++)
                        {
                            tmpWar = new Warranty();
                            tmpWar.SerialNo = "";
                            tmpWar.ItemNo = tmp[i].ItemNo;
                            tmpWar.DateOfPurchase = tmp[i].OrderDetDate.Date;
                            tmpWar.DateExpiry = tmp[i].OrderDetDate.Date.AddYears(1);
                            tmpWar.OrderDetId = tmp[i].OrderDetID;
                            tmpWar.ProductIdentification = "";
                            tmpWar.MembershipNo = membershipNo;                            
                            warCol.Add(tmpWar);
                        }
                    }
                }
            }
        }
        
        private void dgvWarranty_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //populate the value to the text box
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvWarranty.Rows[e.RowIndex].Cells["SerialNo"].Value != null)
                    txtSerialNo.Text = dgvWarranty.Rows[e.RowIndex].Cells["SerialNo"].Value.ToString();

                if (dgvWarranty.Rows[e.RowIndex].Cells["ItemNo"].Value != null)
                    txtItemName.Text = dgvWarranty.Rows[e.RowIndex].Cells["ItemNo"].Value.ToString();

                if (dgvWarranty.Rows[e.RowIndex].Cells["ModelNo"].Value != null)
                    txtModelNo.Text = dgvWarranty.Rows[e.RowIndex].Cells["ModelNo"].Value.ToString();

                if (dgvWarranty.Rows[e.RowIndex].Cells["ProductIdentificationNo"].Value != null) 
                    txtProductIdentificationNo.Text = dgvWarranty.Rows[e.RowIndex].Cells["ProductIdentificationNo"].Value.ToString();

                if (dgvWarranty.Rows[e.RowIndex].Cells["DateOfPurchase"].Value != null &&
                    dgvWarranty.Rows[e.RowIndex].Cells["DateOfPurchase"].Value is DateTime)
                    dtpDateOfPurchase.Value = (DateTime)dgvWarranty.Rows[e.RowIndex].Cells["DateOfPurchase"].Value;

                if (dgvWarranty.Rows[e.RowIndex].Cells["DateOfExpiry"].Value != null &&
                    dgvWarranty.Rows[e.RowIndex].Cells["DateOfExpiry"].Value is DateTime) 
                    dtpDateOfExpiry.Value = (DateTime)dgvWarranty.Rows[e.RowIndex].Cells["DateOfExpiry"].Value;

                selectedIndex = e.RowIndex;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int prevSelectedIndex;
            if (selectedIndex != -1)
            {
                prevSelectedIndex = selectedIndex;
                warCol[selectedIndex].SerialNo = txtSerialNo.Text;
                warCol[selectedIndex].ProductIdentification = txtProductIdentificationNo.Text;
                warCol[selectedIndex].ModelNo = txtModelNo.Text;
                warCol[selectedIndex].DateOfPurchase = dtpDateOfPurchase.Value;
                warCol[selectedIndex].DateExpiry = dtpDateOfExpiry.Value;                
                BindGrid();
                if (prevSelectedIndex + 1 < dgvWarranty.Rows.Count)
                {                    
                    dgvWarranty.Rows[prevSelectedIndex + 1].Selected = true;
                    dgvWarranty_CellClick(this, new DataGridViewCellEventArgs(0, prevSelectedIndex + 1));
                }
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                //check if all serial number has been assigned....
                for (int i = 0; i < warCol.Count; i++)
                {
                    if (warCol[i].SerialNo == "")
                    {
                        MessageBox.Show("Serial number for item " + warCol[i].ItemNo + " has not been specified");
                        return;
                    }
                }

                //save all- catch PK exception
                warCol.SaveAll();
                MessageBox.Show("Save successful");
                IsSuccessful = true;
                this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("PK_"))
                {
                    MessageBox.Show("One of the serial number has already been specified before");
                    return;
                }
                else
                {
                    MessageBox.Show("Unknown error has been encountered. Please try again.");
                    Logger.writeLog(ex);
                    return;
                }
            }
        } 
    }
}
