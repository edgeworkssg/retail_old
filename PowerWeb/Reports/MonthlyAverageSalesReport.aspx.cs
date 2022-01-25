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
using PowerPOS.Container;

using PowerPOS;
using SubSonic;
using SubSonic.Utilities;


public partial class MonthlyAverageSalesReport : PageBase
{    
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
            ddYears.SelectedValue = DateTime.Today.Year.ToString();
            ViewState["Sort"] = "";
            BindGrid("");
        }
        else
        {
            BindGrid("");
        }
    }

    private void BindGrid(string sort)
    {       
        string currentMonth = DateTime.Now.Month.ToString();
         /*
        DataTable dt = 
            ReportController.FetchAverageSalesAndQuantityPerTransactionReport();
        */
        
        //Create HTML table...
        TableRow tr;
        TableCell tc;
                
        tc = new TableCell();
        tc.Text = "";        

        tr = new TableRow();
        tc = new TableCell();        
        tc.Text = DayOfWeek.Monday.ToString();
        tr.Cells.Add(tc);
        tblReport.Rows.Add(tr);

        tr = new TableRow();
        tc = new TableCell();
        tc.Text = DayOfWeek.Tuesday.ToString();
        tr.Cells.Add(tc);
        tblReport.Rows.Add(tr);

        tr = new TableRow();
        tc = new TableCell();
        tc.Text = DayOfWeek.Wednesday.ToString();
        tr.Cells.Add(tc);
        tblReport.Rows.Add(tr);

        tr = new TableRow();
        tc = new TableCell();
        tc.Text = DayOfWeek.Thursday.ToString();
        tr.Cells.Add(tc);
        tblReport.Rows.Add(tr);

        tr = new TableRow();
        tc = new TableCell();
        tc.Text = DayOfWeek.Friday.ToString(); ;
        tr.Cells.Add(tc);
        tblReport.Rows.Add(tr);

        tr = new TableRow();
        tc = new TableCell();
        tc.Text = DayOfWeek.Saturday.ToString(); 
        tr.Cells.Add(tc);
        tblReport.Rows.Add(tr);

        tr = new TableRow();
        tc = new TableCell();
        tc.Text = DayOfWeek.Sunday.ToString();
        tr.Cells.Add(tc);
        tblReport.Rows.Add(tr);

        int rowcount = 0;
        DateTime myDate = new DateTime(int.Parse(ddYears.SelectedValue), int.Parse(ddlMonth.SelectedValue), 1);
        DataTable dt = ReportController.FetchAverageSalesAndQuantityPerTransactionReport(myDate, "", "OrderDate", "ASC");
        //rowcount = (int)myDate.DayOfWeek;
        switch (myDate.DayOfWeek)
        {
            case DayOfWeek.Monday: rowcount = 0; break;
            case DayOfWeek.Tuesday: rowcount = 1; break;
            case DayOfWeek.Wednesday: rowcount = 2; break;
            case DayOfWeek.Thursday: rowcount = 3; break;
            case DayOfWeek.Friday: rowcount = 4; break;
            case DayOfWeek.Saturday: rowcount = 5; break;
            case DayOfWeek.Sunday: rowcount = 6; break;
        }
        for (int i = 0; i < rowcount; i++)
        {
            tc = new TableCell();
            tc.Text = "";
            tc.BackColor = System.Drawing.Color.Gainsboro;           
            tblReport.Rows[i].Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "-";
            tc.BackColor = System.Drawing.Color.LightBlue;
            tblReport.Rows[i].Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "-";
            tc.BackColor = System.Drawing.Color.LightYellow;
            tblReport.Rows[i].Cells.Add(tc);   
        }
        int k = 0;
        
        while (myDate.Month == int.Parse(ddlMonth.SelectedValue.ToString()))
        {
            tc = new TableCell();
            tc.Text = myDate.Day.ToString();
            tc.BackColor = System.Drawing.Color.Gainsboro;           
            tblReport.Rows[rowcount].Cells.Add(tc);

            
            if (k < dt.Rows.Count)
            {
                decimal AveSales, AveItems;
                if (DateTime.Parse(dt.Rows[k]["OrderDate"].ToString()) == myDate
                    && (decimal.TryParse(dt.Rows[k]["AvgSalesPerTrans"].ToString(), out AveSales) 
                        && (decimal.TryParse(dt.Rows[k]["AvgItemsPerTrans"].ToString(), out AveItems))))
                {                                                          
                    tc = new TableCell();
            
                    tc.Text =String.Format("<a href='ProfitLossReportDaily.aspx?id={0}' target=_blank> $" + AveSales.ToString("N2")
                        + "</a>", myDate.ToString("dd/MMM/yyyy"));
                    
                    
                    
                    tc.BackColor = System.Drawing.Color.LightBlue;
                    tblReport.Rows[rowcount].Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = AveItems.ToString("N2");
                    tc.BackColor = System.Drawing.Color.LightYellow;
                    tblReport.Rows[rowcount].Cells.Add(tc);
            
                    k++;
                }
                else
                {
                    tc = new TableCell();
                    tc.Text = "-";
                    tc.BackColor = System.Drawing.Color.LightBlue;
                    tblReport.Rows[rowcount].Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "-";
                    tc.BackColor = System.Drawing.Color.LightYellow;
                    tblReport.Rows[rowcount].Cells.Add(tc);
            
                }
            }
            else
            {
                tc = new TableCell();
                tc.Text = "-";
                tc.BackColor = System.Drawing.Color.LightBlue;
                tblReport.Rows[rowcount].Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "-";
                tc.BackColor = System.Drawing.Color.LightYellow;
                tblReport.Rows[rowcount].Cells.Add(tc);
            
            }
            myDate = myDate.AddDays(1);
            rowcount += 1;
            if (rowcount == 7) rowcount = 0;
        }               
    }
    /*
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid(ViewState["Sort"].ToString());
        DataView dv = (DataView)gvReport.DataSource;
        DataTable dt = dv.Table;
        
        //CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '));
        // Export the details of specified columns to Excel
        int[] column;
        column = new int[dt.Columns.Count];
        for (int i=0; i < column.Length;i++) 
        {
            column[i] = i;
        }
        string[] header;
        header = new string[dt.Columns.Count];
        //Work around for bug in the export to excel library
        for (int i = 0; i < header.Length; i++) 
        {
            header[i] = dt.Columns[i].ColumnName;
            dt.Columns[i].ColumnName= "col" + i.ToString();
        }

        RKLib.ExportData.Export objExport = new
            RKLib.ExportData.Export("Web");
        
        objExport.ExportDetails(dt,column,header,
             RKLib.ExportData.Export.ExportFormat.CSV, 
             this.Page.Title.Trim(' ') + DateTime.Now.ToString("ddMMMyyyy")
             + ".CSV");
    }
     * */
    protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["Sort"] = e.SortExpression;        
        BindGrid(ViewState["Sort"].ToString());
    }
}
