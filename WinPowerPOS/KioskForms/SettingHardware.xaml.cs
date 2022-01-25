using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PowerPOS.Container;
using PowerPOS;
using System.Drawing.Printing;
using PowerPOSLib.Controller;
using System.Printing;

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for xaml
    /// </summary>
    public partial class SettingHardware : UserControl
    {
        private WeighingMachineController wmController;

        public SettingHardware()
        {
            InitializeComponent();

            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                cbPrinter.Items.Add(printer);
            }
        }

        public void Load()
        {
            chWeighingScale.IsChecked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.UseWeighingMachine), false);
            txtWeighingCOMPort.Text = AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.COMPort);
            txtTolerance.Text = AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.Tolerance);

            var ps = cbPrinter.Items;
            for (int i = 0; i < ps.Count; i++)
            {
                string p = (string)ps[i];
                if (p == AppSetting.GetSetting(AppSetting.SettingsName.Print.PrinterName))
                {
                    cbPrinter.SelectedIndex = i;
                    break;
                }
            }

            if (cbPrinter.SelectedIndex >= 0)
            {
                TestPrinter_Click(null, new RoutedEventArgs());
            }
        }

        public void Save()
        {
            AppSetting.SetSetting(AppSetting.SettingsName.WeighingMachine.UseWeighingMachine, chWeighingScale.IsChecked.Value.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.WeighingMachine.COMPort, txtWeighingCOMPort.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.WeighingMachine.Tolerance, txtTolerance.Text);
            if (cbPrinter.SelectedItem != null)
            {
                string p = (string)cbPrinter.SelectedItem;
                AppSetting.SetSetting(AppSetting.SettingsName.Print.PrinterName, p);
            }
        }

        public void SetWMContoller(WeighingMachineController wmController)
        {
            this.wmController = wmController;

            if (wmController != null)
            {
                string status = wmController.getStatus().Trim().ToUpper() == "CONNECTED" ? "OK" : "NOT CONNECTED";
                txtWeighingScaleStatus.Text = status;
            }
        }

        public void setWeighingMachineStatus(bool connected)
        {
            txtWeighingScaleStatus.Text = connected ? "OK" : "NOT CONNECTED";
        }

        private void TestWeighing_Click(object sender, RoutedEventArgs e)
        {
            if (wmController != null)
            {
                string status = wmController.getStatus().Trim().ToUpper() == "CONNECTED" ? "OK" : "NOT CONNECTED";
                txtWeighingScaleStatus.Text = status;

                MessageBox.Show("Weighing Scale = " + status);
            }
        }

        private void TestPrinter_Click(object sender, RoutedEventArgs e)
        {
            if (cbPrinter.SelectedItem != null)
            {
                string printerName = (string)cbPrinter.SelectedItem;

                var server = new PrintServer();
                PrintQueue pq = server.GetPrintQueue(printerName);

                string statusReport = "";
                if (pq.HasPaperProblem)
                {
                    statusReport = statusReport + "Has a paper problem. ";
                }
                if (!(pq.HasToner))
                {
                    statusReport = statusReport + "Is out of toner. ";
                }
                if (pq.IsDoorOpened)
                {
                    statusReport = statusReport + "Has an open door. ";
                }
                if (pq.IsInError)
                {
                    statusReport = statusReport + "Is in an error state. ";
                }
                if (pq.IsNotAvailable)
                {
                    statusReport = statusReport + "Is not available. ";
                }
                if (pq.IsOffline)
                {
                    statusReport = statusReport + "Is off line. ";
                }
                if (pq.IsOutOfMemory)
                {
                    statusReport = statusReport + "Is out of memory. ";
                }
                if (pq.IsOutOfPaper)
                {
                    statusReport = statusReport + "Is out of paper. ";
                }
                if (pq.IsOutputBinFull)
                {
                    statusReport = statusReport + "Has a full output bin. ";
                }
                if (pq.IsPaperJammed)
                {
                    statusReport = statusReport + "Has a paper jam. ";
                }
                if (pq.IsPaused)
                {
                    statusReport = statusReport + "Is paused. ";
                }
                if (pq.IsTonerLow)
                {
                    statusReport = statusReport + "Is low on toner. ";
                }
                if (pq.NeedUserIntervention)
                {
                    statusReport = statusReport + "Needs user intervention. ";
                }

                txtPrinterStatus.Text = !pq.IsOffline ? "OK" : statusReport;
            }
        }
    }
}
