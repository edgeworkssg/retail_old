﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SubSonic;
using SubSonic.Utilities;
using PowerPOS;
using System.Linq;

namespace PowerWeb.Scaffolds
{
    public partial class SAPCustomerCodeScaffold : PageBase
    {
        private bool isAdd = false;
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                string id = Utility.GetParameter("id");
                if (!String.IsNullOrEmpty(id) && id != "0")
                {
                    if (!Page.IsPostBack)
                    {
                        LoadEditor(id);
                    }
                }
                else
                {
                    //it's an add, show the editor
                    isAdd = true;
                    ToggleEditor(true);
                    if (!Page.IsPostBack)
                        LoadDrops();
                    btnDelete.Visible = false;
                }
            }
            else
            {
                ToggleEditor(false);
                if (!Page.IsPostBack)
                {
                    BindGrid(String.Empty);
                }
            }
        }

        /// <summary>
        /// Loads the editor with data
        /// </summary>
        /// <param name="id"></param>
        void LoadEditor(string id)
        {
            ToggleEditor(true);
            LoadDrops();
            if (!string.IsNullOrEmpty(id) && id != "0")
            {
                hdfID.Value = id.ToString();
                txtID.Text = id.ToString();

                //pull the record
                SAPCustomerCode item = new SAPCustomerCode(id);
                ddlSalesType.SelectedValue = item.SalesType;
                txtPaymentType.Text = item.PaymentType;
                ddlPOS.SelectedValue = item.PointOfSaleID.ToString();
                txtCustomerCode.Text = item.CustomerCode;
                ctrlCreatedBy.Text = item.CreatedBy;
                if (item.CreatedOn.HasValue)
                    ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();
                ctrlModifiedBy.Text = item.ModifiedBy;
                if (item.ModifiedOn.HasValue)
                    ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();
                ctrlDeleted.Checked = item.Deleted;
                //set the delete confirmation
                btnDelete.Attributes.Add("onclick", "return CheckDelete();");
            }
        }

        /// <summary>
        /// Loads the DropDownLists
        /// </summary>
        void LoadDrops()
        {
            //load the listboxes
            //ddlPOS.Items.Clear();
            //ddlPOS.Items.AddRange(PointOfSaleController.FetchPointOfSaleNames(false, Session["UserName"] + "", "ALL"));
            //ddlPOS.Items.Insert(0, new ListItem("-- Please select --", ""));
        }


        /// <summary>
        /// Shows/Hides the Grid and Editor panels
        /// </summary>
        /// <param name="showIt"></param>
        void ToggleEditor(bool showIt)
        {
            pnlEdit.Visible = showIt;
            pnlGrid.Visible = !showIt;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            LoadEditor("0");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            SAPCustomerCode.Delete(Utility.GetParameter("id"));
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("DELETE SAPCustomerCode : {0}", Utility.GetParameter("id")), "");
            //redirect
            Response.Redirect(Request.CurrentExecutionFilePath);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id = hdfID.Value;
            //bool haveError = false;
            try
            {
                BindAndSave(id);
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("SAPCustomerCode saved.") + "</span>";
            }
            catch (Exception x)
            {
                //haveError = true;
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("SAPCustomerCode not saved:") + "</span> " + x.Message;
            }
        }

        /// <summary>
        /// Binds and saves the data
        /// </summary>
        /// <param name="id"></param>
        void BindAndSave(string id)
        {
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                Query qr = SAPCustomerCode.CreateQuery();
                qr.QueryType = QueryType.Update;
                qr.AddWhere(SAPCustomerCode.Columns.Id, hdfID.Value);
                qr.AddUpdateSetting(SAPCustomerCode.Columns.SalesType, ddlSalesType.SelectedValue);
                qr.AddUpdateSetting(SAPCustomerCode.Columns.PaymentType, txtPaymentType.Text);
                qr.AddUpdateSetting(SAPCustomerCode.Columns.PointOfSaleID, ddlPOS.SelectedValue);
                qr.AddUpdateSetting(SAPCustomerCode.Columns.CustomerCode, txtCustomerCode.Text);
                qr.AddUpdateSetting(SAPCustomerCode.Columns.ModifiedBy, Session["Username"].ToString());
                qr.AddUpdateSetting(SAPCustomerCode.Columns.ModifiedOn, DateTime.Now);
                qr.AddUpdateSetting(SAPCustomerCode.Columns.Deleted, ctrlDeleted.Checked);
                qr.Execute();
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("UPDATE SAPCustomerCode : {0}", Utility.GetParameter("id")), "");
            }
            else
            {

                SAPCustomerCode ct = new SAPCustomerCode();
                ct.IsNew = true;
                ct.SalesType = ddlSalesType.SelectedValue;
                ct.PaymentType = txtPaymentType.Text;
                ct.PointOfSaleID = ddlPOS.SelectedValue.GetIntValue();
                ct.CustomerCode = txtCustomerCode.Text;
                ct.ModifiedBy = Session["Username"].ToString();
                ct.ModifiedOn = DateTime.Now;
                ct.CreatedBy = Session["Username"].ToString();
                ct.CreatedOn = DateTime.Now;
                ct.Deleted = false;
                ct.IsNew = true;
                ct.Save(Session["username"].ToString());
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("ADD SAPCustomerCode : {0}", txtID.Text), "");
            }
        }

        /// <summary>
        /// Binds the GridView
        /// </summary>
        private void BindGrid(string orderBy)
        {
            string sql = "SELECT sap.*, pos.PointOfSaleName FROM SAPCustomerCode sap LEFT JOIN PointOfSale pos ON pos.PointOfSaleID = sap.PointOfSaleID";
            DataTable dt = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS")).Tables[0];
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGrid(String.Empty);
        }


        protected void GridView1_DataBound(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = GridView1.BottomPagerRow;
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
                for (int i = 0; i < GridView1.PageCount; i++)
                {
                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());
                    if (i == GridView1.PageIndex)
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
                DataSet ds = GridView1.DataSource as DataSet;
                if (ds != null)
                {
                    itemCount = ds.Tables[0].Rows.Count;
                }

                string pageCount = "<b>" + GridView1.PageCount.ToString() + "</b> (" + itemCount.ToString() + " Items)";
                lblPageCount.Text = pageCount;
            }

            Button btnPrev = (Button)gvrPager.Cells[0].FindControl("btnPrev");
            Button btnNext = (Button)gvrPager.Cells[0].FindControl("btnNext");
            Button btnFirst = (Button)gvrPager.Cells[0].FindControl("btnFirst");
            Button btnLast = (Button)gvrPager.Cells[0].FindControl("btnLast");
            //now figure out what page we're on
            if (GridView1.PageIndex == 0)
            {
                btnPrev.Enabled = false;
                btnFirst.Enabled = false;
            }

            else if (GridView1.PageIndex + 1 == GridView1.PageCount)
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


        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = GridView1.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            GridView1.PageIndex = ddlPages.SelectedIndex;
            // a method to populate your grid
            BindGrid(String.Empty);
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            string columnName = e.SortExpression;
            //rebind the grid
            if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
            {
                ViewState[SORT_DIRECTION] = SqlFragment.DESC;
            }
            else
            {
                ViewState[SORT_DIRECTION] = SqlFragment.ASC;
            }
            BindGrid(columnName);
        }


        string GetSortDirection(string sortBy)
        {
            string sortDir = " ASC";
            if (ViewState["sortBy"] != null)
            {
                string sortedBy = ViewState["sortBy"].ToString();
                if (sortedBy == sortBy)
                {
                    //the direction should be desc
                    sortDir = " DESC";
                    //reset the sorter to null
                    ViewState["sortBy"] = null;
                }

                else
                {
                    //this is the first sort for this row
                    //put it to the ViewState
                    ViewState["sortBy"] = sortBy;
                }
            }
            else
            {
                //it's null, so this is the first sort
                ViewState["sortBy"] = sortBy;
            }
            return sortDir;
        }
    }
}
