using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.Threading;
using LanguageManager = WinPowerPOS.Properties.Language;
using System.Globalization;

namespace WinPowerPOS
{
    public class FormBase: Form
    {
        public FormBase()
        {
            #region Load Culture Code
            string CultureCode =
                  AppSetting.GetSetting(AppSetting.SettingsName.CultureCode);

            if (CultureCode != null)
            {
                try
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureCode);
                }
                catch (Exception ex)
                {
                    Logger.writeLog("Invalid Culture Code Specified in Setting!" + ex.Message);
                    MessageBox.Show(LanguageManager.Invalid_Culture_Code_Specified_in_Setting_ + ex.Message);
                }
            }
            #endregion
        }
    }
}
