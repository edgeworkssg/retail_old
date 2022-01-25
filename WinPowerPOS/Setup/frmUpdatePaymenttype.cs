using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using System.Collections;
using POSDevices;
using WinPowerPOS.Reports;
using System.Configuration;
using Features = PowerPOS.Feature;
using System.Xml;

namespace WinPowerPOS.OrderForms
{
    public partial class frmUpdatePaymenttype : Form
    {
        public bool IsSuccessful;
        DataSet ds = new DataSet();
        Int32 iRowIndex = 0;
        DataTable dtRows = new DataTable();
        public frmUpdatePaymenttype()
        {
            InitializeComponent();
           // fillType();
        }
        private void fillType()
        {
            try
            {
                string status;
                PaymentTypesController.LoadPaymentTypes(out status);
                dgvPaymenttype.DataSource = PointOfSaleInfo.PaymentTypes;
            }
            catch (Exception Ex)
            {
                //MessageBox.Show(Ex.Message.ToString()); 
            }
        }

        private void frmUpdatePaymenttype_Load(object sender, EventArgs e)
        {
            AddDataTable();
            fillType();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // For Closing the Form
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
               
                // ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\PaymentTypes.xml");
                //Data Table For Getting all the Rows of Grid
                DataTable dtAllUpDatedRow = (DataTable)dgvPaymenttype.DataSource;
                // ID Column Increaser Variable
                Int32 ICount = 1;
                // Loop for Reading the Grid Rows
                
                    foreach (DataRow dr in dtAllUpDatedRow.Rows)
                    {
                        if (dr["Name"].ToString() != String.Empty)
                        {
                            DataRow dt = dtRows.NewRow();

                            dt["ID"] = ICount++;
                            dt["Name"] = dr["Name"];
                         
                           
                             dtRows.Rows.Add(dt);
                           
                        }
                    }
                    // XML File Writting
                    dtRows.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "\\PaymentTypes.xml", XmlWriteMode.WriteSchema);
                    this.dgvPaymenttype.AllowUserToAddRows = false;
                    MessageBox.Show("Data has been saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.dgvPaymenttype.Rows[dgvPaymenttype.Rows.Count - 1].Cells[0].Value = dgvPaymenttype.Rows.Count;
                    btnType.Enabled = true;
                    btnSave.Enabled = false;
                    //this.Close();
                    //if (dgvPaymenttype.Rows.Count > 0)

                    //{
                    //    for (int b = 0; b < dgvPaymenttype.Rows.Count; b++)
                    //    {
                    //        //if (dgvPaymenttype.Rows[b].Cells[0].Value.ToString() == ds.Tables[0].Rows[b]["ID"].ToString())
                    //        //{
                    //            ds.Tables[0].Rows[b]["ID"] = dgvPaymenttype.Rows[b].Cells[0].Value.ToString();
                    //            ds.Tables[0].Rows[b]["Name"] = dgvPaymenttype.Rows[b].Cells[1].Value.ToString();
                    //       // }
                    //    }

                    //    ds.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "\\PaymentTypes.xml", XmlWriteMode.WriteSchema);
                    //    fillType();
                    //    MessageBox.Show("Data has been Saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                }
            
            
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString());
            }
        
        }

        private void frmUpdatePaymenttype_Load_1(object sender, EventArgs e)
        {
            // Funtion for Adding Columns into the Data table
            AddDataTable();
            fillType();
        }

        private void dgvPaymenttype_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // For Pop to Ask do u want to Delete or Not
                DialogResult dr = MessageBox.Show("Do you really want to delete", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    
                    dgvPaymenttype.Rows.RemoveAt(iRowIndex);
                    dgvPaymenttype.AllowUserToAddRows = false;
                }
                for (Int32 i = 0; i<dgvPaymenttype.Rows.Count; i++)
                {
                    this.dgvPaymenttype.Rows[i].Cells[0].Value = i + 1;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("There is no row to Delete", "Information "+Ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);  
            }
        }

        private void dgvPaymenttype_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvPaymenttype_SelectionChanged(object sender, EventArgs e)
        {
           
        }

        private void dgvPaymenttype_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                iRowIndex = e.RowIndex;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString()); 
            }
        }
        #region Add Table
        private void AddDataTable()
        {
            // For Declaring Table Name
            dtRows.TableName = "PaymentType";
            // Add Columns into the Database
            dtRows.Columns.Add("ID", typeof(Int32));
            dtRows.Columns.Add("Name", typeof(String));

        }

        #endregion

        private void frmUpdatePaymenttype_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F3)
            //{
            //    btnDelete_Click(null, null);
            //}
            //if (e.KeyCode == Keys.F2)
            //{
            //    btnSave_Click(null, null); 
            //}
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            frmUpdatePaymenttype_Load_1(null, null); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Count Rows of the grid
                Int32 iRowCount = dgvPaymenttype.Rows.Count;
                // Allowing User to Add New Row

                this.dgvPaymenttype.AllowUserToAddRows = true;
                // Selected the Last Row to add new Row
                this.dgvPaymenttype.Rows[iRowCount].Selected = true;
                dtRows.Clear();
                btnType.Enabled = false;
                btnSave.Enabled = true;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString()); 
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dgvPaymenttype_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            this.dgvPaymenttype.Rows[dgvPaymenttype.Rows.Count - 2].Cells[0].Value = dgvPaymenttype.Rows.Count - 1;
        }
    }
}