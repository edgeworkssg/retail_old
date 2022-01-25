using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;
using WinPowerPOS.OrderForms;
using PowerPOS.Container;
using System.Collections;
using POSDevices;

namespace WinPowerPOS
{
    public partial class frmSalesReport : Form
    {
        public frmSalesReport()
        {
            InitializeComponent();
            dgvReport.AutoGenerateColumns = false;            
        }

        private void frmSalesReport_Load(object sender, EventArgs e)
        {            
            CommonUILib.FormatDateFilter(ref dtpStartDate, ref dtpEndDate);
            dtpStartDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            BindGrid();
        }

        private void BindGrid()
        {
            dgvReport.DataSource =
                ReportController.FetchCounterClosingReport
                (true, true, dtpStartDate.Value, 
                dtpEndDate.Value, "", "","", PointOfSaleInfo.PointOfSaleID, "%", "%", "0", 
                "EndTime", "DESC");
            dgvReport.Refresh();
        }
        private const int RECORDED_COL = 4;
        private const int COLLECTED_COL = 5;
        private const int VARIANCE_COL = 6;
        private void dgvReport_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            decimal TotalRecorded, TotalCollected;
            if (decimal.TryParse(dgvReport[RECORDED_COL, e.RowIndex].Value.ToString(), out TotalRecorded)
                && decimal.TryParse(dgvReport[COLLECTED_COL, e.RowIndex].Value.ToString(), out TotalCollected))
            {
                //dgvReport[RECORDED_COL, e.RowIndex].ValueType = Type.GetType("System.String");
                //dgvReport[COLLECTED_COL, e.RowIndex].ValueType = Type.GetType("System.String");
                //dgvReport[RECORDED_COL, e.RowIndex].Value = "$" + TotalRecorded.ToString("N2");
                //dgvReport[COLLECTED_COL, e.RowIndex].Value = "$" + TotalCollected.ToString("N2");
                decimal variance = (TotalCollected - TotalRecorded);
                dgvReport[VARIANCE_COL, e.RowIndex].Value = variance;
                
                if (variance >= 0)
                {
                    //dgvReport[VARIANCE_COL, e.RowIndex].Value = "$" + variance.ToString("N2");
                    if (variance > 0)
                    {
                        dgvReport.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                    }
                }
                else
                {
                    //variance = -variance;
                    //dgvReport[VARIANCE_COL, e.RowIndex].Value = "($" + variance.ToString("N2") + ")";
                    dgvReport.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Salmon;
                }
                 
            }
        }

        private void dgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    try
                    {
                        bool PrintProductSalesReport = false;
                        PrintProductSalesReport = (AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport") != null && AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport").ToString().ToUpper() == "YES");
                        bool printDiscount = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintDiscountOnCounterCloseReport), false));
                        POSDeviceController.PrintCloseCounterReport(new CounterCloseLog(CounterCloseLog.Columns.CounterCloseID, dgvReport.Rows[e.RowIndex].Cells[3].Value.ToString()), PrintProductSalesReport, printDiscount);
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        MessageBox.Show("Error printing settlement. Please contact your administrator.");
                    }
                    /*
                    frmClosingReport f = new frmClosingReport();
                    f.CounterCloseLogID = dgvReport.Rows[e.RowIndex].Cells[2].Value.ToString();
                    f.ShowDialog();*/
                }
                else if (e.ColumnIndex == 1)
                {
                    /*
                    try
                    {
                        frmClosingReport f = new frmClosingReport();
                        f.CounterCloseLogID = dgvReport.Rows[e.RowIndex].Cells[3].Value.ToString();
                        f.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        MessageBox.Show("Error printing settlement. Please contact your administrator.");
                    }
                    */                   
                }
                else if (e.ColumnIndex == 10)
                {
                    try
                    {
                        //
                        CounterCloseLog cl = new CounterCloseLog
                            (CounterCloseLog.Columns.CounterCloseID, dgvReport.Rows[e.RowIndex].Cells[3].Value.ToString());

                        if (cl.IsLoaded)
                        {
                            //
                            OrderHdrCollection ordCol = new OrderHdrCollection();
                            ordCol.Where(OrderHdr.Columns.OrderDate, Comparison.GreaterOrEquals, cl.StartTime);
                            ordCol.Where(OrderHdr.Columns.OrderDate, Comparison.LessOrEquals, cl.EndTime);
                            ordCol.Where(OrderHdr.Columns.IsVoided, false);
                            ordCol.Where(OrderHdr.Columns.PointOfSaleID, cl.PointOfSaleID);
                            ordCol.Load();

                            for (int i = 0; i < ordCol.Count; i++)
                            {
                                POSDevices.POSDeviceController.PrintAHAVATransactionReceipt(new POSController(ordCol[i].OrderHdrID), 0, true, POSDevices.ReceiptSizes.A4, 1);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        MessageBox.Show("Error printing settlement. Please contact your administrator.");
                    }
                }
                else if (e.ColumnIndex == 11)
                {
                    try
                    {
                        CounterCloseLog cl = new CounterCloseLog
                            (CounterCloseLog.Columns.CounterCloseID, dgvReport.Rows[e.RowIndex].Cells[3].Value.ToString());
                        if (cl.IsLoaded)
                        {
                            ArrayList emails = new ArrayList();
                            EmailNotificationCollection em = new EmailNotificationCollection();
                            em.Where(EmailNotification.Columns.Deleted, false);
                            em.Load();
                            if (em.Count > 0)
                            {
                                for (int i = 0; i < em.Count; i++)
                                {
                                    emails.Add(em[i].EmailAddress);
                                }
                            }
                            
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        MessageBox.Show("Error sending email. Pleasae contact your administrator");
                    }

                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}