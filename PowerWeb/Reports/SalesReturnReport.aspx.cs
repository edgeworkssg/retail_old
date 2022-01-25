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
    public partial class SalesReturnReport : PageBase
    {
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblYear.Text = DateTime.Today.Year.ToString();
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                txtStartDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(0).ToString("dd MMM yyyy");
                ViewState["sortBy"] = "";
                ViewState[SORT_DIRECTION] = "";

                ddlPOS.DataSource = PointOfSaleController.FetchByUserNameForReport(false, true, Session["UserName"] + "", "ALL");
                ddlPOS.DataBind();
                BindGrid();

                
            }
        }


        protected void lnkStatus_Click(object sender, EventArgs e)
        {
            LinkButton btndetails = sender as LinkButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            hdnReceiptNo.Value = gvReport.DataKeys[gvrow.RowIndex].Value.ToString();
            this.ModalPopupExtender1.Show();
        }
        private void BindGrid()
        {
            if (ViewState["sortBy"] == null)
            {
                ViewState["sortBy"] = "";
            }   

            DateTime startDate = DateTime.Parse(txtStartDate.Text);
            DateTime endDate = DateTime.Parse(txtEndDate.Text);

            string SelectedPOS = ddlPOS.SelectedItem.Text;
            if (SelectedPOS == "ALL")
            {
                SelectedPOS = "%";
            }

            DataTable dt = ReportController.FetchSaleReturnReport
                (startDate, endDate.AddSeconds(86399), "%" + txtSearch.Text + "%", SelectedPOS, ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());

            if (dt != null)
            {
                gvReport.DataSource = dt;
                gvReport.DataBind();

                AttributesLabel al = new AttributesLabel(1);
                if (al != null && al.AttributesNo != null && !String.IsNullOrEmpty(al.Label))
                {
                    gvReport.Columns[7].Visible = true;
                    gvReport.Columns[7].HeaderText = al.Label;
                }
                else
                {
                    gvReport.Columns[7].Visible = false;
                }
            }
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvReport_DataBound(Object sender, EventArgs e)
        {

            //pull the datasource
            DataView ds = gvReport.DataSource as DataView;

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
                if (ds != null)
                {
                    itemCount = ds.Table.Rows.Count;
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

        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvReport.PageIndex = ddlPages.SelectedIndex;
            // a method to populate your grid
            BindGrid();
        }


        protected void lnkExport_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
                DataTable dt = (DataTable)gvReport.DataSource;

                //CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '));
                // Export the details of specified columns to Excel
                int[] column;
                column = new int[dt.Columns.Count];
                for (int i = 0; i < column.Length; i++)
                {
                    column[i] = i;
                }
                string[] header;
                header = new string[dt.Columns.Count];
                //Work around for bug in the export to excel library
                for (int i = 0; i < header.Length; i++)
                {
                    header[i] = dt.Columns[i].ColumnName;
                    dt.Columns[i].ColumnName = "col" + i.ToString();
                }

                RKLib.ExportData.Export objExport = new
                    RKLib.ExportData.Export("Web");

                objExport.ExportDetails(dt, column, header,
                     RKLib.ExportData.Export.ExportFormat.CSV,
                     this.Page.Title.Trim(' ') + DateTime.Now.ToString("ddMMMyyyy")
                     + ".CSV");
            }
            catch (Exception ex)
            {
                litMessage.Text = ex.Message;
            }
        }
        protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["Sort"] = e.SortExpression;
            BindGrid();
        }
        protected void ddDept_Init(object sender, EventArgs e)
        {
            if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
            {

            }
        }
        protected void ddDept_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["sortBy"] = null;

            if (rdbMonth.Checked)
            {
                txtStartDate.Text = (new DateTime(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue), 1)).ToString("dd MMM yyyy");
                txtEndDate.Text = new DateTime(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd MMM yyyy");
            }

            BindGrid();
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //amount column format
                if(e.Row.Cells[8].Text != "")
                    e.Row.Cells[8].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[8].Text));
   
                //string lblCheque lsDataKeyValue = GridView1.DataKeys[e.Row.RowIndex].Values[0].ToString();
                LinkButton lnkStatus = (LinkButton)e.Row.FindControl("lnkStatus");
                Label lblCheque = (Label)e.Row.FindControl("lblCheque");
                bool str = Convert.ToBoolean(lnkStatus.Text);
                if (str == true)
                {
                    lnkStatus.Visible = false;
                    lblCheque.Visible = true;
                    lblCheque.Text = "Yes";

                }
                else
                {
                    lnkStatus.Visible = true;
                    lblCheque.Visible = false;
                    lnkStatus.Text = "No";
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable source = (DataTable)gvReport.DataSource;
                for (int i = 4; i < source.Columns.Count; i++)
                {

                    // e.Row.Cells[i].Text = source.Compute("SUM(" + source.Columns[i].ColumnName + ")", "").ToString();
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string sql = "Update OrderHdr Set userflag1=1,userfld4=" + txtChequeNo.Text + " where OrderHdrID='" + hdnReceiptNo.Value + "'";
            QueryCommand Qcmd = new QueryCommand(sql);
            DataService.ExecuteQuery(Qcmd).ToString();
            ViewState["Sort"] = "";
            BindGrid();
        }

        protected void ddPOS_Init(object sender, EventArgs e)
        {
            //if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
            //{
            //    ddlPOS.WhereField = "DepartmentID";
            //    ddlPOS.WhereValue = Session["DeptID"].ToString();
            //}
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");

            txtSearch.Text = "";
            //txtPointOfSale.Text = "";
            gvReport.PageIndex = 0;
        }
    }
}
