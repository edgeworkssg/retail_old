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
using PowerPOS;
using PowerPOS.Container;
using SubSonic;
public partial class MembershipProductSalesReport: PageBase
{    
    ItemCollection col;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack)
        {
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtStartExpiryDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndExpiryDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtStartBirthDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndBirthDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            col = new ItemCollection();
            ViewState["Controller"] = col;                                  
            
            txtItemBarcode.Focus();
        }
        BindGrid();
    }

    #region "Add Item to table"
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (col == null) col = (ItemCollection)ViewState["Controller"];
        Item it = new Item(Item.Columns.Barcode,txtItemBarcode.Text);
        if (it.IsNew || !it.IsLoaded)
        {
            CommonWebUILib.ShowMessage(lblErrorMsg, "Item barcode " + txtItemBarcode.Text + " does not exist. ", CommonWebUILib.MessageType.BadNews);
            return;
        }
        col.Add(it);        
        BindGrid();
    }
    protected void btnOkItemNo_Click(object sender, EventArgs e)
    {
        if (col == null) col = (ItemCollection)ViewState["Controller"];

        Item it = new Item(txtItemNo.Text);
        if (it.IsNew || !it.IsLoaded)
        {
            CommonWebUILib.ShowMessage(lblErrorMsg, "Item No:" + txtItemNo.Text + " does not exist. ", CommonWebUILib.MessageType.BadNews);
            return;
        }
        col.Add(it);
        BindGrid();
    }
    protected void ddlName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (col == null) col = (ItemCollection)ViewState["Controller"];

        Item it = new Item(ddlName.SelectedValue);
        if (it.IsNew || !it.IsLoaded)
        {
            CommonWebUILib.ShowMessage(lblErrorMsg, "Item No:" + ddlName.SelectedValue + " does not exist. ", CommonWebUILib.MessageType.BadNews);
            return;
        }
        col.Add(it);
        BindGrid();          
    }
    #endregion

    #region "Submit/Cancel"

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (gvDetails.Rows.Count == 0)
        {
            CommonWebUILib.ShowMessage(lblErrorMsg, "No products/courses has been specified.", CommonWebUILib.MessageType.BadNews);
            return;
        }
        //Construct Item Array List
        ArrayList itemList = new ArrayList();
        ArrayList itemNames = new ArrayList();
        for (int i = 0; i < gvDetails.Rows.Count; i++)
        {
            itemList.Add(gvDetails.Rows[i].Cells[1].Text);
            itemNames.Add(gvDetails.Rows[i].Cells[2].Text);
        }

        DateTime startSalesDate = new DateTime(1979, 11, 3);
        DateTime endSalesDate = new DateTime(1979, 11, 3);
        DateTime startExpiryDate = new DateTime(1979, 11, 3);
        DateTime endExpiryDate = new DateTime(1979, 11, 3);
        DateTime startBirthDate = new DateTime(1979, 11, 3);
        DateTime endBirthDate = new DateTime(1979, 11, 3);

        if (cbFilterByDate.Checked &&
            !(DateTime.TryParse(txtStartDate.Text, out startSalesDate) & 
            DateTime.TryParse(txtEndDate.Text, out endSalesDate)))
        {
            CommonWebUILib.ShowMessage(lblErrorMsg, "Please specify the correct start sales date and end sales date.", CommonWebUILib.MessageType.BadNews);
            return;
        }

        if (cbUseStartExpiryDate.Checked && 
            !DateTime.TryParse(txtStartExpiryDate.Text, out startExpiryDate))
        {
            CommonWebUILib.ShowMessage(lblErrorMsg, "Please specify the correct start subcription date.", CommonWebUILib.MessageType.BadNews);
            return;
        }

        if (cbUseEndExpiryDate.Checked && 
            !DateTime.TryParse(txtEndExpiryDate.Text, out endExpiryDate))
        {
            CommonWebUILib.ShowMessage(lblErrorMsg, "Please specify the correct end subscription date.", CommonWebUILib.MessageType.BadNews);
            return;
        }

        if (cbUseStartBirthDate.Checked && !DateTime.TryParse(txtStartBirthDate.Text, out startBirthDate))
        {
            CommonWebUILib.ShowMessage(lblErrorMsg, "Please specify the correct start birth date.", CommonWebUILib.MessageType.BadNews);
            return;
        }
        if (cbUseEndBirthDate.Checked && !DateTime.TryParse(txtEndBirthDate.Text, out endBirthDate))
        {
            CommonWebUILib.ShowMessage(lblErrorMsg, "Please specify the correct end birth date.", CommonWebUILib.MessageType.BadNews);
            return;
        }

        //Generate Report
        DataTable dt = ReportController.FetchMembershipProductSalesReport
            (cbFilterByDate.Checked, cbFilterByDate.Checked,startSalesDate, endSalesDate, itemList,
             cbUseStartExpiryDate.Checked, cbUseEndExpiryDate.Checked, startExpiryDate, endExpiryDate,
             cbUseStartBirthDate.Checked, cbUseEndBirthDate.Checked, startBirthDate, endBirthDate,
             cbUseBirthDayMonth.Checked,int.Parse(ddlMonth.SelectedValue.ToString()), txtMembershipNo.Text,
             int.Parse(ddGroupName.SelectedValue), txtNRIC.Text, ddlGender.SelectedValue,
             txtNameToAppear.Text,txtStreetName.Text, "MembershipNo", "ASC");
        

        //open a new window with the datatable passed out as the 
        dt.Columns[0].Caption = "Card No";
        dt.Columns[0].Caption = "Card No";

        Session["Report"] = dt;
        Session["ReportRemark"] = "<b>LEGEND:</b> <BR />";
        Session["ReportRemark"] += "<table>";
        for (int p = 0; p < itemList.Count; p++)
        {
            Session["ReportRemark"] += "<tr><td>" + itemList[p].ToString() + "&nbsp;&nbsp;&nbsp;</td><td>" + itemNames[p].ToString() + "</td></tr>";
        }
        Session["ReportRemark"] += "</table>";
        col = new ItemCollection();
        ViewState["Controller"] = col;   
        Response.Redirect("../Reports/ViewReport.aspx");
    }
    
    protected void btnClear_Click(object sender, EventArgs e)
    {
        col = new ItemCollection();
        ViewState["Controller"] = col;        
        ClearControls();
        BindGrid();

    }

    private void ClearControls()
    {        
        txtItemBarcode.Text = "";        
        //txtRemark.Text = "";        
        ddlName.SelectedIndex = 0;
    }

    #endregion

    #region "Detail GridView Events"
    private void BindGrid()
    {     
        if (col == null) col = (ItemCollection)ViewState["Controller"];
        gvDetails.DataSource = col.ToDataTable();
        gvDetails.DataBind();
        txtItemNo.Text = "";
        txtItemBarcode.Text = "";
    }    
    
    protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {        
        if (col == null) col = (ItemCollection)ViewState["Controller"];
        col.RemoveAt(e.RowIndex);
        BindGrid();
    }
    
    
    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {        
        }
    }
    #endregion

    protected void ddLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }           
}
