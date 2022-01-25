using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using System.Collections;

namespace WinPowerPOS.Reports
{
    public partial class frmStylistSalesReport : Form
    {
        private string FormName = "New Commission Report";
        private string PivotKeyColumn = "KeyID";
        private string PivotNameColumn = "PackageName";
        private string PivotValueColumn = "Amount";
        //private string QueryStr =
        //    "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,ReceiptDate) AS VARCHAR(100)) AS KeyID, CONVERT(DATE,ReceiptDate) AS Date, SalesCommissionRecord.SalesPersonID AS SalesPerson, ISNULL(Item.ItemName,ReceiptDet.userfld1) AS PackageName, SUM(ReceiptDet.Amount) AS AMOUNT " +
        //    "FROM ReceiptDet " +
        //        "INNER JOIN ReceiptHdr ON ReceiptDet.ReceiptHdrID = ReceiptHdr.ReceiptHdrID " +
        //        "INNER JOIN OrderHdr ON ReceiptHdr.OrderHdrID = OrderHdr.OrderHdrID " +
        //        "INNER JOIN SalesCommissionRecord ON OrderHdr.OrderHdrID = SalesCommissionRecord.OrderHdrID " +
        //        "LEFT OUTER JOIN Item ON ReceiptDet.userfld1 = Item.ItemNo " +
        //    "WHERE ReceiptDet.PaymentType IN ('POINTS','PACKAGE') " +
        //    "GROUP BY CONVERT(DATE,ReceiptDate),SalesCommissionRecord.SalesPersonID,ReceiptDet.userfld1,Item.ItemName " +
        //    "ORDER BY SalesCommissionRecord.SalesPersonID,CONVERT(DATE,ReceiptDate)";
        private string NonPackageFieldName = "COMPANY";
        private string CommissionFieldName = "COMMISSION";
        private string NonCommissionFieldName = "NON COMMISSION";
        private string WalkInFieldName = "WALK IN";

        private string PointQueryStr =
            "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,ReceiptDate) AS VARCHAR(100)) AS KeyID " +
                ", CONVERT(DATE,ReceiptDate) AS Dates, SalesCommissionRecord.SalesPersonID AS [Sales Person] " +
                ", ISNULL(Item.ItemName,ReceiptDet.userfld1) AS PackageName, SUM(ReceiptDet.Amount) AS AMOUNT " +
            "FROM ReceiptDet " +
                "INNER JOIN ReceiptHdr ON ReceiptDet.ReceiptHdrID = ReceiptHdr.ReceiptHdrID " +
                "INNER JOIN OrderHdr ON ReceiptHdr.OrderHdrID = OrderHdr.OrderHdrID " +
                "INNER JOIN SalesCommissionRecord ON OrderHdr.OrderHdrID = SalesCommissionRecord.OrderHdrID " +
                "LEFT OUTER JOIN Item ON ReceiptDet.userfld1 = Item.ItemNo " +
            "WHERE ReceiptDet.PaymentType IN ('" + POSController.PAY_POINTS + "','" + POSController.PAY_PACKAGE + "') AND ReceiptDate BETWEEN @StartDate AND @EndDate " +
                "AND OrderHdr.IsVoided = 0 " +
                "AND SalesCommissionRecord.SalesPersonID LIKE @SalesPerson " +
            "GROUP BY CONVERT(DATE,ReceiptDate),SalesCommissionRecord.SalesPersonID,ReceiptDet.userfld1,Item.ItemName " +
            "UNION ALL " +
            "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,OrderHdr.OrderDate) AS VARCHAR(100)) AS KeyID " +
                ", CONVERT(DATE,OrderHdr.OrderDate) AS Dates, SalesCommissionRecord.SalesPersonID AS [Sales Person] " +
                ", 'NON_PACKAGE_FIELD' AS PackageName, SUM(OrderDet.Amount) AS AMOUNT " +
            "FROM SalesCommissionRecord " +
                "INNER JOIN OrderHdr ON SalesCommissionRecord.OrderHdrID = OrderHdr.OrderHdrID " +
                "INNER JOIN OrderDet ON OrderHdr.OrderHdrID = OrderDet.OrderHdrID " +
            "WHERE OrderDet.giveCommission = 0 AND OrderHdr.OrderDate BETWEEN @StartDate AND @EndDate " +
                "AND OrderHdr.IsVoided = 0 " +
                "AND SalesCommissionRecord.SalesPersonID LIKE @SalesPerson " +
            "GROUP BY CONVERT(DATE,OrderHdr.OrderDate),SalesCommissionRecord.SalesPersonID " +
            //"SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,ReceiptDate) AS VARCHAR(100)) AS KeyID " +
            //    ", CONVERT(DATE,ReceiptDate) AS Dates, SalesCommissionRecord.SalesPersonID AS SalesPerson " +
            //    ", 'NON_PACKAGE_FIELD' AS PackageName, SUM(ReceiptDet.Amount) AS AMOUNT " +
            //"FROM ReceiptDet " +
            //    "INNER JOIN ReceiptHdr ON ReceiptDet.ReceiptHdrID = ReceiptHdr.ReceiptHdrID " +
            //    "INNER JOIN OrderHdr ON ReceiptHdr.OrderHdrID = OrderHdr.OrderHdrID " +
            //    "INNER JOIN SalesCommissionRecord ON OrderHdr.OrderHdrID = SalesCommissionRecord.OrderHdrID " +
            //    "LEFT OUTER JOIN Item ON ReceiptDet.userfld1 = Item.ItemNo " +
            //"WHERE ReceiptDet.PaymentType NOT IN ('" + POSController.PAY_POINTS + "','" + POSController.PAY_PACKAGE + "') AND ReceiptDate BETWEEN @StartDate AND @EndDate " +
            //"GROUP BY CONVERT(DATE,ReceiptDate),SalesCommissionRecord.SalesPersonID " +
            "UNION ALL " +
            "SELECT " +
                "KeyID,Dates,SalesPerson AS [Sales Person],'WALK_IN' AS PackageName " +
	            ",SUM(CASE WHEN (A.PackageName = 'NON_PACKAGE_FLD') THEN A.AMOUNT ELSE 0 END) - SUM(CASE WHEN (A.PackageName = 'NON_COMMISSION_FLD') THEN A.AMOUNT ELSE 0 END) AS AMOUNT " +
            "FROM " +
            "( " +
                "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,ReceiptDate) AS VARCHAR(100)) AS KeyID " +
                    ", CONVERT(DATE,ReceiptDate) AS Dates, SalesCommissionRecord.SalesPersonID AS SalesPerson " +
                    ", 'NON_PACKAGE_FLD' AS PackageName, SUM(ReceiptDet.Amount) AS AMOUNT " +
                "FROM ReceiptDet " +
                    "INNER JOIN ReceiptHdr ON ReceiptDet.ReceiptHdrID = ReceiptHdr.ReceiptHdrID " +
                    "INNER JOIN OrderHdr ON ReceiptHdr.OrderHdrID = OrderHdr.OrderHdrID " +
                    "INNER JOIN SalesCommissionRecord ON OrderHdr.OrderHdrID = SalesCommissionRecord.OrderHdrID " +
                    //"LEFT OUTER JOIN Item ON ReceiptDet.userfld1 = Item.ItemNo " +
                "WHERE ReceiptDet.PaymentType NOT IN ('" + POSController.PAY_POINTS + "','" + POSController.PAY_PACKAGE + "') AND ReceiptDate BETWEEN @StartDate AND @EndDate " +
                    "AND OrderHdr.IsVoided = 0 " +
                    "AND SalesCommissionRecord.SalesPersonID LIKE @SalesPerson " +
                "GROUP BY CONVERT(DATE,ReceiptDate),SalesCommissionRecord.SalesPersonID " +
                "UNION ALL " +
                "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,OrderHdr.OrderDate) AS VARCHAR(100)) AS KeyID " +
                    ", CONVERT(DATE,OrderHdr.OrderDate) AS Dates, SalesCommissionRecord.SalesPersonID AS SalesPerson " +
                    ", 'NON_COMMISSION_FLD' AS PackageName, SUM(OrderDet.Amount) AS AMOUNT " +
                "FROM SalesCommissionRecord " +
                    "INNER JOIN OrderHdr ON SalesCommissionRecord.OrderHdrID = OrderHdr.OrderHdrID " +
                    "INNER JOIN OrderDet ON OrderHdr.OrderHdrID = OrderDet.OrderHdrID " +
                "WHERE OrderDet.giveCommission = 0 AND OrderHdr.OrderDate BETWEEN @StartDate AND @EndDate " +
                    "AND OrderHdr.IsVoided = 0 " +
                    "AND SalesCommissionRecord.SalesPersonID LIKE @SalesPerson " +
                "GROUP BY CONVERT(DATE,OrderHdr.OrderDate),SalesCommissionRecord.SalesPersonID " +
            ") AS A " +
            "GROUP BY KeyID,Dates,SalesPerson " +
            "HAVING SUM(CASE WHEN (A.PackageName = 'NON_PACKAGE_FLD') THEN A.AMOUNT ELSE 0 END) - SUM(CASE WHEN (A.PackageName = 'NON_COMMISSION_FLD') THEN A.AMOUNT ELSE 0 END) <> 0 " +
            "ORDER BY 1";
        //private string PointQueryStr =
        //    "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,ReceiptDate) AS VARCHAR(100)) AS KeyID " +
        //        ", CONVERT(DATE,ReceiptDate) AS Dates, SalesCommissionRecord.SalesPersonID AS SalesPerson " +
        //        ", ISNULL(Item.ItemName,ReceiptDet.userfld1) AS PackageName, SUM(ReceiptDet.Amount) AS AMOUNT " +
        //    "FROM ReceiptDet " +
        //        "INNER JOIN ReceiptHdr ON ReceiptDet.ReceiptHdrID = ReceiptHdr.ReceiptHdrID " +
        //        "INNER JOIN OrderHdr ON ReceiptHdr.OrderHdrID = OrderHdr.OrderHdrID " +
        //        "INNER JOIN SalesCommissionRecord ON OrderHdr.OrderHdrID = SalesCommissionRecord.OrderHdrID " +
        //        "LEFT OUTER JOIN Item ON ReceiptDet.userfld1 = Item.ItemNo " +
        //    "WHERE ReceiptDet.PaymentType IN ('" + POSController.PAY_POINTS + "','" + POSController.PAY_PACKAGE + "') AND ReceiptDate BETWEEN @StartDate AND @EndDate " +
        //    "GROUP BY CONVERT(DATE,ReceiptDate),SalesCommissionRecord.SalesPersonID,ReceiptDet.userfld1,Item.ItemName " +
        //    "UNION ALL " +
        //    "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,ReceiptDate) AS VARCHAR(100)) AS KeyID " +
        //        ", CONVERT(DATE,ReceiptDate) AS Dates, SalesCommissionRecord.SalesPersonID AS SalesPerson " +
        //        ", 'NON_PACKAGE_FIELD' AS PackageName, SUM(ReceiptDet.Amount) AS AMOUNT " +
        //    "FROM ReceiptDet " +
        //        "INNER JOIN ReceiptHdr ON ReceiptDet.ReceiptHdrID = ReceiptHdr.ReceiptHdrID " +
        //        "INNER JOIN OrderHdr ON ReceiptHdr.OrderHdrID = OrderHdr.OrderHdrID " +
        //        "INNER JOIN SalesCommissionRecord ON OrderHdr.OrderHdrID = SalesCommissionRecord.OrderHdrID " +
        //        "LEFT OUTER JOIN Item ON ReceiptDet.userfld1 = Item.ItemNo " +
        //    "WHERE ReceiptDet.PaymentType NOT IN ('" + POSController.PAY_POINTS + "','" + POSController.PAY_PACKAGE + "') AND ReceiptDate BETWEEN @StartDate AND @EndDate " +
        //    "GROUP BY CONVERT(DATE,ReceiptDate),SalesCommissionRecord.SalesPersonID " +
        //    "UNION ALL " +
        //    "SELECT " +
        //        "KeyID,Dates,SalesPerson,'WALK_IN' AS SalesPerson " +
        //        ",SUM(CASE WHEN (A.PackageName = 'NON_PACKAGE_FLD') THEN A.AMOUNT ELSE 0 END) - SUM(CASE WHEN (A.PackageName = 'NON_COMMISSION_FLD') THEN A.AMOUNT ELSE 0 END) AS AMOUNT " +
        //    "FROM " +
        //    "( " +
        //        "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,ReceiptDate) AS VARCHAR(100)) AS KeyID " +
        //            ", CONVERT(DATE,ReceiptDate) AS Dates, SalesCommissionRecord.SalesPersonID AS SalesPerson " +
        //            ", 'NON_PACKAGE_FLD' AS PackageName, SUM(ReceiptDet.Amount) AS AMOUNT " +
        //        "FROM ReceiptDet " +
        //            "INNER JOIN ReceiptHdr ON ReceiptDet.ReceiptHdrID = ReceiptHdr.ReceiptHdrID " +
        //            "INNER JOIN OrderHdr ON ReceiptHdr.OrderHdrID = OrderHdr.OrderHdrID " +
        //            "INNER JOIN SalesCommissionRecord ON OrderHdr.OrderHdrID = SalesCommissionRecord.OrderHdrID " +
        //            "LEFT OUTER JOIN Item ON ReceiptDet.userfld1 = Item.ItemNo " +
        //        "WHERE ReceiptDet.PaymentType NOT IN ('" + POSController.PAY_POINTS + "','" + POSController.PAY_PACKAGE + "') AND ReceiptDate BETWEEN @StartDate AND @EndDate " +
        //        "GROUP BY CONVERT(DATE,ReceiptDate),SalesCommissionRecord.SalesPersonID " +
        //        "UNION ALL " +
        //        "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,OrderHdr.OrderDate) AS VARCHAR(100)) AS KeyID " +
        //            ", CONVERT(DATE,OrderHdr.OrderDate) AS Dates, SalesCommissionRecord.SalesPersonID AS SalesPerson " +
        //            ", 'NON_COMMISSION_FLD' AS PackageName, SUM(OrderDet.Amount) AS AMOUNT " +
        //        "FROM SalesCommissionRecord " +
        //            "INNER JOIN OrderHdr ON SalesCommissionRecord.OrderHdrID = OrderHdr.OrderHdrID " +
        //            "INNER JOIN OrderDet ON OrderHdr.OrderHdrID = OrderDet.OrderHdrID " +
        //        "WHERE OrderDet.giveCommission = 0 AND OrderHdr.OrderDate BETWEEN @StartDate AND @EndDate " +
        //        "GROUP BY CONVERT(DATE,OrderHdr.OrderDate),SalesCommissionRecord.SalesPersonID " +
        //        "HAVING SUM(OrderDet.Quantity * OrderDet.Amount) <> 0 " +
        //    ") AS A " +
        //    "GROUP BY KeyID,Dates,SalesPerson " +
        //    "ORDER BY 3,2";
        #region Obsoleted
        private string CommissionQueryStr =
            "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,OrderHdr.OrderDate) AS VARCHAR(100)) AS KeyID " +
                ", CONVERT(DATE,OrderHdr.OrderDate) AS Date, SalesCommissionRecord.SalesPersonID AS SalesPerson " +
                ", 'COMMISSION_FIELD' AS PackageName, SUM(OrderDet.Amount) AS AMOUNT " +
            "FROM SalesCommissionRecord " +
                "INNER JOIN OrderHdr ON SalesCommissionRecord.OrderHdrID = OrderHdr.OrderHdrID " +
                "INNER JOIN OrderDet ON OrderHdr.OrderHdrID = OrderDet.OrderHdrID " +
            "WHERE OrderDet.giveCommission = 1 AND OrderHdr.OrderDate BETWEEN @StartDate AND @EndDate " +
            "GROUP BY CONVERT(DATE,OrderHdr.OrderDate),SalesCommissionRecord.SalesPersonID " +
            "HAVING SUM(OrderDet.Quantity * OrderDet.Amount) <> 0 " +
            "UNION ALL " +
            "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,OrderHdr.OrderDate) AS VARCHAR(100)) AS KeyID " +
                ", CONVERT(DATE,OrderHdr.OrderDate) AS Date, SalesCommissionRecord.SalesPersonID AS SalesPerson " +
                ", 'NON_COMMISSION_FIELD' AS PackageName, SUM(OrderDet.Amount) AS AMOUNT " +
            "FROM SalesCommissionRecord " +
                "INNER JOIN OrderHdr ON SalesCommissionRecord.OrderHdrID = OrderHdr.OrderHdrID " +
                "INNER JOIN OrderDet ON OrderHdr.OrderHdrID = OrderDet.OrderHdrID " +
            "WHERE OrderDet.giveCommission = 0 AND OrderHdr.OrderDate BETWEEN @StartDate AND @EndDate " +
            "GROUP BY CONVERT(DATE,OrderHdr.OrderDate),SalesCommissionRecord.SalesPersonID " +
            "ORDER BY 3,2";

        private string QueryStrOld =
            "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,ReceiptDate) AS VARCHAR(100)) AS KeyID " +
                ", CONVERT(DATE,ReceiptDate) AS Date, SalesCommissionRecord.SalesPersonID AS SalesPerson " +
                ", ReceiptDet.userfld1 AS OrderFld, ISNULL(Item.ItemName,ReceiptDet.userfld1) AS PackageName, SUM(ReceiptDet.Amount) AS AMOUNT " +
            "FROM ReceiptDet " +
                "INNER JOIN ReceiptHdr ON ReceiptDet.ReceiptHdrID = ReceiptHdr.ReceiptHdrID " +
                "INNER JOIN OrderHdr ON ReceiptHdr.OrderHdrID = OrderHdr.OrderHdrID " +
                "INNER JOIN SalesCommissionRecord ON OrderHdr.OrderHdrID = SalesCommissionRecord.OrderHdrID " +
                "LEFT OUTER JOIN Item ON ReceiptDet.userfld1 = Item.ItemNo " +
            "WHERE ReceiptDet.PaymentType IN ('POINTS','PACKAGE') AND ReceiptDate BETWEEN @StartDate AND @EndDate " +
            "GROUP BY CONVERT(DATE,ReceiptDate),SalesCommissionRecord.SalesPersonID,ReceiptDet.userfld1,Item.ItemName " +
            "UNION ALL " +
            "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,OrderHdr.OrderDate) AS VARCHAR(100)) AS KeyID " +
                ", CAST(CONVERT(DATE,OrderHdr.OrderDate) AS VARCHAR(100)) AS Date, SalesCommissionRecord.SalesPersonID AS SalesPerson " +
                ", '1' AS OrderFld, 'OTHER COMMISSION' AS PackageName, SUM(OrderDet.Amount) - ISNULL(SUM(ReceiptDet.Amount),0) AS AMOUNT " +
            "FROM SalesCommissionRecord " +
                "INNER JOIN OrderHdr ON SalesCommissionRecord.OrderHdrID = OrderHdr.OrderHdrID " +
                "INNER JOIN OrderDet ON OrderHdr.OrderHdrID = OrderDet.OrderHdrID " +
                "LEFT JOIN ReceiptHdr ON ReceiptHdr.OrderHdrID = OrderHdr.OrderHdrID " +
                "LEFT JOIN ReceiptDet ON ReceiptDet.ReceiptHdrID = ReceiptHdr.ReceiptHdrID " +
            "WHERE OrderDet.giveCommission = 1 AND OrderHdr.OrderDate BETWEEN @StartDate AND @EndDate " +
                "AND ReceiptDet.PaymentType IN ('POINTS','PACKAGE') " +
            "GROUP BY CONVERT(DATE,OrderHdr.OrderDate),SalesCommissionRecord.SalesPersonID " +
            "HAVING SUM(OrderDet.Quantity * OrderDet.Amount) <> ISNULL(SUM(ReceiptDet.Amount),0) " +
            "UNION ALL " +
            "SELECT SalesCommissionRecord.SalesPersonID + CAST(CONVERT(DATE,OrderHdr.OrderDate) AS VARCHAR(100)) AS KeyID " +
                ", CAST(CONVERT(DATE,OrderHdr.OrderDate) AS VARCHAR(100)) AS Date, SalesCommissionRecord.SalesPersonID AS SalesPerson " +
                ", '1' AS OrderFld, 'OTHER / NON COMMISSION' AS PackageName, SUM(OrderDet.Amount) AS AMOUNT " +
            "FROM SalesCommissionRecord " +
                "INNER JOIN OrderHdr ON SalesCommissionRecord.OrderHdrID = OrderHdr.OrderHdrID " +
                "INNER JOIN OrderDet ON OrderHdr.OrderHdrID = OrderDet.OrderHdrID " +
            "WHERE OrderDet.giveCommission = 0 AND OrderHdr.OrderDate BETWEEN @StartDate AND @EndDate " +
            "GROUP BY CONVERT(DATE,OrderHdr.OrderDate),SalesCommissionRecord.SalesPersonID " +
            "ORDER BY 3,2,4";
        #endregion

        public frmStylistSalesReport()
        {
            InitializeComponent();
            CommonUILib.FormatDateFilter(ref dtpStartDate, ref dtpEndDate);
        }

        private void frmStylistReport_Load(object sender, EventArgs e)
        {
            UserMstCollection st = new UserMstCollection();
            //st.Where(UserMst.Columns.Deleted, false);
            st.Where(UserMst.Columns.Deleted, false);
            st.Where(UserMst.Columns.IsASalesPerson, true);
            st.Load();
            st.Sort(UserMst.Columns.UserName, true);

            ArrayList ar = new ArrayList();
            ar.Add("-ALL-");
            for (int i = 0; i < st.Count; i++)
            {
                ar.Add(st[i].UserName);
            }

            cmbSearch.DataSource = ar; //SalesPersonController.FetchSalesPersonNames();
            cmbSearch.Refresh();

            if (cmbSearch.Items.Count > 0) cmbSearch.SelectedIndex = 0;

            BindGrid();
        }
        private void BindGrid()
        {
            string ExecQuery1 =
                PointQueryStr.Replace("NON_PACKAGE_FIELD", NonPackageFieldName)
                .Replace("WALK_IN", WalkInFieldName);

            dgvReport.DataSource = null;
            if (!dgvReport.Columns.Contains(dgvcKeyID.Name)) dgvReport.Columns.Add(dgvcKeyID);
            if (!dgvReport.Columns.Contains(dgvcDates.Name)) dgvReport.Columns.Add(dgvcDates);
            //if (!dgvReport.Columns.Contains(dgvcSalesPerson.Name)) dgvReport.Columns.Add(dgvcSalesPerson);

            string SalesPerson = cmbSearch.SelectedIndex == 0 ? "%" : cmbSearch.SelectedItem.ToString();

            SubSonic.QueryCommand Cmd = new SubSonic.QueryCommand(ExecQuery1);
            Cmd.AddParameter("@StartDate", dtpStartDate.Value, DbType.DateTime);
            Cmd.AddParameter("@EndDate", dtpEndDate.Value, DbType.DateTime);
            Cmd.AddParameter("@SalesPerson", SalesPerson, DbType.String);

            dgvReport.DataSource = ReportController.Pivot(
                SubSonic.DataService.GetReader(Cmd)
                , PivotKeyColumn, PivotNameColumn, PivotValueColumn);

            dgvReport.Refresh();
            

            this.Text = FormName;
        }

        private void dgvReport_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt;

            dt = (DataTable)dgvReport.DataSource;

            if (dt != null && dt.Rows.Count > 0)
            {
                fsdExportToExcel.ShowDialog();
            }
            else
            {
                MessageBox.Show("There is no data to export");
            }
        }
        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            DataTable dt;

            ExportController.ExportToCSV(dgvReport, fsdExportToExcel.FileName);

            MessageBox.Show("File saved");
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}
