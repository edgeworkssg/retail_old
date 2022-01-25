using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public class LabelController
    {
        public static string PointOfSaleText
        {
            get
            {
                string txt = "Point Of Sale";

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.UseMallManagement), false))
                {
                    string theText = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.PointOfSaleText) + "";
                    if (!string.IsNullOrEmpty(theText))
                        txt = theText;
                }

                return txt;
            }
        }

        public static string OutletText
        {
            get
            {
                string txt = "Outlet";

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.UseMallManagement), false))
                {
                    string theText = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.OutletText) + "";
                    if (!string.IsNullOrEmpty(theText))
                        txt = theText;
                }

                return txt;
            }
        }
    }
}
