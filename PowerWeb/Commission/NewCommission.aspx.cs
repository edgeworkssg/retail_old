using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PowerPOSLib.Controller.Commission;
using SubSonic.Utilities;
using PowerPOS;
using System.Collections;
using PowerPOSLib.Container;
using SubSonic;

namespace PowerWeb.Commission
{
    public partial class NewCommission : PageBase
    {
        private List<CommissionDetFor> forDetails;
        private List<CommissionDetBy> byDetails;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();

                if (Request.QueryString["id"] != null)
                {
                    PanelList.Visible = false;
                    PanelInput.Visible = true;

                    #region SalesGroup

                    {
                        UserGroupController userGroupController = new UserGroupController();
                        ddlSalesGroup.DataSource = userGroupController.FetchAll().ToList();
                        ddlSalesGroup.DataTextField = "GroupName";
                        ddlSalesGroup.DataValueField = "GroupID";
                        ddlSalesGroup.DataBind();

                        ddlSalesGroup.Items.Insert(0, new ListItem("Select One", ""));
                    }

                    #endregion

                    #region Category

                    {
                        CategoryController categoryController = new CategoryController();
                        ddlCategory.DataSource = categoryController.FetchAll().ToList();
                        ddlCategory.DataTextField = "CategoryName";
                        ddlCategory.DataValueField = "CategoryName";
                        ddlCategory.DataBind();

                        ddlCategory.Items.Insert(0, new ListItem("Select One", ""));
                    }

                    #endregion

                    #region Load Data

                    int id = 0;
                    if (int.TryParse(Request.QueryString["id"], out id))
                    {
                        if (id > 0)
                        {
                            PowerPOS.CommissionHdr obj = new CommissionHdr(id);
                            if (!obj.IsNew)
                            {
                                tbSchemeName.Text = obj.SchemeName;

                                #region Commission Types

                                cbProduct.Checked = obj.IsProduct.GetValueOrDefault(0) > 0;
                                if (cbProduct.Checked)
                                    tbProduct.Text = obj.ProductWeight.GetValueOrDefault(0).ToString("N2");
                                cbService.Checked = obj.IsService.GetValueOrDefault(0) > 0;
                                if (cbService.Checked)
                                    tbService.Text = obj.ServiceWeight.GetValueOrDefault(0).ToString("N2");
                                cbPointSold.Checked = obj.IsPointSold.GetValueOrDefault(0) > 0;
                                if (cbPointSold.Checked)
                                    tbPointSold.Text = obj.PointSoldWeight.GetValueOrDefault(0).ToString("N2");
                                cbPackageSold.Checked = obj.IsPackageSold.GetValueOrDefault(0) > 0;
                                if (cbPackageSold.Checked)
                                    tbPackageSold.Text = obj.PackageSoldWeight.GetValueOrDefault(0).ToString("N2");
                                cbPointRedeem.Checked = obj.IsPointRedeem.GetValueOrDefault(0) > 0;
                                if (cbPointRedeem.Checked)
                                    tbPointRedeem.Text = obj.PointRedeemWeight.GetValueOrDefault(0).ToString("N2");
                                cbPackageRedeem.Checked = obj.IsPackageRedeem.GetValueOrDefault(0) > 0;
                                if (cbPackageRedeem.Checked)
                                    tbPackageRedeem.Text = obj.PackageRedeemWeight.GetValueOrDefault(0).ToString("N2");

                                #endregion

                                #region Commission Detail For

                                Query q = new Query(CommissionDetFor.Schema);
                                q.AddWhere(CommissionDetFor.Columns.CommissionHdrID, id);

                                CommissionDetForCollection coll = new CommissionDetForCollection();
                                coll.LoadAndCloseReader(q.ExecuteReader());

                                forDetails = new List<CommissionDetFor>();
                                forDetails.AddRange(coll.ToList());

                                ViewState["forDetails"] = forDetails;

                                BindForGrid();

                                #endregion

                                #region Deduction

                                cbDeduction.Checked = obj.IsDeductionFor2ndSalesPerson.GetValueOrDefault(0) > 0;
                                if (obj.DeductionBy != null)
                                {
                                    if (obj.DeductionBy.ToLower().Equals("percentage"))
                                    {
                                        rbDeductionByPercentage.Checked = true;
                                        tbDeductionByPercentage.Text = obj.DeductionValue.GetValueOrDefault(0).ToString("N2");
                                    }
                                    else if (obj.DeductionBy.ToLower().Equals("amount"))
                                    {
                                        rbDeductionByAmount.Checked = true;
                                        tbDeductionByAmount.Text = obj.DeductionValue.GetValueOrDefault(0).ToString("N2");
                                    }
                                }

                                #endregion

                                ddlSalesGroup.SelectedValue = obj.SalesGroupID.GetValueOrDefault(0).ToString();

                                Page.ClientScript.RegisterHiddenField("rbCommissionBy", obj.CommissionBy);

                                #region Enable Input Percentage if iselected

                                if (obj.CommissionBy != null)
                                {
                                    if (obj.CommissionBy.ToLower().Equals("percentage"))
                                    {
                                        tbCQFrom1.Enabled = false;
                                        tbCQFrom2.Enabled = false;
                                        tbCQFrom3.Enabled = false;
                                        tbCQFrom4.Enabled = false;
                                        tbCQFrom5.Enabled = false;

                                        tbCQTo1.Enabled = false;
                                        tbCQTo2.Enabled = false;
                                        tbCQTo3.Enabled = false;
                                        tbCQTo4.Enabled = false;
                                        tbCQTo5.Enabled = false;

                                        tbCQValue1.Enabled = false;
                                        tbCQValue2.Enabled = false;
                                        tbCQValue3.Enabled = false;
                                        tbCQValue4.Enabled = false;
                                        tbCQValue5.Enabled = false;

                                        tbCQFrom1.Text = "";
                                        tbCQFrom2.Text = "";
                                        tbCQFrom3.Text = "";
                                        tbCQFrom4.Text = "";
                                        tbCQFrom5.Text = "";

                                        tbCQTo1.Text = "";
                                        tbCQTo2.Text = "";
                                        tbCQTo3.Text = "";
                                        tbCQTo4.Text = "";
                                        tbCQTo5.Text = "";

                                        tbCQValue1.Text = "";
                                        tbCQValue2.Text = "";
                                        tbCQValue3.Text = "";
                                        tbCQValue4.Text = "";
                                        tbCQValue5.Text = "";
                                    }
                                    else if (obj.CommissionBy.ToLower().Equals("amount"))
                                    {
                                        tbCPFrom1.Enabled = false;
                                        tbCPFrom2.Enabled = false;
                                        tbCPFrom3.Enabled = false;
                                        tbCPFrom4.Enabled = false;
                                        tbCPFrom5.Enabled = false;

                                        tbCPTo1.Enabled = false;
                                        tbCPTo2.Enabled = false;
                                        tbCPTo3.Enabled = false;
                                        tbCPTo4.Enabled = false;
                                        tbCPTo5.Enabled = false;

                                        tbCPValue1.Enabled = false;
                                        tbCPValue2.Enabled = false;
                                        tbCPValue3.Enabled = false;
                                        tbCPValue4.Enabled = false;
                                        tbCPValue5.Enabled = false;

                                        tbCPFrom1.Text = "";
                                        tbCPFrom2.Text = "";
                                        tbCPFrom3.Text = "";
                                        tbCPFrom4.Text = "";
                                        tbCPFrom5.Text = "";

                                        tbCPTo1.Text = "";
                                        tbCPTo2.Text = "";
                                        tbCPTo3.Text = "";
                                        tbCPTo4.Text = "";
                                        tbCPTo5.Text = "";

                                        tbCPValue1.Text = "";
                                        tbCPValue2.Text = "";
                                        tbCPValue3.Text = "";
                                        tbCPValue4.Text = "";
                                        tbCPValue5.Text = "";
                                    }
                                }

                                #endregion

                                #region Commission Detail By

                                Query q2 = new Query(CommissionDetBy.Schema);
                                q2.AddWhere(CommissionDetBy.Columns.CommissionHdrID, id);

                                CommissionDetByCollection coll2 = new CommissionDetByCollection();
                                coll2.LoadAndCloseReader(q2.ExecuteReader());

                                byDetails = coll2.ToList();

                                if (obj.CommissionBy != null)
                                {
                                    if (obj.CommissionBy.ToLower().Equals("percentage"))
                                    {
                                        #region Percentage

                                        var el1 = byDetails.ElementAtOrDefault(0);
                                        if (el1 != null)
                                        {
                                            tbCPFrom1.Text = el1.From.GetValueOrDefault(0).ToString("N2");
                                            tbCPTo1.Text = el1.ToX.GetValueOrDefault(0).ToString("N2");
                                            tbCPValue1.Text = el1.ValueX.GetValueOrDefault(0).ToString("N2");
                                        }

                                        var el2 = byDetails.ElementAtOrDefault(1);
                                        if (el2 != null)
                                        {
                                            tbCPFrom2.Text = el2.From.GetValueOrDefault(0).ToString("N2");
                                            tbCPTo2.Text = el2.ToX.GetValueOrDefault(0).ToString("N2");
                                            tbCPValue2.Text = el2.ValueX.GetValueOrDefault(0).ToString("N2");
                                        }

                                        var el3 = byDetails.ElementAtOrDefault(2);
                                        if (el3 != null)
                                        {
                                            tbCPFrom3.Text = el3.From.GetValueOrDefault(0).ToString("N2");
                                            tbCPTo3.Text = el3.ToX.GetValueOrDefault(0).ToString("N2");
                                            tbCPValue3.Text = el3.ValueX.GetValueOrDefault(0).ToString("N2");
                                        }

                                        var el4 = byDetails.ElementAtOrDefault(3);
                                        if (el4 != null)
                                        {
                                            tbCPFrom4.Text = el4.From.GetValueOrDefault(0).ToString("N2");
                                            tbCPTo4.Text = el4.ToX.GetValueOrDefault(0).ToString("N2");
                                            tbCPValue4.Text = el4.ValueX.GetValueOrDefault(0).ToString("N2");
                                        }

                                        var el5 = byDetails.ElementAtOrDefault(4);
                                        if (el5 != null)
                                        {
                                            tbCPFrom5.Text = el5.From.GetValueOrDefault(0).ToString("N2");
                                            tbCPTo5.Text = el5.ToX.GetValueOrDefault(0).ToString("N2");
                                            tbCPValue5.Text = el5.ValueX.GetValueOrDefault(0).ToString("N2");
                                        }

                                        #endregion
                                    }
                                    else if (obj.CommissionBy.ToLower().Equals("quantity"))
                                    {
                                        #region Quantity

                                        var el1 = byDetails.ElementAtOrDefault(0);
                                        if (el1 != null)
                                        {
                                            tbCQFrom1.Text = el1.From.GetValueOrDefault(0).ToString("N2");
                                            tbCQTo1.Text = el1.ToX.GetValueOrDefault(0).ToString("N2");
                                            tbCQValue1.Text = el1.ValueX.GetValueOrDefault(0).ToString("N2");
                                        }

                                        var el2 = byDetails.ElementAtOrDefault(1);
                                        if (el2 != null)
                                        {
                                            tbCQFrom2.Text = el2.From.GetValueOrDefault(0).ToString("N2");
                                            tbCQTo2.Text = el2.ToX.GetValueOrDefault(0).ToString("N2");
                                            tbCQValue2.Text = el2.ValueX.GetValueOrDefault(0).ToString("N2");
                                        }

                                        var el3 = byDetails.ElementAtOrDefault(2);
                                        if (el3 != null)
                                        {
                                            tbCQFrom3.Text = el3.From.GetValueOrDefault(0).ToString("N2");
                                            tbCQTo3.Text = el3.ToX.GetValueOrDefault(0).ToString("N2");
                                            tbCQValue3.Text = el3.ValueX.GetValueOrDefault(0).ToString("N2");
                                        }

                                        var el4 = byDetails.ElementAtOrDefault(3);
                                        if (el4 != null)
                                        {
                                            tbCQFrom4.Text = el4.From.GetValueOrDefault(0).ToString("N2");
                                            tbCQTo4.Text = el4.ToX.GetValueOrDefault(0).ToString("N2");
                                            tbCQValue4.Text = el4.ValueX.GetValueOrDefault(0).ToString("N2");
                                        }

                                        var el5 = byDetails.ElementAtOrDefault(4);
                                        if (el5 != null)
                                        {
                                            tbCQFrom5.Text = el5.From.GetValueOrDefault(0).ToString("N2");
                                            tbCQTo5.Text = el5.ToX.GetValueOrDefault(0).ToString("N2");
                                            tbCQValue5.Text = el5.ValueX.GetValueOrDefault(0).ToString("N2");
                                        }

                                        #endregion
                                    }
                                }

                                #endregion
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterHiddenField("rbCommissionBy", "Percentage");

                            #region Enable Input Percentage if iselected

                            tbCQFrom1.Enabled = false;
                            tbCQFrom2.Enabled = false;
                            tbCQFrom3.Enabled = false;
                            tbCQFrom4.Enabled = false;
                            tbCQFrom5.Enabled = false;

                            tbCQTo1.Enabled = false;
                            tbCQTo2.Enabled = false;
                            tbCQTo3.Enabled = false;
                            tbCQTo4.Enabled = false;
                            tbCQTo5.Enabled = false;

                            tbCQValue1.Enabled = false;
                            tbCQValue2.Enabled = false;
                            tbCQValue3.Enabled = false;
                            tbCQValue4.Enabled = false;
                            tbCQValue5.Enabled = false;

                            tbCQFrom1.Text = "";
                            tbCQFrom2.Text = "";
                            tbCQFrom3.Text = "";
                            tbCQFrom4.Text = "";
                            tbCQFrom5.Text = "";

                            tbCQTo1.Text = "";
                            tbCQTo2.Text = "";
                            tbCQTo3.Text = "";
                            tbCQTo4.Text = "";
                            tbCQTo5.Text = "";

                            tbCQValue1.Text = "";
                            tbCQValue2.Text = "";
                            tbCQValue3.Text = "";
                            tbCQValue4.Text = "";
                            tbCQValue5.Text = "";

                            #endregion

                            #region Disable Input for Amount

                            //tbCAFrom1.Enabled = true;
                            //tbCAFrom2.Enabled = true;
                            //tbCAFrom3.Enabled = true;
                            //tbCAFrom4.Enabled = true;
                            //tbCAFrom5.Enabled = true;

                            //tbCATo1.Enabled = true;
                            //tbCATo2.Enabled = true;
                            //tbCATo3.Enabled = true;
                            //tbCATo4.Enabled = true;
                            //tbCATo5.Enabled = true;

                            //tbCAValue1.Enabled = true;
                            //tbCAValue2.Enabled = true;
                            //tbCAValue3.Enabled = true;
                            //tbCAValue4.Enabled = true;
                            //tbCAValue5.Enabled = true;

                            #endregion

                            forDetails = new List<CommissionDetFor>();

                            ViewState["forDetails"] = forDetails;

                            BindForGrid();
                        }
                    }

                    #endregion
                }
                else
                {
                    PanelList.Visible = true;
                    PanelInput.Visible = false;
                }
            }
            else
            {
                forDetails = (List<CommissionDetFor>)ViewState["forDetails"];
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = @"SELECT ItemNo + ' - ' + ItemName as ItemName, ItemNo FROM Item WHERE CategoryName = @CategoryName";
            QueryCommand qc = new QueryCommand(sql);
            qc.AddParameter("@CategoryName", ddlCategory.SelectedValue);

            DataSet ds = DataService.GetDataSet(qc);
            ddlItem.DataSource = ds;
            ddlItem.DataTextField = "ItemName";
            ddlItem.DataValueField = "ItemNo";
            ddlItem.DataBind();

            ddlItem.Items.Insert(0, new ListItem("Select One", ""));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (!int.TryParse(Request.QueryString["id"], out id))
            {
                Response.Redirect("NewCommission.aspx");
                return;
            }

            if (DoSave(id))
            {
                Response.Redirect("NewCommission.aspx");
                return;
            }
        }

        protected void btnSaveNew_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (!int.TryParse(Request.QueryString["id"], out id))
            {
                Response.Redirect("NewCommission.aspx");
                return;
            }

            if (DoSave(id))
            {
                Response.Redirect("NewCommission.aspx?id=0");
                return;
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewCommission.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (!int.TryParse(Request.QueryString["id"], out id))
            {
                Response.Redirect("NewCommission.aspx");
                return;
            }

            PowerPOS.CommissionHdr obj = null;

            if (id > 0)
                obj = new PowerPOS.CommissionHdr(id);
            else
                obj = new PowerPOS.CommissionHdr();

            if (obj != null)
            {
                QueryCommandCollection qcc = new QueryCommandCollection();

                QueryCommand qc1 = new QueryCommand("DELETE CommissionDetFor WHERE CommissionHdrID = @HdrID");
                qc1.AddParameter("@HdrID", obj.CommissionHdrID);
                qcc.Add(qc1);

                QueryCommand qc2 = new QueryCommand("DELETE CommissionDetBy WHERE CommissionHdrID = @HdrID");
                qc2.AddParameter("@HdrID", obj.CommissionHdrID);
                qcc.Add(qc2);


                QueryCommand qc3 = new QueryCommand("DELETE CommissionHdr WHERE CommissionHdrID = @HdrID");
                qc3.AddParameter("@HdrID", obj.CommissionHdrID);
                qcc.Add(qc3);

                DataService.ExecuteTransaction(qcc);

                Response.Redirect("NewCommission.aspx");
                return;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewCommission.aspx?id=0");
        }

        protected void btnAddCommissionFor_Click(object sender, EventArgs e)
        {
            forDetails = (List<CommissionDetFor>) ViewState["forDetails"];

            CommissionDetFor d = new CommissionDetFor();
            d.CategoryName = ddlCategory.SelectedValue;
            d.ItemNo = ddlItem.SelectedValue;

            forDetails.Add(d);

            ViewState["forDetails"] = forDetails;

            BindForGrid();
        }

        protected void gvDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            forDetails = (List<CommissionDetFor>)ViewState["forDetails"];

            forDetails.RemoveAt(e.RowIndex);

            ViewState["forDetails"] = forDetails;

            BindForGrid();
        }

        protected void rbCommissionBy_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCommissionByPercentage.Checked)
            {
                #region Percentage

                tbCPFrom1.Enabled = true;
                tbCPFrom2.Enabled = true;
                tbCPFrom3.Enabled = true;
                tbCPFrom4.Enabled = true;
                tbCPFrom5.Enabled = true;

                tbCPTo1.Enabled = true;
                tbCPTo2.Enabled = true;
                tbCPTo3.Enabled = true;
                tbCPTo4.Enabled = true;
                tbCPTo5.Enabled = true;

                tbCPValue1.Enabled = true;
                tbCPValue2.Enabled = true;
                tbCPValue3.Enabled = true;
                tbCPValue4.Enabled = true;
                tbCPValue5.Enabled = true;

                #endregion

                #region Quantity

                tbCQFrom1.Enabled = false;
                tbCQFrom2.Enabled = false;
                tbCQFrom3.Enabled = false;
                tbCQFrom4.Enabled = false;
                tbCQFrom5.Enabled = false;

                tbCQTo1.Enabled = false;
                tbCQTo2.Enabled = false;
                tbCQTo3.Enabled = false;
                tbCQTo4.Enabled = false;
                tbCQTo5.Enabled = false;

                tbCQValue1.Enabled = false;
                tbCQValue2.Enabled = false;
                tbCQValue3.Enabled = false;
                tbCQValue4.Enabled = false;
                tbCQValue5.Enabled = false;

                tbCQFrom1.Text = "";
                tbCQFrom2.Text = "";
                tbCQFrom3.Text = "";
                tbCQFrom4.Text = "";
                tbCQFrom5.Text = "";

                tbCQTo1.Text = "";
                tbCQTo2.Text = "";
                tbCQTo3.Text = "";
                tbCQTo4.Text = "";
                tbCQTo5.Text = "";

                tbCQValue1.Text = "";
                tbCQValue2.Text = "";
                tbCQValue3.Text = "";
                tbCQValue4.Text = "";
                tbCQValue5.Text = "";

                #endregion
            }

            if (rbCommissionByQuantity.Checked)
            {
                #region Percentage

                tbCPFrom1.Enabled = false;
                tbCPFrom2.Enabled = false;
                tbCPFrom3.Enabled = false;
                tbCPFrom4.Enabled = false;
                tbCPFrom5.Enabled = false;

                tbCPTo1.Enabled = false;
                tbCPTo2.Enabled = false;
                tbCPTo3.Enabled = false;
                tbCPTo4.Enabled = false;
                tbCPTo5.Enabled = false;

                tbCPValue1.Enabled = false;
                tbCPValue2.Enabled = false;
                tbCPValue3.Enabled = false;
                tbCPValue4.Enabled = false;
                tbCPValue5.Enabled = false;

                tbCPFrom1.Text = "";
                tbCPFrom2.Text = "";
                tbCPFrom3.Text = "";
                tbCPFrom4.Text = "";
                tbCPFrom5.Text = "";

                tbCPTo1.Text = "";
                tbCPTo2.Text = "";
                tbCPTo3.Text = "";
                tbCPTo4.Text = "";
                tbCPTo5.Text = "";

                tbCPValue1.Text = "";
                tbCPValue2.Text = "";
                tbCPValue3.Text = "";
                tbCPValue4.Text = "";
                tbCPValue5.Text = "";

                #endregion

                #region Quantity

                tbCQFrom1.Enabled = true;
                tbCQFrom2.Enabled = true;
                tbCQFrom3.Enabled = true;
                tbCQFrom4.Enabled = true;
                tbCQFrom5.Enabled = true;

                tbCQTo1.Enabled = true;
                tbCQTo2.Enabled = true;
                tbCQTo3.Enabled = true;
                tbCQTo4.Enabled = true;
                tbCQTo5.Enabled = true;

                tbCQValue1.Enabled = true;
                tbCQValue2.Enabled = true;
                tbCQValue3.Enabled = true;
                tbCQValue4.Enabled = true;
                tbCQValue5.Enabled = true;

                #endregion
            }
        }

        #endregion

        #region Methods

        protected void BindGrid()
        {
            string sql = @"
                SELECT
                    CommissionHdrID
                    , SchemeName
                    , UG.GroupName as SalesGroupName
                FROM CommissionHdr CO
                LEFT JOIN UserGroup UG ON UG.GroupId = CO.SalesGroupID
            ";

            DataSet ds = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS"));
            DataTable dt = ds.Tables[0];

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void BindForGrid()
        {
            forDetails = (List<CommissionDetFor>) ViewState["forDetails"];

            gvDetail.DataSource = forDetails;
            gvDetail.DataBind();
        }

        private bool DoSave(int id)
        {
            PowerPOS.CommissionHdr obj = null;

            if (id > 0)
                obj = new PowerPOS.CommissionHdr(id);
            else
                obj = new PowerPOS.CommissionHdr();

            if (obj != null)
            {
                decimal valD = 0m;
                int valI = 0;

                decimal valFrom = 0;
                decimal valTo = 0;
                decimal valValue = 0;


                obj.SchemeName = tbSchemeName.Text;

                obj.IsProduct = cbProduct.Checked ? 1 : 0;
                if (cbProduct.Checked && decimal.TryParse(tbProduct.Text, out valD))
                    obj.ProductWeight = valD;
                else
                    obj.ProductWeight = null;
                obj.IsService = cbService.Checked ? 1 : 0;
                if (cbService.Checked && decimal.TryParse(tbService.Text, out valD))
                    obj.ServiceWeight = valD;
                else
                    obj.ServiceWeight = null;
                obj.IsPointSold = cbPointSold.Checked ? 1 : 0;
                if (cbPointSold.Checked && decimal.TryParse(tbPointSold.Text, out valD))
                    obj.PointSoldWeight = valD;
                else
                    obj.PointSoldWeight = null;
                obj.IsPackageSold = cbPackageSold.Checked ? 1 : 0;
                if (cbPackageSold.Checked && decimal.TryParse(tbPackageSold.Text, out valD))
                    obj.PackageSoldWeight = valD;
                else
                    obj.PackageSoldWeight = null;
                obj.IsPointRedeem = cbPointRedeem.Checked ? 1 : 0;
                if (cbPointRedeem.Checked && decimal.TryParse(tbPointRedeem.Text, out valD))
                    obj.PointRedeemWeight = valD;
                else
                    obj.PointRedeemWeight = null;
                obj.IsPackageRedeem = cbPackageRedeem.Checked ? 1 : 0;
                if (cbPackageRedeem.Checked && decimal.TryParse(tbPackageRedeem.Text, out valD))
                    obj.PackageRedeemWeight = valD;
                else
                    obj.PackageRedeemWeight = null;

                obj.IsDeductionFor2ndSalesPerson = cbDeduction.Checked ? 1 : 0;
                if (cbDeduction.Checked)
                {
                    if (rbDeductionByPercentage.Checked)
                    {
                        obj.DeductionBy = "Percentage";
                        if (decimal.TryParse(tbDeductionByPercentage.Text, out valD))
                            obj.DeductionValue = valD;
                        else
                            obj.DeductionValue = 0m;
                    }
                    else if (rbDeductionByAmount.Checked)
                    {
                        obj.DeductionBy = "Amount";
                        if (decimal.TryParse(tbDeductionByAmount.Text, out valD))
                            obj.DeductionValue = valD;
                        else
                            obj.DeductionValue = 0m;
                    }
                }
                else
                {
                    obj.DeductionBy = "";
                    obj.DeductionValue = null;
                }


                if (int.TryParse(ddlSalesGroup.SelectedValue, out valI))
                    obj.SalesGroupID = valI;

                obj.CommissionBy = rbCommissionByPercentage.Checked ? "Percentage" : (rbCommissionByQuantity.Checked ? "Quantity" : null);

                obj.Save();


                QueryCommand qc1 = new QueryCommand("DELETE CommissionDetFor WHERE CommissionHdrID = @HdrID");
                qc1.AddParameter("@HdrID", obj.CommissionHdrID);
                DataService.ExecuteQuery(qc1);

                for (int i = 0; i < forDetails.Count; i++)
                {
                    CommissionDetFor d = forDetails[i];

                    CommissionDetFor a = new CommissionDetFor();
                    a.CommissionHdrID = obj.CommissionHdrID;
                    a.CategoryName = d.CategoryName;
                    a.ItemNo = d.ItemNo;
                    a.Save();
                }

                if (rbCommissionByPercentage.Checked)
                {
                    QueryCommand qc2 = new QueryCommand("DELETE CommissionDetBy WHERE CommissionHdrID = @HdrID");
                    qc2.AddParameter("@HdrID", obj.CommissionHdrID);
                    DataService.ExecuteQuery(qc2);

                    if (decimal.TryParse(tbCPFrom1.Text, out valFrom) && decimal.TryParse(tbCPTo1.Text, out valTo) && decimal.TryParse(tbCPValue1.Text, out valValue))
                    {
                        CommissionDetBy a = new CommissionDetBy();
                        a.CommissionHdrID = obj.CommissionHdrID;
                        a.From = valFrom;
                        a.ToX = valTo;
                        a.ValueX = valValue;
                        a.Save();
                    }

                    if (decimal.TryParse(tbCPFrom2.Text, out valFrom) && decimal.TryParse(tbCPTo2.Text, out valTo) && decimal.TryParse(tbCPValue2.Text, out valValue))
                    {
                        CommissionDetBy a = new CommissionDetBy();
                        a.CommissionHdrID = obj.CommissionHdrID;
                        a.From = valFrom;
                        a.ToX = valTo;
                        a.ValueX = valValue;
                        a.Save();
                    }

                    if (decimal.TryParse(tbCPFrom3.Text, out valFrom) && decimal.TryParse(tbCPTo3.Text, out valTo) && decimal.TryParse(tbCPValue3.Text, out valValue))
                    {
                        CommissionDetBy a = new CommissionDetBy();
                        a.CommissionHdrID = obj.CommissionHdrID;
                        a.From = valFrom;
                        a.ToX = valTo;
                        a.ValueX = valValue;
                        a.Save();
                    }

                    if (decimal.TryParse(tbCPFrom4.Text, out valFrom) && decimal.TryParse(tbCPTo4.Text, out valTo) && decimal.TryParse(tbCPValue4.Text, out valValue))
                    {
                        CommissionDetBy a = new CommissionDetBy();
                        a.CommissionHdrID = obj.CommissionHdrID;
                        a.From = valFrom;
                        a.ToX = valTo;
                        a.ValueX = valValue;
                        a.Save();
                    }

                    if (decimal.TryParse(tbCPFrom5.Text, out valFrom) && decimal.TryParse(tbCPTo5.Text, out valTo) && decimal.TryParse(tbCPValue5.Text, out valValue))
                    {
                        CommissionDetBy a = new CommissionDetBy();
                        a.CommissionHdrID = obj.CommissionHdrID;
                        a.From = valFrom;
                        a.ToX = valTo;
                        a.ValueX = valValue;
                        a.Save();
                    }
                }
                else if (rbCommissionByQuantity.Checked)
                {
                    QueryCommand qc2 = new QueryCommand("DELETE CommissionDetBy WHERE CommissionHdrID = @HdrID");
                    qc2.AddParameter("@HdrID", obj.CommissionHdrID);
                    DataService.ExecuteQuery(qc2);

                    if (decimal.TryParse(tbCQFrom1.Text, out valFrom) && decimal.TryParse(tbCQTo1.Text, out valTo) && decimal.TryParse(tbCQValue1.Text, out valValue))
                    {
                        CommissionDetBy a = new CommissionDetBy();
                        a.CommissionHdrID = obj.CommissionHdrID;
                        a.From = valFrom;
                        a.ToX = valTo;
                        a.ValueX = valValue;
                        a.Save();
                    }

                    if (decimal.TryParse(tbCQFrom2.Text, out valFrom) && decimal.TryParse(tbCQTo2.Text, out valTo) && decimal.TryParse(tbCQValue2.Text, out valValue))
                    {
                        CommissionDetBy a = new CommissionDetBy();
                        a.CommissionHdrID = obj.CommissionHdrID;
                        a.From = valFrom;
                        a.ToX = valTo;
                        a.ValueX = valValue;
                        a.Save();
                    }

                    if (decimal.TryParse(tbCQFrom3.Text, out valFrom) && decimal.TryParse(tbCQTo3.Text, out valTo) && decimal.TryParse(tbCQValue3.Text, out valValue))
                    {
                        CommissionDetBy a = new CommissionDetBy();
                        a.CommissionHdrID = obj.CommissionHdrID;
                        a.From = valFrom;
                        a.ToX = valTo;
                        a.ValueX = valValue;
                        a.Save();
                    }

                    if (decimal.TryParse(tbCQFrom4.Text, out valFrom) && decimal.TryParse(tbCQTo4.Text, out valTo) && decimal.TryParse(tbCQValue4.Text, out valValue))
                    {
                        CommissionDetBy a = new CommissionDetBy();
                        a.CommissionHdrID = obj.CommissionHdrID;
                        a.From = valFrom;
                        a.ToX = valTo;
                        a.ValueX = valValue;
                        a.Save();
                    }

                    if (decimal.TryParse(tbCQFrom5.Text, out valFrom) && decimal.TryParse(tbCQTo5.Text, out valTo) && decimal.TryParse(tbCQValue5.Text, out valValue))
                    {
                        CommissionDetBy a = new CommissionDetBy();
                        a.CommissionHdrID = obj.CommissionHdrID;
                        a.From = valFrom;
                        a.ToX = valTo;
                        a.ValueX = valValue;
                        a.Save();
                    }
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}
