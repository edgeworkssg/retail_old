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
using PowerPOS;
using SubSonic;
using System.Collections.Generic;

namespace PowerWeb.Product
{
    public partial class ViewOutletPricing : System.Web.UI.Page
    {
        bool DisplayPrice1 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice1), true);
        bool DisplayPrice2 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice2), true);
        bool DisplayPrice3 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice3), true);
        bool DisplayPrice4 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice4), true);
        bool DisplayPrice5 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice5), true);

        protected void Page_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            if (!this.IsPostBack)
            {
                string itemNo = Request.QueryString["ItemNo"] + "";
                if (string.IsNullOrEmpty(itemNo) || !BindData(itemNo))
                    CloseWindow("");
            }
        }

        private bool BindData(string itemNo)
        {


            string P1Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P1DiscountName);
            string P2Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P2DiscountName);
            string P3Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P3DiscountName);
            string P4Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P4DiscountName);
            string P5Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P5DiscountName);

            if (!DisplayPrice1)
            {
                gvProductMaster.Columns[3].Visible = false;
                gvOutletGroupLevel.Columns[3].Visible = false;
                gvOutletLevel.Columns[3].Visible = false;
            }
            else {
                gvProductMaster.Columns[3].HeaderText = P1Name;
            }

            if (!DisplayPrice2)
            {
                gvProductMaster.Columns[4].Visible = false;
                gvOutletGroupLevel.Columns[4].Visible = false;
                gvOutletLevel.Columns[4].Visible = false;
            }
            else
            {
                gvProductMaster.Columns[4].HeaderText = P2Name;
            }

            if (!DisplayPrice3)
            {
                gvProductMaster.Columns[5].Visible = false;
                gvOutletGroupLevel.Columns[5].Visible = false;
                gvOutletLevel.Columns[5].Visible = false;
            }
            else
            {
                gvProductMaster.Columns[5].HeaderText = P3Name;
            }

            if (!DisplayPrice4)
            {
                gvProductMaster.Columns[6].Visible = false;
                gvOutletGroupLevel.Columns[6].Visible = false;
                gvOutletLevel.Columns[6].Visible = false;
            }
            else
            {
                gvProductMaster.Columns[6].HeaderText = P4Name;
            }

            if (!DisplayPrice5)
            {
                gvProductMaster.Columns[7].Visible = false;
                gvOutletGroupLevel.Columns[7].Visible = false;
                gvOutletLevel.Columns[7].Visible = false;
            }
            else
            {
                gvProductMaster.Columns[7].HeaderText = P5Name;
            }

            Item theItem = new Item(itemNo);
            if (!theItem.IsNew)
            {
                lblItemNo.Text = theItem.ItemNo;
                lblItemName.Text = theItem.ItemName;
                lblProductMasterPrice.Text = theItem.RetailPrice.ToString("N2");

                DataTable dtMaster = new DataTable();
                dtMaster.Columns.Add("Active", Type.GetType("System.Boolean"));
                dtMaster.Columns.Add("RetailPrice", Type.GetType("System.Decimal"));
                dtMaster.Columns.Add("P1", Type.GetType("System.Decimal"));
                dtMaster.Columns.Add("P2", Type.GetType("System.Decimal"));
                dtMaster.Columns.Add("P3", Type.GetType("System.Decimal"));
                dtMaster.Columns.Add("P4", Type.GetType("System.Decimal"));
                dtMaster.Columns.Add("P5", Type.GetType("System.Decimal"));

                dtMaster.Rows.Add(theItem.Deleted == false, theItem.RetailPrice, theItem.Userfloat6, theItem.Userfloat7, theItem.Userfloat8, theItem.Userfloat9, theItem.Userfloat10);
                gvProductMaster.DataSource = dtMaster;
                gvProductMaster.DataBind();

                gvOutletGroupLevel.DataSource = theItem.OutletGroupPricing;
                gvOutletGroupLevel.DataBind();

                gvOutletLevel.DataSource = theItem.OutletPricing;
                gvOutletLevel.DataBind();

                gvSupplier.DataSource = ItemSupplierMapController.GetSupplierListByItemNo(theItem.ItemNo);
                gvSupplier.DataBind();

                CheckAssignedOutlet();
                return true;
            }
            else
            {
                return false;
            }
        }

        //protected void gvOutletLevel_DataBound(object sender, EventArgs e)
        protected void CheckAssignedOutlet()
        {
            UserMst user = new UserMst(Session["username"] + "");

            string[] outletList = user.AssignedOutletList;
            bool isProductMasterEnabled = true;
            foreach (GridViewRow row in gvOutletLevel.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    Label lblOutletName = (Label)row.FindControl("lblOutletName");
                    if (Array.IndexOf(outletList, lblOutletName.Text) < 0)
                    {
                        row.Enabled = false;
                        isProductMasterEnabled = false;
                    }
                }
            }

            int[] outletGroupList = user.AssignedOutletGroupList;
            foreach (GridViewRow row in gvOutletGroupLevel.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    Label lblOutletGroupID = (Label)row.FindControl("lblOutletGroupID");

                    bool isOutletGroupEnabled = true;
                    OutletCollection oList = new OutletCollection();
                    oList.Where(Outlet.Columns.OutletGroupID, lblOutletGroupID.Text.GetIntValue());
                    oList.Load();
                    foreach (Outlet ou in oList)
                    {
                        if (Array.IndexOf(outletList, ou.OutletName) < 0)
                        {
                            isOutletGroupEnabled = false;
                        }
                    }

                    if (!isOutletGroupEnabled)
                    {
                        row.Enabled = false;
                    }
                    /*if (Array.IndexOf(outletGroupList, lblOutletGroupID.Text.GetIntValue()) < 0)
                    {
                        row.Enabled = false;
                        isProductMasterEnabled = false;
                    }*/ 
                }
            }
            foreach (GridViewRow row in gvProductMaster.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Enabled = isProductMasterEnabled;
                    Button btnApplyPrice = (Button)row.FindControl("btnApplyPrice");

                    btnApplyPrice.Visible = isProductMasterEnabled;
                }
            }
        }

        private void CloseWindow(string addScript)
        {
            string script = addScript + "window.close();";
            ClientScript.RegisterStartupScript(typeof(Page), "CloseWindow", "<script type=\"text/javascript\">" + script + "</script>");
        }

        protected void btnSetToProductMaster_Click(object sender, EventArgs e)
        {
            if (ItemController.SetOutletPrice((Request.QueryString["ItemNo"] + ""), true, Session["UserName"] + ""))
            {
                lblStatus.Text = "Outlet Price Updated";
                BindData((Request.QueryString["ItemNo"] + ""));
            }
            else
            {
                lblStatus.Text = "Outlet Price Update Failed!";
            }
        }

        protected void btnSetToIndividualOutlet_Click(object sender, EventArgs e)
        {
            if (ItemController.SetOutletPrice((Request.QueryString["ItemNo"] + ""), false, Session["UserName"] + ""))
            {
                lblStatus.Text = "Outlet Price Updated";
                BindData((Request.QueryString["ItemNo"] + ""));
            }
            else
            {
                lblStatus.Text = "Outlet Price Update Failed!";
            }
        }

        protected void btnSavePrice_Click(object sender, EventArgs e)
        {
            QueryCommandCollection cmdColl = new QueryCommandCollection();
            Item itm = new Item(lblItemNo.Text);
            decimal productRetailPrice = 0;
            decimal? productP1 = null;
            decimal? productP2 = null;
            decimal? productP3 = null;
            decimal? productP4 = null;
            decimal? productP5 = null;
            bool isProductMasterChecked = false;
            //Validation and Get Item Retail Price
            foreach (GridViewRow gvRow in gvProductMaster.Rows)
            {
                if (gvRow.RowType != DataControlRowType.DataRow) continue;

                CheckBox chk = (CheckBox)gvRow.FindControl("chkSelected");
                TextBox txtRetailPrice = (TextBox)gvRow.FindControl("txtRetailPrice");
                decimal retailPrice;
                if (!decimal.TryParse(txtRetailPrice.Text, out retailPrice))
                {
                    lblStatus.Text = "Invalid Retail Price.";
                    txtRetailPrice.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {
                    lblStatus.Text = "";
                    txtRetailPrice.ForeColor = System.Drawing.Color.Black;
                }
                productRetailPrice = retailPrice;
                isProductMasterChecked = chk.Checked;

                if (DisplayPrice1)
                {
                    decimal P1;
                    TextBox txtP1 = (TextBox)gvRow.FindControl("txtP1");
                    if (txtP1.Text != "" && !decimal.TryParse(txtP1.Text, out P1))
                    {
                        lblStatus.Text = "Invalid P1.";
                        txtP1.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        txtP1.ForeColor = System.Drawing.Color.Black;
                        if(txtP1.Text != "")
                            productP1 = decimal.Parse(txtP1.Text);
                    }                    
                }

                if (DisplayPrice2)
                {
                    decimal P2;
                    TextBox txtP2 = (TextBox)gvRow.FindControl("txtP2");
                    if (txtP2.Text != "" && !decimal.TryParse(txtP2.Text, out P2))
                    {
                        lblStatus.Text = "Invalid P2.";
                        txtP2.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        txtP2.ForeColor = System.Drawing.Color.Black;
                        if (txtP2.Text != "")
                            productP2 = decimal.Parse(txtP2.Text);
                    }
                }

                if (DisplayPrice3)
                {
                    decimal P3;
                    TextBox txtP3 = (TextBox)gvRow.FindControl("txtP3");
                    if (txtP3.Text != "" && !decimal.TryParse(txtP3.Text, out P3))
                    {
                        lblStatus.Text = "Invalid P3.";
                        txtP3.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        txtP3.ForeColor = System.Drawing.Color.Black;
                        if (txtP3.Text != "")
                            productP3 = decimal.Parse(txtP3.Text);
                    }
                }

                if (DisplayPrice4)
                {
                    decimal P4;
                    TextBox txtP4 = (TextBox)gvRow.FindControl("txtP4");
                    if (txtP4.Text != "" && !decimal.TryParse(txtP4.Text, out P4))
                    {
                        lblStatus.Text = "Invalid P4.";
                        txtP4.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        txtP4.ForeColor = System.Drawing.Color.Black;
                        if (txtP4.Text != "")
                            productP4 = decimal.Parse(txtP4.Text);
                    }
                }

                if (DisplayPrice5)
                {
                    decimal P5;
                    TextBox txtP5 = (TextBox)gvRow.FindControl("txtP5");
                    if (txtP5.Text != "" && !decimal.TryParse(txtP5.Text, out P5))
                    {
                        lblStatus.Text = "Invalid P5.";
                        txtP5.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        txtP5.ForeColor = System.Drawing.Color.Black;
                        if (txtP5.Text != "")
                            productP5 = decimal.Parse(txtP5.Text);
                    }
                }
            }

            if (itm != null && itm.ItemNo == lblItemNo.Text)
            {
                #region Outlet Group level
                foreach (GridViewRow gvRow in gvOutletGroupLevel.Rows)
                {
                    decimal? P1 = null;
                    decimal? P2 = null;
                    decimal? P3 = null;
                    decimal? P4 = null;
                    decimal? P5 = null;

                    if (gvRow.RowType != DataControlRowType.DataRow) continue;
                    if (gvRow.Enabled == false) continue;

                    CheckBox chk = (CheckBox)gvRow.FindControl("chkSelected");
                    Label lblOutletGroupID = (Label)gvRow.FindControl("lblOutletGroupID");
                    TextBox txtRetailPrice = (TextBox)gvRow.FindControl("txtRetailPrice");

                    decimal retailPrice;
                    if (!decimal.TryParse(txtRetailPrice.Text, out retailPrice))
                    {
                        lblStatus.Text = "Invalid Retail Price.";
                        txtRetailPrice.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        txtRetailPrice.ForeColor = System.Drawing.Color.Black;
                    }

                    if (DisplayPrice1)
                    {
                        decimal sP1;
                        TextBox txtP1 = (TextBox)gvRow.FindControl("txtP1");
                        if (txtP1.Text != "" && !decimal.TryParse(txtP1.Text, out sP1))
                        {
                            lblStatus.Text = "Invalid P1.";
                            txtP1.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            lblStatus.Text = "";
                            txtP1.ForeColor = System.Drawing.Color.Black;
                            if (txtP1.Text != "")
                                P1 = decimal.Parse(txtP1.Text);
                        }                        
                    }

                    if (DisplayPrice2)
                    {
                        decimal sP2;
                        TextBox txtP2 = (TextBox)gvRow.FindControl("txtP2");
                        if (txtP2.Text != "" && !decimal.TryParse(txtP2.Text, out sP2))
                        {
                            lblStatus.Text = "Invalid P2.";
                            txtP2.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            lblStatus.Text = "";
                            txtP2.ForeColor = System.Drawing.Color.Black;
                            if (txtP2.Text != "")
                                P2 = decimal.Parse(txtP2.Text);
                        }
                    }

                    if (DisplayPrice3)
                    {
                        decimal sP3;
                        TextBox txtP3 = (TextBox)gvRow.FindControl("txtP3");
                        if (txtP3.Text != "" && !decimal.TryParse(txtP3.Text, out sP3))
                        {
                            lblStatus.Text = "Invalid P3.";
                            txtP3.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            lblStatus.Text = "";
                            txtP3.ForeColor = System.Drawing.Color.Black;
                            if (txtP3.Text != "")
                                P3 = decimal.Parse(txtP3.Text);
                        }                       
                    }

                    if (DisplayPrice4)
                    {
                        decimal sP4;
                        TextBox txtP4 = (TextBox)gvRow.FindControl("txtP4");
                        if (txtP4.Text != "" && !decimal.TryParse(txtP4.Text, out sP4))
                        {   
                            lblStatus.Text = "Invalid P4.";
                            txtP4.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            lblStatus.Text = "";
                            txtP4.ForeColor = System.Drawing.Color.Black;
                            if (txtP4.Text != "")
                                P4 = decimal.Parse(txtP4.Text);
                        }
                    }

                    if (DisplayPrice5)
                    {
                        decimal sP5;
                        TextBox txtP5 = (TextBox)gvRow.FindControl("txtP5");
                        if (txtP5.Text != "" && !decimal.TryParse(txtP5.Text, out sP5))
                        {
                            lblStatus.Text = "Invalid P5.";
                            txtP5.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            lblStatus.Text = "";
                            txtP5.ForeColor = System.Drawing.Color.Black;
                            if (txtP5.Text != "")
                                P5 = decimal.Parse(txtP5.Text);
                        }
                    }

                    int outletGroupID;
                    if (!int.TryParse(lblOutletGroupID.Text, out outletGroupID))
                    {
                        lblStatus.Text = "Invalid Outlet Group ID.";
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                    }

                    Query qr = new Query("OutletGroupItemMap");
                    qr.AddWhere(OutletGroupItemMap.Columns.ItemNo, lblItemNo.Text);
                    qr.AddWhere(OutletGroupItemMap.Columns.OutletGroupID, outletGroupID);
                    OutletGroupItemMapCollection ogimColl = new OutletGroupItemMapController().FetchByQuery(qr);

                    if (isProductMasterChecked)
                    {
                        if (chk.Checked)
                        {
                            //Ticked so item is active in this outlet group id
                            if (ogimColl.Count > 0)
                            {
                                //Record Already Exist Need To delete
                                ogimColl[0].RetailPrice = retailPrice;
                                ogimColl[0].P1 = P1;
                                ogimColl[0].P2 = P2;
                                ogimColl[0].P3 = P3;
                                ogimColl[0].P4 = P4;
                                ogimColl[0].P5 = P5;
                                ogimColl[0].Deleted = false;
                                if ((retailPrice == productRetailPrice) &&
                                    (!DisplayPrice4 || (DisplayPrice1 && productP1 == P1)) &&
                                    (!DisplayPrice4 || (DisplayPrice2 && productP2 == P2)) &&
                                    (!DisplayPrice4 || (DisplayPrice3 && productP3 == P3)) &&
                                    (!DisplayPrice4 || (DisplayPrice4 && productP4 == P4)) &&
                                    (!DisplayPrice5 || (DisplayPrice5 && productP5 == P5)))
                                {
                                    string sqlQuery = "Delete OutletGroupItemMap Where ItemNo = '" + ogimColl[0].ItemNo + "' and OutletGroupID = " + ogimColl[0].OutletGroupID.ToString() + "";
                                    cmdColl.Add(new QueryCommand(sqlQuery));
                                }
                                else
                                {
                                    cmdColl.Add(ogimColl[0].GetUpdateCommand(Session["username"] + ""));
                                }
                            }
                            else
                            {
                                //Record is not available, need to create with deleted = false
                                if ((retailPrice != productRetailPrice)
                                    || (DisplayPrice1 && productP1 != P1)
                                    || (DisplayPrice2 && productP2 != P2)
                                    || (DisplayPrice3 && productP3 != P3)
                                    || (DisplayPrice4 && productP4 != P4)
                                    || (DisplayPrice5 && productP5 != P5))
                                {
                                    OutletGroupItemMap ogim = new OutletGroupItemMap();
                                    ogim.OutletGroupID = outletGroupID;
                                    ogim.ItemNo = lblItemNo.Text;
                                    ogim.RetailPrice = retailPrice;
                                    ogim.CostPrice = new Item(lblItemNo.Text).FactoryPrice;
                                    ogim.P1 = P1;
                                    ogim.P2 = P2;
                                    ogim.P3 = P3;
                                    ogim.P4 = P4;
                                    ogim.P5 = P5;
                                    ogim.Deleted = false;
                                    ogim.IsItemDeleted = false;
                                    cmdColl.Add(ogim.GetSaveCommand(Session["username"] + ""));
                                }
                            }
                        }
                        else
                        {
                            if (ogimColl.Count > 0)
                            {
                                //Record Already Exist Need To Update Price 
                                ogimColl[0].RetailPrice = retailPrice;
                                ogimColl[0].P1 = P1;
                                ogimColl[0].P2 = P2;
                                ogimColl[0].P3 = P3;
                                ogimColl[0].P4 = P4;
                                ogimColl[0].P5 = P5;
                                ogimColl[0].Deleted = true;
                                cmdColl.Add(ogimColl[0].GetUpdateCommand(Session["username"] + ""));
                            }
                            else
                            {
                                OutletGroupItemMap ogim = new OutletGroupItemMap();
                                ogim.OutletGroupID = outletGroupID;
                                ogim.ItemNo = lblItemNo.Text;
                                ogim.RetailPrice = retailPrice;
                                ogim.CostPrice = new Item(lblItemNo.Text).FactoryPrice;
                                ogim.P1 = P1;
                                ogim.P2 = P2;
                                ogim.P3 = P3;
                                ogim.P4 = P4;
                                ogim.P5 = P5;
                                ogim.Deleted = true;
                                ogim.IsItemDeleted = false;
                                cmdColl.Add(ogim.GetSaveCommand(Session["username"] + ""));
                            }
                        }
                    }
                    else
                    {//is Product Master is Not Checked

                        if (chk.Checked)
                        {
                            //Ticked so item is active in this outlet group id
                            if (ogimColl.Count > 0)
                            {
                                //Record Already Exist Need To delete
                                ogimColl[0].RetailPrice = retailPrice;
                                ogimColl[0].P1 = P1;
                                ogimColl[0].P2 = P2;
                                ogimColl[0].P3 = P3;
                                ogimColl[0].P4 = P4;
                                ogimColl[0].P5 = P5;
                                ogimColl[0].Deleted = false;
                                cmdColl.Add(ogimColl[0].GetUpdateCommand(Session["username"] + ""));
                            }
                            else
                            {
                                //Record is not available, need to create with deleted = false
                                OutletGroupItemMap ogim = new OutletGroupItemMap();
                                ogim.OutletGroupID = outletGroupID;
                                ogim.ItemNo = lblItemNo.Text;
                                ogim.RetailPrice = retailPrice;
                                ogim.CostPrice = new Item(lblItemNo.Text).FactoryPrice;
                                ogim.P1 = P1;
                                ogim.P2 = P2;
                                ogim.P3 = P3;
                                ogim.P4 = P4;
                                ogim.P5 = P5;
                                ogim.Deleted = false;
                                ogim.IsItemDeleted = false;
                                cmdColl.Add(ogim.GetSaveCommand(Session["username"] + ""));
                            }
                        }
                        else
                        {
                            if (ogimColl.Count > 0)
                            {
                                //Record Already Exist Need To Delete Price 
                                string sqlQuery = "Delete OutletGroupItemMap Where ItemNo = '" + ogimColl[0].ItemNo + "' and OutletGroupID = " + ogimColl[0].OutletGroupID.ToString() + "";
                                cmdColl.Add(new QueryCommand(sqlQuery));

                            }
                        }
                    }

                    /*if (ogimColl.Count > 0)
                    {
                        ogimColl[0].RetailPrice = retailPrice;
                        ogimColl[0].Deleted = !chk.Checked;
                        cmdColl.Add(ogimColl[0].GetSaveCommand(Session["username"] + ""));
                    }
                    else
                    {
                        if (chk.Checked)
                        {
                            OutletGroupItemMap ogim = new OutletGroupItemMap();
                            ogim.OutletGroupID = outletGroupID;
                            ogim.ItemNo = lblItemNo.Text;
                            ogim.RetailPrice = retailPrice;
                            ogim.CostPrice = new Item(lblItemNo.Text).FactoryPrice;
                            ogim.Deleted = false;
                            ogim.IsItemDeleted = false;
                            cmdColl.Add(ogim.GetSaveCommand(Session["username"] + ""));
                        }
                    }*/
                }
                #endregion

                #region Outlet level
                foreach (GridViewRow gvRow in gvOutletLevel.Rows)
                {
                    decimal? P1 = null;
                    decimal? P2 = null;
                    decimal? P3 = null;
                    decimal? P4 = null;
                    decimal? P5 = null;

                    if (gvRow.RowType != DataControlRowType.DataRow) continue;
                    if (gvRow.Enabled == false) continue;

                    CheckBox chk = (CheckBox)gvRow.FindControl("chkSelected");
                    Label lblOutletName = (Label)gvRow.FindControl("lblOutletName");
                    TextBox txtRetailPrice = (TextBox)gvRow.FindControl("txtRetailPrice");

                    decimal retailPrice;
                    if (!decimal.TryParse(txtRetailPrice.Text, out retailPrice))
                    {
                        lblStatus.Text = "Invalid Retail Price.";
                        txtRetailPrice.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        txtRetailPrice.ForeColor = System.Drawing.Color.Black;
                    }

                    if (DisplayPrice1)
                    {
                        decimal sP1;
                        TextBox txtP1 = (TextBox)gvRow.FindControl("txtP1");
                        if (txtP1.Text != "" && !decimal.TryParse(txtP1.Text, out sP1))
                        {
                            lblStatus.Text = "Invalid P1.";
                            txtP1.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            lblStatus.Text = "";
                            txtP1.ForeColor = System.Drawing.Color.Black;
                            if (txtP1.Text != "")
                                P1 = decimal.Parse(txtP1.Text);
                        }
                    }

                    if (DisplayPrice2)
                    {
                        decimal sP2;
                        TextBox txtP2 = (TextBox)gvRow.FindControl("txtP2");
                        if (txtP2.Text != "" && !decimal.TryParse(txtP2.Text, out sP2))
                        {
                            lblStatus.Text = "Invalid P2.";
                            txtP2.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            lblStatus.Text = "";
                            txtP2.ForeColor = System.Drawing.Color.Black;
                            if (txtP2.Text != "")
                                P2 = decimal.Parse(txtP2.Text);
                        }
                    }

                    if (DisplayPrice3)
                    {
                        decimal sP3;
                        TextBox txtP3 = (TextBox)gvRow.FindControl("txtP3");
                        if (txtP3.Text != "" && !decimal.TryParse(txtP3.Text, out sP3))
                        {
                            lblStatus.Text = "Invalid P3.";
                            txtP3.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            lblStatus.Text = "";
                            txtP3.ForeColor = System.Drawing.Color.Black;
                            if (txtP3.Text != "")
                                P3 = decimal.Parse(txtP3.Text);
                        }
                    }

                    if (DisplayPrice4)
                    {
                        decimal sP4;
                        TextBox txtP4 = (TextBox)gvRow.FindControl("txtP4");
                        if (txtP4.Text != "" && !decimal.TryParse(txtP4.Text, out sP4))
                        {
                            lblStatus.Text = "Invalid P4.";
                            txtP4.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            lblStatus.Text = "";
                            txtP4.ForeColor = System.Drawing.Color.Black;
                            if (txtP4.Text != "")
                                P4 = decimal.Parse(txtP4.Text);
                        }
                    }

                    if (DisplayPrice5)
                    {
                        decimal sP5;
                        TextBox txtP5 = (TextBox)gvRow.FindControl("txtP5");
                        if (txtP5.Text != "" && !decimal.TryParse(txtP5.Text, out sP5))
                        {
                            lblStatus.Text = "Invalid P5.";
                            txtP5.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            lblStatus.Text = "";
                            txtP5.ForeColor = System.Drawing.Color.Black;
                            if (txtP5.Text != "")
                                P5 = decimal.Parse(txtP5.Text);
                        }
                    }

                    Query qr = new Query("OutletGroupItemMap");
                    qr.AddWhere(OutletGroupItemMap.Columns.ItemNo, lblItemNo.Text);
                    qr.AddWhere(OutletGroupItemMap.Columns.OutletName, lblOutletName.Text);
                    OutletGroupItemMapCollection ogimColl = new OutletGroupItemMapController().FetchByQuery(qr);

                    if (isProductMasterChecked)
                    {
                        if (chk.Checked)
                        {
                            //Ticked so item is active in this outlet group id
                            if (ogimColl.Count > 0)
                            {
                                //Record Already Exist Need To delete
                                ogimColl[0].RetailPrice = retailPrice;
                                ogimColl[0].P1 = P1;
                                ogimColl[0].P2 = P2;
                                ogimColl[0].P3 = P3;
                                ogimColl[0].P4 = P4;
                                ogimColl[0].P5 = P5;
                                ogimColl[0].Deleted = false;
                                if ((retailPrice == productRetailPrice) &&
                                    (!DisplayPrice4 || (DisplayPrice1 && productP1 == P1)) &&
                                    (!DisplayPrice4 || (DisplayPrice2 && productP2 == P2)) &&
                                    (!DisplayPrice4 || (DisplayPrice3 && productP3 == P3)) &&
                                    (!DisplayPrice4 || (DisplayPrice4 && productP4 == P4)) &&
                                    (!DisplayPrice5 || (DisplayPrice5 && productP5 == P5)))
                                {
                                    string sqlQuery = "Delete OutletGroupItemMap Where ItemNo = '" + ogimColl[0].ItemNo + "' and OutletName = '" + ogimColl[0].OutletName + "'";
                                    cmdColl.Add(new QueryCommand(sqlQuery));
                                }
                                else
                                {
                                    cmdColl.Add(ogimColl[0].GetUpdateCommand(Session["username"] + ""));
                                }
                            }
                            else
                            {
                                //Record is not available, need to create with deleted = false
                                if ((retailPrice != productRetailPrice)
                                    || (DisplayPrice1 && productP1 != P1)
                                    || (DisplayPrice2 && productP2 != P2)
                                    || (DisplayPrice3 && productP3 != P3)
                                    || (DisplayPrice4 && productP4 != P4)
                                    || (DisplayPrice5 && productP5 != P5))
                                {
                                    OutletGroupItemMap ogim = new OutletGroupItemMap();
                                    ogim.OutletName = lblOutletName.Text;
                                    ogim.ItemNo = lblItemNo.Text;
                                    ogim.RetailPrice = retailPrice;
                                    ogim.CostPrice = new Item(lblItemNo.Text).FactoryPrice;
                                    ogim.P1 = P1;
                                    ogim.P2 = P2;
                                    ogim.P3 = P3;
                                    ogim.P4 = P4;
                                    ogim.P5 = P5;
                                    ogim.Deleted = false;
                                    ogim.IsItemDeleted = false;
                                    cmdColl.Add(ogim.GetSaveCommand(Session["username"] + ""));
                                }
                            }
                        }
                        else
                        {
                            if (ogimColl.Count > 0)
                            {
                                //Record Already Exist Need To Update Price 
                                ogimColl[0].RetailPrice = retailPrice;
                                ogimColl[0].P1 = P1;
                                ogimColl[0].P2 = P2;
                                ogimColl[0].P3 = P3;
                                ogimColl[0].P4 = P4;
                                ogimColl[0].P5 = P5;
                                ogimColl[0].Deleted = true;
                                cmdColl.Add(ogimColl[0].GetUpdateCommand(Session["username"] + ""));
                            }
                            else
                            {
                                OutletGroupItemMap ogim = new OutletGroupItemMap();
                                ogim.OutletName = lblOutletName.Text;
                                ogim.ItemNo = lblItemNo.Text;
                                ogim.RetailPrice = retailPrice;
                                ogim.CostPrice = new Item(lblItemNo.Text).FactoryPrice;
                                ogim.P1 = P1;
                                ogim.P2 = P2;
                                ogim.P3 = P3;
                                ogim.P4 = P4;
                                ogim.P5 = P5;
                                ogim.Deleted = true;
                                ogim.IsItemDeleted = false;
                                cmdColl.Add(ogim.GetSaveCommand(Session["username"] + ""));
                            }
                        }
                    }
                    else
                    {//is Product Master is Not Checked

                        if (chk.Checked)
                        {
                            //Ticked so item is active in this outlet group id
                            if (ogimColl.Count > 0)
                            {
                                //Record Already Exist Need To delete
                                ogimColl[0].RetailPrice = retailPrice;
                                ogimColl[0].P1 = P1;
                                ogimColl[0].P2 = P2;
                                ogimColl[0].P3 = P3;
                                ogimColl[0].P4 = P4;
                                ogimColl[0].P5 = P5;
                                ogimColl[0].Deleted = false;
                                cmdColl.Add(ogimColl[0].GetUpdateCommand(Session["username"] + ""));
                            }
                            else
                            {
                                //Record is not available, need to create with deleted = false
                                OutletGroupItemMap ogim = new OutletGroupItemMap();
                                ogim.OutletName = lblOutletName.Text;
                                ogim.ItemNo = lblItemNo.Text;
                                ogim.RetailPrice = retailPrice;
                                ogim.CostPrice = new Item(lblItemNo.Text).FactoryPrice;
                                ogim.P1 = P1;
                                ogim.P2 = P2;
                                ogim.P3 = P3;
                                ogim.P4 = P4;
                                ogim.P5 = P5;
                                ogim.Deleted = false;
                                ogim.IsItemDeleted = false;
                                cmdColl.Add(ogim.GetSaveCommand(Session["username"] + ""));
                            }
                        }
                        else
                        {
                            if (ogimColl.Count > 0)
                            {
                                //Record Already Exist Need To Delete Price 
                                string sqlQuery = "Delete OutletGroupItemMap Where ItemNo = '" + ogimColl[0].ItemNo + "' and OutletName = '" + ogimColl[0].OutletName + "'";
                                cmdColl.Add(new QueryCommand(sqlQuery));
                            }
                        }
                    }

                    /*if (ogimColl.Count > 0)
                    {
                        ogimColl[0].RetailPrice = retailPrice;
                        ogimColl[0].Deleted = !chk.Checked;
                        cmdColl.Add(ogimColl[0].GetSaveCommand(Session["username"] + ""));
                    }
                    else
                    {
                        if (chk.Checked)
                        {
                            OutletGroupItemMap ogim = new OutletGroupItemMap();
                            ogim.OutletName = lblOutletName.Text;
                            ogim.ItemNo = lblItemNo.Text;
                            ogim.RetailPrice = retailPrice;
                            ogim.CostPrice = new Item(lblItemNo.Text).FactoryPrice;
                            ogim.Deleted = false;
                            ogim.IsItemDeleted = false;
                            cmdColl.Add(ogim.GetSaveCommand(Session["username"] + ""));
                        }
                    }*/
                }
                #endregion

                #region Product Master level
                foreach (GridViewRow gvRow in gvProductMaster.Rows)
                {
                    if (gvRow.RowType != DataControlRowType.DataRow) continue;

                    CheckBox chk = (CheckBox)gvRow.FindControl("chkSelected");
                    TextBox txtRetailPrice = (TextBox)gvRow.FindControl("txtRetailPrice");
                    decimal retailPrice;
                    if (!decimal.TryParse(txtRetailPrice.Text, out retailPrice))
                    {
                        lblStatus.Text = "Invalid Retail Price.";
                        txtRetailPrice.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        txtRetailPrice.ForeColor = System.Drawing.Color.Black;
                    }

                    itm.RetailPrice = retailPrice;
                    itm.Userfloat6 = productP1;
                    itm.Userfloat7 = productP2;
                    itm.Userfloat8 = productP3;
                    itm.Userfloat9 = productP4;
                    itm.Userfloat10 = productP5;
                    itm.Deleted = !chk.Checked;
                    cmdColl.Add(itm.GetUpdateCommand(Session["username"] + ""));
                }
                #endregion

                cmdColl.RemoveAll(c => c == null);
                if (cmdColl.Count > 0)
                {
                    DataService.ExecuteTransaction(cmdColl);
                    lblStatus.Text = "Data saved successfully.";
                    BindData(itm.ItemNo);
                }
            }
        }

        protected void btnApplyPrice_Click(object sender, EventArgs e)
        {
            decimal productRetailPrice = 0;
            string P1Price = "";
            string P2Price = "";
            string P3Price = "";
            string P4Price = "";
            string P5Price = "";

            Item itm = new Item(lblItemNo.Text);

            foreach (GridViewRow gvRow in gvProductMaster.Rows)
            {
                if (gvRow.RowType != DataControlRowType.DataRow) continue;

                CheckBox chk = (CheckBox)gvRow.FindControl("chkSelected");
                TextBox txtRetailPrice = (TextBox)gvRow.FindControl("txtRetailPrice");
                decimal retailPrice;
                if (!decimal.TryParse(txtRetailPrice.Text, out retailPrice))
                {
                    lblStatus.Text = "Invalid Retail Price.";
                    txtRetailPrice.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {
                    lblStatus.Text = "";
                    txtRetailPrice.Text = retailPrice.ToString("N2");
                    txtRetailPrice.ForeColor = System.Drawing.Color.Black;
                }
                productRetailPrice = retailPrice;

                if (DisplayPrice1)
                {
                    TextBox txtP1 = (TextBox)gvRow.FindControl("txtP1");
                    decimal p1Price = 0;
                    if (txtP1.Text != "" && !decimal.TryParse(txtP1.Text, out p1Price))
                    {
                        lblStatus.Text = "Invalid P1 Price.";
                        txtP1.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        if(txtP1.Text != "")
                            txtP1.Text = p1Price.ToString("N2");
                        txtP1.ForeColor = System.Drawing.Color.Black;
                        P1Price = txtP1.Text;
                    }
                    
                }

                if (DisplayPrice2)
                {
                    TextBox txtP2 = (TextBox)gvRow.FindControl("txtP2");
                    decimal p2Price = 0;
                    if (txtP2.Text != "" && !decimal.TryParse(txtP2.Text, out p2Price))
                    {
                        lblStatus.Text = "Invalid P2 Price.";
                        txtP2.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        if(txtP2.Text != "")
                            txtP2.Text = p2Price.ToString("N2");
                        txtP2.ForeColor = System.Drawing.Color.Black;
                        P2Price = txtP2.Text;
                    }                    
                }

                if (DisplayPrice3)
                {
                    TextBox txtP3 = (TextBox)gvRow.FindControl("txtP3");
                    decimal p3Price = 0;
                    if (txtP3.Text != "" && !decimal.TryParse(txtP3.Text, out p3Price))
                    {
                        lblStatus.Text = "Invalid P3 Price.";
                        txtP3.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        if (txtP3.Text != "")
                            txtP3.Text = p3Price.ToString("N2");
                        txtP3.ForeColor = System.Drawing.Color.Black;
                        P3Price = txtP3.Text;
                    }                    
                }

                if (DisplayPrice4)
                {
                    TextBox txtP4 = (TextBox)gvRow.FindControl("txtP4");
                    decimal p4Price = 0;
                    if (txtP4.Text != "" && !decimal.TryParse(txtP4.Text, out p4Price))
                    {
                        lblStatus.Text = "Invalid P4 Price.";
                        txtP4.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        if (txtP4.Text != "")
                            txtP4.Text = p4Price.ToString("N2");
                        txtP4.ForeColor = System.Drawing.Color.Black;
                        P4Price = txtP4.Text;
                    }                    
                }

                if (DisplayPrice5)
                {
                    TextBox txtP5 = (TextBox)gvRow.FindControl("txtP5");
                    decimal p5Price = 0;
                    if (txtP5.Text != "" && !decimal.TryParse(txtP5.Text, out p5Price))
                    {
                        lblStatus.Text = "Invalid P5 Price.";
                        txtP5.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        if (txtP5.Text != "")
                            txtP5.Text = p5Price.ToString("N2");
                        txtP5.ForeColor = System.Drawing.Color.Black;
                        P5Price = txtP5.Text;
                    }
                }
            }

            if (itm != null && itm.ItemNo == lblItemNo.Text)
            {
                #region Outlet Group level
                foreach (GridViewRow gvRow in gvOutletGroupLevel.Rows)
                {
                    if (gvRow.RowType != DataControlRowType.DataRow) continue;
                    if (gvRow.Enabled == false) continue;

                    Label lblOutletGroupID = (Label)gvRow.FindControl("lblOutletGroupID");
                    TextBox txtRetailPrice = (TextBox)gvRow.FindControl("txtRetailPrice");

                    txtRetailPrice.Text = productRetailPrice.ToString("N2");

                    if (DisplayPrice1)
                    {
                        TextBox txtP1 = (TextBox)gvRow.FindControl("txtP1");
                        txtP1.Text = P1Price;
                    }

                    if (DisplayPrice2)
                    {
                        TextBox txtP2 = (TextBox)gvRow.FindControl("txtP2");
                        txtP2.Text = P2Price;
                    }
                    
                    if (DisplayPrice3)
                    {
                        TextBox txtP3 = (TextBox)gvRow.FindControl("txtP3");
                        txtP3.Text = P3Price;
                    }

                    if (DisplayPrice4)
                    {
                        TextBox txtP4 = (TextBox)gvRow.FindControl("txtP4");
                        txtP4.Text = P4Price;
                    }

                    if (DisplayPrice5)
                    {
                        TextBox txtP5 = (TextBox)gvRow.FindControl("txtP5");
                        txtP5.Text = P5Price;
                    }

                }
                #endregion

                #region Outlet level
                foreach (GridViewRow gvRow in gvOutletLevel.Rows)
                {
                    if (gvRow.RowType != DataControlRowType.DataRow) continue;
                    if (gvRow.Enabled == false) continue;

                    CheckBox chk = (CheckBox)gvRow.FindControl("chkSelected");
                    Label lblOutletName = (Label)gvRow.FindControl("lblOutletName");
                    TextBox txtRetailPrice = (TextBox)gvRow.FindControl("txtRetailPrice");

                    txtRetailPrice.Text = productRetailPrice.ToString("N2");


                    if (DisplayPrice1)
                    {
                        TextBox txtP1 = (TextBox)gvRow.FindControl("txtP1");
                        txtP1.Text = P1Price;
                    }

                    if (DisplayPrice2)
                    {
                        TextBox txtP2 = (TextBox)gvRow.FindControl("txtP2");
                        txtP2.Text = P2Price;
                    }

                    if (DisplayPrice3)
                    {
                        TextBox txtP3 = (TextBox)gvRow.FindControl("txtP3");
                        txtP3.Text = P3Price;
                    }

                    if (DisplayPrice4)
                    {
                        TextBox txtP4 = (TextBox)gvRow.FindControl("txtP4");
                        txtP4.Text = P4Price;
                    }

                    if (DisplayPrice5)
                    {
                        TextBox txtP5 = (TextBox)gvRow.FindControl("txtP5");
                        txtP5.Text = P5Price;
                    }

                }
                #endregion

            }
        }
    }
}
