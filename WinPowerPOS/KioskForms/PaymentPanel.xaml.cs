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
using PowerPOS;

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for PaymentPanel.xaml
    /// </summary>
    public partial class PaymentPanel : UserControl
    {
        public delegate void OnClickEventHandler(object sender, EventArgs args);

        public event OnClickEventHandler OnClicked;
        public event OnClickEventHandler OnCashPaymentClicked;
        public event OnClickEventHandler OnNetsPaymentClicked;
        public event OnClickEventHandler OnNetsFlashPaymentClicked;
        public event OnClickEventHandler OnNetsCashCardPaymentClicked;
        public event OnClickEventHandler OnCreditCardPaymentClicked;

        public PaymentPanel()
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

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.EnableCoins), false) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.EnableNotes), false))
                btnCash.Visibility = Visibility.Visible;
            else
                btnCash.Visibility = Visibility.Hidden;
        }

        private void Clicked(object sender, RoutedEventArgs e)
        {
            EventArgs args = new EventArgs();

            if (OnClicked != null)
            {
                OnClicked(this, args);
            }
        }

        private void CashPaymentClicked(object sender, RoutedEventArgs e)
        {
            EventArgs args = new EventArgs();

            if (OnCashPaymentClicked != null)
            {
                OnCashPaymentClicked(this, args);
            }
        }

        private void NetsPaymentClicked(object sender, RoutedEventArgs e)
        {
            EventArgs args = new EventArgs();

            if (OnNetsPaymentClicked != null)
            {
                OnNetsPaymentClicked(this, args);
            }
        }

        private void NetsFlashPaymentClicked(object sender, RoutedEventArgs e)
        {
            EventArgs args = new EventArgs();

            if (OnNetsFlashPaymentClicked != null)
            {
                OnNetsFlashPaymentClicked(this, args);
            }
        }

        private void NetsCashCardPaymentClicked(object sender, RoutedEventArgs e)
        {
            EventArgs args = new EventArgs();

            if (OnNetsCashCardPaymentClicked != null)
            {
                OnNetsCashCardPaymentClicked(this, args);
            }
        }

        private void CreditCardPaymentClicked(object sender, RoutedEventArgs e)
        {
            EventArgs args = new EventArgs();

            if (OnCreditCardPaymentClicked != null)
            {
                OnCreditCardPaymentClicked(this, args);
            }
        }
    }
}
