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
    public partial class frmStaffFunction : Form
    {
        public frmStaffFunction()
        {
            InitializeComponent();

            ctrlReprint.ButtonText = "Reprint Last Receipt";
            ctrlReprint.ControlClick +=new EventHandler(ctrlReprint_ControlClick);

            ctrlResetBagging.ButtonText = "Reset\nBagging Area";
            ctrlResetBagging.ButtonColor = "#CCA351";
            ctrlResetBagging.ButtonHoverColor = "#F2C160";
            ctrlResetBagging.ButtonPressColor = "#CB9933";
            ctrlResetBagging.ControlClick += new EventHandler(ctrlResetBagging_ControlClick);

            ctrlLogout.ButtonText = "Reset Transaction";
            ctrlLogout.ButtonColor = "#FF6633";
            ctrlLogout.ButtonHoverColor = "#FE8259";
            ctrlLogout.ButtonPressColor = "#D83E0A";
            ctrlLogout.ControlClick += new EventHandler(ctrlLogout_ControlClick);

            ctrlClose.ButtonText = "X";
            ctrlClose.ButtonColor = "#A51818";
            ctrlClose.ButtonHoverColor = "#CC3D3D";
            ctrlClose.ButtonPressColor = "#7F0000";
            ctrlClose.ControlClick += new EventHandler(ctrlClose_ControlClick);
        }

        void ctrlResetBagging_ControlClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            this.Close();
        }

        void ctrlClose_ControlClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            this.Close();
        }

        void ctrlReprint_ControlClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        void ctrlLogout_ControlClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
