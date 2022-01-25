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
    /// Interaction logic for SettingLog.xaml
    /// </summary>
    public partial class CheckOut : UserControl
    {

        public event EventHandler CancelClick;
        public event EventHandler AcceptClick;
        public CheckOut()
        {
            InitializeComponent();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            

            //Save Close Counter
            if (AcceptClick != null)
                AcceptClick(sender, e);

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (CancelClick != null)
                CancelClick(sender, e);
        }
    }
}
