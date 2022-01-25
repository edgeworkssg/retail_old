using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PointsImporter
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
