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
    /// Interaction logic for KeyCode.xaml
    /// </summary>
    public partial class KeyCode : UserControl
    {
        public delegate void OnClickEventHandler(object sender, EventArgs args);

        public event OnClickEventHandler OnClicked;

        private string barcode;

        public string Barcode { get { return barcode; } set { barcode = value; } }

        public KeyCode()
        {
            InitializeComponent();

            btn0.Click += new RoutedEventHandler(btn0_Click);
            btn1.Click += new RoutedEventHandler(btn1_Click);
            btn2.Click += new RoutedEventHandler(btn2_Click);
            btn3.Click += new RoutedEventHandler(btn3_Click);
            btn4.Click += new RoutedEventHandler(btn4_Click);
            btn5.Click += new RoutedEventHandler(btn5_Click);
            btn6.Click += new RoutedEventHandler(btn6_Click);
            btn7.Click += new RoutedEventHandler(btn7_Click);
            btn8.Click += new RoutedEventHandler(btn8_Click);
            btn9.Click += new RoutedEventHandler(btn9_Click);
            btnBack.Click += new RoutedEventHandler(btnBack_Click);
            btnOk.Click += new RoutedEventHandler(btnOk_Click);

            Clear();
        }

        void btnOk_Click(object sender, RoutedEventArgs e)
        {
            EventArgs args = new EventArgs();

            if (OnClicked != null)
            {
                OnClicked(this, args);
            }   
        }

        void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (barcode.Length > 0)
                barcode = barcode.Substring(0, barcode.Length - 1);

            textBox.Text = barcode;
        }

        void btn9_Click(object sender, RoutedEventArgs e)
        {
            barcode += "9";

            textBox.Text = barcode;
        }

        void btn8_Click(object sender, RoutedEventArgs e)
        {
            barcode += "8";

            textBox.Text = barcode;
        }

        void btn7_Click(object sender, RoutedEventArgs e)
        {
            barcode += "7";

            textBox.Text = barcode;
        }

        void btn6_Click(object sender, RoutedEventArgs e)
        {
            barcode += "6";

            textBox.Text = barcode;
        }

        void btn5_Click(object sender, RoutedEventArgs e)
        {
            barcode += "5";

            textBox.Text = barcode;
        }

        void btn4_Click(object sender, RoutedEventArgs e)
        {
            barcode += "4";

            textBox.Text = barcode;
        }

        void btn3_Click(object sender, RoutedEventArgs e)
        {
            barcode += "3";

            textBox.Text = barcode;
        }

        void btn2_Click(object sender, RoutedEventArgs e)
        {
            barcode += "2";

            textBox.Text = barcode;
        }

        void btn1_Click(object sender, RoutedEventArgs e)
        {
            barcode += "1";

            textBox.Text = barcode;
        }

        void btn0_Click(object sender, RoutedEventArgs e)
        {
            barcode += "0";

            textBox.Text = barcode;
        }

        public void Clear()
        {
            barcode = "";

            textBox.Text = barcode;
        }
    }
}
