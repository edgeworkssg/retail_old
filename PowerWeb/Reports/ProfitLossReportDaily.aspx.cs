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

public partial class DailyProfitLossReport : PageBase
{
    private const int GROSS_SALES = 1;
    private const int DISCOUNT = 2;
    private const int DISCOUNT_PERCENTAGE = 3;
    private const int NETT_SALES_B4_GST = 4;
    
    private const int GST_AMT = 5;
    private const int NETT_SALES_AFTER_GST = 6;
    private const int COST_OF_GOODS = 7;
    private const int PROFIT_LOSS = 8;
    private const int PROFIT_LOSS_PERCENTAGE = 9;
    private const int ORDERHDRID = 11;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    protected void Page_Load(object sender, EventArgs e)
    {
        lblIncomplete.Visible = false;

        if (!Page.IsPostBack)
        {
            ddlOutlet.DataSource = PointOfSaleController.FetchOutletNames(true, Session["UserName"] + "");
            ddlOutlet.DataBind();
            
            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";
            if (Request.QueryString["id"] != null)
            {
                string id = Utility.GetParameter("id");
                string Outlet = Utility.GetParameter("Outlet");
                if (!String.IsNullOrEmpty(id))
                {
                    DateTime tempDate;
                    if (DateTime.TryParse(id, out tempDate))
                    {
                        txtDate.Text = tempDate.ToString("dd MMM yyyy");
                    }
                }
                else
                {
                    txtDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                }

                if (!String.IsNullOrEmpty(Outlet))
                { 
                    ddlOutlet.SelectedValue = Outlet;
                }

                string IncludePreOrder = Utility.GetParameter("IncludePreOrder");
                if (!string.IsNullOrEmpty(IncludePreOrder) && IncludePreOrder.Equals("1"))
                    cbIncludePreOrder.Checked = true;
                else
                    cbIncludePreOrder.Checked = false;
            }
            else
            {
                txtDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            }
            BindGrid();

        }
    }

    private void BindGrid()
    {
        if (ViewState["sortBy"] == null || ViewState["sortBy"] == "")
        {
            ViewState["sortBy"] = "OrderDate";
            ViewState[SORT_DIRECTION] = "DESC";
        }
        bool includePreOrder = cbIncludePreOrder.Checked;
        DateTime selectedDate;  
        if (DateTime.TryParse(txtDate.Text, out selectedDate))
        {
            DataTable dt =
                ReportController.FetchProfitLossReportOnTransactionLevel_NEW(
                selectedDate, selectedDate.AddSeconds(86399),
                ddlOutlet.SelectedItem.ToString(), "0", includePreOrder,
                ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
            DataView dv = new DataView();
            dt.TableName = "Table";
            dv.Table = dt;
            dv.Sort = ViewState["sortBy"].ToString() + " " + ViewState[SORT_DIRECTION].ToString();
            gvReport.DataSource = dv;
            gvReport.DataBind();
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

    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                bool isNegative = false;
                if (Decimal.Parse(e.Row.Cells[GROSS_SALES].Text) < 0)
                    isNegative = true;

                decimal netAftergst = 0;

                if (isNegative)
                {
                    e.Row.Cells[GROSS_SALES].Text = String.Format("{0:N2}", Decimal.Negate(Decimal.Parse(e.Row.Cells[GROSS_SALES].Text)) < 0 ? Decimal.Negate(Decimal.Parse(e.Row.Cells[GROSS_SALES].Text)) : Decimal.Parse(e.Row.Cells[GROSS_SALES].Text));
                    e.Row.Cells[DISCOUNT].Text = String.Format("{0:N2}", Decimal.Negate(Decimal.Parse(e.Row.Cells[DISCOUNT].Text)) < 0 ? Decimal.Negate(Decimal.Parse(e.Row.Cells[DISCOUNT].Text)) : Decimal.Parse(e.Row.Cells[DISCOUNT].Text));
                    e.Row.Cells[DISCOUNT_PERCENTAGE].Text = String.Format("{0:0%}", Decimal.Negate(Decimal.Parse(e.Row.Cells[DISCOUNT_PERCENTAGE].Text)) < 0 ? Decimal.Negate(Decimal.Parse(e.Row.Cells[DISCOUNT_PERCENTAGE].Text)) : Decimal.Parse(e.Row.Cells[DISCOUNT_PERCENTAGE].Text));
                    e.Row.Cells[GST_AMT].Text = String.Format("{0:N2}", Decimal.Negate(Decimal.Parse(e.Row.Cells[GST_AMT].Text)) < 0 ? Decimal.Negate(Decimal.Parse(e.Row.Cells[GST_AMT].Text)) : Decimal.Parse(e.Row.Cells[GST_AMT].Text));

                    //e.Row.Cells[NETT_SALES_B4_GST].Text = String.Format("{0:N2}", Decimal.Negate(Decimal.Parse(e.Row.Cells[GROSS_SALES].Text.Replace("$", "").Replace("(", "").Replace(")", ""))) < 0 ? Decimal.Negate(Decimal.Parse(e.Row.Cells[GROSS_SALES].Text.Replace("$", "").Replace("(", "").Replace(")", ""))) : Decimal.Parse(e.Row.Cells[GROSS_SALES].Text.Replace("$", "").Replace("(", "").Replace(")", "")));
                    e.Row.Cells[NETT_SALES_B4_GST].Text = String.Format("{0:N2}", Decimal.Negate(Decimal.Parse(e.Row.Cells[NETT_SALES_B4_GST].Text.Replace("$", "").Replace("(", "").Replace(")", ""))) < 0 ? Decimal.Negate(Decimal.Parse(e.Row.Cells[NETT_SALES_B4_GST].Text.Replace("$", "").Replace("(", "").Replace(")", ""))) : Decimal.Parse(e.Row.Cells[NETT_SALES_B4_GST].Text.Replace("$", "").Replace("(", "").Replace(")", "")));
                    //netAftergst = Decimal.Parse(e.Row.Cells[GROSS_SALES].Text.Replace("$", "").Replace("(", "").Replace(")", "")) - Decimal.Parse(e.Row.Cells[GST_AMT].Text.Replace("$", "").Replace("(", "").Replace(")", ""));
                    netAftergst = Decimal.Parse(e.Row.Cells[NETT_SALES_AFTER_GST].Text.Replace("$", "").Replace("(", "").Replace(")", ""));
                    e.Row.Cells[NETT_SALES_AFTER_GST].Text = String.Format("{0:N2}", Decimal.Negate(netAftergst) < 0 ? Decimal.Negate(netAftergst) : netAftergst);
                }
                else
                {
                    e.Row.Cells[GROSS_SALES].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[GROSS_SALES].Text));
                    e.Row.Cells[DISCOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[DISCOUNT].Text));
                    e.Row.Cells[DISCOUNT_PERCENTAGE].Text = String.Format("{0:0%}", Decimal.Parse(e.Row.Cells[DISCOUNT_PERCENTAGE].Text));
                    e.Row.Cells[NETT_SALES_B4_GST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[NETT_SALES_B4_GST].Text));
                    e.Row.Cells[GST_AMT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[GST_AMT].Text));
                    e.Row.Cells[NETT_SALES_AFTER_GST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[NETT_SALES_AFTER_GST].Text));
                }

                decimal loss = 0;
                
                if (!ReportController.IsProfitAndLossShowIncomplete() || OrderHdrController.IsOrderInventoryHasBeenDeductedCompletely(e.Row.Cells[ORDERHDRID].Text))
                {
                    if (isNegative)
                    {
                        e.Row.Cells[COST_OF_GOODS].Text = String.Format("{0:N2}", Decimal.Negate(Decimal.Parse(e.Row.Cells[COST_OF_GOODS].Text)) < 0 ? Decimal.Negate(Decimal.Parse(e.Row.Cells[COST_OF_GOODS].Text)) : Decimal.Parse(e.Row.Cells[COST_OF_GOODS].Text));
                        loss = Decimal.Parse(e.Row.Cells[GROSS_SALES].Text.Replace("$", "").Replace("(", "").Replace(")", "")) - Decimal.Parse(e.Row.Cells[GST_AMT].Text.Replace("$", "").Replace("(", "").Replace(")", "")) - Decimal.Parse(e.Row.Cells[COST_OF_GOODS].Text.Replace("$", "").Replace("(", "").Replace(")", ""));
                        e.Row.Cells[PROFIT_LOSS].Text = String.Format("{0:N2}", Decimal.Negate(loss));
                    }
                    else
                    {
                        e.Row.Cells[COST_OF_GOODS].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[COST_OF_GOODS].Text));
                        e.Row.Cells[PROFIT_LOSS].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[PROFIT_LOSS].Text));
                    }

                    decimal plPercentage = 0;
                    if (isNegative)
                    {
                        decimal percentage = ((loss / netAftergst) * 100);
                        plPercentage = Decimal.Negate(percentage);
                    }
                    else
                    {
                        plPercentage = Decimal.Parse(e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text);
                    }
                    
                    if (plPercentage >= -100 && plPercentage <= 100)
                    {
                        if (isNegative)
                        {
                            e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text = String.Format("({0:0}%)", plPercentage.ToString("N2").Replace("-", ""));
                        }
                        else
                        {
                            e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text = String.Format("{0:0%}", plPercentage);
                        }
                    }
                    else
                    {
                        e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text = "ERR";
                    }
                }
                else
                {
                    e.Row.Cells[COST_OF_GOODS].Text = "INCOMPLETE";
                    e.Row.Cells[PROFIT_LOSS].Text =  "INCOMPLETE";
                    e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text = "INCOMPLETE";
                    lblIncomplete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //Unable to convert
                Logger.writeLog(ex);
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DataTable dt = ((DataView)gvReport.DataSource).Table;
            if (dt != null && dt.Rows.Count > 0) 
            {
                decimal PLAmt, NettSalesAfterGST;

                e.Row.Cells[GROSS_SALES].Text =  (decimal.Parse(dt.Compute("SUM(GrossSales)", "").ToString())).ToString("N2"); //this.GetColumnTotal(GROSS_SALES); 
                e.Row.Cells[DISCOUNT].Text =  decimal.Parse(dt.Compute("SUM(DiscountSales)", "").ToString()).ToString("N2"); //this.GetColumnTotal(DISCOUNT); 
                e.Row.Cells[DISCOUNT_PERCENTAGE].Text = (decimal.Parse(dt.Compute("SUM(DiscountPercentage)", "").ToString()) * 100 / dt.Rows.Count).ToString("N2") + "%";
                e.Row.Cells[NETT_SALES_B4_GST].Text =  (decimal.Parse(dt.Compute("SUM(NettSalesBeforeGST)", "").ToString())).ToString("N2"); //this.GetColumnTotal(NETT_SALES_B4_GST);
                e.Row.Cells[GST_AMT].Text =  (decimal.Parse(dt.Compute("SUM(GSTAmount)", "").ToString())).ToString("N2"); //this.GetColumnTotal(GST_AMT); 
                NettSalesAfterGST = decimal.Parse(dt.Compute("SUM(NettSalesAfterGST)", "").ToString()); //Decimal.Parse(this.GetColumnTotal(NETT_SALES_AFTER_GST).Replace("$",""));
                e.Row.Cells[NETT_SALES_AFTER_GST].Text =  (NettSalesAfterGST).ToString("N2"); //this.GetColumnTotal(NETT_SALES_AFTER_GST); 
                DateTime selectedDate;
                if (!ReportController.IsProfitAndLossShowIncomplete() || (DateTime.TryParse(txtDate.Text, out selectedDate) &&
                    OrderHdrController.IsOrderInventoryHasBeenDeductedCompletely(selectedDate, selectedDate.AddHours(24))))
                {
                    e.Row.Cells[COST_OF_GOODS].Text =  decimal.Parse(dt.Compute("SUM(CostOfGoodsSold)", "").ToString()).ToString("N2");
                    PLAmt = decimal.Parse(dt.Compute("SUM(ProfitLoss)", "").ToString()); //Decimal.Parse(this.GetColumnTotal(PROFIT_LOSS).Replace("$","")); 
                    e.Row.Cells[PROFIT_LOSS].Text =  (PLAmt).ToString("N2"); //this.GetColumnTotal(PROFIT_LOSS); 
                    e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text = (PLAmt * 100 / (NettSalesAfterGST + (decimal)0.0001)).ToString("N2") + "%";
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

    private String GetColumnTotal(Int32 colIndex)
    {
        decimal deduc = 0;
        decimal runningTotal = 0;
        foreach (GridViewRow row in gvReport.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if (row.Cells[colIndex].Text.Contains("("))
                {
                    Decimal amt = Decimal.Parse(row.Cells[colIndex].Text.Replace("$", "").Replace("(", "").Replace(")", "-"));
                    deduc += amt;
                }
                else
                {
                    Decimal amt = Decimal.Parse(row.Cells[colIndex].Text.Replace("$", ""));
                    runningTotal += amt;
                }

            }
        }
        String ret =  (runningTotal + deduc).ToString("N2");
        return ret;
    }

    private String GetTotalPercentage(Int32 colIndex)
    {
        Decimal total = 0;
        Int32 rowCount = 0;

        foreach (GridViewRow row in gvReport.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
               Decimal val = 0;

                if (row.Cells[colIndex].Text.Contains("("))
                {
                    val = Decimal.Parse(row.Cells[colIndex].Text.Replace("%", "").Replace("(", "").Replace(")", "-"));
                }
                else
                {
                     val = Decimal.Parse(row.Cells[colIndex].Text.Replace("%", ""));
                }
                total += val;
            }

            rowCount++;
        }

        return total.ToString("N2") + "%";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        BindGrid();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
  
        ddlOutlet.SelectedIndex = 0;                    
        gvReport.PageIndex = 0;
        cbIncludePreOrder.Checked = false;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = ((DataView)gvReport.DataSource).Table;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
    }
}
