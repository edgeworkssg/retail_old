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
using System.IO;

namespace PowerWeb.Bill
{
    public partial class BillList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DeleteGeneratedFile(); 
        }

        private void DeleteGeneratedFile()
        {
            foreach (string file in Directory.GetFiles(Server.MapPath("~/Exports"), "*.xls*"))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception) { }
            }
        }
    }
}
