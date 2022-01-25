using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PowerPOS;

namespace PowerReport
{
    public abstract class POSReport
    {
        protected string ReportName;

        public abstract DataTable GetData();

        public ReportDocument GetWindowsReport()
        {
            ReportName = Application.StartupPath + "\\Reports\\ProductSales\\" + ReportName;

            return GetReport();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Data.SqlTypes.SqlNullValueException">Occur if no data is loaded</exception>
        public ReportDocument GetReport()
        {
            ReportDocument Output = new ReportDocument();

            bool ReportLoaded = false;
            if (ReportName != null && ReportName.ToLower().EndsWith(".rpt") && File.Exists(ReportName))
            {
                Output.Load(ReportName);
                AssignData(ref Output);
                Output.Refresh();

                ReportLoaded = true;
            }

            if (!ReportLoaded)
                return new ReportDocument();
            else
                return Output;
        }

        public abstract void AssignData(ref ReportDocument Inst);
    }
}
