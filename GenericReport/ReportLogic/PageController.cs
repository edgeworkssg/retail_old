
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using System.Windows.Forms;

namespace GenericReport
{
    public class PageController
    {
        public static void GenerateReport(POSController pos, int pageType, bool reprint, string reportType)
        {
            PageTemplate page = null;
            switch (pageType)
            {
                case PageType.A4:
                    page = new GenericReport.PageTypeA4();
                    break;
                case PageType.A5:
                    page = new GenericReport.PageTypeA5();
                    break;
                default:
                    MessageBox.Show("Invalid page type!", "Report generation failed!");
                    return;
            }
            page.CreateReport(pos, reprint, reportType);
        }
    }
}
