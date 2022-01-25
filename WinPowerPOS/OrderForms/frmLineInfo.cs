using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS.Container;

namespace PowerPOS
{
    public partial class frmLineInfo : Form
    {
        public POSController pos;
        private bool dropdownCanAddNew = false;
        private string lineInfoCaption;

        public frmLineInfo(POSController pos)
        {
            InitializeComponent();
            this.pos = pos;
            dropdownCanAddNew = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.DropdownCanAddNew), false);
            lineInfoCaption = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ReplaceTextWith));
        }

        private void frmLineInfo_Load(object sender, EventArgs e)
        {
            LineInfoCollection lineInfos = new LineInfoCollection();
            lineInfos.Where(LineInfo.Columns.Deleted, false);
            lineInfos.Load();
            //lineInfos.Add(new LineInfo { LineInfoName = "" });
            lineInfos.Sort(LineInfo.Columns.LineInfoName, true);

            cmbLineInfo.DataSource = lineInfos;
            cmbLineInfo.DisplayMember = LineInfo.Columns.LineInfoName;
            cmbLineInfo.ValueMember = LineInfo.Columns.LineInfoName;
            if (dropdownCanAddNew)
                cmbLineInfo.DropDownStyle = ComboBoxStyle.DropDown;
            else
                cmbLineInfo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLineInfo.Refresh();

            if (!string.IsNullOrEmpty(lineInfoCaption)) label2.Text = lineInfoCaption;

            BillInfoCollection billInfos = new BillInfoCollection();
            billInfos.Where(BillInfo.Columns.Deleted, false);
            billInfos.Load();
            billInfos.Add(new BillInfo { BillInfoName = "" });
            billInfos.Sort(BillInfo.Columns.BillInfoName, true);

            cmbBillInfo.DataSource = billInfos;
            cmbBillInfo.DisplayMember = BillInfo.Columns.BillInfoName;
            cmbBillInfo.ValueMember = BillInfo.Columns.BillInfoName;
            cmbBillInfo.Refresh();

            if (!string.IsNullOrEmpty(pos.myOrderHdr.BillInfo))
            {
                cmbBillInfo.SelectedValue = pos.myOrderHdr.BillInfo;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string lineText = cmbLineInfo.Text.Trim();
            //if (cmbLineInfo.SelectedIndex < 0)
            if (cmbLineInfo.FindStringExact(lineText) < 0)
            {
                if (dropdownCanAddNew)
                {
                    DialogResult res = MessageBox.Show("Entered text does not exist in database. Do you want to add new?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (res == DialogResult.OK)
                    {
                        LineInfo li = new LineInfo(LineInfo.Columns.LineInfoName, lineText);
                        if (li == null || li.LineInfoName != lineText)
                        {
                            // not exists, then add new
                            li.LineInfoName = lineText;
                            li.UniqueID = Guid.NewGuid();
                            li.Deleted = false;
                            li.Save(UserInfo.username);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    lblResult.Text = "Please select Line Information";
                    return;
                }
            }
            for (int i = 0; i < pos.myOrderDet.Count; i++)
            {
                pos.myOrderDet[i].Userfld4 = lineText;
            }

            pos.myOrderHdr.BillInfo = cmbBillInfo.SelectedValue.ToString();

            this.Close();
        }
    }
}
