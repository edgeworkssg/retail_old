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
using SubSonic;
using System.Configuration;
using PowerPOS.Container;

namespace WinPowerPOS.OrderForms
{
    public partial class frmLineCommission : Form
    {
        public string UserName = String.Empty;
        public string UserName2 = String.Empty;
        public string Remark = String.Empty;
        public string LineInfoRemark = String.Empty;
        public Boolean IsSalesReturn;

        public string OldReceiptNo;
        string itemNo;

        public bool IsSuccessful;
        private bool dropdownCanAddNew = false;
        private string lineInfoCaption;

        public frmLineCommission()
        {
            InitializeComponent();
            dropdownCanAddNew = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.DropdownCanAddNew), false);
            lineInfoCaption = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ReplaceTextWith));
        }
        public frmLineCommission(string itemNo)
        {
            this.itemNo = itemNo;

            InitializeComponent();

            this.Text += string.Format(" - {0}", itemNo);
            dropdownCanAddNew = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.DropdownCanAddNew), false);
            lineInfoCaption = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ReplaceTextWith));
        }

        private void frmLineCommission_Load(object sender, EventArgs e)
        {
            int currentIndex = 0;

            UserMstCollection st = new UserMstCollection();
            st.Where(UserMst.Columns.Deleted, false);
            st.Where(UserMst.Columns.IsASalesPerson, true);
            st.Load();
            st.Sort(UserMst.Columns.UserName, true);

            ArrayList ar = new ArrayList();
            var theUser = new List<UserMst>();
            var theUser2 = new List<UserMst>();

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

            theUser.Insert(0, new UserMst { UserName = "", DisplayName = "" });

            theUser2.AddRange(theUser);

            cmbSalesPerson.DataSource = theUser.OrderBy(o => o.DisplayName).ToList();
            cmbSalesPerson.DisplayMember = UserMst.Columns.DisplayName;
            cmbSalesPerson.ValueMember = UserMst.Columns.UserName;
            cmbSalesPerson.Refresh();
            cmbSalesPerson.SelectedIndex = currentIndex;

            cmbSalesPerson2.DataSource = theUser2.OrderBy(o => o.DisplayName).ToList(); ;
            cmbSalesPerson2.DisplayMember = UserMst.Columns.DisplayName;
            cmbSalesPerson2.ValueMember = UserMst.Columns.UserName;
            cmbSalesPerson2.Refresh();
            cmbSalesPerson2.SelectedIndex = currentIndex;

            bool EnableSecondSalesPerson = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.EnableSecondSalesPerson), false);
            if (!EnableSecondSalesPerson)
            {
                lblSalesPerson2.Visible = false;
                cmbSalesPerson2.Visible = false;
            }

            IsSuccessful = false;

            if (!string.IsNullOrEmpty(UserName))
                cmbSalesPerson.SelectedValue = UserName;

            if (!string.IsNullOrEmpty(UserName2))
                cmbSalesPerson2.SelectedValue = UserName2;
            

            LineInfoCollection lineInfos = new LineInfoCollection();
            lineInfos.Where(LineInfo.Columns.Deleted, false);
            lineInfos.Load();
            lineInfos.Sort(LineInfo.Columns.LineInfoName, true);

            cmbLineInfo.DataSource = lineInfos;
            cmbLineInfo.DisplayMember = LineInfo.Columns.LineInfoName;
            cmbLineInfo.ValueMember = LineInfo.Columns.LineInfoName;
            if (dropdownCanAddNew)
                cmbLineInfo.DropDownStyle = ComboBoxStyle.DropDown;
            else
                cmbLineInfo.DropDownStyle = ComboBoxStyle.DropDownList; 
            cmbLineInfo.Refresh();


            txtOldReceiptNo.Text = OldReceiptNo;
            txtLineRemark.Text = Remark;
            cmbLineInfo.Text = LineInfoRemark;

            if (!IsSalesReturn)
            {
                groupBox1.Enabled = false;
            }

            if (!string.IsNullOrEmpty(lineInfoCaption)) label2.Text = lineInfoCaption;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (IsSalesReturn)
            {
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.PreviousReceiptNoNotCompulsory), false))
                {
                    if (txtOldReceiptNo.Text == "")
                    {
                        MessageBox.Show("Receipt is required in Sales Return");
                        txtOldReceiptNo.Focus();
                        return;
                    }
                }
                OldReceiptNo = txtOldReceiptNo.Text;
            }
            else
            {
                OldReceiptNo = "";
            }
            
            if (cmbSalesPerson.SelectedIndex > 0)
            {
                UserMst salesman = new UserMst(cmbSalesPerson.SelectedValue.ToString());
                if (!salesman.IsNew)
                {
                    UserName = salesman.UserName;
                    UserName2 = cmbSalesPerson2.SelectedValue.ToString();
                    Remark = txtLineRemark.Text;
                    if (checkSelectedLineInfo())
                    {
                        LineInfoRemark = cmbLineInfo.Text;
                    }
                    else
                    {
                        return;
                    }
                    IsSuccessful = true;
                    this.Close();
                }                
            }
            else
            {
                UserName = "";
                UserName2 = "";
                Remark = txtLineRemark.Text;
                if (checkSelectedLineInfo())
                {
                    LineInfoRemark = cmbLineInfo.Text;
                }
                else
                {
                    return;
                }
                IsSuccessful = true;
                this.Close();
            }
        }

        public bool checkSelectedLineInfo()
        {
            if (cmbLineInfo.SelectedIndex < 0 && !string.IsNullOrEmpty(cmbLineInfo.Text))
            {
                if (dropdownCanAddNew)
                {
                    DialogResult res = MessageBox.Show("Entered text does not exist in database. Do you want to add new?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (res == DialogResult.OK)
                    {
                        LineInfo li = new LineInfo(LineInfo.Columns.LineInfoName, cmbLineInfo.Text);
                        if (li == null || li.LineInfoName != cmbLineInfo.Text)
                        {
                            // not exists, then add new
                            li.LineInfoName = cmbLineInfo.Text;
                            li.UniqueID = Guid.NewGuid();
                            li.Deleted = false;
                            li.Save(UserInfo.username);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Please select Line Information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        private bool checkIfReceiptNumberExisted()
        {
            string result = "";
            //POSController p;
            //if ((txtOldReceiptNo.Text).Substring(0, 2) == "OR")
            //{
            //    p = new POSController((txtOldReceiptNo.Text).Substring(2));
            //}
            //else
            //    p = new POSController(txtOldReceiptNo.Text);
            //result = p.GetSavedRefNo();

            result = POSController.GetOrderHdrIDByCustomReceiptNo(txtOldReceiptNo.Text); // this.GetOrderHdrIDByCustomReceiptNo(txtOldReceiptNo.Text);
            if (result != "")
            {
                POSController pos = new POSController(result);
                //check if item exist in receipt
                if (pos.IsItemIsInOrderLine(itemNo) == "")
                {
                    DialogResult dr = MessageBox.Show
                        ("This item doesn't exist in the entered receipt no.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtOldReceiptNo.Text = "";
                    return false;
                }
                else
                {
                    OldReceiptNo = txtOldReceiptNo.Text;
                    return true;
                }
            }
            else
            {
                DialogResult dr = MessageBox.Show
                        ("This receipt number is not found in this outlet.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOldReceiptNo.Text = "";
                return false;
                //if (dr == DialogResult.No)
                //{
                //    return false;
                //}
                //else
                //    return true;
            }
        }
        private void txtOldReceiptNo_Leave(object sender, EventArgs e)
        {
            if (txtOldReceiptNo.Text.Trim() == "") return;
            
            string result = "";

            if (CheckIfItemAlreadyReturned(this.txtOldReceiptNo.Text, this.itemNo))
            {
                MessageBox.Show(string.Format("Item Already return with same Receipt {0}!", txtOldReceiptNo.Text));
                txtOldReceiptNo.Text = "";
                return;
            }

            if (checkIfReceiptNumberExisted())
            {
                // string strHdrID = checkIfReceiptNumberExisted();
                int Days = 0;
                try
                {
                    Days = int.Parse(ConfigurationManager.AppSettings["SalesReturnDays"]);
                }
                catch
                {
                    Days = 7;
                }
                result = POSController.GetOrderDateByCustomReceiptNo(txtOldReceiptNo.Text); //this.GetOrderDateByCustomReceiptNo(txtOldReceiptNo.Text);
                DateTime dt = Convert.ToDateTime(result);
                DateTime dtAdd = Convert.ToDateTime(DateTime.Now).AddDays(-Days);
                if (dt < dtAdd)
                {
                    DialogResult dr = MessageBox.Show("You can not return item that was purchased more than " + Days + " days ago.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtOldReceiptNo.Text = "";
                }
            }
        }

        private string GetOrderDateByCustomReceiptNo(string receiptNo)
        {
            string orderhdrid = "";

            string sql = "SELECT OrderDate from OrderHdr where OrderHdrID = '" + receiptNo + "'  order by OrderDate desc";

            QueryCommand qr = new QueryCommand(sql);

            object found = DataService.ExecuteScalar(qr);
            if (found != null)
                orderhdrid = found.ToString();
            return orderhdrid;
        }
        private bool CheckIfItemAlreadyReturned(string OrderHdrID, string ItemNo)
        {
            string sql = "SELECT OD.UserFld5, OD.ItemNo  FROM OrderDet OD WHERE OD.Userfld5=@OrderHdrID AND OD.ItemNo=@ItemNo";
            QueryCommand qr = new QueryCommand(sql);
            qr.Parameters.Add("@OrderHdrID", OrderHdrID);
            qr.Parameters.Add("@ItemNo", ItemNo);

            object found = DataService.ExecuteScalar(qr);
            if (found != null)
                return true;
            else
                return false;
        }
        public string GetOrderHdrIDByCustomReceiptNo(string receiptNo)
        {
            string orderhdrid = "";

            string sql = "SELECT OrderHdrID FROM OrderHdr WHERE OrderHdrID = '" + receiptNo + "'";

            QueryCommand qr = new QueryCommand(sql);

            object found = DataService.ExecuteScalar(qr);
            if (found != null)
                orderhdrid = found.ToString();

            return orderhdrid;

        }

        private void cmbSalesPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSalesPerson2.Enabled = cmbSalesPerson.SelectedIndex > 0;
        }

        private void txtLineRemark_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnterAsOK), false))
                {
                    btnOk_Click(btnOk, new EventArgs());
                }
            }
        }
    }
}
