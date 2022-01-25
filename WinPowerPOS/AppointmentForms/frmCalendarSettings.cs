using PowerPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.AppointmentForms
{
    public partial class frmCalendarSettings : Form
    {
        UserMst selUser = null;

        public frmCalendarSettings(UserMst selectedUser)
        {
            InitializeComponent();
            selUser = selectedUser;
        }

        private void frmCalendarSettings_Load(object sender, EventArgs e)
        {
            try
            {
                lblDisplayName.Text = selUser.DisplayName;
                lblCalID.Text = selUser.GoogleCalendarID;

                BindData();
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Some error occured. Please contact your administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.writeLog(X);
                }

                this.Close();
            }
        }

        private void BindData()
        {
            Tabel.AutoGenerateColumns = false;
            Tabel.DataSource = GoogleController.GetOwnCalendar_forListBox();
        }

        private void Tabel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (e.ColumnIndex == Tabel.Columns[dgvcSelect.Name].Index)
            {
                lblCalID.Text = Tabel[dgvcCalendarID.Name, e.RowIndex].Value.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            selUser.GoogleCalendarID = lblCalID.Text;
            selUser.Save();
            MessageBox.Show("Settings saved successfully", "Success", MessageBoxButtons.OK);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
