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
using PowerPOS.Container;
using SubSonic;

public partial class AddItemToEvent : PageBase
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Fetch ListItems
            SpecialEventCollection sp = new SpecialEventCollection();
            //sp.Where(SpecialEvent.Columns.StartDate, Comparison.LessOrEquals, DateTime.Now);
            sp.Where(SpecialEvent.Columns.EndDate, Comparison.GreaterOrEquals, DateTime.Now);
            sp.Load();

            if (sp.Count > 0)
            {
                SubSonic.Utilities.Utility.LoadDropDown(ddEvent, sp, "EventName", "EventID", "");

                BindGrid();
            }
            else
            {
                btnOk.Enabled = false;
                CommonWebUILib.ShowMessage(lblErrorMsg, "Please create event first. To create event, click <a href='EventScaffold.aspx'>here</a>", CommonWebUILib.MessageType.BadNews); 
            }
        }
    }
    
    #region "Detail GridView Events"
    private void BindGrid()
    {
        if (ddEvent.SelectedValue != "0")
        {
            gvDetails.DataSource = SpecialEventController.ViewItemByEvent(int.Parse(ddEvent.SelectedValue));
            gvDetails.DataBind();
        }
        else
        {
            gvDetails.DataSource = null;
            gvDetails.DataBind();
        }
    }    
    protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {        
        gvDetails.EditIndex = e.NewEditIndex;
        BindGrid();
        //btnSubmit.Enabled = false;
        
        
        gvDetails.Rows[gvDetails.EditIndex].Cells[3].FindControl("txtPrice").Focus();

    }
    protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        EventItemMap.Delete(EventItemMap.Columns.EventItemMapID,int.Parse(gvDetails.DataKeys[e.RowIndex].Value.ToString()));
        
        BindGrid();
        e.Cancel = true;
    }
    protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string status;

        SpecialEventController.InsertEventItemMapping(int.Parse(ddEvent.SelectedValue),
            ((Label)gvDetails.Rows[gvDetails.EditIndex].Cells[1].FindControl("lblItemNo")).Text,
            decimal.Parse(((TextBox)gvDetails.Rows[gvDetails.EditIndex].Cells[3].FindControl("txtPrice")).Text)
            , out status);

        gvDetails.EditIndex = -1;

        BindGrid();
    }
    protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvDetails.EditIndex = -1;
        BindGrid();
        //btnSubmit.Enabled = true;
    }
    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Decimal amt;
        if (e.Row.RowType == DataControlRowType.DataRow && gvDetails.EditIndex == -1)
        {
            amt = decimal.Parse(((Label)e.Row.Cells[3].FindControl("lblPrice")).Text);
            ((Label)e.Row.Cells[3].FindControl("lblPrice")).Text = String.Format("{0:N2}", amt);
        }
    }
    #endregion


    protected void btnOk_Click(object sender, EventArgs e)
    {
        //Press ok
        string status;
        SpecialEventController.InsertEventItemMapping
            (int.Parse(ddEvent.SelectedValue), 
            ddlName.SelectedValue, 
            decimal.Parse(txtPrice.Text), out status);

        BindGrid();
    }
    protected void ddEvent_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvDetails.EditIndex = -1;
       BindGrid();
    }    
}
