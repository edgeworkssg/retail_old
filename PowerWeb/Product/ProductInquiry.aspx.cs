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
using SubSonic.Utilities;
using PowerPOS;
using PowerPOS.Container;
using SubSonic;
using System.Globalization;

public partial class ProductInquiry : PageBase
{
    //private bool isAdd = false;
    private const bool AUTO_GENERATEID = false;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    private DataTable dtPriceScheme;

    protected void Page_Load(object sender, EventArgs e)
    {

        var customCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        var nfi = (NumberFormatInfo)customCulture.NumberFormat.Clone();
        nfi.CurrencyDecimalDigits = 4;
        customCulture.NumberFormat = nfi;
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        #region *) Display: Arrange layout to be shown from Front End POS
        if (Request.QueryString["passcode"] != null
            || Session["passcode"] != null)
        {
            string passcode = Utility.GetParameter("passcode");
            if ((passcode != null && passcode.Length >= 5 && passcode.Substring(0, 5) == "31179") || Session["passcode"].ToString().Substring(0, 5) == "31179")
            {
                this.Master.FindControl("OUTERTABLE1").Visible = false;
                this.Master.FindControl("menu_row").Visible = false;
                if (passcode != null && passcode != "") Session["passcode"] = passcode;
                Session["UserName"] = "edgeworks";
                Session["Role"] = "admin";
                PointOfSaleInfo.PointOfSaleID = int.Parse(Session["passcode"].ToString().Substring(5, 2));
            }
        }
        #endregion

        if (Request.QueryString["id"] != null)
        {
            string id = Utility.GetParameter("id");
            #region *) Display: Show Error Message (If Any)
            if (Request.QueryString["msg"] != null)
            {
                string msg = Utility.GetParameter("msg"); ;
                //lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + msg + "</span>";
            }
            #endregion

        
        }
        else
        {
            ToggleEditor(false);
            if (!Page.IsPostBack)
            {
                SetProductLabels();
                BindGrid();
            }
            txtItemNo.Focus();
        }
    }
    const int ATTRIBUTES1_COL = 12;
    const int ATTRIBUTES2_COL = 13;
    const int ATTRIBUTES3_COL = 14;
    const int ATTRIBUTES4_COL = 15;
    const int ATTRIBUTES5_COL = 16;

    void SetProductLabels()
    {

        GridView1.Columns[ATTRIBUTES1_COL].HeaderText = ProductAttributeInfo.Attributes1;
        GridView1.Columns[ATTRIBUTES2_COL].HeaderText = ProductAttributeInfo.Attributes2;
        GridView1.Columns[ATTRIBUTES3_COL].HeaderText = ProductAttributeInfo.Attributes3;
        GridView1.Columns[ATTRIBUTES4_COL].HeaderText = ProductAttributeInfo.Attributes4;
        GridView1.Columns[ATTRIBUTES5_COL].HeaderText = ProductAttributeInfo.Attributes5;

    }

    /// <summary>
    /// Shows/Hides the Grid and Editor panels
    /// </summary>
    /// <param name="showIt"></param>
    void ToggleEditor(bool showIt)
    {
        pnlGrid.Visible = !showIt;
    }

    /// <summary>
    /// Loads the editor with data
    /// </summary>
    /// <param name="id"></param>
    //------------------------------CUSTOM CODE------------------------------------------
    private string GetTempFolderName()
    {
        //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + @"\";
        string strTempFolderName = Server.MapPath(".") + "\\Temp\\";
        Logger.writeLog(strTempFolderName);
        if (System.IO.Directory.Exists(strTempFolderName))
        {
            return strTempFolderName;
        }
        else
        {
            System.IO.Directory.CreateDirectory(strTempFolderName);
            return strTempFolderName;
        }
    }
    //------------------------------END OF CUSTOM CODE------------------------------------------

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Cells[5].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[5].Text));
                e.Row.Cells[6].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[6].Text));
                e.Row.Cells[7].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[7].Text));
                e.Row.Cells[8].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[8].Text));
                e.Row.Cells[9].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[9].Text));
                e.Row.Cells[15].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[15].Text));
                switch (e.Row.Cells[14].Text.ToUpper())
                {
                    case "":
                        e.Row.Cells[14].Text = "Non Point";
                        break;
                    case "N":
                        e.Row.Cells[14].Text = "Non Point";
                        break;
                    case "A":
                        e.Row.Cells[14].Text = "Package";
                        break;
                    case "D":
                        e.Row.Cells[14].Text = "Point Item";
                        break;
                    default:
                        e.Row.Cells[14].Text = "Non Point";
                        break;
                }
            }
            catch (Exception ex)
            {
                //Unable to convert
                Logger.writeLog(ex);
            }
        }
    }
    //protected void DropDown1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    LoadByItemNo(ddlItemName.SelectedValue);
    //}
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid();
        /*
        SubSonic.Where whr = new SubSonic.Where();
        whr.ColumnName = "ItemNo";
        whr.Comparison = SubSonic.Comparison.Equals;
        whr.ParameterName = "@ItemNo";
        whr.ParameterValue = txtItemNo.Text;
        whr.TableName = "Item";


        if ((new SubSonic.Query("Item")).GetCount("ItemNo", whr) > 0)
        {
            LoadByItemNo(txtItemNo.Text);
        }
        else
        {
            CommonWebUILib.ShowMessage(lblMsg, "Item No does not exist in the system.", CommonWebUILib.MessageType.BadNews);

        }*/
    }
    //public void LoadByItemNo(string itemno)
    //{
    //    if (itemno != "")
    //        Response.Redirect("ProductMaster.aspx?id=" + itemno);
    //}

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)GridView1.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, GridView1);
    }
    protected void BindGrid()
    {
        ItemController it = new ItemController();
        DataTable dt = it.SearchItem_PlusPointInfo(txtItemNo.Text, false);
        dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
        //dt = ItemController.ConvertGSTRuleToString(dt);
        //dt= CommonUILib.ConvertDataTableForDisplay(dt);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid();
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
}
