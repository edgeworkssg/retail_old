using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SubSonic;
using SubSonic.Utilities;
using PowerPOS;

namespace PowerWeb.Product
{
    public partial class Attributes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /// If status = View, Hide panelEdit, Show panelView
            /// If status = Insert, Hide Gridview2 and Show cmdSaveSetMeal
            /// If status = Edit, Show Gridview2 and Hide cmdSaveSetMeal
            ErrBox.Visible = false;
            if (!Page.IsPostBack)
            {
                string id = Utility.GetParameter("id");

                if (string.IsNullOrEmpty(id))
                {
                    ToggleEditor(false);
                }
                else
                {
                    Item myItem = new Item(id);
                    ToggleEditor(!myItem.IsNew);

                    if (!myItem.IsNew)
                    {
                        tItemNo.Text = myItem.ItemNo;
                        tItemName.Text = myItem.ItemName;
                    }
                }
            }
        }

        /// <summary>
        /// Shows/Hides the Grid and Editor panels
        /// </summary>
        /// <param name="showIt"></param>
        void ToggleEditor(bool showIt)
        {
            pnlEdit.Visible = showIt;
            pnlGrid.Visible = !showIt;
        }

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "NoDataInsert")
                {
                    string vUserID = (string)Session["DisplayName"];
                    ObjectDataSource1.InsertParameters.Add("UserID", TypeCode.String, vUserID);
                    ObjectDataSource1.InsertParameters.Add("AttributesGroupCode", TypeCode.String, (((DropDownList)GridView2.Controls[0].Controls[0].FindControl("newAttributesGroup")).SelectedValue == null) ? "" : ((DropDownList)GridView2.Controls[0].Controls[0].FindControl("newAttributesGroup")).SelectedValue);

                    ObjectDataSource1.Insert();
                }
                else if (e.CommandName == "InsertNew")
                {
                    string vUserID = (string)Session["DisplayName"];
                    ObjectDataSource1.InsertParameters.Add("UserID", TypeCode.String, vUserID);
                    ObjectDataSource1.InsertParameters.Add("AttributesGroupCode", TypeCode.String, (((DropDownList)GridView2.FooterRow.FindControl("insertAttributesGroup")).SelectedValue == null) ? "" : ((DropDownList)GridView2.FooterRow.FindControl("insertAttributesGroup")).SelectedValue);

                    ObjectDataSource1.Insert();
                }
            }
            catch (Exception X)
            {
                tError.Text = X.InnerException.Message.Replace("(warning)", "");
                ErrBox.Visible = true;
            }
        }
        //protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    SubSetMealDS.DeleteParameters.Add("ItemNo", TypeCode.String, ((Literal)GridView2.Rows[e.RowIndex].FindControl("viewItemNo")).Text);
        //}
        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductMaster.aspx");
        }
        protected void cmdList_Click(object sender, EventArgs e)
        {
            Response.Redirect("SetMeal.aspx");
        }
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ObjectDataSource1.DeleteParameters.Add("AttributesGroupCode", TypeCode.String, ((Literal)GridView2.Rows[e.RowIndex].FindControl("viewAttGroupCode")).Text);
        }
    }

    public class ODSAttributesGroupController
    {
        public List<AttributesGroup> FetchAll()
        {
            return new AttributesGroupController().FetchAll().Where(o => o.Deleted == false).ToList();
        }
    }
}
