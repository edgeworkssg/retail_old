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

namespace WinPowerPOS
{
    public partial class frmCatAccessQuotation : Form
    {
        public bool isSuccessful;
        public frmCatAccessQuotation()
        {
            isSuccessful = false;
            InitializeComponent();
        }
        public QuoteController pos;
        public Guid QuickAccessCategoryID;
        public string ItemDepartmentID;
        public bool useItemForHotKeys;
        private void frmCatAccess_Load(object sender, EventArgs e)
        {
            /*
            if (QuickAccessCategoryID == null)
            {
                //prompt user
                this.Close();
            }*/
         
            DataTable dt;
            if (useItemForHotKeys)
            {
                CategoryCollection ct = new CategoryCollection();
                ct.Where(Category.Columns.Deleted, false);
                if (ItemDepartmentID != "")
                    ct.Where(Category.Columns.ItemDepartmentId, ItemDepartmentID);
                ct.Load();
                dt = ct.ToDataTable();
            }
            else
            {
                dt =
                   QuickAccessController.FetchCategories(null,
                        QuickAccessCategoryID);
            }
            //populate the flow lay out with programmable button
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Button b = new Button();
                b.Width = 150;
                b.Height = 110;
                if (useItemForHotKeys)
                {
                    b.Tag = (dt.Rows[i]["CategoryName"].ToString());
                    b.Text = dt.Rows[i]["CategoryName"].ToString().ToUpper();
                
                }
                else
                {
                    b.Tag = new Guid(dt.Rows[i]["QuickAccessCategoryID"].ToString());
                    b.Text = dt.Rows[i]["QuickAccessCatName"].ToString().ToUpper();
                }
                b.Font = new Font(b.Font.FontFamily, 10, FontStyle.Bold);
                if (!useItemForHotKeys &&
                    dt.Rows[i]["ForeColor"] != null && dt.Rows[i]["ForeColor"].ToString() != "")
                {
                    b.ForeColor = System.Drawing.Color.FromName(dt.Rows[i]["ForeColor"].ToString());
                }
                else
                {
                    b.ForeColor = System.Drawing.Color.Black;
                }

                if (!useItemForHotKeys && dt.Rows[i]["BackColor"] != null && dt.Rows[i]["BackColor"].ToString() != "")
                {
                    string name = AppDomain.CurrentDomain.BaseDirectory +
                                    QuickAccessController.ButtonImageFolder + dt.Rows[i]["BackColor"] + ".png";
                    if (System.IO.File.Exists(name))
                    {
                        b.BackgroundImage = System.Drawing.Image.FromFile(name);
                    }
                    else
                    {
                        b.BackColor = System.Drawing.Color.FromName(dt.Rows[i]["BackColor"].ToString());
                    }
                }
                else
                {
                    b.BackColor = System.Drawing.Color.White;
                }
                b.Click += delegate
                {
                    btnCat_Click(b, new EventArgs());
                };
                flowLayoutPanel1.Controls.Add(b);
            }
        }

        private void btnCat_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(((Button)sender).Tag.ToString());

            //Pop out a message
            frmBtnAccessQuotation f = new frmBtnAccessQuotation();
            if (useItemForHotKeys)
            {
                f.CategoryName = ((Button)sender).Tag.ToString();
                
            }
            else
            {
                f.CatID = ((Guid)((Button)sender).Tag);
            }
            f.DisplayName = ((Button)sender).Text;
            f.useItemForHotKeys = useItemForHotKeys;
            f.pos = pos;
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
