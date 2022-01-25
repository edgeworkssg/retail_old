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
using System.Threading;
using System.Globalization;

namespace WinPowerPOS.Setup
{
    public partial class frmPrivilegeSetup : Form
    {
        public frmPrivilegeSetup()
        {
            InitializeComponent();
        }
        private void LoadPrivilege(string role)
        {
            clearControls();
            //Control cb = this.Controls.Find("cb" + ,true);
            DataTable dt = UserController.FetchGroupPrivileges(role);
            
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                if (flowLayoutPanel1.Controls[i] is CheckBox)
                {
                    if (PrivilegesController.HasPrivilege(((CheckBox)flowLayoutPanel1.Controls[i]).Text, dt))
                    {
                        ((CheckBox)flowLayoutPanel1.Controls[i]).Checked = true;
                    }

                }
            }

            for (int i = 0; i < flowLayoutPanel2.Controls.Count; i++)
            {
                if (flowLayoutPanel2.Controls[i] is CheckBox)
                {
                    if (PrivilegesController.HasPrivilege(((CheckBox)flowLayoutPanel2.Controls[i]).Text, dt))
                    {
                        ((CheckBox)flowLayoutPanel2.Controls[i]).Checked = true;
                    }
                }
            }

            for (int i = 0; i < flowLayoutPanel3.Controls.Count; i++)
            {
                if (flowLayoutPanel3.Controls[i] is CheckBox)
                {
                    if (PrivilegesController.HasPrivilege(((CheckBox)flowLayoutPanel3.Controls[i]).Text, dt))
                    {
                        ((CheckBox)flowLayoutPanel3.Controls[i]).Checked = true;
                    }
                }
            }

            for (int i = 0; i < flowLayoutPanel4.Controls.Count; i++)
            {
                if (flowLayoutPanel4.Controls[i] is CheckBox)
                {
                    if (PrivilegesController.HasPrivilege(((CheckBox)flowLayoutPanel4.Controls[i]).Text, dt))
                    {
                        ((CheckBox)flowLayoutPanel4.Controls[i]).Checked = true;
                    }
                }
            }
            for (int i = 0; i < flowLayoutPanel5.Controls.Count; i++)
            {
                if (flowLayoutPanel5.Controls[i] is CheckBox)
                {
                    if (PrivilegesController.HasPrivilege(((CheckBox)flowLayoutPanel5.Controls[i]).Text, dt))
                    {
                        ((CheckBox)flowLayoutPanel5.Controls[i]).Checked = true;
                    }
                }
            }    
        }
        private void SavePrivilege(CheckBox cb, int GroupUserID, UserPrivilegeCollection col)
        {
            int PrivilegeID = -1;
            for (int i = 0; i < col.Count; i++)
            {
                if (col[i].PrivilegeName == cb.Text)
                {
                    PrivilegeID = col[i].UserPrivilegeID;
                    break;
                }
            }
            if (PrivilegeID != -1)
            {
                if (cb.Checked)
                {
                    Query qr = GroupUserPrivilege.CreateQuery();
                    int count = qr.WHERE(GroupUserPrivilege.Columns.GroupID, GroupUserID).
                        AND(GroupUserPrivilege.Columns.UserPrivilegeID, PrivilegeID).
                        AND(GroupUserPrivilege.Columns.Deleted,false).
                        GetCount(GroupUserPrivilege.Columns.GroupUserPrivilegeID);
                    //if exist already ignore
                    if (count <= 0)
                    {
                        //if not create
                        GroupUserPrivilege gr = new GroupUserPrivilege();
                        gr.GroupID = GroupUserID;
                        gr.UserPrivilegeID = PrivilegeID;
                        gr.Deleted = false;
                        gr.Save(UserInfo.username);
                    }
                }
                else
                {
                    //if not checked
                    //delete without checking
                    Query qr = GroupUserPrivilege.CreateQuery();
                    qr.QueryType = QueryType.Delete;
                    qr.WHERE(GroupUserPrivilege.Columns.GroupID, GroupUserID).
                        AND(GroupUserPrivilege.Columns.UserPrivilegeID, PrivilegeID).
                        Execute();                    
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            UserPrivilegeCollection col = new UserPrivilegeCollection();
            col.Where(UserPrivilege.Columns.Deleted, false);
            col.Load();

            int groupID = ((UserGroup)cmbUserGroup.SelectedValue).GroupID;
                
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                if (flowLayoutPanel1.Controls[i] is CheckBox)
                {
                    SavePrivilege( ((CheckBox)flowLayoutPanel1.Controls[i]), groupID, col);                    
                }
            }

            for (int i = 0; i < flowLayoutPanel2.Controls.Count; i++)
            {
                if (flowLayoutPanel2.Controls[i] is CheckBox)
                {
                    SavePrivilege(((CheckBox)flowLayoutPanel2.Controls[i]), groupID, col);                    
                }
            }

            for (int i = 0; i < flowLayoutPanel3.Controls.Count; i++)
            {
                if (flowLayoutPanel3.Controls[i] is CheckBox)
                {
                    SavePrivilege(((CheckBox)flowLayoutPanel3.Controls[i]), groupID, col);                    
                }
            }

            for (int i = 0; i < flowLayoutPanel4.Controls.Count; i++)
            {
                if (flowLayoutPanel4.Controls[i] is CheckBox)
                {
                    SavePrivilege(((CheckBox)flowLayoutPanel4.Controls[i]), groupID, col);                    
                }
            }
            for (int i = 0; i < flowLayoutPanel5.Controls.Count; i++)
            {
                if (flowLayoutPanel5.Controls[i] is CheckBox)
                {
                    SavePrivilege(((CheckBox)flowLayoutPanel5.Controls[i]), groupID, col);
                }
            }
            MessageBox.Show("Save completed.");
        }
        private void clearControls()
        {
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                if (flowLayoutPanel1.Controls[i] is CheckBox)
                {
                    ((CheckBox)flowLayoutPanel1.Controls[i]).Checked = false;
                }
            }

            for (int i = 0; i < flowLayoutPanel2.Controls.Count; i++)
            {
                if (flowLayoutPanel2.Controls[i] is CheckBox)
                {
                    ((CheckBox)flowLayoutPanel2.Controls[i]).Checked = false;
                }
            }

            for (int i = 0; i < flowLayoutPanel3.Controls.Count; i++)
            {
                if (flowLayoutPanel3.Controls[i] is CheckBox)
                {
                    ((CheckBox)flowLayoutPanel3.Controls[i]).Checked = false;
                }
            }

            for (int i = 0; i < flowLayoutPanel4.Controls.Count; i++)
            {
                if (flowLayoutPanel4.Controls[i] is CheckBox)
                {
                    ((CheckBox)flowLayoutPanel4.Controls[i]).Checked = false;
                }
            }
            for (int i = 0; i < flowLayoutPanel5.Controls.Count; i++)
            {
                if (flowLayoutPanel5.Controls[i] is CheckBox)
                {
                    ((CheckBox)flowLayoutPanel5.Controls[i]).Checked = false;
                }
            }    
        }

        private void frmPrivilegeSetup_Load(object sender, EventArgs e)
        {
            UserGroupCollection fr = new UserGroupCollection();
            fr.Where(UserGroup.Columns.Deleted,false);
            fr.Load();
            cmbUserGroup.DataSource = fr;
            cmbUserGroup.Refresh();
            cmbUserGroup.SelectedIndex = 0;           
            LoadPrivilege(((UserGroup)cmbUserGroup.SelectedValue).GroupName);
        }

        private void cmbUserGroup_SelectedIndexChanged(object sender, EventArgs e)
        {            
            LoadPrivilege(((UserGroup)cmbUserGroup.SelectedValue).GroupName);            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
