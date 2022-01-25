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
using System.Configuration;

namespace WinPowerPOS
{
    public partial class frmGroupAccess : Form
    {
        public bool isSuccessful;
        public frmGroupAccess()
        {
            isSuccessful = false;
            InitializeComponent();
        }
        public POSController pos;
        bool useItemForHotKeys;
        public string PriceMode;
        private void frmGroupAccess_Load(object sender, EventArgs e)
        {         
            DataTable dt;
            useItemForHotKeys = false;
            if (AppSetting.GetSettingFromDBAndConfigFile("UseItemForHotKeys") != null
                && AppSetting.GetSettingFromDBAndConfigFile("UseItemForHotKeys").ToString().ToLower() == "yes")
            {
                ItemDepartmentCollection it = new ItemDepartmentCollection();
                it.Where(ItemDepartment.Columns.Deleted, false);
                it.Load();
                useItemForHotKeys = true;
                dt = it.ToDataTable();
            }
            else
            {
                QuickAccessGroupCollection cb = new QuickAccessGroupCollection();
                cb.Where(QuickAccessGroup.Columns.Deleted, false);
                cb.OrderByAsc(QuickAccessGroup.Columns.ModifiedOn);
                cb.Load();
                useItemForHotKeys = false;
                dt = cb.ToDataTable();
            }
            //populate the flow lay out with programmable button
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Button b = new Button();
                b.Width = 150;
                b.Height = 110;
                if (useItemForHotKeys)
                {
                    b.Tag = dt.Rows[i]["ItemDepartmentID"].ToString();
                    b.Text = dt.Rows[i]["DepartmentName"].ToString();
                }
                else
                {
                    b.Tag = new Guid(dt.Rows[i]["QuickAccessGroupID"].ToString());
                    b.Text = dt.Rows[i]["QuickAccessGroupName"].ToString().ToUpper();           
                }
                b.Font = new Font(b.Font.FontFamily, 10, FontStyle.Bold);
                b.ForeColor = System.Drawing.Color.Black;
                b.BackColor = System.Drawing.Color.LightBlue;
                
                b.Click += delegate
                {
                    btnCat_Click(b, new EventArgs());
                };
                flowLayoutPanel1.Controls.Add(b);
            }
        }

        private void btnCat_Click(object sender, EventArgs e)
        {            
            //Pop out a message
            frmCatAccess f = new frmCatAccess();
            if (useItemForHotKeys)
            {
                f.ItemDepartmentID = ((Button)sender).Tag.ToString();
            }
            else 
            {
                f.QuickAccessCategoryID = ((Guid)((Button)sender).Tag);
            }
            f.useItemForHotKeys = useItemForHotKeys;
            f.pos = pos;
            f.PriceMode = PriceMode;
            f.ShowDialog();
            if (f.isSuccessful)
            {
                isSuccessful = true;
                f.Dispose();
                this.Close();
            }            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isSuccessful = false;
            this.Close();
        }
    }
}
