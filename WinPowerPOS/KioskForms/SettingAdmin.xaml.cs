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
using PowerPOS;
using PowerPOS.Container;
using SubSonic;

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for SettingGeneral.xaml
    /// </summary>
    public partial class SettingAdmin : UserControl
    {
        public SettingAdmin()
        {
            InitializeComponent();
        }

        public void Load()
        {
            {
                QueryCommand qc = new QueryCommand("SELECT OutletName FROM Outlet Where (Deleted IS NULL or Deleted = 0) ORDER BY OutletName");
                var rdr = DataService.GetReader(qc);

                List<String> tmp = new List<string>();
                tmp.Add("ALL");

                while (rdr.Read())
                    tmp.Add(rdr["OutletName"].ToString());

                cbOutlet.ItemsSource = tmp;
                cbOutlet.SelectedValue = PointOfSaleInfo.OutletName;
            }

            {
                QueryCommand qc = new QueryCommand("SELECT PointOfSaleName FROM PointOfSale Where OutletName = @OutletName AND (Deleted IS NULL or Deleted = 0) ORDER BY PointOfSaleName ASC");
                qc.AddParameter("OutletName", cbOutlet.SelectedValue.ToString());

                var rdr = DataService.GetReader(qc);

                List<String> tmp = new List<string>();
                while (rdr.Read())
                    tmp.Add(rdr["PointOfSaleName"].ToString());

                cbPointOfSale.ItemsSource = tmp;
                cbPointOfSale.SelectedValue = PointOfSaleInfo.PointOfSaleName;
            }

            chSimulatorWeighingScale.IsChecked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.SimulatorWeighingScale), false);
            chSimulatorNETS.IsChecked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.SimulatorNETS), false);
            txtIdleTimes.Text = AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.IdleTimes) + "";

            txtBarcodeCOMPort.Text = AppSetting.GetSetting(AppSetting.SettingsName.BarcodeScanner.COMPort);
            txtEnableCmd.Text = AppSetting.GetSetting(AppSetting.SettingsName.BarcodeScanner.enableCommand);
            txtDisableCmd.Text = AppSetting.GetSetting(AppSetting.SettingsName.BarcodeScanner.disableCommand);
            cbEnableBarcodeScanner.IsChecked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.BarcodeScanner.Enabled), false);
            cbShowPageLog.IsChecked = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.HideLogButton), false);


            List<string> data = new List<string>();
            data.Add("ORDER & PAYMENT");
            data.Add("PAYMENT ONLY");

            // ... Assign the ItemsSource to the List.
            cbKioskMode.ItemsSource = data;

            string kioskMode = AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.KioskMode);
            int selectedIdx = 0;
            foreach (var d in data)
            {
                if (d == kioskMode)
                    break;

                selectedIdx++;
            }

            // ... Make the first item selected.
            cbKioskMode.SelectedIndex = selectedIdx;
        }

        public void Save()
        {
            PointOfSale pr = new PointOfSale(PointOfSale.Columns.PointOfSaleName, cbPointOfSale.SelectedItem.ToString());
            if (!pr.IsNew)
            {
                SettingCollection st = new SettingCollection();
                st.Load();

                if (st.Count == 0)
                {
                    Logger.writeLog("Setting table has not been set/data deleted");
                    PowerPOS.Setting stTmp = new PowerPOS.Setting();
                    stTmp.PointOfSaleID = 0;
                    stTmp.WsUrl = "http://localhost:8080/";
                    stTmp.NETSTerminalID = "";
                    stTmp.EZLinkTerminalID = 1002001;
                    stTmp.IsEZLinkTerminal = false;
                    stTmp.EZLinkTerminalPwd = "888888";
                    stTmp.EZLinkCOMPort = "COM1";
                    stTmp.EZLinkBaudRate = 9600;
                    stTmp.EZLinkDataBits = 8;
                    stTmp.EZLinkParity = 0;
                    stTmp.EZLinkStopBits = 1;
                    stTmp.EZLinkHandShake = 0;
                    stTmp.PrintQuickCashReceipt = false;
                    stTmp.PrintEZLinkReceipt = false;
                    stTmp.PrintQuickCashWithEZLink = false;
                    stTmp.PromptSalesPerson = true;
                    stTmp.UseMembership = true;
                    stTmp.AllowLineDisc = true;
                    stTmp.AllowOverallDisc = true;
                    stTmp.AllowChangeCashier = true;
                    stTmp.AllowFeedBack = true;
                    stTmp.SQLServerName = "localhost\\SQLEXPRESS";
                    stTmp.DBName = "";
                    stTmp.IntegrateWithInventory = false;
                    stTmp.Save();
                    st = new SettingCollection();
                    st.Add(stTmp);
                }

                try
                {

                    st[0].PointOfSaleID = pr.PointOfSaleID;
                    st[0].Save("SYSTEM");
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }

                PointOfSaleController.GetPointOfSaleInfo();
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid Point Of Sale ID selected.", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.SimulatorWeighingScale, chSimulatorWeighingScale.IsChecked.Value.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.SimulatorNETS, chSimulatorNETS.IsChecked.Value.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.IdleTimes, txtIdleTimes.Text.GetIntValue()+"");

            AppSetting.SetSetting(AppSetting.SettingsName.BarcodeScanner.COMPort, txtBarcodeCOMPort.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.BarcodeScanner.disableCommand, txtDisableCmd.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.BarcodeScanner.enableCommand, txtEnableCmd.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.BarcodeScanner.Enabled, cbEnableBarcodeScanner.IsChecked + "");
            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.HideLogButton, !cbShowPageLog.IsChecked + "");
            //AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.KioskMode, cbKioskMode.SelectedValue.ToString());
        }

        private void txtDisableCmd_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //var ofd = new System.Windows.Forms.OpenFileDialog();
            ////ofd.Filter = "Print Template|*.rpt";
            //ofd.Tag = txtDisableCmd.Text;
            //ofd.Multiselect = false;
            //if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    txtDisableCmd.Text = ofd.FileName;
        }

        private void txtEnableCmd_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //var ofd = new System.Windows.Forms.OpenFileDialog();
            ////ofd.Filter = "Print Template|*.rpt";
            //ofd.Tag = txtDisableCmd.Text;
            //ofd.Multiselect = false;
            //if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    txtEnableCmd.Text = ofd.FileName;
        }

        private void cbOutlet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PointOfSaleCollection posc = new PointOfSaleCollection();

            if (cbOutlet.SelectedItem == null || cbOutlet.SelectedItem.ToString() == "ALL")
            {
                {
                    QueryCommand qc = new QueryCommand("SELECT PointOfSaleName FROM PointOfSale Where (Deleted IS NULL or Deleted = 0) ORDER BY PointOfSaleName ASC");
                    qc.AddParameter("OutletName", cbOutlet.SelectedValue.ToString());

                    var rdr = DataService.GetReader(qc);

                    List<String> tmp = new List<string>();
                    while (rdr.Read())
                        tmp.Add(rdr["PointOfSaleName"].ToString());

                    cbPointOfSale.ItemsSource = tmp;
                }
            }
            else
            {
                {
                    QueryCommand qc = new QueryCommand("SELECT PointOfSaleName FROM PointOfSale Where OutletName = @OutletName AND (Deleted IS NULL or Deleted = 0) ORDER BY PointOfSaleName ASC");
                    qc.AddParameter("OutletName", cbOutlet.SelectedValue.ToString());

                    var rdr = DataService.GetReader(qc);

                    List<String> tmp = new List<string>();
                    while (rdr.Read())
                        tmp.Add(rdr["PointOfSaleName"].ToString());

                    cbPointOfSale.ItemsSource = tmp;
                }
            }
        }
    }
}
