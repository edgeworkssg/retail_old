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
using PowerPOS;
namespace WebSample.EditorPages
{
    public partial class AttendanceSheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CourseController css = new CourseController();
                //CourseCollection cc =  css.FetchAll();
                lblCourseName.DataSource = css.FetchAll();
                lblCourseName.DataTextField = "CourseName";
                lblCourseName.DataValueField = "ID";
                lblCourseName.DataBind();


                //MembershipController mc = new MembershipController();
                //MembershipCollection mcol = new MembershipCollection();
                //DataTable dt = new DataTable();
                //dt.Columns.Add("Member ID");
                //dt.Columns.Add("Member Name");
                //dt.Columns.Add("Day 1");
                ////string[] members = cc[0].Members.Split(new char[] { ',' });
                ////foreach (string s in members)
                ////{
                ////    DataRow dr = dt.NewRow();
                ////    PowerPOS.Membership member = (mc.FetchByID(s))[0];
                ////    dr[0] =  member.MemberId;
                ////    dr[1] = member.FirstName + " " + member.LastName;
                ////    dt.Rows.Add(dr);
                ////}
                //this.Session["grdAttendance"] = dt;
                //BindData();
            }

        }

        public void BindData()
        {
            grdAttendance.DataSource = (DataTable)this.Session["grdAttendance"];
            grdAttendance.DataBind();

        }
        public int EventId
        {
            get
            {
                if (Request["EventId"] != null)
                {
                    return int.Parse(Request["EventId"].ToString());
                }
                return -1;
            }
        }

        protected void grdAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CheckBox chkBox;
            PlaceHolder ph;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for(int i = 2; i < e.Row.Cells.Count;i++)
                {
                    chkBox = new CheckBox();
                    chkBox.ID = "cbSelectAll";
                    ph = new PlaceHolder();
                    ph.ID = "RowPlaceholder";
                    ph.Controls.Add(chkBox);
                    e.Row.Cells[i].Controls.Add(ph);
                }
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                chkBox = new CheckBox();
                chkBox.Text = "Day One";
                chkBox.ID = "cbSelectAll";
                ph = new PlaceHolder();
                ph.ID = "RowPlaceholder";
                ph.Controls.Add(chkBox);
                e.Row.Cells[2].Controls.Add(ph);
                //Find the checkbox control in header and add an attribute
                ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAll('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
               // ((CheckBox)e.Row.FindControl("cbSelectAll1")).Attributes.Add("onclick", "javascript:SelectAll('" + ((CheckBox)e.Row.FindControl("cbSelectAll1")).ClientID + "')");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            AttendanceSheetController attContr = new AttendanceSheetController();
            if (tbAddrLine1.Text != "")
            {
                /*
                attContr.Create
                    (Convert.ToInt32(tbAddrLine1.Text), Convert.ToInt32(lblCourseName.SelectedValue), 
                    Convert.ToDateTime(txtAttendDate.Text), "Leave", "Leave");*/
            }
        }             
    }
}
