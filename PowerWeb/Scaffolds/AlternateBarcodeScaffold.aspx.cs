using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using System.Data;
using SubSonic;
using SubSonic.Utilities;

namespace PowerWeb.Scaffolds
{
    public partial class AlternateBarcodeScaffold : PageBase
    {
        private bool isAdd = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                string id = Utility.GetParameter("id");

                if (Request.QueryString["msg"] != null)
                {
                    string msg = Utility.GetParameter("msg"); ;
                    lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + msg + "</span>";
                }
                
                if (!String.IsNullOrEmpty(id) && id != "0")
                {
                    if (!Page.IsPostBack)
                    {
                        ToggleEditor(true);
                        LoadEditor(id);
                    }
                }
                else
                {
                    //it's an add, show the editor
                    isAdd = true;
                    ToggleEditor(true);
                    btnDelete.Visible = false;
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    ToggleEditor(false);
                    BindGrid();
                }
            }

        }

        private void LoadDropdown(string search)
        {
            if (search == string.Empty)
                search = "%";

            string sql = "select * from Item where (ISNULL(deleted,0) = 0 OR EXISTS(SELECT * FROM OutletGroupItemMap WHERE ItemNo = Item.ItemNo AND ISNULL(Deleted, 0) = 0) ) and ISNULL(ItemNo,'') + ' ' + ISNULL(ItemName,'') + ' ' + ISNULL(Barcode,'') LIKE '%" + search.Replace("'", "''") + "%' order by ItemName";
            DataSet ds = DataService.GetDataSet(new QueryCommand(sql));
            ItemCollection col = new ItemCollection();
            col.Load(ds.Tables[0]);

            ddsItem.DataSource = col;
            ddsItem.DataBind();
        }


        private void LoadEditor(string id)
        {

            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                lblBarcodeID.Text = id.ToString();
                
                //pull the record
                AlternateBarcode item = new AlternateBarcode(id);
                //bind the page 

                txtBarcode.Text = item.Barcode;

                LoadDropdown(item.ItemNo);
                ddsItem.SelectedValue = item.ItemNo;

                if (item.CreatedOn.HasValue)
                {
                    lblCreatedOn.Text = item.CreatedOn.Value.ToString();
                }

                if (item.ModifiedOn.HasValue)
                {
                    lblModifiedOn.Text = item.ModifiedOn.Value.ToString();
                }

                lblCreatedBy.Text = item.CreatedBy;

                lblModifiedBy.Text = item.ModifiedBy;

                if (item.Deleted.HasValue)
                {
                    cbDeleted.Checked = item.Deleted.Value;
                }

                //set the delete confirmation
                btnDelete.Attributes.Add("onclick", "return CheckDelete();");
            }
        }

        void BindAndSave(string id)
        {
            AlternateBarcode item;
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                if(CheckDuplicateAlternateBarcode(txtBarcode.Text,Int32.Parse(id)))
                {
                     throw new Exception("Barcode is duplicated");
                }
                //it's an edit
                item = new AlternateBarcode(id);
                item.IsNew = false;
            }
            else
            {
                //add
                if(CheckDuplicateAlternateBarcode(txtBarcode.Text,0))
                {
                     throw new Exception("Barcode is duplicated");
                }

                item = new AlternateBarcode();
                item.IsNew = true;
            }

            item.Barcode = txtBarcode.Text;
            item.ItemNo = ddsItem.SelectedValue;

            object valctrlDeleted = Utility.GetDefaultControlValue(ItemDepartment.Schema.GetColumn("Deleted"), cbDeleted, isAdd, false);

            if (valctrlDeleted == null)
            {
                item.Deleted = null;
            }
            else
            {
                item.Deleted = Convert.ToBoolean(valctrlDeleted);
            }

            //bind it
            item.Save(Session["username"]+"");

            lblBarcodeID.Text = item.BarcodeID.ToString();
        }

        private void ToggleEditor(bool showIt)
        {
            pnlEdit.Visible = showIt;
            pnlGrid.Visible = !showIt;
        }

        protected void BindGrid()
        {
            string search = txtSearch.Text;
            
            string sql = "Select b.*, i.ItemName, i.ItemNo, i.Barcode as OriginalBarcode from AlternateBarcode b inner join Item i on b.ItemNo = i.ItemNo " + 
                         "where isnull(i.ItemNo,'') + isnull(i.ItemName,'') + isnull(i.Barcode, '') + isnull(b.barcode,'') like '%" + search + "%' and ISNULL(i.Deleted,0) = 0";
            DataSet ds = DataService.GetDataSet(new QueryCommand(sql));

            if (ds != null)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btnSearchItem_Click(object sender, EventArgs e)
        {
            LoadDropdown(txtSearchItem.Text);
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
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

                string pageCount = "<b>" + GridView1.PageCount.ToString() + "</b>";
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
            BindGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id = Utility.GetParameter("id");
            //bool haveError = false;
            try
            {
                System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                using (System.Transactions.TransactionScope transScope =
                new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                {
                    BindAndSave(id);
                    transScope.Complete();
                    Response.Redirect("AlternateBarcodeScaffold.aspx?id=" + lblBarcodeID.Text + "&msg="+LanguageManager.GetTranslation("Alternate Barcode saved"),false);
                }
            }
            catch (Exception x)
            {
                if (x.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Alternate Barcode ID:") + lblBarcodeID.Text +" "+ LanguageManager.GetTranslation("has already been used. Please use another name.")+"</span> ";
                }
                else
                {
                    //haveError = true;
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Alternate Barcode not saved:") + "</span> " + x.Message;
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            AlternateBarcode.Delete(Utility.GetParameter("id"));
            //redirect
            Response.Redirect(Request.CurrentExecutionFilePath);
        }

        private bool CheckDuplicateAlternateBarcode(string barcode, int BarcodeID)
        {
            if (BarcodeID == 0)
            {
                /*if new*/
                string query = "select ISNULL(MAX(BarcodeID),0) as BarcodeID " +
                                "from AlternateBarcode";
                QueryCommand cmd = new QueryCommand(query);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(cmd));


                if (dt.Rows[0][0] != null && dt.Rows[0][0].ToString() != "{}")
                    BarcodeID = Int32.Parse(dt.Rows[0][0].ToString()) + 1;
            }

            string sqlstring =
                "DECLARE @Barcode varchar(50) " +
                "Declare @BarcodeID int " +
                "set @Barcode = '" + barcode + "' " +
                "set @BarcodeID = " + BarcodeID + " " +
                "select barcode from AlternateBarcode where Barcode = @Barcode and ISNULL(Deleted,0) = 0 and BarcodeID <> @BarcodeID " +
                "union " +
                "select Barcode from PromoCampaignHdr where Barcode = @Barcode and ISNULL(Deleted,0) = 0 " +
                "union " +
                "Select PromoCode from PromoCampaignHdr where PromoCode = @Barcode and ISNULL(Deleted,0) = 0 " +
                "union " +
                "Select barcode from Item where Barcode = @Barcode and ISNULL(Deleted,0) = 0 " +
                "union " +
                "select barcode from ItemGroup where Barcode = @Barcode and ISNULL(Deleted,0) = 0";

            DataTable d = new DataTable();
            d.Load(DataService.GetReader(new QueryCommand(sqlstring)));

            return d.Rows.Count > 0;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindGrid();
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataSet dt = (DataSet)GridView1.DataSource;
            CommonWebUILib.ExportCSV(dt.Tables[0], this.Page.Title.Trim(' '), this.Page.Title, GridView1);
        }

    }
}
