using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WinPowerPOS.KioskForms
{
    public class QuantityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Decimal result = 0;
            Decimal.TryParse(value.ToString(), out result);

            return result.ToString("N0") + "X";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return decimal.Parse(value.ToString());
        }
    }
}
