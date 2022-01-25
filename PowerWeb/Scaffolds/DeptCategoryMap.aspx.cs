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

public partial class DeptCategoryMap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDown1.SelectedIndex = 0;
        }        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DropDown1.SelectedIndex != 0)
        {
            ManyManyList1.PrimaryKeyValue = DropDown1.SelectedValue;
            ManyManyList1.Save();
        }
    }
    protected void DropDown1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDown1.SelectedIndex != 0)
        {
            ManyManyList1.Items.Clear();
            ManyManyList1.PrimaryKeyValue = DropDown1.SelectedValue;
            ManyManyList1.DataBind();
        }
    }
}
