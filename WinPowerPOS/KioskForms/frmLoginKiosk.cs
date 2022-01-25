using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.KioskForms
{
    public partial class frmLoginKiosk : Form
    {
        public bool IsStaffVerify = false;
        public frmLoginKiosk()
        {
            InitializeComponent();
            ctrlLoginKiosk.CancelClick += new EventHandler(ctrlLoginKiosk_CancelClick);
            ctrlLoginKiosk.NRICClick += new EventHandler(ctrlLoginKiosk_NRICClick);
            ctrlLoginKiosk.StaffClick += new EventHandler(ctrlLoginKiosk_StaffClick);
        }

        void ctrlLoginKiosk_StaffClick(object sender, EventArgs e)
        {
            IsStaffVerify = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        void ctrlLoginKiosk_NRICClick(object sender, EventArgs e)
        {
            IsStaffVerify = false;
            DialogResult = DialogResult.OK;
            Close();
        }

        void ctrlLoginKiosk_CancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
