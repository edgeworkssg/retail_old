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
public partial class Order_OrderDetail : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string status;
        if (Session["UserName"] == null || Session["Role"] == null || (string)Session["UserName"] == "" || (string)Session["Role"] == "")
        {
            Response.Redirect("../login.aspx");
        }

        if (Request.QueryString["id"] != null)
        {
            string id = Utility.GetParameter("id");

            POSController pos = new POSController(id);

            if (pos.GetSavedRefNo() != "")
            {
                DataTable dt = pos.FetchUnSavedOrderItems(out status);

                lblOrderDate.Text = pos.GetOrderDate().ToString("dd MMM yyyy HH:mm:ss");
                //lblOrderNo.Text = pos.GetSavedRefNo();
                lblOrderNo.Text = pos.GetUnsavedCustomRefNo();
                lblCashier.Text = pos.GetCashierID();
                lblSalesPerson.Text = pos.GetSalesPerson();

                //pos.LoadMembership();
                PowerPOS.Membership member = pos.GetMemberInfo();
                if (member != null && member.IsLoaded)
                {
                    lblMembershipNo.Text = "<a href='../Membership/MembershipDetail.aspx?id=" + member.MembershipNo + "' target=_blank>" + member.MembershipNo + "</a>";
                    lblMembershipName.Text = member.NameToAppear;
                    lblMembershipGroup.Text = member.MembershipGroup.GroupName;
                }

                bool isHideSalesPerson2 = true;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SalesPerson2"].ToString() != "")
                        isHideSalesPerson2 = false;
                }

                gvOrder.DataSource = dt;
                gvOrder.DataBind();

                if (isHideSalesPerson2)
                    gvOrder.Columns[12].Visible = false;
                else
                    gvOrder.Columns[12].Visible = true;

                //decimal GrossAmt = pos.GetGrossAmount();
                decimal GrossAmt = pos.GetGrossAmountByUnitPrice();
                decimal TotalAmt = pos.CalculateTotalAmount(out status);
                lblGrossSales.Text = GrossAmt.ToString("N2");
                lblGSTTotal.Text = pos.GetGSTAmount().ToString("N2");
                lblTotalDiscount.Text = pos.myOrderHdr.DiscountAmount.ToString("N2");
                lblTotal.Text = TotalAmt.ToString("N2");

                gvReceipt.DataSource = pos.FetchUnsavedReceipt();
                gvReceipt.DataBind();

                lblRemarks.Text = "Remark: " + pos.GetRemarks().Replace("\r","<BR/>");
                /*
                DataTable dat = pos.FetchCostOfGoods();
                gvCostOfGoods.DataSource = dat;
                gvCostOfGoods.DataBind();*/
            }
        }
    }
    protected void gvOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[3].Text = String.Format("{0:N2}", decimal.Parse(e.Row.Cells[3].Text));
            e.Row.Cells[6].Text = String.Format("{0:N2}", decimal.Parse(e.Row.Cells[6].Text));
            for (int i = 7; i < 11; i++)
            {
                if (bool.Parse(e.Row.Cells[i].Text))
                {
                    e.Row.Cells[i].Text = "Yes";
                }
                else
                {
                    e.Row.Cells[i].Text = "No";
                }
            }
        }
    }
    protected void gvReceipt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Text = String.Format("{0:N2}", decimal.Parse(e.Row.Cells[1].Text));
        }

    }
    protected void gvCostOfGoods_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            /*
            decimal cogs = 0.0M;
            decimal.TryParse(e.Row.Cells[2].Text, out cogs);
            e.Row.Cells[2].Text = String.Format("{0:N2}", cogs);
            decimal total = int.Parse(e.Row.Cells[3].Text) * cogs;
            e.Row.Cells[4].Text = String.Format("{0:N2}", total);*/
        }
    }
}
