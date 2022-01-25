using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SubSonic.Utilities;
using PowerPOS;
using SubSonic;
using System.Data;

namespace PowerWeb.SalesPerson
{
    public partial class EditLineSalesPerson : System.Web.UI.Page
    {
        string id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddSalesPerson.DataSource = FetchSalesPersonNames();
                ddSalesPerson.DataBind();

         
               if (Session["UserName"] == null || Session["Role"] == null || 
                   (string)Session["UserName"] == "" || (string)Session["Role"] == "")
               {
                    Response.Redirect("../login.aspx");
               }
               string tmp = Page.Request.FilePath;
               tmp = tmp.Substring(tmp.LastIndexOf('/') + 1);
               if (tmp != "NotAuthorized.aspx" && tmp != "home.aspx")
               {
                   if (!PrivilegesController.IsPageInPrivilege(tmp, (DataTable)Session["privileges"]))
                   {
                       lblMessage.Text = "You dont have permission to change sales person. Please ask contact your administrator";
                       btnSave.Enabled = false;
                            
                   }
               }
         
            }
            if (Request.QueryString["id"] != null)
            {
                id = Utility.GetParameter("id");

            }
            else
            {
                btnSave.Enabled = false;
            }            
        }
        public static ListItem[] FetchSalesPersonNames()
        {
            ListItemCollection ar = new ListItemCollection();
            ListItem[] lrList;
            ListItem lr = new ListItem();
            lr.Text = "--Please Select--";
            lr.Value = "";
            ar.Add(lr);

            UserMstCollection col = new UserMstCollection();
            col.Where(UserMst.Columns.IsASalesPerson, true);
            col.Where(UserMst.Columns.Deleted, false);
            col.Load();
            
            for (int i = 0 ; i < col.Count ; i++)
            {
                lr = new ListItem();
                lr.Text = col[i].DisplayName;
                lr.Value = col[i].UserName;
                ar.Add(lr);
            }

            lrList = new ListItem[ar.Count];
            for (int i = 0; i < ar.Count; i++)
            {
                lrList[i] = ar[i];
            }
            return lrList;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddSalesPerson.SelectedIndex > 0)
            {
                OrderDet det = new OrderDet(id);
                Logger.writeLog("Attempt to change sales person for orderdet = '" + id + "' by user '" + Session["username"] + "'. Previous value = '" + det.SalesPerson + "'");
                string SQL = "Update OrderDet set userfld1 = '" + ddSalesPerson.SelectedValue + "' where orderdetid = '" + id + "'";
                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                DataService.ExecuteQuery(cmd);
                lblMessage.Text = "Operation completed. Please close this window and refresh the previous page";
            }
        }

    }
}
