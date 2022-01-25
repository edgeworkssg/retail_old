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
    public partial class SettingGeneral : UserControl
    {
        public SettingGeneral()
        {
            InitializeComponent();
        }

        public void Load()
        {
            h1.Text = PrintSettingInfo.receiptSetting.ReceiptAddress1;
            h2.Text = PrintSettingInfo.receiptSetting.ReceiptAddress2;
            h3.Text = PrintSettingInfo.receiptSetting.ReceiptAddress3;
            h4.Text = PrintSettingInfo.receiptSetting.ReceiptAddress4;

            f1.Text = PrintSettingInfo.receiptSetting.TermCondition1;
            f2.Text = PrintSettingInfo.receiptSetting.TermCondition2;
            f3.Text = PrintSettingInfo.receiptSetting.TermCondition3;
            f4.Text = PrintSettingInfo.receiptSetting.TermCondition4;
            f5.Text = PrintSettingInfo.receiptSetting.TermCondition5;
            f6.Text = PrintSettingInfo.receiptSetting.TermCondition6;

            //string langID = AppSetting.GetSetting(AppSetting.SettingsName.LanguageSetting);
            //if (langID == "CHS")
            //    chUseChineseLanguage.IsChecked = true;
            //else
            //    chUseChineseLanguage.IsChecked = false;
            chUseChineseLanguage.IsChecked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.UseCHSLang), false);
            cbKioskStatus.IsChecked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.KioskStatus), false);
        }

        public void Save()
        {
            PrintSettingInfo.receiptSetting.ReceiptAddress1 = h1.Text;
            PrintSettingInfo.receiptSetting.ReceiptAddress2 = h2.Text;
            PrintSettingInfo.receiptSetting.ReceiptAddress3 = h3.Text;
            PrintSettingInfo.receiptSetting.ReceiptAddress4 = h4.Text;
            PrintSettingInfo.receiptSetting.TermCondition1 = f1.Text;
            PrintSettingInfo.receiptSetting.TermCondition2 = f2.Text;
            PrintSettingInfo.receiptSetting.TermCondition3 = f3.Text;
            PrintSettingInfo.receiptSetting.TermCondition4 = f4.Text;
            PrintSettingInfo.receiptSetting.TermCondition5 = f5.Text;
            PrintSettingInfo.receiptSetting.TermCondition6 = f6.Text;
            PrintSettingInfo.receiptSetting.Save();
            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.KioskStatus, cbKioskStatus.IsChecked + "");


            AppSetting.SetSetting(AppSetting.SettingsName.Kiosk.UseCHSLang, chUseChineseLanguage.IsChecked.GetValueOrDefault(false).ToString());
            if (chUseChineseLanguage.IsChecked != null && chUseChineseLanguage.IsChecked.Value)
                AppSetting.SetSetting(AppSetting.SettingsName.LanguageSetting, "CHS");
            else
                AppSetting.SetSetting(AppSetting.SettingsName.LanguageSetting, "ENG");
        }
    }
}
