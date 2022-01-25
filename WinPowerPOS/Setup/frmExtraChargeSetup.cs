using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.Setup
{
    public partial class frmExtraChargeSetup : Form
    {
        public frmExtraChargeSetup()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (errorProvider1.GetError(tPaymentType) != "")
                    throw new Exception(errorProvider1.GetError(tPaymentType));

                if (errorProvider1.GetError(cmbMode) != "")
                    throw new Exception(errorProvider1.GetError(cmbMode));

                if (errorProvider1.GetError(tAmount) != "")
                    throw new Exception(errorProvider1.GetError(tAmount));

                AppSetting.SetSetting(AppSetting.SettingsName.Payment.ExtraChargeType + tPaymentType.Text, cmbMode.SelectedItem.ToString());
                AppSetting.SetSetting(AppSetting.SettingsName.Payment.ExtraChargeAmount + tPaymentType.Text, tAmount.Text);

                BindGrid();

                tPaymentType.Text = "";
                cmbMode.SelectedIndex = 0;
                tAmount.Text = "";
            }
            catch (Exception X)
            {
                MessageBox.Show(X.Message);
                return;
            }
        }

        private void tPaymentType_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                errorProvider1.SetError(tPaymentType, "");

                if (tPaymentType.Text == "")
                    throw new Exception("Payment Type is a required value");
            }
            catch (Exception X)
            {
                errorProvider1.SetError(tPaymentType, X.Message);
            }
        }

        private void tAmount_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                errorProvider1.SetError(tAmount, "");

                decimal Tmp = 0;
                if (tAmount.Text == "")
                    throw new Exception("Amount is a required value");

                if (!decimal.TryParse(tAmount.Text, out Tmp))
                    throw new Exception("Amount value is in incorrect format");
            }
            catch (Exception X)
            {
                errorProvider1.SetError(tAmount, X.Message);
            }
        }

        private void frmExtraChargeSetup_Load(object sender, EventArgs e)
        {
            cmbMode.SelectedIndex = 0;

            BindGrid();
        }
        private void BindGrid()
        {
            string SQLString =
                "SELECT PaymentType, ISNULL(Mode,'percent') Mode, ISNULL(Amount, 0) Amount FROM " +
                "( " +
                    "SELECT SUBSTRING(AppSettingKey, LEN('Payment_ExtraChargeType_') + 1, 50) PaymentType, 'Mode' Cols, AppSettingValue " +
                    "FROM AppSetting " +
                    "WHERE AppSettingKey LIKE 'Payment_ExtraChargeType_%' " +
                  "UNION ALL " +
                    "SELECT SUBSTRING(AppSettingKey, LEN('Payment_ExtraChargeAmount_') + 1, 50) PaymentType, 'Amount' Cols, AppSettingValue " +
                    "FROM AppSetting " +
                    "WHERE AppSettingKey LIKE 'Payment_ExtraChargeAmount_%' " +
                ") DT " +
                "PIVOT " +
                "( " +
                    "MAX(AppSettingValue) " +
                    "FOR Cols IN " +
                    "([Mode],[Amount]) " +
                ") AS Pvt ";

            DataTable DT = new DataTable();
            DT.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(SQLString)));

            dataGridView1.DataSource = DT;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == dgvcEdit.Name)
            {
                tPaymentType.Text = dataGridView1[dgvcPaymentType.Name, e.RowIndex].Value.ToString();
                tAmount.Text = dataGridView1[dgvcExtraChargeAmount.Name, e.RowIndex].Value.ToString();
                if (dataGridView1[dgvcExtraChargeType.Name, e.RowIndex].Value.ToString().ToLower() == "amount")
                    cmbMode.SelectedIndex = 0;
                else
                    cmbMode.SelectedIndex = 1;
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == dgvcDel.Name)
            {
                DialogResult DR = MessageBox.Show(
                    "Are you sure you want to delete Extra Charge Information for payment type " + dataGridView1[dgvcPaymentType.Name, e.RowIndex].Value.ToString() + "?"
                    , "Please Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (DR == DialogResult.No)
                    return;

                AppSetting.DeleteSetting(AppSetting.SettingsName.Payment.ExtraChargeType + tPaymentType.Text);
                AppSetting.DeleteSetting(AppSetting.SettingsName.Payment.ExtraChargeAmount + tPaymentType.Text);

                BindGrid();
            }
        }
    }
}