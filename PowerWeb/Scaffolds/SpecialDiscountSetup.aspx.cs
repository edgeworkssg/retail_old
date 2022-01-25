using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
    public partial class SpecialDiscountSetup : PageBase
    {
        private bool isAdd = false;
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        protected void Page_Load(object sender, EventArgs e)
        {
            SetFormSetting();
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

        private void SetFormSetting()
        {
            try
            {
                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                GridView1.Columns[8].HeaderText = outletText;
                lblOutlet.Text = outletText;
                ddlMultiOutlet.Texts.SelectBoxCaption = "Select " + outletText;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        /// <summary>
        /// Loads the editor with data
        /// </summary>
        /// <param name="id"></param>
        void LoadEditor(string id)
        {
            txtDiscName.Enabled = true;
            ToggleEditor(true);
            LoadDrops();
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                txtDiscName.Text = id.ToString();
                txtDiscName.Enabled = false;
                //pull the record
                SpecialDiscount item = new SpecialDiscount(id);

                txtStartDate.Text = item.StartDate.GetValueOrDefault(DateTime.Now).ToString("dd MMM yyyy");
                txtEndDate.Text = item.EndDate.GetValueOrDefault(DateTime.Now).ToString("dd MMM yyyy");
                txtDiscPercentage.Text = item.DiscountPercentage.ToString("N2");
                chkShowLabel.Checked = item.ShowLabel;
                txtPriorityLevel.Text = item.PriorityLevel.ToString();
                txtDiscountLabel.Text = item.DiscountLabel;
                chkEnabled.Checked = item.Enabled.GetValueOrDefault(false);

                try
                {
                    string[] selOutlet = item.AssignedOutletList;
                    for (int i = 0; i < ddlMultiOutlet.Items.Count; i++)
                        ddlMultiOutlet.Items[i].Selected = selOutlet.Contains(ddlMultiOutlet.Items[i].Value);
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }

                ctrlCreatedBy.Text = item.CreatedBy;
                if (item.CreatedOn.HasValue)
                    ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();
                ctrlModifiedBy.Text = item.ModifiedBy;
                if (item.ModifiedOn.HasValue)
                    ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();
                if (item.Deleted.HasValue)
                    ctrlDeleted.Checked = item.Deleted.Value;

                //set the delete confirmation
                btnDelete.Attributes.Add("onclick", "return CheckDelete();");

                #region *) FOR P1 - P5
                if (id.ToUpper() == "P1" || id.ToUpper() == "P2" || id.ToUpper() == "P3" || id.ToUpper() == "P4" || id.ToUpper() == "P5")
                {
                    txtStartDate.Enabled = false;
                    txtEndDate.Enabled = false;
                    imgStartDate.Visible = false;
                    imgEndDate.Visible = false;
                    txtDiscPercentage.Enabled = false;
                    chkShowLabel.Enabled = false;
                    txtPriorityLevel.Enabled = false;
                    txtDiscountLabel.Enabled = false;
                    chkEnabled.Enabled = false;
                    ctrlDeleted.Enabled = false;
                    btnDelete.Visible = false;
                }
                #endregion
            }
            else
            {
                txtStartDate.Text = DateTime.Now.ToString("dd MM yyyy");
                txtEndDate.Text = DateTime.Now.AddYears(100).ToString("dd MM yyyy");
            }

        }

        /// <summary>
        /// Loads the DropDownLists
        /// </summary>
        void LoadDrops()
        {
            //load the listboxes
            var qr = new Query("Outlet");
            qr.AddWhere(Outlet.Columns.Deleted, Comparison.Equals, 0);
            var ouList = new OutletController()
                        .FetchByQuery(qr)
                        .OrderByDescending(o => o.OutletName)
                        .ToList();

            ddlMultiOutlet.Items.Clear();
            foreach (var ou in ouList)
                ddlMultiOutlet.Items.Add(new ListItem { Value = ou.OutletName, Text = ou.OutletName });
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
            SpecialDiscount.Delete(Utility.GetParameter("id"));
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("DELETE SpecialDiscount : {0}", Utility.GetParameter("id")), "");
            //redirect
            Response.Redirect(Request.CurrentExecutionFilePath);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id = txtDiscName.Text;
            try
            {
                BindAndSave(id);
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Discount saved.") + "</span>";
            }
            catch (Exception x)
            {
                //haveError = true;
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Discount not saved:") + "</span> " + x.Message;
            }
        }

        /// <summary>
        /// Binds and saves the data
        /// </summary>
        /// <param name="id"></param>
        void BindAndSave(string id)
        {
            if (string.IsNullOrEmpty(txtDiscName.Text))
                throw new Exception("Discount Name must be filled");

            if (!(id.ToUpper() == "P1" || id.ToUpper() == "P2" || id.ToUpper() == "P3" || id.ToUpper() == "P4" || id.ToUpper() == "P5"))
            {
                if (txtDiscPercentage.Text.GetDecimalValue() <= 0)
                    throw new Exception("Discount percentage must bigger than 0");

                if (txtDiscPercentage.Text.GetDecimalValue() > 100)
                    throw new Exception("Discount percentage cannot bigger than 100");
            }
            SpecialDiscount item = new SpecialDiscount();
            item = new SpecialDiscount(SpecialDiscount.Columns.DiscountName, txtDiscName.Text);
            if (txtDiscName.Enabled && !item.IsNew)
                throw new Exception("Discount Name Already Exist");
            else if (!txtDiscName.Enabled && item.IsNew)
                throw new Exception("Discount Name Already Exist");
            if (item.IsNew)
            {
                item.DiscountName = txtDiscName.Text;
                item.SpecialDiscountID = Guid.NewGuid();
            }
            item.StartDate = txtStartDate.Text.GetDateTimeValue("dd MMM yyyy");
            item.EndDate = txtEndDate.Text.GetDateTimeValue("dd MMM yyyy");
            item.DiscountPercentage = txtDiscPercentage.Text.GetDecimalValue();
            item.ShowLabel = chkShowLabel.Checked;
            item.PriorityLevel = txtPriorityLevel.Text.GetIntValue();
            item.DiscountLabel = txtDiscountLabel.Text;
            item.Enabled = chkEnabled.Checked;
            item.Deleted = ctrlDeleted.Checked;
            item.ApplicableToAllItem = true;
            item.MinimumSpending = 0;
            item.IsBankPromo = false;

            var selOutlet = new List<string>();
            for (int i = 0; i < ddlMultiOutlet.Items.Count; i++)
            {
                if (ddlMultiOutlet.Items[i].Selected)
                    selOutlet.Add(ddlMultiOutlet.Items[i].Value);
            }
            if (selOutlet.Count == ddlMultiOutlet.Items.Count)
            {
                selOutlet.Clear();
                selOutlet.Add("ALL");
            }
            item.AssignedOutletList = selOutlet.ToArray();

            item.Save(Session["UserName"] + "");
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("UPDATE Discount : {0}", txtDiscName.Text), "");
            //bind it
        }

        /// <summary>
        /// Binds the GridView
        /// </summary>
        private void BindGrid(string orderBy)
        {
            DataTable dt;
            if (orderBy == string.Empty)
                orderBy = "DiscountName";

            //Query qr = new Query("SpecialDiscounts");
            //SpecialDiscountCollection sp = new SpecialDiscountCollection();
            //sp.Where(SpecialDiscount.Columns.DiscountName, SubSonic.Comparison.NotEquals, "P1");
            //sp.Where(SpecialDiscount.Columns.DiscountName, SubSonic.Comparison.NotEquals, "P2");
            //sp.Where(SpecialDiscount.Columns.DiscountName, SubSonic.Comparison.NotEquals, "P3");
            //sp.Where(SpecialDiscount.Columns.DiscountName, SubSonic.Comparison.NotEquals, "P4");
            //sp.Where(SpecialDiscount.Columns.DiscountName, SubSonic.Comparison.NotEquals, "P5");

            //if (ViewState[SORT_DIRECTION] == SqlFragment.ASC)
            //    dt = sp.OrderByAsc(orderBy).Load().ToDataTable();
            //else
            //    dt = sp.OrderByDesc(orderBy).Load().ToDataTable();

            string sql = @"
                            SELECT * FROM SpecialDiscounts 
                            WHERE ISNULL(Deleted, 0) = 0
                                AND (DiscountName <> 'P1' OR (DiscountName = 'P1' AND ISNULL(Enabled, 0) = 1))
                                AND (DiscountName <> 'P2' OR (DiscountName = 'P2' AND ISNULL(Enabled, 0) = 1))
                                AND (DiscountName <> 'P3' OR (DiscountName = 'P3' AND ISNULL(Enabled, 0) = 1))
                                AND (DiscountName <> 'P4' OR (DiscountName = 'P4' AND ISNULL(Enabled, 0) = 1))
                                AND (DiscountName <> 'P5' OR (DiscountName = 'P5' AND ISNULL(Enabled, 0) = 1))
                            ORDER BY {0} {1} 
                          ";

            if (ViewState[SORT_DIRECTION] == SqlFragment.DESC)
                sql = string.Format(sql, orderBy, "DESC");
            else
                sql = string.Format(sql, orderBy, "ASC");

            dt = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS")).Tables[0];
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
