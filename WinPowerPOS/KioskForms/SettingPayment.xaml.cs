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

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for xaml
    /// </summary>
    public partial class SettingPayment : UserControl
    {
        public SettingPayment()
        {
            InitializeComponent();
        }

        public void Load()
        {
            chEnabledNotes.IsChecked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.EnableNotes), false);
            txtMachineModelNotes.Text = AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.MachineModelNotes);
            txtCOMPortNotes.Text = AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.COMPortNotes);

            chEnabledCoins.IsChecked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.EnableCoins), false);
            txtMachineModelCoins.Text = AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.MachineModelCoins);
            txtCOMPortCoins.Text = AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.COMPortCoins);

            chEnabledNets.IsChecked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.EnableNETS), false);
            txtCOMPortNets.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.NetsCOMPort);
        }

        public void Save()
        {
            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.EnableNotes, chEnabledNotes.IsChecked.Value.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.MachineModelNotes, txtMachineModelNotes.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.COMPortNotes, txtCOMPortNotes.Text);

            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.EnableCoins, chEnabledCoins.IsChecked.Value.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.MachineModelCoins, txtMachineModelCoins.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.COMPortCoins, txtCOMPortCoins.Text);

            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.EnableNETS, chEnabledNets.IsChecked.Value.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Payment.NetsCOMPort, txtCOMPortNets.Text);
        }
    }
}
