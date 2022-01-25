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
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;
using System.Linq;

namespace PowerWeb.Reports
{
    public partial class ItemTagReport : PageBase
    {
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string SORT_BY = "SORT_BY";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearchItem_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Text = "";
                if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
                    return;

                string search = txtSearch.Text;
                Clear();

                string sql = @"
                DECLARE @Search NVARCHAR(500);

                SET @Search = @Search_;

                SELECT   I.ItemNo
		                ,I.ItemName
                FROM	Item I
                WHERE	ISNULL(I.Deleted,0) = 0
		                AND ( I.ItemNo LIKE '%'+@Search+'%'
		                    OR I.ItemName LIKE '%'+@Search+'%'
		                    OR I.Barcode LIKE '%'+@Search+'%'
		                    OR I.CategoryName LIKE '%'+@Search+'%' )
                ORDER BY I.ItemName";
                QueryCommand cmd = new QueryCommand(sql);
                cmd.AddParameter("@Search_", search);
 
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(cmd));
                ddlItem.DataSource = dt;
                ddlItem.DataBind();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            string itemNo = ddlItem.SelectedValue + "";
            string serialNo = txtSerialNo.Text;

            if (string.IsNullOrEmpty(itemNo))
                return;

            if (string.IsNullOrEmpty(serialNo))
                return;

            string sql = @"
            SELECT  TOP 1 *
            FROM	ItemTagSummary ITS
            WHERE	ISNULL(ITS.Deleted,0) = 0
		            AND ITS.ItemNo = @ItemNo
		            AND ITS.SerialNo = @SerialNo";

            QueryCommand cmd = new QueryCommand(sql);
            cmd.AddParameter("@ItemNo", itemNo);
            cmd.AddParameter("@SerialNo", serialNo);

            var itemTagColl = new ItemTagSummaryCollection();
            itemTagColl.Load(DataService.GetReader(cmd));

            var itemTag = itemTagColl.FirstOrDefault();

            if (itemTag == null)
            {
                Clear();
                lblMessage.Text = "Serial No not found";
                return;
            }

            lblItemName.Text = itemTag.Item.ItemName;
            lblSerialNo.Text = itemTag.SerialNo;
            lblStatus.Text = itemTag.IsAvailable.GetValueOrDefault(false) ? "Available" : "Not Available";
            lblLocation.Text = itemTag.InventoryLocation.InventoryLocationName;
            hfItemNo.Value = itemTag.ItemNo;
            divResult.Visible = true;
            BindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void Clear()
        {
            hfItemNo.Value = "";
            lblItemName.Text = "";
            lblSerialNo.Text = "";
            lblStatus.Text = "";
            lblMessage.Text = "";
            lblLocation.Text = "";
            txtSearch.Text = "";
            txtSerialNo.Text = "";
            ddlItem.DataSource = new DataTable();
            ddlItem.DataBind();
            gvReport.DataSource = new DataTable();
            gvReport.DataBind();
            divResult.Visible = false;
        }

        protected void BindGrid()
        {
            string sql = @"
            DECLARE @SerialNo NVARCHAR(500);
            DECLARE @ItemNo VARCHAR(50);
            DECLARE @Temp AS TABLE
            (
	            ItemNo VARCHAR(50),
	            SerialNo VARCHAR(50),
	            InventoryLocationID INT,
	            InventoryLocationName VARCHAR(500),
	            MovementType VARCHAR(500),
	            InventoryDetRefNo VARCHAR(500),
	            InventoryDate DATETIME
            )

            SET @ItemNo = @ItemNo_;
            SET @SerialNo = @SerialNo_;

            INSERT INTO @Temp
            SELECT   ITS.ItemNo
		            ,ITS.SerialNo
		            ,ITS.InventoryLocationID
		            ,IL.InventoryLocationName
		            ,ITS.MovementType
		            ,ITS.InventoryDetRefNo
		            ,ITS.InventoryDate
            FROM	ItemTagStatusDetail ITS
		            INNER JOIN InventoryLocation IL ON IL.InventoryLocationID = ITS.InventoryLocationID
            WHERE	ISNULL(ITS.Deleted,0) = 0
		            AND ITS.ItemNo = @ItemNo
		            AND ITS.SerialNo = @SerialNo

            SELECT   TAB.InventoryDate
		            ,TAB.ItemNo
		            ,TAB.SerialNo
		            ,CASE WHEN TAB.MovementType LIKE '%Transfer%' THEN 'Transfer'
			              ELSE TAB.MovementType END Movement
		            ,CASE WHEN TAB.MovementType LIKE '%Out%' THEN TAB.InventoryLocationName
			              ELSE ISNULL(TAB2.InventoryLocationName,'') END FromLocation
		            ,CASE WHEN TAB.MovementType LIKE '%In%' THEN TAB.InventoryLocationName
			              ELSE '' END ToLocation
            FROM	@Temp TAB
		            LEFT JOIN @Temp TAB2 ON TAB2.ItemNo = TAB.ItemNo
							             AND TAB2.SerialNo = TAB.SerialNo	
							             AND TAB2.InventoryDetRefNo = TAB.InventoryDetRefNo	
							             AND TAB2.MovementType = 'Transfer Out'
            WHERE	TAB.MovementType <> 'Transfer Out'
            ORDER BY TAB.InventoryDate DESC";

            QueryCommand cmd = new QueryCommand(sql);
            cmd.AddParameter("@ItemNo_", hfItemNo.Value + "");
            cmd.AddParameter("@SerialNo_", lblSerialNo.Text);

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            gvReport.DataSource = dt;
            gvReport.DataBind();
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
                DataTable dt = gvReport.DataSource as DataTable;
                if (dt != null)
                    itemCount = dt.Rows.Count;

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

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState[SORT_BY] = e.SortExpression;

            if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
                ViewState[SORT_DIRECTION] = SqlFragment.DESC;
            else
                ViewState[SORT_DIRECTION] = SqlFragment.ASC;

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

    }
}
