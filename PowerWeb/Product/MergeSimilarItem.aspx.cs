using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SubSonic;
using PowerPOS;
using System.Text;

namespace PowerWeb.Product
{
    public partial class MergeSimilarItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
        }


        protected void BtnMerge_Click(object sender, EventArgs e)
        {
            try
            {
                string barcode = txtBarcode.Text;
                string status = "";

                if (string.IsNullOrEmpty(barcode))
                {
                    lblStatus.Text = "Barcode is empty";
                    return;
                }

                Item i = new Item(Item.Columns.Barcode, barcode);

                if (i == null)
                {
                    throw new Exception("Item with selected barcode doesn't exist");
                }

                ItemCollection col = new ItemCollection();
                col.Where(Item.Columns.Deleted, false);
                col.Where(Item.Columns.ItemName, i.ItemName);
                col.Where(Item.Columns.Barcode, Comparison.NotEquals, i.Barcode);
                col.Load();

                if (col.Count == 0)
                {
                    throw new Exception("There is no item that could be merged. Search another barcode.");
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Item ");
    
                    for (int k = 0; k < col.Count; k++)
                    {
                        if(k == col.Count - 1)
                            sb.Append(col[k].ItemNo + " ");
                        else
                            sb.Append(col[k].ItemNo + " ,");
                    }

                    sb.Append(" will be deleted and merge into Item " + i.ItemNo + " <br /> <br />");
                    sb.Append("Are you sure to merge the item ?");

                    labelWarning.Text = sb.ToString();

                    ClientScriptManager cs = Page.ClientScript;

                    String csname1 = "PopupScript";
                    String csname2 = "ButtonClickScript";
                    Type cstype = this.GetType();

                    // Check to see if the startup script is already registered.
                    if (!cs.IsStartupScriptRegistered(cstype, csname1))
                    {
                        String cstext1 = "$('#dialog-warning').dialog('open');";
                        cs.RegisterStartupScript(cstype, csname1, cstext1, true);
                    }
                }



            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Merge Similar Item : " + ex.Message);
                lblStatus.Text = ex.Message;
            }
        }

        protected void BtnMergeReal_Click(object sender, EventArgs e)
        {
            try
            {
                string barcode = txtBarcode.Text;
                string status = "";

                Item i = new Item(Item.Columns.Barcode, barcode);

                if (!ItemController.MergeSimilarItem(barcode, "Merged By " + Session["username"] + " ", out status))
                {
                    throw new Exception(status);
                }

                BindGrid();
                lblStatus.Text = "Item " + i.ItemName + " with barcode " + barcode + " merged succesfully.";
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Merge Similar Item : " + ex.Message);
                lblStatus.Text = ex.Message;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtBarcode.Text = "";
            BindGrid();
        }

        protected void BtnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewProductMaster.aspx");
        }


        private void BindGrid()
        {
            lblStatus.Text = "";
            DataTable dt = ItemController.GetSimilarItem(txtBarcode.Text);

            gvReport.DataSource = dt;
            gvReport.EmptyDataText = LanguageManager.GetTranslation("No Record");
            gvReport.DataBind();
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
                DataTable ds = (DataTable)gvReport.DataSource;
                if (ds != null)
                {
                    itemCount = ds.Rows.Count;
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
            ViewState["Sort"] = e.SortExpression;
            //if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
            //{
            //    ViewState[SORT_DIRECTION] = SqlFragment.DESC;
            //}

            //else
            //{
            //    ViewState[SORT_DIRECTION] = SqlFragment.ASC;
            //}

            //rebind the grid
            BindGrid();
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {


        }

        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvReport.PageIndex = ddlPages.SelectedIndex;
            BindGrid();
        }



    }
}
