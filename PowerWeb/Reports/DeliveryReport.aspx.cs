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
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace PowerWeb.Reports
{
    public partial class DeliveryReport : System.Web.UI.Page
    {
        private const int DiscColumn = 33;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                txtStartDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                BindGrid();
            }
        }

        #region *) Method

        private void BindGrid()
        {
            #region *) Get Start & End Date
            DateTime startDate = DateTime.Parse(txtStartDate.Text);
            DateTime endDate = DateTime.Parse(txtEndDate.Text);
            #endregion

            DataTable dt = ReportController.FetchDeliveryOrderReport(startDate, endDate);

            gvReport.DataSource = dt;
            gvReport.DataBind();
        }

        #endregion


        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvReport.PageIndex = ddlPages.SelectedIndex;
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            gvReport.PageIndex = 0;
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataTable dt = (DataTable)gvReport.DataSource;
            PutDataTableToCsv(gvReport);
        }

        public void PutDataTableToCsv(GridView gv)
        {
            Response.ClearContent();
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.ContentEncoding = System.Text.Encoding.UTF8; 
            Response.AddHeader("Content-Disposition", "attachment; filename=Delivery_Report_"+DateTime.Now.ToString("yyyyMMdd")+".csv;");

            StringBuilder sb = new StringBuilder();
            DataTable dt = (DataTable)gv.DataSource;
            List<string> columnNames = new List<string>();
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                if (gv.Columns[i] is BoundField && gv.Columns[i].Visible)
                {
                    string dataField = ((BoundField)gv.Columns[i]).DataField;
                    if (dataField != "")
                    {
                        columnNames.Add(gv.Columns[i].HeaderText);
                    }
                }
            }

            sb.AppendLine(string.Join(",", columnNames.ToArray()));

            foreach (DataRow row in dt.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                ToArray();
                for (int i = 0; i < fields.Length; i++)
                {
                    fields[i] = fields[i].Replace(Environment.NewLine," ").Replace("\n", " ").Replace("\r", " ").Replace(",", " ");
                }
                sb.AppendLine(string.Join(",", fields));
            }

            Response.Write(sb.ToString());


            Response.Flush();
            Response.End();
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[DiscColumn].Text = String.Format("{0:N0}", e.Row.Cells[DiscColumn].Text.GetDecimalValue());
            }
        }

        protected void gvReport_DataBound(object sender, EventArgs e)
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
    }
}
