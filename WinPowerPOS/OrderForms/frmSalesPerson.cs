using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using System.Collections;
using System.Linq;
using WinPowerPOS.LoginForms;
using System.Diagnostics;

namespace WinPowerPOS.OrderForms
{
    public partial class frmSalesPerson : Form
    {
        public bool IsSuccessful;
        public string AssignedStaff;

        public frmSalesPerson()
        {
            InitializeComponent();
        }

        private void frmSalesPerson_Load(object sender, EventArgs e)
        {
            UserMstCollection st = new UserMstCollection();
            //st.Where(UserMst.Columns.Deleted, false);
            st.Where(UserMst.Columns.Deleted, false);
            st.Where(UserMst.Columns.IsASalesPerson, true);
            st.Load();
            st.Sort(UserMst.Columns.UserName, true);
            
            ArrayList ar = new ArrayList();
            var theUser = new List<UserMst>();
            bool linkToOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.LinkTheUserWithOutlet), false);

            for (int i = 0; i < st.Count; i++)
            {
                string[] selOutlet = st[i].AssignedOutletList;

                if (linkToOutlet)
                {
                    if (selOutlet.Contains(PointOfSaleInfo.OutletName))
                    {
                        ar.Add(st[i].UserName);
                        theUser.Add(st[i]);
                    }
                }
                else
                {
                    ar.Add(st[i].UserName);
                    theUser.Add(st[i]);
                }
            }
            //cmbSalesPerson.DataSource = ar; //SalesPersonController.FetchSalesPersonNames();
            cmbSalesPerson.DataSource = theUser.OrderBy(o => o.DisplayName).ToList();
            cmbSalesPerson.DisplayMember = UserMst.Columns.DisplayName;
            cmbSalesPerson.ValueMember = UserMst.Columns.UserName;
            cmbSalesPerson.Refresh();

            if (!string.IsNullOrEmpty(AssignedStaff))
            {
                cmbSalesPerson.SelectedValue = AssignedStaff;
            }

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.PromptPasswordOnSelectSalesPerson), false))
            {
                pnlPasswordInput.Visible = true;
                pnlPasswordInput.Show();
            }

            IsSuccessful = false;
        }

        private static void OpenWindows8TouchKeyboard()
        {
            var path = @"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe";
            Process.Start(path);
        }
        private void CloseOnscreenKeyboard()
        {
            //Kill all on screen keyboards
            Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
            if (oskProcessArray.Length > 0)
            {
                foreach (Process onscreenProcess in oskProcessArray)
                {
                    onscreenProcess.Kill();
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            UserMst salesman = new UserMst(cmbSalesPerson.SelectedValue.ToString());
            if (!salesman.IsNew)
            {
                //#region *) Check Privileges

                if (!salesman.UserName.ToLower().Equals(UserInfo.username.ToLower()))
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.PromptPasswordOnSelectSalesPerson), false))
                    {
                        string role, status, DeptID;
                        UserMst user;
                        if (!UserController.login(salesman.UserName, txtPassword.Text, LoginType.Authorizing, out user, out role, out DeptID, out status))
                        {
                            MessageBox.Show("Login failed : " + status);
                            IsSuccessful = false;
                        }
                        else
                        {
                            CloseOnscreenKeyboard();
                            SalesPersonInfo.SalesPersonID = salesman.UserName;
                            SalesPersonInfo.SalesPersonName = salesman.DisplayName;
                            SalesPersonInfo.SalesGroupID = salesman.SalesPersonGroupID;
                            IsSuccessful = true;
                            this.Close();
                        }
                    }
                    else
                    {
                        SalesPersonInfo.SalesPersonID = salesman.UserName;
                        SalesPersonInfo.SalesPersonName = salesman.DisplayName;
                        SalesPersonInfo.SalesGroupID = salesman.SalesPersonGroupID;
                        IsSuccessful = true;
                        this.Close();
                    }
                }
                else
                {
                    SalesPersonInfo.SalesPersonID = salesman.UserName;
                    SalesPersonInfo.SalesPersonName = salesman.DisplayName;
                    SalesPersonInfo.SalesGroupID = salesman.SalesPersonGroupID;

                    IsSuccessful = true;
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SalesPersonInfo.SalesPersonID = "0";
            IsSuccessful = false;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CloseOnscreenKeyboard();
            IsSuccessful = false;
            this.Close();
        }
        private int clickNo = 0;
        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            clickNo++;
            switch (clickNo)
            {
                case 1: OpenWindows8TouchKeyboard(); break;
                case 2: CloseOnscreenKeyboard(); clickNo = 0; break;
            } 
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnterAsOK), false))
                {
                    btnLogin_Click(this, new EventArgs());
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
        }

        private void cmbSalesPerson_SelectedValueChanged(object sender, EventArgs e)
        {
            if(txtPassword.Visible)
                txtPassword.Focus();
        }

    }
}