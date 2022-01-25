using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PowerPOS;
using SpreadsheetLight;

namespace PowerInventory
{
    public static class Helper
    {
        public static bool ExportExcel(DataTable dt, string fileName, out string status)
        {
            status = "";
            try
            {

                SLDocument sl = new SLDocument();

                int iStartRowIndex = 1;
                int iStartColumnIndex = 1;
                sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, dt, true);
                int iEndRowIndex = iStartRowIndex + dt.Rows.Count + 1 - 1;
                int iEndColumnIndex = iStartColumnIndex + dt.Columns.Count - 1;
                SLTable table = sl.CreateTable(iStartRowIndex, iStartColumnIndex, iEndRowIndex, iEndColumnIndex);
                table.SetTableStyle(SLTableStyleTypeValues.Medium2);
                table.HasTotalRow = false;
                sl.InsertTable(table);

                sl.AutoFitColumn(1, iEndColumnIndex);
                sl.SaveAs(fileName);

                return true;
            }
            catch(Exception ex)
            {
                status = ex.Message;
                Logger.writeLog("Error Export to Excel:" + ex.Message);
                return false;
            }
        }
    }
}
