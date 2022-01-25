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
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;
using System.Text;

public partial class QuickAccessButtonForm : PageBase
{
    string CatID;
    int row, col, update;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Get information from Request 
        if (Session["UserName"] == null || Session["Role"] == null || (string)Session["UserName"] == "" || (string)Session["Role"] == "")
        {
            Response.Redirect("../login.aspx");
        }
        if (Request.QueryString["update"] != null)
        {

            if (!int.TryParse(Utility.GetParameter("update"), out update))
            {
                CommonWebUILib.ShowMessage(lblResult,
                    "Error:Query string parameter not well formed",
                    CommonWebUILib.MessageType.BadNews);
                return;
            }
        }
        if (Request.QueryString["CatId"] != null)
        {
            CatID = Utility.GetParameter("CatId");
            //ViewState["CatID"] = CatID;
        }
        if (Request.QueryString["row"] != null)
        {
            if (!int.TryParse(Utility.GetParameter("row"), out row))
            {
                CommonWebUILib.ShowMessage(lblResult,
                    "Querystring parameter not well formed",
                    CommonWebUILib.MessageType.BadNews);
                return;
            }
            //ViewState["row"] = row;
        }
        if (Request.QueryString["col"] != null)
        {
            if (!int.TryParse(Utility.GetParameter("col"), out col))
            {
                CommonWebUILib.ShowMessage(lblResult,
                    "Querystring parameter not well formed",
                    CommonWebUILib.MessageType.BadNews);
                return;
            }
            //ViewState["col"] = col;
        }

        //if update is 1, get the item, fore colour and back colour
        if (update == 1 && CatID != "")
        {
            QuickAccessButtonCollection qb = new QuickAccessButtonCollection();
            qb.Where(QuickAccessButton.Columns.QuickAccessCategoryID, new Guid(CatID));
            qb.Where(QuickAccessButton.Columns.Row, row);
            qb.Where(QuickAccessButton.Columns.Col, col);
            qb.Load();

            if (qb.Count > 0 && !qb[0].IsNew)
            {
                if (!Page.IsPostBack)
                {
                    ddItemName.SelectedValue = qb[0].ItemNo;
                    ddForeColor.SelectedValue = qb[0].ForeColor;
                    ddBackColor.SelectedValue = qb[0].BackColor;
                }
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ddItemName.SelectedValue != "")
        {
            if (update == 0)
            {
                QuickAccessButtonController v = new QuickAccessButtonController();
                v.Insert(Guid.NewGuid(), ddItemName.SelectedValue.ToString(),
                    new Guid(CatID), ddForeColor.SelectedValue.ToString(),
                    ddBackColor.SelectedValue.ToString(), "",
                    row, col, 0,
                    null, true, false,
                    DateTime.Now, "", DateTime.Now, "");
            }
            else
            {
                //do update
                Query qr = QuickAccessButton.CreateQuery();

                qr.QueryType = QueryType.Update;

                qr.AddUpdateSetting(QuickAccessButton.Columns.ItemNo,
                    ddItemName.SelectedValue.ToString());

                qr.AddUpdateSetting(QuickAccessButton.Columns.ForeColor,
                    ddForeColor.SelectedValue.ToString());

                qr.AddUpdateSetting(QuickAccessButton.Columns.BackColor,
                    ddBackColor.SelectedValue.ToString());

                qr.AddUpdateSetting(QuickAccessButton.Columns.Deleted,
                    false);

                qr.WHERE(QuickAccessButton.Columns.QuickAccessCategoryID, new Guid(CatID)).
                    AND(QuickAccessButton.Columns.Row, row).
                    AND(QuickAccessButton.Columns.Col, col).
                    Execute();
            }
            CommonWebUILib.ShowMessage(lblResult, "<b>Button Saved</b>", CommonWebUILib.MessageType.GoodNews);
            CloseWindow();
        }
        else
        {
            CommonWebUILib.ShowMessage(lblResult, "<b>Please select Item</b>", CommonWebUILib.MessageType.BadNews);
        }
    }
    private void CloseWindow()
    {
        RegisterStartupScript("load", "<script type=\"text/javascript\">\n" +
            "self.close();\n" +
            "<" + "/script>");
        //StringBuilder sb = new StringBuilder();
        //sb.Append("window.opener.RefreshPage();");
        //sb.Append("window.close();");

        //ClientScript.RegisterClientScriptBlock(
        //    this.GetType(), 
        //    "CloseWindowScript",
        //    sb.ToString(), 
        //    true);    }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Query qr = QuickAccessButton.CreateQuery();
        qr.QueryType = QueryType.Update;
        qr.AddUpdateSetting(QuickAccessButton.Columns.Deleted, true);
        qr.WHERE(QuickAccessButton.Columns.QuickAccessCategoryID, new Guid(CatID)).
                    AND(QuickAccessButton.Columns.Row, row).
                    AND(QuickAccessButton.Columns.Col, col).
                    Execute();
        CommonWebUILib.ShowMessage(lblResult, "<b>Button Deleted</b>", CommonWebUILib.MessageType.GoodNews);
        CloseWindow();
    }
}
