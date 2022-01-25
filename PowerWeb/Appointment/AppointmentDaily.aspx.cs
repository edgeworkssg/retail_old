using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using PowerPOS;
using SubSonic;
using System.Drawing;
using System.Globalization;

namespace PowerWeb.Appointment
{
    public partial class AppointmentDaily : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isCreateAppot = false;
            var table = (DataTable)Session["privileges"];
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (row["privilegename"].ToString().ToLower() == PrivilegesController.CREATE_APPOINTMENT.ToLower() || row["privilegename"].ToString().ToLower() == PrivilegesController.VIEW_APPOINTMENT.ToLower())
                    {
                        isCreateAppot = true;
                    }
                }
            }

            if (!isCreateAppot)
            {
                UCAppCalendarDaily1.Visible = false;
            }
            else
            {
                UCAppCalendarDaily1.Visible = true;
            }
        }
    }
}
