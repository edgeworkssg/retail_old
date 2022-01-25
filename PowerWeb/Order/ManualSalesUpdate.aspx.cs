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
using PowerPOS;
using SubSonic;
using MKB.TimePicker;
using System.Collections.Generic;

namespace PowerWeb.Order
{
    public partial class ManualSalesUpdate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetFormSetting();
            lblStatus.Text = "";
            if (!this.IsPostBack)
                BindGrid();
        }

        private void SetFormSetting()
        {
            try
            {
                string posText = LabelController.PointOfSaleText;
                string outletText = LabelController.OutletText;
                gvReport.Columns[1].HeaderText = outletText + " Code";
                gvReport.Columns[2].HeaderText = posText + " Code";
                lblPOS.Text = posText;
                lblOutlet.Text = outletText;
                UserMst um = new UserMst(Session["UserName"] + "");
                if (um.IsHavePrivilegesFor("Manual Sales Update"))
                    this.Page.Title = "Manual Sales Update";
                else
                    this.Page.Title = "Manual Sales Submission";

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void BindGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Days", typeof(int));
            dt.Columns.Add("MallCode", typeof(string));
            dt.Columns.Add("TenantCode", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Hour", typeof(string));
            dt.Columns.Add("TransactionCount", typeof(int));
            dt.Columns.Add("TotalSalesAfterTax", typeof(decimal));
            dt.Columns.Add("TotalSalesBeforeTax", typeof(decimal));
            dt.Columns.Add("TotalTax", typeof(decimal));
            dt.Columns.Add("Remarks", typeof(string));
            dt.Columns.Add("POSID", typeof(string));

            DateTime theDate = new DateTime(ddlYear.SelectedItem.Text.GetIntValue()
                , ddlMonth.SelectedValue.GetIntValue(), 1).AddMonths(1).AddDays(-1);
            int noOfDay = theDate.Day;

            PointOfSale pos = new PointOfSale(PointOfSale.Columns.PointOfSaleID, (ddlTenant.SelectedValue + "").GetIntValue());
            Outlet ou = new Outlet(Outlet.Columns.OutletName, (ddlOutlet.SelectedValue + "").ToString());
            if (pos.IsNew || pos.BusinessStartDate == null || pos.BusinessEndDate == null)
            {
                gvReport.DataSource = null;
                gvReport.DataBind();
                return;
            }
            else
            {
                ou = pos.Outlet;
            }
            string posIDPrefix = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.POSIDPrefix);
            for (int i = 0; i < noOfDay; i++)
            {
                DateTime runningDate = new DateTime(theDate.Year, theDate.Month, (i + 1));

                if (runningDate.Date < pos.BusinessStartDate.GetValueOrDefault(DateTime.Now).Date
                    || runningDate.Date > pos.BusinessEndDate.GetValueOrDefault(DateTime.Now).Date
                    || runningDate.Date > DateTime.Now.Date)
                {
                    continue;
                }

                var query = new Query("ManualSalesUpdate");
                query.AddWhere(PowerPOS.ManualSalesUpdate.Columns.PointOfSaleID, Comparison.Equals, pos.PointOfSaleID);
                query.AddWhere(PowerPOS.ManualSalesUpdate.Columns.DateX, Comparison.Equals, theDate.ToString("yyyy-MM") + "-" + (i + 1).ToString("00"));
                PowerPOS.ManualSalesUpdate msu = new PowerPOS.ManualSalesUpdateController().FetchByQuery(query).FirstOrDefault();
                if (msu == null)
                    msu = new PowerPOS.ManualSalesUpdate();

                var newRow = dt.NewRow();
                newRow["Days"] = (i + 1);
                newRow["MallCode"] = ou.MallCode;
                newRow["TenantCode"] = pos.TenantMachineID;
                newRow["Date"] = theDate.ToString("yyyy-MM")+"-" + (i + 1).ToString("00");
                newRow["Hour"] = "";
                newRow["TransactionCount"] = msu.TransactionCount.GetValueOrDefault(0).ToString();
                newRow["TotalSalesAfterTax"] = msu.TotalSalesAfterTax.GetValueOrDefault(0).ToString();
                newRow["TotalSalesBeforeTax"] = msu.TotalSalesBeforeTax.GetValueOrDefault(0).ToString();
                newRow["TotalTax"] = msu.TotalTax.GetValueOrDefault(0).ToString();
                newRow["Remarks"] = msu.Remarks;
                newRow["POSID"] = pos.PointOfSaleID.ToString();

                dt.Rows.Add(newRow);
            }
            gvReport.DataSource = dt;
            gvReport.DataBind();
            UserMst um = new UserMst(Session["UserName"] + "");
            gvReport.Columns[9].Visible = um.IsHavePrivilegesFor("Manual Sales Update");
        }

        protected void ddlOutlet_Init(object sender, EventArgs e)
        {
            try
            {
                ddlOutlet.DataSource = OutletController.FetchByUserNameForReport(false, false, (Session["UserName"] + ""));
                ddlOutlet.DataBind();
                ddlOutlet.SelectedIndex = 0;
                ddlOutlet_SelectedIndexChanged(ddlOutlet, new EventArgs());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void ddlOutlet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlTenant.DataSource = PointOfSaleController.FetchByUserNameForReport(false, false, (Session["UserName"] + ""), ddlOutlet.SelectedValue);
                ddlTenant.DataBind();
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void ddlYear_Init(object sender, EventArgs e)
        {
            try
            {
                ddlYear.Items.Clear();
                for (int i = DateTime.Now.Year - 5; i < DateTime.Now.Year + 5; i++)
                {
                    ddlYear.Items.Add(new ListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = i == DateTime.Now.Year
                    });
                }
                ddlMonth.SelectedIndex = (DateTime.Now.Month - 1);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> listStatus = new List<string>();
                QueryCommandCollection qmc = new QueryCommandCollection();
                string theTenantCode = "";
                int thePointOfSaleID = 0;
                for (int i = 0; i < gvReport.Rows.Count; i++)
                {
                    TimeSelector tsTime = (TimeSelector)(gvReport.Rows[i].Cells[4].Controls[1]);
                    TextBox txtTransactionCount = (TextBox)(gvReport.Rows[i].Cells[5].Controls[1]);
                    TextBox txtTotalSalesAfterTax = (TextBox)(gvReport.Rows[i].Cells[6].Controls[1]);
                    TextBox txtTotalSalesBeforeTax = (TextBox)(gvReport.Rows[i].Cells[7].Controls[1]);
                    TextBox txtTotalTax = (TextBox)(gvReport.Rows[i].Cells[8].Controls[1]);
                    TextBox txtRemarks = (TextBox)(gvReport.Rows[i].Cells[9].Controls[1]);

                    string txtDate = gvReport.Rows[i].Cells[3].Text;
                    string posID = gvReport.Rows[i].Cells[10].Text;
                    string mallCode = gvReport.Rows[i].Cells[1].Text;
                    string tenantCode = gvReport.Rows[i].Cells[2].Text;
                    string strDate = txtDate+" "+
                                     tsTime.Hour.ToString("00") +
                                     ":" +
                                     tsTime.Minute.ToString("00") +
                                     ":00 " +
                                     tsTime.AmPm;
                    if(string.IsNullOrEmpty(theTenantCode))
                        theTenantCode = tenantCode;
                    if (thePointOfSaleID == 0)
                        thePointOfSaleID = posID.GetIntValue();
                    DateTime orderDate = strDate.GetDateTimeValue("yyyy-MM-dd hh:mm:ss tt");
                    int transactionCount = txtTransactionCount.Text.GetIntValue();
                    decimal totalSalesAfterTax = txtTotalSalesAfterTax.Text.GetDecimalValue();
                    decimal totalSalesBeforeTax = txtTotalSalesBeforeTax.Text.GetDecimalValue();
                    decimal totalTax = txtTotalTax.Text.GetDecimalValue();
                    string remarks = txtRemarks.Text;
                    var query = new Query("ManualSalesUpdate");
                    query.AddWhere(PowerPOS.ManualSalesUpdate.Columns.PointOfSaleID, Comparison.Equals, posID);
                    query.AddWhere(PowerPOS.ManualSalesUpdate.Columns.DateX, Comparison.Equals, txtDate);
                    PowerPOS.ManualSalesUpdate msu = new PowerPOS.ManualSalesUpdateController().FetchByQuery(query).FirstOrDefault();
                    if (msu == null)
                    {
                        msu = new PowerPOS.ManualSalesUpdate();
                        msu.PointOfSaleID = posID.GetIntValue();
                        msu.MallCode = mallCode;
                        msu.DateX = orderDate.ToString("yyyy-MM-dd");
                        msu.Hour = orderDate.Date.ToString("hh:mm:ss tt");
                    }
                    msu.TenantCode = tenantCode;
                    msu.TransactionCount = transactionCount;
                    msu.TotalSalesAfterTax = totalSalesAfterTax;
                    msu.TotalSalesBeforeTax = totalSalesBeforeTax;
                    msu.TotalTax = totalTax;
                    msu.Remarks = remarks;
                    msu.Deleted = false;
                    if (msu.IsNew)
                        qmc.Add(msu.GetInsertCommand(Session["UserName"] + ""));
                    else
                        qmc.Add(msu.GetUpdateCommand(Session["UserName"] + ""));
                }
                DataService.ExecuteTransaction(qmc);
                AccessLogController.AddTenantHistory(theTenantCode, thePointOfSaleID, AccessSource.WEB, (Session["UserName"] + ""), string.Format("Manual sales update for {0} {1} success", ddlMonth.SelectedItem.Text, ddlYear.SelectedItem.Text));

                BindGrid();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error : " + ex.Message;
                AccessLogController.AddTenantHistory("", (ddlTenant.SelectedValue + "").GetIntValue(), AccessSource.WEB, (Session["UserName"] + ""), string.Format("Manual sales update for {0} {1} error : {2}", ddlMonth.SelectedItem.Text, ddlYear.SelectedItem.Text, ex.Message));
                Logger.writeLog(ex);
            }
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UserMst um = new UserMst(Session["UserName"] + "");
                bool isHavePrivileges = um.IsHavePrivilegesFor("Manual Sales Update");
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtTransactionCount = (TextBox)e.Row.Cells[5].Controls[1];
                    TextBox txtTotalSalesAfterTax = (TextBox)e.Row.Cells[6].Controls[1];
                    TextBox txtTotalSalesBeforeTax = (TextBox)e.Row.Cells[7].Controls[1];
                    TextBox txtTotalTax = (TextBox)e.Row.Cells[8].Controls[1];
                    TextBox txtRemarks = (TextBox)e.Row.Cells[9].Controls[1];

                    //txtTransactionCount.Text = txtTransactionCount.Text.Replace(",", "");
                    //txtTotalSalesBeforeTax.Text = txtTotalSalesBeforeTax.Text.Replace(",", "");
                    //txtTotalSalesBeforeTax.Text = txtTotalSalesBeforeTax.Text.Replace(",", "");
                    //txtTotalTax.Text = txtTotalTax.Text.Replace(",", "");

                    int cutOffDate = (AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.CutOffDate) + "").GetIntValue();
                    
                    DateTime theDate = e.Row.Cells[3].Text.GetDateTimeValue("yyyy-MM-dd");
                    theDate = new DateTime(theDate.Year, theDate.Month, 1);
                    DateTime nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                    //if (cutOffDate != 0 && (DateTime.Now.Day > cutOffDate || (theDate.Date < nowDate.Date)))
                    if(cutOffDate!=0)
                    {
                        bool isEnabled = true;
                        if (theDate.Date < nowDate.AddMonths(-1).Date)
                            isEnabled = false;
                        if (DateTime.Now.Day >= cutOffDate && theDate.Date < nowDate.Date)
                            isEnabled = false;

                        if (!isEnabled)
                        {
                            txtTransactionCount.Enabled = isHavePrivileges;
                            txtTotalSalesAfterTax.Enabled = isHavePrivileges;
                            txtTotalSalesBeforeTax.Enabled = isHavePrivileges;
                            txtTotalTax.Enabled = isHavePrivileges;
                            txtRemarks.Enabled = isHavePrivileges;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
    }
}
