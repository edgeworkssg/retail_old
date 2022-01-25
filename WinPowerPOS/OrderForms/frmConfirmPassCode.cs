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
    public partial class frmConfirmPassCode : Form
    {
        public bool isConfirmed = false;
        public POSController pos = null;


        public frmConfirmPassCode()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(pos.IsPassCodeMatch(txtPassCode.Text))
            {
                this.isConfirmed = true;
                this.Close();
            }
            else
            {
                this.isConfirmed = false;
                MessageBox.Show("Pass Code not same. Please check your Pass Code.");
                txtPassCode.Text = "";
            }
            
        }

        private void txtPassCode_Click(object sender, EventArgs e)
        {
            frmKeypadPassCode f = new frmKeypadPassCode();
            f.IsInteger = false;
            f.initialValue = "";
            f.ShowDialog();

            var val = f.value;
            //if (val.Length != 4)
            //{
            //    MessageBox.Show("Pass Code should be numeric and must be 4 digit. Please change your Pass Code");
            //    txtPassCode.Text = "";
            //    return;
            //}

            txtPassCode.Text = val;
        }

        private void frmConfirmPassCode_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            try
            {
                this.Location = Screen.AllScreens[1].WorkingArea.Location;
                this.Left = Screen.AllScreens[1].Bounds.Left + Screen.AllScreens[1].Bounds.Width / 4;
                this.Top = Screen.AllScreens[1].Bounds.Top + Screen.AllScreens[1].Bounds.Height / 4;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                this.Location = new Point(Screen.AllScreens[0].Bounds.Width / 4, Screen.AllScreens[0].Bounds.Height / 4);
            }
            this.WindowState = FormWindowState.Normal;
        }

        private void txtPassCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Return)
            {
                btnConfirm_Click(sender, e);
            }
        }

        private void frmConfirmPassCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close(); 
            }
        }

        private void txtPassCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

    }
}
