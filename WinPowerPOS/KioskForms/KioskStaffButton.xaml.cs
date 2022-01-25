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
    /// Interaction logic for KioskStaffButton.xaml
    /// </summary>
    public partial class KioskStaffButton : UserControl
    {
        public event EventHandler Click;
        public KioskStaffButton()
        {
            InitializeComponent();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Click != null)
                Click(sender, new EventArgs());
        }
    }
}
