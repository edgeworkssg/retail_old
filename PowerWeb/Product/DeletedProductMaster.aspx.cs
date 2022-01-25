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

public partial class DeletedProductMaster : PageBase
{
    //private bool isAdd = false;
    private const bool AUTO_GENERATEID = false;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["passcode"] != null
            || Session["Passcode"] != null)
        {
            string passcode = Utility.GetParameter("Passcode");
            if ((passcode != null && passcode.Length >= 5 && passcode.Substring(0, 5) == "31179") || Session["Passcode"].ToString().Substring(0, 5) == "31179")
            {
                this.Master.FindControl("OUTERTABLE1").Visible = false;
                this.Master.FindControl("menu_row").Visible = false;
                if (passcode != null && passcode != "") Session["Passcode"] = passcode;
                Session["UserName"] = "edgeworks";
                Session["Role"] = "Cashier";
                PointOfSaleInfo.PointOfSaleID = int.Parse(Session["Passcode"].ToString().Substring(5, 2));
            }
        }
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
                    ViewState["IsNew"] = false;
                    SetProductLabels();
                    ddlCategoryName.DataSource = ItemController.FetchCategoryNames();
                    ddlCategoryName.DataBind();
                    LoadEditor(id);
                }
            }

            else
            {
                //it's an add, show the editor
                //isAdd = true;
                //lblID.ReadOnly = true;
                //lblID.Text = "-";
                if (!IsPostBack)
                {
                    ViewState["IsNew"] = true;
                    SetProductLabels();
                    ddlCategoryName.DataSource = ItemController.FetchCategoryNames();
                    ddlCategoryName.DataBind();
                    txtItemNoEditor.Text = ItemController.getNewItemRefNo();
                }
                ToggleEditor(true);
                btnDelete.Visible = false;
            }
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
    const int ATTRIBUTES1_COL = 8;
    const int ATTRIBUTES2_COL = 9;
    const int ATTRIBUTES3_COL = 10;
    const int ATTRIBUTES4_COL = 11;
    const int ATTRIBUTES5_COL = 12;

    void SetProductLabels()
    {
        lblAttributes1.Text = ProductAttributeInfo.Attributes1;
        lblAttributes2.Text = ProductAttributeInfo.Attributes2;
        lblAttributes3.Text = ProductAttributeInfo.Attributes3;
        lblAttributes4.Text = ProductAttributeInfo.Attributes4;
        lblAttributes5.Text = ProductAttributeInfo.Attributes5;

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
        pnlEdit.Visible = showIt;
        pnlGrid.Visible = !showIt;
    }

    /// <summary>
    /// Loads the editor with data
    /// </summary>
    /// <param name="id"></param>
    void LoadEditor(string id)
    {
        ToggleEditor(true);
        if (!String.IsNullOrEmpty(id) && id != "0")
        {
            txtItemNoEditor.Text = id.ToString();
            txtItemNoEditor.ReadOnly = true;

            //pull the record
            Item item = new Item(id);
            //bind the page 
            txtBarcode.Text = item.Barcode;

            txtItemName.Text = item.ItemName;

            ddlCategoryName.SelectedValue = item.CategoryName.ToString();

            txtRetailPrice.Text = item.RetailPrice.ToString("N2").Replace(",", "");

            txtFactoryPrice.Text = item.FactoryPrice.ToString("N2").Replace(",", "");

            if (item.IsServiceItem.GetValueOrDefault(false) && !item.IsInInventory)
            {
                rbService.Checked = true;
            }
            else if (item.IsServiceItem.GetValueOrDefault(false) && item.IsInInventory)
            {
                rbOpenPriceProduct.Checked = true;
            }
            else if (item.PointGetMode == Item.PointMode.Dollar)
            {
                rbPoint.Checked = true;
                txtPointGet.Text = item.PointGetAmount.ToString("N2");
            }
            else if (item.PointGetMode == Item.PointMode.Times)
            {
                rbCourse.Checked = true;
                txtTimesGet.Text = item.PointGetAmount.ToString("N0");
                txtBreakdownPrice.Text = item.Userfloat3.GetValueOrDefault(0).ToString("N2");
            }
            else
            {
                rbProduct.Checked = true;
            }
            //cbIsInInventory.Checked = item.IsInInventory;

            cbIsNonDiscountable.Checked = item.IsNonDiscountable;
            //if (item.IsCommission.HasValue && item.IsCommission.Value)
            //{
            //    cbCommission.Checked = true;
            //}
            //else
            //{
            //    cbCommission.Checked = false;
            //}

            #region *) Get Point[Get] Mode & Amount
            //if (item.PointGetMode == Item.PointMode.Times)
            //{
            //    cmbPointGetType.SelectedIndex = 2;
            //}
            //else if (item.PointGetMode == Item.PointMode.Dollar)
            //{
            //    cmbPointGetType.SelectedIndex = 1;
            //}
            //else
            //{
            //    cmbPointGetType.SelectedIndex = 0;
            //}

            //txtPointGetValue.Text = item.PointGetAmount.ToString("N2");
            #endregion

            #region *) Get Point[Redeem] Mode & Amount
            //if (item.PointRedeemMode == Item.PointMode.Times)
            //{
            //    cmbPointRedeemType.SelectedIndex = 2;
            //}
            //else if (item.PointRedeemMode == Item.PointMode.Dollar)
            //{
            //    cmbPointRedeemType.SelectedIndex = 1;
            //}
            //else
            //{
            //    cmbPointRedeemType.SelectedIndex = 0;
            //}

            //txtPointRedeemValue.Text = item.PointRedeemAmount.ToString("N2");
            #endregion

            //txtPointRedeemValue.Text = "";
            //if (item.Userfloat3.HasValue) txtPointRedeemValue.Text = item.Userfloat3.Value.ToString("N2");

            //if (item.IsServiceItem.HasValue && item.IsServiceItem.Value)
            //{
            //    cbIsServiceItem.Checked = true;
            //}
            //else
            //{
            //    cbIsServiceItem.Checked = false;
            //}

            if (item.GSTRule.HasValue)
                ddGST.SelectedIndex = item.GSTRule.Value;

            txtItemDesc.Text = item.ItemDesc;

            txtAttributes1.Text = item.Attributes1;

            txtAttributes2.Text = item.Attributes2;

            txtAttributes3.Text = item.Attributes3;

            txtAttributes5.Text = item.Attributes5;

            txtAttributes4.Text = item.Attributes4;

            txtRemark.Text = item.Remark;

            //Load picture if any.....
            //TO DO: Item Picture logic

            //set the delete confirmation
            btnDelete.Attributes.Add("onclick", "return CheckDelete();");
        }

    }
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //string id = Utility.GetParameter("id");        
        try
        {
            if (ddGST.SelectedIndex == 0)
            {

            }
            Item item;


            if (!((bool)ViewState["IsNew"]))
            {
                item = new Item(txtItemNoEditor.Text);
                item.IsNew = false;
            }
            else
            {
                item = new Item();
                item.ItemNo = txtItemNoEditor.Text;
                item.IsNew = true;
                item.UniqueID = Guid.NewGuid();
            }

            item.Barcode = txtBarcode.Text;

            item.ItemName = txtItemName.Text;

            item.CategoryName = ddlCategoryName.SelectedValue;

            item.RetailPrice = decimal.Parse(txtRetailPrice.Text.Replace(",", ""));

            item.FactoryPrice = decimal.Parse(txtFactoryPrice.Text.Replace(",", ""));

            //item.IsInInventory = cbIsInInventory.Checked;

            item.IsNonDiscountable = cbIsNonDiscountable.Checked;
            
            if (rbService.Checked)
            {
                item.IsInInventory = false;
                item.IsServiceItem = true;
                item.IsCommission = true;
                item.PointGetAmount = 0;
                item.PointGetMode = Item.PointMode.None;
                item.PointRedeemAmount = 0;
                item.PointRedeemMode = Item.PointMode.Dollar;
                item.Userfloat3 = null; /// Course Breakdown Price
            }
            else if (rbPoint.Checked)
            {
                item.IsInInventory = false;
                item.IsServiceItem = false;
                item.IsCommission = false;
                decimal tempDec = 0; decimal.TryParse(txtPointGet.Text, out tempDec);
                item.PointGetAmount = tempDec;
                item.PointGetMode = Item.PointMode.Dollar;
                item.PointRedeemAmount = 0;
                item.PointRedeemMode = Item.PointMode.None;
                item.Userfloat3 = null; /// Course Breakdown Price
            }
            else if (rbCourse.Checked)
            {
                item.IsInInventory = false;
                item.IsServiceItem = false;
                item.IsCommission = false;
                decimal tempDec = 0; decimal.TryParse(txtTimesGet.Text, out tempDec);
                item.PointGetAmount = tempDec;
                item.PointGetMode = Item.PointMode.Times;
                item.PointRedeemAmount = 0;
                item.PointRedeemMode = Item.PointMode.None;
                decimal.TryParse(txtBreakdownPrice.Text, out tempDec);
                item.Userfloat3 = tempDec; /// Course Breakdown Price
            }
            else if (rbOpenPriceProduct.Checked)
            {
                item.IsInInventory = true;
                item.IsServiceItem = true;
                item.IsCommission = false;
                item.PointGetAmount = 0;
                item.PointGetMode = Item.PointMode.None;
                item.PointRedeemAmount = 0;
                item.PointRedeemMode = Item.PointMode.None;
                item.Userfloat3 = null; /// Course Breakdown Price
            }
            else /// Categorized as Product
            {
                item.IsInInventory = true;
                item.IsServiceItem = false;
                item.IsCommission = false;
                item.PointGetAmount = 0;
                item.PointGetMode = Item.PointMode.None;
                item.PointRedeemAmount = 0;
                item.PointRedeemMode = Item.PointMode.None;
                item.Userfloat3 = null; /// Course Breakdown Price
            }

            //item.IsServiceItem = cbIsServiceItem.Checked;

            //item.IsCommission = cbCommission.Checked;           

            //if (item.IsServiceItem.HasValue &&
            //    item.IsServiceItem.Value) cbIsServiceItem.Checked = true;

            //item.PointGetMode = cmbPointGetType.SelectedValue;

            //item.PointGetAmount = decimal.Parse(txtPointGetValue.Text.Replace(",", ""));

            //item.PointRedeemMode = cmbPointRedeemType.SelectedValue;

            //item.PointRedeemAmount = decimal.Parse(txtPointRedeemValue.Text.Replace(",", ""));

            //if (decimal.TryParse(txtPointRedeemValue.Text.Replace(",",""),out tmpParse ))
            //    item.Userfloat3 = tmpParse;

            item.GSTRule = ddGST.SelectedIndex;

            item.ItemDesc = txtItemDesc.Text;

            item.Attributes1 = txtAttributes1.Text;

            item.Attributes2 = txtAttributes2.Text;

            item.Attributes3 = txtAttributes3.Text;

            item.Attributes5 = txtAttributes5.Text;

            item.Attributes4 = txtAttributes4.Text;

            item.Remark = txtRemark.Text;

            /*------------------CUSTOM CODE----------------------------
            if (FileUpload1.HasFile)
            {
                ItemController itemCtr = new ItemController();
                itemCtr.UploadPicture(
                  id, FileUpload1.
                  FileBytes, FileUpload1.FileName.Split('.')[1].ToUpper());
            }*/

            item.Deleted = false;
            item.Save(Session["username"].ToString());
            ViewState["IsNew"] = false;
            txtItemNoEditor.ReadOnly = true;

            /*
            ItemController itemCtr = new ItemController();
            if (!String.IsNullOrEmpty(id) && id != "0")
            {

                //it's an edit
                //CALL UPDATE METHOD USING THE CONTROLLER
                itemCtr.Update(lblID.Text, txtItemName.Text,
                    txtBarcode.Text, ddlCategoryName.SelectedItem.Text,
                    decimal.Parse(txtRetailPrice.Text), Decimal.Parse(txtFactoryPrice.Text), txtItemDesc.Text,
                    cbIsInInventory.Checked, "", 
                    txtRemark.Text, Session["UserName"].ToString(),
                    cbIsNonDiscountable.Checked, txtAttributes1.Text, txtAttributes2.Text,
                    txtAttributes3.Text, txtAttributes4.Text, txtAttributes5.Text, "");                 
            }
            else
            {
                if (txtRetailPrice.Text.Trim() == "") txtRetailPrice.Text = "0";
                if (txtFactoryPrice.Text.Trim() == "") txtFactoryPrice.Text = "0";
                
                //add
                //CALL INSERT METHOD USING THE CONTROLLER
                id = itemCtr.Create(lblID.Text,
                    txtItemName.Text, txtBarcode.Text, 
                    ddlCategoryName.SelectedItem.Text, 
                    decimal.Parse(txtRetailPrice.Text), 
                    decimal.Parse(txtFactoryPrice.Text), 
                    txtItemDesc.Text, 
                    cbIsInInventory.Checked, "", 
                    txtRemark.Text,
                    cbIsNonDiscountable.Checked, txtAttributes1.Text, txtAttributes2.Text,
                    txtAttributes3.Text, txtAttributes4.Text, txtAttributes5.Text, "", AUTO_GENERATEID);
            }

            //------------------CUSTOM CODE----------------------------
            if (FileUpload1.HasFile)
            {
                itemCtr.UploadPicture(
                  id,FileUpload1.
                  FileBytes,FileUpload1.FileName.Split('.')[1].ToUpper());                                    
            }
            //------------------------END OF CUSTOM CODE---------------------------
            */
            //LoadEditor(id);
            //lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">Product saved.</span>";
            Response.Redirect("DeletedproductMaster.aspx?id=" + item.ItemNo + "&msg=Product saved");

        }

        catch (Exception x)
        {
            Logger.writeLog(x);
            if (x.Message.Contains("Violation of PRIMARY KEY constraint"))
            {
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">Product not saved: Item No: " + txtItemNoEditor.Text + " has already been used. Choose another name</span> ";
            }
            else
            {
                //haveError = true;
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">Product not saved:</span> " + x.Message;
            }
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Query qr = Item.CreateQuery();
        qr.QueryType = QueryType.Update;
        qr.AddUpdateSetting(Item.Columns.Deleted, false);
        qr.AddWhere(Item.Columns.ItemNo, Utility.GetParameter("id"));
        qr.Execute();
        
        Response.Redirect(Request.CurrentExecutionFilePath);
    }


    protected void btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("DeletedProductMaster.aspx?id=0");
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Cells[5].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[5].Text));
                e.Row.Cells[14].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[14].Text));
                switch (e.Row.Cells[13].Text.ToUpper())
                {
                    case "":
                        e.Row.Cells[13].Text = "Non Point";
                        break;
                    case "N":
                        e.Row.Cells[13].Text = "Non Point";
                        break;
                    case "A":
                        e.Row.Cells[13].Text = "Package";
                        break;
                    case "D":
                        e.Row.Cells[13].Text = "Point Item";
                        break;
                    default:
                        e.Row.Cells[13].Text = "Non Point";
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
    protected void DropDown1_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadByItemNo(ddlItemName.SelectedValue);
    }
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
    public void LoadByItemNo(string itemno)
    {
        if (itemno != "")
            Response.Redirect("DeletedProductMaster.aspx?id=" + itemno);
    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)GridView1.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, GridView1);
    }
    protected void BindGrid()
    {
        ItemController it = new ItemController();
        DataTable dt = it.SearchDeletedItem_PlusPointInfo(txtItemNo.Text, false);
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
