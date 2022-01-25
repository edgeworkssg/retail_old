
//Generated on 5/6/2007 4:12:16 PM by Albert
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
using PowerPOS.Container;
using System.Collections.Generic;

namespace PowerPOS
{
	public partial class MembershipScaffoldNoExport : System.Web.UI.Page 
	{
		private bool isAdd = false;
        static private bool finishAdd = false;
		private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        //additional fields properties
        private char[] chrArr = { ',' };
        //MembershipCustomFieldCollection cr;
        Table tbAdditionalInfo = new Table();
        Control[] ctrl;
        private const int TEXTBOX_SIZE = 200;
        private const int LABEL_SIZE = 120;

        private bool checkAdditionalFieldExist()
        {
            bool exist = false;

            MembershipCustomFieldCollection cr = new MembershipCustomFieldCollection();
            cr.Where(MembershipCustomField.Columns.Deleted, false);
            cr.Load();

            if (cr != null && cr.Count != 0)
                exist = true;

            return exist;
        }

        private void LoadAdditionalControls()
        {
            //loading additional fields collection and control
            MembershipCustomFieldCollection cr = new MembershipCustomFieldCollection();
            cr.Where(MembershipCustomField.Columns.Deleted, false);
            cr.Sort(MembershipCustomField.Columns.FieldName, true);
            cr.Load();

            ctrl = new Control[cr.Count];

            //create dynamic table to contain the information
            //int row = (cr.Count + 1) / 2;

            tbAdditionalInfo.CellSpacing = 5;

            for (int i = 0; i <= (cr.Count-2);)
            {

                TableRow tr = new TableRow();

                //two fields for each row
                for (int j = 0; j < 2 && ((i+j) <= cr.Count); j++)
                {
                    TableCell tc1 = new TableCell();
                    TableCell tc2 = new TableCell();

                    tc1.Width = LABEL_SIZE;
                    tc2.Width = TEXTBOX_SIZE;
                    
                    tc1.Style.Add("text-align", "right");

                    //create label
                    Label lbl = new Label();
                    lbl.Text = cr[i + j].Label;
                    lbl.Font.Bold = true;
                    lbl.Font.Size = 8;
                    lbl.Style.Add("text-align", "right");
                    
                    tc1.Controls.Add(lbl);
                    tr.Cells.Add(tc1);

                    //create input field, next column
                    //find type name
                    //string type = cr[i + j].Type.ToLower().Split(',')[0];
                    switch (cr[i + j].Type.ToLower())
                    {
                        case "string":
                            //create textbox
                            TextBox tb = new TextBox();
                            //add to controls list
                            ctrl[i + j] = new Control();
                            ctrl[i + j] = tb;
                            tc2.Controls.Add(tb); //add to cell
                            tr.Cells.Add(tc2); //add to row
                            break;
                        case "boolean":
                            //create checkbox
                            CheckBox cb = new CheckBox();
                            //add to controls list
                            ctrl[i + j] = new Control();
                            ctrl[i + j] = cb;
                            tc2.Controls.Add(cb); //add to cell
                            tr.Cells.Add(tc2); //add to row
                            break;

                        case "enum":
                            //create combobox (dropdownlist
                            DropDownList cmb = new DropDownList();

                            //populate dropdown list
                            string[] enumItems = cr[i + j].EnumList.Split(chrArr);
                            var list = new List<KeyValuePair<string, string>>();

                            foreach (string item in enumItems)
                                list.Add(new KeyValuePair<string, string>(item, item.ToString()));

                            cmb.DataSource = list;
                            cmb.DataTextField = "Value";
                            cmb.DataValueField = "Key";
                            cmb.DataBind();

                            //add to controls list
                            ctrl[i + j] = new Control();
                            ctrl[i + j] = cmb;
                            tc2.Controls.Add(cmb); //add to cell
                            tr.Cells.Add(tc2); //add to row
                            break;
                        case "date":
                            //create text box to contain date
                            TextBox txtDate = new TextBox();
                            txtDate.ID = "txtDate" + (i+j);
                            txtDate.Width = 121;
                            tc2.Controls.Add(txtDate);

                            ctrl[i + j] = new Control();
                            ctrl[i + j] = txtDate;
                            
                            //create calendar button
                            ImageButton CalendarImageButton = new ImageButton();
                            CalendarImageButton.ID = "CalendarImageButton" + (i + j).ToString();
                            CalendarImageButton.ImageUrl = "~/App_Themes/Default/image/Calendar_scheduleHS.png";
                            
                            //set calendar extender to pop up from image button and targetted to textbox
                            AjaxControlToolkit.CalendarExtender calendarDate = new AjaxControlToolkit.CalendarExtender();
                            calendarDate.ID = "calendarDate" + (i + j).ToString();
                            calendarDate.PopupButtonID = "CalendarImageButton" + (i + j).ToString();
                            CalendarImageButton.OnClientClick = "return false";
                            tc2.Controls.Add(CalendarImageButton);
                            calendarDate.TargetControlID = "txtDate" + (i+j).ToString();
                            calendarDate.Format = "dd MMM yyyy";

                            tc2.Controls.Add(calendarDate); //add to cell
                            tr.Cells.Add(tc2); //add to row
                            break;
                    }


                }
                //add to table
                tbAdditionalInfo.Rows.Add(tr);
                i = i + 2;
            }

            //add table to place holder
            AdditionalInfoHolder.Controls.Add(tbAdditionalInfo);

        }

        private void LoadAdditionalInformation(Membership mbr)
        {
            //loading additional fields collection and control
            MembershipCustomFieldCollection cr = new MembershipCustomFieldCollection();
            cr.Where(MembershipCustomField.Columns.Deleted, false);
            cr.Sort(MembershipCustomField.Columns.FieldName, true);
            cr.Load();

            if (mbr != null && mbr.IsLoaded && cr != null && cr.Count > 0)
            {
                for (int i = 0; i < cr.Count; i++)
                { 
                    //column name in membership table is stored after comma
                    //string field = cr[i].FieldName.Substring(cr[i].FieldName.LastIndexOf(',') + 1);
                    //object value = mbr.GetColumnValue(field);
                    string[] field = cr[i].FieldName.Split(',');
                    //field name column name consist of 4 ((0)index,(1)fieldname,
                    //(2)column name in membership table, and (3)order it is saved in that field of membership table)

                    //get the value from field in membership table
                    string data = (string)mbr.GetColumnValue(field[2]);


                    string value = "";
                    if ( data != "" && data != null)
                    {
                        string[] splitData = data.Split(',');
                        value = splitData[Convert.ToInt32(field[3]) - 1].Substring(splitData[Convert.ToInt32(field[3]) - 1].LastIndexOf(':') + 1);
                        
                        //object value = splitData[Convert.ToInt32(field[3]) - 1].Substring(splitData[Convert.ToInt32(field[3]) - 1].LastIndexOf(':') + 1);
                    }

                    if (value != "" && value != null && ctrl != null)
                    {
                        value = value.Substring(1, value.Length - 2); //stripping the ""
                        switch (cr[i].Type.ToLower())
                        {

                            case "string":
                                //create textbox
                                ((TextBox)ctrl[i]).Text = value;    
                            break;

                            case "boolean":
                                //create checkbox
                                if(value != "False")
                                    ((CheckBox)ctrl[i]).Checked = true;
                                else
                                    ((CheckBox)ctrl[i]).Checked = false;
                          
                                //((CheckBox)ctrl[i]).Checked = (bool)value;
                                break;

                            case "enum":
                                //create combobox               
                                ((DropDownList)ctrl[i]).SelectedValue = value;
                                break;

                            case "date":
                                // date format
                                DateTime tmpDate;
                                if (DateTime.TryParse(value.ToString(), out tmpDate))
                                    ((TextBox)ctrl[i]).Text = tmpDate.ToString("dd MMM yyyy");
                                else
                                    ((TextBox)ctrl[i]).Text = value;
                                break;
                        }
                    }
                }
            }
        }

		protected void Page_Load(object sender, EventArgs e) 
		{
            

			if (Request.QueryString["id"] != null)
			{   
                //load additional controls on page load
                if(checkAdditionalFieldExist())
                    LoadAdditionalControls();

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

                    if(ctrlMembershipNo.Text == "")
                        ctrlMembershipNo.Text = MembershipController.getNewMembershipNo("E");
                    if (!Page.IsPostBack)
                    {
                        txtSubscriptionDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                        ctrlExpiryDate.Text =
                            (new DateTime
                            (DateTime.Now.Year + 1, DateTime.Now.Month,
                            DateTime.DaysInMonth(DateTime.Now.Year+1, DateTime.Now.Month))).
                            ToString("dd MMM yyyy");
                        ctrlDateOfBirth.Text = DateTime.Today.AddYears(-25).ToString("dd MMM yyyy");
                        if (!Page.IsPostBack)
                        {
                            LoadDrops();
                        }
                    }                                                             
					btnDelete.Visible = false;
				}
			}
			else 
			{
				ToggleEditor(false);
				if(!Page.IsPostBack)
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
                Membership item = new Membership(id);
                //bind the page 

                ctrlMembershipNo.Text = item.MembershipNo;
                ctrlMembershipNo.ReadOnly = true; //cannot edit membership no
                ctrlMembershipGroupId.SelectedValue = item.MembershipGroupId.ToString();
                ctrlStylist.SelectedValue = item.SalesPersonID;
                if (item.ExpiryDate.HasValue)
                    ctrlExpiryDate.Text = item.ExpiryDate.Value.ToString("dd MMM yyyy");
                if (item.SubscriptionDate.HasValue)
                {
                    txtSubscriptionDate.Text = item.SubscriptionDate.Value.ToString("dd MMM yyyy");
                }
                else
                {
                    txtSubscriptionDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                }

                ctrlNameToAppear.Text = item.NameToAppear;
                ctrlNRIC.Text = item.Nric;
                ctrlLastName.Text = item.LastName;
                ctrlFirstName.Text = item.FirstName;
                txtChristianName.Text = item.ChristianName;
                txtChineseName.Text = item.ChineseName;
                ctrlGender.Text = item.Gender;
                if (item.DateOfBirth.HasValue)
                {
                    ctrlDateOfBirth.Text = item.DateOfBirth.Value.ToString("dd MMM yyyy");
                }
                ctrlOccupation.Text = item.Occupation;


                ctrlStreetName.Text = item.StreetName;
                ctrlStreetName2.Text = item.StreetName2;
                ctrlZipCode.Text = item.ZipCode;
                ctrlCity.Text = item.City;
                ctrlCountry.Text = item.Country;

                ctrlMobile.Text = item.Mobile;
                ctrlOffice.Text = item.Office;
                ctrlHome.Text = item.Home;
                ctrlEmail.Text = item.Email;

                ctrlRemarks.Text = item.Remarks;

                //CUSTOMER RELATED INFORMATION
                if (item.IsVitaMix.HasValue)
                {
                    cbIsVitaMix.Checked = item.IsVitaMix.Value;
                }
                if (item.IsJuicePlus.HasValue)
                {
                    cbIsJuicePlus.Checked = item.IsJuicePlus.Value;
                }
                if (item.IsWaterFilter.HasValue)
                {
                    cbIsWaterFilter.Checked = item.IsWaterFilter.Value;
                }
                if (item.IsYoung.HasValue)
                {
                    cbIsYoung.Checked = item.IsYoung.Value;
                }

                //load additional information
                if (checkAdditionalFieldExist())
                    LoadAdditionalInformation(item);

                //set the delete confirmation
                btnDelete.Attributes.Add("onclick", "return CheckDelete();");
            }
		}

        private void SaveAdditionalInformation(Membership mbr)
        {
            //loading additional fields collection and control
            MembershipCustomFieldCollection cr = new MembershipCustomFieldCollection();
            cr.Where(MembershipCustomField.Columns.Deleted, false);
            cr.Sort(MembershipCustomField.Columns.FieldName, true);
            cr.Load();

            if (mbr != null && cr != null && cr.Count > 0)
            {
                for (int i = 0; i < cr.Count; i++)
                {
                    //column name in membership table is stored after comma
                    //string field = cr[i].FieldName.Substring(cr[i].FieldName.LastIndexOf(',') + 1);
                    string[] field = cr[i].FieldName.Split(',');  
                    //field name column name consist of 4 ((0)index,(1)fieldname,
                    //(2)column name in membership table, and (3)order it is saved in that field of membership table)
                                
                    //get the value from field in membership table
                    string data = (string)mbr.GetColumnValue(field[2]);

                    //if the data is empty (has never been processed before)
                    if (data == "" || data == null)
                    {
                        for (int j = 0; j < cr.Count; j++)
                        {
                            string[] field2 = cr[j].FieldName.Split(',');
                            if (field2[2] == field[2]) //if it is the same, concatenate to populate the field
                            {
                                string toBeWritten = '"' + field2[1] + '"' + ":" + ",";
                                data = string.Concat(toBeWritten);
                                mbr.SetColumnValue(field2[2], data);
                            }
                        }
                    }

                    //there's value now
                    string[] splitData = data.Split(',');
                    if (splitData != null && ctrl != null)
                    {
                        switch (cr[i].Type.ToLower())
                        {
                            //only change the word where it is stored
                            case "string":
                                //save value from textbox

                                splitData[Convert.ToInt32(field[3]) - 1] = '"' + field[1] + '"' + ":" + '"' + ((TextBox)ctrl[i]).Text + '"';
                                break;

                            case "boolean":
                                //save value from checkbox
                                splitData[Convert.ToInt32(field[3]) - 1] = '"' + field[1] + '"' + ":" + '"' + ((CheckBox)ctrl[i]).Checked + '"';
                                //mbr.SetColumnValue(field, ((CheckBox)ctrl[i]).Checked);
                                break;

                            case "enum":
                                //save value from combobox  
                                splitData[Convert.ToInt32(field[3]) - 1] = '"' + field[1] + '"' + ":" + '"' + ((DropDownList)ctrl[i]).SelectedValue + '"';
                                //mbr.SetColumnValue(field, ((DropDownList)ctrl[i]).SelectedValue);
                                break;

                            case "date":
                                //save value from date textbox    
                                DateTime dateOfBirth;
                                if (DateTime.TryParse(((TextBox)ctrl[i]).Text, out dateOfBirth))
                                    splitData[Convert.ToInt32(field[3]) - 1] = '"' + field[1] + '"' + ":" + '"' + dateOfBirth.ToString("dd MMM yyyy") + '"';
                                    //mbr.SetColumnValue(field, dateOfBirth.ToString("dd MMM yyyy"));
                                break;

                            //case default:
                            //    break;
                        }

                        data = ""; //clear data string
                        //fill in with new value
                        data = string.Join(",", splitData);
                        //write to the column in tmembership table
                        mbr.SetColumnValue(field[2], data);
                    }
                }
            }
        }

		/// <summary>
		/// Loads the DropDownLists
		/// </summary>
		void LoadDrops() 
		{
			//load the listboxes			
			Query qryctrlMembershipGroupId = MembershipGroup.CreateQuery(); 
			qryctrlMembershipGroupId.OrderBy = OrderBy.Asc("GroupName");
			Utility.LoadDropDown(ctrlMembershipGroupId, qryctrlMembershipGroupId.ExecuteReader(), true);

            Query qryusermst = UserMst.CreateQuery();
            qryusermst.AddWhere(UserMst.Columns.IsASalesPerson, true);
            qryusermst.AddWhere(UserMst.Columns.Deleted, false);
            qryusermst.OrderBy = OrderBy.Asc("GroupName");
            Utility.LoadDropDown(ctrlStylist, qryusermst.ExecuteReader(), true);			

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
			Membership.Delete(Utility.GetParameter("id"));
			//redirect
			Response.Redirect(Request.CurrentExecutionFilePath);
		}

		protected void btnSave_Click(object sender, EventArgs e) 
		{
			string id = Utility.GetParameter("id");
			//bool haveError = false;
			try 
			{
				BindAndSave(id);
                //lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">Member saved.</span>";
            }

			 catch (Exception x) 
			 {
				//haveError = true;
				lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">Customer not saved:</span> " + x.Message;
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
                Membership member;
                if (!String.IsNullOrEmpty(id) && id != "0")
                {
                    string duplicate;
                    if (!string.IsNullOrEmpty(ctrlNRIC.Text) && MembershipController.IsNRICAlreadyExist(ctrlNRIC.Text, out duplicate))
                    {
                        if (duplicate != id)
                        {
                            CommonWebUILib.ShowMessage(lblResult, "<b>Error. Duplicate NRIC number with member <a target=_blank href=../Membership/membershipdetail.aspx?id=" + duplicate + ">" + duplicate + "</a></b>", CommonWebUILib.MessageType.BadNews);
                            return;
                        }
                    }
                    //it's an edit
                    member = new Membership(id);
                }
                else
                {
                    string duplicate;
                    if (!string.IsNullOrEmpty(ctrlNRIC.Text) && MembershipController.IsNRICAlreadyExist(ctrlNRIC.Text, out duplicate))
                    {
                        CommonWebUILib.ShowMessage(lblResult, "<b>Error. Duplicate NRIC number with member <a target=_blank href=../Membership/membershipdetail.aspx?id=" + duplicate + ">" + duplicate + "</a></b>", CommonWebUILib.MessageType.BadNews);
                        return;
                    }
                    //add
                    member = new Membership();
                }
                member.MembershipNo = ctrlMembershipNo.Text;
                object valctrlMembershipGroupId = Utility.GetDefaultControlValue(Membership.Schema.GetColumn("MembershipGroupId"), ctrlMembershipGroupId, isAdd, false);
                member.MembershipGroupId = Convert.ToInt32(valctrlMembershipGroupId);
                member.SalesPersonID = ctrlStylist.SelectedValue;
                DateTime expiryDate;
                if (DateTime.TryParse(ctrlExpiryDate.Text, out expiryDate)) member.ExpiryDate = expiryDate;

                DateTime subscriptionDate;
                if (DateTime.TryParse(txtSubscriptionDate.Text, out subscriptionDate)) member.SubscriptionDate = subscriptionDate;

                member.Nric = ctrlNRIC.Text;
                member.NameToAppear = ctrlNameToAppear.Text;
                member.FirstName = ctrlFirstName.Text;
                member.LastName = ctrlLastName.Text;
                member.ChineseName = txtChineseName.Text;
                member.ChristianName = txtChristianName.Text;
                member.Gender = ctrlGender.SelectedItem.Text;

                DateTime dateOfBirth;
                if (DateTime.TryParse(ctrlDateOfBirth.Text, out dateOfBirth)) member.DateOfBirth = dateOfBirth;

                member.Occupation = ctrlOccupation.Text;

                member.StreetName = ctrlStreetName.Text;
                member.StreetName2 = ctrlStreetName2.Text;
                member.ZipCode = ctrlZipCode.Text;
                member.City = ctrlCity.Text;
                member.Country = ctrlCountry.Text;

                member.Mobile = ctrlMobile.Text;
                member.Fax = ctrlOffice.Text;
                member.Home = ctrlHome.Text;
                member.Email = ctrlEmail.Text;

                member.Remarks = ctrlRemarks.Text;

                member.IsVitaMix = cbIsVitaMix.Checked;
                member.IsJuicePlus = cbIsJuicePlus.Checked;
                member.IsWaterFilter = cbIsWaterFilter.Checked;
                member.IsYoung = cbIsYoung.Checked;
                member.Deleted = false;

                //save additional information
                if (checkAdditionalFieldExist())
                    SaveAdditionalInformation(member);

                //bind it
                if (member.IsNew) member.UniqueID = Guid.NewGuid();
                member.Save(Session["username"].ToString());

                if (!String.IsNullOrEmpty(id) && id != "0")
                {
                    lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">Customer info updated</span>";
                }
                else
                {
                    finishAdd = true;

                    Response.Redirect("MembershipScaffold.aspx");
                    //Response.Redirect("MembershipScaffold.aspx?id=" + ctrlMembershipNo.Text); 
                }
                //lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">Customer saved.</span>";
            }

            catch (Exception e)
            {
                string text = e.Message;
            }

		}

        
        const int CHILD1_COL = 20;
        const int CHILD2_COL = 21;
        const int CHILD3_COL = 22;
        const int CHILD4_COL = 23;

        private string CombineName(string childName)
        {
            string[] name = childName.Split(',');

            string fullName = "";
            for(int i=0; i < name.Length; i++)
            {
                string value = "";
                value = name[i].Substring(name[i].LastIndexOf(':') + 1);
                value = value.Substring(1, value.Length - 2);
                fullName += value + " ";
            }
            return fullName;
        }


		/// <summary>
		/// Binds the GridView
		/// </summary>
        private void BindGrid(string orderBy)
        {
            //ViewMembershipCollection myMember = new ViewMembershipCollection();

            DataTable myMember = new DataTable();

            string SQL = "select a.MembershipNo, b.GroupName, a.NameToAppear, a.LastName, "
            + "a.SalesPersonID, a.Gender, a.DateOfBirth, a.Email, a.NRIC, a.Occupation, "
             + " a.StreetName, a.StreetName2, a.ZipCode, a.City, a.Country, a.Mobile, a.Office, a.Home,"
            + "a.ExpiryDate, a.Remarks, a.userfld1, a.userfld3, a.userfld5, a.userfld7 " +
            "from membership a inner join MembershipGroup b on a.MembershipGroupId = b.MembershipGroupId"
            + " where a.deleted = 0";
/*
            string SQL = "select a.MembershipNo, b.GroupName, a.NameToAppear, a.LastName, "
           + "a.SalesPersonID, a.DateOfBirth, a.Email, a.NRIC, "
            + " a.StreetName, a.StreetName2, a.ZipCode, a.City, a.Country, a.Mobile, a.Office, a.Home,"
           + "a.ExpiryDate, a.Remarks, a.userfld1, a.userfld3, a.userfld5, a.userfld7 " +
                       "from membership a inner join MembershipGroup b on a.MembershipGroupId = b.MembershipGroupId";
  */         

            myMember.Load(SubSonic.DataService.GetReader(new QueryCommand(SQL)));

           
            
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

            int colCount = myMember.Rows[0].Table.Columns.Count;

            if (checkAdditionalFieldExist())
            {
                //parsing all rows of child name
                for (int i = 0; i < myMember.Rows.Count; i++)
                {
                    //check if child name value is not empty
                    string child1Name = "";
                    string child2Name = "";
                    string child3Name = "";
                    string child4Name = "";

                    child1Name = myMember.Rows[i][CHILD1_COL].ToString();
                    child2Name = myMember.Rows[i][CHILD2_COL].ToString();
                    child3Name = myMember.Rows[i][CHILD3_COL].ToString();
                    child4Name = myMember.Rows[i][CHILD4_COL].ToString();


                    if (child1Name != "")
                    {
                        child1Name = CombineName(child1Name);
                        myMember.Rows[i].SetField(CHILD1_COL, child1Name);
                    }

                    if (child2Name != "")
                    {
                        child2Name = CombineName(child2Name);
                        myMember.Rows[i].SetField(CHILD2_COL, child2Name);
                    }

                    if (child3Name != "")
                    {
                        child3Name = CombineName(child3Name);
                        myMember.Rows[i].SetField(CHILD3_COL, child3Name);
                    }

                    if (child4Name != "")
                    {
                        child4Name = CombineName(child4Name);
                        myMember.Rows[i].SetField(CHILD4_COL, child4Name);
                    }
                }

                //setting the columns to be visible
                GridView1.Columns[CHILD1_COL + 1].Visible = true;
                GridView1.Columns[CHILD2_COL + 1].Visible = true;
                GridView1.Columns[CHILD3_COL + 1].Visible = true;
                GridView1.Columns[CHILD4_COL + 1].Visible = true;
            }
            
            //GridView1.DataSource = myMember.ToDataTable();

            GridView1.DataSource = myMember;

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

            if (finishAdd)
            {
                lblResult0.Text = "<span style=\"font-weight:bold; color:#22bb22\">New customer added</span>";
                finishAdd = false;
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

        protected void btnExportAll_Click(object sender, EventArgs e)
        {
            BindGrid(String.Empty);
            DataTable dt = (DataTable)GridView1.DataSource;
            
            //Massage DataTable            
            CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, GridView1);
            
        }
        
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime bday, expirydate;
                
                if (DateTime.TryParse(e.Row.Cells[7].Text, out bday))
                {
                    e.Row.Cells[7].Text = bday.ToString("dd MMM yyyy");
                }                
            }           
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
}
}

