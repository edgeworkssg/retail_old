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
using PowerPOS.Container;
using System.Collections;
using PowerEdge.MembershipForm;

namespace PowerEdge.MembershipForms
{
    
    public partial class frmNewMembershipEdit : Form
    {
        private const int ESPRITMEMBERGROUPID = 13;
        private const int NON_ESPRITMEMBERGROUPID = 9;
        private const int CONTROL_SIZE = 150;
        private const string MEMBERSHIP_PREFIX = "T";
        private char[] chrArr = { ',' };
        
        public string membershipNo;
        public bool IsReadOnly;
        private Membership mbr;
        private ListBox lbNameAutoComplete;

        public frmNewMembershipEdit()
        {
            
            InitializeComponent();
            IsReadOnly = false;

            lbNameAutoComplete = new ListBox();
            lbNameAutoComplete.Visible = false;
            lbNameAutoComplete.KeyDown += new KeyEventHandler(listBox1_KeyDown);
            lbNameAutoComplete.Location = new Point(200, 135);            
            
            this.Controls.Add(lbNameAutoComplete);

            UserMstCollection st = new UserMstCollection();
            //st.Where(UserMst.Columns.Deleted, false);
            st.Where(UserMst.Columns.Deleted, false);
            st.Where(UserMst.Columns.IsASalesPerson, true);
            st.Load();
            st.Sort(UserMst.Columns.UserName, true);
            ArrayList ar = new ArrayList();
            ar.Add("--PLEASE SELECT--");
            for (int i = 0; i < st.Count; i++)
            {
                ar.Add(st[i].UserName);
            }
            cmbSalesPerson.DataSource = ar; //SalesPersonController.FetchSalesPersonNames();
            cmbSalesPerson.Refresh();
        }
        
        private void frmNewMembershipEdit_Load(object sender, EventArgs e)
        {
            frmPin f = new frmPin();
            f.ShowDialog();
            f.Dispose();

            CreateNew();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to close?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }
                
                        
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbSalesPerson.SelectedIndex == 0)
            {
                MessageBox.Show("PLEASE SELECT STYLIST");               
                return;
            }
            int day, year=0, month=0;
            if (txtNameToAppear.Text == "")
            {
                MessageBox.Show("ENTER CUSTOMER NAME");
                txtNameToAppear.Select();
                return;
            }
            if (txtGender.Text != "" && txtGender.Text.ToUpper() != "M" && txtGender.Text != "F")
            {
                MessageBox.Show("ENTER CORRECT GENDER (M or F)");
                txtGender.Select();
                return;
            }
            if (txtDate.Text != "")
            {
                if (!int.TryParse(txtDate.Text, out day) || day < 1 || day > 31)
                {
                    MessageBox.Show("ENTER CORRECT BIRTHDATE (1 to 31)");
                    txtDate.Select();
                    return;
                }
            }
            else
            {
                day = 1;
            }

            if (txtMonth.Text != "")
            {
                if (!int.TryParse(txtMonth.Text, out month) || month < 1 || month > 12)
                {
                    MessageBox.Show("ENTER CORRECT BIRTH MONTH (1 to 12)");
                    return;
                }
            }
            else
            {
                month = 1;
            }
            if (txtYear.Text != "")
            {
                if (!int.TryParse(txtYear.Text, out year))
                {
                    MessageBox.Show("CHOOSE CORRECT BIRTH YEAR");
                    txtYear.Select();
                    return;
                }
            }
            else
            {
                year = 1900;
            }
            DateTime birthdate;
            string tmp;
            
            tmp = year.ToString() + "-" + month.ToString() + "-" + day.ToString();
            
            if (!DateTime.TryParse(tmp, out birthdate))
            {
                MessageBox.Show("CHOOSE CORRECT BIRTHDATE");
                txtDate.Select();
                return;
            }
            mbr = new Membership();
            mbr.IsNew = true;
            mbr.MembershipNo = MembershipController.getNewMembershipNo(MEMBERSHIP_PREFIX);
            
                mbr.StreetName = txtAddress.Text.ToUpper();
                if (txtFloor.Text != "")
                    mbr.StreetName = mbr.StreetName + "#" + txtFloor.Text.Trim('#');
                if (txtUnitNo.Text != "")
                    mbr.StreetName = mbr.StreetName +"-" + txtUnitNo.Text.Trim('-');
                mbr.StreetName2 = txtAddress1.Text.ToUpper();
                mbr.City = txtCity.Text.ToUpper();
                mbr.Country = txtCountry.Text.ToUpper();
                mbr.Email = txtEmail.Text;                    
                mbr.FirstName = "";
                mbr.Home = txtHome.Text.ToUpper();
                mbr.LastName = "";
                mbr.ChineseName = "";
                mbr.ChristianName = "";
                mbr.Mobile = txtMobile.Text.ToUpper();
                mbr.NameToAppear = txtNameToAppear.Text.ToUpper();
                mbr.SalesPersonID = cmbSalesPerson.SelectedItem.ToString();
                if (txtNRIC.Text != "")
                {
                    mbr.Nric = txtNRIC.Text.ToUpper();
                    mbr.MembershipGroupId = ESPRITMEMBERGROUPID;
                }
                else
                {
                    mbr.MembershipGroupId = NON_ESPRITMEMBERGROUPID;
                }
                mbr.Office = txtOffice.Text.ToUpper();
                mbr.ZipCode = txtZipCode.Text.ToUpper();                
                mbr.Gender = txtGender.Text;                
                mbr.Remarks = txtRemark.Text;
                mbr.DateOfBirth = birthdate;
                mbr.UniqueID = Guid.NewGuid();
                mbr.Deleted = false;                
                mbr.Save("");
                CreateNew();                            
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Row % 2 == 0)
            {
                e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.CellBounds);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.LightSkyBlue, e.CellBounds);
            }
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void txtGeneric_Enter(object sender, EventArgs e)
        {
            lbNameAutoComplete.Visible = false;
            ((TextBox)sender).BackColor = Color.Yellow;
        }

        private void txtGeneric_Leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.White;
        }
        private void txtNameToAppear_Leave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.White;
            //lbNameAutoComplete.Visible = false;
        }        

        private void txtZipCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AssignAddressFromPostalCode();
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down)
            {
                SendKeys.Send("{Tab}");
            }
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up)
            {
                SendKeys.Send("+{Tab}");
            }
        }

        private void AssignAddressFromPostalCode()
        {
            DataSet ds = SPs.GetAddressFromPostalCode(txtZipCode.Text).GetDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtAddress.Text = ds.Tables[0].Rows[0]["Area1"].ToString();
                txtAddress1.Text = ds.Tables[0].Rows[0]["Area2"].ToString();
                txtCity.Text = "SINGAPORE";
                txtCountry.Text = "SINGAPORE";
            }
        }
        private void CreateNew()
        {
            txtMembershipNo.Text = MembershipController.getNewMembershipNo(MEMBERSHIP_PREFIX);
            txtGender.Text = "";
            txtMonth.Text = "";
            txtChristianName.Text = "";            
            txtAddress1.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtCountry.Text = "";
            txtEmail.Text = "";
            txtFloor.Text = "";
            txtUnitNo.Text = "";
            txtHome.Text = "";            
            txtMobile.Text = "";
            txtNameToAppear.Text = "";
            txtNRIC.Text = "";            
            txtOffice.Text = "";            
            txtZipCode.Text = "";            
            txtRemark.Text = "";
            txtDate.Text = "";
            txtYear.Text = "";

            txtNameToAppear.Select();
        }

        
        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("All your entry will be lost, are you sure you want to clear?", "", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                CreateNew();
            }
        }

        private void txtGeneric_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down)
            {
                SendKeys.Send("{Tab}");
            }
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up)
            {
                SendKeys.Send("+{Tab}");
            }
        }

        private void txtNameToAppear_KeyUp(object sender, KeyEventArgs e)        
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (lbNameAutoComplete.Visible == true)
                {
                    lbNameAutoComplete.Visible = false;
                }
            }
            if (e.KeyCode == Keys.Enter)
            {
                //if autocomplete is visible, add the words into the query
                if (lbNameAutoComplete.Visible == true)
                {
                    AddAutoSuggest((TextBox)sender);
                }
                else //else send tabs
                {                   
                    lbNameAutoComplete.Visible = false;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                //if auto complete is on, activate it
                if (lbNameAutoComplete.Visible)
                {
                    if (lbNameAutoComplete.Items.Count > 1) lbNameAutoComplete.SelectedIndex = 1;
                    lbNameAutoComplete.Focus();                                        
                }
                else
                {
                    SendKeys.Send("{Tab}");
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                //if auto complete is on, activate it
                if (!lbNameAutoComplete.Visible)
                {
                    SendKeys.Send("+{Tab}");
                }
                
            }
            else
            {
                string senderTextBox = ((TextBox)sender).Text;
                string query = "";
                int lastIndex = senderTextBox.LastIndexOf(' ');
                if (lastIndex < 0) lastIndex = 0;

                query = senderTextBox.Substring(lastIndex).TrimStart();
                if (query != "")
                {
                    //Position the item to the screen
                    lbNameAutoComplete.Size = new Size(200, 200);

                    //populate data
                    lbNameAutoComplete.Items.Clear();

                    DataSet ds = SPs.FetchAutoCompleteNames(query).GetDataSet();
                    ArrayList arraylist = new ArrayList();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        arraylist.Add(row["Names"]);
                    }
                    
                    lbNameAutoComplete.Items.AddRange(arraylist.ToArray());
                    //lbNameAutoComplete.DataSource = arraylist;
                    //lbNameAutoComplete.Refresh();
                    if (lbNameAutoComplete.Items.Count > 0)
                    {
                        lbNameAutoComplete.Visible = true;
                        lbNameAutoComplete.SelectedIndex = 0;
                        lbNameAutoComplete.BringToFront();
                    }
                    else
                    {
                        lbNameAutoComplete.Visible = false;
                    }
                }
                else
                {
                    lbNameAutoComplete.Visible = false;
                }
            }
        }

        private void AddAutoSuggest(TextBox sender)
        {
            string senderTextBox = sender.Text;

            int lastIndex = senderTextBox.LastIndexOf(' ');
            if (lastIndex < 0) lastIndex = 0;
            string textBeforeAppend = "";
            if (lastIndex != 0)
                textBeforeAppend = senderTextBox.Substring(0, lastIndex + 1);
            lbNameAutoComplete.Visible = false;
            sender.Text = textBeforeAppend + lbNameAutoComplete.SelectedItem.ToString();
            sender.Focus();
            sender.Select(sender.Text.Length, 0);
            
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //add item into text...
                AddAutoSuggest(txtNameToAppear);
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (lbNameAutoComplete.SelectedIndex == 0)
                {
                    txtNameToAppear.Focus();
                    txtNameToAppear.Select(txtNameToAppear.Text.Length, 0);
                }
            }
        }

        private void btnSave_Enter(object sender, EventArgs e)
        {
            ((Button)sender).ForeColor = Color.Yellow;
            Font f = new Font("Verdana", 16, FontStyle.Bold);            
            ((Button)sender).Font = f;
            
        }

        private void btnSave_Leave(object sender, EventArgs e)
        {
            ((Button)sender).ForeColor = Color.White;
            Font f = new Font("Verdana", 9, FontStyle.Bold);
            ((Button)sender).Font = f;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            frmMembershipSearch f = new frmMembershipSearch();
            f.ShowDialog();
            f.Dispose();
        }
        
    }
}
