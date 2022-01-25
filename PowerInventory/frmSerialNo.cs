using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;

namespace PowerInventory
{
    public partial class frmSerialNo : Form
    {
        private List<string> _serialNo = new List<string>();
        public List<string> SerialNo { get { return _serialNo; } }

        private int _qty = 0;
        public int Qty { get { return _qty; } }

        private Item _item = new Item();
        private bool _validateSerialNo = false;
        private ValidationMode _validationMode = ValidationMode.CHECK_IS_EXIST;
        private int inventoryLocationID = 0;

        public frmSerialNo(List<string> serialNo, Item item, int qty, bool validateSerialNo, ValidationMode validationMode, int inputInventoryLocationID)
        {
            InitializeComponent();
            _serialNo = serialNo;
            _qty = qty;
            _item = item;
            _validateSerialNo = validateSerialNo;
            _validationMode = validationMode;

            lblItemName.Text = string.Format("{0} x {1}", qty, item.ItemName.Length >= 25 ? item.ItemName.Substring(0, 25) + "..." : item.ItemName);
            txtSerialNo.Text = string.Join(Environment.NewLine, serialNo.ToArray()) + Environment.NewLine;
            txtSerialNo_TextChanged(txtSerialNo, new EventArgs());
            inventoryLocationID = inputInventoryLocationID;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _serialNo = txtSerialNo.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var serialNoTemp = new List<string>();
            foreach (var serial in _serialNo)
            {
                if (serialNoTemp.Exists(o => o.IsEqual(serial)))
                    continue;
                serialNoTemp.Add(serial);
            }

            _serialNo = serialNoTemp;
            txtSerialNo.Text = string.Join(Environment.NewLine, _serialNo.ToArray()) + Environment.NewLine;
            txtSerialNo_TextChanged(txtSerialNo, new EventArgs());

            if (_serialNo.Count == 0)
            {
                MessageBox.Show("Please enter serial no");
                return;
            }

            if (_serialNo.Count != _qty)
            {
                var res = MessageBox.Show(string.Format("Serial No count ({0}) did not tally with entered qty ({1}). Do you want to override entered qty?", _serialNo.Count, _qty), "Confirmation", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                    _qty = _serialNo.Count;
                else if (res == DialogResult.No)
                    return;
            }

            if (_validateSerialNo)
            {
                pnlProgress.Visible = true;
                this.Enabled = false;
                worker.RunWorkerAsync();
            }
            else
                SaveHelper();
        }

        private void SaveHelper()
        {
            DialogResult = DialogResult.OK;
            this.Close(); 
        }

        private void txtSerialNo_TextChanged(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(txtSerialNo.Text);
            _serialNo = txtSerialNo.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            lblTotalItems.Text = string.Format("{0} Item(s)", _serialNo.Count);
        }

        private void frmSerialNo_Shown(object sender, EventArgs e)
        {
            txtSerialNo.Focus();
        }

        private static string _message = "";
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ItemTagModel input = new ItemTagModel();
            input.ItemNo = _item.ItemNo;
            input.InventoryLocationID = inventoryLocationID;
            input.SerialNoColl = _serialNo;
            var inputColl = new List<ItemTagModel>();
            inputColl.Add(input);
            bool isValid = false;
            if(_validationMode == ValidationMode.CHECK_IS_EXIST)
                isValid = ItemTagController.CheckSerialNoIsExist(inputColl, out _message); 
            else if(_validationMode == ValidationMode.CHECK_IS_NOT_EXIST)
                isValid = ItemTagController.CheckSerialNoIsNotExist(inputColl, out _message); 

            e.Result = isValid;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;
            pnlProgress.Visible = false;
            bool isValid = (bool)e.Result;

            if (!isValid)
            {
                MessageBox.Show(_message);
                return;
            }

            SaveHelper();
        }
    }

    public enum ValidationMode
    {
        CHECK_IS_EXIST,
        CHECK_IS_NOT_EXIST
    }
}
