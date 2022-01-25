using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PowerWeb
{
    public partial class exportToExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Report"] != null)
            {
                DataTable dt = ((DataTable)Session["Report"]);
                Page.Title = dt.TableName;
                CommonWebUILib.ExportCSVAllColumns(dt, this.Page.Title.Trim(' '), dt.TableName);
            }
        }
    }
}
