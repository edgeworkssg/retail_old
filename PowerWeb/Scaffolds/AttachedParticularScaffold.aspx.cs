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
using SubSonic;
using SubSonic.Utilities;
using PowerPOS;

namespace PowerPOS
{
    public partial class AttachedParticularScaffold : PageBase
    {
        private bool isAdd = false;
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        public string MembershipNo;

        protected void Page_Load(object sender, EventArgs e)
        {
            MembershipNo = Request.QueryString["membershipno"];
            if (string.IsNullOrEmpty(MembershipNo))
            {
                pnlGrid.Visible = false;
                pnlEdit.Visible = false;
                return;
            }
            else
            {
                Membership mbr = new Membership(MembershipNo);
                if (mbr == null || string.IsNullOrEmpty(mbr.MembershipNo))
                {
                    lblMessage.Text = LanguageManager.GetTranslation("Membership No not found.");
                    pnlGrid.Visible = false;
                    pnlEdit.Visible = false;
                    return;
                }

                lblMembershipNo.Text = MembershipNo;
                Page.Title = LanguageManager.GetTranslation("Attached Particular for Member")+" " + MembershipNo;
            }

            if (Request.QueryString["id"] != null)
            {
                string id = Utility.GetParameter("id");
                if (!String.IsNullOrEmpty(id) && id != "0")
                {
                    if (!Page.IsPostBack)
                    {
                        LoadEditor(id);
                    }
                }
                else
                {
                    //it's an add, show the editor
                    isAdd = true;
                    ToggleEditor(true);
                    LoadDrops();
                    btnDelete.Visible = false;
                }
            }
            else
            {
                ToggleEditor(false);
                if (!Page.IsPostBack)
                {
                    BindGrid(String.Empty);
                }
            }
        }
        /// <summary>
        /// Loads the editor with data
        /// </summary>
        /// <param name="id"></param>
        void LoadEditor(string id)
        {
            ToggleEditor(true);
            LoadDrops();
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                lblID.Text = id.ToString();

                //pull the record
                AttachedParticular item = new AttachedParticular(id);
                //bind the page 

                ctrlFirstName.Text = item.FirstName;
                ctrlLastName.Text = item.LastName;
                ctrlChristianName.Text = item.ChristianName;
                ctrlChineseName.Text = item.ChineseName;
                ctrlGender.Text = item.Gender;
                ctrlOccupation.Text = item.Occupation;
                if (item.DateOfBirth.HasValue)
                {
                    ctrlDateOfBirth.Text = item.DateOfBirth.Value.ToString("dd MMM yyyy");
                }

                //set the delete confirmation
                btnDelete.Attributes.Add("onclick", "return CheckDelete();");
            }
        }
        /// <summary>
        /// Loads the DropDownLists
        /// </summary>
        void LoadDrops()
        {
            //load the listboxes

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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            LoadEditor("0");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            AttachedParticular.Delete(Utility.GetParameter("id"));
            //redirect
            Response.Redirect(Request.CurrentExecutionFilePath + "?membershipno=" + MembershipNo);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id = Utility.GetParameter("id");
            //bool haveError = false;
            try
            {
                BindAndSave(id);
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Attached Particular saved.") + "</span>";
            }
            catch (Exception x)
            {
                //haveError = true;
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Attached Particular not saved:") + "</span> " + x.Message;
            }
            //if(!haveError)
            //  Response.Redirect(Request.CurrentExecutionFilePath);
        }
        /// <summary>
        /// Binds and saves the data
        /// </summary>
        /// <param name="id"></param>
        void BindAndSave(string id)
        {
            try
            {
                AttachedParticular item;
                if (!String.IsNullOrEmpty(id) && id != "0")
                {
                    //it's an edit
                    item = new AttachedParticular(id);
                }
                else
                {
                    //add
                    item = new AttachedParticular();
                    item.Deleted = false;
                    item.UniqueID = Guid.NewGuid();
                }

                item.MembershipNo = lblMembershipNo.Text;
                item.FirstName = ctrlFirstName.Text;
                item.LastName = ctrlLastName.Text;
                item.ChristianName = ctrlChristianName.Text;
                item.ChineseName = ctrlChineseName.Text;
                item.Gender = ctrlGender.SelectedItem.Text;
                item.Occupation = ctrlOccupation.Text;

                DateTime dateOfBirth;
                if (DateTime.TryParse(ctrlDateOfBirth.Text, out dateOfBirth)) item.DateOfBirth = dateOfBirth;

                //bind it
                item.Save(Session["username"].ToString());
            }
            catch (Exception e)
            {
                lblResult.Text = "Error occured: " + e.Message;
                Logger.writeLog(e);
            }
        }
        /// <summary>
        /// Binds the GridView
        /// </summary>
        private void BindGrid(string orderBy)
        {
            DataTable myAP = new DataTable();

            //string SQL = "select a.MembershipNo, b.GroupName, a.NameToAppear, a.LastName, "
            //+ "a.SalesPersonID, a.Gender, a.DateOfBirth, a.Email, a.NRIC, a.Occupation, "
            // + " a.StreetName, a.StreetName2, a.ZipCode, a.City, a.Country, a.Mobile, a.Office, a.Home,"
            //+ "a.ExpiryDate, a.Remarks, a.userfld1, a.userfld2, a.userfld3, a.userfld4, a.userfld5, a.userfld6, a.userfld7 " +
            //"from membership a inner join MembershipGroup b on a.MembershipGroupId = b.MembershipGroupId"
            //+ " where a.deleted = 0";
            /*
                        string SQL = "select a.MembershipNo, b.GroupName, a.NameToAppear, a.LastName, "
                       + "a.SalesPersonID, a.DateOfBirth, a.Email, a.NRIC, "
                        + " a.StreetName, a.StreetName2, a.ZipCode, a.City, a.Country, a.Mobile, a.Office, a.Home,"
                       + "a.ExpiryDate, a.Remarks, a.userfld1, a.userfld3, a.userfld5, a.userfld7 " +
                                   "from membership a inner join MembershipGroup b on a.MembershipGroupId = b.MembershipGroupId";
              */

            AttachedParticularCollection apColl = new AttachedParticularCollection();
            apColl.Where("MembershipNo", MembershipNo);
            apColl.Load();
            myAP = apColl.ToDataTable();

            string sortColumn = null;
            if (!String.IsNullOrEmpty(orderBy))
            {
                ViewState.Add(ORDER_BY, sortColumn);
                if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
                {
                    //myMember.OrderByAsc(orderBy);
                    ViewState[SORT_DIRECTION] = SqlFragment.ASC;
                }

                else
                {
                    //myMember.OrderByDesc(orderBy);
                    ViewState[SORT_DIRECTION] = SqlFragment.DESC;
                }
            }
            else if (ViewState[ORDER_BY] != null)
            {
                sortColumn = (string)ViewState[ORDER_BY];
                //myMember.OrderByAsc(orderBy);
                ViewState.Add(ORDER_BY, sortColumn);
                if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
                {
                    //myMember.OrderByAsc(sortColumn);
                    ViewState[SORT_DIRECTION] = SqlFragment.ASC;
                }

                else
                {
                    // myMember.OrderByDesc(sortColumn);
                    ViewState[SORT_DIRECTION] = SqlFragment.DESC;
                }
            }

            //GridView1.DataSource = myMember.ToDataTable();

            GridView1.DataSource = myAP;

            GridView1.DataBind();
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGrid(String.Empty);
        }

        protected void GridView1_DataBound(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = GridView1.BottomPagerRow;
            if (gvrPager == null)
            {
                return;
            }
            // get your controls from the gridview
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");
            if (ddlPages != null)
            {
                // populate pager
                for (int i = 0; i < GridView1.PageCount; i++)
                {
                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());
                    if (i == GridView1.PageIndex)
                    {
                        lstItem.Selected = true;
                    }
                    ddlPages.Items.Add(lstItem);
                }
            }
            int itemCount = 0;
            // populate page count
            if (lblPageCount != null)
            {
                //pull the datasource
                DataSet ds = GridView1.DataSource as DataSet;
                if (ds != null)
                {
                    itemCount = ds.Tables[0].Rows.Count;
                }
                string pageCount = "<b>" + GridView1.PageCount.ToString() + "</b> (" + itemCount.ToString() + " Items)";
                lblPageCount.Text = pageCount;
            }
            Button btnPrev = (Button)gvrPager.Cells[0].FindControl("btnPrev");
            Button btnNext = (Button)gvrPager.Cells[0].FindControl("btnNext");
            Button btnFirst = (Button)gvrPager.Cells[0].FindControl("btnFirst");
            Button btnLast = (Button)gvrPager.Cells[0].FindControl("btnLast");
            //now figure out what page we're on
            if (GridView1.PageIndex == 0)
            {
                btnPrev.Enabled = false;
                btnFirst.Enabled = false;
            }
            else if (GridView1.PageIndex + 1 == GridView1.PageCount)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
            else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
                btnPrev.Enabled = true;
                btnFirst.Enabled = true;
            }
        }

        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = GridView1.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            GridView1.PageIndex = ddlPages.SelectedIndex;
            // a method to populate your grid
            BindGrid(String.Empty);
        }
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            string columnName = e.SortExpression;
            //rebind the grid
            if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
            {
                ViewState[SORT_DIRECTION] = SqlFragment.DESC;
            }
            else
            {
                ViewState[SORT_DIRECTION] = SqlFragment.ASC;
            }
            BindGrid(columnName);
        }

        string GetSortDirection(string sortBy)
        {
            string sortDir = " ASC";
            if (ViewState["sortBy"] != null)
            {
                string sortedBy = ViewState["sortBy"].ToString();
                if (sortedBy == sortBy)
                {
                    //the direction should be desc
                    sortDir = " DESC";
                    //reset the sorter to null
                    ViewState["sortBy"] = null;
                }
                else
                {
                    //this is the first sort for this row
                    //put it to the ViewState
                    ViewState["sortBy"] = sortBy;
                }
            }
            else
            {
                //it's null, so this is the first sort
                ViewState["sortBy"] = sortBy;
            }
            return sortDir;
        }
    }
}
