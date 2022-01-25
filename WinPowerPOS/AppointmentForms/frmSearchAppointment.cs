using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;

namespace WinPowerPOS.AppointmentForms
{
    public partial class frmSearchAppointment : Form
    {
        //public DateTime _daySearch = DateTime.Now;
        public String _txtSearch;
        public String _SearchType;

        public frmSearchAppointment()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.txtSearch.Text = "";
            this._txtSearch = "";
            this._SearchType = "";
            this.cbSearch.SelectedIndex = 0;
            //this._daySearch = DateTime.Now;
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid(); 
        }

        private void frmSearchAppointmentByMember_Load(object sender, EventArgs e)
        {
            string listsearch = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.AppointmentSearchList) + "";
            if (string.IsNullOrEmpty(listsearch))
                listsearch = "Name,Mobile,NRIC";
            if (!string.IsNullOrEmpty(listsearch))
            {
                cbSearch.Items.Clear();
                cbSearch.Items.Add("Please Select");
                var l = listsearch.Split(',');
                foreach (string s in l)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        cbSearch.Items.Add(s);
                    }
                }

                cbSearch.SelectedIndex = 0;
            }

            if (!string.IsNullOrEmpty(_txtSearch))
                txtSearch.Text = _txtSearch;

            if (!string.IsNullOrEmpty(_SearchType))
                cbSearch.SelectedItem= _SearchType;

           BindGrid();
        }

        private void BindGrid()
        {
            var searchType = cbSearch.SelectedItem;

            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()) && !searchType.ToString().Contains("Please Select"))
            {
                string query = "select m.MembershipNo, m.NameToAppear, M.Mobile, M.NRIC, a.StartTime, u.DisplayName as Stylist " +
                              "from appointment a inner join membership m on a.MembershipNo = m.MembershipNo " +
                              "inner join UserMst u on a.SalesPersonID = u.UserName " +
                              " where ISNULL(a.Deleted,0) = 0 ";

                if (searchType != null && !searchType.ToString().Contains("Please Select"))
                {
                    if (searchType.Equals("Name"))
                    {
                        query += "and isnull(nametoappear,'') + isnull(firstname,'') + isnull(lastname,'') + isnull(chinesename,'') + isnull(christianname,'') LIKE N'%" + txtSearch.Text.Trim() + "%' ";
                    }
                    else if (searchType.Equals("Mobile"))
                    {
                        query += "and m.Mobile LIKE '%" + txtSearch.Text.Trim() + "%' ";
                    }
                    else
                    {
                        query += "and m.NRIC LIKE '%" + txtSearch.Text.Trim() + "%' ";
                    }
                }

                query += "order by m.MembershipNo, a.StartTime asc ";

                DataSet ds = DataService.GetDataSet(new QueryCommand(query));

                dgvSearch.DataSource = ds.Tables[0];
                dgvSearch.Refresh();
            }
        }

        private void frmSearchAppointment_FormClosing(object sender, FormClosingEventArgs e)
        {
            while (dgvSearch.Rows.Count > 0)
            {
                dgvSearch.Rows.RemoveAt(0);
            }
        }
    }
}
