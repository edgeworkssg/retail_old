using System;
using System.Data;
using System.Windows.Forms;

namespace PowerInventory
{
    public partial class frmImportErrorMessage : Form
    {
        public DataTable source;
        public frmImportErrorMessage()
        {
            InitializeComponent();
        }

        private void frmImportErrorMessage_Load(object sender, EventArgs e)
        {
            dgvErrorMessage.DataSource = source;
            dgvErrorMessage.Refresh();
        }
    }
}
