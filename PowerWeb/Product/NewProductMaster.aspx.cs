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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public partial class NewProductMaster : PageBase
{
    //private bool isAdd = false;
    private const bool AUTO_GENERATEID = false;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    private const int Factory_Price = 7;
    private const int ATTRIBUTES1_COL = 9;
    private const int ATTRIBUTES2_COL = 10;
    private const int ATTRIBUTES3_COL = 11;
    private const int ATTRIBUTES4_COL = 12;
    private const int ATTRIBUTES5_COL = 13;
    private const int ATTRIBUTES6_COL = 14;
    private const int ATTRIBUTES7_COL = 15;
    private const int ATTRIBUTES8_COL = 16;
    private const int colItemNo = 2;
    private DataTable dtPriceScheme;
    private DataTable dtSupplier;
    private DataTable dtBarcode;
    private string P1Name, P2Name, P3Name, P4Name, P5Name;

    #region *) Method

    private bool SetUserOutletAssignment()
    {
        var outletList = OutletController.FetchByUserNameForReport(false, false, Session["UserName"] + "");
        var allOutlet = OutletController.FetchAll(false, false);
        bool isAssignedToAll = outletList.Count >= allOutlet.Count;
        ddlApplicableTo.Enabled = isAssignedToAll;
        int selectedIndex = ddlOutletList.SelectedIndex;
        string selectedValue = ddlOutletList.SelectedValue;
        if (!isAssignedToAll)
        {
            ddlApplicableTo.SelectedIndex = 2;
            ddlOutletList.Items.Clear();
            foreach (var ou in outletList)
                ddlOutletList.Items.Add(ou.OutletName);
            ApplicableTo = "Outlet";

            if (ddlOutletList.Items.Count > 0)
            {
                if (selectedIndex < 0)
                {
                    ddlOutletList.SelectedIndex = 0;
                }
                else
                {
                    ddlOutletList.SelectedValue = selectedValue;
                }

            }
            ddlOutlet_SelectedIndexChanged(ddlOutletList, new EventArgs());

            //if (ddlOutletList.Items.Count <= 1)
            //{
            //    ApplicableTo = "Product Master";
            //    ddlApplicableTo.SelectedIndex = 0;
            //}
        }



        return isAssignedToAll;
    }

    private void SetFormEnable()
    {
        rowNonInventoryProduct.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayNonInventoryProduct), false);
        txtBarcode.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtPrefix.Enabled = ddlApplicableTo.SelectedIndex == 0;
        gvBarcode.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtItemName.Enabled = ddlApplicableTo.SelectedIndex == 0 || (ddlApplicableTo.SelectedIndex != 0 && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false));
        ddlCategoryName.Enabled = ddlApplicableTo.SelectedIndex == 0 || (ddlApplicableTo.SelectedIndex != 0 && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.IsEditCategory_ProductOutletSetup), false)); ;
        txtFactoryPrice.Enabled = ddlApplicableTo.SelectedIndex == 0 || (ddlApplicableTo.SelectedIndex != 0 && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.IsEditCostPrice_ProductOutletSetup), false));
        cbIsNonDiscountable.Enabled = ddlApplicableTo.SelectedIndex == 0;
        ddGST.Enabled = ddlApplicableTo.SelectedIndex == 0;
        cbGiveCommission.Enabled = ddlApplicableTo.SelectedIndex == 0;
        cbPointRedeemable.Enabled = ddlApplicableTo.SelectedIndex == 0;
        cbAutoCaptureWeight.Enabled = ddlApplicableTo.SelectedIndex == 0;
        rbProduct.Enabled = ddlApplicableTo.SelectedIndex == 0;
        rbCourse.Enabled = ddlApplicableTo.SelectedIndex == 0;
        rbOpenPriceProduct.Enabled = ddlApplicableTo.SelectedIndex == 0;
        rbPoint.Enabled = ddlApplicableTo.SelectedIndex == 0;
        rbService.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtPointGet.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtTimesGet.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtBreakdownPrice.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtItemDesc.Enabled = ddlApplicableTo.SelectedIndex == 0;
        MatrixAttributes3.Enabled = ddlApplicableTo.SelectedIndex == 0;
        MatrixAttributes4.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtAddAtt3.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtAddAtt4.Enabled = ddlApplicableTo.SelectedIndex == 0;
        btnAddAtt3.Enabled = ddlApplicableTo.SelectedIndex == 0;
        btnAddAtt4.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtAttributes1.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtAttributes2.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtAttributes3.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtAttributes4.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtAttributes5.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtAttributes6.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtAttributes7.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtAttributes8.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtRemark.Enabled = ddlApplicableTo.SelectedIndex == 0;
        fuItemPicture.Enabled = ddlApplicableTo.SelectedIndex == 0;
        btnRemoveImage.Enabled = ddlApplicableTo.SelectedIndex == 0;
        chkIsConsigment.Enabled = ddlApplicableTo.SelectedIndex == 0;
        ddlSupplier.Enabled = ddlApplicableTo.SelectedIndex == 0;
        rbNonInventoryProduct.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtSearchItem.Enabled = ddlApplicableTo.SelectedIndex == 0;
        btnSearchItem.Enabled = ddlApplicableTo.SelectedIndex == 0;
        ddlItem.Enabled = ddlApplicableTo.SelectedIndex == 0;
        btnSetDeductItem.Enabled = ddlApplicableTo.SelectedIndex == 0;
        DeductConvRate.Enabled = ddlApplicableTo.SelectedIndex == 0;
        ddlDeductConvType.Enabled = ddlApplicableTo.SelectedIndex == 0;
        IsUsingFixedCOG.Enabled = ddlApplicableTo.SelectedIndex == 0;
        IsFixedCOGPercentage.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtFixedCOGPercentage.Enabled = ddlApplicableTo.SelectedIndex == 0;
        IsFixedCOGValue.Enabled = ddlApplicableTo.SelectedIndex == 0;
        txtFixedCOGValue.Enabled = ddlApplicableTo.SelectedIndex == 0;
    }

    private string ApplicableTo
    {
        set
        {
            Session["Product_ApplicableTo"] = value;
        }
        get
        {
            return Session["Product_ApplicableTo"] + "";
        }
    }

    private string OutletID
    {
        set
        {
            Session["Product_OutletID"] = value;
        }
        get
        {
            return Session["Product_OutletID"] + "";
        }
    }

    protected void SetProductLabels()
    {
        lblAttributes1.Text = ProductAttributeInfo.Attributes1;
        lblAttributes2.Text = ProductAttributeInfo.Attributes2;
        lblAttributes3.Text = ProductAttributeInfo.Attributes3;
        lblAttributes4.Text = ProductAttributeInfo.Attributes4;
        lblAttributes5.Text = ProductAttributeInfo.Attributes5;
        lblAttributes6.Text = ProductAttributeInfo.Attributes6;
        lblAttributes7.Text = ProductAttributeInfo.Attributes7;
        lblAttributes8.Text = ProductAttributeInfo.Attributes8;

        lblAttributes3Matrix.Text = ProductAttributeInfo.Attributes3;
        lblAttributes4Matrix.Text = ProductAttributeInfo.Attributes4;

        GridView1.Columns[ATTRIBUTES1_COL].HeaderText = ProductAttributeInfo.Attributes1;
        GridView1.Columns[ATTRIBUTES2_COL].HeaderText = ProductAttributeInfo.Attributes2;
        GridView1.Columns[ATTRIBUTES3_COL].HeaderText = ProductAttributeInfo.Attributes3;
        GridView1.Columns[ATTRIBUTES4_COL].HeaderText = ProductAttributeInfo.Attributes4;
        GridView1.Columns[ATTRIBUTES5_COL].HeaderText = ProductAttributeInfo.Attributes5;
        GridView1.Columns[ATTRIBUTES6_COL].HeaderText = ProductAttributeInfo.Attributes6;
        GridView1.Columns[ATTRIBUTES7_COL].HeaderText = ProductAttributeInfo.Attributes7;
        GridView1.Columns[ATTRIBUTES8_COL].HeaderText = ProductAttributeInfo.Attributes8;
    }

    private void ToggleEditor(bool showIt)
    {
        pnlEdit.Visible = showIt;
        pnlGrid.Visible = !showIt;

        var outletList = OutletController.FetchByUserNameForReport(false, false, Session["UserName"] + "");

        if (showIt && (Utility.GetParameter("id") == "0"))
        {
            //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddProductOneOutletOnly), false))
            if (ApplicableTo == "Outlet")
            {
                if (outletList.Count <= 1)
                {
                    ApplicableTo = "Product Master";
                    ddlApplicableTo.SelectedIndex = 0;
                }

                if (ApplicableTo != "")
                {
                    ddlApplicableTo.SelectedValue = ApplicableTo;
                    ddlApplicableTo_SelectedIndexChanged(ddlApplicableTo, new EventArgs());
                }
                if (OutletID != "")
                    ddlOutletList.SelectedValue = OutletID;
            }
            else
            {
                ddlApplicableTo.SelectedIndex = 0;
                ddlOutletList.Text = "";
            }

            //Load Default GST Parameter
            if (AppSetting.GetSetting(AppSetting.SettingsName.Item.DefaultGSTSetting) != "")
            {
                ddGST.Text = AppSetting.GetSetting(AppSetting.SettingsName.Item.DefaultGSTSetting);
            }
        }
        else if (showIt && (Utility.GetParameter("id") != "0"))
        {

            if (outletList.Count <= 1)
            {
                ApplicableTo = "Product Master";
                ddlApplicableTo.SelectedIndex = 0;
            }

            if (ApplicableTo != "")
            {
                ddlApplicableTo.SelectedValue = ApplicableTo;
                ddlApplicableTo_SelectedIndexChanged(ddlApplicableTo, new EventArgs());
            }
            else
            {
                ddlApplicableTo.SelectedIndex = 0;
                ddlApplicableTo_SelectedIndexChanged(ddlApplicableTo, new EventArgs());
            }
            if (OutletID != "")
                ddlOutletList.SelectedValue = OutletID;
        }
        bool isEnableAppTo = true;
        if (!showIt)
            isEnableAppTo = SetUserOutletAssignment();
        ddlApplicableTo.Enabled = !showIt && isEnableAppTo;
        ddlOutletList.Enabled = !showIt;
        //if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddProductOneOutletOnly), false))
        if ((Utility.GetParameter("id") != "0") || ApplicableTo != "Outlet")
        {
            SetFormEnable();
        }
    }

    private void LoadEditor(string id, string matrixmode)
    {
        ToggleEditor(true);
        if (!String.IsNullOrEmpty(id) && id != "0")
        {
            Item item;

            //Load the setting
            string NumDigit = "N" + (String.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Item.NumDigit)) ? "2" : AppSetting.GetSetting(AppSetting.SettingsName.Item.NumDigit));



            //pull the record
            //different treatment for item matrix
            divButton.Visible = false;

            if (matrixmode.ToLower() == "yes")
            {
                item = ItemController.GetDataItemMatrix(id);
                ItemController itemLogic = new ItemController();

                string qry = string.Format("Select * from item where Attributes1 = '{0}' and ISNULL(Deleted,0) = 0", item.Attributes1);
                DataTable dtAtr = DataService.GetDataSet(new QueryCommand(qry)).Tables[0];
                ItemCollection itemCollect = new ItemCollection();
                itemCollect.Load(dtAtr);

                //DataSet ds = qrs.ExecuteDataSet();
                if (itemCollect != null && itemCollect.Count > 0)
                {
                    dtBarcode = new DataTable();
                    dtBarcode.Columns.Add("ItemNo", typeof(string));
                    dtBarcode.Columns.Add("ItemName", typeof(string));
                    dtBarcode.Columns.Add("Barcode", typeof(string));
                    dtBarcode.Columns.Add("Attributes2", typeof(string));
                    dtBarcode.Columns.Add("Attributes3", typeof(string));
                    dtBarcode.Columns.Add("Attributes4", typeof(string));
                    for (int i = 0; i < itemCollect.Count; ++i)
                    {
                        dtBarcode.Rows.Add(itemCollect[i].ItemNo, itemCollect[i].ItemName, itemCollect[i].Barcode, itemCollect[i].Attributes2, itemCollect[i].Attributes3, itemCollect[i].Attributes4);
                    }

                    ViewState["ArrayBarcode"] = dtBarcode;
                }
            }
            else
            {
                item = new Item(id);
            }
            //bind the page 
            ShowWarningWhenSellingPriceLessThanCostPrice.Value = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowWarningWhenSellingPriceLessThanCostPrice), false) ? "1" : "0";
            txtUOM.Text = item.Userfld1;
            txtItemNoEditor.Text = item.ItemNo;
            txtItemNoEditor.ReadOnly = true;
            txtBarcode.Text = item.Barcode;
            txtItemName.Text = item.ItemName;
            ddlCategoryName.SelectedValue = item.CategoryName.ToString();
            txtMinimumPrice.Text = item.MinimumPrice.ToString("N2").Replace(",", "");
            txtRetailPrice.Text = item.RetailPrice.ToString("N2").Replace(",", "");
            txtFactoryPrice.Text = item.FactoryPrice.ToString(NumDigit).Replace(",", "");
            chkDeleted.Checked = item.Deleted.GetValueOrDefault(false);
            cbIsExcludeProfitLossReport.Checked = item.ExcludeProfitLoss == 1;

            //if (ApplicableTo == "Product Master")
            //{
            txtP1.Text = item.Userfloat6.HasValue ? Convert.ToDecimal(item.Userfloat6).ToString("N2").Replace(",", "") : "";
            txtP2.Text = item.Userfloat7.HasValue ? Convert.ToDecimal(item.Userfloat7).ToString("N2").Replace(",", "") : "";
            txtP3.Text = item.Userfloat8.HasValue ? Convert.ToDecimal(item.Userfloat8).ToString("N2").Replace(",", "") : "";
            txtP4.Text = item.Userfloat9.HasValue ? Convert.ToDecimal(item.Userfloat9).ToString("N2").Replace(",", "") : "";
            txtP5.Text = item.Userfloat10.HasValue ? Convert.ToDecimal(item.Userfloat10).ToString("N2").Replace(",", "") : "";
            //}

            if (ddlApplicableTo.SelectedValue == "Outlet Group")
            {
                var query = new Query("OutletGroupItemMap");
                query.AddWhere(OutletGroupItemMap.Columns.ItemNo, Comparison.Equals, item.ItemNo);
                query.AddWhere(OutletGroupItemMap.Columns.OutletGroupID, Comparison.Equals, (ddlOutletList.SelectedValue + "").GetIntValue());
                var ogim = new OutletGroupItemMapController().FetchByQuery(query).FirstOrDefault();
                if (ogim != null)
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false) && !string.IsNullOrEmpty(ogim.ItemName))
                    {
                        if (item.IsMatrixItem)
                        {
                            string[] matName = ogim.ItemName.Split('-');
                            txtItemName.Text = matName[0];
                        }
                        else
                            txtItemName.Text = ogim.ItemName;
                    }

                    txtRetailPrice.Text = ogim.RetailPrice.ToString("NumDigit").Replace(",", "");
                    txtFactoryPrice.Text = ogim.CostPrice.ToString(NumDigit).Replace(",", "");
                    chkDeleted.Checked = ogim.Deleted;

                    if (P1Name != null && P1Name != "")
                    {
                        txtP1.Text = ogim.P1 == null ? "" : ogim.P1.GetValueOrDefault(0).ToString("N2").Replace(",", "");
                    }

                    if (P2Name != null && P2Name != "")
                    {
                        txtP2.Text = ogim.P2 == null ? "" : ogim.P2.GetValueOrDefault(0).ToString("N2").Replace(",", "");
                    }

                    if (P3Name != null && P3Name != "")
                    {
                        txtP3.Text = ogim.P3 == null ? "" : ogim.P3.GetValueOrDefault(0).ToString("N2").Replace(",", "");
                    }

                    if (P4Name != null && P4Name != "")
                    {
                        txtP4.Text = ogim.P4 == null ? "" : ogim.P4.GetValueOrDefault(0).ToString("N2").Replace(",", "");
                    }

                    if (P5Name != null && P5Name != "")
                    {
                        txtP5.Text = ogim.P5 == null ? "" : ogim.P5.GetValueOrDefault(0).ToString("N2").Replace(",", "");
                    }
                }
            }
            else if (ddlApplicableTo.SelectedValue == "Outlet")
            {
                var query = new Query("OutletGroupItemMap");
                query.AddWhere(OutletGroupItemMap.Columns.ItemNo, Comparison.Equals, item.ItemNo);
                query.AddWhere(OutletGroupItemMap.Columns.OutletName, Comparison.Equals, (ddlOutletList.SelectedValue + ""));
                var ogim = new OutletGroupItemMapController().FetchByQuery(query).FirstOrDefault();
                if (ogim != null)
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false) && !string.IsNullOrEmpty(ogim.ItemName))
                    {
                        if (item.IsMatrixItem)
                        {
                            string[] matName = ogim.ItemName.Split('-');
                            txtItemName.Text = matName[0];
                        }
                        else
                            txtItemName.Text = ogim.ItemName;
                    }
                    txtRetailPrice.Text = ogim.RetailPrice.ToString("N2").Replace(",", "");
                    txtFactoryPrice.Text = ogim.CostPrice.ToString(NumDigit).Replace(",", "");
                    chkDeleted.Checked = ogim.Deleted;

                    if (P1Name != null && P1Name != "")
                    {
                        txtP1.Text = ogim.P1 == null ? "" : ogim.P1.GetValueOrDefault(0).ToString("N2").Replace(",", "");
                    }

                    if (P2Name != null && P2Name != "")
                    {
                        txtP2.Text = ogim.P2 == null ? "" : ogim.P2.GetValueOrDefault(0).ToString("N2").Replace(",", "");
                    }

                    if (P3Name != null && P3Name != "")
                    {
                        txtP3.Text = ogim.P3 == null ? "" : ogim.P3.GetValueOrDefault(0).ToString("N2").Replace(",", "");
                    }

                    if (P4Name != null && P4Name != "")
                    {
                        txtP4.Text = ogim.P4 == null ? "" : ogim.P4.GetValueOrDefault(0).ToString("N2").Replace(",", "");
                    }

                    if (P5Name != null && P5Name != "")
                    {
                        txtP5.Text = ogim.P5 == null ? "" : ogim.P5.GetValueOrDefault(0).ToString("N2").Replace(",", "");
                    }
                }
            }

            txtItemDesc.Text = item.ItemDesc;
            if (item.ItemImage != null)
            {
                Image2.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(item.ItemImage, 0, item.ItemImage.Length);
            }

            rowNonInventoryProduct.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayNonInventoryProduct), false);

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
                cbOpenPackace.Checked = item.IsOpenPricePackage;
            }
            else if (item.NonInventoryProduct)
            {
                rbNonInventoryProduct.Checked = true;
                Item it = new Item(item.DeductedItem);

                if (it != null)
                {
                    DeductedItemNo.Value = it.ItemNo;
                    DeductedItemLabel.Text = it.ItemName;
                    DeductedUOM.Text = !string.IsNullOrEmpty(it.UOM) ? it.UOM : "pcs";
                    DeductConvRate.Text = item.DeductConvRate.ToString("N2");
                    ddlDeductConvType.SelectedValue = item.DeductConvType.ToString().ToLower();
                }
            }
            else
            {
                rbProduct.Checked = true;
            }

            cbIsNonDiscountable.Checked = item.IsNonDiscountable;
            cbGiveCommission.Checked = item.IsCommission.GetValueOrDefault(false);
            cbPointRedeemable.Checked = (item.PointRedeemMode != null && item.PointRedeemMode == Item.PointMode.Dollar);
            cbAutoCaptureWeight.Checked = item.AutoCaptureWeight;

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

            if (item.GSTRule.HasValue)
                ddGST.SelectedIndex = item.GSTRule.Value;

            if (item.Userflag1 == true)
            {
                var lattb3 = item.Attributes3.Split(',');
                foreach (string att3 in lattb3)
                {
                    ListItem x = MatrixAttributes3.Items.FindByValue(att3.Trim());
                    if (x != null)
                    {
                        x.Selected = true;
                    }
                }

                var lattb4 = item.Attributes4.Split(',');
                foreach (string att4 in lattb4)
                {
                    ListItem x = MatrixAttributes4.Items.FindByValue(att4.Trim());
                    if (x != null)
                    {
                        x.Selected = true;
                    }
                }

                txtAttributes5.Text = item.Attributes5;
                txtAttributes6.Text = item.Attributes6;
                txtAttributes7.Text = item.Attributes7;
                txtAttributes8.Text = item.Attributes8;
            }
            else
            {
                txtAttributes1.Text = item.Attributes1;
                txtAttributes2.Text = item.Attributes2;
                txtAttributes3.Text = item.Attributes3;
                txtAttributes4.Text = item.Attributes4;
                txtAttributes5.Text = item.Attributes5;
                txtAttributes6.Text = item.Attributes6;
                txtAttributes7.Text = item.Attributes7;
                txtAttributes8.Text = item.Attributes8;

            }
            txtRemark.Text = item.Remark;

            cbPreOrder.Checked = item.AllowPreOrder;
            int capQty = item.CapQty;
            int soldQty = ItemController.FetchPreOrderSoldQty(item.ItemNo);
            int balQty = capQty - soldQty;
            txtPreOrderCapQty.Text = capQty.ToString();
            txtPreOrderSoldQty.Text = soldQty.ToString();
            txtPreOrderBalQty.Text = balQty.ToString();

            cbVendorDelivery.Checked = item.IsVendorDelivery;
            chkIsConsigment.Checked = item.IsConsignment;

            cbPAMed.Checked = item.IsPAMedifund;
            cbSMF.Checked = item.IsSMF;
            txtPAMed.Text = ItemController.FetchPriceForFunding(item.ItemNo, AppSetting.SettingsName.Funding.PAMedPercentage).ToString("N2");
            txtSMF.Text = ItemController.FetchPriceForFunding(item.ItemNo, AppSetting.SettingsName.Funding.SMFPercentage).ToString("N2");

            IsUsingFixedCOG.Checked = item.IsUsingFixedCOG;
            if (item.IsUsingFixedCOG)
            {
                if (item.FixedCOGType == ItemController.FIXEDCOG_PERCENTAGE)
                {
                    IsFixedCOGPercentage.Checked = true;
                    txtFixedCOGPercentage.Text = item.FixedCOGValue.ToString("N2");
                }
                else
                {
                    IsFixedCOGValue.Checked = true;
                    txtFixedCOGValue.Text = item.FixedCOGValue.ToString("N2");
                }
            }

            //Load picture if any.....
            //TO DO: Item Picture logic

            //Adi 20170720 Add Created By and CreatedOn
            LtrlCreatedBy.Text = item.CreatedBy;
            LtrlCreatedOn.Text = item.CreatedOn.GetValueOrDefault(DateTime.Today).ToString("dd MMM yyyy HH:mm:ss");

            if (matrixmode.ToLower() == "yes")
            {
                string que = @"select  distinct itsm.SupplierID
                    from ItemSupplierMap itsm 
                    inner join Item it on it.ItemNo = itsm.ItemNo
                    where isnull(it.attributes1,'') = '{0}'";

                que = string.Format(que, item.Attributes1);

                DataTable dt = DataService.GetDataSet(new QueryCommand(que)).Tables[0];

                if (dt.Rows.Count > 0)
                    ddlSupplier.SelectedValue = dt.Rows[0][0].ToString();
            }
            else
            {

                //Adi 20170823 Show Supplier
                ItemSupplierMapCollection ismCol = new ItemSupplierMapCollection();
                ismCol.Where(ItemSupplierMap.Columns.ItemNo, item.ItemNo);
                ismCol.OrderByDesc(ItemSupplierMap.Columns.IsPreferredSupplier);
                ismCol.Load();
                if (ismCol.Count > 0)
                {
                    ddlSupplier.SelectedValue = ismCol[0].SupplierID.ToString();
                }
            }



            #region *) Load Future Price

            string sqlFuturePrice = @"
DECLARE @ItemPrice AS TABLE 
(
	 ItemNo VARCHAR(50) ,RetailPrice MONEY
	,P1 MONEY ,P2 MONEY ,P3 MONEY ,P4 MONEY ,P5 MONEY
)
IF @ApplicableTo = 'Outlet Group' BEGIN
	INSERT INTO @ItemPrice
	SELECT   ItemNo ,RetailPrice
			,P1 ,P2 ,P3 ,P4 ,P5
	FROM OutletGroupItemMap
	WHERE ItemNo = @ItemNo
		  AND OutletGroupID = @OutletID
END
ELSE IF @ApplicableTo = 'Outlet' BEGIN
	INSERT INTO @ItemPrice
	SELECT   ItemNo ,RetailPrice
			,P1 ,P2 ,P3 ,P4 ,P5
	FROM OutletGroupItemMap
	WHERE ItemNo = @ItemNo
		  AND OutletName = @OutletID
END 
ELSE BEGIN
	INSERT INTO @ItemPrice
	SELECT   I.ItemNo
			,I.RetailPrice RetailPrice
			,I.Userfloat6 P1
			,I.Userfloat7 P2
			,I.Userfloat8 P3
			,I.Userfloat9 P4
			,I.Userfloat10 P5
	FROM	Item I 
	WHERE	I.ItemNo = @ItemNo	
END

SELECT   IPR.ItemNo
		,ISNULL(IFP.RetailPrice, IPR.RetailPrice) RetailPrice
		,ISNULL(IFP.P1, ISNULL(IPR.P1,-9999999)) P1
		,ISNULL(IFP.P2, ISNULL(IPR.P2,-9999999)) P2
		,ISNULL(IFP.P3, ISNULL(IPR.P3,-9999999)) P3
		,ISNULL(IFP.P4, ISNULL(IPR.P4,-9999999)) P4
		,ISNULL(IFP.P5, ISNULL(IPR.P5,-9999999)) P5
		,ISNULL(IFP.ApplicableDate, DATEADD(DAY,1,GETDATE())) ApplicableDate
        ,CAST(CASE WHEN IFP.ItemFuturePriceID IS NULL THEN 0 ELSE 1 END AS BIT) IsUsingFuturePrice
FROM	@ItemPrice IPR
		LEFT JOIN ItemFuturePrice IFP ON IFP.ItemNo = IPR.ItemNo
		    AND ISNULL(IFP.Deleted,0) = 0
		    AND IFP.Status = 'PENDING'	
		    AND IFP.ApplicableTo = @ApplicableTo
		    AND (IFP.OutletID = @OutletID OR @ApplicableTo = 'Product Master')
            AND CAST(IFP.ApplicableDate AS DATE) > CAST(GETDATE() AS DATE)
";

            QueryCommand cmdFuture = new QueryCommand(sqlFuturePrice);
            cmdFuture.AddParameter("@ItemNo", item.ItemNo);
            cmdFuture.AddParameter("@OutletID", ddlOutletList.SelectedValue + "");
            cmdFuture.AddParameter("@ApplicableTo", ddlApplicableTo.SelectedValue + "");

            DataTable dtFuture = new DataTable();
            dtFuture.Load(DataService.GetReader(cmdFuture));
            if (dtFuture.Rows.Count > 0)
            {
                chkApplyFuturPrice.Checked = ((bool)dtFuture.Rows[0]["IsUsingFuturePrice"]);
                txtFuturePriceDate.Text = ((DateTime)dtFuture.Rows[0]["ApplicableDate"]).ToString("dd MMM yyyy");
                txtFutureRetailPrice.Text = ((decimal)dtFuture.Rows[0]["RetailPrice"]).ToString("N2");
                var futureP1 = ((decimal)dtFuture.Rows[0]["P1"]);
                var futureP2 = ((decimal)dtFuture.Rows[0]["P2"]);
                var futureP3 = ((decimal)dtFuture.Rows[0]["P3"]);
                var futureP4 = ((decimal)dtFuture.Rows[0]["P4"]);
                var futureP5 = ((decimal)dtFuture.Rows[0]["P5"]);
                txtFutureP1.Text = futureP1 == -9999999 ? "" : futureP1.ToString("N2");
                txtFutureP2.Text = futureP2 == -9999999 ? "" : futureP2.ToString("N2");
                txtFutureP3.Text = futureP3 == -9999999 ? "" : futureP3.ToString("N2");
                txtFutureP4.Text = futureP4 == -9999999 ? "" : futureP4.ToString("N2");
                txtFutureP5.Text = futureP5 == -9999999 ? "" : futureP5.ToString("N2");
            }


            #endregion

            //set the delete confirmation
            btnDelete.Attributes.Add("onclick", "return CheckDelete();");
        }

    }

    private void LoadAttributes()
    {
        ItemAttributeController itemLogic = new ItemAttributeController();

        //Query qr3 = new Query("ItemAttributes");
        string query = "SELECT * FROM ItemAttributes where Type = 'Attributes3' and ISNULL(Deleted,0) = 0 order by Value";
        DataSet ds = DataService.GetDataSet(new QueryCommand(query));
        ItemAttributeCollection itemCollect = new ItemAttributeCollection();
        itemCollect.Load(ds.Tables[0]);

        int countatt3 = itemCollect.Count;

        for (int i = 0; i < countatt3; ++i)
        {
            if (itemCollect[i].Deleted != true)
            {
                ListItem li = new ListItem();
                li.Value = itemCollect[i].ValueX.Trim();
                MatrixAttributes3.Items.Add(li);
            }
        }

        //Query qr4 = new Query("ItemAttributes");
        //qr4.WHERE("Type", Comparison.Equals, "Attributes4");
        //qr4.OrderBy = OrderBy.Asc("Value");

        string query4 = "SELECT * FROM ItemAttributes where Type = 'Attributes4' and ISNULL(Deleted,0) = 0 order by Value";
        DataSet ds4 = DataService.GetDataSet(new QueryCommand(query4));
        ItemAttributeCollection itemCollect4 = new ItemAttributeCollection();
        itemCollect4.Load(ds4.Tables[0]);
        int countatt4 = itemCollect4.Count;

        for (int i = 0; i < countatt4; ++i)
        {
            if (itemCollect4[i].Deleted != true)
            {
                ListItem li = new ListItem();
                li.Value = itemCollect4[i].ValueX.Trim();
                MatrixAttributes4.Items.Add(li);
            }
        }
    }

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

    public void LoadByItemNo(string itemno)
    {
        if (itemno != "")
            Response.Redirect("NewProductMaster.aspx?id=" + itemno);
    }

    protected void BindGrid(int pageNo)
    {
        ArrayList lstAttributes1 = new ArrayList();
        foreach (ListItem li in ListBox1.Items)
        {
            if (li.Selected)
                lstAttributes1.Add(li.Text);
        }
        ArrayList lstAttributes2 = new ArrayList();
        foreach (ListItem li in ListBox2.Items)
        {
            if (li.Selected)
                lstAttributes2.Add(li.Text);
        }
        ArrayList lstAttributes3 = new ArrayList();
        foreach (ListItem li in ListBox3.Items)
        {
            if (li.Selected)
                lstAttributes3.Add(li.Text);
        }
        ArrayList lstAttributes4 = new ArrayList();
        foreach (ListItem li in ListBox4.Items)
        {
            if (li.Selected)
                lstAttributes4.Add(li.Text);
        }
        ArrayList lstAttributes5 = new ArrayList();
        foreach (ListItem li in ListBox5.Items)
        {
            if (li.Selected)
                lstAttributes5.Add(li.Text);
        }
        ArrayList lstAttributes6 = new ArrayList();
        foreach (ListItem li in ListBox6.Items)
        {
            if (li.Selected)
                lstAttributes6.Add(li.Text);
        }
        ArrayList lstAttributes7 = new ArrayList();
        foreach (ListItem li in ListBox7.Items)
        {
            if (li.Selected)
                lstAttributes7.Add(li.Text);
        }
        ArrayList lstAttributes8 = new ArrayList();
        foreach (ListItem li in ListBox8.Items)
        {
            if (li.Selected)
                lstAttributes8.Add(li.Text);
        }
        ArrayList lstAttributes9 = new ArrayList();
        foreach (ListItem li in ListBox9.Items)
        {
            if (li.Selected)
                lstAttributes9.Add(li.Text);
        }
        ArrayList lstItemNames = new ArrayList();
        foreach (ListItem li in ListBoxItemName.Items)
        {
            if (li.Selected)
                lstItemNames.Add(li.Text);
        }

        ItemController it = new ItemController();
        string[] category = MultiCheckCombo1.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

        int totalPageSize = 0;
        if (pageNo == 0)
            pageNo = 1;

        bool HideDeletedItem = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.HideDeletedItem), false);
        string searchTerm = txtItemNo.Text;
        if (ddlSearchTerm.SelectedValue == "ExactMatch")
            searchTerm = txtItemNo.Text;
        else if (ddlSearchTerm.SelectedValue == "EndsWith")
            searchTerm = "%" + txtItemNo.Text;
        else if (ddlSearchTerm.SelectedValue == "StartsWith")
            searchTerm = txtItemNo.Text + "%";
        else
            searchTerm = "%" + txtItemNo.Text + "%";

        Logger.writeLog(">> Search Term of Product Setup : " + searchTerm);
        DataTable dt = it.SearchItemWithPaging(searchTerm, false, category,
            ddlApplicableTo.SelectedValue, ddlOutletList.SelectedValue, lstItemNames,
            lstAttributes1, lstAttributes2, lstAttributes3, lstAttributes4, lstAttributes5,
            lstAttributes6, lstAttributes7, lstAttributes8, lstAttributes9, null,
            pageNo, out totalPageSize, HideDeletedItem);

        GridView1.DataSource = dt;
        GridView1.DataBind();

        SetPageNo(totalPageSize, pageNo);

        // Hide Unused Attributes Column
        if (ProductAttributeInfo.Attributes1 == null)
            GridView1.Columns[ATTRIBUTES1_COL].Visible = false;
        if (ProductAttributeInfo.Attributes2 == null)
            GridView1.Columns[ATTRIBUTES2_COL].Visible = false;
        if (ProductAttributeInfo.Attributes3 == null)
            GridView1.Columns[ATTRIBUTES3_COL].Visible = false;
        if (ProductAttributeInfo.Attributes4 == null)
            GridView1.Columns[ATTRIBUTES4_COL].Visible = false;
        if (ProductAttributeInfo.Attributes5 == null)
            GridView1.Columns[ATTRIBUTES5_COL].Visible = false;
        if (ProductAttributeInfo.Attributes6 == null)
            GridView1.Columns[ATTRIBUTES6_COL].Visible = false;
        if (ProductAttributeInfo.Attributes7 == null)
            GridView1.Columns[ATTRIBUTES7_COL].Visible = false;
        if (ProductAttributeInfo.Attributes8 == null)
            GridView1.Columns[ATTRIBUTES8_COL].Visible = false;
        //MultiCheckCombo1.Text = "";
    }

    protected void GenerateBarcode()
    {
        //generate barcode when attributes3/attributes4 change
        DataTable dtb = (DataTable)ViewState["ArrayBarcode"];

        if (dtb == null)
        {
            dtb = new DataTable();
            dtb.Columns.Add("ItemNo", typeof(string));
            dtb.Columns.Add("ItemName", typeof(string));
            dtb.Columns.Add("Barcode", typeof(string));
            dtb.Columns.Add("Attributes2", typeof(string));
            dtb.Columns.Add("Attributes3", typeof(string));
            dtb.Columns.Add("Attributes4", typeof(string));
        }

        string prefix = txtPrefix.Text ?? "";
        string attributes2 = txtItemName.Text ?? "";
        string itemno = txtItemNoEditor.Text ?? "";

        foreach (ListItem itema in MatrixAttributes3.Items)
        {
            if (itema.Selected)
            {
                foreach (ListItem itemb in MatrixAttributes4.Items)
                {
                    if (itemb.Selected)
                    {
                        var dr = GetDataRowFromViewState(dtb, itema.Value, itemb.Value);
                        if (dr == null)
                        {
                            var lastitemno = dtb.Rows[dtb.Rows.Count - 1]["ItemNo"].ToString();
                            int lastnumber = 0;
                            if (lastitemno != "")
                            {
                                lastnumber = Convert.ToInt32(lastitemno.Substring(lastitemno.Length - 2, 2) ?? "0");
                            }
                            lastnumber++;
                            string itemname = string.Format("{0}-{1}-{2}", attributes2, itema.Value, itemb.Value);
                            dtb.Rows.Add(itemno + lastnumber.ToString("0#"), itemname, "", attributes2, itema.Value, itemb.Value);
                        }
                    }
                    else
                    {
                        for (int i = dtb.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dtb.Rows[i];
                            if (dr["Attributes4"].ToString() == itemb.Value.ToString())
                                dr.Delete();
                        }
                    }
                }
            }
            else
            {
                for (int i = dtb.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtb.Rows[i];
                    if (dr["Attributes3"].ToString() == itema.Value.ToString())
                        dr.Delete();
                }
            }
        }

        ViewState["ArrayBarcode"] = dtb;
        BindGvBarcode();
    }

    private void SetPageNo(int totalPageNo, int pageNo)
    {
        ddlPages.Items.Clear();
        for (int i = 1; i <= totalPageNo; i++)
        {
            ListItem li = new ListItem(i.ToString());
            li.Selected = i == pageNo;
            ddlPages.Items.Add(li);
        }
        string pageCount = "<b>" + totalPageNo + "</b>";
        lblPageCount.Text = pageCount;

        btnLast.Visible = true;
        btnNext.Visible = true;
        btnPrev.Visible = true;
        btnFirst.Visible = true;
        //now figure out what page we're on
        if (pageNo == 1)
        {
            btnPrev.Visible = false;
            btnFirst.Visible = false;
        }
        else if (pageNo == totalPageNo)
        {
            btnLast.Visible = false;
            btnNext.Visible = false;
        }
        else
        {
            btnLast.Visible = true;
            btnNext.Visible = true;
            btnPrev.Visible = true;
            btnFirst.Visible = true;
        }
    }

    protected void DeleteItem(DataGridCommandEventArgs e)
    {
        dtPriceScheme.Rows[e.Item.ItemIndex].Delete();
        dtPriceScheme.AcceptChanges();
    }

    protected string GetBarcodeFromViewState(string attributes3, string attributes4)
    {
        string objreturn = "";

        DataTable dt = (DataTable)ViewState["ArrayBarcode"];
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if ((dr["Attributes3"].ToString() == attributes3) && dr["Attributes4"].ToString() == attributes4)
                {
                    objreturn = dr["Barcode"].ToString();
                }
            }
        }
        return objreturn;
    }

    protected DataRow GetDataRowFromViewState(DataTable dt, string attributes3, string attributes4)
    {
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if ((dr["Attributes3"].ToString() == attributes3) && dr["Attributes4"].ToString() == attributes4)
                {
                    return dr;
                }
            }
        }
        return null;
    }

    protected bool CheckDuplicateBarcodeViewState(string barcode, string itemno)
    {
        bool objreturn = false;

        DataTable dt = (DataTable)ViewState["ArrayBarcode"];

        foreach (DataRow dr in dt.Rows)
        {
            if ((dr["Barcode"].ToString() == barcode) && (dr["ItemNo"].ToString() != itemno))
            {
                objreturn = true;
            }
        }

        return objreturn;
    }

    protected void BindGvBarcode()
    {
        dtBarcode = (DataTable)ViewState["ArrayBarcode"];
        if (dtBarcode != null)
        {
            DataView dv = dtBarcode.DefaultView;
            dv.Sort = "ItemNo asc";
            DataTable sortedDT = dv.ToTable();

            gvBarcode.DataSource = sortedDT;
            gvBarcode.DataBind();

        }
        else
        {
            gvBarcode.DataSource = null;
            gvBarcode.DataBind();
        }

        Page.ClientScript.RegisterStartupScript(this.GetType(), "RestoreScrollPosition", "setScrollPos();", true);
    }

    protected bool BindAndSave(out string status, out string resultItemNo, out string resultMatrixMode)
    {
        bool isSuccess = false;
        status = "";
        resultItemNo = "";
        resultMatrixMode = "";
        try
        {
            QueryCommandCollection cmdCol = new QueryCommandCollection();
            QueryCommand mycmd;

            decimal retailPrice, factoryPrice;

            //This is to make sure if ApplicableTo is right. sometimes ApplicableTo is string empty
            if (string.IsNullOrEmpty(ApplicableTo))
                ApplicableTo = ddlApplicableTo.SelectedValue;

            System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
            to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (System.Transactions.TransactionScope transScope =
            new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
            {
                ItemController itemLogic = new ItemController();
                Item item;

                #region *) Future Price

                string sqlFuturePrice = @"
                SELECT  TOP 1 *
                FROM	ItemFuturePrice
                WHERE	ISNULL(Deleted,0) = 0
		                AND Status = 'PENDING'
		                AND ItemNo = @ItemNo
		                AND ApplicableTo = @ApplicableTo
		                AND OutletID = @OutletID";
                var cmdFuturePrice = new QueryCommand(sqlFuturePrice);
                cmdFuturePrice.AddParameter("@ItemNo", txtItemNoEditor.Text, DbType.String);
                cmdFuturePrice.AddParameter("@ApplicableTo", ddlApplicableTo.SelectedValue + "", DbType.String);
                cmdFuturePrice.AddParameter("@OutletID", ddlOutletList.SelectedValue + "", DbType.String);
                var futurePriceColl = new ItemFuturePriceCollection();
                futurePriceColl.LoadAndCloseReader(DataService.GetReader(cmdFuturePrice));
                var futurePrice = futurePriceColl.FirstOrDefault();

                if (chkApplyFuturPrice.Checked)
                {
                    if (futurePrice == null)
                    {
                        futurePrice = new ItemFuturePrice();
                        futurePrice.ItemFuturePriceID = Guid.NewGuid();
                        futurePrice.Status = "PENDING";
                        futurePrice.Deleted = false;
                    }
                    futurePrice.ItemNo = txtItemNoEditor.Text;
                    futurePrice.ApplicableDate = txtFuturePriceDate.Text.GetDateTimeValue("dd MMM yyyy");
                    futurePrice.ApplicableTo = ddlApplicableTo.SelectedValue;
                    futurePrice.OutletID = ddlOutletList.SelectedValue + "";
                    futurePrice.RetailPrice = txtFutureRetailPrice.Text.GetDecimalValue();
                    futurePrice.P1 = txtFutureP1.Text.GetDecimalNullValue();
                    futurePrice.P2 = txtFutureP2.Text.GetDecimalNullValue();
                    futurePrice.P3 = txtFutureP3.Text.GetDecimalNullValue();
                    futurePrice.P4 = txtFutureP4.Text.GetDecimalNullValue();
                    futurePrice.P5 = txtFutureP5.Text.GetDecimalNullValue();
                    futurePrice.CostPrice = decimal.Parse(txtFactoryPrice.Text.Replace(",", ""));
                    futurePrice.Deleted = false;
                }
                else
                {
                    if (futurePrice != null)
                    {
                        futurePrice.Status = "CANCELLED";
                        futurePrice.Deleted = true;
                    }
                }

                #endregion

                #region *) Matrix Mode
                //matrix mode
                if (UserFlag1.Value.ToLower() == "true")
                {

                    if (string.IsNullOrEmpty(txtRetailPrice.Text) || !decimal.TryParse(txtRetailPrice.Text.Replace(",", ""), out retailPrice))
                        throw new Exception("Invalid Retail Price");

                    if (string.IsNullOrEmpty(txtFactoryPrice.Text) || !decimal.TryParse(txtFactoryPrice.Text.Replace(",", ""), out factoryPrice))
                        throw new Exception("Invalid Cost Price");

                    if (rbNonInventoryProduct.Checked && (string.IsNullOrEmpty(DeductedItemNo.Value) || string.IsNullOrEmpty(DeductConvRate.Text)))
                        throw new Exception("Must set deducted Item and Conversion Rate");

                    int runningnumber = 0;
                    string itemnofinish = "";
                    foreach (ListItem itema in MatrixAttributes3.Items)
                    {

                        if (itema.Selected)
                        {
                            foreach (ListItem itemb in MatrixAttributes4.Items)
                            {
                                if (itemb.Selected)
                                {
                                    string barcode = GetBarcodeFromViewState(itema.Value.Trim(), itemb.Value.Trim());
                                    if (barcode == null)
                                        barcode = "";

                                    var existingitemno = "";

                                    Query qrs = new Query("Item");
                                    qrs.QueryType = QueryType.Select;
                                    qrs.SelectList = Item.Columns.ItemNo;
                                    qrs.AddWhere(Item.Columns.Attributes1, Comparison.Equals, txtItemNoEditor.Text);
                                    qrs.AddWhere(Item.Columns.Attributes3, Comparison.Equals, itema.Value.Trim());
                                    qrs.AddWhere(Item.Columns.Attributes4, Comparison.Equals, itemb.Value.Trim());
                                    qrs.AddWhere(Item.Columns.Deleted, Comparison.Equals, false);
                                    DataSet ds = qrs.ExecuteDataSet();
                                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        //
                                        existingitemno = ds.Tables[0].Rows[0]["ItemNo"].ToString();
                                        string updbarcode = string.IsNullOrEmpty(barcode) ? existingitemno : barcode;
                                        if (itemLogic.CheckIfBarcodeExists(updbarcode, existingitemno))
                                            throw new Exception("Barcode is duplicated");

                                        item = new Item(existingitemno);
                                        item.Barcode = updbarcode;

                                        itemnofinish = existingitemno;
                                    }
                                    else
                                    {
                                        runningnumber = ItemController.getNewRunningNumberMatrix(txtItemNoEditor.Text);
                                        var itemno = txtItemNoEditor.Text + runningnumber.ToString("00#");

                                        if (itemLogic.CheckIfBarcodeExists(barcode, itemno))
                                            throw new Exception("Barcode is duplicated");

                                        item = new Item();
                                        item.ItemNo = itemno;
                                        item.IsNew = true;
                                        item.UniqueID = Guid.NewGuid();
                                        item.Barcode = itemno;

                                        itemnofinish = itemno;
                                    }
                                    item.Userfld1 = txtUOM.Text;
                                    item.Userflag1 = true;
                                    string itemName = txtItemName.Text;
                                    item.CategoryName = ddlCategoryName.SelectedValue;

                                    decimal minPrice = 0;
                                    if (decimal.TryParse(txtMinimumPrice.Text.Replace(",", ""), out minPrice))
                                        item.MinimumPrice = minPrice;
                                    retailPrice = decimal.Parse(txtRetailPrice.Text.Replace(",", ""));
                                    if (ddlApplicableTo.SelectedValue == "Product Master")
                                    {
                                        item.ItemName = string.Format("{0}-{1}-{2}", itemName, itema.Value, itemb.Value);
                                        item.RetailPrice = retailPrice;
                                        item.Deleted = chkDeleted.Checked;
                                    }
                                    else if (ddlApplicableTo.SelectedValue == "Outlet Group")
                                    {
                                        if (item.IsNew)
                                        {
                                            item.ItemName = string.Format("{0}-{1}-{2}", itemName, itema.Value, itemb.Value);
                                            item.RetailPrice = retailPrice;
                                            item.FactoryPrice = factoryPrice;
                                        }

                                        Query qr = new Query("OutletGroupItemMap");
                                        qr.AddWhere(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                        qr.AddWhere(OutletGroupItemMap.Columns.OutletGroupID, ddlOutletList.SelectedValue.GetIntValue());

                                        OutletGroupItemMap col = new OutletGroupItemMapController().FetchByQuery(qr).FirstOrDefault();

                                        if (col != null)
                                        {
                                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false))
                                            {
                                                col.ItemName = string.Format("{0}-{1}-{2}", itemName, itema.Value, itemb.Value);
                                            }

                                            col.RetailPrice = retailPrice;
                                            col.Deleted = chkDeleted.Checked;
                                            //col.IsItemDeleted = chkDeleted.Checked;
                                            col.Save(Session["username"] + "");
                                        }
                                        else
                                        {
                                            col = new OutletGroupItemMap();
                                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false))
                                            {
                                                col.ItemName = string.Format("{0}-{1}-{2}", itemName, itema.Value, itemb.Value);
                                            }
                                            col.OutletGroupID = ddlOutletList.SelectedValue.GetIntValue();
                                            col.ItemNo = item.ItemNo;
                                            col.RetailPrice = retailPrice;
                                            col.Deleted = chkDeleted.Checked;
                                            //col.IsItemDeleted = chkDeleted.Checked;
                                            col.Save(Session["username"] + "");
                                        }
                                    }
                                    else if (ddlApplicableTo.SelectedValue == "Outlet")
                                    {
                                        if (item.IsNew)
                                        {
                                            item.ItemName = string.Format("{0}-{1}-{2}", itemName, itema.Value, itemb.Value);
                                            item.RetailPrice = retailPrice;
                                            item.FactoryPrice = factoryPrice;
                                        }

                                        Query qr = new Query("OutletGroupItemMap");
                                        qr.AddWhere(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                        qr.AddWhere(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);

                                        OutletGroupItemMap col = new OutletGroupItemMapController().FetchByQuery(qr).FirstOrDefault();

                                        if (col != null)
                                        {
                                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false))
                                            {
                                                col.ItemName = string.Format("{0}-{1}-{2}", itemName, itema.Value, itemb.Value);
                                            }

                                            col.RetailPrice = retailPrice;
                                            col.Deleted = chkDeleted.Checked;
                                            //col.IsItemDeleted = chkDeleted.Checked;
                                            col.Save(Session["username"] + "");
                                        }
                                        else
                                        {
                                            col = new OutletGroupItemMap();
                                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false))
                                            {
                                                col.ItemName = string.Format("{0}-{1}-{2}", itemName, itema.Value, itemb.Value);
                                            }
                                            col.OutletName = ddlOutletList.SelectedValue;
                                            col.ItemNo = item.ItemNo;
                                            col.RetailPrice = retailPrice;
                                            col.Deleted = chkDeleted.Checked;
                                            //col.IsItemDeleted = chkDeleted.Checked;
                                            col.Save(Session["username"] + "");
                                        }

                                        //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddProductOneOutletOnly), false))
                                        //{
                                        //    if (((bool)ViewState["IsNew"]))
                                        //    {
                                        //        item.RetailPrice = retailPrice;
                                        //        string selectedoutlet = ddlOutletList.SelectedValue;

                                        //        for (int o = 0; o < ddlOutletList.Items.Count; o++)
                                        //        {
                                        //            if (ddlOutletList.Items[o].Value != selectedoutlet && !string.IsNullOrEmpty(ddlOutletList.Items[o].Value))
                                        //            {
                                        //                Query qr2 = new Query("OutletGroupItemMap");
                                        //                qr2.AddWhere(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                        //                qr2.AddWhere(OutletGroupItemMap.Columns.OutletName, ddlOutletList.Items[o].Value);

                                        //                OutletGroupItemMap colect = new OutletGroupItemMapController().FetchByQuery(qr2).FirstOrDefault();

                                        //                if (colect == null)
                                        //                {
                                        //                    colect = new OutletGroupItemMap();
                                        //                    colect.OutletName = ddlOutletList.Items[o].Value;
                                        //                    colect.ItemNo = item.ItemNo;
                                        //                    colect.RetailPrice = retailPrice;
                                        //                    colect.Deleted = false;
                                        //                    colect.IsItemDeleted = true;
                                        //                    colect.Save(Session["username"] + "");
                                        //                }
                                        //                else
                                        //                {
                                        //                    colect.RetailPrice = retailPrice;
                                        //                    colect.Deleted = false;
                                        //                    colect.IsItemDeleted = true;
                                        //                    colect.Save(Session["username"] + "");
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                    }

                                    item.FactoryPrice = decimal.Parse(txtFactoryPrice.Text.Replace(",", ""));
                                    item.IsNonDiscountable = cbIsNonDiscountable.Checked;
                                    item.IsCommission = cbGiveCommission.Checked;
                                    item.AutoCaptureWeight = cbAutoCaptureWeight.Checked;

                                    item.NonInventoryProduct = false;
                                    if (rbService.Checked)
                                    {
                                        item.IsInInventory = false;
                                        item.IsServiceItem = true;
                                        item.PointGetAmount = 0;
                                        item.PointGetMode = Item.PointMode.None;
                                        item.PointRedeemAmount = 0;
                                        item.PointRedeemMode = cbPointRedeemable.Checked ? Item.PointMode.Dollar : Item.PointMode.None;
                                        item.Userfloat3 = null; /// Course Breakdown Price
                                    }
                                    else if (rbPoint.Checked)
                                    {
                                        item.IsInInventory = false;
                                        item.IsServiceItem = false;
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
                                        decimal tempDec = 0; decimal.TryParse(txtTimesGet.Text, out tempDec);
                                        item.PointGetAmount = tempDec;
                                        item.PointGetMode = Item.PointMode.Times;
                                        item.PointRedeemAmount = 0;
                                        item.PointRedeemMode = Item.PointMode.None;
                                        decimal.TryParse(txtBreakdownPrice.Text, out tempDec);
                                        item.Userfloat3 = tempDec; // Course Breakdown Price
                                        item.IsOpenPricePackage = cbOpenPackace.Checked;
                                    }
                                    else if (rbOpenPriceProduct.Checked)
                                    {
                                        item.IsInInventory = true;
                                        item.IsServiceItem = true;
                                        item.PointGetAmount = 0;
                                        item.PointGetMode = Item.PointMode.None;
                                        item.PointRedeemAmount = 0;
                                        item.PointRedeemMode = cbPointRedeemable.Checked ? Item.PointMode.Dollar : Item.PointMode.None;
                                        item.Userfloat3 = null; /// Course Breakdown Price
                                    }
                                    else if (rbNonInventoryProduct.Checked)
                                    {
                                        item.IsInInventory = false;
                                        item.IsServiceItem = false;
                                        item.NonInventoryProduct = true;
                                        item.PointGetAmount = 0;
                                        item.PointGetMode = Item.PointMode.None;
                                        item.PointRedeemAmount = 0;
                                        item.PointRedeemMode = cbPointRedeemable.Checked ? Item.PointMode.Dollar : Item.PointMode.None;
                                        item.Userfloat3 = null; /// Course Breakdown Price
                                        item.DeductedItem = DeductedItemNo.Value;
                                        decimal tempDec = 0; decimal.TryParse(DeductConvRate.Text, out tempDec);
                                        item.DeductConvRate = tempDec;
                                        item.DeductConvType = ddlDeductConvType.SelectedValue == "true";
                                    }
                                    else /// Categorized as Product
                                    {
                                        item.IsInInventory = true;
                                        item.IsServiceItem = false;
                                        item.PointGetAmount = 0;
                                        item.PointGetMode = Item.PointMode.None;
                                        item.PointRedeemAmount = 0;
                                        item.PointRedeemMode = cbPointRedeemable.Checked ? Item.PointMode.Dollar : Item.PointMode.None;
                                        item.Userfloat3 = null; /// Course Breakdown Price
                                    }

                                    #region save discount
                                    if (P1Name != null && P1Name != "")
                                    {
                                        if (ApplicableTo == "Product Master")
                                        {
                                            if (txtP1.Text == "")
                                            {
                                                item.Userfloat6 = null;
                                            }
                                            else
                                            {
                                                item.Userfloat6 = decimal.Parse(txtP1.Text); // Promotion Prce
                                                if (item.Userfloat6 < 0)
                                                {
                                                    item.Userfloat6 = null;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                decimal? p1 = null;
                                                if (txtP1.Text != "")
                                                {
                                                    p1 = decimal.Parse(txtP1.Text);

                                                    if (p1 < 0)
                                                        p1 = null;
                                                }

                                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                                if (ApplicableTo == "Outlet Group")
                                                {
                                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, (ddlOutletList.SelectedValue + "").GetIntValue());
                                                    ogl.Load();

                                                }
                                                else
                                                {
                                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);
                                                    ogl.Load();
                                                }

                                                if (ogl.Count > 0)
                                                {
                                                    OutletGroupItemMap sd = ogl[0];
                                                    sd.P1 = p1;
                                                    sd.Save();
                                                }


                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.writeLog(ex.Message);
                                                item.Userfloat6 = null;
                                            }
                                        }
                                    }

                                    if (P2Name != null && P2Name != "")
                                    {
                                        if (ApplicableTo == "Product Master")
                                        {
                                            if (txtP2.Text == "")
                                            {
                                                item.Userfloat7 = null;
                                            }
                                            else
                                            {
                                                item.Userfloat7 = decimal.Parse(txtP2.Text); // Promotion Prce
                                                if (item.Userfloat7 < 0)
                                                {
                                                    item.Userfloat7 = null;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                decimal? p2 = null;
                                                if (txtP2.Text != "")
                                                {
                                                    p2 = decimal.Parse(txtP2.Text);

                                                    if (p2 < 0)
                                                        p2 = null;
                                                }

                                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                                if (ApplicableTo == "Outlet Group")
                                                {
                                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, (ddlOutletList.SelectedValue + "").GetIntValue());
                                                    ogl.Load();

                                                }
                                                else
                                                {
                                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);
                                                    ogl.Load();
                                                }

                                                if (ogl.Count > 0)
                                                {
                                                    OutletGroupItemMap sd = ogl[0];
                                                    sd.P2 = p2;
                                                    sd.Save();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.writeLog(ex.Message);
                                                item.Userfloat7 = null;
                                            }
                                        }
                                    }

                                    if (P3Name != null && P3Name != "")
                                    {
                                        if (ApplicableTo == "Product Master")
                                        {
                                            if (txtP3.Text == "")
                                            {
                                                item.Userfloat8 = null;
                                            }
                                            else
                                            {
                                                item.Userfloat8 = decimal.Parse(txtP3.Text); // Promotion Prce
                                                if (item.Userfloat8 < 0)
                                                {
                                                    item.Userfloat8 = null;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                decimal? p3 = null;
                                                if (txtP3.Text != "")
                                                {
                                                    p3 = decimal.Parse(txtP3.Text);

                                                    if (p3 < 0)
                                                        p3 = null;
                                                }

                                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                                if (ApplicableTo == "Outlet Group")
                                                {
                                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, (ddlOutletList.SelectedValue + "").GetIntValue());
                                                    ogl.Load();

                                                }
                                                else
                                                {
                                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);
                                                    ogl.Load();
                                                }

                                                if (ogl.Count > 0)
                                                {
                                                    OutletGroupItemMap sd = ogl[0];
                                                    sd.P3 = p3;
                                                    sd.Save();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.writeLog(ex.Message);
                                                item.Userfloat8 = null;
                                            }
                                        }
                                    }

                                    if (P4Name != null && P4Name != "")
                                    {
                                        if (ApplicableTo == "Product Master")
                                        {
                                            if (txtP4.Text == "")
                                            {
                                                item.Userfloat9 = null;
                                            }
                                            else
                                            {
                                                item.Userfloat9 = decimal.Parse(txtP4.Text); // Promotion Prce
                                                if (item.Userfloat9 < 0)
                                                {
                                                    item.Userfloat9 = null;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                decimal? p4 = null;
                                                if (txtP4.Text != "")
                                                {
                                                    p4 = decimal.Parse(txtP4.Text);

                                                    if (p4 < 0)
                                                        p4 = null;
                                                }

                                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                                if (ApplicableTo == "Outlet Group")
                                                {
                                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, (ddlOutletList.SelectedValue + "").GetIntValue());
                                                    ogl.Load();

                                                }
                                                else
                                                {
                                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);
                                                    ogl.Load();
                                                }

                                                if (ogl.Count > 0)
                                                {
                                                    OutletGroupItemMap sd = ogl[0];
                                                    sd.P4 = p4;
                                                    sd.Save();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.writeLog(ex.Message);
                                                item.Userfloat9 = null;
                                            }
                                        }
                                    }

                                    if (P5Name != null && P5Name != "")
                                    {
                                        if (ApplicableTo == "Product Master")
                                        {
                                            if (txtP5.Text == "")
                                            {
                                                item.Userfloat10 = null;
                                            }
                                            else
                                            {
                                                item.Userfloat10 = decimal.Parse(txtP5.Text); // Promotion Prce
                                                if (item.Userfloat10 < 0)
                                                {
                                                    item.Userfloat10 = null;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                decimal? p5 = null;
                                                if (txtP5.Text != "")
                                                {
                                                    p5 = decimal.Parse(txtP5.Text);

                                                    if (p5 < 0)
                                                        p5 = null;
                                                }

                                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                                if (ApplicableTo == "Outlet Group")
                                                {
                                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, (ddlOutletList.SelectedValue + "").GetIntValue());
                                                    ogl.Load();

                                                }
                                                else
                                                {
                                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);
                                                    ogl.Load();
                                                }

                                                if (ogl.Count > 0)
                                                {
                                                    OutletGroupItemMap sd = ogl[0];
                                                    sd.P5 = p5;
                                                    sd.Save();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.writeLog(ex.Message);
                                                item.Userfloat10 = null;
                                            }
                                        }
                                    }
                                    #endregion

                                    item.GSTRule = ddGST.SelectedIndex;
                                    item.ItemDesc = txtItemDesc.Text.Trim();
                                    item.Attributes1 = txtItemNoEditor.Text.Trim();
                                    item.Attributes2 = txtItemName.Text;
                                    item.Attributes3 = itema.Value.Trim();
                                    item.Attributes4 = itemb.Value.Trim();
                                    item.Attributes5 = txtAttributes5.Text;
                                    item.Attributes6 = txtAttributes6.Text;
                                    item.Attributes7 = txtAttributes7.Text;
                                    item.Attributes8 = txtAttributes8.Text;
                                    //if (Image2.ImageUrl != "")
                                    //{
                                    if (fuItemPicture.HasFile)
                                        item.ItemImage = ImageCompressor.ResizeAndCompressImage(new MemoryStream(fuItemPicture.FileBytes));
                                    //}

                                    item.Remark = txtRemark.Text;
                                    item.ExcludeProfitLoss = cbIsExcludeProfitLossReport.Checked ? 1 : 0;

                                    item.AllowPreOrder = cbPreOrder.Checked;
                                    item.CapQty = int.Parse(txtPreOrderCapQty.Text == "" ? "0" : txtPreOrderCapQty.Text);

                                    item.IsVendorDelivery = cbVendorDelivery.Checked;
                                    item.IsPAMedifund = cbPAMed.Checked;
                                    item.IsSMF = cbSMF.Checked;
                                    item.IsConsignment = chkIsConsigment.Checked;
                                    item.IsUsingFixedCOG = IsUsingFixedCOG.Checked;
                                    if (item.IsUsingFixedCOG)
                                    {
                                        decimal FixedCOGValue = 0;

                                        if (IsFixedCOGPercentage.Checked)
                                        {
                                            decimal.TryParse(txtFixedCOGPercentage.Text, out FixedCOGValue);
                                            item.FixedCOGType = ItemController.FIXEDCOG_PERCENTAGE;
                                            item.FixedCOGValue = FixedCOGValue;
                                        }
                                        else
                                        {
                                            decimal.TryParse(txtFixedCOGValue.Text, out FixedCOGValue);
                                            item.FixedCOGType = ItemController.FIXEDCOG_VALUE;
                                            item.FixedCOGValue = FixedCOGValue;
                                        }
                                    }

                                    if (ddlApplicableTo.SelectedValue == "Outlet" && ((bool)ViewState["IsNew"])) // if add item to outlet
                                        item.Deleted = true;
                                    else
                                        if (ddlApplicableTo.SelectedValue == "Outlet") // Edit outlet item
                                            item.Deleted = item.Deleted;
                                        else
                                            item.Deleted = chkDeleted.Checked; // normal item
                                    cmdCol.Add(item.GetSaveCommand(Session["username"].ToString()));

                                    string update = @"update item set deleted = 1, modifiedon = getdate() where Attributes1 = '{0}' and Attributes3 = '{1}' and Attributes4 = '{2}' and ItemNo != '{3}'";
                                    update = string.Format(update, item.Attributes1, item.Attributes3, item.Attributes4, item.ItemNo);

                                    cmdCol.Add(new QueryCommand(update));   

                                    //Save supplier
                                    int newSuppId = 0;
                                    QueryCommand c = null;
                                    if (int.TryParse(ddlSupplier.SelectedValue, out newSuppId))
                                        c = SaveSupplier(item.ItemNo, newSuppId, item.FactoryPrice);
                                    if (c != null)
                                        cmdCol.Add(c);


                                    AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", "UPDATE Item " + txtItemNoEditor.Text, "");

                                    DataService.ExecuteTransaction(cmdCol);
                                    cmdCol = new QueryCommandCollection();
                                }
                                else
                                {
                                    //deleted when unchecked item
                                    Query qr = new Query("Item");
                                    qr.QueryType = QueryType.Update;
                                    qr.AddUpdateSetting("Deleted", true);
                                    qr.AddWhere("Attributes1", Comparison.Equals, txtItemNoEditor.Text);
                                    qr.AddWhere("Attributes4", Comparison.Equals, itemb.Value);
                                    qr.Execute();
                                    AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", "UPDATE Item " + txtItemNoEditor.Text, "");
                                }
                            }
                        }
                        else
                        {
                            //deleted when unchecked item
                            Query qr = new Query("Item");
                            qr.QueryType = QueryType.Update;
                            qr.AddUpdateSetting("Deleted", true);
                            qr.AddWhere("Attributes1", Comparison.Equals, txtItemNoEditor.Text);
                            qr.AddWhere("Attributes3", Comparison.Equals, itema.Value);
                            qr.Execute();
                            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", "UPDATE Item " + txtItemNoEditor.Text, "");
                        }
                    }

                    if (futurePrice != null)
                        futurePrice.Save(Session["UserName"] + "");

                    ViewState["IsNew"] = false;
                    txtItemNoEditor.ReadOnly = true;

                    transScope.Complete();
                    resultItemNo = itemnofinish;
                    resultMatrixMode = "Yes";
                    isSuccess = true;
                }
                #endregion

                #region *) Normal Mode
                else
                {
                    if (string.IsNullOrEmpty(txtRetailPrice.Text) || !decimal.TryParse(txtRetailPrice.Text.Replace(",", ""), out retailPrice))
                        throw new Exception("Invalid Retail Price");

                    if (string.IsNullOrEmpty(txtFactoryPrice.Text) || !decimal.TryParse(txtFactoryPrice.Text.Replace(",", ""), out factoryPrice))
                        throw new Exception("Invalid Cost Price");

                    if (rbNonInventoryProduct.Checked && (string.IsNullOrEmpty(DeductedItemNo.Value) || string.IsNullOrEmpty(DeductConvRate.Text)))
                        throw new Exception("Must set deducted Item and Conversion Rate");

                    if (itemLogic.CheckIfBarcodeExists(txtBarcode.Text, txtItemNoEditor.Text))
                        throw new Exception(LanguageManager.GetTranslation("Barcode is duplicated"));

                    Item originalItem = new Item();

                    if (!((bool)ViewState["IsNew"]))
                    {
                        item = new Item(txtItemNoEditor.Text);
                        item.IsNew = false;
                        item.CopyTo(originalItem);
                        originalItem.IsNew = false;
                    }
                    else
                    {
                        item = new Item();
                        item.ItemNo = txtItemNoEditor.Text;
                        item.IsNew = true;
                        item.UniqueID = Guid.NewGuid();
                    }
                    item.Userflag1 = false;
                    item.Userfld1 = txtUOM.Text;
                    item.Barcode = txtBarcode.Text;

                    item.CategoryName = ddlCategoryName.SelectedValue;
                    decimal minPrice = 0;
                    item.MinimumPrice = 0;
                    if (decimal.TryParse(txtMinimumPrice.Text.Replace(",", ""), out minPrice))
                        item.MinimumPrice = minPrice;

                    retailPrice = decimal.Parse(txtRetailPrice.Text.Replace(",", ""));
                    factoryPrice = decimal.Parse(txtFactoryPrice.Text.Replace(",", ""));

                    if (ddlApplicableTo.SelectedValue == "Product Master")
                    {
                        item.ItemName = txtItemName.Text;
                        item.RetailPrice = retailPrice;
                        item.FactoryPrice = factoryPrice;
                        item.Deleted = chkDeleted.Checked;
                    }
                    else if (ddlApplicableTo.SelectedValue == "Outlet Group")
                    {
                        if (item.IsNew)
                        {
                            item.ItemName = txtItemName.Text;
                            item.RetailPrice = retailPrice;
                            item.FactoryPrice = factoryPrice;
                        }

                        item.RetailPrice = retailPrice;
                        Query qr = new Query("OutletGroupItemMap");
                        qr.AddWhere(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                        qr.AddWhere(OutletGroupItemMap.Columns.OutletGroupID, ddlOutletList.SelectedValue.GetIntValue());

                        OutletGroupItemMap col = new OutletGroupItemMapController().FetchByQuery(qr).FirstOrDefault();

                        if (col != null)
                        {
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false))
                            {
                                col.ItemName = txtItemName.Text;
                            }
                            col.RetailPrice = retailPrice;
                            col.CostPrice = factoryPrice;
                            col.Deleted = chkDeleted.Checked;
                            //col.IsItemDeleted = ;
                            col.Save(Session["username"] + "");
                        }
                        else
                        {
                            col = new OutletGroupItemMap();
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false))
                            {
                                col.ItemName = txtItemName.Text;
                            }
                            col.OutletGroupID = ddlOutletList.SelectedValue.GetIntValue();
                            col.ItemNo = item.ItemNo;
                            col.RetailPrice = retailPrice;
                            col.CostPrice = factoryPrice;
                            col.Deleted = chkDeleted.Checked;
                            //col.IsItemDeleted = ;
                            col.Save(Session["username"] + "");
                        }

                    }
                    else if (ddlApplicableTo.SelectedValue == "Outlet")
                    {
                        if (item.IsNew)
                        {
                            item.ItemName = txtItemName.Text;
                            item.RetailPrice = retailPrice;
                            item.FactoryPrice = factoryPrice;
                        }

                        Query qr = new Query("OutletGroupItemMap");
                        qr.AddWhere(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                        qr.AddWhere(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);

                        OutletGroupItemMap col = new OutletGroupItemMapController().FetchByQuery(qr).FirstOrDefault();

                        if (col != null)
                        {
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false))
                            {
                                col.ItemName = txtItemName.Text;
                            }
                            col.RetailPrice = retailPrice;
                            col.Deleted = chkDeleted.Checked;
                            col.CostPrice = factoryPrice;
                            //col.IsItemDeleted = chkDeleted.Checked;
                            col.Save(Session["username"] + "");
                        }
                        else
                        {
                            col = new OutletGroupItemMap();
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false))
                            {
                                col.ItemName = txtItemName.Text;
                            }
                            col.OutletName = ddlOutletList.SelectedValue;
                            col.ItemNo = item.ItemNo;
                            col.RetailPrice = retailPrice;
                            col.CostPrice = factoryPrice;
                            col.Deleted = chkDeleted.Checked;
                            //col.IsItemDeleted = chkDeleted.Checked;
                            col.Save(Session["username"] + "");
                        }

                        //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddProductOneOutletOnly), false))
                        //{
                        //    if (((bool)ViewState["IsNew"]))
                        //    {
                        //        item.RetailPrice = retailPrice;
                        //        string selectedoutlet = ddlOutletList.SelectedValue;

                        //        for (int o = 0; o < ddlOutletList.Items.Count; o++)
                        //        {
                        //            if (ddlOutletList.Items[o].Value != selectedoutlet && !string.IsNullOrEmpty(ddlOutletList.Items[o].Value))
                        //            {
                        //                Query qr2 = new Query("OutletGroupItemMap");
                        //                qr2.AddWhere(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                        //                qr2.AddWhere(OutletGroupItemMap.Columns.OutletName, ddlOutletList.Items[o].Value);

                        //                OutletGroupItemMap colect = new OutletGroupItemMapController().FetchByQuery(qr2).FirstOrDefault();

                        //                if (colect == null)
                        //                {
                        //                    colect = new OutletGroupItemMap();
                        //                    colect.OutletName = ddlOutletList.Items[o].Value;
                        //                    colect.ItemNo = item.ItemNo;
                        //                    colect.RetailPrice = retailPrice;
                        //                    colect.Deleted = false;
                        //                    colect.IsItemDeleted = true;
                        //                    colect.Save(Session["username"] + "");
                        //                }
                        //                else
                        //                {
                        //                    colect.RetailPrice = retailPrice;
                        //                    colect.Deleted = false;
                        //                    colect.IsItemDeleted = true;
                        //                    colect.Save(Session["username"] + "");
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                    }

                    item.IsNonDiscountable = cbIsNonDiscountable.Checked;

                    item.IsCommission = cbGiveCommission.Checked;

                    item.AutoCaptureWeight = cbAutoCaptureWeight.Checked;

                    item.NonInventoryProduct = false;

                    if (rbService.Checked)
                    {
                        item.IsInInventory = false;
                        item.IsServiceItem = true;
                        item.PointGetAmount = 0;
                        item.PointGetMode = Item.PointMode.None;
                        item.PointRedeemAmount = 0;
                        item.PointRedeemMode = cbPointRedeemable.Checked ? Item.PointMode.Dollar : Item.PointMode.None;
                        item.Userfloat3 = null; /// Course Breakdown Price
                    }
                    else if (rbPoint.Checked)
                    {
                        item.IsInInventory = false;
                        item.IsServiceItem = false;
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
                        decimal tempDec = 0; decimal.TryParse(txtTimesGet.Text, out tempDec);
                        item.PointGetAmount = tempDec;
                        item.PointGetMode = Item.PointMode.Times;
                        item.PointRedeemAmount = 0;
                        item.PointRedeemMode = Item.PointMode.None;
                        decimal.TryParse(txtBreakdownPrice.Text, out tempDec);
                        item.Userfloat3 = tempDec; // Course Breakdown Price
                        item.IsOpenPricePackage = cbOpenPackace.Checked;
                    }
                    else if (rbOpenPriceProduct.Checked)
                    {
                        item.IsInInventory = true;
                        item.IsServiceItem = true;
                        item.PointGetAmount = 0;
                        item.PointGetMode = Item.PointMode.None;
                        item.PointRedeemAmount = 0;
                        item.PointRedeemMode = cbPointRedeemable.Checked ? Item.PointMode.Dollar : Item.PointMode.None;
                        item.Userfloat3 = null; /// Course Breakdown Price
                    }
                    else if (rbNonInventoryProduct.Checked)
                    {
                        item.IsInInventory = false;
                        item.IsServiceItem = false;
                        item.NonInventoryProduct = true;
                        item.PointGetAmount = 0;
                        item.PointGetMode = Item.PointMode.None;
                        item.PointRedeemAmount = 0;
                        item.PointRedeemMode = cbPointRedeemable.Checked ? Item.PointMode.Dollar : Item.PointMode.None;
                        item.Userfloat3 = null; /// Course Breakdown Price
                        item.DeductedItem = DeductedItemNo.Value;
                        decimal tempDec = 0; decimal.TryParse(DeductConvRate.Text, out tempDec);
                        item.DeductConvRate = tempDec;
                        item.DeductConvType = ddlDeductConvType.SelectedValue == "true";
                    }
                    else /// Categorized as Product
                    {
                        item.IsInInventory = true;
                        item.IsServiceItem = false;
                        item.PointGetAmount = 0;
                        item.PointGetMode = Item.PointMode.None;
                        item.PointRedeemAmount = 0;
                        item.PointRedeemMode = cbPointRedeemable.Checked ? Item.PointMode.Dollar : Item.PointMode.None;
                        item.Userfloat3 = null; /// Course Breakdown Price
                    }

                    #region save discount
                    if (P1Name != null && P1Name != "")
                    {
                        if (ApplicableTo == "Product Master")
                        {
                            if (txtP1.Text == "")
                            {
                                item.Userfloat6 = null;
                            }
                            else
                            {
                                item.Userfloat6 = decimal.Parse(txtP1.Text); // Promotion Prce
                                if (item.Userfloat6 < 0)
                                {
                                    item.Userfloat6 = null;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                decimal? p1 = null;
                                if (txtP1.Text != "")
                                {
                                    p1 = decimal.Parse(txtP1.Text);

                                    if (p1 < 0)
                                        p1 = null;
                                }

                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                if (ApplicableTo == "Outlet Group")
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, (ddlOutletList.SelectedValue + "").GetIntValue());
                                    ogl.Load();

                                }
                                else
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);
                                    ogl.Load();
                                }

                                if (ogl.Count > 0)
                                {
                                    OutletGroupItemMap sd = ogl[0];
                                    sd.P1 = p1;
                                    sd.Save();
                                }


                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex.Message);
                                item.Userfloat6 = null;
                            }
                        }
                    }

                    if (P2Name != null && P2Name != "")
                    {
                        if (ApplicableTo == "Product Master")
                        {
                            if (txtP2.Text == "")
                            {
                                item.Userfloat7 = null;
                            }
                            else
                            {
                                item.Userfloat7 = decimal.Parse(txtP2.Text); // Promotion Prce
                                if (item.Userfloat7 < 0)
                                {
                                    item.Userfloat7 = null;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                decimal? p2 = null;
                                if (txtP2.Text != "")
                                {
                                    p2 = decimal.Parse(txtP2.Text);

                                    if (p2 < 0)
                                        p2 = null;
                                }

                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                if (ApplicableTo == "Outlet Group")
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, (ddlOutletList.SelectedValue + "").GetIntValue());
                                    ogl.Load();

                                }
                                else
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);
                                    ogl.Load();
                                }

                                if (ogl.Count > 0)
                                {
                                    OutletGroupItemMap sd = ogl[0];
                                    sd.P2 = p2;
                                    sd.Save();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex.Message);
                                item.Userfloat7 = null;
                            }
                        }
                    }

                    if (P3Name != null && P3Name != "")
                    {
                        if (ApplicableTo == "Product Master")
                        {
                            if (txtP3.Text == "")
                            {
                                item.Userfloat8 = null;
                            }
                            else
                            {
                                item.Userfloat8 = decimal.Parse(txtP3.Text); // Promotion Prce
                                if (item.Userfloat8 < 0)
                                {
                                    item.Userfloat8 = null;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                decimal? p3 = null;
                                if (txtP3.Text != "")
                                {
                                    p3 = decimal.Parse(txtP3.Text);

                                    if (p3 < 0)
                                        p3 = null;
                                }

                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                if (ApplicableTo == "Outlet Group")
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, (ddlOutletList.SelectedValue + "").GetIntValue());
                                    ogl.Load();

                                }
                                else
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);
                                    ogl.Load();
                                }

                                if (ogl.Count > 0)
                                {
                                    OutletGroupItemMap sd = ogl[0];
                                    sd.P3 = p3;
                                    sd.Save();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex.Message);
                                item.Userfloat8 = null;
                            }
                        }
                    }

                    if (P4Name != null && P4Name != "")
                    {
                        if (ApplicableTo == "Product Master")
                        {
                            if (txtP4.Text == "")
                            {
                                item.Userfloat9 = null;
                            }
                            else
                            {
                                item.Userfloat9 = decimal.Parse(txtP4.Text); // Promotion Prce
                                if (item.Userfloat9 < 0)
                                {
                                    item.Userfloat9 = null;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                decimal? p4 = null;
                                if (txtP4.Text != "")
                                {
                                    p4 = decimal.Parse(txtP4.Text);

                                    if (p4 < 0)
                                        p4 = null;
                                }

                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                if (ApplicableTo == "Outlet Group")
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, (ddlOutletList.SelectedValue + "").GetIntValue());
                                    ogl.Load();

                                }
                                else
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);
                                    ogl.Load();
                                }

                                if (ogl.Count > 0)
                                {
                                    OutletGroupItemMap sd = ogl[0];
                                    sd.P4 = p4;
                                    sd.Save();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex.Message);
                                item.Userfloat9 = null;
                            }
                        }
                    }

                    if (P5Name != null && P5Name != "")
                    {
                        if (ApplicableTo == "Product Master")
                        {
                            if (txtP5.Text == "")
                            {
                                item.Userfloat10 = null;
                            }
                            else
                            {
                                item.Userfloat10 = decimal.Parse(txtP5.Text); // Promotion Prce
                                if (item.Userfloat10 < 0)
                                {
                                    item.Userfloat10 = null;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                decimal? p5 = null;
                                if (txtP5.Text != "")
                                {
                                    p5 = decimal.Parse(txtP5.Text);

                                    if (p5 < 0)
                                        p5 = null;
                                }

                                OutletGroupItemMapCollection ogl = new OutletGroupItemMapCollection();

                                if (ApplicableTo == "Outlet Group")
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletGroupID, (ddlOutletList.SelectedValue + "").GetIntValue());
                                    ogl.Load();

                                }
                                else
                                {
                                    ogl.Where(OutletGroupItemMap.Columns.ItemNo, item.ItemNo);
                                    ogl.Where(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);
                                    ogl.Load();
                                }

                                if (ogl.Count > 0)
                                {
                                    OutletGroupItemMap sd = ogl[0];
                                    sd.P5 = p5;
                                    sd.Save();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex.Message);
                                item.Userfloat10 = null;
                            }
                        }
                    }
                    #endregion

                    item.GSTRule = ddGST.SelectedIndex;
                    item.ItemDesc = txtItemDesc.Text;
                    item.Attributes1 = txtAttributes1.Text;
                    item.Attributes2 = txtAttributes2.Text;
                    item.Attributes3 = txtAttributes3.Text;
                    item.Attributes4 = txtAttributes4.Text;
                    item.Attributes5 = txtAttributes5.Text;
                    item.Attributes6 = txtAttributes6.Text;
                    item.Attributes7 = txtAttributes7.Text;
                    item.Attributes8 = txtAttributes8.Text;
                    if (fuItemPicture.HasFile)
                        item.ItemImage = ImageCompressor.ResizeAndCompressImage(new MemoryStream(fuItemPicture.FileBytes));
                    item.Remark = txtRemark.Text;
                    item.AllowPreOrder = cbPreOrder.Checked;
                    item.CapQty = int.Parse(txtPreOrderCapQty.Text == "" ? "0" : txtPreOrderCapQty.Text);

                    item.IsVendorDelivery = cbVendorDelivery.Checked;
                    item.IsPAMedifund = cbPAMed.Checked;
                    item.IsSMF = cbSMF.Checked;
                    item.IsConsignment = chkIsConsigment.Checked;
                    item.ExcludeProfitLoss = cbIsExcludeProfitLossReport.Checked ? 1 : 0;
                    item.IsUsingFixedCOG = IsUsingFixedCOG.Checked;
                    if (item.IsUsingFixedCOG)
                    {
                        decimal FixedCOGValue = 0;

                        if (IsFixedCOGPercentage.Checked)
                        {
                            decimal.TryParse(txtFixedCOGPercentage.Text, out FixedCOGValue);
                            item.FixedCOGType = ItemController.FIXEDCOG_PERCENTAGE;
                            item.FixedCOGValue = FixedCOGValue;
                        }
                        else
                        {
                            decimal.TryParse(txtFixedCOGValue.Text, out FixedCOGValue);
                            item.FixedCOGType = ItemController.FIXEDCOG_VALUE;
                            item.FixedCOGValue = FixedCOGValue;
                        }
                    }
                    else
                    {
                        item.FixedCOGType = "";
                        item.FixedCOGValue = 0;
                    }
                    /*------------------CUSTOM CODE----------------------------
                    if (FileUpload1.HasFile)
                    {
                        ItemController itemCtr = new ItemController();
                        itemCtr.UploadPicture(
                          id, FileUpload1.
                          FileBytes, FileUpload1.FileName.Split('.')[1].ToUpper());
                    }*/

                    if (ddlApplicableTo.SelectedValue == "Outlet" && ((bool)ViewState["IsNew"])) // if add item to outlet
                        item.Deleted = true;
                    else
                        if (ddlApplicableTo.SelectedValue == "Outlet") // Edit outlet item
                            item.Deleted = item.Deleted;
                        else
                            item.Deleted = chkDeleted.Checked; // normal item
                    cmdCol.Add(item.GetSaveCommand(Session["username"].ToString()));

                    //Save supplier
                    int newSuppId = 0;
                    QueryCommand c = null;
                    if (int.TryParse(ddlSupplier.SelectedValue, out newSuppId))
                        c = SaveSupplier(item.ItemNo, newSuppId, item.FactoryPrice);
                    if (c != null)
                        cmdCol.Add(c);

                    ViewState["IsNew"] = false;
                    txtItemNoEditor.ReadOnly = true;
                    AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", "UPDATE Item " + item.ItemNo, "");
                    DataService.ExecuteTransaction(cmdCol);
                    UpdateLastGeneratedBarcode();

                    #region *) Audit Log
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.AuditLog.ProductSetup), false))
                    {
                        string operation = originalItem.IsNew ? "INSERT" : "UPDATE";
                        if (item.RetailPrice != originalItem.RetailPrice)
                            AuditLogController.AddLog(operation, "Item", "ItemNo", item.ItemNo, "RetailPrice = " + originalItem.RetailPrice.ToString("N2"), "RetailPrice = " + item.RetailPrice.ToString("N2"), Session["username"].ToString());
                    }
                    #endregion

                    if (futurePrice != null)
                        futurePrice.Save(Session["UserName"] + "");
                    transScope.Complete();
                    resultItemNo = item.ItemNo;
                    resultMatrixMode = "No";
                    isSuccess = true;
                }
                #endregion
            }
        }

        catch (Exception x)
        {
            isSuccess = false;
            status = x.Message;
            Logger.writeLog(x);
        }
        return isSuccess;
    }

    private QueryCommand SaveSupplier(string itemno, int newSupplierID, decimal factoryPrice)
    {
        //QueryCommand qc = new QueryCommand();
        ItemSupplierMapCollection ismCol = new ItemSupplierMapCollection();
        ismCol.Where(ItemSupplierMap.Columns.ItemNo, itemno);
        ismCol.Load();

        if (newSupplierID == -1)
        {
            //if set the supplier to empty and item supplier map no data yet then no need to do anything
            if (ismCol.Count == 0)
            {
                return null;
            }

            ///if set the supplier to empty and item supplier map exist then update deleted
            if (ismCol.Count > 0)
            {
                ismCol[0].Deleted = true;
                return ismCol[0].GetUpdateCommand(Session["UserName"].ToString());
            }
        }
        else
        {
            //if set the supplier have value and item supplier map no data yet then no need to do anything
            if (ismCol.Count > 0)
            {

                ismCol[0].SupplierID = newSupplierID;
                ismCol[0].CostPrice = factoryPrice;


                ismCol[0].Deleted = false;
                return ismCol[0].GetUpdateCommand(Session["UserName"].ToString());
            }

            ///if set the supplier to empty and item supplier map exist then update deleted
            if (ismCol.Count == 0)
            {
                //new ItemSupplierMap
                ItemSupplierMap ism = new ItemSupplierMap();
                ism.SupplierID = newSupplierID;
                ism.ItemNo = itemno;
                ism.CostPrice = factoryPrice;
                ism.Deleted = false;
                return ism.GetInsertCommand(Session["UserName"].ToString());
            }
        }
        return null;
    }

    private void UpdateLastGeneratedBarcode()
    {
        try
        {
            if (ViewState["GenerateBarcode"] != null && ViewState["GenerateBarcode"] is bool && (bool)ViewState["GenerateBarcode"] == true && !string.IsNullOrEmpty(txtBarcode.Text))
            {
                string opt = AppSetting.GetSetting(AppSetting.SettingsName.Item.OptionAutoGenerateBarcode);
                switch (opt)
                {
                    case "A":
                        {
                            string prefix = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.Item.BarcodePrefix));
                            if (string.IsNullOrEmpty(prefix))
                                AppSetting.SetSetting(AppSetting.SettingsName.Item.LastBarcodeGenerated, txtBarcode.Text);
                            else
                            {
                                if (txtBarcode.Text.StartsWith(prefix))
                                    AppSetting.SetSetting(AppSetting.SettingsName.Item.LastBarcodeGenerated, txtBarcode.Text.Replace(prefix, ""));
                            }
                        }
                        break;
                    case "B":
                        {
                            Category ct = new Category(ddlCategoryName.SelectedValue);
                            if (!ct.IsNew)
                            {
                                string prefix = ct.BarcodePrefix;
                                if (!string.IsNullOrEmpty(prefix) && txtBarcode.Text.StartsWith(prefix))
                                {
                                    Query qr = Category.CreateQuery();
                                    qr.QueryType = QueryType.Update;
                                    qr.AddWhere(Category.Columns.CategoryName, ddlCategoryName.SelectedValue);
                                    qr.AddUpdateSetting(Category.UserColumns.LastBarcodeGenerated, txtBarcode.Text.Replace(prefix, ""));
                                    qr.AddUpdateSetting(Category.Columns.ModifiedBy, Session["Username"].ToString());
                                    qr.AddUpdateSetting(Category.Columns.ModifiedOn, DateTime.Now);
                                    qr.Execute();
                                    AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("UPDATE Category : {0}", ddlCategoryName.SelectedValue), "");
                                }
                            }
                        }
                        break;
                }

            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }

    #endregion

    #region *) Event Handler

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        #region *) Item Type Setup

        bool DisplayItemOpenPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayItemOpenPrice), true);
        bool DisplayItemService = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayItemService), true);
        bool DisplayItemPointPackage = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayItemPointPackage), true);
        bool DisplayItemCourse = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayItemCourse), true);

        bool DisplayGiveCommission = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayGiveCommission), true);
        bool DisplayIsNonDiscountable = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayIsNonDiscountable), true);
        bool DisplayPointRedeemable = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPointRedeemable), true);
        bool DisplaySupplier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplaySupplier), true);
        bool DisplayUOM = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayUOM), false);
        bool DisplayAutoCaptureWeight = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayAutoCaptureWeight), false);

        bool DisplayPrice1 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice1), true);
        bool DisplayPrice2 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice2), true);
        bool DisplayPrice3 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice3), true);
        bool DisplayPrice4 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice4), true);
        bool DisplayPrice5 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice5), true);

        discountrow1.Visible = DisplayPrice1;
        discountrow2.Visible = DisplayPrice2;
        discountrow3.Visible = DisplayPrice3;
        discountrow4.Visible = DisplayPrice4;
        discountrow5.Visible = DisplayPrice5;


        string CostPriceText = string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Item.CostPriceText)) ? "Cost Price" : AppSetting.GetSetting(AppSetting.SettingsName.Item.CostPriceText);
        string RetailPriceText = string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Item.RetailPriceText)) ? "Retail Price" : AppSetting.GetSetting(AppSetting.SettingsName.Item.RetailPriceText);

        ltRetailPrice.Text = RetailPriceText;
        ltCostPrice.Text = CostPriceText;

        GridView1.Columns[6].HeaderText = RetailPriceText;
        GridView1.Columns[7].HeaderText = CostPriceText;

        rowOpenPrice.Visible = DisplayItemOpenPrice;
        rowService.Visible = DisplayItemService;
        rowPointPackage.Visible = DisplayItemPointPackage;
        rowCourse1.Visible = DisplayItemCourse;
        rowCourse2.Visible = DisplayItemCourse;

        rowGiveCommission.Visible = DisplayGiveCommission;
        rowNonDiscountable.Visible = DisplayIsNonDiscountable;
        rowPointRedeemable.Visible = DisplayPointRedeemable;
        rowSupplier.Visible = DisplaySupplier;
        ltUOM.Visible = DisplayUOM;
        txtUOM.Visible = DisplayUOM;
        rowAutoCaptureWeight.Visible = DisplayAutoCaptureWeight;


        bool DisplayApplyFuturePrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayApplyFuturePrice), false);
        pnlApplyFuturePrice.Visible = DisplayApplyFuturePrice;
        /*TrcApplyFuturePrice.Visible = DisplayApplyFuturePrice;
        trFutureRetailPrice.Visible = DisplayApplyFuturePrice;
        trFuturePriceDate.Visible = DisplayApplyFuturePrice;
        trFuturePrice1.Visible = DisplayApplyFuturePrice;
        trFuturePrice2.Visible = DisplayApplyFuturePrice;
        trFuturePrice3.Visible = DisplayApplyFuturePrice;
        trFuturePrice4.Visible = DisplayApplyFuturePrice;
        trFuturePrice5.Visible = DisplayApplyFuturePrice;*/

        rowMinimumSellingPrice.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayApplyFuturePrice), false);

        for (int i = 0; i < GridView1.Columns.Count; i++)
        {
            string header = GridView1.Columns[i].HeaderText;
            if (header == "Non Discountable")
                GridView1.Columns[i].Visible = DisplayIsNonDiscountable;
            else if (header == "Point Type" || header == "Point Amount")
                GridView1.Columns[i].Visible = DisplayItemPointPackage;
            else if (header == "Service Item")
                GridView1.Columns[i].Visible = DisplayItemService;
            else if (header == "UOM")
                GridView1.Columns[i].Visible = DisplayUOM;
        }

        #endregion

        P1Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P1DiscountName);
        P2Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P2DiscountName);
        P3Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P3DiscountName);
        P4Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P4DiscountName);
        P5Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P5DiscountName);

        // Hide Unused Attributes Controls (AttributesLabel)
        if (ProductAttributeInfo.Attributes1 == null)
        {
            attributesrow1.Visible = false;
        }
        if (ProductAttributeInfo.Attributes2 == null)
        {
            attributesrow2.Visible = false;
        }
        if (ProductAttributeInfo.Attributes3 == null)
        {
            attributesrow3.Visible = false;
        }
        if (ProductAttributeInfo.Attributes4 == null)
        {
            attributesrow4.Visible = false;
        }
        if (ProductAttributeInfo.Attributes5 == null)
        {
            attributesrow5.Visible = false;
        }
        if (ProductAttributeInfo.Attributes6 == null)
        {
            attributesrow6.Visible = false;
        }
        if (ProductAttributeInfo.Attributes7 == null)
        {
            attributesrow7.Visible = false;
        }
        if (ProductAttributeInfo.Attributes8 == null)
        {
            attributesrow8.Visible = false;
        }

        if (P1Name == null || P1Name == "")
        {
            discountrow1.Visible = false;
        }
        else
        {
            lblP1.Text = P1Name;
            lblFutureP1.Text = P1Name;
        }
        if (P2Name == null || P2Name == "")
        {
            discountrow2.Visible = false;
        }
        else
        {
            lblP2.Text = P2Name;
            lblFutureP2.Text = P2Name;
        }
        if (P3Name == null || P3Name == "")
        {
            discountrow3.Visible = false;
        }
        else
        {
            lblP3.Text = P3Name;
            lblFutureP3.Text = P3Name;
        }
        if (P4Name == null || P4Name == "")
        {
            discountrow4.Visible = false;
        }
        else
        {
            lblP4.Text = P4Name;
            lblFutureP4.Text = P4Name;
        }
        if (P5Name == null || P5Name == "")
        {
            discountrow5.Visible = false;
        }
        else
        {
            lblP5.Text = P5Name;
            lblFutureP5.Text = P5Name;
        }

        #region Implementation of Add new privilege called "Add New Item"

        BtnAddNew.Style.Add("display", "none");
        btnSaveNew.Style.Add("display", "none");

        if (Session["Username"] != null)
        {
            var dt = UserController.FetchUsers(Session["Username"] != null ? Session["Username"].ToString() : "", "", "", 0);
            var dr = dt.Select().FirstOrDefault();
            if (dr != null)
            {
                if (UserController.FetchGroupPrivilegesWithUsername(dr[10].ToString(), Session["UserName"].ToString()).Select("privilegeName='" + PrivilegesController.ADD_NEW_ITEM + "'").Count() > 0)
                {
                    BtnAddNew.Style.Remove("display");
                    btnSaveNew.Style.Remove("display");
                }
            }
        }

        #endregion
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            // NOTE: the following uses an overload of RegisterClientScriptBlock() 
            // that will surround our string with the needed script tags 
            ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = true;", true);
        }

        if (!IsPostBack)
        {
            #region *) Load Filter Data

            CategoryCollection categories = new CategoryCollection();
            ArrayList list = new ArrayList();
            categories.Load();
            for (int i = 0; i < categories.Count; i++)
            {
                list.Add(categories[i].CategoryName);
            }
            MultiCheckCombo1.ClearAll();
            MultiCheckCombo1.AddItems(list);
            list.Clear();

            bool useAttributeFilter = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseSelectableAttributesFilter), true);
            bool useItemNameFilter = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseSelectableItemNameFilter), true);

            trFilterItemName.Visible = useItemNameFilter;
            if (useItemNameFilter)
            {
                QueryCommand cmdItemName = new QueryCommand("SELECT DISTINCT ItemName FROM Item WHERE ISNULL(Deleted, 0) = 0 ORDER BY ItemName");
                var items = new DataTable();
                items.Load(DataService.GetReader(cmdItemName));
                for (int i = 0; i < items.Rows.Count; i++)
                {
                    ListBoxItemName.Items.Add(new ListItem((items.Rows[i]["ItemName"] + "")));
                }
            }

            trLblAttrib1.Visible = useAttributeFilter;
            trListAttrib1.Visible = useAttributeFilter;
            tblAttrib2.Visible = useAttributeFilter;
            tblAttrib3.Visible = useAttributeFilter;
            if (useAttributeFilter)
            {
                AttributesLabelCollection attributes = new AttributesLabelCollection();
                attributes.Load();

                DataSet ds = new DataSet();
                for (int i = 0; i < attributes.Count; i++)
                {
                    QueryCommand cmd = new QueryCommand(string.Format("SELECT DISTINCT(Attributes{0}) FROM Item WHERE Attributes{0} <> '' AND Attributes{0} IS NOT NULL ", attributes[i].AttributesNo));
                    DataTable dt = SubSonic.DataService.GetDataSet(cmd).Tables[0].Copy();
                    dt.TableName = "Attributes" + attributes[i].AttributesNo;
                    ds.Tables.Add(dt);
                }
                if (ds.Tables["Attributes1"] != null && ds.Tables["Attributes1"].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables["Attributes1"].Rows)
                    {
                        ListBox1.Items.Add(new ListItem(row["Attributes1"].ToString()));
                    }
                }
                else
                {
                    ListBox1.Visible = false;
                    lbl_Attributes1.Visible = false;
                }
                if (ds.Tables["Attributes2"] != null && ds.Tables["Attributes2"].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables["Attributes2"].Rows)
                    {
                        ListBox2.Items.Add(new ListItem(row["Attributes2"].ToString()));
                    }
                }
                else
                {
                    ListBox2.Visible = false;
                    lbl_Attributes2.Visible = false;
                }
                if (ds.Tables["Attributes3"] != null && ds.Tables["Attributes3"].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables["Attributes3"].Rows)
                    {
                        ListBox3.Items.Add(new ListItem(row["Attributes3"].ToString()));
                    }
                }
                else
                {
                    ListBox3.Visible = false;
                    lbl_Attributes3.Visible = false;
                }
                //Attributes 4
                if (ds.Tables["Attributes4"] != null && ds.Tables["Attributes4"].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables["Attributes4"].Rows)
                    {
                        ListBox4.Items.Add(new ListItem(row["Attributes4"].ToString()));
                    }
                }
                else
                {
                    ListBox4.Visible = false;
                    lbl_Attributes4.Visible = false;
                }
                //Attributes 5
                if (ds.Tables["Attributes5"] != null && ds.Tables["Attributes5"].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables["Attributes5"].Rows)
                    {
                        ListBox5.Items.Add(new ListItem(row["Attributes5"].ToString()));
                    }
                }
                else
                {
                    ListBox5.Visible = false;
                    lbl_Attributes5.Visible = false;
                }
                //Attributes 6
                if (ds.Tables["Attributes6"] != null && ds.Tables["Attributes6"].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables["Attributes6"].Rows)
                    {
                        ListBox6.Items.Add(new ListItem(row["Attributes6"].ToString()));
                    }
                }
                else
                {
                    ListBox6.Visible = false;
                    lbl_Attributes6.Visible = false;
                }

                //Attributes 7
                if (ds.Tables["Attributes7"] != null && ds.Tables["Attributes7"].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables["Attributes7"].Rows)
                    {
                        ListBox7.Items.Add(new ListItem(row["Attributes7"].ToString()));
                    }
                }
                else
                {
                    ListBox7.Visible = false;
                    lbl_Attributes7.Visible = false;
                }

                //Attributes 8
                if (ds.Tables["Attributes8"] != null && ds.Tables["Attributes8"].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables["Attributes8"].Rows)
                    {
                        ListBox8.Items.Add(new ListItem(row["Attributes8"].ToString()));
                    }
                }
                else
                {
                    ListBox8.Visible = false;
                    lbl_Attributes8.Visible = false;
                }

                //Attributes 9
                if (ds.Tables["Attributes9"] != null && ds.Tables["Attributes9"].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables["Attributes9"].Rows)
                    {
                        ListBox9.Items.Add(new ListItem(row["Attributes9"].ToString()));
                    }
                }
                else
                {
                    ListBox9.Visible = false;
                    lbl_Attributes9.Visible = false;
                }
            }
            #endregion

            #region *) Load Feature Setting
            trPreOrder.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowPreOrder), false);
            trVendorDelivery.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.ShowVendorDeliveryOption), false);
            trFunding.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableFunding), false);
            cbPAMed.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePAMed), false);
            txtPAMed.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePAMed), false);
            cbSMF.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableSMF), false);
            txtSMF.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableSMF), false);
            rowMinimumSellingPrice.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayMinimumSellingPrice), false);
            #endregion

            #region *) Show/hide Generate Barcode button

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AutoGenerateBarcode), false))
            {
                string opt = AppSetting.GetSetting(AppSetting.SettingsName.Item.OptionAutoGenerateBarcode);
                if (opt == "A")
                {
                    btnGenerateBarcode.Visible = !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Item.LastBarcodeGenerated));
                }
                else if (opt == "B")
                {
                    btnGenerateBarcode.Visible = !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Item.CategoryRunningNumberNoofDigit));
                }
            }
            else
                btnGenerateBarcode.Visible = false;

            #endregion
        }
        else
        {
            MultiCheckCombo1.unselectAllItems();
        }

        if (Session["DeleteMessage"] != null && Session["DeleteMessage"].ToString() != "")
        {
            lblMsg.Text = Session["DeleteMessage"].ToString();
            Session["DeleteMessage"] = null;
        }
        else
        {
            lblMsg.Text = "";
        }

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
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + msg + "</span>";
            }
            #endregion

            string matrixmode = Request.QueryString["matrixmode"] == null ? "No" : Request.QueryString["matrixmode"];
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                if (!Page.IsPostBack)
                {
                    LoadAttributes();
                    ViewState["IsNew"] = false;
                    SetProductLabels();
                    ddlCategoryName.DataSource = ItemController.FetchCategoryNames();
                    ddlCategoryName.DataBind();
                    //Load Supplier List
                    ddlSupplier.DataSource = ItemSupplierMapController.FetchSupplierList();
                    ddlSupplier.DataValueField = "SupplierID";
                    ddlSupplier.DataTextField = "SupplierName";
                    ddlSupplier.DataBind();

                    txtItemNoEditor.Text = ItemController.getNewItemRefNo();
                    LoadEditor(id, matrixmode);
                    BindGvBarcode();
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadAttributes();
                    ViewState["IsNew"] = true;
                    SetProductLabels();
                    ddlCategoryName.DataSource = ItemController.FetchCategoryNames();
                    ddlCategoryName.DataBind();
                    //Load Supplier List
                    ddlSupplier.DataSource = ItemSupplierMapController.FetchSupplierList();
                    ddlSupplier.DataValueField = "SupplierID";
                    ddlSupplier.DataTextField = "SupplierName";
                    ddlSupplier.DataBind();


                    txtItemNoEditor.Text = ItemController.getNewItemRefNo();
                    BindGvBarcode();
                    ToggleEditor(true);
                }
                btnDelete.Visible = false;
            }

            string mode = "normal";

            if ((Request.QueryString["matrixmode"] != null) && (Request.QueryString["matrixmode"].ToString().ToLower() == "yes"))
            {
                mode = "matrix";
            }

            if (mode == "matrix")
            {
                if (!IsPostBack)
                {
                    UserFlag1.Value = "true";
                    attributesrow1.Visible = false;
                    attributesrow2.Visible = false;
                    attributesrow3.Visible = false;
                    attributesrow4.Visible = false;
                    //attributesrow5.Visible = false;
                    attributesrowmatrix.Visible = true;
                    btnMatrix.CssClass = "classLightBlue";
                    btnNormal.CssClass = "classname";

                    divBarcodeMatrix.Visible = true;
                    divBarcodeNormal.Visible = false;
                    divBarcodeNull.Visible = false;
                    divPrefixBarcode.Visible = false;

                    if (!String.IsNullOrEmpty(id) && id != "0")
                    {
                        divPrefixBarcode.Visible = true;
                    }
                    else
                    {
                        divBarcodeNull.Visible = true;
                    }
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    UserFlag1.Value = "false";
                    attributesrow1.Visible = true;
                    attributesrow2.Visible = true;
                    attributesrow3.Visible = true;
                    attributesrow4.Visible = true;
                    attributesrow5.Visible = true;
                    attributesrowmatrix.Visible = false;
                    btnMatrix.CssClass = "classname";
                    btnNormal.CssClass = "classLightBlue";
                    divBarcodeMatrix.Visible = false;
                    divBarcodeNormal.Visible = true;
                }
            }

            /*if (!Page.IsPostBack)
            {
                // Load Supplier
                dtSupplier = ItemSupplierMapController.GetSupplierListByItemNo(id); // assign global variable
                ViewState["dtSupplier"] = dtSupplier; // copy to viewstate
                ddlSupplier.DataSource = dtSupplier;
                dgSupplier.DataBind();
            }
            else
            {
                dtSupplier = (DataTable)ViewState["dtSupplier"];
                ddlSupplier.DataSource = dtSupplier;
                dgSupplier.DataBind();
            }*/
        }
        else
        {
            ToggleEditor(false);
            if (!Page.IsPostBack)
            {
                SetProductLabels();
                BindGrid(0);
            }
            txtItemNo.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string status = "";
        string itemNo = "";
        string matrixMode = "";
        if (BindAndSave(out status, out itemNo, out matrixMode))
        {
            if (matrixMode.ToUpper() == "YES")
                Response.Redirect("NewProductMaster.aspx?id=" + itemNo + "&matrixmode=Yes&msg=" + LanguageManager.GetTranslation("Product saved"));
            else
                Response.Redirect("NewProductMaster.aspx?id=" + itemNo + "&msg=" + LanguageManager.GetTranslation("Product saved"));
        }
        else
        {
            if (status.Contains("Violation of PRIMARY KEY constraint"))
            {
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Product not saved. Item No: ") + txtItemNoEditor.Text + " " + LanguageManager.GetTranslation("has already been used. Choose another name") + "</span> ";
            }
            else
            {
                //haveError = true;
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Product not saved:") + "</span> " + status;
            }
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", "Delete Item " + Utility.GetParameter("id"), "");
        if (ddlApplicableTo.SelectedValue == "Product Master")
        {
            ItemController ctr = new ItemController();
            ctr.Delete(Utility.GetParameter("id"));

            #region *) Audit Log
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.AuditLog.ProductSetup), false))
            {
                AuditLogController.AddLog("DELETE", "Item", "ItemNo", Utility.GetParameter("id"), "Deleted = false", "Deleted = true", Session["username"].ToString());
            }
            #endregion
        }
        else if (ddlApplicableTo.SelectedValue == "OutletGroup")
        {
            OutletGroupItemMapCollection col = new OutletGroupItemMapCollection();
            col.Where(OutletGroupItemMap.Columns.ItemNo, Utility.GetParameter("id"));
            col.Where(OutletGroupItemMap.Columns.OutletGroupID, ddlOutletList.SelectedValue.GetIntValue());
            col.Where(OutletGroupItemMap.Columns.Deleted, false);
            col.Load();

            if (col.Count > 0)
            {
                OutletGroupItemMap ot = col[0];
                ot.Deleted = true;
                //ot.IsItemDeleted = chkDeleted.Checked;
                ot.Save(Session["username"] + "");
            }
            else
            {
                OutletGroupItemMap ot = new OutletGroupItemMap();
                ot.OutletGroupID = ddlOutletList.SelectedValue.GetIntValue();
                ot.ItemNo = Utility.GetParameter("id");
                ot.Deleted = true;
                //ot.IsItemDeleted = chkDeleted.Checked;
                ot.Save(Session["username"] + "");
            }
        }
        else if (ddlApplicableTo.SelectedValue == "Outlet")
        {
            OutletGroupItemMapCollection col = new OutletGroupItemMapCollection();
            col.Where(OutletGroupItemMap.Columns.ItemNo, Utility.GetParameter("id"));
            col.Where(OutletGroupItemMap.Columns.OutletName, ddlOutletList.SelectedValue);
            col.Load();

            if (col.Count > 0)
            {
                OutletGroupItemMap ot = col[0];
                ot.Deleted = true;
                //ot.IsItemDeleted = chkDeleted.Checked;
                ot.Save(Session["username"] + "");
            }
            else
            {
                OutletGroupItemMap ot = new OutletGroupItemMap();
                ot.OutletName = ddlOutletList.SelectedValue;
                ot.ItemNo = Utility.GetParameter("id");
                ot.Deleted = true;
                //ot.IsItemDeleted = ;
                ot.Save(Session["username"] + "");
            }
        }

        Response.Redirect(Request.CurrentExecutionFilePath);
    }

    protected void btnNormal_Click(object sender, EventArgs e)
    {
        Uri url = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);

        Response.Redirect(string.Format("{0}{1}{2}{3}?id={4}&&mode=normal", url.Scheme, Uri.SchemeDelimiter, url.Authority, url.AbsolutePath, txtItemNo.Text));
    }

    protected void btnMatrix_Click(object sender, EventArgs e)
    {
        Uri url = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);

        Response.Redirect(string.Format("{0}{1}{2}{3}?id={4}&&matrixmode=Yes", url.Scheme, Uri.SchemeDelimiter, url.Authority, url.AbsolutePath, string.IsNullOrEmpty(txtItemNo.Text) ? "0" : txtItemNo.Text));
    }

    protected void btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("NewProductMaster.aspx?id=0");
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                //Adi -- Hide the data now
                /*if (e.Row.Cells[26].Text.ToLower() == "yes")
                {
                    e.Row.Cells[0].Controls[0].Visible = false;
                }*/
                //e.Row.Cells[5].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[5].Text));
                //e.Row.Cells[6].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[6].Text));
                //e.Row.Cells[15].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[15].Text));
                string NumDigit = "N" + (String.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Item.NumDigit)) ? "2" : AppSetting.GetSetting(AppSetting.SettingsName.Item.NumDigit));
                e.Row.Cells[Factory_Price].Text = String.Format("{0}", Decimal.Parse(e.Row.Cells[Factory_Price].Text).ToString(NumDigit));
                var cell = e.Row.Cells[19];
                switch (cell.Text.ToUpper())
                {
                    case "":
                        cell.Text = "Non Point";
                        break;
                    case "N":
                        cell.Text = "Non Point";
                        break;
                    case "T":
                        cell.Text = "Package";
                        break;
                    case "D":
                        cell.Text = "Point Item";
                        break;
                    default:
                        cell.Text = "Non Point";
                        break;
                }
                DataControlField field = GridView1.Columns[0];
                TableCell td = e.Row.Cells[0];
                if (td.Controls.Count > 0 && td.Controls[0] is HyperLink)
                {
                    HyperLink hyperLink = (HyperLink)td.Controls[0];
                    HyperLinkField hyperLinkField = (HyperLinkField)field;
                    if (!String.IsNullOrEmpty(hyperLinkField.DataNavigateUrlFormatString))
                    {
                        string[] dataUrlFields =
                          new string[hyperLinkField.DataNavigateUrlFields.Length];
                        for (int j = 0; j < dataUrlFields.Length; j++)
                        {
                            object obj = DataBinder.Eval(e.Row.DataItem,
                                hyperLinkField.DataNavigateUrlFields[j]);
                            dataUrlFields[j] = HttpUtility.UrlEncode(
                                (obj == null ? "" : obj.ToString()));
                        }
                        hyperLink.NavigateUrl = String.Format(
                            hyperLinkField.DataNavigateUrlFormatString, dataUrlFields);
                    }
                }


            }
            catch (Exception ex)
            {
                //Unable to convert
                Logger.writeLog(ex);
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid(0);
    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        ArrayList lstAttributes1 = new ArrayList();
        foreach (ListItem li in ListBox1.Items)
        {
            if (li.Selected)
                lstAttributes1.Add(li.Text);
        }
        ArrayList lstAttributes2 = new ArrayList();
        foreach (ListItem li in ListBox2.Items)
        {
            if (li.Selected)
                lstAttributes2.Add(li.Text);
        }
        ArrayList lstAttributes3 = new ArrayList();
        foreach (ListItem li in ListBox3.Items)
        {
            if (li.Selected)
                lstAttributes3.Add(li.Text);
        }
        ArrayList lstAttributes4 = new ArrayList();
        foreach (ListItem li in ListBox4.Items)
        {
            if (li.Selected)
                lstAttributes4.Add(li.Text);
        }
        ArrayList lstAttributes5 = new ArrayList();
        foreach (ListItem li in ListBox5.Items)
        {
            if (li.Selected)
                lstAttributes5.Add(li.Text);
        }
        ArrayList lstAttributes6 = new ArrayList();
        foreach (ListItem li in ListBox6.Items)
        {
            if (li.Selected)
                lstAttributes6.Add(li.Text);
        }
        ArrayList lstAttributes7 = new ArrayList();
        foreach (ListItem li in ListBox7.Items)
        {
            if (li.Selected)
                lstAttributes7.Add(li.Text);
        }
        ArrayList lstAttributes8 = new ArrayList();
        foreach (ListItem li in ListBox8.Items)
        {
            if (li.Selected)
                lstAttributes8.Add(li.Text);
        }
        ArrayList lstAttributes9 = new ArrayList();
        foreach (ListItem li in ListBox9.Items)
        {
            if (li.Selected)
                lstAttributes9.Add(li.Text);
        }
        ArrayList lstItemNames = new ArrayList();
        foreach (ListItem li in ListBoxItemName.Items)
        {
            if (li.Selected)
                lstItemNames.Add(li.Text);
        }

        ItemController it = new ItemController();
        string[] category = MultiCheckCombo1.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

        BindGrid(0);
        DataTable dt = it.SearchItem(txtItemNo.Text, false, category,
            ddlApplicableTo.SelectedValue, ddlOutletList.SelectedValue, lstItemNames,
            lstAttributes1, lstAttributes2, lstAttributes3, lstAttributes4, lstAttributes5,
            lstAttributes6, lstAttributes7, lstAttributes8, lstAttributes9, null);

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Rows[i][j] is string && !dt.Columns[j].ReadOnly)
                {
                    try
                    {
                        dt.Rows[i][j] = dt.Rows[i][j].ToString().Replace("\"", "\"\"");
                    }
                    catch { }
                }
            }
        }

        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, GridView1);
    }

    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        BindGrid(ddlPages.SelectedIndex + 1);
    }

    protected void dgPriceScheme_EditCommand(object sender, DataGridCommandEventArgs e)
    {
        ViewState["editModeCurrentRow"] = e.Item.ItemIndex;
    }

    protected void dgPriceScheme_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (((LinkButton)e.CommandSource).CommandName)
        {
            case "Delete":
                DeleteItem(e);
                break;

            default:
                // Do nothing.
                break;
        }
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        MultiCheckCombo1.unselectAllItems();
        MultiCheckCombo1.Text = "";
        for (int i = 0; i < ListBox1.Items.Count; i++)
            ListBox1.Items[i].Selected = false;
        for (int i = 0; i < ListBox2.Items.Count; i++)
            ListBox2.Items[i].Selected = false;
        for (int i = 0; i < ListBox3.Items.Count; i++)
            ListBox3.Items[i].Selected = false;
        for (int i = 0; i < ListBox4.Items.Count; i++)
            ListBox4.Items[i].Selected = false;
        for (int i = 0; i < ListBox5.Items.Count; i++)
            ListBox5.Items[i].Selected = false;
        for (int i = 0; i < ListBox6.Items.Count; i++)
            ListBox6.Items[i].Selected = false;
    }

    protected void BtnSelectAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox field = (CheckBox)row.FindControl("CheckBox1");
            field.Checked = true;
        }
    }

    protected void BtnClearSelection_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox field = (CheckBox)row.FindControl("CheckBox1");
            field.Checked = false;
        }
    }

    protected void BtnDeleteSelection_Click(object sender, EventArgs e)
    {
        int count = 0;
        QueryCommandCollection commands = new QueryCommandCollection();
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox field = (CheckBox)row.FindControl("CheckBox1");
            if (field.Checked)
            {
                Item myItem = new Item(GridView1.Rows[row.RowIndex].Cells[colItemNo].Text);
                if (myItem.ItemNo != "")
                {
                    myItem.Deleted = true;
                    commands.Add(myItem.GetUpdateCommand("SYSTEM"));

                    #region *) Audit Log
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.AuditLog.ProductSetup), false))
                    {
                        AuditLogController.AddLog("DELETE", "Item", "ItemNo", myItem.ItemNo, "Deleted = false", "Deleted = true", Session["username"].ToString());
                    }
                    #endregion

                    count++;
                }

            }

        }
        SubSonic.DataService.ExecuteTransaction(commands);
        Session["DeleteMessage"] = String.Format("<span style='color:red; font-weight:bold;'>Deleted {0} Record(s)..</span>", count);
        Response.Redirect("NewProductMaster.aspx?");
    }

    protected void btnPrefixBarcode_Click(object sender, EventArgs e)
    {

        DataTable dtb = (DataTable)ViewState["ArrayBarcode"];

        if (dtb != null && dtb.Rows.Count > 0)
        {
            string prefix = txtPrefix.Text ?? "";

            int runningnumber = 1;
            if ((!string.IsNullOrEmpty(prefix)))
            {
                foreach (DataRow dr in dtb.Rows)
                {
                    dr["Barcode"] = prefix + runningnumber.ToString("00#");
                    runningnumber++;
                }

                ViewState["ArrayBarcode"] = dtb;
                BindGvBarcode();
            }
        }
    }

    protected void gvBarcode_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        ItemController itemLogic = new ItemController();

        divBarcodeWarningMessage.Visible = false;

        string itemno = gvBarcode.DataKeys[e.RowIndex].Value.ToString();
        GridViewRow row = (GridViewRow)gvBarcode.Rows[e.RowIndex];
        TextBox barcode = (TextBox)row.Cells[1].FindControl("txtBarcodeGV");

        DataTable dt = (DataTable)ViewState["ArrayBarcode"];
        try
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ItemNo"].ToString() == itemno)
                {
                    if (CheckDuplicateBarcodeViewState(barcode.Text, itemno))
                        throw new Exception(LanguageManager.GetTranslation("Barcode is duplicated"));

                    dr["Barcode"] = barcode.Text;
                    gvBarcode.EditIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            lblBarcodeWarningMessage.Text = ex.Message;
            divBarcodeWarningMessage.Visible = true;
        }
        ViewState["ArrayBarcode"] = dt;
        BindGvBarcode();
    }

    protected void gvBarcode_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        divBarcodeWarningMessage.Visible = false;
        gvBarcode.EditIndex = -1;
        BindGvBarcode();
    }

    protected void gvBarcode_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvBarcode.EditIndex = e.NewEditIndex;
        BindGvBarcode();
    }

    protected void btnAddAtt3_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtAddAtt3.Text))
        {
            ItemAttributeController attLogic = new ItemAttributeController();
            Query qr = ItemAttribute.Query();
            qr.WHERE("Type", Comparison.Equals, "Attributes3");
            qr.WHERE("Value", Comparison.Equals, txtAddAtt3.Text.Trim());
            ItemAttributeCollection col = attLogic.FetchByQuery(qr);

            if (col != null && col.Count() == 0)
            {
                ItemAttribute it = new ItemAttribute();
                it.ValueX = txtAddAtt3.Text;
                it.Type = "Attributes3";
                it.Save(Session["username"].ToString());

                ListItem li = new ListItem();
                li.Value = txtAddAtt3.Text;
                MatrixAttributes3.Items.Add(li);
            }
            txtAddAtt3.Text = "";
        }

        Page.ClientScript.RegisterStartupScript(this.GetType(), "RestoreScrollPosition", "setScrollPos();", true);
    }

    protected void btnAddAtt4_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtAddAtt4.Text))
        {
            ItemAttributeController attLogic = new ItemAttributeController();
            Query qr = ItemAttribute.Query();
            qr.WHERE("Type", Comparison.Equals, "Attributes4");
            qr.WHERE("Value", Comparison.Equals, txtAddAtt4.Text.Trim());
            ItemAttributeCollection col = attLogic.FetchByQuery(qr);

            if (col != null && col.Count() == 0)
            {
                ItemAttribute it = new ItemAttribute();
                it.ValueX = txtAddAtt4.Text;
                it.Type = "Attributes4";
                it.Save(Session["username"].ToString());

                ListItem li = new ListItem();
                li.Value = txtAddAtt4.Text;
                MatrixAttributes4.Items.Add(li);
            }

            txtAddAtt4.Text = "";
        }

        Page.ClientScript.RegisterStartupScript(this.GetType(), "RestoreScrollPosition", "setScrollPos();", true);
    }

    protected void btnRemoveImage_Click(object sender, EventArgs e)
    {
        Image2.ImageUrl = "";
    }

    protected void ddlApplicableTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ApplicableTo = ddlApplicableTo.SelectedValue;
        ddlOutletList.Items.Clear();
        if (ddlApplicableTo.SelectedItem.Text == "Product Master")
        {
            ddlOutletList.Enabled = false;
            BindGrid(0);
        }
        else if (ddlApplicableTo.SelectedItem.Text == "Outlet Group")
        {
            Query qryctrlOutletGroup = OutletGroup.CreateQuery();
            qryctrlOutletGroup.OrderBy = SubSonic.OrderBy.Asc("OutletGroupID");
            Utility.LoadDropDown(ddlOutletList, qryctrlOutletGroup.ExecuteReader(), true);
            ddlOutletList.Items.Insert(0, new ListItem("(Not Specified)", String.Empty));
            ddlOutletList.Enabled = true;
        }
        else if (ddlApplicableTo.SelectedItem.Text == "Outlet")
        {
            var outletList = OutletController.FetchByUserNameForReport(false, false, Session["UserName"] + "");
            var allOutlet = OutletController.FetchAll(false, false);
            bool isAssignedToAll = outletList.Count >= allOutlet.Count;
            if (isAssignedToAll)
            {

                OutletCollection outColl = new OutletCollection();
                outColl.Where(PowerPOS.Outlet.Columns.Deleted, false);
                outColl.OrderByAsc("OutletName");
                outColl.Load();
                foreach (Outlet ou in outColl)
                    ddlOutletList.Items.Add(new ListItem(ou.OutletName, ou.OutletName));
                ddlOutletList.Items.Insert(0, new ListItem("(Not Specified)", String.Empty));
                ddlOutletList.Enabled = true;
            }
            else
            {
                //
                ddlOutletList.Items.Clear();
                foreach (var ou in outletList)
                    ddlOutletList.Items.Add(ou.OutletName);
                ddlOutletList.Enabled = true;

            }
        }
    }

    protected void ddlOutlet_SelectedIndexChanged(object sender, EventArgs e)
    {
        OutletID = ddlOutletList.SelectedValue;
        BindGrid(ddlPages.SelectedIndex + 1);
    }

    protected void btnFirst_Click(object sender, EventArgs e)
    {
        ddlPages.SelectedIndex = 0;
        ddlPages_SelectedIndexChanged(ddlPages, new EventArgs());
    }

    protected void btnPrev_Click(object sender, EventArgs e)
    {
        if (ddlPages.SelectedIndex == 0)
            ddlPages.SelectedIndex = 0;
        else
            ddlPages.SelectedIndex = ddlPages.SelectedIndex - 1;
        ddlPages_SelectedIndexChanged(ddlPages, new EventArgs());
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (ddlPages.SelectedIndex == ddlPages.Items.Count - 1)
            ddlPages.SelectedIndex = ddlPages.Items.Count - 1;
        else
            ddlPages.SelectedIndex = ddlPages.SelectedIndex + 1;
        ddlPages_SelectedIndexChanged(ddlPages, new EventArgs());
    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        ddlPages.SelectedIndex = ddlPages.Items.Count - 1;
        ddlPages_SelectedIndexChanged(ddlPages, new EventArgs());
    }

    protected void btnSaveNew_Click(object sender, EventArgs e)
    {
        string status = "";
        string itemNo = "";
        string matrixMode = "";
        if (BindAndSave(out status, out itemNo, out matrixMode))
        {
            itemNo = "0";
            if (matrixMode.ToUpper() == "YES")
                Response.Redirect("NewProductMaster.aspx?id=" + itemNo + "&matrixmode=Yes&msg=" + LanguageManager.GetTranslation("Product saved"));
            else
                Response.Redirect("NewProductMaster.aspx?id=" + itemNo + "&msg=" + LanguageManager.GetTranslation("Product saved"));
        }
        else
        {
            if (status.Contains("Violation of PRIMARY KEY constraint"))
            {
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Product not saved. Item No:") + txtItemNoEditor.Text + " " + LanguageManager.GetTranslation("has already been used. Choose another name") + "</span> ";
            }
            else
            {
                //haveError = true;
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Product not saved:") + "</span> " + status;
            }
        }
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        int applicableTo = ddlApplicableTo.SelectedIndex;
        string outletID = ddlOutletList.SelectedValue;
        ToggleEditor(false);
        ddlApplicableTo.SelectedIndex = applicableTo;
        ddlApplicableTo_SelectedIndexChanged(ddlApplicableTo, e);
        ddlOutletList.SelectedValue = outletID;
        ddlOutlet_SelectedIndexChanged(ddlOutletList, e);
    }

    protected void BtnAddNew_Click(object sender, EventArgs e)
    {
        if (ApplicableTo == "Outlet" && string.IsNullOrEmpty(OutletID))
        {
            lblMsg.Text = "<span style='color:red; font-weight:bold;'>Please specify the Outlet first.</span>";
            return;
        }
        Response.Redirect("NewProductMaster.aspx?id=0");
    }

    protected void btnMerge_Click(object sender, EventArgs e)
    {
        Response.Redirect("MergeSimilarItem.aspx");
    }

    protected void btnGenerateBarcode_Click(object sender, EventArgs e)
    {
        string opt = AppSetting.GetSetting(AppSetting.SettingsName.Item.OptionAutoGenerateBarcode);
        switch (opt)
        {
            case "A":
                try
                {
                    int runningNo = 0;
                    int length = 1;
                    string result = "";

                    string lastBarcode = AppSetting.GetSetting(AppSetting.SettingsName.Item.LastBarcodeGenerated);
                    if (!string.IsNullOrEmpty(lastBarcode))
                        length = lastBarcode.Length;

                    int.TryParse(lastBarcode, out runningNo);

                    string prefix = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.Item.BarcodePrefix));

                    while (true)
                    {
                        runningNo += 1;
                        result = prefix + runningNo.ToString().PadLeft(length, '0');
                        Item itm = new Item(Item.Columns.Barcode, result);
                        if (itm == null || string.IsNullOrEmpty(itm.Barcode) || itm.Barcode != result)
                            break;
                    }

                    txtBarcode.Text = result;
                    ViewState["GenerateBarcode"] = true;
                }
                catch (Exception ex)
                {
                    lblResult.Text = string.Format("Error occured when generating barcode: {0}", ex.Message);
                    Logger.writeLog(ex);
                }
                break;
            case "B":
                try
                {
                    int runningNo = 0;
                    int length = 1;
                    string result = "";

                    Category ct = new Category(ddlCategoryName.SelectedValue);
                    if (ct.IsNew)
                    {
                        lblResult.Text = string.Format("Please select category before generate barcode");
                        return;
                    }

                    string ctRunningNumber = AppSetting.GetSetting(AppSetting.SettingsName.Item.CategoryRunningNumberNoofDigit);
                    if (string.IsNullOrEmpty(ctRunningNumber))
                    {
                        lblResult.Text = string.Format("Running Number No of Digit is empty!");
                        return;
                    }

                    if (!int.TryParse(ctRunningNumber, out length))
                    {
                        lblResult.Text = string.Format("Running Number No of Digit is not a number!");
                        return;
                    }

                    if (string.IsNullOrEmpty(ct.BarcodePrefix))
                    {
                        lblResult.Text = string.Format("Barcode Prefix is empty!");
                        return;
                    }



                    string prefix = ct.BarcodePrefix;
                    int.TryParse(ct.LastBarcodeGenerated, out runningNo);

                    while (true)
                    {
                        runningNo += 1;
                        result = prefix + runningNo.ToString().PadLeft(length, '0');
                        Item itm = new Item(Item.Columns.Barcode, result);
                        if (itm == null || string.IsNullOrEmpty(itm.Barcode) || itm.Barcode != result)
                            break;
                    }

                    txtBarcode.Text = result;
                    ViewState["GenerateBarcode"] = true;

                    lblResult.Text = "";
                }
                catch (Exception ex)
                {
                    lblResult.Text = string.Format("Error occured when generating barcode: {0}", ex.Message);
                    Logger.writeLog(ex);
                }
                break;
        }
    }

    protected void btnSearchItem_Click(object sender, EventArgs e)
    {
        string search = txtSearchItem.Text;

        if (search == string.Empty)
            search = "%";

        string sql = "select * from Item where IsInInventory = 1 and (ISNULL(deleted,0) = 0 OR EXISTS(SELECT * FROM OutletGroupItemMap WHERE ItemNo = Item.ItemNo AND ISNULL(Deleted, 0) = 0) ) and ISNULL(ItemNo,'') + ' ' + ISNULL(ItemName,'') + ' ' + ISNULL(Barcode,'') LIKE '%" + search.Replace("'", "''") + "%' order by ItemName";
        DataSet ds = DataService.GetDataSet(new QueryCommand(sql));
        ItemCollection col = new ItemCollection();
        col.Load(ds.Tables[0]);

        ddlItem.DataSource = col;
        ddlItem.DataBind();
    }

    protected void btnSetDeductItem_Click(object sender, EventArgs e)
    {
        string itemno = ddlItem.SelectedValue;
        if (!string.IsNullOrEmpty(itemno))
        {
            Item it = new Item(itemno);

            if (it != null)
            {
                DeductedItemNo.Value = it.ItemNo;
                DeductedItemLabel.Text = it.ItemName;
                DeductedUOM.Text = !string.IsNullOrEmpty(it.UOM) ? it.UOM : "pcs";
            }
        }
    }
    #endregion
}
