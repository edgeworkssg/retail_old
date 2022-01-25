using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using PowerPOS;
using SubSonic;

namespace PowerWeb.Support
{
    public partial class DashboardSetup : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
                BindGrid();
            lblResult.Text = "";
        }

        #region *) Form

        public PowerPOS.Dashboard FormData
        {
            set
            {
                hdID.Value = value.Id.ToString();
                txtTitle.Text = value.Title;
                txtSubTitle.Text = value.SubTitle;
                if(!string.IsNullOrEmpty(value.PlotType)) 
                    ddlPlotType.SelectedValue = value.PlotType;
                txtPlotOption.Text = value.PlotOption;
                txtWidth.Text = value.Width;
                txtHeight.Text = value.Height;
                txtSQLString.Text = value.SQLString;
                chkIsInLine.Checked = value.IsInline.GetValueOrDefault(true);
                chkBreakAfter.Checked = value.BreakAfter.GetValueOrDefault(false);
                chkBreakBefore.Checked = value.BreakBefore.GetValueOrDefault(false);
                txtDisplayOrder.Text = value.DisplayOrder.GetValueOrDefault(0).ToString();
                chkEnable.Checked = value.IsEnable.GetValueOrDefault(true);
                btnDelete.Visible = value.Id != 0;
            }
            get
            {
                var data = new PowerPOS.Dashboard(PowerPOS.Dashboard.Columns.Id, hdID.Value.GetIntValue());
                data.Title = txtTitle.Text;
                data.SubTitle = txtSubTitle.Text;
                data.PlotType = (ddlPlotType.SelectedValue + "").Trim();
                data.PlotOption = txtPlotOption.Text;
                data.Width = txtWidth.Text;
                data.Height = txtHeight.Text;
                data.SQLString = txtSQLString.Text;
                data.IsInline = chkIsInLine.Checked;
                data.BreakAfter = chkBreakAfter.Checked;
                data.BreakBefore = chkBreakBefore.Checked;
                data.DisplayOrder = txtDisplayOrder.Text.GetIntValue();
                data.IsEnable = chkEnable.Checked;
                data.Deleted = false;
                if(data.IsNew)  
                    data.UniqueID = Guid.NewGuid();
                return data;
            }
        }

        #endregion

        #region *) Method

        private void BindGrid()
        {
            CloseForm();
            string sql = @"SELECT  DH.ID
		                        ,DH.Title
		                        ,DH.SubTitle
		                        ,DH.PlotType
		                        ,DH.Width
		                        ,DH.Height
		                        ,ISNULL(DH.IsEnable,0) IsEnable
		                        ,DH.DisplayOrder
		                        ,CASE WHEN ISNULL(DH.IsEnable,0) = 1 THEN 'Disable' ELSE 'Enable' END Disabled
		                        ,DH.IsInline
		                        ,DH.BreakAfter
		                        ,DH.BreakBefore
                        FROM	Dashboard DH
                        WHERE	ISNULL(DH.Deleted,0) = 0
                        ORDER BY ISNULL(DH.IsEnable,0) DESC
		                        ,DH.DisplayOrder";
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            gvDashboard.DataSource = dt;
            gvDashboard.DataBind();
        }

        private void CloseForm()
        {
            pnlForm.Visible = false;
            pnlGrid.Visible = true;
        }

        private void OpenForm(int id)
        {
            pnlForm.Visible = true;
            pnlGrid.Visible = false;

            PowerPOS.Dashboard data = new PowerPOS.Dashboard(PowerPOS.Dashboard.Columns.Id, id);
            FormData = data;
        }

        private void SaveData()
        {
            var data = FormData;
            try
            {
                if(!data.IsNew)
                    DataService.ExecuteQuery(data.GetUpdateCommand(Session["UserName"] + ""));
                else
                    DataService.ExecuteQuery(data.GetInsertCommand(Session["UserName"] + ""));
                lblResult.Text = "Save data success";
            }
            catch (Exception ex)
            {
                lblResult.Text = "Save data failed : " + ex.Message;
                Logger.writeLog(ex);
            }
        }

        public void DeleteData(int id)
        {
            PowerPOS.Dashboard data = new PowerPOS.Dashboard(PowerPOS.Dashboard.Columns.Id, id);
            if (!data.IsNew)
            {
                data.Deleted = true;
                try
                {
                    DataService.ExecuteQuery(data.GetUpdateCommand(Session["UserName"] + ""));
                    lblResult.Text = "Delete data success";
                    BindGrid();
                }
                catch (Exception ex)
                {
                    lblResult.Text = "Delete data failed : " + ex.Message;
                    Logger.writeLog(ex);
                }
            }
        }

        public void DisableData(int id)
        {
            PowerPOS.Dashboard data = new PowerPOS.Dashboard(PowerPOS.Dashboard.Columns.Id, id);
            if (!data.IsNew)
            {
                data.IsEnable = !data.IsEnable.GetValueOrDefault(true);
                try
                {
                    DataService.ExecuteQuery(data.GetUpdateCommand(Session["UserName"] + ""));
                    lblResult.Text = "Delete data success";
                    BindGrid();
                }
                catch (Exception ex)
                {
                    lblResult.Text = "Delete data failed : " + ex.Message;
                    Logger.writeLog(ex);
                }
            }
        }

        #endregion

        #region *) Event Handler

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            OpenForm(0);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteData(hdID.Value.GetIntValue());
        }

        protected void gvDashboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDashboard.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvDashboard_DataBound(object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvDashboard.BottomPagerRow;
            if (gvrPager == null)
            {
                return;
            }

            // get your controls from the gridview
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");
            if (ddlPages != null)
            {
                // populate pager
                for (int i = 0; i < gvDashboard.PageCount; i++)
                {
                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());
                    if (i == gvDashboard.PageIndex)
                    {
                        lstItem.Selected = true;
                    }

                    ddlPages.Items.Add(lstItem);
                }

            }

            int itemCount = 0;
            // populate page count
            if (lblPageCount != null)
            {
                //pull the datasource
                DataSet ds = gvDashboard.DataSource as DataSet;
                if (ds != null)
                {
                    itemCount = ds.Tables[0].Rows.Count;
                }

                string pageCount = "<b>" + gvDashboard.PageCount.ToString() + "</b> (" + itemCount.ToString() + " Items)";
                lblPageCount.Text = pageCount;
            }

            Button btnPrev = (Button)gvrPager.Cells[0].FindControl("btnPrev");
            Button btnNext = (Button)gvrPager.Cells[0].FindControl("btnNext");
            Button btnFirst = (Button)gvrPager.Cells[0].FindControl("btnFirst");
            Button btnLast = (Button)gvrPager.Cells[0].FindControl("btnLast");
            //now figure out what page we're on
            if (gvDashboard.PageIndex == 0)
            {
                btnPrev.Enabled = false;
                btnFirst.Enabled = false;
            }

            else if (gvDashboard.PageIndex + 1 == gvDashboard.PageCount)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }

            else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
                btnPrev.Enabled = true;
                btnFirst.Enabled = true;
            }
        }

        protected void gvDashboard_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Disable")
            {
                DisableData(e.CommandArgument.ToString().GetIntValue());
            }
            else if (e.CommandName == "EditData")
            {
                OpenForm(e.CommandArgument.ToString().GetIntValue());
            }
        }

        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvDashboard.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvDashboard.PageIndex = ddlPages.SelectedIndex;
            BindGrid();
        }

        #endregion
    }
}
