using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PowerPOS;
using System.Text.RegularExpressions;
using System.Text;
using SpreadsheetLight;
using System.Collections.Generic;

/// <summary>
/// Summary description for CommonWebUILib
/// </summary>
public class CommonWebUILib
{

    public enum MessageType
    {
        GoodNews = 0,
        BadNews = 1
    }
		//
		// TODO: Add constructor logic here
		//

    public static void SetVideoBrowserClientScript(GridViewRowEventArgs e, int DateColNo, int PointOfSaleID)
        {                        
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                /*
                //if (e.Row.Cells[0].Controls[0] is LinkButton) 
                //{
                    //LOAD CAMERA INFORMATION BASED ON PointOfSale ID                    
                    CameraCollection cmr = new CameraCollection();
                    
                    cmr.Where(Camera.Columns.PointOfSaleID, PointOfSaleID);
                    cmr.Load();
                    DateTime dt = DateTime.Parse(e.Row.Cells[DateColNo].Text.ToString());
                    for (int i = 0; i < cmr.Count; i++)
                    {
                        e.Row.Cells[0].Text += "<input type=\"button\" value=\"" + (cmr[i].CameraNo) + "\" onclick=\"javascript:popup_window=window.open('http://" 
                        + cmr[i].CameraIp + "/PowerPOSWeb/viewVideo.html?year=" +
                        + dt.Year + "&month=" + dt.Month + "&day=" 
                        + dt.Day + "&hour=" + dt.Hour + "&minute=" + dt.Minute + "&camera=" + (cmr[i].CameraNo - 1) +
                        "','popup_window','width=580,height=430,top=2,left=2,toolbar=no,location=no,directories=no,menubar=no,resizable=no');popup_window.focus()\">";
                    }

                    
                    /*
                    DateTime dt = DateTime.Parse(e.Row.Cells[DateColNo].Text.ToString());
                    ((LinkButton)e.Row.Cells[0].Controls[0]).OnClientClick
                    = "javascript:popup_window=window.open('../viewVideo.html?year=" 
                    + dt.Year + "&month=" + dt.Month + "&day=" 
                    + dt.Day + "&hour=" + dt.Hour + "&minute=" + dt.Minute
                    + "','popup_window','width=580,height=430,top=2,left=2,toolbar=no,location=no,directories=no,menubar=no,resizable=no');popup_window.focus()";*/
                //}            */
            }        
        }

    public static void ShowMessage(Label lblErrorMsg, string message, MessageType messageType)
    {
            switch( messageType) {
                case MessageType.GoodNews:
                    //Show error message!
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    lblErrorMsg.Text = message;
                    break;
                case MessageType.BadNews:
                    //Show error message!
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Text = message;
                    break;
            }
    }
    public static void ExportCSV(DataTable dat, string filename, string title, GridView gv)
    {
        filename = filename.Replace(' ', '_');
        // Export the details of specified columns to Excel
        RKLib.ExportData.Export objExport = new
            RKLib.ExportData.Export("Web");

        #region *) Trim Column Name
        /* Somehow, all the columns with space behind, are not shown on CSV */
        for (int Ctr = 0; Ctr < dat.Columns.Count; Ctr++)
        {
            dat.Columns[Ctr].ColumnName = dat.Columns[Ctr].ColumnName.Trim();
        }
        #endregion
        
        DataTable dt;
        DataRow dr;
        
        CopyDataTableToString(dat, out dt);
                
        ArrayList ColumnList = new ArrayList();
        ArrayList Headers = new ArrayList();
        
        if (gv.ShowFooter == true)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);
        }

        //Create New Row for headers
        int colIndex=0;
        dt.Rows.InsertAt(dt.NewRow(),0);
        int Counter1 = 0;
        for (int i = 0; i < gv.Columns.Count; i++)
        {
            if (gv.Columns[i] is BoundField && gv.Columns[i].Visible)
            {
                string dataField = ((BoundField)gv.Columns[i]).DataField;
                if (dataField != "")
                {
                    DataColumn tmp = dt.Columns[dataField];

                    Headers.Add("");
                    dt.Rows[0][dataField] = gv.Columns[i].HeaderText;

                    //Create footer
                    if (gv.FooterRow != null && gv.FooterRow.Cells.Count > 0 && gv.ShowFooter)
                        dt.Rows[dt.Rows.Count - 1][dataField] = gv.FooterRow.Cells[i].Text.
                           Replace("%", "").Replace("$", "").Replace("&nbsp;", ""); ;
                    tmp.SetOrdinal(Counter1++);
                    ColumnList.Add(colIndex);
                    colIndex++;
                }
            }
        }        
       
        //Create Title        
        dt.Rows.InsertAt(dt.NewRow(), 0);
        dt.Rows[0][0] = "Generated on " + DateTime.Now;
        dt.Rows.InsertAt(dt.NewRow(),0);
        dt.Rows[0][0] = title; 

        objExport.ExportDetails(dt,(int[])ColumnList.ToArray(typeof(int)),
            (string[])Headers.ToArray(typeof(string)),
             RKLib.ExportData.Export.ExportFormat.CSV,
             filename + DateTime.Now.ToString("ddMMMyyyy")
             + ".csv");
    }
    private static void CopyDataTableToString(DataTable dat, out DataTable dt)
    {
        dt = new DataTable("");
        //Copy columns
        for (int i = 0; i < dat.Columns.Count; i++)
        {
            dt.Columns.Add(dat.Columns[i].ColumnName);
        }
        DataRow dr;

        //Copy rows
        for (int j = 0; j < dat.Rows.Count; j++)
        {
            dr = dt.NewRow();
            for (int i = 0; i < dat.Columns.Count; i++)
            {
                dr[i] = dat.Rows[j][i].ToString();
            }
            dt.Rows.Add(dr);
        }

        return;
    }
    public static void ExportCSVWithInvinsibleFields(DataTable dat, string filename, string title, GridView gv)
    {
        filename = filename.Replace(' ', '_');
        // Export the details of specified columns to Excel
        RKLib.ExportData.Export objExport = new
            RKLib.ExportData.Export("Web");

        #region *) Trim Column Name
        /* Somehow, all the columns with space behind, are not shown on CSV */
        for (int Counter = 0; Counter < dat.Columns.Count; Counter++)
        {
            dat.Columns[Counter].ColumnName = dat.Columns[Counter].ColumnName.Trim();
        }
        #endregion

        DataTable dt;
        DataRow dr;
        
        CopyDataTableToString(dat, out dt);
        
        //int ColumnList[], string Headers[]
        ArrayList ColumnList = new ArrayList();
        ArrayList Headers = new ArrayList();
        
        if (gv.ShowFooter == true)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);
        }

        //Create New Row for headers
        dt.Rows.InsertAt(dt.NewRow(),0);
        int colIndex = 0;       
        for (int i = 0; i < gv.Columns.Count; i++)
        {
            if (gv.Columns[i] is BoundField)
            {
                string dataField = ((BoundField)gv.Columns[i]).DataField;
                if (dataField != "")
                {
                    DataColumn tmp = dt.Columns[dataField];

                    Headers.Add("");
                    dt.Rows[0][dataField] = gv.Columns[i].HeaderText;

                    //Create footer
                    if (gv.FooterRow != null && gv.FooterRow.Cells.Count > 0 && gv.ShowFooter)
                        dt.Rows[dt.Rows.Count - 1][dataField] = gv.FooterRow.Cells[i].Text.
                           Replace("%", "").Replace("$", "").Replace("&nbsp;", ""); ;
                    tmp.SetOrdinal(colIndex);
                    ColumnList.Add(colIndex);
                    colIndex++;
                }
            }
        }        

        //Create Title        
        dt.Rows.InsertAt(dt.NewRow(), 0);
        dt.Rows[0][0] = "Generated on " + DateTime.Now;
        dt.Rows.InsertAt(dt.NewRow(),0);
        dt.Rows[0][0] = title;

        // Fix for conflicting column header and xpath (Low Quantity Items)
        if (title == "Low Quantity Items")
        {
            if (dt.Columns["On Hand"] != null)
            {
                dt.Columns["On Hand"].ColumnName = "OnHand";
            }
        }
        Regex rgx = new Regex("[^a-zA-Z0-9]");
        //str = rgx.Replace(str, "");

        for (int i = 0; i < dt.Columns.Count; i++)
        {
            //dt.Columns[i].ColumnName = dt.Columns[i]
            //                             .ColumnName
            //                             .Replace(" ", "_")
            //                             .Replace("(", "")
            //                             .Replace(")", "")
            //                             .Replace("<", "")
            //                             .Replace(">", "")
            //                             .Replace("\'", "")
            //                             .Replace("\"", "")
            //                             .Replace("&", "");
            dt.Columns[i].ColumnName = "_"+rgx.Replace(dt.Columns[i].ColumnName, "");
        }
        objExport.ExportDetails(dt,(int[])ColumnList.ToArray(typeof(int)),
            (string[])Headers.ToArray(typeof(string)),
             RKLib.ExportData.Export.ExportFormat.CSV,
             filename + DateTime.Now.ToString("ddMMMyyyy")
             + ".csv");
    }
    public static void ExportCSVAllColumns(DataTable dat, string filename, string title)
    {
        #region *) Set up File Name 
        filename = filename.Replace(' ', '_');
        #endregion

        #region *) Export the details of specified columns to Excel
        RKLib.ExportData.Export objExport = new
            RKLib.ExportData.Export("Web");
        #endregion

        #region *) Trim Column Name 
        /* Somehow, all the columns with space behind, are not shown on CSV */
        for (int Counter = 0; Counter < dat.Columns.Count; Counter++)
        {
            dat.Columns[Counter].ColumnName = dat.Columns[Counter].ColumnName.Replace("(", " ").Replace(")", "").Replace(" ", "_").Replace("-", "").Replace("/", "");
            if (!char.IsLetter(dat.Columns[Counter].ColumnName, 0))
                dat.Columns[Counter].ColumnName = "_" + dat.Columns[Counter].ColumnName;
        }
        #endregion

        DataTable dt;
        //DataRow dr;

        CopyDataTableToString(dat, out dt);

        //int ColumnList[], string Headers[]
        ArrayList ColumnList = new ArrayList();
        ArrayList Headers = new ArrayList();

        //Create New Row for headers
        dt.Rows.InsertAt(dt.NewRow(), 0);

        for (int j = 0; j < dt.Columns.Count; j++)
        {
            ColumnList.Add(j);
            Headers.Add("");
            dt.Rows[0][j] = dt.Columns[j].ColumnName;
        }

        //Create Title        
        dt.Rows.InsertAt(dt.NewRow(), 0);
        dt.Rows[0][0] = "Generated on " + DateTime.Now;
        dt.Rows.InsertAt(dt.NewRow(), 0);
        dt.Rows[0][0] = title;

        objExport.ExportDetails(dt, (int[])ColumnList.ToArray(typeof(int)),
            (string[])Headers.ToArray(typeof(string)),
             RKLib.ExportData.Export.ExportFormat.CSV,
             filename + DateTime.Now.ToString("ddMMMyyyy")
             + ".csv");
    }

    public static void ExportCSV(string filename, string title, GridView gv, HttpResponse Response)
    {
        filename = filename.Replace(' ', '_');

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
         "attachment;filename=" + filename + "_" + DateTime.Now.ToString("ddMMMyyyy") + ".csv");
        Response.Charset = "";
        Response.ContentType = "application/text";

        gv.AllowPaging = false;
        gv.DataBind();

        StringBuilder sb = new StringBuilder();

        sb.AppendLine(title);
        sb.AppendLine("Generated on " + DateTime.Now);

        sb.Append("\r\n");

        for (int k = 0; k < gv.Columns.Count; k++)
        {
            if (gv.Columns[k].Visible)
            {
                //add separator
                sb.Append(gv.Columns[k].HeaderText + ',');
            }
        }
        //append new line
        sb.Append("\r\n");
        for (int i = 0; i < gv.Rows.Count; i++)
        {
            for (int k = 0; k < gv.Columns.Count; k++)
            {
                if (gv.Columns[k].Visible)
                {
                    //add separator
                    if (gv.Columns[k].AccessibleHeaderText != "")
                    {
                        Label lbl = (Label)gv.Rows[i].FindControl(gv.Columns[k].AccessibleHeaderText);
                        if (lbl != null)
                            sb.Append("\"" + lbl.Text + "\"" + ',');
                    }
                    else
                        sb.Append("\"" + gv.Rows[i].Cells[k].Text + "\"" + ',');
                }
            }
            //append new line
            sb.Append("\r\n");
        }

        for (int k = 0; k < gv.Columns.Count; k++)
        {
            if (gv.Columns[k].Visible)
            {
                //add separator
                if (gv.Columns[k].AccessibleHeaderText != "")
                {
                    Label lbl = (Label)gv.FooterRow.FindControl(gv.Columns[k].AccessibleHeaderText);
                    if (lbl != null)
                        sb.Append("\"" + lbl.Text + "\"" + ',');
                    else
                    {
                        sb.Append(",");
                    }
                }
                else
                {
                    sb.Append("\"" + gv.FooterRow.Cells[k].Text.Replace("&nbsp;", "") + "\",");
                }
            }
        }

        Response.Output.Write(sb.ToString());
        Response.Flush();
        Response.End();
    }

    public static void ExportXLSX(string fileName, DataTable dt, GridView gv, HttpResponse Response, Dictionary<String, String> Params)
    {
        fileName = fileName.Replace(" ", "_");
        SLDocument sl = new SLDocument();
        var style = sl.CreateStyle();
        style.FormatCode = "###";
        int iStartRowIndex = 1;
        #region insert params

        sl.SetCellValue("A" + iStartRowIndex, fileName);
        iStartRowIndex++;

        foreach (var entry in Params)
        {
            //entry.Key and entry.Value
            sl.SetCellValue("A" + iStartRowIndex, entry.Key);
            sl.SetCellValue("B" + iStartRowIndex, entry.Value);
            iStartRowIndex++;
        }

        #endregion




        int iStartColumnIndex = 1;
        sl.SetColumnStyle(5, style);
        sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, dt, true);
        int iEndRowIndex = iStartRowIndex + dt.Rows.Count + 1 - 1;
        int iEndColumnIndex = iStartColumnIndex + dt.Columns.Count - 1;
        SLTable table = sl.CreateTable(iStartRowIndex, iStartColumnIndex, iEndRowIndex, iEndColumnIndex);
        table.SetTableStyle(SLTableStyleTypeValues.Medium2);
        table.HasTotalRow = false;
        sl.InsertTable(table);

        sl.AutoFitColumn(2, iEndColumnIndex);

        //sl.FreezePanes(1, 0);

        /*if (dt.Columns[dt.Columns.Count - 1].ColumnName == "Status")
        {
            SLStyle styleWarp = sl.CreateStyle();
            styleWarp.SetWrapText(true);
            sl.SetColumnStyle(dt.Columns.Count, styleWarp);
        }*/


        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "_Result.xlsx");
        sl.SaveAs(Response.OutputStream);
        Response.End();
    }

    public static string ExportText(DataTable dat, GridView gv)
    {
        StringBuilder sb = new StringBuilder();

        #region *) Trim Column Name
        /* Header */

        //for (int Ctr = 0; Ctr < gv.Columns.Count; Ctr++)
        //{
        //    if (gv.Columns[Ctr] is BoundField)
        //    {
        //        string dataField = ((BoundField)gv.Columns[Ctr]).DataField;
        //        if (dataField != "")
        //        {

        //            if (Ctr == dat.Columns.Count - 1)
        //            {
        //                sb.Append(gv.Columns[Ctr].HeaderText.Trim());
        //                sb.Append(Environment.NewLine);
        //            }
        //            else
        //            {
        //                sb.Append(gv.Columns[Ctr].HeaderText.Trim() + "|");
        //            }
        //        }
        //    }
        //}
        #endregion

        for (int i = 0; i < dat.Rows.Count; i++)
        {
            for (int j = 0; j < dat.Columns.Count; j++)
            {
                if (j == dat.Columns.Count - 1)
                {
                    sb.Append(dat.Rows[i][j].ToString());

                }
                else
                {
                    sb.Append(dat.Rows[i][j].ToString() + "|");
                }
            }
            sb.Append(Environment.NewLine);
        }

        return sb.ToString();
    }

}
	

