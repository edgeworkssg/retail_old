using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;

namespace WinPowerPOS.KioskForms
{
    public partial class frmItemList : Form
    {
        private DataTable _itemData = new DataTable();
        public string Barcode = "";

        public frmItemList(DataTable itemData)
        {
            InitializeComponent();

            _itemData = itemData;
            ctrlItemList.ItemDataSource = _itemData;
            ctrlItemList.ItemSelected += new ItemList.ItemSelectedHandler(ctrlItemList_ItemSelected);
            ctrlItemList.CloseForm += new EventHandler(ctrlItemList_CloseForm);
        }

        void ctrlItemList_CloseForm(object sender, EventArgs e)
        {
            Barcode = "";
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        void ctrlItemList_ItemSelected(object sender, ItemSelectedArgs e)
        {
            Barcode = e.Barcode;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmItemList_Load(object sender, EventArgs e)
        {

        }
    }
}
