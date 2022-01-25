using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.Collections;

namespace PowerInventory
{
    public partial class frmExportTemplate : Form
    {
        public string STOCKTYPE { get; set; }
        public frmExportTemplate()
        {
            InitializeComponent();
        }

        private void frmExportTemplate_Load(object sender, EventArgs e)
        {
            RefreshCategoryList();
        }

        private void RefreshCategoryList()
        {
            string QueryStr =
                "SELECT CategoryName FROM Category WHERE (Deleted IS NULL OR Deleted = 0) ";

            listCategory.Items.Clear();

            IDataReader Rdr = SubSonic.DataService.GetReader(new SubSonic.QueryCommand(QueryStr));
            while (Rdr.Read())
            {
                listCategory.Items.Add(Rdr.GetString(0), false);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (listCategory.Items.Count < 1)
            {
                MessageBox.Show("There is no category available\nPlease contact your administrator"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (listCategory.CheckedItems.Count < 1)
            {
                MessageBox.Show("Please select at least 1 (one) category to be exported\nYou can also click select all button to export all category"
                    , "No category is selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            saveFileDialogExport.ShowDialog();
        }

        private void saveFileDialogExport_FileOk(object sender, CancelEventArgs e)
        {
            string QueryStr =
                "SELECT CategoryName, ItemNo, ItemName,ISNULL(ItemDesc,'') ItemDescription, ISNULL(Item.Userfld1,'') as UOM, 0 AS Qty {0} " +
                "FROM Item ";

            QueryStr = string.Format(QueryStr, STOCKTYPE.ToUpper() == "STOCKIN" ? ", FactoryPrice as CostPrice" : "");

            List<string> Conditions = new List<string>();

            if (listCategory.CheckedItems.Count > 0)
            {
                List<string> CheckedCategory = new List<string>();
                for (int Counter = 0; Counter < listCategory.CheckedItems.Count; Counter++)
                {
                    CheckedCategory.Add("N'" + listCategory.CheckedItems[Counter].ToString().Replace("'","''") + "'");
                }
                Conditions.Add("CategoryName IN (" + string.Join(",", CheckedCategory.ToArray()) + ")");
            }

            if (cbIsInInventory.Checked)
                Conditions.Add("IsInInventory = 1");

            if (!cbDeleted.Checked)
                Conditions.Add("(Deleted IS NULL OR Deleted = 0)");

            if (Conditions.Count > 0)
                QueryStr += "WHERE " + string.Join(" AND ", Conditions.ToArray());

            DataTable dt = new DataTable();
            dt.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(QueryStr)));

            try
            {
                ExportController.ExportToCSV(dt, saveFileDialogExport.FileName);
                MessageBox.Show("Save successful.");
                this.Close();
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                MessageBox.Show("Some error occured\nPlease contact your administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbDeleted_CheckedChanged(object sender, EventArgs e)
        {
            //RefreshCategoryList();
        }

        private void btnSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int Counter =0;Counter <listCategory.Items.Count;Counter++)
            {
                listCategory.SetItemChecked(Counter, listCategory.Items[Counter].ToString() != "SYSTEM");
            }
        }
    }
}
