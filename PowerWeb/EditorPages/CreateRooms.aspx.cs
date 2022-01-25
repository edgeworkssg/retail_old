using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Mediachase.AjaxCalendar;
//using MCAjaxCalendar;
using PowerPOS;
namespace WebSample
{
    public partial class CreateRooms : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BuildingController rm = new BuildingController();
                ddBuilding.DataSource = rm.FetchAll();
                ddBuilding.DataTextField = "BuildingName";
                ddBuilding.DataValueField = "BuildingName";
                ddBuilding.DataBind();
                BindData();
                
            }
        }
        private void BindData()
        {
            RoomController rmc = new RoomController();
            //grdRooms.DataSource = rmc.FetchAll();
            //grdRooms.DataBind();
            this.Session["grdRooms"] = rmc.FetchAll();
            grdRooms.DataSource = (RoomCollection)this.Session["grdRooms"];
            grdRooms.DataBind();
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            lbBack.Click += new EventHandler(lbBack_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
        //    btnDelete.Click += new EventHandler(btnDelete_Click);
           // ddBuilding.SelectedIndexChanged += new EventHandler(ddBuilding_SelectedIndexChanged);
            

        }
        void btnSave_Click(object sender, EventArgs e)
        {
            //if (!ValidatePage())//page is not valid
            //{
            //    lbError.Visible = true;
            //    return;
            //}
            lbError.Visible = false;
            try
            {
                if (this.Session["GridState"] == null)
                    this.Session["GridState"] = "New";
                if (this.Session["GridState"].ToString() == "New")
                {
                    Room rm = new Room();
                    //rm.BuildingName = ddBuilding.SelectedItem.Text.Trim();
                    //rm.Floor = tbFloor.Text.Trim();
                    rm.RoomName = tbRoomName.Text.Trim();
                    RoomController.CreateRoom(rm);
                }
                else
                {
                    RoomController bm = new RoomController();
                    string strbuilding = "";
                    if (ddBuilding.SelectedIndex != -1)
                        strbuilding = ddBuilding.SelectedItem.Text;
                    //bm.Update(Convert.ToInt32(lblRoomID.Text),strbuilding, tbRoomName.Text.Trim(), tbFloor.Text);
                    this.Session["GridState"] = "New";
                }
                BindData();
            }
            catch (Exception ex)
            {
                lbError.Text = ex.Message.ToString();
                lbError.Visible = true;
            }
           // Response.Redirect("~/Default.aspx");
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

     

        protected void grdRooms_RowEditing(object sender, GridViewEditEventArgs e)
        {
            RoomCollection bd = (RoomCollection)this.Session["grdRooms"];
            //lblRoomID.Text = bd[e.NewEditIndex].Id.ToString();
            tbRoomName.Text = bd[e.NewEditIndex].RoomName;
            //tbFloor.Text = bd[e.NewEditIndex].Floor;
            //if (bd[e.NewEditIndex].BuildingName != "")
            //    ddBuilding.SelectedItem.Text = bd[e.NewEditIndex].BuildingName;
            this.Session["GridState"] = "Edit";
        }
    }
}
