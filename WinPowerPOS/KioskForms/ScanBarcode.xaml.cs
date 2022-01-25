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
using System.Diagnostics;

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for ScanBarcode.xaml
    /// </summary>
    public partial class ScanBarcode : UserControl
    {
        public event EventHandler CancelClick;
        public event KeyEventHandler ControlKeyDown;

        public ScanBarcode()
        {
            InitializeComponent();
        }

        public string Title
        {
            set
            {
                txtTitle.Text = value;
            }
        }

        public string Message
        {
            set
            {
                txtMessage.Text = value;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (CancelClick != null)
                CancelClick(sender, e);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            //if (ControlKeyDown != null)
            //    ControlKeyDown(sender, e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
