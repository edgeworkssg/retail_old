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

namespace PowerWeb
{
    public partial class redemptionPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["member"] != null)
                {
                    PowerPOS.Membership p = 
                        (PowerPOS.Membership)Session["member"];
                    
                    if (p.IsLoaded && !p.IsNew)
                    {
                        lblMembershipNo.Text = p.MembershipNo;
                        lblMembershipName.Text = p.NameToAppear;                        
                        BindGrid();
                    }
                    else
                    {
                        Response.Redirect("customerLogin.aspx");
                    }
                }
                else
                {
                    Response.Redirect("customerLogin.aspx");
                }                
            }
        }
        private void BindGrid()
        {
            string status;
            PowerPOS.Membership p = 
                        (PowerPOS.Membership)Session["member"];

            decimal points = 
                MembershipController.GetCurrentPoint
                (p.MembershipNo, DateTime.Now, out status);
            lblPoints.Text = points.ToString();
            DateTime startDate, endDate;
            startDate = DateTime.Today.AddYears(-1);
            endDate = DateTime.Today;
            RedemptionItemController v = new RedemptionItemController();

            GridView1.DataSource = v.FetchValidRedemptionItems(DateTime.Now, 0, 0);
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int i = 0;
            //do redemption code here
            string status;
            int rowindex;
            if (txtDeliveryAddress.Text == "" || txtContactNo.Text == "")
            {
                CommonWebUILib.ShowMessage(lblMsg, "Please specify delivery address and contact number", CommonWebUILib.MessageType.BadNews);
                return;
            }
            if (int.TryParse(e.CommandArgument.ToString(), out rowindex))
            {
                PowerPOS.MembershipController mbr = new MembershipController();
                if (mbr.redeemPoint
                    (int.Parse(GridView1.DataKeys[rowindex].Value.ToString()),
                    lblMembershipNo.Text, DateTime.Now, "System", txtDeliveryAddress.Text,
                    txtContactNo.Text, out status))
                {
                    CommonWebUILib.ShowMessage(lblMsg, status, CommonWebUILib.MessageType.BadNews);
                }
                else
                {
                    CommonWebUILib.ShowMessage(lblMsg, "Redemption successful. We will contact you regarding your redemption delivery", CommonWebUILib.MessageType.GoodNews);
                    BindGrid();
                }
            }
        }
    }
}
