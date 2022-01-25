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

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for CircleButton.xaml
    /// </summary>
    public partial class CircleButton : UserControl
    {
        public event EventHandler ControlClick;
        public string ButtonColor
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.Resources["bgColor"] = (SolidColorBrush)(new BrushConverter().ConvertFrom(value));
                }
            }
        }
        public string ButtonHoverColor
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.Resources["bgColorHover"] = (SolidColorBrush)(new BrushConverter().ConvertFrom(value));
                }
            }
        }
        public string ButtonPressColor
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.Resources["bgColorPress"] = (SolidColorBrush)(new BrushConverter().ConvertFrom(value));
                }
            }
        }
        public string ButtonText
        {
            set
            {
                btnCircle.Content = value;
            }
        }

        public CircleButton()
        {
            InitializeComponent();
        }

        private void btnCircle_Click(object sender, RoutedEventArgs e)
        {
            if (ControlClick != null)
                ControlClick(sender, e);
        }
    }
}
