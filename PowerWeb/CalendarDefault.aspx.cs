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
using Mediachase.AjaxCalendar;
using System.Text;
using System.Globalization;

public partial class CalendarDefault : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			BindData();
		}
	}

	protected void Page_Init(object sender, EventArgs e)
	{
		this.ddSkinType.SelectedIndexChanged += new EventHandler(ddSkinType_SelectedIndexChanged);
	}

	void ddSkinType_SelectedIndexChanged(object sender, EventArgs e)
	{
		switch (ddSkinType.SelectedValue)
		{
			case "Google":
				gCalendar.Visible = true;
				oCalendar.Visible = false;
				Session["Calendar_SkinType"] = "Google";
				break;
			case "Outlook":
				gCalendar.Visible = false;
				oCalendar.Visible = true;
				Session["Calendar_SkinType"] = "Outlook";
				break;
			default:
				break;
		}
		//BindData();
		Response.Redirect("~/CalendarDefault.aspx");
		//InitCalendar();
	}
    
	private void BindData()
	{
		if (Session["Calendar_SkinType"] != null &&
			Session["Calendar_SkinType"].ToString() == "Google")
		{
			gCalendar.Visible = true;
			oCalendar.Visible = false;
			ddSkinType.SelectedValue = "Google";
		}
		if (Session["Calendar_SkinType"] != null &&
			Session["Calendar_SkinType"].ToString() == "Outlook")
		{
			gCalendar.Visible = false;
			oCalendar.Visible = true;
			ddSkinType.SelectedValue = "Outlook";
		}
		InitCalendar();
	}

	private void  InitCalendar()
	{

		MediachaseAjaxCalendar cal = null;

		if (Session["Calendar_ViewMode"] != null)
		{
			cal = gCalendar.FindControl("gCalendar") as MediachaseAjaxCalendar;
			if(cal!=null)
				cal.ViewMode = (Mediachase.AjaxCalendar.ViewModeType)Session["Calendar_ViewMode"];
			cal = oCalendar.FindControl("oCalendar") as MediachaseAjaxCalendar;
			if (cal != null)
				cal.ViewMode = (Mediachase.AjaxCalendar.ViewModeType)Session["Calendar_ViewMode"];
		}
		if (Session["Calendar_ViewType"] != null)
		{
			cal = gCalendar.FindControl("gCalendar") as MediachaseAjaxCalendar;
			if (cal != null)
				cal.ViewType = (CalendarViewType)Session["Calendar_ViewType"];
			cal = oCalendar.FindControl("oCalendar") as MediachaseAjaxCalendar;
			if (cal != null)
				cal.ViewType = (CalendarViewType)Session["Calendar_ViewType"];
		}
		if (Session["Calendar_SelectedDate"] != null)
		{
			cal = gCalendar.FindControl("gCalendar") as MediachaseAjaxCalendar;
			if (cal != null)
				cal.SelectedDate = (DateTime)Session["Calendar_SelectedDate"];
			cal = oCalendar.FindControl("oCalendar") as MediachaseAjaxCalendar;
			if (cal != null)
				cal.SelectedDate = (DateTime)Session["Calendar_SelectedDate"];
		}
        
		cal = gCalendar.FindControl("gCalendar") as MediachaseAjaxCalendar;
		if (cal != null)
			cal.Refresh();
		cal = oCalendar.FindControl("oCalendar") as MediachaseAjaxCalendar;
		if (cal != null)
			cal.Refresh();
	}

	public static string GetAppPath()
	{
		StringBuilder builder = new StringBuilder();
		builder.Append(HttpContext.Current.Request.Url.Scheme);
		builder.Append("://");
		builder.Append(HttpContext.Current.Request.Url.Authority);
		builder.Append(HttpContext.Current.Request.ApplicationPath);
		//builder.Append("/");
		return builder.ToString();
	}

    protected void drpAdminTasks_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (((DropDownList)sender).SelectedIndex)
        {
            case 1:
                Response.Redirect("EditorPages/CreateRooms.aspx");
                break;
            case 2:
                Response.Redirect("EditorPages/CreateBuilding.aspx");
                break;
            case 3:
                Response.Redirect("EditorPages/AttendanceSheet.aspx");
                break;
        }
    }
}
