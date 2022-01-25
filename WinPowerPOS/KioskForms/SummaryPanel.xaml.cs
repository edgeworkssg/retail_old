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
    /// Interaction logic for SummaryPanel.xaml
    /// </summary>
    public partial class SummaryPanel : UserControl
    {
        public SummaryPanel()
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

        public void UpdateSummary(decimal totalAmount, decimal paidAmount)
        {
            Visibility = Visibility.Visible;


            if (paidAmount < totalAmount)
            {
                lblPaidAmount.Content = "Paid Amount: $" + paidAmount.ToString("N2");
                lblBalanceDue.Content = "Balance Due: $" + totalAmount.ToString("N2");
            }
            else if (paidAmount == totalAmount)
            {
                Visibility = Visibility.Hidden;
            }
            else
            {
                lblPaidAmount.Content = "Paid Amount: $" + paidAmount.ToString("N2");
                lblBalanceDue.Content = "Change: $" + (paidAmount - totalAmount).ToString("N2");
            }
        }

        public void UpdateReturnedCash(decimal totalAmount)
        {
            Visibility = Visibility.Visible;
            lblPaidAmount.Content = "Returned Amount: $" + totalAmount.ToString("N2");
            lblBalanceDue.Content = "";
            /*if (paidAmount < totalAmount)
            {
                lblPaidAmount.Content = "Returned Amount: $" + totalAmount.ToString("N2");
                //lblBalanceDue.Content = "Balance Due: $" + totalAmount.ToString("N2");
            }
            else if (paidAmount == totalAmount)
            {
                Visibility = Visibility.Hidden;
            }
            else
            {
                lblPaidAmount.Content = "Paid Amount: $" + paidAmount.ToString("N2");
                lblBalanceDue.Content = "Change: $" + (paidAmount - totalAmount).ToString("N2");
            }*/
        }
    }
}
