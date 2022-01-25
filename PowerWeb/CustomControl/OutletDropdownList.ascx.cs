using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;

namespace SRL.UserControls
{
    public partial class OutletDropdownList : System.Web.UI.UserControl
    {
        protected bool IsUseAllBreakdownPOS = true;
        protected bool IsUseAllBreakdownOutlet = true;

        protected string SelectedPOS = "";
        protected string SelectedOutlet = "";

        public void SetLabelPOS(string value)
        {
            this.lblPOS.Text = value;
        }

        public void SetLabelOutlet(string value)
        {
            this.lblOutlet.Text = value;
        }


        public void setIsUseAllBreakdownPOS(bool value)
        {
            this.IsUseAllBreakdownPOS = value;
        }

        public void setIsUseAllBreakdownOutlet(bool value)
        {
            this.IsUseAllBreakdownOutlet = value;
        }

        public string GetDdlPOSSelectedItemText
        {
            get { return this.ddlPOS.SelectedItem  != null ? this.ddlPOS.SelectedItem.Text : "ALL"; }
        }

        public string GetDdlOutletSelectedItemText
        {
            get { return  this.ddlOutlet.SelectedItem != null ? this.ddlOutlet.SelectedItem.Text : "ALL"; }
        }

        public string GetDdlPOSSelectedValue
        {
            get { return this.ddlPOS.SelectedValue; }
        }

        public string GetDdlOutletSelectedValue
        {
            get { return this.ddlOutlet.SelectedValue; }
        }

        public void SetDdlPOSSelectedValue(string value)
        {
            this.ddlPOS.SelectedValue = value;
        }

        public void SetDdlOutletSelectedValue(string value)
        {
            this.ddlOutlet.SelectedValue = value;
        }

        public void SetDdlPOSSelectedValueFirstTime(string value)
        {
            this.SelectedPOS = value;
        }

        public void SetDdlOutletSelectedValueFirstTime(string value)
        {
            this.SelectedOutlet = value;
        }

        public object GetDdlPOSSelectedItem
        {
            get { return this.ddlPOS.SelectedItem; }
        }

        public object GetDdlOutletSelectedItem
        {
            get { return this.ddlOutlet.SelectedItem; }
        }

        public void ResetDdlPOS()
        {
            this.ddlPOS.SelectedIndex = 0;
        }

        public void ResetDdlOutlet()
        {
            this.ddlOutlet.SelectedIndex = 0;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDropDown();

                if(!string.IsNullOrEmpty(this.SelectedPOS))
                    this.SetDdlPOSSelectedValue(this.SelectedPOS);

                if (!string.IsNullOrEmpty(this.SelectedOutlet))
                    this.SetDdlOutletSelectedValue(this.SelectedOutlet);
            }
        }

        protected void FillDropDown()
        {
            ddlOutlet.Items.Clear();
            ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(IsUseAllBreakdownOutlet, Session["UserName"] + ""));
            //ddlOutlet.DataSource = OutletController.FetchByUserNameForReport(IsUseAllBreakdownOutlet, true, (Session["UserName"] + ""));
            ddlOutlet.DataBind();
            ddlOutlet.SelectedIndex = 0;
            ddlOutlet_OnSelectedIndexChanged(ddlOutlet, new EventArgs());
        }

        protected void ddlOutlet_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPOS.Items.Clear();
            string selectedOutlet = GetDdlOutletSelectedItemText;
            if (selectedOutlet.ToLower() == "all" || selectedOutlet.ToLower() == "all - breakdown")
                selectedOutlet = "ALL";

            ddlPOS.Items.AddRange(PointOfSaleController.FetchPointOfSaleNames(IsUseAllBreakdownPOS, Session["UserName"] + "", selectedOutlet));
            ddlPOS.DataBind();
        }

        protected void ddlOutlet_OnInit(object sender, EventArgs e)
        {
            try
            {
                ddlOutlet.Items.Clear();
                ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(IsUseAllBreakdownOutlet, Session["UserName"] + ""));
                ddlOutlet.DataBind();
                ddlOutlet.SelectedIndex = 0;
                ddlOutlet_OnSelectedIndexChanged(ddlOutlet, new EventArgs());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
    }
}