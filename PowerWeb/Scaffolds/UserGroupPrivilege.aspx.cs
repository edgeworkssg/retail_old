using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PowerPOS;
using SubSonic.Utilities;
using SubSonic;
using AjaxControlToolkit;
using System.Collections.Generic;

public partial class UserGroupPrivilege : PageBase
{
    //AjaxControlToolkit.TabContainer tbcDynamic;
    AjaxControlToolkit.Accordion myAccordion;
 
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            BindPrivileges();
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex.Message);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            DropDown1.SelectedIndex = 0;
       }
    }

    protected void BindPrivileges()
    {
        string sql = @"     select distinct CASE WHEN ISNULL(Userfld1,'') = '' THEN 10000 else 1 end as Ind, 
                            CASE WHEN ISNULL(Userfld1,'') = '' THEN 'Misc' else userfld1 end as Text, 
                            CASE WHEN ISNULL(Userfld1,'') = '' THEN 'Misc' else userfld1 end as Value from userprivilege WHERE ISNULL(Deleted,0) = 0 ";

        
        string sql2 = "";

        sql2 = "SELECT DISTINCT CASE WHEN ISNULL(Userfld1,'') = '' THEN 'Misc' else userfld1 end as Category, " + 
                    " UserPrivilegeID, PrivilegeName, 0 as Selected " +
                    " FROM UserPrivilege up " +
                    " WHERE ISNULL(up.Deleted,0) = 0 ";

        string POSType = AppSetting.GetSetting(AppSetting.SettingsName.POSType);

        if (string.IsNullOrEmpty(POSType))
            POSType = "Retail";

        if (POSType.ToLower().Equals("beauty"))
        {
            sql += " AND ISNULL(userflag4,0) = 1 ";
            sql2 += " AND ISNULL(up.userflag4,0) = 1 ";
        }
        else if (POSType.ToLower().Equals("wholesalse"))
        {
            sql += " AND ISNULL(userflag4,0) = 1 ";
            sql2 += " AND ISNULL(up.userflag3,0) = 1 ";
        }
        else
        {
            sql += " AND ISNULL(userflag4,0) = 1 ";
            sql2 += " AND ISNULL(up.userflag2,0) = 1 ";
        }

        sql += " order by Ind, Text ";
        sql2 += "order by up.PrivilegeName ";

        DataTable dt = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];
        DataTable dt2 = DataService.GetDataSet(new QueryCommand(sql2)).Tables[0];


        if (dt.Rows.Count > 0)
        {
            //tbcDynamic = new AjaxControlToolkit.TabContainer();
            myAccordion = new AjaxControlToolkit.Accordion();
            myAccordion.ID = "PrivilegesAccordion";
            myAccordion.SelectedIndex = 0;
            myAccordion.FadeTransitions = true;
            myAccordion.HeaderCssClass = "accordionHeader";
            myAccordion.HeaderSelectedCssClass = "accordionHeaderSelected";
            myAccordion.ContentCssClass = "accordionContent";
            myAccordion.AutoSize = AutoSize.Limit;            
            myAccordion.TransitionDuration= 250;
            myAccordion.FramesPerSecond= 40;
            myAccordion.RequireOpenedPane=false;
            myAccordion.SuppressHeaderPostbacks=true;
            myAccordion.Width = 800;
            //myAccordion.Height = 200;

            for (int i = 0; i<dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                var drw2 = dt2.Select("Category = '" + dr["Value"].ToString() + "'");

                if (drw2.Length > 0)
                {
                    //TabPanel pnl = new TabPanel();
                    //pnl.HeaderText = dr["Text"].ToString();
                    //pnl.ID = dr["Value"].ToString();
                    //tbcDynamic.Tabs.Add(pnl);
                    AccordionPane ap1 = new AccordionPane();
                    ap1.ID = dr["Value"].ToString();
                    ap1.HeaderContainer.Controls.Add(new LiteralControl(dr["Text"].ToString()));
                    ap1.HeaderCssClass = "accordionHeader";
                    
                    string category = dr["Text"].ToString();
                    CheckBoxList cblPrivileges = new CheckBoxList();
                    cblPrivileges.ID = "cbl" + i.ToString();
                    cblPrivileges.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Vertical;
                    cblPrivileges.RepeatLayout = RepeatLayout.Table;


                    var drP = dt2.Select("Category = '" + category + "'");
                    if (drP.Length <= 40)
                    {
                        cblPrivileges.RepeatColumns = (int)Math.Ceiling((decimal)drP.Length / 20);
                    }
                    else
                    {
                        cblPrivileges.RepeatColumns = 3;
                    }

                    for (int j = 0; j < drP.Length; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = drP[j]["PrivilegeName"].ToString();
                        li.Value = drP[j]["UserPrivilegeID"].ToString();
                        li.Selected = drP[j]["Selected"].ToString() != "0";

                        cblPrivileges.Items.Add(li);
                    }
                    ap1.ContentContainer.Controls.Add(cblPrivileges);
                    ap1.ContentCssClass = "accordionContent";
                    
                    myAccordion.Panes.Add(ap1);
                }
            }

            divPrivileges.Controls.Add(myAccordion);

            //for (int i = 0; i < tbcDynamic.Tabs.Count; i++)
            //{
            //    string category = tbcDynamic.Tabs[i].HeaderText.ToString();
            //    CheckBoxList cblPrivileges = new CheckBoxList();
            //    cblPrivileges.ID = "cbl" + i.ToString();
            //    cblPrivileges.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Vertical;
            //    cblPrivileges.RepeatLayout = RepeatLayout.Table;
                

            //    var drP = dt2.Select("Category = '" + category + "'");
            //    if (drP.Length <= 40)
            //    {
            //        cblPrivileges.RepeatColumns = (int)Math.Ceiling((decimal)drP.Length / 20);
            //    }
            //    else
            //    {
            //        cblPrivileges.RepeatColumns = 3;
            //    }

            //    for (int j = 0; j < drP.Length; j++)
            //    {
            //        ListItem li = new ListItem();
            //        li.Text = drP[j]["PrivilegeName"].ToString();
            //        li.Value = drP[j]["UserPrivilegeID"].ToString();
            //        li.Selected = drP[j]["Selected"].ToString() != "0";

            //        cblPrivileges.Items.Add(li);
            //    }
            //    tbcDynamic.Tabs[i].Controls.Add(cblPrivileges);
            //}
            //tbcDynamic.ID = "tbDynamic";

           
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DropDown1.SelectedIndex != 0)
        {
            //ManyManyList1.PrimaryKeyValue = DropDown1.SelectedValue;
            //ManyManyList1.Save();
            lblMessage.Text = "";
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("UPDATE UserGroupProvileges : {0}", SubSonic.Utilities.Utility.GetParameter("id")), "");
            try
            {
                //GroupUserPrivilegeCollection gupColl = new GroupUserPrivilegeCollection();
                //gupColl.Where(GroupUserPrivilege.Columns.GroupID, DropDown1.SelectedValue);
                //gupColl.Load();
                QueryCommandCollection qmc = new QueryCommandCollection();
                //for (int i = 0; i < gupColl.Count; i++)
                //{
                //    var theGUP = gupColl[i];
                //    theGUP.DirtyColumns.Add(GroupUserPrivilege.GroupIDColumn);
                //    qmc.Add(theGUP.GetUpdateCommand(Session["UserName"] + ""));
                //}

                int GroupID = DropDown1.SelectedValue.ToString().GetIntValue();

                //for (int j = 0; j < cblPrivileges.Items.Count; j++)
                //{
                //    if (cblPrivileges.Items[j].Selected)
                //    {

                //        GroupUserPrivilegeCollection col = new GroupUserPrivilegeCollection();
                //        col.Where(GroupUserPrivilege.Columns.UserPrivilegeID, cblPrivileges.Items[j].Value);
                //        col.Where(GroupUserPrivilege.Columns.GroupID, GroupID);
                //        col.Load();

                //        if (col.Count == 0)
                //        {
                //            GroupUserPrivilege gup = new GroupUserPrivilege();
                //            gup.GroupID = GroupID;
                //            gup.UserPrivilegeID = cblPrivileges.Items[j].Value.GetIntValue();
                //            qmc.Add(gup.GetInsertCommand(Session["UserName"] + ""));
                //        }
                //    }
                //    else
                //    {
                //        //remove privileges
                //        string removepv = "Delete from GroupUserPrivilege where groupid = " + GroupID + " and UserPrivilegeID = " + cblPrivileges.Items[j].Value;
                //        qmc.Add(new QueryCommand(removepv));
                //    }
                //}

                for (int i = 0; i < myAccordion.Panes.Count; i++)
                {
                    CheckBoxList cblPrivileges = (CheckBoxList)myAccordion.Panes[i].FindControl("cbl" + i.ToString());
                    for (int j = 0; j < cblPrivileges.Items.Count; j++)
                    {
                        if (cblPrivileges.Items[j].Selected)
                        {

                            GroupUserPrivilegeCollection col = new GroupUserPrivilegeCollection();
                            col.Where(GroupUserPrivilege.Columns.UserPrivilegeID, cblPrivileges.Items[j].Value);
                            col.Where(GroupUserPrivilege.Columns.GroupID, GroupID);
                            col.Load();

                            if (col.Count == 0)
                            {
                                GroupUserPrivilege gup = new GroupUserPrivilege();
                                gup.GroupID = GroupID;
                                gup.UserPrivilegeID = cblPrivileges.Items[j].Value.GetIntValue();
                                qmc.Add(gup.GetInsertCommand(Session["UserName"] + ""));
                            }
                        }
                        else
                        {
                            //remove privileges
                            string removepv = "Delete from GroupUserPrivilege where groupid = " + GroupID + " and UserPrivilegeID = " + cblPrivileges.Items[j].Value;
                            qmc.Add(new QueryCommand(removepv));
                        }
                    }
                }

                GroupUserPrivilegeCollection gupColl = new GroupUserPrivilegeCollection();
                gupColl.Where(GroupUserPrivilege.Columns.GroupID, DropDown1.SelectedValue);
                gupColl.Load();
                for (int i = 0; i < gupColl.Count; i++)
                {
                        var theGUP = gupColl[i];
                        theGUP.DirtyColumns.Add(GroupUserPrivilege.GroupIDColumn);
                        qmc.Add(theGUP.GetUpdateCommand(Session["UserName"] + ""));
                }


                DataService.ExecuteTransaction(qmc);

                lblMessage.Text = "Privileges has been saved";
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                lblMessage.Text = "Error :" + ex.Message;
            }
        }
    }

    protected void ddlPrivilegeCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindManyManyList();
    }

    protected void DropDown1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDown1.SelectedIndex != 0)
        {
            //ManyManyList1.Items.Clear();
            //ManyManyList1.PrimaryKeyValue = DropDown1.SelectedValue;

            //BindManyManyList();
            BindCheckBoxListPrivileges();
        }
    }

    //public void BindManyManyList()
    //{
    //    string sql = "";
    //    if (DropDown1.SelectedIndex != 0)
    //    {
    //        sql = "SELECT DISTINCT up.UserPrivilegeID, up.PrivilegeName, ISNULL(gup.GroupID,0) as Selected " +
    //               "FROM UserPrivilege up left join GroupUserPrivilege gup  on gup.UserPrivilegeID = up.UserPrivilegeID  AND gup.GroupID = " + DropDown1.SelectedValue + " " +
    //               "where ISNULL(up.Deleted,0) = 0 ";
    //    }
    //    else
    //    {
    //        sql = "SELECT DISTINCT UserPrivilegeID, PrivilegeName, 0 as Selected " +
    //                       " FROM UserPrivilege up " +
    //                      "where ISNULL(up.Deleted,0) = 0 ";
    //    }

    //    string POSType = AppSetting.GetSetting(AppSetting.SettingsName.POSType);

    //    if (string.IsNullOrEmpty(POSType))
    //        POSType = "Retail";

    //    if (POSType.ToLower().Equals("beauty"))
    //        sql += " AND ISNULL(up.userflag4,0) = 1 ";
    //    else if (POSType.ToLower().Equals("restaurant"))
    //        sql += " AND ISNULL(up.userflag3,0) = 1 ";
    //    else
    //        sql += " AND ISNULL(up.userflag2,0) = 1 ";

    //    if (ddlPrivilegeCategory.SelectedValue != "")
    //        sql += "AND ISNULL(up.userfld1,'') = '" + ddlPrivilegeCategory.SelectedValue + "'";

    //    sql += "order by up.PrivilegeName ";

    //    DataTable dt = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];
    //    cblPrivileges.Items.Clear();
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        ListItem li = new ListItem();
    //        li.Text = dt.Rows[i]["PrivilegeName"].ToString();
    //        li.Value = dt.Rows[i]["UserPrivilegeID"].ToString();
    //        li.Selected = dt.Rows[i]["Selected"].ToString() != "0";

    //        cblPrivileges.Items.Add(li);
    //    }

    //    //if (DropDown1.SelectedIndex != 0)
    //    //{
    //    //    ManyManyList1.PrimaryKeyValue = DropDown1.SelectedValue;
    //    //}
    //    //ManyManyList1.DataSource = dt;
    //    //ManyManyList1.DataTextField = "PrivilegeName";
    //    //ManyManyList1.DataValueField = "UserPrivilegeID";
    //    //ManyManyList1.DataBind();
    //}

    protected void BtnSelectAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < myAccordion.Panes.Count; i++)
        {
            CheckBoxList cblPrivileges = (CheckBoxList)myAccordion.Panes[i].FindControl("cbl" + i.ToString());
            for (int j = 0; j < cblPrivileges.Items.Count; j++)
            {
                cblPrivileges.Items[j].Selected = true;
            }
        }
    }

    protected void BtnClearSelection_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < myAccordion.Panes.Count; i++)
        {
            CheckBoxList cblPrivileges = (CheckBoxList)myAccordion.Panes[i].FindControl("cbl" + i.ToString());
            for (int j = 0; j < cblPrivileges.Items.Count; j++)
            {
                cblPrivileges.Items[j].Selected = false;
            }
        }
    }

    protected void BindCheckBoxListPrivileges()
    {
        string sql = "";

            if (DropDown1.SelectedIndex != 0)
            {
                sql = "SELECT DISTINCT up.UserPrivilegeID, up.PrivilegeName, ISNULL(gup.GroupID,0) as Selected " +
                       "FROM UserPrivilege up left join GroupUserPrivilege gup  on gup.UserPrivilegeID = up.UserPrivilegeID  AND gup.GroupID = " + DropDown1.SelectedValue + " " +
                       "where ISNULL(up.Deleted,0) = 0 ";
            }
            else
            {
                sql = "SELECT DISTINCT UserPrivilegeID, PrivilegeName, 0 as Selected " +
                          " FROM UserPrivilege up " +
                         "where ISNULL(up.Deleted,0) = 0 ";
            }

            string POSType = AppSetting.GetSetting(AppSetting.SettingsName.POSType);

            if (string.IsNullOrEmpty(POSType))
                POSType = "Retail";

            if (POSType.ToLower().Equals("beauty"))
                sql += " AND ISNULL(up.userflag4,0) = 1 ";
            else if (POSType.ToLower().Equals("restaurant"))
                sql += " AND ISNULL(up.userflag3,0) = 1 ";
            else
                sql += " AND ISNULL(up.userflag2,0) = 1 ";

            sql += "order by up.PrivilegeName ";

            DataTable dt = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];

            for (int i = 0; i < myAccordion.Panes.Count; i++)
            {
                string category = myAccordion.Panes[i].ID.ToString();
                CheckBoxList cblPrivileges = (CheckBoxList)myAccordion.Panes[i].FindControl("cbl" + i.ToString());

                if (cblPrivileges != null)
                {
                    for (int j = 0; j < cblPrivileges.Items.Count; j++)
                    {
                        var id = cblPrivileges.Items[j].Value.ToString();

                        var dr = dt.Select("UserPrivilegeID = " + id + " and Selected <> 0");

                        if (dr.Length > 0)
                            cblPrivileges.Items[j].Selected = true;
                        else
                            cblPrivileges.Items[j].Selected = false;
                    }
                }
            }
    }
}
