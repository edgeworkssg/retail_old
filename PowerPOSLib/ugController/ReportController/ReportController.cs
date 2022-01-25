using System;
using System.Collections.Generic;
using System.Data;

namespace PowerPOS.Report
{
    public interface ReportController
    {
        DataTable FetchAll(DateTime StartDate, DateTime EndDate
            , int PosID, string OutletName, string DeptID
            , string SortColumn, string SortDir, object SearchValue);
        DataTable FetchSpecial(int SpecialCode, object SearchValue);
    }
}
