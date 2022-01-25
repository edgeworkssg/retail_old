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
using PowerPOS;
using SubSonic.Utilities;
public partial class EditPoints : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string status;
        if (Session["UserName"] == null || Session["Role"] == null || 
            (string)Session["UserName"] == "" || (string)Session["Role"] == "")
        {
            Response.Redirect("../login.aspx");
        }
        if (Request.QueryString["id"] != null)
        {
            string id = Utility.GetParameter("id");
            string pointID;
            if (Request.QueryString["pointid"] != null && 
                (pointID = Utility.GetParameter("pointid")) != "")
            {
                //string pointID = Utility.GetParameter("pointid");

                try
                {
                    PowerPOS.Membership member
                        = new PowerPOS.Membership
                        (PowerPOS.Membership.Columns.MembershipNo, id);

                    PowerPOS.Item loadedItem
                        = new PowerPOS.Item
                        (PowerPOS.Item.Columns.ItemNo, pointID);

                    MembershipPoint loadedPoint = member.GetPoints(pointID)[0];

                    lblItemName.ToolTip = loadedItem.ItemNo;
                    lblItemName.Text = loadedItem.ItemName;
                    if (loadedPoint.PackageType == Item.PointMode.Dollar)
                        lblCurrentPoint.Text = member.GetPoints(pointID)[0].Points.ToString("N2");
                    else
                        lblCurrentPoint.Text = member.GetPoints(pointID)[0].Points.ToString("N0");
                    lblMembershipNo.Text = member.MembershipNo;
                    lblNameToAppear.Text = member.NameToAppear;
                    btnAdjust.Enabled = true;

                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
            }
        }
    }
    protected void btnAdjust_Click(object sender, EventArgs e)
    {
        try
        {
            string status;
            decimal points;
            if (!decimal.TryParse(txtPoints.Text, out points))
            {
                CommonWebUILib.ShowMessage
                 (lblError, "Invalid points. Please specify a decimal number",
                 CommonWebUILib.MessageType.BadNews);
                return;
            }

            DateTime startPeriod, endPeriod;

            PowerPOS.Membership member = new PowerPOS.Membership(lblMembershipNo.Text);
            member.AdjustPoint(lblItemName.ToolTip, points, (string)Session["UserName"]);

            CommonWebUILib.ShowMessage(lblError, "Update Successful.", CommonWebUILib.MessageType.GoodNews);
        }
        catch (Exception X)
        {
            CommonWebUILib.ShowMessage(lblError, "Failed. " + X.Message, CommonWebUILib.MessageType.BadNews);
        }
    }
}
