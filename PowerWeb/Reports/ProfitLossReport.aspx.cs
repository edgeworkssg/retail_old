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
using System.Collections.Generic;

using PowerPOS;
using SubSonic;
using SubSonic.Utilities;

public partial class ProfitLossReport : PageBase
{
    private const int NO_OF_TRANSACTION = 2;
    private const int GROSS_SALES = 3;
    private const int DISCOUNT = 4;
    private const int DISCOUNT_PERCENTAGE = 5;
    private const int NETT_SALES_B4_GST = 6;    
    private const int GST_AMT = 7;
    private const int NETT_SALES_AFTER_GST = 8;
    private const int COST_OF_GOODS = 9;
    private const int PROFIT_LOSS = 10;
    private const int PROFIT_LOSS_PERCENTAGE = 11;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    protected void Page_Load(object sender, EventArgs e)
    {
        lblIncomplete.Visible = false;
        if (!Page.IsPostBack)
        {
            ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(true, Session["UserName"]+""));
            ddlOutlet.DataBind();
            List<ListItem> listYear = new List<ListItem>();
            int minyear = UtilityController.GetMinOrderDateYear();
            for (int i = DateTime.Today.Year; i >= minyear; i--)
            {
                listYear.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddlYear.Items.AddRange(listYear.ToArray());
            ddlYear.SelectedIndex = 0;
            
            //lblYear.Text = DateTime.Today.Year.ToString();
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
            txtStartDate.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";
            
            BindGrid();
        }
    }

    private void BindGrid()
    {
        //if (GridModes)
        //{
            bool includePreOrder = cbIncludePreOrder.Checked;

            if (ViewState["sortBy"].ToString() == null | ViewState["sortBy"].ToString() == "")
            {
                ViewState["sortBy"] = "PLDate";
            }
            DateTime startDate = DateTime.Parse(txtStartDate.Text);
            DateTime endDate = DateTime.Parse(txtEndDate.Text);
            DataTable dt =
                ReportController.FetchProfitLossReportGroupByDay_NEW(
                startDate, endDate.AddSeconds(86399),
                ddlOutlet.SelectedItem.Text, "", includePreOrder, 
                ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
            DataView dv = new DataView();
            dt.TableName = "Report";
            dv.Table = dt;
            dv.Sort = ViewState["sortBy"].ToString() + " " + ViewState[SORT_DIRECTION].ToString();
            gvReport.DataSource = dv;
            gvReport.DataBind();
        //}
        //else
        //{
        //    BindChart();
        //}
    }

    private void BindChart()
    {
        if (ViewState["sortBy"].ToString() == null | ViewState["sortBy"].ToString() == "")
        {
            ViewState["sortBy"] = "PLDate";
        }
        bool includePreOrder = cbIncludePreOrder.Checked;

        DateTime startDate = DateTime.Parse(txtStartDate.Text);
        DateTime endDate = DateTime.Parse(txtEndDate.Text);
        DataTable dt =
            ReportController.FetchProfitLossReportGroupByDay_NEW(
            startDate, endDate.AddSeconds(86399),
            ddlOutlet.SelectedItem.Text, "",includePreOrder,
            ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
        DataView dv = new DataView();
        dt.TableName = "Report";
        dv.Table = dt;
        dv.Sort = ViewState["sortBy"].ToString() + " " + ViewState[SORT_DIRECTION].ToString();
        dv.Sort = ViewState["sortBy"].ToString() + " " + ViewState[SORT_DIRECTION].ToString();

        Random random = new Random();
        List<string> Data = new List<string>(), 
            Names = new List<string>(), 
            Colours = new List<string>();

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Data.Add(decimal.Parse(dt.Rows[i]["NettSalesBeforeGST"].ToString()).ToString("N2").Replace(",", ""));
                DateTime TempDate = DateTime.Parse(dt.Rows[i]["PLDate"].ToString());
                if (i == 0 || i == dt.Rows.Count - 1 || i == Math.Ceiling(((decimal)(dt.Rows.Count - 1) / 2))
                    || TempDate.Day == 1)
                    Names.Add(TempDate.ToString("MM/dd"));
                else
                    Names.Add("");
                Colours.Add(String.Format("{0:X6}", random.Next(0x1000000)));
            }
            chartFrame1.Attributes["src"] = ChartController.CreateLineChart(ddlOutlet.SelectedItem.Text, Data.ToArray(), Names.ToArray(), Colours.ToArray());
        }
        else
        {
            chartFrame1.Attributes["src"] = "";
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
        ViewState["sortBy"]  = e.SortExpression;
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

    /*
    private decimal totalGrossSales, totalDiscount, totalDiscountPercentage,
        totalNettSalesB4GST, totalGSTAmt, totalNettSalesAfterGST,
        totalCostOfGoods, totalProfitAndLoss, totalProfitAndLossPercentage;
    */

    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
            decimal GrossSales, Discount, DiscountPercentage,
                NettSalesB4GST, GSTAmt, NettSalesAfterGST,
                CostOfGoods, ProfitAndLoss, ProfitAndLossPercentage;

            GrossSales = Decimal.Parse(e.Row.Cells[GROSS_SALES].Text);
            e.Row.Cells[GROSS_SALES].Text = String.Format("{0:N2}", GrossSales);

            Discount = Decimal.Parse(e.Row.Cells[DISCOUNT].Text);        
            e.Row.Cells[DISCOUNT].Text = String.Format("{0:N2}", Discount);

            DiscountPercentage = Decimal.Parse(e.Row.Cells[DISCOUNT_PERCENTAGE].Text);            
            e.Row.Cells[DISCOUNT_PERCENTAGE].Text = String.Format("{0:0%}", DiscountPercentage);

            NettSalesB4GST = Decimal.Parse(e.Row.Cells[NETT_SALES_B4_GST].Text);            
            e.Row.Cells[NETT_SALES_B4_GST].Text = String.Format("{0:N2}", NettSalesB4GST);

            GSTAmt = Decimal.Parse(e.Row.Cells[GST_AMT].Text);            
            e.Row.Cells[GST_AMT].Text = String.Format("{0:N2}", GSTAmt);

            /* - DISABLE FOR BLIFE*/
            NettSalesAfterGST = Decimal.Parse(e.Row.Cells[NETT_SALES_AFTER_GST].Text);            
            e.Row.Cells[NETT_SALES_AFTER_GST].Text = String.Format("{0:N2}", NettSalesAfterGST);

            DateTime myDate = DateTime.Parse(gvReport.DataKeys[e.Row.RowIndex].Value.ToString());
            if (!ReportController.IsProfitAndLossShowIncomplete()|| OrderHdrController.IsOrderInventoryHasBeenDeductedCompletely(myDate, myDate.AddHours(24)))
            {
                //e.Row.BackColor = System.Drawing.Color.Salmon;

                CostOfGoods = Decimal.Parse(e.Row.Cells[COST_OF_GOODS].Text);
                e.Row.Cells[COST_OF_GOODS].Text = String.Format("{0:N2}", CostOfGoods);

                /* - DISABLE FOR BLIFE*/
                ProfitAndLoss = Decimal.Parse(e.Row.Cells[PROFIT_LOSS].Text);
                e.Row.Cells[PROFIT_LOSS].Text = String.Format("{0:N2}", ProfitAndLoss);

                /* - DISABLE FOR BLIFE*/
                ProfitAndLossPercentage = Decimal.Parse(e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text);
                if (ProfitAndLossPercentage >= -100 && ProfitAndLossPercentage <= 100)
                {
                    e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text = String.Format("{0:0%}", ProfitAndLossPercentage);
                }
                else
                {
                    e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text = "ERR";
                }                                    
            }
            else
            {
                e.Row.Cells[COST_OF_GOODS].Text = "INCOMPLETE";
                e.Row.Cells[PROFIT_LOSS].Text = "INCOMPLETE";
                e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text = "INCOMPLETE";
                lblIncomplete.Visible = true;
            }

            
            
            
            
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {            
            DataTable dt = ((DataView)gvReport.DataSource).Table;
            if (dt != null && dt.Rows.Count > 0)
            {
                decimal TotalNettSales, PLAmt;

                e.Row.Cells[NO_OF_TRANSACTION].Text = decimal.Parse(dt.Compute("SUM(NoOfTransaction)", "").ToString()).ToString("N0"); // totalGrossSales.ToString("N2");
                e.Row.Cells[GROSS_SALES].Text = decimal.Parse(dt.Compute("SUM(GrossSales)", "").ToString()).ToString("N2"); // totalGrossSales.ToString("N2");
                e.Row.Cells[DISCOUNT].Text = decimal.Parse(dt.Compute("SUM(DiscountSales)", "").ToString()).ToString("N2");
                e.Row.Cells[DISCOUNT_PERCENTAGE].Text = ((decimal)dt.Compute("SUM(DiscountPercentage)", "") * 100 / dt.Rows.Count).ToString("N2") + "%";

                e.Row.Cells[NETT_SALES_B4_GST].Text = decimal.Parse(dt.Compute("SUM(NettSalesBeforeGST)", "").ToString()).ToString("N2");
                e.Row.Cells[GST_AMT].Text =  decimal.Parse(dt.Compute("SUM(GstAmount)", "").ToString()).ToString("N2");

                /* - DISABLE FOR BLIFE*/
                TotalNettSales = decimal.Parse(dt.Compute("SUM(NettSalesAfterGST)", "").ToString());
                e.Row.Cells[NETT_SALES_AFTER_GST].Text =  TotalNettSales.ToString("N2");
                DateTime startDate, endDate;

                if (!ReportController.IsProfitAndLossShowIncomplete() || (DateTime.TryParse(txtStartDate.Text, out startDate) &&
                    DateTime.TryParse(txtEndDate.Text, out endDate) &&
                    OrderHdrController.IsOrderInventoryHasBeenDeductedCompletely(startDate, endDate)))
                {
                    e.Row.Cells[COST_OF_GOODS].Text =  decimal.Parse(dt.Compute("SUM(CostOfGoodsSold)", "").ToString()).ToString("N2");
                    PLAmt = decimal.Parse(dt.Compute("SUM(ProfitLoss)", "").ToString());
                    e.Row.Cells[PROFIT_LOSS].Text =  PLAmt.ToString("N2");
                    //e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text = (PLAmt * 100 / TotalNettSales).ToString("N2") + "%";
                }
                else
                {
                    e.Row.Cells[COST_OF_GOODS].Text = "INCOMPLETE";
                    e.Row.Cells[PROFIT_LOSS].Text = "INCOMPLETE";
                    e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text = "INCOMPLETE";
                    lblIncomplete.Visible = true;
                }
            }
        }
    }
    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (rdbMonth.Checked)
        {
            txtStartDate.Text = (new DateTime(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue), 1)).ToString("dd MMM yyyy");
            txtEndDate.Text = new DateTime(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd MMM yyyy");
        }
        BindGrid();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
  
        ddlOutlet.SelectedIndex = 0;
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            cbIncludePreOrder.Checked = false;      
        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        gvReport.Columns[1].Visible = true;
        DataTable dt = ((DataView)gvReport.DataSource).Table;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' ').Replace('/', ' '), "Profit & Loss Report", gvReport);
        gvReport.Columns[1].Visible = false;
    }
}
