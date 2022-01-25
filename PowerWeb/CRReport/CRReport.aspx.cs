using PowerPOS;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
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
using System.IO;
using System.Globalization;
using SubSonic;
using Resources;
using System.Resources;
using System.Collections;
using System.Diagnostics;

namespace PowerWeb.CRReport
{
    /// <remarks>
    /// Workflow in a glance:
    /// 1. Read the Report Name from the parameter [r] and load TheReport.
    /// 2. Generate filter Control in HTML page (like Start Date, End Date, etc) based on the Parameter list 
    ///    registered in the loaded Crystal Report
    /// 3. User key in what kind of data they want to see in the filter
    /// 4. Load the Query (SQLString) from the loaded Crystal Report, and replace the Parameter ({?Key}) 
    ///    with the selected Filter
    /// 5. Use the SQLString, get the DataTable, and pass back to Crystal Report to load the data
    /// </remarks>
    
    public partial class CRReport : PageBase
    {
        /// <summary>
        /// The Report that will be shown in the page. This report shall be loaded from Website Parameter [r]
        /// </summary>
        private PowerReport.CRReport TheReport;

        int Index = 0;
        bool light = true;

        /// <summary>
        /// The List of Control that is generated from Crystal Report's Parameter. This is just for reference because it is difficult to iterate the controls from HTML page
        /// </summary>
        private List<Control> ListOfControl = new List<Control>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TheReport == null)
            {
                #region *) Load TheReport specified in Parameter [r]
                if (Request.QueryString["r"] == null)
                    throw new Exception("Report is not found");


                string ReportName = "";

                if (Session["CURRENT_LANGUAGE"] != null)
                {
                    if (!Request.QueryString["r"].ToLower().Contains("analytic"))
                    {
                        if (Session["CURRENT_LANGUAGE"].ToString() == "zh-CN")
                        {
                            ReportName = Server.MapPath("~\\bin\\Reports\\zh-CN\\" + Request.QueryString["r"]);
                        }
                        else if (Session["CURRENT_LANGUAGE"].ToString() == "zh-SG")
                        {
                            ReportName = Server.MapPath("~\\bin\\Reports\\zh-SG\\" + Request.QueryString["r"]);
                        }
                        else if (Session["CURRENT_LANGUAGE"].ToString() == "en-US")
                        {
                            ReportName = Server.MapPath("~\\bin\\Reports\\\\" + Request.QueryString["r"]);
                        }
                    }
                    else
                    {
                        ReportName = Server.MapPath("~\\bin\\Reports\\Analytic\\" + Request.QueryString["r"]);
                    }
                }
                else 
                {
                    if (!Request.QueryString["r"].ToLower().Contains("analytic"))
                    {
                        ReportName = Server.MapPath("~\\bin\\Reports\\" + Request.QueryString["r"]);
                    }
                    else
                    {
                        ReportName = Server.MapPath("~\\bin\\Reports\\Analytic\\" + Request.QueryString["r"]);
                    }
                }

                if (!File.Exists(ReportName))
                {
                    if (!Request.QueryString["r"].ToLower().Contains("analytic"))
                    {
                        ReportName = Server.MapPath("~\\bin\\Reports\\" + Request.QueryString["r"]);
                    }
                    else
                    {
                        ReportName = Server.MapPath("~\\bin\\Reports\\Analytic\\" + Request.QueryString["r"]);
                    }
                }
                TheReport = new PowerReport.CRReport(ReportName);

                if (!TheReport.IsReportLoaded)
                    throw new Exception("Report is not found");
                #endregion
            }

            if (TheReport != null)
                this.Page.Title = LanguageManager.GetTranslation(TheReport.ReportTitle);

            if (!string.IsNullOrEmpty(Request.QueryString["HideTopBannerMenu"]) && Request.QueryString["HideTopBannerMenu"].ToLower() == "true")
                this.Master.FindControl("OUTERTABLE1").Visible = false;

            if (!string.IsNullOrEmpty(Request.QueryString["showfilter"]))
            {
                bool showFilter = Convert.ToBoolean(Request.QueryString["showfilter"]);
                FilterTable.Visible = showFilter;
                LinkButton1.Visible = showFilter;
                LinkButton4.Visible = showFilter;
            }

            if (Page.IsPostBack && Request.Form["__EVENTTARGET"]== CrystalReportViewer1.UniqueID)
            {
                BindGrid();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

               // WareHouseSetup();

                #region *) Load TheReport specified in Parameter [r]
                if (Request.QueryString["r"] == null)
                    throw new Exception("Report is not found");

                string ReportName = "";


                if (Session["CURRENT_LANGUAGE"] != null)
                {
                    if (!Request.QueryString["r"].ToLower().Contains("analytic"))
                    {
                        if (Session["CURRENT_LANGUAGE"].ToString() == "zh-CN")
                        {
                            ReportName = Server.MapPath("~\\bin\\Reports\\zh-CN\\" + Request.QueryString["r"]);
                        }
                        else if (Session["CURRENT_LANGUAGE"].ToString() == "zh-SG")
                        {
                            ReportName = Server.MapPath("~\\bin\\Reports\\zh-SG\\" + Request.QueryString["r"]);
                        }
                        else if (Session["CURRENT_LANGUAGE"].ToString() == "en-US")
                        {
                            ReportName = Server.MapPath("~\\bin\\Reports\\\\" + Request.QueryString["r"]);
                        }
                        else 
                        {
                            ReportName = Server.MapPath("~\\bin\\Reports\\\\" + Request.QueryString["r"]);
                        }
                    }
                    else
                    {
                        ReportName = Server.MapPath("~\\bin\\Reports\\Analytic\\" + Request.QueryString["r"]);
                    }
                }
                else
                {
                    if (!Request.QueryString["r"].ToLower().Contains("analytic"))
                    {
                        ReportName = Server.MapPath("~\\bin\\Reports\\" + Request.QueryString["r"]);
                    }
                    else
                    {
                        ReportName = Server.MapPath("~\\bin\\Reports\\Analytic\\" + Request.QueryString["r"]);
                    }
                }

                TheReport = new PowerReport.CRReport(ReportName);

                if (!TheReport.IsReportLoaded)
                    throw new Exception("Report is not found");
                #endregion

                #region *) Generate Filter Control from Crystal Report's Parameter List
                SortedList<string, ParameterFieldDefinition> CRFilters = TheReport.GetFilters();

                #region -) Check StartDate
                if (CRFilters.ContainsKey("StartDate"))
                    CreateControl_DateTime(CRFilters["StartDate"], Index++, -1);
                #endregion

                #region -) Check StartDateToday
                if (CRFilters.ContainsKey("StartDateToday"))
                    CreateControl_DateTime(CRFilters["StartDateToday"], Index++, 0);
                #endregion

                #region -) Check StartDateWithTime
                if (CRFilters.ContainsKey("StartDateWithTime"))
                {
                    CreateControl_DateTime(CRFilters["StartDateWithTime"], Index++, -1, "00:00:00");
                    if (!string.IsNullOrEmpty(Request.QueryString["StartDateWithTime"]))
                    {
                        TextBox txt = (TextBox)ListOfControl.Find(c => c.ID == "StartDateWithTime");
                        DateTime startDateWithTime = DateTime.Parse(Request.QueryString["StartDateWithTime"]);
                        txt.Text = startDateWithTime.ToString("dd MMM yyyy");
                        AjaxControlToolkit.CalendarExtender calExt = (AjaxControlToolkit.CalendarExtender)ListOfControl.Find(ce => ce.ID == "act" + "StartDateWithTime");
                        calExt.SelectedDate = DateTime.Parse(Request.QueryString["StartDateWithTime"]);

                        DropDownList hour = (DropDownList)ListOfControl.Find(c => c.ID == "hour_StartDateWithTime");
                        if (hour != null)
                        {
                            hour.SelectedValue = startDateWithTime.ToString("HH");
                        }

                        DropDownList minute = (DropDownList)ListOfControl.Find(c => c.ID == "minute_StartDateWithTime");
                        if (minute != null)
                        {
                            minute.SelectedValue = startDateWithTime.ToString("mm");
                        }

                        DropDownList second = (DropDownList)ListOfControl.Find(c => c.ID == "second_StartDateWithTime");
                        if (second != null)
                        {
                            second.SelectedValue = startDateWithTime.ToString("ss");
                        }
                    }
                }
                #endregion

                #region -) Check EndDate
                if (CRFilters.ContainsKey("EndDate"))
                    CreateControl_DateTime(CRFilters["EndDate"], Index++, -1);
                #endregion

                #region -) Check EndDateToday
                if (CRFilters.ContainsKey("EndDateToday"))
                    CreateControl_DateTime(CRFilters["EndDateToday"], Index++, 0);
                #endregion

                #region -) Check EndDateWithTime
                if (CRFilters.ContainsKey("EndDateWithTime"))
                {
                    CreateControl_DateTime(CRFilters["EndDateWithTime"], Index++, -1, "23:59:59");
                    if (!string.IsNullOrEmpty(Request.QueryString["EndDateWithTime"]))
                    {
                        TextBox txt = (TextBox)ListOfControl.Find(c => c.ID == "EndDateWithTime");
                        DateTime endDateWithTime = DateTime.Parse(Request.QueryString["EndDateWithTime"]);
                        txt.Text = endDateWithTime.ToString("dd MMM yyyy");
                        AjaxControlToolkit.CalendarExtender calExt = (AjaxControlToolkit.CalendarExtender)ListOfControl.Find(ce => ce.ID == "act" + "EndDateWithTime");
                        calExt.SelectedDate = DateTime.Parse(Request.QueryString["EndDateWithTime"]);

                        DropDownList hour = (DropDownList)ListOfControl.Find(c => c.ID == "hour_EndDateWithTime");
                        if (hour != null)
                        {
                            hour.SelectedValue = endDateWithTime.ToString("HH");
                        }

                        DropDownList minute = (DropDownList)ListOfControl.Find(c => c.ID == "minute_EndDateWithTime");
                        if (minute != null)
                        {
                            minute.SelectedValue = endDateWithTime.ToString("mm");
                        }

                        DropDownList second = (DropDownList)ListOfControl.Find(c => c.ID == "second_EndDateWithTime");
                        if (second != null)
                        {
                            second.SelectedValue = endDateWithTime.ToString("ss");
                        }
                    }
                }
                #endregion

                #region -) Check ShowLastWeek
                if (CRFilters.ContainsKey("StartDate") && CRFilters.ContainsKey("EndDate") && CRFilters.ContainsKey("ShowLastWeek"))
                {
                    CreateControl_LastWeekButton(CRFilters["ShowLastWeek"], --Index);
                    Index++;
                }
                #endregion

                #region -) Check Month DropDown
                if (CRFilters.ContainsKey("EndDate") == true && CRFilters.ContainsKey("StartDate") == true)
                    CreateControlDropDown_DateTime(CRFilters["EndDate"], Index++);
                #endregion

                #region -) Check ItemDepartment
                if (CRFilters.ContainsKey("ItemDepartment"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["ItemDepartment"], Index++, out theObject);
                    ItemDepartmentCollection Dt = (new ItemDepartmentCollection()).Load();
                    ItemDepartment Obj = new ItemDepartment();
                    Obj.ItemDepartmentID = "ALL";
                    Obj.DepartmentName = "ALL";
                    Dt.Insert(0, Obj);
                    theObject.DataSource = Dt;
                    theObject.DataValueField = ItemDepartment.Columns.ItemDepartmentID;
                    theObject.DataTextField = ItemDepartment.Columns.DepartmentName;
                    theObject.DataBind();
                }
                #endregion

                #region -) Check Category
                if (CRFilters.ContainsKey("Category"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["Category"],Index++, out theObject);
                    CategoryCollection Dt = (new CategoryCollection()).Load();
                    Category Obj = new Category();
                    Obj.CategoryName = "ALL";
                    Obj.CategoryName = "ALL";
                    Dt.Insert(0, Obj);
                    theObject.DataSource = Dt;
                    theObject.DataValueField = Category.Columns.CategoryName;
                    theObject.DataTextField = Category.Columns.CategoryName;
                    theObject.DataBind();
                }
                #endregion

                #region -) Check UserMst
                if (CRFilters.ContainsKey("UserMst"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["UserMst"], Index++, out theObject);
                    UserMstCollection Dt = (new UserMstCollection()).Load();
                    UserMst Obj = new UserMst();
                    Obj.UserName = "ALL";
                    Obj.UserName = "ALL";
                    Dt.Insert(0, Obj);
                    theObject.DataSource = Dt;
                    theObject.DataValueField = UserMst.Columns.UserName;
                    theObject.DataTextField = UserMst.Columns.UserName;
                    theObject.DataBind();
                }
                #endregion

                #region -) Check UserMst
                if (CRFilters.ContainsKey("SalesPerson"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["SalesPerson"], Index++, out theObject);
                    Query queSalesPerson = new Query("UserMst");
                    queSalesPerson.WHERE(UserMst.Columns.IsASalesPerson, true);
                    queSalesPerson.WHERE(UserMst.Columns.Deleted, false);
                    UserMstCollection Dt = new UserMstCollection();
                    Dt.LoadAndCloseReader(queSalesPerson.ExecuteReader());
                    UserMst Obj = new UserMst();
                    Obj.UserName = "ALL";
                    Obj.UserName = "ALL";
                    Dt.Insert(0, Obj);
                    theObject.DataSource = Dt;
                    theObject.DataValueField = UserMst.Columns.UserName;
                    theObject.DataTextField = UserMst.Columns.UserName;
                    theObject.DataBind();
                }
                #endregion

                #region -) Check AccessSource
                if (CRFilters.ContainsKey("AccessSource"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["AccessSource"], Index++, out theObject);
                    var Dt = new List<string>();
                    Dt.Add("ALL");
                    Dt.Add("POS");
                    Dt.Add("WEB");
                    theObject.DataSource = Dt;
                    theObject.DataBind();
                }
                #endregion

                #region -) Check Outlet
                string selectedOutlet = "ALL";
                if (CRFilters.ContainsKey("Outlet"))
                {
                    //DropDownList theObject;
                    //CreateControl_ComboBox(CRFilters["Outlet"], Index++, out theObject);
                    //OutletCollection Dt = OutletController.FetchByUserNameForReport(false, true, Session["UserName"] + "");
                    //theObject.DataSource = Dt;
                    //theObject.DataValueField = Outlet.Columns.OutletName;
                    //theObject.DataTextField = Outlet.Columns.OutletName;
                    //theObject.DataBind();

                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["Outlet"], Index++, out theObject);
                    theObject.Items.Clear();
                    theObject.Items.AddRange(PointOfSaleController.FetchOutletNames(false, Session["UserName"] + ""));
                    theObject.DataBind();
                    theObject.SelectedIndex = 0;

                    theObject.SelectedIndexChanged += new EventHandler(ddlOutlet_SelectedIndexChanged);
                    theObject.AutoPostBack = true;
                    selectedOutlet = theObject.SelectedValue;
                }
                #endregion

                #region -) Check Region
                if (CRFilters.ContainsKey("Region"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["Region"], Index++, out theObject);
                    string SQLString = "SELECT DISTINCT UserFld2 FROM PointOfSale WHERE UserFld2 IS NOT NULL";
                    DataTable Dt = new DataTable();
                    Dt.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(SQLString)));
                    DataRow Obj = Dt.NewRow();
                    Obj[0] = "ALL";
                    Dt.Rows.InsertAt(Obj, 0);
                    theObject.DataSource = Dt;
                    theObject.DataValueField = "UserFld2";
                    theObject.DataTextField = "UserFld2";
                    theObject.DataBind();

                    theObject.SelectedIndexChanged += new EventHandler(ddlRegion_SelectedIndexChanged);
                    theObject.AutoPostBack = true;
                }
                #endregion

                #region -) Check Country
                if (CRFilters.ContainsKey("Country"))
                {
                    DropDownList theObject;
                    // CreateControl_ComboBox(CRFilters["Country"], Index++, out theObject);
                    CreateControl_ComboBox_Country_WithUpdatePanel(CRFilters["Country"], Index++, out theObject);
                    string SQLString = "SELECT DISTINCT UserFld1 FROM PointOfSale WHERE UserFld2 IS NOT NULL";
                    DataTable Dt = new DataTable();
                    Dt.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(SQLString)));
                    DataRow Obj = Dt.NewRow();
                    Obj[0] = "ALL";
                    Dt.Rows.InsertAt(Obj, 0);
                    theObject.DataSource = Dt;
                    theObject.DataValueField = "UserFld1";
                    theObject.DataTextField = "UserFld1";
                    theObject.DataBind();

                    //attach index change handler
                    theObject.SelectedIndexChanged += new EventHandler(ddlCountry_SelectedIndexChanged);
                    theObject.AutoPostBack =true;
                }
                #endregion

                #region -) Check PointOfSale
                if (CRFilters.ContainsKey("PointOfSale"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox_POS_WithUpdatePanel(CRFilters["PointOfSale"], Index++, out theObject);
                    theObject.Items.Clear();
                    theObject.Items.AddRange(PointOfSaleController.FetchPointOfSaleNames(false, Session["UserName"] + "", selectedOutlet));
                    theObject.DataBind();
                    //PointOfSaleCollection Dt = PointOfSaleController.FetchByUserNameForReport(false,true, Session["UserName"] + "","ALL");
                    //theObject.DataSource = Dt;
                    //theObject.DataValueField = PointOfSale.Columns.PointOfSaleID;
                    //theObject.DataTextField = PointOfSale.Columns.PointOfSaleName;
                    //theObject.DataBind();

                }
                #endregion

                #region -) Check Discount
                if (CRFilters.ContainsKey("Discount"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox_POS_WithUpdatePanel(CRFilters["Discount"], Index++, out theObject);
                    SpecialDiscountCollection Dt = (new SpecialDiscountCollection()).Load();
                    SpecialDiscount Obj = new SpecialDiscount();
                    Obj.DiscountName = "ALL";
                    Obj.DiscountLabel = "ALL";
                    Dt.Insert(0, Obj);
                    for (int i = 0; i < Dt.Count; i++)
                    {
                        if (!Dt[i].ShowLabel)
                            Dt[i].DiscountLabel = Dt[i].DiscountName;
                        else
                            Dt[i].DiscountLabel = Dt[i].DiscountLabel == "" ? Dt[i].DiscountName : Dt[i].DiscountLabel;
                    }

                    theObject.DataSource = Dt;
                    theObject.DataValueField = SpecialDiscount.Columns.DiscountName;
                    theObject.DataTextField = SpecialDiscount.Columns.DiscountLabel;
                    theObject.DataBind();

                }
                #endregion

                #region -) Check DemandTemplate
                if (CRFilters.ContainsKey("DemandTemplate"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["DemandTemplate"], Index++, out theObject);
                    string SQLString = "SELECT DISTINCT TemplateName FROM DemandTemplate ORDER BY TemplateName";
                    DataTable Dt = new DataTable();
                    Dt.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(SQLString)));
                    theObject.DataSource = Dt;
                    theObject.DataValueField = "TemplateName";
                    theObject.DataTextField = "TemplateName";
                    theObject.DataBind();
                }
                #endregion

                #region -) Check InventoryLocationGroup
                if (CRFilters.ContainsKey("InventoryLocationGroup"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["InventoryLocationGroup"], Index++, out theObject);
                    InventoryLocationGroupCollection Dt = (new InventoryLocationGroupCollection()).Load();

                    InventoryLocationGroup Obj = new InventoryLocationGroup();
                    Obj.InventoryLocationGroupID = 0;
                    Obj.InventoryLocationGroupName = "ALL";
                    Dt.Insert(0, Obj);

                    theObject.DataSource = Dt;
                    theObject.DataValueField = InventoryLocationGroup.Columns.InventoryLocationGroupID;
                    theObject.DataTextField = InventoryLocationGroup.Columns.InventoryLocationGroupName;
                    theObject.DataBind();
                    theObject.SelectedIndex = 0;
                }
                #endregion

                #region -) Check InventoryLocationGroupBreakDown
                if (CRFilters.ContainsKey("InventoryLocationGroupBreakDown"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["InventoryLocationGroupBreakDown"], Index++, out theObject);
                    InventoryLocationGroupCollection Dt = (new InventoryLocationGroupCollection()).Load();

                    InventoryLocationGroup Obj1 = new InventoryLocationGroup();
                    Obj1.InventoryLocationGroupID = -1;
                    Obj1.InventoryLocationGroupName = "ALL-BreakDown";
                    Dt.Insert(0, Obj1);

                    InventoryLocationGroup Obj = new InventoryLocationGroup();
                    Obj.InventoryLocationGroupID = 0;
                    Obj.InventoryLocationGroupName = "ALL";
                    Dt.Insert(0, Obj);

                    theObject.DataSource = Dt;
                    theObject.DataValueField = InventoryLocationGroup.Columns.InventoryLocationGroupID;
                    theObject.DataTextField = InventoryLocationGroup.Columns.InventoryLocationGroupName;
                    theObject.DataBind();
                    theObject.SelectedIndex = 0;
                }
                #endregion

                #region -) Check InventoryLocationName
                if (CRFilters.ContainsKey("InventoryLocation"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["InventoryLocation"],  Index++, out theObject);
                    InventoryLocationCollection Dt = (new InventoryLocationCollection()).Load();
                    InventoryLocation Obj = new InventoryLocation();
                    Obj.InventoryLocationID = 0;
                    Obj.InventoryLocationName = "ALL";
                    Dt.Insert(0, Obj);
                    theObject.DataSource = Dt;
                    theObject.DataValueField = InventoryLocation.Columns.InventoryLocationID;
                    theObject.DataTextField = InventoryLocation.Columns.InventoryLocationName;
                    theObject.DataBind();
                }
                #endregion

                #region -) Check InventoryLocationBreakDown
                if (CRFilters.ContainsKey("InventoryLocationBreakDown"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["InventoryLocationBreakDown"], Index++, out theObject);
                    InventoryLocationCollection Dt = (new InventoryLocationCollection()).Load();

                    InventoryLocation Obj1 = new InventoryLocation();
                    Obj1.InventoryLocationID = -1;
                    Obj1.InventoryLocationName = "ALL-BreakDown";
                    Dt.Insert(0, Obj1);

                    InventoryLocation Obj = new InventoryLocation();
                    Obj.InventoryLocationID = 0;
                    Obj.InventoryLocationName = "ALL";
                    Dt.Insert(0, Obj);

                    theObject.DataSource = Dt;
                    theObject.DataValueField = InventoryLocation.Columns.InventoryLocationID;
                    theObject.DataTextField = InventoryLocation.Columns.InventoryLocationName;
                    theObject.DataBind();
                    theObject.SelectedIndex = 0;
                }
                #endregion

                #region -) Check InventoryLocationName
                if (CRFilters.ContainsKey("ToInventoryLocation"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["ToInventoryLocation"], Index++, out theObject);
                    InventoryLocationCollection Dt = (new InventoryLocationCollection()).Load();
                    InventoryLocation Obj = new InventoryLocation();
                    Obj.InventoryLocationID = 0;
                    Obj.InventoryLocationName = "ALL";
                    Dt.Insert(0, Obj);
                    theObject.DataSource = Dt;
                    theObject.DataValueField = InventoryLocation.Columns.InventoryLocationID;
                    theObject.DataTextField = InventoryLocation.Columns.InventoryLocationName;
                    theObject.DataBind();
                }
                #endregion

                #region -) Check InventoryLocationName
                if (CRFilters.ContainsKey("FromInventoryLocation"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["FromInventoryLocation"], Index++, out theObject);
                    InventoryLocationCollection Dt = (new InventoryLocationCollection()).Load();
                    InventoryLocation Obj = new InventoryLocation();
                    Obj.InventoryLocationID = 0;
                    Obj.InventoryLocationName = "ALL";
                    Dt.Insert(0, Obj);
                    theObject.DataSource = Dt;
                    theObject.DataValueField = InventoryLocation.Columns.InventoryLocationID;
                    theObject.DataTextField = InventoryLocation.Columns.InventoryLocationName;
                    theObject.DataBind();
                }
                #endregion

                #region -) Check MovementType
                if (CRFilters.ContainsKey("MovementType"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["MovementType"], Index++, out theObject);
                    theObject.Items.Add("ALL");
                    theObject.Items.Add("Stock In");
                    theObject.Items.Add("Stock Out");
                    theObject.Items.Add("Transfer In");
                    theObject.Items.Add("Transfer Out");
                    theObject.Items.Add("Adjustment In");
                    theObject.Items.Add("Adjustment Out");
                    theObject.Items.Add("Return Out");
                    //theObject.DataBind();
                }
                #endregion

                #region -) Check IsService
                if (CRFilters.ContainsKey("IsService"))
                {
                    CreateControl_CheckBox(CRFilters["IsService"], Index++);
                }
                #endregion

                #region -) Check IsConsignment
                if (CRFilters.ContainsKey("IsConsignment"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["IsConsignment"], Index++, out theObject);
                    List<string> data = new List<string>();
                    data.Add("ALL");
                    data.Add("Yes");
                    data.Add("No");
                    theObject.DataSource = data;
                    theObject.DataBind();
                }
                #endregion

                #region -) Check IsProduct
                if (CRFilters.ContainsKey("IsProduct"))
                {
                    CreateControl_CheckBox(CRFilters["IsProduct"], Index++);
                }
                #endregion

                #region -) Check MembershipGroupID (GroupName)
                if (CRFilters.ContainsKey("MembershipGroupID"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["MembershipGroupID"], Index++, out theObject);
                    MembershipGroupCollection Dt = (new MembershipGroupCollection().Load());
                    MembershipGroup Obj = new MembershipGroup();
                    Obj.MembershipGroupId = 0;
                    Obj.GroupName = "ALL";
                    Dt.Insert(0, Obj);
                    theObject.DataSource = Dt;
                    theObject.DataValueField = MembershipGroup.Columns.MembershipGroupId;
                    theObject.DataTextField = MembershipGroup.Columns.GroupName;
                    theObject.DataBind();
                }
                #endregion

                #region -) Check DepartmentID (Department)
                if (CRFilters.ContainsKey("Department"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["Department"], Index++, out theObject);
                    DepartmentCollection Dt = (new DepartmentCollection().Load());
                    //Department Obj = new Department(); * no need to insert ALL to Department *
                    //Obj.DepartmentID = 0;
                    //Obj.DepartmentName = "ALL";
                    //Dt.Insert(0, Obj);
                    theObject.DataSource = Dt;
                    theObject.DataValueField = Department.Columns.DepartmentID;
                    theObject.DataTextField = Department.Columns.DepartmentName;
                    theObject.DataBind();
                }
                #endregion

                #region -) Check ItemList
                if (CRFilters.ContainsKey("ItemList"))
                {

                }

                #endregion

                #region -) Check Variance
                if (CRFilters.ContainsKey("Variance"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["Variance"], Index++, out theObject);
                    theObject.Items.Add("ALL");
                    theObject.Items.Add("Positive");
                    theObject.Items.Add("Zero");
                    theObject.Items.Add("Negative");
                }
                #endregion

                #region -) Check Operation
                if (CRFilters.ContainsKey("Operation"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["Operation"], Index++, out theObject);
                    var dt = new List<object>();
                    dt.Add(new { Name = "ALL" });
                    //dt.Add(new { Name = "INSERT" });
                    dt.Add(new { Name = "UPDATE" });
                    dt.Add(new { Name = "DELETE" });
                    theObject.DataSource = dt;
                    theObject.DataValueField = "Name";
                    theObject.DataTextField = "Name";
                    theObject.DataBind();

                    if (Request.QueryString["Operation"] != null)
                    {
                        ListItem selItem = theObject.Items.FindByValue(Request.QueryString["Operation"]);
                        if (selItem != null) selItem.Selected = true;
                    }
                }
                #endregion

                #region -) Check TableName
                if (CRFilters.ContainsKey("TableName"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["TableName"], Index++, out theObject);
                    DataSet ds = new DataSet();
                    ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\TableNameFilter.xml");
                    var dt = ds.Tables[0];
                    List<string> tableName = new List<string>();

                    string langSetting = AppSetting.GetSetting(AppSetting.SettingsName.LanguageSetting);
                    if (string.IsNullOrEmpty(langSetting))
                        langSetting = "ENG";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string table = ((string)dt.Rows[i]["Name"]) + "";
                        string sql = string.Format("SELECT TOP 1 {0} FROM TEXT_LANGUAGE WHERE ID = '{1}.{1}'", langSetting, table);
                        DataTable dtLang = new DataTable();
                        dtLang.Load(DataService.GetReader(new QueryCommand(sql)));
                        if (dtLang.Rows.Count > 0)
                            table = (string)dtLang.Rows[0][0];
                        tableName.Add(table);
                    }
                    tableName = tableName.OrderBy(o => o).ToList();
                    tableName.Insert(0, "ALL");
                    theObject.DataSource = tableName;
                    //theObject.DataValueField = "Name";
                    //theObject.DataTextField = "Name";
                    theObject.DataBind();

                    if (Request.QueryString["TableName"] != null)
                    {
                        ListItem selItem = theObject.Items.FindByValue(Request.QueryString["TableName"]);
                        if (selItem != null) selItem.Selected = true;
                    }
                }
                #endregion

                #region -) Check Package Type
                if (CRFilters.ContainsKey("PackageType"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["PackageType"], Index++, out theObject);
                    List<string> data = new List<string>();
                    data.Add("ALL");
                    data.Add("Package");
                    data.Add("Points");
                    theObject.DataSource = data;
                    theObject.DataBind();
                }
                #endregion

                //IsCheckIn
                #region -) Check IsCheckIn
                if (CRFilters.ContainsKey("IsCheckIn"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["IsCheckIn"], Index++, out theObject);
                    List<string> data = new List<string>();
                    data.Add("ALL");
                    data.Add("Yes");
                    data.Add("No");
                    theObject.DataSource = data;
                    theObject.DataBind();
                }
                #endregion

                #region -) Check Discount Reason
                if (CRFilters.ContainsKey("DiscountReason"))
                {
                    DropDownList theObject;
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.DiscountReportShowSearchDiscountReason), false))
                    {
                        CreateControl_ComboBox(CRFilters["DiscountReason"],Index++, out theObject);
                    }
                    else 
                    {
                        CreateControl_ComboBox_VisibleFalse(CRFilters["DiscountReason"], Index, out theObject);
                    }

                    theObject.Items.Add(new ListItem() { Text = "ALL", Value = "", Selected = true });

                    string textDiscountReason = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SelectableDiscountReason);
                    if(!string.IsNullOrEmpty(textDiscountReason))
                    {
                        string[] DiscountReason = textDiscountReason.ToString().Split(';');

                        for (int i = 0; i < DiscountReason.Count(); i++)
                        { 
                            theObject.Items.Add(new ListItem() {Text = DiscountReason[i].Trim(), Value = DiscountReason[i].Trim()});
                        }
                    }
                }
                #endregion

                #region -) Check Supplier
                if (CRFilters.ContainsKey("Supplier"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["Supplier"], Index++, out theObject);
                    SupplierCollection Dt = (new SupplierCollection()).Load();
                    var q = from p in Dt orderby p.SupplierName select p;
                    var tmp = q.ToList();
                    Supplier Obj = new Supplier();
                    Obj.SupplierID = 0;
                    Obj.SupplierName = "ALL";
                    tmp.Insert(0, Obj);
                    theObject.DataSource = tmp;
                    theObject.DataValueField = Supplier.Columns.SupplierName;
                    theObject.DataTextField = Supplier.Columns.SupplierName;
                    theObject.DataBind();
                }
                #endregion

                #region - ) Check StockOut Reason

                if (CRFilters.ContainsKey("InventoryStockOutReason"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["InventoryStockOutReason"], Index++, out theObject);
                    InventoryStockOutReasonCollection Dt = (new InventoryStockOutReasonCollection()).Load();
                    InventoryStockOutReason Obj = new InventoryStockOutReason();
                    Obj.ReasonID = -1;
                    Obj.ReasonName = "--Please Select--";
                    Dt.Insert(0, Obj);
                    theObject.DataSource = Dt;
                    theObject.DataValueField = InventoryStockOutReason.Columns.ReasonID;
                    theObject.DataTextField = InventoryStockOutReason.Columns.ReasonName;
                    theObject.DataBind();
                }
                
                #endregion

                #region -) Check DeliveryStatus
                if (CRFilters.ContainsKey("DeliveryStatus"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["DeliveryStatus"], Index++, out theObject);
                    theObject.Items.Add("ALL");
                    theObject.Items.Add("Delivered");
                    theObject.Items.Add("Not Delivered");
                }
                #endregion

                #region -) Check DocOrderType
                if (CRFilters.ContainsKey("OrderDocType"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["OrderDocType"], Index++, out theObject);
                    theObject.Items.Add("ALL");
                    theObject.Items.Add("Credit Note");
                    theObject.Items.Add("Invoice");
                    //theObject.DataBind();
                }
                #endregion

                #region -) Check IsPreOrder
                if (CRFilters.ContainsKey("IsPreOrder"))
                {
                    CreateControl_CheckBox(CRFilters["IsPreOrder"], Index++);
                }
                #endregion

                #region -) Check COGType
                if (CRFilters.ContainsKey("COGType"))
                {
                    DropDownList theObject;
                    CreateControl_ComboBox(CRFilters["COGType"], Index++, out theObject);
                    theObject.Items.Add("Global Cost");
                    theObject.Items.Add("Inventory Location Cost");
                    string status = "";
                    if (InventoryLocationController.IsHaveLocationGroup(out status))
                    {
                        theObject.Items.Add("Inventory Location Group Cost");
                    }
                    //theObject.DataBind();
                }
                #endregion

                #region -) Check the other controls (Generic)
                foreach (string Key in CRFilters.Keys)
                {
                    if (Key == "StartDate") continue;
                    if (Key == "EndDate") continue;
                    if (Key == "StartDateToday") continue;
                    if (Key == "EndDateToday") continue;
                    if (Key == "StartDateWithTime") continue;
                    if (Key == "EndDateWithTime") continue;
                    if (Key == "ItemDepartment") continue;
                    if (Key == "Category") continue;
                    if (Key == "Outlet") continue;
                    if (Key == "Region") continue;
                    if (Key == "Country") continue;
                    if (Key == "PointOfSale") continue;
                    if (Key == "DemandTemplate") continue;
                    if (Key == "InventoryLocation") continue;
                    if (Key == "FromInventoryLocation") continue;
                    if (Key == "ToInventoryLocation") continue;
                    if (Key == "MovementType") continue;
                    if (Key == "IsService") continue;
                    if (Key == "IsProduct") continue;
                    if (Key == "MembershipGroupID") continue;
                    if (Key == "Department") continue;
                    if (Key == "ItemList") continue;
                    if (Key == "ShowLastWeek") continue;
                    if (Key == "UserMst") continue;
                    if (Key == "AccessSource") continue;
                    if (Key == "Discount") continue;
                    if (Key == "Variance") continue;
                    if (Key == "Operation") continue;
                    if (Key == "TableName") continue;
                    if (Key == "PackageType") continue;
                    if (Key == "IsCheckIn") continue;
                    if (Key == "DiscountReason") continue;
                    if (Key == "Supplier") continue;
                    if (Key == "CurrentSalesPerson") continue;
                    if (Key == "InventoryStockOutReason") continue;
                    if (Key == "DeliveryStatus") continue;
                    if (Key == "InventoryLocationBreakDown") continue;
                    if (Key == "InventoryLocationGroup") continue;
                    if (Key == "InventoryLocationGroupBreakDown") continue;
                    if (Key == "OrderDocType") continue;
                    if (Key == "SalesPerson") continue;
                    if (Key == "IsPreOrder") continue;
                    if (Key == "IsConsignment") continue;
                    if (Key == "COGType") continue;

                    ParameterFieldDefinition TheObject = CRFilters[Key];

                    if (TheObject.ValueType == FieldValueType.DateTimeField)
                    {
                        CreateControl_DateTime(CRFilters[Key], Index++, -1);
                        if (!string.IsNullOrEmpty(Request.QueryString[Key]))
                        {
                            RadioButton rdb = (RadioButton)ListOfControl.Find(c => c.ID == "rdb" + Key);
                            rdb.Checked = true;
                            TextBox txt = (TextBox)ListOfControl.Find(c => c.ID == Key);
                            txt.Text = DateTime.Parse(Request.QueryString[Key]).ToString("dd MMM yyyy");
                            AjaxControlToolkit.CalendarExtender calExt = (AjaxControlToolkit.CalendarExtender)ListOfControl.Find(ce => ce.ID == "act" + Key);
                            calExt.SelectedDate = DateTime.Parse(Request.QueryString[Key]);
                        }
                    }
                    else if (TheObject.ValueType == FieldValueType.DateField)
                    {
                        CreateControl_DateTime(CRFilters[Key], Index++, -1);
                        if (!string.IsNullOrEmpty(Request.QueryString[Key]))
                        {
                            RadioButton rdb = (RadioButton)ListOfControl.Find(c => c.ID == "rdb" + Key);
                            rdb.Checked = true;
                            TextBox txt = (TextBox)ListOfControl.Find(c => c.ID == Key);
                            txt.Text = DateTime.Parse(Request.QueryString[Key]).ToString("dd MMM yyyy");
                            AjaxControlToolkit.CalendarExtender calExt = (AjaxControlToolkit.CalendarExtender)ListOfControl.Find(ce => ce.ID == "act" + Key);
                            calExt.SelectedDate = DateTime.Parse(Request.QueryString[Key]);
                        }
                    }
                    else
                    {
                        CreateControl_String(CRFilters[Key], Index++);
                        if (!string.IsNullOrEmpty(Request.QueryString[Key]))
                        {
                            TextBox txt = (TextBox)ListOfControl.Find(c => c.ID == Key);
                            txt.Text = Request.QueryString[Key];
                        }
                    }
                }
                #endregion

                #endregion

                if (!IsPostBack)
                {
                    BindGrid();
                }
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                lblMsg.Text = X.Message;
                lblMsg.Visible = true;
            }
        }

        private string GetHeader(ParameterFieldDefinition DataFieldName)
        {
            string header = "";
            string oriText = DataFieldName.PromptText == "" ? DataFieldName.ParameterFieldName : DataFieldName.PromptText;
            header = LanguageManager.GetTranslation(oriText + "");
            if (string.IsNullOrEmpty(header))
                header = oriText;
            return header;
        }

        #region *) AJAX for Combo Boxes
        /// <summary>
        /// AJAX to handle Region Selection Changes
        /// </summary>
        void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlRegion = (DropDownList)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "Region"; });

            DropDownList ddlCountry = (DropDownList)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "Country"; });;
            ddlCountry.DataSource = PointOfSaleController.GetCountry(ddlRegion.SelectedValue.ToString());
            ddlCountry.DataTextField = "Txt";
            ddlCountry.DataValueField = "Val";
            ddlCountry.DataBind();
            ddlCountry.SelectedIndex = 0;

            DropDownList ddlPOS = (DropDownList)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "PointOfSale"; });;
            ddlPOS.DataSource = PointOfSaleController.GetPointOfSale(ddlCountry.SelectedValue.ToString());
            ddlPOS.DataTextField = "Txt";
            ddlPOS.DataValueField = "Val";
            ddlPOS.DataBind();
            ddlPOS.SelectedIndex = 0;
        }

        /// <summary>
        /// AJAX to handle Country Selection Changes
        /// </summary>
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlPOS = (DropDownList)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "PointOfSale"; });;
            DropDownList ddlCountry = (DropDownList) ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "Country"; });
            ddlPOS.DataSource = PointOfSaleController.GetPointOfSale(ddlCountry.SelectedValue.ToString());
            ddlPOS.DataTextField = "Txt";
            ddlPOS.DataValueField = "Val";
            ddlPOS.DataBind();
            ddlPOS.SelectedIndex = 0;

            //template specific to that country
            if ((TheReport.GetFilters()).ContainsKey("DemandTemplate"))
            {
                Control temp2 = ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "DemandTemplate"; });
                if (PointOfSaleController.GetTemplates(((DropDownList)sender).SelectedValue.ToString()).Rows.Count > 0)
                {
                    ((DropDownList)temp2).DataSource = PointOfSaleController.GetTemplates(((DropDownList)sender).SelectedValue.ToString());
                    ((DropDownList)temp2).DataTextField = "Txt";
                    ((DropDownList)temp2).DataValueField = "Val";
                    ((DropDownList)temp2).DataBind();
                    ((DropDownList)temp2).SelectedIndex = 0;
                }
            }
        }

        void ddlOutlet_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlOutlet = (DropDownList)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "Outlet"; });
            DropDownList ddlPOS = (DropDownList)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "PointOfSale"; }); ;
            string selectedOutlet = ddlOutlet.SelectedValue.ToString();

            if (ddlPOS != null)
            {
                ddlPOS.Items.Clear();
                ddlPOS.Items.AddRange(PointOfSaleController.FetchPointOfSaleNames(false, Session["UserName"] + "", selectedOutlet));
                ddlPOS.DataBind();
                ddlPOS.SelectedIndex = 0;
            }
        }

        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["sortBy"] = null;
            BindGrid();          
        }

        protected void CR_Navigate(object source, CrystalDecisions.Web.NavigateEventArgs e)
        {
            BindGrid();
        }

        protected void CR_Search(object source, CrystalDecisions.Web.SearchEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// I don't know what does this function do. Please update if you happen to know.
        /// </summary>
        private void ControlChangeHandler()
        {
            SortedList<string, string> theParameters = new SortedList<string, string>();
            #region *) Get the Filter Value that User Keyed in
            foreach (Control obj in ListOfControl)
            {
                string Key = obj.ID;
                string Val = "";

                if (obj is TextBox)
                {
                    Val = ((TextBox)obj).Text;

                    if (Key.ToLower() == "enddate" || Key.ToLower() == "enddatetoday")
                    {
                        try
                        {
                            DateTime tmp = DateTime.Parse(Val);
                            Val = tmp.Date.ToString("dd MMM yyyy") + " 23:59";
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                        }

                    }
                }
                else if (obj is DropDownList)
                {
                    //if (Key.ToLower() == "discountreason" && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.DiscountReportShowSearchDiscountReason), false))
                    //{
                    //    Val = "";
                    //}
                    //else
                    //{
                        Val = ((DropDownList)obj).SelectedValue;
                    //}
                }
                    //Val = ((DropDownList)obj).SelectedValue;

                theParameters.Add(Key, Val);
            }
            #endregion
        }

        private ReportDocument BindGrid()
        {
            Debug.WriteLine("==>> BindGrid Called");

            SortedList<string, string> theParameters = new SortedList<string, string>();
            #region *) Get the Filter Value that User Keyed in
            foreach (Control obj in ListOfControl)
            {
                string Key = obj.ID;
                string Val = "";

                if (obj is TextBox)
                {
                    Val = ((TextBox)obj).Text;

                    if (Key.ToLower() == "enddate" || Key.ToLower() == "enddatetoday" || Key.ToLower() == "enddatewithtime")
                    {
                        try
                        {
                            DateTime tmp = Val.GetDateTimeValue("dd MMM yyyy");
                            if (Key.ToLower() == "enddatewithtime")
                            {
                                // Try to get the time from combobox
                                string hour = "00", minute = "00", second = "00";
                                Control ctl;
                                ctl = FindControl("hour_enddatewithtime");
                                if (ctl is DropDownList) hour = ((DropDownList)ctl).SelectedValue;
                                ctl = FindControl("minute_enddatewithtime");
                                if (ctl is DropDownList) minute = ((DropDownList)ctl).SelectedValue;
                                ctl = FindControl("second_enddatewithtime");
                                if (ctl is DropDownList) second = ((DropDownList)ctl).SelectedValue;
                                Val = tmp.Date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) + " " + hour + ":" + minute + ":" + second;
                            }
                            else
                            {
                                Val = tmp.Date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) + " 23:59";
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                        }
                    }
                    if (Key.ToLower() == "startdate" || Key.ToLower() == "startdatetoday" || Key.ToLower() == "startdatewithtime")
                    {
                        DateTime tmp = Val.GetDateTimeValue("dd MMM yyyy");
                        if (Key.ToLower() == "startdatewithtime")
                        {
                            // Try to get the time from combobox
                            string hour = "00", minute = "00", second = "00";
                            Control ctl;
                            ctl = FindControl("hour_startdatewithtime");
                            if (ctl is DropDownList) hour = ((DropDownList)ctl).SelectedValue;
                            ctl = FindControl("minute_startdatewithtime");
                            if (ctl is DropDownList) minute = ((DropDownList)ctl).SelectedValue;
                            ctl = FindControl("second_startdatewithtime");
                            if (ctl is DropDownList) second = ((DropDownList)ctl).SelectedValue;
                            Val = tmp.Date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) + " " + hour + ":" + minute + ":" + second;
                        }
                        else
                        {
                            Val = tmp.Date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                }
                else if (obj is DropDownList)
                {
                    //if (Key.ToLower() == "discountreason" && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.DiscountReportShowSearchDiscountReason), false))
                    //{
                    //    Val = "";
                    //}
                    //else
                    //{
                        Val = ((DropDownList)obj).SelectedValue;
                    //}
                }
                else if (obj is CheckBox)
                    Val = ((CheckBox)obj).Checked.ToString();

                theParameters.Add(Key, Val);
            }
            if (Session["username"] != null && !string.IsNullOrEmpty(Session["username"].ToString()))
            {
                theParameters.Add("CurrentSalesPerson", Session["username"].ToString());
            }
            #endregion

            #region *) Get Report with DataTable loaded inside, and display it
            ReportDocument Rst = TheReport.GetReport(theParameters);

            #region *) Fill "GeneratedBy" formula in the rpt (if exists)
            foreach (FormulaFieldDefinition ffd in Rst.DataDefinition.FormulaFields)
            {
                if (ffd.Name == "GeneratedBy")
                    ffd.Text = "\"" + Session["username"].ToString() + "\"";

                if (ffd.Name == "DecimalPlaces")
                {
                    string tmpDecimalPlaces = AppSetting.GetSetting("DecimalPlaces");
                    if (String.IsNullOrEmpty(tmpDecimalPlaces))
                        ffd.Text = "2";
                    else
                        ffd.Text = tmpDecimalPlaces;
                }
                //ffd.Text = "2";
            }
            #endregion
            
            CrystalReportViewer1.ReportSource = Rst;
            CrystalReportViewer1.HasExportButton = true;
            CrystalReportViewer1.RefreshReport();
            #endregion

            return Rst;
        }

        private Control FindControl(string ID)
        {
            foreach (Control ctl in ListOfControl)
            {
                if (ctl.ID.ToLower() == ID.ToLower()) return ctl;
            }
            return null;
        }

        protected void lnkExportPDF_Click(object sender, EventArgs e)
        {
            ReportDocument Tst = BindGrid();
            Tst.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Request.QueryString["r"].Replace(".rpt", "") + " " + DateTime.Now.ToString("dd MMM yyyy HHmmss"));
        }

        protected void lnkExportExcel_Click(object sender, EventArgs e)
        {
            ReportDocument Tst = BindGrid();
            Tst.ExportToHttpResponse(ExportFormatType.Excel, Response, true, Request.QueryString["r"].Replace(".rpt", "") + " " + DateTime.Now.ToString("dd MMM yyyy HHmmss"));
        }

        protected void lnkExportRaw_Click(object sender, EventArgs e)
        {
            ReportDocument Tst = BindGrid();
            Tst.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true, Request.QueryString["r"].Replace(".rpt", "") + " " + DateTime.Now.ToString("dd MMM yyyy HHmmss"));
        }

        private void CreateControl_DateTime(ParameterFieldDefinition DataFieldName, int index, int dayAdd)
        {
            CreateControl_DateTime(DataFieldName, index, dayAdd, null);
        }

        private void CreateControl_DateTime(ParameterFieldDefinition DataFieldName,  int index, int dayAdd, string time)
        {
            #region Create New Row
            if (index % 2 == 0)
            {
                HtmlTableRow Rw = new HtmlTableRow();
                if (light)
                    Rw.Attributes.Add("class", "wl_lightRaw");
                else
                    Rw.Attributes.Add("class", "wl_darkRaw");
                HtmlTableCell Cl;

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                FilterTable.Rows.Insert(1 + (int)Math.Floor((decimal)index / 2), Rw);
                light = !light;
            }
            #endregion

            RadioButton rdbStart = new RadioButton();
            if (DataFieldName.ParameterFieldName == "StartDate")
            {
                rdbStart.ID = "rdb" + DataFieldName.ParameterFieldName;
                rdbStart.GroupName = "seldesel";
                FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[0 + 2 * (index % 2)].Controls.Add(rdbStart);
                rdbStart.Checked = true;

                rdbStart.CheckedChanged += new EventHandler(rdbStart_CheckedChanged);
                rdbStart.AutoPostBack = false;
            }
            Literal Header = new Literal();
            Header.Text = GetHeader(DataFieldName);
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[0 + 2 * (index % 2)].Controls.Add(Header);

            TextBox theBox = new TextBox();
            theBox.ID = DataFieldName.ParameterFieldName;
            theBox.Text = DateTime.Today.AddDays(dayAdd).ToString("dd MMM yyyy");
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(theBox);

            ImageButton theBtn = new ImageButton();
            theBtn.ID = "btn" + DataFieldName.ParameterFieldName;
            theBtn.ImageUrl = "~/App_Themes/Default/image/Calendar_scheduleHS.png";
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(theBtn);

            AjaxControlToolkit.CalendarExtender myCalExt = new AjaxControlToolkit.CalendarExtender();
            myCalExt.ID = "act" + DataFieldName.ParameterFieldName;
            myCalExt.Animated = false;
            myCalExt.Format = "dd MMM yyyy";
            myCalExt.TargetControlID = theBox.ID;
            myCalExt.PopupButtonID = theBtn.ID;
            myCalExt.BehaviorID = "calExt" + DataFieldName.ParameterFieldName;
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(myCalExt);

            ListOfControl.Add(theBox);
            ListOfControl.Add(myCalExt);
            if (DataFieldName.ParameterFieldName == "StartDate")
            {
                ListOfControl.Add(rdbStart);
            }

            //if (DataFieldName.ParameterFieldName == "EndDate")
            //{
            //    Literal ltr = new Literal();
            //    ltr.Text = " <input type='button' value='Show Last Week' onclick='ShowLastWeekDate()'>";
            //    FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(ltr);
            //}

            if (!string.IsNullOrEmpty(time))
            {
                DateTime dat;
                DateTime.TryParse(time, out dat);

                Literal lit = new Literal();
                lit.Text = "<br/>";
                FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(lit);

                DropDownList ddlHour = new DropDownList();
                for (var i = 0; i < 24; i++)
                {
                    ddlHour.Items.Add(new ListItem(i.ToString().PadLeft(2, '0')));
                }
                ddlHour.ID = "hour_" + DataFieldName.ParameterFieldName;
                ddlHour.SelectedValue = dat.Hour.ToString();
                FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(ddlHour);

                DropDownList ddlMinute = new DropDownList();
                for (var i = 0; i < 60; i++)
                {
                    ddlMinute.Items.Add(new ListItem(i.ToString().PadLeft(2, '0')));
                }
                ddlMinute.ID = "minute_" + DataFieldName.ParameterFieldName;
                ddlMinute.Text = dat.Minute.ToString();
                FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(ddlMinute);

                DropDownList ddlSecond = new DropDownList();
                for (var i = 0; i < 60; i++)
                {
                    ddlSecond.Items.Add(new ListItem(i.ToString().PadLeft(2, '0')));
                }
                ddlSecond.ID = "second_" + DataFieldName.ParameterFieldName;
                ddlSecond.Text = dat.Second.ToString();
                FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(ddlSecond);

                ListOfControl.Add(ddlHour);
                ListOfControl.Add(ddlMinute);
                ListOfControl.Add(ddlSecond);
            }

            index++;
        }

        private void CreateControl_LastWeekButton(ParameterFieldDefinition DataFieldName, int index)
        {
            Literal ltr = new Literal();
            ltr.Text = " <input type='button' value='Show Last Week' onclick='ShowLastWeekDate()'>";
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(ltr);

            foreach (Control ctl in ListOfControl)
            {
                if (ctl.ID == "StartDate")
                {
                    DateTime d = DateTime.Now;
                    d = d.AddDays(0 - (d.DayOfWeek - 1));
                    d = d.AddDays(-7);
                    TextBox TxtCtl = (TextBox)ctl;
                    TxtCtl.Text = d.ToString("dd MMM yyyy");
                }
                else if (ctl.ID == "EndDate")
                {
                    DateTime d = DateTime.Now;
                    d = d.AddDays(0 - (d.DayOfWeek - 1));
                    d = d.AddDays(-1);
                    TextBox TxtCtl = (TextBox)ctl;
                    TxtCtl.Text = d.ToString("dd MMM yyyy");
                }
                else if (ctl.ID == "actStartDate")
                {
                    DateTime d = DateTime.Now;
                    d = d.AddDays(0 - (d.DayOfWeek - 1));
                    d = d.AddDays(-7);
                    AjaxControlToolkit.CalendarExtender TxtCtl = (AjaxControlToolkit.CalendarExtender)ctl;
                    TxtCtl.SelectedDate = d;
                }
                else if (ctl.ID == "actEndDate")
                {
                    DateTime d = DateTime.Now;
                    d = d.AddDays(0 - (d.DayOfWeek - 1));
                    d = d.AddDays(-1);
                    AjaxControlToolkit.CalendarExtender TxtCtl = (AjaxControlToolkit.CalendarExtender)ctl;
                    TxtCtl.SelectedDate = d;
                }
            }

            index++;
        }

        //Function Created by Aftab for Start date and end date filter
        //Date 4 Sept 2012

        private void CreateControlDropDown_DateTime(ParameterFieldDefinition DataFieldName, int index)
        {
            #region Create New Row
            if (index % 2 == 0)
            {
                HtmlTableRow Rw = new HtmlTableRow();
                if (light)
                    Rw.Attributes.Add("class", "wl_lightRaw");
                else
                    Rw.Attributes.Add("class", "wl_darkRaw");
                HtmlTableCell Cl;

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                FilterTable.Rows.Insert(1 + (int)Math.Floor((decimal)index / 2), Rw);
                light = !light;
            }
            #endregion

            RadioButton rdb = new RadioButton();
            DropDownList theDropDwon = new DropDownList();
            DropDownList yearDropDown = new DropDownList();
            //Label lbl = new Label();
            if (DataFieldName.ParameterFieldName == "EndDate")
            {
                rdb.ID = "rdbMonth" + DataFieldName.ParameterFieldName;
                rdb.GroupName = "seldesel";
                FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[0 + 2 * (index % 2)].Controls.Add(rdb);
                rdb.CheckedChanged += new EventHandler(rdb_CheckedChanged);
                rdb.AutoPostBack =false;
            }
            Literal Header = new Literal();
            if (DataFieldName.ParameterFieldName == "StartDate")
            {
            }
            else
            {
                Header.Text = Resources.dictionary.Month;
            }
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[0 + 2 * (index % 2)].Controls.Add(Header);

            if (DataFieldName.ParameterFieldName == "EndDate")
            {
                theDropDwon.ID = "ddl" + DataFieldName.ParameterFieldName;
                
                theDropDwon.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(theDropDwon);
                theDropDwon.SelectedIndexChanged += new EventHandler(txtStartDate_TextChanged);
                theDropDwon.AutoPostBack =false;

                //lbl.ID = "lbl" + DataFieldName.ParameterFieldName;
                //lbl.Text = DateTime.Today.AddDays(-1).ToString("yyyy");

                List<ListItem> listYear = new List<ListItem>();
                int minyear = UtilityController.GetMinOrderDateYear();
                for (int i = DateTime.Today.Year; i >= minyear; i--)
                {
                    listYear.Add(new ListItem(i.ToString(), i.ToString()));
                }
                yearDropDown.Items.AddRange(listYear.ToArray());
                yearDropDown.SelectedIndex = 0;
                yearDropDown.ID = "ddlEndYear";
                yearDropDown.AutoPostBack = false;
                FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(yearDropDown);
                DateTime month = new DateTime(1979, 1, 1);
                for (int i = 0; i < 12; i++)
                {
                    DateTime NextMont = month.AddMonths(i);
                    ListItem list = new ListItem();
                    list.Text = NextMont.ToString("MMMM");
                    list.Value = NextMont.Month.ToString();
                    theDropDwon.Items.Add(list);
                }
            }
            
            if (DataFieldName.ParameterFieldName == "EndDate")
            {
                ListOfControl.Add(rdb);
                ListOfControl.Add(theDropDwon);
                ListOfControl.Add(yearDropDown);
            }
            index++;
        }

        private void CreateControl_ComboBox(ParameterFieldDefinition DataFieldName, int index, out DropDownList theBox)
        {
            #region Create New Row
            if (index % 2 == 0)
            {
                HtmlTableRow Rw = new HtmlTableRow();
                if (light)
                    Rw.Attributes.Add("class", "wl_lightRaw");
                else
                    Rw.Attributes.Add("class", "wl_darkRaw");
                HtmlTableCell Cl;

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                FilterTable.Rows.Insert(1 + (int)Math.Floor((decimal)index / 2), Rw);
                light = !light;
            }
            #endregion

            Literal Header = new Literal();
            //Header.Text = DataFieldName.PromptText == "" ? DataFieldName.ParameterFieldName : DataFieldName.PromptText;
            Header.Text = GetHeader(DataFieldName);
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[0 + 2 * (index % 2)].Controls.Add(Header);

            theBox = new DropDownList();
            theBox.ID = DataFieldName.ParameterFieldName;
            theBox.Width = 180;
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(theBox);

            ListOfControl.Add(theBox);

            index++;
        }

        private void CreateControl_ComboBox_VisibleFalse(ParameterFieldDefinition DataFieldName, int index, out DropDownList theBox)
        {
            #region Create New Row
            if (index % 2 == 0)
            {
                HtmlTableRow Rw = new HtmlTableRow();
                if (light)
                    Rw.Attributes.Add("class", "wl_lightRaw");
                else
                    Rw.Attributes.Add("class", "wl_darkRaw");
                HtmlTableCell Cl;

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                FilterTable.Rows.Insert(1 + (int)Math.Floor((decimal)index / 2), Rw);
                light = !light;
            }
            #endregion

            Literal Header = new Literal();
            //Header.Text = DataFieldName.PromptText == "" ? DataFieldName.ParameterFieldName : DataFieldName.PromptText;
            Header.Text = GetHeader(DataFieldName);
            Header.Visible = false;
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[0 + 2 * (index % 2)].Controls.Add(Header);

            theBox = new DropDownList();
            theBox.ID = DataFieldName.ParameterFieldName;
            theBox.Width = 180;
            theBox.Visible = false;
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(theBox);

            ListOfControl.Add(theBox);

            //index++;
        }

        private void CreateControl_ComboBox_POS_WithUpdatePanel(ParameterFieldDefinition DataFieldName,  int index, out DropDownList theBox)
        {
            #region Create New Row
            if (index % 2 == 0)
            {
                HtmlTableRow Rw = new HtmlTableRow();
                if (light)
                    Rw.Attributes.Add("class", "wl_lightRaw");
                else
                    Rw.Attributes.Add("class", "wl_darkRaw");
                HtmlTableCell Cl;

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                FilterTable.Rows.Insert(1 + (int)Math.Floor((decimal)index / 2), Rw);
                light = !light;
            }
            #endregion

            Literal Header = new Literal();
            //Header.Text = DataFieldName.PromptText == "" ? DataFieldName.ParameterFieldName : DataFieldName.PromptText;
            Header.Text = GetHeader(DataFieldName);
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[0 + 2 * (index % 2)].Controls.Add(Header);

            theBox = new DropDownList();
            theBox.ID = DataFieldName.ParameterFieldName;
            theBox.Width = 180;

            UpdatePanel uPanel = new UpdatePanel();
            uPanel.ContentTemplateContainer.Controls.Add(theBox);

            if (ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "Country"; }) != null)
            {
                AsyncPostBackTrigger updateTrigger = new AsyncPostBackTrigger();
                updateTrigger.ControlID = "Country";
                updateTrigger.EventName = "SelectedIndexChanged";
                uPanel.Triggers.Add(updateTrigger);
            }

            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(uPanel);

            ListOfControl.Add(theBox);

            index++;
        }

        private void CreateControl_ComboBox_Country_WithUpdatePanel(ParameterFieldDefinition DataFieldName, int index, out DropDownList theBox)
        {
            #region Create New Row
            if (index % 2 == 0)
            {
                HtmlTableRow Rw = new HtmlTableRow();
                if (light)
                    Rw.Attributes.Add("class", "wl_lightRaw");
                else
                    Rw.Attributes.Add("class", "wl_darkRaw");
                HtmlTableCell Cl;

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                FilterTable.Rows.Insert(1 + (int)Math.Floor((decimal)index / 2), Rw);
                light = !light;
            }
            #endregion

            Literal Header = new Literal();
            //Header.Text = DataFieldName.PromptText == "" ? DataFieldName.ParameterFieldName : DataFieldName.PromptText;
            Header.Text = GetHeader(DataFieldName);
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[0 + 2 * (index % 2)].Controls.Add(Header);

            theBox = new DropDownList();
            theBox.ID = DataFieldName.ParameterFieldName;
            theBox.Width = 180;

            UpdatePanel uPanel = new UpdatePanel();
            uPanel.ContentTemplateContainer.Controls.Add(theBox);

            if (ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "Region"; }) != null)
            {
                AsyncPostBackTrigger updateTrigger = new AsyncPostBackTrigger();
                updateTrigger.ControlID = "Region";
                updateTrigger.EventName = "SelectedIndexChanged";
                uPanel.Triggers.Add(updateTrigger);
            }

            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(uPanel);

            ListOfControl.Add(theBox);

            index++;
        }

        private void CreateControl_String(ParameterFieldDefinition DataFieldName, int index)
        {
            #region Create New Row
            if (index % 2 == 0)
            {
                HtmlTableRow Rw = new HtmlTableRow();
                if (light)
                    Rw.Attributes.Add("class", "wl_lightRaw");
                else
                    Rw.Attributes.Add("class", "wl_darkRaw");
                HtmlTableCell Cl;

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                FilterTable.Rows.Insert(1 + (int)Math.Floor((decimal)index / 2), Rw);
                light = !light;
            }
            #endregion

            Literal Header = new Literal();
            //Header.Text = DataFieldName.PromptText == "" ? DataFieldName.ParameterFieldName : DataFieldName.PromptText;
            Header.Text = GetHeader(DataFieldName);
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[0 + 2 * (index % 2)].Controls.Add(Header);

            TextBox theBox = new TextBox();
            theBox.ID = DataFieldName.ParameterFieldName;
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(theBox);

            ListOfControl.Add(theBox);

            index++;
        }

        public void CreateControl_CheckBox(ParameterFieldDefinition DataFieldName, int index)
        {
            #region Create New Row
            if (index % 2 == 0)
            {
                HtmlTableRow Rw = new HtmlTableRow();
                if (light)
                    Rw.Attributes.Add("class", "wl_lightRaw");
                else
                    Rw.Attributes.Add("class", "wl_darkRaw");
                HtmlTableCell Cl;

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                FilterTable.Rows.Insert(1 + (int)Math.Floor((decimal)index / 2), Rw);
                light = !light;
            }
            #endregion

            Literal Header = new Literal();
            //Header.Text = DataFieldName.PromptText == "" ? DataFieldName.ParameterFieldName : DataFieldName.PromptText;
            Header.Text = GetHeader(DataFieldName);
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[0 + 2 * (index % 2)].Controls.Add(Header);

            CheckBox theBox = new CheckBox();
            theBox.ID = DataFieldName.ParameterFieldName;
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(theBox);

            ListOfControl.Add(theBox);

            index++;
        }

        private void CreateControl_Control(ParameterFieldDefinition DataFieldName, int index, Control Obj)
        {
            #region Create New Row
            if (index % 2 == 0)
            {
                HtmlTableRow Rw = new HtmlTableRow();
                if (light)
                    Rw.Attributes.Add("class", "wl_lightRaw");
                else
                    Rw.Attributes.Add("class", "wl_darkRaw");
                HtmlTableCell Cl;

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "150px";
                // Cl.Height = "3px";
                Cl.Attributes.Add("style", "vertical-align :middle;");
                Rw.Cells.Add(Cl);

                Cl = new HtmlTableCell();
                Cl.Width = "404px";
                Cl.Attributes.Add("style", "vertical-align :middle;left:0px;");
                Rw.Cells.Add(Cl);

                FilterTable.Rows.Insert(1 + (int)Math.Floor((decimal)index / 2), Rw);
                light = !light;
            }
            #endregion

            Literal Header = new Literal();
            //Header.Text = DataFieldName.PromptText == "" ? DataFieldName.ParameterFieldName : DataFieldName.PromptText;
            Header.Text = GetHeader(DataFieldName);
            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[0 + 2 * (index % 2)].Controls.Add(Header);

            FilterTable.Rows[1 + (int)Math.Floor((decimal)index / 2)].Cells[1 + 2 * (index % 2)].Controls.Add(Obj);

            ListOfControl.Add(Obj);

            index++;
        }

        //Event Handlers Created by Aftab for binding hidden text boxes to dropdownlist.
        //Date 4 Sept 2012

        private void txtStartDate_TextChanged(object sender, EventArgs e)
        {
               
                 
            rdb_CheckedChanged(sender, e);
                
             
        }

        private void rdb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = (RadioButton)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "rdbMonthEndDate"; });
            RadioButton rdbStart = (RadioButton)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "rdbStartDate"; });
            TextBox txtStart = (TextBox)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "StartDate"; });
            TextBox txtEnd = (TextBox)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "EndDate"; });
            DropDownList ddlEnd = (DropDownList)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "ddlEndDate"; });
            DropDownList ddlEndYear = (DropDownList)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "ddlEndYear"; });
            //Label lbl = (Label)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "lblEndDate"; });
            if (rdb.Checked == true)
            {
                int month = DateTime.ParseExact(ddlEnd.SelectedItem.Text, "MMMM", CultureInfo.CurrentCulture).Month;
                int year = DateTime.ParseExact(ddlEndYear.SelectedItem.Text, "yyyy", CultureInfo.CurrentCulture).Year;
                int numberOfDays = DateTime.DaysInMonth(year, month);
                DateTime lastDay = new DateTime(year, month, numberOfDays);
                DateTime firstDayOfTheMonth = new DateTime(year, month, 1);
                DateTime last = firstDayOfTheMonth.AddMonths(1).AddDays(-1);
                int firstDay = firstDayOfTheMonth.Day;
                int lastday = last.Day;
                txtStart.Text = firstDay.ToString("0#") + " " + firstDayOfTheMonth.ToString("MMM") + " " + ddlEndYear.SelectedItem.Text;
                txtEnd.Text = lastday.ToString() + " " + lastDay.ToString("MMM") + " " + ddlEndYear.SelectedItem.Text;
                //btnSearch_Click(sender, e);
                BindGrid();
                rdbStart.Checked = false;
            }
        }

        //Event Handlers created by Aftab in order to set start date and end date.
        //Date 06 Sept 2012

        private void rdbStart_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = (RadioButton)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "rdbMonthEndDate"; });
            //TextBox txtStart = (TextBox)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "StartDate"; });
            //TextBox txtEnd = (TextBox)ListOfControl.Find(delegate(Control ctrl) { return ctrl.ID == "EndDate"; });
            //txtStart.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            //txtEnd.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            //btnSearch_Click(sender, e);
            rdb.Checked = false;
        }

        //private void WareHouseSetup()
        //{
        //    WarehouseController.Create_ItemAttributesTable();
        //    WarehouseController.Create_RawData();
        //    //WarehouseController.Compile_RawData();
        //}
    }
}
