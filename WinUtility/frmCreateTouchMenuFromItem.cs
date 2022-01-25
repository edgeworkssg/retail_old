using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;

namespace WinUtility
{
    public partial class frmCreateTouchMenuFromItem : Form
    {
        public frmCreateTouchMenuFromItem()
        {
            InitializeComponent();
        }

        private void txtGenerate_Click(object sender, EventArgs e)
        {
            //Create(txtDepartmentName.Text);
            ItemDepartmentCollection allDepartment = new ItemDepartmentCollection();
            ItemDepartmentController controlRoom = new ItemDepartmentController();
            allDepartment = controlRoom.FetchAll();

            for (int Counter = 0; Counter < allDepartment.Count; Counter++)
            {
                ItemDepartment oneDepartment = allDepartment[Counter];
                if (oneDepartment.ItemDepartmentID.ToLower() == "system") continue;
                if (oneDepartment.ItemDepartmentID.ToLower() != "product") continue;

                Create(oneDepartment.ItemDepartmentID);
            }

        }
        private void Create(string DepartmentName)
        {
            //Delete Quick Access Group 
            Query qr = QuickAccessGroup.CreateQuery();
            qr.QueryType = QueryType.Delete;
            qr.AddWhere(QuickAccessGroup.Columns.QuickAccessGroupName, DepartmentName);
            qr.Execute();

            //Create Quick Access Group
            QuickAccessGroup q = new QuickAccessGroup();
            q.QuickAccessGroupName = DepartmentName;
            q.Deleted = false;
            q.Save("System");

            string GroupID = q.GetPrimaryKeyValue().ToString();

            ItemDepartment it = new ItemDepartment(ItemDepartment.Columns.DepartmentName, DepartmentName);

            //Pull all category
            CategoryCollection cat = it.CategoryRecords();

            for (int i = 0; i < cat.Count; i++)
            {
                //Delete Same Quick Access Category
                /*
                qr = QuickAccessCategory.CreateQuery();
                qr.QueryType = QueryType.Delete;
                qr.AddWhere(QuickAccessCategory.Columns.QuickAccessCatName, cat[i].CategoryName);
                qr.Execute();
                */

                //
                QuickAccessCategory tmp = new QuickAccessCategory();
                tmp.IsNew = true;
                tmp.Deleted = false;
                int length = cat[i].CategoryName.Length;
                if (length > 50) length = 50;
                tmp.QuickAccessCatName = cat[i].CategoryName.Substring(0,length);
                tmp.BackColor = "LIGHTBLUE";
                tmp.ForeColor = "BLACK";
                tmp.PriorityLevel = i;
                tmp.IsNew = true;
                tmp.Save();

                string catID = tmp.GetPrimaryKeyValue().ToString();

                //Create new Map
                QuickAccessGroupMap tmp2 = new QuickAccessGroupMap();
                tmp2.IsNew = true;
                tmp2.Deleted = false;
                tmp2.QuickAccessCategoryID = new Guid(catID);
                tmp2.QuickAccessGroupID = new Guid(GroupID);
                tmp2.Save();

                //pull the buttons
                //ItemCollection itemCols = cat[i].ItemRecords();
                ItemCollection itemCols = new ItemCollection();
                itemCols.Where(Item.Columns.CategoryName, cat[i].CategoryName);
                itemCols.OrderByAsc("ItemName");
                itemCols.Load();

                for (int j = 0; j < itemCols.Count; j++)
                {
                    //Create Quick Access Button
                    QuickAccessButton qbutton = new QuickAccessButton();
                    qbutton.BackColor = "LIGHTBLUE";
                    qbutton.ForeColor = "BLACK";
                    qbutton.Deleted = false;
                    qbutton.ItemNo = itemCols[j].ItemNo;
                    //length = cat[i].CategoryName.Length;
                    length = itemCols[j].ItemName.Length;
                    if (length > 50) length = 50;

                    qbutton.Label = itemCols[j].ItemName.Substring(0,length);
                    qbutton.QuickAccessCategoryID = new Guid(catID);
                    qbutton.Save();
                } 
            }
            MessageBox.Show("Done");
        }
    }
}
