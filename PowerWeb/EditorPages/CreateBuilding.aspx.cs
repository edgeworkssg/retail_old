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
namespace WebSample.EditorPages
{
    public partial class CreateBuilding : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                this.Session["GridState"] = "New";
                BindData();
                lbError.Visible = false;
            }
            
        }

        private void BindData()
        {
            BuildingController bdc = new BuildingController();
            this.Session["grdBuilding"] = bdc.FetchAll();
            grdBuilding.DataSource = (BuildingCollection)this.Session["grdBuilding"];
            grdBuilding.DataBind();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            lbBack.Click += new EventHandler(lbBack_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            //    btnDelete.Click += new EventHandler(btnDelete_Click);
            // ddBuilding.SelectedIndexChanged += new EventHandler(ddBuilding_SelectedIndexChanged);


        }
        void btnSave_Click(object sender, EventArgs e)
        {
            //if (!ValidatePage())//page is not valid
            //{
            //    lbError.Visible = true;
            //    return;
            //}
            lbError.Visible = false;
            try
            {
                if (this.Session["GridState"] == null)
                    this.Session["GridState"] = "New";
                if (this.Session["GridState"].ToString() == "New")
                {
                    Building rm = new Building();
                    rm.BuildingName = tbBuildingName.Text.Trim();
                    rm.AddressLine1 = tbAddrLine1.Text.Trim();
                    rm.AddressLine2 = tbAddrLine2.Text.Trim();
                    rm.City = tbCity.Text.Trim();
                    if (ddCountry.SelectedIndex != -1)
                        rm.Country = ddCountry.SelectedItem.Text;
                    rm.PinCode = Convert.ToInt32(tbPinCode.Text);
                    BuildingController.CreateBuilding(rm);
                    this.Session["GridState"] = "New";
                }
                else
                {
                    BuildingController bm = new BuildingController();
                    string strCountry = "";
                    if (ddCountry.SelectedIndex != -1)
                        strCountry = ddCountry.SelectedItem.Text;
                    bm.Update(tbBuildingName.Text.Trim(), tbCity.Text.Trim(), strCountry,
                         tbAddrLine1.Text.Trim(), tbAddrLine2.Text.Trim(), Convert.ToInt32(tbPinCode.Text));
                    this.Session["GridState"] = "New";
                }
                BindData();
            }
            catch (Exception ex)
            {
                lbError.Text = ex.Message.ToString();
                lbError.Visible = true;
            }
//            Response.Redirect("~/Default.aspx");
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void grdBuilding_RowEditing(object sender, GridViewEditEventArgs e)
        {
            BuildingCollection bd = (BuildingCollection)this.Session["grdBuilding"];
            tbBuildingName.Text = bd[e.NewEditIndex].BuildingName;
            tbAddrLine2.Text = bd[e.NewEditIndex].AddressLine2;
            tbAddrLine1.Text = bd[e.NewEditIndex].AddressLine1.ToString();
            tbPinCode.Text = bd[e.NewEditIndex].PinCode.ToString();
            tbCity.Text = bd[e.NewEditIndex].City;
            if(bd[e.NewEditIndex].Country != "")
            ddCountry.SelectedItem.Text = bd[e.NewEditIndex].Country;
            this.Session["GridState"] = "Edit";
            
        }
    }
}
