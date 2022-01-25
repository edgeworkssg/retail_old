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

public partial class POSGroupMap : PageBase
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
            gvDetails.DataSource = SpecialEventController.ViewLocationByEvent(int.Parse(ddEvent.SelectedValue));
            gvDetails.DataBind();
        }
        else
        {
            gvDetails.DataSource = null;
            gvDetails.DataBind();
        }
    }    

    protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        EventLocationMap.Delete(EventLocationMap.Columns.EventLocationMapID,
                                int.Parse(gvDetails.DataKeys[e.RowIndex].Value.ToString()));
        
        BindGrid();
        e.Cancel = true;
    }
    protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string status;

        SpecialEventController.InsertEventLocationMapping(int.Parse(ddEvent.SelectedValue),
            int.Parse(ddlName.SelectedValue.ToString()), 
            out status);

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

    }
    #endregion


    protected void btnOk_Click(object sender, EventArgs e)
    {
        //Press ok
        string status;
        SpecialEventController.InsertEventLocationMapping
            (int.Parse(ddEvent.SelectedValue), 
            int.Parse(ddlName.SelectedValue), 
            out status);

        BindGrid();
    }
    protected void ddEvent_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvDetails.EditIndex = -1;
       BindGrid();
    }    
}
