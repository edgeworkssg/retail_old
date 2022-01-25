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
    /// Interaction logic for LoginKiosk.xaml
    /// </summary>
    public partial class LoginKiosk : UserControl
    {
        public event EventHandler CancelClick;
        public event EventHandler NRICClick;
        public event EventHandler StaffClick;

        public LoginKiosk()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (CancelClick!=null)
                CancelClick(sender, e);
        }

        private void btnNRIC_Click(object sender, RoutedEventArgs e)
        {
            if (NRICClick != null)
                NRICClick(sender, e);
        }

        private void btnStaff_Click(object sender, RoutedEventArgs e)
        {
            if (StaffClick != null)
                StaffClick(sender, e);
        }
    }
}
