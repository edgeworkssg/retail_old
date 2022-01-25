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
using System.Windows.Interop;

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for RoundedButton.xaml
    /// </summary>
    public partial class FinishAndPayButton : UserControl
    {
        public delegate void OnClickEventHandler(object sender, EventArgs args);

        public event OnClickEventHandler OnClicked;

        public FinishAndPayButton()
        {
            InitializeComponent();

            this.Loaded += delegate
            {
                var source = PresentationSource.FromVisual(this);
                var hwndTarget = source.CompositionTarget as HwndTarget;

                if (hwndTarget != null)
                {
                    hwndTarget.RenderMode = RenderMode.SoftwareOnly;
                }
            };
        }

        private void Clicked(object sender, RoutedEventArgs e)
        {
            EventArgs args = new EventArgs();

            if (OnClicked != null)
            {
                OnClicked(this, args);
            }
        }
    }
}
