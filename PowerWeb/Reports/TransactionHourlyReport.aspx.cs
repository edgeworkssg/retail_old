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
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;


namespace PowerWeb.Reports
{
    public partial class TransactionHourlyReport : PageBase
    {
        private const int AMOUNT = 2;
        private const int IsVoided = 10;
        private const int TABLENO = 4;
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";
        protected void Page_Load(object sender, EventArgs e)
        {
            SetFormSetting();
            if (!Page.IsPostBack)
            {
                txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(1).ToString("dd MMM yyyy");
                ViewState["sortBy"] = "";
                ViewState[SORT_DIRECTION] = "";

                //bool showTableNo = (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ShowTableNoInReport"])
                //                    && ConfigurationManager.AppSettings["ShowTableNoInReport"].ToLower() == "yes");
                //litTableNo.Visible = showTableNo;
                //txtTableNo.Visible = showTableNo;
                //gvReport.Columns[TABLENO].Visible = showTableNo;

                BindGrid();
            }
        }

        private void SetFormSetting()
        {
            try
            {
                string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                gvReport.Columns[4].HeaderText = outletText;
                gvReport.Columns[5].HeaderText = posText;
                OutletDropdownList.SetLabelPOS(posText);
                OutletDropdownList.SetLabelOutlet(outletText);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void BindGrid()
        {
            try
            {
                if (ViewState["sortBy"] == null)
                {
                    ViewState["sortBy"] = "";
                }
                string selectedOutlet = OutletDropdownList.GetDdlOutletSelectedItemText;
                string SelectedPOS = OutletDropdownList.GetDdlPOSSelectedItemText;
                String tmpStartDate, tmpEndDate;
                tmpStartDate = txtStartDate.Text + " " + ddStartHour.SelectedValue + ":" + ddStartMinute.SelectedItem;
                tmpEndDate = txtEndDate.Text + " " + ddEndHour.SelectedValue + ":" + ddEndMinute.SelectedItem;

                DateTime startDate = DateTime.Parse(tmpStartDate);
                DateTime endDate = DateTime.Parse(tmpEndDate);


                DataTable dt = ReportController.FetchTransactionHourlyReport(
                    cbUseStartDate.Checked, cbUseStartDate.Checked,
                    startDate, endDate,
                    (OutletDropdownList.GetDdlPOSSelectedValue + "").GetIntValue(), OutletDropdownList.GetDdlOutletSelectedItemText,
                    chkShowBeforeGST.Checked,
                    ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
                gvReport.DataSource = dt;
                gvReport.DataBind();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        decimal TotalAmount = 0;
        decimal TotalQty = 0;
        public void CalculateSum()
        {
            TotalAmount = 0;
            TotalQty = 0;

            DataTable dt = (DataTable)gvReport.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TotalAmount += decimal.Parse(dt.Rows[i]["TotalAmount"].ToString());
                TotalQty += int.Parse(dt.Rows[i]["TotalTrans"].ToString());
            }
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
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

            BindGrid();
        }
        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvReport.PageIndex = ddlPages.SelectedIndex;
            // a method to populate your grid
            BindGrid();
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[AMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT].Text));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                CalculateSum();
                e.Row.Cells[2].Text =  TotalAmount.ToString("N2");
                e.Row.Cells[3].Text = TotalQty.ToString();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["sortBy"] = null;
            BindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            cbUseStartDate.Checked = false;
            cbUseEndDate.Checked = false;
            OutletDropdownList.ResetDdlOutlet();
            OutletDropdownList.ResetDdlPOS();
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            //txtPointOfSale.Text = "";
            gvReport.PageIndex = 0;
        }
        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataTable dt = (DataTable)gvReport.DataSource;
            CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '),this.Page.Title.Trim(' '), gvReport);
        }

    }
}
