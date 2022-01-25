using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace PowerWeb.CRReport
{
    public partial class ProductSalesReportCR : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblYear.Text = DateTime.Today.Year.ToString();
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                //ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(true));
                ddlOutlet.DataBind();
                //ddCategory.Items.AddRange(ItemController.FetchCategoryNamesInListItem());
                ddCategory.DataBind();
                txtStartDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                ViewState["sortBy"] = "";
                //ViewState[SORT_DIRECTION] = "";

                //#region *) Display: Set Attributes Columns
                //Dictionary<int, string> Attributes = AttributesLabelController.GetAttributesLabel();
                //int StartOfAttributesColumn = 10;
                //for (int Counter = 1; Counter <= AttributesLabelController.MaxAttributes; Counter++)
                //{
                //    gvReport.Columns[StartOfAttributesColumn + Counter - 1].Visible = false;
                //    if (Attributes.ContainsKey(Counter))
                //    {
                //        gvReport.Columns[StartOfAttributesColumn + Counter - 1].Visible = true;
                //        gvReport.Columns[StartOfAttributesColumn + Counter - 1].HeaderText = Attributes[Counter];

                //    }
                //}
                //#endregion

                BindGrid();
            }
        }
        private void BindGrid()
        {
            //if (ViewState["sortBy"] == null)
            //{
            //    ViewState["sortBy"] = "";
            //}
            //DateTime startDate = DateTime.Parse(txtStartDate.Text);
            //DateTime endDate = DateTime.Parse(txtEndDate.Text);

            //DataTable dt =
            //    ReportController.FetchProductSalesReport(
            //    startDate, endDate.AddSeconds(86399),
            //    txtItemName.Text, ddPOS.SelectedItem.Text, ddlOutlet.SelectedValue.ToString(),
            //     ddCategory.SelectedValue.ToString(), ddDept.SelectedValue.ToString(), false,
            //    ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
            //gvReport.DataSource = dt;
            //gvReport.DataBind();
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
        protected void btnClear_Click(object sender, EventArgs e)
        {

            ddlOutlet.SelectedIndex = 0;
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtItemName.Text = "";

            //gvReport.PageIndex = 0;
        }
        protected void lnkExport_Click(object sender, EventArgs e)
        {
            //BindGrid();
            //DataTable dt = (DataTable)gvReport.DataSource;
            //CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
        }
        protected void ddDept_Init(object sender, EventArgs e)
        {

        }
        protected void ddPOS_Init(object sender, EventArgs e)
        {

        }
    }
}
