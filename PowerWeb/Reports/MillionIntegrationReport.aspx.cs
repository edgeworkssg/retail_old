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
using PowerPOS.Container;

using PowerPOS;
using SubSonic;
using SubSonic.Utilities;

namespace PowerWeb.Reports
{
    public partial class MillionIntegrationReport : PageBase
    {
        private const int QTY = 4;
        private const int AMOUNT = 5;
        private const int WEIGHT = 6;
        private const int UOM = 7;
        private const int COGS = 8;
        private const int PL = 9;
        private const int PLPercent = 10;

        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Server.ScriptTimeout = 180;

                lblYear.Text = DateTime.Today.Year.ToString();
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(true));
                ddlOutlet.DataBind();
                ddCategory.Items.AddRange(ItemController.FetchCategoryNamesInListItem());
                ddCategory.DataBind();
                txtStartDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                ViewState["sortBy"] = "";
                ViewState[SORT_DIRECTION] = "";
                //BindGrid();
            }
        }
        private void BindGrid()
        {
            if (ViewState["sortBy"] == null)
            {
                ViewState["sortBy"] = "";
            }
            String tmpStartDate, tmpEndDate;
            tmpStartDate = txtStartDate.Text + " " + ddStartHour.SelectedValue + ":" + ddStartMinute.SelectedItem;
            tmpEndDate = txtEndDate.Text + " " + ddEndHour.SelectedValue + ":" + ddEndMinute.SelectedItem;

            DateTime startDate = DateTime.Parse(tmpStartDate);
            DateTime endDate = DateTime.Parse(tmpEndDate);

            DataTable dt =
                ReportController.FetchMillionIntegrationReport(
                startDate, endDate,
                ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());

            dt.TableName = "MILLIONINTEGRATION|FROM_" + startDate.ToString("ddMMMyyyy_HHmm") + "|TO_" + endDate.AddSeconds(-1).ToString("ddMMMyyyy_HHmm") + "|PRINTED";
            Session["Report"] = dt;
            DataView dv = new DataView();
            dv.Table = dt;
            if (ViewState["sortBy"] != "")
            {
                dv.Sort = ViewState["sortBy"] + " " + ViewState[SORT_DIRECTION];
            }
            gvReport.DataSource = dv;
            gvReport.DataBind();
        }
        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            //BindGrid();
            if (Session["Report"] != null)
            {
                DataView dv = new DataView();
                dv.Table = ((DataTable)Session["Report"]);
                if (ViewState["sortBy"].ToString() != "")
                {
                    dv.Sort = ViewState["sortBy"] + " " + ViewState[SORT_DIRECTION];
                }
                gvReport.DataSource = dv;
                gvReport.DataBind();

            }
        }
        protected void gvReport_DataBound(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
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
                for (int i = 0; i < gvReport.PageCount; i++)
                {
                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());
                    if (i == gvReport.PageIndex)
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
                DataSet ds = gvReport.DataSource as DataSet;
                if (ds != null)
                {
                    itemCount = ds.Tables[0].Rows.Count;
                }

                string pageCount = "<b>" + gvReport.PageCount.ToString() + "</b>";
                lblPageCount.Text = pageCount;
            }

            Button btnPrev = (Button)gvrPager.Cells[0].FindControl("btnPrev");
            Button btnNext = (Button)gvrPager.Cells[0].FindControl("btnNext");
            Button btnFirst = (Button)gvrPager.Cells[0].FindControl("btnFirst");
            Button btnLast = (Button)gvrPager.Cells[0].FindControl("btnLast");
            //now figure out what page we're on
            if (gvReport.PageIndex == 0)
            {
                btnPrev.Enabled = false;
                btnFirst.Enabled = false;
            }

            else if (gvReport.PageIndex + 1 == gvReport.PageCount)
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
        protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["sortBy"] = e.SortExpression;
            //rebind the grid
            if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
            {
                ViewState[SORT_DIRECTION] = SqlFragment.DESC;
            }

            else
            {
                ViewState[SORT_DIRECTION] = SqlFragment.ASC;
            }

            if (Session["Report"] != null)
            {
                DataView dv = new DataView();
                dv.Table = ((DataTable)Session["Report"]);
                if (ViewState["sortBy"] != "")
                {
                    dv.Sort = ViewState["sortBy"] + " " + ViewState[SORT_DIRECTION];
                }
                gvReport.DataSource = dv;
                gvReport.DataBind();

            }
        }
        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvReport.PageIndex = ddlPages.SelectedIndex;
            // a method to populate your grid
            //BindGrid();
            if (Session["Report"] != null)
            {
                gvReport.DataSource = ((DataTable)Session["Report"]);
                gvReport.DataBind();
            }
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
        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[AMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT].Text));
                e.Row.Cells[WEIGHT].Text = Decimal.Parse(e.Row.Cells[WEIGHT].Text).ToString("N2");
                e.Row.Cells[COGS].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[COGS].Text));
                decimal tmp = Decimal.Parse(e.Row.Cells[PLPercent].Text);
                if (tmp >= -100 && tmp <= 100)
                {
                    e.Row.Cells[PLPercent].Text = tmp.ToString("N2") + "%";
                }
                else
                {
                    e.Row.Cells[PLPercent].Text = "ERR";
                }
                e.Row.Cells[PL].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[PL].Text));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (Session["Report"] != null)
                {
                    DataTable dt = ((DataTable)Session["Report"]);
                    if (dt != null)
                    {
                        decimal Amount, PLAmt, Weight;
                        e.Row.Cells[QTY].Text = dt.Compute("SUM(TOTALQuantity)", "").ToString();
                        Amount = decimal.Parse(dt.Compute("SUM(TOTALAMOUNT)", "").ToString());
                        e.Row.Cells[AMOUNT].Text =  Amount.ToString("N2");
                        Weight = decimal.Parse(dt.Compute("SUM(TOTALWEIGHT)", "").ToString());
                        e.Row.Cells[WEIGHT].Text = Weight.ToString("N2");
                        e.Row.Cells[COGS].Text =  decimal.Parse(dt.Compute("SUM(TotalCOSTOFGOODSSOLD)", "").ToString()).ToString("N2");
                        PLAmt = decimal.Parse(dt.Compute("SUM(PROFITLOSS)", "").ToString());
                        e.Row.Cells[PL].Text =  PLAmt.ToString("N2");
                        e.Row.Cells[PLPercent].Text = (PLAmt / Amount * 100).ToString("N2") + "%";
                        /*
                    decimal PLPercentageTMP;
                    if (decimal.TryParse(dt.Compute("SUM(PROFITLOSSpercentage)",
                        "ProfitLossPercentage <=100 AND ProfitLossPercentage >= -100").ToString(), out PLPercentageTMP)) 
                    {
                        e.Row.Cells[PLPercent].Text = 
                            (PLPercentageTMP / int.Parse(dt.Compute("COUNT(ItemNO)", 
                            "ProfitLossPercentage <=100 AND ProfitLossPercentage > -100").ToString())).ToString("N2") + "%";

                    }
                    }
                }
            }
            */
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["sortBy"] = null;
            if (rdbMonth.Checked)
            {
                int selectedMonth = int.Parse(ddlMonth.SelectedValue);
                DateTime startDate = new DateTime(int.Parse(lblYear.Text), selectedMonth, 1);
                txtStartDate.Text =
                    startDate.ToString("dd MMM yyyy");

                txtEndDate.Text = startDate.AddMonths(1).ToString("dd MMM yyyy");
            }
            /*
            if (rdbMonth.Checked)
            {            
                txtStartDate.Text = (new DateTime(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue), 1)).ToString("dd MMM yyyy");
                txtEndDate.Text = new DateTime(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd MMM yyyy");                
            }
            */
            BindGrid();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {

            ddlOutlet.SelectedIndex = 0;
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            //ddCategory.SelectedItem.Text 
            // = "";
            txtItemName.Text = "";
            //txtPointOfSale.Text = "";
            gvReport.PageIndex = 0;
        }
        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataTable dt = (DataTable)gvReport.DataSource;
            CommonWebUILib.ExportCSV(dt, this.Page.ToString().Trim(' '),"Million Integration Report", gvReport);
        }
        protected void ddDept_Init(object sender, EventArgs e)
        {
            if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
            {
                ddDept.WhereField = "DepartmentID";
                ddDept.WhereValue = Session["DeptID"].ToString();
            }
        }

        protected void ddPOS_Init(object sender, EventArgs e)
        {
            if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0" & ddDept.SelectedValue == "0")
            {
                ddPOS.WhereField = "DepartmentID";
                ddPOS.WhereValue = Session["DeptID"].ToString();
            }
        }
        protected void ddDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddPOS.Items.Clear();
            IDataReader rdr;

            Query qry = new Query("PointOfSale");
            qry.QueryType = QueryType.Select;

            if (ddDept.SelectedValue != "0")
            {
                qry.AddWhere(PointOfSale.Columns.DepartmentID, ddDept.SelectedValue.ToString());
            }
            if (ddlOutlet.SelectedValue != "")
            {
                qry.AddWhere(PointOfSale.Columns.OutletName,
                    Comparison.Like, ddlOutlet.SelectedValue.ToString());
            }
            rdr = qry.ExecuteReader();

            ListItemCollection ls = new ListItemCollection();
            SubSonic.Utilities.Utility.LoadListItems(ls, rdr, "PointOfSaleName", "PointOfSaleID", "", true);
            ls.Insert(0, new ListItem("ALL", ""));
            SubSonic.Utilities.Utility.LoadDropDown(ddPOS, ls, "Text", "Value", "");
        }
    }
}
